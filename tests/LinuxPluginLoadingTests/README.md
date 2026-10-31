# Linux plugin loading

The Linux shell already queues plugin loading on its UI thread after compiler
startup. It loads plugins implementing the contract from PluginsSupportLinux.dll;
plugins built against PluginsSupport.dll are not compatible yet.

LinuxPluginLoadingTests verifies loading, error isolation, UI registration and
menu/toolbar actions with fixture plugins. These tests run on Windows and do not
replace a Mono/X11 check.

For a manual Mono check, the existing VisualPlugins/TestPlugin project supports
`/p:PluginPlatform=Linux`. Build into a separate output directory against the Linux
dependencies, then copy only TestPlugin.dll beside PascalABCNETLinux.exe. Do not
overwrite bin/TestPlugin.dll with this variant. The default project configuration
still references the Windows contract.

After startup, check the Modules menu and compiler messages for Test Plugin v1.0.
Open the plugin form from the menu, close it, reopen from the toolbar, then return
to typing and compile/run a small program. The sample form is intentionally empty.
TestPlugin.dll is for this manual check and is not included by the normal Linux
release packaging script.

TeacherControlPlugin is built separately by scripts/build-teacher-control-linux.ps1
into bin/PluginsLinux. The Linux packaging script invokes this build and includes
that variant, LightPT.pcu and Tasks.pcu. The Linux contract now also supports
AfterAddInGUI, ChangeArgsBeforeRun and ConvertUnitTextProperty, matching the
corresponding Windows hooks. Fixture tests verify that AfterAddInGUI runs only
after menu and toolbar controls have been assigned.

For the TeacherControl manual check, start the IDE with a program in a working
directory containing lightpt.dat. The plugin intentionally hides its GUI without
that marker, just as on Windows. Test sign-in and a real LightPT task in ALT;
network authentication is not covered by the loader tests.
