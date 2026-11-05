#if !PABC_LINUX_PLUGIN
using System;
using System.Windows.Forms;
namespace VisualPascalABCPlugins
{
    internal sealed class Net10PlatformMenu : IDisposable
    {
        internal ToolStripMenuItem Root { get; } = new ToolStripMenuItem("Платформа") { Name = "CompilePlatformMenu" };
        internal ToolStripMenuItem Legacy { get; } = new ToolStripMenuItem(".NET Framework") { Name = "CompileFrameworkPlatform", Checked = true };
        private readonly ToolStripMenuItem modern;
        internal Net10PlatformMenu(ToolStripMenuItem modern, Action<bool> select)
        {
            this.modern = modern;
            Root.DropDownItems.Add(modern); Root.DropDownItems.Add(Legacy);
            modern.Click += (sender, e) => select(true);
            Legacy.Click += (sender, e) => select(false);
        }
        internal void SetSelected(bool value) { modern.Checked = value; Legacy.Checked = !value; }
        internal void SetAvailable(bool idle, bool runtimeAvailable)
        { Root.Enabled = idle; modern.Enabled = idle && runtimeAvailable; Legacy.Enabled = idle; }
        public void Dispose() => Root.Dispose();
    }
}
#endif
