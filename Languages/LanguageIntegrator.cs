// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using PascalABCCompiler.Errors;
using PascalABCCompiler.Parsers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Languages
{
    public static class LanguageIntegrator
    {

        public static LanguageProvider LanguageProvider = LanguageProvider.Instance;

        private const string languageKitsDirectoryName = "LanguageKits";

        public static event Action<ILanguage> LanguageConnected;
        public static event Action<string> LanguageLoadErrorOccured;

        /// <summary>
        /// Возвращает директорию, содержащую комплекты языков (если ее нет, то вернется null)
        /// </summary>
        private static DirectoryInfo GetLanguageKitsDirectory()
        {
            string binDirectoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string languageKitsDirectoryPath = Path.Combine(binDirectoryPath, languageKitsDirectoryName);

            if (Directory.Exists(languageKitsDirectoryPath))
                return new DirectoryInfo(languageKitsDirectoryPath);

            return null;
        }

        /// <summary>
        /// Загружает все языковые комплекты для использования
        /// </summary>
        public static void LoadAllLanguageKits()
        {
            DirectoryInfo languageKitsDirectory = GetLanguageKitsDirectory();

            if (languageKitsDirectory == null)
                return;

            foreach (var languageKit in languageKitsDirectory.GetDirectories())
            {
                FileInfo[] dllFiles = languageKit.GetFiles();
                
                foreach (var dll in dllFiles)
                {
                    if (dll.Name.EndsWith("Language.dll"))
                    {
                        IntegrateLanguageFromAssembly(dll);
                    }
                }
            }
        }

        /// <summary>
        /// Загружает все языки, доступные в системе
        /// </summary>
        public static void LoadAllLanguages()
        {
            LoadStandardLanguages();
            LoadAllLanguageKits();
        }

        public static void LoadStandardLanguages()
        {
            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);

            DirectoryInfo directory = new DirectoryInfo(directoryName);

            FileInfo[] dllfiles = directory.GetFiles("*Language.dll");

            foreach (FileInfo languageFile in dllfiles)
            {
                if (Path.GetFileName(languageFile.FullName) == "VBNETParser.dll" || Path.GetFileName(languageFile.FullName) == "PascalABCPartParser.dll")
                    continue;

                IntegrateLanguageFromAssembly(languageFile);
            }
        }

        /// <summary>
        /// Загружает все парсеры из языковых комплектов
        /// </summary>
        /*public static void LoadParsersFromLanguageKits()
        {
            DirectoryInfo languageKitsDirectory = GetLanguageKitsDirectory();

            if (languageKitsDirectory == null)
                return;

            foreach (var parserDll in languageKitsDirectory.GetDirectories()
                .SelectMany(directory => directory.GetFiles("*Parser.dll")))
            {
                if (parserDll != null)
                    ParsersController.IntegrateParsersFromAssembly(parserDll);
            }
        }*/

        public static void IntegrateLanguageFromAssembly(FileInfo languageFile)
        {
            Assembly assembly = Assembly.LoadFile(languageFile.FullName);
            try
            {
                Type[] types = assembly.GetTypes();
                if (assembly != null)
                {
                    ILanguage languageFound = null;
                    foreach (Type type in types)
                    {
                        if (type.Name.IndexOf("Language") >= 0)
                        {
                            object obj = Activator.CreateInstance(type);
                            if (obj is ILanguage language)
                            {
                                languageFound = language;
                                LanguageProvider.Instance.Languages.Add(language);
                            }
                        }
                    }

                    if (languageFound == null)
                    {
                        throw new Exception("Language class wasn't found in language assembly. (To be found it should contain 'Language' in it's name)");
                    }

                    LanguageConnected?.Invoke(languageFound);
                }
            }
            catch (ReflectionTypeLoadException e)
            {
#if DEBUG
                string errorMessage = e + Environment.NewLine;
                errorMessage += "<Loader exceptions>:" + Environment.NewLine;
                errorMessage += string.Join(Environment.NewLine, e.LoaderExceptions.Select(error => error.ToString())) + Environment.NewLine;
                File.AppendAllText("log.txt", errorMessage);
#endif
                string errorLocalized = string.Format(ParserErrorsStringResources.Get("LOAD_ERROR{0}"), Path.GetFileName(languageFile.FullName));
                LanguageLoadErrorOccured?.Invoke(errorLocalized + Environment.NewLine);
            }
            catch (Exception e)
            {
#if DEBUG
                string errorMessage = e + Environment.NewLine;
                File.AppendAllText("log.txt", errorMessage);
#endif
                string errorLocalized = string.Format(ParserErrorsStringResources.Get("LOAD_ERROR{0}"), Path.GetFileName(languageFile.FullName));
                LanguageLoadErrorOccured?.Invoke(errorLocalized + Environment.NewLine);
            }
        }

    }
}
