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
    using System.Collections;
	using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Diagnostics;

	/// Links a component to its container.
    internal class SampleDesignSite : ISite, IDictionaryService {
    
        private static Attribute[] designerNameAttribute = new Attribute[] {new SampleDesignerNameAttribute(true)};
        
        private IComponent          component;
        private SampleDesignerHost  host;
        private string              name;
		private Hashtable			dictionary;
        
        internal SampleDesignSite(SampleDesignerHost host, string name) {
            this.host = host;
            this.component = null;
            this.name = name;
        }


        // IServiceProvider

		/// Pass on service requests to the host.
        public object GetService(Type service) {
			if (service == typeof(IDictionaryService)) 
			{
				return this;
			}
            IServiceProvider sp = (IServiceProvider)host;
            return sp.GetService(service);
        }


        // ISite

		/// Return this site's component.
        public IComponent Component {
            get {
                Debug.Assert(component != null, "Need the component before we've established it");
                return component;
            }
        }

		/// Get the component container.
        public IContainer Container {
            get {
                return host.Container;
            }
        }

		/// Get/Set the name of this component.
        public string Name {
            get {
                return name;
            }
            set {
                if (value == null) {
                    throw new ArgumentException("Bad Name Value - cannot be null");
                }
                
                if (value.Equals(name)) return; // No need to rename the same name.

                host.OnComponentRename(new ComponentRenameEventArgs(component, name, value));
                name = value;
            }
        }

		/// Are we in design mode?
        public bool DesignMode {
            get {
                return true;
            }
        }

        /// Set the component for this site (can only be done once).
        internal void SetComponent(IComponent component) {
            Debug.Assert(this.component == null, "Cannot set a component twice");
            this.component = component;

            if (this.name == null) {
                this.name = host.GetNewComponentName(component.GetType());
            }
        }

		/// Set a new name on this component.
        internal void SetName(string newName) {
            name = newName;
        }

		#region Implementation of IDictionaryService

        //  IDictionaryService is a per-component cache used by
        //  many things in the designer.  This must be implemented
        //  on a site that supports design mode.  This is the only
        //  service that is per-component.  All other services are
        //  per-designer host.

		/// Get a value from a key.
		object IDictionaryService.GetValue(object key)
		{
            if (dictionary != null) 
            {
                return dictionary[key];
            }
			return null;
		}

		/// Set a value with a key.
		void IDictionaryService.SetValue(object key, object value)
		{
            if (dictionary == null) 
            {
                dictionary = new Hashtable();
            }
            dictionary[key] = value;
		}

		/// Get a key from a value.
		object IDictionaryService.GetKey(object value)
		{
            if (dictionary != null) 
            {
                foreach(DictionaryEntry de in dictionary) 
                {
                    if (object.Equals(de.Value, value))
                    {
                        return de.Key;
                    }
                }
            }
			return null;
		}
		#endregion
    }

}

