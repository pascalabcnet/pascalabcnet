namespace ReflectionCacheUnloadFixture
{
    public sealed class FixtureType
    {
        public ReflectionCacheUnloadDependency.DependencyType Dependency { get; } = new();
        public ReflectionCacheUnloadCommon.CommonType Common { get; } = new();

        public int Echo(int value)
        {
            return value;
        }

        public int VersionOneOnly()
        {
            return 1;
        }
    }
}
