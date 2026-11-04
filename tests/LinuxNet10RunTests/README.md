# Linux .NET 10 run bridge tests

Build the Linux IDE and CompileNet10 plugin (`PluginPlatform=Linux`) into an isolated directory with the common contract and editor dependencies. Build this project with `IdeReferenceDirectory` and `OutDir` pointing to that directory. Use the Linux IDE's exe.config as `LinuxNet10RunTests.exe.config` on .NET Framework.

Run `LinuxNet10RunTests.exe <path-to-unified-net10-kit>` on Windows, or `mono LinuxNet10RunTests.exe <path-to-kit>` on Linux with .NET 10 on PATH. Without the kit argument only host routing/lifecycle/probe checks run. Tests use temporary program files and never change IDE settings. Host command tests use uninitialized service fixtures and fake document/settings properties; they do not drive native menus or test GTK/X11 focus. The compiler and application pipeline is real.
