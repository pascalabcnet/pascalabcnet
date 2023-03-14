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
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Diagnostics;

namespace SampleDesignerHost
{
	///     Our implementation of IDesignerSerializationManager. This is a private
	///     implementation and we only have one.  So, it could have been implemented
	///     as a private interface implementation on top of the loader class
	///     itself.  I decided against this because there is a lot of
	///     state associated with the serialization manager and it could get
	///     confusing.
	internal class SampleDesignerSerializationManager : IDesignerSerializationManager 
	{
		private SampleDesignerLoader            _loader;
		private ResolveNameEventHandler         _resolveNameEventHandler;
		private EventHandler                    _serializationCompleteEventHandler;
		private ArrayList                       _designerSerializationProviders;
		private Hashtable                       _instancesByName;
		private Hashtable                       _namesByInstance;
		private Hashtable                       _serializers;
		private ArrayList                       _errorList;
		private ContextStack                    _contextStack;
		private PropertyDescriptorCollection    _propertyCollection;

		///     Initializes the serialization manager.
		internal SampleDesignerSerializationManager(SampleDesignerLoader loader) 
		{
			_loader = loader;
		}

		///     This method is called by dependent loads to add additional
		///     errors to the error list.  It is not called for the last
		///     dependent load, nor is it called if IDesignerLoaderService
		///     is not implemented.  If outside parties want to implement
		///     IDesignerLoaderService, they may, but they will have
		///     to provide their own storage for the dependent
		///     error list.  We are just re-using _errorList here to be
		///     efficient.
		internal void AddErrors(ICollection errors) 
		{
			if (errors != null && errors.Count > 0) 
			{
				if (_errorList == null) 
				{
					_errorList = new ArrayList();
				}
				_errorList.AddRange(errors);
			}
		}

		///     This starts the loading process.  Normally, everything 
		///     should be cleared out when this is called.
		internal void Initialize() 
		{
			if (_errorList != null) 
			{
				_errorList.Clear();
			}
		}

		///     This ends the loading process.  This resets the state
		///     of the serialization manager and merges the provided
		///     error collection into the manager's own error list and
		///     returns the merged list.
		internal ICollection Terminate(ICollection errors) 
		{

			// Let interested parties know that we're finished.
			//
			try 
			{
				if (_serializationCompleteEventHandler != null) 
				{
					_serializationCompleteEventHandler(this, EventArgs.Empty);
				}
			}
			catch {}

			// Merge the error list
			//
			if (_errorList != null && _errorList.Count > 0) 
			{
				if (errors != null && errors.Count > 0) 
				{
					_errorList.AddRange(errors);
				}
				errors = _errorList;
			}

			// Now disolve our state.  The serialization manager
			// should remain stateless.
			//
			_resolveNameEventHandler = null;
			_serializationCompleteEventHandler = null;
			_instancesByName = null;
			_namesByInstance = null;
			_serializers = null;
			_errorList = null;
			_contextStack = null;

			return errors;
		}

		///     The Context property provides a user-defined storage area
		///     implemented as a stack.  This storage area is a useful way
		///     to provide communication across serializers, as serialization
		///     is a generally hierarchial process.
		ContextStack IDesignerSerializationManager.Context 
		{
			get 
			{
				if (_contextStack == null) 
				{
					_contextStack = new ContextStack();
				}
				return _contextStack;
			}
		}
    
		///     The Properties property provides a set of custom properties
		///     the serialization manager may surface.  The set of properties
		///     exposed here is defined by the implementor of 
		///     IDesignerSerializationManager.  
		PropertyDescriptorCollection IDesignerSerializationManager.Properties 
		{
			get 
			{
				if (_propertyCollection == null) 
				{
					_propertyCollection = new PropertyDescriptorCollection(new PropertyDescriptor[] {});
				}
				return _propertyCollection;
			}
		}
    
		///     ResolveName event.  This event
		///     is raised when GetName is called, but the name is not found
		///     in the serialization manager's name table.  It provides a 
		///     way for a serializer to demand-create an object so the serializer
		///     does not have to order object creation by dependency.  This
		///     delegate is cleared immediately after serialization or deserialization
		///     is complete.
		event ResolveNameEventHandler IDesignerSerializationManager.ResolveName 
		{
			add 
			{
				_resolveNameEventHandler += value;
			}
			remove 
			{
				_resolveNameEventHandler -= value;
			}
		}

		///     This event is raised when serialization or deserialization
		///     has been completed.  Generally, serialization code should
		///     be written to be stateless.  Should some sort of state
		///     be necessary to maintain, a serializer can listen to
		///     this event to know when that state should be cleared.
		///     An example of this is if a serializer needs to write
		///     to another file, such as a resource file.  In this case
		///     it would be inefficient to design the serializer
		///     to close the file when finished because serialization of
		///     an object graph generally requires several _serializers.
		///     The resource file would be opened and closed many times.
		///     Instead, the resource file could be accessed through
		///     an object that listened to the SerializationComplete
		///     event, and that object could close the resource file
		///     at the end of serialization.
		event EventHandler IDesignerSerializationManager.SerializationComplete 
		{
			add 
			{
				_serializationCompleteEventHandler += value;
			}
			remove 
			{
				_serializationCompleteEventHandler -= value;
			}
		}
    
		///     This method adds a custom serialization provider to the 
		///     serialization manager.  A custom serialization provider will
		///     get the opportunity to return a serializer for a data type
		///     before the serialization manager looks in the type's
		///     metadata.  
		void IDesignerSerializationManager.AddSerializationProvider(IDesignerSerializationProvider provider) 
		{
			if (_designerSerializationProviders == null) 
			{
				_designerSerializationProviders = new ArrayList();
			}
			_designerSerializationProviders.Add(provider);
		}
         
		///     Creates an instance of the given type and adds it to a collection
		///     of named instances.  Objects that implement IComponent will be
		///     added to the design time container if addToContainer is true.
		object IDesignerSerializationManager.CreateInstance(Type type, ICollection arguments, string name, bool addToContainer) 
		{
			object[] argArray = null;
        
			if (arguments != null && arguments.Count > 0) 
			{
				argArray = new object[arguments.Count];
				arguments.CopyTo(argArray, 0);
			}
        
			object instance = null;
        
			// We do some special casing here.  If we are adding to the container, and if this type 
			// is an IComponent, then we don't create the instance through Activator, we go
			// through the loader host.  The reason for this is that if we went through activator,
			// and if the object already specified a constructor that took an IContainer, our
			// deserialization mechanism would equate the container to the designer host.  This
			// is the correct thing to do, but it has the side effect of adding the component
			// to the designer host twice -- once with a default name, and a second time with
			// the name we provide.  This equates to a component rename, which isn't cheap, 
			// so we don't want to do it when we load each and every component.
			//
			if (addToContainer && typeof(IComponent).IsAssignableFrom(type)) 
			{
				IDesignerLoaderHost host = _loader.LoaderHost;
				if (host != null) 
				{
					if (name == null) 
					{
						instance = host.CreateComponent(type);
					}
					else 
					{
						instance = host.CreateComponent(type, name);
					}
				}
			}
        
			if (instance == null) 
			{
        
				// If we have a name but the object wasn't a component
				// that was added to the design container, save the
				// name/value relationship in our own nametable.
				//
				if (name != null && _instancesByName != null) 
				{
					if (_instancesByName.ContainsKey(name)) 
					{
						Exception ex = new InvalidOperationException("Duplicate component declaration for " + name + ".");
						throw ex;
					}
				}
        
				try 
				{
					instance = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, argArray, null);
				}
				catch(MissingMethodException) 
				{
					StringBuilder argTypes = new StringBuilder();
					foreach (object o in argArray) 
					{
						if (argTypes.Length > 0) 
						{
							argTypes.Append(", ");
						}

						if (o != null) 
						{
							argTypes.Append(o.GetType().Name);
						}
						else 
						{
							argTypes.Append("null");
						}
                    
					}

					Exception ex = new InvalidOperationException("No matching constructor for " + type.FullName + "(" + argTypes.ToString() + ")");        
					throw ex;
				}                
                
				// If we have a name but the object wasn't a component
				// that was added to the design container, save the
				// name/value relationship in our own nametable.
				//
				if (name != null) 
				{
					if (_instancesByName == null) 
					{
						_instancesByName = new Hashtable();
						_namesByInstance = new Hashtable();
					}
                
					_instancesByName[name] = instance;
					_namesByInstance[instance] = name;
				}
			}
        
			return instance;
		}

		///     Retrieves an instance of a created object of the given name, or
		///     null if that object does not exist.
		object IDesignerSerializationManager.GetInstance(string name) 
		{
			object instance = null;
        
			if (name == null) 
			{
				throw new ArgumentNullException("name");
			}
        
			// Check our local nametable first
			//
			if (_instancesByName != null) 
			{
				instance = _instancesByName[name];
			}
        
			if (instance == null && _loader.LoaderHost != null) 
			{
				instance = _loader.LoaderHost.Container.Components[name];
			}
        
			if (instance == null && _resolveNameEventHandler != null) 
			{
				ResolveNameEventArgs e = new ResolveNameEventArgs(name);
				_resolveNameEventHandler(this, e);
				instance = e.Value;
			}
        
			return instance;
		}

		///     Retrieves a name for the specified object, or null if the object
		///     has no name.
		string IDesignerSerializationManager.GetName(object value) 
		{
			string name = null;
    
			if (value == null) 
			{
				throw new ArgumentNullException("value");
			}
        
			// Check our local nametable first
			//
			if (_namesByInstance != null) 
			{
				name = (string)_namesByInstance[value];
			}
        
			if (name == null && value is IComponent) 
			{
				ISite site = ((IComponent)value).Site;
				if (site != null) 
				{
					name = site.Name;
				}
			}
			return name;
		}

		///     Retrieves a serializer of the requested type for the given
		///     object type.
		object IDesignerSerializationManager.GetSerializer(Type objectType, Type serializerType) 
		{
			object serializer = null;
        
			if (objectType != null) 
			{
        
				if (_serializers != null) 
				{
					// I don't double hash here.  It will be a very rare day where
					// multiple types of serializers will be used in the same scheme.
					// We still handle it, but we just don't cache.
					//
					serializer = _serializers[objectType];
					if (serializer != null && !serializerType.IsAssignableFrom(serializer.GetType())) 
					{
						serializer = null;
					}
				}
            
				// Now actually look in the type's metadata.
				//
				IDesignerLoaderHost host = _loader.LoaderHost;
				if (serializer == null && host != null) 
				{
					AttributeCollection attributes = TypeDescriptor.GetAttributes(objectType);
					foreach(Attribute attr in attributes) 
					{
						if (attr is DesignerSerializerAttribute) 
						{
							DesignerSerializerAttribute da = (DesignerSerializerAttribute)attr;
							string typeName = da.SerializerBaseTypeName;
                        
							// This serializer must support a CodeDomSerializer or we're not interested.
							//
							if (typeName != null && host.GetType(typeName) == serializerType && da.SerializerTypeName != null && da.SerializerTypeName.Length > 0) 
							{
								Type type = host.GetType(da.SerializerTypeName);
								Debug.Assert(type != null, "Type " + objectType.FullName + " has a serializer that we couldn't bind to: " + da.SerializerTypeName);
								if (type != null) 
								{
									serializer = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
									break;
								}
							}
						}
					}
            
					// And stash this little guy for later.
					//
					if (serializer != null) 
					{
						if (_serializers == null) 
						{
							_serializers = new Hashtable();
						}
						_serializers[objectType] = serializer;
					}
				}
			}
        
			// Designer serialization providers can override our metadata discovery.
			// We loop until we reach steady state.  This breaks order dependencies
			// by allowing all providers a chance to party on each other's serializers.
			//
			if (_designerSerializationProviders != null) 
			{
				bool continueLoop = true;
				for(int i = 0; continueLoop && i < _designerSerializationProviders.Count; i++) 
				{
            
					continueLoop = false;
                
					foreach(IDesignerSerializationProvider provider in _designerSerializationProviders) 
					{
						object newSerializer = provider.GetSerializer(this, serializer, objectType, serializerType);
						if (newSerializer != null) 
						{
							continueLoop = (serializer != newSerializer);
							serializer = newSerializer;
						}
					}
				}
			}
        
			return serializer;
		}

		///     Retrieves a type of the given name.
		Type IDesignerSerializationManager.GetType(string typeName) 
		{
			Type t = null;

			if (_loader.LoaderHost != null) 
			{
				while (t == null) 
				{
					t = _loader.LoaderHost.GetType(typeName);

					if (t == null && typeName != null && typeName.Length > 0) 
					{
						int dotIndex = typeName.LastIndexOf('.');
						if (dotIndex == -1 || dotIndex == typeName.Length - 1)
							break;

						typeName = typeName.Substring(0, dotIndex) + "+" + typeName.Substring(dotIndex + 1, typeName.Length - dotIndex - 1);
					}
				}
			}
        
			return t;
		}

		///     Removes a previously added serialization provider.
		void IDesignerSerializationManager.RemoveSerializationProvider(IDesignerSerializationProvider provider) 
		{
			if (_designerSerializationProviders != null) 
			{
				_designerSerializationProviders.Remove(provider);
			}
		}
    
		///     Reports a non-fatal error in serialization.  The serialization
		///     manager may implement a logging scheme to alert the caller
		///     to all non-fatal errors at once.  If it doesn't, it should
		///     immediately throw in this method, which should abort
		///     serialization.  
		///     Serialization may continue after calling this function.
		void IDesignerSerializationManager.ReportError(object errorInformation) 
		{
			if (errorInformation != null) 
			{
				if (_errorList == null) 
				{
					_errorList = new ArrayList();
				}

				_errorList.Add(errorInformation);
			}
		}
    
		///     Provides a way to set the name of an existing object.
		///     This is useful when it is necessary to create an 
		///     instance of an object without going through CreateInstance.
		///     An exception will be thrown if you try to rename an existing
		///     object or if you try to give a new object a name that
		///     is already taken.
		void IDesignerSerializationManager.SetName(object instance, string name) 
		{
    
			if (instance == null) 
			{
				throw new ArgumentNullException("instance");
			}
        
			if (name == null) 
			{
				throw new ArgumentNullException("name");
			}
        
			if (_instancesByName == null) 
			{
				_instancesByName = new Hashtable();
				_namesByInstance = new Hashtable();
			}
        
			if (_instancesByName[name] != null) 
			{
				Exception ex = new ArgumentException("Designer Loader name " + name + " in use.");
				throw ex;
			}
        
			if (_namesByInstance[instance] != null) 
			{
				Exception ex = new ArgumentException("Designer loader object has name " + name + ".");
				throw ex;
			}
        
			_instancesByName[name] = instance;
			_namesByInstance[instance] = name;
		}

		///     Retrieves the requested service.
		object IServiceProvider.GetService(Type serviceType) 
		{
			return _loader.GetService(serviceType);
		}
	}
}
