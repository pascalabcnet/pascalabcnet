[CmdletBinding()]
param()
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../..'))
$fixture = Join-Path $repositoryRoot ('.codex-build/gitignore-tester-tests/' + [Guid]::NewGuid().ToString('N'))
$bin = Join-Path $fixture 'bin'
New-Item -ItemType Directory -Path $bin -Force | Out-Null
$source = Join-Path $bin 'GitIgnoreTester.pas'
Copy-Item -LiteralPath (Join-Path $repositoryRoot 'bin/GitIgnoreTester.pas') -Destination $source
[IO.File]::WriteAllText((Join-Path $fixture '.gitignore'), "/bin/GitIgnoreTester.exe`n/bin/GitIgnoreTester.pdb`n/bin/GitIgnoreTester.exe.config`n*.tmp`n")
Push-Location $fixture
try {
    & git init -q
    if ($LASTEXITCODE -ne 0) { throw 'git init failed' }
    & git add -- .gitignore bin/GitIgnoreTester.pas
    if ($LASTEXITCODE -ne 0) { throw 'git add failed' }
    $ErrorActionPreference = 'Continue'
    $build = & (Join-Path $repositoryRoot 'bin/pabcnetcclear.exe') $source 2>&1
    $buildExit = $LASTEXITCODE
    $ErrorActionPreference = 'Stop'
    if ($buildExit -ne 0) { throw "Tester compilation failed: $build" }
    $tester = Join-Path $bin 'GitIgnoreTester.exe'
    function Check([string]$Name, [string[]]$Arguments, [int]$ExpectedExit, [string]$Pattern) {
        $ErrorActionPreference = 'Continue'
        $output = & $tester @Arguments 2>&1
        $code = $LASTEXITCODE
        $text = $output -join "`n"
        if ($code -ne $ExpectedExit -or ($Pattern -and $text -notmatch $Pattern)) {
            throw "$Name failed (exit $code, expected $ExpectedExit): $text"
        }
        Write-Host "PASS $Name"
    }
    Check 'clean strict' @('NoWait') 0 ''
    Check 'clean warnings' @('NoWait','WarningsOnly','GitHubActions') 0 ''
    [IO.File]::WriteAllText((Join-Path $fixture 'unexpected%25.txt'), 'fixture')
    Check 'untracked file remains strict by default' @('NoWait') 1 'Unversioned files'
    Check 'warnings with escaped annotation data' @('NoWait','WarningsOnly','GitHubActions') 0 '::warning title=Repository contents::.*unexpected%2525\.txt'
    Check 'explicit strict overrides warnings' @('NoWait','WarningsOnly','Strict','GitHubActions') 1 '::error title=Repository contents::'
    [IO.File]::WriteAllText((Join-Path $fixture 'tracked.tmp'), 'fixture')
    & git add -f -- tracked.tmp
    if ($LASTEXITCODE -ne 0) { throw 'git add -f failed' }
    Check 'ignored tracked file is reported' @('NoWait','WarningsOnly','GitHubActions') 0 'Ignored index files'
    $oldGitDir = [Environment]::GetEnvironmentVariable('GIT_DIR', 'Process')
    try {
        $env:GIT_DIR = Join-Path $fixture 'missing-git-directory'
        Check 'git execution failure is not hidden' @('NoWait','WarningsOnly','GitHubActions') 1 '::error title=Repository checker failed::'
    } finally {
        [Environment]::SetEnvironmentVariable('GIT_DIR', $oldGitDir, 'Process')
    }
} finally {
    Pop-Location
}
