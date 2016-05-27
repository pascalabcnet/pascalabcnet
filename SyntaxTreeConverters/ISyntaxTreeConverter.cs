// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.SyntaxTreeConverters
{
    public enum ConverterType
    {
        Conversion, Analysis
    }
    public enum ExecutionOrder
    {
        Undefined, First, Last
    }
    public interface ISyntaxTreeConverter
    {
        string Name { get; }
        
        string Version { get; }

        string Description { get; }

        string Copyright { get; }

        ConverterType ConverterType { get; }

        ExecutionOrder ExecutionOrder { get; }

        syntax_tree_node Convert(syntax_tree_node root);

    }
}
