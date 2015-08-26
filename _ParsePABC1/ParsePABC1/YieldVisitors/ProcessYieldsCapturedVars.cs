using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

using PascalABCCompiler.ParserTools;
using PascalABCCompiler.Errors;


namespace SyntaxVisitors
{
    public static class Consts
    {
        public static string Current = "<>current";
        public static string State = "<>state";
    }

    public class ProcessYieldCapturedVarsVisitor : BaseChangeVisitor 
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

        public static ProcessYieldCapturedVarsVisitor New
        {
            get { return new ProcessYieldCapturedVarsVisitor(); }
        }

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

        type_declarations GenClassesForYield(procedure_definition pd, IEnumerable<var_def_statement> fields)
        {
            var fh = (pd.proc_header as function_header);
            if (fh == null)
                throw new SyntaxError("Only functions can contain yields", "", pd.proc_header.source_context, pd.proc_header);
            var seqt = fh.return_type as sequence_type;
            if (seqt == null)
                throw new SyntaxError("Functions with yields must return sequences", "", fh.return_type.source_context, fh.return_type);

            // Теперь на месте функции генерируем класс

            // Захваченные переменные
            var cm = class_members.Public;
            foreach (var m in fields)
                cm.Add(m);

            // Параметры функции
            List<ident> lid = new List<ident>();
            var pars = fh.parameters;
            if (pars != null)
                foreach (var ps in pars.params_list)
                {
                    if (ps.param_kind != parametr_kind.none)
                        throw new SyntaxError("Parameters of functions with yields must not have 'var', 'const' or 'params' modifier", "", pars.source_context, pars);
                    if (ps.inital_value != null)
                        throw new SyntaxError("Parameters of functions with yields must not have initial values", "", pars.source_context, pars);
                    var_def_statement vds = new var_def_statement(ps.idents, ps.vars_type);
                    cm.Add(vds); // все параметры функции делаем полями класса
                    lid.AddRange(vds.vars.idents);
                }

            var stels = seqt.elements_type;

            // Системные поля и методы для реализации интерфейса IEnumerable
            cm.Add(new var_def_statement(Consts.State, "integer"),
                new var_def_statement(Consts.Current, stels),
                procedure_definition.EmptyDefaultConstructor,
                new procedure_definition("Reset"),
                new procedure_definition("MoveNext", "boolean", pd.proc_body),
                new procedure_definition("get_Current", "object", new assign("Result", Consts.Current)),
                new procedure_definition("GetEnumerator", "System.Collections.IEnumerator", new assign("Result", "Self"))
                );

            var className = newClassName();
            var classNameHelper = className + "Helper";

            var interfaces = new named_type_reference_list("System.Collections.IEnumerator", "System.Collections.IEnumerable");
            var td = new type_declaration(classNameHelper, SyntaxTreeBuilder.BuildClassDefinition(interfaces, cm));

            // Изменение тела процедуры

            var stl = new statement_list(new var_statement("res", new new_expr(className)));
            stl.AddMany(lid.Select(id => new assign(new dot_node("res", id), id)));
            stl.Add(new assign("Result", "res"));
            pd.proc_body = new block(stl);

            // Второй класс

            var tpl = new template_param_list(stels);

            var IEnumeratorT = new template_type_reference("System.Collections.Generic.IEnumerator", tpl);

            var cm1 = class_members.Public.Add(
                procedure_definition.EmptyDefaultConstructor,
                new procedure_definition(new function_header("get_Current", stels), new assign("Result", Consts.Current)),
                new procedure_definition(new function_header("GetEnumerator", IEnumeratorT), new assign("Result", "Self")),
                new procedure_definition("Dispose")
            );

            var interfaces1 = new named_type_reference_list(classNameHelper);
            var IEnumerableT = new template_type_reference("System.Collections.Generic.IEnumerable", tpl);

            interfaces1.Add(IEnumerableT).Add(IEnumeratorT);

            var td1 = new type_declaration(className, SyntaxTreeBuilder.BuildClassDefinition(interfaces1, cm1));

            var cct = new type_declarations(td);
            cct.Add(td1);

            return cct;
        }

        public override void visit(procedure_definition pd)
        {
            hasYields = false;
            if (pd.proc_header is function_header)
                mids = new FindMainIdentsVisitor();

            base.visit(pd);

            if (!hasYields) // т.е. мы разобрали функцию и уже выходим. Это значит, что пока yield будет обрабатываться только в функциях. Так это и надо.
                return;

            var dld = new DeleteAllLocalDefs(); // mids.vars - все захваченные переменные
            pd.visit(dld); // Удалить в локальных и блочных описаниях этой процедуры все переменные и вынести их в отдельный список var_def_statement

            mids.vars.Except(dld.LocalDeletedDefsNames); // параметры остались. Их тоже надо исключать - они и так будут обработаны
            // В результате работы в mids.vars что-то осталось. Это не локальные переменные и с ними непонятно что делать

            LoweringVisitor.Accept(pd);

            var cfa = new ConstructFiniteAutomata((pd.proc_body as block).program_code);
            cfa.Transform();
            (pd.proc_body as block).program_code = cfa.res;

            // Конструируем определение класса
            var cct = GenClassesForYield(pd, dld.LocalDeletedDefs); // все удаленные описания переменных делаем описанием класса

            UpperNodeAs<declarations>().InsertBefore(pd, cct);

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

    class ConstructFiniteAutomata
    {
        public statement_list res = new statement_list();
        statement_list stl;
        int curState = 0;

        statement_list curStatList;
        statement_list StatListAfterCase = new statement_list();

        case_node cas; // формируемый case

        public ConstructFiniteAutomata(statement_list stl)
        {
            this.stl = stl;
        }

        public void Process(statement st)
        {
            if (!(st is yield_node || st is labeled_statement))
            {
                curStatList.Add(st);
            }
            if (st is yield_node)
            {
                var yn = st as yield_node;
                curState += 1;
                curStatList.AddMany(
                    new assign(Consts.Current, yn.ex),
                    new assign(Consts.State, curState),
                    new assign("Result", true),
                    new procedure_call("exit")
                );

                curStatList = new statement_list();
                case_variant cv = new case_variant(new expression_list(new int32_const(curState)), curStatList);
                cas.conditions.variants.Add(cv);
            }
            if (st is labeled_statement)
            {
                var ls = st as labeled_statement;
                curStatList = StatListAfterCase;
                curStatList.Add(new labeled_statement(ls.label_name));
                Process(ls.to_statement);
            }
        }

        public void Transform()
        {
            cas = new case_node(new ident(Consts.State));

            curStatList = new statement_list();
            case_variant cv = new case_variant(new expression_list(new int32_const(curState)), curStatList);
            cas.conditions.variants.Add(cv);

            foreach (var st in stl.subnodes)
                Process(st);

            stl.subnodes = BaseChangeVisitor.SeqStatements(cas, StatListAfterCase).ToList();
            //statement_list res = new statement_list(cas);
            res = stl;
        }
    }

}
