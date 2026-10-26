[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repo = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$sources = @('TestSuite\l2.dll', 'TestSuite\foreach_resolver_lib.dll',
    'TestSuite\usesunits\UsesInUnits3\uui3_lib.dll')
$hashes = @{}
foreach ($relative in $sources) {
    $hashes[$relative] = (Get-FileHash -LiteralPath (Join-Path $repo $relative)).Hash
}

Push-Location $repo
try {
    foreach ($target in @('net472', 'net10')) {
        foreach ($case in @(
            @('core', 'overloads11', 'l2.dll'),
            @('core', 'foreach_resolver1', 'foreach_resolver_lib.dll'),
            @('units', 'uses_in3_derectives', 'usesunits\UsesInUnits3\uui3_lib.dll')
        )) {
            $started = [DateTime]::UtcNow
            # The batch builds the runner itself for the selected target too.
            & cmd /c _RunCrossTargetTests.bat $target $case[0] $case[1] --skip-build
            if ($LASTEXITCODE -ne 0) { throw "$target $($case[1]) failed: $LASTEXITCODE" }
            $dll = Join-Path $repo ("TestSuite\TestResults\$target\work\" + $case[2])
            if (-not (Test-Path -LiteralPath $dll)) { throw "Missing rebuilt library: $dll" }
            if ((Get-Item -LiteralPath $dll).LastWriteTimeUtc -lt $started.AddSeconds(-2)) {
                throw "Library was not rebuilt during this run: $dll"
            }
            Write-Host "PASS $target $($case[1]): freshly compiled test DLL"
        }
    }
    foreach ($relative in $sources) {
        if ((Get-FileHash -LiteralPath (Join-Path $repo $relative)).Hash -ne $hashes[$relative]) {
            throw "Checked-in library was modified: $relative"
        }
    }
    Write-Host 'PASS checked-in libraries are unchanged'
}
finally { Pop-Location }
