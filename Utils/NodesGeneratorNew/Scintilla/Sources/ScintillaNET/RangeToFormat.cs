#region Using Directives

using System;
using System.Runtime.InteropServices;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Struct used for passing parameters to FormatRange()
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RangeToFormat
    {
        /// <summary>
        ///     The HDC (device context) we print to
        /// </summary>
        public IntPtr hdc;

        /// <summary>
        ///     The HDC we use for measuring (may be same as hdc)
        /// </summary>
        public IntPtr hdcTarget;

        /// <summary>
        ///     Rectangle in which to print
        /// </summary>
        public PrintRectangle rc;

        /// <summary>
        ///     Physically printable page size
        /// </summary>
        public PrintRectangle rcPage;

        /// <summary>
        ///     Range of characters to print
        /// </summary>
        public CharacterRange chrg;
    }
}
