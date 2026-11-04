# Linux IntelliSense UI adapter checks

This fixture exercises `IExternalIntelliSenseSession` against
`PascalABCNETLinux.exe`, without starting the IDE services, LSP or a compiler.
It does not read or change user settings. It checks external completion insertion,
prefixes, ordering, parameter stacks, overload selection, fonts, hover ownership,
late responses and editor focus.

Build for .NET Framework 4.7.2 with `IdeReferenceDirectory` pointing to an isolated
Linux distribution. Set `OutDir` to that same directory so the test executable can
resolve the shell, common editor and plugin contract. Run
`LinuxIntelliSenseUITests.exe` on Windows or `mono LinuxIntelliSenseUITests.exe`
inside the Linux desktop session. A graphical session is required.

The production Linux shell exposes the optional UI service only. This step does
not ship/enable `CompileNet10Plugin`, start LanguageServer or switch platforms.
The existing `LinuxCompletionFocusTests` separately checks the legacy providers.
