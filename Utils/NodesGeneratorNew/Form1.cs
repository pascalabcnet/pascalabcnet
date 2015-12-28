using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Windows;
using Irony.Parsing;
using Irony.Samples.CSharp;
using System.Linq;
using System.Reflection;

using NodeGenerator;

namespace NodesGenerator
{
    enum CloseResult { Saved, NotSaved, NotClosed };
    
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
    {
        //consts
        private const int max_str_len = 80;

        //C# parser-related        
        CSharpGrammar _grammar;
        LanguageData _language;
        Parser _parser;
        private readonly string parser_tech_data = "namespace NS1 { class MyClass {\n";
        HashSet<string> parser_errors_ignore_states = new HashSet<string>(new string[] {"S109", "S315"});

        //routines
        private string default_caption;
        private node_info selected_node = null;
        private node_info new_selected_node = null; //this is needed, 'cause SelectedIndexChanged of ListBox is dirty-programmed in C#
        private int selected_node_index = -1;
        private bool creation_mode = false;
        private bool nodes_list_SelectedIndexChanged_recur = false;
        private bool is_filter_enabled = false;
        //Hierarchy Browser
        private Point hb_location = new Point(-1, -1);

        //Options
        private bool auto_save = true;
        private string version_storage = "Storage";
        private List<string> autocompletion_list = new List<string>();

        //tags
        private List<Tuple<int, string>> temp_tags = new List<Tuple<int, string>>();
        private int selected_tag_cat = -1;
        private bool update_tags = false;

        //filters
        private List<List<bool>> current_filter;
        private int selected_filter_cat = -1;
        private const int filter_panel_height = 92;

        //visuals
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private StatusStrip NavigationPanel;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStrip mainToolStrip;
        private ToolStripButton toolStripOpenButton;
        private ToolStripButton toolStripSaveButton;
        private ToolStripButton toolStripSaveAsButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripGenerateButton;
        private ToolStripButton toolStripOptionsButton;
        private ToolStripSeparator toolStripSeparator2;
        private GroupBox groupBox1;
        internal ListBox nodes_list;
        private GroupBox groupBox2;
        private ToolStrip nodesToolStrip;
        private ToolStripButton toolStripButtonAddNode;
        private ToolStripButton toolStripButtonDeleteNode;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton toolStripButtonMoveUp;
        private ToolStripButton toolStripButtonMoveDown;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripButton toolStripButtonSort;
        private Panel panel2;
        private TextBox help_context;
        private Label label3;
        private Panel panel1;
        private ComboBox base_class;
        private Label label2;
        private Label label1;
        private TextBox node_name;
        private Panel panel3;
        private ScintillaNET.Scintilla scintilla1;
        private ToolStrip edtiorToolStrip;
        private ToolStripButton toolStripButtonCut;
        private ToolStripButton toolStripButtonCopy;
        private ToolStripButton toolStripButtonPaste;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripButton toolStripButtonUndo;
        private ToolStripButton toolStripButtonRedo;
        private Panel panel4;
        private Label label4;
        private TextBox tagItems;
        private ListBox tagCategories;
        private Button reloadButton;
        private Panel panel5;
        private Panel panel6;
        private Button resetFilterButton;
        private Panel panelFilterButtons;
        private ListView filterItems;
        private ListBox filterCategories;
        private Splitter splitter1;
        private Splitter splitter2;
        private Button saveNodeButton;
        private ToolStripButton toolStripButtonNew;
        private ContextMenuStrip nodesContextMenu;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripMenuItem moveUpToolStripMenuItem;
        private ToolStripMenuItem moveDownToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem quitToolStripMenuItem;
        private ToolStripMenuItem nodesToolStripMenuItem;
        private ToolStripMenuItem addNodeToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem moveNodeUpToolStripMenuItem;
        private ToolStripMenuItem moveNodeDownToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem deleteNodeToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem sortToolStripMenuItem;
        private ToolStripMenuItem contentsToolStripMenuItem;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem filterPanelToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem1;
        private ToolStripMenuItem tagsCollectionToolStripMenuItem;
        private ToolStripMenuItem runToolStripMenuItem;
        private ToolStripMenuItem generateToolStripMenuItem;
        private MenuStrip mainMenu;
        private ToolStripMenuItem globalRenamerToolStripMenuItem;
        private ToolStripButton toolStripButtonSaveStore;
        private ToolStripMenuItem saveAndStoreToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ContextMenuStrip filtersPanelContextMenu;
        private ToolStripMenuItem editInTagsCollectionToolStripMenuItem;
        private ToolStripMenuItem grammarBrowserToolStripMenuItem;
        private Label tags;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripButton toolStripButtonTagSelected;
        private Button filterSelected;
        private ToolStripMenuItem присвоитьТегToolStripMenuItem;
        private Splitter splitter3;
        private Button filterAncestors;
        private IContainer components;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.NavigationPanel = new System.Windows.Forms.StatusStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripOpenButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSaveButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSaveAsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveStore = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripOptionsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripGenerateButton = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nodes_list = new System.Windows.Forms.ListBox();
            this.nodesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodesToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddNode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonMoveUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMoveDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonDeleteNode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSort = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonTagSelected = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.scintilla1 = new ScintillaNET.Scintilla();
            this.edtiorToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonCut = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCopy = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonUndo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRedo = new System.Windows.Forms.ToolStripButton();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.help_context = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tags = new System.Windows.Forms.Label();
            this.tagItems = new System.Windows.Forms.TextBox();
            this.tagCategories = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.saveNodeButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.base_class = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.node_name = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.filterItems = new System.Windows.Forms.ListView();
            this.filtersPanelContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editInTagsCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterCategories = new System.Windows.Forms.ListBox();
            this.panelFilterButtons = new System.Windows.Forms.Panel();
            this.filterSelected = new System.Windows.Forms.Button();
            this.filterAncestors = new System.Windows.Forms.Button();
            this.resetFilterButton = new System.Windows.Forms.Button();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAndStoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.moveNodeUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveNodeDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.присвоитьТегToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.globalRenamerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tagsCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grammarBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.NavigationPanel.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.nodesContextMenu.SuspendLayout();
            this.nodesToolStrip.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).BeginInit();
            this.edtiorToolStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.filtersPanelContextMenu.SuspendLayout();
            this.panelFilterButtons.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "nin";
            this.openFileDialog1.Filter = "Файлы синтаксического дерева (*.nin)|*.nin|Все файлы (*.*)|*.*";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "nin";
            this.saveFileDialog1.Filter = "Файлы синтаксического дерева (*.nin)|*.nin|Все файлы (*.*)|*.*";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // NavigationPanel
            // 
            this.NavigationPanel.Font = new System.Drawing.Font("Tahoma", 9F);
            this.NavigationPanel.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.NavigationPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripStatusLabel2});
            this.NavigationPanel.Location = new System.Drawing.Point(0, 696);
            this.NavigationPanel.Name = "NavigationPanel";
            this.NavigationPanel.Size = new System.Drawing.Size(1088, 28);
            this.NavigationPanel.SizingGrip = false;
            this.NavigationPanel.TabIndex = 18;
            this.NavigationPanel.Resize += new System.EventHandler(this.NavigationPanel_Resize);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(34, 24);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 23);
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNew,
            this.toolStripOpenButton,
            this.toolStripSaveButton,
            this.toolStripSaveAsButton,
            this.toolStripButtonSaveStore,
            this.toolStripSeparator1,
            this.toolStripOptionsButton,
            this.toolStripSeparator2,
            this.toolStripGenerateButton});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(1088, 27);
            this.mainToolStrip.TabIndex = 19;
            this.mainToolStrip.Text = "toolStrip1";
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNew.Image")));
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonNew.Text = "Новый";
            this.toolStripButtonNew.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // toolStripOpenButton
            // 
            this.toolStripOpenButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripOpenButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripOpenButton.Image")));
            this.toolStripOpenButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOpenButton.Name = "toolStripOpenButton";
            this.toolStripOpenButton.Size = new System.Drawing.Size(24, 24);
            this.toolStripOpenButton.Text = "Открыть...";
            this.toolStripOpenButton.Click += new System.EventHandler(this.open_Click);
            // 
            // toolStripSaveButton
            // 
            this.toolStripSaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSaveButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSaveButton.Image")));
            this.toolStripSaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSaveButton.Name = "toolStripSaveButton";
            this.toolStripSaveButton.Size = new System.Drawing.Size(24, 24);
            this.toolStripSaveButton.Text = "Сохранить";
            this.toolStripSaveButton.Click += new System.EventHandler(this.save_Click);
            // 
            // toolStripSaveAsButton
            // 
            this.toolStripSaveAsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSaveAsButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSaveAsButton.Image")));
            this.toolStripSaveAsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSaveAsButton.Name = "toolStripSaveAsButton";
            this.toolStripSaveAsButton.Size = new System.Drawing.Size(24, 24);
            this.toolStripSaveAsButton.Text = "Сохранить как...";
            this.toolStripSaveAsButton.Click += new System.EventHandler(this.save_as_Click);
            // 
            // toolStripButtonSaveStore
            // 
            this.toolStripButtonSaveStore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveStore.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveStore.Image")));
            this.toolStripButtonSaveStore.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveStore.Name = "toolStripButtonSaveStore";
            this.toolStripButtonSaveStore.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonSaveStore.Text = "Сохранить и сделать копию";
            this.toolStripButtonSaveStore.Click += new System.EventHandler(this.toolStripButtonSaveStore_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripOptionsButton
            // 
            this.toolStripOptionsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripOptionsButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripOptionsButton.Image")));
            this.toolStripOptionsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOptionsButton.Name = "toolStripOptionsButton";
            this.toolStripOptionsButton.Size = new System.Drawing.Size(24, 24);
            this.toolStripOptionsButton.Text = "Настройки...";
            this.toolStripOptionsButton.Click += new System.EventHandler(this.toolStripOptionsButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripGenerateButton
            // 
            this.toolStripGenerateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripGenerateButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripGenerateButton.Image")));
            this.toolStripGenerateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripGenerateButton.Name = "toolStripGenerateButton";
            this.toolStripGenerateButton.Size = new System.Drawing.Size(24, 24);
            this.toolStripGenerateButton.Text = "Генерировать";
            this.toolStripGenerateButton.Click += new System.EventHandler(this.toolStripGenerateButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nodes_list);
            this.groupBox1.Controls.Add(this.nodesToolStrip);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 164);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(295, 532);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Узлы:";
            // 
            // nodes_list
            // 
            this.nodes_list.ContextMenuStrip = this.nodesContextMenu;
            this.nodes_list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodes_list.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nodes_list.IntegralHeight = false;
            this.nodes_list.ItemHeight = 20;
            this.nodes_list.Location = new System.Drawing.Point(3, 45);
            this.nodes_list.Name = "nodes_list";
            this.nodes_list.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.nodes_list.Size = new System.Drawing.Size(289, 484);
            this.nodes_list.TabIndex = 4;
            this.nodes_list.SelectedIndexChanged += new System.EventHandler(this.nodes_list_SelectedIndexChanged);
            this.nodes_list.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nodes_list_KeyDown);
            this.nodes_list.MouseDown += new System.Windows.Forms.MouseEventHandler(this.nodes_list_MouseMove);
            this.nodes_list.MouseMove += new System.Windows.Forms.MouseEventHandler(this.nodes_list_MouseMove);
            // 
            // nodesContextMenu
            // 
            this.nodesContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.nodesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.toolStripMenuItem6,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.toolStripMenuItem7,
            this.deleteToolStripMenuItem});
            this.nodesContextMenu.Name = "contextMenuStrip1";
            this.nodesContextMenu.Size = new System.Drawing.Size(216, 120);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addToolStripMenuItem.Image")));
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(215, 26);
            this.addToolStripMenuItem.Text = "Добавить";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.add_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(212, 6);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("moveUpToolStripMenuItem.Image")));
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(215, 26);
            this.moveUpToolStripMenuItem.Text = "Передвинуть вверх";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.Up_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("moveDownToolStripMenuItem.Image")));
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(215, 26);
            this.moveDownToolStripMenuItem.Text = "Передвинуть вниз";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.Down_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(212, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(215, 26);
            this.deleteToolStripMenuItem.Text = "Удалить";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.delete_Click);
            // 
            // nodesToolStrip
            // 
            this.nodesToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.nodesToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddNode,
            this.toolStripSeparator3,
            this.toolStripButtonMoveUp,
            this.toolStripButtonMoveDown,
            this.toolStripSeparator4,
            this.toolStripButtonDeleteNode,
            this.toolStripSeparator5,
            this.toolStripButtonSort,
            this.toolStripSeparator7,
            this.toolStripButtonTagSelected});
            this.nodesToolStrip.Location = new System.Drawing.Point(3, 18);
            this.nodesToolStrip.Name = "nodesToolStrip";
            this.nodesToolStrip.Size = new System.Drawing.Size(289, 27);
            this.nodesToolStrip.TabIndex = 27;
            this.nodesToolStrip.Text = "toolStrip2";
            // 
            // toolStripButtonAddNode
            // 
            this.toolStripButtonAddNode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddNode.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddNode.Image")));
            this.toolStripButtonAddNode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddNode.Name = "toolStripButtonAddNode";
            this.toolStripButtonAddNode.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonAddNode.Text = "Add Node";
            this.toolStripButtonAddNode.Click += new System.EventHandler(this.add_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButtonMoveUp
            // 
            this.toolStripButtonMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMoveUp.Image")));
            this.toolStripButtonMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMoveUp.Name = "toolStripButtonMoveUp";
            this.toolStripButtonMoveUp.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonMoveUp.Text = "Move Up";
            this.toolStripButtonMoveUp.Click += new System.EventHandler(this.Up_Click);
            // 
            // toolStripButtonMoveDown
            // 
            this.toolStripButtonMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMoveDown.Image")));
            this.toolStripButtonMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMoveDown.Name = "toolStripButtonMoveDown";
            this.toolStripButtonMoveDown.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonMoveDown.Text = "Move Down";
            this.toolStripButtonMoveDown.Click += new System.EventHandler(this.Down_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButtonDeleteNode
            // 
            this.toolStripButtonDeleteNode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDeleteNode.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDeleteNode.Image")));
            this.toolStripButtonDeleteNode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeleteNode.Name = "toolStripButtonDeleteNode";
            this.toolStripButtonDeleteNode.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonDeleteNode.Text = "Delete Node";
            this.toolStripButtonDeleteNode.Click += new System.EventHandler(this.delete_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButtonSort
            // 
            this.toolStripButtonSort.CheckOnClick = true;
            this.toolStripButtonSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSort.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSort.Image")));
            this.toolStripButtonSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSort.Name = "toolStripButtonSort";
            this.toolStripButtonSort.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonSort.Text = "Sort";
            this.toolStripButtonSort.CheckedChanged += new System.EventHandler(this.toolStripButton1_CheckedChanged);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButtonTagSelected
            // 
            this.toolStripButtonTagSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTagSelected.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTagSelected.Image")));
            this.toolStripButtonTagSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTagSelected.Name = "toolStripButtonTagSelected";
            this.toolStripButtonTagSelected.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonTagSelected.Text = "Присвоить тег";
            this.toolStripButtonTagSelected.Click += new System.EventHandler(this.saveTempFilter_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Controls.Add(this.splitter2);
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Controls.Add(this.splitter1);
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(295, 164);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(793, 532);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Содержимое узлов:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.scintilla1);
            this.panel3.Controls.Add(this.edtiorToolStrip);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 73);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(787, 165);
            this.panel3.TabIndex = 29;
            // 
            // scintilla1
            // 
            this.scintilla1.AutoComplete.IsCaseSensitive = false;
            this.scintilla1.AutoComplete.ListString = "";
            this.scintilla1.AutoComplete.MaxHeight = 10;
            this.scintilla1.ConfigurationManager.Language = "cs";
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Indentation.IndentWidth = 2;
            this.scintilla1.Indentation.SmartIndentType = ScintillaNET.SmartIndent.CPP;
            this.scintilla1.Location = new System.Drawing.Point(0, 27);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(787, 138);
            this.scintilla1.Styles.BraceBad.Size = 7F;
            this.scintilla1.Styles.BraceLight.Size = 7F;
            this.scintilla1.Styles.ControlChar.Size = 7F;
            this.scintilla1.Styles.Default.BackColor = System.Drawing.SystemColors.Window;
            this.scintilla1.Styles.Default.Size = 7F;
            this.scintilla1.Styles.IndentGuide.Size = 7F;
            this.scintilla1.Styles.LastPredefined.Size = 7F;
            this.scintilla1.Styles.LineNumber.Size = 7F;
            this.scintilla1.Styles.Max.Size = 7F;
            this.scintilla1.TabIndex = 23;
            // 
            // edtiorToolStrip
            // 
            this.edtiorToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.edtiorToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonCut,
            this.toolStripButtonCopy,
            this.toolStripButtonPaste,
            this.toolStripSeparator6,
            this.toolStripButtonUndo,
            this.toolStripButtonRedo});
            this.edtiorToolStrip.Location = new System.Drawing.Point(0, 0);
            this.edtiorToolStrip.Name = "edtiorToolStrip";
            this.edtiorToolStrip.Size = new System.Drawing.Size(787, 27);
            this.edtiorToolStrip.TabIndex = 24;
            this.edtiorToolStrip.Text = "toolStrip3";
            // 
            // toolStripButtonCut
            // 
            this.toolStripButtonCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCut.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCut.Image")));
            this.toolStripButtonCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCut.Name = "toolStripButtonCut";
            this.toolStripButtonCut.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonCut.Text = "Cut";
            this.toolStripButtonCut.Click += new System.EventHandler(this.toolStripButtonCut_Click);
            // 
            // toolStripButtonCopy
            // 
            this.toolStripButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCopy.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCopy.Image")));
            this.toolStripButtonCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCopy.Name = "toolStripButtonCopy";
            this.toolStripButtonCopy.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonCopy.Text = "Copy";
            this.toolStripButtonCopy.Click += new System.EventHandler(this.toolStripButtonCopy_Click);
            // 
            // toolStripButtonPaste
            // 
            this.toolStripButtonPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPaste.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPaste.Image")));
            this.toolStripButtonPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPaste.Name = "toolStripButtonPaste";
            this.toolStripButtonPaste.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonPaste.Text = "Paste";
            this.toolStripButtonPaste.Click += new System.EventHandler(this.toolStripButtonPaste_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButtonUndo
            // 
            this.toolStripButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUndo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUndo.Image")));
            this.toolStripButtonUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUndo.Name = "toolStripButtonUndo";
            this.toolStripButtonUndo.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonUndo.Text = "Undo";
            this.toolStripButtonUndo.Click += new System.EventHandler(this.toolStripButtonUndo_Click);
            // 
            // toolStripButtonRedo
            // 
            this.toolStripButtonRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRedo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRedo.Image")));
            this.toolStripButtonRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRedo.Name = "toolStripButtonRedo";
            this.toolStripButtonRedo.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonRedo.Text = "Redo";
            this.toolStripButtonRedo.Click += new System.EventHandler(this.toolStripButtonRedo_Click);
            // 
            // splitter2
            // 
            this.splitter2.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(3, 238);
            this.splitter2.MinExtra = 40;
            this.splitter2.MinSize = 40;
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(787, 3);
            this.splitter2.TabIndex = 30;
            this.splitter2.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.help_context);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 241);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(787, 117);
            this.panel2.TabIndex = 28;
            // 
            // help_context
            // 
            this.help_context.Dock = System.Windows.Forms.DockStyle.Fill;
            this.help_context.Location = new System.Drawing.Point(0, 18);
            this.help_context.Multiline = true;
            this.help_context.Name = "help_context";
            this.help_context.Size = new System.Drawing.Size(787, 99);
            this.help_context.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(787, 18);
            this.label3.TabIndex = 26;
            this.label3.Text = "Справочная информация:";
            // 
            // splitter1
            // 
            this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(3, 358);
            this.splitter1.MinExtra = 40;
            this.splitter1.MinSize = 40;
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(787, 3);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tags);
            this.panel4.Controls.Add(this.tagItems);
            this.panel4.Controls.Add(this.tagCategories);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(3, 361);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(787, 168);
            this.panel4.TabIndex = 27;
            // 
            // tags
            // 
            this.tags.AutoSize = true;
            this.tags.Location = new System.Drawing.Point(269, 0);
            this.tags.Name = "tags";
            this.tags.Size = new System.Drawing.Size(42, 17);
            this.tags.TabIndex = 3;
            this.tags.Text = "Теги:";
            // 
            // tagItems
            // 
            this.tagItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagItems.Location = new System.Drawing.Point(269, 17);
            this.tagItems.Multiline = true;
            this.tagItems.Name = "tagItems";
            this.tagItems.Size = new System.Drawing.Size(518, 151);
            this.tagItems.TabIndex = 2;
            this.tagItems.TextChanged += new System.EventHandler(this.tagItems_TextChanged);
            // 
            // tagCategories
            // 
            this.tagCategories.Dock = System.Windows.Forms.DockStyle.Left;
            this.tagCategories.FormattingEnabled = true;
            this.tagCategories.IntegralHeight = false;
            this.tagCategories.ItemHeight = 16;
            this.tagCategories.Location = new System.Drawing.Point(0, 17);
            this.tagCategories.Name = "tagCategories";
            this.tagCategories.Size = new System.Drawing.Size(269, 151);
            this.tagCategories.TabIndex = 1;
            this.tagCategories.SelectedIndexChanged += new System.EventHandler(this.tagCategories_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Категории:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.saveNodeButton);
            this.panel1.Controls.Add(this.reloadButton);
            this.panel1.Controls.Add(this.base_class);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.node_name);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(787, 55);
            this.panel1.TabIndex = 27;
            // 
            // saveNodeButton
            // 
            this.saveNodeButton.Location = new System.Drawing.Point(4, 3);
            this.saveNodeButton.Name = "saveNodeButton";
            this.saveNodeButton.Size = new System.Drawing.Size(116, 26);
            this.saveNodeButton.TabIndex = 32;
            this.saveNodeButton.Text = "Применить";
            this.saveNodeButton.UseVisualStyleBackColor = true;
            this.saveNodeButton.Click += new System.EventHandler(this.saveNodeButton_Click);
            // 
            // reloadButton
            // 
            this.reloadButton.Location = new System.Drawing.Point(4, 29);
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(116, 25);
            this.reloadButton.TabIndex = 31;
            this.reloadButton.Text = "Перезагрузить";
            this.reloadButton.UseVisualStyleBackColor = true;
            this.reloadButton.Click += new System.EventHandler(this.reloadButton_Click_1);
            // 
            // base_class
            // 
            this.base_class.FormattingEnabled = true;
            this.base_class.Location = new System.Drawing.Point(241, 29);
            this.base_class.Name = "base_class";
            this.base_class.Size = new System.Drawing.Size(539, 24);
            this.base_class.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(128, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 17);
            this.label2.TabIndex = 29;
            this.label2.Text = "Базовый класс:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(128, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 28;
            this.label1.Text = "Имя узла:";
            // 
            // node_name
            // 
            this.node_name.Location = new System.Drawing.Point(241, 3);
            this.node_name.Name = "node_name";
            this.node_name.Size = new System.Drawing.Size(539, 22);
            this.node_name.TabIndex = 27;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.mainToolStrip);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 26);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1088, 135);
            this.panel5.TabIndex = 23;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.filterItems);
            this.panel6.Controls.Add(this.filterCategories);
            this.panel6.Controls.Add(this.panelFilterButtons);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 27);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1088, 108);
            this.panel6.TabIndex = 20;
            // 
            // filterItems
            // 
            this.filterItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.filterItems.CheckBoxes = true;
            this.filterItems.ContextMenuStrip = this.filtersPanelContextMenu;
            this.filterItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterItems.Location = new System.Drawing.Point(322, 0);
            this.filterItems.MultiSelect = false;
            this.filterItems.Name = "filterItems";
            this.filterItems.Size = new System.Drawing.Size(766, 108);
            this.filterItems.TabIndex = 5;
            this.filterItems.UseCompatibleStateImageBehavior = false;
            this.filterItems.View = System.Windows.Forms.View.List;
            this.filterItems.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.filterItems_ItemChecked);
            // 
            // filtersPanelContextMenu
            // 
            this.filtersPanelContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.filtersPanelContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editInTagsCollectionToolStripMenuItem});
            this.filtersPanelContextMenu.Name = "filtersPanelContextMenu";
            this.filtersPanelContextMenu.Size = new System.Drawing.Size(198, 26);
            // 
            // editInTagsCollectionToolStripMenuItem
            // 
            this.editInTagsCollectionToolStripMenuItem.Name = "editInTagsCollectionToolStripMenuItem";
            this.editInTagsCollectionToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.editInTagsCollectionToolStripMenuItem.Text = "Редактор тегов...";
            this.editInTagsCollectionToolStripMenuItem.Click += new System.EventHandler(this.tagsCollectionToolStripMenuItem_Click);
            // 
            // filterCategories
            // 
            this.filterCategories.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.filterCategories.ContextMenuStrip = this.filtersPanelContextMenu;
            this.filterCategories.Dock = System.Windows.Forms.DockStyle.Left;
            this.filterCategories.IntegralHeight = false;
            this.filterCategories.ItemHeight = 16;
            this.filterCategories.Location = new System.Drawing.Point(140, 0);
            this.filterCategories.Name = "filterCategories";
            this.filterCategories.Size = new System.Drawing.Size(182, 108);
            this.filterCategories.TabIndex = 6;
            this.filterCategories.SelectedIndexChanged += new System.EventHandler(this.filterCategories_SelectedIndexChanged);
            // 
            // panelFilterButtons
            // 
            this.panelFilterButtons.Controls.Add(this.filterSelected);
            this.panelFilterButtons.Controls.Add(this.filterAncestors);
            this.panelFilterButtons.Controls.Add(this.resetFilterButton);
            this.panelFilterButtons.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelFilterButtons.Location = new System.Drawing.Point(0, 0);
            this.panelFilterButtons.Name = "panelFilterButtons";
            this.panelFilterButtons.Size = new System.Drawing.Size(140, 108);
            this.panelFilterButtons.TabIndex = 3;
            // 
            // filterSelected
            // 
            this.filterSelected.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.filterSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterSelected.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.filterSelected.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.filterSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.filterSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.filterSelected.Location = new System.Drawing.Point(0, 29);
            this.filterSelected.Name = "filterSelected";
            this.filterSelected.Size = new System.Drawing.Size(140, 50);
            this.filterSelected.TabIndex = 1;
            this.filterSelected.Text = "Только выделенные";
            this.filterSelected.UseVisualStyleBackColor = true;
            this.filterSelected.Click += new System.EventHandler(this.customFilterMode_CheckedChanged);
            // 
            // filterAncestors
            // 
            this.filterAncestors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.filterAncestors.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.filterAncestors.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.filterAncestors.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.filterAncestors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.filterAncestors.Location = new System.Drawing.Point(0, 79);
            this.filterAncestors.Name = "filterAncestors";
            this.filterAncestors.Size = new System.Drawing.Size(140, 29);
            this.filterAncestors.TabIndex = 2;
            this.filterAncestors.Text = "Только предки";
            this.filterAncestors.UseVisualStyleBackColor = true;
            this.filterAncestors.Click += new System.EventHandler(this.filterAncestors_Click);
            // 
            // resetFilterButton
            // 
            this.resetFilterButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.resetFilterButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.resetFilterButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.resetFilterButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.resetFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetFilterButton.Location = new System.Drawing.Point(0, 0);
            this.resetFilterButton.Name = "resetFilterButton";
            this.resetFilterButton.Size = new System.Drawing.Size(140, 29);
            this.resetFilterButton.TabIndex = 0;
            this.resetFilterButton.Text = "Очистить фильтр";
            this.resetFilterButton.UseVisualStyleBackColor = true;
            this.resetFilterButton.Click += new System.EventHandler(this.resetFilterButton_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveAndStoreToolStripMenuItem,
            this.toolStripMenuItem1,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 22);
            this.fileToolStripMenuItem.Text = "Файл";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(273, 26);
            this.newToolStripMenuItem.Text = "Новый";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(273, 26);
            this.openToolStripMenuItem.Text = "Открыть...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.open_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(273, 26);
            this.saveToolStripMenuItem.Text = "Сохранить";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.save_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveAsToolStripMenuItem.Image")));
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(273, 26);
            this.saveAsToolStripMenuItem.Text = "Сохранить как...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.save_as_Click);
            // 
            // saveAndStoreToolStripMenuItem
            // 
            this.saveAndStoreToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveAndStoreToolStripMenuItem.Image")));
            this.saveAndStoreToolStripMenuItem.Name = "saveAndStoreToolStripMenuItem";
            this.saveAndStoreToolStripMenuItem.Size = new System.Drawing.Size(273, 26);
            this.saveAndStoreToolStripMenuItem.Text = "Сохранить и сделать копию";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(270, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(273, 26);
            this.quitToolStripMenuItem.Text = "Выйти";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // nodesToolStripMenuItem
            // 
            this.nodesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNodeToolStripMenuItem,
            this.toolStripMenuItem2,
            this.moveNodeUpToolStripMenuItem,
            this.moveNodeDownToolStripMenuItem,
            this.toolStripMenuItem3,
            this.deleteNodeToolStripMenuItem,
            this.toolStripMenuItem4,
            this.sortToolStripMenuItem,
            this.присвоитьТегToolStripMenuItem,
            this.globalRenamerToolStripMenuItem});
            this.nodesToolStripMenuItem.Name = "nodesToolStripMenuItem";
            this.nodesToolStripMenuItem.Size = new System.Drawing.Size(54, 22);
            this.nodesToolStripMenuItem.Text = "Узлы";
            // 
            // addNodeToolStripMenuItem
            // 
            this.addNodeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addNodeToolStripMenuItem.Image")));
            this.addNodeToolStripMenuItem.Name = "addNodeToolStripMenuItem";
            this.addNodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.addNodeToolStripMenuItem.Size = new System.Drawing.Size(279, 26);
            this.addNodeToolStripMenuItem.Text = "Добавить узел";
            this.addNodeToolStripMenuItem.Click += new System.EventHandler(this.add_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(276, 6);
            // 
            // moveNodeUpToolStripMenuItem
            // 
            this.moveNodeUpToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("moveNodeUpToolStripMenuItem.Image")));
            this.moveNodeUpToolStripMenuItem.Name = "moveNodeUpToolStripMenuItem";
            this.moveNodeUpToolStripMenuItem.Size = new System.Drawing.Size(279, 26);
            this.moveNodeUpToolStripMenuItem.Text = "Передвинуть узел вверх";
            this.moveNodeUpToolStripMenuItem.Click += new System.EventHandler(this.Up_Click);
            // 
            // moveNodeDownToolStripMenuItem
            // 
            this.moveNodeDownToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("moveNodeDownToolStripMenuItem.Image")));
            this.moveNodeDownToolStripMenuItem.Name = "moveNodeDownToolStripMenuItem";
            this.moveNodeDownToolStripMenuItem.Size = new System.Drawing.Size(279, 26);
            this.moveNodeDownToolStripMenuItem.Text = "Передвинуть узел вниз";
            this.moveNodeDownToolStripMenuItem.Click += new System.EventHandler(this.Down_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(276, 6);
            // 
            // deleteNodeToolStripMenuItem
            // 
            this.deleteNodeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteNodeToolStripMenuItem.Image")));
            this.deleteNodeToolStripMenuItem.Name = "deleteNodeToolStripMenuItem";
            this.deleteNodeToolStripMenuItem.Size = new System.Drawing.Size(279, 26);
            this.deleteNodeToolStripMenuItem.Text = "Удалить узел";
            this.deleteNodeToolStripMenuItem.Click += new System.EventHandler(this.delete_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(276, 6);
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("sortToolStripMenuItem.Image")));
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(279, 26);
            this.sortToolStripMenuItem.Text = "Отсортировать узлы";
            this.sortToolStripMenuItem.Click += new System.EventHandler(this.sortToolStripMenuItem_Click);
            // 
            // присвоитьТегToolStripMenuItem
            // 
            this.присвоитьТегToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("присвоитьТегToolStripMenuItem.Image")));
            this.присвоитьТегToolStripMenuItem.Name = "присвоитьТегToolStripMenuItem";
            this.присвоитьТегToolStripMenuItem.Size = new System.Drawing.Size(279, 26);
            this.присвоитьТегToolStripMenuItem.Text = "Присвоить тег";
            this.присвоитьТегToolStripMenuItem.Click += new System.EventHandler(this.saveTempFilter_Click);
            // 
            // globalRenamerToolStripMenuItem
            // 
            this.globalRenamerToolStripMenuItem.Name = "globalRenamerToolStripMenuItem";
            this.globalRenamerToolStripMenuItem.Size = new System.Drawing.Size(279, 26);
            this.globalRenamerToolStripMenuItem.Text = "Глобальное переименование";
            this.globalRenamerToolStripMenuItem.Click += new System.EventHandler(this.globalRenamerToolStripMenuItem_Click);
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripMenuItem5,
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(69, 22);
            this.contentsToolStripMenuItem.Text = "Правка";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.cutToolStripMenuItem.Text = "Вырезать";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonCut_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.copyToolStripMenuItem.Text = "Копировать";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonCopy_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.pasteToolStripMenuItem.Text = "Вставить";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonPaste_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(221, 6);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("undoToolStripMenuItem.Image")));
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.undoToolStripMenuItem.Text = "Отменить";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonUndo_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("redoToolStripMenuItem.Image")));
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Z)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.redoToolStripMenuItem.Text = "Вернуть";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonRedo_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filterPanelToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(45, 22);
            this.viewToolStripMenuItem.Text = "Вид";
            // 
            // filterPanelToolStripMenuItem
            // 
            this.filterPanelToolStripMenuItem.Checked = true;
            this.filterPanelToolStripMenuItem.CheckOnClick = true;
            this.filterPanelToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.filterPanelToolStripMenuItem.Name = "filterPanelToolStripMenuItem";
            this.filterPanelToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
            this.filterPanelToolStripMenuItem.Text = "Панель фильтра";
            this.filterPanelToolStripMenuItem.CheckedChanged += new System.EventHandler(this.filterPanelToolStripMenuItem_CheckedChanged);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem1,
            this.tagsCollectionToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.optionsToolStripMenuItem.Text = "Настройки";
            // 
            // optionsToolStripMenuItem1
            // 
            this.optionsToolStripMenuItem1.Name = "optionsToolStripMenuItem1";
            this.optionsToolStripMenuItem1.Size = new System.Drawing.Size(197, 22);
            this.optionsToolStripMenuItem1.Text = "Настройки...";
            this.optionsToolStripMenuItem1.Click += new System.EventHandler(this.toolStripOptionsButton_Click);
            // 
            // tagsCollectionToolStripMenuItem
            // 
            this.tagsCollectionToolStripMenuItem.Name = "tagsCollectionToolStripMenuItem";
            this.tagsCollectionToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.tagsCollectionToolStripMenuItem.Text = "Редактор тегов...";
            this.tagsCollectionToolStripMenuItem.Click += new System.EventHandler(this.tagsCollectionToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateToolStripMenuItem,
            this.grammarBrowserToolStripMenuItem});
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(81, 22);
            this.runToolStripMenuItem.Text = "Команды";
            // 
            // generateToolStripMenuItem
            // 
            this.generateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("generateToolStripMenuItem.Image")));
            this.generateToolStripMenuItem.Name = "generateToolStripMenuItem";
            this.generateToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.generateToolStripMenuItem.Size = new System.Drawing.Size(205, 26);
            this.generateToolStripMenuItem.Text = "Генерировать";
            this.generateToolStripMenuItem.Click += new System.EventHandler(this.toolStripGenerateButton_Click);
            // 
            // grammarBrowserToolStripMenuItem
            // 
            this.grammarBrowserToolStripMenuItem.Name = "grammarBrowserToolStripMenuItem";
            this.grammarBrowserToolStripMenuItem.Size = new System.Drawing.Size(205, 26);
            this.grammarBrowserToolStripMenuItem.Text = "Иерархия узлов...";
            // 
            // mainMenu
            // 
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.contentsToolStripMenuItem,
            this.nodesToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.runToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(1088, 26);
            this.mainMenu.TabIndex = 22;
            this.mainMenu.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(76, 22);
            this.helpToolStripMenuItem.Text = "Справка";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.aboutToolStripMenuItem.Text = "О программе...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter3.Location = new System.Drawing.Point(0, 161);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(1088, 3);
            this.splitter3.TabIndex = 31;
            this.splitter3.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(1088, 724);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.splitter3);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.NavigationPanel);
            this.Controls.Add(this.mainMenu);
            this.KeyPreview = true;
            this.MainMenuStrip = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(1080, 692);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SyNerator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_ShortCutsHandler);
            this.NavigationPanel.ResumeLayout(false);
            this.NavigationPanel.PerformLayout();
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.nodesContextMenu.ResumeLayout(false);
            this.nodesToolStrip.ResumeLayout(false);
            this.nodesToolStrip.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).EndInit();
            this.edtiorToolStrip.ResumeLayout(false);
            this.edtiorToolStrip.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.filtersPanelContextMenu.ResumeLayout(false);
            this.panelFilterButtons.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new Form1());
        }

        internal NodeGenerator.NodeGenerator node_gen = new NodeGenerator.NodeGenerator();

        private void reload_nodes()
        {
            nodes_list.Items.Clear();
            nodes_list.Items.AddRange(node_gen.all_nodes.ToArray());
        }

        private void set_nodes_information() // set - установить поля интерфейса по node_gen
        {
            if (node_gen == null)
            {
                node_gen = new NodeGenerator.NodeGenerator();
            }

            reload_nodes();

            fillInTagCategories();

            creation_mode = false;
            selected_tag_cat = -1;
            selected_node = null;
            selected_node_index = -1;
            selected_filter_cat = -1;
            is_filter_enabled = false;

            if (nodes_list.Items.Count > 0) { new_selected_node = (nodes_list.Items[0] as node_info); nodes_list.SelectedIndex = 0; }
            else
            {
                node_name.Text = "";
                base_class.Items.Clear();
                base_class.Text = "";
                scintilla1.Text = "";
                help_context.Text = "";
            }
        }

        private void get_nodes_information() // get - получить информацию из полей интерфейса и записать ее в node_gen
        {
            if (node_gen == null)
            {
                node_gen = new NodeGenerator.NodeGenerator();
            }


            node_gen.set_nodes(nodes_list.Items);
        }

        private void loadFile(string fname)
        {
            if (!File.Exists(fname))
            {
                MessageBox.Show("The file '" + fname + "' does NOT exist");
                return;
            }
            node_gen = NodeGenerator.NodeGenerator.deserialize(fname);
            if (node_gen == null)
            {
                MessageBox.Show("Can not read nodes information from: " + fname);
            }
            else
            {
                set_nodes_information();
                this.Text = Path.GetFileNameWithoutExtension(fname) + " - " + default_caption;
            }
        }

        private void open_Click(object sender, System.EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (CloseCurrentFile() == CloseResult.NotClosed) return;
                saveFileDialog1.FileName = openFileDialog1.FileName;
                loadFile(openFileDialog1.FileName);
            }
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            createParser();
            default_caption = this.Text;
            loadConfig();

            if (File.Exists(saveFileDialog1.FileName))
                loadFile(saveFileDialog1.FileName);
        }

        private void save_Click(object sender, System.EventArgs e)
        {
            saveCurrentFile();
        }

        private void save_as_Click(object sender, System.EventArgs e)
        {
            saveAsCurrentFile();
        }

        private void delete_selected_items()
        {
            nodes_list.SelectedIndexChanged -= nodes_list_SelectedIndexChanged;
            var x = nodes_list.SelectedIndices.Cast<int>().ToList(); //already sorted......            
            for (int i = 0; i < x.Count; i++)
            {
                node_gen.all_nodes.Remove(nodes_list.Items[x[i] - i]);
                nodes_list.Items.RemoveAt(x[i] - i);                
            }
            nodes_list.SelectedIndexChanged += nodes_list_SelectedIndexChanged;
        }

        private void add_Click(object sender, System.EventArgs e)
        {
            synchronize_node();
            creation_mode = true;
            selected_node = new node_info();
            set_node_info(selected_node);
            node_name.Focus();
        }

        private void delete_Click(object sender, System.EventArgs e)
        {
            string messageBoxText = "Are you sure, you want to delete the selected node?";
            string caption = "Delete the nodes?";
            MessageBoxButtons button = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Warning;
            MessageBoxDefaultButton def_btn = MessageBoxDefaultButton.Button2;

            if (MessageBox.Show(messageBoxText, caption, button, icon, def_btn) == DialogResult.Yes)
            {                
                int ind = selected_node_index;
                new_selected_node = selected_node = null;
                selected_node_index = -1;
                delete_selected_items();                
                nodes_list.SelectedIndex = Math.Min(nodes_list.Items.Count - 1, ind);

                // Голову отрывать надо за такой небрежный код
                selected_node = nodes_list.SelectedItem as node_info;
                selected_node_index = nodes_list.SelectedIndex;
            }
            //update();
        }

        public void SortNodes()
        {
            string txt = null;
            object item = null;
            if (nodes_list.SelectedIndex >= 0)
                item = selected_node;
            if (item != null)
                txt = item.ToString();

            node_gen.SortNodes();
            reload_nodes();

            if (item != null)
            {
                int ind = nodes_list.FindString(txt);
                nodes_list.SelectedIndex = ind;
            }
        }

        public void UnsortNodes()
        {
            string txt = null;
            object item = null;
            if (nodes_list.SelectedIndex >= 0)
                item = selected_node;
            if (item != null)
                txt = item.ToString();

            node_gen.UnsortNodes();
            reload_nodes();

            if (item != null)
            {
                int ind = nodes_list.FindString(txt);
                nodes_list.SelectedIndex = ind;
            }
        }

        public System.Windows.Forms.DialogResult AskForSave()
        {
            return MessageBox.Show("Do you want to save the currently opened file?", "Save changes?",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }

        //Sub-routine for the next one
        private bool remove_last_nav_label_if_hidden()
        {
            if (NavigationPanel.Items[NavigationPanel.Items.Count - 1].Bounds.Right > NavigationPanel.Width)
            {
                NavigationPanel.Items.RemoveAt(NavigationPanel.Items.Count - 1);
                return true;
            }
            return false;
        }

        private void fill_in_navigation_panel(node_info ni)
        {
            if (ni == null)
                return;

            var DropDownNodes = (NavigationPanel.Items[0] as ToolStripDropDownButton).DropDownItems;

            DropDownNodes.Clear();            
            while (NavigationPanel.Items.Count > 1)
                NavigationPanel.Items.RemoveAt(1);

            var l = new List<string>();
            var s = ni.node_name;
            bool add_dots = false;

            while (true && l.Count < 40) // 40 to prevent perpetum cycle, if recursive base classes
            {
                if (!add_dots)
                {
                    var nav_lbl = new ToolStripStatusLabel(s, null, ItemsClick);
                    nav_lbl.IsLink = true;
                    NavigationPanel.Items.Add(nav_lbl);
                    add_dots = remove_last_nav_label_if_hidden();
                    if (!add_dots)
                    {
                        NavigationPanel.Items.Add(new ToolStripStatusLabel("->"));
                        add_dots = remove_last_nav_label_if_hidden();
                    }
                }
                l.Add(ni.node_name);

                if (ni.base_class == null) break;

                ni = ni.base_class;
                s = ni.node_name;
            }
            if (NavigationPanel.Items.Count > 1)
                NavigationPanel.Items.RemoveAt(NavigationPanel.Items.Count - 1);
            if (add_dots)
                NavigationPanel.Items.Add(new ToolStripStatusLabel("..."));

            for (int i = l.Count - 1; i >= 0; i--)
            {
                DropDownNodes.Add(l[i]);
                DropDownNodes[DropDownNodes.Count - 1].Click += ItemsClick;
            }
        }

        private bool synchronize_node()
        {
            if (selected_node != null && !creation_mode)
            {
                if (!get_node_info(selected_node)) return false;

                //clumsy way, caused by the fact, we need to stay compatible with the old thing
                //haven't made a derived class, 'cause the method is invoked only once
                typeof(ListBox).InvokeMember("RefreshItem",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod,
                    null, nodes_list, new object[] { selected_node_index });
            }
            return true;
        }

        private void nodes_list_SelectedIndexChanged(object sender, EventArgs e)
        {                        
            if (nodes_list_SelectedIndexChanged_recur) return;
            if (nodes_list.SelectedItem == null) return;

            node_info item = null;
            if (new_selected_node != null)
            {
                item = new_selected_node;
                new_selected_node = null;
            }
            else item = (nodes_list.SelectedItem as node_info);

            nodes_list_SelectedIndexChanged_recur = true;

            //reload the item, no other actions required, if the same node was selected
            if (item == selected_node)
            {
                set_node_info(item);
            }
            else
            {
                //creation of a new node case
                if (creation_mode)
                {
                    if (node_name.Text != "")
                    {
                        if (!get_node_info(selected_node))
                        {
                            nodes_list_SelectedIndexChanged_recur = false;
                            return;
                        }
                        nodes_list.Items.Add(selected_node);
                        node_gen.add_node(selected_node);
                    }
                    creation_mode = false;
                }
                else
                    //default switch between nodes            
                    if (!synchronize_node())
                    {
                        nodes_list.SelectedItems.Remove(item);
                        nodes_list.SelectedItem = selected_node;                        
                        nodes_list_SelectedIndexChanged_recur = false;
                        return;
                    }

                fill_in_navigation_panel(item);
                set_node_info(item);
                selected_node = item;
                selected_node_index = nodes_list.Items.IndexOf(item);
            }

            nodes_list_SelectedIndexChanged_recur = false;
        }

        private void ItemsClick(object sender, EventArgs e)
        {
            string text;
            if (sender is ToolStripStatusLabel)
                text = (sender as ToolStripStatusLabel).Text;
            else text = (sender as ToolStripMenuItem).Text;
            int ind = nodes_list.FindString(text);
            if (ind >= 0)
                nodes_list.SelectedIndex = ind;            
        }

        private void Up_Click(object sender, EventArgs e)
        {
            if (selected_node_index < 0)
                return;

            nodes_list.SelectedIndexChanged -= nodes_list_SelectedIndexChanged;

            nodes_list.Items.RemoveAt(selected_node_index);
            nodes_list.Items.Insert(selected_node_index - 1, selected_node);
            if (!is_filter_enabled)
                node_gen.move_node(selected_node_index, selected_node_index - 1);
            selected_node_index--;
            nodes_list.SelectedIndices.Add(selected_node_index);

            nodes_list.SelectedIndexChanged += nodes_list_SelectedIndexChanged;
        }

        private void Down_Click(object sender, EventArgs e)
        {
            if (selected_node_index < 0)
                return;

            nodes_list.SelectedIndexChanged -= nodes_list_SelectedIndexChanged;

            nodes_list.Items.RemoveAt(selected_node_index);
            nodes_list.Items.Insert(selected_node_index + 1, selected_node);
            if (!is_filter_enabled)
                node_gen.move_node(selected_node_index, selected_node_index + 1);
            selected_node_index++;
            nodes_list.SelectedIndices.Add(selected_node_index);

            nodes_list.SelectedIndexChanged += nodes_list_SelectedIndexChanged;
        }

        private void loadConfig()
        {
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(Directory.GetCurrentDirectory() + @"\config.cfg");

                saveFileDialog1.FileName = file.ReadLine();
                auto_save = file.ReadLine() == "1";
                version_storage = file.ReadLine();
                if (version_storage == null) version_storage = "Storage";

                file.Close();

                if (File.Exists("ac_list.cfg"))
                    autocompletion_list.AddRange(File.ReadAllLines("ac_list.cfg"));
            }
            catch (Exception e)
            {
            }
        }

        private void saveConfig()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Directory.GetCurrentDirectory() + @"\config.cfg", false))
            {
                file.WriteLine(saveFileDialog1.FileName);
                file.WriteLine(auto_save ? "1" : "0");
                file.WriteLine(version_storage);
            }

            File.WriteAllLines("ac_list.cfg", autocompletion_list);
        }

        private void saveCurrentFile()
        {
            if (!synchronize_node()) return;
            toolStripButtonSort.Checked = false;
            //update();
            if ((saveFileDialog1.FileName != null) && (saveFileDialog1.FileName != ""))
            {
                node_gen.serialize(saveFileDialog1.FileName);
                fill_in_filter_items();
            }
            else
            {
                saveAsCurrentFile();
            }
        }

        private void saveAsCurrentFile()
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                saveCurrentFile();
            }
        }

        private CloseResult CloseCurrentFile()
        {
            if (!synchronize_node())
                return CloseResult.NotClosed;
            if (auto_save && (saveFileDialog1.FileName != null) && (saveFileDialog1.FileName != ""))
            {
                saveCurrentFile();
                return CloseResult.Saved;
            }
            else
                switch (AskForSave())
                {
                    case System.Windows.Forms.DialogResult.Yes: saveCurrentFile(); return CloseResult.Saved;
                    case System.Windows.Forms.DialogResult.Cancel: return CloseResult.NotClosed;
                }
            return CloseResult.NotSaved;
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseCurrentFile() == CloseResult.NotClosed)
            {
                e.Cancel = true;
                return;
            }            

            saveConfig();
        }

        private void toolStripGenerateButton_Click(object sender, EventArgs e)
        {
            toolStripGenerateButton.Enabled = false;
            mainToolStrip.Items.Add("Generating, please, wait...");
            mainToolStrip.Refresh();


            bool state = toolStripButtonSort.Checked;
            toolStripButtonSort.Checked = false;

            //get_nodes_information();
            node_gen.generate_code();
            node_gen.generate_pcu_writer_code(false);
            node_gen.generate_pcu_reader_code();            

            toolStripButtonSort.Checked = state;


            mainToolStrip.Items.RemoveAt(mainToolStrip.Items.Count - 1);
            toolStripGenerateButton.Enabled = true;
        }

        private void Form1_ShortCutsHandler(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F9)
            //   toolStripGenerateButton_Click(sender, e);
            /*  if (e.Control)
              {
                  if (e.KeyCode == Keys.O)
                      open_Click(sender, e);
                  if (e.KeyCode == Keys.S)
                      if (e.Alt)
                          save_as_Click(sender, e);
                      else save_Click(sender, e);
              }/*/
        }

        private void toolStripOptionsButton_Click(object sender, EventArgs e)
        {
            var dlg = new options_form();
            dlg.set_options_infromation(node_gen, auto_save, version_storage, autocompletion_list);
            if (dlg.ShowDialog() == DialogResult.OK)
                dlg.get_options_infromation(node_gen, out auto_save, out version_storage, autocompletion_list);
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            set_node_info(selected_node);
        }

        //***********node_def replacement - editing node contents
        private void createParser()
        {
            _grammar = new CSharpGrammar();
            _language = new LanguageData(_grammar);
            _parser = new Parser(_language);
        }



        private void set_node_info_help_context(string help_context_name)
        {
            var help_context = node_gen.help_storage.get_help_context(help_context_name).help_context;
            int p = 0;
            while (p < help_context.Length)
            {
                scintilla1.Text += "//" + help_context.Substring(p, Math.Min(max_str_len, help_context.Length - p)) + "\n";
                p += max_str_len;
            }
        }

        private void set_node_info_tags(node_info ni)
        {
            if (ni == null) return;

            tagItems.Clear();
            temp_tags.Clear();
            foreach (var tg in ni.tags)
            {
                temp_tags.Add(new Tuple<int, string>(tg.Item1, node_gen.tag_cats[tg.Item1].tags[tg.Item2].name));
            }
            tagCategories.SelectedIndexChanged -= tagCategories_SelectedIndexChanged;
            if (tagCategories.Items.Count > 0)
            {
                if (selected_tag_cat == -1)
                    selected_tag_cat = tagCategories.SelectedIndex = 0;
                else
                    tagCategories.SelectedIndex = selected_tag_cat;
                fillInTagItems(selected_tag_cat);
            }
            tagCategories.SelectedIndexChanged += tagCategories_SelectedIndexChanged;
            update_tags = false;
        }

        private void set_node_info(node_info ni)
        {
            scintilla1.AutoComplete.List.Clear();
            for (int i = 0; i < nodes_list.Items.Count; i++)
                scintilla1.AutoComplete.List.Add(nodes_list.Items[i].ToString());
            scintilla1.AutoComplete.List.AddRange(autocompletion_list);
            scintilla1.AutoComplete.List.Sort();

            this.node_name.Text = ni.node_name;

            //setting base class
            base_class.Items.Clear();
            base_class.Items.AddRange((object[])node_gen.all_nodes.ToArray(typeof(object)));

            if (ni.base_class == null)
            {
                base_class.SelectedIndex = -1;
                base_class.Text = "";
            }
            else base_class.SelectedIndex = Math.Max(-1, base_class.Items.IndexOf(ni.base_class));
            
            //fields
            scintilla1.Text = "";
            for (int i = 0; i < ni.subnodes.Length; i++)
            {
                set_node_info_help_context(ni.node_name + '.' + ni.subnodes[i].field_name);

                scintilla1.Text += ni.subnodes[i];

                if (ni.subnodes[i] is extended_simple_element && (ni.subnodes[i] as extended_simple_element).create_var)
                    scintilla1.Text += " = new " + (ni.subnodes[i] as extended_simple_element).field_type_name + "();";

                if (scintilla1.Text[scintilla1.Text.Length - 1] != ';')
                    scintilla1.Text += ';';

                scintilla1.Text += "\n";
            }
            scintilla1.Text += "\n";
            
            //methods
            for (int i = 0; i < ni.methods.Length; i++)
            {
                set_node_info_help_context(ni.node_name + '.' + ni.methods[i].method_header);
                scintilla1.Text += ni.methods[i].method_text + "\n\n";
            }

            scintilla1.UndoRedo.EmptyUndoBuffer();

            //Help context
            this.help_context.Text = "";

            if (ni.node_name != null)
            {
                if (node_gen.help_storage.get_help_context(ni.node_name) == null)
                {
                    node_gen.help_storage.add_context(ni.node_name, new HelpContext());
                }
                this.help_context.Text = node_gen.help_storage.get_help_context(ni.node_name).help_context;
            }

            //tags
            set_node_info_tags(ni);
        }

        //parsing node edits and saving

        private ParseTreeNode get_node_info_get_class_body(ParseTree _parseTree)
        {
            return _parseTree.Root.ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[6].ChildNodes[0];
        }

        private string get_node_info_get_subnode_type(ParseTreeNode node, string src)
        {
            return src.Substring(node.Span.Location.Position,
                        node.ChildNodes[2].ChildNodes[0].ChildNodes[0].Span.Location.Position - node.Span.Location.Position).TrimEnd();

            /*            if (node.ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes.Count == 1)
                            return node.ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Text;
                        return node.ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Text;*/
        }

        private string get_node_info_get_subnode_name(ParseTreeNode node)
        {
            return node.ChildNodes[2].ChildNodes[0].ChildNodes[0].Token.Text;
        }

        private bool get_node_info_has_constructor(ParseTreeNode node)
        {
            return node.ChildNodes[2].ChildNodes[0].ChildNodes.Count > 1;
        }

        //root - class body with fields and methods; node - class itself; src - whole source (with namespace)
        private void get_node_info_get_subnodes_and_methods(ParseTreeNode root, node_info node, string src)
        {
            List<node_field_info> subnodes = new List<node_field_info>();
            List<method_info> methods = new List<method_info>();

            var lines = src.Split('\n');
            var nodes = new List<node_info>();
            nodes.AddRange(node_gen.all_nodes.Cast<node_info>());

            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                HelpContext help_context = null;
                if (root.ChildNodes[i].Term.Name == "field_declaration")
                {
                    var subnode_type = get_node_info_get_subnode_type(root.ChildNodes[i], src);
                    var subnode = nodes.Find(x => x.node_name == subnode_type);

                    if (subnode == null)
                        subnodes.Add(new extended_simple_element()
                        {
                            val_field_type_name = subnode_type,
                            field_name = get_node_info_get_subnode_name(root.ChildNodes[i]),
                            create_var = get_node_info_has_constructor(root.ChildNodes[i]) // lines[root.ChildNodes[i].Span.Location.Line].Contains("new List")
                        });
                    else
                        subnodes.Add(new node_field_info()
                        {
                            field_name = get_node_info_get_subnode_name(root.ChildNodes[i]),
                            field_type = subnode
                        });

                    help_context = node_gen.help_storage.get_help_context(node.node_name + "." + subnodes[subnodes.Count - 1].field_name);
                }
                else
                    if (root.ChildNodes[i].Term.Name == "constructor_declaration" || root.ChildNodes[i].Term.Name == "method_declaration")
                    {
                        methods.Add(new method_info(src.Substring(root.ChildNodes[i].Span.Location.Position, root.ChildNodes[i].Span.Length)));
                        help_context = node_gen.help_storage.get_help_context(node.node_name + "." + methods[methods.Count - 1].method_header);
                    }
                else continue;

                //help context
                help_context.help_context = "";
                int help_cur_line = root.ChildNodes[i].Span.Location.Line - 1;
                while (lines[help_cur_line].Length > 2 && lines[help_cur_line].Substring(0, 2) == "//")
                {
                    help_context.help_context = lines[help_cur_line].Substring(2, lines[help_cur_line].Length - 2).Trim() + help_context.help_context;
                    help_cur_line--;
                }
            }

            node.set_subnodes(subnodes);
            node.set_methods(methods);
        }

        private bool cycles_in_deriving(node_info ni, node_info new_base_class)
        {
            if (new_base_class == null) return false;

            HashSet<node_info> chain = new HashSet<node_info>();

            chain.Add(ni);
            while (new_base_class.base_class != null)
            {
                if (chain.Contains(new_base_class))
                    return true;
                chain.Add(new_base_class);
                new_base_class = new_base_class.base_class;
            }

            return false;
        }

        private bool get_node_info(node_info ni)
        {
            ////pre-checking
            //parsing
            //brackets are needed, so the sample grammar works fine without in-grammar changes
            string src = parser_tech_data + scintilla1.Text + "}}";

            try
            {
                _parser.Parse(src, "<source>");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

            ParseTree _parseTree = _parser.Context.CurrentParseTree;
            if (_parseTree.ParserMessages.Count > 0)
            {
                int show_mes_id = 0;
                while (_parseTree.ParserMessages.Count > show_mes_id) //"S109" "S315"
                    if (parser_errors_ignore_states.Contains(_parseTree.ParserMessages[show_mes_id].ParserState.Name))
                        show_mes_id++;
                    else break;

                if (show_mes_id < _parseTree.ParserMessages.Count)
                {

                    MessageBox.Show("Some Syntax Error @ Line " + _parseTree.ParserMessages[show_mes_id].Location.Line.ToString(),
                                    _parseTree.ParserMessages[show_mes_id].Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    scintilla1.Caret.LineNumber = _parseTree.ParserMessages[show_mes_id].Location.Line - 1;

                    scintilla1.Focus();
                    return false;
                }
            }

            //checking, if there are cycles in the deriving chain
            if (cycles_in_deriving(ni, base_class.SelectedItem as node_info))
            {
                MessageBox.Show("Base class is incorrect for this node, as this leads to cycles in the deriving chain!",
                                "Base class is incorrect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            ////setters - work only if the first part is OK!!
            var root = get_node_info_get_class_body(_parseTree);
            get_node_info_get_subnodes_and_methods(root, ni, src);

            //node name & base class
            ni.node_name = node_name.Text;
            int t = base_class.SelectedIndex;
            if (t >= 0)
                ni.base_class = base_class.Items[t] as node_info;

            //help context
            node_gen.help_storage.get_help_context(ni.node_name).help_context = this.help_context.Text;

            //tags
            if (update_tags)
            {
                getTagItems(tagCategories.SelectedIndex);
                update_tags = true;
            }
            node_gen.reduce_ref_counts(ni);
            node_gen.set_tags_for_node(temp_tags, ni);
            if (update_tags)
            {
                fillInTagCategories(true);
                fill_in_filter_items();
                applyCurrentFilterForNode(selected_node);
                update_tags = false;
            }

            return true;
        }

        //end of parse of node edits

        /*        private void AddAllForList_Click(object sender, EventArgs e)
                {
                    if (subnodes.Items.Count == 0)
                        return;
                    var ListItemType = subnodes.Items[0].ToString();
                    var ListName = "";
                    var NodeType = node_name.Text;
                    if (ListItemType.ToLower().StartsWith("list"))
                    {
                        var i1 = ListItemType.IndexOf("<");
                        var i2 = ListItemType.IndexOf(">");
                        ListName = ListItemType.Substring(i2 + 2);
                        ListItemType = ListItemType.Substring(i1 + 1, i2 - i1 - 1);
                    }
                    else return;
                    //            nm.method_header = "void Add(SourceContext sc)";
                    string templateListConstructor = "{0}({1} _{1}, SourceContext sc)\r\n{{\r\n    Add(_{1},sc);\r\n}}";
                    string templateListAddNoSC = "{0} Add({1} _{1})\r\n{{\r\n    {2}.Add(_{1});\r\n    return this;\r\n}}";
                    string templateListAdd = "{0} Add({1} _{1}, SourceContext sc)\r\n{{\r\n    {2}.Add(_{1});\r\n    source_context = sc;\r\n    return this;\r\n}}";
                    method_info nm = new method_info();
                    nm.method_text = string.Format(templateListConstructor, NodeType, ListItemType);
                    methods.Items.Add(nm);
                    nm = new method_info();
                    nm.method_text = string.Format(templateListAddNoSC, NodeType, ListItemType, ListName);
                    methods.Items.Add(nm);
                    nm = new method_info();
                    nm.method_text = string.Format(templateListAdd, NodeType, ListItemType, ListName);
                    methods.Items.Add(nm);
                }

         * */
        //*************end of node_def

        private void toolStripButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripButtonSort.Checked)
                SortNodes();
            else UnsortNodes();
            sortToolStripMenuItem.Checked = toolStripButtonSort.Checked;
        }

        private void reloadButton_Click_1(object sender, EventArgs e)
        {
            nodes_list_SelectedIndexChanged(sender, e);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sortToolStripMenuItem.Checked = toolStripButtonSort.Checked = !sortToolStripMenuItem.Checked;
        }

        private void toolStripButtonCut_Click(object sender, EventArgs e)
        {
            scintilla1.Commands.Execute(ScintillaNET.BindableCommand.Cut);
        }

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            scintilla1.Commands.Execute(ScintillaNET.BindableCommand.Copy);
        }

        private void toolStripButtonPaste_Click(object sender, EventArgs e)
        {
            scintilla1.Commands.Execute(ScintillaNET.BindableCommand.Paste);
        }

        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            scintilla1.Commands.Execute(ScintillaNET.BindableCommand.Undo);
        }

        private void toolStripButtonRedo_Click(object sender, EventArgs e)
        {
            scintilla1.Commands.Execute(ScintillaNET.BindableCommand.Redo);
        }

        private void fillInTagCategories(bool keep_filter = false)
        {
            tagCategories.Items.Clear();
            filterCategories.Items.Clear();
            if (!keep_filter)
                current_filter = new List<List<bool>>();
            for (int i = 0; i < node_gen.tag_cats.Count; i++)
            {
                tagCategories.Items.Add(node_gen.tag_cats[i].name);
                filterCategories.Items.Add(node_gen.tag_cats[i].name);
                if (!keep_filter)
                    current_filter.Add(Enumerable.Repeat(false, node_gen.tag_cats[i].tags.Count).ToList());
                else
                    current_filter[i].AddRange(Enumerable.Repeat(false, Math.Max(0, node_gen.tag_cats[i].tags.Count - current_filter[i].Count)).ToList());
            }
        }

        private void fillInTagItems(int cat_id)
        {
            tagItems.Text = "";
            for (int i = 0; i < temp_tags.Count; i++)
                if (temp_tags[i].Item1 == cat_id)
                    tagItems.Text += temp_tags[i].Item2 + "\r\n";
            if (tagItems.Text.Length > 0)
                tagItems.Text = tagItems.Text.Substring(0, tagItems.Text.Length - 2);
            tagItems.Lines = tagItems.Lines.OrderBy(x => x).ToArray();
        }

        private void getTagItems(int cat_id)
        {
            var remove_list = new List<Tuple<int, string>>();
            for (int i = 0; i < temp_tags.Count; i++)
                if (temp_tags[i].Item1 == cat_id)
                    remove_list.Add(temp_tags[i]);
            foreach (var tg in remove_list)
                temp_tags.Remove(tg);
            for (int i = 0; i < tagItems.Lines.Length; i++)
            {
                tagItems.Lines[i] = tagItems.Lines[i].Trim();
                if (tagItems.Lines[i] != "")
                    temp_tags.Add(new Tuple<int, string>(cat_id, tagItems.Lines[i]));
            }

            update_tags = false;
        }

        private void tagsCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new FilterTagsCollection(node_gen, selected_filter_cat);
            dlg.ShowDialog();
            set_node_info_tags(selected_node);
            fillInTagCategories();
            fill_in_filter_items();
            if (selected_filter_cat >= 0)
            {
                filterCategories.SelectedIndexChanged -= filterCategories_SelectedIndexChanged;
                filterCategories.SelectedIndices.Add(selected_filter_cat);
                filterCategories.SelectedIndexChanged += filterCategories_SelectedIndexChanged;
            }
        }

        private void tagCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selected_tag_cat > -1)
                getTagItems(selected_tag_cat);
            fillInTagItems(tagCategories.SelectedIndex);
            selected_tag_cat = tagCategories.SelectedIndex;
        }

        private void tagItems_TextChanged(object sender, EventArgs e)
        {
            update_tags = true;
        }


        //Filters
        private void fill_in_filter_items()
        {           
            if (selected_filter_cat == -1) return;
            filterItems.ItemChecked -= filterItems_ItemChecked;
            filterItems.Items.Clear();
            for (int i = 0; i < node_gen.tag_cats[selected_filter_cat].tags.Count; i++)
            {
                filterItems.Items.Add(node_gen.tag_cats[selected_filter_cat].tags[i].name);
                if (current_filter[selected_filter_cat][i])
                    filterItems.Items[i].Checked = true;
            }
            filterItems.ItemChecked += filterItems_ItemChecked;         
        }

        private void filterCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filterCategories.SelectedIndices.Count == 0) return;

            if (selected_filter_cat > -1)
            {
                for (int i = 0; i < filterItems.Items.Count; i++)
                    current_filter[selected_filter_cat][i] = filterItems.Items[i].Checked;
            }

            selected_filter_cat = filterCategories.SelectedIndices[0];

            fill_in_filter_items();
        }

        private void applyFilterForNode(List<Tuple<int, int>> filter, node_info node)
        {
            nodes_list.SelectedIndexChanged -= nodes_list_SelectedIndexChanged;

            foreach (var fltr in filter)
                if (node.tags.Find(x => x.Item1 == fltr.Item1 && x.Item2 == fltr.Item2) == null)
                {
                    nodes_list.Items.Remove(node);
                }

            nodes_list.SelectedIndexChanged += nodes_list_SelectedIndexChanged;
        }

        private void applyFilter(List<Tuple<int, int>> filter, bool renew)
        {
            nodes_list.SelectedIndexChanged -= nodes_list_SelectedIndexChanged;
            nodes_list.SelectedIndices.Clear();
            selected_node = null;
            selected_node_index = -1;

            if (renew)
                reload_nodes();
            List<object> to_remove = new List<object>();

            foreach (node_info i in nodes_list.Items)
            {
                foreach (var fltr in filter)
                    if (i.tags.Find(x => x.Item1 == fltr.Item1 && x.Item2 == fltr.Item2) == null)
                    {
                        to_remove.Add(i);
                        break;
                    }
            }

            foreach (var i in to_remove)
                nodes_list.Items.Remove(i);

            if (nodes_list.Items.Count > 0)
            {
                selected_node = nodes_list.Items[0] as node_info;
                selected_node_index = 0;
                set_node_info(nodes_list.Items[0] as node_info);
            }
            nodes_list.SelectedIndexChanged += nodes_list_SelectedIndexChanged;
        }

        private void applyFilter(Tuple<int, int> filter, bool renew = false)
        {
            var lst = new List<Tuple<int, int>>();
            lst.Add(filter);
            applyFilter(lst, renew);
        }

        private void applyCurrentFilterForNode(node_info node)
        {
            var lst = new List<Tuple<int, int>>();
            for (int i = 0; i < current_filter.Count; i++)
                for (int j = 0; j < current_filter[i].Count; j++)
                    if (current_filter[i][j])
                        lst.Add(new Tuple<int, int>(i, j));
            applyFilterForNode(lst, node);
        }

        private void applyCurrentFilter()
        {
            is_filter_enabled = false;
            var lst = new List<Tuple<int, int>>();
            for (int i = 0; i < current_filter.Count; i++)
                for (int j = 0; j < current_filter[i].Count; j++)
                    if (current_filter[i][j])
                    {
                        lst.Add(new Tuple<int, int>(i, j));
                        is_filter_enabled = true;
                    }
            applyFilter(lst, true);
        }

        private void filterItems_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            current_filter[selected_filter_cat][e.Item.Index] = e.Item.Checked;
            if (e.Item.Checked)
            {
                applyFilter(new Tuple<int, int>(selected_filter_cat, e.Item.Index));
                is_filter_enabled = true;
            }
            else
                applyCurrentFilter();
        }

        private void resetFilterButton_Click(object sender, EventArgs e)
        {           
            foreach (var cat in current_filter)
                for (int i = 0; i < cat.Count; i++)
                    cat[i] = false;

            for (int i = 0; i < filterItems.Items.Count; i++)
            {
                filterItems.Items[i].Checked = false;
            }

            is_filter_enabled = false;
            reload_nodes();
        }

        private void customFilterMode_CheckedChanged(object sender, EventArgs e)
        {
            if (nodes_list.SelectedItems.Count != 0)
            {
                var add_list = new List<object>();
                foreach (var o in nodes_list.SelectedItems)
                    add_list.Add(o);
                nodes_list.Items.Clear();
                nodes_list.Items.AddRange(add_list.ToArray());
                nodes_list.SelectedItem = selected_node;
                selected_node_index = nodes_list.Items.IndexOf(selected_node);
                is_filter_enabled = true;
            }                          
        }

        private void saveTempFilter_Click(object sender, EventArgs e)
        {
            var dlg = new TagEmForm(node_gen.tag_cats);
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dlg.categories.Text = dlg.categories.Text.Trim();
                dlg.tagName.Text = dlg.tagName.Text.Trim();
                if (dlg.tagName.Text != "")
                {
                    //oh that buggy C#! makes me do this cycle
                    for (int i = 0; i < dlg.categories.Items.Count; i++)
                        if (dlg.categories.Items[i].ToString() == dlg.categories.Text)
                        {
                            dlg.categories.SelectedIndex = i; break;
                        }

                    if (dlg.categories.SelectedIndex < 0)
                    {
                        if (dlg.categories.Text != "")
                        {
                            node_gen.tag_cats.Add(new FilterCategory() { name = dlg.categories.Text });
                            dlg.categories.Items.Add(dlg.categories.Text);
                            dlg.categories.SelectedIndex = dlg.categories.Items.Count - 1;                            

                            fillInTagCategories();
                            if (tagCategories.Items.Count > 0 && tagCategories.SelectedIndex == -1)
                                tagCategories.SelectedIndex = 0;                            
        
                        }
                        else return;
                    }                    
                    if (dlg.categories.SelectedIndex >= 0)
                    {
                        node_gen.new_tag_for_nodes(nodes_list.SelectedItems.Cast<node_info>(), nodes_list.Items.Count, dlg.categories.SelectedIndex, dlg.tagName.Text);
                        fillInTagCategories();
                        set_node_info_tags(selected_node);
                    }                    
                }
            }
        }

        private void filterPanelToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            panel6.Visible = filterPanelToolStripMenuItem.Checked;
            if (!panel6.Visible)
                panel5.Height = mainToolStrip.Height;
            else panel5.Height = mainToolStrip.Height + filter_panel_height;
        }

        private void saveNodeButton_Click(object sender, EventArgs e)
        {
            if (creation_mode)
            {
                if (node_name.Text != "")
                {
                    if (!get_node_info(selected_node))
                    {
                        return;
                    }
                    nodes_list.Items.Add(selected_node);
                    node_gen.add_node(selected_node);
                    get_node_info(selected_node);
                    fill_in_navigation_panel(selected_node);

                    creation_mode = false;
                }
            }
            else
                synchronize_node();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CloseCurrentFile() == CloseResult.NotClosed)
                return;

            node_gen = null;
            set_nodes_information();
        }

        private void nodes_list_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void globalRenamerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new GlobalRenamer(nodes_list, node_gen).ShowDialog();            
        }

        private void NavigationPanel_Resize(object sender, EventArgs e)
        {
            fill_in_navigation_panel(selected_node);
        }

        private void grammarBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> kwords = new List<string>();

            foreach (var node in node_gen.all_nodes)
                kwords.Add(node.ToString());

            var dlg = new GrammarBrowser(node_gen.generate_hierarchy(), kwords, selected_node.ToString(), this);
            if (hb_location.X >= 0)
                dlg.SetDialogPosition(hb_location);
            dlg.ShowDialog();
            hb_location = dlg.Location;
        }

        private void toolStripButtonSaveStore_Click(object sender, EventArgs e)
        {
            save_Click(sender, e);
            if (!Directory.Exists(version_storage)) Directory.CreateDirectory(version_storage);
            File.Copy(saveFileDialog1.FileName, version_storage + @"\" +
                      Path.GetFileName(saveFileDialog1.FileName) + "__" + DateTime.Now.ToString().Replace(':', '_') + ".nin");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutBox()).ShowDialog();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void nodes_list_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left || e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                int x = nodes_list.IndexFromPoint(nodes_list.PointToClient(Cursor.Position));
                if (x >= 0)
                {
                    if (Control.ModifierKeys == Keys.Shift)
                    {
                        int old_ind = nodes_list.Items.IndexOf(selected_node);
                        for (int i = Math.Min(old_ind, x); i < Math.Max(old_ind, x); i++)
                            nodes_list.SelectedIndices.Add(i);
                    }
                    else                        
                        nodes_list.SelectedIndices.Add(x);
                    new_selected_node = (nodes_list.Items[x] as node_info);
                }
            }
                        
        }

        private void filterAncestors_Click(object sender, EventArgs e)
        {
            nodes_list.SelectedIndexChanged -= nodes_list_SelectedIndexChanged;
            nodes_list.Items.Clear();
            
            node_info base_class = selected_node;
            while (base_class.base_class != null)
            {
                nodes_list.Items.Add(base_class);
                base_class = base_class.base_class;
            }
            nodes_list.Items.Add(base_class);

            nodes_list.SelectedItem = selected_node;
            selected_node_index = nodes_list.Items.IndexOf(selected_node);
            is_filter_enabled = true;

            nodes_list.SelectedIndexChanged += nodes_list_SelectedIndexChanged;
        }

        private void nodes_list_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                var x = nodes_list.Items.IndexOf(selected_node);
                if (x + 1 < nodes_list.Items.Count)
                    new_selected_node = (nodes_list.Items[x + 1] as node_info);
            }
            else
            if (e.KeyCode == Keys.Up)
            {
                var x = nodes_list.Items.IndexOf(selected_node);
                if (x - 1 >= 0)
                    new_selected_node = (nodes_list.Items[x - 1] as node_info);
            }
        }
    }
}
