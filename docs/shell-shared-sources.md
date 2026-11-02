# Shared shell sources: identical files

The Windows source tree is canonical for the 56 files linked by the Linux
project with Compile/Link metadata. Both EXEs and their assembly identities
remain separate. The Windows project and its source files were not changed.

The batch covers identical settings/forms, project helpers, navigation and
other utilities. DockContent, debugger, completion-popup files and files with
explicit focus/activation handlers were excluded, even where identical.

Linux .resx files remain local. Thirteen manifest resource names are explicit
so linking the related source does not change resource lookup.

Validation: both shells built; the Linux EXE had identical method IL and all
106 embedded resources matched the build before linking. Windows editor tests
passed for the .NET Framework and .NET 10 paths. Runtime output and shared
editor/contract distribution checks passed. The user also validated the updated
Linux build on ALT/Mono.

Retired Linux source copies are backed up outside the distribution in
.codex-build/shell-identical/retired-linux-source.
