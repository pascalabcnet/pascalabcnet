namespace ReflectionCacheUnloadCommon
{
    public sealed class CommonType
    {
        public ReflectionCacheUnloadLeaf.LeafType Leaf { get; } = new();
        public int VersionTwoOnly() => 505;
    }
}
