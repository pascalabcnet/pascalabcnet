[CmdletBinding()]
param([switch]$SkipRunnerBuild)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$compiler = Join-Path $repositoryRoot 'bin\pabcnetcclear.exe'
$runner = Join-Path $repositoryRoot 'bin\TestRunner.exe'
$stage = Join-Path $repositoryRoot ('.codex-build\legacy-runner-spython-tests\' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $stage -Force | Out-Null
if (-not $SkipRunnerBuild) {
    if (Test-Path -LiteralPath $runner) {
        Copy-Item -LiteralPath $runner -Destination (Join-Path $stage 'TestRunner-before.exe')
    }
    Push-Location (Join-Path $repositoryRoot 'bin')
    try {
        & $compiler 'TestRunner.pas'
        if ($LASTEXITCODE -ne 0) { throw 'Full legacy TestRunner rebuild failed.' }
    }
    finally { Pop-Location }
}

$suite = Join-Path $stage 'suite'
Copy-Item -LiteralPath (Join-Path $repositoryRoot 'TestSuiteAdditionalLanguages\SPythonTests') -Destination $suite -Recurse
if (Test-Path -LiteralPath (Join-Path $suite 'referencefixtures')) {
    throw 'SPython regression must use a suite without Pascal reference fixtures.'
}
Push-Location $suite
try {
    foreach ($pass in 1..6) {
        Write-Host "SPython pass $pass"
        & $runner "$pass" '1'
        if ($LASTEXITCODE -ne 0) { throw "Legacy SPython pass $pass failed." }
    }
}
finally { Pop-Location }
foreach ($directory in @('referencefixtures', 'units\reference fixtures', 'usesunits\reference fixtures')) {
    if (Test-Path -LiteralPath (Join-Path $suite $directory)) {
        throw "Pascal fixture directory unexpectedly created in SPython suite: $directory"
    }
}
Write-Host "PASS real legacy runner: all six SPython passes without Pascal reference fixtures. Fixtures: $stage"
