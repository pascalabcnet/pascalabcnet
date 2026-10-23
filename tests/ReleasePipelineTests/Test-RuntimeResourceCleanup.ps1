[CmdletBinding()]
param()
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$clean = Join-Path $repositoryRoot 'scripts\clean-net10-runtime-resources.ps1'
$root = Join-Path $repositoryRoot ('.codex-build\resource-cleanup-tests\' + [Guid]::NewGuid().ToString('N'))
function Put([string]$Path) {
    $full = Join-Path $root $Path
    New-Item -ItemType Directory -Path (Split-Path -Parent $full) -Force | Out-Null
    [IO.File]::WriteAllText($full, "sentinel: $Path")
}
function Check([bool]$Condition, [string]$Message) {
    if (-not $Condition) { throw $Message }
    Write-Host "PASS $Message"
}
$languages = @('cs', 'de', 'es', 'fr', 'it', 'ja', 'ko', 'pl', 'pt-BR', 'tr', 'zh-Hans', 'zh-Hant')
$resources = @('Microsoft.VisualStudio.LanguageServer.Protocol.resources.dll',
    'Microsoft.VisualStudio.Threading.resources.dll', 'Microsoft.VisualStudio.Validation.resources.dll',
    'StreamJsonRpc.resources.dll')
foreach ($language in $languages) {
    foreach ($resource in $resources) { Put "$language\$resource" }
}
$preserved = @('ru\StreamJsonRpc.resources.dll', 'ru\custom.txt',
    'runtimes\win\lib\net8.0\System.Security.Cryptography.Pkcs.dll',
    'Lng\Eng\PABCNETC.dat', 'Lng\Rus\PABCNETC.dat', 'Lib\PABCSystem.pcu',
    'fr\custom.txt', 'custom.config', 'Compiler.dll', 'uk\StreamJsonRpc.resources.dll')
foreach ($path in $preserved) { Put $path }
Put 'pabcnetc.dll.config'
Put 'pabcnetcclear.dll.config'
& $clean -RuntimeDirectory $root
foreach ($language in $languages) {
    foreach ($resource in $resources) {
        Check (-not (Test-Path -LiteralPath (Join-Path $root "$language\$resource"))) "removed $language/$resource"
    }
    if ($language -ne 'fr') {
        Check (-not (Test-Path -LiteralPath (Join-Path $root $language))) "removed empty $language directory"
    }
}
foreach ($path in $preserved) {
    Check ([IO.File]::ReadAllText((Join-Path $root $path)) -eq "sentinel: $path") "preserved $path"
}
foreach ($name in @('pabcnetc.dll.config', 'pabcnetcclear.dll.config')) {
    Check (-not (Test-Path -LiteralPath (Join-Path $root $name))) "removed Framework config $name"
}
& $clean -RuntimeDirectory $root
Check ($true) 'cleanup is repeatable'

# Exercise MSBuild's real app.config selection for both console frontends.
foreach ($project in @('pabcnetc\pabcnetc.csproj', 'pabcnetc_clear\PABCNETCclear.csproj')) {
    foreach ($framework in @('net10.0', 'net472')) {
        $legacy = if ($framework -eq 'net472') { 'true' } else { 'false' }
        $output = & dotnet msbuild (Join-Path $repositoryRoot $project) `
            "-p:TargetFramework=$framework" "-p:PABCNET_LEGACY_ONLY=$legacy" `
            "-p:OutputPath=$root\msbuild\" -target:PrepareForBuild -getItem:AppConfigWithTargetPath -nologo
        if ($LASTEXITCODE -ne 0) { throw "MSBuild config selection failed: $project $framework" }
        $items = ($output -join [Environment]::NewLine | ConvertFrom-Json).Items.AppConfigWithTargetPath
        Check ((@($items).Count -gt 0) -eq ($framework -eq 'net472')) "$project config selected only for Framework ($framework)"
    }
}
