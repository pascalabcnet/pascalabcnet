[CmdletBinding()]
param(
    [string]$OutputRoot = '',
    [string]$PascalABCSourcePath = '',
    [ValidateSet('all', 'net-framework', 'net10')]
    [string]$Target = 'all',
    [string]$Configuration = 'Release'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repositoryRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
if ([string]::IsNullOrWhiteSpace($OutputRoot)) {
    $OutputRoot = Join-Path $repositoryRoot 'artifacts\compiler-host'
}
if ([string]::IsNullOrWhiteSpace($PascalABCSourcePath)) {
    $PascalABCSourcePath = $repositoryRoot
}
$OutputRoot = [System.IO.Path]::GetFullPath($OutputRoot)
$PascalABCSourcePath = [System.IO.Path]::GetFullPath($PascalABCSourcePath)

$controllerProject = Join-Path $repositoryRoot `
    'PascalABCNet.CompilerController\PascalABCNet.CompilerController.csproj'
$workerProject = Join-Path $repositoryRoot `
    'PascalABCNet.CompilerWorker\PascalABCNet.CompilerWorker.csproj'

function Assert-FileExists {
    param([string]$Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "Required file was not found: $Path"
    }
}

function Reset-OutputDirectory {
    param([string]$Path)

    $resolvedPath = [System.IO.Path]::GetFullPath($Path)
    if ($resolvedPath -eq $repositoryRoot -or
        $resolvedPath -eq $PascalABCSourcePath -or
        $resolvedPath.Length -lt 4) {
        throw "Refusing to replace unsafe output directory: $resolvedPath"
    }

    if (Test-Path -LiteralPath $resolvedPath) {
        Remove-Item -LiteralPath $resolvedPath -Recurse -Force
    }
    New-Item -ItemType Directory -Path $resolvedPath -Force | Out-Null
}

function Invoke-HostBuild {
    param(
        [string]$ProjectPath,
        [string]$Framework,
        [string]$Destination
    )

    & dotnet build $ProjectPath `
        --configuration $Configuration `
        --framework $Framework `
        --output $Destination `
        --disable-build-servers `
        -m:1 `
        -p:BuildInParallel=false `
        -p:SatelliteResourceLanguages=ru `
        -p:PascalABCSourceRoot=$PascalABCSourcePath `
        --nologo
    if ($LASTEXITCODE -ne 0) {
        throw "Compiler host build failed for $Framework with exit code $LASTEXITCODE."
    }
}

function Build-Target {
    param(
        [string]$Framework,
        [string]$DirectoryName
    )

    $destination = Join-Path $OutputRoot $DirectoryName
    Reset-OutputDirectory $destination
    Invoke-HostBuild $controllerProject $Framework $destination
    Invoke-HostBuild $workerProject $Framework $destination

    Get-ChildItem -LiteralPath $destination -File -Filter '*.pdb' |
        Remove-Item -Force

    if ($Framework -eq 'net472') {
        Assert-FileExists (Join-Path $destination 'PABCCompilerController.exe')
        Assert-FileExists (Join-Path $destination 'ZMQServerPas.exe')
    }
    else {
        foreach ($fileName in @(
            'PABCCompilerController.dll',
            'PABCCompilerController.deps.json',
            'PABCCompilerController.runtimeconfig.json',
            'ZMQServerPas.dll',
            'ZMQServerPas.deps.json',
            'ZMQServerPas.runtimeconfig.json')) {
            Assert-FileExists (Join-Path $destination $fileName)
        }
    }

    foreach ($removedDependency in @('NetMQ.dll', 'AsyncIO.dll', 'NaCl.dll')) {
        $removedDependencyPath = Join-Path $destination $removedDependency
        if (Test-Path -LiteralPath $removedDependencyPath -PathType Leaf) {
            throw "Removed transport dependency is still present: $removedDependencyPath"
        }
    }
    Write-Host "Compiler host built: $destination"
}

Assert-FileExists $controllerProject
Assert-FileExists $workerProject
Assert-FileExists (Join-Path $PascalABCSourcePath 'PascalABCNET.sln')
New-Item -ItemType Directory -Path $OutputRoot -Force | Out-Null

if ($Target -in @('all', 'net-framework')) {
    Build-Target 'net472' 'net-framework'
}
if ($Target -in @('all', 'net10')) {
    Build-Target 'net10.0' 'net10'
}
