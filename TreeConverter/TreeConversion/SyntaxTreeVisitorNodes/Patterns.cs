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
            var types = InferAndCheckPatternVariableTypes(deconstruction.variables.definitions, invokationTarget, deconstruction);
            if (types == null)
                return;

            foreach (var definition in deconstruction.WithTypes(types.Select(x => new semantic_type_node(x)).ToArray()))
                definition.visit(this);
        }

        private type_node[] InferAndCheckPatternVariableTypes(List<var_def_statement> variableDefinitions, expression_node patternInstance, desugared_deconstruction deconstruction)
        {
            if (patternInstance.type.IsPointer)
            {
                AddError(get_location(deconstruction), "PATTERN_MATCHING_DOESNT_SUPPORT_POINTERS");
                return null;
            }
            var parameterTypes = variableDefinitions.Select(x => x.vars_type == null ? null : convert_strong(x.vars_type)).ToArray();
            List<function_node> candidates = new List<function_node>();
            List<type_node[]> deducedParametersList = new List<type_node[]>();
            var allDeconstructs = patternInstance.type.find_in_type(StringConstants.deconstruct_method_name, context.CurrentScope);
            if (allDeconstructs == null)
            {
                AddError(get_location(deconstruction), "NO_DECONSTRUCT_FOUND");
                return null;
            }

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
        /// Проверяет, подходит ли функция для вызова с указанными параметрами
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
            if (candidate.parameters.Count > 0)
                candidate.parameters[0].name = "Self"; // SSM 23.06.20 #2268 - это если в NET где-то такое нашли
            var selfParameter = candidate.is_extension_method ? candidate.parameters.FirstOrDefault(IsSelfParameter) : null;
            Debug.Assert(!candidate.is_extension_method || selfParameter != null, "Couldn't find self parameter in extension method");
            var candidateParameterTypes =
                candidate.is_extension_method ?
                candidate.parameters.Where(x => !IsSelfParameter(x)).ToArray() :
                candidate.parameters.ToArray();

            if (candidateParameterTypes.Length != givenParameterTypes.Length)
                return false;

            // Разрешаем только deconstruct текущего класса, родительские в расчет не берем
            if (candidate is common_method_node commonMethod && !AreTheSameType(patternInstance.type, commonMethod.cont_type))
                return false;

            var genericDeduceNeeded = candidate.is_extension_method && candidate.is_generic_function;
            type_node[] deducedGenerics = new type_node[candidate.generic_parameters_count];
            if (genericDeduceNeeded)
            {
                // Выводим дженерики по self
                var nils = new List<int>();
                var deduceSucceded = generic_convertions.DeduceInstanceTypes(selfParameter.type, patternInstance.type, deducedGenerics, nils, candidate.get_generic_params_list());
                if (!deduceSucceded || deducedGenerics.Contains(null))
                    return false;
            }

            for (int i = 0; i < givenParameterTypes.Length; i++)
            {
                var givenParameter = givenParameterTypes[i];
                var candidateParameter = candidateParameterTypes[i].type;
                if (genericDeduceNeeded && (candidateParameter.is_generic_parameter || candidateParameter.is_generic_type_instance))
                    candidateParameter = InstantiateParameter(candidateParameter, deducedGenerics);

                if (givenParameter != null && !AreTheSameType(candidateParameter, givenParameter)
                    && !(candidateParameter != null && givenParameter != null && candidateParameter.is_generic_parameter 
                    && givenParameter.is_generic_parameter && candidateParameter.name == givenParameter.name && givenParameter.base_type == candidateParameter.base_type))
                    return false;

                parameterTypes[i] = candidateParameter;
            }

            return true;
        }

        private type_node GetDeconstructorOwner(function_node deconstructor)
        {
            /*switch (deconstructor)
            {
                case common_method_node commonMethod: return commonMethod.cont_type;
                case common_namespace_function_node commonNamespaseFunction: return GetSelfParameter(commonNamespaseFunction).type;
                default: return null;
            }*/
            if (deconstructor is common_method_node commonMethod)
                return commonMethod.cont_type;
            if (deconstructor is common_namespace_function_node commonNamespaseFunction)
                return GetSelfParameter(commonNamespaseFunction).type;
            return null;
        }

        private bool AreTheSameType(type_node type1, type_node type2) =>
            type1 != null &&
            type2 != null &&
            convertion_data_and_alghoritms.possible_equal_types(type1, type2);

        private bool IsSelfParameter(parameter parameter) => parameter.name.ToLower() == StringConstants.self_word;

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
                AreTheSameType(function.parameters[0].type, function.parameters[1].type);
        }

        private bool CheckIfParameterListElementsAreTheSame(List<type_node[]> parametersList)
        {
            var first = parametersList.First();
            for (int i = 1; i < parametersList.Count; i++)
            {
                if (first.Length != parametersList[i].Length)
                    return false;

                for (int j = 0; j < first.Length; j++)
                    if (!AreTheSameType(first[j], parametersList[i][j]))
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

        private IEnumerable<int> GenericParameterIndices(SemanticTree.ITypeNode node)
        {
            if (node.is_generic_parameter)
                yield return (node as type_node).generic_param_index;

            if (node is generic_instance_type_node genericNode)
                foreach (var index in genericNode.instance_params.Aggregate(
                    Enumerable.Empty<int>(),
                    (indices, nextNode) => indices.Concat(GenericParameterIndices(nextNode)))
                    .Distinct())
                    yield return index;

            yield break;
        }

        private parameter GetSelfParameter(function_node node) => node.parameters.FirstOrDefault(x => IsSelfParameter(x));

        /// <summary>
        /// Проводит проверки, общие для экземплярных деконструкторов и расширений
        /// </summary>
        /// <param name="deconstructor"></param>
        private void ExecuteCommonChecks(common_function_node deconstructor)
        {
            var dd = deconstructor as common_method_node;
            if (dd != null && dd.IsStatic)
                AddError(deconstructor.loc, "DECONSTRUCTOR_SHOULD_NOT_BE_STATIC");
            if (deconstructor.return_value_type != null)
                AddError(deconstructor.loc, "DECONSTRUCTOR_SHOULD_BE_A_PROCEDURE");

            foreach (var parameter in deconstructor.parameters.Where(x => !IsSelfParameter(x)))
                if (parameter.parameter_type != SemanticTree.parameter_type.var && parameter is common_parameter p)
                    AddError(p.loc, "DECONSTRUCTION_PARAMETERS_SHOULD_HAVE_VAR_MODIFIER");
        }

        /// <summary>
        /// Проверяет выражение, сопоставляемое с образцом
        /// </summary>
        /// <param name="matchedExpression"></param>
        private void CheckMatchedExpression(expression matchedExpression)
        {
            var convertedExpression = convert_strong(matchedExpression);
            if (convertedExpression.type.IsPointer)
                AddError(get_location(matchedExpression), "PATTERN_MATCHING_DOESNT_SUPPORT_POINTERS");
        }

        private void CheckIfCanBeMatched(expression matchedExpression, type_definition targetType)
        {
            var type = convert_strong(targetType);
            var expression = convert_strong(matchedExpression);
            if (expression is typed_expression)
                try_convert_typed_expression_to_function_call(ref expression);

            var expressionType = expression.type;
            if (type_table.is_derived(type, expressionType) ||
                type_table.is_derived(expressionType, type) ||
                AreTheSameType(type, expressionType) ||
                type.IsInterface ||
                expressionType.IsInterface || type.is_generic_parameter || expressionType.is_generic_parameter)
                return;

            AddError(get_location(matchedExpression), "EXPRESSION_OF_TYPE_{0}_CANNOT_BE_MATCHED_AGAINST_PATTERN_WITH_TYPE_{1}", expressionType.name, type.name);
        }

        private void CheckIfCanBeMatched(expression matchedExpression, expression patternExpression)
        {
            var patternNode = convert_strong(patternExpression);
            var patternType = patternNode.type;
            var expressionNode = convert_strong(matchedExpression);
            if (expressionNode is typed_expression)
                try_convert_typed_expression_to_function_call(ref expressionNode);
            var expressionType = expressionNode.type;

            if (!(patternNode is constant_node))
            {
                AddError(get_location(patternExpression), "MATCHING_WITH_NON_CONST");
            }

            var possible_convertion = type_table.get_convertions(patternType, expressionType);
            var expressionTypeName = expressionType.BaseFullName;
            var patternTypeName = patternType.BaseFullName;
            var tupleName = "System.Tuple";
            if (type_table.is_derived(patternType, expressionType) ||
                type_table.is_derived(expressionType, patternType) ||
                possible_convertion.first != null ||
                AreTheSameType(patternType, expressionType) ||
                (expressionTypeName.StartsWith(tupleName) &&
                patternTypeName.StartsWith(tupleName) &&
                int.Parse(expressionTypeName.Substring(tupleName.Length + 1, 1)) ==
                int.Parse(patternTypeName.Substring(tupleName.Length + 1, 1))) ||
                (expressionType.IsPointer && patternNode is null_const_node) ||
                (patternType.IsPointer && expressionNode is null_const_node))
                return;

            AddError(get_location(matchedExpression), "EXPRESSION_OF_TYPE_{0}_CANNOT_BE_MATCHED_AGAINST_PATTERN_WITH_TYPE_{1}", expressionType.name, patternType.name);
        }

        private void CheckIfCanBeMatched(expression tuple, int32_const length)
        {
            var expressionType = convert_strong(tuple).type;
            var expressionTypeName = expressionType.BaseFullName;

            var tupleName = "System.Tuple";
            if (expressionTypeName.StartsWith(tupleName) &&
                int.Parse(expressionTypeName.Substring(tupleName.Length + 1, 1)) == length.val)
                return;

            AddError(get_location(tuple),
                "EXPRESSION_OF_TYPE_{0}_CANNOT_BE_MATCHED_AGAINST_PATTERN_WITH_TYPE_{1}", expressionType.name, "Tuple`" + length.val);
        }
    }
}
