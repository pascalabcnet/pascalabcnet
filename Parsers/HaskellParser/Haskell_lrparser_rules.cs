
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using PascalABCCompiler.HaskellParser.Errors;
using PascalABCCompiler.ParserTools;
using GoldParser;
using PascalABCCompiler;

namespace  PascalABCCompiler.HaskellParser
{
public partial class GPBParser_Haskell : GPBParser
{







    ///////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////
    //SymbolConstants
    ///////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////

    public enum SymbolConstants : int
    {
        SYMBOL_EOF = 0, // (EOF)
        SYMBOL_ERROR = 1, // (Error)
        SYMBOL_WHITESPACE = 2, // (Whitespace)
        SYMBOL_COMMENTEND = 3, // (Comment End)
        SYMBOL_COMMENTLINE = 4, // (Comment Line)
        SYMBOL_COMMENTSTART = 5, // (Comment Start)
        SYMBOL_TKAND = 6, // 'tkAnd'
        SYMBOL_TKARROW = 7, // 'tkArrow'
        SYMBOL_TKARROWGEN = 8, // 'tkArrowGen'
        SYMBOL_TKASSIGN = 9, // 'tkAssign'
        SYMBOL_TKBOOL = 10, // 'tkBool'
        SYMBOL_TKBOTTOMMINUS = 11, // 'tkBottomMinus'
        SYMBOL_TKCASE = 12, // 'tkCase'
        SYMBOL_TKCHAR = 13, // 'tkChar'
        SYMBOL_TKCOLON = 14, // 'tkColon'
        SYMBOL_TKCOMMA = 15, // 'tkComma'
        SYMBOL_TKDO = 16, // 'tkDo'
        SYMBOL_TKDOT = 17, // 'tkDot'
        SYMBOL_TKDOUBLE = 18, // 'tkDouble'
        SYMBOL_TKELSE = 19, // 'tkElse'
        SYMBOL_TKEQUAL = 20, // 'tkEqual'
        SYMBOL_TKFIGURECLOSE = 21, // 'tkFigureClose'
        SYMBOL_TKFIGUREOPEN = 22, // 'tkFigureOpen'
        SYMBOL_TKIDENT = 23, // 'tkIdent'
        SYMBOL_TKIF = 24, // 'tkIf'
        SYMBOL_TKIMPORT = 25, // 'tkImport'
        SYMBOL_TKIN = 26, // 'tkIn'
        SYMBOL_TKINT = 27, // 'tkInt'
        SYMBOL_TKLEFTSLASH = 28, // 'tkLeftSlash'
        SYMBOL_TKLESS = 29, // 'tkLess'
        SYMBOL_TKLESSEQ = 30, // 'tkLessEq'
        SYMBOL_TKLET = 31, // 'tkLet'
        SYMBOL_TKMAINIDENT = 32, // 'tkMainIdent'
        SYMBOL_TKMAINIDENT1 = 33, // 'tkMainIdent1'
        SYMBOL_TKMINUS = 34, // 'tkMinus'
        SYMBOL_TKMODULE = 35, // 'tkModule'
        SYMBOL_TKMORE = 36, // 'tkMore'
        SYMBOL_TKMOREEQ = 37, // 'tkMoreEq'
        SYMBOL_TKNOT = 38, // 'tkNot'
        SYMBOL_TKNOTEQUAL = 39, // 'tkNotEqual'
        SYMBOL_TKOF = 40, // 'tkOf'
        SYMBOL_TKOR = 41, // 'tkOr'
        SYMBOL_TKOTHERWISE = 42, // 'tkOtherwise'
        SYMBOL_TKPLUS = 43, // 'tkPlus'
        SYMBOL_TKQUOTE = 44, // 'tkQuote'
        SYMBOL_TKREF = 45, // 'tkRef'
        SYMBOL_TKRETURN = 46, // 'tkReturn'
        SYMBOL_TKROUNDCLOSE = 47, // 'tkRoundClose'
        SYMBOL_TKROUNDOPEN = 48, // 'tkRoundOpen'
        SYMBOL_TKSEMICOLON = 49, // 'tkSemiColon'
        SYMBOL_TKSLASH = 50, // 'tkSlash'
        SYMBOL_TKSPLIT = 51, // 'tkSplit'
        SYMBOL_TKSQUARECLOSE = 52, // 'tkSquareClose'
        SYMBOL_TKSQUAREOPEN = 53, // 'tkSquareOpen'
        SYMBOL_TKSTAR = 54, // 'tkStar'
        SYMBOL_TKSTRING = 55, // 'tkString'
        SYMBOL_TKTHEN = 56, // 'tkThen'
        SYMBOL_TKWHERE = 57, // 'tkWhere'
        SYMBOL_ADD_EXPR = 58, // <add_expr>
        SYMBOL_ADDOP = 59, // <addop>
        SYMBOL_BODY = 60, // <body>
        SYMBOL_BODY_FUNC = 61, // <body_func>
        SYMBOL_BODY_WHERE = 62, // <body_where>
        SYMBOL_CASE_VARIANT = 63, // <case_variant>
        SYMBOL_CASE_VARIANTS = 64, // <case_variants>
        SYMBOL_CONDITION = 65, // <condition>
        SYMBOL_CONDITIONS = 66, // <conditions>
        SYMBOL_CONDITIONS_COMMA = 67, // <conditions_comma>
        SYMBOL_CORTEG = 68, // <corteg>
        SYMBOL_DEF_VAR = 69, // <def_var>
        SYMBOL_DEF_VARS = 70, // <def_vars>
        SYMBOL_EMPTY = 71, // <empty>
        SYMBOL_EXPR = 72, // <expr>
        SYMBOL_FUNC_CALL = 73, // <func_call>
        SYMBOL_FUNCS = 74, // <funcs>
        SYMBOL_FUNCS_VARIANTS = 75, // <funcs_variants>
        SYMBOL_GENERATOR = 76, // <generator>
        SYMBOL_GENERATORS = 77, // <generators>
        SYMBOL_GUARD = 78, // <guard>
        SYMBOL_GUARD_BODY = 79, // <guard_body>
        SYMBOL_GUARD_BODY_LIST = 80, // <guard_body_list>
        SYMBOL_IMPORT = 81, // <import>
        SYMBOL_IMPORTS = 82, // <imports>
        SYMBOL_INFIX_EXPR = 83, // <infix_expr>
        SYMBOL_INIT = 84, // <init>
        SYMBOL_INITS = 85, // <inits>
        SYMBOL_LAMBDA_FUNC = 86, // <lambda_func>
        SYMBOL_LIST = 87, // <list>
        SYMBOL_LIST_CONSTRUCTOR = 88, // <list_constructor>
        SYMBOL_LIST_ELEMENTS = 89, // <list_elements>
        SYMBOL_LIST_PARAM = 90, // <list_param>
        SYMBOL_LIST_PARAM1 = 91, // <list_param1>
        SYMBOL_MAIN_FUNC = 92, // <main_func>
        SYMBOL_MODULE = 93, // <module>
        SYMBOL_MULT_EXPR = 94, // <mult_expr>
        SYMBOL_MULTOP = 95, // <multop>
        SYMBOL_NEGATE_EXPR = 96, // <negate_expr>
        SYMBOL_PARAM = 97, // <param>
        SYMBOL_PARAM_VALUE = 98, // <param_value>
        SYMBOL_PARAMS = 99, // <params>
        SYMBOL_PARAMS_VALUE = 100, // <params_value>
        SYMBOL_PARAMS_WHERE = 101, // <params_where>
        SYMBOL_PARAMS1 = 102, // <params1>
        SYMBOL_REFERENCE = 103, // <reference>
        SYMBOL_SIMPLE_EXPR = 104, // <simple_expr>
        SYMBOL_SIMPLE_TYPE_EXPR = 105, // <simple_type_expr>
        SYMBOL_STMT = 106, // <stmt>
        SYMBOL_STMTS = 107, // <stmts>
        SYMBOL_STMTS1 = 108, // <stmts1>
        SYMBOL_VARIABLE = 109, // <variable>
        SYMBOL_VARIABLE_EXPR = 110, // <variable_expr>
        SYMBOL_VARIANT = 111, // <variant>
        SYMBOL_VARIANTS = 112, // <variants>
        SYMBOL_WHERE_VAR = 113  // <where_var>
    };














    ///////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////
    //RuleConstants
    ///////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////

    public enum RuleConstants : int
    {
        RULE_MODULE_TKMODULE_TKMAINIDENT_TKWHERE = 0, // <module> ::= 'tkModule' 'tkMainIdent' 'tkWhere' <reference> <imports> <body> <empty>
        RULE_MODULE = 1, // <module> ::= <reference> <imports> <body> <empty>
        RULE_MODULE_TKMODULE_TKIDENT_TKWHERE = 2, // <module> ::= 'tkModule' 'tkIdent' 'tkWhere' <reference> <imports> <funcs> <empty>
        RULE_REFERENCE = 3, // <reference> ::= 
        RULE_REFERENCE_TKREF_TKSTRING = 4, // <reference> ::= 'tkRef' 'tkString'
        RULE_IMPORTS = 5, // <imports> ::= 
        RULE_IMPORTS2 = 6, // <imports> ::= <import> <empty>
        RULE_IMPORTS3 = 7, // <imports> ::= <imports> <empty> <import>
        RULE_IMPORT_TKIMPORT_TKIDENT = 8, // <import> ::= 'tkImport' 'tkIdent'
        RULE_BODY = 9, // <body> ::= <main_func>
        RULE_BODY2 = 10, // <body> ::= <funcs> <main_func>
        RULE_FUNCS = 11, // <funcs> ::= <funcs_variants> <empty>
        RULE_FUNCS_VARIANTS = 12, // <funcs_variants> ::= <variants> <empty>
        RULE_VARIANTS = 13, // <variants> ::= <variant> <empty>
        RULE_VARIANTS2 = 14, // <variants> ::= <variants> <variant>
        RULE_VARIANT_TKIDENT = 15, // <variant> ::= 'tkIdent' <params> <guard_body_list> <where_var>
        RULE_VARIANT_TKQUOTE_TKIDENT_TKQUOTE = 16, // <variant> ::= <list_param1> 'tkQuote' 'tkIdent' 'tkQuote' <list_param1> <guard_body_list> <where_var>
        RULE_LIST_PARAM1 = 17, // <list_param1> ::= <list_param> <empty>
        RULE_BODY_WHERE = 18, // <body_where> ::= <body_func> <where_var>
        RULE_WHERE_VAR = 19, // <where_var> ::= 
        RULE_WHERE_VAR_TKWHERE = 20, // <where_var> ::= 'tkWhere' <inits>
        RULE_INITS = 21, // <inits> ::= <init> <empty>
        RULE_INITS_TKSEMICOLON = 22, // <inits> ::= <inits> 'tkSemiColon' <init>
        RULE_INIT_TKIDENT = 23, // <init> ::= 'tkIdent' <params_where> <guard_body_list> <where_var>
        RULE_PARAMS_WHERE = 24, // <params_where> ::= 
        RULE_PARAMS_WHERE2 = 25, // <params_where> ::= <param> <empty>
        RULE_PARAMS_WHERE3 = 26, // <params_where> ::= <params_where> <param>
        RULE_PARAMS = 27, // <params> ::= 
        RULE_PARAMS2 = 28, // <params> ::= <param> <empty>
        RULE_PARAMS3 = 29, // <params> ::= <params> <param>
        RULE_PARAM = 30, // <param> ::= <list_param>
        RULE_LIST_PARAM_TKIDENT = 31, // <list_param> ::= 'tkIdent' <empty>
        RULE_LIST_PARAM = 32, // <list_param> ::= <simple_type_expr> <empty>
        RULE_LIST_PARAM_TKSQUAREOPEN_TKSQUARECLOSE = 33, // <list_param> ::= 'tkSquareOpen' 'tkSquareClose'
        RULE_LIST_PARAM_TKBOTTOMMINUS = 34, // <list_param> ::= 'tkBottomMinus'
        RULE_LIST_PARAM_TKIDENT_TKCOLON = 35, // <list_param> ::= 'tkIdent' 'tkColon' <list_param>
        RULE_LIST_PARAM_TKBOTTOMMINUS_TKCOLON = 36, // <list_param> ::= 'tkBottomMinus' 'tkColon' <list_param>
        RULE_LIST_PARAM_TKROUNDOPEN_TKROUNDCLOSE = 37, // <list_param> ::= 'tkRoundOpen' <list_param> 'tkRoundClose'
        RULE_GUARD = 38, // <guard> ::= 
        RULE_GUARD_TKSPLIT = 39, // <guard> ::= 'tkSplit' <expr>
        RULE_GUARD_TKSPLIT_TKOTHERWISE = 40, // <guard> ::= 'tkSplit' 'tkOtherwise'
        RULE_MAIN_FUNC_TKMAINIDENT1_TKASSIGN = 41, // <main_func> ::= 'tkMainIdent1' 'tkAssign' <body_func>
        RULE_BODY_FUNC = 42, // <body_func> ::= <stmt>
        RULE_STMTS = 43, // <stmts> ::= <stmt> <empty> <empty>
        RULE_STMTS_TKSEMICOLON = 44, // <stmts> ::= <stmts> 'tkSemiColon' <stmt>
        RULE_STMTS1 = 45, // <stmts1> ::= <stmt> <empty> <empty>
        RULE_STMTS12 = 46, // <stmts1> ::= <stmts1> <stmt>
        RULE_EXPR_TKEQUAL = 47, // <expr> ::= <expr> 'tkEqual' <add_expr>
        RULE_EXPR_TKNOTEQUAL = 48, // <expr> ::= <expr> 'tkNotEqual' <add_expr>
        RULE_EXPR_TKMORE = 49, // <expr> ::= <expr> 'tkMore' <add_expr>
        RULE_EXPR_TKLESS = 50, // <expr> ::= <expr> 'tkLess' <add_expr>
        RULE_EXPR_TKMOREEQ = 51, // <expr> ::= <expr> 'tkMoreEq' <add_expr>
        RULE_EXPR_TKLESSEQ = 52, // <expr> ::= <expr> 'tkLessEq' <add_expr>
        RULE_EXPR = 53, // <expr> ::= <add_expr>
        RULE_LIST_TKSQUAREOPEN_TKSQUARECLOSE = 54, // <list> ::= 'tkSquareOpen' <list_elements> 'tkSquareClose'
        RULE_LIST_TKSQUAREOPEN_TKDOT_TKDOT_TKSQUARECLOSE = 55, // <list> ::= 'tkSquareOpen' <list_elements> 'tkDot' 'tkDot' <simple_expr> 'tkSquareClose'
        RULE_LIST_TKSQUAREOPEN_TKDOT_TKDOT_TKSQUARECLOSE2 = 56, // <list> ::= 'tkSquareOpen' <list_elements> 'tkDot' 'tkDot' 'tkSquareClose'
        RULE_LIST = 57, // <list> ::= <list_constructor>
        RULE_LIST_TKCOLON = 58, // <list> ::= <simple_expr> 'tkColon' <empty> <simple_expr>
        RULE_LIST_CONSTRUCTOR_TKSQUAREOPEN_TKSPLIT_TKSQUARECLOSE = 59, // <list_constructor> ::= 'tkSquareOpen' <simple_expr> 'tkSplit' <generators> <conditions_comma> 'tkSquareClose'
        RULE_GENERATORS = 60, // <generators> ::= <generator> <empty>
        RULE_GENERATORS_TKCOMMA = 61, // <generators> ::= <generators> 'tkComma' <generator>
        RULE_GENERATOR_TKARROWGEN = 62, // <generator> ::= <simple_expr> 'tkArrowGen' <simple_expr>
        RULE_CONDITIONS_COMMA = 63, // <conditions_comma> ::= <empty>
        RULE_CONDITIONS_COMMA_TKCOMMA = 64, // <conditions_comma> ::= 'tkComma' <conditions>
        RULE_CONDITIONS = 65, // <conditions> ::= <condition> <empty>
        RULE_CONDITIONS_TKCOMMA = 66, // <conditions> ::= <conditions> 'tkComma' <condition>
        RULE_CONDITION = 67, // <condition> ::= <expr>
        RULE_CORTEG_TKROUNDOPEN_TKCOMMA_TKROUNDCLOSE = 68, // <corteg> ::= 'tkRoundOpen' <simple_expr> 'tkComma' <list_elements> 'tkRoundClose'
        RULE_LIST_ELEMENTS = 69, // <list_elements> ::= <empty>
        RULE_LIST_ELEMENTS2 = 70, // <list_elements> ::= <simple_expr> <empty>
        RULE_LIST_ELEMENTS_TKCOMMA = 71, // <list_elements> ::= <list_elements> 'tkComma' <simple_expr>
        RULE_ADD_EXPR_TKAND = 72, // <add_expr> ::= <add_expr> 'tkAnd' <mult_expr>
        RULE_ADD_EXPR = 73, // <add_expr> ::= <add_expr> <addop> <mult_expr>
        RULE_ADD_EXPR2 = 74, // <add_expr> ::= <mult_expr>
        RULE_MULT_EXPR_TKOR = 75, // <mult_expr> ::= <mult_expr> 'tkOr' <negate_expr>
        RULE_MULT_EXPR = 76, // <mult_expr> ::= <mult_expr> <multop> <negate_expr>
        RULE_MULT_EXPR2 = 77, // <mult_expr> ::= <negate_expr>
        RULE_NEGATE_EXPR_TKMINUS = 78, // <negate_expr> ::= 'tkMinus' <simple_expr>
        RULE_NEGATE_EXPR_TKNOT = 79, // <negate_expr> ::= 'tkNot' <simple_expr>
        RULE_NEGATE_EXPR = 80, // <negate_expr> ::= <simple_expr>
        RULE_SIMPLE_EXPR = 81, // <simple_expr> ::= <simple_type_expr>
        RULE_SIMPLE_EXPR_TKROUNDOPEN_TKROUNDCLOSE = 82, // <simple_expr> ::= 'tkRoundOpen' <expr> 'tkRoundClose'
        RULE_SIMPLE_EXPR2 = 83, // <simple_expr> ::= <infix_expr> <empty>
        RULE_SIMPLE_EXPR3 = 84, // <simple_expr> ::= <variable>
        RULE_SIMPLE_EXPR_TKLET_TKIN = 85, // <simple_expr> ::= 'tkLet' <def_vars> 'tkIn' <body_func>
        RULE_SIMPLE_EXPR4 = 86, // <simple_expr> ::= <list>
        RULE_SIMPLE_EXPR5 = 87, // <simple_expr> ::= <corteg>
        RULE_SIMPLE_EXPR6 = 88, // <simple_expr> ::= <lambda_func> <empty>
        RULE_SIMPLE_EXPR_TKROUNDOPEN_TKROUNDOPEN_TKROUNDCLOSE_TKROUNDCLOSE = 89, // <simple_expr> ::= 'tkRoundOpen' 'tkRoundOpen' <lambda_func> 'tkRoundClose' <params_value> 'tkRoundClose'
        RULE_SIMPLE_TYPE_EXPR_TKINT = 90, // <simple_type_expr> ::= 'tkInt'
        RULE_SIMPLE_TYPE_EXPR_TKDOUBLE = 91, // <simple_type_expr> ::= 'tkDouble'
        RULE_SIMPLE_TYPE_EXPR_TKBOOL = 92, // <simple_type_expr> ::= 'tkBool'
        RULE_SIMPLE_TYPE_EXPR_TKCHAR = 93, // <simple_type_expr> ::= 'tkChar'
        RULE_SIMPLE_TYPE_EXPR_TKSTRING = 94, // <simple_type_expr> ::= 'tkString'
        RULE_VARIABLE_EXPR = 95, // <variable_expr> ::= <simple_expr>
        RULE_DEF_VARS = 96, // <def_vars> ::= <def_var> <empty>
        RULE_DEF_VARS_TKSEMICOLON = 97, // <def_vars> ::= <def_vars> 'tkSemiColon' <def_var>
        RULE_DEF_VAR_TKIDENT = 98, // <def_var> ::= 'tkIdent' <params> <guard_body_list> <where_var>
        RULE_GUARD_BODY_LIST = 99, // <guard_body_list> ::= <guard_body> <empty>
        RULE_GUARD_BODY_LIST2 = 100, // <guard_body_list> ::= <guard_body_list> <guard_body>
        RULE_GUARD_BODY_TKASSIGN = 101, // <guard_body> ::= <guard> 'tkAssign' <body_func>
        RULE_VARIABLE_TKIDENT = 102, // <variable> ::= 'tkIdent'
        RULE_VARIABLE_TKROUNDOPEN_TKIDENT_TKROUNDCLOSE = 103, // <variable> ::= 'tkRoundOpen' 'tkIdent' <params_value> 'tkRoundClose'
        RULE_VARIABLE_TKROUNDOPEN_TKROUNDCLOSE = 104, // <variable> ::= 'tkRoundOpen' <variable> <params_value> 'tkRoundClose'
        RULE_VARIABLE_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE = 105, // <variable> ::= 'tkRoundOpen' 'tkQuote' 'tkIdent' 'tkQuote' <params_value> 'tkRoundClose'
        RULE_INFIX_EXPR_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE = 106, // <infix_expr> ::= 'tkRoundOpen' <simple_expr> 'tkQuote' 'tkIdent' 'tkQuote' <simple_expr> 'tkRoundClose'
        RULE_INFIX_EXPR_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE2 = 107, // <infix_expr> ::= 'tkRoundOpen' <simple_expr> 'tkQuote' 'tkIdent' 'tkQuote' 'tkRoundClose'
        RULE_INFIX_EXPR_TKROUNDOPEN_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE_TKROUNDCLOSE = 108, // <infix_expr> ::= 'tkRoundOpen' 'tkRoundOpen' <simple_expr> 'tkQuote' 'tkIdent' 'tkQuote' 'tkRoundClose' <params_value> 'tkRoundClose'
        RULE_MULTOP_TKSTAR = 109, // <multop> ::= 'tkStar'
        RULE_MULTOP_TKSLASH = 110, // <multop> ::= 'tkSlash'
        RULE_ADDOP_TKPLUS = 111, // <addop> ::= 'tkPlus'
        RULE_ADDOP_TKMINUS = 112, // <addop> ::= 'tkMinus'
        RULE_STMT_TKIDENT_TKARROWGEN_TKIDENT = 113, // <stmt> ::= 'tkIdent' 'tkArrowGen' 'tkIdent'
        RULE_STMT_TKIF_TKTHEN_TKELSE = 114, // <stmt> ::= 'tkIf' <expr> 'tkThen' <body_func> 'tkElse' <body_func>
        RULE_STMT_TKCASE_TKROUNDOPEN_TKROUNDCLOSE_TKOF = 115, // <stmt> ::= 'tkCase' 'tkRoundOpen' <params1> 'tkRoundClose' 'tkOf' <case_variants>
        RULE_STMT_TKDO_TKFIGUREOPEN_TKSEMICOLON_TKFIGURECLOSE = 116, // <stmt> ::= 'tkDo' 'tkFigureOpen' <stmts> 'tkSemiColon' 'tkFigureClose'
        RULE_STMT_TKDO = 117, // <stmt> ::= 'tkDo' <stmts1>
        RULE_STMT_TKRETURN = 118, // <stmt> ::= 'tkReturn' <expr>
        RULE_STMT = 119, // <stmt> ::= <func_call>
        RULE_CASE_VARIANTS = 120, // <case_variants> ::= <case_variant> <empty>
        RULE_CASE_VARIANTS_TKSPLIT = 121, // <case_variants> ::= <case_variants> 'tkSplit' <case_variant>
        RULE_CASE_VARIANT_TKROUNDOPEN_TKROUNDCLOSE_TKARROW = 122, // <case_variant> ::= 'tkRoundOpen' <params1> 'tkRoundClose' 'tkArrow' <body_func>
        RULE_PARAMS1 = 123, // <params1> ::= <param> <empty>
        RULE_PARAMS1_TKCOMMA = 124, // <params1> ::= <params1> 'tkComma' <param>
        RULE_FUNC_CALL = 125, // <func_call> ::= <expr> <empty>
        RULE_PARAMS_VALUE = 126, // <params_value> ::= <param_value> <empty>
        RULE_PARAMS_VALUE2 = 127, // <params_value> ::= <params_value> <param_value>
        RULE_PARAM_VALUE = 128, // <param_value> ::= <expr> <empty>
        RULE_LAMBDA_FUNC_TKLEFTSLASH_TKARROW = 129, // <lambda_func> ::= 'tkLeftSlash' <params> 'tkArrow' <body_func>
        RULE_EMPTY = 130  // <empty> ::= 
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
    case (int)SymbolConstants.SYMBOL_TKAND :
    	//'tkAnd'

		{
			op_type_node _op_type_node=new op_type_node(Operators.LogicalAND);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKARROW :
    	//'tkArrow'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

    case (int)SymbolConstants.SYMBOL_TKARROWGEN:
        //'tkArrowGen'
        {
            token_info _token_info = new token_info(LRParser.TokenText);
            _token_info.source_context = parsertools.GetTokenSourceContext();

            return _token_info;
        }

	case (int)SymbolConstants.SYMBOL_TKASSIGN :
    	//'tkAssign'

		{
            if (token_where == 2 || token_where == 1)
            {
                token_where = 0;
                lambda_stack.Add(new ArrayList());
                //let_stack.Add(new ArrayList());
                let_where_funcs_main.Add(let_where_funcs.Clone());
                let_where_funcs.Clear();
            }
            else
                if (token_let == 1)
                    lambda_stack.Add(new ArrayList());
            if (token_let == 1)
                let_stack.Add(new ArrayList());
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKBOOL :
    	//'tkBool'
{
							ident _ident = null;
            					string s = LRParser.TokenText;
            					if (s == "True" || s == "true")
                						s = "true";
                					else 
								s = "false";
           						_ident =new ident(s);
            					_ident.source_context = parsertools.GetTokenSourceContext();
            					return _ident;
    							}
    case (int)SymbolConstants.SYMBOL_TKBOTTOMMINUS:
    //'tkBottomMinus'
    {
        token_info _token_info = new token_info(LRParser.TokenText);
        _token_info.source_context = parsertools.GetTokenSourceContext();

        return _token_info;
    }
	case (int)SymbolConstants.SYMBOL_TKCASE :
    	//'tkCase'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKCHAR :
    	//'tkChar'
return parsertools.create_char_const(this);
	case (int)SymbolConstants.SYMBOL_TKCOLON :
    	//'tkColon'

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

	case (int)SymbolConstants.SYMBOL_TKDOUBLE :
    	//'tkDouble'
return parsertools.create_double_const(this);
	case (int)SymbolConstants.SYMBOL_TKELSE :
    	//'tkElse'

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

	case (int)SymbolConstants.SYMBOL_TKFIGURECLOSE :
    	//'tkFigureClose'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKFIGUREOPEN :
    	//'tkFigureOpen'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}
    /*case (int)SymbolConstants.SYMBOL_TKGETCHAR:
        //'tkGetChar'
        {
            token_info _token_info = new token_info(LRParser.TokenText);
            _token_info.source_context = parsertools.GetTokenSourceContext();

            return _token_info;
        }*/

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

	case (int)SymbolConstants.SYMBOL_TKIMPORT :
    	//'tkImport'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKIN :
    	//'tkIn'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKINT :
    	//'tkInt'
return parsertools.create_int_const(this);
	case (int)SymbolConstants.SYMBOL_TKLEFTSLASH :
    	//'tkLeftSlash'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();

            lambda_stack.Add(new ArrayList());
            let_stack.Add(new ArrayList());
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKLESS :
    	//'tkLess'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Less);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKLESSEQ :
    	//'tkLessEq'

		{
			op_type_node _op_type_node=new op_type_node(Operators.LessEqual);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKLET :
    	//'tkLet'

		{
            token_let = 1;
            lambda_stack.Add(new ArrayList());
            //let_where_list_params.Clear();
            /////
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKMAINIDENT :
    	//'tkMainIdent'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKMAINIDENT1 :
    	//'tkMainIdent1'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKMINUS :
    	//'tkMinus'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Minus);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKMODULE :
    	//'tkModule'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKMORE :
    	//'tkMore'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Greater);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKMOREEQ :
    	//'tkMoreEq'

		{
			op_type_node _op_type_node=new op_type_node(Operators.GreaterEqual);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKNOT :
    	//'tkNot'

		{
			op_type_node _op_type_node=new op_type_node(Operators.LogicalNOT);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

    case (int)SymbolConstants.SYMBOL_TKNOTEQUAL:
        //'tkNotEqual'
        {
            op_type_node _op_type_node = new op_type_node(Operators.NotEqual);
            _op_type_node.source_context = parsertools.GetTokenSourceContext();

            return _op_type_node;
        }

	case (int)SymbolConstants.SYMBOL_TKOF :
    	//'tkOf'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKOR :
    	//'tkOr'

		{
			op_type_node _op_type_node=new op_type_node(Operators.LogicalOR);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

    case (int)SymbolConstants.SYMBOL_TKOTHERWISE:
        //'tkOtherwise'
        {
            token_info _token_info = new token_info(LRParser.TokenText);
            _token_info.source_context = parsertools.GetTokenSourceContext();

            return _token_info;
        }

	case (int)SymbolConstants.SYMBOL_TKPLUS :
    	//'tkPlus'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Plus);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	/*case (int)SymbolConstants.SYMBOL_TKPRINT :
    	//'tkPrint'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}*/

    case (int)SymbolConstants.SYMBOL_TKQUOTE:
        //'tkQuote'
        {
            token_info _token_info = new token_info(LRParser.TokenText);
            _token_info.source_context = parsertools.GetTokenSourceContext();

            return _token_info;
        }

	case (int)SymbolConstants.SYMBOL_TKREF :
    	//'tkRef'
return parsertools.create_directive_name(this);
	case (int)SymbolConstants.SYMBOL_TKRETURN :
    	//'tkReturn'

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

	case (int)SymbolConstants.SYMBOL_TKSPLIT :
    	//'tkSplit'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKSQUARECLOSE :
    	//'tkSquareClose'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKSQUAREOPEN :
    	//'tkSquareOpen'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKSTAR :
    	//'tkStar'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Multiplication);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKSTRING :
    	//'tkString'
return parsertools.create_string_const(this);
	case (int)SymbolConstants.SYMBOL_TKTHEN :
    	//'tkThen'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKWHERE :
    	//'tkWhere'

		{
            //let_where_list_params.Clear();
            token_where = 1;
            token_where_count++;
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_ADD_EXPR :
    	//<add_expr>
	//TERMINAL:add_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ADDOP :
    	//<addop>
	//TERMINAL:addop
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_BODY :
    	//<body>
	//TERMINAL:body
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_BODY_FUNC :
    	//<body_func>
	//TERMINAL:body_func
	return null;
	//ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_BODY_WHERE:
    //<body_where>
    //TERMINAL:body_where
    return null;
    //ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CASE_VARIANT :
    	//<case_variant>
	//TERMINAL:case_variant
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CASE_VARIANTS :
    	//<case_variants>
	//TERMINAL:case_variants
	return null;
	//ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_CONDITION:
    //<condition>
    //TERMINAL:condition
    return null;
    //ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_CONDITIONS:
    //<conditions>
    //TERMINAL:conditions
    return null;
    //ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_CONDITIONS_COMMA:
    //<conditions_comma>
    //TERMINAL:conditions_comma
    return null;
    //ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_CORTEG:
    //<corteg>
    //TERMINAL:corteg
    return null;
    //ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_DEF_VAR :
    	//<def_var>
	//TERMINAL:def_var
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_DEF_VARS :
    	//<def_vars>
	//TERMINAL:def_vars
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EMPTY :
    	//<empty>
	//TERMINAL:empty
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXPR :
    	//<expr>
	//TERMINAL:expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FUNC_CALL :
    	//<func_call>
	//TERMINAL:func_call
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FUNCS :
    	//<funcs>
	//TERMINAL:funcs
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FUNCS_VARIANTS :
    	//<funcs_variants>
	//TERMINAL:funcs_variants
	return null;
	//ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_GENERATOR:
    //<generator>
    //TERMINAL:generator
    return null;
    //ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_GENERATORS:
    //<generators>
    //TERMINAL:generators
    return null;
    //ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_GUARD :
    	//<guard>
	//TERMINAL:guard
	return null;
	//ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_GUARD_BODY:
    //<guard_body>
    //TERMINAL:guard_body
    return null;
    //ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_GUARD_BODY_LIST:
    //<guard_body_list>
    //TERMINAL:guard_body_list
    return null;
    //ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_IMPORT :
    	//<import>
	//TERMINAL:import
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_IMPORTS :
    	//<imports>
	//TERMINAL:imports
	return null;
	//ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_INFIX_EXPR:
    //<infix_expr>
    //TERMINAL:infix_expr
    return null;
    //ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_INIT:
    	//<init>
	//TERMINAL:init
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_INITS :
    	//<inits>
	//TERMINAL:inits
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LAMBDA_FUNC :
    	//<lambda_func>
	//TERMINAL:lambda_func
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LIST :
    	//<list>
	//TERMINAL:list
	return null;
	//ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_LIST_CONSTRUCTOR:
    //<list_constructor>
    //TERMINAL:list_constructor
    return null;
    //ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LIST_ELEMENTS :
    	//<list_elements>
	//TERMINAL:list_elements
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LIST_PARAM :
    	//<list_param>
	//TERMINAL:list_param
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_MAIN_FUNC :
    	//<main_func>
	//TERMINAL:main_func
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_MODULE :
    	//<module>
	//TERMINAL:module
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_MULT_EXPR :
    	//<mult_expr>
	//TERMINAL:mult_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_MULTOP :
    	//<multop>
	//TERMINAL:multop
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NEGATE_EXPR :
    	//<negate_expr>
	//TERMINAL:negate_expr
	return null;
	//ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_PARAM :
    	//<param>
	//TERMINAL:param
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PARAM_VALUE :
    	//<param_value>
	//TERMINAL:param_value
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PARAMS :
    	//<params>
	//TERMINAL:params
	return null;
	//ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_PARAMS1:
    //<params>
    //TERMINAL:params
    return null;
    //ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PARAMS_VALUE :
    	//<params_value>
	//TERMINAL:params_value
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_REFERENCE :
    	//<reference>
	//TERMINAL:reference
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_SIMPLE_EXPR :
    	//<simple_expr>
	//TERMINAL:simple_expr
	return null;
	//ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_SIMPLE_TYPE_EXPR:
    //<simple_type_expr>
    //TERMINAL:simple_type_expr
    return null;
    //ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_STMT :
    	//<stmt>
	//TERMINAL:stmt
	return null;
	//ENDTERMINAL
    case (int)SymbolConstants.SYMBOL_STMTS:
    //<stmt>
    //TERMINAL:stmt
    return null;
    //ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_STMTS1 :
    	//<stmts>
	//TERMINAL:stmts
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VARIABLE :
    	//<variable>
	//TERMINAL:variable
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VARIABLE_EXPR :
    	//<variable_expr>
	//TERMINAL:variable_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VARIANT :
    	//<variant>
	//TERMINAL:variant
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VARIANTS :
    	//<variants>
	//TERMINAL:variants
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_WHERE_VAR :
    	//<where_var>
	//TERMINAL:where_var
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
public ArrayList param_value_list = new ArrayList();
public ArrayList body_variant_list = new ArrayList();
public ArrayList param_value_list_main = new ArrayList();
public ArrayList body_variant_list_main = new ArrayList();
public ArrayList guard_list = new ArrayList();
public ArrayList guard_list_main = new ArrayList();
public ArrayList func_name = new ArrayList();
public int decls_counts = 0;
public bool where_flag = false;
public ArrayList list_param = new ArrayList();
public ArrayList list_params = new ArrayList();
public ArrayList list_params_temp = new ArrayList();
public ArrayList list_params_main = new ArrayList();
public ArrayList _function_lambda_definitions = new ArrayList();
public ArrayList _function_lambda_definitions_after = new ArrayList();
public int lambda_num = 0;
public ArrayList _functions = new ArrayList();
public ArrayList list_method_calls = new ArrayList();
public ArrayList list_method_calls_main = new ArrayList();
public ArrayList list_return_funcs = new ArrayList();
public ArrayList list_return_funcs_main = new ArrayList();
public ArrayList list_params1 = new ArrayList();
public ArrayList last_function_lambda_definitions = new ArrayList();
public ArrayList last_list_method_calls = new ArrayList();
public ArrayList last_list_method_calls_lambda = new ArrayList();
public ArrayList let_fact_params = new ArrayList();
public ArrayList let_funcs = new ArrayList();
public ArrayList let_funcs_funcs = new ArrayList();
public ArrayList let_func_last = new ArrayList();
public ArrayList let_funcs1 = new ArrayList();
public ArrayList let_flag = new ArrayList();
public ArrayList where_funcs = new ArrayList();
public ArrayList let_where_funcs = new ArrayList();
public ArrayList let_where_funcs_main = new ArrayList();
public ArrayList let_where_list_params = new ArrayList();
int token_where = 0;
int token_where_count = 0;
public ArrayList last_where_funcs = new ArrayList();
public struct types_param_lampda
{
    public string name;
    public ArrayList types;
}
public ArrayList list_method_calls_lambda = new ArrayList();
/////////////////////////////
public ArrayList lambda_funcs = new ArrayList();
public ArrayList lambda_funcs_funcs = new ArrayList();
/////////////////////////////
public ArrayList _function_lambda_definitions_main = new ArrayList();
int token_let = 0;
public ArrayList let_stack = new ArrayList();
public ArrayList lambda_stack = new ArrayList();
int token_where1 = 0;
int bottom_num = 0;
int rec_num = 0;

public Object CreateNonTerminalObject(int ReductionRuleIndex)
{
switch (ReductionRuleIndex)
{
    case (int)RuleConstants.RULE_MODULE_TKMODULE_TKMAINIDENT_TKWHERE :
	//<module> ::= 'tkModule' 'tkMainIdent' 'tkWhere' <reference> <imports> <body> <empty>
{
ident_list _ident_list = new ident_list();
    _ident_list.idents.Add(new ident("LibForHaskell"));
    unit_or_namespace _unit_or_namespace = new unit_or_namespace(_ident_list);
    uses_list ul = (uses_list)LRParser.GetReductionSyntaxNode(4);
    if (ul == null)
        ul = new uses_list();
    ul.units.Insert(0, _unit_or_namespace);
    ////////////////
    declarations _defs = new declarations();
    for (int i = 0; i < _function_lambda_definitions.Count; i++)
        _defs.defs.Add((declaration)lambda((function_lambda_definition)_function_lambda_definitions[i]));
    for (int i = 0; i < _functions.Count; i++)
    {
        _defs.defs.Add((declaration)_functions[i]);
        int k = 1;
        while (k < _function_lambda_definitions_after.Count)
        {
            int j = k;
            while (j < _function_lambda_definitions_after.Count && ((procedure_definition)_functions[i]).proc_header.name.meth_name.name != (string)_function_lambda_definitions_after[j])
                j += 2;
            if (j < _function_lambda_definitions_after.Count)
            {
                _defs.defs.Add((declaration)lambda((function_lambda_definition)_function_lambda_definitions_after[j - 1]));
                _function_lambda_definitions_after.RemoveAt(j);
                _function_lambda_definitions_after.RemoveAt(j - 1);
            }
            k = j;
        }
    }
    int kk = 1;
    while (kk < _function_lambda_definitions_after.Count)
    {
        int j = 0;
        while (j < _defs.defs.Count && ((procedure_definition)_defs.defs[j]).proc_header.name.meth_name.name != (string)_function_lambda_definitions_after[kk])
            j++;
        if (j < _defs.defs.Count)
        {
            _defs.defs.Insert(j+1, (declaration)lambda((function_lambda_definition)_function_lambda_definitions_after[kk - 1]));
            _function_lambda_definitions_after.RemoveAt(kk);
            _function_lambda_definitions_after.RemoveAt(kk - 1);
        }
        else
            kk += 2;
    }

    //for (int i = 0; i < _function_lambda_definitions.Count; i++)
        //_defs.defs.Add(lambda((function_lambda_definition)_function_lambda_definitions[i]));
    for (int i = 0; i < let_where_funcs.Count; i++)
        _defs.defs.Add((procedure_definition)let_where_funcs[i]);
    
    block _block = new block(_defs, ((block)LRParser.GetReductionSyntaxNode(5)).program_code);
                                                ///////////////////////////////////////
											    program_module _program_module = new program_module(null, ul, _block, null);

            									_program_module.Language = LanguageId.PascalABCNET;
            									parsertools.create_source_context(_program_module, parsertools.sc_not_null(null, ul, null, LRParser.GetReductionSyntaxNode(5)), LRParser.GetReductionSyntaxNode(5));
                                                
                                                _functions.Clear();
                                                _function_lambda_definitions.Clear();
                                                func_name.Clear();
                                                lambda_num = 0;
                                                list_method_calls.Clear();
                                                list_method_calls_main.Clear();
                                                list_return_funcs.Clear();
                                                list_return_funcs_main.Clear();
                                                list_params1.Clear();
                                                list_method_calls_lambda.Clear();
                                                last_list_method_calls.Clear();
                                                last_list_method_calls_lambda.Clear();
                                                last_function_lambda_definitions.Clear();
                                          let_funcs1.Clear();
                                                let_funcs_funcs.Clear();
                                                let_func_last.Clear();
                                                let_flag.Clear();
                                                token_where_count = 0;
                                                let_where_funcs_main.Clear();
                                                let_where_funcs.Clear();
                                                token_where = 0;
                                                last_where_funcs.Clear();
                                                let_where_list_params.Clear();
                                                _function_lambda_definitions_main.Clear();
                                                let_stack.Clear();
                                                token_let = 0;
                                                lambda_stack.Clear();

            									return _program_module;
										}
	case (int)RuleConstants.RULE_MODULE :
	//<module> ::= <reference> <imports> <body> <empty>
{
	ident_list _ident_list = new ident_list();
    _ident_list.idents.Add(new ident("LibForHaskell"));
    unit_or_namespace _unit_or_namespace = new unit_or_namespace(_ident_list);
    uses_list ul = (uses_list)LRParser.GetReductionSyntaxNode(1);
    if (ul == null)
        ul = new uses_list();
    ul.units.Insert(0, _unit_or_namespace);
    ////////////////
    declarations _defs = new declarations();
    for (int i = 0; i < _function_lambda_definitions.Count; i++)
        _defs.defs.Add((declaration)lambda((function_lambda_definition)_function_lambda_definitions[i]));
    for (int i = 0; i < _functions.Count; i++)
    {
        _defs.defs.Add((declaration)_functions[i]);
        int k = 1;
        while (k < _function_lambda_definitions_after.Count)
        {
            int j = k;
            while (j < _function_lambda_definitions_after.Count && ((procedure_definition)_functions[i]).proc_header.name.meth_name.name != (string)_function_lambda_definitions_after[j])
                j += 2;
            if (j < _function_lambda_definitions_after.Count)
            {
                _defs.defs.Add((declaration)lambda((function_lambda_definition)_function_lambda_definitions_after[j - 1]));
                _function_lambda_definitions_after.RemoveAt(j);
                _function_lambda_definitions_after.RemoveAt(j - 1);
            }
            k = j - 1;
        }
    }
    int kk = 1;
    while (kk < _function_lambda_definitions_after.Count)
    {
        int j = 0;
        while (j < _defs.defs.Count && ((procedure_definition)_defs.defs[j]).proc_header.name.meth_name.name != (string)_function_lambda_definitions_after[kk])
            j++;
        if (j < _defs.defs.Count)
        {
            _defs.defs.Insert(j + 1, (declaration)lambda((function_lambda_definition)_function_lambda_definitions_after[kk - 1]));
            _function_lambda_definitions_after.RemoveAt(kk);
            _function_lambda_definitions_after.RemoveAt(kk - 1);
        }
        else
            kk += 2;
    }
    block _block = new block(_defs, ((block)LRParser.GetReductionSyntaxNode(2)).program_code);
    ///////////////////////////////
											program_module _program_module = new program_module(null, ul, (block)LRParser.GetReductionSyntaxNode(2), null);

            									_program_module.Language = LanguageId.PascalABCNET;
            									parsertools.create_source_context(_program_module, parsertools.sc_not_null(null, ul, null, LRParser.GetReductionSyntaxNode(2)), LRParser.GetReductionSyntaxNode(2));

                                                _functions.Clear();
                                                _function_lambda_definitions.Clear();
                                                func_name.Clear();
                                                lambda_num = 0;
                                                list_method_calls.Clear();
                                                list_method_calls_main.Clear();
                                                list_return_funcs.Clear();
                                                list_return_funcs_main.Clear();
                                                list_params1.Clear();
                                                list_method_calls_lambda.Clear();
                                                last_list_method_calls.Clear();
                                                last_list_method_calls_lambda.Clear();
                                                last_function_lambda_definitions.Clear();
                                                let_funcs1.Clear();
                                                let_funcs_funcs.Clear();
                                                let_func_last.Clear();
                                                let_flag.Clear();
                                                let_where_funcs.Clear();
                                                token_where_count = 0;
                                                let_where_funcs_main.Clear();
                                                let_where_funcs.Clear();
                                                token_where = 0;
                                                last_where_funcs.Clear();
                                                let_where_list_params.Clear();
                                                _function_lambda_definitions_main.Clear();
                                                let_stack.Clear();
                                                lambda_stack.Clear();
                                                token_let = 0;

            									return _program_module;
										}
    case (int)RuleConstants.RULE_MODULE_TKMODULE_TKIDENT_TKWHERE:
//<module> ::= 'tkModule' 'tkIdent' 'tkWhere' <reference> <imports> <funcs> <empty>
{
    ident_list _ident_list = new ident_list();
    _ident_list.idents.Add(new ident("LibForHaskell"));
    unit_or_namespace _unit_or_namespace = new unit_or_namespace(_ident_list);
    uses_list ul = (uses_list)LRParser.GetReductionSyntaxNode(4);
    if (ul == null)
        ul = new uses_list();
    ul.units.Insert(0, _unit_or_namespace);
    ////////////////
    declarations _defs = new declarations();
    for (int i = 0; i < _function_lambda_definitions.Count; i++)
        _defs.defs.Add((declaration)lambda((function_lambda_definition)_function_lambda_definitions[i]));
    for (int i = 0; i < _functions.Count; i++)
    {
        _defs.defs.Add((declaration)_functions[i]);
        int k = 1;
        while (k < _function_lambda_definitions_after.Count)
        {
            int j = k;
            while (j < _function_lambda_definitions_after.Count && ((procedure_definition)_functions[i]).proc_header.name.meth_name.name != (string)_function_lambda_definitions_after[j])
                j += 2;
            if (j < _function_lambda_definitions_after.Count)
            {
                _defs.defs.Add((declaration)lambda((function_lambda_definition)_function_lambda_definitions_after[j - 1]));
                _function_lambda_definitions_after.RemoveAt(j);
                _function_lambda_definitions_after.RemoveAt(j - 1);
            }
            k = j;
        }
    }
    int kk = 1;
    while (kk < _function_lambda_definitions_after.Count)
    {
        int j = 0;
        while (j < _defs.defs.Count && ((procedure_definition)_defs.defs[j]).proc_header.name.meth_name.name != (string)_function_lambda_definitions_after[kk])
            j++;
        if (j < _defs.defs.Count)
        {
            _defs.defs.Insert(j + 1, (declaration)lambda((function_lambda_definition)_function_lambda_definitions_after[kk - 1]));
            _function_lambda_definitions_after.RemoveAt(kk);
            _function_lambda_definitions_after.RemoveAt(kk - 1);
        }
        else
            kk += 2;
    }

    //for (int i = 0; i < _function_lambda_definitions.Count; i++)
    //_defs.defs.Add(lambda((function_lambda_definition)_function_lambda_definitions[i]));
    for (int i = 0; i < let_where_funcs.Count; i++)
        _defs.defs.Add((procedure_definition)let_where_funcs[i]);

    //////////////////////////interface
    interface_node _interface_node = new interface_node();
    _interface_node.uses_modules = ul;
    _interface_node.using_namespaces = null;
    _interface_node.interface_definitions = LRParser.GetReductionSyntaxNode(5) as declarations;
    ///////////////////////////unit_heading
    unit_name _unit_name = new unit_name((ident)LRParser.GetReductionSyntaxNode(1), UnitHeaderKeyword.Unit);
    initfinal_part _initfinal_part=new initfinal_part();
    unit_module _unit_module = new unit_module(_unit_name, _interface_node, null, _initfinal_part.initialization_sect, _initfinal_part.finalization_sect);
    _unit_module.Language = LanguageId.PascalABCNET;

    _functions.Clear();
    _function_lambda_definitions.Clear();
    func_name.Clear();
    lambda_num = 0;
    list_method_calls.Clear();
    list_method_calls_main.Clear();
    list_return_funcs.Clear();
    list_return_funcs_main.Clear();
    list_params1.Clear();
    list_method_calls_lambda.Clear();
    last_list_method_calls.Clear();
    last_list_method_calls_lambda.Clear();
    last_function_lambda_definitions.Clear();
    let_funcs1.Clear();
    let_funcs_funcs.Clear();
    let_func_last.Clear();
    let_flag.Clear();
    token_where_count = 0;
    let_where_funcs_main.Clear();
    let_where_funcs.Clear();
    token_where = 0;
    last_where_funcs.Clear();
    let_where_list_params.Clear();
    _function_lambda_definitions_main.Clear();
    let_stack.Clear();
    token_let = 0;
    lambda_stack.Clear();

    return _unit_module;
}
	case (int)RuleConstants.RULE_REFERENCE :
	//<reference> ::= 
	//NONTERMINAL:<reference> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_REFERENCE_TKREF_TKSTRING :
	//<reference> ::= 'tkRef' 'tkString'
{
            							token_info t1 = new token_info();
            							t1.text = ((ident)LRParser.GetReductionSyntaxNode(0)).name;
            							t1.source_context = ((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
            							token_info t2 = new token_info();
            							t2.text = ((string_const)LRParser.GetReductionSyntaxNode(1)).Value;
            							t2.source_context = ((string_const)LRParser.GetReductionSyntaxNode(1)).source_context;
            							compiler_directive cd = new compiler_directive(t1, t2);
            							parsertools.create_source_context(cd, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
            							CompilerDirectives.Add(cd);
            							return null;
        							}
	case (int)RuleConstants.RULE_IMPORTS :
	//<imports> ::= 
	//NONTERMINAL:<imports> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_IMPORTS2 :
	//<imports> ::= <import> <empty>
{
            							uses_list _uses_list = new uses_list();

            							_uses_list.units.Add((unit_or_namespace)LRParser.GetReductionSyntaxNode(0));

            							return _uses_list;
        							}
    case (int)RuleConstants.RULE_IMPORTS3:
//<imports> ::= <imports> <empty> <import>
{
            							uses_list _uses_list = (uses_list)LRParser.GetReductionSyntaxNode(0);
            							_uses_list.units.Add((unit_or_namespace)LRParser.GetReductionSyntaxNode(2));

            							return _uses_list;
        							}
	case (int)RuleConstants.RULE_IMPORT_TKIMPORT_TKIDENT :
	//<import> ::= 'tkImport' 'tkIdent'
{
            							ident_list _ident_list = new ident_list();
            							_ident_list.source_context = ((ident)LRParser.GetReductionSyntaxNode(1)).source_context;
            							_ident_list.idents.Add((ident)LRParser.GetReductionSyntaxNode(1));

            							unit_or_namespace _unit_or_namespace = new unit_or_namespace(_ident_list);
            							parsertools.create_source_context(_unit_or_namespace,_ident_list, _ident_list);

            							return _unit_or_namespace;
        							}
	case (int)RuleConstants.RULE_BODY :
	//<body> ::= <main_func>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_BODY2 :
	//<body> ::= <funcs> <main_func>

								{
            							token_info _token_info = new token_info(";");

           								_token_info.source_context = parsertools.GetTokenSourceContext();

            							block _block = new block((declarations)LRParser.GetReductionSyntaxNode(0), ((block)LRParser.GetReductionSyntaxNode(1)).program_code);

            							parsertools.create_source_context(_block, parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0), ((block)LRParser.GetReductionSyntaxNode(1)).program_code), _token_info);
            							return _block;
       							}

	case (int)RuleConstants.RULE_FUNCS :
	//<funcs> ::= <funcs_variants> <empty>
{
           								declarations _declarations = new declarations();
                                                      ArrayList funcs = (ArrayList)LRParser.GetReductionSyntaxNode(0);
                                        			for (int i = 0;i < funcs.Count;i++)
                                        			{
                                            			_declarations.defs.Add((declaration)funcs[i]);
                                            			parsertools.create_source_context(_declarations, parsertools.sc_not_null(_declarations, (declaration)funcs[i]), (declaration)funcs[i]);
                                        			}

                                        			param_value_list.Clear();
                                                      param_value_list_main.Clear();
                                        			body_variant_list.Clear();
                                        			body_variant_list_main.Clear();
								      guard_list.Clear();
                                                      guard_list_main.Clear();
							            list_params_main.Clear();
                                                      list_params.Clear();
                                                      list_param.Clear();
                                                      list_params_temp.Clear();
                                                      where_flag = false;
                                                      decls_counts = 0;
                                                      let_where_funcs.Clear();
            						      return _declarations;
        							}
	case (int)RuleConstants.RULE_FUNCS_VARIANTS :
	//<funcs_variants> ::= <variants> <empty>
{
param_value_list_main.Add(param_value_list.Clone());
    body_variant_list_main.Add(body_variant_list.Clone());
    list_method_calls_main.Add(list_method_calls.Clone());
    guard_list_main.Add(guard_list.Clone());
    list_params_main.Add(list_params.Clone());
    list_params.Clear();

    ArrayList funcs = new ArrayList();
    
    for (int k = 0; k < func_name.Count; k++)
    {
        //////////////////////////head

        method_name _method_name = new method_name(null, (ident)func_name[k], null);
        parsertools.create_source_context(_method_name, func_name[k], func_name[k]);

        function_header _function_header = new function_header();

        object rt = new object();
        _function_header.name = _method_name;
        if (_function_header.name.meth_name is template_type_name)
        {
            _function_header.template_args = (_function_header.name.meth_name as template_type_name).template_args;
            ident id = new ident(_function_header.name.meth_name.name);
            parsertools.create_source_context(id, _function_header.name.meth_name, _function_header.name.meth_name);
            _function_header.name.meth_name = id;
        }
        ////////////////////////////////params
        formal_parameters _formal_parametres = new formal_parameters();
        expression_list f = null;
        if (((ArrayList)param_value_list_main[k])[0] != null)
        {
            f = (expression_list)((ArrayList)param_value_list_main[k])[((ArrayList)param_value_list_main[k]).Count - 1];
            string s = "";
            for (int i = 0; i < ((ArrayList)param_value_list_main[k]).Count; i++)
            {
                for (int j = 0; j < ((expression_list)((ArrayList)param_value_list_main[k])[i]).expressions.Count; j++)
                    if (((expression_list)((ArrayList)param_value_list_main[k])[i]).expressions[j] is ident)
                        s += ((ident)((expression_list)((ArrayList)param_value_list_main[k])[i]).expressions[j]).name;
            }
            for (int i = 0; i < ((expression_list)((ArrayList)param_value_list_main[k])[((ArrayList)param_value_list_main[k]).Count - 1]).expressions.Count; i++)
            {
                ident_list _ident_list = new ident_list();
                ident id = new ident(s + i.ToString());
                _ident_list.source_context = id.source_context;
                _ident_list.idents.Add(id);
                named_type_reference _named_type_reference1 = new named_type_reference();

                ident idtype1 = new ident("datatype");
                _named_type_reference1.source_context = idtype1.source_context;
                _named_type_reference1.names.Add(idtype1);

                typed_parameters _typed_parametres = new typed_parameters(_ident_list, (type_definition)_named_type_reference1, parametr_kind.none, null);
                parsertools.create_source_context(_typed_parametres, _ident_list, _named_type_reference1);

                _formal_parametres.params_list.Add(_typed_parametres);
            }
            _function_header.parameters = _formal_parametres;
        }
        //////////////////////////type
        {
            named_type_reference _named_type_reference11 = new named_type_reference();
            ident idtype11 = new ident("datatype");
            _named_type_reference11.source_context = idtype11.source_context;
            _named_type_reference11.names.Add(idtype11);

            rt = _named_type_reference11;
            _function_header.return_type = (type_definition)_named_type_reference11;
        }

        _function_header.of_object = false;
        _function_header.class_keyword = false;
        token_info _token_info = new token_info("function");
        _token_info.source_context = parsertools.GetTokenSourceContext();
        parsertools.create_source_context(_function_header, _token_info, rt);
        
        //////////////////////////////////////block
        statement_list stmt_l = new statement_list();

        statement last_if = null;
        ArrayList vars = new ArrayList();
        if (((ArrayList)param_value_list_main[k])[0] != null)
        {
            bool flag = false;
            for (int i = ((ArrayList)body_variant_list_main[k]).Count - 1; i >= 0; i--)
            {
                expression_list pv = (expression_list)((ArrayList)param_value_list_main[k])[i];
                statement body_part = (statement)((ArrayList)body_variant_list_main[k])[i];
                expression guard = null;
                if (i < ((ArrayList)guard_list_main[k]).Count)
                    guard = (expression)((ArrayList)guard_list_main[k])[i];

                if_node _if_node = new if_node(null, body_part, last_if);
                parsertools.create_source_context(_if_node, null, parsertools.sc_not_null(body_part, last_if));

                ///////////
                named_type_reference _named_type_reference111 = new named_type_reference();

                ident idtype111 = new ident("datatype");
                _named_type_reference111.source_context = idtype111.source_context;
                _named_type_reference111.names.Add(idtype111);

                expression_list el = new expression_list();
                el.expressions.Add(new ident("true") as expression);
                literal lt;
                string text = "boolean";
                lt = new string_const(text);
                el.expressions.Add(lt as expression);
                /////
                named_type_reference ntr = _named_type_reference111;
                new_expr newexpr = new new_expr(ntr, el, false, null);
                parsertools.create_source_context(newexpr, new ident("new"), parsertools.sc_not_null(el, _named_type_reference111));
                ///////////
                expression last_expr = newexpr as expression;
                if (guard != null)
                    last_expr = guard;
                for (int j = 0; j < _function_header.parameters.params_list.Count; j++)
                {
                    typed_parameters tp = (typed_parameters)_function_header.parameters.params_list[j];
                    if (j < pv.expressions.Count && !(pv.expressions[j] is ident))//vstavka if (vyhod iz rekursii)
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.Equal);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();
                        bin_expr _bin_expr = new bin_expr(tp.idents.idents[0] as expression, (expression)pv.expressions[j], _op_type_node.type);
                        parsertools.create_source_context(_bin_expr, tp.idents.idents[0], pv.expressions[j]);

                        op_type_node _op_type_node1 = new op_type_node(Operators.LogicalAND);
                        _op_type_node1.source_context = parsertools.GetTokenSourceContext();
                        bin_expr _bin_expr1 = new bin_expr(_bin_expr, last_expr, _op_type_node1.type);
                        parsertools.create_source_context(_bin_expr1, _bin_expr, last_expr);
                        last_expr = _bin_expr1;
                    }
                }
                _if_node.condition = _ob(last_expr);
                ////novye peremennye
                statement_list _ass = new statement_list();
                for (int j = 0; j < _function_header.parameters.params_list.Count; j++)
                {
                    if (j < pv.expressions.Count && pv.expressions[j] is ident && ((ident)pv.expressions[j]).name != _function_header.parameters.params_list[j].idents.idents[0].name)
                    {
                        ident_list il = new ident_list();
                        il.idents.Add(new ident(((ident)pv.expressions[j]).name));
                        method_call mc = find_method_call(((ident)pv.expressions[j]).name, k);
                        if (mc == null)
                        {
                            named_type_reference _named_type_reference1 = new named_type_reference();
                            ident idtype1 = new ident("datatype");
                            _named_type_reference1.source_context = idtype1.source_context;
                            _named_type_reference1.names.Add(idtype1);

                            var_def_statement _var_def_statement = new var_def_statement(il, (type_definition)_named_type_reference1, null, definition_attribute.None, false);
                            parsertools.create_source_context(_var_def_statement, il, _named_type_reference1);
                            var_statement _var_statement = new var_statement(_var_def_statement);
                            parsertools.create_source_context(_var_statement, null, _var_def_statement);
                            //((statement_list)_if_node.then_body).subnodes.Insert(0, _var_statement);//obyavlenie peremennoy
                            int ii = ((ArrayList)body_variant_list_main[k]).Count - 1;
                            int jj = 0;
                            bool b = false;
                            expression_list exl = (expression_list)((ArrayList)param_value_list_main[k])[ii];
                            while (ii > i && !b)
                            {
                                jj = 0;
                                while (jj < exl.expressions.Count && (!(exl.expressions[jj] is ident) || ((ident)exl.expressions[jj]).name != ((ident)pv.expressions[j]).name))
                                    jj++;
                                if (jj < exl.expressions.Count)
                                    b = true;
                                ii--;
                                if (ii > i)
                                    exl = (expression_list)((ArrayList)param_value_list_main[k])[ii];
                            }
                            if (!b)
                            {
                                int kk = 0;
                                while (kk < vars.Count && il.idents[0].name != ((var_statement)vars[kk]).var_def.vars.idents[0].name)
                                    kk++;
                                if (kk >= vars.Count)
                                    vars.Add(_var_statement);
                            }
                        }
                        else
                        {
                            bool b = false;
                            int ii = ((ArrayList)body_variant_list_main[k]).Count - 1;
                            int jj = 0;
                            expression_list exl = (expression_list)((ArrayList)param_value_list_main[k])[ii];
                            while (ii > i && !b)
                            {
                                jj = 0;
                                while (jj < exl.expressions.Count && (!(exl.expressions[jj] is ident) || ((ident)exl.expressions[jj]).name != ((ident)pv.expressions[j]).name))
                                    jj++;
                                if (jj < exl.expressions.Count)
                                    b = true;
                                ii--;
                                if (ii > i)
                                    exl = (expression_list)((ArrayList)param_value_list_main[k])[ii];
                            }
                            if (!b)
                            {
                                _function_header.parameters.params_list[j].vars_type = func_type(mc.parameters.expressions.Count);
                                stmt_l.subnodes.Add(var_st(((ident)pv.expressions[j]).name, func_type(mc.parameters.expressions.Count)));
                            }
                        }

                        op_type_node _op_type_node2 = new op_type_node(Operators.Assignment);
                        _op_type_node2.source_context = parsertools.GetTokenSourceContext();
                        assign _assign = new assign(il.idents[0] as addressed_value, _function_header.parameters.params_list[j].idents.idents[0] as expression, _op_type_node2.type);
                        parsertools.create_source_context(_assign, il.idents[0], _function_header.parameters.params_list[j].idents.idents[0]);
                        _ass.subnodes.Add(_assign);
                    }
                }

                ///////////////////////////////////////list_param
                if (((ArrayList)list_params_main[k]).Count != 0 && i < ((ArrayList)list_params_main[k]).Count)
                {
                    for (int ll = 0; ll < ((ArrayList)((ArrayList)list_params_main[k])[i]).Count; ll++)
                    {
                        //if (i == ((ArrayList)body_variant_list_main[k]).Count - 1)
                        for (int l = 0; l < ((ArrayList)((ArrayList)((ArrayList)list_params_main[k])[i])[ll]).Count; l++)
                        {
                            ident_list il1 = new ident_list();
                            il1.idents.Add(new ident(((ident)((ArrayList)((ArrayList)((ArrayList)list_params_main[k])[i])[ll])[l]).name));
                            named_type_reference _named_type_reference1 = new named_type_reference();
                            ident idtype1 = new ident("datatype");
                            _named_type_reference1.names.Add(idtype1);

                            var_def_statement _var_def_statement = new var_def_statement(il1, (type_definition)_named_type_reference1, null, definition_attribute.None, false);
                            parsertools.create_source_context(_var_def_statement, il1, _named_type_reference1);
                            var_statement _var_statement = new var_statement(_var_def_statement);
                            //stmt_l.subnodes.Add(_var_statement);
                            int j = 0;
                            while (j < vars.Count && il1.idents[0].name != ((var_statement)vars[j]).var_def.vars.idents[0].name)
                                j++;
                            if (j >= vars.Count)
                                vars.Add(_var_statement);
                        }
                        op_type_node _op_type_node = new op_type_node(Operators.Assignment);
                        dot_node _dot_node = new dot_node(null, (addressed_value)(new ident("head")));
                        ident id = new ident();
                        for (int i1 = 0; i1 < ((ArrayList)((ArrayList)((ArrayList)list_params_main[k])[i])[ll]).Count; i1++)
                            id.name += ((ident)((ArrayList)((ArrayList)((ArrayList)list_params_main[k])[i])[ll])[i1]).name;
                        if (id.name != null)
                        {
                            _dot_node.left = (addressed_value)(id);
                            object o = null;
                            method_call _method_call = new method_call(o as expression_list);
                            if (_method_call is dereference)
                                ((dereference)_method_call).dereferencing_value = (addressed_value)_dot_node;
                            assign _assign1 = new assign(((ident)((ArrayList)((ArrayList)((ArrayList)list_params_main[k])[i])[ll])[((ArrayList)((ArrayList)((ArrayList)list_params_main[k])[i])[ll]).Count - 1]) as addressed_value, _method_call as expression, _op_type_node.type);
                            _ass.subnodes.Add(_assign1);

                            ///
                            for (int i1 = ((ArrayList)((ArrayList)((ArrayList)list_params_main[k])[i])[ll]).Count - 2; i1 > 0; i1--)
                            {
                                _dot_node = new dot_node((addressed_value)id, (addressed_value)(new ident("tail")));
                                for (int j1 = 0; j1 < ((ArrayList)((ArrayList)((ArrayList)list_params_main[k])[i])[ll]).Count - 2 - i1; j1++)
                                {
                                    _dot_node = new dot_node(_dot_node, (addressed_value)(new ident("tail")));
                                }
                                _dot_node = new dot_node(_dot_node, (addressed_value)(new ident("head")));

                                _method_call = new method_call(o as expression_list);
                                if (_method_call is dereference)
                                    ((dereference)_method_call).dereferencing_value = (addressed_value)_dot_node;
                                _assign1 = new assign(((ident)((ArrayList)((ArrayList)((ArrayList)list_params_main[k])[i])[ll])[i1]) as addressed_value, _method_call as expression, _op_type_node.type);
                                _ass.subnodes.Add(_assign1);
                            }

                            _dot_node = new dot_node((addressed_value)id, (addressed_value)(new ident("tail")));

                            for (int j1 = 0; j1 < ((ArrayList)((ArrayList)((ArrayList)list_params_main[k])[i])[ll]).Count - 2; j1++)
                            {
                                _dot_node = new dot_node(_dot_node, (addressed_value)(new ident("tail")));
                            }
                            _method_call = new method_call(o as expression_list);
                            if (_method_call is dereference)
                                ((dereference)_method_call).dereferencing_value = (addressed_value)_dot_node;
                            _assign1 = new assign(((ident)((ArrayList)((ArrayList)((ArrayList)list_params_main[k])[i])[ll])[0]) as addressed_value, _method_call as expression, _op_type_node.type);
                            _ass.subnodes.Add(_assign1);
                        }
                    }
                }
                ///////////////////////////////////////////////////////////////////////
                parsertools.create_source_context(_if_node, null, _if_node);
                last_if = new statement_list();
                for (int ii = 0; ii < _ass.subnodes.Count; ii++)
                    ((statement_list)last_if).subnodes.Add((assign)_ass.subnodes[ii]);

                ((statement_list)last_if).subnodes.Add(_if_node);
            }
            stmt_l.subnodes.Add(last_if);
        }
        else
        {
            stmt_l.subnodes.Add((statement)((ArrayList)body_variant_list_main[k])[0]);
        }

        //////////////////

        block _block = new block(null, null);
        _block.defs = new declarations();
        for (int l = 0; l < vars.Count; l++)
            _block.defs.defs.Add(vars[l] as var_statement);

        statement_list sl = null;
        if (stmt_l is statement_list)
            sl = stmt_l as statement_list;
        else
        {
            sl = new statement_list();
            sl.subnodes.Add(stmt_l as statement);
            if (!(stmt_l is empty_statement))
                parsertools.assign_source_context(sl, stmt_l);
        }
        _block.program_code = sl;
        //////////////////
        ArrayList lamdas = find_lambda_funcs_main(_function_header.name.meth_name.name);
        for (int l = 0; l < lamdas.Count; l++)
            _block.defs.defs.Add(lambda((function_lambda_definition)lamdas[l]));
        //////////////////
        int r = 0;
        while (r < let_funcs_funcs.Count && ((string)((ArrayList)let_funcs_funcs[r])[0]) != _function_header.name.meth_name.name)
            r++;
        if (r < let_funcs_funcs.Count)
        {
            if (_block.defs == null)
                _block.defs = new declarations();
            for (int l = 0; l < ((ArrayList)((ArrayList)let_funcs_funcs[r])[1]).Count; l++)
                _block.defs.defs.Add(((ArrayList)((ArrayList)let_funcs_funcs[r])[1])[l] as procedure_definition);
        }
        let_funcs.Clear();
        //////////////////
        procedure_definition _procedure_definition = new procedure_definition(_function_header, null);
        rt = _function_header;
        if (_block != null)
        {
            rt = _block;
            if (_block is proc_block)
                _procedure_definition.proc_body = (proc_block)_block;
        }
        parsertools.create_source_context(_procedure_definition, _function_header, rt);
        funcs.Add(_procedure_definition);
        _functions.Add(_procedure_definition);
    }
    return funcs;}
	case (int)RuleConstants.RULE_VARIANTS :
	//<variants> ::= <variant> <empty>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VARIANTS2 :
	//<variants> ::= <variants> <variant>
return LRParser.GetReductionSyntaxNode(0);
    case (int)RuleConstants.RULE_VARIANT_TKIDENT:
//<variant> ::= 'tkIdent' <params> <guard_body_list> <where_var>
{
    ArrayList body_list = (ArrayList)LRParser.GetReductionSyntaxNode(2);
    for (int g_ind = 0; g_ind < body_list.Count; g_ind++)
    {
        ///////////////////////////////////body
        statement_list st = null;

        statement_list body_1 = (statement_list)((ArrayList)body_list[g_ind])[1];

        if (LRParser.GetReductionSyntaxNode(3) != null)
        {
            ArrayList ar = (ArrayList)LRParser.GetReductionSyntaxNode(3);
            for (int ii = 0; ii < ar.Count; ii++)
                where_funcs.Add(ar[ii]);

            statement_list _statement_list = body_1;
            //////////////////////////////////////////////for guard
            formal_parameters _formal_parametres = null;
            //////////////////////////
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);
            /////////////////////////////
            lambda_num++;
            function_lambda_definition _procedure_definition = new function_lambda_definition();
            _procedure_definition.formal_parameters = _formal_parametres;
            _procedure_definition.return_type = (type_definition)_named_type_reference1;
            _procedure_definition.ident_list = null;
            _procedure_definition.proc_body = null;
            _procedure_definition.parameters = null;
            _procedure_definition.lambda_name = "__lam_where__" + lambda_num;
            //new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, null, null, null, "lambda_where" + lambda_num);
            _procedure_definition.proc_body = _statement_list;
            procedure_definition pr = lambda(_procedure_definition);
            //_function_lambda_definitions.Add(_procedure_definition);////////////////
            ((block)pr.proc_body).defs = new declarations();
            int start = 0;
            int k = where_funcs.Count - 1;
            while (k > 0 && ((procedure_definition)where_funcs[k]).proc_header.name.meth_name.name.Contains("__lambda__"))
                k--;
            int kk = 0;
            while (k > 0 && kk < ar.Count - 1 && !((procedure_definition)where_funcs[k]).proc_header.name.meth_name.name.Contains("__lambda__"))
            {
                k--;
                kk++;
            }
            start = k;
            int i = start;
            int n = where_funcs.Count;
            if (start >= 0)
                while (i < n)
                {
                    ((block)pr.proc_body).defs.defs.Add((procedure_definition)where_funcs[start]);
                    where_funcs.RemoveAt(start);
                    i++;
                }
            /////////////////////////////////lambda
            if (lambda_stack.Count > 0 && ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count == 0)
                lambda_stack.RemoveAt(lambda_stack.Count - 1);
            if (lambda_stack.Count > 0)
            {
                if (((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count > 0)
                {
                    if (((block)pr.proc_body).defs == null)
                        ((block)pr.proc_body).defs = new declarations();
                    for (int ii = 0; ii < ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count; ii++)
                        ((block)pr.proc_body).defs.defs.Add((procedure_definition)((ArrayList)lambda_stack[lambda_stack.Count - 1])[ii]);
                    for (int ii = 0; ii < ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count; ii++)
                        _function_lambda_definitions.RemoveAt(_function_lambda_definitions.Count - 1);
                }
                //lambda_stack.RemoveAt(lambda_stack.Count - 1);
            }
            //////////////////////////////////
            where_funcs.Add(pr);
            //if (let_flag.Count > 0)
            //let_func_last.Add(pr);
            expression_list el = new expression_list();
            //where_fact_params.Clear();
            op_type_node _op_type_node = new op_type_node(Operators.Assignment);
            assign _assign = new assign((addressed_value)new ident("result"), new ident(_procedure_definition.lambda_name), Operators.Assignment);

            statement_list _statement_list1 = new statement_list();
            _statement_list1.subnodes.Add(_assign);
            st = _statement_list1;
        }
        else
        {
            //////////////////////////////////////////////for guard
            st = body_1;
        }
        ///////////////////////////////////\body


        ////////////////////////////////////////////////////for let & where
        let_where_funcs.Clear();
        while (let_where_funcs_main.Count > 1)
            let_where_funcs_main.RemoveAt(let_where_funcs_main.Count - 1);
        list_params1.Clear();
        let_flag.Clear();
        int iiii = 0;
        while (iiii < func_name.Count && ((ident)func_name[iiii]).name != ((ident)LRParser.GetReductionSyntaxNode(0)).name)
            iiii++;
        if (iiii == func_name.Count && let_funcs_funcs.Count == 0 || (let_funcs_funcs.Count > 0 && ((string)((ArrayList)let_funcs_funcs[let_funcs_funcs.Count - 1])[0]) != ((ident)LRParser.GetReductionSyntaxNode(0)).name))
        {
            ArrayList ar = new ArrayList();
            ar.Add(((ident)LRParser.GetReductionSyntaxNode(0)).name);
            if (where_funcs.Count > 0)
            {
                for (int i = 0; i < let_funcs.Count; i++)
                    ((block)((procedure_definition)where_funcs[0]).proc_body).defs.defs.Add((procedure_definition)let_funcs[i]);
                ar.Add(where_funcs.Clone());
            }
            else
                ar.Add(let_funcs.Clone());
            let_funcs_funcs.Add(ar);
            let_funcs.Clear();
            where_funcs.Clear();
            let_func_last.Clear();
        }
        else
        {
            int r = 0;
            while (r < let_funcs_funcs.Count && ((string)((ArrayList)let_funcs_funcs[r])[0]) !=
                ((ident)LRParser.GetReductionSyntaxNode(0)).name)
                r++;
            if (r < let_funcs_funcs.Count)
            {
                if (where_funcs.Count > 0)
                {
                    for (int i = 0; i < let_funcs.Count; i++)
                        ((block)((procedure_definition)where_funcs[0]).proc_body).defs.defs.Add((procedure_definition)let_funcs[i]);
                    for (int i = 0; i < where_funcs.Count; i++)
                        ((ArrayList)((ArrayList)let_funcs_funcs[r])[1]).Add(where_funcs[i]);
                }
                else
                    for (int i = 0; i < let_funcs.Count; i++)
                        ((ArrayList)((ArrayList)let_funcs_funcs[r])[1]).Add(let_funcs[i]);
                let_funcs.Clear();
                where_funcs.Clear();
                let_func_last.Clear();
            }
        }

        if (iiii == func_name.Count)
        {
            /////////////////////////////////////////////////////
            ArrayList ar_lambda = new ArrayList();
            ar_lambda.Add(((ident)LRParser.GetReductionSyntaxNode(0)).name);
            ar_lambda.Add(_function_lambda_definitions.Clone());
            _function_lambda_definitions_main.Add(ar_lambda.Clone());
            _function_lambda_definitions.Clear();
            /////////////////////////////////////////////////////
        }
        else
        {
            //////////////////////////////////
            ArrayList ar_lambda = (ArrayList)_function_lambda_definitions.Clone();
            for (int i = 0; i < ar_lambda.Count; i++)
                ((ArrayList)((ArrayList)_function_lambda_definitions_main[_function_lambda_definitions_main.Count - 1])[1]).Add(ar_lambda[i]);
            _function_lambda_definitions.Clear();
            //////////////////////////////////
        }
        ////////////////////////////////////////////////////

        if (st/* LRParser.GetReductionSyntaxNode(2)*/ != null)
        {
            int i = 0;
            while (i < func_name.Count && ((ident)func_name[i]).name != ((ident)LRParser.GetReductionSyntaxNode(0)).name)
                i++;
            if (i == func_name.Count)
            {
                func_name.Add((ident)LRParser.GetReductionSyntaxNode(0));
                list_return_funcs_main.Add(list_return_funcs.Clone());
                list_return_funcs.Clear();
                if (list_method_calls_main.Count > 0)
                    for (int iii = 0; iii < ((ArrayList)list_method_calls_main[list_method_calls_main.Count - 1]).Count; iii++)
                        if (list_method_calls.Count > 0)
                            list_method_calls.RemoveAt(0);
                list_method_calls_main.Add(list_method_calls.Clone());
                if (param_value_list.Count != 0)
                {
                    param_value_list_main.Add(param_value_list.Clone());
                    body_variant_list_main.Add(body_variant_list.Clone());
                    guard_list_main.Add(guard_list.Clone());
                    list_params_main.Add(list_params.Clone());
                }
                param_value_list.Clear();
                body_variant_list.Clear();
                guard_list.Clear();
                list_params.Clear();
                param_value_list.Add(LRParser.GetReductionSyntaxNode(1));
                ////////////////Dobavlyaem formalnye parametry
                //statement_list st = (statement_list)LRParser.GetReductionSyntaxNode(4);
                if (st.subnodes[0] is assign)
                {
                    if (((assign)st.subnodes[0]).from is function_lambda_call)
                    {
                        function_lambda_call flc = (function_lambda_call)(((assign)st.subnodes[0]).from);
                        function_lambda_definition fld = ((function_lambda_call)(((assign)st.subnodes[0]).from)).f_lambda_def;
                        for (int ind = flc.parameters.expressions.Count; ind < fld.parameters.expressions.Count; ind++)
                        {
                            if (param_value_list.Count == 0)
                                param_value_list.Add(new expression_list());
                            if (param_value_list[param_value_list.Count - 1] == null)
                                param_value_list[param_value_list.Count - 1] = new expression_list();
                            ((expression_list)param_value_list[param_value_list.Count - 1]).expressions.Add(fld.parameters.expressions[ind]);
                        }
                    }
                    if (((assign)st.subnodes[0]).from is method_call)
                    {
                        method_call mc = (method_call)(((assign)st.subnodes[0]).from);
                        int jjj = 0;
                        int ind_null = -1;
                        while (jjj < mc.parameters.expressions.Count)
                            if (mc.parameters.expressions[jjj] is new_expr && ((new_expr)mc.parameters.expressions[jjj]).params_list.expressions[0] is string_const &&
                                ((string_const)((new_expr)mc.parameters.expressions[jjj]).params_list.expressions[0]).Value == "$null")
                            {
                                ind_null = jjj;
                                jjj = mc.parameters.expressions.Count;
                            }
                            else
                                jjj++;
                        if (ind_null >= 0)
                            for (int ind = ind_null; ind < mc.parameters.expressions.Count; ind++)
                            {
                                //int j = 0;
                                //while ()
                                if (param_value_list.Count == 0)
                                    param_value_list.Add(new expression_list());
                                if (param_value_list[param_value_list.Count - 1] == null)
                                    param_value_list[param_value_list.Count - 1] = new expression_list();
                                ((expression_list)param_value_list[param_value_list.Count - 1]).expressions.Add(new ident("$param" + ind));
                                ((method_call)(((assign)st.subnodes[0]).from)).parameters.expressions.Insert(ind, new ident("$param" + ind));
                                ((method_call)(((assign)st.subnodes[0]).from)).parameters.expressions.RemoveAt(ind + 1);
                            }
                    }
                }
                ////////////////
                body_variant_list.Add(st);
                //guard_list.Add(LRParser.GetReductionSyntaxNode(2));
                guard_list.Add(((ArrayList)body_list[g_ind])[0]);
                if (list_params_temp.Count != 0)
                {
                    list_params.Add(list_params_temp.Clone());
                    list_params_temp.Clear();
                }
            }
            else
            {
                ArrayList ar = (ArrayList)list_method_calls.Clone();
                for (int iii = 0; iii < list_method_calls.Count; iii++)
                    ((ArrayList)list_method_calls_main[list_method_calls_main.Count - 1]).Add(ar[iii]);
                param_value_list.Add(LRParser.GetReductionSyntaxNode(1));
                ////////////////Dobavlyaem formalnye parametry
                //statement_list st = (statement_list)LRParser.GetReductionSyntaxNode(4);
                if (st.subnodes[0] is assign)
                {
                    if (((assign)st.subnodes[0]).from is function_lambda_call)
                    {
                        function_lambda_call flc = (function_lambda_call)(((assign)st.subnodes[0]).from);
                        function_lambda_definition fld = ((function_lambda_call)(((assign)st.subnodes[0]).from)).f_lambda_def;
                        for (int ind = flc.parameters.expressions.Count; ind < fld.parameters.expressions.Count; ind++)
                        {
                            if (param_value_list.Count == 0)
                                param_value_list.Add(new expression_list());
                            if (param_value_list[param_value_list.Count - 1] == null)
                                param_value_list[param_value_list.Count - 1] = new expression_list();
                            ((expression_list)param_value_list[param_value_list.Count - 1]).expressions.Add(fld.parameters.expressions[ind]);
                        }
                    }
                    if (((assign)st.subnodes[0]).from is method_call)
                    {
                        method_call mc = (method_call)(((assign)st.subnodes[0]).from);
                        int jjj = 0;
                        int ind_null = -1;
                        while (jjj < mc.parameters.expressions.Count)
                            if (mc.parameters.expressions[jjj] is new_expr && ((new_expr)mc.parameters.expressions[jjj]).params_list.expressions[0] is string_const &&
                                ((string_const)((new_expr)mc.parameters.expressions[jjj]).params_list.expressions[0]).Value == "$null")
                            {
                                ind_null = jjj;
                                jjj = mc.parameters.expressions.Count;
                            }
                            else
                                jjj++;
                        if (ind_null >= 0)
                            for (int ind = ind_null; ind < mc.parameters.expressions.Count; ind++)
                            {
                                if (param_value_list.Count == 0)
                                    param_value_list.Add(new expression_list());
                                if (param_value_list[param_value_list.Count - 1] == null)
                                    param_value_list[param_value_list.Count - 1] = new expression_list();
                                ((expression_list)param_value_list[param_value_list.Count - 1]).expressions.Add(new ident("$param" + ind));
                                ((method_call)(((assign)st.subnodes[0]).from)).parameters.expressions.Insert(ind, new ident("$param" + ind));
                                ((method_call)(((assign)st.subnodes[0]).from)).parameters.expressions.RemoveAt(ind + 1);
                            }
                    }
                }
                ////////////////
                body_variant_list.Add(st);
                //guard_list.Add(LRParser.GetReductionSyntaxNode(2));
                guard_list.Add(((ArrayList)body_list[g_ind])[0]);
                if (list_params_temp.Count != 0)
                {
                    list_params.Add(list_params_temp.Clone());
                    list_params_temp.Clear();
                }
            }
        }
        else
        {
            int i = 0;
            while (i < func_name.Count && ((ident)func_name[i]).name != ((ident)LRParser.GetReductionSyntaxNode(0)).name)
                i++;
            if (i == func_name.Count)
            {
                func_name.Add((ident)LRParser.GetReductionSyntaxNode(0));
            }
        }
        list_params1.Clear();
        //int kk = last_list_method_calls.Count;
        //kk = last_function_lambda_definitions.Count;
        for (int i = 0; i < last_function_lambda_definitions.Count; i++)
        {
            for (int j = 0; j < ((function_lambda_definition)last_function_lambda_definitions[i]).formal_parameters.params_list.Count; j++)
            {
                string name_param = ((function_lambda_definition)last_function_lambda_definitions[i]).formal_parameters.params_list[j].idents.idents[0].name;
                int k = 0;
                while (k < last_list_method_calls.Count && ((ident)((method_call)last_list_method_calls[k]).dereferencing_value).name != name_param)
                    k++;
                if (k < last_list_method_calls.Count)
                {
                    function_lambda_definition fld = find_func_lambda_after(((function_lambda_definition)last_function_lambda_definitions[i]).lambda_name);
                    fld.formal_parameters.params_list[j].vars_type = func_type(((method_call)last_list_method_calls[k]).parameters.expressions.Count);
                }
            }
        }
        last_list_method_calls.Clear();
        last_function_lambda_definitions.Clear();
    }
    return null;
}
    case (int)RuleConstants.RULE_VARIANT_TKQUOTE_TKIDENT_TKQUOTE:
//<variant> ::= <list_param1> 'tkQuote' 'tkIdent' 'tkQuote' <list_param1> <guard_body_list> <where_var>
{
    ArrayList body_list = (ArrayList)LRParser.GetReductionSyntaxNode(5);
    for (int g_ind = 0; g_ind < body_list.Count; g_ind++)
    {
        ///////////////////////////////////body
        statement_list st = null;

        statement_list body_1 = (statement_list)((ArrayList)body_list[g_ind])[1];

        if (LRParser.GetReductionSyntaxNode(6) != null)
        {
            ArrayList ar = (ArrayList)LRParser.GetReductionSyntaxNode(6);
            for (int ii = 0; ii < ar.Count; ii++)
                where_funcs.Add(ar[ii]);

            statement_list _statement_list = body_1;
            //////////////////////////////////////////////for guard
            formal_parameters _formal_parametres = null;
            //////////////////////////
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);
            /////////////////////////////
            lambda_num++;
            function_lambda_definition _procedure_definition = new function_lambda_definition();
            _procedure_definition.formal_parameters = _formal_parametres;
            _procedure_definition.return_type = (type_definition)_named_type_reference1;
            _procedure_definition.ident_list = null;
            _procedure_definition.proc_body = null;
            _procedure_definition.parameters = null;
            _procedure_definition.lambda_name = "__lam_where__" + lambda_num;
            //new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, null, null, null, "lambda_where" + lambda_num);
            _procedure_definition.proc_body = _statement_list;
            procedure_definition pr = lambda(_procedure_definition);
            //_function_lambda_definitions.Add(_procedure_definition);////////////////
            ((block)pr.proc_body).defs = new declarations();
            int start = 0;
            int k = where_funcs.Count - 1;
            while (k > 0 && ((procedure_definition)where_funcs[k]).proc_header.name.meth_name.name.Contains("__lambda__"))
                k--;
            int kk = 0;
            while (k > 0 && kk < ar.Count - 1 && !((procedure_definition)where_funcs[k]).proc_header.name.meth_name.name.Contains("__lambda__"))
            {
                k--;
                kk++;
            }
            start = k;
            int i = start;
            int n = where_funcs.Count;
            if (start >= 0)
                while (i < n)
                {
                    ((block)pr.proc_body).defs.defs.Add((procedure_definition)where_funcs[start]);
                    where_funcs.RemoveAt(start);
                    i++;
                }
            /////////////////////////////////lambda
            if (lambda_stack.Count > 0 && ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count == 0)
                lambda_stack.RemoveAt(lambda_stack.Count - 1);
            if (lambda_stack.Count > 0)
            {
                if (((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count > 0)
                {
                    if (((block)pr.proc_body).defs == null)
                        ((block)pr.proc_body).defs = new declarations();
                    for (int ii = 0; ii < ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count; ii++)
                        ((block)pr.proc_body).defs.defs.Add((procedure_definition)((ArrayList)lambda_stack[lambda_stack.Count - 1])[ii]);
                    for (int ii = 0; ii < ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count; ii++)
                        _function_lambda_definitions.RemoveAt(_function_lambda_definitions.Count - 1);
                }
                //lambda_stack.RemoveAt(lambda_stack.Count - 1);
            }
            //////////////////////////////////
            where_funcs.Add(pr);
            //if (let_flag.Count > 0)
            //let_func_last.Add(pr);
            expression_list el = new expression_list();
            //where_fact_params.Clear();
            op_type_node _op_type_node = new op_type_node(Operators.Assignment);
            assign _assign = new assign((addressed_value)new ident("result"), new ident(_procedure_definition.lambda_name), Operators.Assignment);

            statement_list _statement_list1 = new statement_list();
            _statement_list1.subnodes.Add(_assign);
            st = _statement_list1;
        }
        else
        {
            //////////////////////////////////////////////for guard
            st = body_1;
        }
        ///////////////////////////////////\body

        ident _ident = (ident)LRParser.GetReductionSyntaxNode(2);
        expression_list _ex_l1 = (expression_list)LRParser.GetReductionSyntaxNode(0);
        expression_list _ex_l2 = (expression_list)LRParser.GetReductionSyntaxNode(4);
        for (int i = 0; i < _ex_l2.expressions.Count; i++)
            _ex_l1.expressions.Add(_ex_l2.expressions[i]);

        ////////////////////////////////////////////////////for let & where
        let_where_funcs.Clear();
        while (let_where_funcs_main.Count > 1)
            let_where_funcs_main.RemoveAt(let_where_funcs_main.Count - 1);
        list_params1.Clear();
        let_flag.Clear();
        int iiii = 0;
        while (iiii < func_name.Count && ((ident)func_name[iiii]).name != _ident.name)
            iiii++;
        if (iiii == func_name.Count && let_funcs_funcs.Count == 0 || (let_funcs_funcs.Count > 0 && ((string)((ArrayList)let_funcs_funcs[let_funcs_funcs.Count - 1])[0]) != _ident.name))
        {
            ArrayList ar = new ArrayList();
            ar.Add(_ident.name);
            if (where_funcs.Count > 0)
            {
                for (int i = 0; i < let_funcs.Count; i++)
                    ((block)((procedure_definition)where_funcs[0]).proc_body).defs.defs.Add((procedure_definition)let_funcs[i]);
                ar.Add(where_funcs.Clone());
            }
            else
                ar.Add(let_funcs.Clone());
            let_funcs_funcs.Add(ar);
            let_funcs.Clear();
            where_funcs.Clear();
            let_func_last.Clear();
        }
        else
        {
            int r = 0;
            while (r < let_funcs_funcs.Count && ((string)((ArrayList)let_funcs_funcs[r])[0]) !=
                _ident.name)
                r++;
            if (r < let_funcs_funcs.Count)
            {
                if (where_funcs.Count > 0)
                {
                    for (int i = 0; i < let_funcs.Count; i++)
                        ((block)((procedure_definition)where_funcs[0]).proc_body).defs.defs.Add((procedure_definition)let_funcs[i]);
                    for (int i = 0; i < where_funcs.Count; i++)
                        ((ArrayList)((ArrayList)let_funcs_funcs[r])[1]).Add(where_funcs[i]);
                }
                else
                    for (int i = 0; i < let_funcs.Count; i++)
                        ((ArrayList)((ArrayList)let_funcs_funcs[r])[1]).Add(let_funcs[i]);
                let_funcs.Clear();
                where_funcs.Clear();
                let_func_last.Clear();
            }
        }

        if (iiii == func_name.Count)
        {
            /////////////////////////////////////////////////////
            ArrayList ar_lambda = new ArrayList();
            ar_lambda.Add(_ident.name);
            ar_lambda.Add(_function_lambda_definitions.Clone());
            _function_lambda_definitions_main.Add(ar_lambda.Clone());
            _function_lambda_definitions.Clear();
            /////////////////////////////////////////////////////
        }
        else
        {
            //////////////////////////////////
            ArrayList ar_lambda = (ArrayList)_function_lambda_definitions.Clone();
            for (int i = 0; i < ar_lambda.Count; i++)
                ((ArrayList)((ArrayList)_function_lambda_definitions_main[_function_lambda_definitions_main.Count - 1])[1]).Add(ar_lambda[i]);
            _function_lambda_definitions.Clear();
            //////////////////////////////////
        }
        ////////////////////////////////////////////////////

        if (st/*LRParser.GetReductionSyntaxNode(7)*/ != null)
        {
            int i = 0;
            while (i < func_name.Count && ((ident)func_name[i]).name != _ident.name)
                i++;
            if (i == func_name.Count)
            {
                func_name.Add(_ident);
                list_return_funcs_main.Add(list_return_funcs.Clone());
                list_return_funcs.Clear();
                if (list_method_calls_main.Count > 0)
                    for (int iii = 0; iii < ((ArrayList)list_method_calls_main[list_method_calls_main.Count - 1]).Count; iii++)
                        if (list_method_calls.Count > 0)
                            list_method_calls.RemoveAt(0);
                list_method_calls_main.Add(list_method_calls.Clone());
                if (param_value_list.Count != 0)
                {
                    param_value_list_main.Add(param_value_list.Clone());
                    body_variant_list_main.Add(body_variant_list.Clone());
                    guard_list_main.Add(guard_list.Clone());
                    list_params_main.Add(list_params.Clone());
                }
                param_value_list.Clear();
                body_variant_list.Clear();
                guard_list.Clear();
                list_params.Clear();
                param_value_list.Add(_ex_l1);
                ////////////////Dobavlyaem formalnye parametry
                //statement_list st = (statement_list)LRParser.GetReductionSyntaxNode(7);
                if (st.subnodes[0] is assign)
                {
                    if (((assign)st.subnodes[0]).from is function_lambda_call)
                    {
                        function_lambda_call flc = (function_lambda_call)(((assign)st.subnodes[0]).from);
                        function_lambda_definition fld = ((function_lambda_call)(((assign)st.subnodes[0]).from)).f_lambda_def;
                        for (int ind = flc.parameters.expressions.Count; ind < fld.parameters.expressions.Count; ind++)
                        {
                            if (param_value_list.Count == 0)
                                param_value_list.Add(new expression_list());
                            if (param_value_list[param_value_list.Count - 1] == null)
                                param_value_list[param_value_list.Count - 1] = new expression_list();
                            ((expression_list)param_value_list[param_value_list.Count - 1]).expressions.Add(fld.parameters.expressions[ind]);
                        }
                    }
                    if (((assign)st.subnodes[0]).from is method_call)
                    {
                        method_call mc = (method_call)(((assign)st.subnodes[0]).from);
                        int jjj = 0;
                        int ind_null = -1;
                        while (jjj < mc.parameters.expressions.Count)
                            if (mc.parameters.expressions[jjj] is new_expr && ((new_expr)mc.parameters.expressions[jjj]).params_list.expressions[0] is string_const &&
                                ((string_const)((new_expr)mc.parameters.expressions[jjj]).params_list.expressions[0]).Value == "$null")
                            {
                                ind_null = jjj;
                                jjj = mc.parameters.expressions.Count;
                            }
                            else
                                jjj++;
                        if (ind_null >= 0)
                            for (int ind = ind_null; ind < mc.parameters.expressions.Count; ind++)
                            {
                                //int j = 0;
                                //while ()
                                if (param_value_list.Count == 0)
                                    param_value_list.Add(new expression_list());
                                if (param_value_list[param_value_list.Count - 1] == null)
                                    param_value_list[param_value_list.Count - 1] = new expression_list();
                                ((expression_list)param_value_list[param_value_list.Count - 1]).expressions.Add(new ident("$param" + ind));
                                ((method_call)(((assign)st.subnodes[0]).from)).parameters.expressions.Insert(ind, new ident("$param" + ind));
                                ((method_call)(((assign)st.subnodes[0]).from)).parameters.expressions.RemoveAt(ind + 1);
                            }
                    }
                }
                ////////////////
                body_variant_list.Add(st);
                //guard_list.Add(LRParser.GetReductionSyntaxNode(5));
                guard_list.Add(((ArrayList)body_list[g_ind])[0]);
                if (list_params_temp.Count != 0)
                {
                    list_params.Add(list_params_temp.Clone());
                    list_params_temp.Clear();
                }
            }
            else
            {
                ArrayList ar = (ArrayList)list_method_calls.Clone();
                for (int iii = 0; iii < list_method_calls.Count; iii++)
                    ((ArrayList)list_method_calls_main[list_method_calls_main.Count - 1]).Add(ar[iii]);
                param_value_list.Add(_ex_l1);
                ////////////////Dobavlyaem formalnye parametry
                //statement_list st = (statement_list)LRParser.GetReductionSyntaxNode(7);
                if (st.subnodes[0] is assign)
                {
                    if (((assign)st.subnodes[0]).from is function_lambda_call)
                    {
                        function_lambda_call flc = (function_lambda_call)(((assign)st.subnodes[0]).from);
                        function_lambda_definition fld = ((function_lambda_call)(((assign)st.subnodes[0]).from)).f_lambda_def;
                        for (int ind = flc.parameters.expressions.Count; ind < fld.parameters.expressions.Count; ind++)
                        {
                            if (param_value_list.Count == 0)
                                param_value_list.Add(new expression_list());
                            if (param_value_list[param_value_list.Count - 1] == null)
                                param_value_list[param_value_list.Count - 1] = new expression_list();
                            ((expression_list)param_value_list[param_value_list.Count - 1]).expressions.Add(fld.parameters.expressions[ind]);
                        }
                    }
                    if (((assign)st.subnodes[0]).from is method_call)
                    {
                        method_call mc = (method_call)(((assign)st.subnodes[0]).from);
                        int jjj = 0;
                        int ind_null = -1;
                        while (jjj < mc.parameters.expressions.Count)
                            if (mc.parameters.expressions[jjj] is new_expr && ((new_expr)mc.parameters.expressions[jjj]).params_list.expressions[0] is string_const &&
                                ((string_const)((new_expr)mc.parameters.expressions[jjj]).params_list.expressions[0]).Value == "$null")
                            {
                                ind_null = jjj;
                                jjj = mc.parameters.expressions.Count;
                            }
                            else
                                jjj++;
                        if (ind_null >= 0)
                            for (int ind = ind_null; ind < mc.parameters.expressions.Count; ind++)
                            {
                                if (param_value_list.Count == 0)
                                    param_value_list.Add(new expression_list());
                                if (param_value_list[param_value_list.Count - 1] == null)
                                    param_value_list[param_value_list.Count - 1] = new expression_list();
                                ((expression_list)param_value_list[param_value_list.Count - 1]).expressions.Add(new ident("$param" + ind));
                                ((method_call)(((assign)st.subnodes[0]).from)).parameters.expressions.Insert(ind, new ident("$param" + ind));
                                ((method_call)(((assign)st.subnodes[0]).from)).parameters.expressions.RemoveAt(ind + 1);
                            }
                    }
                }
                ////////////////
                body_variant_list.Add(st);
                //guard_list.Add(LRParser.GetReductionSyntaxNode(5));
                guard_list.Add(((ArrayList)body_list[g_ind])[0]);
                if (list_params_temp.Count != 0)
                {
                    list_params.Add(list_params_temp.Clone());
                    list_params_temp.Clear();
                }
            }
        }
        else
        {
            int i = 0;
            while (i < func_name.Count && ((ident)func_name[i]).name != _ident.name)
                i++;
            if (i == func_name.Count)
            {
                func_name.Add(_ident);
            }
        }
        list_params1.Clear();
        //int kk = last_list_method_calls.Count;
        //kk = last_function_lambda_definitions.Count;
        for (int i = 0; i < last_function_lambda_definitions.Count; i++)
        {
            for (int j = 0; j < ((function_lambda_definition)last_function_lambda_definitions[i]).formal_parameters.params_list.Count; j++)
            {
                string name_param = ((function_lambda_definition)last_function_lambda_definitions[i]).formal_parameters.params_list[j].idents.idents[0].name;
                int k = 0;
                while (k < last_list_method_calls.Count && ((ident)((method_call)last_list_method_calls[k]).dereferencing_value).name != name_param)
                    k++;
                if (k < last_list_method_calls.Count)
                {
                    function_lambda_definition fld = find_func_lambda_after(((function_lambda_definition)last_function_lambda_definitions[i]).lambda_name);
                    fld.formal_parameters.params_list[j].vars_type = func_type(((method_call)last_list_method_calls[k]).parameters.expressions.Count);
                }
            }
        }
        last_list_method_calls.Clear();
        last_function_lambda_definitions.Clear();
    }
    return null;
}
    case (int)RuleConstants.RULE_LIST_PARAM1:
//<list_param1> ::= <list_param> <empty>
{
    bool b = false;
    if (list_param.Count == 1)
    {
        list_params_temp.Add(new ArrayList());
        let_where_list_params.Add(new ArrayList());//
        if (list_param[0] is new_expr && ((string_const)((new_expr)list_param[0]).params_list.expressions[1]).Value == "empty")
            b = true;
        list_param.Clear();
    }
    else
    {
        list_params_temp.Add(list_param.Clone());
        let_where_list_params.Add(list_param.Clone());//
    }
    expression_list _expression_list = new expression_list();
    if (list_param.Count == 0)
    {
        if (b)
        {
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);
            expression_list el = new expression_list();
            el.expressions.Add(new int32_const(0));
            literal lt;
            string text = "empty_list";
            lt = new string_const(text);
            el.expressions.Add(lt as expression);
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, el, false, null);
            _expression_list.expressions.Add(newexpr);
        }
        else
        {
            _expression_list.source_context = ((expression)LRParser.GetReductionSyntaxNode(0)).source_context;
            _expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
        }
    }
    else
    {
        ident id = new ident();
        for (int i = 0; i < list_param.Count; i++)
        {
            if (list_param[i] is ident)
                id.name += ((ident)list_param[i]).name;
            else
            {
                errors.Add(new PascalABCCompiler.Errors.UnexpectedToken(this, "идентификатор"));
                return null;
            }
        }
        _expression_list.source_context = ((expression)id).source_context;
        _expression_list.expressions.Add((expression)id);
        list_param.Clear();
    }
    list_params1.Clear();
    for (int i = 0; i < _expression_list.expressions.Count; i++)
        list_params1.Add(_expression_list.expressions[i]);
    last_list_method_calls_lambda.Clear();
    return _expression_list;
}
    case (int)RuleConstants.RULE_BODY_WHERE:
    //<body_where> ::= <body_func> <where_var>
{
    //guard_list.Add(LRParser.GetReductionSyntaxNode(0));
    if (LRParser.GetReductionSyntaxNode(1) != null)
    {
        ArrayList ar = (ArrayList)LRParser.GetReductionSyntaxNode(1);
        for (int ii = 0; ii < ar.Count; ii++)
            where_funcs.Add(ar[ii]);

        statement_list _statement_list = (statement_list)LRParser.GetReductionSyntaxNode(0);
        //////////////////////////////////////////////for guard
        formal_parameters _formal_parametres = null;
        //////////////////////////
        named_type_reference _named_type_reference1 = new named_type_reference();
        ident idtype1 = new ident("datatype");
        _named_type_reference1.source_context = idtype1.source_context;
        _named_type_reference1.names.Add(idtype1);
        /////////////////////////////
        lambda_num++;
        function_lambda_definition _procedure_definition = new function_lambda_definition();
        _procedure_definition.formal_parameters = _formal_parametres;
        _procedure_definition.return_type = (type_definition)_named_type_reference1;
        _procedure_definition.ident_list = null;
        _procedure_definition.proc_body = null;
        _procedure_definition.parameters = null;
        _procedure_definition.lambda_name = "__lam_where__" + lambda_num;
        //new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, null, null, null, "lambda_where" + lambda_num);
        _procedure_definition.proc_body = _statement_list;
        procedure_definition pr = lambda(_procedure_definition);
        //_function_lambda_definitions.Add(_procedure_definition);////////////////
        ((block)pr.proc_body).defs = new declarations();
        int start = 0;
        int k = where_funcs.Count - 1;
        while (k > 0 && ((procedure_definition)where_funcs[k]).proc_header.name.meth_name.name.Contains("__lambda__"))
            k--;
        int kk = 0;
        while (k > 0 && kk < ar.Count - 1 && !((procedure_definition)where_funcs[k]).proc_header.name.meth_name.name.Contains("__lambda__"))
        {
            k--;
            kk++;
        }
        start = k;
        int i = start;
        int n = where_funcs.Count;
        if (start >= 0)
            while (i < n)
            {
                ((block)pr.proc_body).defs.defs.Add((procedure_definition)where_funcs[start]);
                where_funcs.RemoveAt(start);
                i++;
            }
        /////////////////////////////////lambda
        if (lambda_stack.Count > 0 && ((ArrayList)lambda_stack[lambda_stack.Count-1]).Count==0)
            lambda_stack.RemoveAt(lambda_stack.Count - 1);
        if (lambda_stack.Count > 0)
        {
            if (((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count > 0)
            {
                if (((block)pr.proc_body).defs == null)
                    ((block)pr.proc_body).defs = new declarations();
                for (int ii = 0; ii < ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count; ii++)
                    ((block)pr.proc_body).defs.defs.Add((procedure_definition)((ArrayList)lambda_stack[lambda_stack.Count - 1])[ii]);
                for (int ii = 0; ii < ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count; ii++)
                    _function_lambda_definitions.RemoveAt(_function_lambda_definitions.Count - 1);
            }
            //lambda_stack.RemoveAt(lambda_stack.Count - 1);
        }
        //////////////////////////////////
        where_funcs.Add(pr);
        //if (let_flag.Count > 0)
        //let_func_last.Add(pr);
        expression_list el = new expression_list();
        //where_fact_params.Clear();
        op_type_node _op_type_node = new op_type_node(Operators.Assignment);
        assign _assign = new assign((addressed_value)new ident("result"), new ident(_procedure_definition.lambda_name), Operators.Assignment);

        statement_list _statement_list1 = new statement_list();
        _statement_list1.subnodes.Add(_assign);
        return _statement_list1;
    }
    else
    {
        //////////////////////////////////////////////for guard
        statement_list _statement_list = (statement_list)LRParser.GetReductionSyntaxNode(0);
        return _statement_list;
    }
}
	case (int)RuleConstants.RULE_WHERE_VAR :
	//<where_var> ::= 
	//NONTERMINAL:<where_var> ::= 
	return null;
	//ENDNONTERMINAL
    case (int)RuleConstants.RULE_WHERE_VAR_TKWHERE:
    //<where_var> ::= 'tkWhere' <inits>
    {
        token_where_count--;
        //let_where_funcs_main.RemoveAt(let_where_funcs_main.Count-1);
        let_where_funcs.Clear();
        return LRParser.GetReductionSyntaxNode(1);
    }
    case (int)RuleConstants.RULE_INITS:
//<inits> ::= <init> <empty>
{
    ArrayList ar = new ArrayList();
    ar.Add(LRParser.GetReductionSyntaxNode(0));
    return ar;
}
    case (int)RuleConstants.RULE_INITS_TKSEMICOLON:
//<inits> ::= <inits> 'tkSemiColon' <init>
{
    ArrayList ar = (ArrayList)LRParser.GetReductionSyntaxNode(0);
    ar.Add(LRParser.GetReductionSyntaxNode(2));
    return ar;
}
    case (int)RuleConstants.RULE_INIT_TKIDENT:
//<init> ::= 'tkIdent' <params_where> <guard_body_list> <where_var>
{
    expression_list _expression_list = (expression_list)LRParser.GetReductionSyntaxNode(1);
    /*statement_list _statement_list = (statement_list)LRParser.GetReductionSyntaxNode(4);*/
    statement_list _statement_list = new statement_list();
    ArrayList ar_main = (ArrayList)LRParser.GetReductionSyntaxNode(2);
    ////////////////////////////for list
    statement_list stmt_l = new statement_list();
    int start = let_where_list_params.Count;
    if (_expression_list !=null)
        start = let_where_list_params.Count - _expression_list.expressions.Count;
    int i_ex = 0;
    for (int ll = start; ll < let_where_list_params.Count; ll++)
    {
        for (int l = 0; l < ((ArrayList)let_where_list_params[ll]).Count; l++)
        {
            ident_list il1 = new ident_list();
            il1.idents.Add(new ident(((ident)((ArrayList)let_where_list_params[ll])[l]).name));
            named_type_reference _n_t_r = new named_type_reference();
            ident it = new ident("datatype");
            _n_t_r.names.Add(it);

            var_def_statement _var_def_statement = new var_def_statement(il1, (type_definition)_n_t_r, null, definition_attribute.None, false);
            var_statement _var_statement = new var_statement(_var_def_statement);
            stmt_l.subnodes.Add(_var_statement);
        }
        if (((ArrayList)let_where_list_params[ll]).Count > 0)
        {
            op_type_node _op_type_node = new op_type_node(Operators.Assignment);
            dot_node _dot_node = new dot_node(null, (addressed_value)(new ident("head")));
            ident id = _expression_list.expressions[i_ex] as ident;
            /*for (int i1 = 0; i1 < ((ArrayList)let_where_list_params[ll]).Count; i1++)
                id.name += ((ident)((ArrayList)let_where_list_params[ll])[i1]).name;*/
            if (id.name != null)
            {
                _dot_node.left = (addressed_value)(id);
                object o = null;
                method_call _method_call = new method_call(o as expression_list);
                if (_method_call is dereference)
                    ((dereference)_method_call).dereferencing_value = (addressed_value)_dot_node;
                assign _assign1 = new assign(((ident)((ArrayList)let_where_list_params[ll])[((ArrayList)let_where_list_params[ll]).Count - 1]) as addressed_value, _method_call as expression, _op_type_node.type);
                stmt_l.subnodes.Add(_assign1);

                ///
                for (int i1 = ((ArrayList)let_where_list_params[ll]).Count - 2; i1 > 0; i1--)
                {
                    _dot_node = new dot_node((addressed_value)id, (addressed_value)(new ident("tail")));
                    for (int j1 = 0; j1 < ((ArrayList)let_where_list_params[ll]).Count - 2 - i1; j1++)
                    {
                        _dot_node = new dot_node(_dot_node, (addressed_value)(new ident("tail")));
                    }
                    _dot_node = new dot_node(_dot_node, (addressed_value)(new ident("head")));

                    _method_call = new method_call(o as expression_list);
                    if (_method_call is dereference)
                        ((dereference)_method_call).dereferencing_value = (addressed_value)_dot_node;
                    _assign1 = new assign(((ident)((ArrayList)let_where_list_params[ll])[i1]) as addressed_value, _method_call as expression, _op_type_node.type);
                    stmt_l.subnodes.Add(_assign1);
                }

                _dot_node = new dot_node((addressed_value)id, (addressed_value)(new ident("tail")));

                for (int j1 = 0; j1 < ((ArrayList)let_where_list_params[ll]).Count - 2; j1++)
                {
                    _dot_node = new dot_node(_dot_node, (addressed_value)(new ident("tail")));
                }
                _method_call = new method_call(o as expression_list);
                if (_method_call is dereference)
                    ((dereference)_method_call).dereferencing_value = (addressed_value)_dot_node;
                _assign1 = new assign(((ident)((ArrayList)let_where_list_params[ll])[0]) as addressed_value, _method_call as expression, _op_type_node.type);
                stmt_l.subnodes.Add(_assign1);
            }
        }
        i_ex++;
    }
    for (int ll = 0; ll < let_where_list_params.Count - start; ll++)
        list_params_temp.RemoveAt(list_params_temp.Count - 1);
    for (int i = stmt_l.subnodes.Count - 1; i >= 0; i--)
        _statement_list.subnodes.Insert(0, stmt_l.subnodes[i]);
    if (_expression_list != null)
        for (int i = 0; i < _expression_list.expressions.Count; i++)
            let_where_list_params.RemoveAt(let_where_list_params.Count - 1);
    ////////////////////////////
    formal_parameters _formal_parametres = null;
    ident_list i_l = new ident_list();
    if (_expression_list != null)
    {
        for (int i = 0; i < _expression_list.expressions.Count; i++)
            i_l.idents.Add((ident)_expression_list.expressions[i]);
        /////////////////////////////
        _formal_parametres = new formal_parameters();
        for (int i = 0; i < i_l.idents.Count; i++)
        {
            ident_list _ident_list = new ident_list();
            ident id = (ident)_expression_list.expressions[i];
            _ident_list.idents.Add(id);
            string name_param = id.name;
            typed_parameters _typed_parametres = null;
            int k = 0;
            while (k < last_list_method_calls_lambda.Count && ((ident)((method_call)last_list_method_calls_lambda[k]).dereferencing_value).name != name_param)
                k++;
            if (k < last_list_method_calls_lambda.Count)
                _typed_parametres = new typed_parameters(_ident_list, func_type(((method_call)last_list_method_calls_lambda[k]).parameters.expressions.Count), parametr_kind.none, null);
            else
            {
                named_type_reference _named_type_reference = new named_type_reference();

                ident idtype = new ident("datatype");
                _named_type_reference.names.Add(idtype);

                _typed_parametres = new typed_parameters(_ident_list, (type_definition)_named_type_reference, parametr_kind.none, null);
                parsertools.create_source_context(_typed_parametres, _ident_list, _named_type_reference);
            }
            _formal_parametres.params_list.Add(_typed_parametres);
        }
    }
    //////////////////////////
    named_type_reference _named_type_reference1 = new named_type_reference();
    ident idtype1 = new ident("datatype");
    _named_type_reference1.source_context = idtype1.source_context;
    _named_type_reference1.names.Add(idtype1);
    /////////////////////////////
    function_lambda_definition _procedure_definition = new function_lambda_definition();
    _procedure_definition.formal_parameters = _formal_parametres;
    _procedure_definition.return_type = (type_definition)_named_type_reference1;
    _procedure_definition.ident_list = i_l;
    _procedure_definition.proc_body = null;
    _procedure_definition.parameters = _expression_list;
    _procedure_definition.lambda_name = ((ident)LRParser.GetReductionSyntaxNode(0)).name;
    //new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, i_l, null, _expression_list, ((ident)LRParser.GetReductionSyntaxNode(0)).name);
    object rt = _expression_list;
    ////////////////////////////////////////for guard
    for (int i = 0; i < ar_main.Count; i++)
        if (((ArrayList)ar_main[i])[0] != null)
        {
            if_node _if_node = new if_node();
            _if_node.condition = _ob((expression)((ArrayList)ar_main[i])[0]);
            _if_node.then_body = new statement_list();
            _if_node.then_body = (statement)((ArrayList)ar_main[i])[1];
            procedure_call _exit = new procedure_call(new ident("exit") as addressed_value);
            ((statement_list)_if_node.then_body).subnodes.Add(_exit);
            _statement_list.subnodes.Add(_if_node);
        }
        else
        {
            _statement_list.subnodes.Add((statement)((ArrayList)ar_main[i])[1]);
            procedure_call _exit = new procedure_call(new ident("exit") as addressed_value);
            _statement_list.subnodes.Add(_exit);
        }
    //////////////////////////////////////////
    _procedure_definition.proc_body = _statement_list;
    parsertools.create_source_context(_procedure_definition, _expression_list, rt);
    procedure_definition pr = lambda(_procedure_definition);
    ///////////////////////////
    ///////////////////////////
    if (LRParser.GetReductionSyntaxNode(3) != null)
    {
        if (((block)pr.proc_body).defs == null)
            ((block)pr.proc_body).defs = new declarations();
        for (int i = 0; i < ((ArrayList)LRParser.GetReductionSyntaxNode(3)).Count; i++)
            ((block)pr.proc_body).defs.defs.Add((procedure_definition)((ArrayList)LRParser.GetReductionSyntaxNode(3))[i]);
    }
    if (let_where_funcs.Count > 0)
    {
        if (((block)pr.proc_body).defs == null)
            ((block)pr.proc_body).defs = new declarations();
        for (int i = 0; i < let_where_funcs.Count; i++)
            ((block)pr.proc_body).defs.defs.Add((procedure_definition)let_where_funcs[i]);
        if (let_where_funcs_main.Count+1 > token_where_count)
        {
            if (((block)pr.proc_body).defs == null)
                ((block)pr.proc_body).defs = new declarations();
            for (int i = 0; i < last_where_funcs.Count; i++)
                ((block)pr.proc_body).defs.defs.Add((procedure_definition)last_where_funcs[i]);
            for (int i = 0; i < last_where_funcs.Count; i++)
                if (let_funcs.Count > 0)
                    let_funcs.RemoveAt(let_funcs.Count - 1);
            last_where_funcs.Clear();
        }
        let_where_funcs.Clear();
        let_func_last.Clear();
        let_flag.Clear();
    }
    else
        if (let_where_funcs_main.Count > 2 && let_where_funcs_main.Count>token_where_count)
        {
            ArrayList ar = new ArrayList();
            ar = (let_where_funcs_main[let_where_funcs_main.Count - 1] as ArrayList).Clone() as ArrayList;

            if (((block)pr.proc_body).defs == null)
                ((block)pr.proc_body).defs = new declarations();
            for (int i = 0; i < ar.Count; i++)
                ((block)pr.proc_body).defs.defs.Add((procedure_definition)ar[i]);
            let_where_funcs.Clear();
            let_func_last.Clear();
            let_flag.Clear();
            let_where_funcs_main.RemoveAt(let_where_funcs_main.Count-1);
            for (int i = 0; i < ar.Count; i++)
                if (let_funcs.Count > 0)
                    let_funcs.RemoveAt(let_funcs.Count - 1);
        }
    ///////////////////////////lambda
    if (lambda_stack.Count > 0)
    {
        if (((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count > 0)
        {
            if (((block)pr.proc_body).defs == null)
                ((block)pr.proc_body).defs = new declarations();
            for (int i = 0; i < ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count; i++)
                ((block)pr.proc_body).defs.defs.Add((procedure_definition)((ArrayList)lambda_stack[lambda_stack.Count - 1])[i]);
            for (int ii = 0; ii < ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count; ii++)
                _function_lambda_definitions.RemoveAt(_function_lambda_definitions.Count - 1);
        }
        int number = ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count;
        for (int i = 0; i < number;i++ )
            lambda_stack.RemoveAt(lambda_stack.Count - 1);
    }
    ///////////////////////////
    return pr;
}
        case (int)RuleConstants.RULE_PARAMS_WHERE :
	//<params_where> ::= 
	//NONTERMINAL:<params_where> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_PARAMS_WHERE2 :
	//<params_where> ::= <param> <empty>
{
            							bool b = false;
    if (list_param.Count == 1)
    {
        list_params_temp.Add(new ArrayList());
        let_where_list_params.Add(new ArrayList());//
        if (list_param[0] is new_expr && ((string_const)((new_expr)list_param[0]).params_list.expressions[1]).Value == "empty")
            b = true;
        list_param.Clear();
    }
    else
    {
        list_params_temp.Add(list_param.Clone());
        let_where_list_params.Add(list_param.Clone());//
    }
    expression_list _expression_list = new expression_list();
    if (list_param.Count == 0)
    {
        if (b)
        {
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);
            expression_list el = new expression_list();
            el.expressions.Add(new int32_const(0));
            literal lt;
            string text = "empty_list";
            lt = new string_const(text);
            el.expressions.Add(lt as expression);
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, el, false, null);
            _expression_list.expressions.Add(newexpr);
        }
        else
        {
            _expression_list.source_context = ((expression)LRParser.GetReductionSyntaxNode(0)).source_context;
            _expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
        }
    }
    else
    {
        ident id = new ident();
        for (int i = 0; i < list_param.Count; i++)
        {
            if (list_param[i] is ident)
                id.name += ((ident)list_param[i]).name;
            else
            {
                errors.Add(new PascalABCCompiler.Errors.UnexpectedToken(this, "идентификатор"));
                return null;
            }
        }
        _expression_list.source_context = ((expression)id).source_context;
        _expression_list.expressions.Add((expression)id);
        list_param.Clear();
    }
    list_params1.Clear();
    for (int i = 0; i < _expression_list.expressions.Count; i++)
        list_params1.Add(_expression_list.expressions[i]);
    last_list_method_calls_lambda.Clear();
    return _expression_list;
        							}
	case (int)RuleConstants.RULE_PARAMS_WHERE3 :
	//<params_where> ::= <params_where> <param>

								{
            							bool b = false;
    if (list_param.Count == 1)
    {
        list_params_temp.Add(new ArrayList());
        let_where_list_params.Add(new ArrayList());//
        if (list_param[0] is new_expr && ((string_const)((new_expr)list_param[0]).params_list.expressions[1]).Value == "empty")
            b = true;
        list_param.Clear();
    }
    else
    {
        list_params_temp.Add(list_param.Clone());
        let_where_list_params.Add(list_param.Clone());
    }
    expression_list _expression_list = (expression_list)LRParser.GetReductionSyntaxNode(0);
    if (list_param.Count == 0)
    {
        if (!b)
        {
            _expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(1) as expression);
        }
        else
        {
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);
            expression_list el = new expression_list();
            el.expressions.Add(new int32_const(0));
            literal lt;
            string text = "empty_list";
            lt = new string_const(text);
            el.expressions.Add(lt as expression);
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, el, false, null);
            _expression_list.expressions.Add(newexpr);
        }
    }
    else
    {
        ident id = new ident();
        for (int i = 0; i < list_param.Count; i++)
        {
            if (list_param[i] is ident)
                id.name += ((ident)list_param[i]).name;
            else
            {
                errors.Add(new PascalABCCompiler.Errors.UnexpectedToken(this, "идентификатор"));
                return null;
            }
        }
        _expression_list.expressions.Add((expression)id);
        list_param.Clear();
    }
    list_params1.Clear();
    for (int i = 0; i < _expression_list.expressions.Count; i++)
        list_params1.Add(_expression_list.expressions[i]);
    last_list_method_calls_lambda.Clear();
    return _expression_list;
        							}
	case (int)RuleConstants.RULE_PARAMS :
	//<params> ::= 
	//NONTERMINAL:<params> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_PARAMS2 :
	//<params> ::= <param> <empty>
{
    //let_flag.Add(1);
            							bool b = false;
    if (list_param.Count == 1)
    {
        list_params_temp.Add(new ArrayList());
        let_where_list_params.Add(new ArrayList());//
        if (list_param[0] is new_expr && ((string_const)((new_expr)list_param[0]).params_list.expressions[1]).Value == "empty")
            b = true;
        list_param.Clear();
    }
    else
    {
        list_params_temp.Add(list_param.Clone());
        let_where_list_params.Add(list_param.Clone());//
    }
    expression_list _expression_list = new expression_list();
    if (list_param.Count == 0)
    {
        if (b)
        {
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);
            expression_list el = new expression_list();
            el.expressions.Add(new int32_const(0));
            literal lt;
            string text = "empty_list";
            lt = new string_const(text);
            el.expressions.Add(lt as expression);
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, el, false, null);
            _expression_list.expressions.Add(newexpr);
        }
        else
        {
            _expression_list.source_context = ((expression)LRParser.GetReductionSyntaxNode(0)).source_context;
            _expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
        }
    }
    else
    {
        ident id = new ident();
        for (int i = 0; i < list_param.Count; i++)
        {
            if (list_param[i] is ident)
                id.name += ((ident)list_param[i]).name;
            else
            {
                errors.Add(new PascalABCCompiler.Errors.UnexpectedToken(this, "идентификатор"));
                return null;
            }
        }
        _expression_list.source_context = ((expression)id).source_context;
        _expression_list.expressions.Add((expression)id);
        list_param.Clear();
    }
    list_params1.Clear();
    for (int i = 0; i < _expression_list.expressions.Count; i++)
        list_params1.Add(_expression_list.expressions[i]);
    last_list_method_calls_lambda.Clear();
    return _expression_list;
        							}
	case (int)RuleConstants.RULE_PARAMS3 :
	//<params> ::= <params> <param>

								{
            							bool b = false;
    if (list_param.Count == 1)
    {
        list_params_temp.Add(new ArrayList());
        let_where_list_params.Add(new ArrayList());//
        if (list_param[0] is new_expr && ((string_const)((new_expr)list_param[0]).params_list.expressions[1]).Value == "empty")
            b = true;
        list_param.Clear();
    }
    else
    {
        list_params_temp.Add(list_param.Clone());
        let_where_list_params.Add(list_param.Clone());//
    }
    expression_list _expression_list = (expression_list)LRParser.GetReductionSyntaxNode(0);
    if (list_param.Count == 0)
    {
        if (!b)
        {
            _expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(1) as expression);
        }
        else
        {
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);
            expression_list el = new expression_list();
            el.expressions.Add(new int32_const(0));
            literal lt;
            string text = "empty_list";
            lt = new string_const(text);
            el.expressions.Add(lt as expression);
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, el, false, null);
            _expression_list.expressions.Add(newexpr);
        }
    }
    else
    {
        ident id = new ident();
        for (int i = 0; i < list_param.Count; i++)
        {
            if (list_param[i] is ident)
                id.name += ((ident)list_param[i]).name;
            else
            {
                errors.Add(new PascalABCCompiler.Errors.UnexpectedToken(this, "идентификатор"));
                return null;
            }
        }
        _expression_list.expressions.Add((expression)id);
        list_param.Clear();
    }
    list_params1.Clear();
    for (int i = 0; i < _expression_list.expressions.Count; i++)
        list_params1.Add(_expression_list.expressions[i]);
    last_list_method_calls_lambda.Clear();
    return _expression_list;
        							}
								
	case (int)RuleConstants.RULE_PARAM :
	//<param> ::= <list_param>
return LRParser.GetReductionSyntaxNode(0);
    case (int)RuleConstants.RULE_LIST_PARAM_TKIDENT:
//<list_param> ::= 'tkIdent' <empty>
{
    list_param.Add(LRParser.GetReductionSyntaxNode(0));
    return LRParser.GetReductionSyntaxNode(0);
}
    case (int)RuleConstants.RULE_LIST_PARAM :
	//<list_param> ::= <simple_type_expr> <empty>
{
    list_param.Add(LRParser.GetReductionSyntaxNode(0));
    return LRParser.GetReductionSyntaxNode(0);
}
    case (int)RuleConstants.RULE_LIST_PARAM_TKSQUAREOPEN_TKSQUARECLOSE:
//<list_param> ::= 'tkSquareOpen' 'tkSquareClose'
{
    ArrayList list_elements = new ArrayList();//(ArrayList)LRParser.GetReductionSyntaxNode(1);
    //////////////////////////////////////////////////////////proverka na type
    if (list_elements != null)
    {
        ArrayList types = new ArrayList();
        for (int i = 0; i < list_elements.Count; i++)
        {
            string tp = "";
            if (list_elements[i] is new_expr)
            {
                tp = ((string_const)((new_expr)list_elements[i]).params_list.expressions[1]).Value;
                if (!types.Contains(tp))
                    types.Add(tp);
            }
        }
        if ((types.Count == 2 && !(types.Contains("integer") && types.Contains("real"))) || types.Count > 2)
        {
            //errors.Add(new PascalABCCompiler.Errors.UnexpectedToken(this, "одинаковый тип элементов списка"));
            errors.Add(new PascalABCCompiler.Errors.Error("элементы списка имеют разный тип"));
            return null;
        }
    }
    //////////////////////////////////////////////////////////
    named_type_reference _named_type_reference1 = new named_type_reference();
    ident idtype1 = new ident("datatype_list");
    _named_type_reference1.source_context = idtype1.source_context;
    _named_type_reference1.names.Add(idtype1);
    expression_list el = new expression_list();
    if (list_elements == null)
        list_elements = new ArrayList();
    int32_const n = new int32_const(list_elements.Count);
    el.expressions.Add(n);
    if (list_elements.Count > 0)
    {
        el.expressions.Add(new string_const("list"));
    }
    else
        el.expressions.Add(new string_const("empty"));
    for (int i = 0; i < list_elements.Count; i++)
        el.expressions.Add(list_elements[i] as expression);
    named_type_reference ntr = _named_type_reference1;
    new_expr newexpr = new new_expr(ntr, el, false, null);
    parsertools.create_source_context(newexpr, new ident("new"), parsertools.sc_not_null(el, _named_type_reference1));
    list_param.Add(newexpr);
    return newexpr;
}
    case (int)RuleConstants.RULE_LIST_PARAM_TKBOTTOMMINUS:
//<list_param> ::= 'tkBottomMinus'
{
    ident _ident = new ident("$$$" + bottom_num);
    list_param.Add(_ident);
    bottom_num++;
    return _ident;
}
    case (int)RuleConstants.RULE_LIST_PARAM_TKIDENT_TKCOLON:
//<list_param> ::= 'tkIdent' 'tkColon' <list_param>
{  
								list_param.Add(LRParser.GetReductionSyntaxNode(0));
            							return LRParser.GetReductionSyntaxNode(0);
        							}
    case (int)RuleConstants.RULE_LIST_PARAM_TKBOTTOMMINUS_TKCOLON:
//<list_param> ::= 'tkBottomMinus' 'tkColon' <list_param>
{
    ident _ident = new ident("$$$" + bottom_num);
    list_param.Add(_ident);
    bottom_num++;
    return _ident;
}
    case (int)RuleConstants.RULE_LIST_PARAM_TKROUNDOPEN_TKROUNDCLOSE:
//<list_param> ::= 'tkRoundOpen' <list_param> 'tkRoundClose'
{
    return LRParser.GetReductionSyntaxNode(1);
}
    case (int)RuleConstants.RULE_GUARD :
	//<guard> ::= 
	//NONTERMINAL:<guard> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_GUARD_TKSPLIT :
	//<guard> ::= 'tkSplit' <expr>
    return LRParser.GetReductionSyntaxNode(1);
    case (int)RuleConstants.RULE_GUARD_TKSPLIT_TKOTHERWISE:
    //<guard> ::= 'tkSplit' 'tkOtherwise'
    {
        named_type_reference _named_type_reference1 = new named_type_reference();
        ident idtype1 = new ident("datatype");
        _named_type_reference1.names.Add(idtype1);
        /////
        expression_list el = new expression_list();
        el.expressions.Add(new ident("true") as expression);
        literal lt;
        string text = "boolean";
        lt = new string_const(text);
        el.expressions.Add(lt as expression);
        /////
        named_type_reference ntr = _named_type_reference1;
        new_expr newexpr = new new_expr(ntr, el, false, null);
        parsertools.create_source_context(newexpr, new ident("new"), parsertools.sc_not_null(el, _named_type_reference1));
        return newexpr;
    }
	case (int)RuleConstants.RULE_MAIN_FUNC_TKMAINIDENT1_TKASSIGN :
	//<main_func> ::= 'tkMainIdent1' 'tkAssign' <body_func>
    {
			block _block=new block(null, null);
			
							 	statement_list sl=null;
                                if (LRParser.GetReductionSyntaxNode(2) is statement_list)
                                {
                                    sl = LRParser.GetReductionSyntaxNode(2) as statement_list;
                                    ///////////////////////
                                    ident_list il = new ident_list();
                                    il.idents.Add(new ident("result"));
                                    named_type_reference _named_type_reference1 = new named_type_reference();
                                    ident idtype1 = new ident("datatype");
                                    _named_type_reference1.source_context = idtype1.source_context;
                                    _named_type_reference1.names.Add(idtype1);

                                    var_def_statement _var_def_statement = new var_def_statement(il, (type_definition)_named_type_reference1, null, definition_attribute.None, false);
                                    parsertools.create_source_context(_var_def_statement, il, _named_type_reference1);
                                    var_statement _var_statement = new var_statement(_var_def_statement);
                                    parsertools.create_source_context(_var_statement, null, _var_def_statement);
                                    sl.subnodes.Insert(0,_var_statement);
                                    /////////////////////////
                                }
                                else
                                {
                                    sl = new statement_list();
                                    ///////////////////////
                                    ident_list il = new ident_list();
                                    il.idents.Add(new ident("result"));
                                    named_type_reference _named_type_reference1 = new named_type_reference();
                                    ident idtype1 = new ident("datatype");
                                    _named_type_reference1.source_context = idtype1.source_context;
                                    _named_type_reference1.names.Add(idtype1);

                                    var_def_statement _var_def_statement = new var_def_statement(il, (type_definition)_named_type_reference1, null, definition_attribute.None, false);
                                    parsertools.create_source_context(_var_def_statement, il, _named_type_reference1);
                                    var_statement _var_statement = new var_statement(_var_def_statement);
                                    parsertools.create_source_context(_var_statement, null, _var_def_statement);
                                    sl.subnodes.Add(_var_statement);
                                    /////////////////////////
                                    sl.subnodes.Add(LRParser.GetReductionSyntaxNode(2) as statement);
                                    if (!(LRParser.GetReductionSyntaxNode(2) is empty_statement))
                                        parsertools.assign_source_context(sl, LRParser.GetReductionSyntaxNode(2));
                                }
								_block.program_code=sl;
								
			return _block;
		}
								
	case (int)RuleConstants.RULE_BODY_FUNC :
	//<body_func> ::= <stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_STMTS :
	//<stmts> ::= <stmt> <empty> <empty>
         
		{
			statement_list _statement_list=new statement_list();
			
							_statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(0));
							parsertools.assign_source_context(_statement_list,LRParser.GetReductionSyntaxNode(0)); 
			return _statement_list;
		}

	case (int)RuleConstants.RULE_STMTS_TKSEMICOLON :
	//<stmts> ::= <stmts> 'tkSemiColon' <stmt>
         
		{
			statement_list _statement_list;
			 _statement_list=(statement_list)LRParser.GetReductionSyntaxNode(0);
							_statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(2));
							parsertools.create_source_context(_statement_list,_statement_list,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1))); 
			return _statement_list;
		}
    case (int)RuleConstants.RULE_STMTS1:
        //<stmts1> ::= <stmt> <empty> <empty>
        {
            statement_list _statement_list = new statement_list();

            _statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(0));
            parsertools.assign_source_context(_statement_list, LRParser.GetReductionSyntaxNode(0));
            return _statement_list;
        }
    case (int)RuleConstants.RULE_STMTS12:
        //<stmts1> ::= <stmts1> <stmt>
        {
            statement_list _statement_list;
            _statement_list = (statement_list)LRParser.GetReductionSyntaxNode(0);
            _statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(1));
            parsertools.create_source_context(_statement_list, _statement_list, parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(1), 
                LRParser.GetReductionSyntaxNode(1)));
            return _statement_list;
        } 
	case (int)RuleConstants.RULE_EXPR_TKEQUAL :
	//<expr> ::= <expr> 'tkEqual' <add_expr>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

    case (int)RuleConstants.RULE_EXPR_TKNOTEQUAL:
        //<expr> ::= <expr> 'tkNotEqual' <add_expr>
        {
            bin_expr _bin_expr = new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
            parsertools.create_source_context(_bin_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));

            return _bin_expr;
        }
	
	case (int)RuleConstants.RULE_EXPR_TKMORE :
	//<expr> ::= <expr> 'tkMore' <add_expr>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_EXPR_TKLESS :
	//<expr> ::= <expr> 'tkLess' <add_expr>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_EXPR_TKMOREEQ :
	//<expr> ::= <expr> 'tkMoreEq' <add_expr>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_EXPR_TKLESSEQ :
	//<expr> ::= <expr> 'tkLessEq' <add_expr>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_EXPR :
	//<expr> ::= <add_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIST_TKSQUAREOPEN_TKSQUARECLOSE :
	//<list> ::= 'tkSquareOpen' <list_elements> 'tkSquareClose'
{
		ArrayList list_elements = (ArrayList)LRParser.GetReductionSyntaxNode(1);
    //////////////////////////////////////////////////////////proverka na type
        if (list_elements != null)
        {
            ArrayList types = new ArrayList();
            for (int i = 0; i < list_elements.Count; i++)
            {
                string tp = "";
                if (list_elements[i] is new_expr)
                {
                    tp = ((string_const)((new_expr)list_elements[i]).params_list.expressions[1]).Value;
                    if (!types.Contains(tp))
                        types.Add(tp);
                }
            }
            if ((types.Count == 2 && !(types.Contains("integer") && types.Contains("real"))) || types.Count > 2)
            {
                //errors.Add(new PascalABCCompiler.Errors.UnexpectedToken(this, "одинаковый тип элементов списка"));
                errors.Add(new PascalABCCompiler.Errors.Error("элементы списка имеют разный тип"));
                return null;
            }
        }
    //////////////////////////////////////////////////////////
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype_list");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);
            expression_list el = new expression_list();
            if (list_elements == null)
                list_elements = new ArrayList();
            int32_const n = new int32_const(list_elements.Count);
            el.expressions.Add(n);
            if (list_elements.Count > 0)
            {
                el.expressions.Add(new string_const("list"));
            }
            else
                el.expressions.Add(new string_const("empty"));
            for (int i = 0; i < list_elements.Count;i++ )
                el.expressions.Add(list_elements[i] as expression);
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, el, false, null);
            parsertools.create_source_context(newexpr, new ident("new"), parsertools.sc_not_null(el, _named_type_reference1));
            return newexpr;
		}
	case (int)RuleConstants.RULE_LIST_TKSQUAREOPEN_TKDOT_TKDOT_TKSQUARECLOSE :
	//<list> ::= 'tkSquareOpen' <list_elements> 'tkDot' 'tkDot' <simple_expr> 'tkSquareClose'
{ArrayList list_elements = (ArrayList)LRParser.GetReductionSyntaxNode(1);
//////////////////////////////////////////////////////////proverka na type
ArrayList types = new ArrayList();
for (int i = 0; i < list_elements.Count; i++)
{
    if (list_elements[i] is new_expr)
    {
        string tp = ((string_const)((new_expr)list_elements[i]).params_list.expressions[1]).Value;
        if (!types.Contains(tp))
            types.Add(tp);
    }
}
if (LRParser.GetReductionSyntaxNode(4) is new_expr)
{
    new_expr ex_last = (new_expr)LRParser.GetReductionSyntaxNode(4);
    string tp_last = ((string_const)ex_last.params_list.expressions[1]).Value;
    if (!types.Contains(tp_last))
        types.Add(tp_last);
}
if ((types.Count == 2 && !(types.Contains("integer") && types.Contains("real"))) || types.Count > 2)
{
    //errors.Add(new PascalABCCompiler.Errors.UnexpectedToken(this, "одинаковый тип элементов списка"));
    errors.Add(new PascalABCCompiler.Errors.Error("элементы списка имеют разный тип"));
    return null;
}
if (list_elements.Count > 2)
{
    errors.Add(new PascalABCCompiler.Errors.UnexpectedToken(this, "."));
    return null;
}
//////////////////////////////////////////////////////////
    named_type_reference _named_type_reference1 = new named_type_reference();
		      ident idtype1 = new ident("datatype_list");
		      _named_type_reference1.source_context = idtype1.source_context;
		      _named_type_reference1.names.Add(idtype1);
			expression_list el = new expression_list();
            el.expressions.Add(list_elements[0] as expression);
            if (list_elements.Count>1)
                el.expressions.Add(list_elements[1] as expression);
            el.expressions.Add(LRParser.GetReductionSyntaxNode(4) as expression);
            el.expressions.Add(new string_const("list"));
	named_type_reference ntr = _named_type_reference1;
			new_expr newexpr = new new_expr(ntr, el, false, null);
			parsertools.create_source_context(newexpr, new ident("new"), parsertools.sc_not_null(el, _named_type_reference1));
			return newexpr;
											}
	case (int)RuleConstants.RULE_LIST_TKSQUAREOPEN_TKDOT_TKDOT_TKSQUARECLOSE2 :
	//<list> ::= 'tkSquareOpen' <list_elements> 'tkDot' 'tkDot' 'tkSquareClose'
{ArrayList list_elements = (ArrayList)LRParser.GetReductionSyntaxNode(1);
//////////////////////////////////////////////////////////proverka na type
ArrayList types = new ArrayList();
for (int i = 0; i < list_elements.Count; i++)
{
    if (list_elements[i] is new_expr) 
    {
        string tp = ((string_const)((new_expr)list_elements[i]).params_list.expressions[1]).Value;
        if (!types.Contains(tp))
            types.Add(tp);
    }
}
if (types.Count == 2 && !(types.Contains("integer") && types.Contains("real")))
{
    //errors.Add(new PascalABCCompiler.Errors.UnexpectedToken(this, "одинаковый тип элементов списка"));
    errors.Add(new PascalABCCompiler.Errors.Error("элементы списка имеют разный тип"));
    return null;
}
if (list_elements.Count > 2)
{
    errors.Add(new PascalABCCompiler.Errors.UnexpectedToken(this, "."));
    return null;
}
//////////////////////////////////////////////////////////
    named_type_reference _named_type_reference1 = new named_type_reference();
             ident idtype1 = new ident("datatype_list");
             _named_type_reference1.source_context = idtype1.source_context;
             _named_type_reference1.names.Add(idtype1);
             expression_list el = new expression_list();
             el.expressions.Add(list_elements[0] as expression);
             el.expressions.Add(list_elements[1] as expression);
             named_type_reference _named_type_reference11 = new named_type_reference();
             ident idtype11 = new ident("datatype");
             _named_type_reference11.names.Add(idtype11);
             expression_list ell = new expression_list();
             new_expr ee = list_elements[0] as new_expr;
             ell.expressions.Add(ee.params_list.expressions[0]);
             literal lt;
             string text = "nil";
             lt = new string_const(text);
             ell.expressions.Add(lt as expression);
             named_type_reference ntrr = _named_type_reference11; 
             new_expr newexprr = new new_expr(ntrr, ell, false, null);
             el.expressions.Add(newexprr);
             el.expressions.Add(new string_const("list"));
             named_type_reference ntr = _named_type_reference1;
             new_expr newexpr = new new_expr(ntr, el, false, null);
             parsertools.create_source_context(newexpr, new ident("new"), parsertools.sc_not_null(el, _named_type_reference1));
             return newexpr;}
    case (int)RuleConstants.RULE_LIST:
    //<list> ::= <list_constructor>
    return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIST_TKCOLON :
	//<list> ::= <simple_expr> 'tkColon' <empty> <simple_expr>
{expression_list el = new expression_list();
    el.expressions.Add(LRParser.GetReductionSyntaxNode(3) as expression);
    dot_node _dot_node = new dot_node(null, new ident("colon"));
    _dot_node.left = (addressed_value)LRParser.GetReductionSyntaxNode(0);
    method_call _method_call = new method_call(el);
    _method_call.dereferencing_value = (addressed_value)_dot_node;
    return _method_call;}
    case (int)RuleConstants.RULE_LIST_CONSTRUCTOR_TKSQUAREOPEN_TKSPLIT_TKSQUARECLOSE:
//<list_constructor> ::= 'tkSquareOpen' <simple_expr> 'tkSplit' <generators> <conditions_comma> 'tkSquareClose'
{
    ArrayList generators=(ArrayList)LRParser.GetReductionSyntaxNode(3);
    ArrayList conditions = (ArrayList)LRParser.GetReductionSyntaxNode(4);
    //////////////////////////////////////////////////////////////////////////
    statement_list body = new statement_list();
    rec_num = 0;
    for (int i = 0; i < generators.Count; i++)
    {
        ident id = null;
        if (((ArrayList)generators[i])[0] is new_expr)
        {
            constructor_rec_var(body, ((ArrayList)generators[i])[0] as new_expr);
        }
        else
        {
            id = (ident)((ArrayList)generators[i])[0];
            ident_list _ident_list1 = new ident_list();
            _ident_list1.idents.Add(id);
            string name_param2 = id.name;
            typed_parameters _typed_parametres2 = null;
            named_type_reference _named_type_reference3 = new named_type_reference();
            ident idtype3 = new ident("datatype");
            _named_type_reference3.names.Add(idtype3);
            var_def_statement _var_def_statement2 = new var_def_statement(_ident_list1, (type_definition)_named_type_reference3, null, definition_attribute.None, false);
            var_statement _var_statement2 = new var_statement(_var_def_statement2);
            body.subnodes.Add(_var_statement2);
        }
    }
    ///////////////////////////////////////////////////////////////////////result
    named_type_reference _named_type_reference2 = new named_type_reference();
    ident idtype2 = new ident("datatype_list");
    _named_type_reference2.names.Add(idtype2);
    expression_list el2 = new expression_list();
    int32_const n = new int32_const(0);
    el2.expressions.Add(n);
    el2.expressions.Add(new string_const("list"));
    named_type_reference ntr = _named_type_reference2;
    new_expr newexpr = new new_expr(ntr, el2, false, null);
    assign _result = new assign();
    _result.operator_type = Operators.Assignment;
    _result.to = new ident("result");
    _result.from = newexpr;
    body.subnodes.Add(_result);
    //////////////////////////////////////////////////////////////////////if
    if_node _if_node = new if_node();
    if (conditions != null)
    {
        expression b_e = (expression)conditions[conditions.Count - 1];
        for (int i = conditions.Count - 2; i >= 0; i--)
        {
            bin_expr b_e1 = new bin_expr();
            b_e1.operation_type = Operators.LogicalAND;
            b_e1.left = (expression)conditions[i];
            b_e1.right = b_e;
            b_e = b_e1;
        }
        _if_node.condition = _ob(b_e);
    }
    //////////////////////////////////////////////////////////////////////assign
    statement_list stmt_list=new statement_list();
    rec_num = 0;
    for (int i = 0; i < generators.Count; i++)
    {
        if (((ArrayList)generators[i])[0] is new_expr)
        {
            constructor_rec_ass(stmt_list, ((ArrayList)generators[i])[0] as new_expr, _index((expression)((ArrayList)generators[i])[1], new ident("$i" + i)));
        }
        else
        {
            assign _assign = new assign();
            _assign.operator_type = Operators.Assignment;
            _assign.to = (ident)((ArrayList)generators[i])[0];
            _assign.from = _index((expression)((ArrayList)generators[i])[1], new ident("$i" + i));
            stmt_list.subnodes.Add(_assign);
        }
    }
    if (conditions != null)
    {
        _if_node.then_body = _add(new ident("result"), (expression)LRParser.GetReductionSyntaxNode(1));
        stmt_list.subnodes.Add(_if_node);
    }
    else
        stmt_list.subnodes.Add(_add(new ident("result"), (expression)LRParser.GetReductionSyntaxNode(1)));
    //////////////////////////////////////////////////////////////////////for
    statement _for_stmt = stmt_list;
    for (int i = generators.Count - 1; i >= 0; i--)
    {
        for_node _for_node = new for_node();
        _for_node.cycle_type = for_cycle_type.to;
        _for_node.loop_variable = new ident("$i"+i);
        named_type_reference _named_type_reference = new named_type_reference();
        ident idtype = new ident("integer");
        _named_type_reference.names.Add(idtype);
        _for_node.type_name = (type_definition)_named_type_reference;
        _for_node.initial_value = new int32_const(0);
        _for_node.finish_value = _count((expression)((ArrayList)generators[i])[1]);
        _for_node.increment_value = new int32_const(1);
        _for_node.statements = _for_stmt;
        _for_stmt = _for_node;
    }
    body.subnodes.Add(_for_stmt);
    ///////////////////////////////////////////////////////////////////////////////////////////////////////lambda
    formal_parameters _formal_parametres = null;
    //////////////////////////
    named_type_reference _named_type_reference1 = new named_type_reference();
    ident idtype1 = new ident("datatype");
    _named_type_reference1.source_context = idtype1.source_context;
    _named_type_reference1.names.Add(idtype1);
    /////////////////////////////
    lambda_num++;
    function_lambda_definition _procedure_definition = new function_lambda_definition();
    _procedure_definition.formal_parameters = _formal_parametres;
    _procedure_definition.return_type = (type_definition)_named_type_reference1;
    _procedure_definition.ident_list = null;
    _procedure_definition.proc_body = null;
    _procedure_definition.parameters = null;
    _procedure_definition.lambda_name = "__lambda__" + lambda_num;
    //new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, null, null, null, "lambda" + lambda_num);
    _procedure_definition.proc_body = body;
    procedure_definition pr = lambda(_procedure_definition);
    //_function_lambda_definitions.Add(_procedure_definition);////////////////
    let_funcs.Add(pr);
    let_where_funcs.Add(pr);
    if (let_flag.Count > 0)
        let_func_last.Add(pr);
    expression_list el = new expression_list();
    let_fact_params.Clear();
    return new ident(_procedure_definition.lambda_name);
}
    case (int)RuleConstants.RULE_GENERATORS:
        //<generators> ::= <generator> <empty>
        {
            ArrayList ar = new ArrayList();
            ar.Add(LRParser.GetReductionSyntaxNode(0));
            return ar;
        }
    case (int)RuleConstants.RULE_GENERATORS_TKCOMMA:
        //<generators> ::= <generators> 'tkComma' <generator>
        {
            ArrayList ar = (ArrayList)LRParser.GetReductionSyntaxNode(0);
            ar.Add(LRParser.GetReductionSyntaxNode(2));
            return ar;
        }
    case (int)RuleConstants.RULE_GENERATOR_TKARROWGEN:
        //<generator> ::= <simple_expr> 'tkArrowGen' <simple_expr>
        {
            ArrayList ar = new ArrayList();
            ar.Add(LRParser.GetReductionSyntaxNode(0));
            ar.Add(LRParser.GetReductionSyntaxNode(2));
            return ar;
        }
    case (int)RuleConstants.RULE_CONDITIONS_COMMA:
        //<conditions_comma> ::= <empty>
        {
            return null;
        }
    case (int)RuleConstants.RULE_CONDITIONS_COMMA_TKCOMMA:
        //<conditions_comma> ::= 'tkComma' <conditions>
        {
            return LRParser.GetReductionSyntaxNode(1);
        }
    case (int)RuleConstants.RULE_CONDITIONS:
        //<conditions> ::= <condition> <empty>
        {
            ArrayList ar = new ArrayList();
            ar.Add(LRParser.GetReductionSyntaxNode(0));
            return ar;
        }
    case (int)RuleConstants.RULE_CONDITIONS_TKCOMMA:
        //<conditions> ::= <conditions> 'tkComma' <condition>
        {
            ArrayList ar = (ArrayList)LRParser.GetReductionSyntaxNode(0);
            ar.Add(LRParser.GetReductionSyntaxNode(2));
            return ar;
        }
    case (int)RuleConstants.RULE_CONDITION:
        //<condition> ::= <expr>
        return LRParser.GetReductionSyntaxNode(0);
    case (int)RuleConstants.RULE_CORTEG_TKROUNDOPEN_TKCOMMA_TKROUNDCLOSE:
//<corteg> ::= 'tkRoundOpen' <simple_expr> 'tkComma' <list_elements> 'tkRoundClose'
        {
            //new_expr first_el = (new_expr)LRParser.GetReductionSyntaxNode(1);
            ArrayList list_elements = (ArrayList)LRParser.GetReductionSyntaxNode(3);
            list_elements.Insert(0, LRParser.GetReductionSyntaxNode(1));
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype_list");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);
            expression_list el = new expression_list();
            if (list_elements == null)
                list_elements = new ArrayList();
            int32_const n = new int32_const(list_elements.Count);
            el.expressions.Add(n);
            if (list_elements.Count > 0)
            {
                el.expressions.Add(new string_const("corteg"));
            }
            else
                el.expressions.Add(new string_const("empty"));
            for (int i = 0; i < list_elements.Count; i++)
                el.expressions.Add(list_elements[i] as expression);
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, el, false, null);
            parsertools.create_source_context(newexpr, new ident("new"), parsertools.sc_not_null(el, _named_type_reference1));
            return newexpr;
        }
	case (int)RuleConstants.RULE_LIST_ELEMENTS :
	//<list_elements> ::= <empty>
return null;
	case (int)RuleConstants.RULE_LIST_ELEMENTS2 :
	//<list_elements> ::= <simple_expr> <empty>
{
                 ArrayList list_elements = new ArrayList();
                 list_elements.Add(LRParser.GetReductionSyntaxNode(0));
                 return list_elements;
             }
	case (int)RuleConstants.RULE_LIST_ELEMENTS_TKCOMMA :
	//<list_elements> ::= <list_elements> 'tkComma' <simple_expr>
{
                 ((ArrayList)LRParser.GetReductionSyntaxNode(0)).Add(LRParser.GetReductionSyntaxNode(2));
                 return LRParser.GetReductionSyntaxNode(0);
             }
	case (int)RuleConstants.RULE_ADD_EXPR_TKAND :
	//<add_expr> ::= <add_expr> 'tkAnd' <mult_expr>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_ADD_EXPR :
	//<add_expr> ::= <add_expr> <addop> <mult_expr>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_ADD_EXPR2 :
	//<add_expr> ::= <mult_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_MULT_EXPR_TKOR :
	//<mult_expr> ::= <mult_expr> 'tkOr' <negate_expr>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_MULT_EXPR :
	//<mult_expr> ::= <mult_expr> <multop> <negate_expr>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_MULT_EXPR2 :
	//<mult_expr> ::= <negate_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NEGATE_EXPR_TKMINUS :
	//<negate_expr> ::= 'tkMinus' <simple_expr>
         
		{
			un_expr _un_expr=new un_expr(LRParser.GetReductionSyntaxNode(1) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(0)).type);
			parsertools.create_source_context(_un_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			
			return _un_expr;
		}

	case (int)RuleConstants.RULE_NEGATE_EXPR_TKNOT :
	//<negate_expr> ::= 'tkNot' <simple_expr>
         
		{
			un_expr _un_expr=new un_expr(LRParser.GetReductionSyntaxNode(1) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(0)).type);
			parsertools.create_source_context(_un_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			
			return _un_expr;
		}

	case (int)RuleConstants.RULE_NEGATE_EXPR :
	//<negate_expr> ::= <simple_expr>
return LRParser.GetReductionSyntaxNode(0);
    case (int)RuleConstants.RULE_SIMPLE_EXPR:
//<simple_expr> ::= <simple_type_expr>
return LRParser.GetReductionSyntaxNode(0);
    case (int)RuleConstants.RULE_SIMPLE_TYPE_EXPR_TKINT:
//<simple_type_expr> ::= 'tkInt'

{
            named_type_reference _named_type_reference1 = new named_type_reference();

            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);

            /////
            expression_list el = new expression_list();
            el.expressions.Add(LRParser.GetReductionSyntaxNode(0) as expression);
            literal lt;
            string text = "integer";
            lt = new string_const(text);
            el.expressions.Add(lt as expression);
            /////
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, el, false,null);
            parsertools.create_source_context(newexpr, new ident("new"), parsertools.sc_not_null(el, _named_type_reference1));
            return newexpr;
        }
    case (int)RuleConstants.RULE_SIMPLE_TYPE_EXPR_TKDOUBLE:
//<simple_type_expr> ::= 'tkDouble'
{
            named_type_reference _named_type_reference1 = new named_type_reference();

            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);

            /////
            expression_list el = new expression_list();
            el.expressions.Add(LRParser.GetReductionSyntaxNode(0) as expression);
            literal lt;
            string text = "real";
            lt = new string_const(text);
            el.expressions.Add(lt as expression);
            /////
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, el, false,null);
            parsertools.create_source_context(newexpr, new ident("new"), parsertools.sc_not_null(el, _named_type_reference1));
            return newexpr;
        }
    case (int)RuleConstants.RULE_SIMPLE_TYPE_EXPR_TKBOOL:
//<simple_type_expr> ::= 'tkBool'
{
            named_type_reference _named_type_reference1 = new named_type_reference();

            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);

            /////
            expression_list el = new expression_list();
            el.expressions.Add(LRParser.GetReductionSyntaxNode(0) as expression);
            literal lt;
            string text = "boolean";
            lt = new string_const(text);
            el.expressions.Add(lt as expression);
            /////
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, el, false,null);
            parsertools.create_source_context(newexpr, new ident("new"), parsertools.sc_not_null(el, _named_type_reference1));
            return newexpr;
        }
    case (int)RuleConstants.RULE_SIMPLE_TYPE_EXPR_TKCHAR:
//<simple_type_expr> ::= 'tkChar'
{
            named_type_reference _named_type_reference1 = new named_type_reference();

            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);

            /////
            expression_list el = new expression_list();
            el.expressions.Add(LRParser.GetReductionSyntaxNode(0) as expression);
            literal lt;
            string text = "char";
            lt = new string_const(text);
            el.expressions.Add(lt as expression);
            /////
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, el, false,null);
            parsertools.create_source_context(newexpr, new ident("new"), parsertools.sc_not_null(el, _named_type_reference1));
            return newexpr;
        }
    case (int)RuleConstants.RULE_SIMPLE_TYPE_EXPR_TKSTRING:
//<simple_type_expr> ::= 'tkString'
{
            named_type_reference _named_type_reference1 = new named_type_reference();

            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);

            /////
            expression_list el = new expression_list();
            el.expressions.Add(LRParser.GetReductionSyntaxNode(0) as expression);
            literal lt;
            string text = "string";
            lt = new string_const(text);
            el.expressions.Add(lt as expression);
            /////
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, el, false,null);
            parsertools.create_source_context(newexpr, new ident("new"), parsertools.sc_not_null(el, _named_type_reference1));
            return newexpr;
        }
	case (int)RuleConstants.RULE_SIMPLE_EXPR_TKROUNDOPEN_TKROUNDCLOSE :
	//<simple_expr> ::= 'tkRoundOpen' <expr> 'tkRoundClose'
return LRParser.GetReductionSyntaxNode(1);
    case (int)RuleConstants.RULE_SIMPLE_EXPR2:
//<simple_expr> ::= <infix_expr> <empty>
{
    if (LRParser.GetReductionSyntaxNode(0) is ArrayList)
    {
        ////////////////////////////////////////////////////////ident_params
        ArrayList arr = (ArrayList)LRParser.GetReductionSyntaxNode(0);
        ident _ident = (ident)arr[0];
        expression_list el = new expression_list();
        for (int ii = 1; ii < arr.Count; ii++)
            el.expressions.Add((expression)arr[ii]);
        ////////////////////////////////////////////////////////
        //expression_list el = LRParser.GetReductionSyntaxNode(2) as expression_list;
        ////////////////////////////for curring
        int n = el.expressions.Count;
        int fld_params_count = find_count_params(_ident.name);
        ////////////////////////////
        if (fld_params_count - n > 0)
        {
            expression_list _expression_list = el;
            expression_list _params = new expression_list();
            expression_list _params_el = new expression_list();
            for (int i = 0; i < list_params1.Count; i++)
                if (list_params1[i] is ident)
                {
                    _params.expressions.Add(new ident(((ident)list_params1[i]).name));
                    _params_el.expressions.Add(new ident(((ident)list_params1[i]).name));
                }
            for (int i = 1; i <= fld_params_count - n; i++)
                _expression_list.expressions.Add(new ident("$$" + i));
            for (int i = 1; i <= fld_params_count - n; i++)
            {
                _params.expressions.Add(new ident("$$" + i));
                _params_el.expressions.Add(new ident("$$" + i));
            }
            method_call _method_call = new method_call(_expression_list);
            if (_method_call is dereference)
                ((dereference)_method_call).dereferencing_value = (addressed_value)_ident;

            op_type_node _op_type_node = new op_type_node(Operators.Assignment);
            assign _assign = new assign((addressed_value)new ident("result"), _method_call as expression, _op_type_node.type);

            statement_list _statement_list = new statement_list();
            _statement_list.subnodes.Add(_assign);
            ////////////////////////////
            ident_list i_l = new ident_list();
            for (int i = 1; i <= fld_params_count - n; i++)
                i_l.idents.Add(new ident("$$" + i));
            /////////////////////////////
            formal_parameters _formal_parametres = new formal_parameters();
            for (int i = 0; i < _params.expressions.Count; i++)
            {
                ident_list _ident_list = new ident_list();
                ident id = (ident)_params.expressions[i];
                _ident_list.idents.Add(id);
                named_type_reference _named_type_reference = new named_type_reference();

                ident idtype = new ident("datatype");
                _named_type_reference.names.Add(idtype);

                typed_parameters _typed_parametres = new typed_parameters(_ident_list, (type_definition)_named_type_reference, parametr_kind.none, null);
                parsertools.create_source_context(_typed_parametres, _ident_list, _named_type_reference);

                _formal_parametres.params_list.Add(_typed_parametres);
            }
            //////////////////////////
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);
            /////////////////////////////
            lambda_num++;
            function_lambda_definition _procedure_definition = new function_lambda_definition();
            _procedure_definition.formal_parameters = _formal_parametres;
            _procedure_definition.return_type = (type_definition)_named_type_reference1;
            _procedure_definition.ident_list = i_l;
            _procedure_definition.proc_body = null;
            _procedure_definition.parameters = _params_el;
            _procedure_definition.lambda_name = "__lambda__" + lambda_num;
            //new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, i_l, null, _params_el, "lambda" + lambda_num);
            object rt = _expression_list;
            _procedure_definition.proc_body = _statement_list;
            _function_lambda_definitions_after.Add(_procedure_definition);
            _function_lambda_definitions_after.Add(_ident.name);////////////////
            last_function_lambda_definitions.Add(_procedure_definition);
            return new ident(_procedure_definition.lambda_name);
        }
        else
        {
            method_call _method_call = new method_call(el);

            if (_method_call is dereference)
            {
                ((dereference)_method_call).dereferencing_value = (addressed_value)_ident;
                parsertools.create_source_context(_method_call, _ident, _method_call);
            }
            list_method_calls.Add(_method_call);
            last_list_method_calls.Add(_method_call);
            last_list_method_calls_lambda.Add(_method_call);
            return _method_call;

        }
    }
    else
        return LRParser.GetReductionSyntaxNode(0);
}
	case (int)RuleConstants.RULE_SIMPLE_EXPR3 :
	//<simple_expr> ::= <variable>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_EXPR_TKLET_TKIN :
	//<simple_expr> ::= 'tkLet' <def_vars> 'tkIn' <body_func>
{//////////////////////////
    expression_list _expression_list = (expression_list)LRParser.GetReductionSyntaxNode(1);
    statement_list _statement_list = (statement_list)LRParser.GetReductionSyntaxNode(3);
    //////////////////////////
    formal_parameters _formal_parametres = null;
    //////////////////////////
    named_type_reference _named_type_reference1 = new named_type_reference();
    ident idtype1 = new ident("datatype");
    _named_type_reference1.source_context = idtype1.source_context;
    _named_type_reference1.names.Add(idtype1);
    /////////////////////////////
    lambda_num++;
    function_lambda_definition _procedure_definition = new function_lambda_definition();
    _procedure_definition.formal_parameters = _formal_parametres;
    _procedure_definition.return_type = (type_definition)_named_type_reference1;
    _procedure_definition.ident_list = null;
    _procedure_definition.proc_body = null;
    _procedure_definition.parameters = _expression_list;
    _procedure_definition.lambda_name = "__lambda__" + lambda_num;
    //new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, null, null, _expression_list, "lambda" + lambda_num);
    object rt = _expression_list;
    _procedure_definition.proc_body = _statement_list;
    parsertools.create_source_context(_procedure_definition, _expression_list, rt);
    procedure_definition pr = lambda(_procedure_definition);
    //_function_lambda_definitions.Add(_procedure_definition);////////////////
    ((block)pr.proc_body).defs = new declarations();
    int start=0;
    int k = let_funcs.Count-1;
    while (k>0 && ((procedure_definition)let_funcs[k]).proc_header.name.meth_name.name.Contains("__lambda__"))
        k--;
    int kk = 0;
    while (k > 0 && kk < _expression_list.expressions.Count-1 && !((procedure_definition)let_funcs[k]).proc_header.name.meth_name.name.Contains("__lambda__"))
    {
        k--;
        kk++;
    }
    start = k;
    int i = start;
    int n = let_funcs.Count;
    while (i < n)
    {
        if (let_where_funcs.Count > 0)
            if (start - (let_funcs.Count - let_where_funcs.Count) >= 0)
                let_where_funcs.RemoveAt(start - (let_funcs.Count - let_where_funcs.Count));
            else
                let_where_funcs.RemoveAt(let_where_funcs.Count - 1);
        ((block)pr.proc_body).defs.defs.Add((procedure_definition)let_funcs[start]);
        let_funcs.RemoveAt(start);
        i++;
    }
    ///////////////////////////lambda
    if (((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count > 0)
    {
        if (((block)pr.proc_body).defs==null)
            ((block)pr.proc_body).defs = new declarations();
        for (int ii = 0; ii < ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count; ii++)
            ((block)pr.proc_body).defs.defs.Add((procedure_definition)((ArrayList)lambda_stack[lambda_stack.Count - 1])[ii]);
        for (int ii = 0; ii < ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count; ii++)
            _function_lambda_definitions.RemoveAt(_function_lambda_definitions.Count-1);
    }
    lambda_stack.RemoveAt(lambda_stack.Count - 1);
    //lambda_stack.RemoveAt(lambda_stack.Count - 1);
    /*if (((block)pr.proc_body).defs == null)
        ((block)pr.proc_body).defs = new declarations();
    for (int ii = 0; ii < _function_lambda_definitions.Count; ii++)
        ((block)pr.proc_body).defs.defs.Add(lambda((function_lambda_definition)_function_lambda_definitions[ii]));
    _function_lambda_definitions.Clear();*/
    ///////////////////////////
    let_funcs.Add(pr);
    let_where_funcs.Add(pr);
    if (let_flag.Count>0)
        let_func_last.Add(pr);
    expression_list el = new expression_list();
    let_fact_params.Clear();
    /////////////////////
    if (let_stack.Count > 0)
        ((ArrayList)let_stack[let_stack.Count - 1]).Add(pr);
    /*else
    {
        let_funcs.Add(pr);
        let_where_funcs.Add(pr);
    }*/
    /////////////////////
    return new ident(_procedure_definition.lambda_name);
}
	case (int)RuleConstants.RULE_SIMPLE_EXPR4 :
	//<simple_expr> ::= <list>
return LRParser.GetReductionSyntaxNode(0);
    case (int)RuleConstants.RULE_SIMPLE_EXPR5:
    //<simple_expr> ::= <corteg>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_EXPR6 :
	//<simple_expr> ::= <lambda_func> <empty>
return LRParser.GetReductionSyntaxNode(0);
	//case (int)RuleConstants.RULE_SIMPLE_EXPR_TKROUNDOPEN_TKROUNDCLOSE_TKROUNDOPEN_TKROUNDCLOSE :
	//<simple_expr> ::= 'tkRoundOpen' <lambda_func> 'tkRoundClose' 'tkRoundOpen' <params_value> 'tkRoundClose'
    case (int)RuleConstants.RULE_SIMPLE_EXPR_TKROUNDOPEN_TKROUNDOPEN_TKROUNDCLOSE_TKROUNDCLOSE:
//<simple_expr> ::= 'tkRoundOpen' 'tkRoundOpen' <lambda_func> 'tkRoundClose' <params_value> 'tkRoundClose'
{
            function_lambda_definition fld = find_func_lambda_name(((ident)LRParser.GetReductionSyntaxNode(2)).name);
            //function_lambda_definition fld = find_lambda_funcs(((ident)LRParser.GetReductionSyntaxNode(1)).name);
            //lambda_funcs.RemoveAt(lambda_funcs.Count-1);
            expression_list _expression_list = (expression_list)LRParser.GetReductionSyntaxNode(4);
            function_lambda_definition _lambda_definition = fld;
            function_lambda_call _lambda_call = new function_lambda_call(_lambda_definition, _expression_list);
            return _lambda_call;
        }
	/*case (int)RuleConstants.RULE_SIMPLE_EXPR_TKROUNDOPEN_TKROUNDCLOSE2 :
	//<simple_expr> ::= <simple_expr> 'tkRoundOpen' <params_value> 'tkRoundClose'
{
		if (LRParser.GetReductionSyntaxNode(0) is function_lambda_call)
            {
                expression_list _expression_list = new expression_list();
                expression_list el_new = (expression_list)LRParser.GetReductionSyntaxNode(2);
                function_lambda_call _lambda_definition_call = (function_lambda_call)LRParser.GetReductionSyntaxNode(0);
                function_lambda_definition fld = _lambda_definition_call.f_lambda_def;
                for (int i = 0; i < _lambda_definition_call.parameters.expressions.Count; i++)
                    _expression_list.expressions.Add(_lambda_definition_call.parameters.expressions[i]);
                for (int i = 0; i < el_new.expressions.Count; i++)
                    _expression_list.expressions.Add(el_new.expressions[i]);
                function_lambda_call _lambda_call = new function_lambda_call(fld, _expression_list);
                return _lambda_call;
            }
            else
                return null;}*/
	case (int)RuleConstants.RULE_VARIABLE_EXPR :
	//<variable_expr> ::= <simple_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_DEF_VARS :
	//<def_vars> ::= <def_var> <empty>
{
            expression_list el=new expression_list();
            el.expressions.Add(LRParser.GetReductionSyntaxNode(0) as expression);
            return el;
        }
	case (int)RuleConstants.RULE_DEF_VARS_TKSEMICOLON :
	//<def_vars> ::= <def_vars> 'tkSemiColon' <def_var>
{
            expression_list el=(expression_list)LRParser.GetReductionSyntaxNode(0);
            el.expressions.Add(LRParser.GetReductionSyntaxNode(2) as expression);
            return el;

        }
    case (int)RuleConstants.RULE_DEF_VAR_TKIDENT:
//<def_var> ::= 'tkIdent' <params> <guard_body_list> <where_var>
{
    expression_list _expression_list = (expression_list)LRParser.GetReductionSyntaxNode(1);
    /*statement_list _statement_list = (statement_list)LRParser.GetReductionSyntaxNode(4);*/
    statement_list _statement_list = new statement_list();
    ArrayList ar_main = (ArrayList)LRParser.GetReductionSyntaxNode(2);
    ////////////////////////////for list
    statement_list stmt_l = new statement_list();
    int start = let_where_list_params.Count;
    if (_expression_list != null)
        start = let_where_list_params.Count - _expression_list.expressions.Count;
    int i_ex = 0;
    for (int ll = start; ll < let_where_list_params.Count; ll++)
    {
        for (int l = 0; l < ((ArrayList)let_where_list_params[ll]).Count; l++)
        {
            ident_list il1 = new ident_list();
            il1.idents.Add(new ident(((ident)((ArrayList)let_where_list_params[ll])[l]).name));
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype");
            _named_type_reference1.names.Add(idtype1);

            var_def_statement _var_def_statement = new var_def_statement(il1, (type_definition)_named_type_reference1, null, definition_attribute.None, false);
            var_statement _var_statement = new var_statement(_var_def_statement);
            stmt_l.subnodes.Add(_var_statement);
        }
        if (((ArrayList)let_where_list_params[ll]).Count > 0)
        {
            op_type_node _op_type_node = new op_type_node(Operators.Assignment);
            dot_node _dot_node = new dot_node(null, (addressed_value)(new ident("head")));
            ident id = _expression_list.expressions[i_ex] as ident;
            /*for (int i1 = 0; i1 < ((ArrayList)let_where_list_params[ll]).Count; i1++)
                id.name += ((ident)((ArrayList)let_where_list_params[ll])[i1]).name;*/
            if (id.name != null)
            {
                _dot_node.left = (addressed_value)(id);
                object o = null;
                method_call _method_call = new method_call(o as expression_list);
                if (_method_call is dereference)
                    ((dereference)_method_call).dereferencing_value = (addressed_value)_dot_node;
                assign _assign1 = new assign(((ident)((ArrayList)let_where_list_params[ll])[((ArrayList)let_where_list_params[ll]).Count - 1]) as addressed_value, _method_call as expression, _op_type_node.type);
                stmt_l.subnodes.Add(_assign1);

                ///
                for (int i1 = ((ArrayList)let_where_list_params[ll]).Count - 2; i1 > 0; i1--)
                {
                    _dot_node = new dot_node((addressed_value)id, (addressed_value)(new ident("tail")));
                    for (int j1 = 0; j1 < ((ArrayList)let_where_list_params[ll]).Count - 2 - i1; j1++)
                    {
                        _dot_node = new dot_node(_dot_node, (addressed_value)(new ident("tail")));
                    }
                    _dot_node = new dot_node(_dot_node, (addressed_value)(new ident("head")));

                    _method_call = new method_call(o as expression_list);
                    if (_method_call is dereference)
                        ((dereference)_method_call).dereferencing_value = (addressed_value)_dot_node;
                    _assign1 = new assign(((ident)((ArrayList)let_where_list_params[ll])[i1]) as addressed_value, _method_call as expression, _op_type_node.type);
                    stmt_l.subnodes.Add(_assign1);
                }

                _dot_node = new dot_node((addressed_value)id, (addressed_value)(new ident("tail")));

                for (int j1 = 0; j1 < ((ArrayList)let_where_list_params[ll]).Count - 2; j1++)
                {
                    _dot_node = new dot_node(_dot_node, (addressed_value)(new ident("tail")));
                }
                _method_call = new method_call(o as expression_list);
                if (_method_call is dereference)
                    ((dereference)_method_call).dereferencing_value = (addressed_value)_dot_node;
                _assign1 = new assign(((ident)((ArrayList)let_where_list_params[ll])[0]) as addressed_value, _method_call as expression, _op_type_node.type);
                stmt_l.subnodes.Add(_assign1);
            }
        }
        i_ex++;
    }
    for (int ll = 0; ll < let_where_list_params.Count - start; ll++)
        list_params_temp.RemoveAt(list_params_temp.Count - 1);
    for (int i = stmt_l.subnodes.Count - 1; i >= 0; i--)
        _statement_list.subnodes.Insert(0, stmt_l.subnodes[i]);
    if (_expression_list != null)
        for (int i = 0; i < _expression_list.expressions.Count; i++)
            let_where_list_params.RemoveAt(let_where_list_params.Count - 1);
    ////////////////////////////
    formal_parameters _formal_parametres = null;
    ident_list i_l = new ident_list();
    if (_expression_list != null)
    {
        for (int i = 0; i < _expression_list.expressions.Count; i++)
            i_l.idents.Add((ident)_expression_list.expressions[i]);
        /////////////////////////////
        _formal_parametres = new formal_parameters();
        for (int i = 0; i < i_l.idents.Count; i++)
        {
            ident_list _ident_list = new ident_list();
            ident id = (ident)_expression_list.expressions[i];
            _ident_list.idents.Add(id);
            string name_param = id.name;
            typed_parameters _typed_parametres = null;
            int k = 0;
            while (k < last_list_method_calls_lambda.Count && ((ident)((method_call)last_list_method_calls_lambda[k]).dereferencing_value).name != name_param)
                k++;
            if (k < last_list_method_calls_lambda.Count)
                _typed_parametres = new typed_parameters(_ident_list, func_type(((method_call)last_list_method_calls_lambda[k]).parameters.expressions.Count), parametr_kind.none, null);
            else
            {
                named_type_reference _named_type_reference = new named_type_reference();

                ident idtype = new ident("datatype");
                _named_type_reference.names.Add(idtype);

                _typed_parametres = new typed_parameters(_ident_list, (type_definition)_named_type_reference, parametr_kind.none, null);
                parsertools.create_source_context(_typed_parametres, _ident_list, _named_type_reference);
            }
            _formal_parametres.params_list.Add(_typed_parametres);
        }
    }
    //////////////////////////
    named_type_reference _n_t_r = new named_type_reference();
    ident it = new ident("datatype");
    _n_t_r.names.Add(it);
    /////////////////////////////
    //lambda_num++;
    function_lambda_definition _procedure_definition = new function_lambda_definition();
    _procedure_definition.formal_parameters = _formal_parametres;
    _procedure_definition.return_type = (type_definition)_n_t_r;
    _procedure_definition.ident_list = i_l;
    _procedure_definition.proc_body = null;
    _procedure_definition.parameters = _expression_list;
    _procedure_definition.lambda_name = ((ident)LRParser.GetReductionSyntaxNode(0)).name;
    //new function_lambda_definition(_formal_parametres, (type_definition)_n_t_r, i_l, null, _expression_list, ((ident)LRParser.GetReductionSyntaxNode(0)).name);
    object rt = _expression_list;
    ////////////////////////////////////////////////for guard
    for (int i = 0; i < ar_main.Count;i++ )
        if (((ArrayList)ar_main[i])[0] != null)
        {
            if_node _if_node = new if_node();
            _if_node.condition = _ob((expression)((ArrayList)ar_main[i])[0]);
            _if_node.then_body = new statement_list();
            _if_node.then_body = (statement)((ArrayList)ar_main[i])[1];
            procedure_call _exit = new procedure_call(new ident("exit") as addressed_value);
            ((statement_list)_if_node.then_body).subnodes.Add(_exit);
            _statement_list.subnodes.Add(_if_node);
        }
        else
        {
            _statement_list.subnodes.Add((statement)((ArrayList)ar_main[i])[1]);
            procedure_call _exit = new procedure_call(new ident("exit") as addressed_value);
            _statement_list.subnodes.Add(_exit);
        }
    ////////////////////////////////////////////////
    _procedure_definition.proc_body = _statement_list;
    parsertools.create_source_context(_procedure_definition, _expression_list, rt);
    procedure_definition pr = lambda(_procedure_definition);
    ///////////////////////////
    /*if (let_func_last.Count > 0)
    {
        ((block)pr.proc_body).defs = new declarations();
        for (int i = 0; i < let_func_last.Count; i++)
            ((block)pr.proc_body).defs.defs.Add((procedure_definition)let_func_last[i]);
        for (int i = 0; i < let_func_last.Count; i++)
            if (let_where_funcs.Count > 0)
                let_where_funcs.RemoveAt(let_where_funcs.Count - 1);
        for (int i = 0; i < let_func_last.Count; i++)
            let_funcs.RemoveAt(let_funcs.Count - 1);
        let_func_last.Clear();
        let_flag.RemoveAt(let_flag.Count - 1);
    }
    else
        if ((let_flag.Count > 0 && _expression_list != null) || let_funcs.Count == 0)
            let_flag.Clear();*/
    if (((ArrayList)let_stack[let_stack.Count - 1]).Count > 0)
    {
        ((block)pr.proc_body).defs = new declarations();
        for (int i = 0; i < ((ArrayList)let_stack[let_stack.Count - 1]).Count; i++)
            ((block)pr.proc_body).defs.defs.Add((procedure_definition)((ArrayList)let_stack[let_stack.Count - 1])[i]);
        for (int i = 0; i < ((ArrayList)let_stack[let_stack.Count - 1]).Count; i++)
            if (let_where_funcs.Count > 0)
                let_where_funcs.RemoveAt(let_where_funcs.Count - 1);
        for (int i = 0; i < ((ArrayList)let_stack[let_stack.Count - 1]).Count; i++)
            if (let_funcs.Count > 0)
                let_funcs.RemoveAt(let_funcs.Count - 1);
        let_func_last.Clear();
        if (let_flag.Count>0)
            let_flag.RemoveAt(let_flag.Count - 1);
    }
    let_stack.RemoveAt(let_stack.Count - 1);
    ///////////////////////////////////////////////////////////////////
    if (LRParser.GetReductionSyntaxNode(3) != null)
    {
        if (((block)pr.proc_body).defs == null)
            ((block)pr.proc_body).defs = new declarations();
        for (int j = 0; j < ((ArrayList)LRParser.GetReductionSyntaxNode(3)).Count; j++)
            ((block)pr.proc_body).defs.defs.Add((procedure_definition)((ArrayList)LRParser.GetReductionSyntaxNode(3))[j]);
        ArrayList ar = ((ArrayList)let_where_funcs_main[let_where_funcs_main.Count - 1]).Clone() as ArrayList;
        for (int j = 0; j < ar.Count; j++)
            last_where_funcs.Add(ar[j]);
        let_where_funcs_main.RemoveAt(let_where_funcs_main.Count - 1);
    }
    ///////////////////////////lambda
    if (((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count > 0)
    {
        if (((block)pr.proc_body).defs == null)
            ((block)pr.proc_body).defs = new declarations();
        for (int i = 0; i < ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count; i++)
            ((block)pr.proc_body).defs.defs.Add((procedure_definition)((ArrayList)lambda_stack[lambda_stack.Count - 1])[i]);
        for (int ii = 0; ii < ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count; ii++)
            _function_lambda_definitions.RemoveAt(_function_lambda_definitions.Count - 1);
    }
    lambda_stack.RemoveAt(lambda_stack.Count - 1);
    ///////////////////////////
    let_funcs.Add(pr);
    let_where_funcs.Add(pr);
    //let_funcs1.Add(_procedure_definition);
    //_function_lambda_definitions.Add(_procedure_definition);////////////////
    let_fact_params.Add(_procedure_definition.lambda_name);
    return new ident(((ident)LRParser.GetReductionSyntaxNode(0)).name);
}
    case (int)RuleConstants.RULE_GUARD_BODY_LIST:
    //<guard_body_list> ::= <guard_body> <empty>
    {
        ArrayList ar = (ArrayList)LRParser.GetReductionSyntaxNode(0);
        ArrayList ar_main = new ArrayList();
        ar_main.Add(ar);
        return ar_main;
    }
    case (int)RuleConstants.RULE_GUARD_BODY_LIST2:
    //<guard_body_list> ::= <guard_body_list> <guard_body>
    {
        ArrayList ar_main = (ArrayList)LRParser.GetReductionSyntaxNode(0);
        ar_main.Add(LRParser.GetReductionSyntaxNode(1));
        return ar_main;
    }
    case (int)RuleConstants.RULE_GUARD_BODY_TKASSIGN:
    //<guard_body> ::= <guard> 'tkAssign' <body_func>
    {
        ArrayList ar = new ArrayList();
        ar.Add(LRParser.GetReductionSyntaxNode(0));
        ar.Add(LRParser.GetReductionSyntaxNode(2));
        return ar;
    }
    case (int)RuleConstants.RULE_VARIABLE_TKIDENT:
//<variable> ::= 'tkIdent'
{
    if (token_where==1)
        token_where = 2;
    if (((ident)LRParser.GetReductionSyntaxNode(0)).name=="pi")
    {
        named_type_reference _named_type_reference1 = new named_type_reference();
        ident idtype1 = new ident("datatype");
        _named_type_reference1.names.Add(idtype1);
        expression_list ell = new expression_list();
        ell.expressions.Add(LRParser.GetReductionSyntaxNode(0) as expression);
        ell.expressions.Add(new string_const("real") as expression);
        named_type_reference ntr = _named_type_reference1;
        new_expr newexpr = new new_expr(ntr, ell, false, null);
        return newexpr;
    }
    else
        return LRParser.GetReductionSyntaxNode(0);
}
    case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKIDENT_TKROUNDCLOSE:
//<variable> ::= 'tkRoundOpen' 'tkIdent' <params_value> 'tkRoundClose'
{
    expression_list el = LRParser.GetReductionSyntaxNode(2) as expression_list;
    ////////////////////////////for curring
    int n = el.expressions.Count;
    int fld_params_count = find_count_params(((ident)LRParser.GetReductionSyntaxNode(1)).name);
    ////////////////////////////
    if (fld_params_count - n > 0)
    {
        expression_list _expression_list = (expression_list)LRParser.GetReductionSyntaxNode(2);
        expression_list _params = new expression_list();
        expression_list _params_el = new expression_list();
        for (int i = 0; i < list_params1.Count; i++)
            if (list_params1[i] is ident)
            {
                _params.expressions.Add(new ident(((ident)list_params1[i]).name));
                _params_el.expressions.Add(new ident(((ident)list_params1[i]).name));
            }
        for (int i = 1; i <= fld_params_count - n; i++)
            _expression_list.expressions.Add(new ident("$$" + i));
        for (int i = 1; i <= fld_params_count - n; i++)
        {
            _params.expressions.Add(new ident("$$" + i));
            _params_el.expressions.Add(new ident("$$" + i));
        }
        method_call _method_call = new method_call(_expression_list);
        if (_method_call is dereference)
            ((dereference)_method_call).dereferencing_value = (addressed_value)LRParser.GetReductionSyntaxNode(1);

        op_type_node _op_type_node = new op_type_node(Operators.Assignment);
        assign _assign = new assign((addressed_value)new ident("result"), _method_call as expression, _op_type_node.type);

        statement_list _statement_list = new statement_list();
        _statement_list.subnodes.Add(_assign);
        ////////////////////////////
        ident_list i_l = new ident_list();
        for (int i = 1; i <= fld_params_count - n; i++)
            i_l.idents.Add(new ident("$$" + i));
        /////////////////////////////
        formal_parameters _formal_parametres = new formal_parameters();
        for (int i = 0; i < _params.expressions.Count; i++)
        {
            ident_list _ident_list = new ident_list();
            ident id = (ident)_params.expressions[i];
            _ident_list.idents.Add(id);
            named_type_reference _named_type_reference = new named_type_reference();

            ident idtype = new ident("datatype");
            _named_type_reference.names.Add(idtype);

            typed_parameters _typed_parametres = new typed_parameters(_ident_list, (type_definition)_named_type_reference, parametr_kind.none, null);
            parsertools.create_source_context(_typed_parametres, _ident_list, _named_type_reference);

            _formal_parametres.params_list.Add(_typed_parametres);
        }
        //////////////////////////
        named_type_reference _named_type_reference1 = new named_type_reference();
        ident idtype1 = new ident("datatype");
        _named_type_reference1.source_context = idtype1.source_context;
        _named_type_reference1.names.Add(idtype1);
        /////////////////////////////
        lambda_num++;
        function_lambda_definition _procedure_definition = new function_lambda_definition();
        _procedure_definition.formal_parameters = _formal_parametres;
        _procedure_definition.return_type = (type_definition)_named_type_reference1;
        _procedure_definition.ident_list = i_l;
        _procedure_definition.proc_body = null;
        _procedure_definition.parameters = _params_el;
        _procedure_definition.lambda_name = "__lambda__" + lambda_num;
        //new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, i_l, null, _params_el, "lambda" + lambda_num);
        object rt = _expression_list;
        _procedure_definition.proc_body = _statement_list;
        _function_lambda_definitions_after.Add(_procedure_definition);
        _function_lambda_definitions_after.Add(((ident)LRParser.GetReductionSyntaxNode(1)).name);////////////////
        last_function_lambda_definitions.Add(_procedure_definition);
        return new ident(_procedure_definition.lambda_name);
    }
    else
    {
        method_call _method_call = null;
        string name = ((ident)LRParser.GetReductionSyntaxNode(1)).name;
        if (name == "cos" || name == "sin" || name == "tan")
        {
            bin_expr b_e = new bin_expr();
            b_e.operation_type = Operators.Plus;
            b_e.left = el.expressions[0];
            named_type_reference _named_type_reference2 = new named_type_reference();
            ident idtype2 = new ident("datatype");
            _named_type_reference2.names.Add(idtype2);
            expression_list ell2 = new expression_list();
            ell2.expressions.Add(new double_const(0.0));
            ell2.expressions.Add(new string_const("real") as expression);
            named_type_reference ntr2 = _named_type_reference2;
            b_e.right = new new_expr(ntr2, ell2, false, null);
            //////////////////////////////////
            dot_node d_n = new dot_node();
            d_n.left = (addressed_value)b_e;
            d_n.right = new ident("ob");
            expression_list el1 = new expression_list();
            el1.expressions.Add(d_n);
            method_call m_c = new method_call(el1);
            m_c.dereferencing_value = new ident("real");
            ///////////////////////////////////
            expression_list el2 = new expression_list();
            el2.expressions.Add(m_c);
            _method_call = new method_call(el2);
            _method_call.dereferencing_value = (addressed_value)LRParser.GetReductionSyntaxNode(1);
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype");
            _named_type_reference1.names.Add(idtype1);
            expression_list ell = new expression_list();
            ell.expressions.Add(_method_call as expression);
            ell.expressions.Add(new string_const("real") as expression);
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, ell, false, null);
            return newexpr;
        }
        else
        {
            _method_call = new method_call(el);
            
            if (_method_call is dereference)
            {
                ((dereference)_method_call).dereferencing_value = (addressed_value)LRParser.GetReductionSyntaxNode(1);
                parsertools.create_source_context(_method_call, LRParser.GetReductionSyntaxNode(1), _method_call);
            }
            list_method_calls.Add(_method_call);
            last_list_method_calls.Add(_method_call);
            last_list_method_calls_lambda.Add(_method_call);
            return _method_call;
        }

                                                    }
									}
    //case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKROUNDCLOSE_TKROUNDOPEN_TKROUNDCLOSE:
//<variable> ::= 'tkRoundOpen' <variable> 'tkRoundClose' 'tkRoundOpen' <params_value> 'tkRoundClose'
    case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKROUNDCLOSE:
//<variable> ::= 'tkRoundOpen' <variable> <params_value> 'tkRoundClose'
{
    if (LRParser.GetReductionSyntaxNode(1) is ident)
    {
        expression_list el = LRParser.GetReductionSyntaxNode(2) as expression_list;
        int n = el.expressions.Count;
        int fld_params_count = find_count_params_lambda(((ident)LRParser.GetReductionSyntaxNode(1)).name);
        if (_function_lambda_definitions_after.Count > 0)
        {
            function_lambda_definition fld = (function_lambda_definition)_function_lambda_definitions_after[_function_lambda_definitions_after.Count - 2];
            for (int i = 0; i < fld.formal_parameters.params_list.Count; i++)
                if (!fld.formal_parameters.params_list[i].idents.idents[0].name.Contains("$"))
                    n++;
        }
        ////////////////////////////
        if (fld_params_count - n > 0)
        {
            expression_list _expression_list = (expression_list)LRParser.GetReductionSyntaxNode(2);
            expression_list _params = new expression_list();
            expression_list _params_el = new expression_list();
            for (int i = 0; i < list_params1.Count; i++)
                if (list_params1[i] is ident)
                {
                    _params.expressions.Add(new ident(((ident)list_params1[i]).name));
                    _params_el.expressions.Add(new ident(((ident)list_params1[i]).name));
                }
            for (int i = 1; i <= fld_params_count - n; i++)
                _expression_list.expressions.Add(new ident("$$" + i));
            for (int i = 1; i <= fld_params_count - n; i++)
            {
                _params.expressions.Add(new ident("$$" + i));
                _params_el.expressions.Add(new ident("$$" + i));
            }
            method_call _method_call = new method_call(_expression_list);
            if (_method_call is dereference)
                ((dereference)_method_call).dereferencing_value = (addressed_value)LRParser.GetReductionSyntaxNode(1);

            op_type_node _op_type_node = new op_type_node(Operators.Assignment);
            assign _assign = new assign((addressed_value)new ident("result"), _method_call as expression, _op_type_node.type);

            statement_list _statement_list = new statement_list();
            _statement_list.subnodes.Add(_assign);
            ////////////////////////////
            ident_list i_l = new ident_list();
            for (int i = 1; i <= fld_params_count - n; i++)
                i_l.idents.Add(new ident("$$" + i));
            /////////////////////////////
            formal_parameters _formal_parametres = new formal_parameters();
            for (int i = 0; i < _params.expressions.Count; i++)
            {
                ident_list _ident_list = new ident_list();
                ident id = (ident)_params.expressions[i];
                _ident_list.idents.Add(id);
                named_type_reference _named_type_reference = new named_type_reference();

                ident idtype = new ident("datatype");
                _named_type_reference.names.Add(idtype);

                typed_parameters _typed_parametres = new typed_parameters(_ident_list, (type_definition)_named_type_reference, parametr_kind.none, null);
                parsertools.create_source_context(_typed_parametres, _ident_list, _named_type_reference);

                _formal_parametres.params_list.Add(_typed_parametres);
            }
            //////////////////////////
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);
            /////////////////////////////
            lambda_num++;
            function_lambda_definition _procedure_definition = new function_lambda_definition();
            _procedure_definition.formal_parameters = _formal_parametres;
            _procedure_definition.return_type = (type_definition)_named_type_reference1;
            _procedure_definition.ident_list = i_l;
            _procedure_definition.proc_body = null;
            _procedure_definition.parameters = _params_el;
            _procedure_definition.lambda_name = "__lambda__" + lambda_num;
            //new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, i_l, null, _params_el, "lambda" + lambda_num);
            object rt = _expression_list;
            _procedure_definition.proc_body = _statement_list;
            _function_lambda_definitions_after.Add(_procedure_definition);
            _function_lambda_definitions_after.Add(((ident)LRParser.GetReductionSyntaxNode(1)).name);////////////////
            last_function_lambda_definitions.Add(_procedure_definition);
            return new ident(_procedure_definition.lambda_name);
        }
        else
        {
            for (int i = list_params1.Count - 1; i >= 0; i--)
                if (list_params1[i] is ident)
                {
                    el.expressions.Insert(0, (new ident(((ident)list_params1[i]).name)));
                }

            method_call _method_call = new method_call(el);
            if (_method_call is dereference)
            {
                ((dereference)_method_call).dereferencing_value = (addressed_value)LRParser.GetReductionSyntaxNode(1);
            }
            list_method_calls.Add(_method_call);
            last_list_method_calls.Add(_method_call);
            last_list_method_calls_lambda.Add(_method_call);
            return _method_call;
        }
    }
    else
    {
        if (LRParser.GetReductionSyntaxNode(1) is method_call)
        {
            method_call mc = LRParser.GetReductionSyntaxNode(1) as method_call;
            expression_list el = LRParser.GetReductionSyntaxNode(2) as expression_list;
            expression_list _expression_list = new expression_list();
            for (int i = 0; i < mc.parameters.expressions.Count; i++)
                _expression_list.expressions.Add(mc.parameters.expressions[i]);
            for (int i = 0; i < el.expressions.Count; i++)
                _expression_list.expressions.Add(el.expressions[i]);
            method_call _method_call = new method_call(_expression_list);
            if (_method_call is dereference)
                ((dereference)_method_call).dereferencing_value = (addressed_value)(new ident(((ident)mc.dereferencing_value).name));
            list_method_calls.RemoveAt(list_method_calls.Count - 1);
            list_method_calls.Add(_method_call);
            last_list_method_calls.Add(_method_call);
            last_list_method_calls_lambda.RemoveAt(last_list_method_calls_lambda.Count - 1);
            last_list_method_calls_lambda.Add(_method_call);
            return _method_call;
        }
        else
            return null;
    }
}
    case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE:
//<variable> ::= 'tkRoundtkOpen' 'tkQuote' 'tkIdent' 'tkQuote' <params_value> 'tkRoundClose'
{
    expression_list el = LRParser.GetReductionSyntaxNode(4) as expression_list;
    ////////////////////////////for curring
    int n = el.expressions.Count;
    int fld_params_count = find_count_params(((ident)LRParser.GetReductionSyntaxNode(2)).name);
    ////////////////////////////
    if (fld_params_count - n > 0)
    {
        expression_list _expression_list = (expression_list)LRParser.GetReductionSyntaxNode(4);
        expression_list _params = new expression_list();
        expression_list _params_el = new expression_list();
        for (int i = 0; i < list_params1.Count; i++)
            if (list_params1[i] is ident)
            {
                _params.expressions.Add(new ident(((ident)list_params1[i]).name));
                _params_el.expressions.Add(new ident(((ident)list_params1[i]).name));
            }
        for (int i = 1; i <= fld_params_count - n; i++)
            _expression_list.expressions.Insert(0,new ident("$$" + i));
        for (int i = 1; i <= fld_params_count - n; i++)
        {
            _params.expressions.Add(new ident("$$" + i));
            _params_el.expressions.Add(new ident("$$" + i));
        }
        method_call _method_call = new method_call(_expression_list);
        if (_method_call is dereference)
            ((dereference)_method_call).dereferencing_value = (addressed_value)LRParser.GetReductionSyntaxNode(2);

        op_type_node _op_type_node = new op_type_node(Operators.Assignment);
        assign _assign = new assign((addressed_value)new ident("result"), _method_call as expression, _op_type_node.type);

        statement_list _statement_list = new statement_list();
        _statement_list.subnodes.Add(_assign);
        ////////////////////////////
        ident_list i_l = new ident_list();
        for (int i = 1; i <= fld_params_count - n; i++)
            i_l.idents.Add(new ident("$$" + i));
        /////////////////////////////
        formal_parameters _formal_parametres = new formal_parameters();
        for (int i = 0; i < _params.expressions.Count; i++)
        {
            ident_list _ident_list = new ident_list();
            ident id = (ident)_params.expressions[i];
            _ident_list.idents.Add(id);
            named_type_reference _named_type_reference = new named_type_reference();

            ident idtype = new ident("datatype");
            _named_type_reference.names.Add(idtype);

            typed_parameters _typed_parametres = new typed_parameters(_ident_list, (type_definition)_named_type_reference, parametr_kind.none, null);
            parsertools.create_source_context(_typed_parametres, _ident_list, _named_type_reference);

            _formal_parametres.params_list.Add(_typed_parametres);
        }
        //////////////////////////
        named_type_reference _named_type_reference1 = new named_type_reference();
        ident idtype1 = new ident("datatype");
        _named_type_reference1.source_context = idtype1.source_context;
        _named_type_reference1.names.Add(idtype1);
        /////////////////////////////
        lambda_num++;
        function_lambda_definition _procedure_definition = new function_lambda_definition();
        _procedure_definition.formal_parameters = _formal_parametres;
        _procedure_definition.return_type = (type_definition)_named_type_reference1;
        _procedure_definition.ident_list = i_l;
        _procedure_definition.proc_body = null;
        _procedure_definition.parameters = _params_el;
        _procedure_definition.lambda_name = "__lambda__" + lambda_num;
        //new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, i_l, null, _params_el, "lambda" + lambda_num);
        object rt = _expression_list;
        _procedure_definition.proc_body = _statement_list;
        _function_lambda_definitions_after.Add(_procedure_definition);
        _function_lambda_definitions_after.Add(((ident)LRParser.GetReductionSyntaxNode(2)).name);////////////////
        last_function_lambda_definitions.Add(_procedure_definition);
        return new ident(_procedure_definition.lambda_name);
    }
    else
    {
        method_call _method_call = null;
        string name = ((ident)LRParser.GetReductionSyntaxNode(2)).name;
        _method_call = new method_call(el);
        if (_method_call is dereference)
        {
            ((dereference)_method_call).dereferencing_value = (addressed_value)LRParser.GetReductionSyntaxNode(2);
            parsertools.create_source_context(_method_call, LRParser.GetReductionSyntaxNode(2), _method_call);
        }
        list_method_calls.Add(_method_call);
        last_list_method_calls.Add(_method_call);
        last_list_method_calls_lambda.Add(_method_call);
        return _method_call;
    }
}
    //case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE_TKROUNDOPEN_TKROUNDCLOSE:
//<variable> ::= 'tkRoundOpen' 'tkQuote' 'tkIdent' 'tkQuote' <params_value> 'tkRoundClose' 'tkRoundOpen' <params_value> 'tkRoundClose'
    /*case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE2:
//<variable> ::= 'tkRoundOpen' 'tkQuote' 'tkIdent' 'tkQuote' <params_value> 'tkRoundClose' <params_value>
{
    ////////////////////////////////////////////////////////ident_params
    ident _ident = (ident)LRParser.GetReductionSyntaxNode(2);
    expression_list el = new expression_list();
    el.expressions.Add(((expression_list)LRParser.GetReductionSyntaxNode(6)).expressions[0]);
    el.expressions.Add(((expression_list)LRParser.GetReductionSyntaxNode(4)).expressions[0]);
    ////////////////////////////////////////////////////////
    //expression_list el = LRParser.GetReductionSyntaxNode(2) as expression_list;
    ////////////////////////////for curring
    int n = el.expressions.Count;
    int fld_params_count = find_count_params(_ident.name);
    ////////////////////////////
    if (fld_params_count - n > 0)
    {
        expression_list _expression_list = el;
        expression_list _params = new expression_list();
        expression_list _params_el = new expression_list();
        for (int i = 0; i < list_params1.Count; i++)
            if (list_params1[i] is ident)
            {
                _params.expressions.Add(new ident(((ident)list_params1[i]).name));
                _params_el.expressions.Add(new ident(((ident)list_params1[i]).name));
            }
        for (int i = 1; i <= fld_params_count - n; i++)
            _expression_list.expressions.Add(new ident("$$" + i));
        for (int i = 1; i <= fld_params_count - n; i++)
        {
            _params.expressions.Add(new ident("$$" + i));
            _params_el.expressions.Add(new ident("$$" + i));
        }
        method_call _method_call = new method_call(_expression_list);
        if (_method_call is dereference)
            ((dereference)_method_call).dereferencing_value = (addressed_value)_ident;

        op_type_node _op_type_node = new op_type_node(Operators.Assignment);
        assign _assign = new assign((addressed_value)new ident("result"), _method_call as expression, _op_type_node.type);

        statement_list _statement_list = new statement_list();
        _statement_list.subnodes.Add(_assign);
        ////////////////////////////
        ident_list i_l = new ident_list();
        for (int i = 1; i <= fld_params_count - n; i++)
            i_l.idents.Add(new ident("$$" + i));
        /////////////////////////////
        formal_parameters _formal_parametres = new formal_parameters();
        for (int i = 0; i < _params.expressions.Count; i++)
        {
            ident_list _ident_list = new ident_list();
            ident id = (ident)_params.expressions[i];
            _ident_list.idents.Add(id);
            named_type_reference _named_type_reference = new named_type_reference();

            ident idtype = new ident("datatype");
            _named_type_reference.names.Add(idtype);

            typed_parameters _typed_parametres = new typed_parameters(_ident_list, (type_definition)_named_type_reference, parametr_kind.none, null);
            parsertools.create_source_context(_typed_parametres, _ident_list, _named_type_reference);

            _formal_parametres.params_list.Add(_typed_parametres);
        }
        //////////////////////////
        named_type_reference _named_type_reference1 = new named_type_reference();
        ident idtype1 = new ident("datatype");
        _named_type_reference1.source_context = idtype1.source_context;
        _named_type_reference1.names.Add(idtype1);
        /////////////////////////////
        lambda_num++;
        function_lambda_definition _procedure_definition = new function_lambda_definition();
        _procedure_definition.formal_parameters = _formal_parametres;
        _procedure_definition.return_type = (type_definition)_named_type_reference1;
        _procedure_definition.ident_list = i_l;
        _procedure_definition.proc_body = null;
        _procedure_definition.parameters = _params_el;
        _procedure_definition.lambda_name = "__lambda__" + lambda_num;
        //new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, i_l, null, _params_el, "lambda" + lambda_num);
        object rt = _expression_list;
        _procedure_definition.proc_body = _statement_list;
        _function_lambda_definitions_after.Add(_procedure_definition);
        _function_lambda_definitions_after.Add(_ident.name);////////////////
        last_function_lambda_definitions.Add(_procedure_definition);
        return new ident(_procedure_definition.lambda_name);
    }
    else
    {
        method_call _method_call = new method_call(el);

        if (_method_call is dereference)
        {
            ((dereference)_method_call).dereferencing_value = (addressed_value)_ident;
            parsertools.create_source_context(_method_call, _ident, _method_call);
        }
        list_method_calls.Add(_method_call);
        last_list_method_calls.Add(_method_call);
        last_list_method_calls_lambda.Add(_method_call);
        return _method_call;

    }
}*/
    /*case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE:
//<variable> ::= 'tkRoundOpen' <params_value> 'tkQuote' 'tkIdent' 'tkQuote' 'tkRoundClose'
{
    expression_list el = LRParser.GetReductionSyntaxNode(1) as expression_list;
    ////////////////////////////for curring
    int n = el.expressions.Count;
    int fld_params_count = find_count_params(((ident)LRParser.GetReductionSyntaxNode(3)).name);
    ////////////////////////////
    if (fld_params_count - n > 0)
    {
        expression_list _expression_list = (expression_list)LRParser.GetReductionSyntaxNode(1);
        expression_list _params = new expression_list();
        expression_list _params_el = new expression_list();
        for (int i = 0; i < list_params1.Count; i++)
            if (list_params1[i] is ident)
            {
                _params.expressions.Add(new ident(((ident)list_params1[i]).name));
                _params_el.expressions.Add(new ident(((ident)list_params1[i]).name));
            }
        for (int i = 1; i <= fld_params_count - n; i++)
            _expression_list.expressions.Add(new ident("$$" + i));
        for (int i = 1; i <= fld_params_count - n; i++)
        {
            _params.expressions.Add(new ident("$$" + i));
            _params_el.expressions.Add(new ident("$$" + i));
        }
        method_call _method_call = new method_call(_expression_list);
        if (_method_call is dereference)
            ((dereference)_method_call).dereferencing_value = (addressed_value)LRParser.GetReductionSyntaxNode(3);

        op_type_node _op_type_node = new op_type_node(Operators.Assignment);
        assign _assign = new assign((addressed_value)new ident("result"), _method_call as expression, _op_type_node.type);

        statement_list _statement_list = new statement_list();
        _statement_list.subnodes.Add(_assign);
        ////////////////////////////
        ident_list i_l = new ident_list();
        for (int i = 1; i <= fld_params_count - n; i++)
            i_l.idents.Add(new ident("$$" + i));
        /////////////////////////////
        formal_parameters _formal_parametres = new formal_parameters();
        for (int i = 0; i < _params.expressions.Count; i++)
        {
            ident_list _ident_list = new ident_list();
            ident id = (ident)_params.expressions[i];
            _ident_list.idents.Add(id);
            named_type_reference _named_type_reference = new named_type_reference();

            ident idtype = new ident("datatype");
            _named_type_reference.names.Add(idtype);

            typed_parameters _typed_parametres = new typed_parameters(_ident_list, (type_definition)_named_type_reference, parametr_kind.none, null);
            parsertools.create_source_context(_typed_parametres, _ident_list, _named_type_reference);

            _formal_parametres.params_list.Add(_typed_parametres);
        }
        //////////////////////////
        named_type_reference _named_type_reference1 = new named_type_reference();
        ident idtype1 = new ident("datatype");
        _named_type_reference1.source_context = idtype1.source_context;
        _named_type_reference1.names.Add(idtype1);
        /////////////////////////////
        lambda_num++;
        function_lambda_definition _procedure_definition = new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, i_l, null, _params_el, "lambda" + lambda_num);
        object rt = _expression_list;
        _procedure_definition.proc_body = _statement_list;
        _function_lambda_definitions_after.Add(_procedure_definition);
        _function_lambda_definitions_after.Add(((ident)LRParser.GetReductionSyntaxNode(3)).name);////////////////
        last_function_lambda_definitions.Add(_procedure_definition);
        return new ident(_procedure_definition.lambda_name);
    }
    else
    {
        method_call _method_call = null;
        string name = ((ident)LRParser.GetReductionSyntaxNode(3)).name;
        _method_call = new method_call(el);
        if (_method_call is dereference)
        {
            ((dereference)_method_call).dereferencing_value = (addressed_value)LRParser.GetReductionSyntaxNode(3);
            parsertools.create_source_context(_method_call, LRParser.GetReductionSyntaxNode(3), _method_call);
        }
        list_method_calls.Add(_method_call);
        last_list_method_calls.Add(_method_call);
        last_list_method_calls_lambda.Add(_method_call);
        return _method_call;
    }
}
    case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE_TKROUNDOPEN_TKROUNDCLOSE:
//<variable> ::= 'tkRoundOpen' <params_value> 'tkQuote' 'tkIdent' 'tkQuote' 'tkRoundClose' 'tkRoundOpen' <params_value> 'tkRoundClose'
{
    ////////////////////////////////////////////////////////ident_params
    ident _ident = (ident)LRParser.GetReductionSyntaxNode(3);
    expression_list el = new expression_list();
    el.expressions.Add(((expression_list)LRParser.GetReductionSyntaxNode(1)).expressions[0]);
    el.expressions.Add(((expression_list)LRParser.GetReductionSyntaxNode(7)).expressions[0]);
    ////////////////////////////////////////////////////////
    //expression_list el = LRParser.GetReductionSyntaxNode(2) as expression_list;
    ////////////////////////////for curring
    int n = el.expressions.Count;
    int fld_params_count = find_count_params(_ident.name);
    ////////////////////////////
    if (fld_params_count - n > 0)
    {
        expression_list _expression_list = el;
        expression_list _params = new expression_list();
        expression_list _params_el = new expression_list();
        for (int i = 0; i < list_params1.Count; i++)
            if (list_params1[i] is ident)
            {
                _params.expressions.Add(new ident(((ident)list_params1[i]).name));
                _params_el.expressions.Add(new ident(((ident)list_params1[i]).name));
            }
        for (int i = 1; i <= fld_params_count - n; i++)
            _expression_list.expressions.Add(new ident("$$" + i));
        for (int i = 1; i <= fld_params_count - n; i++)
        {
            _params.expressions.Add(new ident("$$" + i));
            _params_el.expressions.Add(new ident("$$" + i));
        }
        method_call _method_call = new method_call(_expression_list);
        if (_method_call is dereference)
            ((dereference)_method_call).dereferencing_value = (addressed_value)_ident;

        op_type_node _op_type_node = new op_type_node(Operators.Assignment);
        assign _assign = new assign((addressed_value)new ident("result"), _method_call as expression, _op_type_node.type);

        statement_list _statement_list = new statement_list();
        _statement_list.subnodes.Add(_assign);
        ////////////////////////////
        ident_list i_l = new ident_list();
        for (int i = 1; i <= fld_params_count - n; i++)
            i_l.idents.Add(new ident("$$" + i));
        /////////////////////////////
        formal_parameters _formal_parametres = new formal_parameters();
        for (int i = 0; i < _params.expressions.Count; i++)
        {
            ident_list _ident_list = new ident_list();
            ident id = (ident)_params.expressions[i];
            _ident_list.idents.Add(id);
            named_type_reference _named_type_reference = new named_type_reference();

            ident idtype = new ident("datatype");
            _named_type_reference.names.Add(idtype);

            typed_parameters _typed_parametres = new typed_parameters(_ident_list, (type_definition)_named_type_reference, parametr_kind.none, null);
            parsertools.create_source_context(_typed_parametres, _ident_list, _named_type_reference);

            _formal_parametres.params_list.Add(_typed_parametres);
        }
        //////////////////////////
        named_type_reference _named_type_reference1 = new named_type_reference();
        ident idtype1 = new ident("datatype");
        _named_type_reference1.source_context = idtype1.source_context;
        _named_type_reference1.names.Add(idtype1);
        /////////////////////////////
        lambda_num++;
        function_lambda_definition _procedure_definition = new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, i_l, null, _params_el, "lambda" + lambda_num);
        object rt = _expression_list;
        _procedure_definition.proc_body = _statement_list;
        _function_lambda_definitions_after.Add(_procedure_definition);
        _function_lambda_definitions_after.Add(_ident.name);////////////////
        last_function_lambda_definitions.Add(_procedure_definition);
        return new ident(_procedure_definition.lambda_name);
    }
    else
    {
        method_call _method_call = new method_call(el);

        if (_method_call is dereference)
        {
            ((dereference)_method_call).dereferencing_value = (addressed_value)_ident;
            parsertools.create_source_context(_method_call, _ident, _method_call);
        }
        list_method_calls.Add(_method_call);
        last_list_method_calls.Add(_method_call);
        last_list_method_calls_lambda.Add(_method_call);
        return _method_call;

    }
}*/
    /*case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE:
    //<variable> ::= 'tkRoundOpen' <params_value> 'tkQuote' 'tkIdent' 'tkQuote' <params_value> 'tkRoundClose'
    return null;
    case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE_TKROUNDOPEN_TKROUNDCLOSE:
    //<variable> ::= 'tkRoundOpen' <params_value> 'tkQuote' 'tkIdent' 'tkQuote' <params_value> 'tkRoundClose' 'tkRoundOpen' <params_value> 'tkRoundClose'
    return null;*/
    case (int)RuleConstants.RULE_INFIX_EXPR_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE:
//<infix_expr> ::= 'tkRoundOpen' <simple_expr> 'tkQuote' 'tkIdent' 'tkQuote' <simple_expr> 'tkRoundClose'
{
    ArrayList ar = new ArrayList();
    ar.Add(LRParser.GetReductionSyntaxNode(3));
    ar.Add(LRParser.GetReductionSyntaxNode(1));
    ar.Add(LRParser.GetReductionSyntaxNode(5));
    return ar;
}
    case (int)RuleConstants.RULE_INFIX_EXPR_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE2:
//<infix_expr> ::= 'tkRoundOpen' <simple_expr> 'tkQuote' 'tkIdent' 'tkQuote' 'tkRoundClose'
{
    expression_list el = new expression_list();
    el.expressions.Add((expression)LRParser.GetReductionSyntaxNode(1));
    ////////////////////////////for curring
    int n = el.expressions.Count;
    int fld_params_count = find_count_params(((ident)LRParser.GetReductionSyntaxNode(3)).name);
    ////////////////////////////
    if (fld_params_count - n > 0)
    {
        expression_list _expression_list = new expression_list();
        _expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(1));
        expression_list _params = new expression_list();
        expression_list _params_el = new expression_list();
        for (int i = 0; i < list_params1.Count; i++)
            if (list_params1[i] is ident)
            {
                _params.expressions.Add(new ident(((ident)list_params1[i]).name));
                _params_el.expressions.Add(new ident(((ident)list_params1[i]).name));
            }
        for (int i = 1; i <= fld_params_count - n; i++)
            _expression_list.expressions.Add(new ident("$$" + i));
        for (int i = 1; i <= fld_params_count - n; i++)
        {
            _params.expressions.Add(new ident("$$" + i));
            _params_el.expressions.Add(new ident("$$" + i));
        }
        method_call _method_call = new method_call(_expression_list);
        if (_method_call is dereference)
            ((dereference)_method_call).dereferencing_value = (addressed_value)LRParser.GetReductionSyntaxNode(3);

        op_type_node _op_type_node = new op_type_node(Operators.Assignment);
        assign _assign = new assign((addressed_value)new ident("result"), _method_call as expression, _op_type_node.type);

        statement_list _statement_list = new statement_list();
        _statement_list.subnodes.Add(_assign);
        ////////////////////////////
        ident_list i_l = new ident_list();
        for (int i = 1; i <= fld_params_count - n; i++)
            i_l.idents.Add(new ident("$$" + i));
        /////////////////////////////
        formal_parameters _formal_parametres = new formal_parameters();
        for (int i = 0; i < _params.expressions.Count; i++)
        {
            ident_list _ident_list = new ident_list();
            ident id = (ident)_params.expressions[i];
            _ident_list.idents.Add(id);
            named_type_reference _named_type_reference = new named_type_reference();

            ident idtype = new ident("datatype");
            _named_type_reference.names.Add(idtype);

            typed_parameters _typed_parametres = new typed_parameters(_ident_list, (type_definition)_named_type_reference, parametr_kind.none, null);
            parsertools.create_source_context(_typed_parametres, _ident_list, _named_type_reference);

            _formal_parametres.params_list.Add(_typed_parametres);
        }
        //////////////////////////
        named_type_reference _named_type_reference1 = new named_type_reference();
        ident idtype1 = new ident("datatype");
        _named_type_reference1.source_context = idtype1.source_context;
        _named_type_reference1.names.Add(idtype1);
        /////////////////////////////
        lambda_num++;
        function_lambda_definition _procedure_definition = new function_lambda_definition();
        _procedure_definition.formal_parameters = _formal_parametres;
        _procedure_definition.return_type = (type_definition)_named_type_reference1;
        _procedure_definition.ident_list = i_l;
        _procedure_definition.proc_body = null;
        _procedure_definition.parameters = _params_el;
        _procedure_definition.lambda_name = "__lambda__" + lambda_num;
        //new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, i_l, null, _params_el, "lambda" + lambda_num);
        object rt = _expression_list;
        _procedure_definition.proc_body = _statement_list;
        _function_lambda_definitions_after.Add(_procedure_definition);
        _function_lambda_definitions_after.Add(((ident)LRParser.GetReductionSyntaxNode(3)).name);////////////////
        last_function_lambda_definitions.Add(_procedure_definition);
        return new ident(_procedure_definition.lambda_name);
    }
    else
    {
        method_call _method_call = null;
        string name = ((ident)LRParser.GetReductionSyntaxNode(3)).name;
        _method_call = new method_call(el);
        if (_method_call is dereference)
        {
            ((dereference)_method_call).dereferencing_value = (addressed_value)LRParser.GetReductionSyntaxNode(3);
            parsertools.create_source_context(_method_call, LRParser.GetReductionSyntaxNode(3), _method_call);
        }
        list_method_calls.Add(_method_call);
        last_list_method_calls.Add(_method_call);
        last_list_method_calls_lambda.Add(_method_call);
        return _method_call;
    }
}
    //case (int)RuleConstants.RULE_INFIX_EXPR_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE_TKROUNDOPEN_TKROUNDCLOSE:
//<infix_expr> ::= 'tkRoundOpen' <simple_expr> 'tkQuote' 'tkIdent' 'tkQuote' 'tkRoundClose' 'tkRoundOpen' <params_value> 'tkRoundClose'
    //case (int)RuleConstants.RULE_INFIX_EXPR_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE3:
//<infix_expr> ::= 'tkRoundOpen' <simple_expr> 'tkQuote' 'tkIdent' 'tkQuote' 'tkRoundClose' <params_value>
    case (int)RuleConstants.RULE_INFIX_EXPR_TKROUNDOPEN_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE_TKROUNDCLOSE:
//<infix_expr> ::= 'tkRoundOpen' 'tkRoundOpen' <simple_expr> 'tkQuote' 'tkIdent' 'tkQuote' 'tkRoundClose' <params_value> 'tkRoundClose'
{
    ////////////////////////////////////////////////////////ident_params
    ident _ident = (ident)LRParser.GetReductionSyntaxNode(4);
    expression_list el = new expression_list();
    el.expressions.Add((expression)LRParser.GetReductionSyntaxNode(2));
    el.expressions.Add(((expression_list)LRParser.GetReductionSyntaxNode(7)).expressions[0]);
    ////////////////////////////////////////////////////////
    //expression_list el = LRParser.GetReductionSyntaxNode(2) as expression_list;
    ////////////////////////////for curring
    int n = el.expressions.Count;
    int fld_params_count = find_count_params(_ident.name);
    ////////////////////////////
    if (fld_params_count - n > 0)
    {
        expression_list _expression_list = el;
        expression_list _params = new expression_list();
        expression_list _params_el = new expression_list();
        for (int i = 0; i < list_params1.Count; i++)
            if (list_params1[i] is ident)
            {
                _params.expressions.Add(new ident(((ident)list_params1[i]).name));
                _params_el.expressions.Add(new ident(((ident)list_params1[i]).name));
            }
        for (int i = 1; i <= fld_params_count - n; i++)
            _expression_list.expressions.Add(new ident("$$" + i));
        for (int i = 1; i <= fld_params_count - n; i++)
        {
            _params.expressions.Add(new ident("$$" + i));
            _params_el.expressions.Add(new ident("$$" + i));
        }
        method_call _method_call = new method_call(_expression_list);
        if (_method_call is dereference)
            ((dereference)_method_call).dereferencing_value = (addressed_value)_ident;

        op_type_node _op_type_node = new op_type_node(Operators.Assignment);
        assign _assign = new assign((addressed_value)new ident("result"), _method_call as expression, _op_type_node.type);

        statement_list _statement_list = new statement_list();
        _statement_list.subnodes.Add(_assign);
        ////////////////////////////
        ident_list i_l = new ident_list();
        for (int i = 1; i <= fld_params_count - n; i++)
            i_l.idents.Add(new ident("$$" + i));
        /////////////////////////////
        formal_parameters _formal_parametres = new formal_parameters();
        for (int i = 0; i < _params.expressions.Count; i++)
        {
            ident_list _ident_list = new ident_list();
            ident id = (ident)_params.expressions[i];
            _ident_list.idents.Add(id);
            named_type_reference _named_type_reference = new named_type_reference();

            ident idtype = new ident("datatype");
            _named_type_reference.names.Add(idtype);

            typed_parameters _typed_parametres = new typed_parameters(_ident_list, (type_definition)_named_type_reference, parametr_kind.none, null);
            parsertools.create_source_context(_typed_parametres, _ident_list, _named_type_reference);

            _formal_parametres.params_list.Add(_typed_parametres);
        }
        //////////////////////////
        named_type_reference _named_type_reference1 = new named_type_reference();
        ident idtype1 = new ident("datatype");
        _named_type_reference1.source_context = idtype1.source_context;
        _named_type_reference1.names.Add(idtype1);
        /////////////////////////////
        lambda_num++;
        function_lambda_definition _procedure_definition = new function_lambda_definition();
        _procedure_definition.formal_parameters = _formal_parametres;
        _procedure_definition.return_type = (type_definition)_named_type_reference1;
        _procedure_definition.ident_list = i_l;
        _procedure_definition.proc_body = null;
        _procedure_definition.parameters = _params_el;
        _procedure_definition.lambda_name = "__lambda__" + lambda_num;
        //new function_lambda_definition(_formal_parametres, (type_definition)_named_type_reference1, i_l, null, _params_el, "lambda" + lambda_num);
        object rt = _expression_list;
        _procedure_definition.proc_body = _statement_list;
        _function_lambda_definitions_after.Add(_procedure_definition);
        _function_lambda_definitions_after.Add(_ident.name);////////////////
        last_function_lambda_definitions.Add(_procedure_definition);
        return new ident(_procedure_definition.lambda_name);
    }
    else
    {
        method_call _method_call = new method_call(el);

        if (_method_call is dereference)
        {
            ((dereference)_method_call).dereferencing_value = (addressed_value)_ident;
            parsertools.create_source_context(_method_call, _ident, _method_call);
        }
        list_method_calls.Add(_method_call);
        last_list_method_calls.Add(_method_call);
        last_list_method_calls_lambda.Add(_method_call);
        return _method_call;

    }
}
    case (int)RuleConstants.RULE_MULTOP_TKSTAR:
	//<multop> ::= 'tkStar'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_MULTOP_TKSLASH :
	//<multop> ::= 'tkSlash'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ADDOP_TKPLUS :
	//<addop> ::= 'tkPlus'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ADDOP_TKMINUS :
	//<addop> ::= 'tkMinus'
return LRParser.GetReductionSyntaxNode(0);
	/*case (int)RuleConstants.RULE_STMT_TKPRINT :
	//<stmt> ::= 'tkPrint' <expr>
{
	if (!(LRParser.GetReductionSyntaxNode(1) is function_lambda_call))
    {
        dot_node _dot_node = new dot_node(null, (addressed_value)(new ident("print")));
        _dot_node.left = (addressed_value)LRParser.GetReductionSyntaxNode(1);
        object o = null;
        method_call _method_call = new method_call(o as expression_list);
        if (_method_call is dereference)
            ((dereference)_method_call).dereferencing_value = (addressed_value)_dot_node;
        procedure_call _procedure_call = new procedure_call(_method_call as addressed_value);
        return _procedure_call;
    }
    else
    {
        function_lambda_call flc = LRParser.GetReductionSyntaxNode(1) as function_lambda_call;
        method_call _method_call1 = new SyntaxTree.method_call(flc.parameters);
        ((dereference)_method_call1).dereferencing_value = (addressed_value)(new ident(flc.f_lambda_def.lambda_name));
        
        dot_node _dot_node = new dot_node(null, (addressed_value)(new ident("print")));
        _dot_node.left = (addressed_value)_method_call1;
        object o = null;
        method_call _method_call = new method_call(o as expression_list);
        if (_method_call is dereference)
            ((dereference)_method_call).dereferencing_value = (addressed_value)_dot_node;
        procedure_call _procedure_call = new procedure_call(_method_call as addressed_value);
        return _procedure_call;
    }
}*/
    case (int)RuleConstants.RULE_STMT_TKIDENT_TKARROWGEN_TKIDENT:
//<stmt> ::= 'tkIdent' 'tkArrowGen' 'tkIdent'
{
    named_type_reference _named_type_reference1 = new named_type_reference();
    ident idtype1 = new ident("datatype");
    _named_type_reference1.source_context = idtype1.source_context;
    _named_type_reference1.names.Add(idtype1);
    ident_list il = new ident_list();
    il.idents.Add((ident)LRParser.GetReductionSyntaxNode(0));
    ////////////////////////////////////////
    named_type_reference _named_type_reference2 = new named_type_reference();
    _named_type_reference2.names.Add(new SyntaxTree.ident("datatype"));
    expression_list ell = new expression_list();
                 ////////////////////////////////////////
    dot_node _dot_node = new dot_node(null, (addressed_value)LRParser.GetReductionSyntaxNode(2)/*(new ident("getChar"))*/);
    /////*****************
    named_type_reference _named_type_reference3 = new named_type_reference();
    _named_type_reference3.names.Add(new SyntaxTree.ident("datatype"));
    expression_list ell3 = new expression_list();
    ell3.expressions.Add(new string_const("$null"));
    ell3.expressions.Add(new string_const("string") as expression);
    named_type_reference ntr3 = _named_type_reference2;
    new_expr ne3= new new_expr(ntr3, ell3, false, null);
    /////*****************
    _dot_node.left = (addressed_value)ne3;
    object o = null;
    method_call _method_call = new method_call(o as expression_list);
    if (_method_call is dereference)
        ((dereference)_method_call).dereferencing_value = (addressed_value)_dot_node;//LRParser.GetReductionSyntaxNode(2);
                  /////////////////////////////////
    /*ell.expressions.Add(_method_call);
    ell.expressions.Add(new string_const("string") as expression);
    named_type_reference ntr = _named_type_reference2;
    new_expr ne = new new_expr(ntr, ell, false, null);*/
    ////////////////////////////////////////
    var_def_statement _var_def_statement = new var_def_statement(il, (type_definition)_named_type_reference1, _method_call, definition_attribute.None, false);
    var_statement _var_statement = new var_statement(_var_def_statement);
    
    return _var_statement;
}
    case (int)RuleConstants.RULE_STMT_TKIF_TKTHEN_TKELSE:
	//<stmt> ::= 'tkIf' <expr> 'tkThen' <body_func> 'tkElse' <body_func>
{
            								if_node _if_node = new if_node(null, (statement)LRParser.GetReductionSyntaxNode(3), (statement)LRParser.GetReductionSyntaxNode(5));
            								parsertools.create_source_context(_if_node, LRParser.GetReductionSyntaxNode(4), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(5), LRParser.GetReductionSyntaxNode(4)));
            
            								_if_node.condition = _ob((expression)LRParser.GetReductionSyntaxNode(1));
            								parsertools.create_source_context(_if_node, LRParser.GetReductionSyntaxNode(0), _if_node);
            								statement_list sl = new statement_list();
                                            			sl.subnodes.Add(_if_node);
                                            			return sl;
        								}
	case (int)RuleConstants.RULE_STMT_TKCASE_TKROUNDOPEN_TKROUNDCLOSE_TKOF :
	//<stmt> ::= 'tkCase' 'tkRoundOpen' <params1> 'tkRoundClose' 'tkOf' <case_variants>
return null;
    case (int)RuleConstants.RULE_STMT_TKDO_TKFIGUREOPEN_TKSEMICOLON_TKFIGURECLOSE :
	//<stmt> ::= 'tkDo' 'tkFigureOpen' <stmts> 'tkSemiColon' 'tkFigureClose'
return LRParser.GetReductionSyntaxNode(2);
    case (int)RuleConstants.RULE_STMT_TKDO:
//<stmt> ::= 'tkDo' <stmts1>
{
    return LRParser.GetReductionSyntaxNode(1);
}
	case (int)RuleConstants.RULE_STMT_TKRETURN :
	//<stmt> ::= 'tkReturn' <expr>
{
												statement_list _statement_list = new statement_list();

            										ident id = new ident("result");
            										op_type_node _op_type_node = new op_type_node(Operators.Assignment);
            										_op_type_node.source_context = parsertools.GetTokenSourceContext();

            										assign _assign = new assign((addressed_value)id, LRParser.GetReductionSyntaxNode(1) as expression, _op_type_node.type);
            										parsertools.create_source_context(_assign, id, LRParser.GetReductionSyntaxNode(1));
            
            										_statement_list.subnodes.Add((statement)_assign);
            										parsertools.assign_source_context(_statement_list, _assign);
            										return _statement_list;
										}
	case (int)RuleConstants.RULE_STMT :
	//<stmt> ::= <func_call>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CASE_VARIANTS :
	//<case_variants> ::= <case_variant> <empty>
return LRParser.GetReductionSyntaxNode(0);
    case (int)RuleConstants.RULE_CASE_VARIANTS_TKSPLIT:
//<case_variants> ::= <case_variants> tkSplit <case_variant>
    return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CASE_VARIANT_TKROUNDOPEN_TKROUNDCLOSE_TKARROW :
	//<case_variant> ::= 'tkRoundOpen' <params1> 'tkRoundClose' 'tkArrow' <body_func>
{
    												param_value_list.Add(LRParser.GetReductionSyntaxNode(1));
        											body_variant_list.Add(LRParser.GetReductionSyntaxNode(4));
    												return null;
											}
    case (int)RuleConstants.RULE_PARAMS1:
//<params1> ::= <param> <empty>
{
    //let_flag.Add(1);
    bool b = false;
    if (list_param.Count == 1)
    {
        list_params_temp.Add(new ArrayList());
        let_where_list_params.Add(new ArrayList());//
        if (list_param[0] is new_expr && ((string_const)((new_expr)list_param[0]).params_list.expressions[1]).Value == "empty")
            b = true;
        list_param.Clear();
    }
    else
    {
        list_params_temp.Add(list_param.Clone());
        let_where_list_params.Add(list_param.Clone());//
    }
    expression_list _expression_list = new expression_list();
    if (list_param.Count == 0)
    {
        if (b)
        {
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);
            expression_list el = new expression_list();
            el.expressions.Add(new int32_const(0));
            literal lt;
            string text = "empty_list";
            lt = new string_const(text);
            el.expressions.Add(lt as expression);
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, el, false, null);
            _expression_list.expressions.Add(newexpr);
        }
        else
        {
            _expression_list.source_context = ((expression)LRParser.GetReductionSyntaxNode(0)).source_context;
            _expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
        }
    }
    else
    {
        ident id = new ident();
        for (int i = 0; i < list_param.Count; i++)
        {
            if (list_param[i] is ident)
                id.name += ((ident)list_param[i]).name;
            else
            {
                errors.Add(new PascalABCCompiler.Errors.UnexpectedToken(this, "идентификатор"));
                return null;
            }
        }
        _expression_list.source_context = ((expression)id).source_context;
        _expression_list.expressions.Add((expression)id);
        list_param.Clear();
    }
    list_params1.Clear();
    for (int i = 0; i < _expression_list.expressions.Count; i++)
        list_params1.Add(_expression_list.expressions[i]);
    last_list_method_calls_lambda.Clear();
    return _expression_list;
}
    case (int)RuleConstants.RULE_PARAMS1_TKCOMMA:
//<params1> ::= <params1> 'tkComma' <param>
{
    bool b = false;
    if (list_param.Count == 1)
    {
        list_params_temp.Add(new ArrayList());
        let_where_list_params.Add(new ArrayList());//
        if (list_param[0] is new_expr && ((string_const)((new_expr)list_param[0]).params_list.expressions[1]).Value == "empty")
            b = true;
        list_param.Clear();
    }
    else
    {
        list_params_temp.Add(list_param.Clone());
        let_where_list_params.Add(list_param.Clone());//
    }
    expression_list _expression_list = (expression_list)LRParser.GetReductionSyntaxNode(0);
    if (list_param.Count == 0)
    {
        if (!b)
        {
            _expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(2) as expression);
        }
        else
        {
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("datatype");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);
            expression_list el = new expression_list();
            el.expressions.Add(new int32_const(0));
            literal lt;
            string text = "empty_list";
            lt = new string_const(text);
            el.expressions.Add(lt as expression);
            named_type_reference ntr = _named_type_reference1;
            new_expr newexpr = new new_expr(ntr, el, false, null);
            _expression_list.expressions.Add(newexpr);
        }
    }
    else
    {
        ident id = new ident();
        for (int i = 0; i < list_param.Count; i++)
        {
            if (list_param[i] is ident)
                id.name += ((ident)list_param[i]).name;
            else
            {
                errors.Add(new PascalABCCompiler.Errors.UnexpectedToken(this, "идентификатор"));
                return null;
            }
        }
        _expression_list.expressions.Add((expression)id);
        list_param.Clear();
    }
    list_params1.Clear();
    for (int i = 0; i < _expression_list.expressions.Count; i++)
        list_params1.Add(_expression_list.expressions[i]);
    last_list_method_calls_lambda.Clear();
    return _expression_list;
}
	
	case (int)RuleConstants.RULE_FUNC_CALL :
	//<func_call> ::= <expr> <empty>
{
            										statement_list _statement_list = new statement_list();

            										ident id = new ident("result");
            										op_type_node _op_type_node = new op_type_node(Operators.Assignment);
            										_op_type_node.source_context = parsertools.GetTokenSourceContext();
                                                    expression from = (expression)LRParser.GetReductionSyntaxNode(0);
                                                    if (LRParser.GetReductionSyntaxNode(0) is ident && 
                                                        (find_func_name(((ident)LRParser.GetReductionSyntaxNode(0)).name) || find_count_params_lambda(((ident)LRParser.GetReductionSyntaxNode(0)).name) >= 0))
                                                    {
                                                        //expression_list el = LRParser.GetReductionSyntaxNode(2) as expression_list;
                                                        //int n = el.expressions.Count;
                                                        expression_list el = new expression_list();
                                                        int fld_params_count = 0;
                                                        if (find_func_name(((ident)LRParser.GetReductionSyntaxNode(0)).name))
                                                            fld_params_count = find_count_params(((ident)LRParser.GetReductionSyntaxNode(0)).name);
                                                        else
                                                            fld_params_count = find_count_params_lambda(((ident)LRParser.GetReductionSyntaxNode(0)).name);
                                                        if (fld_params_count != 0)
                                                            for (int i = 0; i < fld_params_count; i++)
                                                            {
                                                                string param_name = find_name_params_lambda(((ident)LRParser.GetReductionSyntaxNode(0)).name, i);
                                                                if (param_name.Contains("$"))
                                                                {
                                                                    named_type_reference _named_type_reference1 = new named_type_reference();
                                                                    _named_type_reference1.names.Add(new SyntaxTree.ident("datatype"));
                                                                    expression_list ell = new expression_list();
                                                                    ell.expressions.Add(new string_const("$null") as expression);
                                                                    ell.expressions.Add(new string_const("string") as expression);
                                                                    named_type_reference ntr = _named_type_reference1;
                                                                    new_expr newexpr = new new_expr(ntr, ell, false, null);
                                                                    el.expressions.Add(newexpr);
                                                                }
                                                                else
                                                                {
                                                                    el.expressions.Add(new ident(param_name));
                                                                }
                                                            }
                                                        method_call _method_call = new method_call(el);
                                                        
                                                        if (_method_call is dereference)
                                                            ((dereference)_method_call).dereferencing_value = (addressed_value)LRParser.GetReductionSyntaxNode(0);
                                                        from = _method_call;
                                                    }
                                                    if (LRParser.GetReductionSyntaxNode(0) is ident)
                                                    {
                                                        function_lambda_definition fld = find_func_lambda_name(((ident)LRParser.GetReductionSyntaxNode(0)).name);
                                                        if (fld != null)
                                                        {
                                                            expression_list _expression_list1 = new expression_list();
                                                            function_lambda_call _lambda_call = new function_lambda_call(fld, _expression_list1);
                                                            from = _lambda_call;
                                                        }
                                                    }

            										assign _assign = new assign((addressed_value)id, from, _op_type_node.type);
            										parsertools.create_source_context(_assign, id, LRParser.GetReductionSyntaxNode(0));
            
            										_statement_list.subnodes.Add((statement)_assign);
            										parsertools.assign_source_context(_statement_list, _assign);
                                                    ////////////////////////////
                                                    if (LRParser.GetReductionSyntaxNode(0) is ident)
                                                    {
                                                        list_return_funcs.Add(find_count_params(((ident)LRParser.GetReductionSyntaxNode(0)).name));
                                                    }
                                                    ////////////////////////////
            										return _statement_list;
							}
	case (int)RuleConstants.RULE_PARAMS_VALUE :
	//<params_value> ::= <param_value> <empty>
{
            						expression_list _expression_list = new expression_list();
                                    if (LRParser.GetReductionSyntaxNode(0) != null)
                                        _expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
                                    else
                                        _expression_list.expressions.Add(null);
                                    return _expression_list;
        						}
	case (int)RuleConstants.RULE_PARAMS_VALUE2 :
	//<params_value> ::= <params_value> <param_value>
{
            expression_list _expression_list = (expression_list)LRParser.GetReductionSyntaxNode(0);
            parsertools.create_source_context(_expression_list, _expression_list, LRParser.GetReductionSyntaxNode(1));
            _expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(1) as expression);
            return _expression_list;
        }
    case (int)RuleConstants.RULE_PARAM_VALUE:
//<param_value> ::= <expr> <empty>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LAMBDA_FUNC_TKLEFTSLASH_TKARROW :
	//<lambda_func> ::= 'tkLeftSlash' <params> 'tkArrow' <body_func>

{
    expression_list _expression_list = (expression_list)LRParser.GetReductionSyntaxNode(1);
    statement_list _statement_list = (statement_list)LRParser.GetReductionSyntaxNode(3);
    ////////////////////////////
    ident_list i_l = new ident_list();
    for (int i = 0; i < _expression_list.expressions.Count; i++)
        i_l.idents.Add((ident)_expression_list.expressions[i]);
    /////////////////////////////
    formal_parameters _formal_parametres = new formal_parameters();
    for (int i = 0; i < _expression_list.expressions.Count; i++)
    {
        ident_list _ident_list = new ident_list();
        ident id = (ident)_expression_list.expressions[i];
        _ident_list.idents.Add(id);
        string name_param = id.name;
        typed_parameters _typed_parametres = null;
        int k = 0;
        while (k < last_list_method_calls_lambda.Count && ((ident)((method_call)last_list_method_calls_lambda[k]).dereferencing_value).name != name_param)
            k++;
        if (k < last_list_method_calls_lambda.Count)
            _typed_parametres = new typed_parameters(_ident_list, func_type(((method_call)last_list_method_calls_lambda[k]).parameters.expressions.Count), parametr_kind.none, null);
        else
        {
            named_type_reference _named_type_reference = new named_type_reference();

            ident idtype = new ident("datatype");
            _named_type_reference.names.Add(idtype);

            _typed_parametres = new typed_parameters(_ident_list, (type_definition)_named_type_reference, parametr_kind.none, null);
            parsertools.create_source_context(_typed_parametres, _ident_list, _named_type_reference);
        }
        _formal_parametres.params_list.Add(_typed_parametres);
    }
    //////////////////////////
    named_type_reference _named_type_reference1 = new named_type_reference();
    ident idtype1 = new ident("datatype");
    _named_type_reference1.source_context = idtype1.source_context;
    _named_type_reference1.names.Add(idtype1);
    /////////////////////////////
    lambda_num++;
    function_lambda_definition _procedure_definition = new function_lambda_definition();
    _procedure_definition.formal_parameters = _formal_parametres;
    _procedure_definition.return_type = (type_definition)_named_type_reference1;
    _procedure_definition.ident_list = i_l;
    _procedure_definition.proc_body = null;
    _procedure_definition.parameters = _expression_list;
    _procedure_definition.lambda_name = "__lambda__" + lambda_num;
    //new function_lambda_definition(_formal_parametres,(type_definition)_named_type_reference1,i_l, null,_expression_list, "lambda"+lambda_num);
    object rt = _expression_list;
    _procedure_definition.proc_body = _statement_list;
    //////////////////////let & where
    if (let_stack.Count>0 && ((ArrayList)let_stack[let_stack.Count - 1]).Count > 0)
    {
        _procedure_definition.defs = new List<object>();
        for (int i = 0; i < ((ArrayList)let_stack[let_stack.Count - 1]).Count; i++)
            _procedure_definition.defs.Add((procedure_definition)((ArrayList)let_stack[let_stack.Count - 1])[i]);
        if (let_where_funcs_main.Count + 1 > token_where_count)
        {
            for (int i = 0; i < last_where_funcs.Count; i++)
                //if (((procedure_definition)last_where_funcs[i]).proc_header.name.meth_name.name.Contains("lambda"))
                _procedure_definition.defs.Add((procedure_definition)last_where_funcs[i]);
            for (int i = 0; i < last_where_funcs.Count; i++)
                if (let_funcs.Count > 0)
                    let_funcs.RemoveAt(let_funcs.Count - 1);
            last_where_funcs.Clear();
        }
        for (int i = 0; i < ((ArrayList)let_stack[let_stack.Count - 1]).Count; i++)
            if (let_where_funcs.Count > 0)
                let_where_funcs.RemoveAt(let_where_funcs.Count - 1);
        for (int i = 0; i < ((ArrayList)let_stack[let_stack.Count - 1]).Count; i++)
            if (let_funcs.Count > 0)
                let_funcs.RemoveAt(let_funcs.Count - 1);
        let_func_last.Clear();
        if (let_flag.Count > 0)
            let_flag.RemoveAt(let_flag.Count - 1);
    }
    if (let_stack.Count>0)
        let_stack.RemoveAt(let_stack.Count - 1);
    /*if (let_where_funcs.Count > 0)
    {
        _procedure_definition.defs = new ArrayList();
        for (int i = 0; i < let_where_funcs.Count; i++)
            if (((procedure_definition)let_where_funcs[i]).proc_header.name.meth_name.name.Contains("lambda"))
                _procedure_definition.defs.Add(let_where_funcs[i]);
        if (let_where_funcs_main.Count + 1 > token_where_count)
        {
            for (int i = 0; i < last_where_funcs.Count; i++)
                //if (((procedure_definition)last_where_funcs[i]).proc_header.name.meth_name.name.Contains("lambda"))
                    _procedure_definition.defs.Add((procedure_definition)last_where_funcs[i]);
            for (int i = 0; i < last_where_funcs.Count; i++)
                if (let_funcs.Count > 0)
                    let_funcs.RemoveAt(let_funcs.Count - 1);
            last_where_funcs.Clear();
        }
        int j=0;
        while (j < let_where_funcs.Count)
            if (((procedure_definition)let_where_funcs[j]).proc_header.name.meth_name.name.Contains("lambda"))
                let_where_funcs.RemoveAt(j);
            else
                j++;
        //let_where_funcs.Clear();
        let_func_last.Clear();
        let_flag.Clear();
    }*/
    //////////////////////
    parsertools.create_source_context(_procedure_definition, _expression_list, rt);
    _function_lambda_definitions.Add(_procedure_definition);////////////////
    if (lambda_stack.Count > 0)
    {
        if (((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count > 0)
        {
            if (_procedure_definition.defs == null)
                _procedure_definition.defs = new List<object>();
            for (int i = 0; i < ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count; i++)
            {
                _procedure_definition.defs.Add((procedure_definition)((ArrayList)lambda_stack[lambda_stack.Count - 1])[i]);
                int j = 0;
                while (j < _function_lambda_definitions.Count && ((function_lambda_definition)_function_lambda_definitions[j]).lambda_name !=
                        ((procedure_definition)((ArrayList)lambda_stack[lambda_stack.Count - 1])[i]).proc_header.name.meth_name.name)
                    j++;
                if (j < _function_lambda_definitions.Count)
                    _function_lambda_definitions.RemoveAt(j);
            }
            //for (int i = 0; i < ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Count; i++)
                //_function_lambda_definitions.RemoveAt(_function_lambda_definitions.Count-1);
            lambda_stack.RemoveAt(lambda_stack.Count - 1);
        }
        if (lambda_stack.Count > 1 && ((ArrayList)lambda_stack[lambda_stack.Count - 2]).Count > 0)
        {
            ((ArrayList)lambda_stack[lambda_stack.Count - 2]).Add(lambda(_procedure_definition));
            lambda_stack.RemoveAt(lambda_stack.Count - 1);
        }
        else
            ((ArrayList)lambda_stack[lambda_stack.Count - 1]).Add(lambda(_procedure_definition));
    }
    //lambda_funcs.Add(_procedure_definition);

    //let_funcs.Add(lambda(_procedure_definition));
    //let_where_funcs.Add(lambda(_procedure_definition));

    return new ident(_procedure_definition.lambda_name);
}
	case (int)RuleConstants.RULE_EMPTY :
	//<empty> ::= 
	//NONTERMINAL:<empty> ::= 
	return null;
	//ENDNONTERMINAL
}
throw new RuleException("Unknown rule");
}  

public method_call _ob(object dt)
    {
        token_info _token_info = new token_info(".");
        _token_info.source_context = parsertools.GetTokenSourceContext();
        ///////////
        ident id = new ident("ob");
        dot_node _dot_node = new dot_node(null, (addressed_value)id);
        parsertools.create_source_context(_dot_node, _token_info, id);
        _dot_node.left = (addressed_value)dt;
        parsertools.create_source_context(_dot_node, dt, _dot_node.right);
        /////////////////////////
        expression_list el = new expression_list();
        el.expressions.Add(_dot_node);
        method_call _method_call = new method_call(el);
        parsertools.create_source_context(_method_call, null, null);
        /////////////////////////
        ident id1 = new ident("boolean");
        /////////////////////////
        if (_method_call is dereference)
        {
            ((dereference)_method_call).dereferencing_value = (addressed_value)id1;
            parsertools.create_source_context(_method_call, id1, _method_call);
        }
        return _method_call;
    }

public method_call _count(expression dt)
{
    ident id = new ident("count");
    dot_node _dot_node = new dot_node(null, (addressed_value)id);
    _dot_node.left = (addressed_value)dt;
    method_call _method_call = new method_call();
    _method_call.dereferencing_value = _dot_node;
    return _method_call;
}

public method_call _add(expression dt, expression param)
{
     ident id = new ident("add");
    dot_node _dot_node = new dot_node(null, null);
    expression_list el = new expression_list();
    el.expressions.Add(param);
    //method_call _method_call = new method_call(el);
    //_method_call.dereferencing_value = id;
    _dot_node.left = (addressed_value)dt;
    _dot_node.right = id;
    method_call _method_call = new method_call(el);
    _method_call.dereferencing_value = _dot_node;
    return _method_call;
}

public method_call _index(expression dt, expression param)
{
    token_info _token_info = new token_info(".");
    ident id = new ident("index");
    dot_node _dot_node = new dot_node(null, null);
    expression_list el = new expression_list();
    el.expressions.Add(param);
    _dot_node.left = (addressed_value)dt;
    _dot_node.right = id;
    method_call _method_call = new method_call(el);
    _method_call.dereferencing_value = _dot_node;
    return _method_call;
}

public procedure_definition lambda(function_lambda_definition _function_lambda_definition)
{
    SyntaxTree.procedure_definition _func_def = new PascalABCCompiler.SyntaxTree.procedure_definition();
    SyntaxTree.method_name _method_name1 = new SyntaxTree.method_name(null, new SyntaxTree.ident(_function_lambda_definition.lambda_name), null);
    SyntaxTree.function_header _function_header1 = new SyntaxTree.function_header();

    object rt1 = new object();
    _function_header1.name = _method_name1;
    SyntaxTree.formal_parameters fps = new PascalABCCompiler.SyntaxTree.formal_parameters();
    _function_header1.parameters = _function_lambda_definition.formal_parameters;//fps;
    SyntaxTree.named_type_reference _named_type_reference = new SyntaxTree.named_type_reference();
    SyntaxTree.ident idtype = new SyntaxTree.ident("datatype");
    _named_type_reference.source_context = idtype.source_context;
    _named_type_reference.names.Add(idtype);

    rt1 = _named_type_reference;
    _function_header1.return_type = (SyntaxTree.type_definition)_named_type_reference;

    _function_header1.of_object = false;
    _function_header1.class_keyword = false;
    SyntaxTree.block _block1 = new SyntaxTree.block(null, null);
    SyntaxTree.statement_list sl1 = new SyntaxTree.statement_list();
    sl1.subnodes.Add(_function_lambda_definition.proc_body);
    _block1.program_code = sl1;
    _func_def.proc_header = _function_header1;
    _func_def.proc_body = (SyntaxTree.proc_block)_block1;
    if (_function_lambda_definition.defs != null)
    {
        if (((block)_func_def.proc_body).defs == null)
            ((block)_func_def.proc_body).defs = new declarations();
        for (int l = 0; l < _function_lambda_definition.defs.Count; l++)
            ((block)_func_def.proc_body).defs.defs.Add(_function_lambda_definition.defs[l] as procedure_definition);
    }
    _function_lambda_definition.proc_definition = _func_def;
    return _func_def;
}

public procedure_definition find_function1(string name)
{
    int i = 0;
    while (i < _functions.Count && ((procedure_definition)_functions[i]).proc_header.name.meth_name.name != name)
        i++;
    if (i < _functions.Count)
        return (procedure_definition)_functions[i];
    else
        return null;
}

public int find_count_params(string name)
{
    if (param_value_list_main.Count == 0 && param_value_list.Count == 0)
    {
        int i = 0;
        while (i < _functions.Count && ((procedure_definition)_functions[i]).proc_header.name.meth_name.name != name)
            i++;
        if (i < _functions.Count)
        {
            if (((procedure_definition)_functions[i]).proc_header.parameters == null)
                return 0;
            else
                return ((procedure_definition)_functions[i]).proc_header.parameters.params_list.Count;
        }
        else
            return 0;
    }
    else
    {
        int i = 0;
        while (i < func_name.Count && ((ident)func_name[i]).name != name)
            i++;
        if (i < func_name.Count)
        {
            if (i >= param_value_list_main.Count)
            {
                if (i >= param_value_list_main.Count)
                //if (param_value_list_main.Count==0)
                {
                    if (param_value_list[param_value_list.Count - 1] == null)
                        return 0;
                    else
                        return ((expression_list)param_value_list[param_value_list.Count - 1]).expressions.Count;
                }
                else
                    return 0;
            }
            else
            {
                if (((ArrayList)param_value_list_main[i])[((ArrayList)param_value_list_main[i]).Count - 1] == null)
                    return 0;
                else
                    return ((expression_list)((ArrayList)param_value_list_main[i])[((ArrayList)param_value_list_main[i]).Count - 1]).expressions.Count;
            }
        }
        else
            return 0;
    }
}

public int find_count_params_lambda(string name)
{
    int i = 0;
    while (i < _function_lambda_definitions_after.Count && ((function_lambda_definition)_function_lambda_definitions_after[i]).lambda_name != name)
        i += 2;
    if (i < _function_lambda_definitions_after.Count)
        return ((function_lambda_definition)_function_lambda_definitions_after[i]).formal_parameters.params_list.Count;
    else
        return -1;
}

public string find_name_params_lambda(string name, int ind)
{
    int i = 0;
    while (i < _function_lambda_definitions_after.Count && ((function_lambda_definition)_function_lambda_definitions_after[i]).lambda_name != name)
        i += 2;
    if (i < _function_lambda_definitions_after.Count)
    {
        formal_parameters fp = ((function_lambda_definition)_function_lambda_definitions_after[i]).formal_parameters;
        if (ind < fp.params_list.Count)
            return fp.params_list[ind].idents.idents[0].name;
        else
            return "";
    }
    else
        return "";
}

public bool find_func_name(string name)
{
    int i = 0;
    while (i < func_name.Count && ((ident)func_name[i]).name != name)
        i++;
    if (i < func_name.Count)
        return true;
    else
        return false;
}

public function_lambda_definition find_func_lambda_name(string name)
{
    int i = 0;
    while (i < _function_lambda_definitions.Count && ((function_lambda_definition)_function_lambda_definitions[i]).lambda_name != name)
        i++;
    if (i < _function_lambda_definitions.Count)
        return (function_lambda_definition)_function_lambda_definitions[i];
    else
        return null;
}

/*public procedure_definition find_let_lambda_name(string name)
{
    int i = 0;
    while (i < let_funcs.Count && ((procedure_definition)let_funcs[i]).proc_header.name.meth_name.name != name)
        i++;
    if (i < let_funcs.Count)
        return (procedure_definition)let_funcs[i];
    else
        return null;
}*/

public type_definition func_type(int count)
{
    //int count = mc.parameters.expressions.Count;

    formal_parameters _formal_parametres = new formal_parameters();
    for (int i = 0; i < count; i++)
    {
        ident_list _ident_list = new ident_list();
        ident id = new ident("$a" + i.ToString());
        _ident_list.idents.Add(id);
        named_type_reference _named_type_reference1 = new named_type_reference();
        ident idtype1 = new ident("datatype");
        _named_type_reference1.names.Add(idtype1);
        typed_parameters _typed_parametres = new typed_parameters(_ident_list, (type_definition)_named_type_reference1, parametr_kind.none, null);
        _formal_parametres.params_list.Add(_typed_parametres);
    }
    named_type_reference _named_type_reference = new named_type_reference();
    ident idtype = new ident("datatype");
    _named_type_reference.names.Add(idtype);
    function_header _function_header = new function_header();
    _function_header.parameters = _formal_parametres;
    _function_header.return_type = (type_definition)_named_type_reference;
    _function_header.of_object = false;
    _function_header.class_keyword = false;
    return (type_definition)_function_header;
}

public var_statement var_st(string name, type_definition td)
{
    ident_list il = new ident_list();
    il.idents.Add(new ident(name));
    var_def_statement _var_def_statement = new var_def_statement(il, td, null, definition_attribute.None, false);
    return new var_statement(_var_def_statement);
}

public method_call find_method_call(string name, int k)
{
    int i = 0;
    while (i < ((ArrayList)list_method_calls_main[k]).Count &&
        ((ident)((method_call)((ArrayList)list_method_calls_main[k])[i]).dereferencing_value).name != name)
        i++;
    if (i < ((ArrayList)list_method_calls_main[k]).Count)
        return (method_call)((ArrayList)list_method_calls_main[k])[i];
    else
        return null;
}

public method_call find_method_call_lambda(string name)
{
    int i = 0;
    while (i < list_method_calls_lambda.Count &&
        ((ident)((method_call)list_method_calls_lambda[i]).dereferencing_value).name != name)
        i++;
    if (i < list_method_calls_lambda.Count)
        return (method_call)list_method_calls_lambda[i];
    else
        return null;
}
public function_lambda_definition find_func_lambda_after(string name)
{
    int i = 0;
    while (i < _function_lambda_definitions_after.Count &&
        ((function_lambda_definition)_function_lambda_definitions_after[i]).lambda_name != name)
        i += 2;
    if (i < _function_lambda_definitions_after.Count)
        return _function_lambda_definitions_after[i] as function_lambda_definition;
    else
        return null;
}
public ArrayList find_params(string name)
{
    ArrayList _params = new ArrayList(); 
    if (param_value_list_main.Count == 0 && param_value_list.Count == 0)
    {
        int i = 0;
        while (i < _functions.Count && ((procedure_definition)_functions[i]).proc_header.name.meth_name.name != name)
            i++;
        if (i < _functions.Count)
        {
            if (((procedure_definition)_functions[i]).proc_header.parameters == null)
                return _params;
            else
            {
                for (int j = 0; j < ((procedure_definition)_functions[i]).proc_header.parameters.params_list.Count; j++)
                    _params.Add(((procedure_definition)_functions[i]).proc_header.parameters.params_list[j]);
                    return _params;
            }
        }
        else
            return _params;
    }
    else
    {
        int i = 0;
        while (i < func_name.Count && ((ident)func_name[i]).name != name)
            i++;
        if (i < func_name.Count)
        {
            if (i >= param_value_list_main.Count)
            {
                if (i >= param_value_list_main.Count)
                //if (param_value_list_main.Count==0)
                {
                    if (param_value_list[param_value_list.Count - 1] == null)
                        return _params;
                    else
                    {
                        for (int j = 0; j < ((expression_list)param_value_list[param_value_list.Count - 1]).expressions.Count; j++)
                            _params.Add(((expression_list)param_value_list[param_value_list.Count - 1]).expressions[j]);
                        return _params;
                    }
                }
                else
                    return _params;
            }
            else
            {
                if (((ArrayList)param_value_list_main[i])[((ArrayList)param_value_list_main[i]).Count - 1] == null)
                    return _params;
                else
                {
                    for (int j = 0; j < ((expression_list)((ArrayList)param_value_list_main[i])[((ArrayList)param_value_list_main[i]).Count - 1]).expressions.Count; j++)
                        _params.Add(((expression_list)((ArrayList)param_value_list_main[i])[((ArrayList)param_value_list_main[i]).Count - 1]).expressions[j]);
                    return _params;
                }
            }
        }
        else
            return _params;
    }
}
public void Error_Clear()
{
    param_value_list.Clear();
    body_variant_list.Clear();
    param_value_list_main.Clear();
    body_variant_list_main.Clear();
    guard_list.Clear();
    guard_list_main.Clear();
    func_name.Clear();
    decls_counts = 0;
    where_flag = false;
    list_param.Clear();
    list_params.Clear();
    list_params_temp.Clear();
    list_params_main.Clear();
    _function_lambda_definitions.Clear();
    _function_lambda_definitions_after.Clear();
    lambda_num = 0;
    _functions.Clear();
    list_method_calls.Clear();
    list_method_calls_main.Clear();
    list_return_funcs.Clear();
    list_return_funcs_main.Clear();
    list_params1.Clear();
    last_function_lambda_definitions.Clear();
    last_list_method_calls.Clear();
    last_list_method_calls_lambda.Clear();
    let_fact_params.Clear();
    let_funcs.Clear();
    let_funcs_funcs.Clear();
    let_func_last.Clear();
    let_funcs1.Clear();
    let_flag.Clear();
    where_funcs.Clear();
    let_where_funcs.Clear();
    let_where_funcs_main.Clear();
    token_where = 0;
    token_where_count = 0;
    last_where_funcs.Clear();
    list_method_calls_lambda.Clear();
    let_where_list_params.Clear();
    _function_lambda_definitions_main.Clear();
    let_stack.Clear();
    token_let = 0;
    lambda_stack.Clear();
}

/*public function_lambda_definition find_lambda_funcs(string name)
{
    int i = 0;
    while (i < lambda_funcs.Count && ((function_lambda_definition)lambda_funcs[i]).lambda_name != name)
        i++;
    if (i < lambda_funcs.Count)
        return (function_lambda_definition)lambda_funcs[i];
    else
        return null;
}*/
public ArrayList find_lambda_funcs_main(string name)
{
    int i = 0;
    while (i < _function_lambda_definitions_main.Count && ((ArrayList)_function_lambda_definitions_main[i])[0].ToString() != name)
        i++;
    if (i < _function_lambda_definitions_main.Count)
        return ((ArrayList)_function_lambda_definitions_main[i])[1] as ArrayList;
    else
        return null;
}
public void constructor_rec_var(statement_list body, new_expr n_e)
{
    ident id = null;
    string name_corteg = "";
    for (int ii = 2; ii < n_e.params_list.expressions.Count; ii++)
        if (n_e.params_list.expressions[ii] is ident)
            name_corteg += ((ident)n_e.params_list.expressions[ii]).name;
    if (name_corteg=="")
        name_corteg = "$$";
    name_corteg+=rec_num;
    rec_num++;
    id = new ident(name_corteg);
    ident_list _ident_list1 = new ident_list();
    _ident_list1.idents.Add(id);
    string name_param2 = id.name;
    typed_parameters _typed_parametres2 = null;
    named_type_reference _named_type_reference3 = new named_type_reference();
    ident idtype3 = new ident("datatype");
    _named_type_reference3.names.Add(idtype3);
    var_def_statement _var_def_statement2 = new var_def_statement(_ident_list1, (type_definition)_named_type_reference3, null, definition_attribute.None, false);
    var_statement _var_statement2 = new var_statement(_var_def_statement2);
    body.subnodes.Add(_var_statement2);
    for (int ii = 2; ii < n_e.params_list.expressions.Count; ii++)
    {
        if (n_e.params_list.expressions[ii] is ident)
        {
            ident id1 = (ident)n_e.params_list.expressions[ii];
            ident_list _ident_list = new ident_list();
            _ident_list.idents.Add(id1);
            typed_parameters _typed_parametres = null;
            named_type_reference _named_type_reference = new named_type_reference();
            ident idtype = new ident("datatype");
            _named_type_reference.names.Add(idtype);
            var_def_statement _var_def_statement = new var_def_statement(_ident_list, (type_definition)_named_type_reference, null, definition_attribute.None, false);
            var_statement _var_statement = new var_statement(_var_def_statement);
            body.subnodes.Add(_var_statement);
        }
        else
            constructor_rec_var(body, (new_expr)n_e.params_list.expressions[ii]);
    }
}
public void constructor_rec_ass(statement_list stmt_list, new_expr n_e, method_call _method_call)
{
    string name_corteg = "";
    for (int ii = 2; ii < n_e.params_list.expressions.Count; ii++)
        if (n_e.params_list.expressions[ii] is ident)
            name_corteg += ((ident)n_e.params_list.expressions[ii]).name;
    if (name_corteg == "")
        name_corteg = "$$";
    name_corteg += rec_num;
    rec_num++;
    assign _assign = new assign();
    _assign.operator_type = Operators.Assignment;
    _assign.to = new ident(name_corteg);
    _assign.from = _method_call;
    stmt_list.subnodes.Add(_assign);
    for (int ii = 2; ii < n_e.params_list.expressions.Count; ii++)
    {
        assign _assign1 = new assign();
        _assign1.operator_type = Operators.Assignment;
        if (n_e.params_list.expressions[ii] is ident)
        {
            _assign1.to = (ident)n_e.params_list.expressions[ii];
            _assign1.from = _index(new ident(name_corteg), new int32_const(ii - 2));
            stmt_list.subnodes.Add(_assign1);
        }
        else
            constructor_rec_ass(stmt_list, (new_expr)n_e.params_list.expressions[ii], _index(_method_call, new int32_const(ii - 2)));
    }
}


} 
}
