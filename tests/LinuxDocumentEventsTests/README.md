# Linux document lifecycle checks

Build this net472 fixture with `IdeReferenceDirectory` and `OutDir` pointing to
an isolated Linux distribution. Run `LinuxDocumentEventsTests.exe`, or under
Mono run `mono LinuxDocumentEventsTests.exe` in a graphical desktop session.
An optional first argument selects the Linux shell EXE.

The fixture exercises the real shell's event source and open-document registry
without starting IDE services or changing user settings. It loads/saves temporary
editor text, checks captured old/new names, ordinary Save, snapshot isolation,
subscriber exceptions and notification before editor disposal. Cecil checks the
actual compiled file-operation paths: notification after load/save, early return
on cancelled close, and removal before close notification. It does not automate
the modal file/save confirmation dialogs.

Manual ALT check: open two files and reopen an existing tab; Save As to a new
name and continue editing; cancel closing a modified tab, then close it using
Discard. The tab/text must survive cancellation and normal editing must continue.
The .NET 10 plugin and processes remain disabled throughout this step.
