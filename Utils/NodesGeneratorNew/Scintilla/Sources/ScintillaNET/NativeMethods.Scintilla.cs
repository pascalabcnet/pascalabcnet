#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

#endregion Using Directives


namespace ScintillaNET
{
    partial class NativeMethods
    {
        #region Constants

        public const int
            ANNOTATION_HIDDEN = 0,
            ANNOTATION_STANDARD = 1,
            ANNOTATION_BOXED = 2;

        public const int
            SC_CACHE_NONE = 0,
            SC_CACHE_CARET = 1,
            SC_CACHE_PAGE = 2,
            SC_CACHE_DOCUMENT = 3;

        public const int
            SC_MARK_CIRCLE = 0,
            SC_MARK_ROUNDRECT = 1,
            SC_MARK_ARROW = 2,
            SC_MARK_SMALLRECT = 3,
            SC_MARK_SHORTARROW = 4,
            SC_MARK_EMPTY = 5,
            SC_MARK_ARROWDOWN = 6,
            SC_MARK_MINUS = 7,
            SC_MARK_PLUS = 8,
            SC_MARK_VLINE = 9,
            SC_MARK_LCORNER = 10,
            SC_MARK_TCORNER = 11,
            SC_MARK_BOXPLUS = 12,
            SC_MARK_BOXPLUSCONNECTED = 13,
            SC_MARK_BOXMINUS = 14,
            SC_MARK_BOXMINUSCONNECTED = 15,
            SC_MARK_LCORNERCURVE = 16,
            SC_MARK_TCORNERCURVE = 17,
            SC_MARK_CIRCLEPLUS = 18,
            SC_MARK_CIRCLEPLUSCONNECTED = 19,
            SC_MARK_CIRCLEMINUS = 20,
            SC_MARK_CIRCLEMINUSCONNECTED = 21,
            SC_MARK_BACKGROUND = 22,
            SC_MARK_DOTDOTDOT = 23,
            SC_MARK_ARROWS = 24,
            SC_MARK_PIXMAP = 25,
            SC_MARK_FULLRECT = 26,
            SC_MARK_LEFTRECT = 27,
            SC_MARK_AVAILABLE = 28,
            SC_MARK_UNDERLINE = 29,
            SC_MARK_RGBAIMAGE = 30,
            SC_MARK_CHARACTER = 10000;

        public const int
            SC_WRAP_NONE = 0,
            SC_WRAP_WORD = 1,
            SC_WRAP_CHAR = 2;

        public const int
            SC_WRAPINDENT_FIXED = 0,
            SC_WRAPINDENT_SAME = 1,
            SC_WRAPINDENT_INDENT = 2;

        public const int
            SC_MOD_CHANGEANNOTATION = 0x20000;

        public const int
            SC_WRAPVISUALFLAG_NONE = 0x0000,
            SC_WRAPVISUALFLAG_END = 0x0001,
            SC_WRAPVISUALFLAG_START=  0x0002;

        public const int
            SC_WRAPVISUALFLAGLOC_DEFAULT = 0x0000,
            SC_WRAPVISUALFLAGLOC_END_BY_TEXT = 0x0001,
            SC_WRAPVISUALFLAGLOC_START_BY_TEXT = 0x0002;

        public const int
            SCEN_CHANGE = 768;

        public const int
            SCI_GETCURLINE = 2027,
            SCI_GETSELECTIONSTART = 2143,
            SCI_GETSELECTIONEND = 2145,
            SCI_GETFIRSTVISIBLELINE = 2152,
            SCI_GETLINECOUNT = 2154,
            SCI_POSITIONFROMLINE = 2167,
            SCI_CANPASTE = 2173,
            SCI_CUT = 2177,
            SCI_COPY = 2178,
            SCI_PASTE = 2179,
            SCI_GETDIRECTPOINTER = 2185,
            SCI_SETTARGETSTART = 2190,
            SCI_SETTARGETEND = 2192,
            SCI_WRAPCOUNT = 2235,
            SCI_SETWRAPMODE = 2268,
            SCI_GETWRAPMODE = 2269,
            SCI_SETLAYOUTCACHE = 2272,
            SCI_GETLAYOUTCACHE = 2273,
            SCI_LINESJOIN = 2288,
            SCI_LINESSPLIT = 2289,
            SCI_COPYRANGE = 2419,
            SCI_SETWRAPVISUALFLAGS = 2460,
            SCI_GETWRAPVISUALFLAGS = 2461,
            SCI_SETWRAPVISUALFLAGSLOCATION = 2462,
            SCI_GETWRAPVISUALFLAGSLOCATION = 2463,
            SCI_SETWRAPSTARTINDENT = 2464,
            SCI_GETWRAPSTARTINDENT = 2465,
            SCI_SETPASTECONVERTENDINGS = 2467,
            SCI_GETPASTECONVERTENDINGS = 2468,
            SCI_SETWRAPINDENTMODE = 2472,
            SCI_GETWRAPINDENTMODE = 2473,
            SCI_SETPOSITIONCACHE = 2514,
            SCI_GETPOSITIONCACHE = 2515,
            SCI_COPYALLOWLINE = 2519,
            SCI_ANNOTATIONSETTEXT = 2540,
            SCI_ANNOTATIONGETTEXT = 2541,
            SCI_ANNOTATIONSETSTYLE = 2542,
            SCI_ANNOTATIONGETSTYLE = 2543,
            SCI_ANNOTATIONSETSTYLES = 2544,
            SCI_ANNOTATIONGETSTYLES = 2545,
            SCI_ANNOTATIONGETLINES = 2546,
            SCI_ANNOTATIONCLEARALL = 2547,
            SCI_ANNOTATIONSETVISIBLE = 2548,
            SCI_ANNOTATIONGETVISIBLE = 2549,
            SCI_ANNOTATIONSETSTYLEOFFSET = 2550,
            SCI_ANNOTATIONGETSTYLEOFFSET = 2551,
            SCI_SETFIRSTVISIBLELINE = 2613;

        public const int
            SCN_HOTSPOTCLICK = 2019,
            SCN_HOTSPOTDOUBLECLICK = 2020,
            SCN_HOTSPOTRELEASECLICK = 2027;

        #endregion Constants


        #region Callbacks

        public delegate IntPtr Scintilla_DirectFunction(
            IntPtr sci,
            int iMessage,
            IntPtr wParam,
            IntPtr lParam);

        #endregion Callbacks


        #region Structures

        [StructLayout(LayoutKind.Sequential)]
        public struct Sci_NotifyHeader
        {
            public IntPtr hwndFrom;
            public IntPtr idFrom;
            public int code;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SCNotification
        {
            public Sci_NotifyHeader nmhdr;
            public int position;
            public int ch;
            public int modifiers;
            public int modificationType;
            public IntPtr text;
            public int length;
            public int linesAdded;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int line;
            public int foldLevelNow;
            public int foldLevelPrev;
            public int margin;
            public int listType;
            public int x;
            public int y;
            public int token;
            public int annotationLinesAdded;
            public int updated;
        }

        #endregion Structures
    }
}
