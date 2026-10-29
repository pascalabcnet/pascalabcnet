// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 2533 $</version>
// </file>

using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;

namespace ICSharpCode.TextEditor.Gui.CompletionWindow
{
	public class PABCNETCodeCompletionWindow : PassivePopupWindow
	{
		private readonly Form parentForm;
		protected readonly TextEditorControl control;
		protected Size drawingSize;
		private bool attached, closing;
		private readonly Panel content = new Panel { TabStop = false };
		private CompletionContentHost contentHost;
		private bool arrangingContent;

		private sealed class CompletionContentHost : ToolStripControlHost
		{
			internal CompletionContentHost(Control child) : base(child) { }
			internal void Fill(Rectangle bounds)
			{
				SetBounds(bounds);
				Control.Bounds = bounds;
			}
		}

		protected override void OnLayout(LayoutEventArgs e)
		{
			if (arrangingContent) return;
			arrangingContent = true;
			try {
				base.OnLayout(e);
				// Mono's host alignment may center a full-size control inside a
				// shorter item, clipping its top. Both rectangles must fill the popup.
				contentHost?.Fill(ClientRectangle);
			}
			finally { arrangingContent = false; }
		}
		ICompletionData[] completionData;
       	PABCNETCodeCompletionListView codeCompletionListView;
		VScrollBar    vScrollBar = new PassiveCompletionScrollBar();
		ICompletionDataProvider dataProvider;
		IDocument document;
		private bool _is_overrided_meths = false;
		
		public bool is_overrided_meths
		{
			get
			{
				return _is_overrided_meths;
			}
			set
			{
				_is_overrided_meths = value;
				if (value && completionData != null)
					CalcFormWidth();
			}
		}
		
		int                      startOffset;
		int                      endOffset;
		DeclarationWindow    declarationViewWindow = null;
		Rectangle workingScreen;
        Point lastCursorScreenPosition;

        public static PABCNETCodeCompletionWindow ShowCompletionWindow(Form parent, TextEditorControl control, string fileName, ICompletionDataProvider completionDataProvider, char firstChar, bool visibleKeyPressed, bool is_by_dot,PascalABCCompiler.Parsers.KeywordKind keyw)
		{
        	ICompletionData[] completionData = (completionDataProvider as VisualPascalABC.CodeCompletionProvider).GenerateCompletionDataWithKeyword(fileName, control.ActiveTextAreaControl.TextArea, firstChar, keyw);
			if (completionData == null || completionData.Length == 0) {
				return null;
			}
            PABCNETCodeCompletionWindow codeCompletionWindow = new PABCNETCodeCompletionWindow(completionDataProvider, completionData, parent, control, visibleKeyPressed, is_by_dot);
			codeCompletionWindow.ShowCompletionWindow();
			return codeCompletionWindow;
		}
		
        public static PABCNETCodeCompletionWindow ShowOverridableMethodsCompletionWindows(Form parent, TextEditorControl control, string fileName, ICompletionDataProvider completionDataProvider)
        {
        	ICompletionData[] completionData = (completionDataProvider as VisualPascalABC.CodeCompletionProvider).GenerateCompletionDataForOverridableMethods(fileName, control.ActiveTextAreaControl.TextArea);
        	if (completionData == null || completionData.Length == 0) {
				return null;
			}
        	PABCNETCodeCompletionWindow codeCompletionWindow = new PABCNETCodeCompletionWindow(completionDataProvider, completionData, parent, control, false, false);
			codeCompletionWindow.is_overrided_meths = true;
        	codeCompletionWindow.ShowCompletionWindow();
			return codeCompletionWindow;
        }
        
        public void CalcFormWidth()
        {
        	this.codeCompletionListView.CalcWidth();
			drawingSize.Width = content.Width;
        }
        
        public static PABCNETCodeCompletionWindow ShowCompletionWindowWithFirstChar(Form parent, TextEditorControl control, string fileName, ICompletionDataProvider completionDataProvider, char firstChar, PascalABCCompiler.Parsers.KeywordKind keyw)
        {
        	ICompletionData[] completionData = (completionDataProvider as VisualPascalABC.CodeCompletionProvider).GenerateCompletionDataByFirstChar(fileName, control.ActiveTextAreaControl.TextArea, firstChar, keyw);
        	if (completionData == null || completionData.Length == 0) {
				return null;
			}
        	(completionDataProvider as VisualPascalABC.CodeCompletionProvider).preSelection = firstChar.ToString();
            PABCNETCodeCompletionWindow codeCompletionWindow = new PABCNETCodeCompletionWindow(completionDataProvider, completionData, parent, control, true, false);
			codeCompletionWindow.ShowCompletionWindow();
			return codeCompletionWindow;
        }
        
		PABCNETCodeCompletionWindow(ICompletionDataProvider completionDataProvider, ICompletionData[] completionData, Form parentForm, TextEditorControl control,bool visibleKeyPressed, bool is_by_dot)
		{
			this.parentForm = parentForm;
			this.control = control;
			this.dataProvider = completionDataProvider;
			this.completionData = completionData;
			this.document = control.Document;
			workingScreen = Screen.GetWorkingArea(parentForm);
            lastCursorScreenPosition = control.ActiveTextAreaControl.Caret.ScreenPosition;
            startOffset = control.ActiveTextAreaControl.Caret.Offset + (visibleKeyPressed ? 1 : 0);
			endOffset   = startOffset;
			if (completionDataProvider.PreSelection != null) {
				
				startOffset -= completionDataProvider.PreSelection.Length; //+ 1;
				if (visibleKeyPressed) endOffset--;
				//endOffset--;
				(completionDataProvider as VisualPascalABC.CodeCompletionProvider).preSelection = null;
			}
			
			codeCompletionListView = new PABCNETCodeCompletionListView(completionData, is_by_dot);
			codeCompletionListView.ImageList = completionDataProvider.ImageList;
			codeCompletionListView.Dock = DockStyle.Fill;
			codeCompletionListView.SelectedItemChanged += new EventHandler(CodeCompletionListViewSelectedItemChanged);
			codeCompletionListView.DoubleClick += new EventHandler(CodeCompletionListViewDoubleClick);
            codeCompletionListView.Font = VisualPascalABC.Constants.CompletionFont;
			content.Controls.Add(codeCompletionListView);
			
			int MaxListLength = VisualPascalABC.Constants.CompletionWindowMaxListLength;
			if (completionData.Length > MaxListLength) {
				vScrollBar.Dock = DockStyle.Right;
				vScrollBar.Minimum = 0;
				vScrollBar.Maximum = completionData.Length - 1;
				vScrollBar.SmallChange = 1;
				vScrollBar.LargeChange = MaxListLength;
				codeCompletionListView.FirstItemChanged += new EventHandler(CodeCompletionListViewFirstItemChanged);
				content.Controls.Add(vScrollBar);
			}
			contentHost = new CompletionContentHost(content) {
				AutoSize = false, Margin = Padding.Empty, Padding = Padding.Empty,
				Overflow = ToolStripItemOverflow.Never
			};
			Items.Add(contentHost);
            this.drawingSize = new Size(//codeCompletionListView.ItemHeight * 10,
                                        VisualPascalABC.Constants.CompletionWindowWidth,
                                        codeCompletionListView.ItemHeight * Math.Min(MaxListLength, completionData.Length) + 2);
			SetLocation();
			
			if (declarationViewWindow == null) {
				declarationViewWindow = new DeclarationWindow(parentForm);
				declarationViewWindow.in_completion_list = true;
                declarationViewWindow.Font = VisualPascalABC.Constants.CompletionFont;
			}            
			SetDeclarationViewLocation();
            //DS закоментил, это желтый квадрат при старте
			//declarationViewWindow.ShowDeclarationViewWindow();
			CodeCompletionListViewSelectedItemChanged(this, EventArgs.Empty);
			
			if ((completionDataProvider as VisualPascalABC.CodeCompletionProvider).DefaultCompletionElement != null) {
				if ((completionDataProvider as VisualPascalABC.CodeCompletionProvider).ByFirstChar)
				codeCompletionListView.FirstInsert = true;
				codeCompletionListView.SelectIndexByCompletionData((completionDataProvider as VisualPascalABC.CodeCompletionProvider).DefaultCompletionElement);
			}
			
			if (completionDataProvider.PreSelection != null) {
				CaretOffsetChanged(this, EventArgs.Empty);
			}
			
			vScrollBar.ValueChanged += VScrollBarValueChanged;
			vScrollBar.MouseMove += ControlMouseMove;
			codeCompletionListView.MouseMove += ControlMouseMove;
			codeCompletionListView.MouseWheel += (sender, e) => {
				HandleMouseWheel(e);
				if (e is HandledMouseEventArgs wheel) wheel.Handled = true;
			};
			document.DocumentAboutToBeChanged += DocumentAboutToBeChanged;
            VisualPascalABC.HoverTrace.Observe(this, "completion-list");
            VisualPascalABC.HoverTrace.Observe(codeCompletionListView, "completion-list-content");
            VisualPascalABC.HoverTrace.Observe(vScrollBar, "completion-scrollbar");
            VisualPascalABC.HoverTrace.Observe(declarationViewWindow, "completion-description");
		}
		
		bool inScrollUpdate;
		
		void CodeCompletionListViewFirstItemChanged(object sender, EventArgs e)
		{
			if (inScrollUpdate) return;
			inScrollUpdate = true;
			vScrollBar.Value = Math.Min(vScrollBar.Maximum, codeCompletionListView.FirstItem);
			inScrollUpdate = false;
		}
		
		void VScrollBarValueChanged(object sender, EventArgs e)
		{
			if (inScrollUpdate) return;
			inScrollUpdate = true;
			codeCompletionListView.FirstItem = vScrollBar.Value;
			codeCompletionListView.Refresh();
			inScrollUpdate = false;
		}
		
		void SetDeclarationViewLocation()
		{

			//  This method uses the side with more free space
			int leftSpace = Bounds.Left - workingScreen.Left;
			int rightSpace = workingScreen.Right - Bounds.Right;
			Point pos;
			// The declaration view window has better line break when used on
			// the right side, so prefer the right side to the left.
			//MessageBox.Show((declarationViewWindow == null).ToString());
			/*if (declarationViewWindow == null)
            {
				declarationViewWindow = new DeclarationWindow(VisualPascalABC.VisualPABCSingleton.MainForm);
				declarationViewWindow.in_completion_list = true;
				declarationViewWindow.Font = new Font(VisualPascalABC.Constants.CompletionWindowDeclarationViewWindowFontName, declarationViewWindow.Font.Size);
			}*/
			if (rightSpace * 2 > leftSpace)
				pos = new Point(Bounds.Right+2, Bounds.Top);
			else
				pos = new Point(Bounds.Left - declarationViewWindow.Width+2, Bounds.Top);
			if (declarationViewWindow.Location != pos) {
				declarationViewWindow.Location = pos;
			}
		}
		
		protected virtual void SetLocation()
		{
			var textArea = control.ActiveTextAreaControl.TextArea;
			var caret = textArea.Caret.Position;
			int ruler = control.TextEditorProperties.ShowHorizontalRuler ? textArea.TextView.FontHeight : 0;
			Point point = new Point(textArea.TextView.DrawingPosition.X + textArea.TextView.GetDrawingXPos(caret.Y, caret.X),
				textArea.TextView.DrawingPosition.Y + control.Document.GetVisibleLine(caret.Y) * textArea.TextView.FontHeight
				- textArea.VirtualTop.Y + textArea.TextView.FontHeight + ruler);
			point = control.ActiveTextAreaControl.PointToScreen(point);
			workingScreen = Screen.GetWorkingArea(parentForm);
			if (point.Y + drawingSize.Height > workingScreen.Bottom)
				point.Y -= drawingSize.Height + textArea.TextView.FontHeight;
			point.X = Math.Max(workingScreen.Left, Math.Min(point.X, workingScreen.Right - drawingSize.Width));
			point.Y = Math.Max(workingScreen.Top, Math.Min(point.Y, workingScreen.Bottom - drawingSize.Height));
			Bounds = new Rectangle(point, drawingSize);
			PerformLayout();
			if (declarationViewWindow != null) {
				SetDeclarationViewLocation();
			}
		}
		
		public void HandleMouseWheel(MouseEventArgs e)
		{
			int MAX_DELTA  = 120; // basically it's constant now, but could be changed later by MS
			int multiplier = e.Delta / MAX_DELTA;
			multiplier *= System.Windows.Forms.SystemInformation.MouseWheelScrollLines * vScrollBar.SmallChange;
			
			int newValue;
			if (System.Windows.Forms.SystemInformation.MouseWheelScrollLines > 0) {
				newValue = this.vScrollBar.Value - (control.TextEditorProperties.MouseWheelScrollDown ? 1 : -1) * multiplier;
			} else {
				newValue = this.vScrollBar.Value - (control.TextEditorProperties.MouseWheelScrollDown ? 1 : -1) * multiplier;
			}
			vScrollBar.Value = Math.Max(vScrollBar.Minimum, Math.Min(vScrollBar.Maximum - vScrollBar.LargeChange + 1, newValue));
		}

		private void ShowCompletionWindow()
		{
			if (closing || IsDisposed || parentForm.IsDisposed || !parentForm.Visible) return;
			AttachEditorEvents();
			SetLocation();
			ShowPassiveWindow();
			PerformLayout();
			CodeCompletionListViewSelectedItemChanged(this, EventArgs.Empty);
		}

		private void AttachEditorEvents()
		{
			if (attached) return;
			attached = true;
			var area = control.ActiveTextAreaControl;
			area.TextArea.DoProcessDialogKey += ProcessTextAreaKey;
			area.TextArea.LostFocus += EditorLostFocus;
			area.Caret.PositionChanged += CaretOffsetChanged;
			area.VScrollBar.ValueChanged += ParentLocationChanged;
			area.HScrollBar.ValueChanged += ParentLocationChanged;
			control.Resize += ParentLocationChanged;
			control.Disposed += OwnerDisposed;
			parentForm.LocationChanged += ParentLocationChanged;
			parentForm.Resize += ParentLocationChanged;
			parentForm.Deactivate += OwnerDisposed;
			parentForm.FormClosed += ParentClosed;
			parentForm.Disposed += OwnerDisposed;
		}

		private void DetachEditorEvents()
		{
			if (!attached) return;
			attached = false;
			var area = control.ActiveTextAreaControl;
			area.TextArea.DoProcessDialogKey -= ProcessTextAreaKey;
			area.TextArea.LostFocus -= EditorLostFocus;
			area.Caret.PositionChanged -= CaretOffsetChanged;
			area.VScrollBar.ValueChanged -= ParentLocationChanged;
			area.HScrollBar.ValueChanged -= ParentLocationChanged;
			control.Resize -= ParentLocationChanged;
			control.Disposed -= OwnerDisposed;
			parentForm.LocationChanged -= ParentLocationChanged;
			parentForm.Resize -= ParentLocationChanged;
			parentForm.Deactivate -= OwnerDisposed;
			parentForm.FormClosed -= ParentClosed;
			parentForm.Disposed -= OwnerDisposed;
		}

		private void ParentLocationChanged(object sender, EventArgs e)
		{
			if (parentForm.WindowState == FormWindowState.Minimized) Close();
			else if (Visible) SetLocation();
		}
		private void OwnerDisposed(object sender, EventArgs e) { Close(); }
		private void EditorLostFocus(object sender, EventArgs e) { Close(); }
		private void ParentClosed(object sender, FormClosedEventArgs e) { Close(); }
		private void ControlMouseMove(object sender, MouseEventArgs e) { control.ActiveTextAreaControl.TextArea.ShowHiddenCursor(false); }
		protected override void OnMouseMove(MouseEventArgs e) { ControlMouseMove(this, e); }
		protected override void OnMouseWheel(MouseEventArgs e) { HandleMouseWheel(e); }

		public new void Close()
		{
			if (closing || IsDisposed) return;
			closing = true;
			DetachEditorEvents();
			int notifications = ClosedNotifications;
			base.SetVisibleCore(false);
			declarationViewWindow?.Hide();
			try {
				// Framework raises Closed from SetVisibleCore; Mono needs it here.
				if (ClosedNotifications == notifications)
					OnClosed(new ToolStripDropDownClosedEventArgs(ToolStripDropDownCloseReason.CloseCalled));
			}
			finally { Dispose(); }
		}

		void CodeCompletionListViewSelectedItemChanged(object sender, EventArgs e)
		{
			if (!Visible) return;
			ICompletionData data = codeCompletionListView.SelectedCompletionData;
			if (data != null && !(data as UserDefaultCompletionData).IsOnOverrideWindow && data.Description != null && data.Description.Length > 0) {
				SetDeclarationViewLocation();
				declarationViewWindow.Description = data.Description;
			} else {
				if (declarationViewWindow != null)
					declarationViewWindow.Description = null;
			}
		}
		
		public bool ProcessKeyEvent(char ch)
		{
			if (closing || IsDisposed || !Visible) return false;
			switch (dataProvider.ProcessKey(ch)) {
				case CompletionDataProviderKeyResult.BeforeStartKey:
					// increment start+end, then process as normal char
					++startOffset;
					++endOffset;
					return false;
				case CompletionDataProviderKeyResult.NormalKey:
					// just process normally
                    
					return false;
				case CompletionDataProviderKeyResult.InsertionKey:
					return InsertSelectedItem(ch);
				default:
					throw new InvalidOperationException("Invalid return value of dataProvider.ProcessKey");
			}
		}
		
		void DocumentAboutToBeChanged(object sender, DocumentEventArgs e)
		{
			// => startOffset test required so that this startOffset/endOffset are not incremented again
			//    for BeforeStartKey characters
			if (e.Offset >= startOffset && e.Offset <= endOffset) {
				if (e.Length > 0) { // length of removed region
					endOffset -= e.Length;
				}
				if (!string.IsNullOrEmpty(e.Text)) {
					endOffset += e.Text.Length;
				}
			}
		}

        void fiexerrorTextAreaInvalidate()
        {
            //Исправление глюка с остающимся курсором если нажать ескейп.
            //В новой версии редактора должны исправить, надо убрать это.            
            Rectangle r = new Rectangle(lastCursorScreenPosition, new Size(control.ActiveTextAreaControl.TextArea.Font.Height, control.ActiveTextAreaControl.TextArea.Font.Height + 3));
            control.ActiveTextAreaControl.TextArea.Invalidate(r);
        }


		protected void CaretOffsetChanged(object sender, EventArgs e)
		{
			int offset = control.ActiveTextAreaControl.Caret.Offset;
			if (offset == startOffset) {
				return;
			}
			if (offset < startOffset || offset > endOffset) {
				Close();
                fiexerrorTextAreaInvalidate();
			} else {
				codeCompletionListView.SelectItemWithStart(control.Document.GetText(startOffset, offset - startOffset));
			}
            lastCursorScreenPosition = control.ActiveTextAreaControl.TextArea.Caret.ScreenPosition;
		}
		
		protected bool ProcessTextAreaKey(Keys keyData)
		{
			if (!Visible) { 
				return false;
			}
			
			switch (keyData) {
				case Keys.Home:
					codeCompletionListView.SelectIndex(0);
					return true;
				case Keys.End:
					codeCompletionListView.SelectIndex(completionData.Length-1);
					return true;
				case Keys.PageDown:
					codeCompletionListView.PageDown();
					return true;
				case Keys.PageUp:
					codeCompletionListView.PageUp();
					return true;
				case Keys.Down:
					codeCompletionListView.SelectNextItem();
					return true;
				case Keys.Up:
					codeCompletionListView.SelectPrevItem();
					return true;
				case Keys.Tab:
				case Keys.Return:
					InsertSelectedItem('\0');
					return true;
                case Keys.Escape:
                    fiexerrorTextAreaInvalidate();
					Close();
					return true;
			}
			return false;
		}
		
		void CodeCompletionListViewDoubleClick(object sender, EventArgs e)
		{
			InsertSelectedItem('\0');
		}
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				closing = true;
				DetachEditorEvents();
                (dataProvider as VisualPascalABC.CodeCompletionProvider)?.CancelDocumentation();
				document.DocumentAboutToBeChanged -= DocumentAboutToBeChanged;
				if (codeCompletionListView != null) {
					codeCompletionListView.Dispose();
					codeCompletionListView = null;
				}
				if (declarationViewWindow != null) {
					declarationViewWindow.Dispose();
					declarationViewWindow = null;
				}
			}
			base.Dispose(disposing);
		}
		
		public void SelectIndex(int num)
		{
			if (codeCompletionListView != null)
				codeCompletionListView.SelectIndex(num);
		}
		
		bool InsertSelectedItem(char ch)
		{
            try
            {
                document.DocumentAboutToBeChanged -= DocumentAboutToBeChanged;
                ICompletionData data = codeCompletionListView.SelectedCompletionData;
                if (VisualPascalABC.VisualPABCSingleton.MainForm.UserOptions.EnableSmartIntellisense)
                {
                    VisualPascalABC.CompletionDataDispatcher.AddLastUsedItem(data);
                    VisualPascalABC.CompletionDataDispatcher.BindMember(data);
                }

                bool result = false;
                if (data != null) {
                    control.BeginUpdate();
                    try {
                        if (endOffset - startOffset > 0) {
                            control.Document.Remove(startOffset, endOffset - startOffset);
                        }
                        Debug.Assert(startOffset <= document.TextLength);
                        result = dataProvider.InsertAction(data, control.ActiveTextAreaControl.TextArea, startOffset, ch);
                    } finally {
                        control.EndUpdate();
                    }
                }
                return result;
            }
            finally
            {
                // Close both popup windows even if insertion fails partway through.
                Close();
            }
		}
	}
}
