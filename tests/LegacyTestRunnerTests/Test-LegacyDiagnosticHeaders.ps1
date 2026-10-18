[CmdletBinding()]
param([switch]$SkipRunnerBuild)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$compiler = Join-Path $repositoryRoot 'bin\pabcnetcclear.exe'
$runner = Join-Path $repositoryRoot 'bin\TestRunner.exe'
$stage = Join-Path $repositoryRoot ('.codex-build\legacy-runner-header-tests\' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $stage -Force | Out-Null

& $compiler (Join-Path $PSScriptRoot 'DiagnosticHeaderTests.pas') $stage
if ($LASTEXITCODE -ne 0) { throw 'Header unit tests failed to compile.' }
& (Join-Path $stage 'DiagnosticHeaderTests.exe')
if ($LASTEXITCODE -ne 0) { throw 'Header unit tests failed.' }
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
New-Item -ItemType Directory -Path (Join-Path $suite 'errors') -Force | Out-Null
foreach ($directory in @('units', 'usesunits')) {
    New-Item -ItemType Directory -Path (Join-Path $suite $directory) -Force | Out-Null
}
# Exercise the same sibling-unit layout as the production package conflict test.
Copy-Item -LiteralPath (Join-Path $repositoryRoot 'TestSuite\units\u_directive_package.pas') -Destination (Join-Path $suite 'units')
Copy-Item -LiteralPath (Join-Path $repositoryRoot 'TestSuite\errors\err_directive_package_unit_conflict.pas') -Destination (Join-Path $suite 'errors')
# Pass 3 always bootstraps the shared reference fixtures, even in a tiny suite.
$references = Join-Path $suite 'referencefixtures'
New-Item -ItemType Directory -Path $references -Force | Out-Null
foreach ($name in @('ReferenceTransitiveLeaf.pas', 'ReferenceTransitiveRoot.pas')) {
    Copy-Item -LiteralPath (Join-Path $repositoryRoot "TestSuite\referencefixtures\$name") -Destination $references
}
[IO.File]::WriteAllText((Join-Path $suite 'testsettings.config'), 'languageName: PascalABC.NET')
$warningSource = [IO.File]::ReadAllText((Join-Path $repositoryRoot 'TestSuite\directive_unknown_warning.pas'))
$errorSource = [IO.File]::ReadAllText((Join-Path $repositoryRoot 'TestSuite\errors\err_directive_empty.pas'))
foreach ($style in @('CRLF', 'LF', 'CR', 'mixed')) {
    foreach ($sample in @(
        @{ Source = $warningSource; Destination = (Join-Path $suite "warning_$style.pas") },
        @{ Source = $errorSource; Destination = (Join-Path $suite "errors\error_$style.pas") }
    )) {
        $lines = [regex]::Split($sample.Source, '\r\n|\r|\n')
        $text = New-Object Text.StringBuilder
        for ($i = 0; $i -lt $lines.Length; $i++) {
            $ending = switch ($style) {
                'CRLF' { "`r`n" }
                'LF' { "`n" }
                'CR' { "`r" }
                'mixed' { if ($i % 2 -eq 0) { "`n" } else { "`r`n" } }
            }
            [void]$text.Append($lines[$i]).Append($ending)
        }
        [IO.File]::WriteAllText($sample.Destination, $text.ToString())
    }
}
Push-Location $suite
try {
    foreach ($pass in @(1, 3)) {
        & $runner "$pass" '1'
        if ($LASTEXITCODE -ne 0) { throw "Legacy diagnostic fixture pass $pass failed." }
    }
}
finally { Pop-Location }
Write-Host "PASS real legacy runner: diagnostic line endings and sibling-unit package conflict. Fixtures: $stage"
