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
using System.ComponentModel;
using System.ComponentModel.Design;

namespace SampleDesignerHost
{
    public class SYNamePropertyDescriptor : PropertyDescriptor
    {
        private static bool is_letter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        private static bool is_valid_char(char c)
        {
            return is_letter(c) || c == '_' || c == '0' || (c >= '1' && c <= '9');
        }

        public static bool is_valid_id(string name)
        {
            if (name == null || name == "") return false;
            if (!is_letter(name[0])) return false;
            for (int i = 1; i < name.Length; ++i)
            {
                if (!is_valid_char(name[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private PropertyDescriptor _d;
        AttributeCollection _ac;

        public SYNamePropertyDescriptor(PropertyDescriptor descr):base(descr)
        {
            _d = descr;
            int count = _d.Attributes.Count;
            Attribute[] attrs = new Attribute[count];
            Attribute vis_att = new System.ComponentModel.BrowsableAttribute(true);
            for (int i = 0; i < count; ++i)
            {
                if (_d.Attributes[i].TypeId == vis_att.TypeId)
                {
                    attrs[i] = vis_att;
                }
                else
                {
                    attrs[i] = _d.Attributes[i];
                }
            }
            _ac = new AttributeCollection(attrs);
            //Array.Resize(attrs, attrs.Length + 1);
            //attrs[attrs.Length - 1] = new System.ComponentModel.BrowsableAttribute(true);
        }

        public override bool CanResetValue(object component)
        {
            return _d.CanResetValue(component);
        }

        public override Type ComponentType
        {
            get { return _d.ComponentType; }
        }

        public override object GetValue(object component)
        {
            return _d.GetValue(component);
        }

        public override bool IsReadOnly
        {
            get { return _d.IsReadOnly; }
        }

        public override Type PropertyType
        {
            get { return _d.PropertyType; }
        }

        public override void ResetValue(object component)
        {
            _d.ResetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            string s = value as string;
            if (!is_valid_id(s))
                throw new Exception("Invalid identifer");
            _d.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return _d.ShouldSerializeValue(component);
        }

        public override bool IsBrowsable
        {
            get
            {
                return true;
            }
        }

        public override AttributeCollection Attributes
        {
            get
            {
                return _ac; //base.Attributes;
            }
        }
    }


	/// This service relays requests for filtering a component's exposed
	/// attributes, properties, and events to that component's designer.
	public class SampleTypeDescriptorFilterService : ITypeDescriptorFilterService
	{
		public IDesignerHost host;

		public SampleTypeDescriptorFilterService(IDesignerHost host)
		{
			this.host = host;
		}

		/// Get the designer for the given component and cast it as a designer filter.
		private IDesignerFilter GetDesignerFilter(IComponent component)
		{
			return host.GetDesigner(component) as IDesignerFilter;
		}

		#region Implementation of ITypeDescriptorFilterService
		/// Tell the given component's designer to filter properties.
		public bool FilterProperties(System.ComponentModel.IComponent component, System.Collections.IDictionary properties)
		{
			IDesignerFilter filter = GetDesignerFilter(component);
			if (filter != null)
			{
				filter.PreFilterProperties(properties);
				filter.PostFilterProperties(properties);
                PropertyDescriptor pd = properties["Name"] as PropertyDescriptor;
                if (pd != null)
                {
                    properties["Name"] = new SYNamePropertyDescriptor(pd);
                }
                return true;
			}
			return false;
		}

		/// Tell the given component's designer to filter attributes.
		public bool FilterAttributes(System.ComponentModel.IComponent component, System.Collections.IDictionary attributes)
		{
			IDesignerFilter filter = GetDesignerFilter(component);
			if (filter != null)
			{
				filter.PreFilterAttributes(attributes);
				filter.PostFilterAttributes(attributes);
				return true;
			}
			return false;
		}

		/// Tell the given component's designer to filter events.
		public bool FilterEvents(System.ComponentModel.IComponent component, System.Collections.IDictionary events)
		{
			IDesignerFilter filter = GetDesignerFilter(component);
			if (filter != null)
			{
				filter.PreFilterEvents(events);
				filter.PostFilterEvents(events);
				return true;
			}
			return false;
		}
		#endregion
	}
}
