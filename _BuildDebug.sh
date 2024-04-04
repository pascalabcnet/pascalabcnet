#!/bin/sh -e
MONO_IOMAP=case msbuild /p:Configuration=Debug pabcnetc.sln
MONO_IOMAP=case msbuild /p:Configuration=Debug PascalABCNETLinux.sln
