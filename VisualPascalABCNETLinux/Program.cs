// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VisualPascalABC
{
    struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public uint cbData;
        public string lpData;
    }

    static class VisualPascalABCProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static string[] CommandLineArgs;
        public static Form1 MainForm;
        public static Process CurrentProcess;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [DllImport("User32.dll")]
        static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("User32.dll")]
        static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("User32.dll")]
        static extern int SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll")]
        static extern int IsZoomed(IntPtr hWnd);
        [DllImport("User32.dll")]
        static extern int IsIconic(IntPtr hWnd);

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Process cur_proc = Process.GetCurrentProcess();
                CurrentProcess = cur_proc;
                Process[] p = Process.GetProcessesByName(cur_proc.ProcessName);
                IntPtr handle = IntPtr.Zero;
                if (p.Length > 1)
                {
                    for (int i = 0; i < p.Length; i++)
                        if (string.Compare(System.IO.Path.GetDirectoryName(p[i].MainModule.FileName), System.IO.Path.GetDirectoryName(cur_proc.MainModule.FileName), true) == 0 && p[i].Id != cur_proc.Id)
                        {
                            handle = p[i].MainWindowHandle;
                            /*if (handle == IntPtr.Zero)
                            {
                                System.Threading.Thread.Sleep(5000);
                                p[i].Refresh();
                                if (!p[i].HasExited)
                                    handle = p[i].MainWindowHandle;
                            }*/
                            if (handle != IntPtr.Zero)
                            {
                                SetForegroundWindow(handle);
                                if (IsZoomed(handle) != 0)
                                    ShowWindow(handle, 3/*SW_MAXIMIZE*/);
                                else
                                    if (IsIconic(handle) != 0)
                                    ShowWindow(handle, 9/*SW_RESTORE*/);
                                else
                                    ShowWindow(handle, 5/*SW_SHOW*/);
                                if (args.Length > 0)
                                {
                                    //for (int j = 0; j < args.Length; j++)
                                    {
                                        COPYDATASTRUCT ds = new COPYDATASTRUCT();
                                        IntPtr str_ptr = System.Runtime.InteropServices.Marshal.StringToHGlobalUni(args[0]);
                                        ds.dwData = (IntPtr)32;//flag, zadaem sami, chtoby process znal, kakoe eto konkretno soobshenie
                                        ds.cbData = (uint)(args[0].Length + 1);
                                        ds.lpData = args[0];//str_ptr;
                                        IntPtr ds_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(ds));
                                        Marshal.StructureToPtr(ds, ds_ptr, false);
                                        SendMessage(handle, 74/*WM_COPYDATA*/, cur_proc.Handle, ds_ptr);
                                    }
                                }
                                return;
                            }
                            return;

                        }
                }
            }
            catch
            {

            }
            try
            {

                //if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
                CommandLineArgs = args;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                MainForm = new Form1();
                Application.Run(MainForm);
            }
            catch (Exception e)
            {
                // Ёто ужасно! Ќадо обрабатывать все исключени€ раньше - чтобы оболочка не закрывалась!
                MessageBox.Show(e.ToString());
            }
        }
    }
    
    public class MyForm : Form
    {
    	
    }
}
