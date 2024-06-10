using PascalABCCompiler.SyntaxTree;
using System.Collections.Generic;
using System.Linq;


namespace SyntaxVisitors.Async
{
	internal class AwaitBuilder
	{
		// корень дерева
		private program_module program_Module { get; set; }

		// текущий асинхронный метод
		public procedure_definition proc_def { get; set; }

		int lbnum = 0;

		// тело асинхронного метода
		private statement_list code_list = new statement_list();

		// Список Taskов, принадлежащих к какому-нибудь await
		public expression_list TaskList = new expression_list();

		// Счетчик awaitов
		public int AwaiterCounter = 0;

        // Счетчик временных переменных для Task
        public int TempTaskCounter = 1;

		public VarsHelper VarsHelper;

		// Возвращаемый тип асинхронного метода
		public string ResultType = "";

        // Результат работы асинхронного метода
        public expression ResultVal = new expression();

        //ctor
        public AwaitBuilder(program_module pm, procedure_definition pd)
		{
			program_Module = pm;
			proc_def = pd;
			VarsHelper = new VarsHelper();

		}
        public int NewAwaiterCounter = 1;
        public string NewAwaiter()
		{
			return "@aw@_" + NewAwaiterCounter++;
		}
		public string NewTempTask()
		{
			return "@tt@_" + TempTaskCounter++;
		}
		// для каждого await добавляем awaiter
		public void AddAwaiter(bool IsFirstTime, expression ex, int c)
		{

			var a_name = NewAwaiter();
			TaskList.AddFirst(ex);
			var tdecl = program_Module.program_block.defs.list[c] as type_declarations;
			var tdef = tdecl.types_decl[0];
			var cd = tdef.type_def as class_definition;
			var bd = cd.body;
			var tn = NewTempTask();
			var vds = new var_def_statement(new ident(a_name,bd.source_context), new named_type_reference(new ident("string",bd.source_context)),bd.source_context); 
			var class_name = proc_def.proc_header.name.class_name;
			if (IsFirstTime)
			{
				List<declaration> members = new List<declaration> { vds };
				if (class_name != null)
				{
					members.Add(new var_statement(new var_def_statement(new ident("@awclass",bd.source_context), new named_type_reference(class_name),bd.source_context),bd.source_context));

				}
				var cm = new class_members(members, new access_modifer_node(access_modifer.private_modifer), bd.source_context);
				bd.AddFirst(cm);

			}
			else
			{
				var cm2 = bd.class_def_blocks[0];
				cm2.members.Add(vds);
			}
		}
		// Меняем тип AsyncBuilder
		public void ChangeBuilder(string Type, int ChangeBuilderCounter)
		{
			var tdecl = program_Module.program_block.defs.list[ChangeBuilderCounter] as type_declarations;
			var tdef = tdecl.types_decl[0];
			var cd = tdef.type_def as class_definition;
			var bd = cd.body;
			var cm = bd[1] as class_members;
			var vdf = cm[2] as var_def_statement;
			var pd = program_Module.program_block.defs.list[ChangeBuilderCounter + 1] as procedure_definition;
			var pb = pd.proc_body as block;
			var st = pb.program_code;
			var a = st.list[0] as assign;
			var mc = new method_call();
			mc.source_context = st.source_context;

			if (Type.StartsWith("AsyncVoidMethodBuilder") || Type == "AsyncTaskMethodBuilder")
			{

				vdf.vars_type = new named_type_reference(Type, vdf.source_context);
				mc.dereferencing_value = new dot_node(new ident(Type, vdf.source_context), new ident("Create"));
			}
			else
			{
				var tt = Type.Substring(23);
				tt = tt.Replace(">", "");
				ResultType = tt;
				var tpl = new template_param_list();
				tpl.source_context = vdf.source_context;
				tpl.params_list.Add(new named_type_reference(new ident(tt, vdf.source_context)));
				vdf.vars_type = new template_type_reference("AsyncTaskMethodBuilder", tpl);
				mc.dereferencing_value = new dot_node(new ident_with_templateparams(new ident("AsyncTaskMethodBuilder", vdf.source_context), 
					tpl, vdf.source_context), new ident("Create", vdf.source_context),vdf.source_context);
			}

			a.from = mc;
		}

		public string newLabelName()
		{
			lbnum++;
			return "@awlb@_" + lbnum.ToString();
		}

		// Обрабатываем тело асинхронного метода
		public void GetCode()
		{
			var class_name = proc_def.proc_header.name.class_name;
			if (class_name != null)
			{
				foreach (var item in program_Module.program_block.defs.list)
				{
					if (item is type_declarations)
					{
						var tds = item as type_declarations;
						foreach (var tdef in tds.types_decl)
						{
							if (tdef.type_name.name == class_name.name)
							{
								var cd = tdef.type_def as class_definition;
								var bd = cd.body;
								for (int i = 0; i < bd.Count; i++)
								{
									if (bd[i] is class_members)
									{
										var cm = bd[i] as class_members;
										foreach (var mem in cm.members)
										{
											if (mem is var_def_statement)
											{
												var vm = mem as var_def_statement;
												if (vm.var_attr != definition_attribute.Static)
												{
													foreach (var vms in vm.vars.list)
													{
														VarsHelper.ClassIdentSet.Add(vms.name);
													}
												}
												else
												{
													foreach (var vms in vm.vars.list)
													{
														VarsHelper.ClassStaticSet.Add(vms.name);
													}

												}


											}
										}
									}

								}
							}
						}
					}
				}
			}

			var b = proc_def.proc_header is function_header;
			var procedure_code = (proc_def.proc_body as block).program_code.list[0] as statement_list;
			var temp_list = new statement_list();
            var r_list = new statement_list();


            if (proc_def.proc_header.parameters != null)
			{
				var par = proc_def.proc_header.parameters;
				foreach (var p in par.params_list)
				{
					if (p.param_kind is parametr_kind.var_parametr)
					{
						throw new SyntaxVisitorError("var-параметры нельзя использовать в асинхронных методах ", p.source_context);
					}
					foreach (var id in p.idents.list)
					{
						var v = new var_statement(new var_def_statement(id, p.vars_type, p.source_context), p.source_context);
						VarsHelper.VarsList.Add(v);
						VarsHelper.Parametrs.Add(id.name);
					}
				}
			}

			var await_counter = 0;
			bool flag = true;
			for (int i = 0; i < procedure_code.list.Count; i++)
			{
			  if (procedure_code.list[i] is await_node_statement)
			    {
					var pp = procedure_code.list[i] as await_node_statement;
                    await_counter++;
                    flag = false;
                    var a = pp.aw;
                    if (a.ex is ident)
                    {
                        VarsHelper.ExprHash.Add(a.ex.ToString());
                        if (!VarsHelper.TaskIdents.ContainsKey(a.ex.ToString()))
                        {
                            VarsHelper.TaskIdents.Add(a.ex.ToString(), await_counter.ToString());
                        }

                    }
                }

				if (procedure_code.list[i] is var_statement)
				{
					var pp = procedure_code.list[i] as var_statement;
					if (pp.var_def.inital_value is await_node)
					{
                        var a = pp.var_def.inital_value as await_node;
						VarsHelper.VarsList.Add(pp);
						foreach (var v in pp.var_def.vars.list)
						{
							var mmc = new method_call();
							mmc.source_context = v.source_context;
                            if (a.ex is method_call)
							mmc.dereferencing_value = new dot_node(new dot_node(a.ex as method_call, new ident("GetAwaiter", v.source_context)),
								new ident("GetResult"), v.source_context);
							else
                                mmc.dereferencing_value = new dot_node(new dot_node(new ident(a.ex.ToString(), v.source_context), 
									new ident("GetAwaiter"), v.source_context), new ident("GetResult"), v.source_context);

                            var ass = new assign(v, mmc, Operators.Assignment, v.source_context);
							ass.first_assignment_defines_type = true;
							//temp_list.Add(ass);
							r_list.Add(ass);
						}
						await_counter++;
						flag = false;
						
						if (a.ex is ident)
						{
							VarsHelper.ExprHash.Add(a.ex.ToString());
							if (!VarsHelper.TaskIdents.ContainsKey(a.ex.ToString()))
							{
								VarsHelper.TaskIdents.Add(a.ex.ToString(), await_counter.ToString());
							}

						}
					}
					else
					{
						VarsHelper.VarsList.Add(pp);
						if (pp.var_def.inital_value != null)
						{
							foreach (var item in pp.var_def.vars.list)
							{
								var ass = new assign(item, pp.var_def.inital_value, Operators.Assignment,item.source_context);
								ass.first_assignment_defines_type = true;
								temp_list.Add(ass);
							}
						}
						continue;
					}
				}

				if (procedure_code.list[i] is assign && b)
				{
					var pa = procedure_code.list[i] as assign;
					if (pa.to is ident)
					{
						var pat = pa.to as ident;
						if (pat.name == "Result")
						{
							ResultVal = pa.from;
						}
					}
				}
				if (flag)
					temp_list.Add(procedure_code.list[i]);
				else
				{
					flag = true;
					code_list.Add(temp_list);
					temp_list = r_list;
                    r_list = new statement_list();
                }
			}
			VarsHelper.MarkVars();
			AwaiterCounter = await_counter;
			code_list.Add(temp_list);
			code_list.source_context = procedure_code.source_context;

		}

		// Изменяем метод MoveNext в зависимости от количества await
		public void GenAwait(int c)
		{
			var pd = program_Module.program_block.defs.list[c] as procedure_definition;
			var bl = pd.proc_body as block;

			var class_name = proc_def.proc_header.name.class_name;
			var awaiter = "@aw@_";
			var aw_temp = "@awtemp@_";
			var extp = "awextemp@_";

			// используется если await внутри цикла
			if ((proc_def.proc_body as block).defs != null)
			{
                var loopLabelsList = (proc_def.proc_body as block).defs.list;
				if (loopLabelsList != null)
                foreach (var b in loopLabelsList)
                {
                    bl.defs.Add(b);
                }
            }

			var lb1 = newLabelName();
			bl.defs.Add(new label_definitions(new ident(lb1,bl.source_context)),bl.source_context);
			var ts = bl.program_code.list[0] as try_stmt;
            var g1 = new goto_statement(lb1,ts.source_context);
            var ls = new labeled_statement(lb1);
			ls.source_context = ts.source_context;
            (code_list.list[1] as statement_list).source_context = ts.source_context;

			ts.stmt_list.Remove(ts.stmt_list.Last());


			// если метод возвращает значение
			if (ResultType != "")
			{
				var r_pc = new procedure_call(new method_call(new dot_node(new ident("tbuilder",ts.source_context),
				new ident("SetResult", ts.source_context)),
				new expression_list(new ident("@aw_res", ts.source_context), ts.source_context), ts.source_context), ts.source_context);
				bl.program_code.list[2] = r_pc;
			}

			////// if (state == 1)

			var ifnode = new if_node();
			ifnode.source_context = ts.source_context;
			ifnode.condition = new bin_expr(new dot_node(new ident("self"), new ident("state")), new int32_const(1), Operators.Equal,ts.source_context);

			var extemp = new var_statement(new var_def_statement(new ident(extp,ts.source_context), TaskList[0] as expression,ts.source_context),ts.source_context);
			(code_list.list[0] as statement_list).Add(extemp);

			var mcс = new method_call();
			mcс.dereferencing_value = new dot_node(new ident(extp, ifnode.source_context), new ident("GetAwaiter",ifnode.source_context));
			mcс.source_context = ifnode.source_context;

			// Убираем лишние присваивания для Task
			if (VarsHelper.TaskIdents.Count != 0)
			{
				if (VarsHelper.TaskIdents.First().Value == "1")
				{
					mcс.dereferencing_value = new dot_node(new ident(VarsHelper.TaskIdents.First().Key, ifnode.source_context),
						new ident("GetAwaiter", ts.source_context), ifnode.source_context);
					(code_list.list[0] as statement_list).Remove(extemp);
				}
			}


			var assign_ = new assign(new ident("@aw@_" + "1",ts.source_context), mcс, Operators.Assignment, ts.source_context);
			assign_.first_assignment_defines_type = true;


			var if2 = new if_node();
			if2.source_context= ts.source_context;
			if2.condition = new bin_expr(new dot_node(new ident("@aw@_" + "1", ts.source_context), new ident("IsCompleted", ts.source_context), ts.source_context), 
				new ident("False", ts.source_context), Operators.Equal, ts.source_context);

			var a1 = new assign(new dot_node(new ident("self",if2.source_context), new ident("state", ts.source_context),if2.source_context), new int32_const(2, ts.source_context), Operators.Assignment, ts.source_context);
		
			var temp = new var_statement(new var_def_statement(new ident(aw_temp, ts.source_context), new ident("self", ts.source_context)), ts.source_context);
			var dt = new dot_node(new dot_node(new ident("self", ts.source_context), new ident("tbuilder", ts.source_context)), 
				new ident("AwaitUnsafeOnCompleted", ts.source_context), ts.source_context);
			var ex = new expression_list(new ident("@aw@_" + "1", ts.source_context),ts.source_context);
			ex.Add(new ident(aw_temp));
			var mc = new method_call(dt, ex, ts.source_context);
			var pc = new procedure_call(mc, ts.source_context);
			var ifnode_body2 = new statement_list(a1, temp, pc, new procedure_call(new ident("exit"),ts.source_context), new empty_statement());
			ifnode_body2.left_logical_bracket = new token_info("begin");
			ifnode_body2.right_logical_bracket = new token_info("end");
			if2.then_body = ifnode_body2;
			if2.then_body.source_context = ts.source_context;	

			(code_list.list[0] as statement_list).Add(assign_,ts.source_context);
			(code_list.list[0] as statement_list).Add(if2,ts.source_context);
			(code_list.list[0] as statement_list).Add(new empty_statement(),ts.source_context);
			var ifnode_body1 = new statement_list(code_list.list[0],ts.source_context);

			ifnode_body1.Add(g1,ts.source_context);

			ifnode_body1.left_logical_bracket = new token_info("begin");
			ifnode_body1.right_logical_bracket = new token_info("end");
			ifnode.then_body = ifnode_body1;
			ifnode.source_context = ts.source_context;
			FillTypedVars(c - 2);

			ts.stmt_list.Add(ifnode, ts.source_context);
			////// if (state == 1)


			// Добавляем метки для переходов по состояниям
			var awCount = AwaiterCounter;
			while (awCount > 1)
			{
				awCount--;
				lb1 = newLabelName();
				bl.defs.Add(new label_definitions(new ident(lb1, ts.source_context)), ts.source_context);
			}

			lbnum = 0;
			awCount = AwaiterCounter;

			// Когда Task уже завершен
			while (awCount != 0)
			{
				awCount--;

				///if (state == 2,3,4 ...)
				var ifnode2 = new if_node();
				ifnode2.condition = new bin_expr(new dot_node(new ident("self", ts.source_context), new ident("state", ts.source_context)), 
					new int32_const(lbnum + 2, ts.source_context), Operators.Equal, ts.source_context);
				///state = -1;
				var a5 = new assign(new dot_node(new ident("self", ts.source_context),
					new ident("state", ts.source_context)), new un_expr(new int32_const(1, ts.source_context),
					Operators.Minus, ts.source_context), Operators.Assignment, ts.source_context);
				///goto label;
				var stlist3 = new statement_list(a5, new goto_statement(newLabelName(), ts.source_context), new empty_statement());
				stlist3.left_logical_bracket = new token_info("begin");
				stlist3.right_logical_bracket = new token_info("end");
				ifnode2.then_body = stlist3;
                ifnode2.then_body.source_context = ts.source_context;
                ifnode2.source_context = ts.source_context;
                ts.stmt_list.Add(ifnode2, ts.source_context);

			}


			lbnum = 1;
			awCount = AwaiterCounter;
			ts.stmt_list.Add(ls, ts.source_context);
			var mc2 = new method_call();
			mc2.source_context = ts.source_context;

            // начиная со второго встреченного await
            while (awCount > 1)
			{
				awCount--;
				var lb2 = newLabelName();
				var g2 = new goto_statement(lb2, ts.source_context);
				var ls2 = new labeled_statement(lb2);
				ls2.source_context = ts.source_context;


                mc2 = new method_call();
				mc2.source_context= ts.source_context;

				mc2.dereferencing_value = new dot_node(new ident(awaiter + (lbnum - 1).ToString(), ts.source_context), 
					new ident("GetResult"), ts.source_context);


                ts.stmt_list.Add(new procedure_call(mc2, ts.source_context), ts.source_context);
				var tt = code_list.list[lbnum - 1] as statement_list;
				var fg = true;


                foreach (var stat in tt.list)
				{
					if (stat is assign && fg)
					{
						var asstat = stat as assign;
						if (asstat.from.ToString().Contains("GetResult"))
						{
							asstat.from = mc2;

                        }

                    }
					fg = false;
					ts.stmt_list.Add(stat, stat.source_context);
				}

				extemp = new var_statement(new var_def_statement(new ident(extp + lbnum.ToString(), ts.source_context), 
					TaskList[lbnum - 1] as expression, ts.source_context), ts.source_context);
				extemp.source_context = ts.source_context;
				ts.stmt_list.Add(extemp, ts.source_context);
				var mccc = new method_call();
				mccc.dereferencing_value = new dot_node(new ident(extp + lbnum.ToString(), ts.source_context), 
					new ident("GetAwaiter", ts.source_context), ts.source_context);
				mccc.source_context = ts.source_context;
                // Убираем лишние присваивания для Task
                foreach (var kv in VarsHelper.TaskIdents)
				{
					if (kv.Value == lbnum.ToString())
					{
						mccc.dereferencing_value = new dot_node(new ident(kv.Key, ts.source_context), 
							new ident("GetAwaiter", ts.source_context), ts.source_context);
						ts.stmt_list.Remove(extemp);
						break;
					}
				}

				var assign22_ = new assign(new ident(awaiter + lbnum.ToString(), ts.source_context), mccc, Operators.Assignment, ts.source_context);
				assign22_.source_context = ts.source_context;
				assign22_.first_assignment_defines_type = true;

				ts.stmt_list.Add(assign22_,ts.source_context);

				var if3 = new if_node();
				if3.source_context = ts.source_context;
				if3.condition = new bin_expr(new dot_node(new ident(awaiter + lbnum.ToString(), ts.source_context), 
					new ident("IsCompleted", ts.source_context)), 
					new ident("False", ts.source_context), Operators.Equal, ts.source_context);

				var a11 = new assign(new dot_node(new ident("self", ts.source_context), new ident("state", ts.source_context)),
					new int32_const(lbnum + 1), Operators.Assignment, ts.source_context);
				var temp2 = new var_statement(new var_def_statement
					(new ident(aw_temp, ts.source_context), 
					new ident("self", ts.source_context), ts.source_context), ts.source_context);
				var dt1 = new dot_node(new dot_node(new ident("self", ts.source_context), 
					new ident("tbuilder", ts.source_context), ts.source_context), 
					new ident("AwaitUnsafeOnCompleted", ts.source_context), ts.source_context);

				var ex1 = new expression_list(new ident(awaiter + lbnum.ToString(), ts.source_context), ts.source_context);
				ex1.Add(new ident(aw_temp, ts.source_context), ts.source_context);
				var mc11 = new method_call(dt1, ex1, ts.source_context);
				var pc11 = new procedure_call(mc11, ts.source_context);
				var stlist4 = new statement_list(a11, temp2, pc11, new procedure_call(new ident("exit")), new empty_statement());
				stlist4.source_context = ts.source_context;
				stlist4.left_logical_bracket = new token_info("begin");
				stlist4.right_logical_bracket = new token_info("end");
				if3.then_body = stlist4;
				ts.stmt_list.Add(if3, ts.source_context);
				ts.stmt_list.Add(g2, ts.source_context);
				ts.stmt_list.Add(ls2, ts.source_context);

			}




            // Когда await закончились и пора выводить результат
            mc2 = new method_call();
			mc2.source_context = ts.source_context;
			mc2.dereferencing_value = new dot_node(new ident(awaiter + lbnum.ToString(), ts.source_context), 
				new ident("GetResult"), ts.source_context);


            ts.stmt_list.Add(new procedure_call(mc2, ts.source_context), ts.source_context);
			var tt2 = code_list.list.Last() as statement_list;
            var fg2 = true;
            foreach (var stat in tt2.list)
            {
                if (stat is assign && fg2)
                {
                    var asstat = stat as assign;
                    if (asstat.from.ToString().Contains("GetResult"))
                    {
                        asstat.from = mc2;

                    }
                }
                fg2 = false;
                ts.stmt_list.Add(stat, stat.source_context);
            }

			// Переименование всех переменных класса, находящихся в теле асинхронного метода
			foreach (var cis in VarsHelper.ClassIdentSet)
			{
				var replacerVis = new ReplaceVarNamesVisitor(cis, "@awclass." + cis);
                ts.visit(replacerVis);
                    if (replacerVis.counter != 0 && proc_def.proc_header.class_keyword)
				{
                    throw new SyntaxVisitorError("Для нестатического поля, метода или свойства требуется ссылка на объект",
                                           ts.source_context);
                }
            }
            // Переименование всех статических переменных класса, находящихся в теле асинхронного метода
            foreach (var cis in VarsHelper.ClassStaticSet)
			{
				var replacerVis = new ReplaceVarNamesVisitor(cis, class_name + "." + cis);
				ts.visit(replacerVis);
			}
			foreach (var d in VarsHelper.RepVarsDict)
			{
				if (VarsHelper.Parametrs.Contains(d.Key))
				{
					continue;
				}
				var replacerVis = new ReplaceVarNamesVisitor(d.Key, d.Value);
				ts.visit(replacerVis);
			}
            var MethodsSet = new HashSet<string>();
            if (class_name != null)
			{
                foreach (var dec in program_Module.program_block.defs.list)
                {
                    if (dec is type_declarations)
                    {
                        var td = (type_declarations)dec;

                        foreach (var tdeff in td.types_decl)
                        {
                            if (tdeff.type_name.name != class_name.name)
                            {
                                continue;
                            }
                            if (tdeff.type_def is class_definition)
                            {
                                var cd = tdeff.type_def as class_definition;
                                var bd = cd.body;
                                for (int j = 0; j < bd.Count; j++)
                                {
                                    if (bd[j] is class_members)
                                    {
                                        var cm = bd[j] as class_members;
                                        foreach (var item in cm.members)
                                        {
                                            if (item is procedure_header)
                                            {
											
                                                var pph = item as procedure_header;
												if (!pph.class_keyword)
												{
                                                    MethodsSet.Add(pph.name.meth_name.name);
                                                }
                                            }
                                            if (item is function_header)
                                            {
                                                var fh = item as function_header;
												if (!fh.class_keyword)
                                                    MethodsSet.Add(fh.name.meth_name.name);
                                                    
                                            }
                                        }
                                    }

                                }
                            }


                        }

                    }
                }
            }
           
			foreach (var item in MethodsSet)
			{
                var replacerVis = new ReplaceVarNamesVisitor(item, "@awclass."+item);
                ts.visit(replacerVis);
            }


            // Сохранение результата асинхронного метода
            if (ResultType != "")
			{
				if (ResultVal.Parent == null && ResultVal.Parent == null)
				{
					ts.stmt_list.Add(new assign(new ident("@aw_res", ts.source_context), 
						new default_operator(new named_type_reference(ResultType, ts.source_context), 
						ts.source_context), ts.source_context), ts.source_context);
				}
				else
				{
                    var replacerVis = new ReplaceVarNamesVisitor("Result", "@aw_res");
                    ts.visit(replacerVis);
                }
				
			}

		}

		// Объявляем локальные переменные полями класса StateMachine
		public void FillTypedVars(int c)
		{
			var tdecl = program_Module.program_block.defs.list[c] as type_declarations;
			var tdef = tdecl.types_decl[0];
			var cd = tdef.type_def as class_definition;
			var bd = cd.body;
			var cm = bd[1] as class_members;
			if (ResultType != "")
			{
				var r_vds = new var_def_statement(new ident("@aw_res", cm.source_context), 
					new named_type_reference(new ident(ResultType, cm.source_context),cm.source_context),cm.source_context);
				cm.Add(r_vds);
			}
			foreach (var vt in VarsHelper.TasksHash)
			{
				var vd = new var_def_statement(new ident_list(), new named_type_reference("string", vt.source_context));
				foreach (var w in vt.var_def.vars.list)
				{
					vd.vars.Add(w,w.source_context);
				}
				cm.members.Add(vd);
			}
			foreach (var v in VarsHelper.VarsList)
			{
				if (VarsHelper.TasksHash.Contains(v))
				{
					continue;
				}
				if (VarsHelper.NoneTypedVarsList.Contains(v))
				{
					var vv = new var_statement();
					vv.source_context = v.source_context;
					vv.var_def = new var_def_statement(v.var_def.vars, 
						new named_type_reference(new ident("string",v.source_context),v.source_context),v.source_context);
					VarsHelper.AddNewRep(vv);
					cm.members.Add(vv.var_def);
					continue;
				}
				VarsHelper.AddNewRep(v);
				cm.members.Add(v.var_def);
			}
		}

		// Удаляем тело асинхронной функции 
		public void DeleteBody()
		{
			var b = proc_def.proc_body as block;
			var st = b.program_code;
			st.list.Remove(st.list.First());
		}
	}
}
