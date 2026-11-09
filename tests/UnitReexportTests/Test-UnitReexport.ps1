param(
    [string]$FrameworkRuntime = (Join-Path $PSScriptRoot '../../bin'),
    [string]$ModernRuntime = (Join-Path $PSScriptRoot '../../bin-net10')
)

$ErrorActionPreference = 'Stop'
$root = Join-Path ([IO.Path]::GetTempPath()) ("pabc-reexport-" + [guid]::NewGuid().ToString('N'))
$FrameworkRuntime = (Resolve-Path -LiteralPath $FrameworkRuntime).Path
$ModernRuntime = (Resolve-Path -LiteralPath $ModernRuntime).Path
Add-Type -Path (Join-Path $FrameworkRuntime 'Mono.Cecil.dll')

function Assert-DirectCalls([string]$output) {
    $stream = [IO.MemoryStream]::new([IO.File]::ReadAllBytes($output))
    try {
        $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($stream)
        $calls = @($assembly.MainModule.Types | ForEach-Object { $_.Methods } | Where-Object HasBody |
            ForEach-Object { $_.Body.Instructions } | Where-Object {
                $_.Operand -is [Mono.Cecil.MethodReference] -and
                $_.Operand.DeclaringType.Name -in @('ReexportOrigin', 'ReexportFacade') -and
                $_.Operand.Name -in @('F1', 'F2', 'FFF2')
            } | ForEach-Object { $_.Operand })
        if ($calls.Count -ne 6 -or @($calls | Where-Object {
            $_.DeclaringType.Name -ne 'ReexportOrigin' -or $_.Name -eq 'FFF2'
        }).Count -ne 0) {
            throw "Expected six calls directly to the original functions in $output"
        }
    }
    finally { $stream.Dispose() }
}

foreach ($target in @('net472', 'net10')) {
    $dir = Join-Path $root $target
    New-Item -ItemType Directory -Path $dir -Force | Out-Null
    foreach ($file in @('ReexportOrigin.pas', 'ReexportFacade.pas', 'ReexportClient.pas')) {
        Copy-Item -LiteralPath (Join-Path $PSScriptRoot $file) -Destination $dir
    }

    function Invoke-Compile([string]$source, [string]$output, [string]$expectedError = '') {
        $source = Join-Path $dir $source
        $output = Join-Path $dir $output
        if ($target -eq 'net10') {
            $log = & dotnet (Join-Path $ModernRuntime 'pabcnetcclear.dll') "/Output:$output" $source
        }
        else {
            $log = & (Join-Path $FrameworkRuntime 'pabcnetcclear.exe') "/Output:$output" $source
        }
        $rc = $LASTEXITCODE
        if ($expectedError -ne '') {
            if ($rc -eq 0 -or ($log -join "`n") -notmatch [regex]::Escape($expectedError)) {
                throw "Expected error for $expectedError in $source ($target): `n$log"
            }
        }
        elseif ($rc -ne 0) {
            throw "Compile failed for $source ($target): `n$log"
        }
    }

    function Invoke-Client([string]$output) {
        Invoke-Compile 'ReexportClient.pas' $output
        if ($target -eq 'net10') { & dotnet (Join-Path $dir $output) }
        else { & (Join-Path $dir $output) }
        if ($LASTEXITCODE -ne 0) { throw "Runtime assertions failed: $output" }
        Assert-DirectCalls (Join-Path $dir $output)
    }

    Push-Location $dir
    try {
        Invoke-Client 'FromSource.exe'
        $pcuDir = Join-Path $dir 'pcu-only'
        New-Item -ItemType Directory -Path $pcuDir | Out-Null
        foreach ($unit in @('ReexportOrigin', 'ReexportFacade')) {
            if (!(Test-Path -LiteralPath "$unit.pcu")) { throw "Missing fresh $unit.pcu" }
            Copy-Item -LiteralPath "$unit.pcu" -Destination $pcuDir
        }
        Copy-Item -LiteralPath 'ReexportClient.pas' -Destination $pcuDir
        $dir = $pcuDir
        Set-Location -LiteralPath $dir
        Invoke-Client 'FromPcu.exe'

        [IO.File]::WriteAllText((Join-Path $dir 'HiddenClient.pas'),
            'uses ReexportFacade; begin assert(Hidden = 42); end.')
        Invoke-Compile 'HiddenClient.pas' 'HiddenClient.exe' 'Hidden'
        [IO.File]::WriteAllText((Join-Path $dir 'OriginalNameClient.pas'),
            'uses ReexportFacade; begin var n := 0; ReexportFacade.F2(n); end.')
        Invoke-Compile 'OriginalNameClient.pas' 'OriginalNameClient.exe' 'F2'

        [IO.File]::WriteAllText((Join-Path $dir 'Collision.pas'), @'
unit Collision;
interface
uses ReexportOrigin;
export ReexportOrigin.F1 as Taken;
function Taken: integer;
implementation
function Taken: integer := 0;
end.
'@)
        [IO.File]::WriteAllText((Join-Path $dir 'CollisionClient.pas'), 'uses Collision; begin end.')
        Invoke-Compile 'CollisionClient.pas' 'CollisionClient.exe' 'Taken'

        [IO.File]::WriteAllText((Join-Path $dir 'NotInInterface.pas'), @'
unit NotInInterface;
interface
export ReexportOrigin.F1;
implementation
uses ReexportOrigin;
end.
'@)
        [IO.File]::WriteAllText((Join-Path $dir 'NotInInterfaceClient.pas'),
            'uses NotInInterface; begin end.')
        Invoke-Compile 'NotInInterfaceClient.pas' 'NotInInterfaceClient.exe' 'ReexportOrigin'
    }
    finally {
        Pop-Location
    }
    Write-Host "PASS $target - re-exports from source and PCU, aliases, overloads, visibility, collision"
}
