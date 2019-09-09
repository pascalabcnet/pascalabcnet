// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.Linq;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeConverter;

namespace TreeConverter.LambdaExpressions.Closure
{
    class CapturedVariablesSubstitutionsManager
    {
        public static void Substitute(syntax_tree_visitor _visitor, declarations decls, statement_list _statementList)
        {
            var tree = new CapturedVariablesTreeBuilder(_visitor).BuildTree(_statementList);
            var substs = new CapturedVariablesSubstitutionClassGenerator(tree.RootNode).GenerateSubstitutions();
            new CapturedVariablesSubstitutor(tree.IdentsReferences, substs.GeneratedScopeClassesInfo, substs.LambdasToBeAddedAsMethods, substs.SubstitutionsInfo, tree.CapturedVarsNodesDictionary, substs.ConvertingClassNonPublicMembersMapping, _visitor)
                .Substitute(_statementList);

            if (_visitor.context.converting_block() == block_type.function_block && tree.ProcedureScope != null)
            {
                if (decls != null && decls.defs != null)
                {
                    foreach (var def in decls.defs.Where(d => d is const_definition ||
                                                              d is consts_definitions_list ||
                                                              d is variable_definitions))
                    {
                        var constDef = def as const_definition;
                        if (constDef != null)
                        {
                            var finder = new FindMainIdentsVisitor();
                            finder.ProcessNode(constDef.const_value);

                            foreach (var v in finder.vars)
                            {
                                SymbolInfo si = _visitor.context.find_first(v.name);
                                if (si == null)
                                {
                                    continue;
                                }

                                if(tree.ProcedureScope.VariablesDefinedInScope.Any(var => var.SymbolInfo == si))
                                {
                                    _visitor.AddError(new UsingCapturedParameterIsNotAllowedInInitializers(_visitor.get_location(v)));
                                }
                            }
                            continue;
                        }

                        var constDefList = def as consts_definitions_list;
                        if (constDefList != null)
                        {
                            foreach (var cd in constDefList.const_defs)
                            {
                                var finder = new FindMainIdentsVisitor();
                                finder.ProcessNode(cd.const_value);

                                foreach (var v in finder.vars)
                                {
                                    SymbolInfo si = _visitor.context.find_first(v.name);
                                    if (si == null)
                                    {
                                        continue;
                                    }

                                    if (tree.ProcedureScope.VariablesDefinedInScope.Any(var => var.SymbolInfo == si))
                                    {
                                        _visitor.AddError(new UsingCapturedParameterIsNotAllowedInInitializers(_visitor.get_location(v)));
                                    }
                                }
                            }
                            continue;
                        }

                        var varDefList = def as variable_definitions;
                        if (varDefList != null)
                        {
                            foreach (var d in varDefList.var_definitions)
                            {
                                var finder = new FindMainIdentsVisitor();
                                finder.ProcessNode(d.inital_value);

                                foreach (var v in finder.vars)
                                {
                                    SymbolInfo si = _visitor.context.find_first(v.name);
                                    if (si == null)
                                    {
                                        continue;
                                    }

                                    if (tree.ProcedureScope.VariablesDefinedInScope.Any(var => var.SymbolInfo == si))
                                    {
                                        _visitor.AddError(new UsingCapturedParameterIsNotAllowedInInitializers(_visitor.get_location(v)));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}