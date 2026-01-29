// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.Parsers
{
    /// <summary>
    /// Базовый класс для парсера без поддержки построения частей дерева и построения дерева с пропуском ошибок (Special mode)
    /// </summary>
    public abstract class SimpleParser : BaseParser
    {
        protected override syntax_tree_node BuildTreeInExprMode(string FileName, string Text)
        {
            throw new System.NotImplementedException();
        }

        protected override syntax_tree_node BuildTreeInFormatterMode(string FileName, string Text)
        {
            throw new System.NotImplementedException();
        }

        protected override syntax_tree_node BuildTreeInSpecialMode(string FileName, string Text, bool compilingNotMainProgram)
        {
            throw new System.NotImplementedException();
        }

        protected override syntax_tree_node BuildTreeInStatementMode(string FileName, string Text)
        {
            throw new System.NotImplementedException();
        }

        protected override syntax_tree_node BuildTreeInTypeExprMode(string FileName, string Text)
        {
            throw new System.NotImplementedException();
        }
    }
}
