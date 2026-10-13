namespace ReflectionCacheUnloadCommon
{
    public sealed class CommonType
    {
        public ReflectionCacheUnloadLeaf.LeafType Leaf { get; } = new();
        public int VersionOneOnly() => 404;
    }
}
