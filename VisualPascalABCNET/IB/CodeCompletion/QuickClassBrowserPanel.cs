﻿// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using CodeCompletion;
using System.Threading;
using PascalABCCompiler.Parsers;

namespace VisualPascalABC
{

	public class QuickClassBrowserPanel : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ComboBox classComboBox;
		private System.Windows.Forms.ComboBox membersComboBox;
		
		SymScope currentCompilationUnit;
		CodeFileDocumentTextEditorControl textAreaControl;
		bool autoselect = true;
		ComboBoxItem curItem;
		Position cur_pos;
		
		class ComboBoxItem : System.IComparable
		{
			SymScope item;
			string text;
			int    iconIndex;
			bool   isInCurrentPart;
			Position pos;
			bool is_global;
			
			public int IconIndex {
				get {
					return iconIndex;
				}
			}
			
			public SymScope Item {
				get {
					return item;
				}
			}
			
			public bool IsInCurrentPart {
				get {
					return isInCurrentPart;
				}
			}
			
			public Position ItemRegion {
				get {
					return pos;
				}
			}
			
			public int Line {
				get {
					/*DomRegion r = this.ItemRegion;
					if (r.IsEmpty)
						return 0;
					else
						return r.BeginLine - 1;*/
					return pos.line-1;
				}
			}
			
			public int Column {
				get {
					return pos.column-1;
				}
			}
			
			public int EndLine {
				get {
					return pos.end_line-1;
				}
			}
			
			public ComboBoxItem(SymScope item, string text, int iconIndex, bool isInCurrentPart, bool is_global)
			{
				this.item = item;
				if (item != null)
				pos = item.GetPosition();
				this.text = text;
				this.iconIndex = iconIndex;
				this.isInCurrentPart = true;
				this.is_global = is_global;
			}

			public bool IsInside(int lineNumber, int columnNumber)
			{
				if (item == null || is_global) return true;
				if (pos.line < lineNumber+1 && pos.end_line > lineNumber+1) return true;
				if (pos.line > lineNumber+1 || pos.end_line < lineNumber+1) return false;
				if (pos.line == lineNumber+1 && pos.end_line == lineNumber+1)
				{
					if (pos.column <= columnNumber+1 && pos.end_column >= columnNumber+1) return true;
					else return false;
				}
				else if (pos.end_line != lineNumber+1 && pos.column <= columnNumber+1) return true;
				else if (pos.line != lineNumber+1 && pos.end_column >= columnNumber+1) return true;
				return false;
			}
			
			public int CompareItemTo(object obj)
			{
				ComboBoxItem boxItem = (ComboBoxItem)obj;

				if (boxItem.Item is IComparable) {
					return ((IComparable)boxItem.Item).CompareTo(item);
				}
				if (boxItem.text != text || boxItem.Line != Line || boxItem.EndLine != EndLine || boxItem.iconIndex != iconIndex) {
					return 1;
				}
				return 0;
			}
			
			string cachedString;
			
			public override string ToString()
			{
				// ambience lookups can be expensive when the return type is
				// resolved on the fly.
				// Therefore, we need to cache the generated string because it is used
				// very often for the sorting.
				if (cachedString == null)
					cachedString = ToStringInternal();
				return cachedString;
			}
			
			string ToStringInternal()
			{
				if (text == null) return "";
				return text;
			}
			
			#region System.IComparable interface implementation
			public int CompareTo(object obj)
			{
				return ToString().CompareTo(obj.ToString());
			}
			#endregion
			
		}
		
		public QuickClassBrowserPanel(CodeFileDocumentTextEditorControl textAreaControl)
		{
			InitializeComponent();
			this.membersComboBox.MaxDropDownItems = 20;
			
			base.Dock = DockStyle.Top;
			this.textAreaControl = textAreaControl;
			this.textAreaControl.ActiveTextAreaControl.Caret.PositionChanged += new EventHandler(CaretPositionChanged);
			//this.Paint += new PaintEventHandler( PaintInternal);
            if (VisualPABCSingleton.MainForm != null && !VisualPABCSingleton.MainForm.UserOptions.ShowQuickClassBrowserPanel)
            {
                this.Visible = false;
            }
//            else this.ShowPanel();
			//FillMembersComboBox();
			//UpdateMembersComboBox();
            if (VisualPABCSingleton.MainForm != null)
            {
                th = new Thread(new ThreadStart(ChangeInternal));
                th.Priority = ThreadPriority.Lowest;
                th.IsBackground = true;
                th.Start();
            }
		}
        
        new public bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                if (value)
                    ShowPanel();
                else
                    HidePanel();
            }
        }

		public void ShowPanel()
		{
			base.Visible = true;
            //if (Parent != null)
                //Parent.Parent.Parent.BackColor = SystemColors.Control;
            //textAreaControl.TextAreaPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		}
		
		public void HidePanel()
		{
			base.Visible = false;
            //if (Parent != null)
                //Parent.Parent.Parent.BackColor = SystemColors.ButtonShadow;
            //textAreaControl.TextAreaPanel.BorderStyle = System.Windows.Forms.BorderStyle.None;
		}
		
		private void PaintInternal(object sender, PaintEventArgs e)
		{
			//base.OnPaint(e);
			//e.Graphics.DrawLine(new Pen(SystemColors.ControlLight,0.1F),e.ClipRectangle.Left,e.ClipRectangle.Bottom-3,e.ClipRectangle.Right,e.ClipRectangle.Bottom-3);
			//e.Graphics.DrawLine(new Pen(SystemColors.ControlDark,0.1F),e.ClipRectangle.Left,e.ClipRectangle.Bottom-1,e.ClipRectangle.Right,e.ClipRectangle.Bottom-1);
		}
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				this.textAreaControl.ActiveTextAreaControl.Caret.PositionChanged -= new EventHandler(CaretPositionChanged);
				this.membersComboBox.Dispose();
				this.classComboBox.Dispose();
				this.currentCompilationUnit = null;
				th.Abort();
			}
			base.Dispose(disposing);
		}
		
		void ChangeInternal()
		{
			bool first_comp;
			CodeCompletion.DomConverter dconv=null;
			bool tmp = true;
            while (true)
            {
                first_comp = false;
                if (this.Visible)
                    try
                    {
                        if (currentCompilationUnit == null && textAreaControl.FileName != null)
                        {
                            dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[textAreaControl.FileName];
                            if (dconv != null && dconv.is_compiled)
                            {
                                currentCompilationUnit = dconv.visitor.entry_scope;
                                first_comp = true;
                            }
                            if (clicked || tmp)
                            {
                                FillClassComboBox(true);
                                //clicked = false;
                                tmp = false;
                            }
                        }
                        if (currentCompilationUnit != null && (clicked || first_comp))
                        {
                            dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[textAreaControl.FileName];
                            if (dconv != null && dconv.is_compiled)
                            {
                                currentCompilationUnit = dconv.visitor.entry_scope;
                                FillClassComboBox(true);
                            }

                            //FillMembersComboBox();
                            //UpdateClassComboBox();
                            //UpdateMembersComboBox();
                        }
                        //lock(clicked)
                        {
                            clicked = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageService.ShowError(ex);
                    }
                Thread.Sleep(300);
            }
		}
		
		Thread th;
		bool clicked = false;
		IAsyncResult ar;
		
		void CaretPositionChanged(object sender, EventArgs e)
		{
			// ignore simple movements
			
			if (e != EventArgs.Empty) {
				return;
			}
			clicked = true;
		}
		
		bool membersComboBoxSelectedMember = false;
		
		void UpdateMembersComboBox()
		{
			membersComboBox.Invoke(new Invoke_del(Member_Invoke_Refresh));
			autoselect = false;
            try
            {
                if (currentCompilationUnit != null)
                {
                    for (int i = 0; i < membersComboBox.Items.Count; ++i)
                    {
                        if (((ComboBoxItem)membersComboBox.Items[i]).IsInside(textAreaControl.ActiveTextAreaControl.Caret.Line, textAreaControl.ActiveTextAreaControl.Caret.Column))
                        {
                            if ((int)membersComboBox.Invoke(new Invoke_del_with_ret(GetSelectedMemberInternal)) != i)
                            {
                                membersComboBox.Invoke(new Invoke_param_del(SetSelectedMemberItemInternal), i);
                            }
                            if (!membersComboBoxSelectedMember)
                            {
                                membersComboBox.Invoke(new Invoke_del(Member_Invoke_Refresh));
                            }
                            membersComboBoxSelectedMember = true;
                            return;
                        }
                    }
                }
                membersComboBox.Invoke(new Invoke_param_del(SetSelectedMemberItemInternal), -1);
                if (membersComboBoxSelectedMember)
                {
                    membersComboBox.Invoke(new Invoke_del(Member_Invoke_Refresh));
                    membersComboBoxSelectedMember = false;
                }
            }
            catch (Exception e)
            {
            }
			finally {
				autoselect = true;
			}
		}
		
		bool classComboBoxSelectedMember = false;
        void UpdateClassComboBox(bool need_fill_members)
        {
            // Still needed ?
            /*if (currentCompilationUnit == null) {
                currentCompilationUnit = (ICompilationUnit)ParserService.GetParseInformation(Path.GetFullPath(textAreaControl.FileName)).MostRecentCompilationUnit;
            }*/

            autoselect = false;
            Hashtable ns_list = new Hashtable();
            try
            {
                //if (currentCompilationUnit != null) 
                //// Alex: when changing between files in different compilation units whole process must be restarted
                //// happens usually when files are opened from different project(s)
                for (int i = 0; i < classComboBox.Items.Count; i++)
                {
                    if (((ComboBoxItem)classComboBox.Items[i]).IsInside(textAreaControl.ActiveTextAreaControl.Caret.Line, textAreaControl.ActiveTextAreaControl.Caret.Column))
                    {

                        bool innerClassContainsCaret = false;
                        if (((ComboBoxItem)classComboBox.Items[i]).Item == null || ((ComboBoxItem)classComboBox.Items[i]).Item.si.kind == SymbolKind.Namespace)
                        {
                            ns_list[(ComboBoxItem)classComboBox.Items[i]] = i;
                            continue;
                        }
                        /*for (int j = i + 1; j < classComboBox.Items.Count;j++) {
                            if (((ComboBoxItem)classComboBox.Items[j]).IsInside(textAreaControl.ActiveTextAreaControl.Caret.Line)) {
                                if (((ComboBoxItem)classComboBox.Items[j]).Item.si.kind == SymbolKind.Namespace)
                                {
                                    ns_list[classComboBox.Items[j]] = j;
                                    continue;
                                }
                                innerClassContainsCaret = true;
                                break;
                            }
                        }*/
                        if (!innerClassContainsCaret)
                        {
                            if ((int)classComboBox.Invoke(new Invoke_del_with_ret(GetSelectedClassInternal)) != i)
                            {
                                //classComboBox.SelectedIndex = i;
                                classComboBox.Invoke(new Invoke_param_del(SetSelectedClassItemInternal), i);
                                if (need_fill_members)
                                    FillMembersComboBox();
                            }
                            if (!classComboBoxSelectedMember)
                            {
                                classComboBox.Invoke(new Invoke_del(Invoke_Refresh));
                            }
                            classComboBoxSelectedMember = true;

                            return;
                        }
                    }
                }
                foreach (ComboBoxItem cbi in ns_list.Keys)
                {
                    if (cbi.Item is ImplementationUnitScope && cbi.IsInside(textAreaControl.ActiveTextAreaControl.Caret.Line, textAreaControl.ActiveTextAreaControl.Caret.Column))
                    {
                        //classComboBox.SelectedIndex = (int)ns_list[cbi];
                        classComboBox.Invoke(new Invoke_param_del(SetSelectedClassItemInternal), ns_list[cbi]);
                        if (need_fill_members)
                            FillMembersComboBox();
                        classComboBox.Invoke(new Invoke_del(Invoke_Refresh));
                        classComboBoxSelectedMember = true;
                        return;
                    }
                    else
                    {
                        //classComboBox.SelectedIndex = (int)ns_list[cbi];
                        classComboBox.Invoke(new Invoke_param_del(SetSelectedClassItemInternal), ns_list[cbi]);
                        classComboBoxSelectedMember = true;
                    }
                }
                if (need_fill_members)
                    FillMembersComboBox();
                classComboBox.Invoke(new Invoke_del(Invoke_Refresh));
                return;

                if (classComboBoxSelectedMember)
                {
                    classComboBox.Invoke(new Invoke_del(Invoke_Refresh));
                    classComboBoxSelectedMember = false;
                }
            }
            finally
            {
                autoselect = true;
            }
            //				classComboBox.SelectedIndex = -1;
        }

        SymScope GetCurrentSelectedClass()
        {
            try
            {
                if ((int)classComboBox.Invoke(new Invoke_del_with_ret(GetSelectedClassInternal)) >= 0)
                {
                    return (SymScope)((ComboBoxItem)classComboBox.Items[(int)classComboBox.Invoke(new Invoke_del_with_ret(GetSelectedClassInternal))]).Item;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        SymScope GetCurrentSelectedMember()
        {
            if ((int)membersComboBox.Invoke(new Invoke_del_with_ret(GetSelectedClassInternal)) >= 0)
            {
                return (SymScope)((ComboBoxItem)membersComboBox.Items[(int)membersComboBox.Invoke(new Invoke_del_with_ret(GetSelectedClassInternal))]).Item;
            }
            return null;
        }

        bool NeedtoUpdate(ArrayList items, ComboBox comboBox)
        {
            if (items.Count != comboBox.Items.Count)
            {
                return true;
            }
            for (int i = 0; i < items.Count; ++i)
            {
                ComboBoxItem oldItem = (ComboBoxItem)comboBox.Items[i];
                ComboBoxItem newItem = (ComboBoxItem)items[i];
                if (oldItem.GetType() != newItem.GetType())
                {
                    return true;
                }
                if (newItem.CompareItemTo(oldItem) != 0)
                {
                    return true;
                }
            }
            return false;
        }
		
		//IClass lastClassInMembersComboBox;

        void GetGlobalUnitMembers(ArrayList items)
        {
            foreach (SymScope ss in currentCompilationUnit.members)
            {
                if (ss.si.kind == SymbolKind.Method || ss.si.kind == SymbolKind.Constant || ss.si.kind == SymbolKind.Variable || ss.si.kind == SymbolKind.Event || ss.si.kind == SymbolKind.Field
                   || ss.si.kind == SymbolKind.Parameter || ss.si.kind == SymbolKind.Property)
                {
                    items.Add(new ComboBoxItem(ss, ss.GetDescriptionWithoutDoc(), CodeCompletionProvider.ImagesProvider.GetPictureNum(ss.si), true, false));
                }
            }
            int ind = items.Count;
            items.Sort(0, items.Count, new Comparer(System.Globalization.CultureInfo.InvariantCulture));

            if ((currentCompilationUnit as InterfaceUnitScope).impl_scope != null)
            {
                ImplementationUnitScope us = (currentCompilationUnit as InterfaceUnitScope).impl_scope;
                //items.Add(new ComboBoxItem(us,"implementation",CodeCompletionProvider.ImagesProvider.GetPictureNum(us.si),true));
                foreach (SymScope ss in us.members)
                {
                    if (ss.si.kind == SymbolKind.Method || ss.si.kind == SymbolKind.Constant || ss.si.kind == SymbolKind.Variable || ss.si.kind == SymbolKind.Event || ss.si.kind == SymbolKind.Field
                    || ss.si.kind == SymbolKind.Parameter || ss.si.kind == SymbolKind.Property)
                    {
                        items.Add(new ComboBoxItem(ss, ss.GetDescriptionWithoutDoc(), CodeCompletionProvider.ImagesProvider.GetPictureNum(ss.si), true, false));
                    }
                }
                items.Sort(ind, items.Count - ind, new Comparer(System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        void GetClassMembers(SymScope ss, ArrayList items)
        {
            if (ss != null && ss.members != null)
            {
                ArrayList meths = new ArrayList();
                ArrayList fields = new ArrayList();
                ArrayList events = new ArrayList();
                ArrayList vars = new ArrayList();
                ArrayList props = new ArrayList();
                ArrayList consts = new ArrayList();
                foreach (SymScope el in ss.members)
                    //if (el.si.kind == SymbolKind.Method || el.si.kind == SymbolKind.Constant || el.si.kind == SymbolKind.Variable || el.si.kind == SymbolKind.Event || el.si.kind == SymbolKind.Field
                    //|| el.si.kind == SymbolKind.Property)
                    if (el.GetPosition().file_name != null)
                    {
                        ComboBoxItem cbi = new ComboBoxItem(el, el.GetDescriptionWithoutDoc(), CodeCompletionProvider.ImagesProvider.GetPictureNum(el.si), true, false);
                        switch (el.si.kind)
                        {
                            case SymbolKind.Method: meths.Add(cbi); break;
                            case SymbolKind.Field: fields.Add(cbi); break;
                            case SymbolKind.Property: props.Add(cbi); break;
                            case SymbolKind.Variable: vars.Add(cbi); break;
                            case SymbolKind.Event: events.Add(cbi); break;
                            case SymbolKind.Constant: consts.Add(cbi); break;
                        }
                        //items.Add(new ComboBoxItem(el,el.GetDescriptionWithoutDoc(),CodeCompletionProvider.ImagesProvider.GetPictureNum(el.si),true));
                    }
                meths.Sort(new Comparer(System.Globalization.CultureInfo.InvariantCulture));
                items.AddRange(meths);
                props.Sort(new Comparer(System.Globalization.CultureInfo.InvariantCulture));
                items.AddRange(props);
                fields.Sort(new Comparer(System.Globalization.CultureInfo.InvariantCulture));
                items.AddRange(fields);
                vars.Sort(new Comparer(System.Globalization.CultureInfo.InvariantCulture));
                items.AddRange(vars);
                consts.Sort(new Comparer(System.Globalization.CultureInfo.InvariantCulture));
                items.AddRange(consts);
                events.Sort(new Comparer(System.Globalization.CultureInfo.InvariantCulture));
                items.AddRange(events);
            }
        }

        void FillMembersComboBox()
        {
            ArrayList items = new ArrayList();
            SymScope ss = GetCurrentSelectedClass();
            if (ss == null)
            {
                GetGlobalUnitMembers(items);
                //items.Sort(0,items.Count,new Comparer(System.Globalization.CultureInfo.InvariantCulture));
            }
            else
            {
                GetClassMembers(ss, items);
                //items.Sort(0,items.Count,new Comparer(System.Globalization.CultureInfo.InvariantCulture));
            }
            membersComboBox.Invoke(new Invoke_del(Member_Invoke_Update));
            membersComboBox.Invoke(new Invoke_del(Member_Invoke_Clear));
            //membersComboBox.Items.AddRange(items.ToArray());
            membersComboBox.Invoke(new Invoke_param_del(AddItemsToMembersComboboxInternal), items);
            UpdateMembersComboBox();
            membersComboBox.Invoke(new Invoke_del(Member_Invoke_EndUpdate));
        }

        void AddClasses(ArrayList items)
        {
            ImplementationUnitScope impl = null;
            int ind = 0;
            if (currentCompilationUnit == null)
            {
                //items.Add(new ComboBoxItem(currentCompilationUnit,PascalABCCompiler.StringResources.Get("CODE_COMPLETION_GLOBAL"),CodeCompletionProvider.ImagesProvider.IconNumberUnitNamespace,true,true));
                return;
            }
            if ((currentCompilationUnit as InterfaceUnitScope).impl_scope != null)
            {
                items.Add(new ComboBoxItem(currentCompilationUnit, PascalABCCompiler.StringResources.Get("CODE_COMPLETION_INTERFACE"), CodeCompletionProvider.ImagesProvider.GetPictureNum(currentCompilationUnit.si), true, false));
                impl = (currentCompilationUnit as InterfaceUnitScope).impl_scope;
                items.Add(new ComboBoxItem(impl, PascalABCCompiler.StringResources.Get("CODE_COMPLETION_IMPLEMENTATION"), CodeCompletionProvider.ImagesProvider.GetPictureNum(impl.si), true, false));
                ind = 2;
            }
            else
            {
                items.Add(new ComboBoxItem(currentCompilationUnit, PascalABCCompiler.StringResources.Get("CODE_COMPLETION_GLOBAL"), CodeCompletionProvider.ImagesProvider.IconNumberUnitNamespace, true, true));
                ind = 1;
            }
            foreach (SymScope ss in currentCompilationUnit.members)
            {
                if (ss.si.kind == SymbolKind.Class || ss.si.kind == SymbolKind.Struct || ss.si.kind == SymbolKind.Type || ss.si.kind == SymbolKind.Interface || ss.si.kind == SymbolKind.Enum || ss.si.kind == SymbolKind.Delegate)
                {
                    if (ss.GetPosition().file_name != null && !ss.si.name.Contains("$"))
                        items.Add(new ComboBoxItem(ss, ss.si.name, CodeCompletionProvider.ImagesProvider.GetPictureNum(ss.si), true, false));
                }
            }
            if (impl != null)
                foreach (SymScope ss in impl.members)
                {
                    if (ss.si.kind == SymbolKind.Class || ss.si.kind == SymbolKind.Struct || ss.si.kind == SymbolKind.Type || ss.si.kind == SymbolKind.Interface || ss.si.kind == SymbolKind.Enum || ss.si.kind == SymbolKind.Delegate)
                    {
                        if (!ss.si.name.Contains("$"))
                            items.Add(new ComboBoxItem(ss, ss.si.name, CodeCompletionProvider.ImagesProvider.GetPictureNum(ss.si), true, false));
                    }
                }
            items.Sort(ind, items.Count - ind, new Comparer(System.Globalization.CultureInfo.InvariantCulture));
        }
		
		delegate void Invoke_del();
		delegate void Invoke_param_del(object obj);
		delegate object Invoke_del_with_ret();
		
		void Invoke_Update()
		{
			classComboBox.BeginUpdate();
		}
		
		void Invoke_EndUpdate()
		{
			classComboBox.EndUpdate();
		}
		
		void Invoke_Clear()
		{
			classComboBox.Items.Clear();
		}
		
		void Invoke_Refresh()
		{
			classComboBox.Refresh();
		}
		
		void Member_Invoke_Update()
		{
			membersComboBox.BeginUpdate();
		}
		
		void Member_Invoke_EndUpdate()
		{
			membersComboBox.EndUpdate();
		}
		
		void Member_Invoke_Clear()
		{
			membersComboBox.Items.Clear();
		}
		
		void Member_Invoke_Refresh()
		{
			membersComboBox.Refresh();
		}
		
		void AddItemsToClassComboboxInternal(object items)
		{
			lock(classComboBox)
			{
				classComboBox.Items.AddRange(((ArrayList)items).ToArray());
			}
		}
		
		void AddItemsToMembersComboboxInternal(object items)
		{
			lock (membersComboBox)
			{
				membersComboBox.Items.AddRange(((ArrayList)items).ToArray());
			}
		}
		
		void SetSelectedClassItemInternal(object item)
		{
			classComboBox.SelectedIndex = (int)item;
		}
		
		void SetSelectedMemberItemInternal(object item)
		{
			membersComboBox.SelectedIndex = (int)item;
		}
		
		object GetSelectedClassInternal()
		{
			return classComboBox.SelectedIndex;
		}
		
		object GetSelectedMemberInternal()
		{
			return membersComboBox.SelectedIndex;
		}
		
		void FillClassComboBox(bool isUpdateRequired)
		{
            try //временно, исправть ошибку
            {
                if (isUpdateRequired)
                {
                    classComboBox.Invoke(new Invoke_del(Invoke_Update));
                }
                ArrayList items = new ArrayList();
                bool only_global = false;
                AddClasses(items);
                if (items.Count == 0)
                {
                    items.Add(new ComboBoxItem(currentCompilationUnit, PascalABCCompiler.StringResources.Get("CODE_COMPLETION_GLOBAL"), CodeCompletionProvider.ImagesProvider.IconNumberUnitNamespace, true, true));
                    only_global = true;
                }
                classComboBox.Invoke(new Invoke_del(Invoke_Clear));
                //здесь ошибка доступа из другого потока [DSRE0001]
                //classComboBox.Items.AddRange(items.ToArray());
                classComboBox.Invoke(new Invoke_param_del(AddItemsToClassComboboxInternal), items);
                if (items.Count == 1)
                {
                    try
                    {
                        autoselect = false;
                        //classComboBox.SelectedIndex = 0;

                        //FillMembersComboBox();
                    }
                    finally
                    {
                        autoselect = true;
                    }
                }
                UpdateClassComboBox(!only_global);
                if (isUpdateRequired)
                {
                    classComboBox.Invoke(new Invoke_del(Invoke_EndUpdate));
                }
            }
            catch (Exception ex)
            {
            }
		}
		
		
		// THIS METHOD IS MAINTAINED BY THE FORM DESIGNER
		// DO NOT EDIT IT MANUALLY! YOUR CHANGES ARE LIKELY TO BE LOST
		void InitializeComponent() {
            this.membersComboBox = new System.Windows.Forms.ComboBox();
            this.classComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // membersComboBox
            // 
            this.membersComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.membersComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.membersComboBox.DropDownHeight = 250;
            this.membersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.membersComboBox.Font = new System.Drawing.Font("Arial", 9F);
            this.membersComboBox.IntegralHeight = false;
            this.membersComboBox.Location = new System.Drawing.Point(195, 2);
            this.membersComboBox.Name = "membersComboBox";
            this.membersComboBox.Size = new System.Drawing.Size(212, 26);
            this.membersComboBox.TabIndex = 1;
            this.membersComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ComboBoxDrawItem);
            this.membersComboBox.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.MeasureComboBoxItem);
            this.membersComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSelectedIndexChanged);
            this.membersComboBox.DropDown += new System.EventHandler(this.ComboBoxDropDown);
            // 
            // classComboBox
            // 
            this.classComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.classComboBox.DropDownHeight = 150;
            this.classComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classComboBox.Font = new System.Drawing.Font("Arial", 9F);
            this.classComboBox.IntegralHeight = false;
            this.classComboBox.Location = new System.Drawing.Point(1, 1);
            this.classComboBox.Name = "classComboBox";
            this.classComboBox.Size = new System.Drawing.Size(189, 26);
            this.classComboBox.TabIndex = 0;
            this.classComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ComboBoxDrawItem);
            this.classComboBox.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.MeasureComboBoxItem);
            this.classComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSelectedIndexChanged);
            // 
            // QuickClassBrowserPanel
            // 
            this.Controls.Add(this.membersComboBox);
            this.Controls.Add(this.classComboBox);
            this.Name = "QuickClassBrowserPanel";
            this.Size = new System.Drawing.Size(410, 24);
            this.Resize += new System.EventHandler(this.QuickClassBrowserPanelResize);
            this.ResumeLayout(false);

		}
		
		void ComboBoxDropDown(object sender, System.EventArgs e)
		{
			
		}
		
		void ComboBoxSelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                ComboBox comboBox = (ComboBox)sender;
                if (autoselect)
                {
                    ComboBoxItem item = null;
                    if (comboBox == classComboBox)
                        item = (ComboBoxItem)comboBox.Items[(int)comboBox.Invoke(new Invoke_del_with_ret(GetSelectedClassInternal))];
                    else
                        item = (ComboBoxItem)comboBox.Items[(int)comboBox.Invoke(new Invoke_del_with_ret(GetSelectedMemberInternal))];
                    if (item.IsInCurrentPart)
                    {
                        //textAreaControl.ActiveTextAreaControl.Caret.Position = new ICSharpCode.TextEditor.TextLocation(item.Column, item.Line);
                        //textAreaControl.ActiveTextAreaControl.TextArea.Focus();
                        //VisualPABCSingleton.MainForm.ExecuteSourceLocationAction(new PascalABCCompiler.SourceLocation(textAreaControl.FileName,item.Line+1,item.Column,
                        //                                                                                              item.EndLine+1,item.Column),VisualPascalABCPlugins.SourceLocationAction.NavigationGoto);
                        VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.ExecuteSourceLocationAction(
                            new PascalABCCompiler.SourceLocation(textAreaControl.FileName, item.Line + 1, item.Column + 1, item.Line + 1, item.Column + 1), VisualPascalABCPlugins.SourceLocationAction.GotoBeg);
                    }
                    else
                    {

                    }
                    if (comboBox == classComboBox)
                    {
                        FillMembersComboBox();
                        UpdateMembersComboBox();
                    }
                }
            }
            catch (Exception ex)
            {

            }
		}
		
		// font - has to be static - don't create on each draw
		static Font font = font = new Font("Tahoma", 8.5f);
		static StringFormat drawStringFormat = new StringFormat(StringFormatFlags.NoWrap);
		
		void ComboBoxDrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
            lock (comboBox)
            {
                e.DrawBackground();
                if (e.Index >= 0)
                {

                    ComboBoxItem item = (ComboBoxItem)comboBox.Items[e.Index];

                    e.Graphics.DrawImage(CodeCompletionProvider.ImagesProvider.ImageList.Images[item.IconIndex],
                                                 new Point(e.Bounds.X, e.Bounds.Y + (e.Bounds.Height - CodeCompletionProvider.ImagesProvider.ImageList.ImageSize.Height) / 2));
                    Rectangle drawingRect = new Rectangle(e.Bounds.X + CodeCompletionProvider.ImagesProvider.ImageList.ImageSize.Width,
                                                          e.Bounds.Y,
                                                          e.Bounds.Width - CodeCompletionProvider.ImagesProvider.ImageList.ImageSize.Width,
                                                          e.Bounds.Height);

                    Brush drawItemBrush = SystemBrushes.WindowText;
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        drawItemBrush = SystemBrushes.HighlightText;
                    }
                    if (!item.IsInCurrentPart)
                    {
                        drawItemBrush = SystemBrushes.ControlDark;
                    }
                    else if (e.State == DrawItemState.ComboBoxEdit && !item.IsInside(textAreaControl.ActiveTextAreaControl.Caret.Line, textAreaControl.ActiveTextAreaControl.Caret.Column))
                    {
                        drawItemBrush = SystemBrushes.ControlDark;
                    }
                    e.Graphics.DrawString(item.ToString(),
                                          font,
                                          drawItemBrush,
                                          drawingRect,
                                          drawStringFormat);

                }
                //e.DrawFocusRectangle();
            }
		}
		
		void QuickClassBrowserPanelResize(object sender, System.EventArgs e)
		{
            Size comboBoxSize = new Size(Width / 2 - 1, classComboBox.Size.Height);
            classComboBox.Size = comboBoxSize;
            membersComboBox.Location = new Point(classComboBox.Bounds.Right + 2, classComboBox.Bounds.Top);
            membersComboBox.Size = comboBoxSize;
		}
		
		void MeasureComboBoxItem(object sender, System.Windows.Forms.MeasureItemEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (e.Index >= 0) {
				ComboBoxItem item = (ComboBoxItem)comboBox.Items[e.Index];
				SizeF size = e.Graphics.MeasureString(item.ToString(), font);
				e.ItemWidth  = (int)size.Width;
				
				e.ItemHeight = (int)Math.Max(size.Height, CodeCompletionProvider.ImagesProvider.ImageList.ImageSize.Height);
			}
		}
	}
}
