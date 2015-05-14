
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using PascalABCCompiler.KuMirParser.Errors;
using PascalABCCompiler.ParserTools;
using GoldParser;

namespace PascalABCCompiler.KuMirParser
{
    public partial class GPBParser_KuMir : GPBParser
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
            SYMBOL_COMMENTLINE = 3, // (Comment Line)
            SYMBOL_TK_ALG = 4, // 'tk_alg'
            SYMBOL_TK_AND = 5, // 'tk_and'
            SYMBOL_TK_ARG = 6, // 'tk_arg'
            SYMBOL_TK_ARRAY = 7, // 'tk_array'
            SYMBOL_TK_ASSERT = 8, // 'tk_assert'
            SYMBOL_TK_ASSIGN = 9, // 'tk_Assign'
            SYMBOL_TK_BEGIN = 10, // 'tk_begin'
            SYMBOL_TK_BEGIN_CYCLE = 11, // 'tk_begin_cycle'
            SYMBOL_TK_BOOLEAN_TYPE = 12, // 'tk_boolean_type'
            SYMBOL_TK_CASE = 13, // 'tk_case'
            SYMBOL_TK_CASE_V = 14, // 'tk_case_v'
            SYMBOL_TK_CHAR = 15, // 'tk_char'
            SYMBOL_TK_CHAR_TYPE = 16, // 'tk_char_type'
            SYMBOL_TK_COLON = 17, // 'tk_Colon'
            SYMBOL_TK_COMMA = 18, // 'tk_Comma'
            SYMBOL_TK_DIV = 19, // 'tk_Div'
            SYMBOL_TK_DOT = 20, // 'tk_Dot'
            SYMBOL_TK_ELSE = 21, // 'tk_else'
            SYMBOL_TK_END = 22, // 'tk_end'
            SYMBOL_TK_END_ALL = 23, // 'tk_end_all'
            SYMBOL_TK_END_CYCLE = 24, // 'tk_end_cycle'
            SYMBOL_TK_EOL = 25, // 'tk_eol'
            SYMBOL_TK_EQUAL = 26, // 'tk_Equal'
            SYMBOL_TK_FALSE = 27, // 'tk_false'
            SYMBOL_TK_FOR = 28, // 'tk_for'
            SYMBOL_TK_FROM = 29, // 'tk_from'
            SYMBOL_TK_FUNC_VAL = 30, // 'tk_func_val'
            SYMBOL_TK_GREATER = 31, // 'tk_Greater'
            SYMBOL_TK_GREATEREQUAL = 32, // 'tk_GreaterEqual'
            SYMBOL_TK_IDENTIFIER = 33, // 'tk_Identifier'
            SYMBOL_TK_IF = 34, // 'tk_if'
            SYMBOL_TK_INTEGER = 35, // 'tk_integer'
            SYMBOL_TK_INTEGER_TYPE = 36, // 'tk_integer_type'
            SYMBOL_TK_ISP = 37, // 'tk_isp'
            SYMBOL_TK_LOWER = 38, // 'tk_Lower'
            SYMBOL_TK_LOWEREQUAL = 39, // 'tk_LowerEqual'
            SYMBOL_TK_MINUS = 40, // 'tk_Minus'
            SYMBOL_TK_MULT = 41, // 'tk_Mult'
            SYMBOL_TK_NEWLINE = 42, // 'tk_NewLine'
            SYMBOL_TK_NOT = 43, // 'tk_not'
            SYMBOL_TK_NOTEQUAL = 44, // 'tk_NotEqual'
            SYMBOL_TK_OR = 45, // 'tk_or'
            SYMBOL_TK_PLUS = 46, // 'tk_Plus'
            SYMBOL_TK_POWER = 47, // 'tk_Power'
            SYMBOL_TK_RAZ = 48, // 'tk_raz'
            SYMBOL_TK_READ = 49, // 'tk_read'
            SYMBOL_TK_REAL = 50, // 'tk_real'
            SYMBOL_TK_REAL_TYPE = 51, // 'tk_real_type'
            SYMBOL_TK_ROUNDCLOSE = 52, // 'tk_RoundClose'
            SYMBOL_TK_ROUNDOPEN = 53, // 'tk_RoundOpen'
            SYMBOL_TK_SEMICOLON = 54, // 'tk_SemiColon'
            SYMBOL_TK_SQUARECLOSE = 55, // 'tk_SquareClose'
            SYMBOL_TK_SQUAREOPEN = 56, // 'tk_SquareOpen'
            SYMBOL_TK_STRING_TYPE = 57, // 'tk_string_type'
            SYMBOL_TK_STRINGLITERAL = 58, // 'tk_StringLiteral'
            SYMBOL_TK_THEN = 59, // 'tk_then'
            SYMBOL_TK_TO = 60, // 'tk_to'
            SYMBOL_TK_TRUE = 61, // 'tk_true'
            SYMBOL_TK_USES = 62, // 'tk_uses'
            SYMBOL_TK_VAR = 63, // 'tk_var'
            SYMBOL_TK_WHILE = 64, // 'tk_while'
            SYMBOL_TK_WRITE = 65, // 'tk_write'
            SYMBOL_ADDEXP = 66, // <Add Exp>
            SYMBOL_ARRAY_LIST1 = 67, // <Array_list1>
            SYMBOL_ARRAY_LIST2 = 68, // <Array_list2>
            SYMBOL_CASE_VARIANT = 69, // <Case_variant>
            SYMBOL_CASE_VARIANT_LIST = 70, // <Case_variant_list>
            SYMBOL_DECLARATIONS = 71, // <Declarations>
            SYMBOL_DIAP = 72, // <Diap>
            SYMBOL_DIAP_LIST = 73, // <Diap_list>
            SYMBOL_EXPRESSION = 74, // <Expression>
            SYMBOL_FACT_LIST = 75, // <Fact_list>
            SYMBOL_FORMAL_LIST = 76, // <Formal_list>
            SYMBOL_FORMAL_PARAMETER1 = 77, // <Formal_parameter1>
            SYMBOL_FORMAL_PARAMETER2 = 78, // <Formal_Parameter2>
            SYMBOL_FORMAL_TYPE_LIST1 = 79, // <Formal_type_list1>
            SYMBOL_FORMAL_TYPE_LIST2 = 80, // <Formal_type_list2>
            SYMBOL_FREE_OPERATOR = 81, // <Free_operator>
            SYMBOL_FUNCTION = 82, // <Function>
            SYMBOL_GLOBAL_DECL_LIST = 83, // <Global_decl_list>
            SYMBOL_GLOBAL_PART = 84, // <Global_part>
            SYMBOL_GLOBAL_VARS = 85, // <Global_vars>
            SYMBOL_ID_LIST1 = 86, // <Id_list1>
            SYMBOL_ID_LIST2 = 87, // <Id_list2>
            SYMBOL_INITIALIZATION = 88, // <Initialization>
            SYMBOL_LIST_OF_EXPRESSIONS = 89, // <List_of_expressions>
            SYMBOL_MULTEXP = 90, // <Mult Exp>
            SYMBOL_NEGATEEXP = 91, // <Negate Exp>
            SYMBOL_POWEREXP = 92, // <Power Exp>
            SYMBOL_PROCEDURE = 93, // <Procedure>
            SYMBOL_PROGRAM = 94, // <Program>
            SYMBOL_SEPARATOR = 95, // <Separator>
            SYMBOL_SEPARATORS = 96, // <Separators>
            SYMBOL_SEPARATORSOPT = 97, // <Separators Opt>
            SYMBOL_STATEMENT = 98, // <Statement>
            SYMBOL_STATEMENTS = 99, // <Statements>
            SYMBOL_SUB_DECLARATIONS = 100, // <Sub_declarations>
            SYMBOL_TYPE = 101, // <Type>
            SYMBOL_USES_UNITS = 102, // <Uses_units>
            SYMBOL_VALUE = 103, // <Value>
            SYMBOL_VAR_DECL_LIST1 = 104, // <Var_decl_list1>
            SYMBOL_VAR_DECL_LIST2 = 105, // <Var_decl_list2>
            SYMBOL_VAR_DECLARATIONS1 = 106, // <Var_declarations1>
            SYMBOL_VAR_DECLARATIONS2 = 107  // <Var_declarations2>
        };














        ///////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////
        //RuleConstants
        ///////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////

        public enum RuleConstants : int
        {
            RULE_SEPARATOR_TK_SEMICOLON = 0, // <Separator> ::= 'tk_SemiColon'
            RULE_SEPARATOR_TK_NEWLINE = 1, // <Separator> ::= 'tk_NewLine'
            RULE_SEPARATORS = 2, // <Separators> ::= <Separators> <Separator>
            RULE_SEPARATORS2 = 3, // <Separators> ::= <Separator>
            RULE_SEPARATORSOPT = 4, // <Separators Opt> ::= <Separator> <Separators Opt>
            RULE_SEPARATORSOPT2 = 5, // <Separators Opt> ::= 
            RULE_PROGRAM_TK_ALG_TK_IDENTIFIER_TK_BEGIN_TK_END = 6, // <Program> ::= <Separators Opt> <Global_part> 'tk_alg' 'tk_Identifier' <Separators> 'tk_begin' <Statements> 'tk_end' <Sub_declarations>
            RULE_PROGRAM_TK_ISP_TK_IDENTIFIER_TK_END = 7, // <Program> ::= <Separators Opt> 'tk_isp' 'tk_Identifier' <Separators> <Global_part> <Sub_declarations> 'tk_end'
            RULE_PROCEDURE_TK_ALG_TK_IDENTIFIER_TK_ROUNDOPEN_TK_ROUNDCLOSE_TK_BEGIN_TK_END = 8, // <Procedure> ::= 'tk_alg' 'tk_Identifier' 'tk_RoundOpen' <Formal_list> 'tk_RoundClose' <Separators> 'tk_begin' <Statements> 'tk_end'
            RULE_PROCEDURE_TK_ALG_TK_IDENTIFIER_TK_BEGIN_TK_END = 9, // <Procedure> ::= 'tk_alg' 'tk_Identifier' <Separators> 'tk_begin' <Statements> 'tk_end'
            RULE_FUNCTION_TK_ALG_TK_IDENTIFIER_TK_ROUNDOPEN_TK_ROUNDCLOSE_TK_BEGIN_TK_END = 10, // <Function> ::= 'tk_alg' <Type> 'tk_Identifier' 'tk_RoundOpen' <Formal_list> 'tk_RoundClose' <Separators> 'tk_begin' <Statements> 'tk_end'
            RULE_FUNCTION_TK_ALG_TK_IDENTIFIER_TK_BEGIN_TK_END = 11, // <Function> ::= 'tk_alg' <Type> 'tk_Identifier' <Separators> 'tk_begin' <Statements> 'tk_end'
            RULE_SUB_DECLARATIONS = 12, // <Sub_declarations> ::= 
            RULE_SUB_DECLARATIONS2 = 13, // <Sub_declarations> ::= <Sub_declarations> <Procedure>
            RULE_SUB_DECLARATIONS3 = 14, // <Sub_declarations> ::= <Sub_declarations> <Function>
            RULE_SUB_DECLARATIONS4 = 15, // <Sub_declarations> ::= <Sub_declarations> <Separators>
            RULE_TYPE_TK_INTEGER_TYPE = 16, // <Type> ::= 'tk_integer_type'
            RULE_TYPE_TK_REAL_TYPE = 17, // <Type> ::= 'tk_real_type'
            RULE_TYPE_TK_BOOLEAN_TYPE = 18, // <Type> ::= 'tk_boolean_type'
            RULE_TYPE_TK_CHAR_TYPE = 19, // <Type> ::= 'tk_char_type'
            RULE_TYPE_TK_STRING_TYPE = 20, // <Type> ::= 'tk_string_type'
            RULE_DIAP_TK_COLON = 21, // <Diap> ::= <Expression> 'tk_Colon' <Expression>
            RULE_DIAP_LIST_TK_COMMA = 22, // <Diap_list> ::= <Diap_list> 'tk_Comma' <Diap>
            RULE_DIAP_LIST = 23, // <Diap_list> ::= <Diap>
            RULE_LIST_OF_EXPRESSIONS_TK_COMMA = 24, // <List_of_expressions> ::= <List_of_expressions> 'tk_Comma' <Expression>
            RULE_LIST_OF_EXPRESSIONS = 25, // <List_of_expressions> ::= <Expression>
            RULE_ID_LIST1_TK_IDENTIFIER = 26, // <Id_list1> ::= 'tk_Identifier'
            RULE_ID_LIST1_TK_IDENTIFIER_TK_COMMA = 27, // <Id_list1> ::= 'tk_Identifier' 'tk_Comma' <Id_list1>
            RULE_ARRAY_LIST1_TK_IDENTIFIER_TK_SQUAREOPEN_TK_SQUARECLOSE = 28, // <Array_list1> ::= 'tk_Identifier' 'tk_SquareOpen' <Diap_list> 'tk_SquareClose'
            RULE_ARRAY_LIST1_TK_IDENTIFIER_TK_SQUAREOPEN_TK_SQUARECLOSE_TK_COMMA = 29, // <Array_list1> ::= 'tk_Identifier' 'tk_SquareOpen' <Diap_list> 'tk_SquareClose' 'tk_Comma' <Array_list1>
            RULE_ID_LIST2_TK_IDENTIFIER_TK_COMMA = 30, // <Id_list2> ::= 'tk_Identifier' 'tk_Comma'
            RULE_ID_LIST2_TK_IDENTIFIER_TK_COMMA2 = 31, // <Id_list2> ::= 'tk_Identifier' 'tk_Comma' <Id_list2>
            RULE_ARRAY_LIST2_TK_IDENTIFIER_TK_SQUAREOPEN_TK_SQUARECLOSE_TK_COMMA = 32, // <Array_list2> ::= 'tk_Identifier' 'tk_SquareOpen' <Diap_list> 'tk_SquareClose' 'tk_Comma'
            RULE_ARRAY_LIST2_TK_IDENTIFIER_TK_SQUAREOPEN_TK_SQUARECLOSE_TK_COMMA2 = 33, // <Array_list2> ::= 'tk_Identifier' 'tk_SquareOpen' <Diap_list> 'tk_SquareClose' 'tk_Comma' <Array_list2>
            RULE_VAR_DECLARATIONS1 = 34, // <Var_declarations1> ::= <Type> <Id_list1>
            RULE_VAR_DECLARATIONS1_TK_ARRAY = 35, // <Var_declarations1> ::= <Type> 'tk_array' <Array_list1>
            RULE_VAR_DECLARATIONS2 = 36, // <Var_declarations2> ::= <Type> <Id_list2>
            RULE_VAR_DECLARATIONS2_TK_ARRAY = 37, // <Var_declarations2> ::= <Type> 'tk_array' <Array_list2>
            RULE_VAR_DECL_LIST1 = 38, // <Var_decl_list1> ::= <Var_declarations1>
            RULE_VAR_DECL_LIST12 = 39, // <Var_decl_list1> ::= <Var_decl_list2> <Var_declarations1>
            RULE_VAR_DECL_LIST2 = 40, // <Var_decl_list2> ::= <Var_declarations2>
            RULE_VAR_DECL_LIST22 = 41, // <Var_decl_list2> ::= <Var_decl_list2> <Var_declarations2>
            RULE_DECLARATIONS = 42, // <Declarations> ::= <Var_decl_list1>
            RULE_USES_UNITS_TK_USES = 43, // <Uses_units> ::= 'tk_uses' <Id_list1>
            RULE_GLOBAL_DECL_LIST = 44, // <Global_decl_list> ::= <Global_decl_list> <Separators> <Declarations>
            RULE_GLOBAL_DECL_LIST2 = 45, // <Global_decl_list> ::= <Declarations>
            RULE_INITIALIZATION_TK_ASSIGN = 46, // <Initialization> ::= <Initialization> <Separators> <Value> 'tk_Assign' <Expression>
            RULE_INITIALIZATION_TK_ASSIGN2 = 47, // <Initialization> ::= <Value> 'tk_Assign' <Expression>
            RULE_GLOBAL_VARS = 48, // <Global_vars> ::= <Global_decl_list> <Separators> <Initialization> <Separators>
            RULE_GLOBAL_VARS2 = 49, // <Global_vars> ::= <Global_decl_list> <Separators>
            RULE_GLOBAL_PART = 50, // <Global_part> ::= 
            RULE_GLOBAL_PART2 = 51, // <Global_part> ::= <Uses_units> <Separators>
            RULE_GLOBAL_PART3 = 52, // <Global_part> ::= <Uses_units> <Separators> <Global_vars>
            RULE_GLOBAL_PART4 = 53, // <Global_part> ::= <Global_vars>
            RULE_FORMAL_PARAMETER1 = 54, // <Formal_parameter1> ::= <Type> <Id_list1>
            RULE_FORMAL_PARAMETER1_TK_VAR = 55, // <Formal_parameter1> ::= 'tk_var' <Type> <Id_list1>
            RULE_FORMAL_PARAMETER1_TK_ARRAY = 56, // <Formal_parameter1> ::= <Type> 'tk_array' <Array_list1>
            RULE_FORMAL_PARAMETER1_TK_VAR_TK_ARRAY = 57, // <Formal_parameter1> ::= 'tk_var' <Type> 'tk_array' <Array_list1>
            RULE_FORMAL_PARAMETER2 = 58, // <Formal_Parameter2> ::= <Type> <Id_list2>
            RULE_FORMAL_PARAMETER2_TK_VAR = 59, // <Formal_Parameter2> ::= 'tk_var' <Type> <Id_list2>
            RULE_FORMAL_PARAMETER2_TK_ARRAY = 60, // <Formal_Parameter2> ::= <Type> 'tk_array' <Array_list2>
            RULE_FORMAL_PARAMETER2_TK_VAR_TK_ARRAY = 61, // <Formal_Parameter2> ::= 'tk_var' <Type> 'tk_array' <Array_list2>
            RULE_FORMAL_TYPE_LIST1 = 62, // <Formal_type_list1> ::= <Formal_parameter1>
            RULE_FORMAL_TYPE_LIST12 = 63, // <Formal_type_list1> ::= <Formal_type_list2> <Formal_parameter1>
            RULE_FORMAL_TYPE_LIST2 = 64, // <Formal_type_list2> ::= <Formal_Parameter2>
            RULE_FORMAL_TYPE_LIST22 = 65, // <Formal_type_list2> ::= <Formal_type_list2> <Formal_Parameter2>
            RULE_FORMAL_LIST = 66, // <Formal_list> ::= 
            RULE_FORMAL_LIST2 = 67, // <Formal_list> ::= <Formal_type_list1>
            RULE_FACT_LIST = 68, // <Fact_list> ::= 
            RULE_FACT_LIST2 = 69, // <Fact_list> ::= <List_of_expressions>
            RULE_FREE_OPERATOR = 70, // <Free_operator> ::= 
            RULE_CASE_VARIANT_LIST = 71, // <Case_variant_list> ::= <Case_variant> <Case_variant_list>
            RULE_CASE_VARIANT_LIST2 = 72, // <Case_variant_list> ::= <Case_variant>
            RULE_CASE_VARIANT_TK_CASE_V_TK_COLON = 73, // <Case_variant> ::= 'tk_case_v' <Expression> 'tk_Colon' <Statements>
            RULE_STATEMENTS = 74, // <Statements> ::= <Statements> <Separators> <Statement>
            RULE_STATEMENTS2 = 75, // <Statements> ::= <Statement>
            RULE_STATEMENT = 76, // <Statement> ::= <Free_operator>
            RULE_STATEMENT2 = 77, // <Statement> ::= <Declarations>
            RULE_STATEMENT_TK_ASSIGN = 78, // <Statement> ::= <Value> 'tk_Assign' <Expression>
            RULE_STATEMENT_TK_IF_TK_THEN_TK_END_ALL = 79, // <Statement> ::= 'tk_if' <Expression> 'tk_then' <Statements> 'tk_end_all'
            RULE_STATEMENT_TK_IF_TK_THEN_TK_ELSE_TK_END_ALL = 80, // <Statement> ::= 'tk_if' <Expression> 'tk_then' <Statements> 'tk_else' <Statements> 'tk_end_all'
            RULE_STATEMENT_TK_BEGIN_CYCLE_TK_RAZ_TK_END_CYCLE = 81, // <Statement> ::= 'tk_begin_cycle' <Expression> 'tk_raz' <Statements> 'tk_end_cycle'
            RULE_STATEMENT_TK_BEGIN_CYCLE_TK_FOR_TK_IDENTIFIER_TK_FROM_TK_TO_TK_END_CYCLE = 82, // <Statement> ::= 'tk_begin_cycle' 'tk_for' 'tk_Identifier' 'tk_from' <Expression> 'tk_to' <Expression> <Statements> 'tk_end_cycle'
            RULE_STATEMENT_TK_BEGIN_CYCLE_TK_WHILE_TK_END_CYCLE = 83, // <Statement> ::= 'tk_begin_cycle' 'tk_while' <Expression> <Statements> 'tk_end_cycle'
            RULE_STATEMENT_TK_CASE_TK_END_ALL = 84, // <Statement> ::= 'tk_case' <Separators Opt> <Case_variant_list> 'tk_end_all'
            RULE_STATEMENT_TK_CASE_TK_ELSE_TK_END_ALL = 85, // <Statement> ::= 'tk_case' <Separators Opt> <Case_variant_list> 'tk_else' <Statements> 'tk_end_all'
            RULE_STATEMENT_TK_ASSERT = 86, // <Statement> ::= 'tk_assert' <Expression>
            RULE_STATEMENT_TK_READ = 87, // <Statement> ::= 'tk_read' <Id_list1>
            RULE_STATEMENT_TK_WRITE = 88, // <Statement> ::= 'tk_write' <List_of_expressions>
            RULE_STATEMENT_TK_IDENTIFIER_TK_ROUNDOPEN_TK_ROUNDCLOSE = 89, // <Statement> ::= 'tk_Identifier' 'tk_RoundOpen' <Fact_list> 'tk_RoundClose'
            RULE_STATEMENT_TK_IDENTIFIER = 90, // <Statement> ::= 'tk_Identifier'
            RULE_EXPRESSION_TK_GREATER = 91, // <Expression> ::= <Expression> 'tk_Greater' <Add Exp>
            RULE_EXPRESSION_TK_LOWER = 92, // <Expression> ::= <Expression> 'tk_Lower' <Add Exp>
            RULE_EXPRESSION_TK_LOWEREQUAL = 93, // <Expression> ::= <Expression> 'tk_LowerEqual' <Add Exp>
            RULE_EXPRESSION_TK_GREATEREQUAL = 94, // <Expression> ::= <Expression> 'tk_GreaterEqual' <Add Exp>
            RULE_EXPRESSION_TK_EQUAL = 95, // <Expression> ::= <Expression> 'tk_Equal' <Add Exp>
            RULE_EXPRESSION_TK_NOTEQUAL = 96, // <Expression> ::= <Expression> 'tk_NotEqual' <Add Exp>
            RULE_EXPRESSION = 97, // <Expression> ::= <Add Exp>
            RULE_ADDEXP_TK_OR = 98, // <Add Exp> ::= <Add Exp> 'tk_or' <Mult Exp>
            RULE_ADDEXP_TK_PLUS = 99, // <Add Exp> ::= <Add Exp> 'tk_Plus' <Mult Exp>
            RULE_ADDEXP_TK_MINUS = 100, // <Add Exp> ::= <Add Exp> 'tk_Minus' <Mult Exp>
            RULE_ADDEXP = 101, // <Add Exp> ::= <Mult Exp>
            RULE_MULTEXP_TK_AND = 102, // <Mult Exp> ::= <Mult Exp> 'tk_and' <Power Exp>
            RULE_MULTEXP_TK_MULT = 103, // <Mult Exp> ::= <Mult Exp> 'tk_Mult' <Power Exp>
            RULE_MULTEXP_TK_DIV = 104, // <Mult Exp> ::= <Mult Exp> 'tk_Div' <Power Exp>
            RULE_MULTEXP = 105, // <Mult Exp> ::= <Power Exp>
            RULE_POWEREXP_TK_POWER = 106, // <Power Exp> ::= <Negate Exp> 'tk_Power' <Power Exp>
            RULE_POWEREXP = 107, // <Power Exp> ::= <Negate Exp>
            RULE_NEGATEEXP_TK_MINUS = 108, // <Negate Exp> ::= 'tk_Minus' <Value>
            RULE_NEGATEEXP_TK_NOT = 109, // <Negate Exp> ::= 'tk_not' <Value>
            RULE_NEGATEEXP = 110, // <Negate Exp> ::= <Value>
            RULE_VALUE_TK_IDENTIFIER = 111, // <Value> ::= 'tk_Identifier'
            RULE_VALUE_TK_FUNC_VAL = 112, // <Value> ::= 'tk_func_val'
            RULE_VALUE_TK_INTEGER = 113, // <Value> ::= 'tk_integer'
            RULE_VALUE_TK_REAL = 114, // <Value> ::= 'tk_real'
            RULE_VALUE_TK_TRUE = 115, // <Value> ::= 'tk_true'
            RULE_VALUE_TK_FALSE = 116, // <Value> ::= 'tk_false'
            RULE_VALUE_TK_STRINGLITERAL = 117, // <Value> ::= 'tk_StringLiteral'
            RULE_VALUE_TK_CHAR = 118, // <Value> ::= 'tk_char'
            RULE_VALUE_TK_EOL = 119, // <Value> ::= 'tk_eol'
            RULE_VALUE_TK_IDENTIFIER_TK_SQUAREOPEN_TK_SQUARECLOSE_TK_SQUAREOPEN_TK_SQUARECLOSE = 120, // <Value> ::= 'tk_Identifier' 'tk_SquareOpen' <Fact_list> 'tk_SquareClose' 'tk_SquareOpen' <Expression> 'tk_SquareClose'
            RULE_VALUE_TK_IDENTIFIER_TK_SQUAREOPEN_TK_SQUARECLOSE = 121, // <Value> ::= 'tk_Identifier' 'tk_SquareOpen' <Fact_list> 'tk_SquareClose'
            RULE_VALUE_TK_IDENTIFIER_TK_ROUNDOPEN_TK_ROUNDCLOSE = 122, // <Value> ::= 'tk_Identifier' 'tk_RoundOpen' <Fact_list> 'tk_RoundClose'
            RULE_VALUE_TK_ROUNDOPEN_TK_ROUNDCLOSE = 123  // <Value> ::= 'tk_RoundOpen' <Expression> 'tk_RoundClose'
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
                case (int)SymbolConstants.SYMBOL_EOF:
                    //(EOF)
                    return null;

                case (int)SymbolConstants.SYMBOL_ERROR:
                    //(Error)
                    return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE:
                    //(Whitespace)
                    return null;

                case (int)SymbolConstants.SYMBOL_COMMENTLINE:
                    //(Comment Line)
                    return null;

                case (int)SymbolConstants.SYMBOL_TK_ALG:
                    //'tk_alg'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_AND:
                    //'tk_and'
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.LogicalAND);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_ARG:
                    //'tk_arg'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_ARRAY:
                    //'tk_array'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_ASSERT:
                    //'tk_assert'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_ASSIGN:
                    //'tk_Assign'
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.Assignment);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_BEGIN:
                    //'tk_begin'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_BEGIN_CYCLE:
                    //'tk_begin_cycle'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_BOOLEAN_TYPE:
                    //'tk_boolean_type'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_CASE:
                    //'tk_case'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_CASE_V:
                    //'tk_case_v'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_CHAR:
                    //'tk_char'
                    return parsertools.create_char_const(this);

                case (int)SymbolConstants.SYMBOL_TK_CHAR_TYPE:
                    //'tk_char_type'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_COLON:
                    //'tk_Colon'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_COMMA:
                    //'tk_Comma'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_DIV:
                    //'tk_Div'
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.Division);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_DOT:
                    //'tk_Dot'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_ELSE:
                    //'tk_else'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_END:
                    //'tk_end'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_END_ALL:
                    //'tk_end_all'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_END_CYCLE:
                    //'tk_end_cycle'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_EOL:
                    //'tk_eol'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_EQUAL:
                    //'tk_Equal'
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.Equal);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_FALSE:
                    //'tk_false'
                    bool_const _bool_const = new bool_const(false);
                    _bool_const.source_context = parsertools.GetTokenSourceContext();
                    return _bool_const;

                case (int)SymbolConstants.SYMBOL_TK_FOR:
                    //'tk_for'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_FROM:
                    //'tk_from'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_FUNC_VAL:
                    //'tk_func_val'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_GREATER:
                    //'tk_Greater'
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.Greater);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_GREATEREQUAL:
                    //'tk_GreaterEqual'
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.GreaterEqual);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_IDENTIFIER:
                    //'tk_Identifier'
                    return parsertools.create_ident(this);

                case (int)SymbolConstants.SYMBOL_TK_IF:
                    //'tk_if'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_INTEGER:
                    //'tk_integer'
                    return parsertools.create_int_const(this);

                case (int)SymbolConstants.SYMBOL_TK_INTEGER_TYPE:
                    //'tk_integer_type'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_ISP:
                    //'tk_isp'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_LOWER:
                    //'tk_Lower'
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.Less);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_LOWEREQUAL:
                    //'tk_LowerEqual'
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.LessEqual);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_MINUS:
                    //'tk_Minus'
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.Minus);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_MULT:
                    //'tk_Mult'
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.Multiplication);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_NEWLINE:
                    //'tk_NewLine'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_NOT:
                    //'tk_not'
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.LogicalNOT);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_NOTEQUAL:
                    //'tk_NotEqual'
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.NotEqual);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_OR:
                    //'tk_or'
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.LogicalOR);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_PLUS:
                    //'tk_Plus'
                    {
                        op_type_node _op_type_node = new op_type_node(Operators.Plus);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_POWER:
                    //'tk_Power'
                    {   // make it!
                        op_type_node _op_type_node = new op_type_node(Operators.Undefined);
                        _op_type_node.source_context = parsertools.GetTokenSourceContext();

                        return _op_type_node;
                    }

                case (int)SymbolConstants.SYMBOL_TK_RAZ:
                    //'tk_raz'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_READ:
                    //'tk_read'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_REAL:
                    //'tk_real'
                    return parsertools.create_double_const(this);

                case (int)SymbolConstants.SYMBOL_TK_REAL_TYPE:
                    //'tk_real_type'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_ROUNDCLOSE:
                    //'tk_RoundClose'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_ROUNDOPEN:
                    //'tk_RoundOpen'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_SEMICOLON:
                    //'tk_SemiColon'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_SQUARECLOSE:
                    //'tk_SquareClose'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_SQUAREOPEN:
                    //'tk_SquareOpen'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_STRING_TYPE:
                    //'tk_string_type'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_STRINGLITERAL:
                    //'tk_StringLiteral'
                    return parsertools.create_string_const(this);

                case (int)SymbolConstants.SYMBOL_TK_THEN:
                    //'tk_then'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_TO:
                    //'tk_to'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_TRUE:
                    //'tk_true'
                    bool_const _true_const = new bool_const(true);
                    _true_const.source_context = parsertools.GetTokenSourceContext();
                    return _true_const;
                case (int)SymbolConstants.SYMBOL_TK_USES:
                    //'tk_uses'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_VAR:
                    //'tk_var'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_WHILE:
                    //'tk_while'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_TK_WRITE:
                    //'tk_write'
                    {
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }

                case (int)SymbolConstants.SYMBOL_ADDEXP:
                    //<Add Exp>
                    //TERMINAL:Add Exp
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_ARRAY_LIST1:
                    //<Array_list1>
                    //TERMINAL:Array_list1
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_ARRAY_LIST2:
                    //<Array_list2>
                    //TERMINAL:Array_list2
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_CASE_VARIANT:
                    //<Case_variant>
                    //TERMINAL:Case_variant
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_CASE_VARIANT_LIST:
                    //<Case_variant_list>
                    //TERMINAL:Case_variant_list
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_DECLARATIONS:
                    //<Declarations>
                    //TERMINAL:Declarations
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_DIAP:
                    //<Diap>
                    //TERMINAL:Diap
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_DIAP_LIST:
                    //<Diap_list>
                    //TERMINAL:Diap_list
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_EXPRESSION:
                    //<Expression>
                    //TERMINAL:Expression
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_FACT_LIST:
                    //<Fact_list>
                    //TERMINAL:Fact_list
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_FORMAL_LIST:
                    //<Formal_list>
                    //TERMINAL:Formal_list
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_FORMAL_PARAMETER1:
                    //<Formal_parameter1>
                    //TERMINAL:Formal_parameter1
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_FORMAL_PARAMETER2:
                    //<Formal_Parameter2>
                    //TERMINAL:Formal_Parameter2
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_FORMAL_TYPE_LIST1:
                    //<Formal_type_list1>
                    //TERMINAL:Formal_type_list1
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_FORMAL_TYPE_LIST2:
                    //<Formal_type_list2>
                    //TERMINAL:Formal_type_list2
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_FREE_OPERATOR:
                    //<Free_operator>
                    //TERMINAL:Free_operator
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_FUNCTION:
                    //<Function>
                    //TERMINAL:Function
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_GLOBAL_DECL_LIST:
                    //<Global_decl_list>
                    //TERMINAL:Global_decl_list
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_GLOBAL_PART:
                    //<Global_part>
                    //TERMINAL:Global_part
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_GLOBAL_VARS:
                    //<Global_vars>
                    //TERMINAL:Global_vars
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_ID_LIST1:
                    //<Id_list1>
                    //TERMINAL:Id_list1
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_ID_LIST2:
                    //<Id_list2>
                    //TERMINAL:Id_list2
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_INITIALIZATION:
                    //<Initialization>
                    //TERMINAL:Initialization
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_LIST_OF_EXPRESSIONS:
                    //<List_of_expressions>
                    //TERMINAL:List_of_expressions
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_MULTEXP:
                    //<Mult Exp>
                    //TERMINAL:Mult Exp
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_NEGATEEXP:
                    //<Negate Exp>
                    //TERMINAL:Negate Exp
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_POWEREXP:
                    //<Power Exp>
                    //TERMINAL:Power Exp
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_PROCEDURE:
                    //<Procedure>
                    //TERMINAL:Procedure
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_PROGRAM:
                    //<Program>
                    //TERMINAL:Program
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_SEPARATOR:
                    //<Separator>
                    //TERMINAL:Separator
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_SEPARATORS:
                    //<Separators>
                    //TERMINAL:Separators
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_SEPARATORSOPT:
                    //<Separators Opt>
                    //TERMINAL:Separators Opt
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_STATEMENT:
                    //<Statement>
                    //TERMINAL:Statement
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_STATEMENTS:
                    //<Statements>
                    //TERMINAL:Statements
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_SUB_DECLARATIONS:
                    //<Sub_declarations>
                    //TERMINAL:Sub_declarations
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_TYPE:
                    //<Type>
                    //TERMINAL:Type
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_USES_UNITS:
                    //<Uses_units>
                    //TERMINAL:Uses_units
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_VALUE:
                    //<Value>
                    //TERMINAL:Value
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_VAR_DECL_LIST1:
                    //<Var_decl_list1>
                    //TERMINAL:Var_decl_list1
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_VAR_DECL_LIST2:
                    //<Var_decl_list2>
                    //TERMINAL:Var_decl_list2
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_VAR_DECLARATIONS1:
                    //<Var_declarations1>
                    //TERMINAL:Var_declarations1
                    return null;
                //ENDTERMINAL

                case (int)SymbolConstants.SYMBOL_VAR_DECLARATIONS2:
                    //<Var_declarations2>
                    //TERMINAL:Var_declarations2
                    return null;
                //ENDTERMINAL

            }
            throw new SymbolException("Unknown symbol");
        }



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

        public formal_parameters GetFormals(object _object)
        {
            if (_object == null)
                return null;
            formal_parameters _formal_parametres;
            if (_object is formal_parameters)
                _formal_parametres = _object as formal_parameters;
            else
            {
                _formal_parametres = new formal_parameters();
                _formal_parametres.params_list.Add(_object as typed_parameters);
            }
            return _formal_parametres;
        }

        public named_type_reference GetType(string name)
        {   //case ?             
            named_type_reference _named_type_reference = new named_type_reference();
            if (name == StringResources.Get("tk_integer_type")) _named_type_reference.names.Add(new ident("integer"));
            if (name == StringResources.Get("tk_real_type")) _named_type_reference.names.Add(new ident("real"));
            if (name == StringResources.Get("tk_boolean_type")) _named_type_reference.names.Add(new ident("boolean"));
            if (name == StringResources.Get("tk_char_type")) _named_type_reference.names.Add(new ident("char"));
            if (name == StringResources.Get("tk_string_type")) _named_type_reference.names.Add(new ident("string"));
            return _named_type_reference;
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
                case (int)RuleConstants.RULE_SEPARATOR_TK_SEMICOLON:
                    //<Separator> ::= 'tk_SemiColon'
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_SEPARATOR_TK_NEWLINE:
                    //<Separator> ::= 'tk_NewLine'
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_SEPARATORS:
                    //<Separators> ::= <Separators> <Separator>
                    return LRParser.GetReductionSyntaxNode(1);

                case (int)RuleConstants.RULE_SEPARATORS2:
                    //<Separators> ::= <Separator>
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_SEPARATORSOPT:
                    //<Separators Opt> ::= <Separator> <Separators Opt>
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_SEPARATORSOPT2:
                    //<Separators Opt> ::= 
                    return null;

                case (int)RuleConstants.RULE_PROGRAM_TK_ALG_TK_IDENTIFIER_TK_BEGIN_TK_END:
                    //<Program> ::= <Separators Opt> <Global_part> 'tk_alg' 'tk_Identifier' <Separators> 'tk_begin' <Statements> 'tk_end' <Sub_declarations>
                    {
                        program_module _program_module;
                        block _block;
                        statement_list _statement_list = GetStatements(LRParser.GetReductionSyntaxNode(6));
                        program_name _program_name = new program_name((ident)LRParser.GetReductionSyntaxNode(3));

                        if ((_units[this.unit_number - 1] as unit_data).initialization.subnodes.Count != 0)
                            _statement_list.subnodes.InsertRange(0, (_units[this.unit_number - 1] as unit_data).initialization.subnodes);

                        if ((_units[this.unit_number - 1] as unit_data).sub_progs.defs.Count != 0)
                            _block = new block((_units[this.unit_number - 1] as unit_data).sub_progs, _statement_list);
                        else
                            _block = new block(null, _statement_list);

                        (_units[this.unit_number - 1] as unit_data).used_units.idents.Add(new ident("MathForKumir"));

                        if ((_units[this.unit_number - 1] as unit_data).used_units.idents.Count != 0)
                        {
                            unit_or_namespace _unit_or_namespace;
                            uses_list _uses_list = new uses_list();

                            for (int i = 0; i < (_units[this.unit_number - 1] as unit_data).used_units.idents.Count; i++)
                            {
                                ident_list _ident_list = new ident_list();
                                _ident_list.idents.Add((_units[this.unit_number - 1] as unit_data).used_units.idents[i]);
                                _unit_or_namespace = new unit_or_namespace(_ident_list);
                                _uses_list.units.Add(_unit_or_namespace);
                            }
                            _program_module = new program_module(_program_name, _uses_list, _block, null);

                        }
                        else
                            _program_module = new program_module(_program_name, null, _block, null);
                        declarations _declarations = new declarations();
                        _program_module.Language = LanguageId.PascalABCNET;
                        parsertools.create_source_context(_program_module, parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(3)), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(8), LRParser.GetReductionSyntaxNode(7), LRParser.GetReductionSyntaxNode(6)));
                        parsertools.create_source_context(_block, parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(2)), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(8), LRParser.GetReductionSyntaxNode(7)));
                        parsertools.create_source_context(_program_name, LRParser.GetReductionSyntaxNode(3), LRParser.GetReductionSyntaxNode(3));
                        return _program_module;
                    }

                case (int)RuleConstants.RULE_PROGRAM_TK_ISP_TK_IDENTIFIER_TK_END:
                    //<Program> ::= <Separators Opt> 'tk_isp' 'tk_Identifier' <Separators> <Global_part> <Sub_declarations> 'tk_end'
                    {
                        interface_node _interface_node;
                        (_units[this.unit_number - 1] as unit_data).used_units.idents.Add(new ident("MathForKumir"));
                        if ((_units[this.unit_number - 1] as unit_data).used_units.idents.Count > 0)
                        {
                            unit_or_namespace _unit_or_namespace;
                            uses_list _uses_list = new uses_list();
                            for (int i = 0; i < (_units[this.unit_number - 1] as unit_data).used_units.idents.Count; i++)
                            {
                                ident_list _ident_list = new ident_list();
                                _ident_list.idents.Add((_units[this.unit_number - 1] as unit_data).used_units.idents[i]);
                                _unit_or_namespace = new unit_or_namespace(_ident_list);
                                _uses_list.units.Add(_unit_or_namespace);
                            }
                            _interface_node = new interface_node((_units[this.unit_number - 1] as unit_data).sub_progs, _uses_list, null);
                        }
                        else
                            _interface_node = new interface_node((_units[this.unit_number - 1] as unit_data).sub_progs, null, null);
                        unit_module _unit_module = new unit_module(new unit_name((ident)LRParser.GetReductionSyntaxNode(2), 0), _interface_node, null, (_units[this.unit_number - 1] as unit_data).initialization, null);
                        _unit_module.Language = LanguageId.PascalABCNET;
                        unit_name _unit_name = new unit_name((ident)LRParser.GetReductionSyntaxNode(2), 0);
                        parsertools.create_source_context(_unit_module, parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(3)), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(6), LRParser.GetReductionSyntaxNode(5)));
                        parsertools.create_source_context(_unit_name, LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(2));

                        return _unit_module;
                    }

                case (int)RuleConstants.RULE_PROCEDURE_TK_ALG_TK_IDENTIFIER_TK_ROUNDOPEN_TK_ROUNDCLOSE_TK_BEGIN_TK_END:
                    //<Procedure> ::= 'tk_alg' 'tk_Identifier' 'tk_RoundOpen' <Formal_list> 'tk_RoundClose' <Separators> 'tk_begin' <Statements> 'tk_end'
                    {
                        method_name _method_name = new method_name(null, (ident)LRParser.GetReductionSyntaxNode(1), null);
                        procedure_header _procedure_header = new procedure_header(GetFormals(LRParser.GetReductionSyntaxNode(3)), null, _method_name, false, false, null,null);
                        statement_list _statement_list = GetStatements(LRParser.GetReductionSyntaxNode(7));
                        block _block = new block(null, _statement_list);

                        procedure_definition _procedure_definition = new procedure_definition(_procedure_header, _block);
                        parsertools.create_source_context(_method_name, LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_procedure_header, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(4));
                        parsertools.create_source_context(_procedure_definition, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(8));
                        parsertools.create_source_context(_block, LRParser.GetReductionSyntaxNode(6), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(8), LRParser.GetReductionSyntaxNode(7)));
                        (_units[this.unit_number - 1] as unit_data).sub_progs.defs.Add(_procedure_definition);

                        return _procedure_definition;
                    }

                case (int)RuleConstants.RULE_PROCEDURE_TK_ALG_TK_IDENTIFIER_TK_BEGIN_TK_END:
                    //<Procedure> ::= 'tk_alg' 'tk_Identifier' <Separators> 'tk_begin' <Statements> 'tk_end'
                    {
                        method_name _method_name = new method_name(null, (ident)LRParser.GetReductionSyntaxNode(1), null);
                        procedure_header _procedure_header = new procedure_header(null, null, _method_name, false, false, null,null);
                        statement_list _statement_list = GetStatements(LRParser.GetReductionSyntaxNode(4));
                        block _block = new block(null, _statement_list);

                        procedure_definition _procedure_definition = new procedure_definition(_procedure_header, _block);

                        parsertools.create_source_context(_method_name, LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_procedure_header, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_procedure_definition, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(5));
                        parsertools.create_source_context(_block, LRParser.GetReductionSyntaxNode(3), LRParser.GetReductionSyntaxNode(5));
                        (_units[this.unit_number - 1] as unit_data).sub_progs.defs.Add(_procedure_definition);
                        declarations _declarations = new declarations();
                        _declarations.defs.Add(_procedure_definition);
                        parsertools.create_source_context(_declarations, parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1)), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(5), LRParser.GetReductionSyntaxNode(4)));

                        return _procedure_definition;
                    }

                case (int)RuleConstants.RULE_FUNCTION_TK_ALG_TK_IDENTIFIER_TK_ROUNDOPEN_TK_ROUNDCLOSE_TK_BEGIN_TK_END:
                    //<Function> ::= 'tk_alg' <Type> 'tk_Identifier' 'tk_RoundOpen' <Formal_list> 'tk_RoundClose' <Separators> 'tk_begin' <Statements> 'tk_end'
                    {
                        method_name _method_name = new method_name(null, LRParser.GetReductionSyntaxNode(2) as ident, null);
                        named_type_reference _named_type_reference = GetType(((token_info)LRParser.GetReductionSyntaxNode(1)).text.ToLower());

                        function_header _function_header = new function_header(_named_type_reference);
                        _function_header.of_object = false;
                        _function_header.name = _method_name;
                        _function_header.parameters = GetFormals(LRParser.GetReductionSyntaxNode(4));

                        statement_list _statement_list = GetStatements(LRParser.GetReductionSyntaxNode(8));
                        block _block = new block(null, _statement_list);

                        procedure_definition _procedure_definition = new procedure_definition(_function_header, _block);
                        parsertools.create_source_context(_method_name, LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_function_header, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(5));
                        parsertools.create_source_context(_procedure_definition, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(9));
                        parsertools.create_source_context(_block, LRParser.GetReductionSyntaxNode(7), LRParser.GetReductionSyntaxNode(9));
                        (_units[this.unit_number - 1] as unit_data).sub_progs.defs.Add(_procedure_definition);
                        declarations _declarations = new declarations();
                        _declarations.defs.Add(_procedure_definition);
                        parsertools.create_source_context(_declarations, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(9));
                        return _procedure_definition;
                    }

                case (int)RuleConstants.RULE_FUNCTION_TK_ALG_TK_IDENTIFIER_TK_BEGIN_TK_END:
                    //<Function> ::= 'tk_alg' <Type> 'tk_Identifier' <Separators> 'tk_begin' <Statements> 'tk_end'
                    {
                        method_name _method_name = new method_name(null, LRParser.GetReductionSyntaxNode(2) as ident, null);
                        named_type_reference _named_type_reference = GetType(((token_info)LRParser.GetReductionSyntaxNode(1)).text.ToLower());

                        function_header _function_header = new function_header(_named_type_reference);
                        _function_header.of_object = false;
                        _function_header.name = _method_name;

                        statement_list _statement_list = GetStatements(LRParser.GetReductionSyntaxNode(5));
                        block _block = new block(null, _statement_list);

                        procedure_definition _procedure_definition = new procedure_definition(_function_header, _block);
                        parsertools.create_source_context(_method_name, LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_function_header, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_procedure_definition, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(6));
                        parsertools.create_source_context(_block, LRParser.GetReductionSyntaxNode(4), LRParser.GetReductionSyntaxNode(6));
                        (_units[this.unit_number - 1] as unit_data).sub_progs.defs.Add(_procedure_definition);
                        return _procedure_definition;
                    }

                case (int)RuleConstants.RULE_SUB_DECLARATIONS:
                    //<Sub_declarations> ::= 
                    return null;

                case (int)RuleConstants.RULE_SUB_DECLARATIONS2:
                    //<Sub_declarations> ::= <Sub_declarations> <Procedure>
                    {
                        declarations _declarations;
                        if (LRParser.GetReductionSyntaxNode(0) != null)
                        {
                            _declarations = (declarations)LRParser.GetReductionSyntaxNode(0);
                            _declarations.defs.Add((declaration)LRParser.GetReductionSyntaxNode(1));
                        }
                        else
                        {
                            _declarations = new declarations();
                            _declarations.defs.Add((declaration)LRParser.GetReductionSyntaxNode(1));
                        }
                        // 
                        parsertools.create_source_context(_declarations, parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1)), LRParser.GetReductionSyntaxNode(1));
                        return _declarations;
                    }

                case (int)RuleConstants.RULE_SUB_DECLARATIONS3:
                    //<Sub_declarations> ::= <Sub_declarations> <Function>
                    {
                        declarations _declarations;
                        if (LRParser.GetReductionSyntaxNode(0) != null)
                        {
                            _declarations = (declarations)LRParser.GetReductionSyntaxNode(0);
                            _declarations.defs.Add((declaration)LRParser.GetReductionSyntaxNode(1));
                        }
                        else
                        {
                            _declarations = new declarations();
                            _declarations.defs.Add((declaration)LRParser.GetReductionSyntaxNode(1));
                        }
                        parsertools.create_source_context(_declarations, parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1)), LRParser.GetReductionSyntaxNode(1));
                        return _declarations;
                    }

                case (int)RuleConstants.RULE_SUB_DECLARATIONS4:
                    //<Sub_declarations> ::= <Sub_declarations> <Separators>
                    {
                        declarations _declarations;
                        if (LRParser.GetReductionSyntaxNode(0) != null)
                        {
                            _declarations = (declarations)LRParser.GetReductionSyntaxNode(0);
                        }
                        else
                        {
                            _declarations = new declarations();

                        }
                        parsertools.create_source_context(_declarations, parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1)), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(0)));
                        return _declarations;
                    }


                case (int)RuleConstants.RULE_TYPE_TK_INTEGER_TYPE:
                    //<Type> ::= 'tk_integer_type'
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_TYPE_TK_REAL_TYPE:
                    //<Type> ::= 'tk_real_type'
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_TYPE_TK_BOOLEAN_TYPE:
                    //<Type> ::= 'tk_boolean_type'
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_TYPE_TK_CHAR_TYPE:
                    //<Type> ::= 'tk_char_type'
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_TYPE_TK_STRING_TYPE:
                    //<Type> ::= 'tk_string_type'
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_DIAP_TK_COLON:
                    //<Diap> ::= <Expression> 'tk_Colon' <Expression>
                    {
                        diapason _diapason = new diapason(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression);

                        parsertools.create_source_context(_diapason, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        return _diapason;
                    }

                case (int)RuleConstants.RULE_DIAP_LIST_TK_COMMA:
                    //<Diap_list> ::= <Diap_list> 'tk_Comma' <Diap>
                    {
                        indexers_types _indexers_types = GetIndexers(LRParser.GetReductionSyntaxNode(0));
                        _indexers_types.indexers.Add(LRParser.GetReductionSyntaxNode(2) as diapason);

                        parsertools.create_source_context(_indexers_types, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        return _indexers_types;
                    }

                case (int)RuleConstants.RULE_DIAP_LIST:
                    //<Diap_list> ::= <Diap>
                    {
                        indexers_types _indexers_types = new indexers_types();
                        _indexers_types.indexers.Add(LRParser.GetReductionSyntaxNode(0) as diapason);

                        parsertools.create_source_context(_indexers_types, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        return _indexers_types;
                    }

                case (int)RuleConstants.RULE_LIST_OF_EXPRESSIONS_TK_COMMA:
                    //<List_of_expressions> ::= <List_of_expressions> 'tk_Comma' <Expression>
                    {
                        expression_list _expression_list = GetExpressions(LRParser.GetReductionSyntaxNode(0));
                        _expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(2) as expression);

                        parsertools.create_source_context(_expression_list, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        return _expression_list;
                    }

                case (int)RuleConstants.RULE_LIST_OF_EXPRESSIONS:
                    //<List_of_expressions> ::= <Expression>
                    {
                        expression_list _expression_list = new expression_list();
                        _expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(0) as expression);

                        _expression_list.source_context = ((expression)LRParser.GetReductionSyntaxNode(0)).source_context;
                        return _expression_list;
                    }

                case (int)RuleConstants.RULE_ID_LIST1_TK_IDENTIFIER:
                    //<Id_list1> ::= 'tk_Identifier'
                    {
                        ident_list _ident_list = new ident_list();
                        _ident_list.idents.Add(LRParser.GetReductionSyntaxNode(0) as ident);

                        _ident_list.source_context = ((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
                        return _ident_list;
                    }

                case (int)RuleConstants.RULE_ID_LIST1_TK_IDENTIFIER_TK_COMMA:
                    //<Id_list1> ::= 'tk_Identifier' 'tk_Comma' <Id_list1>
                    {
                        ident_list _ident_list = GetIdents(LRParser.GetReductionSyntaxNode(2));
                        _ident_list.idents.Add(LRParser.GetReductionSyntaxNode(0) as ident);

                        parsertools.create_source_context(_ident_list, LRParser.GetReductionSyntaxNode(0), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(0)));
                        return _ident_list;
                    }

                case (int)RuleConstants.RULE_ARRAY_LIST1_TK_IDENTIFIER_TK_SQUAREOPEN_TK_SQUARECLOSE:
                    //<Array_list1> ::= 'tk_Identifier' 'tk_SquareOpen' <Diap_list> 'tk_SquareClose'
                    {
                        ident_list _ident_list = new ident_list();
                        _ident_list.idents.Add(LRParser.GetReductionSyntaxNode(0) as ident);

                        indexers_types _indexers_types = GetIndexers(LRParser.GetReductionSyntaxNode(2));
                        array_type _array_type = new array_type(_indexers_types, null);

                        var_def_statement _var_def_statement = new var_def_statement(_ident_list, _array_type, null, definition_attribute.None, false);
                        variable_definitions _variable_definitions = new variable_definitions();
                        _variable_definitions.var_definitions.Add(_var_def_statement);

                        //_ident_list.source_context = ((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
                        parsertools.create_source_context(_array_type, LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(3));
                        //parsertools.create_source_context(LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(3));
                        parsertools.create_source_context(_ident_list, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        parsertools.create_source_context(_indexers_types, LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_variable_definitions, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(3));
                        parsertools.create_source_context(_var_def_statement, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(3));
                        return _variable_definitions;
                    }

                case (int)RuleConstants.RULE_ARRAY_LIST1_TK_IDENTIFIER_TK_SQUAREOPEN_TK_SQUARECLOSE_TK_COMMA:
                    //<Array_list1> ::= 'tk_Identifier' 'tk_SquareOpen' <Diap_list> 'tk_SquareClose' 'tk_Comma' <Array_list1>
                    {
                        ident_list _ident_list = new ident_list();
                        _ident_list.idents.Add(LRParser.GetReductionSyntaxNode(0) as ident);

                        indexers_types _indexers_types = GetIndexers(LRParser.GetReductionSyntaxNode(2));
                        array_type _array_type = new array_type(_indexers_types, null);

                        var_def_statement _var_def_statement = new var_def_statement(_ident_list, _array_type, null, definition_attribute.None, false);
                        variable_definitions _variable_definitions = (LRParser.GetReductionSyntaxNode(5) as variable_definitions);
                        _variable_definitions.var_definitions.Add(_var_def_statement);

                        //_ident_list.source_context = ((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
                        parsertools.create_source_context(_array_type, LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(3));
                        parsertools.create_source_context(_ident_list, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        //parsertools.create_source_context(LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(3));
                        parsertools.create_source_context(_indexers_types, LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_variable_definitions, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(5));
                        parsertools.create_source_context(_var_def_statement, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(3));
                        return _variable_definitions;
                    }

                case (int)RuleConstants.RULE_ID_LIST2_TK_IDENTIFIER_TK_COMMA:
                    //<Id_list2> ::= 'tk_Identifier' 'tk_Comma'
                    {
                        ident_list _ident_list = new ident_list();
                        _ident_list.idents.Add(LRParser.GetReductionSyntaxNode(0) as ident);

                        _ident_list.source_context = ((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
                        return _ident_list;
                    }

                case (int)RuleConstants.RULE_ID_LIST2_TK_IDENTIFIER_TK_COMMA2:
                    //<Id_list2> ::= 'tk_Identifier' 'tk_Comma' <Id_list2>
                    {
                        ident_list _ident_list = GetIdents(LRParser.GetReductionSyntaxNode(2));
                        _ident_list.idents.Add(LRParser.GetReductionSyntaxNode(0) as ident);

                        parsertools.create_source_context(_ident_list, LRParser.GetReductionSyntaxNode(0), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(0)));
                        return _ident_list;
                    }

                case (int)RuleConstants.RULE_ARRAY_LIST2_TK_IDENTIFIER_TK_SQUAREOPEN_TK_SQUARECLOSE_TK_COMMA:
                    //<Array_list2> ::= 'tk_Identifier' 'tk_SquareOpen' <Diap_list> 'tk_SquareClose' 'tk_Comma'
                    {
                        ident_list _ident_list = new ident_list();
                        _ident_list.idents.Add(LRParser.GetReductionSyntaxNode(0) as ident);

                        indexers_types _indexers_types = GetIndexers(LRParser.GetReductionSyntaxNode(2));
                        array_type _array_type = new array_type(_indexers_types, null);

                        var_def_statement _var_def_statement = new var_def_statement(_ident_list, _array_type, null, definition_attribute.None, false);
                        variable_definitions _variable_definitions = new variable_definitions();
                        _variable_definitions.var_definitions.Add(_var_def_statement);

                        //_ident_list.source_context = ((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
                        parsertools.create_source_context(_array_type, LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(3));
                        parsertools.create_source_context(_ident_list, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        //parsertools.create_source_context(LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(3));
                        parsertools.create_source_context(_indexers_types, LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(2));

                        parsertools.create_source_context(_variable_definitions, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(3));
                        parsertools.create_source_context(_var_def_statement, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(3));
                        return _variable_definitions;
                    }

                case (int)RuleConstants.RULE_ARRAY_LIST2_TK_IDENTIFIER_TK_SQUAREOPEN_TK_SQUARECLOSE_TK_COMMA2:
                    //<Array_list2> ::= 'tk_Identifier' 'tk_SquareOpen' <Diap_list> 'tk_SquareClose' 'tk_Comma' <Array_list2>
                    {
                        ident_list _ident_list = new ident_list();
                        _ident_list.idents.Add(LRParser.GetReductionSyntaxNode(0) as ident);

                        indexers_types _indexers_types = GetIndexers(LRParser.GetReductionSyntaxNode(2));
                        array_type _array_type = new array_type(_indexers_types, null);

                        var_def_statement _var_def_statement = new var_def_statement(_ident_list, _array_type, null, definition_attribute.None, false);
                        variable_definitions _variable_definitions = (LRParser.GetReductionSyntaxNode(5) as variable_definitions);
                        _variable_definitions.var_definitions.Add(_var_def_statement);

                        //_ident_list.source_context = ((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
                        parsertools.create_source_context(_array_type, LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(3));
                        parsertools.create_source_context(_ident_list, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        //parsertools.create_source_context(LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(3));
                        parsertools.create_source_context(_indexers_types, LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(2));

                        parsertools.create_source_context(_variable_definitions, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(5));
                        parsertools.create_source_context(_var_def_statement, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(3));
                        return _variable_definitions;
                    }

                case (int)RuleConstants.RULE_VAR_DECLARATIONS1:
                    //<Var_declarations1> ::= <Type> <Id_list1>
                    {
                        named_type_reference _named_type_reference = GetType(((token_info)LRParser.GetReductionSyntaxNode(0)).text.ToLower());

                        var_def_statement _var_def_statement = new var_def_statement(GetIdents(LRParser.GetReductionSyntaxNode(1)), _named_type_reference, null, definition_attribute.None, false);
                        var_statement _var_statement = new var_statement(_var_def_statement);

                        parsertools.create_source_context(_var_def_statement, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_var_statement, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_named_type_reference, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        return _var_statement;
                    }

                case (int)RuleConstants.RULE_VAR_DECLARATIONS1_TK_ARRAY:
                    //<Var_declarations1> ::= <Type> 'tk_array' <Array_list1>
                    {
                        named_type_reference _named_type_reference = GetType(((token_info)LRParser.GetReductionSyntaxNode(0)).text.ToLower());

                        statement_list var_statement_list = new statement_list();

                        variable_definitions _variable_definitions = LRParser.GetReductionSyntaxNode(2) as variable_definitions;
                        for (int i = 0; i < _variable_definitions.var_definitions.Count; i++)
                        {
                            ((_variable_definitions.var_definitions[i]).vars_type as array_type).elemets_types = _named_type_reference;
                            var_statement _var_statement = new var_statement((var_def_statement)_variable_definitions.var_definitions[i]);
                            var_statement_list.subnodes.Add(_var_statement);

                            parsertools.create_source_context(_var_statement, _variable_definitions.var_definitions[i], _variable_definitions.var_definitions[i]);

                        }
                        parsertools.create_source_context(_named_type_reference.names[0], LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        parsertools.create_source_context(_variable_definitions, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(var_statement_list, LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_named_type_reference, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        return var_statement_list;
                    }

                case (int)RuleConstants.RULE_VAR_DECLARATIONS2:
                    //<Var_declarations2> ::= <Type> <Id_list2>
                    {
                        named_type_reference _named_type_reference = GetType(((token_info)LRParser.GetReductionSyntaxNode(0)).text.ToLower());
                        var_def_statement _var_def_statement = new var_def_statement(GetIdents(LRParser.GetReductionSyntaxNode(1)), _named_type_reference, null, definition_attribute.None, false);
                        var_statement _var_statement = new var_statement(_var_def_statement);

                        parsertools.create_source_context(_var_def_statement, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_var_statement, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_named_type_reference, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        return _var_statement;
                    }

                case (int)RuleConstants.RULE_VAR_DECLARATIONS2_TK_ARRAY:
                    //<Var_declarations2> ::= <Type> 'tk_array' <Array_list2>
                    {
                        named_type_reference _named_type_reference = GetType(((token_info)LRParser.GetReductionSyntaxNode(0)).text.ToLower());

                        statement_list var_statement_list = new statement_list();

                        variable_definitions _variable_definitions = LRParser.GetReductionSyntaxNode(2) as variable_definitions;
                        for (int i = 0; i < _variable_definitions.var_definitions.Count; i++)
                        {
                            ((_variable_definitions.var_definitions[i]).vars_type as array_type).elemets_types = _named_type_reference;
                            var_statement _var_statement = new var_statement((var_def_statement)_variable_definitions.var_definitions[i]);
                            var_statement_list.subnodes.Add(_var_statement);

                            parsertools.create_source_context(_var_statement, _variable_definitions.var_definitions[i], _variable_definitions.var_definitions[i]);
                        }

                        parsertools.create_source_context(_named_type_reference.names[0], LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        parsertools.create_source_context(var_statement_list, LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_variable_definitions, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_named_type_reference, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));

                        return var_statement_list;
                    }

                case (int)RuleConstants.RULE_VAR_DECL_LIST1:
                    //<Var_decl_list1> ::= <Var_declarations1>
                    {
                        statement_list var_statement_list = GetStatements(LRParser.GetReductionSyntaxNode(0));

                        parsertools.create_source_context(var_statement_list, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        return var_statement_list;
                    }

                case (int)RuleConstants.RULE_VAR_DECL_LIST12:
                    //<Var_decl_list1> ::= <Var_decl_list2> <Var_declarations1>
                    {
                        statement_list var_statement_list = GetStatements(LRParser.GetReductionSyntaxNode(0));
                        var_statement_list.subnodes.AddRange(GetStatements(LRParser.GetReductionSyntaxNode(1)).subnodes);

                        parsertools.create_source_context(var_statement_list, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        return var_statement_list;
                    }

                case (int)RuleConstants.RULE_VAR_DECL_LIST2:
                    //<Var_decl_list2> ::= <Var_declarations2>
                    {
                        statement_list var_statement_list = GetStatements(LRParser.GetReductionSyntaxNode(0));

                        parsertools.create_source_context(var_statement_list, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        return var_statement_list;
                    }

                case (int)RuleConstants.RULE_VAR_DECL_LIST22:
                    //<Var_decl_list2> ::= <Var_decl_list2> <Var_declarations2>
                    {
                        statement_list var_statement_list = GetStatements(LRParser.GetReductionSyntaxNode(0));
                        var_statement_list.subnodes.AddRange(GetStatements(LRParser.GetReductionSyntaxNode(1)).subnodes);

                        parsertools.create_source_context(var_statement_list, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        return var_statement_list;
                    }


                case (int)RuleConstants.RULE_DECLARATIONS:
                    //<Declarations> ::= <Var_decl_list1>
                    return LRParser.GetReductionSyntaxNode(0);

                //------------------------- Globals

                case (int)RuleConstants.RULE_USES_UNITS_TK_USES:
                    //<Uses_units> ::= 'tk_uses' <Id_list1>
                    {
                        if (_units[unit_number - 1] != null)
                            (_units[unit_number - 1] as unit_data).used_units.idents.AddRange(((ident_list)LRParser.GetReductionSyntaxNode(1)).idents);
                        parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        return LRParser.GetReductionSyntaxNode(1);

                        //return null;
                    }

                case (int)RuleConstants.RULE_GLOBAL_DECL_LIST:
                    //<Global_decl_list> ::= <Global_decl_list> <Separators> <Declarations>
                    {
                        statement_list _statement_list = GetStatements(LRParser.GetReductionSyntaxNode(0));
                        _statement_list.subnodes.AddRange(GetStatements(LRParser.GetReductionSyntaxNode(2)).subnodes);
                        parsertools.create_source_context(_statement_list, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        return _statement_list;
                    }

                case (int)RuleConstants.RULE_GLOBAL_DECL_LIST2:
                    //<Global_decl_list> ::= <Declarations>
                    {   // etot kod pohoje inogda ne vipolniaetsia hotia pravilo srabativaet        
                        statement_list _statement_list = GetStatements(LRParser.GetReductionSyntaxNode(0));
                        for (int i = 0; i < _statement_list.subnodes.Count; i++)
                            (_units[this.unit_number - 1] as unit_data).sub_progs.defs.Add(_statement_list.subnodes[i] as declaration);
                        parsertools.create_source_context(_statement_list, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        return _statement_list;
                    }

                case (int)RuleConstants.RULE_INITIALIZATION_TK_ASSIGN:
                    //<Initialization> ::= <Initialization> <Separators> <Value> 'tk_Assign' <Expression>
                    {
                        assign _assign = new assign((addressed_value)LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(4) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(3)).type);
                        (_units[this.unit_number - 1] as unit_data).initialization.subnodes.Add(_assign);

                        parsertools.create_source_context(_assign, LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(4));
                        return null;
                    }

                case (int)RuleConstants.RULE_INITIALIZATION_TK_ASSIGN2:
                    //<Initialization> ::= <Value> 'tk_Assign' <Expression>
                    {
                        assign _assign = new assign((addressed_value)LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
                        (_units[this.unit_number - 1] as unit_data).initialization.subnodes.Add(_assign);

                        parsertools.create_source_context(_assign, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        return null;
                    }

                case (int)RuleConstants.RULE_GLOBAL_VARS:
                    //<Global_vars> ::= <Global_decl_list> <Separators> <Initialization> <Separators>
                    {
                        declarations _declarations = new declarations();
                        //_declarations.defs.Add((declaration)LRParser.GetReductionSyntaxNode(2));
                        statement_list _statement_list = GetStatements(LRParser.GetReductionSyntaxNode(0));
                        for (int i = 0; i < _statement_list.subnodes.Count; i++)
                        {
                            (_units[this.unit_number - 1] as unit_data).sub_progs.defs.Add(_statement_list.subnodes[i] as declaration);
                            _declarations.defs.Add(_statement_list.subnodes[i] as declaration);
                        }
                        parsertools.create_source_context(_declarations, parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(3)), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3), LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(0)));
                        return _declarations;
                    }

                case (int)RuleConstants.RULE_GLOBAL_VARS2:
                    //<Global_vars> ::= <Global_decl_list> <Separators>
                    {
                        //declarations _declarations = (declarations)LRParser.GetReductionSyntaxNode(0);
                        declarations _declarations = new declarations();
                        statement_list _statement_list = GetStatements(LRParser.GetReductionSyntaxNode(0));
                        for (int i = 0; i < _statement_list.subnodes.Count; i++)
                        {
                            (_units[this.unit_number - 1] as unit_data).sub_progs.defs.Add(_statement_list.subnodes[i] as declaration);
                            _declarations.defs.Add(_statement_list.subnodes[i] as declaration);
                        }
                        parsertools.create_source_context(_declarations, parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1)), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(0)));
                        return _declarations;
                    }

                case (int)RuleConstants.RULE_GLOBAL_PART:
                    //<Global_part> ::= 
                    return null;

                case (int)RuleConstants.RULE_GLOBAL_PART2:
                    //<Global_part> ::= <Uses_units> <Separators>
                    {
                        //declarations _declarations = (declarations)LRParser.GetReductionSyntaxNode(0);
                        //parsertools.create_source_context(_declarations, parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1)), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(0)));
                        return null;
                    }

                case (int)RuleConstants.RULE_GLOBAL_PART3:
                    //<Global_part> ::= <Uses_units> <Separators> <Global_vars>
                    {
                        //declarations _declarations = (declarations)LRParser.GetReductionSyntaxNode(0);
                        //_declarations.defs.AddRange(((declarations)LRParser.GetReductionSyntaxNode(2)).defs);
                        //parsertools.create_source_context(_declarations, parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(2)), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(0)));
                        return null;
                    }
                case (int)RuleConstants.RULE_GLOBAL_PART4:
                    //<Global_part> ::= <Global_vars>
                    {
                        //declarations _declarations = (declarations)LRParser.GetReductionSyntaxNode(0);
                        //parsertools.create_source_context(_declarations, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        return null;
                    }

                case (int)RuleConstants.RULE_FORMAL_PARAMETER1:
                    //<Formal_parameter1> ::= <Type> <Id_list1>
                    {
                        named_type_reference _named_type_reference = GetType(((token_info)LRParser.GetReductionSyntaxNode(0)).text.ToLower());
                        ident_list _ident_list = GetIdents(LRParser.GetReductionSyntaxNode(1));
                        _ident_list.idents.Reverse();

                        typed_parameters _typed_parametres = new typed_parameters(_ident_list, _named_type_reference, parametr_kind.none, null);
                        formal_parameters _formal_parametres = new formal_parameters();
                        _formal_parametres.params_list.Add(_typed_parametres);

                        parsertools.create_source_context(_typed_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_formal_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        return _formal_parametres;
                    }

                case (int)RuleConstants.RULE_FORMAL_PARAMETER1_TK_VAR:
                    //<Formal_parameter1> ::= 'tk_var' <Type> <Id_list1>
                    {
                        named_type_reference _named_type_reference = GetType(((token_info)LRParser.GetReductionSyntaxNode(1)).text.ToLower());
                        ident_list _ident_list = GetIdents(LRParser.GetReductionSyntaxNode(2));
                        _ident_list.idents.Reverse();

                        typed_parameters _typed_parametres = new typed_parameters(_ident_list, _named_type_reference, parametr_kind.var_parametr, null);
                        formal_parameters _formal_parametres = new formal_parameters();
                        _formal_parametres.params_list.Add(_typed_parametres);

                        parsertools.create_source_context(_typed_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_formal_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        return _formal_parametres;
                    }

                case (int)RuleConstants.RULE_FORMAL_PARAMETER1_TK_ARRAY:
                    //<Formal_parameter1> ::= <Type> 'tk_array' <Array_list1>
                    {
                        named_type_reference _named_type_reference = GetType(((token_info)LRParser.GetReductionSyntaxNode(0)).text.ToLower());
                        variable_definitions _variable_definitions = LRParser.GetReductionSyntaxNode(2) as variable_definitions;
                        formal_parameters _formal_parametres = new formal_parameters();
                        _variable_definitions.var_definitions.Reverse();

                        for (int i = 0; i < _variable_definitions.var_definitions.Count; i++)
                        {
                            ((_variable_definitions.var_definitions[i]).vars_type as array_type).elemets_types = _named_type_reference;
                            _formal_parametres.params_list.Add(new typed_parameters((_variable_definitions.var_definitions[i]).vars, (_variable_definitions.var_definitions[i]).vars_type, parametr_kind.none, null));
                        }
                        _variable_definitions.var_definitions.Clear();

                        parsertools.create_source_context(_formal_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_variable_definitions, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_named_type_reference, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        return _formal_parametres;
                    }

                case (int)RuleConstants.RULE_FORMAL_PARAMETER1_TK_VAR_TK_ARRAY:
                    //<Formal_parameter1> ::= 'tk_var' <Type> 'tk_array' <Array_list1>
                    {
                        named_type_reference _named_type_reference = GetType(((token_info)LRParser.GetReductionSyntaxNode(1)).text.ToLower());
                        variable_definitions _variable_definitions = LRParser.GetReductionSyntaxNode(3) as variable_definitions;
                        formal_parameters _formal_parametres = new formal_parameters();
                        _variable_definitions.var_definitions.Reverse();

                        for (int i = 0; i < _variable_definitions.var_definitions.Count; i++)
                        {
                            ((_variable_definitions.var_definitions[i]).vars_type as array_type).elemets_types = _named_type_reference;
                            _formal_parametres.params_list.Add(new typed_parameters((_variable_definitions.var_definitions[i]).vars, (_variable_definitions.var_definitions[i]).vars_type, parametr_kind.var_parametr, null));
                        }
                        _variable_definitions.var_definitions.Clear();

                        parsertools.create_source_context(_formal_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(3));
                        parsertools.create_source_context(_variable_definitions, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_named_type_reference, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(3));
                        return _formal_parametres;
                    }

                case (int)RuleConstants.RULE_FORMAL_PARAMETER2:
                    //<Formal_Parameter2> ::= <Type> <Id_list2>
                    {
                        named_type_reference _named_type_reference = GetType(((token_info)LRParser.GetReductionSyntaxNode(0)).text.ToLower());
                        ident_list _ident_list = GetIdents(LRParser.GetReductionSyntaxNode(1));
                        _ident_list.idents.Reverse();

                        typed_parameters _typed_parametres = new typed_parameters(_ident_list, _named_type_reference, parametr_kind.none, null);
                        formal_parameters _formal_parametres = new formal_parameters();
                        _formal_parametres.params_list.Add(_typed_parametres);

                        parsertools.create_source_context(_typed_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_formal_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        return _formal_parametres;
                    }

                case (int)RuleConstants.RULE_FORMAL_PARAMETER2_TK_VAR:
                    //<Formal_Parameter2> ::= 'tk_var' <Type> <Id_list2>
                    {
                        named_type_reference _named_type_reference = GetType(((token_info)LRParser.GetReductionSyntaxNode(1)).text.ToLower());
                        ident_list _ident_list = GetIdents(LRParser.GetReductionSyntaxNode(2));
                        _ident_list.idents.Reverse();

                        typed_parameters _typed_parametres = new typed_parameters(_ident_list, _named_type_reference, parametr_kind.var_parametr, null);
                        formal_parameters _formal_parametres = new formal_parameters();
                        _formal_parametres.params_list.Add(_typed_parametres);

                        parsertools.create_source_context(_typed_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_formal_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        return _formal_parametres;
                    }

                case (int)RuleConstants.RULE_FORMAL_PARAMETER2_TK_ARRAY:
                    //<Formal_Parameter2> ::= <Type> 'tk_array' <Array_list2>
                    {
                        named_type_reference _named_type_reference = GetType(((token_info)LRParser.GetReductionSyntaxNode(0)).text.ToLower());
                        variable_definitions _variable_definitions = LRParser.GetReductionSyntaxNode(2) as variable_definitions;
                        formal_parameters _formal_parametres = new formal_parameters();
                        _variable_definitions.var_definitions.Reverse();

                        for (int i = 0; i < _variable_definitions.var_definitions.Count; i++)
                        {
                            ((_variable_definitions.var_definitions[i]).vars_type as array_type).elemets_types = _named_type_reference;
                            _formal_parametres.params_list.Add(new typed_parameters((_variable_definitions.var_definitions[i]).vars, (_variable_definitions.var_definitions[i]).vars_type, parametr_kind.none, null));
                        }
                        _variable_definitions.var_definitions.Clear();

                        parsertools.create_source_context(_formal_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_variable_definitions, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        return _formal_parametres;
                    }

                case (int)RuleConstants.RULE_FORMAL_PARAMETER2_TK_VAR_TK_ARRAY:
                    //<Formal_Parameter2> ::= 'tk_var' <Type> 'tk_array' <Array_list2>
                    {
                        named_type_reference _named_type_reference = GetType(((token_info)LRParser.GetReductionSyntaxNode(1)).text.ToLower());
                        variable_definitions _variable_definitions = LRParser.GetReductionSyntaxNode(3) as variable_definitions;
                        formal_parameters _formal_parametres = new formal_parameters();
                        _variable_definitions.var_definitions.Reverse();

                        for (int i = 0; i < _variable_definitions.var_definitions.Count; i++)
                        {
                            ((_variable_definitions.var_definitions[i]).vars_type as array_type).elemets_types = _named_type_reference;
                            _formal_parametres.params_list.Add(new typed_parameters((_variable_definitions.var_definitions[i]).vars, (_variable_definitions.var_definitions[i]).vars_type, parametr_kind.var_parametr, null));
                        }
                        _variable_definitions.var_definitions.Clear();

                        parsertools.create_source_context(_variable_definitions, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(3));
                        parsertools.create_source_context(_formal_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(3));
                        return _formal_parametres;
                    }

                case (int)RuleConstants.RULE_FORMAL_TYPE_LIST1:
                    //<Formal_type_list1> ::= <Formal_parameter1>
                    {
                        formal_parameters _formal_parametres = GetFormals(LRParser.GetReductionSyntaxNode(0));

                        parsertools.create_source_context(_formal_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        return _formal_parametres;
                    }

                case (int)RuleConstants.RULE_FORMAL_TYPE_LIST12:
                    //<Formal_type_list1> ::= <Formal_type_list2> <Formal_parameter1>
                    {
                        formal_parameters _formal_parametres = GetFormals(LRParser.GetReductionSyntaxNode(0));
                        _formal_parametres.params_list.AddRange(GetFormals(LRParser.GetReductionSyntaxNode(1)).params_list);

                        parsertools.create_source_context(_formal_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        return _formal_parametres;
                    }

                case (int)RuleConstants.RULE_FORMAL_TYPE_LIST2:
                    //<Formal_type_list2> ::= <Formal_Parameter2>
                    {
                        formal_parameters _formal_parametres = GetFormals(LRParser.GetReductionSyntaxNode(0));

                        parsertools.create_source_context(_formal_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        return _formal_parametres;
                    }

                case (int)RuleConstants.RULE_FORMAL_TYPE_LIST22:
                    //<Formal_type_list2> ::= <Formal_type_list2> <Formal_Parameter2>
                    {
                        formal_parameters _formal_parametres = GetFormals(LRParser.GetReductionSyntaxNode(0));
                        _formal_parametres.params_list.AddRange(GetFormals(LRParser.GetReductionSyntaxNode(1)).params_list);

                        parsertools.create_source_context(_formal_parametres, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        return _formal_parametres;
                    }

                case (int)RuleConstants.RULE_FORMAL_LIST:
                    //<Formal_list> ::= 
                    return null;

                case (int)RuleConstants.RULE_FORMAL_LIST2:
                    //<Formal_list> ::= <Formal_type_list1>
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_FACT_LIST:
                    //<Fact_list> ::= 
                    return null;

                case (int)RuleConstants.RULE_FACT_LIST2:
                    //<Fact_list> ::= <List_of_expressions>
                    return LRParser.GetReductionSyntaxNode(0);

                //-------------------------- Operators

                case (int)RuleConstants.RULE_FREE_OPERATOR:
                    //<Free_operator> ::= 
                    return null;

                case (int)RuleConstants.RULE_CASE_VARIANT_LIST:
                    //<Case_variant_list> ::= <Case_variant> <Case_variant_list>
                    {
                        if_node _if_node1;
                        if (LRParser.GetReductionSyntaxNode(1) is case_variant)
                        {
                            case_variant _case_variant1 = (case_variant)LRParser.GetReductionSyntaxNode(1);
                            _if_node1 = new if_node((expression)_case_variant1.conditions.expressions[0], GetStatements(_case_variant1.exec_if_true), null);
                            parsertools.create_source_context(_if_node1, LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(1)); //make
                        
                        }
                        else
                            _if_node1 = LRParser.GetReductionSyntaxNode(1) as if_node;

                        case_variant _case_variant = (case_variant)LRParser.GetReductionSyntaxNode(0);
                        if_node _if_node = new if_node((expression)_case_variant.conditions.expressions[0], _case_variant.exec_if_true, _if_node1);

                        parsertools.create_source_context(_if_node, LRParser.GetReductionSyntaxNode(0), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(0))); //make
                        return _if_node;
                    }

                case (int)RuleConstants.RULE_CASE_VARIANT_LIST2:
                    //<Case_variant_list> ::= <Case_variant>
                    {   //unreacheable rule.. ?
                        case_variant _case_variant = (case_variant)LRParser.GetReductionSyntaxNode(0);
                        if_node _if_node = new if_node(GetExpressions(_case_variant.conditions), (statement)_case_variant.exec_if_true, null);

                        parsertools.create_source_context(_if_node, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0)); //make
                        return _if_node;
                    }

                case (int)RuleConstants.RULE_CASE_VARIANT_TK_CASE_V_TK_COLON:
                    //<Case_variant> ::= 'tk_case_v' <Expression> 'tk_Colon' <Statements>
                    {
                        expression_list _expression_list = GetExpressions(LRParser.GetReductionSyntaxNode(1));
                        case_variant _case_variant = new case_variant(_expression_list, GetStatements(LRParser.GetReductionSyntaxNode(3)));

                        //_case_variant.source_context = (GetExpressions(LRParser.GetReductionSyntaxNode(1))).source_context;
                        parsertools.create_source_context(_case_variant, LRParser.GetReductionSyntaxNode(0), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3), LRParser.GetReductionSyntaxNode(2)));
                        return _case_variant;
                    }

                case (int)RuleConstants.RULE_STATEMENTS:
                    //<Statements> ::= <Statements> <Separators> <Statement>
                    {
                        statement_list _statement_list = GetStatements(LRParser.GetReductionSyntaxNode(0));
                        _statement_list.subnodes.AddRange(GetStatements(LRParser.GetReductionSyntaxNode(2)).subnodes);

                        parsertools.create_source_context(_statement_list, parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2)), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(0)));
                        //parsertools.create_source_context(_statement_list, _statement_list, _statement_list);   //make
                        return _statement_list;
                    }

                case (int)RuleConstants.RULE_STATEMENTS2:
                    //<Statements> ::= <Statement>
                    {
                        statement_list _statement_list = GetStatements(LRParser.GetReductionSyntaxNode(0));

                        parsertools.create_source_context(_statement_list, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        return _statement_list;
                    }

                case (int)RuleConstants.RULE_STATEMENT:
                    //<Statement> ::= <Free_operator>
                    {
                        empty_statement _empty_statement = new empty_statement();

                        return _empty_statement;
                    }

                case (int)RuleConstants.RULE_STATEMENT2:
                    //<Statement> ::= <Declarations>
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_STATEMENT_TK_ASSIGN:
                    //<Statement> ::= <Value> 'tk_Assign' <Expression>
                    {
                        assign _assign = new assign((addressed_value)LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);

                        parsertools.create_source_context(_assign, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        return _assign;
                    }

                case (int)RuleConstants.RULE_STATEMENT_TK_IF_TK_THEN_TK_END_ALL:
                    //<Statement> ::= 'tk_if' <Expression> 'tk_then' <Statements> 'tk_end_all'
                    {
                        if_node _if_node = new if_node((expression)LRParser.GetReductionSyntaxNode(1), GetStatements(LRParser.GetReductionSyntaxNode(3)), null);

                        parsertools.create_source_context(_if_node, LRParser.GetReductionSyntaxNode(0), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(4), LRParser.GetReductionSyntaxNode(2)));
                        return _if_node;
                    }

                case (int)RuleConstants.RULE_STATEMENT_TK_IF_TK_THEN_TK_ELSE_TK_END_ALL:
                    //<Statement> ::= 'tk_if' <Expression> 'tk_then' <Statements> 'tk_else' <Statements> 'tk_end_all'
                    {
                        if_node _if_node = new if_node((expression)LRParser.GetReductionSyntaxNode(1), GetStatements(LRParser.GetReductionSyntaxNode(3)), GetStatements(LRParser.GetReductionSyntaxNode(5)));

                        parsertools.create_source_context(_if_node, LRParser.GetReductionSyntaxNode(0), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(6), LRParser.GetReductionSyntaxNode(2)));
                        return _if_node;
                    }

                case (int)RuleConstants.RULE_STATEMENT_TK_BEGIN_CYCLE_TK_RAZ_TK_END_CYCLE:
                    //<Statement> ::= 'tk_begin_cycle' <Expression> 'tk_raz' <Statements> 'tk_end_cycle'
                    {   // remake without "_system_loop_var_" !
                        int32_const _int32_const = new int32_const(1);
                        ident loop_var = new ident("&_system_loop_var");
                        statement_list _statement_list = GetStatements(LRParser.GetReductionSyntaxNode(3));

                        for_node _for_node = new for_node(loop_var, _int32_const, (expression)LRParser.GetReductionSyntaxNode(1), _statement_list, for_cycle_type.to, null, null, true);

                        parsertools.create_source_context(_for_node, LRParser.GetReductionSyntaxNode(0), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(4), LRParser.GetReductionSyntaxNode(1)));
                        if ((_statement_list as statement).source_context != null)
                            parsertools.create_source_context(_statement_list, LRParser.GetReductionSyntaxNode(3), LRParser.GetReductionSyntaxNode(3));
                        parsertools.create_source_context(_for_node, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(4));
                        return _for_node;
                    }

                case (int)RuleConstants.RULE_STATEMENT_TK_BEGIN_CYCLE_TK_FOR_TK_IDENTIFIER_TK_FROM_TK_TO_TK_END_CYCLE:
                    //<Statement> ::= 'tk_begin_cycle' 'tk_for' 'tk_Identifier' 'tk_from' <Expression> 'tk_to' <Expression> <Statements> 'tk_end_cycle'
                    {
                        for_node _for_node = new for_node((ident)LRParser.GetReductionSyntaxNode(2), (expression)LRParser.GetReductionSyntaxNode(4), (expression)LRParser.GetReductionSyntaxNode(6), GetStatements(LRParser.GetReductionSyntaxNode(7)), for_cycle_type.to, null, null, false);

                        parsertools.create_source_context(_for_node, LRParser.GetReductionSyntaxNode(0), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(8), LRParser.GetReductionSyntaxNode(1)));
                        parsertools.create_source_context(GetStatements(LRParser.GetReductionSyntaxNode(7)), LRParser.GetReductionSyntaxNode(7), LRParser.GetReductionSyntaxNode(7));
                        return _for_node;
                    }

                case (int)RuleConstants.RULE_STATEMENT_TK_BEGIN_CYCLE_TK_WHILE_TK_END_CYCLE:
                    //<Statement> ::= 'tk_begin_cycle' 'tk_while' <Expression> <Statements> 'tk_end_cycle'
                    {
                        while_node _while_node = new while_node((expression)LRParser.GetReductionSyntaxNode(2), GetStatements(LRParser.GetReductionSyntaxNode(3)), WhileCycleType.While);

                        parsertools.create_source_context(_while_node, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(4));
                        return _while_node;
                    }

                case (int)RuleConstants.RULE_STATEMENT_TK_CASE_TK_END_ALL:
                    //<Statement> ::= 'tk_case' <Separators Opt> <Case_variant_list> 'tk_end_all'
                    {
                        if_node _if_node;
                        if (LRParser.GetReductionSyntaxNode(2) is case_variant)
                        {
                            case_variant _case_variant = (case_variant)LRParser.GetReductionSyntaxNode(2);
                            _if_node = new if_node((expression)_case_variant.conditions.expressions[0], (statement)_case_variant.exec_if_true, null);

                            parsertools.create_source_context(_if_node, LRParser.GetReductionSyntaxNode(0), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3), LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(1)));   //make                       
                            return _if_node;
                        }
                        else{
                            if_node _if_node1;
                            _if_node = (if_node)LRParser.GetReductionSyntaxNode(2);
                            _if_node1 = _if_node;
                            while (_if_node1.else_body is if_node)
                                _if_node1 = _if_node1.else_body as if_node;

                            _if_node1.else_body = null;
                            parsertools.create_source_context(_if_node, LRParser.GetReductionSyntaxNode(0), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3), LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(1)));
                            return _if_node;

                        }
                    }

                case (int)RuleConstants.RULE_STATEMENT_TK_CASE_TK_ELSE_TK_END_ALL:
                    //<Statement> ::= 'tk_case' <Separators Opt> <Case_variant_list> 'tk_else' <Statements> 'tk_end_all'
                    {
                        if_node _if_node;
                        if (LRParser.GetReductionSyntaxNode(2) is case_variant)
                        {
                            case_variant _case_variant = (case_variant)LRParser.GetReductionSyntaxNode(2);
                            _if_node = new if_node((expression)_case_variant.conditions.expressions[0], (statement)_case_variant.exec_if_true, GetStatements(LRParser.GetReductionSyntaxNode(4)));

                            parsertools.create_source_context(_if_node, LRParser.GetReductionSyntaxNode(0), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(5), LRParser.GetReductionSyntaxNode(4), LRParser.GetReductionSyntaxNode(3), LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(1)));   //make                       
                            return _if_node;
                        }
                        else
                        {
                            if_node _if_node1;
                            _if_node = (if_node)LRParser.GetReductionSyntaxNode(2);
                            _if_node1 = _if_node;
                            while (_if_node1.else_body is if_node)
                               _if_node1 = _if_node1.else_body as if_node;
                                
                            
                            _if_node1.else_body = GetStatements(LRParser.GetReductionSyntaxNode(4));
                           // _if_node1.else_body.source_context.end_position
                            //parsertools.create_source_context(_if_node1, LRParser.GetReductionSyntaxNode(2), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(5), LRParser.GetReductionSyntaxNode(4), LRParser.GetReductionSyntaxNode(3)));   //make                       
                            parsertools.create_source_context(_if_node, LRParser.GetReductionSyntaxNode(0), parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(5), LRParser.GetReductionSyntaxNode(4), LRParser.GetReductionSyntaxNode(3), LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(1)));   //make                       
                            return _if_node;
                        }
                    }

                case (int)RuleConstants.RULE_STATEMENT_TK_ASSERT:
                    //<Statement> ::= 'tk_assert' <Expression>
                    {   //make it!
                        procedure_call _procedure_call = new procedure_call();
                        expression_list _expression_list = new expression_list();
                        _expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(1));
                        method_call _method_call = new method_call(_expression_list);
                        _method_call.dereferencing_value = new ident("assert");
                        _procedure_call.func_name = _method_call;

                        parsertools.create_source_context(_method_call.dereferencing_value, LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_expression_list, LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_method_call, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_procedure_call, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));

                        return _procedure_call;
                    }

                case (int)RuleConstants.RULE_STATEMENT_TK_READ:
                    //<Statement> ::= 'tk_read' <Id_list1>
                    {
                        procedure_call _procedure_call = new procedure_call();
                        expression_list _expression_list = new expression_list();
                        ident_list _ident_list = GetIdents(LRParser.GetReductionSyntaxNode(1));

                        for (int i = 0; i < _ident_list.idents.Count; i++)
                        {
                            _expression_list.expressions.Add(_ident_list.idents[i] as expression);
                        }
                        _ident_list.idents.Clear();

                        method_call _method_call = new method_call(_expression_list);
                        _method_call.dereferencing_value = new ident("read");
                        _procedure_call.func_name = _method_call;

                        parsertools.create_source_context(_method_call.dereferencing_value, LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_ident_list, LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_method_call, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_procedure_call, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        return _procedure_call;
                    }

                case (int)RuleConstants.RULE_STATEMENT_TK_WRITE:
                    //<Statement> ::= 'tk_write' <List_of_expressions>
                    {
                        procedure_call _procedure_call = new procedure_call();
                        expression_list _expression_list = GetExpressions(LRParser.GetReductionSyntaxNode(1));
                        method_call _method_call = new method_call(_expression_list);
                        _method_call.dereferencing_value = new ident("write");
                        _procedure_call.func_name = _method_call;

                        parsertools.create_source_context(_method_call.dereferencing_value, LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_expression_list, LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_method_call, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_procedure_call, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        return _procedure_call;
                    }

                case (int)RuleConstants.RULE_STATEMENT_TK_IDENTIFIER_TK_ROUNDOPEN_TK_ROUNDCLOSE:
                    //<Statement> ::= 'tk_Identifier' 'tk_RoundOpen' <Fact_list> 'tk_RoundClose'
                    {
                        procedure_call _procedure_call = new procedure_call();
                        method_call _method_call;
                        expression_list _expression_list;

                        if (LRParser.GetReductionSyntaxNode(2) != null)
                        {
                            _expression_list = GetExpressions(LRParser.GetReductionSyntaxNode(2));
                            _method_call = new method_call(_expression_list);
                        }
                        else
                            _method_call = new method_call();


                        _method_call.dereferencing_value = new ident(((ident)LRParser.GetReductionSyntaxNode(0)).name);
                        _procedure_call.func_name = _method_call;

                        parsertools.create_source_context(_method_call.dereferencing_value, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        //parsertools.create_source_context(_expression_list, LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_method_call, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(3));
                        parsertools.create_source_context(_procedure_call, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(3));
                        return _procedure_call;
                    }


                case (int)RuleConstants.RULE_STATEMENT_TK_IDENTIFIER:
                    //<Statement> ::= 'tk_Identifier'
                    {
                        procedure_call _procedure_call = new procedure_call();
                        method_call _method_call = new method_call(null);
                        _method_call.dereferencing_value = new ident(((ident)LRParser.GetReductionSyntaxNode(0)).name);
                        _procedure_call.func_name = _method_call;

                        parsertools.create_source_context(_method_call.dereferencing_value, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0)); parsertools.create_source_context(_method_call, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        parsertools.create_source_context(_procedure_call, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        return _procedure_call;
                    }

                case (int)RuleConstants.RULE_EXPRESSION_TK_GREATER:
                    //<Expression> ::= <Expression> 'tk_Greater' <Add Exp>
                    {
                        bin_expr _bin_expr = new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);

                        parsertools.create_source_context(_bin_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        return _bin_expr;
                    }

                case (int)RuleConstants.RULE_EXPRESSION_TK_LOWER:
                    //<Expression> ::= <Expression> 'tk_Lower' <Add Exp>
                    {
                        bin_expr _bin_expr = new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
                        parsertools.create_source_context(_bin_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));

                        return _bin_expr;
                    }

                case (int)RuleConstants.RULE_EXPRESSION_TK_LOWEREQUAL:
                    //<Expression> ::= <Expression> 'tk_LowerEqual' <Add Exp>
                    {
                        bin_expr _bin_expr = new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
                        parsertools.create_source_context(_bin_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));

                        return _bin_expr;
                    }

                case (int)RuleConstants.RULE_EXPRESSION_TK_GREATEREQUAL:
                    //<Expression> ::= <Expression> 'tk_GreaterEqual' <Add Exp>
                    {
                        bin_expr _bin_expr = new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
                        parsertools.create_source_context(_bin_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));

                        return _bin_expr;
                    }

                case (int)RuleConstants.RULE_EXPRESSION_TK_EQUAL:
                    //<Expression> ::= <Expression> 'tk_Equal' <Add Exp>
                    {
                        bin_expr _bin_expr = new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
                        parsertools.create_source_context(_bin_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));

                        return _bin_expr;
                    }

                case (int)RuleConstants.RULE_EXPRESSION_TK_NOTEQUAL:
                    //<Expression> ::= <Expression> 'tk_NotEqual' <Add Exp>
                    {
                        bin_expr _bin_expr = new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
                        parsertools.create_source_context(_bin_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));

                        return _bin_expr;
                    }

                case (int)RuleConstants.RULE_EXPRESSION:
                    //<Expression> ::= <Add Exp>
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_ADDEXP_TK_OR:
                    //<Add Exp> ::= <Add Exp> 'tk_or' <Mult Exp>
                    {
                        bin_expr _bin_expr = new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
                        parsertools.create_source_context(_bin_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));

                        return _bin_expr;
                    }

                case (int)RuleConstants.RULE_ADDEXP_TK_PLUS:
                    //<Add Exp> ::= <Add Exp> 'tk_Plus' <Mult Exp>
                    {
                        bin_expr _bin_expr = new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
                        parsertools.create_source_context(_bin_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));

                        return _bin_expr;
                    }

                case (int)RuleConstants.RULE_ADDEXP_TK_MINUS:
                    //<Add Exp> ::= <Add Exp> 'tk_Minus' <Mult Exp>
                    {
                        bin_expr _bin_expr = new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
                        parsertools.create_source_context(_bin_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));

                        return _bin_expr;
                    }

                case (int)RuleConstants.RULE_ADDEXP:
                    //<Add Exp> ::= <Mult Exp>
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_MULTEXP_TK_AND:
                    //<Mult Exp> ::= <Mult Exp> 'tk_and' <Power Exp>
                    {
                        bin_expr _bin_expr = new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
                        parsertools.create_source_context(_bin_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));

                        return _bin_expr;
                    }

                case (int)RuleConstants.RULE_MULTEXP_TK_MULT:
                    //<Mult Exp> ::= <Mult Exp> 'tk_Mult' <Power Exp>
                    {
                        bin_expr _bin_expr = new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
                        parsertools.create_source_context(_bin_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));

                        return _bin_expr;
                    }

                case (int)RuleConstants.RULE_MULTEXP_TK_DIV:
                    //<Mult Exp> ::= <Mult Exp> 'tk_Div' <Power Exp>
                    {
                        bin_expr _bin_expr = new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
                        parsertools.create_source_context(_bin_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));

                        return _bin_expr;
                    }

                case (int)RuleConstants.RULE_MULTEXP:
                    //<Mult Exp> ::= <Power Exp>
                    return LRParser.GetReductionSyntaxNode(0);    //make it!

                case (int)RuleConstants.RULE_POWEREXP_TK_POWER:
                    {
                        //<Power Exp> ::= <Negate Exp> 'tk_Power' <Power Exp>

                        expression_list _expression_list = new expression_list();
                        _expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(0) as expression);
                        _expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(2) as expression);
                        method_call _method_call = new method_call(_expression_list);
                        _method_call.dereferencing_value = new ident("Power");
                        parsertools.create_source_context(_method_call.dereferencing_value, LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_method_call, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));
                        return _method_call;

                        //bin_expr _bin_expr = new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression, LRParser.GetReductionSyntaxNode(2) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
                        //parsertools.create_source_context(_bin_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(2));

                        //return _bin_expr;
                    }
                case (int)RuleConstants.RULE_POWEREXP:
                    //<Power Exp> ::= <Negate Exp>
                    return LRParser.GetReductionSyntaxNode(0);    //make it!

                case (int)RuleConstants.RULE_NEGATEEXP_TK_MINUS:
                    //<Negate Exp> ::= 'tk_Minus' <Value>
                    {
                        un_expr _un_expr = new un_expr(LRParser.GetReductionSyntaxNode(1) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(0)).type);
                        parsertools.create_source_context(_un_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));

                        return _un_expr;
                    }

                case (int)RuleConstants.RULE_NEGATEEXP_TK_NOT:
                    //<Negate Exp> ::= 'tk_not' <Value>
                    {
                        un_expr _un_expr = new un_expr(LRParser.GetReductionSyntaxNode(1) as expression, ((op_type_node)LRParser.GetReductionSyntaxNode(0)).type);
                        parsertools.create_source_context(_un_expr, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));

                        return _un_expr;
                    }

                case (int)RuleConstants.RULE_NEGATEEXP:
                    //<Negate Exp> ::= <Value>
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_VALUE_TK_IDENTIFIER:
                    //<Value> ::= 'tk_Identifier'
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_VALUE_TK_FUNC_VAL:
                    //<Value> ::= 'tk_func_val'
                    return new ident("result");

                case (int)RuleConstants.RULE_VALUE_TK_INTEGER:
                    //<Value> ::= 'tk_integer'
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_VALUE_TK_REAL:
                    //<Value> ::= 'tk_real'
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_VALUE_TK_TRUE:
                    //<Value> ::= 'tk_true'
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_VALUE_TK_FALSE:
                    //<Value> ::= 'tk_false'
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_VALUE_TK_STRINGLITERAL:
                    //<Value> ::= 'tk_StringLiteral'
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_VALUE_TK_CHAR:
                    //<Value> ::= 'tk_char'
                    return LRParser.GetReductionSyntaxNode(0);

                case (int)RuleConstants.RULE_VALUE_TK_EOL:
                    //<Value> ::= 'tk_eol'
                    {
                        literal_const_line _literal_const_line = new literal_const_line();
                        sharp_char_const _sharp_char_const_13 = new sharp_char_const(13);
                        sharp_char_const _sharp_char_const_10 = new sharp_char_const(10);
                        _literal_const_line.literals.Add(_sharp_char_const_13);
                        _literal_const_line.literals.Add(_sharp_char_const_10);

                        parsertools.create_source_context(_sharp_char_const_13, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));    //make
                        parsertools.create_source_context(_sharp_char_const_10, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));    //make
                        parsertools.create_source_context(_literal_const_line, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));     //make
                        return _literal_const_line;
                    }

                case (int)RuleConstants.RULE_VALUE_TK_IDENTIFIER_TK_SQUAREOPEN_TK_SQUARECLOSE_TK_SQUAREOPEN_TK_SQUARECLOSE:
                    //<Value> ::= 'tk_Identifier' 'tk_SquareOpen' <Fact_list> 'tk_SquareClose' 'tk_SquareOpen' <Expression> 'tk_SquareClose'
                    {

                        indexer _indexer = new indexer(GetExpressions(LRParser.GetReductionSyntaxNode(2)));
                        indexer _indexer1 = new indexer(GetExpressions(LRParser.GetReductionSyntaxNode(5)));
                        _indexer.dereferencing_value = LRParser.GetReductionSyntaxNode(0) as ident;
                        _indexer1.dereferencing_value = _indexer;
                        parsertools.create_source_context(_indexer, LRParser.GetReductionSyntaxNode(2), LRParser.GetReductionSyntaxNode(2));
                        parsertools.create_source_context(_indexer, LRParser.GetReductionSyntaxNode(5), LRParser.GetReductionSyntaxNode(5));
                        return _indexer1;
                    }

                case (int)RuleConstants.RULE_VALUE_TK_IDENTIFIER_TK_SQUAREOPEN_TK_SQUARECLOSE:
                    //<Value> ::= 'tk_Identifier' 'tk_SquareOpen' <Fact_list> 'tk_SquareClose'
                    {

                        indexer _indexer = new indexer(GetExpressions(LRParser.GetReductionSyntaxNode(2)));
                        _indexer.dereferencing_value = LRParser.GetReductionSyntaxNode(0) as ident;
                        parsertools.create_source_context(_indexer, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(3));
                        return _indexer;
                    }

                case (int)RuleConstants.RULE_VALUE_TK_IDENTIFIER_TK_ROUNDOPEN_TK_ROUNDCLOSE:
                    //<Value> ::= 'tk_Identifier' 'tk_RoundOpen' <Fact_list> 'tk_RoundClose'
                    {
                        expression_list _expression_list;
                        method_call _method_call;
                        if (LRParser.GetReductionSyntaxNode(2) != null)
                        {
                            _expression_list = GetExpressions(LRParser.GetReductionSyntaxNode(2));
                            _method_call = new method_call(_expression_list);
                        }
                        else
                            _method_call = new method_call();
                        switch (((ident)LRParser.GetReductionSyntaxNode(0)).name)
                        {
                            case "tg": _method_call.dereferencing_value = new ident("tan"); break;
                            case "ctg": _method_call.dereferencing_value = new ident("ctg"); break;
                            case "arctg": _method_call.dereferencing_value = new ident("arctan"); break;
                            case "arcctg": _method_call.dereferencing_value = new ident("arcctg"); break;
                            case "lg": _method_call.dereferencing_value = new ident("log10"); break;
                            case "mod": _method_call.dereferencing_value = new ident("md"); break;
                            case "div": _method_call.dereferencing_value = new ident("dv"); break;
                            case "rnd": _method_call.dereferencing_value = new ident("random"); break;
                            case "int": _method_call.dereferencing_value = new ident("round"); break;

                            default:
                                _method_call.dereferencing_value = new ident(((ident)LRParser.GetReductionSyntaxNode(0)).name);
                                break;
                        }
                        //_method_call.dereferencing_value = LRParser.GetReductionSyntaxNode(0) as ident;

                        parsertools.create_source_context(_method_call.dereferencing_value, LRParser.GetReductionSyntaxNode(1), LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_method_call, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        return _method_call;
                    }

                case (int)RuleConstants.RULE_VALUE_TK_ROUNDOPEN_TK_ROUNDCLOSE:
                    //<Value> ::= 'tk_RoundOpen' <Expression> 'tk_RoundClose'
                    return LRParser.GetReductionSyntaxNode(1);

            }
            throw new RuleException("Unknown rule");
        }






    }
}
