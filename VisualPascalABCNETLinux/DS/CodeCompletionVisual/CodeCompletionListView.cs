// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 2533 $</version>
// </file>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor;

namespace ICSharpCode.TextEditor.Gui.CompletionWindow
{
	public class UserDefaultCompletionData : ICompletionData
	{
		string text;
		string description;
		int imageIndex;
        bool isExtension;

        
		public bool IsOnOverrideWindow=false;
		
		public bool FirstInsert = false;

        public bool IsExtension
        {
            get { return isExtension; }
            set { isExtension = value; }
        }

		public int ImageIndex {
			get {
				return imageIndex;
			}
		}
		
		public string Text {
			get {
				return text;
			}
			set {
				text = value;
			}
		}
		
		public string Description {
			get {
				return description;
			}
			set
			{
				description = value;
			}
		}
		
		double priority;
		
		public double Priority {
			get {
				return priority;
			}
			set {
				priority = value;
			}
		}
		
		public virtual bool InsertAction(TextArea textArea, char ch)
		{
			textArea.InsertString(text);
			return false;
		}
		
		public UserDefaultCompletionData(string text, string description, int imageIndex, bool is_in_override_window, bool is_extension=false)
		{
			this.text        = text;
			this.description = description;
            if (this.description != null)
                this.description = this.description.Replace("!#","");
			this.imageIndex  = imageIndex;
			this.IsOnOverrideWindow = is_in_override_window;
            this.isExtension = is_extension;
		}
		
		public int CompareTo(object obj)
		{
			if (obj == null || !(obj is UserDefaultCompletionData)) {
				return -1;
			}
			return string.Compare(text,((UserDefaultCompletionData)obj).Text,true);
		}
	}

	/// <summary>
	/// Description of CodeCompletionListView.
	/// </summary>
	public class PABCNETCodeCompletionListView: System.Windows.Forms.UserControl
	{
		ICompletionData[] completionData;
		int               firstItem    = 0;
		int               selectedItem = -1;
		ImageList         imageList;
		//Dictionary<ICompletionData,int> cache = new Dictionary<ICompletionData,int>();
		Hashtable cache = new Hashtable();
		
		public bool FirstInsert = false;
		
		public ImageList ImageList {
			get {
				return imageList;
			}
			set {
				imageList = value;
			}
		}
		
		public int FirstItem {
			get {
				return firstItem;
			}
			set {
				if (firstItem != value) {
					firstItem = value;
					OnFirstItemChanged(EventArgs.Empty);
				}
			}
		}
		
		public ICompletionData SelectedCompletionData {
			get {
				if (selectedItem < 0) {
					return null;
				}
				return completionData[selectedItem];
			}
		}
		
		public int ItemHeight {
			get {
				return Math.Max(imageList.ImageSize.Height, (int)(Font.Height * 1.1));
			}
		}
		
		public int MaxVisibleItem {
			get {
				return Height / ItemHeight;
			}
		}
		
		private ICompletionData[] SortByGroup(ICompletionData[] compData)
		{
			UserDefaultCompletionData ddd = null;
			List<ICompletionData> consts = new List<ICompletionData>();
			List<ICompletionData> meths = new List<ICompletionData>();
            List<ICompletionData> extension_meths = new List<ICompletionData>();
			List<ICompletionData> props = new List<ICompletionData>();
			List<ICompletionData> fields = new List<ICompletionData>();
			List<ICompletionData> vars = new List<ICompletionData>();
			List<ICompletionData> events = new List<ICompletionData>();
			List<ICompletionData> others = new List<ICompletionData>();
			List<ICompletionData> res = new List<ICompletionData>();
			foreach (ICompletionData data in compData)
			{
				if (data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberLocal) vars.Add(data);
				else if (data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberConstant) consts.Add(data);
                else if (data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberMethod ||
                         data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberInternalMethod ||
                         data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberPrivateMethod ||
                         data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberProtectedMethod)
                {
                    if (!data.Description.Contains("(" + PascalABCCompiler.StringResources.Get("CODE_COMPLETION_EXTENSION") + ")"))
                        meths.Add(data);
                    else
                        extension_meths.Add(data);
                }
                else if (data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberProperty ||
                         data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberInternalProperty ||
                         data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberPrivateProperty ||
                         data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberProtectedProperty)
                    props.Add(data);
                else if (data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberField ||
                         data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberInternalField ||
                         data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberPrivateField ||
                         data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberProtectedField)
                    fields.Add(data);
                else if (data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberEvent ||
                         data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberInternalEvent ||
                         data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberPrivateEvent ||
                         data.ImageIndex == VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberProtectedEvent)
                    events.Add(data);

                else
                    others.Add(data);
				
			}
			meths.Sort();
            extension_meths.Sort();
			props.Sort();
			fields.Sort();
			vars.Sort();
			consts.Sort();
			events.Sort();
			others.Sort();
			res.AddRange(vars);
			res.AddRange(consts);
			res.AddRange(fields);
			res.AddRange(props);
			res.AddRange(meths);
            
			res.AddRange(events);
			res.AddRange(others);
            res.AddRange(extension_meths);
			return res.ToArray();
		}
		
		public PABCNETCodeCompletionListView(ICompletionData[] completionData, bool is_by_dot)
		{
			if (VisualPascalABC.VisualPABCSingleton.MainForm.UserOptions.ShowCompletionInfoByGroup && is_by_dot)
				completionData = SortByGroup(completionData);
			else
				Array.Sort(completionData);
			this.completionData = completionData;
			for (int i=0; i<completionData.Length; i++)
				cache.Add(completionData[i],i);
//			this.KeyDown += new System.Windows.Forms.KeyEventHandler(OnKey);
//			SetStyle(ControlStyles.Selectable, false);
//			SetStyle(ControlStyles.UserPaint, true);
//			SetStyle(ControlStyles.DoubleBuffer, false);
		}
		
		public void Close()
		{
			if (completionData != null) {
				Array.Clear(completionData, 0, completionData.Length);
				cache.Clear();
			}
			base.Dispose();
		}
		
		public int GetIndexByData(ICompletionData data)
		{
			int ind = -1;
			object o = cache[data];
			if (o != null) return (int)o;
			return -1;
			//if (cache.TryGetValue(data,out ind)) return ind;
			//return -1;
		}
		
		public void SelectIndexByCompletionData(ICompletionData data)
		{
			int index = GetIndexByData(data);
			SelectIndex(index);
		}
		
		public void SelectIndex(int index)
		{
			int oldSelectedItem = selectedItem;
			int oldFirstItem    = firstItem;
			
			index = Math.Max(0, index);
			selectedItem = Math.Max(0, Math.Min(completionData.Length - 1, index));
			if (selectedItem < firstItem) {
				FirstItem = selectedItem;
			}
			if (firstItem + MaxVisibleItem <= selectedItem) {
				FirstItem = selectedItem - MaxVisibleItem + 1;
			}
			if (oldSelectedItem != selectedItem) {
				if (firstItem != oldFirstItem) {
					Invalidate();
				} else {
					int min = Math.Min(selectedItem, oldSelectedItem) - firstItem;
					int max = Math.Max(selectedItem, oldSelectedItem) - firstItem;
					Invalidate(new Rectangle(0, 1 + min * ItemHeight, Width, (max - min + 1) * ItemHeight));
				}
				OnSelectedItemChanged(EventArgs.Empty);
			}
		}
		
		public void CenterViewOn(int index)
		{
			int oldFirstItem = this.FirstItem;
			int firstItem = index - MaxVisibleItem / 2;
			if (firstItem < 0)
				this.FirstItem = 0;
			else if (firstItem >= completionData.Length - MaxVisibleItem)
				this.FirstItem = completionData.Length - MaxVisibleItem;
			else
				this.FirstItem = firstItem;
			if (this.FirstItem != oldFirstItem) {
				Invalidate();
			}
		}
		
		public void CalcWidth()
		{
			int len = Width;
			Graphics g = Graphics.FromHwnd(this.Handle);
			for (int i=0; i<this.completionData.Length; i++)
			{
				SizeF sz = g.MeasureString(completionData[i].Text,this.Font);
				if (len < sz.ToSize().Width+16)
					len = sz.ToSize().Width+16;
			}
			//Width = len;
			this.Parent.Width = len+6;
		}
		
		public void ClearSelection()
		{
			if (selectedItem < 0)
				return;
			int itemNum = selectedItem - firstItem;
			selectedItem = -1;
			Invalidate(new Rectangle(0, itemNum * ItemHeight, Width, (itemNum + 1) * ItemHeight + 1));
			Update();
			OnSelectedItemChanged(EventArgs.Empty);
		}
		
		public void PageDown()
		{
			SelectIndex(selectedItem + MaxVisibleItem);
		}
		
		public void PageUp()
		{
			SelectIndex(selectedItem - MaxVisibleItem);
		}
		
		public void SelectNextItem()
		{
			SelectIndex(selectedItem + 1);
		}
		
		public void SelectPrevItem()
		{
			SelectIndex(selectedItem - 1);
		}
		
		public void SelectItemWithStart(string startText)
		{
			if (startText == null || startText.Length == 0) 
				return;
			if (this.SelectedCompletionData != null && this.FirstInsert)
			{
				this.FirstInsert = false;
				return;
			}
			string originalStartText = startText;
			startText = startText.ToLower();
			int bestIndex = -1;
			int bestQuality = -1;
			// Qualities: 0 = match start
			//            1 = match start case sensitive
			//            2 = full match
			//            3 = full match case sensitive
			double bestPriority = 0;
			for (int i = 0; i < completionData.Length; ++i) {
				string itemText = completionData[i].Text;
				string lowerText = itemText.ToLower();
				if (lowerText.StartsWith(startText)) {
					double priority = completionData[i].Priority;
					int quality;
					if (lowerText == startText) {
						if (itemText == originalStartText)
							quality = 3;
						else
							quality = 2;
					} else if (itemText.StartsWith(originalStartText)) {
						quality = 1;
					} else {
						quality = 0;
					}
					bool useThisItem;
					if (bestQuality < quality) {
						useThisItem = true;
					} else {
						if (bestIndex == selectedItem) {
							useThisItem = false;
						} else if (i == selectedItem) {
							useThisItem = bestQuality == quality;
						} else {
							useThisItem = bestQuality == quality && bestPriority < priority;
						}
					}
					if (useThisItem) {
						bestIndex = i;
						bestPriority = priority;
						bestQuality = quality;
					}
				}
			}
			if (bestIndex < 0) {
				ClearSelection();
			} else {
				if (bestIndex < firstItem || firstItem + MaxVisibleItem <= bestIndex) {
					SelectIndex(bestIndex);
					CenterViewOn(bestIndex);
				} else {
					SelectIndex(bestIndex);
				}
			}
		}
		
		protected override void OnPaint(PaintEventArgs pe)
		{
			float yPos       = 1;
			float itemHeight = ItemHeight;
            int dfont = Math.Max((int)((itemHeight-Font.Height)/2), 0);
			// Maintain aspect ratio
			int imageWidth = (int)(itemHeight * imageList.ImageSize.Width / imageList.ImageSize.Height);
			int curItem = firstItem;
			Graphics g  = pe.Graphics;
			//g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			while (curItem < completionData.Length && yPos < Height) {
                RectangleF drawingBackground = new RectangleF(1, yPos, Width - 2, itemHeight);
                if (drawingBackground.IntersectsWith(pe.ClipRectangle))
                {
                    bool imageExists = imageList != null && completionData[curItem].ImageIndex < imageList.Images.Count;
                    RectangleF drawingBackgroundText = new RectangleF(imageExists ? imageWidth+3 : 1, yPos, Width - 2, itemHeight);
                    // draw Background
                    if (curItem == selectedItem)
                    {
                        g.FillRectangle(SystemBrushes.Window, drawingBackground);
                        g.FillRectangle(SystemBrushes.Highlight, drawingBackgroundText);
                        //Pen pen = new Pen(Color.Black); pen.DashStyle;
                        //g.DrawRectangle(pen, drawingBackgroundText.X, drawingBackgroundText.Y, drawingBackgroundText.Width, drawingBackgroundText.Height);
                    }
                    else
                    {
                        g.FillRectangle(SystemBrushes.Window, drawingBackground);
                    }
					
					// draw Icon
					int   xPos   = 0;
                    if (imageExists && completionData[curItem].ImageIndex != -1)
                    {
						g.DrawImage(imageList.Images[completionData[curItem].ImageIndex], new RectangleF(2, yPos, imageWidth, itemHeight));
						xPos = imageWidth+4;
					}
					// draw text
					if (curItem == selectedItem) {
                        g.DrawString(completionData[curItem].Text, Font, SystemBrushes.HighlightText, xPos, yPos + dfont);
					} else {
                        g.DrawString(completionData[curItem].Text, Font, SystemBrushes.WindowText, xPos, yPos + dfont);
					}
				}

                yPos += itemHeight;
				++curItem;
			}
			g.DrawRectangle(SystemPens.Control, new Rectangle(0, 0, Width - 1, Height - 1));
		}
		
		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			float yPos       = 1;
			int curItem = firstItem;
			float itemHeight = ItemHeight;
			
			while (curItem < completionData.Length && yPos < Height) {
				RectangleF drawingBackground = new RectangleF(1, yPos, Width - 2, itemHeight);
				if (drawingBackground.Contains(e.X, e.Y)) {
					SelectIndex(curItem);
					break;
				}
				yPos += itemHeight;
				++curItem;
			}
		}
		
		protected override void OnPaintBackground(PaintEventArgs pe)
		{
		}
		
		protected virtual void OnSelectedItemChanged(EventArgs e)
		{
			if (SelectedItemChanged != null) {
				SelectedItemChanged(this, e);
			}
		}
		
		protected virtual void OnFirstItemChanged(EventArgs e)
		{
			if (FirstItemChanged != null) {
				FirstItemChanged(this, e);
			}
		}
		
		public event EventHandler SelectedItemChanged;
		public event EventHandler FirstItemChanged;
	}
}
