[CmdletBinding()]
param([string]$IdeDirectory = '', [string]$OutputDirectory = '')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
if (!$IdeDirectory) { $IdeDirectory = Join-Path $repositoryRoot 'bin' }
if (!$OutputDirectory) { $OutputDirectory = Join-Path $repositoryRoot 'bin\PluginsLinux' }
$IdeDirectory = [IO.Path]::GetFullPath($IdeDirectory)
$OutputDirectory = [IO.Path]::GetFullPath($OutputDirectory)
# Keep the Windows plugin in bin intact.
if ($OutputDirectory.TrimEnd('\') -eq (Join-Path $repositoryRoot 'bin')) {
    throw 'Use a separate output directory for the Linux plugin.'
}
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswhere -latest -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
if (!$msbuild) { throw 'Visual Studio MSBuild is required.' }
if (!(Test-Path -LiteralPath (Join-Path $IdeDirectory 'PluginsSupportLinux.dll'))) {
    throw 'Build the Linux shell and PluginsSupportLinux.dll first.'
}
$stage = Join-Path $repositoryRoot ('.codex-build\teacher-control-linux-' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $stage -Force | Out-Null
Get-ChildItem -LiteralPath $IdeDirectory -Filter '*.dll' -File | Copy-Item -Destination $stage
$project = Join-Path $repositoryRoot 'VisualPlugins\TraningControlPlugin\TeacherControlPlugin.csproj'
& $msbuild $project /restore /t:Build /p:Configuration=Release /p:PluginPlatform=Linux /p:BuildProjectReferences=false `
    "/p:OutDir=$stage\" "/p:OutputPath=$stage\" "/p:IntermediateOutputPath=$stage\obj\" /nologo /v:minimal
if ($LASTEXITCODE -ne 0) { throw "Linux TeacherControlPlugin build failed: $LASTEXITCODE" }
New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $stage 'TeacherControlPlugin.dll') -Destination $OutputDirectory -Force
Write-Host "Linux plugin built: $(Join-Path $OutputDirectory 'TeacherControlPlugin.dll')"
