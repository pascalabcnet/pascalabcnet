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

namespace VisualPascalABC.OptionsContent
{
    public partial class EditorOptionsContent : UserControl,IOptionsContent
    {
        Form1 MainForm;
        string strprefix = "VP_OC_EDITOROPTIONS_";
        public EditorOptionsContent(Form1 MainForm)
        {
            this.MainForm = MainForm;
            InitializeComponent();
            PascalABCCompiler.StringResources.SetTextForAllObjects(this, strprefix);
            cbEditorFontSize.Items.Add(8);
            cbEditorFontSize.Items.Add(10);
            cbEditorFontSize.Items.Add(12);
            cbEditorFontSize.Items.Add(14);
            cbEditorFontSize.Items.Add(16);
            cbEditorFontSize.Items.Add(18);
            cbEditorFontSize.Items.Add(20);
            cbEditorFontSize.Items.Add(22);
            cbEditorFontSize.Items.Add(24);
            cbEditorFontSize.Items.Add(28);
            cbEditorFontSize.Items.Add(32);

        }
        /*private void AddcbErrorPosItem(SourceLocationAction sl, string Name)
        {
            ObjectLocalisator ol = new ObjectLocalisator(sl, Name);
            cbErrorPos.Items.Add(ol);
            if (MainForm.ErrorCursorPosStrategy == sl)
                cbErrorPos.SelectedItem = ol;
        }*/

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
                        cbShowLinesNum.Checked = MainForm.UserOptions.ShowLineNums;
                        cbEnableFolding.Checked = MainForm.UserOptions.EnableFolding; // SSM 4.09.08
                        cbShowMatchBracket.Checked = MainForm.UserOptions.ShowMatchBracket;
                        cbConvertTabsToSpaces.Checked = MainForm.UserOptions.ConverTabsToSpaces;
                        cbEnableMatchOperBrackets.Checked = MainForm.UserOptions.HighlightOperatorBrackets;
                        nudTabIndent.Value = MainForm.UserOptions.TabIndent;
                        cbSkipStakTraceItemIfSourceFileInSystemDirectory.Checked = !MainForm.UserOptions.SkipStackTraceItemIfSourceFileInSystemDirectory;
                        //cbErrorPos.Items.Clear();
                        //AddcbErrorPosItem(SourceLocationAction.GotoBeg, strprefix + "EP_BEGIN");
                        //AddcbErrorPosItem(SourceLocationAction.GotoEnd, strprefix + "EP_END");
                        //AddcbErrorPosItem(SourceLocationAction.SelectAndGotoBeg, strprefix + "EP_SELECTBEGIN");
                        //AddcbErrorPosItem(SourceLocationAction.SelectAndGotoEnd, strprefix + "EP_SELECTEND");
                        cbEditorFontSize.SelectedItem = null;
                        foreach (int size in cbEditorFontSize.Items)
                            if (size == MainForm.UserOptions.EditorFontSize)
                            {
                                cbEditorFontSize.SelectedItem = size;
                                break;
                            }
                        if (cbEditorFontSize.SelectedItem == null)
                        {
                            cbEditorFontSize.Items.Add(MainForm.UserOptions.EditorFontSize);
                            cbEditorFontSize.SelectedItem = MainForm.UserOptions.EditorFontSize;
                        }
                        /*if (MainForm.UserOptions.CurrentFontFamily != null)
                        {
                            MainForm.CurrentSyntaxEditor.TextEditor.Font = new Font(new FontFamily(MainForm.UserOptions.CurrentFontFamily),MainForm.CurrentSyntaxEditor.TextEditor.Font.Size);
                        }*/
                        fcbFont.Populate(false, MainForm.CurrentSyntaxEditor.TextEditor.Font);
                        alreadyShown = true;
                    }
                    break;

                case OptionsContentAction.Ok:
                    MainForm.UserOptions.ConverTabsToSpaces = cbConvertTabsToSpaces.Checked;
                    MainForm.UserOptions.TabIndent = (int)nudTabIndent.Value;
                    MainForm.UserOptions.ShowLineNums = cbShowLinesNum.Checked;
                    MainForm.UserOptions.EnableFolding = cbEnableFolding.Checked; // SSM 4.09.08
                    MainForm.UserOptions.ShowMatchBracket = cbShowMatchBracket.Checked;
                    MainForm.UserOptions.HighlightOperatorBrackets = cbEnableMatchOperBrackets.Checked;
                    MainForm.ErrorCursorPosStrategy = SourceLocationAction.GotoBeg;//(SourceLocationAction)(cbErrorPos.SelectedItem as ObjectLocalisator).Value;
                    if (fcbFont.SelectedItem != null)
                    {
                    	MainForm.CurrentSyntaxEditor.TextEditor.Font = new Font(new FontFamily((string)fcbFont.SelectedItem),MainForm.CurrentSyntaxEditor.TextEditor.Font.Size);
                    	MainForm.UserOptions.CurrentFontFamily = fcbFont.SelectedItem as string;
                    }
                    MainForm.UserOptions.EditorFontSize = Convert.ToInt32(cbEditorFontSize.Text);
                    MainForm.UserOptions.SkipStackTraceItemIfSourceFileInSystemDirectory = !cbSkipStakTraceItemIfSourceFileInSystemDirectory.Checked;
                    MainForm.UpdateUserOptions();
                    WorkbenchServiceFactory.OptionsService.SaveOptions();
                    alreadyShown = false;
                    break;
                case OptionsContentAction.Cancel:
                    alreadyShown = false;
                    break;
            }

        }
        #endregion

        private void cbErrorPos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        void EditorOptionsContentPaint(object sender, PaintEventArgs e)
        {
        	
        }
        
        void EditorOptionsContentLoad(object sender, EventArgs e)
        {
        	
        }
        
        void FcbFontSelectedIndexChanged(object sender, EventArgs e)
        {
        	
        }
    }
}
