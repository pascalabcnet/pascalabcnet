[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')][string]$Configuration = 'Debug',
    [switch]$SkipBuild
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
if (-not $SkipBuild) { & (Join-Path $PSScriptRoot 'build-net10-runtime.ps1') -Configuration $Configuration }
& (Join-Path $PSScriptRoot 'assert-net10-runtime.ps1') -Configuration $Configuration
$runtime = Join-Path $repositoryRoot 'bin-net10'
$lib = [IO.Path]::GetFullPath((Join-Path $runtime 'Lib'))
if ($lib -ne (Join-Path $repositoryRoot 'bin-net10\Lib')) { throw "Unsafe standard-unit directory: $lib" }
# Delete only compiler caches in the validated modern library, never user sources.
Get-ChildItem -LiteralPath $lib -Recurse -File -Filter '*.pcu' | Remove-Item -Force
$generator = Join-Path $repositoryRoot 'ReleaseGenerators'
$name = 'RebuildStandartModulesNet10'
Push-Location $generator
try {
    # pabcnetcclear has no /rebuild; the PCU deletion above forces compilation.
    & dotnet (Join-Path $runtime 'pabcnetcclear.dll') "$name.pas"
    if ($LASTEXITCODE -ne 0) { throw "Modern standard-unit rebuild failed: $LASTEXITCODE" }
}
finally { Pop-Location }
& (Join-Path $PSScriptRoot 'assert-net10-runtime.ps1') -Configuration $Configuration -RequireCompiledUnits
foreach ($suffix in @('.exe', '.exe.config', '.pdb', '.runtimeconfig.json')) {
    $generated = Join-Path $generator ($name + $suffix)
    if (Test-Path -LiteralPath $generated) { Remove-Item -LiteralPath $generated -Force }
}
Write-Host 'Modern standard modules rebuilt successfully.'
