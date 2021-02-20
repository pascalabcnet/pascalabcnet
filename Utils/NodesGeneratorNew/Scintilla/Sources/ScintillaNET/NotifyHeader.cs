#region Using Directives

using System;
using System.Runtime.InteropServices;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     This matches the Win32 NMHDR structure
    /// </summary>
    [Obsolete("This type will not be public in future versions.")]
    [StructLayout(LayoutKind.Sequential)]
    public struct NotifyHeader
    {
        public IntPtr hwndFrom; // environment specific window handle/pointer
        public IntPtr idFrom;   // CtrlID of the window issuing the notification
        public uint code;       // The SCN_* notification code
    }
}
