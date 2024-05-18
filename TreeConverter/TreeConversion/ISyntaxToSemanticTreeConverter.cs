using PascalABCCompiler.SyntaxTree;
using System.Collections;
using System.Collections.Generic;

namespace PascalABCCompiler.TreeConverter.TreeConversion
{
    public interface ISyntaxToSemanticTreeConverter : IVisitor
    {
        Errors.SyntaxError ParserError { get; set; }

        Hashtable BadNodesInSyntaxTree { get; set; }

        TreeRealization.unit_node_list ReferencedUnits { get; set; }

        void InitializeForCompilingInterface(InitializationDataForCompilingInterface initializationData);

        void InitializeForCompilingImplementation(InitializationDataForCompilingImplementation initializationData);

        // требуется другая архитектура, чтобы этого не было здесь  EVA
        void ProcessNode<T>(T cd) where T : syntax_tree_node;

        List<TreeRealization.var_definition_node> CompiledVariables { get; }

        TreeRealization.common_unit_node CompiledUnit { get; }

        SymbolTable.TreeConverterSymbolTable SymbolTable { get; }

        void Reset();
    }
}
