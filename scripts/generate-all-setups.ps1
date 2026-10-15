[CmdletBinding()]
param(
    [ValidateSet('All', 'Prepare', 'Test', 'Package')][string]$Stage = 'All',
    [ValidateSet('All', 'NetFramework')][string]$Target = 'All',
    [switch]$NoVersionIncrement,
    [switch]$Plan
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$generators = Join-Path $repositoryRoot 'ReleaseGenerators'
$legacy = Join-Path $repositoryRoot 'bin'
$includeNet10 = $Target -eq 'All'

function Invoke-Checked([string]$File, [string[]]$Arguments = @()) {
    & $File @Arguments
    if ($LASTEXITCODE -ne 0) { throw "$File failed with exit code $LASTEXITCODE." }
}

function Invoke-ReleaseStep([string]$Name, [scriptblock]$Action) {
    Write-Host "RELEASE STEP: $Name"
    if (-not $Plan) { & $Action }
}

function Assert-PreparedRelease {
    $requiredFiles = @('PascalABCNET.exe', 'pabcnetc.exe', 'pabcnetcclear.exe',
        'PluginsSupport.dll', 'Lib\PABCRtl.dll')
    if ($includeNet10) { $requiredFiles += @('CompileNet10Plugin.dll', 'CompileNet10Plugin.ini') }
    foreach ($file in $requiredFiles) {
        if (-not (Test-Path -LiteralPath (Join-Path $legacy $file) -PathType Leaf)) {
            throw "Missing prepared release file: $file. Run -Stage Prepare first."
        }
    }
    if ($includeNet10) {
        & (Join-Path $PSScriptRoot 'assert-net10-runtime.ps1') -Configuration Release -RequireCompiledUnits
    }
}

Push-Location $repositoryRoot
try {
    if ($Stage -in @('All', 'Prepare')) {
        Invoke-ReleaseStep 'Prepare installer samples' {
            $samples = Join-Path $generators 'Samples'
            foreach ($entry in @(
                @{ Name = 'Pas'; Source = (Join-Path $repositoryRoot 'InstallerSamples'); Argument = '..\..\InstallerSamples' },
                @{ Name = 'BF'; Source = (Join-Path $samples '_svn\BF'); Argument = '_svn\BF' },
                @{ Name = 'PL0'; Source = (Join-Path $samples '_svn\PL0'); Argument = '_svn\PL0' }
            )) {
                if (-not (Test-Path -LiteralPath $entry.Source -PathType Container)) { throw "Missing samples: $($entry.Source)" }
                $target = [IO.Path]::GetFullPath((Join-Path $samples $entry.Name))
                if (-not $target.StartsWith($samples + '\', [StringComparison]::OrdinalIgnoreCase)) {
                    throw "Unsafe samples target: $target"
                }
                if (Test-Path -LiteralPath $target) {
                    Get-ChildItem -LiteralPath $target -Recurse -File | Remove-Item -Force
                }
                # GetSamples prefixes BOTH arguments with its executable directory.
                # Absolute arguments are therefore invalid for this historical tool.
                Invoke-Checked (Join-Path $samples 'GetSamples.exe') @($entry.Argument, $entry.Name)
            }
        }
        Invoke-ReleaseStep 'Prepare version once' {
            New-Item -ItemType Directory -Path (Join-Path $repositoryRoot 'Release') -Force | Out-Null
            if (-not $NoVersionIncrement) {
                Invoke-Checked '.\Utils\IncrementVresion\IncrementVresion.exe' @('Configuration\Version.defs', 'REVISION', '1')
            }
            foreach ($pair in @(
                @('Configuration\GlobalAssemblyInfo.cs.tmpl', 'Configuration\GlobalAssemblyInfo.cs'),
                @('ReleaseGenerators\PascalABCNET_version.nsh.tmpl', 'ReleaseGenerators\PascalABCNET_version.nsh'),
                @('Configuration\pabcversion.txt.tmpl', 'Release\pabcversion.txt')
            )) {
                Invoke-Checked '.\Utils\ReplaceInFiles\ReplaceInFiles.exe' @('Configuration\Version.defs', $pair[0], $pair[1])
            }
        }
        Invoke-ReleaseStep 'Build legacy IDE and compiler once' {
            Invoke-Checked 'dotnet' @('build', '-c', 'Release', '--no-incremental', 'PascalABCNET.sln',
                '-p:PABCNET_LEGACY_ONLY=true', '--disable-build-servers', '-m:1')
        }
        # Keep both historical passes: PABCRtl consumes the first PCUs; final
        # standard modules are compiled after the new runtime has been signed.
        Invoke-ReleaseStep 'Bootstrap legacy modules before PABCRtl' {
            Push-Location $generators
            try {
                Invoke-Checked (Join-Path $legacy 'pabcnetc.exe') @('RebuildStandartModules.pas', '/rebuild')
                Get-ChildItem -LiteralPath (Join-Path $legacy 'Lib\SPython') -File -Filter '*.pcu' | Remove-Item -Force
                Invoke-Checked (Join-Path $legacy 'pabcnetc.exe') @('RebuildStandartModulesSPython.pas')
            }
            finally { Pop-Location }
        }
        Invoke-ReleaseStep 'Build, sign and register legacy PABCRtl' {
            Push-Location (Join-Path $generators 'PABCRtl')
            try {
                Invoke-Checked (Join-Path $legacy 'pabcnetc.exe') @('PABCRtl.pas', '/rebuild')
                Invoke-Checked (Join-Path $generators 'sn.exe') @('-Vr', 'PABCRtl.dll')
                try { Invoke-Checked (Join-Path $generators 'sn.exe') @('-R', 'PABCRtl.dll', 'KeyPair.snk') }
                finally { Invoke-Checked (Join-Path $generators 'sn.exe') @('-Vu', 'PABCRtl.dll') }
                Copy-Item -LiteralPath '.\PABCRtl.dll' -Destination (Join-Path $legacy 'Lib\PABCRtl.dll') -Force
            }
            finally { Pop-Location }
            # An absent previous GAC entry is normal. Installing the new one must succeed.
            & (Join-Path $generators 'gacutil.exe') /u PABCRtl
            Invoke-Checked (Join-Path $generators 'gacutil.exe') @('/i', (Join-Path $legacy 'Lib\PABCRtl.dll'))
        }
        Invoke-ReleaseStep 'Prepare final legacy modules after PABCRtl' {
            Push-Location $generators
            try {
                Invoke-Checked (Join-Path $legacy 'pabcnetc.exe') @('RebuildStandartModules.pas', '/rebuild')
                Get-ChildItem -LiteralPath (Join-Path $legacy 'Lib\SPython') -File -Filter '*.pcu' | Remove-Item -Force
                Invoke-Checked (Join-Path $legacy 'pabcnetc.exe') @('RebuildStandartModulesSPython.pas')
            }
            finally { Pop-Location }
        }
        if ($includeNet10) {
            Invoke-ReleaseStep 'Build shared .NET 10 runtime once' {
                & (Join-Path $PSScriptRoot 'build-net10-runtime.ps1') -Configuration Release
            }
            Invoke-ReleaseStep 'Build IDE plugin without rebuilding runtime' {
                & (Join-Path $PSScriptRoot 'build-compile-net10-plugin.ps1') -Configuration Release -SkipRuntimeBuild
            }
            Invoke-ReleaseStep 'Rebuild modern standard modules without rebuilding runtime' {
                & (Join-Path $PSScriptRoot 'rebuild-net10-standard-units.ps1') -Configuration Release -SkipBuild
            }
        }
    }
    if ($Stage -in @('All', 'Test')) {
        Invoke-ReleaseStep 'Editor regression: Enter, file loading and caret in both platform modes' {
            Invoke-Checked 'dotnet' @('build', 'tests\CompileNet10.Tests\CompileNet10.Tests.csproj',
                '-c', 'Release', '-p:BuildProjectReferences=false', '--disable-build-servers', '--nologo')
            Invoke-Checked (Join-Path $repositoryRoot 'tests\CompileNet10.Tests\bin\Release\net472\CompileNet10.Tests.exe') @('--editor-keys')
        }
        Invoke-ReleaseStep 'Validate prepared release for tests' { Assert-PreparedRelease }
        Invoke-ReleaseStep 'Build full legacy test runner using prepared compiler' {
            Push-Location $legacy
            try { Invoke-Checked (Join-Path $legacy 'pabcnetcclear.exe') @('TestRunner.pas') }
            finally { Pop-Location }
        }
        foreach ($suite in @('TestSuite', 'TestSuiteAdditionalLanguages\SPythonTests')) {
            Invoke-ReleaseStep "Legacy tests: $suite, passes 1-6" {
                Push-Location (Join-Path $repositoryRoot $suite)
                try {
                    # The second argument disables GUI assertions in the old runner.
                    foreach ($pass in 1..6) { Invoke-Checked (Join-Path $legacy 'TestRunner.exe') @("$pass", '1') }
                }
                finally { Pop-Location }
            }
        }
        $crossTarget = if ($includeNet10) { 'all' } else { 'net472' }
        Invoke-ReleaseStep "Cross-target tests using prepared compiler binaries: $crossTarget" {
            Invoke-Checked (Join-Path $repositoryRoot '_RunCrossTargetTests.bat') @($crossTarget, 'all', '--skip-build')
        }
    }
    if ($Stage -in @('All', 'Package')) {
        Invoke-ReleaseStep 'Validate prepared release for packaging' { Assert-PreparedRelease }
        Invoke-ReleaseStep 'Package existing legacy installers and console ZIP' {
            Invoke-Checked (Join-Path $generators 'PascalABCNET_ALL.bat')
        }
        if ($includeNet10) {
            Invoke-ReleaseStep 'Package modern console ZIP without rebuilding anything' {
                & (Join-Path $PSScriptRoot 'build-console-net10-distribution.ps1') -SkipBuild
            }
            Invoke-ReleaseStep 'Package dual-target IDE installer without rebuilding anything' {
                & (Join-Path $PSScriptRoot 'build-net10-ide-installer.ps1')
            }
        }
    }
}
finally { Pop-Location }
Write-Host "Release stage completed: $Stage ($Target)"
