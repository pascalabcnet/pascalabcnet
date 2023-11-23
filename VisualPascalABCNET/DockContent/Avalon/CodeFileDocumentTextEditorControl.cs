/*using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;

namespace VisualPascalABC.Avalon
{
    public class CodeFileDocumentTextEditorControl : ICSharpCode.AvalonEdit.TextEditor
    {
        internal Form1 MainForm;
        internal QuickClassBrowserPanel quickClassBrowserPanel;
        FoldingManager foldingManager;

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
            this.foldingManager.FoldingStrategy = new ParserFoldingStrategy();
            Controls.Add(intellisensePanel);
            TextArea.Caret.PositionChanged += docPositionChanged;
            this.TextArea.AllowDrop = true;
            this.TextArea.DragEnter += textEditorDragEnter;
            this.TextArea.DragDrop += textEditorDragDrop;
            foldingManager = FoldingManager.Install(this.TextArea);
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
           TextArea.Caret.PositionChanged -= docPositionChanged;
            this.TextArea.DragEnter -= textEditorDragEnter;
            this.TextArea.DragDrop -= textEditorDragDrop;
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
            if (file_name != null && file_name.Length > 0)
            {
                if (string.Compare(System.IO.Path.GetExtension(file_name[0]), Constants.ProjectExtension, true) == 0)
                    VisualPABCSingleton.ProjectService.OpenProject(file_name[0]);
                else
                    VisualPABCSingleton.FileService.OpenFile(file_name[0], null);
            }
        }

        private void refresh_Folding()
        {
            //TextArea.Refresh(ActiveTextAreaControl.TextArea.FoldMargin);
        }

        public void UpdateFolding()
        {
            CodeCompletionParserController.open_files[this.file_name] = true;
        }

        public void UpdateFolding(object parseInfo, string file_name)
        {
            try
            {
                if (EnableFolding && file_name == this.file_name)
                {
                    foldingManager.UpdateFoldings(file_name, parseInfo);
                    refresh_Folding();
                }
            }
            catch
            {

            }
        }

        void quickClassBrowserPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (quickClassBrowserPanel.Visible)
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
    }
}*/
