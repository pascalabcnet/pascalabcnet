[CmdletBinding()]
param([ValidateSet('Debug', 'Release')][string]$Configuration = 'Release')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$destination = Join-Path $repositoryRoot 'bin-net10'
# Build in a fresh directory: never distribute leftovers from a developer's bin.
$stageParent = Join-Path $repositoryRoot '.codex-build\net10-runtime'
$stage = Join-Path $stageParent ([Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $stage -Force | Out-Null
& dotnet build (Join-Path $repositoryRoot 'Net10Runtime.slnx') `
    --configuration $Configuration -p:TargetFramework=net10.0 `
    "-p:OutputPath=$stage\" -p:AppendTargetFrameworkToOutputPath=false `
    -p:SatelliteResourceLanguages=ru --disable-build-servers -m:1 --nologo
if ($LASTEXITCODE -ne 0) { throw 'Unified .NET 10 runtime build failed.' }

# The console projects also copy local bin assets. Replace these generated copies
# with versioned runtime sources, excluding legacy binaries and compiled units.
foreach ($directory in @('Lib', 'Lng')) {
    $generated = [IO.Path]::GetFullPath((Join-Path $stage $directory))
    if (-not $generated.StartsWith($stage + '\', [StringComparison]::OrdinalIgnoreCase)) {
        throw "Unsafe staging path: $generated"
    }
    if (Test-Path -LiteralPath $generated) { Remove-Item -LiteralPath $generated -Recurse -Force }
}
$assets = @(& git -c core.quotepath=false -C $repositoryRoot ls-files -- bin/Lib bin/Lng)
if ($LASTEXITCODE -ne 0) { throw 'Cannot list versioned runtime assets.' }
foreach ($asset in $assets) {
    if ([IO.Path]::GetExtension($asset) -in @('.pcu', '.dll', '.exe', '.pdb')) { continue }
    $target = Join-Path $stage $asset.Substring(4)
    New-Item -ItemType Directory -Path (Split-Path -Parent $target) -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $repositoryRoot $asset) -Destination $target -Force
}
Get-ChildItem -LiteralPath $stage -Recurse -File -Filter '*.pdb' | Remove-Item -Force
foreach ($entry in @('pabcnetc', 'pabcnetcclear', 'PABCCompilerController', 'PABCCompilerWorker')) {
    foreach ($suffix in @('.dll', '.deps.json', '.runtimeconfig.json')) {
        if (-not (Test-Path -LiteralPath (Join-Path $stage ($entry + $suffix)))) {
            throw "Missing runtime entry: $entry$suffix"
        }
    }
}
foreach ($forbidden in @('NetMQ.dll', 'AsyncIO.dll', 'NaCl.dll')) {
    if (@(Get-ChildItem -LiteralPath $stage -Recurse -File -Filter $forbidden).Count) {
        throw "Unexpected transport dependency: $forbidden"
    }
}
$files = @(Get-ChildItem -LiteralPath $stage -Recurse -File | ForEach-Object {
    [ordered]@{ path = $_.FullName.Substring($stage.Length + 1); sha256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash }
})
New-Item -ItemType Directory -Path $destination -Force | Out-Null
foreach ($file in $files) {
    $target = Join-Path $destination $file.path
    if ($file.path.StartsWith('Lib\') -and [IO.Path]::GetExtension($target) -eq '.pas') {
        # Standard PCUs are compiler caches, not user sources. A unified rebuild
        # must not reuse units produced by an earlier compiler/source snapshot.
        $pcu = [IO.Path]::ChangeExtension($target, '.pcu')
        if (Test-Path -LiteralPath $pcu) { Remove-Item -LiteralPath $pcu -Force }
    }
    New-Item -ItemType Directory -Path (Split-Path -Parent $target) -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $stage $file.path) -Destination $target -Force
}
# Do not delete unrelated developer files from bin-net10. The manifest defines
# the authoritative kit and prevents those files entering exported packages.
$manifest = [ordered]@{ configuration = $Configuration; files = $files }
$manifest | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $destination 'net10-runtime.manifest.json') -Encoding UTF8
Write-Host "Unified .NET 10 runtime: $destination"
