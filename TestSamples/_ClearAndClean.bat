del ..\bin\lib\*.pcu
del ..\Realase\PascalABC.NET\Lib\*.pcu
del ..\Realase\PascalABC.NET\Samples\*.exe
del ..\Realase\PascalABC.NET\Samples\GraphABC\*.exe
del ..\Realase\PascalABC.NET\Samples\Interpreter\*.exe
del ..\Realase\PascalABC.NET\Samples\Interpreter\*.pcu
del ..\Realase\PascalABC.NET\Samples\BF\*.exe
del ..\Realase\PascalABC.NET\Samples\CRT\*.exe
del ..\Realase\PascalABC.NET\Samples\Recursion\*.exe
del ..\Realase\PascalABC.NET\Samples\Params\*.exe
del ..\Realase\PascalABC.NET\Samples\Fractals\*.exe
del ..\Realase\PascalABC.NET\Samples\Exceptions\*.exe
del ..\Realase\PascalABC.NET\Samples\OperatorOverloading\*.exe
del bad\*.exe
del bad\*.pcu
del *.pcu
del *.exe
del *.dll
del *.pdb
del *.dll
del *.duo
del abc.ini

%windir%\microsoft.net\framework\v2.0.50727\msbuild /t:clean ..\PascalABCNET.sln
@IF %ERRORLEVEL% NEQ 0 PAUSE

del ..\*.suo /a:h
@echo Deleting obj folders...
@echo off
rmdir ..\Compiler\obj /s /q
rmdir ..\CompilerTools\obj /s /q
rmdir ..\Localization\obj /s /q
rmdir ..\NetGenerator\obj /s /q
rmdir ..\pabcnetc\obj /s /q
rmdir ..\PluginsSupport\obj /s /q
rmdir ..\ParserTools\obj /s /q
rmdir ..\SemanticTree\obj /s /q
rmdir ..\SyntaxTree\obj /s /q
rmdir ..\TreeConverter\obj /s /q
rmdir ..\VisualPascalABCNET\obj /s /q
rmdir ..\VisualPascalABCNET_NEW\obj /s /q
rmdir ..\Parsers\BFParser\obj /s /q
rmdir ..\Parsers\Oberon2Parser\obj /s /q
rmdir ..\Parsers\PascalABCParser\obj /s /q
rmdir ..\Parsers\CParser\obj /s /q
rmdir ..\VisualPlugins\CompilerController\obj /s /q
rmdir ..\VisualPlugins\InternalErrorReport\obj /s /q
rmdir ..\VisualPlugins\PT4Provider\obj /s /q
rmdir ..\VisualPlugins\SyntaxTreeVisualisator\obj /s /q
rmdir ..\VisualPlugins\SourceTextFormater\obj /s /q
rmdir ..\Errors\bin /s /q
rmdir ..\Errors\obj /s /q
rmdir ..\Optimizer\obj /s /q

@echo on
@echo Done.