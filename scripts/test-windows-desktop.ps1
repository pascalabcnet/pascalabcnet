[CmdletBinding()]
param([switch]$Graphics)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
if ([Environment]::OSVersion.Platform -ne [PlatformID]::Win32NT) {
    Write-Host 'SKIP: WindowsDesktop tests require Windows.'
    return
}
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
# Do not replace an IDE's running compiler. Both compiler and tests use this kit.
$runtime = Join-Path $repositoryRoot ('.codex-build\windows-desktop\' + [Guid]::NewGuid().ToString('N') + '\bin-net10')
Write-Host "Isolated runtime: $runtime"
foreach ($project in @('Net10Runtime.slnx', 'tests\WindowsDesktopTests\WindowsDesktopTests.csproj')) {
    & dotnet build (Join-Path $repositoryRoot $project) --configuration Release `
        -p:TargetFramework=net10.0 -p:PABCNET_LEGACY_ONLY=false "-p:OutputPath=$runtime\" `
        -p:AppendTargetFrameworkToOutputPath=false --disable-build-servers -m:1 --nologo
    if ($LASTEXITCODE -ne 0) { throw "Build failed: $project" }
}
$testArguments = @($runtime)
if ($Graphics) { $testArguments += '--graphics' }
& dotnet (Join-Path $runtime 'WindowsDesktopTests.dll') @testArguments
if ($LASTEXITCODE -ne 0) { throw 'WindowsDesktop tests failed.' }
