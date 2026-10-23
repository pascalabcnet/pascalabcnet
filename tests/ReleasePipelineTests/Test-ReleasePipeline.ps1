[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$scripts = Join-Path $repositoryRoot 'scripts'
& (Join-Path $PSScriptRoot 'Test-RuntimeResourceCleanup.ps1')
function Check([bool]$Condition, [string]$Message) {
    if (-not $Condition) { throw $Message }
    Write-Host "PASS $Message"
}
function Expect-Failure([scriptblock]$Action, [string]$Pattern) {
    $failure = $null
    try { & $Action } catch { $failure = $_.Exception.Message }
    Check ($null -ne $failure -and $failure -like "*$Pattern*") "rejects $Pattern"
}
function Hash([string]$Path) {
    $stream = [IO.File]::OpenRead($Path)
    $sha = [Security.Cryptography.SHA256]::Create()
    try { return ([BitConverter]::ToString($sha.ComputeHash($stream))).Replace('-', '') }
    finally { $stream.Dispose(); $sha.Dispose() }
}
function Write-Fixture([string]$Path, [string]$Content) {
    New-Item -ItemType Directory -Path (Split-Path -Parent $Path) -Force | Out-Null
    [IO.File]::WriteAllText($Path, $Content)
}

$modernBootstrap = [IO.File]::ReadAllText((Join-Path $repositoryRoot 'ReleaseGenerators\RebuildStandartModulesNet10.pas'))
$modernUses = [regex]::Match($modernBootstrap, '(?is)\buses\s+([^;]+);')
$taskModules = @('LightPT', 'Tasks', 'Tasks1Arr', 'Tasks1Begin', 'Tasks1BoolIfCase',
    'Tasks1Loops', 'TasksArr', 'TasksMatr', 'TasksStr',
    'FormsABC', 'GraphWPF', 'GraphWPFBase', 'RBDMUtils', 'Robot', 'RobotField',
    'RobotTaskMaker', 'RobotZadan', 'TurtleABC', 'WPFObjects', 'Controls', 'Мозаика',
    'PT4Databases', 'PT4Exam', 'PT4MakerNetX', 'PT4TaskMakerNET')
foreach ($module in $taskModules) {
    Check ($modernUses.Success -and @($modernUses.Groups[1].Value.Split(',') | ForEach-Object { $_.Trim() }) -contains $module) "production modern bootstrap includes $module"
}

# Run the real packaging and validation scripts in a tiny disposable checkout.
# No IDE binaries, revision files, GAC entries or existing release packages change.
$fixture = Join-Path $repositoryRoot ('.codex-build\release-pipeline-tests\case with spaces\' + [Guid]::NewGuid().ToString('N'))
$fixtureScripts = Join-Path $fixture 'scripts'
New-Item -ItemType Directory -Path $fixtureScripts -Force | Out-Null
foreach ($file in @('assert-net10-runtime.ps1', 'export-net10-runtime.ps1',
    'build-console-net10-distribution.ps1', 'rebuild-net10-standard-units.ps1',
    'build-net10-ide-installer.ps1')) {
    Copy-Item -LiteralPath (Join-Path $scripts $file) -Destination $fixtureScripts
}
Write-Fixture (Join-Path $fixture 'ReleaseGenerators\RebuildStandartModulesNet10.pas') ('uses PABCSystem, PABCExtensions, __RedirectIOMode, ' + ($taskModules -join ', ') + '; begin end.')
Write-Fixture (Join-Path $fixture 'InstallerSamples\sample.pas') 'begin end.'
Write-Fixture (Join-Path $fixture 'ConsoleDistribution\README.txt') 'fixture'
$sampleTool = Join-Path $fixture 'ReleaseGenerators\Samples\GetSamples.exe'
New-Item -ItemType Directory -Path (Split-Path -Parent $sampleTool) -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $repositoryRoot 'ReleaseGenerators\Samples\GetSamples.exe') -Destination $sampleTool
& $sampleTool '..\..\InstallerSamples' 'Pas'
Check ($LASTEXITCODE -eq 0 -and (Test-Path -LiteralPath (Join-Path $fixture 'ReleaseGenerators\Samples\Pas\sample.pas'))) 'historical GetSamples receives executable-relative paths'
$runtime = Join-Path $fixture 'bin-net10'
$files = @()
$paths = @('Compiler.dll', 'TreeConverter.dll', 'NETGenerator.dll', 'CodeCompletion.dll',
    'PascalABCNet.LanguageServices.dll', 'Lib\PABCSystem.pas', 'Lib\PABCExtensions.pas', 'Lib\__RedirectIOMode.pas')
$paths += @($taskModules | ForEach-Object { "Lib\$_.pas" })
foreach ($entry in @('pabcnetc', 'pabcnetcclear', 'PABCCompilerController', 'PABCCompilerWorker', 'PascalABCNet.LanguageServer')) {
    foreach ($suffix in @('.dll', '.deps.json', '.runtimeconfig.json')) { $paths += $entry + $suffix }
}
foreach ($path in $paths) {
    Write-Fixture (Join-Path $runtime $path) "fixture $path"
    $files += @{ path = $path; sha256 = (Hash (Join-Path $runtime $path)) }
}
$manifestPath = Join-Path $runtime 'net10-runtime.manifest.json'
Write-Fixture $manifestPath (@{ configuration = 'Release'; files = $files } | ConvertTo-Json -Depth 5)
$assert = Join-Path $fixtureScripts 'assert-net10-runtime.ps1'
& $assert -Configuration Release
Expect-Failure { & $assert -Configuration Debug } 'Expected Debug runtime'
Expect-Failure { & $assert -RequireCompiledUnits } 'Missing standard PCU'
Write-Fixture (Join-Path $runtime 'Lib\PABCSystem.pcu') 'compiled fixture'
Write-Fixture (Join-Path $runtime 'Lib\PABCExtensions.pcu') 'compiled fixture'
Expect-Failure { & $assert -RequireCompiledUnits } 'Missing standard PCU'
Write-Fixture (Join-Path $runtime 'Lib\__RedirectIOMode.pcu') 'compiled fixture'
foreach ($module in $taskModules) {
    Expect-Failure { & $assert -RequireCompiledUnits } "$module.pcu"
    Write-Fixture (Join-Path $runtime "Lib\$module.pcu") 'compiled fixture'
}
& $assert -RequireCompiledUnits
Write-Fixture (Join-Path $runtime 'Compiler.dll') 'tampered'
Expect-Failure { & $assert } 'Runtime changed'
Write-Fixture (Join-Path $runtime 'Compiler.dll') 'fixture Compiler.dll'
$cleanManifest = [IO.File]::ReadAllText($manifestPath)
$badManifest = @{ configuration = 'Release'; files = @(@{ path = '..\escape'; sha256 = '' }) }
Write-Fixture $manifestPath ($badManifest | ConvertTo-Json -Depth 5)
Expect-Failure { & $assert } 'Invalid runtime manifest path'
Write-Fixture $manifestPath $cleanManifest

# Any accidentally invoked production build fails the test immediately.
Write-Fixture (Join-Path $fixtureScripts 'build-net10-runtime.ps1') "throw 'Unexpected runtime build'"
Write-Fixture (Join-Path $runtime 'leftover-test.dll') 'must not enter the package'
& (Join-Path $fixtureScripts 'build-console-net10-distribution.ps1') -SkipBuild
$package = Join-Path $fixture 'Release\PascalABCNET-Console-net10'
Check (Test-Path -LiteralPath (Join-Path $fixture 'Release\PascalABCNET-Console-net10.zip')) 'prepared packaging creates a ZIP without building'
Check (-not (Test-Path -LiteralPath (Join-Path $package 'leftover-test.dll'))) 'package excludes unlisted leftovers'
Check (Test-Path -LiteralPath (Join-Path $package 'Lib\PABCSystem.pcu')) 'package includes prepared modern PCUs'
foreach ($module in $taskModules) {
    Check (Test-Path -LiteralPath (Join-Path $package "Lib\$module.pcu")) "package includes precompiled $module"
}
Write-Fixture (Join-Path $package 'obsolete.dll') 'obsolete package file'
& (Join-Path $fixtureScripts 'build-console-net10-distribution.ps1') -SkipBuild
Check (-not (Test-Path -LiteralPath (Join-Path $package 'obsolete.dll'))) 'repackaging removes obsolete staged package files'
Expect-Failure { & (Join-Path $fixtureScripts 'rebuild-net10-standard-units.ps1') -Configuration Debug -SkipBuild } 'Expected Debug runtime'
Check ([IO.File]::ReadAllText((Join-Path $runtime 'Lib\PABCSystem.pcu')) -eq 'compiled fixture') 'failed prepared validation does not delete PCUs'

# Exercise the rebuild path with a simulated compiler, not another compiler kit.
$taskCompilerState = @{ Calls = 0 }
function dotnet {
    param([string]$Assembly, [string]$Source, [string]$Mode)
    Check ($Assembly -eq (Join-Path $runtime 'pabcnetcclear.dll') -and $Source -eq 'RebuildStandartModulesNet10.pas') 'rebuild invokes the prepared clear compiler'
    Check ($Mode -eq '/pcuonly') 'modern standard-unit rebuild disables optimization and binary generation'
    Check (-not (Test-Path -LiteralPath (Join-Path $runtime 'Lib\PABCSystem.pcu'))) 'rebuild removes old PCU caches before compiling'
    $taskCompilerState.Calls++
    Write-Fixture (Join-Path $runtime 'Lib\PABCSystem.pcu') 'recompiled fixture'
    Write-Fixture (Join-Path $runtime 'Lib\PABCExtensions.pcu') 'recompiled fixture'
    Write-Fixture (Join-Path $runtime 'Lib\__RedirectIOMode.pcu') 'recompiled fixture'
    foreach ($module in $taskModules) {
        Write-Fixture (Join-Path $runtime "Lib\$module.pcu") 'recompiled fixture'
    }
    $global:LASTEXITCODE = 0
}
Write-Fixture (Join-Path $runtime 'Lib\user-note.txt') 'not a compiler cache'
& (Join-Path $fixtureScripts 'rebuild-net10-standard-units.ps1') -Configuration Release -SkipBuild
Check ($taskCompilerState.Calls -eq 1) 'prepared standard-unit rebuild calls the compiler once and never builds runtime'
Check (Test-Path -LiteralPath (Join-Path $runtime 'Lib\user-note.txt')) 'rebuild leaves non-PCU files untouched'
Remove-Item Function:\dotnet

Write-Fixture (Join-Path $fixture 'bin\CompileNet10Plugin.dll') 'IDE plugin fixture'
Write-Fixture (Join-Path $fixture 'ReleaseGenerators\PT4\xpt4pabc2.dll') 'PT4 native fixture'
Write-Fixture (Join-Path $fixture 'ReleaseGenerators\PT4\PT4Load.exe') 'PT4 tool fixture'
Write-Fixture (Join-Path $fixture 'ReleaseGenerators\PT4\Lib\Graph\описание.txt') 'PT4 nested fixture'
$payloadOutput = (& (Join-Path $fixtureScripts 'build-net10-ide-installer.ps1') -StageOnly 6>&1 |
    ForEach-Object { $_.ToString() }) -join [Environment]::NewLine
$payloadPath = [regex]::Match($payloadOutput, '(?m)^NET10_INSTALLER_PAYLOAD=(.+)$').Groups[1].Value.Trim()
Check (Test-Path -LiteralPath (Join-Path $payloadPath 'Net10\Compiler.dll')) 'IDE payload contains the shared compiler kit'
foreach ($relative in @('xpt4pabc2.dll', 'PT4Load.exe', 'Lib\Graph\описание.txt')) {
    Check ((Hash (Join-Path $payloadPath "Net10\PT4\$relative")) -eq
        (Hash (Join-Path $fixture "ReleaseGenerators\PT4\$relative"))) "IDE payload duplicates full PT4 tree: $relative"
}
Check (Test-Path -LiteralPath (Join-Path $payloadPath 'Net10\Lib\__RedirectIOMode.pcu')) 'IDE payload includes the precompiled IDE I/O module'
Check (Test-Path -LiteralPath (Join-Path $payloadPath 'Net10\LibSource\PABCSystem.pas')) 'IDE payload separates library sources for navigation'
foreach ($module in $taskModules) {
    Check ((Test-Path -LiteralPath (Join-Path $payloadPath "Net10\Lib\$module.pcu")) -and
           (Test-Path -LiteralPath (Join-Path $payloadPath "Net10\LibSource\$module.pas"))) "IDE payload includes $module PCU and navigation source"
}
Check (@(Get-ChildItem -LiteralPath (Join-Path $payloadPath 'Net10\Lib') -Recurse -File -Filter '*.pas').Count -eq 0) 'IDE Lib contains no Pascal sources to recompile'
& $assert -RuntimeDirectory (Join-Path $payloadPath 'Net10') -RequireCompiledUnits
Check (-not (Test-Path -LiteralPath (Join-Path $payloadPath 'Net10\leftover-test.dll'))) 'IDE payload excludes test leftovers'
Check (-not (Test-Path -LiteralPath (Join-Path $payloadPath 'Net10\CompileNet10Plugin.dll'))) 'IDE plugin is not duplicated in the modern compiler kit'
$includeText = [IO.File]::ReadAllText((Join-Path $payloadPath 'net10-payload.nsh'))
Check ($includeText.Contains('Delete "$INSTDIR\Net10\Lib\PABCSystem.pas"')) 'upgrade removes the exact old-layout library source'
$installedFiles = @(Get-ChildItem -LiteralPath (Join-Path $payloadPath 'Net10') -Recurse -File)
Check ([regex]::Matches($includeText, '(?m)^File "').Count -eq $installedFiles.Count -and
    [regex]::Matches($includeText, '(?m)^\$\{AddFile\} "').Count -eq $installedFiles.Count) 'every modern installer file has an uninstall-log entry'
$iniText = [IO.File]::ReadAllText((Join-Path $payloadPath 'CompileNet10Plugin.ini'))
Check ($iniText.Contains('RuntimeDirectory=Net10') -and -not $iniText.Contains('..\bin-net10')) 'installed plugin uses the private INSTDIR kit, not the repository layout'

# The same dry-run graph is available through the public release entrypoint.
$allPlan = & (Join-Path $scripts 'generate-all-setups.ps1') -Plan -NoVersionIncrement 6>&1 | Out-String
Check ([regex]::Matches($allPlan, 'RELEASE STEP: Build shared .NET 10 runtime once').Count -eq 1) 'full graph builds modern compiler once'
Check ([regex]::Matches($allPlan, 'RELEASE STEP: Editor regression: Enter, file loading and caret in both platform modes').Count -eq 1) 'full graph checks editor Enter, loading and caret before packaging'
Check ([regex]::Matches($allPlan, 'RELEASE STEP: Cross-target tests using prepared compiler binaries: net10').Count -eq 1) 'full graph runs the full modern suite once'
Check ([regex]::Matches($allPlan, 'RELEASE STEP: Additional net472 reference checks using prepared compiler binaries').Count -eq 1) 'full graph preserves additional legacy reference checks once'
Check ($allPlan -notmatch 'binaries: (net472|all)') 'full graph does not repeat the full legacy suite'
Check ($allPlan -notmatch 'RELEASE STEP DONE|RELEASE STEP FAILED|RELEASE TOTAL|С НАЧАЛА ЗАПУСКА СКРИПТА:') 'Plan prints no invented execution times'

# Inspect actual invocation arguments too: a dry-run label alone cannot prove
# that the legacy runner is filtered or that runtime rebuilding is disabled.
$releaseAst = [System.Management.Automation.Language.Parser]::ParseFile(
    (Join-Path $scripts 'generate-all-setups.ps1'), [ref]$null, [ref]$null)
$crossCalls = @($releaseAst.FindAll({ param($node)
    $node -is [System.Management.Automation.Language.CommandAst] -and
    $node.GetCommandName() -eq 'Invoke-Checked' -and
    $node.CommandElements[1].Extent.Text.Contains("'_RunCrossTargetTests.bat'")
}, $true))
Check ($crossCalls.Count -eq 2) 'release has exactly two explicit cross-target invocations'
Check (($crossCalls[0].CommandElements[2].SafeGetValue() -join '|') -eq 'net472|units|reference|--skip-build') 'legacy cross-target invocation runs only reference unit checks without rebuilding runtime'
Check (($crossCalls[1].CommandElements[2].SafeGetValue() -join '|') -eq 'net10|all|--skip-build') 'modern cross-target invocation keeps the full suite without rebuilding runtime'
$standardCalls = @($releaseAst.FindAll({ param($node)
    $node -is [System.Management.Automation.Language.CommandAst] -and
    $node.GetCommandName() -eq 'Invoke-Checked' -and
    $node.CommandElements.Count -gt 2 -and
    $node.CommandElements[2].Extent.Text -match 'RebuildStandartModules.*\.pas'
}, $true))
Check ($standardCalls.Count -eq 4) 'both legacy standard-module passes include Pascal and SPython bootstraps'
foreach ($call in $standardCalls) {
    Check ($call.CommandElements[2].SafeGetValue() -contains '/pcuonly') 'legacy standard-module invocation disables EXE generation'
}
$rtlCalls = @($releaseAst.FindAll({ param($node)
    $node -is [System.Management.Automation.Language.CommandAst] -and
    $node.GetCommandName() -eq 'Invoke-Checked' -and
    $node.CommandElements.Count -gt 2 -and
    $node.CommandElements[2].Extent.Text -match "'PABCRtl\.pas'"
}, $true))
Check ($rtlCalls.Count -eq 1 -and $rtlCalls[0].CommandElements[2].SafeGetValue() -notcontains '/pcuonly') 'PABCRtl remains an ordinary DLL build'
foreach ($stage in @('Test', 'Package')) {
    $plan = & (Join-Path $scripts 'generate-all-setups.ps1') -Stage $stage -Plan 6>&1 | Out-String
    Check ($plan -notmatch 'Prepare version|Build shared|Build legacy IDE|Rebuild modern') "$stage does not rebuild production binaries or modules"
}
# Dot-sourcing the Plan exposes the same checked step functions without side effects.
$legacyPlan = & (Join-Path $scripts 'generate-all-setups.ps1') -Target NetFramework -Plan 6>&1 | Out-String
Check ($legacyPlan -notmatch 'Build shared|Build IDE plugin|Rebuild modern|Package modern|Package dual-target') 'NetFramework plan excludes modern preparation and packaging'
Check ($legacyPlan -match 'Additional net472 reference checks' -and $legacyPlan -notmatch 'Cross-target tests using prepared compiler binaries:') 'NetFramework keeps only additional reference checks after the full legacy runner'
Push-Location $fixture
try {
    $wrapperPlan = & (Join-Path $repositoryRoot '_GenerateNetFrameworkSetup.bat') -Stage Package -Plan 2>&1 | Out-String
    Check ($LASTEXITCODE -eq 0 -and $wrapperPlan.Contains('Package (NetFramework)') -and
        $wrapperPlan -notmatch 'Package modern|Package dual-target') 'NetFramework BAT forwards options and works from another working directory'
}
finally { Pop-Location }
$previousStep = -1
foreach ($name in @('Prepare installer samples', 'Prepare version once', 'Build legacy IDE and compiler once',
    'Bootstrap legacy modules before PABCRtl', 'Build, sign and register legacy PABCRtl',
    'Prepare final legacy modules after PABCRtl', 'Build full legacy test runner using prepared compiler',
    'Legacy tests: TestSuite, passes 1-6', 'Legacy tests: TestSuiteAdditionalLanguages\SPythonTests, passes 1-6',
    'Additional net472 reference checks using prepared compiler binaries',
    'Package existing legacy installers and console ZIP')) {
    $position = $legacyPlan.IndexOf('RELEASE STEP: ' + $name, [StringComparison]::Ordinal)
    Check ($position -gt $previousStep) "NetFramework preserves old release step order: $name"
    $previousStep = $position
}
# Run the actual legacy Package branch in isolation: no modern payload/plugin exists here.
Copy-Item -LiteralPath (Join-Path $scripts 'generate-all-setups.ps1') -Destination $fixtureScripts
foreach ($file in @('PascalABCNET.exe', 'pabcnetc.exe', 'pabcnetcclear.exe', 'PluginsSupport.dll', 'Lib\PABCRtl.dll')) {
    Write-Fixture (Join-Path $fixture "bin\$file") 'legacy fixture'
}
Write-Fixture (Join-Path $fixtureScripts 'assert-net10-runtime.ps1') "throw 'Unexpected modern runtime validation'"
Write-Fixture (Join-Path $fixtureScripts 'build-console-net10-distribution.ps1') "throw 'Unexpected modern ZIP packaging'"
Write-Fixture (Join-Path $fixtureScripts 'build-net10-ide-installer.ps1') "throw 'Unexpected modern installer packaging'"
Write-Fixture (Join-Path $fixture 'ReleaseGenerators\PascalABCNET_ALL.bat') "@echo off`r`necho LEGACY_PACKAGE_CALLED`r`nexit /b 0`r`n"
Remove-Item -LiteralPath (Join-Path $fixture 'bin\CompileNet10Plugin.dll')
$legacyPackage = & (Join-Path $fixtureScripts 'generate-all-setups.ps1') -Target NetFramework -Stage Package 6>&1 | Out-String
Check ($legacyPackage.Contains('LEGACY_PACKAGE_CALLED')) 'NetFramework packaging works without modern plugin or runtime validation'
Check ($legacyPackage -match 'RELEASE STEP DONE: Validate prepared release for packaging \| время этапа: \d+ мин \d+ с') 'successful validation reports elapsed time'
Check ($legacyPackage -match 'RELEASE STEP DONE: Package existing legacy installers and console ZIP \| время этапа: \d+ мин \d+ с') 'successful packaging reports elapsed time'
Check ($legacyPackage -match 'RELEASE TOTAL DONE: Package \(NetFramework\) \| общее время: \d+ мин \d+ с') 'NetFramework entrypoint reports total elapsed time'
Check ([regex]::Matches($legacyPackage, '(?m)^={78}\r?\nС НАЧАЛА ЗАПУСКА СКРИПТА: \d+ мин \d+ с\r?\n={78}\r?\nRELEASE STEP:').Count -eq 2) 'visible cumulative-time banner appears before each step'

# Exercise real error unwinding without building/installing anything.
Write-Fixture (Join-Path $fixture 'ReleaseGenerators\PascalABCNET_ALL.bat') "@echo off`r`nexit /b 29`r`n"
$script:failedPackageMessage = ''
$failedPackage = & {
    try { & (Join-Path $fixtureScripts 'generate-all-setups.ps1') -Target NetFramework -Stage Package }
    catch { $script:failedPackageMessage = $_.Exception.Message }
} 6>&1 | Out-String
Check ($failedPackageMessage -like '*exit code 29*') 'timing preserves the original packaging failure'
Check ($failedPackage -match 'RELEASE STEP FAILED: Package existing legacy installers and console ZIP \| время этапа: \d+ мин \d+ с') 'failed packaging still reports elapsed time'
Check ($failedPackage -match 'RELEASE TOTAL FAILED: Package \(NetFramework\) \| общее время: \d+ мин \d+ с' -and
    $failedPackage -notmatch 'Release stage completed:') 'failed release reports total time without claiming success'

# Cover the dual-target entrypoint too, still using only tiny isolated files.
Copy-Item -LiteralPath (Join-Path $scripts 'assert-net10-runtime.ps1') -Destination $fixtureScripts -Force
Write-Fixture (Join-Path $fixture 'bin\CompileNet10Plugin.dll') 'IDE plugin fixture'
Write-Fixture (Join-Path $fixture 'bin\CompileNet10Plugin.ini') 'IDE plugin settings fixture'
Write-Fixture (Join-Path $fixture 'ReleaseGenerators\PascalABCNET_ALL.bat') "@echo off`r`nexit /b 0`r`n"
Write-Fixture (Join-Path $fixtureScripts 'build-console-net10-distribution.ps1') 'param([switch]$SkipBuild)'
Write-Fixture (Join-Path $fixtureScripts 'build-net10-ide-installer.ps1') '# No real installer compilation in this fixture.'
$allPackage = & (Join-Path $fixtureScripts 'generate-all-setups.ps1') -Target All -Stage Package 6>&1 | Out-String
Check ([regex]::Matches($allPackage, 'RELEASE STEP DONE:').Count -eq 4) 'dual-target packaging reports elapsed time for every step'
Check ($allPackage -match 'RELEASE TOTAL DONE: Package \(All\) \| общее время: \d+ мин \d+ с') 'dual-target entrypoint reports total elapsed time'

. (Join-Path $scripts 'generate-all-setups.ps1') -Stage Package -Plan
Check ((Format-ReleaseDuration ([TimeSpan]::FromMilliseconds(3723456))) -eq '62 мин 3 с') 'elapsed-time format uses total minutes and seconds'
Check ((Format-ReleaseDuration ([TimeSpan]::FromHours(25))) -eq '1500 мин 0 с') 'elapsed-time format does not wrap after 24 hours'
$Plan = $false
$releaseBuildTimer = [Diagnostics.Stopwatch]::StartNew()
$timedStep = & { Invoke-ReleaseStep 'timing output fixture' { Write-Output 'ACTION_OUTPUT_PRESERVED' } } 6>&1 | Out-String
Check ($timedStep.Contains('ACTION_OUTPUT_PRESERVED') -and $timedStep -match 'RELEASE STEP DONE: timing output fixture \| время этапа:') 'step timing preserves action output'
$beforeTests = & { Invoke-ReleaseStep 'Legacy tests: TestSuite, passes 1-6' {} } 6>&1 | Out-String
Check ($beforeTests -match '(?m)^={78}\r?\nС НАЧАЛА ЗАПУСКА СКРИПТА: \d+ мин \d+ с\r?\n={78}\r?\nRELEASE STEP: Legacy tests: TestSuite, passes 1-6') 'cumulative time is directly before the legacy tests heading'
Expect-Failure { Invoke-Checked 'cmd.exe' @('/d', '/c', 'exit 37') } 'exit code 37'
$script:reachedPackaging = $false
$stepFailed = $false
try {
    Invoke-ReleaseStep 'deliberately failing test' { Invoke-Checked 'cmd.exe' @('/d', '/c', 'exit 29') }
    Invoke-ReleaseStep 'must not package' { $script:reachedPackaging = $true }
}
catch { $stepFailed = $_.Exception.Message -like '*exit code 29*' }
Check ($stepFailed -and -not $script:reachedPackaging) 'a failed step prevents subsequent packaging'
Write-Host "All release pipeline checks passed. Fixtures: $fixture"
