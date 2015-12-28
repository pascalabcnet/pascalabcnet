using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SemanticTree;


using System.Collections;

namespace VisualPascalABCPlugins
{
    public partial class SemanticTreeVisualisatorForm : Form
    {
        public IVisualEnvironmentCompiler VisualEnvironmentCompiler;

        public SematicTreeVisitor Visitor;

        public SemanticTreeVisualisatorForm()
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
            treeView.Nodes.Clear();
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
            switch (State)
            {
                case PascalABCCompiler.CompilerState.CompilationStarting:
                    BuildButtonsEnabled = false;                
                    this.Refresh();
                    break;
                case PascalABCCompiler.CompilerState.CompilationFinished:
                    ShowTree();
                    BuildButtonsEnabled = true;
                    break;
            }
        }
        public void ShowTree()
        {
            //Желательно строить в отдельном потоке а потом разом выводить. Незабуть про синхронизацию.
            //Пока можно посто в лоб.
            try
            {
                PascalABCCompiler.SemanticTree.IProgramNode Root = VisualEnvironmentCompiler.StandartCompiler.SemanticTree;
                treeView.Nodes.Clear();                
                // связываем Visitor и treeView.Nodes
                Visitor = new SematicTreeVisitor(treeView.Nodes);
                Visitor.CreateMyDictionaries();
                Visitor.setTreeView(treeView);
                // собственно проходим по дереву от корня                
                Root.visit(Visitor);
                //Visitor.visit(Root, treeView.Nodes);             
                //Visitor.visit(Root);
                // устанавливаем обратные связи
                Visitor.makeUpRows(treeView);                
                
                // по умолчанию выбранным является Nodes[0]
                treeView.SelectedNode = treeView.Nodes[0];
                //treeView.SelectedNode.BackColor = Color.Chocolate;
                //treeView.Invalidate();
                treeView.Invalidate();
                treeView.Refresh();
                treeView.Update();
                
            }
            catch (Exception e)
            {
                VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.AddTextToCompilerMessages, "SemanticVisualisator Exception: " + e.ToString());
            }
        }        

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            VisualEnvironmentCompiler.StandartCompiler.InternalDebug.CodeGeneration = false;
            PascalABCCompiler.CompilerType ct = VisualEnvironmentCompiler.DefaultCompilerType;
            VisualEnvironmentCompiler.DefaultCompilerType = PascalABCCompiler.CompilerType.Standart;
            VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.Build, null);
            VisualEnvironmentCompiler.DefaultCompilerType = ct;
            VisualEnvironmentCompiler.StandartCompiler.InternalDebug.CodeGeneration = true;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            VisualEnvironmentCompiler.StandartCompiler.InternalDebug.CodeGeneration = false;
            PascalABCCompiler.CompilerType ct = VisualEnvironmentCompiler.DefaultCompilerType;
            VisualEnvironmentCompiler.DefaultCompilerType = PascalABCCompiler.CompilerType.Standart;
            VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.Rebuild, null);
            VisualEnvironmentCompiler.DefaultCompilerType = ct;
            VisualEnvironmentCompiler.StandartCompiler.InternalDebug.CodeGeneration = true;
        }                      
        
        
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {            
            Visitor.visitSelectedNode(treeView.SelectedNode);
            treeView.Invalidate();
            treeView.Refresh();
            treeView.Update();



            TreeNode tn = treeView.SelectedNode;
            mylabel.Text = "";
            PascalABCCompiler.SemanticTree.ISemanticNode semantic_tree_node = tn.Tag as PascalABCCompiler.SemanticTree.ISemanticNode;
            //PascalABCCompiler.SyntaxTree.syntax_tree_node syntax_tree_node = tn.Tag as PascalABCCompiler.SyntaxTree.syntax_tree_node;
            //if (syntax_tree_node.source_context == null) return;
            if (semantic_tree_node == null) return;
            PascalABCCompiler.SemanticTree.ILocated lt = semantic_tree_node as PascalABCCompiler.SemanticTree.ILocated;
            if (lt == null) return;
            if (lt.Location == null) return;
            //StatusLabel.Text = semantic_tree_node.ToString() + String.Format("({0},+{1})--({2},+{3})", lt.Location.begin_column_num, lt.Location.begin_line_num, lt.Location.end_column_num, lt.Location.end_line_num);
            string s = String.Format("({0},+{1})--({2},+{3})", lt.Location.begin_line_num, lt.Location.begin_column_num, lt.Location.end_line_num, lt.Location.end_column_num);
            mylabel.Text = s;
            mylabel.Invalidate();
            treeView.Invalidate();
            //StatusLabel.Text = syntax_tree_node.source_context.ToString() + string.Format("({0},+{1})", syntax_tree_node.source_context.Position, syntax_tree_node.source_context.Length);
            //if (syntax_tree_node.source_context.FileName != null)
            //    StatusLabel.Text += Path.GetFileName(syntax_tree_node.source_context.FileName);
            //if (Errors.Count > 0)
            //    if (Errors[syntax_tree_node] != null)
            //        StatusLabel.Text += string.Format(" [BAD{0}]", Errors[syntax_tree_node]);
            //this.VisualEnvironmentCompiler.ExecuteSourceLocationAction(
            //    PascalABCCompiler.Tools.ConvertSourceContextToSourceLocation((syntaxTreeSelectComboBox.SelectedItem as SyntaxTreeSelectComboBoxItem).FileName, syntax_tree_node.source_context),
            //    SourceLocationAction.SelectAndGotoBeg);  


            Visitor.visitSelectedNode(treeView.SelectedNode);
            treeView.Invalidate();
            //treeView.Refresh();
            //treeView.Update();
        }

     /*   private void treeView_Click(object sender, EventArgs e)
        {
            Visitor.visitSelectedNode(treeView.SelectedNode);
            treeView.Invalidate();
            treeView.Refresh();
            treeView.Update();



            TreeNode tn = treeView.SelectedNode;
            mylabel.Text = "";
            PascalABCCompiler.SemanticTree.ISemanticNode semantic_tree_node = tn.Tag as PascalABCCompiler.SemanticTree.ISemanticNode;
            //PascalABCCompiler.SyntaxTree.syntax_tree_node syntax_tree_node = tn.Tag as PascalABCCompiler.SyntaxTree.syntax_tree_node;
            //if (syntax_tree_node.source_context == null) return;
            if (semantic_tree_node == null) return;
            PascalABCCompiler.SemanticTree.ILocated lt = semantic_tree_node as PascalABCCompiler.SemanticTree.ILocated;
            if (lt == null) return;
            if (lt.Location == null) return;
            //StatusLabel.Text = semantic_tree_node.ToString() + String.Format("({0},+{1})--({2},+{3})", lt.Location.begin_column_num, lt.Location.begin_line_num, lt.Location.end_column_num, lt.Location.end_line_num);
            string s = String.Format("({0},+{1})--({2},+{3})", lt.Location.begin_line_num, lt.Location.begin_column_num, lt.Location.end_line_num, lt.Location.end_column_num);
            mylabel.Text = s;
            mylabel.Invalidate();
            treeView.Invalidate();
            //StatusLabel.Text = syntax_tree_node.source_context.ToString() + string.Format("({0},+{1})", syntax_tree_node.source_context.Position, syntax_tree_node.source_context.Length);
            //if (syntax_tree_node.source_context.FileName != null)
            //    StatusLabel.Text += Path.GetFileName(syntax_tree_node.source_context.FileName);
            //if (Errors.Count > 0)
            //    if (Errors[syntax_tree_node] != null)
            //        StatusLabel.Text += string.Format(" [BAD{0}]", Errors[syntax_tree_node]);
            //this.VisualEnvironmentCompiler.ExecuteSourceLocationAction(
            //    PascalABCCompiler.Tools.ConvertSourceContextToSourceLocation((syntaxTreeSelectComboBox.SelectedItem as SyntaxTreeSelectComboBoxItem).FileName, syntax_tree_node.source_context),
            //    SourceLocationAction.SelectAndGotoBeg);  


            Visitor.visitSelectedNode(treeView.SelectedNode);
            treeView.Invalidate();
            treeView.Refresh();
            treeView.Update();
        }*/

       
   }
}