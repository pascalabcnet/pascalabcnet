// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using Debugger;

namespace VisualPascalABC
{
    public static class ScreenScale
    {
        private static double scale = -1;
        public static void FirstCalcScale(Form f)
        {
            var gr = f.CreateGraphics();
            scale = (double)gr.DpiX / 96;
            gr.Dispose();
        }
        public static double Calc()
        {
            if (scale > 0)
                return scale;
            return 1.5;
            //var gr = Graphics.FromHwnd(Handle);
            /*var dpiXProperty = typeof(System.Windows.SystemParameters).GetProperty("DpiX", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			var dpiX = (int)dpiXProperty.GetValue(null, null);*/
            //scale = (double)gr.DpiX / 96;
            //return scale;
        }
    }

    public class DynamicTreeDebuggerRow : DynamicTreeRow
    {
        // Columns:
        // 0 = plus sign
        // 1 = icon
        // 2 = text
        // 3 = value

        ListItem listItem;

        Image image;
        bool populated = false;
        bool visible = true;

        public ListItem ListItem
        {
            get
            {
                return listItem;
            }
        }

        public DynamicTreeDebuggerRow(NamedValue val)
            : this(new ValueItem(val,null))
        {

        }

        public DynamicTreeDebuggerRow(ListItem listItem)
        {
            if (listItem == null) throw new ArgumentNullException("listItem");

            this.listItem = listItem;
            this.listItem.Changed += delegate { Update(true); };
            this.Shown += delegate
            {
                visible = true;
                WorkbenchServiceFactory.DebuggerManager.DoInPausedState(delegate { Update(true); });
            };
            this.Hidden += delegate
            {
                visible = false;
            };

            DebuggerGridControl.AddColumns(this.ChildColumns);

            this[1].Paint += OnIconPaint;
            this[3].FinishLabelEdit += OnLabelEdited;
            this[3].MouseDown += OnMouseDown;
			
            Update(true);
        }

        void Update(bool show_val)
        {
            if (!visible) return;
            this[1].Text = ""; // Icon
            if (listItem.SpecialName != null)
            	this[2].Text = listItem.SpecialName;
            else
            this[2].Text = listItem.Name;
            if (!show_val) return;
            this.image = listItem.Image;
            
            if (this[2].Text == "" || this[2].Text == null) 
            	if (!(listItem is BaseTypeItem && listItem.ImageIndex != -1))
            	this[2].Text = "base";
            	else
            	this[2].Text = listItem.Type;
            if (listItem.Name == "" || this.ListItem == null) this[3].Text = "{" + listItem.Type + "}";
            else 
            	this[3].Text = listItem.Text;
            this[3].AllowLabelEdit = listItem.CanEditText;

            this.ShowPlus = listItem.HasSubItems;
            this.ShowMinusWhileExpanded = true;
        }

        void OnIconPaint(object sender, ItemPaintEventArgs e)
        {
            if (image != null)
            {
                // SSM 21.01.19 High dpi
                e.Graphics.DrawImage/*Unscaled*/(image, e.ClipRectangle);
            }
        }

        void OnLabelEdited(object sender, DynamicListEventArgs e)
        {
            try
            {
            	listItem.SetText(((DynamicListItem)sender).Text);
            }
            catch
            {
            	
            }
        }

        void OnMouseDown(object sender, DynamicListMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip menu = listItem.GetContextMenu();
                if (menu != null)
                {
                    menu.Show(e.List, e.Location);
                }
            }
        }

        /// <summary>
        /// Called when plus is pressed in debugger tooltip.
        /// Sets the data to be show in the next level.
        /// </summary>
        protected override void OnExpanding(DynamicListEventArgs e)
        {
            if (!populated)
            {
                WorkbenchServiceFactory.DebuggerManager.DoInPausedState(delegate { Populate(); });
            }
        }

        void Populate()
        {
            this.ChildRows.Clear();
            foreach (ListItem subItem in listItem.SubItems)
            {
                this.ChildRows.Add(new DynamicTreeDebuggerRow(subItem));
            }
            populated = true;
        }
    }

    public class ToolTipInfo
    {
        object toolTipObject;

        /// <summary>
        /// Gets the tool tip text to be displayed.
        /// May be <c>null</c>.
        /// </summary>
        public string ToolTipText
        {
            get
            {
                return this.toolTipObject as string;
            }
        }

        /// <summary>
        /// Gets the DebuggerGridControl to be shown as tooltip.
        /// May be <c>null</c>.
        /// </summary>
        public Control ToolTipControl
        {
            get
            {
                return this.toolTipObject as Control;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolTipInfo"/> class.
        /// </summary>
        /// <param name="toolTipText">The tooltip text to be displayed.</param>
        public ToolTipInfo(string toolTipText)
        {
            this.toolTipObject = toolTipText;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolTipInfo"/> class.
        /// </summary>
        /// <param name="toolTipControl">The DebuggerGridControl to be shown as tooltip.</param>
        public ToolTipInfo(Control toolTipControl)
        {
            this.toolTipObject = toolTipControl;
        }
    }
    public class DebuggerGridControl : DynamicList
    {
        // Columns:
        // 0 = plus sign
        // 1 = icon
        // 2 = text
        // 3 = value

        DynamicTreeRow row;

        public static void AddColumns(IList<DynamicListColumn> columns)
        {
            columns.Add(new DynamicListColumn());
            columns.Add(new DynamicListColumn());
            columns.Add(new DynamicListColumn());
            columns.Add(new DynamicListColumn());
            columns[0].BackgroundBrush = Brushes.White;
            columns[0].BackgroundBrushInactive = Brushes.White;
            columns[0].RowHighlightBrush = null;

            // default is allowgrow = true and autosize = false
            columns[0].AllowGrow = false;
            columns[1].AllowGrow = false;
            // SSM 21.01.19 High DPI
            columns[1].Width = 16 + Convert.ToInt32(16 * (ScreenScale.Calc()-1));
            columns[1].ColumnSeperatorColor = Color.Transparent;
            columns[1].ColumnSeperatorColorInactive = Color.Transparent;
            columns[2].AutoSize = true;
            columns[2].MinimumWidth = 75;
            columns[2].ColumnSeperatorColor = Color.White;
            columns[2].ColumnSeperatorColorInactive = Color.FromArgb(172, 168, 153);
            columns[3].AutoSize = true;
            columns[3].MinimumWidth = 75;
        }

        public DebuggerGridControl(DynamicTreeRow row)
        {
            this.row = row;

            BeginUpdate();

            AddColumns(Columns);

            Rows.Add(row);

            row.Expanded += delegate { isExpanded = true; };
            row.Collapsed += delegate { isExpanded = false; };

            CreateControl();
            using (Graphics g = CreateGraphics())
            {
                this.Width = GetRequiredWidth(g);
            }
            this.Height = row.Height;
            EndUpdate();
        }

        DynamicTreeRow.ChildForm frm;

        public void ShowForm(ICSharpCode.TextEditor.TextArea textArea, /*Point*/ICSharpCode.TextEditor.TextLocation logicTextPos)
        {
            frm = new DynamicTreeRow.ChildForm();
            frm.AllowResizing = false;
            frm.Owner = textArea.FindForm();
            int ypos = (textArea.Document.GetVisibleLine(logicTextPos.Y) + 1) * textArea.TextView.FontHeight - textArea.VirtualTop.Y;
            Point p = new Point(0, ypos);
            p = textArea.PointToScreen(p);
            p.X = Control.MousePosition.X - 16;
            p.Y -= 1;
            frm.StartPosition = FormStartPosition.Manual;
            frm.ShowInTaskbar = false;
            frm.Location = p;
            frm.ClientSize = new Size(Width + 2, row.Height + 2);
            Dock = DockStyle.Fill;
            frm.Controls.Add(this);
            frm.ShowWindowWithoutActivation = true;
            frm.KeyPreview = true;
            frm.KeyDown += DebuggerGridControl_KeyDown;
            frm.Show();
            
            textArea.Click += OnTextAreaClick;
            textArea.KeyDown += OnTextAreaClick;
            frm.ClientSize = new Size(frm.ClientSize.Width, row.Height + 2);
        }

        private void DebuggerGridControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F8)
            {
                WorkbenchServiceFactory.DebuggerManager.Status = DebugStatus.StepOver;
                WorkbenchServiceFactory.DebuggerManager.StepOver();
            }
        }

        private void Frm_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void TextArea_LostFocus(object sender, EventArgs e)
        {
            
        }

        public bool IsMouseOver
        {
            get
            {
                if (frm != null && !frm.IsDisposed)
                {
                    return frm.ClientRectangle.Contains(frm.PointToClient(Control.MousePosition));
                }
                return false;
            }
        }

        void OnTextAreaClick(object sender, EventArgs e)
        {
            ((ICSharpCode.TextEditor.TextArea)sender).KeyDown -= OnTextAreaClick;
            ((ICSharpCode.TextEditor.TextArea)sender).Click -= OnTextAreaClick;
            frm.Close();
            this.isExpanded = false;
        }

        bool isExpanded;

        public bool AllowClose
        {
            get
            {
                return !isExpanded;
            }
        }
    }

    public class DynamicTreeRow : DynamicListRow
    {
        CollectionWithEvents<DynamicListColumn> childColumns = new CollectionWithEvents<DynamicListColumn>();
        CollectionWithEvents<DynamicListRow> childRows = new CollectionWithEvents<DynamicListRow>();

        DynamicListItem plus;

        public DynamicTreeRow()
        {
            plus = this[0];
            ShowPlus = true;
        }

        #region Plus painting
        bool showPlus;

        public bool ShowPlus
        {
            get
            {
                return showPlus;
            }
            set
            {
                if (showPlus == value)
                    return;
                showPlus = value;
                plus.HighlightBrush = showPlus ? Brushes.AliceBlue : null;
                //plus.Cursor = showPlus ? Cursors.Hand : null;
                if (showPlus)
                {
                    plus.Click += OnPlusClick;
                    plus.Paint += OnPlusPaint;
                    //plus.MouseHover += OnPlusMouseHover;
                }
                else
                {
                    plus.Click -= OnPlusClick;
                    plus.Paint -= OnPlusPaint;
                    //plus.MouseHover -= OnPlusMouseHover;
                }
                OnShowPlusChanged(EventArgs.Empty);
            }
        }

        static readonly Color PlusBorder = Color.FromArgb(120, 152, 181);
        static readonly Color LightPlusBorder = Color.FromArgb(176, 194, 221);
        public static readonly Color DefaultExpandedRowColor = Color.FromArgb(235, 229, 209);

        public static void DrawPlusSign(Graphics graphics, Rectangle r, bool drawMinus)
        {
            using (Brush b = new LinearGradientBrush(r, Color.White, DynamicListColumn.DefaultRowHighlightBackColor, 66f))
            {
                graphics.FillRectangle(b, r);
            }
            using (Pen p = new Pen(PlusBorder))
            {
                graphics.DrawRectangle(p, r);
            }
            using (Brush b = new SolidBrush(LightPlusBorder))
            {
                graphics.FillRectangle(b, new Rectangle(r.X, r.Y, 1, 1));
                graphics.FillRectangle(b, new Rectangle(r.Right, r.Y, 1, 1));
                graphics.FillRectangle(b, new Rectangle(r.X, r.Bottom, 1, 1));
                graphics.FillRectangle(b, new Rectangle(r.Right, r.Bottom, 1, 1));
            }
            graphics.DrawLine(Pens.Black, r.Left + 2, r.Top + r.Height / 2, r.Right - 2, r.Top + r.Height / 2);
            if (!drawMinus)
            {
                graphics.DrawLine(Pens.Black, r.Left + r.Width / 2, r.Top + 2, r.Left + r.Width / 2, r.Bottom - 2);
            }
        }

        protected virtual void OnPlusPaint(object sender, ItemPaintEventArgs e)
        {
            Rectangle r = e.ClipRectangle;
            var sc = ScreenScale.Calc();
            var infl = Convert.ToInt32(4 * sc);
            r.Inflate(-infl, -infl);
            DrawPlusSign(e.Graphics, r, expandedIn != null && expandedIn.Contains(e.List));
        }

        protected override DynamicListItem CreateItem()
        {
            DynamicListItem item = base.CreateItem();
            item.Paint += delegate(object sender, ItemPaintEventArgs e)
            {
                if (e.Item != plus && expandedIn != null && !expandedRowColor.IsEmpty && expandedIn.Contains(e.List))
                {
                    using (Brush b = new SolidBrush(expandedRowColor))
                    {
                        e.Graphics.FillRectangle(b, e.FillRectangle);
                    }
                }
            };
            return item;
        }

        List<DynamicList> expandedIn;
        Color expandedRowColor = DefaultExpandedRowColor;

        public bool ShowMinusWhileExpanded
        {
            get
            {
                return expandedIn != null;
            }
            set
            {
                if (this.ShowMinusWhileExpanded == value)
                    return;
                expandedIn = value ? new List<DynamicList>() : null;
            }
        }

        /// <summary>
        /// Gets/Sets the row color used when the row is expanded. Only works together with ShowMinusWhileExpanded.
        /// </summary>
        public Color ExpandedRowColor
        {
            get
            {
                return expandedRowColor;
            }
            set
            {
                expandedRowColor = value;
            }
        }
        #endregion

        #region Events
        public event EventHandler<DynamicListEventArgs> Expanding;
        public event EventHandler<DynamicListEventArgs> Expanded;
        public event EventHandler<DynamicListEventArgs> Collapsed;
        public event EventHandler ShowPlusChanged;

        protected virtual void OnExpanding(DynamicListEventArgs e)
        {
            if (Expanding != null)
            {
                Expanding(this, e);
            }
        }
        protected virtual void OnExpanded(DynamicListEventArgs e)
        {
            if (Expanded != null)
            {
                Expanded(this, e);
            }
        }
        protected virtual void OnCollapsed(DynamicListEventArgs e)
        {
            if (Collapsed != null)
            {
                Collapsed(this, e);
            }
        }
        public virtual void OnShowPlusChanged(EventArgs e)
        {
            if (ShowPlusChanged != null)
            {
                ShowPlusChanged(this, e);
            }
        }
        #endregion

        #region Properties
        public CollectionWithEvents<DynamicListColumn> ChildColumns
        {
            get
            {
                return childColumns;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                childColumns = value;
            }
        }

        public CollectionWithEvents<DynamicListRow> ChildRows
        {
            get
            {
                return childRows;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                childRows = value;
            }
        }

        //public static readonly Color DefaultBorderColor = Color.FromArgb(195, 192, 175);
		public static readonly Color DefaultBorderColor = Color.LightGray;
        Color childBorderColor = DefaultBorderColor;

        public Color ChildBorderColor
        {
            get
            {
                return childBorderColor;
            }
            set
            {
                childBorderColor = value;
            }
        }
        #endregion

        #region Child form
        static bool isOpeningChild;

        /// <summary>
        /// Block the next click event - used to ensure that a click on the "-" sign
        /// does not cause the submenu to open again when the "-" sign becomes a "+" sign
        /// before the click event is handled.
        /// </summary>
        bool blockClickEvent;
		
        protected virtual void OnPlusMouseHover(object sender, DynamicListEventArgs e)
        {
        	OnPlusClick(sender, e);
        }
        
        protected virtual void OnPlusClick(object sender, DynamicListEventArgs e)
        {
            if (blockClickEvent) 
            { 
            	blockClickEvent = false; 
            	return;
            }
            OnExpanding(e);
            ChildForm frm = new ChildForm();
            frm.Closed += delegate
            {
                blockClickEvent = true;
                if (expandedIn != null)
                    expandedIn.Remove(e.List);
                OnCollapsed(e);
                plus.RaiseItemChanged();
                Timer timer = new Timer();
                timer.Interval = 200;
                timer.Tick += delegate(object sender2, EventArgs e2)
                {
                    ((Timer)sender2).Stop();
                    ((Timer)sender2).Dispose();
                    blockClickEvent = false;
                };
                timer.Start();
            };
            Point p = e.List.PointToScreen(e.List.GetPositionFromRow(this));
            p.Offset(e.List.Columns[0].Width, Height);
            frm.StartPosition = FormStartPosition.Manual;
            frm.BackColor = childBorderColor;
            frm.Location = p;
            frm.ShowInTaskbar = false;
            frm.Owner = e.List.FindForm();

            VerticalScrollContainer scrollContainer = new VerticalScrollContainer();
            scrollContainer.Dock = DockStyle.Fill;

            DynamicList childList = new DynamicList(childColumns, childRows);
            childList.Dock = DockStyle.Fill;
            childList.KeyDown += delegate(object sender2, KeyEventArgs e2)
            {
                if (e2.KeyData == Keys.Escape)
                {
                    frm.Close();
                    // workaround focus problem: sometimes the mainform gets focus after this
                    e.List.FindForm().Focus();
                }
            };
            scrollContainer.Controls.Add(childList);

            frm.Controls.Add(scrollContainer);

            int screenHeight = Screen.FromPoint(p).WorkingArea.Bottom - p.Y;
            screenHeight -= frm.Size.Height - frm.ClientSize.Height;
            int requiredHeight = childList.TotalRowHeight + 4;
            int formHeight = Math.Min(requiredHeight, Math.Min(300,screenHeight));
            if (formHeight < requiredHeight && formHeight < 100)
            {
                int missingHeight = Math.Min(200, requiredHeight - formHeight);
                formHeight += missingHeight;
                frm.Top -= missingHeight;
            }
            // Autosize child window
            int formWidth;
            using (Graphics g = childList.CreateGraphics())
            {
                formWidth = 8 + childList.GetRequiredWidth(g);
            }
            int screenWidth = Screen.FromPoint(p).WorkingArea.Right - p.X;
            if (formWidth > screenWidth)
            {
                int missingWidth = Math.Min(100, formWidth - screenWidth);
                formWidth = screenWidth + missingWidth;
                frm.Left -= missingWidth;
            }
            frm.ClientSize = new Size(formWidth, formHeight);
            frm.MinimumSize = new Size(100, Math.Min(50, formHeight));
            isOpeningChild = true;
            frm.Show();
            isOpeningChild = false;
            childList.Focus();
            if (expandedIn != null)
                expandedIn.Add(e.List);
            OnExpanded(e);
            plus.RaiseItemChanged();
        }

        public class ChildForm : Form, IActivatable
        {
            bool isActivated = true;

            public bool IsActivated
            {
                get
                {
                    return isActivated;
                }
            }

            bool allowResizing = true;

            public bool AllowResizing
            {
                get
                {
                    return allowResizing;
                }
                set
                {
                    if (allowResizing == value)
                        return;
                    allowResizing = value;
                    this.DockPadding.All = value ? 2 : 1;
                }
            }

            public ChildForm()
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.DockPadding.All = 1;
                this.BackColor = DefaultBorderColor;
            }

            bool showWindowWithoutActivation;

            /// <summary>
            /// Gets/Sets whether the window will receive focus when it is shown.
            /// </summary>
            public bool ShowWindowWithoutActivation
            {
                get
                {
                    return showWindowWithoutActivation;
                }
                set
                {
                    showWindowWithoutActivation = value;
                }
            }

            protected override bool ShowWithoutActivation
            {
                get
                {
                    return showWindowWithoutActivation;
                }
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams p = base.CreateParams;
                    DesignHelper.AddShadowToWindow(p);
                    return p;
                }
            }

            #region Resizing the form
            private enum MousePositionCodes
            {
                HTERROR = (-2),
                HTTRANSPARENT = (-1),
                HTNOWHERE = 0,
                HTCLIENT = 1,
                HTCAPTION = 2,
                HTSYSMENU = 3,
                HTGROWBOX = 4,
                HTSIZE = HTGROWBOX,
                HTMENU = 5,
                HTHSCROLL = 6,
                HTVSCROLL = 7,
                HTMINBUTTON = 8,
                HTMAXBUTTON = 9,
                HTLEFT = 10,
                HTRIGHT = 11,
                HTTOP = 12,
                HTTOPLEFT = 13,
                HTTOPRIGHT = 14,
                HTBOTTOM = 15,
                HTBOTTOMLEFT = 16,
                HTBOTTOMRIGHT = 17,
                HTBORDER = 18,
                HTREDUCE = HTMINBUTTON,
                HTZOOM = HTMAXBUTTON,
                HTSIZEFIRST = HTLEFT,
                HTSIZELAST = HTBOTTOMRIGHT,
                HTOBJECT = 19,
                HTCLOSE = 20,
                HTHELP = 21
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (m.Msg == 0x0084) // WM_NCHITTEST
                    HitTest(ref m);
            }

            void HitTest(ref Message m)
            {
                if (!allowResizing)
                    return;
                int mousePos = m.LParam.ToInt32();
                int mouseX = mousePos & 0xffff;
                int mouseY = mousePos >> 16;
                //System.Diagnostics.Debug.WriteLine(mouseX + " / " + mouseY);
                Rectangle bounds = Bounds;
                bool isLeft = mouseX == bounds.Left || mouseX + 1 == bounds.Left;
                bool isTop = mouseY == bounds.Top || mouseY + 1 == bounds.Top;
                bool isRight = mouseX == bounds.Right - 1 || mouseX == bounds.Right - 2;
                bool isBottom = mouseY == bounds.Bottom - 1 || mouseY == bounds.Bottom - 2;
                if (isLeft)
                {
                    if (isTop)
                        m.Result = new IntPtr((int)MousePositionCodes.HTTOPLEFT);
                    else if (isBottom)
                        m.Result = new IntPtr((int)MousePositionCodes.HTBOTTOMLEFT);
                    else
                        m.Result = new IntPtr((int)MousePositionCodes.HTLEFT);
                }
                else if (isRight)
                {
                    if (isTop)
                        m.Result = new IntPtr((int)MousePositionCodes.HTTOPRIGHT);
                    else if (isBottom)
                        m.Result = new IntPtr((int)MousePositionCodes.HTBOTTOMRIGHT);
                    else
                        m.Result = new IntPtr((int)MousePositionCodes.HTRIGHT);
                }
                else if (isTop)
                {
                    m.Result = new IntPtr((int)MousePositionCodes.HTTOP);
                }
                else if (isBottom)
                {
                    m.Result = new IntPtr((int)MousePositionCodes.HTBOTTOM);
                }
            }
            #endregion

            protected override void OnActivated(EventArgs e)
            {
                isActivated = true;
                base.OnActivated(e);
                Refresh();
            }

            protected override void OnDeactivate(EventArgs e)
            {
                isActivated = false;
                base.OnDeactivate(e);
                if (isOpeningChild)
                {
                    Refresh();
                    return;
                }
                BeginInvoke(new MethodInvoker(CloseOnDeactivate));
            }

            void CloseOnDeactivate()
            {
                return;
                ChildForm owner = Owner as ChildForm;
                if (owner != null)
                {
                    if (owner.isActivated)
                        Close();
                    else
                        owner.CloseOnDeactivate();
                }
                else
                {
                    Close();
                }
            }
        }
        #endregion
    }

    public static class DesignHelper
    {
        static int shadowStatus;

        /// <summary>
        /// Adds a shadow to the create params if it is supported by the operating system.
        /// </summary>
        public static void AddShadowToWindow(CreateParams createParams)
        {
            if (shadowStatus == 0)
            {
                // Test OS version
                shadowStatus = -1; // shadow not supported
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    Version ver = Environment.OSVersion.Version;
                    if (ver.Major > 5 || ver.Major == 5 && ver.Minor >= 1)
                    {
                        shadowStatus = 1;
                    }
                }
            }
            if (shadowStatus == 1)
            {
                createParams.ClassStyle |= 0x00020000; // set CS_DROPSHADOW
            }
        }
    }
    public class DynamicListRow
    {
        int height = Convert.ToInt32(16 * ScreenScale.Calc());

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                if (value < 2)
                    throw new ArgumentOutOfRangeException("value", value, "value must be at least 2");
                if (height != value)
                {
                    height = value;
                    OnHeightChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler HeightChanged;

        protected virtual void OnHeightChanged(EventArgs e)
        {
            if (HeightChanged != null)
            {
                HeightChanged(this, e);
            }
        }

        public event EventHandler<DynamicListEventArgs> Shown;
        public event EventHandler<DynamicListEventArgs> Hidden;

        protected virtual void OnShown(DynamicListEventArgs e)
        {
            if (Shown != null)
            {
                Shown(this, e);
            }
        }

        protected virtual void OnHidden(DynamicListEventArgs e)
        {
            if (Hidden != null)
            {
                Hidden(this, e);
            }
        }

        internal void NotifyListVisibilityChange(DynamicList list, bool visible)
        {
            if (visible)
                OnShown(new DynamicListEventArgs(list));
            else
                OnHidden(new DynamicListEventArgs(list));
        }

        /// <summary>
        /// Fired when any item has changed.
        /// </summary>
        public event EventHandler ItemChanged;

        protected virtual void OnItemChanged(EventArgs e)
        {
            if (ItemChanged != null)
            {
                ItemChanged(this, e);
            }
        }

        internal void RaiseItemChanged(DynamicListItem item)
        {
            OnItemChanged(EventArgs.Empty);
        }

        DynamicListItem[] items = new DynamicListItem[10];

        public DynamicListItem this[int columnIndex]
        {
            [DebuggerStepThrough]
            get
            {
                if (columnIndex < 0)
                    throw new ArgumentOutOfRangeException("columnIndex", columnIndex, "columnIndex must be >= 0");
                if (columnIndex > DynamicList.MaxColumnCount)
                    throw new ArgumentOutOfRangeException("columnIndex", columnIndex, "columnIndex must be <= " + DynamicList.MaxColumnCount);
                if (columnIndex >= items.Length)
                {
                    Array.Resize(ref items, columnIndex * 2 + 1);
                }
                DynamicListItem item = items[columnIndex];
                if (item == null)
                {
                    items[columnIndex] = item = CreateItem();
                }
                return item;
            }
        }

        protected virtual DynamicListItem CreateItem()
        {
            return new DynamicListItem(this);
        }
    }

    public class DynamicList : UserControl, VerticalScrollContainer.IScrollable
    {
        public const int MaxColumnCount = 1000;
		
        public IDocument doc;
        
        public DynamicList()
            : this(new CollectionWithEvents<DynamicListColumn>(), new CollectionWithEvents<DynamicListRow>())
        {
        }

        public DynamicList(CollectionWithEvents<DynamicListColumn> columns, CollectionWithEvents<DynamicListRow> rows)
        {
            inUpdate = true;
            if (columns == null)
                throw new ArgumentNullException("columns");
            if (rows == null)
                throw new ArgumentNullException("rows");
            this.columns = columns;
            this.rows = rows;
            // we have to register our events on the existing items
            foreach (DynamicListColumn column in columns)
            {
                OnColumnAdded(null, new CollectionItemEventArgs<DynamicListColumn>(column));
            }
            foreach (DynamicListRow row in rows)
            {
                OnRowAdded(null, new CollectionItemEventArgs<DynamicListRow>(row));
            }
            columns.Added += OnColumnAdded;
            columns.Removed += OnColumnRemoved;
            rows.Added += OnRowAdded;
            rows.Removed += OnRowRemoved;
            this.BackColor = DefaultBackColor;
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            inUpdate = false;
            RecalculateColumnWidths();
        }

        public new static readonly Color DefaultBackColor = Color.White;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                columns.Added -= OnColumnAdded;
                columns.Removed -= OnColumnRemoved;
                rows.Added -= OnRowAdded;
                rows.Removed -= OnRowRemoved;
                foreach (DynamicListColumn column in columns)
                {
                    OnColumnRemoved(null, new CollectionItemEventArgs<DynamicListColumn>(column));
                }
                foreach (DynamicListRow row in rows)
                {
                    OnRowRemoved(null, new CollectionItemEventArgs<DynamicListRow>(row));
                }
                WorkbenchServiceFactory.DebuggerManager.RemoveMarker(doc);
            }
        }

        void OnColumnAdded(object sender, CollectionItemEventArgs<DynamicListColumn> e)
        {
            e.Item.MinimumWidthChanged += ColumnMinimumWidthChanged;
            e.Item.WidthChanged += ColumnWidthChanged;
            RecalculateColumnWidths();
        }

        void OnColumnRemoved(object sender, CollectionItemEventArgs<DynamicListColumn> e)
        {
            e.Item.MinimumWidthChanged -= ColumnMinimumWidthChanged;
            e.Item.WidthChanged -= ColumnWidthChanged;
            RecalculateColumnWidths();
        }

        void OnRowAdded(object sender, CollectionItemEventArgs<DynamicListRow> e)
        {
            e.Item.HeightChanged += RowHeightChanged;
            e.Item.ItemChanged += RowItemChanged;
            if (Visible && Parent != null)
                e.Item.NotifyListVisibilityChange(this, true);
        }

        void OnRowRemoved(object sender, CollectionItemEventArgs<DynamicListRow> e)
        {
            e.Item.HeightChanged -= RowHeightChanged;
            e.Item.ItemChanged -= RowItemChanged;
            if (Visible)
                e.Item.NotifyListVisibilityChange(this, false);
        }

        bool oldVisible = false;

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            bool visible = Visible && Parent != null;
            if (visible == oldVisible)
                return;
            oldVisible = visible;
            foreach (DynamicListRow row in Rows)
            {
                row.NotifyListVisibilityChange(this, visible);
            }
        }

        void ColumnMinimumWidthChanged(object sender, EventArgs e)
        {
            RecalculateColumnWidths();
        }

        void RowHeightChanged(object sender, EventArgs e)
        {
            Redraw();
        }

        void RowItemChanged(object sender, EventArgs e)
        {
            Redraw();
        }

        bool inRecalculateColumnWidths;
        bool inRecalculateNeedsRedraw;

        void RecalculateColumnWidths()
        {
            if (inUpdate) return;
            if (inRecalculateColumnWidths) return;
            inRecalculateColumnWidths = true;
            inRecalculateNeedsRedraw = false;
            try
            {
                int availableWidth = ClientSize.Width;
                int minRequiredWidth = 0;
                foreach (DynamicListColumn c in columns)
                {
                    if (c.AllowGrow)
                        minRequiredWidth += c.MinimumWidth;
                    else
                        availableWidth -= c.Width;
                    availableWidth -= 1;
                }
                // everyone gets c.MinimumWidth * availableWidth / minRequiredWidth
                foreach (DynamicListColumn c in columns)
                {
                    if (c.AllowGrow)
                        c.Width = Math.Max(2, c.MinimumWidth * availableWidth / minRequiredWidth);
                }
            }
            finally
            {
                inRecalculateColumnWidths = false;
            }
            if (inRecalculateNeedsRedraw)
            {
                Redraw();
            }
        }

        void ColumnWidthChanged(object sender, EventArgs e)
        {
            if (inRecalculateColumnWidths)
            {
                inRecalculateNeedsRedraw = true;
                return;
            }
            Redraw();
        }

        bool inUpdate;

        public void BeginUpdate()
        {
            inUpdate = true;
        }

        public void EndUpdate()
        {
            inUpdate = false;
            RecalculateColumnWidths();
        }

        void Redraw()
        {
            if (inUpdate) return;
            Invalidate();
        }

        List<Control> allowedControls = new List<Control>();
        List<Control> removedControls = new List<Control>();
        int scrollOffset = 0;

        public int ScrollOffset
        {
            get
            {
                return scrollOffset;
            }
            set
            {
                if (scrollOffset != value)
                {
                    scrollOffset = value;
                    Redraw();
                }
            }
        }

        int VerticalScrollContainer.IScrollable.ScrollOffsetY
        {
            get
            {
                return this.ScrollOffset;
            }
            set
            {
                this.ScrollOffset = value;
            }
        }

        int VerticalScrollContainer.IScrollable.ScrollHeightY
        {
            get
            {
                return this.TotalRowHeight;
            }
        }

        public int GetRequiredWidth(Graphics graphics)
        {
            int width = 0;
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].AutoSize)
                {
                    int minimumWidth = DynamicListColumn.DefaultWidth;
                    foreach (DynamicListRow row in Rows)
                    {
                        DynamicListItem item = row[i];
                        item.MeasureMinimumWidth(graphics, ref minimumWidth);
                    }
                    width += minimumWidth;
                }
                else
                {
                    width += columns[i].Width;
                }
                width += 1;
            }
            return width;
        }

        int lineMarginY = 0;

        public int LineMarginY
        {
            get
            {
                return lineMarginY;
            }
            set
            {
                if (lineMarginY == value)
                    return;
                lineMarginY = value;
                Redraw();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Debug.WriteLine("OnPaint");
            Graphics g = e.Graphics;
            allowedControls.Clear();

            int columnIndex = -1;
            foreach (DynamicListColumn col in Columns)
            {
                columnIndex += 1;
                if (!col.AutoSize)
                    continue;
                int minimumWidth = DynamicListColumn.DefaultWidth;
                foreach (DynamicListRow row in Rows)
                {
                    DynamicListItem item = row[columnIndex];
                    item.MeasureMinimumWidth(e.Graphics, ref minimumWidth);
                }
                col.MinimumWidth = minimumWidth;
            }

            int controlIndex = 0;
            int xPos;
            int yPos = -scrollOffset;
            Size clientSize = ClientSize;
            foreach (DynamicListRow row in Rows)
            {
                if (yPos + row.Height > 0 && yPos < clientSize.Height)
                {
                    xPos = 0;
                    for (columnIndex = 0; columnIndex < columns.Count; columnIndex++)
                    {
                        DynamicListColumn col = columns[columnIndex];
                        Rectangle rect = new Rectangle(xPos, yPos, col.Width, row.Height);
                        if (columnIndex == columns.Count - 1)
                            rect.Width = clientSize.Width - 1 - rect.Left;
                        DynamicListItem item = row[columnIndex];
                        Control ctl = item.Control;
                        if (ctl != null)
                        {
                            allowedControls.Add(ctl);
                            if (rect != ctl.Bounds)
                                ctl.Bounds = rect;
                            if (!this.Controls.Contains(ctl))
                            {
                                this.Controls.Add(ctl);
                                this.Controls.SetChildIndex(ctl, controlIndex);
                            }
                            controlIndex += 1;
                        }
                        else
                        {
                            item.PaintTo(e.Graphics, rect, this, col, item == itemAtMousePosition);
                        }
                        xPos += col.Width + 1;
                    }
                }
                yPos += row.Height + lineMarginY;
            }
            xPos = 0;
            Form containerForm = FindForm();
            bool isFocused;
            if (containerForm is IActivatable)
                isFocused = (containerForm as IActivatable).IsActivated;
            else
                isFocused = this.Focused;
            for (columnIndex = 0; columnIndex < columns.Count - 1; columnIndex++)
            {
                DynamicListColumn col = columns[columnIndex];

                xPos += col.Width + 1;

                Color separatorColor =Color.Empty;
                //if (isFocused)
                if (columnIndex == columns.Count-2)
                {
                    separatorColor = Color.LightGray;//col.ColumnSeperatorColor;
                    if (separatorColor.IsEmpty)
                        separatorColor = col.ColumnSeperatorColorInactive;
                }
                /*else
                {
                    separatorColor = col.ColumnSeperatorColorInactive;
                    if (separatorColor.IsEmpty)
                        separatorColor = col.ColumnSeperatorColor;
                }*/
                if (separatorColor.IsEmpty) separatorColor = BackColor;
                using (Pen separatorPen = new Pen(separatorColor))
                {
                    e.Graphics.DrawLine(separatorPen, xPos - 1, 1, xPos - 1, Math.Min(clientSize.Height, yPos) - 2);
                }
            }
            removedControls.Clear();
            foreach (Control ctl in Controls)
            {
                if (!allowedControls.Contains(ctl))
                    removedControls.Add(ctl);
            }
            foreach (Control ctl in removedControls)
            {
                Debug.WriteLine("Removing control");
                Controls.Remove(ctl);
                Debug.WriteLine("Control removed");
            }
            allowedControls.Clear();
            removedControls.Clear();
            base.OnPaint(e);
        }

        /// <summary>
        /// Gets if the parent form of this list is the active window.
        /// </summary>
        public bool IsActivated
        {
            get
            {
                Form containerForm = FindForm();
                if (containerForm is IActivatable)
                    return (containerForm as IActivatable).IsActivated;
                else
                    return this.Focused;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RecalculateColumnWidths();
        }

        public DynamicListRow GetRowFromPoint(int yPos)
        {
            int y = -scrollOffset;
            foreach (DynamicListRow row in Rows)
            {
                if (yPos < y)
                    break;
                if (yPos <= y + row.Height)
                    return row;
                y += row.Height + lineMarginY;
            }
            return null;
        }

        /// <summary>
        /// Gets the upper left corner of the specified row.
        /// </summary>
        public Point GetPositionFromRow(DynamicListRow row)
        {
            int y = -scrollOffset;
            foreach (DynamicListRow r in Rows)
            {
                if (r == row)
                    return new Point(0, y);
                y += r.Height + lineMarginY;
            }
            throw new ArgumentException("The row in not in this list!");
        }

        /// <summary>
        /// Gets the height of all rows.
        /// </summary>
        public int TotalRowHeight
        {
            get
            {
                int y = 0;
                foreach (DynamicListRow r in Rows)
                {
                    y += r.Height + lineMarginY;
                }
                return y;
            }
        }

        public int GetColumnIndexFromPoint(int xPos)
        {
            int columnIndex = 0;
            int x = 0;
            foreach (DynamicListColumn col in Columns)
            {
                if (xPos < x)
                    break;
                if (xPos <= x + col.Width)
                    return columnIndex;
                x += col.Width + 1;
                columnIndex += 1;
            }
            return -1;
        }

        public DynamicListItem GetItemFromPoint(Point position)
        {
            DynamicListRow row = GetRowFromPoint(position.Y);
            if (row == null)
                return null;
            int columnIndex = GetColumnIndexFromPoint(position.X);
            if (columnIndex < 0)
                return null;
            return row[columnIndex];
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            DynamicListItem item = GetItemFromPoint(PointToClient(Control.MousePosition));
            if (item != null) item.PerformClick(this);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            DynamicListItem item = GetItemFromPoint(PointToClient(Control.MousePosition));
            if (item != null) item.PerformDoubleClick(this);
        }

        protected override void OnMouseHover(EventArgs e) 
        {
            base.OnMouseHover(e);
            DynamicListItem item = GetItemFromPoint(PointToClient(Control.MousePosition));
            if (item != null) 
                item.OnMouseHover(this);
        }

        DynamicListItem itemAtMousePosition;
        DynamicListRow rowAtMousePosition;

        public DynamicListItem ItemAtMousePosition
        {
            get
            {
                return itemAtMousePosition;
            }
        }

        public DynamicListRow RowAtMousePosition
        {
            get
            {
                return rowAtMousePosition;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            DynamicListRow row = GetRowFromPoint(e.Y);
            if (rowAtMousePosition != row)
            {
                rowAtMousePosition = row;
                Invalidate();
            }
            if (row == null)
                return;
            int columnIndex = GetColumnIndexFromPoint(e.X);
            if (columnIndex < 0)
                return;
            DynamicListItem item = row[columnIndex];
            if (itemAtMousePosition != item)
            {
                if (itemAtMousePosition != null)
                {
                    OnLeaveItem(itemAtMousePosition);
                }
                ResetMouseEventArgs(); // raise hover again
                itemAtMousePosition = item;
                if (item != null)
                {
                    if (item.Cursor != null)
                        this.Cursor = item.Cursor;
                    item.OnMouseEnter(this);
                }
            }
            if (item != null)
            {
                item.OnMouseMove(new DynamicListMouseEventArgs(this, e));
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            DynamicListItem item = GetItemFromPoint(e.Location);
            if (item != null) item.OnMouseDown(new DynamicListMouseEventArgs(this, e));
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            DynamicListItem item = GetItemFromPoint(e.Location);
            if (item != null) item.OnMouseUp(new DynamicListMouseEventArgs(this, e));
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            rowAtMousePosition = null;
            if (itemAtMousePosition != null)
            {
                OnLeaveItem(itemAtMousePosition);
                itemAtMousePosition = null;
            }
            base.OnMouseLeave(e);
        }

        protected virtual void OnLeaveItem(DynamicListItem item)
        {
            itemAtMousePosition.OnMouseLeave(this);
            this.Cursor = Cursors.Default;
        }

        readonly CollectionWithEvents<DynamicListColumn> columns;
        readonly CollectionWithEvents<DynamicListRow> rows;

        public CollectionWithEvents<DynamicListColumn> Columns
        {
            [DebuggerStepThrough]
            get
            {
                return columns;
            }
        }

        [Browsable(false)]
        public CollectionWithEvents<DynamicListRow> Rows
        {
            [DebuggerStepThrough]
            get
            {
                return rows;
            }
        }
    }

    public interface IActivatable
    {
        bool IsActivated { get; }
        event EventHandler Activated;
        event EventHandler Deactivate;
    }

    public sealed class DynamicListItem
    {
        DynamicListRow row;

        internal DynamicListItem(DynamicListRow row)
        {
            this.row = row;
        }

        public void RaiseItemChanged()
        {
            row.RaiseItemChanged(this);
        }

        Cursor cursor;

        public DynamicListRow Row
        {
            get
            {
                return row;
            }
        }

        public Cursor Cursor
        {
            get
            {
                return cursor;
            }
            set
            {
                cursor = value;
            }
        }

        #region BackgroundBrush / Control
        Brush backgroundBrush;

        public Brush BackgroundBrush
        {
            get
            {
                return backgroundBrush;
            }
            set
            {
                if (backgroundBrush != value)
                {
                    backgroundBrush = value;
                    RaiseItemChanged();
                }
            }
        }

        Brush backgroundBrushInactive;

        public Brush BackgroundBrushInactive
        {
            get
            {
                return backgroundBrushInactive;
            }
            set
            {
                if (backgroundBrushInactive != value)
                {
                    backgroundBrushInactive = value;
                    RaiseItemChanged();
                }
            }
        }

        Brush highlightBrush;

        public Brush HighlightBrush
        {
            get
            {
                return highlightBrush;
            }
            set
            {
                if (highlightBrush != value)
                {
                    highlightBrush = value;
                    RaiseItemChanged();
                }
            }
        }

        Control control;

        public Control Control
        {
            get
            {
                return control;
            }
            set
            {
                if (control != value)
                {
                    control = value;
                    RaiseItemChanged();
                }
            }
        }
        #endregion

        #region MeasureWidth / Paint
        public event EventHandler<MeasureWidthEventArgs> MeasureWidth;

        internal void MeasureMinimumWidth(Graphics graphics, ref int minimumWidth)
        {
            if (MeasureWidth != null)
            {
                MeasureWidthEventArgs e = new MeasureWidthEventArgs(graphics);
                MeasureWidth(this, e);
                minimumWidth = Math.Max(minimumWidth, e.ItemWidth);
            }
            if (text.Length > 0)
            {
                // Prevent GDI exception (forum-12284) when text is very long
                if (text.Length > short.MaxValue)
                {
                    text = text.Substring(0, short.MaxValue - 1);
                }
                int width = 2 + (int)graphics.MeasureString(text, font, new PointF(0, 0), textFormat).Width;
                minimumWidth = Math.Max(minimumWidth, width);
            }
        }

        public event EventHandler<ItemPaintEventArgs> Paint;

        internal void PaintTo(Graphics g, Rectangle rectangle, DynamicList list, DynamicListColumn column, bool isMouseEntered)
        {
            Rectangle fillRectangle = rectangle;
            fillRectangle.Width += 1;
            if (highlightBrush != null && isMouseEntered)
            {
                g.FillRectangle(highlightBrush, fillRectangle);
            }
            else
            {
                bool isActivated = list.IsActivated;
                Brush bgBrush = GetBrush(isActivated, backgroundBrush, backgroundBrushInactive);
                if (bgBrush == null)
                {
                    bgBrush = GetBrush(isActivated, column.BackgroundBrush, column.BackgroundBrushInactive);
                    if (isActivated && list.RowAtMousePosition == row && column.RowHighlightBrush != null)
                        bgBrush = column.RowHighlightBrush;
                }
                g.FillRectangle(bgBrush, fillRectangle);
            }
            if (Paint != null)
            {
                Paint(this, new ItemPaintEventArgs(g, rectangle, fillRectangle, list, column, this, isMouseEntered));
            }
            if (text.Length > 0)
            {
                // SSM 21.01.19 High DPI
                textFormat.LineAlignment = StringAlignment.Center;
                g.DrawString(text, font, textBrush, rectangle, textFormat);
            }
        }

        Brush GetBrush(bool isActive, Brush activeBrush, Brush inactiveBrush)
        {
            return isActive ? (activeBrush ?? inactiveBrush) : (inactiveBrush ?? activeBrush);
        }
        #endregion

        #region Text drawing
        string text = string.Empty;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (value == null)
                    value = "";
                if (text != value)
                {
                    text = value;
                    RaiseItemChanged();
                }
            }
        }

        Font font = Control.DefaultFont;

        public Font Font
        {
            get
            {
                return font;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (font != value)
                {
                    font = value;
                    RaiseItemChanged();
                }
            }
        }

        Brush textBrush = SystemBrushes.ControlText;

        public Brush TextBrush
        {
            get
            {
                return textBrush;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (textBrush != value)
                {
                    textBrush = value;
                    RaiseItemChanged();
                }
            }
        }

        static StringFormat defaultTextFormat;

        public static StringFormat DefaultTextFormat
        {
            get
            {
                if (defaultTextFormat == null)
                {
                    defaultTextFormat = (StringFormat)StringFormat.GenericDefault.Clone();
                    defaultTextFormat.FormatFlags |= StringFormatFlags.NoWrap;
                }
                return defaultTextFormat;
            }
        }

        StringFormat textFormat = DefaultTextFormat;

        public StringFormat TextFormat
        {
            get
            {
                return textFormat;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (textFormat != value)
                {
                    textFormat = value;
                    RaiseItemChanged();
                }
            }
        }
        #endregion

        #region Mouse Events
        public event EventHandler<DynamicListEventArgs> Click;

        public void PerformClick(DynamicList list)
        {
            if (Click != null)
                Click(this, new DynamicListEventArgs(list));
            HandleLabelEditClick(list);
        }

        public event EventHandler<DynamicListEventArgs> DoubleClick;

        public void PerformDoubleClick(DynamicList list)
        {
            if (DoubleClick != null)
                DoubleClick(this, new DynamicListEventArgs(list));
        }

        public event EventHandler<DynamicListEventArgs> MouseHover;

        internal void OnMouseHover(DynamicList list)
        {
            if (MouseHover != null)
            {
                MouseHover(this, new DynamicListEventArgs(list));
            }
        }

        public event EventHandler<DynamicListEventArgs> MouseLeave;

        internal void OnMouseLeave(DynamicList list)
        {
            if (MouseLeave != null)
            {
                MouseLeave(this, new DynamicListEventArgs(list));
            }
            if (highlightBrush != null) RaiseItemChanged();
        }

        public event EventHandler<DynamicListEventArgs> MouseEnter;

        internal void OnMouseEnter(DynamicList list)
        {
            if (MouseEnter != null)
            {
                MouseEnter(this, new DynamicListEventArgs(list));
            }
            if (highlightBrush != null) RaiseItemChanged();
        }

        public event EventHandler<DynamicListMouseEventArgs> MouseMove;

        internal void OnMouseMove(DynamicListMouseEventArgs e)
        {
            if (MouseMove != null)
            {
                MouseMove(this, e);
            }
        }

        public event EventHandler<DynamicListMouseEventArgs> MouseDown;

        internal void OnMouseDown(DynamicListMouseEventArgs e)
        {
            if (MouseDown != null)
            {
                MouseDown(this, e);
            }
        }

        public event EventHandler<DynamicListMouseEventArgs> MouseUp;

        internal void OnMouseUp(DynamicListMouseEventArgs e)
        {
            if (MouseUp != null)
            {
                MouseUp(this, e);
            }
        }
        #endregion

        #region Label editing
        bool allowLabelEdit;

        public bool AllowLabelEdit
        {
            get
            {
                return allowLabelEdit;
            }
            set
            {
                allowLabelEdit = value;
            }
        }

        public event EventHandler<DynamicListEventArgs> BeginLabelEdit;
        public event EventHandler<DynamicListEventArgs> FinishLabelEdit;
        public event EventHandler<DynamicListEventArgs> CanceledLabelEdit;

        void HandleLabelEditClick(DynamicList list)
        {
            if (!allowLabelEdit)
                return;
            TextBox txt = new TextBox();
            txt.Text = this.Text;
            AssignControlUntilFocusChange(txt);
            if (BeginLabelEdit != null)
                BeginLabelEdit(this, new DynamicListEventArgs(list));
            bool escape = false;
            txt.KeyDown += delegate(object sender2, KeyEventArgs e2)
            {
                if (e2.KeyData == Keys.Enter || e2.KeyData == Keys.Escape)
                {
                    e2.Handled = true;
                    if (e2.KeyData == Keys.Escape)
                    {
                        if (CanceledLabelEdit != null)
                            CanceledLabelEdit(this, new DynamicListEventArgs(list));
                        escape = true;
                    }
                    this.Control = null;
                    txt.Dispose();
                }
            };
            txt.LostFocus += delegate
            {
                if (!escape)
                {
                    this.Text = txt.Text;
                    if (FinishLabelEdit != null)
                        FinishLabelEdit(this, new DynamicListEventArgs(list));
                }
            };
        }

        /// <summary>
        /// Display the control for this item. Automatically assign focus to the control
        /// and removes+disposes the control when it looses focus.
        /// </summary>
        public void AssignControlUntilFocusChange(Control control)
        {
            MethodInvoker method = delegate
            {
                if (!control.Focus())
                {
                    control.Focus();
                }
                control.LostFocus += delegate
                {
                    this.Control = null;
                    control.Dispose();
                };
            };

            control.HandleCreated += delegate
            {
                control.BeginInvoke(method);
            };
            this.Control = control;
        }
        #endregion
    }

    public class DynamicListEventArgs : EventArgs
    {
        DynamicList list;

        public DynamicList List
        {
            [DebuggerStepThrough]
            get
            {
                return list;
            }
        }

        public DynamicListEventArgs(DynamicList list)
        {
            if (list == null) throw new ArgumentNullException("list");
            this.list = list;
        }
    }

    public class DynamicListMouseEventArgs : MouseEventArgs
    {
        DynamicList list;

        public DynamicList List
        {
            [DebuggerStepThrough]
            get
            {
                return list;
            }
        }

        public DynamicListMouseEventArgs(DynamicList list, MouseEventArgs me)
            : base(me.Button, me.Clicks, me.X, me.Y, me.Delta)
        {
            if (list == null) throw new ArgumentNullException("list");
            this.list = list;
        }
    }

    public class MeasureWidthEventArgs : EventArgs
    {
        Graphics graphics;

        public Graphics Graphics
        {
            get
            {
                return graphics;
            }
        }

        int itemWidth;

        public int ItemWidth
        {
            get
            {
                return itemWidth;
            }
            set
            {
                itemWidth = value;
            }
        }

        public MeasureWidthEventArgs(Graphics graphics)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");
            this.graphics = graphics;
        }
    }

    public class ItemPaintEventArgs : PaintEventArgs
    {
        DynamicList list;
        DynamicListColumn column;
        DynamicListItem item;
        bool isMouseEntered;
        Rectangle fillRectangle;

        public Rectangle FillRectangle
        {
            get
            {
                return fillRectangle;
            }
        }

        public DynamicList List
        {
            get
            {
                return list;
            }
        }

        public DynamicListColumn Column
        {
            get
            {
                return column;
            }
        }

        public DynamicListRow Row
        {
            get
            {
                return item.Row;
            }
        }

        public DynamicListItem Item
        {
            get
            {
                return item;
            }
        }

        public bool IsMouseEntered
        {
            get
            {
                return isMouseEntered;
            }
        }

        public ItemPaintEventArgs(Graphics graphics, Rectangle rectangle, Rectangle fillRectangle,
                                  DynamicList list, DynamicListColumn column,
                                  DynamicListItem item, bool isMouseEntered)
            : base(graphics, rectangle)
        {
            this.fillRectangle = fillRectangle;
            this.list = list;
            this.column = column;
            this.item = item;
            this.isMouseEntered = isMouseEntered;
        }
    }

    public class DynamicListColumn : ICloneable
    {
        // SSM 21.01.19 High DPI
        public static int DefaultWidth = Convert.ToInt32(16 * ScreenScale.Calc());

        int width = DefaultWidth;
        int minimumWidth = DefaultWidth;
        bool allowGrow = true;
        bool autoSize = false;

        //public static readonly Color DefaultBackColor = Color.FromArgb(247, 245, 233);
        public static readonly Color DefaultBackColor = SystemColors.Window;
        public static readonly Brush DefaultBackBrush = new SolidBrush(DefaultBackColor);
        public static readonly Color DefaultRowHighlightBackColor = Color.FromArgb(221, 218, 203);
        //public static readonly Color DefaultRowHighlightBackColor = SystemColors.Highlight;
        public static readonly Brush DefaultRowHighlightBrush = new SolidBrush(DefaultRowHighlightBackColor);
        //public static readonly Color DefaultInactiveBackColor = Color.FromArgb(242, 240, 228);
        public static readonly Color DefaultInactiveBackColor = SystemColors.Window;
        public static readonly Brush DefaultInactiveBackBrush = new SolidBrush(DefaultInactiveBackColor);

        Brush backgroundBrush = DefaultBackBrush;
        Brush backgroundBrushInactive = DefaultInactiveBackBrush;
        Brush rowHighlightBrush = DefaultRowHighlightBrush;

        public virtual DynamicListColumn Clone()
        {
            return (DynamicListColumn)base.MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #region Properties
        public int MinimumWidth
        {
            get
            {
                return minimumWidth;
            }
            set
            {
                if (value < 2)
                    throw new ArgumentOutOfRangeException("value", value, "MinimumWidth must be at least 2");
                if (minimumWidth != value)
                {
                    minimumWidth = value;
                    if (MinimumWidthChanged != null)
                    {
                        MinimumWidthChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler MinimumWidthChanged;

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                if (value < 2)
                    throw new ArgumentOutOfRangeException("value", value, "Width must be at least 2");
                if (width != value)
                {
                    width = value;
                    if (WidthChanged != null)
                    {
                        WidthChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler WidthChanged;

        public bool AllowGrow
        {
            get
            {
                return allowGrow;
            }
            set
            {
                allowGrow = value;
            }
        }

        public bool AutoSize
        {
            get
            {
                return autoSize;
            }
            set
            {
                autoSize = value;
            }
        }

        public Brush BackgroundBrush
        {
            get
            {
                return backgroundBrush;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                backgroundBrush = value;
            }
        }

        public Brush BackgroundBrushInactive
        {
            get
            {
                return backgroundBrushInactive;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                backgroundBrushInactive = value;
            }
        }

        public Brush RowHighlightBrush
        {
            get
            {
                return rowHighlightBrush;
            }
            set
            {
                rowHighlightBrush = value;
            }
        }

        Color columnSeperatorColor = Color.Empty;

        /// <summary>
        /// Sets the color that is used to the right of this column as separator color.
        /// </summary>
        public Color ColumnSeperatorColor
        {
            get
            {
                return columnSeperatorColor;
            }
            set
            {
                columnSeperatorColor = value;
            }
        }

        Color columnSeperatorColorInactive = Color.Empty;

        /// <summary>
        /// Sets the color that is used to the right of this column as separator color.
        /// </summary>
        public Color ColumnSeperatorColorInactive
        {
            get
            {
                return columnSeperatorColorInactive;
            }
            set
            {
                columnSeperatorColorInactive = value;
            }
        }
        #endregion
    }

    public sealed class CollectionWithEvents<T> : IList<T>
    {
        List<T> list = new List<T>();
        public event EventHandler<CollectionItemEventArgs<T>> Added;
        public event EventHandler<CollectionItemEventArgs<T>> Removed;

        void OnAdded(T item)
        {
            if (Added != null)
                Added(this, new CollectionItemEventArgs<T>(item));
        }
        void OnRemoved(T item)
        {
            if (Removed != null)
                Removed(this, new CollectionItemEventArgs<T>(item));
        }

        public T this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                T oldValue = list[index];
                if (!object.Equals(oldValue, value))
                {
                    list[index] = value;
                    OnRemoved(oldValue);
                    OnAdded(value);
                }
            }
        }

        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                return list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
            OnAdded(item);
        }

        public void RemoveAt(int index)
        {
            T item = list[index];
            list.RemoveAt(index);
            OnRemoved(item);
        }

        public void Add(T item)
        {
            list.Add(item);
            OnAdded(item);
        }

        public void AddRange(IEnumerable<T> range)
        {
            foreach (T t in range)
            {
                Add(t);
            }
        }

        public void Clear()
        {
            List<T> oldList = list;
            list = new List<T>();
            foreach (T item in oldList)
            {
                OnRemoved(item);
            }
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            if (list.Remove(item))
            {
                OnRemoved(item);
                return true;
            }
            return false;
        }

        [DebuggerStepThrough]
        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        [DebuggerStepThrough]
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }

    public class CollectionItemEventArgs<T> : EventArgs
    {
        T item;

        public T Item
        {
            get
            {
                return item;
            }
        }

        public CollectionItemEventArgs(T item)
        {
            this.item = item;
        }
    }

    public class VerticalScrollContainer : Control
    {
        public interface IScrollable
        {
            int ScrollOffsetY { get; set; }
            int ScrollHeightY { get; }
            int Height { get; }
            event MouseEventHandler MouseWheel;
        }

        IScrollable scrollable;
        bool showButtonsOnlyIfRequired = true;
        ScrollButtonControl down = new ScrollButtonControl();
        ScrollButtonControl up = new ScrollButtonControl();
        Timer timer = new Timer();

        int scrollSpeed = 5;

        public int ScrollSpeed
        {
            get
            {
                return scrollSpeed;
            }
            set
            {
                scrollSpeed = value;
            }
        }

        public VerticalScrollContainer()
        {
            up.Arrow = ScrollButton.Up;
            down.Arrow = ScrollButton.Down;
            up.Dock = DockStyle.Top;
            down.Dock = DockStyle.Bottom;
            this.TabStop = false;
            this.SetStyle(ControlStyles.Selectable, false);
            Controls.Add(up);
            Controls.Add(down);
            UpdateEnabled();

            timer.Interval = 50;
            timer.Tick += delegate
            {
                ScrollBy((int)timer.Tag);
            };
            up.MouseDown += delegate
            {
                timer.Tag = -scrollSpeed;
                ScrollBy(-scrollSpeed);
                timer.Start();
            };
            down.MouseDown += delegate
            {
                timer.Tag = scrollSpeed;
                ScrollBy(scrollSpeed);
                timer.Start();
            };
            up.MouseUp += StopTimer;
            down.MouseUp += StopTimer;
        }

        void StopTimer(object sender, MouseEventArgs e)
        {
            timer.Stop();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                timer.Dispose();
            }
        }

        void UpdateEnabled()
        {
            if (scrollable == null)
            {
                up.Visible = down.Visible = true;
                up.Enabled = down.Enabled = false;
            }
            else
            {
                int scrollHeightY = scrollable.ScrollHeightY;
                if (showButtonsOnlyIfRequired)
                {
                    if (scrollHeightY <= this.Height)
                    {
                        scrollable.ScrollOffsetY = 0;
                        up.Visible = down.Visible = false;
                        return;
                    }
                    up.Visible = down.Visible = true;
                }
                else
                {
                    up.Visible = down.Visible = true;
                    if (scrollable.ScrollHeightY <= scrollable.Height)
                    {
                        scrollable.ScrollOffsetY = 0;
                        up.Enabled = down.Enabled = false;
                        return;
                    }
                }
                // set enabled
                up.Enabled = scrollable.ScrollOffsetY > 0;
                down.Enabled = scrollable.ScrollOffsetY < scrollHeightY - scrollable.Height;
            }
        }

        void ScrollBy(int amount)
        {
            scrollable.ScrollOffsetY = Math.Max(0, Math.Min(scrollable.ScrollOffsetY + amount, scrollable.ScrollHeightY - scrollable.Height));
            UpdateEnabled();
        }

        public bool ShowButtonsOnlyIfRequired
        {
            get
            {
                return showButtonsOnlyIfRequired;
            }
            set
            {
                if (showButtonsOnlyIfRequired != value)
                {
                    showButtonsOnlyIfRequired = value;
                    UpdateEnabled();
                }
            }
        }

        void ScrollableWheel(object sender, MouseEventArgs e)
        {
            ScrollBy(-e.Delta / 3);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateEnabled();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (scrollable == null && !DesignMode)
            {
                scrollable = e.Control as IScrollable;
                if (scrollable != null)
                {
                    scrollable.MouseWheel += ScrollableWheel;
                    Controls.SetChildIndex(e.Control, 0);
                    UpdateEnabled();
                }
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (scrollable == e.Control)
            {
                scrollable.MouseWheel -= ScrollableWheel;
                scrollable = null;
                UpdateEnabled();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            // HACK: Windows.Forms bug workaround
            BeginInvoke(new MethodInvoker(PerformLayout));
        }
    }

    public class ScrollButtonControl : Control
    {
        public ScrollButtonControl()
        {
            this.BackColor = DynamicListColumn.DefaultBackColor;
            this.TabStop = false;
            this.SetStyle(ControlStyles.Selectable, false);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(14, 14);
            }
        }

        ScrollButton arrow = ScrollButton.Down;

        public ScrollButton Arrow
        {
            get
            {
                return arrow;
            }
            set
            {
                arrow = value;
                Invalidate();
            }
        }

        bool drawSeparatorLine = true;

        public bool DrawSeparatorLine
        {
            get
            {
                return drawSeparatorLine;
            }
            set
            {
                drawSeparatorLine = value;
                Invalidate();
            }
        }

        Color highlightColor = SystemColors.Highlight;

        public Color HighlightColor
        {
            get
            {
                return highlightColor;
            }
            set
            {
                highlightColor = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            const int margin = 2;
            int height = this.ClientSize.Height;
            int size = height - 2 * margin;
            int width = this.ClientSize.Width;
            int left = (width - size) / 2;
            int right = (width + size) / 2;
            Point[] triangle;
            switch (arrow)
            {
                case ScrollButton.Down:
                    triangle = new Point[] {
						new Point(left, margin), new Point(right, margin), new Point(width / 2, margin + size)
					};
                    if (drawSeparatorLine)
                        e.Graphics.DrawLine(SystemPens.GrayText, 0, 0, width, 0);
                    break;
                case ScrollButton.Up:
                    triangle = new Point[] {
						new Point(left, margin + size), new Point(right, margin + size), new Point(width / 2, margin)
					};
                    if (drawSeparatorLine)
                        e.Graphics.DrawLine(SystemPens.GrayText, 0, height - 1, width, height - 1);
                    break;
                default:
                    return;
            }
            Color color;
            if (Enabled)
                color = cursorEntered ? HighlightColor : ForeColor;
            else
                color = SystemColors.GrayText;
            using (Brush b = new SolidBrush(color))
            {
                e.Graphics.FillPolygon(b, triangle);
            }
        }

        bool cursorEntered = false;

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            cursorEntered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            cursorEntered = false;
            Invalidate();
        }
    }
}
