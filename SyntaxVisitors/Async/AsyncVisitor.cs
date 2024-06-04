using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler.Errors;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors.Async;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.SyntaxTreeConverters;
using System.IO;
using System.Collections;

namespace SyntaxVisitors
{
	public class AsyncVisitor : BaseChangeVisitor
	{
		private program_module program_Module { get; set; }

		private procedure_definition proc_def { get; set; }

		private List<procedure_definition> proc_def_List = new List<procedure_definition>();

		private string BuilderType = "AsyncVoidMethodBuilder";

		private bool IsAsync = false;

		private int AwaiterCounter = 1;

		private AwaitBuilder AwaitBuilder;

		private AsyncBuilder AsyncBuilder;

		private bool IsFirstAwait = true;

		private bool IsFirstAsync = true;

		private int ChangeBuilderCounter = 0;



        public static AsyncVisitor New
        {

            get { return new AsyncVisitor(); }
        }

        public override void visit(procedure_definition pd)
		{
			if (pd.proc_header.IsAsync)
			{
				MainVisitor.flag = false;
				MainVisitor.Accept(pd);
				if (!MainVisitor.flag)
				{
					pd.proc_header.IsAsync = false;
                    IsAsync = false;
                    return;
				}
				IsAsync = true;
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
				// DefaultVisit(pd);
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
                            throw new SyntaxVisitorError("Ключевое слово 'await' может быть использовано только в асинхронных методах ", procedure_code.list[i].source_context);
                        }
                        if (procedure_code.list[i] is var_statement)
                        {
                            var pp = procedure_code.list[i] as var_statement;
                            if (pp.var_def.inital_value is await_node)
                            {
                                // AddError нужно вместо SyntaxVisitorError
                                throw new SyntaxVisitorError("Ключевое слово 'await' может быть использовано только в асинхронных методах ", pp.var_def.inital_value.source_context);
                            }
                        }
                    }
                }
			}
		}
		public override void visit(await_node_statement astat)
		{
			ProcessNode(astat.aw);
           // DefaultVisit(astat.aw);
        }

        public override void visit(await_node a)
		{
			if (!IsAsync)
			{
				throw new SyntaxVisitorError("'await' can only be used in an async method ", a.source_context);
			}

			AwaiterCounter++;
			if (IsFirstAwait)
			{
				IsFirstAwait = false;
				AwaitBuilder = new AwaitBuilder(program_Module, proc_def);
				AwaitBuilder.VarsHelper.RepVarsDict = AsyncBuilder.RepVarsDict;
				//AwaitBuilder.RemoveRedundantAwait();
				AwaitBuilder.GetCode();
				AwaitBuilder.AddAwaiter("TaskAwaiter", true, a.ex, ChangeBuilderCounter);
				AwaitBuilder.ChangeBuilder(BuilderType, ChangeBuilderCounter);
			}
			else
			{
				AwaitBuilder.ChangeBuilder(BuilderType, ChangeBuilderCounter);
				AwaitBuilder.AddAwaiter("TaskAwaiter", false, a.ex, ChangeBuilderCounter);
			}
			if (AwaitBuilder.AwaiterCounter == AwaiterCounter - 1)
			{

				AwaitBuilder.GenAwait(ChangeBuilderCounter + 2, a.ex);
				AwaitBuilder.DeleteBody();
			}
		}
		public void GenMain(program_module pm)
		{
			if (MainVisitor.flag)
			{
				var d = pm.program_block.defs;
				var p = new procedure_definition();
				p.proc_header = new function_header(new template_type_reference(new named_type_reference(new ident("Task", d.source_context), d.source_context),
					new template_param_list(new named_type_reference(new ident("integer", d.source_context), d.source_context), d.source_context), d.source_context), d.source_context);
				p.proc_header.name = new method_name();
				p.proc_header.name.meth_name = new ident("@AsyncMain", d.source_context);
				p.proc_header.IsAsync = true;
				p.source_context = d.source_context;
				//var n = p.proc_header.name;
				var b = new block();
				b.source_context = d.source_context;
				b.program_code = new statement_list();
				b.program_code.source_context = d.source_context;
				foreach (var item in pm.program_block.program_code.list)
				{
					b.program_code.list.Add(item);

				}
				//b.program_code = pm.program_block.program_code;
				p.proc_body = b;
				pm.program_block.program_code.list.Clear();
				d.Add(p);
				var mc = new method_call();
				mc.dereferencing_value = new ident("@AsyncMain");
				mc.source_context = d.source_context;
				var vst = new var_statement(new var_def_statement(new ident("@aw_main", d.source_context), mc, d.source_context), d.source_context);
				var pc = new procedure_call(new dot_node(new dot_node(new ident("@aw_main", d.source_context), new ident("GetAwaiter")), new ident("GetResult"), d.source_context), d.source_context);
				pm.program_block.program_code.list.Add(vst);
				pm.program_block.program_code.list.Add(pc);

			}
		}
		public override void visit(program_module pm)
		{
			program_Module = pm;
            MainVisitor.flag = false;
            MainVisitor.Accept(pm.program_block.program_code);
            GenMain(pm);
			

            DefaultVisit(pm);
			if (IsAsync)
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
						IsAsync = true;
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
					else
						IsAsync = false;

					DefaultVisit(p);
					ChangeBuilderCounter += 4;
				}
				AsyncBuilder.SortBlocks();

			}
		}
	}
}
