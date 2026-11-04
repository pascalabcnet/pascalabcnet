[CmdletBinding()]
param([string]$IdeDirectory = '', [string]$OutputDirectory = '')
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
if (!$IdeDirectory) { $IdeDirectory = Join-Path $repositoryRoot 'bin' }
if (!$OutputDirectory) { $OutputDirectory = Join-Path $repositoryRoot 'bin\PluginsLinux' }
$IdeDirectory = [IO.Path]::GetFullPath($IdeDirectory)
$OutputDirectory = [IO.Path]::GetFullPath($OutputDirectory)
if ($OutputDirectory.TrimEnd('\') -eq (Join-Path $repositoryRoot 'bin')) { throw 'Keep the Windows plugin in bin intact; use PluginsLinux.' }
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswhere -latest -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
if (!$msbuild) { throw 'Visual Studio MSBuild is required.' }
if (!(Test-Path -LiteralPath (Join-Path $IdeDirectory 'PluginsSupport.dll'))) { throw 'Build PluginsSupport.dll first.' }
$stage = Join-Path $repositoryRoot ('.codex-build\compile-net10-linux-' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $stage -Force | Out-Null
$project = Join-Path $repositoryRoot 'VisualPlugins\CompileNet10\CompileNet10.csproj'
& $msbuild $project /restore /t:Build /p:Configuration=Release /p:PluginPlatform=Linux /p:UseSharedCompilation=false `
    "/p:IdeReferenceDirectory=$IdeDirectory" "/p:OutDir=$stage\" "/p:OutputPath=$stage\" "/p:IntermediateOutputPath=$stage\obj\" /nologo /v:minimal
if ($LASTEXITCODE -ne 0) { throw "Linux CompileNet10Plugin build failed: $LASTEXITCODE" }
New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $stage 'CompileNet10Plugin.dll') -Destination $OutputDirectory -Force
Write-Host "Linux compile/run plugin built: $(Join-Path $OutputDirectory 'CompileNet10Plugin.dll')"
