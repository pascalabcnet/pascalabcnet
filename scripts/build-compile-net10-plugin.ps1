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
if ($IdeDirectory -eq $repositoryRoot -or $IdeDirectory -eq [System.IO.Path]::GetPathRoot($IdeDirectory)) {
    throw "Refusing to deploy into a repository or drive root: $IdeDirectory"
}
& (Join-Path $PSScriptRoot 'build-net10-runtime.ps1') -Configuration $Configuration

$pluginProject = Join-Path $repositoryRoot 'VisualPlugins\CompileNet10\CompileNet10.csproj'
& dotnet build $pluginProject --configuration $Configuration `
    --output $IdeDirectory --disable-build-servers --nologo
if ($LASTEXITCODE -ne 0) {
    throw "CompileNet10Plugin build failed with exit code $LASTEXITCODE."
}
Write-Host "Plugin built: $(Join-Path $IdeDirectory 'CompileNet10Plugin.dll')"
Write-Host "Existing CompileNet10Plugin.ini is preserved. Use RuntimeDirectory=..\bin-net10 for the repository IDE, or point it to $(Join-Path $repositoryRoot 'bin-net10') for another IDE."
