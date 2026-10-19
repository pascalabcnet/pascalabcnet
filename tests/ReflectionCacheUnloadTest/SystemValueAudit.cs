using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using PascalABCCompiler.NetHelper;
using PascalABCCompiler.TreeRealization;
using PascalABCCompiler.SystemLibrary;

internal static class SystemValueAudit
{
    private const BindingFlags Hidden = BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic;
    private static readonly string[] CacheNames =
    {
        "_implicit_convertions_to", "_implicit_convertions_from",
        "_explicit_convertions_to", "_explicit_convertions_from"
    };

    internal static int Run(string fixturePath)
    {
        foreach (FieldInfo field in typeof(compiled_type_node).GetFields(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            if (typeof(IDictionary<type_node, function_node>).IsAssignableFrom(field.FieldType))
                throw new InvalidOperationException("A conversion dictionary is still stored on a compiled type node: " + field.Name);

        // Full replacement invalidates the semantic identity map, so keep it last.
        var scenarios = new[]
        {
            (Name: "CacheEnum", Reset: "assembly"),
            (Name: "CacheFlags", Reset: "reference-set"),
            (Name: "CacheFlags", Reset: "full")
        };
        foreach (var scenario in scenarios)
        {
            var references = Exercise(fixturePath, scenario.Name, scenario.Reset);
            Collect();
            if (references.Context.IsAlive)
                throw new InvalidOperationException(scenario.Name + " context is retained after " + scenario.Reset + " reset.");
            foreach (var cache in references.Caches)
                if (cache.IsAlive)
                    throw new InvalidOperationException("A discarded conversion state is still retained by a permanent node.");
            Console.WriteLine(scenario.Name + ": current-state caches replaced; old state and context collected (" + scenario.Reset + ").");
        }
        return 0;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (WeakReference Context, List<WeakReference> Caches) Exercise(string path, string enumName, string reset)
    {
        var baseline = new Dictionary<object, object>();
        foreach (DictionaryEntry entry in Operators()) baseline.Add(entry.Key, entry.Value);
        var context = new AssemblyLoadContext("PABC.SystemValueAudit", isCollectible: true);
        var weak = new WeakReference(context);
        Assembly assembly = context.LoadFromAssemblyPath(path);
        var integer = (compiled_type_node)SystemLibrary.integer_type;
        var userEnum = compiled_type_node.get_type_node(assembly.GetType("ReflectionCacheUnloadFixture." + enumName, true));
        var conversion = integer.get_explicit_conversion_from(userEnum);
        if (conversion == null || !ReferenceEquals(conversion, integer.get_explicit_conversion_from(userEnum)))
            throw new InvalidOperationException("A real enum conversion was not created/reused in the unchanged reference set.");

        object originalState = ConversionState();
        var discardedStates = new List<WeakReference> { new WeakReference(originalState) };
        var safeKey = compiled_type_node.get_type_node(typeof(string));
        var safeValue = compiled_function_node.get_compiled_method(typeof(string).GetMethod("ToString", Type.EmptyTypes));
        var frameworkKey = compiled_type_node.get_type_node(typeof(short));
        var userValue = compiled_function_node.get_compiled_method(
            assembly.GetType("ReflectionCacheUnloadFixture.FixtureType", true).GetMethod("Echo"));
        var oldMaps = new List<IDictionary>();
        foreach (string name in CacheNames)
        {
            IDictionary map = Map(integer, name);
            if (oldMaps.Exists(other => ReferenceEquals(other, map)))
                throw new InvalidOperationException("Different conversion directions share a dictionary.");
            map[userEnum] = conversion;
            map[frameworkKey] = userValue;
            map[safeKey] = safeValue;
            if (!ReferenceEquals(map, Map(integer, name)))
                throw new InvalidOperationException("The unchanged reference set did not reuse its dictionary.");
            IDictionary otherOwner = Map(safeKey, name);
            if (ReferenceEquals(map, otherOwner) || otherOwner.Contains(userEnum))
                throw new InvalidOperationException("Conversion dictionaries of different owners were mixed.");
            oldMaps.Add(map);
        }
        if (!ReferenceEquals(originalState, ConversionState()))
            throw new InvalidOperationException("The conversion state changed without reference invalidation.");

        if (reset == "full")
        {
            NetHelper.ResetDynamicReferenceCaches();
            AssertFresh(integer, oldMaps, originalState);
            var recreatedInteger = compiled_type_node.get_type_node(typeof(int));
            if (ReferenceEquals(integer, recreatedInteger))
                throw new InvalidOperationException("The recreated-node test did not get a separate semantic owner.");
            // The original integer node is no longer in Current.CompiledTypes.
            // Filling it again must still use the new reference-set state.
            discardedStates.Add(new WeakReference(ConversionState()));
            oldMaps.Clear();
            foreach (string name in CacheNames)
            {
                IDictionary map = Map(integer, name);
                map[userEnum] = conversion;
                map[frameworkKey] = userValue;
                map[safeKey] = safeValue;
                oldMaps.Add(map);
            }
            foreach (string name in CacheNames)
                if (ReferenceEquals(Map(integer, name), Map(recreatedInteger, name)) ||
                    Map(recreatedInteger, name).Contains(userEnum))
                    throw new InvalidOperationException("Different nodes for the same CLR type shared a conversion dictionary.");
        }
        else if (reset == "reference-set")
        {
            // Exercise the production boundary, even when this directly loaded
            // test assembly is not registered with CollectibleReferenceManager.
            typeof(NetHelper).GetMethod("ResetCollectibleReferenceGeneration", Hidden).Invoke(null, null);
            AssertFresh(integer, oldMaps, originalState);
        }

        typeof(SystemLibrary).GetMethod("ResetDynamicReferenceTypeOperators", Hidden).Invoke(null, null);
        object state = CurrentState();
        state.GetType().GetMethod("RemoveAssembly", Hidden).Invoke(state, new object[] { assembly });
        AssertFresh(integer, oldMaps, originalState);

        IDictionary operators = Operators();
        var generation = (IDictionary)typeof(SystemLibrary).GetField("generationOperators", Hidden).GetValue(null);
        if (generation.Count != 0 || operators.Count != baseline.Count)
            throw new InvalidOperationException("Operator cleanup changed the permanent base or retained temporary entries.");
        foreach (var entry in baseline)
            if (!ReferenceEquals(operators[entry.Key], entry.Value) ||
                !ReferenceEquals(SystemLibrary.find_operator((PascalABCCompiler.SemanticTree.basic_function_type)entry.Key), entry.Value))
                throw new InvalidOperationException("System operator changed: " + entry.Key);
        context.Unload();
        return (weak, discardedStates);
    }

    private static void AssertFresh(compiled_type_node owner, List<IDictionary> oldMaps, object oldState)
    {
        if (ReferenceEquals(oldState, ConversionState()))
            throw new InvalidOperationException("The old conversion state was not replaced.");
        for (int i = 0; i < CacheNames.Length; i++)
        {
            IDictionary current = Map(owner, CacheNames[i]);
            if (ReferenceEquals(current, oldMaps[i]) || current.Count != 0)
                throw new InvalidOperationException("A permanent node still sees an old conversion dictionary.");
        }
    }

    private static IDictionary Map(compiled_type_node owner, string name) =>
        (IDictionary)typeof(compiled_type_node).GetProperty(name, Hidden).GetValue(owner);
    private static object CurrentState() =>
        typeof(NetHelper).Assembly.GetType("PascalABCCompiler.TreeConverter.CompilerReflectionCaches", true)
            .GetProperty("Current", Hidden).GetValue(null);
    private static object ConversionState()
    {
        object state = CurrentState();
        return state.GetType().GetProperty("ConversionCaches", Hidden).GetValue(state);
    }
    private static IDictionary Operators() =>
        (IDictionary)typeof(SystemLibrary).GetField("permanentOperators", Hidden).GetValue(null);
    private static void Collect()
    {
        for (int i = 0; i < 10; i++) { GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); }
    }
}
