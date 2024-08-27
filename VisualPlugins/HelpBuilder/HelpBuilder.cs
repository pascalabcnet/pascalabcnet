using System;
using System.Windows.Forms;
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
	public class BuildOptions
	{
		private string outputChmFileName;
		private bool generateChm=true;
		private bool showNoCommentedElements=true;
		private string workingDirectory;
		private string chmBannerText;
		
		[System.ComponentModel.DisplayName("Выходной CHM-файл")]
		public string OutputCHMFileName
		{
			get
			{
				return outputChmFileName;
			}
			set
			{
				outputChmFileName = value;
			}
		}
		
		[System.ComponentModel.DisplayName("Рабочий каталог")]
		public string WorkingDirectory
		{
			get
			{
				return workingDirectory;
			}
			set
			{
				workingDirectory = value;
			}
		}
		
		[System.ComponentModel.DisplayName("Генерировать CHM-файл")]
		public bool GenerateCHM
		{
			get
			{
				return generateChm;
			}
			set
			{
				generateChm = value;
			}
		}
		
		[System.ComponentModel.DisplayName("Показывать недокументированные элементы")]
		public bool ShowNoCommentedElements
		{
			get
			{
				return showNoCommentedElements;
			}
			set
			{
				showNoCommentedElements = value;
			}
		}
		
		[System.ComponentModel.DisplayName("Колонтитул")]
		public string CHMBannerText
		{
			get
			{
				return chmBannerText;
			}
			set
			{
				chmBannerText = value;
			}
		}
	}
	
	public class ItemInfo
	{
		public string header;
		public string file_name;
		public ItemInfo(string file_name, string header)
		{
			this.file_name = file_name;
			this.header = header;
		}
	}
	
	class ItemInfoComparer : IComparer<ItemInfo>
	{
		public int Compare(ItemInfo x, ItemInfo y)
		{
			return string.Compare(x.header,y.header,true);
		}
	}
	
	public class UnitElemRec
	{
		public string unit_name;
		public string full_unit_name;
		public List<ItemInfo> classes = new List<ItemInfo>();
		public List<ItemInfo> interfaces = new List<ItemInfo>();
		public List<ItemInfo> rec_enums = new List<ItemInfo>();
		public List<ItemInfo> types = new List<ItemInfo>();
		public List<ItemInfo> procs = new List<ItemInfo>();
		public List<ItemInfo> vars = new List<ItemInfo>();
		public List<ItemInfo> consts = new List<ItemInfo>();
		
		public UnitElemRec(string unit_name, string full_unit_name,List<ItemInfo> classes, List<ItemInfo> interfaces, List<ItemInfo> rec_enums, List<ItemInfo> types, List<ItemInfo> procs, List<ItemInfo> vars, List<ItemInfo> consts)
		{
			this.unit_name = unit_name;
			this.full_unit_name = full_unit_name;
			this.classes.AddRange(classes.ToArray());
			this.interfaces.AddRange(interfaces.ToArray());
			this.rec_enums.AddRange(rec_enums.ToArray());
			this.types.AddRange(types.ToArray());
			this.procs.AddRange(procs.ToArray());
			this.vars.AddRange(vars.ToArray());
			this.consts.AddRange(consts.ToArray());
		}
	}

    public class NamespaceElemRec
    {
        public List<ItemInfo> classes;
        public List<ItemInfo> interfaces;
        public List<ItemInfo> records;
        public List<ItemInfo> enums;
        public List<ItemInfo> delegates;
    }

	public class TypeElemRec
	{
		public List<ItemInfo> props;
		public List<ItemInfo> meths;
		public List<ItemInfo> constrs;
		public List<ItemInfo> fields;
		public List<ItemInfo> events;
		public List<ItemInfo> consts;
		
		public TypeElemRec()
		{
			this.props = new List<ItemInfo>();
			this.fields = new List<ItemInfo>();
			this.meths = new List<ItemInfo>();
			this.events = new List<ItemInfo>();
			this.constrs = new List<ItemInfo>();
			this.consts = new List<ItemInfo>();
		}
		
		public TypeElemRec(List<ItemInfo> props,List<ItemInfo> meths,List<ItemInfo> constrs,List<ItemInfo> fields,
		                   List<ItemInfo> events,List<ItemInfo> consts)
		{
			this.props = props;
			this.meths = meths;
			this.constrs = constrs;
			this.fields = fields;
			this.events = events;
			this.consts = consts;
		}
	}
	
	public class NameInfo
	{
		public string name;
		public string desc;
		
		public NameInfo(string name, string desc)
		{
			this.name = name;
			this.desc = desc;
		}
	}
	
	public delegate void ProcessEvent(string message);
	
	public class HelpBuilder
	{
		List<ItemInfo> classes = new List<ItemInfo>();
		List<ItemInfo> interfaces = new List<ItemInfo>();
		List<ItemInfo> rec_enums = new List<ItemInfo>();
		List<ItemInfo> types = new List<ItemInfo>();
		List<ItemInfo> vars = new List<ItemInfo>();
		List<ItemInfo> procs = new List<ItemInfo>();
		List<ItemInfo> consts = new List<ItemInfo>();
		List<ItemInfo> elems = new List<ItemInfo>();
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
		public FileParser parser = null;
		public event ProcessEvent CompilationProgress;
		
		public HelpBuilder(IVisualEnvironmentCompiler vec)
		{
			this.vec = vec;
			parser = new FileParser(vec);
			HelpUtils.builder = this;
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
			parser.Clear();
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

        public string BuildHelpForAssembly(string[] file_names)
        {
            AssemblyHelpBuilder ahb = new AssemblyHelpBuilder(this.vec);
            ahb.options = this.options;
            return ahb.BuildHelp(typeof(int).Assembly);
        }

		public string BuildHelp(string[] file_names)
		{
            try
            {
                output_dir = Path.Combine(options.WorkingDirectory, "help_" + Path.GetFileNameWithoutExtension(options.OutputCHMFileName));
                foreach (string s in file_names)
                {
                    CompilationProgress(string.Format(PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix + "PARSING_{0}"), Path.GetFileName(s)));
                    if (!parser.Parse(s))
                    {
                        return null;
                    }
                }
                if (!Directory.Exists(output_dir))
                    Directory.CreateDirectory(output_dir);
                copy_files();
                int i = 0;

                foreach (string s in file_names)
                {
                    if (parser.namespaces.Count <= i)
                    {
                        MessageBox.Show(PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix + "PROGRAMM_MUST_HAVE_NAME"), PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix + "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                    parser.set_ns(i++);
                    CompilationProgress(string.Format(PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix + "GENERATING_HTML_{0}"), Path.GetFileName(s)));
                    if (BuildHelp(s) == null)
                        return null;
                    classes.Clear();
                    interfaces.Clear();
                    rec_enums.Clear();
                    types.Clear();
                    procs.Clear();
                    vars.Clear();
                    consts.Clear();
                }
                GenerateChmProjectFiles();
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
		
		public string BuildHelp(string file_name)
		{
			
			string name = Path.ChangeExtension(file_name,".xml");
			if (!File.Exists(name))
				return null;
			
			if (GenerateDoc(name))
			/*if (File.Exists(Path.Combine(output_dir,unit_name+".chm")))
				return Path.Combine(output_dir,unit_name+".chm");
			else
				return index_file_name;*/
			return "";
			return null;
		}
		
		private bool GenerateDoc(string name)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			if (!File.Exists(name))
			{
				MessageBox.Show(PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix+"XML_NOT_FOUND"));
				return false;
			}
  			reader = XmlTextReader.Create(name,settings);
  			reader.Read();
  			reader.ReadStartElement("doc");
  			if (reader.ReadToNextSibling("unit"))
  			{
  				unit_name = reader.GetAttribute("name");
  				unit_desc = reader.ReadString();
  			}
  			else
  			{
  				reader.Close();
  				reader = XmlTextReader.Create(name,settings);
  				reader.ReadStartElement("doc");
  				reader.ReadStartElement("assembly");
  				reader.ReadToNextSibling("name");
  				unit_name = reader.ReadString();
  			}
  			reader.Close();
  			index_file_name = Path.Combine(output_dir,"index.html");
  			full_unit_name = Path.Combine(output_dir,unit_name+".html");
  			SaveIndexFile();
  			
  			//reader.ReadEndElement();
  			//FillTables();
  			
  			SaveUnitMembers(ElemKind.None);
  			
  			
  			foreach (ITypeSynonym ts in parser.main_ns.type_synonims)
  			{
  				ItemInfo it = SaveTypeSynonim(ts);
  				type_elems.Add(it,new TypeElemRec());
  			}
  			ICommonTypeNode[] _types = parser.GetTypes();
  			foreach (ICommonTypeNode t in _types)
  			{
  				if (!HelpUtils.can_write(t))
  					continue;
  				ItemInfo it = SaveClass(t);
  				type_elems.Add(it,new TypeElemRec());
  			}
			
  			if (_types.Length > 0 || parser.main_ns.type_synonims.Length > 0)
  			{
  				SaveUnitMembers(ElemKind.Type);
  			}
  			
  			ICommonTypeNode[] _classes = parser.GetClasses();
  			if (_classes.Length > 0)
  			{
  				SaveUnitMembers(ElemKind.Class);
  			}
  			foreach (ICommonTypeNode t in _classes)
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
  				ICommonPropertyNode[] _props = parser.GetNonPrivateProperties(t);
  				if (_props.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Property);
  				}
  				foreach (ICommonPropertyNode p in t.properties)
  				{
  					if (!HelpUtils.can_write(p))
  						continue;
  					if (p.field_access_level != field_access_level.fal_private)
  					SaveProperty(p,props,it);
  				}
  				List<ItemInfo> meths = new List<ItemInfo>();
  				List<ItemInfo> constrs = new List<ItemInfo>();
  				ICommonMethodNode[] _meths = parser.GetNonPrivateMethods(t);
  				if (_meths.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Method);
  				}
  				_meths = parser.GetConstructors(t);
  				if (_meths.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Constructor);
  				}
  				foreach (ICommonMethodNode m in t.methods)
  				{
  					if (!HelpUtils.can_write(m))
  						continue;
  					if (m.field_access_level != field_access_level.fal_private)
  					SaveMethod(m,meths,constrs,it);
  					if (m.is_constructor)
  					{
  						if (parser.is_overloaded_constructor(m))
  							SaveConstructorOverloadsList(parser.GetOverloadedConstructors(t));
  					}
  					else
  					if (parser.is_overload(m,t))
  						SaveOverloadsList(parser.GetOverloadsList(t,m));
  				}
  				List<ItemInfo> fields = new List<ItemInfo>();
  				ICommonClassFieldNode[] _fields = parser.GetNonPrivateFields(t);
  				if (_fields.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Field);
  				}
  				foreach (ICommonClassFieldNode f in t.fields)
  				{
  					if (!HelpUtils.can_write(f))
  						continue;
  					if (f.field_access_level != field_access_level.fal_private)
  					SaveField(f,fields,it);
  				}
  				List<ItemInfo> events = new List<ItemInfo>();
  				ICommonEventNode[] _events = parser.GetNonPrivateEvents(t);
  				if (_events.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Event);
  				}
  				foreach (ICommonEventNode e in t.events)
  				{
  					if (!HelpUtils.can_write(e))
  						continue;
  					if (e.field_access_level != field_access_level.fal_private)
  					SaveEvent(e,events,it);
  				}
  				List<ItemInfo> consts = new List<ItemInfo>();
  				IClassConstantDefinitionNode[] _consts = parser.GetNonPrivateClassConstants(t);
  				if (_consts.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.ClassConstant);
  				}
  				foreach (IClassConstantDefinitionNode c in t.constants)
  				{
  					if (!HelpUtils.can_write(c))
  						continue;
  					if (c.field_access_level != field_access_level.fal_private)
  					SaveClassConstant(c,consts,it);
  				}
  				
  				type_elems.Add(it,new TypeElemRec(props,meths,constrs,fields,events,consts));
  			}
  			
  			//interfejsy
  			_classes = parser.GetInterfaces();
  			if (_classes.Length > 0)
  			{
  				SaveUnitMembers(ElemKind.Interface);
  			}
  			foreach (ICommonTypeNode t in _classes)
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
  				ICommonPropertyNode[] _props = parser.GetNonPrivateProperties(t);
  				if (_props.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Property);
  				}
  				foreach (ICommonPropertyNode p in t.properties)
  				{
  					if (!HelpUtils.can_write(p))
  						continue;
  					if (p.field_access_level != field_access_level.fal_private)
  					SaveProperty(p,props,it);
  				}
  				List<ItemInfo> meths = new List<ItemInfo>();
  				List<ItemInfo> constrs = new List<ItemInfo>();
  				ICommonMethodNode[] _meths = parser.GetNonPrivateMethods(t);
  				if (_meths.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Method);
  				}
  				foreach (ICommonMethodNode m in t.methods)
  				{
  					if (!HelpUtils.can_write(m))
  						continue;
  					if (m.field_access_level != field_access_level.fal_private)
  					SaveMethod(m,meths,constrs,it);
  					if (parser.is_overload(m,t))
  						SaveOverloadsList(parser.GetOverloadsList(t,m));
  				}
  				List<ItemInfo> events = new List<ItemInfo>();
  				ICommonEventNode[] _events = parser.GetNonPrivateEvents(t);
  				if (_events.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Event);
  				}
  				foreach (ICommonEventNode e in t.events)
  				{
  					if (!HelpUtils.can_write(e))
  						continue;
  					if (e.field_access_level != field_access_level.fal_private)
  					SaveEvent(e,events,it);
  				}
  				type_elems.Add(it,new TypeElemRec(props,meths,constrs,new List<ItemInfo>(),events,new List<ItemInfo>()));
  			}
  			
  			//zapisi perechislenija
  			ICommonTypeNode[] _rec_enums = parser.GetEnumsRecords();
  			if (_rec_enums.Length > 0)
  			{
  				SaveUnitMembers(ElemKind.EnumRecord);
  			}
  			foreach (ICommonTypeNode t in _rec_enums)
  			{
    			if (!HelpUtils.can_write(t))
  					continue;
  				ItemInfo it = SaveClass(t);
  				if (!HelpUtils.can_show_members(t))
  				{
  					type_elems.Add(it,new TypeElemRec());
  					continue;
  				}
  				ICommonPropertyNode[] _props = parser.GetNonPrivateProperties(t);
  				if (_props.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Property);
  				}
  				List<ItemInfo> props = new List<ItemInfo>();
  				foreach (ICommonPropertyNode p in t.properties)
  				{
  					if (!HelpUtils.can_write(p))
  						continue;
  					if (p.field_access_level != field_access_level.fal_private)
  					SaveProperty(p,props,it);
  				}
  				List<ItemInfo> meths = new List<ItemInfo>();
  				List<ItemInfo> constrs = new List<ItemInfo>();
  				ICommonMethodNode[] _meths = parser.GetNonPrivateMethods(t);
  				if (_meths.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Method);
  				}
  				_meths = parser.GetConstructors(t);
  				if (_meths.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Constructor);
  				}
  				foreach (ICommonMethodNode m in t.methods)
  				{
  					if (!HelpUtils.can_write(m))
  						continue;
  					if (m.field_access_level != field_access_level.fal_private)
  					SaveMethod(m,meths,constrs,it);
  					if (m.is_constructor)
  					{
  						if (parser.is_overloaded_constructor(m))
  							SaveConstructorOverloadsList(parser.GetOverloadedConstructors(t));
  					}
  					else
  					if (parser.is_overload(m,t))
  						SaveOverloadsList(parser.GetOverloadsList(t,m));
  				}
  				ICommonClassFieldNode[] _fields = parser.GetNonPrivateFields(t);
  				if (_fields.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Field);
  				}
  				List<ItemInfo> fields = new List<ItemInfo>();
  				foreach (ICommonClassFieldNode f in t.fields)
  				{
  					if (!HelpUtils.can_write(f))
  						continue;
  					if (f.field_access_level != field_access_level.fal_private)
  					SaveField(f,fields,it);
  				}
  				ICommonEventNode[] _events = parser.GetNonPrivateEvents(t);
  				if (_events.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.Event);
  				}
  				List<ItemInfo> events = new List<ItemInfo>();
  				foreach (ICommonEventNode e in t.events)
  				{
  					if (!HelpUtils.can_write(e))
  						continue;
  					if (e.field_access_level != field_access_level.fal_private)
  					SaveEvent(e,events,it);
  				}
  				IClassConstantDefinitionNode[] _consts = parser.GetNonPrivateClassConstants(t);
  				if (_consts.Length > 0)
  				{
  					SaveMembersToHTML(t,ElemKind.ClassConstant);
  				}
  				List<ItemInfo> consts = new List<ItemInfo>();
  				foreach (IClassConstantDefinitionNode c in t.constants)
  				{
  					if (!HelpUtils.can_write(c))
  						continue;
  					if (c.field_access_level != field_access_level.fal_private)
  					SaveClassConstant(c,consts,it);
  				}
  				
  				type_elems.Add(it,new TypeElemRec(props,meths,constrs,fields,events,consts));
  			}
  			
  			ICommonNamespaceFunctionNode[] _funcs = parser.GetFunctions();
  			if (_funcs.Length > 0)
  			{
  				SaveUnitMembers(ElemKind.Subroutine);
  			}
  			foreach (ICommonNamespaceFunctionNode f in _funcs)
  			{
  				if (HelpUtils.can_write(f))
  				{
  					SaveGlobalFunction(f);
  					if (parser.is_overload(f))
  						SaveOverloadsList(parser.GetOverloadsList(parser.main_ns,f));
  				}
  			}
  			ICommonNamespaceVariableNode[] _vars = parser.GetVariables();
  			if (_vars.Length > 0)
  			{
  				SaveUnitMembers(ElemKind.Variable);
  			}
  			foreach (ICommonNamespaceVariableNode v in _vars)
  			{
  				if (HelpUtils.can_write(v))
  				SaveGlobalVariable(v);
  			}
  			INamespaceConstantDefinitionNode[] _ns_consts = parser.GetNamespaceConstants();
  			if (_ns_consts.Length > 0)
  			{
  				SaveUnitMembers(ElemKind.Const);
  			}
  			foreach (INamespaceConstantDefinitionNode c in _ns_consts)
  			{
  				if (HelpUtils.can_write(c))
  				SaveGlobalConstant(c);
  			}
  			foreach (ICommonTypeNode t in parser.main_ns.types)
  			{
  				if (HelpUtils.can_write(t))
  				SaveMembersToHTML(t, ElemKind.None);
  			}
  			units.Add(new UnitElemRec(unit_name,full_unit_name,classes,interfaces,rec_enums,types,procs,vars,this.consts));
  			//GenerateChmProjectFiles();
  			//BuildChm();
  			return true;
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
			//klassy
			classes.Sort(new ItemInfoComparer());
			if (classes.Count > 0)
			{					
				for (int i=0; i<classes.Count; i++)
				{
					
					TypeElemRec ter = type_elems[classes[i]];
					sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
					sw.WriteLine("<param name=\"Name\" value=\""+classes[i].header+"\">");
					sw.WriteLine("<param name=\"Local\" value=\""+classes[i].file_name+"\">");
					sw.WriteLine("</OBJECT>");
					sw.WriteLine("</LI>");
					ter.constrs.Sort(new ItemInfoComparer());
					if (ter.constrs.Count > 0)
					{
						for (int j=0; j<ter.constrs.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.constrs[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.constrs[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}
					
					ter.meths.Sort(new ItemInfoComparer());
					if (ter.meths.Count > 0)
					{
						for (int j=0; j<ter.meths.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.meths[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.meths[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}
					ter.props.Sort(new ItemInfoComparer());
					if (ter.props.Count > 0)
					{
						for (int j=0; j<ter.props.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.props[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.props[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}
					
					ter.fields.Sort(new ItemInfoComparer());
					if (ter.fields.Count > 0)
					{
						for (int j=0; j<ter.fields.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.fields[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.fields[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}
					ter.events.Sort(new ItemInfoComparer());
					if (ter.events.Count > 0)
					{
						for (int j=0; j<ter.events.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.events[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.events[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}
					ter.consts.Sort(new ItemInfoComparer());
					if (ter.consts.Count > 0)
					{
						for (int j=0; j<ter.consts.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.consts[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.consts[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}
				}
			}
			
			interfaces.Sort(new ItemInfoComparer());
			if (interfaces.Count > 0)
			{					
				for (int i=0; i<interfaces.Count; i++)
				{
					TypeElemRec ter = type_elems[interfaces[i]];
					sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
					sw.WriteLine("<param name=\"Name\" value=\""+interfaces[i].header+"\">");
					sw.WriteLine("<param name=\"Local\" value=\""+interfaces[i].file_name+"\">");
					sw.WriteLine("</OBJECT>");
					sw.WriteLine("</LI>");
					
					ter.meths.Sort(new ItemInfoComparer());
					if (ter.meths.Count > 0)
					{
						for (int j=0; j<ter.meths.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.meths[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.meths[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}
					ter.props.Sort(new ItemInfoComparer());
					if (ter.props.Count > 0)
					{
						for (int j=0; j<ter.props.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.props[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.props[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}

					ter.events.Sort(new ItemInfoComparer());
					if (ter.events.Count > 0)
					{
						for (int j=0; j<ter.events.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.events[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.events[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}
				}
			}
			
			//records, enums
			rec_enums.Sort(new ItemInfoComparer());
			if (rec_enums.Count > 0)
			{					
				for (int i=0; i<rec_enums.Count; i++)
				{
					
					TypeElemRec ter = type_elems[rec_enums[i]];
					sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
					sw.WriteLine("<param name=\"Name\" value=\""+rec_enums[i].header+"\">");
					sw.WriteLine("<param name=\"Local\" value=\""+rec_enums[i].file_name+"\">");
					sw.WriteLine("</OBJECT>");
					sw.WriteLine("</LI>");
					ter.constrs.Sort(new ItemInfoComparer());
					if (ter.constrs.Count > 0)
					{
						for (int j=0; j<ter.constrs.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.constrs[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.constrs[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}
					
					ter.meths.Sort(new ItemInfoComparer());
					if (ter.meths.Count > 0)
					{
						for (int j=0; j<ter.meths.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.meths[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.meths[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}
					ter.props.Sort(new ItemInfoComparer());
					if (ter.props.Count > 0)
					{
						for (int j=0; j<ter.props.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.props[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.props[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}
					
					ter.fields.Sort(new ItemInfoComparer());
					if (ter.fields.Count > 0)
					{
						for (int j=0; j<ter.fields.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.fields[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.fields[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}
					ter.events.Sort(new ItemInfoComparer());
					if (ter.events.Count > 0)
					{
						for (int j=0; j<ter.events.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.events[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.events[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}
					ter.consts.Sort(new ItemInfoComparer());
					if (ter.consts.Count > 0)
					{
						for (int j=0; j<ter.consts.Count; j++)
						{
							sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
							sw.WriteLine("<param name=\"Name\" value=\""+ter.consts[j].header+"\">");
							sw.WriteLine("<param name=\"Local\" value=\""+ter.consts[j].file_name+"\">");
							sw.WriteLine("</OBJECT>");
							sw.WriteLine("</LI>");
						}
					}
				}
			}
			
			//types
			types.Sort(new ItemInfoComparer());
			if (types.Count > 0)
			{					
				for (int i=0; i<types.Count; i++)
				{
					
					TypeElemRec ter = type_elems[types[i]];
					sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
					sw.WriteLine("<param name=\"Name\" value=\""+types[i].header+"\">");
					sw.WriteLine("<param name=\"Local\" value=\""+types[i].file_name+"\">");
					sw.WriteLine("</OBJECT>");
					sw.WriteLine("</LI>");
				}
			}
			procs.Sort(new ItemInfoComparer());
			if (procs.Count > 0)
			{
				for (int i=0; i<procs.Count; i++)
				{
					sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
					sw.WriteLine("<param name=\"Name\" value=\""+procs[i].header+"\">");
					sw.WriteLine("<param name=\"Local\" value=\""+procs[i].file_name+"\">");
					sw.WriteLine("</OBJECT>");
					sw.WriteLine("</LI>");
				}
			}
			vars.Sort(new ItemInfoComparer());
			if (vars.Count > 0)
			{
				for (int i=0; i<vars.Count; i++)
				{
					sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
					sw.WriteLine("<param name=\"Name\" value=\""+vars[i].header+"\">");
					sw.WriteLine("<param name=\"Local\" value=\""+vars[i].file_name+"\">");
					sw.WriteLine("</OBJECT>");
					sw.WriteLine("</LI>");
				}
			}
			consts.Sort(new ItemInfoComparer());
			if (consts.Count > 0)
			{
				for (int i=0; i<consts.Count; i++)
				{
					sw.WriteLine(" <LI><OBJECT type=\"text/sitemap\">");
					sw.WriteLine("<param name=\"Name\" value=\""+consts[i].header+"\">");
					sw.WriteLine("<param name=\"Local\" value=\""+consts[i].file_name+"\">");
					sw.WriteLine("</OBJECT>");
					sw.WriteLine("</LI>");
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
		
		private void SaveUnitMembers(ElemKind kind)
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
  			ICommonTypeNode[] _classes = parser.GetClasses();
            if (_classes.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Class))
            {
                WriteTableHeader(sw, HelpUtils.get_localization("CLASSES"));
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonTypeNode t in _classes)
                {
                    if (!HelpUtils.can_write(t))
                        continue;
                    sw.WriteLine("<tr valign=\"top\">");
                    sw.WriteLine("<td width=\"50%\">");
                    sw.WriteLine("<img src=\"pubclass.gif\" />");
                    sw.WriteLine("<a href=\"" + HelpUtils.get_type_name(t) + ".html" + "\">" + HelpUtils.build_name_with_possible_generic(t, true) + "</a>");
                    sw.WriteLine("</td>");
                    sw.WriteLine("<td width=\"50%\">" + HelpUtils.get_summary(t.Documentation) + "</td>");
                    sw.WriteLine("</tr>");
                }
                WriteFooterTable(sw);
            }
  			
  			ICommonTypeNode[] _interfaces = parser.GetInterfaces();
            if (_interfaces.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Interface))
            {
                WriteTableHeader(sw, HelpUtils.get_localization("INTERFACES"));
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonTypeNode t in _interfaces)
                {
                    if (!HelpUtils.can_write(t))
                        continue;
                    sw.WriteLine("<tr valign=\"top\">");
                    sw.WriteLine("<td width=\"50%\">");
                    sw.WriteLine("<img src=\"pubinterface.gif\" />");
                    sw.WriteLine("<a href=\"" + HelpUtils.get_type_name(t) + ".html" + "\">" + HelpUtils.build_name_with_possible_generic(t, true) + "</a>");
                    sw.WriteLine("</td>");
                    sw.WriteLine("<td width=\"50%\">" + HelpUtils.get_summary(t.Documentation) + "</td>");
                    sw.WriteLine("</tr>");
                }
                WriteFooterTable(sw);
            }
  			
  			ICommonTypeNode[] _enum_recs = parser.GetEnumsRecords();
            if (_enum_recs.Length > 0 && (kind == ElemKind.None || kind == ElemKind.EnumRecord))
            {
                WriteTableHeader(sw, HelpUtils.get_localization("ENUMS_RECORDS"));
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonTypeNode t in _enum_recs)
                {
                    if (!HelpUtils.can_write(t))
                        continue;
                    sw.WriteLine("<tr valign=\"top\">");
                    sw.WriteLine("<td width=\"50%\">");

                    if (t.IsEnum)
                        sw.WriteLine("<img src=\"pubenum.gif\" />");
                    else if (t.is_value_type)
                        sw.WriteLine("<img src=\"pubstructure.gif\" />");
                    sw.WriteLine("<a href=\"" + HelpUtils.get_type_name(t) + ".html" + "\">" + HelpUtils.build_name_with_possible_generic(t, true) + "</a>");
                    sw.WriteLine("</td>");
                    sw.WriteLine("<td width=\"50%\">" + HelpUtils.get_summary(t.Documentation) + "</td>");
                    sw.WriteLine("</tr>");
                }
                WriteFooterTable(sw);
            }
  			
  			ICommonTypeNode[] _types = parser.GetTypes();
  			if ((_types.Length > 0 || parser.main_ns.type_synonims.Length > 0) && (kind == ElemKind.None || kind == ElemKind.Type))
  			{
  				WriteTableHeader(sw,HelpUtils.get_localization("TYPES"));
  				WriteNameDescriptionHeaderForTable(sw);
  				if (_types.Length > 0)
  				{
  			
  					foreach (ICommonTypeNode t in _types)
  					{
  						if (!HelpUtils.can_write(t))
  							continue;
  						sw.WriteLine("<tr valign=\"top\">");
    					sw.WriteLine("<td width=\"50%\">");
    					sw.WriteLine("<a href=\""+HelpUtils.get_type_name(t)+".html"+"\">"+t.name+"</a>");
    					sw.WriteLine("</td>");
    					sw.WriteLine("<td width=\"50%\">"+HelpUtils.get_summary(t.Documentation)+"</td>");
    					sw.WriteLine("</tr>");
  					}
  				}
  				//type synonims
  				foreach (ITypeSynonym ts in parser.main_ns.type_synonims)
  				{
  					sw.WriteLine("<tr valign=\"top\">");
    				sw.WriteLine("<td width=\"50%\">");
    				sw.WriteLine("<a href=\""+HelpUtils.get_type_name(ts)+".html"+"\">"+ts.name+"</a>");
    				sw.WriteLine("</td>");
    				sw.WriteLine("<td width=\"50%\">"+HelpUtils.get_summary(ts.Documentation)+"</td>");
    				sw.WriteLine("</tr>");
  				}
  				WriteFooterTable(sw);
  			}
  //procedury
  			ICommonNamespaceFunctionNode[] funcs = parser.GetFunctions();
            if (funcs.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Subroutine))
            {
                WriteTableHeader(sw, HelpUtils.get_localization("SUBPROGRAMMS"));
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonNamespaceFunctionNode f in funcs)
                {
                    if (!HelpUtils.can_write(f))
                        continue;
                    int ind = parser.GetMethodIndex(f);
                    bool overload = parser.is_overload(f);
                    if (overload && ind > 1)
                        continue;
                    sw.WriteLine("<tr valign=\"top\">");
                    sw.WriteLine("<td width=\"50%\">");
                    sw.WriteLine("<img src=\"pubmethod.gif\" />");
                    if (!overload)
                        sw.WriteLine("<a href=\"" + HelpUtils.get_meth_name(f) + ".html" + "\">" + f.name + "</a>");
                    else
                        sw.WriteLine("<a href=\"" + HelpUtils.get_meth_name(f) + "$overloads" + ".html" + "\">" + f.name + "</a>");
                    sw.WriteLine("</td>");
                    sw.Write("<td width=\"50%\">");
                    if (parser.is_overload(f))
                        sw.Write(HelpUtils.get_localization("OVERLOADED") + " ");
                    sw.Write(HelpUtils.get_summary(f.Documentation));
                    sw.WriteLine("</td></tr>");
                }

                WriteFooterTable(sw);
            }
  			
  			ICommonNamespaceVariableNode[] vars = parser.GetVariables();
            if (vars.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Variable))
            {
                //peremennye
                WriteTableHeader(sw, HelpUtils.get_localization("VARIABLES"));
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonNamespaceVariableNode v in vars)
                {
                    if (!HelpUtils.can_write(v))
                        continue;
                    sw.WriteLine("<tr valign=\"top\">");
                    sw.WriteLine("<td width=\"50%\">");
                    sw.WriteLine("<img src=\"Variable.gif\" />");
                    sw.WriteLine("<a href=\"" + HelpUtils.get_var_name(v) + ".html" + "\">" + v.name + "</a>");
                    sw.WriteLine("</td>");
                    sw.WriteLine("<td width=\"50%\">" + HelpUtils.get_summary(v.Documentation) + "</td>");
                    sw.WriteLine("</tr>");
                }

                WriteFooterTable(sw);
            }
  			
  			
  			//konstanty
  			INamespaceConstantDefinitionNode[] consts = parser.GetNamespaceConstants();
  			if (consts.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Const))
  			{
  			WriteTableHeader(sw,HelpUtils.get_localization("CONSTANTS"));
  			WriteNameDescriptionHeaderForTable(sw);
  			foreach (INamespaceConstantDefinitionNode c in consts)
  			{
  				if (!HelpUtils.can_write(c))
  					continue;
  				sw.WriteLine("<tr valign=\"top\">");
  				sw.WriteLine("<td width=\"50%\">");
  				sw.WriteLine("<img src=\"Constant.gif\" />");
  				sw.WriteLine("<a href=\""+HelpUtils.get_const_name(c)+".html"+"\">"+c.name+"</a>");
    			sw.WriteLine("</td>");
    			sw.WriteLine("<td width=\"50%\">"+HelpUtils.get_summary(c.Documentation)+"</td>");
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
		
		private void SaveOverloadsList(List<ICommonNamespaceFunctionNode> meths)
		{
			string name = HelpUtils.get_meth_name(meths[0])+"$overloads";
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			WriteBanner(sw);
			sw.WriteLine("<div id=\"TitleRow\">");
			if (meths[0].return_value_type == null)
			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("PROCEDURE")+" "+meths[0].name+"</h1>");
			else
			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("FUNCTION")+" "+meths[0].name+"</h1>");
  			sw.WriteLine("</div></div>");
  			WriteTableHeader(sw,HelpUtils.get_localization("OVERLOADED_SUBPROGRAMMS"));
  			//sw.WriteLine("<tr VALIGN=\"top\">");
  			WriteNameDescriptionHeaderForTable(sw);
  			foreach (ICommonNamespaceFunctionNode meth in meths)
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
		
		private void SaveConstructorOverloadsList(List<ICommonMethodNode> meths)
		{
			string name = HelpUtils.get_meth_name(meths[0])+"$overloads";
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			WriteBanner(sw);
			sw.WriteLine("<div id=\"TitleRow\">");
			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("CONSTRUCTOR")+" "+meths[0].name+"</h1>");
  			sw.WriteLine("</div></div>");
  			WriteTableHeader(sw,HelpUtils.get_localization("OVERLOADED_CONSTRUCTORS"));
  			//sw.WriteLine("<tr VALIGN=\"top\">");
  			WriteNameDescriptionHeaderForTable(sw);
  			foreach (ICommonMethodNode meth in meths)
  			{
  				if (!HelpUtils.can_write(meth))
  					continue;
  				sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"pubmethod.gif\"></img>");
  				sw.Write("<a href=\""+HelpUtils.get_meth_name(meth)+HelpUtils.possible_overload_constructor_symbol(meth)+".html\">"+HelpUtils.get_simple_meth_header(meth)+"</a>");
  				sw.Write("</td>");
  				sw.Write("<td width=\"50%\">");
  				sw.Write(HelpUtils.get_summary(meth.Documentation));
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
		
		private void SaveMembersToHTML(ICommonTypeNode t, ElemKind kind)
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
			else if (t.is_value_type)
			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("RECORD_MEMBERS")+" "+name+"</h1>");
			else if (t.IsInterface)
			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("INTERFACE_MEMBERS")+" "+name+"</h1>");
			else if (t.is_class)
			sw.WriteLine("<h1 class=\"dtH1\">"+HelpUtils.get_localization("CLASS_MEMBERS")+" "+name+"</h1>");
  			sw.WriteLine("</div></div>");
  			sw.WriteLine("<div id=\"nstext\">");
  			sw.WriteLine("<p><a href=\""+name+".html"+"\">"+ HelpUtils.get_localization("PREVIEW")+" "+t.name+"</a></p>");
  			ICommonMethodNode[] constrs = parser.GetConstructors(t);
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
  					sw.Write(HelpUtils.get_summary(constrs[0].Documentation));
  				else
  					sw.Write(HelpUtils.get_localization("OVERLOADED_CONSTRUCTOR")+" "+t.name);
  				sw.WriteLine("</td>");
  				sw.WriteLine("</tr></table></div>");
  			}
  			
  			
  			ICommonPropertyNode[] props = parser.GetProperties(t, field_access_level.fal_public);
            if (props.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Property))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PUBLIC_PROPERTIES") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonPropertyNode prop in props)
                {
                    if (!HelpUtils.can_write(prop))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"pubproperty.gif\"></img>");
                    if (prop.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_prop_name(prop) + ".html\">" + prop.name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(prop.Documentation);
                    sw.Write(doc);
                    if (prop.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(prop.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			ICommonMethodNode[] meths = parser.GetMethods(t, field_access_level.fal_public);
            if (meths.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Method))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PUBLIC_METHODS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonMethodNode meth in meths)
                {
                    if (!HelpUtils.can_write(meth))
                        continue;
                    int ind = parser.GetMethodIndex(meth, t);
                    if (parser.is_overload(meth, t) && ind > 1)
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"pubmethod.gif\"></img>");
                    if (meth.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    if (!parser.is_overload(meth, t))
                        sw.Write("<a href=\"" + HelpUtils.get_meth_name(meth) + ".html\">" + meth.name + "</a>");
                    else
                    {
                        sw.Write("<a href=\"" + HelpUtils.get_meth_name(meth) + "$overloads" + ".html\">" + meth.name + "</a>");
                    }
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    if (parser.is_overload(meth, t))
                        sw.Write(HelpUtils.get_localization("OVERLOADED") + " ");
                    string doc = HelpUtils.get_summary(meth.Documentation);
                    sw.Write(doc);
                    if (meth.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(meth.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			ICommonClassFieldNode[] fields = parser.GetFields(t, field_access_level.fal_public);
            if (fields.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Field))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PUBLIC_FIELDS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonClassFieldNode fld in fields)
                {
                    if (!HelpUtils.can_write(fld))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"pubfield.gif\"></img>");
                    if (fld.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_field_name(fld) + ".html\">" + fld.name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(fld.Documentation);
                    sw.Write(doc);
                    if (fld.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(fld.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			ICommonEventNode[] events = parser.GetEvents(t, field_access_level.fal_public);
            if (events.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Event))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PUBLIC_EVENTS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonEventNode e in events)
                {
                    if (!HelpUtils.can_write(e))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"pubevent.gif\"></img>");
                    if (e.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_event_name(e) + ".html\">" + e.Name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(e.Documentation);
                    sw.Write(doc);
                    if (e.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(e.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			IClassConstantDefinitionNode[] consts = parser.GetConstants(t, field_access_level.fal_public);
            if (consts.Length > 0 && (kind == ElemKind.None || kind == ElemKind.ClassConstant))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PUBLIC_CONSTANTS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (IClassConstantDefinitionNode c in consts)
                {
                    if (!HelpUtils.can_write(c))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"Constant.gif\"></img>");
                    if (c.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_const_name(c) + ".html\">" + c.name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(c.Documentation);
                    sw.Write(doc);
                    if (c.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(c.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			//internal
  			props = parser.GetProperties(t, field_access_level.fal_internal);
            if (props.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Property))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("INTERNAL_PROPERTIES") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonPropertyNode prop in props)
                {
                    if (!HelpUtils.can_write(prop))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"intproperty.gif\"></img>");
                    if (prop.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_prop_name(prop) + ".html\">" + prop.name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(prop.Documentation);
                    sw.Write(doc);
                    if (prop.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(prop.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			meths = parser.GetMethods(t, field_access_level.fal_internal);
            if (meths.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Method))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("INTERNAL_METHODS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonMethodNode meth in meths)
                {
                    if (!HelpUtils.can_write(meth))
                        continue;
                    int ind = parser.GetMethodIndex(meth, t);
                    if (parser.is_overload(meth, t) && ind > 1)
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"intmethod.gif\"></img>");
                    if (meth.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    if (!parser.is_overload(meth, t))
                        sw.Write("<a href=\"" + HelpUtils.get_meth_name(meth) + ".html\">" + meth.name + "</a>");
                    else
                    {
                        sw.Write("<a href=\"" + HelpUtils.get_meth_name(meth) + "$overloads" + ".html\">" + meth.name + "</a>");
                    }
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    if (parser.is_overload(meth, t))
                        sw.Write(HelpUtils.get_localization("OVERLOADED") + " ");
                    string doc = HelpUtils.get_summary(meth.Documentation);
                    sw.Write(doc);
                    if (meth.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(meth.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			fields = parser.GetFields(t, field_access_level.fal_internal);
            if (fields.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Field))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("INTERNAL_FIELDS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonClassFieldNode fld in fields)
                {
                    if (!HelpUtils.can_write(fld))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"intfield.gif\"></img>");
                    if (fld.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_field_name(fld) + ".html\">" + fld.name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(fld.Documentation);
                    sw.Write(doc);
                    if (fld.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(fld.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			events = parser.GetEvents(t, field_access_level.fal_internal);
            if (events.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Event))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("INTERNAL_EVENTS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonEventNode e in events)
                {
                    if (!HelpUtils.can_write(e))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"intevent.gif\"></img>");
                    if (e.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_event_name(e) + ".html\">" + e.Name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(e.Documentation);
                    sw.Write(doc);
                    if (e.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(e.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			consts = parser.GetConstants(t, field_access_level.fal_internal);
            if (consts.Length > 0 && (kind == ElemKind.None || kind == ElemKind.ClassConstant))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("INTERNAL_CONSTANTS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (IClassConstantDefinitionNode c in consts)
                {
                    if (!HelpUtils.can_write(c))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"intconstant.gif\"></img>");
                    if (c.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_const_name(c) + ".html\">" + c.name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(c.Documentation);
                    sw.Write(doc);
                    if (c.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(c.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			//protected
  			props = parser.GetProperties(t, field_access_level.fal_protected);
            if (props.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Property))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PROTECTED_FIELDS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonPropertyNode prop in props)
                {
                    if (!HelpUtils.can_write(prop))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"protproperty.gif\"></img>");
                    if (prop.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_prop_name(prop) + ".html\">" + prop.name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(prop.Documentation);
                    sw.Write(doc);
                    if (prop.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(prop.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			meths = parser.GetMethods(t, field_access_level.fal_protected);
            if (meths.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Method))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PROTECTED_METHODS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonMethodNode meth in meths)
                {
                    if (!HelpUtils.can_write(meth))
                        continue;
                    int ind = parser.GetMethodIndex(meth, t);
                    if (parser.is_overload(meth, t) && ind > 1)
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"protmethod.gif\"></img>");
                    if (meth.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    if (!parser.is_overload(meth, t))
                        sw.Write("<a href=\"" + HelpUtils.get_meth_name(meth) + ".html\">" + meth.name + "</a>");
                    else
                    {
                        sw.Write("<a href=\"" + HelpUtils.get_meth_name(meth) + "$overloads" + ".html\">" + meth.name + "</a>");
                    }
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    if (parser.is_overload(meth, t))
                        sw.Write(HelpUtils.get_localization("OVERLOADED") + " ");
                    string doc = HelpUtils.get_summary(meth.Documentation);
                    sw.Write(doc);
                    if (meth.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(meth.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			fields = parser.GetFields(t, field_access_level.fal_protected);
            if (fields.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Field))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PROTECTED_FIELDS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonClassFieldNode fld in fields)
                {
                    if (!HelpUtils.can_write(fld))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"protfield.gif\"></img>");
                    if (fld.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_field_name(fld) + ".html\">" + fld.name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(fld.Documentation);
                    sw.Write(doc);
                    if (fld.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(fld.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			events = parser.GetEvents(t, field_access_level.fal_protected);
            if (events.Length > 0 && (kind == ElemKind.None || kind == ElemKind.Event))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PROTECTED_EVENTS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (ICommonEventNode e in events)
                {
                    if (!HelpUtils.can_write(e))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"protevent.gif\"></img>");
                    if (e.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_event_name(e) + ".html\">" + e.Name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(e.Documentation);
                    sw.Write(doc);
                    if (e.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(e.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			consts = parser.GetConstants(t, field_access_level.fal_protected);
            if (consts.Length > 0 && (kind == ElemKind.None || kind == ElemKind.ClassConstant))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("PROTECTED_CONSTANTS") + "</h4>");
                sw.WriteLine("<div class=\"tablediv\">");
                sw.WriteLine("<table class=\"dtTABLE\" cellspacing=\"2\">");
                WriteNameDescriptionHeaderForTable(sw);
                foreach (IClassConstantDefinitionNode c in consts)
                {
                    if (!HelpUtils.can_write(c))
                        continue;
                    sw.Write("<tr VALIGN=\"top\"><td width=\"50%\"><img src=\"protconstant.gif\"></img>");
                    if (c.polymorphic_state == polymorphic_state.ps_static)
                        sw.Write("<img src=\"static.gif\"></img>");
                    sw.Write("<a href=\"" + HelpUtils.get_const_name(c) + ".html\">" + c.name + "</a>");
                    sw.Write("</td>");
                    sw.Write("<td width=\"50%\">");
                    string doc = HelpUtils.get_summary(c.Documentation);
                    sw.Write(doc);
                    if (c.comperehensive_type != t)
                        sw.Write((doc == HelpUtils.HTMLSpace ? "(" : " (") + HelpUtils.get_localization("INHERITED_FROM") + " " + HelpUtils.get_type_html_text(c.comperehensive_type) + ")");
                    sw.WriteLine("</td></tr>");
                }
                sw.WriteLine("</table>");
                sw.WriteLine("</div>");
            }
  			
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
		}

		private void SaveProperty(ICommonPropertyNode p, List<ItemInfo> props, ItemInfo decl_type)
		{
			string name = HelpUtils.get_prop_name(p);
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			props.Add(new ItemInfo(name+".html",p.name));
			WriteBanner(sw);
			WriteElementHeaderDescription(sw,HelpUtils.get_localization("PROPERTY")+" "+p.name,HelpUtils.get_summary(p.Documentation));
  			WriteSyntaxHeader(sw);
  			sw.WriteLine(HelpUtils.get_property_header(p));
  			sw.WriteLine("</div>");
  			string example = HelpUtils.get_example(p.Documentation);
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("TYPE"),decl_type.file_name,decl_type.header);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
		}
		
		private void SaveField(ICommonClassFieldNode f, List<ItemInfo> fields, ItemInfo decl_type)
		{
			string name = HelpUtils.get_field_name(f);
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			fields.Add(new ItemInfo(name+".html",f.name));
			WriteBanner(sw);
			WriteElementHeaderDescription(sw,HelpUtils.get_localization("FIELD")+" "+f.name,HelpUtils.get_summary(f.Documentation));
  			WriteSyntaxHeader(sw);
  			sw.WriteLine(HelpUtils.get_field_header(f));
  			sw.WriteLine("</div>");
  			string example = HelpUtils.get_example(f.Documentation);
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("TYPE"),decl_type.file_name,decl_type.header);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
		}
		
		private void SaveEvent(ICommonEventNode e, List<ItemInfo> events, ItemInfo decl_type)
		{
			string name = HelpUtils.get_event_name(e);
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			events.Add(new ItemInfo(name+".html",e.Name));
			WriteBanner(sw);
			WriteElementHeaderDescription(sw,HelpUtils.get_localization("EVENT")+" "+e.Name,HelpUtils.get_summary(e.Documentation));
  			WriteSyntaxHeader(sw);
  			sw.WriteLine(HelpUtils.get_event_header(e));
  			sw.WriteLine("</div>");
  			string example = HelpUtils.get_example(e.Documentation);
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("TYPE"),decl_type.file_name,decl_type.header);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
		}
		
		private void SaveClassConstant(IClassConstantDefinitionNode c, List<ItemInfo> consts, ItemInfo decl_type)
		{
			string name = HelpUtils.get_const_name(c);
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			consts.Add(new ItemInfo(name+".html",c.name));
			WriteBanner(sw);
			WriteElementHeaderDescription(sw,HelpUtils.get_localization("CONSTANT")+" "+c.name,HelpUtils.get_summary(c.Documentation));
  			WriteSyntaxHeader(sw);
  			sw.WriteLine(HelpUtils.get_const_header(c));
  			sw.WriteLine("</div>");
  			string example = HelpUtils.get_example(c.Documentation);
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("TYPE"),decl_type.file_name,decl_type.header);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
		}
		
		private void SaveMethod(ICommonMethodNode m, List<ItemInfo> meths, List<ItemInfo> constrs, ItemInfo decl_type)
		{
			string name = HelpUtils.get_meth_name(m);
			if (parser.is_getter_or_setter(m))
				return;
			string file_name = null;
			if (!m.is_constructor)
				file_name = name+HelpUtils.possible_overload_symbol(m)+".html";
			else
				file_name = name+HelpUtils.possible_overload_constructor_symbol(m)+".html";
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,file_name),false,System.Text.Encoding.GetEncoding(1251));
			if (m.is_constructor)
			{
				if (parser.is_overloaded_constructor(m))
				{
					if (parser.GetConstructorIndex(m) == 1)
					constrs.Add(new ItemInfo(name+"$overloads"+".html",m.name));
				}
				else
				constrs.Add(new ItemInfo(file_name,m.name));
			}
			else
			if (parser.is_overload(m,m.common_comprehensive_type))
			{
				if (parser.GetMethodIndex(m,m.common_comprehensive_type) == 1)
					meths.Add(new ItemInfo(name+"$overloads"+".html",m.name));
			}
			else
			meths.Add(new ItemInfo(file_name,m.name));
			WriteBanner(sw);
			if (!m.is_constructor)
				WriteElementHeaderDescription(sw,HelpUtils.get_localization("METHOD")+" "+HelpUtils.build_name_with_possible_generic(m,true),HelpUtils.get_summary(m.Documentation));
			else
				WriteElementHeaderDescription(sw,HelpUtils.get_localization("CONSTRUCTOR")+" "+HelpUtils.build_name_with_possible_generic(m,true),HelpUtils.get_summary(m.Documentation));
  			WriteSyntaxHeader(sw);
  			sw.WriteLine(HelpUtils.get_meth_header(m));
  			sw.WriteLine("</div>");
  			List<NameInfo> prms = HelpUtils.get_parameters(m.Documentation);
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
  			string ret_value = HelpUtils.get_return_value_description(m.Documentation);
  			if (!string.IsNullOrEmpty(ret_value))
  			{
  				sw.WriteLine("<h4 class=\"dtH4\">"+HelpUtils.get_localization("RETURNS")+"</h4>");
  				sw.WriteLine("<p>");
  				sw.WriteLine(ret_value);
  				sw.WriteLine("</p>");
  			}
  			string example = HelpUtils.get_example(m.Documentation);
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("TYPE"),decl_type.file_name,decl_type.header);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
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
		
		private void SaveGlobalConstant(INamespaceConstantDefinitionNode c)
		{
			string name = HelpUtils.get_const_name(c);
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			consts.Add(new ItemInfo(name+".html",c.name));
			WriteBanner(sw);
			WriteElementHeaderDescription(sw,HelpUtils.get_localization("CONSTANT")+" "+c.name,HelpUtils.get_summary(c.Documentation));
  			WriteSyntaxHeader(sw);
  			sw.WriteLine(HelpUtils.get_const_header(c));
  			sw.WriteLine("</div>");
  			string example = HelpUtils.get_example(c.Documentation);
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("UNIT"),unit_name+".html",unit_name);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
		}
		
		private void WriteSyntaxHeader(StreamWriter sw)
		{
			sw.WriteLine("<h4 class=\"dtH4\">"+HelpUtils.get_localization("SYNTAX")+"</h4>");
			sw.WriteLine("<div class=\"activeLangTab\">");
			sw.WriteLine("<div>PascalABC.NET</div>");
			sw.WriteLine("</div>");
			sw.WriteLine("<div class=\"syntax\">");
		}
		
		private void SaveGlobalVariable(ICommonNamespaceVariableNode v)
		{
			string name = HelpUtils.get_var_name(v);
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			vars.Add(new ItemInfo(name+".html",v.name));
			WriteBanner(sw);
			WriteElementHeaderDescription(sw,HelpUtils.get_localization("VARIABLE")+" "+v.name,HelpUtils.get_summary(v.Documentation));
  			
			WriteSyntaxHeader(sw);
  			sw.WriteLine(HelpUtils.get_var_header(v));
  			sw.WriteLine("</div>");
  			string example = HelpUtils.get_example(v.Documentation);
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("UNIT"),unit_name+".html",unit_name);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
		}
		
		private void SaveGlobalFunction(ICommonNamespaceFunctionNode f)
		{
			string name = HelpUtils.get_meth_name(f);
			string file_name = name+HelpUtils.possible_overload_symbol(f)+".html";
			System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}",name,file_name));
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,file_name),false,System.Text.Encoding.GetEncoding(1251));
			if (parser.is_overload(f))
			{
				if (parser.GetMethodIndex(f)==1)
					procs.Add(new ItemInfo(name+"$overloads"+".html",f.name));
			}
			else
			procs.Add(new ItemInfo(file_name,f.name));
			WriteBanner(sw);
			if (f.return_value_type == null)
				WriteElementHeaderDescription(sw,HelpUtils.get_localization("PROCEDURE")+" "+HelpUtils.build_name_with_possible_generic(f,true),HelpUtils.get_summary(f.Documentation));
  			else
  				WriteElementHeaderDescription(sw,HelpUtils.get_localization("FUNCTION")+" "+HelpUtils.build_name_with_possible_generic(f,true),HelpUtils.get_summary(f.Documentation));
  			
  			WriteSyntaxHeader(sw);
  			sw.WriteLine(HelpUtils.get_func_header(f));
  			sw.WriteLine("</div>");
  			List<NameInfo> prms = HelpUtils.get_parameters(f.Documentation);
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
  			string ret_value = HelpUtils.get_return_value_description(f.Documentation);
  			if (!string.IsNullOrEmpty(ret_value))
  			{
  				sw.WriteLine("<h4 class=\"dtH4\">"+HelpUtils.get_localization("RETURNS")+"</h4>");
  				sw.WriteLine("<p>");
  				sw.WriteLine(ret_value);
  				sw.WriteLine("</p>");
  			}
  			string example = HelpUtils.get_example(f.Documentation);
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("UNIT"),unit_name+".html",unit_name);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
		}
		
		private void WriteLocation(StreamWriter sw, string obj, string file_name, string name)
		{
			sw.WriteLine("<h4 class=\"dtH4\">"+HelpUtils.get_localization("LOCATED")+"</h4>");
  			sw.WriteLine("<p>");
  			sw.WriteLine("<b>"+obj+"</b>");
  			sw.WriteLine("<a href=\""+file_name+"\">"+name+"</a>");
  			sw.WriteLine("</p>");
		}
		
		private ItemInfo SaveTypeSynonim(ITypeSynonym tt)
		{
			string name = HelpUtils.get_type_name(tt);
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			ItemInfo it = new ItemInfo(name+".html",tt.name);
			types.Add(it);
			WriteBanner(sw);
			WriteElementHeaderDescription(sw,HelpUtils.get_localization("TYPE")+" "+tt.name,HelpUtils.get_summary(tt.Documentation));
			WriteSyntaxHeader(sw);
			sw.WriteLine(HelpUtils.get_type_header(tt));
  			sw.WriteLine("</div>");
			string example = HelpUtils.get_example(tt.Documentation);
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("UNIT"),unit_name+".html",unit_name);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
  			return it;
		}
		
		private ItemInfo SaveClass(ICommonTypeNode tt)
		{
			string name = HelpUtils.get_type_name(tt);
			StreamWriter sw = new StreamWriter(Path.Combine(output_dir,name+".html"),false,System.Text.Encoding.GetEncoding(1251));
			ItemInfo it = new ItemInfo(name+".html",HelpUtils.build_name_with_possible_generic(tt,false));
			WriteBanner(sw);
			if (!HelpUtils.can_show_members(tt))
			{
				types.Add(it);
				WriteElementHeaderDescription(sw,HelpUtils.get_localization("TYPE")+" "+HelpUtils.build_name_with_possible_generic(tt,true),HelpUtils.get_summary(tt.Documentation));
			}
			else
			if (tt.IsEnum || tt.is_value_type)
			{
				rec_enums.Add(it);
				if (tt.IsEnum)
				{
					WriteElementHeaderDescription(sw,HelpUtils.get_localization("ENUM")+" "+HelpUtils.build_name_with_possible_generic(tt,true),HelpUtils.get_summary(tt.Documentation));
					sw.WriteLine("<p>");
  					sw.WriteLine(HelpUtils.get_localization("FOR_SEE_TYPE_MEMBERS")+" <a href=\""+name+"Members.html"+"\">"+tt.comprehensive_namespace.namespace_name+"."+HelpUtils.build_name_with_possible_generic(tt,true)+"</a>.</p>");
				}
				else if (tt.is_value_type)
				{
					WriteElementHeaderDescription(sw,HelpUtils.get_localization("RECORD")+" "+HelpUtils.build_name_with_possible_generic(tt,true),HelpUtils.get_summary(tt.Documentation));
					sw.WriteLine("<p>");
  					sw.WriteLine(HelpUtils.get_localization("FOR_SEE_RECORD_MEMBERS")+" <a href=\""+name+"Members.html"+"\">"+tt.comprehensive_namespace.namespace_name+"."+HelpUtils.build_name_with_possible_generic(tt,true)+"</a>.</p>");
				}
			}
			else if (tt.IsInterface)
			{
				interfaces.Add(it);
				WriteElementHeaderDescription(sw,HelpUtils.get_localization("INTERFACE")+" "+HelpUtils.build_name_with_possible_generic(tt,true),HelpUtils.get_summary(tt.Documentation));
  				sw.WriteLine("<p>");
  				sw.WriteLine(HelpUtils.get_localization("FOR_SEE_INTERFACE_MEMBERS")+" <a href=\""+name+"Members.html"+"\">"+tt.comprehensive_namespace.namespace_name+"."+HelpUtils.build_name_with_possible_generic(tt,true)+"</a>.</p>");
			}
			else
			{
				classes.Add(it);
				WriteElementHeaderDescription(sw,HelpUtils.get_localization("CLASS")+" "+HelpUtils.build_name_with_possible_generic(tt,true),HelpUtils.get_summary(tt.Documentation));
  				sw.WriteLine("<p>");
  				sw.WriteLine(HelpUtils.get_localization("FOR_SEE_CLASS_MEMBERS")+" <a href=\""+name+"Members.html"+"\">"+tt.comprehensive_namespace.namespace_name+"."+HelpUtils.build_name_with_possible_generic(tt,true)+"</a>.</p>");
			}
  			
  			
  			ITypeNode[] names = parser.GetParentClasses(tt);
  			WriteSyntaxHeader(sw);
  			//sw.WriteLine("<div><b>type</b> "+tt.name+" = <b>class</b>");
  			sw.WriteLine(HelpUtils.get_type_header(tt));
  			sw.WriteLine("</div>");

            if (HelpUtils.can_show_members(tt))
            {
                sw.WriteLine("<h4 class=\"dtH4\">" + HelpUtils.get_localization("CLASS_HIERARCHY") + "</h4>");
                sw.WriteLine("<p>");
                int off = 0;
                foreach (ITypeNode t in names)
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
                names = parser.GetChildClasses(tt);
                foreach (ITypeNode t in names)
                {
                    WriteSpaces(sw, off);
                    sw.Write(HelpUtils.get_type_html_text(t));
                    sw.Write("<br />");
                }
                sw.WriteLine("</p>");
            }
  			string example = HelpUtils.get_example(tt.Documentation);
  			if (example != null)
  			{
  				WriteExample(sw,HelpUtils.get_example_comment(example),HelpUtils.get_example_code(example));
  			}
  			WriteLocation(sw,HelpUtils.get_localization("UNIT"),unit_name+".html",unit_name);
  			sw.WriteLine("</div></body></html>");
  			sw.Close();
  			return it;
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

