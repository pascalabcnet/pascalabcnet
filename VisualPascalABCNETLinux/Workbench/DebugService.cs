using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using VisualPascalABCPlugins;

namespace VisualPascalABC
{
    public partial class WorkbenchDebugService : IWorkbenchDebugService
    {
        void IWorkbenchDebugService.AddDebugPage(ICodeFileDocument tabPage)
        {
            debuggedPage = tabPage;
            AddDebugPage(debuggedPage as CodeFileDocumentControl);
            DebugUtils.DebuggerManager.SetAsDebugPage(debuggedPage);
        }

        private bool row_exists(string expr)
        {
            for (int i = 0; i < this.WdataGridView1.Rows.Count; i++)
            {
                if ((string)this.WdataGridView1.Rows[i].Cells[0].Value == expr)
                    return true;
            }
            return false;
        }

        public void GotoWatch()
        {
            BottomTabsVisible = true;
            SelectContent(DebugWatchListWindow, true);
            DebugWatchListWindow.AddNewEntry("");
        }

        public void AddVariable(string expr, bool show_tab)
        {
            if (row_exists(expr)) return;
            RetValue rv = DebugUtils.DebuggerManager.Evaluate(expr);
            int i = this.WdataGridView1.Rows.Count - 1;
            BottomTabsVisible = true;
            if (show_tab)
                SelectContent(DebugWatchListWindow, false);
            //this.BottomTabControl.SelectTab(this.tpWarningList);
            //if (this.WdataGridView1.Rows.Count == 1 && this.WdataGridView1.Rows[0].Cells[0].Value == null) i = 0;
            if (this.WdataGridView1.Rows[0].Cells[0].Value == null) i = 0;
            else //if (this.WdataGridView1.Rows[1].Cells[0].Value != null)
            {
                this.WdataGridView1.Rows.Insert(0, 1); i = 0;
                //i = this.WdataGridView1.Rows.Count-1;
            }
            if (this.WdataGridView1.Rows.Count == 1) this.WdataGridView1.Rows.Insert(0, 1);
            this.WdataGridView1.Rows[i].Cells[0].Value = expr;
            DebugWatchListWindow.RefreshRow(i);
        }

        public void AddVariable(ListItem nv)
        {
            if (row_exists(nv.Name)) return;
            //this.WdataGridView1.Rows.Add();
            int i = this.WdataGridView1.Rows.Count - 1;
            //this.BottomTabControl.SelectTab(this.tpWarningList);
            BottomTabsVisible = true;
            SelectContent(DebugWatchListWindow, false);
            //if (this.WdataGridView1.Rows.Count == 1 && this.WdataGridView1.Rows[0].Cells[0].Value == null) i = 0;
            if (this.WdataGridView1.Rows[0].Cells[0].Value == null) i = 0;
            else //if (this.WdataGridView1.Rows[1].Cells[0].Value != null)
            {
                this.WdataGridView1.Rows.Insert(0, 1); i = 0;
                //i = this.WdataGridView1.Rows.Count-1;
            }
            if (this.WdataGridView1.Rows.Count == 1) this.WdataGridView1.Rows.Insert(0, 1);
            this.WdataGridView1.Rows[i].Cells[0].Value = nv.Name;
            DebugWatchListWindow.RefreshRow(i);
        }

        public void RefreshPad(IList<IListItem> items)
        {
            try
            {
                DebugWatchListWindow.RefreshWatch();
                AdvancedDataGridView.TreeGridNode.UpdateNodesForLocalList(DebugVariablesListWindow.watchList, DebugVariablesListWindow.watchList.Nodes, items);
            }
            catch (System.Exception e)
            {

            }
        }

        public void AddObjectVariable(NamedValue lv)
        {
            if (lv.IsArray)
            {
                AddArrayVariable(lv);
                return;
            }
            NamedValueCollection nvc = lv.GetMembers();
            //System.Windows.Forms.TreeView obj = new System.Windows.Forms.TreeView();
            //obj.Nodes.Add(
            // dgLocalVars.Rows.Add(lv.Name, "<object>");
        }

        public void AddArrayVariable(NamedValue lv)
        {
            //dgLocalVars.Rows.Add(lv.Name, lv.AsString);
        }
    }
}