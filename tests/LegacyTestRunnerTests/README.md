# Legacy TestRunner diagnostic headers

The full legacy runner and the focused unit test include the same
`ExpectedDiagnosticHeader.inc`. The helper reads only the first physical line
after the language's `!` diagnostic marker, independent of OS line endings.

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File tests/LegacyTestRunnerTests/Test-LegacyDiagnosticHeaders.ps1
```

This rebuilds `bin/TestRunner.exe` (backing up its previous executable), without
rebuilding IDE/compiler DLLs, standard modules or changing GAC. Then it compiles
and runs unit tests for CRLF/LF/CR/EOF, Pascal and SPython markers, and uses the
real legacy runner on isolated warning/error fixtures in `.codex-build`.
The fixture suite also checks a package-version conflict with a module in the
sibling `units` directory, using the production test's explicit relative path.
The production `TestSuite` source files are read only.

Use `-SkipRunnerBuild` only when the runner has already been rebuilt from the
current source. The fixture passes are not a replacement for all six release
passes or the cross-target suite.

To check all six legacy SPython passes on an isolated copy of the production
suite, without creating Pascal-only reference fixtures there:

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File tests/LegacyTestRunnerTests/Test-LegacySPython.ps1
```

This also backs up and rebuilds the runner; use `-SkipRunnerBuild` when it is
already current. It does not rebuild standard modules or change GAC.
