%{
   	public syntax_tree_node root;
	public List<Error> errors;
    // public string current_file_name;
    // public int max_errors = 10;
	public SPythonParserTools parserTools;
    public List<compiler_directive> CompilerDirectives;
	public ParserLambdaHelper lambdaHelper = new ParserLambdaHelper();
	public bool is_unit_to_be_parsed = false;

	public SPythonGPPGParser(AbstractScanner<ValueType, LexLocation> scanner, SPythonParserTools parserTools,
	bool isUnitToBeParsed) : base(scanner) 
	{ 
		this.parserTools = parserTools;
		this.is_unit_to_be_parsed = isUnitToBeParsed;
	}
%}

%using PascalABCCompiler.SyntaxTree;
%using PascalABCCompiler.ParserTools;
%using PascalABCCompiler.Errors;
%using System.Linq;
%using System.Collections.Generic;
%using System.IO;
%using SPythonParser;

%output = SPythonParserYacc.cs
%partial
%parsertype SPythonGPPGParser

%namespace SPythonParserYacc

%union {
	public expression ex;
	public ident id;
    public Object ob;
    public op_type_node op;
    public syntax_tree_node stn;
    public token_info ti;
    public type_definition td;
}

%token <ti> FOR IN WHILE IF ELSE ELIF DEF RETURN BREAK CONTINUE IMPORT FROM GLOBAL AS PASS CLASS LAMBDA EXIT NEW IS
%token <ti> INDENT UNINDENT END_OF_FILE END_OF_LINE DECLTYPE
%token <ex> INTNUM REALNUM TRUE FALSE BIGINT
%token <ti> LPAR RPAR LBRACE RBRACE LBRACKET RBRACKET DOT COMMA COLON SEMICOLON ARROW
%token <stn> STRINGNUM
%token <op> ASSIGN PLUSEQUAL MINUSEQUAL STAREQUAL DIVEQUAL
%token <op> PLUS MINUS STAR DIVIDE SLASHSLASH PERCENTAGE
%token <id> ID
%token <op> LESS GREATER LESSEQUAL GREATEREQUAL EQUAL NOTEQUAL
%token <op> AND OR NOT STARSTAR
%token <ti> tkParseModeExpression tkParseModeStatement tkParseModeType

%left OR
%left AND
%left LESS GREATER LESSEQUAL GREATEREQUAL EQUAL NOTEQUAL
%left PLUS MINUS
%left STAR DIVIDE SLASHSLASH PERCENTAGE
%left NOT
%right STARSTAR

%type <id> ident dotted_ident func_name_ident type_decl_identifier
%type <ex> expr proc_func_call const_value variable optional_condition act_param new_expr is_expr variable_as_type
%type <stn> act_param_list optional_act_param_list proc_func_decl return_stmt break_stmt continue_stmt global_stmt pass_stmt
%type <stn> var_stmt assign_stmt if_stmt stmt proc_func_call_stmt while_stmt for_stmt optional_else optional_elif exit_stmt
%type <stn> expr_list
%type <stn> stmt_list block
%type <stn> program param_name form_param_sect form_param_list optional_form_param_list dotted_ident_list
%type <stn> ident_as_ident ident_as_ident_list
%type <td> proc_func_header type_ref simple_type_identifier template_type
%type <stn> import_clause template_type_params template_param_list parts stmt_or_expression expr_mapping_list
%type <ob> optional_semicolon end_of_line
%type <op> assign_type
%type <ex> expr_mapping 
%type <ex> list_constant set_constant dict_constant generator_object generator_object_for_dict

%start program

/*
ident	= identifier
expr	= expression
stmt	= statement
proc	= procedure
func	= function
const	= constant
decl	= declaration
param	= parameter
assign	= assignment
form	= formal
sect	= section
act		= actual
*/

%%
program
	: stmt_list optional_semicolon END_OF_FILE
		{
			// main program
			if (!is_unit_to_be_parsed) {
				var stl = $1 as statement_list;
				stl.left_logical_bracket = new token_info("");
				stl.right_logical_bracket = new token_info("");
				var bl = new block(new declarations(), stl, @$);
				root = $$ = NewProgramModule(null, null, new uses_list(), bl, $2, @$);
				root.source_context = bl.source_context;
			}
			// unit
			else {
				var interface_part = new interface_node(new declarations(), new uses_list(), null, null);
				var initialization_part = new initfinal_part(null, $1 as statement_list, null, null, null, @$);

				root = $$ = new unit_module(
					new unit_name(new ident(Path.GetFileNameWithoutExtension(parserTools.currentFileName)),
					UnitHeaderKeyword.Unit, @$), interface_part, null,
					initialization_part.initialization_sect,
					initialization_part.finalization_sect, null, @$);
			}
		}
	| parts END_OF_FILE
		{ 
			root = $1; 
		}
	;

parts
    : tkParseModeExpression expr
        { $$ = $2; }
    | tkParseModeExpression DECLTYPE type_decl_identifier
        { $$ = $3; }
    | tkParseModeType variable_as_type
		{ $$ = $2; }
	| tkParseModeStatement stmt_or_expression
        { $$ = $2; }
    ;

type_decl_identifier
    : ident
		{ 
			$$ = $1; 
		}
    | ident  template_type_params           
        { 
			$$ = new template_type_name($1.name, $2 as ident_list, @$); 
        }
	;

variable_as_type
	: dotted_ident 
		{ 
			$$ = $1;
		}
	| dotted_ident template_type_params 
		{ 
			$$ = new ident_with_templateparams($1 as addressed_value, $2 as template_param_list, @$);   
		}
	;

stmt_or_expression
    : expr 
        { $$ = new expression_as_statement($1,@$);}
    | assign_stmt
        { $$ = $1; }
    | var_stmt
        { $$ = $1; }
    ;

stmt_list
	: stmt
		{
			$$ = new statement_list($1 as statement, @$);
		}
	| stmt_list end_of_line stmt
		{
			$$ = ($1 as statement_list).Add($3 as statement, @$);
		}
	;

stmt
	: assign_stmt
		{ 
			$$ = $1; 
		}
	| var_stmt
		{ 
			$$ = $1; 
		}
	| if_stmt
		{ 
			$$ = $1; 
		}
	| proc_func_call_stmt
		{ 
			$$ = $1; 
		}
	| while_stmt
		{ 
			$$ = $1; 
		}
	| for_stmt
		{ 
			$$ = $1; 
		}
	| return_stmt
		{ 
			$$ = $1; 
		}
	| break_stmt
		{ 
			$$ = $1; 
		}
	| continue_stmt
		{ 
			$$ = $1; 
		}
	| global_stmt
		{ 
			$$ = $1; 
		}
	| pass_stmt
		{ 
			$$ = $1; 
		}
	| exit_stmt
		{
			$$ = $1;
		}
	| proc_func_decl
		{
			$$ = new declarations_as_statement(new declarations($1 as procedure_definition, @$), @$);
		}
	| import_clause
		{
			$$ = $1; 
		}
	;

import_clause
	: IMPORT ident_as_ident_list
		{
			$$ = new import_statement($2 as as_statement_list, @$);
		}
	| FROM ident IMPORT ident_as_ident_list 
		{
			$$ = new from_import_statement($2 as ident, false, $4 as as_statement_list, @$);
		}
	| FROM ident IMPORT STAR 
		{
			$$ = new from_import_statement($2 as ident, true, null, @$);
		}
	;

pass_stmt
	: PASS
		{
			$$ = new empty_statement();
		}
	;

exit_stmt
	: EXIT LPAR optional_act_param_list RPAR
		{
			parserTools.AddErrorFromResource("UNSUPPORTED_CONSTRUCTION_{0}", @$, "exit");
		}
	;

global_stmt
	: GLOBAL dotted_ident_list
		{
			$$ = new global_statement($2 as ident_list, @$);
		}
	;

ident
	: ID
		{
			$$ = $1;
		}
	;

dotted_ident
	: ident
		{ 
			$$ = $1; 
		}
	| dotted_ident DOT ident
		{ 
			$$ = new ident($1.name + "." + $3.name, @$); 
		}
	;

dotted_ident_list
    : dotted_ident
        {
			$$ = new ident_list($1, @$);
		}
    | dotted_ident_list COMMA dotted_ident
        {
			$$ = ($1 as ident_list).Add($3, @$);
		}
    ;

ident_as_ident
	: ident AS ident
		{ 
			$$ = new as_statement($1, $3, @$); 
		}
	| ident
		{ 
			$$ = new as_statement($1, $1, @$); 
		}
	;

ident_as_ident_list
    : ident_as_ident
        {
			$$ = new as_statement_list($1 as as_statement, @$);
		}
    | ident_as_ident_list COMMA ident_as_ident
        {
			$$ = ($1 as as_statement_list).Add($3 as as_statement, @$);
		}
    ;

expr_mapping
	: expr COLON expr 
		{
			expression_list el = new expression_list(new List<expression> { $1, $3 }, @$);
			$$ = new tuple_node(el, @$);
		}
	;

expr_mapping_list
    : expr_mapping
        {
			$$ = new expression_list($1, @$);
		}
    | expr_mapping_list COMMA expr_mapping
        {
			$$ = ($1 as expression_list).Add($3, @$);
		}
    ;

var_stmt
	: variable COLON type_ref
		{
			var vds = new var_def_statement(new ident_list($1 as ident, @1), $3, null, definition_attribute.None, false, @$);
			$$ = new var_statement(vds, @$);
		}
	| variable COLON type_ref ASSIGN expr
		{
			var vds = new var_def_statement(new ident_list($1 as ident, @1), $3, $5, definition_attribute.None, false, @$);
			$$ = new var_statement(vds, @$);
		}
	;

assign_stmt
	: variable ASSIGN expr
		{
			if (!($1 is addressed_value))
        		parserTools.AddErrorFromResource("LEFT_SIDE_CANNOT_BE_ASSIGNED_TO", @$);
			$$ = new assign($1 as addressed_value, $3, $2.type, @$);
		}
	| variable assign_type expr
		{
			if (!($1 is addressed_value))
        		parserTools.AddErrorFromResource("LEFT_SIDE_CANNOT_BE_ASSIGNED_TO", @$);
			$$ = new assign($1 as addressed_value, $3, $2.type, @$);
		}
	;

assign_type
	: PLUSEQUAL
		{ 
			$$ = $1; 
		}
    | MINUSEQUAL
		{ 
			$$ = $1; 
		}
    | STAREQUAL
		{ 
			$$ = $1; 
		}
    | DIVEQUAL
		{ 
			$$ = $1; 
		}
    ;

expr
	: expr IF expr ELSE expr
		{
			$$ = new question_colon_expression($3, $1, $5, @$);
		}
	| expr PLUS 		expr
		{ 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
	| expr STAR 	expr
		{ 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
	| expr DIVIDE 		expr
		{ 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
	| expr MINUS 		expr
		{ 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
  	| expr LESS 		expr
		{ 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
	| expr GREATER 		expr
		{ 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
	| expr LESSEQUAL 	expr
		{ 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
	| expr GREATEREQUAL expr
		{ 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
	| expr EQUAL 		expr
		{ 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
	| expr NOTEQUAL 	expr
		{ 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
	| expr AND 			expr
		{ 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
	| expr OR 			expr
		{ 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
	| expr SLASHSLASH	expr
		{ 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
	| expr PERCENTAGE	expr
		{ 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
	| expr STARSTAR		expr
		{
			addressed_value method_name = new ident("!pow", @$);
			expression_list el = new expression_list(new List<expression> { $1, $3 }, @$);
			$$ = new method_call(method_name, el, @$);
		}
	| expr IN			expr
		{
			$$ = new bin_expr($1, $3, Operators.In, @$); 
		}
	| expr NOT IN			expr
		{
			// $$ = new bin_expr($1, $4, Operators.NotIn, @$); 
			$$ = new un_expr(new bin_expr($1, $4, Operators.In, @$),Operators.LogicalNOT,@$);
		}
	| MINUS	expr
		{ 
			$$ = new un_expr($2, $1.type, @$); 
		}
	| NOT	expr
		{ 
			$$ = new un_expr($2, $1.type, @$); 
		}
	| variable
		{ 
			$$ = $1; 
		}
	| const_value
		{ 
			$$ = $1; 
		}
	| new_expr
		{
			$$ = $1;
		}
	| is_expr
		{
			$$ = $1;
		}
	| LPAR expr RPAR
		{ 
			$$ = new bracket_expr($2, @$); 
		}
	;

is_expr
	: variable IS type_ref
		{
			$$ = parserTools.NewAsIsExpr($1, op_typecast.is_op, $3, @$);
		}
	;

new_expr
	: NEW type_ref LPAR optional_act_param_list RPAR
		{
			$$ = new new_expr($2, $4 as expression_list, false, null, @$);
		}
	;

const_value
	: INTNUM
		{ 
			$$ = $1; 
		}
	| REALNUM
		{ 
			$$ = $1; 
		}
	| TRUE
		{ 
			$$ = new ident("true", @$); 
		}
	| FALSE
		{ 
			$$ = new ident("false", @$); 
		}
	| STRINGNUM
		{ 
			$$ = $1 as literal; 
		}
	| BIGINT
		{ 
			$$ = $1;
		}
	;

expr_list
	: expr
		{
			$$ = new expression_list($1, @$);
		}
	| expr_list COMMA expr
		{
			$$ = ($1 as expression_list).Add($3, @$);
		}
	;

if_stmt
	: IF expr COLON block optional_elif
		{
			$$ = new if_node($2, $4 as statement, $5 as statement, @$);
		}
	;

optional_elif
	: ELIF expr COLON block optional_elif
		{
			$$ = new if_node($2, $4 as statement, $5 as statement, @$);
		}
	| optional_else
		{ 
			$$ = $1; 
		}
	;

optional_else
	: ELSE COLON block
		{ 
			$$ = $3; 
		}
	|
		{ 
			$$ = null; 
		}
	;

while_stmt
	: WHILE expr COLON block
		{
			$$ = new while_node($2, $4 as statement, WhileCycleType.While, @$);
		}
	;

for_stmt
	: FOR ident IN expr COLON block
		{
			$$ = new foreach_stmt($2, new no_type_foreach(), $4, $6 as statement, null, @$);
		}
	;

func_name_ident
	: ident
		{
			$$ = $1;
		}
	;

return_stmt
	: RETURN expr
		{
			$$ = new return_statement($2, @$);
		}
	| RETURN
		{
			$$ = new return_statement(null, @$);
		}
	;

break_stmt
	: BREAK
		{
			$$ = new procedure_call(new ident("break", @$), true, @$);
		}
	;

continue_stmt
	: CONTINUE
		{
			$$ = new procedure_call(new ident("continue", @$), true, @$);
		}
	;

proc_func_call_stmt
	:  proc_func_call
        {
			$$ = new procedure_call($1 as addressed_value, $1 is ident, @$);
		}
	;

variable
	: ident
		{ 
			$$ = $1; 
		}
	| proc_func_call
		{ 
			$$ = $1; 
		}
	| variable DOT ident
		{ 
			$$ = new dot_node($1 as addressed_value, $3 as addressed_value, @$); 
		}
	| const_value DOT ident
		{ 
			$$ = new dot_node($1 as addressed_value, $3 as addressed_value, @$); 
		}
	| list_constant
		{
			$$ = $1;
		}
	| set_constant
		{
			$$ = $1;
		}
	| dict_constant
		{
			$$ = $1;
		}
	// index property
	| variable LBRACKET expr RBRACKET
		{
			var el = new expression_list($3 as expression, @$);
			$$ = new indexer($1 as addressed_value, el, @$);
		}
	// list generator
	| LBRACKET generator_object RBRACKET
		{
			dot_node dn = new dot_node($2 as addressed_value, (new ident("ToList")) as addressed_value, $2.source_context);
			$$ = new method_call(dn as addressed_value, null, $2.source_context);
		}
	// set generator
	| LBRACE generator_object RBRACE
		{
			dot_node dn = new dot_node($2 as addressed_value, (new ident("ToSet")) as addressed_value, $2.source_context);
			$$ = new method_call(dn as addressed_value, null, $2.source_context);
		}
	// dict generator
	| LBRACE generator_object_for_dict RBRACE
		{
			dot_node dn = new dot_node($2 as addressed_value, (new ident("ToDictionary")) as addressed_value, $2.source_context);
			$$ = new method_call(dn as addressed_value, null, $2.source_context);
		}
	;

generator_object
	: expr FOR ident IN expr optional_condition
		{
			$$ = new generator_object($1, $3, $5, $6, @$);
		}
	;

generator_object_for_dict
	: expr_mapping FOR ident IN expr optional_condition
		{
			$$ = new generator_object($1, $3, $5, $6, @$);
		}
	;

dict_constant
	: LBRACE expr_mapping_list RBRACE
		{
			$$ = new method_call(new ident("Dict", @$), $2 as expression_list, @$);
		}
	| LBRACE RBRACE
		{
			$$ = new method_call(new ident("!empty_dict", @$), null, @$);
		}
	;

set_constant
	: LBRACE expr_list RBRACE
		{
			$$ = new pascal_set_constant($2 as expression_list, @$);
		}
	;

list_constant
	: LBRACKET expr_list RBRACKET
		{
			var acn = new array_const_new($2 as expression_list, '|', @$);
			var dn = new dot_node(acn as addressed_value, (new ident("ToList", @$)) as addressed_value, @$);
			$$ = new method_call(dn as addressed_value, null, @$);
		}
	| LBRACKET RBRACKET
		{
			$$ = new method_call(new ident("!empty_list", @$), null, @$);
		}
	;

optional_condition
	:
		{ 
			$$ = null; 
		}
	| IF expr
		{ 
			$$ = $2; 
		}
	;

block
	: INDENT stmt_list end_of_line UNINDENT
		{
			$$ = $2 as statement_list;
			($$ as statement_list).left_logical_bracket = $1;
			($$ as statement_list).right_logical_bracket = $4;
			$$.source_context = LexLocation.MergeAll(@2, @3);
		}
	;

proc_func_decl
	: proc_func_header block
		{
			$$ = new procedure_definition($1 as procedure_header, new block(null, $2 as statement_list, @2), @$);
		}
	;

proc_func_header
	: DEF func_name_ident LPAR optional_form_param_list RPAR COLON
		{
			$$ = new procedure_header($4 as formal_parameters, new procedure_attributes_list(new List<procedure_attribute>()), new method_name(null,null, $2, null, @2), null, @$);
		}
	| DEF func_name_ident LPAR optional_form_param_list RPAR ARROW type_ref COLON
		{
			$$ = new function_header($4 as formal_parameters, new procedure_attributes_list(new List<procedure_attribute>()), new method_name(null,null, $2, null, @2), null, $7 as type_definition, @$);
		}
	;

proc_func_call
	: variable LPAR optional_act_param_list RPAR
		{
			$$ = new method_call($1 as addressed_value, $3 as expression_list, @$);
		}
	;

simple_type_identifier
	: ident
		{
			$$ = new named_type_reference($1, @$);
		}
	| simple_type_identifier DOT ident
        { 
			$$ = ($1 as named_type_reference).Add($3, @$);
		}
	;

type_ref
	: simple_type_identifier
		{
			$$ = $1 as named_type_reference;
		}
	| template_type
		{ 
			$$ = $1; 
		}
	;

template_type
    : simple_type_identifier template_type_params    
        { 
			$$ = new template_type_reference($1 as named_type_reference, $2 as template_param_list, @$); 
		}
    ;

template_type_params
    : LBRACKET template_param_list RBRACKET            
        {
			$$ = $2;
			$$.source_context = @$;
		}
    ;

template_param_list
    : type_ref                              
        { 
			$$ = new template_param_list($1, @$);
		}
    | template_param_list COMMA type_ref  
        { 
			$$ = ($1 as template_param_list).Add($3, @$);
		}
    ;

param_name
	: ident
		{
			$$ = new ident_list($1, @$);
		}
    ;

form_param_sect
	: param_name COLON type_ref
		{
			$$ = new typed_parameters($1 as ident_list, $3, parametr_kind.none, null, @$);
		}
	;

form_param_list
    : form_param_sect
        {
			$$ = new formal_parameters($1 as typed_parameters, @$);
        }
    | form_param_list COMMA form_param_sect
        {
			$$ = ($1 as formal_parameters).Add($3 as typed_parameters, @$);
        }
    ;

optional_form_param_list
    : form_param_list
        {
			$$ = $1;
		}
	|
        {
			$$ = null;
		}
    ;

act_param
	: expr
		{ 
			$$ = $1; 
		}
	| ident ASSIGN expr
		{ 
			$$ = new name_assign_expr($1, $3, @$); 
		}
	;

act_param_list
	: act_param
		{
			$$ = new expression_list($1, @$);
		}
	| act_param_list COMMA act_param
		{
			$$ = ($1 as expression_list).Add($3, @$);
		}
	;

optional_act_param_list
	: act_param_list
		{
			$$ = $1;
		}
	| generator_object
		{
			$$ = new expression_list($1, @$);
		}
	|
        {
			$$ = null;
		}
    ;

end_of_line
	: END_OF_LINE
		{
			$$ = $1;
		}
	| SEMICOLON
		{
			$$ = $1;
		}
	;

optional_semicolon
	: SEMICOLON
		{ 
			$$ = $1; 
		}
	|
		{ 
			$$ = null; 
		}
	;

%%

        public program_module NewProgramModule(program_name progName, Object optHeadCompDirs, uses_list mainUsesClose, syntax_tree_node progBlock, Object optPoint, LexLocation loc)
        {
            var progModule = new program_module(progName, mainUsesClose, progBlock as block, null, loc);
            progModule.Language = "SPython";
            if (optPoint == null && progBlock != null)
            {
                var fp = progBlock.source_context.end_position;
                var err_stn = progBlock;
			    if ((progBlock is block) && (progBlock as block).program_code != null && (progBlock as block).program_code.subnodes != null && (progBlock as block).program_code.subnodes.Count > 0)
                    err_stn = (progBlock as block).program_code.subnodes[(progBlock as block).program_code.subnodes.Count - 1];
                //parserTools.errors.Add(new SPythonUnexpectedToken(parserTools.currentFileName, StringResources.Get("TKPOINT"), new SourceContext(fp.line_num, fp.column_num + 1, fp.line_num, fp.column_num + 1, 0, 0), err_stn));
            }
            return progModule;
        }
