//---------------------------------------------------------------------
// 
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//PARTICULAR PURPOSE.
//---------------------------------------------------------------------
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.Text;
using VisualPascalABC;
using VisualPascalABCPlugins;

namespace AdvancedDataGridView
{
	[
		ToolboxItem(false),
		DesignTimeVisible(false)
	]
    public class TreeGridNode : DataGridViewRow//, IComponent
    {
		internal TreeGridView _grid;
		internal TreeGridNode _parent;
		internal TreeGridNodeCollection _owner;
        internal bool IsExpanded;
		internal bool IsRoot;
		internal bool _isSited;
		internal bool _isFirstSibling;
		internal bool _isLastSibling;
		internal Image _image;
		internal int _imageIndex=1;
		internal bool UserRow = true;
		internal bool InExpanding;
		private Random rndSeed = new Random();
		public int UniqueValue = -1;
        TreeGridCell _treeCell;
        TreeGridNodeCollection childrenNodes;
		IListItem content;
		private int _index;
		private int _level;
		private bool childCellsCreated = false;
		private string text;
		private string type;
		private string name;
		internal bool populated;
		private bool canEditText;
		
		// needed for IComponent
		private ISite site = null;
		private EventHandler disposed = null;

		public TreeGridNode(TreeGridView owner)
			: this()
		{
			this._grid = owner;
			
			//this.IsExpanded = true;
		}
		
		public TreeGridNode(TreeGridView owner, IListItem content):this(owner)
		{
			this.content = content;
		}
		
        public TreeGridNode()
        {            
			_index = -1;
			_level = -1;
			this.MinimumHeight = 20;
			//this.Frozen = true;
            IsExpanded = false;
			UniqueValue = this.rndSeed.Next();
			_isSited = false;
			_isFirstSibling = false;
			_isLastSibling = false;
			_imageIndex = -1;
		}

		public override object Clone()
		{
			TreeGridNode r = (TreeGridNode)base.Clone();
			r.UniqueValue = -1;
			r._level = this._level;
			r._grid = this._grid;
			r._parent = this.Parent;

			r._imageIndex = this._imageIndex;
			if (r._imageIndex == -1)
				r.Image = this.Image;

			r.IsExpanded = this.IsExpanded;
			//r.treeCell = new TreeGridCell();

			return r;
		}
		
		public override int GetPreferredHeight(int rowIndex, DataGridViewAutoSizeRowMode autoSizeRowMode, bool fixedWidth)
		{
			int h =  base.GetPreferredHeight(rowIndex, autoSizeRowMode, fixedWidth);
			return h;
		}
		
		internal protected virtual void UnSited()
		{
			// This row is being removed from being displayed on the grid.
			TreeGridCell cell;
			foreach (DataGridViewCell DGVcell in this.Cells)
			{
				cell = DGVcell as TreeGridCell;
				if (cell != null)
				{
					cell.UnSited();
				}
			}
			this._isSited = false;
		}

		internal protected virtual void Sited()
		{
			// This row is being added to the grid.
			this._isSited = true;
			this.childCellsCreated = true;
			//Debug.Assert(this._grid != null);

			TreeGridCell cell;
			foreach (DataGridViewCell DGVcell in this.Cells)
			{
				cell = DGVcell as TreeGridCell;
				if (cell != null)
				{
					cell.Sited();// Level = this.Level;
				}
			}

		}

		// Represents the index of this row in the Grid
		[System.ComponentModel.Description("Represents the index of this row in the Grid. Advanced usage."),
		System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),
		 Browsable(false),
		 DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int RowIndex{
			get{
				return base.Index;
			}
		}

		// Represents the index of this row based upon its position in the collection.
		[Browsable(false),
		 DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int Index
		{
			get
			{
				if (_index == -1)
				{
					// get the index from the collection if unknown
					_index = this._owner.IndexOf(this);
				}

				return _index;
			}
			internal set
			{
				_index = value;
			}
		}

        [Browsable(false),
        EditorBrowsable( EditorBrowsableState.Never), 
        DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public ImageList ImageList
        {
            get
            {
                if (this._grid != null)
                    return this._grid.ImageList;
                else
                    return null;
            }
        }

		private bool ShouldSerializeImageIndex()
		{
			return (this._imageIndex != -1 && this._image == null);
		}

//        [Category("Appearance"),
//        Description("..."), DefaultValue(-1),
//        TypeConverter(typeof(ImageIndexConverter)),
//        Editor("System.Windows.Forms.Design.ImageIndexEditor", typeof(UITypeEditor))]
		public int ImageIndex
		{
			get { return _imageIndex; }
			set
			{
				_imageIndex = value;
				if (_imageIndex != -1)
				{
					// when a imageIndex is provided we do not store the image.
					this._image = null;
				}
				if (this._isSited)
				{
					// when the image changes the cell's style must be updated
					this._treeCell.UpdateStyle();
					if (this.Displayed)
						this._grid.InvalidateRow(this.RowIndex);
				}
			}
		}

		private bool ShouldSerializeImage()
		{
			return (this._imageIndex == -1 && this._image != null);
		}

		public Image Image
		{
			get {
				if (_image == null && _imageIndex != -1)
				{
					if (this.ImageList != null && this._imageIndex < this.ImageList.Images.Count)
					{
						// get image from image index
						return this.ImageList.Images[this._imageIndex];
					}
					else
						return null;
				}
				else
				{
					// image from image property
					return this._image;
				};
			}
			set
			{
				_image = value;
				if (_image != null)
				{
					// when a image is provided we do not store the imageIndex.
					this._imageIndex = -1;
				}
				if (this._isSited)
				{
					// when the image changes the cell's style must be updated
					this._treeCell.UpdateStyle();
					if (this.Displayed)
						this._grid.InvalidateRow(this.RowIndex);
				}
			}
		}

		protected override DataGridViewCellCollection CreateCellsInstance()
		{
			DataGridViewCellCollection cells = base.CreateCellsInstance();
			cells.CollectionChanged += cells_CollectionChanged;
			return cells;
		}

		void cells_CollectionChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
		{
			// Exit if there already is a tree cell for this row
			if (_treeCell != null) return;

			if (e.Action == System.ComponentModel.CollectionChangeAction.Add || e.Action == System.ComponentModel.CollectionChangeAction.Refresh)
			{
				TreeGridCell treeCell = null;

				if (e.Element == null)
				{
					foreach (DataGridViewCell cell in base.Cells)
					{
						if (cell.GetType().IsAssignableFrom(typeof(TreeGridCell)))
						{
							treeCell = (TreeGridCell)cell;
							break;
						}

					}
				}
				else
				{
					treeCell = e.Element as TreeGridCell;
				}

				if (treeCell != null) 
				  _treeCell = treeCell;
			}
		}
		
		public IListItem Content
        {
            get
            {
                return content;
            }
            set
            {
                if (content != null)
                {
                    content.Changed -= OnContentChanged;
                }
                content = value;
                if (content != null)
                {
                    content.Changed += OnContentChanged;
                }
                Update();
            }
        }
		
		void OnContentChanged(object sender, ListItemEventArgs e)
        {
			//Update();
        }
		
		public string Type
        {
            get { return type; }
        }
		
		public string Text
		{
			get { return text; }
		}
		
		private static void internalUpdateNodes(object prm)
		{
			object[] prms = prm as object[];
			TreeGridView tree = prms[0] as TreeGridView;
			TreeGridNodeCollection collection = prms[1] as TreeGridNodeCollection;
			IList<ListItem> contents = prms[2] as IList<ListItem>;
			for (int i = 0; i < contents.Count; i++)
            {
                try
                {
				if (i < collection.Count)
                {
                    // Overwrite
                    //if (!contents[i].IsLiteral)
                        ((TreeGridNode)collection[i]).Content = contents[i];
                }
                else
                {
                    // Add
                    //if (!contents[i].IsLiteral)
                    {
                    	TreeGridNode tn = new TreeGridNode(tree,contents[i]);
                    	tn.UserRow = false;
                    	tn.Update();
                    	collection.Add(tn);
                    }
                }
                }
                catch
                {
                	
                }
            }
            // Delete other nodes
            while (collection.Count > contents.Count)
            {
                collection.RemoveAt(collection.Count - 1);
            }
		}
		private static System.Threading.Thread th;
		
		public static void UpdateNodesForLocalList(TreeGridView tree, TreeGridNodeCollection collection, IList<IListItem> contents)
        {
        	// Add or overwrite existing items
        	for (int i = 0; i < contents.Count; i++)
            {
        		if (i < collection.Count)
                {
                    // Overwrite
                    //if (!contents[i].IsLiteral)
                        ((TreeGridNode)collection[i]).Content = contents[i];
                }
                else
                {
                    // Add
                    //if (!contents[i].IsLiteral)
                    {
                    	TreeGridNode tn = new TreeGridNode(tree,contents[i]);
                    	tn.UserRow = false;
                    	tn.Update();
                    	collection.Add(tn);
                    	tree.Rows.Add(tn);
                    	if (contents[i] is ModuleItem)
                    		tn.Expand();
                    }
                }
            }
            // Delete other nodes
            while (collection.Count > contents.Count)
            {
                collection.RemoveAt(collection.Count - 1);
                tree.Rows.RemoveAt(tree.Rows.Count-1);
            }
            for (int i=0; i<tree.Rows.Count; i++)
            {
            	tree.InvalidateRow(i);
            }
            //tree.Update();
//            tree.UpdateSelection();
//            tree.FullUpdate();
        }
		
		public static void UpdateNodes(TreeGridView tree, TreeGridNodeCollection collection, IList<IListItem> contents)
        {
        	// Add or overwrite existing items
            for (int i = 0; i < contents.Count; i++)
            {
                if (i < collection.Count)
                {
                    // Overwrite
                    //if (!contents[i].IsLiteral)
                        ((TreeGridNode)collection[i]).Content = contents[i];
                }
                else
                {
                    // Add
                    //if (!contents[i].IsLiteral)
                    {
                    	TreeGridNode tn = new TreeGridNode(tree,contents[i]);
                    	tn.UserRow = false;
                    	tn.Update();
                    	collection.Add(tn);
                    }
                }
            }
            // Delete other nodes
            while (collection.Count > contents.Count)
            {
                collection.RemoveAt(collection.Count - 1);
            }
            //tree.Update();
//            tree.UpdateSelection();
//            tree.FullUpdate();
        }
		
		private bool updated=true;
		public bool is_changed=false;
		
		internal void FastUpdate()
		{
			if (this.content != null && !updated)
			{
				this._image = content.Image;
				if (this._image == null)
            	this._image = CodeCompletionProvider.ImagesProvider.ImageList.Images[CodeCompletionProvider.ImagesProvider.IconNumberField];
				string o = content.Text;
				if (o != "")
				{
					this.Cells[1].Value = o;
				}
				this.HasChildren = content.HasSubItems;
				this.canEditText = content.CanEditText;
            	this.type = content.Type;
            	if (this.name == "base")
                	this.Cells[1].Value = "{" + content.Type + "}";
				if (this.Cells[2].Value == null)
				this.Cells[2].Value = this.type;
				if (o != "")
				updated = true;
				//if (this._grid != null)
				//this._grid.InvalidateCell(this.Cells[1]);
			}
			
		}
		
		[System.ComponentModel.EditorBrowsableAttribute()]
		protected override void PaintCells(Graphics graphics, Rectangle clipBounds, Rectangle rowBounds, int rowIndex, DataGridViewElementStates rowState, bool isFirstDisplayedRow, bool isLastVisibleRow, DataGridViewPaintParts paintParts)
		{
			FastUpdate();
			base.PaintCells(graphics, clipBounds, rowBounds, rowIndex, rowState, isFirstDisplayedRow, isLastVisibleRow, paintParts);
		}
		
		private void Update()
		{
			//DateTime start = Debugger.Util.HighPrecisionTimer.Now;
			updated = false;
            //this.HasChildren = Content.HasSubItems;
            //this._image = content.Image;
            if (this._image == null)
            	this._image = CodeCompletionProvider.ImagesProvider.ImageList.Images[CodeCompletionProvider.ImagesProvider.IconNumberField];
            this.name = content.Name;
//            this.text = content.Text;
            
            if (this.name == "" || this.name == null) 
                this.name = "base";
//            this.canEditText = content.CanEditText;
//            this.type = content.Type;
            
//            if (this.name == "base")
//                this.text = "{" + content.Type + "}";
            //DateTime time = Debugger.Util.HighPrecisionTimer.Now;
            //this.type = time.ToLongTimeString() + "." + time.Millisecond.ToString();
            if (!this.UserRow)
            this.Cells[0].Value = this.name;
			//this.Cells[1].Value = null;//this.text;
			if (this.Cells[2].Value == null)
				this.Cells[2].Value = this.type;
			//this.Cells[2].Value = this.type;
            //DateTime end = Debugger.Util.HighPrecisionTimer.Now;

            //LoggingService.InfoFormatted("Updated node {0} ({1} ms)", FullName, (end - start).TotalMilliseconds);

            if (this.IsExpanded)
            {
//                this.childrenNodes.Clear();
//                populated = false;
//                this.Collapse();
                //return;
                //this.Tree.FullUpdate();
                //populated = true;
                if (content is ModuleItem)
                {
                    UpdateNodes(this._grid, this.Nodes, Content.SubItems);
//                    this._grid.Update();
                    //this.Tree.UpdateSelection();
                    //this.Tree.FullUpdate();
                }
                else
                {
                    try
                    {
                        if (content is ValueItem || content is BaseTypeItem/*&& (content as ValueItem).Value.Type.Module.SymbolsLoaded*/)
                        {
                            UpdateNodes(this._grid, this.Nodes, Content.SubItems);
                            //this._grid.Update();
//                            this.Tree.UpdateSelection();
//                            this.Tree.FullUpdate();
                        }
                        else
                        {
                            populated = false;
//                            this.Tree.FullUpdate();
                        }
                    }
                    catch (System.Exception e)
                    {
                        populated = false;
//                        this.Tree.FullUpdate();
                    }
                }
                //
            }
            else
            {
//                this.childrenNodes.Clear();
////                this.Tree.UpdateSelection();
////				this._grid.Update();
//                populated = false;
            }
		}
		
//		[Category("Data"),
//		 Description("The collection of root nodes in the treelist."),
//		 DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
//		 Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public TreeGridNodeCollection Nodes
        {
            get
            {
                if (childrenNodes == null)
                {
                    childrenNodes = new TreeGridNodeCollection(this);
                }
                return childrenNodes;
            }
            set { ;}
        }

		// Create a new Cell property because by default a row is not in the grid and won't
		// have any cells. We have to fabricate the cell collection ourself.
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewCellCollection Cells
		{
			get
			{
				if (!childCellsCreated && this.DataGridView == null)
				{
                    if (this._grid == null) return null;

					this.CreateCells(this._grid);
					childCellsCreated = true;
				}
				return base.Cells;
			}
		}

		[Browsable(false),
		 DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Level
		{
			get {
				if (this._level == -1)
				{
					// calculate level
					int walk = 0;
					TreeGridNode walkRow = this.Parent;
					while (walkRow != null)
					{
						walk++;
						walkRow = walkRow.Parent;
					}
					
					this._level = walk+1;
				}
				return this._level; }
		}

		[Browsable(false),
		 DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TreeGridNode Parent
		{
			get
			{
				return this._parent;
			}
		}
		
		private bool hasChildren;
		
		[Browsable(false),
		 DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool HasChildren
		{
			get
			{
				return hasChildren;
				//return (this.childrenNodes != null && this.Nodes.Count != 0);
			}
			set
			{
				hasChildren = value;
			}
		}

        [Browsable(false)]
        public bool IsSited
        {
            get
            {
                return this._isSited;
            }
        }
		[Browsable(false)]
		public bool IsFirstSibling
		{
			get
			{
				return (this.Index == 0);
			}
		}

		[Browsable(false)]
		public bool IsLastSibling
		{
			get
			{
				TreeGridNode parent = this.Parent;
				if (parent != null && parent.HasChildren)
				{
					return (this.Index == parent.Nodes.Count - 1);
				}
				else
					return true;
			}
		}
		
		public virtual bool Collapse()
		{
			if (this._grid != null)
			return this._grid.CollapseNode(this);
			return false;
		}
		
		public void OnExpanding()
        {
            try
            {
                if (!populated)
                {
                	if (this._owner == null)
                	{
                		this._owner = this.Nodes;
                	}
                	
                	foreach (ListItem item in Content.SubItems)
                    {
                        TreeGridNode tn = new TreeGridNode(this._grid, item);
                        tn._owner = Nodes;
                        tn._parent = this;
                        tn.InExpanding = true;
                        tn.UserRow = false;
                        item.Changed += tn.OnContentChanged;
                        tn.Update();
                    	Nodes.Add(tn);
                    }
                    populated = true;
                    //this.IsExpandedOnce = true;
//                    this.Tree.UpdateSelection();
//                    this.Tree.FullUpdate();
                }
            }
            catch (System.Exception e)
            {
				Console.WriteLine(e.Message + " " + e.StackTrace);
            }
        }
		
		public virtual bool Expand()
		{
			if (this._grid != null)
			{
				OnExpanding();
				bool b = this._grid.ExpandNode(this);
				return b;
			}
			else
			{
				this.IsExpanded = true;
				return true;
			}
		}

		internal protected virtual bool InsertChildNode(int index, TreeGridNode node)
		{
			node._parent = this;
			node._grid = this._grid;

            // ensure that all children of this node has their grid set
            if (this._grid != null)
                UpdateChildNodes(node);

			//TODO: do we need to use index parameter?
			if ((this._isSited || this.IsRoot) && this.IsExpanded)
				this._grid.SiteNode(node);
			return true;
		}

		internal protected virtual bool InsertChildNodes(int index, params TreeGridNode[] nodes)
		{
			foreach (TreeGridNode node in nodes)
			{
				this.InsertChildNode(index, node);
			}
			return true;
		}

		internal protected virtual bool AddChildNode(TreeGridNode node)
		{
			node._parent = this;
			node._grid = this._grid;

            // ensure that all children of this node has their grid set
            if (this._grid != null)
                UpdateChildNodes(node);

			if ((this._isSited || this.IsRoot) && this.IsExpanded && !node._isSited)
				this._grid.SiteNode(node);

			return true;
		}
		
		internal protected virtual bool AddChildNodes(params TreeGridNode[] nodes)
		{
			//TODO: Convert the final call into an SiteNodes??
			foreach (TreeGridNode node in nodes)
			{
				this.AddChildNode(node);
			}
			return true;

		}

		internal protected virtual bool RemoveChildNode(TreeGridNode node)
		{
			if ((this.IsRoot || this._isSited) && this.IsExpanded )
			{
				//We only unsite out child node if we are sited and expanded.
				this._grid.UnSiteNode(node);
			
			}
            node._grid = null;	
			node._parent = null;
			return true;

		}

		internal protected virtual bool ClearNodes()
		{
            if (this.HasChildren)
            {
                for (int i = this.Nodes.Count - 1; i >= 0; i--)
                {
                    this.Nodes.RemoveAt(i);
                }
            }
			return true;
		}

        [
            Browsable(false),
            EditorBrowsable(EditorBrowsableState.Advanced)
        ]
        public event EventHandler Disposed
        {
            add
            {
                this.disposed += value;
            }
            remove
            {
                this.disposed -= value;
            }
        }

		[
			Browsable(false),
			DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public ISite Site
		{
			get
			{
				return this.site;
			}
			set
			{
				this.site = value;
			}
		}

        private void UpdateChildNodes(TreeGridNode node)
        {
            if (node.HasChildren)
            {
                foreach (TreeGridNode childNode in node.Nodes)
                {
                    childNode._grid = node._grid;
                    this.UpdateChildNodes(childNode);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(36);
            sb.Append("TreeGridNode { Index=");
            sb.Append(this.RowIndex.ToString(System.Globalization.CultureInfo.CurrentCulture));
            sb.Append(" }");
            return sb.ToString();
        }

		//protected override void Dispose(bool disposing) {
		//    if (disposing)
		//    {
		//        lock(this)
		//        {
		//            if (this.site != null && this.site.Container != null)
		//            {
		//                this.site.Container.Remove(this);
		//            }

		//            if (this.disposed != null)
		//            {
		//                this.disposed(this, EventArgs.Empty);
		//            }
		//        }
		//    }

		//    base.Dispose(disposing);
		//}
	}

}
