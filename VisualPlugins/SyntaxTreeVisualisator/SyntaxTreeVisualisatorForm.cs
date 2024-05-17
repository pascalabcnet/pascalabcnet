using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PascalABCCompiler.Errors;
using System.IO;
using Languages.Facade;

namespace VisualPascalABCPlugins
{
    public partial class SyntaxTreeVisualisatorForm : Form
    {
        public IVisualEnvironmentCompiler VisualEnvironmentCompiler;
        private System.Collections.Hashtable Errors = new System.Collections.Hashtable();
        
        public SyntaxTreeVisualisatorForm()
        {
            InitializeComponent();
            PascalABCCompiler.StringResources.SetTextForAllObjects(this, SyntaxTreeVisualisator_VisualPascalABCPlugin.StringsPrefix);
        }

        private void SyntaxTreeVisualisatorForm_Load(object sender, EventArgs e)
        {

        }

        private void SyntaxTreeVisualisatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void SyntaxTreeVisualisatorForm_Shown(object sender, EventArgs e)
        {
            VisualEnvironmentCompiler.StandartCompiler.OnChangeCompilerState += new PascalABCCompiler.ChangeCompilerStateEventDelegate(Compiler_OnChangeCompilerState);
        }
        private class SyntaxTreeSelectComboBoxItem
        {
            public PascalABCCompiler.SyntaxTree.syntax_tree_node Node;
            public string FileName;
            public SyntaxTreeSelectComboBoxItem(PascalABCCompiler.SyntaxTree.syntax_tree_node Node, string FileName)
            {
                this.Node=Node;
                this.FileName=FileName;
            }
            public override string ToString()
            {
                return System.IO.Path.GetFileName(FileName);
            }
        }
        bool BuildButtonsEnabled
        {
            set
            {
                tbRebuild.Enabled = tbBuild.Enabled = value;
            }
            get
            {
                return tbRebuild.Enabled;
            }
        }
        void Compiler_OnChangeCompilerState(PascalABCCompiler.ICompiler sender, PascalABCCompiler.CompilerState State, string FileName)
        {
            if (!Visible) return;
            switch (State)
            {
                case PascalABCCompiler.CompilerState.CompilationStarting:
                    BuildButtonsEnabled = syntaxTreeSelectComboBox.Enabled = false;                
                    this.Refresh();
                    break;
                case PascalABCCompiler.CompilerState.CompilationFinished:
                    Errors.Clear();
                    if (VisualEnvironmentCompiler.Compiler.ErrorsList.Count > 0)
                    {
                        SyntaxError er;
                        for (int i = 0; i < VisualEnvironmentCompiler.Compiler.ErrorsList.Count; i++)
                        {
                            er = VisualEnvironmentCompiler.Compiler.ErrorsList[i] as SyntaxError;
                            if (er!=null && er.bad_node != null)
                                Errors[er.bad_node] = i;
                        }
                    }
                    UpdateSelectList();
                    ShowTree();
                    BuildButtonsEnabled = true;
                    break;
            }
        }
        private void UpdateSelectList()
        {
            SyntaxTreeSelectComboBoxItem lastItem = syntaxTreeSelectComboBox.SelectedItem as SyntaxTreeSelectComboBoxItem;
            syntaxTreeSelectComboBox.Items.Clear();
            foreach (PascalABCCompiler.CompilationUnit unit in VisualEnvironmentCompiler.Compiler.UnitTable.Values)
                if (unit.SyntaxTree != null)
                    syntaxTreeSelectComboBox.Items.Add(new SyntaxTreeSelectComboBoxItem(unit.SyntaxTree, unit.SyntaxTree.file_name));
            if (lastItem != null)
                foreach (SyntaxTreeSelectComboBoxItem item in syntaxTreeSelectComboBox.Items)
                    if (item.ToString() == lastItem.ToString())
                        syntaxTreeSelectComboBox.SelectedItem = item;
            if (syntaxTreeSelectComboBox.Items.Count > 0)
            {
                syntaxTreeSelectComboBox.Enabled = !showAllCheckBox.Checked; 
                if (syntaxTreeSelectComboBox.SelectedItem == null)
                    syntaxTreeSelectComboBox.SelectedIndex = 0;
            }
        }
        public void ShowTree()
        {
            treeView.Nodes.Clear();
            visualizator vs = new visualizator(treeView.Nodes);
            if (showAllCheckBox.Checked)
                foreach (SyntaxTreeSelectComboBoxItem item in syntaxTreeSelectComboBox.Items)
                    vs.prepare_node(item.Node, "compilation_unit");
            else
                if (syntaxTreeSelectComboBox.SelectedItem!=null)
                    vs.prepare_node((syntaxTreeSelectComboBox.SelectedItem as SyntaxTreeSelectComboBoxItem).Node, "compilation_unit");
        }

 /*       //ssyy
        public void SaveTree()
        {
            if (syntaxTreeSelectComboBox.SelectedItem != null)
            {
                PascalABCCompiler.SyntaxTree.compilation_unit cu = (syntaxTreeSelectComboBox.SelectedItem as SyntaxTreeSelectComboBoxItem).Node as PascalABCCompiler.SyntaxTree.compilation_unit;
                PascalABCCompiler.SyntaxTree.CPSyntaxTreeStreamWriter sw = new PascalABCCompiler.SyntaxTree.CPSyntaxTreeStreamWriter();
                System.IO.FileStream fs = new System.IO.FileStream(cu.file_name + ".tre", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                System.IO.BinaryWriter fbw = new System.IO.BinaryWriter(fs);
                sw.bw = fbw;
                fbw.Write((byte)1);
                cu.visit(sw);
                fbw.Close();
                fs.Close();
            }
        }
        //\ssyy
*/        
        private void syntaxTreeSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowTree();
        }

        private void showAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxTreeSelectComboBox.Enabled = (syntaxTreeSelectComboBox.Items.Count > 0)&&!showAllCheckBox.Checked;
            ShowTree();
        }

        private void showAllCheckBox_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                UpdateSelectList();
                ShowTree();
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = treeView.SelectedNode;
            StatusLabel.Text = "";
            PascalABCCompiler.SyntaxTree.syntax_tree_node syntax_tree_node = tn.Tag as PascalABCCompiler.SyntaxTree.syntax_tree_node;
            if (syntax_tree_node.source_context == null) return;
            StatusLabel.Text = syntax_tree_node.source_context.ToString() + string.Format("({0},+{1})", syntax_tree_node.source_context.Position, syntax_tree_node.source_context.Length);
            if(syntax_tree_node.source_context.FileName!=null)
                StatusLabel.Text+=Path.GetFileName(syntax_tree_node.source_context.FileName);
            if (Errors.Count > 0)
                if (Errors[syntax_tree_node] != null)
                    StatusLabel.Text += string.Format(" [BAD{0}]", Errors[syntax_tree_node]);
            this.VisualEnvironmentCompiler.ExecuteSourceLocationAction(
                PascalABCCompiler.Tools.ConvertSourceContextToSourceLocation((syntaxTreeSelectComboBox.SelectedItem as SyntaxTreeSelectComboBoxItem).FileName, syntax_tree_node.source_context), 
                SourceLocationAction.SelectAndGotoBeg);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            PascalABCCompiler.CompilerType ct = VisualEnvironmentCompiler.DefaultCompilerType;
            VisualEnvironmentCompiler.DefaultCompilerType = PascalABCCompiler.CompilerType.Standart;
            VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.Build, null);
            VisualEnvironmentCompiler.DefaultCompilerType = ct;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            PascalABCCompiler.CompilerType ct = VisualEnvironmentCompiler.DefaultCompilerType;
            VisualEnvironmentCompiler.DefaultCompilerType = PascalABCCompiler.CompilerType.Standart;
            VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.Rebuild, null);
            VisualEnvironmentCompiler.DefaultCompilerType = ct;
        }

        private void tbCPSaveTree_Click(object sender, EventArgs e)
        {
           // SaveTree();
            
        }

        private void tsParse_Click(object sender, EventArgs e)
        {
            string FileName = (string)VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.GetCurrentSourceFileName, null);
            string FileText = (string)VisualEnvironmentCompiler.Compiler.SourceFilesProvider(FileName, PascalABCCompiler.SourceFileOperation.GetText);
            List<Error> Errors=new List<Error>();
            List<CompilerWarning> Warnings = new List<CompilerWarning>();
            //PascalABCCompiler.SyntaxTree.syntax_tree_node sn = VisualEnvironmentCompiler.Compiler.ParsersController.Compile(file_name, FileText, Errors, PascalABCCompiler.ParserTools.ParseMode.Expression);
            PascalABCCompiler.SyntaxTree.syntax_tree_node sn = LanguageProvider.Instance.SelectLanguageByExtension(FileName).Parser.GetExpression(FileName, FileText, Errors, Warnings);
            if (Errors.Count > 0)
                StatusLabel.Text = Errors.Count + " errors";
            syntaxTreeSelectComboBox.Items.Clear();
            if (sn == null)
                return;
            syntaxTreeSelectComboBox.Items.Add(new SyntaxTreeSelectComboBoxItem(sn, FileName));
            syntaxTreeSelectComboBox.SelectedIndex = 0;
        }

        private void tsParsePart_Click(object sender, EventArgs e)
        {
            // Добавка part_ не поддерживается парсером  EVA
            /*string FileName = (string)VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.GetCurrentSourceFileName, null);
            string FileText = (string)VisualEnvironmentCompiler.Compiler.SourceFilesProvider(FileName, PascalABCCompiler.SourceFileOperation.GetText);
            List<Error> Errors = new List<Error>();
            List<CompilerWarning> Warnings = new List<CompilerWarning>();
            PascalABCCompiler.SyntaxTree.syntax_tree_node sn = VisualEnvironmentCompiler.StandartCompiler.ParsersController.SelectParserForUnitAndBuildTree(FileName+"part_", FileText, Errors, Warnings, PascalABCCompiler.Parsers.ParseMode.Normal);
            if (Errors.Count > 0)
                StatusLabel.Text = Errors.Count + " errors";
            syntaxTreeSelectComboBox.Items.Clear();
            if (sn == null)
                return;
            syntaxTreeSelectComboBox.Items.Add(new SyntaxTreeSelectComboBoxItem(sn, FileName));
            syntaxTreeSelectComboBox.SelectedIndex = 0;*/

        }

        PascalABCCompiler.SyntaxTree.syntax_tree_node ParseCurrent(List<Error> Errors)
        {
            string FileName = (string)VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.GetCurrentSourceFileName, null);
            string FileText = (string)VisualEnvironmentCompiler.StandartCompiler.SourceFilesProvider(FileName, PascalABCCompiler.SourceFileOperation.GetText);
            return LanguageProvider.Instance.SelectLanguageByExtension(FileName).Parser.GetCompilationUnit(FileName, FileText, Errors, new List<CompilerWarning>(), PascalABCCompiler.Parsers.ParseMode.Normal);
        }

        PascalABCCompiler.SyntaxTree.documentation_comment_list ParseCurrentDocs()
        {
            string FileName = (string)VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.GetCurrentSourceFileName, null);
            string FileText = (string)VisualEnvironmentCompiler.StandartCompiler.SourceFilesProvider(FileName, PascalABCCompiler.SourceFileOperation.GetText);
            return LanguageProvider.Instance.SelectLanguageByExtension(FileName).DocParser.BuildTree(FileText);
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            List<Error> Errors = new List<Error>();
            DateTime dt = DateTime.Now;
            PascalABCCompiler.SyntaxTree.syntax_tree_node sn = ParseCurrent(Errors);
            StatusLabel.Text = string.Format("[{0}ms]", (DateTime.Now - dt).TotalMilliseconds);
            if (Errors.Count > 0)
                StatusLabel.Text += Errors.Count + " errors";
            syntaxTreeSelectComboBox.Items.Clear();
            if (sn == null)
                return;
            string FileName = (string)VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.GetCurrentSourceFileName, null);
            syntaxTreeSelectComboBox.Items.Add(new SyntaxTreeSelectComboBoxItem(sn, FileName));
            syntaxTreeSelectComboBox.SelectedIndex = 0;

        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            List<Error> Errors = new List<Error>();
            PascalABCCompiler.SyntaxTree.syntax_tree_node sn = ParseCurrent(Errors);
            PascalABCCompiler.SyntaxTree.documentation_comment_list dt = ParseCurrentDocs();
            PascalABCCompiler.DocumentationConstructor docconst = new PascalABCCompiler.DocumentationConstructor();
            Dictionary<PascalABCCompiler.SyntaxTree.syntax_tree_node, string> documentation = docconst.Construct(sn, dt);
        }
    }
}
