@echo off
chcp 65001 >nul
for /r "%~dp0" %%F in (*.exe) do del /q "%%F"
for /r "%~dp0" %%F in (*.pdb) do del /q "%%F"
echo Done.
pause
