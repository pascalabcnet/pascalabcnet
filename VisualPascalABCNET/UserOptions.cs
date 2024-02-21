// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;

namespace VisualPascalABC
{
    public class UserOptions : VisualPascalABCPlugins.IUserOptions
    {
        public bool RedirectConsoleIO = false;
        public bool ShowLineNums = false;
        public bool EnableFolding = false; // SSM 4.09.08
        public bool ShowMatchBracket = false;
        public bool ConverTabsToSpaces = true;
        public bool deleteEXEAfterExecute = false;
        public bool deletePDBAfterExecute = true;
        public bool UseOutputDirectory = true;
        public string OutputDirectory = null;
        public bool PauseInRunModeIfConsole = true;
        public string DefaultSourceFileNameFormat = "Program{0}.pas";
        public int TabIndent = 2;
        public int EditorFontSize = 12;
        public bool AllowCodeCompletion = true;
        public bool CodeCompletionHint = true;
        public bool CodeCompletionDot = true;
        public bool CodeCompletionParams = true;
        public bool SaveSourceFilesIfComilationOk = false;
        public int CodeCompletionNamespaceVisibleRange = 3;
        public int CursorTabCount = 2;
        public bool ShowCompletionInfoByGroup = true;
        public bool EnableSmartIntellisense = false;
        public bool ShowQuickClassBrowserPanel = false;
        public bool UseSemanticIntellisense = false;
        public bool AllowCodeFormatting = false;
        public bool SkipStackTraceItemIfSourceFileInSystemDirectory = true;
        public bool AlwaysAttachDebuggerAtStart = false;
        public string CurrentFontFamily = "Consolas";
        public bool HighlightOperatorBrackets=true;
        public bool UseDllForSystemUnits = true;
        public bool PABCDllChecked = false;
        public bool AutoInsertCodeIsEnabledOnStartup = true;

        public UserOptions()
        {
            try // SSM 25/04/22
            {
                var installedFontCollection = new System.Drawing.Text.InstalledFontCollection();
                if (!Array.Exists(installedFontCollection.Families, f => f.Name == CurrentFontFamily))
                    CurrentFontFamily = "Courier New";
            }
            catch { }
        }

        public bool DeleteEXEAfterExecute
        {
            get
            {
                if (ProjectFactory.Instance.CurrentProject != null)
                    return ProjectFactory.Instance.CurrentProject.DeleteEXE;
                return deleteEXEAfterExecute;
            }
            set
            {
                deleteEXEAfterExecute = value;
            }
        }

        public bool DeletePDBAfterExecute
        {
            get
            {
                if (ProjectFactory.Instance.CurrentProject != null)
                    return ProjectFactory.Instance.CurrentProject.DeletePDB;
                return deletePDBAfterExecute;
            }
            set
            {
                deletePDBAfterExecute = value;
            }
        }

        #region IUserOptions Member

        bool VisualPascalABCPlugins.IUserOptions.AllowCodeCompletion
        {
            get { return AllowCodeCompletion; }
            set { AllowCodeCompletion = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.AllowCodeFormatting
        {
            get { return AllowCodeFormatting; }
            set { AllowCodeFormatting = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.AlwaysAttachDebuggerAtStart
        {
            get { return AlwaysAttachDebuggerAtStart; }
            set { AlwaysAttachDebuggerAtStart = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.CodeCompletionDot
        {
            get { return CodeCompletionDot; }
            set { CodeCompletionDot = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.CodeCompletionHint
        {
            get { return CodeCompletionHint; }
            set { CodeCompletionHint = value; }
        }

        int VisualPascalABCPlugins.IUserOptions.CodeCompletionNamespaceVisibleRange
        {
            get { return CodeCompletionNamespaceVisibleRange; }
            set { CodeCompletionNamespaceVisibleRange = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.CodeCompletionParams
        {
            get { return CodeCompletionParams; }
            set { CodeCompletionParams = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.ConverTabsToSpaces
        {
            get { return ConverTabsToSpaces; }
            set { ConverTabsToSpaces = value; }
        }

        string VisualPascalABCPlugins.IUserOptions.CurrentFontFamily
        {
            get { return CurrentFontFamily; }
            set { CurrentFontFamily = value; }
        }

        int VisualPascalABCPlugins.IUserOptions.CursorTabCount
        {
            get { return CursorTabCount; }
            set { CursorTabCount = value; }
        }

        string VisualPascalABCPlugins.IUserOptions.DefaultSourceFileNameFormat
        {
            get { return DefaultSourceFileNameFormat; }
            set { DefaultSourceFileNameFormat = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.DeleteEXEAfterExecute
        {
            get { return DeleteEXEAfterExecute; }
            set { DeleteEXEAfterExecute = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.DeletePDBAfterExecute
        {
            get { return DeletePDBAfterExecute; }
            set { DeletePDBAfterExecute = value; }
        }

        int VisualPascalABCPlugins.IUserOptions.EditorFontSize
        {
            get { return EditorFontSize; }
            set { EditorFontSize = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.EnableFolding
        {
            get { return EnableFolding; }
            set { EnableFolding = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.EnableSmartIntellisense
        {
            get { return EnableSmartIntellisense; }
            set { EnableSmartIntellisense = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.HighlightOperatorBrackets
        {
            get { return HighlightOperatorBrackets; }
            set { HighlightOperatorBrackets = value; }
        }

        string VisualPascalABCPlugins.IUserOptions.OutputDirectory
        {
            get { return OutputDirectory; }
            set { OutputDirectory = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.PauseInRunModeIfConsole
        {
            get { return PauseInRunModeIfConsole; }
            set { PauseInRunModeIfConsole = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.RedirectConsoleIO
        {
            get { return RedirectConsoleIO; }
            set { RedirectConsoleIO = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.SaveSourceFilesIfComilationOk
        {
            get { return SaveSourceFilesIfComilationOk; }
            set { SaveSourceFilesIfComilationOk = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.AutoInsertCodeIsEnabledOnStartup
        {
            get { return AutoInsertCodeIsEnabledOnStartup; }
            set { AutoInsertCodeIsEnabledOnStartup = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.ShowCompletionInfoByGroup
        {
            get { return ShowCompletionInfoByGroup; }
            set { ShowCompletionInfoByGroup = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.ShowLineNums
        {
            get { return ShowLineNums; }
            set { ShowLineNums = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.ShowMatchBracket
        {
            get { return ShowMatchBracket; }
            set { ShowMatchBracket = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.ShowQuickClassBrowserPanel
        {
            get { return ShowQuickClassBrowserPanel; }
            set { ShowQuickClassBrowserPanel = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.SkipStakTraceItemIfSourceFileInSystemDirectory
        {
            get { return SkipStackTraceItemIfSourceFileInSystemDirectory; }
            set { SkipStackTraceItemIfSourceFileInSystemDirectory = value; }
        }

        int VisualPascalABCPlugins.IUserOptions.TabIndent
        {
            get { return TabIndent; }
            set { TabIndent = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.UseOutputDirectory
        {
            get { return UseOutputDirectory; }
            set { UseOutputDirectory = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.UseDllForSystemUnits
        {
            get { return UseDllForSystemUnits; }
            set { UseDllForSystemUnits = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.PABCDllChecked
        {
            get { return PABCDllChecked; }
            set { PABCDllChecked = value; }
        }

        bool VisualPascalABCPlugins.IUserOptions.UseSemanticIntellisense
        {
            get { return UseSemanticIntellisense; }
            set { UseSemanticIntellisense = value; }
        }

        #endregion
    }
}
