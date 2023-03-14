using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.ComponentModel.Design;
using NETXP.Controls.Bars;

namespace SampleDesignerApplication
{
    public partial class ToolBoxPane : UserControl
    {
        private SampleDesignerHost.SampleDesignerHost host;
        private ToolboxItem pointer; // a "null" tool
        private ListBarItem pointerItem;
        private int selectedIndex; // the index of the currently selected tool
        private bool initialPaint = true; // see ToolboxPane_Paint method

        private List<ToolboxItem> listData_ti = new List<ToolboxItem>();
        private List<ToolboxItem> listCustomControls_ti = new List<ToolboxItem>();
        private List<ToolboxItem> listWindowsForms_ti = new List<ToolboxItem>();
        private List<ToolboxItem> listComponents_ti = new List<ToolboxItem>();

        private List<ToolboxItem>[] listAll;
        private bool doubleClicked = false;

        // We load types into our categories in a rather primitive way. It is easier than
        // dealing with type resolution, but we can only do this since our list of tools
        // is standard and unchanging.
        //
        private Type[] windowsFormsToolTypes = new Type[] {
													  typeof(System.Windows.Forms.Button),
													  typeof(System.Windows.Forms.CheckBox),
                                                      typeof(System.Windows.Forms.CheckedListBox),
													  typeof(System.Windows.Forms.ComboBox),
                                                      typeof(System.Windows.Forms.DateTimePicker),
                                                      typeof(System.Windows.Forms.DomainUpDown),
                                                      typeof(System.Windows.Forms.FlowLayoutPanel),
                                                      typeof(System.Windows.Forms.GroupBox),
                                                      typeof(System.Windows.Forms.HScrollBar),
													  typeof(System.Windows.Forms.Label),
                                                      typeof(System.Windows.Forms.LinkLabel),
                                                      typeof(System.Windows.Forms.ListBox),
                                                      typeof(System.Windows.Forms.ListView),
                                                      typeof(System.Windows.Forms.MaskedTextBox),
                                                      typeof(System.Windows.Forms.MonthCalendar),
                                                      typeof(System.Windows.Forms.NotifyIcon),
                                                      typeof(System.Windows.Forms.NumericUpDown),
                                                      typeof(System.Windows.Forms.Panel),
                                                      typeof(System.Windows.Forms.PictureBox),
                                                      
                                                      typeof(System.Windows.Forms.ProgressBar),
                                                      typeof(System.Windows.Forms.PropertyGrid),
                                                      typeof(System.Windows.Forms.RadioButton),
                                                      typeof(System.Windows.Forms.RichTextBox),
                                                      typeof(System.Windows.Forms.SplitContainer),

                                                      typeof(System.Windows.Forms.TabControl),
                                                      typeof(System.Windows.Forms.TableLayoutPanel),
													  typeof(System.Windows.Forms.TextBox),
                                                      typeof(System.Windows.Forms.ToolTip),
                                                      typeof(System.Windows.Forms.TrackBar),
                                                      typeof(System.Windows.Forms.TreeView),
                                                      typeof(System.Windows.Forms.VScrollBar),
                                                      typeof(System.Windows.Forms.WebBrowser),
                                                      typeof(System.Windows.Forms.ContextMenuStrip),
													  typeof(System.Windows.Forms.MenuStrip),
                                                      typeof(System.Windows.Forms.StatusStrip),
                                                      typeof(System.Windows.Forms.ToolStrip),
                                                      typeof(System.Windows.Forms.ToolStripContainer),
                                                      typeof(System.Windows.Forms.ColorDialog),
                                                      typeof(System.Windows.Forms.FontDialog),
                                                      typeof(System.Windows.Forms.FolderBrowserDialog),
                                                      typeof(System.Windows.Forms.OpenFileDialog),
                                                      typeof(System.Windows.Forms.SaveFileDialog),
                                                      
												  };

        private Type[] componentsToolTypes = new Type[] {
                                                   typeof(System.ComponentModel.BackgroundWorker),
                                                   typeof(System.IO.FileSystemWatcher),
                                                   typeof(System.Windows.Forms.HelpProvider),
                                                   typeof(System.Windows.Forms.ImageList),
												   typeof(System.Diagnostics.Process),
                                                   typeof(System.IO.Ports.SerialPort),
												   typeof(System.Timers.Timer)
											   };

        private Type[] dataToolTypes = new Type[] {
											 typeof(System.Windows.Forms.DataVisualization.Charting.Chart),
											 typeof(System.Windows.Forms.BindingNavigator),
											 typeof(System.Windows.Forms.BindingSource),
											 typeof(System.Windows.Forms.DataGridView),
                                             typeof(System.Data.DataSet)
										 };


        private System.Reflection.FieldInfo thisMouseTrack;

        public ToolBoxPane()
        {
            InitializeComponent();
            listAll = new List<ToolboxItem>[] {
                listData_ti,
                listCustomControls_ti,
                listWindowsForms_ti,
                listComponents_ti };
            pointer = new ToolboxItem();
            pointer.DisplayName = "<Pointer>";
            pointer.Bitmap = global::VisualPascalABC.Properties.Resources.Pointer; //new Bitmap(16, 16);
            pointer.Bitmap.MakeTransparent(pointer.Bitmap.GetPixel(0, 0));
            pointerItem = new ListBarItem(pointer.DisplayName);
            pointerItem.Image = pointer.Bitmap;
            pointerItem.Tag = pointer;
            FillToolbox();
            listData.Tag = listData_ti;
            listWindowsForms.Tag = listWindowsForms_ti;
            listComponents.Tag = listComponents_ti;
            thisMouseTrack = typeof(ListBar).GetField("mouseTrack",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic);
            foreach (ListBarGroup lg in listBar1.Groups)
            {
                lg.Caption = PascalABCCompiler.StringResources.Get(lg.Caption);
            }
            //listBar1.SelectedGroup.Items.
        }

        /// Clean up any resources being used.
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (components != null)
        //        {
        //            components.Dispose();
        //        }
        //    }
        //    base.Dispose(disposing);
        //}
        
        private void AddBarItem(ListBarGroup group, ToolboxItem tbi)
        {
            ListBarItem lbi = new ListBarItem(tbi.DisplayName);
            lbi.Image = tbi.Bitmap;
            group.Items.Add(lbi);
            lbi.Tag = tbi;
        }

        // Fill our ListBoxes with the appropriate ToolboxItems.
        private void FillToolbox()
        {
            foreach (ListBarGroup tab in listBar1.Groups)
            {
                tab.Items.Add(pointerItem);
            }

            foreach (List<ToolboxItem> t in listAll)
            {
                t.Add(pointer);
            }

            foreach (Type type in windowsFormsToolTypes)
            {
                ToolboxItem tbi = new ToolboxItem(type);
                ToolboxBitmapAttribute tba = TypeDescriptor.GetAttributes(type)[typeof(ToolboxBitmapAttribute)] as ToolboxBitmapAttribute;
                if (tba != null)
                {
                    tbi.Bitmap = (Bitmap)tba.GetImage(type);
                }
                listWindowsForms_ti.Add(tbi);
                AddBarItem(listWindowsForms, tbi);
            }

            foreach (Type type in componentsToolTypes)
            {
                ToolboxItem tbi = new ToolboxItem(type);
                ToolboxBitmapAttribute tba = TypeDescriptor.GetAttributes(type)[typeof(ToolboxBitmapAttribute)] as ToolboxBitmapAttribute;
                if (tba != null)
                {
                    tbi.Bitmap = (Bitmap)tba.GetImage(type);
                }
                listComponents_ti.Add(tbi);
                AddBarItem(listComponents, tbi);
            }

            foreach (Type type in dataToolTypes)
            {
                ToolboxItem tbi = new ToolboxItem(type);
                ToolboxBitmapAttribute tba = TypeDescriptor.GetAttributes(type)[typeof(ToolboxBitmapAttribute)] as ToolboxBitmapAttribute;
                if (tba != null)
                {
                    tbi.Bitmap = (Bitmap)tba.GetImage(type);
                }
                listData_ti.Add(tbi);
                AddBarItem(listData, tbi);
            }
        }
        
        /// We need access to the designers.
        public SampleDesignerHost.SampleDesignerHost Host
        {
            get
            {
                return host;
            }
            set
            {
                host = value;
            }
        }

        /// The currently selected tool is defined by our currently selected
        /// category (ListBox) and our selectedIndex member.
        public ToolboxItem SelectedTool
        {
            get
            {
                if (listBar1.SelectedGroup.SelectedItem == null)
                    return null;
                ToolboxItem t = listBar1.SelectedGroup.SelectedItem.Tag as ToolboxItem;
                return t;
            }
        }

        /// The name of our selected category (Windows Forms, Components, etc.)
        /// This property (and the next few) are all in place to support
        /// methods of the IToolboxService.
        public string SelectedCategory
        {
            get
            {
                return listBar1.SelectedGroup.Caption;
            }
        }

        /// The names of all our categories.
        public CategoryNameCollection CategoryNames
        {
            get
            {
                int count = listBar1.Groups.Count;
                string[] categories = new string[count];
                for (int i = 0; i < count; i++)
                {
                    categories[i] = listBar1.Groups[i].Caption;
                }
                return new CategoryNameCollection(categories);
            }
        }

        /// The IToolboxService has methods for getting tool collections using
        /// an optional category parameter. We support that request here.
        public ToolboxItemCollection GetToolsFromCategory(string category)
        {
            foreach (ListBarGroup g in listBar1.Groups)
            {
                if (g.Caption == category)
                {
                    //ListBox list = tab.Controls[0] as ListBox;
                    List<ToolboxItem> list = g.Tag as List<ToolboxItem>;
                    ToolboxItem[] tools = new ToolboxItem[list.Count];
                    list.CopyTo(tools, 0);
                    return new ToolboxItemCollection(tools);
                }
            }

            return null;
        }

        /// Get all of our tools.
        public ToolboxItemCollection GetAllTools()
        {
            List<ToolboxItem> toolsAL = new List<ToolboxItem>();
            foreach (List<ToolboxItem> t in listAll)
            {
                toolsAL.AddRange(t);
            }
            //ToolboxItem[] tools = new ToolboxItem[toolsAL.Count];
            //toolsAL.CopyTo(tools);
            return new ToolboxItemCollection(toolsAL.ToArray());
        }

        /// Resets the selection to the pointer. Note that this is the only method
        /// which allows our IToolboxService to set our selection. It calls this method
        /// after a tool has been used.
        public void SelectPointer()
        {
            ListBarGroup list = listBar1.SelectedGroup;
            deselectAll();
            //foreach (ListBarItem lbi in list.Items)
            //{
            //    lbi.Selected = false;
            //    //lbi.MouseOver = false;
            //    lbi.MouseDown = false;
            //}
            //list.Invalidate(list.GetItemRectangle(selectedIndex));
            selectedIndex = 0;
            //list.SelectedItem = list.Items[0];
            list.Items[0].Selected = true;
            listBar1.Refresh();
            //list.Invalidate(list.GetItemRectangle(selectedIndex));
            //listBar1.Padding;
        }

        private void list_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
        }

        private void list_MeasureItem(object sender, System.Windows.Forms.MeasureItemEventArgs e)
        {
        }

        private void tabControl_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
        }

        private void deselectAll()
        {
            foreach (ListBarItem lbi in listBar1.SelectedGroup.Items)
            {
                lbi.Selected = false;
                lbi.MouseDown = false;
            }
        }

        private void listBar1_MouseDown(object sender, MouseEventArgs e)
        {
            ListBarItem selected = thisMouseTrack.GetValue(listBar1) as ListBarItem;
            if (selected == null)
            {
                return;
            }
            else
            {
                deselectAll();
                selected.Selected = true;
                selectedIndex = listBar1.SelectedGroup.Items.IndexOf(selected);
                //deselectAll();
            }
            if (selectedIndex != 0)
            {
                // If this is a double-click, then the user wants to add the selected component
                // to the default location on the designer, with the default size. We call
                // ToolPicked on the current designer (as a IToolboxUser) to place the tool.
                // The IToolboxService calls SelectedToolboxItemUsed(), which calls this control's
                // SelectPointer() method.
                //
                if (e.Clicks == 2)
                {
                    IToolboxUser tbu = host.GetDesigner(host.RootComponent) as IToolboxUser;
                    if (tbu != null)
                    {
                        //tbu.ToolPicked((ToolboxItem)(lbSender.Items[selectedIndex]));
                        tbu.ToolPicked((ToolboxItem)(listBar1.SelectedGroup.SelectedItem.Tag));
                    }
                    SelectPointer();
                }
                // Otherwise this is either a single click or a drag. Either way, we do the same
                // thing: start a drag--if this is just a single click, then the drag will
                // abort as soon as there's a MouseUp event.
                //
                else if (e.Clicks < 2)
                {
                    //ToolboxItem tbi = lbSender.Items[selectedIndex] as ToolboxItem;
                    ToolboxItem tbi = listBar1.SelectedGroup.SelectedItem.Tag as ToolboxItem;
                    IToolboxService tbs = ((IServiceContainer)host).GetService(typeof(IToolboxService)) as IToolboxService;

                    // The IToolboxService serializes ToolboxItems by packaging them in DataObjects.
                    DataObject d = tbs.SerializeToolboxItem(tbi) as DataObject;
                    try
                    {
                        (sender as Control).DoDragDrop(d, DragDropEffects.Copy);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void listBar1_SelectedGroupChanged(object sender, EventArgs e)
        {
            SelectPointer();
        }

        private void listBar1_Paint(object sender, PaintEventArgs e)
        {
            if (initialPaint)
            {
                SelectPointer();
            }
            initialPaint = false;
        }

        private void listBar1_ItemClicked(object sender, ItemClickedEventArgs e)
        {
            SelectPointer();
        }

    }
}
