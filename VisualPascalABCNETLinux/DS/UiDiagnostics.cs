using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPascalABC
{
    internal static class UiDiagnostics
    {
        internal static void Print()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Console.WriteLine("Runtime: " + Environment.Version + "; Mono=" + (Type.GetType("Mono.Runtime") != null));
                Console.WriteLine("Session: " + Environment.GetEnvironmentVariable("XDG_SESSION_TYPE"));
                Console.WriteLine("Screen: " + Screen.PrimaryScreen.Bounds);
                using (var form = new Form())
                using (var graphics = form.CreateGraphics())
                using (var menu = new MenuStrip())
                {
                    Console.WriteLine("Graphics DPI: " + graphics.DpiX + " x " + graphics.DpiY);
                    PrintFont(graphics, "System menu", SystemFonts.MenuFont);
                    PrintFont(graphics, "MenuStrip default", menu.Font);
                    foreach (string name in new[] { "Tahoma", "Consolas", "Courier New" })
                    using (var font = new Font(name, 12f))
                        PrintFont(graphics, name + " requested at 12pt", font);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Environment.ExitCode = 1;
            }
        }

        private static void PrintFont(Graphics graphics, string label, Font font)
        {
            var flags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;
            var size = new Size(int.MaxValue, int.MaxValue);
            Console.WriteLine(label + ": family=" + font.FontFamily.Name + "; size=" + font.Size + "; unit=" + font.Unit +
                "; Font.Height=" + font.Height + "; GetHeight=" + font.GetHeight() +
                "; GetHeight(graphics)=" + font.GetHeight(graphics) +
                "; MeasureText=" + TextRenderer.MeasureText("Ag_", font, size, flags) +
                "; MeasureText(graphics)=" + TextRenderer.MeasureText(graphics, "Ag_", font, size, flags) +
                "; MeasureString=" + graphics.MeasureString("Ag_", font));
        }
    }
}
