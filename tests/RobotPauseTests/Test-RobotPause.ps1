[CmdletBinding()]
param([switch]$Gui)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$work = Join-Path $repositoryRoot ('.codex-build\robot-pause-tests\' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $work -Force | Out-Null
$utf8 = New-Object Text.UTF8Encoding($true)
$fieldSource = [IO.File]::ReadAllText((Join-Path $repositoryRoot 'bin\Lib\RobotField.pas'))
$gate = [regex]::Match($fieldSource, '(?s)// BEGIN MODERN ROBOT EXECUTION GATE[^\r\n]*\r?\n(.*?)// END MODERN ROBOT EXECUTION GATE\.')
if (-not $gate.Success) { throw 'Production robot execution gate not found.' }
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
    foreach ($name in @('Robot', 'RobotField')) {
        Copy-Item -LiteralPath (Join-Path $repositoryRoot "bin\Lib\$name.pas") -Destination (Join-Path $runtime "Lib\$name.pas") -Force
        $pcu = Join-Path $runtime "Lib\$name.pcu"
        if (Test-Path -LiteralPath $pcu) { Remove-Item -LiteralPath $pcu }
    }
    $case = Join-Path $work "$target-case"
    New-Item -ItemType Directory -Path $case -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'robot_smoke.pas') -Destination $case
    if ($target -eq 'bin-net10') {
        Copy-Item -LiteralPath (Join-Path $work 'gate_test.pas') -Destination $case
        & dotnet (Join-Path $runtime 'pabcnetcclear.dll') (Join-Path $case 'gate_test.pas')
        if ($LASTEXITCODE -ne 0) { throw 'Gate test compilation failed.' }
        & dotnet (Join-Path $case 'gate_test.exe')
        if ($LASTEXITCODE -ne 0) { throw 'Gate test failed.' }
        & dotnet (Join-Path $runtime 'pabcnetcclear.dll') (Join-Path $case 'robot_smoke.pas')
    }
    else {
        & (Join-Path $runtime 'pabcnetcclear.exe') (Join-Path $case 'robot_smoke.pas')
    }
    if ($LASTEXITCODE -ne 0) { throw "$target robot compilation failed." }
    Write-Host "PASS ${target}: Robot and RobotField compile from edited sources"
    if ($Gui -and $target -eq 'bin-net10') {
        Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'robot_ui_checks.pas') -Destination $case
        & dotnet (Join-Path $runtime 'pabcnetcclear.dll') (Join-Path $case 'robot_ui_checks.pas')
        if ($LASTEXITCODE -ne 0) { throw 'Robot UI test compilation failed.' }
        foreach ($mode in @('', 'close')) {
            $info = New-Object Diagnostics.ProcessStartInfo
            $info.FileName = 'dotnet'
            $info.Arguments = '"' + (Join-Path $case 'robot_ui_checks.exe') + '" ' + $mode
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
                throw 'Robot UI test timed out.'
            }
            Write-Host $stdout.Result
            if ($mode -eq 'close') {
                # GraphABC.OnClosing uses the existing PABCSystem.Halt -> Process.Kill.
                if ($process.ExitCode -notin @(-1, 1) -or $stdout.Result.Trim() -ne 'PASS Robot UI: close while paused' -or $stderr.Result) {
                    throw "Unexpected close result: $($process.ExitCode), $($stdout.Result), $($stderr.Result)"
                }
            }
            elseif ($process.ExitCode -ne 0 -or $stdout.Result -notmatch '^PASS Robot UI: initial pause,' -or $stderr.Result) {
                throw "Robot UI test failed: $($process.ExitCode), $($stdout.Result), $($stderr.Result)"
            }
            $process.Dispose()
        }
    }
}
Write-Host "Robot pause checks passed. Isolated runtimes: $work"
