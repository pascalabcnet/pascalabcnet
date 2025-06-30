// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using PascalABCCompiler.SemanticTree;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeRealization;
using static PascalABCCompiler.StringConstants;

namespace TreeConverter.LambdaExpressions.Closure
{
    internal class CapturedVariablesSubstitutionClassGenerator
    {
        private readonly CapturedVariablesTreeNode _capturedVariablesRootTreeNode;
        private readonly Dictionary<int, ScopeClassDefinition> _capturedVarsClassDefs;
        private readonly List<CapturedVariablesTreeNodeLambdaScope> _lambdasToBeAddedAsMethods;
        private const string GeneratedClassPrefix = "<>local_variables_class_";
        private const string GeneratedUpperClassPrefix = "<>local_variables_class_UPPER_";
        private static int _generatedClassCounter; 
        private static int _generatedUpperClassCounter;
        private readonly Dictionary<SubstitutionKey, dot_node> _substitutions;
        private readonly List<LambdaReferencesSubstitutionInfo> _lambdaIdReferences = new List<LambdaReferencesSubstitutionInfo>(); 
        private readonly List<RewriteReferencesForNodesThatAreChildNodesToThoseThatContainCapturedVariableInfo> _rewriteReferencesForNodesThatAreChildNodesToThoseThatContainCapturedVariableInfo = new List<RewriteReferencesForNodesThatAreChildNodesToThoseThatContainCapturedVariableInfo>(); 
        public static void Reset()
        {
            _generatedClassCounter = 0;
            _generatedUpperClassCounter = 0;
        }

        public class VariableSubstitutionsInfo
        {
            public Dictionary<int, ScopeClassDefinition> GeneratedScopeClassesInfo
            {
                get;
                set;
            }

            public Dictionary<SubstitutionKey, dot_node> SubstitutionsInfo
            {
                get;
                set;
            }

            public List<CapturedVariablesTreeNodeLambdaScope> LambdasToBeAddedAsMethods
            {
                get;
                set;
            }

            public Dictionary<string, Tuple<string, class_field, semantic_node>> ConvertingClassNonPublicMembersMapping
            {
                get; set;
            }
        }

        public class ScopeClassDefinition
        {
            private const string GeneratedSubstitutingFieldPrefix = "<>local_variables_"; //это поле для того, чтобы заменять в текущем блоке, где захватывались переменные, левую часть dot_node
            private static int _generatedSubstitutingFieldCounter;

            public List<CapturedVariablesTreeNodeLambdaScope> NestedLambdas { get; set; }

            public syntax_tree_node CorrespondingSyntaxTreeNode
            {
                get;
                private set;
            }

            public type_declaration ClassDeclaration
            {
                get;
                private set;
            }
            
            private string _generatedSubstitutingFieldName;
            public string GeneratedSubstitutingFieldName
            {
                get {
                    return _generatedSubstitutingFieldName ??
                           (_generatedSubstitutingFieldName =
                            GeneratedSubstitutingFieldPrefix + _generatedSubstitutingFieldCounter++);
                }
            }

            private string _generatedUpperClassFieldName;
            public string GeneratedUpperClassFieldName
            {
                get
                {
                    return _generatedUpperClassFieldName ??
                           (_generatedUpperClassFieldName =
                            GeneratedUpperClassPrefix + _generatedUpperClassCounter++);
                }
            }

            private var_statement _generatedVarStatementForScope;
            public var_statement GeneratedVarStatementForScope
            {
                get
                {
                    if (_generatedVarStatementForScope == null)
                    {
                        var newVarDefSyntaxTreeNode =
                            SyntaxTreeBuilder.BuildSimpleVarDef(GeneratedSubstitutingFieldName, ClassDeclaration.type_name.name);

                        newVarDefSyntaxTreeNode.inital_value =
                            new new_expr(SyntaxTreeBuilder.BuildSimpleType(ClassDeclaration.type_name.name),
                                         new expression_list(), null);

                        _generatedVarStatementForScope = new var_statement(newVarDefSyntaxTreeNode);
                    }

                    return _generatedVarStatementForScope;
                }
            }

            public assign AssignNodeForUpperClassFieldInitialization
            {
                get; 
                set;
            }

            public CapturedVariablesTreeNode CorrespondingTreeNode { get; private set; }

            public ScopeClassDefinition(syntax_tree_node syntaxTreeNode,
                                        type_declaration classDeclaration,
                                        CapturedVariablesTreeNode correspondingTreeNode,
                                        string generatedSubstitutingFieldName = null)
            {
                CorrespondingSyntaxTreeNode = syntaxTreeNode;
                ClassDeclaration = classDeclaration;
                _generatedSubstitutingFieldName = generatedSubstitutingFieldName;
                CorrespondingTreeNode = correspondingTreeNode;
                NestedLambdas = new List<CapturedVariablesTreeNodeLambdaScope>();
            }
        }

        private class LambdaReferencesSubstitutionInfo
        {
            public CapturedVariablesTreeNodeLambdaScope LambdaScope { get; set; }
            public string VarName { get; set; }
            public syntax_tree_node SyntaxTreeNodeWithVarDeclaration { get; set; }
            public dot_node DotNode { get; set; }
        }

        private class RewriteReferencesForNodesThatAreChildNodesToThoseThatContainCapturedVariableInfo
        {
            public CapturedVariablesTreeNode TreeNode { get; set; }
            public string Varname { get; set; }
            public syntax_tree_node NodeWithVarDecl { get; set; }
        }

        public CapturedVariablesSubstitutionClassGenerator(CapturedVariablesTreeNode capturedVariablesRootTreeNode)
        {
            _capturedVariablesRootTreeNode = capturedVariablesRootTreeNode;
            _capturedVarsClassDefs = new Dictionary<int, ScopeClassDefinition>();
            _substitutions = new Dictionary<SubstitutionKey, dot_node>();
            _lambdasToBeAddedAsMethods = new List<CapturedVariablesTreeNodeLambdaScope>();
        }

        private string GeneratedClassName
        {
            get
            {
                return GeneratedClassPrefix + _generatedClassCounter++;
            }
        }

        private void VisitCapturedVar(CapturedVariablesTreeNode scope, CapturedVariablesTreeNode.CapturedSymbolInfo symbolInfo)
        {
            var varName = ((IVAriableDefinitionNode)symbolInfo.SymbolInfo.sym_info).name; //.ToLower();

            var ff = symbolInfo.SymbolInfo.sym_info.GetType();
            var isSelfWordInClass = scope is CapturedVariablesTreeNodeClassScope && varName.Equals(self_word, symbolInfo.SymbolInfo.scope.StringComparison);
            SourceContext sourceCtxt = null;
            if (symbolInfo.SymbolInfo.sym_info.location != null)
                sourceCtxt = new SourceContext(symbolInfo.SymbolInfo.sym_info.location.begin_line_num, symbolInfo.SymbolInfo.sym_info.location.begin_column_num,
                    symbolInfo.SymbolInfo.sym_info.location.end_line_num, symbolInfo.SymbolInfo.sym_info.location.end_column_num);
            foreach (var referencingLambda in symbolInfo.ReferencingLambdas.OrderByDescending(rl => rl.ScopeIndex))
            {
                if (scope != referencingLambda.ParentNode)
                {
                    var upperScopesStack = new Stack<CapturedVariablesTreeNode>();
                    var crawlUpScope = referencingLambda.ParentNode;
                    var anotherLambdaIsOnTheWay = crawlUpScope is CapturedVariablesTreeNodeLambdaScope;

                    while (crawlUpScope != null && crawlUpScope != scope && !anotherLambdaIsOnTheWay)
                    {
                        upperScopesStack.Push(crawlUpScope);
                        crawlUpScope = crawlUpScope.ParentNode;
                        anotherLambdaIsOnTheWay = crawlUpScope is CapturedVariablesTreeNodeLambdaScope;
                    }

                    if (anotherLambdaIsOnTheWay || crawlUpScope == null)
                    {
                        continue;
                    }

                    var upperScopeWhereVarsAreCaptured = scope;
                    var upperScopeWhereVarsAreCapturedClass =
                        _capturedVarsClassDefs[upperScopeWhereVarsAreCaptured.ScopeIndex].ClassDeclaration;
                    var ClassName = upperScopeWhereVarsAreCapturedClass.type_name;
                    var ClassField = symbolInfo.SymbolInfo.sym_info as class_field;

                    var substKey = new SubstitutionKey(varName, symbolInfo.SyntaxTreeNodeWithVarDeclaration,
                                                       scope.CorrespondingSyntaxTreeNode);
                    if (!_substitutions.ContainsKey(substKey))
                    {
                        // SSM 22.10.17 Тут ошибка в случае захвата классовых полей - первый параметр должен быть не self, а имя класса
                        if (ClassField != null && ClassField.IsStatic)
                            _substitutions.Add(substKey, new dot_node(ClassName, new ident(varName)));// sc не заполнен, что плохо!
                        else
                        _substitutions.Add(substKey,
                                           new dot_node(
                                               new ident(
                                                   _capturedVarsClassDefs[upperScopeWhereVarsAreCaptured.ScopeIndex]
                                                       .GeneratedSubstitutingFieldName, sourceCtxt), new ident(varName, sourceCtxt), sourceCtxt));
                    }

                    while (upperScopesStack.Count != 0)
                    {
                        var foundScopeWhereVarsWereCaptured = false;
                        while (upperScopesStack.Count != 0 && !foundScopeWhereVarsWereCaptured)
                        {
                            if (
                                upperScopesStack.Peek()
                                                .VariablesDefinedInScope.Exists(var => var.ReferencingLambdas.Count > 0))
                            {
                                foundScopeWhereVarsWereCaptured = true;
                            }
                            else
                            {
                                var curScope = upperScopesStack.Pop();

                                if (upperScopeWhereVarsAreCaptured == scope &&
                                    upperScopeWhereVarsAreCaptured is CapturedVariablesTreeNodeClassScope)
                                {
                                    continue;
                                }

                                substKey = new SubstitutionKey(varName, symbolInfo.SyntaxTreeNodeWithVarDeclaration,
                                                               curScope.CorrespondingSyntaxTreeNode);

                                dot_node dotnode;

                                if (upperScopeWhereVarsAreCaptured != scope)
                                {
                                    dotnode =
                                        new dot_node(
                                            new ident(
                                                _capturedVarsClassDefs[upperScopeWhereVarsAreCaptured.ScopeIndex]
                                                    .GeneratedSubstitutingFieldName, sourceCtxt),
                                            new ident(
                                                _capturedVarsClassDefs[upperScopeWhereVarsAreCaptured.ScopeIndex]
                                                    .GeneratedUpperClassFieldName, sourceCtxt), sourceCtxt);

                                    var nodeForDotNodeCalc = upperScopeWhereVarsAreCaptured.ParentNode;
                                    while (nodeForDotNodeCalc != scope)
                                    {
                                        if (_capturedVarsClassDefs.ContainsKey(nodeForDotNodeCalc.ScopeIndex) &&
                                            _capturedVarsClassDefs[nodeForDotNodeCalc.ScopeIndex]
                                                .AssignNodeForUpperClassFieldInitialization != null)
                                        {
                                            dotnode = new dot_node(dotnode,
                                                                   new ident(
                                                                       _capturedVarsClassDefs[
                                                                           nodeForDotNodeCalc.ScopeIndex]
                                                                           .GeneratedUpperClassFieldName));
                                        }

                                        nodeForDotNodeCalc = nodeForDotNodeCalc.ParentNode;
                                    }
                                    if (!isSelfWordInClass)
                                    {
                                        dotnode = new dot_node(dotnode, new ident(varName, sourceCtxt), sourceCtxt);
                                    }
                                }
                                else
                                {
                                    dotnode = new dot_node(new ident(
                                                               _capturedVarsClassDefs[
                                                                   upperScopeWhereVarsAreCaptured.ScopeIndex]
                                                                   .GeneratedSubstitutingFieldName, sourceCtxt),
                                                           new ident(varName, sourceCtxt), sourceCtxt);
                                }

                                if (!_substitutions.ContainsKey(substKey))
                                {
                                    _substitutions.Add(substKey, dotnode);
                                }
                            }
                        }

                        if (foundScopeWhereVarsWereCaptured)
                        {
                            var nextNodeWhereVarsAreCaptured = upperScopesStack.Pop();
                            if (!_capturedVarsClassDefs.ContainsKey(nextNodeWhereVarsAreCaptured.ScopeIndex))
                            {
                                var classDef = SyntaxTreeBuilder.BuildClassDefinition();
                                var typeDeclaration = new type_declaration(GeneratedClassName, classDef);
                                _capturedVarsClassDefs.Add(nextNodeWhereVarsAreCaptured.ScopeIndex,
                                                           new ScopeClassDefinition(
                                                               nextNodeWhereVarsAreCaptured.CorrespondingSyntaxTreeNode,
                                                               typeDeclaration,
                                                               nextNodeWhereVarsAreCaptured));
                            }

                            var nextNodeWhereVarsAreCapturedClass =
                                (class_definition)
                                _capturedVarsClassDefs[nextNodeWhereVarsAreCaptured.ScopeIndex].ClassDeclaration
                                                                                               .type_def;

                            if (
                                _capturedVarsClassDefs[nextNodeWhereVarsAreCaptured.ScopeIndex]
                                    .AssignNodeForUpperClassFieldInitialization == null && !(ClassField != null && ClassField.IsStatic))
                            {
                                var fieldType =
                                    SyntaxTreeBuilder.BuildSimpleType(upperScopeWhereVarsAreCapturedClass.type_name.name);
                                var field =
                                    SyntaxTreeBuilder.BuildClassFieldsSection(
                                        new List<ident>
                                            {
                                                new ident(
                                            _capturedVarsClassDefs[nextNodeWhereVarsAreCaptured.ScopeIndex]
                                                .GeneratedUpperClassFieldName, sourceCtxt)
                                            },
                                        new List<type_definition> {fieldType});

                                nextNodeWhereVarsAreCapturedClass.body.Add(field);

                                _capturedVarsClassDefs[nextNodeWhereVarsAreCaptured.ScopeIndex]
                                    .AssignNodeForUpperClassFieldInitialization =
                                    new assign(
                                        new dot_node(
                                            new ident(
                                                _capturedVarsClassDefs[nextNodeWhereVarsAreCaptured.ScopeIndex]
                                                    .GeneratedSubstitutingFieldName, sourceCtxt),
                                            new ident(
                                                _capturedVarsClassDefs[nextNodeWhereVarsAreCaptured.ScopeIndex]
                                                    .GeneratedUpperClassFieldName), sourceCtxt),
                                        new ident(
                                            _capturedVarsClassDefs[upperScopeWhereVarsAreCaptured.ScopeIndex]
                                                .GeneratedSubstitutingFieldName, sourceCtxt), 
                                                sourceCtxt);
                            }

                            substKey = new SubstitutionKey(varName, symbolInfo.SyntaxTreeNodeWithVarDeclaration,
                                                           nextNodeWhereVarsAreCaptured.CorrespondingSyntaxTreeNode);

                            var dot =
                                new dot_node(
                                    new ident(
                                        _capturedVarsClassDefs[nextNodeWhereVarsAreCaptured.ScopeIndex]
                                            .GeneratedSubstitutingFieldName),
                                    new ident(
                                        _capturedVarsClassDefs[nextNodeWhereVarsAreCaptured.ScopeIndex]
                                            .GeneratedUpperClassFieldName));

                            var nodeForDotNodeCalculation = nextNodeWhereVarsAreCaptured.ParentNode;
                            while (nodeForDotNodeCalculation != scope)
                            {
                                if (_capturedVarsClassDefs.ContainsKey(nodeForDotNodeCalculation.ScopeIndex) &&
                                    _capturedVarsClassDefs[nodeForDotNodeCalculation.ScopeIndex]
                                        .AssignNodeForUpperClassFieldInitialization != null)
                                {
                                    dot = new dot_node(dot,
                                                       new ident(
                                                           _capturedVarsClassDefs[nodeForDotNodeCalculation.ScopeIndex]
                                                               .GeneratedUpperClassFieldName, sourceCtxt), sourceCtxt);
                                }

                                nodeForDotNodeCalculation = nodeForDotNodeCalculation.ParentNode;
                            }

                            if (!isSelfWordInClass)
                            {
                                dot = new dot_node(dot, new ident(varName, sourceCtxt), sourceCtxt);
                            }

                            if (!_substitutions.ContainsKey(substKey))
                            {
                                if (ClassField != null && ClassField.IsStatic)
                                    _substitutions.Add(substKey, new dot_node(ClassName, new ident(varName, sourceCtxt), sourceCtxt));// sc не заполнен, что плохо!
                                else
                                    _substitutions.Add(substKey, dot);
                            }

                            upperScopeWhereVarsAreCaptured = nextNodeWhereVarsAreCaptured;
                            upperScopeWhereVarsAreCapturedClass =
                                _capturedVarsClassDefs[upperScopeWhereVarsAreCaptured.ScopeIndex].ClassDeclaration;
                        }
                    }

                    if (!(upperScopeWhereVarsAreCaptured == scope &&
                          upperScopeWhereVarsAreCaptured is CapturedVariablesTreeNodeClassScope))
                    {
                        if (upperScopeWhereVarsAreCaptured != scope)
                        {
                            dot_node dotnode1 = null;
                            if (ClassField != null && ClassField.IsStatic)
                                dotnode1 = new dot_node(ClassName, new ident(varName));
                            else
                                dotnode1 = new dot_node(
                                new ident(PascalABCCompiler.StringConstants.self_word, sourceCtxt),
                                new ident(
                                    _capturedVarsClassDefs[upperScopeWhereVarsAreCaptured.ScopeIndex]
                                        .GeneratedUpperClassFieldName, sourceCtxt), sourceCtxt);

                            if (upperScopeWhereVarsAreCaptured != scope)
                            {
                                var nodeForDotNodeCalc = upperScopeWhereVarsAreCaptured.ParentNode;
                                while (nodeForDotNodeCalc != scope)
                                {
                                    if (_capturedVarsClassDefs.ContainsKey(nodeForDotNodeCalc.ScopeIndex) &&
                                        _capturedVarsClassDefs[nodeForDotNodeCalc.ScopeIndex]
                                            .AssignNodeForUpperClassFieldInitialization != null)
                                    {
                                        dotnode1 = new dot_node(dotnode1,
                                                                new ident(
                                                                    _capturedVarsClassDefs[nodeForDotNodeCalc.ScopeIndex]
                                                                        .GeneratedUpperClassFieldName, sourceCtxt), sourceCtxt);
                                    }

                                    nodeForDotNodeCalc = nodeForDotNodeCalc.ParentNode;
                                }
                            }

                            if (!isSelfWordInClass)
                            {
                                var classScope = scope as CapturedVariablesTreeNodeClassScope;
                                if (classScope != null)
                                {
                                    Tuple<string, class_field, semantic_node> publicProperty;
                                    if (classScope.NonPublicMembersNamesMapping.TryGetValue(varName, out publicProperty))
                                    {
                                        dotnode1 = new dot_node(dotnode1.left, new ident(publicProperty.Item1, sourceCtxt), sourceCtxt); // SSM #869 добавил .left - была ошибка
                                    }
                                    else
                                    {
                                        if (!(ClassField != null && ClassField.IsStatic))
                                            dotnode1 = new dot_node(dotnode1, new ident(varName, sourceCtxt), sourceCtxt); // ?? dotnode1.left ??
                                    }
                                }
                                else
                                {
                                    dotnode1 = new dot_node(dotnode1, new ident(varName, sourceCtxt), sourceCtxt); // ?? dotnode1.left ??
                                }
                            }
                            _lambdaIdReferences.Add(new LambdaReferencesSubstitutionInfo
                                {
                                    LambdaScope = referencingLambda,
                                    VarName = varName,
                                    SyntaxTreeNodeWithVarDeclaration = symbolInfo.SyntaxTreeNodeWithVarDeclaration,
                                    DotNode = dotnode1
                                });
                        }
                        else
                        {
                            var dotnode1 = new dot_node(
                                new ident(PascalABCCompiler.StringConstants.self_word, sourceCtxt),
                                new ident(varName, sourceCtxt), sourceCtxt);

                            _lambdaIdReferences.Add(new LambdaReferencesSubstitutionInfo
                            {
                                LambdaScope = referencingLambda,
                                VarName = varName,
                                SyntaxTreeNodeWithVarDeclaration = symbolInfo.SyntaxTreeNodeWithVarDeclaration,
                                DotNode = dotnode1
                            });
                        }
                    }
                    if (!referencingLambda.ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod.HasValue ||
                        upperScopeWhereVarsAreCaptured.ScopeIndex >
                        referencingLambda.ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod)
                    {
                        referencingLambda.ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod =
                            upperScopeWhereVarsAreCaptured.ScopeIndex;
                    }
                }
                else
                {
                    if (!_capturedVarsClassDefs.ContainsKey(scope.ScopeIndex))
                    {
                        var classDef = SyntaxTreeBuilder.BuildClassDefinition();
                        var typeDeclaration = new type_declaration(GeneratedClassName, classDef);
                        _capturedVarsClassDefs.Add(scope.ScopeIndex,
                                                   new ScopeClassDefinition(
                                                       scope.CorrespondingSyntaxTreeNode,
                                                       typeDeclaration,
                                                       scope));
                    }

                    var substKey = new SubstitutionKey(varName, symbolInfo.SyntaxTreeNodeWithVarDeclaration,
                                                       scope.CorrespondingSyntaxTreeNode);
                    if (!_substitutions.ContainsKey(substKey))
                    {
                        if (!isSelfWordInClass)
                        {
                            string propertyName = null;

                            var classScope = scope as CapturedVariablesTreeNodeClassScope;
                            if (classScope != null)
                            {
                                Tuple<string, class_field, semantic_node> publicProperty;
                                if (classScope.NonPublicMembersNamesMapping.TryGetValue(varName, out publicProperty))
                                {
                                    propertyName = publicProperty.Item1;
                                }
                            }

                            _substitutions.Add(substKey,
                                               new dot_node(
                                                   new ident(
                                                       _capturedVarsClassDefs[scope.ScopeIndex]
                                                           .GeneratedSubstitutingFieldName, sourceCtxt), new ident(propertyName ?? varName, sourceCtxt), sourceCtxt));
                        }
                    }

                    var dotnode1 = new dot_node(
                                new ident(PascalABCCompiler.StringConstants.self_word, sourceCtxt),
                                new ident(varName, sourceCtxt), sourceCtxt);

                    _lambdaIdReferences.Add(new LambdaReferencesSubstitutionInfo
                    {
                        LambdaScope = referencingLambda,
                        VarName = varName,
                        SyntaxTreeNodeWithVarDeclaration = symbolInfo.SyntaxTreeNodeWithVarDeclaration,
                        DotNode = dotnode1
                    });

                    if (!referencingLambda.ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod.HasValue ||
                        scope.ScopeIndex > referencingLambda.ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod)
                    {
                        referencingLambda.ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod = scope.ScopeIndex;
                    }
                }

                if (!_lambdasToBeAddedAsMethods.Contains(referencingLambda))
                {
                    _lambdasToBeAddedAsMethods.Add(referencingLambda);
                }
            }
        }

        private void AddReferencesToIdentInLambda(type_declaration upperScopeWhereVarsAreCapturedClass, CapturedVariablesTreeNode scope, string varName, syntax_tree_node syntaxTreeNodeWithVarDeclaration, dot_node substDotNode, bool nestedLambda)
        {
/*#if DEBUG
            var pp = scope.ToString().IndexOf("TreeNode");
            var ss = scope.ToString().Remove(0, pp + 8).Replace("Scope"," ");
            var cn = "";
            if (scope.ChildNodes.Count>0)
                cn = "Childs: "+ scope.ChildNodes.Aggregate("",(s, x) => s + x.ScopeIndex.ToString() + " ");
            System.IO.File.AppendAllText("d:\\w.txt", "AddR enter: " + ss + scope.ScopeIndex + " " + cn + "" +scope.CorrespondingSyntaxTreeNode + "\n");
#endif*/
            for (var i = 0; i < scope.ChildNodes.Count; i++)
            {
                if (!(scope.ChildNodes[i] is CapturedVariablesTreeNodeLambdaScope))
                {
                    var substKey = new SubstitutionKey(varName, syntaxTreeNodeWithVarDeclaration,
                                                       scope.ChildNodes[i].CorrespondingSyntaxTreeNode);

                    if (_capturedVarsClassDefs.ContainsKey(scope.ChildNodes[i].ScopeIndex))
                    {
                        var cl = _capturedVarsClassDefs[scope.ChildNodes[i].ScopeIndex];
                        if (cl.AssignNodeForUpperClassFieldInitialization == null)
                        {
                            var fieldType =
                                    SyntaxTreeBuilder.BuildSimpleType(upperScopeWhereVarsAreCapturedClass.type_name.name);
                            var field =
                                SyntaxTreeBuilder.BuildClassFieldsSection(
                                    new List<ident>
                                            {
                                                new ident(cl.GeneratedUpperClassFieldName)
                                            },
                                    new List<type_definition> { fieldType });

                            var clClass = (class_definition) cl.ClassDeclaration.type_def;
                            clClass.body.Add(field);

                            cl.AssignNodeForUpperClassFieldInitialization =
                                new assign(
                                    new dot_node(new ident(cl.GeneratedSubstitutingFieldName),new ident(cl.GeneratedUpperClassFieldName)),
                                    new ident(PascalABCCompiler.StringConstants.self_word));

                        }
                    }

                    if (!_substitutions.ContainsKey(substKey))
                    {
/*#if DEBUG
                        System.IO.File.AppendAllText("d:\\w.txt", "1 substitutions.Add: " + substKey + " " + substDotNode + "\n");
#endif*/

                        _substitutions.Add(substKey, substDotNode);
                    }

                    AddReferencesToIdentInLambda(upperScopeWhereVarsAreCapturedClass, scope.ChildNodes[i], varName, syntaxTreeNodeWithVarDeclaration, substDotNode, nestedLambda);
                }
                else
                {
                    var scopeAsLambda = scope.ChildNodes[i] as CapturedVariablesTreeNodeLambdaScope;
                    if (scopeAsLambda.ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod.HasValue)
                    {
                        dot_node substDotNode1;
                        if (!nestedLambda)
                        {
                            var parts = new Stack<ident>();
                            var dn = substDotNode;
                            parts.Push((ident)dn.right);

                            while (!(dn.left is ident && dn.right is ident))
                            {
                                dn = (dot_node)dn.left;
                                parts.Push((ident)dn.right);
                            }

                            substDotNode1 = new dot_node(new ident(_capturedVarsClassDefs[scopeAsLambda.ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod.Value].GeneratedUpperClassFieldName), parts.Pop());
                            while (parts.Count > 0)
                            {
                                substDotNode1 = new dot_node(substDotNode1, parts.Pop());
                            }
                        }
                        else
                        {
                            var parts = new Stack<ident>();
                            var dn = substDotNode;
                            parts.Push((ident)dn.right);

                            while (!(dn.left is ident && dn.right is ident))
                            {
                                dn = (dot_node)dn.left;
                                parts.Push((ident)dn.right);
                            }

                            parts.Push((ident)dn.left);

                            substDotNode1 = new dot_node(new ident(_capturedVarsClassDefs[scopeAsLambda.ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod.Value].GeneratedUpperClassFieldName), parts.Pop());
                            
                            while (parts.Count > 0)
                            {
                                substDotNode1 = new dot_node(substDotNode1, parts.Pop());    
                            }
                        }

                        var substKey = new SubstitutionKey(varName, syntaxTreeNodeWithVarDeclaration,
                                                       scope.ChildNodes[0].CorrespondingSyntaxTreeNode);
                        if (!_substitutions.ContainsKey(substKey))
                        {
/*#if DEBUG
                            System.IO.File.AppendAllText("d:\\w.txt", "2 substitutions.Add: " + substKey + " " + substDotNode + "\n");
#endif*/

                            _substitutions.Add(substKey, substDotNode1);
                        }
                        AddReferencesToIdentInLambda(_capturedVarsClassDefs[scopeAsLambda.ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod.Value].ClassDeclaration, scopeAsLambda.ChildNodes[0], varName, syntaxTreeNodeWithVarDeclaration, substDotNode1, true);
                    }
                    else
                    {
                        // SSM 25.06.19 fix #1988 - заменил ошибочное scope.ChildNodes[0] на scope.ChildNodes[i]
                        AddReferencesToIdentInLambda(upperScopeWhereVarsAreCapturedClass, scope.ChildNodes[i], varName, syntaxTreeNodeWithVarDeclaration, substDotNode, nestedLambda);
                    }
                }
            }
/*#if DEBUG
            System.IO.File.AppendAllText("d:\\w.txt", "AddR exit: " + scope.ScopeIndex+"\n");
#endif*/
        }

        private void VisitTreeAndBuildClassDefinitions(CapturedVariablesTreeNode currentNode)
        {
            var variablesFromThisScopeWhichWereCaptured = currentNode
                .VariablesDefinedInScope
                .Where(var => var.ReferencingLambdas.Count > 0)
                .ToList();

            if (variablesFromThisScopeWhichWereCaptured.Count > 0)
            {
                if (currentNode is CapturedVariablesTreeNodeClassScope)
                {
                    if (!_capturedVarsClassDefs.ContainsKey(currentNode.ScopeIndex))
                    {
                        var classDef = SyntaxTreeBuilder.BuildClassDefinition();
                        var typeDeclaration = new type_declaration(((CapturedVariablesTreeNodeClassScope)currentNode).ClassName, classDef);
                        _capturedVarsClassDefs.Add(currentNode.ScopeIndex,
                                                   new ScopeClassDefinition(currentNode.CorrespondingSyntaxTreeNode,
                                                                            typeDeclaration,
                                                                            currentNode,
                                                                            PascalABCCompiler.StringConstants.self_word));
                    }
                }
                else
                {
                    if (!_capturedVarsClassDefs.ContainsKey(currentNode.ScopeIndex))
                    {
                        var classDef = SyntaxTreeBuilder.BuildClassDefinition();
                        var typeDeclaration = new type_declaration(GeneratedClassName, classDef);
                        _capturedVarsClassDefs.Add(currentNode.ScopeIndex,
                                                   new ScopeClassDefinition(currentNode.CorrespondingSyntaxTreeNode,
                                                                            typeDeclaration,
                                                                            currentNode));
                    }

                   
                    var vars = variablesFromThisScopeWhichWereCaptured
                        .Select(field => field.SymbolInfo.sym_info as IVAriableDefinitionNode)
                        .ToList();

                    var fieldNames = vars
                        .Select(var => new ident(var.name,(var as local_variable)?.loc ?? null))
                        .ToList();

                    var fieldTypes = vars
                        .Select(var => SyntaxTreeBuilder.BuildSemanticType(var.type))
                        .ToList();

                    var classFields = SyntaxTreeBuilder.BuildClassFieldsSection(fieldNames, fieldTypes);

                    ((class_definition) _capturedVarsClassDefs[currentNode.ScopeIndex].ClassDeclaration.type_def).body
                                                                                                                 .Add(
                                                                                                                     classFields);

                    if (currentNode is CapturedVariablesTreeNodeProcedureScope)
                    {
                        var constructorSection = SyntaxTreeBuilder.BuildSimpleConstructorSection(fieldNames,
                                                                                                 fieldNames.Select(
                                                                                                     id =>
                                                                                                     new ident("_" +
                                                                                                               id.name,id.source_context))
                                                                                                           .ToList(),
                                                                                                 fieldTypes);
                        ((class_definition) _capturedVarsClassDefs[currentNode.ScopeIndex].ClassDeclaration.type_def)
                            .body.Add(constructorSection);
                    }
                }

                foreach (var capturedVar in variablesFromThisScopeWhichWereCaptured)
                {
                    VisitCapturedVar(currentNode, capturedVar);
                }
            }

            foreach (CapturedVariablesTreeNode childNode in currentNode.ChildNodes)
            {
                VisitTreeAndBuildClassDefinitions(childNode);
            }

            if (variablesFromThisScopeWhichWereCaptured.Count > 0)
            {
                var vars = variablesFromThisScopeWhichWereCaptured
                    .Select(x =>
                            new
                            {
                                IVarDefinitionNode = x.SymbolInfo.sym_info as IVAriableDefinitionNode,
                                VarDeclNode = x.SyntaxTreeNodeWithVarDeclaration
                            })
                    .Where(x => x.IVarDefinitionNode != null)
                    .ToList();

                foreach (var var in vars)
                {
                    foreach (CapturedVariablesTreeNode childNode in currentNode.ChildNodes)
                    {
                        _rewriteReferencesForNodesThatAreChildNodesToThoseThatContainCapturedVariableInfo.Add(
                            new RewriteReferencesForNodesThatAreChildNodesToThoseThatContainCapturedVariableInfo
                                {
                                    TreeNode = childNode,
                                    Varname = var.IVarDefinitionNode.name,
                                    NodeWithVarDecl = var.VarDeclNode
                                });
                    }
                }
            }
        }

        private void RewriteReferencesForNodesThatAreChildNodesToThoseThatContainCapturedVariable(CapturedVariablesTreeNode node, string varName, syntax_tree_node nodeWithVarDecl)
        {
            if (node is CapturedVariablesTreeNodeLambdaScope)
            {
                RewriteReferencesForNodesThatAreChildNodesToThoseThatContainCapturedVariable(node.ChildNodes[0], varName, nodeWithVarDecl);
                return;
            }

            if (!(node is CapturedVariablesTreeNodeBlockScope || 
                  node is CapturedVariablesTreeNodeForScope || 
                  node is CapturedVariablesTreeNodeForEachScope))
            {
                return;
            }

            var substKey = new SubstitutionKey(varName, nodeWithVarDecl, node.CorrespondingSyntaxTreeNode);
            if (!_substitutions.ContainsKey(substKey))
            {
                var parentNode = node.ParentNode;
                var isReferenceFound = false;
                SubstitutionKey parentSubstKey = null;

                while (parentNode != null && !isReferenceFound)
                {
                    parentSubstKey = new SubstitutionKey(varName, nodeWithVarDecl, parentNode.CorrespondingSyntaxTreeNode);
                    if (_substitutions.ContainsKey(parentSubstKey))
                    {
                        isReferenceFound = true;
                    }
                    else
                    {
                        parentNode = parentNode.ParentNode;
                    }
                }
                
                if (isReferenceFound)
                {
                    _substitutions.Add(substKey, _substitutions[parentSubstKey]);
                }

            }
            foreach (CapturedVariablesTreeNode childNode in node.ChildNodes)
            {
                RewriteReferencesForNodesThatAreChildNodesToThoseThatContainCapturedVariable(childNode, varName, nodeWithVarDecl);
            }
        }

        public VariableSubstitutionsInfo GenerateSubstitutions()
        {
            VisitTreeAndBuildClassDefinitions(_capturedVariablesRootTreeNode);

            foreach (var ls in _lambdaIdReferences.Where(x => x.LambdaScope.ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod.HasValue))
            {
                AddReferencesToIdentInLambda(_capturedVarsClassDefs[ls.LambdaScope.ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod.Value].ClassDeclaration, 
                    ls.LambdaScope.ChildNodes[0], ls.VarName, ls.SyntaxTreeNodeWithVarDeclaration, ls.DotNode, false);
            }

            foreach (var r in _rewriteReferencesForNodesThatAreChildNodesToThoseThatContainCapturedVariableInfo)
            {
                RewriteReferencesForNodesThatAreChildNodesToThoseThatContainCapturedVariable(r.TreeNode, r.Varname, r.NodeWithVarDecl);
            }

            return new VariableSubstitutionsInfo
                {
                    GeneratedScopeClassesInfo = _capturedVarsClassDefs,
                    SubstitutionsInfo = _substitutions,
                    LambdasToBeAddedAsMethods = _lambdasToBeAddedAsMethods,
                    ConvertingClassNonPublicMembersMapping = _capturedVariablesRootTreeNode is CapturedVariablesTreeNodeClassScope ? 
                        ((CapturedVariablesTreeNodeClassScope)_capturedVariablesRootTreeNode).NonPublicMembersNamesMapping : null
                };
        }
    }
}