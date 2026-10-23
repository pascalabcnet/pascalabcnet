[CmdletBinding()]
param()
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$work = Join-Path $repositoryRoot ('.codex-build\messagebox-lookup\' + [Guid]::NewGuid().ToString('N'))
foreach ($target in @('net472', 'net10')) {
    foreach ($name in @('forms_only', 'forms_then_wpf', 'wpf_then_forms', 'native_then_forms', 'original_robot', 'original_pt4', 'mixed_robot', 'mixed_pt4')) {
        $case = Join-Path $work "$target\$name"
        New-Item -ItemType Directory -Path $case -Force | Out-Null
        Copy-Item -Path (Join-Path $PSScriptRoot '*.pas') -Destination $case
        $source = Join-Path $case "$name.pas"
        if ($name -match '^(original|mixed)_(robot|pt4)$') {
            $unitName = if ($Matches[2] -eq 'robot') { 'RobotField' } else { 'PT4MakerNetX' }
            $original = [IO.File]::ReadAllText((Join-Path $repositoryRoot "bin\Lib\$unitName.pas"))
            $original = $original.Replace('System.Windows.Forms.MessageBox.Show', 'MessageBox.Show')
            [IO.File]::WriteAllText((Join-Path $case "$unitName.pas"), $original, (New-Object Text.UTF8Encoding($true)))
            $prefix = if ($name.StartsWith('mixed')) { 'FormsABC, GraphWPF, GraphWPFBase, ' } else { '' }
            [IO.File]::WriteAllText($source, "uses $prefix$unitName in '$unitName.pas'; begin end.")
        }
        if ($target -eq 'net472') {
            $output = & (Join-Path $repositoryRoot 'bin\pabcnetcclear.exe') $source '/pcuonly' 2>&1
        } else {
            $output = & dotnet (Join-Path $repositoryRoot 'bin-net10\pabcnetcclear.dll') $source '/pcuonly' 2>&1
        }
        $exitCode = $LASTEXITCODE
        if ($exitCode -ne 0) { throw "$target $name failed: $($output -join [Environment]::NewLine)" }
        Write-Host "PASS $target $name"
    }
}
foreach ($name in @('cold_robot', 'cold_pt4', 'cold_bootstrap', 'preloaded_bootstrap', 'qualified_bootstrap')) {
    $case = Join-Path $work $name
    & (Join-Path $repositoryRoot 'scripts\export-net10-runtime.ps1') -Destination $case
    foreach ($unitName in @('RobotField', 'PT4MakerNetX')) {
        $path = Join-Path $case "Lib\$unitName.pas"
        $original = [IO.File]::ReadAllText($path).Replace('System.Windows.Forms.MessageBox.Show', 'MessageBox.Show')
        if ($name -eq 'qualified_bootstrap') {
            $original = $original.Replace('MessageBox.Show', 'System.Windows.Forms.MessageBox.Show')
        }
        [IO.File]::WriteAllText($path, $original, (New-Object Text.UTF8Encoding($true)))
    }
    if ($name.EndsWith('bootstrap')) {
        $program = [IO.File]::ReadAllText((Join-Path $repositoryRoot 'ReleaseGenerators\RebuildStandartModulesNet10.pas'))
        if ($name -eq 'preloaded_bootstrap') {
            $program = '{$reference System.Windows.Forms.dll}' + [Environment]::NewLine + $program
        }
    } else {
        $unitName = if ($name.EndsWith('robot')) { 'RobotField' } else { 'PT4MakerNetX' }
        $prefix = if ($name.Contains('mixed')) { 'FormsABC, GraphWPF, GraphWPFBase, ' } else { '' }
        $program = "uses $prefix$unitName; begin end."
    }
    $source = Join-Path $case 'probe.pas'
    [IO.File]::WriteAllText($source, $program, (New-Object Text.UTF8Encoding($true)))
    $output = & dotnet (Join-Path $case 'pabcnetcclear.dll') $source '/pcuonly' 2>&1
    $exitCode = $LASTEXITCODE
    if ($exitCode -ne 0) { throw "$name failed: $($output -join [Environment]::NewLine)" }
    Write-Host "PASS $name"
}
Write-Host "Fixtures: $work"
