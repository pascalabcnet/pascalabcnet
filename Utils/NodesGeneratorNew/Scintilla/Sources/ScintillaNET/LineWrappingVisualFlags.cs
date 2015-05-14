#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Specifies how line wrapping visual glyphs are displayed in a <see cref="Scintilla" /> control.
    /// </summary>
    [Flags]
    public enum LineWrappingVisualFlags
    {
        /// <summary>
        ///     No line wrapping glyphs are displayed.
        /// </summary>
        None = NativeMethods.SC_WRAPVISUALFLAG_NONE,

        /// <summary>
        ///     Line wrapping glyphs are displayed at the end of wrapped lines.
        /// </summary>
        End = NativeMethods.SC_WRAPVISUALFLAG_END,

        /// <summary>
        ///     Line wrapping glyphs are displayed at the start of wrapped lines. This also has
        ///     the effect of indenting the line by one additional unit to accommodate the glyph.
        /// </summary>
        Start = NativeMethods.SC_WRAPVISUALFLAG_START,
    }
}
