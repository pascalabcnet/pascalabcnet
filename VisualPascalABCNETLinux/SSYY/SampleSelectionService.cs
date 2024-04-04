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
namespace SampleDesignerHost
{
    using System;
	using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Collections;
    using System.Windows.Forms;
	using System.Windows.Forms.Design;

    using System.Diagnostics;

	/// This class keeps track of the selected component(s) and provides events
	/// to notify about selection changes.
    internal class SampleSelectionService : ISelectionService {

        // These constitute the current selection at any moment.
        private SampleSelectionItem           primarySelection;         // the primary selection
        private Hashtable                     selectionsByComponent;    // hashtable of selections

        // Hookups to other services
        private IDesignerHost           host;                     // The host interface
        private IContainer              container;                // The container we're showing selection for
        
        // These are used when the host is in batch mode: we want to queue up selection
        // changes in this case.
        private bool                 batchMode;                // Are we in batch mode?
        private bool                 selectionChanged;         // true, if the selection changed in batch mode
        private bool                 selectionContentsChanged; // true, if the selection contents changed in batch mode
        public bool need_clear_selection = false;
        
        internal SampleSelectionService(IDesignerHost host) {
            this.host = host;
            this.container = host.Container;
            this.selectionsByComponent = new Hashtable();
            this.selectionChanged = false;

            // We initialize this to true to catch anything that would cause
            // an update during load.
            this.batchMode = true;

            // And configure the events we want to listen to.
            IComponentChangeService cs = (IComponentChangeService)host.GetService(typeof(IComponentChangeService));
            Debug.Assert(cs != null, "IComponentChangeService not found");
            if (cs != null) {
                cs.ComponentAdded += new ComponentEventHandler(this.DesignerHost_ComponentAdd);
                cs.ComponentRemoved += new ComponentEventHandler(this.DesignerHost_ComponentRemove);
                cs.ComponentChanged += new ComponentChangedEventHandler(this.DesignerHost_ComponentChanged);
            }
            
            host.TransactionOpened += new EventHandler(this.DesignerHost_TransactionOpened);
            host.TransactionClosed += new DesignerTransactionCloseEventHandler(this.DesignerHost_TransactionClosed);
            
			// If our host is already in a transaction, we handle it.
			if (host.InTransaction) {
                DesignerHost_TransactionOpened(host, EventArgs.Empty);
            }

            host.LoadComplete += new EventHandler(this.DesignerHost_LoadComplete);
        }

        // Properties

		/// The primary selection is the last one to have been selected.
        object ISelectionService.PrimarySelection { 
            get {
                if (primarySelection == null) {
                    IDictionaryEnumerator selections = (IDictionaryEnumerator)selectionsByComponent.GetEnumerator();
                    bool valueFound = selections.MoveNext();

                    if (valueFound) {
                        primarySelection = (SampleSelectionItem)selections.Value;
                        primarySelection.Primary = true;
                    }
                }

                if (primarySelection != null) {
                    return primarySelection.Component;
                }
                
                return null;
            }
        }
        
		/// Return the number of components we have selected.
        int ISelectionService.SelectionCount { 
            get {
                return selectionsByComponent.Count;
            }
        }

        // Events
        public event EventHandler SelectionChanged;
        public event EventHandler SelectionChanging;

        // Methods

		/// Check to see if the given component is in our selection group.
        bool ISelectionService.GetComponentSelected(object component) {
            return (component != null && null != selectionsByComponent[component]);
        }

		/// Return our list of selected components.
        ICollection ISelectionService.GetSelectedComponents() {
            object[] sels = new object[selectionsByComponent.Values.Count];
            selectionsByComponent.Values.CopyTo(sels, 0);
            object[] objects = new object[sels.Length];

            for (int i = 0; i < sels.Length; i++) {
                objects[i] = ((SampleSelectionItem)sels[i]).Component;
            }

            return objects;
        }

		/// Select the given components.
        void ISelectionService.SetSelectedComponents(ICollection components) {
            ((ISelectionService)this).SetSelectedComponents(components, SelectionTypes.Normal);
        }

		/// Select the given components with the given SelectionType.
        void ISelectionService.SetSelectedComponents(ICollection components, SelectionTypes selectionType) {
            bool fToggle = false;
            bool fControl = false;
            bool fClick  = false;
            bool fChanged = false;  // did we actually change something?

            // Catch a null input list
            if (components == null){
                components = new Component[0];
            }

            if ((selectionType & SelectionTypes.Normal) == SelectionTypes.Normal
                || (selectionType & SelectionTypes.Click) == SelectionTypes.Click) {

                fControl = ((Control.ModifierKeys & Keys.Control) == Keys.Control);

                // Only toggle when we are affecting a single control, and
                // when we are handling the "mouse" state events (i.e. up/down
                // used to show/hide the selection).
                //
                fToggle = ((Control.ModifierKeys & Keys.Control) != 0 || (Control.ModifierKeys & Keys.Shift) != 0)
                          && components.Count == 1
                          && (selectionType & SelectionTypes.MouseUp) != SelectionTypes.MouseUp;
            }

            if ((selectionType & SelectionTypes.Click) == SelectionTypes.Click) {
                fClick = true;
            }


            // If we are replacing the selection, only remove the ones that are not in our new list.
            // We also handle the special case here of having a singular component selected that's
            // already selected.  In this case we just move it to the primary selection.
            //
            if (!fToggle && !fControl) {
                object firstSelection = null;
                foreach(object o in components) {
                    firstSelection = o;
                    break;
                }
                
                if (fClick && 1 == components.Count && ((ISelectionService)this).GetComponentSelected(firstSelection)) {
                    SampleSelectionItem oldPrimary = primarySelection;
                    SetPrimarySelection((SampleSelectionItem)selectionsByComponent[firstSelection]);
                    if (oldPrimary != primarySelection) {
                        fChanged = true;
                    }
                }
                else {
                    SampleSelectionItem[] selections = new SampleSelectionItem[selectionsByComponent.Values.Count];
                    selectionsByComponent.Values.CopyTo(selections, 0);                    

                    // Even with several hundred components this should be fairly fast
                    foreach(SampleSelectionItem item in selections) {
                        bool remove = true;
                    
                        foreach(object comp in components) {
                            if (comp == item.Component) {
                                remove = false;
                                break;
                            }
                        }
                        
                        if (remove) {
                            RemoveSelection(item);
                            fChanged = true;
                        }
                    }
                }
            }

            SampleSelectionItem primarySel = null;
            int selectedCount = selectionsByComponent.Count;

            // Now do the selection.
            foreach(Component comp in components) {
                if (comp != null) {
                    SampleSelectionItem  s = (SampleSelectionItem)selectionsByComponent[comp];
    
                    if (null == s) {
                        s = new SampleSelectionItem(this, comp);
                        AddSelection(s);

                        if (fControl || fToggle) {
                            primarySel = s;
                        }

                        fChanged = true;
                    }
                    else {
                        if (fToggle) {
                            // Remove the selection from our list.
                            RemoveSelection(s);
                            fChanged = true;
                        }
                    }
                }
            }

            if (primarySel != null) {
                SetPrimarySelection(primarySel);
            }

            // Now notify that our selection has changed
            if (fChanged) {
                OnSelectionChanged();
            }
        }



        ///     Adds the given selection to our selection list.
        private void AddSelection(SampleSelectionItem sel) {
            selectionsByComponent[sel.Component] = sel;
        }


        private void DesignerHost_LoadComplete(object sender, EventArgs e) {
            // Flush any pending changes
            batchMode = false;
            FlushSelectionChanges();
        }


        ///     Called by the designer host when it is entering or leaving a batch
        ///     operation.  Here we queue up selection notification and we turn off
        ///     our UI.
        private void DesignerHost_TransactionClosed(object sender, DesignerTransactionCloseEventArgs e) {
            batchMode = false;
            FlushSelectionChanges();
        }

        ///     Called by the designer host when it is entering or leaving a batch
        ///     operation.  Here we queue up selection notification and we turn off
        ///     our UI.
        private void DesignerHost_TransactionOpened(object sender, EventArgs e) {
            batchMode = true;
        }

        ///     Called by the formcore when someone has added a component.
        private void DesignerHost_ComponentAdd(object sender, ComponentEventArgs ce) {
            OnSelectionContentsChanged();
        }

        ///     Called when a component changes.  Here we look to see if the component is
        ///     selected.  If it is, we notify STrackSelection that the selection has changed.
        private void DesignerHost_ComponentChanged(object sender, ComponentChangedEventArgs ce) {
            if (selectionsByComponent[ce.Component] != null) {
                OnSelectionContentsChanged();
            }
        }

        ///     Called by the formcore when someone has removed a component.  This will
        ///     remove any selection on the component without disturbing the rest of
        ///     the selection.
        private void DesignerHost_ComponentRemove(object sender, ComponentEventArgs ce) {

            SampleSelectionItem sel = (SampleSelectionItem)selectionsByComponent[ce.Component];
            
            if (sel != null) {
                RemoveSelection(sel);
                

                // If we removed the last selection and we have a designer host, then select the base
                // component of the host.  Otherwise, it is OK to have an empty selection.
                if (selectionsByComponent.Count == 0 && host != null) {
                
                    // Now we have to run through all the components and pick
                    // the control with the highest z-order.
                    IComponent[] comps = new IComponent[host.Container.Components.Count];
                    host.Container.Components.CopyTo(comps, 0);
                    
                    if (comps == null) {
                        return;
                    }
                    
                    int maxZOrder = -1;
                    int defaultIndex = -1;
                    object maxIndexComp = null;
                    object baseComp = host.RootComponent;
                    
                    if (baseComp == null) {
                        return;
                    }
                    
                    for (int i = comps.Length - 1; i >= 0; i--) {
                        if (comps[i] == baseComp) {
                            continue;
                        }
                        else if (defaultIndex == -1) {
                            defaultIndex = i;
                        }
                        
                        if (comps[i] is Control) {
                            int zorder = ((Control)comps[i]).TabIndex;
                            if (zorder > maxZOrder) {
                                maxZOrder = zorder;
                                maxIndexComp = (object)comps[i];
                            }
                        }
                    }
                    
                    if (maxIndexComp == null) {
                        if (defaultIndex != -1) {
                            maxIndexComp = comps[defaultIndex];   
                        }
                        else {
                            maxIndexComp = baseComp;
                        }
                    }
                    
                    ((ISelectionService)this).SetSelectedComponents(new object[]{maxIndexComp}, SelectionTypes.Replace);
                }
                else {
                    OnSelectionChanged();
                }
            }
            else {
                // Component isn't selected, but our list of selectable components is
                // different.
                OnSelectionContentsChanged();
            }
        }



        ///     Disposes the entire selection manager.
        internal void Dispose() {
            // We've got to be careful here.  We're one of the last things to go away when
            // a form is being torn down, and we've got to be wary if things have pulled out
            // already.
            host.RemoveService(typeof(ISelectionService));
            host.TransactionOpened -= new EventHandler(this.DesignerHost_TransactionOpened);
            host.TransactionClosed -= new DesignerTransactionCloseEventHandler(this.DesignerHost_TransactionClosed);
            if (host.InTransaction) {
                DesignerHost_TransactionClosed(host, new DesignerTransactionCloseEventArgs(true));
            }
            host.LoadComplete -= new EventHandler(this.DesignerHost_LoadComplete);

            IComponentChangeService cs = (IComponentChangeService)host.GetService(typeof(IComponentChangeService));
            if (cs != null) {
                cs.ComponentAdded -= new ComponentEventHandler(this.DesignerHost_ComponentAdd);
                cs.ComponentRemoved -= new ComponentEventHandler(this.DesignerHost_ComponentRemove);
                cs.ComponentChanged -= new ComponentChangedEventHandler(this.DesignerHost_ComponentChanged);
            }

            SampleSelectionItem[] sels = new SampleSelectionItem[selectionsByComponent.Values.Count];
            selectionsByComponent.Values.CopyTo(sels, 0);
            
            for (int i = 0; i < sels.Length; i++) {
                sels[i].Dispose();
            }

            selectionsByComponent.Clear();
            primarySelection = null;
        }


        ///     Called when our visiblity or batch mode changes.  Flushes
        ///     any pending notifications or updates if possible.
        private void FlushSelectionChanges() {
            if (!batchMode) {
                if (selectionChanged) OnSelectionChanged();
                if (selectionContentsChanged) OnSelectionContentsChanged();
            }
        }
        
        
        ///     Called anytime the selection has changed.  We update our UI for the selection, and
        ///     then we fire a change event.
        private void OnSelectionChanged() {
            if (batchMode) {
                selectionChanged = true;
            }
            else {
                selectionChanged = false;
                
                // Raise SelectionChanging event - this should be moved to OnSelectionChanging()
				// if the implementation requires listening to changING events.
                if (SelectionChanging != null) {
                    try {
                        SelectionChanging(this, EventArgs.Empty);;
                    }
                    catch(Exception) {
                        // It is an error to ever throw in this event.
                        Debug.Fail("Exception thrown in selectionChanging event");
                    }
                }

                if (need_clear_selection)
                {
                    need_clear_selection = false;
                    selectionsByComponent.Clear();
                    primarySelection = null;                    
                }

                if (SelectionChanged != null) {
                    try {
                        SelectionChanged(this, EventArgs.Empty);;
                    }
                    catch(Exception) {
                        // It is an error to ever throw in this event.
                        Debug.Fail("Exception thrown in selectionChanging event");
                    }
                }
                
                
                OnSelectionContentsChanged();
            }
        }

        ///     This should be called when the selection has changed, or when just the contents of
        ///     the selection has changed.  It updates the document manager's notion of selection.
        ///     the selection have changed.
        private void OnSelectionContentsChanged() {
            if (batchMode) {
                selectionContentsChanged = true;
            }
            else {
                selectionContentsChanged = false;
                
                PropertyGrid grid = (PropertyGrid)host.GetService(typeof(PropertyGrid));
                if (grid != null)
                {
                    ICollection col = ((ISelectionService)this).GetSelectedComponents();
                    object[] selection = new Object[col.Count];
                    col.CopyTo(selection, 0);
                    grid.SelectedObjects = selection;
                    (host as SampleDesignerHost).SetSelectedComponents(selection);
                }
            }
        }

        ///     Removes the given selection from our selection list
        private void RemoveSelection(SampleSelectionItem s) {
            selectionsByComponent.Remove(s.Component);
            s.Dispose();
        }

        
        ///     Sets the given selection object to be the primary selection.
        internal void SetPrimarySelection(SampleSelectionItem sel) {
            if (sel != primarySelection) {
                if (null != primarySelection) {
                    primarySelection.Primary = false;
                }

                primarySelection = sel;
            }

            if (null != primarySelection) {
                primarySelection.Primary = true;
            }
        }

        //internal void SetSelectedComponent(IComponent comp)
        //{
        //    ((ISelectionService)this).SetSelectedComponents(new IComponent[] { comp });
        //}

    }
}

