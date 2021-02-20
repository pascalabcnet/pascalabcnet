// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.FormsDesigner;
using ICSharpCode.SharpDevelop.Gui;
using VisualPascalABCPlugins;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualPascalABC
{
    public partial class Form1 : IWorkbenchDesignerService
    {
        private bool _designer_is_active = false;

        public bool DesignerIsActive
        {
            get
            {
                return _designer_is_active;
            }
        }

        public void SetActiveDesignerDirty()
        {
            if (_currentCodeFileDocument.Designer != null)
                _currentCodeFileDocument.Designer.IsDirty = true;
        }

        public void UpdateDesignerIsActive()
        {
            bool active =
                _currentCodeFileDocument != null &&
                _currentCodeFileDocument.DesignerAndCodeTabs != null &&
                _currentCodeFileDocument.DesignerAndCodeTabs.SelectedTab == _currentCodeFileDocument.DesignerPage;
            if (active == _designer_is_active)
            {
                return;
            }
            _designer_is_active = active;
            if (active)
            {
                EnableDesigner();
            }
            else
            {
                DisableDesigner();
            }
        }

        public void DisableDesigner()
        {
            if (ToolBoxWindow != null)
            {
                //ToolBoxWindow.Visible = false;
                ToolBoxWindow.Hide();
            }
            if (PropertiesWindow != null)
            {
                //PropertiesWindow.Visible = false;
                PropertiesWindow.Hide();
            }
            BottomTabsVisible = true;
            mrDesigner.Visible = false;
            miDelete.Visible = false;
            miDelete.Enabled = false;
            miToolBox.Visible = false;
            miProperties.Visible = false;
        }

        public void EnableDesigner()
        {
            if (ToolBoxWindow != null)
            {
                //ToolBoxWindow.Visible = true;
                ToolBoxWindow.Show();
            }
            if (PropertiesWindow != null)
            {
                //PropertiesWindow.Visible = true;
                PropertiesWindow.Show();
            }
            BottomTabsVisible = false;
            mrDesigner.Visible = true;
            miDelete.Visible = true;
            miDelete.Enabled = true;
            miToolBox.Visible = true;
            miProperties.Visible = true;
            if (_currentCodeFileDocument.DesignerPage != null && _currentCodeFileDocument.DesignerPage.Controls.Count > 0) //roman//
                _currentCodeFileDocument.DesignerPage.Controls[0].Focus();//ñòàâèì ôîêóñ íà designSurface, ÷òîáû ñîîáùåíèÿ îò êëàâèàòóðû íå íàïðàâëÿëèñü â òåêñòîâûé ðåäàêòîð 
        }

        public void ExecuteDesignerCommand(CommandID id)
        {
            if (_currentCodeFileDocument != null && _currentCodeFileDocument.DesignerAndCodeTabs != null && _designer_is_active)
            {
                _currentCodeFileDocument.Designer.ExecuteCommand(id);
            }
        }

        public void ExecAlignLefts()
        {
            ExecuteDesignerCommand(StandardCommands.AlignLeft);
        }

        private void miAlignLefts_Click(object sender, EventArgs e)
        {
            ExecAlignLefts();
        }

        public void ExecAlignRights()
        {
            ExecuteDesignerCommand(StandardCommands.AlignRight);
        }

        private void miAlignRights_Click(object sender, EventArgs e)
        {
            ExecAlignRights();
        }

        public void ExecAlignTops()
        {
            ExecuteDesignerCommand(StandardCommands.AlignTop);
        }

        private void miAlignTops_Click(object sender, EventArgs e)
        {
            ExecAlignTops();
        }

        public void ExecAlignBottoms()
        {
            ExecuteDesignerCommand(StandardCommands.AlignBottom);
        }

        private void miAlignBottoms_Click(object sender, EventArgs e)
        {
            ExecAlignBottoms();
        }

        public void ExecAlignMiddles()
        {
            ExecuteDesignerCommand(StandardCommands.AlignHorizontalCenters);
        }

        private void miAlignMiddles_Click(object sender, EventArgs e)
        {
            ExecAlignMiddles();
        }

        public void ExecAlignCenters()
        {
            ExecuteDesignerCommand(StandardCommands.AlignVerticalCenters);
        }

        private void miAlignCenters_Click(object sender, EventArgs e)
        {
            ExecAlignCenters();
        }

        public void ExecAlignToGrid()
        {
            ExecuteDesignerCommand(StandardCommands.AlignToGrid);
        }

        private void miAlignToGrid_Click(object sender, EventArgs e)
        {
            ExecAlignToGrid();
        }

        public void ExecCenterHorizontally()
        {
            ExecuteDesignerCommand(StandardCommands.CenterHorizontally);
        }

        private void miCenterHorizontally_Click(object sender, EventArgs e)
        {
            ExecCenterHorizontally();
        }

        public void ExecCenterVertically()
        {
            ExecuteDesignerCommand(StandardCommands.CenterVertically);
        }

        private void miCenterVertically_Click(object sender, EventArgs e)
        {
            ExecCenterVertically();
        }

        public void ExecSizeToControl()
        {
            ExecuteDesignerCommand(StandardCommands.SizeToControl);
        }

        private void miSizeToControl_Click(object sender, EventArgs e)
        {
            ExecSizeToControl();
        }

        public void ExecSizeToControlWidth()
        {
            ExecuteDesignerCommand(StandardCommands.SizeToControlWidth);
        }

        private void miSizeToControlWidth_Click(object sender, EventArgs e)
        {
            ExecSizeToControlWidth();
        }

        public void ExecSizeToControlHeight()
        {
            ExecuteDesignerCommand(StandardCommands.SizeToControlHeight);
        }

        private void miSizeToControlHeight_Click(object sender, EventArgs e)
        {
            ExecSizeToControlHeight();
        }

        public void ExecSizeToGrid()
        {
            ExecuteDesignerCommand(StandardCommands.SizeToGrid);
        }

        private void miSizeToGrid_Click(object sender, EventArgs e)
        {
            ExecSizeToGrid();
        }

        public void ExecBringToFront()
        {
            ExecuteDesignerCommand(StandardCommands.BringToFront);
        }

        private void miBringToFront_Click(object sender, EventArgs e)
        {
            ExecBringToFront();
        }

        public void ExecSendToBack()
        {
            ExecuteDesignerCommand(StandardCommands.SendToBack);
        }

        private void miSendToBack_Click(object sender, EventArgs e)
        {
            ExecSendToBack();
        }

        public void ExecTabOrder()
        {
            ExecuteDesignerCommand(StandardCommands.TabOrder);
        }

        public void ExecShowGrid()
        {
            ExecuteDesignerCommand(StandardCommands.ShowGrid);
        }

        public void ExecSnapToGrid()
        {
            ExecuteDesignerCommand(StandardCommands.SnapToGrid);
        }

        public void ExecDelete()
        {
            //(_currentCodeFileDocument.Designer.host.selectionService as SampleDesignerHost.SampleSelectionService).need_clear_selection = true;
            //roman//
            if ((_currentCodeFileDocument.Designer.Host.GetService(typeof(ISelectionService)) as ISelectionService).GetComponentSelected(_currentCodeFileDocument.Designer.Host.RootComponent))
            {
                //MessageBox.Show("Íåëüçÿ ïðèìåíèòü êîìàíäó \"Óäàëèòü\" äëÿ ôîðìû", "Äèçàéíåð ôîðì", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ExecuteDesignerCommand(StandardCommands.Delete);
        }

        public void GenerateAllDesignersCode()
        {
            foreach (CodeFileDocumentControl doc in OpenDocuments.Values)
            {
                if (doc.DesignerAndCodeTabs != null)
                {
                    doc.GenerateDesignerCode(null);
                }
            }
        }

        private void GenerateMainProgramForDesigner()
        {
            if (_currentCodeFileDocument.DesignerAndCodeTabs == null)
            {
                string caption = PascalABCCompiler.StringResources.Get("VP_MF_FORM_DESIGNER");
                string text = PascalABCCompiler.StringResources.Get("VP_MF_SHOULD_CREATE_FORM_FIRST");
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            CodeFileDocumentControl DesignerDocument = _currentCodeFileDocument;
            string tmp_dir = WorkbenchStorage.WorkingDirectory;
            WorkbenchStorage.WorkingDirectory = Path.GetDirectoryName(_currentCodeFileDocument.FileName);
            WorkbenchServiceFactory.FileService.OpenFile(null, null);
            _currentCodeFileDocument.GenerateMainProgram(
               Path.GetFileNameWithoutExtension(DesignerDocument.FileName), DesignerDocument.FormName);
        }

        private void GenerateMainProgramForDesigner(string MainFileName)
        {
            if (_currentCodeFileDocument.DesignerAndCodeTabs == null)
            {
                //string caption = PascalABCCompiler.StringResources.Get("VP_MF_FORM_DESIGNER");
                //string text = PascalABCCompiler.StringResources.Get("VP_MF_SHOULD_CREATE_FORM_FIRST");
                //MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            CodeFileDocumentControl DesignerDocument = _currentCodeFileDocument;
            WorkbenchServiceFactory.FileService.OpenFile(MainFileName, null);
            _currentCodeFileDocument.GenerateMainProgram(
               Path.GetFileNameWithoutExtension(DesignerDocument.FileName), DesignerDocument.FormName);
            CurrentCodeFileDocument = DesignerDocument;
        }

        private void miFormProgram_Click(object sender, EventArgs e)
        {
            GenerateMainProgramForDesigner();
        }

        public void ExecDesignerCopy()
        {
            //roman//
            if ((_currentCodeFileDocument.Designer.Host.GetService(typeof(ISelectionService)) as ISelectionService).GetComponentSelected(_currentCodeFileDocument.Designer.Host.RootComponent))
            {
                //MessageBox.Show("Íåëüçÿ ïðèìåíèòü êîìàíäó \"Êîïèðîâàòü\" äëÿ ôîðìû", "Äèçàéíåð ôîðì", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ExecuteDesignerCommand(StandardCommands.Copy);
        }

        public void ExecDesignerCut()
        {
            /*ICollection comps = _currentCodeFileDocument.Designer.host.selectionService.GetSelectedComponents();
            foreach (IComponent comp in comps)
            {
                _currentCodeFileDocument.Designer.host.DeleteDesignedComponent(comp);
            }
            (_currentCodeFileDocument.Designer.host.selectionService as SampleDesignerHost.SampleSelectionService).need_clear_selection = true;*/
            //roman//
            if ((_currentCodeFileDocument.Designer.Host.GetService(typeof(ISelectionService)) as ISelectionService).GetComponentSelected(_currentCodeFileDocument.Designer.Host.RootComponent))
            {
                //MessageBox.Show("Íåëüçÿ ïðèìåíèòü êîìàíäó \"Âûðåçàòü\" äëÿ ôîðìû", "Äèçàéíåð ôîðì", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ExecuteDesignerCommand(StandardCommands.Cut);
        }

        public void ExecDesignerPaste()
        {
            ExecuteDesignerCommand(StandardCommands.Paste);
        }

        private void AddDesignerSidebars()
        {
            if (ToolBoxWindow == null)
            {
                ToolBoxWindow = new ToolBoxForm();
                SharpDevelopSideBar sideBar = FormsDesignerViewContent.FormsDesignerToolBox;
                sideBar.Dock = DockStyle.Fill;
                sideBar.Parent = ToolBoxWindow;
                AddWindowToDockPanel(ToolBoxWindow, MainDockPanel, OutputWindow.Dock, DockState.DockLeft, OutputWindow.IsFloat, null, -1);
                ToolBoxWindow.Visible = false;
                ToolBoxWindow.Hide();
            }
            if (PropertiesWindow == null)
            {
                PropertiesWindow = new PropertiesForm();
                Form1StringResources.SetTextForAllControls(PropertiesWindow);
                Panel properties = FormsDesignerViewContent.PropertyPad.PropertyPadPanel;
                properties.Dock = DockStyle.Fill;
                properties.Parent = PropertiesWindow;
                AddWindowToDockPanel(PropertiesWindow, MainDockPanel, DockStyle.Fill, DockState.DockRight, OutputWindow.IsFloat, ProjectPane, int.MaxValue);
                PropertiesWindow.Visible = false;
                PropertiesWindow.Hide();
            }
        }
    }
}
