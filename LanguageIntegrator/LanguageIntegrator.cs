// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Languages.Facade;
using PascalABCCompiler;

namespace Languages.Integration
{
    /// <summary>
    /// Модуль с функциями загрузки поддерживаемых языков программирования
    /// </summary>
    public static class LanguageIntegrator
    {

        private static readonly LanguageProvider LanguageProvider = LanguageProvider.Instance;

        /// <summary>
        /// Имя директории, содержащей установленные плагины языков программирования
        /// </summary>
        private const string languageKitsDirectoryName = "LanguageKits";

        /// <summary>
        /// Событие, информирующее об успешной загрузке плагина языка
        /// </summary>
        public static event Action<ILanguage> LanguageLoaded;

        /// <summary>
        /// Событие, информирующее об ошибке загрузки плагина языка
        /// </summary>
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
        /// Загружает все языковые комплекты для использования (имя "главной" dll должно заканчиваться на Language)
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
                    if (dll.Name.EndsWith("LanguageInfo.dll"))
                    {
                        IntegrateLanguageFromAssembly(dll);
                        break;
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

        /// <summary>
        /// Загружает стандартные языки платформы из папки bin
        /// </summary>
        public static void LoadStandardLanguages()
        {
            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);

            DirectoryInfo directory = new DirectoryInfo(directoryName);

            FileInfo[] dllFiles = directory.GetFiles("*LanguageInfo.dll");

            var mainLanguageDll = dllFiles.First(fileInfo => fileInfo.Name == StringConstants.pascalLanguageDllName);

            // первым добавляем PascalABC.NET
            IntegrateLanguageFromAssembly(mainLanguageDll);

            foreach (FileInfo languageFile in dllFiles.Where(dll => dll != mainLanguageDll))
            {
                IntegrateLanguageFromAssembly(languageFile);
            }
        }

        /// <summary>
        /// Загружает плагин языка из dll, содержащей класс, представляющий язык 
        /// (класс должен реализовывать интерфейс ILanguage и его имя должно заканчиваться на Language)
        /// </summary>
        /// <param name="languageFile">dll, содержащая язык</param>
        private static void IntegrateLanguageFromAssembly(FileInfo languageFile)
        {
            try
            {
                ILanguage languageFound = InstantiateLanguageAndAddToLanguagesList(languageFile);

                if (languageFound == null)
                {
                    throw new Exception("Language class wasn't found in language assembly. (To be found it should contain 'Language' in it's name)");
                }

                LanguageLoaded?.Invoke(languageFound);
            }
            catch (ReflectionTypeLoadException e)
            {
#if DEBUG
                string errorMessage = e + Environment.NewLine;
                errorMessage += "<Loader exceptions>:" + Environment.NewLine;
                errorMessage += string.Join(Environment.NewLine, e.LoaderExceptions.Select(error => error.ToString())) + Environment.NewLine;
                File.AppendAllText("log.txt", errorMessage);
#endif
                LanguageLoadErrorOccured?.Invoke(languageFile.Name);
            }
            catch (Exception e)
            {
#if DEBUG
                string errorMessage = e + Environment.NewLine;
                File.AppendAllText("log.txt", errorMessage);
#endif
                LanguageLoadErrorOccured?.Invoke(languageFile.Name);
            }
        }

        /// <summary>
        /// Конструирует объект языка и добавляет его в список языков в LanguageProvider
        /// </summary>
        /// <param name="languageFile"></param>
        /// <returns></returns>
        private static ILanguage InstantiateLanguageAndAddToLanguagesList(FileInfo languageFile)
        {
            Assembly assembly = Assembly.LoadFile(languageFile.FullName);

            Type[] types = assembly.GetTypes();

            ILanguage languageFound = null;
            foreach (Type type in types)
            {
                if (type.Name.EndsWith("Language") && type.IsClass)
                {
                    object obj = Activator.CreateInstance(type);
                    if (obj is ILanguage language)
                    {
                        languageFound = language;
                        LanguageProvider.Languages.Add(language);
                    }
                    break;
                }
            }

            return languageFound;
        }
    }
}
