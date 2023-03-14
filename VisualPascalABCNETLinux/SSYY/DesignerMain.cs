using System;
using System.Drawing.Design;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Windows.Forms.Design;
using System.Reflection;
using System.CodeDom;
using System.Collections;
using SampleDesignerHost;
using System.IO;
using VisualPascalABC;

namespace SampleDesignerApplication
{
    public class CodeStrings
    {
        public string PascalABCCode;
        public string XMLCode;

        public void Reset()
        {
            PascalABCCode = null;
            XMLCode = null;
        }
    }

    public class DesignerMain : Component
    {
        public SampleDesignerHost.SampleDesignerHost host;
        public SampleDesignerLoader loader;
        private ServiceContainer hostingServiceContainer;
        private ServiceRequests serviceRequests;
        //private Form1 pabcform;
        private PropertyGrid propertyGrid;
        private ToolBoxPane toolbox;
        private Panel panelMain;
        private KeystrokeMessageFilter filter;
        public CodeStrings generatedCode;

        public DesignerMain(PropertyGrid props, ToolBoxPane t, Panel p)
        {
            propertyGrid = props;
            toolbox = t;
            panelMain = p;
            hostingServiceContainer = new ServiceContainer();
            generatedCode = new CodeStrings();
            hostingServiceContainer.AddService(typeof(CodeStrings), generatedCode);
            hostingServiceContainer.AddService(typeof(PropertyGrid), propertyGrid);
            hostingServiceContainer.AddService(typeof(ToolBoxPane), toolbox);
        }

        /// This method is called whenever we create a new designer or
        /// load one from existing XML.
        private void CreateDesigner(SampleDesignerLoader loader)
        {
            // Our loader will handle loading the file (or creating a blank one).
            host = new SampleDesignerHost.SampleDesignerHost(hostingServiceContainer);

            // The limited events tab functionality we have requires a special kind of
            // property grid site.
            //
            propertyGrid.Site = new PropertyGridSite(host as IServiceProvider, this);
            propertyGrid.PropertyTabs.AddTabType(typeof(EventsTab));

            host.LoadDocument(loader);
            this.loader = loader;

            // The toolbox needs access to the IToolboxService and the designers.
            toolbox.Host = host;

            // Initialize our document window.
            host.View.Dock = DockStyle.Fill;
            host.View.Visible = true;
            panelMain.Controls.Add(host.View);

            // These are normally unavailable if we've got no host created.
            // Otherwise we gets lots of null reference exceptions.
            //
            //tabControl.Visible = true;
            //menuItemSave.Enabled = true;
            //menuItemSaveAs.Enabled = true;
            //menuItemEdit.Enabled = true;
            //menuItemView.Enabled = true;
            //menuItemLayout.Enabled = true;
            //menuItemDebug.Enabled = true;

            // This IMessageFilter is used to intercept and decipher keyboard presses
            // that might be instructions for our designer. We have to do it this way
            // since we can't have our designer raise KeyPress events.
            //
            filter = new KeystrokeMessageFilter(host);
            Application.AddMessageFilter(filter);
        }

        /// This is called to destroy our designer (when we load a new one for example).
        /// It returns true if our environment has been cleared, false if not (if someone
        /// cancels a dispose when prompted to save changes).
        private bool DestroyDesigner()
        {
            if (loader != null)
            {
                if (loader.PromptDispose())
                {
                    // Again, bad things happen if there's no host loaded and 
                    // certain buttons are pressed in our TabControl or MainMenu.
                    //
                    //tabControl.Visible = false;
                    //menuItemSave.Enabled = false;
                    //menuItemSaveAs.Enabled = false;
                    //menuItemEdit.Enabled = false;
                    //menuItemView.Enabled = false;
                    //menuItemLayout.Enabled = false;
                    //menuItemDebug.Enabled = false;

                    // Get rid of our document window.
                    panelMain.Controls.Clear();

                    // No need to filter for designer-intended keyboard strokes now.
                    Application.RemoveMessageFilter(filter);
                    filter = null;

                    // Get rid of that property grid site so it doesn't ask for
                    // any more services from our soon-to-be-disposed host.
                    //
                    propertyGrid.Site = null;

                    host.Dispose();
                    loader = null;
                    host = null;
                    return true;
                }
                return false;
            }
            return true;
        }

        public void CreateNew()
        {
            if (DestroyDesigner()) // make sure we're clear for a new designer
            {
                // A loader created with no parameters creates a blank document.
                SampleDesignerLoader designerLoader = new SampleDesignerLoader();
                CreateDesigner(designerLoader);
            }
        }

        public void CreateFromXML(string file_name)
        {
            if (DestroyDesigner()) // make sure we're clear for a new designer
            {
                SampleDesignerLoader designerLoader = new SampleDesignerLoader(file_name);
                CreateDesigner(designerLoader);
                designerLoader.dirty = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (host != null)
                {
                    host.Dispose();
                }

            }
            base.Dispose(disposing);
        }

        public void ExecuteCommand(CommandID id)
        {
            IServiceContainer sc = host as IServiceContainer;
            IMenuCommandService mcs = sc.GetService(typeof(IMenuCommandService)) as IMenuCommandService;
            mcs.GlobalInvoke(id);
        }

    }
}
