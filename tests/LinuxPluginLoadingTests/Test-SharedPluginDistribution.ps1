[CmdletBinding()]
param([string]$RepositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '../..')).Path)

$ErrorActionPreference = 'Stop'
Add-Type -Path (Join-Path $RepositoryRoot 'bin/Mono.Cecil.dll')
$canonical = Join-Path $RepositoryRoot 'bin/PluginsSupport.dll'
$hash = (Get-FileHash -LiteralPath $canonical).Hash
$identity = 'PluginsSupport, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
foreach ($relative in @('bin', 'Release/PascalABCNETLinux')) {
    $directory = Join-Path $RepositoryRoot $relative
    $path = Join-Path $directory 'PluginsSupport.dll'
    if ((Get-FileHash -LiteralPath $path).Hash -ne $hash) { throw "Different contract: $relative" }
    $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($path)
    try {
        if ($assembly.Name.FullName -cne $identity) { throw "Contract identity changed: $relative" }
    } finally { if ($assembly -is [IDisposable]) { $assembly.Dispose() } }
    if (Test-Path -LiteralPath (Join-Path $directory 'PluginsSupportLinux.dll')) {
        throw "Obsolete contract remains: $relative"
    }
    foreach ($file in (Get-ChildItem -LiteralPath $directory -File | Where-Object Extension -in @('.dll', '.exe'))) {
        try { $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($file.FullName) }
        catch [System.BadImageFormatException] { continue }
        try {
            foreach ($reference in $assembly.MainModule.AssemblyReferences) {
                if ($reference.Name -eq 'PluginsSupportLinux') { throw "Obsolete contract reference: $($file.FullName)" }
                if ($reference.Name -eq 'PluginsSupport' -and $reference.FullName -cne $identity) {
                    throw "Wrong contract identity: $($file.FullName)"
                }
            }
        } finally { if ($assembly -is [IDisposable]) { $assembly.Dispose() } }
    }
}
Write-Output 'PASS identical Windows/Linux plugin contract; no obsolete contract or references'
