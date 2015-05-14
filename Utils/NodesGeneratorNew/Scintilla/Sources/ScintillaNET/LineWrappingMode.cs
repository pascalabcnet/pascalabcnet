#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Specifies the line wrapping modes that can be applied to a <see cref="Scintilla" /> control.
    /// </summary>
    public enum LineWrappingMode
    {
        /// <summary>
        ///     Line wrapping is disabled.
        /// </summary>
        None = NativeMethods.SC_WRAP_NONE,

        /// <summary>
        ///     Lines wrap on word boundaries.
        /// </summary>
        Word = NativeMethods.SC_WRAP_WORD,

        /// <summary>
        ///     Lines wrap between characters.
        /// </summary>
        Char = NativeMethods.SC_WRAP_CHAR,
    }
}
