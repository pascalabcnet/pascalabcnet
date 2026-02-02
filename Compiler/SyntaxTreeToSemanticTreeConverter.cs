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
using PascalABCCompiler.TreeConverter.TreeConversion;
using System.Linq;

namespace PascalABCCompiler.TreeConverter
{
    public class SyntaxTreeToSemanticTreeConverter
    {

        private syntax_tree_visitor syntaxTreeVisitor;

        public SyntaxTreeToSemanticTreeConverter()
        {

            foreach (var language in LanguageProvider.Instance.Languages)
            {
                language.SetSyntaxTreeToSemanticTreeConverter();
            }

            syntaxTreeVisitor = LanguageProvider.Instance.MainLanguage.SyntaxTreeToSemanticTreeConverter;

            //(ssyy) запоминаем visitor 
            SystemLibrary.SystemLibrary.syn_visitor = syntaxTreeVisitor;
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
        public TreeRealization.common_unit_node CompileInterface(ILanguage language, InitializationDataForCompilingInterface initializationData, List<TreeRealization.var_definition_node> CompiledVariables)
        {
            syntaxTreeVisitor = language.SyntaxTreeToSemanticTreeConverter;

            syntaxTreeVisitor.InitializeForCompilingInterface(initializationData);

            language.SetSemanticConstants();

            foreach (SyntaxTree.compiler_directive cd in initializationData.syntaxUnit.compiler_directives)
                syntaxTreeVisitor.ProcessNode(cd);

            syntaxTreeVisitor.DirectivesToNodesLinks = CompilerDirectivesToSyntaxTreeNodesLinker.BuildLinks(initializationData.syntaxUnit, initializationData.errorsList);  //MikhailoMMX добавил передачу списка ошибок (02.10.10)

            syntaxTreeVisitor.BeforeCompilationActions();

            syntaxTreeVisitor.ProcessNode(initializationData.syntaxUnit);

            syntaxTreeVisitor.PostCompilationActions();

            CompiledVariables.AddRange(syntaxTreeVisitor.CompiledVariables);

            return syntaxTreeVisitor.CompiledUnit;
        }

        public void CompileImplementation(ILanguage language, InitializationDataForCompilingImplementation initializationData, List<TreeRealization.var_definition_node> CompiledVariables)
        {
            syntaxTreeVisitor = language.SyntaxTreeToSemanticTreeConverter;

            syntaxTreeVisitor.InitializeForCompilingImplementation(initializationData);

            language.SetSemanticConstants();

            foreach (SyntaxTree.compiler_directive cd in initializationData.syntaxUnit.compiler_directives)
                cd.visit(syntaxTreeVisitor);

            syntaxTreeVisitor.BeforeCompilationActions();

            syntaxTreeVisitor.visit_implementation(initializationData.syntaxUnit as SyntaxTree.unit_module);

            syntaxTreeVisitor.PostCompilationActions();

            CompiledVariables.AddRange(syntaxTreeVisitor.CompiledVariables);
        }

        public void Reset()
        {
            foreach (var syntaxToSemanticTreeConverter in LanguageProvider.Instance.Languages.Select(language => language.SyntaxTreeToSemanticTreeConverter))
            {
                syntaxToSemanticTreeConverter.Reset();
            }

        }
    }
}
