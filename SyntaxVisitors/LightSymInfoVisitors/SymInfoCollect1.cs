using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
    ProgramScopeSyntax
        BlockScopeSyntax
            ProcScopeSyntax (name)
                ParamsScopeSyntax
                    BlockScopeSyntax
                        StatListScopeSyntax (0)
                            StatListScopeSyntax (1)
            StatListScopeSyntax (0)
                StatListScopeSyntax (1)
*/


namespace PascalABCCompiler.SyntaxTree
{
    // Визитор накопления легковесной синтаксической таблицы символов. 
    // Не дописан. Имеет большой потенциал применения
    public partial class CollectLightSymInfoVisitor : BaseEnterExitVisitor
    {
        public ScopeSyntax Root;
        public ScopeSyntax Current;

        public static CollectLightSymInfoVisitor New => new CollectLightSymInfoVisitor();
        public override void Enter(syntax_tree_node st)
        {
            ScopeSyntax t = null;
            switch (st)
            {
                case program_module p:
                case unit_module u:
                    t = new GlobalScopeSyntax();
                    Root = t;
                    break;
                case procedure_definition p:
                    var name = p.proc_header?.name?.meth_name;
                    var attr = p.proc_header.class_keyword ? Attributes.class_attr : 0;
                    if (p.proc_header is function_header && name != null)
                        AddSymbol(name, SymKind.funcname,null, attr);
                    else AddSymbol(name, SymKind.procname, null, attr);
                    t = new ProcScopeSyntax(name);
                    break;
                case formal_parameters p:
                    t = new ParamsScopeSyntax();
                    break;
                case statement_list p:
                    t = new StatListScopeSyntax();
                    break;
                case for_node p:
                    t = new ForScopeSyntax();
                    break;
                case foreach_stmt p:
                    t = new ForeachScopeSyntax();
                    break;
                /*case repeat_node p: // не надо т.к. это StatListScope
                    t = new RepeatScopeSyntax();
                    break;*/
                case case_node p:
                    t = new CaseScopeSyntax();
                    break;
                case class_definition p:
                    var td = p.Parent as type_declaration;
                    var tname = td==null ? "NONAME" : td.type_name;
                    if (p.keyword == class_keyword.Class)
                    {
                        AddSymbol(tname, SymKind.classname);
                        t = new ClassScopeSyntax(tname);
                    }                        
                    else if (p.keyword == class_keyword.Record)
                    {
                        AddSymbol(tname, SymKind.recordname);
                        t = new RecordScopeSyntax(tname);
                    }                        
                    else if (p.keyword == class_keyword.Interface)
                    {
                        AddSymbol(tname, SymKind.interfacename);
                        t = new InterfaceScopeSyntax(tname);
                    }                        
                    break;
                case function_lambda_definition p:
                    t = new LambdaScopeSyntax();
                    break;
            }
            if (t != null)
            {
                t.Parent = Current;
                if (Current != null)
                    Current.Children.Add(t);
                Current = t;
                if (st is procedure_definition p)
                {
                    if (p.proc_header is function_header fh)
                    {
                        AddSymbol(new ident("Result"), SymKind.var, fh.return_type);
                    }
                }
            }
        }
        public override void Exit(syntax_tree_node st)
        {
            switch (st)
            {
                case program_module p:
                case procedure_definition pd:
                case formal_parameters fp:
                case statement_list stl:
                case for_node f:
                case foreach_stmt fe:
                case class_definition cd:
                case record_type rt:
                case function_lambda_definition fld:
                //case repeat_node rep:
                case case_node cas:
                    Current = Current.Parent;
                    break;
            }
        }
        public override void visit(var_def_statement vd)
        {
            var attr = vd.var_attr == definition_attribute.Static ? Attributes.class_attr : 0;
            if (vd == null || vd.vars == null || vd.vars.list == null)
                return;
            var type = vd.vars_type;
            var q = vd.vars.list.Select(x => new SymInfoSyntax(x, SymKind.var, type, attr));
            if (q.Count() > 0)
                Current.Symbols.AddRange(q);
            base.visit(vd);
        }
        public override void visit(formal_parameters fp)
        {
            foreach (var pg in fp.params_list)
            {
                var type = pg.vars_type;
                var q = pg.idents.idents.Select(x => new SymInfoSyntax(x, SymKind.param, type));
                Current.Symbols.AddRange(q);
            }
            base.visit(fp);
        }

        public override void visit(for_node f)
        {
            if (f.create_loop_variable || f.type_name != null)
                AddSymbol(f.loop_variable, SymKind.var, f.type_name);
            base.visit(f);
        }
    }
}

