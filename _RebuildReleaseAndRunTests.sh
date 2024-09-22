#!/bin/sh -e
cd Utils/DefaultLanguageResMaker
mono LanguageResMaker.exe
cd ../..

MONO_IOMAP=case msbuild /p:Configuration=Release pabcnetc.sln
MONO_IOMAP=case msbuild /p:Configuration=Release PascalABCNETLinux.sln
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
export MONO_IOMAP=all
cd ReleaseGenerators
mono ../bin/pabcnetc.exe RebuildStandartModulesMono.pas  /rebuild
if [ $? -eq 0 ]; then
    if [ $? -eq 0 ]; then
      cd ../bin
        mono TestRunner.exe
        cd ..
    fi
fi
