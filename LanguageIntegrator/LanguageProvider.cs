// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.IO;
using PascalABCCompiler.Errors;

namespace Languages.Facade
{
    
    /// <summary>
    /// Хранилище языков, поддерживаемых платформой (Singletone)
    /// </summary>
    public class LanguageProvider
    {
        private static readonly Lazy<LanguageProvider> lazyInstanceWrapper = new Lazy<LanguageProvider>(() => new LanguageProvider(), true);

        public static LanguageProvider Instance { get { return lazyInstanceWrapper.Value; } }

        /// <summary>
        /// Список всех языков, заполняется классом LanguageIntegrator
        /// </summary>
        public List<ILanguage> Languages { get; private set; } = new List<ILanguage>();


        /// <summary>
        /// Основной язык платформы - PascalABC.NET
        /// </summary>
        public ILanguage MainLanguage
        { 
            get
            {
                return Languages[0];
            }
        }

        /// <summary>
        /// Выбор языка по расширению файла - бросает ошибку некорректного расширения
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns></returns>
        /// <exception cref="ParserBadFileExtension"></exception>
        public ILanguage SelectLanguageByExtension(string fileName)
        {
            return SelectLanguageByExtensionSafe(fileName) ?? throw new ParserBadFileExtension(fileName);
        }


        /// <summary>
        /// Выбор языка по расширению файла - возвращает null, если расширение не то
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ILanguage SelectLanguageByExtensionSafe(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();

            foreach (ILanguage language in Languages)
                foreach (string langExtension in language.FilesExtensions)
                    if (langExtension == extension)
                        return language;

            return null;
        }

        /// <summary>
        /// Выбор языка по имени (ILanguage.Name)
        /// </summary>
        /// <param name="languageName">Имя языка</param>
        /// <returns></returns>
        public ILanguage SelectLanguageByName(string languageName)
        {
            foreach (ILanguage language in Languages)
                if (language.Name == languageName)
                    return language;
            
            return null;
        }

    }
}
