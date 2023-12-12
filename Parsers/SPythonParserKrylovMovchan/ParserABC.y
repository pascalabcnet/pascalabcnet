%{
   	public syntax_tree_node root;
	public List<Error> errors;
    public string current_file_name;
    public int max_errors = 10;
	public VeryBasicParserTools parsertools;
    public List<compiler_directive> CompilerDirectives;
   	public VeryBasicGPPGParser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
	private SortedSet<string> symbolTable = new SortedSet<string>();
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

%token <ti> FOR, IN, WHILE, IF, ELSE, ELIF, LOCAL, DEF
%token <ex> INTNUM, REALNUM
%token <ti> LPAR, RPAR, LBRACE, RBRACE, LBRACKET, RBRACKET, DOT, COMMA, COLON, SEMICOLON, INDENT, UNINDENT
%token <op> ASSIGN
%token <op> PLUS, MINUS, MULTIPLY, DIVIDE
%token <id> ID
%token <op> LOWER, GREATER, LOWEREQUAL, GREATEREQUAL, EQUAL, NOTEQUAL

%left LOWER, GREATER, LOWEREQUAL, GREATEREQUAL, EQUAL, NOTEQUAL
%left PLUS MINUS
%left MULTIPLY DIVIDE

%type <id> ident
%type <ex> expr var_reference variable proc_func_call
%type <stn> expr_lst optional_expr_lst proc_func_decl
%type <stn> assign if_stmt stmt proccall while_stmt for_stmt optional_else optional_elif
%type <stn> compound_stmt compound_stmt_lst
%type <stn> stmt_lst block
%type <stn> progr declaration
%type <td> proc_func_header

%start progr

%%
progr   : compound_stmt_lst
		{
			var stl = $1 as statement_list;
			// добавляем ноды инициализации глобальных переменных
			// foreach (string elem in symbolTable) {
			// 	var vds = new var_def_statement(new ident_list(new ident(elem)), null, new int32_const(0), definition_attribute.None, false, @$);
			// 	stl.AddFirst((new var_statement(vds, @$)) as statement);
			// }
			root = $$ = NewProgramModule(null, null, null, new block(decl, stl, @$), new token_info(""), @$);
			$$.source_context = @$;
		}
		;

compound_stmt	: stmt	{ $$ = $1; }
				| declaration { $$ = null; }
				;

declaration	: proc_func_decl 
			{
				$$ = null; 
				decl.Add($1 as procedure_definition, @$);
			}
			;

compound_stmt_lst	: compound_stmt
					{ $$ = new statement_list($1 as statement, @1); }
					| compound_stmt_lst SEMICOLON compound_stmt
					{
						if ($3 is statement st) 
							$$ = ($1 as statement_list).Add(st, @$);
						else $$ = ($1 as statement_list);
					}
					;

stmt_lst	: stmt
			{ $$ = new statement_list($1 as statement, @1); }
			| stmt_lst SEMICOLON stmt
			{ $$ = ($1 as statement_list).Add($3 as statement, @$); }
			;

stmt	: assign		{ $$ = $1; }
		| block			{ $$ = $1; }
		| if_stmt		{ $$ = $1; }
		| proccall		{ $$ = $1; }
		| while_stmt	{ $$ = $1; }
		| for_stmt		{ $$ = $1; }
		;

ident 	: ID	{ $$ = $1; }
		;

assign 	: ident ASSIGN expr
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
		| LOCAL ident ASSIGN expr
		{
			symbolTable.Add($2.name);
			var vds = new var_def_statement(new ident_list($2, @2), null, $4, definition_attribute.None, false, @$);
			$$ = new var_statement(vds, @$);
			// symbolTable.Add($1.name);
			// $$ = new assign($1 as addressed_value, $3, $2.type, @$);
		}
		;

expr 	: expr PLUS 	expr	{ $$ = new bin_expr($1, $3, $2.type, @$); }
		| expr MULTIPLY expr	{ $$ = new bin_expr($1, $3, $2.type, @$); }
		| expr DIVIDE 	expr	{ $$ = new bin_expr($1, $3, $2.type, @$); }
		| expr MINUS 	expr	{ $$ = new bin_expr($1, $3, $2.type, @$); }
  		| expr LOWER 	expr	{ $$ = new bin_expr($1, $3, $2.type, @$); }
		| expr GREATER 	expr	{ $$ = new bin_expr($1, $3, $2.type, @$); }
		| variable				{ $$ = $1; }
		| INTNUM				{ $$ = $1; }
		| REALNUM				{ $$ = $1; }
		| LPAR expr RPAR		{ $$ = $2; }
		;

optional_expr_lst	: expr_lst	{ $$ = $1; }
					| 			{ $$ = null; }
					;

expr_lst	: expr
			{ $$ = new expression_list($1, @$); }
			| expr_lst COMMA expr
			{ $$ = ($1 as expression_list).Add($3, @$); }
			;

if_stmt	: IF expr COLON block optional_elif
		{ $$ = new if_node($2, $4 as statement, $5 as statement, @$); }
		;

optional_elif	: ELIF expr COLON block optional_elif
				{ $$ = new if_node($2, $4 as statement, $5 as statement, @$); }
				| optional_else
				{ $$ = $1; }
				;

optional_else	: ELSE COLON block	{ $$ = $3; }
				|					{ $$ = null; }
				;

while_stmt	: WHILE expr COLON block
			{ $$ = new while_node($2, $4 as statement, WhileCycleType.While, @$); }
			;

for_stmt	: FOR ident IN expr COLON block
			{ $$ = new foreach_stmt($2, new no_type_foreach(), $4, (statement)$6, null, @$); }
			;

proccall	:  var_reference
        	{ $$ = new procedure_call($1 as addressed_value, $1 is ident, @$); }
			;

var_reference	: variable { $$ = $1; }
				;

variable	: ident				{ $$ = $1; }
			| proc_func_call	{ $$ = $1; }
			;

block	: INDENT stmt_lst SEMICOLON UNINDENT
		{ 
			$$ = $2 as statement_list; 
			($$ as statement_list).left_logical_bracket = $1;
			($$ as statement_list).right_logical_bracket = $4;
			$$.source_context = @$;
		}
		;

proc_func_decl	: proc_func_header block 
				{ 
					procedure_definition pd = new procedure_definition($1 as procedure_header, $2 as proc_block, @$);
					pd.AssignAttrList(null);
					$$ = pd;
				} 
				;

proc_func_header: DEF ident LPAR RPAR COLON
				{ $$ = new procedure_header(null, null, new method_name(null,null, $2, null, @$), null, @$); }
//$$ = new procedure_header($3 as formal_parameters, $4 as procedure_attributes_list, $2 as method_name, $5 as where_definition_list, @$); 
				;

proc_func_call	: ident LPAR optional_expr_lst RPAR
				{ $$ = new method_call($1 as addressed_value, $3 as expression_list, @$); }
				;

%%

        public program_module NewProgramModule(program_name progName, Object optHeadCompDirs, uses_list mainUsesClose, syntax_tree_node progBlock, Object optPoint, LexLocation loc)
        {
            var progModule = new program_module(progName, mainUsesClose, progBlock as block, null, loc);
            progModule.Language = LanguageId.PascalABCNET;
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
