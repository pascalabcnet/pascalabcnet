// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace VisualPascalABC
{
	/// <summary>
	/// Description of ReferenceForm.
	/// </summary>
	public partial class ReferenceForm : Form
	{
		public ReferenceForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private static ReferenceForm instance;
		
		public static ReferenceForm Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ReferenceForm();
					Form1StringResources.SetTextForAllControls(instance);
				}
				return instance;
			}
		}
		
		private List<string> assembly_files = new List<string>();
        private List<string> native_dlls = new List<string>();
        public bool fromFile = false;
		private AssemblyType assemblyType = AssemblyType.GAC;
		
		public AssemblyType AssemblyType
		{
			get
			{
				if (assemblyType == AssemblyType.File)
					return assemblyType;
				if (lvGac.SelectedItems.Count > 0)
					return AssemblyType.GAC;
				//if (lvCom.SelectedItems.Count > 0)
				//	return AssemblyType.COM;
				return AssemblyType.GAC;
			}
		}
		
		public void LoadAssemblies()
		{
			lvGac.Items.AddRange(GetCacheContent().ToArray());
		}
		
		public void Clear()
		{
            assembly_files.Clear();
            native_dlls.Clear();
            assemblyType = AssemblyType.GAC;
			lvGac.Items.Clear();
            tbLog.Text = "";
			//lvCom.Items.Clear();
		}
		
		public string[] GetSelectedGACAssemblies()
		{
			List<string> lst = new List<string>();
			for (int i=0; i<lvGac.SelectedItems.Count; i++)
			{
				lst.Add(lvGac.SelectedItems[i].Text);
			}
			return lst.ToArray();
		}
		
		TypeLibConverter conv = new TypeLibConverter();
		public string[] GetSelectedCOMObjects()
		{
			List<string> lst = new List<string>();
			/*for (int i=0; i<lvCom.SelectedItems.Count; i++)
			{
				string s = ProjectTask.GenerateStubForCOM((lvCom.SelectedItems[i].Tag as TypeLibrary).Path);
				if (s != null)
				lst.Add(s);
			}*/
			return lst.ToArray();
		}
		
		public string[] GetSelectedFileAssemblies()
		{
			return assembly_files.ToArray();
		}

        public string[] GetNativeDlls()
        {
            return native_dlls.ToArray();
        }

        private List<ListViewItem> GetCacheContent()
		{
			List<ListViewItem> itemList = new List<ListViewItem>();
			foreach (GacInterop.AssemblyListEntry asm in GacInterop.GetAssemblyList()) {
				ListViewItem item = new ListViewItem(new string[] {asm.Name, asm.Version});
				item.Tag = asm.FullName;
				itemList.Add(item);
			}
			return itemList;
		}

		void Button1Click(object sender, EventArgs e)
		{
			openFileDialog1.InitialDirectory = ProjectFactory.Instance.ProjectDirectory;
			openFileDialog1.Filter = VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.GetAssemblyFilterForDialogs();
			openFileDialog1.ShowDialog();
		}
		
		void OpenFileDialog1FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			foreach (string s in openFileDialog1.FileNames)
			{
				assembly_files.Add(s);
			}
			assemblyType = AssemblyType.File;
			DialogResult = DialogResult.OK;
			Close();
		}
		
		private bool com_page_populated = false;
		
		void LvComVisibleChanged(object sender, EventArgs e)
		{
			/*if (com_page_populated == false && lvCom.Visible) {
				com_page_populated = true;
				PopulateListView();
			}*/
		}
		
		void PopulateListView()
		{
			IEnumerable<TypeLibrary> types = TypeLibrary.Libraries;
			List<TypeLibrary> sort_types = new List<TypeLibrary>();
			sort_types.AddRange(types);
			sort_types.Sort(TypeLibrary.Comparer);
			foreach (TypeLibrary typeLib in sort_types) {
				ListViewItem newItem = new ListViewItem(new string[] { typeLib.Description, typeLib.Path });
				newItem.Tag = typeLib;
				//lvCom.Items.Add(newItem);
			}
		}

        private void nugetInstalled(bool result, string[] dlls, string[] xmls, string[] ndlls)
        {
            assemblyType = AssemblyType.File;
            if (result)
            {
                foreach (string dll in dlls)
                    assembly_files.Add(dll);
                foreach (string dll in ndlls)
                    native_dlls.Add(dll);
                this.DialogResult = DialogResult.OK;
            }
            else
                this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void nugetOutput(string output)
        {
            tbLog.Text += output;
        }

        private void btnInstallPackage_Click(object sender, EventArgs e)
        {
            NuGetTasks.InstallPackage(tbPackageName.Text, 
                                      ProjectFactory.Instance.CurrentProject.ProjectDirectory, 
                                      new NugetPackageInstallHandler(nugetInstalled),
                                      new NugetPackageOutputHandler(nugetOutput));
            
        }

        private void btnSearchPackages_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nuget.org/packages");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
