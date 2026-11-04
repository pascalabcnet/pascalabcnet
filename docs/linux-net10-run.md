# .NET 10 compilation and execution in the Mono Linux IDE

The IDE still runs under Mono. Only the compiler controller, worker and compiled applications run under .NET 10. The Linux plugin synchronizes open documents with LanguageServer from the same net10 kit. Dot completion uses the shared asynchronous provider and the Linux host's passive popup. Legacy analysis and its completion/hover/signature/navigation UI are suspended while .NET 10 is selected; switching back to Mono resumes them without changing saved preferences. Modern parameter help uses the same passive Linux window and supports opening brackets, commas, overloads and nested calls. Modern hover uses the existing passive Linux tooltip and the CodeCompletionHint preference. Ctrl-click uses shared LSP navigation and the existing host file/location services: Pascal source opens at its definition, .NET members open read-only metadata. Multiple targets use a Linux context-menu chooser. A separate LSP status counter confirms synchronization.

## Build and package on Windows

Build `VisualPascalABCNETLinux`, the common `PluginsSupport.dll`, and the unified `bin-net10` runtime kit first. Then run:

```
_GenerateLinuxNet10Version.bat
```

This calls `_GenerateLinuxVersion.bat --net10`. Calling `_GenerateLinuxVersion.bat` without an argument still creates the ordinary Mono distribution.

The .NET 10 variant adds `CompileNet10Plugin.dll`, `CompileNet10Plugin.ini` and a single `net10/` kit inside `Release/PascalABCNETLinux`. The kit is exported from the existing `bin-net10` manifest; no second compiler build or bundled Microsoft runtime is created. The Windows plugin in `bin` is preserved. A package-only update can use `scripts/prepare-linux-net10.ps1` after preparing the Linux IDE folder.

## Run on Linux

Install a .NET 10 runtime matching the guest architecture. `dotnet --list-runtimes` must contain `Microsoft.NETCore.App 10.0.x`; WindowsDesktop is not required. Start the IDE with `mono PascalABCNETLinux.exe`. The Program menu's `.NET 10` switch is initially enabled when the kit and runtime are available, otherwise disabled and legacy compilation remains usable. The choice is session-only.

`CompileNet10Plugin.ini` defaults to:

```
RuntimeDirectory=net10
DotnetPath=dotnet
```

An absolute Linux path to the dotnet executable may be supplied if a desktop launcher has a different PATH. RuntimeDirectory is relative to the IDE executable, not the shell's working directory.

This first integration supports standard Compile/Recompile/Run/Stop, redirected console input/output, open unsaved Pascal sources, compiler diagnostics and runtime source locations. External-terminal execution and the .NET debugger are not connected: use the IDE output-window run mode. Existing Mono programs and their debugger retain their old paths when `.NET 10` is unchecked. Modern dot completion describes the .NET 10 platform.

## Checks

`tests/LinuxNet10RunTests` validates the real Linux host's command entry points and RunManager lifecycle, the absence of Windows shell references and navigation implementation in the Linux plugin, and the Core-only runtime probe. With a kit path argument it uses the actual controller/worker to test unsaved units, Unicode and input, Stop, compilation diagnostics and runtime stack locations. Run it on Windows against the isolated Linux build or with Mono on Linux. Automated checks on Windows do not replace guest UI validation.

Guest checks: normal Mono Run; modern `Println`; `ReadInteger`; Stop on a loop; an unsaved imported unit; compiler and runtime errors; switching back to Mono. In modern mode type `s.` after declaring a string: check typing a prefix, Enter/Tab acceptance and Escape cancellation without losing editor focus. Switching to Mono must restore legacy completion/hover/signatures. In modern mode also check `s.Substring(`, comma, nested calls and Escape; parameter hints must keep the editor focused. The LSP counter should match the number of open Pascal source tabs; switching to Mono hides it. LanguageServer is reused across mode switches and stops when the IDE closes.

Hover guest check: hover a local variable or method, move off the name, edit text, switch tabs and switch to Mono. No stale hint should reappear and typing must retain editor focus.

Navigation guest check: Ctrl-click a local symbol, an unsaved imported module member and a .NET member. Check existing tabs are reused, metadata opens read-only, plain clicks do not navigate and switching to Mono restores legacy navigation.

The navigation bar now receives detached document symbols from LSP through the optional host capability. It updates unsaved source, follows the caret, and returns to legacy analysis in Mono mode. Check the class/method dropdowns and source navigation in ALT.
