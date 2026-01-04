#!/bin/sh -e
dotnet build -c Release --no-incremental pabcnetc.sln
dotnet build -c Release --no-incremental PascalABCNETLinux.sln
mono --aot bin/pabcnetc.exe
mono --aot bin/NETGenerator.dll
mono --aot bin/TreeConverter.dll
mono --aot bin/Compiler.dll
mono --aot bin/SyntaxTree.dll
mono --aot bin/SemanticTree.dll
mono --aot bin/PascalABCParser.dll
mono --aot bin/ParserTools.dll
mono --aot bin/CompilerTools.dll
mono --aot bin/OptimizerConversion.dll
mono --aot bin/Errors.dll
mono --aot bin/LanguageIntegrator.dll
mono --aot bin/StringConstants.dll
mono --aot bin/PascalABCLanguageInfo.dll

cd ReleaseGenerators
mono ../bin/pabcnetc.exe RebuildStandartModulesMono.pas  /rebuild
mono ../bin/pabcnetc RebuildStandartModulesSPython.pas /rebuild
if [ $? -eq 0 ]; then
    if [ $? -eq 0 ]; then
      cd ../TestSuite
        mono ../bin/TestRunner.exe
      cd ../TestSuiteAdditionalLanguages/SPythonTests
        mono ../../bin/TestRunner.exe
      cd ../..
    fi
fi
