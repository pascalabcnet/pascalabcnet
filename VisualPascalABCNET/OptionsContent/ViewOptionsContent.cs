// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using VisualPascalABCPlugins;
using PascalABCCompiler.Errors;

namespace VisualPascalABC.OptionsContent
{
    public partial class ViewOptionsContent : UserControl,IOptionsContent
    {
        Form1 MainForm;
        string strprefix = "VP_OC_VIEWOPTIONS_";
        public ViewOptionsContent(Form1 MainForm)
        {
            this.MainForm = MainForm;
            InitializeComponent();
            PascalABCCompiler.StringResources.SetTextForAllObjects(this, strprefix);
            foreach (string lng in PascalABCCompiler.StringResourcesLanguage.AccessibleLanguages)
            {
                languageSelect.Items.Add(lng);
            }
            if (PascalABCCompiler.StringResourcesLanguage.AccessibleLanguages.Count == 0)
                languageSelect.Enabled = false;
        }

        private bool alreadyShown;
        #region IOptionsContent Members

        public string ContentName
        {
            get
            {
                return PascalABCCompiler.StringResources.Get(strprefix + "NAME");
            }
        }
        public string Description
        {
            get
            {
                return PascalABCCompiler.StringResources.Get(strprefix + "DESCRIPTION");
            }
        }

        public UserControl Content
        {
            get { return this; }
        }

        public void Action(OptionsContentAction action)
        {
            switch (action)
            {
                case OptionsContentAction.Show:
                    if (!alreadyShown)
                    {
                        languageSelect.SelectedItem = PascalABCCompiler.StringResourcesLanguage.CurrentLanguageName;
                        cbSaveFilesIfComilationOk.Checked = MainForm.UserOptions.SaveSourceFilesIfComilationOk;
                        cbPauseInRunModeIfConsole.Checked = MainForm.UserOptions.PauseInRunModeIfConsole;
                        cbAutoInsertCodeIsEnabledOnStartup.Checked = MainForm.UserOptions.AutoInsertCodeIsEnabledOnStartup;
                        addErrorStrategyComboBox();

                        cbShowDebugPlayPauseButtons.Checked = MainForm.PlayPauseButtonsVisibleInPanel;
                        alreadyShown = true;
                    }
                    break;
                case OptionsContentAction.Ok:
                    UserOptions UsOpt = MainForm.UserOptions;
                    UsOpt.SaveSourceFilesIfComilationOk = cbSaveFilesIfComilationOk.Checked;
                    UsOpt.AutoInsertCodeIsEnabledOnStartup = cbAutoInsertCodeIsEnabledOnStartup.Checked;
                    MainForm.ErrorsManager.Strategy = (ErrorsStrategy)cbErrorsStrategy.Items.IndexOf(cbErrorsStrategy.SelectedItem);
                    switch (cbErrorsStrategy.Items.IndexOf(cbErrorsStrategy.SelectedItem))
                    {
                    	case 0:
                    		MainForm.ErrorsManager.Strategy = ErrorsStrategy.FirstOnly;
                            break;
                        case 1:
                           	MainForm.ErrorsManager.Strategy = ErrorsStrategy.FirstSemanticAndSyntax;
                            break; 
                    }
                    /*switch (cbErrorsStrategy.Items.IndexOf(cbErrorsStrategy.SelectedItem))
                    {
                        case 0:
                            MainForm.ErrorsManager.Strategy = ErrorsStrategy.All;
                            break;
                        case 1:
                            MainForm.ErrorsManager.Strategy = ErrorsStrategy.FirstOnly;
                            break;
                        case 2:
                            MainForm.ErrorsManager.Strategy = ErrorsStrategy.FirstSemanticAndSyntax;
                            break;
                    }            
                    */
                    UsOpt.PauseInRunModeIfConsole = cbPauseInRunModeIfConsole.Checked;
                    MainForm.UpdateUserOptions();
                    MainForm.PlayPauseButtonsVisibleInPanel = cbShowDebugPlayPauseButtons.Checked;
                    alreadyShown = false;
                    WorkbenchServiceFactory.OptionsService.SaveOptions();
                    //this.Enabled = true;           
                    break;
                case OptionsContentAction.Cancel:
                    alreadyShown = false;
                    break;
            }

        }
        #endregion

        private void addErrorStrategyComboBox()
        {
            cbErrorsStrategy.Items.Clear();
            //cbErrorsStrategy.Items.Add(PascalABCCompiler.StringResources.Get(strprefix + "ES_ALL"));
            cbErrorsStrategy.Items.Add(PascalABCCompiler.StringResources.Get(strprefix + "ES_FIRSTONLY"));
            cbErrorsStrategy.Items.Add(PascalABCCompiler.StringResources.Get(strprefix + "ES_FIRSTSEMANTICANDSYNTAX"));
            switch (MainForm.ErrorsManager.Strategy)
            {
                /*case ErrorsStrategy.All:
                    cbErrorsStrategy.SelectedItem = cbErrorsStrategy.Items[1];
                    break;*/
                case ErrorsStrategy.FirstOnly:
                    cbErrorsStrategy.SelectedItem = cbErrorsStrategy.Items[0];
                    break;
                case ErrorsStrategy.FirstSemanticAndSyntax:
                    cbErrorsStrategy.SelectedItem = cbErrorsStrategy.Items[1];
                    break;
                default:
                    cbErrorsStrategy.SelectedItem = cbErrorsStrategy.Items[1];
                    break;
            }
        }

        private void languageSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            PascalABCCompiler.StringResourcesLanguage.CurrentLanguageName = (string)languageSelect.SelectedItem;
            CodeCompletionParserController.CurrentTwoLetterISO = PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO;
            PascalABCCompiler.StringResources.SetTextForAllObjects(this, strprefix);
            addErrorStrategyComboBox();
            MainForm.UpdateOptionsForm();
        }

        private void cbShowDebugPlayPauseButtons_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.PlayPauseButtonsVisibleInPanel = cbShowDebugPlayPauseButtons.Checked;
        }

    }
}
