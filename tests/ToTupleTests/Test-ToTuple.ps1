[CmdletBinding()]
param()
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repo = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$work = Join-Path $repo ('.codex-build\to-tuple-regression\' + [Guid]::NewGuid().ToString('N'))
$tests = @('to_tuple_keyvaluepair', 'tuples1', 'tuples2', 'tuple_var1', 'tuple_var2',
    'tuple_var3', 'label_ass_var_tuple', 'generic_tuple_conversion1', 'var_tuple_and_slice')
foreach ($tfm in @('net472', 'net10.0')) {
    $legacy = if ($tfm -eq 'net472') { 'true' } else { 'false' }
    $prepared = if ($tfm -eq 'net472') { 'bin' } else { 'bin-net10' }
    $runtime = Join-Path $work "$tfm\runtime"
    New-Item -ItemType Directory -Path $runtime -Force | Out-Null
    $build = & dotnet build (Join-Path $repo 'pabcnetc_clear\PABCNETCclear.csproj') -c Release `
        "-p:TargetFramework=$tfm" "-p:PABCNET_LEGACY_ONLY=$legacy" "-p:OutputPath=$runtime\" `
        -p:AppendTargetFrameworkToOutputPath=false --nologo 2>&1
    if ($LASTEXITCODE -ne 0) { throw ($build -join [Environment]::NewLine) }
    $build = & dotnet build (Join-Path $repo 'PascalABCLanguageInfo\PascalABCLanguageInfo.csproj') -c Release `
        "-p:TargetFramework=$tfm" "-p:PABCNET_LEGACY_ONLY=$legacy" "-p:OutputPath=$runtime\" `
        -p:AppendTargetFrameworkToOutputPath=false --nologo 2>&1
    if ($LASTEXITCODE -ne 0) { throw ($build -join [Environment]::NewLine) }
    foreach ($dir in @('Lib', 'Lng')) {
        New-Item -ItemType Directory -Path (Join-Path $runtime $dir) -Force | Out-Null
        Copy-Item -Path (Join-Path $repo "$prepared\$dir\*") -Destination (Join-Path $runtime $dir) -Recurse -Force
    }
    $suite = Join-Path $work "$tfm\cases"
    New-Item -ItemType Directory -Path $suite -Force | Out-Null
    $sources = @((Join-Path $PSScriptRoot 'repro.pas'))
    $sources += @($tests | ForEach-Object { Join-Path $repo "TestSuite\$_.pas" })
    foreach ($source in $sources) {
        $name = [IO.Path]::GetFileName($source)
        $testSource = Join-Path $suite $name
        Copy-Item -LiteralPath $source -Destination $testSource
        $ErrorActionPreference = 'Continue'
        if ($tfm -eq 'net472') { $output = & "$runtime\pabcnetcclear.exe" $testSource 2>&1 }
        else { $output = & dotnet "$runtime\pabcnetcclear.dll" $testSource 2>&1 }
        $ErrorActionPreference = 'Stop'
        if ($LASTEXITCODE -ne 0) { throw "$tfm $name compilation failed: $($output -join [Environment]::NewLine)" }
        $program = [IO.Path]::ChangeExtension($testSource, '.exe')
        $ErrorActionPreference = 'Continue'
        if ($tfm -eq 'net472') { $output = & $program 2>&1 }
        else { $output = & dotnet $program 2>&1 }
        $ErrorActionPreference = 'Stop'
        if ($LASTEXITCODE -ne 0) { throw "$tfm $name execution failed: $($output -join [Environment]::NewLine)" }
        Write-Host "PASS compile/run $tfm $name"
    }
}
Write-Host "Fixtures: $work"
