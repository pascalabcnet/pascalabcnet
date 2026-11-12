using System;
using System.IO;
using PascalABCCompiler.NetHelper;

internal static class ReferenceClosureTests
{
    internal static void Run(string fixtureV1, string fixtureV2)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        string directory = Path.Combine(Path.GetTempPath(), "pabc-reference-closure-" + Guid.NewGuid().ToString("N"));
        string local = Directory.CreateDirectory(Path.Combine(directory, "local")).FullName;
        string shared = Directory.CreateDirectory(Path.Combine(directory, "shared")).FullName;
        const string dependency = "ReflectionCacheUnloadDependency.dll";
        string root = Path.Combine(local, Path.GetFileName(fixtureV1));
        string fallback = Path.Combine(shared, dependency);
        string preferred = Path.Combine(local, dependency);
        string alternate = Path.Combine(shared, Path.GetFileName(fixtureV1));
        try
        {
            File.Copy(fixtureV1, root);
            File.Copy(Path.Combine(Path.GetDirectoryName(fixtureV1), dependency), fallback);
            CollectibleReferenceManager.BeginCompilation();
            Validate(root, fallback);
            Validate(root);
            ExpectBuilds(1, "same accumulated roots reuse the closure");

            File.Copy(Path.Combine(Path.GetDirectoryName(fixtureV2), dependency), preferred);
            ExpectConflict(() => Validate(root), "new local dependency shadows the fallback without adding a root");
            ExpectBuilds(2, "appearance of a missing dependency rebuilds the closure");
            File.Delete(preferred);
            Validate(root);
            ExpectBuilds(3, "validation recovers after a conflict");

            File.Copy(fixtureV2, alternate);
            ExpectConflict(() => Validate(alternate), "a new root with the same assembly name is checked");
            ExpectBuilds(4, "new root rebuilds the closure");
            Validate(root);
            ExpectBuilds(5, "a failed validation does not retain the conflicting root");

            File.SetLastWriteTimeUtc(root, File.GetLastWriteTimeUtc(root).AddMinutes(2));
            Validate(root);
            ExpectBuilds(6, "a changed file stamp rebuilds the closure");

            CollectibleReferenceManager.BeginCompilation();
            Validate(root);
            ExpectBuilds(1, "another compilation starts with an empty closure cache");

            // Give both otherwise identical PE files a spare byte beyond their
            // metadata, so we can change contents without changing the identity.
            using (var file = File.Open(root, FileMode.Append))
                file.WriteByte(0);
            File.Copy(root, alternate, overwrite: true);
            CollectibleReferenceManager.BeginCompilation();
            Validate(root, alternate);
            DateTime stamp = File.GetLastWriteTimeUtc(alternate);
            using (var file = File.Open(alternate, FileMode.Open, FileAccess.Write))
            {
                file.Seek(-1, SeekOrigin.End);
                file.WriteByte(1);
            }
            File.SetLastWriteTimeUtc(alternate, stamp);
            ExpectConflict(() => Validate(root),
                "a cache hit still compares the bytes of same-name assemblies");
            ExpectBuilds(1, "same-size, same-stamp conflict was detected using the cached graph");
            Console.WriteLine("PASS reference closure reuse, invalidation and conflict detection");
            CheckPlatformGraph(directory);
            CheckPlatformConstructor(fixtureV1);
        }
        finally
        {
            CollectibleReferenceManager.BeginCompilation();
            Directory.Delete(directory, recursive: true);
        }
    }

    private static void CheckPlatformConstructor(string fixture)
    {
        string systemAssembly = Path.Combine(NetCoreSystemReferences.RuntimeDirectory, "System.Linq.dll");
        CollectibleReferenceManager.BeginCompilation();
        try
        {
            // A registered platform reference adds its whole directory to dependency lookup.
            CollectibleReferenceManager.RegisterReferencePaths(new[] { systemAssembly, fixture });
            var assembly = CollectibleReferenceManager.Load(fixture);
            var type = assembly.GetType("ReflectionCacheUnloadFixture.PlatformConstructorFixture", throwOnError: true);
            if (type.GetConstructor(new[] { typeof(IServiceProvider) }) == null)
                throw new InvalidOperationException("The constructor parameter uses a second copy of the platform type.");
            Console.WriteLine("PASS external constructor uses the platform IServiceProvider type");
        }
        finally
        {
            CollectibleReferenceManager.StartNextGeneration();
        }
    }

    private static void CheckPlatformGraph(string directory)
    {
        string platform = Path.Combine(NetCoreSystemReferences.RuntimeDirectory, "System.Linq.dll");
        string user = Path.Combine(directory, Path.GetFileName(platform));
        File.Copy(platform, user);

        CollectibleReferenceManager.BeginCompilation();
        int before = CollectibleReferenceManager.PlatformReferenceGraphNodeCount;
        Validate(platform);
        int cached = CollectibleReferenceManager.PlatformReferenceGraphNodeCount;
        if (cached <= before + 1)
            throw new InvalidOperationException("The platform graph did not record transitive dependencies.");

        CollectibleReferenceManager.StartNextGeneration();
        CollectibleReferenceManager.BeginCompilation();
        Validate(platform, user);
        if (CollectibleReferenceManager.PlatformReferenceGraphNodeCount != cached)
            throw new InvalidOperationException("The platform graph was rebuilt or a user DLL was cached as platform.");

        DateTime stamp = File.GetLastWriteTimeUtc(user);
        using (var file = File.Open(user, FileMode.Open, FileAccess.ReadWrite))
        {
            file.Seek(-1, SeekOrigin.End);
            int value = file.ReadByte();
            file.Seek(-1, SeekOrigin.End);
            file.WriteByte((byte)(value ^ 1));
        }
        File.SetLastWriteTimeUtc(user, stamp);
        ExpectConflict(() => Validate(user), "a same-stamp user copy still conflicts with a cached platform DLL");
        ExpectBuilds(1, "the platform/user conflict is detected without rebuilding the closure");

        // The permanent graph must not make a platform DLL an active root
        // of a new validation that does not include it.
        CollectibleReferenceManager.BeginCompilation();
        Validate(user);
        if (CollectibleReferenceManager.PlatformReferenceGraphNodeCount != cached)
            throw new InvalidOperationException("The permanent graph changed while validating only a user copy.");
        Console.WriteLine("PASS platform graph survives compilations without hiding conflicts or adding inactive roots");
    }

    private static void Validate(params string[] paths) =>
        NetHelper.ValidateCollectibleReferencesForCompilation(paths);

    private static void ExpectBuilds(int expected, string message)
    {
        if (CollectibleReferenceManager.ReferenceClosureBuildCount != expected)
            throw new InvalidOperationException(message + ": got " + CollectibleReferenceManager.ReferenceClosureBuildCount);
    }

    private static void ExpectConflict(Action validate, string message)
    {
        try { validate(); }
        catch (FileLoadException) { return; }
        throw new InvalidOperationException(message);
    }
}
