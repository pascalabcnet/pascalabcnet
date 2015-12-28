// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;

namespace PascalABCCompiler.TreeRealization
{

    public class as_node : expression_node, SemanticTree.IAsNode
    {
        private expression_node _left;
        private type_node _right;

        public as_node(expression_node left, type_node right, location loc)
            : base(right, loc)
        {
            _left = left;
            _right = right;
        }

        public expression_node left
        {
            get
            {
                return _left;
            }
        }

        public type_node right
        {
            get
            {
                return _right;
            }
        }

        SemanticTree.IExpressionNode SemanticTree.IAsNode.left
        {
            get
            {
                return _left;
            }
        }

        SemanticTree.ITypeNode SemanticTree.IAsNode.right
        {
            get
            {
                return _right;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.as_node;
            }
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }
    }

    public class is_node : expression_node, SemanticTree.IIsNode
    {
        private expression_node _left;
        private type_node _right;

        public is_node(expression_node left, type_node right, location loc)
            : base(compiled_type_node.get_type_node(typeof(bool)), loc)
        {
            _left = left;
            _right = right;
        }

        public expression_node left
        {
            get
            {
                return _left;
            }
        }

        public type_node right
        {
            get
            {
                return _right;
            }
        }

        SemanticTree.IExpressionNode SemanticTree.IIsNode.left
        {
            get
            {
                return _left;
            }
        }

        SemanticTree.ITypeNode SemanticTree.IIsNode.right
        {
            get
            {
                return _right;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.is_node;
            }
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }
    }

    public class sizeof_operator : expression_node, SemanticTree.ISizeOfOperator
    {
        private type_node _oftype;

        public sizeof_operator(type_node oftype, location loc)
            : base(compiled_type_node.get_type_node(typeof(int)), loc)
        {
            _oftype = oftype;
        }

        public type_node oftype
        {
            get
            {
                return _oftype;
            }
        }

        SemanticTree.ITypeNode SemanticTree.ISizeOfOperator.oftype
        {
            get
            {
                return _oftype;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.sizeof_operator;
            }
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }
    }

    public class typeof_operator : expression_node, SemanticTree.ITypeOfOperator
    {
        private type_node _oftype;

        public typeof_operator(type_node oftype, location loc)
            : base(compiled_type_node.get_type_node(typeof(Type),SystemLibrary.SystemLibrary.syn_visitor.SymbolTable), loc)
        {
            _oftype = oftype;
        }

        public type_node oftype
        {
            get
            {
                return _oftype;
            }
        }

        SemanticTree.ITypeNode SemanticTree.ITypeOfOperator.oftype
        {
            get
            {
                return _oftype;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.typeof_operator;
            }
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }
    }

    public class statements_expression_node : expression_node, SemanticTree.IStatementsExpressionNode
    {
        private statement_node_list _statements_list;
        private expression_node _expression;

        public statements_expression_node(statement_node_list statements, expression_node expression, location loc)
            : base(expression.type, loc)
        {
            this._statements_list = statements;
            this._expression = expression;
        }

        public statement_node_list internal_statements
        {
            get
            {
                return _statements_list;
            }
        }

        public expression_node internal_expression
        {
            get
            {
                return _expression;
            }
        }

        public SemanticTree.IStatementNode[] statements
        {
            get
            {
                return _statements_list.ToArray();
            }
        }

        public SemanticTree.IExpressionNode expresion
        {
            get
            {
                return _expression;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.statement_expression_node;
            }
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }
    }

    public class question_colon_expression : expression_node, SemanticTree.IQuestionColonExpressionNode
    {
        private expression_node _condition;
        private expression_node _ret_if_true;
        private expression_node _ret_if_false;

        public question_colon_expression(expression_node condition, expression_node ret_if_true, expression_node ret_if_false,
            location loc)
            : base(ret_if_true is null_const_node ? ret_if_false.type : ret_if_true.type, loc)
        {
            this._condition = condition;
            this._ret_if_true = ret_if_true;
            this._ret_if_false = ret_if_false;
        }

        public expression_node internal_condition
        {
            get
            {
                return _condition;
            }
        }

        public expression_node internal_ret_if_true
        {
            get
            {
                return _ret_if_true;
            }
        }

        public expression_node internal_ret_if_false
        {
            get
            {
                return _ret_if_false;
            }
        }

        public SemanticTree.IExpressionNode condition
        {
            get
            {
                return _condition;
            }
        }

        public SemanticTree.IExpressionNode ret_if_true
        {
            get
            {
                return _ret_if_true;
            }
        }

        public SemanticTree.IExpressionNode ret_if_false
        {
            get
            {
                return _ret_if_false;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.question_colon_expression;
            }
        }
        
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }
    }
	
    public class array_initializer : expression_node, SemanticTree.IArrayInitializer
    {
    	private List<expression_node> _element_values;

        public List<expression_node> element_values
        {
            get
            {
                return _element_values;
            }
            set
            {
            	_element_values = value;
            }
        }

        SemanticTree.IExpressionNode[] SemanticTree.IArrayInitializer.ElementValues
        {
            get
            {
                return _element_values.ToArray();
            }
        }

        SemanticTree.ITypeNode SemanticTree.IArrayInitializer.ElementType
        {
            get
            {
                return element_type;
            }
        }

        public array_initializer(List<expression_node> element_values, location loc)
            :
            base(null, loc)
        {
            this._element_values = element_values;
        }


        public type_node element_type
        {
            get
            {
                return element_values[0].type;
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

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.array_initializer;
            }
        }
    }
    
    public class record_initializer : expression_node, SemanticTree.IRecordInitializer
    {
    	private List<expression_node> _field_values = new List<expression_node>();
        
        public List<expression_node> field_values
        {
            get
            {
                return _field_values;
            }
        }

        internal List<SyntaxTree.record_const_definition> record_const_definition_list;

        public record_initializer(List<expression_node> field_values, location loc)
            :base(null, loc)
        {
            _field_values = field_values;
        }

        internal record_initializer(List<SyntaxTree.record_const_definition> record_const_definition_list, location loc)
            : base(null, loc)
        {
            this.record_const_definition_list = record_const_definition_list;
        }
		
        public SemanticTree.IExpressionNode[] FieldValues
        {
            get
            {
                return _field_values.ToArray();
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

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.record_initializer;
            }
        }
    }
}
