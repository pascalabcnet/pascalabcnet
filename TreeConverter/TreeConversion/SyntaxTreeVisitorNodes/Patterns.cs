using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.TreeConverter
{
    public partial class syntax_tree_visitor
    {
        private void CheckWhenExpression(SyntaxTree.expression when)
        {
            if (when == null)
                return;

            var convertedWhen = convert_strong(when);
            if (!convertion_data_and_alghoritms.can_convert_type(convertedWhen, SystemLibrary.SystemLibrary.bool_type))
                AddError(get_location(when), "WHEN_EXPRESSION_SHOULD_HAVE_BOOL_TYPE");
        }

        public override void visit(desugared_deconstruction deconstruction)
        {
            if (deconstruction.HasAllExplicitTypes)
            {
                foreach (var def in deconstruction.definitions.Select(x => new var_statement(x)))
                    def.visit(this);
                return;
            }

            var invokationTarget = convert_strong(deconstruction.deconstruction_target as expression);
            var types = InferPatternVariableType(deconstruction.definitions, invokationTarget, deconstruction);
            if (types == null)
                return;

            foreach (var definition in deconstruction.WithTypes(types.Select(x => new semantic_type_node(x)).ToArray()))
                definition.visit(this);
        }

        private type_node[] InferPatternVariableType(List<var_def_statement> variableDefinitions, expression_node targetExpression, desugared_deconstruction deconstruction)
        {
            var castType = targetExpression.type as common_type_node;
            var candidates = castType.methods.Where(x => x.name.ToLower() == "deconstruct").ToList();
            var deconstructParametersCount = variableDefinitions.Count;
            parameter_list deconstructionParameters = null;

            int suitableCandidates = 0;
            foreach (var candidate in candidates)
            {
                if (candidate.parameters.Count != deconstructParametersCount)
                    continue;

                deconstructionParameters = candidate.parameters;
                suitableCandidates++;
            }

            type_node[] result = null;
            if (suitableCandidates == 0)
            {
                if (deconstructParametersCount > 1)
                    AddError(get_location(deconstruction), "NO_SUITABLE_DECONSTRUCT_FOUND");
                else
                    result = new[] { targetExpression.type };
            }
            else
            {
                if (suitableCandidates > 1)
                    AddError(get_location(deconstruction), "DECONSTRUCTOR_METHOD_AMBIGUITY");
                else
                    result = deconstructionParameters.Select(x => x.type).ToArray();
            }
            return result;
        }
    }
}
