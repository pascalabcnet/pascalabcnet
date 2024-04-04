//------------------------------------------------------------------------------
/// <copyright from='1997' to='2002' company='Microsoft Corporation'>
///    Copyright (c) Microsoft Corporation. All Rights Reserved.
///
///    This source code is intended only as a supplement to Microsoft
///    Development Tools and/or on-line documentation.  See these other
///    materials for detailed information regarding Microsoft code samples.
///
/// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SampleDesignerApplication
{
	/// This form shows the service requests that have been made to the host.
	/// It divides them into successful and unsuccessful requests.
	public class ServiceRequests : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private TreeNode successfulNode;
		private TreeNode unsuccessfulNode;
		private Hashtable serviceRequests;

		private System.ComponentModel.Container components = null;

		public ServiceRequests()
		{
			InitializeComponent();
			InitializeTreeViewNodes();
		}

		/// Create a new tree node to describe a service request of the given type.
		private TreeNode CreateNodeForType(Type type) 
		{
			TreeNode node = new TreeNode(type.FullName);
			string stack = Environment.StackTrace;
			string[] entries = stack.Split('\r', '\n');

			bool recordEntry = false;
			foreach(string entry in entries) 
			{
				if (entry.Length > 0)
				{
					// Skip the frames that we're currently on.
					//
					if (recordEntry) 
					{
						node.Nodes.Add(new TreeNode(entry));
					}
					else if (entry.IndexOf("GetService") != -1) 
					{
						recordEntry = true;
					}
				}
			}
			return node;
		}

		/// Set up our two root nodes.
		private void InitializeTreeViewNodes() 
		{
			successfulNode = new TreeNode("Successful Requests");
			unsuccessfulNode = new TreeNode("Unsuccessful Requests");

			treeView1.Nodes.Add(successfulNode);
			treeView1.Nodes.Add(unsuccessfulNode);
		}

		public void ServiceFailed(Type type) 
		{
			if (serviceRequests == null) 
			{
				serviceRequests = new Hashtable();
			}

			if (!serviceRequests.ContainsKey(type)) 
			{
				serviceRequests[type] = false; // Mark the request for this type as failed
				unsuccessfulNode.Nodes.Add(CreateNodeForType(type)); // Add it to our tree
			}
		}

		public void ServiceSucceeded(Type type) 
		{
			if (serviceRequests == null) 
			{
				serviceRequests = new Hashtable();
			}

			if (!serviceRequests.ContainsKey(type)) 
			{
				serviceRequests[type] = true; // Mark the request for this type as successful
				successfulNode.Nodes.Add(CreateNodeForType(type)); // Add it to our tree
			}
		}

		/// Clean up any resources being used.
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent()
		{
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.treeView1.ImageIndex = -1;
			this.treeView1.Name = "treeView1";
			this.treeView1.SelectedImageIndex = -1;
			this.treeView1.Size = new System.Drawing.Size(440, 328);
			this.treeView1.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.button1.Location = new System.Drawing.Point(360, 384);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 24);
			this.button1.TabIndex = 1;
			this.button1.Text = "&Clear";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.label1.Location = new System.Drawing.Point(8, 344);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(344, 72);
			this.label1.TabIndex = 2;
			this.label1.Text = "This window shows the services that have been requested by the designer.  There a" +
				"re two top-level categories:  Successful and Unsuccessful.  Unsuccessful service" +
				" requests result in degraded designer functionality.";
			// 
			// ServiceRequests
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 422);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label1,
																		  this.button1,
																		  this.treeView1});
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ServiceRequests";
			this.Text = "ServiceRequests";
			this.ResumeLayout(false);

		}
		#endregion

		/// Clear the trees.
		private void button1_Click(object sender, System.EventArgs e)
		{
			treeView1.Nodes.Clear();
			InitializeTreeViewNodes();
			if (serviceRequests != null) 
			{
				serviceRequests.Clear();
			}
		}
	}
}
