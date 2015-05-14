using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using PascalABCCompiler.SyntaxTree;
using System.Xml;
using System.Text;
using System.IO;
using System.Reflection;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SemanticTree;

namespace VisualPascalABCPlugins
{
	
	public class HelpBuilderProject
	{
		public BuildOptions options;
		public List<string> files=new List<string>();
		public bool closed=true;
		public string file_name;
		
		public void Close()
		{
			files.Clear();
			closed = true;
			file_name = null;
		}
		
		public void Save()
		{
			closed = false;
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Encoding = Encoding.UTF8;
			settings.Indent = true;
			XmlWriter writer = XmlTextWriter.Create(file_name,settings);
			writer.WriteStartDocument();
			writer.WriteStartElement("project");
			writer.WriteStartElement("files");
			foreach (string s in files)
			{
				writer.WriteStartElement("file");
				writer.WriteString(s);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			writer.WriteStartElement("options");
			PropertyInfo[] props = options.GetType().GetProperties();
			foreach (PropertyInfo pi in props)
			{
				writer.WriteStartElement("option");
				writer.WriteAttributeString("type",pi.Name);
				writer.WriteValue(pi.GetValue(options,null));
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Close();
		}
		
		public bool Open(string file_name)
		{
			this.file_name = file_name;
			closed = false;
			XmlReader reader = XmlTextReader.Create(file_name);
			reader.ReadStartElement("project");
			reader.ReadStartElement("files");
			while (reader.ReadToNextSibling("file"))
				files.Add(reader.ReadString());
			reader.ReadEndElement();
			reader.ReadStartElement("options");
			while (reader.ReadToNextSibling("option"))
			{
				string option = reader.GetAttribute("type");
				PropertyInfo pi = options.GetType().GetProperty(option);
				
				object val = null;
				if (pi.PropertyType == typeof(string))
					val = reader.ReadElementContentAsString();
				else if (pi.PropertyType == typeof(bool))
					val = reader.ReadElementContentAsBoolean();
				else if (pi.PropertyType == typeof(int))
					val = reader.ReadElementContentAsInt();
				pi.SetValue(options,val,null);
			}
			reader.ReadEndElement();
			reader.ReadEndElement();
			reader.Close();
			return true;
		}
	}
}

