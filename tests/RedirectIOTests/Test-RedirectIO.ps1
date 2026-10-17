[CmdletBinding()]
param(
    [ValidateSet('all', 'net472', 'net10.0')][string]$Target = 'all',
    [switch]$IncludeClosedXml
)
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../..'))
$root = Join-Path $repositoryRoot ('.codex-build/redirect-io-tests/case with spaces/' + [Guid]::NewGuid().ToString('N'))
$targets = if ($Target -eq 'all') { @('net472', 'net10.0') } else { @($Target) }
foreach ($framework in $targets) {
    $work = Join-Path $root $framework
    New-Item -ItemType Directory -Path $work -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $repositoryRoot 'bin/Lib/__RedirectIOMode.pas') -Destination $work
    $modern = $framework -eq 'net10.0'
    $compiler = if ($modern) { Join-Path $repositoryRoot 'bin-net10/pabcnetcclear.dll' } else { Join-Path $repositoryRoot 'bin/pabcnetcclear.exe' }
    $names = @('console', 'winforms', 'unhandled')
    if ($modern -and $IncludeClosedXml) { $names += 'closedxml_smoke' }
    foreach ($name in $names) {
        $source = Join-Path $work "$name.pas"
        Copy-Item -LiteralPath (Join-Path $PSScriptRoot "$name.pas") -Destination $source
        $ErrorActionPreference = 'Continue'
        $output = if ($modern) { & dotnet $compiler $source 2>&1 } else { & $compiler $source 2>&1 }
        $code = $LASTEXITCODE
        $ErrorActionPreference = 'Stop'
        if ($code -ne 0) { throw "$framework $name compilation failed: $output" }
        if ($modern -and $name -eq 'winforms') {
            # Desktop targeting is not yet emitted by the console compiler.
            # Provide the real shared framework for this focused event-subscription test.
            $configPath = Join-Path $work "$name.runtimeconfig.json"
            $config = Get-Content -LiteralPath $configPath -Raw | ConvertFrom-Json
            $config.runtimeOptions.PSObject.Properties.Remove('framework')
            $config.runtimeOptions | Add-Member -Force -NotePropertyName frameworks -NotePropertyValue @(
                @{ name = 'Microsoft.NETCore.App'; version = '10.0.0' },
                @{ name = 'Microsoft.WindowsDesktop.App'; version = '10.0.0' }
            )
            [IO.File]::WriteAllText($configPath, ($config | ConvertTo-Json -Depth 10))
        }
        if ($modern -and $name -ne 'winforms') {
            $config = Get-Content -LiteralPath (Join-Path $work "$name.runtimeconfig.json") -Raw | ConvertFrom-Json
            $frameworkNames = @($config.runtimeOptions.framework.name) + @($config.runtimeOptions.frameworks | ForEach-Object { $_.name })
            if ($frameworkNames -contains 'Microsoft.WindowsDesktop.App') { throw "$name acquired an unwanted WindowsDesktop dependency" }
        }
        $exe = Join-Path $work "$name.exe"
        $start = New-Object System.Diagnostics.ProcessStartInfo
        $start.FileName = if ($modern) { 'dotnet' } else { $exe }
        $start.Arguments = if ($modern) { '"' + $exe + '" [REDIRECTIOMODE]' } else { '[REDIRECTIOMODE]' }
        $start.WorkingDirectory = $work
        $start.UseShellExecute = $false
        $start.CreateNoWindow = $true
        $start.WindowStyle = 'Hidden'
        $start.RedirectStandardInput = $true
        $start.RedirectStandardOutput = $true
        $start.RedirectStandardError = $true
        $process = [Diagnostics.Process]::Start($start)
        try {
            $stdout = $process.StandardOutput.ReadToEndAsync()
            $stderr = $process.StandardError.ReadToEndAsync()
            $process.StandardInput.WriteLine('GO')
            $process.StandardInput.Close()
            if (!$process.WaitForExit(30000)) { $process.Kill(); throw "$framework $name timed out" }
            $outText = $stdout.GetAwaiter().GetResult()
            $errText = $stderr.GetAwaiter().GetResult()
            if ($name -eq 'unhandled') {
                if ($process.ExitCode -eq 0 -or $errText -notmatch '\[EXCEPTION\].*redirect-unhandled-test') { throw "Unhandled-exception protocol missing: $errText" }
            } else {
                if ($process.ExitCode -ne 0 -or $outText -notmatch "redirect-$name-ok") { throw "$framework $name failed: $outText $errText" }
                if ($name -eq 'winforms' -and $errText -notmatch '\[EXCEPTION\].*redirect-winforms-test') { throw "WinForms exception was not forwarded: $errText" }
                if ($name -eq 'closedxml_smoke' -and !(Test-Path -LiteralPath (Join-Path $work 'redirect-test.xlsx'))) { throw 'Excel file missing' }
            }
            Write-Host "PASS $framework $name"
        } finally {
            $process.Dispose()
        }
    }
}
