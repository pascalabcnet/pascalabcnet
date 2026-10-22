[CmdletBinding()]
param(
    [string]$LegacyRuntimeDirectory = '',
    [string]$ModernRuntimeDirectory = '',
    [ValidateSet('all', 'net472', 'net10')][string]$Target = 'all',
    [switch]$StandardModules
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
if (-not $LegacyRuntimeDirectory) { $LegacyRuntimeDirectory = Join-Path $repositoryRoot 'bin' }
if (-not $ModernRuntimeDirectory) { $ModernRuntimeDirectory = Join-Path $repositoryRoot 'bin-net10' }
$stage = Join-Path $repositoryRoot ('.codex-build\pcu-only-tests\case with spaces\' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $stage -Force | Out-Null

function Check([bool]$Condition, [string]$Message) {
    if (-not $Condition) { throw $Message }
    Write-Host "PASS $Message"
}
function Invoke-Frontend($Runtime, [string]$Frontend, [string[]]$InvocationArguments) {
    # The old console frontend requires this existing flag when its output
    # is redirected by an automated test (there is no real console handle).
    if ($Frontend -eq 'pabcnetc' -and $InvocationArguments -notcontains '/noconsole') {
        $InvocationArguments = @($InvocationArguments) + @('/noconsole')
    }
    if ($Runtime.Modern) {
        $output = @(& dotnet (Join-Path $Runtime.Path "$Frontend.dll") @InvocationArguments 2>&1)
    }
    else {
        $output = @(& (Join-Path $Runtime.Path "$Frontend.exe") @InvocationArguments 2>&1)
    }
    $exitCode = $LASTEXITCODE
    return @{ ExitCode = $exitCode; Text = ($output -join [Environment]::NewLine) }
}
function Check-NoBinary([string]$Directory, [string]$BootstrapName = '') {
    $generated = @(Get-ChildItem -LiteralPath $Directory -Recurse -File |
        Where-Object { $_.Extension -in @('.exe', '.dll', '.pdb', '.config', '.json') -and
            ($BootstrapName -eq '' -or $_.BaseName -eq $BootstrapName -or $_.Name.StartsWith($BootstrapName + '.')) })
    Check ($generated.Count -eq 0) 'PCU-only creates no EXE, DLL, PDB or executable sidecars'
}
function Check-Compilation($Result, [string]$Message) {
    if ($Result.ExitCode -ne 0) { throw "$Message`n$($Result.Text)" }
    Check $true $Message
}

$runtimes = @(
    @{ Name = 'net472'; Path = [IO.Path]::GetFullPath($LegacyRuntimeDirectory); Modern = $false },
    @{ Name = 'net10'; Path = [IO.Path]::GetFullPath($ModernRuntimeDirectory); Modern = $true }
)
foreach ($runtime in $runtimes) {
    if ($Target -ne 'all' -and $Target -ne $runtime.Name) { continue }
    $isolatedRoot = [IO.Path]::GetFullPath((Join-Path $repositoryRoot '.codex-build')) + '\'
    if (-not $runtime.Path.StartsWith($isolatedRoot, [StringComparison]::OrdinalIgnoreCase)) {
        # /rebuild must not rewrite PCUs in the working compiler or installation.
        $copy = Join-Path $stage $(if ($runtime.Modern) { 'runtime\bin-net10' } else { 'runtime\bin' })
        New-Item -ItemType Directory -Path $copy -Force | Out-Null
        Get-ChildItem -LiteralPath $runtime.Path -File | Where-Object { $_.Extension -ne '.pdb' } | Copy-Item -Destination $copy
        foreach ($directory in @('Lib', 'Lng')) {
            Copy-Item -LiteralPath (Join-Path $runtime.Path $directory) -Destination $copy -Recurse
        }
        $runtime.Path = $copy
    }
    foreach ($frontend in @('pabcnetc', 'pabcnetcclear')) {
        foreach ($placement in @('first', 'middle', 'last', 'nodebug')) {
            $case = Join-Path $stage "$($runtime.Name)\$frontend\$placement"
            $outputDir = Join-Path $case 'output with spaces'
            New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
            Get-ChildItem -LiteralPath $PSScriptRoot -File -Filter '*.pas' | Copy-Item -Destination $case
            $arguments = New-Object 'Collections.Generic.List[string]'
            if ($frontend -eq 'pabcnetc') {
                $arguments.AddRange([string[]]@('pcu_only_bootstrap.pas', '/rebuild', '/noconsole'))
                if ($placement -eq 'nodebug') { $arguments[1] = '/rebuildnodebug' }
            }
            else {
                $arguments.AddRange([string[]]@('/Debug:1', "/SearchDir:$case",
                    "/Output:$(Join-Path $outputDir 'custom.exe')", 'pcu_only_bootstrap.pas', $outputDir))
            }
            switch ($placement) {
                'first' { $arguments.Insert(0, '/pcuonly') }
                'middle' { $arguments.Insert(1, '/pcuonly') }
                'last' { $arguments.Add('/PCUONLY'); $arguments.Add('/pcuonly') }
                'nodebug' { $arguments.Add('/pcuonly') }
            }
            Push-Location $case
            try {
                $result = Invoke-Frontend $runtime $frontend $arguments.ToArray()
                Check-Compilation $result "$($runtime.Name) $frontend /pcuonly $placement accepts old argument positions"
                Check-NoBinary $case
                foreach ($unit in @('u_pcu_only_leaf', 'u_pcu_only_middle')) {
                    $pcu = Join-Path $case "$unit.pcu"
                    Check ((Test-Path -LiteralPath $pcu) -and (Get-Item -LiteralPath $pcu).Length -gt 0) "saved complete $unit.pcu"
                    Move-Item -LiteralPath (Join-Path $case "$unit.pas") -Destination (Join-Path $case "$unit.pas.hidden")
                }
                if ($frontend -eq 'pabcnetc') {
                    Check ($result.Text -notmatch 'SemanticTreeConversion |CodeGeneration ') 'PCU-only skips optimizer and code-generation states'
                }

                # Ordinary compilation must still emit a runnable EXE, using
                # only the saved PCUs (including generic and lambda bodies).
                $normalArgs = if ($frontend -eq 'pabcnetc') {
                    @('pcu_only_bootstrap.pas', '/noconsole')
                } else {
                    @('/Debug:1', "/Output:$(Join-Path $outputDir 'custom.exe')", 'pcu_only_bootstrap.pas', $outputDir)
                }
                $result = Invoke-Frontend $runtime $frontend $normalArgs
                Check-Compilation $result 'ordinary compilation consumes PCUs without sources'
                $executable = if ($frontend -eq 'pabcnetc') { Join-Path $case 'pcu_only_bootstrap.exe' } else { Join-Path $outputDir 'custom.exe' }
                Check (Test-Path -LiteralPath $executable) 'ordinary compilation still generates the requested EXE'
                if ($runtime.Modern) { & dotnet $executable } else { & $executable }
                Check ($LASTEXITCODE -eq 0) 'generic and lambda code from PCUs executes correctly'
                $before = [Convert]::ToBase64String([IO.File]::ReadAllBytes($executable))
                $result = Invoke-Frontend $runtime $frontend @('pcu_only_bootstrap.pas', '/pcuonly')
                Check ($result.ExitCode -eq 0 -and $before -eq [Convert]::ToBase64String([IO.File]::ReadAllBytes($executable))) 'PCU-only leaves an existing EXE untouched'

                $result = Invoke-Frontend $runtime $frontend @('invalid_bootstrap.pas', '/pcuonly')
                Check ($result.ExitCode -ne 0) 'PCU-only still rejects semantic errors without waiting for keyboard input'
                $result = Invoke-Frontend $runtime $frontend @('pcu_only_library.pas', '/pcuonly')
                Check-Compilation $result 'PCU-only accepts library source without emitting DLL'
                Check (-not (Test-Path -LiteralPath (Join-Path $case 'pcu_only_library.dll'))) 'PCU-only emits no library DLL'
                $result = Invoke-Frontend $runtime $frontend @('pcu_only_library.pas')
                Check-Compilation $result 'ordinary library compilation still works'
                $library = Join-Path $case 'pcu_only_library.dll'
                Check ((Test-Path -LiteralPath $library) -and
                    [Reflection.AssemblyName]::GetAssemblyName($library).Name -eq 'pcu_only_library') 'ordinary compilation still emits a valid managed DLL'
            }
            finally { Pop-Location }
        }
    }
    if ($StandardModules) {
        # The rebuild flag changes standard PCUs. Require an isolated runtime,
        # not the developer bin or an installed copy under Program Files.
        $isolatedRoot = [IO.Path]::GetFullPath((Join-Path $repositoryRoot '.codex-build')) + '\'
        Check ($runtime.Path.StartsWith($isolatedRoot, [StringComparison]::OrdinalIgnoreCase)) 'full standard-module test uses an isolated runtime'
        $bootstrapNames = if ($runtime.Modern) { @('RebuildStandartModulesNet10') } else { @('RebuildStandartModules', 'RebuildStandartModulesSPython') }
        foreach ($name in $bootstrapNames) {
            $case = Join-Path $stage "$($runtime.Name)\standard-modules\$name"
            New-Item -ItemType Directory -Path $case -Force | Out-Null
            $source = Join-Path $repositoryRoot "ReleaseGenerators\$name.pas"
            Copy-Item -LiteralPath $source -Destination $case
            Push-Location $case
            try {
                $timer = [Diagnostics.Stopwatch]::StartNew()
                $result = Invoke-Frontend $runtime 'pabcnetc' @("$name.pas", '/rebuild', '/pcuonly', '/noconsole')
                Check-Compilation $result "real $($runtime.Name) standard modules compile without EXE"
                # Referenced DLLs may be copied (e.g. nunit.framework.dll),
                # but no binary or sidecar of the bootstrap may be generated.
                Check-NoBinary $case $name
                Check ($result.Text -notmatch 'SemanticTreeConversion |CodeGeneration ') 'standard bootstrap skips optimizer and code generation'
                $bootstrap = [regex]::Replace([IO.File]::ReadAllText($source), '//[^\r\n]*', '')
                $units = [regex]::Match($bootstrap, '(?is)\buses\s+([^;]+);')
                Check $units.Success 'standard bootstrap has a readable unit list'
                foreach ($unit in $units.Groups[1].Value.Split(',')) {
                    $module = $unit.Trim()
                    $pcus = @(Get-ChildItem -LiteralPath (Join-Path $runtime.Path 'Lib') -Recurse -File -Filter "$module.pcu")
                    Check (@($pcus | Where-Object { $_.Length -gt 0 }).Count -gt 0) "standard PCU exists: $module"
                }
                Write-Host "STANDARD_MODULES $($runtime.Name) $name $($timer.Elapsed.TotalSeconds.ToString('F2')) seconds"
            }
            finally { Pop-Location }
        }
    }
}
Write-Host "All PCU-only checks passed. Fixtures: $stage"
