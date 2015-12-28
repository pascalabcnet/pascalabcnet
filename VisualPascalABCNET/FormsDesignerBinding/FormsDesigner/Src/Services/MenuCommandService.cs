using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ICSharpCode.FormsDesigner.Services
{
	/// IMenuCommandService implementation.
	public class MenuCommandService : System.ComponentModel.Design.IMenuCommandService
	{
		// the host
		private IDesignerHost host;
		// commandId-command mapping
		private IDictionary commands;
		// menuItem-verb mapping
		private Dictionary<ToolStripMenuItem, DesignerVerb> menuItemVerb;
		// the global verbs collection. 
		private DesignerVerbCollection globalVerbs; 
		// we use the same context menu over-and-over
		private ContextMenuStrip contextMenu;
		// we keep the lastSelectedComponent around 
		private IComponent lastSelectedComponent;

		public MenuCommandService(IDesignerHost host)
		{
			this.host = host;
			commands = new Hashtable();
			globalVerbs = new DesignerVerbCollection();
			menuItemVerb = new Dictionary<ToolStripMenuItem,DesignerVerb>();
			contextMenu = new ContextMenuStrip();
			lastSelectedComponent = null;
            //roman//
            this.AddCommand(new MenuCommand(ShowProperties, StandardCommands.PropertiesWindow));
            this.AddCommand(new MenuCommand(ViewCode, StandardCommands.ViewCode));
		}

        //roman//
        public void ShowProperties(object sender, EventArgs e)
        {
            VisualPascalABC.VisualPABCSingleton.MainForm.PropertiesWindowVisible = true;
        }

        public void ViewCode(object sender, EventArgs e)
        {
            if (VisualPascalABC.VisualPABCSingleton.MainForm._currentCodeFileDocument.DesignerAndCodeTabs.SelectedTab != null)
            {
                //VisualPascalABC.Form1.Form1_object.CahngedSelectedTab();
                VisualPascalABC.VisualPABCSingleton.MainForm._currentCodeFileDocument.DesignerAndCodeTabs.SelectedTab =
                           VisualPascalABC.VisualPABCSingleton.MainForm._currentCodeFileDocument.TextPage;
            }
        }
        //roman//

		#region Implementation of IMenuCommandService

		/// called to add a MenuCommand
		public void AddCommand(System.ComponentModel.Design.MenuCommand command)
		{
			if (command == null)
			{
				throw new ArgumentException("command");
			}
			// don't add commands twice
			if (FindCommand(command.CommandID) == null)
			{
				commands.Add(command.CommandID, command);
			}
		}

		/// called to remove a MenuCommand
		public void RemoveCommand(System.ComponentModel.Design.MenuCommand command)
		{
			if(command==null)
			{
				throw new ArgumentException("command");
			}
			commands.Remove(command.CommandID);
		}

		/// called when to add a global verb
		public void AddVerb(System.ComponentModel.Design.DesignerVerb verb)
		{
			if(verb==null)
			{
				throw new ArgumentException("verb");
			}
			globalVerbs.Add(verb);
			// create a menu item for the verb and add it to the context menu
            ToolStripMenuItem menuItem = new ToolStripMenuItem(verb.Text);
			menuItem.Click += new EventHandler(MenuItemClickHandler);
			menuItemVerb.Add(menuItem,verb);
			contextMenu.Items.Add(menuItem);
		}
		/// called to remove global verb
		public void RemoveVerb(System.ComponentModel.Design.DesignerVerb verb)
		{
			if(verb==null)
			{
				throw new ArgumentException("verb");
			}
			
			globalVerbs.Remove(verb);

			// find the menu item associated with the verb
			ToolStripMenuItem associatedMenuItem = null;
			foreach(KeyValuePair<ToolStripMenuItem, DesignerVerb> de in menuItemVerb)
			{
				if(de.Value==verb)
				{
					associatedMenuItem=de.Key as ToolStripMenuItem;
					break;
				}
			}
			// if we found the verb's menu item, remove it
			if(associatedMenuItem!=null)
			{
				menuItemVerb.Remove(associatedMenuItem);
			}
			// remove the verb from the context menu too
			contextMenu.Items.Remove(associatedMenuItem);
		}
		/// returns the MenuCommand associated with the commandId.
		public System.ComponentModel.Design.MenuCommand FindCommand(System.ComponentModel.Design.CommandID commandID)
		{
			return commands[commandID] as MenuCommand;
		}

		/// called to invoke a command
		public bool GlobalInvoke(System.ComponentModel.Design.CommandID commandID)
		{
			bool result = false;
			MenuCommand command = FindCommand(commandID);
			if (command != null)
			{
				command.Invoke();
				result = true;
			}
			return result;
		}

		/// called to show the context menu for the selected component.
		public void ShowContextMenu(System.ComponentModel.Design.CommandID menuID, int x, int y)
		{
			ISelectionService selectionService = host.GetService(typeof(ISelectionService)) as ISelectionService;
			// get the primary component
			IComponent primarySelection = selectionService.PrimarySelection as IComponent;
			// if the he clicked on the same component again then just show the context
			// menu. otherwise, we have to throw away the previous
			// set of local menu items and create new ones for the newly
			// selected component
			if (lastSelectedComponent != primarySelection)
			{
				// remove all non-global menu items from the context menu
				ResetContextMenu();
				// get the designer
				IDesigner designer = host.GetDesigner(primarySelection);
				// not all controls need a desinger
				if(designer!=null)
				{
					// get designer's verbs
					DesignerVerbCollection verbs = designer.Verbs;
					foreach (DesignerVerb verb in verbs)
					{
						// add new menu items to the context menu
						CreateAndAddLocalVerb(verb);
					}
				}
			}
			// we only show designer context menus for controls

            Control comp = primarySelection as Control; //roman//
            if (comp == null)
                comp = VisualPascalABC.VisualPABCSingleton.MainForm;

            ToolStripMenuItem[] menuItems1 = new ToolStripMenuItem[VisualPascalABC.VisualPABCSingleton.MainForm.cm_Designer.Items.Count];
            ToolStripMenuItem[] menuItems2 = new ToolStripMenuItem[contextMenu.Items.Count];
            VisualPascalABC.VisualPABCSingleton.MainForm.cm_Designer.Items.CopyTo(menuItems1, 0);
            contextMenu.Items.CopyTo(menuItems2, 0);
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.AddRange(menuItems1);
            if (menuItems2.Length > 0)
                menu.Items.Add(new ToolStripSeparator());
            menu.Items.AddRange(menuItems2);
			Point pt = comp.PointToScreen(new Point(0, 0));
            menu.Show(comp, new Point(x - pt.X, y - pt.Y));
            /*Следующее делается по следующей причине:
                * почему то при menu.Items.AddRange(menuItems1); удаляются элементы из VisualPascalABC.Form1.Form1_object.cm_Designer,
                * аналогично для menu.Items.AddRange(menuItems2);
                */
            menu.Closed += delegate   
            {
                VisualPascalABC.VisualPABCSingleton.MainForm.cm_Designer.Items.Clear();
                contextMenu.Items.Clear();
                VisualPascalABC.VisualPABCSingleton.MainForm.cm_Designer.Items.AddRange(menuItems1);
                contextMenu.Items.AddRange(menuItems2);
                return;
            };
		    // keep the selected component for next time
		    lastSelectedComponent = primarySelection;
		}

		/// returns the the current designer verbs
		public System.ComponentModel.Design.DesignerVerbCollection Verbs
		{
			get
			{
				// create a new collection
				DesignerVerbCollection availableVerbs = new DesignerVerbCollection();
				// add the global verbs
				if(globalVerbs!=null && globalVerbs.Count>0)
				{
					availableVerbs.AddRange(globalVerbs);
				}
				// now add the local verbs
				ISelectionService selectionService = host.GetService(typeof(ISelectionService)) as ISelectionService;
				IComponent primaryComponent = selectionService.PrimarySelection as IComponent;
				if(primaryComponent!=null)
				{
					IDesigner designer = host.GetDesigner(primaryComponent);
					if(designer!=null && designer.Verbs!=null && designer.Verbs.Count>0)
					{
						availableVerbs.AddRange(designer.Verbs);
					}
				}
				return availableVerbs;
			}
		}

		#endregion

		/// called to invoke menu item verbs
		private void MenuItemClickHandler(object sender, EventArgs e)
		{
			// get the menu item
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
			if(menuItem!=null)
			{
				// get and invoke the verb
				DesignerVerb verb = menuItemVerb[menuItem] as DesignerVerb;
				if(verb!=null)
				{
					try
					{
						verb.Invoke();
					}
					catch{}
				}
			}
		}
		/// removes all local verbs from the context menu 
		private void ResetContextMenu()
		{
			if(contextMenu!=null && contextMenu.Items!=null && contextMenu.Items.Count>0)
			{
                ToolStripMenuItem[] menuItemArray = new ToolStripMenuItem[contextMenu.Items.Count];
				contextMenu.Items.CopyTo(menuItemArray,0);
				foreach(ToolStripMenuItem menuItem in menuItemArray)
				{
					// if its not in the global list, remove it
					if(!IsInGlobalList(menuItem.Text))
					{
						contextMenu.Items.Remove(menuItem);
					}
					// get rid of the menu item from the mapping
					menuItemVerb.Remove(menuItem);
				}
			}
		}
		/// removes a local verb
		private void RemoveLocalVerb(DesignerVerb verb)
		{
			if(verb==null)
			{
				throw new ArgumentException("verb");
			}
			// get the associated menuItem 
			ToolStripMenuItem menuItem = GetMenuItemForVerb(verb);
			if(menuItem!=null)
			{
				// undo mapping
				menuItemVerb.Remove(menuItem);
				// remove from context menu
				contextMenu.Items.Remove(menuItem);
			}			
		}
		/// creats and adds a local verb
		private void CreateAndAddLocalVerb(DesignerVerb verb)
		{
			if(verb==null)
			{
				throw new ArgumentException("verb");
			}
			VerifyVerb(verb);
			// create a menu item for the verb
            ToolStripMenuItem menuItem = new ToolStripMenuItem(verb.Text);
			// attach the menu item click listener
			menuItem.Click += new EventHandler(MenuItemClickHandler);
			// do the menuItem-verb mapping
			menuItemVerb.Add(menuItem,verb);
			// add to context menu 
			contextMenu.Items.Add(menuItem);
		}
		/// returns the MenuItem associated with the verb
		private ToolStripMenuItem GetMenuItemForVerb(DesignerVerb verb)
		{
			ToolStripMenuItem menuItem = null;
			if(menuItemVerb!=null && menuItemVerb.Count>0)
			{
				foreach(KeyValuePair<ToolStripMenuItem, DesignerVerb> de in menuItemVerb)
				{
					DesignerVerb dv = de.Value as DesignerVerb;
					if(dv==verb)
					{
						menuItem = de.Key as ToolStripMenuItem;
						break;
					}
				}
			}
			return menuItem;
		}
		/// returns true if the verb is in the global verb collection
		private bool IsInGlobalList(string verbText)
		{
			bool found = false;
			if(globalVerbs!=null && globalVerbs.Count>0)
			{
				foreach(DesignerVerb dv in globalVerbs)
				{
					if(string.Compare(dv.Text,verbText,true)==0)
					{
						found=true;
						break;
					}
				}
			}
			return found;
		}
		/// we can't add the same verb twice
		private void VerifyVerb(DesignerVerb verb)
		{
			if(verb==null)
			{
				throw new ArgumentException("verb");
			}
			// make sure the verb is not in the global list
			if(globalVerbs!=null && globalVerbs.Count>0)
			{
				foreach(DesignerVerb dv in globalVerbs)
				{
					if(string.Compare(dv.Text,verb.Text,true)==0)
					{
						throw new Exception("Cannot add the same verb twice.");
					}
				}
			}
			// now check the menuItemVerb mapping 
			if(menuItemVerb!=null && menuItemVerb.Count>0)
			{
				foreach(DesignerVerb dv in menuItemVerb.Values)
				{
					if(string.Compare(dv.Text,verb.Text,true)==0)
					{
						throw new Exception("Cannot add the same verb twice.");
					}
				}
			}
		}
	}
}
