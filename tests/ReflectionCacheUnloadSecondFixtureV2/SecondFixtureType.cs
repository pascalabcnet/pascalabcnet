namespace ReflectionCacheUnloadSecondFixture
{
    public sealed class SecondFixtureType
    {
        public ReflectionCacheUnloadSecondDependency.SecondDependencyType Dependency { get; } = new();
        public ReflectionCacheUnloadCommon.CommonType Common { get; } = new();
    }
}
