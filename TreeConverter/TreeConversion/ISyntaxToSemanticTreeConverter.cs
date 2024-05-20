using PascalABCCompiler.SyntaxTree;
using System.Collections.Generic;

namespace PascalABCCompiler.TreeConverter.TreeConversion
{
    /// <summary>
    /// Интерфейс преобразователя синтаксического дерева в семантическое
    /// </summary>
    public interface ISyntaxToSemanticTreeConverter : IVisitor
    {
        /// <summary>
        /// Метод инициализации конвертера перед компиляцией секции интерфейса (модуля)
        /// </summary>
        /// <param name="initializationData"></param>
        void InitializeForCompilingInterface(InitializationDataForCompilingInterface initializationData);

        /// <summary>
        /// Метод инициализации конвертера перед компиляцией секции реализации (модуля)
        /// </summary>
        /// <param name="initializationData"></param>
        void InitializeForCompilingImplementation(InitializationDataForCompilingImplementation initializationData);

        // требуется другая архитектура, чтобы этого не было здесь  EVA
        void ProcessNode<T>(T cd) where T : syntax_tree_node;

        List<TreeRealization.var_definition_node> CompiledVariables { get; }

        TreeRealization.common_unit_node CompiledUnit { get; }

        SymbolTable.TreeConverterSymbolTable SymbolTable { get; }

        /// <summary>
        /// Метод для очистки переменных перед новым запуском компиляции
        /// </summary>
        void Reset();
    }
}
