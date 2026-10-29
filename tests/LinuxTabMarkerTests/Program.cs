using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

internal static class Program
{
    private static void Check(bool value, string message)
    {
        if (!value) throw new Exception(message);
        Console.WriteLine("PASS " + message);
    }

    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            string path = Path.GetFullPath(args.Length == 0 ? "bin/WeifenLuo.WinFormsUI.Docking.ThemeVS2005Linux.dll" : args[0]);
            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) => {
                string file = Path.Combine(Path.GetDirectoryName(path), new AssemblyName(e.Name).Name + ".dll");
                return File.Exists(file) ? Assembly.LoadFrom(file) : null;
            };
            Type strip = Assembly.LoadFrom(path).GetType("WeifenLuo.WinFormsUI.Docking.VS2005DockPaneStrip", true);
            MethodInfo hoverBorder = strip.BaseType.Assembly.GetType("WeifenLuo.WinFormsUI.Docking.InertButtonBase", true)
                .GetMethod("ShouldDrawHoverBorder", BindingFlags.NonPublic | BindingFlags.Static);
            Check(!(bool)hoverBorder.Invoke(null, new object[] { true, PlatformID.Unix }),
                "Mono/Linux docking buttons have no black hover border");
            Check((bool)hoverBorder.Invoke(null, new object[] { false, PlatformID.Win32NT }),
                "Windows docking buttons keep their hover border");
            Check((bool)hoverBorder.Invoke(null, new object[] { true, PlatformID.Win32NT }),
                "Mono on Windows keeps its hover border");
            MethodInfo captionFormat = strip.GetMethod("DocumentCaptionFormat", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo markerWidth = strip.GetMethod("DocumentMarkerWidth", BindingFlags.NonPublic | BindingFlags.Static);
            foreach (bool rtl in new[] { false, true })
            {
                var marked = (TextFormatFlags)captionFormat.Invoke(null, new object[] { true, rtl });
                var ordinary = (TextFormatFlags)captionFormat.Invoke(null, new object[] { false, rtl });
                Check((marked & TextFormatFlags.HorizontalCenter) == 0 && (marked & TextFormatFlags.NoPadding) != 0,
                    "marked caption is edge-aligned without hidden glyph padding; rtl=" + rtl);
                Check(((marked & TextFormatFlags.Right) != 0) == rtl, "marker and caption share the same edge; rtl=" + rtl);
                Check((ordinary & TextFormatFlags.HorizontalCenter) != 0 && (ordinary & TextFormatFlags.NoPadding) == 0,
                    "ordinary tabs retain their existing alignment; rtl=" + rtl);
            }
            foreach (float size in new[] { 9f, 11f, 18f, 24f })
            foreach (FontStyle style in new[] { FontStyle.Regular, FontStyle.Bold })
            using (var font = new Font(FontFamily.GenericSansSerif, size, style))
            {
                int diameter = Math.Max(5, font.Height / 3);
                int reserved = (int)markerWidth.Invoke(null, new object[] { font });
                Check(reserved - diameter == Math.Max(2, font.Height / 10),
                    "marker reserves only the small scaled gap; font=" + size + "; style=" + style);
            }
            return 0;
        }
        catch (Exception e) { Console.Error.WriteLine(e); return 1; }
    }
}
