[CmdletBinding()]
param([Parameter(Mandatory = $true)][string]$RuntimeDirectory)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$root = [IO.Path]::GetFullPath($RuntimeDirectory)
if (-not (Test-Path -LiteralPath $root -PathType Container)) { throw "Missing runtime directory: $root" }

# Only known obsolete dependency resources, never arbitrary language folders.
# Do not follow directory junctions/symlinks outside the supplied runtime.
foreach ($language in @('cs', 'de', 'es', 'fr', 'it', 'ja', 'ko', 'pl', 'pt-BR', 'tr', 'zh-Hans', 'zh-Hant')) {
    $directory = Join-Path $root $language
    if (-not (Test-Path -LiteralPath $directory -PathType Container)) { continue }
    if ((Get-Item -LiteralPath $directory).Attributes -band [IO.FileAttributes]::ReparsePoint) {
        throw "Refusing to clean linked resource directory: $directory"
    }
    foreach ($name in @('Microsoft.VisualStudio.LanguageServer.Protocol.resources.dll',
        'Microsoft.VisualStudio.Threading.resources.dll', 'Microsoft.VisualStudio.Validation.resources.dll',
        'StreamJsonRpc.resources.dll')) {
        $file = Join-Path $directory $name
        if (Test-Path -LiteralPath $file -PathType Leaf) { Remove-Item -LiteralPath $file -Force }
    }
    if (@(Get-ChildItem -LiteralPath $directory -Force).Count -eq 0) {
        Remove-Item -LiteralPath $directory -Force
    }
}
foreach ($name in @('pabcnetc.dll.config', 'pabcnetcclear.dll.config')) {
    $file = Join-Path $root $name
    if (Test-Path -LiteralPath $file -PathType Leaf) { Remove-Item -LiteralPath $file -Force }
}
