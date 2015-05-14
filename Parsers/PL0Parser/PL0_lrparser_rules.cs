
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using PascalABCCompiler.PL0Parser.Errors;
using PascalABCCompiler.ParserTools;
using GoldParser;

namespace  PascalABCCompiler.PL0Parser
{
public partial class GPBParser_PL0 : GPBParser
{







///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//SymbolConstants
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

public enum SymbolConstants : int
{
	SYMBOL_EOF                =  0, // (EOF)
	SYMBOL_ERROR              =  1, // (Error)
	SYMBOL_WHITESPACE         =  2, // (Whitespace)
	SYMBOL_COMMENTEND         =  3, // (Comment End)
	SYMBOL_COMMENTLINE        =  4, // (Comment Line)
	SYMBOL_COMMENTSTART       =  5, // (Comment Start)
	SYMBOL_TKASSIGN           =  6, // 'tkAssign'
	SYMBOL_TKBEGIN            =  7, // 'tkBegin'
	SYMBOL_TKCALL             =  8, // 'tkCall'
	SYMBOL_TKCOMMA            =  9, // 'tkComma'
	SYMBOL_TKCONST            = 10, // 'tkConst'
	SYMBOL_TKDO               = 11, // 'tkDo'
	SYMBOL_TKDOT              = 12, // 'tkDot'
	SYMBOL_TKEND              = 13, // 'tkEnd'
	SYMBOL_TKEQUAL            = 14, // 'tkEqual'
	SYMBOL_TKGREATER          = 15, // 'tkGreater'
	SYMBOL_TKGREATEREQUAL     = 16, // 'tkGreaterEqual'
	SYMBOL_TKIDENT            = 17, // 'tkIdent'
	SYMBOL_TKIF               = 18, // 'tkIf'
	SYMBOL_TKLOWER            = 19, // 'tkLower'
	SYMBOL_TKLOWEREQUAL       = 20, // 'tkLowerEqual'
	SYMBOL_TKMINUS            = 21, // 'tkMinus'
	SYMBOL_TKNOTEQUAL         = 22, // 'tkNotEqual'
	SYMBOL_TKNUMBER           = 23, // 'tkNumber'
	SYMBOL_TKODD              = 24, // 'tkOdd'
	SYMBOL_TKPLUS             = 25, // 'tkPlus'
	SYMBOL_TKPROCEDURE        = 26, // 'tkProcedure'
	SYMBOL_TKREADLN           = 27, // 'tkReadln'
	SYMBOL_TKROUNDCLOSE       = 28, // 'tkRoundClose'
	SYMBOL_TKROUNDOPEN        = 29, // 'tkRoundOpen'
	SYMBOL_TKSEMICOLON        = 30, // 'tkSemiColon'
	SYMBOL_TKSLASH            = 31, // 'tkSlash'
	SYMBOL_TKSTAR             = 32, // 'tkStar'
	SYMBOL_TKTHEN             = 33, // 'tkThen'
	SYMBOL_TKVAR              = 34, // 'tkVar'
	SYMBOL_TKWHILE            = 35, // 'tkWhile'
	SYMBOL_TKWRITELN          = 36, // 'tkWriteln'
	SYMBOL_ADDOP              = 37, // <addop>
	SYMBOL_BLOCK              = 38, // <block>
	SYMBOL_COMPOUND_STATEMENT = 39, // <compound_statement>
	SYMBOL_CONSTANTS          = 40, // <constants>
	SYMBOL_CONSTDECL          = 41, // <constdecl>
	SYMBOL_CONSTDEF           = 42, // <constdef>
	SYMBOL_DECLARATION        = 43, // <declaration>
	SYMBOL_DECLARATIONS       = 44, // <declarations>
	SYMBOL_EMPTY              = 45, // <empty>
	SYMBOL_EXPR_LIST          = 46, // <expr_list>
	SYMBOL_EXPRESSION         = 47, // <expression>
	SYMBOL_FACTOR             = 48, // <factor>
	SYMBOL_MULOP              = 49, // <mulop>
	SYMBOL_OPT_DECLARATIONS   = 50, // <opt_declarations>
	SYMBOL_OPT_PARAMS_LIST    = 51, // <opt_params_list>
	SYMBOL_PROC_DECL          = 52, // <proc_decl>
	SYMBOL_PROGRAM            = 53, // <program>
	SYMBOL_RELOP              = 54, // <relop>
	SYMBOL_SIMPLE_EXPR        = 55, // <simple_expr>
	SYMBOL_STATEMENT          = 56, // <statement>
	SYMBOL_TERM               = 57, // <term>
	SYMBOL_VARDECLS           = 58, // <vardecls>
	SYMBOL_VARIABLES          = 59  // <variables>
};














///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//RuleConstants
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

public enum RuleConstants : int
{
	RULE_PROGRAM_TKDOT                                         =  0, // <program> ::= <block> 'tkDot'
	RULE_BLOCK                                                 =  1, // <block> ::= <opt_declarations> <statement>
	RULE_OPT_DECLARATIONS                                      =  2, // <opt_declarations> ::= <declarations>
	RULE_OPT_DECLARATIONS2                                     =  3, // <opt_declarations> ::= 
	RULE_DECLARATIONS                                          =  4, // <declarations> ::= <declaration> <empty>
	RULE_DECLARATIONS2                                         =  5, // <declarations> ::= <declarations> <empty> <declaration>
	RULE_DECLARATION                                           =  6, // <declaration> ::= <constants>
	RULE_DECLARATION2                                          =  7, // <declaration> ::= <variables>
	RULE_DECLARATION3                                          =  8, // <declaration> ::= <proc_decl>
	RULE_CONSTANTS_TKCONST_TKSEMICOLON                         =  9, // <constants> ::= 'tkConst' <constdecl> 'tkSemiColon'
	RULE_CONSTDECL                                             = 10, // <constdecl> ::= <constdef> <empty>
	RULE_CONSTDECL_TKCOMMA                                     = 11, // <constdecl> ::= <constdecl> 'tkComma' <constdef>
	RULE_CONSTDEF_TKIDENT_TKEQUAL_TKNUMBER                     = 12, // <constdef> ::= 'tkIdent' 'tkEqual' 'tkNumber'
	RULE_VARIABLES_TKVAR_TKSEMICOLON                           = 13, // <variables> ::= 'tkVar' <vardecls> 'tkSemiColon'
	RULE_VARDECLS_TKIDENT                                      = 14, // <vardecls> ::= 'tkIdent' <empty>
	RULE_VARDECLS_TKCOMMA_TKIDENT                              = 15, // <vardecls> ::= <vardecls> 'tkComma' 'tkIdent'
	RULE_PROC_DECL_TKPROCEDURE_TKIDENT_TKSEMICOLON_TKSEMICOLON = 16, // <proc_decl> ::= 'tkProcedure' 'tkIdent' 'tkSemiColon' <block> 'tkSemiColon'
	RULE_STATEMENT_TKIDENT_TKASSIGN                            = 17, // <statement> ::= 'tkIdent' 'tkAssign' <simple_expr>
	RULE_STATEMENT_TKCALL_TKIDENT                              = 18, // <statement> ::= 'tkCall' 'tkIdent' <opt_params_list>
	RULE_STATEMENT_TKWRITELN                                   = 19, // <statement> ::= 'tkWriteln' <simple_expr>
	RULE_STATEMENT_TKREADLN_TKIDENT                            = 20, // <statement> ::= 'tkReadln' 'tkIdent'
	RULE_STATEMENT_TKBEGIN_TKEND                               = 21, // <statement> ::= 'tkBegin' <compound_statement> 'tkEnd'
	RULE_STATEMENT_TKIF_TKTHEN                                 = 22, // <statement> ::= 'tkIf' <expression> 'tkThen' <statement>
	RULE_STATEMENT_TKWHILE_TKDO                                = 23, // <statement> ::= 'tkWhile' <expression> 'tkDo' <statement>
	RULE_STATEMENT                                             = 24, // <statement> ::= <empty> <empty>
	RULE_OPT_PARAMS_LIST_TKROUNDOPEN_TKROUNDCLOSE              = 25, // <opt_params_list> ::= 'tkRoundOpen' <expr_list> 'tkRoundClose'
	RULE_OPT_PARAMS_LIST                                       = 26, // <opt_params_list> ::= 
	RULE_EXPR_LIST                                             = 27, // <expr_list> ::= <simple_expr> <empty>
	RULE_EXPR_LIST_TKCOMMA                                     = 28, // <expr_list> ::= <expr_list> 'tkComma' <simple_expr>
	RULE_COMPOUND_STATEMENT                                    = 29, // <compound_statement> ::= <statement> <empty>
	RULE_COMPOUND_STATEMENT_TKSEMICOLON                        = 30, // <compound_statement> ::= <compound_statement> 'tkSemiColon' <statement>
	RULE_EXPRESSION_TKODD                                      = 31, // <expression> ::= 'tkOdd' <simple_expr>
	RULE_EXPRESSION                                            = 32, // <expression> ::= <simple_expr> <relop> <simple_expr>
	RULE_SIMPLE_EXPR                                           = 33, // <simple_expr> ::= <term>
	RULE_SIMPLE_EXPR2                                          = 34, // <simple_expr> ::= <simple_expr> <addop> <term>
	RULE_TERM                                                  = 35, // <term> ::= <factor>
	RULE_TERM2                                                 = 36, // <term> ::= <term> <mulop> <factor>
	RULE_FACTOR_TKIDENT                                        = 37, // <factor> ::= 'tkIdent'
	RULE_FACTOR_TKNUMBER                                       = 38, // <factor> ::= 'tkNumber'
	RULE_FACTOR_TKROUNDOPEN_TKROUNDCLOSE                       = 39, // <factor> ::= 'tkRoundOpen' <expression> 'tkRoundClose'
	RULE_RELOP_TKEQUAL                                         = 40, // <relop> ::= 'tkEqual'
	RULE_RELOP_TKNOTEQUAL                                      = 41, // <relop> ::= 'tkNotEqual'
	RULE_RELOP_TKLOWER                                         = 42, // <relop> ::= 'tkLower'
	RULE_RELOP_TKLOWEREQUAL                                    = 43, // <relop> ::= 'tkLowerEqual'
	RULE_RELOP_TKGREATER                                       = 44, // <relop> ::= 'tkGreater'
	RULE_RELOP_TKGREATEREQUAL                                  = 45, // <relop> ::= 'tkGreaterEqual'
	RULE_ADDOP_TKPLUS                                          = 46, // <addop> ::= 'tkPlus'
	RULE_ADDOP_TKMINUS                                         = 47, // <addop> ::= 'tkMinus'
	RULE_MULOP_TKSTAR                                          = 48, // <mulop> ::= 'tkStar'
	RULE_MULOP_TKSLASH                                         = 49, // <mulop> ::= 'tkSlash'
	RULE_EMPTY                                                 = 50  // <empty> ::= 
};













///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//CreateTerminalObject
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

private Object CreateTerminalObject(int TokenSymbolIndex)
{
switch (TokenSymbolIndex)
{
	case (int)SymbolConstants.SYMBOL_EOF :
    	//(EOF)
	//TERMINAL:EOF
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ERROR :
    	//(Error)
	//TERMINAL:Error
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_WHITESPACE :
    	//(Whitespace)
	//TERMINAL:Whitespace
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_COMMENTEND :
    	//(Comment End)
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'Comment End' return NULL! \n\r");*/return null;
	case (int)SymbolConstants.SYMBOL_COMMENTLINE :
    	//(Comment Line)
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'Comment Line' return NULL! \n\r");*/return null;
	case (int)SymbolConstants.SYMBOL_COMMENTSTART :
    	//(Comment Start)
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'Comment Start' return NULL! \n\r");*/return null;
	case (int)SymbolConstants.SYMBOL_TKASSIGN :
    	//'tkAssign'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Assignment);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKBEGIN :
    	//'tkBegin'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKCALL :
    	//'tkCall'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKCOMMA :
    	//'tkComma'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKCONST :
    	//'tkConst'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKDO :
    	//'tkDo'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKDOT :
    	//'tkDot'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKEND :
    	//'tkEnd'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKEQUAL :
    	//'tkEqual'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Equal);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKGREATER :
    	//'tkGreater'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Greater);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKGREATEREQUAL :
    	//'tkGreaterEqual'

		{
			op_type_node _op_type_node=new op_type_node(Operators.GreaterEqual);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKIDENT :
    	//'tkIdent'
return parsertools.create_ident(this);
	case (int)SymbolConstants.SYMBOL_TKIF :
    	//'tkIf'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKLOWER :
    	//'tkLower'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Less);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKLOWEREQUAL :
    	//'tkLowerEqual'

		{
			op_type_node _op_type_node=new op_type_node(Operators.LessEqual);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKMINUS :
    	//'tkMinus'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Minus);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKNOTEQUAL :
    	//'tkNotEqual'

		{
			op_type_node _op_type_node=new op_type_node(Operators.NotEqual);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKNUMBER :
    	//'tkNumber'
return parsertools.create_int_const(this);
	case (int)SymbolConstants.SYMBOL_TKODD :
    	//'tkOdd'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKPLUS :
    	//'tkPlus'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Plus);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKPROCEDURE :
    	//'tkProcedure'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKREADLN :
    	//'tkReadln'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKROUNDCLOSE :
    	//'tkRoundClose'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKROUNDOPEN :
    	//'tkRoundOpen'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKSEMICOLON :
    	//'tkSemiColon'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKSLASH :
    	//'tkSlash'

		{
			op_type_node _op_type_node=new op_type_node(Operators.IntegerDivision);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKSTAR :
    	//'tkStar'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Multiplication);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKTHEN :
    	//'tkThen'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKVAR :
    	//'tkVar'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKWHILE :
    	//'tkWhile'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKWRITELN :
    	//'tkWriteln'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_ADDOP :
    	//<addop>
	//TERMINAL:addop
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_BLOCK :
    	//<block>
	//TERMINAL:block
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_COMPOUND_STATEMENT :
    	//<compound_statement>
	//TERMINAL:compound_statement
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONSTANTS :
    	//<constants>
	//TERMINAL:constants
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONSTDECL :
    	//<constdecl>
	//TERMINAL:constdecl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONSTDEF :
    	//<constdef>
	//TERMINAL:constdef
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_DECLARATION :
    	//<declaration>
	//TERMINAL:declaration
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_DECLARATIONS :
    	//<declarations>
	//TERMINAL:declarations
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EMPTY :
    	//<empty>
	//TERMINAL:empty
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXPR_LIST :
    	//<expr_list>
	//TERMINAL:expr_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXPRESSION :
    	//<expression>
	//TERMINAL:expression
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FACTOR :
    	//<factor>
	//TERMINAL:factor
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_MULOP :
    	//<mulop>
	//TERMINAL:mulop
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPT_DECLARATIONS :
    	//<opt_declarations>
	//TERMINAL:opt_declarations
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPT_PARAMS_LIST :
    	//<opt_params_list>
	//TERMINAL:opt_params_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROC_DECL :
    	//<proc_decl>
	//TERMINAL:proc_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROGRAM :
    	//<program>
	//TERMINAL:program
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RELOP :
    	//<relop>
	//TERMINAL:relop
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_SIMPLE_EXPR :
    	//<simple_expr>
	//TERMINAL:simple_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_STATEMENT :
    	//<statement>
	//TERMINAL:statement
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TERM :
    	//<term>
	//TERMINAL:term
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VARDECLS :
    	//<vardecls>
	//TERMINAL:vardecls
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VARIABLES :
    	//<variables>
	//TERMINAL:variables
	return null;
	//ENDTERMINAL
}
throw new SymbolException("Unknown symbol");
}















///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//CreateNonTerminalObject
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

public Object CreateNonTerminalObject(int ReductionRuleIndex)
{
switch (ReductionRuleIndex)
{
	case (int)RuleConstants.RULE_PROGRAM_TKDOT :
	//<program> ::= <block> 'tkDot'
         
		{
			program_module _program_module=new program_module(null,null,(block)LRParser.GetReductionSyntaxNode(0),null);
			
					_program_module.Language = LanguageId.PascalABCNET;
					parsertools.create_source_context(_program_module,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)),LRParser.GetReductionSyntaxNode(1));
			return _program_module;
		}

	case (int)RuleConstants.RULE_BLOCK :
	//<block> ::= <opt_declarations> <statement>
         
		{
			block _block=new block((declarations)LRParser.GetReductionSyntaxNode(0),null);
			
								statement_list sl=null;
								if(LRParser.GetReductionSyntaxNode(1) is statement_list)
									sl = LRParser.GetReductionSyntaxNode(1) as statement_list;
								else
								{
									sl = new statement_list();
									sl.subnodes.Add(LRParser.GetReductionSyntaxNode(1) as statement);
									if(!(LRParser.GetReductionSyntaxNode(1) is empty_statement))
										parsertools.assign_source_context(sl,LRParser.GetReductionSyntaxNode(1));						
								}
								_block.program_code=sl;
								if(!(LRParser.GetReductionSyntaxNode(1) is empty_statement))
									parsertools.create_source_context(_block,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));						
								
			return _block;
		}

	case (int)RuleConstants.RULE_OPT_DECLARATIONS :
	//<opt_declarations> ::= <declarations>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OPT_DECLARATIONS2 :
	//<opt_declarations> ::= 
	//NONTERMINAL:<opt_declarations> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_DECLARATIONS :
	//<declarations> ::= <declaration> <empty>
         
		//TemplateList for declarations (create)
		{
			declarations _declarations=new declarations();
			_declarations.source_context=((declaration)LRParser.GetReductionSyntaxNode(0)).source_context;
			_declarations.defs.Add((declaration)LRParser.GetReductionSyntaxNode(0));
			return _declarations;
		}

	case (int)RuleConstants.RULE_DECLARATIONS2 :
	//<declarations> ::= <declarations> <empty> <declaration>

		//TemplateList for declarations (add)         
		{
			declarations _declarations=(declarations)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_declarations,_declarations,LRParser.GetReductionSyntaxNode(2));
			_declarations.defs.Add(LRParser.GetReductionSyntaxNode(2) as declaration);
			return _declarations;
		}

	case (int)RuleConstants.RULE_DECLARATION :
	//<declaration> ::= <constants>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_DECLARATION2 :
	//<declaration> ::= <variables>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_DECLARATION3 :
	//<declaration> ::= <proc_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONSTANTS_TKCONST_TKSEMICOLON :
	//<constants> ::= 'tkConst' <constdecl> 'tkSemiColon'
 parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2)); return LRParser.GetReductionSyntaxNode(1); 
	case (int)RuleConstants.RULE_CONSTDECL :
	//<constdecl> ::= <constdef> <empty>
         
		//TemplateList for consts_definitions_list (create)
		{
			consts_definitions_list _consts_definitions_list=new consts_definitions_list();
			_consts_definitions_list.source_context=((const_definition)LRParser.GetReductionSyntaxNode(0)).source_context;
			_consts_definitions_list.const_defs.Add((const_definition)LRParser.GetReductionSyntaxNode(0));
			return _consts_definitions_list;
		}

	case (int)RuleConstants.RULE_CONSTDECL_TKCOMMA :
	//<constdecl> ::= <constdecl> 'tkComma' <constdef>

		//TemplateList for consts_definitions_list (add)         
		{
			consts_definitions_list _consts_definitions_list=(consts_definitions_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_consts_definitions_list,_consts_definitions_list,LRParser.GetReductionSyntaxNode(2));
			_consts_definitions_list.const_defs.Add(LRParser.GetReductionSyntaxNode(2) as const_definition);
			return _consts_definitions_list;
		}

	case (int)RuleConstants.RULE_CONSTDEF_TKIDENT_TKEQUAL_TKNUMBER :
	//<constdef> ::= 'tkIdent' 'tkEqual' 'tkNumber'
         
		{
			simple_const_definition _simple_const_definition=new simple_const_definition();
			
								_simple_const_definition.const_name=(ident)LRParser.GetReductionSyntaxNode(0);
								_simple_const_definition.const_value=(expression)LRParser.GetReductionSyntaxNode(2);
								parsertools.create_source_context(_simple_const_definition,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _simple_const_definition;
		}

	case (int)RuleConstants.RULE_VARIABLES_TKVAR_TKSEMICOLON :
	//<variables> ::= 'tkVar' <vardecls> 'tkSemiColon'
         
		{
			variable_definitions _variable_definitions=new variable_definitions();
			
								//var_def_statement vdf = new var_def_statement((ident_list)LRParser.GetReductionSyntaxNode(1),null,new int32_const(0),definition_attribute.None,false);
								foreach (ident id in ((ident_list)LRParser.GetReductionSyntaxNode(1)).idents)
								{
								   ident_list idl=new ident_list();
								   idl.idents.Add(id);
								   parsertools.create_source_context(idl,id,id);
								   var_def_statement vdf = new var_def_statement(idl,null,new int32_const(0),definition_attribute.None,false);
								   parsertools.create_source_context(vdf,id,id);
  							           _variable_definitions.var_definitions.Add(vdf);
  								}								
								parsertools.create_source_context(_variable_definitions,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
								
			return _variable_definitions;
		}

	case (int)RuleConstants.RULE_VARDECLS_TKIDENT :
	//<vardecls> ::= 'tkIdent' <empty>
         
		//TemplateList for ident_list (create)
		{
			ident_list _ident_list=new ident_list();
			_ident_list.source_context=((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
			_ident_list.idents.Add((ident)LRParser.GetReductionSyntaxNode(0));
			return _ident_list;
		}

	case (int)RuleConstants.RULE_VARDECLS_TKCOMMA_TKIDENT :
	//<vardecls> ::= <vardecls> 'tkComma' 'tkIdent'

		//TemplateList for ident_list (add)         
		{
			ident_list _ident_list=(ident_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_ident_list,_ident_list,LRParser.GetReductionSyntaxNode(2));
			_ident_list.idents.Add(LRParser.GetReductionSyntaxNode(2) as ident);
			return _ident_list;
		}

	case (int)RuleConstants.RULE_PROC_DECL_TKPROCEDURE_TKIDENT_TKSEMICOLON_TKSEMICOLON :
	//<proc_decl> ::= 'tkProcedure' 'tkIdent' 'tkSemiColon' <block> 'tkSemiColon'
 
								method_name mn = new method_name(null,(ident)LRParser.GetReductionSyntaxNode(1),null);
								procedure_header ph = new procedure_header(null,null,mn,false,false,null,null); 
								procedure_definition pd = new procedure_definition(ph,(block)LRParser.GetReductionSyntaxNode(3));
								parsertools.create_source_context(mn,LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(1));		
								parsertools.create_source_context(ph,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));		
								parsertools.create_source_context(pd,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
								return pd;
	case (int)RuleConstants.RULE_STATEMENT_TKIDENT_TKASSIGN :
	//<statement> ::= 'tkIdent' 'tkAssign' <simple_expr>
         
		{
			assign _assign=new assign(LRParser.GetReductionSyntaxNode(0) as addressed_value,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_assign,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _assign;
		}

	case (int)RuleConstants.RULE_STATEMENT_TKCALL_TKIDENT :
	//<statement> ::= 'tkCall' 'tkIdent' <opt_params_list>
         
		{
			procedure_call _procedure_call=new procedure_call();
			
								method_call mc = new method_call(LRParser.GetReductionSyntaxNode(2) as expression_list);
								mc.dereferencing_value=(addressed_value)LRParser.GetReductionSyntaxNode(1);
								parsertools.create_source_context(mc,LRParser.GetReductionSyntaxNode(1),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1)));
								_procedure_call.func_name = mc;
								parsertools.create_source_context(_procedure_call,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1)));
			return _procedure_call;
		}

	case (int)RuleConstants.RULE_STATEMENT_TKWRITELN :
	//<statement> ::= 'tkWriteln' <simple_expr>

{
                        procedure_call pc = new procedure_call();
			expression_list expr = new expression_list();
			expr.expressions.Add((expression)LRParser.GetReductionSyntaxNode(1));
			method_call mc = new method_call(expr);
			mc.dereferencing_value = new ident("writeln");
			pc.func_name = mc;
			parsertools.create_source_context(mc.dereferencing_value,LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(1));
			parsertools.create_source_context(expr,LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(1));
			parsertools.create_source_context(mc,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			parsertools.create_source_context(pc,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return pc;
}

	case (int)RuleConstants.RULE_STATEMENT_TKREADLN_TKIDENT :
	//<statement> ::= 'tkReadln' 'tkIdent'

{
                        procedure_call pc = new procedure_call();
			expression_list expr = new expression_list();
			expr.expressions.Add((expression)LRParser.GetReductionSyntaxNode(1));
			method_call mc = new method_call(expr);
			mc.dereferencing_value = new ident("readln");
			pc.func_name = mc;
			parsertools.create_source_context(mc.dereferencing_value,LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(1));
			parsertools.create_source_context(expr,LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(1));
			parsertools.create_source_context(mc,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			parsertools.create_source_context(pc,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return pc;
}

	case (int)RuleConstants.RULE_STATEMENT_TKBEGIN_TKEND :
	//<statement> ::= 'tkBegin' <compound_statement> 'tkEnd'
parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
								((statement_list)LRParser.GetReductionSyntaxNode(1)).left_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(0);
								((statement_list)LRParser.GetReductionSyntaxNode(1)).right_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(2);
								return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_STATEMENT_TKIF_TKTHEN :
	//<statement> ::= 'tkIf' <expression> 'tkThen' <statement>
         
		{
			if_node _if_node=new if_node((expression)LRParser.GetReductionSyntaxNode(1),(statement)LRParser.GetReductionSyntaxNode(3),null);
			
								parsertools.create_source_context(_if_node,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(2)));
			return _if_node;
		}

	case (int)RuleConstants.RULE_STATEMENT_TKWHILE_TKDO :
	//<statement> ::= 'tkWhile' <expression> 'tkDo' <statement>
         
		{
			while_node _while_node=new while_node((expression)LRParser.GetReductionSyntaxNode(1),(statement)LRParser.GetReductionSyntaxNode(3),WhileCycleType.While);
			
								parsertools.create_source_context(_while_node,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(2)));
			return _while_node;
		}

	case (int)RuleConstants.RULE_STATEMENT :
	//<statement> ::= <empty> <empty>
         
		{
			empty_statement _empty_statement=new empty_statement();
			
			return _empty_statement;
		}

	case (int)RuleConstants.RULE_OPT_PARAMS_LIST_TKROUNDOPEN_TKROUNDCLOSE :
	//<opt_params_list> ::= 'tkRoundOpen' <expr_list> 'tkRoundClose'
 return LRParser.GetReductionSyntaxNode(1); 
	case (int)RuleConstants.RULE_OPT_PARAMS_LIST :
	//<opt_params_list> ::= 
	//NONTERMINAL:<opt_params_list> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_EXPR_LIST :
	//<expr_list> ::= <simple_expr> <empty>
         
		//TemplateList for expression_list (create)
		{
			expression_list _expression_list=new expression_list();
			_expression_list.source_context=((expression)LRParser.GetReductionSyntaxNode(0)).source_context;
			_expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
			return _expression_list;
		}

	case (int)RuleConstants.RULE_EXPR_LIST_TKCOMMA :
	//<expr_list> ::= <expr_list> 'tkComma' <simple_expr>

		//TemplateList for expression_list (add)         
		{
			expression_list _expression_list=(expression_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_expression_list,_expression_list,LRParser.GetReductionSyntaxNode(2));
			_expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(2) as expression);
			return _expression_list;
		}

	case (int)RuleConstants.RULE_COMPOUND_STATEMENT :
	//<compound_statement> ::= <statement> <empty>
         
		{
			statement_list _statement_list=new statement_list();
			
								_statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(0));
								parsertools.assign_source_context(_statement_list,LRParser.GetReductionSyntaxNode(0)); 
			return _statement_list;
		}

	case (int)RuleConstants.RULE_COMPOUND_STATEMENT_TKSEMICOLON :
	//<compound_statement> ::= <compound_statement> 'tkSemiColon' <statement>
         
		{
			statement_list _statement_list;
			 _statement_list=(statement_list)LRParser.GetReductionSyntaxNode(0);
								_statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(2));
								parsertools.create_source_context(_statement_list,_statement_list,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1))); 
			return _statement_list;
		}

	case (int)RuleConstants.RULE_EXPRESSION_TKODD :
	//<expression> ::= 'tkOdd' <simple_expr>
         
		{
			bin_expr _bin_expr=new bin_expr(new bin_expr((expression)LRParser.GetReductionSyntaxNode(1), new int32_const(2), Operators.ModulusRemainder),new int32_const(0),Operators.Equal);
			
								parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_EXPRESSION :
	//<expression> ::= <simple_expr> <relop> <simple_expr>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_SIMPLE_EXPR :
	//<simple_expr> ::= <term>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_EXPR2 :
	//<simple_expr> ::= <simple_expr> <addop> <term>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_TERM :
	//<term> ::= <factor>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TERM2 :
	//<term> ::= <term> <mulop> <factor>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_FACTOR_TKIDENT :
	//<factor> ::= 'tkIdent'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FACTOR_TKNUMBER :
	//<factor> ::= 'tkNumber'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FACTOR_TKROUNDOPEN_TKROUNDCLOSE :
	//<factor> ::= 'tkRoundOpen' <expression> 'tkRoundClose'
 return LRParser.GetReductionSyntaxNode(1); 
	case (int)RuleConstants.RULE_RELOP_TKEQUAL :
	//<relop> ::= 'tkEqual'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RELOP_TKNOTEQUAL :
	//<relop> ::= 'tkNotEqual'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RELOP_TKLOWER :
	//<relop> ::= 'tkLower'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RELOP_TKLOWEREQUAL :
	//<relop> ::= 'tkLowerEqual'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RELOP_TKGREATER :
	//<relop> ::= 'tkGreater'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RELOP_TKGREATEREQUAL :
	//<relop> ::= 'tkGreaterEqual'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ADDOP_TKPLUS :
	//<addop> ::= 'tkPlus'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ADDOP_TKMINUS :
	//<addop> ::= 'tkMinus'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_MULOP_TKSTAR :
	//<mulop> ::= 'tkStar'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_MULOP_TKSLASH :
	//<mulop> ::= 'tkSlash'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EMPTY :
	//<empty> ::= 
	//NONTERMINAL:<empty> ::= 
	return null;
	//ENDNONTERMINAL
}
throw new RuleException("Unknown rule");
}  






} 
}
