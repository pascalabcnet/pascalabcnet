//------------------------------------------------------------------------------
/// <copyright from='1997' to='2002' company='Microsoft Corporation'>
///    Copyright (c) Microsoft Corporation. All Rights Reserved.
///
///    This source code is intended only as a supplement to Microsoft
///    Development Tools and/or on-line documentation.  See these other
///    materials for detailed information regarding Microsoft code samples.
///
/// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.IO;
using System.Reflection;

namespace SampleDesignerApplication1
{
	/// Our implementation of a toolbox. We kept the actual toolbox control
	/// separate from the IToolboxService, but the toolbox could easily
	/// implement the service itself. Note that IToolboxUser does not pertain
	/// to the toolbox, but instead is implemented by the designers that receive
	/// ToolboxItems.
	public class ToolboxPane : System.Windows.Forms.UserControl
	{
		private SampleDesignerHost.SampleDesignerHost host;
		private System.Windows.Forms.ListBox listData;
		private System.Windows.Forms.ListBox listCustomControls;
		private System.Windows.Forms.ListBox listWindowsForms;
		private System.Windows.Forms.ListBox listComponents;
		private System.Windows.Forms.TabPage tabWindowsForms;
		private System.Windows.Forms.TabPage tabComponents;
		private System.Windows.Forms.TabPage tabData;
		private System.Windows.Forms.TabPage tabCustomControls;
		private System.Windows.Forms.TabControl tabControl;
		private ToolboxItem pointer; // a "null" tool
		private int selectedIndex; // the index of the currently selected tool
		private bool initialPaint = true; // see ToolboxPane_Paint method

		// We load types into our categories in a rather primitive way. It is easier than
		// dealing with type resolution, but we can only do this since our list of tools
		// is standard and unchanging.
		//
		private Type[] windowsFormsToolTypes =  new Type[] {
													  typeof(System.Windows.Forms.PropertyGrid),
													  typeof(System.Windows.Forms.Label),
													  typeof(System.Windows.Forms.LinkLabel),
													  typeof(System.Windows.Forms.Button),
													  typeof(System.Windows.Forms.TextBox),
													  typeof(System.Windows.Forms.CheckBox),
													  typeof(System.Windows.Forms.RadioButton),
													  typeof(System.Windows.Forms.GroupBox),
													  typeof(System.Windows.Forms.PictureBox),
													  typeof(System.Windows.Forms.Panel),
													  typeof(System.Windows.Forms.DataGrid),
													  typeof(System.Windows.Forms.ListBox),
													  typeof(System.Windows.Forms.CheckedListBox),
													  typeof(System.Windows.Forms.ComboBox),
													  typeof(System.Windows.Forms.ListView),
													  typeof(System.Windows.Forms.TreeView),
													  typeof(System.Windows.Forms.TabControl),
													  typeof(System.Windows.Forms.DateTimePicker),
													  typeof(System.Windows.Forms.MonthCalendar),
													  typeof(System.Windows.Forms.HScrollBar),
													  typeof(System.Windows.Forms.VScrollBar),
													  typeof(System.Windows.Forms.Timer),
													  typeof(System.Windows.Forms.Splitter),
													  typeof(System.Windows.Forms.DomainUpDown),
													  typeof(System.Windows.Forms.NumericUpDown),
													  typeof(System.Windows.Forms.TrackBar),
													  typeof(System.Windows.Forms.ProgressBar),
													  typeof(System.Windows.Forms.RichTextBox),
													  typeof(System.Windows.Forms.ImageList),
													  typeof(System.Windows.Forms.HelpProvider),
													  typeof(System.Windows.Forms.ToolTip),
													  typeof(System.Windows.Forms.ToolBar),
													  typeof(System.Windows.Forms.StatusBar),
													  typeof(System.Windows.Forms.UserControl),
													  typeof(System.Windows.Forms.NotifyIcon),
													  typeof(System.Windows.Forms.OpenFileDialog),
													  typeof(System.Windows.Forms.SaveFileDialog),
													  typeof(System.Windows.Forms.FontDialog),
													  typeof(System.Windows.Forms.ColorDialog),
													  typeof(System.Windows.Forms.PrintDialog),
													  typeof(System.Windows.Forms.PrintPreviewDialog),
													  typeof(System.Windows.Forms.PrintPreviewControl),
													  typeof(System.Windows.Forms.ErrorProvider),
													  typeof(System.Drawing.Printing.PrintDocument),
													  typeof(System.Windows.Forms.PageSetupDialog)
												  };
		private Type[] componentsToolTypes = new Type[] {
												   typeof(System.IO.FileSystemWatcher),
												   typeof(System.Diagnostics.Process),
												   typeof(System.Timers.Timer)
											   };

		private Type[] dataToolTypes = new Type[] {
											 typeof(System.Data.OleDb.OleDbCommandBuilder),
											 typeof(System.Data.OleDb.OleDbConnection),
											 typeof(System.Data.SqlClient.SqlCommandBuilder),
											 typeof(System.Data.SqlClient.SqlConnection),
										 };

		private System.ComponentModel.Container components = null;

		public ToolboxPane()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			pointer = new ToolboxItem();
			pointer.DisplayName = "<Pointer>";
			pointer.Bitmap = new Bitmap(16, 16);
			
			// Populate our tool lists.
			FillToolbox();
			ListBox list = this.tabControl.SelectedTab.Controls[0] as ListBox;
		}

		/// Clean up any resources being used.
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		// Properties

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
				ListBox list = this.tabControl.SelectedTab.Controls[0] as ListBox;
				return list.Items[selectedIndex] as ToolboxItem;
			}
		}

		/// The name of our selected category (Windows Forms, Components, etc.)
		/// This property (and the next few) are all in place to support
		/// methods of the IToolboxService.
		public string SelectedCategory
		{
			get
			{
				return tabControl.SelectedTab.Text;
			}
		}

		/// The names of all our categories.
		public CategoryNameCollection CategoryNames
		{
			get
			{
				string[] categories = new string[tabControl.TabPages.Count];
				for (int i = 0; i < tabControl.TabPages.Count; i++)
				{
					categories[i] = tabControl.TabPages[i].Text;
				}
				return new CategoryNameCollection(categories);
			}
		}

		// Methods

		/// The IToolboxService has methods for getting tool collections using
		/// an optional category parameter. We support that request here.
		public ToolboxItemCollection GetToolsFromCategory(string category)
		{
			foreach (TabPage tab in tabControl.TabPages)
			{
				if (tab.Text == category)
				{
					ListBox list = tab.Controls[0] as ListBox;
					ToolboxItem[] tools = new ToolboxItem[list.Items.Count];
					list.Items.CopyTo(tools, 0);
					return new ToolboxItemCollection(tools);
				}
			}

			return null;
		}

		/// Get all of our tools.
		public ToolboxItemCollection GetAllTools()
		{
			ArrayList toolsAL = new ArrayList();
			foreach (TabPage tab in tabControl.TabPages)
			{
				ListBox list = tab.Controls[0] as ListBox;
				toolsAL.Add(list.Items);
			}
			ToolboxItem[] tools = new ToolboxItem[toolsAL.Count];
			toolsAL.CopyTo(tools);
			return new ToolboxItemCollection(tools);
		}

		/// Resets the selection to the pointer. Note that this is the only method
		/// which allows our IToolboxService to set our selection. It calls this method
		/// after a tool has been used.
		public void SelectPointer()
		{
			ListBox list = this.tabControl.SelectedTab.Controls[0] as ListBox;
			list.Invalidate(list.GetItemRectangle(selectedIndex));
			selectedIndex = 0;
			list.SelectedIndex = 0;
			list.Invalidate(list.GetItemRectangle(selectedIndex));
		}

		#region Component Designer generated code
		 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		
		private void InitializeComponent()
		{
			this.listData = new System.Windows.Forms.ListBox();
			this.listCustomControls = new System.Windows.Forms.ListBox();
			this.listWindowsForms = new System.Windows.Forms.ListBox();
			this.listComponents = new System.Windows.Forms.ListBox();
			this.tabWindowsForms = new System.Windows.Forms.TabPage();
			this.tabComponents = new System.Windows.Forms.TabPage();
			this.tabData = new System.Windows.Forms.TabPage();
			this.tabCustomControls = new System.Windows.Forms.TabPage();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabWindowsForms.SuspendLayout();
			this.tabComponents.SuspendLayout();
			this.tabData.SuspendLayout();
			this.tabCustomControls.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// listData
			// 
			this.listData.BackColor = System.Drawing.Color.LightSlateGray;
			this.listData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listData.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.listData.Location = new System.Drawing.Point(0, 0);
			this.listData.Name = "listData";
			this.listData.Size = new System.Drawing.Size(280, 468);
			this.listData.TabIndex = 0;
			this.listData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.list_KeyDown);
			this.listData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.list_MouseDown);
			this.listData.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.list_MeasureItem);
			this.listData.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.list_DrawItem);
			// 
			// listCustomControls
			// 
			this.listCustomControls.BackColor = System.Drawing.Color.LightSlateGray;
			this.listCustomControls.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listCustomControls.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.listCustomControls.Location = new System.Drawing.Point(0, 0);
			this.listCustomControls.Name = "listCustomControls";
			this.listCustomControls.Size = new System.Drawing.Size(280, 508);
			this.listCustomControls.TabIndex = 0;
			this.listCustomControls.KeyDown += new System.Windows.Forms.KeyEventHandler(this.list_KeyDown);
			this.listCustomControls.MouseDown += new System.Windows.Forms.MouseEventHandler(this.list_MouseDown);
			this.listCustomControls.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.list_MeasureItem);
			this.listCustomControls.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.list_DrawItem);
			// 
			// listWindowsForms
			// 
			this.listWindowsForms.AllowDrop = true;
			this.listWindowsForms.BackColor = System.Drawing.Color.LightSlateGray;
			this.listWindowsForms.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listWindowsForms.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.listWindowsForms.Location = new System.Drawing.Point(0, 0);
			this.listWindowsForms.Name = "listWindowsForms";
			this.listWindowsForms.Size = new System.Drawing.Size(280, 508);
			this.listWindowsForms.TabIndex = 0;
			this.listWindowsForms.KeyDown += new System.Windows.Forms.KeyEventHandler(this.list_KeyDown);
			this.listWindowsForms.MouseDown += new System.Windows.Forms.MouseEventHandler(this.list_MouseDown);
			this.listWindowsForms.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.list_MeasureItem);
			this.listWindowsForms.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.list_DrawItem);
			// 
			// listComponents
			// 
			this.listComponents.BackColor = System.Drawing.Color.LightSlateGray;
			this.listComponents.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listComponents.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.listComponents.Location = new System.Drawing.Point(0, 0);
			this.listComponents.Name = "listComponents";
			this.listComponents.Size = new System.Drawing.Size(280, 468);
			this.listComponents.TabIndex = 0;
			this.listComponents.KeyDown += new System.Windows.Forms.KeyEventHandler(this.list_KeyDown);
			this.listComponents.MouseDown += new System.Windows.Forms.MouseEventHandler(this.list_MouseDown);
			this.listComponents.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.list_MeasureItem);
			this.listComponents.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.list_DrawItem);
			// 
			// tabWindowsForms
			// 
			this.tabWindowsForms.BackColor = System.Drawing.Color.LightSlateGray;
			this.tabWindowsForms.Controls.AddRange(new System.Windows.Forms.Control[] {
																						  this.listWindowsForms});
			this.tabWindowsForms.Location = new System.Drawing.Point(4, 40);
			this.tabWindowsForms.Name = "tabWindowsForms";
			this.tabWindowsForms.Size = new System.Drawing.Size(280, 508);
			this.tabWindowsForms.TabIndex = 0;
			this.tabWindowsForms.Text = "Windows Forms";
			// 
			// tabComponents
			// 
			this.tabComponents.BackColor = System.Drawing.Color.LightSlateGray;
			this.tabComponents.Controls.AddRange(new System.Windows.Forms.Control[] {
																						this.listComponents});
			this.tabComponents.Location = new System.Drawing.Point(4, 40);
			this.tabComponents.Name = "tabComponents";
			this.tabComponents.Size = new System.Drawing.Size(280, 468);
			this.tabComponents.TabIndex = 1;
			this.tabComponents.Text = "Components";
			// 
			// tabData
			// 
			this.tabData.BackColor = System.Drawing.Color.LightSlateGray;
			this.tabData.Controls.AddRange(new System.Windows.Forms.Control[] {
																				  this.listData});
			this.tabData.Location = new System.Drawing.Point(4, 40);
			this.tabData.Name = "tabData";
			this.tabData.Size = new System.Drawing.Size(280, 468);
			this.tabData.TabIndex = 2;
			this.tabData.Text = "Data";
			// 
			// tabCustomControls
			// 
			this.tabCustomControls.BackColor = System.Drawing.Color.LightSlateGray;
			this.tabCustomControls.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this.listCustomControls});
			this.tabCustomControls.Location = new System.Drawing.Point(4, 40);
			this.tabCustomControls.Name = "tabCustomControls";
			this.tabCustomControls.Size = new System.Drawing.Size(280, 508);
			this.tabCustomControls.TabIndex = 3;
			this.tabCustomControls.Text = "Custom Controls";
			// 
			// tabControl
			// 
			this.tabControl.Controls.AddRange(new System.Windows.Forms.Control[] {
																					 this.tabWindowsForms,
																					 this.tabData,
																					 this.tabComponents,
																					 this.tabCustomControls});
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tabControl.ItemSize = new System.Drawing.Size(99, 18);
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Multiline = true;
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(288, 552);
			this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
			this.tabControl.TabIndex = 1;
			this.tabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl_DrawItem);
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// ToolboxPane
			// 
			this.BackColor = System.Drawing.Color.Black;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl});
			this.Name = "ToolboxPane";
			this.Size = new System.Drawing.Size(288, 552);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ToolboxPane_Paint);
			this.tabWindowsForms.ResumeLayout(false);
			this.tabComponents.ResumeLayout(false);
			this.tabData.ResumeLayout(false);
			this.tabCustomControls.ResumeLayout(false);
			this.tabControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		// Fill our ListBoxes with the appropriate ToolboxItems.
		private void FillToolbox()
		{
			foreach (TabPage tab in tabControl.TabPages)
			{
				ListBox list = tab.Controls[0] as ListBox;
				list.Items.Add(pointer);
			}

			foreach (Type type in windowsFormsToolTypes)
			{
				ToolboxItem tbi = new ToolboxItem(type);
				ToolboxBitmapAttribute tba = TypeDescriptor.GetAttributes(type)[typeof(ToolboxBitmapAttribute)] as ToolboxBitmapAttribute;
				if (tba != null)
				{
					tbi.Bitmap = (Bitmap)tba.GetImage(type);
				}
				listWindowsForms.Items.Add(tbi);
			}

			foreach (Type type in componentsToolTypes)
			{
				ToolboxItem tbi = new ToolboxItem(type);
				ToolboxBitmapAttribute tba = TypeDescriptor.GetAttributes(type)[typeof(ToolboxBitmapAttribute)] as ToolboxBitmapAttribute;
				if (tba != null)
				{
					tbi.Bitmap = (Bitmap)tba.GetImage(type);
				}
				listComponents.Items.Add(tbi);
			}

			foreach (Type type in dataToolTypes)
			{
				ToolboxItem tbi = new ToolboxItem(type);
				ToolboxBitmapAttribute tba = TypeDescriptor.GetAttributes(type)[typeof(ToolboxBitmapAttribute)] as ToolboxBitmapAttribute;
				if (tba != null)
				{
					tbi.Bitmap = (Bitmap)tba.GetImage(type);
				}
				listData.Items.Add(tbi);
			}
		}

		// Event Handlers

		/// Each ToolboxItem contains a string and a bitmap. We draw each of these each time
		/// we draw a ListBox item (a tool).
		private void list_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			ListBox lbSender = sender as ListBox;

			// If this tool is the currently selected tool, draw it with a highlight.
			if (selectedIndex == e.Index)
			{
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, e.Bounds);
			}
			else
			{
				e.Graphics.FillRectangle(Brushes.LightSlateGray, e.Bounds);
			}

			ToolboxItem tbi = lbSender.Items[e.Index] as ToolboxItem;
			Rectangle BitmapBounds = new Rectangle(e.Bounds.Location.X, e.Bounds.Location.Y, tbi.Bitmap.Width, e.Bounds.Height);
			Rectangle StringBounds = new Rectangle(e.Bounds.Location.X + BitmapBounds.Width, e.Bounds.Location.Y, e.Bounds.Width - BitmapBounds.Width, e.Bounds.Height);
			e.Graphics.DrawImage(tbi.Bitmap, BitmapBounds);
			e.Graphics.DrawString(tbi.DisplayName, lbSender.Font, Brushes.White, StringBounds);

		}

		/// We measure each item by taking the combined width of the string and bitmap,
		/// and the greater height of the two.
		private void list_MeasureItem(object sender, System.Windows.Forms.MeasureItemEventArgs e)
		{
			ListBox lbSender = sender as ListBox;
			ToolboxItem tbi = lbSender.Items[e.Index] as ToolboxItem;
			Size textSize = e.Graphics.MeasureString(tbi.DisplayName, lbSender.Font).ToSize();
			e.ItemWidth = tbi.Bitmap.Width + textSize.Width;
			if (tbi.Bitmap.Height > textSize.Height)
			{
				e.ItemHeight = tbi.Bitmap.Height;
			}
			else
			{
				e.ItemHeight = textSize.Height;
			}
		}

		/// Owner draw for that rainbow look.
		private void tabControl_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			TabControl tcSender = sender as TabControl;
			string text = tcSender.TabPages[e.Index].Text;
			Size textSize = e.Graphics.MeasureString(text, tcSender.Font).ToSize();

            e.Graphics.FillRectangle(Brushes.SteelBlue, e.Bounds);
            //switch (e.Index)
            //{
            //    case 0: e.Graphics.FillRectangle(Brushes.SteelBlue, e.Bounds);
            //        break;
            //    case 1: e.Graphics.FillRectangle(Brushes.Gold, e.Bounds);
            //        break;
            //    case 2: e.Graphics.FillRectangle(Brushes.Tomato, e.Bounds);
            //        break;
            //    case 3: e.Graphics.FillRectangle(Brushes.LimeGreen, e.Bounds);
            //        break;
            //}

			Point textLocation = new Point(
				e.Bounds.X + (e.Bounds.Width - textSize.Width) / 2,
				e.Bounds.Y + (e.Bounds.Height - textSize.Height) / 2);
			e.Graphics.DrawString(text, tcSender.Font, Brushes.White, textLocation);
		}

		/// This method handles a MouseDown event, which might be one of three things:
		///		1) the start of a single click
		///		2) the start of a drag
		///		3) the start of a second of two consecutive clicks (double-click)
		private void list_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// Regardless of which kind of click this is, we need to change the selection.
			// First we grab the bounds of the old selected tool so that we can de-higlight it.
			//
			ListBox lbSender = sender as ListBox;
			Rectangle lastSelectedBounds = lbSender.GetItemRectangle(selectedIndex);

			selectedIndex = lbSender.IndexFromPoint(e.X, e.Y); // change our selection
			lbSender.SelectedIndex = selectedIndex;

			lbSender.Invalidate(lastSelectedBounds); // clear highlight from last selection
			lbSender.Invalidate(lbSender.GetItemRectangle(selectedIndex)); // highlight new one
			
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
						tbu.ToolPicked((ToolboxItem)(lbSender.Items[selectedIndex]));
					}
				}
				// Otherwise this is either a single click or a drag. Either way, we do the same
				// thing: start a drag--if this is just a single click, then the drag will
				// abort as soon as there's a MouseUp event.
				//
				else if (e.Clicks < 2)
				{
					ToolboxItem tbi = lbSender.Items[selectedIndex] as ToolboxItem;
					IToolboxService tbs = ((IServiceContainer)host).GetService(typeof(IToolboxService)) as IToolboxService;

					// The IToolboxService serializes ToolboxItems by packaging them in DataObjects.
					DataObject d = tbs.SerializeToolboxItem(tbi) as DataObject;
					try
					{
						lbSender.DoDragDrop(d, DragDropEffects.Copy);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		/// Go to the pointer whenever we change categories.
		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SelectPointer();
		}

		/// On our first paint, select the pointer.
		private void ToolboxPane_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (initialPaint)
			{
				SelectPointer();
			}
			initialPaint = false;
		}

		/// The toolbox can also be navigated using the keyboard commands Up, Down, and Enter.
		private void list_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			ListBox lbSender = sender as ListBox;
			Rectangle lastSelectedBounds = lbSender.GetItemRectangle(selectedIndex);
			switch (e.KeyCode)
			{
				case Keys.Up: if (selectedIndex > 0)
					{
						selectedIndex--; // change selection
						lbSender.SelectedIndex = selectedIndex;
						lbSender.Invalidate(lastSelectedBounds); // clear old highlight
						lbSender.Invalidate(lbSender.GetItemRectangle(selectedIndex)); // add new one
					}
					break;
				case Keys.Down: if (selectedIndex + 1 < lbSender.Items.Count)
					{
						selectedIndex++; // change selection
						lbSender.SelectedIndex = selectedIndex;
						lbSender.Invalidate(lastSelectedBounds); // clear old highlight
						lbSender.Invalidate(lbSender.GetItemRectangle(selectedIndex)); // add new one
					}
					break;
				case Keys.Enter: IToolboxUser tbu = host.GetDesigner(host.RootComponent) as IToolboxUser;
					if (tbu != null)
					{
						// Enter means place the tool with default location and default size.
						tbu.ToolPicked((ToolboxItem)(lbSender.Items[selectedIndex]));
					}
					break;
			}
		}
	}
}
