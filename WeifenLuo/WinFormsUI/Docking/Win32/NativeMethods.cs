using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using WeifenLuo.WinFormsUI.Docking.Win32;

namespace WeifenLuo.WinFormsUI.Docking
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public POINT(System.Drawing.Point pt) : this(pt.X, pt.Y) { }

        public static implicit operator System.Drawing.Point(POINT p)
        {
            return new System.Drawing.Point(p.X, p.Y);
        }

        public static implicit operator POINT(System.Drawing.Point p)
        {
            return new POINT(p.X, p.Y);
        }
    }

    internal static class NativeMethods
    {
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DragDetect(IntPtr hWnd, POINT pt);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetFocus();

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int X, int Y, int Width, int Height, FlagsSetWindowPos flags);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowLong(IntPtr hWnd, int Index);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SetWindowLong(IntPtr hWnd, int Index, int Value);

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern int ShowScrollBar(IntPtr hWnd, int wBar, int bShow);

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
        //*********************************
        // FxCop bug, suppress the message
        //*********************************
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
		public static extern IntPtr WindowFromPoint(Point point);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetCurrentThreadId();

        public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(Win32.HookType code, HookProc func, IntPtr hInstance, int threadID);

        [DllImport("user32.dll")]
        public static extern int UnhookWindowsHookEx(IntPtr hhook);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wParam, IntPtr lParam);

        public static bool IsUnix()
        {
            return System.Environment.OSVersion.Platform == System.PlatformID.Unix || System.Environment.OSVersion.Platform == System.PlatformID.MacOSX;
        }
	}
}