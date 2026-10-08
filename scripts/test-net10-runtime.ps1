[CmdletBinding()]
param([switch]$SkipBuild)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
if (-not $SkipBuild) { & (Join-Path $PSScriptRoot 'build-net10-runtime.ps1') }
$source = Join-Path $repositoryRoot 'bin-net10'
foreach ($ideAsset in @('Highlighting', 'Ico')) {
    if (Test-Path -LiteralPath (Join-Path $source $ideAsset)) {
        throw "IDE assets must not be present in the console runtime: $ideAsset"
    }
}
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
foreach ($entry in @('pabcnetc', 'pabcnetcclear', 'PABCCompilerController', 'PABCCompilerWorker', 'PascalABCNet.LanguageServer')) {
    foreach ($suffix in @('.dll', '.deps.json', '.runtimeconfig.json')) {
        if (-not (Test-Path -LiteralPath (Join-Path $destination ($entry + $suffix)))) {
            throw "Missing exported entry: $entry$suffix"
        }
    }
}
# Compiler assemblies must occur exactly once, at the root of the shared kit.
foreach ($library in @(
    'Compiler.dll', 'CompilerTools.dll', 'Errors.dll', 'TreeConverter.dll',
    'NETGenerator.dll', 'CodeCompletion.dll', 'SyntaxTree.dll', 'SemanticTree.dll',
    'SyntaxVisitors.dll', 'SyntaxTreeConverters.dll', 'ParserTools.dll',
    'PascalABCParser.dll', 'PascalABCLanguageInfo.dll', 'LanguageIntegrator.dll',
    'LambdaAnySynToSemConverter.dll', 'Localization.dll', 'StringConstants.dll',
    'PABCCoreUtils.dll', 'PascalABCNet.LanguageServices.dll'
)) {
    $matches = @($manifest.files | Where-Object { [IO.Path]::GetFileName($_.path) -eq $library })
    if ($matches.Count -ne 1 -or $matches[0].path -ne $library) {
        throw "Shared library must occur only at the runtime root: $library"
    }
}
Write-Host "PASS unified kit: all five entrypoints, one shared compiler kit, identical hashes, manifest-only export, no PDB/legacy PCU/transport dependencies ($($manifest.files.Count) files)."
