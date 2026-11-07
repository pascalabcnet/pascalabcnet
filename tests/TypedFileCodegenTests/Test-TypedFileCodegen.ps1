param(
    [string]$FrameworkRuntime = (Join-Path $PSScriptRoot '..\..\bin'),
    [string]$ModernRuntime = (Join-Path $PSScriptRoot '..\..\bin-net10')
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$FrameworkRuntime = (Resolve-Path -LiteralPath $FrameworkRuntime).Path
$ModernRuntime = (Resolve-Path -LiteralPath $ModernRuntime).Path
Add-Type -Path (Join-Path $FrameworkRuntime 'Mono.Cecil.dll')
$root = Join-Path ([IO.Path]::GetTempPath()) ('pabc-typed-file-' + [Guid]::NewGuid().ToString('N'))

function Assert-TypedFileIL([string]$file) {
    $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($file)
    $type = $assembly.MainModule.GetType('PABCSystem.TypedFile')
    if ($null -eq $type) { $type = $assembly.MainModule.GetType('$PABCSystem.TypedFile') }
    if ($null -eq $type) { throw "TypedFile not emitted in $file" }
    $constructors = @($type.Methods | Where-Object Name -eq '.cctor')
    if ($constructors.Count -ne 1) {
        throw "Expected one TypedFile .cctor, found $($constructors.Count) in $file"
    }
    $init = @($type.Methods | Where-Object Name -eq '$Init$')
    if ($init.Count -ne 1) { throw "Expected one TypedFile initializer in $file" }
    $instructions = @($init[0].Body.Instructions | Where-Object { $_.OpCode.Name -ne 'nop' })
    if ($instructions.Count -ne 1 -or $instructions[0].OpCode.Name -ne 'ret') {
        throw "Expected one ret in TypedFile initializer in $file"
    }
}

function Assert-FileOfMetadata([string]$file) {
    $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($file)
    $expected = @{ ImportedIntFile = 'System.Int32'; ImportedRealFile = 'System.Double' }
    foreach ($entry in $expected.GetEnumerator()) {
        $fields = @($assembly.MainModule.Types | ForEach-Object { $_.Fields } |
            Where-Object Name -eq $entry.Key)
        if ($fields.Count -ne 1) { throw "Expected field $($entry.Key) in $file" }
        $attributes = @($fields[0].CustomAttributes |
            Where-Object { $_.AttributeType.FullName -eq '$Attributes.$FileOfAttr' })
        if ($attributes.Count -ne 1) { throw "Expected FileOf on $($entry.Key) in $file" }
        $value = $attributes[0].ConstructorArguments[0].Value
        if ($value -is [Mono.Cecil.CustomAttributeArgument]) { $value = $value.Value }
        if ($value -is [Mono.Cecil.TypeReference]) { $value = $value.FullName }
        if ($value -ne $entry.Value) { throw "Wrong FileOf element on $($entry.Key): $value" }
    }
}

foreach ($target in @('net472', 'net10')) {
    $modern = $target -eq 'net10'
    $runtime = if ($modern) { $ModernRuntime } else { $FrameworkRuntime }
    $dir = Join-Path $root $target
    New-Item -ItemType Directory -Path $dir -Force | Out-Null
    foreach ($name in @('TypedFileRoundTrip.pas', 'TypedFileLibrary.pas', 'TypedFileLibraryClient.pas')) {
        Copy-Item -LiteralPath (Join-Path $PSScriptRoot $name) -Destination $dir
    }
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot '..\..\TestSuite\constructor5.pas') -Destination $dir
    Push-Location $dir
    try {
        foreach ($sample in @('TypedFileRoundTrip', 'TypedFileLibrary', 'TypedFileLibraryClient', 'constructor5')) {
            $source = Join-Path $dir "$sample.pas"
            $extension = if ($sample -eq 'TypedFileLibrary') { 'dll' } else { 'exe' }
            $output = Join-Path $dir "$sample.$extension"
            if ($modern) { & dotnet (Join-Path $runtime 'pabcnetcclear.dll') "/Output:$output" $source }
            else { & (Join-Path $runtime 'pabcnetcclear.exe') "/Output:$output" $source }
            if ($LASTEXITCODE -ne 0) { throw "$target compilation failed: $sample" }
            if ($sample -eq 'TypedFileLibrary') {
                Assert-TypedFileIL $output
                Assert-FileOfMetadata $output
                continue
            }
            Assert-TypedFileIL $output
            if ($modern) { & dotnet $output } else { & $output }
            if ($LASTEXITCODE -ne 0) { throw "$target execution failed: $sample" }
        }
    }
    finally { Pop-Location }
    Write-Host "PASS $target - TypedFile IL, integer/real round-trip, FileOf metadata and DLL import, ordinary static constructor"
}
