[CmdletBinding()]
param([switch]$Gui)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$work = Join-Path $repositoryRoot ('.codex-build\drawman-pause-tests\' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $work -Force | Out-Null
$utf8 = New-Object Text.UTF8Encoding($true)
$fieldSource = [IO.File]::ReadAllText((Join-Path $repositoryRoot 'bin\Lib\DrawmanField.pas'))
$gate = [regex]::Match($fieldSource, '(?s)// BEGIN MODERN DRAWMAN EXECUTION GATE[^\r\n]*\r?\n(.*?)// END MODERN DRAWMAN EXECUTION GATE\.')
if (-not $gate.Success) { throw 'Production drawman execution gate not found.' }
$checks = [IO.File]::ReadAllText((Join-Path $PSScriptRoot 'gate_checks.pas'))
[IO.File]::WriteAllText((Join-Path $work 'gate_test.pas'), $gate.Groups[1].Value + $checks, $utf8)

foreach ($target in @('bin', 'bin-net10')) {
    $runtime = Join-Path $work $target
    New-Item -ItemType Directory -Path $runtime -Force | Out-Null
    Get-ChildItem -LiteralPath (Join-Path $repositoryRoot $target) -File |
        Where-Object { $_.Extension -ne '.pdb' } | Copy-Item -Destination $runtime
    foreach ($directory in @('Lib', 'Lng')) {
        Copy-Item -LiteralPath (Join-Path $repositoryRoot "$target\$directory") -Destination $runtime -Recurse
    }
    # Compile the edited common sources, not a PCU from an earlier release.
    foreach ($name in @('Drawman', 'DrawManField')) {
        Copy-Item -LiteralPath (Join-Path $repositoryRoot "bin\Lib\$name.pas") -Destination (Join-Path $runtime "Lib\$name.pas") -Force
        $pcu = Join-Path $runtime "Lib\$name.pcu"
        if (Test-Path -LiteralPath $pcu) { Remove-Item -LiteralPath $pcu }
    }
    $case = Join-Path $work "$target-case"
    New-Item -ItemType Directory -Path $case -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'drawman_smoke.pas') -Destination $case
    if ($target -eq 'bin-net10') {
        Copy-Item -LiteralPath (Join-Path $work 'gate_test.pas') -Destination $case
        & dotnet (Join-Path $runtime 'pabcnetcclear.dll') (Join-Path $case 'gate_test.pas')
        if ($LASTEXITCODE -ne 0) { throw 'Gate test compilation failed.' }
        & dotnet (Join-Path $case 'gate_test.exe')
        if ($LASTEXITCODE -ne 0) { throw 'Gate test failed.' }
        & dotnet (Join-Path $runtime 'pabcnetcclear.dll') (Join-Path $case 'drawman_smoke.pas')
    }
    else {
        & (Join-Path $runtime 'pabcnetcclear.exe') (Join-Path $case 'drawman_smoke.pas')
    }
    if ($LASTEXITCODE -ne 0) { throw "$target drawman compilation failed." }
    Write-Host "PASS ${target}: Drawman and DrawmanField compile from edited sources"
    if ($Gui -and $target -eq 'bin-net10') {
        Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'drawman_ui_checks.pas') -Destination $case
        & dotnet (Join-Path $runtime 'pabcnetcclear.dll') (Join-Path $case 'drawman_ui_checks.pas')
        if ($LASTEXITCODE -ne 0) { throw 'Drawman UI test compilation failed.' }
        foreach ($mode in @('', 'close')) {
            $info = New-Object Diagnostics.ProcessStartInfo
            $info.FileName = 'dotnet'
            $info.Arguments = '"' + (Join-Path $case 'drawman_ui_checks.exe') + '" ' + $mode
            $info.WorkingDirectory = $case
            $info.UseShellExecute = $false
            $info.CreateNoWindow = $true
            $info.RedirectStandardOutput = $true
            $info.RedirectStandardError = $true
            $process = [Diagnostics.Process]::Start($info)
            $stdout = $process.StandardOutput.ReadToEndAsync()
            $stderr = $process.StandardError.ReadToEndAsync()
            if (-not $process.WaitForExit(45000)) {
                $process.Kill()
                throw 'Drawman UI test timed out.'
            }
            Write-Host $stdout.Result
            if ($mode -eq 'close') {
                # GraphABC.OnClosing uses the existing PABCSystem.Halt -> Process.Kill.
                if ($process.ExitCode -notin @(-1, 1) -or $stdout.Result.Trim() -ne 'PASS Drawman UI: close while paused' -or $stderr.Result) {
                    throw "Unexpected close result: $($process.ExitCode), $($stdout.Result), $($stderr.Result)"
                }
            }
            elseif ($process.ExitCode -ne 0 -or $stdout.Result -notmatch '^PASS Drawman UI: initial pause,' -or $stderr.Result) {
                throw "Drawman UI test failed: $($process.ExitCode), $($stdout.Result), $($stderr.Result)"
            }
            $process.Dispose()
        }
    }
}
Write-Host "Drawman pause checks passed. Isolated runtimes: $work"
