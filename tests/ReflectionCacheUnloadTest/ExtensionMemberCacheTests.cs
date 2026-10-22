using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using PascalABCCompiler.NetHelper;

internal static class ExtensionMemberCacheTests
{
    private const BindingFlags Hidden = BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic;

    internal static void Run(string fixturePath)
    {
        WeakReference context = Exercise(fixturePath);
        for (int i = 0; context.IsAlive && i < 10; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        Check(!context.IsAlive, "Full member cache retained the unloaded DLL.");
        Console.WriteLine("Full extension cache: active/inactive DLLs filtered; tables/lists reused; late references discovered; context unloaded.");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference Exercise(string fixturePath)
    {
        NetHelper.ResetDynamicReferenceCaches();
        NetHelper.reset();
        Assembly stable = typeof(PermanentCacheExtensions).Assembly;
        NetHelper.GetMembers(typeof(string), "Length");
        var stringTable = Table(typeof(string));
        var lengthMembers = stringTable["Length"];
        NetHelper.GetMembers(typeof(List<int>), "Count");
        var listTable = Table(typeof(List<int>));

        NetHelper.init_namespaces(stable);
        var context = new AssemblyLoadContext("FullExtensionMemberCache", isCollectible: true);
        var weak = new WeakReference(context);
        Assembly fixture = context.LoadFromAssemblyPath(fixturePath);
        NetHelper.init_namespaces(fixture);
        Check(ReferenceEquals(stringTable, Table(typeof(string))), "Late DLL replaced a native member table.");
        Check(ReferenceEquals(listTable, Table(typeof(List<int>))), "Late interface extensions replaced a member table.");
        var stringCandidates = stringTable["FullCacheProbe"];
        Check(stringCandidates.Any(m => m.DeclaringType.Assembly == fixture), "Late DLL did not update a cached string.");

        for (int cycle = 0; cycle < 12; cycle++)
        {
            NetHelper.reset();
            NetHelper.init_namespaces(stable);
            Check(NetHelper.GetMembers(typeof(string), "FullCacheProbe").All(m => m.DeclaringType.Assembly == stable),
                "Disconnected DLL leaked through GetMembers.");
            Check(NetHelper.FindName(typeof(string), "FullCacheProbe").Count == 1,
                "Disconnected DLL leaked through FindName.");
            Check(NetHelper.FindNameIncludeProtected(typeof(string), "FullCacheProbe", true).Count == 1,
                "Disconnected DLL leaked through protected/case-sensitive lookup.");
            Check(NetHelper.GetExtensionMethods(typeof(string)).All(m => m.DeclaringType.Assembly != fixture),
                "Disconnected DLL leaked into IntelliSense extension candidates.");
            Check(NetHelper.GetMembers(typeof(List<int>), "FullCacheProbe").Count == 0,
                "Disconnected interface extension remained visible.");
            Type fixtureType = fixture.GetType("ReflectionCacheUnloadFixture.FixtureType", true);
            Check(NetHelper.GetMembers(fixtureType, "Echo").Count > 0,
                "Filtering extensions hid an ordinary member from a dependency.");
            Check(stringCandidates.Any(m => m.DeclaringType.Assembly == fixture),
                "Filtering deleted a candidate from the full cache.");

            // Build new receiver tables while the extension DLL is inactive.
            Check(NetHelper.GetMembers(typeof(HashSet<int>), "FullCacheProbe").Count == 0,
                "An inactive DLL was visible on a newly cached receiver.");
            var inactiveTable = Table(typeof(HashSet<int>));
            Check(inactiveTable["FullCacheProbe"].Any(m => m.DeclaringType.Assembly == fixture),
                "A table built without the DLL omitted its known extensions.");
            NetHelper.GetMembers(typeof(int[]), "FullArrayCacheProbe");
            var arrayTable = Table(typeof(int[]));

            NetHelper.init_namespaces(fixture);
            Check(ReferenceEquals(stringTable, Table(typeof(string))) &&
                  ReferenceEquals(listTable, Table(typeof(List<int>))) &&
                  ReferenceEquals(inactiveTable, Table(typeof(HashSet<int>))) &&
                  ReferenceEquals(arrayTable, Table(typeof(int[]))),
                "Reconnecting an unchanged DLL rebuilt metadata tables.");
            Check(ReferenceEquals(lengthMembers, NetHelper.GetMembers(typeof(string), "Length")),
                "Ordinary member lists were rebuilt.");
            Check(ReferenceEquals(stringCandidates, NetHelper.GetMembers(typeof(string), "FullCacheProbe")),
                "All-active lookup allocated/replaced the cached candidate list.");
            // Name lookup returns candidates, including generic-interface
            // candidates that overload resolution may subsequently reject.
            Check(NetHelper.FindName(typeof(string), "FullCacheProbe").Count == stringCandidates.Count &&
                  NetHelper.FindNameIncludeProtected(typeof(string), "FullCacheProbe", true).Count == stringCandidates.Count,
                "Reconnected DLL was missing from semantic member lookup.");
            Check(NetHelper.GetMembers(typeof(HashSet<int>), "FullCacheProbe").Any(m => m.DeclaringType.Assembly == fixture),
                "Reconnected interface extension was not found.");
            Check(NetHelper.GetMembers(typeof(int[]), "FullArrayCacheProbe").Any(m => m.DeclaringType.Assembly == fixture),
                "Reconnected array extension was not found.");
            int candidateCount = stringCandidates.Count;
            int completionCount = NetHelper.GetExtensionMethods(typeof(string)).Count();
            for (int repeat = 0; repeat < 20; repeat++)
            {
                NetHelper.init_namespaces(fixture);
                Check(ReferenceEquals(stringTable, Table(typeof(string))) && stringCandidates.Count == candidateCount,
                    "Unchanged references rebuilt or duplicated member metadata.");
                Check(NetHelper.GetExtensionMethods(typeof(string)).Count() == completionCount,
                    "Completion modified the shared extension index.");
            }
        }

        object state = State();
        state.GetType().GetMethod("RemoveAssembly", Hidden).Invoke(state, new object[] { fixture });
        Check(NetHelper.GetMembers(typeof(string), "FullCacheProbe").Any(m => m.DeclaringType.Assembly == stable),
            "Unloading one DLL removed another DLL's string extension.");
        Check(NetHelper.GetMembers(typeof(int[]), "FullArrayCacheProbe").Any(m => m.DeclaringType.Assembly == stable),
            "Unloading one DLL removed another DLL's array extension.");
        context.Unload();
        return weak;
    }

    private static object State()
    {
        Type owner = typeof(NetHelper).Assembly.GetType("PascalABCCompiler.TreeConverter.CompilerReflectionCaches", true);
        return owner.GetProperty("Current", Hidden).GetValue(null);
    }

    private static Dictionary<string, List<MemberInfo>> Table(Type type)
    {
        object state = State();
        var tables = (Dictionary<Type, Dictionary<string, List<MemberInfo>>>)state.GetType().GetField("Members", Hidden).GetValue(state);
        return tables[type];
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }
}

public static class PermanentCacheExtensions
{
    public static int FullCacheProbe(this string value, int unused) => value.Length;
    public static int FullArrayCacheProbe<T>(this T[] value, int unused) => value.Length;
}
