param(
    [string]$FrameworkRuntime = (Join-Path $PSScriptRoot '../../bin'),
    [string]$ModernRuntime = (Join-Path $PSScriptRoot '../../bin-net10'),
    [switch]$CheckIntelliSense
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
        if ($calls.Count -ne 7 -or @($calls | Where-Object {
            $_.DeclaringType.Name -ne 'ReexportOrigin' -or $_.Name -eq 'FFF2'
        }).Count -ne 0) {
            throw "Expected seven calls directly to the original functions in $output"
        }
    }
    finally { $stream.Dispose() }
}

foreach ($target in @('net472', 'net10')) {
    $dir = Join-Path $root $target
    New-Item -ItemType Directory -Path $dir -Force | Out-Null
    foreach ($file in @('ReexportOrigin.pas', 'ReexportFacade.pas', 'ReexportBridge.pas', 'ReexportClient.pas')) {
        Copy-Item -LiteralPath (Join-Path $PSScriptRoot $file) -Destination $dir
    }

    function Invoke-Compile([string]$source, [string]$output, [string[]]$expectedErrors = @()) {
        $source = Join-Path $dir $source
        $output = Join-Path $dir $output
        if ($target -eq 'net10') {
            $log = & dotnet (Join-Path $ModernRuntime 'pabcnetcclear.dll') "/Output:$output" $source
        }
        else {
            $log = & (Join-Path $FrameworkRuntime 'pabcnetcclear.exe') "/Output:$output" $source
        }
        $rc = $LASTEXITCODE
        if ($expectedErrors.Count -ne 0) {
            $found = @($expectedErrors | Where-Object { ($log -join "`n").Contains($_) })
            if ($rc -ne 1 -or $found.Count -eq 0) {
                throw "Expected one of [$($expectedErrors -join ' | ')] in $source ($target): `n$log"
            }
        }
        elseif ($rc -ne 0) {
            throw "Compile failed for $source ($target): `n$log"
        }
    }

    function Invoke-TestProgram([string]$source, [string]$output) {
        Invoke-Compile $source $output
        if ($target -eq 'net10') { & dotnet (Join-Path $dir $output) }
        else { & (Join-Path $dir $output) }
        if ($LASTEXITCODE -ne 0) { throw "Runtime assertions failed: $output" }
    }

    function Invoke-Client([string]$output) {
        Invoke-TestProgram 'ReexportClient.pas' $output
        Assert-DirectCalls (Join-Path $dir $output)
    }

    Push-Location $dir
    try {
        $runtime = if ($target -eq 'net10') { $ModernRuntime } else { $FrameworkRuntime }
        function Get-DiagnosticTexts([string]$file, [string]$key) {
            foreach ($language in @('Eng', 'Rus')) {
                $resource = Join-Path $runtime "Lng/$language/$file"
                $entries = @([IO.File]::ReadAllLines($resource) | Where-Object { $_.StartsWith($key + '=') })
                if ($entries.Count -ne 1) { throw "Missing or duplicate $key in $resource" }
                $entries[0].Substring($key.Length + 1)
            }
        }
        Invoke-Client 'FromSource.exe'
        $sourceDir = $dir
        $pcuDir = Join-Path $dir 'pcu-only'
        New-Item -ItemType Directory -Path $pcuDir | Out-Null
        foreach ($unit in @('ReexportOrigin', 'ReexportFacade', 'ReexportBridge')) {
            if (!(Test-Path -LiteralPath "$unit.pcu")) { throw "Missing fresh $unit.pcu" }
            Copy-Item -LiteralPath "$unit.pcu" -Destination $pcuDir
        }
        Copy-Item -LiteralPath 'ReexportClient.pas' -Destination $pcuDir
        $dir = $pcuDir
        Set-Location -LiteralPath $dir
        Invoke-Client 'FromPcu.exe'
        if ($CheckIntelliSense) {
            if ($target -eq 'net10') {
                & dotnet (Join-Path $ModernRuntime 'UnitReexportIntelliSense.dll') $sourceDir $pcuDir
            }
            else {
                & (Join-Path $FrameworkRuntime 'UnitReexportIntelliSense.exe') $sourceDir $pcuDir
            }
            if ($LASTEXITCODE -ne 0) { throw "IntelliSense checks failed for $target" }
        }

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

        foreach ($name in 'TOriginPoint', 'OriginCount', 'OriginCounter') {
            $usage = switch ($name) {
                'TOriginPoint' { "var p: $name;" }
                'OriginCount' { "var c := $name;" }
                'OriginCounter' { "$name := 0;" }
            }
            [IO.File]::WriteAllText((Join-Path $dir 'OriginalSymbolClient.pas'),
                "uses ReexportFacade; begin $usage end.")
            Invoke-Compile 'OriginalSymbolClient.pas' 'OriginalSymbolClient.exe' $name
        }

        [IO.File]::WriteAllLines((Join-Path $dir 'PrivateExport.pas'), @(
            'unit PrivateExport;', 'interface', 'uses ReexportOrigin;',
            'export ReexportOrigin.PrivateCounter;', 'implementation', 'end.'
        ))
        [IO.File]::WriteAllText((Join-Path $dir 'PrivateExportClient.pas'), 'uses PrivateExport; begin end.')
        $unsupported = Get-DiagnosticTexts 'SemanticErrors_ssyy.dat' 'REEXPORT_REQUIRES_PUBLIC_SYMBOL{0}{1}'
        Invoke-Compile 'PrivateExportClient.pas' 'PrivateExportClient.exe' @($unsupported | ForEach-Object {
            $_ -f 'ReexportOrigin', 'PrivateCounter'
        })

        [IO.File]::WriteAllLines((Join-Path $dir 'TypeCollision.pas'), @(
            'unit TypeCollision;', 'interface', 'uses ReexportOrigin;',
            'export ReexportOrigin.TOriginBox as LocalBox;', 'type LocalBox<T> = class end;',
            'implementation', 'end.'
        ))
        [IO.File]::WriteAllText((Join-Path $dir 'TypeCollisionClient.pas'), 'uses TypeCollision; begin end.')
        $collision = Get-DiagnosticTexts 'SemanticErrors_ssyy.dat' 'REEXPORT_NAME_ALREADY_DECLARED{0}'
        Invoke-Compile 'TypeCollisionClient.pas' 'TypeCollisionClient.exe' @($collision | ForEach-Object { $_ -f 'LocalBox' })

        $genericSourceDir = Join-Path $sourceDir 'qualified-generics'
        New-Item -ItemType Directory -Path $genericSourceDir | Out-Null
        $dir = $genericSourceDir
        Set-Location -LiteralPath $dir
        foreach ($file in @('ReexportOrigin.pas', 'ReexportFacade.pas', 'QualifiedGenericCalls.pas')) {
            Copy-Item -LiteralPath (Join-Path $PSScriptRoot $file) -Destination $dir
        }
        Invoke-TestProgram 'QualifiedGenericCalls.pas' 'QualifiedGenericsFromSource.exe'

        $dir = Join-Path $genericSourceDir 'pcu-only'
        New-Item -ItemType Directory -Path $dir | Out-Null
        foreach ($file in @('ReexportOrigin.pcu', 'ReexportFacade.pcu', 'QualifiedGenericCalls.pas')) {
            Copy-Item -LiteralPath (Join-Path $genericSourceDir $file) -Destination $dir
        }
        Set-Location -LiteralPath $dir
        Invoke-TestProgram 'QualifiedGenericCalls.pas' 'QualifiedGenericsFromPcu.exe'
    }
    finally {
        Pop-Location
    }
    Write-Host "PASS $target - re-exports and qualified generic calls from source and PCU, visibility and diagnostics"
}
