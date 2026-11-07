param(
    [string]$FrameworkRuntime = (Join-Path $PSScriptRoot '..\..\bin'),
    [string]$ModernRuntime = (Join-Path $PSScriptRoot '..\..\bin-net10')
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$FrameworkRuntime = (Resolve-Path -LiteralPath $FrameworkRuntime).Path
$ModernRuntime = (Resolve-Path -LiteralPath $ModernRuntime).Path
Add-Type -Path (Join-Path $FrameworkRuntime 'Mono.Cecil.dll')
$root = Join-Path ([IO.Path]::GetTempPath()) ('pabc-generic-init-' + [Guid]::NewGuid().ToString('N'))

function Invoke-CompileAndRun([string]$source, [string]$output) {
    if ($modern) { & dotnet (Join-Path $runtime 'pabcnetcclear.dll') "/Output:$output" $source }
    else { & (Join-Path $runtime 'pabcnetcclear.exe') "/Output:$output" $source }
    if ($LASTEXITCODE -ne 0) { throw "$target compilation failed: $source" }
    if ($modern) { & dotnet $output } else { & $output }
    if ($LASTEXITCODE -ne 0) { throw "$target execution failed: $output" }
}

function Assert-InitializationIL([string]$file, [bool]$standalone, [bool]$needsHelpers, [bool]$needsArrayHelper = $false) {
    $module = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($file).MainModule
    $helpers = @($module.Types | Where-Object Name -eq '$GenericInitialization$')
    $caches = @($module.Types | Where-Object Name -like '$GenericInitializationKind$*')
    $systemCalls = @($module.Types | ForEach-Object { $_.Methods } | Where-Object HasBody |
        ForEach-Object { $_.Body.Instructions } | Where-Object {
            $_.Operand -is [Mono.Cecil.MethodReference] -and
            $_.Operand.DeclaringType.FullName -like '*PABCSystem*'
        })
    $expectedMethods = @()
    if ($standalone -and $needsHelpers) { $expectedMethods += 'DetermineType', 'Initialize' }
    if ($needsArrayHelper) { $expectedMethods += 'InitializeArray' }
    if ($helpers.Count -ne [int]($expectedMethods.Count -gt 0) -or
        $caches.Count -ne [int]($standalone -and $needsHelpers)) {
        throw "Unexpected number of generic initialization helper types in $file"
    }
    if ($expectedMethods.Count -gt 0) {
        $methodNames = @($helpers[0].Methods | ForEach-Object Name)
        if ($methodNames.Count -ne $expectedMethods.Count -or
            @($methodNames | Where-Object { $_ -notin $expectedMethods }).Count -ne 0) {
            throw "Unexpected helper methods in $file"
        }
    }
    if ($standalone) {
        $systemTypes = @($module.Types | Where-Object Namespace -like '*PABCSystem*')
        $systemReferences = @($module.AssemblyReferences | Where-Object Name -in @('PABCSystem','PABCRtl'))
        if ($systemTypes.Count -ne 0 -or $systemReferences.Count -ne 0 -or $systemCalls.Count -ne 0) {
            throw "Hidden PABCSystem dependency in $file"
        }
    }
    elseif ($needsHelpers) {
        foreach ($name in @('RuntimeDetermineType','RuntimeInitialize')) {
            if (@($systemCalls | Where-Object { $_.Operand.Name -eq $name }).Count -eq 0) {
                throw "The normal compiler path no longer calls $name in $file"
            }
        }
    }
}

foreach ($target in @('net472', 'net10')) {
    $modern = $target -eq 'net10'
    $runtime = if ($modern) { $ModernRuntime } else { $FrameworkRuntime }
    $dir = Join-Path $root $target
    New-Item -ItemType Directory -Path $dir -Force | Out-Null
    $source = Join-Path $dir 'GenericInitialization.pas'
    $standaloneSource = Join-Path $dir 'GenericInitializationStandalone.pas'
    $arraysSource = Join-Path $dir 'GenericStringArray.pas'
    $standaloneArraysSource = Join-Path $dir 'GenericStringArrayStandalone.pas'
    $emptySource = Join-Path $dir 'EmptyGeneric.pas'
    $client = Join-Path $dir 'GenericInitializationClient.pas'
    $unit = Join-Path $dir 'GenericInitializationUnit.pas'
    foreach ($file in @($source, $arraysSource, $client, $unit)) {
        Copy-Item -LiteralPath (Join-Path $PSScriptRoot (Split-Path -Leaf $file)) -Destination $file
    }
    $directive = '{$DisableStandardUnits}'
    $nl = [Environment]::NewLine
    [IO.File]::WriteAllText($standaloneSource, $directive + $nl + [IO.File]::ReadAllText($source))
    [IO.File]::WriteAllText($standaloneArraysSource, $directive + $nl + [IO.File]::ReadAllText($arraysSource))
    [IO.File]::WriteAllText($emptySource, $directive + $nl + 'type C<T> = class end;' + $nl + 'begin end.')
    Push-Location $dir
    try {
        $standaloneExe = Join-Path $dir 'Standalone.exe'
        Invoke-CompileAndRun $standaloneSource $standaloneExe
        Assert-InitializationIL $standaloneExe $true $true $true
        $normalExe = Join-Path $dir 'Normal.exe'
        Invoke-CompileAndRun $source $normalExe
        Assert-InitializationIL $normalExe $false $true $true
        $arraysExe = Join-Path $dir 'StringArrays.exe'
        Invoke-CompileAndRun $arraysSource $arraysExe
        Assert-InitializationIL $arraysExe $false $false $true
        $standaloneArraysExe = Join-Path $dir 'StringArraysStandalone.exe'
        Invoke-CompileAndRun $standaloneArraysSource $standaloneArraysExe
        Assert-InitializationIL $standaloneArraysExe $true $false $true
        $emptyExe = Join-Path $dir 'EmptyGeneric.exe'
        Invoke-CompileAndRun $emptySource $emptyExe
        Assert-InitializationIL $emptyExe $true $false

        Invoke-CompileAndRun $client (Join-Path $dir 'BeforePcu.exe')
        if (-not (Test-Path -LiteralPath (Join-Path $dir 'GenericInitializationUnit.pcu'))) {
            throw 'No fresh PCU was produced'
        }
        Move-Item -LiteralPath $unit -Destination ($unit + '.saved')
        $afterPcu = Join-Path $dir 'AfterPcu.exe'
        Invoke-CompileAndRun $client $afterPcu
        Assert-InitializationIL $afterPcu $true $true
    }
    finally { Pop-Location }
    Write-Host "PASS $target - generic initialization with and without PABCSystem, helper IL, PCU round trip"
}
