using System;
using System.Collections.Generic;
using PascalABCCompiler;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;

namespace Languages.Pascal
{
    internal class PascalLanguage : ILanguage
    {
        public string Name => StringConstants.pascalLanguageName;

        public BaseParser Parser { get; set; }

        public IParser DocParser { get; set; }

        public List<IVisitor> SyntaxTreePostProcessors { get; set; }

        public Dictionary<Type, int> PostProcessorsOrder { get; }
        
        private readonly string[] filesExtensions = new string[] { StringConstants.pascalSourceFileExtension };

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
