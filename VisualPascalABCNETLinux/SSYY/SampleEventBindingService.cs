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
	using System.Design;
	using System.Diagnostics;
	using System.Globalization;
	using System.Text;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class EventDescription
    {
        public string EventName;
        public EventDescriptor e;

        public int line_num = 0;
        public int column_num = 0;

        public VisualPascalABC.CodeFileDocumentTextEditorControl editor = null;

        public EventDescription(string event_name, EventDescriptor descriptor)
        {
            EventName = event_name;
            e = descriptor;
        }
    }

	///     This class provides a default implementation of the event
	///     binding service.
	public class SampleEventBindingService : IEventBindingService 
	{
		private Hashtable _eventProperties;
		private IServiceProvider _provider;

		///     You must provide a service provider to the binding
		///     service. We give it our host.
		public SampleEventBindingService(IServiceProvider provider) 
		{
			if (provider == null) 
			{
				throw new ArgumentNullException("provider");
			}
			_provider = provider;
		}

		///     Creates a unique method name.  The name must be
		///     compatible with the script language being used and
		///     it must not conflict with any other name in the user's
		///     code. Since we have no code editor, we can guarantee this
		///     method name will be unique. However, if code were editable,
		///     we would have to check our codeCompileUnit's methods.
		protected string CreateUniqueMethodName(IComponent component, EventDescriptor e)
		{
			string name = component.ToString().Split(new char[] { ' ' })[0];
			//return "handler_" + name + "_" + e.Name;
            return name + "_" + e.Name;
        }

		///     This provides a notification that a particular method
		///     is no longer being used by an event handler.  Some implementations
		///     may want to remove the event hander when no events are using
		///     it.  By overriding UseMethod and FreeMethod, an implementation
		///     can know when a method is no longer needed.
		protected virtual void FreeMethod(object component, EventDescriptor e, string methodName) 
		{
			// UNIMPLEMENTED - We don't add method signatures for our handlers, so
			// don't need to worry about removing them.
		}

		///     Returns a collection of strings.  Each string is
		///     the method name of a method whose signature is
		///     compatible with the delegate contained in the
		///     event descriptor.  This should return an empty
		///     collection if no names are compatible.
		protected ICollection GetCompatibleMethods(EventDescriptor e)
		{
			// EMPTY IMPLEMENTATION
			return new string[] {};
		}

		///     Generates a key based on a method name and it's parameters by just concatenating the
		///     parameters.
		private string GetEventDescriptorHashCode(EventDescriptor eventDesc) 
		{
			StringBuilder builder = new StringBuilder(eventDesc.Name);
			builder.Append(eventDesc.EventType.GetHashCode().ToString());
            
			foreach(Attribute a in eventDesc.Attributes) 
			{
				builder.Append(a.GetHashCode().ToString());
			}
            
			return builder.ToString();
		}

		///     Gets the requested service from our service provider (the host).	
		protected object GetService(Type serviceType) 
		{
			if (_provider != null) 
			{
				return _provider.GetService(serviceType);
			}
			return null;
		}

		///     Shows the user code.  This method does not show any
		///     particular code; generally it shows the last code the
		///     user typed.  This returns true if it was possible to 
		///     show the code, or false if not. We are never showing code
		///     since we do not generate handler methods.
		protected bool ShowCode()
        {
            return false;
        }
        
		///     Shows the user code at the given line number.  Line
		///     numbers are one-based.  This returns true if it was
		///     possible to show the code, or false if not. We are 
		///     never showing code since we do not generate handler methods.
		protected bool ShowCode(int lineNumber)
        {
            return false;
        }
        
		///     Shows the body of the user code with the given method
		///     name. This returns true if it was possible to show
		///     the code, or false if not. We are never showing code
		///     since we do not generate handler methods.
		protected bool ShowCode(object component, EventDescriptor e, string methodName)
        {
            EventDescription ev = new EventDescription(methodName, e);
            VisualPascalABC.CodeFileDocumentControl cfdc = VisualPascalABC.Form1.Form1_object._currentCodeFileDocument;
            cfdc.GenerateDesignerCode(ev);
            if (ev.editor != null)
            {
                cfdc.DesignerAndCodeTabs.SelectedTab = cfdc.TextPage;
                VisualPascalABC.VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.ExecuteSourceLocationAction(
                    new PascalABCCompiler.SourceLocation(cfdc.FileName, ev.line_num, ev.column_num, ev.line_num, ev.column_num),
                    VisualPascalABCPlugins.SourceLocationAction.GotoBeg);
            }
            return false;
        }

        protected void ShowMessageBadCode()
        {
            VisualPascalABC.Form1.Form1_object._currentCodeFileDocument.DesignerAndCodeTabs.SelectedTab =
                VisualPascalABC.Form1.Form1_object._currentCodeFileDocument.TextPage;
            MessageBox.Show(PascalABCCompiler.StringResources.Get("VP_MF_CAN_NOT_NAVIGATE_TO_EVENT_HANDLER"),
                PascalABCCompiler.StringResources.Get("VP_MF_FORM_DESIGNER"),
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

		///     This provides a notification that a particular method
		///     is being used by an event handler.  Some implementations
		///     may want to remove the event hander when no events are using
		///     it.  By overriding UseMethod and FreeMethod, an implementation
		///     can know when a method is no longer needed.
		protected virtual void UseMethod(object component, EventDescriptor e, string methodName) 
		{
			// UNIMPLEMENTED - We do not add method signatures to our code.
		}

		///     This validates that the provided method name is valid for
		///     the language / script being used.  The default does nothing.
		///     You may override this and throw an exception if the name
		///     is invalid for your use.
		protected virtual void ValidateMethodName(string methodName) 
		{
			// UNIMPLEMENTED - We are guaranteed our method names are valid.
		}

		///     This creates a name for an event handling method for the given component
		///     and event.  The name that is created is guaranteed to be unique in the user's source
		///     code.
		string IEventBindingService.CreateUniqueMethodName(IComponent component, EventDescriptor e) 
		{
			if (component == null) 
			{
				throw new ArgumentNullException("component");
			}

			if (e == null) 
			{
				throw new ArgumentNullException("e");
			}

			return CreateUniqueMethodName(component, e);
		}
        
		///     Retrieves a collection of strings.  Each string is the name of a method
		///     in user code that has a signature that is compatible with the given event.
		ICollection IEventBindingService.GetCompatibleMethods(EventDescriptor e) 
		{
			if (e == null) 
			{
				throw new ArgumentNullException("e");
			}

			return GetCompatibleMethods(e);
		}

		///     For properties that are representing events, this will return the event
		///     that the property represents.
		EventDescriptor IEventBindingService.GetEvent(PropertyDescriptor property) 
		{
			if (property is EventPropertyDescriptor) 
			{
				return ((EventPropertyDescriptor)property).Event;
			}
			else
			{
				return null;
			}
		}
        
		///     Converts a set of events to a set of properties.
		PropertyDescriptorCollection IEventBindingService.GetEventProperties(EventDescriptorCollection events) 
		{

			if (events == null) 
			{
				throw new ArgumentNullException("events");
			}

			PropertyDescriptor[] props = new PropertyDescriptor[events.Count];

			// We cache the property descriptors here for speed.  Create those for
			// events that we don't have yet.
			//
			if (_eventProperties == null) 
			{
				_eventProperties = new Hashtable();
			}
            
			for (int i = 0; i < events.Count; i++) 
			{
                
				object eventHashCode = GetEventDescriptorHashCode(events[i]);

				props[i] = (PropertyDescriptor)_eventProperties[eventHashCode];

				if (props[i] == null) 
				{
					props[i] = new EventPropertyDescriptor(events[i], this);
					_eventProperties[eventHashCode] = props[i];
				}
			}

			return new PropertyDescriptorCollection(props);
		}

		///     Converts a single event to a property.
		PropertyDescriptor IEventBindingService.GetEventProperty(EventDescriptor e) 
		{

			if (e == null) 
			{
				throw new ArgumentNullException("e");
			}

			if (_eventProperties == null) 
			{
				_eventProperties = new Hashtable();
			}

			object eventHashCode = GetEventDescriptorHashCode(e);

			PropertyDescriptor prop = (PropertyDescriptor)_eventProperties[eventHashCode];

			if (prop == null) 
			{
				prop = new EventPropertyDescriptor(e, this);
				_eventProperties[eventHashCode] = prop;
			}

			return prop;
		}
        
		///     Displays the user code for this designer.  This will return true if the user
		///     code could be displayed, or false otherwise.
		bool IEventBindingService.ShowCode() 
		{
			return ShowCode();
		}
        
		///     Displays the user code for the designer.  This will return true if the user
		///     code could be displayed, or false otherwise.
		bool IEventBindingService.ShowCode(int lineNumber) 
		{
			return ShowCode(lineNumber);
		}
        
		///     Displays the user code for the given event.  This will return true if the user
		///     code could be displayed, or false otherwise.
		bool IEventBindingService.ShowCode(IComponent component, EventDescriptor e) 
		{
			if (component == null) 
			{
				throw new ArgumentNullException("component");
			}

			if (e == null) 
			{
				throw new ArgumentNullException("e");
			}

			PropertyDescriptor prop = ((IEventBindingService)this).GetEventProperty(e);
            
			string methodName = (string)prop.GetValue(component);
			if (methodName == null) 
			{
				return false;   // the event is not bound to a method.
			}
            
			return ShowCode(component, e, methodName);
		}
		
		///     This is an EventDescriptor cleverly wrapped in a PropertyDescriptor
		///     of type String.  Note that we now handle subobjects by storing their
		///     event information in their base component's site's dictionary.
		///     Note also that when a value is set for this property we will code-gen
		///     the event method.  If the property is set to a new value we will
		///     remove the old event method ONLY if it is empty.
		private class EventPropertyDescriptor : PropertyDescriptor 
		{
			private EventDescriptor     _eventDesc;
			private SampleEventBindingService _eventSvc;
			private TypeConverter       _converter;

			///     Creates a new EventPropertyDescriptor.
			internal EventPropertyDescriptor(EventDescriptor eventDesc, SampleEventBindingService eventSvc) : base(eventDesc, null) 
			{
				_eventDesc = eventDesc;
				_eventSvc = eventSvc;
			}

			///     Indicates whether reset will change the value of the component.  If there
			///     is a DefaultValueAttribute, then this will return true if getValue returns
			///     something different than the default value.  If there is a reset method and
			///     a shouldPersist method, this will return what shouldPersist returns.
			///     If there is just a reset method, this always returns true.  If none of these
			///     cases apply, this returns false.
			public override bool CanResetValue(object component) 
			{
				return GetValue(component) != null;
			}

			///     Retrieves the type of the component this PropertyDescriptor is bound to.
			public override Type ComponentType 
			{
				get 
				{
					return _eventDesc.ComponentType;
				}
			}

			///      Retrieves the type converter for this property.
			public override TypeConverter Converter 
			{
				get 
				{
					if (_converter == null) 
					{
						_converter = new EventConverter(_eventDesc);
					}
                    
					return _converter;
				}
			}
            
			///     Retrieves the event descriptor we are representing.
			internal EventDescriptor Event 
			{
				get 
				{
					return _eventDesc;
				}
			}
			
			///     Indicates whether this property is read only.
			public override bool IsReadOnly 
			{
				get 
				{
					return Attributes[typeof(ReadOnlyAttribute)].Equals(ReadOnlyAttribute.Yes);
				}
			}

			///     Retrieves the type of the property.
			public override Type PropertyType 
			{
				get 
				{
					return _eventDesc.EventType;
				}
			}

			///     Retrieves the current value of the property on component,
			///     invoking the getXXX method.  An exception in the getXXX
			///     method will pass through.
			public override object GetValue(object component) 
			{

				if (component == null) 
				{
					throw new ArgumentNullException("component");
				}

				// We must locate the sited component, because we store data on the dictionary
				// service for the component.
				//
				ISite site = null;
                
				if (component is IComponent) 
				{
					site = ((IComponent)component).Site;
				}

				if (site == null) 
				{
					IReferenceService rs = _eventSvc._provider.GetService(typeof(IReferenceService)) as IReferenceService;
					if (rs != null) 
					{
						IComponent baseComponent = rs.GetComponent(component);
						if (baseComponent != null) 
						{
							site = baseComponent.Site;
						}
					}
				}

				if (site == null) 
				{
					Exception ex = new InvalidOperationException("There is no site for component" + component.ToString() + ".");
					throw ex;
				}

				IDictionaryService ds = (IDictionaryService)site.GetService(typeof(IDictionaryService));
				if (ds == null) 
				{
					Exception ex = new InvalidOperationException("Cannot find IDictionaryService.");
					throw ex;
				}

				return (string)ds.GetValue(new ReferenceEventClosure(component, this));
			}

			///     Will reset the default value for this property on the component.  If
			///     there was a default value passed in as a DefaultValueAttribute, that
			///     value will be set as the value of the property on the component.  If
			///     there was no default value passed in, a ResetXXX method will be looked
			///     for.  If one is found, it will be invoked.  If one is not found, this
			///     is a nop.
			public override void ResetValue(object component) 
			{
				SetValue(component, null);
			}

			///     This will set value to be the new value of this property on the
			///     component by invoking the setXXX method on the component.  If the
			///     value specified is invalid, the component should throw an exception
			///     which will be passed up.  The component designer should design the
			///     property so that getXXX following a setXXX should return the value
			///     passed in if no exception was thrown in the setXXX call.
			public override void SetValue(object component, object value) 
			{
				// Argument, state checking.  Is it ok to set this event?
				//
				if (IsReadOnly) 
				{
					Exception ex = new InvalidOperationException("Tried to set a read only event.");
					throw ex;
				}
                
				if (value != null && !(value is string)) 
				{
					Exception ex = new ArgumentException("Cannot set to value " + value.ToString() + ".");
					throw ex;
				}

				string name = (string)value;
				if (name != null && name.Length == 0) 
				{
					name = null; 
				}

				// Obtain the site for the component.  Note that this can be a site
				// to a parent component if we can get to the reference service.
				//
				ISite site = null;
                
				if (component is IComponent) 
				{
					site = ((IComponent)component).Site;
				}

				if (site == null) 
				{
					IReferenceService rs = _eventSvc._provider.GetService(typeof(IReferenceService)) as IReferenceService;
					if (rs != null) 
					{
						IComponent baseComponent = rs.GetComponent(component);
						if (baseComponent != null) 
						{
							site = baseComponent.Site;
						}
					}
				}

				if (site == null) 
				{
					Exception ex = new InvalidOperationException("There is no site for component " + component.ToString() + ".");
					throw ex;
				}

				// The dictionary service is where we store the actual event method name.
				//
				IDictionaryService ds = (IDictionaryService)site.GetService(typeof(IDictionaryService));
				if (ds == null) 
				{
					Exception ex = new InvalidOperationException("Cannot find IDictionaryService");
					throw ex;
				}

				// Get the old method name, ensure that they are different, and then continue.
				//
				ReferenceEventClosure key = new ReferenceEventClosure(component, this);
				string oldName = (string)ds.GetValue(key);

				if (object.ReferenceEquals(oldName, name)) 
				{
					return;
				}
                
				if (oldName != null && name != null && oldName.Equals(name)) 
				{
					return;
				}

				// Before we continue our work, ensure that the name is
				// actually valid.
				//
				if (name != null) 
				{
					_eventSvc.ValidateMethodName(name);
				}
                
				// Ok, the names are different.  Fire a changing event to make
				// sure it's OK to perform the change.
				//
				IComponentChangeService change = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
				if (change != null) 
				{
					try
					{
						change.OnComponentChanging(component, this);
					}
					catch(CheckoutException coEx)
					{
						if (coEx == CheckoutException.Canceled)
						{
							return;
						}
						throw;
					}
				}

				// Less chance of success of adding a new method name, so
				// don't release the old name until we verify that adding
				// the new one actually succeeded.
				//
				if (name != null) 
				{
					_eventSvc.UseMethod(component, _eventDesc, name);
				}

				if (oldName != null) 
				{
					_eventSvc.FreeMethod(component, _eventDesc, oldName);
				}
                
				ds.SetValue(key, name);

				if (change != null) 
				{
					change.OnComponentChanged(component, this, oldName, name);
				}

				OnValueChanged(component, EventArgs.Empty);
			}

			///     Indicates whether the value of this property needs to be persisted. In
			///     other words, it indicates whether the state of the property is distinct
			///     from when the component is first instantiated. If there is a default
			///     value specified in this PropertyDescriptor, it will be compared against the
			///     property's current value to determine this.  If there is't, the
			///     shouldPersistXXX method is looked for and invoked if found.  If both
			///     these routes fail, true will be returned.
			///
			///     If this returns false, a tool should not persist this property's value.
			public override bool ShouldSerializeValue(object component) 
			{
				return CanResetValue(component);
			}
			
			///     Implements a type converter for event objects.
			private class EventConverter : TypeConverter 
			{

				private EventDescriptor _evt;
                
				
				///     Creates a new EventConverter.
				
				internal EventConverter(EventDescriptor evt) 
				{
					_evt = evt;
				}
                
				
				///     Determines if this converter can convert an object in the given source
				///     type to the native type of the converter.
				public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) 
				{
					if (sourceType == typeof(string)) 
					{
						return true;
					}
					return base.CanConvertFrom(context, sourceType);
				}
        
				///     Determines if this converter can convert an object to the given destination
				///     type.
				public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) 
				{
					if (destinationType == typeof(string)) 
					{
						return true;
					}
					return base.CanConvertTo(context, destinationType);
				}
        
				///     Converts the given object to the converter's native type.
				public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) 
				{
					if (value == null) 
					{
						return value;
					}
					if (value is string) 
					{
						if (((string)value).Length == 0) 
						{
							return null;
						}
						return value;
					}
					return base.ConvertFrom(context, culture, value);
				}
        
				///     Converts the given object to another type.  The most common types to convert
				///     are to and from a string object.  The default implementation will make a call
				///     to ToString on the object if the object is valid and if the destination
				///     type is string.  If this cannot convert to the desitnation type, this will
				///     throw a NotSupportedException.
				public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) 
				{
					if (destinationType == typeof(string)) 
					{
						return value == null ? string.Empty : value;
					}
					return base.ConvertTo(context, culture, value, destinationType);
				}
				
				///     Retrieves a collection containing a set of standard values
				///     for the data type this validator is designed for.  This
				///     will return null if the data type does not support a
				///     standard set of values.
				public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
				{
					// We cannot cache this because it depends on the contents of the source file.
					//
					string[] eventMethods = null;
    
					if (context != null) 
					{
						IEventBindingService ebs = (IEventBindingService)context.GetService(typeof(IEventBindingService));
						if (ebs != null) 
						{
							ICollection methods = ebs.GetCompatibleMethods(_evt);
							eventMethods = new string[methods.Count];
							int i = 0;
							foreach(string s in methods) 
							{
								eventMethods[i++] = s;
							}
						}
					}
                    
					return new StandardValuesCollection(eventMethods);
				}
        
				///     Determines if the list of standard values returned from
				///     GetStandardValues is an exclusive list.  If the list
				///     is exclusive, then no other values are valid, such as
				///     in an enum data type.  If the list is not exclusive,
				///     then there are other valid values besides the list of
				///     standard values GetStandardValues provides.
				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) 
				{
					return false;
				}
				
				///     Determines if this object supports a standard set of values
				///     that can be picked from a list.
				public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
				{
					return true;
				}
			}
        
			///     This is a combination of a reference and a property, so that it can be used
			///     as the key of a hashtable.  This is because we may have subobjects that share
			///     the same property.
			private class ReferenceEventClosure 
			{
				object reference;
				EventPropertyDescriptor propertyDescriptor;

				public ReferenceEventClosure(object reference, EventPropertyDescriptor prop) 
				{
					this.reference = reference;
					this.propertyDescriptor = prop;
				}

				public override int GetHashCode() 
				{
					return reference.GetHashCode() * propertyDescriptor.GetHashCode();
				}

				public override bool Equals(Object otherClosure) 
				{
					if (otherClosure is ReferenceEventClosure) 
					{
						ReferenceEventClosure typedClosure = (ReferenceEventClosure) otherClosure;
						return(typedClosure.reference == reference &&
							typedClosure.propertyDescriptor == propertyDescriptor);
					}
					return false;
				}
			}
		}
	}
}



