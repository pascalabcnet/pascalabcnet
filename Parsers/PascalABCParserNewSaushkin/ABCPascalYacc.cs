// GPPG version 1.3.6
// Machine:  LAPTOP-TE3HP881
// DateTime: 14.12.2024 13:55:00
// UserName: miks
// Input file <D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y>

// options: lines gplex

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using QUT.Gppg;
using PascalABCCompiler.SyntaxTree;
using Languages.Pascal.Frontend.Errors;
using PascalABCCompiler.ParserTools;
using System.Linq;

namespace Languages.Pascal.Frontend.Core
{
public enum Tokens {
    error=1,EOF=2,tkDirectiveName=3,tkAmpersend=4,tkColon=5,tkDotDot=6,
    tkPoint=7,tkRoundOpen=8,tkRoundClose=9,tkSemiColon=10,tkSquareOpen=11,tkSquareClose=12,
    tkQuestion=13,tkUnderscore=14,tkQuestionPoint=15,tkDoubleQuestion=16,tkQuestionSquareOpen=17,tkBackSlashRoundOpen=18,
    tkAsync=19,tkAwait=20,tkSizeOf=21,tkTypeOf=22,tkWhere=23,tkArray=24,
    tkCase=25,tkClass=26,tkAuto=27,tkStatic=28,tkConst=29,tkConstructor=30,
    tkDestructor=31,tkElse=32,tkExcept=33,tkFile=34,tkFor=35,tkForeach=36,
    tkFunction=37,tkMatch=38,tkWhen=39,tkIf=40,tkImplementation=41,tkInherited=42,
    tkInterface=43,tkProcedure=44,tkOperator=45,tkProperty=46,tkRaise=47,tkRecord=48,
    tkSet=49,tkType=50,tkThen=51,tkUses=52,tkVar=53,tkWhile=54,
    tkWith=55,tkNil=56,tkGoto=57,tkOf=58,tkLabel=59,tkLock=60,
    tkProgram=61,tkEvent=62,tkDefault=63,tkTemplate=64,tkExports=65,tkResourceString=66,
    tkThreadvar=67,tkSealed=68,tkPartial=69,tkTo=70,tkDownto=71,tkLoop=72,
    tkSequence=73,tkYield=74,tkShortProgram=75,tkVertParen=76,tkShortSFProgram=77,tkNew=78,
    tkOn=79,tkName=80,tkPrivate=81,tkProtected=82,tkPublic=83,tkInternal=84,
    tkRead=85,tkWrite=86,tkIndex=87,tkParseModeExpression=88,tkParseModeStatement=89,tkParseModeType=90,
    tkBegin=91,tkEnd=92,tkAsmBody=93,tkILCode=94,tkError=95,INVISIBLE=96,
    tkRepeat=97,tkUntil=98,tkDo=99,tkComma=100,tkFinally=101,tkTry=102,
    tkInitialization=103,tkFinalization=104,tkUnit=105,tkLibrary=106,tkExternal=107,tkParams=108,
    tkNamespace=109,tkAssign=110,tkPlusEqual=111,tkMinusEqual=112,tkMultEqual=113,tkDivEqual=114,
    tkMinus=115,tkPlus=116,tkSlash=117,tkStar=118,tkStarStar=119,tkEqual=120,
    tkGreater=121,tkGreaterEqual=122,tkLower=123,tkLowerEqual=124,tkNotEqual=125,tkCSharpStyleOr=126,
    tkArrow=127,tkOr=128,tkXor=129,tkAnd=130,tkDiv=131,tkMod=132,
    tkShl=133,tkShr=134,tkNot=135,tkAs=136,tkIn=137,tkIs=138,
    tkImplicit=139,tkExplicit=140,tkAddressOf=141,tkDeref=142,tkIdentifier=143,tkStringLiteral=144,
    tkFormatStringLiteral=145,tkMultilineStringLiteral=146,tkAsciiChar=147,tkAbstract=148,tkForward=149,tkOverload=150,
    tkReintroduce=151,tkOverride=152,tkVirtual=153,tkExtensionMethod=154,tkInteger=155,tkBigInteger=156,
    tkFloat=157,tkHex=158,tkUnknown=159,tkStep=160};

// Abstract base class for GPLEX scanners
public abstract class ScanBase : AbstractScanner<PascalABCCompiler.ParserTools.Union,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

public partial class GPPGParser: ShiftReduceParser<PascalABCCompiler.ParserTools.Union, LexLocation>
{
  // Verbatim content from D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y
#line 6 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
// Э�?и об�?явления добавля�?�?ся в класс GPPGParser, п�?едс�?авля�?�?ий собой па�?се�?, гене�?и�?�?ем�?й сис�?емой gppg
#line 7 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"

#line 8 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    public syntax_tree_node root; // �?о�?невой �?зел син�?акси�?еского де�?ева 
#line 9 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"

#line 10 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    // private int maxErrors = 10;
#line 11 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    private PascalParserTools parserTools;
#line 12 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	private ParserLambdaHelper lambdaHelper = new ParserLambdaHelper();
#line 13 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	
#line 14 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    public GPPGParser(AbstractScanner<PascalABCCompiler.ParserTools.Union, LexLocation> scanner, PascalParserTools parserTools) : base(scanner) 
#line 15 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	{ 
#line 16 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		this.parserTools = parserTools;
#line 17 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	}
  // End verbatim content from D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y

#pragma warning disable 649
  private static Dictionary<int, string> aliasses;
#pragma warning restore 649
  private static Rule[] rules = new Rule[1031];
  private static State[] states = new State[1708];
  private static string[] nonTerms = new string[] {
      "parse_goal", "unit_key_word", "class_or_static", "assignment", "optional_array_initializer", 
      "attribute_declarations", "ot_visibility_specifier", "one_attribute", "attribute_variable", 
      "const_factor", "const_factor_without_unary_op", "const_variable_2", "const_term", 
      "const_variable", "literal_or_number", "unsigned_number", "variable_or_literal_or_number", 
      "program_block", "optional_var", "class_attribute", "class_attributes", 
      "class_attributes1", "lambda_unpacked_params_or_id", "lambda_list_of_unpacked_params_or_id", 
      "member_list_section", "optional_component_list_seq_end", "const_decl", 
      "only_const_decl", "const_decl_sect", "object_type", "record_type", "member_list", 
      "method_decl_list", "field_or_const_definition_list", "case_stmt", "case_list", 
      "program_decl_sect_list", "int_decl_sect_list1", "inclass_decl_sect_list1", 
      "interface_decl_sect_list", "decl_sect_list", "decl_sect_list1", "inclass_decl_sect_list", 
      "decl_sect_list_proc_func_only", "field_or_const_definition", "abc_decl_sect", 
      "decl_sect", "int_decl_sect", "type_decl", "simple_type_decl", "simple_field_or_const_definition", 
      "res_str_decl_sect", "method_decl_withattr", "method_or_property_decl", 
      "property_definition", "fp_sect", "default_expr", "tuple", "expr_as_stmt", 
      "exception_block", "external_block", "exception_handler", "exception_handler_list", 
      "exception_identifier", "typed_const_list1", "typed_const_list", "optional_expr_list", 
      "optional_expr_list_func_param", "elem_list", "optional_expr_list_with_bracket", 
      "expr_list", "expr_list_func_param", "const_elem_list1", "case_label_list", 
      "const_elem_list", "optional_const_func_expr_list", "elem_list1", "enumeration_id", 
      "expr_l1_or_unpacked_list", "enumeration_id_list", "const_simple_expr", 
      "term", "term1", "typed_const", "typed_const_plus", "typed_var_init_expression", 
      "expr", "expr_with_func_decl_lambda", "expr_with_func_decl_lambda_ass", 
      "const_expr", "const_relop_expr", "elem", "range_expr", "const_elem", "array_const", 
      "factor", "factor_without_unary_op", "relop_expr", "expr_dq", "lambda_unpacked_params", 
      "expr_l1", "expr_l1_or_unpacked", "expr_l1_func_decl_lambda", "expr_l1_for_lambda", 
      "simple_expr", "range_term", "range_factor", "external_directive_ident", 
      "init_const_expr", "case_label", "variable", "proc_func_call", "var_reference", 
      "optional_read_expr", "simple_expr_or_nothing", "var_question_point", "expr_l1_for_question_expr", 
      "expr_l1_for_new_question_expr", "for_cycle_type", "format_expr", "format_const_expr", 
      "const_expr_or_nothing", "foreach_stmt", "for_stmt", "loop_stmt", "yield_stmt", 
      "yield_sequence_stmt", "fp_list", "fp_sect_list", "file_type", "sequence_type", 
      "var_address", "goto_stmt", "func_name_ident", "param_name", "const_field_name", 
      "func_name_with_template_args", "identifier_or_keyword", "unit_name", "exception_variable", 
      "const_name", "func_meth_name_ident", "label_name", "type_decl_identifier", 
      "template_identifier_with_equal", "program_param", "identifier", "identifier_keyword_operatorname", 
      "func_class_name_ident", "visibility_specifier", "property_specifier_directives", 
      "non_reserved", "if_stmt", "initialization_part", "template_arguments", 
      "label_list", "ident_or_keyword_pointseparator_list", "ident_list", "param_name_list", 
      "inherited_message", "implementation_part", "interface_part", "abc_interface_part", 
      "simple_type_list", "literal", "one_literal", "literal_list", "label_decl_sect", 
      "lock_stmt", "func_name", "proc_name", "optional_proc_name", "new_expr", 
      "allowable_expr_as_stmt", "parts", "inclass_block", "block", "proc_func_external_block", 
      "exception_class_type_identifier", "simple_type_identifier", "base_class_name", 
      "base_classes_names_list", "optional_base_classes", "one_compiler_directive", 
      "optional_head_compiler_directives", "head_compiler_directives", "program_heading_2", 
      "optional_tk_point", "program_param_list", "optional_semicolon", "operator_name_ident", 
      "const_relop", "const_addop", "assign_operator", "const_mulop", "relop", 
      "addop", "mulop", "sign", "overload_operator", "typecast_op", "property_specifiers", 
      "write_property_specifiers", "read_property_specifiers", "array_defaultproperty", 
      "meth_modificators", "optional_method_modificators", "optional_method_modificators1", 
      "meth_modificator", "property_modificator", "optional_property_initialization", 
      "proc_call", "proc_func_constr_destr_decl", "proc_func_decl", "inclass_proc_func_decl", 
      "inclass_proc_func_decl_noclass", "constr_destr_decl", "inclass_constr_destr_decl", 
      "method_decl", "proc_func_constr_destr_decl_with_attr", "proc_func_decl_noclass", 
      "method_header", "proc_type_decl", "procedural_type_kind", "proc_header", 
      "procedural_type", "constr_destr_header", "proc_func_header", "func_header", 
      "method_procfunc_header", "int_func_header", "int_proc_header", "property_interface", 
      "program_file", "program_header", "parameter_decl", "parameter_decl_list", 
      "property_parameter_list", "const_set", "pascal_set_const", "question_expr", 
      "question_constexpr", "new_question_expr", "record_const", "const_field_list_1", 
      "const_field_list", "const_field", "repeat_stmt", "raise_stmt", "pointer_type", 
      "attribute_declaration", "one_or_some_attribute", "stmt_list", "else_case", 
      "exception_block_else_branch", "compound_stmt", "string_type", "sizeof_expr", 
      "simple_property_definition", "stmt_or_expression", "unlabelled_stmt", 
      "stmt", "case_item", "set_type", "as_is_expr", "as_is_constexpr", "is_type_expr", 
      "as_expr", "power_expr", "power_constexpr", "unsized_array_type", "simple_type_or_", 
      "simple_type", "simple_type_question", "optional_type_specification", "fptype", 
      "type_ref", "fptype_noproctype", "array_type", "template_param", "template_empty_param", 
      "structured_type", "empty_template_type_reference", "simple_or_template_type_reference", 
      "simple_or_template_or_question_type_reference", "type_ref_or_secific", 
      "type_decl_type", "type_ref_and_secific_list", "type_decl_sect", "try_handler", 
      "class_or_interface_keyword", "optional_tk_do", "keyword", "reserved_keyword", 
      "typeof_expr", "simple_fp_sect", "template_param_list", "template_empty_param_list", 
      "template_type_params", "template_type_empty_params", "template_type", 
      "try_stmt", "uses_clause", "used_units_list", "uses_clause_one", "uses_clause_one_or_empty", 
      "unit_file", "used_unit_name", "unit_header", "var_decl_sect", "var_decl", 
      "var_decl_part", "field_definition", "var_decl_with_assign_var_tuple", 
      "var_stmt", "where_part", "where_part_list", "optional_where_section", 
      "while_stmt", "with_stmt", "variable_as_type", "dotted_identifier", "func_decl_lambda", 
      "expl_func_decl_lambda", "lambda_type_ref", "lambda_type_ref_noproctype", 
      "full_lambda_fp_list", "lambda_simple_fp_sect", "lambda_function_body", 
      "lambda_procedure_body", "common_lambda_body", "optional_full_lambda_fp_list", 
      "field_in_unnamed_object", "list_fields_in_unnamed_object", "func_class_name_ident_list", 
      "rem_lambda", "variable_list", "var_ident_list", "tkAssignOrEqual", "const_pattern_expression", 
      "pattern", "deconstruction_or_const_pattern", "pattern_optional_var", "collection_pattern", 
      "tuple_pattern", "collection_pattern_list_item", "tuple_pattern_item", 
      "collection_pattern_var_item", "match_with", "pattern_case", "pattern_cases", 
      "pattern_out_param", "pattern_out_param_optional_var", "pattern_out_param_list", 
      "pattern_out_param_list_optional_var", "collection_pattern_expr_list", 
      "tuple_pattern_item_list", "const_pattern_expr_list", "var_with_init_for_expr_with_let", 
      "var_with_init_for_expr_with_let_list", "index_or_nothing", "$accept", 
      };

  static GPPGParser() {
    states[0] = new State(new int[]{61,1606,105,1673,106,1674,109,1675,88,1680,90,1685,89,1692,75,1697,77,1704,3,-27,52,-27,91,-27,59,-27,29,-27,66,-27,50,-27,53,-27,62,-27,11,-27,44,-27,37,-27,28,-27,26,-27,19,-27,30,-27,31,-27},new int[]{-1,1,-234,3,-235,4,-307,1618,-309,1619,-2,1668,-175,1679});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{3,1602,52,-14,91,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-185,5,-186,1600,-184,1605});
    states[5] = new State(-41,new int[]{-303,6});
    states[6] = new State(new int[]{52,1588,59,-67,29,-67,66,-67,50,-67,53,-67,62,-67,11,-67,44,-67,37,-67,28,-67,26,-67,19,-67,30,-67,31,-67,91,-67},new int[]{-18,7,-305,14,-37,15,-41,1519,-42,1520});
    states[7] = new State(new int[]{7,9,10,10,5,11,100,12,6,13,2,-26},new int[]{-188,8});
    states[8] = new State(-20);
    states[9] = new State(-21);
    states[10] = new State(-22);
    states[11] = new State(-23);
    states[12] = new State(-24);
    states[13] = new State(-25);
    states[14] = new State(-42);
    states[15] = new State(new int[]{91,17},new int[]{-256,16});
    states[16] = new State(-34);
    states[17] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,762,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494},new int[]{-253,18,-262,760,-261,22,-4,23,-113,24,-132,419,-111,429,-147,761,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895,-143,1050});
    states[18] = new State(new int[]{92,19,10,20});
    states[19] = new State(-530);
    states[20] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,762,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494},new int[]{-262,21,-261,22,-4,23,-113,24,-132,419,-111,429,-147,761,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895,-143,1050});
    states[21] = new State(-532);
    states[22] = new State(-492);
    states[23] = new State(-495);
    states[24] = new State(new int[]{110,169,111,170,112,171,113,172,114,173,92,-528,10,-528,98,-528,101,-528,33,-528,104,-528,2,-528,9,-528,100,-528,12,-528,99,-528,32,-528,85,-528,84,-528,83,-528,82,-528,81,-528,86,-528},new int[]{-194,25});
    states[25] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,673,8,674,21,323,22,328,76,552,40,620,5,629,18,693,37,702,44,706},new int[]{-88,26,-87,27,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-323,843,-100,683,-324,701});
    states[26] = new State(-522);
    states[27] = new State(new int[]{70,28,92,-598,10,-598,98,-598,101,-598,33,-598,104,-598,2,-598,9,-598,100,-598,12,-598,99,-598,32,-598,85,-598,84,-598,83,-598,82,-598,81,-598,86,-598});
    states[28] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,29,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[29] = new State(-607);
    states[30] = new State(new int[]{16,31,70,-608,92,-608,10,-608,98,-608,101,-608,33,-608,104,-608,2,-608,9,-608,100,-608,12,-608,99,-608,32,-608,85,-608,84,-608,83,-608,82,-608,81,-608,86,-608,6,-608,76,-608,5,-608,51,-608,58,-608,141,-608,143,-608,80,-608,78,-608,160,-608,87,-608,45,-608,42,-608,8,-608,21,-608,22,-608,144,-608,147,-608,145,-608,146,-608,155,-608,158,-608,157,-608,156,-608,57,-608,91,-608,40,-608,25,-608,97,-608,54,-608,35,-608,55,-608,102,-608,47,-608,36,-608,53,-608,60,-608,74,-608,72,-608,38,-608,71,-608,13,-611});
    states[31] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552},new int[]{-98,32,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610});
    states[32] = new State(new int[]{120,184,125,185,123,186,121,187,124,188,122,189,137,190,135,191,16,-621,70,-621,92,-621,10,-621,98,-621,101,-621,33,-621,104,-621,2,-621,9,-621,100,-621,12,-621,99,-621,32,-621,85,-621,84,-621,83,-621,82,-621,81,-621,86,-621,13,-621,6,-621,76,-621,5,-621,51,-621,58,-621,141,-621,143,-621,80,-621,78,-621,160,-621,87,-621,45,-621,42,-621,8,-621,21,-621,22,-621,144,-621,147,-621,145,-621,146,-621,155,-621,158,-621,157,-621,156,-621,57,-621,91,-621,40,-621,25,-621,97,-621,54,-621,35,-621,55,-621,102,-621,47,-621,36,-621,53,-621,60,-621,74,-621,72,-621,38,-621,71,-621,116,-621,115,-621,128,-621,129,-621,126,-621,138,-621,136,-621,118,-621,117,-621,131,-621,132,-621,133,-621,134,-621,130,-621},new int[]{-196,33});
    states[33] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-105,34,-243,1518,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,633,-268,610});
    states[34] = new State(new int[]{6,35,120,-646,125,-646,123,-646,121,-646,124,-646,122,-646,137,-646,135,-646,16,-646,70,-646,92,-646,10,-646,98,-646,101,-646,33,-646,104,-646,2,-646,9,-646,100,-646,12,-646,99,-646,32,-646,85,-646,84,-646,83,-646,82,-646,81,-646,86,-646,13,-646,76,-646,5,-646,51,-646,58,-646,141,-646,143,-646,80,-646,78,-646,160,-646,87,-646,45,-646,42,-646,8,-646,21,-646,22,-646,144,-646,147,-646,145,-646,146,-646,155,-646,158,-646,157,-646,156,-646,57,-646,91,-646,40,-646,25,-646,97,-646,54,-646,35,-646,55,-646,102,-646,47,-646,36,-646,53,-646,60,-646,74,-646,72,-646,38,-646,71,-646,116,-646,115,-646,128,-646,129,-646,126,-646,138,-646,136,-646,118,-646,117,-646,131,-646,132,-646,133,-646,134,-646,130,-646});
    states[35] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552},new int[]{-83,36,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,633,-268,610});
    states[36] = new State(new int[]{116,199,115,200,128,201,129,202,126,203,6,-725,5,-725,120,-725,125,-725,123,-725,121,-725,124,-725,122,-725,137,-725,135,-725,16,-725,70,-725,92,-725,10,-725,98,-725,101,-725,33,-725,104,-725,2,-725,9,-725,100,-725,12,-725,99,-725,32,-725,85,-725,84,-725,83,-725,82,-725,81,-725,86,-725,13,-725,76,-725,51,-725,58,-725,141,-725,143,-725,80,-725,78,-725,160,-725,87,-725,45,-725,42,-725,8,-725,21,-725,22,-725,144,-725,147,-725,145,-725,146,-725,155,-725,158,-725,157,-725,156,-725,57,-725,91,-725,40,-725,25,-725,97,-725,54,-725,35,-725,55,-725,102,-725,47,-725,36,-725,53,-725,60,-725,74,-725,72,-725,38,-725,71,-725,138,-725,136,-725,118,-725,117,-725,131,-725,132,-725,133,-725,134,-725,130,-725},new int[]{-197,37});
    states[37] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-82,38,-243,1517,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,633,-268,610});
    states[38] = new State(new int[]{138,205,136,1505,118,1508,117,1509,131,1510,132,1511,133,1512,134,1513,130,1514,116,-727,115,-727,128,-727,129,-727,126,-727,6,-727,5,-727,120,-727,125,-727,123,-727,121,-727,124,-727,122,-727,137,-727,135,-727,16,-727,70,-727,92,-727,10,-727,98,-727,101,-727,33,-727,104,-727,2,-727,9,-727,100,-727,12,-727,99,-727,32,-727,85,-727,84,-727,83,-727,82,-727,81,-727,86,-727,13,-727,76,-727,51,-727,58,-727,141,-727,143,-727,80,-727,78,-727,160,-727,87,-727,45,-727,42,-727,8,-727,21,-727,22,-727,144,-727,147,-727,145,-727,146,-727,155,-727,158,-727,157,-727,156,-727,57,-727,91,-727,40,-727,25,-727,97,-727,54,-727,35,-727,55,-727,102,-727,47,-727,36,-727,53,-727,60,-727,74,-727,72,-727,38,-727,71,-727},new int[]{-198,39});
    states[39] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-96,40,-269,1515,-243,1516,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-97,563});
    states[40] = new State(new int[]{7,41,8,175,138,-748,136,-748,118,-748,117,-748,131,-748,132,-748,133,-748,134,-748,130,-748,116,-748,115,-748,128,-748,129,-748,126,-748,6,-748,5,-748,120,-748,125,-748,123,-748,121,-748,124,-748,122,-748,137,-748,135,-748,16,-748,70,-748,92,-748,10,-748,98,-748,101,-748,33,-748,104,-748,2,-748,9,-748,100,-748,12,-748,99,-748,32,-748,85,-748,84,-748,83,-748,82,-748,81,-748,86,-748,13,-748,76,-748,51,-748,58,-748,141,-748,143,-748,80,-748,78,-748,160,-748,87,-748,45,-748,42,-748,21,-748,22,-748,144,-748,147,-748,145,-748,146,-748,155,-748,158,-748,157,-748,156,-748,57,-748,91,-748,40,-748,25,-748,97,-748,54,-748,35,-748,55,-748,102,-748,47,-748,36,-748,53,-748,60,-748,74,-748,72,-748,38,-748,71,-748});
    states[41] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,84,55,83,56,82,57,81,58,68,59,64,60,128,61,22,62,21,63,63,64,23,65,129,66,130,67,131,68,132,69,133,70,134,71,135,72,136,73,137,74,138,75,24,76,73,77,91,78,25,79,26,80,29,81,30,82,31,83,71,84,99,85,32,86,92,87,33,88,34,89,27,90,104,91,101,92,35,93,36,94,37,95,40,96,41,97,42,98,103,99,43,100,44,101,46,102,47,103,48,104,97,105,49,106,102,107,50,108,28,109,51,110,70,111,98,112,52,113,53,114,54,115,55,116,56,117,57,118,58,119,59,120,61,121,105,122,106,123,109,124,107,125,108,126,62,127,74,128,38,129,39,130,69,131,148,132,60,133,139,134,140,135,79,136,153,137,152,138,72,139,154,140,150,141,151,142,149,143,45,145},new int[]{-148,42,-147,43,-151,45,-152,48,-293,53,-150,54,-191,144});
    states[42] = new State(-767);
    states[43] = new State(-844);
    states[44] = new State(-836);
    states[45] = new State(-837);
    states[46] = new State(-857);
    states[47] = new State(-858);
    states[48] = new State(-838);
    states[49] = new State(-859);
    states[50] = new State(-860);
    states[51] = new State(-839);
    states[52] = new State(-840);
    states[53] = new State(-845);
    states[54] = new State(-865);
    states[55] = new State(-861);
    states[56] = new State(-862);
    states[57] = new State(-863);
    states[58] = new State(-864);
    states[59] = new State(-866);
    states[60] = new State(-867);
    states[61] = new State(-868);
    states[62] = new State(-869);
    states[63] = new State(-870);
    states[64] = new State(-871);
    states[65] = new State(-872);
    states[66] = new State(-873);
    states[67] = new State(-874);
    states[68] = new State(-875);
    states[69] = new State(-876);
    states[70] = new State(-877);
    states[71] = new State(-878);
    states[72] = new State(-879);
    states[73] = new State(-880);
    states[74] = new State(-881);
    states[75] = new State(-882);
    states[76] = new State(-883);
    states[77] = new State(-884);
    states[78] = new State(-885);
    states[79] = new State(-886);
    states[80] = new State(-887);
    states[81] = new State(-888);
    states[82] = new State(-889);
    states[83] = new State(-890);
    states[84] = new State(-891);
    states[85] = new State(-892);
    states[86] = new State(-893);
    states[87] = new State(-894);
    states[88] = new State(-895);
    states[89] = new State(-896);
    states[90] = new State(-897);
    states[91] = new State(-898);
    states[92] = new State(-899);
    states[93] = new State(-900);
    states[94] = new State(-901);
    states[95] = new State(-902);
    states[96] = new State(-903);
    states[97] = new State(-904);
    states[98] = new State(-905);
    states[99] = new State(-906);
    states[100] = new State(-907);
    states[101] = new State(-908);
    states[102] = new State(-909);
    states[103] = new State(-910);
    states[104] = new State(-911);
    states[105] = new State(-912);
    states[106] = new State(-913);
    states[107] = new State(-914);
    states[108] = new State(-915);
    states[109] = new State(-916);
    states[110] = new State(-917);
    states[111] = new State(-918);
    states[112] = new State(-919);
    states[113] = new State(-920);
    states[114] = new State(-921);
    states[115] = new State(-922);
    states[116] = new State(-923);
    states[117] = new State(-924);
    states[118] = new State(-925);
    states[119] = new State(-926);
    states[120] = new State(-927);
    states[121] = new State(-928);
    states[122] = new State(-929);
    states[123] = new State(-930);
    states[124] = new State(-931);
    states[125] = new State(-932);
    states[126] = new State(-933);
    states[127] = new State(-934);
    states[128] = new State(-935);
    states[129] = new State(-936);
    states[130] = new State(-937);
    states[131] = new State(-938);
    states[132] = new State(-939);
    states[133] = new State(-940);
    states[134] = new State(-941);
    states[135] = new State(-942);
    states[136] = new State(-943);
    states[137] = new State(-944);
    states[138] = new State(-945);
    states[139] = new State(-946);
    states[140] = new State(-947);
    states[141] = new State(-948);
    states[142] = new State(-949);
    states[143] = new State(-950);
    states[144] = new State(-846);
    states[145] = new State(new int[]{115,147,116,148,117,149,118,150,120,151,121,152,122,153,123,154,124,155,125,156,128,157,129,158,130,159,131,160,132,161,133,162,134,163,135,164,137,165,139,166,140,167,110,169,111,170,112,171,113,172,114,173,119,174},new int[]{-200,146,-194,168});
    states[146] = new State(-829);
    states[147] = new State(-952);
    states[148] = new State(-953);
    states[149] = new State(-954);
    states[150] = new State(-955);
    states[151] = new State(-956);
    states[152] = new State(-957);
    states[153] = new State(-958);
    states[154] = new State(-959);
    states[155] = new State(-960);
    states[156] = new State(-961);
    states[157] = new State(-962);
    states[158] = new State(-963);
    states[159] = new State(-964);
    states[160] = new State(-965);
    states[161] = new State(-966);
    states[162] = new State(-967);
    states[163] = new State(-968);
    states[164] = new State(-969);
    states[165] = new State(-970);
    states[166] = new State(-971);
    states[167] = new State(-972);
    states[168] = new State(-973);
    states[169] = new State(-975);
    states[170] = new State(-976);
    states[171] = new State(-977);
    states[172] = new State(-978);
    states[173] = new State(-979);
    states[174] = new State(-974);
    states[175] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,1477,8,674,21,323,22,328,76,552,40,620,5,629,18,693,37,702,44,706,9,-815},new int[]{-68,176,-72,178,-89,424,-87,181,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,1474,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-323,1478,-100,683,-324,701});
    states[176] = new State(new int[]{9,177});
    states[177] = new State(-768);
    states[178] = new State(new int[]{100,179,9,-814});
    states[179] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,1477,8,674,21,323,22,328,76,552,40,620,5,629,18,693,37,702,44,706},new int[]{-89,180,-87,181,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,1474,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-323,1478,-100,683,-324,701});
    states[180] = new State(-595);
    states[181] = new State(new int[]{70,28,100,-601,9,-601});
    states[182] = new State(-605);
    states[183] = new State(new int[]{120,184,125,185,123,186,121,187,124,188,122,189,137,190,135,191,16,-620,70,-620,92,-620,10,-620,98,-620,101,-620,33,-620,104,-620,2,-620,9,-620,100,-620,12,-620,99,-620,32,-620,85,-620,84,-620,83,-620,82,-620,81,-620,86,-620,13,-620,6,-620,76,-620,5,-620,51,-620,58,-620,141,-620,143,-620,80,-620,78,-620,160,-620,87,-620,45,-620,42,-620,8,-620,21,-620,22,-620,144,-620,147,-620,145,-620,146,-620,155,-620,158,-620,157,-620,156,-620,57,-620,91,-620,40,-620,25,-620,97,-620,54,-620,35,-620,55,-620,102,-620,47,-620,36,-620,53,-620,60,-620,74,-620,72,-620,38,-620,71,-620,116,-620,115,-620,128,-620,129,-620,126,-620,138,-620,136,-620,118,-620,117,-620,131,-620,132,-620,133,-620,134,-620,130,-620},new int[]{-196,33});
    states[184] = new State(-716);
    states[185] = new State(-717);
    states[186] = new State(-718);
    states[187] = new State(-719);
    states[188] = new State(-720);
    states[189] = new State(-721);
    states[190] = new State(-722);
    states[191] = new State(new int[]{137,192});
    states[192] = new State(-723);
    states[193] = new State(new int[]{6,35,5,194,120,-645,125,-645,123,-645,121,-645,124,-645,122,-645,137,-645,135,-645,16,-645,70,-645,92,-645,10,-645,98,-645,101,-645,33,-645,104,-645,2,-645,9,-645,100,-645,12,-645,99,-645,32,-645,85,-645,84,-645,83,-645,82,-645,81,-645,86,-645,13,-645,76,-645});
    states[194] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,5,-705,70,-705,92,-705,10,-705,98,-705,101,-705,33,-705,104,-705,2,-705,9,-705,100,-705,12,-705,99,-705,32,-705,84,-705,83,-705,82,-705,81,-705,6,-705},new int[]{-115,195,-105,634,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,633,-268,610});
    states[195] = new State(new int[]{5,196,70,-708,92,-708,10,-708,98,-708,101,-708,33,-708,104,-708,2,-708,9,-708,100,-708,12,-708,99,-708,32,-708,85,-708,84,-708,83,-708,82,-708,81,-708,86,-708,6,-708,76,-708});
    states[196] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552},new int[]{-105,197,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,633,-268,610});
    states[197] = new State(new int[]{6,35,70,-710,92,-710,10,-710,98,-710,101,-710,33,-710,104,-710,2,-710,9,-710,100,-710,12,-710,99,-710,32,-710,85,-710,84,-710,83,-710,82,-710,81,-710,86,-710,76,-710});
    states[198] = new State(new int[]{116,199,115,200,128,201,129,202,126,203,6,-724,5,-724,120,-724,125,-724,123,-724,121,-724,124,-724,122,-724,137,-724,135,-724,16,-724,70,-724,92,-724,10,-724,98,-724,101,-724,33,-724,104,-724,2,-724,9,-724,100,-724,12,-724,99,-724,32,-724,85,-724,84,-724,83,-724,82,-724,81,-724,86,-724,13,-724,76,-724,51,-724,58,-724,141,-724,143,-724,80,-724,78,-724,160,-724,87,-724,45,-724,42,-724,8,-724,21,-724,22,-724,144,-724,147,-724,145,-724,146,-724,155,-724,158,-724,157,-724,156,-724,57,-724,91,-724,40,-724,25,-724,97,-724,54,-724,35,-724,55,-724,102,-724,47,-724,36,-724,53,-724,60,-724,74,-724,72,-724,38,-724,71,-724,138,-724,136,-724,118,-724,117,-724,131,-724,132,-724,133,-724,134,-724,130,-724},new int[]{-197,37});
    states[199] = new State(-729);
    states[200] = new State(-730);
    states[201] = new State(-731);
    states[202] = new State(-732);
    states[203] = new State(-733);
    states[204] = new State(new int[]{138,205,136,1505,118,1508,117,1509,131,1510,132,1511,133,1512,134,1513,130,1514,116,-726,115,-726,128,-726,129,-726,126,-726,6,-726,5,-726,120,-726,125,-726,123,-726,121,-726,124,-726,122,-726,137,-726,135,-726,16,-726,70,-726,92,-726,10,-726,98,-726,101,-726,33,-726,104,-726,2,-726,9,-726,100,-726,12,-726,99,-726,32,-726,85,-726,84,-726,83,-726,82,-726,81,-726,86,-726,13,-726,76,-726,51,-726,58,-726,141,-726,143,-726,80,-726,78,-726,160,-726,87,-726,45,-726,42,-726,8,-726,21,-726,22,-726,144,-726,147,-726,145,-726,146,-726,155,-726,158,-726,157,-726,156,-726,57,-726,91,-726,40,-726,25,-726,97,-726,54,-726,35,-726,55,-726,102,-726,47,-726,36,-726,53,-726,60,-726,74,-726,72,-726,38,-726,71,-726},new int[]{-198,39});
    states[205] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,24,482},new int[]{-284,206,-279,207,-180,208,-147,249,-151,45,-152,48,-271,488});
    states[206] = new State(-740);
    states[207] = new State(-741);
    states[208] = new State(new int[]{7,209,4,216,123,218,8,-629,138,-629,136,-629,118,-629,117,-629,131,-629,132,-629,133,-629,134,-629,130,-629,116,-629,115,-629,128,-629,129,-629,126,-629,6,-629,5,-629,120,-629,125,-629,121,-629,124,-629,122,-629,137,-629,135,-629,16,-629,70,-629,92,-629,10,-629,98,-629,101,-629,33,-629,104,-629,2,-629,9,-629,100,-629,12,-629,99,-629,32,-629,85,-629,84,-629,83,-629,82,-629,81,-629,86,-629,13,-629,76,-629,51,-629,58,-629,141,-629,143,-629,80,-629,78,-629,160,-629,87,-629,45,-629,42,-629,21,-629,22,-629,144,-629,147,-629,145,-629,146,-629,155,-629,158,-629,157,-629,156,-629,57,-629,91,-629,40,-629,25,-629,97,-629,54,-629,35,-629,55,-629,102,-629,47,-629,36,-629,53,-629,60,-629,74,-629,72,-629,38,-629,71,-629,11,-629},new int[]{-299,215});
    states[209] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,84,55,83,56,82,57,81,58,68,59,64,60,128,61,22,62,21,63,63,64,23,65,129,66,130,67,131,68,132,69,133,70,134,71,135,72,136,73,137,74,138,75,24,76,73,77,91,78,25,79,26,80,29,81,30,82,31,83,71,84,99,85,32,86,92,87,33,88,34,89,27,90,104,91,101,92,35,93,36,94,37,95,40,96,41,97,42,98,103,99,43,100,44,101,46,102,47,103,48,104,97,105,49,106,102,107,50,108,28,109,51,110,70,111,98,112,52,113,53,114,54,115,55,116,56,117,57,118,58,119,59,120,61,121,105,122,106,123,109,124,107,125,108,126,62,127,74,128,38,129,39,130,69,131,148,132,60,133,139,134,140,135,79,136,153,137,152,138,72,139,154,140,150,141,151,142,149,143,45,214},new int[]{-138,210,-147,211,-151,45,-152,48,-293,212,-150,54,-294,213});
    states[210] = new State(-261);
    states[211] = new State(-841);
    states[212] = new State(-842);
    states[213] = new State(-843);
    states[214] = new State(-951);
    states[215] = new State(-630);
    states[216] = new State(new int[]{123,218},new int[]{-299,217});
    states[217] = new State(-631);
    states[218] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-297,219,-280,342,-273,223,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-282,1468,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,1469,-224,593,-223,594,-301,1470});
    states[219] = new State(new int[]{121,220,100,221});
    states[220] = new State(-235);
    states[221] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-280,222,-273,223,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-282,1468,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,1469,-224,593,-223,594,-301,1470});
    states[222] = new State(-239);
    states[223] = new State(new int[]{13,224,121,-243,100,-243,120,-243,9,-243,10,-243,127,-243,8,-243,138,-243,136,-243,118,-243,117,-243,131,-243,132,-243,133,-243,134,-243,130,-243,116,-243,115,-243,128,-243,129,-243,126,-243,6,-243,5,-243,125,-243,123,-243,124,-243,122,-243,137,-243,135,-243,16,-243,70,-243,92,-243,98,-243,101,-243,33,-243,104,-243,2,-243,12,-243,99,-243,32,-243,85,-243,84,-243,83,-243,82,-243,81,-243,86,-243,76,-243,51,-243,58,-243,141,-243,143,-243,80,-243,78,-243,160,-243,87,-243,45,-243,42,-243,21,-243,22,-243,144,-243,147,-243,145,-243,146,-243,155,-243,158,-243,157,-243,156,-243,57,-243,91,-243,40,-243,25,-243,97,-243,54,-243,35,-243,55,-243,102,-243,47,-243,36,-243,53,-243,60,-243,74,-243,72,-243,38,-243,71,-243,110,-243});
    states[224] = new State(-244);
    states[225] = new State(new int[]{6,1503,116,276,115,277,128,278,129,279,13,-248,121,-248,100,-248,120,-248,9,-248,10,-248,127,-248,8,-248,138,-248,136,-248,118,-248,117,-248,131,-248,132,-248,133,-248,134,-248,130,-248,126,-248,5,-248,125,-248,123,-248,124,-248,122,-248,137,-248,135,-248,16,-248,70,-248,92,-248,98,-248,101,-248,33,-248,104,-248,2,-248,12,-248,99,-248,32,-248,85,-248,84,-248,83,-248,82,-248,81,-248,86,-248,76,-248,51,-248,58,-248,141,-248,143,-248,80,-248,78,-248,160,-248,87,-248,45,-248,42,-248,21,-248,22,-248,144,-248,147,-248,145,-248,146,-248,155,-248,158,-248,157,-248,156,-248,57,-248,91,-248,40,-248,25,-248,97,-248,54,-248,35,-248,55,-248,102,-248,47,-248,36,-248,53,-248,60,-248,74,-248,72,-248,38,-248,71,-248,110,-248},new int[]{-193,226});
    states[226] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314},new int[]{-106,227,-107,344,-180,385,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312});
    states[227] = new State(new int[]{118,283,117,284,131,285,132,286,133,287,134,288,130,289,6,-252,116,-252,115,-252,128,-252,129,-252,13,-252,121,-252,100,-252,120,-252,9,-252,10,-252,127,-252,8,-252,138,-252,136,-252,126,-252,5,-252,125,-252,123,-252,124,-252,122,-252,137,-252,135,-252,16,-252,70,-252,92,-252,98,-252,101,-252,33,-252,104,-252,2,-252,12,-252,99,-252,32,-252,85,-252,84,-252,83,-252,82,-252,81,-252,86,-252,76,-252,51,-252,58,-252,141,-252,143,-252,80,-252,78,-252,160,-252,87,-252,45,-252,42,-252,21,-252,22,-252,144,-252,147,-252,145,-252,146,-252,155,-252,158,-252,157,-252,156,-252,57,-252,91,-252,40,-252,25,-252,97,-252,54,-252,35,-252,55,-252,102,-252,47,-252,36,-252,53,-252,60,-252,74,-252,72,-252,38,-252,71,-252,110,-252},new int[]{-195,228});
    states[228] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314},new int[]{-107,229,-180,385,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312});
    states[229] = new State(new int[]{8,230,118,-254,117,-254,131,-254,132,-254,133,-254,134,-254,130,-254,6,-254,116,-254,115,-254,128,-254,129,-254,13,-254,121,-254,100,-254,120,-254,9,-254,10,-254,127,-254,138,-254,136,-254,126,-254,5,-254,125,-254,123,-254,124,-254,122,-254,137,-254,135,-254,16,-254,70,-254,92,-254,98,-254,101,-254,33,-254,104,-254,2,-254,12,-254,99,-254,32,-254,85,-254,84,-254,83,-254,82,-254,81,-254,86,-254,76,-254,51,-254,58,-254,141,-254,143,-254,80,-254,78,-254,160,-254,87,-254,45,-254,42,-254,21,-254,22,-254,144,-254,147,-254,145,-254,146,-254,155,-254,158,-254,157,-254,156,-254,57,-254,91,-254,40,-254,25,-254,97,-254,54,-254,35,-254,55,-254,102,-254,47,-254,36,-254,53,-254,60,-254,74,-254,72,-254,38,-254,71,-254,110,-254});
    states[230] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372,9,-185},new int[]{-75,231,-73,233,-94,1502,-90,236,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[231] = new State(new int[]{9,232});
    states[232] = new State(-259);
    states[233] = new State(new int[]{100,234,9,-184,12,-184});
    states[234] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-94,235,-90,236,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[235] = new State(-187);
    states[236] = new State(new int[]{13,237,16,241,6,1496,100,-188,9,-188,12,-188,5,-188});
    states[237] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-90,238,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[238] = new State(new int[]{5,239,13,237,16,241});
    states[239] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-90,240,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[240] = new State(new int[]{13,237,16,241,6,-123,100,-123,9,-123,12,-123,5,-123,92,-123,10,-123,98,-123,101,-123,33,-123,104,-123,2,-123,99,-123,32,-123,85,-123,84,-123,83,-123,82,-123,81,-123,86,-123});
    states[241] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-91,242,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858});
    states[242] = new State(new int[]{120,268,125,269,123,270,121,271,124,272,122,273,137,274,13,-122,16,-122,6,-122,100,-122,9,-122,12,-122,5,-122,92,-122,10,-122,98,-122,101,-122,33,-122,104,-122,2,-122,99,-122,32,-122,85,-122,84,-122,83,-122,82,-122,81,-122,86,-122},new int[]{-192,243});
    states[243] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-81,244,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858});
    states[244] = new State(new int[]{116,276,115,277,128,278,129,279,120,-119,125,-119,123,-119,121,-119,124,-119,122,-119,137,-119,13,-119,16,-119,6,-119,100,-119,9,-119,12,-119,5,-119,92,-119,10,-119,98,-119,101,-119,33,-119,104,-119,2,-119,99,-119,32,-119,85,-119,84,-119,83,-119,82,-119,81,-119,86,-119},new int[]{-193,245});
    states[245] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-13,246,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858});
    states[246] = new State(new int[]{136,281,138,282,118,283,117,284,131,285,132,286,133,287,134,288,130,289,116,-132,115,-132,128,-132,129,-132,120,-132,125,-132,123,-132,121,-132,124,-132,122,-132,137,-132,13,-132,16,-132,6,-132,100,-132,9,-132,12,-132,5,-132,92,-132,10,-132,98,-132,101,-132,33,-132,104,-132,2,-132,99,-132,32,-132,85,-132,84,-132,83,-132,82,-132,81,-132,86,-132},new int[]{-201,247,-195,250});
    states[247] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-284,248,-180,208,-147,249,-151,45,-152,48});
    states[248] = new State(-137);
    states[249] = new State(-260);
    states[250] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-10,251,-270,252,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-11,858});
    states[251] = new State(-144);
    states[252] = new State(-145);
    states[253] = new State(new int[]{4,255,11,257,7,836,142,838,8,839,136,-155,138,-155,118,-155,117,-155,131,-155,132,-155,133,-155,134,-155,130,-155,116,-155,115,-155,128,-155,129,-155,120,-155,125,-155,123,-155,121,-155,124,-155,122,-155,137,-155,13,-155,16,-155,6,-155,100,-155,9,-155,12,-155,5,-155,92,-155,10,-155,98,-155,101,-155,33,-155,104,-155,2,-155,99,-155,32,-155,85,-155,84,-155,83,-155,82,-155,81,-155,86,-155,119,-153},new int[]{-12,254});
    states[254] = new State(-175);
    states[255] = new State(new int[]{123,218},new int[]{-299,256});
    states[256] = new State(-176);
    states[257] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372,5,1498,12,-185},new int[]{-121,258,-75,260,-90,262,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864,-73,233,-94,1502});
    states[258] = new State(new int[]{12,259});
    states[259] = new State(-177);
    states[260] = new State(new int[]{12,261});
    states[261] = new State(-181);
    states[262] = new State(new int[]{5,263,13,237,16,241,6,1496,100,-188,12,-188});
    states[263] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372,5,-707,12,-707},new int[]{-122,264,-90,1495,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[264] = new State(new int[]{5,265,12,-712});
    states[265] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-90,266,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[266] = new State(new int[]{13,237,16,241,12,-714});
    states[267] = new State(new int[]{120,268,125,269,123,270,121,271,124,272,122,273,137,274,13,-120,16,-120,6,-120,100,-120,9,-120,12,-120,5,-120,92,-120,10,-120,98,-120,101,-120,33,-120,104,-120,2,-120,99,-120,32,-120,85,-120,84,-120,83,-120,82,-120,81,-120,86,-120},new int[]{-192,243});
    states[268] = new State(-124);
    states[269] = new State(-125);
    states[270] = new State(-126);
    states[271] = new State(-127);
    states[272] = new State(-128);
    states[273] = new State(-129);
    states[274] = new State(-130);
    states[275] = new State(new int[]{116,276,115,277,128,278,129,279,120,-118,125,-118,123,-118,121,-118,124,-118,122,-118,137,-118,13,-118,16,-118,6,-118,100,-118,9,-118,12,-118,5,-118,92,-118,10,-118,98,-118,101,-118,33,-118,104,-118,2,-118,99,-118,32,-118,85,-118,84,-118,83,-118,82,-118,81,-118,86,-118},new int[]{-193,245});
    states[276] = new State(-133);
    states[277] = new State(-134);
    states[278] = new State(-135);
    states[279] = new State(-136);
    states[280] = new State(new int[]{136,281,138,282,118,283,117,284,131,285,132,286,133,287,134,288,130,289,116,-131,115,-131,128,-131,129,-131,120,-131,125,-131,123,-131,121,-131,124,-131,122,-131,137,-131,13,-131,16,-131,6,-131,100,-131,9,-131,12,-131,5,-131,92,-131,10,-131,98,-131,101,-131,33,-131,104,-131,2,-131,99,-131,32,-131,85,-131,84,-131,83,-131,82,-131,81,-131,86,-131},new int[]{-201,247,-195,250});
    states[281] = new State(-734);
    states[282] = new State(-735);
    states[283] = new State(-146);
    states[284] = new State(-147);
    states[285] = new State(-148);
    states[286] = new State(-149);
    states[287] = new State(-150);
    states[288] = new State(-151);
    states[289] = new State(-152);
    states[290] = new State(-141);
    states[291] = new State(-169);
    states[292] = new State(new int[]{26,1484,143,44,85,46,86,47,80,49,78,50,160,51,87,52,8,-860,7,-860,142,-860,4,-860,15,-860,110,-860,111,-860,112,-860,113,-860,114,-860,92,-860,10,-860,11,-860,17,-860,5,-860,98,-860,101,-860,33,-860,104,-860,2,-860,127,-860,138,-860,136,-860,118,-860,117,-860,131,-860,132,-860,133,-860,134,-860,130,-860,116,-860,115,-860,128,-860,129,-860,126,-860,6,-860,120,-860,125,-860,123,-860,121,-860,124,-860,122,-860,137,-860,135,-860,16,-860,70,-860,9,-860,100,-860,12,-860,99,-860,32,-860,84,-860,83,-860,82,-860,81,-860,13,-860,119,-860,76,-860,51,-860,58,-860,141,-860,45,-860,42,-860,21,-860,22,-860,144,-860,147,-860,145,-860,146,-860,155,-860,158,-860,157,-860,156,-860,57,-860,91,-860,40,-860,25,-860,97,-860,54,-860,35,-860,55,-860,102,-860,47,-860,36,-860,53,-860,60,-860,74,-860,72,-860,38,-860,71,-860},new int[]{-284,293,-180,208,-147,249,-151,45,-152,48});
    states[293] = new State(new int[]{11,295,8,668,92,-643,10,-643,98,-643,101,-643,33,-643,104,-643,2,-643,138,-643,136,-643,118,-643,117,-643,131,-643,132,-643,133,-643,134,-643,130,-643,116,-643,115,-643,128,-643,129,-643,126,-643,6,-643,5,-643,120,-643,125,-643,123,-643,121,-643,124,-643,122,-643,137,-643,135,-643,16,-643,70,-643,9,-643,100,-643,12,-643,99,-643,32,-643,85,-643,84,-643,83,-643,82,-643,81,-643,86,-643,13,-643,76,-643,51,-643,58,-643,141,-643,143,-643,80,-643,78,-643,160,-643,87,-643,45,-643,42,-643,21,-643,22,-643,144,-643,147,-643,145,-643,146,-643,155,-643,158,-643,157,-643,156,-643,57,-643,91,-643,40,-643,25,-643,97,-643,54,-643,35,-643,55,-643,102,-643,47,-643,36,-643,53,-643,60,-643,74,-643,72,-643,38,-643,71,-643},new int[]{-70,294});
    states[294] = new State(-636);
    states[295] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,673,8,674,21,323,22,328,76,552,40,620,5,629,18,693,37,702,44,706,12,-813},new int[]{-67,296,-71,671,-88,672,-87,27,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-323,843,-100,683,-324,701});
    states[296] = new State(new int[]{12,297});
    states[297] = new State(new int[]{8,299,92,-635,10,-635,98,-635,101,-635,33,-635,104,-635,2,-635,138,-635,136,-635,118,-635,117,-635,131,-635,132,-635,133,-635,134,-635,130,-635,116,-635,115,-635,128,-635,129,-635,126,-635,6,-635,5,-635,120,-635,125,-635,123,-635,121,-635,124,-635,122,-635,137,-635,135,-635,16,-635,70,-635,9,-635,100,-635,12,-635,99,-635,32,-635,85,-635,84,-635,83,-635,82,-635,81,-635,86,-635,13,-635,76,-635,51,-635,58,-635,141,-635,143,-635,80,-635,78,-635,160,-635,87,-635,45,-635,42,-635,21,-635,22,-635,144,-635,147,-635,145,-635,146,-635,155,-635,158,-635,157,-635,156,-635,57,-635,91,-635,40,-635,25,-635,97,-635,54,-635,35,-635,55,-635,102,-635,47,-635,36,-635,53,-635,60,-635,74,-635,72,-635,38,-635,71,-635},new int[]{-5,298});
    states[298] = new State(-637);
    states[299] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,1010,135,850,116,386,115,387,63,372,9,-198},new int[]{-66,300,-65,302,-85,1013,-84,305,-90,306,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864,-95,1014,-244,1015});
    states[300] = new State(new int[]{9,301});
    states[301] = new State(-634);
    states[302] = new State(new int[]{100,303,9,-199});
    states[303] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,1010,135,850,116,386,115,387,63,372},new int[]{-85,304,-84,305,-90,306,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864,-95,1014,-244,1015});
    states[304] = new State(-201);
    states[305] = new State(-417);
    states[306] = new State(new int[]{13,237,16,241,100,-194,9,-194,92,-194,10,-194,98,-194,101,-194,33,-194,104,-194,2,-194,12,-194,99,-194,32,-194,85,-194,84,-194,83,-194,82,-194,81,-194,86,-194});
    states[307] = new State(-170);
    states[308] = new State(new int[]{144,310,147,311,7,-824,11,-824,17,-824,8,-824,138,-824,136,-824,118,-824,117,-824,131,-824,132,-824,133,-824,134,-824,130,-824,116,-824,115,-824,128,-824,129,-824,126,-824,6,-824,5,-824,120,-824,125,-824,123,-824,121,-824,124,-824,122,-824,137,-824,135,-824,16,-824,70,-824,92,-824,10,-824,98,-824,101,-824,33,-824,104,-824,2,-824,9,-824,100,-824,12,-824,99,-824,32,-824,85,-824,84,-824,83,-824,82,-824,81,-824,86,-824,13,-824,119,-824,76,-824,51,-824,58,-824,141,-824,143,-824,80,-824,78,-824,160,-824,87,-824,45,-824,42,-824,21,-824,22,-824,145,-824,146,-824,155,-824,158,-824,157,-824,156,-824,57,-824,91,-824,40,-824,25,-824,97,-824,54,-824,35,-824,55,-824,102,-824,47,-824,36,-824,53,-824,60,-824,74,-824,72,-824,38,-824,71,-824,127,-824,110,-824,4,-824,142,-824},new int[]{-166,309});
    states[309] = new State(-828);
    states[310] = new State(-822);
    states[311] = new State(-823);
    states[312] = new State(-827);
    states[313] = new State(-825);
    states[314] = new State(-826);
    states[315] = new State(-171);
    states[316] = new State(-190);
    states[317] = new State(-191);
    states[318] = new State(-192);
    states[319] = new State(-193);
    states[320] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,321,-151,45,-152,48});
    states[321] = new State(-172);
    states[322] = new State(-173);
    states[323] = new State(new int[]{8,324});
    states[324] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-284,325,-180,208,-147,249,-151,45,-152,48});
    states[325] = new State(new int[]{9,326});
    states[326] = new State(-622);
    states[327] = new State(-174);
    states[328] = new State(new int[]{8,329});
    states[329] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-284,330,-283,332,-180,334,-147,249,-151,45,-152,48});
    states[330] = new State(new int[]{9,331});
    states[331] = new State(-623);
    states[332] = new State(new int[]{9,333});
    states[333] = new State(-624);
    states[334] = new State(new int[]{7,209,4,335,123,337,125,1482,9,-629},new int[]{-299,215,-300,1483});
    states[335] = new State(new int[]{123,337,125,1482},new int[]{-299,217,-300,336});
    states[336] = new State(-628);
    states[337] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,24,482,48,490,49,581,34,585,73,589,44,595,37,635,121,-242,100,-242},new int[]{-297,219,-298,338,-280,342,-273,223,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-282,1468,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,1469,-224,593,-223,594,-301,1470,-281,1481});
    states[338] = new State(new int[]{121,339,100,340});
    states[339] = new State(-237);
    states[340] = new State(-242,new int[]{-281,341});
    states[341] = new State(-241);
    states[342] = new State(-238);
    states[343] = new State(new int[]{118,283,117,284,131,285,132,286,133,287,134,288,130,289,6,-251,116,-251,115,-251,128,-251,129,-251,13,-251,121,-251,100,-251,120,-251,9,-251,10,-251,127,-251,8,-251,138,-251,136,-251,126,-251,5,-251,125,-251,123,-251,124,-251,122,-251,137,-251,135,-251,16,-251,70,-251,92,-251,98,-251,101,-251,33,-251,104,-251,2,-251,12,-251,99,-251,32,-251,85,-251,84,-251,83,-251,82,-251,81,-251,86,-251,76,-251,51,-251,58,-251,141,-251,143,-251,80,-251,78,-251,160,-251,87,-251,45,-251,42,-251,21,-251,22,-251,144,-251,147,-251,145,-251,146,-251,155,-251,158,-251,157,-251,156,-251,57,-251,91,-251,40,-251,25,-251,97,-251,54,-251,35,-251,55,-251,102,-251,47,-251,36,-251,53,-251,60,-251,74,-251,72,-251,38,-251,71,-251,110,-251},new int[]{-195,228});
    states[344] = new State(new int[]{8,230,118,-253,117,-253,131,-253,132,-253,133,-253,134,-253,130,-253,6,-253,116,-253,115,-253,128,-253,129,-253,13,-253,121,-253,100,-253,120,-253,9,-253,10,-253,127,-253,138,-253,136,-253,126,-253,5,-253,125,-253,123,-253,124,-253,122,-253,137,-253,135,-253,16,-253,70,-253,92,-253,98,-253,101,-253,33,-253,104,-253,2,-253,12,-253,99,-253,32,-253,85,-253,84,-253,83,-253,82,-253,81,-253,86,-253,76,-253,51,-253,58,-253,141,-253,143,-253,80,-253,78,-253,160,-253,87,-253,45,-253,42,-253,21,-253,22,-253,144,-253,147,-253,145,-253,146,-253,155,-253,158,-253,157,-253,156,-253,57,-253,91,-253,40,-253,25,-253,97,-253,54,-253,35,-253,55,-253,102,-253,47,-253,36,-253,53,-253,60,-253,74,-253,72,-253,38,-253,71,-253,110,-253});
    states[345] = new State(new int[]{7,209,127,346,123,218,8,-255,118,-255,117,-255,131,-255,132,-255,133,-255,134,-255,130,-255,6,-255,116,-255,115,-255,128,-255,129,-255,13,-255,121,-255,100,-255,120,-255,9,-255,10,-255,138,-255,136,-255,126,-255,5,-255,125,-255,124,-255,122,-255,137,-255,135,-255,16,-255,70,-255,92,-255,98,-255,101,-255,33,-255,104,-255,2,-255,12,-255,99,-255,32,-255,85,-255,84,-255,83,-255,82,-255,81,-255,86,-255,76,-255,51,-255,58,-255,141,-255,143,-255,80,-255,78,-255,160,-255,87,-255,45,-255,42,-255,21,-255,22,-255,144,-255,147,-255,145,-255,146,-255,155,-255,158,-255,157,-255,156,-255,57,-255,91,-255,40,-255,25,-255,97,-255,54,-255,35,-255,55,-255,102,-255,47,-255,36,-255,53,-255,60,-255,74,-255,72,-255,38,-255,71,-255,110,-255},new int[]{-299,667});
    states[346] = new State(new int[]{8,348,143,44,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-280,347,-273,223,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-282,1468,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,1469,-224,593,-223,594,-301,1470});
    states[347] = new State(-288);
    states[348] = new State(new int[]{9,349,143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-80,354,-78,360,-277,361,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[349] = new State(new int[]{127,350,121,-292,100,-292,120,-292,9,-292,10,-292,8,-292,138,-292,136,-292,118,-292,117,-292,131,-292,132,-292,133,-292,134,-292,130,-292,116,-292,115,-292,128,-292,129,-292,126,-292,6,-292,5,-292,125,-292,123,-292,124,-292,122,-292,137,-292,135,-292,16,-292,70,-292,92,-292,98,-292,101,-292,33,-292,104,-292,2,-292,12,-292,99,-292,32,-292,85,-292,84,-292,83,-292,82,-292,81,-292,86,-292,13,-292,76,-292,51,-292,58,-292,141,-292,143,-292,80,-292,78,-292,160,-292,87,-292,45,-292,42,-292,21,-292,22,-292,144,-292,147,-292,145,-292,146,-292,155,-292,158,-292,157,-292,156,-292,57,-292,91,-292,40,-292,25,-292,97,-292,54,-292,35,-292,55,-292,102,-292,47,-292,36,-292,53,-292,60,-292,74,-292,72,-292,38,-292,71,-292,110,-292});
    states[350] = new State(new int[]{8,352,143,44,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-280,351,-273,223,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-282,1468,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,1469,-224,593,-223,594,-301,1470});
    states[351] = new State(-290);
    states[352] = new State(new int[]{9,353,143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-80,354,-78,360,-277,361,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[353] = new State(new int[]{127,350,121,-294,100,-294,120,-294,9,-294,10,-294,8,-294,138,-294,136,-294,118,-294,117,-294,131,-294,132,-294,133,-294,134,-294,130,-294,116,-294,115,-294,128,-294,129,-294,126,-294,6,-294,5,-294,125,-294,123,-294,124,-294,122,-294,137,-294,135,-294,16,-294,70,-294,92,-294,98,-294,101,-294,33,-294,104,-294,2,-294,12,-294,99,-294,32,-294,85,-294,84,-294,83,-294,82,-294,81,-294,86,-294,13,-294,76,-294,51,-294,58,-294,141,-294,143,-294,80,-294,78,-294,160,-294,87,-294,45,-294,42,-294,21,-294,22,-294,144,-294,147,-294,145,-294,146,-294,155,-294,158,-294,157,-294,156,-294,57,-294,91,-294,40,-294,25,-294,97,-294,54,-294,35,-294,55,-294,102,-294,47,-294,36,-294,53,-294,60,-294,74,-294,72,-294,38,-294,71,-294,110,-294});
    states[354] = new State(new int[]{9,355,100,392});
    states[355] = new State(new int[]{127,356,13,-250,121,-250,100,-250,120,-250,9,-250,10,-250,8,-250,138,-250,136,-250,118,-250,117,-250,131,-250,132,-250,133,-250,134,-250,130,-250,116,-250,115,-250,128,-250,129,-250,126,-250,6,-250,5,-250,125,-250,123,-250,124,-250,122,-250,137,-250,135,-250,16,-250,70,-250,92,-250,98,-250,101,-250,33,-250,104,-250,2,-250,12,-250,99,-250,32,-250,85,-250,84,-250,83,-250,82,-250,81,-250,86,-250,76,-250,51,-250,58,-250,141,-250,143,-250,80,-250,78,-250,160,-250,87,-250,45,-250,42,-250,21,-250,22,-250,144,-250,147,-250,145,-250,146,-250,155,-250,158,-250,157,-250,156,-250,57,-250,91,-250,40,-250,25,-250,97,-250,54,-250,35,-250,55,-250,102,-250,47,-250,36,-250,53,-250,60,-250,74,-250,72,-250,38,-250,71,-250,110,-250});
    states[356] = new State(new int[]{8,358,143,44,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-280,357,-273,223,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-282,1468,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,1469,-224,593,-223,594,-301,1470});
    states[357] = new State(-291);
    states[358] = new State(new int[]{9,359,143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-80,354,-78,360,-277,361,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[359] = new State(new int[]{127,350,121,-295,100,-295,120,-295,9,-295,10,-295,8,-295,138,-295,136,-295,118,-295,117,-295,131,-295,132,-295,133,-295,134,-295,130,-295,116,-295,115,-295,128,-295,129,-295,126,-295,6,-295,5,-295,125,-295,123,-295,124,-295,122,-295,137,-295,135,-295,16,-295,70,-295,92,-295,98,-295,101,-295,33,-295,104,-295,2,-295,12,-295,99,-295,32,-295,85,-295,84,-295,83,-295,82,-295,81,-295,86,-295,13,-295,76,-295,51,-295,58,-295,141,-295,143,-295,80,-295,78,-295,160,-295,87,-295,45,-295,42,-295,21,-295,22,-295,144,-295,147,-295,145,-295,146,-295,155,-295,158,-295,157,-295,156,-295,57,-295,91,-295,40,-295,25,-295,97,-295,54,-295,35,-295,55,-295,102,-295,47,-295,36,-295,53,-295,60,-295,74,-295,72,-295,38,-295,71,-295,110,-295});
    states[360] = new State(-262);
    states[361] = new State(new int[]{120,362,9,-264,100,-264});
    states[362] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-87,363,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[363] = new State(new int[]{70,28,9,-265,100,-265});
    states[364] = new State(new int[]{7,41,8,175,138,-745,136,-745,118,-745,117,-745,131,-745,132,-745,133,-745,134,-745,130,-745,116,-745,115,-745,128,-745,129,-745,126,-745,6,-745,5,-745,120,-745,125,-745,123,-745,121,-745,124,-745,122,-745,137,-745,135,-745,16,-745,70,-745,92,-745,10,-745,98,-745,101,-745,33,-745,104,-745,2,-745,9,-745,100,-745,12,-745,99,-745,32,-745,85,-745,84,-745,83,-745,82,-745,81,-745,86,-745,13,-745,76,-745,51,-745,58,-745,141,-745,143,-745,80,-745,78,-745,160,-745,87,-745,45,-745,42,-745,21,-745,22,-745,144,-745,147,-745,145,-745,146,-745,155,-745,158,-745,157,-745,156,-745,57,-745,91,-745,40,-745,25,-745,97,-745,54,-745,35,-745,55,-745,102,-745,47,-745,36,-745,53,-745,60,-745,74,-745,72,-745,38,-745,71,-745});
    states[365] = new State(-763);
    states[366] = new State(new int[]{7,367,8,-764,138,-764,136,-764,118,-764,117,-764,131,-764,132,-764,133,-764,134,-764,130,-764,116,-764,115,-764,128,-764,129,-764,126,-764,6,-764,5,-764,120,-764,125,-764,123,-764,121,-764,124,-764,122,-764,137,-764,135,-764,16,-764,70,-764,92,-764,10,-764,98,-764,101,-764,33,-764,104,-764,2,-764,9,-764,100,-764,12,-764,99,-764,32,-764,85,-764,84,-764,83,-764,82,-764,81,-764,86,-764,13,-764,76,-764,51,-764,58,-764,141,-764,143,-764,80,-764,78,-764,160,-764,87,-764,45,-764,42,-764,21,-764,22,-764,144,-764,147,-764,145,-764,146,-764,155,-764,158,-764,157,-764,156,-764,57,-764,91,-764,40,-764,25,-764,97,-764,54,-764,35,-764,55,-764,102,-764,47,-764,36,-764,53,-764,60,-764,74,-764,72,-764,38,-764,71,-764,11,-791,17,-791,119,-761});
    states[367] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,84,55,83,56,82,57,81,58,68,59,64,60,128,61,22,62,21,63,63,64,23,65,129,66,130,67,131,68,132,69,133,70,134,71,135,72,136,73,137,74,138,75,24,76,73,77,91,78,25,79,26,80,29,81,30,82,31,83,71,84,99,85,32,86,92,87,33,88,34,89,27,90,104,91,101,92,35,93,36,94,37,95,40,96,41,97,42,98,103,99,43,100,44,101,46,102,47,103,48,104,97,105,49,106,102,107,50,108,28,109,51,110,70,111,98,112,52,113,53,114,54,115,55,116,56,117,57,118,58,119,59,120,61,121,105,122,106,123,109,124,107,125,108,126,62,127,74,128,38,129,39,130,69,131,148,132,60,133,139,134,140,135,79,136,153,137,152,138,72,139,154,140,150,141,151,142,149,143,45,214},new int[]{-138,368,-147,211,-151,45,-152,48,-293,212,-150,54,-294,213});
    states[368] = new State(-803);
    states[369] = new State(-774);
    states[370] = new State(-775);
    states[371] = new State(-765);
    states[372] = new State(new int[]{8,373});
    states[373] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,389},new int[]{-285,374,-284,376,-180,377,-147,249,-151,45,-152,48,-274,379,-273,380,-93,225,-106,343,-107,344,-16,382,-199,383,-165,388,-167,308,-166,312,-301,1480});
    states[374] = new State(new int[]{9,375});
    states[375] = new State(-759);
    states[376] = new State(-632);
    states[377] = new State(new int[]{7,209,4,216,123,218,9,-629,8,-255,118,-255,117,-255,131,-255,132,-255,133,-255,134,-255,130,-255,6,-255,116,-255,115,-255,128,-255,129,-255,13,-255},new int[]{-299,378});
    states[378] = new State(new int[]{9,-630,13,-234});
    states[379] = new State(-633);
    states[380] = new State(new int[]{13,381});
    states[381] = new State(-225);
    states[382] = new State(-256);
    states[383] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314},new int[]{-107,384,-180,385,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312});
    states[384] = new State(new int[]{8,230,118,-257,117,-257,131,-257,132,-257,133,-257,134,-257,130,-257,6,-257,116,-257,115,-257,128,-257,129,-257,13,-257,121,-257,100,-257,120,-257,9,-257,10,-257,127,-257,138,-257,136,-257,126,-257,5,-257,125,-257,123,-257,124,-257,122,-257,137,-257,135,-257,16,-257,70,-257,92,-257,98,-257,101,-257,33,-257,104,-257,2,-257,12,-257,99,-257,32,-257,85,-257,84,-257,83,-257,82,-257,81,-257,86,-257,76,-257,51,-257,58,-257,141,-257,143,-257,80,-257,78,-257,160,-257,87,-257,45,-257,42,-257,21,-257,22,-257,144,-257,147,-257,145,-257,146,-257,155,-257,158,-257,157,-257,156,-257,57,-257,91,-257,40,-257,25,-257,97,-257,54,-257,35,-257,55,-257,102,-257,47,-257,36,-257,53,-257,60,-257,74,-257,72,-257,38,-257,71,-257,110,-257});
    states[385] = new State(new int[]{7,209,8,-255,118,-255,117,-255,131,-255,132,-255,133,-255,134,-255,130,-255,6,-255,116,-255,115,-255,128,-255,129,-255,13,-255,121,-255,100,-255,120,-255,9,-255,10,-255,127,-255,138,-255,136,-255,126,-255,5,-255,125,-255,123,-255,124,-255,122,-255,137,-255,135,-255,16,-255,70,-255,92,-255,98,-255,101,-255,33,-255,104,-255,2,-255,12,-255,99,-255,32,-255,85,-255,84,-255,83,-255,82,-255,81,-255,86,-255,76,-255,51,-255,58,-255,141,-255,143,-255,80,-255,78,-255,160,-255,87,-255,45,-255,42,-255,21,-255,22,-255,144,-255,147,-255,145,-255,146,-255,155,-255,158,-255,157,-255,156,-255,57,-255,91,-255,40,-255,25,-255,97,-255,54,-255,35,-255,55,-255,102,-255,47,-255,36,-255,53,-255,60,-255,74,-255,72,-255,38,-255,71,-255,110,-255});
    states[386] = new State(-167);
    states[387] = new State(-168);
    states[388] = new State(-258);
    states[389] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-80,390,-78,360,-277,361,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[390] = new State(new int[]{9,391,100,392});
    states[391] = new State(-250);
    states[392] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-78,393,-277,361,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[393] = new State(-263);
    states[394] = new State(new int[]{13,381,120,-227,9,-227,100,-227,10,-227,127,-227,121,-227,8,-227,138,-227,136,-227,118,-227,117,-227,131,-227,132,-227,133,-227,134,-227,130,-227,116,-227,115,-227,128,-227,129,-227,126,-227,6,-227,5,-227,125,-227,123,-227,124,-227,122,-227,137,-227,135,-227,16,-227,70,-227,92,-227,98,-227,101,-227,33,-227,104,-227,2,-227,12,-227,99,-227,32,-227,85,-227,84,-227,83,-227,82,-227,81,-227,86,-227,76,-227,51,-227,58,-227,141,-227,143,-227,80,-227,78,-227,160,-227,87,-227,45,-227,42,-227,21,-227,22,-227,144,-227,147,-227,145,-227,146,-227,155,-227,158,-227,157,-227,156,-227,57,-227,91,-227,40,-227,25,-227,97,-227,54,-227,35,-227,55,-227,102,-227,47,-227,36,-227,53,-227,60,-227,74,-227,72,-227,38,-227,71,-227,110,-227});
    states[395] = new State(new int[]{11,396,7,-836,127,-836,123,-836,8,-836,118,-836,117,-836,131,-836,132,-836,133,-836,134,-836,130,-836,6,-836,116,-836,115,-836,128,-836,129,-836,13,-836,120,-836,9,-836,100,-836,10,-836,121,-836,138,-836,136,-836,126,-836,5,-836,125,-836,124,-836,122,-836,137,-836,135,-836,16,-836,70,-836,92,-836,98,-836,101,-836,33,-836,104,-836,2,-836,12,-836,99,-836,32,-836,85,-836,84,-836,83,-836,82,-836,81,-836,86,-836,76,-836,51,-836,58,-836,141,-836,143,-836,80,-836,78,-836,160,-836,87,-836,45,-836,42,-836,21,-836,22,-836,144,-836,147,-836,145,-836,146,-836,155,-836,158,-836,157,-836,156,-836,57,-836,91,-836,40,-836,25,-836,97,-836,54,-836,35,-836,55,-836,102,-836,47,-836,36,-836,53,-836,60,-836,74,-836,72,-836,38,-836,71,-836,110,-836});
    states[396] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-90,397,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[397] = new State(new int[]{12,398,13,237,16,241});
    states[398] = new State(-283);
    states[399] = new State(-156);
    states[400] = new State(-165);
    states[401] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629,12,-817},new int[]{-69,402,-77,404,-92,555,-87,407,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[402] = new State(new int[]{12,403});
    states[403] = new State(-164);
    states[404] = new State(new int[]{100,405,12,-816,76,-816});
    states[405] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-92,406,-87,407,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[406] = new State(-819);
    states[407] = new State(new int[]{70,28,6,408,100,-820,12,-820,76,-820});
    states[408] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-87,409,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[409] = new State(new int[]{70,28,100,-821,12,-821,76,-821});
    states[410] = new State(-766);
    states[411] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552},new int[]{-96,412,-15,413,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,414,-113,418,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560});
    states[412] = new State(new int[]{7,41,8,175,138,-769,136,-769,118,-769,117,-769,131,-769,132,-769,133,-769,134,-769,130,-769,116,-769,115,-769,128,-769,129,-769,126,-769,6,-769,5,-769,120,-769,125,-769,123,-769,121,-769,124,-769,122,-769,137,-769,135,-769,16,-769,70,-769,92,-769,10,-769,98,-769,101,-769,33,-769,104,-769,2,-769,9,-769,100,-769,12,-769,99,-769,32,-769,85,-769,84,-769,83,-769,82,-769,81,-769,86,-769,13,-769,76,-769,51,-769,58,-769,141,-769,143,-769,80,-769,78,-769,160,-769,87,-769,45,-769,42,-769,21,-769,22,-769,144,-769,147,-769,145,-769,146,-769,155,-769,158,-769,157,-769,156,-769,57,-769,91,-769,40,-769,25,-769,97,-769,54,-769,35,-769,55,-769,102,-769,47,-769,36,-769,53,-769,60,-769,74,-769,72,-769,38,-769,71,-769});
    states[413] = new State(new int[]{7,367,8,-764,138,-764,136,-764,118,-764,117,-764,131,-764,132,-764,133,-764,134,-764,130,-764,116,-764,115,-764,128,-764,129,-764,126,-764,6,-764,5,-764,120,-764,125,-764,123,-764,121,-764,124,-764,122,-764,137,-764,135,-764,16,-764,70,-764,92,-764,10,-764,98,-764,101,-764,33,-764,104,-764,2,-764,9,-764,100,-764,12,-764,99,-764,32,-764,85,-764,84,-764,83,-764,82,-764,81,-764,86,-764,13,-764,76,-764,51,-764,58,-764,141,-764,143,-764,80,-764,78,-764,160,-764,87,-764,45,-764,42,-764,21,-764,22,-764,144,-764,147,-764,145,-764,146,-764,155,-764,158,-764,157,-764,156,-764,57,-764,91,-764,40,-764,25,-764,97,-764,54,-764,35,-764,55,-764,102,-764,47,-764,36,-764,53,-764,60,-764,74,-764,72,-764,38,-764,71,-764,11,-791,17,-791});
    states[414] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552},new int[]{-96,415,-15,413,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,414,-113,418,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560});
    states[415] = new State(new int[]{7,41,8,175,138,-770,136,-770,118,-770,117,-770,131,-770,132,-770,133,-770,134,-770,130,-770,116,-770,115,-770,128,-770,129,-770,126,-770,6,-770,5,-770,120,-770,125,-770,123,-770,121,-770,124,-770,122,-770,137,-770,135,-770,16,-770,70,-770,92,-770,10,-770,98,-770,101,-770,33,-770,104,-770,2,-770,9,-770,100,-770,12,-770,99,-770,32,-770,85,-770,84,-770,83,-770,82,-770,81,-770,86,-770,13,-770,76,-770,51,-770,58,-770,141,-770,143,-770,80,-770,78,-770,160,-770,87,-770,45,-770,42,-770,21,-770,22,-770,144,-770,147,-770,145,-770,146,-770,155,-770,158,-770,157,-770,156,-770,57,-770,91,-770,40,-770,25,-770,97,-770,54,-770,35,-770,55,-770,102,-770,47,-770,36,-770,53,-770,60,-770,74,-770,72,-770,38,-770,71,-770});
    states[416] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552},new int[]{-96,417,-15,413,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,414,-113,418,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560});
    states[417] = new State(new int[]{7,41,8,175,138,-771,136,-771,118,-771,117,-771,131,-771,132,-771,133,-771,134,-771,130,-771,116,-771,115,-771,128,-771,129,-771,126,-771,6,-771,5,-771,120,-771,125,-771,123,-771,121,-771,124,-771,122,-771,137,-771,135,-771,16,-771,70,-771,92,-771,10,-771,98,-771,101,-771,33,-771,104,-771,2,-771,9,-771,100,-771,12,-771,99,-771,32,-771,85,-771,84,-771,83,-771,82,-771,81,-771,86,-771,13,-771,76,-771,51,-771,58,-771,141,-771,143,-771,80,-771,78,-771,160,-771,87,-771,45,-771,42,-771,21,-771,22,-771,144,-771,147,-771,145,-771,146,-771,155,-771,158,-771,157,-771,156,-771,57,-771,91,-771,40,-771,25,-771,97,-771,54,-771,35,-771,55,-771,102,-771,47,-771,36,-771,53,-771,60,-771,74,-771,72,-771,38,-771,71,-771});
    states[418] = new State(-772);
    states[419] = new State(new int[]{141,1479,143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,42,440,8,442,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552},new int[]{-111,420,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720});
    states[420] = new State(new int[]{8,421,7,430,142,432,4,433,110,-778,111,-778,112,-778,113,-778,114,-778,92,-778,10,-778,98,-778,101,-778,33,-778,104,-778,2,-778,138,-778,136,-778,118,-778,117,-778,131,-778,132,-778,133,-778,134,-778,130,-778,116,-778,115,-778,128,-778,129,-778,126,-778,6,-778,5,-778,120,-778,125,-778,123,-778,121,-778,124,-778,122,-778,137,-778,135,-778,16,-778,70,-778,9,-778,100,-778,12,-778,99,-778,32,-778,85,-778,84,-778,83,-778,82,-778,81,-778,86,-778,13,-778,119,-778,76,-778,51,-778,58,-778,141,-778,143,-778,80,-778,78,-778,160,-778,87,-778,45,-778,42,-778,21,-778,22,-778,144,-778,147,-778,145,-778,146,-778,155,-778,158,-778,157,-778,156,-778,57,-778,91,-778,40,-778,25,-778,97,-778,54,-778,35,-778,55,-778,102,-778,47,-778,36,-778,53,-778,60,-778,74,-778,72,-778,38,-778,71,-778,11,-790,17,-790});
    states[421] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,1477,8,674,21,323,22,328,76,552,40,620,5,629,18,693,37,702,44,706,9,-815},new int[]{-68,422,-72,178,-89,424,-87,181,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,1474,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-323,1478,-100,683,-324,701});
    states[422] = new State(new int[]{9,423});
    states[423] = new State(-795);
    states[424] = new State(-594);
    states[425] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552},new int[]{-96,415,-269,426,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-97,563});
    states[426] = new State(-744);
    states[427] = new State(new int[]{7,-772,8,-772,138,-772,136,-772,118,-772,117,-772,131,-772,132,-772,133,-772,134,-772,130,-772,116,-772,115,-772,128,-772,129,-772,126,-772,6,-772,5,-772,120,-772,125,-772,123,-772,121,-772,124,-772,122,-772,137,-772,135,-772,16,-772,70,-772,92,-772,10,-772,98,-772,101,-772,33,-772,104,-772,2,-772,9,-772,100,-772,12,-772,99,-772,32,-772,85,-772,84,-772,83,-772,82,-772,81,-772,86,-772,13,-772,76,-772,51,-772,58,-772,141,-772,143,-772,80,-772,78,-772,160,-772,87,-772,45,-772,42,-772,21,-772,22,-772,144,-772,147,-772,145,-772,146,-772,155,-772,158,-772,157,-772,156,-772,57,-772,91,-772,40,-772,25,-772,97,-772,54,-772,35,-772,55,-772,102,-772,47,-772,36,-772,53,-772,60,-772,74,-772,72,-772,38,-772,71,-772,119,-762});
    states[428] = new State(-782);
    states[429] = new State(new int[]{8,421,7,430,142,432,4,433,15,435,110,-779,111,-779,112,-779,113,-779,114,-779,92,-779,10,-779,98,-779,101,-779,33,-779,104,-779,2,-779,138,-779,136,-779,118,-779,117,-779,131,-779,132,-779,133,-779,134,-779,130,-779,116,-779,115,-779,128,-779,129,-779,126,-779,6,-779,5,-779,120,-779,125,-779,123,-779,121,-779,124,-779,122,-779,137,-779,135,-779,16,-779,70,-779,9,-779,100,-779,12,-779,99,-779,32,-779,85,-779,84,-779,83,-779,82,-779,81,-779,86,-779,13,-779,119,-779,76,-779,51,-779,58,-779,141,-779,143,-779,80,-779,78,-779,160,-779,87,-779,45,-779,42,-779,21,-779,22,-779,144,-779,147,-779,145,-779,146,-779,155,-779,158,-779,157,-779,156,-779,57,-779,91,-779,40,-779,25,-779,97,-779,54,-779,35,-779,55,-779,102,-779,47,-779,36,-779,53,-779,60,-779,74,-779,72,-779,38,-779,71,-779,11,-790,17,-790});
    states[430] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,84,55,83,56,82,57,81,58,68,59,64,60,128,61,22,62,21,63,63,64,23,65,129,66,130,67,131,68,132,69,133,70,134,71,135,72,136,73,137,74,138,75,24,76,73,77,91,78,25,79,26,80,29,81,30,82,31,83,71,84,99,85,32,86,92,87,33,88,34,89,27,90,104,91,101,92,35,93,36,94,37,95,40,96,41,97,42,98,103,99,43,100,44,101,46,102,47,103,48,104,97,105,49,106,102,107,50,108,28,109,51,110,70,111,98,112,52,113,53,114,54,115,55,116,56,117,57,118,58,119,59,120,61,121,105,122,106,123,109,124,107,125,108,126,62,127,74,128,38,129,39,130,69,131,148,132,60,133,139,134,140,135,79,136,153,137,152,138,72,139,154,140,150,141,151,142,149,143,45,145},new int[]{-148,431,-147,43,-151,45,-152,48,-293,53,-150,54,-191,144});
    states[431] = new State(-808);
    states[432] = new State(-810);
    states[433] = new State(new int[]{123,218},new int[]{-299,434});
    states[434] = new State(-811);
    states[435] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,42,440,8,442,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552},new int[]{-111,436,-116,437,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720});
    states[436] = new State(new int[]{8,421,7,430,142,432,4,433,15,435,110,-776,111,-776,112,-776,113,-776,114,-776,92,-776,10,-776,98,-776,101,-776,33,-776,104,-776,2,-776,138,-776,136,-776,118,-776,117,-776,131,-776,132,-776,133,-776,134,-776,130,-776,116,-776,115,-776,128,-776,129,-776,126,-776,6,-776,5,-776,120,-776,125,-776,123,-776,121,-776,124,-776,122,-776,137,-776,135,-776,16,-776,70,-776,9,-776,100,-776,12,-776,99,-776,32,-776,85,-776,84,-776,83,-776,82,-776,81,-776,86,-776,13,-776,119,-776,76,-776,51,-776,58,-776,141,-776,143,-776,80,-776,78,-776,160,-776,87,-776,45,-776,42,-776,21,-776,22,-776,144,-776,147,-776,145,-776,146,-776,155,-776,158,-776,157,-776,156,-776,57,-776,91,-776,40,-776,25,-776,97,-776,54,-776,35,-776,55,-776,102,-776,47,-776,36,-776,53,-776,60,-776,74,-776,72,-776,38,-776,71,-776,11,-790,17,-790});
    states[437] = new State(-777);
    states[438] = new State(-796);
    states[439] = new State(-797);
    states[440] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,441,-151,45,-152,48});
    states[441] = new State(-798);
    states[442] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629,53,1471,18,693},new int[]{-87,443,-360,445,-102,450,-101,724,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-359,727,-100,728});
    states[443] = new State(new int[]{9,444,70,28});
    states[444] = new State(-799);
    states[445] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629,53,1471},new int[]{-87,446,-359,448,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[446] = new State(new int[]{9,447,70,28});
    states[447] = new State(-800);
    states[448] = new State(-794);
    states[449] = new State(new int[]{53,675,56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629,18,693},new int[]{-87,443,-360,445,-102,450,-101,724,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-359,727,-100,728});
    states[450] = new State(new int[]{100,451});
    states[451] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,18,693},new int[]{-79,452,-102,1152,-101,1151,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-100,728});
    states[452] = new State(new int[]{100,1149,5,464,10,-1009,9,-1009},new int[]{-325,453});
    states[453] = new State(new int[]{10,456,9,-997},new int[]{-332,454});
    states[454] = new State(new int[]{9,455});
    states[455] = new State(-760);
    states[456] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-327,457,-328,1101,-158,460,-147,819,-151,45,-152,48});
    states[457] = new State(new int[]{10,458,9,-998});
    states[458] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-328,459,-158,460,-147,819,-151,45,-152,48});
    states[459] = new State(-1007);
    states[460] = new State(new int[]{100,462,5,464,10,-1009,9,-1009},new int[]{-325,461});
    states[461] = new State(-1008);
    states[462] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,463,-151,45,-152,48});
    states[463] = new State(-344);
    states[464] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-276,465,-277,466,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[465] = new State(-1010);
    states[466] = new State(-486);
    states[467] = new State(new int[]{9,468,143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-80,354,-78,360,-277,361,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[468] = new State(new int[]{127,350});
    states[469] = new State(-228);
    states[470] = new State(new int[]{13,471,127,472,120,-233,9,-233,100,-233,10,-233,121,-233,8,-233,138,-233,136,-233,118,-233,117,-233,131,-233,132,-233,133,-233,134,-233,130,-233,116,-233,115,-233,128,-233,129,-233,126,-233,6,-233,5,-233,125,-233,123,-233,124,-233,122,-233,137,-233,135,-233,16,-233,70,-233,92,-233,98,-233,101,-233,33,-233,104,-233,2,-233,12,-233,99,-233,32,-233,85,-233,84,-233,83,-233,82,-233,81,-233,86,-233,76,-233,51,-233,58,-233,141,-233,143,-233,80,-233,78,-233,160,-233,87,-233,45,-233,42,-233,21,-233,22,-233,144,-233,147,-233,145,-233,146,-233,155,-233,158,-233,157,-233,156,-233,57,-233,91,-233,40,-233,25,-233,97,-233,54,-233,35,-233,55,-233,102,-233,47,-233,36,-233,53,-233,60,-233,74,-233,72,-233,38,-233,71,-233,110,-233});
    states[471] = new State(-226);
    states[472] = new State(new int[]{8,474,143,44,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-280,473,-273,223,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-282,1468,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,1469,-224,593,-223,594,-301,1470});
    states[473] = new State(-289);
    states[474] = new State(new int[]{9,475,143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-80,354,-78,360,-277,361,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[475] = new State(new int[]{127,350,121,-293,100,-293,120,-293,9,-293,10,-293,8,-293,138,-293,136,-293,118,-293,117,-293,131,-293,132,-293,133,-293,134,-293,130,-293,116,-293,115,-293,128,-293,129,-293,126,-293,6,-293,5,-293,125,-293,123,-293,124,-293,122,-293,137,-293,135,-293,16,-293,70,-293,92,-293,98,-293,101,-293,33,-293,104,-293,2,-293,12,-293,99,-293,32,-293,85,-293,84,-293,83,-293,82,-293,81,-293,86,-293,13,-293,76,-293,51,-293,58,-293,141,-293,143,-293,80,-293,78,-293,160,-293,87,-293,45,-293,42,-293,21,-293,22,-293,144,-293,147,-293,145,-293,146,-293,155,-293,158,-293,157,-293,156,-293,57,-293,91,-293,40,-293,25,-293,97,-293,54,-293,35,-293,55,-293,102,-293,47,-293,36,-293,53,-293,60,-293,74,-293,72,-293,38,-293,71,-293,110,-293});
    states[476] = new State(-229);
    states[477] = new State(-230);
    states[478] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-276,479,-277,466,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[479] = new State(-266);
    states[480] = new State(-231);
    states[481] = new State(-267);
    states[482] = new State(new int[]{11,483,58,1466});
    states[483] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,389,12,-279,100,-279},new int[]{-164,484,-272,1465,-273,1464,-93,225,-106,343,-107,344,-180,385,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312});
    states[484] = new State(new int[]{12,485,100,1462});
    states[485] = new State(new int[]{58,486});
    states[486] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-277,487,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[487] = new State(-273);
    states[488] = new State(-274);
    states[489] = new State(-268);
    states[490] = new State(new int[]{8,1342,23,-315,11,-315,92,-315,84,-315,83,-315,82,-315,81,-315,29,-315,143,-315,85,-315,86,-315,80,-315,78,-315,160,-315,87,-315,62,-315,28,-315,26,-315,44,-315,37,-315,19,-315,30,-315,31,-315,46,-315,27,-315},new int[]{-183,491});
    states[491] = new State(new int[]{23,1333,11,-322,92,-322,84,-322,83,-322,82,-322,81,-322,29,-322,143,-322,85,-322,86,-322,80,-322,78,-322,160,-322,87,-322,62,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322},new int[]{-318,492,-317,1331,-316,1353});
    states[492] = new State(new int[]{11,659,92,-339,84,-339,83,-339,82,-339,81,-339,29,-212,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,62,-212,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-25,493,-32,1254,-34,497,-45,1255,-6,1256,-251,1137,-33,1418,-54,1420,-53,503,-55,1419});
    states[493] = new State(new int[]{92,494,84,1250,83,1251,82,1252,81,1253},new int[]{-7,495});
    states[494] = new State(-297);
    states[495] = new State(new int[]{11,659,92,-339,84,-339,83,-339,82,-339,81,-339,29,-212,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,62,-212,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-32,496,-34,497,-45,1255,-6,1256,-251,1137,-33,1418,-54,1420,-53,503,-55,1419});
    states[496] = new State(-334);
    states[497] = new State(new int[]{10,499,92,-345,84,-345,83,-345,82,-345,81,-345},new int[]{-190,498});
    states[498] = new State(-340);
    states[499] = new State(new int[]{11,659,92,-346,84,-346,83,-346,82,-346,81,-346,29,-212,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,62,-212,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-45,500,-33,501,-6,1256,-251,1137,-54,1420,-53,503,-55,1419});
    states[500] = new State(-348);
    states[501] = new State(new int[]{11,659,92,-342,84,-342,83,-342,82,-342,81,-342,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-54,502,-53,503,-6,504,-251,1137,-55,1419});
    states[502] = new State(-351);
    states[503] = new State(-352);
    states[504] = new State(new int[]{28,1378,26,1379,44,1326,37,1361,19,1381,30,1389,31,1396,11,659,46,1403,27,1412},new int[]{-222,505,-251,506,-219,507,-259,508,-3,509,-230,1380,-228,1314,-225,1325,-229,1360,-227,1387,-215,1400,-216,1401,-218,1402});
    states[505] = new State(-361);
    states[506] = new State(-211);
    states[507] = new State(-362);
    states[508] = new State(-378);
    states[509] = new State(new int[]{30,511,19,1196,46,1268,27,1306,44,1326,37,1361},new int[]{-230,510,-216,1195,-228,1314,-225,1325,-229,1360});
    states[510] = new State(-365);
    states[511] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,8,-377,110,-377,10,-377},new int[]{-172,512,-171,1178,-170,1179,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[512] = new State(new int[]{8,597,110,-470,10,-470},new int[]{-128,513});
    states[513] = new State(new int[]{110,515,10,1167},new int[]{-207,514});
    states[514] = new State(-374);
    states[515] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494},new int[]{-261,516,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[516] = new State(new int[]{10,517});
    states[517] = new State(-423);
    states[518] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,92,-574,10,-574,98,-574,101,-574,33,-574,104,-574,2,-574,9,-574,100,-574,12,-574,99,-574,32,-574,84,-574,83,-574,82,-574,81,-574},new int[]{-147,441,-151,45,-152,48});
    states[519] = new State(new int[]{53,520,56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629,18,693},new int[]{-87,443,-360,445,-102,450,-111,711,-101,724,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-359,727,-100,728});
    states[520] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,521,-151,45,-152,48});
    states[521] = new State(new int[]{110,522,100,1157});
    states[522] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-99,523,-87,525,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-101,182,-241,611,-117,612,-243,619,-120,628});
    states[523] = new State(new int[]{9,524,16,31,10,-608,70,-608,13,-611});
    states[524] = new State(-781);
    states[525] = new State(new int[]{10,526,70,28});
    states[526] = new State(-792);
    states[527] = new State(-801);
    states[528] = new State(-802);
    states[529] = new State(new int[]{11,530,17,1153});
    states[530] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,673,8,674,21,323,22,328,76,552,40,620,5,629,18,693,37,702,44,706},new int[]{-71,531,-88,672,-87,27,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-323,843,-100,683,-324,701});
    states[531] = new State(new int[]{12,532,100,533});
    states[532] = new State(-804);
    states[533] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,673,8,674,21,323,22,328,76,552,40,620,5,629,18,693,37,702,44,706},new int[]{-88,534,-87,27,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-323,843,-100,683,-324,701});
    states[534] = new State(-593);
    states[535] = new State(new int[]{127,536,8,-796,7,-796,142,-796,4,-796,15,-796,138,-796,136,-796,118,-796,117,-796,131,-796,132,-796,133,-796,134,-796,130,-796,116,-796,115,-796,128,-796,129,-796,126,-796,6,-796,5,-796,120,-796,125,-796,123,-796,121,-796,124,-796,122,-796,137,-796,135,-796,16,-796,70,-796,92,-796,10,-796,98,-796,101,-796,33,-796,104,-796,2,-796,9,-796,100,-796,12,-796,99,-796,32,-796,85,-796,84,-796,83,-796,82,-796,81,-796,86,-796,13,-796,119,-796,11,-796,17,-796});
    states[536] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,541,21,323,22,328,76,552,18,693,37,702,44,706,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-329,537,-104,538,-99,539,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,681,-117,612,-323,682,-100,683,-324,701,-331,868,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[537] = new State(-989);
    states[538] = new State(-1026);
    states[539] = new State(new int[]{16,31,92,-617,10,-617,98,-617,101,-617,33,-617,104,-617,2,-617,9,-617,100,-617,12,-617,99,-617,32,-617,85,-617,84,-617,83,-617,82,-617,81,-617,86,-617,13,-611});
    states[540] = new State(new int[]{6,35,120,-645,125,-645,123,-645,121,-645,124,-645,122,-645,137,-645,135,-645,16,-645,70,-645,92,-645,10,-645,98,-645,101,-645,33,-645,104,-645,2,-645,9,-645,100,-645,12,-645,99,-645,32,-645,85,-645,84,-645,83,-645,82,-645,81,-645,86,-645,76,-645,13,-645,5,-645,51,-645,58,-645,141,-645,143,-645,80,-645,78,-645,160,-645,87,-645,45,-645,42,-645,8,-645,21,-645,22,-645,144,-645,147,-645,145,-645,146,-645,155,-645,158,-645,157,-645,156,-645,57,-645,91,-645,40,-645,25,-645,97,-645,54,-645,35,-645,55,-645,102,-645,47,-645,36,-645,53,-645,60,-645,74,-645,72,-645,38,-645,71,-645,116,-645,115,-645,128,-645,129,-645,126,-645,138,-645,136,-645,118,-645,117,-645,131,-645,132,-645,133,-645,134,-645,130,-645});
    states[541] = new State(new int[]{53,675,9,677,56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,726,21,323,22,328,76,552,40,620,5,629,18,693},new int[]{-87,443,-360,445,-102,542,-147,1112,-4,722,-101,724,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,725,-132,419,-111,429,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-359,727,-100,728});
    states[542] = new State(new int[]{100,543});
    states[543] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,18,693},new int[]{-79,544,-102,1152,-101,1151,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-100,728});
    states[544] = new State(new int[]{100,1149,5,464,10,-1009,9,-1009},new int[]{-325,545});
    states[545] = new State(new int[]{10,456,9,-997},new int[]{-332,546});
    states[546] = new State(new int[]{9,547});
    states[547] = new State(new int[]{5,685,7,-760,8,-760,138,-760,136,-760,118,-760,117,-760,131,-760,132,-760,133,-760,134,-760,130,-760,116,-760,115,-760,128,-760,129,-760,126,-760,6,-760,120,-760,125,-760,123,-760,121,-760,124,-760,122,-760,137,-760,135,-760,16,-760,70,-760,92,-760,10,-760,98,-760,101,-760,33,-760,104,-760,2,-760,9,-760,100,-760,12,-760,99,-760,32,-760,85,-760,84,-760,83,-760,82,-760,81,-760,86,-760,13,-760,127,-1011},new int[]{-336,548,-326,549});
    states[548] = new State(-994);
    states[549] = new State(new int[]{127,550});
    states[550] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,541,21,323,22,328,76,552,18,693,37,702,44,706,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-329,551,-104,538,-99,539,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,681,-117,612,-323,682,-100,683,-324,701,-331,868,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[551] = new State(-999);
    states[552] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-69,553,-77,404,-92,555,-87,407,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[553] = new State(new int[]{76,554});
    states[554] = new State(-806);
    states[555] = new State(-818);
    states[556] = new State(-807);
    states[557] = new State(new int[]{7,558,8,-773,138,-773,136,-773,118,-773,117,-773,131,-773,132,-773,133,-773,134,-773,130,-773,116,-773,115,-773,128,-773,129,-773,126,-773,6,-773,5,-773,120,-773,125,-773,123,-773,121,-773,124,-773,122,-773,137,-773,135,-773,16,-773,70,-773,92,-773,10,-773,98,-773,101,-773,33,-773,104,-773,2,-773,9,-773,100,-773,12,-773,99,-773,32,-773,85,-773,84,-773,83,-773,82,-773,81,-773,86,-773,13,-773,76,-773,51,-773,58,-773,141,-773,143,-773,80,-773,78,-773,160,-773,87,-773,45,-773,42,-773,21,-773,22,-773,144,-773,147,-773,145,-773,146,-773,155,-773,158,-773,157,-773,156,-773,57,-773,91,-773,40,-773,25,-773,97,-773,54,-773,35,-773,55,-773,102,-773,47,-773,36,-773,53,-773,60,-773,74,-773,72,-773,38,-773,71,-773});
    states[558] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,84,55,83,56,82,57,81,58,68,59,64,60,128,61,22,62,21,63,63,64,23,65,129,66,130,67,131,68,132,69,133,70,134,71,135,72,136,73,137,74,138,75,24,76,73,77,91,78,25,79,26,80,29,81,30,82,31,83,71,84,99,85,32,86,92,87,33,88,34,89,27,90,104,91,101,92,35,93,36,94,37,95,40,96,41,97,42,98,103,99,43,100,44,101,46,102,47,103,48,104,97,105,49,106,102,107,50,108,28,109,51,110,70,111,98,112,52,113,53,114,54,115,55,116,56,117,57,118,58,119,59,120,61,121,105,122,106,123,109,124,107,125,108,126,62,127,74,128,38,129,39,130,69,131,148,132,60,133,139,134,140,135,79,136,153,137,152,138,72,139,154,140,150,141,151,142,149,143,45,145},new int[]{-148,559,-147,43,-151,45,-152,48,-293,53,-150,54,-191,144});
    states[559] = new State(-809);
    states[560] = new State(-780);
    states[561] = new State(-746);
    states[562] = new State(-747);
    states[563] = new State(new int[]{119,564});
    states[564] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552},new int[]{-96,565,-269,566,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-97,563});
    states[565] = new State(new int[]{7,41,8,175,138,-742,136,-742,118,-742,117,-742,131,-742,132,-742,133,-742,134,-742,130,-742,116,-742,115,-742,128,-742,129,-742,126,-742,6,-742,5,-742,120,-742,125,-742,123,-742,121,-742,124,-742,122,-742,137,-742,135,-742,16,-742,70,-742,92,-742,10,-742,98,-742,101,-742,33,-742,104,-742,2,-742,9,-742,100,-742,12,-742,99,-742,32,-742,85,-742,84,-742,83,-742,82,-742,81,-742,86,-742,13,-742,76,-742,51,-742,58,-742,141,-742,143,-742,80,-742,78,-742,160,-742,87,-742,45,-742,42,-742,21,-742,22,-742,144,-742,147,-742,145,-742,146,-742,155,-742,158,-742,157,-742,156,-742,57,-742,91,-742,40,-742,25,-742,97,-742,54,-742,35,-742,55,-742,102,-742,47,-742,36,-742,53,-742,60,-742,74,-742,72,-742,38,-742,71,-742});
    states[566] = new State(-743);
    states[567] = new State(-751);
    states[568] = new State(new int[]{8,569,138,-736,136,-736,118,-736,117,-736,131,-736,132,-736,133,-736,134,-736,130,-736,116,-736,115,-736,128,-736,129,-736,126,-736,6,-736,5,-736,120,-736,125,-736,123,-736,121,-736,124,-736,122,-736,137,-736,135,-736,16,-736,70,-736,92,-736,10,-736,98,-736,101,-736,33,-736,104,-736,2,-736,9,-736,100,-736,12,-736,99,-736,32,-736,85,-736,84,-736,83,-736,82,-736,81,-736,86,-736,13,-736,76,-736,51,-736,58,-736,141,-736,143,-736,80,-736,78,-736,160,-736,87,-736,45,-736,42,-736,21,-736,22,-736,144,-736,147,-736,145,-736,146,-736,155,-736,158,-736,157,-736,156,-736,57,-736,91,-736,40,-736,25,-736,97,-736,54,-736,35,-736,55,-736,102,-736,47,-736,36,-736,53,-736,60,-736,74,-736,72,-736,38,-736,71,-736});
    states[569] = new State(new int[]{14,574,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,53,576,143,44,85,46,86,47,80,49,78,50,160,51,87,52,11,936,8,949},new int[]{-354,570,-352,1148,-15,575,-165,369,-167,308,-166,312,-16,370,-341,1139,-284,1140,-180,208,-147,249,-151,45,-152,48,-344,1146,-345,1147});
    states[570] = new State(new int[]{9,571,10,572,100,1144});
    states[571] = new State(-648);
    states[572] = new State(new int[]{14,574,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,53,576,143,44,85,46,86,47,80,49,78,50,160,51,87,52,11,936,8,949},new int[]{-352,573,-15,575,-165,369,-167,308,-166,312,-16,370,-341,1139,-284,1140,-180,208,-147,249,-151,45,-152,48,-344,1146,-345,1147});
    states[573] = new State(-685);
    states[574] = new State(-687);
    states[575] = new State(-688);
    states[576] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,577,-151,45,-152,48});
    states[577] = new State(new int[]{5,578,9,-690,10,-690,100,-690});
    states[578] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-277,579,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[579] = new State(-689);
    states[580] = new State(-269);
    states[581] = new State(new int[]{58,582});
    states[582] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-277,583,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[583] = new State(-280);
    states[584] = new State(-270);
    states[585] = new State(new int[]{58,586,121,-282,100,-282,120,-282,9,-282,10,-282,127,-282,8,-282,138,-282,136,-282,118,-282,117,-282,131,-282,132,-282,133,-282,134,-282,130,-282,116,-282,115,-282,128,-282,129,-282,126,-282,6,-282,5,-282,125,-282,123,-282,124,-282,122,-282,137,-282,135,-282,16,-282,70,-282,92,-282,98,-282,101,-282,33,-282,104,-282,2,-282,12,-282,99,-282,32,-282,85,-282,84,-282,83,-282,82,-282,81,-282,86,-282,13,-282,76,-282,51,-282,141,-282,143,-282,80,-282,78,-282,160,-282,87,-282,45,-282,42,-282,21,-282,22,-282,144,-282,147,-282,145,-282,146,-282,155,-282,158,-282,157,-282,156,-282,57,-282,91,-282,40,-282,25,-282,97,-282,54,-282,35,-282,55,-282,102,-282,47,-282,36,-282,53,-282,60,-282,74,-282,72,-282,38,-282,71,-282,110,-282});
    states[586] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-277,587,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[587] = new State(-281);
    states[588] = new State(-271);
    states[589] = new State(new int[]{58,590});
    states[590] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-277,591,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[591] = new State(-272);
    states[592] = new State(-232);
    states[593] = new State(-284);
    states[594] = new State(-285);
    states[595] = new State(new int[]{8,597,121,-470,100,-470,120,-470,9,-470,10,-470,127,-470,138,-470,136,-470,118,-470,117,-470,131,-470,132,-470,133,-470,134,-470,130,-470,116,-470,115,-470,128,-470,129,-470,126,-470,6,-470,5,-470,125,-470,123,-470,124,-470,122,-470,137,-470,135,-470,16,-470,70,-470,92,-470,98,-470,101,-470,33,-470,104,-470,2,-470,12,-470,99,-470,32,-470,85,-470,84,-470,83,-470,82,-470,81,-470,86,-470,13,-470,76,-470,51,-470,58,-470,141,-470,143,-470,80,-470,78,-470,160,-470,87,-470,45,-470,42,-470,21,-470,22,-470,144,-470,147,-470,145,-470,146,-470,155,-470,158,-470,157,-470,156,-470,57,-470,91,-470,40,-470,25,-470,97,-470,54,-470,35,-470,55,-470,102,-470,47,-470,36,-470,53,-470,60,-470,74,-470,72,-470,38,-470,71,-470,110,-470},new int[]{-128,596});
    states[596] = new State(-286);
    states[597] = new State(new int[]{9,598,11,659,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,53,-212,29,-212,108,-212},new int[]{-129,599,-56,1138,-6,603,-251,1137});
    states[598] = new State(-471);
    states[599] = new State(new int[]{9,600,10,601});
    states[600] = new State(-472);
    states[601] = new State(new int[]{11,659,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,53,-212,29,-212,108,-212},new int[]{-56,602,-6,603,-251,1137});
    states[602] = new State(-474);
    states[603] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,53,643,29,649,108,655,11,659},new int[]{-296,604,-251,506,-159,605,-135,642,-147,641,-151,45,-152,48});
    states[604] = new State(-475);
    states[605] = new State(new int[]{5,606,100,639});
    states[606] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-276,607,-277,466,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[607] = new State(new int[]{110,608,9,-476,10,-476});
    states[608] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-87,609,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[609] = new State(new int[]{70,28,9,-480,10,-480});
    states[610] = new State(-737);
    states[611] = new State(new int[]{70,-609,92,-609,10,-609,98,-609,101,-609,33,-609,104,-609,2,-609,9,-609,100,-609,12,-609,99,-609,32,-609,85,-609,84,-609,83,-609,82,-609,81,-609,86,-609,6,-609,76,-609,5,-609,51,-609,58,-609,141,-609,143,-609,80,-609,78,-609,160,-609,87,-609,45,-609,42,-609,8,-609,21,-609,22,-609,144,-609,147,-609,145,-609,146,-609,155,-609,158,-609,157,-609,156,-609,57,-609,91,-609,40,-609,25,-609,97,-609,54,-609,35,-609,55,-609,102,-609,47,-609,36,-609,53,-609,60,-609,74,-609,72,-609,38,-609,71,-609,13,-612});
    states[612] = new State(new int[]{13,613});
    states[613] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552},new int[]{-117,614,-99,617,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,618});
    states[614] = new State(new int[]{5,615,13,613});
    states[615] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552},new int[]{-117,616,-99,617,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,618});
    states[616] = new State(new int[]{13,613,70,-625,92,-625,10,-625,98,-625,101,-625,33,-625,104,-625,2,-625,9,-625,100,-625,12,-625,99,-625,32,-625,85,-625,84,-625,83,-625,82,-625,81,-625,86,-625,6,-625,76,-625,5,-625,51,-625,58,-625,141,-625,143,-625,80,-625,78,-625,160,-625,87,-625,45,-625,42,-625,8,-625,21,-625,22,-625,144,-625,147,-625,145,-625,146,-625,155,-625,158,-625,157,-625,156,-625,57,-625,91,-625,40,-625,25,-625,97,-625,54,-625,35,-625,55,-625,102,-625,47,-625,36,-625,53,-625,60,-625,74,-625,72,-625,38,-625,71,-625});
    states[617] = new State(new int[]{16,31,5,-611,13,-611,70,-611,92,-611,10,-611,98,-611,101,-611,33,-611,104,-611,2,-611,9,-611,100,-611,12,-611,99,-611,32,-611,85,-611,84,-611,83,-611,82,-611,81,-611,86,-611,6,-611,76,-611,51,-611,58,-611,141,-611,143,-611,80,-611,78,-611,160,-611,87,-611,45,-611,42,-611,8,-611,21,-611,22,-611,144,-611,147,-611,145,-611,146,-611,155,-611,158,-611,157,-611,156,-611,57,-611,91,-611,40,-611,25,-611,97,-611,54,-611,35,-611,55,-611,102,-611,47,-611,36,-611,53,-611,60,-611,74,-611,72,-611,38,-611,71,-611});
    states[618] = new State(-612);
    states[619] = new State(-610);
    states[620] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-118,621,-99,626,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-243,627});
    states[621] = new State(new int[]{51,622});
    states[622] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-118,623,-99,626,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-243,627});
    states[623] = new State(new int[]{32,624});
    states[624] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-118,625,-99,626,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-243,627});
    states[625] = new State(-626);
    states[626] = new State(new int[]{16,31,51,-613,32,-613,120,-613,125,-613,123,-613,121,-613,124,-613,122,-613,137,-613,135,-613,70,-613,92,-613,10,-613,98,-613,101,-613,33,-613,104,-613,2,-613,9,-613,100,-613,12,-613,99,-613,85,-613,84,-613,83,-613,82,-613,81,-613,86,-613,13,-613,6,-613,76,-613,5,-613,58,-613,141,-613,143,-613,80,-613,78,-613,160,-613,87,-613,45,-613,42,-613,8,-613,21,-613,22,-613,144,-613,147,-613,145,-613,146,-613,155,-613,158,-613,157,-613,156,-613,57,-613,91,-613,40,-613,25,-613,97,-613,54,-613,35,-613,55,-613,102,-613,47,-613,36,-613,53,-613,60,-613,74,-613,72,-613,38,-613,71,-613,116,-613,115,-613,128,-613,129,-613,126,-613,138,-613,136,-613,118,-613,117,-613,131,-613,132,-613,133,-613,134,-613,130,-613});
    states[627] = new State(-614);
    states[628] = new State(-606);
    states[629] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,5,-705,70,-705,92,-705,10,-705,98,-705,101,-705,33,-705,104,-705,2,-705,9,-705,100,-705,12,-705,99,-705,32,-705,84,-705,83,-705,82,-705,81,-705,6,-705},new int[]{-115,630,-105,634,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,633,-268,610});
    states[630] = new State(new int[]{5,631,70,-709,92,-709,10,-709,98,-709,101,-709,33,-709,104,-709,2,-709,9,-709,100,-709,12,-709,99,-709,32,-709,85,-709,84,-709,83,-709,82,-709,81,-709,86,-709,6,-709,76,-709});
    states[631] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552},new int[]{-105,632,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,633,-268,610});
    states[632] = new State(new int[]{6,35,70,-711,92,-711,10,-711,98,-711,101,-711,33,-711,104,-711,2,-711,9,-711,100,-711,12,-711,99,-711,32,-711,85,-711,84,-711,83,-711,82,-711,81,-711,86,-711,76,-711});
    states[633] = new State(-736);
    states[634] = new State(new int[]{6,35,5,-704,70,-704,92,-704,10,-704,98,-704,101,-704,33,-704,104,-704,2,-704,9,-704,100,-704,12,-704,99,-704,32,-704,85,-704,84,-704,83,-704,82,-704,81,-704,86,-704,76,-704});
    states[635] = new State(new int[]{8,597,5,-470},new int[]{-128,636});
    states[636] = new State(new int[]{5,637});
    states[637] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-276,638,-277,466,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[638] = new State(-287);
    states[639] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-135,640,-147,641,-151,45,-152,48});
    states[640] = new State(-484);
    states[641] = new State(-485);
    states[642] = new State(-483);
    states[643] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-159,644,-135,642,-147,641,-151,45,-152,48});
    states[644] = new State(new int[]{5,645,100,639});
    states[645] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-276,646,-277,466,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[646] = new State(new int[]{110,647,9,-477,10,-477});
    states[647] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-87,648,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[648] = new State(new int[]{70,28,9,-481,10,-481});
    states[649] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-159,650,-135,642,-147,641,-151,45,-152,48});
    states[650] = new State(new int[]{5,651,100,639});
    states[651] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-276,652,-277,466,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[652] = new State(new int[]{110,653,9,-478,10,-478});
    states[653] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-87,654,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[654] = new State(new int[]{70,28,9,-482,10,-482});
    states[655] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-159,656,-135,642,-147,641,-151,45,-152,48});
    states[656] = new State(new int[]{5,657,100,639});
    states[657] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-276,658,-277,466,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[658] = new State(-479);
    states[659] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-252,660,-8,1136,-9,664,-180,665,-147,1131,-151,45,-152,48,-301,1134});
    states[660] = new State(new int[]{12,661,100,662});
    states[661] = new State(-213);
    states[662] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-8,663,-9,664,-180,665,-147,1131,-151,45,-152,48,-301,1134});
    states[663] = new State(-215);
    states[664] = new State(-216);
    states[665] = new State(new int[]{7,209,8,668,123,218,12,-643,100,-643},new int[]{-70,666,-299,667});
    states[666] = new State(-784);
    states[667] = new State(-234);
    states[668] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,673,8,674,21,323,22,328,76,552,40,620,5,629,18,693,37,702,44,706,9,-813},new int[]{-67,669,-71,671,-88,672,-87,27,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-323,843,-100,683,-324,701});
    states[669] = new State(new int[]{9,670});
    states[670] = new State(-644);
    states[671] = new State(new int[]{100,533,12,-812,9,-812});
    states[672] = new State(-592);
    states[673] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,92,-600,10,-600,98,-600,101,-600,33,-600,104,-600,2,-600,9,-600,100,-600,12,-600,99,-600,32,-600,84,-600,83,-600,82,-600,81,-600},new int[]{-147,441,-151,45,-152,48});
    states[674] = new State(new int[]{53,675,9,677,56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629,18,693},new int[]{-87,443,-360,445,-102,542,-147,1112,-101,724,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-359,727,-100,728});
    states[675] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,676,-151,45,-152,48});
    states[676] = new State(new int[]{110,522});
    states[677] = new State(new int[]{5,685,127,-1011},new int[]{-326,678});
    states[678] = new State(new int[]{127,679});
    states[679] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,541,21,323,22,328,76,552,18,693,37,702,44,706,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-329,680,-104,538,-99,539,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,681,-117,612,-323,682,-100,683,-324,701,-331,868,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[680] = new State(-990);
    states[681] = new State(new int[]{92,-618,10,-618,98,-618,101,-618,33,-618,104,-618,2,-618,9,-618,100,-618,12,-618,99,-618,32,-618,85,-618,84,-618,83,-618,82,-618,81,-618,86,-618,13,-612});
    states[682] = new State(-619);
    states[683] = new State(new int[]{5,685,127,-1011},new int[]{-336,684,-326,549});
    states[684] = new State(-995);
    states[685] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,389,142,478,24,482,48,490,49,581,34,585,73,589},new int[]{-278,686,-273,687,-93,225,-106,343,-107,344,-180,688,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-257,689,-250,690,-282,691,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-301,692});
    states[686] = new State(-1012);
    states[687] = new State(-487);
    states[688] = new State(new int[]{7,209,123,218,8,-255,118,-255,117,-255,131,-255,132,-255,133,-255,134,-255,130,-255,6,-255,116,-255,115,-255,128,-255,129,-255,127,-255},new int[]{-299,667});
    states[689] = new State(-488);
    states[690] = new State(-489);
    states[691] = new State(-490);
    states[692] = new State(-491);
    states[693] = new State(new int[]{18,693,143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-24,694,-23,700,-100,698,-147,699,-151,45,-152,48});
    states[694] = new State(new int[]{100,695});
    states[695] = new State(new int[]{18,693,143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-23,696,-100,698,-147,699,-151,45,-152,48});
    states[696] = new State(new int[]{9,697,100,-984});
    states[697] = new State(-980);
    states[698] = new State(-981);
    states[699] = new State(-982);
    states[700] = new State(-983);
    states[701] = new State(-996);
    states[702] = new State(new int[]{8,1102,5,685,127,-1011},new int[]{-326,703});
    states[703] = new State(new int[]{127,704});
    states[704] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,541,21,323,22,328,76,552,18,693,37,702,44,706,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-329,705,-104,538,-99,539,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,681,-117,612,-323,682,-100,683,-324,701,-331,868,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[705] = new State(-1000);
    states[706] = new State(new int[]{127,707,8,1093});
    states[707] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,42,440,8,710,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-330,708,-212,709,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-4,729,-331,730,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[708] = new State(-1003);
    states[709] = new State(-1028);
    states[710] = new State(new int[]{53,675,56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,726,21,323,22,328,76,552,40,620,5,629,18,693},new int[]{-87,443,-360,445,-102,450,-111,711,-4,722,-101,724,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,725,-132,419,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-359,727,-100,728});
    states[711] = new State(new int[]{100,712,8,421,7,430,142,432,4,433,15,435,138,-779,136,-779,118,-779,117,-779,131,-779,132,-779,133,-779,134,-779,130,-779,116,-779,115,-779,128,-779,129,-779,126,-779,6,-779,5,-779,120,-779,125,-779,123,-779,121,-779,124,-779,122,-779,137,-779,135,-779,16,-779,9,-779,70,-779,13,-779,119,-779,110,-779,111,-779,112,-779,113,-779,114,-779,11,-790,17,-790});
    states[712] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,42,440,8,442,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552},new int[]{-337,713,-111,721,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720});
    states[713] = new State(new int[]{9,714,100,717});
    states[714] = new State(new int[]{110,169,111,170,112,171,113,172,114,173},new int[]{-194,715});
    states[715] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-87,716,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[716] = new State(new int[]{70,28,92,-523,10,-523,98,-523,101,-523,33,-523,104,-523,2,-523,9,-523,100,-523,12,-523,99,-523,32,-523,85,-523,84,-523,83,-523,82,-523,81,-523,86,-523});
    states[717] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,42,440,8,442,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552},new int[]{-111,718,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720});
    states[718] = new State(new int[]{8,421,7,430,142,432,4,433,9,-525,100,-525,11,-790,17,-790});
    states[719] = new State(new int[]{7,367,11,-791,17,-791});
    states[720] = new State(new int[]{7,558});
    states[721] = new State(new int[]{8,421,7,430,142,432,4,433,9,-524,100,-524,11,-790,17,-790});
    states[722] = new State(new int[]{9,723});
    states[723] = new State(-1025);
    states[724] = new State(new int[]{9,-605,70,-605,100,-985});
    states[725] = new State(new int[]{110,169,111,170,112,171,113,172,114,173,7,-772,8,-772,138,-772,136,-772,118,-772,117,-772,131,-772,132,-772,133,-772,134,-772,130,-772,116,-772,115,-772,128,-772,129,-772,126,-772,6,-772,5,-772,120,-772,125,-772,123,-772,121,-772,124,-772,122,-772,137,-772,135,-772,16,-772,9,-772,70,-772,100,-772,13,-772,2,-772,119,-762},new int[]{-194,25});
    states[726] = new State(new int[]{53,675,56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629,18,693},new int[]{-87,443,-360,445,-102,450,-111,711,-101,724,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-359,727,-100,728});
    states[727] = new State(-793);
    states[728] = new State(-986);
    states[729] = new State(-1029);
    states[730] = new State(-1030);
    states[731] = new State(-1013);
    states[732] = new State(-1014);
    states[733] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,734,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[734] = new State(new int[]{51,735});
    states[735] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-261,736,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[736] = new State(new int[]{32,737,92,-533,10,-533,98,-533,101,-533,33,-533,104,-533,2,-533,9,-533,100,-533,12,-533,99,-533,85,-533,84,-533,83,-533,82,-533,81,-533,86,-533});
    states[737] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-261,738,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[738] = new State(-534);
    states[739] = new State(-496);
    states[740] = new State(-497);
    states[741] = new State(new int[]{155,743,143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-143,742,-147,744,-151,45,-152,48});
    states[742] = new State(-529);
    states[743] = new State(-99);
    states[744] = new State(-100);
    states[745] = new State(-498);
    states[746] = new State(-499);
    states[747] = new State(-500);
    states[748] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,749,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[749] = new State(new int[]{58,750});
    states[750] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372,32,758,92,-553},new int[]{-36,751,-254,1090,-263,1092,-74,1083,-110,1089,-94,1088,-90,236,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[751] = new State(new int[]{10,754,32,758,92,-553},new int[]{-254,752});
    states[752] = new State(new int[]{92,753});
    states[753] = new State(-544);
    states[754] = new State(new int[]{32,758,143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372,92,-553},new int[]{-254,755,-263,757,-74,1083,-110,1089,-94,1088,-90,236,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[755] = new State(new int[]{92,756});
    states[756] = new State(-545);
    states[757] = new State(-548);
    states[758] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,762,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494,92,-494},new int[]{-253,759,-262,760,-261,22,-4,23,-113,24,-132,419,-111,429,-147,761,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895,-143,1050});
    states[759] = new State(new int[]{10,20,92,-554});
    states[760] = new State(-531);
    states[761] = new State(new int[]{8,-796,7,-796,142,-796,4,-796,15,-796,110,-796,111,-796,112,-796,113,-796,114,-796,92,-796,10,-796,11,-796,17,-796,98,-796,101,-796,33,-796,104,-796,2,-796,5,-100});
    states[762] = new State(new int[]{7,-190,11,-190,17,-190,5,-99});
    states[763] = new State(-501);
    states[764] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,762,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,98,-494,10,-494},new int[]{-253,765,-262,760,-261,22,-4,23,-113,24,-132,419,-111,429,-147,761,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895,-143,1050});
    states[765] = new State(new int[]{98,766,10,20});
    states[766] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-87,767,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[767] = new State(new int[]{70,28,92,-555,10,-555,98,-555,101,-555,33,-555,104,-555,2,-555,9,-555,100,-555,12,-555,99,-555,32,-555,85,-555,84,-555,83,-555,82,-555,81,-555,86,-555});
    states[768] = new State(-502);
    states[769] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,770,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[770] = new State(new int[]{99,1079,141,-558,143,-558,85,-558,86,-558,80,-558,78,-558,160,-558,87,-558,45,-558,42,-558,8,-558,21,-558,22,-558,144,-558,147,-558,145,-558,146,-558,155,-558,158,-558,157,-558,156,-558,76,-558,57,-558,91,-558,40,-558,25,-558,97,-558,54,-558,35,-558,55,-558,102,-558,47,-558,36,-558,53,-558,60,-558,74,-558,72,-558,38,-558,92,-558,10,-558,98,-558,101,-558,33,-558,104,-558,2,-558,9,-558,100,-558,12,-558,32,-558,84,-558,83,-558,82,-558,81,-558},new int[]{-292,771});
    states[771] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-261,772,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[772] = new State(-556);
    states[773] = new State(-503);
    states[774] = new State(new int[]{53,1082,143,-565,85,-565,86,-565,80,-565,78,-565,160,-565,87,-565},new int[]{-19,775});
    states[775] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,776,-151,45,-152,48});
    states[776] = new State(new int[]{5,1032,110,-563},new int[]{-275,777});
    states[777] = new State(new int[]{110,778});
    states[778] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,779,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[779] = new State(new int[]{70,1080,71,1081},new int[]{-119,780});
    states[780] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,781,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[781] = new State(new int[]{160,1075,99,1079,141,-558,143,-558,85,-558,86,-558,80,-558,78,-558,87,-558,45,-558,42,-558,8,-558,21,-558,22,-558,144,-558,147,-558,145,-558,146,-558,155,-558,158,-558,157,-558,156,-558,76,-558,57,-558,91,-558,40,-558,25,-558,97,-558,54,-558,35,-558,55,-558,102,-558,47,-558,36,-558,53,-558,60,-558,74,-558,72,-558,38,-558,92,-558,10,-558,98,-558,101,-558,33,-558,104,-558,2,-558,9,-558,100,-558,12,-558,32,-558,84,-558,83,-558,82,-558,81,-558},new int[]{-292,782});
    states[782] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-261,783,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[783] = new State(-571);
    states[784] = new State(-504);
    states[785] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,673,8,674,21,323,22,328,76,552,40,620,5,629,18,693,37,702,44,706},new int[]{-71,786,-88,672,-87,27,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-323,843,-100,683,-324,701});
    states[786] = new State(new int[]{99,787,100,533});
    states[787] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-261,788,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[788] = new State(-573);
    states[789] = new State(-505);
    states[790] = new State(-506);
    states[791] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,762,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494,101,-494,33,-494},new int[]{-253,792,-262,760,-261,22,-4,23,-113,24,-132,419,-111,429,-147,761,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895,-143,1050});
    states[792] = new State(new int[]{10,20,101,794,33,1053},new int[]{-290,793});
    states[793] = new State(-575);
    states[794] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,762,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494},new int[]{-253,795,-262,760,-261,22,-4,23,-113,24,-132,419,-111,429,-147,761,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895,-143,1050});
    states[795] = new State(new int[]{92,796,10,20});
    states[796] = new State(-576);
    states[797] = new State(-507);
    states[798] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629,92,-590,10,-590,98,-590,101,-590,33,-590,104,-590,2,-590,9,-590,100,-590,12,-590,99,-590,32,-590,84,-590,83,-590,82,-590,81,-590},new int[]{-87,799,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[799] = new State(new int[]{70,28,92,-591,10,-591,98,-591,101,-591,33,-591,104,-591,2,-591,9,-591,100,-591,12,-591,99,-591,32,-591,85,-591,84,-591,83,-591,82,-591,81,-591,86,-591});
    states[800] = new State(-508);
    states[801] = new State(new int[]{53,1034,143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,802,-151,45,-152,48});
    states[802] = new State(new int[]{5,1032,137,-563},new int[]{-275,803});
    states[803] = new State(new int[]{137,804});
    states[804] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,805,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[805] = new State(new int[]{87,1030,99,-561},new int[]{-361,806});
    states[806] = new State(new int[]{99,807});
    states[807] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-261,808,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[808] = new State(-568);
    states[809] = new State(-509);
    states[810] = new State(new int[]{8,812,143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-312,811,-158,820,-147,819,-151,45,-152,48});
    states[811] = new State(-519);
    states[812] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,813,-151,45,-152,48});
    states[813] = new State(new int[]{100,814});
    states[814] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-158,815,-147,819,-151,45,-152,48});
    states[815] = new State(new int[]{9,816,100,462});
    states[816] = new State(new int[]{110,817});
    states[817] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-87,818,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[818] = new State(new int[]{70,28,92,-521,10,-521,98,-521,101,-521,33,-521,104,-521,2,-521,9,-521,100,-521,12,-521,99,-521,32,-521,85,-521,84,-521,83,-521,82,-521,81,-521,86,-521});
    states[819] = new State(-343);
    states[820] = new State(new int[]{5,821,100,462,110,1028});
    states[821] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-277,822,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[822] = new State(new int[]{110,1026,120,1027,92,-407,10,-407,98,-407,101,-407,33,-407,104,-407,2,-407,9,-407,100,-407,12,-407,99,-407,32,-407,85,-407,84,-407,83,-407,82,-407,81,-407,86,-407},new int[]{-339,823});
    states[823] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,997,135,850,116,386,115,387,63,372,37,702,44,706,40,620},new int[]{-86,824,-85,825,-84,305,-90,306,-91,267,-81,826,-13,280,-10,290,-14,253,-147,865,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864,-95,1014,-244,1015,-324,1024,-243,1025});
    states[824] = new State(-409);
    states[825] = new State(-410);
    states[826] = new State(new int[]{6,827,116,276,115,277,128,278,129,279,120,-118,125,-118,123,-118,121,-118,124,-118,122,-118,137,-118,13,-118,16,-118,92,-118,10,-118,98,-118,101,-118,33,-118,104,-118,2,-118,9,-118,100,-118,12,-118,99,-118,32,-118,85,-118,84,-118,83,-118,82,-118,81,-118,86,-118},new int[]{-193,245});
    states[827] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-13,828,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858});
    states[828] = new State(new int[]{136,281,138,282,118,283,117,284,131,285,132,286,133,287,134,288,130,289,92,-411,10,-411,98,-411,101,-411,33,-411,104,-411,2,-411,9,-411,100,-411,12,-411,99,-411,32,-411,85,-411,84,-411,83,-411,82,-411,81,-411,86,-411},new int[]{-201,247,-195,250});
    states[829] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-69,830,-77,404,-92,555,-87,407,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[830] = new State(new int[]{76,831});
    states[831] = new State(-166);
    states[832] = new State(-157);
    states[833] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,844,135,850,116,386,115,387,63,372},new int[]{-10,834,-14,835,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,852,-173,854,-57,855});
    states[834] = new State(-158);
    states[835] = new State(new int[]{4,255,11,257,7,836,142,838,8,839,136,-155,138,-155,118,-155,117,-155,131,-155,132,-155,133,-155,134,-155,130,-155,116,-155,115,-155,128,-155,129,-155,120,-155,125,-155,123,-155,121,-155,124,-155,122,-155,137,-155,13,-155,16,-155,6,-155,100,-155,9,-155,12,-155,5,-155,92,-155,10,-155,98,-155,101,-155,33,-155,104,-155,2,-155,99,-155,32,-155,85,-155,84,-155,83,-155,82,-155,81,-155,86,-155},new int[]{-12,254});
    states[836] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,84,55,83,56,82,57,81,58,68,59,64,60,128,61,22,62,21,63,63,64,23,65,129,66,130,67,131,68,132,69,133,70,134,71,135,72,136,73,137,74,138,75,24,76,73,77,91,78,25,79,26,80,29,81,30,82,31,83,71,84,99,85,32,86,92,87,33,88,34,89,27,90,104,91,101,92,35,93,36,94,37,95,40,96,41,97,42,98,103,99,43,100,44,101,46,102,47,103,48,104,97,105,49,106,102,107,50,108,28,109,51,110,70,111,98,112,52,113,53,114,54,115,55,116,56,117,57,118,58,119,59,120,61,121,105,122,106,123,109,124,107,125,108,126,62,127,74,128,38,129,39,130,69,131,148,132,60,133,139,134,140,135,79,136,153,137,152,138,72,139,154,140,150,141,151,142,149,143,45,214},new int[]{-138,837,-147,211,-151,45,-152,48,-293,212,-150,54,-294,213});
    states[837] = new State(-178);
    states[838] = new State(-179);
    states[839] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,673,8,674,21,323,22,328,76,552,40,620,5,629,18,693,37,702,44,706,9,-183},new int[]{-76,840,-71,842,-88,672,-87,27,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-323,843,-100,683,-324,701});
    states[840] = new State(new int[]{9,841});
    states[841] = new State(-180);
    states[842] = new State(new int[]{100,533,9,-182});
    states[843] = new State(-599);
    states[844] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-90,845,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[845] = new State(new int[]{9,846,13,237,16,241});
    states[846] = new State(-159);
    states[847] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-90,848,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[848] = new State(new int[]{9,849,13,237,16,241});
    states[849] = new State(new int[]{136,-159,138,-159,118,-159,117,-159,131,-159,132,-159,133,-159,134,-159,130,-159,116,-159,115,-159,128,-159,129,-159,120,-159,125,-159,123,-159,121,-159,124,-159,122,-159,137,-159,13,-159,16,-159,6,-159,100,-159,9,-159,12,-159,5,-159,92,-159,10,-159,98,-159,101,-159,33,-159,104,-159,2,-159,99,-159,32,-159,85,-159,84,-159,83,-159,82,-159,81,-159,86,-159,119,-154});
    states[850] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,844,135,850,116,386,115,387,63,372},new int[]{-10,851,-14,835,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,852,-173,854,-57,855});
    states[851] = new State(-160);
    states[852] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,844,135,850,116,386,115,387,63,372},new int[]{-10,853,-14,835,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,852,-173,854,-57,855});
    states[853] = new State(-161);
    states[854] = new State(-162);
    states[855] = new State(-163);
    states[856] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-10,853,-270,857,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-11,858});
    states[857] = new State(-140);
    states[858] = new State(new int[]{119,859});
    states[859] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-10,860,-270,861,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-11,858});
    states[860] = new State(-138);
    states[861] = new State(-139);
    states[862] = new State(-142);
    states[863] = new State(-143);
    states[864] = new State(-121);
    states[865] = new State(new int[]{127,866,4,-169,11,-169,7,-169,142,-169,8,-169,136,-169,138,-169,118,-169,117,-169,131,-169,132,-169,133,-169,134,-169,130,-169,6,-169,116,-169,115,-169,128,-169,129,-169,120,-169,125,-169,123,-169,121,-169,124,-169,122,-169,137,-169,13,-169,16,-169,92,-169,10,-169,98,-169,101,-169,33,-169,104,-169,2,-169,9,-169,100,-169,12,-169,99,-169,32,-169,85,-169,84,-169,83,-169,82,-169,81,-169,86,-169,119,-169});
    states[866] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,541,21,323,22,328,76,552,18,693,37,702,44,706,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-329,867,-104,538,-99,539,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,681,-117,612,-323,682,-100,683,-324,701,-331,868,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[867] = new State(-413);
    states[868] = new State(-1027);
    states[869] = new State(-1015);
    states[870] = new State(-1016);
    states[871] = new State(-1017);
    states[872] = new State(-1018);
    states[873] = new State(-1019);
    states[874] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,875,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[875] = new State(new int[]{99,876});
    states[876] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-261,877,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[877] = new State(-516);
    states[878] = new State(-510);
    states[879] = new State(-596);
    states[880] = new State(-597);
    states[881] = new State(-511);
    states[882] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,883,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[883] = new State(new int[]{99,884});
    states[884] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-261,885,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[885] = new State(-559);
    states[886] = new State(-512);
    states[887] = new State(new int[]{73,889,56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,674,21,323,22,328,76,552,40,620,18,693,37,702,44,706},new int[]{-103,888,-101,891,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-323,892,-100,683,-324,701});
    states[888] = new State(-517);
    states[889] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,674,21,323,22,328,76,552,40,620,18,693,37,702,44,706},new int[]{-103,890,-101,891,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-323,892,-100,683,-324,701});
    states[890] = new State(-518);
    states[891] = new State(-615);
    states[892] = new State(-616);
    states[893] = new State(-513);
    states[894] = new State(-514);
    states[895] = new State(-515);
    states[896] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,897,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[897] = new State(new int[]{55,898});
    states[898] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,56,976,21,323,22,328,11,936,8,949},new int[]{-351,899,-350,990,-343,906,-284,911,-180,208,-147,249,-151,45,-152,48,-342,968,-358,971,-340,979,-15,974,-165,369,-167,308,-166,312,-16,370,-258,977,-295,978,-344,980,-345,983});
    states[899] = new State(new int[]{10,902,32,758,92,-553},new int[]{-254,900});
    states[900] = new State(new int[]{92,901});
    states[901] = new State(-535);
    states[902] = new State(new int[]{32,758,143,44,85,46,86,47,80,49,78,50,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,56,976,21,323,22,328,11,936,8,949,92,-553},new int[]{-254,903,-350,905,-343,906,-284,911,-180,208,-147,249,-151,45,-152,48,-342,968,-358,971,-340,979,-15,974,-165,369,-167,308,-166,312,-16,370,-258,977,-295,978,-344,980,-345,983});
    states[903] = new State(new int[]{92,904});
    states[904] = new State(-536);
    states[905] = new State(-538);
    states[906] = new State(new int[]{39,907});
    states[907] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,908,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[908] = new State(new int[]{5,909});
    states[909] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494,32,-494,92,-494},new int[]{-261,910,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[910] = new State(-539);
    states[911] = new State(new int[]{8,912,100,-656,5,-656});
    states[912] = new State(new int[]{14,917,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,116,386,115,387,143,44,85,46,86,47,80,49,78,50,160,51,87,52,53,924,11,936,8,949},new int[]{-355,913,-353,967,-15,918,-165,369,-167,308,-166,312,-16,370,-199,919,-147,921,-151,45,-152,48,-343,928,-284,929,-180,208,-344,935,-345,966});
    states[913] = new State(new int[]{9,914,10,915,100,933});
    states[914] = new State(new int[]{39,-650,5,-651});
    states[915] = new State(new int[]{14,917,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,116,386,115,387,143,44,85,46,86,47,80,49,78,50,160,51,87,52,53,924,11,936,8,949},new int[]{-353,916,-15,918,-165,369,-167,308,-166,312,-16,370,-199,919,-147,921,-151,45,-152,48,-343,928,-284,929,-180,208,-344,935,-345,966});
    states[916] = new State(-682);
    states[917] = new State(-694);
    states[918] = new State(-695);
    states[919] = new State(new int[]{144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319},new int[]{-15,920,-165,369,-167,308,-166,312,-16,370});
    states[920] = new State(-696);
    states[921] = new State(new int[]{5,922,9,-698,10,-698,100,-698,7,-260,4,-260,123,-260,8,-260});
    states[922] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-277,923,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[923] = new State(-697);
    states[924] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,925,-151,45,-152,48});
    states[925] = new State(new int[]{5,926,9,-700,10,-700,100,-700});
    states[926] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-277,927,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[927] = new State(-699);
    states[928] = new State(-701);
    states[929] = new State(new int[]{8,930});
    states[930] = new State(new int[]{14,917,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,116,386,115,387,143,44,85,46,86,47,80,49,78,50,160,51,87,52,53,924,11,936,8,949},new int[]{-355,931,-353,967,-15,918,-165,369,-167,308,-166,312,-16,370,-199,919,-147,921,-151,45,-152,48,-343,928,-284,929,-180,208,-344,935,-345,966});
    states[931] = new State(new int[]{9,932,10,915,100,933});
    states[932] = new State(-650);
    states[933] = new State(new int[]{14,917,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,116,386,115,387,143,44,85,46,86,47,80,49,78,50,160,51,87,52,53,924,11,936,8,949},new int[]{-353,934,-15,918,-165,369,-167,308,-166,312,-16,370,-199,919,-147,921,-151,45,-152,48,-343,928,-284,929,-180,208,-344,935,-345,966});
    states[934] = new State(-683);
    states[935] = new State(-702);
    states[936] = new State(new int[]{144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,53,943,14,945,143,44,85,46,86,47,80,49,78,50,160,51,87,52,11,936,8,949,6,964},new int[]{-356,937,-346,965,-15,941,-165,369,-167,308,-166,312,-16,370,-348,942,-343,946,-284,929,-180,208,-147,249,-151,45,-152,48,-344,947,-345,948});
    states[937] = new State(new int[]{12,938,100,939});
    states[938] = new State(-660);
    states[939] = new State(new int[]{144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,53,943,14,945,143,44,85,46,86,47,80,49,78,50,160,51,87,52,11,936,8,949,6,964},new int[]{-346,940,-15,941,-165,369,-167,308,-166,312,-16,370,-348,942,-343,946,-284,929,-180,208,-147,249,-151,45,-152,48,-344,947,-345,948});
    states[940] = new State(-662);
    states[941] = new State(-663);
    states[942] = new State(-664);
    states[943] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,944,-151,45,-152,48});
    states[944] = new State(-670);
    states[945] = new State(-665);
    states[946] = new State(-666);
    states[947] = new State(-667);
    states[948] = new State(-668);
    states[949] = new State(new int[]{14,954,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,116,386,115,387,53,958,143,44,85,46,86,47,80,49,78,50,160,51,87,52,11,936,8,949},new int[]{-357,950,-347,963,-15,955,-165,369,-167,308,-166,312,-16,370,-199,956,-343,960,-284,929,-180,208,-147,249,-151,45,-152,48,-344,961,-345,962});
    states[950] = new State(new int[]{9,951,100,952});
    states[951] = new State(-671);
    states[952] = new State(new int[]{14,954,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,116,386,115,387,53,958,143,44,85,46,86,47,80,49,78,50,160,51,87,52,11,936,8,949},new int[]{-347,953,-15,955,-165,369,-167,308,-166,312,-16,370,-199,956,-343,960,-284,929,-180,208,-147,249,-151,45,-152,48,-344,961,-345,962});
    states[953] = new State(-680);
    states[954] = new State(-672);
    states[955] = new State(-673);
    states[956] = new State(new int[]{144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319},new int[]{-15,957,-165,369,-167,308,-166,312,-16,370});
    states[957] = new State(-674);
    states[958] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,959,-151,45,-152,48});
    states[959] = new State(-675);
    states[960] = new State(-676);
    states[961] = new State(-677);
    states[962] = new State(-678);
    states[963] = new State(-679);
    states[964] = new State(-669);
    states[965] = new State(-661);
    states[966] = new State(-703);
    states[967] = new State(-681);
    states[968] = new State(new int[]{5,969});
    states[969] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494,32,-494,92,-494},new int[]{-261,970,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[970] = new State(-540);
    states[971] = new State(new int[]{100,972,5,-652});
    states[972] = new State(new int[]{144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,143,44,85,46,86,47,80,49,78,50,160,51,87,52,56,976,21,323,22,328},new int[]{-340,973,-15,974,-165,369,-167,308,-166,312,-16,370,-284,975,-180,208,-147,249,-151,45,-152,48,-258,977,-295,978});
    states[973] = new State(-654);
    states[974] = new State(-655);
    states[975] = new State(-656);
    states[976] = new State(-657);
    states[977] = new State(-658);
    states[978] = new State(-659);
    states[979] = new State(-653);
    states[980] = new State(new int[]{5,981});
    states[981] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494,32,-494,92,-494},new int[]{-261,982,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[982] = new State(-541);
    states[983] = new State(new int[]{39,984,5,988});
    states[984] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,985,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[985] = new State(new int[]{5,986});
    states[986] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494,32,-494,92,-494},new int[]{-261,987,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[987] = new State(-542);
    states[988] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494,32,-494,92,-494},new int[]{-261,989,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[989] = new State(-543);
    states[990] = new State(-537);
    states[991] = new State(-1020);
    states[992] = new State(-1021);
    states[993] = new State(-1022);
    states[994] = new State(-1023);
    states[995] = new State(-1024);
    states[996] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,674,21,323,22,328,76,552,40,620,18,693,37,702,44,706},new int[]{-103,888,-101,891,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-323,892,-100,683,-324,701});
    states[997] = new State(new int[]{9,1005,143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,1010,135,850,116,386,115,387,63,372},new int[]{-90,998,-66,999,-246,1003,-91,267,-81,275,-13,280,-10,290,-14,253,-147,1009,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864,-65,302,-85,1013,-84,305,-95,1014,-244,1015,-245,1016,-247,1023,-136,1019});
    states[998] = new State(new int[]{9,849,13,237,16,241,100,-194});
    states[999] = new State(new int[]{9,1000});
    states[1000] = new State(new int[]{127,1001,92,-197,10,-197,98,-197,101,-197,33,-197,104,-197,2,-197,9,-197,100,-197,12,-197,99,-197,32,-197,85,-197,84,-197,83,-197,82,-197,81,-197,86,-197});
    states[1001] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,541,21,323,22,328,76,552,18,693,37,702,44,706,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-329,1002,-104,538,-99,539,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,681,-117,612,-323,682,-100,683,-324,701,-331,868,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[1002] = new State(-415);
    states[1003] = new State(new int[]{9,1004});
    states[1004] = new State(-202);
    states[1005] = new State(new int[]{5,464,127,-1009},new int[]{-325,1006});
    states[1006] = new State(new int[]{127,1007});
    states[1007] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,541,21,323,22,328,76,552,18,693,37,702,44,706,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-329,1008,-104,538,-99,539,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,681,-117,612,-323,682,-100,683,-324,701,-331,868,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[1008] = new State(-414);
    states[1009] = new State(new int[]{4,-169,11,-169,7,-169,142,-169,8,-169,136,-169,138,-169,118,-169,117,-169,131,-169,132,-169,133,-169,134,-169,130,-169,116,-169,115,-169,128,-169,129,-169,120,-169,125,-169,123,-169,121,-169,124,-169,122,-169,137,-169,9,-169,13,-169,16,-169,100,-169,119,-169,5,-208});
    states[1010] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,1010,135,850,116,386,115,387,63,372,9,-198},new int[]{-90,998,-66,1011,-246,1003,-91,267,-81,275,-13,280,-10,290,-14,253,-147,1009,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864,-65,302,-85,1013,-84,305,-95,1014,-244,1015,-245,1016,-247,1023,-136,1019});
    states[1011] = new State(new int[]{9,1012});
    states[1012] = new State(-197);
    states[1013] = new State(-200);
    states[1014] = new State(-195);
    states[1015] = new State(-196);
    states[1016] = new State(new int[]{10,1017,9,-203});
    states[1017] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,9,-204},new int[]{-247,1018,-136,1019,-147,1022,-151,45,-152,48});
    states[1018] = new State(-206);
    states[1019] = new State(new int[]{5,1020});
    states[1020] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,1010,135,850,116,386,115,387,63,372},new int[]{-84,1021,-90,306,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864,-95,1014,-244,1015});
    states[1021] = new State(-207);
    states[1022] = new State(-208);
    states[1023] = new State(-205);
    states[1024] = new State(-412);
    states[1025] = new State(-416);
    states[1026] = new State(-405);
    states[1027] = new State(-406);
    states[1028] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,673,8,674,21,323,22,328,76,552,40,620,5,629,18,693,37,702,44,706},new int[]{-88,1029,-87,27,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-323,843,-100,683,-324,701});
    states[1029] = new State(-408);
    states[1030] = new State(new int[]{143,1031});
    states[1031] = new State(-560);
    states[1032] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-277,1033,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[1033] = new State(-562);
    states[1034] = new State(new int[]{8,1042,143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,1035,-151,45,-152,48});
    states[1035] = new State(new int[]{5,1032,137,-563},new int[]{-275,1036});
    states[1036] = new State(new int[]{137,1037});
    states[1037] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,1038,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[1038] = new State(new int[]{87,1030,99,-561},new int[]{-361,1039});
    states[1039] = new State(new int[]{99,1040});
    states[1040] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-261,1041,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[1041] = new State(-569);
    states[1042] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-158,1043,-147,819,-151,45,-152,48});
    states[1043] = new State(new int[]{9,1044,100,462});
    states[1044] = new State(new int[]{137,1045});
    states[1045] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,1046,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[1046] = new State(new int[]{87,1030,99,-561},new int[]{-361,1047});
    states[1047] = new State(new int[]{99,1048});
    states[1048] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-261,1049,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[1049] = new State(-570);
    states[1050] = new State(new int[]{5,1051});
    states[1051] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,762,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494},new int[]{-262,1052,-261,22,-4,23,-113,24,-132,419,-111,429,-147,761,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895,-143,1050});
    states[1052] = new State(-493);
    states[1053] = new State(new int[]{79,1061,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,762,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494,92,-494},new int[]{-60,1054,-63,1056,-62,1073,-253,1074,-262,760,-261,22,-4,23,-113,24,-132,419,-111,429,-147,761,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895,-143,1050});
    states[1054] = new State(new int[]{92,1055});
    states[1055] = new State(-577);
    states[1056] = new State(new int[]{10,1058,32,1071,92,-583},new int[]{-255,1057});
    states[1057] = new State(-578);
    states[1058] = new State(new int[]{79,1061,32,1071,92,-583},new int[]{-62,1059,-255,1060});
    states[1059] = new State(-582);
    states[1060] = new State(-579);
    states[1061] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-64,1062,-179,1065,-180,1066,-147,1067,-151,45,-152,48,-140,1068});
    states[1062] = new State(new int[]{99,1063});
    states[1063] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494,32,-494,92,-494},new int[]{-261,1064,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[1064] = new State(-585);
    states[1065] = new State(-586);
    states[1066] = new State(new int[]{7,209,99,-588});
    states[1067] = new State(new int[]{7,-260,99,-260,5,-589});
    states[1068] = new State(new int[]{5,1069});
    states[1069] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-179,1070,-180,1066,-147,249,-151,45,-152,48});
    states[1070] = new State(-587);
    states[1071] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,762,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494,92,-494},new int[]{-253,1072,-262,760,-261,22,-4,23,-113,24,-132,419,-111,429,-147,761,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895,-143,1050});
    states[1072] = new State(new int[]{10,20,92,-584});
    states[1073] = new State(-581);
    states[1074] = new State(new int[]{10,20,92,-580});
    states[1075] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,1076,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[1076] = new State(new int[]{99,1077});
    states[1077] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-261,1078,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[1078] = new State(-572);
    states[1079] = new State(-557);
    states[1080] = new State(-566);
    states[1081] = new State(-567);
    states[1082] = new State(-564);
    states[1083] = new State(new int[]{5,1084,100,1086});
    states[1084] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494,32,-494,92,-494},new int[]{-261,1085,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[1085] = new State(-549);
    states[1086] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-110,1087,-94,1088,-90,236,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[1087] = new State(-551);
    states[1088] = new State(-552);
    states[1089] = new State(-550);
    states[1090] = new State(new int[]{92,1091});
    states[1091] = new State(-546);
    states[1092] = new State(-547);
    states[1093] = new State(new int[]{9,1094,143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-327,1097,-328,1101,-158,460,-147,819,-151,45,-152,48});
    states[1094] = new State(new int[]{127,1095});
    states[1095] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,42,440,8,710,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-330,1096,-212,709,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-4,729,-331,730,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[1096] = new State(-1004);
    states[1097] = new State(new int[]{9,1098,10,458});
    states[1098] = new State(new int[]{127,1099});
    states[1099] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,42,440,8,710,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-330,1100,-212,709,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-4,729,-331,730,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[1100] = new State(-1005);
    states[1101] = new State(-1006);
    states[1102] = new State(new int[]{9,1103,143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-327,1107,-328,1101,-158,460,-147,819,-151,45,-152,48});
    states[1103] = new State(new int[]{5,685,127,-1011},new int[]{-326,1104});
    states[1104] = new State(new int[]{127,1105});
    states[1105] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,541,21,323,22,328,76,552,18,693,37,702,44,706,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-329,1106,-104,538,-99,539,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,681,-117,612,-323,682,-100,683,-324,701,-331,868,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[1106] = new State(-1001);
    states[1107] = new State(new int[]{9,1108,10,458});
    states[1108] = new State(new int[]{5,685,127,-1011},new int[]{-326,1109});
    states[1109] = new State(new int[]{127,1110});
    states[1110] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,541,21,323,22,328,76,552,18,693,37,702,44,706,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-329,1111,-104,538,-99,539,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,681,-117,612,-323,682,-100,683,-324,701,-331,868,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[1111] = new State(-1002);
    states[1112] = new State(new int[]{5,1113,10,1125,8,-796,7,-796,142,-796,4,-796,15,-796,110,-796,111,-796,112,-796,113,-796,114,-796,138,-796,136,-796,118,-796,117,-796,131,-796,132,-796,133,-796,134,-796,130,-796,116,-796,115,-796,128,-796,129,-796,126,-796,6,-796,120,-796,125,-796,123,-796,121,-796,124,-796,122,-796,137,-796,135,-796,16,-796,9,-796,70,-796,100,-796,13,-796,119,-796,11,-796,17,-796});
    states[1113] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-276,1114,-277,466,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[1114] = new State(new int[]{9,1115,10,1119});
    states[1115] = new State(new int[]{5,685,127,-1011},new int[]{-326,1116});
    states[1116] = new State(new int[]{127,1117});
    states[1117] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,541,21,323,22,328,76,552,18,693,37,702,44,706,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-329,1118,-104,538,-99,539,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,681,-117,612,-323,682,-100,683,-324,701,-331,868,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[1118] = new State(-991);
    states[1119] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-327,1120,-328,1101,-158,460,-147,819,-151,45,-152,48});
    states[1120] = new State(new int[]{9,1121,10,458});
    states[1121] = new State(new int[]{5,685,127,-1011},new int[]{-326,1122});
    states[1122] = new State(new int[]{127,1123});
    states[1123] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,541,21,323,22,328,76,552,18,693,37,702,44,706,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-329,1124,-104,538,-99,539,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,681,-117,612,-323,682,-100,683,-324,701,-331,868,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[1124] = new State(-993);
    states[1125] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-327,1126,-328,1101,-158,460,-147,819,-151,45,-152,48});
    states[1126] = new State(new int[]{9,1127,10,458});
    states[1127] = new State(new int[]{5,685,127,-1011},new int[]{-326,1128});
    states[1128] = new State(new int[]{127,1129});
    states[1129] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,541,21,323,22,328,76,552,18,693,37,702,44,706,91,17,40,733,54,769,97,764,35,774,36,801,72,874,25,748,102,791,60,882,47,798,74,996},new int[]{-329,1130,-104,538,-99,539,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,681,-117,612,-323,682,-100,683,-324,701,-331,868,-256,731,-153,732,-319,869,-248,870,-124,871,-123,872,-125,873,-35,991,-302,992,-169,993,-249,994,-126,995});
    states[1130] = new State(-992);
    states[1131] = new State(new int[]{5,1132,7,-260,8,-260,123,-260,12,-260,100,-260});
    states[1132] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-9,1133,-180,665,-147,249,-151,45,-152,48,-301,1134});
    states[1133] = new State(-217);
    states[1134] = new State(new int[]{8,668,12,-643,100,-643},new int[]{-70,1135});
    states[1135] = new State(-785);
    states[1136] = new State(-214);
    states[1137] = new State(-210);
    states[1138] = new State(-473);
    states[1139] = new State(-691);
    states[1140] = new State(new int[]{8,1141});
    states[1141] = new State(new int[]{14,574,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,53,576,143,44,85,46,86,47,80,49,78,50,160,51,87,52,11,936,8,949},new int[]{-354,1142,-352,1148,-15,575,-165,369,-167,308,-166,312,-16,370,-341,1139,-284,1140,-180,208,-147,249,-151,45,-152,48,-344,1146,-345,1147});
    states[1142] = new State(new int[]{9,1143,10,572,100,1144});
    states[1143] = new State(-649);
    states[1144] = new State(new int[]{14,574,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,53,576,143,44,85,46,86,47,80,49,78,50,160,51,87,52,11,936,8,949},new int[]{-352,1145,-15,575,-165,369,-167,308,-166,312,-16,370,-341,1139,-284,1140,-180,208,-147,249,-151,45,-152,48,-344,1146,-345,1147});
    states[1145] = new State(-686);
    states[1146] = new State(-692);
    states[1147] = new State(-693);
    states[1148] = new State(-684);
    states[1149] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,18,693},new int[]{-102,1150,-101,1151,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-100,728});
    states[1150] = new State(-988);
    states[1151] = new State(-985);
    states[1152] = new State(-987);
    states[1153] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,5,629},new int[]{-120,1154,-105,1156,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,633,-268,610});
    states[1154] = new State(new int[]{12,1155});
    states[1155] = new State(-805);
    states[1156] = new State(new int[]{5,194,6,35});
    states[1157] = new State(new int[]{53,1165},new int[]{-338,1158});
    states[1158] = new State(new int[]{9,1159,100,1162});
    states[1159] = new State(new int[]{110,1160});
    states[1160] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-87,1161,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[1161] = new State(new int[]{70,28,92,-520,10,-520,98,-520,101,-520,33,-520,104,-520,2,-520,9,-520,100,-520,12,-520,99,-520,32,-520,85,-520,84,-520,83,-520,82,-520,81,-520,86,-520});
    states[1162] = new State(new int[]{53,1163});
    states[1163] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,1164,-151,45,-152,48});
    states[1164] = new State(-527);
    states[1165] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,1166,-151,45,-152,48});
    states[1166] = new State(-526);
    states[1167] = new State(new int[]{148,1171,150,1172,151,1173,152,1174,154,1175,153,1176,107,-830,91,-830,59,-830,29,-830,66,-830,50,-830,53,-830,62,-830,11,-830,28,-830,26,-830,44,-830,37,-830,19,-830,30,-830,31,-830,46,-830,27,-830,92,-830,84,-830,83,-830,82,-830,81,-830,23,-830,149,-830,41,-830},new int[]{-206,1168,-209,1177});
    states[1168] = new State(new int[]{10,1169});
    states[1169] = new State(new int[]{148,1171,150,1172,151,1173,152,1174,154,1175,153,1176,107,-831,91,-831,59,-831,29,-831,66,-831,50,-831,53,-831,62,-831,11,-831,28,-831,26,-831,44,-831,37,-831,19,-831,30,-831,31,-831,46,-831,27,-831,92,-831,84,-831,83,-831,82,-831,81,-831,23,-831,149,-831,41,-831},new int[]{-209,1170});
    states[1170] = new State(-835);
    states[1171] = new State(-847);
    states[1172] = new State(-848);
    states[1173] = new State(-849);
    states[1174] = new State(-850);
    states[1175] = new State(-851);
    states[1176] = new State(-852);
    states[1177] = new State(-834);
    states[1178] = new State(-376);
    states[1179] = new State(-447);
    states[1180] = new State(-448);
    states[1181] = new State(new int[]{8,-453,110,-453,10,-453,11,-453,5,-453,7,-450});
    states[1182] = new State(new int[]{123,1184,8,-456,110,-456,10,-456,7,-456,11,-456,5,-456},new int[]{-155,1183});
    states[1183] = new State(-457);
    states[1184] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-158,1185,-147,819,-151,45,-152,48});
    states[1185] = new State(new int[]{121,1186,100,462});
    states[1186] = new State(-321);
    states[1187] = new State(-458);
    states[1188] = new State(new int[]{123,1184,8,-454,110,-454,10,-454,11,-454,5,-454},new int[]{-155,1189});
    states[1189] = new State(-455);
    states[1190] = new State(new int[]{7,1191});
    states[1191] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145},new int[]{-142,1192,-149,1193,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188});
    states[1192] = new State(-449);
    states[1193] = new State(-452);
    states[1194] = new State(-451);
    states[1195] = new State(-438);
    states[1196] = new State(new int[]{44,1326,37,1361},new int[]{-216,1197,-228,1198,-225,1325,-229,1360});
    states[1197] = new State(-440);
    states[1198] = new State(new int[]{107,1316,59,-70,29,-70,66,-70,50,-70,53,-70,62,-70,91,-70},new int[]{-176,1199,-43,1200,-39,1203,-61,1315});
    states[1199] = new State(-441);
    states[1200] = new State(new int[]{91,17},new int[]{-256,1201});
    states[1201] = new State(new int[]{10,1202});
    states[1202] = new State(-468);
    states[1203] = new State(new int[]{59,1206,29,1227,66,1231,50,1443,53,1458,62,1460,91,-69},new int[]{-46,1204,-168,1205,-29,1212,-52,1229,-289,1233,-310,1445});
    states[1204] = new State(-71);
    states[1205] = new State(-87);
    states[1206] = new State(new int[]{155,743,143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-156,1207,-143,1211,-147,744,-151,45,-152,48});
    states[1207] = new State(new int[]{10,1208,100,1209});
    states[1208] = new State(-96);
    states[1209] = new State(new int[]{155,743,143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-143,1210,-147,744,-151,45,-152,48});
    states[1210] = new State(-98);
    states[1211] = new State(-97);
    states[1212] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,59,-88,29,-88,66,-88,50,-88,53,-88,62,-88,91,-88},new int[]{-27,1213,-28,1214,-141,1216,-147,1226,-151,45,-152,48});
    states[1213] = new State(-102);
    states[1214] = new State(new int[]{10,1215});
    states[1215] = new State(-112);
    states[1216] = new State(new int[]{120,1217,5,1222});
    states[1217] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,1220,135,850,116,386,115,387,63,372},new int[]{-109,1218,-90,1219,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864,-95,1221});
    states[1218] = new State(-113);
    states[1219] = new State(new int[]{13,237,16,241,10,-115,92,-115,84,-115,83,-115,82,-115,81,-115});
    states[1220] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,1010,135,850,116,386,115,387,63,372,9,-198},new int[]{-90,998,-66,1011,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864,-65,302,-85,1013,-84,305,-95,1014,-244,1015});
    states[1221] = new State(-116);
    states[1222] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-277,1223,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[1223] = new State(new int[]{120,1224});
    states[1224] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,1010,135,850,116,386,115,387,63,372},new int[]{-84,1225,-90,306,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864,-95,1014,-244,1015});
    states[1225] = new State(-114);
    states[1226] = new State(-117);
    states[1227] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-27,1228,-28,1214,-141,1216,-147,1226,-151,45,-152,48});
    states[1228] = new State(-101);
    states[1229] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,59,-89,29,-89,66,-89,50,-89,53,-89,62,-89,91,-89},new int[]{-27,1230,-28,1214,-141,1216,-147,1226,-151,45,-152,48});
    states[1230] = new State(-104);
    states[1231] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-27,1232,-28,1214,-141,1216,-147,1226,-151,45,-152,48});
    states[1232] = new State(-103);
    states[1233] = new State(new int[]{11,659,59,-90,29,-90,66,-90,50,-90,53,-90,62,-90,91,-90,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212},new int[]{-49,1234,-6,1235,-251,1137});
    states[1234] = new State(-106);
    states[1235] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,11,659},new int[]{-50,1236,-251,506,-144,1237,-147,1435,-151,45,-152,48,-145,1440});
    states[1236] = new State(-209);
    states[1237] = new State(new int[]{120,1238});
    states[1238] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635,68,1429,69,1430,148,1431,27,1432,28,1433,26,-303,43,-303,64,-303},new int[]{-287,1239,-277,1241,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594,-30,1242,-21,1243,-22,1427,-20,1434});
    states[1239] = new State(new int[]{10,1240});
    states[1240] = new State(-218);
    states[1241] = new State(-223);
    states[1242] = new State(-224);
    states[1243] = new State(new int[]{26,1421,43,1422,64,1423},new int[]{-291,1244});
    states[1244] = new State(new int[]{8,1342,23,-315,11,-315,92,-315,84,-315,83,-315,82,-315,81,-315,29,-315,143,-315,85,-315,86,-315,80,-315,78,-315,160,-315,87,-315,62,-315,28,-315,26,-315,44,-315,37,-315,19,-315,30,-315,31,-315,46,-315,27,-315,10,-315},new int[]{-183,1245});
    states[1245] = new State(new int[]{23,1333,11,-322,92,-322,84,-322,83,-322,82,-322,81,-322,29,-322,143,-322,85,-322,86,-322,80,-322,78,-322,160,-322,87,-322,62,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322,10,-322},new int[]{-318,1246,-317,1331,-316,1353});
    states[1246] = new State(new int[]{11,659,10,-313,92,-339,84,-339,83,-339,82,-339,81,-339,29,-212,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,62,-212,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-26,1247,-25,1248,-32,1254,-34,497,-45,1255,-6,1256,-251,1137,-33,1418,-54,1420,-53,503,-55,1419});
    states[1247] = new State(-296);
    states[1248] = new State(new int[]{92,1249,84,1250,83,1251,82,1252,81,1253},new int[]{-7,495});
    states[1249] = new State(-314);
    states[1250] = new State(-335);
    states[1251] = new State(-336);
    states[1252] = new State(-337);
    states[1253] = new State(-338);
    states[1254] = new State(-333);
    states[1255] = new State(-347);
    states[1256] = new State(new int[]{29,1258,143,44,85,46,86,47,80,49,78,50,160,51,87,52,62,1262,28,1378,26,1379,11,659,44,1326,37,1361,19,1381,30,1389,31,1396,46,1403,27,1412},new int[]{-51,1257,-251,506,-222,505,-219,507,-259,508,-313,1260,-312,1261,-158,820,-147,819,-151,45,-152,48,-3,1266,-230,1380,-228,1314,-225,1325,-229,1360,-227,1387,-215,1400,-216,1401,-218,1402});
    states[1257] = new State(-349);
    states[1258] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-28,1259,-141,1216,-147,1226,-151,45,-152,48});
    states[1259] = new State(-354);
    states[1260] = new State(-355);
    states[1261] = new State(-359);
    states[1262] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-158,1263,-147,819,-151,45,-152,48});
    states[1263] = new State(new int[]{5,1264,100,462});
    states[1264] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-277,1265,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[1265] = new State(-360);
    states[1266] = new State(new int[]{30,511,19,1196,46,1268,27,1306,143,44,85,46,86,47,80,49,78,50,160,51,87,52,62,1262,44,1326,37,1361},new int[]{-313,1267,-230,510,-216,1195,-312,1261,-158,820,-147,819,-151,45,-152,48,-228,1314,-225,1325,-229,1360});
    states[1267] = new State(-356);
    states[1268] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145},new int[]{-170,1269,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[1269] = new State(new int[]{11,1297,5,-390},new int[]{-233,1270,-238,1294});
    states[1270] = new State(new int[]{85,1283,86,1289,10,-397},new int[]{-202,1271});
    states[1271] = new State(new int[]{10,1272});
    states[1272] = new State(new int[]{63,1277,153,1279,152,1280,148,1281,151,1282,11,-387,28,-387,26,-387,44,-387,37,-387,19,-387,30,-387,31,-387,46,-387,27,-387,92,-387,84,-387,83,-387,82,-387,81,-387},new int[]{-205,1273,-210,1274});
    states[1273] = new State(-381);
    states[1274] = new State(new int[]{10,1275});
    states[1275] = new State(new int[]{63,1277,11,-387,28,-387,26,-387,44,-387,37,-387,19,-387,30,-387,31,-387,46,-387,27,-387,92,-387,84,-387,83,-387,82,-387,81,-387},new int[]{-205,1276});
    states[1276] = new State(-382);
    states[1277] = new State(new int[]{10,1278});
    states[1278] = new State(-388);
    states[1279] = new State(-853);
    states[1280] = new State(-854);
    states[1281] = new State(-855);
    states[1282] = new State(-856);
    states[1283] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,673,8,674,21,323,22,328,76,552,40,620,5,629,18,693,37,702,44,706,10,-396},new int[]{-114,1284,-88,1288,-87,27,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-323,843,-100,683,-324,701});
    states[1284] = new State(new int[]{86,1286,10,-400},new int[]{-203,1285});
    states[1285] = new State(-398);
    states[1286] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494},new int[]{-261,1287,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[1287] = new State(-401);
    states[1288] = new State(-395);
    states[1289] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494},new int[]{-261,1290,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[1290] = new State(new int[]{85,1292,10,-402},new int[]{-204,1291});
    states[1291] = new State(-399);
    states[1292] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,673,8,674,21,323,22,328,76,552,40,620,5,629,18,693,37,702,44,706,10,-396},new int[]{-114,1293,-88,1288,-87,27,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-323,843,-100,683,-324,701});
    states[1293] = new State(-403);
    states[1294] = new State(new int[]{5,1295});
    states[1295] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-276,1296,-277,466,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[1296] = new State(-389);
    states[1297] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-237,1298,-236,1305,-158,1302,-147,819,-151,45,-152,48});
    states[1298] = new State(new int[]{12,1299,10,1300});
    states[1299] = new State(-391);
    states[1300] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-236,1301,-158,1302,-147,819,-151,45,-152,48});
    states[1301] = new State(-393);
    states[1302] = new State(new int[]{5,1303,100,462});
    states[1303] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-276,1304,-277,466,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[1304] = new State(-394);
    states[1305] = new State(-392);
    states[1306] = new State(new int[]{46,1307});
    states[1307] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145},new int[]{-170,1308,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[1308] = new State(new int[]{11,1297,5,-390},new int[]{-233,1309,-238,1294});
    states[1309] = new State(new int[]{110,1312,10,-386},new int[]{-211,1310});
    states[1310] = new State(new int[]{10,1311});
    states[1311] = new State(-384);
    states[1312] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-87,1313,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[1313] = new State(new int[]{70,28,10,-385});
    states[1314] = new State(new int[]{107,1316,11,-370,28,-370,26,-370,44,-370,37,-370,19,-370,30,-370,31,-370,46,-370,27,-370,92,-370,84,-370,83,-370,82,-370,81,-370,59,-70,29,-70,66,-70,50,-70,53,-70,62,-70,91,-70},new int[]{-176,1199,-43,1200,-39,1203,-61,1315});
    states[1315] = new State(-469);
    states[1316] = new State(new int[]{10,1324,143,44,85,46,86,47,80,49,78,50,160,51,87,52,144,310,147,311,145,313,146,314},new int[]{-108,1317,-147,1321,-151,45,-152,48,-165,1322,-167,308,-166,312});
    states[1317] = new State(new int[]{80,1318,10,1323});
    states[1318] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,144,310,147,311,145,313,146,314},new int[]{-108,1319,-147,1321,-151,45,-152,48,-165,1322,-167,308,-166,312});
    states[1319] = new State(new int[]{10,1320});
    states[1320] = new State(-462);
    states[1321] = new State(-465);
    states[1322] = new State(-466);
    states[1323] = new State(-463);
    states[1324] = new State(-464);
    states[1325] = new State(-371);
    states[1326] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145},new int[]{-171,1327,-170,1179,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[1327] = new State(new int[]{8,597,10,-470,110,-470},new int[]{-128,1328});
    states[1328] = new State(new int[]{10,1358,110,-832},new int[]{-207,1329,-208,1354});
    states[1329] = new State(new int[]{23,1333,107,-322,91,-322,59,-322,29,-322,66,-322,50,-322,53,-322,62,-322,11,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322,92,-322,84,-322,83,-322,82,-322,81,-322,149,-322,41,-322},new int[]{-318,1330,-317,1331,-316,1353});
    states[1330] = new State(-459);
    states[1331] = new State(new int[]{23,1333,11,-323,92,-323,84,-323,83,-323,82,-323,81,-323,29,-323,143,-323,85,-323,86,-323,80,-323,78,-323,160,-323,87,-323,62,-323,28,-323,26,-323,44,-323,37,-323,19,-323,30,-323,31,-323,46,-323,27,-323,10,-323,107,-323,91,-323,59,-323,66,-323,50,-323,53,-323,149,-323,41,-323},new int[]{-316,1332});
    states[1332] = new State(-325);
    states[1333] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-158,1334,-147,819,-151,45,-152,48});
    states[1334] = new State(new int[]{5,1335,100,462});
    states[1335] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,1341,49,581,34,585,73,589,44,595,37,635,26,1350,30,1351},new int[]{-288,1336,-286,1352,-277,1340,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[1336] = new State(new int[]{10,1337,100,1338});
    states[1337] = new State(-326);
    states[1338] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,1341,49,581,34,585,73,589,44,595,37,635,26,1350,30,1351},new int[]{-286,1339,-277,1340,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[1339] = new State(-328);
    states[1340] = new State(-329);
    states[1341] = new State(new int[]{8,1342,10,-331,100,-331,23,-315,11,-315,92,-315,84,-315,83,-315,82,-315,81,-315,29,-315,143,-315,85,-315,86,-315,80,-315,78,-315,160,-315,87,-315,62,-315,28,-315,26,-315,44,-315,37,-315,19,-315,30,-315,31,-315,46,-315,27,-315},new int[]{-183,491});
    states[1342] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-182,1343,-181,1349,-180,1347,-147,249,-151,45,-152,48,-301,1348});
    states[1343] = new State(new int[]{9,1344,100,1345});
    states[1344] = new State(-316);
    states[1345] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-181,1346,-180,1347,-147,249,-151,45,-152,48,-301,1348});
    states[1346] = new State(-318);
    states[1347] = new State(new int[]{7,209,123,218,9,-319,100,-319},new int[]{-299,667});
    states[1348] = new State(-320);
    states[1349] = new State(-317);
    states[1350] = new State(-330);
    states[1351] = new State(-332);
    states[1352] = new State(-327);
    states[1353] = new State(-324);
    states[1354] = new State(new int[]{110,1355});
    states[1355] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494},new int[]{-261,1356,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[1356] = new State(new int[]{10,1357});
    states[1357] = new State(-444);
    states[1358] = new State(new int[]{148,1171,150,1172,151,1173,152,1174,154,1175,153,1176,23,-830,107,-830,91,-830,59,-830,29,-830,66,-830,50,-830,53,-830,62,-830,11,-830,28,-830,26,-830,44,-830,37,-830,19,-830,30,-830,31,-830,46,-830,27,-830,92,-830,84,-830,83,-830,82,-830,81,-830,149,-830},new int[]{-206,1359,-209,1177});
    states[1359] = new State(new int[]{10,1169,110,-833});
    states[1360] = new State(-372);
    states[1361] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145},new int[]{-170,1362,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[1362] = new State(new int[]{8,597,5,-470,10,-470,110,-470},new int[]{-128,1363});
    states[1363] = new State(new int[]{5,1366,10,1358,110,-832},new int[]{-207,1364,-208,1374});
    states[1364] = new State(new int[]{23,1333,107,-322,91,-322,59,-322,29,-322,66,-322,50,-322,53,-322,62,-322,11,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322,92,-322,84,-322,83,-322,82,-322,81,-322,149,-322,41,-322},new int[]{-318,1365,-317,1331,-316,1353});
    states[1365] = new State(-460);
    states[1366] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-276,1367,-277,466,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[1367] = new State(new int[]{10,1358,110,-832},new int[]{-207,1368,-208,1370});
    states[1368] = new State(new int[]{23,1333,107,-322,91,-322,59,-322,29,-322,66,-322,50,-322,53,-322,62,-322,11,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322,92,-322,84,-322,83,-322,82,-322,81,-322,149,-322,41,-322},new int[]{-318,1369,-317,1331,-316,1353});
    states[1369] = new State(-461);
    states[1370] = new State(new int[]{110,1371});
    states[1371] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,674,21,323,22,328,76,552,40,620,18,693,37,702,44,706},new int[]{-103,1372,-101,891,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-323,892,-100,683,-324,701});
    states[1372] = new State(new int[]{10,1373});
    states[1373] = new State(-442);
    states[1374] = new State(new int[]{110,1375});
    states[1375] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,674,21,323,22,328,76,552,40,620,18,693,37,702,44,706},new int[]{-103,1376,-101,891,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-323,892,-100,683,-324,701});
    states[1376] = new State(new int[]{10,1377});
    states[1377] = new State(-443);
    states[1378] = new State(-357);
    states[1379] = new State(-358);
    states[1380] = new State(-366);
    states[1381] = new State(new int[]{28,1378,26,1379,44,1326,37,1361},new int[]{-3,1382,-230,1385,-216,1386,-228,1314,-225,1325,-229,1360});
    states[1382] = new State(new int[]{44,1326,37,1361},new int[]{-230,1383,-216,1384,-228,1314,-225,1325,-229,1360});
    states[1383] = new State(-367);
    states[1384] = new State(-439);
    states[1385] = new State(-368);
    states[1386] = new State(-437);
    states[1387] = new State(new int[]{107,1316,11,-369,28,-369,26,-369,44,-369,37,-369,19,-369,30,-369,31,-369,46,-369,27,-369,92,-369,84,-369,83,-369,82,-369,81,-369,59,-70,29,-70,66,-70,50,-70,53,-70,62,-70,91,-70},new int[]{-176,1388,-43,1200,-39,1203,-61,1315});
    states[1388] = new State(-421);
    states[1389] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,8,-377,110,-377,10,-377},new int[]{-172,1390,-171,1178,-170,1179,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[1390] = new State(new int[]{8,597,110,-470,10,-470},new int[]{-128,1391});
    states[1391] = new State(new int[]{110,1393,10,1167},new int[]{-207,1392});
    states[1392] = new State(-373);
    states[1393] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494},new int[]{-261,1394,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[1394] = new State(new int[]{10,1395});
    states[1395] = new State(-422);
    states[1396] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,8,-377,10,-377},new int[]{-172,1397,-171,1178,-170,1179,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[1397] = new State(new int[]{8,597,10,-470},new int[]{-128,1398});
    states[1398] = new State(new int[]{10,1167},new int[]{-207,1399});
    states[1399] = new State(-375);
    states[1400] = new State(-363);
    states[1401] = new State(-436);
    states[1402] = new State(-364);
    states[1403] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145},new int[]{-170,1404,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[1404] = new State(new int[]{11,1297,5,-390},new int[]{-233,1405,-238,1294});
    states[1405] = new State(new int[]{85,1283,86,1289,10,-397},new int[]{-202,1406});
    states[1406] = new State(new int[]{10,1407});
    states[1407] = new State(new int[]{63,1277,153,1279,152,1280,148,1281,151,1282,11,-387,28,-387,26,-387,44,-387,37,-387,19,-387,30,-387,31,-387,46,-387,27,-387,92,-387,84,-387,83,-387,82,-387,81,-387},new int[]{-205,1408,-210,1409});
    states[1408] = new State(-379);
    states[1409] = new State(new int[]{10,1410});
    states[1410] = new State(new int[]{63,1277,11,-387,28,-387,26,-387,44,-387,37,-387,19,-387,30,-387,31,-387,46,-387,27,-387,92,-387,84,-387,83,-387,82,-387,81,-387},new int[]{-205,1411});
    states[1411] = new State(-380);
    states[1412] = new State(new int[]{46,1413});
    states[1413] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145},new int[]{-170,1414,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[1414] = new State(new int[]{11,1297,5,-390},new int[]{-233,1415,-238,1294});
    states[1415] = new State(new int[]{110,1312,10,-386},new int[]{-211,1416});
    states[1416] = new State(new int[]{10,1417});
    states[1417] = new State(-383);
    states[1418] = new State(new int[]{11,659,92,-341,84,-341,83,-341,82,-341,81,-341,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-54,502,-53,503,-6,504,-251,1137,-55,1419});
    states[1419] = new State(-353);
    states[1420] = new State(-350);
    states[1421] = new State(-307);
    states[1422] = new State(-308);
    states[1423] = new State(new int[]{26,1424,48,1425,43,1426,8,-309,23,-309,11,-309,92,-309,84,-309,83,-309,82,-309,81,-309,29,-309,143,-309,85,-309,86,-309,80,-309,78,-309,160,-309,87,-309,62,-309,28,-309,44,-309,37,-309,19,-309,30,-309,31,-309,46,-309,27,-309,10,-309});
    states[1424] = new State(-310);
    states[1425] = new State(-311);
    states[1426] = new State(-312);
    states[1427] = new State(new int[]{68,1429,69,1430,148,1431,27,1432,28,1433,26,-304,43,-304,64,-304},new int[]{-20,1428});
    states[1428] = new State(-306);
    states[1429] = new State(-298);
    states[1430] = new State(-299);
    states[1431] = new State(-300);
    states[1432] = new State(-301);
    states[1433] = new State(-302);
    states[1434] = new State(-305);
    states[1435] = new State(new int[]{123,1437,120,-220},new int[]{-155,1436});
    states[1436] = new State(-221);
    states[1437] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-158,1438,-147,819,-151,45,-152,48});
    states[1438] = new State(new int[]{122,1439,121,1186,100,462});
    states[1439] = new State(-222);
    states[1440] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635,68,1429,69,1430,148,1431,27,1432,28,1433,26,-303,43,-303,64,-303},new int[]{-287,1441,-277,1241,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594,-30,1242,-21,1243,-22,1427,-20,1434});
    states[1441] = new State(new int[]{10,1442});
    states[1442] = new State(-219);
    states[1443] = new State(new int[]{11,659,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212},new int[]{-49,1444,-6,1235,-251,1137});
    states[1444] = new State(-105);
    states[1445] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,8,1450,59,-91,29,-91,66,-91,50,-91,53,-91,62,-91,91,-91},new int[]{-314,1446,-311,1447,-312,1448,-158,820,-147,819,-151,45,-152,48});
    states[1446] = new State(-111);
    states[1447] = new State(-107);
    states[1448] = new State(new int[]{10,1449});
    states[1449] = new State(-404);
    states[1450] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,1451,-151,45,-152,48});
    states[1451] = new State(new int[]{100,1452});
    states[1452] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-158,1453,-147,819,-151,45,-152,48});
    states[1453] = new State(new int[]{9,1454,100,462});
    states[1454] = new State(new int[]{110,1455});
    states[1455] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,1456,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[1456] = new State(new int[]{10,1457});
    states[1457] = new State(-108);
    states[1458] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,8,1450},new int[]{-314,1459,-311,1447,-312,1448,-158,820,-147,819,-151,45,-152,48});
    states[1459] = new State(-109);
    states[1460] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,8,1450},new int[]{-314,1461,-311,1447,-312,1448,-158,820,-147,819,-151,45,-152,48});
    states[1461] = new State(-110);
    states[1462] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,389,12,-279,100,-279},new int[]{-272,1463,-273,1464,-93,225,-106,343,-107,344,-180,385,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312});
    states[1463] = new State(-277);
    states[1464] = new State(-278);
    states[1465] = new State(-276);
    states[1466] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-277,1467,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[1467] = new State(-275);
    states[1468] = new State(-245);
    states[1469] = new State(-246);
    states[1470] = new State(new int[]{127,472,121,-247,100,-247,120,-247,9,-247,10,-247,8,-247,138,-247,136,-247,118,-247,117,-247,131,-247,132,-247,133,-247,134,-247,130,-247,116,-247,115,-247,128,-247,129,-247,126,-247,6,-247,5,-247,125,-247,123,-247,124,-247,122,-247,137,-247,135,-247,16,-247,70,-247,92,-247,98,-247,101,-247,33,-247,104,-247,2,-247,12,-247,99,-247,32,-247,85,-247,84,-247,83,-247,82,-247,81,-247,86,-247,13,-247,76,-247,51,-247,58,-247,141,-247,143,-247,80,-247,78,-247,160,-247,87,-247,45,-247,42,-247,21,-247,22,-247,144,-247,147,-247,145,-247,146,-247,155,-247,158,-247,157,-247,156,-247,57,-247,91,-247,40,-247,25,-247,97,-247,54,-247,35,-247,55,-247,102,-247,47,-247,36,-247,53,-247,60,-247,74,-247,72,-247,38,-247,71,-247,110,-247});
    states[1471] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,1472,-151,45,-152,48});
    states[1472] = new State(new int[]{110,1473});
    states[1473] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-87,525,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[1474] = new State(new int[]{110,1475,127,536,8,-796,7,-796,142,-796,4,-796,15,-796,138,-796,136,-796,118,-796,117,-796,131,-796,132,-796,133,-796,134,-796,130,-796,116,-796,115,-796,128,-796,129,-796,126,-796,6,-796,5,-796,120,-796,125,-796,123,-796,121,-796,124,-796,122,-796,137,-796,135,-796,16,-796,70,-796,100,-796,9,-796,13,-796,119,-796,11,-796,17,-796});
    states[1475] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,1476,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[1476] = new State(-602);
    states[1477] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,100,-604,9,-604},new int[]{-147,441,-151,45,-152,48});
    states[1478] = new State(-603);
    states[1479] = new State(-783);
    states[1480] = new State(new int[]{13,471});
    states[1481] = new State(-240);
    states[1482] = new State(-236);
    states[1483] = new State(-627);
    states[1484] = new State(new int[]{8,1485});
    states[1485] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-334,1486,-333,1494,-147,1490,-151,45,-152,48,-101,1493,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[1486] = new State(new int[]{9,1487,100,1488});
    states[1487] = new State(-638);
    states[1488] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-333,1489,-147,1490,-151,45,-152,48,-101,1493,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[1489] = new State(-642);
    states[1490] = new State(new int[]{110,1491,8,-796,7,-796,142,-796,4,-796,15,-796,138,-796,136,-796,118,-796,117,-796,131,-796,132,-796,133,-796,134,-796,130,-796,116,-796,115,-796,128,-796,129,-796,126,-796,6,-796,120,-796,125,-796,123,-796,121,-796,124,-796,122,-796,137,-796,135,-796,16,-796,9,-796,100,-796,13,-796,119,-796,11,-796,17,-796});
    states[1491] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620},new int[]{-101,1492,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619});
    states[1492] = new State(-639);
    states[1493] = new State(-640);
    states[1494] = new State(-641);
    states[1495] = new State(new int[]{13,237,16,241,5,-706,12,-706});
    states[1496] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-90,1497,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[1497] = new State(new int[]{13,237,16,241,100,-189,9,-189,12,-189,5,-189});
    states[1498] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372,5,-707,12,-707},new int[]{-122,1499,-90,1495,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[1499] = new State(new int[]{5,1500,12,-713});
    states[1500] = new State(new int[]{143,44,85,46,86,47,80,49,78,292,160,51,87,52,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,42,320,21,323,22,328,11,401,76,829,56,832,141,833,8,847,135,850,116,386,115,387,63,372},new int[]{-90,1501,-91,267,-81,275,-13,280,-10,290,-14,253,-147,291,-151,45,-152,48,-165,307,-167,308,-166,312,-16,315,-258,322,-295,327,-239,399,-240,400,-199,856,-173,854,-57,855,-266,862,-270,863,-11,858,-242,864});
    states[1501] = new State(new int[]{13,237,16,241,12,-715});
    states[1502] = new State(-186);
    states[1503] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314},new int[]{-93,1504,-106,343,-107,344,-180,385,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312});
    states[1504] = new State(new int[]{116,276,115,277,128,278,129,279,13,-249,121,-249,100,-249,120,-249,9,-249,10,-249,127,-249,8,-249,138,-249,136,-249,118,-249,117,-249,131,-249,132,-249,133,-249,134,-249,130,-249,126,-249,6,-249,5,-249,125,-249,123,-249,124,-249,122,-249,137,-249,135,-249,16,-249,70,-249,92,-249,98,-249,101,-249,33,-249,104,-249,2,-249,12,-249,99,-249,32,-249,85,-249,84,-249,83,-249,82,-249,81,-249,86,-249,76,-249,51,-249,58,-249,141,-249,143,-249,80,-249,78,-249,160,-249,87,-249,45,-249,42,-249,21,-249,22,-249,144,-249,147,-249,145,-249,146,-249,155,-249,158,-249,157,-249,156,-249,57,-249,91,-249,40,-249,25,-249,97,-249,54,-249,35,-249,55,-249,102,-249,47,-249,36,-249,53,-249,60,-249,74,-249,72,-249,38,-249,71,-249,110,-249},new int[]{-193,226});
    states[1505] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,24,482},new int[]{-284,1506,-279,1507,-180,208,-147,249,-151,45,-152,48,-271,488});
    states[1506] = new State(-738);
    states[1507] = new State(-739);
    states[1508] = new State(-752);
    states[1509] = new State(-753);
    states[1510] = new State(-754);
    states[1511] = new State(-755);
    states[1512] = new State(-756);
    states[1513] = new State(-757);
    states[1514] = new State(-758);
    states[1515] = new State(-749);
    states[1516] = new State(-750);
    states[1517] = new State(-728);
    states[1518] = new State(-647);
    states[1519] = new State(-35);
    states[1520] = new State(new int[]{59,1206,29,1227,66,1231,50,1443,53,1458,62,1460,11,659,91,-64,92,-64,103,-64,44,-212,37,-212,28,-212,26,-212,19,-212,30,-212,31,-212},new int[]{-47,1521,-168,1522,-29,1523,-52,1524,-289,1525,-310,1526,-220,1527,-6,1528,-251,1137});
    states[1521] = new State(-68);
    states[1522] = new State(-78);
    states[1523] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,59,-79,29,-79,66,-79,50,-79,53,-79,62,-79,11,-79,44,-79,37,-79,28,-79,26,-79,19,-79,30,-79,31,-79,91,-79,92,-79,103,-79},new int[]{-27,1213,-28,1214,-141,1216,-147,1226,-151,45,-152,48});
    states[1524] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,59,-80,29,-80,66,-80,50,-80,53,-80,62,-80,11,-80,44,-80,37,-80,28,-80,26,-80,19,-80,30,-80,31,-80,91,-80,92,-80,103,-80},new int[]{-27,1230,-28,1214,-141,1216,-147,1226,-151,45,-152,48});
    states[1525] = new State(new int[]{11,659,59,-81,29,-81,66,-81,50,-81,53,-81,62,-81,44,-81,37,-81,28,-81,26,-81,19,-81,30,-81,31,-81,91,-81,92,-81,103,-81,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212},new int[]{-49,1234,-6,1235,-251,1137});
    states[1526] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,8,1450,59,-82,29,-82,66,-82,50,-82,53,-82,62,-82,11,-82,44,-82,37,-82,28,-82,26,-82,19,-82,30,-82,31,-82,91,-82,92,-82,103,-82},new int[]{-314,1446,-311,1447,-312,1448,-158,820,-147,819,-151,45,-152,48});
    states[1527] = new State(-83);
    states[1528] = new State(new int[]{44,1541,37,1548,28,1378,26,1379,19,1575,30,1582,31,1396,11,659},new int[]{-213,1529,-251,506,-214,1530,-221,1531,-228,1532,-225,1325,-229,1360,-3,1565,-217,1579,-227,1580});
    states[1529] = new State(-86);
    states[1530] = new State(-84);
    states[1531] = new State(-424);
    states[1532] = new State(new int[]{149,1534,107,1316,59,-67,29,-67,66,-67,50,-67,53,-67,62,-67,11,-67,44,-67,37,-67,28,-67,26,-67,19,-67,30,-67,31,-67,91,-67},new int[]{-178,1533,-177,1536,-41,1537,-42,1520,-61,1540});
    states[1533] = new State(-429);
    states[1534] = new State(new int[]{10,1535});
    states[1535] = new State(-435);
    states[1536] = new State(-445);
    states[1537] = new State(new int[]{91,17},new int[]{-256,1538});
    states[1538] = new State(new int[]{10,1539});
    states[1539] = new State(-467);
    states[1540] = new State(-446);
    states[1541] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145},new int[]{-171,1542,-170,1179,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[1542] = new State(new int[]{8,597,10,-470,110,-470},new int[]{-128,1543});
    states[1543] = new State(new int[]{10,1358,110,-832},new int[]{-207,1329,-208,1544});
    states[1544] = new State(new int[]{110,1545});
    states[1545] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494},new int[]{-261,1546,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[1546] = new State(new int[]{10,1547});
    states[1547] = new State(-434);
    states[1548] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145},new int[]{-170,1549,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[1549] = new State(new int[]{8,597,5,-470,10,-470,110,-470},new int[]{-128,1550});
    states[1550] = new State(new int[]{5,1551,10,1358,110,-832},new int[]{-207,1364,-208,1559});
    states[1551] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-276,1552,-277,466,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[1552] = new State(new int[]{10,1358,110,-832},new int[]{-207,1368,-208,1553});
    states[1553] = new State(new int[]{110,1554});
    states[1554] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,674,21,323,22,328,76,552,40,620,18,693,37,702,44,706},new int[]{-101,1555,-323,1557,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-100,683,-324,701});
    states[1555] = new State(new int[]{10,1556});
    states[1556] = new State(-430);
    states[1557] = new State(new int[]{10,1558});
    states[1558] = new State(-432);
    states[1559] = new State(new int[]{110,1560});
    states[1560] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,674,21,323,22,328,76,552,40,620,18,693,37,702,44,706},new int[]{-101,1561,-323,1563,-99,30,-98,183,-105,540,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,535,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-100,683,-324,701});
    states[1561] = new State(new int[]{10,1562});
    states[1562] = new State(-431);
    states[1563] = new State(new int[]{10,1564});
    states[1564] = new State(-433);
    states[1565] = new State(new int[]{19,1567,30,1569,44,1541,37,1548},new int[]{-221,1566,-228,1532,-225,1325,-229,1360});
    states[1566] = new State(-425);
    states[1567] = new State(new int[]{44,1541,37,1548},new int[]{-221,1568,-228,1532,-225,1325,-229,1360});
    states[1568] = new State(-428);
    states[1569] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,8,-377,110,-377,10,-377},new int[]{-172,1570,-171,1178,-170,1179,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[1570] = new State(new int[]{8,597,110,-470,10,-470},new int[]{-128,1571});
    states[1571] = new State(new int[]{110,1572,10,1167},new int[]{-207,514});
    states[1572] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494},new int[]{-261,1573,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[1573] = new State(new int[]{10,1574});
    states[1574] = new State(-420);
    states[1575] = new State(new int[]{44,1541,37,1548,28,1378,26,1379},new int[]{-221,1576,-3,1577,-228,1532,-225,1325,-229,1360});
    states[1576] = new State(-426);
    states[1577] = new State(new int[]{44,1541,37,1548},new int[]{-221,1578,-228,1532,-225,1325,-229,1360});
    states[1578] = new State(-427);
    states[1579] = new State(-85);
    states[1580] = new State(-67,new int[]{-177,1581,-41,1537,-42,1520});
    states[1581] = new State(-418);
    states[1582] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145,8,-377,110,-377,10,-377},new int[]{-172,1583,-171,1178,-170,1179,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[1583] = new State(new int[]{8,597,110,-470,10,-470},new int[]{-128,1584});
    states[1584] = new State(new int[]{110,1585,10,1167},new int[]{-207,1392});
    states[1585] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,10,-494},new int[]{-261,1586,-4,23,-113,24,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895});
    states[1586] = new State(new int[]{10,1587});
    states[1587] = new State(-419);
    states[1588] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,84,55,83,56,82,57,81,58,68,59,64,60,128,61,22,62,21,63,63,64,23,65,129,66,130,67,131,68,132,69,133,70,134,71,135,72,136,73,137,74,138,75,24,76,73,77,91,78,25,79,26,80,29,81,30,82,31,83,71,84,99,85,32,86,92,87,33,88,34,89,27,90,104,91,101,92,35,93,36,94,37,95,40,96,41,97,42,98,103,99,43,100,44,101,46,102,47,103,48,104,97,105,49,106,102,107,50,108,28,109,51,110,70,111,98,112,52,113,53,114,54,115,55,116,56,117,57,118,58,119,59,120,61,121,105,122,106,123,109,124,107,125,108,126,62,127,74,128,38,129,39,130,69,131,148,132,60,133,139,134,140,135,79,136,153,137,152,138,72,139,154,140,150,141,151,142,149,143,45,214},new int[]{-304,1589,-308,1599,-157,1593,-138,1598,-147,211,-151,45,-152,48,-293,212,-150,54,-294,213});
    states[1589] = new State(new int[]{10,1590,100,1591});
    states[1590] = new State(-38);
    states[1591] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,84,55,83,56,82,57,81,58,68,59,64,60,128,61,22,62,21,63,63,64,23,65,129,66,130,67,131,68,132,69,133,70,134,71,135,72,136,73,137,74,138,75,24,76,73,77,91,78,25,79,26,80,29,81,30,82,31,83,71,84,99,85,32,86,92,87,33,88,34,89,27,90,104,91,101,92,35,93,36,94,37,95,40,96,41,97,42,98,103,99,43,100,44,101,46,102,47,103,48,104,97,105,49,106,102,107,50,108,28,109,51,110,70,111,98,112,52,113,53,114,54,115,55,116,56,117,57,118,58,119,59,120,61,121,105,122,106,123,109,124,107,125,108,126,62,127,74,128,38,129,39,130,69,131,148,132,60,133,139,134,140,135,79,136,153,137,152,138,72,139,154,140,150,141,151,142,149,143,45,214},new int[]{-308,1592,-157,1593,-138,1598,-147,211,-151,45,-152,48,-293,212,-150,54,-294,213});
    states[1592] = new State(-44);
    states[1593] = new State(new int[]{7,1594,137,1596,10,-45,100,-45});
    states[1594] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,84,55,83,56,82,57,81,58,68,59,64,60,128,61,22,62,21,63,63,64,23,65,129,66,130,67,131,68,132,69,133,70,134,71,135,72,136,73,137,74,138,75,24,76,73,77,91,78,25,79,26,80,29,81,30,82,31,83,71,84,99,85,32,86,92,87,33,88,34,89,27,90,104,91,101,92,35,93,36,94,37,95,40,96,41,97,42,98,103,99,43,100,44,101,46,102,47,103,48,104,97,105,49,106,102,107,50,108,28,109,51,110,70,111,98,112,52,113,53,114,54,115,55,116,56,117,57,118,58,119,59,120,61,121,105,122,106,123,109,124,107,125,108,126,62,127,74,128,38,129,39,130,69,131,148,132,60,133,139,134,140,135,79,136,153,137,152,138,72,139,154,140,150,141,151,142,149,143,45,214},new int[]{-138,1595,-147,211,-151,45,-152,48,-293,212,-150,54,-294,213});
    states[1595] = new State(-37);
    states[1596] = new State(new int[]{144,1597});
    states[1597] = new State(-46);
    states[1598] = new State(-36);
    states[1599] = new State(-43);
    states[1600] = new State(new int[]{3,1602,52,-15,91,-15,59,-15,29,-15,66,-15,50,-15,53,-15,62,-15,11,-15,44,-15,37,-15,28,-15,26,-15,19,-15,30,-15,31,-15,43,-15,92,-15,103,-15},new int[]{-184,1601});
    states[1601] = new State(-17);
    states[1602] = new State(new int[]{143,1603,144,1604});
    states[1603] = new State(-18);
    states[1604] = new State(-19);
    states[1605] = new State(-16);
    states[1606] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-147,1607,-151,45,-152,48});
    states[1607] = new State(new int[]{10,1609,8,1610},new int[]{-187,1608});
    states[1608] = new State(-28);
    states[1609] = new State(-29);
    states[1610] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-189,1611,-146,1617,-147,1616,-151,45,-152,48});
    states[1611] = new State(new int[]{9,1612,100,1614});
    states[1612] = new State(new int[]{10,1613});
    states[1613] = new State(-30);
    states[1614] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-146,1615,-147,1616,-151,45,-152,48});
    states[1615] = new State(-32);
    states[1616] = new State(-33);
    states[1617] = new State(-31);
    states[1618] = new State(-3);
    states[1619] = new State(new int[]{43,1640,52,-41,59,-41,29,-41,66,-41,50,-41,53,-41,62,-41,11,-41,44,-41,37,-41,28,-41,26,-41,19,-41,30,-41,31,-41,92,-41,103,-41,91,-41},new int[]{-162,1620,-163,1637,-303,1666});
    states[1620] = new State(new int[]{41,1634},new int[]{-161,1621});
    states[1621] = new State(new int[]{92,1624,103,1625,91,1631},new int[]{-154,1622});
    states[1622] = new State(new int[]{7,1623});
    states[1623] = new State(-47);
    states[1624] = new State(-57);
    states[1625] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,762,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,104,-494,10,-494},new int[]{-253,1626,-262,760,-261,22,-4,23,-113,24,-132,419,-111,429,-147,761,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895,-143,1050});
    states[1626] = new State(new int[]{92,1627,104,1628,10,20});
    states[1627] = new State(-58);
    states[1628] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,762,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494},new int[]{-253,1629,-262,760,-261,22,-4,23,-113,24,-132,419,-111,429,-147,761,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895,-143,1050});
    states[1629] = new State(new int[]{92,1630,10,20});
    states[1630] = new State(-59);
    states[1631] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,762,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,92,-494,10,-494},new int[]{-253,1632,-262,760,-261,22,-4,23,-113,24,-132,419,-111,429,-147,761,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895,-143,1050});
    states[1632] = new State(new int[]{92,1633,10,20});
    states[1633] = new State(-60);
    states[1634] = new State(-41,new int[]{-303,1635});
    states[1635] = new State(new int[]{52,1588,59,-67,29,-67,66,-67,50,-67,53,-67,62,-67,11,-67,44,-67,37,-67,28,-67,26,-67,19,-67,30,-67,31,-67,92,-67,103,-67,91,-67},new int[]{-41,1636,-305,14,-42,1520});
    states[1636] = new State(-55);
    states[1637] = new State(new int[]{92,1624,103,1625,91,1631},new int[]{-154,1638});
    states[1638] = new State(new int[]{7,1639});
    states[1639] = new State(-48);
    states[1640] = new State(-41,new int[]{-303,1641});
    states[1641] = new State(new int[]{52,1588,29,-62,66,-62,50,-62,53,-62,62,-62,11,-62,44,-62,37,-62,41,-62},new int[]{-40,1642,-305,14,-38,1643});
    states[1642] = new State(-54);
    states[1643] = new State(new int[]{29,1227,66,1231,50,1443,53,1458,62,1460,11,659,41,-61,44,-212,37,-212},new int[]{-48,1644,-29,1645,-52,1646,-289,1647,-310,1648,-232,1649,-6,1650,-251,1137,-231,1665});
    states[1644] = new State(-63);
    states[1645] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,29,-72,66,-72,50,-72,53,-72,62,-72,11,-72,44,-72,37,-72,41,-72},new int[]{-27,1213,-28,1214,-141,1216,-147,1226,-151,45,-152,48});
    states[1646] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,29,-73,66,-73,50,-73,53,-73,62,-73,11,-73,44,-73,37,-73,41,-73},new int[]{-27,1230,-28,1214,-141,1216,-147,1226,-151,45,-152,48});
    states[1647] = new State(new int[]{11,659,29,-74,66,-74,50,-74,53,-74,62,-74,44,-74,37,-74,41,-74,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212},new int[]{-49,1234,-6,1235,-251,1137});
    states[1648] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,8,1450,29,-75,66,-75,50,-75,53,-75,62,-75,11,-75,44,-75,37,-75,41,-75},new int[]{-314,1446,-311,1447,-312,1448,-158,820,-147,819,-151,45,-152,48});
    states[1649] = new State(-76);
    states[1650] = new State(new int[]{44,1657,11,659,37,1660},new int[]{-225,1651,-251,506,-229,1654});
    states[1651] = new State(new int[]{149,1652,29,-92,66,-92,50,-92,53,-92,62,-92,11,-92,44,-92,37,-92,41,-92});
    states[1652] = new State(new int[]{10,1653});
    states[1653] = new State(-93);
    states[1654] = new State(new int[]{149,1655,29,-94,66,-94,50,-94,53,-94,62,-94,11,-94,44,-94,37,-94,41,-94});
    states[1655] = new State(new int[]{10,1656});
    states[1656] = new State(-95);
    states[1657] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145},new int[]{-171,1658,-170,1179,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[1658] = new State(new int[]{8,597,10,-470},new int[]{-128,1659});
    states[1659] = new State(new int[]{10,1167},new int[]{-207,1329});
    states[1660] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,45,145},new int[]{-170,1661,-142,1180,-137,1181,-134,1182,-147,1187,-151,45,-152,48,-191,1188,-335,1190,-149,1194});
    states[1661] = new State(new int[]{8,597,5,-470,10,-470},new int[]{-128,1662});
    states[1662] = new State(new int[]{5,1663,10,1167},new int[]{-207,1364});
    states[1663] = new State(new int[]{143,395,85,46,86,47,80,49,78,50,160,51,87,52,155,316,158,317,157,318,156,319,116,386,115,387,144,310,147,311,145,313,146,314,8,467,142,478,24,482,48,490,49,581,34,585,73,589,44,595,37,635},new int[]{-276,1664,-277,466,-273,394,-93,225,-106,343,-107,344,-180,345,-147,249,-151,45,-152,48,-16,382,-199,383,-165,388,-167,308,-166,312,-274,469,-301,470,-257,476,-250,477,-282,480,-279,481,-271,488,-31,489,-264,580,-130,584,-131,588,-226,592,-224,593,-223,594});
    states[1664] = new State(new int[]{10,1167},new int[]{-207,1368});
    states[1665] = new State(-77);
    states[1666] = new State(new int[]{52,1588,59,-67,29,-67,66,-67,50,-67,53,-67,62,-67,11,-67,44,-67,37,-67,28,-67,26,-67,19,-67,30,-67,31,-67,92,-67,103,-67,91,-67},new int[]{-41,1667,-305,14,-42,1520});
    states[1667] = new State(-56);
    states[1668] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-139,1669,-147,1672,-151,45,-152,48});
    states[1669] = new State(new int[]{10,1670});
    states[1670] = new State(new int[]{3,1602,43,-14,92,-14,103,-14,91,-14,52,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-185,1671,-186,1600,-184,1605});
    states[1671] = new State(-49);
    states[1672] = new State(-53);
    states[1673] = new State(-51);
    states[1674] = new State(-52);
    states[1675] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,84,55,83,56,82,57,81,58,68,59,64,60,128,61,22,62,21,63,63,64,23,65,129,66,130,67,131,68,132,69,133,70,134,71,135,72,136,73,137,74,138,75,24,76,73,77,91,78,25,79,26,80,29,81,30,82,31,83,71,84,99,85,32,86,92,87,33,88,34,89,27,90,104,91,101,92,35,93,36,94,37,95,40,96,41,97,42,98,103,99,43,100,44,101,46,102,47,103,48,104,97,105,49,106,102,107,50,108,28,109,51,110,70,111,98,112,52,113,53,114,54,115,55,116,56,117,57,118,58,119,59,120,61,121,105,122,106,123,109,124,107,125,108,126,62,127,74,128,38,129,39,130,69,131,148,132,60,133,139,134,140,135,79,136,153,137,152,138,72,139,154,140,150,141,151,142,149,143,45,214},new int[]{-157,1676,-138,1598,-147,211,-151,45,-152,48,-293,212,-150,54,-294,213});
    states[1676] = new State(new int[]{10,1677,7,1594});
    states[1677] = new State(new int[]{3,1602,43,-14,92,-14,103,-14,91,-14,52,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-185,1678,-186,1600,-184,1605});
    states[1678] = new State(-50);
    states[1679] = new State(-4);
    states[1680] = new State(new int[]{50,1682,56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,449,21,323,22,328,76,552,40,620,5,629},new int[]{-87,1681,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,427,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628});
    states[1681] = new State(new int[]{70,28,2,-7});
    states[1682] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-144,1683,-147,1684,-151,45,-152,48});
    states[1683] = new State(-8);
    states[1684] = new State(new int[]{123,1184,2,-220},new int[]{-155,1436});
    states[1685] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52},new int[]{-321,1686,-322,1687,-147,1691,-151,45,-152,48});
    states[1686] = new State(-9);
    states[1687] = new State(new int[]{7,1688,123,218,2,-788},new int[]{-299,1690});
    states[1688] = new State(new int[]{143,44,85,46,86,47,80,49,78,50,160,51,87,52,84,55,83,56,82,57,81,58,68,59,64,60,128,61,22,62,21,63,63,64,23,65,129,66,130,67,131,68,132,69,133,70,134,71,135,72,136,73,137,74,138,75,24,76,73,77,91,78,25,79,26,80,29,81,30,82,31,83,71,84,99,85,32,86,92,87,33,88,34,89,27,90,104,91,101,92,35,93,36,94,37,95,40,96,41,97,42,98,103,99,43,100,44,101,46,102,47,103,48,104,97,105,49,106,102,107,50,108,28,109,51,110,70,111,98,112,52,113,53,114,54,115,55,116,56,117,57,118,58,119,59,120,61,121,105,122,106,123,109,124,107,125,108,126,62,127,74,128,38,129,39,130,69,131,148,132,60,133,139,134,140,135,79,136,153,137,152,138,72,139,154,140,150,141,151,142,149,143,45,214},new int[]{-138,1689,-147,211,-151,45,-152,48,-293,212,-150,54,-294,213});
    states[1689] = new State(-787);
    states[1690] = new State(-789);
    states[1691] = new State(-786);
    states[1692] = new State(new int[]{56,365,144,310,147,311,145,313,146,314,155,316,158,317,157,318,156,319,63,372,11,401,135,411,116,386,115,387,142,416,141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,440,8,519,21,323,22,328,76,552,40,620,5,629,53,810},new int[]{-260,1693,-87,1694,-101,182,-99,30,-98,183,-105,193,-83,198,-82,204,-96,364,-15,366,-165,369,-167,308,-166,312,-16,370,-57,371,-240,410,-199,425,-113,725,-132,419,-111,429,-147,438,-151,45,-152,48,-191,439,-258,527,-295,528,-17,529,-112,556,-58,557,-116,560,-173,561,-269,562,-97,563,-265,567,-267,568,-268,610,-241,611,-117,612,-243,619,-120,628,-4,1695,-315,1696});
    states[1693] = new State(-10);
    states[1694] = new State(new int[]{70,28,2,-11});
    states[1695] = new State(-12);
    states[1696] = new State(-13);
    states[1697] = new State(new int[]{52,1588,141,-39,143,-39,85,-39,86,-39,80,-39,78,-39,160,-39,87,-39,45,-39,42,-39,8,-39,21,-39,22,-39,144,-39,147,-39,145,-39,146,-39,155,-39,158,-39,157,-39,156,-39,76,-39,57,-39,91,-39,40,-39,25,-39,97,-39,54,-39,35,-39,55,-39,102,-39,47,-39,36,-39,53,-39,60,-39,74,-39,72,-39,38,-39,11,-39,10,-39,44,-39,37,-39,2,-39},new int[]{-306,1698,-305,1703});
    states[1698] = new State(-65,new int[]{-44,1699});
    states[1699] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,762,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,11,659,10,-494,2,-494,44,-212,37,-212},new int[]{-253,1700,-6,1701,-262,760,-261,22,-4,23,-113,24,-132,419,-111,429,-147,761,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895,-143,1050,-251,1137});
    states[1700] = new State(new int[]{10,20,2,-5});
    states[1701] = new State(new int[]{44,1541,37,1548,11,659},new int[]{-221,1702,-251,506,-228,1532,-225,1325,-229,1360});
    states[1702] = new State(-66);
    states[1703] = new State(-40);
    states[1704] = new State(new int[]{52,1588,141,-39,143,-39,85,-39,86,-39,80,-39,78,-39,160,-39,87,-39,45,-39,42,-39,8,-39,21,-39,22,-39,144,-39,147,-39,145,-39,146,-39,155,-39,158,-39,157,-39,156,-39,76,-39,57,-39,91,-39,40,-39,25,-39,97,-39,54,-39,35,-39,55,-39,102,-39,47,-39,36,-39,53,-39,60,-39,74,-39,72,-39,38,-39,11,-39,10,-39,44,-39,37,-39,2,-39},new int[]{-306,1705,-305,1703});
    states[1705] = new State(-65,new int[]{-44,1706});
    states[1706] = new State(new int[]{141,428,143,44,85,46,86,47,80,49,78,292,160,51,87,52,45,145,42,518,8,519,21,323,22,328,144,310,147,311,145,313,146,314,155,762,158,317,157,318,156,319,76,552,57,741,91,17,40,733,25,748,97,764,54,769,35,774,55,785,102,791,47,798,36,801,53,810,60,882,74,887,72,874,38,896,11,659,10,-494,2,-494,44,-212,37,-212},new int[]{-253,1707,-6,1701,-262,760,-261,22,-4,23,-113,24,-132,419,-111,429,-147,761,-151,45,-152,48,-191,439,-258,527,-295,528,-15,719,-165,369,-167,308,-166,312,-16,370,-17,529,-112,556,-58,720,-116,560,-212,739,-133,740,-256,745,-153,746,-35,747,-248,763,-319,768,-124,773,-320,784,-160,789,-302,790,-249,797,-123,800,-315,809,-59,878,-174,879,-173,880,-169,881,-126,886,-127,893,-125,894,-349,895,-143,1050,-251,1137});
    states[1707] = new State(new int[]{10,20,2,-6});

    rules[1] = new Rule(-362, new int[]{-1,2});
    rules[2] = new Rule(-1, new int[]{-234});
    rules[3] = new Rule(-1, new int[]{-307});
    rules[4] = new Rule(-1, new int[]{-175});
    rules[5] = new Rule(-1, new int[]{75,-306,-44,-253});
    rules[6] = new Rule(-1, new int[]{77,-306,-44,-253});
    rules[7] = new Rule(-175, new int[]{88,-87});
    rules[8] = new Rule(-175, new int[]{88,50,-144});
    rules[9] = new Rule(-175, new int[]{90,-321});
    rules[10] = new Rule(-175, new int[]{89,-260});
    rules[11] = new Rule(-260, new int[]{-87});
    rules[12] = new Rule(-260, new int[]{-4});
    rules[13] = new Rule(-260, new int[]{-315});
    rules[14] = new Rule(-185, new int[]{});
    rules[15] = new Rule(-185, new int[]{-186});
    rules[16] = new Rule(-186, new int[]{-184});
    rules[17] = new Rule(-186, new int[]{-186,-184});
    rules[18] = new Rule(-184, new int[]{3,143});
    rules[19] = new Rule(-184, new int[]{3,144});
    rules[20] = new Rule(-234, new int[]{-235,-185,-303,-18,-188});
    rules[21] = new Rule(-188, new int[]{7});
    rules[22] = new Rule(-188, new int[]{10});
    rules[23] = new Rule(-188, new int[]{5});
    rules[24] = new Rule(-188, new int[]{100});
    rules[25] = new Rule(-188, new int[]{6});
    rules[26] = new Rule(-188, new int[]{});
    rules[27] = new Rule(-235, new int[]{});
    rules[28] = new Rule(-235, new int[]{61,-147,-187});
    rules[29] = new Rule(-187, new int[]{10});
    rules[30] = new Rule(-187, new int[]{8,-189,9,10});
    rules[31] = new Rule(-189, new int[]{-146});
    rules[32] = new Rule(-189, new int[]{-189,100,-146});
    rules[33] = new Rule(-146, new int[]{-147});
    rules[34] = new Rule(-18, new int[]{-37,-256});
    rules[35] = new Rule(-37, new int[]{-41});
    rules[36] = new Rule(-157, new int[]{-138});
    rules[37] = new Rule(-157, new int[]{-157,7,-138});
    rules[38] = new Rule(-305, new int[]{52,-304,10});
    rules[39] = new Rule(-306, new int[]{});
    rules[40] = new Rule(-306, new int[]{-305});
    rules[41] = new Rule(-303, new int[]{});
    rules[42] = new Rule(-303, new int[]{-303,-305});
    rules[43] = new Rule(-304, new int[]{-308});
    rules[44] = new Rule(-304, new int[]{-304,100,-308});
    rules[45] = new Rule(-308, new int[]{-157});
    rules[46] = new Rule(-308, new int[]{-157,137,144});
    rules[47] = new Rule(-307, new int[]{-309,-162,-161,-154,7});
    rules[48] = new Rule(-307, new int[]{-309,-163,-154,7});
    rules[49] = new Rule(-309, new int[]{-2,-139,10,-185});
    rules[50] = new Rule(-309, new int[]{109,-157,10,-185});
    rules[51] = new Rule(-2, new int[]{105});
    rules[52] = new Rule(-2, new int[]{106});
    rules[53] = new Rule(-139, new int[]{-147});
    rules[54] = new Rule(-162, new int[]{43,-303,-40});
    rules[55] = new Rule(-161, new int[]{41,-303,-41});
    rules[56] = new Rule(-163, new int[]{-303,-41});
    rules[57] = new Rule(-154, new int[]{92});
    rules[58] = new Rule(-154, new int[]{103,-253,92});
    rules[59] = new Rule(-154, new int[]{103,-253,104,-253,92});
    rules[60] = new Rule(-154, new int[]{91,-253,92});
    rules[61] = new Rule(-40, new int[]{-38});
    rules[62] = new Rule(-38, new int[]{});
    rules[63] = new Rule(-38, new int[]{-38,-48});
    rules[64] = new Rule(-41, new int[]{-42});
    rules[65] = new Rule(-44, new int[]{});
    rules[66] = new Rule(-44, new int[]{-44,-6,-221});
    rules[67] = new Rule(-42, new int[]{});
    rules[68] = new Rule(-42, new int[]{-42,-47});
    rules[69] = new Rule(-43, new int[]{-39});
    rules[70] = new Rule(-39, new int[]{});
    rules[71] = new Rule(-39, new int[]{-39,-46});
    rules[72] = new Rule(-48, new int[]{-29});
    rules[73] = new Rule(-48, new int[]{-52});
    rules[74] = new Rule(-48, new int[]{-289});
    rules[75] = new Rule(-48, new int[]{-310});
    rules[76] = new Rule(-48, new int[]{-232});
    rules[77] = new Rule(-48, new int[]{-231});
    rules[78] = new Rule(-47, new int[]{-168});
    rules[79] = new Rule(-47, new int[]{-29});
    rules[80] = new Rule(-47, new int[]{-52});
    rules[81] = new Rule(-47, new int[]{-289});
    rules[82] = new Rule(-47, new int[]{-310});
    rules[83] = new Rule(-47, new int[]{-220});
    rules[84] = new Rule(-213, new int[]{-214});
    rules[85] = new Rule(-213, new int[]{-217});
    rules[86] = new Rule(-220, new int[]{-6,-213});
    rules[87] = new Rule(-46, new int[]{-168});
    rules[88] = new Rule(-46, new int[]{-29});
    rules[89] = new Rule(-46, new int[]{-52});
    rules[90] = new Rule(-46, new int[]{-289});
    rules[91] = new Rule(-46, new int[]{-310});
    rules[92] = new Rule(-232, new int[]{-6,-225});
    rules[93] = new Rule(-232, new int[]{-6,-225,149,10});
    rules[94] = new Rule(-231, new int[]{-6,-229});
    rules[95] = new Rule(-231, new int[]{-6,-229,149,10});
    rules[96] = new Rule(-168, new int[]{59,-156,10});
    rules[97] = new Rule(-156, new int[]{-143});
    rules[98] = new Rule(-156, new int[]{-156,100,-143});
    rules[99] = new Rule(-143, new int[]{155});
    rules[100] = new Rule(-143, new int[]{-147});
    rules[101] = new Rule(-29, new int[]{29,-27});
    rules[102] = new Rule(-29, new int[]{-29,-27});
    rules[103] = new Rule(-52, new int[]{66,-27});
    rules[104] = new Rule(-52, new int[]{-52,-27});
    rules[105] = new Rule(-289, new int[]{50,-49});
    rules[106] = new Rule(-289, new int[]{-289,-49});
    rules[107] = new Rule(-314, new int[]{-311});
    rules[108] = new Rule(-314, new int[]{8,-147,100,-158,9,110,-101,10});
    rules[109] = new Rule(-310, new int[]{53,-314});
    rules[110] = new Rule(-310, new int[]{62,-314});
    rules[111] = new Rule(-310, new int[]{-310,-314});
    rules[112] = new Rule(-27, new int[]{-28,10});
    rules[113] = new Rule(-28, new int[]{-141,120,-109});
    rules[114] = new Rule(-28, new int[]{-141,5,-277,120,-84});
    rules[115] = new Rule(-109, new int[]{-90});
    rules[116] = new Rule(-109, new int[]{-95});
    rules[117] = new Rule(-141, new int[]{-147});
    rules[118] = new Rule(-91, new int[]{-81});
    rules[119] = new Rule(-91, new int[]{-91,-192,-81});
    rules[120] = new Rule(-90, new int[]{-91});
    rules[121] = new Rule(-90, new int[]{-242});
    rules[122] = new Rule(-90, new int[]{-90,16,-91});
    rules[123] = new Rule(-242, new int[]{-90,13,-90,5,-90});
    rules[124] = new Rule(-192, new int[]{120});
    rules[125] = new Rule(-192, new int[]{125});
    rules[126] = new Rule(-192, new int[]{123});
    rules[127] = new Rule(-192, new int[]{121});
    rules[128] = new Rule(-192, new int[]{124});
    rules[129] = new Rule(-192, new int[]{122});
    rules[130] = new Rule(-192, new int[]{137});
    rules[131] = new Rule(-81, new int[]{-13});
    rules[132] = new Rule(-81, new int[]{-81,-193,-13});
    rules[133] = new Rule(-193, new int[]{116});
    rules[134] = new Rule(-193, new int[]{115});
    rules[135] = new Rule(-193, new int[]{128});
    rules[136] = new Rule(-193, new int[]{129});
    rules[137] = new Rule(-266, new int[]{-13,-201,-284});
    rules[138] = new Rule(-270, new int[]{-11,119,-10});
    rules[139] = new Rule(-270, new int[]{-11,119,-270});
    rules[140] = new Rule(-270, new int[]{-199,-270});
    rules[141] = new Rule(-13, new int[]{-10});
    rules[142] = new Rule(-13, new int[]{-266});
    rules[143] = new Rule(-13, new int[]{-270});
    rules[144] = new Rule(-13, new int[]{-13,-195,-10});
    rules[145] = new Rule(-13, new int[]{-13,-195,-270});
    rules[146] = new Rule(-195, new int[]{118});
    rules[147] = new Rule(-195, new int[]{117});
    rules[148] = new Rule(-195, new int[]{131});
    rules[149] = new Rule(-195, new int[]{132});
    rules[150] = new Rule(-195, new int[]{133});
    rules[151] = new Rule(-195, new int[]{134});
    rules[152] = new Rule(-195, new int[]{130});
    rules[153] = new Rule(-11, new int[]{-14});
    rules[154] = new Rule(-11, new int[]{8,-90,9});
    rules[155] = new Rule(-10, new int[]{-14});
    rules[156] = new Rule(-10, new int[]{-239});
    rules[157] = new Rule(-10, new int[]{56});
    rules[158] = new Rule(-10, new int[]{141,-10});
    rules[159] = new Rule(-10, new int[]{8,-90,9});
    rules[160] = new Rule(-10, new int[]{135,-10});
    rules[161] = new Rule(-10, new int[]{-199,-10});
    rules[162] = new Rule(-10, new int[]{-173});
    rules[163] = new Rule(-10, new int[]{-57});
    rules[164] = new Rule(-240, new int[]{11,-69,12});
    rules[165] = new Rule(-239, new int[]{-240});
    rules[166] = new Rule(-239, new int[]{76,-69,76});
    rules[167] = new Rule(-199, new int[]{116});
    rules[168] = new Rule(-199, new int[]{115});
    rules[169] = new Rule(-14, new int[]{-147});
    rules[170] = new Rule(-14, new int[]{-165});
    rules[171] = new Rule(-14, new int[]{-16});
    rules[172] = new Rule(-14, new int[]{42,-147});
    rules[173] = new Rule(-14, new int[]{-258});
    rules[174] = new Rule(-14, new int[]{-295});
    rules[175] = new Rule(-14, new int[]{-14,-12});
    rules[176] = new Rule(-14, new int[]{-14,4,-299});
    rules[177] = new Rule(-14, new int[]{-14,11,-121,12});
    rules[178] = new Rule(-12, new int[]{7,-138});
    rules[179] = new Rule(-12, new int[]{142});
    rules[180] = new Rule(-12, new int[]{8,-76,9});
    rules[181] = new Rule(-12, new int[]{11,-75,12});
    rules[182] = new Rule(-76, new int[]{-71});
    rules[183] = new Rule(-76, new int[]{});
    rules[184] = new Rule(-75, new int[]{-73});
    rules[185] = new Rule(-75, new int[]{});
    rules[186] = new Rule(-73, new int[]{-94});
    rules[187] = new Rule(-73, new int[]{-73,100,-94});
    rules[188] = new Rule(-94, new int[]{-90});
    rules[189] = new Rule(-94, new int[]{-90,6,-90});
    rules[190] = new Rule(-16, new int[]{155});
    rules[191] = new Rule(-16, new int[]{158});
    rules[192] = new Rule(-16, new int[]{157});
    rules[193] = new Rule(-16, new int[]{156});
    rules[194] = new Rule(-84, new int[]{-90});
    rules[195] = new Rule(-84, new int[]{-95});
    rules[196] = new Rule(-84, new int[]{-244});
    rules[197] = new Rule(-95, new int[]{8,-66,9});
    rules[198] = new Rule(-66, new int[]{});
    rules[199] = new Rule(-66, new int[]{-65});
    rules[200] = new Rule(-65, new int[]{-85});
    rules[201] = new Rule(-65, new int[]{-65,100,-85});
    rules[202] = new Rule(-244, new int[]{8,-246,9});
    rules[203] = new Rule(-246, new int[]{-245});
    rules[204] = new Rule(-246, new int[]{-245,10});
    rules[205] = new Rule(-245, new int[]{-247});
    rules[206] = new Rule(-245, new int[]{-245,10,-247});
    rules[207] = new Rule(-247, new int[]{-136,5,-84});
    rules[208] = new Rule(-136, new int[]{-147});
    rules[209] = new Rule(-49, new int[]{-6,-50});
    rules[210] = new Rule(-6, new int[]{-251});
    rules[211] = new Rule(-6, new int[]{-6,-251});
    rules[212] = new Rule(-6, new int[]{});
    rules[213] = new Rule(-251, new int[]{11,-252,12});
    rules[214] = new Rule(-252, new int[]{-8});
    rules[215] = new Rule(-252, new int[]{-252,100,-8});
    rules[216] = new Rule(-8, new int[]{-9});
    rules[217] = new Rule(-8, new int[]{-147,5,-9});
    rules[218] = new Rule(-50, new int[]{-144,120,-287,10});
    rules[219] = new Rule(-50, new int[]{-145,-287,10});
    rules[220] = new Rule(-144, new int[]{-147});
    rules[221] = new Rule(-144, new int[]{-147,-155});
    rules[222] = new Rule(-145, new int[]{-147,123,-158,122});
    rules[223] = new Rule(-287, new int[]{-277});
    rules[224] = new Rule(-287, new int[]{-30});
    rules[225] = new Rule(-274, new int[]{-273,13});
    rules[226] = new Rule(-274, new int[]{-301,13});
    rules[227] = new Rule(-277, new int[]{-273});
    rules[228] = new Rule(-277, new int[]{-274});
    rules[229] = new Rule(-277, new int[]{-257});
    rules[230] = new Rule(-277, new int[]{-250});
    rules[231] = new Rule(-277, new int[]{-282});
    rules[232] = new Rule(-277, new int[]{-226});
    rules[233] = new Rule(-277, new int[]{-301});
    rules[234] = new Rule(-301, new int[]{-180,-299});
    rules[235] = new Rule(-299, new int[]{123,-297,121});
    rules[236] = new Rule(-300, new int[]{125});
    rules[237] = new Rule(-300, new int[]{123,-298,121});
    rules[238] = new Rule(-297, new int[]{-280});
    rules[239] = new Rule(-297, new int[]{-297,100,-280});
    rules[240] = new Rule(-298, new int[]{-281});
    rules[241] = new Rule(-298, new int[]{-298,100,-281});
    rules[242] = new Rule(-281, new int[]{});
    rules[243] = new Rule(-280, new int[]{-273});
    rules[244] = new Rule(-280, new int[]{-273,13});
    rules[245] = new Rule(-280, new int[]{-282});
    rules[246] = new Rule(-280, new int[]{-226});
    rules[247] = new Rule(-280, new int[]{-301});
    rules[248] = new Rule(-273, new int[]{-93});
    rules[249] = new Rule(-273, new int[]{-93,6,-93});
    rules[250] = new Rule(-273, new int[]{8,-80,9});
    rules[251] = new Rule(-93, new int[]{-106});
    rules[252] = new Rule(-93, new int[]{-93,-193,-106});
    rules[253] = new Rule(-106, new int[]{-107});
    rules[254] = new Rule(-106, new int[]{-106,-195,-107});
    rules[255] = new Rule(-107, new int[]{-180});
    rules[256] = new Rule(-107, new int[]{-16});
    rules[257] = new Rule(-107, new int[]{-199,-107});
    rules[258] = new Rule(-107, new int[]{-165});
    rules[259] = new Rule(-107, new int[]{-107,8,-75,9});
    rules[260] = new Rule(-180, new int[]{-147});
    rules[261] = new Rule(-180, new int[]{-180,7,-138});
    rules[262] = new Rule(-80, new int[]{-78});
    rules[263] = new Rule(-80, new int[]{-80,100,-78});
    rules[264] = new Rule(-78, new int[]{-277});
    rules[265] = new Rule(-78, new int[]{-277,120,-87});
    rules[266] = new Rule(-250, new int[]{142,-276});
    rules[267] = new Rule(-282, new int[]{-279});
    rules[268] = new Rule(-282, new int[]{-31});
    rules[269] = new Rule(-282, new int[]{-264});
    rules[270] = new Rule(-282, new int[]{-130});
    rules[271] = new Rule(-282, new int[]{-131});
    rules[272] = new Rule(-131, new int[]{73,58,-277});
    rules[273] = new Rule(-279, new int[]{24,11,-164,12,58,-277});
    rules[274] = new Rule(-279, new int[]{-271});
    rules[275] = new Rule(-271, new int[]{24,58,-277});
    rules[276] = new Rule(-164, new int[]{-272});
    rules[277] = new Rule(-164, new int[]{-164,100,-272});
    rules[278] = new Rule(-272, new int[]{-273});
    rules[279] = new Rule(-272, new int[]{});
    rules[280] = new Rule(-264, new int[]{49,58,-277});
    rules[281] = new Rule(-130, new int[]{34,58,-277});
    rules[282] = new Rule(-130, new int[]{34});
    rules[283] = new Rule(-257, new int[]{143,11,-90,12});
    rules[284] = new Rule(-226, new int[]{-224});
    rules[285] = new Rule(-224, new int[]{-223});
    rules[286] = new Rule(-223, new int[]{44,-128});
    rules[287] = new Rule(-223, new int[]{37,-128,5,-276});
    rules[288] = new Rule(-223, new int[]{-180,127,-280});
    rules[289] = new Rule(-223, new int[]{-301,127,-280});
    rules[290] = new Rule(-223, new int[]{8,9,127,-280});
    rules[291] = new Rule(-223, new int[]{8,-80,9,127,-280});
    rules[292] = new Rule(-223, new int[]{-180,127,8,9});
    rules[293] = new Rule(-223, new int[]{-301,127,8,9});
    rules[294] = new Rule(-223, new int[]{8,9,127,8,9});
    rules[295] = new Rule(-223, new int[]{8,-80,9,127,8,9});
    rules[296] = new Rule(-30, new int[]{-21,-291,-183,-318,-26});
    rules[297] = new Rule(-31, new int[]{48,-183,-318,-25,92});
    rules[298] = new Rule(-20, new int[]{68});
    rules[299] = new Rule(-20, new int[]{69});
    rules[300] = new Rule(-20, new int[]{148});
    rules[301] = new Rule(-20, new int[]{27});
    rules[302] = new Rule(-20, new int[]{28});
    rules[303] = new Rule(-21, new int[]{});
    rules[304] = new Rule(-21, new int[]{-22});
    rules[305] = new Rule(-22, new int[]{-20});
    rules[306] = new Rule(-22, new int[]{-22,-20});
    rules[307] = new Rule(-291, new int[]{26});
    rules[308] = new Rule(-291, new int[]{43});
    rules[309] = new Rule(-291, new int[]{64});
    rules[310] = new Rule(-291, new int[]{64,26});
    rules[311] = new Rule(-291, new int[]{64,48});
    rules[312] = new Rule(-291, new int[]{64,43});
    rules[313] = new Rule(-26, new int[]{});
    rules[314] = new Rule(-26, new int[]{-25,92});
    rules[315] = new Rule(-183, new int[]{});
    rules[316] = new Rule(-183, new int[]{8,-182,9});
    rules[317] = new Rule(-182, new int[]{-181});
    rules[318] = new Rule(-182, new int[]{-182,100,-181});
    rules[319] = new Rule(-181, new int[]{-180});
    rules[320] = new Rule(-181, new int[]{-301});
    rules[321] = new Rule(-155, new int[]{123,-158,121});
    rules[322] = new Rule(-318, new int[]{});
    rules[323] = new Rule(-318, new int[]{-317});
    rules[324] = new Rule(-317, new int[]{-316});
    rules[325] = new Rule(-317, new int[]{-317,-316});
    rules[326] = new Rule(-316, new int[]{23,-158,5,-288,10});
    rules[327] = new Rule(-288, new int[]{-286});
    rules[328] = new Rule(-288, new int[]{-288,100,-286});
    rules[329] = new Rule(-286, new int[]{-277});
    rules[330] = new Rule(-286, new int[]{26});
    rules[331] = new Rule(-286, new int[]{48});
    rules[332] = new Rule(-286, new int[]{30});
    rules[333] = new Rule(-25, new int[]{-32});
    rules[334] = new Rule(-25, new int[]{-25,-7,-32});
    rules[335] = new Rule(-7, new int[]{84});
    rules[336] = new Rule(-7, new int[]{83});
    rules[337] = new Rule(-7, new int[]{82});
    rules[338] = new Rule(-7, new int[]{81});
    rules[339] = new Rule(-32, new int[]{});
    rules[340] = new Rule(-32, new int[]{-34,-190});
    rules[341] = new Rule(-32, new int[]{-33});
    rules[342] = new Rule(-32, new int[]{-34,10,-33});
    rules[343] = new Rule(-158, new int[]{-147});
    rules[344] = new Rule(-158, new int[]{-158,100,-147});
    rules[345] = new Rule(-190, new int[]{});
    rules[346] = new Rule(-190, new int[]{10});
    rules[347] = new Rule(-34, new int[]{-45});
    rules[348] = new Rule(-34, new int[]{-34,10,-45});
    rules[349] = new Rule(-45, new int[]{-6,-51});
    rules[350] = new Rule(-33, new int[]{-54});
    rules[351] = new Rule(-33, new int[]{-33,-54});
    rules[352] = new Rule(-54, new int[]{-53});
    rules[353] = new Rule(-54, new int[]{-55});
    rules[354] = new Rule(-51, new int[]{29,-28});
    rules[355] = new Rule(-51, new int[]{-313});
    rules[356] = new Rule(-51, new int[]{-3,-313});
    rules[357] = new Rule(-3, new int[]{28});
    rules[358] = new Rule(-3, new int[]{26});
    rules[359] = new Rule(-313, new int[]{-312});
    rules[360] = new Rule(-313, new int[]{62,-158,5,-277});
    rules[361] = new Rule(-53, new int[]{-6,-222});
    rules[362] = new Rule(-53, new int[]{-6,-219});
    rules[363] = new Rule(-219, new int[]{-215});
    rules[364] = new Rule(-219, new int[]{-218});
    rules[365] = new Rule(-222, new int[]{-3,-230});
    rules[366] = new Rule(-222, new int[]{-230});
    rules[367] = new Rule(-222, new int[]{19,-3,-230});
    rules[368] = new Rule(-222, new int[]{19,-230});
    rules[369] = new Rule(-222, new int[]{-227});
    rules[370] = new Rule(-230, new int[]{-228});
    rules[371] = new Rule(-228, new int[]{-225});
    rules[372] = new Rule(-228, new int[]{-229});
    rules[373] = new Rule(-227, new int[]{30,-172,-128,-207});
    rules[374] = new Rule(-227, new int[]{-3,30,-172,-128,-207});
    rules[375] = new Rule(-227, new int[]{31,-172,-128,-207});
    rules[376] = new Rule(-172, new int[]{-171});
    rules[377] = new Rule(-172, new int[]{});
    rules[378] = new Rule(-55, new int[]{-6,-259});
    rules[379] = new Rule(-259, new int[]{46,-170,-233,-202,10,-205});
    rules[380] = new Rule(-259, new int[]{46,-170,-233,-202,10,-210,10,-205});
    rules[381] = new Rule(-259, new int[]{-3,46,-170,-233,-202,10,-205});
    rules[382] = new Rule(-259, new int[]{-3,46,-170,-233,-202,10,-210,10,-205});
    rules[383] = new Rule(-259, new int[]{27,46,-170,-233,-211,10});
    rules[384] = new Rule(-259, new int[]{-3,27,46,-170,-233,-211,10});
    rules[385] = new Rule(-211, new int[]{110,-87});
    rules[386] = new Rule(-211, new int[]{});
    rules[387] = new Rule(-205, new int[]{});
    rules[388] = new Rule(-205, new int[]{63,10});
    rules[389] = new Rule(-233, new int[]{-238,5,-276});
    rules[390] = new Rule(-238, new int[]{});
    rules[391] = new Rule(-238, new int[]{11,-237,12});
    rules[392] = new Rule(-237, new int[]{-236});
    rules[393] = new Rule(-237, new int[]{-237,10,-236});
    rules[394] = new Rule(-236, new int[]{-158,5,-276});
    rules[395] = new Rule(-114, new int[]{-88});
    rules[396] = new Rule(-114, new int[]{});
    rules[397] = new Rule(-202, new int[]{});
    rules[398] = new Rule(-202, new int[]{85,-114,-203});
    rules[399] = new Rule(-202, new int[]{86,-261,-204});
    rules[400] = new Rule(-203, new int[]{});
    rules[401] = new Rule(-203, new int[]{86,-261});
    rules[402] = new Rule(-204, new int[]{});
    rules[403] = new Rule(-204, new int[]{85,-114});
    rules[404] = new Rule(-311, new int[]{-312,10});
    rules[405] = new Rule(-339, new int[]{110});
    rules[406] = new Rule(-339, new int[]{120});
    rules[407] = new Rule(-312, new int[]{-158,5,-277});
    rules[408] = new Rule(-312, new int[]{-158,110,-88});
    rules[409] = new Rule(-312, new int[]{-158,5,-277,-339,-86});
    rules[410] = new Rule(-86, new int[]{-85});
    rules[411] = new Rule(-86, new int[]{-81,6,-13});
    rules[412] = new Rule(-86, new int[]{-324});
    rules[413] = new Rule(-86, new int[]{-147,127,-329});
    rules[414] = new Rule(-86, new int[]{8,9,-325,127,-329});
    rules[415] = new Rule(-86, new int[]{8,-66,9,127,-329});
    rules[416] = new Rule(-86, new int[]{-243});
    rules[417] = new Rule(-85, new int[]{-84});
    rules[418] = new Rule(-217, new int[]{-227,-177});
    rules[419] = new Rule(-217, new int[]{30,-172,-128,110,-261,10});
    rules[420] = new Rule(-217, new int[]{-3,30,-172,-128,110,-261,10});
    rules[421] = new Rule(-218, new int[]{-227,-176});
    rules[422] = new Rule(-218, new int[]{30,-172,-128,110,-261,10});
    rules[423] = new Rule(-218, new int[]{-3,30,-172,-128,110,-261,10});
    rules[424] = new Rule(-214, new int[]{-221});
    rules[425] = new Rule(-214, new int[]{-3,-221});
    rules[426] = new Rule(-214, new int[]{19,-221});
    rules[427] = new Rule(-214, new int[]{19,-3,-221});
    rules[428] = new Rule(-214, new int[]{-3,19,-221});
    rules[429] = new Rule(-221, new int[]{-228,-178});
    rules[430] = new Rule(-221, new int[]{37,-170,-128,5,-276,-208,110,-101,10});
    rules[431] = new Rule(-221, new int[]{37,-170,-128,-208,110,-101,10});
    rules[432] = new Rule(-221, new int[]{37,-170,-128,5,-276,-208,110,-323,10});
    rules[433] = new Rule(-221, new int[]{37,-170,-128,-208,110,-323,10});
    rules[434] = new Rule(-221, new int[]{44,-171,-128,-208,110,-261,10});
    rules[435] = new Rule(-221, new int[]{-228,149,10});
    rules[436] = new Rule(-215, new int[]{-216});
    rules[437] = new Rule(-215, new int[]{19,-216});
    rules[438] = new Rule(-215, new int[]{-3,-216});
    rules[439] = new Rule(-215, new int[]{19,-3,-216});
    rules[440] = new Rule(-215, new int[]{-3,19,-216});
    rules[441] = new Rule(-216, new int[]{-228,-176});
    rules[442] = new Rule(-216, new int[]{37,-170,-128,5,-276,-208,110,-103,10});
    rules[443] = new Rule(-216, new int[]{37,-170,-128,-208,110,-103,10});
    rules[444] = new Rule(-216, new int[]{44,-171,-128,-208,110,-261,10});
    rules[445] = new Rule(-178, new int[]{-177});
    rules[446] = new Rule(-178, new int[]{-61});
    rules[447] = new Rule(-171, new int[]{-170});
    rules[448] = new Rule(-170, new int[]{-142});
    rules[449] = new Rule(-170, new int[]{-335,7,-142});
    rules[450] = new Rule(-149, new int[]{-137});
    rules[451] = new Rule(-335, new int[]{-149});
    rules[452] = new Rule(-335, new int[]{-335,7,-149});
    rules[453] = new Rule(-142, new int[]{-137});
    rules[454] = new Rule(-142, new int[]{-191});
    rules[455] = new Rule(-142, new int[]{-191,-155});
    rules[456] = new Rule(-137, new int[]{-134});
    rules[457] = new Rule(-137, new int[]{-134,-155});
    rules[458] = new Rule(-134, new int[]{-147});
    rules[459] = new Rule(-225, new int[]{44,-171,-128,-207,-318});
    rules[460] = new Rule(-229, new int[]{37,-170,-128,-207,-318});
    rules[461] = new Rule(-229, new int[]{37,-170,-128,5,-276,-207,-318});
    rules[462] = new Rule(-61, new int[]{107,-108,80,-108,10});
    rules[463] = new Rule(-61, new int[]{107,-108,10});
    rules[464] = new Rule(-61, new int[]{107,10});
    rules[465] = new Rule(-108, new int[]{-147});
    rules[466] = new Rule(-108, new int[]{-165});
    rules[467] = new Rule(-177, new int[]{-41,-256,10});
    rules[468] = new Rule(-176, new int[]{-43,-256,10});
    rules[469] = new Rule(-176, new int[]{-61});
    rules[470] = new Rule(-128, new int[]{});
    rules[471] = new Rule(-128, new int[]{8,9});
    rules[472] = new Rule(-128, new int[]{8,-129,9});
    rules[473] = new Rule(-129, new int[]{-56});
    rules[474] = new Rule(-129, new int[]{-129,10,-56});
    rules[475] = new Rule(-56, new int[]{-6,-296});
    rules[476] = new Rule(-296, new int[]{-159,5,-276});
    rules[477] = new Rule(-296, new int[]{53,-159,5,-276});
    rules[478] = new Rule(-296, new int[]{29,-159,5,-276});
    rules[479] = new Rule(-296, new int[]{108,-159,5,-276});
    rules[480] = new Rule(-296, new int[]{-159,5,-276,110,-87});
    rules[481] = new Rule(-296, new int[]{53,-159,5,-276,110,-87});
    rules[482] = new Rule(-296, new int[]{29,-159,5,-276,110,-87});
    rules[483] = new Rule(-159, new int[]{-135});
    rules[484] = new Rule(-159, new int[]{-159,100,-135});
    rules[485] = new Rule(-135, new int[]{-147});
    rules[486] = new Rule(-276, new int[]{-277});
    rules[487] = new Rule(-278, new int[]{-273});
    rules[488] = new Rule(-278, new int[]{-257});
    rules[489] = new Rule(-278, new int[]{-250});
    rules[490] = new Rule(-278, new int[]{-282});
    rules[491] = new Rule(-278, new int[]{-301});
    rules[492] = new Rule(-262, new int[]{-261});
    rules[493] = new Rule(-262, new int[]{-143,5,-262});
    rules[494] = new Rule(-261, new int[]{});
    rules[495] = new Rule(-261, new int[]{-4});
    rules[496] = new Rule(-261, new int[]{-212});
    rules[497] = new Rule(-261, new int[]{-133});
    rules[498] = new Rule(-261, new int[]{-256});
    rules[499] = new Rule(-261, new int[]{-153});
    rules[500] = new Rule(-261, new int[]{-35});
    rules[501] = new Rule(-261, new int[]{-248});
    rules[502] = new Rule(-261, new int[]{-319});
    rules[503] = new Rule(-261, new int[]{-124});
    rules[504] = new Rule(-261, new int[]{-320});
    rules[505] = new Rule(-261, new int[]{-160});
    rules[506] = new Rule(-261, new int[]{-302});
    rules[507] = new Rule(-261, new int[]{-249});
    rules[508] = new Rule(-261, new int[]{-123});
    rules[509] = new Rule(-261, new int[]{-315});
    rules[510] = new Rule(-261, new int[]{-59});
    rules[511] = new Rule(-261, new int[]{-169});
    rules[512] = new Rule(-261, new int[]{-126});
    rules[513] = new Rule(-261, new int[]{-127});
    rules[514] = new Rule(-261, new int[]{-125});
    rules[515] = new Rule(-261, new int[]{-349});
    rules[516] = new Rule(-125, new int[]{72,-101,99,-261});
    rules[517] = new Rule(-126, new int[]{74,-103});
    rules[518] = new Rule(-127, new int[]{74,73,-103});
    rules[519] = new Rule(-315, new int[]{53,-312});
    rules[520] = new Rule(-315, new int[]{8,53,-147,100,-338,9,110,-87});
    rules[521] = new Rule(-315, new int[]{53,8,-147,100,-158,9,110,-87});
    rules[522] = new Rule(-4, new int[]{-113,-194,-88});
    rules[523] = new Rule(-4, new int[]{8,-111,100,-337,9,-194,-87});
    rules[524] = new Rule(-337, new int[]{-111});
    rules[525] = new Rule(-337, new int[]{-337,100,-111});
    rules[526] = new Rule(-338, new int[]{53,-147});
    rules[527] = new Rule(-338, new int[]{-338,100,53,-147});
    rules[528] = new Rule(-212, new int[]{-113});
    rules[529] = new Rule(-133, new int[]{57,-143});
    rules[530] = new Rule(-256, new int[]{91,-253,92});
    rules[531] = new Rule(-253, new int[]{-262});
    rules[532] = new Rule(-253, new int[]{-253,10,-262});
    rules[533] = new Rule(-153, new int[]{40,-101,51,-261});
    rules[534] = new Rule(-153, new int[]{40,-101,51,-261,32,-261});
    rules[535] = new Rule(-349, new int[]{38,-101,55,-351,-254,92});
    rules[536] = new Rule(-349, new int[]{38,-101,55,-351,10,-254,92});
    rules[537] = new Rule(-351, new int[]{-350});
    rules[538] = new Rule(-351, new int[]{-351,10,-350});
    rules[539] = new Rule(-350, new int[]{-343,39,-101,5,-261});
    rules[540] = new Rule(-350, new int[]{-342,5,-261});
    rules[541] = new Rule(-350, new int[]{-344,5,-261});
    rules[542] = new Rule(-350, new int[]{-345,39,-101,5,-261});
    rules[543] = new Rule(-350, new int[]{-345,5,-261});
    rules[544] = new Rule(-35, new int[]{25,-101,58,-36,-254,92});
    rules[545] = new Rule(-35, new int[]{25,-101,58,-36,10,-254,92});
    rules[546] = new Rule(-35, new int[]{25,-101,58,-254,92});
    rules[547] = new Rule(-36, new int[]{-263});
    rules[548] = new Rule(-36, new int[]{-36,10,-263});
    rules[549] = new Rule(-263, new int[]{-74,5,-261});
    rules[550] = new Rule(-74, new int[]{-110});
    rules[551] = new Rule(-74, new int[]{-74,100,-110});
    rules[552] = new Rule(-110, new int[]{-94});
    rules[553] = new Rule(-254, new int[]{});
    rules[554] = new Rule(-254, new int[]{32,-253});
    rules[555] = new Rule(-248, new int[]{97,-253,98,-87});
    rules[556] = new Rule(-319, new int[]{54,-101,-292,-261});
    rules[557] = new Rule(-292, new int[]{99});
    rules[558] = new Rule(-292, new int[]{});
    rules[559] = new Rule(-169, new int[]{60,-101,99,-261});
    rules[560] = new Rule(-361, new int[]{87,143});
    rules[561] = new Rule(-361, new int[]{});
    rules[562] = new Rule(-275, new int[]{5,-277});
    rules[563] = new Rule(-275, new int[]{});
    rules[564] = new Rule(-19, new int[]{53});
    rules[565] = new Rule(-19, new int[]{});
    rules[566] = new Rule(-119, new int[]{70});
    rules[567] = new Rule(-119, new int[]{71});
    rules[568] = new Rule(-123, new int[]{36,-147,-275,137,-101,-361,99,-261});
    rules[569] = new Rule(-123, new int[]{36,53,-147,-275,137,-101,-361,99,-261});
    rules[570] = new Rule(-123, new int[]{36,53,8,-158,9,137,-101,-361,99,-261});
    rules[571] = new Rule(-124, new int[]{35,-19,-147,-275,110,-101,-119,-101,-292,-261});
    rules[572] = new Rule(-124, new int[]{35,-19,-147,-275,110,-101,-119,-101,160,-101,99,-261});
    rules[573] = new Rule(-320, new int[]{55,-71,99,-261});
    rules[574] = new Rule(-160, new int[]{42});
    rules[575] = new Rule(-302, new int[]{102,-253,-290});
    rules[576] = new Rule(-290, new int[]{101,-253,92});
    rules[577] = new Rule(-290, new int[]{33,-60,92});
    rules[578] = new Rule(-60, new int[]{-63,-255});
    rules[579] = new Rule(-60, new int[]{-63,10,-255});
    rules[580] = new Rule(-60, new int[]{-253});
    rules[581] = new Rule(-63, new int[]{-62});
    rules[582] = new Rule(-63, new int[]{-63,10,-62});
    rules[583] = new Rule(-255, new int[]{});
    rules[584] = new Rule(-255, new int[]{32,-253});
    rules[585] = new Rule(-62, new int[]{79,-64,99,-261});
    rules[586] = new Rule(-64, new int[]{-179});
    rules[587] = new Rule(-64, new int[]{-140,5,-179});
    rules[588] = new Rule(-179, new int[]{-180});
    rules[589] = new Rule(-140, new int[]{-147});
    rules[590] = new Rule(-249, new int[]{47});
    rules[591] = new Rule(-249, new int[]{47,-87});
    rules[592] = new Rule(-71, new int[]{-88});
    rules[593] = new Rule(-71, new int[]{-71,100,-88});
    rules[594] = new Rule(-72, new int[]{-89});
    rules[595] = new Rule(-72, new int[]{-72,100,-89});
    rules[596] = new Rule(-59, new int[]{-174});
    rules[597] = new Rule(-174, new int[]{-173});
    rules[598] = new Rule(-88, new int[]{-87});
    rules[599] = new Rule(-88, new int[]{-323});
    rules[600] = new Rule(-88, new int[]{42});
    rules[601] = new Rule(-89, new int[]{-87});
    rules[602] = new Rule(-89, new int[]{-147,110,-101});
    rules[603] = new Rule(-89, new int[]{-323});
    rules[604] = new Rule(-89, new int[]{42});
    rules[605] = new Rule(-87, new int[]{-101});
    rules[606] = new Rule(-87, new int[]{-120});
    rules[607] = new Rule(-87, new int[]{-87,70,-101});
    rules[608] = new Rule(-101, new int[]{-99});
    rules[609] = new Rule(-101, new int[]{-241});
    rules[610] = new Rule(-101, new int[]{-243});
    rules[611] = new Rule(-117, new int[]{-99});
    rules[612] = new Rule(-117, new int[]{-241});
    rules[613] = new Rule(-118, new int[]{-99});
    rules[614] = new Rule(-118, new int[]{-243});
    rules[615] = new Rule(-103, new int[]{-101});
    rules[616] = new Rule(-103, new int[]{-323});
    rules[617] = new Rule(-104, new int[]{-99});
    rules[618] = new Rule(-104, new int[]{-241});
    rules[619] = new Rule(-104, new int[]{-323});
    rules[620] = new Rule(-99, new int[]{-98});
    rules[621] = new Rule(-99, new int[]{-99,16,-98});
    rules[622] = new Rule(-258, new int[]{21,8,-284,9});
    rules[623] = new Rule(-295, new int[]{22,8,-284,9});
    rules[624] = new Rule(-295, new int[]{22,8,-283,9});
    rules[625] = new Rule(-241, new int[]{-117,13,-117,5,-117});
    rules[626] = new Rule(-243, new int[]{40,-118,51,-118,32,-118});
    rules[627] = new Rule(-283, new int[]{-180,-300});
    rules[628] = new Rule(-283, new int[]{-180,4,-300});
    rules[629] = new Rule(-284, new int[]{-180});
    rules[630] = new Rule(-284, new int[]{-180,-299});
    rules[631] = new Rule(-284, new int[]{-180,4,-299});
    rules[632] = new Rule(-285, new int[]{-284});
    rules[633] = new Rule(-285, new int[]{-274});
    rules[634] = new Rule(-5, new int[]{8,-66,9});
    rules[635] = new Rule(-5, new int[]{});
    rules[636] = new Rule(-173, new int[]{78,-284,-70});
    rules[637] = new Rule(-173, new int[]{78,-284,11,-67,12,-5});
    rules[638] = new Rule(-173, new int[]{78,26,8,-334,9});
    rules[639] = new Rule(-333, new int[]{-147,110,-101});
    rules[640] = new Rule(-333, new int[]{-101});
    rules[641] = new Rule(-334, new int[]{-333});
    rules[642] = new Rule(-334, new int[]{-334,100,-333});
    rules[643] = new Rule(-70, new int[]{});
    rules[644] = new Rule(-70, new int[]{8,-67,9});
    rules[645] = new Rule(-98, new int[]{-105});
    rules[646] = new Rule(-98, new int[]{-98,-196,-105});
    rules[647] = new Rule(-98, new int[]{-98,-196,-243});
    rules[648] = new Rule(-98, new int[]{-267,8,-354,9});
    rules[649] = new Rule(-341, new int[]{-284,8,-354,9});
    rules[650] = new Rule(-343, new int[]{-284,8,-355,9});
    rules[651] = new Rule(-342, new int[]{-284,8,-355,9});
    rules[652] = new Rule(-342, new int[]{-358});
    rules[653] = new Rule(-358, new int[]{-340});
    rules[654] = new Rule(-358, new int[]{-358,100,-340});
    rules[655] = new Rule(-340, new int[]{-15});
    rules[656] = new Rule(-340, new int[]{-284});
    rules[657] = new Rule(-340, new int[]{56});
    rules[658] = new Rule(-340, new int[]{-258});
    rules[659] = new Rule(-340, new int[]{-295});
    rules[660] = new Rule(-344, new int[]{11,-356,12});
    rules[661] = new Rule(-356, new int[]{-346});
    rules[662] = new Rule(-356, new int[]{-356,100,-346});
    rules[663] = new Rule(-346, new int[]{-15});
    rules[664] = new Rule(-346, new int[]{-348});
    rules[665] = new Rule(-346, new int[]{14});
    rules[666] = new Rule(-346, new int[]{-343});
    rules[667] = new Rule(-346, new int[]{-344});
    rules[668] = new Rule(-346, new int[]{-345});
    rules[669] = new Rule(-346, new int[]{6});
    rules[670] = new Rule(-348, new int[]{53,-147});
    rules[671] = new Rule(-345, new int[]{8,-357,9});
    rules[672] = new Rule(-347, new int[]{14});
    rules[673] = new Rule(-347, new int[]{-15});
    rules[674] = new Rule(-347, new int[]{-199,-15});
    rules[675] = new Rule(-347, new int[]{53,-147});
    rules[676] = new Rule(-347, new int[]{-343});
    rules[677] = new Rule(-347, new int[]{-344});
    rules[678] = new Rule(-347, new int[]{-345});
    rules[679] = new Rule(-357, new int[]{-347});
    rules[680] = new Rule(-357, new int[]{-357,100,-347});
    rules[681] = new Rule(-355, new int[]{-353});
    rules[682] = new Rule(-355, new int[]{-355,10,-353});
    rules[683] = new Rule(-355, new int[]{-355,100,-353});
    rules[684] = new Rule(-354, new int[]{-352});
    rules[685] = new Rule(-354, new int[]{-354,10,-352});
    rules[686] = new Rule(-354, new int[]{-354,100,-352});
    rules[687] = new Rule(-352, new int[]{14});
    rules[688] = new Rule(-352, new int[]{-15});
    rules[689] = new Rule(-352, new int[]{53,-147,5,-277});
    rules[690] = new Rule(-352, new int[]{53,-147});
    rules[691] = new Rule(-352, new int[]{-341});
    rules[692] = new Rule(-352, new int[]{-344});
    rules[693] = new Rule(-352, new int[]{-345});
    rules[694] = new Rule(-353, new int[]{14});
    rules[695] = new Rule(-353, new int[]{-15});
    rules[696] = new Rule(-353, new int[]{-199,-15});
    rules[697] = new Rule(-353, new int[]{-147,5,-277});
    rules[698] = new Rule(-353, new int[]{-147});
    rules[699] = new Rule(-353, new int[]{53,-147,5,-277});
    rules[700] = new Rule(-353, new int[]{53,-147});
    rules[701] = new Rule(-353, new int[]{-343});
    rules[702] = new Rule(-353, new int[]{-344});
    rules[703] = new Rule(-353, new int[]{-345});
    rules[704] = new Rule(-115, new int[]{-105});
    rules[705] = new Rule(-115, new int[]{});
    rules[706] = new Rule(-122, new int[]{-90});
    rules[707] = new Rule(-122, new int[]{});
    rules[708] = new Rule(-120, new int[]{-105,5,-115});
    rules[709] = new Rule(-120, new int[]{5,-115});
    rules[710] = new Rule(-120, new int[]{-105,5,-115,5,-105});
    rules[711] = new Rule(-120, new int[]{5,-115,5,-105});
    rules[712] = new Rule(-121, new int[]{-90,5,-122});
    rules[713] = new Rule(-121, new int[]{5,-122});
    rules[714] = new Rule(-121, new int[]{-90,5,-122,5,-90});
    rules[715] = new Rule(-121, new int[]{5,-122,5,-90});
    rules[716] = new Rule(-196, new int[]{120});
    rules[717] = new Rule(-196, new int[]{125});
    rules[718] = new Rule(-196, new int[]{123});
    rules[719] = new Rule(-196, new int[]{121});
    rules[720] = new Rule(-196, new int[]{124});
    rules[721] = new Rule(-196, new int[]{122});
    rules[722] = new Rule(-196, new int[]{137});
    rules[723] = new Rule(-196, new int[]{135,137});
    rules[724] = new Rule(-105, new int[]{-83});
    rules[725] = new Rule(-105, new int[]{-105,6,-83});
    rules[726] = new Rule(-83, new int[]{-82});
    rules[727] = new Rule(-83, new int[]{-83,-197,-82});
    rules[728] = new Rule(-83, new int[]{-83,-197,-243});
    rules[729] = new Rule(-197, new int[]{116});
    rules[730] = new Rule(-197, new int[]{115});
    rules[731] = new Rule(-197, new int[]{128});
    rules[732] = new Rule(-197, new int[]{129});
    rules[733] = new Rule(-197, new int[]{126});
    rules[734] = new Rule(-201, new int[]{136});
    rules[735] = new Rule(-201, new int[]{138});
    rules[736] = new Rule(-265, new int[]{-267});
    rules[737] = new Rule(-265, new int[]{-268});
    rules[738] = new Rule(-268, new int[]{-82,136,-284});
    rules[739] = new Rule(-268, new int[]{-82,136,-279});
    rules[740] = new Rule(-267, new int[]{-82,138,-284});
    rules[741] = new Rule(-267, new int[]{-82,138,-279});
    rules[742] = new Rule(-269, new int[]{-97,119,-96});
    rules[743] = new Rule(-269, new int[]{-97,119,-269});
    rules[744] = new Rule(-269, new int[]{-199,-269});
    rules[745] = new Rule(-82, new int[]{-96});
    rules[746] = new Rule(-82, new int[]{-173});
    rules[747] = new Rule(-82, new int[]{-269});
    rules[748] = new Rule(-82, new int[]{-82,-198,-96});
    rules[749] = new Rule(-82, new int[]{-82,-198,-269});
    rules[750] = new Rule(-82, new int[]{-82,-198,-243});
    rules[751] = new Rule(-82, new int[]{-265});
    rules[752] = new Rule(-198, new int[]{118});
    rules[753] = new Rule(-198, new int[]{117});
    rules[754] = new Rule(-198, new int[]{131});
    rules[755] = new Rule(-198, new int[]{132});
    rules[756] = new Rule(-198, new int[]{133});
    rules[757] = new Rule(-198, new int[]{134});
    rules[758] = new Rule(-198, new int[]{130});
    rules[759] = new Rule(-57, new int[]{63,8,-285,9});
    rules[760] = new Rule(-58, new int[]{8,-102,100,-79,-325,-332,9});
    rules[761] = new Rule(-97, new int[]{-15});
    rules[762] = new Rule(-97, new int[]{-113});
    rules[763] = new Rule(-96, new int[]{56});
    rules[764] = new Rule(-96, new int[]{-15});
    rules[765] = new Rule(-96, new int[]{-57});
    rules[766] = new Rule(-96, new int[]{-240});
    rules[767] = new Rule(-96, new int[]{-96,7,-148});
    rules[768] = new Rule(-96, new int[]{-96,8,-68,9});
    rules[769] = new Rule(-96, new int[]{135,-96});
    rules[770] = new Rule(-96, new int[]{-199,-96});
    rules[771] = new Rule(-96, new int[]{142,-96});
    rules[772] = new Rule(-96, new int[]{-113});
    rules[773] = new Rule(-96, new int[]{-58});
    rules[774] = new Rule(-15, new int[]{-165});
    rules[775] = new Rule(-15, new int[]{-16});
    rules[776] = new Rule(-116, new int[]{-111,15,-111});
    rules[777] = new Rule(-116, new int[]{-111,15,-116});
    rules[778] = new Rule(-113, new int[]{-132,-111});
    rules[779] = new Rule(-113, new int[]{-111});
    rules[780] = new Rule(-113, new int[]{-116});
    rules[781] = new Rule(-113, new int[]{8,53,-147,110,-99,9});
    rules[782] = new Rule(-132, new int[]{141});
    rules[783] = new Rule(-132, new int[]{-132,141});
    rules[784] = new Rule(-9, new int[]{-180,-70});
    rules[785] = new Rule(-9, new int[]{-301,-70});
    rules[786] = new Rule(-322, new int[]{-147});
    rules[787] = new Rule(-322, new int[]{-322,7,-138});
    rules[788] = new Rule(-321, new int[]{-322});
    rules[789] = new Rule(-321, new int[]{-322,-299});
    rules[790] = new Rule(-17, new int[]{-111});
    rules[791] = new Rule(-17, new int[]{-15});
    rules[792] = new Rule(-359, new int[]{53,-147,110,-87,10});
    rules[793] = new Rule(-360, new int[]{-359});
    rules[794] = new Rule(-360, new int[]{-360,-359});
    rules[795] = new Rule(-112, new int[]{-111,8,-68,9});
    rules[796] = new Rule(-111, new int[]{-147});
    rules[797] = new Rule(-111, new int[]{-191});
    rules[798] = new Rule(-111, new int[]{42,-147});
    rules[799] = new Rule(-111, new int[]{8,-87,9});
    rules[800] = new Rule(-111, new int[]{8,-360,-87,9});
    rules[801] = new Rule(-111, new int[]{-258});
    rules[802] = new Rule(-111, new int[]{-295});
    rules[803] = new Rule(-111, new int[]{-15,7,-138});
    rules[804] = new Rule(-111, new int[]{-17,11,-71,12});
    rules[805] = new Rule(-111, new int[]{-17,17,-120,12});
    rules[806] = new Rule(-111, new int[]{76,-69,76});
    rules[807] = new Rule(-111, new int[]{-112});
    rules[808] = new Rule(-111, new int[]{-111,7,-148});
    rules[809] = new Rule(-111, new int[]{-58,7,-148});
    rules[810] = new Rule(-111, new int[]{-111,142});
    rules[811] = new Rule(-111, new int[]{-111,4,-299});
    rules[812] = new Rule(-67, new int[]{-71});
    rules[813] = new Rule(-67, new int[]{});
    rules[814] = new Rule(-68, new int[]{-72});
    rules[815] = new Rule(-68, new int[]{});
    rules[816] = new Rule(-69, new int[]{-77});
    rules[817] = new Rule(-69, new int[]{});
    rules[818] = new Rule(-77, new int[]{-92});
    rules[819] = new Rule(-77, new int[]{-77,100,-92});
    rules[820] = new Rule(-92, new int[]{-87});
    rules[821] = new Rule(-92, new int[]{-87,6,-87});
    rules[822] = new Rule(-166, new int[]{144});
    rules[823] = new Rule(-166, new int[]{147});
    rules[824] = new Rule(-165, new int[]{-167});
    rules[825] = new Rule(-165, new int[]{145});
    rules[826] = new Rule(-165, new int[]{146});
    rules[827] = new Rule(-167, new int[]{-166});
    rules[828] = new Rule(-167, new int[]{-167,-166});
    rules[829] = new Rule(-191, new int[]{45,-200});
    rules[830] = new Rule(-207, new int[]{10});
    rules[831] = new Rule(-207, new int[]{10,-206,10});
    rules[832] = new Rule(-208, new int[]{});
    rules[833] = new Rule(-208, new int[]{10,-206});
    rules[834] = new Rule(-206, new int[]{-209});
    rules[835] = new Rule(-206, new int[]{-206,10,-209});
    rules[836] = new Rule(-147, new int[]{143});
    rules[837] = new Rule(-147, new int[]{-151});
    rules[838] = new Rule(-147, new int[]{-152});
    rules[839] = new Rule(-147, new int[]{160});
    rules[840] = new Rule(-147, new int[]{87});
    rules[841] = new Rule(-138, new int[]{-147});
    rules[842] = new Rule(-138, new int[]{-293});
    rules[843] = new Rule(-138, new int[]{-294});
    rules[844] = new Rule(-148, new int[]{-147});
    rules[845] = new Rule(-148, new int[]{-293});
    rules[846] = new Rule(-148, new int[]{-191});
    rules[847] = new Rule(-209, new int[]{148});
    rules[848] = new Rule(-209, new int[]{150});
    rules[849] = new Rule(-209, new int[]{151});
    rules[850] = new Rule(-209, new int[]{152});
    rules[851] = new Rule(-209, new int[]{154});
    rules[852] = new Rule(-209, new int[]{153});
    rules[853] = new Rule(-210, new int[]{153});
    rules[854] = new Rule(-210, new int[]{152});
    rules[855] = new Rule(-210, new int[]{148});
    rules[856] = new Rule(-210, new int[]{151});
    rules[857] = new Rule(-151, new int[]{85});
    rules[858] = new Rule(-151, new int[]{86});
    rules[859] = new Rule(-152, new int[]{80});
    rules[860] = new Rule(-152, new int[]{78});
    rules[861] = new Rule(-150, new int[]{84});
    rules[862] = new Rule(-150, new int[]{83});
    rules[863] = new Rule(-150, new int[]{82});
    rules[864] = new Rule(-150, new int[]{81});
    rules[865] = new Rule(-293, new int[]{-150});
    rules[866] = new Rule(-293, new int[]{68});
    rules[867] = new Rule(-293, new int[]{64});
    rules[868] = new Rule(-293, new int[]{128});
    rules[869] = new Rule(-293, new int[]{22});
    rules[870] = new Rule(-293, new int[]{21});
    rules[871] = new Rule(-293, new int[]{63});
    rules[872] = new Rule(-293, new int[]{23});
    rules[873] = new Rule(-293, new int[]{129});
    rules[874] = new Rule(-293, new int[]{130});
    rules[875] = new Rule(-293, new int[]{131});
    rules[876] = new Rule(-293, new int[]{132});
    rules[877] = new Rule(-293, new int[]{133});
    rules[878] = new Rule(-293, new int[]{134});
    rules[879] = new Rule(-293, new int[]{135});
    rules[880] = new Rule(-293, new int[]{136});
    rules[881] = new Rule(-293, new int[]{137});
    rules[882] = new Rule(-293, new int[]{138});
    rules[883] = new Rule(-293, new int[]{24});
    rules[884] = new Rule(-293, new int[]{73});
    rules[885] = new Rule(-293, new int[]{91});
    rules[886] = new Rule(-293, new int[]{25});
    rules[887] = new Rule(-293, new int[]{26});
    rules[888] = new Rule(-293, new int[]{29});
    rules[889] = new Rule(-293, new int[]{30});
    rules[890] = new Rule(-293, new int[]{31});
    rules[891] = new Rule(-293, new int[]{71});
    rules[892] = new Rule(-293, new int[]{99});
    rules[893] = new Rule(-293, new int[]{32});
    rules[894] = new Rule(-293, new int[]{92});
    rules[895] = new Rule(-293, new int[]{33});
    rules[896] = new Rule(-293, new int[]{34});
    rules[897] = new Rule(-293, new int[]{27});
    rules[898] = new Rule(-293, new int[]{104});
    rules[899] = new Rule(-293, new int[]{101});
    rules[900] = new Rule(-293, new int[]{35});
    rules[901] = new Rule(-293, new int[]{36});
    rules[902] = new Rule(-293, new int[]{37});
    rules[903] = new Rule(-293, new int[]{40});
    rules[904] = new Rule(-293, new int[]{41});
    rules[905] = new Rule(-293, new int[]{42});
    rules[906] = new Rule(-293, new int[]{103});
    rules[907] = new Rule(-293, new int[]{43});
    rules[908] = new Rule(-293, new int[]{44});
    rules[909] = new Rule(-293, new int[]{46});
    rules[910] = new Rule(-293, new int[]{47});
    rules[911] = new Rule(-293, new int[]{48});
    rules[912] = new Rule(-293, new int[]{97});
    rules[913] = new Rule(-293, new int[]{49});
    rules[914] = new Rule(-293, new int[]{102});
    rules[915] = new Rule(-293, new int[]{50});
    rules[916] = new Rule(-293, new int[]{28});
    rules[917] = new Rule(-293, new int[]{51});
    rules[918] = new Rule(-293, new int[]{70});
    rules[919] = new Rule(-293, new int[]{98});
    rules[920] = new Rule(-293, new int[]{52});
    rules[921] = new Rule(-293, new int[]{53});
    rules[922] = new Rule(-293, new int[]{54});
    rules[923] = new Rule(-293, new int[]{55});
    rules[924] = new Rule(-293, new int[]{56});
    rules[925] = new Rule(-293, new int[]{57});
    rules[926] = new Rule(-293, new int[]{58});
    rules[927] = new Rule(-293, new int[]{59});
    rules[928] = new Rule(-293, new int[]{61});
    rules[929] = new Rule(-293, new int[]{105});
    rules[930] = new Rule(-293, new int[]{106});
    rules[931] = new Rule(-293, new int[]{109});
    rules[932] = new Rule(-293, new int[]{107});
    rules[933] = new Rule(-293, new int[]{108});
    rules[934] = new Rule(-293, new int[]{62});
    rules[935] = new Rule(-293, new int[]{74});
    rules[936] = new Rule(-293, new int[]{38});
    rules[937] = new Rule(-293, new int[]{39});
    rules[938] = new Rule(-293, new int[]{69});
    rules[939] = new Rule(-293, new int[]{148});
    rules[940] = new Rule(-293, new int[]{60});
    rules[941] = new Rule(-293, new int[]{139});
    rules[942] = new Rule(-293, new int[]{140});
    rules[943] = new Rule(-293, new int[]{79});
    rules[944] = new Rule(-293, new int[]{153});
    rules[945] = new Rule(-293, new int[]{152});
    rules[946] = new Rule(-293, new int[]{72});
    rules[947] = new Rule(-293, new int[]{154});
    rules[948] = new Rule(-293, new int[]{150});
    rules[949] = new Rule(-293, new int[]{151});
    rules[950] = new Rule(-293, new int[]{149});
    rules[951] = new Rule(-294, new int[]{45});
    rules[952] = new Rule(-200, new int[]{115});
    rules[953] = new Rule(-200, new int[]{116});
    rules[954] = new Rule(-200, new int[]{117});
    rules[955] = new Rule(-200, new int[]{118});
    rules[956] = new Rule(-200, new int[]{120});
    rules[957] = new Rule(-200, new int[]{121});
    rules[958] = new Rule(-200, new int[]{122});
    rules[959] = new Rule(-200, new int[]{123});
    rules[960] = new Rule(-200, new int[]{124});
    rules[961] = new Rule(-200, new int[]{125});
    rules[962] = new Rule(-200, new int[]{128});
    rules[963] = new Rule(-200, new int[]{129});
    rules[964] = new Rule(-200, new int[]{130});
    rules[965] = new Rule(-200, new int[]{131});
    rules[966] = new Rule(-200, new int[]{132});
    rules[967] = new Rule(-200, new int[]{133});
    rules[968] = new Rule(-200, new int[]{134});
    rules[969] = new Rule(-200, new int[]{135});
    rules[970] = new Rule(-200, new int[]{137});
    rules[971] = new Rule(-200, new int[]{139});
    rules[972] = new Rule(-200, new int[]{140});
    rules[973] = new Rule(-200, new int[]{-194});
    rules[974] = new Rule(-200, new int[]{119});
    rules[975] = new Rule(-194, new int[]{110});
    rules[976] = new Rule(-194, new int[]{111});
    rules[977] = new Rule(-194, new int[]{112});
    rules[978] = new Rule(-194, new int[]{113});
    rules[979] = new Rule(-194, new int[]{114});
    rules[980] = new Rule(-100, new int[]{18,-24,100,-23,9});
    rules[981] = new Rule(-23, new int[]{-100});
    rules[982] = new Rule(-23, new int[]{-147});
    rules[983] = new Rule(-24, new int[]{-23});
    rules[984] = new Rule(-24, new int[]{-24,100,-23});
    rules[985] = new Rule(-102, new int[]{-101});
    rules[986] = new Rule(-102, new int[]{-100});
    rules[987] = new Rule(-79, new int[]{-102});
    rules[988] = new Rule(-79, new int[]{-79,100,-102});
    rules[989] = new Rule(-323, new int[]{-147,127,-329});
    rules[990] = new Rule(-323, new int[]{8,9,-326,127,-329});
    rules[991] = new Rule(-323, new int[]{8,-147,5,-276,9,-326,127,-329});
    rules[992] = new Rule(-323, new int[]{8,-147,10,-327,9,-326,127,-329});
    rules[993] = new Rule(-323, new int[]{8,-147,5,-276,10,-327,9,-326,127,-329});
    rules[994] = new Rule(-323, new int[]{8,-102,100,-79,-325,-332,9,-336});
    rules[995] = new Rule(-323, new int[]{-100,-336});
    rules[996] = new Rule(-323, new int[]{-324});
    rules[997] = new Rule(-332, new int[]{});
    rules[998] = new Rule(-332, new int[]{10,-327});
    rules[999] = new Rule(-336, new int[]{-326,127,-329});
    rules[1000] = new Rule(-324, new int[]{37,-326,127,-329});
    rules[1001] = new Rule(-324, new int[]{37,8,9,-326,127,-329});
    rules[1002] = new Rule(-324, new int[]{37,8,-327,9,-326,127,-329});
    rules[1003] = new Rule(-324, new int[]{44,127,-330});
    rules[1004] = new Rule(-324, new int[]{44,8,9,127,-330});
    rules[1005] = new Rule(-324, new int[]{44,8,-327,9,127,-330});
    rules[1006] = new Rule(-327, new int[]{-328});
    rules[1007] = new Rule(-327, new int[]{-327,10,-328});
    rules[1008] = new Rule(-328, new int[]{-158,-325});
    rules[1009] = new Rule(-325, new int[]{});
    rules[1010] = new Rule(-325, new int[]{5,-276});
    rules[1011] = new Rule(-326, new int[]{});
    rules[1012] = new Rule(-326, new int[]{5,-278});
    rules[1013] = new Rule(-331, new int[]{-256});
    rules[1014] = new Rule(-331, new int[]{-153});
    rules[1015] = new Rule(-331, new int[]{-319});
    rules[1016] = new Rule(-331, new int[]{-248});
    rules[1017] = new Rule(-331, new int[]{-124});
    rules[1018] = new Rule(-331, new int[]{-123});
    rules[1019] = new Rule(-331, new int[]{-125});
    rules[1020] = new Rule(-331, new int[]{-35});
    rules[1021] = new Rule(-331, new int[]{-302});
    rules[1022] = new Rule(-331, new int[]{-169});
    rules[1023] = new Rule(-331, new int[]{-249});
    rules[1024] = new Rule(-331, new int[]{-126});
    rules[1025] = new Rule(-331, new int[]{8,-4,9});
    rules[1026] = new Rule(-329, new int[]{-104});
    rules[1027] = new Rule(-329, new int[]{-331});
    rules[1028] = new Rule(-330, new int[]{-212});
    rules[1029] = new Rule(-330, new int[]{-4});
    rules[1030] = new Rule(-330, new int[]{-331});
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
    {
  CurrentSemanticValue = new Union();
  CurrentSemanticValue = new Union();
    switch (action)
    {
      case 2: // parse_goal -> program_file
#line 195 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ root = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 3: // parse_goal -> unit_file
#line 197 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ root = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 4: // parse_goal -> parts
#line 199 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ root = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 5: // parse_goal -> tkShortProgram, uses_clause_one_or_empty, 
              //               decl_sect_list_proc_func_only, stmt_list
#line 201 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 202 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var stl = ValueStack[ValueStack.Depth-1].stn as statement_list;
#line 203 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			stl.left_logical_bracket = new token_info("");
#line 204 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			stl.right_logical_bracket = new token_info("");
#line 205 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var ul = ValueStack[ValueStack.Depth-3].stn as uses_list;
#line 206 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			root = CurrentSemanticValue.stn = NewProgramModule(null, null, ul, new block(ValueStack[ValueStack.Depth-2].stn as declarations, stl, LocationStack[LocationStack.Depth-1]), new token_info(""), LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1])); 
#line 207 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 6: // parse_goal -> tkShortSFProgram, uses_clause_one_or_empty, 
              //               decl_sect_list_proc_func_only, stmt_list
#line 209 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 210 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var stl = ValueStack[ValueStack.Depth-1].stn as statement_list;
#line 211 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			stl.left_logical_bracket = new token_info("");
#line 212 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			stl.right_logical_bracket = new token_info("");
#line 213 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var un = new unit_or_namespace(new ident_list("SF"),null);
#line 214 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var ul = ValueStack[ValueStack.Depth-3].stn as uses_list;
#line 215 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (ul == null)
#line 216 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			//var un1 = new unit_or_namespace(new ident_list("School"),null);
				ul = new uses_list(un,null);
#line 217 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else ul.Insert(0,un);
#line 218 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			//ul.Add(un1);
			root = CurrentSemanticValue.stn = NewProgramModule(null, null, ul, new block(ValueStack[ValueStack.Depth-2].stn as declarations, stl, CurrentLocationSpan), new token_info(""), LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1])); 
#line 219 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 7: // parts -> tkParseModeExpression, expr
#line 226 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 8: // parts -> tkParseModeExpression, tkType, type_decl_identifier
#line 228 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 9: // parts -> tkParseModeType, variable_as_type
#line 230 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 10: // parts -> tkParseModeStatement, stmt_or_expression
#line 232 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 11: // stmt_or_expression -> expr
#line 238 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = new expression_as_statement(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);}
        break;
      case 12: // stmt_or_expression -> assignment
#line 240 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 13: // stmt_or_expression -> var_stmt
#line 242 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 14: // optional_head_compiler_directives -> /* empty */
#line 247 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = null; }
        break;
      case 15: // optional_head_compiler_directives -> head_compiler_directives
#line 249 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = null; }
        break;
      case 16: // head_compiler_directives -> one_compiler_directive
#line 254 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = null; }
        break;
      case 17: // head_compiler_directives -> head_compiler_directives, one_compiler_directive
#line 256 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = null; }
        break;
      case 18: // one_compiler_directive -> tkDirectiveName, tkIdentifier
#line 261 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 262 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			parserTools.AddErrorFromResource("UNSUPPORTED_OLD_DIRECTIVES",CurrentLocationSpan);
#line 263 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = null;
#line 264 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 19: // one_compiler_directive -> tkDirectiveName, tkStringLiteral
#line 266 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 267 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			parserTools.AddErrorFromResource("UNSUPPORTED_OLD_DIRECTIVES",CurrentLocationSpan);
#line 268 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = null;
#line 269 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 20: // program_file -> program_header, optional_head_compiler_directives, uses_clause, 
               //                 program_block, optional_tk_point
#line 274 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 275 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = NewProgramModule(ValueStack[ValueStack.Depth-5].stn as program_name, ValueStack[ValueStack.Depth-4].ob, ValueStack[ValueStack.Depth-3].stn as uses_list, ValueStack[ValueStack.Depth-2].stn, ValueStack[ValueStack.Depth-1].ob, CurrentLocationSpan);
#line 276 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 21: // optional_tk_point -> tkPoint
#line 282 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 22: // optional_tk_point -> tkSemiColon
#line 284 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = null; }
        break;
      case 23: // optional_tk_point -> tkColon
#line 286 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = null; }
        break;
      case 24: // optional_tk_point -> tkComma
#line 288 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = null; }
        break;
      case 25: // optional_tk_point -> tkDotDot
#line 290 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = null; }
        break;
      case 27: // program_header -> /* empty */
#line 296 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 28: // program_header -> tkProgram, identifier, program_heading_2
#line 298 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = new program_name(ValueStack[ValueStack.Depth-2].id,CurrentLocationSpan); }
        break;
      case 29: // program_heading_2 -> tkSemiColon
#line 303 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = null; }
        break;
      case 30: // program_heading_2 -> tkRoundOpen, program_param_list, tkRoundClose, tkSemiColon
#line 305 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = null; }
        break;
      case 31: // program_param_list -> program_param
#line 310 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = null; }
        break;
      case 32: // program_param_list -> program_param_list, tkComma, program_param
#line 312 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = null; }
        break;
      case 33: // program_param -> identifier
#line 317 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 34: // program_block -> program_decl_sect_list, compound_stmt
#line 322 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 323 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-2].stn as declarations, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
#line 324 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 35: // program_decl_sect_list -> decl_sect_list
#line 329 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 36: // ident_or_keyword_pointseparator_list -> identifier_or_keyword
#line 334 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 335 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
#line 336 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 37: // ident_or_keyword_pointseparator_list -> ident_or_keyword_pointseparator_list, 
               //                                         tkPoint, identifier_or_keyword
#line 338 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 339 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
#line 340 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 38: // uses_clause_one -> tkUses, used_units_list, tkSemiColon
#line 346 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 347 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
#line 348 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 349 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 39: // uses_clause_one_or_empty -> /* empty */
#line 354 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 355 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = null; 
#line 356 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 40: // uses_clause_one_or_empty -> uses_clause_one
#line 358 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 359 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (parserTools.buildTreeForFormatter)
#line 360 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = new uses_closure(ValueStack[ValueStack.Depth-1].stn as uses_list,CurrentLocationSpan);
#line 361 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 362 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 41: // uses_clause -> /* empty */
#line 367 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 368 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = null; 
#line 369 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 42: // uses_clause -> uses_clause, uses_clause_one
#line 371 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 372 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   			if (parserTools.buildTreeForFormatter)
#line 373 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   			{
#line 374 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	        	if (ValueStack[ValueStack.Depth-2].stn == null)
#line 375 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                {
#line 376 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	        		CurrentSemanticValue.stn = new uses_closure(ValueStack[ValueStack.Depth-1].stn as uses_list,CurrentLocationSpan);
#line 377 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                }
#line 378 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	        	else {
#line 379 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    (ValueStack[ValueStack.Depth-2].stn as uses_closure).AddUsesList(ValueStack[ValueStack.Depth-1].stn as uses_list,CurrentLocationSpan);
#line 380 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
#line 381 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                }
#line 382 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   			}
#line 383 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   			else 
#line 384 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   			{
#line 385 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	        	if (ValueStack[ValueStack.Depth-2].stn == null)
#line 386 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                {
#line 387 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 388 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 389 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                }
#line 390 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	        	else 
#line 391 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                {
#line 392 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    (ValueStack[ValueStack.Depth-2].stn as uses_list).AddUsesList(ValueStack[ValueStack.Depth-1].stn as uses_list,CurrentLocationSpan);
#line 393 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
#line 394 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 395 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                }
#line 396 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}
#line 397 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 43: // used_units_list -> used_unit_name
#line 402 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 403 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		  CurrentSemanticValue.stn = new uses_list(ValueStack[ValueStack.Depth-1].stn as unit_or_namespace,CurrentLocationSpan);
#line 404 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 44: // used_units_list -> used_units_list, tkComma, used_unit_name
#line 406 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 407 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		  CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as uses_list).Add(ValueStack[ValueStack.Depth-1].stn as unit_or_namespace, CurrentLocationSpan);
#line 408 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 45: // used_unit_name -> ident_or_keyword_pointseparator_list
#line 413 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 414 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new unit_or_namespace(ValueStack[ValueStack.Depth-1].stn as ident_list,CurrentLocationSpan); 
#line 415 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 46: // used_unit_name -> ident_or_keyword_pointseparator_list, tkIn, tkStringLiteral
#line 417 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 418 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	if (ValueStack[ValueStack.Depth-1].stn is char_const _cc)
#line 419 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		ValueStack[ValueStack.Depth-1].stn = new string_const(_cc.cconst.ToString());
#line 420 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new uses_unit_in(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].stn as string_const, CurrentLocationSpan);
#line 421 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 47: // unit_file -> unit_header, interface_part, implementation_part, 
               //              initialization_part, tkPoint
#line 428 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 429 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new unit_module(ValueStack[ValueStack.Depth-5].stn as unit_name, ValueStack[ValueStack.Depth-4].stn as interface_node, ValueStack[ValueStack.Depth-3].stn as implementation_node, 
#line 430 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			  (ValueStack[ValueStack.Depth-2].stn as initfinal_part).initialization_sect, (ValueStack[ValueStack.Depth-2].stn as initfinal_part).finalization_sect, /*$1 as attribute_list*/ null, CurrentLocationSpan);   
#line 431 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.stn as compilation_unit).Language = PascalABCCompiler.StringConstants.pascalLanguageName;                
#line 432 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 48: // unit_file -> unit_header, abc_interface_part, initialization_part, tkPoint
#line 436 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 437 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new unit_module(ValueStack[ValueStack.Depth-4].stn as unit_name, ValueStack[ValueStack.Depth-3].stn as interface_node, null, 
#line 438 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			  (ValueStack[ValueStack.Depth-2].stn as initfinal_part).initialization_sect, (ValueStack[ValueStack.Depth-2].stn as initfinal_part).finalization_sect, /*$1 as attribute_list*/ null, CurrentLocationSpan);
#line 439 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.stn as compilation_unit).Language = PascalABCCompiler.StringConstants.pascalLanguageName;   
#line 440 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 49: // unit_header -> unit_key_word, unit_name, tkSemiColon, 
               //                optional_head_compiler_directives
#line 445 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 446 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = NewUnitHeading(new ident(ValueStack[ValueStack.Depth-4].ti.text, LocationStack[LocationStack.Depth-4]), ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan); 
#line 447 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 50: // unit_header -> tkNamespace, ident_or_keyword_pointseparator_list, tkSemiColon, 
               //                optional_head_compiler_directives
#line 449 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 450 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = NewNamespaceHeading(new ident(ValueStack[ValueStack.Depth-4].ti.text, LocationStack[LocationStack.Depth-4]), ValueStack[ValueStack.Depth-3].stn as ident_list, CurrentLocationSpan);
#line 451 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 51: // unit_key_word -> tkUnit
#line 456 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 52: // unit_key_word -> tkLibrary
#line 458 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 53: // unit_name -> identifier
#line 463 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 54: // interface_part -> tkInterface, uses_clause, interface_decl_sect_list
#line 468 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 469 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new interface_node(ValueStack[ValueStack.Depth-1].stn as declarations, ValueStack[ValueStack.Depth-2].stn as uses_list, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1])); 
#line 470 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 55: // implementation_part -> tkImplementation, uses_clause, decl_sect_list
#line 475 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 476 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new implementation_node(ValueStack[ValueStack.Depth-2].stn as uses_list, ValueStack[ValueStack.Depth-1].stn as declarations, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1])); 
#line 477 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 56: // abc_interface_part -> uses_clause, decl_sect_list
#line 482 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 483 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new interface_node(ValueStack[ValueStack.Depth-1].stn as declarations, ValueStack[ValueStack.Depth-2].stn as uses_list, null, null); 
#line 484 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 57: // initialization_part -> tkEnd
#line 489 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 490 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new initfinal_part(); 
#line 491 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 492 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 58: // initialization_part -> tkInitialization, stmt_list, tkEnd
#line 494 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 495 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, null, null, CurrentLocationSpan);
#line 496 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 59: // initialization_part -> tkInitialization, stmt_list, tkFinalization, stmt_list, 
               //                        tkEnd
#line 498 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 499 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-5].ti, ValueStack[ValueStack.Depth-4].stn as statement_list, ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, CurrentLocationSpan);
#line 500 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 60: // initialization_part -> tkBegin, stmt_list, tkEnd
#line 502 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 503 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, null, null, CurrentLocationSpan);
#line 504 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 61: // interface_decl_sect_list -> int_decl_sect_list1
#line 509 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 510 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
#line 511 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
#line 512 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else 
#line 513 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 514 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 62: // int_decl_sect_list1 -> /* empty */
#line 519 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 520 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new declarations();  
#line 521 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (GlobalDecls==null) 
#line 522 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				GlobalDecls = CurrentSemanticValue.stn as declarations;
#line 523 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 63: // int_decl_sect_list1 -> int_decl_sect_list1, int_decl_sect
#line 525 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 526 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
#line 527 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 64: // decl_sect_list -> decl_sect_list1
#line 532 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 533 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
#line 534 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
#line 535 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else 
#line 536 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 537 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 65: // decl_sect_list_proc_func_only -> /* empty */
#line 542 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 543 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new declarations(); 
#line 544 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (GlobalDecls==null) 
#line 545 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				GlobalDecls = CurrentSemanticValue.stn as declarations;
#line 546 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 66: // decl_sect_list_proc_func_only -> decl_sect_list_proc_func_only, 
               //                                  attribute_declarations, 
               //                                  proc_func_decl_noclass
#line 548 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 549 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var dcl = ValueStack[ValueStack.Depth-3].stn as declarations;
#line 550 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).AssignAttrList(ValueStack[ValueStack.Depth-2].stn as attribute_list);
#line 551 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (dcl.Count == 0)			
#line 552 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = dcl.Add(ValueStack[ValueStack.Depth-1].stn as declaration, LocationStack[LocationStack.Depth-1]);
#line 553 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else
#line 554 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			{
#line 555 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var sc = dcl.source_context;
#line 556 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				sc = sc.Merge(ValueStack[ValueStack.Depth-1].stn.source_context);
#line 557 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = dcl.Add(ValueStack[ValueStack.Depth-1].stn as declaration, LocationStack[LocationStack.Depth-1]);
#line 558 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn.source_context = sc;			
#line 559 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}
#line 560 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 67: // decl_sect_list1 -> /* empty */
#line 565 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 566 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new declarations(); 
#line 567 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (GlobalDecls==null) 
#line 568 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				GlobalDecls = CurrentSemanticValue.stn as declarations;
#line 569 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 68: // decl_sect_list1 -> decl_sect_list1, decl_sect
#line 571 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 572 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
#line 573 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 69: // inclass_decl_sect_list -> inclass_decl_sect_list1
#line 578 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 579 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
#line 580 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
#line 581 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else 
#line 582 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 583 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 70: // inclass_decl_sect_list1 -> /* empty */
#line 588 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 589 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	CurrentSemanticValue.stn = new declarations(); 
#line 590 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 71: // inclass_decl_sect_list1 -> inclass_decl_sect_list1, abc_decl_sect
#line 592 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 593 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
#line 594 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 72: // int_decl_sect -> const_decl_sect
#line 599 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 73: // int_decl_sect -> res_str_decl_sect
#line 601 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 74: // int_decl_sect -> type_decl_sect
#line 603 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 75: // int_decl_sect -> var_decl_sect
#line 605 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 76: // int_decl_sect -> int_proc_header
#line 607 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 77: // int_decl_sect -> int_func_header
#line 609 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 78: // decl_sect -> label_decl_sect
#line 614 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 79: // decl_sect -> const_decl_sect
#line 616 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 80: // decl_sect -> res_str_decl_sect
#line 618 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 81: // decl_sect -> type_decl_sect
#line 620 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 82: // decl_sect -> var_decl_sect
#line 622 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 83: // decl_sect -> proc_func_constr_destr_decl_with_attr
#line 624 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 84: // proc_func_constr_destr_decl -> proc_func_decl
#line 630 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 85: // proc_func_constr_destr_decl -> constr_destr_decl
#line 632 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 86: // proc_func_constr_destr_decl_with_attr -> attribute_declarations, 
               //                                          proc_func_constr_destr_decl
#line 637 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 638 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    (ValueStack[ValueStack.Depth-1].stn as procedure_definition).AssignAttrList(ValueStack[ValueStack.Depth-2].stn as attribute_list);
#line 639 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 640 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 87: // abc_decl_sect -> label_decl_sect
#line 645 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 88: // abc_decl_sect -> const_decl_sect
#line 647 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 89: // abc_decl_sect -> res_str_decl_sect
#line 649 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 90: // abc_decl_sect -> type_decl_sect
#line 651 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 91: // abc_decl_sect -> var_decl_sect
#line 653 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 92: // int_proc_header -> attribute_declarations, proc_header
#line 658 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 659 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
#line 660 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.td as procedure_header).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
#line 661 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 93: // int_proc_header -> attribute_declarations, proc_header, tkForward, tkSemiColon
#line 663 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 664 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = NewProcedureHeader(ValueStack[ValueStack.Depth-4].stn as attribute_list, ValueStack[ValueStack.Depth-3].td as procedure_header, ValueStack[ValueStack.Depth-2].id as procedure_attribute, CurrentLocationSpan);
#line 665 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 94: // int_func_header -> attribute_declarations, func_header
#line 670 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 671 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
#line 672 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.td as procedure_header).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
#line 673 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 95: // int_func_header -> attribute_declarations, func_header, tkForward, tkSemiColon
#line 675 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 676 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = NewProcedureHeader(ValueStack[ValueStack.Depth-4].stn as attribute_list, ValueStack[ValueStack.Depth-3].td as procedure_header, ValueStack[ValueStack.Depth-2].id as procedure_attribute, CurrentLocationSpan);
#line 677 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 96: // label_decl_sect -> tkLabel, label_list, tkSemiColon
#line 682 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 683 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new label_definitions(ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
#line 684 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 97: // label_list -> label_name
#line 689 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 690 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
#line 691 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 98: // label_list -> label_list, tkComma, label_name
#line 693 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 694 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
#line 695 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 99: // label_name -> tkInteger
#line 700 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 701 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ex.ToString(), CurrentLocationSpan);
#line 702 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 100: // label_name -> identifier
#line 704 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 705 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; 
#line 706 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 101: // const_decl_sect -> tkConst, const_decl
#line 711 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 712 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new consts_definitions_list(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
#line 713 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 102: // const_decl_sect -> const_decl_sect, const_decl
#line 715 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 716 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as consts_definitions_list).Add(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
#line 717 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 103: // res_str_decl_sect -> tkResourceString, const_decl
#line 722 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 723 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
#line 724 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 104: // res_str_decl_sect -> res_str_decl_sect, const_decl
#line 726 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 727 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
#line 728 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 105: // type_decl_sect -> tkType, type_decl
#line 733 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 734 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new type_declarations(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
#line 735 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 106: // type_decl_sect -> type_decl_sect, type_decl
#line 737 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 738 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as type_declarations).Add(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
#line 739 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 107: // var_decl_with_assign_var_tuple -> var_decl
#line 744 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 745 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
#line 746 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 108: // var_decl_with_assign_var_tuple -> tkRoundOpen, identifier, tkComma, ident_list, 
                //                                   tkRoundClose, tkAssign, expr_l1, 
                //                                   tkSemiColon
#line 748 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 749 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-5].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-7].id);
#line 750 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			ValueStack[ValueStack.Depth-5].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]);
#line 751 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new var_tuple_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
#line 752 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 109: // var_decl_sect -> tkVar, var_decl_with_assign_var_tuple
#line 757 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 758 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
#line 759 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 110: // var_decl_sect -> tkEvent, var_decl_with_assign_var_tuple
#line 761 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 762 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);                        
#line 763 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).is_event = true;
#line 764 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 111: // var_decl_sect -> var_decl_sect, var_decl_with_assign_var_tuple
#line 766 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 767 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as variable_definitions).Add(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
#line 768 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 112: // const_decl -> only_const_decl, tkSemiColon
#line 779 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 113: // only_const_decl -> const_name, tkEqual, init_const_expr
#line 784 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 785 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new simple_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 786 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 114: // only_const_decl -> const_name, tkColon, type_ref, tkEqual, typed_const
#line 788 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 789 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new typed_const_definition(ValueStack[ValueStack.Depth-5].id, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-3].td, CurrentLocationSpan);
#line 790 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 115: // init_const_expr -> const_expr
#line 795 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 116: // init_const_expr -> array_const
#line 797 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 117: // const_name -> identifier
#line 802 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 118: // const_relop_expr -> const_simple_expr
#line 818 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 819 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
#line 820 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 119: // const_relop_expr -> const_relop_expr, const_relop, const_simple_expr
#line 822 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 823 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
#line 824 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 120: // const_expr -> const_relop_expr
#line 829 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 830 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
#line 831 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 121: // const_expr -> question_constexpr
#line 833 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 834 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
#line 835 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 122: // const_expr -> const_expr, tkDoubleQuestion, const_relop_expr
#line 837 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 123: // question_constexpr -> const_expr, tkQuestion, const_expr, tkColon, const_expr
#line 842 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 124: // const_relop -> tkEqual
#line 847 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 125: // const_relop -> tkNotEqual
#line 849 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 126: // const_relop -> tkLower
#line 851 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 127: // const_relop -> tkGreater
#line 853 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 128: // const_relop -> tkLowerEqual
#line 855 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 129: // const_relop -> tkGreaterEqual
#line 857 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 130: // const_relop -> tkIn
#line 859 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 131: // const_simple_expr -> const_term
#line 864 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 132: // const_simple_expr -> const_simple_expr, const_addop, const_term
#line 866 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 133: // const_addop -> tkPlus
#line 871 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 134: // const_addop -> tkMinus
#line 873 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 135: // const_addop -> tkOr
#line 875 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 136: // const_addop -> tkXor
#line 877 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 137: // as_is_constexpr -> const_term, typecast_op, simple_or_template_type_reference
#line 882 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 883 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = NewAsIsConstexpr(ValueStack[ValueStack.Depth-3].ex, (op_typecast)ValueStack[ValueStack.Depth-2].ob, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);                                
#line 884 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 138: // power_constexpr -> const_factor_without_unary_op, tkStarStar, const_factor
#line 889 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 139: // power_constexpr -> const_factor_without_unary_op, tkStarStar, power_constexpr
#line 891 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 140: // power_constexpr -> sign, power_constexpr
#line 893 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 141: // const_term -> const_factor
#line 898 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 142: // const_term -> as_is_constexpr
#line 900 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 143: // const_term -> power_constexpr
#line 902 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 144: // const_term -> const_term, const_mulop, const_factor
#line 904 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 145: // const_term -> const_term, const_mulop, power_constexpr
#line 906 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 146: // const_mulop -> tkStar
#line 911 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 147: // const_mulop -> tkSlash
#line 913 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 148: // const_mulop -> tkDiv
#line 915 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 149: // const_mulop -> tkMod
#line 917 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 150: // const_mulop -> tkShl
#line 919 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 151: // const_mulop -> tkShr
#line 921 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 152: // const_mulop -> tkAnd
#line 923 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 153: // const_factor_without_unary_op -> const_variable
#line 928 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 154: // const_factor_without_unary_op -> tkRoundOpen, const_expr, tkRoundClose
#line 930 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 155: // const_factor -> const_variable
#line 935 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 156: // const_factor -> const_set
#line 937 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 157: // const_factor -> tkNil
#line 939 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 940 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new nil_const();  
#line 941 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
#line 942 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 158: // const_factor -> tkAddressOf, const_factor
#line 944 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 945 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new get_address(ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);  
#line 946 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 159: // const_factor -> tkRoundOpen, const_expr, tkRoundClose
#line 948 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 949 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	 	    CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
#line 950 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 160: // const_factor -> tkNot, const_factor
#line 952 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 953 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
#line 954 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 161: // const_factor -> sign, const_factor
#line 956 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 957 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    // �?�?�?наЯ ко�?�?ек�?иЯ �?ел�?�? конс�?ан�?
			if (ValueStack[ValueStack.Depth-2].op.type == Operators.Minus)
#line 958 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			{
#line 959 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			    var i64 = ValueStack[ValueStack.Depth-1].ex as int64_const;
#line 960 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (i64 != null && i64.val == (Int64)Int32.MaxValue + 1)
#line 961 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				{
#line 962 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					CurrentSemanticValue.ex = new int32_const(Int32.MinValue,CurrentLocationSpan);
#line 963 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					break;
#line 964 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				}
#line 965 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var ui64 = ValueStack[ValueStack.Depth-1].ex as uint64_const;
#line 966 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (ui64 != null && ui64.val == (UInt64)Int64.MaxValue + 1)
#line 967 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				{
#line 968 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					CurrentSemanticValue.ex = new int64_const(Int64.MinValue,CurrentLocationSpan);
#line 969 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					break;
#line 970 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				}
#line 971 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (ui64 != null && ui64.val > (UInt64)Int64.MaxValue + 1)
#line 972 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				{
#line 973 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					parserTools.AddErrorFromResource("BAD_INT2",CurrentLocationSpan);
#line 974 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					break;
#line 975 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				}
#line 976 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			    // можно сдела�?�? в�?�?исление конс�?ан�?�? с вмон�?и�?ованн�?м мин�?сом
			}
#line 977 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
#line 978 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 162: // const_factor -> new_expr
#line 982 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 163: // const_factor -> default_expr
#line 984 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 164: // pascal_set_const -> tkSquareOpen, elem_list, tkSquareClose
#line 993 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 994 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            // �?сли elem_list п�?с�? или соде�?жи�? диапазон, �?о э�?о множес�?во, ина�?е массив. С PascalABC.NET 3.10  
            /*var is_set = false;
            var el = $2 as expression_list;
            if (el == null || el.Count == 0)
              is_set = true;
            else if (el.expressions.Count(x => x is diapason_expr_new) > 0)
                is_set = true;
            if (is_set)*/    
#line 995 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.ex = new pascal_set_constant(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
#line 996 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			//else $$ = new array_const_new($2 as expression_list, @$); 				
		}
        break;
      case 165: // const_set -> pascal_set_const
#line 1009 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1010 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;				
#line 1011 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 166: // const_set -> tkVertParen, elem_list, tkVertParen
#line 1013 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1014 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new array_const_new(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
#line 1015 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 167: // sign -> tkPlus
#line 1020 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 168: // sign -> tkMinus
#line 1022 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 169: // const_variable -> identifier
#line 1027 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 170: // const_variable -> literal
#line 1029 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 171: // const_variable -> unsigned_number
#line 1031 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 172: // const_variable -> tkInherited, identifier
#line 1033 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1034 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
#line 1035 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 173: // const_variable -> sizeof_expr
#line 1037 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 174: // const_variable -> typeof_expr
#line 1039 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 175: // const_variable -> const_variable, const_variable_2
#line 1050 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1051 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = NewConstVariable(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 1052 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 176: // const_variable -> const_variable, tkAmpersend, template_type_params
#line 1054 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1055 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
#line 1056 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 177: // const_variable -> const_variable, tkSquareOpen, format_const_expr, 
                //                   tkSquareClose
#line 1058 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1059 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		var fe = ValueStack[ValueStack.Depth-2].ex as format_expr;
#line 1060 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (!parserTools.buildTreeForFormatter)
#line 1061 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            {
#line 1062 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                if (fe.expr == null)
#line 1063 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    fe.expr = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
#line 1064 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                if (fe.format1 == null)
#line 1065 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    fe.format1 = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
#line 1066 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            }
#line 1067 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		CurrentSemanticValue.ex = new slice_expr(ValueStack[ValueStack.Depth-4].ex as addressed_value,fe.expr,fe.format1,fe.format2,CurrentLocationSpan);
#line 1068 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 178: // const_variable_2 -> tkPoint, identifier_or_keyword
#line 1073 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1074 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new dot_node(null, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
#line 1075 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 179: // const_variable_2 -> tkDeref
#line 1077 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1078 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new roof_dereference();  
#line 1079 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
#line 1080 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 180: // const_variable_2 -> tkRoundOpen, optional_const_func_expr_list, tkRoundClose
#line 1082 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1083 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
#line 1084 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 181: // const_variable_2 -> tkSquareOpen, const_elem_list, tkSquareClose
#line 1086 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1087 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new indexer(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
#line 1088 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 182: // optional_const_func_expr_list -> expr_list
#line 1093 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 183: // optional_const_func_expr_list -> /* empty */
#line 1095 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 184: // const_elem_list -> const_elem_list1
#line 1111 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 186: // const_elem_list1 -> const_elem
#line 1117 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1118 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 1119 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 187: // const_elem_list1 -> const_elem_list1, tkComma, const_elem
#line 1121 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1122 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 1123 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 188: // const_elem -> const_expr
#line 1128 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 189: // const_elem -> const_expr, tkDotDot, const_expr
#line 1130 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1131 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 1132 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 190: // unsigned_number -> tkInteger
#line 1137 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 191: // unsigned_number -> tkHex
#line 1139 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 192: // unsigned_number -> tkFloat
#line 1141 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 193: // unsigned_number -> tkBigInteger
#line 1143 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 194: // typed_const -> const_expr
#line 1148 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 195: // typed_const -> array_const
#line 1150 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 196: // typed_const -> record_const
#line 1152 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 197: // array_const -> tkRoundOpen, typed_const_list, tkRoundClose
#line 1157 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1158 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new array_const(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan); 
#line 1159 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 199: // typed_const_list -> typed_const_list1
#line 1169 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1170 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 1171 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 200: // typed_const_list1 -> typed_const_plus
#line 1176 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1177 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 1178 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 201: // typed_const_list1 -> typed_const_list1, tkComma, typed_const_plus
#line 1180 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1181 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 1182 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 202: // record_const -> tkRoundOpen, const_field_list, tkRoundClose
#line 1187 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1188 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
#line 1189 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
#line 1190 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 203: // const_field_list -> const_field_list_1
#line 1195 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 204: // const_field_list -> const_field_list_1, tkSemiColon
#line 1197 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 205: // const_field_list_1 -> const_field
#line 1202 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1203 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new record_const(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
#line 1204 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 206: // const_field_list_1 -> const_field_list_1, tkSemiColon, const_field
#line 1206 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1207 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = (ValueStack[ValueStack.Depth-3].ex as record_const).Add(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
#line 1208 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 207: // const_field -> const_field_name, tkColon, typed_const
#line 1213 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1214 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new record_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 1215 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 208: // const_field_name -> identifier
#line 1220 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 209: // type_decl -> attribute_declarations, simple_type_decl
#line 1225 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 1226 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
#line 1227 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 1228 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = LocationStack[LocationStack.Depth-1];
#line 1229 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 210: // attribute_declarations -> attribute_declaration
#line 1234 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1235 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new attribute_list(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
#line 1236 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    }
        break;
      case 211: // attribute_declarations -> attribute_declarations, attribute_declaration
#line 1238 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1239 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
#line 1240 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 212: // attribute_declarations -> /* empty */
#line 1242 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 213: // attribute_declaration -> tkSquareOpen, one_or_some_attribute, tkSquareClose
#line 1247 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 214: // one_or_some_attribute -> one_attribute
#line 1252 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1253 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new simple_attribute_list(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
#line 1254 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 215: // one_or_some_attribute -> one_or_some_attribute, tkComma, one_attribute
#line 1256 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1257 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as simple_attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
#line 1258 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 216: // one_attribute -> attribute_variable
#line 1263 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 217: // one_attribute -> identifier, tkColon, attribute_variable
#line 1265 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 1266 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].stn as attribute).qualifier = ValueStack[ValueStack.Depth-3].id;
#line 1267 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 1268 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 1269 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 218: // simple_type_decl -> type_decl_identifier, tkEqual, type_decl_type, tkSemiColon
#line 1274 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1275 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-4].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
#line 1276 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 219: // simple_type_decl -> template_identifier_with_equal, type_decl_type, tkSemiColon
#line 1278 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1279 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
#line 1280 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 220: // type_decl_identifier -> identifier
#line 1285 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 221: // type_decl_identifier -> identifier, template_arguments
#line 1287 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1288 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
#line 1289 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 222: // template_identifier_with_equal -> identifier, tkLower, ident_list, 
                //                                   tkGreaterEqual
#line 1294 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1295 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-4].id.name, ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
#line 1296 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 223: // type_decl_type -> type_ref
#line 1301 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 224: // type_decl_type -> object_type
#line 1303 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 225: // simple_type_question -> simple_type, tkQuestion
#line 1308 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1309 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (parserTools.buildTreeForFormatter)
#line 1310 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   			{
#line 1311 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                CurrentSemanticValue.td = ValueStack[ValueStack.Depth-2].td;
#line 1312 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            }
#line 1313 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            else
#line 1314 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            {
#line 1315 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                var l = new List<ident>();
#line 1316 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                l.Add(new ident("System"));
#line 1317 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                l.Add(new ident("Nullable"));
#line 1318 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                CurrentSemanticValue.td = new template_type_reference(new named_type_reference(l), new template_param_list(ValueStack[ValueStack.Depth-2].td), CurrentLocationSpan);
#line 1319 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            }
#line 1320 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 226: // simple_type_question -> template_type, tkQuestion
#line 1322 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1323 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (parserTools.buildTreeForFormatter)
#line 1324 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   			{
#line 1325 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                CurrentSemanticValue.td = ValueStack[ValueStack.Depth-2].td;
#line 1326 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            }
#line 1327 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            else
#line 1328 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            {
#line 1329 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                var l = new List<ident>();
#line 1330 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                l.Add(new ident("System"));
#line 1331 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                l.Add(new ident("Nullable"));
#line 1332 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                CurrentSemanticValue.td = new template_type_reference(new named_type_reference(l), new template_param_list(ValueStack[ValueStack.Depth-2].td), CurrentLocationSpan);
#line 1333 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            }
#line 1334 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 227: // type_ref -> simple_type
#line 1339 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 228: // type_ref -> simple_type_question
#line 1341 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 229: // type_ref -> string_type
#line 1343 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 230: // type_ref -> pointer_type
#line 1345 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 231: // type_ref -> structured_type
#line 1347 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 232: // type_ref -> procedural_type
#line 1349 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 233: // type_ref -> template_type
#line 1351 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 234: // template_type -> simple_type_identifier, template_type_params
#line 1356 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1357 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new template_type_reference(ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan); 
#line 1358 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 235: // template_type_params -> tkLower, template_param_list, tkGreater
#line 1363 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1364 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
#line 1365 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 1366 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 236: // template_type_empty_params -> tkNotEqual
#line 1371 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1372 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            var ntr = new named_type_reference(new ident(""), CurrentLocationSpan);
#line 1373 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            
#line 1374 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new template_param_list(ntr, CurrentLocationSpan);
#line 1375 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            ntr.source_context = new SourceContext(CurrentSemanticValue.stn.source_context.end_position.line_num, CurrentSemanticValue.stn.source_context.end_position.column_num, CurrentSemanticValue.stn.source_context.begin_position.line_num, CurrentSemanticValue.stn.source_context.begin_position.column_num);
#line 1376 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 237: // template_type_empty_params -> tkLower, template_empty_param_list, tkGreater
#line 1378 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1379 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
#line 1380 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 1381 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 238: // template_param_list -> template_param
#line 1386 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1387 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
#line 1388 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 239: // template_param_list -> template_param_list, tkComma, template_param
#line 1390 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1391 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
#line 1392 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 240: // template_empty_param_list -> template_empty_param
#line 1397 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1398 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
#line 1399 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 241: // template_empty_param_list -> template_empty_param_list, tkComma, 
                //                              template_empty_param
#line 1401 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1402 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
#line 1403 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 242: // template_empty_param -> /* empty */
#line 1408 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1409 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.td = new named_type_reference(new ident(""), CurrentLocationSpan);
#line 1410 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 243: // template_param -> simple_type
#line 1415 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 244: // template_param -> simple_type, tkQuestion
#line 1417 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1418 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (parserTools.buildTreeForFormatter)
#line 1419 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   			{
#line 1420 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                CurrentSemanticValue.td = ValueStack[ValueStack.Depth-2].td;
#line 1421 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            }
#line 1422 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            else
#line 1423 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            {
#line 1424 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                var l = new List<ident>();
#line 1425 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                l.Add(new ident("System"));
#line 1426 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                l.Add(new ident("Nullable"));
#line 1427 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                CurrentSemanticValue.td = new template_type_reference(new named_type_reference(l), new template_param_list(ValueStack[ValueStack.Depth-2].td), CurrentLocationSpan);
#line 1428 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            }
#line 1429 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 245: // template_param -> structured_type
#line 1431 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 246: // template_param -> procedural_type
#line 1433 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 247: // template_param -> template_type
#line 1435 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 248: // simple_type -> range_expr
#line 1440 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1441 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	    	CurrentSemanticValue.td = parserTools.ConvertDotNodeOrIdentToNamedTypeReference(ValueStack[ValueStack.Depth-1].ex); 
#line 1442 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	    }
        break;
      case 249: // simple_type -> range_expr, tkDotDot, range_expr
#line 1444 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1445 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new diapason(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 1446 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 250: // simple_type -> tkRoundOpen, enumeration_id_list, tkRoundClose
#line 1448 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1449 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new enum_type_definition(ValueStack[ValueStack.Depth-2].stn as enumerator_list, CurrentLocationSpan);  
#line 1450 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 251: // range_expr -> range_term
#line 1455 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 252: // range_expr -> range_expr, const_addop, range_term
#line 1457 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1458 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
#line 1459 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 253: // range_term -> range_factor
#line 1464 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 254: // range_term -> range_term, const_mulop, range_factor
#line 1466 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1467 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
#line 1468 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 255: // range_factor -> simple_type_identifier
#line 1473 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1474 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = parserTools.ConvertNamedTypeReferenceToDotNodeOrIdent(ValueStack[ValueStack.Depth-1].td as named_type_reference);
#line 1475 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 256: // range_factor -> unsigned_number
#line 1477 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 257: // range_factor -> sign, range_factor
#line 1479 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1480 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
#line 1481 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 258: // range_factor -> literal
#line 1483 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 259: // range_factor -> range_factor, tkRoundOpen, const_elem_list, tkRoundClose
#line 1485 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1486 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value, ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
#line 1487 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 260: // simple_type_identifier -> identifier
#line 1492 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1493 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new named_type_reference(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
#line 1494 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 261: // simple_type_identifier -> simple_type_identifier, tkPoint, 
                //                           identifier_or_keyword
#line 1496 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1497 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = (ValueStack[ValueStack.Depth-3].td as named_type_reference).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
#line 1498 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 262: // enumeration_id_list -> enumeration_id
#line 1503 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1504 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new enumerator_list(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
#line 1505 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 263: // enumeration_id_list -> enumeration_id_list, tkComma, enumeration_id
#line 1507 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1508 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as enumerator_list).Add(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
#line 1509 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 264: // enumeration_id -> type_ref
#line 1514 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1515 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan); 
#line 1516 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 265: // enumeration_id -> type_ref, tkEqual, expr
#line 1518 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1519 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 1520 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 266: // pointer_type -> tkDeref, fptype
#line 1525 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1526 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new ref_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
#line 1527 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 267: // structured_type -> array_type
#line 1532 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 268: // structured_type -> record_type
#line 1534 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 269: // structured_type -> set_type
#line 1536 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 270: // structured_type -> file_type
#line 1538 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 271: // structured_type -> sequence_type
#line 1540 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 272: // sequence_type -> tkSequence, tkOf, type_ref
#line 1545 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1546 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new sequence_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
#line 1547 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 273: // array_type -> tkArray, tkSquareOpen, simple_type_list, tkSquareClose, tkOf, 
                //               type_ref
#line 1552 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1553 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new array_type(ValueStack[ValueStack.Depth-4].stn as indexers_types, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
#line 1554 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 274: // array_type -> unsized_array_type
#line 1556 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 275: // unsized_array_type -> tkArray, tkOf, type_ref
#line 1561 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1562 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new array_type(null, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
#line 1563 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 276: // simple_type_list -> simple_type_or_
#line 1568 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1569 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new indexers_types(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
#line 1570 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 277: // simple_type_list -> simple_type_list, tkComma, simple_type_or_
#line 1572 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1573 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as indexers_types).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
#line 1574 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 278: // simple_type_or_ -> simple_type
#line 1579 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 279: // simple_type_or_ -> /* empty */
#line 1581 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = null; }
        break;
      case 280: // set_type -> tkSet, tkOf, type_ref
#line 1586 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1587 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new set_type_definition(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
#line 1588 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 281: // file_type -> tkFile, tkOf, type_ref
#line 1593 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1594 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new file_type(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
#line 1595 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 282: // file_type -> tkFile
#line 1597 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1598 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new file_type();  
#line 1599 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td.source_context = CurrentLocationSpan;
#line 1600 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 283: // string_type -> tkIdentifier, tkSquareOpen, const_expr, tkSquareClose
#line 1605 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1606 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new string_num_definition(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-4].id, CurrentLocationSpan);
#line 1607 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 284: // procedural_type -> procedural_type_kind
#line 1612 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 285: // procedural_type_kind -> proc_type_decl
#line 1617 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 286: // proc_type_decl -> tkProcedure, fp_list
#line 1622 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1623 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-1].stn as formal_parameters,null,null,false,false,null,null,CurrentLocationSpan);
#line 1624 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 287: // proc_type_decl -> tkFunction, fp_list, tkColon, fptype
#line 1630 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1631 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, null, null, null, ValueStack[ValueStack.Depth-1].td as type_definition, CurrentLocationSpan);
#line 1632 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 288: // proc_type_decl -> simple_type_identifier, tkArrow, template_param
#line 1634 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1635 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
#line 1636 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    	}
        break;
      case 289: // proc_type_decl -> template_type, tkArrow, template_param
#line 1638 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1639 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
#line 1640 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    	}
        break;
      case 290: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, template_param
#line 1642 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1643 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		CurrentSemanticValue.td = new modern_proc_type(null,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
#line 1644 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    	}
        break;
      case 291: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   template_param
#line 1646 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1647 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-4].stn as enumerator_list,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
#line 1648 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    	}
        break;
      case 292: // proc_type_decl -> simple_type_identifier, tkArrow, tkRoundOpen, tkRoundClose
#line 1650 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1651 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
#line 1652 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    	}
        break;
      case 293: // proc_type_decl -> template_type, tkArrow, tkRoundOpen, tkRoundClose
#line 1654 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1655 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
#line 1656 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    	}
        break;
      case 294: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, tkRoundOpen, tkRoundClose
#line 1658 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1659 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		CurrentSemanticValue.td = new modern_proc_type(null,null,null,CurrentLocationSpan);
#line 1660 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    	}
        break;
      case 295: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   tkRoundOpen, tkRoundClose
#line 1662 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1663 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-5].stn as enumerator_list,null,CurrentLocationSpan);
#line 1664 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    	}
        break;
      case 296: // object_type -> class_attributes, class_or_interface_keyword, 
                //                optional_base_classes, optional_where_section, 
                //                optional_component_list_seq_end
#line 1669 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1670 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            var cd = NewObjectType((class_attribute)ValueStack[ValueStack.Depth-5].ob, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].stn as named_type_reference_list, ValueStack[ValueStack.Depth-2].stn as where_definition_list, ValueStack[ValueStack.Depth-1].stn as class_body_list, CurrentLocationSpan); 
#line 1671 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = cd;
#line 1672 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 297: // record_type -> tkRecord, optional_base_classes, optional_where_section, 
                //                member_list_section, tkEnd
#line 1677 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1678 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var nnrt = new class_definition(ValueStack[ValueStack.Depth-4].stn as named_type_reference_list, ValueStack[ValueStack.Depth-2].stn as class_body_list, class_keyword.Record, null, ValueStack[ValueStack.Depth-3].stn as where_definition_list, class_attribute.None, false, CurrentLocationSpan); 
#line 1679 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (/*nnrt.body!=null && nnrt.body.class_def_blocks!=null && 
				nnrt.body.class_def_blocks.Count>0 &&*/ 
#line 1680 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				nnrt.body.class_def_blocks[0].access_mod==null)
#line 1681 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			{
#line 1682 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                nnrt.body.class_def_blocks[0].access_mod = new access_modifer_node(access_modifer.public_modifer);
#line 1683 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}        
#line 1684 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = nnrt;
#line 1685 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 298: // class_attribute -> tkSealed
#line 1691 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = class_attribute.Sealed; }
        break;
      case 299: // class_attribute -> tkPartial
#line 1693 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = class_attribute.Partial; }
        break;
      case 300: // class_attribute -> tkAbstract
#line 1695 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = class_attribute.Abstract; }
        break;
      case 301: // class_attribute -> tkAuto
#line 1697 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = class_attribute.Auto; }
        break;
      case 302: // class_attribute -> tkStatic
#line 1699 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = class_attribute.Static; }
        break;
      case 303: // class_attributes -> /* empty */
#line 1704 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1705 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = class_attribute.None; 
#line 1706 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 304: // class_attributes -> class_attributes1
#line 1708 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1709 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
#line 1710 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 305: // class_attributes1 -> class_attribute
#line 1715 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1716 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
#line 1717 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 306: // class_attributes1 -> class_attributes1, class_attribute
#line 1719 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1720 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (((class_attribute)ValueStack[ValueStack.Depth-2].ob & (class_attribute)ValueStack[ValueStack.Depth-1].ob) == (class_attribute)ValueStack[ValueStack.Depth-1].ob)
#line 1721 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                parserTools.AddErrorFromResource("ATTRIBUTE_REDECLARED",LocationStack[LocationStack.Depth-1]);
#line 1722 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob  = ((class_attribute)ValueStack[ValueStack.Depth-2].ob) | ((class_attribute)ValueStack[ValueStack.Depth-1].ob);
#line 1723 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			//$$ = $1;
		}
        break;
      case 307: // class_or_interface_keyword -> tkClass
#line 1729 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 308: // class_or_interface_keyword -> tkInterface
#line 1731 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 309: // class_or_interface_keyword -> tkTemplate
#line 1733 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1734 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-1].ti);
#line 1735 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 310: // class_or_interface_keyword -> tkTemplate, tkClass
#line 1737 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1738 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "c", CurrentLocationSpan);
#line 1739 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 311: // class_or_interface_keyword -> tkTemplate, tkRecord
#line 1741 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1742 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "r", CurrentLocationSpan);
#line 1743 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 312: // class_or_interface_keyword -> tkTemplate, tkInterface
#line 1745 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1746 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "i", CurrentLocationSpan);
#line 1747 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 313: // optional_component_list_seq_end -> /* empty */
#line 1752 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 314: // optional_component_list_seq_end -> member_list_section, tkEnd
#line 1754 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1755 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
#line 1756 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 1757 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 316: // optional_base_classes -> tkRoundOpen, base_classes_names_list, tkRoundClose
#line 1763 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 317: // base_classes_names_list -> base_class_name
#line 1768 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1769 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new named_type_reference_list(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
#line 1770 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 318: // base_classes_names_list -> base_classes_names_list, tkComma, base_class_name
#line 1772 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1773 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as named_type_reference_list).Add(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
#line 1774 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 319: // base_class_name -> simple_type_identifier
#line 1779 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 320: // base_class_name -> template_type
#line 1781 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 321: // template_arguments -> tkLower, ident_list, tkGreater
#line 1786 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 1787 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
#line 1788 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 1789 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 322: // optional_where_section -> /* empty */
#line 1794 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 323: // optional_where_section -> where_part_list
#line 1796 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 324: // where_part_list -> where_part
#line 1801 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1802 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new where_definition_list(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
#line 1803 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 325: // where_part_list -> where_part_list, where_part
#line 1805 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1806 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as where_definition_list).Add(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
#line 1807 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 326: // where_part -> tkWhere, ident_list, tkColon, type_ref_and_secific_list, 
                //               tkSemiColon
#line 1812 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1813 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new where_definition(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-2].stn as where_type_specificator_list, CurrentLocationSpan); 
#line 1814 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 327: // type_ref_and_secific_list -> type_ref_or_secific
#line 1819 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1820 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new where_type_specificator_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
#line 1821 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 328: // type_ref_and_secific_list -> type_ref_and_secific_list, tkComma, 
                //                              type_ref_or_secific
#line 1823 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1824 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as where_type_specificator_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
#line 1825 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 329: // type_ref_or_secific -> type_ref
#line 1830 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 330: // type_ref_or_secific -> tkClass
#line 1832 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1833 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefClass, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
#line 1834 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 331: // type_ref_or_secific -> tkRecord
#line 1836 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1837 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefValueType, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
#line 1838 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 332: // type_ref_or_secific -> tkConstructor
#line 1840 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1841 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
#line 1842 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 333: // member_list_section -> member_list
#line 1847 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1848 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new class_body_list(ValueStack[ValueStack.Depth-1].stn as class_members, CurrentLocationSpan);
#line 1849 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 334: // member_list_section -> member_list_section, ot_visibility_specifier, 
                //                        member_list
#line 1851 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1852 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    (ValueStack[ValueStack.Depth-1].stn as class_members).access_mod = ValueStack[ValueStack.Depth-2].stn as access_modifer_node;
#line 1853 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-3].stn as class_body_list).Add(ValueStack[ValueStack.Depth-1].stn as class_members,CurrentLocationSpan);
#line 1854 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			
#line 1855 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if ((ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks[0].Count == 0)
#line 1856 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                (ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks.RemoveAt(0);
#line 1857 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			
#line 1858 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
#line 1859 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 335: // ot_visibility_specifier -> tkInternal
#line 1864 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.internal_modifer, CurrentLocationSpan); }
        break;
      case 336: // ot_visibility_specifier -> tkPublic
#line 1866 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.public_modifer, CurrentLocationSpan); }
        break;
      case 337: // ot_visibility_specifier -> tkProtected
#line 1868 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.protected_modifer, CurrentLocationSpan); }
        break;
      case 338: // ot_visibility_specifier -> tkPrivate
#line 1870 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.private_modifer, CurrentLocationSpan); }
        break;
      case 339: // member_list -> /* empty */
#line 1875 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = new class_members(); }
        break;
      case 340: // member_list -> field_or_const_definition_list, optional_semicolon
#line 1877 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 341: // member_list -> method_decl_list
#line 1879 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 342: // member_list -> field_or_const_definition_list, tkSemiColon, method_decl_list
#line 1881 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 1882 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-3].stn as class_members).members.AddRange((ValueStack[ValueStack.Depth-1].stn as class_members).members);
#line 1883 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-3].stn as class_members).source_context = CurrentLocationSpan;
#line 1884 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
#line 1885 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 343: // ident_list -> identifier
#line 1890 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1891 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
#line 1892 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 344: // ident_list -> ident_list, tkComma, identifier
#line 1894 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1895 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
#line 1896 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 345: // optional_semicolon -> /* empty */
#line 1901 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = null; }
        break;
      case 346: // optional_semicolon -> tkSemiColon
#line 1903 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 347: // field_or_const_definition_list -> field_or_const_definition
#line 1908 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1909 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
#line 1910 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 348: // field_or_const_definition_list -> field_or_const_definition_list, tkSemiColon, 
                //                                   field_or_const_definition
#line 1912 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{   
#line 1913 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
#line 1914 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 349: // field_or_const_definition -> attribute_declarations, 
                //                              simple_field_or_const_definition
#line 1919 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 1920 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    (ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
#line 1921 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 1922 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 350: // method_decl_list -> method_or_property_decl
#line 1927 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1928 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
#line 1929 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 351: // method_decl_list -> method_decl_list, method_or_property_decl
#line 1931 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1932 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
#line 1933 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 352: // method_or_property_decl -> method_decl_withattr
#line 1938 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 353: // method_or_property_decl -> property_definition
#line 1940 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 354: // simple_field_or_const_definition -> tkConst, only_const_decl
#line 1945 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1946 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 1947 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 1948 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 355: // simple_field_or_const_definition -> field_definition
#line 1950 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 356: // simple_field_or_const_definition -> class_or_static, field_definition
#line 1952 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1953 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).var_attr = definition_attribute.Static;
#line 1954 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).source_context = CurrentLocationSpan;
#line 1955 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 1956 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 357: // class_or_static -> tkStatic
#line 1961 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 358: // class_or_static -> tkClass
#line 1963 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 359: // field_definition -> var_decl_part
#line 1968 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 360: // field_definition -> tkEvent, ident_list, tkColon, type_ref
#line 1970 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 1971 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, true, CurrentLocationSpan); 
#line 1972 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 361: // method_decl_withattr -> attribute_declarations, method_header
#line 1977 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 1978 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].td as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
#line 1979 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td;
#line 1980 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 362: // method_decl_withattr -> attribute_declarations, method_decl
#line 1982 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 1983 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
#line 1984 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (ValueStack[ValueStack.Depth-1].stn is procedure_definition && (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
#line 1985 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
#line 1986 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 1987 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
     }
        break;
      case 363: // method_decl -> inclass_proc_func_decl
#line 1992 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 364: // method_decl -> inclass_constr_destr_decl
#line 1994 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 365: // method_header -> class_or_static, method_procfunc_header
#line 1999 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2000 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
#line 2001 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
#line 2002 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 366: // method_header -> method_procfunc_header
#line 2004 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2005 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; 
#line 2006 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 367: // method_header -> tkAsync, class_or_static, method_procfunc_header
#line 2008 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2009 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
#line 2010 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].td as procedure_header).IsAsync = true;
#line 2011 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
#line 2012 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 368: // method_header -> tkAsync, method_procfunc_header
#line 2014 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2015 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].td as procedure_header).IsAsync = true;
#line 2016 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; 
#line 2017 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 369: // method_header -> constr_destr_header
#line 2019 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 370: // method_procfunc_header -> proc_func_header
#line 2024 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2025 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = NewProcfuncHeading(ValueStack[ValueStack.Depth-1].td as procedure_header);
#line 2026 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 371: // proc_func_header -> proc_header
#line 2031 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 372: // proc_func_header -> func_header
#line 2033 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 373: // constr_destr_header -> tkConstructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
#line 2038 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2039 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,false,null,null,CurrentLocationSpan);
#line 2040 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 374: // constr_destr_header -> class_or_static, tkConstructor, optional_proc_name, 
                //                        fp_list, optional_method_modificators
#line 2042 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2043 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,true,null,null,CurrentLocationSpan);
#line 2044 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 375: // constr_destr_header -> tkDestructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
#line 2046 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2047 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new destructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name, false,false,null,null,CurrentLocationSpan);
#line 2048 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 376: // optional_proc_name -> proc_name
#line 2053 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 377: // optional_proc_name -> /* empty */
#line 2055 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 378: // property_definition -> attribute_declarations, simple_property_definition
#line 2060 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 2061 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = NewPropertyDefinition(ValueStack[ValueStack.Depth-2].stn as attribute_list, ValueStack[ValueStack.Depth-1].stn as declaration, LocationStack[LocationStack.Depth-1]);
#line 2062 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 379: // simple_property_definition -> tkProperty, func_name, property_interface, 
                //                               property_specifiers, tkSemiColon, 
                //                               array_defaultproperty
#line 2067 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2068 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
#line 2069 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 380: // simple_property_definition -> tkProperty, func_name, property_interface, 
                //                               property_specifiers, tkSemiColon, 
                //                               property_modificator, tkSemiColon, 
                //                               array_defaultproperty
#line 2071 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2072 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            proc_attribute pa = proc_attribute.attr_none;
#line 2073 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (ValueStack[ValueStack.Depth-3].id.name.ToLower() == "virtual")
#line 2074 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
               	pa = proc_attribute.attr_virtual;
#line 2075 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
 			else if (ValueStack[ValueStack.Depth-3].id.name.ToLower() == "override") 
#line 2076 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
 			    pa = proc_attribute.attr_override;
#line 2077 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            else if (ValueStack[ValueStack.Depth-3].id.name.ToLower() == "abstract") 
#line 2078 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
 			    pa = proc_attribute.attr_abstract;
#line 2079 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-7].stn as method_name, ValueStack[ValueStack.Depth-6].stn as property_interface, ValueStack[ValueStack.Depth-5].stn as property_accessors, pa, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
#line 2080 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 381: // simple_property_definition -> class_or_static, tkProperty, func_name, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, array_defaultproperty
#line 2082 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2083 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
#line 2084 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
#line 2085 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 382: // simple_property_definition -> class_or_static, tkProperty, func_name, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, property_modificator, tkSemiColon, 
                //                               array_defaultproperty
#line 2087 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2088 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			parserTools.AddErrorFromResource("STATIC_PROPERTIES_CANNOT_HAVE_ATTRBUTE_{0}",LocationStack[LocationStack.Depth-3],ValueStack[ValueStack.Depth-3].id.name);        	
#line 2089 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 383: // simple_property_definition -> tkAuto, tkProperty, func_name, property_interface, 
                //                               optional_property_initialization, tkSemiColon
#line 2091 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2092 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
#line 2093 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
#line 2094 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
#line 2095 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 384: // simple_property_definition -> class_or_static, tkAuto, tkProperty, func_name, 
                //                               property_interface, 
                //                               optional_property_initialization, tkSemiColon
#line 2097 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2098 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
#line 2099 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
#line 2100 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
#line 2101 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
#line 2102 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 385: // optional_property_initialization -> tkAssign, expr
#line 2106 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 386: // optional_property_initialization -> /* empty */
#line 2107 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = null; }
        break;
      case 387: // array_defaultproperty -> /* empty */
#line 2112 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 388: // array_defaultproperty -> tkDefault, tkSemiColon
#line 2114 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2115 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new property_array_default();  
#line 2116 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 2117 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 389: // property_interface -> property_parameter_list, tkColon, fptype
#line 2124 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2125 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new property_interface(ValueStack[ValueStack.Depth-3].stn as property_parameter_list, ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan);
#line 2126 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 390: // property_parameter_list -> /* empty */
#line 2131 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 391: // property_parameter_list -> tkSquareOpen, parameter_decl_list, tkSquareClose
#line 2133 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 392: // parameter_decl_list -> parameter_decl
#line 2138 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2139 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new property_parameter_list(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
#line 2140 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 393: // parameter_decl_list -> parameter_decl_list, tkSemiColon, parameter_decl
#line 2142 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2143 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as property_parameter_list).Add(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
#line 2144 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 394: // parameter_decl -> ident_list, tkColon, fptype
#line 2149 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2150 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new property_parameter(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
#line 2151 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 395: // optional_read_expr -> expr_with_func_decl_lambda
#line 2170 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 396: // optional_read_expr -> /* empty */
#line 2172 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = null; }
        break;
      case 398: // property_specifiers -> tkRead, optional_read_expr, write_property_specifiers
#line 2178 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2179 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	if (ValueStack[ValueStack.Depth-2].ex == null || ValueStack[ValueStack.Depth-2].ex is ident) // с�?анда�?�?н�?е свойс�?ва
        	{
#line 2180 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		CurrentSemanticValue.stn = NewPropertySpecifiersRead(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-2].ex as ident, null, null, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan);
#line 2181 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	}
#line 2182 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	else // �?ас�?и�?енн�?е свойс�?ва
        	{
#line 2183 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var id = NewId("#GetGen", LocationStack[LocationStack.Depth-2]);
#line 2184 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                procedure_definition pr = null;
#line 2185 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                if (!parserTools.buildTreeForFormatter)
#line 2186 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    pr = CreateAndAddToClassReadFunc(ValueStack[ValueStack.Depth-2].ex, id, LocationStack[LocationStack.Depth-2]);
#line 2187 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = NewPropertySpecifiersRead(ValueStack[ValueStack.Depth-3].id, id, pr, ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan); // $2 пе�?еда�?�?ся для �?о�?ма�?и�?ования 
			}
#line 2188 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 399: // property_specifiers -> tkWrite, unlabelled_stmt, read_property_specifiers
#line 2193 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2194 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
#line 2195 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	{
#line 2196 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	
#line 2197 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-3].id, null, null, null, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan);
#line 2198 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	}
#line 2199 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	else if (ValueStack[ValueStack.Depth-2].stn is procedure_call && (ValueStack[ValueStack.Depth-2].stn as procedure_call).is_ident) // с�?анда�?�?н�?е свойс�?ва
        	{
#line 2200 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	
#line 2201 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-3].id, (ValueStack[ValueStack.Depth-2].stn as procedure_call).func_name as ident, null, null, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan);  // с�?а�?�?е свойс�?ва - с иден�?и�?ика�?о�?ом
        	}
#line 2202 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	else // �?ас�?и�?енн�?е свойс�?ва
        	{
#line 2203 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var id = NewId("#SetGen", LocationStack[LocationStack.Depth-2]);
#line 2204 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                procedure_definition pr = null;
#line 2205 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                if (!parserTools.buildTreeForFormatter)
#line 2206 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    pr = CreateAndAddToClassWriteProc(ValueStack[ValueStack.Depth-2].stn as statement,id,LocationStack[LocationStack.Depth-2]);
#line 2207 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                if (parserTools.buildTreeForFormatter)
#line 2208 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-3].id, id, pr, ValueStack[ValueStack.Depth-2].stn as statement, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan); // $2 пе�?еда�?�?ся для �?о�?ма�?и�?ования
				else CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-3].id, id, pr, null, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan); 	
#line 2209 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}
#line 2210 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 401: // write_property_specifiers -> tkWrite, unlabelled_stmt
#line 2219 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2220 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	if (ValueStack[ValueStack.Depth-1].stn is empty_statement)
#line 2221 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	{
#line 2222 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	
#line 2223 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-2].id, null, null, null, null, CurrentLocationSpan);
#line 2224 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	}
#line 2225 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	else if (ValueStack[ValueStack.Depth-1].stn is procedure_call && (ValueStack[ValueStack.Depth-1].stn as procedure_call).is_ident)
#line 2226 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	{
#line 2227 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-2].id, (ValueStack[ValueStack.Depth-1].stn as procedure_call).func_name as ident, null, null, null, CurrentLocationSpan); // с�?а�?�?е свойс�?ва - с иден�?и�?ика�?о�?ом
        	}
#line 2228 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	else 
#line 2229 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	{
#line 2230 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var id = NewId("#SetGen", LocationStack[LocationStack.Depth-1]);
#line 2231 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                procedure_definition pr = null;
#line 2232 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                if (!parserTools.buildTreeForFormatter)
#line 2233 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    pr = CreateAndAddToClassWriteProc(ValueStack[ValueStack.Depth-1].stn as statement,id,LocationStack[LocationStack.Depth-1]);
#line 2234 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                if (parserTools.buildTreeForFormatter)
#line 2235 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-2].id, id, pr, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
#line 2236 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				else CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-2].id, id, pr, null, null, CurrentLocationSpan);	
#line 2237 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}
#line 2238 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
       }
        break;
      case 403: // read_property_specifiers -> tkRead, optional_read_expr
#line 2245 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2246 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	if (ValueStack[ValueStack.Depth-1].ex == null || ValueStack[ValueStack.Depth-1].ex is ident)
#line 2247 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	{
#line 2248 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		CurrentSemanticValue.stn = NewPropertySpecifiersRead(ValueStack[ValueStack.Depth-2].id, ValueStack[ValueStack.Depth-1].ex as ident, null, null, null, CurrentLocationSpan);
#line 2249 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	}
#line 2250 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	else 
#line 2251 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	{
#line 2252 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var id = NewId("#GetGen", LocationStack[LocationStack.Depth-1]);
#line 2253 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                procedure_definition pr = null;
#line 2254 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                if (!parserTools.buildTreeForFormatter)
#line 2255 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    pr = CreateAndAddToClassReadFunc(ValueStack[ValueStack.Depth-1].ex,id,LocationStack[LocationStack.Depth-1]);
#line 2256 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = NewPropertySpecifiersRead(ValueStack[ValueStack.Depth-2].id, id, pr, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan);
#line 2257 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}
#line 2258 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
       }
        break;
      case 404: // var_decl -> var_decl_part, tkSemiColon
#line 2263 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 407: // var_decl_part -> ident_list, tkColon, type_ref
#line 2288 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2289 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, false, CurrentLocationSpan);
#line 2290 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 408: // var_decl_part -> ident_list, tkAssign, expr_with_func_decl_lambda
#line 2292 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2293 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, null, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan);		
#line 2294 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 409: // var_decl_part -> ident_list, tkColon, type_ref, tkAssignOrEqual, 
                //                  typed_var_init_expression
#line 2300 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2301 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan); 
#line 2302 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 410: // typed_var_init_expression -> typed_const_plus
#line 2307 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 411: // typed_var_init_expression -> const_simple_expr, tkDotDot, const_term
#line 2309 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2310 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		if (parserTools.buildTreeForFormatter)
#line 2311 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
#line 2312 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		else 
#line 2313 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new diapason_expr_new(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); 
#line 2314 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 412: // typed_var_init_expression -> expl_func_decl_lambda
#line 2316 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 413: // typed_var_init_expression -> identifier, tkArrow, lambda_function_body
#line 2318 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 2319 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var idList = new ident_list(ValueStack[ValueStack.Depth-3].id, LocationStack[LocationStack.Depth-3]); 
#line 2320 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
#line 2321 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
#line 2322 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 414: // typed_var_init_expression -> tkRoundOpen, tkRoundClose, lambda_type_ref, 
                //                              tkArrow, lambda_function_body
#line 2324 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2325 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
#line 2326 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 415: // typed_var_init_expression -> tkRoundOpen, typed_const_list, tkRoundClose, 
                //                              tkArrow, lambda_function_body
#line 2328 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 2329 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    var el = ValueStack[ValueStack.Depth-4].stn as expression_list;
#line 2330 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    var cnt = el.Count;
#line 2331 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    
#line 2332 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var idList = new ident_list();
#line 2333 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			idList.source_context = LocationStack[LocationStack.Depth-4];
#line 2334 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			
#line 2335 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			for (int j = 0; j < cnt; j++)
#line 2336 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			{
#line 2337 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (!(el.expressions[j] is ident))
#line 2338 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					parserTools.AddErrorFromResource("ONE_TKIDENTIFIER",el.expressions[j].source_context);
#line 2339 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				idList.idents.Add(el.expressions[j] as ident);
#line 2340 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}	
#line 2341 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				
#line 2342 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var any = new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-4]);	
#line 2343 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				
#line 2344 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var formalPars = new formal_parameters(new typed_parameters(idList, any, parametr_kind.none, null, LocationStack[LocationStack.Depth-4]), LocationStack[LocationStack.Depth-4]);
#line 2345 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, any, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
#line 2346 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 416: // typed_var_init_expression -> new_question_expr
#line 2348 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 417: // typed_const_plus -> typed_const
#line 2353 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 418: // constr_destr_decl -> constr_destr_header, block
#line 2360 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2361 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
#line 2362 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 419: // constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, tkAssign, 
                //                      unlabelled_stmt, tkSemiColon
#line 2364 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2365 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
#line 2366 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
#line 2367 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,false,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
#line 2368 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-6].Merge(LocationStack[LocationStack.Depth-2]));
#line 2369 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (parserTools.buildTreeForFormatter)
#line 2370 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
#line 2371 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 420: // constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
                //                      fp_list, tkAssign, unlabelled_stmt, tkSemiColon
#line 2373 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2374 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
#line 2375 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
#line 2376 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,true,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
#line 2377 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-2]));
#line 2378 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (parserTools.buildTreeForFormatter)
#line 2379 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
#line 2380 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 421: // inclass_constr_destr_decl -> constr_destr_header, inclass_block
#line 2385 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2386 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
#line 2387 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 422: // inclass_constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, 
                //                              tkAssign, unlabelled_stmt, tkSemiColon
#line 2389 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2390 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
#line 2391 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
#line 2392 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,false,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
#line 2393 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-6].Merge(LocationStack[LocationStack.Depth-2]));
#line 2394 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (parserTools.buildTreeForFormatter)
#line 2395 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
#line 2396 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 423: // inclass_constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
                //                              fp_list, tkAssign, unlabelled_stmt, tkSemiColon
#line 2398 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2399 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
#line 2400 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
#line 2401 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,true,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
#line 2402 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-2]));
#line 2403 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (parserTools.buildTreeForFormatter)
#line 2404 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
#line 2405 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 424: // proc_func_decl -> proc_func_decl_noclass
#line 2410 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 425: // proc_func_decl -> class_or_static, proc_func_decl_noclass
#line 2412 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2413 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
#line 2414 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 2415 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 426: // proc_func_decl -> tkAsync, proc_func_decl_noclass
#line 2417 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2418 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;		
#line 2419 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
#line 2420 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 427: // proc_func_decl -> tkAsync, class_or_static, proc_func_decl_noclass
#line 2422 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2423 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
#line 2424 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
#line 2425 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 2426 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 428: // proc_func_decl -> class_or_static, tkAsync, proc_func_decl_noclass
#line 2428 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2429 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
#line 2430 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
#line 2431 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 2432 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 429: // proc_func_decl_noclass -> proc_func_header, proc_func_external_block
#line 2437 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2438 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
#line 2439 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 430: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                           optional_method_modificators1, tkAssign, expr_l1, 
                //                           tkSemiColon
#line 2441 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2442 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
#line 2443 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 431: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, expr_l1, 
                //                           tkSemiColon
#line 2445 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2446 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (ValueStack[ValueStack.Depth-2].ex is dot_question_node)
#line 2447 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("DOT_QUECTION_IN_SHORT_FUN",LocationStack[LocationStack.Depth-2]);
#line 2448 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	
#line 2449 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
#line 2450 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 432: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                           optional_method_modificators1, tkAssign, 
                //                           func_decl_lambda, tkSemiColon
#line 2452 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2453 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
#line 2454 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 433: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, 
                //                           func_decl_lambda, tkSemiColon
#line 2456 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2457 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
#line 2458 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 434: // proc_func_decl_noclass -> tkProcedure, proc_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, 
                //                           unlabelled_stmt, tkSemiColon
#line 2460 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2461 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
#line 2462 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-2]);
#line 2463 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
#line 2464 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 435: // proc_func_decl_noclass -> proc_func_header, tkForward, tkSemiColon
#line 2466 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2467 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-3].td as procedure_header, null, CurrentLocationSpan);
#line 2468 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            (CurrentSemanticValue.stn as procedure_definition).proc_header.proc_attributes.Add((procedure_attribute)ValueStack[ValueStack.Depth-2].id, ValueStack[ValueStack.Depth-2].id.source_context);
#line 2469 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 436: // inclass_proc_func_decl -> inclass_proc_func_decl_noclass
#line 2478 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2479 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
#line 2480 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 437: // inclass_proc_func_decl -> tkAsync, inclass_proc_func_decl_noclass
#line 2482 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2483 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
#line 2484 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
#line 2485 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 438: // inclass_proc_func_decl -> class_or_static, inclass_proc_func_decl_noclass
#line 2487 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2488 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
#line 2489 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    {
#line 2490 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
#line 2491 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}
#line 2492 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 2493 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 439: // inclass_proc_func_decl -> tkAsync, class_or_static, 
                //                           inclass_proc_func_decl_noclass
#line 2495 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2496 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
#line 2497 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    {
#line 2498 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
#line 2499 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
#line 2500 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}
#line 2501 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 2502 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 440: // inclass_proc_func_decl -> class_or_static, tkAsync, 
                //                           inclass_proc_func_decl_noclass
#line 2504 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2505 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
#line 2506 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    {
#line 2507 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
#line 2508 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
#line 2509 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}
#line 2510 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 2511 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 441: // inclass_proc_func_decl_noclass -> proc_func_header, inclass_block
#line 2516 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2517 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
#line 2518 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 442: // inclass_proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, 
                //                                   fptype, optional_method_modificators1, 
                //                                   tkAssign, expr_l1_func_decl_lambda, 
                //                                   tkSemiColon
#line 2520 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2521 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
#line 2522 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (parserTools.buildTreeForFormatter)
#line 2523 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
#line 2524 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 443: // inclass_proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                                   optional_method_modificators1, tkAssign, 
                //                                   expr_l1_func_decl_lambda, tkSemiColon
#line 2526 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2527 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
#line 2528 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (parserTools.buildTreeForFormatter)
#line 2529 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
#line 2530 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 444: // inclass_proc_func_decl_noclass -> tkProcedure, proc_name, fp_list, 
                //                                   optional_method_modificators1, tkAssign, 
                //                                   unlabelled_stmt, tkSemiColon
#line 2532 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2533 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
#line 2534 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (parserTools.buildTreeForFormatter)
#line 2535 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
#line 2536 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 445: // proc_func_external_block -> block
#line 2541 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 446: // proc_func_external_block -> external_block
#line 2543 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 447: // proc_name -> func_name
#line 2548 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 448: // func_name -> func_meth_name_ident
#line 2553 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2554 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new method_name(null,null, ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan); 
#line 2555 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 449: // func_name -> func_class_name_ident_list, tkPoint, func_meth_name_ident
#line 2557 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2558 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	var ln = ValueStack[ValueStack.Depth-3].ob as List<ident>;
#line 2559 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	var cnt = ln.Count;
#line 2560 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	if (cnt == 1)
#line 2561 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = new method_name(null, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
#line 2562 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else 	
#line 2563 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = new method_name(ln, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
#line 2564 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 450: // func_class_name_ident -> func_name_with_template_args
#line 2569 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 451: // func_class_name_ident_list -> func_class_name_ident
#line 2574 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2575 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = new List<ident>(); 
#line 2576 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
#line 2577 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 452: // func_class_name_ident_list -> func_class_name_ident_list, tkPoint, 
                //                               func_class_name_ident
#line 2579 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2580 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-3].ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
#line 2581 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob; 
#line 2582 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 453: // func_meth_name_ident -> func_name_with_template_args
#line 2587 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 454: // func_meth_name_ident -> operator_name_ident
#line 2589 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 455: // func_meth_name_ident -> operator_name_ident, template_arguments
#line 2591 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = new template_operator_name(null, ValueStack[ValueStack.Depth-1].stn as ident_list, ValueStack[ValueStack.Depth-2].ex as operator_name_ident, CurrentLocationSpan); }
        break;
      case 456: // func_name_with_template_args -> func_name_ident
#line 2596 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 457: // func_name_with_template_args -> func_name_ident, template_arguments
#line 2598 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2599 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
#line 2600 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 458: // func_name_ident -> identifier
#line 2605 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 459: // proc_header -> tkProcedure, proc_name, fp_list, optional_method_modificators, 
                //                optional_where_section
#line 2612 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2613 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, CurrentLocationSpan); 
#line 2614 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 460: // func_header -> tkFunction, func_name, fp_list, optional_method_modificators, 
                //                optional_where_section
#line 2619 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2620 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, null, CurrentLocationSpan); 
#line 2621 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 461: // func_header -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                optional_method_modificators, optional_where_section
#line 2623 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2624 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, ValueStack[ValueStack.Depth-3].td as type_definition, CurrentLocationSpan); 
#line 2625 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 462: // external_block -> tkExternal, external_directive_ident, tkName, 
                //                   external_directive_ident, tkSemiColon
#line 2630 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2631 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
#line 2632 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 463: // external_block -> tkExternal, external_directive_ident, tkSemiColon
#line 2634 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2635 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-2].ex, null, CurrentLocationSpan); 
#line 2636 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 464: // external_block -> tkExternal, tkSemiColon
#line 2638 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2639 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new external_directive(null, null, CurrentLocationSpan); 
#line 2640 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 465: // external_directive_ident -> identifier
#line 2645 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 466: // external_directive_ident -> literal
#line 2647 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 467: // block -> decl_sect_list, compound_stmt, tkSemiColon
#line 2652 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2653 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
#line 2654 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 468: // inclass_block -> inclass_decl_sect_list, compound_stmt, tkSemiColon
#line 2659 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2660 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
#line 2661 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 469: // inclass_block -> external_block
#line 2663 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 470: // fp_list -> /* empty */
#line 2668 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 471: // fp_list -> tkRoundOpen, tkRoundClose
#line 2670 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2671 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = null;
#line 2672 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 472: // fp_list -> tkRoundOpen, fp_sect_list, tkRoundClose
#line 2674 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2675 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
#line 2676 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (CurrentSemanticValue.stn != null)
#line 2677 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 2678 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 473: // fp_sect_list -> fp_sect
#line 2683 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2684 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new formal_parameters(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
#line 2685 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 474: // fp_sect_list -> fp_sect_list, tkSemiColon, fp_sect
#line 2687 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2688 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);   
#line 2689 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 475: // fp_sect -> attribute_declarations, simple_fp_sect
#line 2694 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 2695 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as  attribute_list;
#line 2696 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 2697 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 476: // simple_fp_sect -> param_name_list, tkColon, fptype
#line 2702 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2703 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan); 
#line 2704 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 477: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype
#line 2706 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2707 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.var_parametr, null, CurrentLocationSpan);  
#line 2708 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 478: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype
#line 2710 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2711 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.const_parametr, null, CurrentLocationSpan);  
#line 2712 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 479: // simple_fp_sect -> tkParams, param_name_list, tkColon, fptype
#line 2714 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2715 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td,parametr_kind.params_parametr,null, CurrentLocationSpan);  
#line 2716 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 480: // simple_fp_sect -> param_name_list, tkColon, fptype, tkAssign, expr
#line 2718 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2719 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.none, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 2720 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 481: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype, tkAssign, expr
#line 2722 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2723 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.var_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
#line 2724 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 482: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype, tkAssign, expr
#line 2726 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2727 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.const_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
#line 2728 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 483: // param_name_list -> param_name
#line 2733 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2734 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
#line 2735 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 484: // param_name_list -> param_name_list, tkComma, param_name
#line 2737 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2738 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);  
#line 2739 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 485: // param_name -> identifier
#line 2744 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 486: // fptype -> type_ref
#line 2749 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 487: // fptype_noproctype -> simple_type
#line 2754 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 488: // fptype_noproctype -> string_type
#line 2756 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 489: // fptype_noproctype -> pointer_type
#line 2758 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 490: // fptype_noproctype -> structured_type
#line 2760 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 491: // fptype_noproctype -> template_type
#line 2762 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 492: // stmt -> unlabelled_stmt
#line 2767 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 493: // stmt -> label_name, tkColon, stmt
#line 2769 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2770 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new labeled_statement(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);  
#line 2771 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 494: // unlabelled_stmt -> /* empty */
#line 2776 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2777 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new empty_statement(); 
#line 2778 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = null;
#line 2779 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 495: // unlabelled_stmt -> assignment
#line 2781 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 496: // unlabelled_stmt -> proc_call
#line 2783 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 497: // unlabelled_stmt -> goto_stmt
#line 2785 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 498: // unlabelled_stmt -> compound_stmt
#line 2787 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 499: // unlabelled_stmt -> if_stmt
#line 2789 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 500: // unlabelled_stmt -> case_stmt
#line 2791 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 501: // unlabelled_stmt -> repeat_stmt
#line 2793 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 502: // unlabelled_stmt -> while_stmt
#line 2795 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 503: // unlabelled_stmt -> for_stmt
#line 2797 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 504: // unlabelled_stmt -> with_stmt
#line 2799 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 505: // unlabelled_stmt -> inherited_message
#line 2801 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 506: // unlabelled_stmt -> try_stmt
#line 2803 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 507: // unlabelled_stmt -> raise_stmt
#line 2805 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 508: // unlabelled_stmt -> foreach_stmt
#line 2807 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 509: // unlabelled_stmt -> var_stmt
#line 2809 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 510: // unlabelled_stmt -> expr_as_stmt
#line 2811 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 511: // unlabelled_stmt -> lock_stmt
#line 2813 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 512: // unlabelled_stmt -> yield_stmt
#line 2815 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 513: // unlabelled_stmt -> yield_sequence_stmt
#line 2817 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 514: // unlabelled_stmt -> loop_stmt
#line 2819 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 515: // unlabelled_stmt -> match_with
#line 2821 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 516: // loop_stmt -> tkLoop, expr_l1, tkDo, unlabelled_stmt
#line 2826 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2827 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new loop_stmt(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].stn as statement,CurrentLocationSpan);
#line 2828 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 517: // yield_stmt -> tkYield, expr_l1_func_decl_lambda
#line 2833 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2834 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new yield_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
#line 2835 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 518: // yield_sequence_stmt -> tkYield, tkSequence, expr_l1_func_decl_lambda
#line 2840 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2841 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new yield_sequence_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
#line 2842 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 519: // var_stmt -> tkVar, var_decl_part
#line 2847 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2848 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new var_statement(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
#line 2849 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 520: // var_stmt -> tkRoundOpen, tkVar, identifier, tkComma, var_ident_list, 
                //             tkRoundClose, tkAssign, expr
#line 2851 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2852 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-4].ob as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
#line 2853 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
#line 2854 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].ob as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 2855 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 521: // var_stmt -> tkVar, tkRoundOpen, identifier, tkComma, ident_list, tkRoundClose, 
                //             tkAssign, expr
#line 2857 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2858 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-4].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
#line 2859 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			ValueStack[ValueStack.Depth-4].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
#line 2860 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 2861 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	    }
        break;
      case 522: // assignment -> var_reference, assign_operator, expr_with_func_decl_lambda
#line 2866 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{      
#line 2867 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	if (!(ValueStack[ValueStack.Depth-3].ex is addressed_value))
#line 2868 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		parserTools.AddErrorFromResource("LEFT_SIDE_CANNOT_BE_ASSIGNED_TO",CurrentLocationSpan);
#line 2869 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan);
#line 2870 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 523: // assignment -> tkRoundOpen, variable, tkComma, variable_list, tkRoundClose, 
                //               assign_operator, expr
#line 2872 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2873 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (ValueStack[ValueStack.Depth-2].op.type != Operators.Assignment)
#line 2874 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			    parserTools.AddErrorFromResource("ONLY_BASE_ASSIGNMENT_FOR_TUPLE",LocationStack[LocationStack.Depth-2]);
#line 2875 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-4].ob as addressed_value_list).Insert(0,ValueStack[ValueStack.Depth-6].ex as addressed_value);
#line 2876 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
#line 2877 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new assign_tuple(ValueStack[ValueStack.Depth-4].ob as addressed_value_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 2878 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 524: // variable_list -> variable
#line 2896 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2897 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		CurrentSemanticValue.ob = new addressed_value_list(ValueStack[ValueStack.Depth-1].ex as addressed_value,LocationStack[LocationStack.Depth-1]);
#line 2898 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	}
        break;
      case 525: // variable_list -> variable_list, tkComma, variable
#line 2900 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2901 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		(ValueStack[ValueStack.Depth-3].ob as addressed_value_list).Add(ValueStack[ValueStack.Depth-1].ex as addressed_value);
#line 2902 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		(ValueStack[ValueStack.Depth-3].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
#line 2903 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
#line 2904 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	}
        break;
      case 526: // var_ident_list -> tkVar, identifier
#line 2909 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2910 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		CurrentSemanticValue.ob = new ident_list(ValueStack[ValueStack.Depth-1].id,CurrentLocationSpan);
#line 2911 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	}
        break;
      case 527: // var_ident_list -> var_ident_list, tkComma, tkVar, identifier
#line 2913 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2914 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		(ValueStack[ValueStack.Depth-4].ob as ident_list).Add(ValueStack[ValueStack.Depth-1].id);
#line 2915 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		(ValueStack[ValueStack.Depth-4].ob as ident_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
#line 2916 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-4].ob;
#line 2917 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	}
        break;
      case 528: // proc_call -> var_reference
#line 2922 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2923 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new procedure_call(ValueStack[ValueStack.Depth-1].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex is ident, CurrentLocationSpan); 
#line 2924 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 529: // goto_stmt -> tkGoto, label_name
#line 2934 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2935 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new goto_statement(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
#line 2936 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 530: // compound_stmt -> tkBegin, stmt_list, tkEnd
#line 2941 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2942 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
#line 2943 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
#line 2944 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
#line 2945 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 2946 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 531: // stmt_list -> stmt
#line 2951 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2952 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, LocationStack[LocationStack.Depth-1]);
#line 2953 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 532: // stmt_list -> stmt_list, tkSemiColon, stmt
#line 2955 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{  
#line 2956 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as statement_list).Add(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 2957 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 533: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt
#line 2962 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2963 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan); 
#line 2964 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 534: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt, tkElse, unlabelled_stmt
#line 2966 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2967 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as statement, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
#line 2968 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 535: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, else_case, tkEnd
#line 2973 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2974 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
#line 2975 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 536: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, tkSemiColon, else_case, 
                //               tkEnd
#line 2977 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 2978 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
#line 2979 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 537: // pattern_cases -> pattern_case
#line 2984 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2985 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new pattern_cases(ValueStack[ValueStack.Depth-1].stn as pattern_case);
#line 2986 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 538: // pattern_cases -> pattern_cases, tkSemiColon, pattern_case
#line 2988 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2989 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as pattern_cases).Add(ValueStack[ValueStack.Depth-1].stn as pattern_case);
#line 2990 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 539: // pattern_case -> pattern_optional_var, tkWhen, expr_l1, tkColon, unlabelled_stmt
#line 2995 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 2996 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
#line 2997 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 540: // pattern_case -> deconstruction_or_const_pattern, tkColon, unlabelled_stmt
#line 2999 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3000 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
#line 3001 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 541: // pattern_case -> collection_pattern, tkColon, unlabelled_stmt
#line 3003 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3004 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
#line 3005 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 542: // pattern_case -> tuple_pattern, tkWhen, expr_l1, tkColon, unlabelled_stmt
#line 3007 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3008 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
#line 3009 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 543: // pattern_case -> tuple_pattern, tkColon, unlabelled_stmt
#line 3011 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3012 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
#line 3013 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 544: // case_stmt -> tkCase, expr_l1, tkOf, case_list, else_case, tkEnd
#line 3018 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3019 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
#line 3020 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 545: // case_stmt -> tkCase, expr_l1, tkOf, case_list, tkSemiColon, else_case, tkEnd
#line 3022 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3023 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
#line 3024 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 546: // case_stmt -> tkCase, expr_l1, tkOf, else_case, tkEnd
#line 3026 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3027 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-4].ex, NewCaseItem(new empty_statement(), null), ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
#line 3028 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 547: // case_list -> case_item
#line 3033 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3034 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (ValueStack[ValueStack.Depth-1].stn is empty_statement) 
#line 3035 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, null);
#line 3036 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
#line 3037 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 548: // case_list -> case_list, tkSemiColon, case_item
#line 3039 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3040 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = AddCaseItem(ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
#line 3041 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 549: // case_item -> case_label_list, tkColon, unlabelled_stmt
#line 3046 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3047 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new case_variant(ValueStack[ValueStack.Depth-3].stn as expression_list, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
#line 3048 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 550: // case_label_list -> case_label
#line 3053 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3054 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 3055 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 551: // case_label_list -> case_label_list, tkComma, case_label
#line 3057 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3058 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 3059 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 552: // case_label -> const_elem
#line 3064 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 553: // else_case -> /* empty */
#line 3069 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null;}
        break;
      case 554: // else_case -> tkElse, stmt_list
#line 3071 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 555: // repeat_stmt -> tkRepeat, stmt_list, tkUntil, expr
#line 3076 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3077 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    CurrentSemanticValue.stn = new repeat_node(ValueStack[ValueStack.Depth-3].stn as statement_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 3078 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-3].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-4].ti;
#line 3079 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-3].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-2].ti;
#line 3080 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			ValueStack[ValueStack.Depth-3].stn.source_context = LocationStack[LocationStack.Depth-4].Merge(LocationStack[LocationStack.Depth-2]);
#line 3081 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 556: // while_stmt -> tkWhile, expr_l1, optional_tk_do, unlabelled_stmt
#line 3086 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3087 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = NewWhileStmt(ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);    
#line 3088 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 557: // optional_tk_do -> tkDo
#line 3093 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 558: // optional_tk_do -> /* empty */
#line 3095 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = null; }
        break;
      case 559: // lock_stmt -> tkLock, expr_l1, tkDo, unlabelled_stmt
#line 3100 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3101 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new lock_stmt(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
#line 3102 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 560: // index_or_nothing -> tkIndex, tkIdentifier
#line 3107 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 561: // index_or_nothing -> /* empty */
#line 3109 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = null; }
        break;
      case 562: // optional_type_specification -> tkColon, type_ref
#line 3114 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 563: // optional_type_specification -> /* empty */
#line 3116 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = null; }
        break;
      case 564: // optional_var -> tkVar
#line 3121 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = true; }
        break;
      case 565: // optional_var -> /* empty */
#line 3123 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = false; }
        break;
      case 566: // for_cycle_type -> tkTo
#line 3128 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = for_cycle_type.to; }
        break;
      case 567: // for_cycle_type -> tkDownto
#line 3130 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ob = for_cycle_type.downto; }
        break;
      case 568: // foreach_stmt -> tkForeach, identifier, optional_type_specification, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
#line 3135 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3136 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
#line 3137 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (ValueStack[ValueStack.Depth-6].td == null)
#line 3138 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                parserTools.AddWarningFromResource("USING_UNLOCAL_FOREACH_VARIABLE", ValueStack[ValueStack.Depth-7].id.source_context);
#line 3139 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 569: // foreach_stmt -> tkForeach, tkVar, identifier, optional_type_specification, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
#line 3141 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3142 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	if (ValueStack[ValueStack.Depth-6].td == null)
#line 3143 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, new no_type_foreach(), ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
#line 3144 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
#line 3145 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 570: // foreach_stmt -> tkForeach, tkVar, tkRoundOpen, ident_list, tkRoundClose, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
#line 3147 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3148 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	if (parserTools.buildTreeForFormatter)
#line 3149 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	{
#line 3150 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		var il = ValueStack[ValueStack.Depth-7].stn as ident_list;
#line 3151 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		il.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6]); // н�?жно для �?о�?ма�?и�?ования
        		CurrentSemanticValue.stn = new foreach_stmt_formatting(il,ValueStack[ValueStack.Depth-4].ex,ValueStack[ValueStack.Depth-1].stn as statement,ValueStack[ValueStack.Depth-3].id,CurrentLocationSpan);
#line 3152 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	}
#line 3153 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	else
#line 3154 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	{
#line 3155 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		// �?с�?�? п�?облема - непоня�?но, где здес�? сдела�?�? семан�?�?еский �?зел для п�?ове�?ки
        		// �?�?ове�?и�?�? можно и в foreach, но где-�?о должен б�?�?�? ма�?ке�?, �?�?о э�?о са�?а�?н�?й �?зел
        		// Нап�?име�?, иден�?и�?ика�?о�? #fe - но э�?о пло�?ая идея
                var id = NewId("#fe",LocationStack[LocationStack.Depth-7]);
#line 3156 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                var tttt = new assign_var_tuple(ValueStack[ValueStack.Depth-7].stn as ident_list, id, CurrentLocationSpan);
#line 3157 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                statement_list nine = ValueStack[ValueStack.Depth-1].stn is statement_list ? ValueStack[ValueStack.Depth-1].stn as statement_list : new statement_list(ValueStack[ValueStack.Depth-1].stn as statement,LocationStack[LocationStack.Depth-2]);
#line 3158 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                nine.Insert(0,tttt);
#line 3159 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			    var fe = new foreach_stmt(id, new no_type_foreach(), ValueStack[ValueStack.Depth-4].ex, nine, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
#line 3160 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			    fe.ext = ValueStack[ValueStack.Depth-7].stn as ident_list;
#line 3161 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			    CurrentSemanticValue.stn = fe;
#line 3162 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}
#line 3163 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 571: // for_stmt -> tkFor, optional_var, identifier, optional_type_specification, 
                //             tkAssign, expr_l1, for_cycle_type, expr_l1, optional_tk_do, 
                //             unlabelled_stmt
#line 3172 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3173 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-9].ob, ValueStack[ValueStack.Depth-8].id, ValueStack[ValueStack.Depth-7].td, ValueStack[ValueStack.Depth-5].ex, (for_cycle_type)ValueStack[ValueStack.Depth-4].ob, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
#line 3174 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 572: // for_stmt -> tkFor, optional_var, identifier, optional_type_specification, 
                //             tkAssign, expr_l1, for_cycle_type, expr_l1, tkStep, expr_l1, tkDo, 
                //             unlabelled_stmt
#line 3176 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3177 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-11].ob, ValueStack[ValueStack.Depth-10].id, ValueStack[ValueStack.Depth-9].td, ValueStack[ValueStack.Depth-7].ex, (for_cycle_type)ValueStack[ValueStack.Depth-6].ob, ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
#line 3178 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 573: // with_stmt -> tkWith, expr_list, tkDo, unlabelled_stmt
#line 3183 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3184 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new with_statement(ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].stn as expression_list, CurrentLocationSpan); 
#line 3185 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 574: // inherited_message -> tkInherited
#line 3190 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3191 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new inherited_message();  
#line 3192 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 3193 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 575: // try_stmt -> tkTry, stmt_list, try_handler
#line 3198 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3199 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new try_stmt(ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].stn as try_handler, CurrentLocationSpan); 
#line 3200 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
#line 3201 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			ValueStack[ValueStack.Depth-2].stn.source_context = LocationStack[LocationStack.Depth-3].Merge(LocationStack[LocationStack.Depth-2]);
#line 3202 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 576: // try_handler -> tkFinally, stmt_list, tkEnd
#line 3207 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3208 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new try_handler_finally(ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan);
#line 3209 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
#line 3210 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(ValueStack[ValueStack.Depth-2].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
#line 3211 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 577: // try_handler -> tkExcept, exception_block, tkEnd
#line 3213 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3214 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new try_handler_except((exception_block)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan);  
#line 3215 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if ((ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list != null)
#line 3216 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			{
#line 3217 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				(ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list.source_context = CurrentLocationSpan;
#line 3218 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				(ValueStack[ValueStack.Depth-2].stn as exception_block).source_context = CurrentLocationSpan;
#line 3219 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}
#line 3220 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 578: // exception_block -> exception_handler_list, exception_block_else_branch
#line 3225 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3226 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-2].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
#line 3227 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 579: // exception_block -> exception_handler_list, tkSemiColon, 
                //                    exception_block_else_branch
#line 3229 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3230 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-3].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
#line 3231 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 580: // exception_block -> stmt_list
#line 3233 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3234 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new exception_block(ValueStack[ValueStack.Depth-1].stn as statement_list, null, null, LocationStack[LocationStack.Depth-1]);
#line 3235 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 581: // exception_handler_list -> exception_handler
#line 3240 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3241 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new exception_handler_list(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
#line 3242 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 582: // exception_handler_list -> exception_handler_list, tkSemiColon, 
                //                           exception_handler
#line 3244 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3245 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as exception_handler_list).Add(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
#line 3246 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 583: // exception_block_else_branch -> /* empty */
#line 3251 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 584: // exception_block_else_branch -> tkElse, stmt_list
#line 3253 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 585: // exception_handler -> tkOn, exception_identifier, tkDo, unlabelled_stmt
#line 3258 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3259 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new exception_handler((ValueStack[ValueStack.Depth-3].stn as exception_ident).variable, (ValueStack[ValueStack.Depth-3].stn as exception_ident).type_name, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 3260 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 586: // exception_identifier -> exception_class_type_identifier
#line 3265 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3266 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new exception_ident(null, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
#line 3267 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 587: // exception_identifier -> exception_variable, tkColon, 
                //                         exception_class_type_identifier
#line 3269 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3270 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new exception_ident(ValueStack[ValueStack.Depth-3].id, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
#line 3271 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 588: // exception_class_type_identifier -> simple_type_identifier
#line 3276 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 589: // exception_variable -> identifier
#line 3281 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 590: // raise_stmt -> tkRaise
#line 3286 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3287 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new raise_stmt(); 
#line 3288 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 3289 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 591: // raise_stmt -> tkRaise, expr
#line 3291 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3292 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new raise_stmt(ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan);  
#line 3293 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 592: // expr_list -> expr_with_func_decl_lambda
#line 3298 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3299 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 3300 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 593: // expr_list -> expr_list, tkComma, expr_with_func_decl_lambda
#line 3302 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3303 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 3304 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 594: // expr_list_func_param -> expr_with_func_decl_lambda_ass
#line 3309 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3310 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 3311 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 595: // expr_list_func_param -> expr_list_func_param, tkComma, 
                //                         expr_with_func_decl_lambda_ass
#line 3313 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3314 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 3315 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 596: // expr_as_stmt -> allowable_expr_as_stmt
#line 3320 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3321 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new expression_as_statement(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
#line 3322 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 597: // allowable_expr_as_stmt -> new_expr
#line 3327 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 598: // expr_with_func_decl_lambda -> expr
#line 3332 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 599: // expr_with_func_decl_lambda -> func_decl_lambda
#line 3334 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 600: // expr_with_func_decl_lambda -> tkInherited
#line 3336 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new inherited_ident("", CurrentLocationSpan); }
        break;
      case 601: // expr_with_func_decl_lambda_ass -> expr
#line 3341 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 602: // expr_with_func_decl_lambda_ass -> identifier, tkAssign, expr_l1
#line 3343 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); }
        break;
      case 603: // expr_with_func_decl_lambda_ass -> func_decl_lambda
#line 3345 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 604: // expr_with_func_decl_lambda_ass -> tkInherited
#line 3347 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new inherited_ident("", CurrentLocationSpan); }
        break;
      case 605: // expr -> expr_l1
#line 3352 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 606: // expr -> format_expr
#line 3354 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 607: // expr -> expr, tkTo, expr_l1
#line 3356 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new to_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 608: // expr_l1 -> expr_dq
#line 3361 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 609: // expr_l1 -> question_expr
#line 3363 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 610: // expr_l1 -> new_question_expr
#line 3365 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 611: // expr_l1_for_question_expr -> expr_dq
#line 3372 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 612: // expr_l1_for_question_expr -> question_expr
#line 3374 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 613: // expr_l1_for_new_question_expr -> expr_dq
#line 3379 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 614: // expr_l1_for_new_question_expr -> new_question_expr
#line 3381 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 615: // expr_l1_func_decl_lambda -> expr_l1
#line 3386 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 616: // expr_l1_func_decl_lambda -> func_decl_lambda
#line 3388 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 617: // expr_l1_for_lambda -> expr_dq
#line 3393 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 618: // expr_l1_for_lambda -> question_expr
#line 3395 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 619: // expr_l1_for_lambda -> func_decl_lambda
#line 3397 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 620: // expr_dq -> relop_expr
#line 3402 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 621: // expr_dq -> expr_dq, tkDoubleQuestion, relop_expr
#line 3404 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 622: // sizeof_expr -> tkSizeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
#line 3409 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3410 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new sizeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, null, CurrentLocationSpan);  
#line 3411 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 623: // typeof_expr -> tkTypeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
#line 3416 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3417 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
#line 3418 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 624: // typeof_expr -> tkTypeOf, tkRoundOpen, empty_template_type_reference, 
                //                tkRoundClose
#line 3421 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3422 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
#line 3423 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 625: // question_expr -> expr_l1_for_question_expr, tkQuestion, 
                //                  expr_l1_for_question_expr, tkColon, 
                //                  expr_l1_for_question_expr
#line 3428 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3429 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (ValueStack[ValueStack.Depth-3].ex is nil_const && ValueStack[ValueStack.Depth-1].ex is nil_const)
#line 3430 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            	parserTools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",LocationStack[LocationStack.Depth-3]);
#line 3431 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
#line 3432 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 626: // new_question_expr -> tkIf, expr_l1_for_new_question_expr, tkThen, 
                //                      expr_l1_for_new_question_expr, tkElse, 
                //                      expr_l1_for_new_question_expr
#line 3437 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3438 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	if (parserTools.buildTreeForFormatter)
#line 3439 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	{
#line 3440 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		CurrentSemanticValue.ex = new if_expr_new(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 3441 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	}
#line 3442 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	else
#line 3443 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	{
#line 3444 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            	if (ValueStack[ValueStack.Depth-3].ex is nil_const && ValueStack[ValueStack.Depth-1].ex is nil_const)
#line 3445 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            		parserTools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",LocationStack[LocationStack.Depth-3]);
#line 3446 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 3447 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}			
#line 3448 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 627: // empty_template_type_reference -> simple_type_identifier, 
                //                                  template_type_empty_params
#line 3454 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3455 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
#line 3456 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 628: // empty_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                  template_type_empty_params
#line 3458 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3459 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
#line 3460 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 629: // simple_or_template_type_reference -> simple_type_identifier
#line 3465 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3466 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
#line 3467 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 630: // simple_or_template_type_reference -> simple_type_identifier, 
                //                                      template_type_params
#line 3469 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3470 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
#line 3471 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 631: // simple_or_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                      template_type_params
#line 3473 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3474 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
#line 3475 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 632: // simple_or_template_or_question_type_reference -> 
                //                                                  simple_or_template_type_reference
#line 3480 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3481 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
#line 3482 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 633: // simple_or_template_or_question_type_reference -> simple_type_question
#line 3484 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3485 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
#line 3486 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 634: // optional_array_initializer -> tkRoundOpen, typed_const_list, tkRoundClose
#line 3491 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3492 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new array_const((expression_list)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan); 
#line 3493 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 636: // new_expr -> tkNew, simple_or_template_type_reference, 
                //             optional_expr_list_with_bracket
#line 3499 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3500 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new new_expr(ValueStack[ValueStack.Depth-2].td, ValueStack[ValueStack.Depth-1].stn as expression_list, false, null, CurrentLocationSpan);
#line 3501 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 637: // new_expr -> tkNew, simple_or_template_type_reference, tkSquareOpen, 
                //             optional_expr_list, tkSquareClose, optional_array_initializer
#line 3503 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3504 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	var el = ValueStack[ValueStack.Depth-3].stn as expression_list;
#line 3505 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	if (el == null)
#line 3506 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	{
#line 3507 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		var cnt = 0;
#line 3508 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		var ac = ValueStack[ValueStack.Depth-1].stn as array_const;
#line 3509 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		if (ac != null && ac.elements != null)
#line 3510 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	        	    cnt = ac.elements.Count;
#line 3511 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	        	else parserTools.AddErrorFromResource("WITHOUT_INIT_AND_SIZE",LocationStack[LocationStack.Depth-2]);
#line 3512 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		el = new expression_list(new int32_const(cnt),LocationStack[LocationStack.Depth-6]);
#line 3513 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	}	
#line 3514 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new new_expr(ValueStack[ValueStack.Depth-5].td, el, true, ValueStack[ValueStack.Depth-1].stn as array_const, CurrentLocationSpan);
#line 3515 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 638: // new_expr -> tkNew, tkClass, tkRoundOpen, list_fields_in_unnamed_object, 
                //             tkRoundClose
#line 3517 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3518 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        // sugared node	
        	var l = ValueStack[ValueStack.Depth-2].ob as name_assign_expr_list;
#line 3519 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	var exprs = l.name_expr.Select(x=>x.expr.Clone() as expression).ToList();
#line 3520 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	var typename = "AnonymousType#"+Guid();
#line 3521 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	var type = new named_type_reference(typename,LocationStack[LocationStack.Depth-5]);
#line 3522 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	
#line 3523 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			// node new_expr - for code generation of new node
			var ne = new new_expr(type, new expression_list(exprs), CurrentLocationSpan);
#line 3524 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			// node unnamed_type_object - for formatting and code generation (new node and Anonymous class)
			CurrentSemanticValue.ex = new unnamed_type_object(l, true, ne, CurrentLocationSpan);
#line 3525 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 639: // field_in_unnamed_object -> identifier, tkAssign, expr_l1
#line 3533 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3534 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    if (ValueStack[ValueStack.Depth-1].ex is nil_const)
#line 3535 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("NIL_IN_UNNAMED_OBJECT",CurrentLocationSpan);		    
#line 3536 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
#line 3537 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 640: // field_in_unnamed_object -> expr_l1
#line 3539 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3540 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			ident name = null;
#line 3541 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var id = ValueStack[ValueStack.Depth-1].ex as ident;
#line 3542 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			dot_node dot;
#line 3543 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (id != null)
#line 3544 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				name = id;
#line 3545 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else 
#line 3546 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            {
#line 3547 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            	dot = ValueStack[ValueStack.Depth-1].ex as dot_node;
#line 3548 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            	if (dot != null)
#line 3549 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            	{
#line 3550 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            		name = dot.right as ident;
#line 3551 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            	}            	
#line 3552 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            } 
#line 3553 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (name == null)
#line 3554 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.errors.Add(new bad_anon_type(parserTools.currentFileName, LocationStack[LocationStack.Depth-1], null));	
#line 3555 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = new name_assign_expr(name,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
#line 3556 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 641: // list_fields_in_unnamed_object -> field_in_unnamed_object
#line 3561 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3562 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var l = new name_assign_expr_list();
#line 3563 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = l.Add(ValueStack[ValueStack.Depth-1].ob as name_assign_expr);
#line 3564 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 642: // list_fields_in_unnamed_object -> list_fields_in_unnamed_object, tkComma, 
                //                                  field_in_unnamed_object
#line 3566 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3567 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var nel = ValueStack[ValueStack.Depth-3].ob as name_assign_expr_list;
#line 3568 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var ss = nel.name_expr.Select(ne=>ne.name.name).FirstOrDefault(x=>string.Compare(x,(ValueStack[ValueStack.Depth-1].ob as name_assign_expr).name.name,true)==0);
#line 3569 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (ss != null)
#line 3570 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            	parserTools.errors.Add(new anon_type_duplicate_name(parserTools.currentFileName, LocationStack[LocationStack.Depth-1], null));
#line 3571 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			nel.Add(ValueStack[ValueStack.Depth-1].ob as name_assign_expr);
#line 3572 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
#line 3573 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 643: // optional_expr_list_with_bracket -> /* empty */
#line 3585 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 644: // optional_expr_list_with_bracket -> tkRoundOpen, optional_expr_list, 
                //                                    tkRoundClose
#line 3587 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 645: // relop_expr -> simple_expr
#line 3592 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 646: // relop_expr -> relop_expr, relop, simple_expr
#line 3594 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3595 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
#line 3596 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
#line 3597 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	else	
#line 3598 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
#line 3599 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 647: // relop_expr -> relop_expr, relop, new_question_expr
#line 3601 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3602 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
#line 3603 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
#line 3604 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	else	
#line 3605 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
#line 3606 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 648: // relop_expr -> is_type_expr, tkRoundOpen, pattern_out_param_list, tkRoundClose
#line 3608 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3609 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            var isTypeCheck = ValueStack[ValueStack.Depth-4].ex as typecast_node;
#line 3610 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            var deconstructorPattern = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, isTypeCheck.type_def, null, CurrentLocationSpan); 
#line 3611 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ex = new is_pattern_expr(isTypeCheck.expr, deconstructorPattern, CurrentLocationSpan);
#line 3612 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 649: // pattern -> simple_or_template_type_reference, tkRoundOpen, 
                //            pattern_out_param_list, tkRoundClose
#line 3627 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3628 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
#line 3629 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 650: // pattern_optional_var -> simple_or_template_type_reference, tkRoundOpen, 
                //                         pattern_out_param_list_optional_var, tkRoundClose
#line 3634 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3635 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
#line 3636 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 651: // deconstruction_or_const_pattern -> simple_or_template_type_reference, 
                //                                    tkRoundOpen, 
                //                                    pattern_out_param_list_optional_var, 
                //                                    tkRoundClose
#line 3641 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3642 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
#line 3643 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 652: // deconstruction_or_const_pattern -> const_pattern_expr_list
#line 3645 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3646 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    CurrentSemanticValue.stn = new const_pattern(ValueStack[ValueStack.Depth-1].ob as List<syntax_tree_node>, CurrentLocationSpan); 
#line 3647 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 653: // const_pattern_expr_list -> const_pattern_expression
#line 3652 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3653 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = new List<syntax_tree_node>(); 
#line 3654 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.ob as List<syntax_tree_node>).Add(ValueStack[ValueStack.Depth-1].stn);
#line 3655 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 654: // const_pattern_expr_list -> const_pattern_expr_list, tkComma, 
                //                            const_pattern_expression
#line 3657 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3658 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var list = ValueStack[ValueStack.Depth-3].ob as List<syntax_tree_node>;
#line 3659 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            list.Add(ValueStack[ValueStack.Depth-1].stn);
#line 3660 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ob = list;
#line 3661 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 655: // const_pattern_expression -> literal_or_number
#line 3666 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 656: // const_pattern_expression -> simple_or_template_type_reference
#line 3668 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 657: // const_pattern_expression -> tkNil
#line 3670 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3671 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new nil_const();  
#line 3672 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 3673 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 658: // const_pattern_expression -> sizeof_expr
#line 3675 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 659: // const_pattern_expression -> typeof_expr
#line 3677 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 660: // collection_pattern -> tkSquareOpen, collection_pattern_expr_list, tkSquareClose
#line 3682 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3683 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new collection_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
#line 3684 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 661: // collection_pattern_expr_list -> collection_pattern_list_item
#line 3689 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3690 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = new List<pattern_parameter>();
#line 3691 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
#line 3692 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 662: // collection_pattern_expr_list -> collection_pattern_expr_list, tkComma, 
                //                                 collection_pattern_list_item
#line 3694 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3695 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
#line 3696 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
#line 3697 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ob = list;
#line 3698 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 663: // collection_pattern_list_item -> literal_or_number
#line 3703 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3704 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 3705 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 664: // collection_pattern_list_item -> collection_pattern_var_item
#line 3707 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3708 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 3709 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 665: // collection_pattern_list_item -> tkUnderscore
#line 3711 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3712 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new collection_pattern_wild_card(CurrentLocationSpan);
#line 3713 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 666: // collection_pattern_list_item -> pattern_optional_var
#line 3719 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3720 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
#line 3721 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 667: // collection_pattern_list_item -> collection_pattern
#line 3723 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3724 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
#line 3725 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 668: // collection_pattern_list_item -> tuple_pattern
#line 3727 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3728 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
#line 3729 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 669: // collection_pattern_list_item -> tkDotDot
#line 3731 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3732 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new collection_pattern_gap_parameter(CurrentLocationSpan);
#line 3733 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 670: // collection_pattern_var_item -> tkVar, identifier
#line 3738 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3739 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new collection_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
#line 3740 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 671: // tuple_pattern -> tkRoundOpen, tuple_pattern_item_list, tkRoundClose
#line 3745 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3746 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if ((ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>).Count>6) 
#line 3747 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",CurrentLocationSpan);
#line 3748 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new tuple_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
#line 3749 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 672: // tuple_pattern_item -> tkUnderscore
#line 3754 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3755 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new tuple_pattern_wild_card(CurrentLocationSpan); 
#line 3756 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 673: // tuple_pattern_item -> literal_or_number
#line 3758 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3759 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 3760 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 674: // tuple_pattern_item -> sign, literal_or_number
#line 3762 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3763 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
#line 3764 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 675: // tuple_pattern_item -> tkVar, identifier
#line 3766 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3767 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new tuple_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
#line 3768 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 676: // tuple_pattern_item -> pattern_optional_var
#line 3770 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3771 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
#line 3772 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 677: // tuple_pattern_item -> collection_pattern
#line 3774 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3775 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
#line 3776 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 678: // tuple_pattern_item -> tuple_pattern
#line 3778 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3779 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
#line 3780 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 679: // tuple_pattern_item_list -> tuple_pattern_item
#line 3785 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3786 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = new List<pattern_parameter>();
#line 3787 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
#line 3788 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 680: // tuple_pattern_item_list -> tuple_pattern_item_list, tkComma, tuple_pattern_item
#line 3790 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3791 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
#line 3792 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
#line 3793 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ob = list;
#line 3794 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 681: // pattern_out_param_list_optional_var -> pattern_out_param_optional_var
#line 3799 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3800 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ob = new List<pattern_parameter>();
#line 3801 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
#line 3802 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 682: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkSemiColon, 
                //                                        pattern_out_param_optional_var
#line 3804 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3805 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
#line 3806 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
#line 3807 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ob = list;
#line 3808 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 683: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkComma, 
                //                                        pattern_out_param_optional_var
#line 3810 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3811 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
#line 3812 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
#line 3813 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ob = list;
#line 3814 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 684: // pattern_out_param_list -> pattern_out_param
#line 3819 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3820 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ob = new List<pattern_parameter>();
#line 3821 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
#line 3822 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 685: // pattern_out_param_list -> pattern_out_param_list, tkSemiColon, 
                //                           pattern_out_param
#line 3824 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3825 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
#line 3826 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
#line 3827 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ob = list;
#line 3828 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 686: // pattern_out_param_list -> pattern_out_param_list, tkComma, pattern_out_param
#line 3830 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3831 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
#line 3832 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
#line 3833 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ob = list;
#line 3834 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 687: // pattern_out_param -> tkUnderscore
#line 3839 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3840 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
#line 3841 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 688: // pattern_out_param -> literal_or_number
#line 3843 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3844 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 3845 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 689: // pattern_out_param -> tkVar, identifier, tkColon, type_ref
#line 3847 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3848 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
#line 3849 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 690: // pattern_out_param -> tkVar, identifier
#line 3851 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3852 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
#line 3853 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 691: // pattern_out_param -> pattern
#line 3855 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3856 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
#line 3857 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 692: // pattern_out_param -> collection_pattern
#line 3859 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3860 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
#line 3861 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 693: // pattern_out_param -> tuple_pattern
#line 3863 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3864 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
#line 3865 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 694: // pattern_out_param_optional_var -> tkUnderscore
#line 3870 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3871 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
#line 3872 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 695: // pattern_out_param_optional_var -> literal_or_number
#line 3874 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3875 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 3876 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 696: // pattern_out_param_optional_var -> sign, literal_or_number
#line 3878 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3879 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
#line 3880 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 697: // pattern_out_param_optional_var -> identifier, tkColon, type_ref
#line 3882 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3883 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, false, CurrentLocationSpan);
#line 3884 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 698: // pattern_out_param_optional_var -> identifier
#line 3886 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3887 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, false, CurrentLocationSpan);
#line 3888 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 699: // pattern_out_param_optional_var -> tkVar, identifier, tkColon, type_ref
#line 3890 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3891 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
#line 3892 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 700: // pattern_out_param_optional_var -> tkVar, identifier
#line 3894 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3895 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
#line 3896 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 701: // pattern_out_param_optional_var -> pattern_optional_var
#line 3898 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3899 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
#line 3900 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 702: // pattern_out_param_optional_var -> collection_pattern
#line 3902 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3903 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
#line 3904 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 703: // pattern_out_param_optional_var -> tuple_pattern
#line 3906 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3907 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
#line 3908 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 704: // simple_expr_or_nothing -> simple_expr
#line 3913 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3914 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
#line 3915 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	}
        break;
      case 705: // simple_expr_or_nothing -> /* empty */
#line 3917 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3918 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		CurrentSemanticValue.ex = null;
#line 3919 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	}
        break;
      case 706: // const_expr_or_nothing -> const_expr
#line 3924 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3925 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
#line 3926 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	}
        break;
      case 707: // const_expr_or_nothing -> /* empty */
#line 3928 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3929 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		CurrentSemanticValue.ex = null;
#line 3930 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	}
        break;
      case 708: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing
#line 3961 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 3962 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
#line 3963 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 709: // format_expr -> tkColon, simple_expr_or_nothing
#line 3965 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3966 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
#line 3967 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 710: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing, tkColon, 
                //                simple_expr
#line 3969 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3970 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 3971 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 711: // format_expr -> tkColon, simple_expr_or_nothing, tkColon, simple_expr
#line 3973 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3974 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 3975 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 712: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing
#line 3980 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3981 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
#line 3982 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 713: // format_const_expr -> tkColon, const_expr_or_nothing
#line 3984 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3985 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
#line 3986 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 714: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing, tkColon, 
                //                      const_expr
#line 3988 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3989 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 3990 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 715: // format_const_expr -> tkColon, const_expr_or_nothing, tkColon, const_expr
#line 3992 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 3993 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 3994 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 716: // relop -> tkEqual
#line 4000 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 717: // relop -> tkNotEqual
#line 4002 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 718: // relop -> tkLower
#line 4004 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 719: // relop -> tkGreater
#line 4006 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 720: // relop -> tkLowerEqual
#line 4008 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 721: // relop -> tkGreaterEqual
#line 4010 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 722: // relop -> tkIn
#line 4012 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 723: // relop -> tkNot, tkIn
#line 4014 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4015 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (parserTools.buildTreeForFormatter)
#line 4016 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op;
#line 4017 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else
#line 4018 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			{
#line 4019 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op;	
#line 4020 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.op.type = Operators.NotIn;
#line 4021 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}				
#line 4022 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 724: // simple_expr -> term1
#line 4027 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 725: // simple_expr -> simple_expr, tkDotDot, term1
#line 4029 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4030 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		if (parserTools.buildTreeForFormatter)
#line 4031 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
#line 4032 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		else 
#line 4033 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new diapason_expr_new(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); 
#line 4034 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	}
        break;
      case 726: // term1 -> term
#line 4039 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 727: // term1 -> term1, addop, term
#line 4041 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4042 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
#line 4043 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 728: // term1 -> term1, addop, new_question_expr
#line 4045 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4046 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
#line 4047 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 729: // addop -> tkPlus
#line 4052 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 730: // addop -> tkMinus
#line 4054 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 731: // addop -> tkOr
#line 4056 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 732: // addop -> tkXor
#line 4058 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 733: // addop -> tkCSharpStyleOr
#line 4060 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 734: // typecast_op -> tkAs
#line 4065 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4066 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = op_typecast.as_op; 
#line 4067 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 735: // typecast_op -> tkIs
#line 4069 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4070 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = op_typecast.is_op; 
#line 4071 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 736: // as_is_expr -> is_type_expr
#line 4076 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 737: // as_is_expr -> as_expr
#line 4078 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 738: // as_expr -> term, tkAs, simple_or_template_type_reference
#line 4083 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4084 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
#line 4085 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 739: // as_expr -> term, tkAs, array_type
#line 4087 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4088 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
#line 4089 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	    }
        break;
      case 740: // is_type_expr -> term, tkIs, simple_or_template_type_reference
#line 4094 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4095 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
#line 4096 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 741: // is_type_expr -> term, tkIs, array_type
#line 4098 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4099 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
#line 4100 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	    }
        break;
      case 742: // power_expr -> factor_without_unary_op, tkStarStar, factor
#line 4105 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 743: // power_expr -> factor_without_unary_op, tkStarStar, power_expr
#line 4107 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 744: // power_expr -> sign, power_expr
#line 4109 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 745: // term -> factor
#line 4114 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 746: // term -> new_expr
#line 4116 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 747: // term -> power_expr
#line 4118 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 748: // term -> term, mulop, factor
#line 4120 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 749: // term -> term, mulop, power_expr
#line 4122 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 750: // term -> term, mulop, new_question_expr
#line 4124 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 751: // term -> as_is_expr
#line 4126 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 752: // mulop -> tkStar
#line 4131 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 753: // mulop -> tkSlash
#line 4133 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 754: // mulop -> tkDiv
#line 4135 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 755: // mulop -> tkMod
#line 4137 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 756: // mulop -> tkShl
#line 4139 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 757: // mulop -> tkShr
#line 4141 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 758: // mulop -> tkAnd
#line 4143 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 759: // default_expr -> tkDefault, tkRoundOpen, 
                //                 simple_or_template_or_question_type_reference, tkRoundClose
#line 4148 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4149 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new default_operator(ValueStack[ValueStack.Depth-2].td as named_type_reference, CurrentLocationSpan);  
#line 4150 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 760: // tuple -> tkRoundOpen, expr_l1_or_unpacked, tkComma, expr_l1_or_unpacked_list, 
                //          lambda_type_ref, optional_full_lambda_fp_list, tkRoundClose
#line 4155 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4156 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (ValueStack[ValueStack.Depth-6].ex is unpacked_list_of_ident_or_list) 
#line 4157 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("EXPRESSION_EXPECTED",LocationStack[LocationStack.Depth-6]);
#line 4158 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			foreach (var ex in (ValueStack[ValueStack.Depth-4].stn as expression_list).expressions)
#line 4159 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (ex is unpacked_list_of_ident_or_list)
#line 4160 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					parserTools.AddErrorFromResource("EXPRESSION_EXPECTED",ex.source_context);
#line 4161 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (!(ValueStack[ValueStack.Depth-3].td is lambda_inferred_type)) 
#line 4162 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("BAD_TUPLE",LocationStack[LocationStack.Depth-3]);
#line 4163 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (ValueStack[ValueStack.Depth-2].stn != null) 
#line 4164 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("BAD_TUPLE",LocationStack[LocationStack.Depth-2]);
#line 4165 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"

#line 4166 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if ((ValueStack[ValueStack.Depth-4].stn as expression_list).Count>6) 
#line 4167 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",CurrentLocationSpan);
#line 4168 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            (ValueStack[ValueStack.Depth-4].stn as expression_list).Insert(0,ValueStack[ValueStack.Depth-6].ex);
#line 4169 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new tuple_node(ValueStack[ValueStack.Depth-4].stn as expression_list,CurrentLocationSpan);
#line 4170 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 761: // factor_without_unary_op -> literal_or_number
#line 4175 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 762: // factor_without_unary_op -> var_reference
#line 4177 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 763: // factor -> tkNil
#line 4184 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4185 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new nil_const();  
#line 4186 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
#line 4187 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 764: // factor -> literal_or_number
#line 4189 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 765: // factor -> default_expr
#line 4191 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 766: // factor -> pascal_set_const
#line 4193 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4194 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
#line 4195 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 767: // factor -> factor, tkPoint, identifier_keyword_operatorname
#line 4197 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4198 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
#line 4199 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 768: // factor -> factor, tkRoundOpen, optional_expr_list_func_param, tkRoundClose
#line 4201 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4202 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
#line 4203 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 769: // factor -> tkNot, factor
#line 4205 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4206 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
#line 4207 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 770: // factor -> sign, factor
#line 4209 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4210 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (ValueStack[ValueStack.Depth-2].op.type == Operators.Minus)
#line 4211 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			{
#line 4212 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			    var i64 = ValueStack[ValueStack.Depth-1].ex as int64_const;
#line 4213 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (i64 != null && i64.val == (Int64)Int32.MaxValue + 1)
#line 4214 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				{
#line 4215 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					CurrentSemanticValue.ex = new int32_const(Int32.MinValue,CurrentLocationSpan);
#line 4216 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					break;
#line 4217 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				}
#line 4218 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var ui64 = ValueStack[ValueStack.Depth-1].ex as uint64_const;
#line 4219 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (ui64 != null && ui64.val == (UInt64)Int64.MaxValue + 1)
#line 4220 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				{
#line 4221 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					CurrentSemanticValue.ex = new int64_const(Int64.MinValue,CurrentLocationSpan);
#line 4222 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					break;
#line 4223 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				}
#line 4224 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (ui64 != null && ui64.val > (UInt64)Int64.MaxValue + 1)
#line 4225 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				{
#line 4226 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					parserTools.AddErrorFromResource("BAD_INT2",CurrentLocationSpan);
#line 4227 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					break;
#line 4228 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				}
#line 4229 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			    // можно сдела�?�? в�?�?исление конс�?ан�?�? с вмон�?и�?ованн�?м мин�?сом
			}
#line 4230 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan);
#line 4231 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 771: // factor -> tkDeref, factor
#line 4234 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4235 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.ex = new index(ValueStack[ValueStack.Depth-1].ex, true, CurrentLocationSpan);
#line 4236 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 772: // factor -> var_reference
#line 4238 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 773: // factor -> tuple
#line 4240 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 774: // literal_or_number -> literal
#line 4245 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 775: // literal_or_number -> unsigned_number
#line 4247 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 776: // var_question_point -> variable, tkQuestionPoint, variable
#line 4257 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4258 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
#line 4259 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	}
        break;
      case 777: // var_question_point -> variable, tkQuestionPoint, var_question_point
#line 4261 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4262 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
#line 4263 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	}
        break;
      case 778: // var_reference -> var_address, variable
#line 4268 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4269 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = NewVarReference(ValueStack[ValueStack.Depth-2].stn as get_address, ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);
#line 4270 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 779: // var_reference -> variable
#line 4272 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 780: // var_reference -> var_question_point
#line 4274 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 781: // var_reference -> tkRoundOpen, tkVar, identifier, tkAssign, expr_dq, 
                //                  tkRoundClose
#line 4276 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new let_var_expr(ValueStack[ValueStack.Depth-4].id,ValueStack[ValueStack.Depth-2].ex,CurrentLocationSpan); }
        break;
      case 782: // var_address -> tkAddressOf
#line 4281 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4282 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = NewVarAddress(CurrentLocationSpan);
#line 4283 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 783: // var_address -> var_address, tkAddressOf
#line 4285 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4286 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = NewVarAddress(ValueStack[ValueStack.Depth-2].stn as get_address, CurrentLocationSpan);
#line 4287 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 784: // attribute_variable -> simple_type_identifier, optional_expr_list_with_bracket
#line 4292 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4293 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
#line 4294 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 785: // attribute_variable -> template_type, optional_expr_list_with_bracket
#line 4296 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4297 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
#line 4298 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 786: // dotted_identifier -> identifier
#line 4302 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 787: // dotted_identifier -> dotted_identifier, tkPoint, identifier_or_keyword
#line 4304 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4305 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (ValueStack[ValueStack.Depth-3].ex is index)
#line 4306 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
#line 4307 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
#line 4308 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 788: // variable_as_type -> dotted_identifier
#line 4312 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;}
        break;
      case 789: // variable_as_type -> dotted_identifier, template_type_params
#line 4314 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-2].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);   }
        break;
      case 790: // variable_or_literal_or_number -> variable
#line 4319 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 791: // variable_or_literal_or_number -> literal_or_number
#line 4321 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 792: // var_with_init_for_expr_with_let -> tkVar, identifier, tkAssign, expr, 
                //                                    tkSemiColon
#line 4326 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4327 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-4].id as addressed_value, ValueStack[ValueStack.Depth-2].ex, Operators.Assignment, CurrentLocationSpan);
#line 4328 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 793: // var_with_init_for_expr_with_let_list -> var_with_init_for_expr_with_let
#line 4333 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4334 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 4335 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 794: // var_with_init_for_expr_with_let_list -> var_with_init_for_expr_with_let_list, 
                //                                         var_with_init_for_expr_with_let
#line 4337 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4338 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			ValueStack[ValueStack.Depth-2].stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 4339 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
#line 4340 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 795: // proc_func_call -> variable, tkRoundOpen, optional_expr_list_func_param, 
                //                   tkRoundClose
#line 4345 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4346 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (ValueStack[ValueStack.Depth-4].ex is index)
#line 4347 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-4], "^");
#line 4348 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
#line 4349 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 796: // variable -> identifier
#line 4354 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 797: // variable -> operator_name_ident
#line 4356 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 798: // variable -> tkInherited, identifier
#line 4358 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4359 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
#line 4360 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 799: // variable -> tkRoundOpen, expr, tkRoundClose
#line 4362 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4363 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    if (!parserTools.buildTreeForFormatter) 
#line 4364 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            {
#line 4365 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                ValueStack[ValueStack.Depth-2].ex.source_context = CurrentLocationSpan;
#line 4366 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
#line 4367 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            } 
#line 4368 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
#line 4369 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 800: // variable -> tkRoundOpen, var_with_init_for_expr_with_let_list, expr, 
                //             tkRoundClose
#line 4371 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4372 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    if (!parserTools.buildTreeForFormatter) 
#line 4373 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            {
#line 4374 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                ValueStack[ValueStack.Depth-2].ex.source_context = CurrentLocationSpan;
#line 4375 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
#line 4376 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            } 
#line 4377 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else CurrentSemanticValue.ex = new expression_with_let(ValueStack[ValueStack.Depth-3].stn as statement_list, ValueStack[ValueStack.Depth-3].stn as expression, CurrentLocationSpan);
#line 4378 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 801: // variable -> sizeof_expr
#line 4380 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 802: // variable -> typeof_expr
#line 4382 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 803: // variable -> literal_or_number, tkPoint, identifier_or_keyword
#line 4384 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4385 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (ValueStack[ValueStack.Depth-3].ex is index)
#line 4386 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");		
#line 4387 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
#line 4388 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 804: // variable -> variable_or_literal_or_number, tkSquareOpen, expr_list, 
                //             tkSquareClose
#line 4390 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4391 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	//$$ = NewIndexerOrSlice($1 as addressed_value,$3 as expression_list,@$);
        	var el = ValueStack[ValueStack.Depth-2].stn as expression_list; // SSM 10/03/16
        	if (el.Count==1 && el.expressions[0] is format_expr) 
#line 4392 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	{
#line 4393 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		var fe = el.expressions[0] as format_expr;
#line 4394 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                if (!parserTools.buildTreeForFormatter)
#line 4395 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                {
#line 4396 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    if (fe.expr == null)
#line 4397 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                        fe.expr = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
#line 4398 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    if (fe.format1 == null)
#line 4399 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                        fe.format1 = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
#line 4400 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                }
#line 4401 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        		CurrentSemanticValue.ex = new slice_expr(ValueStack[ValueStack.Depth-4].ex as addressed_value,fe.expr,fe.format1,fe.format2,CurrentLocationSpan);
#line 4402 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}   
#line 4403 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			// многоме�?н�?е с�?ез�?
            else if (el.expressions.Any(e => e is format_expr))
#line 4404 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            {
#line 4405 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            	if (el.expressions.Count > 4)
#line 4406 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            		parserTools.AddErrorFromResource("SLICES_OF MULTIDIMENSIONAL_ARRAYS_ALLOW_ONLY_FOR_RANK_LT_5",CurrentLocationSpan); // С�?ез�? многоме�?н�?�? массивов �?аз�?е�?ен�? �?ол�?ко для массивов �?азме�?нос�?и < 5  
                var ll = new List<Tuple<expression, expression, expression>>();
#line 4407 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                foreach (var ex in el.expressions)
#line 4408 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                {
#line 4409 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    if (ex is format_expr fe)
#line 4410 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    {
#line 4411 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                        if (fe.expr == null)
#line 4412 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                            fe.expr = new int32_const(int.MaxValue, fe.source_context);
#line 4413 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                        if (fe.format1 == null)
#line 4414 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                            fe.format1 = new int32_const(int.MaxValue, fe.source_context);
#line 4415 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                        if (fe.format2 == null)
#line 4416 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                            fe.format2 = new int32_const(1, fe.source_context);
#line 4417 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                        ll.Add(Tuple.Create(fe.expr, fe.format1, fe.format2));
#line 4418 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    }
#line 4419 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    else
#line 4420 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    {
#line 4421 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    	ll.Add(Tuple.Create(ex, (expression)new int32_const(0, ex.source_context), (expression)new int32_const(int.MaxValue, ex.source_context))); // скаля�?ное зна�?ение вмес�?о с�?еза
                    }
#line 4422 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				}
#line 4423 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var sle = new slice_expr(ValueStack[ValueStack.Depth-4].ex as addressed_value,null,null,null,CurrentLocationSpan);
#line 4424 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				sle.slices = ll;
#line 4425 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.ex = sle;
#line 4426 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            }
#line 4427 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else CurrentSemanticValue.ex = new indexer(ValueStack[ValueStack.Depth-4].ex as addressed_value, el, CurrentLocationSpan);
#line 4428 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 805: // variable -> variable_or_literal_or_number, tkQuestionSquareOpen, format_expr, 
                //             tkSquareClose
#line 4435 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4436 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	var fe = ValueStack[ValueStack.Depth-2].ex as format_expr; // SSM 9/01/17
            if (!parserTools.buildTreeForFormatter)
#line 4437 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            {
#line 4438 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                if (fe.expr == null)
#line 4439 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    fe.expr = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
#line 4440 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                if (fe.format1 == null)
#line 4441 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                    fe.format1 = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
#line 4442 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            }
#line 4443 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
      		CurrentSemanticValue.ex = new slice_expr_question(ValueStack[ValueStack.Depth-4].ex as addressed_value,fe.expr,fe.format1,fe.format2,CurrentLocationSpan);
#line 4444 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 806: // variable -> tkVertParen, elem_list, tkVertParen
#line 4447 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4448 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new array_const_new(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
#line 4449 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 807: // variable -> proc_func_call
#line 4451 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 808: // variable -> variable, tkPoint, identifier_keyword_operatorname
#line 4453 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4454 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (ValueStack[ValueStack.Depth-3].ex is index)
#line 4455 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
#line 4456 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
#line 4457 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 809: // variable -> tuple, tkPoint, identifier_keyword_operatorname
#line 4459 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4460 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
#line 4461 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 810: // variable -> variable, tkDeref
#line 4467 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4468 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new roof_dereference(ValueStack[ValueStack.Depth-2].ex as addressed_value,CurrentLocationSpan);
#line 4469 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 811: // variable -> variable, tkAmpersend, template_type_params
#line 4471 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4472 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
#line 4473 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 812: // optional_expr_list -> expr_list
#line 4478 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 813: // optional_expr_list -> /* empty */
#line 4480 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 814: // optional_expr_list_func_param -> expr_list_func_param
#line 4485 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 815: // optional_expr_list_func_param -> /* empty */
#line 4487 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 816: // elem_list -> elem_list1
#line 4492 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 817: // elem_list -> /* empty */
#line 4494 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 818: // elem_list1 -> elem
#line 4499 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4500 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 4501 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 819: // elem_list1 -> elem_list1, tkComma, elem
#line 4503 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4504 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
#line 4505 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 820: // elem -> expr
#line 4510 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 821: // elem -> expr, tkDotDot, expr
#line 4512 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 822: // one_literal -> tkStringLiteral
#line 4517 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 823: // one_literal -> tkAsciiChar
#line 4519 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 824: // literal -> literal_list
#line 4524 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4525 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = NewLiteral(ValueStack[ValueStack.Depth-1].stn as literal_const_line);
#line 4526 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 825: // literal -> tkFormatStringLiteral
#line 4528 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4529 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (parserTools.buildTreeForFormatter)
#line 4530 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   			{
#line 4531 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as string_const;
#line 4532 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            }
#line 4533 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            else
#line 4534 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            {
#line 4535 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                CurrentSemanticValue.ex = NewFormatString(ValueStack[ValueStack.Depth-1].stn as string_const);
#line 4536 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            }
#line 4537 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 826: // literal -> tkMultilineStringLiteral
#line 4539 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4540 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (parserTools.buildTreeForFormatter)
#line 4541 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   			{
#line 4542 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   				var sc = ValueStack[ValueStack.Depth-1].stn as string_const;
#line 4543 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
   				sc.IsMultiline = true;
#line 4544 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                CurrentSemanticValue.ex = sc;
#line 4545 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            }
#line 4546 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            else
#line 4547 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            {
#line 4548 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                CurrentSemanticValue.ex = NewLiteral(new literal_const_line(ValueStack[ValueStack.Depth-1].stn as literal, CurrentLocationSpan));
#line 4549 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            }
#line 4550 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 827: // literal_list -> one_literal
#line 4555 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4556 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new literal_const_line(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
#line 4557 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 828: // literal_list -> literal_list, one_literal
#line 4559 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4560 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        	var line = ValueStack[ValueStack.Depth-2].stn as literal_const_line;
#line 4561 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (line.literals.Last() is string_const && ValueStack[ValueStack.Depth-1].ex is string_const)
#line 4562 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            	parserTools.AddErrorFromResource("TWO_STRING_LITERALS_IN_SUCCESSION",LocationStack[LocationStack.Depth-1]);
#line 4563 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = line.Add(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
#line 4564 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 829: // operator_name_ident -> tkOperator, overload_operator
#line 4569 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4570 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new operator_name_ident((ValueStack[ValueStack.Depth-1].op as op_type_node).text, (ValueStack[ValueStack.Depth-1].op as op_type_node).type, CurrentLocationSpan);
#line 4571 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 830: // optional_method_modificators -> tkSemiColon
#line 4576 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4577 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
#line 4578 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 831: // optional_method_modificators -> tkSemiColon, meth_modificators, tkSemiColon
#line 4580 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4581 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			//parserTools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
#line 4582 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 832: // optional_method_modificators1 -> /* empty */
#line 4588 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4589 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
#line 4590 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 833: // optional_method_modificators1 -> tkSemiColon, meth_modificators
#line 4592 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4593 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			//parserTools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
#line 4594 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 834: // meth_modificators -> meth_modificator
#line 4600 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4601 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new procedure_attributes_list(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan); 
#line 4602 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 835: // meth_modificators -> meth_modificators, tkSemiColon, meth_modificator
#line 4604 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4605 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as procedure_attributes_list).Add(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan);  
#line 4606 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 836: // identifier -> tkIdentifier
#line 4611 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 837: // identifier -> property_specifier_directives
#line 4613 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 838: // identifier -> non_reserved
#line 4615 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 839: // identifier -> tkStep
#line 4617 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 840: // identifier -> tkIndex
#line 4619 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 841: // identifier_or_keyword -> identifier
#line 4624 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 842: // identifier_or_keyword -> keyword
#line 4626 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 843: // identifier_or_keyword -> reserved_keyword
#line 4628 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 844: // identifier_keyword_operatorname -> identifier
#line 4633 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 845: // identifier_keyword_operatorname -> keyword
#line 4635 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 846: // identifier_keyword_operatorname -> operator_name_ident
#line 4637 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 847: // meth_modificator -> tkAbstract
#line 4642 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 848: // meth_modificator -> tkOverload
#line 4644 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4645 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id;
#line 4646 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            parserTools.AddWarningFromResource("OVERLOAD_IS_NOT_USED", ValueStack[ValueStack.Depth-1].id.source_context);
#line 4647 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
        }
        break;
      case 849: // meth_modificator -> tkReintroduce
#line 4649 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 850: // meth_modificator -> tkOverride
#line 4651 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 851: // meth_modificator -> tkExtensionMethod
#line 4653 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 852: // meth_modificator -> tkVirtual
#line 4655 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 853: // property_modificator -> tkVirtual
#line 4660 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 854: // property_modificator -> tkOverride
#line 4662 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 855: // property_modificator -> tkAbstract
#line 4664 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 856: // property_modificator -> tkReintroduce
#line 4666 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 857: // property_specifier_directives -> tkRead
#line 4671 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 858: // property_specifier_directives -> tkWrite
#line 4673 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 859: // non_reserved -> tkName
#line 4678 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 860: // non_reserved -> tkNew
#line 4680 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 861: // visibility_specifier -> tkInternal
#line 4685 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 862: // visibility_specifier -> tkPublic
#line 4687 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 863: // visibility_specifier -> tkProtected
#line 4689 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 864: // visibility_specifier -> tkPrivate
#line 4691 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 865: // keyword -> visibility_specifier
#line 4696 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4697 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  
#line 4698 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 866: // keyword -> tkSealed
#line 4700 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 867: // keyword -> tkTemplate
#line 4702 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 868: // keyword -> tkOr
#line 4704 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 869: // keyword -> tkTypeOf
#line 4706 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 870: // keyword -> tkSizeOf
#line 4708 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 871: // keyword -> tkDefault
#line 4710 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 872: // keyword -> tkWhere
#line 4712 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 873: // keyword -> tkXor
#line 4714 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 874: // keyword -> tkAnd
#line 4716 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 875: // keyword -> tkDiv
#line 4718 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 876: // keyword -> tkMod
#line 4720 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 877: // keyword -> tkShl
#line 4722 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 878: // keyword -> tkShr
#line 4724 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 879: // keyword -> tkNot
#line 4726 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 880: // keyword -> tkAs
#line 4728 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 881: // keyword -> tkIn
#line 4730 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 882: // keyword -> tkIs
#line 4732 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 883: // keyword -> tkArray
#line 4734 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 884: // keyword -> tkSequence
#line 4736 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 885: // keyword -> tkBegin
#line 4738 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 886: // keyword -> tkCase
#line 4740 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 887: // keyword -> tkClass
#line 4742 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 888: // keyword -> tkConst
#line 4744 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 889: // keyword -> tkConstructor
#line 4746 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 890: // keyword -> tkDestructor
#line 4748 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 891: // keyword -> tkDownto
#line 4750 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 892: // keyword -> tkDo
#line 4752 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 893: // keyword -> tkElse
#line 4754 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 894: // keyword -> tkEnd
#line 4756 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 895: // keyword -> tkExcept
#line 4758 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 896: // keyword -> tkFile
#line 4760 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 897: // keyword -> tkAuto
#line 4762 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 898: // keyword -> tkFinalization
#line 4764 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 899: // keyword -> tkFinally
#line 4766 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 900: // keyword -> tkFor
#line 4768 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 901: // keyword -> tkForeach
#line 4770 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 902: // keyword -> tkFunction
#line 4772 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 903: // keyword -> tkIf
#line 4774 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 904: // keyword -> tkImplementation
#line 4776 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 905: // keyword -> tkInherited
#line 4778 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 906: // keyword -> tkInitialization
#line 4780 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 907: // keyword -> tkInterface
#line 4782 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 908: // keyword -> tkProcedure
#line 4784 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 909: // keyword -> tkProperty
#line 4786 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 910: // keyword -> tkRaise
#line 4788 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 911: // keyword -> tkRecord
#line 4790 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 912: // keyword -> tkRepeat
#line 4792 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 913: // keyword -> tkSet
#line 4794 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 914: // keyword -> tkTry
#line 4796 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 915: // keyword -> tkType
#line 4798 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 916: // keyword -> tkStatic
#line 4800 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 917: // keyword -> tkThen
#line 4802 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 918: // keyword -> tkTo
#line 4804 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 919: // keyword -> tkUntil
#line 4806 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 920: // keyword -> tkUses
#line 4808 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 921: // keyword -> tkVar
#line 4810 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 922: // keyword -> tkWhile
#line 4812 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 923: // keyword -> tkWith
#line 4814 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 924: // keyword -> tkNil
#line 4816 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 925: // keyword -> tkGoto
#line 4818 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 926: // keyword -> tkOf
#line 4820 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 927: // keyword -> tkLabel
#line 4822 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 928: // keyword -> tkProgram
#line 4824 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 929: // keyword -> tkUnit
#line 4826 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 930: // keyword -> tkLibrary
#line 4828 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 931: // keyword -> tkNamespace
#line 4830 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 932: // keyword -> tkExternal
#line 4832 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 933: // keyword -> tkParams
#line 4834 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 934: // keyword -> tkEvent
#line 4836 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 935: // keyword -> tkYield
#line 4838 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 936: // keyword -> tkMatch
#line 4840 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 937: // keyword -> tkWhen
#line 4842 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 938: // keyword -> tkPartial
#line 4844 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 939: // keyword -> tkAbstract
#line 4846 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 940: // keyword -> tkLock
#line 4848 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 941: // keyword -> tkImplicit
#line 4850 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 942: // keyword -> tkExplicit
#line 4852 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 943: // keyword -> tkOn
#line 4854 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 944: // keyword -> tkVirtual
#line 4856 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 945: // keyword -> tkOverride
#line 4858 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 946: // keyword -> tkLoop
#line 4860 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 947: // keyword -> tkExtensionMethod
#line 4862 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 948: // keyword -> tkOverload
#line 4864 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 949: // keyword -> tkReintroduce
#line 4866 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 950: // keyword -> tkForward
#line 4868 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 951: // reserved_keyword -> tkOperator
#line 4873 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 952: // overload_operator -> tkMinus
#line 4878 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 953: // overload_operator -> tkPlus
#line 4880 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 954: // overload_operator -> tkSlash
#line 4882 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 955: // overload_operator -> tkStar
#line 4884 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 956: // overload_operator -> tkEqual
#line 4886 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 957: // overload_operator -> tkGreater
#line 4888 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 958: // overload_operator -> tkGreaterEqual
#line 4890 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 959: // overload_operator -> tkLower
#line 4892 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 960: // overload_operator -> tkLowerEqual
#line 4894 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 961: // overload_operator -> tkNotEqual
#line 4896 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 962: // overload_operator -> tkOr
#line 4898 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 963: // overload_operator -> tkXor
#line 4900 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 964: // overload_operator -> tkAnd
#line 4902 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 965: // overload_operator -> tkDiv
#line 4904 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 966: // overload_operator -> tkMod
#line 4906 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 967: // overload_operator -> tkShl
#line 4908 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 968: // overload_operator -> tkShr
#line 4910 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 969: // overload_operator -> tkNot
#line 4912 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 970: // overload_operator -> tkIn
#line 4914 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 971: // overload_operator -> tkImplicit
#line 4916 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 972: // overload_operator -> tkExplicit
#line 4918 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 973: // overload_operator -> assign_operator
#line 4920 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 974: // overload_operator -> tkStarStar
#line 4922 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 975: // assign_operator -> tkAssign
#line 4927 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 976: // assign_operator -> tkPlusEqual
#line 4929 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 977: // assign_operator -> tkMinusEqual
#line 4931 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 978: // assign_operator -> tkMultEqual
#line 4933 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 979: // assign_operator -> tkDivEqual
#line 4935 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 980: // lambda_unpacked_params -> tkBackSlashRoundOpen, 
                //                           lambda_list_of_unpacked_params_or_id, tkComma, 
                //                           lambda_unpacked_params_or_id, tkRoundClose
#line 4940 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4941 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			// �?ез�?л�?�?а�? надо п�?исвои�?�? каком�? �?о са�?а�?ном�? пол�? в function_lambda_definition
			(ValueStack[ValueStack.Depth-4].ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-2].ob as ident_or_list);
#line 4942 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-4].ob as unpacked_list_of_ident_or_list;
#line 4943 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 981: // lambda_unpacked_params_or_id -> lambda_unpacked_params
#line 4949 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4950 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = new ident_or_list(ValueStack[ValueStack.Depth-1].ex as unpacked_list_of_ident_or_list);
#line 4951 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 982: // lambda_unpacked_params_or_id -> identifier
#line 4953 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4954 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = new ident_or_list(ValueStack[ValueStack.Depth-1].id as ident);
#line 4955 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 983: // lambda_list_of_unpacked_params_or_id -> lambda_unpacked_params_or_id
#line 4960 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4961 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = new unpacked_list_of_ident_or_list();
#line 4962 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-1].ob as ident_or_list);
#line 4963 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).source_context = LocationStack[LocationStack.Depth-1];
#line 4964 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 984: // lambda_list_of_unpacked_params_or_id -> lambda_list_of_unpacked_params_or_id, 
                //                                         tkComma, lambda_unpacked_params_or_id
#line 4966 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4967 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
#line 4968 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-1].ob as ident_or_list);
#line 4969 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-1]);
#line 4970 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 985: // expr_l1_or_unpacked -> expr_l1
#line 4976 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 986: // expr_l1_or_unpacked -> lambda_unpacked_params
#line 4978 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 987: // expr_l1_or_unpacked_list -> expr_l1_or_unpacked
#line 4984 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 4985 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 4986 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 988: // expr_l1_or_unpacked_list -> expr_l1_or_unpacked_list, tkComma, 
                //                             expr_l1_or_unpacked
#line 4988 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4989 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
#line 4990 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 989: // func_decl_lambda -> identifier, tkArrow, lambda_function_body
#line 4995 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 4996 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var idList = new ident_list(ValueStack[ValueStack.Depth-3].id, LocationStack[LocationStack.Depth-3]); 
#line 4997 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
#line 4998 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			//var sl = $3 as statement_list;
			//if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName($3, "Result") != null) // если э�?о б�?ло в�?�?ажение или ес�?�? пе�?еменная Result, �?о ав�?ов�?вод �?ипа 
			    CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
#line 4999 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			//else 
			//$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, $3 as statement_list, @$);  
		}
        break;
      case 990: // func_decl_lambda -> tkRoundOpen, tkRoundClose, lambda_type_ref_noproctype, 
                //                     tkArrow, lambda_function_body
#line 5005 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5006 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    // �?дес�? надо анализи�?ова�?�? по �?ел�? и либо ос�?авля�?�? lambda_inferred_type, либо дела�?�? его null!
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
#line 5007 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
#line 5008 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, sl, CurrentLocationSpan);	
#line 5009 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 991: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkRoundClose, 
                //                     lambda_type_ref_noproctype, tkArrow, lambda_function_body
#line 5013 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5014 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var idList = new ident_list(ValueStack[ValueStack.Depth-7].id, LocationStack[LocationStack.Depth-7]); 
#line 5015 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            var loc = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]);
#line 5016 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var formalPars = new formal_parameters(new typed_parameters(idList, ValueStack[ValueStack.Depth-5].td, parametr_kind.none, null, loc), loc);
#line 5017 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
#line 5018 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
#line 5019 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, sl, CurrentLocationSpan);	
#line 5020 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 992: // func_decl_lambda -> tkRoundOpen, identifier, tkSemiColon, full_lambda_fp_list, 
                //                     tkRoundClose, lambda_type_ref_noproctype, tkArrow, 
                //                     lambda_function_body
#line 5023 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5024 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var idList = new ident_list(ValueStack[ValueStack.Depth-7].id, LocationStack[LocationStack.Depth-7]);
#line 5025 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new lambda_any_type_node_syntax(), null), parametr_kind.none, null, LocationStack[LocationStack.Depth-7]), LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]));
#line 5026 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			for (int i = 0; i < (ValueStack[ValueStack.Depth-5].stn as formal_parameters).Count; i++)
#line 5027 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				formalPars.Add((ValueStack[ValueStack.Depth-5].stn as formal_parameters).params_list[i]);
#line 5028 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
#line 5029 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
#line 5030 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, sl, CurrentLocationSpan);	
#line 5031 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 993: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkSemiColon, 
                //                     full_lambda_fp_list, tkRoundClose, 
                //                     lambda_type_ref_noproctype, tkArrow, lambda_function_body
#line 5034 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5035 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var idList = new ident_list(ValueStack[ValueStack.Depth-9].id, LocationStack[LocationStack.Depth-9]);
#line 5036 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            var loc = LexLocation.MergeAll(LocationStack[LocationStack.Depth-9],LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7]);
#line 5037 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var formalPars = new formal_parameters(new typed_parameters(idList, ValueStack[ValueStack.Depth-7].td, parametr_kind.none, null, loc), LexLocation.MergeAll(LocationStack[LocationStack.Depth-9],LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]));
#line 5038 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			for (int i = 0; i < (ValueStack[ValueStack.Depth-5].stn as formal_parameters).Count; i++)
#line 5039 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				formalPars.Add((ValueStack[ValueStack.Depth-5].stn as formal_parameters).params_list[i]);
#line 5040 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
#line 5041 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
#line 5042 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, sl, CurrentLocationSpan);
#line 5043 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 994: // func_decl_lambda -> tkRoundOpen, expr_l1_or_unpacked, tkComma, 
                //                     expr_l1_or_unpacked_list, lambda_type_ref, 
                //                     optional_full_lambda_fp_list, tkRoundClose, rem_lambda
#line 5046 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 5047 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var pair = ValueStack[ValueStack.Depth-1].ob as pair_type_stlist;
#line 5048 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			
#line 5049 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (ValueStack[ValueStack.Depth-4].td is lambda_inferred_type)
#line 5050 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			{
#line 5051 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				// добавим с�?да \(x,y)
				// �?�?ой�?ис�? по всем expr_list1. �?сли �?о�?я б�? одна - �?ипа ident_or_list �?о пой�?и по э�?ой ве�?ке и в�?й�?и
				// �?беди�?�?ся, �?�?о $6 = null
				// с�?о�?ми�?ова�?�? List<expression> для unpacked_params и п�?исвои�?�?
				var has_unpacked = false;
#line 5052 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (ValueStack[ValueStack.Depth-7].ex is unpacked_list_of_ident_or_list)
#line 5053 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					has_unpacked = true;
#line 5054 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (!has_unpacked)
#line 5055 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					foreach (var x in (ValueStack[ValueStack.Depth-5].stn as expression_list).expressions)
#line 5056 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					{
#line 5057 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
						if (x is unpacked_list_of_ident_or_list)
#line 5058 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
						{
#line 5059 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
							has_unpacked = true;
#line 5060 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
							break;
#line 5061 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
						}
#line 5062 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					}
#line 5063 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (has_unpacked) // �?�?�? новая ве�?ка
				{
#line 5064 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					if (ValueStack[ValueStack.Depth-3].stn != null)
#line 5065 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					{
#line 5066 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
						parserTools.AddErrorFromResource("SEMICOLON_IN_PARAMS",LocationStack[LocationStack.Depth-3]);
#line 5067 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					}
#line 5068 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				
#line 5069 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					var lst_ex = new List<expression>();
#line 5070 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					lst_ex.Add(ValueStack[ValueStack.Depth-7].ex as expression);
#line 5071 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					foreach (var x in (ValueStack[ValueStack.Depth-5].stn as expression_list).expressions)
#line 5072 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
						lst_ex.Add(x);
#line 5073 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					
#line 5074 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					function_lambda_definition fld = null; //= new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, 
    					//new lambda_inferred_type(new lambda_any_type_node_syntax(), @2), pair.exprs, @$);

#line 5075 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					var sl1 = pair.exprs;
#line 5076 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			    	if (sl1.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl1, "result") != null) // �?о надо в�?води�?�?
						fld = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, pair.tn, pair.exprs, CurrentLocationSpan);
#line 5077 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					else fld = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, pair.exprs, CurrentLocationSpan);	
#line 5078 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"

#line 5079 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					fld.unpacked_params = lst_ex;
#line 5080 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					CurrentSemanticValue.ex = fld;					
#line 5081 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					return;
#line 5082 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				}
#line 5083 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				
#line 5084 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var formal_pars = new formal_parameters();
#line 5085 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var idd = ValueStack[ValueStack.Depth-7].ex as ident;
#line 5086 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (idd==null)
#line 5087 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					parserTools.AddErrorFromResource("ONE_TKIDENTIFIER",LocationStack[LocationStack.Depth-7]);
#line 5088 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var lambda_inf_type = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
#line 5089 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var new_typed_pars = new typed_parameters(new ident_list(idd, idd.source_context), lambda_inf_type, parametr_kind.none, null, idd.source_context);
#line 5090 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				formal_pars.Add(new_typed_pars);
#line 5091 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				foreach (var id in (ValueStack[ValueStack.Depth-5].stn as expression_list).expressions)
#line 5092 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				{
#line 5093 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					var idd1 = id as ident;
#line 5094 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					if (idd1==null)
#line 5095 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
						parserTools.AddErrorFromResource("ONE_TKIDENTIFIER",id.source_context);
#line 5096 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					
#line 5097 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					lambda_inf_type = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
#line 5098 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					new_typed_pars = new typed_parameters(new ident_list(idd1, idd1.source_context), lambda_inf_type, parametr_kind.none, null, idd1.source_context);
#line 5099 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					formal_pars.Add(new_typed_pars);
#line 5100 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				}
#line 5101 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				
#line 5102 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (ValueStack[ValueStack.Depth-3].stn != null)
#line 5103 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					for (int i = 0; i < (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Count; i++)
#line 5104 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
						formal_pars.Add((ValueStack[ValueStack.Depth-3].stn as formal_parameters).params_list[i]);		
#line 5105 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					
#line 5106 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				formal_pars.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]);
#line 5107 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			    
#line 5108 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			    var sl = pair.exprs;
#line 5109 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
					CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formal_pars, pair.tn, pair.exprs, CurrentLocationSpan);
#line 5110 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formal_pars, null, pair.exprs, CurrentLocationSpan);	
#line 5111 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}
#line 5112 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else
#line 5113 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			{			
#line 5114 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var loc = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]);
#line 5115 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var idd = ValueStack[ValueStack.Depth-7].ex as ident;
#line 5116 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (idd==null)
#line 5117 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					parserTools.AddErrorFromResource("ONE_TKIDENTIFIER",LocationStack[LocationStack.Depth-7]);
#line 5118 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				
#line 5119 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var idList = new ident_list(idd, loc);
#line 5120 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				
#line 5121 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var iddlist = (ValueStack[ValueStack.Depth-5].stn as expression_list).expressions;
#line 5122 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				
#line 5123 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				for (int j = 0; j < iddlist.Count; j++)
#line 5124 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				{
#line 5125 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					var idd2 = iddlist[j] as ident;
#line 5126 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					if (idd2==null)
#line 5127 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
						parserTools.AddErrorFromResource("ONE_TKIDENTIFIER",idd2.source_context);
#line 5128 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					idList.Add(idd2);
#line 5129 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				}	
#line 5130 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var parsType = ValueStack[ValueStack.Depth-4].td;
#line 5131 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var formalPars = new formal_parameters(new typed_parameters(idList, parsType, parametr_kind.none, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4])), LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]));
#line 5132 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				
#line 5133 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				if (ValueStack[ValueStack.Depth-3].stn != null)
#line 5134 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					for (int i = 0; i < (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Count; i++)
#line 5135 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
						formalPars.Add((ValueStack[ValueStack.Depth-3].stn as formal_parameters).params_list[i]);
#line 5136 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"

#line 5137 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				var sl = pair.exprs;
#line 5138 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
					CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, pair.tn, pair.exprs, CurrentLocationSpan);
#line 5139 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, pair.exprs, CurrentLocationSpan);
#line 5140 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}
#line 5141 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 995: // func_decl_lambda -> lambda_unpacked_params, rem_lambda
#line 5153 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5154 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		var pair = ValueStack[ValueStack.Depth-1].ob as pair_type_stlist;
#line 5155 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		// пока �?о�?мал�?н�?е па�?аме�?�?�? - null. Раск�?оем и�? са�?а�?н�?м визи�?о�?ом
    		CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, 
#line 5156 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    			new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-2]), pair.exprs, CurrentLocationSpan);
#line 5157 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		// unpacked_params - э�?о для одного па�?аме�?�?а. �?ля нескол�?ки�? - надо д�?�?г�?�? с�?�?�?к�?�?�?�?. �?озможно, список списков
    		var lst_ex = new List<expression>();
#line 5158 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		lst_ex.Add(ValueStack[ValueStack.Depth-2].ex as unpacked_list_of_ident_or_list);
#line 5159 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    		(CurrentSemanticValue.ex as function_lambda_definition).unpacked_params = lst_ex;  
#line 5160 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
    	}
        break;
      case 996: // func_decl_lambda -> expl_func_decl_lambda
#line 5164 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5165 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
#line 5166 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 997: // optional_full_lambda_fp_list -> /* empty */
#line 5170 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ CurrentSemanticValue.stn = null; }
        break;
      case 998: // optional_full_lambda_fp_list -> tkSemiColon, full_lambda_fp_list
#line 5172 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5173 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
#line 5174 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
	}
        break;
      case 999: // rem_lambda -> lambda_type_ref_noproctype, tkArrow, lambda_function_body
#line 5179 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{ 
#line 5180 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    CurrentSemanticValue.ob = new pair_type_stlist(ValueStack[ValueStack.Depth-3].td,ValueStack[ValueStack.Depth-1].stn as statement_list);
#line 5181 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1000: // expl_func_decl_lambda -> tkFunction, lambda_type_ref_noproctype, tkArrow, 
                 //                          lambda_function_body
#line 5186 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5187 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
#line 5188 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1001: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, tkRoundClose, 
                 //                          lambda_type_ref_noproctype, tkArrow, 
                 //                          lambda_function_body
#line 5190 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5191 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
#line 5192 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1002: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, full_lambda_fp_list, 
                 //                          tkRoundClose, lambda_type_ref_noproctype, tkArrow, 
                 //                          lambda_function_body
#line 5194 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5195 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
#line 5196 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1003: // expl_func_decl_lambda -> tkProcedure, tkArrow, lambda_procedure_body
#line 5198 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5199 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
#line 5200 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1004: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, tkRoundClose, tkArrow, 
                 //                          lambda_procedure_body
#line 5202 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5203 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
#line 5204 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1005: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, full_lambda_fp_list, 
                 //                          tkRoundClose, tkArrow, lambda_procedure_body
#line 5206 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5207 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-4].stn as formal_parameters, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
#line 5208 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1006: // full_lambda_fp_list -> lambda_simple_fp_sect
#line 5213 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5214 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var typed_pars = ValueStack[ValueStack.Depth-1].stn as typed_parameters;
#line 5215 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			if (typed_pars.vars_type is lambda_inferred_type)
#line 5216 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			{
#line 5217 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = new formal_parameters();
#line 5218 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				foreach (var id in typed_pars.idents.idents)
#line 5219 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				{
#line 5220 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					var lambda_inf_type = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
#line 5221 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					var new_typed_pars = new typed_parameters(new ident_list(id, id.source_context), lambda_inf_type, parametr_kind.none, null, id.source_context);
#line 5222 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
					(CurrentSemanticValue.stn as formal_parameters).Add(new_typed_pars);
#line 5223 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				}
#line 5224 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
#line 5225 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}
#line 5226 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			else
#line 5227 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			{
#line 5228 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
				CurrentSemanticValue.stn = new formal_parameters(typed_pars, CurrentLocationSpan);
#line 5229 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			}
#line 5230 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1007: // full_lambda_fp_list -> full_lambda_fp_list, tkSemiColon, lambda_simple_fp_sect
#line 5232 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5233 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn =(ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
#line 5234 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1008: // lambda_simple_fp_sect -> ident_list, lambda_type_ref
#line 5239 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5240 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-2].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan);
#line 5241 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1009: // lambda_type_ref -> /* empty */
#line 5246 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5247 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
#line 5248 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1010: // lambda_type_ref -> tkColon, fptype
#line 5250 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5251 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
#line 5252 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1011: // lambda_type_ref_noproctype -> /* empty */
#line 5257 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5258 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
#line 5259 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1012: // lambda_type_ref_noproctype -> tkColon, fptype_noproctype
#line 5261 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5262 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
#line 5263 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1013: // common_lambda_body -> compound_stmt
#line 5268 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5269 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 5270 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1014: // common_lambda_body -> if_stmt
#line 5272 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5273 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 5274 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1015: // common_lambda_body -> while_stmt
#line 5276 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5277 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 5278 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1016: // common_lambda_body -> repeat_stmt
#line 5280 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5281 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 5282 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1017: // common_lambda_body -> for_stmt
#line 5284 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5285 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 5286 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1018: // common_lambda_body -> foreach_stmt
#line 5288 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5289 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 5290 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1019: // common_lambda_body -> loop_stmt
#line 5292 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5293 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 5294 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1020: // common_lambda_body -> case_stmt
#line 5296 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5297 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 5298 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1021: // common_lambda_body -> try_stmt
#line 5300 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5301 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 5302 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1022: // common_lambda_body -> lock_stmt
#line 5304 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5305 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 5306 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1023: // common_lambda_body -> raise_stmt
#line 5308 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5309 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 5310 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1024: // common_lambda_body -> yield_stmt
#line 5312 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5313 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			parserTools.AddErrorFromResource("YIELD_STATEMENT_CANNOT_BE_USED_IN_LAMBDA_BODY", CurrentLocationSpan);
#line 5314 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1025: // common_lambda_body -> tkRoundOpen, assignment, tkRoundClose
#line 5316 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5317 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-2]);
#line 5318 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1026: // lambda_function_body -> expr_l1_for_lambda
#line 5324 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5325 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		    var id = SyntaxVisitors.HasNameVisitor.HasName(ValueStack[ValueStack.Depth-1].ex, "Result"); 
#line 5326 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            if (id != null)
#line 5327 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            {
#line 5328 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
                 parserTools.AddErrorFromResource("RESULT_IDENT_NOT_EXPECTED_IN_THIS_CONTEXT", id.source_context);
#line 5329 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
            }
#line 5330 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			var sl = new statement_list(new assign("result",ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan),CurrentLocationSpan); // надо поме�?а�?�? е�?�? и assign как ав�?осгене�?и�?ованн�?й для лямбд�? - �?�?об�? зап�?е�?и�?�? явн�?й Result
			sl.expr_lambda_body = true;
#line 5331 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = sl;
#line 5332 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1027: // lambda_function_body -> common_lambda_body
#line 5335 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5336 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 5337 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1028: // lambda_procedure_body -> proc_call
#line 5342 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5343 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 5344 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1029: // lambda_procedure_body -> assignment
#line 5346 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5347 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
#line 5348 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
      case 1030: // lambda_procedure_body -> common_lambda_body
#line 5350 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
{
#line 5351 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
#line 5352 "D:\PABC_GIT\pascalabcnet\Parsers\PascalABCParserNewSaushkin\ABCPascal.y"
		}
        break;
    }
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliasses != null && aliasses.ContainsKey(terminal))
        return aliasses[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }

}
}
