using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PascalABCCompiler;
using PascalABCCompiler.Errors;
using PascalABCCompiler.Parsers;

namespace Languages
{
    public class LanguageProvider
    {
        private static readonly Lazy<LanguageProvider> lazyInstanceWrapper = new Lazy<LanguageProvider>(() => new LanguageProvider(), true);

        public static LanguageProvider Instance { get { return lazyInstanceWrapper.Value; } }

        public List<ILanguage> Languages { get; private set; } = new List<ILanguage>();


        public ILanguage SelectLanguageByExtension(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();

            foreach (ILanguage language in Languages)
                foreach (string langExtension in language.FilesExtensions)
                    if (langExtension == extension)
                        return language;

            throw new ParserBadFileExtension(fileName);
        }

        public ILanguage SelectLanguageByName(string languageName)
        {
            foreach (ILanguage language in Languages)
                if (language.Name == languageName)
                    return language;
            
            return null;
        }


        SourceFilesProviderDelegate sourceFilesProvider = SourceFilesProviders.DefaultSourceFilesProvider;

        public SourceFilesProviderDelegate SourceFilesProvider
        {
            get
            {
                return sourceFilesProvider;
            }
            set
            {
                sourceFilesProvider = value;
                foreach (Tuple<BaseParser, IParser> parsers in Languages.Select(language => Tuple.Create(language.Parser, language.DocParser)))
                {
                    parsers.Item1.SourceFilesProvider = value;
                    if (parsers.Item2 != null)
                        parsers.Item2.SourceFilesProvider = value;
                }
                
            }
        }

    }
}
