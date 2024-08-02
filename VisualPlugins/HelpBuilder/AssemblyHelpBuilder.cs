using System;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using System.Text;
using System.IO;
using PascalABCCompiler.SemanticTree;
using System.Text.RegularExpressions;

namespace VisualPascalABCPlugins
{
	public class AssemblyHelpBuilder
	{
        List<ItemInfo> namespaces = new List<ItemInfo>();
        List<ItemInfo> classes = new List<ItemInfo>();
		List<ItemInfo> interfaces = new List<ItemInfo>();
		List<ItemInfo> rec_enums = new List<ItemInfo>();
		List<ItemInfo> types = new List<ItemInfo>();
		List<ItemInfo> vars = new List<ItemInfo>();
		List<ItemInfo> procs = new List<ItemInfo>();
		List<ItemInfo> consts = new List<ItemInfo>();
		List<ItemInfo> elems = new List<ItemInfo>();
        Dictionary<ItemInfo, NamespaceElemRec> namespace_elems = new Dictionary<ItemInfo, NamespaceElemRec>();
		Dictionary<ItemInfo, TypeElemRec> type_elems = new Dictionary<ItemInfo, TypeElemRec>();
		List<UnitElemRec> units = new List<UnitElemRec>();
  		XmlReader reader;
  		string unit_name;
  		string full_unit_name;
  		string unit_desc;
  		string output_dir;
  		string index_file_name;
		IVisualEnvironmentCompiler vec = null;
		public BuildOptions options = new BuildOptions();
		public event ProcessEvent CompilationProgress;
		
		public AssemblyHelpBuilder(IVisualEnvironmentCompiler vec)
		{
			this.vec = vec;
			//parser = new FileParser(vec);
			//HelpUtils.builder = this;
		}
		
		public void Clear()
		{
			classes.Clear();
			interfaces.Clear();
			rec_enums.Clear();
			types.Clear();
			vars.Clear();
			procs.Clear();
			consts.Clear();
			type_elems.Clear();
			units.Clear();
			elems.Clear();
			unit_name = null;
			full_unit_name = null;
			unit_desc = null;
			output_dir = null;
			index_file_name = null;
			reader = null;
		}
		
		private void copy_files()
		{
			//string[] files = Directory.GetFiles("Data\\HelpBuilder");
			string[] names = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
			for (int i=0; i<names.Length; i++)
			{
				if (names[i].EndsWith(".gif") || names[i].EndsWith(".css"))
				{
					FileStream fs = File.Create(Path.Combine(output_dir,names[i].Substring("HelpBuilder.Resources.".Length)));
					Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(names[i]);
					byte[] buf = new byte[s.Length];
					s.Read(buf,0,(int)s.Length);
					fs.Write(buf,0,(int)buf.Length);
					fs.Close();
				}
			}
			//for (int i=0; i<files.Length; i++)
			//	File.Copy(files[i],Path.Combine(output_dir,Path.GetFileName(files[i])),true);
		}
		
		public string BuildHelp(Assembly a)
		{
            try
            {
                output_dir = Path.Combine(options.WorkingDirectory, "help_" + Path.GetFileNameWithoutExtension(options.OutputCHMFileName));
                
                if (!Directory.Exists(output_dir))
                    Directory.CreateDirectory(output_dir);
                copy_files();
                int i = 0;
                if (CompilationProgress != null)
                CompilationProgress(string.Format(PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix + "GENERATING_HTML_{0}"), Path.GetFileName(a.ManifestModule.Name)));
                if (_BuildHelp(a) == null)
                    return null;
                classes.Clear();
                interfaces.Clear();
                rec_enums.Clear();
                types.Clear();
                procs.Clear();
                vars.Clear();
                consts.Clear();
                GenerateChmProjectFiles();
                if (CompilationProgress != null)
                CompilationProgress(string.Format(PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix + "CHM_GENERATION_{0}"), options.OutputCHMFileName));
                BuildChm();
                CompilationProgress(PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix + "READY"));
                if (options.GenerateCHM)
                {
                    if (File.Exists(Path.Combine(output_dir, options.OutputCHMFileName)))
                        File.Copy(Path.Combine(output_dir, options.OutputCHMFileName), Path.Combine(options.WorkingDirectory, options.OutputCHMFileName), true);

                    return Path.Combine(options.WorkingDirectory, options.OutputCHMFileName);
                }
                else
                    return index_file_name;
            }
            catch
            {
                throw;
            }
		}
		
		public string _BuildHelp(Assembly a)
		{
			string name = Path.ChangeExtension(a.ManifestModule.Name,".xml");
            if (!File.Exists(name))
            {
                //MessageBox.Show(PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix + "XML_NOT_FOUND"));
                //return null;
            }
			if (GenerateDoc(a))
			    return "";
			return null;
		}
		
		private bool GenerateDoc(Assembly a)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
		    string name = CodeCompletion.AssemblyDocCache.Load(a,a.ManifestModule.FullyQualifiedName);
            if (name == null)
            {
                bool ru = false;
                name = CodeCompletionTools.XmlDoc.LookupLocalizedXmlDoc(a.ManifestModule.FullyQualifiedName, out ru);
            }
            reader = XmlTextReader.Create(name,settings);
  			reader.ReadStartElement("doc");
  			reader.ReadStartElement("assembly");
  			reader.ReadToNextSibling("name");
  			unit_name = reader.ReadString();
  			reader.Close();
  			index_file_name = Path.Combine(output_dir,"index.html");
  			full_unit_name = Path.Combine(output_dir,unit_name+".html");
  			SaveIndexFile();
  			
  			//reader.ReadEndElement();
  			//FillTables();
  			
  			SaveUnitMembers(a,ElemKind.None);
  			
  			Type[] _classes = GetClasses(a);
  			if (_classes.Length > 0)
  			{
  				SaveUnitMembers(a,ElemKind.Class);
  			}
  			foreach (Type t in _classes)
  			{
    			if (!HelpUtils.can_write(t))
  					continue;
  				ItemInfo it = SaveClass(t);
  				if (!HelpUtils.can_show_members(t))
  				{
  					type_elems.Add(it,new TypeElemRec());
  					continue;
  				}
  				List<ItemInfo> props = new List<ItemInfo>();
  				PropertyInfo[] _props = GetNonPrivateProperties(t);
  				if (_props.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Property);
  				}
  				foreach (PropertyInfo p in _props)
  				{
  					if (!HelpUtils.can_write(p))
  						continue;
  					
  					SaveProperty(p,props,it);
  				}
  				List<ItemInfo> meths = new List<ItemInfo>();
  				List<ItemInfo> constrs = new List<ItemInfo>();
  				MethodInfo[] _meths = GetNonPrivateMethods(t);
  				if (_meths.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Method);
  				}
  				ConstructorInfo[] _constrs = GetNonPrivateConstructors(t);
  				if (_constrs.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Constructor);
  				}
  				foreach (MethodInfo m in t.GetMethods())
  				{
  					if (!HelpUtils.can_write(m))
  						continue;
  					if (!m.IsPrivate)
  					SaveMethod(m,meths,constrs,it);
  					/*if (m.IsConstructor)
  					{
  						if (is_overloaded_constructor(m))
  							SaveConstructorOverloadsList(GetOverloadedConstructors(t));
  					}
  					else*/
  					if (is_overload(m,t))
  						SaveOverloadsList(GetOverloadsList(t,m));
  				}
  				List<ItemInfo> fields = new List<ItemInfo>();
  				FieldInfo[] _fields = GetNonPrivateFields(t);
  				if (_fields.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Field);
  				}
  				foreach (FieldInfo f in _fields)
  				{
  					if (!HelpUtils.can_write(f))
  						continue;
  					SaveField(f,fields,it);
  				}
  				List<ItemInfo> events = new List<ItemInfo>();
  				EventInfo[] _events = GetNonPrivateEvents(t);
  				if (_events.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Event);
  				}
  				foreach (EventInfo e in _events)
  				{
  					if (!HelpUtils.can_write(e))
  						continue;
  					SaveEvent(e,events,it);
  				}
  				List<ItemInfo> consts = new List<ItemInfo>();
                IClassConstantDefinitionNode[] _consts = null;
  				type_elems.Add(it,new TypeElemRec(props,meths,constrs,fields,events,consts));
  			}
  			
  			//interfejsy
  			_classes = GetInterfaces(a);
  			if (_classes.Length > 0)
  			{
  				SaveUnitMembers(a,ElemKind.Interface);
  			}
  			foreach (Type t in _classes)
  			{
    			if (!HelpUtils.can_write(t))
  					continue;
  				ItemInfo it = SaveClass(t);
  				if (!HelpUtils.can_show_members(t))
  				{
  					type_elems.Add(it,new TypeElemRec());
  					continue;
  				}
  				List<ItemInfo> props = new List<ItemInfo>();
  				PropertyInfo[] _props = GetNonPrivateProperties(t);
  				if (_props.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Property);
  				}
  				foreach (PropertyInfo p in _props)
  				{
  					if (!HelpUtils.can_write(p))
  						continue;
  					SaveProperty(p,props,it);
  				}
  				List<ItemInfo> meths = new List<ItemInfo>();
  				List<ItemInfo> constrs = new List<ItemInfo>();
  				MethodInfo[] _meths = GetNonPrivateMethods(t);
  				if (_meths.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Method);
  				}
  				foreach (MethodInfo m in _meths)
  				{
  					if (!HelpUtils.can_write(m))
  						continue;
  					SaveMethod(m,meths,constrs,it);
  					if (is_overload(m,t))
  						SaveOverloadsList(GetOverloadsList(t,m));
  				}
  				List<ItemInfo> events = new List<ItemInfo>();
  				EventInfo[] _events = GetNonPrivateEvents(t);
  				if (_events.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Event);
  				}
  				foreach (EventInfo e in _events)
  				{
  					if (!HelpUtils.can_write(e))
  						continue;
  					SaveEvent(e,events,it);
  				}
  				type_elems.Add(it,new TypeElemRec(props,meths,constrs,new List<ItemInfo>(),events,new List<ItemInfo>()));
  			}
  			
  			//zapisi perechislenija
  			Type[] _rec_enums = GetEnumsRecords(a);
  			if (_rec_enums.Length > 0)
  			{
  				SaveUnitMembers(a,ElemKind.EnumRecord);
  			}
  			foreach (Type t in _rec_enums)
  			{
    			if (!HelpUtils.can_write(t))
  					continue;
  				ItemInfo it = SaveClass(t);
  				if (!HelpUtils.can_show_members(t))
  				{
  					type_elems.Add(it,new TypeElemRec());
  					continue;
  				}
  				PropertyInfo[] _props = GetNonPrivateProperties(t);
  				if (_props.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Property);
  				}
  				List<ItemInfo> props = new List<ItemInfo>();
  				foreach (PropertyInfo p in _props)
  				{
  					if (!HelpUtils.can_write(p))
  						continue;
  					SaveProperty(p,props,it);
  				}
  				List<ItemInfo> meths = new List<ItemInfo>();
  				List<ItemInfo> constrs = new List<ItemInfo>();
  				MethodInfo[] _meths = GetNonPrivateMethods(t);
  				if (_meths.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Method);
  				}
  				ConstructorInfo[] _constrs = GetNonPrivateConstructors(t);
  				if (_constrs.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Constructor);
  				}
                
  				foreach (MethodInfo m in _meths)
  				{
  					if (!HelpUtils.can_write(m))
  						continue;
  					SaveMethod(m,meths,constrs,it);
  					/*if (m.IsConstructor)
  					{
  						if (is_overloaded_constructor(m))
  							SaveConstructorOverloadsList(GetOverloadedConstructors(t));
  					}
  					else*/
  					if (is_overload(m,t))
  						SaveOverloadsList(GetOverloadsList(t,m));
  				}

                /*foreach (ConstructorInfo ci in _constrs)
                {
                    if (!HelpUtils.can_write(ci))
                        continue;
                    SaveConstructor(ci,constrs,it);
                    if (is_overloaded_constructor(ci))
                        SaveConstructorOverloadsList();
                }*/

  				FieldInfo[] _fields = GetNonPrivateFields(t);
  				if (_fields.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Field);
  				}
  				List<ItemInfo> fields = new List<ItemInfo>();
  				foreach (FieldInfo f in _fields)
  				{
  					if (!HelpUtils.can_write(f))
  						continue;
  					SaveField(f,fields,it);
  				}
  				EventInfo[] _events = GetNonPrivateEvents(t);
  				if (_events.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Event);
  				}
  				List<ItemInfo> events = new List<ItemInfo>();
  				foreach (EventInfo e in _events)
  				{
  					if (!HelpUtils.can_write(e))
  						continue;
  					SaveEvent(e,events,it);
  				}
  				List<ItemInfo> consts = new List<ItemInfo>();
  				
  				type_elems.Add(it,new TypeElemRec(props,meths,constrs,fields,events,consts));
  			}
  			
  			
  			units.Add(new UnitElemRec(unit_name,full_unit_name,classes,interfaces,rec_enums,types,procs,vars,this.consts));
  			//GenerateChmProjectFiles();
  			//BuildChm();
  			return true;
		}

        private List<MethodInfo> GetOverloadsList(Type t, MethodInfo mi)
        {
            List<MethodInfo> meths = new List<MethodInfo>();
            MethodInfo[] mis = mi.DeclaringType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            meths.Add(mi);
            foreach (MethodInfo m in mis)
            {
                if (m != mi && m.Name == mi.Name)
                    meths.Add(m);
            }
            return meths;
        }

        public static int GetConstructorIndex(ConstructorInfo ci)
        {
            ConstructorInfo[] constrs = ci.DeclaringType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            int ind = 1;
            foreach (ConstructorInfo c in constrs)
            {
                if ((c.IsPublic || c.IsFamily) && c != ci)
                    ind++;
                else
                    return ind;
            }
            return ind;
        }

        public static string possible_overload_constructor_symbol(ConstructorInfo f)
        {
            if (is_overloaded_constructor(f))
            {
                int ind = GetConstructorIndex(f);
                if (ind != 1)
                    return "$" + GetConstructorIndex(f);
                else
                    return "";
            }
            return "";
        }

        public static int GetMethodIndex(MethodInfo ci, Type t)
        {
            MethodInfo[] constrs = ci.DeclaringType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            int ind = 1;
            foreach (MethodInfo c in constrs)
            {
                if ((c.IsPublic || c.IsFamily) && c.Name == ci.Name && c != ci)
                    ind++;
                else
                    return ind;
            }
            return ind;
        }

        public static bool is_overloaded_constructor(ConstructorInfo ci)
        {
            ConstructorInfo[] constrs = ci.DeclaringType.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ConstructorInfo c in constrs)
                if (c != ci && (c.IsPublic ||c.IsFamily))
                    return true;
            return false;
        }

        private static bool is_overload(MethodInfo mi, Type t)
        {
            MethodInfo[] mis = GetNonPrivateMethods(t);
            foreach (MethodInfo meth in mis)
            {
                if (meth.Name == mi.Name && meth != mi)
                    return true;
            }
            return false;
        }

        private Type[] GetClasses(Assembly a)
        {
            Type[] tt = a.GetExportedTypes();
            List<Type> classes = new List<Type>();
            foreach (Type t in tt)
            {
                if (t.IsClass)
                    classes.Add(t);
            }
            return classes.ToArray();
        }

        private static Type[] GetInterfaces(Assembly a)
        {
            Type[] tt = a.GetExportedTypes();
            List<Type> classes = new List<Type>();
            foreach (Type t in tt)
            {
                if (t.IsInterface)
                    classes.Add(t);
            }
            return classes.ToArray();
        }

        private static Type[] GetEnumsRecords(Assembly a)
        {
            Type[] tt = a.GetExportedTypes();
            List<Type> classes = new List<Type>();
            foreach (Type t in tt)
            {
                if (t.IsEnum || t.IsValueType)
                    classes.Add(t);
            }
            return classes.ToArray();
        }

        private static PropertyInfo[] GetNonPrivateProperties(Type t)
        {
            PropertyInfo[] pis = t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            List<PropertyInfo> props = new List<PropertyInfo>();
            foreach (PropertyInfo pi in pis)
            {
                if (is_not_private(pi))
                    props.Add(pi);
            }
            return props.ToArray();
        }

        private static FieldInfo[] GetNonPrivateFields(Type t)
        {
            FieldInfo[] pis = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            List<FieldInfo> props = new List<FieldInfo>();
            foreach (FieldInfo pi in pis)
            {
                if (is_not_private(pi))
                    props.Add(pi);
            }
            return props.ToArray();
        }

        private static ConstructorInfo[] GetNonPrivateConstructors(Type t)
        {
            ConstructorInfo[] pis = t.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            List<ConstructorInfo> props = new List<ConstructorInfo>();
            foreach (ConstructorInfo pi in pis)
            {
                if (is_not_private(pi))
                    props.Add(pi);
            }
            return props.ToArray();
        }
        
        private static MethodInfo[] GetNonPrivateMethods(Type t)
        {
            MethodInfo[] pis = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            List<MethodInfo> props = new List<MethodInfo>();
            foreach (MethodInfo pi in pis)
            {
                if (is_not_private(pi))
                    props.Add(pi);
            }
            return props.ToArray();
        }

        private EventInfo[] GetNonPrivateEvents(Type t)
        {
            EventInfo[] pis = t.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            List<EventInfo> props = new List<EventInfo>();
            foreach (EventInfo pi in pis)
            {
                if (is_not_private(pi))
                    props.Add(pi);
            }
            return props.ToArray();
        }

        internal static bool is_not_private(MethodInfo mi)
        {
            return mi.IsPublic || mi.IsFamily;
        }

        internal static bool is_not_private(ConstructorInfo mi)
        {
            return mi.IsPublic || mi.IsFamily;
        }

        internal static bool is_not_private(FieldInfo fi)
        {
            return fi.IsPublic || fi.IsFamily;
        }

        internal static bool is_not_private(PropertyInfo pi)
        {
            if (pi.GetGetMethod(true) != null)
            return pi.GetGetMethod(true).IsPublic || pi.GetGetMethod(true).IsFamily;
            return false;
        }

        internal static bool is_not_private(EventInfo pi)
        {
            if (pi.GetAddMethod(true) != null)
                return pi.GetAddMethod(true).IsPublic || pi.GetAddMethod(true).IsFamily;
            return false;
        }

        private FieldInfo[] GetFields(Type t, field_access_level mod)
        {
            FieldInfo[] flds = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            List<FieldInfo> fields = new List<FieldInfo>();
            foreach (FieldInfo fi in flds)
            {
                if (mod == field_access_level.fal_public && fi.IsPublic)
                    fields.Add(fi);
                else if (mod == field_access_level.fal_protected && (fi.IsFamily || fi.IsFamilyOrAssembly))
                    fields.Add(fi);
            }
            return fields.ToArray();
        }
        

        private MethodInfo[] GetMethods(Type t, field_access_level mod)
        {
            MethodInfo[] mths = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            List<MethodInfo> meths = new List<MethodInfo>();
            foreach (MethodInfo mi in mths)
            {
                if (mod == field_access_level.fal_public && mi.IsPublic)
                    meths.Add(mi);
                else if (mod == field_access_level.fal_protected && (mi.IsFamily || mi.IsFamilyOrAssembly))
                    meths.Add(mi);
            }
            return meths.ToArray();
        }

        private PropertyInfo[] GetProperties(Type t, field_access_level mod)
        {
            PropertyInfo[] flds = t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            List<PropertyInfo> fields = new List<PropertyInfo>();
            foreach (PropertyInfo fi in flds)
            {
                MethodInfo get_meth = fi.GetGetMethod(true);
                if (get_meth != null)
                if (mod == field_access_level.fal_public && get_meth.IsPublic)
                    fields.Add(fi);
                else if (mod == field_access_level.fal_protected && (get_meth.IsFamily || get_meth.IsFamilyOrAssembly))
                    fields.Add(fi);
            }
            return fields.ToArray();
        }

        private EventInfo[] GetEvents(Type t, field_access_level mod)
        {
            EventInfo[] flds = t.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            List<EventInfo> fields = new List<EventInfo>();
            foreach (EventInfo fi in flds)
            {
                MethodInfo add_meth = fi.GetAddMethod(true);
                if (add_meth != null)
                if (mod == field_access_level.fal_public && add_meth.IsPublic)
                    fields.Add(fi);
                else if (mod == field_access_level.fal_protected && (add_meth.IsFamily || add_meth.IsFamilyOrAssembly))
                    fields.Add(fi);
            }
            return fields.ToArray();
        }

		private void BuildChm()
		{
			string hhc_file_name = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),@"HTML Help Workshop\hhc.exe");
			
			if (!File.Exists(hhc_file_name))
				return;
			Process p = new Process();
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.ErrorDialog = true;
			p.StartInfo.FileName = hhc_file_name;
			p.StartInfo.Arguments = Path.Combine(this.output_dir,"prj.hhp");
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.WorkingDirectory = this.output_dir;
			bool b = p.Start();
			while (!p.HasExited)
				Thread.Sleep(10);
		}
		
		private void GenerateChmProjectFiles()
		{
			//StreamWriter sw = File.CreateText(Path.Combine(this.output_dir,"prj.hhp"));
			StreamWriter sw = new StreamWriter(Path.Combine(this.output_dir,"prj.hhp"),false,System.Text.Encoding.GetEncoding(1251));
			sw.WriteLine("[OPTIONS]");
			sw.WriteLine("Compatibility=1.1 or later");
			sw.WriteLine("Default Window=MsdnHelp");
			sw.WriteLine("Display compile progress=No");
			sw.WriteLine("Full-text search=Yes");
			sw.WriteLine("Language=0x0419 Russian");
			sw.WriteLine("Binary TOC=Yes");
			sw.WriteLine("Title=PascalABC.NET");
			sw.WriteLine("Default topic="+unit_name+".html");
			sw.WriteLine("Compiled file="+options.OutputCHMFileName);
			sw.WriteLine("Contents file=Documentation.hhc");
			sw.WriteLine();
			
			sw.WriteLine("[WINDOWS]");
			sw.WriteLine("MsdnHelp=\"PascalABC.NET Help Library\",\"Documentation.hhc\",\"Documentation.hhk\",\"index.html\",\"index.html\",,,,,0x62520,220,0x387e,[86,51,872,558],,,,,,,0");
			sw.WriteLine();
			sw.WriteLine("[FILES]");
			string[] files = Directory.GetFiles(output_dir);
			for (int i=0; i<files.Length; i++)
			{
				string ext = Path.GetExtension(files[i]).ToLower();
				if (ext != ".hhc" && ext != ".hhp" && ext != ".hhk")
					sw.WriteLine(Path.GetFileName(files[i]));
			}
			sw.WriteLine();
			sw.WriteLine("[INFOTYPES]");
			sw.Close();
			
			sw = new StreamWriter(Path.Combine(this.output_dir,"Documentation.hhc"),false,System.Text.Encoding.GetEncoding(1251));
			sw.WriteLine("<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML/RU\">");
			sw.WriteLine("<HTML><BODY>");
			//files = Directory.GetFiles(output_dir,"*.html");
			foreach (UnitElemRec r in units)
			{
				this.classes = r.classes;
				this.interfaces = r.interfaces;
				this.rec_enums = r.rec_enums;
				this.types = r.types;
				this.consts = r.consts;
				this.vars = r.vars;
				this.procs = r.procs;
				this.unit_name = r.unit_name;
				this.full_unit_name = r.full_unit_name;
				WriteHHCContent(sw);
			}
			sw.WriteLine("</HTML></BODY>");
			sw.Close();
			
			sw = new StreamWriter(Path.Combine(this.output_dir,"Documentation.hhk"),false,System.Text.Encoding.GetEncoding(1251));
			sw.WriteLine("<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML/RU\">");
			sw.WriteLine("<HTML><BODY>");
			//files = Directory.GetFiles(output_dir,"*.html");
			foreach (UnitElemRec r in units)
			{
				this.classes = r.classes;
				this.interfaces = r.interfaces;
				this.rec_enums = r.rec_enums;
				this.types = r.types;
				this.consts = r.consts;
				this.vars = r.vars;
				this.procs = r.procs;
				this.unit_name = r.unit_name;
				WriteHHKContent(sw);
			}
			sw.WriteLine("</HTML></BODY>");
			
			sw.Close();
		}
		
		private void WriteHHCContent(StreamWriter sw)
		{
			sw.WriteLine("<UL>");
			sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
			sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("UNIT")+" "+unit_name+"\">");
			sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileName(full_unit_name)+"\">");
			sw.WriteLine("</OBJECT>");
			sw.WriteLine("</LI>");
			classes.Sort(new ItemInfoComparer());
			if (classes.Count > 0)
			{
				sw.WriteLine("<UL>");
				sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
				sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("CLASSES")+"\">");
				sw.WriteLine("<param name=\"Local\" value=\""+unit_name+"$classes.html"+"\">");
				sw.WriteLine("</OBJECT>");
				sw.WriteLine("</LI>");
				
				
				for (int i=0; i<classes.Count; i++)
				{
					sw.WriteLine("<UL>");
					sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
					sw.WriteLine("<param name=\"Name\" value=\""+classes[i].header+"\">");
					sw.WriteLine("<param name=\"Local\" value=\""+classes[i].file_name+"\">");
					sw.WriteLine("</OBJECT>");
					sw.WriteLine("</LI>");
					
					TypeElemRec ter = type_elems[classes[i]];
					ter.constrs.Sort(new ItemInfoComparer());
					if (ter.constrs.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("CONSTRUCTORS")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(classes[i].file_name)+"Members$Constructors.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.constrs.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.constrs[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.constrs[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					
					ter.meths.Sort(new ItemInfoComparer());
					if (ter.meths.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("METHODS")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(classes[i].file_name)+"Members$Methods.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.meths.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.meths[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.meths[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					ter.props.Sort(new ItemInfoComparer());
					if (ter.props.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("PROPERTIES")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(classes[i].file_name)+"Members$Properties.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.props.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.props[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.props[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					
					ter.fields.Sort(new ItemInfoComparer());
					if (ter.fields.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("FIELDS")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(classes[i].file_name)+"Members$Fields.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.fields.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.fields[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.fields[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					ter.events.Sort(new ItemInfoComparer());
					if (ter.events.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("EVENTS")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(classes[i].file_name)+"Members$Events.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.events.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.events[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.events[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					ter.consts.Sort(new ItemInfoComparer());
					if (ter.consts.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("CONSTANTS")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(classes[i].file_name)+"Members$ClassConstants.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.consts.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.consts[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.consts[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					sw.WriteLine("</UL>");
				}
				sw.WriteLine("</UL>");
			}
			interfaces.Sort(new ItemInfoComparer());
			if (interfaces.Count > 0)
			{
				sw.WriteLine("<UL>");
				sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
				sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("INTERFACES")+"\">");
				sw.WriteLine("<param name=\"Local\" value=\""+unit_name+"$interfaces.html"+"\">");
				sw.WriteLine("</OBJECT>");
				sw.WriteLine("</LI>");
				
				
				for (int i=0; i<interfaces.Count; i++)
				{
					sw.WriteLine("<UL>");
					sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
					sw.WriteLine("<param name=\"Name\" value=\""+interfaces[i].header+"\">");
					sw.WriteLine("<param name=\"Local\" value=\""+interfaces[i].file_name+"\">");
					sw.WriteLine("</OBJECT>");
					sw.WriteLine("</LI>");
					
					TypeElemRec ter = type_elems[classes[i]];
					ter.meths.Sort(new ItemInfoComparer());
					if (ter.meths.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("METHODS")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(interfaces[i].file_name)+"Members$Methods.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.meths.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.meths[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.meths[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					ter.props.Sort(new ItemInfoComparer());
					if (ter.props.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("PROPERTIES")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(interfaces[i].file_name)+"Members$Properties.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.props.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.props[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.props[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					
					ter.events.Sort(new ItemInfoComparer());
					if (ter.events.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("EVENTS")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(interfaces[i].file_name)+"Members$Events.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.events.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.events[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.events[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					sw.WriteLine("</UL>");
				}
				sw.WriteLine("</UL>");
			}
			rec_enums.Sort(new ItemInfoComparer());
			if (rec_enums.Count > 0)
			{
				sw.WriteLine("<UL>");
				sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
				sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("ENUMS_RECORDS")+"\">");
				sw.WriteLine("<param name=\"Local\" value=\""+unit_name+"$records.html"+"\">");
				sw.WriteLine("</OBJECT>");
				sw.WriteLine("</LI>");
				
				
				for (int i=0; i<rec_enums.Count; i++)
				{
					sw.WriteLine("<UL>");
					sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
					sw.WriteLine("<param name=\"Name\" value=\""+rec_enums[i].header+"\">");
					sw.WriteLine("<param name=\"Local\" value=\""+rec_enums[i].file_name+"\">");
					sw.WriteLine("</OBJECT>");
					sw.WriteLine("</LI>");
					
					TypeElemRec ter = type_elems[rec_enums[i]];
					ter.constrs.Sort(new ItemInfoComparer());
					if (ter.constrs.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("CONSTRUCTORS")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(rec_enums[i].file_name)+"Members$Constructors.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.constrs.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.constrs[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.constrs[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					
					ter.meths.Sort(new ItemInfoComparer());
					if (ter.meths.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("METHODS")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(rec_enums[i].file_name)+"Members$Methods.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.meths.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.meths[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.meths[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					ter.props.Sort(new ItemInfoComparer());
					if (ter.props.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("PROPERTIES")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(rec_enums[i].file_name)+"Members$Properties.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.props.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.props[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.props[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					
					ter.fields.Sort(new ItemInfoComparer());
					if (ter.fields.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("FIELDS")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(rec_enums[i].file_name)+"Members$Fields.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.fields.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.fields[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.fields[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					ter.events.Sort(new ItemInfoComparer());
					if (ter.events.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("EVENTS")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(rec_enums[i].file_name)+"Members$Events.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.events.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.events[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.events[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					ter.consts.Sort(new ItemInfoComparer());
					if (ter.consts.Count > 0)
					{
						sw.WriteLine("<UL>");
						sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
						sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("CONSTANTS")+"\">");
						sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileNameWithoutExtension(rec_enums[i].file_name)+"Members$ClassConstants.html"+"\">");
						sw.WriteLine("</OBJECT>");
						sw.WriteLine("</LI>");
						for (int j=0; j<ter.consts.Count; j++)
						{
							sw.WriteLine("<UL>");
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.consts[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.consts[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
							sw.WriteLine("</UL>");
						}
						sw.WriteLine("</UL>");
					}
					sw.WriteLine("</UL>");
				}
				sw.WriteLine("</UL>");
			}
			
			//types
			types.Sort(new ItemInfoComparer());
			if (types.Count > 0)
			{
				sw.WriteLine("<UL>");
				sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
				sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("TYPES")+"\">");
				sw.WriteLine("<param name=\"Local\" value=\""+unit_name+"$types.html"+"\">");
				sw.WriteLine("</OBJECT>");
				sw.WriteLine("</LI>");
				
				
				for (int i=0; i<types.Count; i++)
				{
					sw.WriteLine("<UL>");
					sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
					sw.WriteLine("<param name=\"Name\" value=\""+types[i].header+"\">");
					sw.WriteLine("<param name=\"Local\" value=\""+types[i].file_name+"\">");
					sw.WriteLine("</OBJECT>");
					sw.WriteLine("</LI>");
					sw.WriteLine("</UL>");
				}
				sw.WriteLine("</UL>");
			}
			
			//globals
			procs.Sort(new ItemInfoComparer());
			if (procs.Count > 0)
			{
				sw.WriteLine("<UL>");
				sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
				sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("SUBPROGRAMMS")+"\">");
				sw.WriteLine("<param name=\"Local\" value=\""+unit_name+"$subroutines.html"+"\">");
				sw.WriteLine("</OBJECT>");
				sw.WriteLine("</LI>");
				
				
				for (int i=0; i<procs.Count; i++)
				{
					sw.WriteLine("<UL>");
					sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
					sw.WriteLine("<param name=\"Name\" value=\""+procs[i].header+"\">");
					sw.WriteLine("<param name=\"Local\" value=\""+procs[i].file_name+"\">");
					sw.WriteLine("</OBJECT>");
					sw.WriteLine("</LI>");
					sw.WriteLine("</UL>");
				}
				sw.WriteLine("</UL>");
			}
			vars.Sort(new ItemInfoComparer());
			if (vars.Count > 0)
			{
				sw.WriteLine("<UL>");
				sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
				sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("VARIABLES")+"\">");
				sw.WriteLine("<param name=\"Local\" value=\""+unit_name+"$variables.html"+"\">");
				sw.WriteLine("</OBJECT>");
				sw.WriteLine("</LI>");
				
				
				for (int i=0; i<vars.Count; i++)
				{
					sw.WriteLine("<UL>");
					sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
					sw.WriteLine("<param name=\"Name\" value=\""+vars[i].header+"\">");
					sw.WriteLine("<param name=\"Local\" value=\""+vars[i].file_name+"\">");
					sw.WriteLine("</OBJECT>");
					sw.WriteLine("</LI>");
					sw.WriteLine("</UL>");
				}
				sw.WriteLine("</UL>");
			}
			consts.Sort(new ItemInfoComparer());
			if (consts.Count > 0)
			{
				sw.WriteLine("<UL>");
				sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
				sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("CONSTANTS")+"\">");
				sw.WriteLine("<param name=\"Local\" value=\""+unit_name+"$constants.html"+"\">");
				sw.WriteLine("</OBJECT>");
				sw.WriteLine("</LI>");
				
				
				for (int i=0; i<consts.Count; i++)
				{
					sw.WriteLine("<UL>");
					sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
					sw.WriteLine("<param name=\"Name\" value=\""+consts[i].header+"\">");
					sw.WriteLine("<param name=\"Local\" value=\""+consts[i].file_name+"\">");
					sw.WriteLine("</OBJECT>");
					sw.WriteLine("</LI>");
					sw.WriteLine("</UL>");
				}
				sw.WriteLine("</UL>");
			}
			sw.WriteLine("</UL>");
			//foreach (ICo
		}
		
		private void WriteHHKContent(StreamWriter sw)
		{
			sw.WriteLine("<UL>");
			sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
			sw.WriteLine("<param name=\"Name\" value=\""+HelpUtils.get_localization("UNIT")+" "+unit_name+"\">");
			sw.WriteLine("<param name=\"Local\" value=\""+Path.GetFileName(full_unit_name)+"\">");
			sw.WriteLine("</OBJECT>");
			sw.WriteLine("</LI>");
            namespaces.Sort(new ItemInfoComparer());
            if (namespaces.Count > 0)
            {
                for (int k = 0; k < namespaces.Count; k++)
                {
                    NamespaceElemRec ns = namespace_elems[namespaces[k]];
                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                    sw.WriteLine("<param name=\"Name\" value=\"" + namespaces[k].header + "\">");
                    sw.WriteLine("<param name=\"Local\" value=\"" + namespaces[k].file_name + "\">");
                    sw.WriteLine("</OBJECT>");
                    sw.WriteLine("</LI>");
                    //klassy
                    List<ItemInfo> classes = ns.classes;
                    classes.Sort(new ItemInfoComparer());
                    if (classes.Count > 0)
                    {
                        for (int i = 0; i < classes.Count; i++)
                        {
                            TypeElemRec ter = type_elems[classes[i]];
                            sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                            sw.WriteLine("<param name=\"Name\" value=\"" + classes[i].header + "\">");
                            sw.WriteLine("<param name=\"Local\" value=\"" + classes[i].file_name + "\">");
                            sw.WriteLine("</OBJECT>");
                            sw.WriteLine("</LI>");
                            ter.constrs.Sort(new ItemInfoComparer());
                            if (ter.constrs.Count > 0)
                            {
                                for (int j = 0; j < ter.constrs.Count; j++)
                                {
                                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                                    sw.WriteLine("<param name=\"Name\" value=\"" + ter.constrs[j].header + "\">");
                                    sw.WriteLine("<param name=\"Local\" value=\"" + ter.constrs[j].file_name + "\">");
                                    sw.WriteLine("</OBJECT>");
                                    sw.WriteLine("</LI>");
                                }
                            }

                            ter.meths.Sort(new ItemInfoComparer());
                            if (ter.meths.Count > 0)
                            {
                                for (int j = 0; j < ter.meths.Count; j++)
                                {
                                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                                    sw.WriteLine("<param name=\"Name\" value=\"" + ter.meths[j].header + "\">");
                                    sw.WriteLine("<param name=\"Local\" value=\"" + ter.meths[j].file_name + "\">");
                                    sw.WriteLine("</OBJECT>");
                                    sw.WriteLine("</LI>");
                                }
                            }
                            ter.props.Sort(new ItemInfoComparer());
                            if (ter.props.Count > 0)
                            {
                                for (int j = 0; j < ter.props.Count; j++)
                                {
                                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                                    sw.WriteLine("<param name=\"Name\" value=\"" + ter.props[j].header + "\">");
                                    sw.WriteLine("<param name=\"Local\" value=\"" + ter.props[j].file_name + "\">");
                                    sw.WriteLine("</OBJECT>");
                                    sw.WriteLine("</LI>");
                                }
                            }

                            ter.fields.Sort(new ItemInfoComparer());
                            if (ter.fields.Count > 0)
                            {
                                for (int j = 0; j < ter.fields.Count; j++)
                                {
                                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                                    sw.WriteLine("<param name=\"Name\" value=\"" + ter.fields[j].header + "\">");
                                    sw.WriteLine("<param name=\"Local\" value=\"" + ter.fields[j].file_name + "\">");
                                    sw.WriteLine("</OBJECT>");
                                    sw.WriteLine("</LI>");
                                }
                            }
                            ter.events.Sort(new ItemInfoComparer());
                            if (ter.events.Count > 0)
                            {
                                for (int j = 0; j < ter.events.Count; j++)
                                {
                                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                                    sw.WriteLine("<param name=\"Name\" value=\"" + ter.events[j].header + "\">");
                                    sw.WriteLine("<param name=\"Local\" value=\"" + ter.events[j].file_name + "\">");
                                    sw.WriteLine("</OBJECT>");
                                    sw.WriteLine("</LI>");
                                }
                            }
                        }
                    }
                    List<ItemInfo> interfaces = ns.interfaces;
                    interfaces.Sort(new ItemInfoComparer());
                    if (interfaces.Count > 0)
                    {
                        for (int i = 0; i < interfaces.Count; i++)
                        {
                            TypeElemRec ter = type_elems[interfaces[i]];
                            sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                            sw.WriteLine("<param name=\"Name\" value=\"" + interfaces[i].header + "\">");
                            sw.WriteLine("<param name=\"Local\" value=\"" + interfaces[i].file_name + "\">");
                            sw.WriteLine("</OBJECT>");
                            sw.WriteLine("</LI>");

                            ter.meths.Sort(new ItemInfoComparer());
                            if (ter.meths.Count > 0)
                            {
                                for (int j = 0; j < ter.meths.Count; j++)
                                {
                                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                                    sw.WriteLine("<param name=\"Name\" value=\"" + ter.meths[j].header + "\">");
                                    sw.WriteLine("<param name=\"Local\" value=\"" + ter.meths[j].file_name + "\">");
                                    sw.WriteLine("</OBJECT>");
                                    sw.WriteLine("</LI>");
                                }
                            }
                            ter.props.Sort(new ItemInfoComparer());
                            if (ter.props.Count > 0)
                            {
                                for (int j = 0; j < ter.props.Count; j++)
                                {
                                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                                    sw.WriteLine("<param name=\"Name\" value=\"" + ter.props[j].header + "\">");
                                    sw.WriteLine("<param name=\"Local\" value=\"" + ter.props[j].file_name + "\">");
                                    sw.WriteLine("</OBJECT>");
                                    sw.WriteLine("</LI>");
                                }
                            }

                            ter.events.Sort(new ItemInfoComparer());
                            if (ter.events.Count > 0)
                            {
                                for (int j = 0; j < ter.events.Count; j++)
                                {
                                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                                    sw.WriteLine("<param name=\"Name\" value=\"" + ter.events[j].header + "\">");
                                    sw.WriteLine("<param name=\"Local\" value=\"" + ter.events[j].file_name + "\">");
                                    sw.WriteLine("</OBJECT>");
                                    sw.WriteLine("</LI>");
                                }
                            }
                        }
                    }

                    //records
                    List<ItemInfo> records = ns.records;
                    records.Sort(new ItemInfoComparer());
                    if (records.Count > 0)
                    {
                        for (int i = 0; i < records.Count; i++)
                        {
                            TypeElemRec ter = type_elems[records[i]];
                            sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                            sw.WriteLine("<param name=\"Name\" value=\"" + records[i].header + "\">");
                            sw.WriteLine("<param name=\"Local\" value=\"" + records[i].file_name + "\">");
                            sw.WriteLine("</OBJECT>");
                            sw.WriteLine("</LI>");
                            ter.constrs.Sort(new ItemInfoComparer());
                            if (ter.constrs.Count > 0)
                            {
                                for (int j = 0; j < ter.constrs.Count; j++)
                                {
                                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                                    sw.WriteLine("<param name=\"Name\" value=\"" + ter.constrs[j].header + "\">");
                                    sw.WriteLine("<param name=\"Local\" value=\"" + ter.constrs[j].file_name + "\">");
                                    sw.WriteLine("</OBJECT>");
                                    sw.WriteLine("</LI>");
                                }
                            }

                            ter.meths.Sort(new ItemInfoComparer());
                            if (ter.meths.Count > 0)
                            {
                                for (int j = 0; j < ter.meths.Count; j++)
                                {
                                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                                    sw.WriteLine("<param name=\"Name\" value=\"" + ter.meths[j].header + "\">");
                                    sw.WriteLine("<param name=\"Local\" value=\"" + ter.meths[j].file_name + "\">");
                                    sw.WriteLine("</OBJECT>");
                                    sw.WriteLine("</LI>");
                                }
                            }
                            ter.props.Sort(new ItemInfoComparer());
                            if (ter.props.Count > 0)
                            {
                                for (int j = 0; j < ter.props.Count; j++)
                                {
                                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                                    sw.WriteLine("<param name=\"Name\" value=\"" + ter.props[j].header + "\">");
                                    sw.WriteLine("<param name=\"Local\" value=\"" + ter.props[j].file_name + "\">");
                                    sw.WriteLine("</OBJECT>");
                                    sw.WriteLine("</LI>");
                                }
                            }

                            ter.fields.Sort(new ItemInfoComparer());
                            if (ter.fields.Count > 0)
                            {
                                for (int j = 0; j < ter.fields.Count; j++)
                                {
                                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                                    sw.WriteLine("<param name=\"Name\" value=\"" + ter.fields[j].header + "\">");
                                    sw.WriteLine("<param name=\"Local\" value=\"" + ter.fields[j].file_name + "\">");
                                    sw.WriteLine("</OBJECT>");
                                    sw.WriteLine("</LI>");
                                }
                            }
                            ter.events.Sort(new ItemInfoComparer());
                            if (ter.events.Count > 0)
                            {
                                for (int j = 0; j < ter.events.Count; j++)
                                {
                                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                                    sw.WriteLine("<param name=\"Name\" value=\"" + ter.events[j].header + "\">");
                                    sw.WriteLine("<param name=\"Local\" value=\"" + ter.events[j].file_name + "\">");
                                    sw.WriteLine("</OBJECT>");
                                    sw.WriteLine("</LI>");
                                }
                            }
                            ter.consts.Sort(new ItemInfoComparer());
                            if (ter.consts.Count > 0)
                            {
                                for (int j = 0; j < ter.consts.Count; j++)
                                {
                                    sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                                    sw.WriteLine("<param name=\"Name\" value=\"" + ter.consts[j].header + "\">");
                                    sw.WriteLine("<param name=\"Local\" value=\"" + ter.consts[j].file_name + "\">");
                                    sw.WriteLine("</OBJECT>");
                                    sw.WriteLine("</LI>");
                                }
                            }
                        }
                    }

                    List<ItemInfo> enums = ns.enums;
                    //types
                    types.Sort(new ItemInfoComparer());
                    if (types.Count > 0)
                    {
                        for (int i = 0; i < types.Count; i++)
                        {

                            TypeElemRec ter = type_elems[types[i]];
                            sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
                            sw.WriteLine("<param name=\"Name\" value=\"" + types[i].header + "\">");
                            sw.WriteLine("<param name=\"Local\" value=\"" + types[i].file_name + "\">");
                            sw.WriteLine("</OBJECT>");
                            sw.WriteLine("</LI>");
                        }
                    }

                }
            }
			sw.WriteLine("</UL>");
		}
		
		private void SaveIndexFile()
		{
			StreamWriter sw = new StreamWriter(index_file_name,false,System.Text.Encoding.GetEncoding(1251));
			sw.WriteLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Frameset//EN\">");
			sw.WriteLine("<html>");
			sw.WriteLine("<head>");
			sw.WriteLine("<title>"+unit_name+"</title>");
			sw.WriteLine("</head>");
			sw.WriteLine("<frameset cols=\"*\" framespacing=\"6\" bordercolor=\"#6699CC\">");
			sw.WriteLine("<frame name=\"main\" src=\""+unit_name+".html\""+" frameborder=\"1\">");
			sw.WriteLine("<noframes><p>This page requires frames, but your browser does not support them.</p></noframes>");
			sw.WriteLine("</frameset>");
			sw.WriteLine("</html>");
			sw.Close();
		}
		
		private void WriteNameDescriptionHeaderForTable(StreamWriter sw)
		{
			sw.WriteLine("<tr valign=\"top\">");
  			sw.WriteLine("<th width=\"50%\">"+HelpUtils.get_localization("NAME")+"</th>");
  			sw.WriteLine("<th width=\"50%\">"+HelpUtils.get_localization("DESCRIPTION_WO_COLON")+"</th>");
  			sw.WriteLine("</tr>");
		}
		
		private void WriteTableHeader(StreamWriter sw, string name)
		{
			sw.WriteLine("<div id=\"nstext\">");
  			sw.WriteLine("<h3 class=\"dtH3\">"+name+"</h3>");
  			sw.WriteLine("<div class=\"tablediv\">");
  			sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
		}
		
		private void WriteFooterTable(StreamWriter sw)
		{
			sw.WriteLine("</table>");
  			sw.WriteLine("</div>");
  			sw.WriteLine("</div>");
		}
		
		enum ElemKind
		{
			None,
			Class,
			Interface,
			EnumRecord,
			Type,
			Variable,
			Const,
			Property,
			Constructor,
			Method,
			Field,
			Subroutine,
			Event,
			ClassConstant
		}
		
		private void SaveUnitMembers(Assembly a, ElemKind kind)
		{
			string _unit_name = unit_name;
			switch (kind)
			{
				case ElemKind.Class : _unit_name += "$Classes"; break;
				case ElemKind.EnumRecord : _unit_name += "$Records"; break;
				case ElemKind.Type : _unit_name += "$Types"; break;
				case ElemKind.Variable : _unit_name += "$Variables"; break;
				case ElemKind.Subroutine : _unit_name += "$Subroutines"; break;
				case ElemKind.Const : _unit_name += "$Constants"; break;
				case ElemKind.Interface : _unit_name += "$Interfaces"; break;
			}
			string _full_unit_name = Path.Combine(output_dir,_unit_name+".html");
			StreamWriter sw = new StreamWriter(_full_unit_name,false,System.Text.Encoding.GetEncoding(1251));
  			WriteBanner(sw);
  			if (kind == ElemKind.None)
  			elems.Add(new ItemInfo(unit_name+".html",HelpUtils.get_localization("UNIT")+" "+unit_name));
  			sw.WriteLine("<div id=\"TitleRow\">");
  			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("UNIT")+" "+unit_name+"</h1>");
  			sw.WriteLine("</div></div>");
  			if (kind == ElemKind.None)
  			{
  				sw.WriteLine("<div id=\"nstext\">");
  				if (!string.IsNullOrEmpty(unit_desc) && unit_desc != Convert.ToString((char)0xA0))
  				sw.WriteLine("<h4 class=\"dtH4\">"+HelpUtils.get_localization("DESCRIPTION")+"</h4>");
  				if (!string.IsNullOrEmpty(unit_desc) && unit_desc != Convert.ToString((char)0xA0))
  				sw.WriteLine("<p>"+unit_desc+"</p>");
  				sw.WriteLine("</div>");
  			}
  			//klassy
  			Type[] _classes = GetClasses(a);
            if (_classes.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Class))
            {
                WriteTableHeader(sw, HelpUtils.get_localization("CLASSES"));
                WriteNameDescriptionHeaderForTable(sw);
                foreach (Type t in _classes)
                {
                    if (!HelpUtils.can_write(t))
                        continue;
                    sw.WriteLine("<tr valign=\"top\">");
                    sw.WriteLine("<td width=\"50%\">");
                    sw.WriteLine("<img src=\"pubclass.gif\" />");
                    sw.WriteLine("<a href=\"" + HelpUtils.get_type_name(t) + ".html" + "\">" + HelpUtils.build_name_with_possible_generic(t, true) + "</a>");
                    sw.WriteLine("</td>");
                    sw.WriteLine("<td width=\"50%\">" + HelpUtils.get_summary(HelpUtils.GetDocumentation(t)) + "</td>");
                    sw.WriteLine("</tr>");
                }
                WriteFooterTable(sw);
            }
  			
  			Type[] _interfaces = GetInterfaces(a);
            if (_interfaces.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Interface))
            {
                WriteTableHeader(sw, HelpUtils.get_localization("INTERFACES"));
                WriteNameDescriptionHeaderForTable(sw);
                foreach (Type t in _interfaces)
                {
                    if (!HelpUtils.can_write(t))
                        continue;
                    sw.WriteLine("<tr valign=\"top\">");
                    sw.WriteLine("<td width=\"50%\">");
                    sw.WriteLine("<img src=\"pubinterface.gif\" />");
                    sw.WriteLine("<a href=\"" + HelpUtils.get_type_name(t) + ".html" + "\">" + HelpUtils.build_name_with_possible_generic(t, true) + "</a>");
                    sw.WriteLine("</td>");
                    sw.WriteLine("<td width=\"50%\">" + HelpUtils.get_summary(HelpUtils.GetDocumentation(t)) + "</td>");
                    sw.WriteLine("</tr>");
                }
                WriteFooterTable(sw);
            }
  			
  			Type[] _enum_recs = GetEnumsRecords(a);
            if (_enum_recs.Length > 0 && (kind == ElemKind.None || kind == ElemKind.EnumRecord))
            {
                WriteTableHeader(sw, HelpUtils.get_localization("ENUMS_RECORDS"));
                WriteNameDescriptionHeaderForTable(sw);
                foreach (Type t in _enum_recs)
                {
                    if (!HelpUtils.can_write(t))
                        continue;
                    sw.WriteLine("<tr valign=\"top\">");
                    sw.WriteLine("<td width=\"50%\">");

                    if (t.IsEnum)
                        sw.WriteLine("<img src=\"pubenum.gif\" />");
                    else if (t.IsValueType)
                        sw.WriteLine("<img src=\"pubstructure.gif\" />");
                    sw.WriteLine("<a href=\"" + HelpUtils.get_type_name(t) + ".html" + "\">" + HelpUtils.build_name_with_possible_generic(t, true) + "</a>");
                    sw.WriteLine("</td>");
                    sw.WriteLine("<td width=\"50%\">" + HelpUtils.get_summary(HelpUtils.GetDocumentation(t)) + "</td>");
                    sw.WriteLine("</tr>");
                }
                WriteFooterTable(sw);
            }
  			
  			sw.WriteLine("</body>");
  			sw.WriteLine("</html>");
  			sw.Close();
		}
		
		private void WriteSpaces(StreamWriter sw, int num)
		{
			for (int i=0; i<num; i++)
				sw.Write((char)0xA0);
		}
		
		private void SaveOverloadsList(List<MethodInfo> meths)
		{
			string name = HelpUtils.get_meth_name(meths[0])+"$overloads";
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			WriteBanner(sw);
			sw.WriteLine("<div id=\"TitleRow\">");
			if (meths[0].ReturnType == typeof(void))
			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("PROCEDURE")+" "+meths[0].Name+"</h1>");
			else
			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("FUNCTION")+" "+meths[0].Name+"</h1>");
  			sw.WriteLine("</div></div>");
  			WriteTableHeader(sw,HelpUtils.get_localization("OVERLOADED_SUBPROGRAMMS"));
  			//sw.WriteLine("<tr VALIGN=\"top\">");
  			WriteNameDescriptionHeaderForTable(sw);
  			foreach (MethodInfo meth in meths)
  			{
  				if (!HelpUtils.can_write(meth))
  					continue;
  				sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"pubmethod.gif\"></img>");
  				sw.Write("<a href=\""+HelpUtils.get_meth_name(meth)+possible_overload_symbol(meth)+".html\">"+HelpUtils.get_simple_meth_header(meth)+"</a>");
  				sw.Write("</td>");
  				sw.Write("<td width=\"50%\">");
  				sw.Write(HelpUtils.get_summary(HelpUtils.GetDocumentation(meth)));
  				sw.WriteLine("</td></tr>");
  			}
  			WriteFooterTable(sw);
  			sw.WriteLine("</body></html>");
  			sw.Close();
		}
		
		private void SaveConstructorOverloadsList(List<ConstructorInfo> meths)
		{
			string name = HelpUtils.get_meth_name(meths[0])+"$overloads";
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			WriteBanner(sw);
			sw.WriteLine("<div id=\"TitleRow\">");
			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("CONSTRUCTOR")+" "+"Create"+"</h1>");
  			sw.WriteLine("</div></div>");
  			WriteTableHeader(sw,HelpUtils.get_localization("OVERLOADED_CONSTRUCTORS"));
  			//sw.WriteLine("<tr VALIGN=\"top\">");
  			WriteNameDescriptionHeaderForTable(sw);
  			foreach (ConstructorInfo meth in meths)
  			{
  				if (!HelpUtils.can_write(meth))
  					continue;
  				sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"pubmethod.gif\"></img>");
  				sw.Write("<a href=\""+HelpUtils.get_meth_name(meth)+possible_overload_constructor_symbol(meth)+".html\">"+HelpUtils.get_simple_meth_header(meth)+"</a>");
  				sw.Write("</td>");
  				sw.Write("<td width=\"50%\">");
  				sw.Write(HelpUtils.get_summary(HelpUtils.GetDocumentation(meth)));
  				sw.WriteLine("</td></tr>");
  			}
  			WriteFooterTable(sw);
  			sw.WriteLine("</body></html>");
  			sw.Close();
		}
		
		private void SaveOverloadsList(List<ICommonMethodNode> meths)
		{
			string name = HelpUtils.get_meth_name(meths[0])+"$overloads";
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			WriteBanner(sw);
			sw.WriteLine("<div id=\"TitleRow\">");
			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("METHOD")+" "+meths[0].name+"</h1>");
  			sw.WriteLine("</div></div>");
  			WriteTableHeader(sw,HelpUtils.get_localization("OVERLOADED_METHODS"));
  			//sw.WriteLine("<tr VALIGN=\"top\">");
  			WriteNameDescriptionHeaderForTable(sw);
  			foreach (ICommonMethodNode meth in meths)
  			{
  				if (!HelpUtils.can_write(meth))
  					continue;
  				sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"pubmethod.gif\"></img>");
  				sw.Write("<a href=\""+HelpUtils.get_meth_name(meth)+HelpUtils.possible_overload_symbol(meth)+".html\">"+HelpUtils.get_simple_meth_header(meth)+"</a>");
  				sw.Write("</td>");
  				sw.Write("<td width=\"50%\">");
  				sw.Write(HelpUtils.get_summary(meth.Documentation));
  				sw.WriteLine("</td></tr>");
  			}
  			WriteFooterTable(sw);
  			sw.WriteLine("</body></html>");
  			sw.Close();
		}
		
		private void SaveMembersToHTML(Type t, ElemKind kind)
		{
			string name = HelpUtils.get_type_name(t);
			string _name = name;
			switch (kind)
			{
				case ElemKind.Property : _name += "Members$Properties.html"; break;
				case ElemKind.Method : _name += "Members$Methods.html"; break;
				case ElemKind.Constructor : _name += "Members$Constructors.html"; break;
				case ElemKind.Field : _name += "Members$Fields.html"; break;
				case ElemKind.Event : _name += "Members$Events.html"; break;
				case ElemKind.ClassConstant : _name += "Members$ClassConstants.html"; break;
				default : _name += "Members.html"; break;
			}
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,_name),false,System.Text.Encoding.GetEncoding(1251));
			if (kind == ElemKind.None)
			elems.Add(new ItemInfo(name+"Members.html",HelpUtils.get_localization("CLASS")+" "+name));
			WriteBanner(sw);
			sw.WriteLine("<div id=\"TitleRow\">");
			if (t.IsEnum)
  			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("TYPE_MEMBERS")+" "+name+"</h1>");
			else if (t.IsValueType)
			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("RECORD_MEMBERS")+" "+name+"</h1>");
			else if (t.IsInterface)
			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("INTERFACE_MEMBERS")+" "+name+"</h1>");
			else if (t.IsClass)
			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("CLASS_MEMBERS")+" "+name+"</h1>");
  			sw.WriteLine("</div></div>");
  			sw.WriteLine("<div id=\"nstext\">");
  			sw.WriteLine("<p><a href=\""+name+".html"+"\">"+ HelpUtils.get_localization("PREVIEW")+" "+t.Name+"</a></p>");
  			ConstructorInfo[] constrs = GetNonPrivateConstructors(t);
  			if (constrs.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Constructor))
  			{
  				sw.WriteLine("<h4 class=\"dtH4\">"+HelpUtils.get_localization("CONSTRUCTORS")+"</h4>");
  				sw.WriteLine("<div class=\"tablediv\">");
  				sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
  				WriteNameDescriptionHeaderForTable(sw);
  				sw.WriteLine("<td width=\"50%\">");
  				sw.WriteLine("<img src=\"pubmethod.gif\" />");
  				if (constrs.Length == 1)
  				sw.WriteLine("<a href=\""+HelpUtils.get_meth_name(constrs[0])+".html\">Create</a>");
  				else
  				sw.WriteLine("<a href=\""+HelpUtils.get_meth_name(constrs[0])+"$overloads"+".html\">Create</a>");
  				sw.WriteLine("</td>");
  				sw.Write("<td width=\"50%\">");
  				if (constrs.Length == 1)
  					sw.Write(HelpUtils.get_summary(HelpUtils.GetDocumentation(constrs[0])));
  				else
  					sw.Write(HelpUtils.get_localization("OVERLOADED_CONSTRUCTOR")+" "+t.Name);
  				sw.WriteLine("</td>");
  				sw.WriteLine("</tr></table></div>");
  			}
  			
  			
  			PropertyInfo[] props = GetProperties(t, field_access_level.fal_public);
            if (props.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Property))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PUBLIC_PROPERTIES") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (PropertyInfo prop in props)
                {
                    if (!HelpUtils.can_write(prop))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"pubproperty.gif\"></img>");
                    if (prop.GetGetMethod() != null && prop.GetGetMethod().IsStatic)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_prop_name(prop) + ".html\">" + prop.Name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(HelpUtils.GetDocumentation(prop));
                    sw.Write(doc);
                    if (prop.DeclaringType != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(prop.DeclaringType) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			MethodInfo[] meths = GetMethods(t, field_access_level.fal_public);
            if (meths.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Method))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PUBLIC_METHODS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (MethodInfo meth in meths)
                {
                    if (!HelpUtils.can_write(meth))
                        continue;
                    int ind = GetMethodIndex(meth, t);
                    if (is_overload(meth, t) && ind > 1)
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"pubmethod.gif\"></img>");
                    if (meth.IsStatic)
                        sw.Write("<img src=\"static.gif\"></img>");
                    if (is_overload(meth, t))
                        sw.Write("<a href=\"" + HelpUtils.get_meth_name(meth) + ".html\">" + meth.Name + "</a>");
                    else
                    {
                        sw.Write("<a href=\"" + HelpUtils.get_meth_name(meth) + "$overloads" + ".html\">" + meth.Name + "</a>");
                    }
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    if (is_overload(meth, t))
                        sw.Write(HelpUtils.get_localization("OVERLOADED") + " ");
                    string doc = HelpUtils.get_summary(HelpUtils.GetDocumentation(meth));
                    sw.Write(doc);
                    if (meth.DeclaringType != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(meth.DeclaringType) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			FieldInfo[] fields = GetFields(t, field_access_level.fal_public);
            if (fields.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Field))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PUBLIC_FIELDS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (FieldInfo fld in fields)
                {
                    if (!HelpUtils.can_write(fld))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"pubfield.gif\"></img>");
                    if (fld.IsStatic)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_field_name(fld) + ".html\">" + fld.Name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(HelpUtils.GetDocumentation(fld));
                    sw.Write(doc);
                    if (fld.DeclaringType != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(fld.DeclaringType) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			EventInfo[] events = GetEvents(t, field_access_level.fal_public);
            if (events.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Event))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PUBLIC_EVENTS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (EventInfo e in events)
                {
                    if (!HelpUtils.can_write(e))
                        continue;
                    sw.Write("<tr valign=\"top\"><td width=\"50%\"><img src=\"pubevent.gif\"></img>");
                    if (e.GetAddMethod().IsStatic)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_event_name(e) + ".html\">" + e.Name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(HelpUtils.GetDocumentation(e));
                    sw.Write(doc);
                    if (e.DeclaringType != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(e.DeclaringType) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			//internal
  			props = GetProperties(t, field_access_level.fal_internal);
            if (props.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Property))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("INTERNAL_PROPERTIES") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (PropertyInfo prop in props)
                {
                    if (!HelpUtils.can_write(prop))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"intproperty.gif\"></img>");
                    if (prop.GetGetMethod() != null && prop.GetGetMethod().IsStatic)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_prop_name(prop) + ".html\">" + prop.Name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(HelpUtils.GetDocumentation(prop));
                    sw.Write(doc);
                    if (prop.DeclaringType != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(prop.DeclaringType) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			meths = GetMethods(t, field_access_level.fal_internal);
            if (meths.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Method))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("INTERNAL_METHODS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (MethodInfo meth in meths)
                {
                    if (!HelpUtils.can_write(meth))
                        continue;
                    int ind = GetMethodIndex(meth, t);
                    if (is_overload(meth, t) && ind > 1)
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"intmethod.gif\"></img>");
                    if (meth.IsStatic)
                        sw.Write("<img src=\"static.gif\"></img>");
                    if (!is_overload(meth, t))
                        sw.Write("<a href=\"" + HelpUtils.get_meth_name(meth) + ".html\">" + meth.Name + "</a>");
                    else
                    {
                        sw.Write("<a href=\"" + HelpUtils.get_meth_name(meth) + "$overloads" + ".html\">" + meth.Name + "</a>");
                    }
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    if (is_overload(meth, t))
                        sw.Write(HelpUtils.get_localization("OVERLOADED") + " ");
                    string doc = HelpUtils.get_summary(HelpUtils.GetDocumentation(meth));
                    sw.Write(doc);
                    if (meth.DeclaringType != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(meth.DeclaringType) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			fields = GetFields(t, field_access_level.fal_internal);
            if (fields.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Field))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("INTERNAL_FIELDS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (FieldInfo fld in fields)
                {
                    if (!HelpUtils.can_write(fld))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"intfield.gif\"></img>");
                    if (fld.IsStatic)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_field_name(fld) + ".html\">" + fld.Name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(HelpUtils.GetDocumentation(fld));
                    sw.Write(doc);
                    if (fld.DeclaringType != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(fld.DeclaringType) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			events = GetEvents(t, field_access_level.fal_internal);
            if (events.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Event))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("INTERNAL_EVENTS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (EventInfo e in events)
                {
                    if (!HelpUtils.can_write(e))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"intevent.gif\"></img>");
                    if (e.GetAddMethod().IsStatic)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_event_name(e) + ".html\">" + e.Name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(HelpUtils.GetDocumentation(e));
                    sw.Write(doc);
                    if (e.DeclaringType != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(e.DeclaringType) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  				
  			//protected
  			props = GetProperties(t, field_access_level.fal_protected);
            if (props.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Property))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PROTECTED_FIELDS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (PropertyInfo prop in props)
                {
                    if (!HelpUtils.can_write(prop))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"protproperty.gif\"></img>");
                    if (prop.GetGetMethod(true).IsStatic)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_prop_name(prop) + ".html\">" + prop.Name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(HelpUtils.GetDocumentation(prop));
                    sw.Write(doc);
                    if (prop.DeclaringType != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(prop.DeclaringType) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			meths = GetMethods(t, field_access_level.fal_protected);
            if (meths.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Method))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PROTECTED_METHODS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (MethodInfo meth in meths)
                {
                    if (!HelpUtils.can_write(meth))
                        continue;
                    int ind = GetMethodIndex(meth, t);
                    if (is_overload(meth, t) && ind > 1)
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"protmethod.gif\"></img>");
                    if (meth.IsStatic)
                        sw.Write("<img src=\"static.gif\"></img>");
                    if (!is_overload(meth, t))
                        sw.Write("<a href=\"" + HelpUtils.get_meth_name(meth) + ".html\">" + meth.Name + "</a>");
                    else
                    {
                        sw.Write("<a href=\"" + HelpUtils.get_meth_name(meth) + "$overloads" + ".html\">" + meth.Name + "</a>");
                    }
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    if (is_overload(meth, t))
                        sw.Write(HelpUtils.get_localization("OVERLOADED") + " ");
                    string doc = HelpUtils.get_summary(HelpUtils.GetDocumentation(meth));
                    sw.Write(doc);
                    if (meth.DeclaringType != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(meth.DeclaringType) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			fields = GetFields(t, field_access_level.fal_protected);
            if (fields.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Field))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PROTECTED_FIELDS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (FieldInfo fld in fields)
                {
                    if (!HelpUtils.can_write(fld))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"protfield.gif\"></img>");
                    if (fld.IsStatic)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_field_name(fld) + ".html\">" + fld.Name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(HelpUtils.GetDocumentation(fld));
                    sw.Write(doc);
                    if (fld.DeclaringType != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(fld.DeclaringType) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			events = GetEvents(t, field_access_level.fal_protected);
            if (events.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Event))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PROTECTED_EVENTS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (EventInfo e in events)
                {
                    if (!HelpUtils.can_write(e))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"protevent.gif\"></img>");
                    if (e.GetAddMethod(true).IsStatic)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_event_name(e) + ".html\">" + e.Name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(HelpUtils.GetDocumentation(e));
                    sw.Write(doc);
                    if (e.DeclaringType != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(e.DeclaringType) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
		}

		private void SaveProperty(PropertyInfo p, List<ItemInfo> props, ItemInfo decl_type)
		{
			string name = HelpUtils.get_prop_name(p);
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			props.Add(new ItemInfo(name+".html",p.Name));
			WriteBanner(sw);
			WriteElementHeaderDescription(sw,HelpUtils.get_localization("PROPERTY")+" "+p.Name,HelpUtils.get_summary(HelpUtils.GetDocumentation(p)));
  			WriteSyntaxHeader(sw);
  			sw.WriteLine(HelpUtils.get_property_header(p));
  			sw.WriteLine("</div>");
  			string example = HelpUtils.get_example(HelpUtils.GetDocumentation(p));
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("TYPE"),decl_type.file_name,decl_type.header);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
		}
		
		private void SaveField(FieldInfo f, List<ItemInfo> fields, ItemInfo decl_type)
		{
			string name = HelpUtils.get_field_name(f);
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			fields.Add(new ItemInfo(name+".html",f.Name));
			WriteBanner(sw);
			WriteElementHeaderDescription(sw,HelpUtils.get_localization("FIELD")+" "+f.Name,HelpUtils.get_summary(HelpUtils.GetDocumentation(f)));
  			WriteSyntaxHeader(sw);
  			sw.WriteLine(HelpUtils.get_field_header(f));
  			sw.WriteLine("</div>");
  			string example = HelpUtils.get_example(HelpUtils.GetDocumentation(f));
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("TYPE"),decl_type.file_name,decl_type.header);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
		}
		
		private void SaveEvent(EventInfo e, List<ItemInfo> events, ItemInfo decl_type)
		{
			string name = HelpUtils.get_event_name(e);
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			events.Add(new ItemInfo(name+".html",e.Name));
			WriteBanner(sw);
			WriteElementHeaderDescription(sw,HelpUtils.get_localization("EVENT")+" "+e.Name,HelpUtils.get_summary(HelpUtils.GetDocumentation(e)));
  			WriteSyntaxHeader(sw);
  			sw.WriteLine(HelpUtils.get_event_header(e));
  			sw.WriteLine("</div>");
  			string example = HelpUtils.get_example(HelpUtils.GetDocumentation(e));
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("TYPE"),decl_type.file_name,decl_type.header);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
		}

        private bool is_getter_or_setter(MethodInfo mi)
        {
            PropertyInfo[] props = mi.DeclaringType.GetProperties(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static);
            foreach (PropertyInfo pi in props)
                if (pi.GetGetMethod(true) == mi || pi.GetSetMethod(true) == mi)
                    return true;
            return false;
        }

		private void SaveMethod(MethodInfo m, List<ItemInfo> meths, List<ItemInfo> constrs, ItemInfo decl_type)
		{
			string name = HelpUtils.get_meth_name(m);
			if (is_getter_or_setter(m))
				return;
			string file_name = null;
			if (!m.IsConstructor)
				file_name = name+possible_overload_symbol(m)+".html";
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,file_name),false,System.Text.Encoding.GetEncoding(1251));
			if (is_overload(m, m.DeclaringType))
			{
				if (GetMethodIndex(m, m.DeclaringType) == 1)
					meths.Add(new ItemInfo(name+"$overloads"+".html",m.Name));
			}
			else
			meths.Add(new ItemInfo(file_name,m.Name));
			WriteBanner(sw);
			WriteElementHeaderDescription(sw,HelpUtils.get_localization("METHOD")+" "+HelpUtils.build_name_with_possible_generic(m,true),HelpUtils.get_summary(HelpUtils.GetDocumentation(m)));
  			WriteSyntaxHeader(sw);
  			sw.WriteLine(HelpUtils.get_meth_header(m));
  			sw.WriteLine("</div>");
  			List<NameInfo> prms = HelpUtils.get_parameters(HelpUtils.GetDocumentation(m));
  			if (prms.Count > 0)
  			{
  				sw.WriteLine("<h4 class=\"dtH4\">"+HelpUtils.get_localization("PARAMETERS")+"</h4>");
  				sw.WriteLine("<p>");
  				foreach (NameInfo pi in prms)
  				{
  					sw.Write("<b>"+pi.name+":</b>");
  					sw.WriteLine("<br />");
  					WriteSpaces(sw,3);
  					sw.WriteLine(pi.desc);
  					sw.WriteLine("<br />");
  				}
  				sw.WriteLine("</p>");
  			}
  			string ret_value = HelpUtils.get_return_value_description(HelpUtils.GetDocumentation(m));
  			if (!string.IsNullOrEmpty(ret_value))
  			{
  				sw.WriteLine("<h4 class=\"dtH4\">"+HelpUtils.get_localization("RETURNS")+"</h4>");
  				sw.WriteLine("<p>");
  				sw.WriteLine(ret_value);
  				sw.WriteLine("</p>");
  			}
  			string example = HelpUtils.get_example(HelpUtils.GetDocumentation(m));
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("TYPE"),decl_type.file_name,decl_type.header);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
		}

        public static string possible_overload_symbol(MethodInfo f)
        {
            if (is_overload(f, f.DeclaringType))
            {
                int ind = GetMethodIndex(f, f.DeclaringType);
                if (ind != 1)
                    return "$" + GetMethodIndex(f, f.DeclaringType);
                else
                    return "";
            }
            return "";
        }

		private void WriteExample(StreamWriter sw, string comment, string code)
		{
			sw.WriteLine("<h4 class=\"dtH4\">"+HelpUtils.get_localization("EXAMPLE")+"</h4>");
			if (!string.IsNullOrEmpty(comment))
			sw.WriteLine("<p>"+comment+"<p>");
			if (!string.IsNullOrEmpty(code))
			{
				sw.WriteLine("<div class=\"activeLangTab\">");
				sw.WriteLine("<div>PascalABC.NET</div>");
				sw.WriteLine("</div>");
				sw.WriteLine("<div class=\"syntax\">");
				sw.Write("<pre style=\"font-size:110%; padding:2px;\">");
				sw.Write(code);
				sw.Write("</pre>");
				sw.WriteLine("</div>");
			}
		}
		
		private void WriteElementHeaderDescription(StreamWriter sw, string name, string doc)
		{
			sw.WriteLine("<div id=\"TitleRow\">");
  			sw.WriteLine("<h1 class=\"dtH1\">"+name+"</h1>");
			sw.WriteLine("</div></div>");			
  			sw.WriteLine("<div id=\"nstext\">");
  			if (!string.IsNullOrEmpty(doc) && doc != Convert.ToString((char)0xA0))
  			sw.WriteLine("<h4 class=\"dtH4\">"+HelpUtils.get_localization("DESCRIPTION")+"</h4>");
  			if (!string.IsNullOrEmpty(doc) && doc != Convert.ToString((char)0xA0))
  			sw.WriteLine("<p>"+doc+"</p>");
		}
		
		private void WriteSyntaxHeader(StreamWriter sw)
		{
			sw.WriteLine("<h4 class=\"dtH4\">"+HelpUtils.get_localization("SYNTAX")+"</h4>");
			sw.WriteLine("<div class=\"activeLangTab\">");
			sw.WriteLine("<div>PascalABC.NET</div>");
			sw.WriteLine("</div>");
			sw.WriteLine("<div class=\"syntax\">");
		}
		
		private void WriteLocation(StreamWriter sw, string obj, string file_name, string name)
		{
			sw.WriteLine("<h4 class=\"dtH4\">"+HelpUtils.get_localization("LOCATED")+"</h4>");
  			sw.WriteLine("<p>");
  			sw.WriteLine("<b>"+obj+"</b>");
  			sw.WriteLine("<a href=\""+file_name+"\">"+name+"</a>");
  			sw.WriteLine("</p>");
		}
		
		private ItemInfo SaveClass(Type tt)
		{
			string name = HelpUtils.get_type_name(tt);
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			ItemInfo it = new ItemInfo(name+".html",HelpUtils.build_name_with_possible_generic(tt,false));
			WriteBanner(sw);
			if (!HelpUtils.can_show_members(tt))
			{
				types.Add(it);
				WriteElementHeaderDescription(sw,HelpUtils.get_localization("TYPE")+" "+HelpUtils.build_name_with_possible_generic(tt,true),HelpUtils.get_summary(HelpUtils.GetDocumentation(tt)));
			}
			else
			if (tt.IsEnum || tt.IsValueType)
			{
				rec_enums.Add(it);
				if (tt.IsEnum)
				{
					WriteElementHeaderDescription(sw,HelpUtils.get_localization("ENUM")+" "+HelpUtils.build_name_with_possible_generic(tt,true),HelpUtils.get_summary(HelpUtils.GetDocumentation(tt)));
					sw.WriteLine("<p>");
  					sw.WriteLine(HelpUtils.get_localization("FOR_SEE_TYPE_MEMBERS")+" <a href=\""+name+"Members.html"+"\">"+tt.Namespace+"."+HelpUtils.build_name_with_possible_generic(tt,true)+"</a>.</p>");
				}
				else if (tt.IsValueType)
				{
                    WriteElementHeaderDescription(sw, HelpUtils.get_localization("RECORD") + " " + HelpUtils.build_name_with_possible_generic(tt, true), HelpUtils.get_summary(HelpUtils.GetDocumentation(tt)));
					sw.WriteLine("<p>");
  					sw.WriteLine(HelpUtils.get_localization("FOR_SEE_RECORD_MEMBERS")+" <a href=\""+name+"Members.html"+"\">"+tt.Namespace+"."+HelpUtils.build_name_with_possible_generic(tt,true)+"</a>.</p>");
				}
			}
			else if (tt.IsInterface)
			{
				interfaces.Add(it);
                WriteElementHeaderDescription(sw, HelpUtils.get_localization("INTERFACE") + " " + HelpUtils.build_name_with_possible_generic(tt, true), HelpUtils.get_summary(HelpUtils.GetDocumentation(tt)));
  				sw.WriteLine("<p>");
  				sw.WriteLine(HelpUtils.get_localization("FOR_SEE_INTERFACE_MEMBERS")+" <a href=\""+name+"Members.html"+"\">"+tt.Namespace+"."+HelpUtils.build_name_with_possible_generic(tt,true)+"</a>.</p>");
			}
			else
			{
				classes.Add(it);
                WriteElementHeaderDescription(sw, HelpUtils.get_localization("CLASS") + " " + HelpUtils.build_name_with_possible_generic(tt, true), HelpUtils.get_summary(HelpUtils.GetDocumentation(tt)));
  				sw.WriteLine("<p>");
  				sw.WriteLine(HelpUtils.get_localization("FOR_SEE_CLASS_MEMBERS")+" <a href=\""+name+"Members.html"+"\">"+tt.Namespace+"."+HelpUtils.build_name_with_possible_generic(tt,true)+"</a>.</p>");
			}
  			
  			
  			Type[] names = GetParentClasses(tt);
  			WriteSyntaxHeader(sw);
  			//sw.WriteLine("<div><b>type</b> "+tt.name+" = <b>class</b>");
  			sw.WriteLine(HelpUtils.get_type_header(tt));
  			sw.WriteLine("</div>");

            if (HelpUtils.can_show_members(tt))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("CLASS_HIERARCHY") + "</h4>");
                sw.WriteLine("<p>");
                int off = 0;
                foreach (Type t in names)
                {
                    WriteSpaces(sw, off);
                    sw.Write(HelpUtils.get_type_html_text(t));
                    sw.Write("<br />");
                    off += 2;
                }
                WriteSpaces(sw, off);
                off += 2;
                sw.Write("<b>" + HelpUtils.build_name_with_possible_generic(tt, true) + "</b>");
                sw.Write("<br />");
                names = GetChildClasses(tt);
                foreach (Type t in names)
                {
                    WriteSpaces(sw, off);
                    sw.Write(HelpUtils.get_type_html_text(t));
                    sw.Write("<br />");
                }
                sw.WriteLine("</p>");
            }
  			string example = HelpUtils.get_example(HelpUtils.GetDocumentation(tt));
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("UNIT"),unit_name+".html",unit_name);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
  			return it;
		}

        private Type[] GetParentClasses(Type t)
        {
            List<Type> types = new List<Type>();
            while (t.BaseType != null)
            {
                types.Add(t.BaseType);
                t = t.BaseType;
            }
            return types.ToArray();
        }

        private Type[] GetChildClasses(Type t)
        {
            Type[] typs = t.Assembly.GetExportedTypes();
            List<Type> types = new List<Type>();
            foreach (Type typ in typs)
            {
                if (typ.BaseType == t)
                    types.Add(typ);
            }
            return types.ToArray();
        }

		private void WriteBanner(StreamWriter sw)
		{
			sw.WriteLine("<html dir=\"LTR\">");
  			sw.WriteLine("<head>");
  			sw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1251\" />");
  			sw.WriteLine("<meta name=\"vs_targetSchema\" content=\"http://schemas.microsoft.com/intellisense/ie5\" />");
  			sw.WriteLine("<title>"+unit_name+"</title>");
  			sw.WriteLine("<xml></xml>");
  			sw.WriteLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"MSDN.css\" />");
  			sw.WriteLine("</head>");
  			sw.WriteLine("<body id=\"bodyID\" class=\"dtBODY\">");
  			sw.WriteLine("<div id=\"nsbanner\">");
  			sw.WriteLine("<div id=\"bannerrow1\">");
  			sw.WriteLine("<table class=\"bannerparthead\" cellspacing=\"0\">");
  			sw.WriteLine("<tr id=\"hdr\">");
  			sw.WriteLine("<td class=\"runninghead\">"+options.CHMBannerText+"</td>");
 	 		sw.WriteLine("<td class=\"product\"></td>");
  			sw.WriteLine("</tr></table></div>");
		}
	}
		
}
