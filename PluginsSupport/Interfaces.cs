// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;

namespace VisualPascalABCPlugins
{
    public interface IPluginGUIItem
    {
        string Text
        {
            get;
        }
        string Hint
        {
            get;
        }
        System.Drawing.Image Image
        {
            get;
        }
        System.Drawing.Color ImageTransparentColor
        {
            get;
        }
        System.Windows.Forms.Keys ShortcutKeys
        {
            get;
        }
        string ShortcutKeyDisplayString
        {
            get;
        }

        /// SSM 07.08.22 кнопка пункта меню. В плагине преобразовать к ToolStripMenuItem
        object menuItem { get; set; }

        /// SSM 07.08.22 кнопка панели инструментов. В плагине преобразовать к ToolStripButton
        object toolStripButton { get; set; } 

        void Execute();
    }

    public interface IVisualPascalABCPlugin
    {
        string Name
        {
            get;
        }
        string Version
        {
            get;
        }
        string Copyright
        {
            get;
        }
        void GetGUI(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems);
    }

    public interface IExtendedVisualPascalABCPlugin: IVisualPascalABCPlugin
    {
        void AfterAddInGUI();
    }


    public enum VisualEnvironmentState
    {
        CaretPosChangedMouse,
        CaretPosChangedKeyboard,
        StartCompilerLoading,
        FinishCompilerLoading,
        ProcessStarting,
        ProcessStartingDebug,
        ProcessExited
    }
    public delegate void ChangeVisualEnvironmentStateDelegate(VisualEnvironmentState State, object obj);
    // SSM 16/05/22 - перенес сюда из RunManager
    public delegate void RunnerManagerActionDelegate(string fileName);
    // SSM 02.07.22 - перед стартом компиляции для плагинов
    public delegate void BuildServiceActionDelegate(string fileName);
    // SSM 02.07.22 - перед запуском изменить параметры командной строки
    public delegate void ChangeArgsBeforeRunDelegate(ref string args);

    public enum SourceLocationAction
    {
        SelectAndGotoBeg,
        SelectAndGotoEnd,
        GotoBeg,
        GotoEnd,
        NavigationGoto,
        FindSelection
    }
    public enum VisualEnvironmentCompilerAction
    {
        Run,
        Build,
        Rebuild,
        Stop,
        OpenFile,
        GetDirectory,
        GetCurrentSourceFileName,
        SetCurrentSourceFileText,
        AddTextToCompilerMessages,
        SaveFile,
        BuildUnit,
        AddMessageToErrorListWindow,
        SetCurrentSourceFileTextFormatting,
        PT4PositionCursorAfterTask
    }

    public delegate IAsyncResult InvokeDegegate(Delegate method, params object[] args);

    public delegate string ConvertTextDelegate(string FileName, string text); // Это пока только для шифрованного Tasks. 

    public interface IVisualEnvironmentCompiler
    {
        PascalABCCompiler.ICompiler Compiler
        {
            get;
        }
        PascalABCCompiler.Compiler StandartCompiler
        {
            get;
        }
        PascalABCCompiler.RemoteCompiler RemoteCompiler
        {
            get;
        }
        PascalABCCompiler.CompilerType DefaultCompilerType
        {
            get;
            set;
        }
        void ExecuteSourceLocationAction(PascalABCCompiler.SourceLocation SourceLocation, SourceLocationAction Action);
        event ChangeVisualEnvironmentStateDelegate ChangeVisualEnvironmentState;

        object ExecuteAction(VisualEnvironmentCompilerAction Action, object obj);

        string Compile(PascalABCCompiler.CompilerOptions CompilerOptions);
        void StartCompile(PascalABCCompiler.CompilerOptions CompilerOptions);

        object SourceFilesProvider(string FileName, PascalABCCompiler.SourceFileOperation FileOperation);
        InvokeDegegate BeginInvoke
        {
            get;
        }
        ConvertTextDelegate ConvertUnitTextProperty { get; set; }
    }

    public interface ICodeFileDocument
    {
        string FileName
        {
            get;
        }

        string EXEFileName
        {
            get;
        }

        

        int LinesCount
        {
            get;
        }

        ICSharpCode.TextEditor.TextEditorControl TextEditor
        {
            get;
        }

        string Text
        {
            get;
            set;
        }

        bool FromMetadata
        {
            get;
        }

        bool DocumentChanged
        {
            get;
        }

        string ToolTipText
        {
            get;
            set;
        }

        bool Run
        {
            get;
            set;
        }

        void LoadFromFile(string fileName);

    }

    public interface ITextEditorControl
    {
        void SetFocus();
        int CaretLine { get; set; }
        int CaretColumn { get; set; }
    }

    public class ListItemEventArgs : EventArgs
    {
        IListItem listItem;

        public IListItem ListItem
        {
            get
            {
                return listItem;
            }
        }

        public ListItemEventArgs(IListItem listItem)
        {
            this.listItem = listItem;
        }
    }

    public interface IUserOptions
    {
        bool RedirectConsoleIO { get; set; }
        bool ShowLineNums { get; set; }
        bool EnableFolding { get; set; }
        bool ShowMatchBracket { get; set; }
        bool ConverTabsToSpaces { get; set; }
        bool DeleteEXEAfterExecute { get; set; }
        bool DeletePDBAfterExecute { get; set; }
        bool UseOutputDirectory { get; set; }
        string OutputDirectory { get; set; }
        bool PauseInRunModeIfConsole { get; set; }
        string DefaultSourceFileNameFormat { get; set; }
        int TabIndent { get; set; }
        int EditorFontSize { get; set; }
        bool AllowCodeCompletion { get; set; }
        bool CodeCompletionHint { get; set; }
        bool CodeCompletionDot { get; set; }
        bool CodeCompletionParams { get; set; }
        bool SaveSourceFilesIfComilationOk { get; set; }
        int CodeCompletionNamespaceVisibleRange { get; set; }
        int CursorTabCount { get; set; }
        bool ShowCompletionInfoByGroup { get; set; }
        bool EnableSmartIntellisense { get; set; }
        bool ShowQuickClassBrowserPanel { get; set; }
        bool AllowCodeFormatting { get; set; }
        bool SkipStakTraceItemIfSourceFileInSystemDirectory { get; set; }
        bool AlwaysAttachDebuggerAtStart { get; set; }
        string CurrentFontFamily { get; set; }
        bool HighlightOperatorBrackets { get; set; }
        bool UseDllForSystemUnits { get; set; }
        bool PABCDllChecked { get; set; }
        bool AutoInsertCodeIsEnabledOnStartup { get; set; }
        bool UseSemanticIntellisense { get; set; }
    }

    public interface IListItem
    {
        int ImageIndex { get; }
        string Name { get; }
        string SpecialName { get; set; }
        string Text { get; }
        bool CanEditText { get; }
        string Type { get; }
        bool HasSubItems { get; }
        IList<IListItem> SubItems { get; }
        bool IsLiteral { get; }
        System.Drawing.Image Image { get; }
        event EventHandler<ListItemEventArgs> Changed;
    }

    public interface IWorkbenchFileService
    {
        bool OpenFile(string FileName, string PreferedFileName, bool notOpenDesigner = false);
        void RenameFile(string OldFileName, string NewFileName);
        void CloseFile(string FileName);
        void ReloadFile(string FileName);
        bool SaveAll(bool Question);
        void SetFileAsChanged(string FileName);
        void OpenTabWithText(string tabName, string Text);
        ICodeFileDocument OpenFileForDebug(string FileName);
        void CloseAllButThis(ICodeFileDocument nonCloseTab);
        void PrintActiveDocument();
    }

    public interface IWorkbenchProjectService
    {
        void NewProject();
        void OpenProject(string projectFileName);
        bool CloseProject();
        void SaveProject();
        void AddLastProject(string FileName);
    }

    public interface IWorkbenchDesignerService
    {
        void DisableDesigner();
        void EnableDesigner();
        void GenerateAllDesignersCode();
        void SetActiveDesignerDirty();
    }

    public interface IWorkbenchRunService
    {
        event RunnerManagerActionDelegate Starting; // можно вызывать из плагина
        event RunnerManagerActionDelegate Exited;   // можно вызывать из плагина
        event ChangeArgsBeforeRunDelegate ChangeArgsBeforeRun;
        bool IsRun();
        bool IsRun(string FileName);
        bool Run(bool RedirectConsoleIO);
        bool Stop();
        void Stop(string FileName);
        bool Run(bool forDebugging, bool startWithGoto, bool need_first_brpt);
        bool Run(ICodeFileDocument tabPage, bool forDebugging, bool startWithGoto, bool needFirstBreakpoint);
        void Run(string fileName, bool redirectIO, string ModeName, bool RunWithPause, string WorkingDirectory, string CommandLineArguments, bool attachDebugger, bool fictive_attach);
        bool HasRunArgument(string FileName);
        string GetRunArgument(string FileName);
        void AddRunArgument(string FileName, string argument);
        ICodeFileDocument GetRunTab(string FileName);
        void SendInputTextToProcess();
        void UpdateReadRequest(bool changeSelected);
        void KillAll();
    }

    public interface IWorkbenchUpdateService
    {
        void CheckForUpdates();
    }

    public interface IWorkbenchBuildService
    {
        BuildServiceActionDelegate BeforeCompile { get; set; }
        string Compile(string FileName, bool rebuild, string RuntimeServicesModule, bool ForRun, bool RunWithEnvironment);
        string Compile(PascalABCCompiler.IProjectInfo project, bool rebuild, string RuntimeServicesModule, bool ForRun, bool RunWithEnvironment);
        bool Build();
        bool Rebuild();
        bool Build(string FileName);
        void StartCompile(bool rebuild);

        List<PascalABCCompiler.Errors.Error> ErrorsList
        {
            get;
        }

        PascalABCCompiler.CompilerOptions CompilerOptions
        {
            get;
        }
    }

    public interface IWidgetStatusContoller
    {
        void SetStartDebugAndRunEnabled();
        void SetStartDebugAndRunDisabled();
        void SetDebugStopDisabled();
        void SetDebugStopEnabled();
        void SetAddExprMenuVisible(bool val);
        void SetDisassemblyMenuVisible(bool val);
        void SetDebugTabsVisible(bool val);
        void SetDebugPausedDisabled();
        void SetPlayButtonsVisible(bool val);
        void SetStopEnabled(bool enabled);
        void SetCompilingButtonsEnabled(bool Enabled);
        void SetDebugAndRunButtonsEnabled(bool val);
        void SetOptionsEnabled(bool val);
        bool CompilingButtonsEnabled { get; set; }
        void EnableCodeCompletionToolTips(bool val);
        void ChangeContinueDebugNameOnStart();
        void ChangeStartDebugNameOnContinue();
    }

    public interface IHelpService
    {
        void ExecShowHelpF1();
        void OpenMSDN();
    }

    public interface IEditorService
    {
        void SetFocusToEditor();
        void ExecSelectAll();
        void ExecGotoLine();
        void ExecFindReferences();
        void ExecCut();
        void ExecCopy();
        void ExecPaste();
        void ExecReplace();
        void ExecFind();
        void ExecFindNext();
        void ExecUndo();
        void ExecRedo();
        void CollapseRegions();
        void CodeFormat();
        void SetEditorDisabled(bool val);
    }

    public interface IWorkbenchDebuggerOperationsService
    {
        void RefreshPad(IList<IListItem> items);
        void GotoWatch();
        void AddVariable(string expr, bool show_tab);
        void AddDebugPage(ICodeFileDocument tabPage);
        void ClearWatch();
        void ClearLocalVarTree();
        void ClearDebugTabs();
        void DisplayDisassembledCode(string code);
    }

    public interface IWorkbenchOperationsService
    {
        void ClearTabStack();
        void SendNewLineToInputTextBox();
        void AddTextToOutputWindowSync(string fileName, string text);
        void WriteToOutputBox(string message, bool is_exc);
        void ClearOutputTextBoxForTabPage(ICodeFileDocument tabPage);
        void AddTabWithUrl(string title, string url);
        void AddTextToCompilerMessagesSync(string text);
    }

    public interface IWorkbenchOptionService
    {
        void SaveOptions();
        void LoadOptions();
        void UpdateUserOptions();
    }

    public interface IWorkbenchDocumentService
    {
        ICodeFileDocument CurrentCodeFileDocument { get; set; }
        ICodeFileDocument ActiveCodeFileDocument { get; set; }
        ICodeFileDocument LastSelectedTab { get; }
        bool ContainsTab(ICodeFileDocument tab);
        bool ContainsTab(string FileName);
        ICodeFileDocument GetDocument(string FileName);
        ICodeFileDocument GetTabPageForMainFile();
        void SetTabPageText(ICodeFileDocument tp);
    }

    public interface IWorkbenchServiceContainer
    {
        IWorkbenchDocumentService DocumentService { get; }
        IWorkbenchOptionService OptionsService { get; }
        IEditorService EditorService { get; }
        IHelpService HelpService { get; }
        IWorkbenchDebuggerOperationsService DebuggerOperationsService { get; }
        IWorkbenchBuildService BuildService { get; }
        IWorkbenchFileService FileService { get; }
        IWorkbenchProjectService ProjectService { get; }
        IWorkbenchRunService RunService { get; }
        IWorkbenchDesignerService DesignerService { get; }
        IWorkbenchOperationsService OperationsService { get; }
        IWorkbenchUpdateService UpdateService { get;  }
        ICodeCompletionService CodeCompletionService { get; }
    }

    public interface ICodeCompletionService
    {
        PascalABCCompiler.Parsers.ICodeCompletionDomConverter GetConverter(string FileName);
        void SetAsChanged(string FileName);
        void RegisterFileForParsing(string FileName);
    }

    public interface IWorkbench
    {
        IVisualEnvironmentCompiler VisualEnvironmentCompiler { get; }
        IWidgetStatusContoller WidgetController { get; }
        IDebuggerManager DebuggerManager { get; }
        System.Windows.Forms.Form MainForm { get; }
        IWorkbenchServiceContainer ServiceContainer { get; }
        IUserOptions UserOptions { get; }
        PascalABCCompiler.Errors.ErrorsStrategyManager ErrorsManager { get; }
        string CurrentEXEFileName { get; }
        ICompilerConsoleWindow CompilerConsoleWindow { get; }
        IOutputWindow OutputWindow { get; }
        IErrorListWindow ErrorsListWindow { get; }
        IDisassemblyWindow DisassemblyWindow { get; }
        void BeginInvoke(Delegate del, params object[] args);
    }

    public interface ICompilerConsoleWindow
    {
        void AddTextToCompilerMessages(string text);
        void ClearConsole();
    }

    public interface IErrorListWindow
    {
        void ShowErrorsSync(List<PascalABCCompiler.Errors.Error> errors, bool ChangeViewTab);
        void ClearErrorList();
    }

    public interface IDisassemblyWindow
    {
        bool IsVisible { get; }
        void ClearWindow();
    }

    public interface IOutputWindow
    {
        bool InputPanelVisible { get; set; }
        string InputTextBoxText { get; set; }
        void ClearTextBox();
        void InputTextBoxCursorToEnd();
        void OutputTextBoxScrolToEnd();
        void AppendTextToOutputBox(string Text);
    }

    public interface IValue
    {
    }

    public interface IRetValue
    {
        IValue ObjectValue { get; }
        object PrimitiveValue { get; }
    }
	
    public interface IProcess
    {
    	bool HasExited { get; }
    }
    
    public interface IProcessEventArgs
    {
    	IProcess Process { get; }
    }
    
    public interface IDebuggerManager
    {
        bool IsRunning { get; }
        bool IsRun(string FileName);
        bool IsRun(string ExeFileName, string SourceFileName);
        string MakeValueView(IValue value);
        IRetValue Evaluate(string expr);
        void NullProcessHandleIfNeed(string FileName);
        void Attach(uint handle, string fileName, bool real_attach, bool late_attach);
        void Continue();
        void StepInto();
        void StepOver();
        void RunToCursor();
        event EventHandler<EventArgs> DebuggeeStateChanged;
    }

    public interface ILanguageManager
    {
        PascalABCCompiler.ICompiler StandartCompiler { get; }
        PascalABCCompiler.ICompiler RemoteCompiler { get; }
        IDebuggerManager Debugger { get; }
    }
}
