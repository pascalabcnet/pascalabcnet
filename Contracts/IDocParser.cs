using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.Contracts
{
    /// <summary>
    /// Интерфейс парсера документирующих комментариев (///)
    /// </summary>
    public interface IDocParser
    {
        /// <summary>
        /// Метод построения синтаксического дерева документирующих комментариев
        /// </summary>
        /// <param name="Text">Текст программы</param>
        /// <returns></returns>
        documentation_comment_list BuildTree(string Text);
    }
}
