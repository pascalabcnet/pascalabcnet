// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using ICSharpCode.Core;
using ICSharpCode.FormsDesigner.Services;
using ICSharpCode.FormsDesigner.UndoRedo;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Editor; 
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Refactoring;
namespace ICSharpCode.FormsDesigner
{
    public class FormsDesignerViewContent : IDisposable, IServiceProvider, IClipboardHandler, IUndoHandler, IHasPropertyContainer, IContextHelpProvider, IToolsHost
	{
		DesignSurface designSurface;
		bool disposing;
		
		DesignerLoader loader;
		FormsDesignerUndoEngine undoEngine;
        DesignerResourceService designerResourceService;
		TypeResolutionService typeResolutionService;
        string generatedCode = null;
        VisualPascalABC.CodeFileDocumentControl codeFileDocument; //roman//
        public delegate void ModifyDelegate();
        public ModifyDelegate Modify;


        //roman//
        static FormsDesignerViewContent()
        {
            ICSharpCode.Core.WinForms.WinFormsResourceService.AddToBitmapCache("Icons.16x16.SideBarDocument", global::VisualPascalABC.Properties.Resources.Pointer);
        }

        private void PropertyPadEditorKeyPress(object sender, KeyPressEventArgs e)
        {
            if (PropertyPad.Grid.SelectedGridItem.Label == "(Name)")
            {
                if (e.KeyChar == (char)Keys.Back || isNameValid((string)PropertyPad.Grid.SelectedGridItem.Value + e.KeyChar))
                {
                    e.Handled = false;
                }
                else
                    e.Handled = true;
            }
            else
                e.Handled = false;
        }

        private static bool isNameValid(string name)
        {
            if (name == null)
                return false;
            if (name.Length == 0)
                return false;
            if (!(name[0] == '_' || (name[0] >= 'a' && name[0] <= 'z') || (name[0] >= 'A' && name[0] <= 'Z')))
                return false;
            for (int i = 1; i < name.Length; i++)
                if (!(name[i] == '_' || (name[i] >= 'a' && name[i] <= 'z') || (name[i] >= 'A' && name[i] <= 'Z') || char.IsDigit(name[i])))
                    return false;
            return true;
        }
        //roman//




        public System.CodeDom.CodeCompileUnit CodeCompileUnit
        {
            get
            {
                return (loader as CodeDomHostLoader).GetCodeCompileUnit();
            }
        }
        public string FileName
        {
            get
            {
                return codeFileDocument.FileName;
            }
        }

        public void ResetGeneratedCode()
        {
            generatedCode = null;
        }

        public string GeneratedCode
        {
            get
            {
                generatedCode = (loader as CodeDomHostLoader).GetCode();
                return generatedCode;
            }
        }

        private bool isDirty;
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }
            set
            {
                isDirty = value;
                if (value && Modify != null) Modify();
            }
        }

		public DesignSurface DesignSurface {
			get {
				return designSurface;
			}
		}

        public static SharpDevelopSideBar FormsDesignerToolBox
        {
            get
            {
                return ToolboxProvider.FormsDesignerSideBar;
            }
        }

        public static PropertyPad PropertyPad
        {
            get
            {
                return PropertyContainer.PropertyPad;
            }
        }

		public IDesignerHost Host {
			get {
				if (designSurface == null)
					return null;
				return (IDesignerHost)designSurface.GetService(typeof(IDesignerHost));
			}
		}
		
		public FormsDesignerViewContent(VisualPascalABC.CodeFileDocumentControl documentControl) //roman//
			: base()
		{
            if (documentControl == null)
                throw new ArgumentNullException("documentControl");
            this.codeFileDocument = documentControl;
			if (!FormKeyHandler.inserted) {
				FormKeyHandler.Insert();
			}
		}
		
		bool inMasterLoadOperation;
		
		public void LoadDesigner(string formFile)
		{
            UnloadDesigner();
			DefaultServiceContainer serviceContainer = new DefaultServiceContainer();
			serviceContainer.AddService(typeof(System.Windows.Forms.Design.IUIService), new UIService());
			serviceContainer.AddService(typeof(System.Drawing.Design.IToolboxService), ToolboxProvider.ToolboxService);
			
			serviceContainer.AddService(typeof(IHelpService), new HelpService());
			serviceContainer.AddService(typeof(System.Drawing.Design.IPropertyValueUIService), new PropertyValueUIService());
			
			AmbientProperties ambientProperties = new AmbientProperties();
			serviceContainer.AddService(typeof(AmbientProperties), ambientProperties);
			this.typeResolutionService = new TypeResolutionService();
			serviceContainer.AddService(typeof(ITypeResolutionService), this.typeResolutionService);
			serviceContainer.AddService(typeof(DesignerOptionService), new SharpDevelopDesignerOptionService());
			serviceContainer.AddService(typeof(ITypeDiscoveryService), new TypeDiscoveryService());
			serviceContainer.AddService(typeof(MemberRelationshipService), new DefaultMemberRelationshipService());
			
			designSurface = CreateDesignSurface(serviceContainer);
			designSurface.Loading += this.DesignerLoading;
			designSurface.Loaded += this.DesignerLoaded;
			designSurface.Flushed += this.DesignerFlushed;
			designSurface.Unloading += this.DesignerUnloading;

            designerResourceService = new DesignerResourceService(this);//roman//
			serviceContainer.AddService(typeof(System.ComponentModel.Design.IResourceService), designerResourceService);
            loader = new CodeDomHostLoader(this.Host, formFile, FileName);

			serviceContainer.AddService(typeof(System.ComponentModel.Design.IMenuCommandService), new ICSharpCode.FormsDesigner.Services.MenuCommandService(Host));
			ICSharpCode.FormsDesigner.Services.EventBindingService eventBindingService = new ICSharpCode.FormsDesigner.Services.EventBindingService(Host);
			serviceContainer.AddService(typeof(System.ComponentModel.Design.IEventBindingService), eventBindingService);
			
            designSurface.BeginLoad(loader);
			
			if (!designSurface.IsLoaded) {
				throw new FormsDesignerLoadException(FormatLoadErrors(designSurface));
			}
			
			undoEngine = new FormsDesignerUndoEngine(Host);
			serviceContainer.AddService(typeof(UndoEngine), undoEngine);
			
			IComponentChangeService componentChangeService = (IComponentChangeService)designSurface.GetService(typeof(IComponentChangeService));
			componentChangeService.ComponentChanged += ComponentChanged;
			componentChangeService.ComponentAdded   += ComponentListChanged;
			componentChangeService.ComponentRemoved += ComponentListChanged;
			componentChangeService.ComponentRename  += ComponentListChanged;
			this.Host.TransactionClosed += TransactionClose;
			
			ISelectionService selectionService = (ISelectionService)designSurface.GetService(typeof(ISelectionService));
			selectionService.SelectionChanged  += SelectionChangedHandler;
			
			if (IsTabOrderMode) { // fixes SD2-1015
				tabOrderMode = false; // let ShowTabOrder call the designer command again
				ShowTabOrder();
			}
			
			UpdatePropertyPad();
			
			hasUnmergedChanges = false;
            MakeDirty();

            PropertyGrid grid = PropertyPad.Grid;
            
            var gridView = (Control)grid.GetType().GetField("gridView", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(grid);
            var edit = (Control)gridView.GetType().GetField("edit", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(gridView);
            edit.KeyPress += PropertyPadEditorKeyPress;
            edit.ContextMenu = new ContextMenu();
		}
		//roman//
        public void SaveResources()
        {
            if (designerResourceService != null)
            {
                designerResourceService.Save();
            }
        }
        //roman//
		
		bool hasUnmergedChanges;
		
		void MakeDirty()
		{
			hasUnmergedChanges = true;
            IsDirty = true;
			//System.Windows.Input.CommandManager.InvalidateRequerySuggested();
		}
		
		bool shouldUpdateSelectableObjects = false;
		
		void TransactionClose(object sender, DesignerTransactionCloseEventArgs e)
		{
			if (shouldUpdateSelectableObjects) {
				// update the property pad after the transaction is *really* finished
				// (including updating the selection)
				//roman//WorkbenchSingleton.SafeThreadAsyncCall(UpdatePropertyPad);
				VisualPascalABC.VisualPABCSingleton.MainForm.BeginInvoke((Action)UpdatePropertyPad, new object[0]);//roman//
			    shouldUpdateSelectableObjects = false;
			}
            VisualPascalABC.VisualPABCSingleton.MainForm.UpdateUndoRedoEnabled();
		}

		void ComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			bool loading = this.loader != null && this.loader.Loading;
            if (!loading && !unloading) {
				try {
					this.MakeDirty();
				} catch (Exception ex) {
					MessageService.ShowException(ex);
				}
			}
            VisualPascalABC.VisualPABCSingleton.MainForm.UpdateUndoRedoEnabled();
		}

		void ComponentListChanged(object sender, EventArgs e)
		{
			bool loading = this.loader != null && this.loader.Loading;
			if (!loading && !unloading) {
				shouldUpdateSelectableObjects = true;
				this.MakeDirty();
			}
            VisualPascalABC.VisualPABCSingleton.MainForm.UpdateUndoRedoEnabled();
		}

		public void UnloadDesigner()
		{
			designSurfaceManager.ActiveDesignSurface = null;
			
			if (designSurface != null) {
				designSurface.Loading -= this.DesignerLoading;
				designSurface.Loaded -= this.DesignerLoaded;
				designSurface.Flushed -= this.DesignerFlushed;
				designSurface.Unloading -= this.DesignerUnloading;
				
				IComponentChangeService componentChangeService = designSurface.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (componentChangeService != null) {
					componentChangeService.ComponentChanged -= ComponentChanged;
					componentChangeService.ComponentAdded   -= ComponentListChanged;
					componentChangeService.ComponentRemoved -= ComponentListChanged;
					componentChangeService.ComponentRename  -= ComponentListChanged;
				}
				if (this.Host != null) {
					this.Host.TransactionClosed -= TransactionClose;
				}
				
				ISelectionService selectionService = designSurface.GetService(typeof(ISelectionService)) as ISelectionService;
				if (selectionService != null) {
					selectionService.SelectionChanged -= SelectionChangedHandler;
				}
				
				designSurface.Unloaded += delegate {
					ServiceContainer serviceContainer = designSurface.GetService(typeof(ServiceContainer)) as ServiceContainer;
					if (serviceContainer != null) {
						// Workaround for .NET bug: .NET unregisters the designer host only if no component throws an exception,
						// but then in a finally block assumes that the designer host is already unloaded.
						// Thus we would get the confusing "InvalidOperationException: The container cannot be disposed at design time"
						// when any component throws an exception.
						
						// See http://community.sharpdevelop.net/forums/p/10928/35288.aspx
						// Reproducible with a custom control that has a designer that crashes on unloading
						// e.g. http://www.codeproject.com/KB/toolbars/WinFormsRibbon.aspx
						
						// We work around this problem by unregistering the designer host manually.
						try {
							var services = (Dictionary<Type, object>)typeof(ServiceContainer).InvokeMember(
								"Services",
								BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.NonPublic,
								null, serviceContainer, null);
							foreach (var pair in services.ToArray()) {
								if (pair.Value is IDesignerHost) {
									serviceContainer.GetType().InvokeMember(
										"RemoveFixedService",
										BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic,
										null, serviceContainer, new object[] { pair.Key });
								}
							}
						} catch (Exception ex) {
							LoggingService.Error(ex);
						}
					}
				};
				try {
					designSurface.Dispose();
				} catch (ExceptionCollection exceptions) {
					foreach (Exception ex in exceptions.Exceptions) {
						LoggingService.Error(ex);
					}
				} finally {
					designSurface = null;
				}
			}
			
			this.typeResolutionService = null;
			this.loader = null;
		}

		readonly PropertyContainer propertyContainer = new PropertyContainer();

		public PropertyContainer PropertyContainer {
			get {
				return propertyContainer;
			}
		}

		public void ShowHelp()
		{
			if (Host == null) {
				return;
			}
			
			ISelectionService selectionService = (ISelectionService)Host.GetService(typeof(ISelectionService));
			if (selectionService != null) {
				Control ctl = selectionService.PrimarySelection as Control;
				if (ctl != null) {
					ICSharpCode.SharpDevelop.HelpProvider.ShowHelp(ctl.GetType().FullName);
				}
			}
		}

		public void LoadAndDisplayDesigner(string formFile)
		{
			try {

                LoadDesigner(formFile);
				
			} catch (Exception e) {
				
				if (e.InnerException is FormsDesignerLoadException) {
					throw new FormsDesignerLoadException(e.InnerException.Message, e);
				} else if (e is FormsDesignerLoadException) {
					throw;
				} else if (designSurface != null && !designSurface.IsLoaded && designSurface.LoadErrors != null) {
					throw new FormsDesignerLoadException(FormatLoadErrors(designSurface), e);
				} else {
					throw;
				}
				
			}
		}

		void DesignerLoading(object sender, EventArgs e)
		{
			this.reloadPending = false;
			this.unloading = false;
		}

		void DesignerUnloading(object sender, EventArgs e)
		{
			this.unloading = true;
		}

		bool reloadPending;
		bool unloading;

		void DesignerLoaded(object sender, LoadedEventArgs e)
		{
			// This method is called when the designer has loaded.
			this.reloadPending = false;
			this.unloading = false;
			
			if (e.HasSucceeded) {
				// Display the designer on the view content
				Control designView = (Control)this.designSurface.View;
				
				designView.BackColor = Color.White;
				designView.RightToLeft = RightToLeft.No;
				// Make sure auto-scaling is based on the correct font.
				// This is required on Vista, I don't know why it works correctly in XP
				designView.Font = System.Windows.Forms.Control.DefaultFont;

                MakeDirty();
				designSurfaceManager.ActiveDesignSurface = this.designSurface;
                this.UserContent = designView;
				this.UpdatePropertyPad();
			} else {
				// This method can not only be called during initialization,
				// but also when the designer reloads itself because of
				// a language change.
				// When a load error occurs there, we are not somewhere
				// below the Load method which handles load errors.
				// That is why we create an error text box here anyway.
			}
		}

		void DesignerFlushed(object sender, EventArgs e)
		{
			this.hasUnmergedChanges = false;
            IsDirty = false;
            SaveResources();
		}

		static string FormatLoadErrors(DesignSurface designSurface)
		{
			StringBuilder sb = new StringBuilder();
			foreach(Exception le in designSurface.LoadErrors) {
				sb.AppendLine(le.ToString());
				sb.AppendLine();
			}
			return sb.ToString();
		}

		public virtual void MergeFormChanges()
		{
			designSurface.Flush();
			hasUnmergedChanges = false;
		}

		public void ShowSourceCode()
		{
			//roman// this.WorkbenchWindow.ActiveViewContent = this.PrimaryViewContent;
		}

		void IsActiveViewContentChangedHandler(object sender, EventArgs e)
		{			
		}

		public void Dispose()
		{
			disposing = true;
			try {
				// base.Dispose() is called first because it may trigger a call
				// to SaveInternal which requires the designer to be loaded.
				//roman// base.Dispose();
			} finally {
				this.UnloadDesigner();
			}
		}

		void SelectionChangedHandler(object sender, EventArgs args)
		{
			UpdatePropertyPadSelection((ISelectionService)sender);
		}

		void UpdatePropertyPadSelection(ISelectionService selectionService)
		{
			ICollection selection = selectionService.GetSelectedComponents();
			object[] selArray = new object[selection.Count];
			selection.CopyTo(selArray, 0);
			propertyContainer.SelectedObjects = selArray;
			//System.Windows.Input.CommandManager.InvalidateRequerySuggested();
		}

		protected void UpdatePropertyPad()
		{
			if (Host != null) {
				propertyContainer.Host = Host;
				propertyContainer.SelectableObjects = Host.Container.Components;
				ISelectionService selectionService = (ISelectionService)Host.GetService(typeof(ISelectionService));
				if (selectionService != null) {
					UpdatePropertyPadSelection(selectionService);
				}
			}
		}

        Control userContent;

        public Control UserContent
        {
            get
            {
                /*SDWindowsFormsHost host = userContent as SDWindowsFormsHost;
                return host != null ? host.Child : null;*/
                //roman//
                return userContent;
            }
            set
            {
                /*SDWindowsFormsHost host = userContent as SDWindowsFormsHost;
                if (value == null)
                {
                    userContent = null;
                    if (host != null)
                        host.Dispose();
                    return;
                }
                if (host != null)
                {
                    if (host.IsDisposed)
                    {
                        host = null;
                    }
                    else if (host.Child == value)
                    {
                        return;
                    }
                }
                if (host == null)
                {
                    host = new SDWindowsFormsHost(true);
                    host.ServiceObject = this;
                    host.DisposeChild = false;
                }
                host.Child = value;
                userContent = host;*/

                userContent = value;
            }
        }

		#region IUndoHandler implementation
		public bool EnableUndo {
			get {
				if (undoEngine != null) {
					return undoEngine.EnableUndo;
				}
				return false;
			}
		}
		public bool EnableRedo {
			get {
				if (undoEngine != null) {
					return undoEngine.EnableRedo;
				}
				return false;
			}
		}

		public virtual void Undo()
		{
			if (undoEngine != null) {
				undoEngine.Undo();
			}
		}

		public virtual void Redo()
		{
			if (undoEngine != null) {
				undoEngine.Redo();
			}
		}
		#endregion

		#region IClipboardHandler implementation
		bool IsMenuCommandEnabled(CommandID commandID)
		{
			if (designSurface == null) {
				return false;
			}
			
			IMenuCommandService menuCommandService = (IMenuCommandService)designSurface.GetService(typeof(IMenuCommandService));
			if (menuCommandService == null) {
				return false;
			}
			
			System.ComponentModel.Design.MenuCommand menuCommand = menuCommandService.FindCommand(commandID);
			if (menuCommand == null) {
				return false;
			}
			
			//int status = menuCommand.OleStatus;
			return menuCommand.Enabled;
		}

		public bool EnableCut {
			get {
				return IsMenuCommandEnabled(StandardCommands.Cut);
			}
		}

		public bool EnableCopy {
			get {
				return IsMenuCommandEnabled(StandardCommands.Copy);
			}
		}

		const string ComponentClipboardFormat = "CF_DESIGNERCOMPONENTS";
		public bool EnablePaste {
			get {
				return IsMenuCommandEnabled(StandardCommands.Paste);
			}
		}

		public bool EnableDelete {
			get {
				return IsMenuCommandEnabled(StandardCommands.Delete);
			}
		}

		public bool EnableSelectAll {
			get {
				return designSurface != null;
			}
		}

		public void Cut()
		{
			IMenuCommandService menuCommandService = (IMenuCommandService)designSurface.GetService(typeof(IMenuCommandService));
			menuCommandService.GlobalInvoke(StandardCommands.Cut);
		}

		public void Copy()
		{
			IMenuCommandService menuCommandService = (IMenuCommandService)designSurface.GetService(typeof(IMenuCommandService));
			menuCommandService.GlobalInvoke(StandardCommands.Copy);
		}

		public void Paste()
		{
			IMenuCommandService menuCommandService = (IMenuCommandService)designSurface.GetService(typeof(IMenuCommandService));
			menuCommandService.GlobalInvoke(StandardCommands.Paste);
		}

		public void Delete()
		{
			IMenuCommandService menuCommandService = (IMenuCommandService)designSurface.GetService(typeof(IMenuCommandService));
			menuCommandService.GlobalInvoke(StandardCommands.Delete);
		}

		public void SelectAll()
		{
			IMenuCommandService menuCommandService = (IMenuCommandService)designSurface.GetService(typeof(IMenuCommandService));
			menuCommandService.GlobalInvoke(StandardCommands.SelectAll);
		}
		#endregion

		#region Tab Order Handling
		bool tabOrderMode = false;
		public virtual bool IsTabOrderMode {
			get {
				return tabOrderMode;
			}
		}

		public virtual void ShowTabOrder()
		{
			if (!IsTabOrderMode) {
				IMenuCommandService menuCommandService = (IMenuCommandService)designSurface.GetService(typeof(IMenuCommandService));
				menuCommandService.GlobalInvoke(StandardCommands.TabOrder);
				tabOrderMode = true;
			}
		}

		public virtual void HideTabOrder()
		{
			if (IsTabOrderMode) {
				IMenuCommandService menuCommandService = (IMenuCommandService)designSurface.GetService(typeof(IMenuCommandService));
				menuCommandService.GlobalInvoke(StandardCommands.TabOrder);
				tabOrderMode = false;
			}
		}
		#endregion

		protected void MergeAndUnloadDesigner()
		{
			propertyContainer.Clear();
            MergeFormChanges();
			UnloadDesigner();
		}

        public void ExecuteCommand(CommandID id)
        {
            IServiceContainer sc = Host as IServiceContainer;
            IMenuCommandService mcs = sc.GetService(typeof(IMenuCommandService)) as IMenuCommandService;
            mcs.GlobalInvoke(id);
        }

		public virtual object ToolsContent {
			get { return ToolboxProvider.FormsDesignerSideBar; }
		}

		#region Design surface manager (static)

		static readonly DesignSurfaceManager designSurfaceManager = new DesignSurfaceManager();

		public static DesignSurface CreateDesignSurface(IServiceProvider serviceProvider)
		{
			return designSurfaceManager.CreateDesignSurface(serviceProvider);
		}

		#endregion

		#region Debugger event handling (to prevent designer reload while debugger is starting)

		void DebugStarting(object sender, EventArgs e)
		{
			if (designSurfaceManager.ActiveDesignSurface != this.DesignSurface ||
			    !this.reloadPending)
				return;
			
			// The designer loader does not reload immediately,
			// but only when the Application.Idle event is raised.
			// When the IsActiveViewContentChangedHandler has been called because of the
			// layout change prior to starting the debugger, and it has
			// initiated a reload because of a changed referenced assembly,
			// the reload can interrupt the starting of the debugger.
			// To prevent this, we explicitly raise the Idle event here.
			LoggingService.Debug("Forms designer: DebugStarting raises the Idle event to force pending reload now");
			Cursor oldCursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			try {
				Application.RaiseIdle(EventArgs.Empty);
			} finally {
				Cursor.Current = oldCursor;
			}
		}

		#endregion

        #region IServiceProvider
        ServiceContainer services = new ServiceContainer();

        public object GetService(Type serviceType)
        {
            object obj = services.GetService(serviceType);
            if (obj == null)
            {
                if (serviceType.IsInstanceOfType(this))
                    return this;
            }
            return obj;
        }
        #endregion
    }
}
