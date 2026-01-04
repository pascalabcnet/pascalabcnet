// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Resources;
using System.Reflection;
using System.ComponentModel;

namespace PascalABCCompiler
{
    public class StringResourcesObject
    {
        public delegate string[] GetStringsDelegate(object obj);
        public delegate void SetStringsDelegate(object obj, string[] strings);
        public delegate object[] GetChildrenDelegate(object obj);
        internal GetStringsDelegate GetStrings;
        internal SetStringsDelegate SetStrings;
        internal GetChildrenDelegate GetChildren;
        internal Type ObjectType;
        public StringResourcesObject(Type ObjectType, GetStringsDelegate GetStrings, SetStringsDelegate SetStrings, GetChildrenDelegate GetChildren)
        {
            this.ObjectType = ObjectType;
            this.GetStrings = GetStrings;
            this.SetStrings = SetStrings;
            this.GetChildren = GetChildren;
        }
    }
	
    public class SourceFileInfo
    {
    	public string FileName;
    	public int Line;
    	public int Column;
    	
    	public SourceFileInfo(string FileName, int Line, int Column)
    	{
    		this.FileName = FileName;
    		this.Line = Line;
    		this.Column = Column;
    	}
    }
    
    public class BreakpointInfo
    {
    	public string FileName;
    	public int Line;
    	public bool IsConditional;
    	public string Expression;
    	public int ConcreteCondition;
    	
		public BreakpointInfo(string fileName, int line, bool isConditional, string expression, int concreteCondition)
		{
			this.FileName = fileName;
			this.Line = line;
			this.IsConditional = isConditional;
			this.Expression = expression;
			this.ConcreteCondition = concreteCondition;
		}
    	
    }
    
    public class BookmarkInfo
    {
    	public string FileName;
    	public int Line;
    	
    	public BookmarkInfo(string FileName, int Line)
    	{
    		this.FileName = FileName;
    		this.Line = Line;
    	}
    }
    
    public class AdvancedOptions
    {
    	public List<string> watch_list = new List<string>();
    	public List<SourceFileInfo> open_files = new List<SourceFileInfo>();
    	public List<BreakpointInfo> breakpoints = new List<BreakpointInfo>();
    	public List<BookmarkInfo> bookmarks = new List<BookmarkInfo>();
    }
    
    public class StringResources
    {
        private static Hashtable ObjectsHashtable = new Hashtable();
        private static Dictionary<Type, StringResourcesObject> StringResourcesObjects = new Dictionary<Type, StringResourcesObject>();
        private static Dictionary<Type, List<PropertyInfo>> StringPropertis = new Dictionary<Type, List<PropertyInfo>>();
        private static Dictionary<Type, List<FieldInfo>> StringFields = new Dictionary<Type, List<FieldInfo>>();
        private static Dictionary<Type, List<PropertyInfo>> ChildrenCollections = new Dictionary<Type, List<PropertyInfo>>();
        public static void AddStringResourcesObject(StringResourcesObject obj)
        {
            StringResourcesObjects[obj.ObjectType] = obj;
        }
        
        public static void UpdateObjectsText()
        {
            foreach (object obj in ObjectsHashtable.Keys)
            {
                string[] objstrings = (string[])ObjectsHashtable[obj];
                string[] resstrings = new string[objstrings.Length];
                for (int i = 0; i < objstrings.Length; i++)
                    resstrings[i] = Get(objstrings[i]);
                SetObjectStrings(obj, resstrings);
            }
        }
        
        public static void ReadStringsFromStreamAsXml(string FileName, StreamReader sr, Hashtable Strings, AdvancedOptions adv_opt)
        {
        	XmlReaderSettings xrs = new XmlReaderSettings();
        	//xrs.IgnoreWhitespace = true;
        	xrs.CloseInput = true;
        	XmlReader reader = XmlTextReader.Create(new StreamReader(FileName,System.Text.Encoding.UTF8),xrs);
        	try
        	{
        		reader.Read();
        	}
        	catch
        	{
        		System.Text.Encoding enc = sr.CurrentEncoding;
        		reader.Close();
        		sr.Close();
        		ReadStringsFromStream(new StreamReader(FileName,enc),Strings);
        		return;
        	}
        	//reader.ReadStartElement("options");
        	reader.ReadStartElement("general_options");
        	string prefix = "";
        	while (reader.ReadToNextSibling("option"))
        	{
        		string option_name = reader.GetAttribute("name");
        		string option_value = reader.ReadString();
        		if (option_name == "%PREFIX%")
        			prefix = option_value;
        		else
        		{
                   	Strings[prefix + option_name] = option_value;
        		}
        	}
        	reader.ReadEndElement();
        	/*reader.ReadEndElement();
        	if (!reader.EOF)
        	{
        		reader.ReadStartElement("advanced_options");
        		if (!reader.IsEmptyElement)
        		{
        			reader.ReadStartElement("open_files");
        			while (reader.ReadToNextSibling("open_file"))
        			{
        				adv_opt.open_files.Add(new SourceFileInfo(reader.GetAttribute("file_name"),
        			                                          Convert.ToInt32(reader.GetAttribute("line")),Convert.ToInt32(reader.GetAttribute("column"))));
					
        			}
        			reader.ReadEndElement();
        		}
        		else
        			reader.ReadStartElement("open_files");
        		reader.ReadStartElement("watch_list");
        		if (!reader.IsEmptyElement)
        		{
        			
        			while (reader.ReadToNextSibling("watch_expr"))
        			{
        				adv_opt.watch_list.Add(reader.GetAttribute("expr"));
        			}
        			reader.ReadEndElement();
        		}
        		reader.ReadStartElement("breakpoints_list");
        		if (!reader.IsEmptyElement)
        		{
        			
        			while (reader.ReadToNextSibling("breakpoint"))
        			{
        				adv_opt.breakpoints.Add(new BreakpointInfo(reader.GetAttribute("file_name"),
        			                                           Convert.ToInt32(reader.GetAttribute("line")),Convert.ToBoolean(reader.GetAttribute("is_conditional")),
        			                                           reader.GetAttribute("expression"),Convert.ToInt32(reader.GetAttribute("concrete_condition"))));
					
        			}
        			reader.ReadEndElement();
        		}
        		if (!reader.IsEmptyElement)
        		{
        			reader.ReadStartElement("bookmarks_list");
        			while (reader.ReadToNextSibling("bookmark"))
        			{
        				adv_opt.bookmarks.Add(new BookmarkInfo(reader.GetAttribute("file_name"),
        			                                          Convert.ToInt32(reader.GetAttribute("line"))));
					
        			}
        			reader.ReadEndElement();
        		}
        		reader.ReadEndElement();
        	}
        	reader.ReadEndElement();*/
        	reader.Close();
        	sr.Close();
        }
        
        public static void ReadStringsFromStream(StreamReader sr, Hashtable Strings)
        {
            string line, value, ident;
            string prefix = "";
            int ind;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line != "" && line.IndexOf("//") != 0 && (ind = line.IndexOf("=")) > 0)
                {
                    ident = line.Substring(0, ind);
                    value = line.Substring(ind + 1, line.Length - ind - 1);
                    if (ident == "%PREFIX%")
                        prefix = value;
                    else
                    {
                        if ((ind = value.IndexOf("LINES")) > 0)
                            try
                            {
                                int n = Convert.ToInt32(value.Substring(0, ind)) - 1;
                                value = sr.ReadLine();
                                for (int i = 0; i < n; i++)
                                    value += Environment.NewLine + sr.ReadLine();
                            }
                            catch (Exception)
                            {
                            }
                        Strings[prefix + ident] = value;
                    }
                }
            }
            sr.Close();
        }
        
        public static void WriteStringsToStreamAsXml(StreamWriter sw, Hashtable Strings, AdvancedOptions adv_opt)
        {
        	XmlWriterSettings xwr = new XmlWriterSettings();
        	xwr.Indent = true;
        	xwr.OmitXmlDeclaration = false;
        	xwr.Encoding = sw.Encoding;
        	xwr.CloseOutput = true;
        	XmlWriter writer = XmlWriter.Create(sw.BaseStream,xwr);
        	//writer.WriteStartElement("options");
        	writer.WriteStartElement("general_options");
        	foreach (string key in Strings.Keys)
        	{
        		writer.WriteStartElement("option");
        		writer.WriteAttributeString("name",key);
        		writer.WriteString(Convert.ToString(Strings[key]));
        		writer.WriteEndElement();
        	}
        	writer.WriteEndElement();
        	/*writer.WriteStartElement("advanced_options");
        	writer.WriteStartElement("open_files");
        	foreach (SourceFileInfo fi in adv_opt.open_files)
        	{
        		writer.WriteStartElement("open_file");
        		writer.WriteAttributeString("name",fi.FileName);
        		writer.WriteAttributeString("line",Convert.ToString(fi.Line.ToString()));
        		writer.WriteAttributeString("column",Convert.ToString(fi.Column.ToString()));
        		writer.WriteEndElement();
        	}
        	writer.WriteEndElement();
        	
        	writer.WriteStartElement("watch_list");
        	foreach (string s in adv_opt.watch_list)
        	{
        		writer.WriteStartElement("watch_expr");
        		writer.WriteAttributeString("expr",s);
        		writer.WriteEndElement();
        	}
        	writer.WriteEndElement();
        	
        	writer.WriteStartElement("breakpoints_list");
        	foreach (BreakpointInfo br in adv_opt.breakpoints)
        	{
        		writer.WriteStartElement("breakpoint");
        		writer.WriteAttributeString("file_name",br.FileName);
        		writer.WriteAttributeString("line",br.Line.ToString());
        		writer.WriteAttributeString("is_conditional",br.IsConditional.ToString());
        		writer.WriteAttributeString("expression",br.Expression);
        		writer.WriteAttributeString("concrete_condition",br.ConcreteCondition.ToString());
        		writer.WriteEndElement();
        	}
        	writer.WriteEndElement();
        	
        	writer.WriteStartElement("bookmarks_list");
        	foreach (BookmarkInfo br in adv_opt.bookmarks)
        	{
        		writer.WriteStartElement("bookmark");
        		writer.WriteAttributeString("file_name",br.FileName);
        		writer.WriteAttributeString("line",br.Line.ToString());
        		writer.WriteEndElement();
        	}
        	writer.WriteEndElement();
        	writer.WriteEndElement();
        	writer.WriteEndElement();*/
        	writer.Close();
        }
        
        public static void WriteStringsToStream(StreamWriter sw, Hashtable Strings)
        {
            foreach (string key in Strings.Keys)
                sw.WriteLine(string.Format("{0}={1}",key,Strings[key]));
            sw.Close();
        }

        private static string[] GetObjectStrings(object obj)
        {
            List<PropertyInfo> SetStringProperties = GetStringProperties(obj.GetType());
            string[] res = new string[SetStringProperties.Count];
            for (int i = 0; i < SetStringProperties.Count; i++)
                res[i] = (string)SetStringProperties[i].GetValue(obj, noIndex);
            return res;
        }

        private static string[] GetWPFObjectStrings(object obj)
        {
            List<FieldInfo> SetStringFields = GetStringFields(obj, obj.GetType());
            string[] res = new string[SetStringFields.Count];
            for (int i = 0; i < SetStringFields.Count; i++)
                res[i] = (string)SetStringFields[i].GetValue(obj);
            return res;
        }

        private static object[] noIndex = new object[0];

        private static List<PropertyInfo> GetStringProperties(Type type)
        {
            if (StringPropertis.ContainsKey(type))
                return StringPropertis[type];
            PropertyInfo[] Propertis = type.GetProperties();
            List<PropertyInfo> SetStringProperty = new List<PropertyInfo>();
            foreach (PropertyInfo pi in Propertis)
                if (pi.PropertyType == typeof(string) && pi.CanRead && pi.CanWrite && pi.GetIndexParameters().Length == 0)
                    SetStringProperty.Add(pi);
            StringPropertis.Add(type, SetStringProperty);
            return SetStringProperty;
        }

        private static List<FieldInfo> GetStringFields(object obj, Type type)
        {
            if (StringFields.ContainsKey(type))
                return StringFields[type];
            FieldInfo[] Fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            List<FieldInfo> SetStringField = new List<FieldInfo>();
            foreach (FieldInfo fi in Fields)
                //if (fi.GetValue(obj) != null && fi.GetValue(obj).GetType() == typeof(string) && !fi.IsInitOnly)
                if (fi.Name == "Header")
                    SetStringField.Add(fi);
            StringFields.Add(type, SetStringField);
            return SetStringField;
        }

        private static void SetObjectStrings(object obj, string[] strings)
        {
            List<PropertyInfo> SetStringProperties = GetStringProperties(obj.GetType());
            for (int i = 0; i < SetStringProperties.Count; i++)
                if(strings[i]!=null)
                    SetStringProperties[i].SetValue(obj, strings[i], noIndex);
        }

        private static void SetWPFObjectStrings(object obj, string[] strings)
        {
            List<FieldInfo> SetStringFields = GetStringFields(obj, obj.GetType());
            for (int i = 0; i < SetStringFields.Count; i++)
                if (strings[i] != null)
                    SetStringFields[i].SetValue(obj, strings[i]);
        }

        public static void SetTextForObject(object obj, string prefix)
        {
            string[] objstrings = GetObjectStrings(obj);
            if (objstrings == null) return;
            string[] resstrings = new string[objstrings.Length];
            string text;
            bool nonekeys = true;
            for (int i = 0; i < objstrings.Length; i++)
            {
                if ((text = objstrings[i]) != null)
                {
                    if (prefix != null && text.Length > 0 && text[0] != '!' && text != "")
                    {
                        string gets, s = prefix + text;
                        if ((gets = Get(s)) != s)
                        {
                            objstrings[i] = s;
                            resstrings[i] = gets;
                        }
                        else
                        {
                            resstrings[i] = null;
                            objstrings[i] = null;
                        }
                    }
                    else
                    {
                        resstrings[i] = Get(text);
                        if (resstrings[i] == objstrings[i])
                            resstrings[i] = objstrings[i] = null;
                    }
                }
                if (nonekeys && resstrings[i] != null)
                    nonekeys = false;
            }
            if (!nonekeys)
            {
                ObjectsHashtable[obj] = objstrings;
                SetObjectStrings(obj, resstrings);
            }
        }

        public static void SetTextForWPFObject(object obj, string prefix)
        {
            string[] objstrings = GetWPFObjectStrings(obj);
            if (objstrings == null) return;
            string[] resstrings = new string[objstrings.Length];
            string text;
            bool nonekeys = true;
            for (int i = 0; i < objstrings.Length; i++)
            {
                if ((text = objstrings[i]) != null)
                {
                    if (prefix != null && text.Length > 0 && text[0] != '!' && text != "")
                    {
                        string gets, s = prefix + text;
                        if ((gets = Get(s)) != s)
                        {
                            objstrings[i] = s;
                            resstrings[i] = gets;
                        }
                        else
                        {
                            resstrings[i] = null;
                            objstrings[i] = null;
                        }
                    }
                    else
                    {
                        resstrings[i] = Get(text);
                        if (resstrings[i] == objstrings[i])
                            resstrings[i] = objstrings[i] = null;
                    }
                }
                if (nonekeys && resstrings[i] != null)
                    nonekeys = false;
            }
            if (!nonekeys)
            {
                ObjectsHashtable[obj] = objstrings;
                SetWPFObjectStrings(obj, resstrings);
            }
        }

        public static void SetTextForAllWPFObjects(object obj, string prefix)
        {
            SetTextForWPFObject(obj, prefix);
            List<PropertyInfo> childComponentLists = GetWPFChildren(obj.GetType());
            foreach (PropertyInfo pi in childComponentLists)
            {
                try
                {
                    object val = pi.GetValue(obj, noIndex);
                    if (val is IEnumerator)
                    {
                        IEnumerator col = pi.GetValue(obj, noIndex) as IEnumerator;
                        col.Reset();
                        while (col.MoveNext())
                           SetTextForAllWPFObjects(col.Current, prefix);
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public static void SetTextForAllObjects(object obj, string prefix)
        {
            SetTextForObject(obj, prefix);
            List<PropertyInfo> childComponentLists = GetChildren(obj.GetType());
            foreach (PropertyInfo pi in childComponentLists)
            {
                try
                {
                    object val = pi.GetValue(obj, noIndex);
                    if (val is ICollection)
                    {
                        ICollection col = pi.GetValue(obj, noIndex) as ICollection;
                        if (col != null)
                            foreach (object ob in col)
                                //не расматриваем формы
                                if (ob.GetType().FullName != "System.Windows.Forms.Form")
                                    SetTextForAllObjects(ob, prefix);
                    }
                    if (val is IContainer)
                    {
                        IContainer cont = pi.GetValue(obj, noIndex) as IContainer;
                        if (cont != null)
                            foreach (object ob in cont.Components)
                                //не расматриваем формы
                                if (ob.GetType().FullName != "System.Windows.Forms.Form")
                                    SetTextForAllObjects(ob, prefix);
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private static List<PropertyInfo> GetChildren(Type type)
        {
            if (ChildrenCollections.ContainsKey(type))
                return ChildrenCollections[type];
            PropertyInfo[] propertis = type.GetProperties();
            List<PropertyInfo> childComponentLists = new List<PropertyInfo>();
            foreach (PropertyInfo pi in propertis)
                if (pi.PropertyType.GetInterface("ICollection") != null || pi.PropertyType.GetInterface("IContainer") != null)
                    childComponentLists.Add(pi);
            ChildrenCollections.Add(type, childComponentLists);
            return childComponentLists;
        }

        private static List<PropertyInfo> GetWPFChildren(Type type)
        {
            if (ChildrenCollections.ContainsKey(type))
                return ChildrenCollections[type];
            PropertyInfo property = type.GetProperty("LogicalChildren", BindingFlags.Instance | BindingFlags.NonPublic);
            List<PropertyInfo> childComponentLists = new List<PropertyInfo>();

            if (property != null)
                childComponentLists.Add(property);
            ChildrenCollections.Add(type, childComponentLists);
            return childComponentLists;
        }

        public static void SetTextForAllObjects(object obj)
        {
            SetTextForAllObjects(obj, null);
        }
        private static string resDirectoyName=null;
        public static string ResDirectoryName
        {
            get
            {
                return resDirectoyName;
            }
            set
            {
                resDirectoyName = value;
                Load();
            }

        }
        private static Hashtable Strings = new Hashtable();

        private static void LoadFile(string FileName)
        {
            if (!File.Exists(FileName))
                return;
            //    throw new
            LoadFile(new StreamReader(FileName, System.Text.Encoding.GetEncoding(1251)));
        }
        private static void LoadFile(StreamReader sr)
        {
            ReadStringsFromStream(sr, Strings);
        }
        private static void Load()
        {
            //try
            {
                //Strings.Clear();
                DirectoryInfo di = new DirectoryInfo(ResDirectoryName);
                if (!di.Exists)
                    return;
                FileInfo[] files = di.GetFiles("*.*");
                foreach (FileInfo fi in files)
                    LoadFile(fi.FullName);
            }
            //catch (Exception e)
            { 
            }
        }

        public static string Get(string keyName)
        {
            if (keyName == null || keyName == "") return keyName;
            string res = Strings[keyName] as string;
            if (res != null)
                return res;
            else
                return keyName;
        }
        public static void Set(string keyName,string value)
        {
            Strings.Add(keyName,value);
        }
        static StringResources()
        {
            //TODO: сделать так чтобы язык поумолчанию был в ресурсе
            /*StringResourcesLanguage.ConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName) + "\\lng\\languages.dat";
            if (StringResourcesLanguage.AccessibleLanguages.Count > 0)
                StringResourcesLanguage.CurrentLanguageName = StringResourcesLanguage.AccessibleLanguages[0];
             */
            if (Strings.Count == 0)
            {
                ResourceManager ResourceManager = new ResourceManager("PascalABCCompiler.DefaultLang", System.Reflection.Assembly.GetExecutingAssembly());
                MemoryStream ms = new MemoryStream((byte[])ResourceManager.GetObject("DefaultLanguage", System.Threading.Thread.CurrentThread.CurrentCulture));
                ms.Seek(0, SeekOrigin.Begin);
                LoadFile(new StreamReader(ms,Encoding.GetEncoding(1251)));
            }
        }
       
    }
}
