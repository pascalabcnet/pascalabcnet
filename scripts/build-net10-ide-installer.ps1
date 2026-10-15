[CmdletBinding()]
param(
    [string]$OutputFile = '',
    [switch]$StageOnly,
    [string]$NsisCompiler = ''
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$generators = Join-Path $repositoryRoot 'ReleaseGenerators'
& (Join-Path $PSScriptRoot 'assert-net10-runtime.ps1') -Configuration Release -RequireCompiledUnits
$plugin = Join-Path $repositoryRoot 'bin\CompileNet10Plugin.dll'
if (-not (Test-Path -LiteralPath $plugin -PathType Leaf)) { throw 'Prepare the IDE plugin before packaging.' }

# The compiler payload comes from the manifest and prepared PCUs; PT4 is copied separately.
# Never File /r the developer bin-net10 directory.
$stage = Join-Path $repositoryRoot ('.codex-build\net10-ide-installer\' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $stage -Force | Out-Null
$kit = Join-Path $stage 'Net10'
& (Join-Path $PSScriptRoot 'export-net10-runtime.ps1') -Destination $kit -IncludeCompiledUnits -SeparateLibrarySources
$pt4Source = Join-Path $generators 'PT4'
if (-not (Test-Path -LiteralPath (Join-Path $pt4Source 'xpt4pabc2.dll') -PathType Leaf)) {
    throw "Missing PT4 distribution: $pt4Source"
}
# PT4.pcu resolves native imports relative to the modern compiler's system directory.
# Keep the ordinary INSTDIR\PT4 intact and ship a complete second copy beside Net10.
Copy-Item -LiteralPath $pt4Source -Destination (Join-Path $kit 'PT4') -Recurse -Force
Copy-Item -LiteralPath $plugin -Destination $stage
$utf8 = New-Object Text.UTF8Encoding($true)
[IO.File]::WriteAllText((Join-Path $stage 'CompileNet10Plugin.ini'),
    "# Installed paths are relative to the IDE executable directory.$([Environment]::NewLine)RuntimeDirectory=Net10$([Environment]::NewLine)DotnetPath=dotnet$([Environment]::NewLine)", $utf8)

function Quote-Nsis([string]$Value) {
    if ($Value.IndexOfAny([char[]]"`r`n") -ge 0) { throw 'Line breaks in an NSIS path are not supported.' }
    return $Value.Replace('$', '$$').Replace('"', '$\"')
}
$files = @(Get-ChildItem -LiteralPath $kit -Recurse -File | Sort-Object FullName)
$directories = @($kit) + @(Get-ChildItem -LiteralPath $kit -Recurse -Directory | Sort-Object { $_.FullName.Length } | ForEach-Object FullName)
$include = New-Object 'Collections.Generic.List[string]'
$include.Add('; Generated from the validated shared runtime; do not edit.')
# All directories are logged before any files so reverse-order removal is safe.
foreach ($directory in $directories) {
    $relative = $directory.Substring($stage.Length + 1)
    $include.Add('${AddDirectory} "$INSTDIR\' + (Quote-Nsis $relative) + '"')
}
$outPath = ''
foreach ($file in $files) {
    if ($file.Name -in @('dotnet.exe', 'coreclr.dll', 'System.Private.CoreLib.dll', 'PABCRtl.dll') -or
        $file.Extension -eq '.pdb') { throw "Unexpected installer payload: $($file.Name)" }
    $relativeDirectory = $file.DirectoryName.Substring($stage.Length + 1)
    if ($relativeDirectory -eq 'Net10\LibSource' -or $relativeDirectory.StartsWith('Net10\LibSource\')) {
        # Upgrade the previous installer layout without leaving searchable sources in Lib.
        # Delete only the exact source paths owned by the old payload, not user files.
        $oldRelativePath = 'Net10\Lib\' + $file.FullName.Substring((Join-Path $stage 'Net10\LibSource').Length + 1)
        $include.Add('Delete "$INSTDIR\' + (Quote-Nsis $oldRelativePath) + '"')
    }
    if ($outPath -ne $relativeDirectory) {
        $include.Add('SetOutPath "$INSTDIR\' + (Quote-Nsis $relativeDirectory) + '"')
        $outPath = $relativeDirectory
    }
    $include.Add('File "' + (Quote-Nsis $file.FullName) + '"')
    $include.Add('${AddFile} "' + (Quote-Nsis $file.Name) + '"')
}
[IO.File]::WriteAllLines((Join-Path $stage 'net10-payload.nsh'), $include, $utf8)
Write-Host "NET10_INSTALLER_PAYLOAD=$stage"
if ($StageOnly) { return }

if (-not $OutputFile) { $OutputFile = Join-Path $repositoryRoot 'Release\PascalABCNETSetupNet10.exe' }
$OutputFile = [IO.Path]::GetFullPath($OutputFile)
New-Item -ItemType Directory -Path (Split-Path -Parent $OutputFile) -Force | Out-Null
if (-not $NsisCompiler) { $NsisCompiler = Join-Path $repositoryRoot 'Utils\NSIS2\Bin\makensis.exe' }
Push-Location $generators
try {
    & $NsisCompiler '/V3' '/INPUTCHARSET' 'UTF8' "/DPABC_NET10_PAYLOAD_DIR=$stage" "/DPABC_INSTALLER_OUTFILE=$OutputFile" 'PascalABCNETNet10.nsi'
    if ($LASTEXITCODE -ne 0) { throw "NSIS compilation failed with exit code $LASTEXITCODE." }
}
finally { Pop-Location }
if (-not (Test-Path -LiteralPath $OutputFile -PathType Leaf)) { throw 'NSIS did not create the installer.' }
Write-Host "Created: $OutputFile ($((Get-Item -LiteralPath $OutputFile).Length) bytes)"
