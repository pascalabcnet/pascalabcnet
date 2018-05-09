using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var types = InferAndCheckPatternVariableTypes(deconstruction.definitions, invokationTarget, deconstruction);
            if (types == null)
                return;

            foreach (var definition in deconstruction.WithTypes(types.Select(x => new semantic_type_node(x)).ToArray()))
                definition.visit(this);
        }

        private type_node[] InferAndCheckPatternVariableTypes(List<var_def_statement> variableDefinitions, expression_node targetExpression, desugared_deconstruction deconstruction)
        {
            /*
            var castType = targetExpression.type as common_type_node;
            var candidates = castType.methods.Where(x => x.name.ToLower() == "deconstruct").ToList();
            
            var deconstructParametersCount = variableDefinitions.Count;
            parameter_list deconstructionParameters = null;

            int suitableCandidates = 0;
            foreach (var candidate in candidates)
            {
                if (candidate.parameters.Count != deconstructParametersCount)
                    continue;

                
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
            */

            var parameterTypes = variableDefinitions.Select(x => x.vars_type == null ? null : convert_strong(x.vars_type)).ToArray();
            List<function_node> candidates = new List<function_node>();

            var allDeconstructs = targetExpression.type.find_in_type("deconstruct", context.CurrentScope);
            foreach (var canditateSymbol in allDeconstructs.list)
            {
                var possibleCandidate = canditateSymbol.sym_info as function_node;
                if (!IsSuitableFunction(possibleCandidate, parameterTypes))
                    continue;

                candidates.Add(possibleCandidate);
            }

            if (candidates.Count == 0)
            {
                AddError(get_location(deconstruction), "NO_SUITABLE_DECONSTRUCT_FOUND");
                return null;
            }
            else
            if (candidates.Count > 1)
            {
                RemoveDefaultDeconstruct(candidates);
                if (candidates.Count > 1)
                {
                    AddError(get_location(deconstruction), "DECONSTRUCTOR_METHOD_AMBIGUITY");
                    return null;
                }
            }

            // Единственный подхдящий кандидат найден
            Debug.Assert(candidates.Count == 1, "Candidate ambiguity");
            var chosenDeconstruct = candidates[0];
            type_node[] deducedGenerics = new type_node[chosenDeconstruct.generic_parameters_count];
            var nils = new List<int>();

            if (chosenDeconstruct.is_generic_function && chosenDeconstruct.is_extension_method)
            {
                var deduceSucceded = generic_convertions.DeduceInstanceTypes(chosenDeconstruct.parameters[0].type, targetExpression.type, deducedGenerics, nils);
                if (!deduceSucceded || deducedGenerics.Contains(null))
                {
                    AddError(get_location(deconstruction), "COULDNT_DECUCE_DECONSTRUCT_GENERIC_TYPE");
                    return null;
                }
            }

            return GetVariableTypesFromDestructionCall(chosenDeconstruct, deducedGenerics);
        }

        /// <summary>
        /// Проверяет, подходит ли фаункция для вызова с указанными параметрами
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="parameterTypes">Типы параметров, указанные пользователем</param>
        /// <returns></returns>
        private bool IsSuitableFunction(function_node candidate, type_node[] parameterTypes)
        {
            var candidateParameterTypes =
                candidate.is_extension_method ?
                candidate.parameters.Where(x => !IsSelfParameter(x)).ToArray() :
                candidate.parameters.ToArray();

            if (candidateParameterTypes.Length != parameterTypes.Length)
                return false;

            for (int i = 0; i < parameterTypes.Length; i++)
            {
                if (parameterTypes[i] != null && !AreTheSameTypes(candidateParameterTypes[i].type, parameterTypes[i]))
                    return false;
            }

            return true;
        }

        private bool AreTheSameTypes(type_node type1, type_node type2)
        {
            return type1.full_name == type2.full_name;
        }

        private bool IsSelfParameter(parameter parameter) => parameter.name.ToLower() == compiler_string_consts.self_word;

        private void RemoveDefaultDeconstruct(List<function_node> candidates)
        {
            var index = candidates.FindIndex(IsDefaultDeconstruct);
            if (index >= 0)
                candidates.RemoveAt(index);
        }

        private bool  IsDefaultDeconstruct(function_node function)
        {
            return
                function.generic_parameters_count == 1 &&
                function.parameters.Count == 2 &&
                function.parameters[0].type.is_generic_parameter &&
                function.parameters[1].type.is_generic_parameter &&
                AreTheSameTypes(function.parameters[0].type, function.parameters[1].type);
        }

        private type_node[] GetVariableTypesFromDestructionCall(function_node deconstruct, type_node[] generics)
        {
            return deconstruct.parameters
                .Where(parameter => !IsSelfParameter(parameter))
                .Select(x => x.type.is_generic_parameter ? generics[x.type.generic_param_index] : x.type)
                .ToArray();
        }
    }
}
