// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

/*************************************************************************
 *
 *Интерфейс конвертора синтаксического дерева в семантическое для Compiler   
 *   Зависит от Errors, SemanticTree, PascalABCCompiler.SyntaxTree
 *
 *************************************************************************/

using System.Collections.Generic;
using Languages.Facade;
using PascalABCCompiler.Errors;
using PascalABCCompiler.TreeConverter.TreeConversion;
using System.Linq;

namespace PascalABCCompiler.TreeConverter
{
    public class SyntaxTreeToSemanticTreeConverter
    {

        private ISyntaxToSemanticTreeConverter syntaxTreeVisitor;

        public SyntaxTreeToSemanticTreeConverter()
        {
            foreach (var language in LanguageProvider.Instance.Languages)
            {
                language.RefreshSyntaxTreeToSemanticTreeConverter();
                // пока что берем стандартный визитор вначале  EVA
                if (language.Name == StringConstants.pascalLanguageName)
                    syntaxTreeVisitor = language.SyntaxTreeToSemanticTreeConverter;
            }

            //(ssyy) запоминаем visitor 
            SystemLibrary.SystemLibrary.syn_visitor = syntaxTreeVisitor as syntax_tree_visitor;
        }

        //TODO: Разобраться, где использутеся.
        public SymbolTable.TreeConverterSymbolTable SymbolTable
        {
            get
            {
                return syntaxTreeVisitor.SymbolTable;
            }
        }

        //TODO: Исправить коллекцию модулей.
        public TreeRealization.common_unit_node CompileInterface(ILanguage language, SyntaxTree.compilation_unit SyntaxUnit,
            TreeRealization.unit_node_list UsedUnits, List<Error> ErrorsList, List<CompilerWarning> WarningsList, SyntaxError parser_error,
            System.Collections.Hashtable bad_nodes, PascalABCCompiler.TreeRealization.using_namespace_list namespaces, Dictionary<SyntaxTree.syntax_tree_node, string> docs, bool debug, bool debugging, bool for_intellisense, List<PascalABCCompiler.TreeRealization.var_definition_node> CompiledVariables)
        {
            syntaxTreeVisitor = language.SyntaxTreeToSemanticTreeConverter;

            syntaxTreeVisitor.InitializeForCompilingInterface(parser_error, bad_nodes, UsedUnits, namespaces, SyntaxUnit, ErrorsList, WarningsList, docs, debug, debugging, for_intellisense);

            language.SetSemanticRules();

            foreach (SyntaxTree.compiler_directive cd in SyntaxUnit.compiler_directives)
                syntaxTreeVisitor.ProcessNode(cd);

            (syntaxTreeVisitor as syntax_tree_visitor).DirectivesToNodesLinks = CompilerDirectivesToSyntaxTreeNodesLinker.BuildLinks(SyntaxUnit, ErrorsList);  //MikhailoMMX добавил передачу списка ошибок (02.10.10)

            syntaxTreeVisitor.ProcessNode(SyntaxUnit);

            CompiledVariables.AddRange(syntaxTreeVisitor.CompiledVariables);
            /*SyntaxTree.program_module pmod = SyntaxUnit as SyntaxTree.program_module;
            if (pmod != null)
            {
                syntaxTreeVisitor.visit(pmod);
            }
            else
            {
                SyntaxTree.unit_module umod = SyntaxUnit as SyntaxTree.unit_module;
                if (umod == null)
                {
                    throw new PascalABCCompiler.TreeConverter.CompilerInternalError("Undefined module type (not program and not unit)");
                }
                syntaxTreeVisitor.visit(umod);
            }*/
            //syntaxTreeVisitor.visit(SyntaxUnit);
            //if (ErrorsList.Count>0) throw ErrorsList[0];
            return syntaxTreeVisitor.CompiledUnit;
        }

        public void CompileImplementation(ILanguage language, TreeRealization.common_unit_node SemanticUnit,
            SyntaxTree.compilation_unit SyntaxUnit, TreeRealization.unit_node_list UsedUnits, List<Error> ErrorsList, List<CompilerWarning> WarningsList,
            SyntaxError parser_error, System.Collections.Hashtable bad_nodes, PascalABCCompiler.TreeRealization.using_namespace_list interface_namespaces, TreeRealization.using_namespace_list imlementation_namespaces,
           Dictionary<SyntaxTree.syntax_tree_node, string> docs, bool debug, bool debugging, bool for_intellisense, List<TreeRealization.var_definition_node> CompiledVariables)
        {
            //if (ErrorsList.Count>0) throw ErrorsList[0];

            syntaxTreeVisitor = language.SyntaxTreeToSemanticTreeConverter;

            syntaxTreeVisitor.InitializeForCompilingImplementation(parser_error, bad_nodes, UsedUnits, interface_namespaces, imlementation_namespaces, SyntaxUnit, SemanticUnit, ErrorsList, WarningsList, docs, debug, debugging, for_intellisense);

            language.SetSemanticRules();

            foreach (SyntaxTree.compiler_directive cd in SyntaxUnit.compiler_directives)
                cd.visit(syntaxTreeVisitor);

            (syntaxTreeVisitor as syntax_tree_visitor).visit_implementation(SyntaxUnit as SyntaxTree.unit_module);
            CompiledVariables.AddRange(syntaxTreeVisitor.CompiledVariables);
            //stv.visit(SyntaxUnit);
            //return stv.compiled_unit;
        }

        public void Reset()
        {
            foreach (var syntaxToSemanticTreeConverter in LanguageProvider.Instance.Languages.Select(language => language.SyntaxTreeToSemanticTreeConverter))
            {
                syntaxToSemanticTreeConverter.Reset();
            }
            //stv = new syntax_tree_visitor(); // SSM 14/07/13 - может, будет занимать больше памяти, зато все внутренние переменные будут чиститься
            //SystemLibrary.SystemLibrary.syn_visitor = stv;

        }
    }
}
