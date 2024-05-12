using System.Collections.Generic;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;

namespace Languages
{
    public interface ILanguage
    {
        string Name { get; }

        BaseParser Parser { get; set; }

        IParser DocParser { get; set; }

        List<IVisitor> SyntaxTreePostProcessors { get; set; }

        Dictionary<System.Type, int> PostProcessorsOrder { get; }

        string[] FilesExtensions { get; }

        bool CaseSensitive { get; }

        string[] SystemUnitNames { get; }
    }
}
