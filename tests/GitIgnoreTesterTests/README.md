# Repository-content checker regression tests

```powershell
powershell.exe -NoProfile -File tests/GitIgnoreTesterTests/Test-GitIgnoreTester.ps1
```

Compiles the production Pascal checker with the legacy compiler into an isolated
Git fixture under `.codex-build`. Does not alter the real repository index.

Checks clean runs, strict exit codes, warning annotations, escaping of annotation
data, ignored tracked files, and actual Git execution failures.

`GitIgnoreTester.exe NoWait` retains strict behavior. `WarningsOnly` makes findings
nonfatal; `GitHubActions` emits annotations. `Strict` overrides `WarningsOnly`.
Execution failures always return 1. Windows CI runs the checker as a separate
step after compilation and language tests, using `NoWait WarningsOnly GitHubActions`.
