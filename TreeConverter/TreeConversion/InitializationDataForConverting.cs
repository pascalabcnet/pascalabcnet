using System.Collections.Generic;
using System.Collections;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.TreeConverter.TreeConversion
{
    /// <summary>
    /// Вспомогательная структура данных для инициализации ISyntaxToSemanticTreeConverter перед компиляцией интерфейса
    /// </summary>
    public class InitializationDataForCompilingInterface
    {
        public readonly Errors.SyntaxError parserError;

        public readonly Hashtable badNodes;

        public readonly TreeRealization.unit_node_list usedUnits;

        public readonly TreeRealization.using_namespace_list interfaceNamespaces;

        public readonly compilation_unit syntaxUnit;

        public readonly List<Errors.Error> errorsList;
        
        public readonly List<Errors.CompilerWarning> warningsList;

        public readonly Dictionary<syntax_tree_node, string> docs;
        
        public readonly bool debug;
        
        public readonly bool debugging;
        
        public readonly bool forIntellisense;

        public InitializationDataForCompilingInterface(
            
            Errors.SyntaxError parserError, 
            Hashtable badNodes, 
            TreeRealization.unit_node_list usedUnits, 
            TreeRealization.using_namespace_list interfaceNamespaces, 
            compilation_unit syntaxUnit, 
            List<Errors.Error> errorsList, 
            List<Errors.CompilerWarning> warningsList, 
            Dictionary<syntax_tree_node, string> docs, 
            bool debug, 
            bool debugging, 
            bool forIntellisense
            
            )
        {
            this.parserError = parserError;
            this.badNodes = badNodes;
            this.usedUnits = usedUnits;
            this.interfaceNamespaces = interfaceNamespaces;
            this.syntaxUnit = syntaxUnit;
            this.errorsList = errorsList;
            this.warningsList = warningsList;
            this.docs = docs;
            this.debug = debug;
            this.debugging = debugging;
            this.forIntellisense = forIntellisense;
        }
    }

    /// <summary>
    /// Вспомогательная структура данных для инициализации ISyntaxToSemanticTreeConverter перед компиляцией реализации
    /// </summary>
    public class InitializationDataForCompilingImplementation : InitializationDataForCompilingInterface
    {
        public readonly TreeRealization.using_namespace_list implementationNamespaces;

        public readonly TreeRealization.common_unit_node semanticUnit;

        public InitializationDataForCompilingImplementation(
            
            Errors.SyntaxError parserError, 
            Hashtable badNodes, 
            TreeRealization.unit_node_list usedUnits, 
            TreeRealization.using_namespace_list interfaceNamespaces, 
            TreeRealization.using_namespace_list implementationNamespaces,
            compilation_unit syntaxUnit, 
            TreeRealization.common_unit_node semanticUnit,
            List<Errors.Error> errorsList, 
            List<Errors.CompilerWarning> warningsList, 
            Dictionary<syntax_tree_node, string> docs, 
            bool debug, 
            bool debugging, 
            bool forIntellisense
            
            ) : base(parserError, badNodes, usedUnits, interfaceNamespaces, syntaxUnit, errorsList, warningsList, docs, debug, debugging, forIntellisense)
        {
            this.implementationNamespaces = implementationNamespaces;
            this.semanticUnit = semanticUnit;
        }
    }
}
