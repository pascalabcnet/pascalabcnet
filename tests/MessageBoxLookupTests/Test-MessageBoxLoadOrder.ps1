[CmdletBinding()]
param([string]$LegacyTreeConverterPath = '', [string]$ModernTreeConverterPath = '')
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
function Copy-CompilerOverride([string]$TreeConverterPath, [string]$Destination) {
    if (-not $TreeConverterPath) { return }
    # A newly built TreeConverter can reference a newer compiler revision.
    # Keep its directly built compiler dependencies coherent in the test clone.
    $directory = Split-Path -Parent $TreeConverterPath
    foreach ($name in @('TreeConverter', 'CompilerTools', 'Errors', 'Localization',
        'SemanticTree', 'StringConstants', 'SyntaxTree', 'PABCCoreUtils')) {
        Copy-Item -LiteralPath (Join-Path $directory "$name.dll") -Destination $Destination -Force
    }
}
$repo = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$work = Join-Path $repo ('.codex-build\messagebox-load-order\' + [Guid]::NewGuid().ToString('N'))
$runtime = Join-Path $work 'legacy'
New-Item -ItemType Directory -Path "$runtime\Lib" -Force | Out-Null
Get-ChildItem -LiteralPath (Join-Path $repo 'bin') -File | Where-Object {
    $_.Extension -in @('.dll', '.exe', '.config')
} | Copy-Item -Destination $runtime
Copy-Item -LiteralPath (Join-Path $repo 'bin\Lng') -Destination $runtime -Recurse
Copy-CompilerOverride $LegacyTreeConverterPath $runtime
$sourceLib = Join-Path $repo 'bin\Lib'
foreach ($file in Get-ChildItem -LiteralPath $sourceLib -Recurse -File) {
    if ($file.Extension -eq '.pcu') { continue }
    $destination = Join-Path "$runtime\Lib" $file.FullName.Substring($sourceLib.Length + 1)
    New-Item -ItemType Directory -Path (Split-Path -Parent $destination) -Force | Out-Null
    Copy-Item -LiteralPath $file.FullName -Destination $destination
}
foreach ($unitName in @('RobotField', 'PT4MakerNetX')) {
    $path = Join-Path $runtime "Lib\$unitName.pas"
    $text = [IO.File]::ReadAllText($path).Replace('System.Windows.Forms.MessageBox.Show', 'MessageBox.Show')
    [IO.File]::WriteAllText($path, $text, (New-Object Text.UTF8Encoding($true)))
}
foreach ($target in @('legacy', 'modern')) {
if ($target -eq 'modern') {
    $runtime = Join-Path $work 'modern'
    & (Join-Path $repo 'scripts\export-net10-runtime.ps1') -Destination $runtime
    Copy-CompilerOverride $ModernTreeConverterPath $runtime
    foreach ($unitName in @('RobotField', 'PT4MakerNetX')) {
        $path = Join-Path $runtime "Lib\$unitName.pas"
        $text = [IO.File]::ReadAllText($path).Replace('System.Windows.Forms.MessageBox.Show', 'MessageBox.Show')
        [IO.File]::WriteAllText($path, $text, (New-Object Text.UTF8Encoding($true)))
    }
}
$variants = @('same_modern_bootstrap', 'early_graphics_like_ordinary')
if ($target -eq 'legacy') { $variants += 'ordinary_bootstrap' }
foreach ($variant in $variants) {
    $lib = [IO.Path]::GetFullPath((Join-Path $runtime 'Lib'))
    if (-not $lib.StartsWith($work + '\', [StringComparison]::OrdinalIgnoreCase)) { throw 'Unsafe fixture path' }
    Get-ChildItem -LiteralPath $lib -File -Filter '*.pcu' | Remove-Item -Force
    $source = Join-Path $work "$target-$variant.pas"
    $bootstrap = if ($variant -eq 'ordinary_bootstrap') { 'RebuildStandartModules.pas' } else { 'RebuildStandartModulesNet10.pas' }
    $text = [IO.File]::ReadAllText((Join-Path $repo "ReleaseGenerators\$bootstrap"))
    if ($variant -eq 'early_graphics_like_ordinary') {
        $text = $text.Replace('MLUtilsABC;', 'MLUtilsABC, GraphABC;')
    }
    [IO.File]::WriteAllText($source, $text, (New-Object Text.UTF8Encoding($true)))
    if ($target -eq 'legacy') { $output = & "$runtime\pabcnetcclear.exe" $source '/pcuonly' 2>&1 }
    else { $output = & dotnet "$runtime\pabcnetcclear.dll" $source '/pcuonly' 2>&1 }
    $code = $LASTEXITCODE
    [IO.File]::WriteAllText((Join-Path $work "$target-$variant.log"), ($output -join [Environment]::NewLine))
    Write-Host "$target $variant : exit=$code"
    Write-Host ($output -join [Environment]::NewLine)
    if ($code -ne 0) { throw "$target $variant compilation failed." }
}
}
Write-Host "Fixtures: $work"
