using System.Collections.Generic;

namespace ReflectionCacheUnloadFixture
{
    public static class CacheExtensions
    {
        public static int FullCacheProbe(this string value) => value.Length;
        public static int FullCacheProbe(this IEnumerable<int> value) => 1;
        public static int FullArrayCacheProbe<T>(this T[] value) => value.Length;
    }
}
