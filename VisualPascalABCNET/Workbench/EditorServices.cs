// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VisualPascalABCPlugins;

namespace VisualPascalABC
{
    public partial class Form1 : IEditorService
    {
        public CodeFileDocumentControl CurrentSyntaxEditor
        {
            get
            {
                return CurrentCodeFileDocument;
            }
        }

        public void ExecFind()
        {
            string selt = CurrentSyntaxEditor.SelectedText;
            if (selt != string.Empty)
                FindForm.tbTextToFind.Text = selt;
            FindForm.Show();
        }

        public void ExecFindNext()
        {
            FindForm.FindNext();
        }

        public void ExecReplace()
        {
            string selt = CurrentSyntaxEditor.SelectedText;
            if (selt != string.Empty)
                ReplaceForm.tbTextToFind.Text = selt;
            ReplaceForm.Show();
        }

        public void ExecUndo()
        {
            var d = CurrentCodeFileDocument;
            if (d.DesignerAndCodeTabs != null)
            {
                if (d.DesignerPage != null && d.Designer != null && d.DesignerAndCodeTabs.SelectedTab == d.DesignerPage)
                    d.Designer.Undo();
                else
                    d.TextEditor.Undo();
            }
            else
                d.TextEditor.Undo();
            UpdateUndoRedoEnabled();
        }

        public void ExecRedo()
        {
            var d = CurrentCodeFileDocument;
            if (d.DesignerAndCodeTabs != null)
            {
                if (d.DesignerPage != null && d.Designer != null && d.DesignerAndCodeTabs.SelectedTab == d.DesignerPage)
                    d.Designer.Redo();
                else
                    d.TextEditor.Redo();
            }
            else
                d.TextEditor.Redo();
            UpdateUndoRedoEnabled();
        }

        public void SetFocusToEditor()
        {
            CurrentSyntaxEditor.TextEditor.SetFocus();
        }

        public void SynEdit_SelectionChanged(object sender)
        {
            UpdateCutCopyButtonsEnabled();
        }

        //ìåíÿåì ñòðàòåãèþ ïîäñâåäêè â ñîîòâåòñâèè ñ ðàñøèðåíèåì ôàéëà
        private void SetHighlightingStrategy(ICSharpCode.TextEditor.TextEditorControl edit, string FileName)
        {
            edit.Document.HighlightingStrategy = ICSharpCode.TextEditor.Document.HighlightingManager.Manager.FindHighlighterForFile(FileName);
        }

        internal bool editorDisabled;

        public void SetEditorDisabled(bool val)
        {
            this.editorDisabled = val;
            if (!val)
                foreach (var tab in OpenDocuments.Values)
                    tab.TextEditor.Document.ReadOnly = val;
            CurrentCodeFileDocument.TextEditor.Document.ReadOnly = val;
        }

        public void SynEdit_ChangeText(object sender, CodeFileDocumentControl cfdc)
        {
            CodeFileDocumentControl d = cfdc;
            if (!d.DocumentChanged && !visualStates.FileOpened)//!((Data)tabControl1.SelectedTab.Tag).FileOpened) 
            {
                d.DocumentChanged = true;
                SetTabPageText(CurrentCodeFileDocument);
                if (!d.FromMetadata)
                    SaveAllButtonsEnabled = SaveButtonsEnabled = true;

            }
            
            NavigationLocationChanged();
            UpdateUndoRedoEnabled();   
            WorkbenchServiceFactory.CodeCompletionParserController.SetAsChanged(cfdc.FileName);
        }

        public void UpdateLineColPosition()
        {
            slLine.Text = CurrentSyntaxEditor.CaretPosition.Y.ToString();
            slCol.Text = CurrentSyntaxEditor.CaretPosition.X.ToString();
        }

        public void ExecNavigateBackward()
        {
            NavigationManager.NavigateBackward();
        }

        public void ExecNavigateForward()
        {
            NavigationManager.NavigateForward();
        }

        public void ExecSelectAll()
        {
            CurrentSyntaxEditor.SelectAll();
        }

        public void ExecGotoLine()
        {
            GotoLineForm.ShowDialog();
        }

        public void ExecFindReferences()
        {
            FindSymbolsResultWindow.FindAllReferences();
        }

        public void ExecCut()
        {
            CurrentCodeFileDocument.Cut();
        }

        public void ExecCopy()
        {
            CurrentCodeFileDocument.Copy();
        }

        public void ExecPaste()
        {
            try
            {
                CurrentCodeFileDocument?.Paste(true);
            }
            catch (Exception)
            {

            }
        }

        internal void NavigationLocationChanged()
        {
            ICSharpCode.TextEditor.Caret c = CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.Caret;
            NavigationManager.LocationChanged(c.Line + 1, c.Column + 1, CurrentSourceFileName);
        }

        internal void ExecuteErrorPos(PascalABCCompiler.SourceLocation sl, int imageindex)
        {
            if (CurrentCodeFileDocument.DesignerAndCodeTabs != null)
            {
                CurrentCodeFileDocument.DesignerAndCodeTabs.SelectedTab = CurrentCodeFileDocument.TextPage;
            }
            ExecuteSourceLocationAction(sl, ErrorCursorPosStrategy);
            if (imageindex == 1)
                ErrorLineBookmark.SetPosition(CurrentSyntaxEditor.TextEditor, sl.BeginPosition.Line);
            else if (imageindex == 2)
                RuntimeErrorBookmark.SetPosition(CurrentSyntaxEditor.TextEditor, sl.BeginPosition.Line);

        }

        public void ExecuteSourceLocationAction(PascalABCCompiler.SourceLocation
            SourceLocation, VisualPascalABCPlugins.SourceLocationAction Action)
        {
            if (Action != SourceLocationAction.NavigationGoto)
                NavigationLocationChanged();
            if (SourceLocation.FileName != null)
                WorkbenchServiceFactory.FileService.OpenFile(SourceLocation.FileName, null, true);
            CodeFileDocumentTextEditorControl editor = CurrentSyntaxEditor.TextEditor;
            editor.ActiveTextAreaControl.SelectionManager.ClearSelection();

            //Point Beg = new Point(SourceLocation.BeginPosition.Column - 1, SourceLocation.BeginPosition.Line - 1);
            //Point End = new Point(SourceLocation.EndPosition.Column, SourceLocation.EndPosition.Line - 1);
            ICSharpCode.TextEditor.TextLocation Beg = new ICSharpCode.TextEditor.TextLocation(SourceLocation.BeginPosition.Column - 1, SourceLocation.BeginPosition.Line - 1);
            ICSharpCode.TextEditor.TextLocation End = new ICSharpCode.TextEditor.TextLocation(SourceLocation.EndPosition.Column, SourceLocation.EndPosition.Line - 1);
            switch (Action)
            {
                case SourceLocationAction.SelectAndGotoBeg:
                case SourceLocationAction.SelectAndGotoEnd:
                case SourceLocationAction.FindSelection:
                    if (Action == SourceLocationAction.SelectAndGotoBeg)
                        editor.ActiveTextAreaControl.Caret.Position = Beg;
                    else
                        editor.ActiveTextAreaControl.Caret.Position = End;
                    editor.ActiveTextAreaControl.SelectionManager.SetSelection(Beg, End);
                    if (Action != SourceLocationAction.FindSelection)
                        SetFocusToEditor();
                    break;
                case SourceLocationAction.NavigationGoto:
                case SourceLocationAction.GotoBeg:
                case SourceLocationAction.GotoEnd:
                    if (Action == SourceLocationAction.GotoBeg || Action == SourceLocationAction.NavigationGoto)
                        editor.ActiveTextAreaControl.Caret.Position = Beg;
                    else
                        editor.ActiveTextAreaControl.Caret.Position = End;
                    SetFocusToEditor();
                    break;
            }
            if (Action != SourceLocationAction.NavigationGoto)
            {
                NavigationLocationChanged();
                CurrentSyntaxEditor.CenterView();
            }
        }

        private void AddEditorHandlers(CodeFileDocumentControl tp)
        {
            tp.TextEditor.AddToolTipRequestHandler(TooltipServiceManager.ToolTipService_TextAreaToolTipRequest);
            tp.TextEditor.AddMouseMoveHandler(TooltipServiceManager.ToolTipService_TextAreaMouseMove);
            tp.TextEditor.AddKeyDownHandler(TooltipServiceManager.ToolTipService_TextAreaKeyDown);
            tp.TextEditor.AddMouseWheelHandler(TooltipServiceManager.ToolTipService_TextAreaMouseEvent_HideToolTip);
            tp.TextEditor.AddMouseDownHandler(TooltipServiceManager.ToolTipService_TextAreaMouseEvent_HideToolTip);
            tp.TextEditor.AddMouseMoveHandler(DefinitionByMouseClickManager.DefinitionByMouseClickManager_TextAreaMouseMove);
            tp.TextEditor.ActiveTextAreaControl.TextArea.Click += DefinitionByMouseClickManager.DefinitionByMouseClickManager_TextAreaMouseDown;
            //tp.TextEditor.AddMouseDownHandler(DefinitionByMouseClickManager.DefinitionByMouseClickManager_TextAreaMouseDown);
        }

        private void RemoveEditorHandlers(CodeFileDocumentControl tp)
        {
            tp.TextEditor.RemoveToolTipRequestHandler(TooltipServiceManager.ToolTipService_TextAreaToolTipRequest);
            tp.TextEditor.RemoveMouseMoveHandler(TooltipServiceManager.ToolTipService_TextAreaMouseMove);
            tp.TextEditor.RemoveKeyDownHandler(TooltipServiceManager.ToolTipService_TextAreaKeyDown);
            tp.TextEditor.RemoveMouseWheelHandler(TooltipServiceManager.ToolTipService_TextAreaMouseEvent_HideToolTip);
            tp.TextEditor.RemoveMouseDownHandler(TooltipServiceManager.ToolTipService_TextAreaMouseEvent_HideToolTip);
            tp.TextEditor.RemoveMouseMoveHandler(DefinitionByMouseClickManager.DefinitionByMouseClickManager_TextAreaMouseMove);
            tp.TextEditor.ActiveTextAreaControl.TextArea.Click -= DefinitionByMouseClickManager.DefinitionByMouseClickManager_TextAreaMouseDown;
            //tp.TextEditor.RemoveMouseMoveHandler(DefinitionByMouseClickManager.DefinitionByMouseClickManager_TextAreaMouseDown);
        }

        public void CollapseRegions()
        {
            CurrentCodeFileDocument.TextEditor.CollapseRegions();
        }

        public void CodeFormat()
        {
            (new CodeFormattingAction()).Execute(CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.TextArea);
        }
    }
}
