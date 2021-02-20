#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     The flags affecting how the fold is marked in the main text area (as well as in the margin). If the value
    ///     changes for onscreen text, the display will redraw.
    /// </summary>
    [Flags]
    public enum FoldFlag
    {
        /// <summary>
        ///     A line is drawn above the text if the fold is expanded.
        /// </summary>
        LineBeforeExpanded = 0x0002,

        /// <summary>
        ///     A line is drawn above the text if the fold is collapsed.
        /// </summary>
        LineBeforeContracted = 0x0004,

        /// <summary>
        ///     A line is drawn below the text if the fold is expanded.
        /// </summary>
        LineAfterExpanded = 0x0008,

        /// <summary>
        ///     A line is drawn below the text if the fold is collapsed.
        /// </summary>
        LineAfterContracted = 0x0010,

        /// <summary>
        ///     Display hexadecimal fold levels in line margin to aid debugging of folding. The appearance of this feature may change in the future.
        /// </summary>
        LevelNumbers = 0x0040,

        /// <summary>
        ///     Experimental feature that has been removed.
        /// </summary>
        Box = 0x0001,
    }
}
