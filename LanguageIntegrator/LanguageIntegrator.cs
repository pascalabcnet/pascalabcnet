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
        /// Событие, информирующее об успешной загрузке языкового пакета
        /// </summary>
        public static event Action<ILanguage> LanguageLoaded;

        /// <summary>
        /// Событие, информирующее об ошибке загрузки языкового пакета
        /// </summary>
        public static event Action<string> LanguageLoadErrorOccured;

        /// <summary>
        /// Загружает все языки платформы из папки bin
        /// </summary>
        public static void LoadAllLanguages()
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
            // Assembly assembly = Assembly.LoadFile(languageFile.FullName);
            // SSM 02.01.2026 Поменял для net 9.0 иначе падение - не загружался PascalABCParser
            Assembly assembly = Assembly.LoadFrom(languageFile.FullName);

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
