using PascalABCCompiler.SyntaxTree;
using System.Collections;
using System.Collections.Generic;

namespace PascalABCCompiler.TreeConverter.TreeConversion
{
    public interface ISyntaxToSemanticTreeConverter : IVisitor
    {
        string[] FilesExtensions { get; }

        Errors.SyntaxError ParserError { get; set; }

        Hashtable BadNodesInSyntaxTree { get; set; }

        TreeRealization.unit_node_list ReferencedUnits { get; set; }

        void InitializeForCompilingInterface(Errors.SyntaxError parser_error,
            Hashtable bad_nodes, TreeRealization.unit_node_list usedUnits,
            TreeRealization.using_namespace_list namespaces, compilation_unit syntaxUnit,
            List<Errors.Error> errorsList, List<Errors.CompilerWarning> warningsList,
            Dictionary<syntax_tree_node, string> docs, bool debug, bool debugging, bool for_intellisense);

        void InitializeForCompilingImplementation(Errors.SyntaxError parser_error,
            Hashtable bad_nodes, TreeRealization.unit_node_list usedUnits,
            TreeRealization.using_namespace_list namespaces, TreeRealization.using_namespace_list implementationNamespaces, compilation_unit syntaxUnit, TreeRealization.common_unit_node SemanticUnit,
            List<Errors.Error> errorsList, List<Errors.CompilerWarning> warningsList,
            Dictionary<syntax_tree_node, string> docs, bool debug, bool debugging, bool for_intellisense);

        void ProcessNode<T>(T cd) where T : syntax_tree_node;

        List<TreeRealization.var_definition_node> CompiledVariables { get; }

        TreeRealization.common_unit_node CompiledUnit { get; }

        SymbolTable.TreeConverterSymbolTable SymbolTable { get; }

        void Reset();
    }
}
