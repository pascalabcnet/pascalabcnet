[CmdletBinding()]
param([string]$NsisCompiler = '')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
if (-not $NsisCompiler) { $NsisCompiler = Join-Path $repositoryRoot 'Utils\NSIS2\Bin\makensis.exe' }
$stage = Join-Path $repositoryRoot ('.codex-build\installer-checker-tests\' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $stage -Force | Out-Null
$output = Join-Path $stage 'Net10RuntimeCheckerTests.exe'
Push-Location $PSScriptRoot
try {
    & $NsisCompiler /V3 "/DTEST_OUTPUT=$output" 'Net10RuntimeCheckerTests.nsi'
    if ($LASTEXITCODE -ne 0) { throw "NSIS test compilation failed: $LASTEXITCODE" }
}
finally { Pop-Location }
# This is the headless test harness, never the PascalABC.NET product installer.
$process = Start-Process -FilePath $output -ArgumentList '/S' -WindowStyle Hidden -Wait -PassThru
$results = Join-Path $stage 'runtime-checker-results.txt'
if (-not (Test-Path -LiteralPath $results)) { throw 'Runtime checker did not create a test log.' }
Get-Content -LiteralPath $results
if ($process.ExitCode -ne 0) { throw "Runtime checker tests failed: $results" }
if ([IO.File]::ReadAllText($results) -notmatch 'Failures: 0') { throw 'Runtime checker reported failures.' }
Write-Host "PASS NSIS runtime detection tests: $stage"
