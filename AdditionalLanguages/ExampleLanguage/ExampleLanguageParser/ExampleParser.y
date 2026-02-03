%using PascalABCCompiler.SyntaxTree;
%using PascalABCCompiler.ParserTools;

%namespace Languages.Example.Frontend.Core

%{
   	internal syntax_tree_node root;
	internal ExampleParserTools parserTools;

	internal ExampleGPPGParser(AbstractScanner<Union, LexLocation> scanner, ExampleParserTools parserTools) : base(scanner)
	{ 
		this.parserTools = parserTools;
	}
%}

%output=ExampleParserYacc.cs
%partial
%parsertype ExampleGPPGParser

%YYSTYPE PascalABCCompiler.ParserTools.Union

%token <ti> VAR ASSIGN SEMICOLON LPAR RPAR
%token <ex> INTNUM
%token <id> ID

%type <stn> program stmt_list stmt var_stmt proc_func_call
%type <id> ident
%type <ex> param

%start program

%%
program
	: stmt_list
		{
			// main program
			var stl = $1 as statement_list;
            stl.left_logical_bracket = new token_info("");
            stl.right_logical_bracket = new token_info("");
            
            var bl = new block(new declarations(), stl, @$);
            
            root = $$ = NewProgramModule(bl, @$);
        }
    ;

stmt_list
    : stmt SEMICOLON                                    
        { 
			$$ = new statement_list($1 as statement, @1);
        }
    | stmt_list stmt SEMICOLON                
        {  
			$$ = ($1 as statement_list).Add($2 as statement, @$);
        }
    ;

stmt 
    : var_stmt
        { 
			$$ = $1;
		}
    | proc_func_call
        {
            $$ = $1;
        }
    ;

var_stmt
    : VAR ident ASSIGN INTNUM
        { 
			$$ = new var_statement(new var_def_statement(new ident_list($2), null, $4, definition_attribute.None, false, @$));
		}
    ;

proc_func_call
	: ident LPAR param RPAR
		{
			$$ = new procedure_call(new method_call($1 as addressed_value, new expression_list($3, @$)), true, @$);
		}
	;

param 
    : ident
        {
            $$ = $1;
        }
    | INTNUM
    ;

ident
	: ID
		{
			$$ = $1;
		}
	;

%%

public program_module NewProgramModule(block progBlock, LexLocation loc)
{
    var progModule = new program_module(null, new uses_list(), progBlock, null, loc);

    progModule.source_context = progBlock.source_context;

    return progModule;
}