[CmdletBinding()]
param([Parameter(Mandatory = $true)][string]$Destination, [switch]$IncludeCompiledUnits,
    [switch]$SeparateLibrarySources)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
if ($SeparateLibrarySources -and -not $IncludeCompiledUnits) {
    throw 'Separate library sources require prepared compiled units.'
}
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$source = Join-Path $repositoryRoot 'bin-net10'
$Destination = [IO.Path]::GetFullPath($Destination)
if ($Destination -eq $source -or $Destination -eq (Join-Path $repositoryRoot 'bin') -or
    $source.StartsWith($Destination + '\', [StringComparison]::OrdinalIgnoreCase) -or
    $Destination -eq $repositoryRoot -or
    $Destination -eq [IO.Path]::GetPathRoot($Destination)) { throw 'Unsafe export destination.' }
$manifestPath = Join-Path $source 'net10-runtime.manifest.json'
& (Join-Path $PSScriptRoot 'assert-net10-runtime.ps1') -RequireCompiledUnits:$IncludeCompiledUnits
$manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
$exportedFiles = @()
foreach ($file in $manifest.files) {
    $inputFile = [IO.Path]::GetFullPath((Join-Path $source $file.path))
    $exportPath = $file.path
    if ($SeparateLibrarySources -and $file.path.StartsWith('Lib\') -and
        [IO.Path]::GetExtension($inputFile) -in @('.pas', '.pys', '.vb')) {
        $exportPath = 'LibSource\' + $file.path.Substring(4)
    }
    $outputFile = [IO.Path]::GetFullPath((Join-Path $Destination $exportPath))
    if (-not $inputFile.StartsWith($source + '\', [StringComparison]::OrdinalIgnoreCase) -or
        -not $outputFile.StartsWith($Destination + '\', [StringComparison]::OrdinalIgnoreCase)) { throw 'Unsafe manifest path.' }
    New-Item -ItemType Directory -Path (Split-Path -Parent $outputFile) -Force | Out-Null
    Copy-Item -LiteralPath $inputFile -Destination $outputFile -Force
    $exportedFiles += [ordered]@{ path = $exportPath; sha256 = $file.sha256 }
    if ($IncludeCompiledUnits -and $file.path.StartsWith('Lib\') -and [IO.Path]::GetExtension($inputFile) -eq '.pas') {
        $pcu = [IO.Path]::ChangeExtension($inputFile, '.pcu')
        if (Test-Path -LiteralPath $pcu) {
            $outputPcu = Join-Path $Destination ([IO.Path]::ChangeExtension($file.path, '.pcu'))
            New-Item -ItemType Directory -Path (Split-Path -Parent $outputPcu) -Force | Out-Null
            Copy-Item -LiteralPath $pcu -Destination $outputPcu -Force
        }
    }
}
if ($SeparateLibrarySources) {
    [ordered]@{ configuration = $manifest.configuration; files = $exportedFiles } |
        ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $Destination 'net10-runtime.manifest.json') -Encoding UTF8
}
else { Copy-Item -LiteralPath $manifestPath -Destination $Destination -Force }
Write-Host "Exported unified runtime: $Destination"
