using System.Collections.Generic;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;

namespace Languages.Facade
{
    public interface ILanguage
    {
        string Name { get; }

        string Version { get; }

        string Copyright { get; }

        BaseParser Parser { get; }

        IParser DocParser { get; }

        List<IVisitor> SyntaxTreePostProcessors { get; set; }

        string[] FilesExtensions { get; }

        bool CaseSensitive { get; }

        string[] SystemUnitNames { get; }
    }
}
