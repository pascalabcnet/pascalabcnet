# Linux completion focus regression checks

This executable checks documentation cancellation during a deliberately blocked read,
replacement by the latest request, UI publication, disposal and read failures.
It does not install software or touch user settings or source files.

```powershell
dotnet build tests/LinuxCompletionFocusTests -c Release
tests/LinuxCompletionFocusTests/bin/Release/net472/LinuxCompletionFocusTests.exe
```

On Linux with Mono and an active graphical desktop (no .NET 10 required):

```sh
mono LinuxCompletionFocusTests.exe > focus-test.log 2>&1
```

A Windows pass checks controller logic, **not** the ALT/Mono/X11 implementation.
Run this harness and the modified Linux IDE in the affected graphical environment.
The standalone harness needs only its EXE and the installed Mono/WinForms runtime.

To additionally exercise a built Linux IDE's actual completion window, provider,
editor and `Closed` cleanup, pass the path to its EXE:

```sh
mono LinuxCompletionFocusTests.exe ./PascalABCNETLinux.exe > focus-test.log 2>&1
```

The optional checks cover `uses School;` without a popup, semicolon accepting
`School` and `Sch`, and Enter/Tab through the real editor. Each
case checks disposal of both popup windows, editor focus and the next character.
The same acceptance paths are tested with the real IDE's documentation reader
deliberately held at a gate: closing completion must cancel it immediately while
input continues. Additional checks cover queue snapshots, the legacy API and
publication of actual XML text using an in-memory XML fixture.
They use controlled completion data, not a real parse of School.
They exercise the editor's key handlers directly; physical
keyboard routing and the native window manager still need a Linux desktop check.
The EXE must have its matching distribution DLLs alongside it. The harness reads
the popup's options through an isolated host without invoking the IDE constructor.

The description window retains its original implementation, including enabled
state and native styles. Tests check showing,
hiding and reusing the original popup; they make no claim that it never takes
focus under Mono. Both the ActiveForm/Activate controller and the subsequent
WS_DISABLED/WS_POPUP/WS_EX_NOACTIVATE experiment have been withdrawn.

The Linux hover service now owns each displayed hint by its source editor.
Requests from hidden tabs/outside the document cannot dismiss another editor's
hint, and repeated requests keep the current hint instead of toggling it off.
The regression test failed before this change and now passes. The tests also
keep the normal dismissal behavior when leaving the owning document/opening
a menu. Timer requests are injected explicitly, with the physical desktop
pointer excluded from the assertions. The suite also compiles
a real Pascal sample and checks name resolution and the editor-to-window event
path. The extended suite needs the matching PascalABCLanguageInfo.dll/parser
assemblies alongside the IDE (normally included in a Linux distribution).

Optional local diagnostics, disabled by default:

```sh
PABCNET_HOVER_TRACE=1 mono PascalABCNETLinux.exe
```

The terminal prints the path to a PID-specific log in the temporary directory.
It records loaded assembly paths, hover events, early returns, resolved text
length, window state and exceptions. It also records handle creation/destruction,
focus, mouse-capture notifications, and navigation/deletion keys for each popup
and editor, so X11 BadWindow resource IDs can be correlated with their owners.
The logger never reads Handle during destruction; a test checks that tracing
does not create/recreate native handles. It never changes focus or user settings.

Manual IDE checks: `uses School;`; completion after a dot accepted with Enter,
Tab, punctuation and double-click; Esc; scrolling a long list; showing/changing
the yellow description; menus/context menus; changing editor tabs; switching to
another application and back; hover hints; closing the IDE with a popup open.
Check that input continues and no hidden popup retains focus. The background
parser, compiler and Windows IDE are deliberately unchanged.

The shared `CodeCompletion.dll` has an additive snapshot API used by the Linux
shell. Queue synchronization is short and never covers XML reads. Windows keeps
using its existing `CompleteDocumentation`/`Reset` entry points. Deploy the new
Linux EXE and its matching `CodeCompletion.dll` together in a separate test copy.
