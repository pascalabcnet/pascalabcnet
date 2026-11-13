using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PascalABCCompiler.Contracts.CodeCompletion;
using PascalABCCompiler.SyntaxTree;

namespace CodeCompletion
{
    internal static class ExtensionMethodApplicability
    {
        internal static IEnumerable<MethodInfo> Filter(CompiledScope receiver, IEnumerable<MemberInfo> candidates)
        {
            TypeScope[] receiverTypes = null;
            var inferred = new Dictionary<Type, TypeScope>();
            foreach (var method in candidates.OfType<MethodInfo>())
            {
                var parameter = method.GetParameters()[0].ParameterType;
                if (parameter.IsByRef)
                    parameter = parameter.GetElementType();
                if (!parameter.ContainsGenericParameters && !receiver.ctn.ContainsGenericParameters &&
                    parameter.IsAssignableFrom(receiver.ctn))
                {
                    yield return method;
                    continue;
                }

                if (receiverTypes == null)
                    receiverTypes = GetReceiverTypes(receiver).ToArray();
                foreach (var actual in receiverTypes)
                {
                    inferred.Clear();
                    if (Matches(parameter, actual, inferred, infer: true) &&
                        inferred.All(pair => SatisfiesConstraints(pair.Key, pair.Value, inferred)))
                    {
                        yield return method;
                        break;
                    }
                }
            }
        }

        private static IEnumerable<TypeScope> GetReceiverTypes(TypeScope type)
        {
            var seen = new HashSet<TypeScope>();
            var pending = new Stack<TypeScope>();
            pending.Push(type);
            while (pending.Count > 0)
            {
                var current = pending.Pop();
                if (current is TypeSynonim alias)
                    current = alias.actType;
                if (current == null || !seen.Add(current))
                    continue;
                yield return current;

                if (current is CompiledScope compiled)
                {
                    var arguments = compiled.GetInstances();
                    foreach (var iface in compiled.ctn.GetInterfaces())
                        yield return CompiledScope.get_type_instance(iface, arguments);
                    var baseType = compiled.ctn.BaseType;
                    if (baseType != null)
                        pending.Push(CompiledScope.get_type_instance(baseType, arguments));
                    if (compiled.ctn.IsGenericParameter)
                        foreach (var constraint in compiled.ctn.GetGenericParameterConstraints())
                            pending.Push(TypeTable.get_compiled_type(constraint));
                    continue;
                }

                pending.Push(current.baseScope);
                if (current.implemented_interfaces != null)
                    foreach (var iface in current.implemented_interfaces)
                        pending.Push(iface);
            }
        }

        private static bool Matches(Type pattern, TypeScope actual, Dictionary<Type, TypeScope> inferred,
            bool infer, bool allowVariance = true)
        {
            while (actual is TypeSynonim alias)
                actual = alias.actType;
            if (actual == null)
                return false;
            if (pattern.IsGenericParameter)
            {
                if (inferred.TryGetValue(pattern, out var bound))
                    return ReferenceEquals(bound, actual) ||
                        bound is CompiledScope b && actual is CompiledScope a &&
                        !b.ctn.ContainsGenericParameters && b.ctn == a.ctn;
                if (!infer)
                    return false;
                inferred.Add(pattern, actual);
                return true;
            }
            if (pattern.IsArray)
                return actual.IsArray && actual.Rank == pattern.GetArrayRank() &&
                    Matches(pattern.GetElementType(), actual.GetElementType(), inferred, infer, allowVariance: false);
            if (!(actual is CompiledScope compiled))
                return false;
            if (!pattern.IsGenericType)
                return compiled.ctn == pattern;
            if (!compiled.ctn.IsGenericType || compiled.ctn.GetGenericTypeDefinition() != pattern.GetGenericTypeDefinition())
                return false;

            var parameters = pattern.GetGenericArguments();
            var arguments = compiled.GetInstances();
            if (arguments.Count != parameters.Length)
                return false;
            var variance = pattern.GetGenericTypeDefinition().GetGenericArguments();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (Matches(parameters[i], arguments[i], inferred, infer, allowVariance: false))
                    continue;
                if (!allowVariance || parameters[i].ContainsGenericParameters ||
                    (variance[i].GenericParameterAttributes & GenericParameterAttributes.VarianceMask) != GenericParameterAttributes.Covariant ||
                    arguments[i].si.kind == SymbolKind.Struct || arguments[i].si.kind == SymbolKind.Enum ||
                    !GetReceiverTypes(arguments[i]).Any(type => Matches(parameters[i], type, inferred, infer: false)))
                    return false;
            }
            return true;
        }

        private static bool SatisfiesConstraints(Type parameter, TypeScope actual, Dictionary<Type, TypeScope> inferred)
        {
            var flags = parameter.GenericParameterAttributes & GenericParameterAttributes.SpecialConstraintMask;
            if (flags != GenericParameterAttributes.None && !SatisfiesSpecialConstraints(actual, flags))
                return false;

            return parameter.GetGenericParameterConstraints().All(constraint =>
                !IsBound(constraint, inferred) ||
                GetReceiverTypes(actual).Any(type => Matches(constraint, type, inferred, infer: false)));
        }

        private static bool IsBound(Type type, Dictionary<Type, TypeScope> inferred)
        {
            if (type.IsGenericParameter)
                return inferred.ContainsKey(type);
            if (type.HasElementType)
                return IsBound(type.GetElementType(), inferred);
            return type.GetGenericArguments().All(argument => IsBound(argument, inferred));
        }

        private static bool SatisfiesSpecialConstraints(TypeScope actual, GenericParameterAttributes flags)
        {
            if (actual is UnknownScope)
                return false;
            if (actual is CompiledScope compiled && compiled.ctn.IsGenericParameter)
            {
                var actualFlags = compiled.ctn.GenericParameterAttributes;
                if (compiled.ctn.BaseType is Type baseType && baseType != typeof(object) &&
                    baseType != typeof(ValueType) && baseType != typeof(Enum) && !baseType.IsGenericParameter)
                    actualFlags |= GenericParameterAttributes.ReferenceTypeConstraint;
                return (actualFlags & flags) == flags;
            }
            bool valueType = actual.si.kind == SymbolKind.Struct || actual.si.kind == SymbolKind.Enum;
            if ((flags & GenericParameterAttributes.ReferenceTypeConstraint) != 0 &&
                (valueType || actual.IsGenericParameter &&
                    !GetReceiverTypes(actual).OfType<CompiledScope>().Any(type => type.ctn.IsClass && type.ctn != typeof(object))))
                return false;
            if ((flags & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0 &&
                (!valueType || actual is CompiledScope nullable && nullable.ctn.IsGenericType &&
                    nullable.ctn.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return false;
            if ((flags & GenericParameterAttributes.DefaultConstructorConstraint) == 0 || valueType)
                return true;
            if (actual.IsGenericParameter || actual.IsAbstract)
                return false;
            if (actual is CompiledScope concrete)
                return concrete.ctn.GetConstructor(Type.EmptyTypes) != null;

            return actual.GetConstructors(false).Any(ctor => ctor.parameters.Count == 0 &&
                ctor.acc_mod == access_modifer.public_modifer);
        }
    }
}
