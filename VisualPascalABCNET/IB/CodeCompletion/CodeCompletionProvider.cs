// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using ICSharpCode.TextEditor.Document;
using PascalABCCompiler.Parsers;

namespace VisualPascalABC
{

    public class CodeCompletionProvider : ICompletionDataProvider
    {
        public string preSelection;
        public static CodeCompletionImagesProvider ImagesProvider;
        public static DefaultDispatcher disp;
        public bool ByFirstChar;

        static CodeCompletionProvider()
        {
            ImagesProvider = new CodeCompletionImagesProvider();
            disp = new DefaultDispatcher();
            CodeCompletion.AssemblyDocCache.dispatcher = disp;
            CodeCompletion.UnitDocCache.dispatcher = disp;
        }

        public CodeCompletionProvider()
        {
        }

        public ImageList ImageList
        {
            get
            {
                return ImagesProvider.ImageList;
            }
        }

        public string PreSelection
        {
            get
            {
                return preSelection;
            }
        }

        public int DefaultIndex
        {
            get
            {
                return defaultIndex;
            }
        }

        public UserDefaultCompletionData DefaultCompletionElement
        {
            get
            {
                return defaultCompletionElement;
            }
        }

        private UserDefaultCompletionData defaultCompletionElement;
        private int defaultIndex = -1;

        public CompletionDataProviderKeyResult ProcessKey(char key)
        {
            if (char.IsLetterOrDigit(key) || key == '_')
            {
                return CompletionDataProviderKeyResult.NormalKey;
            }
            else
            {
                // key triggers insertion of selected items
                return CompletionDataProviderKeyResult.InsertionKey;
            }
        }

        /// <summary>
        /// Called when entry should be inserted. Forward to the insertion action of the completion data.
        /// </summary>
        public bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key)
        {
            textArea.Caret.Position = textArea.Document.OffsetToPosition(insertionOffset);
            if (!(data as UserDefaultCompletionData).IsOnOverrideWindow)
            {
                int ind = data.Text.IndexOf('<');
                if (ind != -1 && data.Text.Length > ind + 1 && data.Text[ind + 1] == '>') data.Text = data.Text.Substring(0, ind);
            }
            else
                data.Text = data.Description;

            return data.InsertAction(textArea, key);
        }

        public PascalABCCompiler.Parsers.KeywordKind keyword;

        public List<PascalABCCompiler.Parsers.Position> GetDefinition(string expr, string fileName, int line, int column, bool only_check)
        {
            List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
            PascalABCCompiler.SyntaxTree.expression e = null;
            List<PascalABCCompiler.Parsers.Position> loc = null;
            if (VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.compilerLoaded)
                e = WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.StandartCompiler.ParsersController.GetExpression("test" + System.IO.Path.GetExtension(fileName), expr, Errors);
            if (e == null /*|| Errors.Count > 0*/) return loc;
            CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[fileName];
            if (dconv == null) return loc;
            loc = dconv.GetDefinition(e, line, column, keyword, only_check);
            return loc;
        }

        private string construct_header(string meth, CodeCompletion.ProcScope ps, int tabCount)
        {
            //if (CodeCompletion.CodeCompletionController.currentParser != null)
            return CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.ConstructHeader(meth, ps, tabCount);
        }

        private string construct_header(CodeCompletion.ProcRealization ps, int tabCount)
        {
            return CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.ConstructHeader(ps, tabCount);
        }

        public CodeCompletion.SymScope FindScopeByLocation(string fileName, int line, int col)
        {
            CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[fileName];
            if (dconv == null) return null;
            return dconv.FindScopeByLocation(line, col);
        }

        public string GetMethodImplementationTextToAdd(string fileName, int line, int col, ref PascalABCCompiler.Parsers.Position pos, TextArea textArea)
        {
            CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[fileName];
            if (dconv == null) return null;
            CodeCompletion.ProcScope[] procs = dconv.GetNotImplementedMethodHeaders(line, col, ref pos);
            if (procs == null || procs.Length == 0) return null;
            System.Text.StringBuilder sb = new StringBuilder();
            for (int i = 0; i < procs.Length; i++)
            {
                string meth = construct_header(procs[i] as CodeCompletion.ProcRealization, VisualPABCSingleton.MainForm.UserOptions.CursorTabCount);
                sb.Append(meth);
                sb.Append('\n');
            }
            return sb.ToString();
        }

        public string GetRealizationTextToAdd(string fileName, int line, int col, ref PascalABCCompiler.Parsers.Position pos, TextArea textArea)
        {
            CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[fileName];
            if (dconv == null) return null;
            CodeCompletion.ProcScope[] procs = dconv.GetNotImplementedMethods(line, col, ref pos);
            if (procs == null) return null;
            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append("\n\n");
            for (int i = 0; i < procs.Length; i++)
            {
                int off = textArea.Document.PositionToOffset(new TextLocation(procs[i].GetPosition().column - 1, procs[i].GetPosition().line - 1));
                string meth = textArea.Document.GetText(off, textArea.Document.PositionToOffset(new TextLocation(procs[i].GetPosition().end_column - 1, procs[i].GetPosition().end_line - 1)) - off + 1);
                meth = construct_header(meth, procs[i], VisualPABCSingleton.MainForm.UserOptions.CursorTabCount);
                sb.Append(meth);
                if (i < procs.Length - 1) sb.Append('\n');
            }
            return sb.ToString();
        }

        public List<PascalABCCompiler.Parsers.Position> GetRealization(string expr, string fileName, int line, int column)
        {
            List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
            PascalABCCompiler.SyntaxTree.expression e = null;
            List<PascalABCCompiler.Parsers.Position> loc = null;
            if (VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.compilerLoaded)
                e = VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.StandartCompiler.ParsersController.GetExpression("test" + System.IO.Path.GetExtension(fileName), expr, Errors);
            if (e == null /*|| Errors.Count > 0*/) return loc;
            CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[fileName];
            if (dconv == null) return loc;
            loc = dconv.GetRealization(e, line, column, keyword);
            return loc;
        }

        private RenameForm rf;

        public List<SymbolsViewerSymbol> Rename(string expr, string name, string fileName, int line, int column)
        {
            List<SymbolsViewerSymbol> refers = FindReferences(expr, fileName, line, column, true);
            return refers;
        }

        public List<SymbolsViewerSymbol> Rename(string expr, string name, string fileName, int line, int column, ref string new_val)
        {
            if (rf == null)
            {
                rf = new RenameForm();
                Form1StringResources.SetTextForAllControls(rf);
            }
            rf.EditValue = name.Trim(' ');
            DialogResult dr = rf.ShowDialog();
            if (dr == DialogResult.OK)
            {
                new_val = rf.EditValue;
                List<SymbolsViewerSymbol> refers = FindReferences(expr, fileName, line, column, true);
                if (refers == null) return null;
                return refers;
            }
            return null;
        }

      
        public List<SymbolsViewerSymbol> FindReferences(string expr, string fileName, int line, int column, bool for_refact)
        {
            List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
            PascalABCCompiler.SyntaxTree.expression e = null;
            if (VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.compilerLoaded)
                e = VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.StandartCompiler.ParsersController.GetExpression("test" + System.IO.Path.GetExtension(fileName), expr, Errors);
            if (e == null) return new List<SymbolsViewerSymbol>();
            CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[fileName];
            if (dconv == null) return new List<SymbolsViewerSymbol>();
            List<SymbolsViewerSymbol> lst = InternalFindReferences(fileName, e, line, column, for_refact);
            //if (lst != null && expr != null && for_refact)
            //lst.Insert(0,new SymbolsViewerSymbol(new PascalABCCompiler.SourceLocation(fileName,line+1,column,line+1,column+expr.Length),ImagesProvider.IconNumberGotoText));
            return lst;
        }

        private List<SymbolsViewerSymbol> InternalFindReferences(string fname, PascalABCCompiler.SyntaxTree.expression expr, int line, int col, bool for_refact)
        {
            List<PascalABCCompiler.Parsers.Position> lst = new List<PascalABCCompiler.Parsers.Position>();
            List<SymbolsViewerSymbol> svs_lst = new List<SymbolsViewerSymbol>();
            try
            {
                CodeCompletion.DomConverter dc = CodeCompletion.CodeCompletionController.comp_modules[fname] as CodeCompletion.DomConverter;
                IBaseScope fnd_scope = null;
                IBaseScope cur_sc = null;
                if (dc != null)
                {
                    fnd_scope = dc.GetSymDefinition(expr, line, col, keyword);
                    cur_sc = dc.FindScopeByLocation(line, col);
                }
                if (fnd_scope != null)
                {
                    foreach (string FileName in CodeCompletionParserController.open_files.Keys)
                    {
                        CodeCompletion.CodeCompletionController controller = new CodeCompletion.CodeCompletionController();
                        string text = VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.SourceFilesProvider(FileName, PascalABCCompiler.SourceFileOperation.GetText) as string;
                        PascalABCCompiler.SyntaxTree.compilation_unit cu = controller.ParseOnlySyntaxTree(FileName, text);
                        if (cu != null)
                        {
                            dc = CodeCompletion.CodeCompletionController.comp_modules[FileName] as CodeCompletion.DomConverter;
                            CodeCompletion.ReferenceFinder rf = null;
                            if (dc != null && dc.stv.entry_scope != null)
                            {
                                rf = new CodeCompletion.ReferenceFinder(fnd_scope, dc.stv.entry_scope, cu, FileName, for_refact);
                                lst.AddRange(rf.FindPositions());
                            }
                        }
                    }
                    PascalABCCompiler.Parsers.Position p = fnd_scope.GetPosition();
                    bool need_add_def = !for_refact; // true
                    //if (for_refact)
                    //foreach (PascalABCCompiler.Parsers.Position pos in lst)
                    //    if (p.file_name == pos.file_name && p.line == pos.line && p.column == pos.column && p.end_line == pos.end_line && p.end_column == pos.end_column)
                    //    {
                    //        need_add_def = false;
                    //        break;
                    //    }
                    if (p.file_name != null && need_add_def)       
                        svs_lst.Add(new SymbolsViewerSymbol(new PascalABCCompiler.SourceLocation(p.file_name, p.line, p.column, p.end_line, p.end_column), ImagesProvider.GetPictureNum(fnd_scope.SymbolInfo)));
                    foreach (PascalABCCompiler.Parsers.Position pos in lst)
                    {
                        if (pos.file_name != null)
                            svs_lst.Add(new SymbolsViewerSymbol(new PascalABCCompiler.SourceLocation(pos.file_name, pos.line, pos.column, pos.end_line, pos.end_column), ImagesProvider.IconNumberGotoText));
                    }
                }
            }
            catch (Exception e)
            {

            }
            GC.Collect();
            return svs_lst;
            //return svs_lst.ToArray();
        }

        public string FindExpression(int off, string Text, int line, int col)
        {
            if (CodeCompletion.CodeCompletionController.CurrentParser != null)
                return CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.FindExpression(off, Text, line, col, out keyword);
            return null;
        }

        private string SkipNew(int off, string Text)
        {
            int tmp = off;
            string expr = null;
            while (off >= 0 && Char.IsLetterOrDigit(Text[off])) off--;
            while (off >= 0 && (Text[off] == ' ' || char.IsControl(Text[off]))) off--;
            if (off >= 1 && Text[off] == '=' && Text[off - 1] == ':')
            {
                off -= 2;
                while (off >= 0 && (Text[off] == ' ' || char.IsControl(Text[off]))) off--;
                if (off >= 0 && (Text[off] == '_' || char.IsLetterOrDigit(Text[off]) || Text[off] == ']'))
                    expr = FindExpression(off + 1, Text, 0, 0);
            }
            return expr;
        }

        public ICompletionData[] GetCompletionDataByFirst(int off, string Text, int line, int col, char charTyped, PascalABCCompiler.Parsers.KeywordKind keyw)
        {
            List<ICompletionData> resultList = new List<ICompletionData>();
            List<ICompletionData> lst = new List<ICompletionData>();
            try
            {
                CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[FileName];
                /*if (dconv == null && CodeCompletion.CodeCompletionNameHelper.system_unit_file_full_name != null
                    && (keyw == CodeCompletion.KeywordKind.kw_colon || keyw == CodeCompletion.KeywordKind.kw_of))
                {
                	dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[CodeCompletion.CodeCompletionNameHelper.system_unit_file_full_name];
                	special_module = true;
                }*/
                string pattern = charTyped.ToString();
                string[] keywords = CodeCompletion.CodeCompletionNameHelper.Helper.GetKeywords();
                if (CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.IsTypeAfterKeyword(keyw))
                {
                    keywords = CodeCompletion.CodeCompletionNameHelper.Helper.GetTypeKeywords();
                }
                if (!CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.IsNamespaceAfterKeyword(keyw))
                    foreach (string key in keywords)
                    {
                        //if (key.StartsWith(pattern, StringComparison.CurrentCultureIgnoreCase))
                        resultList.Add(new UserDefaultCompletionData(key, null, ImagesProvider.IconNumberKeyword, false));
                    }
                PascalABCCompiler.Parsers.SymInfo[] mis = null;
                if (CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.IsNamespaceAfterKeyword(keyw))
                {
                    mis = CodeCompletion.DomConverter.standard_units;
                }
                if (dconv != null)
                {
                    //if (keyw == PascalABCCompiler.Parsers.KeywordKind.Colon || keyw == PascalABCCompiler.Parsers.KeywordKind.Of || keyw == PascalABCCompiler.Parsers.KeywordKind.TypeDecl)
                    if (CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.IsTypeAfterKeyword(keyw))
                        mis = dconv.GetTypeByPattern(pattern, line, col, true, VisualPABCSingleton.MainForm.UserOptions.CodeCompletionNamespaceVisibleRange);
                    else if (CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.IsNamespaceAfterKeyword(keyw) && mis == null)
                        mis = dconv.GetNamespaces();
                    else
                        mis = dconv.GetNameByPattern(null, line, col, true, VisualPABCSingleton.MainForm.UserOptions.CodeCompletionNamespaceVisibleRange);
                }
                Hashtable cache = new Hashtable();
                int num = 0;
                if (mis != null)
                {
                    bool stop = false;
                    ICompletionData data = CompletionDataDispatcher.GetLastUsedItem(charTyped);
                    foreach (PascalABCCompiler.Parsers.SymInfo mi in mis)
                    {
                        if (mi.not_include) continue;

                        if (cache.Contains(mi.name))
                            continue;

                        UserDefaultCompletionData ddd = new UserDefaultCompletionData(mi.name, mi.describe, ImagesProvider.GetPictureNum(mi), false);
                        if (!stop && data != null && string.Compare(mi.name, data.Text, true) == 0)
                        {
                            defaultCompletionElement = ddd;
                            stop = true;
                        }
                        else if (!stop && data == null && mi.name.StartsWith(charTyped.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        {
                            //defaultCompletionElement = ddd;
                            lst.Add(ddd);
                            //stop = true;
                        }
                        disp.Add(mi, ddd);
                        resultList.Add(ddd);
                        cache[mi.name] = mi;
                    }
                    //resultList.Sort();
                    //defaultCompletionElement = resultList[0] as DefaultCompletionData;
                }
            }
            catch (Exception e)
            {

            }

            lst.Sort();
            if (lst.Count > 0) defaultCompletionElement = lst[0] as UserDefaultCompletionData;
            ICompletionData[] res_arr = resultList.ToArray();
            this.ByFirstChar = true;
            return res_arr;
        }

        public ICompletionData[] GetCompletionData(int off, string Text, int line, int col, char charTyped, PascalABCCompiler.Parsers.KeywordKind keyw)
        {
            List<ICompletionData> resultList = new List<ICompletionData>();
            try
            {
                string pattern = null;
                string expr = null;
                bool ctrl_space = charTyped == '\0' || charTyped == '_';
                bool shift_space = charTyped == '\0';
                bool new_space = keyw == PascalABCCompiler.Parsers.KeywordKind.New;
                if (ctrl_space)
                {
                    bool is_pattern = false;
                    pattern = CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.FindPattern(off, Text, out is_pattern);
                }
                else if (new_space)
                {
                    expr = CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.SkipNew(off - 1, Text, ref keyword);
                }
                else
                    if (!new_space && keyw != PascalABCCompiler.Parsers.KeywordKind.Uses)
                        if (charTyped != '$')
                            expr = FindExpression(off, Text, line, col);
                        else expr = FindExpression(off - 1, Text, line, col);
                List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
                PascalABCCompiler.SyntaxTree.expression e = null;
                if (ctrl_space && !shift_space && (pattern == null || pattern == ""))
                {
                    string[] keywords = CodeCompletion.CodeCompletionNameHelper.Helper.GetKeywords();
                    foreach (string key in keywords)
                    {
                        //if (key.StartsWith(pattern, StringComparison.CurrentCultureIgnoreCase))
                        resultList.Add(new UserDefaultCompletionData(key, null, ImagesProvider.IconNumberKeyword, false));
                    }
                }
                if (!ctrl_space && expr != null)
                {
                    e = WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.StandartCompiler.ParsersController.GetTypeAsExpression("test" + System.IO.Path.GetExtension(FileName), expr, Errors);
                    if (e == null)
                    {
                        Errors.Clear();
                        e = WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.StandartCompiler.ParsersController.GetExpression("test" + System.IO.Path.GetExtension(FileName), expr, Errors);
                    }
                    if ((e == null || Errors.Count > 0) && !new_space) return null;
                }
                SymInfo[] mis = null;
                CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[FileName];
                if (dconv == null)
                {
                    if (keyw == PascalABCCompiler.Parsers.KeywordKind.Uses)
                        mis = CodeCompletion.DomConverter.standard_units;
                    else if (!ctrl_space)
                        return new ICompletionData[0];
                }
                string fname = FileName;
                SymInfo sel_si = null;
                string last_used_member = null;
                if (dconv != null)
                {
                    if (new_space)
                        mis = dconv.GetTypes(e, line, col, out sel_si);
                    else if (keyw == PascalABCCompiler.Parsers.KeywordKind.Uses && mis == null)
                        mis = dconv.GetNamespaces();
                    else
                        if (!ctrl_space)
                        {
                            CodeCompletion.SymScope dot_sc = null;
                            mis = dconv.GetName(e, expr, line, col, keyword, ref dot_sc);
                            if (dot_sc != null && VisualPABCSingleton.MainForm.UserOptions.EnableSmartIntellisense)
                            {
                                CompletionDataDispatcher.AddMemberBeforeDot(dot_sc);
                                last_used_member = CompletionDataDispatcher.GetRecentUsedMember(dot_sc);
                            }
                        }
                        else
                            mis = dconv.GetNameByPattern(pattern, line, col, charTyped == '_', VisualPABCSingleton.MainForm.UserOptions.CodeCompletionNamespaceVisibleRange);
                }
                Hashtable cache = null;
                if (!CodeCompletion.CodeCompletionController.CurrentParser.CaseSensitive)
                    cache = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
                else
                    cache = new Hashtable();
                int num = 0;
                if (mis != null)
                {
                    bool stop = false;
                    ICompletionData data = null;

                    foreach (SymInfo mi in mis)
                    {
                        if (mi.not_include) continue;
                        if (cache.Contains(mi.name))
                            continue;

                        UserDefaultCompletionData ddd = new UserDefaultCompletionData(mi.addit_name != null ? mi.addit_name : mi.name, mi.describe, ImagesProvider.GetPictureNum(mi), false);

                        disp.Add(mi, ddd);
                        resultList.Add(ddd);
                        cache[mi.name] = mi;
                        /*if (VisualPABCSingleton.MainForm.UserOptions.EnableSmartIntellisense && mi.name != null && mi.name != "" && data == null)
                        {
                        		data = CompletionDataDispatcher.GetLastUsedItem(mi.name[0]);
                        		if (data != null && data.Text == ddd.Text) data = ddd;
                        }*/
                        if (last_used_member != null && last_used_member == mi.name)
                        {
                            defaultCompletionElement = ddd;
                        }
                        if (sel_si != null && mi == sel_si)
                        {
                            defaultCompletionElement = ddd;
                            stop = true;
                        }
                    }

                    if (defaultCompletionElement == null && data != null)
                        defaultCompletionElement = data as UserDefaultCompletionData;
                }
            }
            catch (Exception e)
            {

            }
            return resultList.ToArray();
        }

        private CodeCompletion.CodeCompletionController controller;
        private string FileName;
        private string Text;

        public void CompileInThread()
        {
            controller.Compile(FileName, Text /*+ ")))));end."*/);
        }

        public ICompletionData[] ddata;

        public ICompletionData[] GenerateCompletionDataWithKeyword(string fileName, TextArea textArea, char charTyped, PascalABCCompiler.Parsers.KeywordKind keyw)
        {
            controller = new CodeCompletion.CodeCompletionController();
            int off = textArea.Caret.Offset;
            string text = textArea.Document.TextContent.Substring(0, textArea.Caret.Offset);
            //controller.Compile(fileName, text /*+ ")))));end."*/);
            FileName = fileName; Text = text;
            //System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(CompileInThread));
            //th.Start();
            //while (th.ThreadState == System.Threading.ThreadState.Running) {}
            ICompletionData[] data = GetCompletionData(off, text, textArea.Caret.Line, textArea.Caret.Column, charTyped, keyw);
            CodeCompletion.AssemblyDocCache.CompleteDocumentation();
            CodeCompletion.UnitDocCache.CompleteDocumentation();
            controller = null;
            //GC.Collect();
            return data;
        }

        public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
        {
            return null;
        }

        public ICompletionData[] GenerateCompletionDataForOverridableMethods(string fileName, TextArea textArea)
        {
            controller = new CodeCompletion.CodeCompletionController();
            List<ICompletionData> lst = new List<ICompletionData>();
            int line = textArea.Caret.Line;
            int col = textArea.Caret.Column;
            try
            {
                CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[fileName];
                SymInfo[] mis = null;
                if (dconv != null)
                {
                    mis = dconv.GetOverridableMethods(line, col);
                }
                if (mis != null)
                {
                    foreach (SymInfo mi in mis)
                    {
                        UserDefaultCompletionData ddd = new UserDefaultCompletionData(mi.name, mi.describe, ImagesProvider.GetPictureNum(mi), true);
                        lst.Add(ddd);
                    }
                }
            }
            catch (Exception e)
            {

            }

            //lst.Sort();
            ICompletionData[] res_arr = lst.ToArray();
            controller = null;
            return res_arr;
        }

        public ICompletionData[] GenerateCompletionDataByFirstChar(string fileName, TextArea textArea, char charTyped, PascalABCCompiler.Parsers.KeywordKind keyw)
        {
            controller = new CodeCompletion.CodeCompletionController();
            int off = textArea.Caret.Offset;
            string text = textArea.Document.TextContent.Substring(0, textArea.Caret.Offset);
            //controller.Compile(fileName, text /*+ ")))));end."*/);
            FileName = fileName; Text = text;
            //System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(CompileInThread));
            //th.Start();
            //while (th.ThreadState == System.Threading.ThreadState.Running) {}
            ICompletionData[] data = GetCompletionDataByFirst(off, text, textArea.Caret.Line, textArea.Caret.Column, charTyped, keyw);
            CodeCompletion.AssemblyDocCache.CompleteDocumentation();
            CodeCompletion.UnitDocCache.CompleteDocumentation();
            controller = null;
            //GC.Collect();
            return data;
        }
    }
}
