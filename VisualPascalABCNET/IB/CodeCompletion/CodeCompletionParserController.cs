// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using PascalABCCompiler.CoreUtils;

namespace VisualPascalABC
{
    public delegate void ParseInformationUpdatedDelegate(object obj, string fileName);

    public class CodeCompletionParserController : VisualPascalABCPlugins.ICodeCompletionService
    {
        public static Dictionary<string, bool> filesToParse = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
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
                    th.Abort();
                    th.Join(); // Это обязательно. По большому счету вообще Abort надо заменить на современное завершение потоков
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
                filesToParse[NewFileName] = filesToParse[OldFileName];
                if (filesToParse.ContainsKey(OldFileName))
                    filesToParse.Remove(OldFileName);
            }
        }

        public void RegisterFileForParsing(string FileName)
        {
            filesToParse[FileName] = true;
        }

        public void CloseFile(string FileName)
        {
            if (CodeCompletion.CodeCompletionController.comp_modules[FileName] != null)
                CodeCompletion.CodeCompletionController.comp_modules.Remove(FileName);
            filesToParse.Remove(FileName);
        }

        public void SetAsChanged(string FileName)
        {
            if (FileName != null && filesToParse.ContainsKey(FileName))
                filesToParse[FileName] = true;
        }

        public void SetAllInProjectChanged()
        {
            try
            {
                foreach (string s in filesToParse.Keys.ToArray())
                {
                    if (ProjectFactory.Instance.CurrentProject.ContainsSourceFile(s))
                        filesToParse[s] = true;
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Запуск потока с Intellisence
        /// </summary>
        public void SwitchOnIntellisence()
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

        private void InternalParsing()
        {
            while (true)
            {
                ParseInThread();
                System.Threading.Thread.Sleep(2000);
            }
        }

        private long mem_delta = 0;

        internal void ParseInThread()
        {
            try
            {
                HashSet<string> recomp_files = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                bool is_comp = false;
                foreach (string FileName in filesToParse.Keys.ToArray()) // копирование ключей обязательно, иначе будет InvalidOperationException EVA
                {

                    if (filesToParse[FileName])
                    {
                        // Попытка компиляции была
                        filesToParse[FileName] = false;

                        is_comp = true;
                        string text = visualEnvironmentCompiler.SourceFilesProvider(FileName, SourceFileOperation.GetText) as string;
                        if (string.IsNullOrEmpty(text))
                            text = "begin end.";

                        if (CompileWatchedFile(FileName, text, true))
                        {
                            // успешная компиляция
                            recomp_files.Add(FileName);
                        }
                    }
                }
                foreach (string FileName in filesToParse.Keys.ToArray()) // копирование ключей обязательно, иначе будет InvalidOperationException EVA
                {
                    CodeCompletion.DomConverter dc = CodeCompletion.CodeCompletionController.comp_modules[FileName] as CodeCompletion.DomConverter;
                    
                    if (dc != null)
                    {
                        CodeCompletion.SymScope watchedUnitScope = dc.visitor.entry_scope;
                        if (watchedUnitScope != null)
                        {
                            var usedUnitsTransitive = watchedUnitScope.GetRealUsedUnitsTransitive();

                            for (int i = 0; i < usedUnitsTransitive.Length; i++)
                            {
                                string usedUnitFileName = usedUnitsTransitive[i].file_name;
                                
                                // Если какая-то из зависимостей была перекомпилирована
                                if (recomp_files.Contains(usedUnitFileName))
                                {
                                    // Помечаем нужные модули для будущей перекомпиляции
                                    InvalidateDependentModules(usedUnitsTransitive, usedUnitFileName, filesToParse, FileName);

                                    // Перекомпилируем текущий модуль
                                    is_comp = true;
                                    string text = visualEnvironmentCompiler.SourceFilesProvider(FileName, SourceFileOperation.GetText) as string;

                                    // Здесь третий параметр false, потому что старые данные компиляции еще могут пригодиться до новой перекомпиляции  EVA
                                    if (CompileWatchedFile(FileName, text, false))
                                    {
                                        // успешная компиляция
                                        recomp_files.Add(FileName);
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

        /// <summary>
        /// Вызов компиляции (интеллисенсом) файла с именем fileName и содержимым fileText.
        /// Возвращает true, если компиляция была успешна, false в противном случае.
        /// </summary>
        private bool CompileWatchedFile(string fileName, string fileText, bool clearOldScope)
        {
            bool success = false;

            CodeCompletion.DomConverter tmp = CodeCompletion.CodeCompletionController.comp_modules[fileName] as CodeCompletion.DomConverter;
            long cur_mem = Environment.WorkingSet;
            CodeCompletion.CodeCompletionController controller = new CodeCompletion.CodeCompletionController();
            CodeCompletion.DomConverter dc = controller.Compile(fileName, fileText);
            mem_delta += Environment.WorkingSet - cur_mem;
            
            if (dc.is_compiled)
            {
                if (clearOldScope && tmp != null)
                {
                    tmp.visitor.entry_scope?.Clear();
                    tmp.visitor.cur_scope?.Clear();
                }
                CodeCompletion.CodeCompletionController.comp_modules[fileName] = dc;

                success = true;
                ParseInformationUpdated?.Invoke(dc.visitor.entry_scope, fileName);
            }
            else if (tmp == null)
                CodeCompletion.CodeCompletionController.comp_modules[fileName] = dc;

            return success;
        }

        /// <summary>
        /// Помечает модули из candidateModulesToCheck и watchedFiles для будущей перекомпиляции, если они завивисимы от модуля с именем recompiledDependencyName
        /// </summary>
        private void InvalidateDependentModules(CodeCompletion.SymScope[] candidateModulesToCheck, string recompiledDependencyName, Dictionary<string, bool> watchedFiles, string currentFileForRecompiling)
        {
            // Скоупы модулей, соответствующих watchedFiles
            var compiledWatchedScopes = watchedFiles.Where(watchedFile => watchedFile.Key != currentFileForRecompiling)
                                                    .Select(watchedFile => CodeCompletion.CodeCompletionController.comp_modules[watchedFile.Key])
                                                    .SelectMany(converter => converter is CodeCompletion.DomConverter dc ?
                                                                             new CodeCompletion.SymScope[] { dc.visitor.entry_scope, dc.visitor.impl_scope }
                                                                             : Enumerable.Empty<CodeCompletion.SymScope>())
                                                    .Where(sc => sc != null);

            var scopesToCheck = compiledWatchedScopes.Concat(candidateModulesToCheck).Distinct();

            foreach (var scope in scopesToCheck)
            {
                string scopeFileName = scope.file_name;

                if (scope is CodeCompletion.ImplementationUnitScope)
                    scopeFileName = ((CodeCompletion.SymScope)scope.TopScope).file_name;

                if (scopeFileName == recompiledDependencyName)
                    continue;

                var usedUnitsForUnit = scope.GetRealUsedUnitsTransitive();

                // Если в зависимостях скоупа есть recompiledDependency
                if (usedUnitsForUnit.FirstOrDefault(u => u.file_name == recompiledDependencyName) != null)
                {
                    var unitOldConverter = CodeCompletion.CodeCompletionController.comp_modules[scopeFileName];

                    if (unitOldConverter != null)
                    {
                        // Помечаем для перекомпиляции
                        if (watchedFiles.ContainsKey(scopeFileName))
                        {
                            watchedFiles[scopeFileName] = true;
                        }
                        else
                        {
                            CodeCompletion.CodeCompletionController.comp_modules.Remove(scopeFileName);
                        }
                    }
                }
            }
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

