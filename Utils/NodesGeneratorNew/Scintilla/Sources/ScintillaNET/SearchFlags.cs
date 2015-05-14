#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    [Flags]
    public enum SearchFlags
    {
        Empty       = 0,
        WholeWord   = 2,
        MatchCase   = 4,
        WordStart   = 0x00100000,
        RegExp      = 0x00200000,
        Posix       = 0x00400000
    }
}
