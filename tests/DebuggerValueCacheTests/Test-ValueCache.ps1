[CmdletBinding()]
param([string]$DebuggerAssembly = '')
$ErrorActionPreference = 'Stop'
$root = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
if (!$DebuggerAssembly) { $DebuggerAssembly = Join-Path $root 'Libraries\Debugger.Core.dll' }
$output = Join-Path $root ('.codex-build\debugger-cache-tests\' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $output -Force | Out-Null
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe'
& $csc /nologo /debug /optimize- /platform:x86 "/out:$output\Fixture.exe" "$PSScriptRoot\Fixture.cs"
if ($LASTEXITCODE) { throw 'Fixture build failed.' }
& $csc /nologo /platform:x86 "/r:$DebuggerAssembly" /r:System.Windows.Forms.dll "/out:$output\Probe.exe" "$PSScriptRoot\Probe.cs"
if ($LASTEXITCODE) { throw 'Probe build failed.' }
Copy-Item -LiteralPath $DebuggerAssembly -Destination "$output\Debugger.Core.dll"
& "$output\Probe.exe" "$output\Fixture.exe"
if ($LASTEXITCODE) { throw 'Debugger value cache regression test failed.' }
