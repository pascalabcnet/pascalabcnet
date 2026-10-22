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

To check interface extension operators through `PABCRtl.dll` without a prior
PCU/x86 compilation in the same process:

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File tests/LegacyTestRunnerTests/Test-PABCRtlOperators.ps1
```

This copies the runtime into `.codex-build`, rebuilds only the isolated test
runner, and starts a fresh process for every compile/run pass. It tests the
unchanged `extensionmethods26.pas`, `overloads7.pas`, `SliceArrGeneric.pas`
and additional string/array/list cases,
including ordinary extension methods. The DLL-only checks temporarily hide the
copied system PCUs and verify that the generated EXE really references
`PABCRtl.dll`. The same sources are then compiled and run normally through PCUs.
No production PCUs or GAC entries change. `-LegacyRuntimeDirectory` can select
another prepared runtime for validation.
It also checks `RTL -> PCU -> RTL` four times in one compiler process using the
production `beginend`, `arrays4` (with its module) and `foreach9` tests, then runs
the generated executables. It asserts that the same metadata table survives
each compilation and that inactive RTL operators are hidden, not deleted.
No previous x86 pass is involved.

`ReflectionCacheUnloadTest` additionally checks late DLL discovery, cached
interface/array extensions, inactive DLL filtering in compiler and IntelliSense
lookups, stable table/list identity on repeated references, and real collectible
context unloading without losing another DLL's methods in shared indexes.
