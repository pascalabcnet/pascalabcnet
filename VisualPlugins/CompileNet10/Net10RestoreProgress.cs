using System;

namespace VisualPascalABCPlugins
{
    internal sealed class Net10PackageRestoreRequired
    {
        public string uri { get; set; }
        public bool required { get; set; }
    }

    internal sealed class Net10RestoreProgress
    {
        public string operationId { get; set; }
        public string stage { get; set; }
        public string packageId { get; set; }
        public int installedCount { get; set; }
        public long elapsedMilliseconds { get; set; }
        public bool cancelled { get; set; }

        internal string Format(Func<string, string> resource) => installedCount == 0
            ? string.Format(resource("NUGET_RESOLVING{0}"), elapsedMilliseconds / 1000)
            : string.Format(resource("NUGET_INSTALLED{0}{1}{2}"), installedCount,
                packageId ?? "", elapsedMilliseconds / 1000);
    }
}
