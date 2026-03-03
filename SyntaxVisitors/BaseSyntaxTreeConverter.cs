// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.SyntaxTreeConverters
{

    public abstract class BaseSyntaxTreeConverter : ISyntaxTreeConverter
    {
        public abstract string Name { get; }

        public syntax_tree_node Convert(syntax_tree_node root, bool forIntellisense)
        {
            // очистка счетчиков сгенерированных переменных
            CoreUtils.GeneratedNamesManager.Reset();

            // Прошивание ссылками на Parent nodes. Должно идти первым
            // FillParentNodeVisitor расположен в SyntaxTree/tree как базовый визитор, отвечающий за построение дерева
            //FillParentNodeVisitor.New.ProcessNode(root); // почему-то перепрошивает не всё. А следующий вызов - всё
            root.FillParentsInAllChilds();

            return ApplyConversions(root, forIntellisense);
        }

        protected abstract syntax_tree_node ApplyConversions(syntax_tree_node root, bool forIntellisense);

        public syntax_tree_node ConvertAfterUsedModulesCompilation(syntax_tree_node root, bool forIntellisense, in CompilationArtifactsUsedBySyntaxConverters compilationArtifacts) {

            // очистка счетчиков сгенерированных переменных
            CoreUtils.GeneratedNamesManager.Reset();

            return ApplyConversionsAfterUsedModulesCompilation(root, forIntellisense, in compilationArtifacts);
        }

        protected virtual syntax_tree_node ApplyConversionsAfterUsedModulesCompilation(syntax_tree_node root, bool forIntellisense, in CompilationArtifactsUsedBySyntaxConverters compilationArtifacts) => root;
    }


    public class DefaultSyntaxTreeConverter : BaseSyntaxTreeConverter
    {
        public override string Name => "Default";

        protected override syntax_tree_node ApplyConversions(syntax_tree_node root, bool forIntellisense) => root;
    }
}
