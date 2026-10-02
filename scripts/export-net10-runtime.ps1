[CmdletBinding()]
param([Parameter(Mandatory = $true)][string]$Destination, [switch]$IncludeCompiledUnits)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$source = Join-Path $repositoryRoot 'bin-net10'
$Destination = [IO.Path]::GetFullPath($Destination)
if ($Destination -eq $source -or $Destination -eq (Join-Path $repositoryRoot 'bin') -or
    $source.StartsWith($Destination + '\', [StringComparison]::OrdinalIgnoreCase) -or
    $Destination -eq $repositoryRoot -or
    $Destination -eq [IO.Path]::GetPathRoot($Destination)) { throw 'Unsafe export destination.' }
$manifestPath = Join-Path $source 'net10-runtime.manifest.json'
$manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
foreach ($file in $manifest.files) {
    $inputFile = [IO.Path]::GetFullPath((Join-Path $source $file.path))
    $outputFile = [IO.Path]::GetFullPath((Join-Path $Destination $file.path))
    if (-not $inputFile.StartsWith($source + '\', [StringComparison]::OrdinalIgnoreCase) -or
        -not $outputFile.StartsWith($Destination + '\', [StringComparison]::OrdinalIgnoreCase)) { throw 'Unsafe manifest path.' }
    if ((Get-FileHash -LiteralPath $inputFile -Algorithm SHA256).Hash -ne $file.sha256) {
        throw "Runtime changed since unified build: $inputFile. Rebuild it first."
    }
    New-Item -ItemType Directory -Path (Split-Path -Parent $outputFile) -Force | Out-Null
    Copy-Item -LiteralPath $inputFile -Destination $outputFile -Force
    if ($IncludeCompiledUnits -and $file.path.StartsWith('Lib\') -and [IO.Path]::GetExtension($inputFile) -eq '.pas') {
        $pcu = [IO.Path]::ChangeExtension($inputFile, '.pcu')
        if (Test-Path -LiteralPath $pcu) {
            Copy-Item -LiteralPath $pcu -Destination ([IO.Path]::ChangeExtension($outputFile, '.pcu')) -Force
        }
    }
}
Copy-Item -LiteralPath $manifestPath -Destination $Destination -Force
Write-Host "Exported unified runtime: $Destination"
