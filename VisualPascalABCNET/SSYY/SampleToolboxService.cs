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
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace SampleDesignerHost
{
	/// The IToolboxService is responsible for communication between the designer
	/// and whatever implementation of a toolbox you have. This service can be
	/// its own class (as it is here) or it can be implemented on the actual
	/// toolbox control.
	public class SampleToolboxService : System.Drawing.Design.IToolboxService
	{
		private IDesignerHost host;
		private SampleDesignerApplication.ToolBoxPane toolbox;

		public SampleToolboxService(IDesignerHost host)
		{
			this.host = host;

			// Our MainForm adds our ToolboxPane to the host's services.
			toolbox = host.GetService(typeof(SampleDesignerApplication.ToolBoxPane)) as SampleDesignerApplication.ToolBoxPane;
		}
		#region Implementation of IToolboxService

		/// Add a ToolboxItem to our toolbox, in a specific category, bound to a certain host.
		public void AddLinkedToolboxItem(ToolboxItem toolboxItem, string category, System.ComponentModel.Design.IDesignerHost host)
		{
			// UNIMPLEMENTED - We didn't end up doing a project system, so there's no need
			// to add custom tools (despite that we do have a tab for such tools).
		}

		/// Add a ToolboxItem to our toolbox, bound to a certain host.
		void System.Drawing.Design.IToolboxService.AddLinkedToolboxItem(ToolboxItem toolboxItem, System.ComponentModel.Design.IDesignerHost host)
		{
			// UNIMPLEMENTED - We didn't end up doing a project system, so there's no need
			// to add custom tools (despite that we do have a tab for such tools).
		}

		/// We deserialize a ToolboxItem when we drop it onto our design surface.
		/// The ToolboxItem comes packaged in a DataObject. We're just working
		/// with standard tools and one host, so the host parameter is ignored.
		public ToolboxItem DeserializeToolboxItem(object serializedObject, System.ComponentModel.Design.IDesignerHost host)
		{
			return (ToolboxItem)((DataObject)serializedObject).GetData(typeof(ToolboxItem));
		}

		/// We deserialize a ToolboxItem when we drop it onto our design surface.
		/// The ToolboxItem comes packaged in a DataObject.
		ToolboxItem System.Drawing.Design.IToolboxService.DeserializeToolboxItem(object serializedObject)
		{
			return this.DeserializeToolboxItem(serializedObject, this.host);
		}

		/// We serialize a toolbox by packaging it in a DataObject. Simple!
		public object SerializeToolboxItem(ToolboxItem toolboxItem)
		{
			return new DataObject(toolboxItem);
		}

		/// Remove a ToolboxItem from the specified category in our toolbox.
		public void RemoveToolboxItem(ToolboxItem toolboxItem, string category)
		{
			// UNIMPLEMENTED - Our ToolboxPane handles its own creation and modification
			// well enough, and our tool list stays constant. However, if you want a tool
			// list to have Add/Remove capability, here's the best place to do it.
		}

		/// Remove a ToolboxItem from our toolbox.
		void System.Drawing.Design.IToolboxService.RemoveToolboxItem(ToolboxItem toolboxItem)
		{
			// UNIMPLEMENTED - Our ToolboxPane handles its own creation and modification
			// well enough, and our tool list stays constant. However, if you want a tool
			// list to have Add/Remove capability, here's the best place to do it.
		}

		/// If we've got a tool selected, then perhaps we want to set our cursor to do
		/// something interesting when its over the design surface. If we do, then
		/// we do it here and return true. Otherwise we return false so the caller
		/// can set the cursor in some default manor.
		public bool SetCursor()
		{
			if (toolbox.SelectedTool != null && toolbox.SelectedTool.DisplayName != "<Pointer>") // <Pointer> is not a tool.
			{
				Cursor.Current = Cursors.Cross;
				return true;
			}
			return false;
		}

		/// Add a ToolboxItem to our toolbox under the specified category.
		public void AddToolboxItem(ToolboxItem toolboxItem, string category)
		{
			// UNIMPLEMENTED - Our ToolboxPane handles its own creation and modification
			// well enough, and our tool list stays constant. However, if you want a tool
			// list to have Add/Remove capability, here's the best place to do it.
		}

		/// Add a ToolboxItem to our toolbox.
		void System.Drawing.Design.IToolboxService.AddToolboxItem(ToolboxItem toolboxItem)
		{
			// UNIMPLEMENTED - Our ToolboxPane handles its own creation and modification
			// well enough, and our tool list stays constant. However, if you want a tool
			// list to have Add/Remove capability, here's the best place to do it.
		}

		/// Add a creator that will convert non-standard tools in the specified format into ToolboxItems, to be associated with a host.
		public void AddCreator(System.Drawing.Design.ToolboxItemCreatorCallback creator, string format, System.ComponentModel.Design.IDesignerHost host)
		{
			// UNIMPLEMENTED - We aren't handling any non-standard tools here. Our toolset is constant.
		}

		/// Add a creator that will convert non-standard tools in the specified format into ToolboxItems.
		void System.Drawing.Design.IToolboxService.AddCreator(System.Drawing.Design.ToolboxItemCreatorCallback creator, string format)
		{
			// UNIMPLEMENTED - We aren't handling any non-standard tools here. Our toolset is constant.
		}

		/// Remove the creator for the specified format, associated with a particular host.
		public void RemoveCreator(string format, System.ComponentModel.Design.IDesignerHost host)
		{
			// UNIMPLEMENTED - We aren't handling any non-standard tools here. Our toolset is constant.
		}

		/// Remove the creator for the specified format.
		void System.Drawing.Design.IToolboxService.RemoveCreator(string format)
		{
			// UNIMPLEMENTED - We aren't handling any non-standard tools here. Our toolset is constant.
		}

		///  Get all the tools in a category.
		public ToolboxItemCollection GetToolboxItems(string category, System.ComponentModel.Design.IDesignerHost host)
		{
			return toolbox.GetToolsFromCategory(category);
		}

		/// Get all of the tools.
		ToolboxItemCollection System.Drawing.Design.IToolboxService.GetToolboxItems(string category)
		{
			return this.GetToolboxItems(category, this.host);
		}

		/// Get all of the tools. We're always using our current host though.
		ToolboxItemCollection System.Drawing.Design.IToolboxService.GetToolboxItems(System.ComponentModel.Design.IDesignerHost host)
		{
			return toolbox.GetAllTools();
		}

		/// Get all of the tools.
		ToolboxItemCollection System.Drawing.Design.IToolboxService.GetToolboxItems()
		{
			return toolbox.GetAllTools();
		}

		/// Our toolbox has categories akin to those of Visual Studio, but you
		/// could group them any which way. Just make sure your IToolboxService knows.
		public CategoryNameCollection CategoryNames
		{
			get
			{
				return toolbox.CategoryNames;
			}
		}

		/// Return the selected ToolboxItem in our toolbox if it is associated with this host.
		/// Since all of our tools are associated with our only host, the host parameter
		/// is ignored.
		public ToolboxItem GetSelectedToolboxItem(System.ComponentModel.Design.IDesignerHost host)
		{
			if (toolbox.SelectedTool.DisplayName != "<Pointer>")
			{
				return toolbox.SelectedTool;
			}
			return null;
		}

		/// Return the selected ToolboxItem in our toolbox.
		ToolboxItem System.Drawing.Design.IToolboxService.GetSelectedToolboxItem()
		{
			return this.GetSelectedToolboxItem(this.host);
		}

		/// Set the selected ToolboxItem in our toolbox.
		public void SetSelectedToolboxItem(ToolboxItem toolboxItem)
		{
			// UNIMPLEMENTED - We never had a need to do this, since
			// our toolbox handles input and selection all by itself.
		}

		/// Refreshes the toolbox.
		public void Refresh()
		{
			toolbox.Refresh();
		}

		/// We are always using standard ToolboxItems, so they are always supported.
		public bool IsSupported(object serializedObject, System.Collections.ICollection filterAttributes)
		{
			return true;
		}

		/// We are always using standard ToolboxItems, so they are always supported.
		bool System.Drawing.Design.IToolboxService.IsSupported(object serializedObject, System.ComponentModel.Design.IDesignerHost host)
		{
			return true;
		}

		/// This gets called after our IToolboxUser (the designer) ToolPicked method is called.
		/// In our case, we select the pointer. 
		public void SelectedToolboxItemUsed()
		{
			toolbox.SelectPointer();
		}

		/// Check if a serialized object is a ToolboxItem. In our case, all of our tools
		/// are standard and from a constant set, and they all extend ToolboxItem, so if
		/// we can deserialize it in our standard-way, then it is indeed a ToolboxItem.
		/// The host is ignored.
		public bool IsToolboxItem(object serializedObject, System.ComponentModel.Design.IDesignerHost host)
		{
			// If we can deserialize it, it's a ToolboxItem.
			if (this.DeserializeToolboxItem(serializedObject, host) != null)
			{
				return true;
			}
			return false;
		}

		/// Check if a serialized object is a ToolboxItem. In our case, all of our tools
		/// are standard and from a constant set, and they all extend ToolboxItem, so if
		/// we can deserialize it in our standard-way, then it is indeed a ToolboxItem.
		bool System.Drawing.Design.IToolboxService.IsToolboxItem(object serializedObject)
		{
			return IsToolboxItem(serializedObject, this.host);
		}

		/// If your toolbox is categorized, then it's good for others to know
		/// which category is selected.
		public string SelectedCategory
		{
			get
			{
				return toolbox.SelectedCategory;
			}
			set
			{
				// UNIMPLEMENTED - We don't give our IToolboxService control
				// over tab selection.
			}
		}

		#endregion
	}
}
