﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 2683 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;

using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace ICSharpCode.TextEditor
{
	public delegate bool KeyEventHandler(char ch);
	public delegate bool DialogKeyProcessor(Keys keyData);

    /// <summary>
    /// This class paints the textarea.
    /// </summary>
    [ToolboxItem(false)]
    public class TextArea : Control
    {
        bool hiddenMouseCursor = false;
        /// <summary>
        /// The position where the mouse cursor was when it was hidden. Sometimes the text editor gets MouseMove
        /// events when typing text even if the mouse is not moved.
        /// </summary>
        Point mouseCursorHidePosition;

        Point virtualTop = new Point(0, 0);
        TextAreaControl motherTextAreaControl;
        TextEditorControl motherTextEditorControl;

        List<BracketHighlightingSheme> bracketshemes = new List<BracketHighlightingSheme>();
        TextAreaClipboardHandler textAreaClipboardHandler;
        bool autoClearSelection = false;

        List<AbstractMargin> leftMargins = new List<AbstractMargin>();

        TextView textView;
        GutterMargin gutterMargin;
        FoldMargin foldMargin;
        IconBarMargin iconBarMargin;

        SelectionManager selectionManager;
        Caret caret;

        internal Point mousepos = new Point(0, 0);
        //public Point selectionStartPos = new Point(0,0);

        bool disposed;

        [Browsable(false)]
        public IList<AbstractMargin> LeftMargins {
            get {
                return leftMargins.AsReadOnly();
            }
        }

        public void InsertLeftMargin(int index, AbstractMargin margin)
        {
            leftMargins.Insert(index, margin);
            Refresh();
        }

        public TextEditorControl MotherTextEditorControl {
            get {
                return motherTextEditorControl;
            }
        }

        public TextAreaControl MotherTextAreaControl {
            get {
                return motherTextAreaControl;
            }
        }

        public SelectionManager SelectionManager {
            get {
                return selectionManager;
            }
        }

        public Caret Caret {
            get {
                return caret;
            }
        }

        public TextView TextView {
            get {
                return textView;
            }
        }

        public GutterMargin GutterMargin {
            get {
                return gutterMargin;
            }
        }

        public FoldMargin FoldMargin {
            get {
                return foldMargin;
            }
        }

        public IconBarMargin IconBarMargin {
            get {
                return iconBarMargin;
            }
        }

        public Encoding Encoding {
            get {
                return motherTextEditorControl.Encoding;
            }
        }
        public int MaxVScrollValue {
            get {
                return (Document.GetVisibleLine(Document.TotalNumberOfLines - 1) + 1 + TextView.VisibleLineCount * 2 / 3) * TextView.FontHeight;
            }
        }

        public Point VirtualTop {
            get {
                return virtualTop;
            }
            set {
                Point newVirtualTop = new Point(value.X, Math.Min(MaxVScrollValue, Math.Max(0, value.Y)));
                if (virtualTop != newVirtualTop) {
                    virtualTop = newVirtualTop;
                    motherTextAreaControl.VScrollBar.Value = virtualTop.Y;
                    Invalidate();
                }
            }
        }

        public bool AutoClearSelection {
            get {
                return autoClearSelection;
            }
            set {
                autoClearSelection = value;
            }
        }

        [Browsable(false)]
        public IDocument Document {
            get {
                return motherTextEditorControl.Document;
            }
        }

        public TextAreaClipboardHandler ClipboardHandler {
            get {
                return textAreaClipboardHandler;
            }
        }


        public ITextEditorProperties TextEditorProperties {
            get {
                return motherTextEditorControl.TextEditorProperties;
            }
        }

        // У каждого редактора - свой таймер
        // Альтернатива - сделать создание таймера по OnHover и удаление его в конце своего срока
        private Timer HoverTimer = null;

        public TextArea(TextEditorControl motherTextEditorControl, TextAreaControl motherTextAreaControl)
		{
            HoverTimer = new Timer();
            HoverTimer.Interval = 400;
            HoverTimer.Tick += new EventHandler(HoverTimerEventProcessor);
            HoverTimer.Start();

            this.motherTextAreaControl      = motherTextAreaControl;
			this.motherTextEditorControl    = motherTextEditorControl;
			
			caret            = new Caret(this);
			selectionManager = new SelectionManager(Document, this);
			
			this.textAreaClipboardHandler = new TextAreaClipboardHandler(this);
			
			ResizeRedraw = true;
			
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
//			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
//			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.Opaque, false);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.Selectable, true);
			
			textView = new TextView(this);
			
			gutterMargin = new GutterMargin(this);
			foldMargin   = new FoldMargin(this);
			iconBarMargin = new IconBarMargin(this);
			leftMargins.AddRange(new AbstractMargin[] { iconBarMargin, gutterMargin, foldMargin });
			OptionsChanged();
			
			
			new TextAreaMouseHandler(this).Attach();
			new TextAreaDragDropHandler().Attach(this);
			
			bracketshemes.Add(new BracketHighlightingSheme('{', '}'));
			bracketshemes.Add(new BracketHighlightingSheme('(', ')'));
			bracketshemes.Add(new BracketHighlightingSheme('[', ']'));
			
			caret.PositionChanged += new EventHandler(SearchMatchingBracket);
			Document.TextContentChanged += new EventHandler(TextContentChanged);
			Document.FoldingManager.FoldingsChanged += new EventHandler(DocumentFoldingsChanged);

        }
		
		public void UpdateMatchingBracket()
		{
			SearchMatchingBracket(null, null);
		}
		
		void TextContentChanged(object sender, EventArgs e)
		{
			Caret.Position = new TextLocation(0, 0);
			SelectionManager.SelectionCollection.Clear();
		}
		void SearchMatchingBracket(object sender, EventArgs e)
		{
			if (!TextEditorProperties.ShowMatchingBracket) {
				textView.Highlight = null;
				return;
			}
			int oldLine1 = -1, oldLine2 = -1;
			if (textView.Highlight != null && textView.Highlight.OpenBrace.Y >=0 && textView.Highlight.OpenBrace.Y < Document.TotalNumberOfLines) {
				oldLine1 = textView.Highlight.OpenBrace.Y;
			}
			if (textView.Highlight != null && textView.Highlight.CloseBrace.Y >=0 && textView.Highlight.CloseBrace.Y < Document.TotalNumberOfLines) {
				oldLine2 = textView.Highlight.CloseBrace.Y;
			}
			textView.Highlight = FindMatchingBracketHighlight();
			if (oldLine1 >= 0)
				UpdateLine(oldLine1);
			if (oldLine2 >= 0 && oldLine2 != oldLine1)
				UpdateLine(oldLine2);
			if (textView.Highlight != null) {
				int newLine1 = textView.Highlight.OpenBrace.Y;
				int newLine2 = textView.Highlight.CloseBrace.Y;
				if (newLine1 != oldLine1 && newLine1 != oldLine2)
					UpdateLine(newLine1);
				if (newLine2 != oldLine1 && newLine2 != oldLine2 && newLine2 != newLine1)
					UpdateLine(newLine2);
			}
		}
		
		public Highlight FindMatchingBracketHighlight()
		{
			if (Caret.Offset == 0)
				return null;
			foreach (BracketHighlightingSheme bracketsheme in bracketshemes) {
				Highlight highlight = bracketsheme.GetHighlight(Document, Caret.Offset - 1);
				if (highlight != null) {
					return highlight;
				}
			}
			return null;
		}
		
		public void SetDesiredColumn()
		{
			Caret.DesiredColumn = TextView.GetDrawingXPos(Caret.Line, Caret.Column) + VirtualTop.X;
		}
		
		public void SetCaretToDesiredColumn()
		{
			FoldMarker dummy;
			Caret.Position = textView.GetLogicalColumn(Caret.Line, Caret.DesiredColumn + VirtualTop.X, out dummy);
		}
		
		public void OptionsChanged()
		{
			UpdateMatchingBracket();
			textView.OptionsChanged();
			caret.RecreateCaret();
			caret.UpdateCaretPosition();
			Refresh();
		}
		
		AbstractMargin lastMouseInMargin;
		
		protected override void OnMouseLeave(System.EventArgs e)
		{
			base.OnMouseLeave(e);
			this.Cursor = Cursors.Default;
			if (lastMouseInMargin != null) {
				lastMouseInMargin.HandleMouseLeave(EventArgs.Empty);
				lastMouseInMargin = null;
			}
			CloseToolTip();
		}
		
		internal bool on_icon_bar_clicked(System.Windows.Forms.MouseEventArgs e)
		{
			mousepos = new Point(e.X, e.Y);
				foreach (AbstractMargin margin in leftMargins) {
				if (margin.DrawingPosition.Contains(e.X, e.Y) && margin is IconBarMargin) {
					return true;
				}
				}
			return false;
		}

        // SSM 11.07.22
        protected MouseButtons MyMouseButtons = MouseButtons.None;
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            MyMouseButtons = MouseButtons.None;
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			// this corrects weird problems when text is selected,
			// then a menu item is selected, then the text is
			// clicked again - it correctly synchronises the
			// click position
			mousepos = new Point(e.X, e.Y);
            // SSM 11.07.22
            MyMouseButtons = e.Button;
//			if (e.Button == MouseButtons.Right)
//			{
//				foreach (AbstractMargin margin in leftMargins) {
//				if (margin.DrawingPosition.Contains(e.X, e.Y) && margin is IconBarMargin) {
//					margin.HandleMouseDown(new Point(e.X, e.Y), e.Button);
//					CloseToolTip();
//					return;
//				}
//				}
//			}
			base.OnMouseDown(e);
			CloseToolTip();
			
			foreach (AbstractMargin margin in leftMargins) {
				if (margin.DrawingPosition.Contains(e.X, e.Y) && margin.IsVisible) {
					margin.HandleMouseDown(new Point(e.X, e.Y), e.Button);
				}
			}
		}
		
		/// <summary>
		/// Shows the mouse cursor if it has been hidden.
		/// </summary>
		/// <param name="forceShow"><c>true</c> to always show the cursor or <c>false</c> to show it only if it has been moved since it was hidden.</param>
		internal void ShowHiddenCursor(bool forceShow)
		{
			if (hiddenMouseCursor) {
				if (mouseCursorHidePosition != Cursor.Position || forceShow) {
					Cursor.Show();
					hiddenMouseCursor = false;
				}
			}
		}
		
		
		// static because the mouse can only be in one text area and we don't want to have
		// tooltips of text areas from inactive tabs floating around.
		static DeclarationViewWindow toolTip;
		static string oldToolTip;
		
		void SetToolTip(string text, int lineNumber)
		{
			if (toolTip == null || toolTip.IsDisposed)
			{
				toolTip = new DeclarationViewWindow(this.FindForm());
				toolTip.TextEditorControl = motherTextEditorControl;
			}
				
			if (oldToolTip == text)
				return;
			if (text == null || text.Trim().Length == 0) {
				toolTip.Hide();
			} else {
				Point p = Control.MousePosition;
				Point cp = PointToClient(p);
				if (lineNumber >= 0) {
					lineNumber = this.Document.GetVisibleLine(lineNumber);
					p.Y = (p.Y - cp.Y) + (lineNumber * this.TextView.FontHeight) - this.virtualTop.Y;
				}
				p.Offset(3, 3);
				
				toolTip.Location = p;
				toolTip.Description = text;
				toolTip.HideOnClick = true;
				toolTip.Show();
			}
			oldToolTip = text;
		}
		
		public event ToolTipRequestEventHandler ToolTipRequest;
		
		protected virtual void OnToolTipRequest(ToolTipRequestEventArgs e)
		{
			if (ToolTipRequest != null) {
				ToolTipRequest(this, e);
			}
		}
		
		bool toolTipActive;
		/// <summary>
		/// Rectangle in text area that caused the current tool tip.
		/// Prevents tooltip from re-showing when it was closed because of a click or keyboard
		/// input and the mouse was not used.
		/// </summary>
		Rectangle toolTipRectangle;
		
		void CloseToolTip()
		{
			if (toolTipActive) {
				toolTipActive = false;
				SetToolTip(null, -1);
			}
			ResetMouseEventArgs();
		}

        protected override void OnMouseHover(EventArgs e)
		{
            //System.IO.File.AppendAllText("d:\\logP.txt", "OnMouseHover" + $" {Control.MousePosition.ToString()} {DateTime.Now}\n");
            //MessageBox.Show("OnMHOne");
            //MessageBox.Show(MouseButtons.ToString());
            // Под Mono MouseButtons возвращает цену на дрова

            // SSM 11/07/22 - в Mono OnMouseHower вызывается единожды

            base.OnMouseHover(e);
            if (MyMouseButtons == MouseButtons.None) {
                RequestToolTip(PointToClient(Control.MousePosition));
			} else {
				CloseToolTip();
			}
		}

		protected void RequestToolTip(Point mousePos)
		{
			//MessageBox.Show("RequestToolTip");
			if (toolTipRectangle.Contains(mousePos)) {
				if (!toolTipActive)
					ResetMouseEventArgs();
				return;
			}
			
			//Console.WriteLine("Request tooltip for " + mousePos);
			
			toolTipRectangle = new Rectangle(mousePos.X - 4, mousePos.Y - 4, 8, 8);
			
			TextLocation logicPos = textView.GetLogicalPosition(mousePos.X - textView.DrawingPosition.Left,
			                                                    mousePos.Y - textView.DrawingPosition.Top);
			bool inDocument = textView.DrawingPosition.Contains(mousePos)
				&& logicPos.Y >= 0 && logicPos.Y < Document.TotalNumberOfLines;
			ToolTipRequestEventArgs args = new ToolTipRequestEventArgs(mousePos, logicPos, inDocument);
			OnToolTipRequest(args);
			if (args.ToolTipShown) {
				//Console.WriteLine("Set tooltip to " + args.toolTipText);
				toolTipActive = true;
				SetToolTip(args.toolTipText, inDocument ? logicPos.Y + 1 : -1);
			} else {
				CloseToolTip();
			}
		}
		
		// external interface to the attached event
		internal void RaiseMouseMove(MouseEventArgs e)
		{
			OnMouseMove(e);
		}

		private int mx = -1;
		private int my = -1;
        private int prevmx = -2;
        private int prevmy = -2;
        //private DateTime dt0 = DateTime.Now;
        //int deltaTime = 0;

        private void HoverTimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            if (mx == prevmx && my == prevmy)
            {
                OnMouseHover(EventArgs.Empty);
            }
            prevmx = mx;
            prevmy = my;
        }

        protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
            mx = e.X;
            my = e.Y;
            // Эмуляция MouseHower. Создадим таймер и будем его проверять каждые 400 мс. 
            // Если координаты мыши за это время не меняются, то генерим событие OnMouseHower
            // 

            /*if (e.X == mx && e.Y == my)
            {
				if (deltaTime < 2000)
                {
					deltaTime = (DateTime.Now - dt0).Milliseconds;
					//System.IO.File.AppendAllText("d:\\logP.txt", "OnMouseMove" + $" {deltaTime} {DateTime.Now}\n");
				}
				else
                {
					//System.IO.File.AppendAllText("d:\\logP.txt", "OnMouseMove22222" + $" {deltaTime} {DateTime.Now}\n");
					deltaTime = 0;
					// MouseHower аналог
					OnMouseHover(EventArgs.Empty);
				}
            }
			else
            {
				deltaTime = 0;
				// Снова ждать 400 мс
			}

            mx = e.X;
            my = e.Y;*/


            if (!toolTipRectangle.Contains(e.Location)) 
			{
                // Так было в оригинальном коде
				toolTipRectangle = Rectangle.Empty;
                if (toolTipActive)
                    RequestToolTip(e.Location);
                /*if (deltaTime < 400)
				{
					deltaTime = (DateTime.Now - dt0).Milliseconds;
				}
				else
                {
					deltaTime = 0;
					dt0 = DateTime.Now;
                    //if (toolTipActive) // Что то с этим условием не так
                    //toolTipActive = true;
                    //MessageBox.Show("RequestToolTip");
                    //-------------------- ssm 11/07/22 - пока закомментил эту ерунду
                    // пока вернулся к старой проблеме - OnMouseHover срабатывает лишь 1 раз
                    //RequestToolTip(e.Location);
				}*/
			}
			foreach (AbstractMargin margin in leftMargins) {
				if (margin.DrawingPosition.Contains(e.X, e.Y) && margin.IsVisible) {
					this.Cursor = margin.Cursor;
					margin.HandleMouseMove(new Point(e.X, e.Y), e.Button);
					if (lastMouseInMargin != margin) {
						if (lastMouseInMargin != null) {
							lastMouseInMargin.HandleMouseLeave(EventArgs.Empty);
						}
						lastMouseInMargin = margin;
					}
					return;
				}
			}
			if (lastMouseInMargin != null) {
				lastMouseInMargin.HandleMouseLeave(EventArgs.Empty);
				lastMouseInMargin = null;
			}
			if (textView.DrawingPosition.Contains(e.X, e.Y)) {
				TextLocation realmousepos = TextView.GetLogicalPosition(e.X - TextView.DrawingPosition.X, e.Y - TextView.DrawingPosition.Y);
				if(SelectionManager.IsSelected(Document.PositionToOffset(realmousepos)) && MouseButtons == MouseButtons.None) {
					// mouse is hovering over a selection, so show default mouse
					this.Cursor = Cursors.Default;
				} else {
					// mouse is hovering over text area, not a selection, so show the textView cursor
					this.Cursor = textView.Cursor;
				}
				return;
			}
			this.Cursor = Cursors.Default;
		}
		AbstractMargin updateMargin = null;
		
		public void Refresh(AbstractMargin margin)
		{
			updateMargin = margin;
			Invalidate(updateMargin.DrawingPosition);
			Update();
			updateMargin = null;
		}
		
		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
		{
		}
		
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			int currentXPos = 0;
			int currentYPos = 0;
			bool adjustScrollBars = false;
			Graphics  g             = e.Graphics;
			Rectangle clipRectangle = e.ClipRectangle;
			
			bool isFullRepaint = clipRectangle.X == 0 && clipRectangle.Y == 0
				&& clipRectangle.Width == this.Width && clipRectangle.Height == this.Height;
			
			g.TextRenderingHint = this.TextEditorProperties.TextRenderingHint;
			
			if (updateMargin != null) {
				updateMargin.Paint(g, updateMargin.DrawingPosition);
//				clipRectangle.Intersect(updateMargin.DrawingPosition);
			}
			
			if (clipRectangle.Width <= 0 || clipRectangle.Height <= 0) {
				return;
			}
			
			foreach (AbstractMargin margin in leftMargins) {
				if (margin.IsVisible) {
					Rectangle marginRectangle = new Rectangle(currentXPos , currentYPos, margin.Size.Width, Height - currentYPos);
					if (marginRectangle != margin.DrawingPosition) {
						// margin changed size
						if (!isFullRepaint && !clipRectangle.Contains(marginRectangle)) {
							Invalidate(); // do a full repaint
						}
						adjustScrollBars = true;
						margin.DrawingPosition = marginRectangle;
					}
					currentXPos += margin.DrawingPosition.Width;
					if (clipRectangle.IntersectsWith(marginRectangle)) {
						marginRectangle.Intersect(clipRectangle);
						if (!marginRectangle.IsEmpty) {
							margin.Paint(g, marginRectangle);
						}
					}
				}
			}

			Rectangle textViewArea = new Rectangle(currentXPos, currentYPos, Width - currentXPos, Height - currentYPos);
			if (textViewArea != textView.DrawingPosition) {
				adjustScrollBars = true;
				textView.DrawingPosition = textViewArea;
			}
			if (clipRectangle.IntersectsWith(textViewArea)) {
				textViewArea.Intersect(clipRectangle);
				if (!textViewArea.IsEmpty) {
					textView.Paint(g, textViewArea);
				}
			}
			
			if (adjustScrollBars) {
				this.motherTextAreaControl.AdjustScrollBars();
			}
			
			Caret.UpdateCaretPosition();
			
			base.OnPaint(e);
		}
		void DocumentFoldingsChanged(object sender, EventArgs e)
		{
			Invalidate();
			this.motherTextAreaControl.AdjustScrollBars();
		}
		
		#region keyboard handling methods
		
		/// <summary>
		/// This method is called on each Keypress
		/// </summary>
		/// <returns>
		/// True, if the key is handled by this method and should NOT be
		/// inserted in the textarea.
		/// </returns>
		protected internal virtual bool HandleKeyPress(char ch)
		{
			if (KeyEventHandler != null) {
				return KeyEventHandler(ch);
			}
			return false;
		}
		
		// Fixes SD2-747: Form containing the text editor and a button with a shortcut
		protected override bool IsInputChar(char charCode)
		{
			return true;
		}
		
		public void SimulateKeyPress(char ch)
		{
			if (Document.ReadOnly) {
				return;
			}
			
			if (TextEditorProperties.UseCustomLine == true) {
				if (SelectionManager.HasSomethingSelected) {
					if (SelectionManager.SelectionIsReadonly)
						return;
				} else if (Document.CustomLineManager.IsReadOnly(Caret.Line, false) == true)
					return;
			}
			
			if (ch < ' ') {
				return;
			}
			
			if (!hiddenMouseCursor && TextEditorProperties.HideMouseCursor) {
				if (this.ClientRectangle.Contains(PointToClient(Cursor.Position))) {
					mouseCursorHidePosition = Cursor.Position;
					hiddenMouseCursor = true;
					Cursor.Hide();
				}
			}
			CloseToolTip();
			
			motherTextEditorControl.BeginUpdate();
			Document.UndoStack.StartUndoGroup();
			// INSERT char
			if (!HandleKeyPress(ch)) {
				switch (Caret.CaretMode) {
					case CaretMode.InsertMode:
						InsertChar(ch);
						break;
					case CaretMode.OverwriteMode:
						ReplaceChar(ch);
						break;
					default:
						Debug.Assert(false, "Unknown caret mode " + Caret.CaretMode);
						break;
				}
			}
			
			int currentLineNr = Caret.Line;
			Document.FormattingStrategy.FormatLine(this, currentLineNr, Document.PositionToOffset(Caret.Position), ch);
			
			motherTextEditorControl.EndUpdate();
			Document.UndoStack.EndUndoGroup();
		}
		
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			SimulateKeyPress(e.KeyChar);
			e.Handled = true;
		}
		
		/// <summary>
		/// This method executes a dialog key
		/// </summary>
		public bool ExecuteDialogKey(Keys keyData)
		{
			// try, if a dialog key processor was set to use this
			if (DoProcessDialogKey != null && DoProcessDialogKey(keyData)) {
				return true;
			}
			
			if (keyData == Keys.Back || keyData == Keys.Delete || keyData == Keys.Enter) {
				if (TextEditorProperties.UseCustomLine == true) {
					if (SelectionManager.HasSomethingSelected) {
						if (SelectionManager.SelectionIsReadonly)
							return true;
					} else {
						int curLineNr   = Document.GetLineNumberForOffset(Caret.Offset);
						if (Document.CustomLineManager.IsReadOnly(curLineNr, false) == true)
							return true;
						if ((Caret.Column == 0) && (curLineNr - 1 >= 0) && keyData == Keys.Back &&
						    Document.CustomLineManager.IsReadOnly(curLineNr - 1, false) == true)
							return true;
						if (keyData == Keys.Delete) {
							LineSegment curLine = Document.GetLineSegment(curLineNr);
							if (curLine.Offset + curLine.Length == Caret.Offset &&
							    Document.CustomLineManager.IsReadOnly(curLineNr + 1, false) == true) {
								return true;
							}
						}
					}
				}
			}
			
			// if not (or the process was 'silent', use the standard edit actions
			IEditAction action =  motherTextEditorControl.GetEditAction(keyData);
			AutoClearSelection = true;
			if (action != null) {
				motherTextEditorControl.BeginUpdate();
				try {
					lock (Document) {
						action.Execute(this);
						if (SelectionManager.HasSomethingSelected && AutoClearSelection /*&& caretchanged*/) {
							if (Document.TextEditorProperties.DocumentSelectionMode == DocumentSelectionMode.Normal) {
								SelectionManager.ClearSelection();
							}
						}
					}
				} finally {
					motherTextEditorControl.EndUpdate();
					Caret.UpdateCaretPosition();
				}
				return true;
			}
			return false;
		}
		
		protected override bool ProcessDialogKey(Keys keyData)
		{
			return ExecuteDialogKey(keyData) || base.ProcessDialogKey(keyData);
		}
		#endregion
		
		public void ScrollToCaret()
		{
			motherTextAreaControl.ScrollToCaret();
		}
		
		public void ScrollTo(int line)
		{
			motherTextAreaControl.ScrollTo(line);
		}
		
		public void BeginUpdate()
		{
			motherTextEditorControl.BeginUpdate();
		}
		
		public void EndUpdate()
		{
			motherTextEditorControl.EndUpdate();
		}
		
		public bool EnableCutOrPaste {
			get {
				if (motherTextAreaControl == null)
					return false;
				if (TextEditorProperties.UseCustomLine == true) {
					if (SelectionManager.HasSomethingSelected == true) {
						if (SelectionManager.SelectionIsReadonly)
							return false;
					}
					if (Document.CustomLineManager.IsReadOnly(Caret.Line, false) == true)
						return false;
				}
				return true;
				
			}
		}
		
		string GenerateWhitespaceString(int length)
		{
			return new String(' ', length);
		}
		/// <remarks>
		/// Inserts a single character at the caret position
		/// </remarks>
		public void InsertChar(char ch)
		{
			bool updating = motherTextEditorControl.IsInUpdate;
			if (!updating) {
				BeginUpdate();
			}
			
			// filter out forgein whitespace chars and replace them with standard space (ASCII 32)
			if (Char.IsWhiteSpace(ch) && ch != '\t' && ch != '\n') {
				ch = ' ';
			}
			
			Document.UndoStack.StartUndoGroup();
			if (Document.TextEditorProperties.DocumentSelectionMode == DocumentSelectionMode.Normal &&
			    SelectionManager.SelectionCollection.Count > 0) {
				Caret.Position = SelectionManager.SelectionCollection[0].StartPosition;
				SelectionManager.RemoveSelectedText();
			}
			LineSegment caretLine = Document.GetLineSegment(Caret.Line);
			int offset = Caret.Offset;
			// use desired column for generated whitespaces
			int dc=Math.Min(Caret.Column,Caret.DesiredColumn);
			if (caretLine.Length < dc && ch != '\n') {
				Document.Insert(offset, GenerateWhitespaceString(dc - caretLine.Length) + ch);
			} else {
				Document.Insert(offset, ch.ToString());
			}
			Document.UndoStack.EndUndoGroup();
			++Caret.Column;
			
			if (!updating) {
				EndUpdate();
				UpdateLineToEnd(Caret.Line, Caret.Column);
			}
			
			// I prefer to set NOT the standard column, if you type something
//			++Caret.DesiredColumn;
		}
		
		/// <remarks>
		/// Inserts a whole string at the caret position
		/// </remarks>
		public void InsertString(string str)
		{
			bool updating = motherTextEditorControl.IsInUpdate;
			if (!updating) {
				BeginUpdate();
			}
			try {
				Document.UndoStack.StartUndoGroup();
				if (Document.TextEditorProperties.DocumentSelectionMode == DocumentSelectionMode.Normal &&
				    SelectionManager.SelectionCollection.Count > 0) {
					Caret.Position = SelectionManager.SelectionCollection[0].StartPosition;
					SelectionManager.RemoveSelectedText();
				}
				
				int oldOffset = Document.PositionToOffset(Caret.Position);
				int oldLine   = Caret.Line;
				LineSegment caretLine = Document.GetLineSegment(Caret.Line);
				if (caretLine.Length < Caret.Column) {
					int whiteSpaceLength = Caret.Column - caretLine.Length;
					Document.Insert(oldOffset, GenerateWhitespaceString(whiteSpaceLength) + str);
					Caret.Position = Document.OffsetToPosition(oldOffset + str.Length + whiteSpaceLength);
				} else {
					Document.Insert(oldOffset, str);
					Caret.Position = Document.OffsetToPosition(oldOffset + str.Length);
				}
				Document.UndoStack.EndUndoGroup();
				if (oldLine != Caret.Line) {
					UpdateToEnd(oldLine);
				} else {
					UpdateLineToEnd(Caret.Line, Caret.Column);
				}
			} finally {
				if (!updating) {
                    Document.UndoStack.EndUndoGroup();
					EndUpdate();
				}
			}
		}
		
		/// <remarks>
		/// Replaces a char at the caret position
		/// </remarks>
		public void ReplaceChar(char ch)
		{
			bool updating = motherTextEditorControl.IsInUpdate;
			if (!updating) {
				BeginUpdate();
			}
			if (Document.TextEditorProperties.DocumentSelectionMode == DocumentSelectionMode.Normal && SelectionManager.SelectionCollection.Count > 0) {
				Caret.Position = SelectionManager.SelectionCollection[0].StartPosition;
				SelectionManager.RemoveSelectedText();
			}
			
			int lineNr   = Caret.Line;
			LineSegment  line = Document.GetLineSegment(lineNr);
			int offset = Document.PositionToOffset(Caret.Position);
			if (offset < line.Offset + line.Length) {
				Document.Replace(offset, 1, ch.ToString());
			} else {
				Document.Insert(offset, ch.ToString());
			}
			if (!updating) {
				EndUpdate();
				UpdateLineToEnd(lineNr, Caret.Column);
			}
			++Caret.Column;
//			++Caret.DesiredColumn;
		}
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing) {
				if (!disposed) {
					disposed = true;
					if (caret != null) {
						caret.PositionChanged -= new EventHandler(SearchMatchingBracket);
						caret.Dispose();
					}
					if (selectionManager != null) {
						selectionManager.Dispose();
					}
					Document.TextContentChanged -= new EventHandler(TextContentChanged);
					Document.FoldingManager.FoldingsChanged -= new EventHandler(DocumentFoldingsChanged);
					motherTextAreaControl = null;
					motherTextEditorControl = null;
					foreach (AbstractMargin margin in leftMargins) {
						if (margin is IDisposable)
							(margin as IDisposable).Dispose();
					}
					textView.Dispose();
				}
			}
            HoverTimer.Stop();
            HoverTimer.Dispose();
        }
		
		#region UPDATE Commands
		internal void UpdateLine(int line)
		{
			UpdateLines(0, line, line);
		}
		
		internal void UpdateLines(int lineBegin, int lineEnd)
		{
			UpdateLines(0, lineBegin, lineEnd);
		}
		
		internal void UpdateToEnd(int lineBegin)
		{
//			if (lineBegin > FirstPhysicalLine + textView.VisibleLineCount) {
//				return;
//			}
			
			lineBegin = Document.GetVisibleLine(lineBegin);
			int y         = Math.Max(    0, (int)(lineBegin * textView.FontHeight));
			y = Math.Max(0, y - this.virtualTop.Y);
			Rectangle r = new Rectangle(0,
			                            y,
			                            Width,
			                            Height - y);
			Invalidate(r);
		}
		
		internal void UpdateLineToEnd(int lineNr, int xStart)
		{
			UpdateLines(xStart, lineNr, lineNr);
		}
		
		internal void UpdateLine(int line, int begin, int end)
		{
			UpdateLines(line, line);
		}
		int FirstPhysicalLine {
			get {
				return VirtualTop.Y / textView.FontHeight;
			}
		}
		internal void UpdateLines(int xPos, int lineBegin, int lineEnd)
		{
//			if (lineEnd < FirstPhysicalLine || lineBegin > FirstPhysicalLine + textView.VisibleLineCount) {
//				return;
//			}
			
			InvalidateLines((int)(xPos * this.TextView.WideSpaceWidth), lineBegin, lineEnd);
		}
		
		void InvalidateLines(int xPos, int lineBegin, int lineEnd)
		{
			lineBegin     = Math.Max(Document.GetVisibleLine(lineBegin), FirstPhysicalLine);
			lineEnd       = Math.Min(Document.GetVisibleLine(lineEnd),   FirstPhysicalLine + textView.VisibleLineCount);
			int y         = Math.Max(    0, (int)(lineBegin  * textView.FontHeight));
			int height    = Math.Min(textView.DrawingPosition.Height, (int)((1 + lineEnd - lineBegin) * (textView.FontHeight + 1)));
			
			Rectangle r = new Rectangle(0,
			                            y - 1 - this.virtualTop.Y,
			                            Width,
			                            height + 3);
			
			Invalidate(r);
		}
		#endregion
		public event KeyEventHandler    KeyEventHandler;
		public event DialogKeyProcessor DoProcessDialogKey;
		
		//internal void
	}
}
