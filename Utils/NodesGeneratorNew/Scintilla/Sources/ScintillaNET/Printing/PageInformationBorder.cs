#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Type of border to print for a Page Information section
    /// </summary>
    public enum PageInformationBorder
    {
        /// <summary>
        ///     No border
        /// </summary>
        None,

        /// <summary>
        ///     Border along the top
        /// </summary>
        Top,

        /// <summary>
        ///     Border along the bottom
        /// </summary>
        Bottom,

        /// <summary>
        ///     A full border around the page information section
        /// </summary>
        Box
    }
}
