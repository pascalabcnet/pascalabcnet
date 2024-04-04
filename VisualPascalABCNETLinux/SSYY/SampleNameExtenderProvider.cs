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

    ///      This extender provider provides a Name property to all components
    ///      Its used by the DesignerHost so that all components, not just controls 
    ///      can have a name property
    [
    ProvideProperty("Name", typeof(IComponent))
    ]
    internal class SampleNameExtenderProvider : IExtenderProvider {

		// Any "Name" properties that we give out will have this attribute on them,
		// so that we can tell it from any other "Name" properties which might be out there.
		// We use this particular member as a filter to find the "Name"s we provided.
		//
        private static Attribute[] designerNameAttribute = new Attribute[] {new SampleDesignerNameAttribute(true)};

        private SampleDesignerHost host;

        internal SampleNameExtenderProvider(SampleDesignerHost host) {
            this.host = host;
        }

        public virtual bool CanExtend(object o) {

            // We don't add name or modifiers to the base component.
            if (o == Host.RootComponent) {
                return false;
            }

            // Now see if this object is inherited.  If so, then we don't want to extend.
            if (!TypeDescriptor.GetAttributes(o)[typeof(InheritanceAttribute)].Equals(InheritanceAttribute.NotInherited)) {
                return false;
            }

            return true;
        }

        public SampleDesignerHost Host {
            get {
                return host;
            }
        }

        ///     This is an extender property that we offer to all components
        ///     on the form.  It implements the "Name" property.
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        SampleDesignerName(true), // this is one of our "Name"s
        ParenthesizePropertyName(true),
        MergableProperty(false),
        Description("The name property for the component"),
        Category("Design")
        ]
        public virtual string GetName(IComponent comp) {
            ISite site = comp.Site;
            if (site != null) {
                return site.Name;
            }
            return null;
        }

        public void SetName(IComponent comp, string newName) {

            // trim any spaces off of the name
            newName = newName.Trim();

            SampleDesignSite cs = (SampleDesignSite) comp.Site;
            if (newName.Equals(cs.Name)) return;

            // If the rename is only a case change 
            if (string.Compare(newName, cs.Name, true) != 0) {
                Host.CheckName(newName);
            }

			// Find the name property provided by us.
			//
            ((IComponentChangeService)Host).OnComponentChanging(comp, TypeDescriptor.GetProperties(comp, designerNameAttribute)["Name"]);

            Host.Sites.Remove(cs.Name);
            Host.Sites[newName] = cs;

            string oldName = cs.Name;
            cs.SetName(newName);

            Host.OnComponentRename(new ComponentRenameEventArgs(comp, oldName, newName));
            ((IComponentChangeService)Host).OnComponentChanged(comp, TypeDescriptor.GetProperties(comp, designerNameAttribute)["Name"], oldName, newName);
        }
    }

    internal class SampleInheritedNameExtenderProvider : SampleNameExtenderProvider {

        internal SampleInheritedNameExtenderProvider(SampleDesignerHost host) : base(host) {
        }

        public override bool CanExtend(object o) {
            // We don't add name or modifiers to the base component.
            if (o == Host.RootComponent) {
                return false;
            }

            // Now see if this object is inherited.  If so, then we are interested in it.
            if (!TypeDescriptor.GetAttributes(o)[typeof(InheritanceAttribute)].Equals(InheritanceAttribute.NotInherited)) {
                return true;
            }

            return false;
        }

        [ReadOnly(true)]
        public override string GetName(IComponent comp) {
            return base.GetName(comp);
        }
    }

}

