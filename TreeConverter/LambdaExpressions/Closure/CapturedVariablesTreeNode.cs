using System;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;

namespace TreeConverter.LambdaExpressions.Closure
{
    internal abstract class CapturedVariablesTreeNode
    {
        public class CapturedSymbolInfo
        {
            public List<CapturedVariablesTreeNodeLambdaScope> ReferencingLambdas
            {
                get; 
                private set;
            }

            public SymbolInfo SymbolInfo
            {
                get;
                private set;
            }

            public syntax_tree_node SyntaxTreeNodeWithVarDeclaration
            {
                get;
                private set;
            }

            public CapturedSymbolInfo(syntax_tree_node syntaxTreeNodeWithVarDeclaration, SymbolInfo symbolInfo)
            {
                ReferencingLambdas = new List<CapturedVariablesTreeNodeLambdaScope>();
                SyntaxTreeNodeWithVarDeclaration = syntaxTreeNodeWithVarDeclaration;
                SymbolInfo = symbolInfo;
            }
        }

        public List<CapturedVariablesTreeNode> ChildNodes
        {
            get; 
            private set;
        }

        public CapturedVariablesTreeNode ParentNode
        {
            get; 
            private set;
        }

        public List<CapturedSymbolInfo> VariablesDefinedInScope
        {
            get; 
            private set;
        }

        public int ScopeIndex
        {
            get;
            private set;
        }

        public syntax_tree_node CorrespondingSyntaxTreeNode
        {
            get;
            private set;
        }

        public List<CapturedVariablesTreeNodeLambdaScope> LambdasDefinedInScope
        {
            get;
            private set;
        }

        protected CapturedVariablesTreeNode(CapturedVariablesTreeNode parentNode, int scopeNum, syntax_tree_node correspondingSyntaxTreeNode)
        {
            ParentNode = parentNode;
            ChildNodes = new List<CapturedVariablesTreeNode>();
            VariablesDefinedInScope = new List<CapturedSymbolInfo>();
            ScopeIndex = scopeNum;
            CorrespondingSyntaxTreeNode = correspondingSyntaxTreeNode;
            LambdasDefinedInScope = new List<CapturedVariablesTreeNodeLambdaScope>();
        }
    }

    internal class CapturedVariablesTreeNodeBlockScope : CapturedVariablesTreeNode
    {
        /*public List<function_lambda_definition> LambdasDefinedInScope
        {
            get; 
            private set;
        }*/

        public CapturedVariablesTreeNodeBlockScope(CapturedVariablesTreeNode parentNode, int scopeNum, syntax_tree_node correspondingSyntaxTreeNode)
            : base(parentNode, scopeNum, correspondingSyntaxTreeNode)
        {
            //LambdasDefinedInScope = new List<function_lambda_definition>();
        }
    }

    internal class CapturedVariablesTreeNodeProcedureScope : CapturedVariablesTreeNode
    {
        public common_function_node FunctionNode
        {
            get;
            private set;
        }
        public CapturedVariablesTreeNodeProcedureScope(CapturedVariablesTreeNode parentNode, common_function_node function, int scopeNum, syntax_tree_node correspondingSyntaxTreeNode)
            : base(parentNode, scopeNum, correspondingSyntaxTreeNode)
        {
            FunctionNode = function;
        }
    }

    internal class CapturedVariablesTreeNodeClassScope : CapturedVariablesTreeNode
    {
        public string ClassName
        {
            get;
            private set;
        }

        public CapturedVariablesTreeNodeClassScope(CapturedVariablesTreeNode parentNode, int scopeNum, syntax_tree_node correspondingSyntaxTreeNode, string className)
            : base(parentNode, scopeNum, correspondingSyntaxTreeNode)
        {
            NonPublicMembersNamesMapping = new Dictionary<string, Tuple<string, class_field, semantic_node>>();
            ClassName = className;
        }

        public Dictionary<string, Tuple<string, class_field, semantic_node>> NonPublicMembersNamesMapping { get; set; }
    }

    internal class CapturedVariablesTreeNodeForScope : CapturedVariablesTreeNode
    {
        public CapturedVariablesTreeNodeBlockScope EnclosedUpperBlockScope
        {
            get; 
            private set;
        }

        public SymbolInfo SymbolInfoLoopVar
        {
            get;
            set;
        }

        public CapturedVariablesTreeNodeForScope(CapturedVariablesTreeNode parentNode, int scopeNum, syntax_tree_node correspondingSyntaxTreeNode)
            : base(parentNode, scopeNum, correspondingSyntaxTreeNode)
        {
            var blockScope = parentNode;
            while (!(blockScope is CapturedVariablesTreeNodeBlockScope))
            {
                blockScope = blockScope.ParentNode;
            }

            EnclosedUpperBlockScope = (CapturedVariablesTreeNodeBlockScope)blockScope;
        }
    }

    internal class CapturedVariablesTreeNodeForEachScope : CapturedVariablesTreeNode
    {
        public SymbolInfo SymbolInfoLoopVar
        {
            get;
            set;
        }

        public CapturedVariablesTreeNodeForEachScope(CapturedVariablesTreeNode parentNode, int scopeNum, syntax_tree_node correspondingSyntaxTreeNode)
            : base(parentNode, scopeNum, correspondingSyntaxTreeNode)
        {
            
        }
    }

    internal class CapturedVariablesTreeNodeLambdaScope : CapturedVariablesTreeNode
    {
        public List<SymbolInfo> CapturedVarsSymbolInfo
        {
            get;
            private set;
        }

        public function_lambda_definition LambdaDefinition
        {
            get;
            private set;
        }

        public int? ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod
        {
            get;
            set;
        }

        public CapturedVariablesTreeNodeLambdaScope(CapturedVariablesTreeNode parentNode, function_lambda_definition lambdaDef, int scopeNum, syntax_tree_node correspondingSyntaxTreeNode)
            : base(parentNode, scopeNum, correspondingSyntaxTreeNode)
        {
            LambdaDefinition = lambdaDef;
            CapturedVarsSymbolInfo = new List<SymbolInfo>();
            ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod = null;
        }
    }
}