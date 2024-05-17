using System.Collections.Generic;
using PascalABCCompiler.Errors;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.TreeConverter.TreeConversion;

namespace Languages.Facade
{
    public interface ILanguage
    {
        string Name { get; }

        string Version { get; }

        string Copyright { get; }

        IParser Parser { get; }

        IParser DocParser { get; }

        List<ISyntaxTreeConverter> SyntaxTreeConverters { get; }

        ISyntaxToSemanticTreeConverter SyntaxTreeToSemanticTreeConverter { get; }

        string[] FilesExtensions { get; }

        bool CaseSensitive { get; }

        string[] SystemUnitNames { get; }

        void SetSemanticRules();

        void RefreshSyntaxTreeToSemanticTreeConverter();

    }
}
