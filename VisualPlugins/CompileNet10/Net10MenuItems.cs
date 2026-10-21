using System.Windows.Forms;
using PascalABCCompiler;

namespace VisualPascalABCPlugins
{
    internal static class Net10MenuItems
    {
        internal static ToolStripMenuItem CreatePublishItem()
        {
            // Register the key, not its current translation: IDE startup/options
            // can select a different language after the plugin creates its menu.
            var item = new ToolStripMenuItem("PUBLISH_NET10") { Name = "CompileNet10Publish" };
            StringResources.SetTextForObject(item, "VP_VEC_");
            return item;
        }
    }
}
