using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.CParser.Errors;


namespace PascalABCCompiler.CParser
{
    internal class token_info_with_data<T> : token_info
    {
        public T data;
        public token_info_with_data(T data)
        {
            this.data = data;
        }
    }
    internal class c_scalar_sign_token : token_info_with_data<c_scalar_sign>
    {
        public c_scalar_sign_token(c_scalar_sign data)
            : base(data)
        {
        }
    }

    internal class ref_type_and_var_def_statement : var_def_statement
    {
        public ref_type ref_type;
        public var_def_statement var_def_statement;
        public function_header function_header;
    }

    internal class ConvertionTools
    {
        parser_tools parsertools;

        GPBParser parser;

        public List<PascalABCCompiler.Errors.Error> errors;

        public ConvertionTools(GPBParser parser)
        {
            this.parser = parser;
            errors=parser.errors;
            parsertools = parser.parsertools;
        }

        public syntax_tree_node PrepareArrayType(declaration td, object lp, expression expr, object rp)
        {
            array_type ar = new array_type();
            indexers_types it = new indexers_types();
            ar.indexers = it;
            parsertools.create_source_context(it, lp, rp);
            if (expr != null)
            {
                array_size sz = new array_size(expr);
                parsertools.assign_source_context(sz, expr);
                it.indexers.Add(sz);
            }
            parsertools.create_source_context(ar, lp, rp);
            var_def_statement vdf = (var_def_statement)td;
            ref_type rt = null;
            if (vdf is ref_type_and_var_def_statement)
            {
                rt = (vdf as ref_type_and_var_def_statement).ref_type;
                vdf = (vdf as ref_type_and_var_def_statement).var_def_statement;
                vdf.vars_type = SafeConnectToRefType(rt, vdf.vars_type);
            }
            if (vdf.vars_type == null)
            {
                vdf.vars_type = ar;
                return vdf;
            }
            ar.elemets_types = vdf.vars_type;
            vdf.vars_type = ar;

            //ConnectElementTypeToArrayType((array_type)vdf.vars_type, SafeConnectToRefType(rt,ar));
            return vdf;
            /*


            return vdf;
                parsertools.create_source_context(r, ar, ar);
                return r;
            else
                if (td is array_type_and_var_def_statement)
                {
                    array_type_and_var_def_statement r = td as array_type_and_var_def_statement;
                    array_type t = r.array_type;
                    while (t.elemets_types != null)
                        t = (array_type)t.elemets_types;
                    t.elemets_types = ar;
                    parsertools.create_source_context(t, t, ar);
                    parsertools.assign_source_context(r, t);
                    return r;
                }*/
            return null;
        }

        public ref_type PrepareRefType(ref_type rf)
        {
            while (rf.pointed_to != null && (rf.pointed_to is ref_type))
                rf = (ref_type)rf.pointed_to;
            return rf;
        }
        public type_definition SafePrepareRefType(type_definition td)
        {
            if (td is ref_type)
                return PrepareRefType(td as ref_type).pointed_to;
            return td;
        }
        public type_definition SafeConnectToRefType(ref_type rf, type_definition td)
        {
            if (rf == null)
                return td;
            ref_type r = rf;
            rf = PrepareRefType(rf);
            rf.pointed_to = td;
            return r;
        }

        public declaration PrepareInitDeclarator(object obj)
        {
            ref_type_and_var_def_statement rtvds = null;
            if (obj is var_def_statement)
                return obj as var_def_statement;
            if (obj is ref_type)
            {
                rtvds = new ref_type_and_var_def_statement();
                rtvds.ref_type = obj as ref_type;
                obj = PrepareRefType(rtvds.ref_type).pointed_to;
                PrepareRefType(rtvds.ref_type).pointed_to = null;
            }
            var_def_statement vds = null;
            if (obj is function_header)
            {
                function_header fh = obj as function_header;
                if (fh.return_type == null)
                {
                    //это прототип
                    if (rtvds != null)
                        fh.return_type = rtvds.ref_type;
                    return fh;
                }
                vds = new var_def_statement();
                parsertools.create_source_context(vds, fh, fh);
                vds.vars = new ident_list();
                vds.vars.idents.Add(fh.name.meth_name);
                parsertools.assign_source_context(vds.vars, fh.name.meth_name);
                fh.name = null;
                if (rtvds != null)
                    rtvds.function_header = fh;
                ref_type rt = (ref_type)fh.return_type, rtp = PrepareRefType(rt);
                rtp.pointed_to = fh;
                vds.vars_type = rt;
                fh.return_type = null;
            }
            if (rtvds != null)
            {
                rtvds.var_def_statement = vds;
                return rtvds;
            }
            else
                return vds;
        }

        public void ConnectElementTypeToArrayType(array_type ar, type_definition elem_type)
        {
            while (ar.elemets_types != null)
            {
                if (ar.elemets_types is array_type)
                    ar = ar.elemets_types as array_type;
                else
                {
                    if (ar.elemets_types is function_header)
                    {
                        errors.Add(new UnexpectedPartOfFunctionHeader(parser.current_file_name,ar.elemets_types.source_context,ar));
                        return;
                    }
                    type_definition td = PrepareRefType((ref_type)ar.elemets_types).pointed_to;
                    if (td is array_type)
                        ar = td as array_type;
                    else
                        if (td is function_header)
                        {
                            (td as function_header).return_type = elem_type;
                            return;
                        }

                }
            }
            ar.elemets_types = elem_type;
        }

        public type_definition SafeConnectElementTypeToArrayType(type_definition arr, type_definition elem_type)
        {
            if (arr == null)
                return elem_type;
            ConnectElementTypeToArrayType((array_type)arr,elem_type);
            return ((array_type)arr).elemets_types;
        }

        public void PrepareVariableDefinitions(declaration std, declarations defs)
        {
            int ii = 0;
            type_definition td=null;
            if (std is type_definition)
                td = (type_definition)std;
            else
            {
                named_type_reference ntr = new named_type_reference();
                type_declaration tdecl=(type_declaration)std;
                ntr.names.Add(tdecl.type_name);
                td = ntr;
                parsertools.assign_source_context(td, tdecl.type_name);
                ii = 1;
                defs.defs.Insert(0,std);
            }
            for (int i=ii; i < defs.defs.Count; i++)
            {
                declaration sd = defs.defs[i];
                if (sd is function_header)
                {
                    (sd as function_header).return_type = SafeConnectToRefType((sd as function_header).return_type as ref_type,td);
                    continue;
                }
                var_def_statement vds = sd as var_def_statement;
                if (vds is ref_type_and_var_def_statement)
                {
                    ref_type_and_var_def_statement rtvd = vds as ref_type_and_var_def_statement;
                    defs.defs.RemoveAt(i);
                    defs.defs.Insert(i, rtvd.var_def_statement);
                    vds = rtvd.var_def_statement;
                    PrepareRefType(rtvd.ref_type).pointed_to = td;
                    if (rtvd.function_header != null)
                        //обрабатывали заголовок функции
                        rtvd.function_header.return_type = rtvd.ref_type;
                    else
                        //обрабатывали varы
                        if(vds.vars_type==null)
                            vds.vars_type = rtvd.ref_type;
                        else
                            if (vds.vars_type is array_type)
                            {
                                ConnectElementTypeToArrayType(vds.vars_type as array_type, rtvd.ref_type);
                            }                            
                }
                else
                {
                    if (vds.vars_type == null)
                        vds.vars_type = td;
                    else
                    {
                        type_definition tdf = SafePrepareRefType(vds.vars_type);
                        if (tdf is function_header)
                            (tdf as function_header).return_type = td;
                        else
                            if (tdf is array_type)
                                ConnectElementTypeToArrayType(tdf as array_type, td);
                            else
                                throw new Exception();
                    }
                        
                    /*else
                    if (vds.vars_type is array_type)
                    {
                        array_type t = vds.vars_type as array_type;
                        while (t.elemets_types != null)
                            t = (array_type)t.elemets_types;
                        t.elemets_types = td;
                    }
                    else
                    {
                        ref_type rt = PrepareRefType((ref_type)vds.vars_type);
                        if (rt.pointed_to == null)
                        {
                            rt.pointed_to = td;
                            parsertools.create_source_context(vds, rt, vds);
                        }
                    }*/
                }
            }
        }

        public void AddToSubporgramDefs(List<declaration> defs, object node)
        {
            if (node is declaration)
                defs.Add((declaration)node);
            else
            {
                declarations sd = (declarations)node;
                foreach (declaration s in sd.defs)
                    defs.Add(s);
            }
        }

        public declaration PrepareUnionOrStructDefinition(token_info keyword, ident name, object lp, class_members members, object rp)
        {
            class_definition cd = new class_definition();
            parsertools.create_source_context(cd, keyword, parsertools.sc_not_null(rp, name));
            if (keyword.text == "struct")
                cd.keyword = class_keyword.Struct;
            else
                cd.keyword = class_keyword.Union;
            if (members != null)
            {
                cd.body = new class_body();
                members.access_mod = new access_modifer_node(access_modifer.none);
                cd.body.class_def_blocks.Add(members);
                parsertools.create_source_context(cd.body, lp,rp);
            }
            if (name != null)
            {
                type_declaration td = new type_declaration();
                td.type_name = name;
                td.type_def = cd;
                parsertools.assign_source_context(td, cd);
                parser.LRParser.SepecialSymbolPrefixDirection.Add(name.name, '$'); 
                return td;
            }
            return cd;

        }

        public syntax_tree_node PreparePointerDirectDeclarator(ref_type reftype, declaration td)
        {
            if (td is var_def_statement)
            {

                ref_type_and_var_def_statement rtvd;
                if (td is ref_type_and_var_def_statement)
                {
                    rtvd = td as ref_type_and_var_def_statement;
                    reftype.pointed_to = rtvd.ref_type;
                    parsertools.create_source_context(rtvd, reftype.pointed_to, rtvd.ref_type);
                    rtvd.ref_type = reftype;
                    return rtvd;
                }
                rtvd = new ref_type_and_var_def_statement();
                rtvd.ref_type = reftype;
                rtvd.var_def_statement = td as var_def_statement;
                parsertools.create_source_context(rtvd, reftype, td);
                return rtvd;
            }
            else
            {
                //int [S(Sname)(...)]
                ref_type rtc = PrepareRefType(reftype);
                rtc.pointed_to = (type_definition)td;
                return reftype;
            }
        }

        public void PrepareDeclaratorList(type_declarations tds, var_def_statement vds)
        {
            type_definition tdf = null;
            if (vds is ref_type_and_var_def_statement)
            {
                tdf = (vds as ref_type_and_var_def_statement).ref_type;
                vds = (vds as ref_type_and_var_def_statement).var_def_statement;
            }
            else
                tdf = vds.vars_type;
            type_declaration td = new type_declaration(vds.vars.idents[0], tdf);
            parsertools.create_source_context(td, parsertools.sc_not_null(td.type_def, td.type_name), td.type_name);
            tds.types_decl.Add(td);
            parsertools.create_source_context(tds, tds, vds);
        }

        public syntax_tree_node PrepareFunctionHeader(declaration td, object lp, formal_parameters fp, object rp)
        {
            function_header fh = new function_header();
            fh.parameters = fp;
            fh.of_object = false;
            fh.class_keyword = false;
            method_name mn;
            if (td is var_def_statement)
            {
                var_def_statement vds = td as var_def_statement;
                ref_type rf = null;
                if (vds is ref_type_and_var_def_statement)
                {
                    rf = (vds as ref_type_and_var_def_statement).ref_type;
                    vds = (vds as ref_type_and_var_def_statement).var_def_statement;
                }
                if (vds.vars_type is array_type)
                {
                    //функция не может возвращать масив поэтому это обьявление переменной
                    array_type ar=vds.vars_type as array_type;
                    ar.elemets_types = SafeConnectElementTypeToArrayType(ar,SafeConnectToRefType(rf, fh));                    
                    parsertools.create_source_context(fh, lp, rp);
                    return vds;            
                }
                mn = new method_name(null, vds.vars.idents[0],null);
                parsertools.assign_source_context(mn, mn.meth_name);
                fh.return_type = SafeConnectToRefType(rf, vds.vars_type);
                fh.name = mn;
                parsertools.create_source_context(fh, td, rp);
                return fh;
            }
            else
            if(td is ref_type)
            {
                //eto type_name ( Sb(...) )(...);
                //1= Sb() 
                function_header ret_type = new function_header();
                ref_type ret_ref = (ref_type)td;
                ref_type ret_ref_pt = PrepareRefType(ret_ref);
                fh = (function_header)ret_ref_pt.pointed_to;
                ret_type.parameters = fp;
                parsertools.create_source_context(ret_type, fp, fp);
                ret_ref_pt.pointed_to = ret_type;
                fh.return_type = ret_ref;
                parsertools.create_source_context(fh, td, rp);
                return fh;
            }
            else
            if (td is function_header)
            {
                errors.Add(new FunctionReturnFuction(parser.current_file_name, (rp as syntax_tree_node).source_context, fh));
                return fh;
            }
            else
            {
                errors.Add(new BadDeclaration(parser.current_file_name, (rp as syntax_tree_node).source_context, fh));
                return fh;
            }
        }

        public typed_parameters PrepareParametrDeclaration(declaration declaration_specifiers, declaration decarator)
        {
            typed_parameters tp = new typed_parameters();
            function_header fh = decarator as function_header;
            var_def_statement vd = decarator as var_def_statement;
            ref_type rt=null;
            if (decarator is ref_type)
            {
                rt = decarator as ref_type;
                fh = PrepareRefType(rt).pointed_to as function_header;
                PrepareRefType(rt).pointed_to = (type_definition)declaration_specifiers;
                declaration_specifiers = rt;
            }            
            if (decarator is ref_type_and_var_def_statement)
            {
                vd = (decarator as ref_type_and_var_def_statement).var_def_statement;
                declaration_specifiers = SafeConnectToRefType((decarator as ref_type_and_var_def_statement).ref_type, (type_definition)declaration_specifiers);
                //parsertools.create_source_context(declaration_specifiers, declaration_specifiers, (decarator as ref_type_and_var_def_statement).ref_type);
            }
            if (vd != null)
            {
                tp.idents = vd.vars;
                if (vd.vars_type is ref_type)
                    rt = (ref_type)vd.vars_type;
                parsertools.create_source_context(tp, declaration_specifiers, decarator);
            }
            else
            {
                tp.idents = new ident_list();
                tp.idents.idents.Add(fh.name.meth_name);
                parsertools.assign_source_context(tp.idents, fh.name.meth_name);
                fh.name = null;
                rt = (ref_type)fh.return_type;
                fh.return_type = (type_definition)declaration_specifiers;
                parsertools.create_source_context(fh, declaration_specifiers, fh);
                declaration_specifiers = fh;
                parsertools.create_source_context(tp, declaration_specifiers, fh);
            }
            if (rt != null)
            {
                ref_type rtc = PrepareRefType(rt);
                rtc.pointed_to = (type_definition)declaration_specifiers;
                if (vd != null)
                    parsertools.create_source_context(rtc, declaration_specifiers, rtc);
                tp.vars_type = rt;
            }
            else
            {
                tp.vars_type = (type_definition)declaration_specifiers;
            }
            return tp;
        }

        public function_header PrepareFunctionDefinitionHeader(type_definition td, declaration func_header)
        {
            function_header fh = func_header as function_header;
            if (func_header is ref_type)
            {
                ref_type rt = (ref_type)func_header, rtc = PrepareRefType(rt);
                fh = rtc.pointed_to as function_header;
                rtc.pointed_to = td;
                td = rt;
                parsertools.create_source_context(td, td, rt);
            }
            if (fh == null)
            {
                fh = new function_header();
                errors.Add(new FunctionHeaderExpected(parser.current_file_name, func_header.source_context, fh));
            }
            if (fh.return_type == null)
                fh.return_type = td;
            else
                if (fh.return_type is ref_type)
                {
                    ref_type rt = PrepareRefType(fh.return_type as ref_type);
                    function_header fh2 = (function_header)rt.pointed_to;
                    fh2.return_type = td;
                }
            parsertools.create_source_context(fh, parsertools.sc_not_null(td,fh), fh);
            return fh;
        }

        public type_definition PrepareTypeSpecifiers(object l, object r)
        {
            c_scalar_type erwp = null;
            if ((l is c_scalar_type) && (r is c_scalar_type))
            {
                c_scalar_type sc1 = l as c_scalar_type, sc2 = r as c_scalar_type;
                if (sc1.scalar_name == c_scalar_type_name.tn_long || sc1.scalar_name == c_scalar_type_name.tn_short)
                {
                    if (sc2.scalar_name == c_scalar_type_name.tn_int)
                    {
                        if (sc1.scalar_name == c_scalar_type_name.tn_long)
                            sc1.scalar_name = c_scalar_type_name.tn_long_int;
                        else
                            sc1.scalar_name = c_scalar_type_name.tn_short_int;
                        parsertools.create_source_context(sc1, sc1, sc2);
                        return sc1;
                    }
                    else
                    {
                        erwp = new c_scalar_type(c_scalar_type_name.tn_void, c_scalar_sign.none);
                        parsertools.create_source_context(erwp,l,r);
                        errors.Add(new IntExpected(parser.current_file_name, sc2.source_context, erwp));
                        return erwp;
                    }
                }
                else
                {
                    erwp = new c_scalar_type(c_scalar_type_name.tn_void, c_scalar_sign.none);
                    parsertools.create_source_context(erwp, l, r);
                    errors.Add(new ShortOrLongExpected(parser.current_file_name, sc1.source_context, erwp));
                    return erwp;
                }

            }
            if (l is c_scalar_sign_token)
                if (r is c_scalar_type)
                {
                    (r as c_scalar_type).sign = (l as c_scalar_sign_token).data;
                    parsertools.create_source_context(r, l, r);
                    return (c_scalar_type)r;
                }
                else
                {
                    erwp = new c_scalar_type(c_scalar_type_name.tn_void, c_scalar_sign.none);
                    parsertools.create_source_context(erwp, l, r);
                    errors.Add(new ScalarTypeNameExpected(parser.current_file_name, (r as syntax_tree_node).source_context, erwp));
                    return erwp;
                }
            if ((l is type_definition_attr) && (r is type_definition))
            {
                type_definition td=r as type_definition;
                type_definition_attr tda = l as type_definition_attr;
                if (td.attr_list == null)
                {
                    td.attr_list = new type_definition_attr_list();
                    parsertools.create_source_context(td.attr_list, l, l);
                }
                td.attr_list.attributes.Insert(0, tda);
                parsertools.create_source_context(td, l, r);
                parsertools.create_source_context(td.attr_list, l, td.attr_list);
                return td;
            }

            erwp = new c_scalar_type(c_scalar_type_name.tn_void, c_scalar_sign.none);
            parsertools.create_source_context(erwp, l, r);
            errors.Add(new BadDeclaration(parser.current_file_name, (r as syntax_tree_node).source_context, erwp));
            return erwp;
        }

    }
}
