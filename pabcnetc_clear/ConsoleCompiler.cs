// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using PascalABCCompiler.Errors;
using System.IO;
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

        public static bool CheckAndSplitDirective(string directive, out string name, out string value)
        {
            System.Diagnostics.Debug.Assert(directive[0] == '/');
            directive = directive.Remove(0, 1);

            name = null;
            value = null;
            var ss = directive.Split( new[]{':'}, 2);
            name = ss[0].ToLower();
            if (ss.Length > 1)
            {
                value = ss[1].Trim();
            }

            return true;
        }

        public static bool ApplyDirective(string name, string value, CompilerOptions co)
        {
            switch (name)
            {
                case "debug":
                    switch (value)
                    {
                        case "0":
                            co.Debug = false;
                            co.Optimise = true;
                            return true;
                        case "1":
                            co.Debug = true;
                            return true;
                        default:
                            Console.WriteLine("Bad value in 'Debug' directive '{0}'. Acceptable values are 0 or 1", value);
                            return false;
                    }

                case "define":
                    co.ForceDefines.Add(value);
                    return true;

                case "output":
                    if (!Compiler.CheckPathValid(value))
                        return false;
                    co.OutputFileName = Path.GetFileName(value);
                    if (Path.IsPathRooted(value))
                    {
                        co.OutputDirectory = Path.GetDirectoryName(value);
                    }
                    return true;

                case "searchdir":
                    if (!Directory.Exists(value))
                    {
                        Console.WriteLine($"SearchDir \"{value}\" not found relative to \"{Environment.CurrentDirectory}\"");
                        return false;
                    }
                    co.SearchDirectories.Insert(0, value); // .Insert, чтобы определённые пользователем папки имели бОльший приоритет, чем стандартная
                    return true;

                case "locale":
                    co.Locale = value;
                    return true;

                default:
                    Console.WriteLine("No such directive name: '{0}'", name);
                    return false;
            }
        }

        public static bool CheckAndApplyDirective(string directive, CompilerOptions co)
        {
            string name, value;
            var b = CheckAndSplitDirective(directive,out name, out value);
            if (!b)
                return false;

            b = ApplyDirective(name, value, co);
            return b;
        }

        public static void OutputHelp()
        {
            Console.WriteLine("Command line: ");
            Console.WriteLine("pabcnetcclear /directive1:value1 /directive2:value2 ... [inputfile]\n");
            Console.WriteLine("Available directives:");
            //Console.WriteLine("  /Help  /H  /?"); - запретил - конкурирует с pabcnetcclear /w/a.pas
            Console.WriteLine("  /Debug:<0/1>");
            Console.WriteLine("  /Define:<name>");
            Console.WriteLine("  /Output:<[path\\]name>");
            Console.WriteLine("  /SearchDir:<path>");
            Console.WriteLine("  /Locale:<locale>");
            Console.WriteLine("  /Version:");
            Console.WriteLine();
            Console.WriteLine("/Help show this message");
            Console.WriteLine("/Output:<[path\\]name> compile into an executable called \"name\" and save it in \"path\" directory");
            Console.WriteLine("/Debug:0 generates code with all .NET optimizations");
            Console.WriteLine("/SearchDir:<path> add \"path\" to list of standart unit search directories. Last added paths would be searched first");
            Console.WriteLine("/Locale:<locale> set locale of main thread of compiled program executable");
            Console.WriteLine("/Version: outputs PascalABC.NET version");
        }

        public static int Main(string[] args)
		{
            // SSM 18.03.19 Делаю параметры командной строки. Формат: pabcnetc.exe /Dir1=value1 /Dir2=value2 ... fname dir
            // dir - это пережиток старого - так можно было задавать каталог раньше. Пока - увы - пусть останется
            // Пока сделаю только директивы /Help /H /? и /Debug=0(1)
            // Имя директивы - это одно слово. Равенства может не быть - тогда value директивы равно null
            // Вычленяем первое равенство и делим директиву: до него - name, после него - value. Если name или value - пустые строки, то ошибка
            // 2022 г. К сожалению, директива с / конкурирует с именами каталогов в Linux. Надо писать новый консольный компилятор

            DateTime ldt = DateTime.Now;
            PascalABCCompiler.StringResourcesLanguage.LoadDefaultConfig();

            // загрузка всех парсеров и других составляющих языков  EVA
            Languages.LanguageIntegrator.LoadAllLanguages();

            Compiler = new PascalABCCompiler.Compiler(null, null);
            Compiler.InternalDebug.SkipPCUErrors = false;
            CultureInfo ci = CultureInfo.InstalledUICulture;
            if (StringResourcesLanguage.CurrentTwoLetterISO == "ru" && ci.TwoLetterISOLanguageName != "ru")
                StringResourcesLanguage.CurrentLanguageName = StringResourcesLanguage.AccessibleLanguages[StringResourcesLanguage.TwoLetterISOLanguages.IndexOf("en")];
            else
                StringResourcesLanguage.CurrentLanguageName = StringResourcesLanguage.AccessibleLanguages[StringResourcesLanguage.TwoLetterISOLanguages.IndexOf(StringResourcesLanguage.CurrentTwoLetterISO)];

            //Console.WriteLine("OK {0}ms", (DateTime.Now - ldt).TotalMilliseconds);
            ldt = DateTime.Now;

            //ShowConnectedParsers();

            var n = args.Length;
            if (n == 0)
            {
                //Console.WriteLine(StringResourcesGet("COMMANDLINEISABSENT"));
                OutputHelp();
                return 2;
            }

            //  /W/a.pas расценивается как директива. Нужен более сложный паттерн: начинается с \буквы:
            //var n1 = args.TakeWhile(a => a[0] == '/').Count(); // количество директив
            // Поменяем: SSM 14/12/21
            var n1 = args.TakeWhile(a => System.Text.RegularExpressions.Regex.IsMatch(a, @"^/\w+:")).Count(); // количество директив

            if (n1<n-2)
            {
                Console.WriteLine("Error in argument {0}", args[n1+2]);
                Console.WriteLine("Command line cannot contain any arguments after filename '{0}' and outdirname '{1}'", args[n1], args[n1 + 1]);
                return 4;
            }

            if (n1 == n) // только директивы
            {
                string name, value;
                var b = CheckAndSplitDirective(args[0], out name, out value);
                if (!b) // Сообщение уже будет выдано
                    return 4;
                switch (name)
                {
                    case "help":
                    case "h":
                    case "?":
                        OutputHelp();
                        return 0;
                    case "version":
                        Console.WriteLine(PascalABCCompiler.Compiler.Version);
                        return 0;

                    default:
                        Console.WriteLine("Filename is absent. Nothing to compile");
                        return 4;
                }
            }

            FileName = args[n1]; // следующий аргумент за директивами - имя файла
            if (!File.Exists(FileName))
            {
                Console.WriteLine(StringResourcesGet("FILEISABSENT{0}"), "'" + FileName + "'");
                return 3;
            }

            outputType = CompilerOptions.OutputType.ConsoleApplicaton;

            CompilerOptions co = new CompilerOptions(FileName, outputType);
            if (FileName.ToLower().EndsWith(".pabcproj"))
                co.ProjectCompiled = true;
            if (n1 == n - 1)
                //co.OutputDirectory = ""
                ;
            else co.OutputDirectory = args[n - 1];
            co.Rebuild = false;
            co.Debug = false;
            co.UseDllForSystemUnits = false;

            for (var i = 0; i < n1; i++)
            {
                var b = CheckAndApplyDirective(args[i], co);
                if (!b)
                    return 4;
            }


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
                else
                {
                    Console.WriteLine(Compiler.ErrorsList[i].Message);
                    Console.WriteLine(Compiler.ErrorsList[i].StackTrace);
                }
                break; // выйти после первой же ошибки
            }
            if (success)
                return 0;
            else return 1;
		}
	}
}
