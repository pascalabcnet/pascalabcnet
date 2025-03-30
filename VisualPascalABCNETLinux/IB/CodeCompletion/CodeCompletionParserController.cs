// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using ICSharpCode.TextEditor.Document;
using Languages.Integration;

namespace VisualPascalABC
{
    public delegate void ParseInformationUpdatedDelegate(object obj, string fileName);

    public class CodeCompletionParserController : VisualPascalABCPlugins.ICodeCompletionService
    {
        public static Hashtable open_files = new Hashtable(StringComparer.OrdinalIgnoreCase);
        public VisualEnvironmentCompiler visualEnvironmentCompiler;
        private System.Threading.Thread th = null;
        private CodeCompletionProvider ccp;
        public event ParseInformationUpdatedDelegate ParseInformationUpdated;

        public void StopParseThread()
        {
            try
            {
                if (th != null)
                {
                    IsDone = true;
                    th.Abort(); // надо бы заменить на что то современное
                    th.Join();
                }
            }
            catch
            {
            }
        }

        public CodeCompletionParserController()
        {
            this.th = new System.Threading.Thread(new System.Threading.ThreadStart(ParseInThread));
            ccp = new CodeCompletionProvider();
        }

        public PascalABCCompiler.Parsers.ICodeCompletionDomConverter GetConverter(string fileName)
        {
            return CodeCompletion.CodeCompletionController.comp_modules[fileName] as PascalABCCompiler.Parsers.ICodeCompletionDomConverter;
        }

        public static string CurrentTwoLetterISO
        {
            get
            {
                return CodeCompletion.CodeCompletionController.currentLanguageISO;
            }
            set
            {
                CodeCompletion.CodeCompletionController.currentLanguageISO = value;
            }
        }

        public void Init()
        {
            //LanguageProvider.Instance.SourceFilesProvider = visualEnvironmentCompiler.SourceFilesProvider;
            CodeCompletion.CodeCompletionController.currentLanguageISO = PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO;
        }

        public void RenameFile(string OldFileName, string NewFileName)
        {
            if (string.Compare(OldFileName, NewFileName, true) != 0)
            {
                CodeCompletion.CodeCompletionController.comp_modules[NewFileName] = CodeCompletion.CodeCompletionController.comp_modules[OldFileName];
                if (CodeCompletion.CodeCompletionController.comp_modules.ContainsKey(OldFileName))
                    CodeCompletion.CodeCompletionController.comp_modules.Remove(OldFileName);
                open_files[NewFileName] = open_files[OldFileName];
                if (open_files.ContainsKey(OldFileName))
                    open_files.Remove(OldFileName);
            }
        }

        public void RegisterFileForParsing(string FileName)
        {
            open_files[FileName] = true;
            CodeCompletion.CodeCompletionController.SetParser(System.IO.Path.GetExtension(FileName));
            //ParseAllFiles();
        }

        public void CloseFile(string FileName)
        {
            if (CodeCompletion.CodeCompletionController.comp_modules[FileName] != null)
                CodeCompletion.CodeCompletionController.comp_modules.Remove(FileName);
            if (open_files[FileName] != null)
                open_files.Remove(FileName);
        }

        public void SetAsChanged(string FileName)
        {
            if (FileName != null)
                open_files[FileName] = true;
        }

        public void SetAllInProjectChanged()
        {
            try
            {
                Hashtable open_files2 = open_files.Clone() as Hashtable;
                foreach (string s in open_files2.Keys)
                {
                    if (ProjectFactory.Instance.CurrentProject.ContainsSourceFile(s))
                        open_files[s] = true;
                }
            }
            catch (Exception e)
            {

            }
        }

        public void RunParseThread()
        {
            th = new System.Threading.Thread(InternalParsing);
            th.Priority = System.Threading.ThreadPriority.BelowNormal;
            th.IsBackground = true;
            th.Start();
        }

        public void StopParsing()
        {
            try
            {
                VisualPABCSingleton.MainForm.StopTimer();
            }
            catch
            {

            }
        }

        private volatile bool IsDone = false;

        private void InternalParsing()
        {
            while (!IsDone)
            {
                ParseInThread();
                if (IsDone)
                    break;
                System.Threading.Thread.Sleep(2000);
            }
        }

        private long mem_delta = 0;

        internal void ParseInThread()
        {
            try
            {
                Hashtable open_files2 = (Hashtable)open_files.Clone();
                Hashtable recomp_files = new Hashtable(StringComparer.OrdinalIgnoreCase);
                bool is_comp = false;
                foreach (string FileName in open_files2.Keys)
                {
                    //(ssyy) 18.05.08 Вставил проверку на null
                    object o = open_files[FileName];
                    if (o != null && (bool)o == true)
                    {
                        is_comp = true;
                        CodeCompletion.CodeCompletionController controller = new CodeCompletion.CodeCompletionController();
                        string text = visualEnvironmentCompiler.SourceFilesProvider(FileName, PascalABCCompiler.SourceFileOperation.GetText) as string;
                        if (string.IsNullOrEmpty(text))
                            text = "begin end.";
                        CodeCompletion.DomConverter tmp = CodeCompletion.CodeCompletionController.comp_modules[FileName] as CodeCompletion.DomConverter;

                        // очистка кэша и данных от старых компиляций, чтобы при новой компиляции не появились ссылки на старые данные
                        CodeCompletion.TypeTable.Clear();

                        long cur_mem = Environment.WorkingSet;
                        CodeCompletion.DomConverter dc = controller.Compile(FileName, text);
                        mem_delta += Environment.WorkingSet - cur_mem;
                        open_files[FileName] = false;
                        if (dc.is_compiled)
                        {
                            //CodeCompletion.CodeCompletionController.comp_modules.Remove(file_name);
                            if (tmp != null && tmp.visitor.entry_scope != null)
                            {
                                tmp.visitor.entry_scope.Clear();
                                if (tmp.visitor.cur_scope != null)
                                    tmp.visitor.cur_scope.Clear();
                            }
                            CodeCompletion.CodeCompletionController.comp_modules[FileName] = dc;
                            recomp_files[FileName] = FileName;
                            open_files[FileName] = false;
                            if (ParseInformationUpdated != null)
                                ParseInformationUpdated(dc.visitor.entry_scope, FileName);
                        }
                        else if (CodeCompletion.CodeCompletionController.comp_modules[FileName] == null)
                            CodeCompletion.CodeCompletionController.comp_modules[FileName] = dc;
                    }
                }
                foreach (string FileName in open_files2.Keys)
                {
                    CodeCompletion.DomConverter dc = CodeCompletion.CodeCompletionController.comp_modules[FileName] as CodeCompletion.DomConverter;
                    CodeCompletion.SymScope ss = null;
                    if (dc != null)
                    {
                        if (dc.visitor.entry_scope != null) ss = dc.visitor.entry_scope;
                        else if (dc.visitor.impl_scope != null) ss = dc.visitor.impl_scope;
                        int j = 0;
                        while (j < 2)
                        {
                            if (j == 0)
                            {
                                ss = dc.visitor.entry_scope;
                                j++;
                            }
                            else
                            {
                                ss = dc.visitor.impl_scope;
                                j++;
                            }
                            if (ss != null)
                            {
                                for (int i = 0; i < ss.used_units.Count; i++)
                                {
                                    string s = ss.used_units[i].file_name;
                                    if (s != null && open_files2.ContainsKey(s) && recomp_files.ContainsKey(s))
                                    {
                                        is_comp = true;
                                        CodeCompletion.CodeCompletionController controller = new CodeCompletion.CodeCompletionController();
                                        string text = visualEnvironmentCompiler.SourceFilesProvider(FileName, PascalABCCompiler.SourceFileOperation.GetText) as string;
                                        CodeCompletion.DomConverter tmp = CodeCompletion.CodeCompletionController.comp_modules[FileName] as CodeCompletion.DomConverter;

                                        // очистка кэша и данных от старых компиляций, чтобы при новой компиляции не появились ссылки на старые данные
                                        CodeCompletion.TypeTable.Clear();

                                        long cur_mem = Environment.WorkingSet;
                                        dc = controller.Compile(FileName, text);
                                        mem_delta += Environment.WorkingSet - cur_mem;
                                        open_files[FileName] = false;
                                        CodeCompletion.CodeCompletionController.comp_modules[FileName] = dc;
                                        if (dc.is_compiled)
                                        {
                                            /*if (tmp != null && tmp.stv.entry_scope != null)
                                            {
                                                tmp.stv.entry_scope.Clear();
                                                if (tmp.stv.cur_scope != null) tmp.stv.cur_scope.Clear();
                                            }*/
                                            CodeCompletion.CodeCompletionController.comp_modules[FileName] = dc;
                                            recomp_files[FileName] = FileName;
                                            ss.used_units[i] = dc.visitor.entry_scope;
                                            if (ParseInformationUpdated != null)
                                                ParseInformationUpdated(dc.visitor.entry_scope, FileName);
                                        }
                                        else if (CodeCompletion.CodeCompletionController.comp_modules[FileName] == null)
                                            CodeCompletion.CodeCompletionController.comp_modules[FileName] = dc;
                                    }
                                }
                            }
                        }
                    }
                }
                if (is_comp && mem_delta > 20000000 /*&& mem_delta > 10000000*/)
                //postavil delta dlja pamjati, posle kototoj delaetsja sborka musora
                {
                    mem_delta = 0;
                    GC.Collect();
                }
            }
            catch (Exception) { }

        }

        public bool IsParsing()
        {
            return th != null && th.ThreadState == System.Threading.ThreadState.Running;
        }

        public void ParseAllFiles()
        {
            if (visualEnvironmentCompiler.UserOptions.AllowCodeCompletion)// && visualEnvironmentCompiler.compilerLoaded)
            {
                if (th.ThreadState != System.Threading.ThreadState.Running)
                {
                    th = new System.Threading.Thread(new System.Threading.ThreadStart(this.ParseInThread));
                    th.Priority = System.Threading.ThreadPriority.BelowNormal;
                    //th.IsBackground = true;
                    th.Start();
                }
                //if (th == null) 
                //	RunParseThread();
            }
        }
    }
}

