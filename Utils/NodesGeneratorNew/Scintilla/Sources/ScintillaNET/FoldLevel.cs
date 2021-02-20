#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    // TODO: SHOULD HAVE [Flags] here ?, with Base and NumberMask extracted out?
    /// <summary>
    ///     The flags that are stored along with the fold level. 
    /// </summary>
    public enum FoldLevel
    {
        /// <summary>
        ///     The base value for a 0-level fold. The fold level is a number in the range 0 to 4095 (NumberMask).
        ///     However, the initial fold level is set to Base(1024) to allow unsigned arithmetic on folding levels.
        /// </summary>
        Base = 0x400,

        /// <summary>
        ///     WhiteFlag indicates that the line is blank and allows it to be treated slightly different then its level may
        ///     indicate. For example, blank lines should generally not be fold points and will be considered part
        ///     of the preceding section even though they may have a lesser fold level.
        /// </summary>
        WhiteFlag = 0x1000,

        /// <summary>
        ///     HeaderFlag indicates that the line is a header (fold point).
        /// </summary>
        HeaderFlag = 0x2000,

        /// <summary>
        ///     Not documented by current Scintilla docs - associated with the removed Box fold style?
        /// </summary>
        BoxHeaderFlag = 0x4000,

        BoxFooterFlag = 0x8000,

        Contracted = 0x10000,

        Unindent = 0x20000,

        /// <summary>
        ///     A bit-mask indicating which bits are used to store the actual fold level.
        /// </summary>
        NumberMask = 0x0FFF,
    }
}
