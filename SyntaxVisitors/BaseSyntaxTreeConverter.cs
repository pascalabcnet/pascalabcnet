// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;

namespace PascalABCCompiler.SyntaxTreeConverters
{

    public abstract class BaseSyntaxTreeConverter : ISyntaxTreeConverter
    {
        public abstract string Name { get; }

        protected abstract IPipelineVisitor[] VisitorsForConvert { get; }

        protected virtual IPipelineVisitor[] VisitorsForConvertAfterUsedModulesCompilation => new IPipelineVisitor[0];

        public syntax_tree_node Convert(syntax_tree_node root, bool forIntellisense)
        {
            // Прошивание ссылками на Parent nodes. Должно идти первым
            // FillParentNodeVisitor расположен в SyntaxTree/tree как базовый визитор, отвечающий за построение дерева
            //FillParentNodeVisitor.New.ProcessNode(root); // почему-то перепрошивает не всё. А следующий вызов - всё
            root.FillParentsInAllChilds();

            return ApplyConversions(root, forIntellisense);
        }

        private syntax_tree_node ApplyConversions(syntax_tree_node root, bool forIntellisense)
        {
            var context = new VisitorsContext();

            context.Set("forIntellisense", forIntellisense);

            int i = 0;

            if (forIntellisense)
            {
                PipelineSafe(VisitorsForConvert, context, root, i);
            }
            else
            {
                Pipeline(VisitorsForConvert, context, root, i);
            }

            return root;
        }

        public syntax_tree_node ConvertAfterUsedModulesCompilation(syntax_tree_node root, bool forIntellisense, in CompilationArtifactsUsedBySyntaxConverters compilationArtifacts) 
        {
            var context = new VisitorsContext();

            context.Set("forIntellisense", forIntellisense);

            context.Set("namesFromUsedUnits", compilationArtifacts.NamesFromUsedUnits);

            int i = 0;

            if (forIntellisense)
            {
                PipelineSafe(VisitorsForConvertAfterUsedModulesCompilation, context, root, i);
            }
            else
            {
                Pipeline(VisitorsForConvertAfterUsedModulesCompilation, context, root, i);
            }

            return root;
        }

        private void Pipeline(IPipelineVisitor[] visitors, VisitorsContext context, syntax_tree_node root, int index)
        {
            if (index == visitors.Length)
                return;

            visitors[index].Visit(root, context, () => Pipeline(visitors, context, root, index + 1));
        }

        private void PipelineSafe(IPipelineVisitor[] visitors, VisitorsContext context, syntax_tree_node root, int index)
        {
            if (index == visitors.Length)
                return;

            try
            {
                visitors[index].Visit(root, context, () => PipelineSafe(visitors, context, root, index + 1));
            }
            catch (Exception)
            {
                // Продолжаем пробовать обходить
                PipelineSafe(visitors, context, root, index + 1);
            }
        }
    }

    public interface IPipelineVisitor
    {
        void Visit(syntax_tree_node root, VisitorsContext context, Action next);
    }

    public class VisitorsContext
    {
        public Dictionary<string, object> data = new Dictionary<string, object>();

        public void Set(string key, object value) => data[key] = value;
        public T Get<T>(string key) => (T)data[key];
    }


    public class DefaultSyntaxTreeConverter : BaseSyntaxTreeConverter
    {
        public override string Name => "Default";

        protected override IPipelineVisitor[] VisitorsForConvert => new IPipelineVisitor[0];
    }

    
    public class DefaultSyntaxTreeConverter : BaseSyntaxTreeConverter
    {
        public override string Name => "Default";

        protected override syntax_tree_node ApplyConcreteConversions(syntax_tree_node root) => root;
    }
}
