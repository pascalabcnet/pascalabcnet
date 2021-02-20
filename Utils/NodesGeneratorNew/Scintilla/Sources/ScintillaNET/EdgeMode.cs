#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     How long lines are visually indicated
    /// </summary>
    public enum EdgeMode
    {
        /// <summary>
        ///     No indication
        /// </summary>
        None = 0,

        /// <summary>
        ///     A vertical line is displayed
        /// </summary>
        Line = 1,

        /// <summary>
        ///     The background color changes
        /// </summary>
        Background = 2,
    }
}
