#region Using Directives

using System;
using System.Runtime.InteropServices;

#endregion Using Directives


namespace ScintillaNET
{
    // TODO Make internal
    [StructLayout(LayoutKind.Sequential)]
    public struct TextToFind
    {
        public CharacterRange chrg;         // range to search
        public IntPtr lpstrText;            // the search pattern (zero terminated)
        public CharacterRange chrgText;     // returned as position of matching text
    }
}
