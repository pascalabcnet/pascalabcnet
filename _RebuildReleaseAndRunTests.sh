#!/bin/sh
MONO_IOMAP=case xbuild /p:Configuration=release pabcnetc.sln
MONO_IOMAP=case xbuild /p:Configuration=release CodeCompletion/CodeCompletion.csproj
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
export MONO_IOMAP=all
cd ReleaseGenerators
mono ../bin/pabcnetc.exe RebuildStandartModules.pas /rebuild
if [ $? -eq 0 ]; then
    cd PABCRtl
    mono ../../bin/pabcnetc.exe PABCRtl.pas /rebuild
    if [ $? -eq 0 ]; then
        sn -Vr PABCRtl.dll
        sn -R PABCRtl.dll KeyPair.snk
        sn -Vu PABCRtl.dll
        cp PABCRtl.dll ../../bin/Lib
        mono ../../bin/pabcnetc.exe PABCRtl32.pas /rebuild
        if [ $? -eq 0 ]; then
            sn -Vr PABCRtl32.dll
            sn -R PABCRtl32.dll KeyPair32.snk
            sn -Vu PABCRtl32.dll
            cp PABCRtl32.dll ../../bin/Lib
            gacutil -u PABCRtl
            gacutil -i ../../bin/Lib/PABCRtl.dll
	    cd ..
            mono ../bin/pabcnetc.exe RebuildStandartModules.pas /rebuild
            if [ $? -eq 0 ]; then
                cd ../bin
                mono TestRunner.exe
                cd ..
            fi
        fi
    fi
fi
