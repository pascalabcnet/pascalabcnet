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

namespace VisualPascalABC.OptionsContent
{
    public partial class CompilerOptionsContent : UserControl,IOptionsContent
    {
        Form1 MainForm;
        string strprefix = "VP_OC_COMPILEROPTIONS_";
        public CompilerOptionsContent(Form1 MainForm)
        {
            this.MainForm = MainForm;
            InitializeComponent();
            PascalABCCompiler.StringResources.SetTextForAllObjects(this, strprefix);
            //this.cbUseDllForSystemUnits.Visible = false;
        }

        #region IOptionsContent Members

        private bool alreadyShown;

        public string ContentName
        {
            get
            {
                return PascalABCCompiler.StringResources.Get(strprefix+"NAME");
            }
        }
        public string Description
        {
            get
            {
                return PascalABCCompiler.StringResources.Get(strprefix+"DESCRIPTION");
            }
        }

        public UserControl Content
        {
            get { return this; }
        }

        public void Action(OptionsContentAction action)
        {
            PascalABCCompiler.CompilerOptions CompOpt = WorkbenchServiceFactory.BuildService.CompilerOptions;
            UserOptions opt = MainForm.UserOptions;

            switch (action)
            {
                case OptionsContentAction.Show:
                    //comboBox1.SelectedItem = comboBox1.Items[0];
                    if (!alreadyShown)
                    {
                        comboBox1.Items.Clear();
                        comboBox1.Items.Add(PascalABCCompiler.StringResources.Get("VP_MF_AT_CONSOLE"));
                        comboBox1.Items.Add(PascalABCCompiler.StringResources.Get("VP_MF_AT_WINDOWS"));
                        comboBox1.Items.Add(PascalABCCompiler.StringResources.Get("VP_MF_AT_DLL"));
                        switch (CompOpt.OutputFileType)
                        {
                            case PascalABCCompiler.CompilerOptions.OutputType.ConsoleApplicaton:
                                comboBox1.SelectedItem = comboBox1.Items[0];
                                break;
                            case PascalABCCompiler.CompilerOptions.OutputType.WindowsApplication:
                                comboBox1.SelectedItem = comboBox1.Items[1];
                                break;
                            case PascalABCCompiler.CompilerOptions.OutputType.ClassLibrary:
                                comboBox1.SelectedItem = comboBox1.Items[2];
                                break;
                        }
                        checkBox1.Checked = CompOpt.Debug;
                        cbDeleteExe.Checked = opt.DeleteEXEAfterExecute;
                        cbDeletePdb.Checked = opt.DeletePDBAfterExecute;
                        cbUseOutputDirectory.Checked = opt.UseOutputDirectory;
                        cbUseOutputDirectory_CheckedChanged(null, null);
                        tbOutputDirectory.Text = opt.OutputDirectory;
                        cbUseDllForSystemUnits.Checked = opt.UseDllForSystemUnits;
                        alreadyShown = true;
                    }
                    break;

                case OptionsContentAction.Ok:
                    /*switch (comboBox1.Items.IndexOf(comboBox1.SelectedItem))
                    {
                        case 0:
                            CompOpt.OutputFileType = PascalABCCompiler.CompilerOptions.OutputType.ConsoleApplicaton;
                            break;
                        case 1:
                            CompOpt.OutputFileType = PascalABCCompiler.CompilerOptions.OutputType.WindowsApplication;
                            break;
                        case 2:
                            CompOpt.OutputFileType = PascalABCCompiler.CompilerOptions.OutputType.ClassLibrary;
                            break;
                    }*/
                    CompOpt.Debug = checkBox1.Checked;
                    MainForm.SelectAppType(CompOpt.OutputFileType);
                    opt.DeleteEXEAfterExecute = cbDeleteExe.Checked;
                    opt.DeletePDBAfterExecute = cbDeletePdb.Checked;
                    if (!Directory.Exists(tbOutputDirectory.Text))
                    {
                        try
                        {
                            Directory.CreateDirectory(tbOutputDirectory.Text);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, PascalABCCompiler.StringResources.Get("!ERROR_ON_CREATE_DIRECTORY"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ActiveControl = tbOutputDirectory;
                            return;
                        }
                    }
                    opt.UseOutputDirectory = cbUseOutputDirectory.Checked;
                    opt.OutputDirectory = tbOutputDirectory.Text;
                    opt.UseDllForSystemUnits = cbUseDllForSystemUnits.Checked;
                    if (opt.UseOutputDirectory)
                        WorkbenchStorage.StandartDirectories[Constants.OutputDirectoryIdent] = opt.OutputDirectory;
                    else
                        WorkbenchStorage.StandartDirectories[Constants.OutputDirectoryIdent] = null;
                    alreadyShown = false;
                    WorkbenchServiceFactory.OptionsService.SaveOptions();
                    break;
                case OptionsContentAction.Cancel:
                    alreadyShown = false;
                    break;
            }
          

        }
        #endregion

        private void btSelectOutpotDirectory_Click(object sender, EventArgs e)
        {
            if (fbSelectOutputDirectory.ShowDialog() == DialogResult.OK)
                tbOutputDirectory.Text = fbSelectOutputDirectory.SelectedPath;

        }

        private void cbUseOutputDirectory_CheckedChanged(object sender, EventArgs e)
        {
            tbOutputDirectory.Enabled = btSelectOutpotDirectory.Enabled = cbUseOutputDirectory.Checked;
        }

 
    }
}
