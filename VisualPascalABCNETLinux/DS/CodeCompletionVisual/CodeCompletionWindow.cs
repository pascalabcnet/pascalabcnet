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
	public class PABCNETCodeCompletionWindow : AbstractCompletionWindow
	{
		ICompletionData[] completionData;
       	PABCNETCodeCompletionListView codeCompletionListView;
		VScrollBar    vScrollBar = new VScrollBar();
		ICompletionDataProvider dataProvider;
		IDocument document;
		private bool _is_overrided_meths = false;
		Control editorControl;
		
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
        
		PABCNETCodeCompletionWindow(ICompletionDataProvider completionDataProvider, ICompletionData[] completionData, Form parentForm, TextEditorControl control,bool visibleKeyPressed, bool is_by_dot) : base(parentForm, control)
		{
			editorControl = control;
			this.dataProvider = completionDataProvider;
			this.completionData = completionData;
			this.document = control.Document;
			workingScreen = Screen.GetWorkingArea(Location);
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
			codeCompletionListView.Click  += new EventHandler(CodeCompletionListViewClick);
            codeCompletionListView.Font = new Font(VisualPascalABC.Constants.CompletionWindowCodeCompletionListViewFontName,codeCompletionListView.Font.Size);
			Controls.Add(codeCompletionListView);
			
			int MaxListLength = VisualPascalABC.Constants.CompletionWindowMaxListLength;
			if (completionData.Length > MaxListLength) {
				vScrollBar.Dock = DockStyle.Right;
				vScrollBar.Minimum = 0;
				vScrollBar.Maximum = completionData.Length - 1;
				vScrollBar.SmallChange = 1;
				vScrollBar.LargeChange = MaxListLength;
				codeCompletionListView.FirstItemChanged += new EventHandler(CodeCompletionListViewFirstItemChanged);
				Controls.Add(vScrollBar);
			}
            this.drawingSize = new Size(//codeCompletionListView.ItemHeight * 10,
                                        VisualPascalABC.Constants.CompletionWindowWidth,
                                        codeCompletionListView.ItemHeight * Math.Min(MaxListLength, completionData.Length) + 2);
			SetLocation();
			
			if (declarationViewWindow == null) {
				declarationViewWindow = new DeclarationWindow(parentForm);
				declarationViewWindow.in_completion_list = true;
                declarationViewWindow.Font = new Font(VisualPascalABC.Constants.CompletionWindowDeclarationViewWindowFontName, declarationViewWindow.Font.Size);
			}            
			SetDeclarationViewLocation();
            //DS закоментил, это желтый квадрат при старте
			//declarationViewWindow.ShowDeclarationViewWindow();
			declarationViewWindow.MouseMove += ControlMouseMove;
			control.Focus();
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
			document.DocumentAboutToBeChanged += DocumentAboutToBeChanged;
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
			control.ActiveTextAreaControl.TextArea.Focus();
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
		
		protected override void SetLocation()
		{
			base.SetLocation();
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

		void CodeCompletionListViewSelectedItemChanged(object sender, EventArgs e)
		{
			ICompletionData data = codeCompletionListView.SelectedCompletionData;
			if (data != null && !(data as UserDefaultCompletionData).IsOnOverrideWindow && data.Description != null && data.Description.Length > 0) {
				SetDeclarationViewLocation();
				declarationViewWindow.Description = data.Description;
			} else {
				if (declarationViewWindow != null)
					declarationViewWindow.Description = null;
			}
		}
		
		public override bool ProcessKeyEvent(char ch)
		{
			switch (dataProvider.ProcessKey(ch)) {
				case CompletionDataProviderKeyResult.BeforeStartKey:
					// increment start+end, then process as normal char
					++startOffset;
					++endOffset;
					return base.ProcessKeyEvent(ch);
				case CompletionDataProviderKeyResult.NormalKey:
					// just process normally
                    
					return base.ProcessKeyEvent(ch);
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


		protected override void CaretOffsetChanged(object sender, EventArgs e)
		{
			int offset = control.ActiveTextAreaControl.Caret.Offset;
			if (offset == startOffset) {
				return;
			}
			if (offset < startOffset || offset > endOffset) {
				Close();
                fiexerrorTextAreaInvalidate();
				editorControl.Focus();
			} else {
				codeCompletionListView.SelectItemWithStart(control.Document.GetText(startOffset, offset - startOffset));
			}
            lastCursorScreenPosition = control.ActiveTextAreaControl.TextArea.Caret.ScreenPosition;
		}
		
		protected override bool ProcessTextAreaKey(Keys keyData)
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
					editorControl.Focus();
					return true;
				case Keys.Up:
					codeCompletionListView.SelectPrevItem();
					editorControl.Focus();
					return true;
				case Keys.Tab:
				case Keys.Return:
					InsertSelectedItem('\0');
					editorControl.Focus();
					return true;
                case Keys.Escape:
                    fiexerrorTextAreaInvalidate();
					break;
			}
			var v = base.ProcessTextAreaKey(keyData);
			if (keyData == Keys.Escape)
			  editorControl.Focus();
			return v;
		}
		
		void CodeCompletionListViewDoubleClick(object sender, EventArgs e)
		{
			InsertSelectedItem('\0');
			editorControl.Focus();
		}
		
		void CodeCompletionListViewClick(object sender, EventArgs e)
		{
			control.ActiveTextAreaControl.TextArea.Focus();
		}
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
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
			Close();
			return result;
		}
	}
}
