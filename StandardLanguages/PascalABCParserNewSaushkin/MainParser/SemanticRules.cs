// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using QUT.Gppg;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using Languages.Pascal.Errors;
using System.Text.RegularExpressions;
using Languages.Pascal.PascalABCNewParser;

namespace GPPGParserScanner
{
    public class NameExprPair
    {
        public ident id;
        public expression ex;
        public NameExprPair(ident id, expression ex)
        {
            this.id = id;
            this.ex = ex;
        }
    }

    public partial class GPPGParser
    {
        public declarations GlobalDecls = null;

        private int num = 0;

        public string Guid()
        {
            num++;
            return num.ToString();
        }

        public LexLocation SCToLexLoc(SourceContext sc) // переопределить неявное преобразование типов?
        {
            return new LexLocation(sc.begin_position.line_num, sc.begin_position.column_num - 1, sc.end_position.line_num, sc.end_position.column_num, sc.FileName);
        }

        public program_module NewProgramModule(program_name progName, Object optHeadCompDirs, uses_list mainUsesClose, syntax_tree_node progBlock, Object optPoint, LexLocation loc)
        {
            var progModule = new program_module(progName, mainUsesClose, progBlock as block, null, loc);
            progModule.Language = LanguageId.PascalABCNET;
            if (optPoint == null && progBlock != null)
            {
                var fp = progBlock.source_context.end_position;
                var err_stn = progBlock;
			    if ((progBlock is block) && (progBlock as block).program_code != null && (progBlock as block).program_code.subnodes != null && (progBlock as block).program_code.subnodes.Count > 0)
                    err_stn = (progBlock as block).program_code.subnodes[(progBlock as block).program_code.subnodes.Count - 1];
                parserTools.errors.Add(new PABCNETUnexpectedToken(parserTools.currentFileName, StringResources.Get("TKPOINT"), new SourceContext(fp.line_num, fp.column_num + 1, fp.line_num, fp.column_num + 1, 0, 0), err_stn));
            }
            return progModule;
        }

        public unit_name NewUnitHeading(ident unitkeyword, ident uname, LexLocation loc)
        {
            var un = new unit_name(uname, UnitHeaderKeyword.Unit, loc);
            if (unitkeyword.name.ToLower().Equals("library"))
				un.HeaderKeyword = UnitHeaderKeyword.Library;
            return un;
        }

        public unit_name NewNamespaceHeading(ident unitkeyword, ident_list nname, LexLocation loc)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nname.idents.Count; i++)
            {
                sb.Append(nname.idents[i]);
                if (i < nname.idents.Count - 1)
                    sb.Append(".");
            }
            ident uname = new ident(sb.ToString(), nname.source_context);
            var un = new unit_name(uname, UnitHeaderKeyword.Unit, loc);
            un.HeaderKeyword = UnitHeaderKeyword.Namespace;
            return un;
        }

        public procedure_header NewProcedureHeader(attribute_list attrlist, procedure_header nprh, procedure_attribute forw, LexLocation loc)
        {
            if (nprh.proc_attributes == null) 
				nprh.proc_attributes = new procedure_attributes_list();
            nprh.proc_attributes.Add(forw, forw.source_context); 
			nprh.attributes = attrlist;
			nprh.source_context = loc;
            return nprh;
        }

        public typecast_node NewAsIsConstexpr(expression constterm, op_typecast typecastop, type_definition tdef, LexLocation loc)
        {
            var naic = new typecast_node(constterm as addressed_value, tdef, typecastop, loc); 
			if (!(constterm is addressed_value))
                parserTools.errors.Add(new bad_operand_type(parserTools.currentFileName, constterm.source_context, naic));
            return naic;
        }

        public expression NewConstVariable(expression constvariable, expression constvariable2, LexLocation loc)
        {
            if (constvariable2 is dereference) 
				((dereference)constvariable2).dereferencing_value = (addressed_value)constvariable;
			if (constvariable2 is dot_node) 
				((dot_node)constvariable2).left = (addressed_value)constvariable;
			var ncv = constvariable2;
            ncv.source_context = loc;
            return ncv;
        }

        public class_definition NewObjectType(class_attribute class_attributes, token_info class_or_interface_keyword, named_type_reference_list opt_base_classes, where_definition_list opt_where_section, class_body_list opt_not_component_list_seq_end, LexLocation loc)
        {
            var nnof = new class_definition(opt_base_classes, opt_not_component_list_seq_end, class_keyword.Class, null, opt_where_section, class_attribute.None, false, loc); 
			string kw = class_or_interface_keyword.text.ToLower();
            nnof.attribute = class_attributes;
			if (kw=="record") 
				nnof.keyword=class_keyword.Record;
			else
			if (kw=="interface") 
				nnof.keyword=class_keyword.Interface;
			else
			if (kw=="i") 
				nnof.keyword=class_keyword.TemplateInterface;
			else
			if (kw=="r") 
				nnof.keyword=class_keyword.TemplateRecord;
			else
			if (kw=="c") 
				nnof.keyword=class_keyword.TemplateClass;
            if (nnof.body != null && ((class_definition)nnof).body.class_def_blocks != null &&
                ((class_definition)nnof).body.class_def_blocks.Count > 0 && ((class_definition)nnof).body.class_def_blocks[0].access_mod == null)
            {
                if (nnof.keyword == class_keyword.Class)
                    nnof.body.class_def_blocks[0].access_mod = new access_modifer_node(access_modifer.internal_modifer);
                else
                    nnof.body.class_def_blocks[0].access_mod = new access_modifer_node(access_modifer.none);
            }
            return nnof;
        }

        /*public class_definition NewRecordType(named_type_reference_list opt_base_classes, where_definition_list opt_where_section, class_body_list component_list_seq, LexLocation loc)
        {
            var nnrt = new class_definition(opt_base_classes, component_list_seq, class_keyword.Record, null, opt_where_section, class_attribute.None, false, loc); 
			if (nnrt.body!=null && nnrt.body.class_def_blocks!=null && 
				nnrt.body.class_def_blocks.Count>0 && nnrt.body.class_def_blocks[0].access_mod==null)
			{
                nnrt.body.class_def_blocks[0].access_mod = new access_modifer_node(access_modifer.public_modifer);
			}
            return nnrt;
        } */

        public token_info NewClassOrInterfaceKeyword(token_info tktemp)
        {
            var ncoik = tktemp;
            ncoik.text = "c";
            return ncoik;
        }

        public token_info NewClassOrInterfaceKeyword(token_info tktemp, string text, LexLocation loc)
        {
            var ncoik = tktemp;
			ncoik.text = text;
			ncoik.source_context = loc;
            return ncoik;
        }

        public procedure_header NewProcfuncHeading(procedure_header ph)
        {
            //ph.name.explicit_interface_name = ph.name.class_name;
			//ph.name.class_name = null;
            return ph;
        }

        public method_name NewQualifiedIdentifier(method_name qualified_identifier, ident identifier, LexLocation loc)
        {
            var nqi = qualified_identifier;
            nqi.class_name = nqi.meth_name;
			nqi.meth_name = identifier;
			nqi.source_context = loc;
            return nqi;
        }

        public declaration NewPropertyDefinition(attribute_list opt_attribute_declarations, declaration simple_property_definition, LexLocation loc)
        {
            var nnpd = simple_property_definition;
            nnpd.attributes = opt_attribute_declarations;
            nnpd.source_context = loc;
            return nnpd;
        }

        public simple_property NewSimplePrimPropertyDefinition(simple_property simple_property_definition, LexLocation loc)
        {
            var nsnpd = simple_property_definition;
			nsnpd.attr = definition_attribute.Static;
			nsnpd.source_context = loc;
            return nsnpd;
        }

        public simple_property NewSimplePropertyDefinition(method_name qualified_identifier, property_interface property_interface, property_accessors property_specifiers, proc_attribute virt_over_none_attr, property_array_default array_defaultproperty, LexLocation loc)
        {
            var nnspd = new simple_property();
            nnspd.virt_over_none_attr = virt_over_none_attr;
            List<ident> ln = null;
            if (qualified_identifier.ln != null)
                ln = qualified_identifier.ln;
            else if (qualified_identifier.class_name != null)
            {
                ln = new List<ident>();
                ln.Add(qualified_identifier.class_name);
            }
            nnspd.property_name = new property_ident(qualified_identifier.meth_name.name, ln, qualified_identifier.source_context);
			if (property_interface != null)
			{
				nnspd.parameter_list = property_interface.parameter_list;
				nnspd.property_type = property_interface.property_type;
				nnspd.index_expression = property_interface.index_expression;
			}
			if (property_specifiers != null) 
                nnspd.accessors = property_specifiers;
			if (array_defaultproperty != null) 
                nnspd.array_default = array_defaultproperty;
			nnspd.source_context = loc;
            return nnspd;
        }

        public property_accessors NewPropertySpecifiersRead(ident tkRead, ident opt_identifier, procedure_definition pr, expression ex, property_accessors property_specifiers, LexLocation loc)
        {
            var nnps = property_specifiers;
			if (nnps == null) 
			{
				nnps = new property_accessors();
			}
            if (opt_identifier != null && opt_identifier.name.ToLower() == "write")
            {
                nnps.read_accessor = new read_accessor_name(null, null, null);
                nnps.write_accessor = new write_accessor_name(null, null, null);
                nnps.read_accessor.source_context = tkRead.source_context;
                nnps.write_accessor.source_context = opt_identifier.source_context;
            }
            else
            {
                if (ex != null)
                    nnps.read_accessor = new read_accessor_name(opt_identifier, pr, ex, tkRead.source_context.Merge(ex.source_context));
                else if (opt_identifier != null)
                    nnps.read_accessor = new read_accessor_name(opt_identifier, pr, ex, tkRead.source_context.Merge(opt_identifier.source_context));
                else nnps.read_accessor = new read_accessor_name(opt_identifier, pr, ex, tkRead.source_context);

            }
			nnps.source_context = loc;
            return nnps;
        }

        public property_accessors NewPropertySpecifiersWrite(ident tkWrite, ident opt_identifier, procedure_definition pr, statement st, property_accessors property_specifiers, LexLocation loc)
        {
            var nnpsw = property_specifiers;
			if (nnpsw == null) 
			{
				nnpsw = new property_accessors();
			}
            if (st != null)
                nnpsw.write_accessor = new write_accessor_name(opt_identifier, pr, st, tkWrite.source_context.Merge(st.source_context));
            else if (opt_identifier != null)
			    nnpsw.write_accessor = new write_accessor_name(opt_identifier,pr,st,tkWrite.source_context.Merge(opt_identifier.source_context));
            else
                nnpsw.write_accessor = new write_accessor_name(opt_identifier,pr,st,tkWrite.source_context);
			nnpsw.source_context = loc;
            return nnpsw;
        }
        /*public procedure_definition NewProcDecl(procedure_definition proc_decl_noclass, LexLocation loc)
        {
            (proc_decl_noclass.proc_header as procedure_header).class_keyword = true;
            proc_decl_noclass.source_context = loc; 
			return proc_decl_noclass;
        }*/
        public case_variants NewCaseItem(syntax_tree_node case_item, LexLocation loc)
        {
            var nci = new case_variants(); 
			if (case_item is case_variant)
				nci.Add((case_variant)case_item);
            nci.source_context = loc;
            return nci;
        }

        public case_variants AddCaseItem(case_variants case_list, syntax_tree_node case_item, LexLocation loc)
        {
            var nci = case_list;
			if (case_item is case_variant)
                nci.Add((case_variant)case_item);
            nci.source_context = loc;
            return nci;
        }

        public while_node NewWhileStmt(token_info tkWhile, expression expr, token_info opt_tk_do, statement stmt, LexLocation loc)
        {
            var nws = new while_node(expr, stmt, WhileCycleType.While, loc); 
			if (opt_tk_do == null)
			{
				file_position fp = expr.source_context.end_position;
				syntax_tree_node err_stn = stmt;
				if (err_stn == null)
					err_stn = expr;
                parserTools.errors.Add(new PABCNETUnexpectedToken(parserTools.currentFileName, StringResources.Get("TKDO"), new SourceContext(fp.line_num, fp.column_num + 1, fp.line_num, fp.column_num + 1, 0, 0), err_stn));
			}
            return nws;
        }

        public for_node NewForStmt(bool opt_var, ident identifier, type_definition for_stmt_decl_or_assign, expression expr1, for_cycle_type fc_type, expression expr2, token_info opt_tk_do, statement stmt, expression increment_value, LexLocation loc)
        {
            var nfs = new for_node(identifier, expr1, expr2, stmt, fc_type, increment_value, for_stmt_decl_or_assign, opt_var != false, loc); 
            if (opt_tk_do == null)
            {
                file_position fp = expr2.source_context.end_position;
                syntax_tree_node err_stn = stmt;
                if (err_stn == null)
                    err_stn = expr2;
                parserTools.errors.Add(new PABCNETUnexpectedToken(parserTools.currentFileName, StringResources.Get("TKDO"), new SourceContext(fp.line_num, fp.column_num + 1, fp.line_num, fp.column_num + 1, 0, 0), err_stn));
            }
            if (!opt_var && for_stmt_decl_or_assign == null)
                parserTools.AddWarningFromResource("USING_UNLOCAL_FOR_VARIABLE", identifier.source_context);
            return nfs;
        }

        public typecast_node NewAsIsExpr(syntax_tree_node term, op_typecast typecast_op, type_definition simple_or_template_type_reference, LexLocation loc)
        {
            if (!(term is addressed_value))
                parserTools.errors.Add(new bad_operand_type(parserTools.currentFileName, term.source_context, term));
            var naie = new typecast_node((addressed_value)term, simple_or_template_type_reference, typecast_op, loc); 
			
            return naie;
        }

        public function_lambda_call NewFactor(ident func_decl_lambda, expression_list expr_list, LexLocation loc)
        {
            var fld = parserTools.find_pascalABC_lambda_name(func_decl_lambda.name);
            var _expression_list = expr_list;
			var _lambda_definition = fld;
			var _lambda_call = new function_lambda_call(_lambda_definition, _expression_list, loc);
			_lambda_call.source_context = func_decl_lambda.source_context;
			return _lambda_call;
        }

        public addressed_value NewVarReference(get_address var_address, addressed_value variable, LexLocation loc)
        {
            var_address.address_of = variable;
			parserTools.create_source_context(parserTools.NodesStack.Peek(),parserTools.NodesStack.Peek(), variable);
			return (addressed_value)parserTools.NodesStack.Pop();
        }

        public get_address NewVarAddress(LexLocation loc)
        {
            var nva = new get_address(); 
			nva.source_context = loc;
            parserTools.NodesStack.Push(nva);
            return nva;
        }

        public get_address NewVarAddress(get_address var_address, LexLocation loc)
        {
            var nva = new get_address();
            nva.source_context = loc;
            var_address.address_of = (addressed_value)nva;
            return nva;
        }
        
        public expression NewVariable(addressed_value variable, expression var_specifiers, LexLocation loc)
        {
            if (var_specifiers is dot_node) 
			{
                var dn = (dot_node)var_specifiers;
				dn.left = variable;
			}
			else if (var_specifiers is template_param_list) 
			{
                var tpl = (template_param_list)var_specifiers;
				((dot_node)tpl.dereferencing_value).left = variable;
				parserTools.create_source_context(tpl.dereferencing_value, variable, tpl.dereferencing_value);
			}
			else if (var_specifiers is dereference) 
			{
				((dereference)var_specifiers).dereferencing_value = variable;
			}
			else if (var_specifiers is ident_with_templateparams) 
			{
                ((ident_with_templateparams)var_specifiers).name = (addressed_value_funcname)variable;
			}
            var_specifiers.source_context = loc;
            return var_specifiers;
        }

        public literal NewLiteral(literal_const_line literal_list)
        {
            var lcl = literal_list;
			if (lcl.literals.Count == 1) 
				return lcl.literals[0];
			return lcl;
        }
        
        public expression ParseExpression(string Text, int line, int col)
        {
            PascalParserTools parserTools = new PascalParserTools(); // контекст сканера и парсера
            parserTools.errors = new List<Error>();
            parserTools.warnings = new List<CompilerWarning>();
            parserTools.currentFileName = System.IO.Path.GetFullPath(this.parserTools.currentFileName);
            parserTools.buildTreeForFormatterStrings = true;
            Scanner scanner = new Scanner();
            scanner.SetSource("<<expression>>"+Text, 0);
            scanner.parserTools = parserTools;// передали parserTools в объект сканера
            GPPGParser parser = new GPPGParser(scanner);
            parserTools.buildTreeForFormatter = false;
            parser.lambdaHelper = this.lambdaHelper;
            parser.parserTools = parserTools;
            if (!parser.Parse())
                if (parserTools.errors.Count == 0)
                    parserTools.AddError("Неопознанная синтаксическая ошибка!", null);
            foreach (Error err in parserTools.errors)
            {
                this.parserTools.errors.Add(err);
            }
            return parser.root as expression;
        }

        public expression NewFormatString(string_const str)
        {
            try
            {
                method_call mc = new method_call();
                mc.dereferencing_value = new dot_node(new ident("string", str.source_context), new ident("Format", str.source_context), str.source_context);
                mc.parameters = new expression_list();
                if (!str.Value.Contains("{"))
                    return str;
                string val = str.Value.Replace("{{","![&").Replace("}}}","}&]!").Replace("}}", "&]!");
                
                string[] arr = Regex.Split(val, @"\{[^\}]+\}");
                Match match = Regex.Match(val, @"\{[^\}]+\}");
                List<string> vars = new List<string>();
                //Dictionary<string, int> var_offsets = new Dictionary<string, int>();
                List<int> var_offsets = new List<int>();
                Dictionary<int, string> var_formats = new Dictionary<int, string>();
                int ind = 0;
                while (match.Success)
                {
                    string s = match.Value.Replace("{", "").Replace("}", "");
                    int colon_pos = s.LastIndexOf(':');
                    int comma_pos = s.LastIndexOf(',');
                    int bracket_pos = s.LastIndexOf(')');
                    int sqbracked_pos = s.LastIndexOf(']');
                    if (comma_pos != -1 && comma_pos > bracket_pos && comma_pos > sqbracked_pos)
                        colon_pos = comma_pos;
                    if (colon_pos != -1 && s.IndexOf('?') == -1 && s.Substring(colon_pos).IndexOf(']') == -1)
                    {
                        var_formats.Add(ind, s.Substring(colon_pos));
                        s = s.Substring(0, colon_pos);
                        
                    }
                    if (s.IndexOf("&]!") != -1 || s.IndexOf("![&") != -1)
                    {
                        parserTools.errors.Add(new bad_format_string(parserTools.currentFileName, str.source_context, str));
                        return str;
                    }
                    vars.Add(s);
                    var_offsets.Add(match.Index);
                    match = match.NextMatch();
                    ind++;
                }
                if (vars.Count == 0 && val.IndexOf("![&") == -1 && val.IndexOf("{") != -1)
                {
                    parserTools.errors.Add(new bad_format_string(parserTools.currentFileName, str.source_context, str));
                    return str;
                }
                    
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].IndexOf("{") != -1 || arr[i].IndexOf("}") != -1)
                    {
                        parserTools.errors.Add(new bad_format_string(parserTools.currentFileName, str.source_context, str));
                        return str;
                    }
                    sb.Append(arr[i].Replace("![&", "{{").Replace("&]!", "}}"));
                    if (i < arr.Length - 1)
                    {
                        sb.Append("{" + i);
                        string fmt;
                        if (var_formats.TryGetValue(i, out fmt))
                        {
                            sb.Append(fmt);
                        }
                        sb.Append("}");
                    }
                        
                }
                
                string str2 = sb.ToString();
                if (str2.Trim().EndsWith("{"))
                {
                    int cnt = 0;
                    string tmp = str2.Trim();
                    for (var i = 0; i < tmp.Length; i++)
                        if (tmp[i] == '{')
                            cnt++;
                    if (cnt % 2 == 1)
                    {
                        parserTools.errors.Add(new bad_format_string(parserTools.currentFileName, str.source_context, str));
                        return str;
                    }
                        
                }
                mc.parameters.Add(new string_const(str2, str.source_context), str.source_context);
                for (int i = 0; i < vars.Count; i++)
                {
                    string s = vars[i];
                    var expr = ParseExpression(new string('\n', str.source_context.begin_position.line_num - 1) + new string(' ', str.source_context.begin_position.column_num + var_offsets[i] + 2) + s, str.source_context.begin_position.line_num, str.source_context.begin_position.column_num + var_offsets[i] + 2);
                    if (expr == null)
                    {
                        var err = parserTools.errors[0] as LocatedError;
                        err.SourceContext.begin_position.line_num = str.source_context.begin_position.line_num;
                        err.SourceContext.begin_position.column_num = str.source_context.begin_position.column_num + var_offsets[i] + vars[i].Length + 3;
                        return str;
                    }
                    expr.source_context.begin_position.line_num = str.source_context.begin_position.line_num;
                    expr.source_context.end_position.line_num = str.source_context.end_position.line_num;
                    mc.parameters.Add(expr);
                }

                mc.source_context = str.source_context;
                return mc;
            }
            catch (Exception ex)
            {
                parserTools.errors.Add(new bad_format_string(parserTools.currentFileName, str.source_context, str));
            }
            return str;
        }

        /*public var_def_statement NewVarOrIdentifier(ident identifier, named_type_reference fptype, LexLocation loc)
        {
            var n_t_r = fptype;
			var vds = new var_def_statement();
			vds.vars = new ident_list();
			vds.vars.idents.Add(identifier);
			vds.vars_type = n_t_r;
            vds.source_context = loc;
			return vds;
        }*/

        /* Функция стала короткой и утратила необходимость
         *public statement_list NewLambdaBody(expression expr_l1, LexLocation loc)
        {
            var sl = new statement_list();
            sl.expr_lambda_body = true;
            var id = new ident("result");
			var op = new op_type_node(Operators.Assignment);
			var ass = new assign(id, expr_l1, op.type);
			parserTools.create_source_context(ass, id, expr_l1); // дурацкая функция - если хотя бы у одного sc=null, то возвращает null
            if (ass.source_context == null)
                if (expr_l1.source_context != null)
                    ass.source_context = expr_l1.source_context;
                else if (id.source_context != null)
                    ass.source_context = id.source_context;
            sl.subnodes.Add(ass);
            sl.source_context = loc;
            var sl = new statement_list(new assign("result",expr_l1,loc),loc);

            return sl;
        }/**/

        public procedure_definition CreateAndAddToClassReadFunc(expression ex, ident id, SourceContext sc)
        {
            var fd = SyntaxTreeBuilder.BuildShortFuncDefinition(new formal_parameters(), null, new method_name(id.name, sc), new no_type(), ex, sc);
            return fd;
        }
        public procedure_definition CreateAndAddToClassWriteProc(statement st, ident id, SourceContext sc)
        {
            var fp = SyntaxTreeBuilder.BuildFormalParameters(new List<ident>() { new ident("value") }, new List<type_definition>() { new no_type() });
            var pd = SyntaxTreeBuilder.BuildShortProcDefinition(fp, null, new method_name(id.name, sc), st, sc);
            return pd;
        }

        private int num1 = 0;

        public ident NewId(string prefix, SourceContext sc = null)
        {
            num1++;
            return new ident(prefix + num1.ToString(),sc);
        }
    }
}
