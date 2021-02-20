@echo off

cd ..\..
@echo CS:
Utils\LinesCount\LinesCount.exe *.cs

cd bin\lib
@echo LIB\PAS:
..\..\Utils\LinesCount\LinesCount.exe *.pas

pause