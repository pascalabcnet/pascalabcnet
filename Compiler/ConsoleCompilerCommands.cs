// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;

namespace PascalABCCompiler
{
    public class ConsoleCompilerConstants
    {
        public const int MaxProcessMemoryMB = 500;
        
        public const string DataSeparator = "]\r\n[";
        public static readonly string MessageSeparator = "\002\002\002";

        //1xx to remoteCompiler
        public const int CommandStartNumber = 100;

        public const int LinesCompiled = 180;
        public const int BeginOffest = 181;
        public const int Error = 182;
        public const int Warning = 183;
        public const int FileExsist = 184;
        public const int SourceFileText = 185;
        public const int GetLastWriteTime = 186;
        public const int VarBeginOffest = 187;
        public const int InternalError = 188;
        public const int PABCHealth = 189;


        //2xx to pabcnetc.exe
        public const int CommandExit = 200;
        public const int CommandGCCollect = 201;
        public const int CommandCompile = 210;
        public const int CompilerOptionsOutputType = 211;
        public const int CompilerOptionsDebug = 212;
        public const int CompilerOptionsOutputDirectory = 213;
        public const int CompilerOptionsRebuild= 214;
        public const int CompilerOptionsFileName = 215;
        public const int InternalDebugSavePCU = 216;
        public const int WorkingSet = 217;
        public const int CompilerOptionsStandartModule = 218;
        public const int CompilerOptionsClearStandartModules = 219;
        public const int InternalDebug = 221;
        public const int CompilerOptions = 222;
        public const int CompilerOptionsForDebugging = 223;
        public const int CompilerOptionsRunWithEnvironment = 224;
        public const int CompilerOptionsProjectCompiled = 225;
        public const int UseDllForSystemUnits = 226;
        public const int IDELocale = 227;
        public const int CompilerLocale = 228;

        static ConsoleCompilerConstants()
        {
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix || System.Environment.OSVersion.Platform == System.PlatformID.MacOSX)
                MessageSeparator = "*\002\002\002*";
        }
    }
}
