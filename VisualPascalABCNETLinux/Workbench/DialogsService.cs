// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VisualPascalABC
{
    public partial class Form1
    {
        private void OpenProjectDialog(string InitialDirectory)
        {
            openProjectDialog.FileName = "";
            openProjectDialog.InitialDirectory = InitialDirectory;
            openProjectDialog.ShowDialog();
            SetFocusToEditor();
        }

        private void openProjectDialog_FileOk(object sender, CancelEventArgs e)
        {
            WorkbenchServiceFactory.ProjectService.OpenProject(openProjectDialog.FileName);
        }

        private void OpenFileDialog(string InitialDirectory)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = InitialDirectory;
            // Activate documents only after Mono has released the modal dialog.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string FileName in openFileDialog1.FileNames)
                    WorkbenchServiceFactory.FileService.OpenFile(FileName, null);
            }
            SetFocusToEditor();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //ReplaceLastFile(CurrentSourceFileName, saveFileDialog1.file_name);
            string fn = Tools.FileNameToLower(saveFileDialog1.FileName);
            if (OpenDocuments.ContainsKey(fn) && OpenDocuments[fn] != CurrentCodeFileDocument)
            {
                MessageBox.Show(Form1StringResources.Get("ERROR_FILE_EXISTS_IN_CODEPAGES"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SaveFileAs(CurrentCodeFileDocument, saveFileDialog1.FileName);
                AddLastFile(saveFileDialog1.FileName);
            }
        }
    }
}
