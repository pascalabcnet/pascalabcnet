// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 2681 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ICSharpCode.TextEditor.Gui.CompletionWindow;
using ICSharpCode.TextEditor.Util;

namespace ICSharpCode.TextEditor.Gui.InsightWindow
{
	public class PABCNETInsightWindow : PassivePopupWindow
	{
		private readonly Form parentForm;
		private readonly TextEditorControl control;
		private bool attached, closing;
		private bool positioned;
		private Point anchor;
		private Rectangle previousArrow, nextArrow;
		internal Form OwnerForm { get { return parentForm; } }

		public PABCNETInsightWindow(Form parentForm, TextEditorControl control)
		{
			this.parentForm = parentForm;
			this.control = control;
			SetStyle(ControlStyles.UserPaint, true);
            VisualPascalABC.HoverTrace.Observe(this, "parameter-hint");
			//SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}
		
		public void ShowInsightWindow()
		{
			if (closing || IsDisposed) return;
			if (insightDataProviderStack.Count == 0) { Close(); return; }
			if (parentForm.IsDisposed || !parentForm.Visible || control.IsDisposed) return;
			AttachEditorEvents();
			SetLocation();
			using (Graphics graphics = parentForm.CreateGraphics()) {
				GetTipText(out var count, out var description, out var parameter, out var parameters);
				TipPainterTools.GetDrawingSizeHelpTipFromCombinedDescription(this,
					graphics, Font, count, description, parameter, parameters, true);
			}
			SetLocation();
			ShowPassiveWindow();
			Refresh();
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
			control.Disposed += EditorDisposed;
			parentForm.LocationChanged += ParentLocationChanged;
			parentForm.Resize += ParentLocationChanged;
			parentForm.Deactivate += ParentDeactivated;
			parentForm.FormClosed += ParentClosed;
			parentForm.Disposed += EditorDisposed;
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
			control.Disposed -= EditorDisposed;
			parentForm.LocationChanged -= ParentLocationChanged;
			parentForm.Resize -= ParentLocationChanged;
			parentForm.Deactivate -= ParentDeactivated;
			parentForm.FormClosed -= ParentClosed;
			parentForm.Disposed -= EditorDisposed;
		}

		private void ParentLocationChanged(object sender, EventArgs e)
		{
			if (parentForm.WindowState == FormWindowState.Minimized) base.SetVisibleCore(false);
			else if (Visible) { positioned = false; SetLocation(); }
		}
		private void ParentDeactivated(object sender, EventArgs e) { base.SetVisibleCore(false); }
		private void ParentClosed(object sender, FormClosedEventArgs e) { Close(); }
		private void EditorDisposed(object sender, EventArgs e) { Close(); }
		private void EditorLostFocus(object sender, EventArgs e) { Close(); }

		private void SetLocation()
		{
			Point point = GetCusorCoord();
			// Keep the hint anchored while arguments are typed on the same line.
			if (positioned && point.Y == anchor.Y) point.X = anchor.X;
			else { anchor = point; positioned = true; }
			Rectangle area = Screen.GetWorkingArea(parentForm);
			if (point.Y + Height > area.Bottom)
				point.Y -= Height + control.ActiveTextAreaControl.TextArea.TextView.FontHeight;
			Location = new Point(Math.Max(area.Left, Math.Min(point.X, area.Right - Width)),
				Math.Max(area.Top, Math.Min(point.Y, area.Bottom - Height)));
		}

		public new void Close()
		{
			if (closing || IsDisposed) return;
			closing = true;
			DetachEditorEvents();
			int notifications = ClosedNotifications;
			base.SetVisibleCore(false);
			try {
				if (ClosedNotifications == notifications)
					OnClosed(new ToolStripDropDownClosedEventArgs(ToolStripDropDownCloseReason.CloseCalled));
			}
			finally { Dispose(); }
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				closing = true;
				DetachEditorEvents();
				insightDataProviderStack.Clear();
			}
			base.Dispose(disposing);
		}
		
		#region Event handling routines
		protected bool ProcessTextAreaKey(Keys keyData)
		{
			if (!Visible) {
				return false;
			}
			// Both passive popups may be open inside an argument expression.
			// Let the editor's existing completion-list handler consume keys first.
			var list = VisualPascalABC.CodeCompletionNamesOnlyInModuleAction.comp_windows[
				control.ActiveTextAreaControl.TextArea] as PABCNETCodeCompletionWindow;
			if (list != null && !list.IsDisposed && list.Visible) return false;
			switch (keyData) {
				case Keys.Down:
					if (DataProvider != null && DataProvider.InsightDataCount > 0) {
						CurrentData = (CurrentData + 1) % DataProvider.InsightDataCount;
						ShowInsightWindow();
					}
					if (DataProvider != null && DataProvider.InsightDataCount > 1)
					return true;
					else return false;
				case Keys.Up:
					if (DataProvider != null && DataProvider.InsightDataCount > 0) {
						CurrentData = (CurrentData + DataProvider.InsightDataCount - 1) % DataProvider.InsightDataCount;
						ShowInsightWindow();
					}
					if (DataProvider != null && DataProvider.InsightDataCount > 1)
					return true;
					else return false;
				case Keys.Escape:
                    fixerrorTextAreaInvalidate();
					Close();
					return true;
			}
			return false;
		}
		
		private Point lastCursorScreenPosition;
		
		internal Point GetCusorCoord()
		{
			TextLocation caretPos  = control.ActiveTextAreaControl.Caret.Position;
			//lastCursorScreenPosition = control.ActiveTextAreaControl.TextArea.Caret.ScreenPosition;
			int y = (int)((1 + caretPos.Y) * control.ActiveTextAreaControl.TextArea.TextView.FontHeight) 
				- control.ActiveTextAreaControl.TextArea.VirtualTop.Y - 1 
				+ control.ActiveTextAreaControl.TextArea.TextView.DrawingPosition.Y;
			
			int xpos = control.ActiveTextAreaControl.TextArea.TextView.GetDrawingXPos(caretPos.Y, caretPos.X);
			int ypos = (control.ActiveTextAreaControl.Document.GetVisibleLine(caretPos.Y) + 1) * control.ActiveTextAreaControl.TextArea.TextView.FontHeight 
				- control.ActiveTextAreaControl.TextArea.VirtualTop.Y;
			int rulerHeight = control.TextEditorProperties.ShowHorizontalRuler ? control.ActiveTextAreaControl.TextArea.TextView.FontHeight : 0;
			
			Point p = control.ActiveTextAreaControl.PointToScreen(new Point(xpos, ypos + rulerHeight));
			return p;
		}
		
		protected void CaretOffsetChanged(object sender, EventArgs e)
		{
			// move the window under the caret (don't change the x position)
			TextLocation caretPos  = control.ActiveTextAreaControl.Caret.Position;
			//lastCursorScreenPosition = control.ActiveTextAreaControl.TextArea.Caret.ScreenPosition;
			int y = (int)((1 + caretPos.Y) * control.ActiveTextAreaControl.TextArea.TextView.FontHeight) 
				- control.ActiveTextAreaControl.TextArea.VirtualTop.Y - 1 
				+ control.ActiveTextAreaControl.TextArea.TextView.DrawingPosition.Y;
			
			int xpos = control.ActiveTextAreaControl.TextArea.TextView.GetDrawingXPos(caretPos.Y, caretPos.X);
			int ypos = (control.ActiveTextAreaControl.Document.GetVisibleLine(caretPos.Y) + 1) * control.ActiveTextAreaControl.TextArea.TextView.FontHeight 
				- control.ActiveTextAreaControl.TextArea.VirtualTop.Y;
			int rulerHeight = control.TextEditorProperties.ShowHorizontalRuler ? control.ActiveTextAreaControl.TextArea.TextView.FontHeight : 0;
			
			Point p = control.ActiveTextAreaControl.PointToScreen(new Point(xpos, ypos + rulerHeight));
			if (p.Y != Location.Y) {
				Location = p;
			}
			fixerrorTextAreaInvalidate();
			lastCursorScreenPosition = control.ActiveTextAreaControl.TextArea.Caret.ScreenPosition;
			while (DataProvider != null && DataProvider.CaretOffsetChanged()) {
				CloseCurrentDataProvider();
			}
			if (!closing && control.ActiveTextAreaControl.TextArea.Focused) ShowInsightWindow();
		}
		
		void fixerrorTextAreaInvalidate()
        {
            //Исправление глюка с остающимся курсором если нажать ескейп.
            //В новой версии редактора должны исправить, надо убрать это.            
            Rectangle r = new Rectangle(lastCursorScreenPosition, new Size(control.ActiveTextAreaControl.TextArea.Font.Height, control.ActiveTextAreaControl.TextArea.Font.Height + 3));
            control.ActiveTextAreaControl.TextArea.Invalidate(r);
        }
		
		protected override void OnMouseDown(MouseEventArgs e)
		{
			// Do not enter ToolStrip's menu-selection or focus logic on click.
			if (DataProvider == null || DataProvider.InsightDataCount == 0) return;
			if (previousArrow.Contains(e.X, e.Y)) {
				CurrentData = (CurrentData + DataProvider.InsightDataCount - 1) % DataProvider.InsightDataCount;
				ShowInsightWindow();
			}
			if (nextArrow.Contains(e.X, e.Y)) {
				CurrentData = (CurrentData + 1) % DataProvider.InsightDataCount;
				ShowInsightWindow();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e) { Capture = false; }
		protected override void OnMouseMove(MouseEventArgs e) { control.ActiveTextAreaControl.TextArea.ShowHiddenCursor(false); }
		protected override void OnMouseWheel(MouseEventArgs e) { HandleMouseWheel(e); }
		
		#endregion
		
		
		public void HandleMouseWheel(MouseEventArgs e)
		{
			if (DataProvider != null && DataProvider.InsightDataCount > 0) {
				if (e.Delta > 0) {
					if (control.TextEditorProperties.MouseWheelScrollDown) {
						CurrentData = (CurrentData + 1) % DataProvider.InsightDataCount;
					} else {
						CurrentData = (CurrentData + DataProvider.InsightDataCount - 1) % DataProvider.InsightDataCount;
					}
				} if (e.Delta < 0) {
					if (control.TextEditorProperties.MouseWheelScrollDown) {
						CurrentData = (CurrentData + DataProvider.InsightDataCount - 1) % DataProvider.InsightDataCount;
					} else {
						CurrentData = (CurrentData + 1) % DataProvider.InsightDataCount;
					}
				}
				ShowInsightWindow();
			}
		}
		
		#region Insight Window Drawing routines
		private void GetTipText(out string methodCountMessage, out string description, out int parameter, out int parameters)
		{
			methodCountMessage = null;
			if (DataProvider == null || DataProvider.InsightDataCount < 1) {
				description = "Unknown Method";
			} else {
				if (DataProvider.InsightDataCount > 1) {
					methodCountMessage = VisualPascalABC.Tools.GetRangeDescription(CurrentData + 1, DataProvider.InsightDataCount);
				}
				description = DataProvider.GetInsightData(CurrentData);
			}
			var provider = DataProvider as VisualPascalABC.DefaultInsightDataProvider;
			parameter = provider != null ? provider.num_param : 1;
			parameters = provider != null ? provider.param_count : 0;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			if (closing || DataProvider == null) return;
			GetTipText(out var count, out var description, out var parameter, out var parameters);
			TipPainterTools.DrawHelpTipFromCombinedDescription(this, pe.Graphics, Font,
				count, description, parameter, parameters, true);
			// Other descriptions use the painter too; retain this popup's hit areas.
			previousArrow = TipPainterTools.DrawingRectangle1;
			nextArrow = TipPainterTools.DrawingRectangle2;
			lastCursorScreenPosition = control.ActiveTextAreaControl.TextArea.Caret.ScreenPosition;
		}
		
		protected override void OnPaintBackground(PaintEventArgs pe)
		{
			pe.Graphics.FillRectangle(SystemBrushes.Info, pe.ClipRectangle);
		}
		#endregion
		
		#region InsightDataProvider handling
		Stack<InsightDataProviderStackElement> insightDataProviderStack = new Stack<InsightDataProviderStackElement>();
		
		int CurrentData {
			get {
				return insightDataProviderStack.Peek().currentData;
			}
			set {
				insightDataProviderStack.Peek().currentData = value;
			}
		}
		
		public int GetCurrentData()
		{
			return CurrentData;
		}
		
		IInsightDataProvider DataProvider {
			get {
				if (insightDataProviderStack.Count == 0) {
					return null;
				}
				return insightDataProviderStack.Peek().dataProvider;
			}
		}
		
		public void AddInsightDataProvider(IInsightDataProvider provider, string fileName)
		{
			provider.SetupDataProvider(fileName, control.ActiveTextAreaControl.TextArea);
			if (provider.InsightDataCount > 0) {
				insightDataProviderStack.Push(new InsightDataProviderStackElement(provider));
			}
		}
		
		public IInsightDataProvider GetInsightProvider()
		{
			return insightDataProviderStack.Peek().dataProvider;
		}
		
		void CloseCurrentDataProvider()
		{
			insightDataProviderStack.Pop();
			if (insightDataProviderStack.Count == 0) {
				Close();
			} else {
				Refresh();
			}
		}
		
		public int InsightProviderStackLength
		{
			get
			{
				return insightDataProviderStack.Count;
			}
		}
		
		class InsightDataProviderStackElement
		{
			public int                  currentData;
			public IInsightDataProvider dataProvider;
			
			public InsightDataProviderStackElement(IInsightDataProvider dataProvider)
			{
				this.currentData = Math.Min(Math.Max(dataProvider.DefaultIndex, 0), dataProvider.InsightDataCount - 1);
				this.dataProvider = dataProvider;
			}
		}
		#endregion
	}
}
