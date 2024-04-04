// Copyright (©) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using PascalABCCompiler.Errors;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

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
        private const int DefaultConsoleBufferWidth = 80;

        private static string StringsPrefix = "PABCNETC_";
        static bool short_output = true;
        static string BlankString = "";
        private static string StringResourcesGet(string Key)
        {
            return StringResources.Get(StringsPrefix + Key);
        }
        public static bool IgnoreNotSupportedError = false;
        public static PascalABCCompiler.Compiler Compiler=null;
        public static string FileName = "";
        public static string TagertFileName = "";
        public static string OutputDirectory = "";
        public static bool RestartOnNewCompile = false;
        public static bool DetailOut = false;
        public static bool Rebuild=false;
        public static bool NoConsole = false;
        public static CompilerOptions.OutputType outputType;
        public static bool Debug = true;
        public static bool UseDll = false;
        public static List<Error> GlobalErrorsList = new List<Error>();
        public static DateTime StartTime;
        public static uint AllLinesCompiled;
        public static bool ScanSubdirs = false;
            
        public static bool ExecuteCommand(string command)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            Console.ForegroundColor = ConsoleColor.Gray;
            if (command == "" || command == null) return false;
            if (command.ToLower().IndexOf("cd ") == 0)
            {
                try
                {
                    Environment.CurrentDirectory = (new DirectoryInfo(command.Remove(0, 3))).FullName;
                }
                catch (Exception)
                {
                    WriteErrorText(StringResourcesGet("ERROR_INVALID_DIRECTORY") + Environment.NewLine);
                }
                //Console.WriteLine("Current directory change to " + Environment.CurrentDirectory);
                return true;
            }
            if (command.ToLower() == "reset")
            {
                //Console.Clear();
                Reset();
                return true;
            }
            if (command.ToLower().IndexOf("resetoncompile=")==0)
            {
                RestartOnNewCompile = command.IndexOf("1")>0;
                Console.WriteLine("ResetOnCompile==" + RestartOnNewCompile);
                return true;
            }
            if (command.ToLower().IndexOf("ignorense=") == 0)
            {
                IgnoreNotSupportedError = command.IndexOf("1") > 0;
                Console.WriteLine("IgnoreNotSupportedError==" + IgnoreNotSupportedError);
                return true;
            }
            if (command.ToLower().IndexOf("scansubdirs=") == 0)
            {
                ScanSubdirs = command.IndexOf("1") > 0;
                Console.WriteLine("ScanSubdirs==" + ScanSubdirs);
                return true;
            }
            if (command.ToLower().IndexOf("debug=") == 0)
            {
                Debug = command.IndexOf("1") > 0;
                Console.WriteLine("Debug==" + Debug);
                return true;
            }
            if (command.ToLower().IndexOf("usedll=") == 0)
            {
                UseDll = command.IndexOf("1") > 0;
                Console.WriteLine("UseDll==" + UseDll);
                return true;
            }
            if (command.ToLower().IndexOf("showallmessages=") == 0)
            {
                DetailOut = command.IndexOf("1") > 0;
                Console.WriteLine("ShowAllMessages==" + DetailOut);
                return true;
            } if (command.ToLower().IndexOf("outdir=") == 0)
            {
                OutputDirectory = command.Remove(0, 7);
                Console.WriteLine("OutDir==" + OutputDirectory);
                return true;
            }
            if (command.ToLower().IndexOf("outtype=") == 0)
            {
                SetOutType(command.Remove(0, 8));
                Console.WriteLine("OutType==" + outputType);
                return true;
            }
            if (command.ToLower()=="clrscr")
            {
                Console.Clear();
                return true;
            }
            if (command.ToLower() == "collect")
            {
                GC.Collect();
                return true;
            }
            if (command.ToLower().IndexOf("rebuild=") == 0)
            {
                Rebuild = command.IndexOf("1") > 0;
                Console.WriteLine("Rebuild==" + Rebuild);
                return true;
            }
            if (command.ToLower().IndexOf("language=") == 0)
            {
                int i=command.IndexOf("=");
                int n = Convert.ToInt32(command.Substring(i + 1, command.Length-i-1));
                if (n < StringResourcesLanguage.AccessibleLanguages.Count && n >= 0)
                {
                    StringResourcesLanguage.CurrentLanguageName = StringResourcesLanguage.AccessibleLanguages[n];
                    ExecuteCommand("ClrScr"); ExecuteCommand("?");
                }
                else
                    Console.WriteLine("Language=" + StringResourcesLanguage.CurrentLanguageName);
                return true;
            }
            if (command == "?")
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(StringResourcesGet("HELP"));
                for (int i = 0; i < StringResourcesLanguage.AccessibleLanguages.Count;i++)
                    Console.WriteLine(string.Format("\t{0}-{1}", i, StringResourcesLanguage.AccessibleLanguages[i]));
                //int jjj = 5;
                //bool bbb= jjj < mc<int>.meth();

                return true;
            }

            if (ScanSubdirs)
                StartScanDir(command, Environment.CurrentDirectory);
            else
                CompileAllFilesInDirectory(command, Environment.CurrentDirectory);
            
            return true;
        }

        static int AllFilesCount;
        static int SuccessFilesCount;
        static void StartScanDir(string mask, string directory)
        {
            AllFilesCount = SuccessFilesCount = 0;
            ScanDir(mask, directory);
            Console.WriteLine(string.Format(StringResourcesGet("TOTAL{0}_SUCCESS{1}_PROCENT{2}"), AllFilesCount, SuccessFilesCount, Convert.ToInt32((double)SuccessFilesCount / (double)AllFilesCount * 100.0)));

        }
        
        static void ScanDir(string mask,string directory)
        {
            CompileAllFilesInDirectory(mask, directory);
            DirectoryInfo[] dirs = (new DirectoryInfo(directory)).GetDirectories();
            foreach (DirectoryInfo dinf in dirs)
                if (directory.ToLower() != dinf.FullName)
                    ScanDir(mask, dinf.FullName);
        }

        static void CompileAllFilesInDirectory(string mask,string directory)
        {
            DirectoryInfo di = new DirectoryInfo(directory);
            
            //Console.WriteLine("["+directory+"]");
            FileInfo[] files;
            try
            {
                files = di.GetFiles(mask);
            }
            catch (DirectoryNotFoundException)
            {
                WriteErrorText(StringResourcesGet("ERROR_DIRECTORY_NOT_FOUND") + Environment.NewLine);
                return;
            }
            catch (ArgumentException)
            {
                WriteErrorText(StringResourcesGet("ERROR_INVALID_DIRECTORY") + Environment.NewLine);
                return;
            }
            DateTime dt = DateTime.Now;
            long length = 0;
            GlobalErrorsList.Clear();
            AllLinesCompiled = 0;
            foreach (FileInfo fi in files)
            {
                CompileAssembly(fi.FullName, outputType, false);
                AllLinesCompiled += Compiler.LinesCompiled;
                length += fi.Length;
            }
            if (!short_output)
            if (GlobalErrorsList.Count > 0 && files.Length > 1)
            {
                WriteErrorText(StringResourcesGet("FULL_ERROR_LIST") + Environment.NewLine);
                for (int i = 0; i < GlobalErrorsList.Count; i++)
                {
                    if (GlobalErrorsList[i] is ProgramModuleExpected || GlobalErrorsList[i] is ParserBadFileExtension)
                        WriteColorText("[" + i + "]", ConsoleColor.Green);
                    else
                        if (GlobalErrorsList[i] is SemanticError)
                            WriteColorText("[" + i + "]", ConsoleColor.Red);
                        else
                            WriteErrorText("[" + i + "]");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (GlobalErrorsList[i] is LocatedError)
                    {
                        SourceLocation sl;
                        if ((sl = (GlobalErrorsList[i] as LocatedError).SourceLocation) != null)
                            Console.WriteLine(string.Format("[{0},{1}] {2}: {3}", sl.BeginPosition.Line, sl.BeginPosition.Column, Path.GetFileName(sl.FileName), GlobalErrorsList[i].Message));
                        else
                            Console.WriteLine(GlobalErrorsList[i]);
                    }
                    else
                    {
                        if(GlobalErrorsList[i] is CompilerInternalError)
                            Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(GlobalErrorsList[i]);
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            if (short_output)
                ClearLine();
            Console.WriteLine(string.Format(StringResourcesGet("TOTAL{0}_FILES{1}_LINES_{2}_TIME{3}"), files.Length, (length / 1024), AllLinesCompiled, (DateTime.Now - dt).TotalMilliseconds));
        }

        public static void ClearLine()
        {
            if (NoConsole)
            {
                return;
            }

            Console.CursorLeft = 0;
            Console.Write(BlankString);
            Console.CursorLeft = 0;
        }

        public static int CompileAssembly(string FileName, CompilerOptions.OutputType outputType, bool PauseIfError)
        {
            if (RestartOnNewCompile)
                Reset();
            StartTime = DateTime.Now;
            Console.ForegroundColor = ConsoleColor.White;
            string msg = string.Format(StringResourcesGet("COMPILING_ASSEMBLY{0}"), System.IO.Path.GetFileName(FileName));
            if (short_output)
            {
                ClearLine();
                Console.Write(msg);
            }
            else
                Console.WriteLine(msg);
            DateTime ldt = DateTime.Now;
            CompilerOptions co = new CompilerOptions(FileName, outputType);
            if (FileName.ToLower().EndsWith(".pabcproj"))
                co.ProjectCompiled = true;
            if (OutputDirectory != "")
                co.OutputDirectory=OutputDirectory;
            co.Rebuild = Rebuild;
            co.Debug = Debug;
            co.UseDllForSystemUnits = UseDll;
            AllFilesCount++;
            if (Compiler.Compile(co) != null)
                SuccessFilesCount++;
            if (IgnoreNotSupportedError)
                if ((Compiler.ErrorsList.Count == 1) && (Compiler.ErrorsList[0] is Errors.SemanticNonSupportedError))
                {
                    Compiler.ErrorsList.Clear();
                    SuccessFilesCount++;
                }
            if (Compiler.ErrorsList.Count > 0)
            {
                //Console.Beep();
                if (short_output) Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Gray;
                for (int i = 0; i < Compiler.ErrorsList.Count; i++)
                {
                    WriteErrorText("[" + i + "]");
                    if (Compiler.ErrorsList[i] is LocatedError)
                    {
                        SourceLocation sl;
                        if ((sl = (Compiler.ErrorsList[i] as LocatedError).SourceLocation) != null)
                            Console.WriteLine(string.Format("[{0},{1}] {2}: {3}", sl.BeginPosition.Line, sl.BeginPosition.Column, Path.GetFileName(sl.FileName), Compiler.ErrorsList[i].Message));
                        else
                            Console.WriteLine(Compiler.ErrorsList[i]);
                    }
                    else
                    {
                        if (Compiler.ErrorsList[i] is CompilerInternalError)
                            Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(Compiler.ErrorsList[i]);
                    }
                }
                GlobalErrorsList.AddRange(Compiler.ErrorsList);
                if (PauseIfError)
                    Console.ReadKey();
            }
            else
            {
                if (!short_output)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(string.Format(StringResourcesGet("OK_{0}MS_{1}LINES"), (DateTime.Now - ldt).TotalMilliseconds, Compiler.LinesCompiled));
                }
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            return Compiler.ErrorsList.Count;
        }
        private static void ChangeCompilerState(ICompiler sender, PascalABCCompiler.CompilerState State, string FileName)
        {
            if (DetailOut)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                if (FileName != null)
                {
                    FileName = string.Format("[{0}]{1} {2}...", Math.Round((DateTime.Now - StartTime).TotalMilliseconds), State, System.IO.Path.GetFileName(FileName));
                    StartTime = DateTime.Now;
                    Console.WriteLine(FileName);
                    //Console.Title = file_name;
                }
                else
                {
                    Console.WriteLine(string.Format("{0}.", State));
                    //Console.Title = State.ToString();
                }
            }
        }
        public static void WriteColorText(string Text, ConsoleColor FGColor)
        {
            Console.ForegroundColor = FGColor;
            Console.Write(Text);
            Console.ResetColor();
        }
        public static void WriteErrorText(string Text)
        {
            WriteColorText(Text, ConsoleColor.Red);
        }
        public static void Reset()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(StringResourcesGet("RESTARTING_COMPILER"));
            //StringResourcesLanguage.CurrentLanguageName = "–усский";
            DateTime ldt = DateTime.Now;
            Compiler = new PascalABCCompiler.Compiler(null,ChangeCompilerState);
            //GC.Collect();
            WriteColorText(Compiler.Banner + "\nCopyright (c) 2005-2024 by Ivan Bondarev, Stanislav Mikhalkovich\n", ConsoleColor.Green);
            Console.WriteLine("OK {0}ms", (DateTime.Now - ldt).TotalMilliseconds);
            if (Compiler.SupportedSourceFiles.Length == 0)
                WriteColorText(StringResourcesGet("ERROR_PARSERS_NOT_FOUND")+Environment.NewLine, ConsoleColor.Red);
            Compiler.InternalDebug.SkipPCUErrors = false;
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
        public static void ShowConnectedConversions()
        {
            if (Compiler.SemanticTreeConvertersController.SemanticTreeConverters.Count > 0)
            {
                Console.Write(StringResourcesGet("CONNECTED_CONVERSIONS"));
                foreach (PascalABCCompiler.SemanticTreeConverters.ISemanticTreeConverter sc in Compiler.SemanticTreeConvertersController.SemanticTreeConverters)
                    Console.Write(sc.Name + "; ");
                Console.WriteLine();
            }
        }
        public static void SetOutType(string type)
        {
            type = type.ToLower();
            if (type == "dll")
                outputType = CompilerOptions.OutputType.ClassLibrary;
            if (type == "exe")
                outputType = CompilerOptions.OutputType.ConsoleApplicaton;
            if (type == "winexe")
                outputType = CompilerOptions.OutputType.WindowsApplication;
        }


        public static int Main(string[] initialArgs)
        {
            var args = initialArgs.ToList();
            if (args.Remove("/noconsole"))
            {
                NoConsole = true;
            }

            PascalABCCompiler.StringResourcesLanguage.LoadDefaultConfig();

            if (args.Count == 1 && args[0] == "commandmode")
            {
                return (new CommandConsoleCompiler()).Run();
            }
            else
            {
                CultureInfo ci = CultureInfo.InstalledUICulture;
                if (StringResourcesLanguage.CurrentTwoLetterISO == "ru" && ci.TwoLetterISOLanguageName != "ru")
                    StringResourcesLanguage.CurrentLanguageName = StringResourcesLanguage.AccessibleLanguages[StringResourcesLanguage.TwoLetterISOLanguages.IndexOf("en")];
                else
                    StringResourcesLanguage.CurrentLanguageName = StringResourcesLanguage.AccessibleLanguages[StringResourcesLanguage.TwoLetterISOLanguages.IndexOf(StringResourcesLanguage.CurrentTwoLetterISO)];
                
            }
                

            for (int i = 0; i < ConsoleBufferWidth-1; i++)
              BlankString += " ";

            Console.ForegroundColor = ConsoleColor.White;
            OutputDirectory="";
            
            Console.Title = StringResourcesGet("STARTING");

            // загрузка всех парсеров и других составляющих языков  EVA
            LanguageIntegration.LanguageIntegrator.LoadAllLanguages();
            
            Reset();
            Console.Title = Compiler.Banner;
            

            //Console.CursorLeft = (Console.BufferWidth - Compiler.Banner.Length) / 2;
            //Console.WriteLine(Compiler.Banner); 


            short_output = args.Count != 1;

            if (args.Count >= 1) 
            {
                FileName = args[0];
            }
            if (args.Count >= 2)
                if (args[1] == "/rebuildnodebug")
                {
                    Rebuild = true;
                    Compiler.InternalDebug.IncludeDebugInfoInPCU = false;
                    short_output = false;
                    DetailOut = true;
                }
                else
                    if (args[1] == "/rebuild")
                    {
                        Rebuild = true;
                        short_output = false;
                        DetailOut = true;
                    }
                    else
                        TagertFileName = args[1];
            outputType = CompilerOptions.OutputType.ConsoleApplicaton;
            if (args.Count >= 3)
                    SetOutType(args[2]);
            


            ShowConnectedParsers();
            ShowConnectedConversions();
            if (args.Count >= 1)
            {
                return CompileAssembly(FileName, outputType, true);
            }
            ExecuteCommand("?");
            while (true)
            {
                
                Console.WriteLine();
                string Dir = Environment.CurrentDirectory; int dleng = 25;
                Console.ForegroundColor = ConsoleColor.Gray;
                if (Dir.Length > dleng)
                    Dir = string.Format("...{0}>", Dir.Substring(Dir.Length - dleng));
                else
                    Dir = string.Format("{0}>", Dir);
                Console.Write(Dir);
                Console.ForegroundColor = ConsoleColor.Green;
                if (!ExecuteCommand(Console.ReadLine()))
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return 0;
                }
            }
		}

        private static int ConsoleBufferWidth
        {
            get { return NoConsole ? DefaultConsoleBufferWidth : Console.BufferWidth; }
        }

/*        public class MainClass
        {
            public static void Main(string[] args)
            {

            }
        }*/
         
	}
}
