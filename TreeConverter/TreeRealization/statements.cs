// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;

namespace PascalABCCompiler.TreeRealization
{
    /// <summary>
    /// Пустое выражение.
    /// </summary>
	[Serializable]
	public class empty_statement : statement_node
	{
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="loc">Расположение узла.</param>
        public empty_statement(location loc) : base(loc)
        {
        }

        /// <summary>
        /// Тип выржения.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.empty_statement;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

	}

    [Serializable]
    public class wrapped_statement : statement_node
    {
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="loc">Расположение узла.</param>
        public wrapped_statement(location loc)
            : base(loc)
        {
        }

        /// <summary>
        /// Тип выржения.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.wrapped_statement;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }

        public virtual statement_node restore()
        {
            return null;
        }
    }

    [Serializable]
    public class runtime_statement : statement_node, SemanticTree.IRuntimeManagedMethodBody
    {
        private SemanticTree.runtime_statement_type _rts;

        public runtime_statement(SemanticTree.runtime_statement_type rts,location loc) :base(loc)
        {
            _rts = rts;
        }

        public SemanticTree.runtime_statement_type runtime_statement_type
        {
            get
            {
                return _rts;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.runtime_statement;
            }
        }

    }
	
    //TODO: Луше сделать специальный вид функции.
    /// <summary>
    /// Обращение к методу в неуправляемой dll.
    /// </summary>
	[Serializable]
	public class external_statement : statement_node, SemanticTree.IExternalStatementNode
	{
        /// <summary>
        /// Имя dll.
        /// </summary>
		private string _module_name;

        /// <summary>
        /// Имя метода.
        /// </summary>
		private string _name;
		
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="_module_name">Имя dll.</param>
        /// <param name="_name">Имя метода.</param>
        /// <param name="loc">Расположение выражения.</param>
		public external_statement(string _module_name, string _name, location loc) : base(loc)
		{
			this._module_name = _module_name;
			this._name = _name;
		}

        /// <summary>
        /// Имя dll.
        /// </summary>
		public string module_name
        {
			get 
			{
				return _module_name;
			}
		}

        /// <summary>
        /// Имя метода.
        /// </summary>
		public string name
        {
			get 
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
		
        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.external_statement_node;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}
	}
	
	[Serializable]
	public class pinvoke_statement : statement_node, SemanticTree.IPInvokeStatementNode
	{
		public pinvoke_statement(location loc) : base(loc)
		{

		}
		
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.pinvoke_node;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}
	}
	
    /// <summary>
    /// Класс, представляющий узел if  в программе.
    /// </summary>
	[Serializable]
	public class if_node : statement_node, SemanticTree.IIfNode
	{
        /// <summary>
        /// Условие.
        /// </summary>
		private expression_node _condition;

        /// <summary>
        /// Тело then.
        /// </summary>
		private statement_node _then_body;

        /// <summary>
        /// Тело else.
        /// </summary>
		private statement_node _else_body;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="condition">Условие.</param>
        /// <param name="then_body">Тело then.</param>
        /// <param name="else_body">Тело else.</param>
        /// <param name="loc">Расположение узла.</param>
		public if_node(expression_node condition,statement_node then_body,statement_node else_body,location loc) :
            base(loc)
		{
			_condition=condition;
			_then_body=then_body;
			_else_body=else_body;
		}

        /// <summary>
        /// Условие.
        /// </summary>
		public expression_node condition
		{
			get
			{
				return _condition;
			}
		}

        /// <summary>
        /// Тело then.
        /// </summary>
		public statement_node then_body
		{
			get
			{
				return _then_body;
			}
			set
			{
				_then_body = value;
			}
		}

        /// <summary>
        /// Тело else. Если if без then это свойство = null.
        /// </summary>
		public statement_node else_body
		{
			get
			{
				return _else_body;
			}
			set
			{
				_else_body = value;
			}
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.if_node;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		SemanticTree.IStatementNode SemanticTree.IIfNode.else_body
		{
			get
			{
				return _else_body;
			}
		}

		SemanticTree.IExpressionNode SemanticTree.IIfNode.condition
		{
			get
			{
				return _condition;
			}
		}

		SemanticTree.IStatementNode SemanticTree.IIfNode.then_body
		{
			get
			{
				return _then_body;
			}
		}
	}

    /// <summary>
    /// Класс, представляющий узел while в программе.
    /// </summary>
	[Serializable]
	public class while_node : statement_node, SemanticTree.IWhileNode
	{
        /// <summary>
        /// Условие цикла.
        /// </summary>
		private expression_node _condition;

        /// <summary>
        /// Тело цикла.
        /// </summary>
		private statement_node _body;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="condition">Условие цикла.</param>
        /// <param name="loc">Расположение узла.</param>
        public while_node(expression_node condition, location loc)
            : base(loc)
        {
            _condition = condition;
        }


        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="condition">Условие цикла.</param>
        /// <param name="body">Тело цикла.</param>
        /// <param name="loc">Расположение узла.</param>
		public while_node(expression_node condition,statement_node body,location loc) : base(loc)
		{
			_condition=condition;
			_body=body;
		}

        /// <summary>
        /// Условие цикла.
        /// </summary>
		public expression_node condition
		{
			get
			{
				return _condition;
			}
		}

		//Тело while.
		public statement_node body
		{
			get
			{
				return _body;
			}
            set
            {
                _body = value;
            }
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.while_node;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		SemanticTree.IExpressionNode SemanticTree.IWhileNode.condition
		{
			get
			{
				return _condition;
			}
		}

		SemanticTree.IStatementNode SemanticTree.IWhileNode.body
		{
			get
			{
				return _body;
			}
		}
	}

    /// <summary>
    /// Класс, представляющий узел repeat в программе.
    /// </summary>
	[Serializable]
	public class repeat_node : statement_node, SemanticTree.IRepeatNode
	{
        /// <summary>
        /// Тело цикла.
        /// </summary>
		private statement_node _body;

        /// <summary>
        /// Условие цикла.
        /// </summary>
		private expression_node _condition;

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="loc">Расположение узла.</param>
        public repeat_node(location loc)
            : base(loc)
        {
        }

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="body">Тело цикла.</param>
        /// <param name="condition">Условие.</param>
        /// <param name="loc">Расположение узла.</param>
		public repeat_node(statement_node body,expression_node condition, location loc) : base(loc)
		{
			_body=body;
			_condition=condition;
		}

        /// <summary>
        /// Тело цикла.
        /// </summary>
		public statement_node body
		{
			get
			{
				return _body;
			}
            set
            {
                _body = value;
            }
		}

        /// <summary>
        /// Условие цикла.
        /// </summary>
		public expression_node condition
		{
			get
			{
				return _condition;
			}
            set
            {
                _condition = value;
            }
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.repeat_node;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		SemanticTree.IExpressionNode SemanticTree.IRepeatNode.condition
		{
			get
			{
				return _condition;
			}
		}

		SemanticTree.IStatementNode SemanticTree.IRepeatNode.body
		{
			get
			{
				return _body;
			}
		}
	}

    /// <summary>
    /// Узел цикла for.
    /// </summary>
	[Serializable]
	public class for_node : statement_node, SemanticTree.IForNode
	{
        /// <summary>
        /// Выражение инициализации переменной цикла.
        /// </summary>
		private statement_node _initialization_statement;

        /// <summary>
        /// Условие цикла
        /// </summary>
		private expression_node _while_expr;
		
		private expression_node _init_while_expr;

        /// <summary>
        /// Выражение измененияя счетчика цикла.
        /// </summary>
		private statement_node _increment_statement;

        /// <summary>
        /// Тело цикла.
        /// </summary>
		private statement_node _body;

        /// <summary>
        /// Шаг цикла
        /// </summary>
		public expression_node for_step { get; set; }

        private bool _bool_cycle;
		
        /// <summary>
        /// Конструктор клааса.
        /// </summary>
        /// <param name="initialization_statement">Выражение инициализации переменной цикла.</param>
        /// <param name="while_expr">Условие цикла.</param>
        /// <param name="increment_statement">Выражение измененияя счетчика цикла.</param>
        /// <param name="body">Тело цикла.</param>
		public for_node(statement_node initialization_statement,expression_node while_expr, expression_node init_while_expr,
			statement_node increment_statement,statement_node body, location loc) : base(loc)
		{
			_initialization_statement=initialization_statement;
			_while_expr=while_expr;
			_init_while_expr = init_while_expr;
			_increment_statement=increment_statement;
			_body=body;
		}

        /// <summary>
        /// Выражение инициализации переменной цикла.
        /// </summary>
		public statement_node initialization_statement
		{
			get
			{
				return _initialization_statement;
			}
			set
			{
				_increment_statement = value;
			}
		}

        /// <summary>
        /// Условие продолжения цикла.
        /// </summary>
		public expression_node while_expr
		{
			get
			{
				return _while_expr;
			}
			set
			{
				_while_expr = value;
			}
		}
		
		public expression_node init_while_expr
		{
			get
			{
				return _init_while_expr;
			}
			set
			{
				_init_while_expr = value;
			}
		}
		
        /// <summary>
        /// Изменение счетчиков цикла.
        /// </summary>
		public statement_node increment_statement
		{
			get
			{
				return _increment_statement;
			}
			set
			{
				_increment_statement = value;
			}
		}

        /// <summary>
        /// Тело цикла.
        /// </summary>
		public statement_node body
		{
			get
			{
				return _body;
			}
            set
            {
                _body = value;
            }

		}
		
		public bool bool_cycle
		{
			get
			{
				return _bool_cycle;
			}
			set
			{
				_bool_cycle = value;
			}
		}
		
        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.for_node;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		SemanticTree.IStatementNode SemanticTree.IForNode.initialization_statement
		{
			get
			{
				return _initialization_statement;
			}
		}

		SemanticTree.IStatementNode SemanticTree.IForNode.body
		{
			get
			{
				return _body;
			}
		}

		SemanticTree.IStatementNode SemanticTree.IForNode.increment_statement
		{
			get
			{
				return _increment_statement;
			}
		}

		SemanticTree.IExpressionNode SemanticTree.IForNode.while_expr
		{
			get
			{
				return _while_expr;
			}
		}
		
		SemanticTree.IExpressionNode SemanticTree.IForNode.init_while_expr
		{
			get
			{
				return _init_while_expr;
			}
		}
		
		bool SemanticTree.IForNode.IsBoolCycle
		{
			get
			{
				return _bool_cycle;
			}
		}
	}

    /// <summary>
    /// Список выражений.
    /// </summary>
	[Serializable]
	public class statements_list : statement_node, SemanticTree.IStatementsListNode
	{
        /// <summary>
        /// Список выражений.
        /// </summary>
        private statement_node_list _statements = new statement_node_list();

        private location _llbl=null,_rlbl=null;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="loc">Расположение списка выражений.</param>
        public statements_list(location loc) : base(loc)
        {
        }
        
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="loc">Расположение списка выражений.</param>
        /// <param name="leftLogicalBracketLocation">Положение левой логической скобки</param>
        /// <param name="rightLogicalBracketLocation">Положение правой логической скобки</param>
        public statements_list(location loc, location leftLogicalBracketLocation, location rightLogicalBracketLocation)
            : base(loc)
        {
            this._llbl = leftLogicalBracketLocation;
            this._rlbl = rightLogicalBracketLocation;
        }



        private List<local_block_variable> _var_defs = new List<local_block_variable>();

        /// <summary>
        /// Массив переменных, определенных в блоке.
        /// Используется при обходе дерева посетителем.
        /// </summary>
        SemanticTree.ILocalBlockVariableNode[] SemanticTree.IStatementsListNode.LocalVariables
        {
            get
            {
                return (_var_defs.ToArray());
            }
        }

        public List<local_block_variable> local_variables
        {
            get
            {
                return _var_defs;
            }
            set
            {
                _var_defs = value;
            }
        }

        /// <summary>
        /// Список statement-ов.
        /// </summary>
		public statement_node_list statements
		{
			get
			{
				return _statements;
			}
		}

        /// <summary>
        /// Положение левой логической скобки
        /// </summary>
        public SemanticTree.ILocation LeftLogicalBracketLocation
        {
            get
            {
                return _llbl;
            }
            set
            {
                _llbl = (location)value;
            }
        }

        /// <summary>
        /// Положение правой логической скобки
        /// </summary>
        public SemanticTree.ILocation RightLogicalBracketLocation
        {
            get
            {
                return _rlbl;
            }
            set
            {
                _rlbl = (location)value;
            }
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.statements_list;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		SemanticTree.IStatementNode[] SemanticTree.IStatementsListNode.statements
		{
			get
			{
				return (_statements.ToArray());
			}
		}

        internal SymbolTable.Scope Scope = null;
	}

    //TODO: Неплохо-бы в следующих 3-х классах хранить ссылку на следующее за циклом выражение.
    /// <summary>
    /// Узел break в цикле while.
    /// </summary>
    [Serializable]
	public class while_break_node : expression_node, SemanticTree.IWhileBreakNode
	{
        /// <summary>
        /// Узел while.
        /// </summary>
		private while_node _whnd;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="whnd">Узел while из которого ведет этот break.</param>
        /// <param name="loc">Расположение узла.</param>
		public while_break_node(while_node whnd,location loc) :
			base(null,loc)
		{
			_whnd=whnd;
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.while_break_node;
			}
		}

		public SemanticTree.IWhileNode while_node
		{
			get
			{
				return _whnd;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit((SemanticTree.IWhileBreakNode)this);
		}
	}

    /// <summary>
    /// Узел break в цикле repeat.
    /// </summary>
    [Serializable]
	public class repeat_break_node : expression_node, SemanticTree.IRepeatBreakNode
	{
        /// <summary>
        /// Узел repeat, внутри которого расположен этот break.
        /// </summary>
		private repeat_node _rpnd;

        /// <summary>
        /// Конструктор касса.
        /// </summary>
        /// <param name="rn">Узел repeat, внутри которого расположен этот break.</param>
        /// <param name="loc">Расположение узла.</param>
		public repeat_break_node(repeat_node rn,location loc) :
			base(null,loc)
		{
			_rpnd=rn;
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.repeat_break_node;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit((SemanticTree.IRepeatBreakNode)this);
		}

		public SemanticTree.IRepeatNode repeat_node
		{
			get
			{
				return _rpnd;
			}
		}
	}

    /// <summary>
    /// Узел break в икле for.
    /// </summary>
    [Serializable]
	public class for_break_node : expression_node, SemanticTree.IForBreakNode
	{
        /// <summary>
        /// Цикл for внутри которого расположен этот break.
        /// </summary>
		private for_node _frnd;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="fn">Цикл for внутри которого расположен этот break.</param>
        /// <param name="loc">Расположение узла.</param>
		public for_break_node(for_node fn,location loc) :
			base(null,loc)
		{
			_frnd=fn;
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.for_break_node;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit((SemanticTree.IForBreakNode)this);
		}

		public SemanticTree.IForNode for_node
		{
			get
			{
				return _frnd;
			}
		}
	}
	
	[Serializable]
	public class foreach_break_node : expression_node, SemanticTree.IForeachBreakNode
	{
        /// <summary>
        /// Цикл for внутри которого расположен этот break.
        /// </summary>
		private foreach_node _frnd;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="fn">Цикл for внутри которого расположен этот break.</param>
        /// <param name="loc">Расположение узла.</param>
		public foreach_break_node(foreach_node fn,location loc) :
			base(null,loc)
		{
			_frnd=fn;
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.foreach_break_node;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit((SemanticTree.IForeachBreakNode)this);
		}

		public SemanticTree.IForeachNode foreach_node
		{
			get
			{
				return _frnd;
			}
		}
	}
	
    /// <summary>
    /// Узел exit где угодно
    /// </summary>
    [Serializable]
    public class exit_procedure : expression_node, SemanticTree.IExitProcedure
    {

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="loc">Расположение узла.</param>
        public exit_procedure(location loc)
            :
            base(null, loc)
        {
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.exit_procedure;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit((SemanticTree.IExitProcedure)this);
        }

    }


    /// <summary>
    /// Узел, представляющий continue внутри цикла while.
    /// </summary>
    [Serializable]
	public class while_continue_node : expression_node, SemanticTree.IWhileContinueNode
	{
        /// <summary>
        /// Цикл while внутри которого расположен этот continue.
        /// </summary>
		private while_node _whnd;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="whnd">Цикл while внутри которого расположен этот continue.</param>
        /// <param name="loc">Расположение узла.</param>
		public while_continue_node(while_node whnd,location loc) :
			base(null,loc)
		{
			_whnd=whnd;
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.while_continue_node;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit((SemanticTree.IWhileContinueNode)this);
		}

		public SemanticTree.IWhileNode while_node
		{
			get
			{
				return _whnd;
			}
		}
	}

    /// <summary>
    /// Узел continue в цикле repeat.
    /// </summary>
    [Serializable]
	public class repeat_continue_node : expression_node, SemanticTree.IRepeatContinueNode
	{
        /// <summary>
        /// Цикл repeat, внутри которог расположен этот continue.
        /// </summary>
		private repeat_node _rpnd;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="rn">Цикл repeat, внутри которог расположен этот continue.</param>
        /// <param name="loc">Расположение узла.</param>
		public repeat_continue_node(repeat_node rn, location loc) :
			base(null,loc)
		{
			_rpnd=rn;
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.repeat_continue_node;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit((SemanticTree.IRepeatContinueNode)this);
		}

		public SemanticTree.IRepeatNode repeat_node
		{
			get
			{
				return _rpnd;
			}
		}
	}

    /// <summary>
    /// Узел continue внутри цикла for.
    /// </summary>
    [Serializable]
	public class for_continue_node : expression_node, SemanticTree.IForContinueNode
	{
        /// <summary>
        /// Цикл for, внутри которого расположен этот continue.
        /// </summary>
		private for_node _frnd;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="loc">Расположение узла.</param>
		public for_continue_node(for_node fn,location loc):
			base(null,loc)
		{
			_frnd=fn;
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.for_continue_node;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit((SemanticTree.IForContinueNode)this);
        }

		public SemanticTree.IForNode for_node
		{
			get
			{
				return _frnd;
			}
		}
	}
	
	[Serializable]
	public class foreach_continue_node : expression_node, SemanticTree.IForeachContinueNode
	{
        /// <summary>
        /// Цикл for, внутри которого расположен этот continue.
        /// </summary>
		private foreach_node _frnd;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="loc">Расположение узла.</param>
		public foreach_continue_node(foreach_node fn,location loc):
			base(null,loc)
		{
			_frnd=fn;
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.foreach_continue_node;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit((SemanticTree.IForeachContinueNode)this);
        }

		public SemanticTree.IForeachNode foreach_node
		{
			get
			{
				return _frnd;
			}
		}
	}
	
    [Serializable]
    public class switch_node : statement_node, SemanticTree.ISwitchNode
    {
        private expression_node _condition;
        private case_variant_node_list _case_variants = new case_variant_node_list();
        private statement_node _default_statement;

        public switch_node(location loc) : base(loc)
        {
        }

        public case_variant_node_list case_variants
        {
            get
            {
                return _case_variants;
            }
        }

        public expression_node condition
        {
            get
            {
                return _condition;
            }
            set
            {
                _condition = value;
            }
        }

        public statement_node default_statement
        {
            get
            {
                return _default_statement;
            }
            set
            {
                _default_statement = value;
            }
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.switch_node;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }

        SemanticTree.ICaseVariantNode[] SemanticTree.ISwitchNode.case_variants
        {
            get
            {
                return _case_variants.ToArray();
            }
        }

        SemanticTree.IStatementNode SemanticTree.ISwitchNode.default_statement
        {
            get
            {
                return _default_statement;
            }
        }

        public SemanticTree.IExpressionNode case_expression
        {
            get
            {
                return _condition;
            }
        }
    }

    [Serializable]
    public class case_variant_node : statement_node, SemanticTree.ICaseVariantNode
    {
        private case_range_node_list _case_ranges = new case_range_node_list();
        //private constant_node_list _case_constants = new constant_node_list();
        private int_const_node_list _case_constants = new int_const_node_list();
        private statement_node _case_statement;

        public case_variant_node(location loc) : base(loc)
        {
        }

        public case_range_node_list case_ranges
        {
            get
            {
                return _case_ranges;
            }
        }

        public int_const_node_list case_constants
        {
            get
            {
                return _case_constants;
            }
        }

        public statement_node case_statement
        {
            get
            {
                return _case_statement;
            }
            set
            {
                _case_statement = value;
            }
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.case_variant_node;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }

        public SemanticTree.IIntConstantNode[] elements
        {
            get
            {
                return _case_constants.ToArray();
            }
        }

        public SemanticTree.ICaseRangeNode[] ranges
        {
            get
            {
                return _case_ranges.ToArray();
            }
        }

        public SemanticTree.IStatementNode statement_to_execute
        {
            get
            {
                return _case_statement;
            }
        }
    }

    [Serializable]
    public class case_range_node : statement_node, SemanticTree.ICaseRangeNode
    {
        private int_const_node _lower_bound;
        private int_const_node _high_bound;

        public case_range_node(int_const_node lower_bound, int_const_node high_bound, location loc) : base(loc)
        {
            _lower_bound = lower_bound;
            _high_bound = high_bound;
        }

        public int_const_node lower_bound
        {
            get
            {
                return _lower_bound;
            }
        }

        public int_const_node high_bound
        {
            get
            {
                return _high_bound;
            }
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.case_range_node;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }

        SemanticTree.IIntConstantNode SemanticTree.ICaseRangeNode.lower_bound
        {
            get
            {
                return _lower_bound;
            }
        }

        SemanticTree.IIntConstantNode SemanticTree.ICaseRangeNode.high_bound
        {
            get
            {
                return _high_bound;
            }
        }
    }

    [Serializable]
    public class exception_filter : statement_node, SemanticTree.IExceptionFilterBlockNode
    {
        private type_node _filter_type;
        private local_block_variable_reference _exception_var;
        private statement_node _exception_handler;

        public exception_filter(type_node filter_type, local_block_variable_reference exception_var,
            statement_node exception_handler, location loc) : base(loc)
        {
            _filter_type = filter_type;
            _exception_var = exception_var;
            _exception_handler = exception_handler;
        }

        public type_node filter_type
        {
            get
            {
                return _filter_type;
            }
            set
            {
            	_filter_type = value;
            }
        }

        public local_block_variable_reference exception_var
        {
            get
            {
                return _exception_var;
            }
            set
            {
            	_exception_var = value;
            }
        }

        public statement_node exception_handler
        {
            get
            {
                return _exception_handler;
            }
            set
            {
            	_exception_handler = value;
            }
        }

        public SemanticTree.ITypeNode ExceptionType
        {
            get
            {
                return _filter_type;
            }
        }

        public SemanticTree.ILocalBlockVariableReferenceNode ExceptionInstance
        {
            get
            {
                return _exception_var;
            }
        }

        public SemanticTree.IStatementNode ExceptionHandler
        {
            get
            {
                return _exception_handler;
            }
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.exception_filter;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }
    }

    [Serializable]
    public class try_block : statement_node, SemanticTree.ITryBlockNode
    {
        private statement_node _try_statements;
        private statement_node _finally_statements;
        private exception_filters_list _filters;

        public try_block(statement_node try_statements, statement_node finally_statements,
            exception_filters_list filters, location loc)
            : base(loc)
        {
            _try_statements = try_statements;
            _finally_statements = finally_statements;
            _filters = filters;
        }

        public statement_node try_statements
        {
            get
            {
                return _try_statements;
            }
        }

        public statement_node finally_statements
        {
            get
            {
                return _finally_statements;
            }
        }

        public exception_filters_list filters
        {
            get
            {
                return _filters;
            }
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.try_block;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }

        public SemanticTree.IStatementNode TryStatements
        {
            get
            {
                return _try_statements;
            }
        }

        public SemanticTree.IStatementNode FinallyStatements
        {
            get
            {
                return _finally_statements;
            }
        }

        public SemanticTree.IExceptionFilterBlockNode[] ExceptionFilters
        {
            get
            {
                return _filters.ToArray();
            }
        }

    }

    [Serializable]
    public class foreach_node : statement_node, SemanticTree.IForeachNode
    {
        private var_definition_node _ident;
        private expression_node _in_what;
        private statement_node _what_do;
        private type_node _element_type;
        private bool _is_generic;
		
        public foreach_node(
            var_definition_node _ident,
            expression_node _in_what,
            statement_node _what_do,
            type_node _element_type,
            bool _is_generic,
            location loc
        ):base(loc)
        {
            this._ident = _ident;
            this._in_what = _in_what;
            this._what_do = _what_do;
            this._element_type = _element_type;
            this._is_generic = _is_generic;
        }

        public var_definition_node ident
        {
            get
            {
                return _ident;
            }
            set
            {
            	_ident = value;
            }
        }

        public expression_node in_what
        {
            get
            {
                return _in_what;
            }
            set
            {
            	_in_what = value;
            }
        }

        public statement_node what_do
        {
            get
            {
                return _what_do;
            }
            set
            {
            	_what_do = value;
            }
        }

        public type_node element_type
        {
            get { return _element_type; }
            set { _element_type = value; }
        }

        public bool is_generic
        {
            get { return _is_generic; }
            set { _is_generic = value; }
        }

        public SemanticTree.IStatementNode Body
        {
            get
            {
                return _what_do;
            }
        }

        public SemanticTree.IExpressionNode InWhatExpr
        {
            get
            {
                return _in_what;
            }
        }

        public SemanticTree.IVAriableDefinitionNode VarIdent
        {
            get
            {
                return _ident;
            }
        }

        public SemanticTree.ITypeNode ElementType
        {
            get { return _element_type; }
        }

        public bool IsGeneric
        {
            get { return _is_generic; }
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.foreach_node;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }
    }


    [Serializable]
    public class lock_statement : statement_node, SemanticTree.ILockStatement
    {
        private expression_node _lock_object;
        private statement_node _body;

        public lock_statement(expression_node _lock_object, statement_node _body, location loc)
            : base(loc)
        {
            this._lock_object = _lock_object;
            this._body = _body;
        }

        public expression_node lock_object
        {
            get
            {
                return _lock_object;
            }
            set
            {
            	_lock_object = value;
            }
        }

        public statement_node body
        {
            get
            {
                return _body;
            }
            set
            {
            	_body = value;
            }
        }

        public SemanticTree.IStatementNode Body
        {
            get
            {
                return _body;
            }
        }

        public SemanticTree.IExpressionNode LockObject
        {
            get
            {
                return _lock_object;
            }
        }

       
        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.lock_statement;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }
    }
}
