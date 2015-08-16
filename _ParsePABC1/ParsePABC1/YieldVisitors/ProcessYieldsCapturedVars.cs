using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

using PascalABCCompiler.ParserTools;
using PascalABCCompiler.Errors;


namespace ParsePABC1
{
    class ProcessYieldCapturedVarsVisitor : BaseChangeVisitor 
    {
        int clnum = 0;

        public string newClassName()
        {
            clnum++;
            return "clyield#" + clnum.ToString();
        }
       
        public FindMainIdentsVisitor mids; // захваченные переменные процедуры по всем её yield 

        public int countNodesVisited;

        public bool hasYields = false;

        public ProcessYieldCapturedVarsVisitor()
        {
            //PrintInfo = false; 
        }

        public override void Enter(syntax_tree_node st)
        {
            base.Enter(st);
            countNodesVisited++;

            // сокращение обходимых узлов. Как сделать фильтр по тем узлам, которые необходимо обходить? Например, все операторы (без выражений и описаний), все описания (без операторов)
            if (st is assign || st is var_def_statement || st is procedure_call || st is procedure_header || st is expression)
            {
                visitNode = false; // фильтр - куда не заходить 
            }
        }

        type_declaration GenClassForYield(procedure_definition pd, IEnumerable<var_def_statement> fields)
        {
            // Теперь на месте процедуры генерируем класс
            var cm = new class_members(access_modifer.public_modifer);
            foreach (var m in fields)
                cm.Add(m);

            var st = new var_def_statement("state", "integer");

            var fh = (pd.proc_header as function_header);
            var seqt = fh.return_type as sequence_type;
            if (seqt == null)
                throw new SyntaxError("Functions with yields must return sequences", "", fh.return_type.source_context, fh.return_type);

            List<ident> lid = new List<ident>();
            var pars = fh.parameters;
            foreach (var ps in pars.params_list)
            {
                if (ps.param_kind != parametr_kind.none)
                    throw new SyntaxError("Parameters of functions with yields must not have 'var', 'const' or 'params' modifier", "", pars.source_context, pars);
                if (ps.inital_value != null)
                    throw new SyntaxError("Parameters of functions with yields must not have initial values", "", pars.source_context, pars);
                var_def_statement vds = new var_def_statement(ps.idents, ps.vars_type);
                cm.Add(vds);
                lid.AddRange(vds.vars.idents);
            }

            var stels = seqt.elements_type;
            var cur = new var_def_statement("current", stels);

            var Constr = new procedure_definition(new constructor(formal_parameters.Empty), block.Empty, null);

            var MoveNext = new procedure_definition("MoveNext", "boolean", pd.proc_body);

            var Reset = new procedure_definition("Reset", statement_list.Empty);

            var GetCurrent = new procedure_definition("get_Current", "object", new assign("Result", "current"));

            var className = newClassName() + "Helper";
            var assG = new assign("Result", new new_expr(new named_type_reference(className),expression_list.Empty));
            var GetEnumerator = new procedure_definition("GetEnumerator", "System.Collections.IEnumerator", assG);

            var stl = new statement_list(assG);
            foreach (var id in lid)
            {
                var ass = new assign(new dot_node(new ident("Result"), id),id);
                stl.Add(ass);
            }
            pd.proc_body = new block(stl);


            cm.Add(st, cur, Constr, Reset, MoveNext, GetCurrent, GetEnumerator);

            var interfaces = new named_type_reference_list("System.Collections.IEnumerator","System.Collections.IEnumerable");
            var td = new type_declaration(className, SyntaxTreeBuilder.BuildClassDefinition(interfaces, cm));
            return td;
        }

        public override void visit(procedure_definition pd)
        {
            hasYields = false;
            if (pd.proc_header is function_header)
                mids = new FindMainIdentsVisitor();

            base.visit(pd);

            if (!hasYields) // т.е. мы разобрали функцию и уже выходим. Это значит, что пока yield будет обрабатываться только в функциях. Так это и надо.
                return;

            var dld = new DeleteLocalDefs(mids.vars); // mids.vars - все захваченные переменные
            pd.visit(dld); // Удалить в локальных и блочных описаниях этой процедуры все захваченные переменные
            // В результате работы в mids.vars что-то осталось. Это не локальные переменные и с ними непонятно что делать
            dld.AfterProcTraverse();

            ChangeWhileVisitor.New.ProcessNode(pd);

            // Конструируем определение класса
            var cc = GenClassForYield(pd, dld.BlockDeletedIds.Union(dld.LocalDeletedIds));
            var cct = new type_declarations(cc);

            var decls = UpperNodeAs<declarations>();
            decls.InsertBefore(pd, cct);

            mids = null; // вдруг мы выйдем из процедуры, не зайдем в другую, а там - оператор! Такого конечно не может быть
        }

        public override void visit(yield_node yn)
        {
            hasYields = true;
            if (mids != null) // если мы - внутри процедуры
                yn.visit(mids);
            else throw new SyntaxError("Yield must be in functions only", "", yn.source_context, yn);
            // mids.vars - надо установить, какие из них - локальные, какие - из этого класса, какие - являются параметрами функции, а какие - глобальные (все остальные)
            // те, которые являются параметрами, надо скопировать в локальные переменные и переименовать использование везде по ходу данной функции 
            // самое сложное - переменные-поля этого класса - они требуют в создаваемом классе, реализующем итератор, хранить Self текущего класса и добавлять это Self везде по ходу алгоритма
            // вначале будем считать, что переменные-поля этого класса и переменные-параметры не захватываются yield
            //base.visit(yn);
        }
    }
}
