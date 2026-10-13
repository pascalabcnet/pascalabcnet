[CmdletBinding()]
param([switch]$SkipBuild)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$runtime = Join-Path $repositoryRoot 'bin-net10'
if (-not $SkipBuild) { & (Join-Path $PSScriptRoot 'build-net10-runtime.ps1') }
& (Join-Path $PSScriptRoot 'test-net10-runtime.ps1') -SkipBuild

# Tests run alongside the shared binaries. No private compiler kit is built
# or copied to a separate IntelliSense directory. Test entrypoints are not
# part of the production manifest and do not enter exported distributions.
$savedServerAssembly = $env:PABC_TOOLING_SERVER_ASSEMBLY
Push-Location $repositoryRoot
try {
    $env:PABC_TOOLING_SERVER_ASSEMBLY = Join-Path $runtime 'PascalABCNet.LanguageServer.dll'
    foreach ($project in @('HeadlessSmokeTest', 'LanguageServerSmokeTest')) {
        & dotnet build (Join-Path $repositoryRoot "tests\$project\$project.csproj") `
            --configuration Release -p:TargetFramework=net10.0 `
            -p:BuildProjectReferences=false "-p:OutputPath=$runtime\" `
            -p:AppendTargetFrameworkToOutputPath=false -p:SatelliteResourceLanguages=ru `
            --disable-build-servers -m:1 --nologo
        if ($LASTEXITCODE -ne 0) { throw "$project build failed." }
        & dotnet (Join-Path $runtime "$project.dll")
        if ($LASTEXITCODE -ne 0) { throw "$project failed." }
    }
    # Start the server from an exported distribution as well, without relying
    # on Tooling or on assemblies outside that distribution.
    $export = Join-Path $repositoryRoot ('.codex-build\language-server-export\' + [Guid]::NewGuid().ToString('N'))
    & (Join-Path $PSScriptRoot 'export-net10-runtime.ps1') -Destination $export
    $env:PABC_TOOLING_SERVER_ASSEMBLY = Join-Path $export 'PascalABCNet.LanguageServer.dll'
    & dotnet (Join-Path $runtime 'LanguageServerSmokeTest.dll')
    if ($LASTEXITCODE -ne 0) { throw 'Exported LanguageServer failed.' }
}
finally {
    $env:PABC_TOOLING_SERVER_ASSEMBLY = $savedServerAssembly
    Pop-Location
}
# Test builds must not replace any of the manifest's production files.
& (Join-Path $PSScriptRoot 'test-net10-runtime.ps1') -SkipBuild
