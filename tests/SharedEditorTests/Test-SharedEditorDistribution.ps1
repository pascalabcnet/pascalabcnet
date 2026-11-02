[CmdletBinding()]
param([string]$RepositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../..')))

$ErrorActionPreference = 'Stop'
Add-Type -Path (Join-Path $RepositoryRoot 'bin/Mono.Cecil.dll')
$canonical = Join-Path $RepositoryRoot 'Libraries/ICSharpCode.TextEditor.dll'
$expectedHash = (Get-FileHash -LiteralPath $canonical).Hash
$expectedIdentity = 'ICSharpCode.TextEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=4d61825e8dd49f1a'
foreach ($relative in @('Libraries/ICSharpCode.TextEditor.dll', 'bin/ICSharpCode.TextEditor.dll', 'Release/PascalABCNETLinux/ICSharpCode.TextEditor.dll')) {
    $path = Join-Path $RepositoryRoot $relative
    if ((Get-FileHash -LiteralPath $path).Hash -ne $expectedHash) { throw "Different editor payload: $relative" }
    $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($path)
    if ($assembly.Name.FullName -cne $expectedIdentity) { throw "Editor identity changed: $relative" }
    foreach ($reference in $assembly.MainModule.AssemblyReferences) {
        if ($reference.Name -in @('PresentationFramework', 'PresentationCore', 'WindowsBase', 'System.Xaml')) {
            throw "Mandatory WPF dependency: $($reference.Name)"
        }
    }
}
foreach ($directory in @('bin', 'Release/PascalABCNETLinux')) {
    $root = Join-Path $RepositoryRoot $directory
    if (Test-Path -LiteralPath (Join-Path $root 'ICSharpCode.TextEditorLinux.dll')) { throw "Obsolete editor remains: $directory" }
    foreach ($name in @('PascalABCNETLinux.exe', 'PluginsSupport.dll')) {
        $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly((Join-Path $root $name))
        $references = @($assembly.MainModule.AssemblyReferences | Where-Object Name -Like 'ICSharpCode.TextEditor*')
        if ($references.Count -ne 1 -or $references[0].FullName -cne $expectedIdentity) { throw "Wrong editor reference: $directory/$name" }
    }
}
Write-Output 'PASS identical signed editor payload, no mandatory WPF, both Linux consumers use the common identity, obsolete DLL absent'
