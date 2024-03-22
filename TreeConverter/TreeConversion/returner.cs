// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Используется для возвращения значения при обходе дерева.

//(ssyy) 01.12.2007
//returner восполняет тот недостаток, что функции visit класса syntax_tree_visitor
//сами ничего не возвращают.
//Я внёс изменения в концепцию реализации класса returner.
//Теперь здесь только одна переменная ret_semantic типа semantic_node.
//Она приводится при необходимости к нужному типу.
//Внешне при этом работа с классом returner не изменилась.
using System;

using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.TreeConverter
{
	public class returner
	{
        private semantic_node ret_semantic;

        private syntax_tree_visitor syntax_tree_visitor;

		public returner(syntax_tree_visitor stv)
		{
            syntax_tree_visitor = stv;
		}

        public expression_node get_expression()
        {
            return ret_semantic as expression_node;
        }

        public void reset()
        {
            ret_semantic = null;
        }

		public semantic_node visit(SyntaxTree.syntax_tree_node tn)
		{
            syntax_tree_visitor.ProcessNode(tn);
            return ret_semantic;
		}

		public statement_node visit(SyntaxTree.statement st)
		{
            syntax_tree_visitor.ProcessNode(st);
            return ret_semantic as statement_node;
		}

		public expression_node visit(SyntaxTree.expression expr)
		{
            syntax_tree_visitor.ProcessNode(expr);

            /*addressed_expression ad = ret_semantic as addressed_expression;
            if (ad != null && ad.is_addressed)
            {
                if (convertion_data_and_alghoritms.check_for_constant(ad))
                    ad.is_addressed = false;
            }*/

            //Надеюсь, это сильно не скажется на производительности, хотя в другом случае этот же код просто будет разбросан по всем метдам syntax_tree_visitor-а.
            base_function_call bfc = ret_semantic as base_function_call;
            if (bfc != null)
            {
                if (bfc.simple_function_node.compile_time_executor != null)
                {
                    expression_node ex = bfc.simple_function_node.compile_time_executor(bfc.location, bfc.parameters.ToArray());
                    if (ex != null)
                    {
                        return ex;
                    }
                }
            }

            return ret_semantic as expression_node;
		}

		public type_node visit(SyntaxTree.type_definition type_def)
		{
            syntax_tree_visitor.ProcessNode(type_def);
            return ret_semantic as type_node;
		}

		public addressed_expression visit(SyntaxTree.addressed_value av)
		{
            syntax_tree_visitor.ProcessNode(av);
            return ret_semantic as addressed_expression;
		}

		public void return_value(semantic_node sn)
		{
            ret_semantic = sn;
		}

        public semantic_node get_result()
        {
            return ret_semantic;
        }

	}

}