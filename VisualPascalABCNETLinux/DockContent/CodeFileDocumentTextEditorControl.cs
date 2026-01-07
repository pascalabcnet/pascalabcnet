// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ICSharpCode.TextEditor.Document;

namespace VisualPascalABC
{
    public class CodeFileDocumentTextEditorControl : ICSharpCode.TextEditor.TextEditorControl, VisualPascalABCPlugins.ITextEditorControl
    {
        internal Form1 MainForm;
        internal QuickClassBrowserPanel quickClassBrowserPanel;
        internal Panel TextAreaPanel
        {
            get { return textAreaPanel; }
        }
        private Panel intellisensePanel;
        public CodeFileDocumentTextEditorControl()
        {
            editactions[Keys.Space | Keys.Control] = new CodeCompletionAllNamesAction();
            editactions[Keys.Space | Keys.Shift] = new CodeCompletionNamesOnlyInModuleAction();
            editactions[Keys.Enter | Keys.Control] = new GotoAction();
            editactions[Keys.LButton | Keys.Control] = new GotoAction();
            editactions[Keys.C | Keys.Shift | Keys.Control] = new ClassOrMethodRealizationAction();
            editactions[Keys.Space | Keys.Shift | Keys.Control] = new VirtualMethodsAction();
            editactions[Keys.I | Keys.Shift | Keys.Control] = new GenerateMethodImplementationHeadersAction();
            editactions[Keys.F5 | Keys.Control] = new AddToWatchAction();
            editactions[Keys.F | Keys.Shift | Keys.Control] = new CodeFormattingAction();
            editactions[Keys.Control | Keys.Y] = new ICSharpCode.TextEditor.Actions.DeleteLine();
            editactions[Keys.Escape] = new HideBottomTabs();
            quickClassBrowserPanel = new QuickClassBrowserPanel(this);
            quickClassBrowserPanel.VisibleChanged += new EventHandler(quickClassBrowserPanel_VisibleChanged);
            if (VisualPABCSingleton.MainForm != null)
            {
                quickClassBrowserPanel.Visible = VisualPABCSingleton.MainForm.UserOptions.ShowQuickClassBrowserPanel;
            }
            intellisensePanel = new Panel();
            quickClassBrowserPanel_VisibleChanged(null, null);
            intellisensePanel.Dock = DockStyle.Top;
            intellisensePanel.Controls.Add(quickClassBrowserPanel);
            intellisensePanel.Paint += new PaintEventHandler(separatorPanel_Paint);
            Document.FoldingManager.FoldingStrategy = new ParserFoldingStrategy();
            Controls.Add(intellisensePanel);
            ActiveTextAreaControl.TextArea.Caret.PositionChanged += docPositionChanged;
            this.ActiveTextAreaControl.TextArea.AllowDrop = true;
            this.ActiveTextAreaControl.TextArea.DragEnter += textEditorDragEnter;
            this.ActiveTextAreaControl.TextArea.DragDrop += textEditorDragDrop;
            this.ActiveTextAreaControl.Document.LineDeleted += Document_LineDeleted;
        }

        private void Document_LineDeleted(object sender, LineEventArgs e)
        {
            /*for (int i = 0; i < e.LineSegment.Words.Count; i++)
            {
                if (e.LineSegment.Words[i].SyntaxColor != null && e.LineSegment.Words[i].SyntaxColor.Color.Name == "Green")
                {
                    e.Document.HighlightingStrategy.MarkTokens(e.Document);
                    e.Document.RequestUpdate(new ICSharpCode.TextEditor.TextAreaUpdate(ICSharpCode.TextEditor.TextAreaUpdateType.WholeTextArea));
                    e.Document.CommitUpdate();
                    return;
                }
            }*/
        }

        public void MarkForImmediateWindow()
        {
        	editactions[Keys.Space | Keys.Control] = null;
            editactions[Keys.Space | Keys.Shift] = null;
            editactions[Keys.Enter | Keys.Control] = null;
            editactions[Keys.LButton | Keys.ControlKey] = null;
            editactions[Keys.C | Keys.Shift | Keys.Control] = null;
            editactions[Keys.Space | Keys.Shift | Keys.Control] = null;
            editactions[Keys.I | Keys.Shift | Keys.Control] = null;
            editactions[Keys.F5 | Keys.Control] = null;
            editactions[Keys.Control | Keys.Y] = null;
            editactions[Keys.Escape] = null;
            quickClassBrowserPanel = null;
     		ActiveTextAreaControl.TextArea.Caret.PositionChanged -= docPositionChanged;
            this.ActiveTextAreaControl.TextArea.DragEnter -= textEditorDragEnter;
            this.ActiveTextAreaControl.TextArea.DragDrop -= textEditorDragDrop;
            //this.Font = new Font(new FontFamily(Constants.CompletionWindowCodeCompletionListViewFontName),8.5f);
            editactions[Keys.Enter] = new ImmediateEvaluateAction();
        }
        
        //internal int DownSeparatorHeight = 3;
		internal int DownSeparatorHeight = 2;
		
		delegate void invoke_delegate();
		
		private void textEditorDragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}
		
		private void textEditorDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			string[] file_name = e.Data.GetData(DataFormats.FileDrop) as string[];
			if (file_name != null && file_name.Length>0)
			{
				if (string.Compare(System.IO.Path.GetExtension(file_name[0]), PascalABCCompiler.StringConstants.platformProjectExtension,true)==0)
                    WorkbenchServiceFactory.ProjectService.OpenProject(file_name[0]);
				else
                    WorkbenchServiceFactory.FileService.OpenFile(file_name[0], null);
			}
		}
		
		private void refresh_Folding()
		{
			ActiveTextAreaControl.TextArea.Refresh(ActiveTextAreaControl.TextArea.FoldMargin);
		}

        public void UpdateFolding()
        {
            if (Languages.Facade.LanguageProvider.Instance.HasLanguageForExtension(FileName))
                CodeCompletionParserController.filesToParse[FileName] = true;
        }

        public void CollapseRegions()
        {
            foreach (FoldMarker marker in Document.FoldingManager.FoldMarker)
            {
                if (marker is RegionFoldMarker && !marker.IsFolded)
                    marker.IsFolded = true;
            }
            refresh_Folding();
            ActiveTextAreaControl.TextArea.Refresh();
            refresh_Folding();
        }

		public void UpdateFolding(object parseInfo, string fileName)
		{
			try
			{
				if (EnableFolding && fileName == this.FileName)
				{
					Document.FoldingManager.UpdateFoldings(fileName, parseInfo);
					ActiveTextAreaControl.TextArea.Invoke(new invoke_delegate(refresh_Folding));
				}
			}
			catch
			{
				
			}
		}
		
        void quickClassBrowserPanel_VisibleChanged(object sender, EventArgs e)
        {
            if(quickClassBrowserPanel.Visible)
                intellisensePanel.Size = new Size(1, quickClassBrowserPanel.Height + DownSeparatorHeight);
            else
                intellisensePanel.Size = new Size(1, DownSeparatorHeight);
        }

        void separatorPanel_Paint(object sender, PaintEventArgs e)
        {
        	//e.Graphics.DrawLine(new Pen(SystemColors.ControlDark, 0.1F), 0, intellisensePanel.Height - 3, intellisensePanel.Width, intellisensePanel.Height - 3);
        	e.Graphics.DrawLine(new Pen(SystemColors.ControlDark, 0.1F), 0, intellisensePanel.Height - 1, intellisensePanel.Width, intellisensePanel.Height - 1);
        }
        void docPositionChanged(object sender, EventArgs e)
        {
            ErrorLineBookmark.Remove();
            RuntimeErrorBookmark.Remove();
        }

        public void SetFocus()
        {
            this.ActiveTextAreaControl.TextArea.Focus();
        }

        public int CaretLine
        {
            get
            {
                return this.ActiveTextAreaControl.Caret.Line;
            }
            set
            {
                this.ActiveTextAreaControl.Caret.Line = value;
            }
        }

        public int CaretColumn
        {
            get
            {
                return this.ActiveTextAreaControl.Caret.Column;
            }
            set
            {
                this.ActiveTextAreaControl.Caret.Column = value;
            }
        }

        public void AddToolTipRequestHandler(ICSharpCode.TextEditor.ToolTipRequestEventHandler handler)
        {
            this.ActiveTextAreaControl.TextArea.ToolTipRequest += handler;
        }

        public void AddMouseMoveHandler(MouseEventHandler handler)
        {
            this.ActiveTextAreaControl.TextArea.MouseMove += handler;
        }

        public void AddKeyDownHandler(KeyEventHandler handler)
        {
            this.ActiveTextAreaControl.TextArea.KeyDown += handler;
        }

        public void AddMouseWheelHandler(MouseEventHandler handler)
        {
            this.ActiveTextAreaControl.TextArea.MouseWheel += handler;
        }

        public void AddMouseDownHandler(MouseEventHandler handler)
        {
            this.ActiveTextAreaControl.TextArea.MouseDown += handler;
        }

        public void RemoveToolTipRequestHandler(ICSharpCode.TextEditor.ToolTipRequestEventHandler handler)
        {
            this.ActiveTextAreaControl.TextArea.ToolTipRequest -= handler;
        }

        public void RemoveMouseMoveHandler(MouseEventHandler handler)
        {
            this.ActiveTextAreaControl.TextArea.MouseMove -= handler;
        }

        public void RemoveKeyDownHandler(KeyEventHandler handler)
        {
            this.ActiveTextAreaControl.TextArea.KeyDown -= handler;
        }

        public void RemoveMouseWheelHandler(MouseEventHandler handler)
        {
            this.ActiveTextAreaControl.TextArea.MouseWheel -= handler;
        }

        public void RemoveMouseDownHandler(MouseEventHandler handler)
        {
            this.ActiveTextAreaControl.TextArea.MouseDown -= handler;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // textAreaPanel
            // 
            this.textAreaPanel.Size = new System.Drawing.Size(121, 118);
            // 
            // CodeFileDocumentTextEditorControl
            // 
            this.Name = "CodeFileDocumentTextEditorControl";
            this.Size = new System.Drawing.Size(121, 118);
            this.ResumeLayout(false);

        }
    }
}
