using System;
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
