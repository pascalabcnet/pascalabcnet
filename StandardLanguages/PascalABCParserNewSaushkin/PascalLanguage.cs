using System.Collections.Generic;
using PascalABCCompiler;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;

namespace Languages.Pascal
{
    public class PascalLanguage : ILanguage
    {
        private readonly BaseParser parser = new Frontend.Wrapping.PascalABCNewLanguageParser();
        
        private readonly IParser docParser = new Frontend.Documentation.PascalDocTagsLanguageParser();

        private readonly string[] filesExtensions = new string[] { StringConstants.pascalSourceFileExtension };


        public string Name => StringConstants.pascalLanguageName;

        public string Version => "1.2";

        public string Copyright => "Copyright © 2005-2024 by Ivan Bondarev, Stanislav Mikhalkovich";

        public BaseParser Parser => parser;

        public IParser DocParser => docParser;

        public List<IVisitor> SyntaxTreePostProcessors { get; set; }

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
