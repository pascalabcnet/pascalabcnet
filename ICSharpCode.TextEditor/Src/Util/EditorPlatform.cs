using System;

namespace ICSharpCode.TextEditor
{
    internal static class EditorPlatform
    {
        internal static readonly bool IsMonoUnix = Type.GetType("Mono.Runtime") != null &&
            (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX);
    }
}
