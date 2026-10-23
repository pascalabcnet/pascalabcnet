using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using PascalABCCompiler.NetHelper;
using PascalABCCompiler.TreeRealization;

internal static class Program
{
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
        Console.WriteLine("PASS " + message);
    }

    private static IDictionary Cache(string name)
    {
        return (IDictionary)typeof(NetHelper).GetProperty(name,
            BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
    }

    private static using_namespace_list Namespaces(string name)
    {
        var result = new using_namespace_list();
        result.AddElement(new using_namespace(name));
        return result;
    }

    private static Type AddAssembly(string ns, string shortName = "LateProbe")
    {
        var assembly = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName("LateLookup" + Guid.NewGuid().ToString("N")), AssemblyBuilderAccess.Run);
        var module = assembly.DefineDynamicModule("Main");
        Type type = module.DefineType(ns + "." + shortName, TypeAttributes.Public).CreateType();
        NetHelper.init_namespaces(type.Assembly);
        return type;
    }

    private static void Main()
    {
        NetHelper.reset();
        var first = Namespaces("FirstLateLookup");
        var second = Namespaces("SecondLateLookup");
        var unrelated = Namespaces("UnrelatedLookup");
        NetHelper.GetMembers(typeof(string), "Length");
        object table = Cache("members")[typeof(string)];
        Check(NetHelper.FindType("LateProbe", first) == null, "initial short miss");
        Check(NetHelper.FindType("FirstLateLookup.LateProbe") == null, "initial qualified miss");
        object shortMiss = Cache("type_search_cache")["LateProbe"];
        object fullMiss = Cache("type_search_cache")["FirstLateLookup.LateProbe"];
        for (int i = 0; i < 100; i++)
        {
            if (NetHelper.FindType("LateProbe", first) != null ||
                NetHelper.FindType("FirstLateLookup.LateProbe") != null)
                throw new Exception("A missing name unexpectedly resolved.");
        }
        Check(shortMiss != null && ReferenceEquals(shortMiss, Cache("type_search_cache")["LateProbe"]),
            "short miss retained and reused while DLL set is unchanged");
        Check(fullMiss != null && ReferenceEquals(fullMiss, Cache("type_search_cache")["FirstLateLookup.LateProbe"]),
            "qualified miss retained and reused while DLL set is unchanged");
        Type firstType = AddAssembly("FirstLateLookup");
        Check(!Cache("type_search_cache").Contains("LateProbe") &&
            !Cache("type_search_cache").Contains("FirstLateLookup.LateProbe"), "new DLL clears old misses");
        Check(NetHelper.FindType("LateProbe", first) == firstType, "short name found after late DLL indexing");
        Check(NetHelper.FindType("FirstLateLookup.LateProbe") == firstType, "qualified name found after late DLL indexing");
        object positive = Cache("type_search_cache")["FirstLateLookup.LateProbe"];
        Check(NetHelper.FindType("StillMissing", first) == null, "miss after initial DLL");
        object unchangedMiss = Cache("type_search_cache")["StillMissing"];
        for (int i = 0; i < 20; i++) NetHelper.init_namespaces(firstType.Assembly);
        Check(ReferenceEquals(unchangedMiss, Cache("type_search_cache")["StillMissing"]),
            "reconnecting an indexed DLL does not clear misses");
        Check(NetHelper.FindType("LateProbe", unrelated) == null, "unimported namespace remains invisible");
        Type secondType = AddAssembly("SecondLateLookup");
        Check(!Cache("type_search_cache").Contains("StillMissing"), "second new DLL clears only stale misses");
        Check(NetHelper.FindType("LateProbe", second) == secondType, "same short name in a later namespace");
        Check(NetHelper.FindType("LateProbe", first) == firstType, "original namespace still resolves correctly");
        Check(NetHelper.FindType("lateprobe", second) == secondType, "Pascal case-insensitive lookup");
        Check(ReferenceEquals(positive, Cache("type_search_cache")["FirstLateLookup.LateProbe"]),
            "positive type cache entry preserved");
        Check(ReferenceEquals(table, Cache("members")[typeof(string)]), "member table was not reset");
        Check(NetHelper.FindType("DirectAlias") == null, "direct registration starts with cached miss");
        Check(NetHelper.FindType("directalias") == null, "case-variant registration starts with cached miss");
        NetHelper.AddType("DirectAlias", typeof(int));
        Check(NetHelper.FindType("DirectAlias") == typeof(int), "AddType invalidates its own miss");
        Check(NetHelper.FindType("directalias") == typeof(int), "AddType invalidates case-variant misses");
        string resolvedName = typeof(Uri).AssemblyQualifiedName;
        Check(NetHelper.FindType(resolvedName) == null, "runtime resolution starts with cached miss");
        Check(NetHelper.FindTypeOrCreate(resolvedName) == typeof(Uri) &&
            NetHelper.FindType(resolvedName) == typeof(Uri), "runtime type discovery invalidates its own miss");
        Type sharedFirst = AddAssembly("SharedFirst", "SharedOnly");
        Type sharedSecond = AddAssembly("SharedSecond", "SharedOnly");
        Check(NetHelper.FindType("SharedOnly") == null && !Cache("type_search_cache").Contains("SharedOnly"),
            "an indexed list is not cached as globally absent");
        Check(NetHelper.FindType("SharedOnly", Namespaces("SharedFirst")) == sharedFirst &&
            NetHelper.FindType("SharedOnly", Namespaces("SharedSecond")) == sharedSecond,
            "scoped lookup survives unscoped lookup of a shared name");
    }
}
