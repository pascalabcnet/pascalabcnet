// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using Languages.Facade;
using PascalABCCompiler.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
                if (ind != -1 && data.Text.Length > ind + 1 && data.Text[ind + 1] == '>')
                    data.Text = data.Text.Substring(0, ind);
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

            ILanguage language = LanguageProvider.Instance.SelectLanguageByExtension(fileName);

            if (language == null)
                return null;

            if (VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.compilerLoaded)
                e = language.Parser.GetExpression("test" + System.IO.Path.GetExtension(fileName), expr, Errors, new List<PascalABCCompiler.Errors.CompilerWarning>());
            if (e == null)
                return loc;
            CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[fileName];
            if (dconv == null)
                return loc;
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
                //sb.Append('\n');
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

                if (i < procs.Length - 1)
                {
                    sb.Append(meth);
                    sb.Append('\n');
                }
                else
                    sb.Append(meth.Trim());

            }
            return sb.ToString();
        }

        public List<PascalABCCompiler.Parsers.Position> GetRealization(string expr, string fileName, int line, int column)
        {
            List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
            PascalABCCompiler.SyntaxTree.expression e = null;
            List<PascalABCCompiler.Parsers.Position> loc = null;
            if (VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.compilerLoaded)
                e = LanguageProvider.Instance.SelectLanguageByExtensionSafe(fileName)?.Parser.GetExpression("test" + System.IO.Path.GetExtension(fileName), expr, Errors, new List<PascalABCCompiler.Errors.CompilerWarning>());
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
            rf.EditValue = name.Trim(' ').Replace("&", "");
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
                e = LanguageProvider.Instance.SelectLanguageByExtensionSafe(fileName)?.Parser.GetExpression("test" + System.IO.Path.GetExtension(fileName), expr, Errors, new List<PascalABCCompiler.Errors.CompilerWarning>());
            if (e == null) return new List<SymbolsViewerSymbol>();
            CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[fileName];
            if (dconv == null) return new List<SymbolsViewerSymbol>();
            List<SymbolsViewerSymbol> lst = InternalFindReferences(fileName, e, line, column, for_refact);
            //if (lst != null && expr != null && for_refact)
            //lst.Insert(0,new SymbolsViewerSymbol(new PascalABCCompiler.SourceLocation(file_name,line+1,column,line+1,column+expr.Length),ImagesProvider.IconNumberGotoText));
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
                            if (dc != null && dc.visitor.entry_scope != null)
                            {
                                rf = new CodeCompletion.ReferenceFinder(fnd_scope, dc.visitor.entry_scope, cu, FileName, for_refact);
                                lst.AddRange(rf.FindPositions());
                            }
                        }
                    }
                    PascalABCCompiler.Parsers.Position p = fnd_scope.GetPosition();
                    bool need_add_def = !for_refact; // true
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

        /// <summary>
        /// Возвращает подсказки определяемые первым символом выражения, введенного пользователем
        /// </summary>
        public ICompletionData[] GetCompletionDataByFirst(int line, int col, char charTyped, KeywordKind keyw)
        {
            List<ICompletionData> resultList = new List<ICompletionData>();
            try
            {

                ILanguageInformation languageInformation = CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation;

                List<string> keywords;

                bool isTypeAfterKeyword = false;

                // если по смыслу должен вводиться тип данных
                if (languageInformation.IsTypeAfterKeyword(keyw))
                {
                    keywords = CodeCompletion.CodeCompletionNameHelper.Helper.GetTypeKeywords();
                    isTypeAfterKeyword = true;
                }
                else
                {
                    keywords = CodeCompletion.CodeCompletionNameHelper.Helper.GetKeywords();
                }

                bool isNamespaceAfterKeyword = false;

                // конструкция типа "uses"
                if (languageInformation.IsNamespaceAfterKeyword(keyw))
                {
                    isNamespaceAfterKeyword = true;
                }
                else
                {
                    resultList.AddRange(keywords.Select(keyword =>
                        new UserDefaultCompletionData(keyword, null, ImagesProvider.IconNumberKeyword, false)));
                }

                SymInfo[] symInfos = GetSymInfosForCompletionDataByFirst(line, col, isTypeAfterKeyword, isNamespaceAfterKeyword, charTyped.ToString());

                if (symInfos != null)
                {
                    bool languageCaseSensitive = LanguageProvider.Instance.SelectLanguageByExtension(FileName).CaseSensitive;

                    languageInformation.RenameOrExcludeSpecialNames(symInfos);

                    AddCompletionDatasByFirstForSymInfos(resultList, charTyped, symInfos, languageCaseSensitive);
                    
                    //resultList.Sort();
                    //defaultCompletionElement = resultList[0] as DefaultCompletionData;
                }
            }
            catch (Exception) { }
            
            this.ByFirstChar = true;
            
            return resultList.ToArray();
        }

        /// <summary>
        /// Добавляет в resultList (итоговый список подсказок) данные, соответствующие переданному массиву типа SymInfo[].
        /// Используется для случая ввода пользователем первого символа выражения
        /// </summary>
        private void AddCompletionDatasByFirstForSymInfos(List<ICompletionData> resultList, char charTyped, SymInfo[] symInfos, bool languageCaseSensitive)
        {
            HashSet<string> symbolsAdded = languageCaseSensitive ? new HashSet<string>() : new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);

            List<ICompletionData> candidatesForDefault = new List<ICompletionData>();

            bool stop = false;
            ICompletionData lastUsedItem = CompletionDataDispatcher.GetLastUsedItem(charTyped);

            foreach (SymInfo symInfo in symInfos)
            {
                if (symInfo == null || symInfo.not_include) continue;

                string nameToShow = GetDisplayedName(symInfo);

                if (symbolsAdded.Contains(nameToShow + symInfo.kind))
                    continue;

                UserDefaultCompletionData completionData = new UserDefaultCompletionData(nameToShow, symInfo.description, ImagesProvider.GetPictureNum(symInfo), false);

                StringComparison stringComparison = languageCaseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;

                // если мы выбирали что-то раньше из списка подсказок, то считаем это элементом по умолчанию
                if (!stop && lastUsedItem != null && string.Equals(nameToShow, lastUsedItem.Text, stringComparison))
                {
                    defaultCompletionElement = completionData;
                    stop = true;
                }
                // иначе формируем список подходящих подсказок - "кандидатов" для элемента по умолчанию
                else if (!stop && lastUsedItem == null && nameToShow.StartsWith(charTyped.ToString(), stringComparison))
                {
                    //defaultCompletionElement = ddd;
                    candidatesForDefault.Add(completionData);
                    //stop = true;
                }
                
                disp.Add(symInfo, completionData);

                resultList.Add(completionData);

                symbolsAdded.Add(nameToShow + symInfo.kind);
            }

            if (candidatesForDefault.Count > 0)
                defaultCompletionElement = candidatesForDefault.Min() as UserDefaultCompletionData; // здесь выбирается минимальное по длине
        }


        /// <summary>
        /// Формирует массив данных из таблицы символов для подсказок в случае введения пользователем первой буквы выражения
        /// </summary>
        private SymInfo[] GetSymInfosForCompletionDataByFirst(int line, int col, bool isTypeAfterKeyword, bool isNamespaceAfterKeyword, string pattern)
        {
            CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[FileName];
            /*if (dconv == null && CodeCompletion.CodeCompletionNameHelper.system_unit_file_full_name != null
                && (keyw == CodeCompletion.KeywordKind.kw_colon || keyw == CodeCompletion.KeywordKind.kw_of))
            {
                dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[CodeCompletion.CodeCompletionNameHelper.system_unit_file_full_name];
                special_module = true;
            }*/

            SymInfo[] symInfos = null;

            if (dconv == null)
            {
                if (isNamespaceAfterKeyword)
                {
                    symInfos = CodeCompletion.DomConverter.standard_units;
                }
            }
            else
            {
                //if (keyw == PascalABCCompiler.Parsers.KeywordKind.Colon || keyw == PascalABCCompiler.Parsers.KeywordKind.Of || keyw == PascalABCCompiler.Parsers.KeywordKind.TypeDecl)
                if (isTypeAfterKeyword)
                {
                    symInfos = dconv.GetTypeByPattern(pattern, line, col, true, VisualPABCSingleton.MainForm.UserOptions.CodeCompletionNamespaceVisibleRange);
                }
                else if (isNamespaceAfterKeyword)
                {
                    if (WorkbenchServiceFactory.Workbench.UserOptions.EnableSmartIntellisense)
                        symInfos = dconv.GetNamespaces();
                    else
                        symInfos = CodeCompletion.DomConverter.standard_units;
                }
                else
                {
                    // интересно, что передается pattern = null  EVA
                    symInfos = dconv.GetNameByPattern(null, line, col, true, VisualPABCSingleton.MainForm.UserOptions.CodeCompletionNamespaceVisibleRange);
                }
            }

            return symInfos;
        }

        /// <summary>
        /// Вспомогательная струтура для метода GetCompletionData, хранит информацию о действиях пользователя
        /// </summary>
        private struct ActionContext
        {
            public bool dotPressed;
            public bool ctrlSpace;
            public bool shiftSpace;
            public bool spaceAfterNew;
            public bool spaceAfterUses;
        }

        /// <summary>
        /// Возвращает массив подсказок для случая нажатия пользователем "триггерной" клавиши
        /// </summary>
        public ICompletionData[] GetCompletionData(int off, string text, int line, int col, char charTyped, KeywordKind keywordKind)
        {
            List<ICompletionData> resultList = new List<ICompletionData>();
            try
            {
                // поменять на обращение к CodeCompletionController.CurrentLanguage
                ILanguage currentLanguage = LanguageProvider.Instance.SelectLanguageByExtension(FileName);

                var context = new ActionContext()
                {
                    dotPressed = charTyped == '.',
                    ctrlSpace = charTyped == '_',
                    shiftSpace = charTyped == '\0',
                    spaceAfterNew = keywordKind == KeywordKind.New,
                    spaceAfterUses = keywordKind == KeywordKind.Uses
                };

                string expressionText = GetExpressionTextForCompletionData(off, text, line, col,
                    currentLanguage.LanguageInformation, in context, out var insidePatternWithDots, out var ctrlOrShiftSpaceAfterDot, out var pattern);

                // добавляем ключевые слова в случае "ctrl + space", нажатых в "пустом" месте
                if (!ctrlOrShiftSpaceAfterDot && context.ctrlSpace && string.IsNullOrEmpty(pattern))
                {
                    var keywords = CodeCompletion.CodeCompletionNameHelper.Helper.GetKeywords();

                    resultList.AddRange(keywords.Select(keyword =>
                        new UserDefaultCompletionData(keyword, null, ImagesProvider.IconNumberKeyword, false)));
                }

                PascalABCCompiler.SyntaxTree.expression expr = null;
                
                // для "ctrl + space" и "shift + space" дерево expression не требуется (кроме случая insidePatternWithDots)
                if ((context.dotPressed || context.spaceAfterNew || context.spaceAfterUses || insidePatternWithDots) && expressionText != null)
                {
                    expr = GetExpressionForCompletionData(currentLanguage.Parser,
                        in context, expressionText, insidePatternWithDots, out var shouldReturnNull);

                    if (shouldReturnNull)
                        return null;
                }

                SymInfo[] symInfos = GetSymInfosForCompletionData(line, col, in context, currentLanguage.CaseSensitive,
                    expressionText, ctrlOrShiftSpaceAfterDot, insidePatternWithDots, pattern, expr, out var selectedSymInfo, out var lastUsedMember, out var shouldReturnNull2);

                if (shouldReturnNull2)
                    return null;

                if (symInfos != null)
                {
                    currentLanguage.LanguageInformation.RenameOrExcludeSpecialNames(symInfos);

                    AddCompletionDatasForSymInfos(resultList, currentLanguage.CaseSensitive, symInfos, selectedSymInfo, lastUsedMember);
                }
            }
            catch (Exception) { }

            return resultList.ToArray();
        }

        /// <summary>
        /// Добавляет в resultList (итоговый список подсказок) данные, соответствующие переданному массиву типа SymInfo[].
        /// Используется для случая нажатия пользователем "триггерной" клавиши
        /// </summary>
        private void AddCompletionDatasForSymInfos(List<ICompletionData> resultList, bool languageCaseSensitive, SymInfo[] symInfos, SymInfo selectedSymInfo, string lastUsedMember)
        {
            // ICompletionData data = null;

            HashSet<string> symbolsAdded = languageCaseSensitive ? new HashSet<string>() : new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);

            foreach (SymInfo symInfo in symInfos)
            {
                if (symInfo == null || symInfo.not_include)
                    continue;

                string nameToShow = GetDisplayedName(symInfo);
                
                if (symbolsAdded.Contains(nameToShow + symInfo.kind))
                    continue;

                UserDefaultCompletionData completionData = new UserDefaultCompletionData(nameToShow, symInfo.description, ImagesProvider.GetPictureNum(symInfo), false);

                disp.Add(symInfo, completionData);

                resultList.Add(completionData);

                symbolsAdded.Add(nameToShow + symInfo.kind);

                /*if (VisualPABCSingleton.MainForm.UserOptions.EnableSmartIntellisense && mi.name != null && mi.name != "" && data == null)
                {
                        data = CompletionDataDispatcher.GetLastUsedItem(mi.name[0]);
                        if (data != null && data.Text == ddd.Text) data = ddd;
                }*/

                if (lastUsedMember != null && lastUsedMember == nameToShow || selectedSymInfo != null && symInfo == selectedSymInfo)
                {
                    defaultCompletionElement = completionData;
                }
            }

            /*if (defaultCompletionElement == null && data != null)
                defaultCompletionElement = data as UserDefaultCompletionData;*/
        }

        /// <summary>
        /// Формирует массив данных из таблицы символов для подсказок в случае нажатия пользователем "триггерной" клавиши 
        /// (в зависимости от введенного выражения и другого контекста)
        /// </summary>
        private SymInfo[] GetSymInfosForCompletionData(int line, int col, in ActionContext context, bool languageCaseSensitive, string expressionText, bool ctrlOrShiftSpaceAfterDot, bool insidePatternWithDots, string pattern, PascalABCCompiler.SyntaxTree.expression expr, out SymInfo selectedSymInfo, out string lastUsedMember, out bool shouldReturnNull)
        {
            SymInfo[] symInfos = null;

            shouldReturnNull = false;

            selectedSymInfo = null;
            lastUsedMember = null;

            CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[FileName];

            if (dconv == null)
            {
                // в данном случае возвращаем пустой массив ICompletionData
                if (!context.spaceAfterUses && !context.ctrlSpace)
                    shouldReturnNull = true;

                if (context.spaceAfterUses)
                    symInfos = CodeCompletion.DomConverter.standard_units;
            }
            else
            {
                if (context.spaceAfterNew)
                {
                    symInfos = dconv.GetTypes(expr, line, col, out selectedSymInfo);
                }
                else if (context.spaceAfterUses)
                {
                    if (WorkbenchServiceFactory.Workbench.UserOptions.EnableSmartIntellisense)
                        symInfos = dconv.GetNamespaces();
                    else
                        symInfos = CodeCompletion.DomConverter.standard_units;
                }
                // нажатие ctrl + space и shift + space сразу после точки приравнивается к нажатию точки
                else if (context.dotPressed || ctrlOrShiftSpaceAfterDot)
                {
                    CodeCompletion.SymScope dotScope = null;
                    symInfos = dconv.GetName(expr, expressionText, line, col, keyword, ref dotScope);

                    if (dotScope != null && VisualPABCSingleton.MainForm.UserOptions.EnableSmartIntellisense)
                    {
                        CompletionDataDispatcher.AddMemberBeforeDot(dotScope);
                        lastUsedMember = CompletionDataDispatcher.GetRecentUsedMember(dotScope);
                    }
                }
                // ctrl + space или shift + space
                else if (context.ctrlSpace || context.shiftSpace)
                {
                    CodeCompletion.SymScope dotScope = null;

                    // если мы в цепочечном выражении с точками
                    if (insidePatternWithDots)
                    {

                        symInfos = dconv.GetName(expr, expressionText, line, col, keyword, ref dotScope)
                            .Where(symInfo => symInfo.name.StartsWith(pattern,
                            languageCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
                            .ToArray();
                    }
                    else
                        symInfos = dconv.GetNameByPattern(pattern, line, col, context.ctrlSpace, VisualPABCSingleton.MainForm.UserOptions.CodeCompletionNamespaceVisibleRange);
                }

            }

            return symInfos;
        }

        /// <summary>
        /// Возвращает дерево выражения, введенного пользователем перед нажатием "триггерной" клавиши
        /// </summary>
        private PascalABCCompiler.SyntaxTree.expression GetExpressionForCompletionData(IParser parser, in ActionContext context, string expressionText, bool insidePatternWithDots, out bool shouldReturnNull)
        {
            shouldReturnNull = false;

            List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
            List<PascalABCCompiler.Errors.CompilerWarning> Warnings = new List<PascalABCCompiler.Errors.CompilerWarning>();

            var expr = parser.GetTypeAsExpression("test" + System.IO.Path.GetExtension(FileName), expressionText, Errors, Warnings);
            if (expr == null)
            {
                Errors.Clear();
                expr = parser.GetExpression("test" + System.IO.Path.GetExtension(FileName), expressionText, Errors, Warnings);
            }

            if ((expr == null || Errors.Count > 0) && !context.spaceAfterNew)
                shouldReturnNull = true;

            return expr;
        }

        /// <summary>
        /// Получение текста выражения, введенного пользователем перед нажатием "триггерной" клавиши
        /// </summary>
        private string GetExpressionTextForCompletionData(int off, string text, int line, int col, ILanguageInformation languageInformation, in ActionContext context, out bool insidePatternWithDots, out bool ctrlOrShiftSpaceAfterDot, out string pattern)
        {

            string expressionText = null;
            pattern = null;
            insidePatternWithDots = false;
            ctrlOrShiftSpaceAfterDot = false;

            if (context.ctrlSpace || context.shiftSpace)
            {

                pattern = languageInformation.FindPattern(off, text, out var isPattern);

                // в конце выражения точка
                if (!isPattern && text[off - 1] == '.')
                {
                    ctrlOrShiftSpaceAfterDot = true;
                }
                
                // если нужно подсказать все варианты после точки, то поведение как в случае context.dotPressed
                if (isPattern && text[off - pattern.Length - 1] == '.' || ctrlOrShiftSpaceAfterDot)
                {
                    insidePatternWithDots = true;
                    expressionText = FindExpression(off - (pattern?.Length ?? 0) - 1, text, line, col);
                }

            }
            else if (context.spaceAfterNew)
            {
                expressionText = languageInformation.SkipNew(off - 1, text, ref keyword);
            }
            else if (context.dotPressed) // keywordKind != KeywordKind.Uses
            {
                expressionText = FindExpression(off, text, line, col);
            }

            return expressionText;
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
            //controller.Compile(file_name, text /*+ ")))));end."*/);
            FileName = fileName; Text = text;
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
                        UserDefaultCompletionData ddd = new UserDefaultCompletionData(mi.name, mi.description, ImagesProvider.GetPictureNum(mi), true);
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
            // int off = textArea.Caret.Offset;
            string text = textArea.Document.TextContent.Substring(0, textArea.Caret.Offset);
            //controller.Compile(file_name, text /*+ ")))));end."*/);
            FileName = fileName; Text = text;
            ICompletionData[] data = GetCompletionDataByFirst(textArea.Caret.Line, textArea.Caret.Column, charTyped, keyw);
            CodeCompletion.AssemblyDocCache.CompleteDocumentation();
            CodeCompletion.UnitDocCache.CompleteDocumentation();
            controller = null;
            //GC.Collect();
            return data;
        }

        private string GetDisplayedName(SymInfo symInfo)
        {
            if (symInfo.aliasName != null)
                return symInfo.aliasName;

            if (symInfo.addit_name != null)
                return symInfo.addit_name;

            return symInfo.name;
        }
    }
}
