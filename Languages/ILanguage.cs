using System.Collections.Generic;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;

namespace Languages
{
    public interface ILanguage
    {
        string Name { get; }

        BaseParser Parser { get; }

        IParser DocParser { get; }

        List<IVisitor> SyntaxTreePostProcessors { get; set; }

        string[] FilesExtensions { get; }

        bool CaseSensitive { get; }

        string[] SystemUnitNames { get; }
    }
}
