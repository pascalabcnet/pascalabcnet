// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text; 
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

//using ICSharpCode.FormsDesigner.Services; 
//using ICSharpCode.FormsDesigner;
//using ICSharpCode.SharpDevelop.Gui;
//using ICSharpCode.Core;
//using ICSharpCode.SharpDevelop;

namespace VisualPascalABC
{
    public partial class CodeFileDocumentControl : DockContent, VisualPascalABCPlugins.ICodeFileDocument
    {
        const string xml_extension = "xml";

        Form1 MainForm=null;
        string _file_name;
        public string FileName
        {
            get
            {
                return _file_name;
            }
            set
            {
                _file_name = value;
                TextEditor.FileName = value;
            }
        }
        internal bool FromMetadata = false;
        internal bool DocumentChanged = false;
        internal bool Run = false;
        internal bool DocumentSavedToDisk = false;

        public event EventHandler PositionChanged;

        public CodeFileDocumentControl(Form1 MainForm)
        {
            this.MainForm = MainForm;
            InitializeComponent();
            TextEditor.MainForm = MainForm;
            try
            {
            	if (MainForm.UserOptions.CurrentFontFamily == null)
            		TextEditor.Font = new Font(TextEditor.Font.FontFamily, MainForm.UserOptions.EditorFontSize);
            	else
            		TextEditor.Font = new Font(new FontFamily(MainForm.UserOptions.CurrentFontFamily), MainForm.UserOptions.EditorFontSize);
            }
            catch
            {
            	
            }
            //TextEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            TextEditor.ShowEOLMarkers = false;
            TextEditor.ShowSpaces = false;
            TextEditor.ShowTabs = false;
            TextEditor.ShowInvalidLines = false;
            TextEditor.IsIconBarVisible = true;
            TextEditor.ShowLineNumbers = MainForm.UserOptions.ShowLineNums;
            TextEditor.EnableFolding = MainForm.UserOptions.EnableFolding; // SSM 4.09.08
            
//            TextEditor.EnableFolding = MainForm.UserOptions.ShowLineNums;
            TextEditor.ShowMatchingBracket = MainForm.UserOptions.ShowMatchBracket;
            TextEditor.ActiveTextAreaControl.TextArea.MouseClick += new MouseEventHandler(edit_MouseClick);
            TextEditor.Document.DocumentChanged += new ICSharpCode.TextEditor.Document.DocumentEventHandler(Document_DocumentChanged);
            TextEditor.ActiveTextAreaControl.SelectionManager.SelectionChanged += new EventHandler(SelectionManager_SelectionChanged);
            TextEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += new EventHandler(Caret_PositionChanged);
            TextEditor.ContextMenuStrip = MainForm.cmEditor;
            //TextEditor.Tag = this;
            TextEditor.TextEditorProperties.IndentStyle = ICSharpCode.TextEditor.Document.IndentStyle.Smart;
            TextEditor.TextEditorProperties.ConvertTabsToSpaces = true;
            TextEditor.TextEditorProperties.TabIndent = MainForm.UserOptions.TabIndent;
            TextEditor.TextEditorProperties.IndentationSize = MainForm.UserOptions.TabIndent;
            //TextEditor.Encoding = System.Text.Encoding.Default;
            TextEditor.Encoding = System.Text.Encoding.GetEncoding(1251);
            //this.Dock = DockStyle.Fill;
        }

        public string EXEFileName
        {
            get
            {
            	if (ProjectFactory.Instance.ProjectLoaded)
            		return Path.ChangeExtension(ProjectFactory.Instance.CurrentProject.Path,".exe");
            	return Path.ChangeExtension(FileName, ".exe");
            }
        }

        void edit_MouseClick(object sender, MouseEventArgs e)
        {
            this.TextEditor.ContextMenuStrip = MainForm.cmEditor;
        	MainForm.NavigationLocationChanged();
        }

        public float FontSize
        {
            get
            {
                return TextEditor.Font.Size;
            }
            set
            {
                TextEditor.Font = new Font(TextEditor.Font.FontFamily, value);
            }
        }

        public void LoadFromFile(string FileName)
        {
            TextEditor.LoadFile(FileName);
        }

        //меняем стратегию подсведки в соответсвии с расширением файла
        public void SetHighlightingStrategyForFile(string ForFile)
        {
            TextEditor.Document.HighlightingStrategy = ICSharpCode.TextEditor.Document.HighlightingManager.Manager.FindHighlighterForFile(ForFile);
        }


        void SelectionManager_SelectionChanged(object sender, EventArgs e)
        {
            MainForm.SynEdit_SelectionChanged(sender);
        }

        string VisualPascalABCPlugins.ICodeFileDocument.FileName
        {
            get
            {
                return this.FileName;
            }
        }

        ICSharpCode.TextEditor.TextEditorControl VisualPascalABCPlugins.ICodeFileDocument.TextEditor
        {
            get
            {
                return TextEditor;
            }
        }

        public Point CaretPosition
        {
            get
            {
                return new Point(TextEditor.CaretColumn + 1, TextEditor.CaretLine + 1);
            }
            set
            {
                //TextEditor.ActiveTextAreaControl as SharpDevelopTextAreaControl;
                TextEditor.SetFocus();
                TextEditor.CaretLine = value.Y - 1;
                TextEditor.CaretColumn = value.X - 1;
            }
        }
        public int LinesCount
        {
            get
            {
                return TextEditor.ActiveTextAreaControl.Document.LineSegmentCollection.Count;
            }
        }

        void Caret_PositionChanged(object sender, EventArgs e)
        {
            MainForm.UpdateLineColPosition();
        }

        public void SetHighlighting(string filename)
        {
            TextEditor.SetHighlighting(filename);
        }

        public void Document_DocumentChanged(object sender, ICSharpCode.TextEditor.Document.DocumentEventArgs e)
        {
            MainForm.SynEdit_ChangeText(sender, this);
            lastChanges = DateTime.Now;
        }

        public void Cut()
        {
            ICSharpCode.TextEditor.Actions.Cut cut = new ICSharpCode.TextEditor.Actions.Cut();
            cut.Execute(TextEditor.ActiveTextAreaControl.TextArea);
            //ConvertCurrentClipboardData();
        }

        private void ConvertCurrentClipboardData()
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            string[] formats = dataObject.GetFormats();
            IDataObject dataNew = new DataObject();
            foreach (string format in formats)
            {
                object data = dataObject.GetData(format);
                if (data is string)
                    dataNew.SetData(format, ConvertToWin1251((string)data));
            }
            Clipboard.Clear();
            Clipboard.SetDataObject(dataNew);
        }

        private object ConvertToWin1251(string str)
        {
            //return str.Replace("абс", "\'e0\'e1\'f1");
            return str;
        }


        public void Copy()
        {
            ICSharpCode.TextEditor.Actions.Copy copy = new ICSharpCode.TextEditor.Actions.Copy();
            copy.Execute(TextEditor.ActiveTextAreaControl.TextArea);
            //ConvertCurrentClipboardData();            
        }

        public void Paste(bool canInsertInInputBox)
        {
            try
            {
                if (MainForm.OutputWindow != null && MainForm.OutputWindow.InputTextBox != null &&
                    MainForm.OutputWindow.InputTextBox.Focused && canInsertInInputBox)
                {
                    MainForm.OutputWindow.InputTextBox.Paste();
                    return;
                }
                ICSharpCode.TextEditor.Actions.Paste paste = new ICSharpCode.TextEditor.Actions.Paste();
                paste.Execute(TextEditor.ActiveTextAreaControl.TextArea);
            }
            catch (Exception)
            {

            }
        }

        public void SetText(string text)
        {
            TextEditor.ActiveTextAreaControl.Document.Remove(0, TextEditor.ActiveTextAreaControl.Document.TextLength);
            TextEditor.ActiveTextAreaControl.Document.Insert(0, text);
            //TextEditor.ActiveTextAreaControl.Document.TextContent = text;
            TextEditor.ActiveTextAreaControl.Document.CommitUpdate();
        }

        public void SelectAll()
        {
            (new ICSharpCode.TextEditor.Actions.SelectWholeDocument()).Execute(TextEditor.ActiveTextAreaControl.TextArea);
        }

        public void DeselectAll()
        {
            (new ICSharpCode.TextEditor.Actions.ClearAllSelections()).Execute(TextEditor.ActiveTextAreaControl.TextArea);
        }

        public void Delete()
        {
            (new ICSharpCode.TextEditor.Actions.Delete()).Execute(TextEditor.ActiveTextAreaControl.TextArea);
        }

        public bool CanUndo
        {
            get { return TextEditor.Document.UndoStack.CanUndo; }
        }
        public bool CanRedo
        {
            get { return TextEditor.Document.UndoStack.CanRedo; }
        }
        public bool TextSelected
        {
            get { return TextEditor.ActiveTextAreaControl.SelectionManager.SelectedText != string.Empty; }
        }
        public string SelectedText
        {
            get { return TextEditor.ActiveTextAreaControl.SelectionManager.SelectedText; }
        }
        private DateTime lastChanges;

        public DateTime ModifyDateTime
        {
            get
            {
                if (this.DocumentChanged)
                    return lastChanges;
                if (File.Exists(this.FileName))
                    return File.GetLastWriteTime(this.FileName);
                return DateTime.Now;

            }
        }

        public static bool AcceptAllBookmarks(ICSharpCode.TextEditor.Document.Bookmark mark)
        {
            return true;
        }

        public void ToggleBookmark()
        {
            (new ICSharpCode.TextEditor.Actions.ToggleBookmark()).Execute(TextEditor.ActiveTextAreaControl.TextArea);
        }
        public void CenterView()
        {
            TextEditor.ActiveTextAreaControl.Update();
            TextEditor.ActiveTextAreaControl.CenterViewOn(TextEditor.ActiveTextAreaControl.Caret.Line, 0);
        }
        public void NextBookmark()
        {
            (new ICSharpCode.TextEditor.Actions.GotoNextBookmark(AcceptAllBookmarks)).Execute(TextEditor.ActiveTextAreaControl.TextArea);
            CenterView();
        }
        public void PrevBookmark()
        {
            (new ICSharpCode.TextEditor.Actions.GotoPrevBookmark(AcceptAllBookmarks)).Execute(TextEditor.ActiveTextAreaControl.TextArea);
            CenterView();
        }
        public void ClearAllBookmarks()
        {
            (new ICSharpCode.TextEditor.Actions.ClearAllBookmarks(AcceptAllBookmarks)).Execute(TextEditor.ActiveTextAreaControl.TextArea);
        }

        private void CodeFileDocumentControl_FormClosing(object sender, FormClosingEventArgs e)
        {
           e.Cancel = true;
           if (MainForm.OpenDocuments.Count == 1 && e.CloseReason == CloseReason.UserClosing)
               return;
           if (e.CloseReason == CloseReason.MdiFormClosing)
               return;
           MainForm.CloseFile(this);
        }

        private void CodeFileDocumentControl_Activated(object sender, EventArgs e)
        {
            //if (Designer != null)
            //    FormsDesignerViewContent.PropertyPad.SetActiveContainer(Designer.PropertyContainer);
            MainForm._currentCodeFileDocument = this;
            if (FileName != null)
                MainForm.ChangedSelectedTab();
            else
                MainForm.SetFocusToEditor();
        }

        //ssyy
        public TabControl DesignerAndCodeTabs = null;
        public TabPage TextPage = null;
        public TabPage DesignerPage = null;
        //internal FormsDesignerViewContent Designer = null;
        //string XMLCode = null;
        public string FormName = "Form1";
        public bool FirstCodeGeneration = false;

        //ssyy взводит кнопки "сохраниить" и "сохранить всё"
        public void SetDocumentChanged()
        {
            DocumentChanged = true;
            MainForm.UpdateSaveButtonsEnabled();
        }

        public void AddDesigner(string FormFileName)
        {
            FirstCodeGeneration = FormFileName == null;
            if (DesignerAndCodeTabs != null) return;
            DesignerAndCodeTabs = new TabControl();
            DesignerAndCodeTabs.Visible = false;
            Controls.Add(DesignerAndCodeTabs);
            DesignerAndCodeTabs.Dock = DockStyle.Fill;
            DesignerAndCodeTabs.TabPages.Add(PascalABCCompiler.StringResources.Get("VP_MF_M_FORM_TAB"));
            DesignerAndCodeTabs.TabPages.Add(PascalABCCompiler.StringResources.Get("VP_MF_M_PROGRAM_TAB"));
            DesignerPage = DesignerAndCodeTabs.TabPages[0];
            TextPage = DesignerAndCodeTabs.TabPages[1];
            Controls.Remove(basePanel);
            TextPage.Controls.Add(basePanel);
            //MainForm.AddToolBox();
            MainForm.AddPropertiesWindow();
            //Designer = new FormsDesignerViewContent(this);
            //Designer.LoadDesigner(FormFileName);
            DesignerAndCodeTabs.SelectedIndexChanged += tabControl_SelectedIndexChanged;
            //Designer.Modify += SetDocumentChanged;
            //Control designerSurface = Designer.DesignSurface.View as Control;
            //designerSurface.Dock = DockStyle.Fill;
            //DesignerPage.Controls.Add(designerSurface);
            //FormsDesignerViewContent.PropertyPad.SetActiveContainer(Designer.PropertyContainer);
            DesignerAndCodeTabs.Show();
            //MainForm.UpdateDesignerIsActive();
            //MainForm.UpdateUndoRedoEnabled(); //roman//
        }

        /*public void GenerateDesignerCode(EventDescription ev)
        {
            if (ev != null)
            {
                Designer.IsDirty = true;
            }
            if (Designer.IsDirty) //roman//
            {
                Designer.ResetGeneratedCode();
                /*Designer.loader.file_name = Path.GetFileNameWithoutExtension(_file_name);
                Designer.loader.form_name = FormName;
                Designer.loader.Flush();
                Designer.DesignSurface.Flush();
                string code = Designer.GeneratedCode;
                if (code != null)
                {
                    string gen_code = code;
                    string new_code = BuildCode(TextEditor.Text, gen_code, ev);
                    if (new_code != null)
                    {
                        TextEditor.Text = new_code;
                    }
                    //XMLCode = Designer.generatedCode.XMLCode;
                }
            }
        }*/

        /*public void GenerateMainProgram(string MainUnitName, string MainFormName)
        {
            TextEditor.Text = String.Format(string_consts.main_designer_program, MainUnitName, MainFormName);
        }

        public void SaveFormFile(string PasFileName)
        {
            if (Designer == null || Designer.CodeCompileUnit == null) return;
            try
            {
                string FormFileName = Path.ChangeExtension(PasFileName, string_consts.xml_form_extention);
                FileStream fs = new FileStream(FormFileName, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, Designer.CodeCompileUnit);
                fs.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error saving form file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/

        public void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // If the tab is changing, that means we'd better let the loader know it needs
            // to flush changes that have been made and update the code windows.
            //
            //MainForm.UpdateDesignerIsActive();
            //GenerateDesignerCode(null);
            //roman//
            MainForm.UpdateUndoRedoEnabled();
        }

        public static string BuildName(List<PascalABCCompiler.SyntaxTree.ident> names)
        {
            if (names == null || names.Count == 0)
                return null;
            string rez = names[0].name;
            for (int i = 1; i < names.Count; ++i)
            {
                rez += "." + names[i].name;
            }
            return rez;
        }

        /*public string BuildCode(string existing_text, string generated_text, EventDescription event_description)
        {
            if (FirstCodeGeneration)
            {
                FirstCodeGeneration = false;
                string form_name = (Designer.Host.RootComponent as Control).Name;
                if (form_name == "Form1")
                {

                }
                string beg = string.Format(string_consts.begin_unit,
                    Path.GetFileNameWithoutExtension(_file_name), (Designer.Host.RootComponent as Control).Name);
                existing_text = beg + generated_text + string_consts.end_unit;
                TextEditor.Text = existing_text;
                if (event_description == null)
                {
                    return existing_text; //beg + generated_text + SampleDesignerHost.string_consts.end_unit;
                }
            }
            //StringBuilder sb = new StringBuilder(existing_text); //TextEditor.Text);
            string[] sep = new string[1]{string_consts.nr};
            string[] lines = existing_text.Split(sep, StringSplitOptions.None);
            int count = lines.Length;
            int s_num = 0;
            string trimed;
            //int end_region_num;
            while (s_num < count)
            {
                trimed = lines[s_num].TrimStart(' ','\t');
                if (trimed.StartsWith(string_consts.begin_designer_region, StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }
                s_num++;
            }
            if (s_num == count)
            {
                MessageDesignerCodeGenerationFailed();
                return null;
            }
            int e_num = s_num + 1;
            while (e_num < count)
            {
                trimed = lines[e_num].TrimStart(' ', '\t');
                if (trimed.StartsWith(string_consts.end_designer_region, StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }
                e_num++;
            }
            if (e_num == count)
            {
                MessageDesignerCodeGenerationFailed();
                return null;
            }
            List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
            List<PascalABCCompiler.Errors.CompilerWarning> Warnings = new List<PascalABCCompiler.Errors.CompilerWarning>();
            //PascalABCCompiler.SyntaxTree.syntax_tree_node sn =
            //    MainForm.VisualEnvironmentCompiler.Compiler.ParsersController.Compile(
            //    file_name, TextEditor.Text, null, Errors, PascalABCCompiler.Parsers.ParseMode.Normal);
            PascalABCCompiler.SyntaxTree.compilation_unit sn =
                CodeCompletion.CodeCompletionController.ParsersController.GetCompilationUnit(
                VisualPABCSingleton.MainForm._currentCodeFileDocument.file_name,
                existing_text, //VisualPascalABC.Form1.Form1_object._currentCodeFileDocument.TextEditor.Text,
                Errors,
                Warnings);
            PascalABCCompiler.SyntaxTree.unit_module um = sn as PascalABCCompiler.SyntaxTree.unit_module;
            bool good_syntax = um != null;
            PascalABCCompiler.SyntaxTree.type_declaration form_decl = null;
            if (good_syntax)
            {
                good_syntax = um.implementation_part != null &&
                    um.interface_part != null &&
                    um.interface_part.interface_definitions != null &&
                    um.interface_part.interface_definitions.defs != null &&
                    um.interface_part.interface_definitions.defs.Count > 0;
            }
            if (good_syntax)
            {
                foreach (PascalABCCompiler.SyntaxTree.declaration decl in um.interface_part.interface_definitions.defs)
                {
                    PascalABCCompiler.SyntaxTree.type_declarations tdecls = decl as PascalABCCompiler.SyntaxTree.type_declarations;
                    if (tdecls != null)
                    {
                        foreach (PascalABCCompiler.SyntaxTree.type_declaration tdecl in tdecls.types_decl)
                        {
                            if (tdecl.source_context.begin_position.line_num - 1 < s_num &&
                                tdecl.source_context.end_position.line_num - 1 > e_num)
                            {
                                form_decl = tdecl;
                            }
                        }
                    }
                }
            }
            PascalABCCompiler.SyntaxTree.class_definition form_def = null;
            if (form_decl != null)
            {
                form_def = form_decl.type_def as PascalABCCompiler.SyntaxTree.class_definition;
            }
            if (form_decl == null || form_def == null || form_def.body == null)
            {
                MessageBox.Show(PascalABCCompiler.StringResources.Get("VP_MF_CODE_GENERATION_UNSUCCEFULL"),
                PascalABCCompiler.StringResources.Get("VP_MF_FORM_DESIGNER"),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            else
            {
                string old_form_name = form_decl.type_name.name;
                string new_form_name = (Designer.Host.RootComponent as Control).Name;
                bool implementation_not_null =
                    um.implementation_part.implementation_definitions != null &&
                    um.implementation_part.implementation_definitions.defs != null &&
                     um.implementation_part.implementation_definitions.defs.Count > 0;
                if (new_form_name != old_form_name)
                {
                    ReplaceName(form_decl.type_name, new_form_name, lines);
                    if (implementation_not_null)
                    {
                        foreach (PascalABCCompiler.SyntaxTree.declaration decl in
                            um.implementation_part.implementation_definitions.defs)
                        {
                            PascalABCCompiler.SyntaxTree.procedure_definition pd = decl as PascalABCCompiler.SyntaxTree.procedure_definition;
                            if (pd != null)
                            {
                                if (pd.proc_header.name.class_name != null &&
                                    string.Compare(pd.proc_header.name.class_name.name, old_form_name, true) == 0)
                                {
                                    ReplaceName(pd.proc_header.name.class_name, new_form_name, lines);
                                }
                            }
                        }
                    }
                }
                if (event_description != null)
                {
                    MethodInfo mi = event_description.e.EventType.GetMethod(
                        StringConstants.invoke_method_name);
                    ParameterInfo[] pinfos = mi.GetParameters();
                    bool handler_found = false;

                    event_description.editor = TextEditor;
                    System.Text.RegularExpressions.MatchCollection matches =
                        System.Text.RegularExpressions.Regex.Matches(generated_text, string_consts.nr);
                    //строка, на которой последнее описание из секции реализаций
                    PascalABCCompiler.SyntaxTree.file_position last_defs_pos = null;
                    if (implementation_not_null)
                    {
                        last_defs_pos = um.implementation_part.implementation_definitions.defs[
                            um.implementation_part.implementation_definitions.defs.Count - 1
                            ].source_context.end_position;
                        
                        //Ищем описание обработчика
                        foreach (PascalABCCompiler.SyntaxTree.declaration decl in
                            um.implementation_part.implementation_definitions.defs)
                        {
                            PascalABCCompiler.SyntaxTree.procedure_definition pd = decl as PascalABCCompiler.SyntaxTree.procedure_definition;
                            if (pd == null)
                            {
                                continue;
                            }
                            if (pd.proc_header.name == null || pd.proc_header.name.class_name == null ||              //roman//
                                String.Compare(pd.proc_header.name.class_name.name, new_form_name, true) != 0 ||
                                String.Compare(pd.proc_header.name.meth_name.name, event_description.EventName, true) != 0)
                            {
                                continue;
                            }
                            List<PascalABCCompiler.SyntaxTree.typed_parameters> syn_pars =
                                pd.proc_header.parameters.params_list;
                            bool should_continue = false;
                            int par_count = syn_pars.Count;
                            if (par_count != pinfos.Length)
                            {
                                continue;
                            }
                            for (int i = 0; i < par_count; ++i)
                            {
                                if (syn_pars[i].idents.idents.Count != 1)
                                {
                                    should_continue = true;
                                    break;
                                }
                                PascalABCCompiler.SyntaxTree.named_type_reference ntr =
                                    syn_pars[i].vars_type as PascalABCCompiler.SyntaxTree.named_type_reference;
                                if (ntr == null || syn_pars[i].param_kind != PascalABCCompiler.SyntaxTree.parametr_kind.none)
                                {
                                    should_continue = true;
                                    break;
                                }
                                string syn_name = BuildName(ntr.names);
                                if (String.Compare(syn_name, pinfos[i].ParameterType.Name) != 0 &&
                                    String.Compare(syn_name, pinfos[i].ParameterType.FullName) != 0)
                                {
                                    should_continue = true;
                                    break;
                                }
                            }
                            if (should_continue)
                            {
                                continue;
                            }
                            handler_found = true;
                            event_description.line_num = pd.proc_body.source_context.begin_position.line_num +
                                matches.Count + s_num - e_num + 2;
                            //last_defs_pos.line_num + s_num - e_num + matches.Count + 7;
                            event_description.column_num = pd.proc_body.source_context.begin_position.column_num;
                        }
                    }
                    else
                    {
                        last_defs_pos = um.implementation_part.source_context.end_position;
                    }
                    if (!handler_found)
                    {
                        string new_event = event_description.EventName;
                        if (pinfos.Length != 0)
                        {
                            new_event += "(";
                            new_event += pinfos[0].Name + ": " + pinfos[0].ParameterType.FullName.Replace("System.Windows.Forms.","").Replace("System.","");
                            for (int i = 1; i < pinfos.Length; ++i)
                            {
                                new_event += "; ";
                                new_event += pinfos[i].Name + ": " + pinfos[i].ParameterType.FullName.Replace("System.Windows.Forms.", "").Replace("System.", "");
                            }
                            new_event += ")";
                        }
                        new_event += ";";
                        string new_event_header = new_event;
                        new_event = string_consts.nr + string_consts.nr + "procedure " + new_form_name + "." + new_event;
                        new_event += string_consts.nr + "begin" +
                            string_consts.nr + "  " + string_consts.nr +
                            "end;";
                        event_description.column_num = 3;
                        event_description.line_num = last_defs_pos.line_num + s_num - e_num + matches.Count + 7;
                        lines[last_defs_pos.line_num - 1] = lines[last_defs_pos.line_num - 1].Insert(last_defs_pos.column_num, new_event);
                        //Добавляем заголовок события
                        //int last_form_member_line = form_def.body.class_def_blocks[form_def.body.class_def_blocks.Count - 1].source_context.end_position.line_num - 1;
                        lines[s_num] = string_consts.event_handler_header_trim +
                            "procedure " + new_event_header +
                            string_consts.nr + lines[s_num];
                    }
                }
            }
            //generated_text = SampleDesignerHost.string_consts.tab + "private" +
            //    SampleDesignerHost.string_consts.nr + generated_text;
            string s1 = string.Join(string_consts.nr, lines, 0, s_num + 1);
            string s2 = string.Join(string_consts.nr, lines, e_num, lines.Length - e_num);
            return s1 + string_consts.nr + string_consts.tab +
                "internal" + string_consts.nr + 
                string_consts.tab2 + generated_text + s2;
        }*/

        public void ReplaceName(PascalABCCompiler.SyntaxTree.ident id, string new_name, string[] lines)
        {
            PascalABCCompiler.SyntaxTree.SourceContext sc = id.source_context;
            int type_name_num = sc.begin_position.line_num - 1;
            int bcol = sc.begin_position.column_num;
            int ecol = sc.end_position.column_num;
            string s = lines[type_name_num].Remove(bcol - 1, ecol - bcol + 1);
            lines[type_name_num] = s.Insert(bcol - 1, new_name);
        }

        public void MessageDesignerCodeGenerationFailed()
        {
            MessageBox.Show(PascalABCCompiler.StringResources.Get("VP_MF_CAN_NOT_GENERATE_CODE"),
                PascalABCCompiler.StringResources.Get("VP_MF_FORM_DESIGNER"),
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        #region ICodeFileDocument Member


        string VisualPascalABCPlugins.ICodeFileDocument.EXEFileName
        {
            get {
                return this.EXEFileName;
            }
        }

        int VisualPascalABCPlugins.ICodeFileDocument.LinesCount
        {
            get {
                return this.LinesCount;
            }
        }

        string VisualPascalABCPlugins.ICodeFileDocument.Text
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        bool VisualPascalABCPlugins.ICodeFileDocument.FromMetadata
        {
            get {
                return this.FromMetadata;
            }
        }

        bool VisualPascalABCPlugins.ICodeFileDocument.DocumentChanged
        {
            get {
                return this.DocumentChanged;
            }
        }

        string VisualPascalABCPlugins.ICodeFileDocument.ToolTipText
        {
            get
            {
                return this.ToolTipText;
            }
            set
            {
                this.ToolTipText = value;
            }
        }

        bool VisualPascalABCPlugins.ICodeFileDocument.Run
        {
            get {
                return this.Run;
            }
            set
            {
                this.Run = value;
            }
        }



        #endregion

        private void CodeFileDocumentControl_Paint(object sender, PaintEventArgs e)
        {
            MainForm._currentCodeFileDocument = this;
            if (FileName != null)
                MainForm.ChangedSelectedTab();
            else
                MainForm.SetFocusToEditor();
        }
    }

    public class HideBottomTabs : ICSharpCode.TextEditor.Actions.AbstractEditAction
    {
        public HideBottomTabs()
        {
        }
        public override void Execute(ICSharpCode.TextEditor.TextArea textArea)
        {
            var MainForm = (textArea.MotherTextEditorControl as CodeFileDocumentTextEditorControl).MainForm;
            MainForm.HideBottomPanel();
            // (textArea.MotherTextEditorControl as CodeFileDocumentTextEditorControl).MainForm.BottomTabsVisible = false;
        }
    }
    
}
