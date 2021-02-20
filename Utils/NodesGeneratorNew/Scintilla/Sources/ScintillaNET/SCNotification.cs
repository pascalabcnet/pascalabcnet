#region Using Directives

using System;
using System.Runtime.InteropServices;

#endregion Using Directives


namespace ScintillaNET
{
    [Obsolete("This type will not be public in future versions.")]
    [StructLayout(LayoutKind.Sequential)]
    public struct SCNotification
    {
        public NotifyHeader nmhdr;
        public int position;            // SCN_STYLENEEDED, SCN_MODIFIED, SCN_DWELLSTART, SCN_DWELLEND, 
                                        // SCN_CALLTIPCLICK, SCN_HOTSPOTCLICK, SCN_HOTSPOTDOUBLECLICK
        public char ch;                 // SCN_CHARADDED, SCN_KEY
        public int modifiers;           // SCN_KEY
        public int modificationType;    // SCN_MODIFIED
        public IntPtr text;             // SCN_MODIFIED
        public int length;              // SCN_MODIFIED
        public int linesAdded;          // SCN_MODIFIED
        public int message;             // SCN_MACRORECORD
        public IntPtr wParam;           // SCN_MACRORECORD
        public IntPtr lParam;           // SCN_MACRORECORD
        public int line;                // SCN_MODIFIED
        public int foldLevelNow;        // SCN_MODIFIED
        public int foldLevelPrev;       // SCN_MODIFIED
        public int margin;              // SCN_MARGINCLICK
        public int listType;            // SCN_USERLISTSELECTION
        public int x;                   // SCN_DWELLSTART, SCN_DWELLEND
        public int y;                   // SCN_DWELLSTART, SCN_DWELLEND
    }
}
