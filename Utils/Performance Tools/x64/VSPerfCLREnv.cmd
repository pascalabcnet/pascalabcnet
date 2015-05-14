@echo off

set SAMPLING=0

@REM
@REM This is the Sampling and Tracing Profiler GUIDs
@REM
set SAMPLE_GUID={2CCFACEE-5E60-4734-8A98-181D93097CD9}
set TRACE_GUID={B61B010D-1035-48A9-9833-32C2A2CDC294}

if /i "%1"=="/sampleon"             goto sample_on
if /i "%1"=="/samplegc"             goto sample_gc
if /i "%1"=="/samplegclife"         goto sample_gclife
if /i "%1"=="/samplelineoff"        goto profile_global

if /i "%1"=="/traceon"              goto trace_on
if /i "%1"=="/tracegc"              goto trace_gc
if /i "%1"=="/tracegclife"          goto trace_gclife

if /i "%1"=="/interactionon"        goto interaction_on

if /i "%1"=="/off"                  goto profile_off

if /i "%1"=="/globalsampleon"       goto profile_global
if /i "%1"=="/globalsamplegc"       goto profile_global
if /i "%1"=="/globalsamplegclife"   goto profile_global

if /i "%1"=="/globaltraceon"        goto profile_global
if /i "%1"=="/globaltracegc"        goto profile_global
if /i "%1"=="/globaltracegclife"    goto profile_global
if /i "%1"=="/globalinteractionon"  goto profile_global

if /i "%1"=="/globaloff"            goto profile_global

if /i "%1"=="/clrswoff"             goto profile_global
if /i "%1"=="/clrswon"              goto profile_global

if /i "%1"=="/?" goto usage
goto usage

:sample_on
@echo Enabling VSPerf Sampling Attach Profiling. Allows to 'attaching' to managed applications.
set SAMPLING=1
set COR_ENABLE_PROFILING=1
set COR_PROFILER=%SAMPLE_GUID%
set COR_LINE_PROFILING=1
set COR_INTERACTION_PROFILING=0
set COR_GC_PROFILING=0
set CORECLR_ENABLE_PROFILING=%COR_ENABLE_PROFILING%
set CORECLR_PROFILER=%COR_PROFILER%
title VSPerf Sampling Attaching 'ON'
goto show_settings

:sample_gc
@echo Enabling VSPerf Sampling Attach Profiling. Allows to 'attaching' to managed applications.
set SAMPLING=1
set COR_ENABLE_PROFILING=1
set COR_PROFILER=%SAMPLE_GUID%
set COR_LINE_PROFILING=1
set COR_INTERACTION_PROFILING=0
set COR_GC_PROFILING=1
set CORECLR_ENABLE_PROFILING=%COR_ENABLE_PROFILING%
set CORECLR_PROFILER=%COR_PROFILER%
title VSPerf Sampling Attaching and Allocation Profiling 'ON'
goto show_settings

:sample_gclife
@echo Enabling VSPerf Sampling Attach Profiling. Allows to 'attaching' to managed applications.
set SAMPLING=1
set COR_ENABLE_PROFILING=1
set COR_PROFILER=%SAMPLE_GUID%
set COR_LINE_PROFILING=1
set COR_INTERACTION_PROFILING=0
set COR_GC_PROFILING=2
set CORECLR_ENABLE_PROFILING=%COR_ENABLE_PROFILING%
set CORECLR_PROFILER=%COR_PROFILER%
title VSPerf Sampling Attaching and Allocation Profiling with objects lifetime 'ON'
goto show_settings

:trace_on
@echo Enabling VSPerf Trace Profiling of managed applications (excluding allocation profiling).
set COR_ENABLE_PROFILING=1
set COR_PROFILER=%TRACE_GUID%
set COR_LINE_PROFILING=0
set COR_INTERACTION_PROFILING=0
set COR_GC_PROFILING=0
title VSPerf Trace Profiling 'ON'
goto show_settings

:trace_gc
@echo Enabling VSPerf Trace Profiling of managed applications (including allocation profiling).
set COR_ENABLE_PROFILING=1
set COR_PROFILER=%TRACE_GUID%
set COR_LINE_PROFILING=0
set COR_INTERACTION_PROFILING=0
set COR_GC_PROFILING=1
title VSPerf Trace and Allocation Profiling 'ON'
goto show_settings

:trace_gclife
@echo Enabling VSPerf Trace Profiling of managed applications (including allocation profiling and objects lifetime).
set COR_ENABLE_PROFILING=1
set COR_PROFILER=%TRACE_GUID%
set COR_LINE_PROFILING=0
set COR_INTERACTION_PROFILING=0
set COR_GC_PROFILING=2
title VSPerf Trace and Allocation Profiling with objects lifetime 'ON'
goto show_settings

:interaction_on
@echo Enables collection of interaction profiling data for managed applications
set COR_INTERACTION_PROFILING=1
goto show_settings

:profile_off
@echo Disabling VSPerf Attach or Trace Profiling.
set COR_ENABLE_PROFILING=
set COR_PROFILER=
set COR_LINE_PROFILING=
set COR_INTERACTION_PROFILING=
set COR_GC_PROFILING=
set CORECLR_ENABLE_PROFILING=
set CORECLR_PROFILER=
title VSPerf Sampling Attaching / Tracing 'OFF'
goto show_settings

:profile_global
@rem
@rem make output file
@rem
IF NOT DEFINED TEMP SET TEMP=.\
SET TEMPF="%TEMP%\VSPERFENV.js"
del /Q %TEMPF% >nul 2>&1

@rem Wrap up our registry calls in a try catch block
echo try { > %TEMPF%

if /i "%1"=="/globalsampleon"       goto profile_global_sample_on
if /i "%1"=="/globalsamplegc"       goto profile_global_sample_gc
if /i "%1"=="/globalsamplegclife"   goto profile_global_sample_gclife
if /i "%1"=="/globaltraceon"        goto profile_global_trace_on
if /i "%1"=="/globaltracegc"        goto profile_global_trace_gc
if /i "%1"=="/globaltracegclife"    goto profile_global_trace_gclife
if /i "%1"=="/globaloff"            goto profile_global_off
if /i "%1"=="/clrswoff"             goto profile_global_CLRsw_off
if /i "%1"=="/clrswon"              goto profile_global_CLRsw_on
if /i "%1"=="/samplelineoff"        goto profile_global_sample_line_off
if /i "%1"=="/globalinteractionon"  goto profile_global_interaction_on

:profile_global_CLRsw_on
@echo Enabling CLR stack walk. To disable it, use /clrswoff
@rem
@rem Set to 1 key in HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment
@rem
echo var WshShell = WScript.CreateObject("WScript.Shell"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\F1.CLR_SW", "1", "REG_SZ"); >> %TEMPF%
title CLR stack walk 'ON'
goto global_finish_no_reboot

:profile_global_CLRsw_off
@echo Disabling CLR stack walk. To enable it, use /clrswon
@rem
@rem Set to 0 key in HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment
@rem
echo var WshShell = WScript.CreateObject("WScript.Shell"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\F1.CLR_SW", "0", "REG_SZ"); >> %TEMPF%
title CLR stack walk 'OFF'
goto global_finish_no_reboot

:profile_global_sample_on
@echo Enabling VSPerf Global Profiling. Allows to 'attaching' to managed services.
@rem
@rem Add to HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment
@rem
echo var WshShell = WScript.CreateObject("WScript.Shell"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_ENABLE_PROFILING", "1", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_PROFILER", "%SAMPLE_GUID%", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\CORECLR_ENABLE_PROFILING", "1", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\CORECLR_PROFILER", "%SAMPLE_GUID%", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_LINE_PROFILING", "1", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_INTERACTION_PROFILING", "0", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_GC_PROFILING", "0", "REG_SZ"); >> %TEMPF%
title VSPerf Global Profiling 'ON'
goto global_finish

:profile_global_sample_gc
@echo Enabling VSPerf Global Profiling. Allows to 'attaching' to managed services.
@rem
@rem Add to HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment
@rem
echo var WshShell = WScript.CreateObject("WScript.Shell"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_ENABLE_PROFILING", "1", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_PROFILER", "%SAMPLE_GUID%", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\CORECLR_ENABLE_PROFILING", "1", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\CORECLR_PROFILER", "%SAMPLE_GUID%", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_LINE_PROFILING", "1", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_INTERACTION_PROFILING", "0", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_GC_PROFILING", "1", "REG_SZ"); >> %TEMPF%
title VSPerf Global Profiling 'ON'
goto global_finish

:profile_global_sample_gclife
@echo Enabling VSPerf Global Profiling. Allows to 'attaching' to managed services.
@rem
@rem Add to HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment
@rem
echo var WshShell = WScript.CreateObject("WScript.Shell"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_ENABLE_PROFILING", "1", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_PROFILER", "%SAMPLE_GUID%", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_LINE_PROFILING", "1", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\CORECLR_ENABLE_PROFILING", "1", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\CORECLR_PROFILER", "%SAMPLE_GUID%", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_INTERACTION_PROFILING", "0", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_GC_PROFILING", "2", "REG_SZ"); >> %TEMPF%
title VSPerf Global Profiling 'ON'
goto global_finish

:profile_global_sample_line_off
@echo Excluding managed line-level data
set COR_LINE_PROFILING=0
set COR_LINE_PROFILING
@rem
@rem Add to HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment
@rem
echo var WshShell = WScript.CreateObject("WScript.Shell"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_LINE_PROFILING", "0", "REG_SZ"); >> %TEMPF%
goto global_finish_no_reboot

:profile_global_interaction_on
@rem
@rem Add to HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment
@rem
echo var WshShell = WScript.CreateObject("WScript.Shell"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_INTERACTION_PROFILING", "1", "REG_SZ"); >> %TEMPF%
goto global_finish_no_reboot

:profile_global_trace_on
@echo Enabling VSPerf Global Profiling. Allows trace profiling of managed services without allocation profiling.
@rem
@rem Add to HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment
@rem
echo var WshShell = WScript.CreateObject("WScript.Shell"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_ENABLE_PROFILING", "1", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_PROFILER", "%TRACE_GUID%", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_LINE_PROFILING", "0", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_INTERACTION_PROFILING", "0", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_GC_PROFILING", "0", "REG_SZ"); >> %TEMPF%
title VSPerf Global Profiling 'ON'
goto global_finish

:profile_global_trace_gc
@echo Enabling VSPerf Global Profiling. Allows trace profiling of managed services with allocation profiling.
@rem
@rem Add to HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment
@rem
echo var WshShell = WScript.CreateObject("WScript.Shell"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_ENABLE_PROFILING", "1", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_PROFILER", "%TRACE_GUID%", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_LINE_PROFILING", "0", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_INTERACTION_PROFILING", "0", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_GC_PROFILING", "1", "REG_SZ"); >> %TEMPF%
title VSPerf Global Profiling 'ON'
goto global_finish

:profile_global_trace_gclife
@echo Enabling VSPerf Global Profiling. Allows trace profiling of managed services with allocation profiling and objects lifetime.
@rem
@rem Add to HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment
@rem
echo var WshShell = WScript.CreateObject("WScript.Shell"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_ENABLE_PROFILING", "1", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_PROFILER", "%TRACE_GUID%", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_LINE_PROFILING", "0", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_INTERACTION_PROFILING", "0", "REG_SZ"); >> %TEMPF%
echo WshShell.RegWrite ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_GC_PROFILING", "2", "REG_SZ"); >> %TEMPF%
title VSPerf Global Profiling 'ON'
goto global_finish

:profile_global_off
@echo Disabling VSPerf Global Profiling.
@rem
@rem Remove key from HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment
@rem
echo  HKLM = 0x80000002; >> %TEMPF%
echo  sRegPath = "SYSTEM\\CurrentControlSet\\Services\\"; >> %TEMPF%
echo  WshShell = WScript.CreateObject("WScript.Shell");  >> %TEMPF%
echo  WshShell.RegDelete ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_ENABLE_PROFILING"); >> %TEMPF%
echo  WshShell.RegDelete ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_PROFILER"); >> %TEMPF%
echo  WshShell.RegDelete ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\CORECLR_ENABLE_PROFILING"); >> %TEMPF%
echo  WshShell.RegDelete ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\CORECLR_PROFILER"); >> %TEMPF%
echo  WshShell.RegDelete ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_LINE_PROFILING"); >> %TEMPF%
echo  WshShell.RegDelete ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_INTERACTION_PROFILING"); >> %TEMPF%
echo  WshShell.RegDelete ("HKLM\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\COR_GC_PROFILING"); >> %TEMPF%
echo    WScript.Echo( "You need to restart the service to detect the new settings. This may require a reboot of your machine." ); >> %TEMPF%
echo } >> %TEMPF%
echo catch (err) >> %TEMPF%
echo { >> %TEMPF%
echo      WScript.Echo( "Could not remove registry keys. Either they do not exist, or you are not running as an administrator." ); >> %TEMPF%
echo } >> %TEMPF%

goto global_runscript

:global_finish

@rem Add in the end of the try block, and the catch block
echo WScript.Echo( "You need to restart the service to detect the new settings. This may require a reboot of your machine." ); >> %TEMPF%

:global_finish_no_reboot

echo } >> %TEMPF%
echo catch (e) { >> %TEMPF%
echo WScript.Echo( ); >> %TEMPF%
echo WScript.Echo( "Error in writing to registry. You must be an administrator to change any global sampling settings." ); >> %TEMPF%
echo } >> %TEMPF%
goto global_runscript

:global_runscript

cscript //Nologo %TEMPF%
@rem del /Q %TEMPF% >nul 2>&1
set TEMPF=
goto end

:show_settings
@echo.
@echo Current Profiling Environment variables are:
set COR_ENABLE_PROFILING
set COR_PROFILER
set COR_LINE_PROFILING
set COR_INTERACTION_PROFILING
set COR_GC_PROFILING
if %SAMPLING%==0 goto end
set CORECLR_ENABLE_PROFILING
set CORECLR_PROFILER
goto end

:usage
@echo.
@echo Microsoft (R) Visual Studio Performance Tools .NET Profiling Utility for managed code
@echo Copyright (C) Microsoft Corporation. All rights reserved.
@echo.
@echo Usage: VSPerfCLREnv [/?]
@echo                     [/sampleon^|/samplegc^|/samplegclife^|/samplelineoff^|
@echo                      /traceon ^|/tracegc ^|/tracegclife ^|
@echo                      /interactionon^|
@echo                      /globalsampleon^|/globalsamplegc^|
@echo                      /globalsamplegclife^|/globaltraceon^|
@echo                      /globaltracegc^|/globaltracegclife^|
@echo                      /globalinteractionon^|
@echo                      /off^|/globaloff^]
@echo.
@echo This script is for setting the profiling environment for managed code.
@echo.
@echo Options:
@echo.
@echo./?                      Displays this help
@echo.
@echo /sampleon               Enables sampling 'attaching' to managed applications (excluding allocation profiling)
@echo.
@echo /samplegc               Enables sampling 'attaching' to managed applications (including allocation profiling)
@echo.
@echo /samplegclife           Enables sampling 'attaching' to managed applications (including allocation profiling and objects lifetime)
@echo.
@echo /samplelineoff          Disables collection of line-level profiling data for managed code
@echo.
@echo.
@echo /traceon                Enables trace profiling of managed applications (excluding allocation profiling)
@echo.
@echo /tracegc                Enables trace profiling of managed applications (including allocation profiling)
@echo.
@echo /tracegclife            Enables trace profiling of managed applications (including allocation profiling and objects lifetime)
@echo.
@echo /interactionon          Enables collection of interaction profiling data for managed applications
@echo.
@echo.
@echo /globalsampleon         Enables sampling 'attaching' to managed services (excluding allocation profiling)
@echo.
@echo /globalsamplegc         Enables sampling 'attaching' to managed services (including allocation profiling)
@echo.
@echo /globalsamplegclife     Enables sampling 'attaching' to managed services (including allocation profiling and objects lifetime)
@echo.
@echo.
@echo /globaltraceon          Enables trace profiling of managed services (excluding allocation profiling)
@echo.
@echo /globaltracegc          Enables trace profiling of managed services (including allocation profiling)
@echo.
@echo /globaltracegclife      Enables trace profiling of managed services (including allocation profiling and objects lifetime)
@echo.
@echo /globalinteractionon    Enables global collection of interaction profiling data for managed applications
@echo.
@echo.
@echo /off                    Disables sampling 'attaching' or trace reporting of managed applications
@echo.
@echo /globaloff              Disables sampling 'attaching' or trace profiling of managed services
@echo.

:end
set SAMPLE_GUID=
set TRACE_GUID=
set SAMPLING=
