#!/bin/sh
MONO_IOMAP=case xbuild pabcnetc.sln
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
            gacutil -i ../bin/Lib/PABCRtl.dll
            mono ../bin/pabcnetc.exe RebuildStandartModules.pas /rebuild
            if [ $? -eq 0 ]; then
                cd ../bin
                mono TestRunner.exe
                cd ..
            fi
        fi
    fi
fi