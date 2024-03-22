// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VisualPascalABC
{
    public partial class Form1 : Form, VisualPascalABCPlugins.IWorkbench, VisualPascalABCPlugins.IWorkbenchOptionService
    {
        private string OldOptionsFileName = Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName, ".ini");
        private string OptionsFileName = Path.Combine(Constants.DefaultWorkingDirectory, "PascalABCNET.ini");
        public UserOptions UserOptions;

        private string OptionsItemNameLanguage = "Language";
        private string OptionsItemNameMainFormLeft = "MainFormLeft";
        private string OptionsItemNameMainFormTop = "MainFormTop";
        private string OptionsItemNameMainFormHeight = "MainFormHeight";
        private string OptionsItemNameMainFormWidth = "MainFormWidth";
        private string OptionsItemNameMainFormMaximized = "MainFormMaximized";
        private string OptionsItemNameShowLinesNum = "ShowLinesNum";
        private string OptionsItemNameEnableFolding = "EnableFolding";
        private string OptionsItemNameRedirectConsoleIO = "RedirectConsoleIO";
        private string OptionsItemNameGenerateDebugInfo = "GenerateDebugInfo";
        private string OptionsItemNameLastFile = "LastFile";
        private string OptionsItemNameLastProject = "LastProject";
        private string OptionsItemNameWorkDirectory = "WorkDirectory";
        private string OptionsItemNameShowOutputWindow = "ShowOutputWindow";
        private string OptionsItemNameErrorsStrategy = "ErrorsStrategy";
        private string OptionsItemNameEditorFontFamily = "EditorFontFamily";
        private string OptionsItemNameEditorFontSize = "EditorFontSize";
        private string OptionsItemNameErrorsCursorPosStrategy = "ErrorsCursorPosStrategy";
        private string OptionsItemNameShowMatchBracket = "ShowMatchBracket";
        private string OptionsItemNameShowMatchOperatorBracket = "ShowMatchOperatorBracket";
        private string OptionsItemNameConvertTabsToSpaces = "ConvertTabsToSpaces";
        private string OptionsItemNameTabIdent = "TabIdent";
        private string OptionsItemNameDeleteEXEAfterExecute = "DeleteEXEAfterExecute";
        private string OptionsItemNameDeletePDBAfterExecute = "DeletePDBAfterExecute";
        private string OptionsItemNameOutputDirectory = "OutputDirectory";
        private string OptionsItemNameUseOutputDirectory = "UseOutputDirectory";
        private string OptionsItemNameDefaultSourceFileNameFormat = "DefaultSourceFileNameFormat";
        private string OptionsItemNamePlayPauseButtonsVisibleInPanel = "PlayPauseButtonsVisibleInPanel";
        private string OptionsItemNamePauseInRunModeIfConsole = "PauseInRunModeIfConsole";
        private string OptionsItemNameMainFormTitle = "MainFormTitle";
        private string OptionsItemNameCodeCompletionDot = "CodeCompletionDot";
        private string OptionsItemNameCodeCompletionHint = "CodeCompletionHint";
        private string OptionsItemNameCodeCompletionParams = "CodeCompletionParams";
        private string OptionsItemNameCodeCompletionKeyPressed = "CodeCompletionKeyPressed";
        private string OptionsItemNameCodeCompletionNamespaceVisibleRange = "CodeCompletionNamespaceVisibleRange";
        private string OptionsItemNameAllowCodeCompletion = "AllowCodeCompletion";
        private string OptionsItemNameSaveSourceFilesIfComilationOk = "SaveSourceFilesIfComilationOk";
        private string OptionsItemNameDockBottomPortion = "DockBottomPortion";
        private string OptionsItemNameDockLeftPortion = "DockLeftPortion";
        private string OptionsItemNameDockRightPortion = "DockRightPortion";
        private string OptionsItemNameShowQuickClassBrowserPanel = "ShowQuickClassBrowserPanel";
        private string OptionsItemNameUseSemanticIntellisense = "UseSemanticIntellisense";
        private string OptionsItemNameSkipStackTraceItemIfSourceFileInSystemDirectory = "SkipStakTraceItemIfSourceFileInSystemDirectory";
        private string OptionsItemNameShowFoundedNamesTab = "ShowFoundedNamesTab";
        private string OptionsItemNameShowWatchTab = "ShowWatchTab";
        private string OptionsItemNameShowLocalVarsTab = "ShowLocalVarsTab";
        private string OptionsItemUseDllForSystemModules = "UseDllForSystemModules";
        private string OptionsItemPABCDllChecked = "PABCDllChecked";
        private string OptionsItemNameAutoInsertCode = "AutoInsertCodeIsEnabledOnStartup";

        bool _mainFormWindowStateMaximized = false;

        VisualPascalABCPlugins.IUserOptions VisualPascalABCPlugins.IWorkbench.UserOptions
        {
            get
            {
                return UserOptions;
            }
        }

        public void LoadOptions()
        {
            string FileName = OptionsFileName;
            try
            {
                if (!File.Exists(FileName))
                {
                    try
                    {
                        File.Copy(OldOptionsFileName, FileName, true);
                    }
                    catch
                    {
                    }
                }
                if (!File.Exists(FileName))
                {
                    string language = "Русский";
                    try
                    {
                        var cp = Registry.CurrentUser.OpenSubKey("Software\\PascalABC.NET").GetValue("Installer Language").ToString();
                        if (cp == "1033")
                            language = "English";
                    }
                    catch(Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                    PascalABCCompiler.StringResourcesLanguage.CurrentLanguageName = language;
                    CodeCompletionParserController.CurrentTwoLetterISO = PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO;
                    return;
                }
                Hashtable Options = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
                PascalABCCompiler.AdvancedOptions adv_opt = new PascalABCCompiler.AdvancedOptions();
                PascalABCCompiler.StringResources.ReadStringsFromStreamAsXml(FileName, new StreamReader(FileName, VisualEnvironmentCompiler.DefaultFileEncoding), Options, adv_opt);
                string value;
                int val;
                if ((value = (string)Options[OptionsItemNameWorkDirectory]) != null)
                {
                    WorkbenchStorage.WorkingDirectoryInOptionsFile = value;
                    WorkbenchStorage.WorkingDirectory = PascalABCCompiler.Tools.ReplaceAllKeys(value, WorkbenchStorage.StandartDirectories);
                }
                if ((value = (string)Options[OptionsItemNameLanguage]) != null)
                {
                    PascalABCCompiler.StringResourcesLanguage.CurrentLanguageName = value;
                    CodeCompletionParserController.CurrentTwoLetterISO = PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO;
                }
                if ((value = (string)Options[OptionsItemNameMainFormLeft]) != null)
                {
                    val = Convert.ToInt32(value);
                    if (val < Screen.PrimaryScreen.WorkingArea.Size.Width && val >= 0)
                        FormLeft = this.Left = val;
                }
                if ((value = (string)Options[OptionsItemNameMainFormTop]) != null)
                {
                    val = Convert.ToInt32(value);
                    if (val < Screen.PrimaryScreen.WorkingArea.Size.Height && val >= 0)
                        FormTop = this.Top = val;
                }
                if ((value = (string)Options[OptionsItemNameMainFormWidth]) != null)
                {
                    val = Convert.ToInt32(value);
                    if (val < Screen.PrimaryScreen.WorkingArea.Size.Width && val >= 100)
                        FormWidth = this.Width = val;
                }
                if ((value = (string)Options[OptionsItemNameMainFormHeight]) != null)
                {
                    val = Convert.ToInt32(value);
                    if (val < Screen.PrimaryScreen.WorkingArea.Size.Height && val >= 100)
                        FormHeight = this.Height = val;
                }
                if ((value = (string)Options[OptionsItemNameMainFormMaximized]) != null)
                    _mainFormWindowStateMaximized = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameShowLinesNum]) != null)
                    UserOptions.ShowLineNums = Convert.ToBoolean(value);

                if ((value = (string)Options[OptionsItemNameEnableFolding]) != null)
                    UserOptions.EnableFolding = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameSaveSourceFilesIfComilationOk]) != null)
                    UserOptions.SaveSourceFilesIfComilationOk = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameAutoInsertCode]) != null)
                {
                    UserOptions.AutoInsertCodeIsEnabledOnStartup = Convert.ToBoolean(value);
                    tsAutoInsertCode.Checked = UserOptions.AutoInsertCodeIsEnabledOnStartup;
                    mAUTOINSERTToolStripMenuItem.Checked = UserOptions.AutoInsertCodeIsEnabledOnStartup;
                }

                if ((value = (string)Options[OptionsItemNameMainFormTitle]) != null)
                    MainFormText = value;
                if ((value = (string)Options[OptionsItemNameDeleteEXEAfterExecute]) != null)
                    UserOptions.DeleteEXEAfterExecute = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameDeletePDBAfterExecute]) != null)
                    UserOptions.DeletePDBAfterExecute = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameShowMatchBracket]) != null)
                    UserOptions.ShowMatchBracket = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameShowMatchOperatorBracket]) != null)
                    UserOptions.HighlightOperatorBrackets = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameEditorFontFamily]) != null)
                    UserOptions.CurrentFontFamily = Convert.ToString(value);
                if ((value = (string)Options[OptionsItemNameEditorFontSize]) != null)
                    UserOptions.EditorFontSize = Convert.ToInt32(value);
                if ((value = (string)Options[OptionsItemNameErrorsStrategy]) != null)
                {
                    ErrorsManager.Strategy = (PascalABCCompiler.Errors.ErrorsStrategy)Convert.ToByte(value);
                    if (ErrorsManager.Strategy == PascalABCCompiler.Errors.ErrorsStrategy.All)
                        ErrorsManager.Strategy = PascalABCCompiler.Errors.ErrorsStrategy.FirstSemanticAndSyntax;
                }
                /*if ((value = (string)Options[OptionsItemNameErrorsCursorPosStrategy]) != null)
                {
                    ErrorCursorPosStrategy = (VisualPascalABCPlugins.SourceLocationAction)Convert.ToByte(value);
                    if (ErrorCursorPosStrategy == VisualPascalABCPlugins.SourceLocationAction.GotoEnd)
                    	ErrorCursorPosStrategy = VisualPascalABCPlugins.SourceLocationAction.GotoBeg;
                    else if (ErrorCursorPosStrategy == VisualPascalABCPlugins.SourceLocationAction.SelectAndGotoEnd)
                    	ErrorCursorPosStrategy = VisualPascalABCPlugins.SourceLocationAction.SelectAndGotoBeg;
                }*/
                if ((value = (string)Options[OptionsItemNameShowOutputWindow]) != null)
                    BottomTabsVisible = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameRedirectConsoleIO]) != null)
                    UserOptions.RedirectConsoleIO = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameConvertTabsToSpaces]) != null)
                    UserOptions.ConverTabsToSpaces = true;// Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameGenerateDebugInfo]) != null)
                    WorkbenchServiceFactory.BuildService.CompilerOptions.Debug = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameTabIdent]) != null)
                    UserOptions.TabIndent = Convert.ToInt32(value);
                if ((value = (string)Options[OptionsItemNameOutputDirectory]) != null)
                    UserOptions.OutputDirectory = value;
                if ((value = (string)Options[OptionsItemNameUseOutputDirectory]) != null)
                    UserOptions.UseOutputDirectory = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNamePlayPauseButtonsVisibleInPanel]) != null)
                    PlayPauseButtonsVisibleInPanel = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNamePauseInRunModeIfConsole]) != null)
                    UserOptions.PauseInRunModeIfConsole = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameCodeCompletionHint]) != null)
                    UserOptions.CodeCompletionHint = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameCodeCompletionDot]) != null)
                    UserOptions.CodeCompletionDot = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameCodeCompletionParams]) != null)
                    UserOptions.CodeCompletionParams = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameCodeCompletionKeyPressed]) != null)
                    UserOptions.EnableSmartIntellisense = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameShowQuickClassBrowserPanel]) != null)
                    UserOptions.ShowQuickClassBrowserPanel = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameDefaultSourceFileNameFormat]) != null)
                    UserOptions.DefaultSourceFileNameFormat = value;
                if ((value = (string)Options[OptionsItemNameAllowCodeCompletion]) != null)
                    UserOptions.AllowCodeCompletion = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameSkipStackTraceItemIfSourceFileInSystemDirectory]) != null)
                    UserOptions.SkipStackTraceItemIfSourceFileInSystemDirectory = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameCodeCompletionNamespaceVisibleRange]) != null)
                    UserOptions.CodeCompletionNamespaceVisibleRange = Convert.ToInt32(value);
                if ((value = (string)Options[OptionsItemNameDockBottomPortion]) != null)
                    MainDockPanel.DockBottomPortion = Convert.ToDouble(value);
                if ((value = (string)Options[OptionsItemNameDockLeftPortion]) != null)
                    MainDockPanel.DockLeftPortion = Convert.ToDouble(value);
                if ((value = (string)Options[OptionsItemNameDockRightPortion]) != null)
                    MainDockPanel.DockRightPortion = Convert.ToDouble(value);
                if ((value = (string)Options[OptionsItemUseDllForSystemModules]) != null)
                    UserOptions.UseDllForSystemUnits = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemPABCDllChecked]) != null)
                    UserOptions.PABCDllChecked = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameUseSemanticIntellisense]) != null)
                {
                    UserOptions.UseSemanticIntellisense = Convert.ToBoolean(value);
                    CodeCompletion.DomSyntaxTreeVisitor.use_semantic_for_intellisense = UserOptions.UseSemanticIntellisense;
                }
                    
                int i = 0;
                while ((value = (string)Options[OptionsItemNameLastFile + (i++).ToString()]) != null)
                    AddLastFile(value);
                i = 0;
                while ((value = (string)Options[OptionsItemNameLastProject + (i++).ToString()]) != null)
                    WorkbenchServiceFactory.ProjectService.AddLastProject(value);
                /*foreach (string w in adv_opt.watch_list)
                {
                	AddVariable(w,false);
                }*/
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO);
            }
            catch (Exception e)
            {
            }
            UserOptions.AllowCodeCompletion = UserOptions.CodeCompletionHint || UserOptions.CodeCompletionDot || UserOptions.CodeCompletionParams;
        }

        public void SaveOptions()
        {
            string FileName = OptionsFileName;
            Hashtable Options = new Hashtable(StringComparer.CurrentCultureIgnoreCase);

            Options.Add(OptionsItemNameWorkDirectory, WorkbenchStorage.WorkingDirectoryInOptionsFile);
            Options.Add(OptionsItemNameLanguage, PascalABCCompiler.StringResourcesLanguage.CurrentLanguageName);
            Options.Add(OptionsItemNameMainFormLeft, FormLeft);
            Options.Add(OptionsItemNameMainFormTop, FormTop);
            Options.Add(OptionsItemNameMainFormHeight, FormHeight);
            Options.Add(OptionsItemNameMainFormWidth, FormWidth);
            Options.Add(OptionsItemNameMainFormMaximized, this.WindowState == FormWindowState.Maximized);
            Options.Add(OptionsItemNameShowOutputWindow, CurrentWebBrowserControl != null?true:BottomTabsVisible);
            Options.Add(OptionsItemNameErrorsStrategy, (byte)ErrorsManager.Strategy);
            //Options.Add(OptionsItemNameErrorsCursorPosStrategy, (byte)ErrorCursorPosStrategy);
            Options.Add(OptionsItemNameGenerateDebugInfo, WorkbenchServiceFactory.BuildService.CompilerOptions.Debug);
            Options.Add(OptionsItemNameDeleteEXEAfterExecute, UserOptions.DeleteEXEAfterExecute);
            Options.Add(OptionsItemNameDeletePDBAfterExecute, UserOptions.DeletePDBAfterExecute);
            Options.Add(OptionsItemNameOutputDirectory, UserOptions.OutputDirectory);
            Options.Add(OptionsItemNameUseOutputDirectory, UserOptions.UseOutputDirectory);
            Options.Add(OptionsItemNameDefaultSourceFileNameFormat, UserOptions.DefaultSourceFileNameFormat);
            Options.Add(OptionsItemNamePlayPauseButtonsVisibleInPanel, PlayPauseButtonsVisibleInPanel);
            Options.Add(OptionsItemNamePauseInRunModeIfConsole, UserOptions.PauseInRunModeIfConsole);
            Options.Add(OptionsItemNameAllowCodeCompletion, UserOptions.AllowCodeCompletion);
            Options.Add(OptionsItemNameCodeCompletionDot, UserOptions.CodeCompletionDot);
            Options.Add(OptionsItemNameCodeCompletionHint, UserOptions.CodeCompletionHint);
            Options.Add(OptionsItemNameCodeCompletionParams, UserOptions.CodeCompletionParams);
            Options.Add(OptionsItemNameCodeCompletionKeyPressed, UserOptions.EnableSmartIntellisense);
            Options.Add(OptionsItemNameCodeCompletionNamespaceVisibleRange, UserOptions.CodeCompletionNamespaceVisibleRange);
            Options.Add(OptionsItemNameSaveSourceFilesIfComilationOk, UserOptions.SaveSourceFilesIfComilationOk);
            Options.Add(OptionsItemNameShowQuickClassBrowserPanel, UserOptions.ShowQuickClassBrowserPanel);
            Options.Add(OptionsItemNameUseSemanticIntellisense, UserOptions.UseSemanticIntellisense);
            Options.Add(OptionsItemNameSkipStackTraceItemIfSourceFileInSystemDirectory, UserOptions.SkipStackTraceItemIfSourceFileInSystemDirectory);
            Options.Add(OptionsItemNameAutoInsertCode, UserOptions.AutoInsertCodeIsEnabledOnStartup);

            Options.Add(OptionsItemNameConvertTabsToSpaces, UserOptions.ConverTabsToSpaces);
            Options.Add(OptionsItemNameTabIdent, UserOptions.TabIndent);
            Options.Add(OptionsItemNameShowLinesNum, UserOptions.ShowLineNums);
            Options.Add(OptionsItemNameEnableFolding, UserOptions.EnableFolding); // SSM 4.09.08
            Options.Add(OptionsItemNameShowMatchBracket, UserOptions.ShowMatchBracket);
            Options.Add(OptionsItemNameShowMatchOperatorBracket, UserOptions.HighlightOperatorBrackets);
            Options.Add(OptionsItemNameEditorFontFamily, UserOptions.CurrentFontFamily);
            Options.Add(OptionsItemNameEditorFontSize, UserOptions.EditorFontSize);
            Options.Add(OptionsItemNameRedirectConsoleIO, UserOptions.RedirectConsoleIO);
            Options.Add(OptionsItemNameDockBottomPortion, MainDockPanel.DockBottomPortion);
            Options.Add(OptionsItemNameDockLeftPortion, MainDockPanel.DockLeftPortion);
            Options.Add(OptionsItemNameDockRightPortion, MainDockPanel.DockRightPortion);
            Options.Add(OptionsItemUseDllForSystemModules, UserOptions.UseDllForSystemUnits);
            Options.Add(OptionsItemPABCDllChecked, UserOptions.PABCDllChecked);
            if (MainFormText != MainFormTitle)
                Options.Add(OptionsItemNameMainFormTitle, MainFormText);
            for (int i = 0; i < LastOpenFiles.Count; i++)
                Options.Add(OptionsItemNameLastFile + (LastOpenFiles.Count - i - 1).ToString(), LastOpenFiles[i]);
            for (int i = 0; i < LastOpenProjects.Count; i++)
                Options.Add(OptionsItemNameLastProject + (LastOpenProjects.Count - i - 1).ToString(), LastOpenProjects[i]);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO);
            try
            {
                PascalABCCompiler.StringResources.WriteStringsToStreamAsXml(new StreamWriter(FileName, false, /*VisualEnvironmentCompiler.DefaultFileEncoding*/System.Text.Encoding.UTF8), Options, GetAdvancedOptions());
            }
            catch (Exception)
            {
                //ãàñèì èñêëþ÷åíèÿ åñëè íå óäàëîñü çàïèñàòü ôàéë. ýòî íå ñìåðòåëüíî
            }
        }

        public void UpdateUserOptions()
        {
            foreach (CodeFileDocumentControl si in OpenDocuments.Values)
            {
                if (si.TextEditor.ShowLineNumbers != UserOptions.ShowLineNums)
                    si.TextEditor.ShowLineNumbers = UserOptions.ShowLineNums;
                if (si.TextEditor.EnableFolding != UserOptions.EnableFolding) // SSM 4.09.08
                {
                    si.TextEditor.EnableFolding = UserOptions.EnableFolding;
                    si.TextEditor.UpdateFolding();
                }
                if (si.TextEditor.ShowMatchingBracket != UserOptions.ShowMatchBracket)
                    si.TextEditor.ShowMatchingBracket = UserOptions.ShowMatchBracket;
                if (si.FontSize != UserOptions.EditorFontSize)
                    si.FontSize = UserOptions.EditorFontSize;
                if (si.TextEditor.ConvertTabsToSpaces != UserOptions.ConverTabsToSpaces)
                    si.TextEditor.ConvertTabsToSpaces = UserOptions.ConverTabsToSpaces;
                si.TextEditor.ConvertTabsToSpaces = true;
                if (si.TextEditor.TabIndent != UserOptions.TabIndent)
                {
                    si.TextEditor.TabIndent = UserOptions.TabIndent;
                    si.TextEditor.TextEditorProperties.IndentationSize = UserOptions.TabIndent;
                }
                if (si.TextEditor.quickClassBrowserPanel.Visible != UserOptions.ShowQuickClassBrowserPanel)
                    si.TextEditor.quickClassBrowserPanel.Visible = UserOptions.ShowQuickClassBrowserPanel;
            }

            foreach (var rtb in this.OutputTextBoxs.Values)
            {
                rtb.Font = OpenDocuments.Values.First().TextEditor.Font;
            }

            tsViewIntellisensePanel.Checked = UserOptions.ShowQuickClassBrowserPanel;
            tsViewIntellisensePanel.Visible = tssmIntellisence.Visible = tsGotoDefinition.Visible = tsGotoRealization.Visible =
                tsFindAllReferences.Visible = miGenerateRealization.Visible =
                miGenerateRealization.Visible = cmGenerateRealization.Visible =
                cmsCodeCompletion.Visible = cmFindAllReferences.Visible = cmGotoDefinition.Visible =
                cmGotoRealization.Visible = UserOptions.AllowCodeCompletion;

        }

        private PascalABCCompiler.AdvancedOptions GetAdvancedOptions()
        {
            PascalABCCompiler.AdvancedOptions adv_opt = new PascalABCCompiler.AdvancedOptions();
            return adv_opt;
        }
        
    }
}
