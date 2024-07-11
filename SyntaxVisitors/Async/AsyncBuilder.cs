using PascalABCCompiler.SyntaxTree;
using System.Collections.Generic;

namespace SyntaxVisitors.Async
{
    // Билдер для построения StateMachine для каждого асинхронного метода
    internal class AsyncBuilder
    {
        // корень дерева
        private program_module program_Module { get; set; }
        // нужен для перестановки defs, содержащихся в блоке program block 
        public List<declarations> declarationsList = new List<declarations>();
        // список всех асинхронных методов
        public List<procedure_definition> pdList = new List<procedure_definition>();

        // Словарик переменных, которые нужно будет переименовать
        public Dictionary<string, string> RepVarsDict = new Dictionary<string, string>();

        int LabelNameCounter = 0;
        private int StateCounter = 1;

        // копирует блоки построенной StateMachine и передает их в declarationsList
        private declarations Decls = new declarations();

        public AsyncBuilder(program_module program_Module)
        {
            this.program_Module = program_Module;
        }
        // Подключаем необходимые модули
        public void AddUsesCompilerServices()
        {
            if (program_Module.used_units != null)
            {
                program_Module.used_units.Add(new unit_or_namespace(new ident_list(new ident("System"),
                    new ident("Runtime"), new ident("CompilerServices"))), program_Module.used_units.source_context);
            }
            else
            {
                program_Module.used_units = new uses_list(new unit_or_namespace(new ident_list(new ident("System"),
                    new ident("Runtime"), new ident("CompilerServices"))), program_Module.source_context);
            }
        }

        public string NewStateMachine()
        {
            return "@StateMachine@_" + StateCounter++;
        }

        // Создаем StateMachine вручную
        public void BuildStateMachine()
        {
            var defs = program_Module.program_block.defs;
            var vds = new var_def_statement(new ident("state"), new named_type_reference(new ident("integer")), defs.source_context);
            var vds2 = new var_def_statement(new ident("tbuilder"), new named_type_reference(new ident("AsyncVoidMethodBuilder")), defs.source_context);
            var ph = new procedure_header();
            ph.name = new method_name();
            ph.name.meth_name = new ident("MoveNext");
            ph.proc_attributes = new procedure_attributes_list();
            ph.source_context = defs.source_context;
            var ph2 = new procedure_header();
            ph2.name = new method_name();
            ph2.name.meth_name = new ident("SetStateMachine");
            ph2.source_context = defs.source_context;

            var namedtr = new named_type_reference(new ident("System"), defs.source_context);
            namedtr.Add(new ident("Runtime"));
            namedtr.Add(new ident("CompilerServices"));
            namedtr.Add(new ident("IAsyncStateMachine"));
            var tp = new typed_parameters(new ident("stateMachine"), namedtr, defs.source_context);
            ph2.parameters = new formal_parameters(tp);
            ph2.proc_attributes = new procedure_attributes_list();

            List<declaration> members = new List<declaration> { vds, vds2, ph, ph2 };

            var cm = new class_members(members, new access_modifer_node(access_modifer.public_modifer), defs.source_context);
            var classbodylist = new class_body_list(cm, defs.source_context);

            var ntr = new named_type_reference_list(new named_type_reference(new ident("IAsyncStateMachine")), defs.source_context);
            var cd = new class_definition(ntr, classbodylist, defs.source_context);
            var td = new type_declaration(new ident("StateMachine"), cd, program_Module.source_context);

            var pd = new procedure_definition();
            pd.proc_header = ph2;
            var a = new assign(new ident("tbuilder"), new dot_node(new ident("AsyncVoidMethodBuilder"), new ident("Create")), Operators.Assignment, defs.source_context);
            var p = new procedure_call(new method_call(new dot_node(new ident("tbuilder"), new ident("SetStateMachine")), new expression_list(new ident("stateMachine"))), defs.source_context);
            var subnodes = new List<statement> { a, p };
            var stlist = new statement_list(subnodes, new token_info("begin"), new token_info("end"), false, defs.source_context);
            pd.proc_body = new block(new declarations(), stlist, defs.source_context);
            pd.source_context = defs.source_context;



            var pd2 = new procedure_definition();
            pd2.proc_header = ph;
            pd2.source_context = defs.source_context;
            stlist = new statement_list(new empty_statement(), defs.source_context);
            stlist.left_logical_bracket = new token_info("try", defs.source_context);

            a = new assign(new ident("state"), new un_expr(new int32_const(2), Operators.Minus), Operators.Assignment, defs.source_context);
            var e = new new_expr(new named_type_reference(new ident("Exception")), new expression_list(new string_const("MoveNextException")), defs.source_context);
            p = new procedure_call(new method_call(new dot_node(new ident("tbuilder"), new ident("SetException")), new expression_list(e)), defs.source_context);
            var exception_block = new exception_block();
            exception_block.stmt_list = new statement_list(a, p, new procedure_call(new ident("exit")), new empty_statement());
            exception_block.source_context = defs.source_context;
            exception_block.stmt_list.source_context = defs.source_context;
            var try_handler = new try_handler_except(exception_block, defs.source_context);

            var tr = new try_stmt(stlist, try_handler, defs.source_context);
            var mc = new method_call();
            mc.source_context = defs.source_context;
            mc.dereferencing_value = new dot_node(new ident("tbuilder"), new ident("SetResult"), defs.source_context);
            p = new procedure_call(mc, defs.source_context);
            subnodes = new List<statement> { tr, a, p, new empty_statement() };
            stlist = new statement_list(subnodes, new token_info("begin"), new token_info("end"), false);
            stlist.source_context = defs.source_context;
            pd2.proc_body = new block(new declarations(), stlist, defs.source_context);

            Decls.Add(new type_declarations(td, defs.source_context), defs.source_context);
            Decls.Add(pd, defs.source_context);
            Decls.Add(pd2, defs.source_context);
        }


        // Генерируем StateMachine
        public void ParseStateMachine(procedure_definition p)
        {
            var defs = program_Module.program_block.defs;
            BuildStateMachine();

            var name = NewStateMachine();
            var t_name = Decls.list[0] as type_declarations;
            t_name.types_decl[0].type_name = name;

            var tdef2 = t_name.types_decl[0];
            var cd2 = tdef2.type_def as class_definition;
            var bd2 = cd2.body;
            var cm2 = bd2[0] as class_members;

            var ph = new procedure_header();
            ph.name = new method_name();
            ph.name.meth_name = new ident("MoveNext");
            ph.proc_attributes = new procedure_attributes_list();
            ph.source_context = defs.source_context;

            var ph2 = new procedure_header();
            ph2.name = new method_name();
            ph2.name.meth_name = new ident("SetStateMachine", defs.source_context);
            ph2.source_context = defs.source_context;

            var namedtr = new named_type_reference(new ident("System"), defs.source_context);
            namedtr.Add(new ident("Runtime"));
            namedtr.Add(new ident("CompilerServices"));
            namedtr.Add(new ident("IAsyncStateMachine"));
            var tp = new typed_parameters(new ident("stateMachine"), namedtr, defs.source_context);
            ph2.parameters = new formal_parameters(tp, defs.source_context);
            ph2.proc_attributes = new procedure_attributes_list();
            ph2.source_context = defs.source_context;

            cm2.members[2] = ph;
            ph.Parent = cm2;
            cm2.members[3] = ph2;
            ph2.Parent = cm2;

            var pd1 = Decls.list[1] as procedure_definition;
            pd1.proc_header.name.class_name = name;
            var pd2 = Decls.list[2] as procedure_definition;
            pd2.proc_header.name.class_name = name;

            var dc = new declarations();
            dc.Add(Decls.list[0], defs.source_context);
            dc.Add(Decls.list[1], defs.source_context);
            dc.Add(Decls.list[2], defs.source_context);
            dc.Add(p, defs.source_context);

            declarationsList.Add(dc);
            Decls.list.Clear();


        }

        // Для каждого асинхронного метода вызываем ParseStateMachine
        public void ParseStateMachines()
        {
            var defs = program_Module.program_block.defs;
            var t = new List<procedure_definition>();
            foreach (var item in pdList)
            {
                t.Add(item);
                defs.Remove(item);
            }

            t.Reverse();
            foreach (var pd in t)
            {
                ParseStateMachine(pd);
            }
            declarationsList.Reverse();

            foreach (var item in declarationsList)
            {
                defs.AddFirst(item.list);
            }

        }

        // Переставляем асинхронные методы в нужном порядке
        public void SortBlocks()
        {
            var defs = program_Module.program_block.defs;
            var ind = pdList.Count * 4;
            int k = 0;
            for (int i = ind; i < defs.list.Count; i++)
            {
                if (defs.list[i] is procedure_definition)
                {
                    var p = defs.list[i] as procedure_definition;
                    if (p.proc_header.IsAsync)
                        continue;
                    var d = defs.list[i];
                    defs.list.Remove(d);
                    defs.AddFirst(d);
                    k++;

                }
            }
            var sortedNumbers = new List<declaration>();
            for (int i = k - 1; i >= 0; i--)
            {
                sortedNumbers.Add(defs.list[i]);
            }
            for (int i = k; i < defs.list.Count; i++)
            {
                sortedNumbers.Add(defs.list[i]);
            }
            defs.list.Clear();
            foreach (var item in sortedNumbers)
            {
                defs.list.Add(item);
            }



            for (int i = 0; i < defs.list.Count; i++)
            {
                if (defs.list[i] is variable_definitions || defs.list[i] is type_declarations)
                {
                    var d = defs.list[i];
                    defs.list.Remove(d);
                    defs.AddFirst(d);
                }
            }


        }
        public void GetMethods(procedure_definition p)
        {
            pdList.Add(p);
        }
        // Для каждого асинхронного метода вызываем ChangeBody
        public void ChangeBodies()
        {
            StateCounter = 1;
            pdList.Reverse();
            foreach (var item in pdList)
            {
                var b = item.proc_header is function_header;
                var s = "Void";
                if (b)
                {
                    var fh = item.proc_header as function_header;
                    s = fh.return_type.ToString();
                    if (s.Contains("."))
                    {
                        s = s.Substring(s.LastIndexOf('.') + 1);
                    }
                    if (!s.Contains("Task"))
                        throw new SyntaxVisitorError("Возвращаемым типом асинхронного метода должен быть void, Task, Task<T> или аналогичный тип, IAsyncEnumerable<T> или IAsyncEnumerator<T>",
                            item.proc_header.source_context);
                }
                if (s == "Void" || s == "Task")
                {
                    ChangeBody(item, b, "Async" + s + "MethodBuilder");
                }
                else
                {
                    ChangeBody(item, b, "AsyncTaskMethodBuilder" + s.Substring(4));
                }
            }
        }
        public string newLabelName(string old)
        {
            LabelNameCounter++;
            return "@awvar@_" + LabelNameCounter.ToString() + "_" + old;
        }

        // Изменяем тело асинхронной функции, добавляя AsyncBuilder для запуска StateMachine
        public void ChangeBody(procedure_definition pd, bool IsFunc, string Type)
        {
            var state = "@awst@_";
            var block = pd.proc_body as block;

            if (pd.proc_header.IsAsync == false)
            {
                return;
            }

            var i = new ident(state, block.source_context);
            var v = new var_def_statement(i, new new_expr(NewStateMachine()), block.source_context); // st:= new StateMachine();
            var st = new var_statement(v, block.source_context); // st:= new StateMachine();
            var d = new dot_node(i, new ident("state"), block.source_context);
            var a = new assign(d, new int32_const(1), Operators.Assignment, block.source_context);//st.state := 1;

            var parsList = new statement_list();
            parsList.source_context = block.source_context;
            if (pd.proc_header.parameters != null)
            {
                var par = pd.proc_header.parameters;
                foreach (var pp in par.params_list)
                {

                    foreach (var id in pp.idents.list)
                    {
                        if (!RepVarsDict.ContainsKey(id.name))
                        {
                            var nv = newLabelName(id.name);
                            RepVarsDict.Add(id.name, nv);
                            parsList.Add(new assign(new dot_node(i, id), id, Operators.Assignment), block.source_context);
                        }
                        else
                        {
                            var nv = newLabelName(id.name);
                            RepVarsDict[id.name] = nv;
                            parsList.Add(new assign(new dot_node(i, id), id, Operators.Assignment), block.source_context);
                        }
                    }
                }
            }
            parsList.source_context = block.source_context;




            var fr2 = new method_call();
            fr2.source_context = block.source_context;


            if (Type.StartsWith("AsyncVoidMethodBuilder") || Type == "AsyncTaskMethodBuilder")
            {
                fr2.dereferencing_value = new dot_node(new ident(Type), new ident("Create"), block.source_context);
            }
            else
            {
                var ttt = Type.Substring(23);
                ttt = ttt.Replace(">", "");
                var tpl = new template_param_list();
                tpl.source_context = block.source_context;
                tpl.params_list.Add(new named_type_reference(new ident(ttt, block.source_context)));
                fr2.dereferencing_value = new dot_node(new ident_with_templateparams(new ident("AsyncTaskMethodBuilder", block.source_context), tpl, block.source_context), new ident("Create", block.source_context), block.source_context);
            }

            var a2 = new assign(new dot_node(i, new ident("tbuilder", block.source_context)), fr2, Operators.Assignment, block.source_context);//st.tbuilder := AsyncTaskMethodBuilder.Create();

            var m = new method_call(new expression_list(i), block.source_context);
            m.dereferencing_value = new dot_node(new dot_node(i, new ident("tbuilder", block.source_context)), new ident("Start", block.source_context), block.source_context);
            var p = new procedure_call(m, block.source_context);//st.tbuilder.Start(st);

            var temp_def = new declarations();
            var defsCount = 0;
            if (block.defs != null)
            {
                foreach (var dl in block.defs.list)
                {
                    if (dl is variable_definitions)
                    {
                        defsCount++;
                        var ddl = dl as variable_definitions;
                        foreach (var item in ddl.list)
                        {
                            (pd.proc_body as block).program_code.AddFirst(new var_statement(item));
                        }
                    }
                    else
                        temp_def.Add(dl);
                }
            }

            if (defsCount > 0)
            {
                block.defs = temp_def;
                if (temp_def != null)
                    foreach (var dl in temp_def.list)
                    {
                        dl.Parent = block.defs;
                    }
            }
            var stl = new statement_list((pd.proc_body as block).program_code, st, a, parsList, a2);


            var class_name = pd.proc_header.name.class_name;
            if (class_name != null)
            {
                if (!pd.proc_header.class_keyword)
                {
                    var ascl = new assign(new dot_node(i, "@awclass", pd.source_context), new ident("self", pd.source_context), pd.source_context); // st:= new StateMachine();
                    stl.Add(ascl);
                }
            }
            stl.Add(p);
            if (IsFunc)
            {
                var a3 = new assign(new ident("Result", pd.source_context), new dot_node(new dot_node(new ident(state, block.source_context), new ident("tbuilder", block.source_context)), new ident("Task", block.source_context)), Operators.Assignment, pd.source_context);
                stl.Add(a3);
            }
            stl.source_context = pd.source_context;
            pd.proc_body = new block(stl);
            var tt = pd.proc_body as block;
            tt.defs = block.defs;
            if (block.defs != null)
            {
                foreach (var item in block.defs.list)
                {
                    item.Parent = tt.defs;
                }
            }




        }
    }
}
