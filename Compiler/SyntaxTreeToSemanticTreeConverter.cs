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

        private ISyntaxToSemanticTreeConverter syntaxTreeVisitor;

        public SyntaxTreeToSemanticTreeConverter()
        {
            foreach (var language in LanguageProvider.Instance.Languages)
            {
                language.SetSyntaxTreeToSemanticTreeConverter();
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
        public TreeRealization.common_unit_node CompileInterface(ILanguage language, InitializationDataForCompilingInterface initializationData, List<TreeRealization.var_definition_node> CompiledVariables)
        {
            syntaxTreeVisitor = language.SyntaxTreeToSemanticTreeConverter;

            syntaxTreeVisitor.InitializeForCompilingInterface(initializationData);

            language.SetSemanticConstants();

            foreach (SyntaxTree.compiler_directive cd in initializationData.syntaxUnit.compiler_directives)
                syntaxTreeVisitor.ProcessNode(cd);

            // TODO: избавиться от преобразования типа  EVA
            (syntaxTreeVisitor as syntax_tree_visitor).DirectivesToNodesLinks = CompilerDirectivesToSyntaxTreeNodesLinker.BuildLinks(initializationData.syntaxUnit, initializationData.errorsList);  //MikhailoMMX добавил передачу списка ошибок (02.10.10)

            syntaxTreeVisitor.ProcessNode(initializationData.syntaxUnit);

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

        public void CompileImplementation(ILanguage language, InitializationDataForCompilingImplementation initializationData, List<TreeRealization.var_definition_node> CompiledVariables)
        {
            //if (ErrorsList.Count>0) throw ErrorsList[0];

            syntaxTreeVisitor = language.SyntaxTreeToSemanticTreeConverter;

            syntaxTreeVisitor.InitializeForCompilingImplementation(initializationData);

            language.SetSemanticConstants();

            foreach (SyntaxTree.compiler_directive cd in initializationData.syntaxUnit.compiler_directives)
                cd.visit(syntaxTreeVisitor);

            // TODO: избавиться от преобразования типа  EVA
            (syntaxTreeVisitor as syntax_tree_visitor).visit_implementation(initializationData.syntaxUnit as SyntaxTree.unit_module);
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
