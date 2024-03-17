// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using PascalABCCompiler.Parsers;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LanguageIntegration
{
    public static class LanguageIntegrator
    {

        public static Controller ParsersController = Controller.Instance;

        private const string languageKitsDirectoryName = "LanguageKits";

        /// <summary>
        /// Возвращает директорию, содержащую комплекты языков
        /// </summary>
        private static DirectoryInfo GetLanguageKitsDirectory()
        {
            string binDirectoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string languageKitsDirectoryPath = Path.Combine(binDirectoryPath, languageKitsDirectoryName);

            return new DirectoryInfo(languageKitsDirectoryPath);
        }

        /// <summary>
        /// Загружает все языковые комплекты для использования
        /// </summary>
        public static void LoadAllLanguageKits()
        {
            DirectoryInfo languageKitsDirectory = GetLanguageKitsDirectory();

            foreach (var languageKit in languageKitsDirectory.GetDirectories())
            {
                FileInfo[] dllFiles = languageKit.GetFiles();
                
                foreach (var dll in dllFiles)
                {
                    if (Regex.IsMatch(dll.Name, @".*Parser\.dll"))
                    {
                        ParsersController.IntegrateParsersFromAssembly(dll);
                    }
                }
            }
        }

        /// <summary>
        /// Загружает все языки, доступные в системе
        /// </summary>
        public static void LoadAllLanguages()
        {
            ParsersController.LoadStandardParsers();

            LoadAllLanguageKits();
        }

        /// <summary>
        /// Перезагружает парсеры всех языков
        /// </summary>
        public static void ReloadAllParsers()
        {
            ParsersController.Parsers.Clear();
            ParsersController.LoadStandardParsers();
            LoadParsersFromLanguageKits();
        }

        /// <summary>
        /// Загружает все парсеры из языковых комплектов
        /// </summary>
        public static void LoadParsersFromLanguageKits()
        {
            DirectoryInfo languageKitsDirectory = GetLanguageKitsDirectory();

            foreach (var parserDll in languageKitsDirectory.GetDirectories()
                .SelectMany(directory => directory.GetFiles("*Parser.dll")))
            {
                if (parserDll != null)
                    ParsersController.IntegrateParsersFromAssembly(parserDll);
            }
        }

    }
}
