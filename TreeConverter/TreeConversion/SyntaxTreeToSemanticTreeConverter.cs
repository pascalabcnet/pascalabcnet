// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

/***************************************************************************
 *   
 *   Интерфейс конвертора синтаксического дерева в семантическое для Compiler   
 *   Зависит от Errors,SemanticTree,PascalABCCompiler.SyntaxTree
 *
 ***************************************************************************/

using PascalABCCompiler.TreeRealization;
using System;
using System.Collections.Generic;
using PascalABCCompiler.TreeConverter;
using System.IO;

namespace PascalABCCompiler.TreeConverter
{
	public class SyntaxTreeToSemanticTreeConverter 
	{
        private PascalABCCompiler.TreeConverter.syntax_tree_visitor stv = new syntax_tree_visitor();
        private PascalABCCompiler.TreeConverter.TreeConversion.SyntaxTreeVisitorsController stvController = 
            new PascalABCCompiler.TreeConverter.TreeConversion.SyntaxTreeVisitorsController();
        public SyntaxTreeToSemanticTreeConverter()
        {
            //(ssyy) запоминаем visitor
            SystemLibrary.SystemLibrary.syn_visitor = stv;
        }

        //TODO: Разобраться, где использутеся.
        public SymbolTable.TreeConverterSymbolTable SymbolTable
		{
			get
			{
				return stv.SymbolTable;
			}
		}

        void SetSemanticRules(SyntaxTree.compilation_unit SyntaxUnit)
        {
            SemanticRules.ClassBaseType = SystemLibrary.SystemLibrary.object_type;
            SemanticRules.StructBaseType = SystemLibrary.SystemLibrary.value_type;
            switch (SyntaxUnit.Language)
            {
                case PascalABCCompiler.SyntaxTree.LanguageId.PascalABCNET:
                    SemanticRules.AddResultVariable = true;
                    SemanticRules.ZeroBasedStrings = false;
                    SemanticRules.FastStrings = false;
                    SemanticRules.InitStringAsEmptyString = true;
                    SemanticRules.UseDivisionAssignmentOperatorsForIntegerTypes = false;
                    SemanticRules.ManyVariablesOneInitializator = false;
                    SemanticRules.OrderIndependedMethodNames = true;
                    SemanticRules.OrderIndependedFunctionNames = false;
                    SemanticRules.OrderIndependedTypeNames = false;
                    SemanticRules.EnableExitProcedure = true;
                    SemanticRules.StrongPointersTypeCheckForDotNet = true;
                    SemanticRules.AllowChangeLoopVariable = false;
                    SemanticRules.AllowGlobalVisibilityForPABCDll = true;
                    break;
                case PascalABCCompiler.SyntaxTree.LanguageId.C:
                    SemanticRules.AddResultVariable = false;
                    SemanticRules.ZeroBasedStrings = true;
                    SemanticRules.InitStringAsEmptyString = false;
                    SemanticRules.UseDivisionAssignmentOperatorsForIntegerTypes = true;
                    SemanticRules.ManyVariablesOneInitializator = false;
                    SemanticRules.OrderIndependedMethodNames = false;
                    SemanticRules.OrderIndependedFunctionNames = false;
                    SemanticRules.OrderIndependedTypeNames = false;
                    SemanticRules.EnableExitProcedure = false;
                    SemanticRules.StrongPointersTypeCheckForDotNet = false;
                    SemanticRules.AllowGlobalVisibilityForPABCDll = false;
                    break;
            }
        }


        //TODO: Исправить коллекцию модулей.
        public PascalABCCompiler.TreeRealization.common_unit_node CompileInterface(SyntaxTree.compilation_unit SyntaxUnit,
            PascalABCCompiler.TreeRealization.unit_node_list UsedUnits, List<Errors.Error> ErrorsList, List<Errors.CompilerWarning> WarningsList, PascalABCCompiler.Errors.SyntaxError parser_error,
            System.Collections.Hashtable bad_nodes, TreeRealization.using_namespace_list namespaces, Dictionary<SyntaxTree.syntax_tree_node,string> docs, bool debug, bool debugging, bool for_intellisense, List<TreeRealization.var_definition_node> CompiledVariables)
		{
            stv = stvController.SelectVisitor(Path.GetExtension(SyntaxUnit.file_name));
            //convertion_data_and_alghoritms.__i = 0;
            stv.parser_error=parser_error;
            stv.bad_nodes_in_syntax_tree = bad_nodes;
			stv.referenced_units=UsedUnits;
			//stv.comp_units=UsedUnits;
			//stv.visit(SyntaxUnit
            //stv.interface_using_list = namespaces;
            stv.using_list.Clear();
            stv.interface_using_list.Clear();
            stv.using_list.AddRange(namespaces);
            stv.current_document = new TreeRealization.document(SyntaxUnit.file_name);
            stv.ErrorsList = ErrorsList;
            stv.WarningsList = WarningsList;
            stv.SymbolTable.CaseSensitive = SemanticRules.SymbolTableCaseSensitive;
            stv.docs = docs;
            stv.debug = debug;
            stv.debugging = debugging;
            stv.for_intellisense = for_intellisense;
			SystemLibrary.SystemLibrary.syn_visitor = stv;
            SetSemanticRules(SyntaxUnit);
            

            foreach (SyntaxTree.compiler_directive cd in SyntaxUnit.compiler_directives)
                cd.visit(stv);

            stv.DirectivesToNodesLinks = CompilerDirectivesToSyntaxTreeNodesLinker.BuildLinks(SyntaxUnit, ErrorsList);  //MikhailoMMX добавил передачу списка ошибок (02.10.10)

            SyntaxUnit.visit(stv);
            
            CompiledVariables.AddRange(stv.CompiledVariables);
            /*SyntaxTree.program_module pmod=SyntaxUnit as SyntaxTree.program_module;
			if (pmod!=null)
			{
				stv.visit(pmod);
			}
			else
			{
				SyntaxTree.unit_module umod=SyntaxUnit as SyntaxTree.unit_module;
				if (umod==null)
				{
					throw new PascalABCCompiler.TreeConverter.CompilerInternalError("Undefined module type (not program and not unit)");
				}
				stv.visit(umod);
			}*/
            //stv.visit(SyntaxUnit);
            //if (ErrorsList.Count>0) throw ErrorsList[0];
            return stv.compiled_unit;
		}

        public void CompileImplementation(PascalABCCompiler.TreeRealization.common_unit_node SemanticUnit,
			SyntaxTree.compilation_unit SyntaxUnit,PascalABCCompiler.TreeRealization.unit_node_list UsedUnits,List<Errors.Error> ErrorsList,List<Errors.CompilerWarning> WarningsList,
            PascalABCCompiler.Errors.SyntaxError parser_error, System.Collections.Hashtable bad_nodes, TreeRealization.using_namespace_list interface_namespaces, TreeRealization.using_namespace_list imlementation_namespaces,
           Dictionary<SyntaxTree.syntax_tree_node,string> docs, bool debug, bool debugging, bool for_intellisense, List<TreeRealization.var_definition_node> CompiledVariables)
		{
            stv = stvController.SelectVisitor(Path.GetExtension(SyntaxUnit.file_name));
            //if (ErrorsList.Count>0) throw ErrorsList[0];
            stv.parser_error=parser_error;
            stv.bad_nodes_in_syntax_tree = bad_nodes;
            stv.referenced_units = UsedUnits;

            stv.using_list.Clear();
            stv.using_list.AddRange(interface_namespaces);
            stv.interface_using_list.AddRange(interface_namespaces);
            stv.using_list.AddRange(imlementation_namespaces);
            stv.ErrorsList = ErrorsList;
            stv.WarningsList = WarningsList;
            stv.SymbolTable.CaseSensitive = SemanticRules.SymbolTableCaseSensitive;
            if (docs != null)
            stv.docs = docs;
            stv.debug = debug;
            stv.debugging = debugging;
            stv.for_intellisense = for_intellisense;
            SystemLibrary.SystemLibrary.syn_visitor = stv;
            SetSemanticRules(SyntaxUnit);

			SyntaxTree.unit_module umod = SyntaxUnit as SyntaxTree.unit_module;
			if (umod==null)
			{
                throw new PascalABCCompiler.TreeConverter.CompilerInternalError("Program has not implementation part");
			}

			stv.compiled_unit = SemanticUnit;
            stv.current_document = new TreeRealization.document(SyntaxUnit.file_name);

            foreach (SyntaxTree.compiler_directive cd in umod.compiler_directives)
                cd.visit(stv);

			stv.visit_implementation(umod);
            CompiledVariables.AddRange(stv.CompiledVariables);
			//stv.visit(SyntaxUnit);
			//return stv.compiled_unit;
		}

		public void Reset()
		{
			stv.reset();
            //stv = new syntax_tree_visitor(); // SSM 14/07/13 - может, будет занимать больше памяти, зато все внутренние переменные будут чиститься
            //SystemLibrary.SystemLibrary.syn_visitor = stv;

		}
	}
}
