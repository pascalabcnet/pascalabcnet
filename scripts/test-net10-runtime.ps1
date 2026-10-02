[CmdletBinding()]
param([switch]$SkipBuild)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
if (-not $SkipBuild) { & (Join-Path $PSScriptRoot 'build-net10-runtime.ps1') }
$source = Join-Path $repositoryRoot 'bin-net10'
$destination = Join-Path $repositoryRoot ('.codex-build\runtime-export-tests\' + [Guid]::NewGuid().ToString('N'))
& (Join-Path $PSScriptRoot 'export-net10-runtime.ps1') -Destination $destination
$manifest = Get-Content -LiteralPath (Join-Path $source 'net10-runtime.manifest.json') -Raw | ConvertFrom-Json
foreach ($file in $manifest.files) {
    if ((Get-FileHash -LiteralPath (Join-Path $destination $file.path) -Algorithm SHA256).Hash -ne $file.sha256) {
        throw "Exported file differs: $($file.path)"
    }
}
$exported = @(Get-ChildItem -LiteralPath $destination -Recurse -File)
if ($exported.Count -ne $manifest.files.Count + 1) { throw 'Export contains unlisted files.' }
foreach ($file in $exported) {
    if ($file.Extension -in @('.pdb', '.pcu') -or $file.Name -in @('NetMQ.dll', 'AsyncIO.dll', 'NaCl.dll')) {
        throw "Unexpected distribution file: $($file.FullName)"
    }
}
foreach ($entry in @('pabcnetc', 'pabcnetcclear', 'PABCCompilerController', 'ZMQServerPas')) {
    foreach ($suffix in @('.dll', '.deps.json', '.runtimeconfig.json')) {
        if (-not (Test-Path -LiteralPath (Join-Path $destination ($entry + $suffix)))) {
            throw "Missing exported entry: $entry$suffix"
        }
    }
}
Write-Host "PASS unified kit: all four entrypoints, identical hashes, manifest-only export, no PDB/legacy PCU/transport dependencies ($($manifest.files.Count) files)."
