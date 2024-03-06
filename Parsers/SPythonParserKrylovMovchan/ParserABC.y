%{
   	public syntax_tree_node root;
	public List<Error> errors;
    public string current_file_name;
    public int max_errors = 10;
	public VeryBasicParserTools parsertools;
    public List<compiler_directive> CompilerDirectives;
   	public VeryBasicGPPGParser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }

	private SymbolTable symbolTable = new SymbolTable();
	private declarations decl_forward = new declarations();
	private declarations decl = new declarations();
%}

%using PascalABCCompiler.SyntaxTree;
%using PascalABCCompiler.ParserTools;
%using PascalABCCompiler.Errors;
%using System.Linq;
%using System.Collections.Generic;
%using VeryBasicParser;

%output = VeryBasicParserYacc.cs
%partial
%parsertype VeryBasicGPPGParser

%namespace VeryBasicParserYacc

%union {
	public expression ex;
	public ident id;
    public Object ob;
    public op_type_node op;
    public syntax_tree_node stn;
    public token_info ti;
    public type_definition td;
}

%token <ti> FOR IN WHILE IF ELSE ELIF DEF RETURN BREAK CONTINUE
%token <ex> INTNUM REALNUM
%token <ti> LPAR RPAR LBRACE RBRACE LBRACKET RBRACKET DOT COMMA COLON SEMICOLON INDENT UNINDENT ARROW
%token <stn> STRINGNUM
%token <op> ASSIGN
%token <op> PLUS MINUS MULTIPLY DIVIDE SLASHSLASH PERCENTAGE
%token <id> ID INT
%token <op> LESS GREATER LESSEQUAL GREATEREQUAL EQUAL NOTEQUAL
%token <op> AND OR

%left OR
%left AND
%left LESS GREATER LESSEQUAL GREATEREQUAL EQUAL NOTEQUAL
%left PLUS MINUS
%left MULTIPLY DIVIDE SLASHSLASH PERCENTAGE

%type <id> identifier
%type <ex> expr var_reference variable proc_func_call range_expr constant_value
%type <stn> expr_lst optional_expr_lst proc_func_decl return_stmt break_stmt continue_stmt
%type <stn> assign if_stmt stmt proccall while_stmt for_stmt optional_else optional_elif
%type <stn> decl_or_stmt decl_or_stmt_list
%type <stn> stmt_lst compound_stmt proc_func_body
%type <stn> progr decl param_name simple_fp_sect fp_sect fp_sect_list fp_list
%type <td> proc_func_header fp_type simple_type_identifier

%start progr

%%
progr   
	: decl_or_stmt_list
		{
			var stl = $1 as statement_list;
			decl.AddFirst(decl_forward.defs);
			// добавляем ноды инициализации глобальных переменных
			// foreach (string elem in symbolTable) {
			// 	var vds = new var_def_statement(new ident_list(new ident(elem)), null, new int32_const(0), definition_attribute.None, false, @$);
			// 	stl.AddFirst((new var_statement(vds, @$)) as statement);
			// }
			root = $$ = NewProgramModule(null, null, null, new block(decl, stl, @$), new token_info(""), @$);
			$$.source_context = @$;
		}
	;

decl_or_stmt	
	: stmt	
		{ $$ = $1; }
	| decl 
		{ $$ = null; }
	;

decl	
	: proc_func_decl 
		{
			$$ = null; 
			decl.Add($1 as procedure_definition, @$);
		}
	;

decl_or_stmt_list	
	: decl_or_stmt
		{
			if ($1 is statement st)
				$$ = new statement_list($1 as statement, @1);
			else
				$$ =  new statement_list(); 
		}
	| decl_or_stmt_list SEMICOLON decl_or_stmt
		{
			if ($3 is statement st) 
				$$ = ($1 as statement_list).Add(st, @$);
			else 
				$$ = ($1 as statement_list);
		}
	;

stmt_lst	
	: stmt
		{ 
			$$ = new statement_list($1 as statement, @1); 
		}
	| stmt_lst SEMICOLON stmt
		{ 
			$$ = ($1 as statement_list).Add($3 as statement, @$); 
		}
	;

stmt	
	: assign		
		{ $$ = $1; }
	| compound_stmt	
		{ $$ = $1; }
	| if_stmt		
		{ $$ = $1; }
	| proccall		
		{ $$ = $1; }
	| while_stmt	
		{ $$ = $1; }
	| for_stmt		
		{ $$ = $1; }
	| return_stmt
		{ $$ = $1; }
	| break_stmt
		{ $$ = $1; }
	| continue_stmt
		{ $$ = $1; }
	;

identifier	
	: ID	
		{
			if ($1.name == "result")
				$1.name = "%result";
			$$ = $1; 
		}
	;

assign
	: identifier ASSIGN expr
		{
			if (!symbolTable.Contains($1.name)) {
				symbolTable.Add($1.name);
				var vds = new var_def_statement(new ident_list($1, @1), null, $3, definition_attribute.None, false, @$);
				$$ = new var_statement(vds, @$);
			}
			else {
				$$ = new assign($1 as addressed_value, $3, $2.type, @$);
			}
			// symbolTable.Add($1.name);
			// $$ = new assign($1 as addressed_value, $3, $2.type, @$);
		}
	;

expr 	
	: expr PLUS 		expr	
		{ $$ = new bin_expr($1, $3, $2.type, @$); }
	| expr MULTIPLY 	expr	
		{ $$ = new bin_expr($1, $3, $2.type, @$); }
	| expr DIVIDE 		expr	
		{ $$ = new bin_expr($1, $3, $2.type, @$); }
	| expr MINUS 		expr	
		{ $$ = new bin_expr($1, $3, $2.type, @$); }
  	| expr LESS 		expr	
		{ $$ = new bin_expr($1, $3, $2.type, @$); }
	| expr GREATER 		expr	
		{ $$ = new bin_expr($1, $3, $2.type, @$); }
	| expr LESSEQUAL 	expr	
		{ $$ = new bin_expr($1, $3, $2.type, @$); }
	| expr GREATEREQUAL expr	
		{ $$ = new bin_expr($1, $3, $2.type, @$); }
	| expr EQUAL 		expr	
		{ $$ = new bin_expr($1, $3, $2.type, @$); }
	| expr NOTEQUAL 	expr	
		{ $$ = new bin_expr($1, $3, $2.type, @$); }
	| expr AND 			expr	
		{ $$ = new bin_expr($1, $3, $2.type, @$); }
	| expr OR 			expr	
		{ $$ = new bin_expr($1, $3, $2.type, @$); }
	| expr SLASHSLASH	expr	
		{ $$ = new bin_expr($1, $3, $2.type, @$); }
	| expr PERCENTAGE	expr	
		{ $$ = new bin_expr($1, $3, $2.type, @$); }
	| variable				
		{ $$ = $1; }
	| constant_value
		{ $$ = $1; }
	| LPAR expr RPAR		
		{ $$ = $2; }
	;

constant_value
	: INTNUM
		{ $$ = $1; }
	| REALNUM				
		{ $$ = $1; }
	| STRINGNUM				
		{ $$ = $1 as literal; }
	;

optional_expr_lst	
	: expr_lst	
		{ $$ = $1; }
	|
		{ $$ = null; }
	;

expr_lst	
	: expr
		{ 
			$$ = new expression_list($1, @$); 
		}
	| expr_lst COMMA expr
		{ 
			$$ = ($1 as expression_list).Add($3, @$); 
		}
	;

if_stmt	
	: IF expr COLON compound_stmt optional_elif
		{ 
			$$ = new if_node($2, $4 as statement, $5 as statement, @$); 
		}
	;

optional_elif	
	: ELIF expr COLON compound_stmt optional_elif
		{ 
			$$ = new if_node($2, $4 as statement, $5 as statement, @$); 
		}
	| optional_else
		{ $$ = $1; }
	;

optional_else	
	: ELSE COLON compound_stmt	
		{ $$ = $3; }
	|
		{ $$ = null; }
	;

while_stmt	
	: WHILE expr COLON compound_stmt
		{ 
			$$ = new while_node($2, $4 as statement, WhileCycleType.While, @$); 
		}
	;

for_stmt	
	: FOR identifier IN expr COLON compound_stmt
		{ 
			$$ = new foreach_stmt($2, new no_type_foreach(), $4, (statement)$6, null, @$); 
		}
	;

// return `expr` ~ result := `expr`; exit;
return_stmt
	: RETURN expr
		{
			statement res_assign = new assign(new ident("result"), $2, Operators.Assignment, @$);
			statement exit_call = new procedure_call(new ident("exit"), true, @$);
			$$ = new statement_list(res_assign, @$);
			($$  as statement_list).Add(exit_call, @$);
		}
	| RETURN
		{
			$$ = new procedure_call(new ident("exit"), true, @$);
		}
	;

break_stmt
	: BREAK
		{
			$$ = new procedure_call(new ident("break"), true, @$);
		}
	;

continue_stmt
	: CONTINUE
		{
			$$ = new procedure_call(new ident("continue"), true, @$);
		}
	;

proccall	
	:  var_reference
        {
			$$ = new procedure_call($1 as addressed_value, $1 is ident, @$);
		}
	;

var_reference	
	: variable 
		{ $$ = $1; }
	;

variable	
	: identifier		
		{ $$ = $1; }
	| proc_func_call
		{ $$ = $1; }
	| variable DOT identifier
		{ $$ = new dot_node($1 as addressed_value, $3 as addressed_value, @$); }
	| constant_value DOT identifier
		{ $$ = new dot_node($1 as addressed_value, $3 as addressed_value, @$); }
	;

compound_stmt	
	: NestedSymbolTable INDENT stmt_lst SEMICOLON UNINDENT
		{ 
			symbolTable = symbolTable.OuterScope;

			$$ = $3 as statement_list; 
			($$ as statement_list).left_logical_bracket = $2;
			($$ as statement_list).right_logical_bracket = $5;
			$$.source_context = @$;
		}
	;

NestedSymbolTable	
	:
		{ 
			symbolTable = new SymbolTable(symbolTable); 
		}
	;

// аналог compound_stmt для тела функции не создающий пространства имён (оно создаётся вместе с заголовком)
proc_func_body
	: INDENT stmt_lst SEMICOLON UNINDENT
		{	
			$$ = $2 as statement_list; 
			($$ as statement_list).left_logical_bracket = $1;
			($$ as statement_list).right_logical_bracket = $4;
			$$.source_context = @$;
		}
	;

proc_func_decl	
	: NestedSymbolTable proc_func_header proc_func_body 
		{ 
			symbolTable = symbolTable.OuterScope;

			//var pd1 = new procedure_definition($1 as procedure_header, new block(null, $2 as statement_list, @2), @$);
			//pd1.AssignAttrList(null);
			//$$ = pd1;
			$$ = new procedure_definition($2 as procedure_header, new block(null, $3 as statement_list, @3), @$);

			var pd = new procedure_definition($2 as procedure_header, null, @2);
            pd.proc_header.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_forward));
			decl_forward.Add(pd, @2);
		} 
	;

proc_func_header
	: DEF identifier fp_list COLON 
		{
			// надо добавить все имена (из fp_list) переменных в symbolTable
			// symbolTable = new SymbolTable(symbolTable); 
			$$ = new procedure_header($3 as formal_parameters, new procedure_attributes_list(new List<procedure_attribute>(), @$), new method_name(null,null, $2, null, @$), null, @$); 
		}
	| DEF identifier fp_list ARROW fp_type COLON
		{
			// надо добавить все имена (из fp_list) переменных в symbolTable
			// symbolTable = new SymbolTable(symbolTable); 
			$$ = new function_header($3 as formal_parameters, new procedure_attributes_list(new List<procedure_attribute>(), @$), new method_name(null,null, $2, null, @$), null, $5 as type_definition, @$);
		}
	;

proc_func_call
	: variable LPAR optional_expr_lst RPAR
		{ 
			$$ = new method_call($1 as addressed_value, $3 as expression_list, @$); 
		}
	;

simple_type_identifier
	: identifier
		{
			switch ($1.name) {
				case "int":
					$$ = new named_type_reference("integer", @$);
					break;
				case "float":
					$$ = new named_type_reference("real", @$);
					break;
				case "str":
					$$ = new named_type_reference("string", @$);
					break;
				
				case "integer":
				case "real":
				case "string":
					$$ = new named_type_reference("error", @$);
					break;
				
				default:
					$$ = new named_type_reference($1, @$);
					break;
			}
		}
	;

range_expr	
	: simple_type_identifier
		{
			$$ = parsertools.ConvertNamedTypeReferenceToDotNodeOrIdent($1 as named_type_reference);
		}
	;

fp_type	
	: range_expr 
		{ 
			$$ = parsertools.ConvertDotNodeOrIdentToNamedTypeReference($1); 
		}
	;

param_name	
	: identifier
		{
			symbolTable.Add($1.name);
			$$ = new ident_list($1, @$);
		}
    ;

simple_fp_sect	
	: param_name COLON fp_type
		{
			$$ = new typed_parameters($1 as ident_list, $3, parametr_kind.none, null, @$); 
		}
	;

fp_sect	
	: simple_fp_sect
		{ 
			$$ = $1; 
		}
	;

fp_sect_list
    : fp_sect                                          
        { 
			$$ = new formal_parameters($1 as typed_parameters, @$);
        }
    | fp_sect_list COMMA fp_sect               
        { 
			$$ = ($1 as formal_parameters).Add($3 as typed_parameters, @$);   
        } 
    ;

fp_list
    : LPAR RPAR        
        {
			$$ = null; 
		}
    | LPAR fp_sect_list RPAR         
        { 
			$$ = $2;
			if ($$ != null)
				$$.source_context = @$;
		}
    ;

%%

        public program_module NewProgramModule(program_name progName, Object optHeadCompDirs, uses_list mainUsesClose, syntax_tree_node progBlock, Object optPoint, LexLocation loc)
        {
            var progModule = new program_module(progName, mainUsesClose, progBlock as block, null, loc);
            progModule.Language = LanguageId.SPython;
            if (optPoint == null && progBlock != null)
            {
                var fp = progBlock.source_context.end_position;
                var err_stn = progBlock;
			    if ((progBlock is block) && (progBlock as block).program_code != null && (progBlock as block).program_code.subnodes != null && (progBlock as block).program_code.subnodes.Count > 0)
                    err_stn = (progBlock as block).program_code.subnodes[(progBlock as block).program_code.subnodes.Count - 1];
                parsertools.errors.Add(new SPythonUnexpectedToken(parsertools.CurrentFileName, StringResources.Get("TKPOINT"), new SourceContext(fp.line_num, fp.column_num + 1, fp.line_num, fp.column_num + 1, 0, 0), err_stn));
            }
            return progModule;
        }
