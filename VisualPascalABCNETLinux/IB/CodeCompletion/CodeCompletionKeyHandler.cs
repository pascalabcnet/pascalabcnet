// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;

using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using ICSharpCode.TextEditor.Gui.InsightWindow;

namespace VisualPascalABC
{

    class CodeCompletionKeyHandler
    {
        public TextEditorControl editor;
        public PABCNETCodeCompletionWindow codeCompletionWindow;
        public PABCNETInsightWindow insightWindow;
        public CodeCompletionProvider completionDataProvider;

        private static Hashtable ht = new Hashtable();

        private CodeCompletionKeyHandler(TextEditorControl editor)
        {
            this.editor = editor;
        }

        public static CodeCompletionKeyHandler Attach(TextEditorControl editor)
        {
            CodeCompletionKeyHandler h = new CodeCompletionKeyHandler(editor);
            ht[editor] = h;
            editor.ActiveTextAreaControl.TextArea.KeyEventHandler += h.TextAreaKeyEventHandler;
            editor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += h.CaretPositionChangedEventHandler;
            //editor.ActiveTextAreaControl.TextArea.KeyDown += new System.Windows.Forms.KeyEventHandler(TextArea_KeyDown);
            //editor.ActiveTextAreaControl.KeyDown += h.TextControlEventHandler;
            // When the editor is disposed, close the code completion window
            editor.Disposed += h.CloseCodeCompletionWindow;
            return h;
        }

        public static void Detach(TextEditorControl editor)
        {
            if (ht.ContainsKey(editor))
                ht.Remove(editor);
        }

        void CaretPositionChangedEventHandler(object sender, EventArgs e)
        {
            if (!VisualPABCSingleton.MainForm.UserOptions.HighlightOperatorBrackets || WorkbenchServiceFactory.DebuggerManager.IsRunning)
                return;
            CodeCompletionHighlighter.Highlight(editor.ActiveTextAreaControl.TextArea);
        }

        /// <summary>
        /// Return true to handle the keypress, return false to let the text area handle the keypress
        /// </summary>
        bool TextAreaKeyEventHandler(char key)
        {
            if (!WorkbenchServiceFactory.Workbench.UserOptions.AllowCodeCompletion || !VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.compilerLoaded)
                return false;
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return false;
            if (codeCompletionWindow != null)
            {
                // If completion window is open and wants to handle the key, don't let the text area
                // handle it
                if (codeCompletionWindow.ProcessKeyEvent(key))
                    return true;
            }
            else
            {
                PABCNETCodeCompletionWindow ccw = CodeCompletionNamesOnlyInModuleAction.comp_windows[editor.ActiveTextAreaControl.TextArea] as PABCNETCodeCompletionWindow;

                if (ccw != null && CodeCompletionNamesOnlyInModuleAction.is_begin)
                {
                    CodeCompletionNamesOnlyInModuleAction.is_begin = false;
                    if (key != ' ')
                        ccw.ProcessKeyEvent(key);
                    else
                    {
                        ccw.ProcessKeyEvent('_');
                        ccw.Close();
                    }
                        
                }
                else if (ccw != null && ccw.ProcessKeyEvent(key))
                    return true;
            }
            if (key == '.')
            {
                if (!string.IsNullOrEmpty(WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.SelectionManager.SelectedText))
                {
                    WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.Caret.Position = WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].StartPosition;
                    WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.SelectionManager.RemoveSelectedText();
                }
                    
                if (WorkbenchServiceFactory.Workbench.UserOptions.CodeCompletionDot)
                {
                    completionDataProvider = new CodeCompletionProvider();

                    codeCompletionWindow = PABCNETCodeCompletionWindow.ShowCompletionWindow(
                        VisualPABCSingleton.MainForm,					// The parent window for the completion window
                        editor, 					// The text editor to show the window for
                        editor.FileName,		// Filename - will be passed back to the provider
                        completionDataProvider,		// Provider to get the list of possible completions
                        key,							// Key pressed - will be passed to the provider
                        true,
                        true,
                        PascalABCCompiler.Parsers.KeywordKind.None
                    );
                    editor.Focus();
                    CodeCompletionNamesOnlyInModuleAction.is_begin = true;
                    CodeCompletionNamesOnlyInModuleAction.comp_windows[editor.ActiveTextAreaControl.TextArea] = codeCompletionWindow;
                    if (codeCompletionWindow != null)
                        codeCompletionWindow.Closed += new EventHandler(CloseCodeCompletionWindow);
                }
            }
            else if (key == '(' || key == '[' || key == ',')
            {
                if (VisualPABCSingleton.MainForm.UserOptions.CodeCompletionParams)
                {
                    ICSharpCode.TextEditor.Gui.InsightWindow.IInsightDataProvider idp = new DefaultInsightDataProvider(-1, false, key);
                    if (insightWindow == null || insightWindow.InsightProviderStackLength == 0)
                    {
                        insightWindow = new PABCNETInsightWindow(VisualPABCSingleton.MainForm, editor);
                        insightWindow.Font = new Font(Constants.CompletionInsightWindowFontName, insightWindow.Font.Size);
                        insightWindow.Closed += new EventHandler(CloseInsightWindow);
                    }
                    else
                    {
                        (idp as DefaultInsightDataProvider).defaultIndex = insightWindow.GetCurrentData();
                        (idp as DefaultInsightDataProvider).cur_param_num = (insightWindow.GetInsightProvider() as DefaultInsightDataProvider).param_count;

                    }
                    insightWindow.AddInsightDataProvider(idp, editor.FileName);
                    insightWindow.ShowInsightWindow();
                    editor.Focus();
                }
            }
            else if (key == ' ')
            {
                if (VisualPABCSingleton.MainForm.UserOptions.CodeCompletionDot)
                {
                    PascalABCCompiler.Parsers.KeywordKind keyw = KeywordChecker.TestForKeyword(editor.Document.TextContent, editor.ActiveTextAreaControl.TextArea.Caret.Offset - 1);
                    if (keyw == PascalABCCompiler.Parsers.KeywordKind.New || keyw == PascalABCCompiler.Parsers.KeywordKind.Uses)
                    {
                        completionDataProvider = new CodeCompletionProvider();
                        codeCompletionWindow = PABCNETCodeCompletionWindow.ShowCompletionWindow(
                            VisualPABCSingleton.MainForm,					// The parent window for the completion window
                            editor, 					// The text editor to show the window for
                            editor.FileName,		// Filename - will be passed back to the provider
                            completionDataProvider,		// Provider to get the list of possible completions
                            ' ',							// Key pressed - will be passed to the provider
                            true,
                            false,
                            keyw
                        );
                        CodeCompletionNamesOnlyInModuleAction.comp_windows[editor.ActiveTextAreaControl.TextArea] = codeCompletionWindow;
                        if (codeCompletionWindow != null)
                            codeCompletionWindow.Closed += new EventHandler(CloseCodeCompletionWindow);
                        //return true;
                    }
                }
            }
            else if (key == '\n')
            {
                if (VisualPABCSingleton.MainForm.UserOptions.AllowCodeCompletion)
                {
                    try
                    {
                        CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[editor.FileName];
                        if (dconv != null)
                        {
                            CodeCompletion.SymScope ss = dconv.FindScopeByLocation(editor.ActiveTextAreaControl.TextArea.Caret.Line + 1, editor.ActiveTextAreaControl.TextArea.Caret.Column + 1);
                            ss.IncreaseEndLine();
                        }
                    }
                    catch
                    {

                    }
                }
            }
            else if (codeCompletionWindow == null && VisualPABCSingleton.MainForm.UserOptions.EnableSmartIntellisense && (char.IsLetter(key) || key == '_'))
            {
                if (VisualPABCSingleton.MainForm.UserOptions.CodeCompletionDot)
                {
                    PascalABCCompiler.Parsers.KeywordKind keyw = KeywordChecker.TestForKeyword(editor.Document.TextContent, editor.ActiveTextAreaControl.TextArea.Caret.Offset - 1);
                    if (CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.IsDefinitionIdentifierAfterKeyword(keyw))
                        return false;

                    // если не первый символ выражения (предыдущий тоже буква или "_"), то не открываем новое окно
                    if (editor.ActiveTextAreaControl.TextArea.Caret.Offset > 0 && (char.IsLetterOrDigit(editor.Document.TextContent[editor.ActiveTextAreaControl.TextArea.Caret.Offset - 1]) || editor.Document.TextContent[editor.ActiveTextAreaControl.TextArea.Caret.Offset - 1] == '_'))
                        return false;
                    
                    completionDataProvider = new CodeCompletionProvider();
                    codeCompletionWindow = PABCNETCodeCompletionWindow.ShowCompletionWindowWithFirstChar(
                        VisualPABCSingleton.MainForm,					// The parent window for the completion window
                        editor, 					// The text editor to show the window for
                        editor.FileName,		// Filename - will be passed back to the provider
                        completionDataProvider,		// Provider to get the list of possible completions
                        key,						// Key pressed - will be passed to the provider
                        keyw
                    );
                    CodeCompletionNamesOnlyInModuleAction.comp_windows[editor.ActiveTextAreaControl.TextArea] = codeCompletionWindow;
                    if (codeCompletionWindow != null)
                        codeCompletionWindow.Closed += new EventHandler(CloseCodeCompletionWindow);

                }
            }
            /*else if (codeCompletionWindow == null && key == '\n')
            {
            	if (mainForm.UserOptions.AllowCodeFormatting)
                {
            		if (CodeCompletion.CodeCompletionController.currentParser == null) return false;
            		string bracket = CodeCompletion.CodeCompletionController.currentParser.LanguageInformation.GetBodyStartBracket();
            		string end_bracket = CodeCompletion.CodeCompletionController.currentParser.LanguageInformation.GetBodyEndBracket();
            		if (bracket != null)
            		{
            			int i = editor.ActiveTextAreaControl.TextArea.Caret.Offset-1;
            			int j = bracket.Length-1;
            			bool eq=true;
            			while (i >= 0 && j >= 0)
            			{
            				if (editor.Document.TextContent[i] != bracket[j])
            				{
            					eq = false;
            					break;
            				}
            				i--; j--;
            			}
            			if (eq && j<0)
            			{
            				TextArea textArea = editor.ActiveTextAreaControl.TextArea;
            				int col = textArea.Caret.Column-bracket.Length;
							textArea.InsertString("\n\n"+end_bracket);
							textArea.Caret.Column = 0;
							textArea.Caret.Line = textArea.Caret.Line-1;
							textArea.Caret.Column = VisualPABCSingleton.MainForm.UserOptions.CursorTabCount+col;
							return true;
            			}
            		}
            	}
            }*/

            return false;
        }

        void CloseInsightWindow(object sender, EventArgs e)
        {
            if (insightWindow != null)
            {
                insightWindow.Closed -= new EventHandler(CloseInsightWindow);
                insightWindow.Dispose();
                insightWindow = null;
            }
        }

        void CloseCodeCompletionWindow(object sender, EventArgs e)
        {
            if (codeCompletionWindow != null)
            {
                codeCompletionWindow.Closed -= new EventHandler(CloseCodeCompletionWindow);
                CodeCompletionProvider.disp.Reset();
                CodeCompletion.AssemblyDocCache.Reset();
                CodeCompletion.UnitDocCache.Reset();
                codeCompletionWindow.Dispose();
                CodeCompletionNamesOnlyInModuleAction.comp_windows[editor.ActiveTextAreaControl.TextArea] = null;
                codeCompletionWindow = null;
            }
        }
    }
}

