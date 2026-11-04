[CmdletBinding()]
param([string]$Destination = '')
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
if (!$Destination) { $Destination = Join-Path $repositoryRoot 'Release\PascalABCNETLinux' }
$Destination = [IO.Path]::GetFullPath($Destination)
if (!(Test-Path -LiteralPath (Join-Path $Destination 'PascalABCNETLinux.exe'))) { throw 'Prepare the Linux IDE distribution first.' }
if ($Destination.TrimEnd('\') -eq (Join-Path $repositoryRoot 'bin')) { throw 'Do not replace the Windows plugin.' }
& (Join-Path $PSScriptRoot 'build-compile-net10-linux.ps1')
& (Join-Path $PSScriptRoot 'export-net10-runtime.ps1') -Destination (Join-Path $Destination 'net10')
Copy-Item -LiteralPath (Join-Path $repositoryRoot 'bin\PluginsLinux\CompileNet10Plugin.dll') -Destination $Destination -Force
$settings = Join-Path $Destination 'CompileNet10Plugin.ini'
if (!(Test-Path -LiteralPath $settings)) {
    [IO.File]::WriteAllText($settings, "RuntimeDirectory=net10`nDotnetPath=dotnet`n", (New-Object Text.UTF8Encoding($false)))
}
Write-Host 'Linux .NET 10 compile/run files prepared. The guest needs Microsoft.NETCore.App 10.0.x; LSP provides dot completion through the passive Linux popup; legacy analysis resumes in Mono mode.'
