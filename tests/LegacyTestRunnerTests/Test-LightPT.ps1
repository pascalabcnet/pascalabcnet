[CmdletBinding()]
param([string]$ModernRuntimeDirectory = '')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$runner = Join-Path $repositoryRoot 'bin\TestRunner.exe'
if (-not $ModernRuntimeDirectory) { $ModernRuntimeDirectory = Join-Path $repositoryRoot 'bin-net10' }
$ModernRuntimeDirectory = [IO.Path]::GetFullPath($ModernRuntimeDirectory)
$source = Join-Path $repositoryRoot 'TestSuite\lightpt_smoke.pas'
if (-not [IO.File]::ReadAllText($source).StartsWith('//nopabcrtl')) {
    throw 'The standalone LightPT test must not require a LightPT unit inside PABCRtl.dll.'
}
$suite = Join-Path $repositoryRoot ('.codex-build\legacy-lightpt-tests\' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $suite -Force | Out-Null
Copy-Item -LiteralPath $source -Destination $suite
Copy-Item -LiteralPath (Join-Path $repositoryRoot 'TestSuite\tasks_standard_smoke.pas') -Destination $suite
Copy-Item -LiteralPath (Join-Path $repositoryRoot 'TestSuite\testsettings.config') -Destination $suite

# The production runner reads the production smoke test unchanged. Pass 5
# honours nopabcrtl for its DLL-only compilation; the full x86 block is disabled.
# No IDE/compiler binaries, GAC registrations or production test outputs change.
Push-Location $suite
try {
    foreach ($pass in @(1, 4, 5)) {
        & $runner "$pass" '1'
        if ($LASTEXITCODE -ne 0) { throw "LightPT legacy runner pass $pass failed." }
    }
    New-Item -ItemType Directory -Path 'modern' -Force | Out-Null
    foreach ($testName in @('lightpt_smoke', 'tasks_standard_smoke')) {
        Copy-Item -LiteralPath (Join-Path $suite "$testName.pas") -Destination 'modern'
        & dotnet (Join-Path $ModernRuntimeDirectory 'pabcnetcclear.dll') (Join-Path $suite "modern\$testName.pas")
        if ($LASTEXITCODE -ne 0) { throw "$testName modern compilation failed." }
        & dotnet (Join-Path $suite "modern\$testName.exe")
        if ($LASTEXITCODE -ne 0) { throw "$testName modern execution failed." }
    }
}
finally { Pop-Location }
Write-Host "PASS LightPT and Tasks: real TestRunner passes 1/4/5 and modern compile/run. Fixtures: $suite"
