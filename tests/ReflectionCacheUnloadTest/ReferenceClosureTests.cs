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
            Console.WriteLine("PASS reference closure reuse, invalidation and conflict detection");
        }
        finally
        {
            CollectibleReferenceManager.BeginCompilation();
            Directory.Delete(directory, recursive: true);
        }
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
