using System;
using System.Drawing;
using System.Windows.Forms;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static void CheckPlatformUI()
    {
        using (var form = new Form { Width = 800, Height = 300, ShowInTaskbar = false, Opacity = 0 })
        using (var strip = new StatusStrip { Name = "statusStrip1" })
        {
            var message = new ToolStripStatusLabel { Name = "toolStripStatusLabel5", Spring = true };
            var line = new ToolStripStatusLabel("Строка 999") { Name = "toolStripStatusLabel1" };
            var column = new ToolStripStatusLabel("Столбец 999");
            strip.Items.AddRange(new ToolStripItem[] { message, line, column });
            form.Controls.Add(strip);form.Show();Application.DoEvents();
            using (var status = new Net10StatusDisplay(form))
            {
                var platform = (ToolStripStatusLabel)strip.Items["Net10PlatformStatus"];
                Check(platform != null && strip.Items.IndexOf(platform) + 2 == strip.Items.IndexOf(line),
                    "platform field precedes row/column coordinates");
                strip.PerformLayout();
                Check(message.Margin.Left == 6 && strip.Items["Net10PlatformDivider"].Bounds.Left - platform.Bounds.Right == 11,
                    "actual item bounds reserve left message space and the gap before divider");
                int width = platform.Width;
                status.StartReady(true, "Готов");strip.PerformLayout();
                Check(platform.Text == ".NET 10" && message.Text == "Готов", "modern platform and ready message use separate fields");
                int x = line.Bounds.X;
                status.StartReady(false, "Готов");strip.PerformLayout();
                Check(platform.Text == ".NET Framework" && platform.Width == width && line.Bounds.X == x,
                    "switching platform keeps coordinate positions stable");
                status.Start(".NET 10: Компиляция прошла успешно");
                Check(message.Text == "Компиляция прошла успешно" && platform.Text == ".NET Framework",
                    "operation messages neither duplicate nor change selected platform");
                Net10PlatformMenu selection = null;
                var modern = new ToolStripMenuItem(".NET 10");
                using (selection = new Net10PlatformMenu(modern, value => {
                    selection.SetSelected(value);status.StartReady(value, "Готов"); }))
                {
                    Check(selection.Root.Text == "Платформа" && selection.Root.DropDownItems.Count == 2 && selection.Legacy.Checked,
                        "platform submenu contains two explicit mutually exclusive choices");
                    modern.PerformClick();modern.PerformClick();
                    Check(modern.Checked && !selection.Legacy.Checked && platform.Text == ".NET 10",
                        "choosing modern twice never toggles back to Framework");
                    selection.Legacy.PerformClick();
                    Check(!modern.Checked && selection.Legacy.Checked && platform.Text == ".NET Framework",
                        "Framework selection updates both menu and persistent status");
                    selection.SetAvailable(true, false);
                    Check(!modern.Enabled && selection.Legacy.Enabled, "missing runtime disables only the modern option");
                    selection.SetAvailable(false, true);
                    Check(!selection.Root.Enabled, "platform selection is disabled during execution or compilation");
                }
                using(var font = new Font(strip.Font.FontFamily, strip.Font.Size + 2))
                { strip.Font = font;Check(platform.Width > width, "platform width follows the status font"); }
            }
            Check(strip.Items["Net10PlatformStatus"] == null, "disposing the plugin removes its platform field");
        }
    }
}
