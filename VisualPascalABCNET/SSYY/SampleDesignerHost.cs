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
    using System.ComponentModel.Design.Serialization;
    using System.Diagnostics;
    using System.Collections;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;
    using System.Drawing.Design;
    using System.Reflection;
    using System.Text;
    using System.Collections.Generic;
    using System.Drawing;

    public class SampleDesignerHost : 
          IContainer
        , IDesignerHost
        , IDesignerLoaderHost 
        , IComponentChangeService
        , IExtenderProviderService
		, IDesignerEventService
	{

        // Service objects we are responsible for
        internal ISelectionService              selectionService;       // selection services
        private IMenuCommandService            menuCommandService;     // the menu command service
        private IHelpService                   helpService;            // the help service - UNIMPLEMENTED
        private IReferenceService              referenceService;       // service to maintain references - UNIMPLEMENTED
        private IMenuEditorService             menuEditorService;      // Menu editor service - UNIMPLEMENTED
		private IToolboxService	               toolboxService;         // toolbox service

        // User attached events, designers and services.
        private IServiceContainer              serviceContainer;          // holds global services users have added
        private Hashtable                      designerTable;             // holds component<->designer mappings

        // The current Form Design
        private DesignerLoader                 designerLoader;            // the loader that reads/writes the document
        private Hashtable                      sites;                     // name -> DesignSite mapping
        private DesignerComponentCollection    components;                // public component collection.  Tracks sites table.
        
        private ArrayList                      extenderProviders;         // extenders that are on the current form
        private IComponent                     rootComponent;             // the base document component we're designing

        NETXP.Controls.ComboBoxExItem[] savedAllComponents = null;
        internal Dictionary<IComponent, NETXP.Controls.ComboBoxExItem> savedComponentItems = null;
        internal Dictionary<NETXP.Controls.ComboBoxExItem, IComponent> savedItemsComponent = null;


        private string                         rootComponentClassName;    // the name of the class the base component represents
        private IRootDesigner                  rootDesigner;              // the designer for the document
        private SampleDocumentWindow           documentWindow;            // the thing being reparented by the docwin
        private ITypeResolutionService         typeResolver;              // The object we load types through.
        private Exception                      loadError;                 // The first load error, or null.
        private INameCreationService           nameService;               // service we use to validate names of things.
        private int                            transactionCount;          // >0 means we're doing a transaction
        private StringStack                    transactionDescriptions;   // string descriptions of the current transactions

        // Flags that determine the validity of our code...are we in sync?
        private bool                           loadingDesigner;           // true if we are loading
        private bool                           reloading;                 // true if we are reloading the document

        // Transient stuff
        private SampleDesignSite               newComponentSite;          // used during new component creation

        public static Dictionary<Type, int> componentBitmaps = new Dictionary<Type, int>();
        private static ImageList controlImages = new ImageList();
        public Dictionary<IComponent, NETXP.Controls.ComboBoxExItem> componentItems = new Dictionary<IComponent, NETXP.Controls.ComboBoxExItem>();
        public Dictionary<NETXP.Controls.ComboBoxExItem, IComponent> itemsComponent = new Dictionary<NETXP.Controls.ComboBoxExItem, IComponent>();

        internal NETXP.Controls.ComboBoxEx currentComponentsCombo;

        //SampleDesignerHost
        public SampleDesignerHost() : this(new ServiceContainer()) {
        }

        // We take a service provider in our constructor so that our main form can give us
		// services (like the property grid and toolbox).
        public SampleDesignerHost(IServiceProvider parentProvider) {
            this.serviceContainer = new ServiceContainer(parentProvider);
            designerTable = new Hashtable();
            sites = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
            loadingDesigner = false;
            transactionCount = 0;
            reloading = false;
        
            // Services that we already have implemented on our object
            serviceContainer.AddService(typeof(IDesignerHost), this);
            serviceContainer.AddService(typeof(IContainer), this);
            serviceContainer.AddService(typeof(IComponentChangeService), this);
            serviceContainer.AddService(typeof(IExtenderProviderService), this);
			serviceContainer.AddService(typeof(IDesignerEventService), this);

			// And services that we demand create.
			ServiceCreatorCallback callback = new ServiceCreatorCallback(this.OnCreateService);

			serviceContainer.AddService(typeof(IToolboxService), callback);      
            serviceContainer.AddService(typeof(ISelectionService), callback);
			serviceContainer.AddService(typeof(ITypeDescriptorFilterService), callback);
			serviceContainer.AddService(typeof(IMenuCommandService), callback);
//          serviceContainer.AddService(typeof(IMenuEditorService), callback); - UNIMPLEMENTED
//          serviceContainer.AddService(typeof(IHelpService), callback); - UNIMPLEMENTED
//          serviceContainer.AddService(typeof(IReferenceService), callback); - UNIMPLEMENTED
//          serviceContainer.AddService(typeof(IPropertyValueUIService), callback); - UNIMPLEMENTED

            // Configure extender providers.
            ((IExtenderProviderService)this).AddExtenderProvider(new SampleNameExtenderProvider(this));
            ((IExtenderProviderService)this).AddExtenderProvider(new SampleInheritedNameExtenderProvider(this));
            //defaultMenuCommands = new DefaultMenuCommands(this);
            //defaultMenuCommands.AddTo(menuCommandService);
            // +Add Serialization Service  
            serviceContainer.AddService(typeof(IDesignerSerializationService), callback);
            // -Add Serialization Service  
            currentComponentsCombo = new NETXP.Controls.ComboBoxEx();
            currentComponentsCombo.Visible = false;
            VisualPascalABC.Form1.Form1_object.PropertiesWindow.GetComponentsComboPanel().Controls.Add(currentComponentsCombo);
            currentComponentsCombo.EnableMRU = false;
            currentComponentsCombo.Dock = DockStyle.Fill;
            NETXP.Controls.ComboBoxEx baseCombo = VisualPascalABC.Form1.Form1_object.PropertiesWindow.GetComponentsCombo();
            currentComponentsCombo.ItemHeight = baseCombo.ItemHeight;
            currentComponentsCombo.Font = baseCombo.Font;
            currentComponentsCombo.DrawMode = baseCombo.DrawMode;
            currentComponentsCombo.DropDownStyle = baseCombo.DropDownStyle;
            currentComponentsCombo.Flags = baseCombo.Flags;
            currentComponentsCombo.SelectedIndexChanged += new EventHandler(currentComponentsCombo_SelectedIndexChanged);
            currentComponentsCombo.Visible = true;
            currentComponentsCombo.ImageList = controlImages;
        }

        private void currentComponentsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentComponentsCombo.SelectedItem == null) return;
            IComponent comp = itemsComponent[currentComponentsCombo.SelectedItem as NETXP.Controls.ComboBoxExItem];
            selectionService.SetSelectedComponents(new IComponent[] { comp });
        }

        public void SetSelectedComponents(object[] selection)
        {
            switch (selection.Length)
            {
                case 0:
                    currentComponentsCombo.SelectedItem = null;
                    break;
                case 1:
                    currentComponentsCombo.SelectedItem = componentItems[selection[0] as IComponent];
                    break;
                default:
                    currentComponentsCombo.SelectedItem = null;
                    break;
            }
        }

        public static string getComboText(IComponent comp)
        {
            if (comp.Site != null)
            {
                return comp.Site.Name + ": " + comp.GetType().FullName;
            }
            else
            {
                return comp.GetType().FullName;
            }
        }

        public void AddDesignedComponent(IComponent comp)
        {
            int num = -1;
            Type type = comp.GetType();
            if (!componentBitmaps.TryGetValue(type, out num))
            {
                ToolboxBitmapAttribute tba = TypeDescriptor.GetAttributes(type)[typeof(ToolboxBitmapAttribute)] as ToolboxBitmapAttribute;
                if (tba == null)
                {
                    num = -1;
                }
                else
                {
                    num = currentComponentsCombo.ImageList.Images.Count;
                    currentComponentsCombo.ImageList.Images.Add(tba.GetImage(type));
                }
                componentBitmaps.Add(type, num);
            }
            NETXP.Controls.ComboBoxExItem it = new NETXP.Controls.ComboBoxExItem(num, getComboText(comp), 0);
            currentComponentsCombo.Items.Add(it);
            componentItems.Add(comp, it);
            itemsComponent.Add(it, comp);
        }

        public void DeleteDesignedComponent(IComponent comp)
        {
            NETXP.Controls.ComboBoxExItem it;
            bool exist = componentItems.TryGetValue(comp, out it);
            if (!exist) return;
            currentComponentsCombo.Items.Remove(it);
            itemsComponent.Remove(componentItems[comp]);
            componentItems.Remove(comp);
        }

        //****
        //****
        //**** IDisposable Implementation
        //****
        //****

        ///     Disposes of the DesignContainer.  This cleans up any objects we may be holding
        ///     and removes any services that we created.
        public void Dispose() {
        
            // Dispose the loader before destroying the designer.  Otherwise, the
            // act of destroying all the components on the designer surface will
            // be reflected in the loader, deleting the user's file.
            if (designerLoader != null) {
                try 
                {
                    designerLoader.Flush();
                }
                catch (Exception e1) 
                {
                    Debug.Fail("Designer loader '" + designerLoader.GetType().Name + "' threw during Flush: " + e1.ToString());
                    e1 = null;
                }

                try {
                    designerLoader.Dispose();
                }
                catch (Exception e2) {
                    Debug.Fail("Designer loader '" + designerLoader.GetType().Name + "' threw during Dispose: " + e2.ToString());
                    e2 = null;
                }
                designerLoader = null;
            }

            // Unload the document.
            UnloadDocument();

            // No services after this!
            serviceContainer = null;

            // Now tear down all of our services.
            if (menuEditorService != null) {
                IDisposable d = menuEditorService as IDisposable;
                if (d != null) d.Dispose();
                menuEditorService = null ;
            }

            if (selectionService != null) {
                IDisposable d = selectionService as IDisposable;
                if (d != null) d.Dispose();
                selectionService = null;
            }

            if (menuCommandService != null) {
                IDisposable d = menuCommandService as IDisposable;
                if (d != null) d.Dispose();
                menuCommandService = null;
            }

			if (toolboxService != null) 
			{
				IDisposable d = toolboxService as IDisposable;
				if (d != null) d.Dispose();
				toolboxService = null;
			}

            if (helpService != null) {
                IDisposable d = helpService as IDisposable;
                if (d != null) d.Dispose();
                helpService = null;
            }

            if (referenceService != null) {
                IDisposable d = referenceService as IDisposable;
                if (d != null) d.Dispose();
                referenceService = null;
            }

            // Destroy our document window.
            if (documentWindow != null) {
                documentWindow.Dispose();
                documentWindow = null;
            }
        }



        //****
        //****
        //**** IContainer Implementation
        //****
        //****

        // Properties
        ComponentCollection IContainer.Components { 
            get {
                if (components == null) {
                    components = new DesignerComponentCollection(this);
                }
                return components;
            }
        }

        // Methods

        ///     Adds the given component to the Designer.  This sets up the lifetime and site
        ///     relationship between component / container and then adds the new component
        ///     to the formcore and code buffer.
        ///     This will fabricate a default name for the component.
        void IContainer.Add(IComponent component) {
            ((IContainer)this).Add(component, null); 
        }

        void IContainer.Add(IComponent component, string name) {
            
            if (null == component) return;

            if (rootComponent != null) {
                // Compare to the class the root component represents, not it's actual class!
                if (String.Compare(component.GetType().FullName, rootComponentClassName, true) == 0) {
                    Exception ex = new Exception("Can't add a component to itself " + component.GetType().FullName);
                    throw ex;
                }
            }

            ISite site = component.Site;

            // If the component is already added to us, all we have to do is check
            // the name.  If the name is different we rename it, otherwise we do
            // nothing.
            if (site != null && site.Container == this) {
                if (name != null && !name.Equals(site.Name)) {
                    CheckName(name);
                    site.Name = name;
                }
                return;
            }

            // Check to see if someone has already configured a site for us.  If so,
            // use it.  Otherwise, fabricate a new site.
            // newComponentSite is created by CreateComponent.
            SampleDesignSite newSite = newComponentSite;
            newComponentSite = null;

            if (newSite != null && name == null) {
                name = newSite.Name;
            }

            // Make sure the name is valid.
            if (name != null) {
                CheckName(name);
            }

            ComponentEventArgs ce = new ComponentEventArgs(component);
            OnComponentAdding(ce);

            // Remove this component from its current site
            if (site != null) site.Container.Remove(component);

            if (newSite == null) {
                newSite = new SampleDesignSite(this, name);
            }

            // And set the relationship between this site and it's component.  If the
            // site has no name at this point, it will fabricate one.
            newSite.SetComponent(component);

            // If we were given a site, the name we're given should always be null,
            // or at least be the same name as that stored in the new site.
            Debug.Assert(name == null || name.Equals(newSite.Name), "Name should match the one in newComponentSite");
            
            if (component is IExtenderProvider &&
                !TypeDescriptor.GetAttributes(component).Contains(InheritanceAttribute.InheritedReadOnly)) {
                ((IExtenderProviderService)this).AddExtenderProvider((IExtenderProvider)component);
            }

            // And establish the component/site relationship
            sites[newSite.Name] = newSite;
            component.Site = newSite;
            if (components != null) {
                components.Add(newSite);
            }

            try {

                CreateComponentDesigner(component);

                // The component has been added.  Note that it is tempting to move this above the
                // designer because the designer will never need to know that its own component just
                // got added, but this would be bad because the designer is needed to extract
                // shadowed properties from the component.
                //
                OnComponentAdded(ce);
            }
            catch (Exception) {
                // If we're loading, then don't remove the component.  We are about to
                // fail the load anyway here, and we don't want to be firing remove events during
                // a load.
                if (!loadingDesigner) {
                    ((IContainer)this).Remove(component);
                }
                throw;
            }
        }


        void IContainer.Remove(IComponent component) {
            if (component == null) return;
            ISite site = component.Site;
            if (!sites.ContainsValue(site)) return;
            if (site == null || site.Container != this) return;
            if (!(site is SampleDesignSite)) return;

            ComponentEventArgs ce = new ComponentEventArgs(component);
            OnComponentRemoving(ce);

            SampleDesignSite csite = (SampleDesignSite)site;
            if (csite.Component != rootComponent) {
                if (component is IExtenderProvider) {
                    ((IExtenderProviderService)this).RemoveExtenderProvider((IExtenderProvider)component);
                }
            }

            // and remove it's designer, should one exist.
            IDesigner designer = (IDesigner)designerTable[component];
            if (designer != null) {
                designer.Dispose();
            }

            designerTable.Remove(component);

            sites.Remove(csite.Name);
            if (components != null) {
                components.Remove(site);
            }

            // By this time, the component is dead.  If someone
            // threw an exception, there's nothing we can do about it.
            //
            try {
                OnComponentRemoved(ce);
            }
            catch (Exception) {
            }

            // Finally, rip the site instance.
            //
            component.Site = null;
        }

        //****
        //****
        //**** IServiceProvider Implementation
        //****
        //****
        object IServiceProvider.GetService(Type serviceType) {
            Debug.Assert(serviceContainer != null, "We have no sevice container.  Either the host has not been initialized yet or it has been disposed.");
			object service = null;
            if (serviceContainer != null) {
                service = serviceContainer.GetService(serviceType);

				// Record the success or failure of service requests in case someone
				// wants to see in the Main Form.
				//
				SampleDesignerApplication.ServiceRequests requests = (SampleDesignerApplication.ServiceRequests)serviceContainer.GetService(typeof(SampleDesignerApplication.ServiceRequests));
				if (requests != null) 
				{
					if (service != null) 
					{
						requests.ServiceSucceeded(serviceType);
					}
					else
					{
						requests.ServiceFailed(serviceType);
					}
				}
            }
            return service;
        }

        //****
        //****
        //**** IServiceContainer Implementation
        //****
        //****
        void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback) {
            Debug.Assert(serviceContainer != null, "We have no sevice container.  Either the host has not been initialized yet or it has been disposed.");
            if (serviceContainer != null) {
                serviceContainer.AddService(serviceType, callback);
            }
        }

        void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback, bool promote) {
            Debug.Assert(serviceContainer != null, "We have no sevice container.  Either the host has not been initialized yet or it has been disposed.");
            if (serviceContainer != null) {
                serviceContainer.AddService(serviceType, callback, promote);
            }
        }

        void IServiceContainer.AddService(Type serviceType, object serviceInstance) {
            Debug.Assert(serviceContainer != null, "We have no sevice container.  Either the host has not been initialized yet or it has been disposed.");
            if (serviceContainer != null) {
                serviceContainer.AddService(serviceType, serviceInstance);
            }
        }

        void IServiceContainer.AddService(Type serviceType, object serviceInstance, bool promote) {
            Debug.Assert(serviceContainer != null, "We have no sevice container.  Either the host has not been initialized yet or it has been disposed.");
            if (serviceContainer != null) {
                serviceContainer.AddService(serviceType, serviceInstance, promote);            
            }
        }

        void IServiceContainer.RemoveService(Type serviceType) {
            if (serviceContainer != null) {
                serviceContainer.RemoveService(serviceType);            
            }
        }

        void IServiceContainer.RemoveService(Type serviceType, bool promote) {
            if (serviceContainer != null) {
                serviceContainer.RemoveService(serviceType, promote);            
            }
        }

        //****
        //****
        //**** IDesignerLoaderHost Implementation
        //****
        //****
        
        // Methods

        ///     This is called by the designer loader to indicate that the load has 
        ///     terminated.  If there were errors, they should be passed in the errorCollection
        ///     as a collection of exceptions (if they are not exceptions the designer
        ///     loader host may just call ToString on them).  If the load was successful then
        ///     errorCollection should either be null or contain an empty collection.
        void IDesignerLoaderHost.EndLoad(string baseClassName, bool successful, ICollection errorCollection) {

            bool wasReload = reloading;
            bool wasLoading = loadingDesigner;

            // Set our state back to the starting point.
            this.loadingDesigner = false;
            this.reloading = false;
            
            if (baseClassName != null) {
                this.rootComponentClassName = baseClassName;
            }
            
            // If we had errors, report them.
            //
            if (successful && rootComponent == null) {
                ArrayList errorList = new ArrayList();
                errorList.Add(new Exception("No Base Class"));
                errorCollection = errorList;
                successful = false;
            }
            
            if (!successful) {
            
                // The document is partially loaded.  Unload it here.
                try {
                    UnloadDocument();
                }
                catch (Exception ex) {
                    Debug.Fail("Failed to unload after a...failed load.", ex.ToString());
                }

                if (errorCollection != null) {
                    foreach(object errorObj in errorCollection) {
                        if (errorObj is Exception) {
                            loadError = (Exception)errorObj;
                        }
                        else {
                            loadError = new Exception(errorObj.ToString());
                        }
                        break;
                    }
                }
                else {
                    loadError = new Exception("Unknown Load Error");
                }

				documentWindow.SetDesigner(null);
			}
            else {
            
                // We may be invoked to do an EndLoad when we are already loaded.  This can happen
                // if the user called AddLoadDependency, essentially putting us in a loading state
                // while we are already loaded.  This is OK, and is used as a hint that the user
                // is going to monkey with settings but doesn't want the code engine to report
                // it.
                if (wasLoading) {
                
                    // and let everyone know that we're loaded
                    if (LoadComplete != null) {
                        LoadComplete(this, EventArgs.Empty);
                    }
                }
            }

			documentWindow.ReportErrors(errorCollection);
			documentWindow.DocumentVisible = true;
        }
    
        ///     This is called by the designer loader when it wishes to reload the
        ///     design document.  The reload may happen immediately or it may be deferred
        ///     until idle time.  The designer loader should unload itself before calling
        ///     this method, to reset its state to that before BeginLoad was originally
        ///     called.
        void IDesignerLoaderHost.Reload() {
        
            // If the DesignerLoader has been destroyed already, then there
            // is no need to reload
            if (designerLoader == null || documentWindow == null) {
                return;
            }
            
            // Before reloading, flush any changes!
            designerLoader.Flush();
            
            Cursor oldCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            ICollection selectedObjects = null;
            ArrayList selectedNames = null;
            
            IServiceProvider sp = (IServiceProvider)this;
            ISelectionService selectionService = (ISelectionService)sp.GetService(typeof(ISelectionService));
            if (selectionService != null) {
                selectedObjects = selectionService.GetSelectedComponents();
                selectedNames = new ArrayList();
                
                foreach(object comp in selectedObjects) {
                    if (comp is IComponent && ((IComponent)comp).Site != null) {
                        selectedNames.Add(((IComponent)comp).Site.Name);
                    }
                }
            }
            
            try {
                UnloadDocument();
                Load(true);
            }
            finally {
            
                if (selectionService != null) {
                
                    ArrayList selection = new ArrayList();
                    foreach(string name in selectedNames) {
                        if (name != null) {
                            IComponent comp = ((IContainer)this).Components[name];
                            if (comp != null) {
                                selection.Add(comp);
                            }
                        }
                    }
                    
                    selectionService.SetSelectedComponents(selection);
                }
                Cursor.Current = oldCursor;
            }
        }

        
        //****
        //****
        //**** IDesignerHost Implementation
        //**** 
        //****

        // Properties
        public IContainer Container {
            get {
                return (IContainer)this;
            }
        }

        // Gets a value indicating whether the designer host is currently in a transaction.
        public bool InTransaction { 
            get {
                return transactionCount > 0;
            }
        }

        ///     Is the document currently being loaded.
        public bool Loading {
            get {
                return loadingDesigner || (designerLoader != null && designerLoader.Loading);
            }
        }

        ///     Retrieves the instance of the base class that is being used as the basis
        ///     for this design.  This is typically a Form or UserControl instance; it
        ///     defines the class for which the user's derived class will extend.
        public IComponent RootComponent {
            get {
                return rootComponent;
            }
        }


        ///     Retrieves the fully qualified name of the class that is being designed.
        ///     This class is not available at design time because it is the class that
        ///     is being designed, so the class's superclass is substituted.  This allows
        ///     you to get at the fully qualified name of the class that will be used
        ///     at runtime.
        public string RootComponentClassName {
            get {
                return rootComponentClassName;
            }
        }

        // Gets the description of the current transaction.
        public string TransactionDescription {
            get {
                if (transactionDescriptions != null) {
                    return transactionDescriptions.GetNonNull();
                }
                return "";
            }
        }

        // Events
        public event EventHandler Activated;
        public event EventHandler Deactivated;
        public event EventHandler LoadComplete;
        public event DesignerTransactionCloseEventHandler TransactionClosed;
        public event DesignerTransactionCloseEventHandler TransactionClosing;
        public event EventHandler TransactionOpened;
        public event EventHandler TransactionOpening;

        // Methods

        ///     Causes the designer that this host is hosting to become activated.
        public void Activate() {
            documentWindow.Focus();
        }

        /// Creates a new component from the given class. A component name will be fabricated.
        public IComponent CreateComponent(Type componentClass) {
            string name = this.GetNewComponentName(componentClass);
            return CreateComponent(componentClass, name);
        }

        public IComponent CreateComponent(Type componentClass, string name) {

            object obj = null;
            IComponent comp = null;

            // Create the site we are going to use here so that our Container.Add implementation 
            // can pick it up - need to do this here because name is not passed to Container.Add
            // by Foo(IContainer) constructor below.
            newComponentSite = new SampleDesignSite(this,name);

            // Create the Component - there are 2 possible ways to do this:
            // Foo(IComponent) or Foo().

            try {
                // See if we can create the component using an IContainer constructor...
                try {
                    object[] args = new object[] {this};
                    Type[] argTypes = new Type[] {typeof(IContainer)};
                    obj = CreateObject(componentClass, args, argTypes, false);
                }
                catch (Exception) {
                }

                // If it failed, try to create it with a default constructor
                if (null == obj) {
                    obj = CreateObject(componentClass, null, null, false);
                }

                //Make sure we have a component
                comp = obj as Component ;
                if (comp == null) {
                    Exception ex = new Exception("The class is not a component " + componentClass.FullName);
                    throw ex;
                }

                // If we didn't have a constructor that took a Container or the site is not 
                // a SampleDesignSite the we will do the Container.Add() work 
                SampleDesignSite site = comp.Site as SampleDesignSite;
                if (site == null) {
                    ((IContainer)this).Add(comp);
                }

                // At this point, our add call should have used the new site we gave it (if there was
                // one), and nulled out the holder pointer.
                Debug.Assert(newComponentSite == null, "add didn't use newComponentSite");
            }
            catch (Exception) {
                if (comp != null) {
                    try {
                        DestroyComponent(comp);
                    }
                    catch (Exception) {
                    }
                }

                throw;
            }
            if (comp != null)
            {
                AddDesignedComponent(comp);
            }
            return comp;
        }

        ///     Lengthy operations that involve multiple components may raise many events.  These events
        ///     may cause other side-effects, such as flicker or performance degradation.  When operating
        ///     on multiple components at one time, or setting multiple properties on a single component,
        ///     you should encompass these changes inside a transaction.  Transactions are used
        ///     to improve performance and reduce flicker.  Slow operations can listen to 
        ///     transaction events and only do work when the transaction completes.
        public DesignerTransaction CreateTransaction() {
            return CreateTransaction(null);
        }

        public DesignerTransaction CreateTransaction(string description) {
            if (description == null) {
                description = string.Empty;
            }
            
            return new SampleDesignerTransaction(this, description);
        }


        ///     Destroys the given component, removing it from the design container.
        public void DestroyComponent(IComponent comp) {
            string name;
            
            if (comp.Site != null && comp.Site.Name != null) {
                name = comp.Site.Name;
            }
            else {
                name = comp.GetType().Name;
            }

            // Make sure the component is not being inherited -- we can't delete these!
            InheritanceAttribute ia = (InheritanceAttribute)TypeDescriptor.GetAttributes(comp)[typeof(InheritanceAttribute)];
            if (ia != null && ia.InheritanceLevel != InheritanceLevel.NotInherited) {
                throw new InvalidOperationException("CantDestroyInheritedComponent" + name);
            }
            
            DesignerTransaction t = null;
            try {
                // We try to remove the component from the container before destroying it.  
                t = CreateTransaction("DestroyComponentTransaction" + name);

                // We need to signal changing and then perform the remove.  Remove must be done by us and not
                // by Dispose because (a) people need a chance to cancel through a Removing event, and (b)
                // Dispose removes from the container last and anything that would sync Removed would end up
                // with a dead component.
                //
                ((IComponentChangeService)this).OnComponentChanging(comp, null);
                if (comp.Site != null) {
                    Remove(comp);
                }
                DeleteDesignedComponent(comp);
                comp.Dispose();
                ((IComponentChangeService)this).OnComponentChanged(comp, null, null, null);
            }
            finally {
                if (t != null) {
                    t.Commit();
                }
            }
        }

        // Retrieves the designer for the given component.
        public IDesigner GetDesigner(IComponent component) {
            Debug.Assert(component != null, "Cannot call GetDesigner with a NULL component.  Check that the root hasn't been disposed.");
            if (component == null) throw new ArgumentNullException("component");
            return(IDesigner)designerTable[component];
        }


        ///     Retrieves the Type instance for the given type name.  The type name must be
        ///     fully qualified, or it must be contained in an assembly that has been referenced
        ///     by the designer.
        public Type GetType(string typeName) {
            if (typeResolver == null) {
                IServiceProvider sp = (IServiceProvider)this;
                typeResolver = (ITypeResolutionService)sp.GetService(typeof(ITypeResolutionService));
            }
            if (typeResolver != null) {
                return typeResolver.GetType(typeName);
            }
            return Type.GetType(typeName);
        }


        //****
        //****
        //**** IComponentChangeService Implementation
        //****
        //****

        // Events
        public event ComponentEventHandler ComponentAdded;
        public event ComponentEventHandler ComponentAdding;
        public event ComponentChangedEventHandler ComponentChanged;
        public event ComponentChangingEventHandler ComponentChanging;
        public event ComponentEventHandler ComponentRemoved;
        public event ComponentEventHandler ComponentRemoving;
        public event ComponentRenameEventHandler ComponentRename;

        // Methods
        
        ///     This is called after a property has been changed.  It allows
        ///     the implementor to do any post-processing that may be needed
        ///     after a property change.  For example, a designer will typically
        ///     update the source code that sets the property with the new value.
        void IComponentChangeService.OnComponentChanged(object component, MemberDescriptor member, object oldValue, object newValue) {
            
            // If we're loading then eat changes.  This just slows us down.
            if (Loading) {
                return;
            }

            if (ComponentChanged != null) {
                ComponentChangedEventArgs ce = new ComponentChangedEventArgs(component, member, oldValue, newValue);
                ComponentChanged(this, ce);
            }
        }
        
        ///     This is called when a property is about to change.  Before the
        ///     property descriptor commits the property it will call this
        ///     method.  This method should throw an exception if the property
        ///     cannot be changed.  This is not intended to validate the values
        ///     of a particular property.  Instead, it is intended to be a global
        ///     way of preventing a component from changing.  For example, if
        ///     a designer file is checked into source code control, this would
        ///     typically throw an exception if the user refused to check out
        ///     the file.
        void IComponentChangeService.OnComponentChanging(object component, MemberDescriptor member) {
            // If we're loading then eat changes.  This just slows us down.
            if (Loading) {
				if (member.Name == "Size")
				{
					string _DEBUG_ = member.Name;
				}
                return;
            }

            if (ComponentChanging != null) {
                ComponentChangingEventArgs ce = new ComponentChangingEventArgs(component, member);
                ComponentChanging(this, ce);
            }

        }

        //****
        //****
        //**** IExtenderProviderService Implementation
        //****
        //****

        // Methods

        void IExtenderProviderService.AddExtenderProvider(IExtenderProvider provider) {
            if (extenderProviders == null) {
                extenderProviders = new ArrayList();
            }
            extenderProviders.Add(provider);
        }


        void IExtenderProviderService.RemoveExtenderProvider(IExtenderProvider provider) {
            if (extenderProviders != null) {
                extenderProviders.Remove(provider);
            }
        }


        //****
        //****
        //**** Sample Designer Host methods
        //****
        //****



        ///     Validates that the given name is OK to use.  Not only does it have to
        ///     be a valid identifier, but it must not already be in our container.
        internal void CheckName(string name) {

            if (name == null || name.Length == 0) {
                Exception ex = new Exception("Components must have a name");
                throw ex;
            }

            if (((IContainer)this).Components[name] != null) {
                Exception ex = new Exception("We already have a component named " + name);
                throw ex;
            }
            
            if (nameService == null) {
                IServiceProvider sp = (IServiceProvider)this;
                nameService = (INameCreationService)sp.GetService(typeof(INameCreationService));
            }
            
            if (nameService != null) {
                nameService.ValidateName(name);
            }
        }
    
    
        private void CreateComponentDesigner(IComponent component) {
            
            // Is this the first component the loader has created?  If so, then it must
            // be the base component (by definition) so we will expect there to be a document
            // designer associated with the component.  Otherwise, we search for a
            // normal designer, which can be optionally provided.
            
            IDesigner designer = null;

            if (rootComponent == null) {
                rootComponent = component;

                // Get the root designer.  We check right here to see if the document window supports
                // hosting this type of designer.  If not, we bail early.
                rootDesigner = (IRootDesigner)TypeDescriptor.CreateDesigner(component, typeof(IRootDesigner));

                if (rootDesigner == null) {
                    rootComponent = null;
                    Exception ex = new Exception("No Top Level Designer for " + component.GetType().FullName);
                    throw ex;
                }

                ViewTechnology[] technologies = rootDesigner.SupportedTechnologies;
                bool supported = false;
                foreach(ViewTechnology tech in technologies) {
                    if (tech == ViewTechnology.WindowsForms) {
                        supported = true;
                        break;
                    }
                }

                if (!supported) {
                    Exception ex = new Exception("This designer host does not support the designer for " + component.GetType().FullName);
                    throw ex;
                }                    

                designer = rootDesigner;

                // Check and see if anyone has set the class name of the base component.
                // We default to the component name.
                if (rootComponentClassName == null) {
                    rootComponentClassName = component.Site.Name;
                }
            }
            else {
                designer = TypeDescriptor.CreateDesigner(component, typeof(IDesigner));
            }

            if (designer != null) {
                designerTable[component] = designer;
                try {
                    designer.Initialize(component);
                }
                catch {
                    designerTable.Remove(component);

                    if (designer == rootDesigner) {
                        rootDesigner = null;
                    }

                    throw;
                }

                if (designer is IExtenderProvider &&
                    !TypeDescriptor.GetAttributes(designer).Contains(InheritanceAttribute.InheritedReadOnly)) {
                    ((IExtenderProviderService)this).AddExtenderProvider((IExtenderProvider)designer);
                }

                // Now, if this is the root designer, initialize the designer window with it.
                if (designer == rootDesigner) {
                    documentWindow.SetDesigner(rootDesigner);
                }
            }
        }
        
		/// Create an instance of a type given arguments and their types.
        private object CreateObject(Type objectClass, object []args, Type[] argTypes, bool fThrowException) {
            ConstructorInfo ctr = null;

            if (args != null && args.Length > 0) {
                if (argTypes == null) {
                    argTypes = new Type[args.Length];

                    for (int i = args.Length - 1; i>= 0; i--) {
                        if (args[i] != null) argTypes[i] = args[i].GetType();
                    }
                }

                ctr = objectClass.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, argTypes, null);

                if (ctr == null && fThrowException) {
                    Exception ex = new Exception("Cannot find a constructor with the right arguments for " + objectClass.FullName);
                    throw ex;
                }
                else
                    return null;
            }

            try {
                return(ctr == null) ? Activator.CreateInstance(objectClass) : ctr.Invoke(args);
            }
            catch (Exception e) {
                if (e is TargetInvocationException) {
                    e = e.InnerException;
                }
                
                string message = e.Message;
                if (message == null) {
                    message = e.ToString();
                }
                
                throw new Exception(   "Cannot create an instance of " 
                                     + objectClass.FullName 
                                     + " because " + e.ToString(), e);
            }
        }

        /// Create a name from a new component of a given type.
        internal string GetNewComponentName(Type compClass) {
            IServiceProvider sp = (IServiceProvider)this;
            INameCreationService nameCreate = (INameCreationService)sp.GetService(typeof(INameCreationService));
            if (nameCreate != null) {
                return nameCreate.CreateName(Container, compClass);
            }
            
            // Do a default thing...
            //
            string baseName = compClass.Name;
            
            // Camel case the base name.
            //
            StringBuilder b = new StringBuilder(baseName.Length);
            for (int i = 0; i < baseName.Length; i++) {
                if (Char.IsUpper(baseName[i]) && (i == 0 || i == baseName.Length - 1 || Char.IsUpper(baseName[i+1]))) {
                    b.Append(Char.ToLower(baseName[i]));
                }
                else {
                    b.Append(baseName.Substring(i));
                    break;
                }
            }
            baseName = b.ToString();
            
            int idx = 1;
            string finalName = baseName + idx.ToString();
            while(Container.Components[finalName] != null) {
                idx++;
                finalName = baseName + idx.ToString();
            }
            
            return finalName;
        }
        
        ///  Loads the design document using the DesignerLoader
        private void Load(bool reloading) {
            Cursor oldCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            this.reloading = reloading;

            try {
                if (!reloading && designerLoader is IExtenderProvider) {
                   ((IExtenderProviderService)this).AddExtenderProvider((IExtenderProvider)designerLoader);
                }
                
                loadingDesigner = true;
                loadError = null;
                designerLoader.BeginLoad(this);
            }
            catch (Exception e) {
                Exception exNew = e;
                
                if (e is TargetInvocationException) {
                    exNew = e.InnerException;
                }

                string message = exNew.Message;

                // We must handle the case of an exception with no message.
                if (message == null || message.Length == 0) {
                    Debug.Fail("Parser has thrown an exception that has no friendly message", exNew.ToString());
                    exNew = new Exception("Parser has thrown an exception that has no friendly message" + exNew.ToString());
                }
            
                // Loader blew up.  Add this exception to our error list
                ArrayList errorList = new ArrayList();
                errorList.Add(exNew);
                ((IDesignerLoaderHost)this).EndLoad(null, false, errorList);
            }
            
            Cursor.Current = oldCursor;
        }
        
        
        ///    Load the contents of the designer
        public void LoadDocument(DesignerLoader designerLoader) {
            try {
                this.designerLoader = designerLoader;
                
                // Create the Design View
                documentWindow = new SampleDocumentWindow(this);

                // Load the document
                Load(false);
                
            }
            catch (Exception t) {
                Debug.Fail(t.ToString());
                throw;
            }
        }

        ///     This is called after a component has been added to the container.
        private void OnComponentAdded(ComponentEventArgs ce) {
            if (ComponentAdded != null) {

				// This is just to double-check that our TypeResolutionService has the
				// necessary assemblies in its internal set. After all, compilation depends
				// on it! Normally our ToolboxItems do this for us, but if we open an XML
				// document and load it, we get no help from the ToolboxItems.
				//
				ITypeResolutionService trs = ((IServiceContainer)this).GetService(typeof(ITypeResolutionService)) as ITypeResolutionService;
				trs.ReferenceAssembly(ce.Component.GetType().Assembly.GetName());

                ComponentAdded(this, ce);
            }
        }

        ///     This is called when a component is about to be added to our container.
        private void OnComponentAdding(ComponentEventArgs ce) {
            if (ComponentAdding != null) {
                ComponentAdding(this, ce);
            }
        }



        ///     This is called after a component has been removed from the container, but before
        ///     the component's site has been destroyed.
        private void OnComponentRemoved(ComponentEventArgs ce) {
            if (ComponentRemoved != null) {
                ComponentRemoved(this, ce);
            }
        }

        ///     This is called when a component is about to be removed from our container.
        private void OnComponentRemoving(ComponentEventArgs ce) {
            if (ComponentRemoving != null) {
                ComponentRemoving(this, ce);
            }
        }

        ///     This is called when a component has been renamed.
        internal void OnComponentRename(ComponentRenameEventArgs ce) {
            if (ComponentRename != null) {
                ComponentRename(this, ce);
            }
        }

        /// Creates some of the more infrequently used services
        private object OnCreateService(IServiceContainer container, Type serviceType) {
            
            // Create SelectionService
            if (serviceType == typeof(ISelectionService)) {
                if (selectionService == null) {
                    selectionService = new SampleSelectionService(this);
                }
                return selectionService;
            }

			if (serviceType == typeof(ITypeDescriptorFilterService)) {
				return new SampleTypeDescriptorFilterService(this);
			}         

			
			if (serviceType == typeof(IToolboxService)) {
				if (toolboxService == null) {
					toolboxService = new SampleToolboxService(this);
				}
				return toolboxService;
			}
            
            
            if (serviceType == typeof(IMenuCommandService)) {
                if (menuCommandService == null) {
                    menuCommandService = new SampleMenuCommandService(this);
                }
                return menuCommandService;
            }

// UNIMPLEMENTED         
//            if (serviceType == typeof(IHelpService)) {
//                if (helpService == null) {
//                    helpService = new SampleHelpService(this);
//                }
//                return helpService;
//            }
//            
//            if (serviceType == typeof(IReferenceService)) {
//                if (referenceService == null) {
//                    referenceService = new SampleReferenceService(this, true);
//                }
//                return referenceService;
//            }
//            
//            if (serviceType == typeof(IPropertyValueUIService)) {
//                return new SamplePropertyValueUIService();
//            }
//
//            if (serviceType == typeof(IMenuEditorService)) {
//                if (menuEditorService == null) {
//                    menuEditorService = new SampleMenuEditorService(this);
//                }
//                return menuEditorService;
//            }

            if (serviceType == typeof(IDesignerSerializationService))
            {
                if (serializationService == null)
                {
                    serializationService = new SampleDesignerSerializationService(this);
                }
                return serializationService;
            }

            Debug.Fail("Service type " + serviceType.FullName + " requested but we don't support it");
            return null;
        }

        internal void OnTransactionOpened(EventArgs e) {
             if (TransactionOpened != null)
                 TransactionOpened(this, e);
        }
        
        internal void OnTransactionOpening(EventArgs e) {
            if (TransactionOpening != null)
                TransactionOpening(this, e);
        }

        internal void OnTransactionClosed(DesignerTransactionCloseEventArgs e) {
            if (TransactionClosed != null)
                TransactionClosed(this, e);
        }
        
        internal void OnTransactionClosing(DesignerTransactionCloseEventArgs e) {
            if (TransactionClosing != null)
                TransactionClosing(this, e);
        }
        
        /// Called to remove a component from its container.
        private void Remove(IComponent component) {

            // Bail early if this component hasn't been added to us.
            if (component == null) return;
            ISite site = component.Site;
            if (!sites.ContainsValue(site)) return;
            if (site == null || site.Container != this) return;
            if (!(site is SampleDesignSite)) return;

            ComponentEventArgs ce = new ComponentEventArgs(component);
            OnComponentRemoving(ce);

            // Remove the component from extender provider list if its an extender provider
            SampleDesignSite csite = (SampleDesignSite)site;
            
            if (csite.Component != rootComponent) {
                if (component is IExtenderProvider) {
                    ((IExtenderProviderService)this).RemoveExtenderProvider((IExtenderProvider)component);
                }
            }

            // and remove it's designer, should one exist.
            IDesigner designer = (IDesigner)designerTable[component];
            if (designer != null) {
                designer.Dispose();
            }

            designerTable.Remove(component);

            sites.Remove(csite.Name);
            if (components != null) {
                components.Remove(site);
            }

            // By this time, the component is dead.  If some bonehead
            // threw, there's nothing we can do about it.
            //
            try {
                OnComponentRemoved(ce);
            }
            catch (Exception) {
            }

            // Finally, rip the site instance.
            component.Site = null;
        }

        // Exposed to allow ancillary classes to access this internal state.
        internal Hashtable Sites {
            get {
                return sites;
            }
        }

		// Get descriptions of all of our transactions.
        internal StringStack TransactionDescriptions {
            get {
                if (transactionDescriptions == null) {
                    transactionDescriptions = new StringStack();
                }
                return transactionDescriptions;
            }
        }

		// Get or set the number of transactions we have.
        internal int TransactionCount {
            get {
                return transactionCount;
            }
            set {
                transactionCount = value ;
            }
        }

        // This is called during Dispose and Reload methods to unload the current designer.
        private void UnloadDocument() {

            if (helpService != null && rootDesigner != null) {
                helpService.RemoveContextAttribute("Keyword", "Designer_" + rootDesigner.GetType().FullName);
            }
            
            // Note: Because this can be called during Dispose, we are very anal here
            // about checking for null references.

            // If we can get a selection service, clear the selection...
            // we don't want the property browser browsing disposed components...
            // or components who's designer has been destroyed.
            IServiceProvider sp = (IServiceProvider)this;
            ISelectionService selectionService = (ISelectionService)sp.GetService(typeof(ISelectionService));
            Debug.Assert(selectionService != null, "ISelectionService not found");
            if (selectionService != null) {
                selectionService.SetSelectedComponents(null);
            }

            // Stash off the base designer and component.  We are
            // going to be destroying these and we don't want them
            // to be accidently referenced after they're dead.
            //
            IDesigner rootDesignerHolder = rootDesigner;
            IComponent rootComponentHolder = rootComponent;

            rootDesigner = null;
            rootComponent = null;
            rootComponentClassName = null;

            SampleDesignSite[] siteArray = new SampleDesignSite[sites.Values.Count];
            sites.Values.CopyTo(siteArray, 0);

            // Destroy all designers.  We save the base designer for last.
            //
            IDesigner[] designers = new IDesigner[designerTable.Values.Count];
            designerTable.Values.CopyTo(designers, 0);
            designerTable.Clear();

            // Loading, unloading, it's all the same.  It indicates that you
            // shouldn't dirty or otherwise mess with the buffer.  We also
            // create a transaction here to limit the effects of making
            // so many changes.
            loadingDesigner = true;
            DesignerTransaction trans = CreateTransaction();
            
            try {
                for (int i = 0; i < designers.Length; i++) {
                    if (designers[i] != rootDesignerHolder) {
                        try {
                            designers[i].Dispose();
                        }
                        catch {
                            Debug.Fail("Designer " + designers[i].GetType().Name + " threw an exception during Dispose.");
                        }
                    }
                }
    
                // Now destroy all components.
                for (int i = 0; i < siteArray.Length; i++) {
                    SampleDesignSite site = siteArray[i];
                    IComponent comp = site.Component;
                    if (comp != null && comp != rootComponentHolder) {
                        try {
                            comp.Dispose();
                        }
                        catch {
                            Debug.Fail("Component " + site.Name + " threw during dispose.  Bad component!!");
                        }
                        if (comp.Site != null) {
                            Debug.Fail("Component " + site.Name + " did not remove itself from its container");
                            Remove(comp);
                        }
                    }
                }
    
                // Finally, do the base designer and component.
                //
                if (rootDesignerHolder != null) {
                    try {
                        rootDesignerHolder.Dispose();
                    }
                    catch {
                        Debug.Fail("Designer " + rootDesignerHolder.GetType().Name + " threw an exception during Dispose.");
                    }
                }
    
                if (rootComponentHolder != null) {
                    try {
                        rootComponentHolder.Dispose();
                    }
                    catch {
                        Debug.Fail("Component " + rootComponentHolder.GetType().Name + " threw during dispose.  Bad component!!");
                    }
                    
                    if (rootComponentHolder.Site != null) {
                        Debug.Fail("Component " + rootComponentHolder.Site.Name + " did not remove itself from its container");
                        Remove(rootComponentHolder);
                    }
                }
            
                sites.Clear();
                if (components != null) {
                    components.Clear();
                }
            }
            finally {
                loadingDesigner = false;
                trans.Commit();
            }
            
            // And clear the document window
            //
            if (documentWindow != null) {
                documentWindow.SetDesigner(null);
            }
        }

        //Return the designer control.
        public Control View {
           get {
               return documentWindow;
           }
		}

		/// The IDesignerEventService is responsible for designer events. Since we have only
		/// our one designer host, we always return it when asked for the ActiveDesigner or
		/// Designers. 
		#region Implementation of IDesignerEventService

		public DesignerCollection Designers
		{
			get
			{
				return new DesignerCollection(new IDesignerHost[] { this });
			}
		}

		public event System.ComponentModel.Design.DesignerEventHandler DesignerDisposed;

		public IDesignerHost ActiveDesigner
		{
			get
			{
				return this;
			}
		}

		public event System.ComponentModel.Design.DesignerEventHandler DesignerCreated;

		public event System.ComponentModel.Design.ActiveDesignerEventHandler ActiveDesignerChanged;

		public event System.EventHandler SelectionChanged;

		#endregion

        //public void ChangeSelectionExecute(object sender, EventArgs e)
        //{
        //    throw new Exception();
        //
        //}

        // START OF ADD TO SampleDesignerHost

        private IDesignerSerializationService serializationService;         // service de serialisation

        // +Add for the copy function
        internal void WriteComponentCollection(System.Xml.XmlDocument document, IList list, System.Xml.XmlNode parent)
        {
            ((SampleDesignerLoader)designerLoader).WriteComponentCollection(document, list, parent);
        }

        internal ICollection CreateComponents(System.Xml.XmlDocument xSource, ArrayList errors)
        {
            return ((SampleDesignerLoader)designerLoader).CreateComponents(xSource, errors);
        }
        // -Add for the copy function
        // END OF ADD TO SampleDesignerHost

        public void HideCombo()
        {
            currentComponentsCombo.Visible = false;
            currentComponentsCombo.Parent = null;
            //savedComponentItems = VisualPascalABC.Form1.Form1_object.PropertiesWindow.componentItems;
            //savedItemsComponent = VisualPascalABC.Form1.Form1_object.PropertiesWindow.itemsComponent;
            //NETXP.Controls.ComboBoxExItemCollection c = VisualPascalABC.Form1.Form1_object.PropertiesWindow.GetItemsControls();
            //savedAllComponents = new NETXP.Controls.ComboBoxExItem[c.Count];
            //if (c.Count > 0)
            //{
            //    c.CopyTo(savedAllComponents, 0);
            //}
        }

        public void ShowCombo()
        {
            Panel p = VisualPascalABC.Form1.Form1_object.PropertiesWindow.GetComponentsComboPanel();
            currentComponentsCombo.Parent = p;
            currentComponentsCombo.Visible = true;
            //VisualPascalABC.Form1.Form1_object.PropertiesWindow.componentItems = savedComponentItems;
            //VisualPascalABC.Form1.Form1_object.PropertiesWindow.itemsComponent = savedItemsComponent;
            //NETXP.Controls.ComboBoxExItemCollection c = VisualPascalABC.Form1.Form1_object.PropertiesWindow.GetItemsControls();
            //c.Clear();
            //c.AddRange(savedAllComponents);
        }

	}
}
