[CmdletBinding()]
param([switch]$SkipBuild)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
if (-not $SkipBuild) {
    & (Join-Path $PSScriptRoot 'rebuild-net10-standard-units.ps1') -Configuration Release
}
& (Join-Path $PSScriptRoot 'assert-net10-runtime.ps1') -Configuration Release -RequireCompiledUnits
$release = Join-Path $repositoryRoot 'Release'
$stage = [IO.Path]::GetFullPath((Join-Path $release 'PascalABCNET-Console-net10'))
$zip = Join-Path $release 'PascalABCNET-Console-net10.zip'
if ($stage -ne (Join-Path $repositoryRoot 'Release\PascalABCNET-Console-net10')) { throw "Unsafe package directory: $stage" }
if (Test-Path -LiteralPath $stage) { Remove-Item -LiteralPath $stage -Recurse -Force }
New-Item -ItemType Directory -Path $stage | Out-Null
& (Join-Path $PSScriptRoot 'export-net10-runtime.ps1') -Destination $stage -IncludeCompiledUnits
Copy-Item -LiteralPath (Join-Path $repositoryRoot 'InstallerSamples') -Destination (Join-Path $stage 'Samples') -Recurse
Copy-Item -LiteralPath (Join-Path $repositoryRoot 'ConsoleDistribution\README.txt') -Destination (Join-Path $stage 'README.txt')
if (Test-Path -LiteralPath $zip) { Remove-Item -LiteralPath $zip -Force }
Compress-Archive -LiteralPath $stage -DestinationPath $zip -CompressionLevel Optimal
if (-not (Test-Path -LiteralPath $zip -PathType Leaf)) { throw 'Console ZIP was not created.' }
Write-Host "Created: $zip"
