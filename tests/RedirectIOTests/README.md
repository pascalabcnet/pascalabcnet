# Redirected I/O without an implicit WinForms dependency

```powershell
powershell.exe -NoProfile -File tests/RedirectIOTests/Test-RedirectIO.ps1
powershell.exe -NoProfile -File tests/RedirectIOTests/Test-RedirectIO.ps1 -Target net10.0 -IncludeClosedXml
```

Uses the existing legacy and modern compilers. Copies the production
`bin/Lib/__RedirectIOMode.pas` into isolated work directories to avoid stale PCUs.
Checks that console programs do not load WinForms or require WindowsDesktop,
ordinary WinForms programs still forward `Application.ThreadException`, and
common unhandled exceptions still reach the IDE protocol. The WinForms event is
raised through `Application.OnThreadException`; no interactive windows are opened.
The modern WinForms fixture explicitly adds Microsoft.WindowsDesktop.App to its
runtimeconfig: desktop targeting is not yet emitted by the console compiler.

The optional ClosedXML 0.105.1 integration case restores packages using normal
NuGet configuration, writes/reopens an Excel workbook and verifies the absence
of WinForms. It requires cached packages or access to configured feeds.

The runtime module subscribes on its initialization thread to already loaded
WinForms, or loads it only if the generated program really references it.
Dynamically loaded GUI plugins and WPF exception handling are separate scenarios;
this change does not introduce a WPF handler or change exception termination policy.
