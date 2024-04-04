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
namespace SampleDesignerHost {

    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Windows.Forms;

    // This class represents a single selected object.
    internal class SampleSelectionItem {

        // Public objects this selection deals with
        private Component                 component;      // the component that's selected
        private SampleSelectionService    selectionMgr;   // host interface
        private bool                      primary;        // is this the primary selection?

        ///  Constructor
        internal SampleSelectionItem(SampleSelectionService selectionMgr, Component component) {
            this.component = component;
            this.selectionMgr = selectionMgr;
        }

        internal Component Component {
            get {
                return component;
            }
        }

        ///     Determines if this is the primary selection.  The primary selection uses a
        ///     different set of grab handles and generally supports sizing. The caller must
        ///     verify that there is only one primary object; this merely updates the
        ///     UI.
        internal virtual bool Primary {
            get {
                return primary;
            }
            set {
                if (this.primary != value) {
                    this.primary = value;
                    if (SelectionItemInvalidate != null)
                        SelectionItemInvalidate(this, EventArgs.Empty);
                }
            }
        }

        internal event EventHandler SampleSelectionItemDispose ;
        internal event EventHandler SelectionItemInvalidate ;

        ///     Disposes of this selection.  We dispose of our region object if it still exists and we
        ///     invalidate our UI so that we don't leave any turds laying around.
        internal virtual void Dispose() {
            if (primary) {
                selectionMgr.SetPrimarySelection((SampleSelectionItem)null);
            }

            if (SampleSelectionItemDispose != null)
                SampleSelectionItemDispose(this, EventArgs.Empty);
        }
    }

}
