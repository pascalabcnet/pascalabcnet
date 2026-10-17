param(
    [ValidateSet('net472', 'net10.0', 'all')]
    [string]$Target = 'all',
    [string]$LegacyCompilerDirectory,
    [string]$ModernCompilerDirectory
)
$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
if (!$LegacyCompilerDirectory) { $LegacyCompilerDirectory = Join-Path $repositoryRoot 'bin' }
if (!$ModernCompilerDirectory) { $ModernCompilerDirectory = Join-Path $repositoryRoot 'bin-net10' }
$targets = if ($Target -eq 'all') { @('net472', 'net10.0') } else { @($Target) }
foreach ($framework in $targets) {
    $work = Join-Path $repositoryRoot ".codex-build/property-extension-tests/$framework"
    New-Item -ItemType Directory -Path $work -Force | Out-Null
    $buildOutput = & dotnet build (Join-Path $PSScriptRoot 'Fixture/PropertyExtensionFixture.csproj') -f $framework -o $work --nologo -v:q 2>&1
    if ($LASTEXITCODE -ne 0) { throw "Fixture build failed: $buildOutput" }
    Copy-Item (Join-Path $PSScriptRoot '*.pas') -Destination $work
    $modern = $framework -eq 'net10.0'
    $compiler = if ($modern) { Join-Path $ModernCompilerDirectory 'pabcnetcclear.dll' } else { Join-Path $LegacyCompilerDirectory 'pabcnetcclear.exe' }
    if (!(Test-Path -LiteralPath $compiler)) { throw "Compiler not found: $compiler" }
    Push-Location $work
    try {
        foreach ($name in @('property_extension_priority', 'property_extension_readonly', 'property_extension_method_only')) {
            $source = Join-Path $work "$name.pas"
            $ErrorActionPreference = 'Continue'
            $output = if ($modern) { & dotnet $compiler $source 2>&1 } else { & $compiler $source 2>&1 }
            $compileExit = $LASTEXITCODE
            $ErrorActionPreference = 'Stop'
            $text = $output -join "`n"
            if ($name -eq 'property_extension_priority') {
                if ($compileExit -ne 0) { throw "$framework compilation failed: $text" }
                $program = Join-Path $work "$name.exe"
                $ErrorActionPreference = 'Continue'
                $runOutput = if ($modern) { & dotnet $program 2>&1 } else { & $program 2>&1 }
                $runExit = $LASTEXITCODE
                $ErrorActionPreference = 'Stop'
                if ($runExit -ne 0) { throw "$framework assertions failed: $runOutput" }
            } else {
                if ($compileExit -eq 0) { throw "$framework incorrectly accepted $name" }
                if ($name -eq 'property_extension_readonly' -and $text -notmatch 'ReadOnly') {
                    throw "$framework did not select the readonly property: $text"
                }
            }
            Write-Host "PASS $framework $name"
        }
    } finally {
        Pop-Location
    }
}
