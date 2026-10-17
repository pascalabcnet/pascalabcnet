[CmdletBinding()]
param([string]$Installer = '')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
if (-not $Installer) { $Installer = Join-Path $repositoryRoot 'Release\PascalABCNETSetupNet10.exe' }

# The NSIS executable manifest is an uncompressed UTF-8 PE resource.
# Read the binary; never execute the product installer in this check.
$binaryText = [Text.Encoding]::UTF8.GetString([IO.File]::ReadAllBytes([IO.Path]::GetFullPath($Installer)))
$manifests = @([regex]::Matches($binaryText, '<assembly\b[\s\S]*?</assembly>') | ForEach-Object {
    [xml]$_.Value
})
$manifest = @($manifests | Where-Object {
    $_.SelectSingleNode("/*[local-name()='assembly']/*[local-name()='assemblyIdentity' and @name='Nullsoft.NSIS.exehead']")
})
if ($manifest.Count -ne 1) { throw 'Expected one NSIS executable manifest.' }
$dpi = $manifest[0].SelectSingleNode("//*[local-name()='dpiAware' and namespace-uri()='http://schemas.microsoft.com/SMI/2005/WindowsSettings']")
if ($null -eq $dpi -or $dpi.InnerText -ne 'true') { throw 'The installer does not declare system DPI awareness.' }
$execution = $manifest[0].SelectSingleNode("//*[local-name()='requestedExecutionLevel']")
if ($null -eq $execution -or $execution.GetAttribute('level') -ne 'requireAdministrator') {
    throw 'The installer must still request administrator privileges.'
}
Write-Host 'PASS installer declares system DPI awareness and retains administrator privileges.'
