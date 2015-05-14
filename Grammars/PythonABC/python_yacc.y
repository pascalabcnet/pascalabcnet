%output=python_gppg_yacc.cs 

%{
	// for fake dynamic typification
    List<var_def_statement> _var_def_statement_list = new List<var_def_statement>();
%}

%union { 
            public statement _statement;  
            public addressed_value _addressed_value;
            public statement_list _statement_list;            
            public const_node _const_node;
            public op_type_node _op_type_node;
            public token_info _token_info;            
            public ident _ident;
			
			public named_type_reference _named_type_reference;
			public typed_parametres _typed_parametres;
			public formal_parametres _formal_parametres;
			public procedure_definition _procedure_definition;
			public declarations _declarations;
			
			public expression_list _expression_list;
       }

%using System.IO
%using PascalABCCompiler.SyntaxTree
%using GPPGTools

%namespace PascalABCCompiler.PythonABCParser

%start program

%token SEMICOLUMN ENDLINE
%token <_token_info> kINDENT kDEDENT kIF kELSE kWHILE kDEF kFED kRETURN
%token <_token_info> LPAREN RPAREN COLUMN COLON LSQUARE RSQUARE FUNCRETSYMB DOT
%token <_op_type_node> ASSIGN 
%token <_op_type_node> PLUS MINUS MULT DIVIDE 
%token <_op_type_node> AND OR NOT LT GT EQ NE LE GE

%token <_const_node> INTNUM 
%token <_const_node> FLOATNUM 
%token <_const_node> STRINGLITERAL
%token <_const_node> MULTILINESTRING
%token <_const_node> TRUECONST FALSECONST
%token <_ident> ID

%token <_token_info> bINT bFLOAT bSTR bBOOL
%type <_named_type_reference> builtintype
%type <_typed_parametres> formalparam
%type <_formal_parametres> formallist formalparams
%type <_procedure_definition> procdecl funcdecl
%type <_declarations> decls

%type <_expression_list> exprlist factparams

%type <_statement> retstmt

%type <_addressed_value> expr
%type <_statement> stmt
%type <_statement_list> liststmt program

%left LT GT LE GE EQ NE
%left MINUS PLUS OR
%left MULT DIVIDE AND
%left UMINUS NOT

%nonassoc LOWERTHENELSE
%nonassoc kELSE


%%

// ACHTUNG! ATTENTION! Don't uncomment code! It can crash PascalABC.NET :)

program: decls kINDENT liststmt kDEDENT
        {
			//$1.source_context = PT.GetTokenSourceContext(@1);
			$3.source_context = PT.GetTokenSourceContext(@3);
				
			variable_definitions _variable_definitions = new variable_definitions();
			foreach (var_def_statement vds in _var_def_statement_list)
			{
				_variable_definitions.var_definitions.Add(vds);
			}
			_var_def_statement_list.Clear();
			
			declarations _declarations1;
			if($1 == null)
				_declarations1 = new declarations();
			else
				_declarations1 = $1;
			_declarations1.defs.Add(_variable_definitions);
			
			//_declarations1.source_context = PT.GetTokenSourceContext(@1); //

			block _block;	
			if(_declarations1.defs.Count > 0)
				_block = new block(_declarations1, $3);
			else
				_block = new block(null, $3);
				
			//_block.source_context = PT.GetTokenSourceContext(@1.Merge(@2)); // @$
			
			GPPGParser.CompilationUnit = new program_module(null, null, _block, null);
			GPPGParser.CompilationUnit.Language = LanguageId.PascalABCNET;
			
			//GPPGParser.CompilationUnit.source_context = _block.source_context; // @$
        }
	| error
		{
			GPPGParser.CompilationUnit = new compilation_unit();
			
			Errors.ErrorMsg err = new Errors.ErrorMsg(GPPGParser.current_file_name, PT.GetTokenSourceContext(@$), "PROGRAM_ERROR");
			GPPGParser.errors.Add(err);
		}
    ;
	
decls: 
		{
			$$ = null;
		}
	| decls procdecl 
		{
			//if ($1 != null) $1.source_context = PT.GetTokenSourceContext(@1);
			//$2.source_context = PT.GetTokenSourceContext(@2);
			
			declarations _declarations1;
			if ($1 != null)
			{
				_declarations1 = $1;
				_declarations1.defs.Add($2);
			}
			else
			{
				_declarations1 = new declarations();
				_declarations1.defs.Add($2);
			}
			$$ = _declarations1;
			
			//$$.source_context = PT.GetTokenSourceContext(@2); // !!!
		}
	| decls funcdecl
		{
			declarations _declarations1;
			if ($1 != null)
			{
				_declarations1 = $1;
				_declarations1.defs.Add($2);
			}
			else
			{
				_declarations1 = new declarations();
				_declarations1.defs.Add($2);
			}
			$$ = _declarations1;
		}
	;
	
funcdecl: kDEF ID LPAREN formalparams RPAREN FUNCRETSYMB builtintype COLON term liststmt kFED term
		{// 1   2   3         4         5        6           7         8     9      10     11   12
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			PT.AssignSourceContext($4, @4);
			$5.source_context = PT.GetTokenSourceContext(@5);
			$6.source_context = PT.GetTokenSourceContext(@6);
			$7.source_context = PT.GetTokenSourceContext(@7);
			$8.source_context = PT.GetTokenSourceContext(@8);
			$10.source_context = PT.GetTokenSourceContext(@10);
			$11.source_context = PT.GetTokenSourceContext(@11);
			
			method_name _method_name = new method_name(null, $2, null);
			
			function_header _function_header = new function_header($7);
			_function_header.of_object = false;
            _function_header.name = _method_name;
			_function_header.parametres = $4;
			
			_method_name.source_context = $2.source_context;
			_function_header.source_context = PT.GetTokenSourceContext(@1.Merge(@8));
			
			// HACK: aftereffects fake dynamic typification
			
			if($4 != null)
				foreach (typed_parametres tp in $4.params_list)
				{
					bool HaveIdent = false;
					var_def_statement v = new var_def_statement();
					foreach (var_def_statement vds in _var_def_statement_list)
					{
						if(tp.idents.idents[0].name == vds.vars.idents[0].name)
						{
							HaveIdent = true;
							v = vds;
							break;
						}
					}
					if (HaveIdent)
						_var_def_statement_list.Remove(v);
				}	
			
			variable_definitions _variable_definitions = new variable_definitions();
			foreach (var_def_statement vds in _var_def_statement_list)
            {
                _variable_definitions.var_definitions.Add(vds);
            }
			
			// end hack
			
			declarations _declarations1 = new declarations();
			_declarations1.defs.Add(_variable_definitions);
			
			_declarations1.source_context = PT.GetTokenSourceContext(@4); // formalparams
			
			block _block;	
			if(_declarations1.defs.Count > 0)
				_block = new block(_declarations1, $10);
			else
				_block = new block(null, $10);
			_var_def_statement_list.Clear();
			
			_block.source_context = PT.GetTokenSourceContext(@$);
			
			procedure_definition _procedure_definition = new procedure_definition(_function_header, _block);
			$$ = _procedure_definition;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	;
	
procdecl: kDEF ID LPAREN formalparams RPAREN COLON term liststmt kFED term
		{// 1   2    3      4           5      6     7     8      9
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			PT.AssignSourceContext($4, @4);
			$5.source_context = PT.GetTokenSourceContext(@5);
			$6.source_context = PT.GetTokenSourceContext(@6);
			$8.source_context = PT.GetTokenSourceContext(@8);
			$9.source_context = PT.GetTokenSourceContext(@9);
			
			method_name _method_name = new method_name(null, $2, null);
			procedure_header _procedure_header = new procedure_header($4, null, _method_name, false, false, null, null);
			
			_method_name.source_context = $2.source_context;
			_procedure_header.source_context = PT.GetTokenSourceContext(@1.Merge(@6)); // sum of lex locations 1..6 
			
			// HACK: aftereffects fake dynamic typification
			
			if($4 != null)
				foreach (typed_parametres tp in $4.params_list)
				{
					bool HaveIdent = false;
					var_def_statement v = new var_def_statement();
					foreach (var_def_statement vds in _var_def_statement_list)
					{
						if(tp.idents.idents[0].name == vds.vars.idents[0].name)
						{
							HaveIdent = true;
							v = vds;
							break;
						}
					}
					if (HaveIdent)
						_var_def_statement_list.Remove(v);
				}	
			
			variable_definitions _variable_definitions = new variable_definitions();
			foreach (var_def_statement vds in _var_def_statement_list)
            {
                _variable_definitions.var_definitions.Add(vds);
            }
			
			// end hack
			
			declarations _declarations1 = new declarations();
			_declarations1.defs.Add(_variable_definitions);
			
			_declarations1.source_context = PT.GetTokenSourceContext(@4); // formalparams
			
			block _block;	
			if(_declarations1.defs.Count > 0)
				_block = new block(_declarations1, $8);
			else
				_block = new block(null, $8);
			_var_def_statement_list.Clear();
			
			_block.source_context = PT.GetTokenSourceContext(@$);
			
			procedure_definition _procedure_definition = new procedure_definition(_procedure_header, _block);
			$$ = _procedure_definition;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	;

formalparams:
		{
			$$ = null;
		}
	| formallist
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
			
			$$ = $1;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	;

formallist: formalparam
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
			
			formal_parametres _formal_parametres = new formal_parametres();
			_formal_parametres.params_list.Add($1);
			$$ = _formal_parametres;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	| formallist COLUMN formalparam
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
			formal_parametres _formal_parametres = $1;
			_formal_parametres.params_list.Add($3);
			$$ = _formal_parametres;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	| error RPAREN
		{
			GPPGParser.CompilationUnit = new compilation_unit();
			
			Errors.ErrorMsg err = new Errors.ErrorMsg(GPPGParser.current_file_name, PT.GetTokenSourceContext(@$), "FORMALLIST_ERROR");
			GPPGParser.errors.Add(err);	
		}
	;	

formalparam: ID COLON builtintype
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
			ident_list _ident_list = new ident_list();
			_ident_list.idents.Add($1);
			named_type_reference _named_type_reference = $3;
			typed_parametres _typed_parametres = new typed_parametres(_ident_list, _named_type_reference, parametr_kind.none, null);
			$$ = _typed_parametres;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	;

builtintype: bINT
		{	
			$1.source_context = PT.GetTokenSourceContext(@1);
			
			named_type_reference _named_type_reference = new named_type_reference();
			_named_type_reference.names.Add(new ident("integer"));
			$$ = _named_type_reference;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	| bFLOAT
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
		
			named_type_reference _named_type_reference = new named_type_reference();
			_named_type_reference.names.Add(new ident("real"));
			$$ = _named_type_reference;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	| bSTR
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
			
			named_type_reference _named_type_reference = new named_type_reference();
			_named_type_reference.names.Add(new ident("string"));
			$$ = _named_type_reference;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	| bBOOL
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
			
			named_type_reference _named_type_reference = new named_type_reference();
			_named_type_reference.names.Add(new ident("boolean"));
			$$ = _named_type_reference;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}		
	;

liststmt: stmt
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			
			statement_list _sl = new statement_list();
			_sl.subnodes.Add($1);            
            $$ = _sl;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | liststmt term stmt
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$3.source_context = PT.GetTokenSourceContext(@3);
		
            statement_list _sl = $1;
            _sl.subnodes.Add($3);
            $$ = _sl;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
	| liststmt kDEDENT error 
		{
			GPPGParser.CompilationUnit = new compilation_unit();
			
			Errors.ErrorMsg err = new Errors.ErrorMsg(GPPGParser.current_file_name, PT.GetTokenSourceContext(@$), "err in liststmt");
			GPPGParser.errors.Add(err);
		}		
    ;
	
	
stmt:
		{
			$$ = new empty_statement();
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	| ID ASSIGN expr
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
			// HACK: fake dynamic typification
			
			ident_list _ident_list = new ident_list();
			_ident_list.idents.Add($1);
			var_def_statement _var_def_statement = new var_def_statement(_ident_list, null, $3, definition_attribute.None, false);

			bool HaveIdent = false;
			foreach (var_def_statement vds in _var_def_statement_list)
			{
				if(vds.vars.idents[0].name == $1.name)
				{
					HaveIdent = true;
					break;
				}
			}
			if(!HaveIdent)
				_var_def_statement_list.Add(_var_def_statement);
				
			// end hack

			$$ = new assign($1, $3, $2.type);	
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | kWHILE expr COLON ENDLINE stmt
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			$5.source_context = PT.GetTokenSourceContext(@5);
			
		
            $$ = new while_node($2, $5, WhileCycleType.While);
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | kIF expr COLON ENDLINE stmt ENDLINE %prec LOWERTHENELSE
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			$5.source_context = PT.GetTokenSourceContext(@5);
			
            $$ = new if_node($2, $5, null);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | kIF expr COLON ENDLINE stmt ENDLINE kELSE COLON ENDLINE stmt
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			$5.source_context = PT.GetTokenSourceContext(@5);
			$10.source_context = PT.GetTokenSourceContext(@10);
			
            $$ = new if_node($2, $5, $10);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | ID LPAREN factparams RPAREN
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			PT.AssignSourceContext($3, @3);
			$4.source_context = PT.GetTokenSourceContext(@4);

			procedure_call _procedure_call = new procedure_call();
			method_call _method_call;
			
			if($3 != null)
				_method_call = new method_call($3);
			else
				_method_call = new method_call();
				
			if (ParserHelper.builtins.ContainsKey($1.name))
			{
				_method_call.dereferencing_value = new ident(ParserHelper.builtins[$1.name]);
			}
			else
			{
				_method_call.dereferencing_value = $1;
			}
			_method_call.source_context = $1.source_context;
			_procedure_call.func_name = _method_call;
			$$ = _procedure_call;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	| retstmt
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
			
			$$ = $1;
			
			$$.source_context = PT.GetTokenSourceContext(@$);			
		}
    | kINDENT liststmt kDEDENT
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
            $$ = $2;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
	| error EOF
		{
			GPPGParser.CompilationUnit = new compilation_unit();
			
			Errors.ErrorMsg err = new Errors.ErrorMsg(GPPGParser.current_file_name, PT.GetTokenSourceContext(@$), "STATEMENT_EXPECTED");
			GPPGParser.errors.Add(err);
		}
    ;
	
retstmt: kRETURN expr
		{
			$2.source_context = PT.GetTokenSourceContext(@2);
			
			ident id = PT.create_directive_name("result", @1);
			$$ = new assign(id, $2, Operators.Assignment);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	;
	
factparams:
		{
			$$ = null;
		}
	| exprlist
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
			
			$$ = $1;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	;
	
exprlist: expr
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
			
			expression_list _expression_list = new expression_list();
			_expression_list.expressions.Add($1);
			$$ = _expression_list;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	| exprlist COLUMN expr
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
			expression_list _expression_list = $1;
			_expression_list.expressions.Add($3);
			$$ = _expression_list;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	;

expr: INTNUM
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			
            $$ = $1;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | FLOATNUM
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			
            $$ = $1;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
	| TRUECONST
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
			
            $$ = $1;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	| FALSECONST
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
			
            $$ = $1;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
    | ID
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			
            $$ = $1;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
	| STRINGLITERAL
		{
			$1.source_context = PT.GetTokenSourceContext(@1);

            $$ = $1;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	| MULTILINESTRING
		{
			$1.source_context = PT.GetTokenSourceContext(@1);

            $$ = $1;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
    |expr PLUS expr
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
			
            $$ = new bin_expr($1, $3, $2.type);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | expr MINUS expr
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
			
            $$ = new bin_expr($1, $3, $2.type);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | expr MULT expr
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
		
            $$ = new bin_expr($1, $3, $2.type);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | expr DIVIDE expr
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
		
            $$ = new bin_expr($1, $3, $2.type);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | expr AND expr
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
		
            $$ = new bin_expr($1, $3, $2.type);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | expr OR expr
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
		
            $$ = new bin_expr($1, $3, $2.type);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | expr LT expr
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
		
            $$ = new bin_expr($1, $3, $2.type);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | expr GT expr
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
		
			$$ = new bin_expr($1, $3, $2.type);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | expr LE expr
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
			
			$$ = new bin_expr($1, $3, $2.type);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | expr GE expr
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
		
            $$ = new bin_expr($1, $3, $2.type);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | expr EQ expr
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
			
            $$ = new bin_expr($1, $3, $2.type);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | expr NE expr
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			$3.source_context = PT.GetTokenSourceContext(@3);
			
			
            $$ = new bin_expr($1, $3, $2.type);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | NOT expr
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			
			
            $$ = new un_expr($2, $1.type);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | MINUS expr %prec UMINUS
        {
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			
			$$ = new un_expr($2, $1.type);
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
    | LPAREN expr RPAREN
        {
			$2.source_context = PT.GetTokenSourceContext(@2);
			
	        $$ = $2;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
        }
	| ID LPAREN factparams RPAREN
		{
			$1.source_context = PT.GetTokenSourceContext(@1);
			$2.source_context = PT.GetTokenSourceContext(@2);
			PT.AssignSourceContext($3, @3);
			$4.source_context = PT.GetTokenSourceContext(@4);

			method_call _method_call;
			
			if($3 != null)
				_method_call = new method_call($3);
			else
				_method_call = new method_call();
			
			if (ParserHelper.builtins.ContainsKey($1.name))
			{
				_method_call.dereferencing_value = new ident(ParserHelper.builtins[$1.name]);
			}
			else
			{
				_method_call.dereferencing_value = $1;
			}
			_method_call.dereferencing_value.source_context = $1.source_context;
						
			$$ = _method_call;
			
			$$.source_context = PT.GetTokenSourceContext(@$);
		}
	| error term
		{
			GPPGParser.CompilationUnit = new compilation_unit();
			
			Errors.ErrorMsg err = new Errors.ErrorMsg(GPPGParser.current_file_name, PT.GetTokenSourceContext(@$), "EXPRESSION_EXPECTED");
			GPPGParser.errors.Add(err);
		}
    ;

	
term: SEMICOLUMN
    | ENDLINE
    ;

%%

PythonParserTools PT = new PythonParserTools();

class ParserHelper 
{
	public static Dictionary<string, string> builtins;

	static ParserHelper() 
	{
		builtins = new Dictionary<string, string>();
		
		builtins.Add("print", "writeln");
		builtins.Add("input", "readln");
		
	}
}
