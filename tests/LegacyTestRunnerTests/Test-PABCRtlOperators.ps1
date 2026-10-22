[CmdletBinding()]
param([string]$LegacyRuntimeDirectory = '')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
if (-not $LegacyRuntimeDirectory) { $LegacyRuntimeDirectory = Join-Path $repositoryRoot 'bin' }
$LegacyRuntimeDirectory = [IO.Path]::GetFullPath($LegacyRuntimeDirectory)
$stage = Join-Path $repositoryRoot ('.codex-build\pabcrtl-operator-tests\' + [Guid]::NewGuid().ToString('N'))
$runtime = Join-Path $stage 'bin'
New-Item -ItemType Directory -Path $runtime -Force | Out-Null
Get-ChildItem -LiteralPath $LegacyRuntimeDirectory -File |
    Where-Object { $_.Extension -in @('.dll', '.exe', '.config', '.ini') } |
    Copy-Item -Destination $runtime
foreach ($directory in @('Lib', 'Lng')) {
    Copy-Item -LiteralPath (Join-Path $LegacyRuntimeDirectory $directory) -Destination $runtime -Recurse
}

# Rebuild the real runner in isolation. Never change the working runtime, PCUs,
# PABCRtl.dll or GAC. Each invocation below starts a fresh compiler process.
& (Join-Path $LegacyRuntimeDirectory 'pabcnetcclear.exe') (Join-Path $repositoryRoot 'bin\TestRunner.pas') $runtime
if ($LASTEXITCODE -ne 0) { throw 'Cannot compile isolated TestRunner.' }
$runner = Join-Path $runtime 'TestRunner.exe'

function Invoke-Pass([string]$Pass) {
    # Windows PowerShell turns native stderr into errors. Capture the complete
    # compiler diagnostic before checking the actual process exit code.
    $savedPreference = $ErrorActionPreference
    try {
        $ErrorActionPreference = 'Continue'
        $output = @(& $runner $Pass '1' 2>&1)
        $exitCode = $LASTEXITCODE
    }
    finally { $ErrorActionPreference = $savedPreference }
    if ($exitCode -ne 0) { throw ($output -join [Environment]::NewLine) }
    if (($output -join ' ') -match '32bit mode') { throw 'The full x86 pass must remain disabled.' }
}

foreach ($name in @('extensionmethods26', 'extension_interface_operators', 'overloads7', 'SliceArrGeneric')) {
    $suite = Join-Path $stage $name
    New-Item -ItemType Directory -Path $suite -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $repositoryRoot "TestSuite\$name.pas") -Destination $suite
    Copy-Item -LiteralPath (Join-Path $repositoryRoot 'TestSuite\testsettings.config') -Destination $suite
    Push-Location $suite
    try {
        # A DLL-only compile must not rely on PCUs loaded by an earlier pass.
        # Hide both system PCUs in this copy, leaving their sources and RTL intact.
        foreach ($unit in @('PABCSystem', 'PABCExtensions')) {
            $pcu = Join-Path $runtime "Lib\$unit.pcu"
            Move-Item -LiteralPath $pcu -Destination "$pcu.hidden"
        }
        try {
            Invoke-Pass '5'
            $binary = Join-Path $suite "exe\$name.exe"
            # Reflection-only loads keep their files open in this process.
            # Inspect a separate copy so pass 4 can delete and pass 1 overwrite EXE.
            $referenceProbe = Join-Path $stage "$name.referencecheck.exe"
            Copy-Item -LiteralPath $binary -Destination $referenceProbe
            $references = [Reflection.Assembly]::ReflectionOnlyLoadFrom($referenceProbe).GetReferencedAssemblies()
            if (@($references | Where-Object { $_.Name -eq 'PABCRtl' }).Count -ne 1) {
                throw "$name silently fell back to standalone PCUs."
            }
            Invoke-Pass '4'
            Write-Host "PASS ${name}: cold PABCRtl compile and execution, without system PCUs or x86 warm-up"
        }
        finally {
            foreach ($unit in @('PABCSystem', 'PABCExtensions')) {
                $pcu = Join-Path $runtime "Lib\$unit.pcu"
                Move-Item -LiteralPath "$pcu.hidden" -Destination $pcu
            }
        }
        Invoke-Pass '1'
        Invoke-Pass '4'
        Write-Host "PASS ${name}: ordinary PCU compile and execution"
    }
    finally { Pop-Location }
}

# This must use ONE compiler process. Independent runs cannot detect a member
# table built without RTL and then reused after RTL is referenced again.
$switchSuite = Join-Path $stage 'switch-suite'
New-Item -ItemType Directory -Path $switchSuite -Force | Out-Null
foreach ($name in @('beginend', 'arrays4', 'arrays4u', 'foreach9')) {
    Copy-Item -LiteralPath (Join-Path $repositoryRoot "TestSuite\$name.pas") -Destination $switchSuite
}
$helperSource = Join-Path $stage 'helper-source'
New-Item -ItemType Directory -Path $helperSource -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'PABCRtlSwitchTests.pas') -Destination $helperSource
foreach ($name in @('Compiler', 'CompilerTools', 'Errors', 'LanguageIntegrator', 'TreeConverter')) {
    Copy-Item -LiteralPath (Join-Path $LegacyRuntimeDirectory "$name.dll") -Destination $helperSource
}
& (Join-Path $LegacyRuntimeDirectory 'pabcnetcclear.exe') (Join-Path $helperSource 'PABCRtlSwitchTests.pas') $runtime
if ($LASTEXITCODE -ne 0) { throw 'Cannot compile RTL/PCU switching test.' }
& (Join-Path $runtime 'PABCRtlSwitchTests.exe') $switchSuite
if ($LASTEXITCODE -ne 0) { throw 'RTL/PCU switching test failed.' }
foreach ($binary in Get-ChildItem -LiteralPath (Join-Path $switchSuite 'exe') -Filter '*.exe') {
    & $binary.FullName
    if ($LASTEXITCODE -ne 0) { throw "$($binary.Name) failed after RTL/PCU switching." }
}
Write-Host 'PASS executables generated after RTL/PCU switching run successfully'
Write-Host "Fixtures: $stage"
