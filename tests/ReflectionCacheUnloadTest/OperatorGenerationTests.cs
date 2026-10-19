using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using PascalABCCompiler.NetHelper;
using PascalABCCompiler.SemanticTree;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.TreeRealization;

internal static class OperatorGenerationTests
{
    private const BindingFlags Hidden = BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic;

    internal static int Run(string fixturePath)
    {
        WeakReference context = Exercise(fixturePath);
        for (int i = 0; context.IsAlive && i < 10; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        if (context.IsAlive)
            throw new InvalidOperationException("Operator generation retained its collectible assembly.");
        Console.WriteLine("Operator layers: base unchanged, overrides/generation-only keys reset, context unloaded.");
        return 0;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference Exercise(string fixturePath)
    {
        ResetGeneration();
        IDictionary permanent = Table("permanentOperators");
        var baseline = new Dictionary<object, object>();
        foreach (DictionaryEntry pair in permanent) baseline.Add(pair.Key, pair.Value);
        var key = basic_function_type.iadd;
        var baseOperator = SystemLibrary.find_operator(key);
        var fallback = SystemLibrary.find_operator(basic_function_type.isub);
        var generationOnlyKey = (basic_function_type)int.MaxValue;
        var missingKey = (basic_function_type)(int.MaxValue - 1);
        ExpectMissing(generationOnlyKey);
        ExpectMissing(missingKey);

        var context = new AssemblyLoadContext("PABC.OperatorGenerationTest", isCollectible: true);
        var weak = new WeakReference(context);
        Assembly assembly = context.LoadFromAssemblyPath(fixturePath);
        var userType = compiled_type_node.get_type_node(assembly.GetType("ReflectionCacheUnloadFixture.FixtureType", true));
        var first = new basic_function_node(key, userType, false);
        var second = new basic_function_node(key, userType, false);
        var generationOnly = new basic_function_node(generationOnlyKey, userType, false);

        // Use the common registration path, not direct writes to either table.
        Register(key, first);
        Register(key, second);
        Register(generationOnlyKey, generationOnly);
        if (!ReferenceEquals(SystemLibrary.find_operator(key), second) ||
            !ReferenceEquals(SystemLibrary.find_operator(generationOnlyKey), generationOnly) ||
            !ReferenceEquals(SystemLibrary.find_operator(basic_function_type.isub), fallback))
            throw new InvalidOperationException("Generation-first lookup or last-registration-wins changed.");
        AssertPermanentTable(permanent, baseline);

        for (int generation = 0; generation < 3; generation++)
        {
            ResetGeneration();
            AssertPermanentTable(permanent, baseline);
            if (Table("generationOperators").Count != 0 ||
                !ReferenceEquals(SystemLibrary.find_operator(key), baseOperator))
                throw new InvalidOperationException("Reset did not reveal the unchanged permanent operator.");
            ExpectMissing(generationOnlyKey);
            ExpectMissing(missingKey);
            if (generation < 2)
            {
                Register(key, first);
                Register(generationOnlyKey, generationOnly);
            }
        }

        Type owner = typeof(NetHelper).Assembly.GetType("PascalABCCompiler.TreeConverter.CompilerReflectionCaches", true);
        object state = owner.GetProperty("Current", Hidden).GetValue(null);
        state.GetType().GetMethod("RemoveAssembly", Hidden).Invoke(state, new object[] { assembly });
        context.Unload();
        return weak;
    }

    private static void AssertPermanentTable(IDictionary original, Dictionary<object, object> baseline)
    {
        IDictionary current = Table("permanentOperators");
        if (!ReferenceEquals(current, original) || current.Count != baseline.Count)
            throw new InvalidOperationException("The permanent table was replaced or resized during a generation.");
        foreach (var pair in baseline)
            if (!ReferenceEquals(current[pair.Key], pair.Value))
                throw new InvalidOperationException("A permanent operator was overwritten: " + pair.Key);
    }

    private static IDictionary Table(string name) =>
        (IDictionary)typeof(SystemLibrary).GetField(name, Hidden).GetValue(null);
    private static void Register(basic_function_type key, basic_function_node value) =>
        typeof(SystemLibrary).GetMethod("add_stand_type", Hidden).Invoke(null, new object[] { key, value });
    private static void ResetGeneration() =>
        typeof(SystemLibrary).GetMethod("ResetDynamicReferenceTypeOperators", Hidden).Invoke(null, null);
    private static void ExpectMissing(basic_function_type key)
    {
        try { SystemLibrary.find_operator(key); }
        catch (KeyNotFoundException) { return; }
        throw new InvalidOperationException("An absent/generation-only operator was found after reset: " + key);
    }
}
