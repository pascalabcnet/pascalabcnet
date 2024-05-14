using System.Collections.Generic;
using Languages.Facade;
using PascalABCCompiler;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;

namespace Languages.Pascal
{
    public class PascalLanguage : ILanguage
    {
        private readonly IParser parser = new Frontend.Wrapping.PascalABCNewLanguageParser();
        
        private readonly IParser docParser = new Frontend.Documentation.PascalDocTagsLanguageParser();

        private readonly string[] filesExtensions = new string[] { StringConstants.pascalSourceFileExtension };

        private readonly List<ISyntaxTreeConverter> syntaxTreeConverters = new List<ISyntaxTreeConverter>() { new Frontend.Converters.StandardSyntaxTreeConverter(), new Frontend.Converters.LambdaAnyConverter() };

        public string Name => StringConstants.pascalLanguageName;

        public string Version => "1.2";

        public string Copyright => "Copyright © 2005-2024 by Ivan Bondarev, Stanislav Mikhalkovich";

        public IParser Parser => parser;

        public IParser DocParser => docParser;

        public List<IVisitor> SyntaxTreePostProcessors { get; set; }

        public List<ISyntaxTreeConverter> SyntaxTreeConverters => syntaxTreeConverters;

        public string[] FilesExtensions
        {
            get
            {
                return filesExtensions;
            }
        }

        public bool CaseSensitive => false;

        public string[] SystemUnitNames => StringConstants.pascalDefaultStandardModules;

        
    }
}
