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
    using System.Diagnostics;
    using System.Collections;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Drawing.Design;
	using System.Windows.Forms.Design;
    
    ///     This class implements the actual document window for a form design.  This is the
    ///     window that the base designer will be parented into.  
    internal class SampleDocumentWindow : Control {

		private IDesignerHost           designerHost;
        private Control                 designerView;

        internal SampleDocumentWindow(IDesignerHost designerHost) {
            SetStyle(ControlStyles.ResizeRedraw, true);
            this.designerHost = designerHost;
            TabStop = false;
            Visible = false; // The host will tell us when to be visible.
            Text = "DocumentWindow";
            BackColor = SystemColors.Window;
			this.AllowDrop = true;
        }

        ///      Allows you to affect the visibility of the document.
        public bool DocumentVisible {
            get {
                return (designerView == null ? false : designerView.Visible);
            }

            set {
                if (designerView != null) {
                    designerView.Visible = value;
                }
            }
        }

        ///     Disposes of the document.
        protected override void Dispose(bool disposing) {
            if (disposing) {
                SetDesigner(null);

                designerHost = null;
            }
            base.Dispose(disposing);
        }

		/// The document window itself never really has focus. Instead
		/// it passes focus to the designerView.
        protected override void OnGotFocus(EventArgs e) {
            if (designerView != null) {
                designerView.Focus();
            }
            else {
                base.OnGotFocus(e);
            }
        }

		/// Likewise, when we check the document window for focus,
		/// we really want to know if the designerView has it. The only
		/// time we do this check is when intercepting keyboard messages.
		public override bool Focused
		{
			get
			{
				if (designerView != null)
				{
					return designerView.Focused;
				}
				else
				{
					return base.Focused;
				}
			}
		}

        ///     Paints the surface of the document window with the given error collection.
        public void ReportErrors(ICollection errors) {
			if (errors.Count > 0)
			{
				ListBox list = new ListBox();
				foreach(object err in errors) 
				{
					list.Items.Add(err);
				}

				list.Dock = DockStyle.Fill;
				list.Height = 200;
				Controls.Add(list);
			}
        }

        ///     Establishes the given designer as the main top level designer for the document.
        public void SetDesigner(IRootDesigner document) {

            if (designerView != null) {
				Controls.Clear();
                designerView.Dispose();
				designerView = null;
            }
            
            if (document != null) {
                // Demand create the designer holder, if it doesn't already exist.
                ViewTechnology[] technologies = document.SupportedTechnologies;
                bool supportedTechnology = false;
                
				// Search for supported technologies that we know how to design.
				// In our case, we only know how to design WindowsForms.
				//
                foreach(ViewTechnology tech in technologies) {
                    switch(tech) {
                        case ViewTechnology.WindowsForms: {
                            designerView = (Control)document.GetView(ViewTechnology.WindowsForms);
                            designerView.Dock = DockStyle.Fill;
							Controls.Add(designerView);
                            supportedTechnology = true;
                            break;
                        }
                    }
                    
                    // Stop looping if we found one
                    //
                    if (supportedTechnology) {
                        break;
                    }
                }
                
                // If we didn't find a supported technology, report it.
                //
                if (!supportedTechnology) {
                    throw new Exception("Unsupported Technology " + designerHost.RootComponent.GetType().FullName);
                }

            }
        }
    }
}

