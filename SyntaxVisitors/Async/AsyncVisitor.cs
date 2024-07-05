using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors.Async;

namespace SyntaxVisitors
{
    public class AsyncVisitor : BaseChangeVisitor
    {
        private program_module program_Module { get; set; }

        // текущий асинхронный метод, для которого будет построена StateMachine
        private procedure_definition proc_def { get; set; }

        // список всех асинхронных методов
        private List<procedure_definition> proc_def_List = new List<procedure_definition>();

        // Тип билдера, который нужно менять в зависимости от возвращаемого
        // типа асинхронного типа
        private string BuilderType = "AsyncVoidMethodBuilder";

        private int AwaiterCounter = 1;

        private AwaitBuilder AwaitBuilder;

        private AsyncBuilder AsyncBuilder;

        private bool IsFirstAwait = true;

        private bool IsFirstAsync = true;

        // Нужен, чтобы понимать с какой машиной
        // состояний мы сейчас работаем
        private int StateMachineNumber = 0;

        // Множество всех процедур
        public static HashSet<string> ProcedureSet = new HashSet<string>();

        // Множество всех функций
        public static HashSet<string> FunctionSet = new HashSet<string>();


        public static AsyncVisitor New
        {
            get { return new AsyncVisitor(); }
        }

        public override void visit(procedure_definition pd)
        {
            if (pd.proc_header is function_header)
            {
                var ph = pd.proc_header as function_header;
                if (ph.name != null)
                {
                    FunctionSet.Add(ph.name.meth_name.name);
                }
            }
            else
            {
                var ph = pd.proc_header;
                if (ph.name != null)
                {
                    ProcedureSet.Add(ph.name.meth_name.name);

                }
            }
            // Собираем все асинхронные методы в proc_def_List
            if (pd.proc_header.IsAsync)
            {
                MainVisitor.HasAwait = false;
                MainVisitor.Accept(pd);
                if (!MainVisitor.HasAwait)
                {
                    var b = pd.proc_body as block;
                    if (b != null)
                    {
                        var await = new await_node_statement();
                        await.source_context = b.source_context;
                        await.aw = new await_node();
                        await.aw.source_context = b.source_context;
                        var mc = new method_call();
                        mc.dereferencing_value = new dot_node(new dot_node(new dot_node(new dot_node(new ident("System"),
                            new ident("Threading")), new ident("Tasks")), new ident("Task")), new ident("Delay"));
                        mc.parameters = new expression_list(new int32_const(0));
                        mc.source_context = b.source_context;

                        await.aw.ex = mc;
                        b.program_code.list.Add(await);
                    }
                }
                proc_def = pd;
                proc_def_List.Add(pd);
                if (IsFirstAsync)
                {

                    IsFirstAsync = false;
                    AsyncBuilder = new AsyncBuilder(program_Module);
                    AsyncBuilder.AddUsesCompilerServices();
                }
                LoweringAsyncVisitor.Accept(pd);
                AsyncBuilder.GetMethods(pd);
            }
            else
            {
                if (pd.proc_body is block)
                {
                    var procedure_code = (pd.proc_body as block).program_code;
                    for (int i = 0; i < procedure_code.list.Count; i++)
                    {
                        if (procedure_code.list[i] is await_node_statement)
                        {
                            throw new SyntaxVisitorError("Ключевое слово 'await' может быть использовано " +
                                "только в асинхронных методах ", procedure_code.list[i].source_context);
                        }
                        if (procedure_code.list[i] is var_statement)
                        {
                            var pp = procedure_code.list[i] as var_statement;
                            if (pp.var_def.inital_value is await_node)
                            {
                                throw new SyntaxVisitorError("Ключевое слово 'await' может быть использовано " +
                                    "только в асинхронных методах ", pp.var_def.inital_value.source_context);
                            }
                        }
                    }
                }
            }
        }
        public override void visit(await_node_statement astat)
        {
            ProcessNode(astat.aw);
        }

        public override void visit(await_node a)
        {
            // сразу обернуть в sugared_addressed_value
            var sug = new sugared_expression(a, a.ex, a.ex.source_context);
            ReplaceUsingParent(a, sug);
            AwaiterCounter++;
            if (IsFirstAwait)
            {
                IsFirstAwait = false;
                AwaitBuilder = new AwaitBuilder(program_Module, proc_def);
                AwaitBuilder.VarsHelper.RepVarsDict = AsyncBuilder.RepVarsDict;
                AwaitBuilder.GetCode();
                AwaitBuilder.AddAwaiter(true, a.ex, StateMachineNumber);
                AwaitBuilder.ChangeBuilder(BuilderType, StateMachineNumber);
            }
            else
            {
                AwaitBuilder.ChangeBuilder(BuilderType, StateMachineNumber);
                AwaitBuilder.AddAwaiter(false, a.ex, StateMachineNumber);
            }
            if (AwaitBuilder.AwaiterCounter == AwaiterCounter - 1)
            {

                AwaitBuilder.GenAwait(StateMachineNumber + 2);
                AwaitBuilder.DeleteBody();
            }
        }
        public void GenMain(program_module pm)
        {
            if (MainVisitor.HasAwait)
            {
                var d = pm.program_block.defs;
                var p = new procedure_definition();
                p.proc_header = new function_header(new template_type_reference(new named_type_reference(new ident("Task",
                    d.source_context), d.source_context),
                    new template_param_list(new named_type_reference(new ident("integer", d.source_context),
                    d.source_context), d.source_context), d.source_context), d.source_context);
                p.proc_header.name = new method_name();
                p.proc_header.name.meth_name = new ident("@AsyncMain", d.source_context);
                p.proc_header.IsAsync = true;
                p.source_context = d.source_context;
                var b = new block();
                b.source_context = d.source_context;
                b.program_code = new statement_list();
                b.program_code.source_context = d.source_context;
                foreach (var item in pm.program_block.program_code.list)
                {
                    b.program_code.list.Add(item);
                    item.Parent = b.program_code;
                }
                p.proc_body = b;
                pm.program_block.program_code.list.Clear();
                d.Add(p);
                var mc = new method_call();
                mc.dereferencing_value = new ident("@AsyncMain");
                mc.source_context = d.source_context;
                var vst = new var_statement(new var_def_statement(new ident("@aw_main",
                    d.source_context), mc, d.source_context), d.source_context);
                var pc = new procedure_call(new dot_node(new dot_node(new ident("@aw_main", d.source_context),
                    new ident("GetAwaiter")), new ident("GetResult"), d.source_context), d.source_context);
                pm.program_block.program_code.list.Add(vst);
                pm.program_block.program_code.list.Add(pc);

            }
        }
        public override void visit(program_module pm)
        {
            program_Module = pm;
            MainVisitor.HasAwait = false;
            MainVisitor.Accept(pm.program_block.program_code);
            GenMain(pm);

            DefaultVisit(pm);
            if (proc_def_List.Count > 0)
            {
                AsyncBuilder.ParseStateMachines();
                AsyncBuilder.ChangeBodies();
                proc_def_List.Reverse();
                foreach (var p in proc_def_List)
                {
                    AwaiterCounter = 1;
                    IsFirstAwait = true;
                    proc_def = p;
                    if (p.proc_header.IsAsync)
                    {
                        if (p.proc_header is function_header)
                        {
                            var fh = p.proc_header as function_header;
                            var s = fh.return_type.ToString();
                            if (s.Contains("."))
                            {
                                s = s.Substring(s.LastIndexOf('.') + 1);
                            }
                            if (s.StartsWith("Task"))
                            {
                                BuilderType = "AsyncTaskMethodBuilder" + s.Substring(4);
                            }
                            else
                                BuilderType = "Async" + fh.return_type.ToString() + "MethodBuilder";
                        }
                        else
                        {
                            BuilderType = "AsyncVoidMethodBuilder";


                        }
                    }

                    DefaultVisit(p);
                    StateMachineNumber += 4;
                }
                AsyncBuilder.SortBlocks();

            }
        }
    }
}
