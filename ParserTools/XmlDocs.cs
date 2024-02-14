// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Reflection;
using System.Text;
using System.Globalization;

namespace CodeCompletionTools
{
	
	public partial class AssemblyDocCache
	{
		private static Hashtable ht=new Hashtable();

        public static string Load(Assembly a, string path)
        {
            if (ht[a] != null) return null;
            string dir;
            if (string.IsNullOrEmpty(a.Location))
            	dir = path;
            else
            if (a == typeof(string).Assembly) 
                dir = a.Location;
            else 
                dir = typeof(string).Assembly.Location.Substring(0, typeof(string).Assembly.Location.LastIndexOf('\\')) + "\\" + System.IO.Path.GetFileName(a.Location);
            bool ru = false;
            string xml_loc = XmlDoc.LookupLocalizedXmlDoc(dir, out ru);
            
            if (xml_loc == null)
                return null;
            XmlDoc xdoc = XmlDoc.Load(xml_loc, Environment.GetEnvironmentVariable("TEMP"), ru);
            xdoc.GetDocumentation("T:System.Int32",false);
            ht[a] = xdoc;
            return xml_loc;
        }

        public static string GetNormalHint(string s)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder tmp = new StringBuilder();
            int i = 0;
            if (s == null) return "";
            s = s.Trim(' ', '\n', '\t', '\r');
            if (s.Length > 0 && s[0] != '<')
                return s;
            bool was_param = false;
            bool new_line = false;
            while (i < s.Length)
            {
                if (s[i] == '<')
                {
                    i++;
                    bool close = false;
                    if (i < s.Length && s[i] == '/')
                        close = true;
                    while (i < s.Length && s[i] != '>')
                        tmp.Append(s[i++]);
                    i++;
                    if (!close)
                    {
                        string stmp = tmp.ToString();
                        switch (stmp)
                        {
                            case "summary":
                                tmp.Remove(0, tmp.Length);
                                break;
                            case "returns":
                                tmp.Remove(0, tmp.Length);
                                sb.Append("<returns>");
                                break;
                            default:
                                if (stmp.StartsWith("param name=\""))
                                {
                                    int start_ind = stmp.IndexOf('"') + 1;//"param name=\"".Length;
                                    int ind = stmp.IndexOf('"', start_ind);
                                    if (!was_param)
                                        sb.Append("<params>");
                                    was_param = true;
                                    sb.Append("<param>" + stmp.Substring(start_ind, ind - start_ind) + ": " + "</param>");
                                    tmp.Remove(0, tmp.Length);
                                }
                                else if (stmp.StartsWith("see cref=\""))
                                {
                                    int start_ind = "see cref=\"".Length + 2;
                                    if (stmp[start_ind - 1] != ':')
                                        start_ind -= 2;
                                    int ind = stmp.IndexOf('"', start_ind);
                                    sb.Append(stmp.Substring(start_ind, ind - start_ind));
                                    tmp.Remove(0, tmp.Length);
                                }
                                /*else if (stmp.StartsWith("exception cref=\""))
                                {
                                    int start_ind = "exception cref=\"".Length+2;
                                     if (stmp[start_ind-1] != ':')
                                        start_ind -= 2;
                                    int ind = stmp.IndexOf('"',start_ind);
                                    sb.Append(stmp.Substring(start_ind,ind-start_ind)+": ");
                                    tmp.Remove(0,tmp.Length);
                                }*/
                                else
                                    return sb.ToString();
                                break;
                        }
                    }
                    else
                    {
                        tmp.Remove(0, tmp.Length);
                    }
                }
                else
                    if (s[i] == '\n')
                    {
                        new_line = true;
                        sb.Append(s[i++]);
                    }
                    else if (s[i] == ' ' || s[i] == '\t')
                    {
                        if (!new_line)
                            sb.Append(s[i++]);
                        else
                            i++;
                    }
                    else
                    {
                        new_line = false;
                        if (i < s.Length && s[i] != '<')
                            sb.Append(s[i++]);
                    }
            }
            /*while (i < s.Length)
            {
                if (s[i]=='<')
                {
                    i++;
                    while (s[i] != '>') tmp.Append(s[i++]);
                    if (tmp.ToString() == "summary")
                    {
                        while (i < s.Length)
                        {
                        tmp.Remove(0,tmp.Length);
                        i++;
                        while (s[i] != '<') 
                        {
                            sb.Append(s[i++]);
                        }
                        i++;
                        while (s[i] != '>') 
                        {
                            tmp.Append(s[i++]);
                            if (tmp.ToString() == "see")
                            {
                                while (i < s.Length && s[i] != ':')
                                    i++;
                                i++;
                                while (i<s.Length && s[i] != '"')
                                    sb.Append(s[i++]);
                                i+=3;
                                while (i<s.Length && s[i] != '>') i++;
								
                                break;
                            }
                        }
                        if (tmp.ToString() == "/summary") return sb.ToString();
                        }
                    }
                }
                else return "";
				
            }*/
            return sb.ToString();
        }
		
		public static string GetDocumentation(Type t)
		{
			XmlDoc xdoc = (XmlDoc)ht[t.Assembly];
			try
			{
				if (xdoc != null)
				{
					string s = GetNormalHint(xdoc.GetDocumentation("T:"+t.FullName,false));
					return s;
				}
			}
			catch(Exception)
			{
				
			}
			return "";
		}

        public static string GetFullDocumentation(Type t)
        {
            XmlDoc xdoc = (XmlDoc)ht[t.Assembly];
            try
            {
                if (xdoc != null)
                {
                    return xdoc.GetDocumentation("T:" + t.FullName, false);
                }
            }
            catch (Exception)
            {

            }
            return "";
        }

		public static string GetParamNames(ConstructorInfo mi)
		{
			ParameterInfo[] pis = mi.GetParameters();
			if (pis.Length == 0) return "";
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append('(');
			for (int i=0; i<pis.Length; i++)
			{
				if (pis[i].ParameterType.FullName != null)
				sb.Append(pis[i].ParameterType.FullName.Replace('&','@'));
				if (i < pis.Length-1) sb.Append(',');
			}
			sb.Append(')');
			return sb.ToString();
		}
		
		public static string GetParamNames(MethodInfo mi)
		{
			ParameterInfo[] pis = mi.GetParameters();
			if (pis.Length == 0) return "";
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append('(');
			for (int i=0; i<pis.Length; i++)
			{
				sb.Append(GetTypeName(pis[i].ParameterType));
				if (i < pis.Length-1) sb.Append(',');
			}
			sb.Append(')');
			return sb.ToString();
		}

        public static string GetTypeName(Type t)
        {
            StringBuilder sb = new StringBuilder();
            if (t.FullName != null)
                sb.Append(t.FullName.Replace('&', '@'));
            else
            {
                if (t.IsGenericParameter)
                {
                    if (t.DeclaringMethod != null)
                        sb.Append("``" + t.GenericParameterPosition.ToString());
                    else
                        sb.Append("`" + t.GenericParameterPosition.ToString());
                }
                else if (t.IsArray)
                    sb.Append(GetTypeName(t.GetElementType()) + "[]");
                else if (t.IsByRef)
                    sb.Append(GetTypeName(t.GetElementType()) + "@");
                else if (t.ContainsGenericParameters)
                {
                    sb.Append(t.Namespace + "." + t.Name.Substring(0, t.Name.IndexOf('`')));
                    Type[] tt = t.GetGenericArguments();
                    sb.Append('{');
                    for (int j = 0; j < tt.Length; j++)
                    {
                        sb.Append(GetTypeName(tt[j]));
                        if (j < tt.Length - 1)
                            sb.Append(',');
                    }
                    sb.Append('}');
                }

            }
            return sb.ToString();
        }
		
		public static string GetDocumentation(ConstructorInfo mi)
		{
            try
            {
                XmlDoc xdoc = (XmlDoc)ht[mi.DeclaringType.Assembly];
                if (xdoc != null)
                {
                    string s = GetNormalHint(xdoc.GetDocumentation("M:" + mi.DeclaringType.FullName + ".#ctor" + GetParamNames(mi), false));
                    return s;
                }
            }
            catch (Exception e)
            {

            }
			return "";
		}

        public static string GetFullDocumentation(ConstructorInfo mi)
        {
            try
            {
                XmlDoc xdoc = (XmlDoc)ht[mi.DeclaringType.Assembly];
                if (xdoc != null)
                {
                    return xdoc.GetDocumentation("M:" + mi.DeclaringType.FullName + ".#ctor" + GetParamNames(mi), false);
                }
            }
            catch (Exception e)
            {

            }
            return "";
        }

		public static string GetDocumentation(FieldInfo fi)
		{
			try
			{
				XmlDoc xdoc = (XmlDoc)ht[fi.DeclaringType.Assembly];
				if (xdoc != null)
				{
					string s = GetNormalHint(xdoc.GetDocumentation("F:"+fi.DeclaringType.FullName+"."+fi.Name,false));
					return s;
				}
			}
			catch(Exception e)
			{
				
			}
			return "";
		}

        public static string GetFullDocumentation(FieldInfo fi)
        {
            try
            {
                XmlDoc xdoc = (XmlDoc)ht[fi.DeclaringType.Assembly];
                if (xdoc != null)
                {
                    return xdoc.GetDocumentation("F:" + fi.DeclaringType.FullName + "." + fi.Name, false);
                }
            }
            catch (Exception e)
            {

            }
            return "";
        }

		public static string GetDocumentation(PropertyInfo pi)
		{
			try
			{
				XmlDoc xdoc = (XmlDoc)ht[pi.DeclaringType.Assembly];
				if (xdoc != null)
				{
					string s = GetNormalHint(xdoc.GetDocumentation("P:"+pi.DeclaringType.FullName+"."+pi.Name,false));
					return s;
				}
			}
			catch(Exception e)
			{
				
			}
			return "";
		}

        public static string GetFullDocumentation(PropertyInfo pi)
        {
            try
            {
                XmlDoc xdoc = (XmlDoc)ht[pi.DeclaringType.Assembly];
                if (xdoc != null)
                {
                    return xdoc.GetDocumentation("P:" + pi.DeclaringType.FullName + "." + pi.Name, false);
                }
            }
            catch (Exception e)
            {

            }
            return "";
        }

		public static string GetDocumentation(EventInfo ei)
		{
			try
			{
				XmlDoc xdoc = (XmlDoc)ht[ei.DeclaringType.Assembly];
				if (xdoc != null)
				{
					string s = GetNormalHint(xdoc.GetDocumentation("E:"+ei.DeclaringType.FullName+"."+ei.Name,false));
					return s;
				}
			}
			catch(Exception e)
			{
				
			}
			return "";
		}

        public static string GetFullDocumentation(EventInfo ei)
        {
            try
            {
                XmlDoc xdoc = (XmlDoc)ht[ei.DeclaringType.Assembly];
                if (xdoc != null)
                {
                    return xdoc.GetDocumentation("E:" + ei.DeclaringType.FullName + "." + ei.Name, false);
                }
            }
            catch (Exception e)
            {

            }
            return "";
        }

		private static string GetGenericAddString(MethodInfo mi)
		{
			if (mi.ContainsGenericParameters && mi.IsGenericMethodDefinition)
				return "``"+Convert.ToString(mi.GetGenericArguments().Length);
			else
				return "";
		}
		
		public static string GetDocumentation(MethodInfo mi)
		{
			try
			{
				XmlDoc xdoc = (XmlDoc)ht[mi.DeclaringType.Assembly];
				if (xdoc != null)
				{
					string generic_add = GetGenericAddString(mi);
					string s = GetNormalHint(xdoc.GetDocumentation("M:"+mi.DeclaringType.FullName+"."+mi.Name+generic_add+GetParamNames(mi),false));
					return s;
				}
			}
			catch(Exception e)
			{
				
			}
			return "";
		}

        public static string GetFullDocumentation(MethodInfo mi)
        {
            try
            {
                XmlDoc xdoc = (XmlDoc)ht[mi.DeclaringType.Assembly];
                if (xdoc != null)
                {
                    string generic_add = GetGenericAddString(mi);
                    string s = xdoc.GetDocumentation("M:" + mi.DeclaringType.FullName + "." + mi.Name + generic_add + GetParamNames(mi), false);
                    return s;
                }
            }
            catch (Exception e)
            {

            }
            return "";
        }

        public static string GetDocumentationForNamespace(string name)
        {
            return "";
        }
		
		public static string GetDocumentation(Assembly a, string descr)
		{
			XmlDoc xdoc = (XmlDoc)ht[a];
			try
			{
				if (xdoc != null)
				{
					string s = GetNormalHint(xdoc.GetDocumentation(descr,false));
					return s;
				}
			}
			catch(Exception e)
			{
				
			}
			return "";
		}
	}
	
	/// <summary>
	/// Class capable of loading xml documentation files. XmlDoc automatically creates a
	/// binary cache for big xml files to reduce memory usage.
	/// </summary>
	public sealed class XmlDoc : IDisposable
	{
		static readonly List<string> xmlDocLookupDirectories;
		
		static XmlDoc()
		{
			xmlDocLookupDirectories = new List<string>();
			try
			{
				xmlDocLookupDirectories.AddRange(new string[] { System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory() } );
			}
			catch
			{
				
			}
		}
		
		public static IList<string> XmlDocLookupDirectories {
			get { return xmlDocLookupDirectories; }
		}
		
		struct IndexEntry : IComparable<IndexEntry>
		{
			public int HashCode;
			public int FileLocation;
			
			public int CompareTo(IndexEntry other)
			{
				return HashCode.CompareTo(other.HashCode);
			}
			
			public IndexEntry(int HashCode, int FileLocation)
			{
				this.HashCode = HashCode;
				this.FileLocation = FileLocation;
			}
		}
		
		Dictionary<string, string> xmlDescription = new Dictionary<string, string>();
		IndexEntry[] index; // SORTED array of index entries
		Queue<string> keyCacheQueue;
		
		const int cacheLength = 150; // number of strings to cache when working in file-mode
		
		void ReadMembersSection(XmlReader reader)
		{
			while (reader.Read()) {
				switch (reader.NodeType) {
					case XmlNodeType.EndElement:
						if (reader.LocalName == "members") {
							return;
						}
						break;
					case XmlNodeType.Element:
						if (reader.LocalName == "member" || reader.LocalName == "unit") {
							string memberAttr = reader.GetAttribute(0);
							string innerXml   = reader.ReadInnerXml();
							xmlDescription[memberAttr] = innerXml;
						}
							
						break;
				}
			}
		}
		
		public string GetDocumentation(string key, bool ignoreCase)
		{
			if (xmlDescription == null)
				throw new ObjectDisposedException("XmlDoc");
			lock (xmlDescription) {
				string result;
				//if (ignoreCase) key = key.ToLower();
				if (xmlDescription.TryGetValue(key, out result))
					return result;
				if (index == null)
					return null;
				return LoadDocumentation(key);
			}
		}
		
		#region Save binary files
		// FILE FORMAT FOR BINARY DOCUMENTATION
		// long  magic = 0x4244636f446c6d58 (identifies file type = 'XmlDocDB')
		const long magic = 0x4244636f446c6d58;
		// short version = 2              (file version)
		const short version = 2;
		// long  fileDate                 (last change date of xml file in DateTime ticks)
		// int   testHashCode = magicTestString.GetHashCode() // (check if hash-code implementation is compatible)
		// int   entryCount               (count of entries)
		// int   indexPointer             (points to location where index starts in the file)
		// {
		//   string key                   (documentation key as length-prefixed string)
		//   string docu                  (xml documentation as length-prefixed string)
		// }
		// indexPointer points to the start of the following section:
		// {
		//   int hashcode
		//   int    index           (index where the docu string starts in the file)
		// }
		
		void Save(string fileName, DateTime fileDate)
		{
			using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None)) {
				using (BinaryWriter w = new BinaryWriter(fs)) {
					w.Write(magic);
					w.Write(version);
					w.Write(fileDate.Ticks);
					
					IndexEntry[] index = new IndexEntry[xmlDescription.Count];
					w.Write(index.Length);
					
					int indexPointerPos = (int)fs.Position;
					w.Write(0); // skip 4 bytes
					
					int i = 0;
					foreach (KeyValuePair<string, string> p in xmlDescription) {
						index[i] = new IndexEntry(p.Key.GetHashCode(), (int)fs.Position);
						w.Write(p.Key);
						w.Write(p.Value.Trim());
						i += 1;
					}
					
					Array.Sort(index);
					
					int indexStart = (int)fs.Position;
					foreach (IndexEntry entry in index) {
						w.Write(entry.HashCode);
						w.Write(entry.FileLocation);
					}
					w.Seek(indexPointerPos, SeekOrigin.Begin);
					w.Write(indexStart);
				}
			}
		}
		#endregion
		
		#region Load binary files
		BinaryReader loader;
		FileStream fs;
		
		bool LoadFromBinary(string fileName, DateTime fileDate)
		{
			keyCacheQueue   = new Queue<string>(cacheLength);
			fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			int len = (int)fs.Length;
			loader = new BinaryReader(fs);
			try {
				if (loader.ReadInt64() != magic) {
					//LoggingService.Warn("Cannot load XmlDoc: wrong magic");
					return false;
				}
				if (loader.ReadInt16() != version) {
					//LoggingService.Warn("Cannot load XmlDoc: wrong version");
					return false;
				}
				if (loader.ReadInt64() != fileDate.Ticks) {
					//LoggingService.Info("Not loading XmlDoc: file changed since cache was created");
					return false;
				}
				int count = loader.ReadInt32();
				int indexStartPosition = loader.ReadInt32(); // go to start of index
				if (indexStartPosition >= len) {
					//LoggingService.Error("XmlDoc: Cannot find index, cache invalid!");
					return false;
				}
				fs.Position = indexStartPosition;
				IndexEntry[] index = new IndexEntry[count];
				for (int i = 0; i < index.Length; i++) {
					index[i] = new IndexEntry(loader.ReadInt32(), loader.ReadInt32());
				}
				this.index = index;
				return true;
			} catch (Exception ex) {
				//LoggingService.Error("Cannot load from cache", ex);
				return false;
			}
		}
	
		public static string LookupLocalizedXmlDoc(string fileName, out bool ru)
		{
			string xmlFileName         = Path.ChangeExtension(fileName, ".xml");
			ru = false;
			string localizedXmlDocFile = Path.GetDirectoryName(fileName);
			string ru_localizedXmlDocFile = localizedXmlDocFile;
			localizedXmlDocFile = Path.Combine(localizedXmlDocFile, Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
			localizedXmlDocFile = Path.Combine(localizedXmlDocFile, Path.GetFileName(xmlFileName));
			ru_localizedXmlDocFile = Path.Combine(ru_localizedXmlDocFile, /*"ru"*/PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO);
			ru_localizedXmlDocFile = Path.Combine(ru_localizedXmlDocFile, Path.GetFileName(xmlFileName));
			if (File.Exists(ru_localizedXmlDocFile))
			{
			   	ru = true;
				return ru_localizedXmlDocFile;
			}
			if (File.Exists(localizedXmlDocFile)) {
				return localizedXmlDocFile;
			}
			if (File.Exists(xmlFileName)) {
				return xmlFileName;
			}
			return null;
		}
		
		public static string LookupLocalizedXmlDocForUnitWithSources(string fileName, string langISO)
		{
            return null;
			string xmlFileName = Path.ChangeExtension(fileName, ".xml");
			string localizedXmlDocFile = Path.GetDirectoryName(fileName);
			localizedXmlDocFile = Path.Combine(localizedXmlDocFile, langISO);
			localizedXmlDocFile = Path.Combine(localizedXmlDocFile, Path.GetFileName(xmlFileName));
			if (File.Exists(localizedXmlDocFile))
				return localizedXmlDocFile;
			return null;
		}
		
		public static string LookupLocalizedXmlDocForUnit(string fileName, string langISO)
		{
            return null;
			string xmlFileName = Path.ChangeExtension(fileName, ".xml");
			string localizedXmlDocFile = Path.GetDirectoryName(fileName);
			localizedXmlDocFile = Path.Combine(localizedXmlDocFile, langISO);
			localizedXmlDocFile = Path.Combine(localizedXmlDocFile, Path.GetFileName(xmlFileName));
			if (File.Exists(localizedXmlDocFile))
				return localizedXmlDocFile;
			if (File.Exists(xmlFileName)) 
				return xmlFileName;
			return null;
		}
		
		string LoadDocumentation(string key)
		{
			if (keyCacheQueue.Count > cacheLength - 1) {
				xmlDescription.Remove(keyCacheQueue.Dequeue());
			}
			
			int hashcode = key.GetHashCode();
			
			// use interpolation search to find the item
			string resultDocu = null;
			
			int m = Array.BinarySearch(index, new IndexEntry(hashcode, 0));
			if (m >= 0) {
				// correct hash code found.
				// possibly there are multiple items with the same hash, so go to the first.
				while (--m >= 0 && index[m].HashCode == hashcode);
				// go through all items that have the correct hash
				while (++m < index.Length && index[m].HashCode == hashcode) {
					fs.Position = index[m].FileLocation;
					string keyInFile = loader.ReadString();
					if (keyInFile == key) {
						resultDocu = loader.ReadString();
						break;
					} else {
						//LoggingService.Warn("Found " + keyInFile + " instead of " + key);
					}
				}
			}
			
			keyCacheQueue.Enqueue(key);
			xmlDescription.Add(key, resultDocu);
			
			return resultDocu;
		}
		
		public void Dispose()
		{
			if (loader != null) {
				loader.Close();
				fs.Close();
			}
			xmlDescription = null;
			index = null;
			keyCacheQueue = null;
			loader = null;
			fs = null;
		}
		#endregion
		
		public static XmlDoc Load(XmlReader reader)
		{
			XmlDoc newXmlDoc = new XmlDoc();
			while (reader.Read()) {
				if (reader.IsStartElement()) {
					switch (reader.LocalName) {
						case "members":
							newXmlDoc.ReadMembersSection(reader);
							break;
						case "doc":
							newXmlDoc.ReadMembersSection(reader);
							break;
					}
				}
			}
			return newXmlDoc;
		}
		
		public static XmlDoc Load(string fileName, string cachePath, bool ru)
		{
			//LoggingService.Debug("Loading XmlDoc for " + file_name);
			XmlDoc doc;
			string cacheName = null;
			if (cachePath != null) {
				Directory.CreateDirectory(cachePath);
				cacheName = cachePath + "/" + Path.GetFileNameWithoutExtension(fileName)
					+ "." + fileName.GetHashCode().ToString("x") + (ru?"$ru":"") + ".dat";
				if (File.Exists(cacheName)) {
					doc = new XmlDoc();
					if (doc.LoadFromBinary(cacheName, File.GetLastWriteTimeUtc(fileName))) {
						//LoggingService.Debug("XmlDoc: Load from cache successful");
						return doc;
					} else {
						doc.Dispose();
						try {
							File.Delete(cacheName);
						} catch {}
					}
				}
			}
			
			try {
				using (XmlTextReader xmlReader = new XmlTextReader(fileName)) {
					doc = Load(xmlReader);
				}
			} catch (XmlException ex) {
				//LoggingService.Warn("Error loading XmlDoc " + file_name, ex);
				return new XmlDoc();
			}
			
			if (cachePath != null && doc.xmlDescription.Count > cacheLength * 2) {
				//LoggingService.Debug("XmlDoc: Creating cache for " + file_name);
				DateTime date = File.GetLastWriteTimeUtc(fileName);
				try {
					doc.Save(cacheName, date);
				} catch (Exception ex) {
					//LoggingService.Error("Cannot write to cache file " + cacheName, ex);
					return doc;
				}
				doc.Dispose();
				doc = new XmlDoc();
				doc.LoadFromBinary(cacheName, date);
			}
			return doc;
		}
	}
}
