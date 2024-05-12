// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using PascalABCCompiler.SemanticTreeConverters;
using PascalABCCompiler.TreeRealization;
using PascalABCCompiler.TreeConverter;

namespace PascalABCCompiler
{
	public class DocXmlManager
	{
		private XmlWriter xtw;
		private CompilationUnit cu;
		private bool is_assembly;
		private string unit_name;
		
		public void SaveXml(CompilationUnit cu)
		{
            try
            {
                this.cu = cu;
                is_assembly = cu.SyntaxTree is SyntaxTree.program_module || Compiler.IsDll(cu.SyntaxTree);
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;
                xtw = XmlTextWriter.Create(Path.ChangeExtension(cu.UnitFileName, ".xml"), settings);
                unit_name = Path.GetFileNameWithoutExtension(cu.UnitFileName);
                xtw.WriteStartDocument();
                xtw.WriteStartElement("doc");
                if (is_assembly)
                {
                    xtw.WriteStartElement("assembly");
                    xtw.WriteStartElement("name");
                    xtw.WriteString(Path.GetFileNameWithoutExtension(cu.UnitFileName));
                    xtw.WriteEndElement();
                    xtw.WriteEndElement();
                }
                else
                {
                    xtw.WriteStartElement("unit");
                    xtw.WriteStartAttribute("name");
                    //xtw.WriteString((cu.SemanticTree as common_unit_node).unit_name);
                    xtw.WriteString(Path.GetFileNameWithoutExtension(cu.UnitFileName));
                    xtw.WriteEndAttribute();
                    xtw.WriteString(cu.SemanticTree.documentation);
                    xtw.WriteEndElement();
                }
                SaveMembers();
                xtw.WriteEndElement();
                xtw.Flush();
            }
            catch (Exception)
            {

            }
			try
			{
				if (xtw != null)
					xtw.Close();
			}
			catch
			{
				
			}
		}
		
		private void SaveMembers()
		{
			common_unit_node cun = cu.SemanticTree as common_unit_node;
			xtw.WriteStartElement("members");
			foreach(common_namespace_node cnn in cun.namespaces)
			{
				foreach (common_type_node ctn in cnn.types)
					SaveType(ctn);
				foreach (type_synonym ts in cnn.type_synonyms)
					SaveTypeSynonim(ts);
				foreach (common_namespace_function_node cnfn in cnn.functions)
					SaveFunction(cnfn);
				foreach (namespace_variable nv in cnn.variables)
					SaveVariable(nv);
				foreach (namespace_constant_definition ncd in cnn.constants)
					SaveConstant(ncd);
			}
			xtw.WriteEndElement();
		}
		
		private void SaveType(common_type_node ctn)
		{
			if (!string.IsNullOrEmpty(ctn.documentation))
			{
				if (!ctn.documentation.Trim(' ','\t').StartsWith("<summary>"))
				{
					xtw.WriteStartElement("member");
					xtw.WriteStartAttribute("name");
					if (is_assembly)
						xtw.WriteString("T:"+get_name(ctn));
					else
						xtw.WriteString("T:"+ctn.name);
					xtw.WriteEndAttribute();
					xtw.WriteStartElement("summary");
					xtw.WriteString(ctn.documentation);
					xtw.WriteEndElement();
					xtw.WriteEndElement();
				}
				else
				{
					string doc = string.Concat("<member name=\""+"T:"+(is_assembly?get_name(ctn):ctn.name)+"\">",ctn.documentation,"</member>");
					StringReader sr = new StringReader(doc);
					XmlReader xr = XmlTextReader.Create(sr);
					xr.Read();
					xtw.WriteNode(xr.ReadSubtree(),false);
					sr.Close();
					xr.Close();
				}
			}
			foreach (common_method_node cmn in ctn.methods)
				SaveMethod(cmn);
			foreach (common_property_node cmn in ctn.properties)
				SaveProperty(cmn);
			foreach (common_event ce in ctn.events)
				SaveEvent(ce);
			foreach (class_field cmn in ctn.fields)
				SaveField(cmn);
			foreach (class_constant_definition cmn in ctn.const_defs)
				SaveClassConstant(cmn);
		}
		
		private void SaveTypeSynonim(type_synonym ctn)
		{
			if (!string.IsNullOrEmpty(ctn.documentation))
			{
				if (!ctn.documentation.Trim(' ','\t').StartsWith("<summary>"))
				{
					xtw.WriteStartElement("member");
					xtw.WriteStartAttribute("name");
					xtw.WriteString("T:"+ctn.name);
					xtw.WriteEndAttribute();
					xtw.WriteStartElement("summary");
					xtw.WriteString(ctn.documentation);
					xtw.WriteEndElement();
					xtw.WriteEndElement();
				}
				else
				{
					string doc = string.Concat("<member name=\""+"T:"+ctn.name+"\">",ctn.documentation,"</member>");
					StringReader sr = new StringReader(doc);
					XmlReader xr = XmlTextReader.Create(sr);
					xr.Read();
					xtw.WriteNode(xr.ReadSubtree(),false);
					sr.Close();
					xr.Close();
				}
			}
		}
		
		private void SaveFunction(common_namespace_function_node cfn)
		{
			if (!string.IsNullOrEmpty(cfn.documentation))
			{
				if (!cfn.documentation.Trim(' ','\t').StartsWith("<summary>"))
				{
					xtw.WriteStartElement("member");
					xtw.WriteStartAttribute("name");
					xtw.WriteString("M:"+(is_assembly?unit_name+"."+unit_name+".":"")+cfn.name+GetGenericFlag(cfn)+GetParameters(cfn));
					xtw.WriteEndAttribute();
					xtw.WriteStartElement("summary");
					xtw.WriteString(cfn.documentation);
					xtw.WriteEndElement();
					xtw.WriteEndElement();
				}
				else
				{
					string doc = string.Concat("<member name=\""+"M:"+(is_assembly?unit_name+"."+unit_name+".":"")+cfn.name+GetGenericFlag(cfn)+GetParameters(cfn)+"\">",cfn.documentation,"</member>");
					StringReader sr = new StringReader(doc);
					XmlReader xr = XmlTextReader.Create(sr);
					xr.Read();
					xtw.WriteNode(xr.ReadSubtree(),false);
					sr.Close();
					xr.Close();
				}
			}
		}
		
		private void SaveEvent(common_event cfn)
		{
			if (!string.IsNullOrEmpty(cfn.documentation))
			{
				if (!cfn.documentation.Trim(' ','\t').StartsWith("<summary>"))
				{
					xtw.WriteStartElement("member");
					xtw.WriteStartAttribute("name");
					if (is_assembly)
						xtw.WriteString("E:"+get_name(cfn.cont_type)+"."+cfn.name);
					else
					xtw.WriteString("E:"+cfn.cont_type.name+"."+cfn.name);
					xtw.WriteEndAttribute();
					xtw.WriteStartElement("summary");
					xtw.WriteString(cfn.documentation);
					xtw.WriteEndElement();
					xtw.WriteEndElement();
				}
				else
				{
					string doc = string.Concat("<member name=\""+(is_assembly?("E:"+get_name(cfn.cont_type)+"."+cfn.name):("E:"+cfn.cont_type.name+"."+cfn.name))+"\">",cfn.documentation,"</member>");
					StringReader sr = new StringReader(doc);
					XmlReader xr = XmlTextReader.Create(sr);
					xr.Read();
					xtw.WriteNode(xr.ReadSubtree(),false);
					sr.Close();
					xr.Close();
				}
			}
		}
		
		private string GetGenericFlag(common_function_node cfn)
		{
			if (!cfn.is_generic_function) return "";
			return "``"+cfn.get_generic_params_list().Count.ToString();
		}
		
		private void SaveMethod(common_method_node cfn)
		{
			if (!string.IsNullOrEmpty(cfn.documentation))
			{
				if (!cfn.documentation.Trim(' ','\t').StartsWith("<summary>"))
				{
				xtw.WriteStartElement("member");
				xtw.WriteStartAttribute("name");
				if (is_assembly)
				{
					if (!cfn.is_constructor)
						xtw.WriteString("M:"+get_name(cfn.cont_type)+"."+cfn.name+GetGenericFlag(cfn)+GetParameters(cfn));
					else
					xtw.WriteString("M:"+get_name(cfn.cont_type)+".#ctor"+GetGenericFlag(cfn)+GetParameters(cfn));
				}
				else
				{
					if (!cfn.is_constructor)
					xtw.WriteString("M:"+cfn.cont_type.name+"."+cfn.name+GetGenericFlag(cfn)+GetParameters(cfn));
					else
					xtw.WriteString("M:"+cfn.cont_type.name+".#ctor"+GetGenericFlag(cfn)+GetParameters(cfn));
				}
				xtw.WriteEndAttribute();
				xtw.WriteStartElement("summary");
				xtw.WriteString(cfn.documentation);
				xtw.WriteEndElement();
				xtw.WriteEndElement();
				}
				else
				{
					string descr = null;
					if (is_assembly)
					{
						if (!cfn.is_constructor)
							descr = "M:"+get_name(cfn.cont_type)+"."+cfn.name+GetGenericFlag(cfn)+GetParameters(cfn);
						else
							descr = "M:"+get_name(cfn.cont_type)+".#ctor"+GetGenericFlag(cfn)+GetParameters(cfn);
					}
					else
					{
						if (!cfn.is_constructor)
							descr = "M:"+cfn.cont_type.name+"."+cfn.name+GetGenericFlag(cfn)+GetParameters(cfn);
						else
							descr = "M:"+cfn.cont_type.name+".#ctor"+GetGenericFlag(cfn)+GetParameters(cfn);
					}
					string doc = string.Concat("<member name=\""+descr+"\">",cfn.documentation,"</member>");
					StringReader sr = new StringReader(doc);
					XmlReader xr = XmlTextReader.Create(sr);
					xr.Read();
					xtw.WriteNode(xr.ReadSubtree(),false);
					sr.Close();
					xr.Close();
				}
			}
		}
		
		private string GetParameters(common_function_node cfn)
		{
			StringBuilder sb = new StringBuilder();
			if (cfn.parameters.Count > 0)
			{
				sb.Append("(");
				for (int i=0; i<cfn.parameters.Count; i++)
				{
					sb.Append(get_name(cfn.parameters[i].type));
					if (cfn.parameters[i].parameter_type == SemanticTree.parameter_type.var)
						sb.Append('@');
					if (i<cfn.parameters.Count-1)
						sb.Append(",");
				}
				sb.Append(")");
			}
			return sb.ToString();
		}
		
		private string get_name(type_node tn)
		{
			if (tn is compiled_type_node)
			return tn.full_name;
			if (tn is common_type_node)
			{
				if (tn.is_generic_type_instance)
				{
					StringBuilder sb = new StringBuilder();
					sb.Append(tn.original_generic.full_name);
					if (tn.instance_params.Count > 0)
					{
						sb.Append("{");
						for (int i=0; i<tn.instance_params.Count; i++)
						{
							sb.Append(get_name(tn.instance_params[i]));
							if (i<tn.instance_params.Count-1)
								sb.Append(",");
						}
						sb.Append("}");
					}
					return sb.ToString();
				}
				else if (tn.is_generic_parameter)
				{	
					if (tn.generic_function_container != null)
					return "``"+tn.generic_param_index;
					return "`"+tn.generic_param_index;
				}
				else if (tn.IsDelegate)
					return get_delegate_name(tn as common_type_node);
			}
			else if (tn is ref_type_node)
			{
				return get_pointer_name(tn as ref_type_node);
			}
			switch (tn.type_special_kind)
			{
				case SemanticTree.type_special_kind.array_wrapper : return get_array_name(tn as common_type_node);
				case SemanticTree.type_special_kind.set_type : return get_set_type_name(tn as common_type_node);
				case SemanticTree.type_special_kind.short_string : return get_shortstring_name(tn as short_string_type_node);
				case SemanticTree.type_special_kind.diap_type : return get_diap_name(tn as common_type_node);
				case SemanticTree.type_special_kind.binary_file: return get_binary_file_name(tn as common_type_node);
				case SemanticTree.type_special_kind.typed_file : return get_typed_file_name(tn as common_type_node);
				case SemanticTree.type_special_kind.array_kind : return get_dyn_array_name(tn as common_type_node);
				default : return tn.full_name;
			}
		}
		
		private string get_pointer_name(ref_type_node ctn)
		{
			type_node tn = ctn.pointed_type;
			int i=1;
			while (tn is ref_type_node)
			{
				i++;
				tn = (tn as ref_type_node).pointed_type;
			}
			string s = get_name(tn);
			for (int j=0; j<i; j++)
				s += "*";
			return s;
		}
		
		private string get_delegate_name(common_type_node ctn)
		{
			common_method_node cmn = ctn.find_first_in_type(StringConstants.invoke_method_name).sym_info as common_method_node;
			StringBuilder sb = new StringBuilder();
			if (cmn.return_value_type != null)
				sb.Append("@function");
			else sb.Append("@procedure");
			if (cmn.parameters.Count > 0)
			{
				sb.Append('(');
				for (int i=0; i<cmn.parameters.Count; i++)
				{
					sb.Append(get_name(cmn.parameters[i].type));
					if (cmn.parameters[i].parameter_type == SemanticTree.parameter_type.var)
						sb.Append('@');
					if (i<cmn.parameters.Count-1)
						sb.Append(',');
				}
				sb.Append(')');
			}
			if (cmn.return_value_type != null)
				sb.Append(":"+get_name(cmn.return_value_type));
			return sb.ToString();
		}
		
		private string get_constant(constant_node cn)
		{
			if (cn is int_const_node) return (cn as int_const_node).constant_value.ToString();
			if (cn is char_const_node) return "'"+(cn as char_const_node).constant_value.ToString()+"'";
			if (cn is byte_const_node) return (cn as byte_const_node).constant_value.ToString();
			if (cn is sbyte_const_node) return (cn as sbyte_const_node).constant_value.ToString();
			if (cn is short_const_node) return (cn as short_const_node).constant_value.ToString();
			if (cn is ushort_const_node) return (cn as ushort_const_node).constant_value.ToString();
			if (cn is uint_const_node) return (cn as uint_const_node).constant_value.ToString();
			if (cn is long_const_node) return (cn as long_const_node).constant_value.ToString();
			if (cn is ulong_const_node) return (cn as ulong_const_node).constant_value.ToString();
			return "";
		}
		
		private string get_dyn_array_name(common_type_node ctn)
		{
			type_node tn = ctn.element_type;
			int i=1;
			while (tn.type_special_kind == SemanticTree.type_special_kind.array_kind)
			{
				i++;
				tn = tn.element_type;
			}
			string s = get_name(tn);
			for (int j=0; j<i; j++)
				s += "[]";
			return s;
		}
		
		private string get_array_name(common_type_node ctn)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("@array");
			bounded_array_interface bai = ctn.get_internal_interface(internal_interface_kind.bounded_array_interface) as bounded_array_interface;
			sb.Append('[');
			sb.Append(get_constant(bai.ordinal_type_interface.lower_value));
			sb.Append("..");
			sb.Append(get_constant(bai.ordinal_type_interface.upper_value));
			sb.Append(']');
			sb.Append("["+get_name(ctn.element_type)+"]");
			return sb.ToString();
		}
		
		private string get_typed_file_name(common_type_node ctn)
		{
			return "@fileof["+get_name(ctn.element_type)+"]";
		}
		
		private string get_binary_file_name(common_type_node ctn)
		{
			return "@file";
		}
		
		private string get_shortstring_name(short_string_type_node ctn)
		{
			return "@string["+ctn.Length.ToString()+"]";
		}
		
		private string get_set_type_name(common_type_node ctn)
		{
			return "@set["+get_name(ctn.element_type)+"]";
		}
		
		private string get_diap_name(common_type_node ctn)
		{
			return "@diap["+get_constant(ctn.low_bound)+".."+get_constant(ctn.upper_bound)+"]";
		}
		
		private void SaveProperty(common_property_node cfn)
		{
			if (!string.IsNullOrEmpty(cfn.documentation))
			{
				if (!cfn.documentation.Trim(' ','\t').StartsWith("<summary>"))
				{
					xtw.WriteStartElement("member");
					xtw.WriteStartAttribute("name");
					if (is_assembly)
					xtw.WriteString("P:"+get_name(cfn.comprehensive_type)+"."+cfn.name);
					else
					xtw.WriteString("P:"+cfn.comprehensive_type.name+"."+cfn.name);
					xtw.WriteEndAttribute();
					xtw.WriteStartElement("summary");
					xtw.WriteString(cfn.documentation);
					xtw.WriteEndElement();
					xtw.WriteEndElement();
				}
				else
				{
					string doc = string.Concat("<member name=\""+(is_assembly?("P:"+get_name(cfn.comprehensive_type)+"."+cfn.name):("P:"+cfn.comprehensive_type.name+"."+cfn.name))+"\">",cfn.documentation,"</member>");
					StringReader sr = new StringReader(doc);
					XmlReader xr = XmlTextReader.Create(sr);
					xr.Read();
					xtw.WriteNode(xr.ReadSubtree(),false);
					sr.Close();
					xr.Close();
				}
			}
		}
		
		private void SaveField(class_field cfn)
		{
			if (!string.IsNullOrEmpty(cfn.documentation))
			{
				if (!cfn.documentation.Trim(' ','\t').StartsWith("<summary>"))
				{
					xtw.WriteStartElement("member");
					xtw.WriteStartAttribute("name");
					if (is_assembly)
						xtw.WriteString("F:"+get_name(cfn.cont_type)+"."+cfn.name);
					else
						xtw.WriteString("F:"+cfn.cont_type.name+"."+cfn.name);
					xtw.WriteEndAttribute();
					xtw.WriteStartElement("summary");
					xtw.WriteString(cfn.documentation);
					xtw.WriteEndElement();
					xtw.WriteEndElement();
				}
				else 
				{
					string doc = string.Concat("<member name=\""+(is_assembly?("F:"+get_name(cfn.cont_type)+"."+cfn.name):("F:"+cfn.cont_type.name+"."+cfn.name))+"\">",cfn.documentation,"</member>");
					StringReader sr = new StringReader(doc);
					XmlReader xr = XmlTextReader.Create(sr);
					xr.Read();
					xtw.WriteNode(xr.ReadSubtree(),false);
					sr.Close();
					xr.Close();
				}
			}
		}
		
		private void SaveClassConstant(class_constant_definition cfn)
		{
			if (!string.IsNullOrEmpty(cfn.documentation))
			{
				if (!cfn.documentation.Trim(' ','\t').StartsWith("<summary>"))
				{
					xtw.WriteStartElement("member");
					xtw.WriteStartAttribute("name");
					if (is_assembly)
					xtw.WriteString("F:"+get_name(cfn.comperehensive_type)+"."+cfn.name);
					else
					xtw.WriteString("F:"+cfn.comperehensive_type.name+"."+cfn.name);
					xtw.WriteEndAttribute();
					xtw.WriteStartElement("summary");
					xtw.WriteString(cfn.documentation);
					xtw.WriteEndElement();
					xtw.WriteEndElement();
				}
				else
				{
					string doc = string.Concat("<member name=\""+(is_assembly?("F:"+get_name(cfn.comperehensive_type)+"."+cfn.name):("F:"+cfn.comperehensive_type.name+"."+cfn.name))+"\">",cfn.documentation,"</member>");
					StringReader sr = new StringReader(doc);
					XmlReader xr = XmlTextReader.Create(sr);
					xr.Read();
					xtw.WriteNode(xr.ReadSubtree(),false);
					sr.Close();
					xr.Close();
				}
			}
		}
		
		private void SaveConstant(namespace_constant_definition cfn)
		{
			if (!string.IsNullOrEmpty(cfn.documentation))
			{
				if (!cfn.documentation.Trim(' ','\t').StartsWith("<summary>"))
				{
					xtw.WriteStartElement("member");
					xtw.WriteStartAttribute("name");
					xtw.WriteString((is_assembly?"F:"+unit_name+"."+unit_name+".":"C:")+cfn.name);
					xtw.WriteEndAttribute();
					xtw.WriteStartElement("summary");
					xtw.WriteString(cfn.documentation);
					xtw.WriteEndElement();
					xtw.WriteEndElement();
				}
				else
				{
					string doc = string.Concat("<member name=\""+(is_assembly?"F:"+unit_name+"."+unit_name+".":"C:")+cfn.name+"\">",cfn.documentation,"</member>");
					StringReader sr = new StringReader(doc);
					XmlReader xr = XmlTextReader.Create(sr);
					xr.Read();
					xtw.WriteNode(xr.ReadSubtree(),false);
					sr.Close();
					xr.Close();
				}
			}
		}
		
		private void SaveVariable(var_definition_node cfn)
		{
			if (!string.IsNullOrEmpty(cfn.documentation))
			{
				if (!cfn.documentation.Trim(' ','\t').StartsWith("<summary>"))
				{
					xtw.WriteStartElement("member");
					xtw.WriteStartAttribute("name");
					if (!is_assembly)
						xtw.WriteString("V:"+cfn.name);
					else
						xtw.WriteString("F:"+unit_name+"."+unit_name+"."+cfn.name);
					xtw.WriteEndAttribute();
					xtw.WriteStartElement("summary");
					xtw.WriteString(cfn.documentation);
					xtw.WriteEndElement();
					xtw.WriteEndElement();
				}
				else
				{
					
					string doc = string.Concat("<member name=\""+(is_assembly?"F:"+unit_name+"."+unit_name+".":"V:")+cfn.name+"\">",cfn.documentation,"</member>");
					StringReader sr = new StringReader(doc);
					XmlReader xr = XmlTextReader.Create(sr);
					xr.Read();
					xtw.WriteNode(xr.ReadSubtree(),false);
					sr.Close();
					xr.Close();
				}
			}
		}
	}
}

