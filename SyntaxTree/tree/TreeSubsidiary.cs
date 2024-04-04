using System;
using System.Collections.Generic;

namespace PascalABCCompiler.SyntaxTree
{
    public enum LanguageId { CommonLanguage=32, PascalABCNET=2, C=4, VBNET=8, Oberon00 = 16}

	public enum oberon_export_marker {export,export_readonly};

    public enum op_typecast { is_op, as_op, typecast };

    public enum WhileCycleType { While, DoWhile };

    public enum UnitHeaderKeyword { Unit, Library, Namespace }

    public enum JumpStmtType { Return, Break, Continue };
    public enum SwitchPartType { Switch, Case, Default };

    public enum DeclarationSpecificator { WhereDefConstructor, WhereDefClass, WhereDefValueType };
    
	public enum Operators 
	{                            // Pascal  C
		Undefined,
        
        //Arithmetic operators
        Plus,                   //  +       +
        Minus,                  //  -       -
        Multiplication,         //  *       *   
        Division,               //  /       /
        ModulusRemainder,       //  mod     %
        AssignmentAddition,     //  +=      +=
        AssignmentSubtraction,  //  -=      -=
        AssignmentMultiplication,// *=      *= 
        AssignmentDivision,     //  /=      /=
        AssignmentModulus,      //          %=
        PrefixIncrement,        //          ++()
        PostfixIncrement, 	    //          ()++
        PrefixDecrement,        //          --()
        PostfixDecrement, 	    //          ()--
        
        //Comparison operators
        LogicalAND,             //  and     &&
        LogicalOR,              //  or      ||
        IntegerDivision,        //  div     
        Less,                   //  <       < 
        Greater,                //  >       >
        LessEqual,              //  <=      <= 
        GreaterEqual,           //  >=      >=
        Equal,                  //  =       ==
        NotEqual,               //  <>      !=
        LogicalNOT,             //  not     !
        
        //Bitwise operators
        BitwiseLeftShift,       //  shl     <<                 
        BitwiseRightShift,      //  shr     >>
        BitwiseAND,             //          &
        BitwiseOR,              //          |
        BitwiseXOR,             //  xor     ^
        BitwiseNOT,             //          ~
        AssignmentBitwiseLeftShift,//       <<=
        AssignmentBitwiseRightShift,//      >>=
        AssignmentBitwiseAND,   //          &=
        AssignmentBitwiseOR,    //          |=
        AssignmentBitwiseXOR,   //          ^=

        //Other operators
        Assignment,             //  :=      =                 
        In,                     //  in
        NotIn,                  //  not in
        Is,                     //  is
        As,                     //  as
        Dereference,            //  ^       *
        AddressOf,              //  @       &
        MemberByPointer,        //          ->
        Member,                 //  .       .
        Implicit,
        Explicit,
        Deref,                   // ^ SSM 3.02.12
        Power
    };

    public class OperatorServices
    {
        public static bool IsAssigmentOperator(Operators Operator)
        {
            switch (Operator)
            {
                case Operators.AssignmentAddition: 
                case Operators.AssignmentSubtraction: 
                case Operators.AssignmentMultiplication: 
                case Operators.AssignmentDivision: 
                case Operators.Assignment: return true;          
            }
            return false;
        }
        public static string ToString(Operators Operator, LanguageId Language)
        {
            if (Language == LanguageId.PascalABCNET)
            {
                switch (Operator)
                {
                    case Operators.Plus: return "+";
                    case Operators.Minus: return "-";                 
                    case Operators.Multiplication: return "*";    
                    case Operators.Division: return "/";               
                    case Operators.ModulusRemainder: return "mod";     
                    case Operators.AssignmentAddition: return "+=";    
                    case Operators.AssignmentSubtraction: return "-="; 
                    case Operators.AssignmentMultiplication: return "*=";
                    case Operators.AssignmentDivision: return "/=";     
                    case Operators.LogicalAND: return "and";           
                    case Operators.LogicalOR: return "or";             
                    case Operators.IntegerDivision: return "div";      
                    case Operators.Less: return "<";                   
                    case Operators.Greater: return ">";                
                    case Operators.LessEqual: return "<=";             
                    case Operators.GreaterEqual: return ">=";          
                    case Operators.Equal: return "=";                  
                    case Operators.NotEqual: return "<>";              
                    case Operators.LogicalNOT: return "not";      
                    case Operators.BitwiseLeftShift: return "shl"; 
                    case Operators.BitwiseRightShift: return "shr";
                    case Operators.BitwiseXOR: return "xor";       
                    case Operators.Assignment: return ":=";          
                    case Operators.In: return "in";                  
                    case Operators.Is: return "is";                  
                    case Operators.As: return "as"; 
                    case Operators.Power: return "**";
                }
            }
            else
            if (Language == LanguageId.C)
            {
                switch (Operator)
                {
                    case Operators.Plus: return "+";
                    case Operators.Minus: return "-";
                    case Operators.Multiplication: return "*";
                    case Operators.Division: return "/";              
                    case Operators.ModulusRemainder: return "%";      
                    case Operators.AssignmentAddition: return "+=";   
                    case Operators.AssignmentSubtraction: return "-=";
                    case Operators.AssignmentMultiplication: return "*=";
                    case Operators.AssignmentDivision: return "/=";    
                    case Operators.AssignmentModulus: return "%=";     
                    case Operators.PrefixIncrement: return "++";     
                    case Operators.PostfixIncrement: return "++"; 	   
                    case Operators.PrefixDecrement: return "--";     
                    case Operators.PostfixDecrement: return "--"; 	   
                    case Operators.LogicalAND: return "%%";            
                    case Operators.LogicalOR: return "||";             
                    case Operators.Less: return "<";                   
                    case Operators.Greater: return ">";                
                    case Operators.LessEqual: return "<=";             
                    case Operators.GreaterEqual: return ">=";          
                    case Operators.Equal: return "==";                 
                    case Operators.NotEqual: return "!=";              
                    case Operators.LogicalNOT: return "!";        
                    case Operators.BitwiseLeftShift: return "<<";      
                    case Operators.BitwiseRightShift: return ">>";     
                    case Operators.BitwiseAND: return "&";             
                    case Operators.BitwiseOR: return "|";              
                    case Operators.BitwiseXOR: return "^";             
                    case Operators.BitwiseNOT: return "~";   
                    case Operators.AssignmentBitwiseLeftShift: return "<<=";
                    case Operators.AssignmentBitwiseRightShift: return ">>=";
                    case Operators.AssignmentBitwiseAND: return "&=";  
                    case Operators.AssignmentBitwiseOR: return "|=";   
                    case Operators.AssignmentBitwiseXOR: return "^=";  
                    case Operators.Assignment: return "=";             
                }
            }
            return null;
        }
    }

	public enum for_cycle_type {to,downto};
	
	public enum proc_attribute {attr_override, attr_forward, attr_virtual, attr_overload, attr_reintroduce, attr_abstract, attr_static, attr_extension, attr_none }; // attr_none нужно для свойств когда virtual И override не указывается

    public enum definition_attribute {None, Static, Const};

	public enum access_modifer {public_modifer, protected_modifer, private_modifer, published_modifer, internal_modifer, none};

	public enum known_type {string_type};

    public enum parametr_kind { none, var_parametr, const_parametr, out_parametr, params_parametr };

    [FlagsAttribute] public enum class_attribute { None = 0, Sealed = 1, Partial = 2, Abstract = 4, Auto = 8, Static=16 }; // Auto - SSM 24.03.14
    public enum class_keyword { Class, Interface, Record, Struct, Union, TemplateClass, TemplateRecord, TemplateInterface };

    public enum c_scalar_type_name { tn_char, tn_int, tn_short, tn_long, tn_short_int, tn_long_int, tn_float, tn_double, tn_void};

    public enum c_scalar_sign { none, signed, unsigned };

    [Serializable]
    public class file_position
	{
		private int _line_num;
		private int _column_num;

		public file_position(int line_num,int column_num)
		{
			this._line_num=line_num;
            this._column_num = column_num;
		}

		public int line_num
		{
			get
			{
				return _line_num;
			}
            set
            {
                _line_num = value;
            }
		}

		public int column_num
		{
			get
			{
				return _column_num;
			}
            set
            {
                _column_num = value;
            }
		}

	}

    [Serializable]
    public class SourceContext
	{
		private file_position _begin_position;
		private file_position _end_position;
        private int _begin_symbol_position;
        private int _end_symbol_position;
        private string _file_name=null;
        public string FileName
        {
            get
            {
                return _file_name;
            }
            set
            {
                _file_name = value;
            }
        }

        /// <summary>
        /// дефолтный конструктор (присваивает всем позициям единицы, что соответствует позиции начала файла)
        /// </summary>
        public SourceContext() : this(1, 1, 1, 1, 1, 1) { }

        public SourceContext(int beg_line_num, int beg_column_num, int end_line_num, int end_column_num, int _begin_symbol_position, int _end_symbol_position)
		{
			_begin_position=new file_position(beg_line_num,beg_column_num);
			_end_position=new file_position(end_line_num,end_column_num);
            this._begin_symbol_position = _begin_symbol_position;
            this._end_symbol_position = _end_symbol_position;
		}
        public SourceContext(int beg_line_num, int beg_column_num, int end_line_num, int end_column_num, string filename = null)
        {
            _begin_position = new file_position(beg_line_num, beg_column_num);
            _end_position = new file_position(end_line_num, end_column_num);
            this._begin_symbol_position = 0;
            this._end_symbol_position = 0;
            _file_name = filename;
        }

        public SourceContext(SourceContext left, SourceContext right)
        {
            if (left != null)
                _begin_position = new file_position(left.begin_position.line_num, left.begin_position.column_num);
            if (right != null)
                _end_position = new file_position(right.end_position.line_num, right.end_position.column_num);
            if (left == null)
                _begin_position = _end_position;
            if (right == null)
                _end_position = _begin_position;
            if (left != null)
                this._begin_symbol_position = left._begin_symbol_position;
            if (right != null)
                this._end_symbol_position = right._end_symbol_position;
            if (left != null && right != null)
                this.FileName = left.FileName == right.FileName ? left.FileName : null;
        }

        public SourceContext(SourceContext sc)
        {
            if (sc == null)
                throw new ArgumentNullException(nameof(sc));

            if (sc._begin_position != null)
                _begin_position = new file_position(sc._begin_position.line_num, sc._begin_position.column_num);
            if (sc._end_position != null)
                _end_position = new file_position(sc._end_position.line_num, sc.end_position.column_num);
            _begin_symbol_position = sc._begin_symbol_position;
            _end_symbol_position = sc._end_symbol_position;
            _file_name = sc._file_name;
        }

        public SourceContext Merge(SourceContext right)
        {
            return new SourceContext(this, right);
        }

		public file_position begin_position
		{
			get
			{
				return _begin_position;
			}
		}

		public file_position end_position
		{
			get
			{
				return _end_position;
			}
		}

        public int Position
        {
            get
            {
                return _begin_symbol_position;
            }
        }

        public int Length
        {
            get
            {
                return _end_symbol_position-_begin_symbol_position;
            }
        }

        public SourceContext LeftSourceContext
        {
            get
            {
                SourceContext sc = new SourceContext(begin_position.line_num, begin_position.column_num, begin_position.line_num, begin_position.column_num);
                sc.FileName = FileName;
                return sc;
            }
        }
        public SourceContext RightSourceContext
        {
            get
            {
                SourceContext sc = new SourceContext(end_position.line_num, end_position.column_num, end_position.line_num, end_position.column_num);
                sc.FileName = FileName;
                return sc;
            }
        }

		public override string ToString()
		{
            return string.Format("[({0},{1})-({2},{3})]",
				begin_position.line_num,begin_position.column_num,end_position.line_num,end_position.column_num);
		}
        public bool Eq(SourceContext sc)
        {
            return begin_position.line_num == sc.begin_position.line_num && begin_position.column_num == sc.begin_position.column_num
                && end_position.line_num == sc.end_position.line_num && end_position.column_num == sc.end_position.column_num;
        }
        public bool In(SourceContext sc)
        {
            //return Position >= sc.Position && (Length + Position <= sc.Length + sc.Position);
            return
                !((begin_position.line_num < sc.begin_position.line_num) ||
                (end_position.line_num > sc.end_position.line_num) ||
                (begin_position.line_num == sc.begin_position.line_num && begin_position.column_num < sc.begin_position.column_num) ||
                (end_position.line_num == sc.end_position.line_num && end_position.column_num > sc.end_position.column_num));
        }
        public bool Between(SourceContext sc1, SourceContext sc2)
        {
            return
                (begin_position.line_num > sc1.begin_position.line_num || (begin_position.line_num == sc1.begin_position.line_num && begin_position.column_num > sc1.begin_position.column_num)) &&
                (end_position.line_num < sc2.begin_position.line_num || (end_position.line_num == sc2.begin_position.line_num && end_position.column_num < sc2.begin_position.column_num));
        }
        public bool Less(SourceContext sc)
        {
            return begin_position.line_num < sc.begin_position.line_num || (begin_position.line_num == sc.begin_position.line_num && begin_position.column_num < sc.begin_position.column_num);
        }

    }

    public class Utils
    {
        public static string IdentListToString(List<ident> list, string separator)
        {
            string res="";
            if (list.Count > 0)
            {
                res = list[0].name;
                for (int i = 1; i < list.Count; i++)
                    res += separator + list[i].name;
            }
            return res;

        }
    }

    public class base_syntax_namespace_node: declaration
    {
        string _name;
        List<declaration> _defs;
        uses_list _uses_modules;

        public base_syntax_namespace_node(string name)
        {
            _name = name;
            _defs = new List<declaration>();
        }

        public string name { get => _name; set => _name = value; }
        public List<declaration> defs { get => _defs; set => _defs = value; }
        public uses_list uses_modules { get => _uses_modules; set => _uses_modules = value; }
 
    }
}
