#region Using Directives

using System;
using System.Runtime.InteropServices;
using System.Text;

#endregion Using Directives


namespace ScintillaNET
{
    internal static partial class NativeMethods
    {
        #region Constants

        private const string
            DLL_NAME_GDI32 = "gdi32.dll",
            DLL_NAME_KERNEL32 = "kernel32.dll",
            DLL_NAME_OLE32 = "ole32.dll",
            DLL_NAME_USER32 = "user32.dll";

        public const int
            RGN_AND = 1;

        public const int
            WM_DESTROY = 0x0002,
            WM_GETTEXT = 0x000D,
            WM_GETTEXTLENGTH = 0x000E,
            WM_PAINT = 0x000F,
            WM_SETCURSOR = 0x0020,
            WM_NOTIFY = 0x004E,
            WM_NCPAINT = 0x0085,
            WM_COMMAND = 0x0111,
            WM_HSCROLL = 0x114,
            WM_VSCROLL = 0x115,
            WM_USER = 0x0400,
            WM_REFLECT = WM_USER + 0x1C00;

        public const int
            WS_BORDER = 0x00800000;

        public const int
            WS_EX_CLIENTEDGE = 0x00000200;

        #endregion Constants


        #region Fields

        internal static readonly IntPtr HWND_MESSAGE = new IntPtr(-3);

        #endregion Fields


        #region Structures

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        #endregion Structures


        #region Functions

        [DllImport(DLL_NAME_GDI32, ExactSpelling = true)]
        public static extern int CombineRgn(
            IntPtr hrgnDest,
            IntPtr hrgnSrc1,
            IntPtr hrgnSrc2,
            int fnCombineMode);

        [DllImport(DLL_NAME_GDI32, ExactSpelling = true)]
        public static extern IntPtr CreateRectRgn(
            int x1,
            int y1,
            int x2,
            int y2);

        [DllImport(DLL_NAME_KERNEL32, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr GetProcAddress(
            IntPtr hModule,
            string procName);

        [DllImport(DLL_NAME_USER32, ExactSpelling = true)]
        public static extern bool GetUpdateRect(
            IntPtr hWnd,
            out ScintillaNET.RECT lpRect,
            bool bErase);

        [DllImport(DLL_NAME_USER32, ExactSpelling = true)]
        public static extern IntPtr GetWindowDC(
            IntPtr hWnd);

        [DllImport(DLL_NAME_USER32, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(
            IntPtr hWnd,
            out RECT lpRect);

        [DllImport(DLL_NAME_KERNEL32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LoadLibrary(
            string lpLibFileName);

        [DllImport(DLL_NAME_USER32, ExactSpelling = true)]
        public static extern int ReleaseDC(
            IntPtr hWnd,
            IntPtr hDC);

        [DllImport(DLL_NAME_OLE32, ExactSpelling = true)]
        public static extern int RevokeDragDrop(
            IntPtr hwnd);

        [DllImport(DLL_NAME_USER32, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(
            IntPtr hWnd,
            int Msg,
            IntPtr wParam,
            IntPtr lParam);

        [DllImport(DLL_NAME_USER32, ExactSpelling = true)]
        public static extern IntPtr SetParent(
            IntPtr hWndChild,
            IntPtr hWndNewParent);

        #endregion Functions
    }
}
