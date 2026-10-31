using System;
using System.Collections.Generic;
using System.Drawing;
using VisualPascalABCPlugins;

public class GoodVisualPascalABCPlugin : IExtendedVisualPascalABCPlugin
{
    public int Thread;
    public int Clicks;
    public int AfterGuiCalls;
    private PluginGUIItem guiItem;
    public void AfterAddInGUI()
    {
        if (guiItem.menuItem == null || guiItem.toolStripButton == null)
            throw new Exception("GUI must be registered before AfterAddInGUI");
        AfterGuiCalls++;
    }
    public GoodVisualPascalABCPlugin(IWorkbench workbench) { Thread = System.Threading.Thread.CurrentThread.ManagedThreadId; }
    public string Name { get { return "Linux plugin test"; } }
    public string Version { get { return "1"; } }
    public string Copyright { get { return ""; } }
    public virtual void GetGUI(List<IPluginGUIItem> menu, List<IPluginGUIItem> toolbar)
    {
        if (Thread != System.Threading.Thread.CurrentThread.ManagedThreadId) throw new Exception("Wrong GUI thread");
        var item = new PluginGUIItem("Plugin test", "", null, Color.Transparent, () => Clicks++);
        guiItem = item;
        menu.Add(item);
        toolbar.Add(item);
    }
}

public class BrokenConstructorVisualPascalABCPlugin : GoodVisualPascalABCPlugin
{
    public BrokenConstructorVisualPascalABCPlugin(IWorkbench workbench) : base(workbench) { throw new Exception("Broken constructor"); }
}

public class BrokenGuiVisualPascalABCPlugin : GoodVisualPascalABCPlugin
{
    public BrokenGuiVisualPascalABCPlugin(IWorkbench workbench) : base(workbench) { }
    public override void GetGUI(List<IPluginGUIItem> menu, List<IPluginGUIItem> toolbar) { throw new Exception("Broken GUI"); }
}

public class UnrelatedVisualPascalABCPlugin
{
    public UnrelatedVisualPascalABCPlugin(IWorkbench workbench) { throw new Exception("Must not construct an incompatible plugin"); }
}

public abstract class AbstractVisualPascalABCPlugin : GoodVisualPascalABCPlugin
{
    protected AbstractVisualPascalABCPlugin(IWorkbench workbench) : base(workbench) { }
}
