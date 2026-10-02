[CmdletBinding()]
param(
    [ValidateSet('all', 'net-framework', 'net10')]
    [string]$Target = 'all',
    [string]$PascalABCSourcePath = ''
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repositoryRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
if ([string]::IsNullOrWhiteSpace($PascalABCSourcePath)) {
    $PascalABCSourcePath = $repositoryRoot
}
$PascalABCSourcePath = [System.IO.Path]::GetFullPath($PascalABCSourcePath)
$buildRoot = Join-Path $repositoryRoot '.codex-build\compiler-controller'
$hostRoot = Join-Path $buildRoot 'host'
$runtimeRoot = Join-Path $buildRoot 'runtime'
$buildScript = Join-Path $PSScriptRoot 'build-compiler-host.ps1'
$smokeProject = Join-Path $repositoryRoot `
    'CompilerControllerSmokeTest\CompilerControllerSmokeTest.csproj'

$compilerDlls = @(
    'Compiler.dll',
    'CompilerTools.dll',
    'Errors.dll',
    'LambdaAnySynToSemConverter.dll',
    'LanguageIntegrator.dll',
    'Localization.dll',
    'NETGenerator.dll',
    'PABCCoreUtils.dll',
    'ParserTools.dll',
    'PascalABCLanguageInfo.dll',
    'PascalABCParser.dll',
    'SemanticTree.dll',
    'StringConstants.dll',
    'SyntaxTree.dll',
    'SyntaxTreeConverters.dll',
    'SyntaxVisitors.dll',
    'TreeConverter.dll'
)

function Assert-FileExists {
    param([string]$Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "Required file was not found: $Path"
    }
}

function Copy-RuntimeTree {
    param(
        [string]$HostDirectory,
        [string]$CompilerDirectory,
        [string]$Destination
    )

    if (Test-Path -LiteralPath $Destination) {
        Remove-Item -LiteralPath $Destination -Recurse -Force
    }
    New-Item -ItemType Directory -Path $Destination -Force | Out-Null

    Copy-Item -Path (Join-Path $HostDirectory '*') -Destination $Destination `
        -Recurse -Force
    foreach ($dllName in $compilerDlls) {
        $destinationPath = Join-Path $Destination $dllName
        if (-not (Test-Path -LiteralPath $destinationPath -PathType Leaf)) {
            $sourcePath = Join-Path $CompilerDirectory $dllName
            Assert-FileExists $sourcePath
            Copy-Item -LiteralPath $sourcePath -Destination $Destination -Force
        }
    }

    foreach ($directoryName in @('Lib', 'Lng')) {
        $sourceDirectory = Join-Path $CompilerDirectory $directoryName
        if (-not (Test-Path -LiteralPath $sourceDirectory -PathType Container)) {
            $sourceDirectory = Join-Path (Join-Path $PascalABCSourcePath 'bin') `
                $directoryName
        }
        if (-not (Test-Path -LiteralPath $sourceDirectory -PathType Container)) {
            throw "Required directory was not found: $sourceDirectory"
        }
        Copy-Item -LiteralPath $sourceDirectory -Destination $Destination `
            -Recurse -Force
    }
}

function Invoke-SmokeTest {
    param([string]$RuntimeDirectory, [string]$RuntimeTarget)

    & dotnet run --project $smokeProject --configuration Release -- `
        --runtime $RuntimeDirectory --target $RuntimeTarget
    if ($LASTEXITCODE -ne 0) {
        throw "Compiler-controller smoke test failed for $RuntimeTarget."
    }
}

if (Test-Path -LiteralPath $buildRoot) {
    Remove-Item -LiteralPath $buildRoot -Recurse -Force
}
New-Item -ItemType Directory -Path $buildRoot -Force | Out-Null

& $buildScript -OutputRoot $hostRoot `
    -PascalABCSourcePath $PascalABCSourcePath -Target $Target
if ($LASTEXITCODE -ne 0) {
    throw "Compiler-host build failed with exit code $LASTEXITCODE."
}

if ($Target -in @('all', 'net-framework')) {
    $legacyRuntime = Join-Path $runtimeRoot 'net-framework'
    Copy-RuntimeTree (Join-Path $hostRoot 'net-framework') `
        (Join-Path $PascalABCSourcePath 'bin') $legacyRuntime
    # The checked-in legacy PABCSystem PCU is the compatible net472 artifact.
    # A newer modern source timestamp must not make the legacy worker parse it.
    $legacySystemSource = Join-Path $legacyRuntime 'Lib\PABCSystem.pas'
    $legacySystemPcu = Join-Path $legacyRuntime 'Lib\PABCSystem.pcu'
    if ((Test-Path -LiteralPath $legacySystemSource -PathType Leaf) -and
        (Test-Path -LiteralPath $legacySystemPcu -PathType Leaf)) {
        $sourceTime = (Get-Item -LiteralPath $legacySystemSource).LastWriteTimeUtc
        $pcuItem = Get-Item -LiteralPath $legacySystemPcu
        if ($pcuItem.LastWriteTimeUtc -le $sourceTime) {
            $pcuItem.LastWriteTimeUtc = $sourceTime.AddSeconds(1)
        }
    }
    Invoke-SmokeTest $legacyRuntime 'net-framework'
}

if ($Target -in @('all', 'net10')) {
    $modernRuntime = Join-Path $runtimeRoot 'net10'
    Copy-RuntimeTree (Join-Path $hostRoot 'net10') `
        (Join-Path $PascalABCSourcePath 'bin-net10') $modernRuntime
    $redirectModuleName = '__RedirectIOMode'
    $redirectModuleSource = Join-Path $PascalABCSourcePath `
        "bin\Lib\$redirectModuleName.pas"
    $modernLibrary = Join-Path $modernRuntime 'Lib'
    Assert-FileExists $redirectModuleSource
    Copy-Item -LiteralPath $redirectModuleSource -Destination $modernLibrary `
        -Force
    $staleRedirectPcu = Join-Path $modernLibrary "$redirectModuleName.pcu"
    if (Test-Path -LiteralPath $staleRedirectPcu -PathType Leaf) {
        Remove-Item -LiteralPath $staleRedirectPcu -Force
    }
    Invoke-SmokeTest $modernRuntime 'net10'
}

Write-Host 'All requested compiler-controller smoke tests passed.'
