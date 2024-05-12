using System;
using System.Collections.Generic;
using PascalABCCompiler;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;

namespace Languages.Pascal
{
    public class PascalLanguage : ILanguage
    {
        private readonly BaseParser parser = new PascalABCNewParser.PascalABCNewLanguageParser();
        
        private readonly IParser docParser = new DocTagsParser.PascalDocTagsLanguageParser();

        private readonly string[] filesExtensions = new string[] { StringConstants.pascalSourceFileExtension };


        public string Name => StringConstants.pascalLanguageName;

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
