// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace VisualPascalABC
{
	/// <summary>
	/// Description of ProjectProperties.
	/// </summary>
	public partial class ProjectProperties : Form
	{
		public ProjectProperties()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
            openFileDialog1.Filter = VECStringResources.Get("DIALOGS_FILTER_ICO_FILES")+"|*.ico";
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

        private PascalABCCompiler.IProjectInfo proj;

        public void LoadOptions(PascalABCCompiler.IProjectInfo prj)
        {
            proj = prj;
            this.cbDeleteExe.Checked = prj.DeleteEXE;
            this.cbDeletePdb.Checked = prj.DeletePDB;
            if (prj.IncludeDebugInfo)
                this.cbDebugRelease.SelectedIndex = 0;
            else
                this.cbDebugRelease.SelectedIndex = 1;
            this.tbOutputDirectory.Text = prj.OutputDirectory;
            this.tbRunArguments.Text = prj.CommandLineArguments;
            if (!string.IsNullOrEmpty(prj.AppIcon))
            {
                this.tbAppIcon.Text = prepare_icon_name(prj.AppIcon);
            }
            this.tbMajor.Text = Convert.ToString(prj.MajorVersion);
            this.tbMinor.Text = Convert.ToString(prj.MinorVersion);
            this.tbBuild.Text = Convert.ToString(prj.BuildVersion);
            this.tbRevision.Text = Convert.ToString(prj.RevisionVersion);
            this.cbGenerateXmlDoc.Checked = prj.GenerateXMLDoc;
            this.tbProduct.Text = prj.Product;
            this.tbCompany.Text = prj.Company;
            this.tbTradeMark.Text = prj.Trademark;
            this.tbCopyright.Text = prj.Copyright;
            //if (prj.Title != null) { this.tbTitle.Text = prj.Description; } else { this.tbTitle.Text = ""; };
            //if (prj.Description != null) { this.tbDescription.Text = prj.Description; } else { this.tbDescription.Text = ""; };
        }

        public void SetOptions(PascalABCCompiler.IProjectInfo prj)
        {
            prj.DeleteEXE = this.cbDeleteExe.Checked;
            prj.DeletePDB = this.cbDeletePdb.Checked;
            prj.IncludeDebugInfo = this.cbDebugRelease.SelectedIndex == 0;
            prj.OutputDirectory = this.tbOutputDirectory.Text;
            prj.CommandLineArguments = this.tbRunArguments.Text;
            if (!string.IsNullOrEmpty(this.tbAppIcon.Text))
            {
                /*if (File.Exists(this.tbAppIcon.Text))
                    prj.AppIcon = this.tbAppIcon.Text;
                else*/
                    prj.AppIcon = Path.Combine(prj.ProjectDirectory, this.tbAppIcon.Text);
            }
            else
                prj.AppIcon = null;
            prj.MajorVersion = Convert.ToInt32(this.tbMajor.Text);
            prj.MinorVersion = Convert.ToInt32(this.tbMinor.Text);
            prj.BuildVersion = Convert.ToInt32(this.tbBuild.Text);
            prj.RevisionVersion = Convert.ToInt32(this.tbRevision.Text);
            prj.GenerateXMLDoc = this.cbGenerateXmlDoc.Checked;
            prj.Product = this.tbProduct.Text;
            prj.Company = this.tbCompany.Text;
            prj.Trademark = this.tbTradeMark.Text;
            prj.Copyright = this.tbCopyright.Text;
            //prj.Title = this.tbTitle.Text;
            //prj.Description = this.tbDescription.Text;
        }

        private void ProjectProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel)
                return;
            if (string.IsNullOrEmpty(this.tbMajor.Text))
            {
                e.Cancel = true;
                MessageBox.Show(Form1StringResources.Get("MAJOR_VERSION_MUST_BE_SET"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                int ver = Convert.ToInt32(this.tbMajor.Text);
                if (ver < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show(Form1StringResources.Get("MAJOR_VERSION_LESS_ZERO"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch
            {
                e.Cancel = true;
                MessageBox.Show(Form1StringResources.Get("MAJOR_VERSION_MUST_BE_INT"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(this.tbMinor.Text))
            {
                e.Cancel = true;
                MessageBox.Show(Form1StringResources.Get("MINOR_VERSION_MUST_BE_SET"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                int ver = Convert.ToInt32(this.tbMajor.Text);
                if (ver < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show(Form1StringResources.Get("MINOR_VERSION_LESS_ZERO"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch
            {
                e.Cancel = true;
                MessageBox.Show(Form1StringResources.Get("MINOR_VERSION_MUST_BE_INT"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(this.tbBuild.Text))
            {
                e.Cancel = true;
                MessageBox.Show(Form1StringResources.Get("BUILD_VERSION_MUST_BE_SET"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                int ver = Convert.ToInt32(this.tbBuild.Text);
                if (ver < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show(Form1StringResources.Get("BUILD_VERSION_LESS_ZERO"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch
            {
                e.Cancel = true;
                MessageBox.Show(Form1StringResources.Get("BUILD_VERSION_MUST_BE_INT"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(this.tbRevision.Text))
            {
                e.Cancel = true;
                MessageBox.Show(Form1StringResources.Get("REVISION_VERSION_MUST_BE_SET"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                int ver = Convert.ToInt32(this.tbRevision.Text);
                if (ver < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show(Form1StringResources.Get("REVISION_VERSION_LESS_ZERO"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch
            {
                e.Cancel = true;
                MessageBox.Show(Form1StringResources.Get("REVISION_VERSION_MUST_BE_INT"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!string.IsNullOrEmpty(tbAppIcon.Text))
                if (!File.Exists(tbAppIcon.Text) && !File.Exists(Path.Combine(proj.ProjectDirectory, tbAppIcon.Text)))
                {
                    e.Cancel = true;
                    MessageBox.Show(Form1StringResources.Get("ICON_FILE_NOT_FOUND"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
        }

        private Image ReadIconImage(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            Image tmp = Image.FromStream(fs);
            fs.Flush();
            fs.Close();
            Bitmap bmp = new Bitmap(tmp);
            tmp.Dispose();
            return bmp;
        }

        private string prepare_icon_name(string fileName)
        {
            try
            {
                pbAppIcon.Image = ReadIconImage(fileName);
            }
            catch
            {
            }
            return PascalABCCompiler.Tools.RelativePathTo(ProjectFactory.Instance.CurrentProject.ProjectDirectory, fileName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (Path.GetDirectoryName(openFileDialog1.FileName) != ProjectFactory.Instance.CurrentProject.ProjectDirectory)
                        File.Copy(openFileDialog1.FileName, Path.Combine(ProjectFactory.Instance.CurrentProject.ProjectDirectory,Path.GetFileName(openFileDialog1.FileName)),true);
                    pbAppIcon.Image = ReadIconImage(openFileDialog1.FileName);
                }
                catch
                {
                }
                this.tbAppIcon.Text = Path.GetFileName(openFileDialog1.FileName);//PascalABCCompiler.Tools.RelativePathTo(ProjectFactory.Instance.CurrentProject.ProjectDirectory, openFileDialog1.file_name);
            }
        }

        private void btSelectOutpotDirectory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbOutputDirectory.Text = folderBrowserDialog1.SelectedPath;
            }
        }
	}
}
