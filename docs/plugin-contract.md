# Common plugin contract

Windows and Linux shells both reference PluginsSupport/PluginsSupport.csproj and
ship PluginsSupport.dll (version 1.0.0.0). Interfaces.cs and Classes.cs from the
Windows project are the only contract sources. Their public API and behavior
were not changed during the migration; existing Windows plugins keep working.

The Linux shell implements BeforeCompile in addition to its existing hooks.
It invokes the callback before building a program for execution, matching the
Windows Run service. The shells and their plugin loaders remain separate.

TestPlugin now uses the same DLL on both platforms. TeacherControlPlugin still
has a Linux build option for its Mono-specific form rendering and UI dispatch;
this option no longer selects a different plugin contract. Linux packaging copies
PluginsSupport.dll and the Linux TeacherControl variant from bin/PluginsLinux.

Migration checks on 2026-10-31:
- Both shells built against the common contract.
- Rebuilt contract types, signatures, method IL and assembly references matched
  the previous Windows contract. The deployed common DLL is the existing Windows
  binary, unchanged (SHA256 338BB4546EB5A8E0438323DBE10F4945BC369DD22EABF733BC086B6DB15B4E4E).
- Windows editor tests passed with the rebuilt Windows shell, including legacy
  and .NET 10 paths. Installed Windows EXE and Windows plugins were not replaced.
- Loader fixtures, TeacherControl initialization/decryption/argument handling,
  rating layout and Linux output color tests passed in the Windows test host.
- Shared editor and plugin distribution checks passed for installed files.
- User validated the Linux build on ALT/Mono and both .NET Framework and .NET 10
  modes in the Windows IDE.
