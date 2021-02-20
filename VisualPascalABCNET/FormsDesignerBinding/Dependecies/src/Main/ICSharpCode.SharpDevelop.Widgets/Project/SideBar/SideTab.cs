﻿// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ICSharpCode.SharpDevelop.Widgets.SideBar
{
	public enum SideTabStatus {
		Normal,
		Selected,
		Dragged
	}
	
	public delegate void SideTabEventHandler(object source, SideTabEventArgs e);
	
	public class SideTabEventArgs
	{
		SideTab tab;
		
		public SideTabEventArgs(SideTab tab)
		{
			this.tab = tab;
		}
		
		public SideTab SideTab {
			get {
				return tab;
			}
		}
	}
	
	public delegate void SideTabItemEventHandler(object source, SideTabItemEventArgs e);
	
	public class SideTabItemEventArgs
	{
		SideTabItem item;
		
		public SideTabItemEventArgs(SideTabItem item)
		{
			this.item = item;
		}
		
		public SideTabItem Item {
			get {
				return item;
			}
		}
	}
	
	public delegate void SideTabItemExchangeEventHandler(object source, SideTabItemExchangeEventArgs e);
	
	public class SideTabItemExchangeEventArgs
	{
		SideTabItem item1;
		SideTabItem item2;
		
		public SideTabItemExchangeEventArgs(SideTabItem item1, SideTabItem item2)
		{
			this.item1 = item1;
			this.item2 = item2;
		}
		
		public SideTabItem Item1 {
			get {
				return item1;
			}
		}
		
		public SideTabItem Item2 {
			get {
				return item2;
			}
		}
	}
	
	public class SideTab
	{
		string    name, displayName;
		bool      canDragDrop  = true;
		bool      canBeDeleted = true;
		bool      canBeRenamed = true;
		SideTabItemCollection items = new SideTabItemCollection();
		SideTabStatus sideTabStatus;
		SideTabItem   selectedItem = null;
		SideTabItem   chosenItem  = null;
		
		ImageList largeImageList = null;
		ImageList smallImageList = null;
		int       scrollIndex    = 0;

		float     dpiY           = 96.0F;

		public bool Hidden = false;
		
		public int ScrollIndex {
			get {
				return scrollIndex;
			}
			set {
				scrollIndex = value;
			}
		}
		
		public ImageList LargeImageList {
			get {
				return largeImageList;
			}
			set {
				largeImageList = value;
			}
		}
		
		public ImageList SmallImageList {
			get {
				return smallImageList;
			}
			set {
				smallImageList = value;
			}
		}
		
		public SideTabStatus SideTabStatus {
			get {
				return sideTabStatus;
			}
			
			set {
				sideTabStatus = value;
			}
		}
		
		public bool CanBeDeleted {
			get {
				return canBeDeleted;
			}
			set {
				canBeDeleted = value;
			}
		}
		
		public bool CanBeRenamed {
			get {
				return canBeRenamed;
			}
			set {
				canBeRenamed = value;
			}
		}
		
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
				displayName = value;
			}
		}
		
		public string DisplayName {
			get { return displayName; }
			set { displayName = value; }
		}
		
		public SideTabItemCollection Items  {
			get {
				return items;
			}
		}
		
		public bool CanDragDrop {
			get {
				return canDragDrop;
			}
			set {
				canDragDrop = value;
			}
		}
		
		bool canSaved = true;
		
		public bool CanSaved {
			get {
				return canSaved;
			}
			set {
				canSaved = value;
			}
		}
		
		public SideTabItem SelectedItem {
			get {
				return selectedItem;
			}
			set {
				if (selectedItem != null && selectedItem != chosenItem) {
					selectedItem.SideTabItemStatus = SideTabItemStatus.Normal;
				}
				selectedItem = value;
				if (selectedItem != null && selectedItem != chosenItem) {
					selectedItem.SideTabItemStatus = SideTabItemStatus.Selected;
				}
			}
		}
		
		protected  void OnChosenItemChanged(EventArgs e)
		{
			if (ChosenItemChanged != null) {
				ChosenItemChanged(this, e);
			}
		}
		public event EventHandler ChosenItemChanged;
		
		public SideTabItem ChosenItem {
			get {
				return chosenItem;
			}
			set {
				if (chosenItem != null) {
					chosenItem.SideTabItemStatus = SideTabItemStatus.Normal;
				}
				chosenItem = value;
				if (chosenItem != null) {
					chosenItem.SideTabItemStatus = SideTabItemStatus.Chosen;
				}
				OnChosenItemChanged(null);
			}
		}
		
		/// <summary>
		/// A SideTabItem has been removed.
		/// </summary>
		public event SideTabItemEventHandler ItemRemoved;
		
		/// <summary>
		/// Two SideTabItems have exchanged locations.
		/// </summary>
		public event SideTabItemExchangeEventHandler ItemsExchanged;
		
		public ISideTabItemFactory SideTabItemFactory {
			get {
				return items.SideTabItemFactory;
			}
			set {
				items.SideTabItemFactory = value;
			}
		}
		
		protected SideTab()
		{
		}
		
		private SideTab(ISideTabItemFactory sideTabItemFactory)
		{
			SideTabItemFactory = sideTabItemFactory;
		}
		
		public SideTab(SideBarControl sideBar, string name) : this(sideBar.SideTabItemFactory)
		{
			var g = sideBar.CreateGraphics();
			dpiY = g.DpiY;
			g.Dispose();

			this.Name = name;
			SetCanRename();
			items.ItemRemoved += OnSideTabItemRemoved;
		}
		
		public SideTab(string name)
		{
			this.Name = name;
			SetCanRename();
			items.ItemRemoved += OnSideTabItemRemoved;
		}
		
		public bool ScrollDownButtonActivated {
			get {
				return scrollIndex > 0;
			}
		}
		
		public bool ScrollUpButtonActivated {
			get {
				return true;
			}
		}
		
		public void DrawTabHeader(Graphics g, Font font, Point pos, int width)
		{
            var sz = ScreenScale.Calc();
            var xleft = Convert.ToInt32(3 * sz);
            switch (sideTabStatus) {
				case SideTabStatus.Normal:
					ControlPaint.DrawBorder3D(g, new Rectangle(0, pos.Y, width - 4, font.Height + 4), Border3DStyle.RaisedInner);
					g.DrawString(displayName, font, SystemBrushes.ControlText, new RectangleF(1 + xleft, pos.Y + 1, width - 5, font.Height + 1));
					
					break;
				case SideTabStatus.Selected:
					ControlPaint.DrawBorder3D(g, new Rectangle(0, pos.Y, width - 4, font.Height + 4), Border3DStyle.Sunken);
					g.DrawString(displayName, font, SystemBrushes.ControlText, new RectangleF(1 + 1 + xleft, pos.Y + 2, width - 5, font.Height + 2));
					break;
				case SideTabStatus.Dragged:
					Rectangle r = new Rectangle(0, pos.Y, width - 4, font.Height + 4);
					ControlPaint.DrawBorder3D(g, r, Border3DStyle.RaisedInner);
					r.X += 2;
					r.Y += 1;
					r.Width  -= 4;
					r.Height -= 2;
					
					g.FillRectangle(SystemBrushes.ControlDarkDark, r);
					
					g.DrawString(displayName, font, SystemBrushes.HighlightText, new RectangleF(1 + 1 + xleft, pos.Y + 2, width - 5, font.Height + 2));
					break;
			}
		}
		
		public int Height {
			get {
				return Items.Count * ItemHeight;
			}
		}
		
		public Point GetLocation(SideTabItem whichItem)
		{
			for (int i = 0; i < Items.Count; ++i) {
				SideTabItem item = (SideTabItem)Items[i];
				if (item == whichItem) {
					return new Point(0, i * ItemHeight);
				}
			}
			return new Point(-1, -1);
		}
		
		public SideTabItem GetItemAt(int x, int y)
		{
			int index = ScrollIndex + y / ItemHeight;
			return (index >= 0 && index < Items.Count) ? (SideTabItem)Items[index] : null;
		}
		
		public SideTabItem GetItemAt(Point pos)
		{
			return GetItemAt(pos.X, pos.Y);
		}
		
		public int ItemHeight {
			get {
				return (int)(20 * (dpiY / 96)); ;
			}
		}
		
		public void DrawTabContent(Graphics g, Font f, Rectangle rectangle)
		{
			for (int i = 0; i + ScrollIndex < Items.Count; ++i) {
				SideTabItem item = (SideTabItem)Items[ScrollIndex + i];
				if (rectangle.Height < i * ItemHeight) {
					break;
				}
				item.DrawItem(g, f, new Rectangle(rectangle.X,
				                                  rectangle.Y + i * ItemHeight,
				                                  rectangle.Width,
				                                  ItemHeight));
			}
		}
		
		/// <summary>
		/// Swaps two side tab items with the given indexes.
		/// </summary>
		public void Exchange(int a, int b)
		{
			SideTabItem itemA = Items[a];
			SideTabItem itemB = Items[b];
			Items[a] = itemB;
			Items[b] = itemA;
			OnExchange(itemA, itemB);
		}
		
		void SetCanRename()
		{
			if (name != null && name.StartsWith("${res:")) {
				canBeRenamed = false;
			}
		}
		
		void OnSideTabItemRemoved(object source, SideTabItemEventArgs e)
		{
			if (ItemRemoved != null) {
				ItemRemoved(this, e);
			}
		}
		
		void OnExchange(SideTabItem item1, SideTabItem item2)
		{
			if (ItemsExchanged != null) {
				ItemsExchanged(this, new SideTabItemExchangeEventArgs(item1, item2));
			}
		}

		public class SideTabItemCollection : ICollection<SideTabItem>, IEnumerable<SideTabItem>
		{
			List<SideTabItem> list = new List<SideTabItem>();
			ISideTabItemFactory sideTabItemFactory = new DefaultSideTabItemFactory();
			
			public event SideTabItemEventHandler ItemRemoved;
			
			public ISideTabItemFactory SideTabItemFactory {
				get {
					return sideTabItemFactory;
				}
				set {
					sideTabItemFactory = value;
				}
			}
			
			public SideTabItemCollection()
			{
			}
			
			public SideTabItem this[int index] {
				get {
					return (SideTabItem)list[index];
				}
				set {
					list[index] = value;
				}
			}
			
			public int DraggedIndex {
				get {
					for (int i = 0; i < Count; ++i) {
						if (this[i].SideTabItemStatus == SideTabItemStatus.Drag)
							return i;
					}
					return -1;
				}
			}
			
			public int Count {
				get {
					return list.Count;
				}
			}
			
			public virtual bool IsSynchronized {
				get {
					return false;
				}
			}
			
			public virtual object SyncRoot {
				get {
					return this;
				}
			}
			
			public virtual void Add(SideTabItem item)
			{
				list.Add(item);
			}
			
			public virtual SideTabItem Add(string name, object content)
			{
				return Add(name, content, -1);
			}
			
			public virtual SideTabItem Add(string name, object content, int imageIndex)
			{
				SideTabItem item = sideTabItemFactory.CreateSideTabItem(name, imageIndex);
				item.Tag = content;
				Add(item);
				return item;
			}
			
			public virtual void Clear()
			{
				list.Clear();
			}
			
			public bool Contains(SideTabItem item)
			{
				return list.Contains(item);
			}
			
			public IEnumerator<SideTabItem> GetEnumerator()
			{
				return list.GetEnumerator();
			}
			
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return list.GetEnumerator();
			}
			
			public int IndexOf(SideTabItem item)
			{
				return list.IndexOf(item);
			}
			
			public void CopyTo(Array dest, int index)
			{
				list.CopyTo((SideTabItem[])dest, index);
			}
			
			public virtual SideTabItem Insert(int index, SideTabItem item)
			{
				list.Insert(index, item);
				return item;
			}
			
			public virtual SideTabItem Insert(int index, string name, object content)
			{
				return Insert(index, name, content, -1);
			}
			
			public virtual SideTabItem Insert(int index, string name, object content, int imageIndex)
			{
				SideTabItem item = sideTabItemFactory.CreateSideTabItem(name, imageIndex);
				item.Tag = content;
				return Insert(index, item);
			}
			
			public virtual bool Remove(SideTabItem item)
			{
				bool r = list.Remove(item);
				OnItemRemoved(item);
				return r;
			}
			
			public virtual void RemoveAt(int index)
			{
				if (index < 0 || index >= list.Count) {
					return;
				}
				SideTabItem item = this[index];
				list.Remove(item);
				OnItemRemoved(item);
			}
			
			void OnItemRemoved(SideTabItem item)
			{
				if (ItemRemoved != null) {
					ItemRemoved(this, new SideTabItemEventArgs(item));
				}
			}
			
			public bool IsReadOnly {
				get {
					return false;
				}
			}
			
			public void CopyTo(SideTabItem[] array, int arrayIndex)
			{
				list.CopyTo(array, arrayIndex);
			}
		}
	}
}
