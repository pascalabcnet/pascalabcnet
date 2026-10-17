[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)][string]$PayloadDirectory,
    [string]$NsisCompiler = ''
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$PayloadDirectory = [IO.Path]::GetFullPath($PayloadDirectory)
if (-not $NsisCompiler) { $NsisCompiler = Join-Path $repositoryRoot 'Utils\NSIS2\Bin\makensis.exe' }
$stage = Join-Path $repositoryRoot ('.codex-build\installer-payload-tests\' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $stage -Force | Out-Null
$output = Join-Path $stage 'Net10PayloadTests.exe'
Push-Location $PSScriptRoot
try {
    & $NsisCompiler /V3 /INPUTCHARSET UTF8 "/DTEST_OUTPUT=$output" "/DPABC_NET10_PAYLOAD_DIR=$PayloadDirectory" 'Net10PayloadTests.nsi'
    if ($LASTEXITCODE -ne 0) { throw "NSIS payload test compilation failed: $LASTEXITCODE" }
}
finally { Pop-Location }
function Run-TestExecutable([string]$Path) {
    $process = Start-Process -FilePath $Path -ArgumentList '/S' -WindowStyle Hidden -Wait -PassThru
    if ($process.ExitCode -ne 0) { throw "Test executable failed: $Path" }
}
# Only this test section is installed: no product GAC, associations or shortcuts.
$installed = Join-Path $stage 'installed'
$ordinaryPt4Note = Join-Path $installed 'PT4\ordinary-copy.txt'
New-Item -ItemType Directory -Path (Split-Path -Parent $ordinaryPt4Note) -Force | Out-Null
[IO.File]::WriteAllText($ordinaryPt4Note, 'Keep the ordinary PT4 installation.')
Run-TestExecutable $output
$log = Get-Content -LiteralPath (Join-Path $installed 'uninstall.log')
$files = @(Get-ChildItem -LiteralPath (Join-Path $PayloadDirectory 'Net10') -Recurse -File)
if ($log -notcontains (Join-Path $installed 'CompileNet10Plugin.dll') -or
    (Get-FileHash -LiteralPath (Join-Path $installed 'CompileNet10Plugin.dll')).Hash -ne
    (Get-FileHash -LiteralPath (Join-Path $PayloadDirectory 'CompileNet10Plugin.dll')).Hash) {
    throw 'IDE plugin is missing, changed or unlogged.'
}
foreach ($file in $files) {
    $relative = $file.FullName.Substring($PayloadDirectory.Length + 1)
    $target = Join-Path $installed $relative
    if (-not (Test-Path -LiteralPath $target) -or $log -notcontains $target -or
        (Get-FileHash -LiteralPath $target).Hash -ne (Get-FileHash -LiteralPath $file.FullName).Hash) {
        throw "Missing, changed or unlogged installed file: $relative"
    }
}
if (-not ([IO.File]::ReadAllText((Join-Path $installed 'localized-label.txt')).Contains('Открыть страницу Microsoft'))) {
    throw 'Russian installer text is damaged.'
}
Write-Host "PASS installed, logged and checked $($files.Count) modern files, including Cyrillic names."
if (-not (Test-Path -LiteralPath (Join-Path $installed 'Net10\PT4\xpt4pabc2.dll'))) { throw 'Modern PT4 native DLL is missing.' }
# Fixture data only: test preservation of existing settings and unlisted user files.
$ini = Join-Path $installed 'CompileNet10Plugin.ini'
if (-not ([IO.File]::ReadAllText($ini).Contains('RuntimeDirectory=Net10'))) { throw 'Fresh installation has the wrong compiler-kit path.' }
[IO.File]::WriteAllText($ini, 'RuntimeDirectory=Custom')
[IO.File]::WriteAllText((Join-Path $installed 'user-note.txt'), 'Preserve this user file.')
$modernPt4Note = Join-Path $installed 'Net10\PT4\user-note.txt'
[IO.File]::WriteAllText($modernPt4Note, 'Preserve unlisted PT4 user data.')
# Simulate the tracked source path left by the previous installer layout.
$oldLibrarySource = Join-Path $installed 'Net10\Lib\PABCSystem.pas'
[IO.File]::WriteAllText($oldLibrarySource, 'old installer source')
Run-TestExecutable $output
if (Test-Path -LiteralPath $oldLibrarySource) { throw 'Upgrade left an old library source searchable in Lib.' }
if ([IO.File]::ReadAllText($ini) -ne 'RuntimeDirectory=Custom') { throw 'Reinstallation overwrote plugin settings.' }
Run-TestExecutable (Join-Path $installed 'payload-uninstall.exe')
if (Test-Path -LiteralPath (Join-Path $installed 'CompileNet10Plugin.dll')) { throw 'Uninstall left an active IDE plugin behind.' }
foreach ($file in $files) {
    if (Test-Path -LiteralPath (Join-Path $installed $file.FullName.Substring($PayloadDirectory.Length + 1))) {
        throw "Installer-owned file remains: $($file.Name)"
    }
}
if (-not (Test-Path -LiteralPath (Join-Path $installed 'user-note.txt'))) { throw 'Uninstaller removed an unlisted user file.' }
if ([IO.File]::ReadAllText($ini) -ne 'RuntimeDirectory=Custom') { throw 'Uninstaller removed user plugin settings.' }
if ([IO.File]::ReadAllText($ordinaryPt4Note) -ne 'Keep the ordinary PT4 installation.' -or
    [IO.File]::ReadAllText($modernPt4Note) -ne 'Preserve unlisted PT4 user data.') {
    throw 'Modern payload modified the ordinary PT4 copy or removed unlisted PT4 user data.'
}
Write-Host "PASS reinstallation and payload uninstall preserve user data. Fixtures: $stage"
