// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using PascalABCCompiler.SemanticTree;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;
using array_const = PascalABCCompiler.SyntaxTree.array_const;
using for_node = PascalABCCompiler.SyntaxTree.for_node;

namespace TreeConverter.LambdaExpressions.Closure
{
    internal class SubstitutionKey
    {
        private readonly string _variableName;
        private readonly syntax_tree_node _syntaxTreeNodeWhereVaribleIsDeclared;
        private readonly syntax_tree_node _syntaxTreeUnderSubstitution;

        public SubstitutionKey(string varName, syntax_tree_node varDefLocation, syntax_tree_node block)
        {
            _variableName = varName.ToLower();
            _syntaxTreeNodeWhereVaribleIsDeclared = varDefLocation;
            _syntaxTreeUnderSubstitution = block;
        }
        
        public bool IsPartialKeyEquals(syntax_tree_node block)
        {
            return block == _syntaxTreeUnderSubstitution;
        }

        public override bool Equals(object obj)
        {
            var other = obj as SubstitutionKey;
			if (other == null)
			{
			    return false;
			}

            return _variableName == other._variableName &&
                   _syntaxTreeNodeWhereVaribleIsDeclared == other._syntaxTreeNodeWhereVaribleIsDeclared &&
                   _syntaxTreeUnderSubstitution == other._syntaxTreeUnderSubstitution;
        }

        public override int GetHashCode()
        {
            Func<syntax_tree_node, int> getHashCodeOfSyntaxTreeNode = stn =>
                {
                    if (stn == null || stn.source_context == null)
                    {
                        return 0;
                    }

                    var begin = stn.source_context.begin_position;
                    var end = stn.source_context.end_position;

                    return (begin == null ? 0 : begin.column_num.GetHashCode() ^ begin.line_num.GetHashCode()) ^
                           (end == null ? 0 : end.column_num.GetHashCode() ^ end.line_num.GetHashCode());
                };

            return _variableName.ToLower().GetHashCode() ^
                (_syntaxTreeNodeWhereVaribleIsDeclared == null ? 0 : _syntaxTreeNodeWhereVaribleIsDeclared.GetHashCode()) ^
                (_syntaxTreeUnderSubstitution == null ? 0 : _syntaxTreeUnderSubstitution.GetHashCode());
        }
    }

    internal class CapturedVariablesSubstitutor : WalkingVisitorNew
    {
        private readonly Dictionary<SubstitutionKey, List<ident>> _identsReferences;
        private readonly Dictionary<int, CapturedVariablesSubstitutionClassGenerator.ScopeClassDefinition> _generatedScopeClassesInfo;
        private readonly Dictionary<SubstitutionKey, dot_node> _substitutionsInfo;
        private readonly syntax_tree_visitor _visitor;
        private readonly Stack<syntax_tree_node> _syntaxTreeNodeStack = new Stack<syntax_tree_node>();
        private readonly Dictionary<int, CapturedVariablesTreeNode> _capturedVarsTreeNodesDictionary;
        private readonly List<CapturedVariablesTreeNodeLambdaScope> _lambdasToBeAddedAsMethods;
        private readonly Dictionary<string, Tuple<string, class_field, semantic_node>> _mappingForNonPublicFieldsOfClass;

        public CapturedVariablesSubstitutor(Dictionary<SubstitutionKey, List<ident>> identsReferences,
                                            Dictionary<int, CapturedVariablesSubstitutionClassGenerator.ScopeClassDefinition> generatedScopeClassesInfo,
                                            List<CapturedVariablesTreeNodeLambdaScope> lambdasToBeAddedAsMethods,
                                            Dictionary<SubstitutionKey, dot_node> substitutionsInfo,
                                            Dictionary<int, CapturedVariablesTreeNode> capturedVarsTreeNodesDictionary,
                                            Dictionary<string, Tuple<string, class_field, semantic_node>> mappingForNonPublicFieldsOfClass,
                                            syntax_tree_visitor visitor)
        {
            _identsReferences = identsReferences;
            _generatedScopeClassesInfo = generatedScopeClassesInfo;
            _substitutionsInfo = substitutionsInfo;
            _capturedVarsTreeNodesDictionary = capturedVarsTreeNodesDictionary;
            _lambdasToBeAddedAsMethods = lambdasToBeAddedAsMethods;
            _mappingForNonPublicFieldsOfClass = mappingForNonPublicFieldsOfClass;
            _visitor = visitor;
        }

        public override void visit(statement_list stmtList)
        {
            _syntaxTreeNodeStack.Push(stmtList);
            base.visit(stmtList);
            _syntaxTreeNodeStack.Pop();
        }

        public override void visit(for_node fn)
        {
            SubstituteInNode(fn, fn); //Здесь обработаются замены поверхностного уровня, то есть, если в initial_value и т.д. содержатся ident

            _syntaxTreeNodeStack.Push(fn); //Здесь обработаются случаи, если initial_value и т.д. представляют собой более сложные случаи, такие как indexer и т.д.
            ProcessNode(fn.initial_value);
            ProcessNode(fn.finish_value);
            ProcessNode(fn.increment_value);
            _syntaxTreeNodeStack.Pop();

            ProcessNode(fn.statements);
        }

        public override void visit(foreach_stmt fn)
        {
            SubstituteInNode(fn, fn); //Здесь обработаются замены поверхностного уровня, то есть, если в initial_value и т.д. содержатся ident

            _syntaxTreeNodeStack.Push(fn); //Здесь обработаются случаи, если in_what представляет собой более сложные случаи, такие как indexer и т.д.
            ProcessNode(fn.in_what);
            _syntaxTreeNodeStack.Pop();

            ProcessNode(fn.stmt);
        }

        private void AddPropertiesToConvertingClass()
        {
            if (_visitor.context._ctn != null)
            {
                if (_mappingForNonPublicFieldsOfClass != null)
                {
                    var pairs = _mappingForNonPublicFieldsOfClass.Where(pair => pair.Value.Item3 is common_property_node);

                    foreach (var pair in pairs)
                    {
                        var name = pair.Value.Item1;
                        var semClassField = pair.Value.Item2;
                        var pn = (common_property_node)pair.Value.Item3;

                        var readFunction = _visitor.GenerateGetMethodForField(pn, compiler_string_consts.GetGetAccessorName(pn.name), semClassField, null);
                        var writeFunction = _visitor.GenerateSetMethodForField(pn, compiler_string_consts.GetSetAccessorName(pn.name), semClassField, null);

                        pn.internal_get_function = readFunction;
                        pn.internal_set_function = writeFunction;

                        _visitor.context._ctn.Scope.AddSymbol(name, new SymbolInfo(pn));
                        _visitor.context._ctn.properties.AddElement(pn);
                    }
                }
            }
        }
        private void SubstituteInNode(syntax_tree_node currentNode, syntax_tree_node nodeWhereSubstitute)
        {
            var identSubnodesIndexes = Enumerable
                .Range(0, nodeWhereSubstitute.subnodes_count)
                .Where(index => nodeWhereSubstitute[index] is ident)
                .ToList();

            if (nodeWhereSubstitute is for_node) //нужно исключить переменную цикла, так как она оброабатывается отдельно в SubstituteForLoopVariables
            {
                var fn = (for_node) nodeWhereSubstitute;
                var indexWithLoopIdent = identSubnodesIndexes.First(ind =>
                    {
                        var id = nodeWhereSubstitute[ind] as ident;
                        if (id == null)
                        {
                            return false;
                        }
                        return id == fn.loop_variable;
                    });

                identSubnodesIndexes.Remove(indexWithLoopIdent);
            }

            if (nodeWhereSubstitute is foreach_stmt) //нужно исключить переменную цикла, так как она оброабатывается отдельно в SubstituteForEachLoopVariables
            {
                var fn = (foreach_stmt)nodeWhereSubstitute;
                var indexWithLoopIdent = identSubnodesIndexes.First(ind =>
                {
                    var id = nodeWhereSubstitute[ind] as ident;
                    if (id == null)
                    {
                        return false;
                    }
                    return id == fn.identifier;
                });

                identSubnodesIndexes.Remove(indexWithLoopIdent);
            }

            if (identSubnodesIndexes.Count == 0)
            {
                return;
            }

            var keys = _substitutionsInfo
                .Keys
                .Where(k => k.IsPartialKeyEquals(currentNode))
                .Where(k => _identsReferences.ContainsKey(k));
            
            foreach (var key in keys)
            {
                var subst = _substitutionsInfo[key];
                foreach (var id in _identsReferences[key])
                {
                    for (var i = 0; i < identSubnodesIndexes.Count; i++)
                    {
                        if (nodeWhereSubstitute[identSubnodesIndexes[i]] == id)
                        {
                            closure_substituting_node csn = new closure_substituting_node(subst);
                            if (subst.right is ident)
                                csn.name = (subst.right as ident).name;
                            nodeWhereSubstitute[identSubnodesIndexes[i]] = csn;
                        }
                    }
                }
            }
        }

        private void OnNodeLeave(syntax_tree_node node)
        {
            if (_syntaxTreeNodeStack.Count == 0)
            {
                return;
            }

            if (node is for_node || node is foreach_stmt) // заголовки циклов обрабатываются отдельно
            {
                return;
            }

            var currentBlock = _syntaxTreeNodeStack.Peek();
            SubstituteInNode(currentBlock, node);
        }
        
        private List<CapturedVariablesTreeNodeLambdaScope> GetNestedLambdasToBeAddedToThisClass(CapturedVariablesTreeNode scope)
        {
            var res = new List<CapturedVariablesTreeNodeLambdaScope>();

            foreach (var child in scope.ChildNodes)
            {
                var childAsLambda = child as CapturedVariablesTreeNodeLambdaScope;
                if (childAsLambda == null)
                {
                    res.AddRange(GetNestedLambdasToBeAddedToThisClass(child));
                }
                else
                {
                    if (childAsLambda.ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod.HasValue)
                    {
                        continue;
                    }
                    else
                    {
                        res.Add(childAsLambda);
                        res.AddRange(GetNestedLambdasToBeAddedToThisClass(childAsLambda));
                    }
                }
            }

            return res;
        }

        private void SubstituteLambdas()
        {
            for (var i = 0; i < _lambdasToBeAddedAsMethods.Count; i++)
            {
                if (_lambdasToBeAddedAsMethods[i].ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod != null)
                {
                    var classDef = _generatedScopeClassesInfo[_lambdasToBeAddedAsMethods[i].ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod.Value];
                    
                    if (classDef.CorrespondingTreeNode is CapturedVariablesTreeNodeClassScope)
                    {
                        continue;
                    }
                    
                    var otherLambdasToBeAddedAsMethods =
                        GetNestedLambdasToBeAddedToThisClass(_lambdasToBeAddedAsMethods[i]);
                    classDef.NestedLambdas.AddRange(otherLambdasToBeAddedAsMethods);

                    foreach (var oltaam in otherLambdasToBeAddedAsMethods)
                    {
                        oltaam.LambdaDefinition.substituting_node = new ident(oltaam.LambdaDefinition.lambda_name);
                    }

                    var left = new ident(classDef.GeneratedSubstitutingFieldName);

                    var lambdaDef = _lambdasToBeAddedAsMethods[i].LambdaDefinition;
                    lambdaDef.substituting_node = new dot_node(left, new ident(lambdaDef.lambda_name));

                    /*var classDecl = (class_definition)classDef.ClassDeclaration.type_def;
                    var procDecl = LambdaHelper.ConvertLambdaNodeToProcDefNode(lambdaDef);

                    classDecl.body.Add(SyntaxTreeBuilder.BuildOneMemberSection(procDecl));*/
                }
            }
        }

        private void SubstituteForLoopVariables()
        {
            var forScopes =
                _capturedVarsTreeNodesDictionary.Where(n => n.Value is CapturedVariablesTreeNodeForScope)
                                                .Select(n => (CapturedVariablesTreeNodeForScope) n.Value)
                                                .ToList();

            foreach (var forScope in forScopes)
            {
                var forNode = forScope.CorrespondingSyntaxTreeNode as for_node;
                if (forNode != null)
                {
                    CapturedVariablesSubstitutionClassGenerator.ScopeClassDefinition generatedClass;
                    if (_generatedScopeClassesInfo.TryGetValue(forScope.ScopeIndex, out generatedClass))
                    {
                        var nodesToAdd = new List<statement>();
                        var enclosedStatementList = (statement_list) forScope.EnclosedUpperBlockScope.CorrespondingSyntaxTreeNode;
                        nodesToAdd.Add(generatedClass.GeneratedVarStatementForScope);
                        if (generatedClass.AssignNodeForUpperClassFieldInitialization != null)
                        {
                            nodesToAdd.Add(generatedClass.AssignNodeForUpperClassFieldInitialization);
                        }

                        var forNodeIndex = enclosedStatementList.subnodes.FindIndex(stmt => stmt == forNode);
                        enclosedStatementList.subnodes.InsertRange(forNodeIndex, nodesToAdd);


                        var substDotNode = new dot_node(new ident(generatedClass.GeneratedSubstitutingFieldName), new ident(forNode.loop_variable.name));
                        var assignNode = new assign(substDotNode, new ident(forNode.loop_variable.name));    // TODO: SourceContexts!!!!!
                        ((statement_list)forNode.statements).subnodes.Insert(0, assignNode); 
                    }
                    else
                    {
                        var si = forScope.SymbolInfoLoopVar;
                        var scopeWhereVarDefined = _capturedVarsTreeNodesDictionary[si.scope.ScopeNum];
                        var idRef = scopeWhereVarDefined
                            .VariablesDefinedInScope
                            .Find(var => var.SymbolInfo == si);                                                 //TODO: !!!!!!!!!!!!!!!!!!!!!!

                        var substKey = new SubstitutionKey(forNode.loop_variable.name, idRef.SyntaxTreeNodeWithVarDeclaration, forNode); 

                        if (_substitutionsInfo.ContainsKey(substKey)) // Если нашли замену, то переменная где-то захватывалась
                        {
                            forNode.create_loop_variable = true;
                            var substDotNode = _substitutionsInfo[substKey];

                            var assignNode = new assign(substDotNode, new ident(forNode.loop_variable.name));    // TODO: SourceContexts!!!!!
                            (((statement_list)forNode.statements).subnodes).Insert(0, assignNode);
                        }
                    }

                }
            }
        }

        private void SubstituteForEachLoopVariables()
        {
            var forEachScopes =
                _capturedVarsTreeNodesDictionary.Where(n => n.Value is CapturedVariablesTreeNodeForEachScope)
                                                .Select(n => (CapturedVariablesTreeNodeForEachScope)n.Value)
                                                .ToList();

            foreach (var forEachScope in forEachScopes)
            {
                var forEachNode = forEachScope.CorrespondingSyntaxTreeNode as foreach_stmt;
                if (forEachNode != null)
                {
                    CapturedVariablesSubstitutionClassGenerator.ScopeClassDefinition generatedClass;
                    if (_generatedScopeClassesInfo.TryGetValue(forEachScope.ScopeIndex, out generatedClass))
                    {
                        var nodesToAdd = new List<statement>();
                        var bodyStmtList = (statement_list)forEachNode.stmt;
                        nodesToAdd.Add(generatedClass.GeneratedVarStatementForScope);
                        if (generatedClass.AssignNodeForUpperClassFieldInitialization != null)
                        {
                            nodesToAdd.Add(generatedClass.AssignNodeForUpperClassFieldInitialization);
                        }

                        var substDotNode = new dot_node(new ident(generatedClass.GeneratedSubstitutingFieldName), new ident(forEachNode.identifier.name));
                        var assignNode = new assign(substDotNode, new ident(forEachNode.identifier.name));    // TODO: SourceContexts!!!!!
                        nodesToAdd.Add(assignNode);

                        bodyStmtList.subnodes.InsertRange(0, nodesToAdd);
                    }
                    else
                    {
                        var si = forEachScope.SymbolInfoLoopVar;
                        var scopeWhereVarDefined = _capturedVarsTreeNodesDictionary[si.scope.ScopeNum];
                        var idRef = scopeWhereVarDefined
                            .VariablesDefinedInScope
                            .Find(var => var.SymbolInfo == si);                                                 //TODO: !!!!!!!!!!!!!!!!!!!!!!

                        var substKey = new SubstitutionKey(forEachNode.identifier.name, idRef.SyntaxTreeNodeWithVarDeclaration, forEachNode);

                        if (_substitutionsInfo.ContainsKey(substKey)) // Если нашли замену, то переменная где-то захватывалась
                        {
                            if (forEachNode.type_name == null)  //Нужно, чтобы переменная цикла создалась в этом случае в контескте с этим же именем что и была
                            {
                                forEachNode.type_name = new no_type_foreach();
                            }

                            var substDotNode = _substitutionsInfo[substKey];

                            var assignNode = new assign(substDotNode, new ident(forEachNode.identifier.name));    // TODO: SourceContexts!!!!!
                            (((statement_list)forEachNode.stmt).subnodes).Insert(0, assignNode);
                        }
                    }

                }
            }
        }

        private void SubstituteLambdaParameters()
        {
            var lambdaScopes =
                _capturedVarsTreeNodesDictionary.Where(n => n.Value is CapturedVariablesTreeNodeLambdaScope)
                                                .Select(n => (CapturedVariablesTreeNodeLambdaScope)n.Value)
                                                .ToList();

            foreach (var lambdaScope in lambdaScopes)
            {
                CapturedVariablesSubstitutionClassGenerator.ScopeClassDefinition generatedClass;
                if (_generatedScopeClassesInfo.TryGetValue(lambdaScope.ScopeIndex, out generatedClass))
                {

                    var nodesToAdd = new List<statement>();
                    nodesToAdd.Add(generatedClass.GeneratedVarStatementForScope);
                    if (generatedClass.AssignNodeForUpperClassFieldInitialization != null)
                    {
                        nodesToAdd.Add(generatedClass.AssignNodeForUpperClassFieldInitialization);
                    }

                    var variables = lambdaScope
                        .VariablesDefinedInScope
                        .Where(l => l.ReferencingLambdas.Count > 0)
                        .Select(var => ((IVAriableDefinitionNode) var.SymbolInfo.sym_info).name.ToLower())
                        .ToList();

                    foreach (var variable in variables)
                    {
                        var assignStmt = new assign(new dot_node(new ident(generatedClass.GeneratedSubstitutingFieldName),
                                                                    new ident(variable)), new ident(variable));
                        nodesToAdd.Add(assignStmt);
                    }

                    ((statement_list)lambdaScope.LambdaDefinition.proc_body).subnodes.InsertRange(0, nodesToAdd);
                }
            }
        }

        private void SubstituteVariablesDeclarations()
        {
            var classDefsTreeNodes = _generatedScopeClassesInfo.Join(_capturedVarsTreeNodesDictionary,
                                                                     outer => outer.Key,
                                                                     inner => inner.Key,
                                                                     (outer, inner) => new
                                                                         {
                                                                             ClassDeclaration = outer.Value,
                                                                             TreeNode = inner.Value
                                                                         })
                                                               .Where(p => p.TreeNode is CapturedVariablesTreeNodeBlockScope)
                                                               .Select(p => new
                                                                   {
                                                                       p.ClassDeclaration,
                                                                       TreeNode = (CapturedVariablesTreeNodeBlockScope) p.TreeNode
                                                                   }); //TODO: Сейчас рассматриваются только захватываемые переменные, определенные внутри какого-либо блока. Рассмотреть остальные случаи.

            foreach (var classDefTreeNode in classDefsTreeNodes)
            {
                var statementListNode = classDefTreeNode.TreeNode.CorrespondingSyntaxTreeNode as statement_list; //TODO: Сейчас рассматриваются только захватываемые переменные, определенные внутри какого-либо блока. Рассмотреть остальные случаи.
                if (statementListNode != null)
                {
                    var variables = classDefTreeNode
                        .TreeNode
                        .VariablesDefinedInScope
                        .Where(var => var.ReferencingLambdas.Count > 0)
                        .GroupBy(var => var.SyntaxTreeNodeWithVarDeclaration)
                        .Select(gr => new
                            {
                                SyntaxTreeNodeWithVarDeclaration = gr.Key,
                                Vars = gr.ToList()
                            })
                        .ToList();

                    if (variables.Count == 0)
                    {
                        return;
                    }

                    var newStmtList = new statement_list();
                    newStmtList.Add(classDefTreeNode.ClassDeclaration.GeneratedVarStatementForScope);
                    if (classDefTreeNode.ClassDeclaration.AssignNodeForUpperClassFieldInitialization != null)
                    {
                        newStmtList.Add(classDefTreeNode.ClassDeclaration.AssignNodeForUpperClassFieldInitialization);
                    }

                    var stmtListQueue = new Queue<statement>(statementListNode.subnodes);
                    while (stmtListQueue.Count > 0)
                    {
                        var currentStatement = stmtListQueue.Dequeue();
                        var varStatement = currentStatement as var_statement;                                                   //TODO: пока что только локальные переменные, определенные внутри var_def_statement!!!!!!!!
                        if (varStatement != null &&
                            varStatement.var_def != null)
                        {
                            var varDefKey =
                                variables.FirstOrDefault(gr => gr.SyntaxTreeNodeWithVarDeclaration == varStatement.var_def);
                            if (varDefKey != null)
                            {
                                var varsToExclude =
                                    varDefKey.Vars.Select(var => ((IVAriableDefinitionNode) var.SymbolInfo.sym_info).name.ToLower()); //TODO: пока что только локальные переменные!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                                    
                                var newVarList = varStatement.var_def.vars.idents
                                    .Where(id => !varsToExclude.Contains(id.name.ToLower()))
                                    .ToList();

                                if (newVarList.Count > 0)
                                {
                                    var newVarDefStmt = new var_def_statement(new ident_list(newVarList), varStatement.var_def.vars_type)
                                        {
                                            inital_value = varStatement.var_def.inital_value
                                        };
                                    var newVarStmt = new var_statement(newVarDefStmt);
                                    newStmtList.Add(newVarStmt);
                                }

                                if (varStatement.var_def.inital_value != null)
                                {
                                    var initVal = varStatement.var_def.inital_value;
                                    string auxVarName = null;

                                    if (initVal is array_const)
                                    {
                                        auxVarName = LambdaHelper.GetAuxVarName();
                                        var newVarDefStmt = new var_def_statement(new ident_list(new ident(auxVarName)), varStatement.var_def.vars_type)
                                        {
                                            inital_value = initVal
                                        };
                                        var newVarStmt = new var_statement(newVarDefStmt);
                                        newStmtList.Add(newVarStmt);
                                    }

                                    foreach (var variable in varsToExclude)
                                    {
                                        var assignStmt =
                                            new assign(new dot_node(new ident(classDefTreeNode.ClassDeclaration.GeneratedSubstitutingFieldName),
                                                                    new ident(variable)),
                                                        auxVarName == null ? varStatement.var_def.inital_value : new ident(auxVarName));

                                        newStmtList.Add(assignStmt);
                                    }
                                }
                            }
                            else
                            {
                                newStmtList.Add(currentStatement);
                            }
                        }
                        else
                        {
                            newStmtList.Add(currentStatement);
                        }
                    }
                    
                    statementListNode.subnodes.Clear();
                    statementListNode.subnodes.AddRange(newStmtList.subnodes);
                }
            }
        }

        private Tuple<type_declaration, List<procedure_definition>> CreateTypeDeclarationWithForwardDeclaration(type_declaration cl)
        {
            var oldClDef = (class_definition) cl.type_def;
            var classDef = SyntaxTreeBuilder.BuildClassDefinition();
            var typeDeclaration = new type_declaration(cl.type_name, classDef);
            classDef.where_section = oldClDef.where_section;
            var procedures = new List<procedure_definition>();
            var classMembers = new class_members(access_modifer.public_modifer);
            classDef.body.class_def_blocks.Add(classMembers);

            foreach (var member in oldClDef.body.class_def_blocks.SelectMany(x => x.members))
            {
                if (member is var_def_statement)
                {
                    classMembers.Add(member);
                }
                else
                {
                    var procDef = (procedure_definition) member;
                    if (procDef.proc_header is constructor)
                    {
                        classMembers.Add(procDef);
                        continue;
                    }
                    procedure_header procHeader;
                    if (procDef.proc_header is function_header)
                    {
                        var fh = (function_header) procDef.proc_header;
                        procHeader = new function_header
                            {
                                name = new method_name(fh.name.meth_name.name),
                                source_context = fh.source_context,
                                parameters = fh.parameters,
                                of_object = fh.of_object,
                                class_keyword = fh.class_keyword
                            };
                        ((function_header)procHeader).return_type = fh.return_type;
                    }
                    else
                    {
                        procHeader = new procedure_header
                            {
                                name = new method_name(procDef.proc_header.name.meth_name.name),
                                source_context = procDef.proc_header.source_context,
                                parameters = procDef.proc_header.parameters,
                                of_object = procDef.proc_header.of_object,
                                class_keyword = procDef.proc_header.class_keyword
                            };
                    }

                    procDef.proc_header.name.class_name = cl.type_name;
                    procedures.Add(procDef);
                    classMembers.Add(procHeader);
                }
            }

            return new Tuple<type_declaration, List<procedure_definition>>(typeDeclaration, procedures);
        }

        private List<procedure_definition> VisitClassDefinitions()
        {
            var procedures = new List<procedure_definition>();
            _visitor.context.SaveContextAndUpToGlobalLevel();
            try
            {
                _visitor.lambdaProcessingState = LambdaProcessingState.ClosuresProcessingVisitGeneratedClassesPhase;

                var lambdaGroups =
                    _lambdasToBeAddedAsMethods.GroupBy(l => l.ScopeIndexOfClassWhereLambdaWillBeAddedAsMethod)
                                              .ToDictionary(x => x.Key, x => x.ToList());
                foreach (var clDecl in _generatedScopeClassesInfo.Where(cd => !(cd.Value.CorrespondingTreeNode is CapturedVariablesTreeNodeClassScope)).OrderBy(cd => cd.Key))
                {
                    var lambdas = clDecl.Value.NestedLambdas;
                    List<CapturedVariablesTreeNodeLambdaScope> ls;
                    if (lambdaGroups.TryGetValue(clDecl.Key, out ls))
                    {
                        lambdas = lambdas.Concat(ls).ToList();
                    }
                    var classDecl = (class_definition)clDecl.Value.ClassDeclaration.type_def;
                    foreach (var lambdaDef in lambdas)
                    {
                        var procDecl = LambdaHelper.ConvertLambdaNodeToProcDefNode(lambdaDef.LambdaDefinition);
                        classDecl.body.Add(SyntaxTreeBuilder.BuildOneMemberSection(procDecl));
                    }

                    var cl = CreateTypeDeclarationWithForwardDeclaration(clDecl.Value.ClassDeclaration);
                    procedures.AddRange(cl.Item2);
                    _visitor.visit(cl.Item1);
                }
            }
            finally
            {
                _visitor.context.RestoreCurrentContext();
                _visitor.lambdaProcessingState = LambdaProcessingState.ClosuresProcessingPhase;
            }
            

            return procedures;
        }

        private void VisitProceduresOfClasses(List<procedure_definition> procedures)
        {
            _visitor.context.SaveContextAndUpToGlobalLevel();
            _visitor.lambdaProcessingState = LambdaProcessingState.ClosuresProcessingVisitGeneratedClassesPhase;
            try
            { 
                procedures.ForEach(p => _visitor.visit(p));
            }
            finally
            {
                _visitor.context.RestoreCurrentContext();
                _visitor.lambdaProcessingState = LambdaProcessingState.ClosuresProcessingPhase;
            }
        }

        private void SubstituteVarDefInProcedure(statement_list statementList) //TODO: сейчас обрабатывается так, как будто блока объявлений declarations в функции нет. Нужно будет предусмотреть, но не только для функций, а еще для всех узлов где есть block, который содержит declarations
        {
            var classDefsTreeNode = _generatedScopeClassesInfo.Join(_capturedVarsTreeNodesDictionary,
                                                                     outer => outer.Key,
                                                                     inner => inner.Key,
                                                                     (outer, inner) => new
                                                                         {
                                                                             ClassDeclaration = outer.Value,
                                                                             TreeNode = inner.Value
                                                                         })
                                                               .Where(p => p.TreeNode is CapturedVariablesTreeNodeProcedureScope)
                                                               .Select(p => new
                                                                   {
                                                                       p.ClassDeclaration,
                                                                       TreeNode = (CapturedVariablesTreeNodeProcedureScope)p.TreeNode
                                                                   })
                                                               .FirstOrDefault();

            if (classDefsTreeNode == null)
            {
                return;
            }

            var variables = classDefsTreeNode
                        .TreeNode
                        .VariablesDefinedInScope
                        .Where(l => l.ReferencingLambdas.Count > 0)
                        .ToList();

            if (variables.Count == 0)
            {
                return;
            }

            CapturedVariablesSubstitutionClassGenerator.ScopeClassDefinition generatedClass;
            if (_generatedScopeClassesInfo.TryGetValue(classDefsTreeNode.TreeNode.ScopeIndex, out generatedClass))
            {
                var exprList = variables.Select(v => (expression)new ident(((IVAriableDefinitionNode) v.SymbolInfo.sym_info).name)).ToList();

                if (IsInGenerics)
                {
                    var newVarDefSyntaxTreeNode = new var_def_statement(generatedClass.GeneratedSubstitutingFieldName, new template_type_reference(new named_type_reference(generatedClass.ClassDeclaration.type_name.name), new template_param_list(AllGenericParameters.Select(l => SyntaxTreeBuilder.BuildSimpleType(l.name)).ToList())));
                    newVarDefSyntaxTreeNode.inital_value =
                        new new_expr(new template_type_reference(new named_type_reference(generatedClass.ClassDeclaration.type_name.name), new template_param_list(AllGenericParameters.Select(l => SyntaxTreeBuilder.BuildSimpleType(l.name)).ToList())),
                                        new expression_list(exprList), null);

                    generatedClass.GeneratedVarStatementForScope.var_def = newVarDefSyntaxTreeNode;
                }
                else
                {
                    generatedClass.GeneratedVarStatementForScope.var_def.inital_value =
                        new new_expr(SyntaxTreeBuilder.BuildSimpleType(generatedClass.ClassDeclaration.type_name.name),
                                     new expression_list(exprList), null);
                }

                var nodesToAdd = new List<statement>();
                nodesToAdd.Add(generatedClass.GeneratedVarStatementForScope);

                if (generatedClass.AssignNodeForUpperClassFieldInitialization != null)
                {
                    nodesToAdd.Add(generatedClass.AssignNodeForUpperClassFieldInitialization);
                }

                statementList.subnodes.InsertRange(0, nodesToAdd);
            }
        }

        private void SubstituteTypesInCaseOfGenerics()
        {
            if (!IsInGenerics)
            {
                return;
            }

            /* TODO: нужно заменить в полях сгенерированных классов, параметрах и возвращаемом значении лямбд - у всех, с учетом того как чистятся все дженерики...
            /* Предусмотреть случай, когда захвата переменных не происходит, но мы все равно находимся в лямбде - это нужно сделать на FinishPhase в syntax_tree_visitor
            /* Не забыть про where секцию */

            var nestedLambdas = new List<CapturedVariablesTreeNodeLambdaScope>();
            foreach (var cl in _generatedScopeClassesInfo.Where(cd => !(cd.Value.CorrespondingTreeNode is CapturedVariablesTreeNodeClassScope)))
            {
                nestedLambdas.AddRange(cl.Value.NestedLambdas);
            }

            foreach (CapturedVariablesTreeNodeLambdaScope t in _lambdasToBeAddedAsMethods.Concat(nestedLambdas))
            {
                var lambdaDef = t.LambdaDefinition;
                if (lambdaDef.formal_parameters != null && lambdaDef.formal_parameters.params_list.Count > 0)
                {
                    for (var j = 0; j < lambdaDef.formal_parameters.params_list.Count; j++)
                    {
                        if (lambdaDef.formal_parameters.params_list[j].vars_type is lambda_inferred_type)
                        {
                            if ((lambdaDef.formal_parameters.params_list[j].vars_type as lambda_inferred_type).real_type is type_node)
                            {
                                lambdaDef.formal_parameters.params_list[j].vars_type = 
                                    LambdaHelper.ConvertSemanticTypeToSyntaxType((type_node)(lambdaDef.formal_parameters.params_list[j].vars_type as lambda_inferred_type).real_type);
                            }
                        }
                    }
                }

                if (lambdaDef.return_type is lambda_inferred_type)
                {
                    if ((lambdaDef.return_type as lambda_inferred_type).real_type is type_node)
                    {
                        lambdaDef.return_type = LambdaHelper.ConvertSemanticTypeToSyntaxType((type_node)(lambdaDef.return_type as lambda_inferred_type).real_type);
                    }
                }
            }
            
            foreach (var clDecl in _generatedScopeClassesInfo.Where(cd => !(cd.Value.CorrespondingTreeNode is CapturedVariablesTreeNodeClassScope)))
            {
                var fields =
                    ((class_definition) clDecl.Value.ClassDeclaration.type_def).body.class_def_blocks
                    .SelectMany(d => d.members)
                    .OfType<var_def_statement>();

                foreach (var field in fields)
                {
                    var semTypeNode = field.vars_type as semantic_type_node;
                    if (semTypeNode != null)
                    {
                        if (semTypeNode.type is type_node)
                        {
                            field.vars_type = LambdaHelper.ConvertSemanticTypeToSyntaxType((type_node) semTypeNode.type);
                        }
                    }
                }

                var constructors =
                    ((class_definition)clDecl.Value.ClassDeclaration.type_def).body.class_def_blocks
                    .SelectMany(d => d.members)
                    .OfType<procedure_definition>()
                    .Where(pr => pr.proc_header is constructor);

                foreach (var constructor in constructors)
                {
                    if (constructor.proc_header != null && constructor.proc_header.parameters != null && constructor.proc_header.parameters.params_list != null)
                    {
                        var pars = constructor.proc_header.parameters.params_list;
                        foreach (var p in pars)
                        {
                            var semTypeNode = p.vars_type as semantic_type_node;
                            if (semTypeNode != null)
                            {
                                if (semTypeNode.type is type_node)
                                {
                                    p.vars_type = LambdaHelper.ConvertSemanticTypeToSyntaxType((type_node)semTypeNode.type);
                                }
                            }
                        }
                    }
                }

                var upperField =
                    ((class_definition)clDecl.Value.ClassDeclaration.type_def).body.class_def_blocks
                    .SelectMany(d => d.members)
                    .OfType<var_def_statement>()
                    .FirstOrDefault(vds =>
                        {
                            if (vds.vars != null && vds.vars.idents != null)
                            {
                                if (vds.vars.idents.FirstOrDefault(id => id.name == clDecl.Value.GeneratedUpperClassFieldName) != null)
                                {
                                    return true;
                                }
                            }
                            return false;
                        });

                if (upperField != null)
                {
                    var genericParameters = AllGenericParameters;

                    if (_visitor.context._ctn != null && _visitor.context._ctn.generic_params != null)
                    {
                        var tr = upperField.vars_type as named_type_reference;
                        if (tr != null && tr.names != null && tr.names.Count == 1)
                        {
                            if (tr.names[0].name == _visitor.context._ctn.name)
                            {
                                genericParameters = ClassGenericParameters;
                            }
                        }
                    }

                    upperField.vars_type =
                        new template_type_reference(
                            (named_type_reference)upperField.vars_type,
                            new template_param_list(genericParameters.Select(l => SyntaxTreeBuilder.BuildSimpleType(l.name)).ToList()));
                }

                if (clDecl.Value.GeneratedVarStatementForScope != null)
                {
                    var oldInitialValueExpressions =
                        ((new_expr) clDecl.Value.GeneratedVarStatementForScope.var_def.inital_value).params_list;

                    var newVarDefSyntaxTreeNode = new var_def_statement(clDecl.Value.GeneratedSubstitutingFieldName, new template_type_reference(new named_type_reference(clDecl.Value.ClassDeclaration.type_name.name), new template_param_list(AllGenericParameters.Select(l => SyntaxTreeBuilder.BuildSimpleType(l.name)).ToList())));
                    newVarDefSyntaxTreeNode.inital_value =
                        new new_expr(new template_type_reference(new named_type_reference(clDecl.Value.ClassDeclaration.type_name.name), new template_param_list(AllGenericParameters.Select(l => SyntaxTreeBuilder.BuildSimpleType(l.name)).ToList())),
                                     oldInitialValueExpressions, null);

                    clDecl.Value.GeneratedVarStatementForScope.var_def = newVarDefSyntaxTreeNode;
                }

                clDecl.Value.ClassDeclaration.type_name = new template_type_name(clDecl.Value.ClassDeclaration.type_name.name, new ident_list(AllGenericParameters));

                if (WhereSection != null && WhereSection.defs != null && WhereSection.defs.Count != 0)
                {
                    ((class_definition) clDecl.Value.ClassDeclaration.type_def).where_section = WhereSection;
                }
            }
        }

        private bool IsInGenerics
        {
            get
            {
                var funcStackAsList = _visitor.context.func_stack == null
                                          ? null
                                          : _visitor.context.func_stack.CloneInternalStack().ToList();

                var funcIsGeneric = false;
                if (funcStackAsList != null)
                {
                    funcIsGeneric = funcStackAsList.FirstOrDefault(f => f.generic_params != null) != null;
                }

                return funcIsGeneric || _visitor.context._ctn != null && _visitor.context._ctn.generic_params != null;

            }
        }

        private List<ident> AllGenericParameters
        {
            get
            {
                if (!IsInGenerics)
                {
                    return null;
                }

                var res = new List<ident>();

                if (_visitor.context._ctn != null && _visitor.context._ctn.generic_params != null)
                {
                    res.AddRange(_visitor.context._ctn.generic_params.Select(par => new ident(par.name)));
                }

                var funcStackAsList = _visitor.context.func_stack == null
                                          ? null
                                          : _visitor.context.func_stack.CloneInternalStack().ToList();

                if (funcStackAsList != null)
                {
                    var funcGeneric = funcStackAsList.FirstOrDefault(f => f.generic_params != null);
                    if (funcGeneric != null)
                    {
                        res.AddRange(funcGeneric.generic_params.Select(par => new ident(par.name)));
                    }
                }
                
                return res;
            }
        }

        private List<ident> ClassGenericParameters
        {
            get
            {
                if (!IsInGenerics)
                {
                    return null;
                }

                var res = new List<ident>();

                if (_visitor.context._ctn != null && _visitor.context._ctn.generic_params != null)
                {
                    res.AddRange(_visitor.context._ctn.generic_params.Select(par => new ident(par.name)));
                }

                return res;
            }
        }

        public static where_definition_list GetWhereSection(List<generic_parameter_eliminations> genericParameterEliminations, List<ident> pars)
        {
            var list = new List<where_definition>();

            for (var i = 0; i < pars.Count; i++)
            {
                var el = genericParameterEliminations[i];
                var par = pars[i];

                var whereDef = new where_definition
                {
                    names = new ident_list(par),
                    types = new where_type_specificator_list()
                };

                if (el.is_class)
                {
                    var d = new declaration_specificator(DeclarationSpecificator.WhereDefClass, "class");
                    whereDef.types.Add(d);
                }
                if (el.is_value)
                {
                    var d = new declaration_specificator(DeclarationSpecificator.WhereDefValueType, "record");
                    whereDef.types.Add(d);
                }

                if (el.base_class != null && el.base_class != SystemLibrary.object_type)
                {
                    var d = LambdaHelper.ConvertSemanticTypeToSyntaxType(el.base_class);
                    whereDef.types.Add(d);
                }

                foreach (var interf in el.implementing_interfaces)
                {
                    var d = LambdaHelper.ConvertSemanticTypeToSyntaxType(interf);
                    whereDef.types.Add(d);
                }

                if (el.has_default_ctor)
                {
                    var d = new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, "constructor");
                    whereDef.types.Add(d);
                }

                if (whereDef.types.defs.Count != 0)
                {
                    list.Add(whereDef);
                }
            }

            return new where_definition_list(list);
        }

        private where_definition_list _whereSection = null;
        private where_definition_list WhereSection
        {
            get
            {
                if (!IsInGenerics)
                {
                    return null;
                }

                if (_whereSection != null)
                {
                    return _whereSection;
                }

                var genericParameterEliminations = new List<generic_parameter_eliminations>();

                if (_visitor.context._ctn != null && _visitor.context._ctn.generic_params != null)
                {
                    genericParameterEliminations.AddRange(generic_parameter_eliminations.make_eliminations_common(_visitor.context._ctn.generic_params));
                }

                if (_visitor.context.top_function != null && _visitor.context.top_function.generic_params != null)
                {
                    genericParameterEliminations.AddRange(generic_parameter_eliminations.make_eliminations_common(_visitor.context.top_function.generic_params));
                }

                _whereSection = GetWhereSection(genericParameterEliminations, AllGenericParameters);
                return _whereSection;
            }
        }

        public void Substitute(statement_list statementList)
        {
            OnLeave += OnNodeLeave;
            ProcessNode(statementList);
            SubstituteLambdas();
            SubstituteTypesInCaseOfGenerics();
            AddPropertiesToConvertingClass();
            var procedures = VisitClassDefinitions();
            SubstituteVariablesDeclarations();
            SubstituteForLoopVariables();
            SubstituteForEachLoopVariables();
            SubstituteVarDefInProcedure(statementList);
            //SubstituteLambdaParameters();
            VisitProceduresOfClasses(procedures);
        }
    }
}