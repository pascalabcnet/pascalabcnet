[CmdletBinding()]
param([Parameter(Mandatory = $true)][string]$RuntimeDirectory)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$RuntimeDirectory = [IO.Path]::GetFullPath($RuntimeDirectory)
$lib = Join-Path $RuntimeDirectory 'Lib'
$compiler = Join-Path $RuntimeDirectory 'pabcnetcclear.dll'
if (-not (Test-Path -LiteralPath $compiler)) { throw 'Missing modern compiler.' }
if (@(Get-ChildItem -LiteralPath $lib -Filter '*.pas' -Recurse).Count) {
    throw 'Use an installer payload with PCUs in Lib and navigation sources in LibSource.'
}
$modules = @('FormsABC', 'GraphWPF', 'GraphWPFBase', 'RBDMUtils', 'Robot', 'RobotField',
    'RobotTaskMaker', 'RobotZadan', 'TurtleABC', 'WPFObjects', 'Controls', 'Мозаика',
    'PT4Databases', 'PT4Exam', 'PT4MakerNetX', 'PT4TaskMakerNET',
    'DMCollect', 'DMTaskMaker', 'DMZadan', 'Drawman', 'DrawManField')
$work = Join-Path $repositoryRoot ('.codex-build\modern-standard-module-tests\' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $work -Force | Out-Null
foreach ($module in $modules) {
    $pcu = Join-Path $lib "$module.pcu"
    if (-not (Test-Path -LiteralPath $pcu)) { throw "Missing PCU: $module" }
    $before = [Convert]::ToBase64String([IO.File]::ReadAllBytes($pcu))
    $source = Join-Path $work "$module-test.pas"
    [IO.File]::WriteAllText($source, "uses $module; begin end.", (New-Object Text.UTF8Encoding($true)))
    $output = & dotnet $compiler $source '/pcuonly' 2>&1
    if ($LASTEXITCODE -ne 0) { throw "$module PCU consumption failed: $($output -join [Environment]::NewLine)" }
    if ([Convert]::ToBase64String([IO.File]::ReadAllBytes($pcu)) -ne $before) {
        throw "Compilation replaced the packaged PCU: $module"
    }
    Write-Host "PASS installer PCU without Lib sources: $module"
}
