// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using PascalABCCompiler.Errors;
using System.IO;
using System.Collections.Generic;

namespace PascalABCCompiler
{
    /*class mc<T>
    {
        public static int meth()
        {
            return 3;
        }
    }*/
	class ConsoleCompiler
	{        
        private static string StringsPrefix = "PABCNETC_";
        public static PascalABCCompiler.Compiler Compiler=null;
        public static string FileName = "";
        public static CompilerOptions.OutputType outputType;
        public static DateTime StartTime;

        private static string StringResourcesGet(string Key)
        {
            return StringResources.Get(StringsPrefix + Key);
        }

        public static void ShowConnectedParsers()
        {
            if (Compiler.SupportedSourceFiles.Length > 0)
            {
                Console.Write(StringResourcesGet("CONNECTED_PARSERS"));
                foreach (PascalABCCompiler.SupportedSourceFile ssf in Compiler.SupportedSourceFiles)
                    Console.Write(ssf+"; ");
                Console.WriteLine();
            }
        }

        public static int Main(string[] args)
		{
            DateTime ldt = DateTime.Now;
            PascalABCCompiler.StringResourcesLanguage.LoadDefaultConfig();

            Compiler = new PascalABCCompiler.Compiler(null, null);
            StringResourcesLanguage.CurrentLanguageName = StringResourcesLanguage.AccessibleLanguages[0];
            //Console.WriteLine("OK {0}ms", (DateTime.Now - ldt).TotalMilliseconds);
            ldt = DateTime.Now;
            
            //ShowConnectedParsers();

            if (args.Length == 0)
            {
                Console.WriteLine(StringResourcesGet("COMMANDLINEISABSENT"));
                return 2;
            }

            FileName = args[0];
            if (!File.Exists(FileName))
            {
                Console.WriteLine(StringResourcesGet("FILEISABSENT{0}"),FileName);
                return 3;
            }
            outputType = CompilerOptions.OutputType.ConsoleApplicaton;

            CompilerOptions co = new CompilerOptions(FileName, outputType);
            if (args.Length==1)
                co.OutputDirectory = "";
            else co.OutputDirectory = args[1];
            co.Rebuild = false;
            co.Debug = true;
            co.UseDllForSystemUnits = false;

            bool success = true;
            if (Compiler.Compile(co) != null)
                Console.WriteLine("OK");
            else
            {
                Console.WriteLine(StringResourcesGet("COMPILEERRORS"));
                success = false;
            }
            // Console.WriteLine("OK {0}ms", (DateTime.Now - ldt).TotalMilliseconds); /////

            for (int i = 0; i < Compiler.ErrorsList.Count; i++)
            {
                if (Compiler.ErrorsList[i] is LocatedError)
                {
                    SourceLocation sl;
                    if ((sl = (Compiler.ErrorsList[i] as LocatedError).SourceLocation) != null)
                        Console.WriteLine(string.Format("[{0},{1}] {2}: {3}", sl.BeginPosition.Line, sl.BeginPosition.Column, Path.GetFileName(sl.FileName), Compiler.ErrorsList[i].Message));
                    else
                        Console.WriteLine(Compiler.ErrorsList[i]);
                }
                break; // выйти после первой же ошибки
            }
            if (success)
                return 0;
            else return 1;
		}
	}
}
