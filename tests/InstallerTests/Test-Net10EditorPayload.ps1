[CmdletBinding()]
param([string]$Installer = '', [string]$SevenZip = '')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
if (-not $Installer) { $Installer = Join-Path $repositoryRoot 'Release\PascalABCNETSetupNet10.exe' }
if (-not $SevenZip) { $SevenZip = Join-Path $env:ProgramFiles '7-Zip\7z.exe' }
if (-not (Test-Path -LiteralPath $SevenZip -PathType Leaf)) { throw 'Specify -SevenZip with the path to 7z.exe.' }
if (-not (Test-Path -LiteralPath $Installer -PathType Leaf)) { throw "Installer not found: $Installer" }
$stage = Join-Path $repositoryRoot ('.codex-build\installer-editor-tests\' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $stage -Force | Out-Null

# Build only the test entrypoint; references come from the prepared IDE in bin.
& dotnet build (Join-Path $repositoryRoot 'tests\CompileNet10.Tests\CompileNet10.Tests.csproj') `
    -c Release --output $stage -p:BuildProjectReferences=false --disable-build-servers --nologo
if ($LASTEXITCODE -ne 0) { throw 'Editor test build failed.' }

# Read the product installer as an archive: never execute its installation sections.
# Replace both copies: the editor DLL and the IDE containing bracket highlighting.
& $SevenZip e ([IO.Path]::GetFullPath($Installer)) 'ICSharpCode.TextEditor.dll' 'PascalABCNET.exe' "-o$stage" -y
if ($LASTEXITCODE -ne 0) { throw 'Could not extract the editor from the installer.' }
foreach ($file in @('ICSharpCode.TextEditor.dll', 'PascalABCNET.exe')) {
    $extracted = Join-Path $stage $file
    $prepared = Join-Path (Join-Path $repositoryRoot 'bin') $file
    if ((Get-FileHash -LiteralPath $extracted).Hash -ne (Get-FileHash -LiteralPath $prepared).Hash) {
        throw "The installer contains a different file than the prepared IDE: $file"
    }
}
& (Join-Path $stage 'CompileNet10.Tests.exe') '--editor-keys'
if ($LASTEXITCODE -ne 0) { throw "The editor shipped in the installer failed Enter regression checks: $stage" }
Write-Host "PASS editor extracted from installer: $stage"
