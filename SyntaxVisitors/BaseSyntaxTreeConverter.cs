// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.SyntaxTreeConverters
{

    public abstract class BaseSyntaxTreeConverter : ISyntaxTreeConverter
    {
        public abstract string Name { get; }

        public syntax_tree_node Convert(syntax_tree_node root)
        {
            // Прошивание ссылками на Parent nodes. Должно идти первым
            // FillParentNodeVisitor расположен в SyntaxTree/tree как базовый визитор, отвечающий за построение дерева
            //FillParentNodeVisitor.New.ProcessNode(root); // почему-то перепрошивает не всё. А следующий вызов - всё
            root.FillParentsInAllChilds();

            return ApplyConversions(root);
        }

        protected abstract syntax_tree_node ApplyConversions(syntax_tree_node root);
    }
}
