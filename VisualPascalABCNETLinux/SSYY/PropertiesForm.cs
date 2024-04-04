using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualPascalABC
{
    public partial class PropertiesForm : DockContent
    {
        public PropertiesForm()
        {
            InitializeComponent();
            TabText = PascalABCCompiler.StringResources.Get("VP_MF_M_PROPERTIES");
            componentsCombo.ImageList = new ImageList();
            //componentsCombo.
        }

        public PropertyGrid Properties
        {
            get
            {
                return propertyGrid1;
            }
        }

//        public Dictionary<Type, int> componentBitmaps = new Dictionary<Type, int>();
//        public Dictionary<IComponent, NETXP.Controls.ComboBoxExItem> componentItems = new Dictionary<IComponent, NETXP.Controls.ComboBoxExItem>();
//        public Dictionary<NETXP.Controls.ComboBoxExItem, IComponent> itemsComponent = new Dictionary<NETXP.Controls.ComboBoxExItem, IComponent>();

//        public static string getComboText(IComponent comp)
//        {
//            if (comp.Site != null)
//            {
//                return comp.Site.Name + ": " + comp.GetType().FullName;
//            }
//            else
//            {
//                return comp.GetType().FullName;
//            }
//        }

//        public void AddDesignedComponent(IComponent comp)
//        {
//            int num = -1;
//            Type type = comp.GetType();
//            if (!componentBitmaps.TryGetValue(type, out num))
//            {
//                ToolboxBitmapAttribute tba = TypeDescriptor.GetAttributes(type)[typeof(ToolboxBitmapAttribute)] as ToolboxBitmapAttribute;
//                if (tba == null)
//                {
//                    num = -1;
//                }
//                else
//                {
//                    num = componentsCombo.ImageList.Images.Count;
//                    componentsCombo.ImageList.Images.Add(tba.GetImage(type));
//                }
//                componentBitmaps.Add(type, num);
//            }
//            NETXP.Controls.ComboBoxExItem it = new NETXP.Controls.ComboBoxExItem(num, getComboText(comp), 0);
//            componentsCombo.Items.Add(it);
//            componentItems.Add(comp, it);
//            itemsComponent.Add(it, comp);
//        }

//        public void DeleteDesignedComponent(IComponent comp)
//        {
//            NETXP.Controls.ComboBoxExItem it;
//            bool exist = componentItems.TryGetValue(comp, out it);
//            if (!exist) return;
//            componentsCombo.Items.Remove(it);
//            itemsComponent.Remove(componentItems[comp]);
//            componentItems.Remove(comp);
//        }

//        public void SetSelectedComponents(object[] selection)
//        {
//            switch (selection.Length)
//            {
//                case 0:
//                    componentsCombo.SelectedItem = null;
////                    componentsCombo.SelectedText = Form1StringResources.Get("NOTHING_SELECTED");
//                    break;
//                case 1:
//                    componentsCombo.SelectedItem = componentItems[selection[0] as IComponent];
//                    break;
//                default:
////                    string s = string.Format(Form1StringResources.Get("{0}_COMPONENTS5_SELECTED"), selection.Length);
//                    componentsCombo.SelectedItem = null;
//                    break;
//            }
//        }

        private void componentsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
//            if (componentsCombo.SelectedItem == null) return;
//            IComponent comp = itemsComponent[componentsCombo.SelectedItem as NETXP.Controls.ComboBoxExItem];
//            Form1.Form1_object.CurrentCodeFileDocument.Designer.host.selectionService.SetSelectedComponents(new IComponent[]{comp});
//            //SampleDesignerHost.SampleSelectionService
        }

//        public NETXP.Controls.ComboBoxExItemCollection GetItemsControls()
//        {
//            return componentsCombo.Items;
//        }

        public Panel GetComponentsComboPanel()
        {
            return componentsComboPanel;
        }

        public NETXP.Controls.ComboBoxEx GetComponentsCombo()
        {
            return componentsCombo;
        }

    }

    public class MyGrid : DataGrid
    {
        public override ISite Site
        {
            get
            {
               return base.Site;
            }
            set
            {
                base.Site = value;
            }
        }
    }

}