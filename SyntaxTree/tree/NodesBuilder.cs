using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PascalABCCompiler.SyntaxTree
{
    public class SyntaxTreeBuilder
    {
        public static SourceContext BuildGenSC = new SourceContext(0, 777777, 0, 0, 0, 0);

        public static type_definition BuildSimpleType(string name)
        {
            return new named_type_reference(name, null);
        }

        public static type_definition BuildSameType(expression ex)
        {
            return new same_type_node(ex);
        }

        public static type_definition BuildSemanticType(Object t)
        {
            return new semantic_type_node(t) as type_definition;
        }

        public static var_def_statement BuildSimpleVarDef(string name, string type)
        {
            return new var_def_statement(name, BuildSimpleType(type));
        }

        public static class_members BuildClassFieldsSection(List<ident> names, List<type_definition> types)
        {
            var cm = new class_members(access_modifer.private_modifer);
            for (int i = 0; i < names.Count; i++)
                cm.Add(new var_def_statement(names[i], types[i]));
            return cm;
        }

        public static class_members BuildOneMemberSection(declaration m)
        {
            var cm = new class_members(access_modifer.public_modifer);
            cm.Add(m);
            return cm;
        }

        public static formal_parameters BuildFormalParameters(List<ident> names, List<type_definition> types)
        {
            var fp = new formal_parameters();
            for (int i = 0; i < names.Count; i++)
                fp.Add(new typed_parameters(names[i], types[i]));
            return fp;
        }

        public static statement_list BuildSimpleAssignList(List<ident> lnames, List<ident> rnames)
        {
            var sl = new statement_list();
            for (int i = 0; i < lnames.Count; i++)
                sl.Add( new assign(lnames[i], rnames[i]));
            return sl;
        }

        public static procedure_definition BuildSimpleConstructor(List<ident> fields, List<ident> formal_names, List<type_definition> types)
        {
            var fp = SyntaxTreeBuilder.BuildFormalParameters(formal_names, types);
            if (fp.params_list.Count == 0)
                fp = null;
            var sl = SyntaxTreeBuilder.BuildSimpleAssignList(fields, formal_names);

            return new procedure_definition(new constructor(fp), new block(sl));
        }

        public static class_members BuildSimpleConstructorSection(List<ident> fields, List<ident> formal_names, List<type_definition> types)
        {
            var cm = new class_members(access_modifer.public_modifer);
            cm.Add(BuildSimpleConstructor(fields,formal_names,types));
            return cm;
        }

        public static simple_property BuildSimpleReadWriteProperty(ident name, ident field, type_definition type)
        {
            return new simple_property(name, type, new property_accessors(new read_accessor_name(field), new write_accessor_name(field)));
        }

        public static class_members BuildSimpleReadPropertiesSection(List<ident> names, List<ident> fields, List<type_definition> types)
        {
            var cm = new class_members(access_modifer.public_modifer);
            for (int i = 0; i < names.Count; i++)
                cm.Add(BuildSimpleReadWriteProperty(names[i], fields[i], types[i]));
            return cm;
        }

        public static class_definition BuildClassOrRecordDefinition(bool is_class, params class_members[] cms)
        {
            var cb = new class_body();
            foreach (var cm in cms)
                cb.Add(cm);
            var cd = new class_definition(cb);
            if (!is_class)
                cd.keyword = class_keyword.Record;
            return cd;
        }

        public static class_definition BuildClassDefinition(params class_members[] cms)
        {
            return BuildClassOrRecordDefinition(true, cms);
        }

        public static class_definition BuildClassDefinition(named_type_reference_list parents, params class_members[] cms)
        {
            var cb = new class_body();
            foreach (var cm in cms)
                cb.Add(cm);
            var cd = new class_definition(parents,cb);
            return cd;
        }

        // frninja 23/04/16 - для шаблонных классов в yield
        public static class_definition BuildClassDefinition(named_type_reference_list parents, ident_list template_args, params class_members[] cms)
        {
            var cb = new class_body();
            foreach (var cm in cms)
                cb.Add(cm);

            var cd = new class_definition(parents, cb, class_keyword.Class, template_args, null, class_attribute.None, false, null);
            return cd;
        }
        // end frninja

        // names и types передаю во внешний мир на предмет анализа того, что они не указатели. Снаружи они инициализируются пустыми списками
        public static void AddMembersForAutoClass(class_definition cd, ref List<ident> names, ref List<type_definition> types) // SSM 24.03.14
        {
            //var types = new List<type_definition>();
            class_body cb = cd.body;
            bool HasToString = false;
            bool HasConstructor = false;
            foreach (var l in cb.class_def_blocks)
            {
                foreach (var m in l.members)
                {
                    var mm = m as var_def_statement;
                    if (mm != null)
                    {
                        if (mm.var_attr != definition_attribute.Static)
                            foreach (var v in mm.vars.idents)
                            {
                                names.Add(v);
                                types.Add(mm.vars_type);    // во внешний мир для определения pointerов
                                //types.Add(BuildSameType(v)); // почему-то только так хочет работать с рекурсивным типом Node<T>
                            }
                    }
                    else 
                    {
                        var ts = m as procedure_definition;
                        if (!HasConstructor)
                        {
                            if (ts != null && ts.proc_header is constructor)
                            {
                                HasConstructor = true;
                            }
                        }   

                        if (!HasToString)
                        {
                            if (ts != null && ts.proc_header.name != null && ts.proc_header.name.meth_name.name != null)
                            {
                                HasToString = ts.proc_header.name.meth_name.name.ToUpper().Equals("TOSTRING");
                            }
                        }
                    }
                }
            }
            
            if (!HasConstructor)
            {
                var fnames = names.Select(x => new ident("f" + x.name)).ToList();    
                var cm = BuildSimpleConstructorSection(names, fnames, types);
                cb.Add(cm);
                //cb.class_def_blocks.Insert(0, cm);
            }

            if (!HasToString)
            { 
                var tostr = BuildToStringFuncForAutoClass(names);
                cb.Add(BuildOneMemberSection(tostr));
                //cb.class_def_blocks.Insert(0, BuildOneMemberSection(tostr));
            }
        }

        public static type_declaration BuildClassWithOneMethod(string class_name, List<ident> names, List<type_definition> types, procedure_definition pd)
        {
            var formnames = names.Select(x => new ident("form"+x.name)).ToList();

            var cm1 = BuildClassFieldsSection(names, types);
            var cm2 = BuildSimpleConstructorSection(names, formnames, types);
            var cm3 = BuildOneMemberSection(pd);

            return new type_declaration(class_name, BuildClassOrRecordDefinition(true, cm1, cm2, cm3), BuildGenSC);
        }

        public static type_declaration BuildAutoClass(string class_name, List<ident> names, List<type_definition> types, bool is_class)
        {
            var fnames = names.Select(x=>new ident("f"+x.name)).ToList();

            var cm1 = BuildClassFieldsSection(fnames,types);
            var cm2 = BuildSimpleConstructorSection(fnames,names,types);
            var cm3 = BuildSimpleReadPropertiesSection(names, fnames, types);

            return new type_declaration(class_name, BuildClassOrRecordDefinition(is_class, cm1, cm2, cm3), BuildGenSC);
        }

        public static type_declaration BuildClassWithFieldsOnly(string class_name, List<ident> names, List<type_definition> types, bool is_class)
        {
            var fnames = names.Select(x => new ident(x.name)).ToList();

            var cm1 = BuildClassFieldsSection(fnames, types);

            return new type_declaration(class_name, BuildClassOrRecordDefinition(is_class, cm1), BuildGenSC);
        }

        public static procedure_definition BuildShortFuncDefinition(formal_parameters fp, procedure_attributes_list att, method_name name, type_definition result, expression ex, SourceContext headsc)
        {
            var ff = new function_header(fp, att, name, null, result, headsc);
            procedure_definition pd = BuildShortProcFuncDefinition(ff, new assign("Result", ex, ex.source_context));
            return pd;
        }

        public static procedure_definition BuildShortProcDefinition(formal_parameters fp, procedure_attributes_list att, method_name name, statement st, SourceContext headsc)
        {
            var ff = new procedure_header(fp, att, name, null, headsc);
            return BuildShortProcFuncDefinition(ff, st);
        }

        public static procedure_definition BuildShortProcFuncDefinition(procedure_header header, statement st)
        {
            var stlist = new statement_list(st, st.source_context);
            var b = new block(null, stlist, st.source_context);

            return new procedure_definition(header, b, true, new SourceContext(header.source_context,st.source_context));
        }

        public static procedure_definition BuildShortProcFuncDefinitionNoSC(procedure_header header, statement st)
        {
            var stlist = new statement_list(st, st.source_context);
            var b = new block(null, stlist, st.source_context);

            return new procedure_definition(header, b, true, BuildGenSC);
        }

        public static procedure_definition BuildToStringFuncForAutoClass(List<ident> names)
        {
            var pal = new procedure_attributes_list(proc_attribute.attr_override);
            var fp = new formal_parameters();
            var ff = new function_header("ToString", "string", fp, pal);

            var cleft = new char_const('(');
            var cright = new char_const(')');
            var ccomma = new char_const(',');
            bin_expr ex = new bin_expr(cleft, cright, Operators.Plus);
            for (var i = 0; i < names.Count; i++)
            { 
                var dn = new dot_node(names[i], new ident("ToString"));
                var asnode = new typecast_node(names[i], new named_type_reference("object"), op_typecast.as_op);
                var eqnode = new bin_expr(asnode, new nil_const(), Operators.Equal);
                var expr = new question_colon_expression(eqnode, new string_const("nil"), dn);
                ex.left = new bin_expr(ex.left, expr, Operators.Plus);
                if (i<names.Count-1)
                    ex.left = new bin_expr(ex.left, ccomma, Operators.Plus);
            }
            var ass = new assign("Result", ex);

            return BuildShortProcFuncDefinitionNoSC(ff, ass);
        }
    }
}
