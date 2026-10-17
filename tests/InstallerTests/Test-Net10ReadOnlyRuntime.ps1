[CmdletBinding()]
param([Parameter(Mandatory = $true)][string]$RuntimeDirectory)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$stage = Join-Path $repositoryRoot ('.codex-build\installer-readonly-tests\' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $stage -Force | Out-Null
$runtime = Join-Path $stage 'Net10'
Copy-Item -LiteralPath $RuntimeDirectory -Destination $runtime -Recurse
$lib = Join-Path $runtime 'Lib'
$libSource = Join-Path $runtime 'LibSource'
if (@(Get-ChildItem -LiteralPath $lib -Recurse -File -Filter '*.pas').Count -ne 0) {
    throw 'Installed Lib must not contain Pascal sources.'
}
if (-not (Test-Path -LiteralPath (Join-Path $lib '__RedirectIOMode.pcu'))) {
    throw 'IDE I/O module PCU is missing.'
}
$originalAcls = @{}
$sid = [Security.Principal.WindowsIdentity]::GetCurrent().User
$rule = New-Object Security.AccessControl.FileSystemAccessRule(
    $sid, [Security.AccessControl.FileSystemRights]::Write,
    ([Security.AccessControl.InheritanceFlags]::ContainerInherit -bor [Security.AccessControl.InheritanceFlags]::ObjectInherit),
    [Security.AccessControl.PropagationFlags]::None, [Security.AccessControl.AccessControlType]::Deny)
$process = New-Object Diagnostics.Process
$processStarted = $false
try {
    foreach ($directory in @($lib, $libSource)) {
        $acl = Get-Acl -LiteralPath $directory
        $originalAcls[$directory] = $acl.Sddl
        $acl.AddAccessRule($rule)
        Set-Acl -LiteralPath $directory -AclObject $acl
    }
    # Verify that this really emulates a protected installation, even for the test author.
    $denied = $false
    try { [IO.File]::WriteAllText((Join-Path $lib 'write-probe.tmp'), 'must fail') }
    catch [UnauthorizedAccessException] { $denied = $true }
    if (-not $denied) { throw 'Read-only directory probe was not denied.' }
    $source = Join-Path $stage 'Program1.pas'
    $output = Join-Path $stage 'output'
    New-Item -ItemType Directory -Path $output -Force | Out-Null
    $process.StartInfo = New-Object Diagnostics.ProcessStartInfo
    $process.StartInfo.FileName = (Get-Command dotnet -CommandType Application).Source
    $process.StartInfo.Arguments = '"' + (Join-Path $runtime 'PABCCompilerController.dll') + '" "' + (Join-Path $runtime 'PABCCompilerWorker.dll') + '"'
    $process.StartInfo.WorkingDirectory = $runtime
    $process.StartInfo.UseShellExecute = $false
    $process.StartInfo.CreateNoWindow = $true
    $process.StartInfo.RedirectStandardInput = $true
    $process.StartInfo.RedirectStandardOutput = $true
    $process.StartInfo.RedirectStandardError = $true
    $process.StartInfo.StandardOutputEncoding = New-Object Text.UTF8Encoding($false)
    $processStarted = $process.Start()
    $stderr = $process.StandardError.ReadToEndAsync()
    $unitSource = Join-Path $stage 'Standalone.pas'
    $unitOutput = Join-Path $stage 'net10-output'
    $unitRequest = @{ id = 900; command = 'compile'; fileName = $unitSource; outputDirectory = $unitOutput;
        sourceFiles = @(@{ fileName = $unitSource;
            text = 'unit Standalone; interface procedure p1; implementation procedure p1; begin end; end.' }) }
    $process.StandardInput.WriteLine(($unitRequest | ConvertTo-Json -Depth 5 -Compress))
    $unitResponseTask = $process.StandardOutput.ReadLineAsync()
    if (-not $unitResponseTask.Wait(30000)) { throw 'Standalone unit compile timed out.' }
    $unitResponse = $unitResponseTask.Result | ConvertFrom-Json
    if (-not $unitResponse.success -or -not (Test-Path -LiteralPath (Join-Path $stage 'Standalone.pcu'))) {
        throw 'Standalone unit compilation did not create its PCU beside source.'
    }
    if (Test-Path -LiteralPath $unitOutput) { throw 'PCU-only compilation created net10-output.' }
    Write-Host 'PASS standalone PCU stays beside source without creating net10-output.'
    $request = @{ id = 1; command = 'compile'; fileName = $source; outputDirectory = $output;
        runtimeModule = '__RedirectIOMode'; sourceFiles = @(@{ fileName = $source;
            text = 'begin Print(1); var a := 1; var c: System.Span<integer>; end.' }) }
    $process.StandardInput.WriteLine(($request | ConvertTo-Json -Depth 5 -Compress))
    $responseTask = $process.StandardOutput.ReadLineAsync()
    if (-not $responseTask.Wait(30000)) { throw 'Controller compile timed out.' }
    $response = $responseTask.Result | ConvertFrom-Json
    if (-not $response.success) { throw "Protected-kit compilation failed: $($response | ConvertTo-Json -Depth 5 -Compress)" }
    if (-not (Test-Path -LiteralPath $response.outputFile)) { throw 'Program output is missing.' }
    & dotnet $response.outputFile
    if ($LASTEXITCODE -ne 0) { throw 'Compiled program failed to run.' }
    $process.StandardInput.WriteLine('{"id":2,"command":"shutdown"}')
    if (-not $process.WaitForExit(10000)) { throw 'Controller shutdown timed out.' }
    Write-Host "PASS unsaved IDE program compiles and runs with writes denied to Lib and LibSource. Fixtures: $stage"
}
finally {
    if ($processStarted -and -not $process.HasExited) { $process.Kill(); $process.WaitForExit() }
    $process.Dispose()
    foreach ($directory in $originalAcls.Keys) {
        $acl = Get-Acl -LiteralPath $directory
        $acl.SetSecurityDescriptorSddlForm($originalAcls[$directory])
        Set-Acl -LiteralPath $directory -AclObject $acl
    }
}
