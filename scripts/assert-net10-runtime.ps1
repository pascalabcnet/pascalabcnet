[CmdletBinding()]
param(
    [string]$RuntimeDirectory = '',
    [ValidateSet('Debug', 'Release')][string]$Configuration,
    [switch]$RequireCompiledUnits
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-RuntimeHash([string]$Path) {
    # Do not depend on PowerShell module auto-loading when invoked from cmd/NSIS.
    $stream = [IO.File]::OpenRead($Path)
    $sha = [Security.Cryptography.SHA256]::Create()
    try { return ([BitConverter]::ToString($sha.ComputeHash($stream))).Replace('-', '') }
    finally { $sha.Dispose(); $stream.Dispose() }
}

$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
if (-not $RuntimeDirectory) { $RuntimeDirectory = Join-Path $repositoryRoot 'bin-net10' }
$RuntimeDirectory = [IO.Path]::GetFullPath($RuntimeDirectory)
$manifest = Get-Content -LiteralPath (Join-Path $RuntimeDirectory 'net10-runtime.manifest.json') -Raw -Encoding UTF8 | ConvertFrom-Json
if ($Configuration -and $manifest.configuration -ne $Configuration) {
    throw "Expected $Configuration runtime, found $($manifest.configuration). Prepare it first."
}
$listed = @{}
foreach ($file in $manifest.files) {
    $path = [IO.Path]::GetFullPath((Join-Path $RuntimeDirectory $file.path))
    if (-not $path.StartsWith($RuntimeDirectory + '\', [StringComparison]::OrdinalIgnoreCase) -or $listed.ContainsKey($file.path)) {
        throw "Invalid runtime manifest path: $($file.path)"
    }
    $listed[$file.path] = $true
    if ((Get-RuntimeHash $path) -ne $file.sha256) {
        throw "Runtime changed after preparation: $path. Prepare it again."
    }
}
foreach ($entry in @('pabcnetc', 'pabcnetcclear', 'PABCCompilerController', 'PABCCompilerWorker', 'PascalABCNet.LanguageServer')) {
    foreach ($suffix in @('.dll', '.deps.json', '.runtimeconfig.json')) {
        if (-not $listed.ContainsKey($entry + $suffix)) { throw "Missing runtime entry: $entry$suffix" }
    }
}
foreach ($library in @('Compiler.dll', 'TreeConverter.dll', 'NETGenerator.dll', 'CodeCompletion.dll', 'PascalABCNet.LanguageServices.dll')) {
    if (-not $listed.ContainsKey($library)) { throw "Missing runtime library: $library" }
}
if ($RequireCompiledUnits) {
    # The bootstrap source, not a second hard-coded list, defines the required base PCUs.
    $bootstrap = Get-Content -LiteralPath (Join-Path $repositoryRoot 'ReleaseGenerators\RebuildStandartModulesNet10.pas') -Raw -Encoding UTF8
    $uses = [regex]::Match($bootstrap, '(?is)\buses\s+([^;]+);')
    if (-not $uses.Success) { throw 'Cannot read the modern standard-unit bootstrap.' }
    foreach ($module in $uses.Groups[1].Value.Split(',')) {
        $name = $module.Trim()
        if ($name -notmatch '^\w+$' -or
            (-not $listed.ContainsKey("Lib\$name.pas") -and -not $listed.ContainsKey("LibSource\$name.pas"))) {
            throw "Invalid or unlisted standard module: $name"
        }
        $pcu = Join-Path $RuntimeDirectory "Lib\$name.pcu"
        if (-not (Test-Path -LiteralPath $pcu -PathType Leaf) -or (Get-Item -LiteralPath $pcu).Length -eq 0) {
            throw "Missing standard PCU: $pcu. Rebuild the modern standard modules first."
        }
    }
}
Write-Host "Prepared .NET 10 runtime verified: $RuntimeDirectory"
