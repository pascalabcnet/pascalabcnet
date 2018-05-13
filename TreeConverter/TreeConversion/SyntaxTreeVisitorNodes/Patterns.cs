using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.TreeConverter
{
    // Patterns
    public partial class syntax_tree_visitor
    {
        public override void visit(desugared_deconstruction deconstruction)
        {
            var invokationTarget = convert_strong(deconstruction.deconstruction_target as expression);
            var types = InferAndCheckPatternVariableTypes(deconstruction.definitions, invokationTarget, deconstruction);
            if (types == null)
                return;

            foreach (var definition in deconstruction.WithTypes(types.Select(x => new semantic_type_node(x)).ToArray()))
                definition.visit(this);
        }

        private type_node[] InferAndCheckPatternVariableTypes(List<var_def_statement> variableDefinitions, expression_node patternInstance, desugared_deconstruction deconstruction)
        {
            var parameterTypes = variableDefinitions.Select(x => x.vars_type == null ? null : convert_strong(x.vars_type)).ToArray();
            List<function_node> candidates = new List<function_node>();
            List<type_node[]> deducedParametersList = new List<type_node[]>();

            var allDeconstructs = patternInstance.type.find_in_type(compiler_string_consts.deconstruct_method_name, context.CurrentScope);
            foreach (var canditateSymbol in allDeconstructs)
            {
                var deducedParameters = new type_node[parameterTypes.Length];
                var possibleCandidate = canditateSymbol.sym_info as function_node;
                if (!IsSuitableFunction(possibleCandidate, parameterTypes, patternInstance, get_location(deconstruction), out deducedParameters))
                    continue;

                deducedParametersList.Add(deducedParameters);
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
                if (candidates.Count > 1 && !CheckIfParameterListElementsAreTheSame(deducedParametersList))
                {
                    AddError(get_location(deconstruction), "DECONSTRUCTOR_METHOD_AMBIGUITY");
                    return null;
                }
            }

            // Единственный подхдящий кандидат найден, либо их несколько, с одинаковыми выходными параметрами
            var chosenFunction = candidates.First();
            if (chosenFunction.is_extension_method)
            {
                if (chosenFunction.is_generic_function)
                    chosenFunction = generic_convertions.get_function_instance(chosenFunction, deducedParametersList.First().ToList());

                return chosenFunction.parameters.Where(x => !IsSelfParameter(x)).Select(x => x.type).ToArray();
            }
            else
                return chosenFunction.parameters.Select(x => x.type).ToArray();//deducedParametersList[0];
        }

        /// <summary>
        /// Проверяет, подходит ли фаункция для вызова с указанными параметрами
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="givenParameterTypes">Типы параметров, указанные пользователем</param>
        /// <returns></returns>
        private bool IsSuitableFunction(
            function_node candidate,
            type_node[] givenParameterTypes,
            expression_node patternInstance,
            location deconstructionLocation,
            out type_node[] parameterTypes)
        {
            parameterTypes = new type_node[givenParameterTypes.Length];
            var selfParameter = candidate.is_extension_method ? candidate.parameters.First(IsSelfParameter) : null;
            Debug.Assert(!candidate.is_extension_method || selfParameter != null, "Couldn't find self parameter in extension method");
            var candidateParameterTypes =
                candidate.is_extension_method ?
                candidate.parameters.Where(x => !IsSelfParameter(x)).ToArray() :
                candidate.parameters.ToArray();

            if (candidateParameterTypes.Length != givenParameterTypes.Length)
                return false;

            var genericDeduceNeeded = candidate.is_extension_method && candidate.is_generic_function;
            type_node[] deducedGenerics = new type_node[candidate.generic_parameters_count];
            if (genericDeduceNeeded)
            {
                // Выводим дженерики по self
                var nils = new List<int>();
                var deduceSucceded = generic_convertions.DeduceInstanceTypes(selfParameter.type, patternInstance.type, deducedGenerics, nils);
                if (!deduceSucceded || deducedGenerics.Contains(null))
                    // Проверка на то, что в Deconstruct все дженерики выводятся по self делается в другом месте
                    // TODO Patterns: сделать проверку
                    // TODO Patterns: запретить дженерик методы в классах. Можно использовать только дженерик-типы самого класса в качестве параметров
                    //AddError(deconstructionLocation, "COULDNT_DEDUCE_DECONSTRUCT_GENERIC_TYPE");
                    return false;
            }

            for (int i = 0; i < givenParameterTypes.Length; i++)
            {
                var givenParameter = givenParameterTypes[i];
                var candidateParameter = candidateParameterTypes[i].type;
                if (genericDeduceNeeded && (candidateParameter.is_generic_parameter || candidateParameter.is_generic_type_instance))
                    candidateParameter = InstantiateParameter(candidateParameter, deducedGenerics);

                if (givenParameter != null && !AreTheSameTypes(candidateParameter, givenParameter))
                    return false;

                parameterTypes[i] = candidateParameter;
            }

            return true;
        }

        private bool AreTheSameTypes(type_node type1, type_node type2)
        {
            return convertion_data_and_alghoritms.possible_equal_types(type1, type2);
        }

        private bool IsSelfParameter(parameter parameter) => parameter.name.ToLower() == compiler_string_consts.self_word;

        private void RemoveDefaultDeconstruct(List<function_node> candidates)
        {
            var index = candidates.FindIndex(IsDefaultDeconstruct);
            if (index >= 0)
                candidates.RemoveAt(index);
        }

        private bool IsDefaultDeconstruct(function_node function)
        {
            // TODO Patterns: check if it declared in PABCSystem
            return
                function.generic_parameters_count == 1 &&
                function.parameters.Count == 2 &&
                function.parameters[0].type.is_generic_parameter &&
                function.parameters[1].type.is_generic_parameter &&
                AreTheSameTypes(function.parameters[0].type, function.parameters[1].type);
        }

        private bool CheckIfParameterListElementsAreTheSame(List<type_node[]> parametersList)
        {
            var first = parametersList.First();
            for (int i = 1; i < parametersList.Count; i++)
            {
                if (first.Length != parametersList[i].Length)
                    return false;

                for (int j = 0; j < first.Length; j++)
                    if (!AreTheSameTypes(first[j], parametersList[i][j]))
                        return false;
            }

            return true;
        }

        private type_node InstantiateParameter(SemanticTree.ITypeNode genericType, type_node[] instances)
        {
            if (genericType.is_generic_parameter)
                return instances[(genericType as type_node).generic_param_index];
            else
            {
                var genericNode = (genericType as generic_instance_type_node);
                if (genericNode != null)
                {
                    var parameters = genericNode.instance_params.Select(x => InstantiateParameter(x, instances)).ToList();
                    genericNode.instance_params = parameters;
                }

                return genericNode as type_node;
            }
        }
    }
}
