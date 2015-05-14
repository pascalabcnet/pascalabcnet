
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using PascalABCCompiler.HaskellParser.Errors;
using PascalABCCompiler.ParserTools;
using GoldParser;

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
	SYMBOL_EOF              =   0, // (EOF)
	SYMBOL_ERROR            =   1, // (Error)
	SYMBOL_WHITESPACE       =   2, // (Whitespace)
	SYMBOL_COMMENTEND       =   3, // (Comment End)
	SYMBOL_COMMENTLINE      =   4, // (Comment Line)
	SYMBOL_COMMENTSTART     =   5, // (Comment Start)
	SYMBOL_TKAND            =   6, // 'tkAnd'
	SYMBOL_TKARROW          =   7, // 'tkArrow'
	SYMBOL_TKARROWGEN       =   8, // 'tkArrowGen'
	SYMBOL_TKASSIGN         =   9, // 'tkAssign'
	SYMBOL_TKBOOL           =  10, // 'tkBool'
	SYMBOL_TKBOTTOMMINUS    =  11, // 'tkBottomMinus'
	SYMBOL_TKCASE           =  12, // 'tkCase'
	SYMBOL_TKCHAR           =  13, // 'tkChar'
	SYMBOL_TKCOLON          =  14, // 'tkColon'
	SYMBOL_TKCOMMA          =  15, // 'tkComma'
	SYMBOL_TKDO             =  16, // 'tkDo'
	SYMBOL_TKDOT            =  17, // 'tkDot'
	SYMBOL_TKDOUBLE         =  18, // 'tkDouble'
	SYMBOL_TKELSE           =  19, // 'tkElse'
	SYMBOL_TKEQUAL          =  20, // 'tkEqual'
	SYMBOL_TKFIGURECLOSE    =  21, // 'tkFigureClose'
	SYMBOL_TKFIGUREOPEN     =  22, // 'tkFigureOpen'
	SYMBOL_TKIDENT          =  23, // 'tkIdent'
	SYMBOL_TKIF             =  24, // 'tkIf'
	SYMBOL_TKIMPORT         =  25, // 'tkImport'
	SYMBOL_TKIN             =  26, // 'tkIn'
	SYMBOL_TKINT            =  27, // 'tkInt'
	SYMBOL_TKLEFTSLASH      =  28, // 'tkLeftSlash'
	SYMBOL_TKLESS           =  29, // 'tkLess'
	SYMBOL_TKLESSEQ         =  30, // 'tkLessEq'
	SYMBOL_TKLET            =  31, // 'tkLet'
	SYMBOL_TKMAINIDENT      =  32, // 'tkMainIdent'
	SYMBOL_TKMAINIDENT1     =  33, // 'tkMainIdent1'
	SYMBOL_TKMINUS          =  34, // 'tkMinus'
	SYMBOL_TKMODULE         =  35, // 'tkModule'
	SYMBOL_TKMORE           =  36, // 'tkMore'
	SYMBOL_TKMOREEQ         =  37, // 'tkMoreEq'
	SYMBOL_TKNOT            =  38, // 'tkNot'
	SYMBOL_TKNOTEQUAL       =  39, // 'tkNotEqual'
	SYMBOL_TKOF             =  40, // 'tkOf'
	SYMBOL_TKOR             =  41, // 'tkOr'
	SYMBOL_TKOTHERWISE      =  42, // 'tkOtherwise'
	SYMBOL_TKPLUS           =  43, // 'tkPlus'
	SYMBOL_TKQUOTE          =  44, // 'tkQuote'
	SYMBOL_TKREF            =  45, // 'tkRef'
	SYMBOL_TKRETURN         =  46, // 'tkReturn'
	SYMBOL_TKROUNDCLOSE     =  47, // 'tkRoundClose'
	SYMBOL_TKROUNDOPEN      =  48, // 'tkRoundOpen'
	SYMBOL_TKSEMICOLON      =  49, // 'tkSemiColon'
	SYMBOL_TKSLASH          =  50, // 'tkSlash'
	SYMBOL_TKSPLIT          =  51, // 'tkSplit'
	SYMBOL_TKSQUARECLOSE    =  52, // 'tkSquareClose'
	SYMBOL_TKSQUAREOPEN     =  53, // 'tkSquareOpen'
	SYMBOL_TKSTAR           =  54, // 'tkStar'
	SYMBOL_TKSTRING         =  55, // 'tkString'
	SYMBOL_TKTHEN           =  56, // 'tkThen'
	SYMBOL_TKWHERE          =  57, // 'tkWhere'
	SYMBOL_ADD_EXPR         =  58, // <add_expr>
	SYMBOL_ADDOP            =  59, // <addop>
	SYMBOL_BODY             =  60, // <body>
	SYMBOL_BODY_FUNC        =  61, // <body_func>
	SYMBOL_BODY_WHERE       =  62, // <body_where>
	SYMBOL_CASE_VARIANT     =  63, // <case_variant>
	SYMBOL_CASE_VARIANTS    =  64, // <case_variants>
	SYMBOL_CONDITION        =  65, // <condition>
	SYMBOL_CONDITIONS       =  66, // <conditions>
	SYMBOL_CONDITIONS_COMMA =  67, // <conditions_comma>
	SYMBOL_CORTEG           =  68, // <corteg>
	SYMBOL_DEF_VAR          =  69, // <def_var>
	SYMBOL_DEF_VARS         =  70, // <def_vars>
	SYMBOL_EMPTY            =  71, // <empty>
	SYMBOL_EXPR             =  72, // <expr>
	SYMBOL_FUNC_CALL        =  73, // <func_call>
	SYMBOL_FUNCS            =  74, // <funcs>
	SYMBOL_FUNCS_VARIANTS   =  75, // <funcs_variants>
	SYMBOL_GENERATOR        =  76, // <generator>
	SYMBOL_GENERATORS       =  77, // <generators>
	SYMBOL_GUARD            =  78, // <guard>
	SYMBOL_GUARD_BODY       =  79, // <guard_body>
	SYMBOL_GUARD_BODY_LIST  =  80, // <guard_body_list>
	SYMBOL_IMPORT           =  81, // <import>
	SYMBOL_IMPORTS          =  82, // <imports>
	SYMBOL_INFIX_EXPR       =  83, // <infix_expr>
	SYMBOL_INIT             =  84, // <init>
	SYMBOL_INITS            =  85, // <inits>
	SYMBOL_LAMBDA_FUNC      =  86, // <lambda_func>
	SYMBOL_LIST             =  87, // <list>
	SYMBOL_LIST_CONSTRUCTOR =  88, // <list_constructor>
	SYMBOL_LIST_ELEMENTS    =  89, // <list_elements>
	SYMBOL_LIST_PARAM       =  90, // <list_param>
	SYMBOL_LIST_PARAM1      =  91, // <list_param1>
	SYMBOL_MAIN_FUNC        =  92, // <main_func>
	SYMBOL_MODULE           =  93, // <module>
	SYMBOL_MULT_EXPR        =  94, // <mult_expr>
	SYMBOL_MULTOP           =  95, // <multop>
	SYMBOL_NEGATE_EXPR      =  96, // <negate_expr>
	SYMBOL_PARAM            =  97, // <param>
	SYMBOL_PARAM_VALUE      =  98, // <param_value>
	SYMBOL_PARAMS           =  99, // <params>
	SYMBOL_PARAMS_VALUE     = 100, // <params_value>
	SYMBOL_PARAMS_WHERE     = 101, // <params_where>
	SYMBOL_PARAMS1          = 102, // <params1>
	SYMBOL_REFERENCE        = 103, // <reference>
	SYMBOL_SIMPLE_EXPR      = 104, // <simple_expr>
	SYMBOL_SIMPLE_TYPE_EXPR = 105, // <simple_type_expr>
	SYMBOL_STMT             = 106, // <stmt>
	SYMBOL_STMTS            = 107, // <stmts>
	SYMBOL_STMTS1           = 108, // <stmts1>
	SYMBOL_VARIABLE         = 109, // <variable>
	SYMBOL_VARIABLE_EXPR    = 110, // <variable_expr>
	SYMBOL_VARIANT          = 111, // <variant>
	SYMBOL_VARIANTS         = 112, // <variants>
	SYMBOL_WHERE_VAR        = 113  // <where_var>
};














///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//RuleConstants
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

public enum RuleConstants : int
{
	RULE_MODULE_TKMODULE_TKMAINIDENT_TKWHERE                                                  =   0, // <module> ::= 'tkModule' 'tkMainIdent' 'tkWhere' <reference> <imports> <body> <empty>
	RULE_MODULE                                                                               =   1, // <module> ::= <reference> <imports> <body> <empty>
	RULE_MODULE_TKMODULE_TKIDENT_TKWHERE                                                      =   2, // <module> ::= 'tkModule' 'tkIdent' 'tkWhere' <reference> <imports> <funcs> <empty>
	RULE_REFERENCE                                                                            =   3, // <reference> ::= 
	RULE_REFERENCE_TKREF_TKSTRING                                                             =   4, // <reference> ::= 'tkRef' 'tkString'
	RULE_IMPORTS                                                                              =   5, // <imports> ::= 
	RULE_IMPORTS2                                                                             =   6, // <imports> ::= <import> <empty>
	RULE_IMPORTS3                                                                             =   7, // <imports> ::= <imports> <empty> <import>
	RULE_IMPORT_TKIMPORT_TKIDENT                                                              =   8, // <import> ::= 'tkImport' 'tkIdent'
	RULE_BODY                                                                                 =   9, // <body> ::= <main_func>
	RULE_BODY2                                                                                =  10, // <body> ::= <funcs> <main_func>
	RULE_FUNCS                                                                                =  11, // <funcs> ::= <funcs_variants> <empty>
	RULE_FUNCS_VARIANTS                                                                       =  12, // <funcs_variants> ::= <variants> <empty>
	RULE_VARIANTS                                                                             =  13, // <variants> ::= <variant> <empty>
	RULE_VARIANTS2                                                                            =  14, // <variants> ::= <variants> <variant>
	RULE_VARIANT_TKIDENT                                                                      =  15, // <variant> ::= 'tkIdent' <params> <guard_body_list> <where_var>
	RULE_VARIANT_TKQUOTE_TKIDENT_TKQUOTE                                                      =  16, // <variant> ::= <list_param1> 'tkQuote' 'tkIdent' 'tkQuote' <list_param1> <guard_body_list> <where_var>
	RULE_LIST_PARAM1                                                                          =  17, // <list_param1> ::= <list_param> <empty>
	RULE_BODY_WHERE                                                                           =  18, // <body_where> ::= <body_func> <where_var>
	RULE_WHERE_VAR                                                                            =  19, // <where_var> ::= 
	RULE_WHERE_VAR_TKWHERE                                                                    =  20, // <where_var> ::= 'tkWhere' <inits>
	RULE_INITS                                                                                =  21, // <inits> ::= <init> <empty>
	RULE_INITS_TKSEMICOLON                                                                    =  22, // <inits> ::= <inits> 'tkSemiColon' <init>
	RULE_INIT_TKIDENT                                                                         =  23, // <init> ::= 'tkIdent' <params_where> <guard_body_list> <where_var>
	RULE_PARAMS_WHERE                                                                         =  24, // <params_where> ::= 
	RULE_PARAMS_WHERE2                                                                        =  25, // <params_where> ::= <param> <empty>
	RULE_PARAMS_WHERE3                                                                        =  26, // <params_where> ::= <params_where> <param>
	RULE_PARAMS                                                                               =  27, // <params> ::= 
	RULE_PARAMS2                                                                              =  28, // <params> ::= <param> <empty>
	RULE_PARAMS3                                                                              =  29, // <params> ::= <params> <param>
	RULE_PARAM                                                                                =  30, // <param> ::= <list_param>
	RULE_LIST_PARAM_TKIDENT                                                                   =  31, // <list_param> ::= 'tkIdent' <empty>
	RULE_LIST_PARAM                                                                           =  32, // <list_param> ::= <simple_type_expr> <empty>
	RULE_LIST_PARAM_TKSQUAREOPEN_TKSQUARECLOSE                                                =  33, // <list_param> ::= 'tkSquareOpen' 'tkSquareClose'
	RULE_LIST_PARAM_TKBOTTOMMINUS                                                             =  34, // <list_param> ::= 'tkBottomMinus'
	RULE_LIST_PARAM_TKIDENT_TKCOLON                                                           =  35, // <list_param> ::= 'tkIdent' 'tkColon' <list_param>
	RULE_LIST_PARAM_TKBOTTOMMINUS_TKCOLON                                                     =  36, // <list_param> ::= 'tkBottomMinus' 'tkColon' <list_param>
	RULE_LIST_PARAM_TKROUNDOPEN_TKROUNDCLOSE                                                  =  37, // <list_param> ::= 'tkRoundOpen' <list_param> 'tkRoundClose'
	RULE_GUARD                                                                                =  38, // <guard> ::= 
	RULE_GUARD_TKSPLIT                                                                        =  39, // <guard> ::= 'tkSplit' <expr>
	RULE_GUARD_TKSPLIT_TKOTHERWISE                                                            =  40, // <guard> ::= 'tkSplit' 'tkOtherwise'
	RULE_MAIN_FUNC_TKMAINIDENT1_TKASSIGN                                                      =  41, // <main_func> ::= 'tkMainIdent1' 'tkAssign' <body_func>
	RULE_BODY_FUNC                                                                            =  42, // <body_func> ::= <stmt>
	RULE_STMTS                                                                                =  43, // <stmts> ::= <stmt> <empty> <empty>
	RULE_STMTS_TKSEMICOLON                                                                    =  44, // <stmts> ::= <stmts> 'tkSemiColon' <stmt>
	RULE_STMTS1                                                                               =  45, // <stmts1> ::= <stmt> <empty> <empty>
	RULE_STMTS12                                                                              =  46, // <stmts1> ::= <stmts1> <stmt>
	RULE_EXPR_TKEQUAL                                                                         =  47, // <expr> ::= <expr> 'tkEqual' <add_expr>
	RULE_EXPR_TKNOTEQUAL                                                                      =  48, // <expr> ::= <expr> 'tkNotEqual' <add_expr>
	RULE_EXPR_TKMORE                                                                          =  49, // <expr> ::= <expr> 'tkMore' <add_expr>
	RULE_EXPR_TKLESS                                                                          =  50, // <expr> ::= <expr> 'tkLess' <add_expr>
	RULE_EXPR_TKMOREEQ                                                                        =  51, // <expr> ::= <expr> 'tkMoreEq' <add_expr>
	RULE_EXPR_TKLESSEQ                                                                        =  52, // <expr> ::= <expr> 'tkLessEq' <add_expr>
	RULE_EXPR                                                                                 =  53, // <expr> ::= <add_expr>
	RULE_LIST_TKSQUAREOPEN_TKSQUARECLOSE                                                      =  54, // <list> ::= 'tkSquareOpen' <list_elements> 'tkSquareClose'
	RULE_LIST_TKSQUAREOPEN_TKDOT_TKDOT_TKSQUARECLOSE                                          =  55, // <list> ::= 'tkSquareOpen' <list_elements> 'tkDot' 'tkDot' <simple_expr> 'tkSquareClose'
	RULE_LIST_TKSQUAREOPEN_TKDOT_TKDOT_TKSQUARECLOSE2                                         =  56, // <list> ::= 'tkSquareOpen' <list_elements> 'tkDot' 'tkDot' 'tkSquareClose'
	RULE_LIST                                                                                 =  57, // <list> ::= <list_constructor>
	RULE_LIST_TKCOLON                                                                         =  58, // <list> ::= <simple_expr> 'tkColon' <empty> <simple_expr>
	RULE_LIST_CONSTRUCTOR_TKSQUAREOPEN_TKSPLIT_TKSQUARECLOSE                                  =  59, // <list_constructor> ::= 'tkSquareOpen' <simple_expr> 'tkSplit' <generators> <conditions_comma> 'tkSquareClose'
	RULE_GENERATORS                                                                           =  60, // <generators> ::= <generator> <empty>
	RULE_GENERATORS_TKCOMMA                                                                   =  61, // <generators> ::= <generators> 'tkComma' <generator>
	RULE_GENERATOR_TKARROWGEN                                                                 =  62, // <generator> ::= <simple_expr> 'tkArrowGen' <simple_expr>
	RULE_CONDITIONS_COMMA                                                                     =  63, // <conditions_comma> ::= <empty>
	RULE_CONDITIONS_COMMA_TKCOMMA                                                             =  64, // <conditions_comma> ::= 'tkComma' <conditions>
	RULE_CONDITIONS                                                                           =  65, // <conditions> ::= <condition> <empty>
	RULE_CONDITIONS_TKCOMMA                                                                   =  66, // <conditions> ::= <conditions> 'tkComma' <condition>
	RULE_CONDITION                                                                            =  67, // <condition> ::= <expr>
	RULE_CORTEG_TKROUNDOPEN_TKCOMMA_TKROUNDCLOSE                                              =  68, // <corteg> ::= 'tkRoundOpen' <simple_expr> 'tkComma' <list_elements> 'tkRoundClose'
	RULE_LIST_ELEMENTS                                                                        =  69, // <list_elements> ::= <empty>
	RULE_LIST_ELEMENTS2                                                                       =  70, // <list_elements> ::= <simple_expr> <empty>
	RULE_LIST_ELEMENTS_TKCOMMA                                                                =  71, // <list_elements> ::= <list_elements> 'tkComma' <simple_expr>
	RULE_ADD_EXPR_TKAND                                                                       =  72, // <add_expr> ::= <add_expr> 'tkAnd' <mult_expr>
	RULE_ADD_EXPR                                                                             =  73, // <add_expr> ::= <add_expr> <addop> <mult_expr>
	RULE_ADD_EXPR2                                                                            =  74, // <add_expr> ::= <mult_expr>
	RULE_MULT_EXPR_TKOR                                                                       =  75, // <mult_expr> ::= <mult_expr> 'tkOr' <negate_expr>
	RULE_MULT_EXPR                                                                            =  76, // <mult_expr> ::= <mult_expr> <multop> <negate_expr>
	RULE_MULT_EXPR2                                                                           =  77, // <mult_expr> ::= <negate_expr>
	RULE_NEGATE_EXPR_TKMINUS                                                                  =  78, // <negate_expr> ::= 'tkMinus' <simple_expr>
	RULE_NEGATE_EXPR_TKNOT                                                                    =  79, // <negate_expr> ::= 'tkNot' <simple_expr>
	RULE_NEGATE_EXPR                                                                          =  80, // <negate_expr> ::= <simple_expr>
	RULE_SIMPLE_EXPR                                                                          =  81, // <simple_expr> ::= <simple_type_expr>
	RULE_SIMPLE_EXPR_TKROUNDOPEN_TKROUNDCLOSE                                                 =  82, // <simple_expr> ::= 'tkRoundOpen' <expr> 'tkRoundClose'
	RULE_SIMPLE_EXPR2                                                                         =  83, // <simple_expr> ::= <infix_expr> <empty>
	RULE_SIMPLE_EXPR3                                                                         =  84, // <simple_expr> ::= <variable>
	RULE_SIMPLE_EXPR_TKLET_TKIN                                                               =  85, // <simple_expr> ::= 'tkLet' <def_vars> 'tkIn' <body_func>
	RULE_SIMPLE_EXPR4                                                                         =  86, // <simple_expr> ::= <list>
	RULE_SIMPLE_EXPR5                                                                         =  87, // <simple_expr> ::= <corteg>
	RULE_SIMPLE_EXPR6                                                                         =  88, // <simple_expr> ::= <lambda_func> <empty>
	RULE_SIMPLE_EXPR_TKROUNDOPEN_TKROUNDOPEN_TKROUNDCLOSE_TKROUNDCLOSE                        =  89, // <simple_expr> ::= 'tkRoundOpen' 'tkRoundOpen' <lambda_func> 'tkRoundClose' <params_value> 'tkRoundClose'
	RULE_SIMPLE_TYPE_EXPR_TKINT                                                               =  90, // <simple_type_expr> ::= 'tkInt'
	RULE_SIMPLE_TYPE_EXPR_TKDOUBLE                                                            =  91, // <simple_type_expr> ::= 'tkDouble'
	RULE_SIMPLE_TYPE_EXPR_TKBOOL                                                              =  92, // <simple_type_expr> ::= 'tkBool'
	RULE_SIMPLE_TYPE_EXPR_TKCHAR                                                              =  93, // <simple_type_expr> ::= 'tkChar'
	RULE_SIMPLE_TYPE_EXPR_TKSTRING                                                            =  94, // <simple_type_expr> ::= 'tkString'
	RULE_VARIABLE_EXPR                                                                        =  95, // <variable_expr> ::= <simple_expr>
	RULE_DEF_VARS                                                                             =  96, // <def_vars> ::= <def_var> <empty>
	RULE_DEF_VARS_TKSEMICOLON                                                                 =  97, // <def_vars> ::= <def_vars> 'tkSemiColon' <def_var>
	RULE_DEF_VAR_TKIDENT                                                                      =  98, // <def_var> ::= 'tkIdent' <params> <guard_body_list> <where_var>
	RULE_GUARD_BODY_LIST                                                                      =  99, // <guard_body_list> ::= <guard_body> <empty>
	RULE_GUARD_BODY_LIST2                                                                     = 100, // <guard_body_list> ::= <guard_body_list> <guard_body>
	RULE_GUARD_BODY_TKASSIGN                                                                  = 101, // <guard_body> ::= <guard> 'tkAssign' <body_func>
	RULE_VARIABLE_TKIDENT                                                                     = 102, // <variable> ::= 'tkIdent'
	RULE_VARIABLE_TKROUNDOPEN_TKIDENT_TKROUNDCLOSE                                            = 103, // <variable> ::= 'tkRoundOpen' 'tkIdent' <params_value> 'tkRoundClose'
	RULE_VARIABLE_TKROUNDOPEN_TKROUNDCLOSE                                                    = 104, // <variable> ::= 'tkRoundOpen' <variable> <params_value> 'tkRoundClose'
	RULE_VARIABLE_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE                            = 105, // <variable> ::= 'tkRoundOpen' 'tkQuote' 'tkIdent' 'tkQuote' <params_value> 'tkRoundClose'
	RULE_INFIX_EXPR_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE                          = 106, // <infix_expr> ::= 'tkRoundOpen' <simple_expr> 'tkQuote' 'tkIdent' 'tkQuote' <simple_expr> 'tkRoundClose'
	RULE_INFIX_EXPR_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE2                         = 107, // <infix_expr> ::= 'tkRoundOpen' <simple_expr> 'tkQuote' 'tkIdent' 'tkQuote' 'tkRoundClose'
	RULE_INFIX_EXPR_TKROUNDOPEN_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE_TKROUNDCLOSE = 108, // <infix_expr> ::= 'tkRoundOpen' 'tkRoundOpen' <simple_expr> 'tkQuote' 'tkIdent' 'tkQuote' 'tkRoundClose' <params_value> 'tkRoundClose'
	RULE_MULTOP_TKSTAR                                                                        = 109, // <multop> ::= 'tkStar'
	RULE_MULTOP_TKSLASH                                                                       = 110, // <multop> ::= 'tkSlash'
	RULE_ADDOP_TKPLUS                                                                         = 111, // <addop> ::= 'tkPlus'
	RULE_ADDOP_TKMINUS                                                                        = 112, // <addop> ::= 'tkMinus'
	RULE_STMT_TKIDENT_TKARROWGEN_TKIDENT                                                      = 113, // <stmt> ::= 'tkIdent' 'tkArrowGen' 'tkIdent'
	RULE_STMT_TKIF_TKTHEN_TKELSE                                                              = 114, // <stmt> ::= 'tkIf' <expr> 'tkThen' <body_func> 'tkElse' <body_func>
	RULE_STMT_TKCASE_TKROUNDOPEN_TKROUNDCLOSE_TKOF                                            = 115, // <stmt> ::= 'tkCase' 'tkRoundOpen' <params1> 'tkRoundClose' 'tkOf' <case_variants>
	RULE_STMT_TKDO_TKFIGUREOPEN_TKSEMICOLON_TKFIGURECLOSE                                     = 116, // <stmt> ::= 'tkDo' 'tkFigureOpen' <stmts> 'tkSemiColon' 'tkFigureClose'
	RULE_STMT_TKDO                                                                            = 117, // <stmt> ::= 'tkDo' <stmts1>
	RULE_STMT_TKRETURN                                                                        = 118, // <stmt> ::= 'tkReturn' <expr>
	RULE_STMT                                                                                 = 119, // <stmt> ::= <func_call>
	RULE_CASE_VARIANTS                                                                        = 120, // <case_variants> ::= <case_variant> <empty>
	RULE_CASE_VARIANTS_TKSPLIT                                                                = 121, // <case_variants> ::= <case_variants> 'tkSplit' <case_variant>
	RULE_CASE_VARIANT_TKROUNDOPEN_TKROUNDCLOSE_TKARROW                                        = 122, // <case_variant> ::= 'tkRoundOpen' <params1> 'tkRoundClose' 'tkArrow' <body_func>
	RULE_PARAMS1                                                                              = 123, // <params1> ::= <param> <empty>
	RULE_PARAMS1_TKCOMMA                                                                      = 124, // <params1> ::= <params1> 'tkComma' <param>
	RULE_FUNC_CALL                                                                            = 125, // <func_call> ::= <expr> <empty>
	RULE_PARAMS_VALUE                                                                         = 126, // <params_value> ::= <param_value> <empty>
	RULE_PARAMS_VALUE2                                                                        = 127, // <params_value> ::= <params_value> <param_value>
	RULE_PARAM_VALUE                                                                          = 128, // <param_value> ::= <expr> <empty>
	RULE_LAMBDA_FUNC_TKLEFTSLASH_TKARROW                                                      = 129, // <lambda_func> ::= 'tkLeftSlash' <params> 'tkArrow' <body_func>
	RULE_EMPTY                                                                                = 130  // <empty> ::= 
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

	case (int)SymbolConstants.SYMBOL_TKARROWGEN :
    	//'tkArrowGen'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKASSIGN :
    	//'tkAssign'
{
            if (token_where == 2 || token_where == 1)
            {
                token_where = 0;
                //if (let_where_funcs_main.Count > 1)
                    //for (int i = 0; i  let_where_funcs.Count; i++)
                        //if (let_funcs.Count > 0)
                            //let_funcs.RemoveAt(let_funcs.Count - 1);
                let_where_funcs_main.Add(let_where_funcs.Clone());
                let_where_funcs.Clear();
            }
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
	case (int)SymbolConstants.SYMBOL_TKBOTTOMMINUS :
    	//'tkBottomMinus'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
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

	case (int)SymbolConstants.SYMBOL_TKNOTEQUAL :
    	//'tkNotEqual'

		{
			op_type_node _op_type_node=new op_type_node(Operators.NotEqual);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
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

	case (int)SymbolConstants.SYMBOL_TKOTHERWISE :
    	//'tkOtherwise'

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

	case (int)SymbolConstants.SYMBOL_TKQUOTE :
    	//'tkQuote'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
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
	case (int)SymbolConstants.SYMBOL_BODY_WHERE :
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
	case (int)SymbolConstants.SYMBOL_CONDITION :
    	//<condition>
	//TERMINAL:condition
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONDITIONS :
    	//<conditions>
	//TERMINAL:conditions
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONDITIONS_COMMA :
    	//<conditions_comma>
	//TERMINAL:conditions_comma
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CORTEG :
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
	case (int)SymbolConstants.SYMBOL_GENERATOR :
    	//<generator>
	//TERMINAL:generator
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_GENERATORS :
    	//<generators>
	//TERMINAL:generators
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_GUARD :
    	//<guard>
	//TERMINAL:guard
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_GUARD_BODY :
    	//<guard_body>
	//TERMINAL:guard_body
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_GUARD_BODY_LIST :
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
	case (int)SymbolConstants.SYMBOL_INFIX_EXPR :
    	//<infix_expr>
	//TERMINAL:infix_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_INIT :
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
	case (int)SymbolConstants.SYMBOL_LIST_CONSTRUCTOR :
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
	case (int)SymbolConstants.SYMBOL_LIST_PARAM1 :
    	//<list_param1>
	//TERMINAL:list_param1
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
	case (int)SymbolConstants.SYMBOL_PARAMS_VALUE :
    	//<params_value>
	//TERMINAL:params_value
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PARAMS_WHERE :
    	//<params_where>
	//TERMINAL:params_where
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PARAMS1 :
    	//<params1>
	//TERMINAL:params1
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
	case (int)SymbolConstants.SYMBOL_SIMPLE_TYPE_EXPR :
    	//<simple_type_expr>
	//TERMINAL:simple_type_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_STMT :
    	//<stmt>
	//TERMINAL:stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_STMTS :
    	//<stmts>
	//TERMINAL:stmts
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_STMTS1 :
    	//<stmts1>
	//TERMINAL:stmts1
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
public ArrayList where_list_var = new ArrayList();
public ArrayList where_list_assign = new ArrayList();
public ArrayList where_list_var_main = new ArrayList();
public ArrayList where_list_assign_main = new ArrayList();
public ArrayList func_name = new ArrayList();
public ArrayList where_list_counts=new ArrayList();
public ArrayList where_list_counts_main = new ArrayList();
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
public struct types_param_lampda
{
    public string name;
    public ArrayList types;
}
public ArrayList list_method_calls_lambda = new ArrayList();

public Object CreateNonTerminalObject(int ReductionRuleIndex)
{
switch (ReductionRuleIndex)
{
	case (int)RuleConstants.RULE_MODULE_TKMODULE_TKMAINIDENT_TKWHERE :
	//<module> ::= 'tkModule' 'tkMainIdent' 'tkWhere' <reference> <imports> <body> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<module> ::= tkModule tkMainIdent tkWhere <reference> <imports> <body> <empty>"));}return null;
	case (int)RuleConstants.RULE_MODULE :
	//<module> ::= <reference> <imports> <body> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<module> ::= <reference> <imports> <body> <empty>"));}return null;
	case (int)RuleConstants.RULE_MODULE_TKMODULE_TKIDENT_TKWHERE :
	//<module> ::= 'tkModule' 'tkIdent' 'tkWhere' <reference> <imports> <funcs> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<module> ::= tkModule tkIdent tkWhere <reference> <imports> <funcs> <empty>"));}return null;
	case (int)RuleConstants.RULE_REFERENCE :
	//<reference> ::= 
	//NONTERMINAL:<reference> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_REFERENCE_TKREF_TKSTRING :
	//<reference> ::= 'tkRef' 'tkString'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<reference> ::= tkRef tkString"));}return null;
	case (int)RuleConstants.RULE_IMPORTS :
	//<imports> ::= 
	//NONTERMINAL:<imports> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_IMPORTS2 :
	//<imports> ::= <import> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<imports> ::= <import> <empty>"));}return null;
	case (int)RuleConstants.RULE_IMPORTS3 :
	//<imports> ::= <imports> <empty> <import>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<imports> ::= <imports> <empty> <import>"));}return null;
	case (int)RuleConstants.RULE_IMPORT_TKIMPORT_TKIDENT :
	//<import> ::= 'tkImport' 'tkIdent'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<import> ::= tkImport tkIdent"));}return null;
	case (int)RuleConstants.RULE_BODY :
	//<body> ::= <main_func>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_BODY2 :
	//<body> ::= <funcs> <main_func>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<body> ::= <funcs> <main_func>"));}return null;
	case (int)RuleConstants.RULE_FUNCS :
	//<funcs> ::= <funcs_variants> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<funcs> ::= <funcs_variants> <empty>"));}return null;
	case (int)RuleConstants.RULE_FUNCS_VARIANTS :
	//<funcs_variants> ::= <variants> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<funcs_variants> ::= <variants> <empty>"));}return null;
	case (int)RuleConstants.RULE_VARIANTS :
	//<variants> ::= <variant> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<variants> ::= <variant> <empty>"));}return null;
	case (int)RuleConstants.RULE_VARIANTS2 :
	//<variants> ::= <variants> <variant>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<variants> ::= <variants> <variant>"));}return null;
	case (int)RuleConstants.RULE_VARIANT_TKIDENT :
	//<variant> ::= 'tkIdent' <params> <guard_body_list> <where_var>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<variant> ::= tkIdent <params> <guard_body_list> <where_var>"));}return null;
	case (int)RuleConstants.RULE_VARIANT_TKQUOTE_TKIDENT_TKQUOTE :
	//<variant> ::= <list_param1> 'tkQuote' 'tkIdent' 'tkQuote' <list_param1> <guard_body_list> <where_var>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<variant> ::= <list_param1> tkQuote tkIdent tkQuote <list_param1> <guard_body_list> <where_var>"));}return null;
	case (int)RuleConstants.RULE_LIST_PARAM1 :
	//<list_param1> ::= <list_param> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<list_param1> ::= <list_param> <empty>"));}return null;
	case (int)RuleConstants.RULE_BODY_WHERE :
	//<body_where> ::= <body_func> <where_var>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<body_where> ::= <body_func> <where_var>"));}return null;
	case (int)RuleConstants.RULE_WHERE_VAR :
	//<where_var> ::= 
	//NONTERMINAL:<where_var> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_WHERE_VAR_TKWHERE :
	//<where_var> ::= 'tkWhere' <inits>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<where_var> ::= tkWhere <inits>"));}return null;
	case (int)RuleConstants.RULE_INITS :
	//<inits> ::= <init> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<inits> ::= <init> <empty>"));}return null;
	case (int)RuleConstants.RULE_INITS_TKSEMICOLON :
	//<inits> ::= <inits> 'tkSemiColon' <init>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<inits> ::= <inits> tkSemiColon <init>"));}return null;
	case (int)RuleConstants.RULE_INIT_TKIDENT :
	//<init> ::= 'tkIdent' <params_where> <guard_body_list> <where_var>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<init> ::= tkIdent <params_where> <guard_body_list> <where_var>"));}return null;
	case (int)RuleConstants.RULE_PARAMS_WHERE :
	//<params_where> ::= 
	//NONTERMINAL:<params_where> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_PARAMS_WHERE2 :
	//<params_where> ::= <param> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<params_where> ::= <param> <empty>"));}return null;
	case (int)RuleConstants.RULE_PARAMS_WHERE3 :
	//<params_where> ::= <params_where> <param>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<params_where> ::= <params_where> <param>"));}return null;
	case (int)RuleConstants.RULE_PARAMS :
	//<params> ::= 
	//NONTERMINAL:<params> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_PARAMS2 :
	//<params> ::= <param> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<params> ::= <param> <empty>"));}return null;
	case (int)RuleConstants.RULE_PARAMS3 :
	//<params> ::= <params> <param>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<params> ::= <params> <param>"));}return null;
	case (int)RuleConstants.RULE_PARAM :
	//<param> ::= <list_param>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIST_PARAM_TKIDENT :
	//<list_param> ::= 'tkIdent' <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<list_param> ::= tkIdent <empty>"));}return null;
	case (int)RuleConstants.RULE_LIST_PARAM :
	//<list_param> ::= <simple_type_expr> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<list_param> ::= <simple_type_expr> <empty>"));}return null;
	case (int)RuleConstants.RULE_LIST_PARAM_TKSQUAREOPEN_TKSQUARECLOSE :
	//<list_param> ::= 'tkSquareOpen' 'tkSquareClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<list_param> ::= tkSquareOpen tkSquareClose"));}return null;
	case (int)RuleConstants.RULE_LIST_PARAM_TKBOTTOMMINUS :
	//<list_param> ::= 'tkBottomMinus'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIST_PARAM_TKIDENT_TKCOLON :
	//<list_param> ::= 'tkIdent' 'tkColon' <list_param>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<list_param> ::= tkIdent tkColon <list_param>"));}return null;
	case (int)RuleConstants.RULE_LIST_PARAM_TKBOTTOMMINUS_TKCOLON :
	//<list_param> ::= 'tkBottomMinus' 'tkColon' <list_param>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<list_param> ::= tkBottomMinus tkColon <list_param>"));}return null;
	case (int)RuleConstants.RULE_LIST_PARAM_TKROUNDOPEN_TKROUNDCLOSE :
	//<list_param> ::= 'tkRoundOpen' <list_param> 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<list_param> ::= tkRoundOpen <list_param> tkRoundClose"));}return null;
	case (int)RuleConstants.RULE_GUARD :
	//<guard> ::= 
	//NONTERMINAL:<guard> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_GUARD_TKSPLIT :
	//<guard> ::= 'tkSplit' <expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<guard> ::= tkSplit <expr>"));}return null;
	case (int)RuleConstants.RULE_GUARD_TKSPLIT_TKOTHERWISE :
	//<guard> ::= 'tkSplit' 'tkOtherwise'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<guard> ::= tkSplit tkOtherwise"));}return null;
	case (int)RuleConstants.RULE_MAIN_FUNC_TKMAINIDENT1_TKASSIGN :
	//<main_func> ::= 'tkMainIdent1' 'tkAssign' <body_func>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<main_func> ::= tkMainIdent1 tkAssign <body_func>"));}return null;
	case (int)RuleConstants.RULE_BODY_FUNC :
	//<body_func> ::= <stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_STMTS :
	//<stmts> ::= <stmt> <empty> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<stmts> ::= <stmt> <empty> <empty>"));}return null;
	case (int)RuleConstants.RULE_STMTS_TKSEMICOLON :
	//<stmts> ::= <stmts> 'tkSemiColon' <stmt>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<stmts> ::= <stmts> tkSemiColon <stmt>"));}return null;
	case (int)RuleConstants.RULE_STMTS1 :
	//<stmts1> ::= <stmt> <empty> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<stmts1> ::= <stmt> <empty> <empty>"));}return null;
	case (int)RuleConstants.RULE_STMTS12 :
	//<stmts1> ::= <stmts1> <stmt>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<stmts1> ::= <stmts1> <stmt>"));}return null;
	case (int)RuleConstants.RULE_EXPR_TKEQUAL :
	//<expr> ::= <expr> 'tkEqual' <add_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<expr> ::= <expr> tkEqual <add_expr>"));}return null;
	case (int)RuleConstants.RULE_EXPR_TKNOTEQUAL :
	//<expr> ::= <expr> 'tkNotEqual' <add_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<expr> ::= <expr> tkNotEqual <add_expr>"));}return null;
	case (int)RuleConstants.RULE_EXPR_TKMORE :
	//<expr> ::= <expr> 'tkMore' <add_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<expr> ::= <expr> tkMore <add_expr>"));}return null;
	case (int)RuleConstants.RULE_EXPR_TKLESS :
	//<expr> ::= <expr> 'tkLess' <add_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<expr> ::= <expr> tkLess <add_expr>"));}return null;
	case (int)RuleConstants.RULE_EXPR_TKMOREEQ :
	//<expr> ::= <expr> 'tkMoreEq' <add_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<expr> ::= <expr> tkMoreEq <add_expr>"));}return null;
	case (int)RuleConstants.RULE_EXPR_TKLESSEQ :
	//<expr> ::= <expr> 'tkLessEq' <add_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<expr> ::= <expr> tkLessEq <add_expr>"));}return null;
	case (int)RuleConstants.RULE_EXPR :
	//<expr> ::= <add_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIST_TKSQUAREOPEN_TKSQUARECLOSE :
	//<list> ::= 'tkSquareOpen' <list_elements> 'tkSquareClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<list> ::= tkSquareOpen <list_elements> tkSquareClose"));}return null;
	case (int)RuleConstants.RULE_LIST_TKSQUAREOPEN_TKDOT_TKDOT_TKSQUARECLOSE :
	//<list> ::= 'tkSquareOpen' <list_elements> 'tkDot' 'tkDot' <simple_expr> 'tkSquareClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<list> ::= tkSquareOpen <list_elements> tkDot tkDot <simple_expr> tkSquareClose"));}return null;
	case (int)RuleConstants.RULE_LIST_TKSQUAREOPEN_TKDOT_TKDOT_TKSQUARECLOSE2 :
	//<list> ::= 'tkSquareOpen' <list_elements> 'tkDot' 'tkDot' 'tkSquareClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<list> ::= tkSquareOpen <list_elements> tkDot tkDot tkSquareClose"));}return null;
	case (int)RuleConstants.RULE_LIST :
	//<list> ::= <list_constructor>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIST_TKCOLON :
	//<list> ::= <simple_expr> 'tkColon' <empty> <simple_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<list> ::= <simple_expr> tkColon <empty> <simple_expr>"));}return null;
	case (int)RuleConstants.RULE_LIST_CONSTRUCTOR_TKSQUAREOPEN_TKSPLIT_TKSQUARECLOSE :
	//<list_constructor> ::= 'tkSquareOpen' <simple_expr> 'tkSplit' <generators> <conditions_comma> 'tkSquareClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<list_constructor> ::= tkSquareOpen <simple_expr> tkSplit <generators> <conditions_comma> tkSquareClose"));}return null;
	case (int)RuleConstants.RULE_GENERATORS :
	//<generators> ::= <generator> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<generators> ::= <generator> <empty>"));}return null;
	case (int)RuleConstants.RULE_GENERATORS_TKCOMMA :
	//<generators> ::= <generators> 'tkComma' <generator>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<generators> ::= <generators> tkComma <generator>"));}return null;
	case (int)RuleConstants.RULE_GENERATOR_TKARROWGEN :
	//<generator> ::= <simple_expr> 'tkArrowGen' <simple_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<generator> ::= <simple_expr> tkArrowGen <simple_expr>"));}return null;
	case (int)RuleConstants.RULE_CONDITIONS_COMMA :
	//<conditions_comma> ::= <empty>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONDITIONS_COMMA_TKCOMMA :
	//<conditions_comma> ::= 'tkComma' <conditions>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<conditions_comma> ::= tkComma <conditions>"));}return null;
	case (int)RuleConstants.RULE_CONDITIONS :
	//<conditions> ::= <condition> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<conditions> ::= <condition> <empty>"));}return null;
	case (int)RuleConstants.RULE_CONDITIONS_TKCOMMA :
	//<conditions> ::= <conditions> 'tkComma' <condition>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<conditions> ::= <conditions> tkComma <condition>"));}return null;
	case (int)RuleConstants.RULE_CONDITION :
	//<condition> ::= <expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CORTEG_TKROUNDOPEN_TKCOMMA_TKROUNDCLOSE :
	//<corteg> ::= 'tkRoundOpen' <simple_expr> 'tkComma' <list_elements> 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<corteg> ::= tkRoundOpen <simple_expr> tkComma <list_elements> tkRoundClose"));}return null;
	case (int)RuleConstants.RULE_LIST_ELEMENTS :
	//<list_elements> ::= <empty>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIST_ELEMENTS2 :
	//<list_elements> ::= <simple_expr> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<list_elements> ::= <simple_expr> <empty>"));}return null;
	case (int)RuleConstants.RULE_LIST_ELEMENTS_TKCOMMA :
	//<list_elements> ::= <list_elements> 'tkComma' <simple_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<list_elements> ::= <list_elements> tkComma <simple_expr>"));}return null;
	case (int)RuleConstants.RULE_ADD_EXPR_TKAND :
	//<add_expr> ::= <add_expr> 'tkAnd' <mult_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<add_expr> ::= <add_expr> tkAnd <mult_expr>"));}return null;
	case (int)RuleConstants.RULE_ADD_EXPR :
	//<add_expr> ::= <add_expr> <addop> <mult_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<add_expr> ::= <add_expr> <addop> <mult_expr>"));}return null;
	case (int)RuleConstants.RULE_ADD_EXPR2 :
	//<add_expr> ::= <mult_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_MULT_EXPR_TKOR :
	//<mult_expr> ::= <mult_expr> 'tkOr' <negate_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<mult_expr> ::= <mult_expr> tkOr <negate_expr>"));}return null;
	case (int)RuleConstants.RULE_MULT_EXPR :
	//<mult_expr> ::= <mult_expr> <multop> <negate_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<mult_expr> ::= <mult_expr> <multop> <negate_expr>"));}return null;
	case (int)RuleConstants.RULE_MULT_EXPR2 :
	//<mult_expr> ::= <negate_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NEGATE_EXPR_TKMINUS :
	//<negate_expr> ::= 'tkMinus' <simple_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<negate_expr> ::= tkMinus <simple_expr>"));}return null;
	case (int)RuleConstants.RULE_NEGATE_EXPR_TKNOT :
	//<negate_expr> ::= 'tkNot' <simple_expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<negate_expr> ::= tkNot <simple_expr>"));}return null;
	case (int)RuleConstants.RULE_NEGATE_EXPR :
	//<negate_expr> ::= <simple_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_EXPR :
	//<simple_expr> ::= <simple_type_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_EXPR_TKROUNDOPEN_TKROUNDCLOSE :
	//<simple_expr> ::= 'tkRoundOpen' <expr> 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<simple_expr> ::= tkRoundOpen <expr> tkRoundClose"));}return null;
	case (int)RuleConstants.RULE_SIMPLE_EXPR2 :
	//<simple_expr> ::= <infix_expr> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<simple_expr> ::= <infix_expr> <empty>"));}return null;
	case (int)RuleConstants.RULE_SIMPLE_EXPR3 :
	//<simple_expr> ::= <variable>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_EXPR_TKLET_TKIN :
	//<simple_expr> ::= 'tkLet' <def_vars> 'tkIn' <body_func>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<simple_expr> ::= tkLet <def_vars> tkIn <body_func>"));}return null;
	case (int)RuleConstants.RULE_SIMPLE_EXPR4 :
	//<simple_expr> ::= <list>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_EXPR5 :
	//<simple_expr> ::= <corteg>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_EXPR6 :
	//<simple_expr> ::= <lambda_func> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<simple_expr> ::= <lambda_func> <empty>"));}return null;
	case (int)RuleConstants.RULE_SIMPLE_EXPR_TKROUNDOPEN_TKROUNDOPEN_TKROUNDCLOSE_TKROUNDCLOSE :
	//<simple_expr> ::= 'tkRoundOpen' 'tkRoundOpen' <lambda_func> 'tkRoundClose' <params_value> 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<simple_expr> ::= tkRoundOpen tkRoundOpen <lambda_func> tkRoundClose <params_value> tkRoundClose"));}return null;
	case (int)RuleConstants.RULE_SIMPLE_TYPE_EXPR_TKINT :
	//<simple_type_expr> ::= 'tkInt'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_TYPE_EXPR_TKDOUBLE :
	//<simple_type_expr> ::= 'tkDouble'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_TYPE_EXPR_TKBOOL :
	//<simple_type_expr> ::= 'tkBool'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_TYPE_EXPR_TKCHAR :
	//<simple_type_expr> ::= 'tkChar'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_TYPE_EXPR_TKSTRING :
	//<simple_type_expr> ::= 'tkString'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VARIABLE_EXPR :
	//<variable_expr> ::= <simple_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_DEF_VARS :
	//<def_vars> ::= <def_var> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<def_vars> ::= <def_var> <empty>"));}return null;
	case (int)RuleConstants.RULE_DEF_VARS_TKSEMICOLON :
	//<def_vars> ::= <def_vars> 'tkSemiColon' <def_var>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<def_vars> ::= <def_vars> tkSemiColon <def_var>"));}return null;
	case (int)RuleConstants.RULE_DEF_VAR_TKIDENT :
	//<def_var> ::= 'tkIdent' <params> <guard_body_list> <where_var>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<def_var> ::= tkIdent <params> <guard_body_list> <where_var>"));}return null;
	case (int)RuleConstants.RULE_GUARD_BODY_LIST :
	//<guard_body_list> ::= <guard_body> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<guard_body_list> ::= <guard_body> <empty>"));}return null;
	case (int)RuleConstants.RULE_GUARD_BODY_LIST2 :
	//<guard_body_list> ::= <guard_body_list> <guard_body>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<guard_body_list> ::= <guard_body_list> <guard_body>"));}return null;
	case (int)RuleConstants.RULE_GUARD_BODY_TKASSIGN :
	//<guard_body> ::= <guard> 'tkAssign' <body_func>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<guard_body> ::= <guard> tkAssign <body_func>"));}return null;
	case (int)RuleConstants.RULE_VARIABLE_TKIDENT :
	//<variable> ::= 'tkIdent'
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKIDENT_TKROUNDCLOSE :
	//<variable> ::= 'tkRoundOpen' 'tkIdent' <params_value> 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<variable> ::= tkRoundOpen tkIdent <params_value> tkRoundClose"));}return null;
	case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKROUNDCLOSE :
	//<variable> ::= 'tkRoundOpen' <variable> <params_value> 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<variable> ::= tkRoundOpen <variable> <params_value> tkRoundClose"));}return null;
	case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE :
	//<variable> ::= 'tkRoundOpen' 'tkQuote' 'tkIdent' 'tkQuote' <params_value> 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<variable> ::= tkRoundOpen tkQuote tkIdent tkQuote <params_value> tkRoundClose"));}return null;
	case (int)RuleConstants.RULE_INFIX_EXPR_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE :
	//<infix_expr> ::= 'tkRoundOpen' <simple_expr> 'tkQuote' 'tkIdent' 'tkQuote' <simple_expr> 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<infix_expr> ::= tkRoundOpen <simple_expr> tkQuote tkIdent tkQuote <simple_expr> tkRoundClose"));}return null;
	case (int)RuleConstants.RULE_INFIX_EXPR_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE2 :
	//<infix_expr> ::= 'tkRoundOpen' <simple_expr> 'tkQuote' 'tkIdent' 'tkQuote' 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<infix_expr> ::= tkRoundOpen <simple_expr> tkQuote tkIdent tkQuote tkRoundClose"));}return null;
	case (int)RuleConstants.RULE_INFIX_EXPR_TKROUNDOPEN_TKROUNDOPEN_TKQUOTE_TKIDENT_TKQUOTE_TKROUNDCLOSE_TKROUNDCLOSE :
	//<infix_expr> ::= 'tkRoundOpen' 'tkRoundOpen' <simple_expr> 'tkQuote' 'tkIdent' 'tkQuote' 'tkRoundClose' <params_value> 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<infix_expr> ::= tkRoundOpen tkRoundOpen <simple_expr> tkQuote tkIdent tkQuote tkRoundClose <params_value> tkRoundClose"));}return null;
	case (int)RuleConstants.RULE_MULTOP_TKSTAR :
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
	case (int)RuleConstants.RULE_STMT_TKIDENT_TKARROWGEN_TKIDENT :
	//<stmt> ::= 'tkIdent' 'tkArrowGen' 'tkIdent'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<stmt> ::= tkIdent tkArrowGen tkIdent"));}return null;
	case (int)RuleConstants.RULE_STMT_TKIF_TKTHEN_TKELSE :
	//<stmt> ::= 'tkIf' <expr> 'tkThen' <body_func> 'tkElse' <body_func>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<stmt> ::= tkIf <expr> tkThen <body_func> tkElse <body_func>"));}return null;
	case (int)RuleConstants.RULE_STMT_TKCASE_TKROUNDOPEN_TKROUNDCLOSE_TKOF :
	//<stmt> ::= 'tkCase' 'tkRoundOpen' <params1> 'tkRoundClose' 'tkOf' <case_variants>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<stmt> ::= tkCase tkRoundOpen <params1> tkRoundClose tkOf <case_variants>"));}return null;
	case (int)RuleConstants.RULE_STMT_TKDO_TKFIGUREOPEN_TKSEMICOLON_TKFIGURECLOSE :
	//<stmt> ::= 'tkDo' 'tkFigureOpen' <stmts> 'tkSemiColon' 'tkFigureClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<stmt> ::= tkDo tkFigureOpen <stmts> tkSemiColon tkFigureClose"));}return null;
	case (int)RuleConstants.RULE_STMT_TKDO :
	//<stmt> ::= 'tkDo' <stmts1>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<stmt> ::= tkDo <stmts1>"));}return null;
	case (int)RuleConstants.RULE_STMT_TKRETURN :
	//<stmt> ::= 'tkReturn' <expr>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<stmt> ::= tkReturn <expr>"));}return null;
	case (int)RuleConstants.RULE_STMT :
	//<stmt> ::= <func_call>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CASE_VARIANTS :
	//<case_variants> ::= <case_variant> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<case_variants> ::= <case_variant> <empty>"));}return null;
	case (int)RuleConstants.RULE_CASE_VARIANTS_TKSPLIT :
	//<case_variants> ::= <case_variants> 'tkSplit' <case_variant>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<case_variants> ::= <case_variants> tkSplit <case_variant>"));}return null;
	case (int)RuleConstants.RULE_CASE_VARIANT_TKROUNDOPEN_TKROUNDCLOSE_TKARROW :
	//<case_variant> ::= 'tkRoundOpen' <params1> 'tkRoundClose' 'tkArrow' <body_func>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<case_variant> ::= tkRoundOpen <params1> tkRoundClose tkArrow <body_func>"));}return null;
	case (int)RuleConstants.RULE_PARAMS1 :
	//<params1> ::= <param> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<params1> ::= <param> <empty>"));}return null;
	case (int)RuleConstants.RULE_PARAMS1_TKCOMMA :
	//<params1> ::= <params1> 'tkComma' <param>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<params1> ::= <params1> tkComma <param>"));}return null;
	case (int)RuleConstants.RULE_FUNC_CALL :
	//<func_call> ::= <expr> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<func_call> ::= <expr> <empty>"));}return null;
	case (int)RuleConstants.RULE_PARAMS_VALUE :
	//<params_value> ::= <param_value> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<params_value> ::= <param_value> <empty>"));}return null;
	case (int)RuleConstants.RULE_PARAMS_VALUE2 :
	//<params_value> ::= <params_value> <param_value>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<params_value> ::= <params_value> <param_value>"));}return null;
	case (int)RuleConstants.RULE_PARAM_VALUE :
	//<param_value> ::= <expr> <empty>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<param_value> ::= <expr> <empty>"));}return null;
	case (int)RuleConstants.RULE_LAMBDA_FUNC_TKLEFTSLASH_TKARROW :
	//<lambda_func> ::= 'tkLeftSlash' <params> 'tkArrow' <body_func>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<lambda_func> ::= tkLeftSlash <params> tkArrow <body_func>"));}return null;
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
public procedure_definition lambda(function_lambda_definition _function_lambda_definition)
{
    SyntaxTree.procedure_definition _func_def = new PascalABCCompiler.SyntaxTree.procedure_definition();
    SyntaxTree.method_name _method_name1 = new SyntaxTree.method_name(null, new SyntaxTree.ident(_function_lambda_definition.lambda_name), null);
    SyntaxTree.function_header _function_header1 = new SyntaxTree.function_header();

    object rt1 = new object();
    _function_header1.name = _method_name1;
    SyntaxTree.formal_parametres fps = new PascalABCCompiler.SyntaxTree.formal_parametres();
    _function_header1.parametres = _function_lambda_definition.formal_parametres;//fps;
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
            if (((procedure_definition)_functions[i]).proc_header.parametres == null)
                return 0;
            else
                return ((procedure_definition)_functions[i]).proc_header.parametres.params_list.Count;
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
        return ((function_lambda_definition)_function_lambda_definitions_after[i]).formal_parametres.params_list.Count;
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
        formal_parametres fp = ((function_lambda_definition)_function_lambda_definitions_after[i]).formal_parametres;
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

public type_definition func_type(int count)
{
    //int count = mc.parametres.expressions.Count;

    formal_parametres _formal_parametres = new formal_parametres();
    for (int i = 0; i < count; i++)
    {
        ident_list _ident_list = new ident_list();
        ident id = new ident("$a" + i.ToString());
        _ident_list.idents.Add(id);
        named_type_reference _named_type_reference1 = new named_type_reference();
        ident idtype1 = new ident("datatype");
        _named_type_reference1.names.Add(idtype1);
        typed_parametres _typed_parametres = new typed_parametres(_ident_list, (type_definition)_named_type_reference1, parametr_kind.none, null);
        _formal_parametres.params_list.Add(_typed_parametres);
    }
    named_type_reference _named_type_reference = new named_type_reference();
    ident idtype = new ident("datatype");
    _named_type_reference.names.Add(idtype);
    function_header _function_header = new function_header();
    _function_header.parametres = _formal_parametres;
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

public method_call  find_method_call_lambda(string name)
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
            if (((procedure_definition)_functions[i]).proc_header.parametres == null)
                return _params;
            else
            {
                for (int j = 0; j < ((procedure_definition)_functions[i]).proc_header.parametres.params_list.Count; j++)
                    _params.Add(((procedure_definition)_functions[i]).proc_header.parametres.params_list[j]);
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



} 
}
