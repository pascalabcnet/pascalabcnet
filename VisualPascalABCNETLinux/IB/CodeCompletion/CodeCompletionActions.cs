// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using PascalABCCompiler;
using PascalABCCompiler.Parsers;

namespace VisualPascalABC
{
    public class CodeCompletionActionsManager
    {
        static CodeCompletionProvider ccp;
        public static CodeTemplateManager templateManager;

        public static void Rename(TextArea textArea)
        {
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return;
            IDocument doc = textArea.Document;
            string textContent = doc.TextContent;
            ccp = new CodeCompletionProvider();
            string name = null;
            string expressionResult = FindOnlyIdent(textContent, textArea, ref name).Trim(' ', '\n', '\t', '\r');
            string new_val = null;
            List<SymbolsViewerSymbol> refers = ccp.Rename(expressionResult, name, textArea.MotherTextEditorControl.FileName, textArea.Caret.Line, textArea.Caret.Column, ref new_val);
            if (refers == null || new_val == null) return;
            int addit = 0;
            PascalABCCompiler.SourceLocation tmp = new PascalABCCompiler.SourceLocation(null, 0, 0, 0, 0);
            string file_name = null;
            if (CodeCompletion.CodeCompletionNameHelper.Helper.IsKeyword(new_val))
                new_val = "&" + new_val;
            foreach (SymbolsViewerSymbol svs in refers)
            {
                if (svs.SourceLocation.BeginPosition.Line != tmp.BeginPosition.Line)
                    addit = 0;
                else if (svs.SourceLocation.BeginPosition.Column < tmp.BeginPosition.Column)
                    addit = 0;
                if (svs.SourceLocation.FileName != file_name)
                {
                    CodeFileDocumentControl cfdoc = VisualPABCSingleton.MainForm.FindTab(svs.SourceLocation.FileName);
                    if (cfdoc == null) continue;
                    doc = cfdoc.TextEditor.ActiveTextAreaControl.TextArea.Document;
                    file_name = svs.SourceLocation.FileName;
                }
                tmp = svs.SourceLocation;
                TextLocation tl_beg = new TextLocation(svs.SourceLocation.BeginPosition.Column - 1 + addit, svs.SourceLocation.BeginPosition.Line - 1);
                //TextLocation tl_end = new TextLocation(svs.SourceLocation.EndPosition.Line,svs.SourceLocation.EndPosition.Column);
                int offset = doc.PositionToOffset(tl_beg);
                
                addit += new_val.Length - name.Length;
                doc.Replace(offset, name.Length, new_val);
                doc.CommitUpdate();
            }
        }

        public static void RenameUnit(string FileName, string new_val)
        {
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return;
            ccp = new CodeCompletionProvider();
            IDocument doc = null;
            CodeCompletion.CodeCompletionController controller = new CodeCompletion.CodeCompletionController();
            string text = WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.SourceFilesProvider(FileName, PascalABCCompiler.SourceFileOperation.GetText) as string;
            PascalABCCompiler.SyntaxTree.compilation_unit cu = controller.ParseOnlySyntaxTree(FileName, text);
            if (cu == null)
                return;
            PascalABCCompiler.SyntaxTree.ident unitName = null;
            if (cu is PascalABCCompiler.SyntaxTree.unit_module)
            {
                unitName = (cu as PascalABCCompiler.SyntaxTree.unit_module).unit_name.idunit_name;
            }
            else  if (cu is PascalABCCompiler.SyntaxTree.program_module)
            {
                if ((cu as PascalABCCompiler.SyntaxTree.program_module).program_name == null)
                    return;
                unitName = (cu as PascalABCCompiler.SyntaxTree.program_module).program_name.prog_name;
            }
            if (unitName.source_context == null)
                return;
            List<SymbolsViewerSymbol> refers = ccp.Rename(unitName.name, unitName.name, FileName, unitName.source_context.begin_position.line_num, unitName.source_context.begin_position.column_num);
            if (refers == null || new_val == null) return;
            int addit = 0;
            PascalABCCompiler.SourceLocation tmp = new PascalABCCompiler.SourceLocation(null, 0, 0, 0, 0);
            string file_name = null;
            VisualPABCSingleton.MainForm.StopTimer();
            WorkbenchServiceFactory.CodeCompletionParserController.StopParseThread();
            foreach (IFileInfo fi in ProjectFactory.Instance.CurrentProject.SourceFiles)
            {
                WorkbenchServiceFactory.CodeCompletionParserController.RegisterFileForParsing(fi.Path);
            }
            WorkbenchServiceFactory.CodeCompletionParserController.ParseInThread();
            foreach (SymbolsViewerSymbol svs in refers)
            {
                if (svs.SourceLocation.BeginPosition.Line != tmp.BeginPosition.Line)
                    addit = 0;
                else if (svs.SourceLocation.BeginPosition.Column < tmp.BeginPosition.Column)
                    addit = 0;
                if (svs.SourceLocation.FileName != file_name)
                {
                    CodeFileDocumentControl cfdoc = VisualPABCSingleton.MainForm.FindTab(svs.SourceLocation.FileName);
                    if (cfdoc == null) continue;
                    doc = cfdoc.TextEditor.ActiveTextAreaControl.TextArea.Document;
                    file_name = svs.SourceLocation.FileName;
                }
                tmp = svs.SourceLocation;
                TextLocation tl_beg = new TextLocation(svs.SourceLocation.BeginPosition.Column - 1, svs.SourceLocation.BeginPosition.Line - 1);
                //TextLocation tl_end = new TextLocation(svs.SourceLocation.EndPosition.Line,svs.SourceLocation.EndPosition.Column);
                int offset = doc.PositionToOffset(tl_beg);
                //addit += new_val.Length - unitName.name.Length;
                doc.Replace(offset, unitName.name.Length, new_val);
                doc.CommitUpdate();
            }
            WorkbenchServiceFactory.CodeCompletionParserController.RunParseThread();
            VisualPABCSingleton.MainForm.StartTimer();
        }

        public static List<Position> GetDefinitionPosition(TextArea textArea, bool only_check)
        {
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return new List<Position>();

            IDocument doc = textArea.Document;
            string textContent = doc.TextContent;
            ccp = new CodeCompletionProvider();
            string full_expr;
            string expressionResult = FindFullExpression(textContent, textArea, out ccp.keyword, out full_expr);

            if (expressionResult != null)
                expressionResult = expressionResult.Trim();


            // Проверка, что компилируем Паскаль временно  EVA 10.11.2024
            if (CodeCompletion.CodeCompletionController.CurrentLanguage == Languages.Facade.LanguageProvider.Instance.MainLanguage)
            {
                if (expressionResult != full_expr && full_expr.StartsWith("("))
                    return new List<Position>();

                if (full_expr != null && full_expr.Contains("^"))
                {
                    full_expr = full_expr.Replace("^", "");
                }
            }

            List<Position> poses = ccp.GetDefinition(full_expr, textArea.MotherTextEditorControl.FileName, textArea.Caret.Line, textArea.Caret.Column, only_check);

            if (poses == null || poses.Count == 0)
                poses = ccp.GetDefinition(expressionResult, textArea.MotherTextEditorControl.FileName, textArea.Caret.Line, textArea.Caret.Column, only_check);

            return poses;
        }

        public static List<Position> GetRealizationPosition(TextArea textArea)
        {
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return new List<Position>();
            IDocument doc = textArea.Document;
            string textContent = doc.TextContent;
            ccp = new CodeCompletionProvider();
            string full_expr;
            string expressionResult = FindFullExpression(textContent, textArea, out ccp.keyword, out full_expr);
            List<Position> poses = ccp.GetRealization(full_expr, textArea.MotherTextEditorControl.FileName, textArea.Caret.Line, textArea.Caret.Column);
            if (poses == null || poses.Count == 0)
                poses = ccp.GetRealization(expressionResult, textArea.MotherTextEditorControl.FileName, textArea.Caret.Line, textArea.Caret.Column);
            return poses;
        }

        public static List<SymbolsViewerSymbol> FindReferences(TextArea textArea)
        {
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return new List<SymbolsViewerSymbol>();
            ccp = new CodeCompletionProvider();
            string full_expr;
            string expressionResult = FindFullExpression(textArea.Document.TextContent, textArea, out ccp.keyword, out full_expr);
            return ccp.FindReferences(expressionResult, textArea.MotherTextEditorControl.FileName, textArea.Caret.Line, textArea.Caret.Column, false);
        }

        public static void GotoRealization(TextArea textArea)
        {
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return;
            position = GetRealizationPosition(textArea);
            if (position == null || position.Count == 0) return;
            if (position.Count == 1)
            {
                Position pos = position[0];
                if (pos.file_name != null)
                {
                    WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.ExecuteSourceLocationAction(
                        new PascalABCCompiler.SourceLocation(pos.file_name, pos.line, pos.column, pos.line, pos.column), VisualPascalABCPlugins.SourceLocationAction.GotoBeg);
                }
            }
            else
            {
                List<SymbolsViewerSymbol> svs_lst = new List<SymbolsViewerSymbol>();
                foreach (Position pos in position)
                {
                    if (pos.file_name != null)
                        svs_lst.Add(new SymbolsViewerSymbol(new PascalABCCompiler.SourceLocation(pos.file_name, pos.line, pos.column, pos.end_line, pos.end_column), CodeCompletionProvider.ImagesProvider.IconNumberGotoText));
                }
                VisualPABCSingleton.MainForm.FindSymbolResults.showInThread = false;
                VisualPABCSingleton.MainForm.ShowFindResults(svs_lst);
                VisualPABCSingleton.MainForm.FindSymbolResults.showInThread = true;

            }
        }

        public static bool CanGoTo(TextArea textArea)
        {
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return false;
            List<Position> poses = GetDefinitionPosition(textArea, true);
            if (poses == null || poses.Count == 0) return false;
            foreach (Position pos in poses)
                if (pos.from_metadata || pos.file_name != null && (bool)WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.SourceFilesProvider(pos.file_name, PascalABCCompiler.SourceFileOperation.Exists))
                    return true;
            return false;
            //string file_name = GetDefinitionPosition(textArea).file_name;
            //return file_name != null && (bool)VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.SourceFilesProvider(file_name, PascalABCCompiler.SourceFileOperation.Exists);
        }

        public static bool CanGoToRealization(TextArea textArea)
        {
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return false;
            List<Position> poses = GetRealizationPosition(textArea);
            if (poses == null || poses.Count == 0) return false;
            foreach (Position pos in poses)
                if (pos.file_name != null && (bool)WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.SourceFilesProvider(pos.file_name, PascalABCCompiler.SourceFileOperation.Exists))
                    return true;
            return false;
            //        	string file_name = GetRealizationPosition(textArea).file_name;
            //          return file_name != null && (bool)VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.SourceFilesProvider(file_name, PascalABCCompiler.SourceFileOperation.Exists);
        }

        public static bool CanFindReferences(TextArea textArea)
        {
            try
            {
                if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return false;
                ccp = new CodeCompletionProvider();
                string full_expr;
                string expressionResult = FindFullExpression(textArea.Document.TextContent, textArea, out ccp.keyword, out full_expr);
                if (expressionResult != null)
                {
                    expressionResult = expressionResult.Trim(' ', '\n', '\t', '\r');
                    return expressionResult != null && expressionResult != "";
                }
                return expressionResult != null && expressionResult != "";
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool CanGenerateRealization(TextArea textArea)
        {
            try
            {
                if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return false;
                ccp = new CodeCompletionProvider();
                Position pos = new Position();
                //string text = "procedure Test(a : integer);\n begin \n x := 1; \n end;";//ccp.GetRealizationTextToAdd(out pos);
                string text = ccp.GetRealizationTextToAdd(textArea.MotherTextEditorControl.FileName, textArea.Caret.Line, textArea.Caret.Column, ref pos, textArea);
                return text != null && pos.file_name != null;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static void GenerateMethodImplementationHeaders(TextArea textArea)
        {
            try
            {
                if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return;
                ccp = new CodeCompletionProvider();
                Position pos = new Position();
                //string text = "procedure Test(a : integer);\n begin \n x := 1; \n end;";//ccp.GetRealizationTextToAdd(out pos);
                string text = ccp.GetMethodImplementationTextToAdd(textArea.MotherTextEditorControl.FileName, textArea.Caret.Line, textArea.Caret.Column, ref pos, textArea);
                if (!string.IsNullOrEmpty(text) && pos.file_name != null)
                {
                    textArea.Caret.Line = pos.line - 1;
                    textArea.Caret.Column = 0;
                    textArea.InsertString(text);
                    textArea.Caret.Line = pos.line - 1;
                    textArea.Caret.Column = 0;
                }
            }
            catch (System.Exception e)
            {

            }
        }
        private static PABCNETCodeCompletionWindow codeCompletionWindow;
        private static TextArea _textArea;
        public static void GenerateOverridableMethods(TextArea textArea)
        {
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return;
            ccp = new CodeCompletionProvider();
            _textArea = textArea;
            int off = textArea.Caret.Offset;
            codeCompletionWindow = PABCNETCodeCompletionWindow.ShowOverridableMethodsCompletionWindows
                (VisualPABCSingleton.MainForm, textArea.MotherTextEditorControl, textArea.MotherTextEditorControl.FileName, ccp);
            CodeCompletionAllNamesAction.comp_windows[textArea] = codeCompletionWindow;
            if (codeCompletionWindow != null)
            {
                // ShowCompletionWindow can return null when the provider returns an empty list
                codeCompletionWindow.Closed += new EventHandler(CloseCodeCompletionWindow);
            }
        }

        private static void CloseCodeCompletionWindow(object sender, EventArgs e)
        {
            if (codeCompletionWindow != null)
            {
                codeCompletionWindow.Closed -= new EventHandler(CloseCodeCompletionWindow);
                CodeCompletionProvider.disp.Reset();
                codeCompletionWindow.Dispose();
                CodeCompletion.AssemblyDocCache.Reset();
                CodeCompletion.UnitDocCache.Reset();
                codeCompletionWindow = null;
            }
            CodeCompletionAllNamesAction.comp_windows[_textArea] = null;
        }

        public static void GenerateClassOrMethodRealization(TextArea textArea)
        {
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return;
            ccp = new CodeCompletionProvider();
            Position pos = new Position();
            //string text = "procedure Test(a : integer);\n begin \n x := 1; \n end;";//ccp.GetRealizationTextToAdd(out pos);
            string text = ccp.GetRealizationTextToAdd(textArea.MotherTextEditorControl.FileName, textArea.Caret.Line, textArea.Caret.Column, ref pos, textArea);
            if (text != null && pos.file_name != null)
            {
                textArea.Caret.Line = pos.line - 1;
                textArea.Caret.Column = pos.column - 1;
                textArea.InsertString(text);
                textArea.Caret.Line = pos.line + 4 - 1;
                textArea.Caret.Column = VisualPABCSingleton.MainForm.UserOptions.CursorTabCount + 1;
            }
        }

        private static void find_cursor_pos(string s, out int line, out int col)
        {
            line = 0;
            col = 0;
            for (int i = 0; i < s.Length; i++)
                if (s[i] == '\n')
                {
                    line++;
                    col = 0;
                }
                else if (s[i] == '|')
                {
                    break;
                }
                else
                {
                    col++;
                }
        }

        public static void GenerateTemplate(string pattern, TextArea textArea) => GenerateTemplate(pattern, textArea, templateManager);

        public static void GenerateTemplate(string pattern, TextArea textArea, CodeTemplateManager templateManager, bool withPatternLength = true, Func<string,string> PostAction = null)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                IDocument doc = textArea.Document;

                if (textArea.SelectionManager.HasSomethingSelected) // удаление выделенного
                {
                    var isel = textArea.SelectionManager.SelectionCollection[0];
                    textArea.Caret.Line = isel.StartPosition.Line;
                    textArea.Caret.Column = isel.StartPosition.Column;
                    textArea.SelectionManager.RemoveSelectedText();
                }
                
                int line = textArea.Caret.Line;
                int col = textArea.Caret.Column;
                string name = templateManager.GetTemplateHeader(pattern);
                if (name == null) return;
                string templ = templateManager.GetTemplate(name);
                int ind = withPatternLength ? pattern.Length : 0;
                int cline;
                int ccol;
                find_cursor_pos(templ, out cline, out ccol);
                sb.Append(templ);
                int cur_ind = templ.IndexOf('|');
                if (cur_ind != -1)
                    sb = sb.Remove(cur_ind, 1);
                sb = sb.Replace("<filename>", Path.GetFileNameWithoutExtension(textArea.MotherTextEditorControl.FileName));
                templ = sb.ToString();
                int i = 0;
                i = templ.IndexOf('\n', i);
                while (i != -1)
                {
                    if (i + 1 < sb.Length)
                        sb.Insert(i + 1, " ", col - ind);
                    i = sb.ToString().IndexOf('\n', i + 1);
                }
                TextLocation tl_beg = new TextLocation(col - ind, line);
                int offset = doc.PositionToOffset(tl_beg);
                doc.Replace(offset, ind, "");
                doc.CommitUpdate();
                textArea.Caret.Column = col - ind;
                var sbs = sb.ToString();
                if (PostAction != null)
                {
                    sbs = PostAction(sbs);
                }
                textArea.InsertString(sbs);
                textArea.Caret.Line = line + cline;
                textArea.Caret.Column = col - ind + ccol;
            }
            catch (Exception e)
            {

            }
        }

        private static bool should_insert_comment(TextArea textArea)
        {
            string content = textArea.Document.TextContent;
            LineSegment seg = textArea.Document.GetLineSegment(textArea.Caret.Line);
            string line = textArea.Document.GetText(seg);
            if (line.Trim(' ', '\t') != "//")
                return false;
            if (textArea.Caret.Line + 1 >= textArea.Document.TotalNumberOfLines)
                return false;
            return true;
        }

        private static string get_next_line(TextArea textArea)
        {
            if (textArea.Caret.Line + 1 >= textArea.Document.TotalNumberOfLines)
                return null;
            LineSegment seg = textArea.Document.GetLineSegment(textArea.Caret.Line + 1);
            return textArea.Document.GetText(seg).Trim(' ', '\t');
        }

        private static string get_possible_description(TextArea textArea, out int len)
        {
            LineSegment seg = textArea.Document.GetLineSegment(textArea.Caret.Line);
            len = 0;
            string s = textArea.Document.GetText(seg).TrimStart(' ', '\t');
            if (s != "///" && s.StartsWith("///"))
            {
                len = s.Length - 3;
                return s.Substring(3).Trim(' ', '\t');
            }
            return null;
        }

        public static bool GenerateCommentTemplate(TextArea textArea)
        {
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return false;
            ccp = new CodeCompletionProvider();
            //if (!should_insert_comment(textArea))
            //	return false;
            string lineText = get_next_line(textArea);
            string addit = CodeCompletion.CodeCompletionController.CurrentLanguage.LanguageIntellisenseSupport.GetDocumentTemplate(
                lineText, textArea.Document.TextContent, textArea.Caret.Line, textArea.Caret.Column, textArea.Caret.Offset);
            if (addit == null)
                return false;
            int col = textArea.Caret.Column;
            int line = textArea.Caret.Line;
            int len;
            string desc = get_possible_description(textArea, out len);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" <summary>");
            for (int i = 0; i < col - 3; i++)
                sb.Append(' ');
            sb.Append("/// ");
            if (desc != null)
                sb.Append(desc);
            sb.AppendLine();
            for (int i = 0; i < col - 3; i++)
                sb.Append(' ');
            sb.Append("/// </summary>");
            if (addit != "")
            {
                sb.Append(addit);
            }
            if (desc == null)
                textArea.InsertString(sb.ToString());
            else
            {
                IDocument doc = textArea.Document;
                TextLocation tl_beg = new TextLocation(col, line);
                //TextLocation tl_end = new TextLocation(svs.SourceLocation.EndPosition.Line,svs.SourceLocation.EndPosition.Column);
                int offset = doc.PositionToOffset(tl_beg);
                doc.Replace(offset, len, "");
                doc.CommitUpdate();
                textArea.InsertString(sb.ToString());
            }
            textArea.Caret.Line = line + 1;
            textArea.Caret.Column = col + 1;
            return true;
        }

        public static void GotoDefinition(TextArea _textArea)
        {
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable()) return;
            position = GetDefinitionPosition(_textArea, false);
            if (position == null) return;
            if (position.Count == 1)
            {
                Position pos = position[0];
                if (pos.file_name != null)
                {
                    WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.ExecuteSourceLocationAction(
                        new PascalABCCompiler.SourceLocation(pos.file_name, pos.line, pos.column, pos.line, pos.column), VisualPascalABCPlugins.SourceLocationAction.GotoBeg);
                }
                else
                    if (pos.from_metadata)
                    {
                        WorkbenchServiceFactory.FileService.OpenTabWithText(pos.metadata_title, pos.metadata);
                        WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.ExecuteSourceLocationAction(
                        new PascalABCCompiler.SourceLocation(pos.file_name, pos.line, pos.column, pos.line, pos.column), VisualPascalABCPlugins.SourceLocationAction.GotoBeg);
                    }
            }
            else
            {
                List<SymbolsViewerSymbol> svs_lst = new List<SymbolsViewerSymbol>();
                foreach (Position pos in position)
                {
                    if (pos.file_name != null)
                        svs_lst.Add(new SymbolsViewerSymbol(new PascalABCCompiler.SourceLocation(pos.file_name, pos.line, pos.column, pos.end_line, pos.end_column), CodeCompletionProvider.ImagesProvider.IconNumberGotoText));
                }
                VisualPABCSingleton.MainForm.FindSymbolResults.showInThread = false;
                VisualPABCSingleton.MainForm.ShowFindResults(svs_lst);
                VisualPABCSingleton.MainForm.FindSymbolResults.showInThread = true;
            }
        }

        private static string FindFullExpression(string Text, TextArea _textArea, out PascalABCCompiler.Parsers.KeywordKind keyw, out string full_expr)
        {
            keyw = PascalABCCompiler.Parsers.KeywordKind.None;
            string expr_without_brackets = null;
            full_expr = null;
            if (CodeCompletion.CodeCompletionController.IntellisenseAvailable())
            {
                full_expr = CodeCompletion.CodeCompletionController.CurrentLanguage.LanguageIntellisenseSupport.FindExpressionFromAnyPosition(_textArea.Caret.Offset, Text, _textArea.Caret.Line, _textArea.Caret.Column, out keyw, out expr_without_brackets);
                return expr_without_brackets;
            }
            return null;
        }

        private static string FindOnlyIdent(string Text, TextArea _textArea, ref string name)
        {
            return CodeCompletion.CodeCompletionController.CurrentLanguage.LanguageIntellisenseSupport.FindOnlyIdentifier(_textArea.Caret.Offset, Text, _textArea.Caret.Line, _textArea.Caret.Column, ref name);
        }


        private static List<Position> position;
    }

    public class VirtualMethodsAction : ICSharpCode.TextEditor.Actions.AbstractEditAction
    {
        public override void Execute(TextArea textArea)
        {
            CodeCompletionActionsManager.GenerateOverridableMethods(textArea);
        }
    }

    public class ClassOrMethodRealizationAction : ICSharpCode.TextEditor.Actions.AbstractEditAction
    {
        public override void Execute(TextArea textArea)
        {
            CodeCompletionActionsManager.GenerateClassOrMethodRealization(textArea);
        }
    }

    public class GenerateMethodImplementationHeadersAction : ICSharpCode.TextEditor.Actions.AbstractEditAction
    {
        public override void Execute(TextArea textArea)
        {
            CodeCompletionActionsManager.GenerateMethodImplementationHeaders(textArea);
        }
    }

    public class GotoAction : ICSharpCode.TextEditor.Actions.AbstractEditAction
    {
        public override void Execute(TextArea textArea)
        {
            CodeCompletionActionsManager.GotoDefinition(textArea);
        }
    }

    public class CodeCompletionNamesOnlyInModuleAction : CodeCompletionAllNamesAction
    {
        public override void Execute(TextArea _textArea)
        {
            key = '\0';
            base.Execute(_textArea);
        }
    }

    public class CodeFormattingAction : ICSharpCode.TextEditor.Actions.AbstractEditAction
    {
        public override void Execute(TextArea textArea)
        {
            if (WorkbenchServiceFactory.DebuggerManager.IsRunning || !CodeCompletion.CodeCompletionController.IntellisenseAvailable())
                return;
            WorkbenchServiceFactory.Workbench.ErrorsListWindow.ClearErrorList();
            VisualPABCSingleton.MainForm.CurrentCodeFileDocument.DeselectAll();
            CodeFormatters.CodeFormatter cf = new CodeFormatters.CodeFormatter(VisualPABCSingleton.MainForm.UserOptions.TabIndent);
            List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
            //PascalABCCompiler.SyntaxTree.syntax_tree_node sn =
            //    MainForm.VisualEnvironmentCompiler.Compiler.ParsersController.Compile(
            //    file_name, TextEditor.Text, null, Errors, PascalABCCompiler.Parsers.ParseMode.Normal);
            string text = WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.SourceFilesProvider(VisualPABCSingleton.MainForm.CurrentCodeFileDocument.FileName, PascalABCCompiler.SourceFileOperation.GetText) as string;

            PascalABCCompiler.SyntaxTree.compilation_unit cu =
                CodeCompletion.CodeCompletionController.CurrentLanguage.Parser.GetCompilationUnitForFormatter(
                VisualPABCSingleton.MainForm.CurrentCodeFileDocument.FileName,
               text, //VisualPascalABC.Form1.Form1_object._currentCodeFileDocument.TextEditor.Text,
                Errors,
                new List<PascalABCCompiler.Errors.CompilerWarning>());

            if (Errors.Count == 0)
            {
                string formattedText = cf.FormatTree(text, cu, textArea.Caret.Line + 1, textArea.Caret.Column + 1);
                bool success = true;
                if (success)
                {
                    //WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.ExecuteAction(VisualPascalABCPlugins.VisualEnvironmentCompilerAction.SetCurrentSourceFileTextFormatting, "");
                    WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.ExecuteAction(VisualPascalABCPlugins.VisualEnvironmentCompilerAction.SetCurrentSourceFileTextFormatting, formattedText);
                    WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.ExecuteSourceLocationAction(
                                new PascalABCCompiler.SourceLocation(VisualPABCSingleton.MainForm.CurrentCodeFileDocument.FileName, cf.Line, cf.Column, cf.Line, cf.Column), VisualPascalABCPlugins.SourceLocationAction.GotoBeg);
                }
            }
            else
            {
                WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.ExecuteAction(VisualPascalABCPlugins.VisualEnvironmentCompilerAction.AddMessageToErrorListWindow, new List<PascalABCCompiler.Errors.Error>(new PascalABCCompiler.Errors.Error[] { Errors[0] }));
            }
        }
    }

    public class CodeCompletionAllNamesAction : ICSharpCode.TextEditor.Actions.AbstractEditAction
    {
        PABCNETCodeCompletionWindow codeCompletionWindow;
        public static TextArea textArea;
        public static Hashtable comp_windows = new Hashtable();
        public static bool is_begin = false;
        protected char key = '_';

        private string get_prev_text(string text, int off)
        {
            StringBuilder sb = new StringBuilder();
            while (off >= 0 && char.IsLetterOrDigit(text[off]))
            {
                sb.Insert(0, text[off--]);
            }
            return sb.ToString();
        }

        public override void Execute(TextArea _textArea)
        {
            //try
            {
                textArea = _textArea;
                int off = textArea.Caret.Offset;
                string text = textArea.Document.TextContent.Substring(0, textArea.Caret.Offset);
                if (key == '\0')
                    if (off > 2 && text[off - 1] == '/' && text[off - 2] == '/' && text[off - 3] == '/')
                    {
                        CodeCompletionActionsManager.GenerateCommentTemplate(textArea);
                        return;
                    }
                    else
                    {
                        string prev = get_prev_text(text, off - 1);
                        if (!string.IsNullOrEmpty(prev))
                        {
                            CodeCompletionActionsManager.GenerateTemplate(prev, textArea);
                            return;
                        }
                    }
                if (!WorkbenchServiceFactory.Workbench.UserOptions.CodeCompletionDot)
                    return;
                if (CodeCompletion.CodeCompletionController.CurrentLanguage == null) return;
                CodeCompletionProvider completionDataProvider = new CodeCompletionProvider();

                bool is_pattern = false;


                is_begin = true;

                completionDataProvider.preSelection = CodeCompletion.CodeCompletionController.CurrentLanguage.LanguageIntellisenseSupport.FindPattern(off, text, out is_pattern);

                if (!is_pattern && off > 0 && text[off - 1] == '.')
                    key = '_';//was '$'
                codeCompletionWindow = PABCNETCodeCompletionWindow.ShowCompletionWindow(
                    VisualPABCSingleton.MainForm,					// The parent window for the completion window
                    textArea.MotherTextEditorControl, 					// The text editor to show the window for
                    textArea.MotherTextEditorControl.FileName,		// Filename - will be passed back to the provider
                    completionDataProvider,		// Provider to get the list of possible completions
                    key,							// Key pressed - will be passed to the provider
                    false,
                    false,
                    PascalABCCompiler.Parsers.KeywordKind.None
                );
                key = '_';
                CodeCompletionNamesOnlyInModuleAction.comp_windows[textArea] = codeCompletionWindow;

                if (codeCompletionWindow != null)
                {
                    // ShowCompletionWindow can return null when the provider returns an empty list
                    codeCompletionWindow.Closed += new EventHandler(CloseCodeCompletionWindow);
                }
            }
            //catch (Exception e)
            {

            }
        }

        public void CloseCodeCompletionWindow(object sender, EventArgs e)
        {
            if (codeCompletionWindow != null)
            {
                codeCompletionWindow.Closed -= new EventHandler(CloseCodeCompletionWindow);
                CodeCompletionProvider.disp.Reset();
                CodeCompletion.AssemblyDocCache.Reset();
                CodeCompletion.UnitDocCache.Reset();
                codeCompletionWindow.Dispose();
                codeCompletionWindow = null;
            }
            comp_windows[textArea] = null;
        }
    }
}

