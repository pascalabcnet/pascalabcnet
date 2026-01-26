// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using VisualPascalABCPlugins;
using System.Threading;
using Languages.Integration;
using Languages.Facade;

namespace VisualPascalABC
{
    public class VECStringResources
    {
        public static string Get(string key)
        {
            return PascalABCCompiler.StringResources.Get("VP_VEC_"+key);
        }

    }
    
    public class VisualEnvironmentCompiler: IVisualEnvironmentCompiler
    {
        PascalABCCompiler.CompilerType defaultCompilerType = PascalABCCompiler.CompilerType.Remote;

        public static Encoding DefaultFileEncoding = Encoding.UTF8;

        private IWorkbench workbench;
        private PascalABCCompiler.RemoteCompiler remoteCompiler;
        private PascalABCCompiler.Compiler standartCompiler;
        private System.Threading.Thread MainProgramThread = System.Threading.Thread.CurrentThread;
        private System.Threading.Thread StartingThread;
        public delegate void SetFlagDelegate(bool flag);
        private SetFlagDelegate SetCompilingAndRunButtonsEnabled;
        private SetFlagDelegate SetDebugButtonsEnabled;
        public ConvertTextDelegate ConvertUnitTextProperty { get; set; }

        public delegate void SetTextDelegate(string text);
        private SetTextDelegate SetStateText;
        private SetTextDelegate AddTextToCompilerMessages;
        private bool Compilation = false;
        public VisualPascalABCPlugins.PluginsController PluginsController;
        private ToolStripMenuItem PluginsMenuItem;
        private ToolStrip PluginsToolStrip;
        public bool compilerLoaded=false;
        InvokeDegegate beginInvoke;
        public InvokeDegegate BeginInvoke
        {
            get
            {
                return beginInvoke;                
            }
        }
        public delegate void ExecuteSourceLocationActionDelegate(PascalABCCompiler.SourceLocation SourceLocation, SourceLocationAction Action);
        private ExecuteSourceLocationActionDelegate ExecuteSLAction;
        public delegate object ExecuteVisualEnvironmentCompilerActionDelegate(VisualEnvironmentCompilerAction Action, object obj);
        private ExecuteVisualEnvironmentCompilerActionDelegate ExecuteVECAction;
        private bool StartingCompleted = false;
        public PascalABCCompiler.Errors.ErrorsStrategyManager ErrorsManager;
        private RunManager RunnerManager;
        private DebugHelper DebugHelper;
		private CodeCompletionParserController CodeCompletionParserController;
		public UserOptions UserOptions;
        private Dictionary<string, string> StandartDirectories;
        Dictionary<string, CodeFileDocumentControl> OpenDocuments;

        public VisualEnvironmentCompiler(InvokeDegegate beginInvoke, 
            SetFlagDelegate setCompilingAndRunButtonsEnabled, SetFlagDelegate setCompilingDebugEnabled, SetTextDelegate setStateText, 
            SetTextDelegate addTextToCompilerMessages, ToolStripMenuItem pluginsMenuItem, 
            ToolStrip pluginsToolStrip, ExecuteSourceLocationActionDelegate ExecuteSLAction, 
            ExecuteVisualEnvironmentCompilerActionDelegate ExecuteVECAction,
            PascalABCCompiler.Errors.ErrorsStrategyManager ErrorsManager, RunManager RunnerManager, DebugHelper DebugHelper,UserOptions UserOptions, Dictionary<string, string> StandartDirectories,
            Dictionary<string, CodeFileDocumentControl> OpenDocuments, IWorkbench workbench)
        {
            this.StandartDirectories = StandartDirectories;
            this.ErrorsManager = ErrorsManager;
            this.ChangeVisualEnvironmentState += onChangeVisualEnvironmentState;
            SetCompilingAndRunButtonsEnabled = setCompilingAndRunButtonsEnabled;
            SetDebugButtonsEnabled = setCompilingDebugEnabled;
            SetStateText = setStateText;
            AddTextToCompilerMessages = addTextToCompilerMessages;
            this.beginInvoke = beginInvoke;
            this.ExecuteSLAction=ExecuteSLAction;
            this.ExecuteVECAction = ExecuteVECAction;
            PluginsMenuItem = pluginsMenuItem;
            PluginsToolStrip = pluginsToolStrip;
            PluginsController = new PluginsController(this, PluginsMenuItem, PluginsToolStrip, workbench);
            this.RunnerManager = RunnerManager;
            this.DebugHelper = DebugHelper;
            DebugHelper.Starting += DebugHelper_Starting;
            DebugHelper.Exited += DebugHelper_Exited;
            RunnerManager.Starting += RunnerManager_Starting;
            RunnerManager.Exited += RunnerManager_Exited;
            this.CodeCompletionParserController = WorkbenchServiceFactory.CodeCompletionParserController;
            this.CodeCompletionParserController.visualEnvironmentCompiler = this;
            this.UserOptions = UserOptions;
            this.OpenDocuments = OpenDocuments;
            
            LanguageIntegrator.LanguageLoaded += OnLanguageLoaded;
            LanguageIntegrator.LanguageLoadErrorOccured += OnLanguageLoadErrorOccured;
        }

        void RunnerManager_Exited(string fileName)
        {
            ChangeVisualEnvironmentState(VisualEnvironmentState.ProcessExited, fileName);
        }

        void RunnerManager_Starting(string fileName)
        {
            ChangeVisualEnvironmentState(VisualEnvironmentState.ProcessStarting, fileName);
        }

        void DebugHelper_Exited(string FileName)
        {
            ChangeVisualEnvironmentState(VisualEnvironmentState.ProcessExited, FileName);
        }

        void DebugHelper_Starting(string FileName)
        {
            ChangeVisualEnvironmentState(VisualEnvironmentState.ProcessStartingDebug, FileName);
        }


        public bool Starting()
        {
            return StartingThread.ThreadState == System.Threading.ThreadState.Running;
        }
        public bool Started
        {
            get { return StartingThread.ThreadState != System.Threading.ThreadState.Running; }
        }
        public void AbortStaring()
        {
            //для того чтобы отмена прошла быстрее увеличиваем приоритет потока
            StartingThread.Priority = System.Threading.ThreadPriority.AboveNormal;
            //даем команду на завершение и ждем завершения
            StartingThread.Join();
        }
        
        private void CreateCompiler()
        {
            standartCompiler = new PascalABCCompiler.Compiler(SourceFilesProvider, OnChangeCompilerState);
            CodeCompletion.CodeCompletionController.comp = new PascalABCCompiler.Compiler((PascalABCCompiler.Compiler)standartCompiler, SourceFilesProvider, OnChangeCompilerState);
            if (ProjectFactory.Instance.CurrentProject != null)
            {
            	CodeCompletion.CodeCompletionController.comp.CompilerOptions.CurrentProject = ProjectFactory.Instance.CurrentProject;
            	ProjectFactory.Instance.Dirty = true;
            }
            CodeCompletion.CodeCompletionController.StandartDirectories = StandartDirectories;

            this.CodeCompletionParserController.Init();
            if (defaultCompilerType == PascalABCCompiler.CompilerType.Remote && !Tools.IsUnix())
                LoadRemoteCompiler();
            else
                defaultCompilerType = PascalABCCompiler.CompilerType.Standart;
        }
        
        public void LoadRemoteCompiler()
        {
            remoteCompiler = new PascalABCCompiler.RemoteCompiler(PascalABCCompiler.ConsoleCompilerConstants.MaxProcessMemoryMB, OnChangeCompilerState, SourceFilesProvider);
        }

        public void RunStartingThread()
        {
            StartingThread = new System.Threading.Thread(new System.Threading.ThreadStart(CreateCompiler));
            StartingThread.Priority = System.Threading.ThreadPriority.BelowNormal;
            ChangeVisualEnvironmentState(VisualEnvironmentState.StartCompilerLoading, StartingThread);
            StartingThread.Start();
        }

        public string GetFilterForDialogs()
        {
            if (standartCompiler == null) 
                return null;
            string Filter = "", AllFilter = "";
            foreach (ILanguage lang in LanguageProvider.Instance.Languages)
            {
                Filter = Tools.MakeFilter(Filter, lang.Name, lang.FilesExtensions);
                AllFilter = Tools.MakeAllFilter(AllFilter, lang.Name, lang.FilesExtensions);
            }
            Filter += "Программы на C# (*.cs)|*.cs|";
            AllFilter += "*.cs;";
            return Tools.FinishMakeFilter(Filter, AllFilter);
        }
        
        public string GetProjectFilterForDialogs()
        {
        	if (standartCompiler == null)
                return null;
            string Filter = "", AllFilter = "";
            foreach (ILanguage lang in LanguageProvider.Instance.Languages)
            {
                Filter = Tools.MakeProjectFilter(Filter, lang.Name, 
                                                 new string[1] { PascalABCCompiler.StringConstants.platformProjectExtension });   
            }
           
            AllFilter = Tools.MakeAllFilter(AllFilter, PascalABCCompiler.StringConstants.pascalLanguageName, 
                                            new string[1] { PascalABCCompiler.StringConstants.platformProjectExtension });
            return Tools.FinishMakeFilter(Filter, AllFilter);
        }
        
        public string GetAssemblyFilterForDialogs()
        {
        	if (standartCompiler == null) 
                return null;
        	string Filter = "";
        	string AllFilter = "";
        	Filter = Tools.MakeFilter(Filter, VECStringResources.Get("ASSEMBLIES"), new string[1]{".dll"});
        	AllFilter = Tools.MakeAllFilter(AllFilter, VECStringResources.Get("ASSEMBLIES"), new string[1]{".dll"});
        	return Tools.FinishMakeFilter(Filter, AllFilter);
        }
        
        private bool CurrentThreadIsMainThread()
        {
            return MainProgramThread == System.Threading.Thread.CurrentThread;
        }

        DateTime compilationStartTime;
        public string Compile(PascalABCCompiler.CompilerOptions CompilerOptions)
        {
            compilationStartTime = DateTime.Now;
            Compiler.CompilerOptions = CompilerOptions;
            string fn=Compiler.Compile();
            return fn;
        }
        
        public void StartCompile(PascalABCCompiler.CompilerOptions CompilerOptions)
        {
            compilationStartTime = DateTime.Now;
            Compiler.CompilerOptions = CompilerOptions;
           	Compiler.StartCompile();
        }

        void onChangeVisualEnvironmentState(VisualEnvironmentState State, object obj)
        {

        }

        
        public object SourceFilesProvider(string FileName, PascalABCCompiler.SourceFileOperation FileOperation)
        {


            CodeFileDocumentControl tp = null;
            ICSharpCode.TextEditor.TextEditorControl ed = null;
            if (OpenDocuments.TryGetValue(Tools.FileNameToLower(FileName), out tp))
                ed = tp.TextEditor;
            
            
            switch (FileOperation)
            {
                case PascalABCCompiler.SourceFileOperation.GetText:
                    if (tp != null)
                    {
                        string Text1 = ed.Document.TextContent;
                        if (ConvertUnitTextProperty != null)
                            Text1 = ConvertUnitTextProperty(FileName, Text1);
                        return Text1;
                    }
                        
                    if (!File.Exists(FileName))
                        return null;
                    /*TextReader tr = new StreamReader(file_name, System.Text.Encoding.GetEncoding(1251));
                    string Text = tr.ReadToEnd();
                    tr.Close();*/
                    string Text = PascalABCCompiler.FileReader.ReadFileContent(FileName, null);
                    if (ConvertUnitTextProperty != null)
                        Text = ConvertUnitTextProperty(FileName, Text);
                    return Text;
                case PascalABCCompiler.SourceFileOperation.Exists:
                    if (tp != null)
                        return true;
                    return File.Exists(FileName);
                case PascalABCCompiler.SourceFileOperation.GetLastWriteTime:
                    if (tp != null)
                        return tp.ModifyDateTime;
                    return File.GetLastWriteTime(FileName);
                case PascalABCCompiler.SourceFileOperation.FileEncoding:
                    return DefaultFileEncoding;
            }
            return null;
        }

        private delegate void ChangeCompilerStateDelegate(PascalABCCompiler.ICompiler sender, PascalABCCompiler.CompilerState State, string FileName);
        private void OnChangeCompilerState(PascalABCCompiler.ICompiler sender, PascalABCCompiler.CompilerState State, string FileName)
        {
            if (CurrentThreadIsMainThread())
                OnChangeCompilerStateEx(sender, State, FileName);
            else
                BeginInvoke(new ChangeCompilerStateDelegate(OnChangeCompilerStateEx), sender, State, FileName);
        }

        private void CompileTestProgram(object state)
        {
            string fileName = @"C:\PABCWork.NET\Samples\test.pas";
            if (File.Exists(fileName))
            {
                PascalABCCompiler.CompilerOptions co = new PascalABCCompiler.CompilerOptions();
                co.SourceFileName = fileName;
                Compile(co);
            }
        }

        private void LoadPlugins(object state)
        {
            AddTextToCompilerMessages(VECStringResources.Get("LOAD_PLUGINS") + Environment.NewLine);
            PluginsController.AddPlugins();
            if (PluginsController.Plugins.Count == 0)
                AddTextToCompilerMessages(VECStringResources.Get("PLUGINS_NOTFOUND") + Environment.NewLine);
            else
                foreach (VisualPascalABCPlugins.IVisualPascalABCPlugin Plugin in PluginsController.Plugins)
                    AddTextToCompilerMessages(Plugin.Name+" v"+Plugin.Version + Environment.NewLine);
            if (PluginsController.ErrorList.Count > 0)
            {
                AddTextToCompilerMessages(VECStringResources.Get("PLUGINS_LOADING_ERRORS") + Environment.NewLine);
                foreach (VisualPascalABCPlugins.PluginLoadingError Error in PluginsController.ErrorList)
                    AddTextToCompilerMessages(Error.ToString() + Environment.NewLine);
            }
            
           	//VisualPABCSingleton.MainForm.codeCompletionParserController.ParseAllFiles();
        }

        void AddCompilerTextToCompilerMessages(PascalABCCompiler.ICompiler sender, string text)
        {
            if (sender.CompilerType == PascalABCCompiler.CompilerType.Standart)
                AddTextToCompilerMessages(VECStringResources.Get("LOCAL_COMPILER_PREFIX") + text);
            else
                AddTextToCompilerMessages(VECStringResources.Get("REMOTE_COMPILER_PREFIX") + text);
        }
        List<string> ParsedFiles = new List<string>();
        
        /// <summary>
        /// Выводит сообщения о подключенных языках
        /// </summary>
        private void OnLanguageLoaded(Languages.Facade.ILanguage language)
        {
            string languageConnectedMessage = string.Format(VECStringResources.Get("LANGUAGE_LOADED{0}"), language.Name);
            languageConnectedMessage += Environment.NewLine;
            AddTextToCompilerMessages(languageConnectedMessage);
        }

        /// <summary>
        /// Выводит сообщения об ошибках, возникших при загрузке языков
        /// </summary>
        private void OnLanguageLoadErrorOccured(string languageFileName)
        {
            string languageLoadErrorMessage = string.Format(VECStringResources.Get("LANGUAGE_LOAD_ERROR{0}"), Path.GetFileName(languageFileName));
            languageLoadErrorMessage += Environment.NewLine;
            AddTextToCompilerMessages(languageLoadErrorMessage);
        }

        private void OnChangeCompilerStateEx(PascalABCCompiler.ICompiler sender, PascalABCCompiler.CompilerState State, string FullFileName)
        {

            string RusName = null;
            string FileName = null;
            if (FullFileName != null)
                FileName = Path.GetFileName(FullFileName);
            switch (State)
            {
                case PascalABCCompiler.CompilerState.CompilationStarting:
                    RusName = VECStringResources.Get("STATE_START_COMPILING_ASSEMBLY{0}");
                    SetCompilingAndRunButtonsEnabled(false);
                    ParsedFiles.Clear();
                    break;
                case PascalABCCompiler.CompilerState.BeginCompileFile: RusName = VECStringResources.Get("STATE_BEGINCOMPILEFILE{0}"); break;
                case PascalABCCompiler.CompilerState.CompileInterface: RusName = VECStringResources.Get("STATE_COMPILEINTERFACE{0}"); break;
                case PascalABCCompiler.CompilerState.CompileImplementation: RusName = VECStringResources.Get("STATE_COMPILEIMPLEMENTATION{0}"); break;
                case PascalABCCompiler.CompilerState.EndCompileFile: RusName = VECStringResources.Get("STATE_ENDCOMPILEFILE{0}"); break;
                case PascalABCCompiler.CompilerState.CodeGeneration: RusName = VECStringResources.Get("STATE_CODEGENERATION{0}"); break;
                case PascalABCCompiler.CompilerState.ReadDLL: RusName = VECStringResources.Get("STATE_READDLL{0}"); break;
                case PascalABCCompiler.CompilerState.SavePCUFile: RusName = VECStringResources.Get("STATE_SAVEPCUFILE{0}"); break;
                case PascalABCCompiler.CompilerState.ReadPCUFile: RusName = VECStringResources.Get("STATE_READPCUFILE{0}"); break;
                case PascalABCCompiler.CompilerState.PCUReadingError: RusName = VECStringResources.Get("STATE_PCUREADINGERROR{0}"); break;
                case PascalABCCompiler.CompilerState.PCUWritingError: RusName = VECStringResources.Get("STATE_PCUWRITINGERROR{0}"); break;
                case PascalABCCompiler.CompilerState.SemanticTreeConversion: RusName = VECStringResources.Get("STATE_SEMANTICTREECONVERSION{0}"); break;
                case PascalABCCompiler.CompilerState.SemanticTreeConverterConnected: RusName = VECStringResources.Get("STATE_SEMANTICTREECONVERTERCONNECTED{0}"); break;
                case PascalABCCompiler.CompilerState.SyntaxTreeConversion: RusName = VECStringResources.Get("STATE_SYNTAXTREECONVERSION{0}"); break;
                case PascalABCCompiler.CompilerState.Ready:
                    RusName = VECStringResources.Get("STATE_READY");
                    if (!StartingCompleted)
                    {
                        switch (sender.CompilerType)
                        {
                            case PascalABCCompiler.CompilerType.Standart:
                                standartCompiler = (PascalABCCompiler.Compiler)sender;
                                break;
                            case PascalABCCompiler.CompilerType.Remote:
                                remoteCompiler = (PascalABCCompiler.RemoteCompiler)sender;
                                break;
                        }
                        if (!(defaultCompilerType == PascalABCCompiler.CompilerType.Remote) || (standartCompiler.State == PascalABCCompiler.CompilerState.Ready && (remoteCompiler != null && remoteCompiler.State == PascalABCCompiler.CompilerState.Ready)))
                            StartingCompleted = true;
                        AddCompilerTextToCompilerMessages(sender, VECStringResources.Get("SUPPORTED_LANGUAGES") + Environment.NewLine);
                        foreach (ILanguage lang in LanguageProvider.Instance.Languages)
                            AddCompilerTextToCompilerMessages(sender, string.Format(VECStringResources.Get("CM_LANGUAGE_{0}"), 
                                PascalABCCompiler.FormatTools.LanguageAndExtensionsFormatted(lang.Name, lang.FilesExtensions) + Environment.NewLine));
                        if (StartingCompleted)
                        {
                            ChangeVisualEnvironmentState(VisualEnvironmentState.FinishCompilerLoading, standartCompiler);
                            compilerLoaded = true;
            				VisualPABCSingleton.MainForm.StartTimer();
                            /*if (!ThreadPool.QueueUserWorkItem(new WaitCallback(CompileTestProgram)))
                                CompileTestProgram(null);*/
                            if (!ThreadPool.QueueUserWorkItem(new WaitCallback(LoadPlugins)))
                                LoadPlugins(null);
                            
                            SetCompilingAndRunButtonsEnabled(true);
                            if (CodeCompletion.CodeCompletionController.IntellisenseAvailable())
                                SetDebugButtonsEnabled(true);
                        }
                    }
                    else
                    {
                        if (Compiler == sender)
                        {
                            SetCompilingAndRunButtonsEnabled(true); 
                        }
                    }
                    break;
                case PascalABCCompiler.CompilerState.Reloading:
                    if (Compiler == sender)
                        SetCompilingAndRunButtonsEnabled(false);
                    RusName = VECStringResources.Get("STATE_RELOADING");
                    break;
                case PascalABCCompiler.CompilerState.CompilationFinished:
                    RusName = VECStringResources.Get("STATE_COMPILATIONFINISHED{0}");
                    if (Compiler.ErrorsList.Count == 0)
                    {
                        SaveSourceFilesIfCompilationOk();
                        AddTextToCompilerMessages(string.Format(VECStringResources.Get("CM_OK_{0}MS") + Environment.NewLine, (DateTime.Now - compilationStartTime).TotalMilliseconds) + Environment.NewLine);
                    }
                    else
                    {
                        List<PascalABCCompiler.Errors.Error> ErrorsList = ErrorsManager.CreateErrorsList(Compiler.ErrorsList);
                        AddTextToCompilerMessages(string.Format(VECStringResources.Get("CM_{0}_ERROS") + Environment.NewLine, ErrorsList.Count));
                        foreach (PascalABCCompiler.Errors.Error Err in ErrorsList)
                            AddTextToCompilerMessages(Err.ToString() + Environment.NewLine);
                    }

                    break;
                default: 
                    //RusName = State.ToString(); 
                    break;
            }
            if (RusName != null)
            {
                if (FileName != null)
                    RusName = string.Format(RusName, FileName);
                else RusName = string.Format(RusName, "");
                RusName += Environment.NewLine;
                AddCompilerTextToCompilerMessages(sender, RusName);
            }

            RusName = null;
            switch (State)
            {
                case PascalABCCompiler.CompilerState.BeginParsingFile:
                    ParsedFiles.Add(FullFileName);
                    break;
                case PascalABCCompiler.CompilerState.BeginCompileFile: RusName = VECStringResources.Get("STATETEXT_BEGINCOMPILEFILE{0}"); break;
                //case PascalABCCompiler.CompilerState.ReadPCUFile: RusName = "Чтение {0}..."; break;
                case PascalABCCompiler.CompilerState.CodeGeneration: RusName = VECStringResources.Get("STATETEXT_CODEGENERATION{0}"); break;
                //case PascalABCCompiler.CompilerState.ReadDLL: 
                //case PascalABCCompiler.CompilerState.ReadPCUFile: RusName = "Чтение {0}..."; break;
                case PascalABCCompiler.CompilerState.Ready:
                    if (Compiler != null)
                    {
                        List<PascalABCCompiler.Errors.Error> ErrorsList = ErrorsManager.CreateErrorsList(Compiler.ErrorsList);
                        if (ErrorsList.Count > 0)
                        {
                            RusName = string.Format(VECStringResources.Get("STATETEXT_{0}_ERROS"), ErrorsList.Count);
                        }
                        else
                            if (Compilation)
                            {
                                if (Compiler.Warnings.Count > 0)
                                    RusName = string.Format(VECStringResources.Get("STATETEXT_COMPILATION_SUCCESS_{0}LINES_{1}WARNINGS"), Compiler.LinesCompiled, Compiler.Warnings.Count);
                                else
                                    RusName = string.Format(VECStringResources.Get("STATETEXT_COMPILATION_SUCCESS_{0}LINES"), Compiler.LinesCompiled);
                            }
                            else
                                RusName = VECStringResources.Get("STATETEXT_READY");
                    }
                    else
                        RusName = VECStringResources.Get("STATETEXT_READY");
                    break;
                case PascalABCCompiler.CompilerState.Reloading: RusName = VECStringResources.Get("STATETEXT_RELOADING"); break;
            }
            if (RusName != null)
            {
                if (FileName != null)
                    RusName = string.Format(RusName, FileName);
                SetStateText(RusName);


            }
            if (!Compilation && State == PascalABCCompiler.CompilerState.CompilationStarting)
                Compilation = true;
            if (Compilation && State == PascalABCCompiler.CompilerState.Ready)
                Compilation = false;
        }


        void SaveSourceFilesIfCompilationOk()
        {
            if (!UserOptions.SaveSourceFilesIfComilationOk)
                return;
            foreach (string name in ParsedFiles)
                ExecuteAction(VisualEnvironmentCompilerAction.SaveFile, name);
        }

        #region IVisualEnvironmentCompiler Members

        public event ChangeVisualEnvironmentStateDelegate ChangeVisualEnvironmentState;

        public PascalABCCompiler.ICompiler Compiler
        {
            get 
            {
                if (defaultCompilerType == PascalABCCompiler.CompilerType.Remote)
                    return remoteCompiler;
                return standartCompiler;
        }
        }
        public PascalABCCompiler.Compiler StandartCompiler
        {
            get
            {
                return standartCompiler;
            }
        }
        public PascalABCCompiler.RemoteCompiler RemoteCompiler
        {
            get
            {
                return remoteCompiler;
            }
        }

        public void ExecuteSourceLocationAction(PascalABCCompiler.SourceLocation SourceLocation,SourceLocationAction Action)
        {
            ExecuteSLAction(SourceLocation,Action);
        }

        public object ExecuteAction(VisualEnvironmentCompilerAction Action, object obj)
        {
            return ExecuteVECAction(Action, obj);
        }

        public PascalABCCompiler.CompilerType DefaultCompilerType
        {
            get
            {
                return defaultCompilerType;
            }
            set
            {
                if (defaultCompilerType == value)
                    return;
                defaultCompilerType=value;
                if (defaultCompilerType == PascalABCCompiler.CompilerType.Remote && remoteCompiler == null)
                {
                    LoadRemoteCompiler();
                }
            }
        }

        #endregion
    }
}
