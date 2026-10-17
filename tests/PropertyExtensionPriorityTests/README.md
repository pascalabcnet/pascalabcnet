# Property / extension-method name collision regression tests

Run from the repository root (Windows, .NET SDK and .NET Framework 4.7.2):

```powershell
powershell -NoProfile -File tests/PropertyExtensionPriorityTests/run.ps1
```

Build the updated compiler for each target first. The script builds its small C#
fixture for each target and compiles/runs the Pascal tests with the corresponding
console compiler. It needs no third-party packages. Outputs stay under
`.codex-build/property-extension-tests/`.

Use `-Target net472` or `-Target net10.0` to select one target. Optional
`-LegacyCompilerDirectory` and `-ModernCompilerDirectory` allow isolated compiler
builds instead of the default `bin` and `bin-net10`.

Checks assignment and reading of string and nullable integer properties,
inherited properties, explicit extension calls with one and two arguments,
readonly-property diagnostics, and rejection of assignment to a method when
there is no property. No NuGet or Spectre.Console installation is required.
