using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace VisualPascalABCPlugins
{
    public partial class CompilerInternalErrorReport : Form
    {
        public string ReportText;
        public List<string> FileNames = new List<string>();
        private string StringPrefix = InternalErrorReport_VisualPascalABCPlugin.StringsPrefix + "INTERNALERRORREPORT_";
        internal VisualPascalABCPlugins.IVisualEnvironmentCompiler VEC;

        public CompilerInternalErrorReport()
        {
            InitializeComponent();
            PascalABCCompiler.StringResources.SetTextForAllObjects(this, StringPrefix);
            this.ActiveControl = okButton;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CompilerInternalErrorReport_Shown(object sender, EventArgs e)
        {
            ReportTextBox.Text = PascalABCCompiler.StringResources.Get(StringPrefix+"ADD_YUOR_COMMENT_HERE") + Environment.NewLine + ReportText;
            string fn = "";
            if (FileNames.Count > 0)
                if (FileNames[0]!=null)
                    fn = "_"+Path.GetFileNameWithoutExtension(FileNames[0]);
            saveFileDialog.FileName = "PascalABCErrorReport"+fn+".zip";
            
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            ReportText = ReportTextBox.Text;
            ZipEntry ze;
            ZipOutputStream zip_out = new ZipOutputStream(File.Create((sender as SaveFileDialog).FileName));
            string SourceText;
            byte[] data=null;
            foreach (string FileName in FileNames)
            {
                SourceText = VEC.StandartCompiler.GetSourceFileText(FileName);
                if (SourceText != null)
                {
                    data = System.Text.Encoding.GetEncoding(1251).GetBytes(SourceText);
                    ze = new ZipEntry(System.IO.Path.GetFileName(FileName));
                    zip_out.PutNextEntry(ze);
                    zip_out.Write(data, 0, data.Length);
                }
            }
            ze = new ZipEntry("Report.txt");
            zip_out.PutNextEntry(ze);
            data = System.Text.Encoding.GetEncoding(1251).GetBytes(ReportText); 
            zip_out.Write(data, 0, data.Length);
           	zip_out.Finish();
			zip_out.Close();
        }

        public delegate void ShowDialogDel();

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:" + ((Label)sender).Text+ "?subject=Внутренняя ошибка компилятора&cc=miks@math.rsu.ru");
        }

        private void CompilerInternalErrorReport_Load(object sender, EventArgs e)
        {

        }
    }
}

