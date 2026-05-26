param(
  [string]$Root = 'D:\PABC_GIT\TestSuite\_MachineLearning',
  [switch]$Fast
)

[Console]::InputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$OutputEncoding = [System.Text.Encoding]::UTF8

$compiler = 'D:\PABC_GIT\bin\pabcnetcclear.exe'
$libRoot = 'D:\PABC_GIT\bin\Lib'
$tests = Get-ChildItem -Path $Root -Recurse -Filter *.pas | Where-Object { $_.Name -ne 'TestHelpers.pas' } | Sort-Object FullName
$libStamp = (Get-ChildItem -Path $libRoot -Recurse -Filter *.pas | Measure-Object LastWriteTime -Maximum).Maximum
$testHelpers = Join-Path $Root 'TestHelpers.pas'
$helperStamp = if (Test-Path $testHelpers) { (Get-Item $testHelpers).LastWriteTime } else { Get-Date '2000-01-01' }

if ($tests.Count -eq 0) {
  Write-Host 'No tests found.'
  exit 0
}

$failed = @()
$passedCount = 0
$progressOnLine = 0

Write-Host "Found tests: $($tests.Count)"
if ($Fast) {
  Write-Host 'Mode: FAST'
}
Write-Host ''

foreach ($test in $tests) {
  $exe = [System.IO.Path]::ChangeExtension($test.FullName, '.exe')
  $pdb = [System.IO.Path]::ChangeExtension($test.FullName, '.pdb')

  $needCompile = $true
  if ($Fast -and (Test-Path $exe)) {
    $exeStamp = (Get-Item $exe).LastWriteTime
    if (($exeStamp -ge $test.LastWriteTime) -and ($exeStamp -ge $libStamp) -and ($exeStamp -ge $helperStamp)) {
      $needCompile = $false
    }
  }

  if ($needCompile) {
    $compileOut = & $compiler "/SearchDir:$Root" $test.FullName 2>&1
    if ($LASTEXITCODE -ne 0) {
      if ($progressOnLine -gt 0) {
        Write-Host ''
        $progressOnLine = 0
      }
      Write-Host "=== $($test.FullName) ==="
      if ($compileOut) { $compileOut | ForEach-Object { Write-Host $_ } }
      Write-Host 'COMPILE FAIL'
      Write-Host ''
      $failed += "$($test.FullName) [compile]"
      continue
    }
  }

  if (-not (Test-Path $exe)) {
    if ($progressOnLine -gt 0) {
      Write-Host ''
      $progressOnLine = 0
    }
    Write-Host "=== $($test.FullName) ==="
    Write-Host 'EXE NOT FOUND'
    Write-Host ''
    $failed += "$($test.FullName) [no exe]"
    continue
  }

  $runOut = & $exe 2>&1
  $runCode = $LASTEXITCODE

  if (-not $Fast) {
    if (Test-Path $exe) { Remove-Item -LiteralPath $exe -Force }
    if (Test-Path $pdb) { Remove-Item -LiteralPath $pdb -Force }
  }

  if ($runCode -ne 0) {
    if ($progressOnLine -gt 0) {
      Write-Host ''
      $progressOnLine = 0
    }
    Write-Host "=== $($test.FullName) ==="
    if ($runOut) { $runOut | ForEach-Object { Write-Host $_ } }
    Write-Host 'RUN FAIL'
    Write-Host ''
    $failed += "$($test.FullName) [run]"
    continue
  }

  $passedCount += 1
  Write-Host -NoNewline '.'
  $progressOnLine += 1
  if ($progressOnLine -ge 10) {
    Write-Host ''
    $progressOnLine = 0
  }
}

if ($progressOnLine -gt 0) {
  Write-Host ''
}

Write-Host ''
Write-Host 'SUMMARY'
Write-Host "  Total : $($tests.Count)"
Write-Host "  Passed: $passedCount"
Write-Host "  Failed: $($failed.Count)"

if ($failed.Count -gt 0) {
  Write-Host ''
  Write-Host 'FAILED TESTS:'
  $failed | ForEach-Object { Write-Host $_ }
  exit 1
}

Write-Host ''
Write-Host 'ALL TESTS PASSED'
exit 0
