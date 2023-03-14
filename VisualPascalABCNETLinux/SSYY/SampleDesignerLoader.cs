///------------------------------------------------------------------------------
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
	using System.Collections;
	using System.Diagnostics;
	using System.Globalization;
	using System.IO;
	using System.Reflection;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Text;
	using System.Windows.Forms;
	using System.Xml;
	using System.CodeDom.Compiler;
	using System.CodeDom;
	using Microsoft.CSharp;
	using Microsoft.VisualBasic;
    using SampleDesignerApplication;
    using System.Resources;

	/// This is a designer loader that is based on XML.  We use reflection
	/// to write out values into an XML document.  The techniques used in this
	/// designer loader to discover, via reflection, the properties and
	/// objects that need to be saved or loaded can be applied to any
	/// persistence format.
	/// 
	/// The XML format we use here is not terribly user-friendly, but
	/// is fairly straightforward.  It handles the vast majority of
	/// persistence requirements including collections, instance descriptors,
	/// and binary data.
	/// 
	/// In addition to maintaining the buffer in the form of an XmlDocument,
	/// we also maintain it in a CodeCompileUnit. We use this DOM to generate
	/// C# and VB code, as well as to compile the buffer into an executable.
    public delegate void ModifyDelegate();
    
    public class SampleDesignerLoader : DesignerLoader 
	{
        private bool _dirty;
        public bool dirty
        {
            get
            {
                return _dirty;
            }
            set
            {
                _dirty = value;
                if (value && modify != null) modify();
            }
        }

        public ModifyDelegate modify;
		private bool unsaved;
		private string fileName;
		private string executable;
		internal IDesignerLoaderHost host;
		private XmlDocument xmlDocument;
		private CodeCompileUnit codeCompileUnit;
		private Process run;

		private static readonly Attribute[] propertyAttributes = new Attribute[] 
			{
				DesignOnlyAttribute.No
			};

		/// Empty constructor simply creates a new form.
		public SampleDesignerLoader() 
		{
		}

		/// This constructor takes a file name.  This file
		/// should exist on disk and consist of XML that
		/// can be read by a data set.
		public SampleDesignerLoader(string fileName) 
		{
			if (fileName == null) 
			{
				throw new ArgumentNullException("fileName");
			}
			this.fileName = fileName;
		}

		// Called by the host when we load a document.
		public override void BeginLoad(IDesignerLoaderHost host) 
		{
			if (host == null) 
			{
				throw new ArgumentNullException("SampleDesignerLoader.BeginLoad: Invalid designerLoaderHost.");
			}

			this.host = host;

			// The loader will put error messages in here.
			//
			ArrayList errors = new ArrayList();
			bool successful = true;
			string baseClassName;
			
			// The loader is responsible for providing certain services to the host.
			//
			host.AddService(typeof(IDesignerSerializationManager), new SampleDesignerSerializationManager(this));
			host.AddService(typeof(IEventBindingService), new SampleEventBindingService(host));
			host.AddService(typeof(ITypeResolutionService), new SampleTypeResolutionService());
			host.AddService(typeof(CodeDomProvider), new CSharpCodeProvider());
			host.AddService(typeof(IResourceService), new SampleResourceService(host));

			// If no filename was passed in, just create a form and be done with it.  If a file name
			// was passed, read it.
			//
			if (fileName == null) 
			{
				baseClassName = host.CreateComponent(typeof(System.Windows.Forms.Form)).Site.Name;
			}
			else 
			{
				baseClassName = ReadFile(fileName, errors, out xmlDocument);
			}

			// Now that we are done with the load work, we need to begin to listen to events.
			// Listening to event notifications is how a designer "Loader" can also be used
			// to save data.  If we wanted to integrate this loader with source code control,
			// we would listen to the "ing" events as well as the "ed" events.
			//
			IComponentChangeService cs = host.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if (cs != null)
			{
				cs.ComponentChanged += new ComponentChangedEventHandler(OnComponentChanged);
				cs.ComponentAdded += new ComponentEventHandler(OnComponentAddedRemoved);
				cs.ComponentRemoved += new ComponentEventHandler(OnComponentAddedRemoved);
			}

			// Let the host know we are done loading.
			host.EndLoad(baseClassName, successful, errors);
			
			// We've just loaded a document, so you can bet we need to flush changes.
			dirty = true;
			unsaved = false;
		}

		public override void Dispose() 
		{
			// Always remove attached event handlers in Dispose.
			IComponentChangeService cs = host.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if (cs != null)
			{
				cs.ComponentChanged -= new ComponentChangedEventHandler(OnComponentChanged);
				cs.ComponentAdded -= new ComponentEventHandler(OnComponentAddedRemoved);
				cs.ComponentRemoved -= new ComponentEventHandler(OnComponentAddedRemoved);
			}
		}

		/// This method is called by the designer host whenever it wants the
		/// designer loader to flush any pending changes.  Flushing changes
		/// does not mean the same thing as saving to disk.  For example,
		/// In Visual Studio, flushing changes causes new code to be generated
		/// and inserted into the text editing window.  The user can edit
		/// the new code in the editing window, but nothing has been saved
		/// to disk.  This sample adheres to this separation between flushing
		/// and saving, since a flush occurs whenever the code windows are
		/// displayed or there is a build. Neither of those items demands a save.
		public override void Flush() 
		{
			// Nothing to flush if nothing has changed.
			if (!dirty) 
			{
				return;
			}

			// We use an XmlDocument to build up the XML for
			// the designer.  Here is a sample XML chunk:
			//
			XmlDocument document = new XmlDocument();

			// This element will serve as the undisputed DocumentElement (root)
			// of our document. This allows us to have objects of equal level below,
			// which we need, since component tray items are not children of the form.
			//
			document.AppendChild(document.CreateElement("DOCUMENT_ELEMENT"));

			// We start with the root component and then continue
			// to all the rest of the components.  The nametable
			// object we create keeps track of which objects we have
			// seen.  As we write out an object's contents, we save
			// it in the nametable, so we don't write out an object
			// twice.
			//
			IComponent root = host.RootComponent;
			Hashtable nametable = new Hashtable(host.Container.Components.Count);

            resources_file_name = Path.GetDirectoryName(full_file_name) + '\\' + Path.GetFileNameWithoutExtension(file_name) + '.' + root.Site.Name + ".resources";
            resources_not_empty = false;

            current_component = root;
			document.DocumentElement.AppendChild(WriteObject(document, nametable, root));

			foreach(IComponent comp in host.Container.Components)
			{
				if (comp != root && !nametable.ContainsKey(comp))
				{
                    current_component = comp; 
					document.DocumentElement.AppendChild(WriteObject(document, nametable, comp));
				}
			}

            FinishResources();

			// Along with the XML, we also represent the designer in a CodeCompileUnit,
			// which we can use to generate C# and VB, and which we can compile from.
			//
			CodeCompileUnit code = new CodeCompileUnit();

			// Our dummy namespace is the name of our main form + "Namespace". Creative, eh?
			CodeNamespace ns = new CodeNamespace(root.Site.Name + "Namespace");
			ns.Imports.Add(new CodeNamespaceImport("System"));

			// We need to look at our type resolution service to find out what references
			// to import.
			//
			SampleTypeResolutionService strs = host.GetService(typeof(ITypeResolutionService)) as SampleTypeResolutionService;
			foreach (Assembly assm in strs.RefencedAssemblies)
			{
				ns.Imports.Add(new CodeNamespaceImport(assm.GetName().Name));
			}

            //ssyy
            host.RemoveService(typeof(IDesignerSerializationManager));
            host.AddService(typeof(IDesignerSerializationManager), new SampleDesignerSerializationManager(this));

            // Autogenerates member declaration and InitializeComponent()
			// in a new CodeTypeDeclaration
			//
			RootDesignerSerializerAttribute a = TypeDescriptor.GetAttributes(root)[typeof(RootDesignerSerializerAttribute)] as RootDesignerSerializerAttribute;
			Type t = host.GetType(a.SerializerTypeName);
			CodeDomSerializer cds = Activator.CreateInstance(t) as CodeDomSerializer;
			IDesignerSerializationManager manager = host.GetService(typeof(IDesignerSerializationManager)) as IDesignerSerializationManager;
			CodeTypeDeclaration td = cds.Serialize(manager, root) as CodeTypeDeclaration;

			// We need a constructor that will call the InitializeComponent()
			// that we just generated.
			//
			CodeConstructor con = new CodeConstructor();
			con.Attributes = MemberAttributes.Public;
			con.Statements.Add(new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "InitializeComponent")));
			td.Members.Add(con);

			// Finally our Main method, where the magic begins.
			CodeEntryPointMethod main = new CodeEntryPointMethod();
			main.Name = "Main";
			main.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			main.CustomAttributes.Add(new CodeAttributeDeclaration("System.STAThreadAttribute"));
			main.Statements.Add(new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(
				new CodeTypeReferenceExpression(
				typeof(System.Windows.Forms.Application)), "Run"), 
				new CodeExpression[] {
										new CodeObjectCreateExpression(new CodeTypeReference(root.Site.Name))
										}));
			td.Members.Add(main);


			ns.Types.Add(td);
			code.Namespaces.Add(ns);

			// Now we reset our dirty bit and set the member
			// variables.
			dirty = false;
			xmlDocument = document;
			codeCompileUnit = code;

			// Now we update the code windows to show what new stuff we've learned.
			UpdateCodeWindows();
            resources_not_empty = false;
		}

        public string file_name;
        public string full_file_name;
        public string form_name;
        public string resources_file_name;
        public bool resources_not_empty = false;
        private ResourceWriter resource_writer = null;
        private IComponent current_component = null;
        private PropertyDescriptor current_property = null;

		/// This method writes out the contents of our designer in C#, VB, and XML.
		/// For the first two, it generates code from our codeCompileUnit. For the XML,
		/// it writes out the contents of xmlDocument.
		private void UpdateCodeWindows()
		{
			// The main form's TabControl was added to the host's lists of services
			// just so we could get at it here. Fortunately for us, each tab page
			// has but one Control--a textbox. 
			//
			
            CodeStrings codes = host.GetService(typeof(CodeStrings)) as CodeStrings;
            //TabControl tc = host.GetService(typeof(TabControl)) as TabControl;
            //TextBox csWindow = tc.TabPages[1].Controls[0] as TextBox;
            //TextBox vbWindow = tc.TabPages[2].Controls[0] as TextBox;
            //TextBox xmlWindow = tc.TabPages[3].Controls[0] as TextBox;
			
			// The string writer we'll generate code to.
			StringWriter sw;

			// The options for our code generation.
			CodeGeneratorOptions o = new CodeGeneratorOptions();
			o.BlankLinesBetweenMembers = false;
			o.BracingStyle = "C";
			o.ElseOnClosing = false;
			o.IndentString = "    ";

			// CSharp Code Generation
            //sw = new StringWriter();
            //CSharpCodeProvider cs = new CSharpCodeProvider();
            //cs.CreateGenerator().GenerateCodeFromCompileUnit(codeCompileUnit, sw, o);
            //csWindow.Text = sw.ToString();
            //sw.Close();

			// PABC Code Generation
			sw = new StringWriter();
            if (resources_not_empty)
            {
                sw.WriteLine("{$resource " + Path.GetFileName(resources_file_name) + '}');
                sw.Write("    ");
            }
            //VBCodeProvider vb = new VBCodeProvider();
			//vb.CreateGenerator().GenerateCodeFromCompileUnit(codeCompileUnit, sw, o);
            ICodeGenerator abc = new PABCCodeGenerator();
            //sw.Write(string.Format(string_consts.begin_unit, file_name, form_name));
            abc.GenerateCodeFromCompileUnit(codeCompileUnit, sw, o);
            //sw.Write(string_consts.end_unit);
            
			codes.PascalABCCode = sw.ToString();
			sw.Close();

			// XML Output
			sw = new StringWriter();
			XmlTextWriter xtw = new XmlTextWriter(sw);
			xtw.Formatting = Formatting.Indented;
			xmlDocument.WriteTo(xtw);

			// Get rid of our artificial super-root before we display the XML.
			//
			string cleanup = sw.ToString().Replace("<DOCUMENT_ELEMENT>", "");
			cleanup = cleanup.Replace("</DOCUMENT_ELEMENT>", "");
			//xmlWindow.Text = cleanup;
            codes.XMLCode = cleanup;
			sw.Close();
		}

		/// Simple helper method that returns true if the given type converter supports
		/// two-way conversion of the given type.
		private bool GetConversionSupported(TypeConverter converter, Type conversionType)
		{
			return (converter.CanConvertFrom(conversionType) && converter.CanConvertTo(conversionType));
		}

		// As soon as things change, we're dirty, so Flush()ing will give us a new
		// xmlDocument and codeCompileUnit.
		private void OnComponentChanged(object sender, ComponentChangedEventArgs ce) 
		{
			dirty = true;
			unsaved = true;
		}

		private void OnComponentAddedRemoved(object sender, ComponentEventArgs ce) 
		{
			dirty = true;
			unsaved = true;
		}

		/// This method prompts the user to see if it is OK to dispose this document.  
		/// The prompt only happens if the user has made changes.
		internal bool PromptDispose() 
		{
			if (dirty || unsaved)
			{
				switch(MessageBox.Show("Save changes to existing designer?", "Unsaved Changes", MessageBoxButtons.YesNoCancel))
				{
					case DialogResult.Yes:
						Save(false);
						break;
					case DialogResult.Cancel:
						return false;
				}
			}
			return true;
		}

		///  Reads an Event node and binds the event.
		private void ReadEvent(XmlNode childNode, object instance, ArrayList errors)
		{
			IEventBindingService bindings = host.GetService(typeof(IEventBindingService)) as IEventBindingService;
			if (bindings == null)
			{
				errors.Add("Unable to contact event binding service so we can't bind any events");
				return;
			}

			XmlAttribute nameAttr = childNode.Attributes["name"];
			if (nameAttr == null)
			{
				errors.Add("No event name");
				return;
			}

			XmlAttribute methodAttr = childNode.Attributes["method"];
			if (methodAttr == null || methodAttr.Value == null || methodAttr.Value.Length == 0)
			{
				errors.Add(string.Format("Event {0} has no method bound to it"));
				return;
			}

			EventDescriptor evt = TypeDescriptor.GetEvents(instance)[nameAttr.Value];
			if (evt == null)
			{
				errors.Add(string.Format("Event {0} does not exist on {1}", nameAttr.Value, instance.GetType().FullName));
				return;
			}

			PropertyDescriptor prop = bindings.GetEventProperty(evt);
			Debug.Assert(prop != null, "Bad event binding service");

			try
			{
				prop.SetValue(instance, methodAttr.Value);
			}
			catch(Exception ex)
			{
				errors.Add(ex.Message);
			}
		}

		/// This method is used to parse the given file.  Before calling this 
		/// method the host member variable must be setup.  This method will
		/// create a data set, read the data set from the XML stored in the
		/// file, and then walk through the data set and create components
		/// stored within it.  The data set is stored in the persistedData
		/// member variable upon return.
		/// 
		/// This method never throws exceptions.  It will set the successful
		/// ref parameter to false when there are catastrophic errors it can't
		/// resolve (like being unable to parse the XML).  All error exceptions
		/// are added to the errors array list, including minor errors.
		private string ReadFile(string fileName, ArrayList errors, out XmlDocument document) 
		{
			string baseClass = null;

			// Anything unexpected is a fatal error.
			//
			try 
			{
				// The main form and items in the component tray will be at the
				// same level, so we have to create a higher super-root in order
				// to construct our XmlDocument.
				//
				StreamReader sr = new StreamReader(fileName);
				string cleandown = sr.ReadToEnd();
				cleandown = "<DOCUMENT_ELEMENT>" + cleandown + "</DOCUMENT_ELEMENT>";
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(cleandown);

				// Now, walk through the document's elements.
				//
				foreach(XmlNode node in doc.DocumentElement.ChildNodes) 
				{
					if (baseClass == null)
					{
						baseClass = node.Attributes["name"].Value;
					}
					if (node.Name.Equals("Object"))
					{
						ReadObject(node, errors);
					}
					else
					{
						errors.Add(string.Format("Node type {0} is not allowed here.", node.Name));
					}
				}

				document = doc;
			}
			catch(Exception ex) 
			{
				document = null;
				errors.Add(ex);
			}

			return baseClass;
		}

		private object ReadInstanceDescriptor(XmlNode node, ArrayList errors)
		{
			// First, need to deserialize the member
			//
			XmlAttribute memberAttr = node.Attributes["member"];
			if (memberAttr == null)
			{
				errors.Add("No member attribute on instance descriptor");
				return null;
			}

			byte[] data = Convert.FromBase64String(memberAttr.Value);
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream stream = new MemoryStream(data);
			MemberInfo mi = (MemberInfo)formatter.Deserialize(stream);
			object[] args = null;

			// Check to see if this member needs arguments.  If so, gather
			// them from the XML.
			if (mi is MethodBase)
			{
				ParameterInfo[] paramInfos = ((MethodBase)mi).GetParameters();
				args = new object[paramInfos.Length];
				int idx = 0;

				foreach(XmlNode child in node.ChildNodes)
				{
					if (child.Name.Equals("Argument"))
					{
						object value;
						if (!ReadValue(child, TypeDescriptor.GetConverter(paramInfos[idx].ParameterType), errors, out value))
						{
							return null;
						}

						args[idx++] = value;
					}
				}

				if (idx != paramInfos.Length)
				{
					errors.Add(string.Format("Member {0} requires {1} arguments, not {2}.", mi.Name, args.Length, idx));
					return null;
				}
			}

			InstanceDescriptor id = new InstanceDescriptor(mi, args);
			object instance = id.Invoke();

			// Ok, we have our object.  Now, check to see if there are any properties, and if there are, 
			// set them.
			//
			foreach(XmlNode prop in node.ChildNodes)
			{
				if (prop.Name.Equals("Property"))
				{
					ReadProperty(prop, instance, errors);
				}
			}

			return instance;
		}

		/// Reads the "Object" tags. This returns an instance of the
		/// newly created object. Returns null if there was an error.
		private object ReadObject(XmlNode node, ArrayList errors)
		{
			XmlAttribute typeAttr = node.Attributes["type"];
			if (typeAttr == null)
			{
				errors.Add("<Object> tag is missing required type attribute");
				return null;
			}

			Type type = Type.GetType(typeAttr.Value);
			if (type == null)
			{
				errors.Add(string.Format("Type {0} could not be loaded.", typeAttr.Value));
				return null;
			}

			// This can be null if there is no name for the object.
			//
			XmlAttribute nameAttr = node.Attributes["name"];

			object instance;

			if (typeof(IComponent).IsAssignableFrom(type))
			{
				if (nameAttr == null)
				{
					instance = host.CreateComponent(type);
				}
				else
				{
					instance = host.CreateComponent(type, nameAttr.Value);
				}
			}
			else
			{
				instance = Activator.CreateInstance(type);
			}

			// Got an object, now we must process it.  Check to see if this tag
			// offers a child collection for us to add children to.
			//
			XmlAttribute childAttr = node.Attributes["children"];
			IList childList = null;
			if (childAttr != null)
			{
				PropertyDescriptor childProp = TypeDescriptor.GetProperties(instance)[childAttr.Value];
				if (childProp == null)
				{
					errors.Add(string.Format("The children attribute lists {0} as the child collection but this is not a property on {1}", childAttr.Value, instance.GetType().FullName));
				}
				else 
				{
					childList = childProp.GetValue(instance) as IList;
					if (childList == null)
					{
						errors.Add(string.Format("The property {0} was found but did not return a valid IList", childProp.Name));
					}
				}
			}

			// Now, walk the rest of the tags under this element.
			//
			foreach(XmlNode childNode in node.ChildNodes)
			{
				if (childNode.Name.Equals("Object"))
				{
					// Another object.  In this case, create the object, and
					// parent it to ours using the children property.  If there
					// is no children property, bail out now.
					if (childAttr == null)
					{
						errors.Add("Child object found but there is no children attribute");
						continue;
					}

					// no sense doing this if there was an error getting the property.  We've already reported the
					// error above.
					if (childList != null)
					{
						object childInstance = ReadObject(childNode, errors);
						childList.Add(childInstance);
					}
				}
				else if (childNode.Name.Equals("Property"))
				{
					// A property.  Ask the property to parse itself.
					//
					ReadProperty(childNode, instance, errors);
				}
				else if (childNode.Name.Equals("Event"))
				{
					// An event.  Ask the event to parse itself.
					//
					ReadEvent(childNode, instance, errors);
				}
			}
			return instance;
		}

		/// Parses the given XML node and sets the resulting property value.
		private void ReadProperty(XmlNode node, object instance, ArrayList errors)
		{
			XmlAttribute nameAttr = node.Attributes["name"];
			if (nameAttr == null)
			{
				errors.Add("Property has no name");
				return;
			}

			PropertyDescriptor prop = TypeDescriptor.GetProperties(instance)[nameAttr.Value];
			if (prop == null)
			{
				errors.Add(string.Format("Property {0} does not exist on {1}", nameAttr.Value, instance.GetType().FullName));
				return;
			}

			// Get the type of this property.  We have three options:
			// 1.  A normal read/write property.
			// 2.  A "Content" property.
			// 3.  A collection.
			//
			bool isContent = prop.Attributes.Contains(DesignerSerializationVisibilityAttribute.Content);

			if (isContent)
			{
				object value = prop.GetValue(instance);

				// Handle the case of a content property that is a collection.
				//
				if (value is IList)
				{
					foreach(XmlNode child in node.ChildNodes)
					{
						if (child.Name.Equals("Item"))
						{
							object item;
							XmlAttribute typeAttr = child.Attributes["type"];
							if (typeAttr == null)
							{
								errors.Add("Item has no type attribute");
								continue;
							}

							Type type = Type.GetType(typeAttr.Value);
							if (type == null)
							{
								errors.Add(string.Format("Item type {0} could not be found.", typeAttr.Value));
								continue;
							}

							if (ReadValue(child, TypeDescriptor.GetConverter(type), errors, out item))
							{
								try
								{
									((IList)value).Add(item);
								}
								catch(Exception ex)
								{
									errors.Add(ex.Message);
								}
							}
						}
						else
						{
							errors.Add(string.Format("Only Item elements are allowed in collections, not {0} elements.", child.Name));
						}
					}
				}
				else
				{
					// Handle the case of a content property that consists of child properties.
					//
					foreach(XmlNode child in node.ChildNodes)
					{
						if (child.Name.Equals("Property"))
						{
							ReadProperty(child, value, errors);
						}
						else
						{
							errors.Add(string.Format("Only Property elements are allowed in content properties, not {0} elements.", child.Name));
						}
					}
				}
			}
			else
			{
				object value;
				if (ReadValue(node, prop.Converter, errors, out value))
				{
					// ReadValue succeeded.  Fill in the property value.
					//
					try
					{
						prop.SetValue(instance, value);
					}
					catch(Exception ex)
					{
						errors.Add(ex.Message);
					}
				}
			}
		}

		/// Generic function to read an object value.  Returns true if the read
		/// succeeded.
		private bool ReadValue(XmlNode node, TypeConverter converter, ArrayList errors, out object value)
		{
			try 
			{
				foreach(XmlNode child in node.ChildNodes)
				{
					if (child.NodeType == XmlNodeType.Text) 
					{
						value = converter.ConvertFromInvariantString(node.InnerText);
						return true;
					}
					else if (child.Name.Equals("Binary"))
					{
						byte[] data = Convert.FromBase64String(child.InnerText);

						// Binary blob.  Now, check to see if the type converter
						// can convert it.  If not, use serialization.
						//
						if (GetConversionSupported(converter, typeof(byte[])))
						{
							value = converter.ConvertFrom(null, CultureInfo.InvariantCulture, data);
							return true;
						}
						else
						{
							BinaryFormatter formatter = new BinaryFormatter();
							MemoryStream stream = new MemoryStream(data);
							value = formatter.Deserialize(stream);
							return true;
						}
					}
					else if (child.Name.Equals("InstanceDescriptor"))
					{
						value = ReadInstanceDescriptor(child, errors);
						return (value != null);
					}
					else 
					{
						errors.Add(string.Format("Unexpected element type {0}", child.Name));
						value = null;
						return false;
					}
				}

				// If we get here, it is because there were no nodes.  No nodes and no inner
				// text is how we signify null.
				//
				value = null;
				return true;
			}
			catch(Exception ex)
			{
				errors.Add(ex.Message);
				value = null;
				return false;
			}
		}

		/// This method writes a given byte[] into the XML document, returning the node that
		/// it just created.  Byte arrays have the following XML:
		/// 
		/// <c>
		/// <Binary>
		///		64 bit encoded string representing binary data
		/// </Binary>
		/// </c>
		private XmlNode WriteBinary(XmlDocument document, byte[] value)
		{
			XmlNode node = document.CreateElement("Binary");
			node.InnerText = Convert.ToBase64String(value);
			return node;
		}

		/// Writes the given IList contents into the given parent node.
		private void WriteCollection(XmlDocument document, IList list, XmlNode parent)
		{
			foreach(object obj in list)
			{
				XmlNode node = document.CreateElement("Item");
				XmlAttribute typeAttr = document.CreateAttribute("type");
				typeAttr.Value = obj.GetType().AssemblyQualifiedName;
				node.Attributes.Append(typeAttr);
				WriteValue(document, obj, node);
				parent.AppendChild(node);
			}
		}

		/// This method writes a given instance descriptor into the XML document, returning a node
		/// that it just created.  Instance descriptors have the following XML:
		/// 
		/// <c>
		/// <InstanceDescriptor member="asdfasdfasdf">
		///		<Object>
		///			// param value
		///		</Object>
		/// </InstanceDescriptor>
		/// </c>
		/// 
		/// Here, member is a 64 bit encoded string representing the member, and there is one Parameter
		/// tag for each parameter of the descriptor.
		private XmlNode WriteInstanceDescriptor(XmlDocument document, InstanceDescriptor desc, object value)
		{
			XmlNode node = document.CreateElement("InstanceDescriptor");
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream stream = new MemoryStream();
			formatter.Serialize(stream, desc.MemberInfo);
			XmlAttribute memberAttr = document.CreateAttribute("member");
			memberAttr.Value = Convert.ToBase64String(stream.ToArray());
			node.Attributes.Append(memberAttr);

			foreach(object arg in desc.Arguments)
			{
				XmlNode argNode = document.CreateElement("Argument");
				if (WriteValue(document, arg, argNode))
				{
					node.AppendChild(argNode);
				}
			}

			// Instance descriptors also support "partial" creation, where 
			// properties must also be persisted.
			//
			if (!desc.IsComplete)
			{
				PropertyDescriptorCollection props = TypeDescriptor.GetProperties(value, propertyAttributes);
				WriteProperties(document, props, value, node, "Property");
			}

			return node;
		}

		/// This method writes the given object out to the XML document.  Objects have
		/// the following XML:
		/// 
		/// <c>
		/// <Object type="<object type>" name="<object name>" children="<child property name>">
		/// 
		/// </Object>
		/// </c>
		/// 
		/// Here, Object is the element that defines a custom object.  Type is required
		/// and specifies the data type of the object.  Name is optional.  If present, it names
		/// this object, adding it to the container if the object is an IComponent.
		/// Finally, the children attribute is optional.  If present, this object can have
		/// nested objects, and those objects will be added to the given property name.  The
		/// property must be a collection property that returns an object that implements IList.
		/// 
		/// Inside the object tag there can be zero or more of the following subtags:
		/// 
		///		InstanceDescriptor -- describes how to create an instance of the object.
		///		Property -- a property set on the object
		///		Event -- an event binding
		///		Binary -- binary data
		private XmlNode WriteObject(XmlDocument document, IDictionary nametable, object value)
		{
			Debug.Assert(value != null, "Should not invoke WriteObject with a null value");

			XmlNode node = document.CreateElement("Object");

			XmlAttribute typeAttr = document.CreateAttribute("type");
			typeAttr.Value = value.GetType().AssemblyQualifiedName;
			node.Attributes.Append(typeAttr);

			// Does this object have a name?
			//
			IComponent component = value as IComponent;
			if (component != null && component.Site != null && component.Site.Name != null)
			{
				XmlAttribute nameAttr = document.CreateAttribute("name");
				nameAttr.Value = component.Site.Name;
				node.Attributes.Append(nameAttr);
				Debug.Assert(nametable[component] == null, "WriteObject should not be called more than once for the same object.  Use WriteReference instead");
				nametable[value] = component.Site.Name;
			}

			// Special case:  We want Windows Forms controls to "nest", so child
			// elements are child controls on the form.  This requires either an
			// extensible serialization mechanism (like the existing CodeDom
			// serialization scheme), or it requires special casing in the
			// serialization code.  We choose the latter in order to make
			// this example easier to understand.
			//
			bool isControl = (value is Control);

			if (isControl) 
			{
				XmlAttribute childAttr = document.CreateAttribute("children");
				childAttr.Value = "Controls";
				node.Attributes.Append(childAttr);
			}

			if (component != null)
			{
				// We have a component.  Write out the definition for the component here.  If the
				// component is also a control, recurse so we build up the parent hierarchy.
				//
				if (isControl)
				{
                    IComponent tmp_component = current_component;
					foreach(Control child in ((Control)value).Controls)
					{
						if (child.Site != null && child.Site.Container == host.Container)
						{
                            current_component = child;
							node.AppendChild(WriteObject(document, nametable, child));
						}
					}
                    current_component = tmp_component;
				}

				// Now do our own properties.
				//
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value, propertyAttributes);

				if (isControl)
				{
					// If we are a control and we can locate the control property, we should remove
					// the property from the collection. The collection that comes back from TypeDescriptor
					// is read-only, however, so we must clone it first.
					//
					PropertyDescriptor controlProp = properties["Controls"];
					if (controlProp != null)
					{
						PropertyDescriptor[] propArray = new PropertyDescriptor[properties.Count - 1];
						int idx = 0;
						foreach(PropertyDescriptor p in properties)
						{
							if (p != controlProp)
							{
								propArray[idx++] = p;
							}
						}

						properties = new PropertyDescriptorCollection(propArray);
					}
				}

				WriteProperties(document, properties, value, node, "Property");

				EventDescriptorCollection events = TypeDescriptor.GetEvents(value, propertyAttributes);
				IEventBindingService bindings = host.GetService(typeof(IEventBindingService)) as IEventBindingService;
				if (bindings != null)
				{
					properties = bindings.GetEventProperties(events);
					WriteProperties(document, properties, value, node, "Event");
				}
			}
			else
			{
				// Not a component, so we just write out the value.
				//
				WriteValue(document, value, node);
			}

			return node;
		}

		/// This method writes zero or more property elements into the given parent node.  
		private void WriteProperties(XmlDocument document, PropertyDescriptorCollection properties, object value, XmlNode parent, string elementName)
		{
            PropertyDescriptor tmp_property = current_property;
			foreach(PropertyDescriptor prop in properties)
			{
                current_property = prop;
				if (prop.Name == "AutoScaleBaseSize")
				{
					string _DEBUG_ = prop.Name;
				}

				if (prop.ShouldSerializeValue(value))
				{
					XmlNode node = document.CreateElement(elementName);
					XmlAttribute attr = document.CreateAttribute("name");
					attr.Value = prop.Name;
					node.Attributes.Append(attr);

					DesignerSerializationVisibilityAttribute visibility = (DesignerSerializationVisibilityAttribute)prop.Attributes[typeof(DesignerSerializationVisibilityAttribute)];
					switch(visibility.Visibility)
					{
						case DesignerSerializationVisibility.Visible:
							if (!prop.IsReadOnly && WriteValue(document, prop.GetValue(value), node))
							{
                                PropertyDescriptor tmp_prop = current_property;
                                current_property = prop;
								parent.AppendChild(node);
                                current_property = tmp_prop;
							}
							break;

						case DesignerSerializationVisibility.Content:
							// A "Content" property needs to have its properties stored here, not the actual value.  We 
							// do another special case here to account for collections.  Collections are content properties
							// that implement IList and are read-only.
							//
							object propValue = prop.GetValue(value);

							if (typeof(IList).IsAssignableFrom(prop.PropertyType))
							{
								WriteCollection(document, (IList)propValue, node);
							}
							else
							{
								PropertyDescriptorCollection props = TypeDescriptor.GetProperties(propValue, propertyAttributes);
								WriteProperties(document, props, propValue, node, elementName);
							}
							if (node.ChildNodes.Count > 0)
							{
								parent.AppendChild(node);
							}
							break;

						default:
							break;
					}
				}
			}
            current_property = tmp_property;
		}

		/// Writes a reference to the given component.  Emits the following
		/// XML:
		/// 
		/// <c>
		/// <Reference name="component name"></Reference>
		/// </c>
		private XmlNode WriteReference(XmlDocument document, IComponent value)
		{
			Debug.Assert(value != null && value.Site != null && value.Site.Container == host.Container, "Invalid component passed to WriteReference");

			XmlNode node = document.CreateElement("Reference");
			XmlAttribute attr = document.CreateAttribute("name");
			attr.Value = value.Site.Name;
			node.Attributes.Append(attr);
			return node;
		}

		/// This method writes the given object into the given parent node.  It returns
		/// true if it was successful, or false if it was unable to convert the object
		/// to XML.
		private bool WriteValue(XmlDocument document, object value, XmlNode parent)
		{
			// For empty values, we just return.  This creates an empty node.
			if (value == null)
			{
				return true;
			}

			TypeConverter converter = TypeDescriptor.GetConverter(value);

			if (GetConversionSupported(converter, typeof(string)))
			{
				// Strings have the most fidelity.  If this object
				// supports being converted to a string, do so, and then
				// we're done.
				//
				parent.InnerText = (string)converter.ConvertTo(null, CultureInfo.InvariantCulture, value, typeof(string));
			}
			else if (GetConversionSupported(converter, typeof(byte[])))
			{
                if (value.GetType().IsSerializable)
                {
                    WriteToResources(value);
                }
				// Binary blobs are converted by encoding as a binary element.
				// 
				byte[] data = (byte[])converter.ConvertTo(null, CultureInfo.InvariantCulture, value, typeof(byte[]));
				parent.AppendChild(WriteBinary(document, data));
			}
			else if (GetConversionSupported(converter, typeof(InstanceDescriptor)))
			{
				// InstanceDescriptors are encoded as an InstanceDescriptor element.
				//
				InstanceDescriptor id = (InstanceDescriptor)converter.ConvertTo(null, CultureInfo.InvariantCulture, value, typeof(InstanceDescriptor));
				parent.AppendChild(WriteInstanceDescriptor(document, id, value));
			}
			else if (value is IComponent && ((IComponent)value).Site != null && ((IComponent)value).Site.Container == host.Container)
			{
				// IComponent.  Treat this as a reference.
				//
				parent.AppendChild(WriteReference(document, (IComponent)value));
			}
			else if (value.GetType().IsSerializable)
			{
				// Finally, check to see if this object is serializable.  If it is, we serialize it here
				// and then write it as a binary.
				//
                WriteToResources(value);
				BinaryFormatter formatter = new BinaryFormatter();
				MemoryStream stream = new MemoryStream();
				formatter.Serialize(stream, value);
				XmlNode binaryNode = WriteBinary(document, stream.ToArray());
				parent.AppendChild(binaryNode);
			}
			else
			{
				return false;
			}
			return true;
		}

		public object GetService(Type serviceType)
		{
			return host.GetService(serviceType);
		}

		public IDesignerLoaderHost LoaderHost
		{
			get
			{
				return host as IDesignerLoaderHost;
			}
		}

		/// Save the current state of the loader. If the user loaded the file
		/// or saved once before, then he doesn't need to select a file again.
		/// Unless this is being called as a result of "Save As..." being clicked,
		/// in which case forceFilePrompt will be true.
		internal void Save(bool forceFilePrompt) 
		{
			try
			{
				if (dirty) 
				{
					// Flush any changes to the buffer.
					Flush();
				}

				
				// If the buffer has no name or this is a "Save As...",
				// prompt the user for a file name. The user can save
				// either the C#, VB, or XML (though only the XML can be loaded).
				//
				int filterIndex = 3;
				if ((fileName == null) || forceFilePrompt) 
				{
					SaveFileDialog dlg = new SaveFileDialog();
					dlg.DefaultExt = "xml";
                    dlg.Filter = "Python Files|*.py|C# Files|*.cs|Visual Basic Files|*.vb|XML Files|*.xml";

					if (dlg.ShowDialog() == DialogResult.OK) 
					{
						fileName = dlg.FileName;
						filterIndex = dlg.FilterIndex;
					}
				}

				if (fileName != null) 
				{
					switch (filterIndex)
					{
						case 1:
						{
							// Generate C# code from our codeCompileUnit and save it.
							CodeGeneratorOptions o = new CodeGeneratorOptions();
							o.BlankLinesBetweenMembers = true;
							o.BracingStyle = "C";
							o.ElseOnClosing = false;
							o.IndentString = "    ";
							StreamWriter sw = new StreamWriter(fileName);
							CSharpCodeProvider cs = new CSharpCodeProvider();
							cs.CreateGenerator().GenerateCodeFromCompileUnit(codeCompileUnit, sw, o);
							sw.Close();
						} break;
						case 2:
						{
							// Generate VB code from our codeCompileUnit and save it.
							CodeGeneratorOptions o = new CodeGeneratorOptions();
							o.BlankLinesBetweenMembers = true;
							o.BracingStyle = "C";
							o.ElseOnClosing = false;
							o.IndentString = "    ";
							StreamWriter sw = new StreamWriter(fileName);
							VBCodeProvider vb = new VBCodeProvider();
							vb.CreateGenerator().GenerateCodeFromCompileUnit(codeCompileUnit, sw, o);
							sw.Close();
						} break;
						case 3:
						{
							// Write out our xmlDocument to a file.
							StringWriter sw = new StringWriter();
							XmlTextWriter xtw = new XmlTextWriter(sw);
							xtw.Formatting = Formatting.Indented;
							xmlDocument.WriteTo(xtw);

							// Get rid of our artificial super-root before we save out
							// the XML.
							//
							string cleanup = sw.ToString().Replace("<DOCUMENT_ELEMENT>", "");
							cleanup = cleanup.Replace("</DOCUMENT_ELEMENT>", "");
							xtw.Close();
							StreamWriter file = new StreamWriter(fileName);
							file.Write(cleanup);
							file.Close();
						} break;
					}
					unsaved = false;
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error during save: " + ex.Message);
			}
		}

		/// Called when we want to build an executable. Returns true if we succeeded.
		internal bool Build()
		{
			if (dirty)
			{
				// Flush any changes made to the buffer.
				Flush();
			}

			// If we haven't already chosen a spot to write the executable to,
			// do so now.
			//
			if (executable == null)
			{
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.DefaultExt = "exe";
				dlg.Filter = "Executables|*.exe";

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					executable = dlg.FileName;
				}
			}

			if (executable != null)
			{
				// We'll need our type resolution service in order to find out what
				// assemblies we're dealing with.
				//
				SampleTypeResolutionService strs = host.GetService(typeof(ITypeResolutionService)) as SampleTypeResolutionService;
				
				// We need to collect the parameters that our compiler will use.
				CompilerParameters cp = new CompilerParameters();

				// First, we tell our compiler to reference the assemblies which
				// our designers have referenced (the ones which have import statements
				// in our codeCompileUnit).....
				//
				foreach(Assembly assm in strs.RefencedAssemblies)
				{
					cp.ReferencedAssemblies.Add(assm.Location);

					// .....then we have to look at each one of those assemblies,
					// see which assemblies they reference, and make sure our compiler
					// references those too! Phew!
					//
					foreach (AssemblyName refAssmName in assm.GetReferencedAssemblies())
					{
						Assembly refAssm = Assembly.Load(refAssmName);
						cp.ReferencedAssemblies.Add(refAssm.Location);
					}
				}

				cp.GenerateExecutable = true;
				cp.OutputAssembly = executable;

				// Remember our main class is not Form, but Form1 (or whatever the user calls it)!
				cp.MainClass = host.RootComponent.Site.Name + "Namespace." + host.RootComponent.Site.Name;
				ICodeCompiler cc = new CSharpCodeProvider().CreateCompiler();
				CompilerResults cr = cc.CompileAssemblyFromDom(cp, codeCompileUnit);
				if (cr.Errors.HasErrors)
				{
					string errors = "";
					foreach (CompilerError error in cr.Errors)
					{
						errors += error.ErrorText + "\n";
					}
					MessageBox.Show(errors, "Errors during compile.");
				}
				return !cr.Errors.HasErrors;
			}

			return false;
		}

		// Here we build the executable and then run it. We make sure to not start
		// two of the same process.
		internal void Run()
		{
			if ((run == null) || (run.HasExited))
			{
				if (Build())
				{
					run = new Process();
					run.StartInfo.FileName = executable;
					run.Start();
				}
			}
		}

		// Just in case the red X in the upper right isn't good enough,
		// we can kill our process here.
		internal void Stop()
		{
			if ((run != null) && (!run.HasExited))
			{
				run.Kill();
			}
		}

        // START OF ADD TO SampleDesignerLoader

        // +Add for the copy function
        public void WriteComponentCollection(XmlDocument document, IList list, XmlNode parent)
        {
            Hashtable arr = new Hashtable();
            foreach (object obj in list)
            {
                XmlNode xNode = WriteObject(document, arr, obj);
                document.DocumentElement.AppendChild(xNode);
            }
        }

        public ICollection CreateComponents(XmlDocument xSource, ArrayList errors)
        {
            System.Collections.ArrayList components = new ArrayList();
            string baseClass = null;

            try
            {
                XmlDocument doc = xSource;

                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    if (node.Name.Equals("Object"))
                    {
                        Object obj = CreateObject(node, errors);
                        if (!(obj.GetType().IsSubclassOf(typeof(System.Windows.Forms.Form))))
                        {
                            components.Add(obj);
                        }
                    }
                    else
                    {
                        errors.Add(string.Format("Node type {0} is not allowed here.", node.Name));
                    }
                }
                return components;
            }
            catch (Exception ex)
            {
                errors.Add(ex);
                return null;
            }
        }
        private object CreateObject(XmlNode node, ArrayList errors)
        {
            XmlAttribute typeAttr = node.Attributes["type"];
            if (typeAttr == null)
            {
                errors.Add("<Object> tag is missing required type attribute");
                return null;
            }

            Type type = Type.GetType(typeAttr.Value);
            if (type == null)
            {
                errors.Add(string.Format("Type {0} could not be loaded.", typeAttr.Value));
                return null;
            }

            // This can be null if there is no name for the object.
            //
            XmlAttribute nameAttr = node.Attributes["name"];

            object instance;

            instance = Activator.CreateInstance(type);

            // Got an object, now we must process it.  Check to see if this tag
            // offers a child collection for us to add children to.
            //
            XmlAttribute childAttr = node.Attributes["children"];
            IList childList = null;
            if (childAttr != null)
            {
                PropertyDescriptor childProp = TypeDescriptor.GetProperties(instance)[childAttr.Value];
                if (childProp == null)
                {
                    errors.Add(string.Format("The children attribute lists {0} as the child collection but this is not a property on {1}", childAttr.Value, instance.GetType().FullName));
                }
                else
                {
                    childList = childProp.GetValue(instance) as IList;
                    if (childList == null)
                    {
                        errors.Add(string.Format("The property {0} was found but did not return a valid IList", childProp.Name));
                    }
                }
            }

            // Now, walk the rest of the tags under this element.
            //
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name.Equals("Object"))
                {
                    // Another object.  In this case, create the object, and
                    // parent it to ours using the children property.  If there
                    // is no children property, bail out now.
                    if (childAttr == null)
                    {
                        errors.Add("Child object found but there is no children attribute");
                        continue;
                    }

                    // no sense doing this if there was an error getting the property.  We've already reported the
                    // error above.
                    if (childList != null)
                    {
                        object childInstance = CreateObject(childNode, errors);
                        childList.Add(childInstance);
                    }
                }
                else if (childNode.Name.Equals("Property"))
                {
                    // A property.  Ask the property to parse itself.
                    //
                    ReadProperty(childNode, instance, errors);
                }
                else if (childNode.Name.Equals("Event"))
                {
                    // An event.  Ask the event to parse itself.
                    //
                    ReadEvent(childNode, instance, errors);
                }
            }
            return instance;
        }
        // -Add for the copy function

        // END OF ADD TO SampleDesignerLoader

        public void WriteToResources(object data)
        {
            if (!resources_not_empty)
            {
                resource_writer = new ResourceWriter(resources_file_name);
                resources_not_empty = true;
            }
            if (current_component == null || current_property == null)
            {
                return;
            }
            string name = current_component.Site.Name + '.' + current_property.Name;
            resource_writer.AddResource(name, data);
        }

        public void FinishResources()
        {
            if (resources_not_empty)
            {
                resource_writer.Generate();
                resource_writer.Close();
                resource_writer.Dispose();
                resource_writer = null;
            }
        }
	}
}

