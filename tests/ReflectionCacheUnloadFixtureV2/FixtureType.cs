namespace ReflectionCacheUnloadFixture
{
    public sealed class FixtureType
    {
        public ReflectionCacheUnloadDependency.DependencyType Dependency { get; } = new();
        public ReflectionCacheUnloadCommon.CommonType Common { get; } = new();

        public int Echo(int value)
        {
            return value * 2;
        }

        public int VersionTwoOnly()
        {
            return 2;
        }
    }
}
