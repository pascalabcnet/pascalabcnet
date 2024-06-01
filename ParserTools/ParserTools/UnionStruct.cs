// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.ParserTools
{
    /// <summary>
    /// Структура данных, хранящая узел синтаксического дерева в одном из полей.
    /// Используется GPPG парсером.
    /// </summary>
    public class Union
    {
        public expression ex;
        public ident id;
        public object ob;
        public op_type_node op;
        public syntax_tree_node stn;
        public token_info ti;
        public type_definition td;
    }
}
