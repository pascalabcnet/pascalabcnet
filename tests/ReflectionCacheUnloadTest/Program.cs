using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using PascalABCCompiler.NetHelper;
using PascalABCCompiler.TreeRealization;

internal static class Program
{
    private const string FixtureAssemblyName = "ReflectionCacheUnloadFixture.dll";
    private const string FixtureTypeName = "ReflectionCacheUnloadFixture.FixtureType";

    private static int Main(string[] args)
    {
        string repositoryRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
            "..", "..", "..", "..", ".."));
        string versionOnePath = Path.Combine(repositoryRoot, "tests", "ReflectionCacheUnloadFixture",
            "bin", "Debug", "net10.0", FixtureAssemblyName);
        string versionTwoPath = Path.Combine(repositoryRoot, "tests", "ReflectionCacheUnloadFixtureV2",
            "bin", "Debug", "net10.0", FixtureAssemblyName);

        if (!File.Exists(versionOnePath) || !File.Exists(versionTwoPath))
        {
            Console.Error.WriteLine("Fixture assemblies were not built.");
            return 2;
        }

        if (Array.IndexOf(args, "--audit-system-values") >= 0)
            return SystemValueAudit.Run(versionOnePath);

        if (Array.IndexOf(args, "--operator-generations") >= 0)
            return OperatorGenerationTests.Run(versionOnePath);


        if (OperatorGenerationTests.Run(versionOnePath) != 0)
            return 1;

        if (SystemValueAudit.Run(versionOnePath) != 0)
            return 1;

        string workDirectory = Path.Combine(Path.GetTempPath(),
            "pabc-reflection-cache-reload-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(workDirectory);
        string workingAssemblyPath = Path.Combine(workDirectory, FixtureAssemblyName);

        try
        {
            File.Copy(versionOnePath, workingAssemblyPath, overwrite: true);
            WeakReference versionOneContext = FillCachesAndRequestUnload(
                workingAssemblyPath, "VersionOneOnly", "VersionTwoOnly");
            if (!WaitForUnload(versionOneContext, "version 1", args))
                return 1;

            // Replace the DLL at exactly the same path with an assembly having the same identity.
            File.Copy(versionTwoPath, workingAssemblyPath, overwrite: true);
            WeakReference versionTwoContext = FillCachesAndRequestUnload(
                workingAssemblyPath, "VersionTwoOnly", "VersionOneOnly");
            if (!WaitForUnload(versionTwoContext, "version 2", args))
                return 1;

            WeakReference selectiveContext = FillFrameworkKeyCachesAndUnload(versionOnePath);
            if (!WaitForUnload(selectiveContext, "framework-key extension and reflected member caches", args))
                return 1;

            // This test replaces the metadata identity map. Run it after the
            // enum/conversion audit, which intentionally keeps permanent owners.
            ExtensionMemberCacheTests.Run(versionOnePath);

            Console.WriteLine("Both versions were loaded from the same path and unloaded successfully.");
            return 0;
        }
        finally
        {
            try
            {
                Directory.Delete(workDirectory, recursive: true);
            }
            catch (IOException)
            {
                // A cleanup failure must not hide the unload-test result.
            }
        }
    }

    private static bool WaitForUnload(WeakReference contextReference, string version, string[] args)
    {
        for (int attempt = 0; contextReference.IsAlive && attempt < 10; attempt++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        if (!contextReference.IsAlive)
            return true;

        Console.Error.WriteLine("Collectible AssemblyLoadContext for " + version + " is still alive.");
        if (Array.IndexOf(args, "--wait-for-dump") >= 0)
        {
            Console.WriteLine("PID: " + Environment.ProcessId);
            Console.WriteLine("Press Enter after collecting the dump.");
            Console.ReadLine();
        }
        return false;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference FillFrameworkKeyCachesAndUnload(string fixturePath)
    {
        NetHelper.ResetDynamicReferenceCaches();
        var context = new AssemblyLoadContext("PABC.FrameworkKeyCacheTest", isCollectible: true);
        var weak = new WeakReference(context);
        Assembly assembly = context.LoadFromAssemblyPath(fixturePath);
        Type fixture = assembly.GetType(FixtureTypeName, throwOnError: true);
        MethodInfo method = fixture.GetMethod("Echo");
        MethodInfo inherited = fixture.GetMethod("ToString", Type.EmptyTypes);
        if (inherited.DeclaringType != typeof(object) || inherited.ReflectedType != fixture)
            throw new InvalidOperationException("The reflected-member fixture is not an inherited core method.");
        var flags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        Type owner = typeof(NetHelper).Assembly.GetType("PascalABCCompiler.TreeConverter.CompilerReflectionCaches", true);
        object state = owner.GetProperty("Current", flags).GetValue(null);
        T Cache<T>(string name) => (T)state.GetType().GetField(name, flags).GetValue(state);
        var members = Cache<Dictionary<Type, Dictionary<string, List<MemberInfo>>>>("Members");
        members[typeof(object)] = new Dictionary<string, List<MemberInfo>> { ["extension"] = new List<MemberInfo> { method } };
        members[typeof(string)] = new Dictionary<string, List<MemberInfo>> { ["Length"] = new List<MemberInfo> { typeof(string).GetProperty("Length") } };
        var handles = Cache<Dictionary<Type, Dictionary<int, MethodInfo>>>("MethodHandles");
        handles[typeof(object)] = new Dictionary<int, MethodInfo> { [1] = method };
        var constructors = Cache<Dictionary<Type, Dictionary<int, ConstructorInfo>>>("ConstructorHandles");
        constructors[typeof(object)] = new Dictionary<int, ConstructorInfo> { [1] = fixture.GetConstructor(Type.EmptyTypes) };
        var fields = Cache<Dictionary<Type, Dictionary<int, FieldInfo>>>("FieldHandles");
        fields[typeof(object)] = new Dictionary<int, FieldInfo> { [1] = fixture.GetField("<Dependency>k__BackingField", flags) };
        var entries = Cache<Dictionary<Assembly, List<Type>>>("EntryTypes");
        entries[typeof(object).Assembly] = new List<Type> { fixture };
        var memberCache = Cache<Dictionary<string, MemberInfo>>("MemberCache");
        memberCache["inherited"] = inherited;
        memberCache["framework"] = typeof(string).GetProperty("Length");
        Type generic = typeof(List<>).MakeGenericType(fixture);
        members[generic] = new Dictionary<string, List<MemberInfo>>();
        var extensionMethods = Cache<Dictionary<Type, MethodInfo[]>>("ExtensionMethods");
        extensionMethods[typeof(object)] = new[] { method };
        extensionMethods[typeof(string)] = new[] { typeof(string).GetMethod("ToString", Type.EmptyTypes) };
        var typeExtensions = Cache<Dictionary<Type, List<MethodInfo>>>("TypeExtensions");
        typeExtensions[typeof(object)] = new List<MethodInfo> { method };
        var arrays = Cache<Dictionary<Type, Type>>("ArraysWithExtensionMethods");
        arrays[typeof(object[])] = fixture.MakeArrayType();
        var generics = Cache<Dictionary<Type, Type>>("GenericsWithExtensionMethods");
        generics[typeof(List<>)] = generic;
        var genericArrays = Cache<Dictionary<int, List<MethodInfo>>>("GenericArrayTypeExtensions");
        genericArrays[1] = new List<MethodInfo> { method };
        var genericParameters = Cache<List<MethodInfo>>("GenericParameterTypeExtensions");
        genericParameters.Add(method);
        var specialTypes = Cache<Dictionary<int, Type>>("SpecialTypes");
        specialTypes[1] = generic;
        var namespaces = Cache<Dictionary<string, Type>>("Namespaces");
        namespaces["audit"] = generic;
        var types = Cache<Dictionary<string, object>>("Types");
        types["audit"] = generic;
        var pascalTypes = Cache<Dictionary<string, object>>("CompiledPascalTypes");
        pascalTypes["audit"] = generic;
        state.GetType().GetMethod("RemoveAssembly", flags).Invoke(state, new object[] { assembly });
        if (members.ContainsKey(typeof(object)) || handles.ContainsKey(typeof(object)) ||
            constructors.ContainsKey(typeof(object)) || fields.ContainsKey(typeof(object)) ||
            entries.ContainsKey(typeof(object).Assembly) || memberCache.ContainsKey("inherited"))
            throw new InvalidOperationException("Selective cleanup retained a collectible value under a framework key.");
        if (!members.ContainsKey(typeof(string)) || !memberCache.ContainsKey("framework"))
            throw new InvalidOperationException("Selective cleanup discarded unrelated framework cache entries.");
        if (members.ContainsKey(generic) || extensionMethods.ContainsKey(typeof(object)) ||
            typeExtensions.Count != 0 || arrays.Count != 0 || generics.Count != 0 ||
            genericArrays.Count != 0 || genericParameters.Count != 0 || specialTypes.Count != 0 ||
            namespaces.Count != 0 || types.Count != 0 || pascalTypes.Count != 0)
            throw new InvalidOperationException("Extension/type caches retained a collectible value or generic argument.");
        if (!extensionMethods.ContainsKey(typeof(string)))
            throw new InvalidOperationException("Cleanup discarded unrelated framework extension cache entries.");
        context.Unload();
        Console.WriteLine("Framework-key member/handle caches and inherited ReflectedType released; unrelated framework entries preserved.");
        return weak;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference FillCachesAndRequestUnload(
        string fixturePath, string expectedMember, string obsoleteMember)
    {
        var context = new AssemblyLoadContext("PABC.ReflectionCacheUnloadTest", isCollectible: true);
        var contextReference = new WeakReference(context);
        Assembly assembly = context.LoadFromAssemblyPath(fixturePath);
        Type fixtureType = assembly.GetType(FixtureTypeName, throwOnError: true);
        MethodInfo echoMethod = fixtureType.GetMethod("Echo");

        NetHelper.init_namespaces(assembly);
        if (NetHelper.GetMembers(fixtureType, expectedMember).Count == 0)
            throw new InvalidOperationException("Expected member was not found: " + expectedMember);
        if (NetHelper.GetMembers(fixtureType, obsoleteMember).Count != 0)
            throw new InvalidOperationException("Obsolete member is still visible: " + obsoleteMember);

        compiled_type_node.get_type_node(fixtureType);
        compiled_function_node.get_compiled_method(echoMethod);

        NetHelper.ResetDynamicReferenceCaches();
        context.Unload();
        return contextReference;
    }
}
