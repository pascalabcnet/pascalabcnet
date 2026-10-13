// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Reflection;
using PascalABCCompiler.NetHelper;
using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.TreeConverter
{
    /// <summary>
    /// Long-lived compiler caches whose keys or values can retain reflected assemblies.
    /// Keeping them in one state object prepares an atomic reset when external assemblies
    /// are loaded through a collectible AssemblyLoadContext. No reset policy lives here.
    /// </summary>
    internal sealed class CompilerReflectionCacheState
    {
        internal readonly Dictionary<string, Type> Namespaces =
            new Dictionary<string, Type>(1024, StringComparer.CurrentCultureIgnoreCase);
        internal readonly Dictionary<string, object> Types =
            new Dictionary<string, object>(8096, StringComparer.CurrentCultureIgnoreCase);
        internal readonly Dictionary<string, FoundInfo> TypeSearch =
            new Dictionary<string, FoundInfo>(300);
        internal readonly Dictionary<string, object> CompiledPascalTypes =
            new Dictionary<string, object>(1024, StringComparer.CurrentCultureIgnoreCase);

        internal readonly Dictionary<Type, Dictionary<string, List<MemberInfo>>> Members =
            new Dictionary<Type, Dictionary<string, List<MemberInfo>>>(128);
        internal readonly Dictionary<PropertyInfo, compiled_property_node> PropertyNodes =
            new Dictionary<PropertyInfo, compiled_property_node>();
        internal readonly Dictionary<MemberInfo, definition_node> FieldNodes =
            new Dictionary<MemberInfo, definition_node>();
        internal readonly Dictionary<ConstructorInfo, compiled_constructor_node> ConstructorNodes =
            new Dictionary<ConstructorInfo, compiled_constructor_node>();

        internal readonly HashSet<Type> StandardTypes = new HashSet<Type>();
        internal readonly Dictionary<Assembly, Dictionary<int, Type>> TypeHandles =
            new Dictionary<Assembly, Dictionary<int, Type>>();
        internal readonly Dictionary<Type, Dictionary<int, MethodInfo>> MethodHandles =
            new Dictionary<Type, Dictionary<int, MethodInfo>>();
        internal readonly Dictionary<Type, Dictionary<int, ConstructorInfo>> ConstructorHandles =
            new Dictionary<Type, Dictionary<int, ConstructorInfo>>();
        internal readonly Dictionary<Type, Dictionary<int, FieldInfo>> FieldHandles =
            new Dictionary<Type, Dictionary<int, FieldInfo>>();
        internal readonly HashSet<Assembly> Assemblies = new HashSet<Assembly>();
        internal readonly Dictionary<int, Type> SpecialTypes = new Dictionary<int, Type>();
        internal readonly Dictionary<string, Type[]> NamespaceTypes =
            new Dictionary<string, Type[]>(StringComparer.CurrentCultureIgnoreCase);
        internal readonly Dictionary<Assembly, HashSet<string>> AssemblyNamespaces =
            new Dictionary<Assembly, HashSet<string>>();
        internal HashSet<Assembly> CurrentUsedAssemblies = new HashSet<Assembly>();

        internal readonly HashSet<Type> CachedTypeExtensions = new HashSet<Type>();
        internal readonly Dictionary<Type, MethodInfo[]> ExtensionMethods =
            new Dictionary<Type, MethodInfo[]>();
        internal readonly Dictionary<Type, List<MethodInfo>> TypeExtensions =
            new Dictionary<Type, List<MethodInfo>>();
        internal readonly Dictionary<Type, Type> ArraysWithExtensionMethods =
            new Dictionary<Type, Type>();
        internal readonly Dictionary<Type, Type> GenericsWithExtensionMethods =
            new Dictionary<Type, Type>();
        internal readonly Dictionary<int, List<MethodInfo>> GenericArrayTypeExtensions =
            new Dictionary<int, List<MethodInfo>>();
        internal readonly List<MethodInfo> GenericParameterTypeExtensions = new List<MethodInfo>();
        internal readonly Dictionary<string, MemberInfo> MemberCache =
            new Dictionary<string, MemberInfo>();

        internal readonly Dictionary<string, Assembly> AssembliesByName =
            new Dictionary<string, Assembly>(1024, StringComparer.CurrentCultureIgnoreCase);
        internal readonly Dictionary<Assembly, DateTime> AssemblyFileDates =
            new Dictionary<Assembly, DateTime>();
        internal readonly Dictionary<Assembly, string> AssemblyFullPaths =
            new Dictionary<Assembly, string>();
        internal readonly Dictionary<Assembly, List<Type>> EntryTypes =
            new Dictionary<Assembly, List<Type>>();

        internal readonly Dictionary<Type, compiled_type_node> CompiledTypes =
            new Dictionary<Type, compiled_type_node>();
        internal readonly Dictionary<MethodInfo, compiled_function_node> CompiledMethods =
            new Dictionary<MethodInfo, compiled_function_node>();
        internal readonly Dictionary<ConstructorInfo, compiled_constructor_node> CompiledConstructors =
            new Dictionary<ConstructorInfo, compiled_constructor_node>();

        internal CompilerReflectionCacheState()
        {
            StandardTypes.Add(typeof(int));
            StandardTypes.Add(typeof(byte));
            StandardTypes.Add(typeof(bool));
            StandardTypes.Add(typeof(sbyte));
            StandardTypes.Add(typeof(short));
            StandardTypes.Add(typeof(ushort));
            StandardTypes.Add(typeof(uint));
            StandardTypes.Add(typeof(long));
            StandardTypes.Add(typeof(ulong));
            StandardTypes.Add(typeof(char));
            StandardTypes.Add(typeof(float));
            StandardTypes.Add(typeof(double));
        }

#if PABCNET_MODERN
        internal void RemoveAssembly(Assembly assembly)
        {
            RemoveWhere(Namespaces, pair => TypeReferencesAssembly(pair.Value, assembly));
            RemoveWhere(Types, pair => ObjectReferencesAssembly(pair.Value, assembly));
            RemoveWhere(TypeSearch, pair => FoundInfoReferencesAssembly(pair.Value, assembly));
            RemoveWhere(CompiledPascalTypes, pair => ObjectReferencesAssembly(pair.Value, assembly));

            RemoveWhere(Members, pair => TypeReferencesAssembly(pair.Key, assembly));
            RemoveWhere(PropertyNodes, pair => MemberReferencesAssembly(pair.Key, assembly));
            RemoveWhere(FieldNodes, pair => MemberReferencesAssembly(pair.Key, assembly));
            RemoveWhere(ConstructorNodes, pair => MemberReferencesAssembly(pair.Key, assembly));

            TypeHandles.Remove(assembly);
            RemoveWhere(MethodHandles, pair => TypeReferencesAssembly(pair.Key, assembly));
            RemoveWhere(ConstructorHandles, pair => TypeReferencesAssembly(pair.Key, assembly));
            RemoveWhere(FieldHandles, pair => TypeReferencesAssembly(pair.Key, assembly));
            Assemblies.Remove(assembly);
            RemoveWhere(SpecialTypes, pair => TypeReferencesAssembly(pair.Value, assembly));
            NamespaceTypes.Clear();
            AssemblyNamespaces.Remove(assembly);
            CurrentUsedAssemblies.Remove(assembly);

            CachedTypeExtensions.RemoveWhere(type => TypeReferencesAssembly(type, assembly));
            RemoveWhere(ExtensionMethods, pair =>
                TypeReferencesAssembly(pair.Key, assembly) ||
                Array.Exists(pair.Value, method => MemberReferencesAssembly(method, assembly)));
            RemoveWhere(TypeExtensions, pair =>
                TypeReferencesAssembly(pair.Key, assembly) ||
                pair.Value.Exists(method => MemberReferencesAssembly(method, assembly)));
            RemoveWhere(ArraysWithExtensionMethods, pair =>
                TypeReferencesAssembly(pair.Key, assembly) || TypeReferencesAssembly(pair.Value, assembly));
            RemoveWhere(GenericsWithExtensionMethods, pair =>
                TypeReferencesAssembly(pair.Key, assembly) || TypeReferencesAssembly(pair.Value, assembly));
            RemoveWhere(GenericArrayTypeExtensions, pair =>
                pair.Value.Exists(method => MemberReferencesAssembly(method, assembly)));
            GenericParameterTypeExtensions.RemoveAll(method => MemberReferencesAssembly(method, assembly));
            RemoveWhere(MemberCache, pair => MemberReferencesAssembly(pair.Value, assembly));

            RemoveWhere(AssembliesByName, pair => pair.Value == assembly);
            AssemblyFileDates.Remove(assembly);
            AssemblyFullPaths.Remove(assembly);
            EntryTypes.Remove(assembly);

            RemoveWhere(CompiledTypes, pair => TypeReferencesAssembly(pair.Key, assembly));
            RemoveWhere(CompiledMethods, pair => MemberReferencesAssembly(pair.Key, assembly));
            RemoveWhere(CompiledConstructors, pair => MemberReferencesAssembly(pair.Key, assembly));
        }

        private static bool FoundInfoReferencesAssembly(FoundInfo foundInfo, Assembly assembly)
        {
            if (foundInfo == null || foundInfo.type_infos == null)
                return false;
            return foundInfo.type_infos.Exists(info =>
                info != null && info.type_info != null &&
                TypeReferencesAssembly(info.type_info.type, assembly));
        }

        private static bool ObjectReferencesAssembly(object value, Assembly assembly)
        {
            NetHelper.TypeInfo typeInfo = value as NetHelper.TypeInfo;
            if (typeInfo != null)
                return TypeReferencesAssembly(typeInfo.type, assembly);

            List<NetHelper.TypeInfo> typeInfos = value as List<NetHelper.TypeInfo>;
            if (typeInfos != null)
                return typeInfos.Exists(info => info != null && TypeReferencesAssembly(info.type, assembly));

            Type type = value as Type;
            if (type != null)
                return TypeReferencesAssembly(type, assembly);

            compiled_type_node compiledType = value as compiled_type_node;
            return compiledType != null && TypeReferencesAssembly(compiledType.compiled_type, assembly);
        }

        private static bool MemberReferencesAssembly(MemberInfo member, Assembly assembly)
        {
            if (member == null)
                return false;
            if (member.Module.Assembly == assembly || TypeReferencesAssembly(member.DeclaringType, assembly))
                return true;

            MethodInfo method = member as MethodInfo;
            if (method != null)
            {
                if (TypeReferencesAssembly(method.ReturnType, assembly))
                    return true;
                if (method.IsGenericMethod &&
                    Array.Exists(method.GetGenericArguments(), type => TypeReferencesAssembly(type, assembly)))
                    return true;
                return Array.Exists(method.GetParameters(), parameter =>
                    TypeReferencesAssembly(parameter.ParameterType, assembly));
            }

            PropertyInfo property = member as PropertyInfo;
            if (property != null)
                return TypeReferencesAssembly(property.PropertyType, assembly) ||
                    Array.Exists(property.GetIndexParameters(), parameter =>
                        TypeReferencesAssembly(parameter.ParameterType, assembly));

            FieldInfo field = member as FieldInfo;
            return field != null && TypeReferencesAssembly(field.FieldType, assembly);
        }

        private static bool TypeReferencesAssembly(Type type, Assembly assembly)
        {
            if (type == null)
                return false;
            if (type.Assembly == assembly)
                return true;
            if (type.HasElementType && TypeReferencesAssembly(type.GetElementType(), assembly))
                return true;
            return type.IsGenericType &&
                Array.Exists(type.GetGenericArguments(), argument => TypeReferencesAssembly(argument, assembly));
        }

        private static void RemoveWhere<TKey, TValue>(
            Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate)
        {
            var keys = new List<TKey>();
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
                if (predicate(pair))
                    keys.Add(pair.Key);
            foreach (TKey key in keys)
                dictionary.Remove(key);
        }
#endif
    }

    internal static class CompilerReflectionCaches
    {
        internal static CompilerReflectionCacheState Current { get; private set; } =
            new CompilerReflectionCacheState();

#if PABCNET_MODERN
        internal static CompilerReflectionCacheState Replace()
        {
            Current = new CompilerReflectionCacheState();
            return Current;
        }
#endif
    }
}
