/*using System;
using System.IO;
using System.Text;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using System.Resources;
using System.Reflection;
using System.Collections.Generic;
using PascalABCCompiler.Errors;

namespace PascalABCCompiler.VBNETParser
{
	public class ASTConverter
	{
		public syntax_tree_node get_syntax_tree(ICSharpCode.NRefactory.Ast.CompilationUnit unit, string FileName)
		{
			unit_module mod = null;
			List<string> imports = new List<string>();
			foreach (ICSharpCode.NRefactory.Ast.INode cu in unit.Children)
			if (cu is ICSharpCode.NRefactory.Ast.TypeDeclaration)
			{
				ICSharpCode.NRefactory.Ast.TypeDeclaration td = cu as ICSharpCode.NRefactory.Ast.TypeDeclaration;
				if (td.Type == ICSharpCode.NRefactory.Ast.ClassType.Module)
				{
					mod = new unit_module();
					//mod.Language = "..."; // нужно заполнять !!!
					mod.file_name = FileName;
					mod.unit_name = new unit_name();
					mod.unit_name.idunit_name = new ident(td.Name);
					mod.compiler_directives = new List<compiler_directive>();
					mod.compiler_directives.Add(new compiler_directive(new token_info("reference"),new token_info("System.dll")));
					mod.compiler_directives.Add(new compiler_directive(new token_info("reference"),new token_info("Microsoft.VisualBasic.dll")));
					mod.compiler_directives.Add(new compiler_directive(new token_info("reference"),new token_info("System.Windows.Forms.dll")));
					mod.compiler_directives.Add(new compiler_directive(new token_info("reference"),new token_info("System.Drawing.dll")));
					mod.source_context = get_source_context(td);
					mod.interface_part = new interface_node();
					mod.interface_part.interface_definitions = new declarations();
					//mod.interface_part.source_context = new SourceContext(td.StartLocation.Line,td.StartLocation.Column,td.EndLocation.Line,td.EndLocation.Column);
					mod.interface_part.uses_modules = new uses_list();
					
					foreach (string s in imports)
					{
						List<ident> id_name = new List<ident>();
						id_name.Add(new ident(s));
						mod.interface_part.uses_modules.units.Add(new unit_or_namespace(new ident_list(id_name)));
					}
					List<ident> ids = new List<ident>();
					ids.Add(new ident("Microsoft.VisualBasic.Strings"));
					mod.interface_part.uses_modules.units.Add(new unit_or_namespace(new ident_list(ids)));
					ids = new List<ident>();
					ids.Add(new ident("System"));
					mod.interface_part.uses_modules.units.Add(new unit_or_namespace(new ident_list(ids)));
					ids = new List<ident>();
					ids.Add(new ident("System.Collections.Generic"));
					mod.interface_part.uses_modules.units.Add(new unit_or_namespace(new ident_list(ids)));
					ids = new List<ident>();
					ids.Add(new ident("Microsoft.VisualBasic.Constants"));
					mod.interface_part.uses_modules.units.Add(new unit_or_namespace(new ident_list(ids)));
					ids = new List<ident>();
					ids.Add(new ident("Microsoft.VisualBasic.VBMath"));
					mod.interface_part.uses_modules.units.Add(new unit_or_namespace(new ident_list(ids)));
					ids = new List<ident>();
					ids.Add(new ident("Microsoft.VisualBasic.Information"));
					mod.interface_part.uses_modules.units.Add(new unit_or_namespace(new ident_list(ids)));
					ids = new List<ident>();
					ids.Add(new ident("Microsoft.VisualBasic.Interaction"));
					mod.interface_part.uses_modules.units.Add(new unit_or_namespace(new ident_list(ids)));
					ids = new List<ident>();
					ids.Add(new ident("Microsoft.VisualBasic.FileSystem"));
					mod.interface_part.uses_modules.units.Add(new unit_or_namespace(new ident_list(ids)));
					ids = new List<ident>();
					ids.Add(new ident("Microsoft.VisualBasic.Financial"));
					mod.interface_part.uses_modules.units.Add(new unit_or_namespace(new ident_list(ids)));
					ids = new List<ident>();
					ids.Add(new ident("Microsoft.VisualBasic.DateAndTime"));
					mod.interface_part.uses_modules.units.Add(new unit_or_namespace(new ident_list(ids)));
					
					add_module_members(mod,td);
				}
			}
			else if (cu is ICSharpCode.NRefactory.Ast.UsingDeclaration)
			{
				ICSharpCode.NRefactory.Ast.UsingDeclaration usings = cu as ICSharpCode.NRefactory.Ast.UsingDeclaration;
				for (int i=0; i<usings.Usings.Count; i++)
				{
					imports.Add(usings.Usings[i].Name);
				}
			}
			return mod;
		}
		
		private void add_module_members(unit_module mod, ICSharpCode.NRefactory.Ast.TypeDeclaration td)
		{
			foreach (ICSharpCode.NRefactory.Ast.INode node in td.Children)
			{
				if (node is ICSharpCode.NRefactory.Ast.FieldDeclaration)
				{
					ICSharpCode.NRefactory.Ast.FieldDeclaration vd = node as ICSharpCode.NRefactory.Ast.FieldDeclaration;
					if ((vd.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.Dim) == ICSharpCode.NRefactory.Ast.Modifiers.Dim)
					foreach (ICSharpCode.NRefactory.Ast.VariableDeclaration vard in vd.Fields)
					{
						var_def_statement vds = new var_def_statement();
						vds.source_context = get_source_context(vd);
						vds.vars_type = get_type_reference(vard.TypeReference);
						ident_list idents = new ident_list();
						ident name = new ident(vard.Name);
						name.source_context = vds.source_context;
						idents.idents.Add(name);
						vds.vars = idents;
						mod.interface_part.interface_definitions.defs.Add(vds);
					}
					else if ((vd.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.Const) == ICSharpCode.NRefactory.Ast.Modifiers.Const)
					foreach (ICSharpCode.NRefactory.Ast.VariableDeclaration vard in vd.Fields)
					{
						const_definition tcd = null;
						if (vard.TypeReference is ICSharpCode.NRefactory.Ast.TypeReference)
							tcd = new simple_const_definition();
						else
							tcd = new typed_const_definition();
						tcd.source_context = get_source_context(vd);
						if (tcd is typed_const_definition)
							(tcd as typed_const_definition).const_type = get_type_reference(vard.TypeReference);
						tcd.const_name = new ident(vard.Name);
						tcd.const_name.source_context = tcd.source_context;
						tcd.const_value = get_expression(vard.Initializer);
						mod.interface_part.interface_definitions.defs.Add(tcd);
					}
				}
				else if (node is ICSharpCode.NRefactory.Ast.TypeDeclaration)
				{
					mod.interface_part.interface_definitions.defs.Add(get_type_declaration(node as ICSharpCode.NRefactory.Ast.TypeDeclaration));
				}
				else if (node is ICSharpCode.NRefactory.Ast.MethodDeclaration)
				{
					ICSharpCode.NRefactory.Ast.MethodDeclaration meth = node as ICSharpCode.NRefactory.Ast.MethodDeclaration;
					if (!meth.Body.IsNull)
					mod.interface_part.interface_definitions.defs.Add(get_method_declaration(meth));
					else
					mod.interface_part.interface_definitions.defs.Add(get_method_header(meth));
				}
			}
		}
		
		private type_declaration get_type_declaration(ICSharpCode.NRefactory.Ast.TypeDeclaration td)
		{
			type_declaration type_decl = new type_declaration();
			type_decl.type_name = new ident(td.Name);
			if (td.Type == ICSharpCode.NRefactory.Ast.ClassType.Enum)
			{
				type_decl.type_def = get_enum_type(td);
				type_decl.source_context = type_decl.type_def.source_context;
				return type_decl;
			}
			class_definition class_def = new class_definition();
			class_def.source_context = get_source_context(td);
			if (td.Type == ICSharpCode.NRefactory.Ast.ClassType.Interface)
				class_def.keyword = class_keyword.Interface;
			else if (td.Type == ICSharpCode.NRefactory.Ast.ClassType.Struct)
				class_def.keyword = class_keyword.Record;
			class_def.class_parents = get_base_classes(td.BaseTypes);
			class_def.body = get_class_body(td);
			type_decl.type_def = class_def;
			type_decl.source_context = class_def.source_context;
			return type_decl;
		}
		
		private enum_type_definition get_enum_type(ICSharpCode.NRefactory.Ast.TypeDeclaration td)
		{
			enum_type_definition enum_td = new enum_type_definition();
			enum_td.source_context = get_source_context(td);
			enum_td.enumerators = new enumerator_list();
			foreach (ICSharpCode.NRefactory.Ast.INode node in td.Children)
			{
				if (node is ICSharpCode.NRefactory.Ast.FieldDeclaration)
				{
					ICSharpCode.NRefactory.Ast.FieldDeclaration fld = node as ICSharpCode.NRefactory.Ast.FieldDeclaration;
					foreach (ICSharpCode.NRefactory.Ast.VariableDeclaration vd in fld.Fields)
					{
						enumerator en = new enumerator(new named_type_reference(vd.Name),null); // SSM здесь исправил 15.1.16
						en.source_context = get_source_context(fld);
						enum_td.enumerators.enumerators.Add(en);
					}
				}
			}
			return enum_td;
		}
		
		private named_type_reference_list get_base_classes(List<ICSharpCode.NRefactory.Ast.TypeReference> types)
		{
			named_type_reference_list ntrl = new named_type_reference_list();
			foreach (ICSharpCode.NRefactory.Ast.TypeReference tr in types)
			{
				ntrl.types.Add(get_type_reference(tr) as named_type_reference);
			}
			return ntrl;
		}
		
		private bool is_void(ICSharpCode.NRefactory.Ast.TypeReference tr)
		{
			if (tr.Type == "System.Void")
				return true;
			return false;
		}
		
		private class_body_list get_class_body(ICSharpCode.NRefactory.Ast.TypeDeclaration td)
		{
            class_body_list body = new class_body_list();
			body.source_context = get_source_context(td);
			foreach (ICSharpCode.NRefactory.Ast.INode node in td.Children)
			{
				body.class_def_blocks.Add(get_class_member(node));
			}
			return body;
		}
		
		private access_modifer_node get_access_modifier(ICSharpCode.NRefactory.Ast.Modifiers mod)
		{
				if ((mod & ICSharpCode.NRefactory.Ast.Modifiers.Public) == ICSharpCode.NRefactory.Ast.Modifiers.Public)
				{
					return new access_modifer_node(access_modifer.public_modifer);
				}
				else
				if ((mod & ICSharpCode.NRefactory.Ast.Modifiers.Private) == ICSharpCode.NRefactory.Ast.Modifiers.Private)
				{
					return new access_modifer_node(access_modifer.private_modifer);
				}
				else
				if ((mod & ICSharpCode.NRefactory.Ast.Modifiers.Protected) == ICSharpCode.NRefactory.Ast.Modifiers.Protected)
				{
					return new access_modifer_node(access_modifer.protected_modifer);
				}
				else
				if ((mod & ICSharpCode.NRefactory.Ast.Modifiers.Internal) == ICSharpCode.NRefactory.Ast.Modifiers.Internal)
				{
					return new access_modifer_node(access_modifer.internal_modifer);
				}
				else
				return new access_modifer_node(access_modifer.internal_modifer);
		}
		
		private class_members get_class_member(ICSharpCode.NRefactory.Ast.INode node)
		{
			class_members cmem = new class_members();
			if (node is ICSharpCode.NRefactory.Ast.FieldDeclaration)
			{
				ICSharpCode.NRefactory.Ast.FieldDeclaration fld = node as ICSharpCode.NRefactory.Ast.FieldDeclaration;
				cmem.access_mod = get_access_modifier(fld.Modifier);
				//if ((fld.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.Const) == ICSharpCode.NRefactory.Ast.Modifiers.Const)
				//cmem.members.AddRange(get_const_declaration(fld));
				//else
				cmem.members.AddRange(get_field_declaration(fld));
			}
			else if (node is ICSharpCode.NRefactory.Ast.ConstructorDeclaration)
			{
				ICSharpCode.NRefactory.Ast.ConstructorDeclaration meth = node as ICSharpCode.NRefactory.Ast.ConstructorDeclaration;
				cmem.access_mod = get_access_modifier(meth.Modifier);
				cmem.members.Add(get_constructor_declaration(meth));
			}
			else if (node is ICSharpCode.NRefactory.Ast.MethodDeclaration)
			{
				ICSharpCode.NRefactory.Ast.MethodDeclaration meth = node as ICSharpCode.NRefactory.Ast.MethodDeclaration;
				cmem.access_mod = get_access_modifier(meth.Modifier);
				if (!meth.Body.IsNull)
				cmem.members.Add(get_method_declaration(meth));
				else
				cmem.members.Add(get_method_header(meth));
			}
			return cmem;
			*//*else if (node is ICSharpCode.NRefactory.Ast.EventDeclaration)
			{
				ICSharpCode.NRefactory.Ast.EventDeclaration meth = node as ICSharpCode.NRefactory.Ast.EventDeclaration;
				cmem.access_mod = get_access_modifier(meth.Modifier);
				cmem.members.Add(get_event_declaration(meth));
			}*//*
		}
		
		private constructor get_constructor_header(ICSharpCode.NRefactory.Ast.ConstructorDeclaration method)
		{
			constructor proc_header = new constructor();
			proc_header.name = new method_name();
			proc_header.name.meth_name = new ident(method.Name);
			proc_header.source_context = get_source_context(method);
			proc_header.parameters = get_parameters(method.Parameters);
			if ((method.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.Abstract) == ICSharpCode.NRefactory.Ast.Modifiers.Abstract)
				proc_header.proc_attributes.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_abstract));
			if ((method.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.Virtual) == ICSharpCode.NRefactory.Ast.Modifiers.Virtual)
				proc_header.proc_attributes.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_virtual));
			if ((method.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.Override) == ICSharpCode.NRefactory.Ast.Modifiers.Override)
				proc_header.proc_attributes.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_override));
			//if ((method.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.Overloads) == ICSharpCode.NRefactory.Ast.Modifiers.Overloads)
			//	proc_header.proc_attributes.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_overload));
			if ((method.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.New) == ICSharpCode.NRefactory.Ast.Modifiers.New)
				proc_header.proc_attributes.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_reintroduce));
			if ((method.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.Static) == ICSharpCode.NRefactory.Ast.Modifiers.Static)
				proc_header.class_keyword = true;
			return proc_header;
		}
		
		private procedure_header get_method_header(ICSharpCode.NRefactory.Ast.MethodDeclaration method)
		{
			procedure_header proc_header;
			if (is_void(method.TypeReference))
			{
				proc_header = new procedure_header();
			}
			else
			{
				proc_header = new function_header();
			}
			proc_header.name = new method_name();
			proc_header.name.meth_name = new ident(method.Name);
			proc_header.source_context = get_source_context(method);
			if (proc_header is function_header)
				(proc_header as function_header).return_type = get_type_reference(method.TypeReference);
			proc_header.parameters = get_parameters(method.Parameters);
			proc_header.proc_attributes = new procedure_attributes_list();
			if ((method.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.Abstract) == ICSharpCode.NRefactory.Ast.Modifiers.Abstract)
				proc_header.proc_attributes.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_abstract));
			if ((method.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.Virtual) == ICSharpCode.NRefactory.Ast.Modifiers.Virtual)
				proc_header.proc_attributes.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_virtual));
			if ((method.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.Override) == ICSharpCode.NRefactory.Ast.Modifiers.Override)
				proc_header.proc_attributes.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_override));
			//if ((method.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.Overloads) == ICSharpCode.NRefactory.Ast.Modifiers.Overloads)
			//	proc_header.proc_attributes.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_overload));
			if ((method.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.New) == ICSharpCode.NRefactory.Ast.Modifiers.New)
				proc_header.proc_attributes.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_reintroduce));
			if ((method.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.Static) == ICSharpCode.NRefactory.Ast.Modifiers.Static)
				proc_header.class_keyword = true;
			return proc_header;
		}
		
		private procedure_definition get_method_declaration(ICSharpCode.NRefactory.Ast.MethodDeclaration method)
		{
			procedure_definition proc = new procedure_definition();
			proc.proc_header = get_method_header(method);
			proc.proc_body = get_body(method.Body);
			proc.source_context = new SourceContext(method.StartLocation.Line,method.StartLocation.Column,method.Body.EndLocation.Line,method.Body.EndLocation.Column);
			return proc;
		}
		
		private procedure_definition get_constructor_declaration(ICSharpCode.NRefactory.Ast.ConstructorDeclaration method)
		{
			procedure_definition proc = new procedure_definition();
			proc.proc_header = get_constructor_header(method);
			proc.proc_body = get_body(method.Body);
			proc.source_context = new SourceContext(method.StartLocation.Line,method.StartLocation.Column,method.Body.EndLocation.Line,method.Body.EndLocation.Column);
			return proc;
		}
		
		private List<declaration> get_field_declaration(ICSharpCode.NRefactory.Ast.FieldDeclaration vd)
		{
			List<declaration> fields = new List<declaration>();
			bool is_static = (vd.Modifier & ICSharpCode.NRefactory.Ast.Modifiers.Static) == ICSharpCode.NRefactory.Ast.Modifiers.Static;
			foreach (ICSharpCode.NRefactory.Ast.VariableDeclaration vard in vd.Fields)
			{
				var_def_statement vds = new var_def_statement();
				vds.source_context = get_source_context(vd);
				vds.vars_type = get_type_reference(vard.TypeReference);
				ident_list idents = new ident_list();
				ident name = new ident(vard.Name);
				name.source_context = vds.source_context;
				idents.idents.Add(name);
				vds.vars = idents;
				if (is_static)
				vds.var_attr = definition_attribute.Static;
				fields.Add(vds);
			}
			return fields;
		}
		
		private block get_body(ICSharpCode.NRefactory.Ast.BlockStatement body)
		{
			block blck = new block();
			blck.source_context = get_source_context(body);
			statement_list sl = new statement_list();
			sl.source_context = blck.source_context;
			blck.program_code = sl;
			foreach (ICSharpCode.NRefactory.Ast.INode stmt in body.Children)
			{
				if (stmt is ICSharpCode.NRefactory.Ast.LocalVariableDeclaration)
					get_var_statement(sl,stmt as ICSharpCode.NRefactory.Ast.LocalVariableDeclaration);
				//else if (stmt is ICSharpCode.NRefactory.Ast.ForStatement)
				//	get_for_statement(sl,stmt as ICSharpCode.NRefactory.Ast.ForStatement);
				else if (stmt is ICSharpCode.NRefactory.Ast.WithStatement)
					get_with_statement(sl,stmt as ICSharpCode.NRefactory.Ast.WithStatement);
				else if (stmt is ICSharpCode.NRefactory.Ast.BlockStatement)
					get_body(sl,stmt as ICSharpCode.NRefactory.Ast.BlockStatement);
				//else if (stmt is ICSharpCode.NRefactory.Ast.SwitchStatement)
				//	get_switch_statement(sl, stmt as ICSharpCode.NRefactory.Ast.SwitchStatement);
				else if (stmt is ICSharpCode.NRefactory.Ast.ForNextStatement)
					get_fornext_statement(sl, stmt as ICSharpCode.NRefactory.Ast.ForNextStatement);
					
			}
			return blck;
		}
		
		private statement_list get_statement_list(ICSharpCode.NRefactory.Ast.BlockStatement body)
		{
			return get_body(body).program_code;
		}
		
		private void get_with_statement(statement_list sl, ICSharpCode.NRefactory.Ast.WithStatement stmt)
		{
			with_statement with_stmt = new with_statement();
			with_stmt.source_context = get_source_context(stmt);
			with_stmt.do_with = new expression_list();
			with_stmt.do_with.source_context = get_source_context(stmt.Expression);
			with_stmt.do_with.expressions.Add(get_expression(stmt.Expression));
			with_stmt.what_do = get_statement_list(stmt.Body);
			sl.subnodes.Add(with_stmt);
		}
		
		private void get_fornext_statement(statement_list sl, ICSharpCode.NRefactory.Ast.ForNextStatement stmt)
		{
			for_node for_stmt = new for_node();
			for_stmt.loop_variable = new ident(stmt.VariableName);
			
			sl.subnodes.Add(for_stmt);
		}
		
		private void get_body(statement_list sl, ICSharpCode.NRefactory.Ast.BlockStatement stmt)
		{
			block block_stmt = new block();
			block_stmt.source_context = get_source_context(stmt);
			sl.subnodes.Add(get_statement_list(stmt));
		}
		
		private void get_var_statement(statement_list sl, ICSharpCode.NRefactory.Ast.LocalVariableDeclaration var)
		{
			foreach (ICSharpCode.NRefactory.Ast.VariableDeclaration vd in var.Variables)
			{
				var_def_statement vds = new var_def_statement();
				vds.source_context = get_source_context(var);
				vds.vars_type = get_type_reference(vd.TypeReference);
				ident_list idents = new ident_list();
				ident name = new ident(vd.Name);
				name.source_context = get_source_context(var);
				idents.idents.Add(name);
				vds.vars = idents;
				sl.subnodes.Add(new var_statement(vds));
			}
		}
		
		private SourceContext get_source_context(ICSharpCode.NRefactory.Ast.INode node)
		{
			return new SourceContext(node.StartLocation.Line, node.StartLocation.Column, node.EndLocation.Line, node.EndLocation.Column);
		}
		
		private type_definition get_type_reference(ICSharpCode.NRefactory.Ast.TypeReference tr)
		{
			string[] names = tr.Type.Split('.');
			named_type_reference ntr = new named_type_reference();
			ntr.source_context = get_source_context(tr);
			for (int i=0; i<names.Length; i++)
				ntr.names.Add(new ident(names[i]));
			return ntr;
		}
		
		private formal_parameters get_parameters(List<ICSharpCode.NRefactory.Ast.ParameterDeclarationExpression> prms)
		{
			formal_parameters fp = new formal_parameters();
			foreach (ICSharpCode.NRefactory.Ast.ParameterDeclarationExpression prm in prms)
			{
				typed_parameters tp = new typed_parameters();
				tp.source_context = get_source_context(prm);
				tp.idents = new ident_list();
				tp.idents.idents.Add(new ident(prm.ParameterName));
				tp.vars_type = get_type_reference(prm.TypeReference);
				tp.inital_value = get_expression(prm.DefaultValue);
				switch (prm.ParamModifier)
				{
					case ICSharpCode.NRefactory.Ast.ParameterModifiers.Ref : tp.param_kind = parametr_kind.var_parametr; break;
					case ICSharpCode.NRefactory.Ast.ParameterModifiers.Params : tp.param_kind = parametr_kind.params_parametr; break;
				}
				fp.params_list.Add(tp);
			}
			return fp;
		}
		
		public expression get_expression(ICSharpCode.NRefactory.Ast.Expression expr)
		{
			return convert_expression(expr);
		}
		
		private expression convert_expression(ICSharpCode.NRefactory.Ast.Expression expr)
		{
			if (expr is ICSharpCode.NRefactory.Ast.IdentifierExpression)
				return convert_identifier_expression(expr as ICSharpCode.NRefactory.Ast.IdentifierExpression);
			if (expr is ICSharpCode.NRefactory.Ast.BinaryOperatorExpression)
				return convert_binary_expression(expr as ICSharpCode.NRefactory.Ast.BinaryOperatorExpression);
			if (expr is ICSharpCode.NRefactory.Ast.UnaryOperatorExpression)
				return convert_unary_expression(expr as ICSharpCode.NRefactory.Ast.UnaryOperatorExpression);
			if (expr is ICSharpCode.NRefactory.Ast.InvocationExpression)
				return convert_invocation_expression(expr as ICSharpCode.NRefactory.Ast.InvocationExpression);
			*//*if (expr is ICSharpCode.NRefactory.Ast.FieldReferenceExpression)
				return convert_field_reference_expression(expr as ICSharpCode.NRefactory.Ast.FieldReferenceExpression);
			if (expr is ICSharpCode.NRefactory.Ast.NullExpression)
				return new nil_const();*//*
			if (expr is ICSharpCode.NRefactory.Ast.PrimitiveExpression)
				return convert_primitive_expression(expr as ICSharpCode.NRefactory.Ast.PrimitiveExpression);
			if (expr is ICSharpCode.NRefactory.Ast.TypeReferenceExpression)
				return convert_type_reference_expression(expr as ICSharpCode.NRefactory.Ast.TypeReferenceExpression);
			if (expr is ICSharpCode.NRefactory.Ast.TypeOfExpression)
				return convert_typeof_expression(expr as ICSharpCode.NRefactory.Ast.TypeOfExpression);
			if (expr is ICSharpCode.NRefactory.Ast.CastExpression)
				return convert_cast_expression(expr as ICSharpCode.NRefactory.Ast.CastExpression);
			if (expr is ICSharpCode.NRefactory.Ast.ParenthesizedExpression)
				return convert_expression((expr as ICSharpCode.NRefactory.Ast.ParenthesizedExpression).Expression);
			return null;
		}
		
		private expression convert_cast_expression(ICSharpCode.NRefactory.Ast.CastExpression expr)
		{
			type_definition td = get_type_reference(expr.CastTo);
			expression exp = get_expression(expr.Expression);
			return new typecast_node(exp as addressed_value, td, op_typecast.typecast);
		}
		
		private expression convert_typeof_expression(ICSharpCode.NRefactory.Ast.TypeOfExpression expr)
		{
			return new typeof_operator(get_type_reference(expr.TypeReference) as named_type_reference);
		}
		
		private expression convert_type_reference_expression(ICSharpCode.NRefactory.Ast.TypeReferenceExpression expr)
		{
			ICSharpCode.NRefactory.Ast.TypeReferenceExpression tre = expr as ICSharpCode.NRefactory.Ast.TypeReferenceExpression;
			if (!string.IsNullOrEmpty(tre.TypeReference.Type))
				return new ident(tre.TypeReference.Type);
			else
				return new ident(tre.TypeReference.Type);
		}
		
		private expression convert_primitive_expression(ICSharpCode.NRefactory.Ast.PrimitiveExpression expr)
		{
			TypeCode code = Type.GetTypeCode(expr.Value.GetType());
			switch (code)
			{
				case TypeCode.Boolean : return new bool_const((bool)expr.Value);
				case TypeCode.Byte : 
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
					return new int32_const(Convert.ToInt32(expr.Value));
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return new int64_const(Convert.ToInt64(expr.Value));
				case TypeCode.UInt64:
					return new uint64_const(Convert.ToUInt64(expr.Value));
				case TypeCode.Char:
					return new char_const((char)expr.Value);
				case TypeCode.String:
					return new string_const((string)expr.Value);
				case TypeCode.Double:
				case TypeCode.Single:
					return new double_const(Convert.ToDouble(expr.Value));
				default :
						return null;
			}
		}
		
		private expression convert_identifier_expression(ICSharpCode.NRefactory.Ast.IdentifierExpression expr)
		{
			return new ident(expr.Identifier);
		}
		
		*//*private expression convert_field_reference_expression(ICSharpCode.NRefactory.Ast.FieldReferenceExpression expr)
		{			
			expression obj = convert_expression(expr.TargetObject);
			if (!string.IsNullOrEmpty(expr.FieldName))
			return new dot_node(obj as addressed_value,new ident(expr.FieldName));
			else return obj;
		}*//*
		
		private expression convert_invocation_expression(ICSharpCode.NRefactory.Ast.InvocationExpression expr)
		{
			List<expression> prms = new List<expression>();
			for (int i=0; i<expr.Arguments.Count; i++)
			{
				prms.Add(convert_expression(expr.Arguments[i]));
			}
			expression obj = convert_expression(expr.TargetObject);
			method_call mcall = new method_call();
			mcall.dereferencing_value = obj as addressed_value;
			mcall.parameters = new expression_list();
			mcall.parameters.expressions.AddRange(prms);
			return mcall;
		}
		
		private expression convert_unary_expression(ICSharpCode.NRefactory.Ast.UnaryOperatorExpression expr)
		{
			Operators op = Operators.Undefined;
			expression left = convert_expression(expr.Expression);
			switch (expr.Op)
			{
				case ICSharpCode.NRefactory.Ast.UnaryOperatorType.Plus : op = Operators.Plus; break;
				case ICSharpCode.NRefactory.Ast.UnaryOperatorType.Minus : op = Operators.Minus; break;
				case ICSharpCode.NRefactory.Ast.UnaryOperatorType.Not : op = Operators.LogicalNOT; break;
				case ICSharpCode.NRefactory.Ast.UnaryOperatorType.BitNot : op = Operators.BitwiseNOT; break;
			}
			return new un_expr(left,op);
		}
		
		private expression convert_binary_expression(ICSharpCode.NRefactory.Ast.BinaryOperatorExpression expr)
		{
			Operators op = Operators.Undefined;
			expression left = convert_expression(expr.Left);
			expression right = convert_expression(expr.Right);
			
			switch (expr.Op)
			{
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.Add : op = Operators.Plus; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.Subtract : op = Operators.Minus; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.Multiply : op = Operators.Multiplication; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.Divide : op = Operators.Division; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.LessThan : op = Operators.Less; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.LessThanOrEqual : op = Operators.LessEqual; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.GreaterThan : op = Operators.Greater; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.GreaterThanOrEqual : op = Operators.GreaterEqual; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.Equality : op = Operators.Equal; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.InEquality : op = Operators.NotEqual; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.LogicalAnd : op = Operators.LogicalAND; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.LogicalOr : op = Operators.LogicalOR; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.ExclusiveOr : op = Operators.BitwiseXOR; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.Like : op = Operators.Equal; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.DivideInteger : op = Operators.IntegerDivision; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.Modulus : op = Operators.ModulusRemainder; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.BitwiseAnd : op = Operators.BitwiseAND; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.BitwiseOr : op = Operators.BitwiseOR; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.ReferenceEquality : op = Operators.Equal; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.ReferenceInequality : op = Operators.NotEqual; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.ShiftLeft : op = Operators.BitwiseLeftShift; break;
				case ICSharpCode.NRefactory.Ast.BinaryOperatorType.ShiftRight : op = Operators.BitwiseRightShift; break;
			}
			return new bin_expr(left,right,op);
		}
	}
}

*/