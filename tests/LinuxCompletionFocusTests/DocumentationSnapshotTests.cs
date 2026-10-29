using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

internal static class DocumentationSnapshotTests
{
    internal static void Run(string directory, Action<bool, string> check)
    {
        Assembly completion = Assembly.LoadFrom(Path.Combine(directory, "CodeCompletion.dll"));
        Type symbol = Assembly.LoadFrom(Path.Combine(directory, "ParserTools.dll"))
            .GetType("PascalABCCompiler.Parsers.SymInfo", true);
        Type kind = symbol.GetConstructors()[0].GetParameters()[1].ParameterType;
        Func<string, object> makeSymbol = name => Activator.CreateInstance(symbol, name, Activator.CreateInstance(kind), "original " + name);
        foreach (bool units in new[] { false, true })
        {
            Type cache = completion.GetType(units ? "CodeCompletion.UnitDocCache" : "CodeCompletion.AssemblyDocCache", true);
            string label = units ? "unit" : "assembly";
            Action<object, Type> add;
            if (units)
            {
                Type scopeType = completion.GetType("CodeCompletion.TypeScope", true);
                MethodInfo enqueue = cache.GetMethod("AddDescribeToComplete", new[] { scopeType });
                add = (info, ignored) => {
                    object scope = Activator.CreateInstance(scopeType);
                    scopeType.GetField("si").SetValue(scope, info);
                    enqueue.Invoke(null, new[] { scope });
                };
            }
            else
            {
                MethodInfo enqueue = cache.GetMethod("AddDescribeToComplete", new[] { symbol, typeof(Type) });
                add = (info, type) => enqueue.Invoke(null, new[] { info, type });
            }
            MethodInfo take = cache.GetMethod("TakeDocumentationWork");
            Func<Action<object, string>, Func<bool>, Action> work = (publish, cancelled) =>
                (Action)take.Invoke(null, new object[] { publish, cancelled });
            var oldResults = new List<object>();
            var newResults = new List<object>();
            object first = makeSymbol("first"), second = makeSymbol("second");
            add(first, typeof(string));
            Action firstWork = work((info, text) => oldResults.Add(info), () => false);
            add(second, typeof(int));
            Action secondWork = work((info, text) => newResults.Add(info), () => false);
            firstWork(); secondWork();
            check(oldResults.Count == 1 && oldResults[0] == first && newResults.Count == 1 && newResults[0] == second &&
                !(bool)symbol.GetField("has_doc").GetValue(first) && (string)symbol.GetField("description").GetValue(first) == "original first",
                label + " documentation snapshots are independent and do not mutate symbols on the reader thread");
            int cancelledResults = 0;
            add(makeSymbol("cancelled"), typeof(double));
            work((info, text) => cancelledResults++, () => true)();
            check(cancelledResults == 0, label + " documentation work stops before reading a cancelled request");
            int partialResults = 0;
            bool stop = false;
            add(makeSymbol("one"), typeof(long)); add(makeSymbol("two"), typeof(decimal));
            work((info, text) => { partialResults++; stop = true; }, () => stop)();
            check(partialResults == 1, label + " documentation work checks cancellation between descriptions");
            int remaining = 0;
            work((info, text) => remaining++, () => false)();
            check(remaining == 0, label + " pending queue is drained without waiting for a reader");
            object legacy = makeSymbol("legacy");
            add(legacy, typeof(string));
            cache.GetMethod("CompleteDocumentation").Invoke(null, null);
            var legacyReader = (Thread)cache.GetField("th", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            if (!legacyReader.Join(3000)) throw new Exception("Legacy documentation reader timed out.");
            cache.GetMethod("Reset").Invoke(null, null);
            check((bool)symbol.GetField("has_doc").GetValue(legacy),
                label + " legacy CompleteDocumentation/Reset behavior remains available for other hosts");
        }
    }
}
