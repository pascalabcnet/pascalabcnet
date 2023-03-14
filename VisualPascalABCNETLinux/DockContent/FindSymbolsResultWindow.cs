// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Threading;

namespace VisualPascalABC
{
    public partial class FindSymbolsResultWindowForm : BottomDockContentForm
    {
        public SymbolsViwer FindSymbolResults;
       
        public FindSymbolsResultWindowForm(Form1 MainForm)
            :base(MainForm)
        {
            InitializeComponent();
            MainForm.OnEnvorimentEvent += new Form1.EnvorimentEventDelegate(MainForm_OnEnvorimentEvent);
        }

        void MainForm_OnEnvorimentEvent(Form1.EnvorimentEvent EnvorimentEvent)
        {
            switch (EnvorimentEvent)
            {
                case Form1.EnvorimentEvent.VisualEnvironmentCompilerCreated:
                    FindSymbolResults = new SymbolsViwer(lvFindSymbolResults, CodeCompletionProvider.ImagesProvider.ImageList, true, BeginInvoke, MainForm.VisualEnvironmentCompiler.SourceFilesProvider, MainForm.ExecuteSourceLocationAction);
                    break;
            }
        }

        internal void Resized()
        {
            lvFindSymbolResults.Columns[1].Width = lvFindSymbolResults.ClientSize.Width - lvFindSymbolResults.Columns[0].Width - 20;
        }
        private void lvFindSymbolResults_Resize(object sender, EventArgs e)
        {
            Resized();
            
        }
        ICSharpCode.TextEditor.TextArea __textarea;
        int __syms_count;
        private delegate void proc();
        private void __gaFindReferencesEnd()
        {
            MainForm.BottomTabsVisible = true;
            MainForm.SelectContent(this, true);
            MainForm.FindAllReferncesEnabled = true;
            if (__syms_count > 0)
                MainForm.SetStateText(string.Format(PascalABCCompiler.StringResources.Get("VP_VEC_STATETEXT_FINDREFERNCES_RESULT{0}"), __syms_count));
            else
                MainForm.SetStateText(PascalABCCompiler.StringResources.Get("VP_VEC_STATETEXT_NOTFOUND"));
        }
        private void __gaFindReferences(object state)
        {
            //GotoAction ga = new GotoAction();
            List<SymbolsViewerSymbol> syms = CodeCompletionActionsManager.FindReferences(__textarea);
            __syms_count = syms.Count;
            FindSymbolResults.Show(syms);
            BeginInvoke(new proc(__gaFindReferencesEnd));
        }

        public void ShowFindResults(List<SymbolsViewerSymbol> syms)
        {
            MainForm.BottomTabsVisible = true;
            MainForm.SelectContent(this, true);
            FindSymbolResults.Show(syms);
        }
        public void FindAllReferences()
        {
            if (!Visible)
            {
            	if (!MainForm.BottomTabsVisible)
            		MainForm.BottomTabsVisible = true;
            	MainForm.ShowContent(this, false);
            }
            MainForm.FindAllReferncesEnabled = false;
            MainForm.SetStateText(PascalABCCompiler.StringResources.Get("VP_VEC_STATETEXT_FINDREFERNCES"));
            lvFindSymbolResults.Items.Clear();
            __textarea = MainForm.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea;
            if (!ThreadPool.QueueUserWorkItem(__gaFindReferences))
                __gaFindReferences(null);
        }
    }
}
