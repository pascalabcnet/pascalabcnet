#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Style of Indicator to be displayed
    /// </summary>
    public enum IndicatorStyle
    {
        /// <summary>
        ///     Underline
        /// </summary>
        Plain = 0,

        /// <summary>
        ///     Squigly lines (commonly used for spellcheck)
        /// </summary>
        Squiggle = 1,

        /// <summary>
        ///     Small t's are displayed
        /// </summary>
        TT = 2,

        /// <summary>
        ///     Small diagnol lines
        /// </summary>
        Diagonal = 3,

        /// <summary>
        ///     Strikethrough line
        /// </summary>
        Strike = 4,

        /// <summary>
        ///     Hidden
        /// </summary>
        Hidden = 5,

        /// <summary>
        ///     Displayes a bounding box around the indicated text
        /// </summary>
        Box = 6,

        /// <summary>
        ///     Displayes a bounding box around the indicated text with rounded corners
        ///     and an translucent background color
        /// </summary>
        RoundBox = 7,
    }
}
