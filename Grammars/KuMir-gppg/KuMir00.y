%{
    public syntax_tree_node root; // Корневой узел синтаксического дерева 	
    public GPPGParser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
    public int unit_number;
    public System.Collections.ArrayList _units = new System.Collections.ArrayList(); // for modules

	///////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////
        //Some methods
        ///////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////

        public class unit_data
        {
            public declarations sub_progs;
            public statement_list initialization;
            public ident_list used_units;

            public unit_data()
            {
                sub_progs = new declarations();
                used_units = new ident_list();
                initialization = new statement_list();
                initialization.subnodes.Add(new empty_statement());
            }

            public void ClearData()
            {
                sub_progs.defs.Clear();
                used_units.idents.Clear();
                initialization.subnodes.Clear();
                initialization.subnodes.Add(new empty_statement());
            }
        }

        public statement_list GetStatements(object _object)
        {
            statement_list _statement_list;
            if (_object is statement_list)
                _statement_list = _object as statement_list;
            else
            {
                _statement_list = new statement_list();
                if (_object is statement)
                    _statement_list.subnodes.Add(_object as statement);
                else
                    _statement_list.subnodes.Add(new empty_statement());
            }
            return _statement_list;
        }

        public expression_list GetExpressions(object _object)
        {
            expression_list _expression_list;
            if (_object is expression_list)
                _expression_list = _object as expression_list;
            else
            {
                _expression_list = new expression_list();
                _expression_list.expressions.Add(_object as expression);
            }
            return _expression_list;
        }

        public ident_list GetIdents(object _object)
        {
            ident_list _ident_list;
            if (_object is ident_list)
                _ident_list = _object as ident_list;
            else
            {
                _ident_list = new ident_list();
                _ident_list.idents.Add(_object as ident);
            }
            return _ident_list;
        }

        public indexers_types GetIndexers(object _object)
        {
            indexers_types _indexers_types;
            if (_object is indexers_types)
                _indexers_types = _object as indexers_types;
            else
            {
                _indexers_types = new indexers_types();
                _indexers_types.indexers.Add(_object as diapason);

            }
            return _indexers_types;
        }

        public formal_parametres GetFormals(object _object)
        {
            if (_object == null)
                return null;
            formal_parametres _formal_parametres;
            if (_object is formal_parametres)
                _formal_parametres = _object as formal_parametres;
            else
            {
                _formal_parametres = new formal_parametres();
                _formal_parametres.params_list.Add(_object as typed_parametres);
            }
            return _formal_parametres;
        }

///////////////////////
%}

%output=KuMir00yacc.cs 

%parsertype GPPGParser

%union  
	{ 
		public bool bVal; 
		public string sVal; 
	      public int iVal; 
		public double rVal;
		public char chVal;		
		public named_type_reference ntr;
		public ident_list il;
		public var_def_statement vds;
		public variable_definitions vdss;
		public expression ex;
		public expression_list el;
		public ident id;
		public statement st;
		public statement_list sl;
		public declarations decl;
		public Operators op;
		public case_variant cv;
		public diapason dp;
		public indexers_types it;	
		public formal_parametres fps;
		public procedure_definition pd;
	}

%using PascalABCCompiler.SyntaxTree
%using PascalABCCompiler.Errors
%using PascalABCCompiler.KuMir00Parser

%namespace GPPGParserScanner

%start module

%token <sVal> ID STRING ENDL
%token <iVal> INTNUM 
%token <bVal> TK_TRUE TK_FALSE
%token <rVal> REAL
%token <chVal> CHAR
%token <op> PLUS MINUS MULT DIVIDE AND OR LT GT LE GE EQ NE
%token TK_POWER
%token ASSIGN SEMICOLUMN LPAREN RPAREN COLUMN COMMA COLON TK_NEWLINE
%token TRUE FALSE ODD BOOLEAN INTEGER
%token INVISIBLE

%token <sVal> TK_COMMENT_LINE TK_EOL
%token TK_ALG TK_USES TK_ARG TK_VAR TK_FUNC_VAL TK_BEGIN TK_END TK_BEGIN_CYCLE TK_END_CYCLE TK_ARRAY TK_ISP
%token TK_ASSERT TK_READ TK_WRITE
%token TK_IF TK_THEN TK_ELSE TK_END_ALL TK_CASE TK_CASE_V TK_WHILE TK_FOR TK_FROM TK_TO TK_RAZ
%token <op> TK_AND TK_OR
%token TK_NOT
%token TK_INTEGER_TYPE TK_REAL_TYPE TK_BOOLEAN_TYPE TK_CHAR_TYPE TK_STRING_TYPE
%token TK_DOT TK_ROUND_OPEN TK_ROUND_CLOSE TK_SQUARE_OPEN TK_SQUARE_CLOSE

%type <id> ident
%type <ntr> type
%type <ex> expr
%type <st> Assignment Statement If_Statement While_Statement
%type <st> EmptyStatement Case_variant_list Case_statement For_statement Proccall_statement Var_Decl
%type <sl> StatementSequence Global_decl_list
%type <decl> Sub_declarations Global_vars
%type <el> factparams
%type <il> IDList
%type <vdss>Array_list
%type <cv> Case_variant
%type <dp> Diap
%type <it> Diap_list
%type <fps> Formal_parameter Formal_type_list Formal_list
%type <pd> Procedure Function

%left LT GT LE GE EQ NE
%left PLUS MINUS TK_OR
%left MULT DIVIDE TK_AND TK_POWER
%left TK_NOT
%left UMINUS

%%

module  
	: Separators_Opt Global_part TK_ALG ident Separators TK_BEGIN StatementSequence TK_END EmptyLines Sub_declarations
    {		
		// Формирование главной программы
            program_module _program_module;
            program_name _program_name = new program_name($4);
            if ((_units[this.unit_number - 1] as unit_data).initialization.subnodes.Count != 0)
	            $7.subnodes.InsertRange(0, (_units[this.unit_number - 1] as unit_data).initialization.subnodes);

		block b;
		if ($10 != null)
		{
			if ((_units[this.unit_number - 1] as unit_data).sub_progs.defs.Count != 0)
				$10.defs.InsertRange(0,(_units[this.unit_number - 1] as unit_data).sub_progs.defs );
			$10.source_context = PT.ToSourceContext(@10.Merge(@10));
			b = new block($10, $7);
		}
		else

		{
			if ((_units[this.unit_number - 1] as unit_data).sub_progs.defs.Count != 0)
				b = new block((_units[this.unit_number - 1] as unit_data).sub_progs, $7);
			else
				b = new block(null, $7);
		}

		(_units[this.unit_number - 1] as unit_data).used_units.idents.Add(new ident("MathForKumir"));
            if ((_units[this.unit_number - 1] as unit_data).used_units.idents.Count != 0)
            {
 	           unit_or_namespace _unit_or_namespace;
                 uses_list _uses_list = new uses_list();

                 for (int i = 0; -i > -(_units[this.unit_number - 1] as unit_data).used_units.idents.Count; i++)
                 {
      	           ident_list _ident_list = new ident_list();
                       _ident_list.idents.Add((_units[this.unit_number - 1] as unit_data).used_units.idents[i]);
                       _unit_or_namespace = new unit_or_namespace(_ident_list);
                       _uses_list.units.Add(_unit_or_namespace);
                 }
                 _program_module = new program_module(_program_name, _uses_list, b, null);
            }
            else
			_program_module = new program_module(_program_name, null, b, null);

		
		b.source_context = PT.ToSourceContext(@6.Merge(@8));
		_program_name.source_context = PT.ToSourceContext(@4.Merge(@4));
		//var r = new program_module(null, ul, b, null);
		_program_module.Language = LanguageId.PascalABCNET;
		_program_module.source_context = PT.ToSourceContext(@$);
		root = _program_module;
    }
	| 
	Separators_Opt TK_ISP ident Separators Global_part Sub_declarations TK_END EmptyLines	{
	      interface_node _interface_node;
            (_units[this.unit_number - 1] as unit_data).used_units.idents.Add(new ident("MathForKumir"));
            if ((_units[this.unit_number - 1] as unit_data).used_units.idents.Count > 0)
	      {
      	      unit_or_namespace _unit_or_namespace;
                  uses_list _uses_list = new uses_list();
                  for (int i = 0; -i > -(_units[this.unit_number - 1] as unit_data).used_units.idents.Count; i++)
                  {
            	      ident_list _ident_list = new ident_list();
                  	_ident_list.idents.Add((_units[this.unit_number - 1] as unit_data).used_units.idents[i]);
                        _unit_or_namespace = new unit_or_namespace(_ident_list);
                        _uses_list.units.Add(_unit_or_namespace);
                  }
			if ($6!=null)
				(_units[this.unit_number - 1] as unit_data).sub_progs.defs.AddRange($6.defs);
                  _interface_node = new interface_node((_units[this.unit_number - 1] as unit_data).sub_progs, _uses_list, null); 
            }
            else
		{
			if ($6!=null)
				(_units[this.unit_number - 1] as unit_data).sub_progs.defs.AddRange($6.defs);
            	_interface_node = new interface_node((_units[this.unit_number - 1] as unit_data).sub_progs, null, null);
		}
            unit_module _unit_module = new unit_module(new unit_name($3, 0), _interface_node, null, (_units[this.unit_number - 1] as unit_data).initialization, null);
            _unit_module.Language = LanguageId.PascalABCNET;
            unit_name _unit_name = new unit_name($3, 0);
            _unit_module.source_context = PT.ToSourceContext(@$);
            _unit_name.source_context = PT.ToSourceContext(@3.Merge(@3));
	      root = _unit_module;
	}
	| INVISIBLE expr { // Для Intellisense
		root = $2;
	}
    ;

Separator 		
	: SEMICOLUMN {
		
	}
	| 
	ENDL {

	}
	;

Separators
	: Separator {
	}
	|
	Separator Separators {
	}
	;

Separators_Opt
	: Separator Separators_Opt {
	}
	|
	{
	}
	;

EmptyLines 		
	:	
	| 
	EmptyLines ENDL {

	}
	;

ident 
	: ID {
		$$ = new ident($1); 
		$$.source_context = PT.ToSourceContext(@$);
	}
	;
	
expr 	
	: ident {
		$$ = $1;
	}
      |  ident TK_SQUARE_OPEN factparams TK_SQUARE_CLOSE	 {
	      indexer _indexer = new indexer(GetExpressions($3));
            _indexer.dereferencing_value = $1;
		_indexer.source_context = PT.ToSourceContext(@3.Merge(@3));
      	$$ = _indexer as expression;    
      }
	|ident TK_SQUARE_OPEN factparams TK_SQUARE_CLOSE TK_SQUARE_OPEN expr TK_SQUARE_CLOSE  {

	      indexer _indexer = new indexer(GetExpressions($3));
            indexer _indexer1 = new indexer(GetExpressions($6));
            _indexer.dereferencing_value = $1;
            _indexer1.dereferencing_value = _indexer;
		_indexer.source_context = PT.ToSourceContext(@3.Merge(@3));
            _indexer1.source_context = PT.ToSourceContext(@6.Merge(@6));
		$$ = _indexer1 as expression;
	}
	| ident LPAREN factparams RPAREN {
		method_call mc = new method_call($3);
		mc.dereferencing_value = $1;
		switch ($1.name)
            {
	            case "tg": mc.dereferencing_value = new ident("tan"); break;
                  case "ctg": mc.dereferencing_value = new ident("ctg"); break;
                  case "arctg": mc.dereferencing_value = new ident("arctan"); break;
                  case "arcctg": mc.dereferencing_value = new ident("arcctg"); break;
                  case "lg": mc.dereferencing_value = new ident("log10"); break;
                  case "mod": mc.dereferencing_value = new ident("md"); break;
                  case "div": mc.dereferencing_value = new ident("dv"); break;
                  case "rnd": mc.dereferencing_value = new ident("random"); break;
                  case "int": mc.dereferencing_value = new ident("round"); break;
            }
		
		$$ = mc as expression;
		$$.source_context = PT.ToSourceContext(@$);
	}
    | INTNUM { 
		$$ = new int32_const($1); 		
		$$.source_context = PT.ToSourceContext(@$);
	}
    | REAL { 
		$$ = new double_const($1); 		
		$$.source_context = PT.ToSourceContext(@$);
	}
    | CHAR { 
		$$ = new char_const($1); 		
		$$.source_context = PT.ToSourceContext(@$);
	}
    | STRING { 
		$$ = new string_const($1); 		
		$$.source_context = PT.ToSourceContext(@$);
	}

    | TK_EOL { 
		 literal_const_line _literal_const_line = new literal_const_line();
             sharp_char_const _sharp_char_const_13 = new sharp_char_const(13);
             sharp_char_const _sharp_char_const_10 = new sharp_char_const(10);
             _literal_const_line.literals.Add(_sharp_char_const_13);
             _literal_const_line.literals.Add(_sharp_char_const_10);
  		 _sharp_char_const_13.source_context = PT.ToSourceContext(@$);
		 _sharp_char_const_10.source_context = PT.ToSourceContext(@$);
             $$ = _literal_const_line;
   		 $$.source_context = PT.ToSourceContext(@$);
	}
    | TRUE {
		$$ = new bool_const(true);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | TK_TRUE {
		$$ = new bool_const(true);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | FALSE {
		$$ = new bool_const(false);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | TK_FALSE {
		$$ = new bool_const(false);
		$$.source_context = PT.ToSourceContext(@$);
	}	
	| MINUS expr %prec UMINUS {
		$$ = new un_expr($2,Operators.Minus);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | LPAREN expr RPAREN {$$ = $2;}
	
    | TK_NOT expr {
		$$ = new un_expr($2,Operators.LogicalNOT);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | expr PLUS expr {
		$$ = new bin_expr($1,$3,Operators.Plus);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | expr MINUS expr {
		$$ = new bin_expr($1,$3,Operators.Minus);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | expr MULT expr {
		$$ = new bin_expr($1,$3,Operators.Multiplication);
		$$.source_context = PT.ToSourceContext(@$);
	}
    |expr TK_POWER expr {
 		expression_list _expression_list = new expression_list();
            _expression_list.expressions.Add($1);
            _expression_list.expressions.Add($3);
            method_call _method_call = new method_call(_expression_list);
            _method_call.dereferencing_value = new ident("power");
 
		_method_call.dereferencing_value.source_context = PT.ToSourceContext(@2.Merge(@2));
            _method_call.source_context = PT.ToSourceContext(@$);
		$$ = _method_call as expression ;
    }
    | expr DIVIDE expr {
		$$ = new bin_expr($1,$3,Operators.IntegerDivision);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | expr TK_AND expr {
		$$ = new bin_expr($1,$3,Operators.LogicalAND);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | expr TK_OR expr {
		$$ = new bin_expr($1,$3,Operators.LogicalOR);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | expr EQ expr {
		$$ = new bin_expr($1,$3,Operators.Equal);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | expr NE expr {
		$$ = new bin_expr($1,$3,Operators.NotEqual);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | expr LT expr {
		$$ = new bin_expr($1,$3,Operators.Less);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | expr LE expr {
		$$ = new bin_expr($1,$3,Operators.LessEqual);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | expr GT expr {
		$$ = new bin_expr($1,$3,Operators.Greater);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | expr GE expr {
		$$ = new bin_expr($1,$3,Operators.GreaterEqual);
		$$.source_context = PT.ToSourceContext(@$);
	}
	;

Assignment 
	: ident ASSIGN expr {
		$$ = new assign($1, $3, Operators.Assignment);
		$$.source_context = PT.ToSourceContext(@$);
	}
	|
	ident TK_SQUARE_OPEN factparams TK_SQUARE_CLOSE	ASSIGN expr {
	      indexer _indexer = new indexer(GetExpressions($3));
            _indexer.dereferencing_value = $1;
		_indexer.source_context = PT.ToSourceContext(@3.Merge(@3));
		$$ = new assign(_indexer, $6, Operators.Assignment);
		$$.source_context = PT.ToSourceContext(@$);
	}
	|ident TK_SQUARE_OPEN factparams TK_SQUARE_CLOSE TK_SQUARE_OPEN expr TK_SQUARE_CLOSE ASSIGN expr {
	      indexer _indexer = new indexer(GetExpressions($3));
            indexer _indexer1 = new indexer(GetExpressions($6));
            _indexer.dereferencing_value = $1;
            _indexer1.dereferencing_value = _indexer;
		_indexer.source_context = PT.ToSourceContext(@3.Merge(@3));
            _indexer1.source_context = PT.ToSourceContext(@6.Merge(@6));
		$$ = new assign(_indexer1, $9, Operators.Assignment);
		$$.source_context = PT.ToSourceContext(@$);
    	}	
	;

If_Statement 
	: TK_IF expr TK_THEN StatementSequence TK_END_ALL {
		$$ = new if_node($2, $4, null);
		$$.source_context = PT.ToSourceContext(@$);
	}
	| TK_IF expr TK_THEN StatementSequence TK_ELSE StatementSequence TK_END_ALL {
		$$ = new if_node($2, $4, $6);
		$$.source_context = PT.ToSourceContext(@$);
	}
	;


While_Statement 
	: TK_BEGIN_CYCLE TK_WHILE expr StatementSequence TK_END_CYCLE {
		$$ = new while_node($3, $4, WhileCycleType.While);
		$$.source_context = PT.ToSourceContext(@$);
	}
	;
	
factparams
	: expr {
		$$ = new expression_list();
		$$.expressions.Add($1);
		$$.source_context = PT.ToSourceContext(@$);
	}
	| factparams COLUMN expr {
		$1.expressions.Add($3);
		$$ = $1;
		$$.source_context = PT.ToSourceContext(@$);
	}
	;
	
	
EmptyStatement
	: {
		$$ = new empty_statement();		
	}
	;
	
Statement 
	: Assignment
	| EmptyStatement
	| If_Statement
      | While_Statement	
	| Case_statement
	| For_statement
	| Proccall_statement
	| Var_Decl 
	;

StatementSequence 
	: Statement {
		$$ = GetStatements($1);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | StatementSequence Separator Statement {
		statement_list sl = GetStatements($3);
		for (int i=0; i < sl.subnodes.Count; i++)
			$1.subnodes.Add(sl.subnodes[i]);
		$$ = $1;
		$$.source_context = PT.ToSourceContext(@$);
	}
	;

type 	
	: BOOLEAN {
		$$ = new named_type_reference();
		$$.names.Add(new ident("boolean"));
		$$.source_context = PT.ToSourceContext(@$);
	}
	| INTEGER {
		$$ = new named_type_reference();
		$$.names.Add(new ident("integer"));
		$$.source_context = PT.ToSourceContext(@$);
	}
	| TK_INTEGER_TYPE {
		$$ = new named_type_reference();
		$$.names.Add(new ident("integer"));
		$$.source_context = PT.ToSourceContext(@$);
	}
	
	| TK_REAL_TYPE {
		$$ = new named_type_reference();
		$$.names.Add(new ident("real"));
		$$.source_context = PT.ToSourceContext(@$);
	}	
	| TK_BOOLEAN_TYPE {
		$$ = new named_type_reference();
		$$.names.Add(new ident("boolean"));
		$$.source_context = PT.ToSourceContext(@$);
	}
	| TK_CHAR_TYPE {
		$$ = new named_type_reference();
		$$.names.Add(new ident("char"));
		$$.source_context = PT.ToSourceContext(@$);
	}
	| TK_STRING_TYPE {
		$$ = new named_type_reference();
		$$.names.Add(new ident("string"));
		$$.source_context = PT.ToSourceContext(@$);
	}	
	;

IDList 
	: ident {
		$$=new ident_list();
		$$.idents.Add($1);
		$$.source_context = PT.ToSourceContext(@$);
	}
    | IDList COLUMN ident {
		$1.idents.Add($3);
		$$ = $1;
		$$.source_context = PT.ToSourceContext(@$);
	}
	;

Var_Decl 
	: type IDList{
	      var_def_statement _var_def_statement = new var_def_statement(GetIdents($2), $1, null, definition_attribute.None, false);
            var_statement _var_statement = new var_statement(_var_def_statement);
   		_var_def_statement.source_context = PT.ToSourceContext(@1.Merge(@2));		
		_var_statement.source_context = PT.ToSourceContext(@1.Merge(@2));		
		$$ = _var_statement;
	}
	|  
	type TK_ARRAY Array_list{	
		statement_list var_statement_list = new statement_list();
	      variable_definitions _variable_definitions = $3;
            for (int i = 0; -i > -(_variable_definitions.var_definitions.Count); i++)
            {
            	((_variable_definitions.var_definitions[i]).vars_type as array_type).elemets_types = $1;
                  var_statement _var_statement = new var_statement((var_def_statement)_variable_definitions.var_definitions[i]);
                  var_statement_list.subnodes.Add(_var_statement);
	            _var_statement.source_context = _variable_definitions.var_definitions[i].source_context;
            }
		_variable_definitions.source_context = PT.ToSourceContext(@1.Merge(@3));		
		var_statement_list.source_context = PT.ToSourceContext(@$);		
		$$ = var_statement_list;
	}
	;

Case_variant
	: TK_CASE_V expr COLON StatementSequence {
	  	$$ = new case_variant(GetExpressions($2), GetStatements($4));
		$$.source_context = PT.ToSourceContext(@$);
	}
	;

Case_variant_list	
	: Case_variant {
	      case_variant _case_variant = (case_variant)$1;     
		$$ = new if_node(GetExpressions(_case_variant.conditions), (statement)_case_variant.exec_if_true, null);
		$$.source_context = PT.ToSourceContext(@$);
	}
	|
	Case_variant Case_variant_list {
	      if_node _if_node1 = $2 as if_node;
            case_variant _case_variant = (case_variant)$1;
            $$ = new if_node((expression)_case_variant.conditions.expressions[0], _case_variant.exec_if_true, _if_node1);
            $$.source_context = PT.ToSourceContext(@$);
	}		
	;

Case_statement
	: TK_CASE ENDL Case_variant_list TK_END_ALL {
		if_node _if_node = (if_node)$3;      
		if_node _if_node1;
            _if_node1 = _if_node;
            while (_if_node1.else_body is if_node)
	           _if_node1 = _if_node1.else_body as if_node;
            _if_node1.else_body = null;
		$$ = _if_node;
		$$.source_context = PT.ToSourceContext(@$);
	}
	|
	TK_CASE ENDL Case_variant_list TK_ELSE StatementSequence TK_END_ALL {
	      if_node _if_node = (if_node)$3;
            if_node _if_node1;
		_if_node1 = _if_node;
            while (_if_node1.else_body is if_node)
  	          _if_node1 = _if_node1.else_body as if_node;                                                  
            _if_node1.else_body = GetStatements($5);
		$$ = _if_node;
		$$.source_context = PT.ToSourceContext(@$);
	}	
	;

For_statement
	: TK_BEGIN_CYCLE TK_FOR ID TK_FROM expr TK_TO expr StatementSequence TK_END_CYCLE {
		ident id = new ident($3);
		id.source_context = PT.ToSourceContext(@3.Merge(@3));
		$$ = new for_node(id, (expression)$5, (expression)$7, GetStatements($8), for_cycle_type.to, null, null, false);
		$$.source_context = PT.ToSourceContext(@$);
	}
	| 
	TK_BEGIN_CYCLE expr TK_RAZ StatementSequence TK_END_CYCLE {
        	int32_const _int32_const = new int32_const(1);
                ident loop_var = new ident("&_system_loop_var");
                statement_list _statement_list = GetStatements($4);
                $$ = new for_node(loop_var, _int32_const, (expression)$2, _statement_list, for_cycle_type.to, null, null, true);
		$$.source_context = PT.ToSourceContext(@$);
	}
	;

Proccall_statement
	: TK_ASSERT factparams {
		method_call mc = new method_call($2);
		ident id = new ident("assert");
		id.source_context = PT.ToSourceContext(@1.Merge(@1));		
		mc.dereferencing_value = id;
		$$ = mc;
		$$.source_context = PT.ToSourceContext(@$);
	}
	| TK_READ IDList {
            procedure_call _procedure_call = new procedure_call();
            expression_list _expression_list = new expression_list();
            ident_list _ident_list = GetIdents($2);
            for (int i = 0; -i > -(_ident_list.idents.Count); i++)
            	_expression_list.expressions.Add(_ident_list.idents[i] as expression);
            _ident_list.idents.Clear();                      

		method_call _method_call = new method_call(_expression_list);
        	ident id = new ident("read");
		id.source_context = PT.ToSourceContext(@1.Merge(@1));
		_expression_list.source_context = PT.ToSourceContext(@2.Merge(@2));
		_method_call.dereferencing_value = id;
            _procedure_call.func_name = _method_call;
 		$$ = _procedure_call;
		$$.source_context = PT.ToSourceContext(@$);	
	}
	|
	TK_WRITE factparams {
            procedure_call _procedure_call = new procedure_call();
            expression_list _expression_list = $2;
            method_call _method_call = new method_call(_expression_list);
		ident id = new ident("write");
		id.source_context = PT.ToSourceContext(@1.Merge(@1));
            _method_call.dereferencing_value = id;
            _procedure_call.func_name = _method_call;
            $$ = _procedure_call;
		$$.source_context = PT.ToSourceContext(@$);

		//method_call mc = new method_call($2);
		//ident id = new ident("write");
		//id.source_context = PT.ToSourceContext(@1.Merge(@1));
		//mc.dereferencing_value = id;
		//$$ = mc;
		//$$.source_context = PT.ToSourceContext(@$);	
	}
	|
	ident {
		method_call mc = new method_call(null);
		mc.source_context = PT.ToSourceContext(@1.Merge(@1));
		mc.dereferencing_value = $1;
		$$ = mc;
		$$.source_context = PT.ToSourceContext(@$);
	}
	| ident LPAREN factparams RPAREN {
		method_call mc = new method_call($3);
		mc.dereferencing_value = $1;
		switch ($1.name)
            {
	            case "tg": mc.dereferencing_value = new ident("tan"); break;
                  case "ctg": mc.dereferencing_value = new ident("ctg"); break;
                  case "arctg": mc.dereferencing_value = new ident("arctan"); break;
                  case "arcctg": mc.dereferencing_value = new ident("arcctg"); break;
                  case "lg": mc.dereferencing_value = new ident("log10"); break;
                  case "mod": mc.dereferencing_value = new ident("md"); break;
                  case "div": mc.dereferencing_value = new ident("dv"); break;
                  case "rnd": mc.dereferencing_value = new ident("random"); break;
                  case "int": mc.dereferencing_value = new ident("round"); break;
            }
		
		$$ = mc;
		$$.source_context = PT.ToSourceContext(@$);
	}	
	;

Diap			
	: expr COLON expr {
            $$ = new diapason($1, $3);
		$$.source_context = PT.ToSourceContext(@$);
      }
	;

Diap_list	
	: Diap_list COLUMN Diap 	{
            indexers_types _indexers_types = GetIndexers($1);
            _indexers_types.indexers.Add($3);
		_indexers_types.source_context = PT.ToSourceContext(@$);
            $$ = _indexers_types;
      }
	|  
	Diap	{
            indexers_types _indexers_types = new indexers_types();
            _indexers_types.indexers.Add($1);
		_indexers_types.source_context = PT.ToSourceContext(@$);
            $$ = _indexers_types;
      }
	;

Array_list	
	: ID TK_SQUARE_OPEN Diap_list TK_SQUARE_CLOSE {
            ident_list _ident_list = new ident_list();
		ident id = new ident($1);
            _ident_list.idents.Add(id);
		_ident_list.source_context = PT.ToSourceContext(@1.Merge(@1));
		id.source_context = PT.ToSourceContext(@1.Merge(@1));
            indexers_types _indexers_types = GetIndexers($3);
		_indexers_types.source_context = PT.ToSourceContext(@3.Merge(@3));
            array_type _array_type = new array_type(_indexers_types, null);
		_array_type.source_context = PT.ToSourceContext(@2.Merge(@4));				

            var_def_statement _var_def_statement = new var_def_statement(_ident_list, _array_type, null, definition_attribute.None, false);
            variable_definitions _variable_definitions = new variable_definitions();
            _variable_definitions.var_definitions.Add(_var_def_statement);
		_var_def_statement.source_context = PT.ToSourceContext(@$);
		_variable_definitions.source_context = PT.ToSourceContext(@$);
            $$ = _variable_definitions;
	}
	|  
	ID TK_SQUARE_OPEN Diap_list TK_SQUARE_CLOSE COLUMN Array_list {
		ident_list _ident_list = new ident_list();
		ident id = new ident($1);
            _ident_list.idents.Add(id);
		id.source_context = PT.ToSourceContext(@1.Merge(@1));
		indexers_types _indexers_types = GetIndexers($3);
		_indexers_types.source_context = PT.ToSourceContext(@3.Merge(@3));
            array_type _array_type = new array_type(_indexers_types, null);
		_array_type.source_context = PT.ToSourceContext(@2.Merge(@4));				

            var_def_statement _var_def_statement = new var_def_statement(_ident_list, _array_type, null, definition_attribute.None, false);    
            variable_definitions _variable_definitions = ($6);
            _variable_definitions.var_definitions.Add(_var_def_statement);
		_var_def_statement.source_context = PT.ToSourceContext(@$);
		_variable_definitions.source_context = PT.ToSourceContext(@$);
		$$ = _variable_definitions;
	}
	;

Formal_parameter	
	: type IDList {
		named_type_reference _named_type_reference = $1;
            ident_list _ident_list = GetIdents($2);
            //_ident_list.idents.Reverse();
            typed_parametres _typed_parametres = new typed_parametres(_ident_list, _named_type_reference, parametr_kind.none, null);
            formal_parametres _formal_parametres = new formal_parametres();
            _formal_parametres.params_list.Add(_typed_parametres);

		_typed_parametres.source_context = PT.ToSourceContext(@$);
		_formal_parametres.source_context = PT.ToSourceContext(@$);
            $$ = _formal_parametres;
      }
	|  
	TK_VAR type IDList {
		named_type_reference _named_type_reference = $2;
            ident_list _ident_list = GetIdents($3);
            //_ident_list.idents.Reverse();
            typed_parametres _typed_parametres = new typed_parametres(_ident_list, _named_type_reference, parametr_kind.var_parametr, null);
            formal_parametres _formal_parametres = new formal_parametres();
           _formal_parametres.params_list.Add(_typed_parametres);

		_typed_parametres.source_context = PT.ToSourceContext(@$);
		_formal_parametres.source_context = PT.ToSourceContext(@$);
            $$ = _formal_parametres;
      }
	|  
	type TK_ARRAY Array_list {
		named_type_reference _named_type_reference = $1;
            variable_definitions _variable_definitions = $3;
            formal_parametres _formal_parametres = new formal_parametres();
           //_variable_definitions.var_definitions.Reverse();
            for (int i = 0; -i > -(_variable_definitions.var_definitions.Count); i++)
            {
		    ((_variable_definitions.var_definitions[i]).vars_type as array_type).elemets_types = _named_type_reference;
                _formal_parametres.params_list.Add(new typed_parametres((_variable_definitions.var_definitions[i]).vars, (_variable_definitions.var_definitions[i]).vars_type, parametr_kind.none, null));
            }
            _variable_definitions.var_definitions.Clear();

		_formal_parametres.source_context = PT.ToSourceContext(@$);
		_variable_definitions.source_context = PT.ToSourceContext(@$);
		_named_type_reference.source_context = PT.ToSourceContext(@2.Merge(@3));
	      $$ = _formal_parametres;
      }
	|  
	TK_VAR type TK_ARRAY Array_list {
		named_type_reference _named_type_reference = $2;
            variable_definitions _variable_definitions = $4;
            formal_parametres _formal_parametres = new formal_parametres();
            //_variable_definitions.var_definitions.Reverse();
            for (int i = 0; -i > -(_variable_definitions.var_definitions.Count); i++)
            {
            	((_variable_definitions.var_definitions[i]).vars_type as array_type).elemets_types = _named_type_reference;
                  _formal_parametres.params_list.Add(new typed_parametres((_variable_definitions.var_definitions[i]).vars, (_variable_definitions.var_definitions[i]).vars_type, parametr_kind.var_parametr, null));
            }
            _variable_definitions.var_definitions.Clear();

		_formal_parametres.source_context = PT.ToSourceContext(@$);
		_variable_definitions.source_context = PT.ToSourceContext(@2.Merge(@3));
		_named_type_reference.source_context = PT.ToSourceContext(@$);
            $$ = _formal_parametres;
      }
	;

Formal_type_list	
	: Formal_parameter {
	      formal_parametres _formal_parametres = GetFormals($1);
		_formal_parametres.source_context = PT.ToSourceContext(@$);
            $$ = _formal_parametres;
	}
	|  
	Formal_type_list SEMICOLUMN Formal_parameter {
	      formal_parametres _formal_parametres = GetFormals($1);
            _formal_parametres.params_list.AddRange(GetFormals($3).params_list);

		_formal_parametres.source_context = PT.ToSourceContext(@$);
            $$ = _formal_parametres;
	}
	;


Formal_list 
 	:                      
	|  
	Formal_type_list {
		$$ = $1;	
		$$.source_context = $1.source_context;
	}
	;

Procedure		
	: TK_ALG ident LPAREN Formal_list RPAREN EmptyLines TK_BEGIN StatementSequence TK_END			{       
		method_name _method_name = new method_name(null, $2, null);
            procedure_header _procedure_header = new procedure_header(GetFormals($4), null, _method_name, false, false, null,null);
            statement_list _statement_list = GetStatements($8);
            block _block = new block(null, _statement_list);
            procedure_definition _procedure_definition = new procedure_definition(_procedure_header, _block);

            _method_name.source_context = PT.ToSourceContext(@2.Merge(@2));
		_procedure_header.source_context = PT.ToSourceContext(@1.Merge(@5));
		_procedure_definition.source_context = PT.ToSourceContext(@$);
		_block.source_context = PT.ToSourceContext(@7.Merge(@9));
            //(_units[this.unit_number - 1] as unit_data).sub_progs.defs.Add(_procedure_definition);
            $$ = _procedure_definition; 
      }
	| 
	TK_ALG ident EmptyLines TK_BEGIN StatementSequence TK_END								{
            method_name _method_name = new method_name(null, (ident)$2, null);
		procedure_header _procedure_header = new procedure_header(null, null, _method_name, false, false, null,null);
		statement_list _statement_list = GetStatements($5);
            block _block = new block(null, _statement_list);
            procedure_definition _procedure_definition = new procedure_definition(_procedure_header, _block);

            _method_name.source_context = PT.ToSourceContext(@2.Merge(@2));
		_procedure_header.source_context = PT.ToSourceContext(@1.Merge(@2));
            _procedure_definition.source_context = PT.ToSourceContext(@1.Merge(@6));
		_block.source_context = PT.ToSourceContext(@4.Merge(@6));
		//(_units[this.unit_number - 1] as unit_data).sub_progs.defs.Add(_procedure_definition);
            declarations _declarations = new declarations();
            _declarations.defs.Add(_procedure_definition);
            _declarations.source_context = PT.ToSourceContext(@1.Merge(@6));            
            $$ = _procedure_definition;
      }
	;

Function		
	: TK_ALG type ident LPAREN Formal_list RPAREN EmptyLines TK_BEGIN StatementSequence TK_END	{
      	method_name _method_name = new method_name(null, $3, null);
            named_type_reference _named_type_reference = $2;
            function_header _function_header = new function_header(_named_type_reference);
            _function_header.of_object = false;
            _function_header.name = _method_name;
            _function_header.parametres = GetFormals($5);
            statement_list _statement_list = GetStatements($9);
            block _block = new block(null, _statement_list);
            procedure_definition _procedure_definition = new procedure_definition(_function_header, _block);

		_method_name.source_context = PT.ToSourceContext(@3.Merge(@3));
            _function_header.source_context = PT.ToSourceContext(@1.Merge(@5));
            _procedure_definition.source_context = PT.ToSourceContext(@$);
            _block.source_context = PT.ToSourceContext(@8.Merge(@10));
		//(_units[this.unit_number - 1] as unit_data).sub_progs.defs.Add(_procedure_definition);
            declarations _declarations = new declarations();
            _declarations.defs.Add(_procedure_definition);
            _declarations.source_context = PT.ToSourceContext(@$);
            $$ = _procedure_definition;
      }
	|
	TK_ALG type ident EmptyLines TK_BEGIN StatementSequence TK_END {
		method_name _method_name = new method_name(null, $3, null);
		named_type_reference _named_type_reference = $2;
            function_header _function_header = new function_header(_named_type_reference);
            _function_header.of_object = false;
            _function_header.name = _method_name;
            statement_list _statement_list = GetStatements($6);
		block _block = new block(null, _statement_list);
		procedure_definition _procedure_definition = new procedure_definition(_function_header, _block);
            
            _method_name.source_context = PT.ToSourceContext(@3.Merge(@3));
            _function_header.source_context = PT.ToSourceContext(@1.Merge(@3));
            _procedure_definition.source_context = PT.ToSourceContext(@$);
            _block.source_context = PT.ToSourceContext(@5.Merge(@7));
            //(_units[this.unit_number - 1] as unit_data).sub_progs.defs.Add(_procedure_definition);
            $$ = _procedure_definition;
      }
	;
Sub_declarations
	 :	
	|
	 Sub_declarations Procedure EmptyLines     {
	 if ($1 == null)
		$$ = new declarations();
	 else
		$$ = $1;
       $$.defs.Add((declaration)$2);                 
       $$.source_context = PT.ToSourceContext(@$);
	}
	|
	 Sub_declarations Function EmptyLines  {
	 if ($1 == null)
		$$ = new declarations();
	 else
		$$ = $1;
       $$.defs.Add((declaration)$2);                 
       $$.source_context = PT.ToSourceContext(@$);
	}
     ;

Uses_units		
 	: TK_USES IDList  {
       if (_units[unit_number - 1] != null)
 	      (_units[unit_number - 1] as unit_data).used_units.idents.AddRange($2.idents);
       }
	 ;

Global_decl_list
	: Global_decl_list Separator Var_Decl  {
       statement_list _statement_list = GetStatements($1);
       _statement_list.subnodes.AddRange(GetStatements($3).subnodes);
       _statement_list.source_context = PT.ToSourceContext(@$);
       $$ = _statement_list;
       }
	 |  
	 Var_Decl {  
       //statement_list _statement_list = GetStatements($1);
       //for (int i = 0; i < (_statement_list.subnodes.Count); i++)
       //	(_units[this.unit_number - 1] as unit_data).sub_progs.defs.Add(_statement_list.subnodes[i] as declaration);
       $$ = new statement_list();
  	 $$.source_context = PT.ToSourceContext(@$);
       $$.subnodes.Add($1);
       }
	 ;

Initialization
	: Initialization Separator Assignment {
      (_units[this.unit_number - 1] as unit_data).initialization.subnodes.Add($3);
      }
	|  
	Assignment                              
	{
      (_units[this.unit_number - 1] as unit_data).initialization.subnodes.Add($1);
      }
	;

Global_vars		
	: Global_decl_list Separator Initialization Separator {
      declarations _declarations = new declarations();
      statement_list _statement_list = GetStatements($1);
      for (int i = 0; -i > -(_statement_list.subnodes.Count); i++)
      {
 	     (_units[this.unit_number - 1] as unit_data).sub_progs.defs.Add(_statement_list.subnodes[i] as declaration);
           _declarations.defs.Add(_statement_list.subnodes[i] as declaration);
      }
      _declarations.source_context = PT.ToSourceContext(@$);
	$$ = _declarations;
      }
	|  
	Global_decl_list Separator	{
      declarations _declarations = new declarations();
      statement_list _statement_list = GetStatements($1);
      for (int i = 0; -i > -(_statement_list.subnodes.Count); i++)
      {
            (_units[this.unit_number - 1] as unit_data).sub_progs.defs.Add(_statement_list.subnodes[i] as declaration);
      	_declarations.defs.Add(_statement_list.subnodes[i] as declaration);
      }
      _declarations.source_context = PT.ToSourceContext(@$);
      $$ = _declarations;
      }
	;

Global_part
	:		
	{
	}		
	|  Uses_units Separators { 		
      } 
	|  Uses_units Separators Global_vars   {
      } 
	|  Global_vars {
     	}
	;
%%
