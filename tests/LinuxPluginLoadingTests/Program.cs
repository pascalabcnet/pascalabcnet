using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using VisualPascalABCPlugins;

internal static class Program
{
    [STAThread]
    private static int Main()
    {
        try
        {
            string root = Path.Combine(Path.GetTempPath(), "pabc-linux-plugins-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(root);
            File.Copy(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "LinuxPluginFixtures.dll"),
                Path.Combine(root, "Fixtures.dll"));
            File.WriteAllText(Path.Combine(root, "not-managed.dll"), "not a managed library");
            using (var menu = new ToolStripMenuItem())
            using (var toolbar = new ToolStrip())
            {
                var loader = new PluginsController(null, menu, toolbar, null);
                loader.AddPlugins(root);
                Check(loader.Plugins.Count == 1 && loader.Plugins[0].Name == "Linux plugin test", "compatible plugin loads despite broken plugins");
                Check(loader.ErrorList.Count == 2, "constructor and GetGUI errors are recorded; unrelated/abstract types are skipped");
                Check(menu.DropDownItems.Count == 1 && toolbar.Items.Count == 1, "plugin menu and toolbar are registered synchronously");
                var plugin = loader.Plugins[0];
                Check((int)plugin.GetType().GetField("AfterGuiCalls").GetValue(plugin) == 1, "extended plugin receives AfterAddInGUI after controls are registered");
                int thread = (int)plugin.GetType().GetField("Thread").GetValue(plugin);
                Check(thread == Thread.CurrentThread.ManagedThreadId, "constructor and GetGUI run on the caller UI thread");
                menu.DropDownItems[0].PerformClick();
                Check((int)plugin.GetType().GetField("Clicks").GetValue(plugin) == 1, "menu action works");
                toolbar.Items[0].PerformClick();
                Check((int)plugin.GetType().GetField("Clicks").GetValue(plugin) == 2, "toolbar action works");
            }
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }

    private static void Check(bool value, string message)
    {
        if (!value) throw new Exception(message);
        Console.WriteLine("PASS: " + message);
    }
}
