[CmdletBinding()]
param(
    [string]$IdeDirectory = '',
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
if ([string]::IsNullOrWhiteSpace($IdeDirectory)) {
    $IdeDirectory = Join-Path $repositoryRoot 'bin'
}
$IdeDirectory = [System.IO.Path]::GetFullPath($IdeDirectory)
$hostRoot = Join-Path $IdeDirectory 'CompilerHost'
$runtimeDirectory = Join-Path $hostRoot 'net10'
# This script replaces only its generated CompilerHost\net10, not the IDE directory.
if ($IdeDirectory -eq $repositoryRoot -or $IdeDirectory -eq [System.IO.Path]::GetPathRoot($IdeDirectory)) {
    throw "Refusing to deploy into a repository or drive root: $IdeDirectory"
}
Write-Host "Building local compiler host: $runtimeDirectory"
& (Join-Path $PSScriptRoot 'build-compiler-host.ps1') -Target net10 `
    -PascalABCSourcePath $repositoryRoot -OutputRoot $hostRoot `
    -Configuration $Configuration -IncludeRuntime

$pluginProject = Join-Path $repositoryRoot 'VisualPlugins\CompileNet10\CompileNet10.csproj'
& dotnet build $pluginProject --configuration $Configuration `
    --output $IdeDirectory --disable-build-servers --nologo
if ($LASTEXITCODE -ne 0) {
    throw "CompileNet10Plugin build failed with exit code $LASTEXITCODE."
}
Write-Host "Plugin built: $(Join-Path $IdeDirectory 'CompileNet10Plugin.dll')"
Write-Host 'Existing CompileNet10Plugin.ini is preserved. For the bundled host use RuntimeDirectory=CompilerHost\net10.'
