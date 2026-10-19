namespace NuGetRestoreFixture;

// A collectible method cached under System.Object exposed the Members cleanup bug.
public static class CacheExtensions
{
    public static int PackageCacheProbe(this object value) => 42;
}

public class InheritedCacheProbe { }
