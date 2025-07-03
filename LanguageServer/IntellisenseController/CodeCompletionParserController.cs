// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CodeCompletion
{
    public delegate void ParseInformationUpdatedDelegate(object obj, string fileName);

    public class CodeCompletionParserController : VisualPascalABCPlugins.ICodeCompletionService, IDisposable
    {
        private Dictionary<string, bool> openFiles = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        public event ParseInformationUpdatedDelegate ParseInformationUpdated;

        private Thread backgroundParsingThread;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEventSlim parseLockEvent = new ManualResetEventSlim(false);

        private readonly PascalABCCompiler.ISourceTextProvider sourceFileProvider;

        public CodeCompletionParserController(PascalABCCompiler.ISourceTextProvider sourceFileProvider) 
        {
            this.sourceFileProvider = sourceFileProvider;
        }

        public void SwitchOnIntellisense()
        {
            backgroundParsingThread = new Thread(BackgroundParsingLoop)
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };

            backgroundParsingThread.Start();
        }

        public void SwitchOffIntellisense()
        {
            cancellationTokenSource.Cancel(); // Отменяем операцию парсинга
            parseLockEvent.Set(); // Разблокируем поток, если он ждёт
            backgroundParsingThread.Join();
        }

        private void BackgroundParsingLoop()
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    ParseInThread();

                    // ожидание 2 секунды
                    parseLockEvent.Wait(TimeSpan.FromSeconds(2), cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    // Выход при отмене
                    break;
                }
                catch (Exception)
                {
                    // ожидание перед повторной попыткой
                    Thread.Sleep(500);
                }
            }
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
                openFiles[NewFileName] = openFiles[OldFileName];
                if (openFiles.ContainsKey(OldFileName))
                    openFiles.Remove(OldFileName);
            }
        }

        public void RegisterFileForParsing(string FileName)
        {
            openFiles[FileName] = true;
            CodeCompletion.CodeCompletionController.SetLanguage(FileName);
            //ParseAllFiles();
        }

        public void CloseFile(string FileName)
        {
            if (CodeCompletion.CodeCompletionController.comp_modules[FileName] != null)
                CodeCompletion.CodeCompletionController.comp_modules.Remove(FileName);
            openFiles.Remove(FileName);
        }

        public void SetAsChanged(string FileName)
        {
            if (FileName != null)
                openFiles[FileName] = true;
        }

        private void ParseInThread()
        {
            long mem_delta = 0;

            Dictionary<string, string> recomp_files = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            bool is_comp = false;
            foreach (string FileName in openFiles.Keys.ToArray()) // копирование ключей обязательно, иначе будет InvalidOperationException EVA
            {
                if (cancellationTokenSource.IsCancellationRequested)
                    return;

                if (openFiles[FileName])
                {
                    is_comp = true;
                    CodeCompletion.CodeCompletionController controller = new CodeCompletion.CodeCompletionController();
                    string text = sourceFileProvider.GetText(FileName);
                    if (string.IsNullOrEmpty(text))
                        text = "begin end.";
                    CodeCompletion.DomConverter tmp = CodeCompletion.CodeCompletionController.comp_modules[FileName] as CodeCompletion.DomConverter;
                    long cur_mem = Environment.WorkingSet;
                    CodeCompletion.DomConverter dc = controller.Compile(FileName, text);
                    mem_delta += Environment.WorkingSet - cur_mem;
                    openFiles[FileName] = false;
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
                        openFiles[FileName] = false;
                        //if (ParseInformationUpdated != null)
                        //    ParseInformationUpdated(dc.visitor.entry_scope, FileName);
                    }
                    else if (CodeCompletion.CodeCompletionController.comp_modules[FileName] == null)
                        CodeCompletion.CodeCompletionController.comp_modules[FileName] = dc;
                }
            }
            foreach (string FileName in openFiles.Keys.ToArray()) // копирование ключей обязательно, иначе будет InvalidOperationException EVA
            {
                if (cancellationTokenSource.IsCancellationRequested)
                    return;

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
                                if (s != null && openFiles.ContainsKey(s) && recomp_files.ContainsKey(s))
                                {
                                    is_comp = true;
                                    CodeCompletion.CodeCompletionController controller = new CodeCompletion.CodeCompletionController();
                                    string text = sourceFileProvider.GetText(FileName);
                                    CodeCompletion.DomConverter tmp = CodeCompletion.CodeCompletionController.comp_modules[FileName] as CodeCompletion.DomConverter;
                                    long cur_mem = Environment.WorkingSet;
                                    dc = controller.Compile(FileName, text);
                                    mem_delta += Environment.WorkingSet - cur_mem;
                                    openFiles[FileName] = false;
                                    CodeCompletion.CodeCompletionController.comp_modules[FileName] = dc;
                                    if (dc.is_compiled)
                                    {
                                        //if (tmp != null && tmp.stv.entry_scope != null)
                                        //{
                                        //    tmp.stv.entry_scope.Clear();
                                        //    if (tmp.stv.cur_scope != null) tmp.stv.cur_scope.Clear();
                                        //}
                                        CodeCompletion.CodeCompletionController.comp_modules[FileName] = dc;
                                        recomp_files[FileName] = FileName;
                                        ss.used_units[i] = dc.visitor.entry_scope;
                                        //if (ParseInformationUpdated != null)
                                        //    ParseInformationUpdated(dc.visitor.entry_scope, FileName);
                                    }
                                    else if (CodeCompletion.CodeCompletionController.comp_modules[FileName] == null)
                                        CodeCompletion.CodeCompletionController.comp_modules[FileName] = dc;
                                }
                            }
                        }
                    }
                }
            }
            if (is_comp && mem_delta > 20000000)
            //postavil delta dlja pamjati, posle kototoj delaetsja sborka musora
            {
                GC.Collect();
            }
        }

        public void Dispose()
        {
            SwitchOffIntellisense();
            parseLockEvent.Dispose();
            cancellationTokenSource.Dispose();
        }
    }
}

