# Linux plugin loading

The Linux shell already queues plugin loading on its UI thread after compiler
startup. Windows and Linux use the same PluginsSupport.dll contract. Its name,
version and API are preserved from the Windows assembly.

Run `Test-SharedPluginDistribution.ps1` to check installed Windows/Linux payloads:
identical contract bytes, the original assembly identity, and no references to the
retired PluginsSupportLinux assembly. Both solutions now build PluginsSupport;
the duplicate Linux project has been removed.

LinuxPluginLoadingTests verifies loading, error isolation, UI registration and
menu/toolbar actions with fixture plugins. These tests run on Windows and do not
replace a Mono/X11 check.

For a manual Mono check, the existing VisualPlugins/TestPlugin project builds one
TestPlugin.dll for both shells. Copy it beside PascalABCNETLinux.exe.

After startup, check the Modules menu and compiler messages for Test Plugin v1.0.
Open the plugin form from the menu, close it, reopen from the toolbar, then return
to typing and compile/run a small program. The sample form is intentionally empty.
TestPlugin.dll is for this manual check and is not included by the normal Linux
release packaging script.

TeacherControlPlugin is built separately by scripts/build-teacher-control-linux.ps1
into bin/PluginsLinux. The Linux packaging script invokes this build and includes
that variant, LightPT.pcu and Tasks*.pcu. Only TeacherControl's Linux UI fixes are
selected by PluginPlatform=Linux; its contract is shared. The Linux shell supports
BeforeCompile, AfterAddInGUI, ChangeArgsBeforeRun and ConvertUnitTextProperty.
Fixture tests verify that AfterAddInGUI runs only
after menu and toolbar controls have been assigned.

For the TeacherControl manual check, start the IDE with a program in a working
directory containing lightpt.dat. The plugin intentionally hides its GUI without
that marker, just as on Windows. Test sign-in and a real LightPT task in ALT;
network authentication is not covered by the loader tests.
