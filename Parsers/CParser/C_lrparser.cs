
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using PascalABCCompiler.CParser.Errors;
using PascalABCCompiler.ParserTools;
using GoldParser;

namespace  PascalABCCompiler.CParser
{
    
    public class GPBParser_C:GPBParser
    {
        private Stack NodesStack;
        private int operation_value=0;
        private object left_node,right_node;
	private ConvertionTools Converter;
        internal SourceContextMap scm;


        public GPBParser_C(Stream stream)
            :base(stream)
        {
            parsertools = new C_parsertools();

	    Converter = new ConvertionTools(this);
        }

        public object Parse(string source)
        {
            NodesStack = new Stack();
            LRParser = new Parser(new StringReader(source), LanguageGrammar);
            parsertools.parser = LRParser;
            (parsertools as C_parsertools).scm = scm;
            LRParser.TrimReductions = false;
		Converter.errors=errors;
            while (true)
            {
                switch (LRParser.Parse())
                {
                    case ParseMessage.LexicalError:
                        errors.Add(new TokenReadError(this));
                        LRParser.PopInputToken();
                        if (errors.Count > max_errors)
                            return null;
                        break;

                    case ParseMessage.SyntaxError:
                        if ((LRParser.TokenSyntaxNode as syntax_tree_node)!= null) 
                            prev_node = LRParser.TokenSyntaxNode;                            
                        Error er = new PABCNETUnexpectedToken(this);
                        Symbol sym = LRParser.PopInputToken();
                        if (sym.SymbolType == SymbolType.End && errors.Count > 0)
                            return null;
                        errors.Add(er);
                        if (errors.Count > max_errors)
                            return null;
                        break;
                    case ParseMessage.Reduction:
                        LRParser.TokenSyntaxNode = CreateNonTerminalObject();
                        break;

                    case ParseMessage.Accept:
                        return LRParser.TokenSyntaxNode;

                    case ParseMessage.TokenRead:
                        LRParser.TokenSyntaxNode = CreateTerminalObject();
                        break;

                    case ParseMessage.InternalError:
                        errors.Add(new CompilerInternalError("CParser", new Exception("ParseMessage.InternalError")));
                        return null;

                    case ParseMessage.NotLoadedError:
                        errors.Add(new CompilerInternalError("CParser", new Exception("ParseMessage.NotLoadedError")));
                        return null;

                    case ParseMessage.CommentError:
                        errors.Add(new UnexpectedToken(this,"(EOF)"));
                        return null;

                    /*case ParseMessage.CommentBlockRead:
                        break;

                    case ParseMessage.CommentLineRead:
                        break;*/
                }
            }

        }        

    public enum SymbolConstants : int
    {
        SYMBOL_EOF                                    =   0, // (EOF)
        SYMBOL_ERROR                                  =   1, // (Error)
        SYMBOL_WHITESPACE                             =   2, // (Whitespace)
        SYMBOL_COMMENTEND                             =   3, // (Comment End)
        SYMBOL_COMMENTLINE                            =   4, // (Comment Line)
        SYMBOL_COMMENTSTART                           =   5, // (Comment Start)
        SYMBOL_ADD_ASSIGN                             =   6, // 'ADD_ASSIGN'
        SYMBOL_AND_ASSIGN                             =   7, // 'AND_ASSIGN'
        SYMBOL_AND_OP                                 =   8, // 'AND_OP'
        SYMBOL_AUTO                                   =   9, // 'AUTO'
        SYMBOL_B_AND_OP                               =  10, // 'B_AND_OP'
        SYMBOL_B_E_OR_OP                              =  11, // 'B_E_OR_OP'
        SYMBOL_B_I_OR_OP                              =  12, // 'B_I_OR_OP'
        SYMBOL_BOC                                    =  13, // 'BOC'
        SYMBOL_BREAK                                  =  14, // 'BREAK'
        SYMBOL_CASE                                   =  15, // 'CASE'
        SYMBOL_CHAR                                   =  16, // 'CHAR'
        SYMBOL_CHARLITERAL                            =  17, // 'CharLiteral'
        SYMBOL_CONST                                  =  18, // 'CONST'
        SYMBOL_CONTINUE                               =  19, // 'CONTINUE'
        SYMBOL_DEC_OP                                 =  20, // 'DEC_OP'
        SYMBOL_DECLITERAL                             =  21, // 'DecLiteral'
        SYMBOL_DEFAULT                                =  22, // 'DEFAULT'
        SYMBOL_DIV_ASSIGN                             =  23, // 'DIV_ASSIGN'
        SYMBOL_DIVISION                               =  24, // 'DIVISION'
        SYMBOL_DO                                     =  25, // 'DO'
        SYMBOL_DOUBLE                                 =  26, // 'DOUBLE'
        SYMBOL_ELLIPSIS                               =  27, // 'ELLIPSIS'
        SYMBOL_ELSE                                   =  28, // 'ELSE'
        SYMBOL_ENUM                                   =  29, // 'ENUM'
        SYMBOL_EQ_OP                                  =  30, // 'EQ_OP'
        SYMBOL_EQUAL                                  =  31, // 'EQUAL'
        SYMBOL_EXTERN                                 =  32, // 'EXTERN'
        SYMBOL_FLOAT                                  =  33, // 'FLOAT'
        SYMBOL_FLOATLITERAL                           =  34, // 'FloatLiteral'
        SYMBOL_FOR                                    =  35, // 'FOR'
        SYMBOL_GE_OP                                  =  36, // 'GE_OP'
        SYMBOL_GOTO                                   =  37, // 'GOTO'
        SYMBOL_GREATER                                =  38, // 'GREATER'
        SYMBOL_HEXLITERAL                             =  39, // 'HexLiteral'
        SYMBOL_IDENTIFIER                             =  40, // 'IDENTIFIER'
        SYMBOL_IF                                     =  41, // 'IF'
        SYMBOL_INC_OP                                 =  42, // 'INC_OP'
        SYMBOL_INLINE                                 =  43, // 'INLINE'
        SYMBOL_INT                                    =  44, // 'INT'
        SYMBOL_LE_OP                                  =  45, // 'LE_OP'
        SYMBOL_LEFT_ASSIGN                            =  46, // 'LEFT_ASSIGN'
        SYMBOL_LEFT_OP                                =  47, // 'LEFT_OP'
        SYMBOL_LESS                                   =  48, // 'LESS'
        SYMBOL_LONG                                   =  49, // 'LONG'
        SYMBOL_MINUS                                  =  50, // 'MINUS'
        SYMBOL_MOD                                    =  51, // 'MOD'
        SYMBOL_MOD_ASSIGN                             =  52, // 'MOD_ASSIGN'
        SYMBOL_MUL_ASSIGN                             =  53, // 'MUL_ASSIGN'
        SYMBOL_NE_OP                                  =  54, // 'NE_OP'
        SYMBOL_NOT                                    =  55, // 'NOT'
        SYMBOL_OCTLITERAL                             =  56, // 'OctLiteral'
        SYMBOL_OR_ASSIGN                              =  57, // 'OR_ASSIGN'
        SYMBOL_OR_OP                                  =  58, // 'OR_OP'
        SYMBOL_PARSEMODEEXPRESSION                    =  59, // 'ParseModeExpression'
        SYMBOL_PLUS                                   =  60, // 'PLUS'
        SYMBOL_PTR_OP                                 =  61, // 'PTR_OP'
        SYMBOL_REGISTER                               =  62, // 'REGISTER'
        SYMBOL_RETURN                                 =  63, // 'RETURN'
        SYMBOL_RIGHT_ASSIGN                           =  64, // 'RIGHT_ASSIGN'
        SYMBOL_RIGHT_OP                               =  65, // 'RIGHT_OP'
        SYMBOL_SEMICOLON                              =  66, // 'SEMICOLON'
        SYMBOL_SHORT                                  =  67, // 'SHORT'
        SYMBOL_SIGNED                                 =  68, // 'SIGNED'
        SYMBOL_SIZEOF                                 =  69, // 'SIZEOF'
        SYMBOL_STAR                                   =  70, // 'STAR'
        SYMBOL_STATIC                                 =  71, // 'STATIC'
        SYMBOL_STRING_LITERAL                         =  72, // 'STRING_LITERAL'
        SYMBOL_STRUCT                                 =  73, // 'STRUCT'
        SYMBOL_SUB_ASSIGN                             =  74, // 'SUB_ASSIGN'
        SYMBOL_SWITCH                                 =  75, // 'SWITCH'
        SYMBOL_TKCOLON                                =  76, // 'tkColon'
        SYMBOL_TKCOMMA                                =  77, // 'tkComma'
        SYMBOL_TKDOT                                  =  78, // 'tkDot'
        SYMBOL_TKFIGURECLOSE                          =  79, // 'tkFigureClose'
        SYMBOL_TKFIGUREOPEN                           =  80, // 'tkFigureOpen'
        SYMBOL_TKQUESTION                             =  81, // 'tkQuestion'
        SYMBOL_TKROUNDCLOSE                           =  82, // 'tkRoundClose'
        SYMBOL_TKROUNDOPEN                            =  83, // 'tkRoundOpen'
        SYMBOL_TKSQUARECLOSE                          =  84, // 'tkSquareClose'
        SYMBOL_TKSQUAREOPEN                           =  85, // 'tkSquareOpen'
        SYMBOL_TYPE_NAME                              =  86, // 'TYPE_NAME'
        SYMBOL_TYPEDEF                                =  87, // 'TYPEDEF'
        SYMBOL_UNION                                  =  88, // 'UNION'
        SYMBOL_UNSIGNED                               =  89, // 'UNSIGNED'
        SYMBOL_VOID                                   =  90, // 'VOID'
        SYMBOL_VOLATILE                               =  91, // 'VOLATILE'
        SYMBOL_WHILE                                  =  92, // 'WHILE'
        SYMBOL_XOR_ASSIGN                             =  93, // 'XOR_ASSIGN'
        SYMBOL_ABSTRACT_DECLARATOR                    =  94, // <abstract_declarator>
        SYMBOL_ADDITIVE_EXPRESSION                    =  95, // <additive_expression>
        SYMBOL_AND_EXPRESSION                         =  96, // <and_expression>
        SYMBOL_ARGUMENT_EXPRESSION_LIST               =  97, // <argument_expression_list>
        SYMBOL_ASSIGNMENT_EXPRESSION                  =  98, // <assignment_expression>
        SYMBOL_ASSIGNMENT_OPERATOR                    =  99, // <assignment_operator>
        SYMBOL_CAST_EXPRESSION                        = 100, // <cast_expression>
        SYMBOL_COMPOUND_STATEMENT                     = 101, // <compound_statement>
        SYMBOL_CONDITIONAL_EXPRESSION                 = 102, // <conditional_expression>
        SYMBOL_CONSTANT_EXPRESSION                    = 103, // <constant_expression>
        SYMBOL_DECLARATION                            = 104, // <declaration>
        SYMBOL_DECLARATION_LIST                       = 105, // <declaration_list>
        SYMBOL_DECLARATION_SPECIFIERS                 = 106, // <declaration_specifiers>
        SYMBOL_DECLARATOR                             = 107, // <declarator>
        SYMBOL_DECLARATOR_LIST                        = 108, // <declarator_list>
        SYMBOL_DIRECT_ABSTRACT_DECLARATOR             = 109, // <direct_abstract_declarator>
        SYMBOL_DIRECT_DECLARATOR                      = 110, // <direct_declarator>
        SYMBOL_DIRECT_DECLARATOR_IDENT                = 111, // <direct_declarator_ident>
        SYMBOL_EMPTY                                  = 112, // <empty>
        SYMBOL_ENUM_SPECIFIER                         = 113, // <enum_specifier>
        SYMBOL_ENUMERATOR                             = 114, // <enumerator>
        SYMBOL_ENUMERATOR_LIST                        = 115, // <enumerator_list>
        SYMBOL_EQUALITY_EXPRESSION                    = 116, // <equality_expression>
        SYMBOL_EXCLUSIVE_OR_EXPRESSION                = 117, // <exclusive_or_expression>
        SYMBOL_EXPRESSION                             = 118, // <expression>
        SYMBOL_EXPRESSION_STATEMENT                   = 119, // <expression_statement>
        SYMBOL_EXPRESSION_STATEMENT_OR_DECLARATION    = 120, // <expression_statement_or_declaration>
        SYMBOL_EXTERNAL_DECLARATION                   = 121, // <external_declaration>
        SYMBOL_EXTERNAL_DECLARATION_LIST              = 122, // <external_declaration_list>
        SYMBOL_FUNCTION_DEFINITION                    = 123, // <function_definition>
        SYMBOL_FUNCTION_DEFINITION_COMPOUND_STATEMENT = 124, // <function_definition_compound_statement>
        SYMBOL_FUNCTION_DEFINITION_HEADER             = 125, // <function_definition_header>
        SYMBOL_IDENTIFIER_LIST                        = 126, // <identifier_list>
        SYMBOL_INCLUSIVE_OR_EXPRESSION                = 127, // <inclusive_or_expression>
        SYMBOL_INIT_DECLARATOR                        = 128, // <init_declarator>
        SYMBOL_INIT_DECLARATOR_LIST                   = 129, // <init_declarator_list>
        SYMBOL_INITIALIZER                            = 130, // <initializer>
        SYMBOL_INITIALIZER_LIST                       = 131, // <initializer_list>
        SYMBOL_ITERATION_STATEMENT                    = 132, // <iteration_statement>
        SYMBOL_JUMP_STATEMENT                         = 133, // <jump_statement>
        SYMBOL_LABELED_STATEMENT                      = 134, // <labeled_statement>
        SYMBOL_LOGICAL_AND_EXPRESSION                 = 135, // <logical_and_expression>
        SYMBOL_LOGICAL_OR_EXPRESSION                  = 136, // <logical_or_expression>
        SYMBOL_MULTIPLICATIVE_EXPRESSION              = 137, // <multiplicative_expression>
        SYMBOL_ONTHER_SPECIFIER                       = 138, // <onther_specifier>
        SYMBOL_OPT_ENUMERATOR_LIST                    = 139, // <opt_enumerator_list>
        SYMBOL_PARAMETER_DECLARATION                  = 140, // <parameter_declaration>
        SYMBOL_PARAMETER_LIST                         = 141, // <parameter_list>
        SYMBOL_PARAMETER_TYPE_LIST                    = 142, // <parameter_type_list>
        SYMBOL_PARSE_MODE_EXPRESSION                  = 143, // <parse_mode_expression>
        SYMBOL_POINTER                                = 144, // <pointer>
        SYMBOL_POSTFIX_EXPRESSION                     = 145, // <postfix_expression>
        SYMBOL_PRIMARY_EXPRESSION                     = 146, // <primary_expression>
        SYMBOL_RELATIONAL_EXPRESSION                  = 147, // <relational_expression>
        SYMBOL_SELECTION_STATEMENT                    = 148, // <selection_statement>
        SYMBOL_SHIFT_EXPRESSION                       = 149, // <shift_expression>
        SYMBOL_SPECIFIER_QUALIFIER_LIST               = 150, // <specifier_qualifier_list>
        SYMBOL_START_RULE                             = 151, // <start_rule>
        SYMBOL_STATEMENT                              = 152, // <statement>
        SYMBOL_STATEMENT_LIST                         = 153, // <statement_list>
        SYMBOL_STORAGE_CLASS_SPECIFIER                = 154, // <storage_class_specifier>
        SYMBOL_STRUCT_DECLARATION                     = 155, // <struct_declaration>
        SYMBOL_STRUCT_DECLARATION_LIST                = 156, // <struct_declaration_list>
        SYMBOL_STRUCT_DECLARATOR                      = 157, // <struct_declarator>
        SYMBOL_STRUCT_DECLARATOR_LIST                 = 158, // <struct_declarator_list>
        SYMBOL_STRUCT_OR_UNION                        = 159, // <struct_or_union>
        SYMBOL_STRUCT_OR_UNION_SPECIFIER              = 160, // <struct_or_union_specifier>
        SYMBOL_TRANSLATION_UNIT                       = 161, // <translation_unit>
        SYMBOL_TYPE_NAME2                             = 162, // <type_name>
        SYMBOL_TYPE_QUALIFIER                         = 163, // <type_qualifier>
        SYMBOL_TYPE_QUALIFIER_LIST                    = 164, // <type_qualifier_list>
        SYMBOL_TYPE_SPECIFIER                         = 165, // <type_specifier>
        SYMBOL_TYPE_SPECIFIERS                        = 166, // <type_specifiers>
        SYMBOL_UNARY_EXPRESSION                       = 167, // <unary_expression>
        SYMBOL_UNARY_OPERATOR                         = 168  // <unary_operator>
    };

    public enum RuleConstants : int
    {
        RULE_START_RULE                                                      =   0, // <start_rule> ::= <translation_unit>
        RULE_START_RULE2                                                     =   1, // <start_rule> ::= <parse_mode_expression>
        RULE_PARSE_MODE_EXPRESSION_PARSEMODEEXPRESSION                       =   2, // <parse_mode_expression> ::= 'ParseModeExpression' <expression>
        RULE_PRIMARY_EXPRESSION_IDENTIFIER                                   =   3, // <primary_expression> ::= 'IDENTIFIER'
        RULE_PRIMARY_EXPRESSION_DECLITERAL                                   =   4, // <primary_expression> ::= 'DecLiteral'
        RULE_PRIMARY_EXPRESSION_OCTLITERAL                                   =   5, // <primary_expression> ::= 'OctLiteral'
        RULE_PRIMARY_EXPRESSION_HEXLITERAL                                   =   6, // <primary_expression> ::= 'HexLiteral'
        RULE_PRIMARY_EXPRESSION_FLOATLITERAL                                 =   7, // <primary_expression> ::= 'FloatLiteral'
        RULE_PRIMARY_EXPRESSION_CHARLITERAL                                  =   8, // <primary_expression> ::= 'CharLiteral'
        RULE_PRIMARY_EXPRESSION_STRING_LITERAL                               =   9, // <primary_expression> ::= 'STRING_LITERAL'
        RULE_PRIMARY_EXPRESSION_TKROUNDOPEN_TKROUNDCLOSE                     =  10, // <primary_expression> ::= 'tkRoundOpen' <expression> 'tkRoundClose'
        RULE_POSTFIX_EXPRESSION                                              =  11, // <postfix_expression> ::= <primary_expression>
        RULE_POSTFIX_EXPRESSION_TKSQUAREOPEN_TKSQUARECLOSE                   =  12, // <postfix_expression> ::= <postfix_expression> 'tkSquareOpen' <expression> 'tkSquareClose'
        RULE_POSTFIX_EXPRESSION_TKROUNDOPEN_TKROUNDCLOSE                     =  13, // <postfix_expression> ::= <postfix_expression> 'tkRoundOpen' 'tkRoundClose'
        RULE_POSTFIX_EXPRESSION_TKROUNDOPEN_TKROUNDCLOSE2                    =  14, // <postfix_expression> ::= <postfix_expression> 'tkRoundOpen' <argument_expression_list> 'tkRoundClose'
        RULE_POSTFIX_EXPRESSION_TKDOT_IDENTIFIER                             =  15, // <postfix_expression> ::= <postfix_expression> 'tkDot' 'IDENTIFIER'
        RULE_POSTFIX_EXPRESSION_PTR_OP_IDENTIFIER                            =  16, // <postfix_expression> ::= <postfix_expression> 'PTR_OP' 'IDENTIFIER'
        RULE_POSTFIX_EXPRESSION_INC_OP                                       =  17, // <postfix_expression> ::= <postfix_expression> 'INC_OP'
        RULE_POSTFIX_EXPRESSION_DEC_OP                                       =  18, // <postfix_expression> ::= <postfix_expression> 'DEC_OP'
        RULE_ARGUMENT_EXPRESSION_LIST                                        =  19, // <argument_expression_list> ::= <assignment_expression>
        RULE_ARGUMENT_EXPRESSION_LIST_TKCOMMA                                =  20, // <argument_expression_list> ::= <argument_expression_list> 'tkComma' <assignment_expression>
        RULE_UNARY_EXPRESSION                                                =  21, // <unary_expression> ::= <postfix_expression>
        RULE_UNARY_EXPRESSION_INC_OP                                         =  22, // <unary_expression> ::= 'INC_OP' <unary_expression>
        RULE_UNARY_EXPRESSION_DEC_OP                                         =  23, // <unary_expression> ::= 'DEC_OP' <unary_expression>
        RULE_UNARY_EXPRESSION2                                               =  24, // <unary_expression> ::= <unary_operator> <cast_expression>
        RULE_UNARY_EXPRESSION_SIZEOF                                         =  25, // <unary_expression> ::= 'SIZEOF' <unary_expression>
        RULE_UNARY_EXPRESSION_SIZEOF_TKROUNDOPEN_TKROUNDCLOSE                =  26, // <unary_expression> ::= 'SIZEOF' 'tkRoundOpen' <type_name> 'tkRoundClose'
        RULE_UNARY_OPERATOR_B_AND_OP                                         =  27, // <unary_operator> ::= 'B_AND_OP' <empty>
        RULE_UNARY_OPERATOR_STAR                                             =  28, // <unary_operator> ::= 'STAR' <empty>
        RULE_UNARY_OPERATOR_PLUS                                             =  29, // <unary_operator> ::= 'PLUS'
        RULE_UNARY_OPERATOR_MINUS                                            =  30, // <unary_operator> ::= 'MINUS'
        RULE_UNARY_OPERATOR_BOC                                              =  31, // <unary_operator> ::= 'BOC'
        RULE_UNARY_OPERATOR_NOT                                              =  32, // <unary_operator> ::= 'NOT'
        RULE_CAST_EXPRESSION                                                 =  33, // <cast_expression> ::= <unary_expression>
        RULE_CAST_EXPRESSION_TKROUNDOPEN_TKROUNDCLOSE                        =  34, // <cast_expression> ::= 'tkRoundOpen' <type_name> 'tkRoundClose' <cast_expression>
        RULE_MULTIPLICATIVE_EXPRESSION                                       =  35, // <multiplicative_expression> ::= <cast_expression>
        RULE_MULTIPLICATIVE_EXPRESSION_STAR                                  =  36, // <multiplicative_expression> ::= <multiplicative_expression> 'STAR' <cast_expression>
        RULE_MULTIPLICATIVE_EXPRESSION_DIVISION                              =  37, // <multiplicative_expression> ::= <multiplicative_expression> 'DIVISION' <cast_expression>
        RULE_MULTIPLICATIVE_EXPRESSION_MOD                                   =  38, // <multiplicative_expression> ::= <multiplicative_expression> 'MOD' <cast_expression>
        RULE_ADDITIVE_EXPRESSION                                             =  39, // <additive_expression> ::= <multiplicative_expression>
        RULE_ADDITIVE_EXPRESSION_PLUS                                        =  40, // <additive_expression> ::= <additive_expression> 'PLUS' <multiplicative_expression>
        RULE_ADDITIVE_EXPRESSION_MINUS                                       =  41, // <additive_expression> ::= <additive_expression> 'MINUS' <multiplicative_expression>
        RULE_SHIFT_EXPRESSION                                                =  42, // <shift_expression> ::= <additive_expression>
        RULE_SHIFT_EXPRESSION_LEFT_OP                                        =  43, // <shift_expression> ::= <shift_expression> 'LEFT_OP' <additive_expression>
        RULE_SHIFT_EXPRESSION_RIGHT_OP                                       =  44, // <shift_expression> ::= <shift_expression> 'RIGHT_OP' <additive_expression>
        RULE_RELATIONAL_EXPRESSION                                           =  45, // <relational_expression> ::= <shift_expression>
        RULE_RELATIONAL_EXPRESSION_LESS                                      =  46, // <relational_expression> ::= <relational_expression> 'LESS' <shift_expression>
        RULE_RELATIONAL_EXPRESSION_GREATER                                   =  47, // <relational_expression> ::= <relational_expression> 'GREATER' <shift_expression>
        RULE_RELATIONAL_EXPRESSION_LE_OP                                     =  48, // <relational_expression> ::= <relational_expression> 'LE_OP' <shift_expression>
        RULE_RELATIONAL_EXPRESSION_GE_OP                                     =  49, // <relational_expression> ::= <relational_expression> 'GE_OP' <shift_expression>
        RULE_EQUALITY_EXPRESSION                                             =  50, // <equality_expression> ::= <relational_expression>
        RULE_EQUALITY_EXPRESSION_EQ_OP                                       =  51, // <equality_expression> ::= <equality_expression> 'EQ_OP' <relational_expression>
        RULE_EQUALITY_EXPRESSION_NE_OP                                       =  52, // <equality_expression> ::= <equality_expression> 'NE_OP' <relational_expression>
        RULE_AND_EXPRESSION                                                  =  53, // <and_expression> ::= <equality_expression>
        RULE_AND_EXPRESSION_B_AND_OP                                         =  54, // <and_expression> ::= <and_expression> 'B_AND_OP' <equality_expression>
        RULE_EXCLUSIVE_OR_EXPRESSION                                         =  55, // <exclusive_or_expression> ::= <and_expression>
        RULE_EXCLUSIVE_OR_EXPRESSION_B_E_OR_OP                               =  56, // <exclusive_or_expression> ::= <exclusive_or_expression> 'B_E_OR_OP' <and_expression>
        RULE_INCLUSIVE_OR_EXPRESSION                                         =  57, // <inclusive_or_expression> ::= <exclusive_or_expression>
        RULE_INCLUSIVE_OR_EXPRESSION_B_I_OR_OP                               =  58, // <inclusive_or_expression> ::= <inclusive_or_expression> 'B_I_OR_OP' <exclusive_or_expression>
        RULE_LOGICAL_AND_EXPRESSION                                          =  59, // <logical_and_expression> ::= <inclusive_or_expression>
        RULE_LOGICAL_AND_EXPRESSION_AND_OP                                   =  60, // <logical_and_expression> ::= <logical_and_expression> 'AND_OP' <inclusive_or_expression>
        RULE_LOGICAL_OR_EXPRESSION                                           =  61, // <logical_or_expression> ::= <logical_and_expression>
        RULE_LOGICAL_OR_EXPRESSION_OR_OP                                     =  62, // <logical_or_expression> ::= <logical_or_expression> 'OR_OP' <logical_and_expression>
        RULE_CONDITIONAL_EXPRESSION                                          =  63, // <conditional_expression> ::= <logical_or_expression>
        RULE_CONDITIONAL_EXPRESSION_TKQUESTION_TKCOLON                       =  64, // <conditional_expression> ::= <logical_or_expression> 'tkQuestion' <expression> 'tkColon' <conditional_expression>
        RULE_ASSIGNMENT_EXPRESSION                                           =  65, // <assignment_expression> ::= <conditional_expression>
        RULE_ASSIGNMENT_EXPRESSION2                                          =  66, // <assignment_expression> ::= <unary_expression> <assignment_operator> <assignment_expression>
        RULE_ASSIGNMENT_OPERATOR_EQUAL                                       =  67, // <assignment_operator> ::= 'EQUAL'
        RULE_ASSIGNMENT_OPERATOR_MUL_ASSIGN                                  =  68, // <assignment_operator> ::= 'MUL_ASSIGN'
        RULE_ASSIGNMENT_OPERATOR_DIV_ASSIGN                                  =  69, // <assignment_operator> ::= 'DIV_ASSIGN'
        RULE_ASSIGNMENT_OPERATOR_MOD_ASSIGN                                  =  70, // <assignment_operator> ::= 'MOD_ASSIGN'
        RULE_ASSIGNMENT_OPERATOR_ADD_ASSIGN                                  =  71, // <assignment_operator> ::= 'ADD_ASSIGN'
        RULE_ASSIGNMENT_OPERATOR_SUB_ASSIGN                                  =  72, // <assignment_operator> ::= 'SUB_ASSIGN'
        RULE_ASSIGNMENT_OPERATOR_LEFT_ASSIGN                                 =  73, // <assignment_operator> ::= 'LEFT_ASSIGN'
        RULE_ASSIGNMENT_OPERATOR_RIGHT_ASSIGN                                =  74, // <assignment_operator> ::= 'RIGHT_ASSIGN'
        RULE_ASSIGNMENT_OPERATOR_AND_ASSIGN                                  =  75, // <assignment_operator> ::= 'AND_ASSIGN'
        RULE_ASSIGNMENT_OPERATOR_XOR_ASSIGN                                  =  76, // <assignment_operator> ::= 'XOR_ASSIGN'
        RULE_ASSIGNMENT_OPERATOR_OR_ASSIGN                                   =  77, // <assignment_operator> ::= 'OR_ASSIGN'
        RULE_EXPRESSION                                                      =  78, // <expression> ::= <assignment_expression>
        RULE_EXPRESSION_TKCOMMA                                              =  79, // <expression> ::= <expression> 'tkComma' <assignment_expression>
        RULE_CONSTANT_EXPRESSION                                             =  80, // <constant_expression> ::= <conditional_expression>
        RULE_DECLARATION_SEMICOLON                                           =  81, // <declaration> ::= <declaration_specifiers> 'SEMICOLON'
        RULE_DECLARATION_SEMICOLON2                                          =  82, // <declaration> ::= <declaration_specifiers> <init_declarator_list> 'SEMICOLON'
        RULE_DECLARATION_TYPEDEF_SEMICOLON                                   =  83, // <declaration> ::= 'TYPEDEF' <type_specifiers> <declarator_list> 'SEMICOLON'
        RULE_DECLARATOR_LIST                                                 =  84, // <declarator_list> ::= <declarator>
        RULE_DECLARATOR_LIST_TKCOMMA                                         =  85, // <declarator_list> ::= <declarator_list> 'tkComma' <declarator>
        RULE_INIT_DECLARATOR_LIST                                            =  86, // <init_declarator_list> ::= <init_declarator>
        RULE_INIT_DECLARATOR_LIST_TKCOMMA                                    =  87, // <init_declarator_list> ::= <init_declarator_list> 'tkComma' <init_declarator>
        RULE_INIT_DECLARATOR                                                 =  88, // <init_declarator> ::= <declarator>
        RULE_INIT_DECLARATOR_EQUAL                                           =  89, // <init_declarator> ::= <declarator> 'EQUAL' <initializer>
        RULE_DECLARATION_SPECIFIERS                                          =  90, // <declaration_specifiers> ::= <type_specifiers>
        RULE_DECLARATION_SPECIFIERS2                                         =  91, // <declaration_specifiers> ::= <storage_class_specifier>
        RULE_DECLARATION_SPECIFIERS3                                         =  92, // <declaration_specifiers> ::= <storage_class_specifier> <type_specifiers>
        RULE_STORAGE_CLASS_SPECIFIER_REGISTER                                =  93, // <storage_class_specifier> ::= 'REGISTER'
        RULE_STORAGE_CLASS_SPECIFIER_EXTERN                                  =  94, // <storage_class_specifier> ::= 'EXTERN'
        RULE_STORAGE_CLASS_SPECIFIER_STATIC                                  =  95, // <storage_class_specifier> ::= 'STATIC'
        RULE_STORAGE_CLASS_SPECIFIER_AUTO                                    =  96, // <storage_class_specifier> ::= 'AUTO'
        RULE_STORAGE_CLASS_SPECIFIER                                         =  97, // <storage_class_specifier> ::= <onther_specifier>
        RULE_ONTHER_SPECIFIER_INLINE                                         =  98, // <onther_specifier> ::= 'INLINE'
        RULE_TYPE_SPECIFIERS                                                 =  99, // <type_specifiers> ::= <type_specifier>
        RULE_TYPE_SPECIFIERS2                                                = 100, // <type_specifiers> ::= <type_specifiers> <type_specifier>
        RULE_TYPE_SPECIFIERS3                                                = 101, // <type_specifiers> ::= <type_qualifier>
        RULE_TYPE_QUALIFIER_CONST                                            = 102, // <type_qualifier> ::= 'CONST'
        RULE_TYPE_QUALIFIER_VOLATILE                                         = 103, // <type_qualifier> ::= 'VOLATILE'
        RULE_TYPE_SPECIFIER_VOID                                             = 104, // <type_specifier> ::= 'VOID'
        RULE_TYPE_SPECIFIER_CHAR                                             = 105, // <type_specifier> ::= 'CHAR'
        RULE_TYPE_SPECIFIER_SHORT                                            = 106, // <type_specifier> ::= 'SHORT'
        RULE_TYPE_SPECIFIER_INT                                              = 107, // <type_specifier> ::= 'INT'
        RULE_TYPE_SPECIFIER_LONG                                             = 108, // <type_specifier> ::= 'LONG'
        RULE_TYPE_SPECIFIER_FLOAT                                            = 109, // <type_specifier> ::= 'FLOAT'
        RULE_TYPE_SPECIFIER_DOUBLE                                           = 110, // <type_specifier> ::= 'DOUBLE'
        RULE_TYPE_SPECIFIER_UNSIGNED                                         = 111, // <type_specifier> ::= 'UNSIGNED'
        RULE_TYPE_SPECIFIER_SIGNED                                           = 112, // <type_specifier> ::= 'SIGNED'
        RULE_TYPE_SPECIFIER                                                  = 113, // <type_specifier> ::= <struct_or_union_specifier>
        RULE_TYPE_SPECIFIER2                                                 = 114, // <type_specifier> ::= <enum_specifier>
        RULE_TYPE_SPECIFIER_TYPE_NAME                                        = 115, // <type_specifier> ::= 'TYPE_NAME' <empty>
        RULE_STRUCT_OR_UNION_SPECIFIER_IDENTIFIER_TKFIGUREOPEN_TKFIGURECLOSE = 116, // <struct_or_union_specifier> ::= <struct_or_union> 'IDENTIFIER' 'tkFigureOpen' <struct_declaration_list> 'tkFigureClose'
        RULE_STRUCT_OR_UNION_SPECIFIER_TKFIGUREOPEN_TKFIGURECLOSE            = 117, // <struct_or_union_specifier> ::= <struct_or_union> 'tkFigureOpen' <struct_declaration_list> 'tkFigureClose'
        RULE_STRUCT_OR_UNION_SPECIFIER_IDENTIFIER                            = 118, // <struct_or_union_specifier> ::= <struct_or_union> 'IDENTIFIER'
        RULE_STRUCT_OR_UNION_STRUCT                                          = 119, // <struct_or_union> ::= 'STRUCT'
        RULE_STRUCT_OR_UNION_UNION                                           = 120, // <struct_or_union> ::= 'UNION'
        RULE_STRUCT_DECLARATION_LIST                                         = 121, // <struct_declaration_list> ::= <struct_declaration>
        RULE_STRUCT_DECLARATION_LIST2                                        = 122, // <struct_declaration_list> ::= <struct_declaration_list> <struct_declaration>
        RULE_STRUCT_DECLARATION_SEMICOLON                                    = 123, // <struct_declaration> ::= <specifier_qualifier_list> <struct_declarator_list> 'SEMICOLON'
        RULE_SPECIFIER_QUALIFIER_LIST                                        = 124, // <specifier_qualifier_list> ::= <type_specifiers>
        RULE_STRUCT_DECLARATOR_LIST                                          = 125, // <struct_declarator_list> ::= <struct_declarator>
        RULE_STRUCT_DECLARATOR_LIST_TKCOMMA                                  = 126, // <struct_declarator_list> ::= <struct_declarator_list> 'tkComma' <struct_declarator>
        RULE_STRUCT_DECLARATOR                                               = 127, // <struct_declarator> ::= <declarator>
        RULE_STRUCT_DECLARATOR_TKCOLON                                       = 128, // <struct_declarator> ::= 'tkColon' <constant_expression>
        RULE_STRUCT_DECLARATOR_TKCOLON2                                      = 129, // <struct_declarator> ::= <declarator> 'tkColon' <constant_expression>
        RULE_ENUM_SPECIFIER_ENUM_TKFIGUREOPEN_TKFIGURECLOSE                  = 130, // <enum_specifier> ::= 'ENUM' 'tkFigureOpen' <opt_enumerator_list> 'tkFigureClose'
        RULE_ENUM_SPECIFIER_ENUM_IDENTIFIER_TKFIGUREOPEN_TKFIGURECLOSE       = 131, // <enum_specifier> ::= 'ENUM' 'IDENTIFIER' 'tkFigureOpen' <opt_enumerator_list> 'tkFigureClose'
        RULE_ENUM_SPECIFIER_ENUM_IDENTIFIER                                  = 132, // <enum_specifier> ::= 'ENUM' 'IDENTIFIER'
        RULE_OPT_ENUMERATOR_LIST                                             = 133, // <opt_enumerator_list> ::= <enumerator_list>
        RULE_OPT_ENUMERATOR_LIST2                                            = 134, // <opt_enumerator_list> ::= 
        RULE_ENUMERATOR_LIST                                                 = 135, // <enumerator_list> ::= <enumerator>
        RULE_ENUMERATOR_LIST_TKCOMMA                                         = 136, // <enumerator_list> ::= <enumerator_list> 'tkComma' <enumerator>
        RULE_ENUMERATOR_IDENTIFIER                                           = 137, // <enumerator> ::= 'IDENTIFIER'
        RULE_ENUMERATOR_IDENTIFIER_EQUAL                                     = 138, // <enumerator> ::= 'IDENTIFIER' 'EQUAL' <constant_expression>
        RULE_DECLARATOR                                                      = 139, // <declarator> ::= <pointer> <direct_declarator>
        RULE_DECLARATOR2                                                     = 140, // <declarator> ::= <direct_declarator>
        RULE_DIRECT_DECLARATOR                                               = 141, // <direct_declarator> ::= <direct_declarator_ident> <empty>
        RULE_DIRECT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE                      = 142, // <direct_declarator> ::= 'tkRoundOpen' <declarator> 'tkRoundClose'
        RULE_DIRECT_DECLARATOR_TKSQUAREOPEN_TKSQUARECLOSE                    = 143, // <direct_declarator> ::= <direct_declarator> 'tkSquareOpen' <constant_expression> 'tkSquareClose'
        RULE_DIRECT_DECLARATOR_TKSQUAREOPEN_TKSQUARECLOSE2                   = 144, // <direct_declarator> ::= <direct_declarator> 'tkSquareOpen' 'tkSquareClose'
        RULE_DIRECT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE2                     = 145, // <direct_declarator> ::= <direct_declarator> 'tkRoundOpen' <parameter_type_list> 'tkRoundClose'
        RULE_DIRECT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE3                     = 146, // <direct_declarator> ::= <direct_declarator> 'tkRoundOpen' <identifier_list> 'tkRoundClose'
        RULE_DIRECT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE4                     = 147, // <direct_declarator> ::= <direct_declarator> 'tkRoundOpen' 'tkRoundClose'
        RULE_DIRECT_DECLARATOR_IDENT_IDENTIFIER                              = 148, // <direct_declarator_ident> ::= 'IDENTIFIER' <empty>
        RULE_POINTER_STAR                                                    = 149, // <pointer> ::= 'STAR'
        RULE_POINTER_STAR2                                                   = 150, // <pointer> ::= <pointer> 'STAR'
        RULE_TYPE_QUALIFIER_LIST                                             = 151, // <type_qualifier_list> ::= <type_qualifier>
        RULE_TYPE_QUALIFIER_LIST2                                            = 152, // <type_qualifier_list> ::= <type_qualifier_list> <type_qualifier>
        RULE_PARAMETER_TYPE_LIST                                             = 153, // <parameter_type_list> ::= <parameter_list>
        RULE_PARAMETER_TYPE_LIST_TKCOMMA_ELLIPSIS                            = 154, // <parameter_type_list> ::= <parameter_list> 'tkComma' 'ELLIPSIS'
        RULE_PARAMETER_LIST                                                  = 155, // <parameter_list> ::= <parameter_declaration> <empty>
        RULE_PARAMETER_LIST_TKCOMMA                                          = 156, // <parameter_list> ::= <parameter_list> 'tkComma' <parameter_declaration>
        RULE_PARAMETER_DECLARATION                                           = 157, // <parameter_declaration> ::= <declaration_specifiers> <declarator>
        RULE_PARAMETER_DECLARATION2                                          = 158, // <parameter_declaration> ::= <declaration_specifiers> <abstract_declarator>
        RULE_PARAMETER_DECLARATION3                                          = 159, // <parameter_declaration> ::= <declaration_specifiers>
        RULE_IDENTIFIER_LIST_IDENTIFIER                                      = 160, // <identifier_list> ::= 'IDENTIFIER'
        RULE_IDENTIFIER_LIST_TKCOMMA_IDENTIFIER                              = 161, // <identifier_list> ::= <identifier_list> 'tkComma' 'IDENTIFIER'
        RULE_TYPE_NAME                                                       = 162, // <type_name> ::= <specifier_qualifier_list>
        RULE_TYPE_NAME2                                                      = 163, // <type_name> ::= <specifier_qualifier_list> <abstract_declarator>
        RULE_ABSTRACT_DECLARATOR                                             = 164, // <abstract_declarator> ::= <pointer>
        RULE_ABSTRACT_DECLARATOR2                                            = 165, // <abstract_declarator> ::= <direct_abstract_declarator>
        RULE_ABSTRACT_DECLARATOR3                                            = 166, // <abstract_declarator> ::= <pointer> <direct_abstract_declarator>
        RULE_DIRECT_ABSTRACT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE             = 167, // <direct_abstract_declarator> ::= 'tkRoundOpen' <abstract_declarator> 'tkRoundClose'
        RULE_DIRECT_ABSTRACT_DECLARATOR_TKSQUAREOPEN_TKSQUARECLOSE           = 168, // <direct_abstract_declarator> ::= 'tkSquareOpen' 'tkSquareClose'
        RULE_DIRECT_ABSTRACT_DECLARATOR_TKSQUAREOPEN_TKSQUARECLOSE2          = 169, // <direct_abstract_declarator> ::= 'tkSquareOpen' <constant_expression> 'tkSquareClose'
        RULE_DIRECT_ABSTRACT_DECLARATOR_TKSQUAREOPEN_TKSQUARECLOSE3          = 170, // <direct_abstract_declarator> ::= <direct_abstract_declarator> 'tkSquareOpen' 'tkSquareClose'
        RULE_DIRECT_ABSTRACT_DECLARATOR_TKSQUAREOPEN_TKSQUARECLOSE4          = 171, // <direct_abstract_declarator> ::= <direct_abstract_declarator> 'tkSquareOpen' <constant_expression> 'tkSquareClose'
        RULE_DIRECT_ABSTRACT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE2            = 172, // <direct_abstract_declarator> ::= 'tkRoundOpen' 'tkRoundClose'
        RULE_DIRECT_ABSTRACT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE3            = 173, // <direct_abstract_declarator> ::= 'tkRoundOpen' <parameter_type_list> 'tkRoundClose'
        RULE_DIRECT_ABSTRACT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE4            = 174, // <direct_abstract_declarator> ::= <direct_abstract_declarator> 'tkRoundOpen' 'tkRoundClose'
        RULE_DIRECT_ABSTRACT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE5            = 175, // <direct_abstract_declarator> ::= <direct_abstract_declarator> 'tkRoundOpen' <parameter_type_list> 'tkRoundClose'
        RULE_INITIALIZER                                                     = 176, // <initializer> ::= <assignment_expression>
        RULE_INITIALIZER_TKFIGUREOPEN_TKFIGURECLOSE                          = 177, // <initializer> ::= 'tkFigureOpen' <initializer_list> 'tkFigureClose'
        RULE_INITIALIZER_TKFIGUREOPEN_TKCOMMA_TKFIGURECLOSE                  = 178, // <initializer> ::= 'tkFigureOpen' <initializer_list> 'tkComma' 'tkFigureClose'
        RULE_INITIALIZER_LIST                                                = 179, // <initializer_list> ::= <initializer>
        RULE_INITIALIZER_LIST_TKCOMMA                                        = 180, // <initializer_list> ::= <initializer_list> 'tkComma' <initializer>
        RULE_STATEMENT                                                       = 181, // <statement> ::= <labeled_statement>
        RULE_STATEMENT2                                                      = 182, // <statement> ::= <compound_statement>
        RULE_STATEMENT3                                                      = 183, // <statement> ::= <expression_statement>
        RULE_STATEMENT4                                                      = 184, // <statement> ::= <selection_statement>
        RULE_STATEMENT5                                                      = 185, // <statement> ::= <iteration_statement>
        RULE_STATEMENT6                                                      = 186, // <statement> ::= <jump_statement>
        RULE_LABELED_STATEMENT_IDENTIFIER_TKCOLON                            = 187, // <labeled_statement> ::= 'IDENTIFIER' 'tkColon' <statement>
        RULE_LABELED_STATEMENT_CASE_TKCOLON                                  = 188, // <labeled_statement> ::= 'CASE' <constant_expression> 'tkColon' <statement>
        RULE_LABELED_STATEMENT_DEFAULT_TKCOLON                               = 189, // <labeled_statement> ::= 'DEFAULT' 'tkColon' <statement>
        RULE_COMPOUND_STATEMENT_TKFIGUREOPEN_TKFIGURECLOSE                   = 190, // <compound_statement> ::= 'tkFigureOpen' 'tkFigureClose'
        RULE_COMPOUND_STATEMENT_TKFIGUREOPEN_TKFIGURECLOSE2                  = 191, // <compound_statement> ::= 'tkFigureOpen' <statement_list> 'tkFigureClose'
        RULE_COMPOUND_STATEMENT_TKFIGUREOPEN_TKFIGURECLOSE3                  = 192, // <compound_statement> ::= 'tkFigureOpen' <declaration_list> 'tkFigureClose'
        RULE_COMPOUND_STATEMENT_TKFIGUREOPEN_TKFIGURECLOSE4                  = 193, // <compound_statement> ::= 'tkFigureOpen' <declaration_list> <statement_list> 'tkFigureClose'
        RULE_DECLARATION_LIST                                                = 194, // <declaration_list> ::= <declaration>
        RULE_DECLARATION_LIST2                                               = 195, // <declaration_list> ::= <declaration_list> <declaration>
        RULE_STATEMENT_LIST                                                  = 196, // <statement_list> ::= <statement>
        RULE_STATEMENT_LIST2                                                 = 197, // <statement_list> ::= <statement_list> <statement>
        RULE_EXPRESSION_STATEMENT_SEMICOLON                                  = 198, // <expression_statement> ::= 'SEMICOLON' <empty>
        RULE_EXPRESSION_STATEMENT_SEMICOLON2                                 = 199, // <expression_statement> ::= <expression> 'SEMICOLON'
        RULE_EXPRESSION_STATEMENT_OR_DECLARATION                             = 200, // <expression_statement_or_declaration> ::= <expression_statement>
        RULE_EXPRESSION_STATEMENT_OR_DECLARATION2                            = 201, // <expression_statement_or_declaration> ::= <declaration>
        RULE_SELECTION_STATEMENT_IF_TKROUNDOPEN_TKROUNDCLOSE                 = 202, // <selection_statement> ::= 'IF' 'tkRoundOpen' <expression> 'tkRoundClose' <statement>
        RULE_SELECTION_STATEMENT_IF_TKROUNDOPEN_TKROUNDCLOSE_ELSE            = 203, // <selection_statement> ::= 'IF' 'tkRoundOpen' <expression> 'tkRoundClose' <statement> 'ELSE' <statement>
        RULE_SELECTION_STATEMENT_SWITCH_TKROUNDOPEN_TKROUNDCLOSE             = 204, // <selection_statement> ::= 'SWITCH' 'tkRoundOpen' <expression> 'tkRoundClose' <statement>
        RULE_ITERATION_STATEMENT_WHILE_TKROUNDOPEN_TKROUNDCLOSE              = 205, // <iteration_statement> ::= 'WHILE' 'tkRoundOpen' <expression> 'tkRoundClose' <statement>
        RULE_ITERATION_STATEMENT_DO_WHILE_TKROUNDOPEN_TKROUNDCLOSE_SEMICOLON = 206, // <iteration_statement> ::= 'DO' <statement> 'WHILE' 'tkRoundOpen' <expression> 'tkRoundClose' 'SEMICOLON'
        RULE_ITERATION_STATEMENT_FOR_TKROUNDOPEN_TKROUNDCLOSE                = 207, // <iteration_statement> ::= 'FOR' 'tkRoundOpen' <expression_statement_or_declaration> <expression_statement> 'tkRoundClose' <statement>
        RULE_ITERATION_STATEMENT_FOR_TKROUNDOPEN_TKROUNDCLOSE2               = 208, // <iteration_statement> ::= 'FOR' 'tkRoundOpen' <expression_statement_or_declaration> <expression_statement> <expression> 'tkRoundClose' <statement>
        RULE_JUMP_STATEMENT_GOTO_IDENTIFIER_SEMICOLON                        = 209, // <jump_statement> ::= 'GOTO' 'IDENTIFIER' 'SEMICOLON'
        RULE_JUMP_STATEMENT_CONTINUE_SEMICOLON                               = 210, // <jump_statement> ::= 'CONTINUE' 'SEMICOLON'
        RULE_JUMP_STATEMENT_BREAK_SEMICOLON                                  = 211, // <jump_statement> ::= 'BREAK' 'SEMICOLON'
        RULE_JUMP_STATEMENT_RETURN_SEMICOLON                                 = 212, // <jump_statement> ::= 'RETURN' 'SEMICOLON'
        RULE_JUMP_STATEMENT_RETURN_SEMICOLON2                                = 213, // <jump_statement> ::= 'RETURN' <expression> 'SEMICOLON'
        RULE_TRANSLATION_UNIT                                                = 214, // <translation_unit> ::= <external_declaration_list>
        RULE_EXTERNAL_DECLARATION_LIST                                       = 215, // <external_declaration_list> ::= <external_declaration>
        RULE_EXTERNAL_DECLARATION_LIST2                                      = 216, // <external_declaration_list> ::= <external_declaration_list> <external_declaration>
        RULE_EXTERNAL_DECLARATION                                            = 217, // <external_declaration> ::= <function_definition>
        RULE_EXTERNAL_DECLARATION2                                           = 218, // <external_declaration> ::= <declaration> <empty>
        RULE_FUNCTION_DEFINITION_HEADER                                      = 219, // <function_definition_header> ::= <declaration_specifiers> <declarator>
        RULE_FUNCTION_DEFINITION_HEADER2                                     = 220, // <function_definition_header> ::= <declarator> <empty>
        RULE_FUNCTION_DEFINITION_COMPOUND_STATEMENT                          = 221, // <function_definition_compound_statement> ::= <declaration_list> <compound_statement>
        RULE_FUNCTION_DEFINITION_COMPOUND_STATEMENT2                         = 222, // <function_definition_compound_statement> ::= <compound_statement>
        RULE_FUNCTION_DEFINITION                                             = 223, // <function_definition> ::= <function_definition_header> <function_definition_compound_statement>
        RULE_EMPTY                                                           = 224  // <empty> ::= 
    };

        private Object CreateTerminalObject()
        {
            switch ((SymbolConstants) LRParser.TokenSymbol.Index)
            {
		case SymbolConstants.SYMBOL_EOF :
		//(EOF)
		//TERMINAL:EOF
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ERROR :
		//(Error)
		//TERMINAL:Error
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_WHITESPACE :
		//(Whitespace)
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'Whitespace' return NULL! \n\r");*/return null;

		case SymbolConstants.SYMBOL_COMMENTEND :
		//(Comment End)
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'Comment End' return NULL! \n\r");*/return null;

		case SymbolConstants.SYMBOL_COMMENTLINE :
		//(Comment Line)
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'Comment Line' return NULL! \n\r");*/return null;

		case SymbolConstants.SYMBOL_COMMENTSTART :
		//(Comment Start)
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'Comment Start' return NULL! \n\r");*/return null;

		case SymbolConstants.SYMBOL_ADD_ASSIGN :
		//'ADD_ASSIGN'

		{
			op_type_node _op_type_node=new op_type_node(Operators.AssignmentAddition);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_AND_ASSIGN :
		//'AND_ASSIGN'

		{
			op_type_node _op_type_node=new op_type_node(Operators.AssignmentBitwiseAND);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_AND_OP :
		//'AND_OP'
return null;

		case SymbolConstants.SYMBOL_AUTO :
		//'AUTO'

		{
			type_definition_attr _type_definition_attr=new type_definition_attr(definition_attribute.Auto);
			_type_definition_attr.source_context=parsertools.GetTokenSourceContext();
			
			return _type_definition_attr;
		}


		case SymbolConstants.SYMBOL_B_AND_OP :
		//'B_AND_OP'

		{
			op_type_node _op_type_node=new op_type_node(Operators.BitwiseAND);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_B_E_OR_OP :
		//'B_E_OR_OP'
return null;

		case SymbolConstants.SYMBOL_B_I_OR_OP :
		//'B_I_OR_OP'
return null;

		case SymbolConstants.SYMBOL_BOC :
		//'BOC'

		{
			op_type_node _op_type_node=new op_type_node(Operators.BitwiseNOT);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_BREAK :
		//'BREAK'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_CASE :
		//'CASE'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_CHAR :
		//'CHAR'

		{
			c_scalar_type _c_scalar_type=new c_scalar_type(c_scalar_type_name.tn_char,c_scalar_sign.none);
			_c_scalar_type.source_context=parsertools.GetTokenSourceContext();
			
			return _c_scalar_type;
		}


		case SymbolConstants.SYMBOL_CHARLITERAL :
		//'CharLiteral'
return parsertools.create_char_const(this);

		case SymbolConstants.SYMBOL_CONST :
		//'CONST'

		{
			type_definition_attr _type_definition_attr=new type_definition_attr(definition_attribute.Const);
			_type_definition_attr.source_context=parsertools.GetTokenSourceContext();
			
			return _type_definition_attr;
		}


		case SymbolConstants.SYMBOL_CONTINUE :
		//'CONTINUE'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_DEC_OP :
		//'DEC_OP'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_DECLITERAL :
		//'DecLiteral'
return parsertools.create_int_const(this);

		case SymbolConstants.SYMBOL_DEFAULT :
		//'DEFAULT'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_DIV_ASSIGN :
		//'DIV_ASSIGN'

		{
			op_type_node _op_type_node=new op_type_node(Operators.AssignmentDivision);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_DIVISION :
		//'DIVISION'
return null;

		case SymbolConstants.SYMBOL_DO :
		//'DO'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_DOUBLE :
		//'DOUBLE'

		{
			c_scalar_type _c_scalar_type=new c_scalar_type(c_scalar_type_name.tn_double,c_scalar_sign.none);
			_c_scalar_type.source_context=parsertools.GetTokenSourceContext();
			
			return _c_scalar_type;
		}


		case SymbolConstants.SYMBOL_ELLIPSIS :
		//'ELLIPSIS'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_ELSE :
		//'ELSE'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_ENUM :
		//'ENUM'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_EQ_OP :
		//'EQ_OP'
return null;

		case SymbolConstants.SYMBOL_EQUAL :
		//'EQUAL'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Assignment);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_EXTERN :
		//'EXTERN'

		{
			type_definition_attr _type_definition_attr=new type_definition_attr(definition_attribute.Extern);
			_type_definition_attr.source_context=parsertools.GetTokenSourceContext();
			
			return _type_definition_attr;
		}


		case SymbolConstants.SYMBOL_FLOAT :
		//'FLOAT'

		{
			c_scalar_type _c_scalar_type=new c_scalar_type(c_scalar_type_name.tn_float,c_scalar_sign.none);
			_c_scalar_type.source_context=parsertools.GetTokenSourceContext();
			
			return _c_scalar_type;
		}


		case SymbolConstants.SYMBOL_FLOATLITERAL :
		//'FloatLiteral'
return parsertools.create_double_const(this);

		case SymbolConstants.SYMBOL_FOR :
		//'FOR'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_GE_OP :
		//'GE_OP'
return null;

		case SymbolConstants.SYMBOL_GOTO :
		//'GOTO'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_GREATER :
		//'GREATER'
return null;

		case SymbolConstants.SYMBOL_HEXLITERAL :
		//'HexLiteral'
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'HexLiteral' return NULL! \n\r");*/return null;

		case SymbolConstants.SYMBOL_IDENTIFIER :
		//'IDENTIFIER'
return parsertools.create_ident(this);

		case SymbolConstants.SYMBOL_IF :
		//'IF'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_INC_OP :
		//'INC_OP'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_INLINE :
		//'INLINE'

		{
			type_definition_attr _type_definition_attr=new type_definition_attr(definition_attribute.Inline);
			_type_definition_attr.source_context=parsertools.GetTokenSourceContext();
			
			return _type_definition_attr;
		}


		case SymbolConstants.SYMBOL_INT :
		//'INT'

		{
			c_scalar_type _c_scalar_type=new c_scalar_type(c_scalar_type_name.tn_int,c_scalar_sign.none);
			_c_scalar_type.source_context=parsertools.GetTokenSourceContext();
			
			return _c_scalar_type;
		}


		case SymbolConstants.SYMBOL_LE_OP :
		//'LE_OP'
return null;

		case SymbolConstants.SYMBOL_LEFT_ASSIGN :
		//'LEFT_ASSIGN'

		{
			op_type_node _op_type_node=new op_type_node(Operators.AssignmentBitwiseLeftShift);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_LEFT_OP :
		//'LEFT_OP'
return null;

		case SymbolConstants.SYMBOL_LESS :
		//'LESS'
return null;

		case SymbolConstants.SYMBOL_LONG :
		//'LONG'

		{
			c_scalar_type _c_scalar_type=new c_scalar_type(c_scalar_type_name.tn_long,c_scalar_sign.none);
			_c_scalar_type.source_context=parsertools.GetTokenSourceContext();
			
			return _c_scalar_type;
		}


		case SymbolConstants.SYMBOL_MINUS :
		//'MINUS'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Plus);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_MOD :
		//'MOD'
return null;

		case SymbolConstants.SYMBOL_MOD_ASSIGN :
		//'MOD_ASSIGN'

		{
			op_type_node _op_type_node=new op_type_node(Operators.AssignmentModulus);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_MUL_ASSIGN :
		//'MUL_ASSIGN'

		{
			op_type_node _op_type_node=new op_type_node(Operators.AssignmentMultiplication);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_NE_OP :
		//'NE_OP'
return null;

		case SymbolConstants.SYMBOL_NOT :
		//'NOT'

		{
			op_type_node _op_type_node=new op_type_node(Operators.LogicalNOT);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_OCTLITERAL :
		//'OctLiteral'
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'OctLiteral' return NULL! \n\r");*/return null;

		case SymbolConstants.SYMBOL_OR_ASSIGN :
		//'OR_ASSIGN'

		{
			op_type_node _op_type_node=new op_type_node(Operators.AssignmentBitwiseOR);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_OR_OP :
		//'OR_OP'
return null;

		case SymbolConstants.SYMBOL_PARSEMODEEXPRESSION :
		//'ParseModeExpression'
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'ParseModeExpression' return NULL! \n\r");*/return null;

		case SymbolConstants.SYMBOL_PLUS :
		//'PLUS'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Minus);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_PTR_OP :
		//'PTR_OP'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_REGISTER :
		//'REGISTER'

		{
			type_definition_attr _type_definition_attr=new type_definition_attr(definition_attribute.Register);
			_type_definition_attr.source_context=parsertools.GetTokenSourceContext();
			
			return _type_definition_attr;
		}


		case SymbolConstants.SYMBOL_RETURN :
		//'RETURN'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_RIGHT_ASSIGN :
		//'RIGHT_ASSIGN'

		{
			op_type_node _op_type_node=new op_type_node(Operators.AssignmentBitwiseRightShift);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_RIGHT_OP :
		//'RIGHT_OP'
return null;

		case SymbolConstants.SYMBOL_SEMICOLON :
		//'SEMICOLON'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_SHORT :
		//'SHORT'

		{
			c_scalar_type _c_scalar_type=new c_scalar_type(c_scalar_type_name.tn_short,c_scalar_sign.none);
			_c_scalar_type.source_context=parsertools.GetTokenSourceContext();
			
			return _c_scalar_type;
		}


		case SymbolConstants.SYMBOL_SIGNED :
		//'SIGNED'

		{
			c_scalar_sign_token _c_scalar_sign_token=new c_scalar_sign_token(c_scalar_sign.signed);
			_c_scalar_sign_token.source_context=parsertools.GetTokenSourceContext();
			
			return _c_scalar_sign_token;
		}


		case SymbolConstants.SYMBOL_SIZEOF :
		//'SIZEOF'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_STAR :
		//'STAR'

		{
			op_type_node _op_type_node=new op_type_node(Operators.Multiplication);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_STATIC :
		//'STATIC'

		{
			type_definition_attr _type_definition_attr=new type_definition_attr(definition_attribute.Static);
			_type_definition_attr.source_context=parsertools.GetTokenSourceContext();
			
			return _type_definition_attr;
		}


		case SymbolConstants.SYMBOL_STRING_LITERAL :
		//'STRING_LITERAL'
return parsertools.create_string_const(this);

		case SymbolConstants.SYMBOL_STRUCT :
		//'STRUCT'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_SUB_ASSIGN :
		//'SUB_ASSIGN'

		{
			op_type_node _op_type_node=new op_type_node(Operators.AssignmentSubtraction);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_SWITCH :
		//'SWITCH'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKCOLON :
		//'tkColon'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKCOMMA :
		//'tkComma'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKDOT :
		//'tkDot'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKFIGURECLOSE :
		//'tkFigureClose'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKFIGUREOPEN :
		//'tkFigureOpen'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKQUESTION :
		//'tkQuestion'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKROUNDCLOSE :
		//'tkRoundClose'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKROUNDOPEN :
		//'tkRoundOpen'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKSQUARECLOSE :
		//'tkSquareClose'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKSQUAREOPEN :
		//'tkSquareOpen'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TYPE_NAME :
		//'TYPE_NAME'
return parsertools.create_ident(this);

		case SymbolConstants.SYMBOL_TYPEDEF :
		//'TYPEDEF'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_UNION :
		//'UNION'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_UNSIGNED :
		//'UNSIGNED'

		{
			c_scalar_sign_token _c_scalar_sign_token=new c_scalar_sign_token(c_scalar_sign.unsigned);
			_c_scalar_sign_token.source_context=parsertools.GetTokenSourceContext();
			
			return _c_scalar_sign_token;
		}


		case SymbolConstants.SYMBOL_VOID :
		//'VOID'

		{
			c_scalar_type _c_scalar_type=new c_scalar_type(c_scalar_type_name.tn_void,c_scalar_sign.none);
			_c_scalar_type.source_context=parsertools.GetTokenSourceContext();
			
			return _c_scalar_type;
		}


		case SymbolConstants.SYMBOL_VOLATILE :
		//'VOLATILE'

		{
			type_definition_attr _type_definition_attr=new type_definition_attr(definition_attribute.Volatile);
			_type_definition_attr.source_context=parsertools.GetTokenSourceContext();
			
			return _type_definition_attr;
		}


		case SymbolConstants.SYMBOL_WHILE :
		//'WHILE'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_XOR_ASSIGN :
		//'XOR_ASSIGN'

		{
			op_type_node _op_type_node=new op_type_node(Operators.AssignmentBitwiseXOR);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}


		case SymbolConstants.SYMBOL_ABSTRACT_DECLARATOR :
		//<abstract_declarator>
		//TERMINAL:abstract_declarator
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ADDITIVE_EXPRESSION :
		//<additive_expression>
		//TERMINAL:additive_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_AND_EXPRESSION :
		//<and_expression>
		//TERMINAL:and_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ARGUMENT_EXPRESSION_LIST :
		//<argument_expression_list>
		//TERMINAL:argument_expression_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ASSIGNMENT_EXPRESSION :
		//<assignment_expression>
		//TERMINAL:assignment_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ASSIGNMENT_OPERATOR :
		//<assignment_operator>
		//TERMINAL:assignment_operator
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_CAST_EXPRESSION :
		//<cast_expression>
		//TERMINAL:cast_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_COMPOUND_STATEMENT :
		//<compound_statement>
		//TERMINAL:compound_statement
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_CONDITIONAL_EXPRESSION :
		//<conditional_expression>
		//TERMINAL:conditional_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_CONSTANT_EXPRESSION :
		//<constant_expression>
		//TERMINAL:constant_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_DECLARATION :
		//<declaration>
		//TERMINAL:declaration
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_DECLARATION_LIST :
		//<declaration_list>
		//TERMINAL:declaration_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_DECLARATION_SPECIFIERS :
		//<declaration_specifiers>
		//TERMINAL:declaration_specifiers
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_DECLARATOR :
		//<declarator>
		//TERMINAL:declarator
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_DECLARATOR_LIST :
		//<declarator_list>
		//TERMINAL:declarator_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_DIRECT_ABSTRACT_DECLARATOR :
		//<direct_abstract_declarator>
		//TERMINAL:direct_abstract_declarator
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_DIRECT_DECLARATOR :
		//<direct_declarator>
		//TERMINAL:direct_declarator
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_DIRECT_DECLARATOR_IDENT :
		//<direct_declarator_ident>
		//TERMINAL:direct_declarator_ident
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_EMPTY :
		//<empty>
		//TERMINAL:empty
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ENUM_SPECIFIER :
		//<enum_specifier>
		//TERMINAL:enum_specifier
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ENUMERATOR :
		//<enumerator>
		//TERMINAL:enumerator
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ENUMERATOR_LIST :
		//<enumerator_list>
		//TERMINAL:enumerator_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_EQUALITY_EXPRESSION :
		//<equality_expression>
		//TERMINAL:equality_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_EXCLUSIVE_OR_EXPRESSION :
		//<exclusive_or_expression>
		//TERMINAL:exclusive_or_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_EXPRESSION :
		//<expression>
		//TERMINAL:expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_EXPRESSION_STATEMENT :
		//<expression_statement>
		//TERMINAL:expression_statement
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_EXPRESSION_STATEMENT_OR_DECLARATION :
		//<expression_statement_or_declaration>
		//TERMINAL:expression_statement_or_declaration
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_EXTERNAL_DECLARATION :
		//<external_declaration>
		//TERMINAL:external_declaration
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_EXTERNAL_DECLARATION_LIST :
		//<external_declaration_list>
		//TERMINAL:external_declaration_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_FUNCTION_DEFINITION :
		//<function_definition>
		//TERMINAL:function_definition
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_FUNCTION_DEFINITION_COMPOUND_STATEMENT :
		//<function_definition_compound_statement>
		//TERMINAL:function_definition_compound_statement
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_FUNCTION_DEFINITION_HEADER :
		//<function_definition_header>
		//TERMINAL:function_definition_header
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_IDENTIFIER_LIST :
		//<identifier_list>
		//TERMINAL:identifier_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_INCLUSIVE_OR_EXPRESSION :
		//<inclusive_or_expression>
		//TERMINAL:inclusive_or_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_INIT_DECLARATOR :
		//<init_declarator>
		//TERMINAL:init_declarator
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_INIT_DECLARATOR_LIST :
		//<init_declarator_list>
		//TERMINAL:init_declarator_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_INITIALIZER :
		//<initializer>
		//TERMINAL:initializer
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_INITIALIZER_LIST :
		//<initializer_list>
		//TERMINAL:initializer_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ITERATION_STATEMENT :
		//<iteration_statement>
		//TERMINAL:iteration_statement
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_JUMP_STATEMENT :
		//<jump_statement>
		//TERMINAL:jump_statement
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_LABELED_STATEMENT :
		//<labeled_statement>
		//TERMINAL:labeled_statement
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_LOGICAL_AND_EXPRESSION :
		//<logical_and_expression>
		//TERMINAL:logical_and_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_LOGICAL_OR_EXPRESSION :
		//<logical_or_expression>
		//TERMINAL:logical_or_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_MULTIPLICATIVE_EXPRESSION :
		//<multiplicative_expression>
		//TERMINAL:multiplicative_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ONTHER_SPECIFIER :
		//<onther_specifier>
		//TERMINAL:onther_specifier
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_OPT_ENUMERATOR_LIST :
		//<opt_enumerator_list>
		//TERMINAL:opt_enumerator_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_PARAMETER_DECLARATION :
		//<parameter_declaration>
		//TERMINAL:parameter_declaration
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_PARAMETER_LIST :
		//<parameter_list>
		//TERMINAL:parameter_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_PARAMETER_TYPE_LIST :
		//<parameter_type_list>
		//TERMINAL:parameter_type_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_PARSE_MODE_EXPRESSION :
		//<parse_mode_expression>
		//TERMINAL:parse_mode_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_POINTER :
		//<pointer>
		//TERMINAL:pointer
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_POSTFIX_EXPRESSION :
		//<postfix_expression>
		//TERMINAL:postfix_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_PRIMARY_EXPRESSION :
		//<primary_expression>
		//TERMINAL:primary_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_RELATIONAL_EXPRESSION :
		//<relational_expression>
		//TERMINAL:relational_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_SELECTION_STATEMENT :
		//<selection_statement>
		//TERMINAL:selection_statement
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_SHIFT_EXPRESSION :
		//<shift_expression>
		//TERMINAL:shift_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_SPECIFIER_QUALIFIER_LIST :
		//<specifier_qualifier_list>
		//TERMINAL:specifier_qualifier_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_START_RULE :
		//<start_rule>
		//TERMINAL:start_rule
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_STATEMENT :
		//<statement>
		//TERMINAL:statement
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_STATEMENT_LIST :
		//<statement_list>
		//TERMINAL:statement_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_STORAGE_CLASS_SPECIFIER :
		//<storage_class_specifier>
		//TERMINAL:storage_class_specifier
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_STRUCT_DECLARATION :
		//<struct_declaration>
		//TERMINAL:struct_declaration
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_STRUCT_DECLARATION_LIST :
		//<struct_declaration_list>
		//TERMINAL:struct_declaration_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_STRUCT_DECLARATOR :
		//<struct_declarator>
		//TERMINAL:struct_declarator
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_STRUCT_DECLARATOR_LIST :
		//<struct_declarator_list>
		//TERMINAL:struct_declarator_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_STRUCT_OR_UNION :
		//<struct_or_union>
		//TERMINAL:struct_or_union
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_STRUCT_OR_UNION_SPECIFIER :
		//<struct_or_union_specifier>
		//TERMINAL:struct_or_union_specifier
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_TRANSLATION_UNIT :
		//<translation_unit>
		//TERMINAL:translation_unit
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_TYPE_NAME2 :
		//<type_name>
		//TERMINAL:type_name
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_TYPE_QUALIFIER :
		//<type_qualifier>
		//TERMINAL:type_qualifier
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_TYPE_QUALIFIER_LIST :
		//<type_qualifier_list>
		//TERMINAL:type_qualifier_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_TYPE_SPECIFIER :
		//<type_specifier>
		//TERMINAL:type_specifier
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_TYPE_SPECIFIERS :
		//<type_specifiers>
		//TERMINAL:type_specifiers
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_UNARY_EXPRESSION :
		//<unary_expression>
		//TERMINAL:unary_expression
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_UNARY_OPERATOR :
		//<unary_operator>
		//TERMINAL:unary_operator
		return null;
		//ENDTERMINAL

            }
            throw new SymbolException("Unknown symbol");
        }

	

        public Object CreateNonTerminalObject()
        {
            switch ((RuleConstants) LRParser.ReductionRule.Index)
            {
		case RuleConstants.RULE_START_RULE :
		//<start_rule> ::= <translation_unit>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_START_RULE2 :
		//<start_rule> ::= <parse_mode_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_PARSE_MODE_EXPRESSION_PARSEMODEEXPRESSION :
		//<parse_mode_expression> ::= 'ParseModeExpression' <expression>
return LRParser.GetReductionSyntaxNode(1);

		case RuleConstants.RULE_PRIMARY_EXPRESSION_IDENTIFIER :
		//<primary_expression> ::= 'IDENTIFIER'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_PRIMARY_EXPRESSION_DECLITERAL :
		//<primary_expression> ::= 'DecLiteral'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_PRIMARY_EXPRESSION_OCTLITERAL :
		//<primary_expression> ::= 'OctLiteral'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_PRIMARY_EXPRESSION_HEXLITERAL :
		//<primary_expression> ::= 'HexLiteral'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_PRIMARY_EXPRESSION_FLOATLITERAL :
		//<primary_expression> ::= 'FloatLiteral'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_PRIMARY_EXPRESSION_CHARLITERAL :
		//<primary_expression> ::= 'CharLiteral'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_PRIMARY_EXPRESSION_STRING_LITERAL :
		//<primary_expression> ::= 'STRING_LITERAL'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_PRIMARY_EXPRESSION_TKROUNDOPEN_TKROUNDCLOSE :
		//<primary_expression> ::= 'tkRoundOpen' <expression> 'tkRoundClose'
 return LRParser.GetReductionSyntaxNode(1); 

		case RuleConstants.RULE_POSTFIX_EXPRESSION :
		//<postfix_expression> ::= <primary_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_POSTFIX_EXPRESSION_TKSQUAREOPEN_TKSQUARECLOSE :
		//<postfix_expression> ::= <postfix_expression> 'tkSquareOpen' <expression> 'tkSquareClose'
         
		{
			indexer _indexer=new indexer();
			 
								_indexer.indexes=new expression_list();
								_indexer.indexes.expressions.Add((expression)LRParser.GetReductionSyntaxNode(2));
								_indexer.dereferencing_value=(addressed_value)LRParser.GetReductionSyntaxNode(0);
								parsertools.assign_source_context(_indexer.indexes,LRParser.GetReductionSyntaxNode(2));
								parsertools.create_source_context(_indexer,LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(3));
			return _indexer;
		}


		case RuleConstants.RULE_POSTFIX_EXPRESSION_TKROUNDOPEN_TKROUNDCLOSE :
		//<postfix_expression> ::= <postfix_expression> 'tkRoundOpen' 'tkRoundClose'
         
		{
			method_call _method_call=new method_call(null);
			
								_method_call.dereferencing_value=(addressed_value)LRParser.GetReductionSyntaxNode(0);
								parsertools.create_source_context(_method_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _method_call;
		}


		case RuleConstants.RULE_POSTFIX_EXPRESSION_TKROUNDOPEN_TKROUNDCLOSE2 :
		//<postfix_expression> ::= <postfix_expression> 'tkRoundOpen' <argument_expression_list> 'tkRoundClose'
         
		{
			method_call _method_call=new method_call((expression_list)LRParser.GetReductionSyntaxNode(2));
			
								_method_call.dereferencing_value=(addressed_value)LRParser.GetReductionSyntaxNode(0);
								parsertools.create_source_context(_method_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _method_call;
		}


		case RuleConstants.RULE_POSTFIX_EXPRESSION_TKDOT_IDENTIFIER :
		//<postfix_expression> ::= <postfix_expression> 'tkDot' 'IDENTIFIER'
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.Member);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_POSTFIX_EXPRESSION_PTR_OP_IDENTIFIER :
		//<postfix_expression> ::= <postfix_expression> 'PTR_OP' 'IDENTIFIER'
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.MemberByPointer);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_POSTFIX_EXPRESSION_INC_OP :
		//<postfix_expression> ::= <postfix_expression> 'INC_OP'
         
		{
			un_expr _un_expr=new un_expr((expression)LRParser.GetReductionSyntaxNode(0),Operators.PostfixIncrement);
			 parsertools.create_source_context(_un_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _un_expr;
		}


		case RuleConstants.RULE_POSTFIX_EXPRESSION_DEC_OP :
		//<postfix_expression> ::= <postfix_expression> 'DEC_OP'
         
		{
			un_expr _un_expr=new un_expr((expression)LRParser.GetReductionSyntaxNode(0),Operators.PostfixDecrement);
			 parsertools.create_source_context(_un_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _un_expr;
		}


		case RuleConstants.RULE_ARGUMENT_EXPRESSION_LIST :
		//<argument_expression_list> ::= <assignment_expression>
         
		//TemplateList for expression_list (create)
		{
			expression_list _expression_list=new expression_list();
			_expression_list.source_context=((expression)LRParser.GetReductionSyntaxNode(0)).source_context;
			_expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
			return _expression_list;
		}


		case RuleConstants.RULE_ARGUMENT_EXPRESSION_LIST_TKCOMMA :
		//<argument_expression_list> ::= <argument_expression_list> 'tkComma' <assignment_expression>

		//TemplateList for expression_list (add)         
		{
			expression_list _expression_list=(expression_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_expression_list,_expression_list,LRParser.GetReductionSyntaxNode(2));
			_expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(2) as expression);
			return _expression_list;
		}


		case RuleConstants.RULE_UNARY_EXPRESSION :
		//<unary_expression> ::= <postfix_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_UNARY_EXPRESSION_INC_OP :
		//<unary_expression> ::= 'INC_OP' <unary_expression>
         
		{
			un_expr _un_expr=new un_expr((expression)LRParser.GetReductionSyntaxNode(1),Operators.PrefixIncrement);
			 parsertools.create_source_context(_un_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _un_expr;
		}


		case RuleConstants.RULE_UNARY_EXPRESSION_DEC_OP :
		//<unary_expression> ::= 'DEC_OP' <unary_expression>
         
		{
			un_expr _un_expr=new un_expr((expression)LRParser.GetReductionSyntaxNode(1),Operators.PrefixDecrement);
			 parsertools.create_source_context(_un_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _un_expr;
		}


		case RuleConstants.RULE_UNARY_EXPRESSION2 :
		//<unary_expression> ::= <unary_operator> <cast_expression>
         
		{
			un_expr _un_expr=new un_expr((expression)LRParser.GetReductionSyntaxNode(1),((op_type_node)LRParser.GetReductionSyntaxNode(0)).type);
			parsertools.create_source_context(_un_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			
			return _un_expr;
		}


		case RuleConstants.RULE_UNARY_EXPRESSION_SIZEOF :
		//<unary_expression> ::= 'SIZEOF' <unary_expression>
         
		{
			sizeof_operator _sizeof_operator=new sizeof_operator(null,(expression)LRParser.GetReductionSyntaxNode(1));
			 parsertools.create_source_context(_sizeof_operator,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _sizeof_operator;
		}


		case RuleConstants.RULE_UNARY_EXPRESSION_SIZEOF_TKROUNDOPEN_TKROUNDCLOSE :
		//<unary_expression> ::= 'SIZEOF' 'tkRoundOpen' <type_name> 'tkRoundClose'
         
		{
			sizeof_operator _sizeof_operator=new sizeof_operator((type_definition)LRParser.GetReductionSyntaxNode(2),null);
			 parsertools.create_source_context(_sizeof_operator,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _sizeof_operator;
		}


		case RuleConstants.RULE_UNARY_OPERATOR_B_AND_OP :
		//<unary_operator> ::= 'B_AND_OP' <empty>
 ((op_type_node)LRParser.GetReductionSyntaxNode(0)).type=Operators.AddressOf; return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_UNARY_OPERATOR_STAR :
		//<unary_operator> ::= 'STAR' <empty>
 ((op_type_node)LRParser.GetReductionSyntaxNode(0)).type=Operators.Dereference; return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_UNARY_OPERATOR_PLUS :
		//<unary_operator> ::= 'PLUS'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_UNARY_OPERATOR_MINUS :
		//<unary_operator> ::= 'MINUS'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_UNARY_OPERATOR_BOC :
		//<unary_operator> ::= 'BOC'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_UNARY_OPERATOR_NOT :
		//<unary_operator> ::= 'NOT'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_CAST_EXPRESSION :
		//<cast_expression> ::= <unary_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_CAST_EXPRESSION_TKROUNDOPEN_TKROUNDCLOSE :
		//<cast_expression> ::= 'tkRoundOpen' <type_name> 'tkRoundClose' <cast_expression>
         
		{
			typecast_node _typecast_node=new typecast_node((addressed_value)LRParser.GetReductionSyntaxNode(3),(type_definition)LRParser.GetReductionSyntaxNode(1),op_typecast.typecast);
			 parsertools.create_source_context(_typecast_node,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _typecast_node;
		}


		case RuleConstants.RULE_MULTIPLICATIVE_EXPRESSION :
		//<multiplicative_expression> ::= <cast_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_MULTIPLICATIVE_EXPRESSION_STAR :
		//<multiplicative_expression> ::= <multiplicative_expression> 'STAR' <cast_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.Multiplication);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_MULTIPLICATIVE_EXPRESSION_DIVISION :
		//<multiplicative_expression> ::= <multiplicative_expression> 'DIVISION' <cast_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.Division);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_MULTIPLICATIVE_EXPRESSION_MOD :
		//<multiplicative_expression> ::= <multiplicative_expression> 'MOD' <cast_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.ModulusRemainder);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_ADDITIVE_EXPRESSION :
		//<additive_expression> ::= <multiplicative_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ADDITIVE_EXPRESSION_PLUS :
		//<additive_expression> ::= <additive_expression> 'PLUS' <multiplicative_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.Plus);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_ADDITIVE_EXPRESSION_MINUS :
		//<additive_expression> ::= <additive_expression> 'MINUS' <multiplicative_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.Minus);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_SHIFT_EXPRESSION :
		//<shift_expression> ::= <additive_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_SHIFT_EXPRESSION_LEFT_OP :
		//<shift_expression> ::= <shift_expression> 'LEFT_OP' <additive_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.BitwiseLeftShift);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_SHIFT_EXPRESSION_RIGHT_OP :
		//<shift_expression> ::= <shift_expression> 'RIGHT_OP' <additive_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.BitwiseRightShift);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_RELATIONAL_EXPRESSION :
		//<relational_expression> ::= <shift_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_RELATIONAL_EXPRESSION_LESS :
		//<relational_expression> ::= <relational_expression> 'LESS' <shift_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.Less);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_RELATIONAL_EXPRESSION_GREATER :
		//<relational_expression> ::= <relational_expression> 'GREATER' <shift_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.Greater);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_RELATIONAL_EXPRESSION_LE_OP :
		//<relational_expression> ::= <relational_expression> 'LE_OP' <shift_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.LessEqual);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_RELATIONAL_EXPRESSION_GE_OP :
		//<relational_expression> ::= <relational_expression> 'GE_OP' <shift_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.GreaterEqual);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_EQUALITY_EXPRESSION :
		//<equality_expression> ::= <relational_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_EQUALITY_EXPRESSION_EQ_OP :
		//<equality_expression> ::= <equality_expression> 'EQ_OP' <relational_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.Equal);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_EQUALITY_EXPRESSION_NE_OP :
		//<equality_expression> ::= <equality_expression> 'NE_OP' <relational_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.NotEqual);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_AND_EXPRESSION :
		//<and_expression> ::= <equality_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_AND_EXPRESSION_B_AND_OP :
		//<and_expression> ::= <and_expression> 'B_AND_OP' <equality_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_EXCLUSIVE_OR_EXPRESSION :
		//<exclusive_or_expression> ::= <and_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_EXCLUSIVE_OR_EXPRESSION_B_E_OR_OP :
		//<exclusive_or_expression> ::= <exclusive_or_expression> 'B_E_OR_OP' <and_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.BitwiseXOR);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_INCLUSIVE_OR_EXPRESSION :
		//<inclusive_or_expression> ::= <exclusive_or_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_INCLUSIVE_OR_EXPRESSION_B_I_OR_OP :
		//<inclusive_or_expression> ::= <inclusive_or_expression> 'B_I_OR_OP' <exclusive_or_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.BitwiseOR);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_LOGICAL_AND_EXPRESSION :
		//<logical_and_expression> ::= <inclusive_or_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_LOGICAL_AND_EXPRESSION_AND_OP :
		//<logical_and_expression> ::= <logical_and_expression> 'AND_OP' <inclusive_or_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.LogicalAND);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_LOGICAL_OR_EXPRESSION :
		//<logical_or_expression> ::= <logical_and_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_LOGICAL_OR_EXPRESSION_OR_OP :
		//<logical_or_expression> ::= <logical_or_expression> 'OR_OP' <logical_and_expression>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),Operators.LogicalOR);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}


		case RuleConstants.RULE_CONDITIONAL_EXPRESSION :
		//<conditional_expression> ::= <logical_or_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_CONDITIONAL_EXPRESSION_TKQUESTION_TKCOLON :
		//<conditional_expression> ::= <logical_or_expression> 'tkQuestion' <expression> 'tkColon' <conditional_expression>
         
		{
			question_colon_expression _question_colon_expression=new question_colon_expression((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),(expression)LRParser.GetReductionSyntaxNode(4));
			 parsertools.create_source_context(_question_colon_expression,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
			return _question_colon_expression;
		}


		case RuleConstants.RULE_ASSIGNMENT_EXPRESSION :
		//<assignment_expression> ::= <conditional_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ASSIGNMENT_EXPRESSION2 :
		//<assignment_expression> ::= <unary_expression> <assignment_operator> <assignment_expression>
         
		{
			assign _assign=new assign(LRParser.GetReductionSyntaxNode(0) as addressed_value,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			 parsertools.create_source_context(_assign,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _assign;
		}


		case RuleConstants.RULE_ASSIGNMENT_OPERATOR_EQUAL :
		//<assignment_operator> ::= 'EQUAL'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ASSIGNMENT_OPERATOR_MUL_ASSIGN :
		//<assignment_operator> ::= 'MUL_ASSIGN'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ASSIGNMENT_OPERATOR_DIV_ASSIGN :
		//<assignment_operator> ::= 'DIV_ASSIGN'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ASSIGNMENT_OPERATOR_MOD_ASSIGN :
		//<assignment_operator> ::= 'MOD_ASSIGN'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ASSIGNMENT_OPERATOR_ADD_ASSIGN :
		//<assignment_operator> ::= 'ADD_ASSIGN'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ASSIGNMENT_OPERATOR_SUB_ASSIGN :
		//<assignment_operator> ::= 'SUB_ASSIGN'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ASSIGNMENT_OPERATOR_LEFT_ASSIGN :
		//<assignment_operator> ::= 'LEFT_ASSIGN'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ASSIGNMENT_OPERATOR_RIGHT_ASSIGN :
		//<assignment_operator> ::= 'RIGHT_ASSIGN'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ASSIGNMENT_OPERATOR_AND_ASSIGN :
		//<assignment_operator> ::= 'AND_ASSIGN'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ASSIGNMENT_OPERATOR_XOR_ASSIGN :
		//<assignment_operator> ::= 'XOR_ASSIGN'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ASSIGNMENT_OPERATOR_OR_ASSIGN :
		//<assignment_operator> ::= 'OR_ASSIGN'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_EXPRESSION :
		//<expression> ::= <assignment_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_EXPRESSION_TKCOMMA :
		//<expression> ::= <expression> 'tkComma' <assignment_expression>
 
						expression_list el=LRParser.GetReductionSyntaxNode(0) as expression_list;
						if(el==null)
						{
							el=new expression_list();
							el.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
						}
						el.expressions.Add((expression)LRParser.GetReductionSyntaxNode(2));
						parsertools.create_source_context(el,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
						return el;
						

		case RuleConstants.RULE_CONSTANT_EXPRESSION :
		//<constant_expression> ::= <conditional_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_DECLARATION_SEMICOLON :
		//<declaration> ::= <declaration_specifiers> 'SEMICOLON'
{ 									   
						declarations sds=new declarations();
						sds.defs.Add((declaration)LRParser.GetReductionSyntaxNode(0));
						declarations_as_statement vdss = new declarations_as_statement(sds);
						parsertools.create_source_context(vdss,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
						parsertools.assign_source_context(vdss.defs,vdss);						
						if((LRParser.GetReductionSyntaxNode(0) is enum_type_definition) || (LRParser.GetReductionSyntaxNode(0) is type_declaration))
						{
						}
						else
						{
							errors.Add(new InitDeclaratorListExpected(current_file_name,(LRParser.GetReductionSyntaxNode(1) as syntax_tree_node).source_context,sds)); 
						}
						return vdss;
						}

		case RuleConstants.RULE_DECLARATION_SEMICOLON2 :
		//<declaration> ::= <declaration_specifiers> <init_declarator_list> 'SEMICOLON'
{
			declarations_as_statement vdss = new declarations_as_statement((declarations)LRParser.GetReductionSyntaxNode(1));
                        Converter.PrepareVariableDefinitions((declaration)LRParser.GetReductionSyntaxNode(0),vdss.defs);
			parsertools.create_source_context(vdss,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			parsertools.assign_source_context(vdss.defs,vdss);
			return vdss;
			}

		case RuleConstants.RULE_DECLARATION_TYPEDEF_SEMICOLON :
		//<declaration> ::= 'TYPEDEF' <type_specifiers> <declarator_list> 'SEMICOLON'
         
		{
			type_declarations _type_declarations=(type_declarations)LRParser.GetReductionSyntaxNode(2);
			foreach(type_declaration td in _type_declarations.types_decl)
			{
				LRParser.SepecialSymbolPrefixDirection.Add(td.type_name.name,'$'); 
				if(td.type_def==null)
					td.type_def=(type_definition)LRParser.GetReductionSyntaxNode(1);
				else
				{
					ref_type rt=(ref_type)td.type_def;
					while(rt.pointed_to!=null)
						rt=(ref_type)rt.pointed_to;
					rt.pointed_to=(type_definition)LRParser.GetReductionSyntaxNode(1);
				}
			}
			parsertools.create_source_context(_type_declarations,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _type_declarations;
		}


		case RuleConstants.RULE_DECLARATOR_LIST :
		//<declarator_list> ::= <declarator>
         
		{
			type_declarations _type_declarations=new type_declarations();
			 
						 parsertools.assign_source_context(_type_declarations,LRParser.GetReductionSyntaxNode(0));
						 Converter.PrepareDeclaratorList(_type_declarations,(var_def_statement)LRParser.GetReductionSyntaxNode(0));
			return _type_declarations;
		}


		case RuleConstants.RULE_DECLARATOR_LIST_TKCOMMA :
		//<declarator_list> ::= <declarator_list> 'tkComma' <declarator>
         
		{
			type_declarations _type_declarations=(type_declarations)LRParser.GetReductionSyntaxNode(0);
						 Converter.PrepareDeclaratorList(_type_declarations,(var_def_statement)LRParser.GetReductionSyntaxNode(2));
			return _type_declarations;
		}


		case RuleConstants.RULE_INIT_DECLARATOR_LIST :
		//<init_declarator_list> ::= <init_declarator>
         
		{
			declarations _declarations=new declarations();
			
								_declarations.defs.Add(Converter.PrepareInitDeclarator(LRParser.GetReductionSyntaxNode(0)));
								parsertools.create_source_context(_declarations,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _declarations;
		}


		case RuleConstants.RULE_INIT_DECLARATOR_LIST_TKCOMMA :
		//<init_declarator_list> ::= <init_declarator_list> 'tkComma' <init_declarator>
         
		{
			declarations _declarations=(declarations)LRParser.GetReductionSyntaxNode(0);
								_declarations.defs.Add(Converter.PrepareInitDeclarator(LRParser.GetReductionSyntaxNode(2)));
								parsertools.create_source_context(_declarations,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _declarations;
		}


		case RuleConstants.RULE_INIT_DECLARATOR :
		//<init_declarator> ::= <declarator>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_INIT_DECLARATOR_EQUAL :
		//<init_declarator> ::= <declarator> 'EQUAL' <initializer>
         
		{
			var_def_statement _var_def_statement=(var_def_statement)LRParser.GetReductionSyntaxNode(0); 
								if(_var_def_statement is ref_type_and_var_def_statement)
									((ref_type_and_var_def_statement)_var_def_statement).var_def_statement.inital_value=(expression)LRParser.GetReductionSyntaxNode(2);
								else
									_var_def_statement.inital_value=(expression)LRParser.GetReductionSyntaxNode(2);
								parsertools.create_source_context(_var_def_statement,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _var_def_statement;
		}


		case RuleConstants.RULE_DECLARATION_SPECIFIERS :
		//<declaration_specifiers> ::= <type_specifiers>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_DECLARATION_SPECIFIERS2 :
		//<declaration_specifiers> ::= <storage_class_specifier>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_DECLARATION_SPECIFIERS3 :
		//<declaration_specifiers> ::= <storage_class_specifier> <type_specifiers>
return Converter.PrepareTypeSpecifiers(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));

		case RuleConstants.RULE_STORAGE_CLASS_SPECIFIER_REGISTER :
		//<storage_class_specifier> ::= 'REGISTER'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_STORAGE_CLASS_SPECIFIER_EXTERN :
		//<storage_class_specifier> ::= 'EXTERN'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_STORAGE_CLASS_SPECIFIER_STATIC :
		//<storage_class_specifier> ::= 'STATIC'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_STORAGE_CLASS_SPECIFIER_AUTO :
		//<storage_class_specifier> ::= 'AUTO'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_STORAGE_CLASS_SPECIFIER :
		//<storage_class_specifier> ::= <onther_specifier>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ONTHER_SPECIFIER_INLINE :
		//<onther_specifier> ::= 'INLINE'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_SPECIFIERS :
		//<type_specifiers> ::= <type_specifier>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_SPECIFIERS2 :
		//<type_specifiers> ::= <type_specifiers> <type_specifier>
return Converter.PrepareTypeSpecifiers(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));

		case RuleConstants.RULE_TYPE_SPECIFIERS3 :
		//<type_specifiers> ::= <type_qualifier>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_QUALIFIER_CONST :
		//<type_qualifier> ::= 'CONST'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_QUALIFIER_VOLATILE :
		//<type_qualifier> ::= 'VOLATILE'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_SPECIFIER_VOID :
		//<type_specifier> ::= 'VOID'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_SPECIFIER_CHAR :
		//<type_specifier> ::= 'CHAR'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_SPECIFIER_SHORT :
		//<type_specifier> ::= 'SHORT'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_SPECIFIER_INT :
		//<type_specifier> ::= 'INT'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_SPECIFIER_LONG :
		//<type_specifier> ::= 'LONG'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_SPECIFIER_FLOAT :
		//<type_specifier> ::= 'FLOAT'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_SPECIFIER_DOUBLE :
		//<type_specifier> ::= 'DOUBLE'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_SPECIFIER_UNSIGNED :
		//<type_specifier> ::= 'UNSIGNED'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_SPECIFIER_SIGNED :
		//<type_specifier> ::= 'SIGNED'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_SPECIFIER :
		//<type_specifier> ::= <struct_or_union_specifier>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_SPECIFIER2 :
		//<type_specifier> ::= <enum_specifier>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_SPECIFIER_TYPE_NAME :
		//<type_specifier> ::= 'TYPE_NAME' <empty>
         
		{
			named_type_reference _named_type_reference=new named_type_reference();
			 _named_type_reference.names.Add((ident)LRParser.GetReductionSyntaxNode(0)); parsertools.assign_source_context(_named_type_reference,LRParser.GetReductionSyntaxNode(0));
			return _named_type_reference;
		}


		case RuleConstants.RULE_STRUCT_OR_UNION_SPECIFIER_IDENTIFIER_TKFIGUREOPEN_TKFIGURECLOSE :
		//<struct_or_union_specifier> ::= <struct_or_union> 'IDENTIFIER' 'tkFigureOpen' <struct_declaration_list> 'tkFigureClose'
 
						return Converter.PrepareUnionOrStructDefinition((token_info)LRParser.GetReductionSyntaxNode(0),(ident)LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(2),(class_members)LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(4));

		case RuleConstants.RULE_STRUCT_OR_UNION_SPECIFIER_TKFIGUREOPEN_TKFIGURECLOSE :
		//<struct_or_union_specifier> ::= <struct_or_union> 'tkFigureOpen' <struct_declaration_list> 'tkFigureClose'
 
						return Converter.PrepareUnionOrStructDefinition((token_info)LRParser.GetReductionSyntaxNode(0),null,LRParser.GetReductionSyntaxNode(1),(class_members)LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(3));

		case RuleConstants.RULE_STRUCT_OR_UNION_SPECIFIER_IDENTIFIER :
		//<struct_or_union_specifier> ::= <struct_or_union> 'IDENTIFIER'
 
						return Converter.PrepareUnionOrStructDefinition((token_info)LRParser.GetReductionSyntaxNode(0),(ident)LRParser.GetReductionSyntaxNode(1),null,null,null);

		case RuleConstants.RULE_STRUCT_OR_UNION_STRUCT :
		//<struct_or_union> ::= 'STRUCT'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_STRUCT_OR_UNION_UNION :
		//<struct_or_union> ::= 'UNION'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_STRUCT_DECLARATION_LIST :
		//<struct_declaration_list> ::= <struct_declaration>
         
		{
			class_members _class_members=new class_members();
			 
								Converter.AddToSubporgramDefs(_class_members.members,LRParser.GetReductionSyntaxNode(0));
								parsertools.assign_source_context(_class_members,LRParser.GetReductionSyntaxNode(0));
			return _class_members;
		}


		case RuleConstants.RULE_STRUCT_DECLARATION_LIST2 :
		//<struct_declaration_list> ::= <struct_declaration_list> <struct_declaration>
         
		{
			class_members _class_members=(class_members)LRParser.GetReductionSyntaxNode(0); 
								Converter.AddToSubporgramDefs(_class_members.members,LRParser.GetReductionSyntaxNode(1));
								parsertools.create_source_context(_class_members,_class_members,LRParser.GetReductionSyntaxNode(1));
			return _class_members;
		}


		case RuleConstants.RULE_STRUCT_DECLARATION_SEMICOLON :
		//<struct_declaration> ::= <specifier_qualifier_list> <struct_declarator_list> 'SEMICOLON'
{
			declarations defs=(declarations)LRParser.GetReductionSyntaxNode(1);
                        Converter.PrepareVariableDefinitions(LRParser.GetReductionSyntaxNode(0) as type_definition,defs);
			return defs;
			}

		case RuleConstants.RULE_SPECIFIER_QUALIFIER_LIST :
		//<specifier_qualifier_list> ::= <type_specifiers>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_STRUCT_DECLARATOR_LIST :
		//<struct_declarator_list> ::= <struct_declarator>
         
		{
			declarations _declarations=new declarations();
			
								_declarations.defs.Add(Converter.PrepareInitDeclarator(LRParser.GetReductionSyntaxNode(0)));
								parsertools.create_source_context(_declarations,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _declarations;
		}


		case RuleConstants.RULE_STRUCT_DECLARATOR_LIST_TKCOMMA :
		//<struct_declarator_list> ::= <struct_declarator_list> 'tkComma' <struct_declarator>
         
		{
			declarations _declarations=(declarations)LRParser.GetReductionSyntaxNode(0);
								_declarations.defs.Add(Converter.PrepareInitDeclarator(LRParser.GetReductionSyntaxNode(2)));
								parsertools.create_source_context(_declarations,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _declarations;
		}


		case RuleConstants.RULE_STRUCT_DECLARATOR :
		//<struct_declarator> ::= <declarator>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_STRUCT_DECLARATOR_TKCOLON :
		//<struct_declarator> ::= 'tkColon' <constant_expression>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<struct_declarator> ::= tkColon <constant_expression>"));}return null;

		case RuleConstants.RULE_STRUCT_DECLARATOR_TKCOLON2 :
		//<struct_declarator> ::= <declarator> 'tkColon' <constant_expression>
         
		{
			var_def_statement _var_def_statement=(var_def_statement)LRParser.GetReductionSyntaxNode(0); 
								if(_var_def_statement is ref_type_and_var_def_statement)
									((ref_type_and_var_def_statement)_var_def_statement).var_def_statement.inital_value=(expression)LRParser.GetReductionSyntaxNode(2);
								else
									_var_def_statement.inital_value=(expression)LRParser.GetReductionSyntaxNode(2);
								parsertools.create_source_context(_var_def_statement,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _var_def_statement;
		}


		case RuleConstants.RULE_ENUM_SPECIFIER_ENUM_TKFIGUREOPEN_TKFIGURECLOSE :
		//<enum_specifier> ::= 'ENUM' 'tkFigureOpen' <opt_enumerator_list> 'tkFigureClose'
         
		{
			enum_type_definition _enum_type_definition=new enum_type_definition((enumerator_list)LRParser.GetReductionSyntaxNode(2));
			 
								parsertools.create_source_context(_enum_type_definition,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _enum_type_definition;
		}


		case RuleConstants.RULE_ENUM_SPECIFIER_ENUM_IDENTIFIER_TKFIGUREOPEN_TKFIGURECLOSE :
		//<enum_specifier> ::= 'ENUM' 'IDENTIFIER' 'tkFigureOpen' <opt_enumerator_list> 'tkFigureClose'
         
		{
			enum_type_definition _enum_type_definition=new enum_type_definition((enumerator_list)LRParser.GetReductionSyntaxNode(3));
			 
								LRParser.SepecialSymbolPrefixDirection.Add((LRParser.GetReductionSyntaxNode(1) as ident).name,'$'); 
								parsertools.create_source_context(_enum_type_definition,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
								type_declaration td=new type_declaration((ident)LRParser.GetReductionSyntaxNode(1),_enum_type_definition);
								parsertools.create_source_context(td,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
								return td;
								
			return _enum_type_definition;
		}


		case RuleConstants.RULE_ENUM_SPECIFIER_ENUM_IDENTIFIER :
		//<enum_specifier> ::= 'ENUM' 'IDENTIFIER'
         
		{
			enum_type_definition _enum_type_definition=new enum_type_definition(null);
			 
								LRParser.SepecialSymbolPrefixDirection.Add((LRParser.GetReductionSyntaxNode(1) as ident).name,'$'); 
								parsertools.create_source_context(_enum_type_definition,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
								type_declaration td=new type_declaration((ident)LRParser.GetReductionSyntaxNode(1),_enum_type_definition);
								parsertools.create_source_context(td,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
								return td;
								
			return _enum_type_definition;
		}


		case RuleConstants.RULE_OPT_ENUMERATOR_LIST :
		//<opt_enumerator_list> ::= <enumerator_list>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_OPT_ENUMERATOR_LIST2 :
		//<opt_enumerator_list> ::= 
		//NONTERMINAL:<opt_enumerator_list> ::= 
		return null;
		//ENDNONTERMINAL

		case RuleConstants.RULE_ENUMERATOR_LIST :
		//<enumerator_list> ::= <enumerator>
         
		//TemplateList for enumerator_list (create)
		{
			enumerator_list _enumerator_list=new enumerator_list();
			_enumerator_list.source_context=((enumerator)LRParser.GetReductionSyntaxNode(0)).source_context;
			_enumerator_list.enumerators.Add((enumerator)LRParser.GetReductionSyntaxNode(0));
			return _enumerator_list;
		}


		case RuleConstants.RULE_ENUMERATOR_LIST_TKCOMMA :
		//<enumerator_list> ::= <enumerator_list> 'tkComma' <enumerator>

		//TemplateList for enumerator_list (add)         
		{
			enumerator_list _enumerator_list=(enumerator_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_enumerator_list,_enumerator_list,LRParser.GetReductionSyntaxNode(2));
			_enumerator_list.enumerators.Add(LRParser.GetReductionSyntaxNode(2) as enumerator);
			return _enumerator_list;
		}


		case RuleConstants.RULE_ENUMERATOR_IDENTIFIER :
		//<enumerator> ::= 'IDENTIFIER'
         
		{
			enumerator _enumerator=new enumerator(LRParser.GetReductionSyntaxNode(0) as ident,null);
			parsertools.create_source_context(_enumerator,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			
			return _enumerator;
		}


		case RuleConstants.RULE_ENUMERATOR_IDENTIFIER_EQUAL :
		//<enumerator> ::= 'IDENTIFIER' 'EQUAL' <constant_expression>
         
		{
			enumerator _enumerator=new enumerator(LRParser.GetReductionSyntaxNode(0) as ident,LRParser.GetReductionSyntaxNode(2) as expression);
			parsertools.create_source_context(_enumerator,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _enumerator;
		}


		case RuleConstants.RULE_DECLARATOR :
		//<declarator> ::= <pointer> <direct_declarator>
 return Converter.PreparePointerDirectDeclarator((ref_type)LRParser.GetReductionSyntaxNode(0),(declaration)LRParser.GetReductionSyntaxNode(1));	

		case RuleConstants.RULE_DECLARATOR2 :
		//<declarator> ::= <direct_declarator>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_DIRECT_DECLARATOR :
		//<direct_declarator> ::= <direct_declarator_ident> <empty>
         
		{
			var_def_statement _var_def_statement=new var_def_statement((ident_list)LRParser.GetReductionSyntaxNode(0),null,null,definition_attribute.None,false);
			
								parsertools.create_source_context(_var_def_statement,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _var_def_statement;
		}


		case RuleConstants.RULE_DIRECT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE :
		//<direct_declarator> ::= 'tkRoundOpen' <declarator> 'tkRoundClose'
 return LRParser.GetReductionSyntaxNode(1); 

		case RuleConstants.RULE_DIRECT_DECLARATOR_TKSQUAREOPEN_TKSQUARECLOSE :
		//<direct_declarator> ::= <direct_declarator> 'tkSquareOpen' <constant_expression> 'tkSquareClose'
 return Converter.PrepareArrayType((declaration)LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1),(expression)LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(3)); 

		case RuleConstants.RULE_DIRECT_DECLARATOR_TKSQUAREOPEN_TKSQUARECLOSE2 :
		//<direct_declarator> ::= <direct_declarator> 'tkSquareOpen' 'tkSquareClose'
 return Converter.PrepareArrayType((declaration)LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1),null,LRParser.GetReductionSyntaxNode(2)); 

		case RuleConstants.RULE_DIRECT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE2 :
		//<direct_declarator> ::= <direct_declarator> 'tkRoundOpen' <parameter_type_list> 'tkRoundClose'
 return Converter.PrepareFunctionHeader((declaration)LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1),(formal_parameters)LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(3)); 

		case RuleConstants.RULE_DIRECT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE3 :
		//<direct_declarator> ::= <direct_declarator> 'tkRoundOpen' <identifier_list> 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<direct_declarator> ::= <direct_declarator> tkRoundOpen <identifier_list> tkRoundClose"));}return null;

		case RuleConstants.RULE_DIRECT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE4 :
		//<direct_declarator> ::= <direct_declarator> 'tkRoundOpen' 'tkRoundClose'
 return Converter.PrepareFunctionHeader((declaration)LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1),null,LRParser.GetReductionSyntaxNode(2)); 

		case RuleConstants.RULE_DIRECT_DECLARATOR_IDENT_IDENTIFIER :
		//<direct_declarator_ident> ::= 'IDENTIFIER' <empty>
         
		{
			ident_list _ident_list=new ident_list();
			 _ident_list.idents.Add((ident)LRParser.GetReductionSyntaxNode(0)); parsertools.assign_source_context(_ident_list,LRParser.GetReductionSyntaxNode(0));
			return _ident_list;
		}


		case RuleConstants.RULE_POINTER_STAR :
		//<pointer> ::= 'STAR'
         
		{
			ref_type _ref_type=new ref_type();
			 parsertools.assign_source_context(_ref_type,LRParser.GetReductionSyntaxNode(0));
			return _ref_type;
		}


		case RuleConstants.RULE_POINTER_STAR2 :
		//<pointer> ::= <pointer> 'STAR'
         
		{
			ref_type _ref_type=new ref_type();
			 _ref_type.pointed_to=(ref_type)LRParser.GetReductionSyntaxNode(0); parsertools.create_source_context(_ref_type,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _ref_type;
		}


		case RuleConstants.RULE_TYPE_QUALIFIER_LIST :
		//<type_qualifier_list> ::= <type_qualifier>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_QUALIFIER_LIST2 :
		//<type_qualifier_list> ::= <type_qualifier_list> <type_qualifier>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<type_qualifier_list> ::= <type_qualifier_list> <type_qualifier>"));}return null;

		case RuleConstants.RULE_PARAMETER_TYPE_LIST :
		//<parameter_type_list> ::= <parameter_list>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_PARAMETER_TYPE_LIST_TKCOMMA_ELLIPSIS :
		//<parameter_type_list> ::= <parameter_list> 'tkComma' 'ELLIPSIS'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<parameter_type_list> ::= <parameter_list> tkComma ELLIPSIS"));}return null;

		case RuleConstants.RULE_PARAMETER_LIST :
		//<parameter_list> ::= <parameter_declaration> <empty>
         
		//TemplateList for formal_parameters (create)
		{
			formal_parameters _formal_parametres=new formal_parameters();
			_formal_parametres.source_context=((typed_parameters)LRParser.GetReductionSyntaxNode(0)).source_context;
			_formal_parametres.params_list.Add((typed_parameters)LRParser.GetReductionSyntaxNode(0));
			return _formal_parametres;
		}


		case RuleConstants.RULE_PARAMETER_LIST_TKCOMMA :
		//<parameter_list> ::= <parameter_list> 'tkComma' <parameter_declaration>

		//TemplateList for formal_parameters (add)         
		{
			formal_parameters _formal_parametres=(formal_parameters)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_formal_parametres,_formal_parametres,LRParser.GetReductionSyntaxNode(2));
			_formal_parametres.params_list.Add(LRParser.GetReductionSyntaxNode(2) as typed_parameters);
			return _formal_parametres;
		}


		case RuleConstants.RULE_PARAMETER_DECLARATION :
		//<parameter_declaration> ::= <declaration_specifiers> <declarator>
 return Converter.PrepareParametrDeclaration((declaration)LRParser.GetReductionSyntaxNode(0),(declaration)LRParser.GetReductionSyntaxNode(1));

		case RuleConstants.RULE_PARAMETER_DECLARATION2 :
		//<parameter_declaration> ::= <declaration_specifiers> <abstract_declarator>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<parameter_declaration> ::= <declaration_specifiers> <abstract_declarator>"));}return null;

		case RuleConstants.RULE_PARAMETER_DECLARATION3 :
		//<parameter_declaration> ::= <declaration_specifiers>
         
		{
			typed_parameters _typed_parametres=new typed_parameters(null,LRParser.GetReductionSyntaxNode(0) as type_definition,parametr_kind.none,null);
			parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			
			return _typed_parametres;
		}


		case RuleConstants.RULE_IDENTIFIER_LIST_IDENTIFIER :
		//<identifier_list> ::= 'IDENTIFIER'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_IDENTIFIER_LIST_TKCOMMA_IDENTIFIER :
		//<identifier_list> ::= <identifier_list> 'tkComma' 'IDENTIFIER'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<identifier_list> ::= <identifier_list> tkComma IDENTIFIER"));}return null;

		case RuleConstants.RULE_TYPE_NAME :
		//<type_name> ::= <specifier_qualifier_list>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_TYPE_NAME2 :
		//<type_name> ::= <specifier_qualifier_list> <abstract_declarator>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<type_name> ::= <specifier_qualifier_list> <abstract_declarator>"));}return null;

		case RuleConstants.RULE_ABSTRACT_DECLARATOR :
		//<abstract_declarator> ::= <pointer>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ABSTRACT_DECLARATOR2 :
		//<abstract_declarator> ::= <direct_abstract_declarator>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ABSTRACT_DECLARATOR3 :
		//<abstract_declarator> ::= <pointer> <direct_abstract_declarator>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<abstract_declarator> ::= <pointer> <direct_abstract_declarator>"));}return null;

		case RuleConstants.RULE_DIRECT_ABSTRACT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE :
		//<direct_abstract_declarator> ::= 'tkRoundOpen' <abstract_declarator> 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<direct_abstract_declarator> ::= tkRoundOpen <abstract_declarator> tkRoundClose"));}return null;

		case RuleConstants.RULE_DIRECT_ABSTRACT_DECLARATOR_TKSQUAREOPEN_TKSQUARECLOSE :
		//<direct_abstract_declarator> ::= 'tkSquareOpen' 'tkSquareClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<direct_abstract_declarator> ::= tkSquareOpen tkSquareClose"));}return null;

		case RuleConstants.RULE_DIRECT_ABSTRACT_DECLARATOR_TKSQUAREOPEN_TKSQUARECLOSE2 :
		//<direct_abstract_declarator> ::= 'tkSquareOpen' <constant_expression> 'tkSquareClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<direct_abstract_declarator> ::= tkSquareOpen <constant_expression> tkSquareClose"));}return null;

		case RuleConstants.RULE_DIRECT_ABSTRACT_DECLARATOR_TKSQUAREOPEN_TKSQUARECLOSE3 :
		//<direct_abstract_declarator> ::= <direct_abstract_declarator> 'tkSquareOpen' 'tkSquareClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<direct_abstract_declarator> ::= <direct_abstract_declarator> tkSquareOpen tkSquareClose"));}return null;

		case RuleConstants.RULE_DIRECT_ABSTRACT_DECLARATOR_TKSQUAREOPEN_TKSQUARECLOSE4 :
		//<direct_abstract_declarator> ::= <direct_abstract_declarator> 'tkSquareOpen' <constant_expression> 'tkSquareClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<direct_abstract_declarator> ::= <direct_abstract_declarator> tkSquareOpen <constant_expression> tkSquareClose"));}return null;

		case RuleConstants.RULE_DIRECT_ABSTRACT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE2 :
		//<direct_abstract_declarator> ::= 'tkRoundOpen' 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<direct_abstract_declarator> ::= tkRoundOpen tkRoundClose"));}return null;

		case RuleConstants.RULE_DIRECT_ABSTRACT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE3 :
		//<direct_abstract_declarator> ::= 'tkRoundOpen' <parameter_type_list> 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<direct_abstract_declarator> ::= tkRoundOpen <parameter_type_list> tkRoundClose"));}return null;

		case RuleConstants.RULE_DIRECT_ABSTRACT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE4 :
		//<direct_abstract_declarator> ::= <direct_abstract_declarator> 'tkRoundOpen' 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<direct_abstract_declarator> ::= <direct_abstract_declarator> tkRoundOpen tkRoundClose"));}return null;

		case RuleConstants.RULE_DIRECT_ABSTRACT_DECLARATOR_TKROUNDOPEN_TKROUNDCLOSE5 :
		//<direct_abstract_declarator> ::= <direct_abstract_declarator> 'tkRoundOpen' <parameter_type_list> 'tkRoundClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<direct_abstract_declarator> ::= <direct_abstract_declarator> tkRoundOpen <parameter_type_list> tkRoundClose"));}return null;

		case RuleConstants.RULE_INITIALIZER :
		//<initializer> ::= <assignment_expression>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_INITIALIZER_TKFIGUREOPEN_TKFIGURECLOSE :
		//<initializer> ::= 'tkFigureOpen' <initializer_list> 'tkFigureClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<initializer> ::= tkFigureOpen <initializer_list> tkFigureClose"));}return null;

		case RuleConstants.RULE_INITIALIZER_TKFIGUREOPEN_TKCOMMA_TKFIGURECLOSE :
		//<initializer> ::= 'tkFigureOpen' <initializer_list> 'tkComma' 'tkFigureClose'
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<initializer> ::= tkFigureOpen <initializer_list> tkComma tkFigureClose"));}return null;

		case RuleConstants.RULE_INITIALIZER_LIST :
		//<initializer_list> ::= <initializer>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_INITIALIZER_LIST_TKCOMMA :
		//<initializer_list> ::= <initializer_list> 'tkComma' <initializer>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<initializer_list> ::= <initializer_list> tkComma <initializer>"));}return null;

		case RuleConstants.RULE_STATEMENT :
		//<statement> ::= <labeled_statement>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_STATEMENT2 :
		//<statement> ::= <compound_statement>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_STATEMENT3 :
		//<statement> ::= <expression_statement>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_STATEMENT4 :
		//<statement> ::= <selection_statement>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_STATEMENT5 :
		//<statement> ::= <iteration_statement>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_STATEMENT6 :
		//<statement> ::= <jump_statement>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_LABELED_STATEMENT_IDENTIFIER_TKCOLON :
		//<labeled_statement> ::= 'IDENTIFIER' 'tkColon' <statement>
         
		{
			labeled_statement _labeled_statement=new labeled_statement((ident)LRParser.GetReductionSyntaxNode(0),(statement)LRParser.GetReductionSyntaxNode(2));
			parsertools.create_source_context(_labeled_statement,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _labeled_statement;
		}


		case RuleConstants.RULE_LABELED_STATEMENT_CASE_TKCOLON :
		//<labeled_statement> ::= 'CASE' <constant_expression> 'tkColon' <statement>
         
		{
			switch_stmt _switch_stmt=new switch_stmt((expression)LRParser.GetReductionSyntaxNode(1),(statement)LRParser.GetReductionSyntaxNode(3),SwitchPartType.Case);
			
								parsertools.create_source_context(_switch_stmt,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _switch_stmt;
		}


		case RuleConstants.RULE_LABELED_STATEMENT_DEFAULT_TKCOLON :
		//<labeled_statement> ::= 'DEFAULT' 'tkColon' <statement>
         
		{
			switch_stmt _switch_stmt=new switch_stmt(null,(statement)LRParser.GetReductionSyntaxNode(2),SwitchPartType.Default);
			
								parsertools.create_source_context(_switch_stmt,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _switch_stmt;
		}


		case RuleConstants.RULE_COMPOUND_STATEMENT_TKFIGUREOPEN_TKFIGURECLOSE :
		//<compound_statement> ::= 'tkFigureOpen' 'tkFigureClose'
         
		{
			statement_list _statement_list=new statement_list(null,(syntax_tree_node)LRParser.GetReductionSyntaxNode(0),(syntax_tree_node)LRParser.GetReductionSyntaxNode(1));
			 _statement_list.subnodes = new List<statement>(); parsertools.create_source_context(_statement_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _statement_list;
		}


		case RuleConstants.RULE_COMPOUND_STATEMENT_TKFIGUREOPEN_TKFIGURECLOSE2 :
		//<compound_statement> ::= 'tkFigureOpen' <statement_list> 'tkFigureClose'
         
		{
			statement_list _statement_list=(statement_list)LRParser.GetReductionSyntaxNode(1);_statement_list.left_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(0);_statement_list.right_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(2); parsertools.create_source_context(_statement_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _statement_list;
		}


		case RuleConstants.RULE_COMPOUND_STATEMENT_TKFIGUREOPEN_TKFIGURECLOSE3 :
		//<compound_statement> ::= 'tkFigureOpen' <declaration_list> 'tkFigureClose'
         
		{
			statement_list _statement_list=(statement_list)LRParser.GetReductionSyntaxNode(1);_statement_list.left_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(0);_statement_list.right_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(2); parsertools.create_source_context(_statement_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _statement_list;
		}


		case RuleConstants.RULE_COMPOUND_STATEMENT_TKFIGUREOPEN_TKFIGURECLOSE4 :
		//<compound_statement> ::= 'tkFigureOpen' <declaration_list> <statement_list> 'tkFigureClose'
         
		{
			statement_list _statement_list=(statement_list)LRParser.GetReductionSyntaxNode(1);_statement_list.left_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(0);_statement_list.right_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(3);
								foreach(statement stmt in ((statement_list)LRParser.GetReductionSyntaxNode(2)).subnodes)
									_statement_list.subnodes.Add(stmt);
								parsertools.create_source_context(_statement_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _statement_list;
		}


		case RuleConstants.RULE_DECLARATION_LIST :
		//<declaration_list> ::= <declaration>
         
		{
			statement_list _statement_list=new statement_list();
			 _statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(0));parsertools.create_source_context(_statement_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _statement_list;
		}


		case RuleConstants.RULE_DECLARATION_LIST2 :
		//<declaration_list> ::= <declaration_list> <declaration>
         
		{
			statement_list _statement_list=(statement_list)LRParser.GetReductionSyntaxNode(0); _statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(1));parsertools.create_source_context(_statement_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _statement_list;
		}


		case RuleConstants.RULE_STATEMENT_LIST :
		//<statement_list> ::= <statement>
         
		{
			statement_list _statement_list=new statement_list();
			 _statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(0));parsertools.create_source_context(_statement_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _statement_list;
		}


		case RuleConstants.RULE_STATEMENT_LIST2 :
		//<statement_list> ::= <statement_list> <statement>
         
		{
			statement_list _statement_list=(statement_list)LRParser.GetReductionSyntaxNode(0); _statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(1));parsertools.create_source_context(_statement_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _statement_list;
		}


		case RuleConstants.RULE_EXPRESSION_STATEMENT_SEMICOLON :
		//<expression_statement> ::= 'SEMICOLON' <empty>
         
		{
			empty_statement _empty_statement=new empty_statement();
			 parsertools.create_source_context(_empty_statement,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _empty_statement;
		}


		case RuleConstants.RULE_EXPRESSION_STATEMENT_SEMICOLON2 :
		//<expression_statement> ::= <expression> 'SEMICOLON'
 return LRParser.GetReductionSyntaxNode(0); 

		case RuleConstants.RULE_EXPRESSION_STATEMENT_OR_DECLARATION :
		//<expression_statement_or_declaration> ::= <expression_statement>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_EXPRESSION_STATEMENT_OR_DECLARATION2 :
		//<expression_statement_or_declaration> ::= <declaration>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_SELECTION_STATEMENT_IF_TKROUNDOPEN_TKROUNDCLOSE :
		//<selection_statement> ::= 'IF' 'tkRoundOpen' <expression> 'tkRoundClose' <statement>
         
		{
			if_node _if_node=new if_node((expression)LRParser.GetReductionSyntaxNode(2),(statement)LRParser.GetReductionSyntaxNode(4),null);
			 
								 parsertools.create_source_context(_if_node,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
			return _if_node;
		}


		case RuleConstants.RULE_SELECTION_STATEMENT_IF_TKROUNDOPEN_TKROUNDCLOSE_ELSE :
		//<selection_statement> ::= 'IF' 'tkRoundOpen' <expression> 'tkRoundClose' <statement> 'ELSE' <statement>
         
		{
			if_node _if_node=new if_node((expression)LRParser.GetReductionSyntaxNode(2),(statement)LRParser.GetReductionSyntaxNode(4),(statement)LRParser.GetReductionSyntaxNode(6));
			 
								 parsertools.create_source_context(_if_node,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(6));
			return _if_node;
		}


		case RuleConstants.RULE_SELECTION_STATEMENT_SWITCH_TKROUNDOPEN_TKROUNDCLOSE :
		//<selection_statement> ::= 'SWITCH' 'tkRoundOpen' <expression> 'tkRoundClose' <statement>
         
		{
			switch_stmt _switch_stmt=new switch_stmt((expression)LRParser.GetReductionSyntaxNode(2),(statement)LRParser.GetReductionSyntaxNode(4),SwitchPartType.Switch);
			
								 parsertools.create_source_context(_switch_stmt,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
			return _switch_stmt;
		}


		case RuleConstants.RULE_ITERATION_STATEMENT_WHILE_TKROUNDOPEN_TKROUNDCLOSE :
		//<iteration_statement> ::= 'WHILE' 'tkRoundOpen' <expression> 'tkRoundClose' <statement>
         
		{
			while_node _while_node=new while_node((expression)LRParser.GetReductionSyntaxNode(2),(statement)LRParser.GetReductionSyntaxNode(4),WhileCycleType.While);
			
								 parsertools.create_source_context(_while_node,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
			return _while_node;
		}


		case RuleConstants.RULE_ITERATION_STATEMENT_DO_WHILE_TKROUNDOPEN_TKROUNDCLOSE_SEMICOLON :
		//<iteration_statement> ::= 'DO' <statement> 'WHILE' 'tkRoundOpen' <expression> 'tkRoundClose' 'SEMICOLON'
         
		{
			while_node _while_node=new while_node((expression)LRParser.GetReductionSyntaxNode(4),(statement)LRParser.GetReductionSyntaxNode(1),WhileCycleType.DoWhile);
			
								 parsertools.create_source_context(_while_node,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(6));
			return _while_node;
		}


		case RuleConstants.RULE_ITERATION_STATEMENT_FOR_TKROUNDOPEN_TKROUNDCLOSE :
		//<iteration_statement> ::= 'FOR' 'tkRoundOpen' <expression_statement_or_declaration> <expression_statement> 'tkRoundClose' <statement>
         
		{
			c_for_cycle _c_for_cycle=new c_for_cycle(LRParser.GetReductionSyntaxNode(2) as statement,LRParser.GetReductionSyntaxNode(3) as expression,null,(statement)LRParser.GetReductionSyntaxNode(5));
			
                                                                 parsertools.create_source_context(_c_for_cycle,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(5));
			return _c_for_cycle;
		}


		case RuleConstants.RULE_ITERATION_STATEMENT_FOR_TKROUNDOPEN_TKROUNDCLOSE2 :
		//<iteration_statement> ::= 'FOR' 'tkRoundOpen' <expression_statement_or_declaration> <expression_statement> <expression> 'tkRoundClose' <statement>
         
		{
			c_for_cycle _c_for_cycle=new c_for_cycle(LRParser.GetReductionSyntaxNode(2) as statement,LRParser.GetReductionSyntaxNode(3) as expression,LRParser.GetReductionSyntaxNode(4) as expression,(statement)LRParser.GetReductionSyntaxNode(6));
			
                                                                 parsertools.create_source_context(_c_for_cycle,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(6));
			return _c_for_cycle;
		}


		case RuleConstants.RULE_JUMP_STATEMENT_GOTO_IDENTIFIER_SEMICOLON :
		//<jump_statement> ::= 'GOTO' 'IDENTIFIER' 'SEMICOLON'
         
		{
			goto_statement _goto_statement=new goto_statement((ident)LRParser.GetReductionSyntaxNode(1));
			 parsertools.create_source_context(_goto_statement,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _goto_statement;
		}


		case RuleConstants.RULE_JUMP_STATEMENT_CONTINUE_SEMICOLON :
		//<jump_statement> ::= 'CONTINUE' 'SEMICOLON'
         
		{
			jump_stmt _jump_stmt=new jump_stmt(null,JumpStmtType.Continue);
			 parsertools.create_source_context(_jump_stmt,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _jump_stmt;
		}


		case RuleConstants.RULE_JUMP_STATEMENT_BREAK_SEMICOLON :
		//<jump_statement> ::= 'BREAK' 'SEMICOLON'
         
		{
			jump_stmt _jump_stmt=new jump_stmt(null,JumpStmtType.Break);
			 parsertools.create_source_context(_jump_stmt,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _jump_stmt;
		}


		case RuleConstants.RULE_JUMP_STATEMENT_RETURN_SEMICOLON :
		//<jump_statement> ::= 'RETURN' 'SEMICOLON'
         
		{
			jump_stmt _jump_stmt=new jump_stmt(null,JumpStmtType.Return);
			 parsertools.create_source_context(_jump_stmt,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _jump_stmt;
		}


		case RuleConstants.RULE_JUMP_STATEMENT_RETURN_SEMICOLON2 :
		//<jump_statement> ::= 'RETURN' <expression> 'SEMICOLON'
         
		{
			jump_stmt _jump_stmt=new jump_stmt((expression)LRParser.GetReductionSyntaxNode(1),JumpStmtType.Return);
			 parsertools.create_source_context(_jump_stmt,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _jump_stmt;
		}


		case RuleConstants.RULE_TRANSLATION_UNIT :
		//<translation_unit> ::= <external_declaration_list>
         
		{
			c_module _c_module=new c_module(LRParser.GetReductionSyntaxNode(0) as declarations,null);
			 
								_c_module.Language = LanguageId.C;
								parsertools.assign_source_context(_c_module,LRParser.GetReductionSyntaxNode(0));
			return _c_module;
		}


		case RuleConstants.RULE_EXTERNAL_DECLARATION_LIST :
		//<external_declaration_list> ::= <external_declaration>
         
		{
			declarations _declarations=new declarations();
			 
									Converter.AddToSubporgramDefs(_declarations.defs,LRParser.GetReductionSyntaxNode(0));
									parsertools.assign_source_context(_declarations,LRParser.GetReductionSyntaxNode(0));
			return _declarations;
		}


		case RuleConstants.RULE_EXTERNAL_DECLARATION_LIST2 :
		//<external_declaration_list> ::= <external_declaration_list> <external_declaration>
         
		{
			declarations _declarations=(declarations)LRParser.GetReductionSyntaxNode(0); 
									Converter.AddToSubporgramDefs(_declarations.defs,LRParser.GetReductionSyntaxNode(1));
									parsertools.create_source_context(_declarations,_declarations,LRParser.GetReductionSyntaxNode(1));
			return _declarations;
		}


		case RuleConstants.RULE_EXTERNAL_DECLARATION :
		//<external_declaration> ::= <function_definition>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_EXTERNAL_DECLARATION2 :
		//<external_declaration> ::= <declaration> <empty>

				  if(LRParser.GetReductionSyntaxNode(0) is declarations_as_statement) 
				       return ((declarations_as_statement)LRParser.GetReductionSyntaxNode(0)).defs;
				  return LRParser.GetReductionSyntaxNode(0);
				  

		case RuleConstants.RULE_FUNCTION_DEFINITION_HEADER :
		//<function_definition_header> ::= <declaration_specifiers> <declarator>
 return Converter.PrepareFunctionDefinitionHeader((type_definition)LRParser.GetReductionSyntaxNode(0),(declaration)LRParser.GetReductionSyntaxNode(1));

		case RuleConstants.RULE_FUNCTION_DEFINITION_HEADER2 :
		//<function_definition_header> ::= <declarator> <empty>

								syntax_tree_node s=Converter.PrepareFunctionDefinitionHeader(null,(declaration)LRParser.GetReductionSyntaxNode(0));
								//errors.Add(new MissingTypeSpecifier(current_file_name,s.source_context,s));
								return s;
								

		case RuleConstants.RULE_FUNCTION_DEFINITION_COMPOUND_STATEMENT :
		//<function_definition_compound_statement> ::= <declaration_list> <compound_statement>

								statement_list l=LRParser.GetReductionSyntaxNode(0) as statement_list,r=LRParser.GetReductionSyntaxNode(1) as statement_list;
								foreach(statement stmt in r.subnodes)
									l.subnodes.Add(stmt);
								l.left_logical_bracket=r.left_logical_bracket;
								l.right_logical_bracket=r.right_logical_bracket;
								parsertools.create_source_context(l,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
								return l; 
								

		case RuleConstants.RULE_FUNCTION_DEFINITION_COMPOUND_STATEMENT2 :
		//<function_definition_compound_statement> ::= <compound_statement>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_FUNCTION_DEFINITION :
		//<function_definition> ::= <function_definition_header> <function_definition_compound_statement>
         
		{
			procedure_definition _procedure_definition=new procedure_definition((function_header)LRParser.GetReductionSyntaxNode(0),null);
			
								 
								block bl=new block(null,(statement_list)LRParser.GetReductionSyntaxNode(1));
								parsertools.create_source_context(bl,LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(1));
								_procedure_definition.proc_body=bl;
	  							parsertools.create_source_context(_procedure_definition,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _procedure_definition;
		}


		case RuleConstants.RULE_EMPTY :
		//<empty> ::= 
		//NONTERMINAL:<empty> ::= 
		return null;
		//ENDNONTERMINAL

            }
            throw new RuleException("Unknown rule");
        }
	}
}
