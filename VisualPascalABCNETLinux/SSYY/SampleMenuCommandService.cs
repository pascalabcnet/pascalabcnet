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
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Drawing;

namespace SampleDesignerHost
{
	/// The IMenuCommandService keeps track of MenuCommands and Designer Verbs
	/// that are available at any single moment. It can invoke these commands
	/// and also handles the displaying of ContextMenus for designers that support them.
	public class SampleMenuCommandService : System.ComponentModel.Design.IMenuCommandService
	{
		private Hashtable commands; // added MenuCommands
		private Hashtable verbsFromMenuItems; // DesignerVerb values mapped to MenuItem keys
		private Hashtable menuItemsFromVerbs; // MenuItem values mapped to DesignerVerb keys
		private ArrayList globalVerbs; // verbs currently available to all designers
		private IDesignerHost host;
		private ContextMenu cm;
		private IComponent lastSelection; // needed to clean up local verbs from a previous selection
        private DefaultMenuCommands defaultCommands;

		public SampleMenuCommandService(IDesignerHost host)
		{
			this.host = host;
		}

		/// The IMenuCommandService deals with two kinds of verbs: 1) local verbs specific
		/// to each designer (i.e. Add/Remove Tab on a TabControl) which are added
		/// and removed on-demand, each time a designer is right-clicked, 2) the rarer
		/// global verbs, which once added are available to all designers,
		/// until removed. This method (not a standard part of IMenuCommandService) is used
		/// to add a local verb. If the verb is already in our global list, we don't add it 
		/// again. It is called through IMenuCommandService.ShowContextMenu().
		public void AddLocalVerb(DesignerVerb verb)
		{
			if ((globalVerbs == null) || (!globalVerbs.Contains(verb)))
			{
				if (cm == null)
				{
					cm = new ContextMenu();
					verbsFromMenuItems = new Hashtable();
					menuItemsFromVerbs = new Hashtable();
				}

				// Verbs and MenuItems are dually mapped to each other, so that we can
				// check for association given either half of the pair. All of our MenuItems
				// use the same event handler, but we can check the event sender to see
				// what verb to invoke. MenuItems like to only be assigned to one Menu in their
				// lifetime, so we have to create a single ContextMenu and use that thereafter.
				// If we were to instead create a ContextMenu every time we need to show one,
				// the MenuItems' click events might not work properly.
				//
				MenuItem menuItem = new MenuItem(verb.Text);
				menuItem.Click += new EventHandler(menuItem_Click);
				verbsFromMenuItems.Add(menuItem, verb);
				menuItemsFromVerbs.Add(verb, menuItem);
				cm.MenuItems.Add(menuItem);
			}
		}

		/// Remove a local verb, but only if it isn't in our global list.
		/// It is also called through IMenuCommandService.ShowContextMenu().
		public void RemoveLocalVerb(DesignerVerb verb)
		{
			if ((globalVerbs == null) || (!globalVerbs.Contains(verb)))
			{
				if (cm != null)
				{
					// Remove the verb and its mapped MenuItem from our tables and menu.
					MenuItem key = menuItemsFromVerbs[verb] as MenuItem;
					verbsFromMenuItems.Remove(key);
					menuItemsFromVerbs.Remove(verb);
					cm.MenuItems.Remove(key);
				}
			}				
		}

		#region Implementation of IMenuCommandService

		/// If a designer supports a MenuCommand, it will add it here.
		public void AddCommand(System.ComponentModel.Design.MenuCommand command)
		{
			if (commands == null)
			{
				commands = new Hashtable();
                defaultCommands = new DefaultMenuCommands(host as IServiceProvider);
                defaultCommands.AddTo(this);
			}

			// Only add a command if we haven't already.
			if (FindCommand(command.CommandID) == null)
			{
				commands.Add(command.CommandID, command);
			}
		}

		/// This is only called by external callers if someone wants to
		/// remove a verb that is available for all designers. We keep track of
		/// such verbs to make sure that they are not re-added or removed
		/// when we frequently manipulate our local verbs.
		public void RemoveVerb(System.ComponentModel.Design.DesignerVerb verb)
		{
			if (globalVerbs != null)
			{
				globalVerbs.Remove(verb);

				if (cm != null)
				{
					// Remove the verb and its mapped MenuItem from our tables and menu.
					MenuItem key = menuItemsFromVerbs[verb] as MenuItem;
					verbsFromMenuItems.Remove(key);
					menuItemsFromVerbs.Remove(verb);
					cm.MenuItems.Remove(key);
				}
			}
		}

		/// If a command is no longer viable for a designer, it is removed.
		public void RemoveCommand(System.ComponentModel.Design.MenuCommand command)
		{
			if (commands != null)
			{
				// Hashtable already has nothing happen if the command isn't there.
				commands.Remove(command.CommandID);
			}
		}

		/// Find a command based on its CommandID.
		public System.ComponentModel.Design.MenuCommand FindCommand(System.ComponentModel.Design.CommandID commandID)
		{
			if (commands != null)
			{
				MenuCommand command = commands[commandID] as MenuCommand;
				if (command != null)
				{
					return command;
				}
			}
			return null;
		}

		/// We only invoke commands that have been added.
		public bool GlobalInvoke(System.ComponentModel.Design.CommandID commandID)
		{
			MenuCommand command = FindCommand(commandID);
			if (command != null)
			{
				command.Invoke();
				return true;
			}
			return false;
		}

		/// This is called whenever the user right-clicks on a designer. It removes any local verbs
		/// added by a previous, different selection and adds the local verbs for the current (primary)
		/// selection. Then it displays the ContextMenu.
		public void ShowContextMenu(System.ComponentModel.Design.CommandID menuID, int x, int y)
		{
			ISelectionService ss = host.GetService(typeof(ISelectionService)) as ISelectionService;

			// If this is the same component as was last right-clicked on, then we don't need to
			// make any changes to our collection of local verbs.
			//
			if ((lastSelection != null) && (lastSelection != ss.PrimarySelection))
			{
                IDesigner d = host.GetDesigner(lastSelection);
                if (d != null)
                {
                    foreach (DesignerVerb verb in d.Verbs)
                    {
                        RemoveLocalVerb(verb);
                    }
                }
			}

			// Update the local verbs for the new selection, if it is indeed new.
			if (lastSelection != ss.PrimarySelection)
			{
                IDesigner d = host.GetDesigner(ss.PrimarySelection as IComponent);
                if (d.Verbs.Count == 0)
                {
                    FillVerbs(ss.PrimarySelection as IComponent, d);                
                }
				foreach (DesignerVerb verb in d.Verbs)
				{
					AddLocalVerb(verb);
				}
			}

			// Display our ContextMenu! Note that the coordinate parameters to this method
			// are in screen coordinates, so we've got to translate them into client coordinates.
			//
			//if (cm != null) 
			{
				Control ps = ss.PrimarySelection as Control;
                if (ps == null)
                {
                    ps = VisualPascalABC.Form1.Form1_object;
                }
				Point s = ps.PointToScreen(new Point(0, 0));
				VisualPascalABC.Form1.Form1_object.cm_Designer.Show(ps, new Point(x - s.X, y - s.Y));
                //VisualPascalABC.Form1.Form1_object.cm_Designer.Show(ps, Cursor.Position);
            }

			// ss.PrimarySelection might be old news by the next right-click. We need to 
			// be able to remove its verbs if we get a different selection next time
			// this method is called. We can't remove them right now because we aren't sure
			// if the MenuItem click events have finished yet (and removing verbs kills
			// their MenuItem mapping). So we save the selection and do it next time if necessary.
			//
			lastSelection = ss.PrimarySelection as IComponent;
		}

        public void FillVerbs(IComponent comp, IDesigner d)
        {
            //DesignerVerb dv = new DesignerVerb("horiz", new EventHandler(VisualPascalABC.Form1.Form1_object.ExecSendToBack));
            
            //d.Verbs.Add(dv);
        }

		/// This is only called by external callers if someone wants to
		/// remove a verb that is available for all designers. We keep track of
		/// such verbs to make sure that they are not re-added or removed
		/// when we frequently manipulate our local verbs.
		public void AddVerb(System.ComponentModel.Design.DesignerVerb verb)
		{
			if (globalVerbs == null)
			{
				globalVerbs = new ArrayList();
			}

			globalVerbs.Add(verb);

			if (cm == null)
			{
				cm = new ContextMenu();
				verbsFromMenuItems = new Hashtable();
				menuItemsFromVerbs = new Hashtable();
			}

			// Verbs and MenuItems are dually mapped to each other, so that we can
			// check for association given either half of the pair. All of our MenuItems
			// use the same event handler, but we can check the event sender to see
			// what verb to invoke. We have to have a stable ContextMenu, and we need
			// to add the MenuItems to it now. If we try to add the MenuItems on-demand
			// right before we show a ContextMenu, the clicks events won't work.
			//
			MenuItem menuItem = new MenuItem(verb.Text);
			menuItem.Click += new EventHandler(menuItem_Click);
			verbsFromMenuItems.Add(menuItem, verb);
			menuItemsFromVerbs.Add(verb, menuItem);
			cm.MenuItems.Add(menuItem);
		}

		/// This is frequently called to get the current list of available verbs. We have
		/// to be careful in what verbs we return, however, because the information contained
		/// in menuItemsFromVerbs and verbsFromMenuItems will not be current if there has been
		/// no right-click since the last selection change. Thus we return the global verbs
		/// plus any additional local verbs for the current selection.
		public System.ComponentModel.Design.DesignerVerbCollection Verbs
		{
			get
			{
				ISelectionService ss = host.GetService(typeof(ISelectionService)) as ISelectionService;
				ArrayList currentVerbs;
				if (globalVerbs != null)
				{
					currentVerbs = new ArrayList(globalVerbs);
				}
				else
				{
					currentVerbs = new ArrayList();
				}

				foreach (DesignerVerb verb in host.GetDesigner(ss.PrimarySelection as IComponent).Verbs)
				{
					if (!currentVerbs.Contains(verb))
					{
						currentVerbs.Add(verb);
					}
				}

				if (currentVerbs.Count > 0)
				{
					DesignerVerb[] ret = new DesignerVerb[currentVerbs.Count];
					currentVerbs.CopyTo(ret);
					return new DesignerVerbCollection(ret);
				}
				else
				{
					return new DesignerVerbCollection();
				}
			}
		}

		#endregion

		/// All of our MenuItems' click events are handled by this method. It checks the
		/// mapping to see which verb is associated with the MenuItem that sent the event
		/// and then invokes that verb.
		private void menuItem_Click(object sender, EventArgs e)
		{
			MenuItem key = sender as MenuItem;
			DesignerVerb v = verbsFromMenuItems[key] as DesignerVerb;
			v.Invoke();
		}
	}
}
