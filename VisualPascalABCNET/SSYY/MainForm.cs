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
namespace SampleDesignerApplication {
    
    using System;
    using System.Drawing.Design;
	using System.Drawing;
    using System.Windows.Forms;
    using System.ComponentModel;
    using System.ComponentModel.Design;
	using System.ComponentModel.Design.Serialization;
	using System.Windows.Forms.Design;
    using System.Reflection;
	using System.CodeDom;
	using System.Collections;
    using SampleDesignerHost;
    using System.IO;

	/// The main UI for our design environment.
    public class MainForm : System.Windows.Forms.Form {
        private System.ComponentModel.IContainer components = null;

        private SampleDesignerHost host;
        private SampleDesignerLoader loader;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItemNew;
		private System.Windows.Forms.MenuItem menuItemOpen;
		private System.Windows.Forms.MenuItem menuItemSave;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItemView;
		private System.Windows.Forms.MenuItem menuItemServiceRequests;
        private ServiceContainer hostingServiceContainer;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabDesign;
		private System.Windows.Forms.TabPage tabXML;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private System.Windows.Forms.TextBox textXML;
		private System.Windows.Forms.TextBox textCS;
		private System.Windows.Forms.TextBox textVB;
		private System.Windows.Forms.TabPage tabCS;
		private System.Windows.Forms.TabPage tabVB;
		private System.Windows.Forms.MenuItem menuItemEdit;
		private System.Windows.Forms.MenuItem menuItemUndo;
		private System.Windows.Forms.MenuItem menuItemRedo;
		private System.Windows.Forms.MenuItem menuItemCut;
		private System.Windows.Forms.MenuItem menuItemCopy;
		private System.Windows.Forms.MenuItem menuItemPaste;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.Windows.Forms.MenuItem menuItemSelectAll;
		private System.Windows.Forms.MenuItem menuItemRule;
		private System.Windows.Forms.MenuItem menuItemDesign;
		private System.Windows.Forms.MenuItem menuItemCSource;
		private System.Windows.Forms.MenuItem menuItemVBSource;
		private System.Windows.Forms.MenuItem menuItemXML;
		private System.Windows.Forms.MenuItem menuItemLayout;
		private System.Windows.Forms.MenuItem menuItemShowGrid;
		private System.Windows.Forms.MenuItem menuItemAlign;
		private System.Windows.Forms.MenuItem menuItemCenter;
		private System.Windows.Forms.MenuItem menuItemSizeTo;
		private System.Windows.Forms.MenuItem menuItemSnapToGrid;
		private System.Windows.Forms.MenuItem menuItemLefts;
		private System.Windows.Forms.MenuItem menuItemToGrid;
		private System.Windows.Forms.MenuItem menuItemRights;
		private System.Windows.Forms.MenuItem menuItemTops;
		private System.Windows.Forms.MenuItem menuItemBottoms;
		private System.Windows.Forms.MenuItem menuItemMiddles;
		private System.Windows.Forms.MenuItem menuItemCenters;
		private System.Windows.Forms.MenuItem menuItemHoriz;
		private System.Windows.Forms.MenuItem menuItemVert;
		private System.Windows.Forms.MenuItem menuItemControl;
		private System.Windows.Forms.MenuItem menuItemControlW;
		private System.Windows.Forms.MenuItem menuItemControlH;
		private System.Windows.Forms.MenuItem menuItemGrid;
		private System.Windows.Forms.MenuItem menuItemZO;
		private System.Windows.Forms.MenuItem menuItemBTF;
		private System.Windows.Forms.MenuItem menuItemSTB;
		private System.Windows.Forms.MenuItem menuItemTO;
		private ServiceRequests serviceRequests;
		private System.Windows.Forms.MenuItem menuItemDebug;
		private System.Windows.Forms.MenuItem menuItemBuild;
		private System.Windows.Forms.MenuItem menuItemRun;
		private System.Windows.Forms.MenuItem menuItemStop;
		private System.Windows.Forms.MenuItem menuItemProperties;
		private SampleDesignerApplication.ToolBoxPane toolbox;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panelMain;
		private System.Windows.Forms.MenuItem menuItemSaveAs;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private KeystrokeMessageFilter filter;
        
        public MainForm() {

            // Required for Windows Forms Designer support
            this.InitializeComponent();
            hostingServiceContainer = new ServiceContainer();

            // Our ISelectionService looks for this and informs the grid
            // of selection changes.  In an environment such as Visual Studio, this
            // is all handled for you.
			//
            hostingServiceContainer.AddService(typeof(PropertyGrid), propertyGrid);

			// Our IToolboxService needs access to our toolbox.
			hostingServiceContainer.AddService(typeof(ToolBoxPane), toolbox);

			// Our IDesignerLoader needs access to our main tab control in order
			// to fill in the text in the source code windows.
			//
			hostingServiceContainer.AddService(typeof(TabControl), tabControl);
        }

		/// This method is called whenever we create a new designer or
		/// load one from existing XML.
        private void CreateDesigner(SampleDesignerLoader loader) 
        {
			// Our loader will handle loading the file (or creating a blank one).
            host = new SampleDesignerHost(hostingServiceContainer);
			
			// The limited events tab functionality we have requires a special kind of
			// property grid site.
			//
			propertyGrid.Site = new PropertyGridSite(host as IServiceProvider, this);
			propertyGrid.PropertyTabs.AddTabType(typeof(EventsTab));
           
			host.LoadDocument(loader);
			this.loader = loader;

			// The toolbox needs access to the IToolboxService and the designers.
			toolbox.Host = host;

			// Initialize our document window.
            host.View.Dock = DockStyle.Fill;
			host.View.Visible = true;
			panelMain.Controls.Add(host.View);

			// These are normally unavailable if we've got no host created.
			// Otherwise we gets lots of null reference exceptions.
			//
			tabControl.Visible = true;
            menuItemSave.Enabled = true;
			menuItemSaveAs.Enabled = true;
			menuItemEdit.Enabled = true;
			menuItemView.Enabled = true;
			menuItemLayout.Enabled = true;
			menuItemDebug.Enabled = true;

			// This IMessageFilter is used to intercept and decipher keyboard presses
			// that might be instructions for our designer. We have to do it this way
			// since we can't have our designer raise KeyPress events.
			//
			filter = new KeystrokeMessageFilter(host);
			Application.AddMessageFilter(filter);
        }

		/// This is called to destroy our designer (when we load a new one for example).
		/// It returns true if our environment has been cleared, false if not (if someone
		/// cancels a dispose when prompted to save changes).
        private bool DestroyDesigner() 
        {
            if (loader != null) 
            {
                if (loader.PromptDispose())
                {
					// Again, bad things happen if there's no host loaded and 
					// certain buttons are pressed in our TabControl or MainMenu.
					//
					tabControl.Visible = false;
                    menuItemSave.Enabled = false;
					menuItemSaveAs.Enabled = false;
					menuItemEdit.Enabled = false;
					menuItemView.Enabled = false;
					menuItemLayout.Enabled = false;
					menuItemDebug.Enabled = false;

					// Get rid of our document window.
                    panelMain.Controls.Clear();

					// No need to filter for designer-intended keyboard strokes now.
					Application.RemoveMessageFilter(filter);
					filter = null;

					// Get rid of that property grid site so it doesn't ask for
					// any more services from our soon-to-be-disposed host.
					//
					propertyGrid.Site = null;

                    host.Dispose();
                    loader = null;
                    host = null;
                    return true;
                }
                return false;
            }
            return true;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {                
                if (host != null) {
                    host.Dispose();
                }

            }
            base.Dispose(disposing);
        }

		/// Check to make sure our stuff is saved before we close.
		protected override void OnClosing(CancelEventArgs e)
		{
			if (DestroyDesigner())
			{
				e.Cancel = false;
				base.OnClosing (e);
			}
			else
			{
				e.Cancel = true;
				base.OnClosing (e);
			}
		}
        
        private void InitializeComponent() 
		{
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemNew = new System.Windows.Forms.MenuItem();
			this.menuItemOpen = new System.Windows.Forms.MenuItem();
			this.menuItemSave = new System.Windows.Forms.MenuItem();
			this.menuItemSaveAs = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.menuItemEdit = new System.Windows.Forms.MenuItem();
			this.menuItemUndo = new System.Windows.Forms.MenuItem();
			this.menuItemRedo = new System.Windows.Forms.MenuItem();
			this.menuItemRule = new System.Windows.Forms.MenuItem();
			this.menuItemCut = new System.Windows.Forms.MenuItem();
			this.menuItemCopy = new System.Windows.Forms.MenuItem();
			this.menuItemPaste = new System.Windows.Forms.MenuItem();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			this.menuItemSelectAll = new System.Windows.Forms.MenuItem();
			this.menuItemView = new System.Windows.Forms.MenuItem();
			this.menuItemServiceRequests = new System.Windows.Forms.MenuItem();
			this.menuItemDesign = new System.Windows.Forms.MenuItem();
			this.menuItemCSource = new System.Windows.Forms.MenuItem();
			this.menuItemVBSource = new System.Windows.Forms.MenuItem();
			this.menuItemXML = new System.Windows.Forms.MenuItem();
			this.menuItemProperties = new System.Windows.Forms.MenuItem();
			this.menuItemLayout = new System.Windows.Forms.MenuItem();
			this.menuItemShowGrid = new System.Windows.Forms.MenuItem();
			this.menuItemSnapToGrid = new System.Windows.Forms.MenuItem();
			this.menuItemAlign = new System.Windows.Forms.MenuItem();
			this.menuItemLefts = new System.Windows.Forms.MenuItem();
			this.menuItemRights = new System.Windows.Forms.MenuItem();
			this.menuItemTops = new System.Windows.Forms.MenuItem();
			this.menuItemBottoms = new System.Windows.Forms.MenuItem();
			this.menuItemMiddles = new System.Windows.Forms.MenuItem();
			this.menuItemCenters = new System.Windows.Forms.MenuItem();
			this.menuItemToGrid = new System.Windows.Forms.MenuItem();
			this.menuItemCenter = new System.Windows.Forms.MenuItem();
			this.menuItemHoriz = new System.Windows.Forms.MenuItem();
			this.menuItemVert = new System.Windows.Forms.MenuItem();
			this.menuItemSizeTo = new System.Windows.Forms.MenuItem();
			this.menuItemControl = new System.Windows.Forms.MenuItem();
			this.menuItemControlW = new System.Windows.Forms.MenuItem();
			this.menuItemControlH = new System.Windows.Forms.MenuItem();
			this.menuItemGrid = new System.Windows.Forms.MenuItem();
			this.menuItemZO = new System.Windows.Forms.MenuItem();
			this.menuItemBTF = new System.Windows.Forms.MenuItem();
			this.menuItemSTB = new System.Windows.Forms.MenuItem();
			this.menuItemTO = new System.Windows.Forms.MenuItem();
			this.menuItemDebug = new System.Windows.Forms.MenuItem();
			this.menuItemBuild = new System.Windows.Forms.MenuItem();
			this.menuItemRun = new System.Windows.Forms.MenuItem();
			this.menuItemStop = new System.Windows.Forms.MenuItem();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabDesign = new System.Windows.Forms.TabPage();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.panelMain = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.toolbox = new SampleDesignerApplication.ToolBoxPane();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.tabCS = new System.Windows.Forms.TabPage();
			this.textCS = new System.Windows.Forms.TextBox();
			this.tabVB = new System.Windows.Forms.TabPage();
			this.textVB = new System.Windows.Forms.TextBox();
			this.tabXML = new System.Windows.Forms.TabPage();
			this.textXML = new System.Windows.Forms.TextBox();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.tabControl.SuspendLayout();
			this.tabDesign.SuspendLayout();
			this.tabCS.SuspendLayout();
			this.tabVB.SuspendLayout();
			this.tabXML.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuItemFile,
																					 this.menuItemEdit,
																					 this.menuItemView,
																					 this.menuItemLayout,
																					 this.menuItemDebug});
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 0;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemNew,
																						 this.menuItemOpen,
																						 this.menuItemSave,
																						 this.menuItemSaveAs,
																						 this.menuItemExit});
			this.menuItemFile.Text = "&File";
			// 
			// menuItemNew
			// 
			this.menuItemNew.Index = 0;
			this.menuItemNew.Text = "&New";
			this.menuItemNew.Click += new System.EventHandler(this.menuItemNew_Click);
			// 
			// menuItemOpen
			// 
			this.menuItemOpen.Index = 1;
			this.menuItemOpen.Text = "&Open...";
			this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
			// 
			// menuItemSave
			// 
			this.menuItemSave.Enabled = false;
			this.menuItemSave.Index = 2;
			this.menuItemSave.Text = "&Save";
			this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
			// 
			// menuItemSaveAs
			// 
			this.menuItemSaveAs.Enabled = false;
			this.menuItemSaveAs.Index = 3;
			this.menuItemSaveAs.Text = "Save &As...";
			this.menuItemSaveAs.Click += new System.EventHandler(this.menuItemSaveAs_Click);
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 4;
			this.menuItemExit.Text = "&Exit";
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// menuItemEdit
			// 
			this.menuItemEdit.Enabled = false;
			this.menuItemEdit.Index = 1;
			this.menuItemEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemUndo,
																						 this.menuItemRedo,
																						 this.menuItemRule,
																						 this.menuItemCut,
																						 this.menuItemCopy,
																						 this.menuItemPaste,
																						 this.menuItemDelete,
																						 this.menuItemSelectAll});
			this.menuItemEdit.Text = "&Edit";
			// 
			// menuItemUndo
			// 
			this.menuItemUndo.Enabled = false;
			this.menuItemUndo.Index = 0;
			this.menuItemUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.menuItemUndo.Text = "Undo";
			// 
			// menuItemRedo
			// 
			this.menuItemRedo.Enabled = false;
			this.menuItemRedo.Index = 1;
			this.menuItemRedo.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
			this.menuItemRedo.Text = "Redo";
			// 
			// menuItemRule
			// 
			this.menuItemRule.Index = 2;
			this.menuItemRule.Text = "-";
			// 
			// menuItemCut
			// 
			this.menuItemCut.Enabled = false;
			this.menuItemCut.Index = 3;
			this.menuItemCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.menuItemCut.Text = "Cu&t";
			this.menuItemCut.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemCopy
			// 
			this.menuItemCopy.Enabled = false;
			this.menuItemCopy.Index = 4;
			this.menuItemCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.menuItemCopy.Text = "&Copy";
			this.menuItemCopy.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemPaste
			// 
			this.menuItemPaste.Enabled = false;
			this.menuItemPaste.Index = 5;
			this.menuItemPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
			this.menuItemPaste.Text = "&Paste";
			this.menuItemPaste.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemDelete
			// 
			this.menuItemDelete.Index = 6;
			this.menuItemDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
			this.menuItemDelete.Text = "&Delete";
			this.menuItemDelete.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemSelectAll
			// 
			this.menuItemSelectAll.Index = 7;
			this.menuItemSelectAll.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			this.menuItemSelectAll.Text = "Select &All";
			this.menuItemSelectAll.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemView
			// 
			this.menuItemView.Enabled = false;
			this.menuItemView.Index = 2;
			this.menuItemView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemServiceRequests,
																						 this.menuItemDesign,
																						 this.menuItemCSource,
																						 this.menuItemVBSource,
																						 this.menuItemXML,
																						 this.menuItemProperties});
			this.menuItemView.Text = "&View";
			// 
			// menuItemServiceRequests
			// 
			this.menuItemServiceRequests.Index = 0;
			this.menuItemServiceRequests.Text = "&Service Requests";
			this.menuItemServiceRequests.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemDesign
			// 
			this.menuItemDesign.Index = 1;
			this.menuItemDesign.Text = "&Design";
			this.menuItemDesign.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemCSource
			// 
			this.menuItemCSource.Index = 2;
			this.menuItemCSource.Text = "&C# Source";
			this.menuItemCSource.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemVBSource
			// 
			this.menuItemVBSource.Index = 3;
			this.menuItemVBSource.Text = "&VB Source";
			this.menuItemVBSource.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemXML
			// 
			this.menuItemXML.Index = 4;
			this.menuItemXML.Text = "&XML";
			this.menuItemXML.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemProperties
			// 
			this.menuItemProperties.Index = 5;
			this.menuItemProperties.Text = "&Properties";
			this.menuItemProperties.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemLayout
			// 
			this.menuItemLayout.Enabled = false;
			this.menuItemLayout.Index = 3;
			this.menuItemLayout.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.menuItemShowGrid,
																						   this.menuItemSnapToGrid,
																						   this.menuItemAlign,
																						   this.menuItemCenter,
																						   this.menuItemSizeTo,
																						   this.menuItemZO,
																						   this.menuItemTO});
			this.menuItemLayout.Text = "&Layout";
			// 
			// menuItemShowGrid
			// 
			this.menuItemShowGrid.Checked = true;
			this.menuItemShowGrid.Index = 0;
			this.menuItemShowGrid.Text = "Show &Grid";
			this.menuItemShowGrid.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemSnapToGrid
			// 
			this.menuItemSnapToGrid.Checked = true;
			this.menuItemSnapToGrid.Index = 1;
			this.menuItemSnapToGrid.Text = "S&nap to Grid";
			this.menuItemSnapToGrid.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemAlign
			// 
			this.menuItemAlign.Index = 2;
			this.menuItemAlign.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.menuItemLefts,
																						  this.menuItemRights,
																						  this.menuItemTops,
																						  this.menuItemBottoms,
																						  this.menuItemMiddles,
																						  this.menuItemCenters,
																						  this.menuItemToGrid});
			this.menuItemAlign.Text = "&Align...";
			// 
			// menuItemLefts
			// 
			this.menuItemLefts.Index = 0;
			this.menuItemLefts.Text = "&Lefts";
			this.menuItemLefts.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemRights
			// 
			this.menuItemRights.Index = 1;
			this.menuItemRights.Text = "&Rights";
			this.menuItemRights.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemTops
			// 
			this.menuItemTops.Index = 2;
			this.menuItemTops.Text = "&Tops";
			this.menuItemTops.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemBottoms
			// 
			this.menuItemBottoms.Index = 3;
			this.menuItemBottoms.Text = "&Bottoms";
			this.menuItemBottoms.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemMiddles
			// 
			this.menuItemMiddles.Index = 4;
			this.menuItemMiddles.Text = "&Middles";
			this.menuItemMiddles.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemCenters
			// 
			this.menuItemCenters.Index = 5;
			this.menuItemCenters.Text = "&Centers";
			this.menuItemCenters.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemToGrid
			// 
			this.menuItemToGrid.Index = 6;
			this.menuItemToGrid.Text = "to &Grid";
			this.menuItemToGrid.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemCenter
			// 
			this.menuItemCenter.Index = 3;
			this.menuItemCenter.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.menuItemHoriz,
																						   this.menuItemVert});
			this.menuItemCenter.Text = "&Center...";
			// 
			// menuItemHoriz
			// 
			this.menuItemHoriz.Index = 0;
			this.menuItemHoriz.Text = "&Horizontally";
			this.menuItemHoriz.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemVert
			// 
			this.menuItemVert.Index = 1;
			this.menuItemVert.Text = "&Vertically";
			this.menuItemVert.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemSizeTo
			// 
			this.menuItemSizeTo.Index = 4;
			this.menuItemSizeTo.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.menuItemControl,
																						   this.menuItemControlW,
																						   this.menuItemControlH,
																						   this.menuItemGrid});
			this.menuItemSizeTo.Text = "&Size to...";
			// 
			// menuItemControl
			// 
			this.menuItemControl.Index = 0;
			this.menuItemControl.Text = "&Control";
			this.menuItemControl.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemControlW
			// 
			this.menuItemControlW.Index = 1;
			this.menuItemControlW.Text = "Control &Width";
			this.menuItemControlW.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemControlH
			// 
			this.menuItemControlH.Index = 2;
			this.menuItemControlH.Text = "Control &Height";
			this.menuItemControlH.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemGrid
			// 
			this.menuItemGrid.Index = 3;
			this.menuItemGrid.Text = "&Grid";
			this.menuItemGrid.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemZO
			// 
			this.menuItemZO.Index = 5;
			this.menuItemZO.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuItemBTF,
																					   this.menuItemSTB});
			this.menuItemZO.Text = "&Z Order";
			// 
			// menuItemBTF
			// 
			this.menuItemBTF.Index = 0;
			this.menuItemBTF.Text = "&Bring to Front";
			this.menuItemBTF.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemSTB
			// 
			this.menuItemSTB.Index = 1;
			this.menuItemSTB.Text = "&Send to Back";
			this.menuItemSTB.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemTO
			// 
			this.menuItemTO.Index = 6;
			this.menuItemTO.Text = "&Tab Order";
			this.menuItemTO.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItemDebug
			// 
			this.menuItemDebug.Enabled = false;
			this.menuItemDebug.Index = 4;
			this.menuItemDebug.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.menuItemBuild,
																						  this.menuItemRun,
																						  this.menuItemStop});
			this.menuItemDebug.Text = "&Debug";
			// 
			// menuItemBuild
			// 
			this.menuItemBuild.Index = 0;
			this.menuItemBuild.Text = "&Build...";
			this.menuItemBuild.Click += new System.EventHandler(this.menuItemBuild_Click);
			// 
			// menuItemRun
			// 
			this.menuItemRun.Index = 1;
			this.menuItemRun.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.menuItemRun.Text = "&Run";
			this.menuItemRun.Click += new System.EventHandler(this.menuItemRun_Click);
			// 
			// menuItemStop
			// 
			this.menuItemStop.Index = 2;
			this.menuItemStop.Text = "&Stop";
			this.menuItemStop.Click += new System.EventHandler(this.menuItemStop_Click);
			// 
			// tabControl
			// 
			this.tabControl.Controls.AddRange(new System.Windows.Forms.Control[] {
																					 this.tabDesign,
																					 this.tabCS,
																					 this.tabVB,
																					 this.tabXML});
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tabControl.ItemSize = new System.Drawing.Size(120, 25);
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(760, 537);
			this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabControl.TabIndex = 0;
			this.tabControl.Visible = false;
			this.tabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl_DrawItem);
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// tabDesign
			// 
			this.tabDesign.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.splitter2,
																					this.panelMain,
																					this.splitter1,
																					this.toolbox,
																					this.propertyGrid});
			this.tabDesign.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tabDesign.Location = new System.Drawing.Point(4, 29);
			this.tabDesign.Name = "tabDesign";
			this.tabDesign.Size = new System.Drawing.Size(752, 504);
			this.tabDesign.TabIndex = 0;
			this.tabDesign.Text = "Design";
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter2.Location = new System.Drawing.Point(520, 0);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(3, 504);
			this.splitter2.TabIndex = 12;
			this.splitter2.TabStop = false;
			// 
			// panelMain
			// 
			this.panelMain.BackColor = System.Drawing.SystemColors.Window;
			this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.panelMain.Location = new System.Drawing.Point(291, 0);
			this.panelMain.Name = "panelMain";
			this.panelMain.Size = new System.Drawing.Size(232, 504);
			this.panelMain.TabIndex = 11;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(288, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 504);
			this.splitter1.TabIndex = 10;
			this.splitter1.TabStop = false;
			// 
			// toolbox
			// 
			this.toolbox.BackColor = System.Drawing.Color.Black;
			this.toolbox.Dock = System.Windows.Forms.DockStyle.Left;
			this.toolbox.Host = null;
			this.toolbox.Location = new System.Drawing.Point(0, 0);
			this.toolbox.Name = "toolbox";
			this.toolbox.Size = new System.Drawing.Size(288, 504);
			this.toolbox.TabIndex = 9;
			// 
			// propertyGrid
			// 
			this.propertyGrid.BackColor = System.Drawing.SystemColors.Control;
			this.propertyGrid.CausesValidation = false;
			this.propertyGrid.CommandsVisibleIfAvailable = true;
			this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Right;
			this.propertyGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.propertyGrid.LargeButtons = false;
			this.propertyGrid.LineColor = System.Drawing.Color.LightSlateGray;
			this.propertyGrid.Location = new System.Drawing.Point(523, 0);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new System.Drawing.Size(229, 504);
			this.propertyGrid.TabIndex = 7;
			this.propertyGrid.Text = "propertyGrid";
			this.propertyGrid.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// tabCS
			// 
			this.tabCS.Controls.AddRange(new System.Windows.Forms.Control[] {
																				this.textCS});
			this.tabCS.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tabCS.Location = new System.Drawing.Point(4, 29);
			this.tabCS.Name = "tabCS";
			this.tabCS.Size = new System.Drawing.Size(752, 504);
			this.tabCS.TabIndex = 2;
			this.tabCS.Text = "C# Source";
			// 
			// textCS
			// 
			this.textCS.BackColor = System.Drawing.Color.DarkSlateGray;
			this.textCS.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textCS.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textCS.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textCS.ForeColor = System.Drawing.Color.Gold;
			this.textCS.Location = new System.Drawing.Point(0, 0);
			this.textCS.Multiline = true;
			this.textCS.Name = "textCS";
			this.textCS.ReadOnly = true;
			this.textCS.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textCS.Size = new System.Drawing.Size(752, 504);
			this.textCS.TabIndex = 1;
			this.textCS.Text = "";
			this.textCS.WordWrap = false;
			// 
			// tabVB
			// 
			this.tabVB.Controls.AddRange(new System.Windows.Forms.Control[] {
																				this.textVB});
			this.tabVB.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tabVB.Location = new System.Drawing.Point(4, 29);
			this.tabVB.Name = "tabVB";
			this.tabVB.Size = new System.Drawing.Size(752, 504);
			this.tabVB.TabIndex = 1;
			this.tabVB.Text = "PABC.NET";
			// 
			// textVB
			// 
			this.textVB.BackColor = System.Drawing.Color.DarkSlateGray;
			this.textVB.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textVB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textVB.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textVB.ForeColor = System.Drawing.Color.Tomato;
			this.textVB.Location = new System.Drawing.Point(0, 0);
			this.textVB.Multiline = true;
			this.textVB.Name = "textVB";
			this.textVB.ReadOnly = true;
			this.textVB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textVB.Size = new System.Drawing.Size(752, 504);
			this.textVB.TabIndex = 0;
			this.textVB.Text = "";
			this.textVB.WordWrap = false;
			// 
			// tabXML
			// 
			this.tabXML.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.textXML});
			this.tabXML.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tabXML.Location = new System.Drawing.Point(4, 29);
			this.tabXML.Name = "tabXML";
			this.tabXML.Size = new System.Drawing.Size(752, 504);
			this.tabXML.TabIndex = 3;
			this.tabXML.Text = "XML";
			// 
			// textXML
			// 
			this.textXML.BackColor = System.Drawing.Color.DarkSlateGray;
			this.textXML.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textXML.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textXML.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textXML.ForeColor = System.Drawing.Color.LimeGreen;
			this.textXML.Location = new System.Drawing.Point(0, 0);
			this.textXML.Multiline = true;
			this.textXML.Name = "textXML";
			this.textXML.ReadOnly = true;
			this.textXML.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textXML.Size = new System.Drawing.Size(752, 504);
			this.textXML.TabIndex = 1;
			this.textXML.Text = "";
			this.textXML.WordWrap = false;
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "XML Files|*.xml";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(760, 537);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl});
			this.Menu = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "Simple Designer Host Sample";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.tabControl.ResumeLayout(false);
			this.tabDesign.ResumeLayout(false);
			this.tabCS.ResumeLayout(false);
			this.tabVB.ResumeLayout(false);
			this.tabXML.ResumeLayout(false);
			this.ResumeLayout(false);

		}
        
        //[System.STAThreadAttribute()]
        //public static void Main()
        //{
        //    System.Windows.Forms.Application.Run(new MainForm());
        //}

		private void menuItemNew_Click(object sender, System.EventArgs e)
		{
            if (DestroyDesigner()) // make sure we're clear for a new designer
            {
				// A loader created with no parameters creates a blank document.
                SampleDesignerLoader designerLoader = new SampleDesignerLoader();
                CreateDesigner(designerLoader);
            }
		}

		private void menuItemOpen_Click(object sender, System.EventArgs e)
		{
			if (DestroyDesigner()) // make sure we're clear for a new designer
			{
				if (openFileDialog.ShowDialog(this) == DialogResult.OK)
				{
					// Load up this file (XML is all we support for now).
					SampleDesignerLoader designerLoader = new SampleDesignerLoader(openFileDialog.FileName);
					CreateDesigner(designerLoader);
				}
			}
		}

		/// The user gets to choose between saving in XML, C#, or VB.
		/// Just remember that you can only load XML.
		private void menuItemSave_Click(object sender, System.EventArgs e)
		{
			loader.Save(false); // false == don't force file dialog if file already chosen
		}

		/// The user gets to choose between saving in XML, C#, or VB.
		/// Just remember that you can only load XML.
		private void menuItemSaveAs_Click(object sender, System.EventArgs e)
		{
			loader.Save(true); // true == force file dialog even if file already chosen
		}

		/// Try to close our form. Our OnClosing method will check to make sure our stuff
		/// is saved before closing.
		private void menuItemExit_Click(object sender, System.EventArgs e)
		{
			if (DestroyDesigner())
			{
				this.Close();
			}
		}

		/// The loader also handles building. It keeps a codeCompileUnit that represents
		/// the state of the document at any time. The user will be prompted for an executable
		/// filename the first time there's a build.
		private void menuItemBuild_Click(object sender, System.EventArgs e)
		{
			loader.Build();
		}

		/// This builds to the executable the user has selected and then runs it.
		private void menuItemRun_Click(object sender, System.EventArgs e)
		{
			loader.Run();
		}

		/// Another way to kill any running executable.
		private void menuItemStop_Click(object sender, System.EventArgs e)
		{
			loader.Stop();
		}

		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// If the tab is changing, that means we'd better let the loader know it needs
			// to flush changes that have been made and update the code windows.
			//
			loader.Flush();
		}		

		/// The rest of the MenuItem click events are handled here. Most of these MenuItems
		/// require a loaded host to have any applicable use--thus they are disabled
		/// if there isn't one.
		private void menuItem_Click(object sender, System.EventArgs e)
		{
			// The IMenuCommandService makes doing common commands easy. 
			// It keeps track of what commands and verbs the designer supports
			// and can invoke them given members of the MenuCommands enum (CommandID's).
			IServiceContainer sc = host as IServiceContainer;
			IMenuCommandService mcs = sc.GetService(typeof(IMenuCommandService)) as IMenuCommandService;

			switch ((sender as MenuItem).Text)
			{
				case "Cu&t": mcs.GlobalInvoke(StandardCommands.Cut);
					break;
				case "&Copy": mcs.GlobalInvoke(StandardCommands.Copy);
					break;
				case "&Paste": mcs.GlobalInvoke(StandardCommands.Paste);
					break;
				case "&Delete": mcs.GlobalInvoke(StandardCommands.Delete);
					break;
				case "Select &All": mcs.GlobalInvoke(StandardCommands.SelectAll);
					break;
				case "&Service Requests":
					if (serviceRequests == null) 
					{
						serviceRequests = new ServiceRequests();
						serviceRequests.Closed += new EventHandler(OnServiceRequestsClosed);

						// Our designer host looks for this service to announce the success / failure
						// of service requests.
						hostingServiceContainer.AddService(typeof(ServiceRequests), serviceRequests);
						serviceRequests.Show();
					}
					serviceRequests.Activate();
					break;
				case "&Design": tabControl.SelectedTab = tabDesign;
					break;
				case "&C# Source": tabControl.SelectedTab = tabCS;
					break;
				case "&VB Source": tabControl.SelectedTab = tabVB;
					break;
				case "&XML": tabControl.SelectedTab = tabXML;
					break;
				case "&Properties": mcs.GlobalInvoke(MenuCommands.Properties);
					break;
				case "Show &Grid":
					mcs.GlobalInvoke(StandardCommands.ShowGrid);
					menuItemShowGrid.Checked = !menuItemShowGrid.Checked;
					break;
				case "S&nap to Grid":
					mcs.GlobalInvoke(StandardCommands.SnapToGrid);
					menuItemSnapToGrid.Checked = !menuItemSnapToGrid.Checked;
					break;
				case "&Lefts": mcs.GlobalInvoke(StandardCommands.AlignLeft);
					break;
				case "&Rights": mcs.GlobalInvoke(StandardCommands.AlignRight);
					break;
				case "&Tops": mcs.GlobalInvoke(StandardCommands.AlignTop);
					break;
				case "&Bottoms": mcs.GlobalInvoke(StandardCommands.AlignBottom);
					break;
				case "&Middles": mcs.GlobalInvoke(StandardCommands.AlignHorizontalCenters);
					break;
				case "&Centers": mcs.GlobalInvoke(StandardCommands.AlignVerticalCenters);
					break;
				case "to &Grid": mcs.GlobalInvoke(StandardCommands.AlignToGrid);
					break;
				case "&Horizontally": mcs.GlobalInvoke(StandardCommands.CenterHorizontally);
					break;
				case "&Vertically": mcs.GlobalInvoke(StandardCommands.CenterVertically);
					break;
				case "&Control": mcs.GlobalInvoke(StandardCommands.SizeToControl);
					break;
				case "Control &Width": mcs.GlobalInvoke(StandardCommands.SizeToControlWidth);
					break;
				case "Control &Height": mcs.GlobalInvoke(StandardCommands.SizeToControlHeight);
					break;
				case "&Grid": mcs.GlobalInvoke(StandardCommands.SizeToGrid);
					break;
				case "&Bring to Front": mcs.GlobalInvoke(StandardCommands.BringToFront);
					break;
				case "&Send to Back": mcs.GlobalInvoke(StandardCommands.SendToBack);
					break;
				case "&Tab Order": mcs.GlobalInvoke(StandardCommands.TabOrder);
					break;
			}
		}

		/// Put away our serviceRequests service if no one's looking.
		private void OnServiceRequestsClosed(object sender, EventArgs e) 
		{
			serviceRequests = null;
			hostingServiceContainer.RemoveService(typeof(ServiceRequests));
		}

		/// OwnerDraw for that rainbow look.
		private void tabControl_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			TabControl tcSender = sender as TabControl;
			string text = tcSender.TabPages[e.Index].Text;
			Size textSize = e.Graphics.MeasureString(text, tcSender.Font).ToSize();

			switch (e.Index)
			{
				case 0: e.Graphics.FillRectangle(Brushes.SteelBlue, e.Bounds);
					break;
				case 1: e.Graphics.FillRectangle(Brushes.Gold, e.Bounds);
					break;
				case 2: e.Graphics.FillRectangle(Brushes.Tomato, e.Bounds);
					break;
				case 3: e.Graphics.FillRectangle(Brushes.LimeGreen, e.Bounds);
					break;
			}

			Point textLocation = new Point(
				e.Bounds.X + (e.Bounds.Width - textSize.Width) / 2,
				e.Bounds.Y + (e.Bounds.Height - textSize.Height) / 2);
			e.Graphics.DrawString(text, tcSender.Font, Brushes.White, textLocation);
		}
    }
}
