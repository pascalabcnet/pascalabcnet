using System;

namespace PascalABCCompiler.Contracts
{
    /// <summary>
    /// Этап, которому принадлежит директива. Это не обработчик директивы, а
    /// явная граница ответственности между лексером, компиляцией модуля,
    /// сборкой итоговой программы и редактором.
    /// </summary>
    [Flags]
    public enum DirectiveProcessingStage
    {
        Lexer = 1,
        Unit = 2,
        Compilation = 4,
        Editor = 8
    }
}
