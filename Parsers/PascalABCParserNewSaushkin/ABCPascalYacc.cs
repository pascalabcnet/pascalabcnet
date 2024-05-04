// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.6
// Machine:  DESKTOP-V3E9T2U
// DateTime: 23.04.2024 22:43:23
// UserName: alex
// Input file <ABCPascal.y>

// options: no-lines gplex

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using QUT.Gppg;
using PascalABCCompiler.SyntaxTree;
using PascalABCSavParser;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.Errors;
using System.Linq;
using SyntaxVisitors;

namespace GPPGParserScanner
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
public abstract class ScanBase : AbstractScanner<PascalABCSavParser.Union,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

public partial class GPPGParser: ShiftReduceParser<PascalABCSavParser.Union, LexLocation>
{
  // Verbatim content from ABCPascal.y
// Э�?и об�?явления добавля�?�?ся в класс GPPGParser, п�?едс�?авля�?�?ий собой па�?се�?, гене�?и�?�?ем�?й сис�?емой gppg
    public syntax_tree_node root; // �?о�?невой �?зел син�?акси�?еского де�?ева 

    public List<Error> errors;
    public string current_file_name;
    public int max_errors = 10;
    public PascalParserTools parsertools;
    public List<compiler_directive> CompilerDirectives;
	public ParserLambdaHelper lambdaHelper = new ParserLambdaHelper();
	
    public GPPGParser(AbstractScanner<PascalABCSavParser.Union, LexLocation> scanner) : base(scanner) { }
  // End verbatim content from ABCPascal.y

#pragma warning disable 649
  private static Dictionary<int, string> aliasses;
#pragma warning restore 649
  private static Rule[] rules = new Rule[1028];
  private static State[] states = new State[1704];
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
      "property_parameter_list", "const_set", "question_expr", "question_constexpr", 
      "new_question_expr", "record_const", "const_field_list_1", "const_field_list", 
      "const_field", "repeat_stmt", "raise_stmt", "pointer_type", "attribute_declaration", 
      "one_or_some_attribute", "stmt_list", "else_case", "exception_block_else_branch", 
      "compound_stmt", "string_type", "sizeof_expr", "simple_property_definition", 
      "stmt_or_expression", "unlabelled_stmt", "stmt", "case_item", "set_type", 
      "as_is_expr", "as_is_constexpr", "is_type_expr", "as_expr", "power_expr", 
      "power_constexpr", "unsized_array_type", "simple_type_or_", "simple_type", 
      "simple_type_question", "optional_type_specification", "fptype", "type_ref", 
      "fptype_noproctype", "array_type", "template_param", "template_empty_param", 
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
    states[0] = new State(new int[]{61,1602,105,1669,106,1670,109,1671,88,1676,90,1681,89,1688,75,1693,77,1700,3,-27,52,-27,91,-27,59,-27,29,-27,66,-27,50,-27,53,-27,62,-27,11,-27,44,-27,37,-27,28,-27,26,-27,19,-27,30,-27,31,-27},new int[]{-1,1,-234,3,-235,4,-306,1614,-308,1615,-2,1664,-175,1675});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{3,1598,52,-14,91,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-185,5,-186,1596,-184,1601});
    states[5] = new State(-41,new int[]{-302,6});
    states[6] = new State(new int[]{52,1584,59,-67,29,-67,66,-67,50,-67,53,-67,62,-67,11,-67,44,-67,37,-67,28,-67,26,-67,19,-67,30,-67,31,-67,91,-67},new int[]{-18,7,-304,14,-37,15,-41,1515,-42,1516});
    states[7] = new State(new int[]{7,9,10,10,5,11,100,12,6,13,2,-26},new int[]{-188,8});
    states[8] = new State(-20);
    states[9] = new State(-21);
    states[10] = new State(-22);
    states[11] = new State(-23);
    states[12] = new State(-24);
    states[13] = new State(-25);
    states[14] = new State(-42);
    states[15] = new State(new int[]{91,17},new int[]{-255,16});
    states[16] = new State(-34);
    states[17] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,757,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493},new int[]{-252,18,-261,755,-260,22,-4,23,-113,24,-132,373,-111,386,-147,756,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890,-143,1045});
    states[18] = new State(new int[]{92,19,10,20});
    states[19] = new State(-529);
    states[20] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,757,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493,98,-493,101,-493,33,-493,104,-493,2,-493},new int[]{-261,21,-260,22,-4,23,-113,24,-132,373,-111,386,-147,756,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890,-143,1045});
    states[21] = new State(-531);
    states[22] = new State(-491);
    states[23] = new State(-494);
    states[24] = new State(new int[]{110,416,111,417,112,418,113,419,114,420,92,-527,10,-527,98,-527,101,-527,33,-527,104,-527,2,-527,9,-527,100,-527,12,-527,99,-527,32,-527,85,-527,84,-527,83,-527,82,-527,81,-527,86,-527},new int[]{-194,25});
    states[25] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,663,8,664,21,270,22,275,76,541,20,599,40,610,5,619,18,688,37,697,44,701},new int[]{-88,26,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-322,838,-100,673,-323,696});
    states[26] = new State(-521);
    states[27] = new State(-597);
    states[28] = new State(-604);
    states[29] = new State(new int[]{16,30,92,-606,10,-606,98,-606,101,-606,33,-606,104,-606,2,-606,9,-606,100,-606,12,-606,99,-606,32,-606,85,-606,84,-606,83,-606,82,-606,81,-606,86,-606,6,-606,76,-606,5,-606,51,-606,58,-606,141,-606,143,-606,80,-606,78,-606,160,-606,87,-606,45,-606,42,-606,8,-606,21,-606,22,-606,144,-606,147,-606,145,-606,146,-606,155,-606,158,-606,157,-606,156,-606,57,-606,91,-606,40,-606,25,-606,97,-606,54,-606,35,-606,55,-606,102,-606,47,-606,36,-606,53,-606,60,-606,74,-606,72,-606,38,-606,70,-606,71,-606,13,-609});
    states[30] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-98,31,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598});
    states[31] = new State(new int[]{120,312,125,313,123,314,121,315,124,316,122,317,137,318,135,319,16,-620,92,-620,10,-620,98,-620,101,-620,33,-620,104,-620,2,-620,9,-620,100,-620,12,-620,99,-620,32,-620,85,-620,84,-620,83,-620,82,-620,81,-620,86,-620,13,-620,6,-620,76,-620,5,-620,51,-620,58,-620,141,-620,143,-620,80,-620,78,-620,160,-620,87,-620,45,-620,42,-620,8,-620,21,-620,22,-620,144,-620,147,-620,145,-620,146,-620,155,-620,158,-620,157,-620,156,-620,57,-620,91,-620,40,-620,25,-620,97,-620,54,-620,35,-620,55,-620,102,-620,47,-620,36,-620,53,-620,60,-620,74,-620,72,-620,38,-620,70,-620,71,-620,116,-620,115,-620,128,-620,129,-620,126,-620,138,-620,136,-620,118,-620,117,-620,131,-620,132,-620,133,-620,134,-620,130,-620},new int[]{-196,32});
    states[32] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,610},new int[]{-105,33,-242,1514,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,623,-267,598});
    states[33] = new State(new int[]{6,34,120,-645,125,-645,123,-645,121,-645,124,-645,122,-645,137,-645,135,-645,16,-645,92,-645,10,-645,98,-645,101,-645,33,-645,104,-645,2,-645,9,-645,100,-645,12,-645,99,-645,32,-645,85,-645,84,-645,83,-645,82,-645,81,-645,86,-645,13,-645,76,-645,5,-645,51,-645,58,-645,141,-645,143,-645,80,-645,78,-645,160,-645,87,-645,45,-645,42,-645,8,-645,21,-645,22,-645,144,-645,147,-645,145,-645,146,-645,155,-645,158,-645,157,-645,156,-645,57,-645,91,-645,40,-645,25,-645,97,-645,54,-645,35,-645,55,-645,102,-645,47,-645,36,-645,53,-645,60,-645,74,-645,72,-645,38,-645,70,-645,71,-645,116,-645,115,-645,128,-645,129,-645,126,-645,138,-645,136,-645,118,-645,117,-645,131,-645,132,-645,133,-645,134,-645,130,-645});
    states[34] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-83,35,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,623,-267,598});
    states[35] = new State(new int[]{116,327,115,328,128,329,129,330,126,331,6,-724,5,-724,120,-724,125,-724,123,-724,121,-724,124,-724,122,-724,137,-724,135,-724,16,-724,92,-724,10,-724,98,-724,101,-724,33,-724,104,-724,2,-724,9,-724,100,-724,12,-724,99,-724,32,-724,85,-724,84,-724,83,-724,82,-724,81,-724,86,-724,13,-724,76,-724,51,-724,58,-724,141,-724,143,-724,80,-724,78,-724,160,-724,87,-724,45,-724,42,-724,8,-724,21,-724,22,-724,144,-724,147,-724,145,-724,146,-724,155,-724,158,-724,157,-724,156,-724,57,-724,91,-724,40,-724,25,-724,97,-724,54,-724,35,-724,55,-724,102,-724,47,-724,36,-724,53,-724,60,-724,74,-724,72,-724,38,-724,70,-724,71,-724,138,-724,136,-724,118,-724,117,-724,131,-724,132,-724,133,-724,134,-724,130,-724},new int[]{-197,36});
    states[36] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,610},new int[]{-82,37,-242,1513,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,623,-267,598});
    states[37] = new State(new int[]{138,333,136,1476,118,1479,117,1480,131,1481,132,1482,133,1483,134,1484,130,1485,116,-726,115,-726,128,-726,129,-726,126,-726,6,-726,5,-726,120,-726,125,-726,123,-726,121,-726,124,-726,122,-726,137,-726,135,-726,16,-726,92,-726,10,-726,98,-726,101,-726,33,-726,104,-726,2,-726,9,-726,100,-726,12,-726,99,-726,32,-726,85,-726,84,-726,83,-726,82,-726,81,-726,86,-726,13,-726,76,-726,51,-726,58,-726,141,-726,143,-726,80,-726,78,-726,160,-726,87,-726,45,-726,42,-726,8,-726,21,-726,22,-726,144,-726,147,-726,145,-726,146,-726,155,-726,158,-726,157,-726,156,-726,57,-726,91,-726,40,-726,25,-726,97,-726,54,-726,35,-726,55,-726,102,-726,47,-726,36,-726,53,-726,60,-726,74,-726,72,-726,38,-726,70,-726,71,-726},new int[]{-198,38});
    states[38] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,610},new int[]{-96,39,-268,40,-242,41,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-97,551});
    states[39] = new State(-747);
    states[40] = new State(-748);
    states[41] = new State(-749);
    states[42] = new State(-762);
    states[43] = new State(new int[]{7,44,138,-763,136,-763,118,-763,117,-763,131,-763,132,-763,133,-763,134,-763,130,-763,116,-763,115,-763,128,-763,129,-763,126,-763,6,-763,5,-763,120,-763,125,-763,123,-763,121,-763,124,-763,122,-763,137,-763,135,-763,16,-763,92,-763,10,-763,98,-763,101,-763,33,-763,104,-763,2,-763,9,-763,100,-763,12,-763,99,-763,32,-763,85,-763,84,-763,83,-763,82,-763,81,-763,86,-763,13,-763,76,-763,51,-763,58,-763,141,-763,143,-763,80,-763,78,-763,160,-763,87,-763,45,-763,42,-763,8,-763,21,-763,22,-763,144,-763,147,-763,145,-763,146,-763,155,-763,158,-763,157,-763,156,-763,57,-763,91,-763,40,-763,25,-763,97,-763,54,-763,35,-763,55,-763,102,-763,47,-763,36,-763,53,-763,60,-763,74,-763,72,-763,38,-763,70,-763,71,-763,11,-788,17,-788,119,-760});
    states[44] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148},new int[]{-138,45,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[45] = new State(-800);
    states[46] = new State(-838);
    states[47] = new State(-833);
    states[48] = new State(-834);
    states[49] = new State(-854);
    states[50] = new State(-855);
    states[51] = new State(-835);
    states[52] = new State(-856);
    states[53] = new State(-857);
    states[54] = new State(-836);
    states[55] = new State(-837);
    states[56] = new State(-839);
    states[57] = new State(-862);
    states[58] = new State(-858);
    states[59] = new State(-859);
    states[60] = new State(-860);
    states[61] = new State(-861);
    states[62] = new State(-863);
    states[63] = new State(-864);
    states[64] = new State(-865);
    states[65] = new State(-866);
    states[66] = new State(-867);
    states[67] = new State(-868);
    states[68] = new State(-869);
    states[69] = new State(-870);
    states[70] = new State(-871);
    states[71] = new State(-872);
    states[72] = new State(-873);
    states[73] = new State(-874);
    states[74] = new State(-875);
    states[75] = new State(-876);
    states[76] = new State(-877);
    states[77] = new State(-878);
    states[78] = new State(-879);
    states[79] = new State(-880);
    states[80] = new State(-881);
    states[81] = new State(-882);
    states[82] = new State(-883);
    states[83] = new State(-884);
    states[84] = new State(-885);
    states[85] = new State(-886);
    states[86] = new State(-887);
    states[87] = new State(-888);
    states[88] = new State(-889);
    states[89] = new State(-890);
    states[90] = new State(-891);
    states[91] = new State(-892);
    states[92] = new State(-893);
    states[93] = new State(-894);
    states[94] = new State(-895);
    states[95] = new State(-896);
    states[96] = new State(-897);
    states[97] = new State(-898);
    states[98] = new State(-899);
    states[99] = new State(-900);
    states[100] = new State(-901);
    states[101] = new State(-902);
    states[102] = new State(-903);
    states[103] = new State(-904);
    states[104] = new State(-905);
    states[105] = new State(-906);
    states[106] = new State(-907);
    states[107] = new State(-908);
    states[108] = new State(-909);
    states[109] = new State(-910);
    states[110] = new State(-911);
    states[111] = new State(-912);
    states[112] = new State(-913);
    states[113] = new State(-914);
    states[114] = new State(-915);
    states[115] = new State(-916);
    states[116] = new State(-917);
    states[117] = new State(-918);
    states[118] = new State(-919);
    states[119] = new State(-920);
    states[120] = new State(-921);
    states[121] = new State(-922);
    states[122] = new State(-923);
    states[123] = new State(-924);
    states[124] = new State(-925);
    states[125] = new State(-926);
    states[126] = new State(-927);
    states[127] = new State(-928);
    states[128] = new State(-929);
    states[129] = new State(-930);
    states[130] = new State(-931);
    states[131] = new State(-932);
    states[132] = new State(-933);
    states[133] = new State(-934);
    states[134] = new State(-935);
    states[135] = new State(-936);
    states[136] = new State(-937);
    states[137] = new State(-938);
    states[138] = new State(-939);
    states[139] = new State(-940);
    states[140] = new State(-941);
    states[141] = new State(-942);
    states[142] = new State(-943);
    states[143] = new State(-944);
    states[144] = new State(-945);
    states[145] = new State(-946);
    states[146] = new State(-947);
    states[147] = new State(-840);
    states[148] = new State(-948);
    states[149] = new State(-771);
    states[150] = new State(new int[]{144,152,147,153,7,-821,11,-821,17,-821,138,-821,136,-821,118,-821,117,-821,131,-821,132,-821,133,-821,134,-821,130,-821,116,-821,115,-821,128,-821,129,-821,126,-821,6,-821,5,-821,120,-821,125,-821,123,-821,121,-821,124,-821,122,-821,137,-821,135,-821,16,-821,92,-821,10,-821,98,-821,101,-821,33,-821,104,-821,2,-821,9,-821,100,-821,12,-821,99,-821,32,-821,85,-821,84,-821,83,-821,82,-821,81,-821,86,-821,13,-821,119,-821,76,-821,51,-821,58,-821,141,-821,143,-821,80,-821,78,-821,160,-821,87,-821,45,-821,42,-821,8,-821,21,-821,22,-821,145,-821,146,-821,155,-821,158,-821,157,-821,156,-821,57,-821,91,-821,40,-821,25,-821,97,-821,54,-821,35,-821,55,-821,102,-821,47,-821,36,-821,53,-821,60,-821,74,-821,72,-821,38,-821,70,-821,71,-821,127,-821,110,-821,4,-821,142,-821},new int[]{-166,151});
    states[151] = new State(-825);
    states[152] = new State(-819);
    states[153] = new State(-820);
    states[154] = new State(-824);
    states[155] = new State(-822);
    states[156] = new State(-823);
    states[157] = new State(-772);
    states[158] = new State(-189);
    states[159] = new State(-190);
    states[160] = new State(-191);
    states[161] = new State(-192);
    states[162] = new State(-764);
    states[163] = new State(new int[]{8,164});
    states[164] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,679},new int[]{-284,165,-283,167,-180,168,-147,207,-151,48,-152,51,-273,1510,-272,1511,-93,181,-106,290,-107,291,-16,457,-199,458,-165,461,-167,150,-166,154,-300,1512});
    states[165] = new State(new int[]{9,166});
    states[166] = new State(-758);
    states[167] = new State(-631);
    states[168] = new State(new int[]{7,169,4,172,123,174,9,-628,8,-254,118,-254,117,-254,131,-254,132,-254,133,-254,134,-254,130,-254,6,-254,116,-254,115,-254,128,-254,129,-254,13,-254},new int[]{-298,171});
    states[169] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148},new int[]{-138,170,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[170] = new State(-260);
    states[171] = new State(new int[]{9,-629,13,-233});
    states[172] = new State(new int[]{123,174},new int[]{-298,173});
    states[173] = new State(-630);
    states[174] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-296,175,-279,289,-272,179,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-281,1457,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,1458,-224,581,-223,582,-300,1459});
    states[175] = new State(new int[]{121,176,100,177});
    states[176] = new State(-234);
    states[177] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-279,178,-272,179,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-281,1457,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,1458,-224,581,-223,582,-300,1459});
    states[178] = new State(-238);
    states[179] = new State(new int[]{13,180,121,-242,100,-242,120,-242,9,-242,8,-242,138,-242,136,-242,118,-242,117,-242,131,-242,132,-242,133,-242,134,-242,130,-242,116,-242,115,-242,128,-242,129,-242,126,-242,6,-242,5,-242,125,-242,123,-242,124,-242,122,-242,137,-242,135,-242,16,-242,92,-242,10,-242,98,-242,101,-242,33,-242,104,-242,2,-242,12,-242,99,-242,32,-242,85,-242,84,-242,83,-242,82,-242,81,-242,86,-242,76,-242,51,-242,58,-242,141,-242,143,-242,80,-242,78,-242,160,-242,87,-242,45,-242,42,-242,21,-242,22,-242,144,-242,147,-242,145,-242,146,-242,155,-242,158,-242,157,-242,156,-242,57,-242,91,-242,40,-242,25,-242,97,-242,54,-242,35,-242,55,-242,102,-242,47,-242,36,-242,53,-242,60,-242,74,-242,72,-242,38,-242,70,-242,71,-242,127,-242,110,-242});
    states[180] = new State(-243);
    states[181] = new State(new int[]{6,1508,116,234,115,235,128,236,129,237,13,-247,121,-247,100,-247,120,-247,9,-247,8,-247,138,-247,136,-247,118,-247,117,-247,131,-247,132,-247,133,-247,134,-247,130,-247,126,-247,5,-247,125,-247,123,-247,124,-247,122,-247,137,-247,135,-247,16,-247,92,-247,10,-247,98,-247,101,-247,33,-247,104,-247,2,-247,12,-247,99,-247,32,-247,85,-247,84,-247,83,-247,82,-247,81,-247,86,-247,76,-247,51,-247,58,-247,141,-247,143,-247,80,-247,78,-247,160,-247,87,-247,45,-247,42,-247,21,-247,22,-247,144,-247,147,-247,145,-247,146,-247,155,-247,158,-247,157,-247,156,-247,57,-247,91,-247,40,-247,25,-247,97,-247,54,-247,35,-247,55,-247,102,-247,47,-247,36,-247,53,-247,60,-247,74,-247,72,-247,38,-247,70,-247,71,-247,127,-247,110,-247},new int[]{-193,182});
    states[182] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156},new int[]{-106,183,-107,291,-180,460,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154});
    states[183] = new State(new int[]{118,241,117,242,131,243,132,244,133,245,134,246,130,247,6,-251,116,-251,115,-251,128,-251,129,-251,13,-251,121,-251,100,-251,120,-251,9,-251,8,-251,138,-251,136,-251,126,-251,5,-251,125,-251,123,-251,124,-251,122,-251,137,-251,135,-251,16,-251,92,-251,10,-251,98,-251,101,-251,33,-251,104,-251,2,-251,12,-251,99,-251,32,-251,85,-251,84,-251,83,-251,82,-251,81,-251,86,-251,76,-251,51,-251,58,-251,141,-251,143,-251,80,-251,78,-251,160,-251,87,-251,45,-251,42,-251,21,-251,22,-251,144,-251,147,-251,145,-251,146,-251,155,-251,158,-251,157,-251,156,-251,57,-251,91,-251,40,-251,25,-251,97,-251,54,-251,35,-251,55,-251,102,-251,47,-251,36,-251,53,-251,60,-251,74,-251,72,-251,38,-251,70,-251,71,-251,127,-251,110,-251},new int[]{-195,184});
    states[184] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156},new int[]{-107,185,-180,460,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154});
    states[185] = new State(new int[]{8,186,118,-253,117,-253,131,-253,132,-253,133,-253,134,-253,130,-253,6,-253,116,-253,115,-253,128,-253,129,-253,13,-253,121,-253,100,-253,120,-253,9,-253,138,-253,136,-253,126,-253,5,-253,125,-253,123,-253,124,-253,122,-253,137,-253,135,-253,16,-253,92,-253,10,-253,98,-253,101,-253,33,-253,104,-253,2,-253,12,-253,99,-253,32,-253,85,-253,84,-253,83,-253,82,-253,81,-253,86,-253,76,-253,51,-253,58,-253,141,-253,143,-253,80,-253,78,-253,160,-253,87,-253,45,-253,42,-253,21,-253,22,-253,144,-253,147,-253,145,-253,146,-253,155,-253,158,-253,157,-253,156,-253,57,-253,91,-253,40,-253,25,-253,97,-253,54,-253,35,-253,55,-253,102,-253,47,-253,36,-253,53,-253,60,-253,74,-253,72,-253,38,-253,70,-253,71,-253,127,-253,110,-253});
    states[186] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163,9,-184},new int[]{-75,187,-73,189,-94,1507,-90,192,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[187] = new State(new int[]{9,188});
    states[188] = new State(-258);
    states[189] = new State(new int[]{100,190,9,-183,12,-183});
    states[190] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-94,191,-90,192,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[191] = new State(-186);
    states[192] = new State(new int[]{13,193,16,197,6,1501,100,-187,9,-187,12,-187,5,-187});
    states[193] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-90,194,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[194] = new State(new int[]{5,195,13,193,16,197});
    states[195] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-90,196,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[196] = new State(new int[]{13,193,16,197,6,-123,100,-123,9,-123,12,-123,5,-123,92,-123,10,-123,98,-123,101,-123,33,-123,104,-123,2,-123,99,-123,32,-123,85,-123,84,-123,83,-123,82,-123,81,-123,86,-123});
    states[197] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-91,198,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853});
    states[198] = new State(new int[]{120,226,125,227,123,228,121,229,124,230,122,231,137,232,13,-122,16,-122,6,-122,100,-122,9,-122,12,-122,5,-122,92,-122,10,-122,98,-122,101,-122,33,-122,104,-122,2,-122,99,-122,32,-122,85,-122,84,-122,83,-122,82,-122,81,-122,86,-122},new int[]{-192,199});
    states[199] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-81,200,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853});
    states[200] = new State(new int[]{116,234,115,235,128,236,129,237,120,-119,125,-119,123,-119,121,-119,124,-119,122,-119,137,-119,13,-119,16,-119,6,-119,100,-119,9,-119,12,-119,5,-119,92,-119,10,-119,98,-119,101,-119,33,-119,104,-119,2,-119,99,-119,32,-119,85,-119,84,-119,83,-119,82,-119,81,-119,86,-119},new int[]{-193,201});
    states[201] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-13,202,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853});
    states[202] = new State(new int[]{136,239,138,240,118,241,117,242,131,243,132,244,133,245,134,246,130,247,116,-132,115,-132,128,-132,129,-132,120,-132,125,-132,123,-132,121,-132,124,-132,122,-132,137,-132,13,-132,16,-132,6,-132,100,-132,9,-132,12,-132,5,-132,92,-132,10,-132,98,-132,101,-132,33,-132,104,-132,2,-132,99,-132,32,-132,85,-132,84,-132,83,-132,82,-132,81,-132,86,-132},new int[]{-201,203,-195,208});
    states[203] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-283,204,-180,205,-147,207,-151,48,-152,51});
    states[204] = new State(-137);
    states[205] = new State(new int[]{7,169,4,172,123,174,136,-628,138,-628,118,-628,117,-628,131,-628,132,-628,133,-628,134,-628,130,-628,116,-628,115,-628,128,-628,129,-628,120,-628,125,-628,121,-628,124,-628,122,-628,137,-628,13,-628,16,-628,6,-628,100,-628,9,-628,12,-628,5,-628,92,-628,10,-628,98,-628,101,-628,33,-628,104,-628,2,-628,99,-628,32,-628,85,-628,84,-628,83,-628,82,-628,81,-628,86,-628,11,-628,8,-628,126,-628,135,-628,76,-628,51,-628,58,-628,141,-628,143,-628,80,-628,78,-628,160,-628,87,-628,45,-628,42,-628,21,-628,22,-628,144,-628,147,-628,145,-628,146,-628,155,-628,158,-628,157,-628,156,-628,57,-628,91,-628,40,-628,25,-628,97,-628,54,-628,35,-628,55,-628,102,-628,47,-628,36,-628,53,-628,60,-628,74,-628,72,-628,38,-628,70,-628,71,-628},new int[]{-298,206});
    states[206] = new State(-629);
    states[207] = new State(-259);
    states[208] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-10,209,-269,210,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-11,853});
    states[209] = new State(-144);
    states[210] = new State(-145);
    states[211] = new State(new int[]{4,213,11,215,7,831,142,833,8,834,136,-155,138,-155,118,-155,117,-155,131,-155,132,-155,133,-155,134,-155,130,-155,116,-155,115,-155,128,-155,129,-155,120,-155,125,-155,123,-155,121,-155,124,-155,122,-155,137,-155,13,-155,16,-155,6,-155,100,-155,9,-155,12,-155,5,-155,92,-155,10,-155,98,-155,101,-155,33,-155,104,-155,2,-155,99,-155,32,-155,85,-155,84,-155,83,-155,82,-155,81,-155,86,-155,119,-153},new int[]{-12,212});
    states[212] = new State(-174);
    states[213] = new State(new int[]{123,174},new int[]{-298,214});
    states[214] = new State(-175);
    states[215] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163,5,1503,12,-184},new int[]{-121,216,-75,218,-90,220,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859,-73,189,-94,1507});
    states[216] = new State(new int[]{12,217});
    states[217] = new State(-176);
    states[218] = new State(new int[]{12,219});
    states[219] = new State(-180);
    states[220] = new State(new int[]{5,221,13,193,16,197,6,1501,100,-187,12,-187});
    states[221] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163,5,-706,12,-706},new int[]{-122,222,-90,1500,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[222] = new State(new int[]{5,223,12,-711});
    states[223] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-90,224,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[224] = new State(new int[]{13,193,16,197,12,-713});
    states[225] = new State(new int[]{120,226,125,227,123,228,121,229,124,230,122,231,137,232,13,-120,16,-120,6,-120,100,-120,9,-120,12,-120,5,-120,92,-120,10,-120,98,-120,101,-120,33,-120,104,-120,2,-120,99,-120,32,-120,85,-120,84,-120,83,-120,82,-120,81,-120,86,-120},new int[]{-192,199});
    states[226] = new State(-124);
    states[227] = new State(-125);
    states[228] = new State(-126);
    states[229] = new State(-127);
    states[230] = new State(-128);
    states[231] = new State(-129);
    states[232] = new State(-130);
    states[233] = new State(new int[]{116,234,115,235,128,236,129,237,120,-118,125,-118,123,-118,121,-118,124,-118,122,-118,137,-118,13,-118,16,-118,6,-118,100,-118,9,-118,12,-118,5,-118,92,-118,10,-118,98,-118,101,-118,33,-118,104,-118,2,-118,99,-118,32,-118,85,-118,84,-118,83,-118,82,-118,81,-118,86,-118},new int[]{-193,201});
    states[234] = new State(-133);
    states[235] = new State(-134);
    states[236] = new State(-135);
    states[237] = new State(-136);
    states[238] = new State(new int[]{136,239,138,240,118,241,117,242,131,243,132,244,133,245,134,246,130,247,116,-131,115,-131,128,-131,129,-131,120,-131,125,-131,123,-131,121,-131,124,-131,122,-131,137,-131,13,-131,16,-131,6,-131,100,-131,9,-131,12,-131,5,-131,92,-131,10,-131,98,-131,101,-131,33,-131,104,-131,2,-131,99,-131,32,-131,85,-131,84,-131,83,-131,82,-131,81,-131,86,-131},new int[]{-201,203,-195,208});
    states[239] = new State(-733);
    states[240] = new State(-734);
    states[241] = new State(-146);
    states[242] = new State(-147);
    states[243] = new State(-148);
    states[244] = new State(-149);
    states[245] = new State(-150);
    states[246] = new State(-151);
    states[247] = new State(-152);
    states[248] = new State(-141);
    states[249] = new State(-168);
    states[250] = new State(new int[]{26,1489,143,47,85,49,86,50,80,52,78,53,160,54,87,55,8,-857,7,-857,142,-857,4,-857,15,-857,110,-857,111,-857,112,-857,113,-857,114,-857,92,-857,10,-857,11,-857,17,-857,5,-857,98,-857,101,-857,33,-857,104,-857,2,-857,127,-857,138,-857,136,-857,118,-857,117,-857,131,-857,132,-857,133,-857,134,-857,130,-857,116,-857,115,-857,128,-857,129,-857,126,-857,6,-857,120,-857,125,-857,123,-857,121,-857,124,-857,122,-857,137,-857,135,-857,16,-857,9,-857,100,-857,12,-857,99,-857,32,-857,84,-857,83,-857,82,-857,81,-857,13,-857,119,-857,76,-857,51,-857,58,-857,141,-857,45,-857,42,-857,21,-857,22,-857,144,-857,147,-857,145,-857,146,-857,155,-857,158,-857,157,-857,156,-857,57,-857,91,-857,40,-857,25,-857,97,-857,54,-857,35,-857,55,-857,102,-857,47,-857,36,-857,53,-857,60,-857,74,-857,72,-857,38,-857,70,-857,71,-857},new int[]{-283,251,-180,205,-147,207,-151,48,-152,51});
    states[251] = new State(new int[]{11,253,8,658,92,-642,10,-642,98,-642,101,-642,33,-642,104,-642,2,-642,138,-642,136,-642,118,-642,117,-642,131,-642,132,-642,133,-642,134,-642,130,-642,116,-642,115,-642,128,-642,129,-642,126,-642,6,-642,5,-642,120,-642,125,-642,123,-642,121,-642,124,-642,122,-642,137,-642,135,-642,16,-642,9,-642,100,-642,12,-642,99,-642,32,-642,85,-642,84,-642,83,-642,82,-642,81,-642,86,-642,13,-642,76,-642,51,-642,58,-642,141,-642,143,-642,80,-642,78,-642,160,-642,87,-642,45,-642,42,-642,21,-642,22,-642,144,-642,147,-642,145,-642,146,-642,155,-642,158,-642,157,-642,156,-642,57,-642,91,-642,40,-642,25,-642,97,-642,54,-642,35,-642,55,-642,102,-642,47,-642,36,-642,53,-642,60,-642,74,-642,72,-642,38,-642,70,-642,71,-642},new int[]{-70,252});
    states[252] = new State(-635);
    states[253] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,663,8,664,21,270,22,275,76,541,20,599,40,610,5,619,18,688,37,697,44,701,12,-810},new int[]{-67,254,-71,661,-88,662,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-322,838,-100,673,-323,696});
    states[254] = new State(new int[]{12,255});
    states[255] = new State(new int[]{8,257,92,-634,10,-634,98,-634,101,-634,33,-634,104,-634,2,-634,138,-634,136,-634,118,-634,117,-634,131,-634,132,-634,133,-634,134,-634,130,-634,116,-634,115,-634,128,-634,129,-634,126,-634,6,-634,5,-634,120,-634,125,-634,123,-634,121,-634,124,-634,122,-634,137,-634,135,-634,16,-634,9,-634,100,-634,12,-634,99,-634,32,-634,85,-634,84,-634,83,-634,82,-634,81,-634,86,-634,13,-634,76,-634,51,-634,58,-634,141,-634,143,-634,80,-634,78,-634,160,-634,87,-634,45,-634,42,-634,21,-634,22,-634,144,-634,147,-634,145,-634,146,-634,155,-634,158,-634,157,-634,156,-634,57,-634,91,-634,40,-634,25,-634,97,-634,54,-634,35,-634,55,-634,102,-634,47,-634,36,-634,53,-634,60,-634,74,-634,72,-634,38,-634,70,-634,71,-634},new int[]{-5,256});
    states[256] = new State(-636);
    states[257] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,1005,135,845,116,368,115,369,63,163,9,-197},new int[]{-66,258,-65,260,-85,1008,-84,263,-90,264,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859,-95,1009,-243,1010});
    states[258] = new State(new int[]{9,259});
    states[259] = new State(-633);
    states[260] = new State(new int[]{100,261,9,-198});
    states[261] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,1005,135,845,116,368,115,369,63,163},new int[]{-85,262,-84,263,-90,264,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859,-95,1009,-243,1010});
    states[262] = new State(-200);
    states[263] = new State(-416);
    states[264] = new State(new int[]{13,193,16,197,100,-193,9,-193,92,-193,10,-193,98,-193,101,-193,33,-193,104,-193,2,-193,12,-193,99,-193,32,-193,85,-193,84,-193,83,-193,82,-193,81,-193,86,-193});
    states[265] = new State(-169);
    states[266] = new State(-170);
    states[267] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,268,-151,48,-152,51});
    states[268] = new State(-171);
    states[269] = new State(-172);
    states[270] = new State(new int[]{8,271});
    states[271] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-283,272,-180,205,-147,207,-151,48,-152,51});
    states[272] = new State(new int[]{9,273});
    states[273] = new State(-621);
    states[274] = new State(-173);
    states[275] = new State(new int[]{8,276});
    states[276] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-283,277,-282,279,-180,281,-147,207,-151,48,-152,51});
    states[277] = new State(new int[]{9,278});
    states[278] = new State(-622);
    states[279] = new State(new int[]{9,280});
    states[280] = new State(-623);
    states[281] = new State(new int[]{7,169,4,282,123,284,125,1487,9,-628},new int[]{-298,206,-299,1488});
    states[282] = new State(new int[]{123,284,125,1487},new int[]{-298,173,-299,283});
    states[283] = new State(-627);
    states[284] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,24,336,48,479,49,569,34,573,73,577,44,583,37,625,121,-241,100,-241},new int[]{-296,175,-297,285,-279,289,-272,179,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-281,1457,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,1458,-224,581,-223,582,-300,1459,-280,1486});
    states[285] = new State(new int[]{121,286,100,287});
    states[286] = new State(-236);
    states[287] = new State(-241,new int[]{-280,288});
    states[288] = new State(-240);
    states[289] = new State(-237);
    states[290] = new State(new int[]{118,241,117,242,131,243,132,244,133,245,134,246,130,247,6,-250,116,-250,115,-250,128,-250,129,-250,13,-250,121,-250,100,-250,120,-250,9,-250,8,-250,138,-250,136,-250,126,-250,5,-250,125,-250,123,-250,124,-250,122,-250,137,-250,135,-250,16,-250,92,-250,10,-250,98,-250,101,-250,33,-250,104,-250,2,-250,12,-250,99,-250,32,-250,85,-250,84,-250,83,-250,82,-250,81,-250,86,-250,76,-250,51,-250,58,-250,141,-250,143,-250,80,-250,78,-250,160,-250,87,-250,45,-250,42,-250,21,-250,22,-250,144,-250,147,-250,145,-250,146,-250,155,-250,158,-250,157,-250,156,-250,57,-250,91,-250,40,-250,25,-250,97,-250,54,-250,35,-250,55,-250,102,-250,47,-250,36,-250,53,-250,60,-250,74,-250,72,-250,38,-250,70,-250,71,-250,127,-250,110,-250},new int[]{-195,184});
    states[291] = new State(new int[]{8,186,118,-252,117,-252,131,-252,132,-252,133,-252,134,-252,130,-252,6,-252,116,-252,115,-252,128,-252,129,-252,13,-252,121,-252,100,-252,120,-252,9,-252,138,-252,136,-252,126,-252,5,-252,125,-252,123,-252,124,-252,122,-252,137,-252,135,-252,16,-252,92,-252,10,-252,98,-252,101,-252,33,-252,104,-252,2,-252,12,-252,99,-252,32,-252,85,-252,84,-252,83,-252,82,-252,81,-252,86,-252,76,-252,51,-252,58,-252,141,-252,143,-252,80,-252,78,-252,160,-252,87,-252,45,-252,42,-252,21,-252,22,-252,144,-252,147,-252,145,-252,146,-252,155,-252,158,-252,157,-252,156,-252,57,-252,91,-252,40,-252,25,-252,97,-252,54,-252,35,-252,55,-252,102,-252,47,-252,36,-252,53,-252,60,-252,74,-252,72,-252,38,-252,70,-252,71,-252,127,-252,110,-252});
    states[292] = new State(new int[]{7,169,127,293,123,174,8,-254,118,-254,117,-254,131,-254,132,-254,133,-254,134,-254,130,-254,6,-254,116,-254,115,-254,128,-254,129,-254,13,-254,121,-254,100,-254,120,-254,9,-254,138,-254,136,-254,126,-254,5,-254,125,-254,124,-254,122,-254,137,-254,135,-254,16,-254,92,-254,10,-254,98,-254,101,-254,33,-254,104,-254,2,-254,12,-254,99,-254,32,-254,85,-254,84,-254,83,-254,82,-254,81,-254,86,-254,76,-254,51,-254,58,-254,141,-254,143,-254,80,-254,78,-254,160,-254,87,-254,45,-254,42,-254,21,-254,22,-254,144,-254,147,-254,145,-254,146,-254,155,-254,158,-254,157,-254,156,-254,57,-254,91,-254,40,-254,25,-254,97,-254,54,-254,35,-254,55,-254,102,-254,47,-254,36,-254,53,-254,60,-254,74,-254,72,-254,38,-254,70,-254,71,-254,110,-254},new int[]{-298,657});
    states[293] = new State(new int[]{8,295,143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-279,294,-272,179,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-281,1457,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,1458,-224,581,-223,582,-300,1459});
    states[294] = new State(-287);
    states[295] = new State(new int[]{9,296,143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-80,301,-78,307,-276,308,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[296] = new State(new int[]{127,297,121,-291,100,-291,120,-291,9,-291,8,-291,138,-291,136,-291,118,-291,117,-291,131,-291,132,-291,133,-291,134,-291,130,-291,116,-291,115,-291,128,-291,129,-291,126,-291,6,-291,5,-291,125,-291,123,-291,124,-291,122,-291,137,-291,135,-291,16,-291,92,-291,10,-291,98,-291,101,-291,33,-291,104,-291,2,-291,12,-291,99,-291,32,-291,85,-291,84,-291,83,-291,82,-291,81,-291,86,-291,13,-291,76,-291,51,-291,58,-291,141,-291,143,-291,80,-291,78,-291,160,-291,87,-291,45,-291,42,-291,21,-291,22,-291,144,-291,147,-291,145,-291,146,-291,155,-291,158,-291,157,-291,156,-291,57,-291,91,-291,40,-291,25,-291,97,-291,54,-291,35,-291,55,-291,102,-291,47,-291,36,-291,53,-291,60,-291,74,-291,72,-291,38,-291,70,-291,71,-291,110,-291});
    states[297] = new State(new int[]{8,299,143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-279,298,-272,179,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-281,1457,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,1458,-224,581,-223,582,-300,1459});
    states[298] = new State(-289);
    states[299] = new State(new int[]{9,300,143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-80,301,-78,307,-276,308,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[300] = new State(new int[]{127,297,121,-293,100,-293,120,-293,9,-293,8,-293,138,-293,136,-293,118,-293,117,-293,131,-293,132,-293,133,-293,134,-293,130,-293,116,-293,115,-293,128,-293,129,-293,126,-293,6,-293,5,-293,125,-293,123,-293,124,-293,122,-293,137,-293,135,-293,16,-293,92,-293,10,-293,98,-293,101,-293,33,-293,104,-293,2,-293,12,-293,99,-293,32,-293,85,-293,84,-293,83,-293,82,-293,81,-293,86,-293,13,-293,76,-293,51,-293,58,-293,141,-293,143,-293,80,-293,78,-293,160,-293,87,-293,45,-293,42,-293,21,-293,22,-293,144,-293,147,-293,145,-293,146,-293,155,-293,158,-293,157,-293,156,-293,57,-293,91,-293,40,-293,25,-293,97,-293,54,-293,35,-293,55,-293,102,-293,47,-293,36,-293,53,-293,60,-293,74,-293,72,-293,38,-293,70,-293,71,-293,110,-293});
    states[301] = new State(new int[]{9,302,100,682});
    states[302] = new State(new int[]{127,303,13,-249,121,-249,100,-249,120,-249,9,-249,8,-249,138,-249,136,-249,118,-249,117,-249,131,-249,132,-249,133,-249,134,-249,130,-249,116,-249,115,-249,128,-249,129,-249,126,-249,6,-249,5,-249,125,-249,123,-249,124,-249,122,-249,137,-249,135,-249,16,-249,92,-249,10,-249,98,-249,101,-249,33,-249,104,-249,2,-249,12,-249,99,-249,32,-249,85,-249,84,-249,83,-249,82,-249,81,-249,86,-249,76,-249,51,-249,58,-249,141,-249,143,-249,80,-249,78,-249,160,-249,87,-249,45,-249,42,-249,21,-249,22,-249,144,-249,147,-249,145,-249,146,-249,155,-249,158,-249,157,-249,156,-249,57,-249,91,-249,40,-249,25,-249,97,-249,54,-249,35,-249,55,-249,102,-249,47,-249,36,-249,53,-249,60,-249,74,-249,72,-249,38,-249,70,-249,71,-249,110,-249});
    states[303] = new State(new int[]{8,305,143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-279,304,-272,179,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-281,1457,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,1458,-224,581,-223,582,-300,1459});
    states[304] = new State(-290);
    states[305] = new State(new int[]{9,306,143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-80,301,-78,307,-276,308,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[306] = new State(new int[]{127,297,121,-294,100,-294,120,-294,9,-294,8,-294,138,-294,136,-294,118,-294,117,-294,131,-294,132,-294,133,-294,134,-294,130,-294,116,-294,115,-294,128,-294,129,-294,126,-294,6,-294,5,-294,125,-294,123,-294,124,-294,122,-294,137,-294,135,-294,16,-294,92,-294,10,-294,98,-294,101,-294,33,-294,104,-294,2,-294,12,-294,99,-294,32,-294,85,-294,84,-294,83,-294,82,-294,81,-294,86,-294,13,-294,76,-294,51,-294,58,-294,141,-294,143,-294,80,-294,78,-294,160,-294,87,-294,45,-294,42,-294,21,-294,22,-294,144,-294,147,-294,145,-294,146,-294,155,-294,158,-294,157,-294,156,-294,57,-294,91,-294,40,-294,25,-294,97,-294,54,-294,35,-294,55,-294,102,-294,47,-294,36,-294,53,-294,60,-294,74,-294,72,-294,38,-294,70,-294,71,-294,110,-294});
    states[307] = new State(-261);
    states[308] = new State(new int[]{120,309,9,-263,100,-263});
    states[309] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-87,310,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[310] = new State(-264);
    states[311] = new State(new int[]{120,312,125,313,123,314,121,315,124,316,122,317,137,318,135,319,16,-618,92,-618,10,-618,98,-618,101,-618,33,-618,104,-618,2,-618,9,-618,100,-618,12,-618,99,-618,32,-618,85,-618,84,-618,83,-618,82,-618,81,-618,86,-618,13,-618,6,-618,76,-618,5,-618,51,-618,58,-618,141,-618,143,-618,80,-618,78,-618,160,-618,87,-618,45,-618,42,-618,8,-618,21,-618,22,-618,144,-618,147,-618,145,-618,146,-618,155,-618,158,-618,157,-618,156,-618,57,-618,91,-618,40,-618,25,-618,97,-618,54,-618,35,-618,55,-618,102,-618,47,-618,36,-618,53,-618,60,-618,74,-618,72,-618,38,-618,70,-618,71,-618,116,-618,115,-618,128,-618,129,-618,126,-618,138,-618,136,-618,118,-618,117,-618,131,-618,132,-618,133,-618,134,-618,130,-618},new int[]{-196,32});
    states[312] = new State(-715);
    states[313] = new State(-716);
    states[314] = new State(-717);
    states[315] = new State(-718);
    states[316] = new State(-719);
    states[317] = new State(-720);
    states[318] = new State(-721);
    states[319] = new State(new int[]{137,320});
    states[320] = new State(-722);
    states[321] = new State(new int[]{6,34,5,322,120,-644,125,-644,123,-644,121,-644,124,-644,122,-644,137,-644,135,-644,16,-644,92,-644,10,-644,98,-644,101,-644,33,-644,104,-644,2,-644,9,-644,100,-644,12,-644,99,-644,32,-644,85,-644,84,-644,83,-644,82,-644,81,-644,86,-644,13,-644,76,-644});
    states[322] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,5,-704,92,-704,10,-704,98,-704,101,-704,33,-704,104,-704,2,-704,9,-704,100,-704,12,-704,99,-704,32,-704,84,-704,83,-704,82,-704,81,-704,6,-704},new int[]{-115,323,-105,624,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,623,-267,598});
    states[323] = new State(new int[]{5,324,92,-707,10,-707,98,-707,101,-707,33,-707,104,-707,2,-707,9,-707,100,-707,12,-707,99,-707,32,-707,85,-707,84,-707,83,-707,82,-707,81,-707,86,-707,6,-707,76,-707});
    states[324] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-105,325,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,623,-267,598});
    states[325] = new State(new int[]{6,34,92,-709,10,-709,98,-709,101,-709,33,-709,104,-709,2,-709,9,-709,100,-709,12,-709,99,-709,32,-709,85,-709,84,-709,83,-709,82,-709,81,-709,86,-709,76,-709});
    states[326] = new State(new int[]{116,327,115,328,128,329,129,330,126,331,6,-723,5,-723,120,-723,125,-723,123,-723,121,-723,124,-723,122,-723,137,-723,135,-723,16,-723,92,-723,10,-723,98,-723,101,-723,33,-723,104,-723,2,-723,9,-723,100,-723,12,-723,99,-723,32,-723,85,-723,84,-723,83,-723,82,-723,81,-723,86,-723,13,-723,76,-723,51,-723,58,-723,141,-723,143,-723,80,-723,78,-723,160,-723,87,-723,45,-723,42,-723,8,-723,21,-723,22,-723,144,-723,147,-723,145,-723,146,-723,155,-723,158,-723,157,-723,156,-723,57,-723,91,-723,40,-723,25,-723,97,-723,54,-723,35,-723,55,-723,102,-723,47,-723,36,-723,53,-723,60,-723,74,-723,72,-723,38,-723,70,-723,71,-723,138,-723,136,-723,118,-723,117,-723,131,-723,132,-723,133,-723,134,-723,130,-723},new int[]{-197,36});
    states[327] = new State(-728);
    states[328] = new State(-729);
    states[329] = new State(-730);
    states[330] = new State(-731);
    states[331] = new State(-732);
    states[332] = new State(new int[]{138,333,136,1476,118,1479,117,1480,131,1481,132,1482,133,1483,134,1484,130,1485,116,-725,115,-725,128,-725,129,-725,126,-725,6,-725,5,-725,120,-725,125,-725,123,-725,121,-725,124,-725,122,-725,137,-725,135,-725,16,-725,92,-725,10,-725,98,-725,101,-725,33,-725,104,-725,2,-725,9,-725,100,-725,12,-725,99,-725,32,-725,85,-725,84,-725,83,-725,82,-725,81,-725,86,-725,13,-725,76,-725,51,-725,58,-725,141,-725,143,-725,80,-725,78,-725,160,-725,87,-725,45,-725,42,-725,8,-725,21,-725,22,-725,144,-725,147,-725,145,-725,146,-725,155,-725,158,-725,157,-725,156,-725,57,-725,91,-725,40,-725,25,-725,97,-725,54,-725,35,-725,55,-725,102,-725,47,-725,36,-725,53,-725,60,-725,74,-725,72,-725,38,-725,70,-725,71,-725},new int[]{-198,38});
    states[333] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,24,336},new int[]{-283,334,-278,335,-180,205,-147,207,-151,48,-152,51,-270,477});
    states[334] = new State(-739);
    states[335] = new State(-740);
    states[336] = new State(new int[]{11,337,58,1474});
    states[337] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,679,12,-278,100,-278},new int[]{-164,338,-271,1473,-272,1472,-93,181,-106,290,-107,291,-180,460,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154});
    states[338] = new State(new int[]{12,339,100,1470});
    states[339] = new State(new int[]{58,340});
    states[340] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-276,341,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[341] = new State(-272);
    states[342] = new State(new int[]{13,343,120,-226,9,-226,100,-226,121,-226,8,-226,138,-226,136,-226,118,-226,117,-226,131,-226,132,-226,133,-226,134,-226,130,-226,116,-226,115,-226,128,-226,129,-226,126,-226,6,-226,5,-226,125,-226,123,-226,124,-226,122,-226,137,-226,135,-226,16,-226,92,-226,10,-226,98,-226,101,-226,33,-226,104,-226,2,-226,12,-226,99,-226,32,-226,85,-226,84,-226,83,-226,82,-226,81,-226,86,-226,76,-226,51,-226,58,-226,141,-226,143,-226,80,-226,78,-226,160,-226,87,-226,45,-226,42,-226,21,-226,22,-226,144,-226,147,-226,145,-226,146,-226,155,-226,158,-226,157,-226,156,-226,57,-226,91,-226,40,-226,25,-226,97,-226,54,-226,35,-226,55,-226,102,-226,47,-226,36,-226,53,-226,60,-226,74,-226,72,-226,38,-226,70,-226,71,-226,127,-226,110,-226});
    states[343] = new State(-224);
    states[344] = new State(new int[]{11,345,7,-833,127,-833,123,-833,8,-833,118,-833,117,-833,131,-833,132,-833,133,-833,134,-833,130,-833,6,-833,116,-833,115,-833,128,-833,129,-833,13,-833,120,-833,9,-833,100,-833,121,-833,138,-833,136,-833,126,-833,5,-833,125,-833,124,-833,122,-833,137,-833,135,-833,16,-833,92,-833,10,-833,98,-833,101,-833,33,-833,104,-833,2,-833,12,-833,99,-833,32,-833,85,-833,84,-833,83,-833,82,-833,81,-833,86,-833,76,-833,51,-833,58,-833,141,-833,143,-833,80,-833,78,-833,160,-833,87,-833,45,-833,42,-833,21,-833,22,-833,144,-833,147,-833,145,-833,146,-833,155,-833,158,-833,157,-833,156,-833,57,-833,91,-833,40,-833,25,-833,97,-833,54,-833,35,-833,55,-833,102,-833,47,-833,36,-833,53,-833,60,-833,74,-833,72,-833,38,-833,70,-833,71,-833,110,-833});
    states[345] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-90,346,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[346] = new State(new int[]{12,347,13,193,16,197});
    states[347] = new State(-282);
    states[348] = new State(-156);
    states[349] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619,12,-814},new int[]{-69,350,-77,352,-92,362,-87,355,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[350] = new State(new int[]{12,351});
    states[351] = new State(-164);
    states[352] = new State(new int[]{100,353,12,-813,76,-813});
    states[353] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-92,354,-87,355,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[354] = new State(-816);
    states[355] = new State(new int[]{6,356,100,-817,12,-817,76,-817});
    states[356] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-87,357,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[357] = new State(-818);
    states[358] = new State(-744);
    states[359] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619,12,-814},new int[]{-69,360,-77,352,-92,362,-87,355,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[360] = new State(new int[]{12,361});
    states[361] = new State(-765);
    states[362] = new State(-815);
    states[363] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-96,364,-15,365,-165,149,-167,150,-166,154,-16,157,-57,162,-199,366,-113,372,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548});
    states[364] = new State(-766);
    states[365] = new State(new int[]{7,44,138,-763,136,-763,118,-763,117,-763,131,-763,132,-763,133,-763,134,-763,130,-763,116,-763,115,-763,128,-763,129,-763,126,-763,6,-763,5,-763,120,-763,125,-763,123,-763,121,-763,124,-763,122,-763,137,-763,135,-763,16,-763,92,-763,10,-763,98,-763,101,-763,33,-763,104,-763,2,-763,9,-763,100,-763,12,-763,99,-763,32,-763,85,-763,84,-763,83,-763,82,-763,81,-763,86,-763,13,-763,76,-763,51,-763,58,-763,141,-763,143,-763,80,-763,78,-763,160,-763,87,-763,45,-763,42,-763,8,-763,21,-763,22,-763,144,-763,147,-763,145,-763,146,-763,155,-763,158,-763,157,-763,156,-763,57,-763,91,-763,40,-763,25,-763,97,-763,54,-763,35,-763,55,-763,102,-763,47,-763,36,-763,53,-763,60,-763,74,-763,72,-763,38,-763,70,-763,71,-763,11,-788,17,-788});
    states[366] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-96,367,-15,365,-165,149,-167,150,-166,154,-16,157,-57,162,-199,366,-113,372,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548});
    states[367] = new State(-767);
    states[368] = new State(-166);
    states[369] = new State(-167);
    states[370] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-96,371,-15,365,-165,149,-167,150,-166,154,-16,157,-57,162,-199,366,-113,372,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548});
    states[371] = new State(-768);
    states[372] = new State(-769);
    states[373] = new State(new int[]{141,1469,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,432,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541},new int[]{-111,374,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715});
    states[374] = new State(new int[]{8,375,7,387,142,422,4,423,110,-775,111,-775,112,-775,113,-775,114,-775,92,-775,10,-775,98,-775,101,-775,33,-775,104,-775,2,-775,138,-775,136,-775,118,-775,117,-775,131,-775,132,-775,133,-775,134,-775,130,-775,116,-775,115,-775,128,-775,129,-775,126,-775,6,-775,5,-775,120,-775,125,-775,123,-775,121,-775,124,-775,122,-775,137,-775,135,-775,16,-775,9,-775,100,-775,12,-775,99,-775,32,-775,85,-775,84,-775,83,-775,82,-775,81,-775,86,-775,13,-775,119,-775,76,-775,51,-775,58,-775,141,-775,143,-775,80,-775,78,-775,160,-775,87,-775,45,-775,42,-775,21,-775,22,-775,144,-775,147,-775,145,-775,146,-775,155,-775,158,-775,157,-775,156,-775,57,-775,91,-775,40,-775,25,-775,97,-775,54,-775,35,-775,55,-775,102,-775,47,-775,36,-775,53,-775,60,-775,74,-775,72,-775,38,-775,70,-775,71,-775,11,-787,17,-787});
    states[375] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,1466,8,664,21,270,22,275,76,541,20,599,40,610,5,619,18,688,37,697,44,701,9,-812},new int[]{-68,376,-72,378,-89,1468,-87,381,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,1463,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-322,1467,-100,673,-323,696});
    states[376] = new State(new int[]{9,377});
    states[377] = new State(-792);
    states[378] = new State(new int[]{100,379,9,-811});
    states[379] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,1466,8,664,21,270,22,275,76,541,20,599,40,610,5,619,18,688,37,697,44,701},new int[]{-89,380,-87,381,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,1463,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-322,1467,-100,673,-323,696});
    states[380] = new State(-594);
    states[381] = new State(-600);
    states[382] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-96,367,-268,383,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-97,551});
    states[383] = new State(-743);
    states[384] = new State(new int[]{138,-769,136,-769,118,-769,117,-769,131,-769,132,-769,133,-769,134,-769,130,-769,116,-769,115,-769,128,-769,129,-769,126,-769,6,-769,5,-769,120,-769,125,-769,123,-769,121,-769,124,-769,122,-769,137,-769,135,-769,16,-769,92,-769,10,-769,98,-769,101,-769,33,-769,104,-769,2,-769,9,-769,100,-769,12,-769,99,-769,32,-769,85,-769,84,-769,83,-769,82,-769,81,-769,86,-769,13,-769,76,-769,51,-769,58,-769,141,-769,143,-769,80,-769,78,-769,160,-769,87,-769,45,-769,42,-769,8,-769,21,-769,22,-769,144,-769,147,-769,145,-769,146,-769,155,-769,158,-769,157,-769,156,-769,57,-769,91,-769,40,-769,25,-769,97,-769,54,-769,35,-769,55,-769,102,-769,47,-769,36,-769,53,-769,60,-769,74,-769,72,-769,38,-769,70,-769,71,-769,119,-761});
    states[385] = new State(-779);
    states[386] = new State(new int[]{8,375,7,387,142,422,4,423,15,425,110,-776,111,-776,112,-776,113,-776,114,-776,92,-776,10,-776,98,-776,101,-776,33,-776,104,-776,2,-776,138,-776,136,-776,118,-776,117,-776,131,-776,132,-776,133,-776,134,-776,130,-776,116,-776,115,-776,128,-776,129,-776,126,-776,6,-776,5,-776,120,-776,125,-776,123,-776,121,-776,124,-776,122,-776,137,-776,135,-776,16,-776,9,-776,100,-776,12,-776,99,-776,32,-776,85,-776,84,-776,83,-776,82,-776,81,-776,86,-776,13,-776,119,-776,76,-776,51,-776,58,-776,141,-776,143,-776,80,-776,78,-776,160,-776,87,-776,45,-776,42,-776,21,-776,22,-776,144,-776,147,-776,145,-776,146,-776,155,-776,158,-776,157,-776,156,-776,57,-776,91,-776,40,-776,25,-776,97,-776,54,-776,35,-776,55,-776,102,-776,47,-776,36,-776,53,-776,60,-776,74,-776,72,-776,38,-776,70,-776,71,-776,11,-787,17,-787});
    states[387] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,392},new int[]{-148,388,-147,389,-151,48,-152,51,-292,390,-150,57,-191,391});
    states[388] = new State(-805);
    states[389] = new State(-841);
    states[390] = new State(-842);
    states[391] = new State(-843);
    states[392] = new State(new int[]{115,394,116,395,117,396,118,397,120,398,121,399,122,400,123,401,124,402,125,403,128,404,129,405,130,406,131,407,132,408,133,409,134,410,135,411,137,412,139,413,140,414,110,416,111,417,112,418,113,419,114,420,119,421},new int[]{-200,393,-194,415});
    states[393] = new State(-826);
    states[394] = new State(-949);
    states[395] = new State(-950);
    states[396] = new State(-951);
    states[397] = new State(-952);
    states[398] = new State(-953);
    states[399] = new State(-954);
    states[400] = new State(-955);
    states[401] = new State(-956);
    states[402] = new State(-957);
    states[403] = new State(-958);
    states[404] = new State(-959);
    states[405] = new State(-960);
    states[406] = new State(-961);
    states[407] = new State(-962);
    states[408] = new State(-963);
    states[409] = new State(-964);
    states[410] = new State(-965);
    states[411] = new State(-966);
    states[412] = new State(-967);
    states[413] = new State(-968);
    states[414] = new State(-969);
    states[415] = new State(-970);
    states[416] = new State(-972);
    states[417] = new State(-973);
    states[418] = new State(-974);
    states[419] = new State(-975);
    states[420] = new State(-976);
    states[421] = new State(-971);
    states[422] = new State(-807);
    states[423] = new State(new int[]{123,174},new int[]{-298,424});
    states[424] = new State(-808);
    states[425] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,432,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541},new int[]{-111,426,-116,427,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715});
    states[426] = new State(new int[]{8,375,7,387,142,422,4,423,15,425,110,-773,111,-773,112,-773,113,-773,114,-773,92,-773,10,-773,98,-773,101,-773,33,-773,104,-773,2,-773,138,-773,136,-773,118,-773,117,-773,131,-773,132,-773,133,-773,134,-773,130,-773,116,-773,115,-773,128,-773,129,-773,126,-773,6,-773,5,-773,120,-773,125,-773,123,-773,121,-773,124,-773,122,-773,137,-773,135,-773,16,-773,9,-773,100,-773,12,-773,99,-773,32,-773,85,-773,84,-773,83,-773,82,-773,81,-773,86,-773,13,-773,119,-773,76,-773,51,-773,58,-773,141,-773,143,-773,80,-773,78,-773,160,-773,87,-773,45,-773,42,-773,21,-773,22,-773,144,-773,147,-773,145,-773,146,-773,155,-773,158,-773,157,-773,156,-773,57,-773,91,-773,40,-773,25,-773,97,-773,54,-773,35,-773,55,-773,102,-773,47,-773,36,-773,53,-773,60,-773,74,-773,72,-773,38,-773,70,-773,71,-773,11,-787,17,-787});
    states[427] = new State(-774);
    states[428] = new State(-793);
    states[429] = new State(-794);
    states[430] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,431,-151,48,-152,51});
    states[431] = new State(-795);
    states[432] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619,53,1460,18,688},new int[]{-87,433,-359,435,-102,440,-101,719,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-358,722,-100,723});
    states[433] = new State(new int[]{9,434});
    states[434] = new State(-796);
    states[435] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619,53,1460},new int[]{-87,436,-358,438,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[436] = new State(new int[]{9,437});
    states[437] = new State(-797);
    states[438] = new State(-791);
    states[439] = new State(new int[]{53,665,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619,18,688},new int[]{-87,433,-359,435,-102,440,-101,719,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-358,722,-100,723});
    states[440] = new State(new int[]{100,441});
    states[441] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,18,688},new int[]{-79,442,-102,1147,-101,1146,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-100,723});
    states[442] = new State(new int[]{100,1144,5,454,10,-1006,9,-1006},new int[]{-324,443});
    states[443] = new State(new int[]{10,446,9,-994},new int[]{-331,444});
    states[444] = new State(new int[]{9,445});
    states[445] = new State(-759);
    states[446] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-326,447,-327,1096,-158,450,-147,814,-151,48,-152,51});
    states[447] = new State(new int[]{10,448,9,-995});
    states[448] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-327,449,-158,450,-147,814,-151,48,-152,51});
    states[449] = new State(-1004);
    states[450] = new State(new int[]{100,452,5,454,10,-1006,9,-1006},new int[]{-324,451});
    states[451] = new State(-1005);
    states[452] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,453,-151,48,-152,51});
    states[453] = new State(-343);
    states[454] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-275,455,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[455] = new State(-1007);
    states[456] = new State(-485);
    states[457] = new State(-255);
    states[458] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156},new int[]{-107,459,-180,460,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154});
    states[459] = new State(new int[]{8,186,118,-256,117,-256,131,-256,132,-256,133,-256,134,-256,130,-256,6,-256,116,-256,115,-256,128,-256,129,-256,13,-256,121,-256,100,-256,120,-256,9,-256,138,-256,136,-256,126,-256,5,-256,125,-256,123,-256,124,-256,122,-256,137,-256,135,-256,16,-256,92,-256,10,-256,98,-256,101,-256,33,-256,104,-256,2,-256,12,-256,99,-256,32,-256,85,-256,84,-256,83,-256,82,-256,81,-256,86,-256,76,-256,51,-256,58,-256,141,-256,143,-256,80,-256,78,-256,160,-256,87,-256,45,-256,42,-256,21,-256,22,-256,144,-256,147,-256,145,-256,146,-256,155,-256,158,-256,157,-256,156,-256,57,-256,91,-256,40,-256,25,-256,97,-256,54,-256,35,-256,55,-256,102,-256,47,-256,36,-256,53,-256,60,-256,74,-256,72,-256,38,-256,70,-256,71,-256,127,-256,110,-256});
    states[460] = new State(new int[]{7,169,8,-254,118,-254,117,-254,131,-254,132,-254,133,-254,134,-254,130,-254,6,-254,116,-254,115,-254,128,-254,129,-254,13,-254,121,-254,100,-254,120,-254,9,-254,138,-254,136,-254,126,-254,5,-254,125,-254,123,-254,124,-254,122,-254,137,-254,135,-254,16,-254,92,-254,10,-254,98,-254,101,-254,33,-254,104,-254,2,-254,12,-254,99,-254,32,-254,85,-254,84,-254,83,-254,82,-254,81,-254,86,-254,76,-254,51,-254,58,-254,141,-254,143,-254,80,-254,78,-254,160,-254,87,-254,45,-254,42,-254,21,-254,22,-254,144,-254,147,-254,145,-254,146,-254,155,-254,158,-254,157,-254,156,-254,57,-254,91,-254,40,-254,25,-254,97,-254,54,-254,35,-254,55,-254,102,-254,47,-254,36,-254,53,-254,60,-254,74,-254,72,-254,38,-254,70,-254,71,-254,127,-254,110,-254});
    states[461] = new State(-257);
    states[462] = new State(new int[]{9,463,143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-80,301,-78,307,-276,308,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[463] = new State(new int[]{127,297});
    states[464] = new State(-227);
    states[465] = new State(new int[]{13,466,127,467,120,-232,9,-232,100,-232,121,-232,8,-232,138,-232,136,-232,118,-232,117,-232,131,-232,132,-232,133,-232,134,-232,130,-232,116,-232,115,-232,128,-232,129,-232,126,-232,6,-232,5,-232,125,-232,123,-232,124,-232,122,-232,137,-232,135,-232,16,-232,92,-232,10,-232,98,-232,101,-232,33,-232,104,-232,2,-232,12,-232,99,-232,32,-232,85,-232,84,-232,83,-232,82,-232,81,-232,86,-232,76,-232,51,-232,58,-232,141,-232,143,-232,80,-232,78,-232,160,-232,87,-232,45,-232,42,-232,21,-232,22,-232,144,-232,147,-232,145,-232,146,-232,155,-232,158,-232,157,-232,156,-232,57,-232,91,-232,40,-232,25,-232,97,-232,54,-232,35,-232,55,-232,102,-232,47,-232,36,-232,53,-232,60,-232,74,-232,72,-232,38,-232,70,-232,71,-232,110,-232});
    states[466] = new State(-225);
    states[467] = new State(new int[]{8,469,143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-279,468,-272,179,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-281,1457,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,1458,-224,581,-223,582,-300,1459});
    states[468] = new State(-288);
    states[469] = new State(new int[]{9,470,143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-80,301,-78,307,-276,308,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[470] = new State(new int[]{127,297,121,-292,100,-292,120,-292,9,-292,8,-292,138,-292,136,-292,118,-292,117,-292,131,-292,132,-292,133,-292,134,-292,130,-292,116,-292,115,-292,128,-292,129,-292,126,-292,6,-292,5,-292,125,-292,123,-292,124,-292,122,-292,137,-292,135,-292,16,-292,92,-292,10,-292,98,-292,101,-292,33,-292,104,-292,2,-292,12,-292,99,-292,32,-292,85,-292,84,-292,83,-292,82,-292,81,-292,86,-292,13,-292,76,-292,51,-292,58,-292,141,-292,143,-292,80,-292,78,-292,160,-292,87,-292,45,-292,42,-292,21,-292,22,-292,144,-292,147,-292,145,-292,146,-292,155,-292,158,-292,157,-292,156,-292,57,-292,91,-292,40,-292,25,-292,97,-292,54,-292,35,-292,55,-292,102,-292,47,-292,36,-292,53,-292,60,-292,74,-292,72,-292,38,-292,70,-292,71,-292,110,-292});
    states[471] = new State(-228);
    states[472] = new State(-229);
    states[473] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-275,474,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[474] = new State(-265);
    states[475] = new State(-230);
    states[476] = new State(-266);
    states[477] = new State(-273);
    states[478] = new State(-267);
    states[479] = new State(new int[]{8,1337,23,-314,11,-314,92,-314,84,-314,83,-314,82,-314,81,-314,29,-314,143,-314,85,-314,86,-314,80,-314,78,-314,160,-314,87,-314,62,-314,28,-314,26,-314,44,-314,37,-314,19,-314,30,-314,31,-314,46,-314,27,-314},new int[]{-183,480});
    states[480] = new State(new int[]{23,1328,11,-321,92,-321,84,-321,83,-321,82,-321,81,-321,29,-321,143,-321,85,-321,86,-321,80,-321,78,-321,160,-321,87,-321,62,-321,28,-321,26,-321,44,-321,37,-321,19,-321,30,-321,31,-321,46,-321,27,-321},new int[]{-317,481,-316,1326,-315,1348});
    states[481] = new State(new int[]{11,649,92,-338,84,-338,83,-338,82,-338,81,-338,29,-211,143,-211,85,-211,86,-211,80,-211,78,-211,160,-211,87,-211,62,-211,28,-211,26,-211,44,-211,37,-211,19,-211,30,-211,31,-211,46,-211,27,-211},new int[]{-25,482,-32,1249,-34,486,-45,1250,-6,1251,-250,1132,-33,1413,-54,1415,-53,492,-55,1414});
    states[482] = new State(new int[]{92,483,84,1245,83,1246,82,1247,81,1248},new int[]{-7,484});
    states[483] = new State(-296);
    states[484] = new State(new int[]{11,649,92,-338,84,-338,83,-338,82,-338,81,-338,29,-211,143,-211,85,-211,86,-211,80,-211,78,-211,160,-211,87,-211,62,-211,28,-211,26,-211,44,-211,37,-211,19,-211,30,-211,31,-211,46,-211,27,-211},new int[]{-32,485,-34,486,-45,1250,-6,1251,-250,1132,-33,1413,-54,1415,-53,492,-55,1414});
    states[485] = new State(-333);
    states[486] = new State(new int[]{10,488,92,-344,84,-344,83,-344,82,-344,81,-344},new int[]{-190,487});
    states[487] = new State(-339);
    states[488] = new State(new int[]{11,649,92,-345,84,-345,83,-345,82,-345,81,-345,29,-211,143,-211,85,-211,86,-211,80,-211,78,-211,160,-211,87,-211,62,-211,28,-211,26,-211,44,-211,37,-211,19,-211,30,-211,31,-211,46,-211,27,-211},new int[]{-45,489,-33,490,-6,1251,-250,1132,-54,1415,-53,492,-55,1414});
    states[489] = new State(-347);
    states[490] = new State(new int[]{11,649,92,-341,84,-341,83,-341,82,-341,81,-341,28,-211,26,-211,44,-211,37,-211,19,-211,30,-211,31,-211,46,-211,27,-211},new int[]{-54,491,-53,492,-6,493,-250,1132,-55,1414});
    states[491] = new State(-350);
    states[492] = new State(-351);
    states[493] = new State(new int[]{28,1373,26,1374,44,1321,37,1356,19,1376,30,1384,31,1391,11,649,46,1398,27,1407},new int[]{-222,494,-250,495,-219,496,-258,497,-3,498,-230,1375,-228,1309,-225,1320,-229,1355,-227,1382,-215,1395,-216,1396,-218,1397});
    states[494] = new State(-360);
    states[495] = new State(-210);
    states[496] = new State(-361);
    states[497] = new State(-377);
    states[498] = new State(new int[]{30,500,19,1191,46,1263,27,1301,44,1321,37,1356},new int[]{-230,499,-216,1190,-228,1309,-225,1320,-229,1355});
    states[499] = new State(-364);
    states[500] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,8,-376,110,-376,10,-376},new int[]{-172,501,-171,1173,-170,1174,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[501] = new State(new int[]{8,585,110,-469,10,-469},new int[]{-128,502});
    states[502] = new State(new int[]{110,504,10,1162},new int[]{-207,503});
    states[503] = new State(-373);
    states[504] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493},new int[]{-260,505,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[505] = new State(new int[]{10,506});
    states[506] = new State(-422);
    states[507] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,92,-573,10,-573,98,-573,101,-573,33,-573,104,-573,2,-573,9,-573,100,-573,12,-573,99,-573,32,-573,84,-573,83,-573,82,-573,81,-573},new int[]{-147,431,-151,48,-152,51});
    states[508] = new State(new int[]{53,509,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619,18,688},new int[]{-87,433,-359,435,-102,440,-111,706,-101,719,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-358,722,-100,723});
    states[509] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,510,-151,48,-152,51});
    states[510] = new State(new int[]{110,511,100,1152});
    states[511] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-99,512,-87,514,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-101,28,-240,601,-117,602,-242,609,-120,618});
    states[512] = new State(new int[]{9,513,16,30,10,-606,13,-609});
    states[513] = new State(-778);
    states[514] = new State(new int[]{10,515});
    states[515] = new State(-789);
    states[516] = new State(-798);
    states[517] = new State(-799);
    states[518] = new State(new int[]{11,519,17,1148});
    states[519] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,663,8,664,21,270,22,275,76,541,20,599,40,610,5,619,18,688,37,697,44,701},new int[]{-71,520,-88,662,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-322,838,-100,673,-323,696});
    states[520] = new State(new int[]{12,521,100,522});
    states[521] = new State(-801);
    states[522] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,663,8,664,21,270,22,275,76,541,20,599,40,610,5,619,18,688,37,697,44,701},new int[]{-88,523,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-322,838,-100,673,-323,696});
    states[523] = new State(-592);
    states[524] = new State(new int[]{127,525,8,-793,7,-793,142,-793,4,-793,15,-793,138,-793,136,-793,118,-793,117,-793,131,-793,132,-793,133,-793,134,-793,130,-793,116,-793,115,-793,128,-793,129,-793,126,-793,6,-793,5,-793,120,-793,125,-793,123,-793,121,-793,124,-793,122,-793,137,-793,135,-793,16,-793,92,-793,10,-793,98,-793,101,-793,33,-793,104,-793,2,-793,9,-793,100,-793,12,-793,99,-793,32,-793,85,-793,84,-793,83,-793,82,-793,81,-793,86,-793,13,-793,119,-793,11,-793,17,-793});
    states[525] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,20,599,18,688,37,697,44,701,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-328,526,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,671,-117,602,-322,672,-100,673,-323,696,-330,863,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[526] = new State(-986);
    states[527] = new State(-1023);
    states[528] = new State(new int[]{16,30,92,-615,10,-615,98,-615,101,-615,33,-615,104,-615,2,-615,9,-615,100,-615,12,-615,99,-615,32,-615,85,-615,84,-615,83,-615,82,-615,81,-615,86,-615,13,-609});
    states[529] = new State(new int[]{6,34,120,-644,125,-644,123,-644,121,-644,124,-644,122,-644,137,-644,135,-644,16,-644,92,-644,10,-644,98,-644,101,-644,33,-644,104,-644,2,-644,9,-644,100,-644,12,-644,99,-644,32,-644,85,-644,84,-644,83,-644,82,-644,81,-644,86,-644,13,-644,76,-644,5,-644,51,-644,58,-644,141,-644,143,-644,80,-644,78,-644,160,-644,87,-644,45,-644,42,-644,8,-644,21,-644,22,-644,144,-644,147,-644,145,-644,146,-644,155,-644,158,-644,157,-644,156,-644,57,-644,91,-644,40,-644,25,-644,97,-644,54,-644,35,-644,55,-644,102,-644,47,-644,36,-644,53,-644,60,-644,74,-644,72,-644,38,-644,70,-644,71,-644,116,-644,115,-644,128,-644,129,-644,126,-644,138,-644,136,-644,118,-644,117,-644,131,-644,132,-644,133,-644,134,-644,130,-644});
    states[530] = new State(new int[]{53,665,9,667,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,721,21,270,22,275,76,541,20,599,40,610,5,619,18,688},new int[]{-87,433,-359,435,-102,531,-147,1107,-4,717,-101,719,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,720,-132,373,-111,386,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-358,722,-100,723});
    states[531] = new State(new int[]{100,532});
    states[532] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,18,688},new int[]{-79,533,-102,1147,-101,1146,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-100,723});
    states[533] = new State(new int[]{100,1144,5,454,10,-1006,9,-1006},new int[]{-324,534});
    states[534] = new State(new int[]{10,446,9,-994},new int[]{-331,535});
    states[535] = new State(new int[]{9,536});
    states[536] = new State(new int[]{5,675,7,-759,138,-759,136,-759,118,-759,117,-759,131,-759,132,-759,133,-759,134,-759,130,-759,116,-759,115,-759,128,-759,129,-759,126,-759,6,-759,120,-759,125,-759,123,-759,121,-759,124,-759,122,-759,137,-759,135,-759,16,-759,92,-759,10,-759,98,-759,101,-759,33,-759,104,-759,2,-759,9,-759,100,-759,12,-759,99,-759,32,-759,85,-759,84,-759,83,-759,82,-759,81,-759,86,-759,13,-759,127,-1008},new int[]{-335,537,-325,538});
    states[537] = new State(-991);
    states[538] = new State(new int[]{127,539});
    states[539] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,20,599,18,688,37,697,44,701,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-328,540,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,671,-117,602,-322,672,-100,673,-323,696,-330,863,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[540] = new State(-996);
    states[541] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-69,542,-77,352,-92,362,-87,355,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[542] = new State(new int[]{76,543});
    states[543] = new State(-803);
    states[544] = new State(-804);
    states[545] = new State(new int[]{7,546,138,-770,136,-770,118,-770,117,-770,131,-770,132,-770,133,-770,134,-770,130,-770,116,-770,115,-770,128,-770,129,-770,126,-770,6,-770,5,-770,120,-770,125,-770,123,-770,121,-770,124,-770,122,-770,137,-770,135,-770,16,-770,92,-770,10,-770,98,-770,101,-770,33,-770,104,-770,2,-770,9,-770,100,-770,12,-770,99,-770,32,-770,85,-770,84,-770,83,-770,82,-770,81,-770,86,-770,13,-770,76,-770,51,-770,58,-770,141,-770,143,-770,80,-770,78,-770,160,-770,87,-770,45,-770,42,-770,8,-770,21,-770,22,-770,144,-770,147,-770,145,-770,146,-770,155,-770,158,-770,157,-770,156,-770,57,-770,91,-770,40,-770,25,-770,97,-770,54,-770,35,-770,55,-770,102,-770,47,-770,36,-770,53,-770,60,-770,74,-770,72,-770,38,-770,70,-770,71,-770});
    states[546] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,392},new int[]{-148,547,-147,389,-151,48,-152,51,-292,390,-150,57,-191,391});
    states[547] = new State(-806);
    states[548] = new State(-777);
    states[549] = new State(-745);
    states[550] = new State(-746);
    states[551] = new State(new int[]{119,552});
    states[552] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-96,553,-268,554,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-97,551});
    states[553] = new State(-741);
    states[554] = new State(-742);
    states[555] = new State(-750);
    states[556] = new State(new int[]{8,557,138,-735,136,-735,118,-735,117,-735,131,-735,132,-735,133,-735,134,-735,130,-735,116,-735,115,-735,128,-735,129,-735,126,-735,6,-735,5,-735,120,-735,125,-735,123,-735,121,-735,124,-735,122,-735,137,-735,135,-735,16,-735,92,-735,10,-735,98,-735,101,-735,33,-735,104,-735,2,-735,9,-735,100,-735,12,-735,99,-735,32,-735,85,-735,84,-735,83,-735,82,-735,81,-735,86,-735,13,-735,76,-735,51,-735,58,-735,141,-735,143,-735,80,-735,78,-735,160,-735,87,-735,45,-735,42,-735,21,-735,22,-735,144,-735,147,-735,145,-735,146,-735,155,-735,158,-735,157,-735,156,-735,57,-735,91,-735,40,-735,25,-735,97,-735,54,-735,35,-735,55,-735,102,-735,47,-735,36,-735,53,-735,60,-735,74,-735,72,-735,38,-735,70,-735,71,-735});
    states[557] = new State(new int[]{14,562,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,53,564,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,931,8,944},new int[]{-353,558,-351,1143,-15,563,-165,149,-167,150,-166,154,-16,157,-340,1134,-283,1135,-180,205,-147,207,-151,48,-152,51,-343,1141,-344,1142});
    states[558] = new State(new int[]{9,559,10,560,100,1139});
    states[559] = new State(-647);
    states[560] = new State(new int[]{14,562,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,53,564,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,931,8,944},new int[]{-351,561,-15,563,-165,149,-167,150,-166,154,-16,157,-340,1134,-283,1135,-180,205,-147,207,-151,48,-152,51,-343,1141,-344,1142});
    states[561] = new State(-684);
    states[562] = new State(-686);
    states[563] = new State(-687);
    states[564] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,565,-151,48,-152,51});
    states[565] = new State(new int[]{5,566,9,-689,10,-689,100,-689});
    states[566] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-276,567,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[567] = new State(-688);
    states[568] = new State(-268);
    states[569] = new State(new int[]{58,570});
    states[570] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-276,571,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[571] = new State(-279);
    states[572] = new State(-269);
    states[573] = new State(new int[]{58,574,121,-281,100,-281,120,-281,9,-281,8,-281,138,-281,136,-281,118,-281,117,-281,131,-281,132,-281,133,-281,134,-281,130,-281,116,-281,115,-281,128,-281,129,-281,126,-281,6,-281,5,-281,125,-281,123,-281,124,-281,122,-281,137,-281,135,-281,16,-281,92,-281,10,-281,98,-281,101,-281,33,-281,104,-281,2,-281,12,-281,99,-281,32,-281,85,-281,84,-281,83,-281,82,-281,81,-281,86,-281,13,-281,76,-281,51,-281,141,-281,143,-281,80,-281,78,-281,160,-281,87,-281,45,-281,42,-281,21,-281,22,-281,144,-281,147,-281,145,-281,146,-281,155,-281,158,-281,157,-281,156,-281,57,-281,91,-281,40,-281,25,-281,97,-281,54,-281,35,-281,55,-281,102,-281,47,-281,36,-281,53,-281,60,-281,74,-281,72,-281,38,-281,70,-281,71,-281,127,-281,110,-281});
    states[574] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-276,575,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[575] = new State(-280);
    states[576] = new State(-270);
    states[577] = new State(new int[]{58,578});
    states[578] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-276,579,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[579] = new State(-271);
    states[580] = new State(-231);
    states[581] = new State(-283);
    states[582] = new State(-284);
    states[583] = new State(new int[]{8,585,121,-469,100,-469,120,-469,9,-469,138,-469,136,-469,118,-469,117,-469,131,-469,132,-469,133,-469,134,-469,130,-469,116,-469,115,-469,128,-469,129,-469,126,-469,6,-469,5,-469,125,-469,123,-469,124,-469,122,-469,137,-469,135,-469,16,-469,92,-469,10,-469,98,-469,101,-469,33,-469,104,-469,2,-469,12,-469,99,-469,32,-469,85,-469,84,-469,83,-469,82,-469,81,-469,86,-469,13,-469,76,-469,51,-469,58,-469,141,-469,143,-469,80,-469,78,-469,160,-469,87,-469,45,-469,42,-469,21,-469,22,-469,144,-469,147,-469,145,-469,146,-469,155,-469,158,-469,157,-469,156,-469,57,-469,91,-469,40,-469,25,-469,97,-469,54,-469,35,-469,55,-469,102,-469,47,-469,36,-469,53,-469,60,-469,74,-469,72,-469,38,-469,70,-469,71,-469,127,-469,110,-469},new int[]{-128,584});
    states[584] = new State(-285);
    states[585] = new State(new int[]{9,586,11,649,143,-211,85,-211,86,-211,80,-211,78,-211,160,-211,87,-211,53,-211,29,-211,108,-211},new int[]{-129,587,-56,1133,-6,591,-250,1132});
    states[586] = new State(-470);
    states[587] = new State(new int[]{9,588,10,589});
    states[588] = new State(-471);
    states[589] = new State(new int[]{11,649,143,-211,85,-211,86,-211,80,-211,78,-211,160,-211,87,-211,53,-211,29,-211,108,-211},new int[]{-56,590,-6,591,-250,1132});
    states[590] = new State(-473);
    states[591] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,53,633,29,639,108,645,11,649},new int[]{-295,592,-250,495,-159,593,-135,632,-147,631,-151,48,-152,51});
    states[592] = new State(-474);
    states[593] = new State(new int[]{5,594,100,629});
    states[594] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-275,595,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[595] = new State(new int[]{110,596,9,-475,10,-475});
    states[596] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-87,597,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[597] = new State(-479);
    states[598] = new State(-736);
    states[599] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-98,600,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598});
    states[600] = new State(new int[]{120,312,125,313,123,314,121,315,124,316,122,317,137,318,135,319,16,-619,92,-619,10,-619,98,-619,101,-619,33,-619,104,-619,2,-619,9,-619,100,-619,12,-619,99,-619,32,-619,85,-619,84,-619,83,-619,82,-619,81,-619,86,-619,13,-619,6,-619,76,-619,5,-619,51,-619,58,-619,141,-619,143,-619,80,-619,78,-619,160,-619,87,-619,45,-619,42,-619,8,-619,21,-619,22,-619,144,-619,147,-619,145,-619,146,-619,155,-619,158,-619,157,-619,156,-619,57,-619,91,-619,40,-619,25,-619,97,-619,54,-619,35,-619,55,-619,102,-619,47,-619,36,-619,53,-619,60,-619,74,-619,72,-619,38,-619,70,-619,71,-619,116,-619,115,-619,128,-619,129,-619,126,-619,138,-619,136,-619,118,-619,117,-619,131,-619,132,-619,133,-619,134,-619,130,-619},new int[]{-196,32});
    states[601] = new State(new int[]{92,-607,10,-607,98,-607,101,-607,33,-607,104,-607,2,-607,9,-607,100,-607,12,-607,99,-607,32,-607,85,-607,84,-607,83,-607,82,-607,81,-607,86,-607,6,-607,76,-607,5,-607,51,-607,58,-607,141,-607,143,-607,80,-607,78,-607,160,-607,87,-607,45,-607,42,-607,8,-607,21,-607,22,-607,144,-607,147,-607,145,-607,146,-607,155,-607,158,-607,157,-607,156,-607,57,-607,91,-607,40,-607,25,-607,97,-607,54,-607,35,-607,55,-607,102,-607,47,-607,36,-607,53,-607,60,-607,74,-607,72,-607,38,-607,70,-607,71,-607,13,-610});
    states[602] = new State(new int[]{13,603});
    states[603] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599},new int[]{-117,604,-99,607,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,608});
    states[604] = new State(new int[]{5,605,13,603});
    states[605] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599},new int[]{-117,606,-99,607,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,608});
    states[606] = new State(new int[]{13,603,92,-624,10,-624,98,-624,101,-624,33,-624,104,-624,2,-624,9,-624,100,-624,12,-624,99,-624,32,-624,85,-624,84,-624,83,-624,82,-624,81,-624,86,-624,6,-624,76,-624,5,-624,51,-624,58,-624,141,-624,143,-624,80,-624,78,-624,160,-624,87,-624,45,-624,42,-624,8,-624,21,-624,22,-624,144,-624,147,-624,145,-624,146,-624,155,-624,158,-624,157,-624,156,-624,57,-624,91,-624,40,-624,25,-624,97,-624,54,-624,35,-624,55,-624,102,-624,47,-624,36,-624,53,-624,60,-624,74,-624,72,-624,38,-624,70,-624,71,-624});
    states[607] = new State(new int[]{16,30,5,-609,13,-609,92,-609,10,-609,98,-609,101,-609,33,-609,104,-609,2,-609,9,-609,100,-609,12,-609,99,-609,32,-609,85,-609,84,-609,83,-609,82,-609,81,-609,86,-609,6,-609,76,-609,51,-609,58,-609,141,-609,143,-609,80,-609,78,-609,160,-609,87,-609,45,-609,42,-609,8,-609,21,-609,22,-609,144,-609,147,-609,145,-609,146,-609,155,-609,158,-609,157,-609,156,-609,57,-609,91,-609,40,-609,25,-609,97,-609,54,-609,35,-609,55,-609,102,-609,47,-609,36,-609,53,-609,60,-609,74,-609,72,-609,38,-609,70,-609,71,-609});
    states[608] = new State(-610);
    states[609] = new State(-608);
    states[610] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-118,611,-99,616,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-242,617});
    states[611] = new State(new int[]{51,612});
    states[612] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-118,613,-99,616,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-242,617});
    states[613] = new State(new int[]{32,614});
    states[614] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-118,615,-99,616,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-242,617});
    states[615] = new State(-625);
    states[616] = new State(new int[]{16,30,51,-611,32,-611,120,-611,125,-611,123,-611,121,-611,124,-611,122,-611,137,-611,135,-611,92,-611,10,-611,98,-611,101,-611,33,-611,104,-611,2,-611,9,-611,100,-611,12,-611,99,-611,85,-611,84,-611,83,-611,82,-611,81,-611,86,-611,13,-611,6,-611,76,-611,5,-611,58,-611,141,-611,143,-611,80,-611,78,-611,160,-611,87,-611,45,-611,42,-611,8,-611,21,-611,22,-611,144,-611,147,-611,145,-611,146,-611,155,-611,158,-611,157,-611,156,-611,57,-611,91,-611,40,-611,25,-611,97,-611,54,-611,35,-611,55,-611,102,-611,47,-611,36,-611,53,-611,60,-611,74,-611,72,-611,38,-611,70,-611,71,-611,116,-611,115,-611,128,-611,129,-611,126,-611,138,-611,136,-611,118,-611,117,-611,131,-611,132,-611,133,-611,134,-611,130,-611});
    states[617] = new State(-612);
    states[618] = new State(-605);
    states[619] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,5,-704,92,-704,10,-704,98,-704,101,-704,33,-704,104,-704,2,-704,9,-704,100,-704,12,-704,99,-704,32,-704,84,-704,83,-704,82,-704,81,-704,6,-704},new int[]{-115,620,-105,624,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,623,-267,598});
    states[620] = new State(new int[]{5,621,92,-708,10,-708,98,-708,101,-708,33,-708,104,-708,2,-708,9,-708,100,-708,12,-708,99,-708,32,-708,85,-708,84,-708,83,-708,82,-708,81,-708,86,-708,6,-708,76,-708});
    states[621] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-105,622,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,623,-267,598});
    states[622] = new State(new int[]{6,34,92,-710,10,-710,98,-710,101,-710,33,-710,104,-710,2,-710,9,-710,100,-710,12,-710,99,-710,32,-710,85,-710,84,-710,83,-710,82,-710,81,-710,86,-710,76,-710});
    states[623] = new State(-735);
    states[624] = new State(new int[]{6,34,5,-703,92,-703,10,-703,98,-703,101,-703,33,-703,104,-703,2,-703,9,-703,100,-703,12,-703,99,-703,32,-703,85,-703,84,-703,83,-703,82,-703,81,-703,86,-703,76,-703});
    states[625] = new State(new int[]{8,585,5,-469},new int[]{-128,626});
    states[626] = new State(new int[]{5,627});
    states[627] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-275,628,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[628] = new State(-286);
    states[629] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-135,630,-147,631,-151,48,-152,51});
    states[630] = new State(-483);
    states[631] = new State(-484);
    states[632] = new State(-482);
    states[633] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-159,634,-135,632,-147,631,-151,48,-152,51});
    states[634] = new State(new int[]{5,635,100,629});
    states[635] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-275,636,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[636] = new State(new int[]{110,637,9,-476,10,-476});
    states[637] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-87,638,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[638] = new State(-480);
    states[639] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-159,640,-135,632,-147,631,-151,48,-152,51});
    states[640] = new State(new int[]{5,641,100,629});
    states[641] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-275,642,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[642] = new State(new int[]{110,643,9,-477,10,-477});
    states[643] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-87,644,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[644] = new State(-481);
    states[645] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-159,646,-135,632,-147,631,-151,48,-152,51});
    states[646] = new State(new int[]{5,647,100,629});
    states[647] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-275,648,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[648] = new State(-478);
    states[649] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-251,650,-8,1131,-9,654,-180,655,-147,1126,-151,48,-152,51,-300,1129});
    states[650] = new State(new int[]{12,651,100,652});
    states[651] = new State(-212);
    states[652] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-8,653,-9,654,-180,655,-147,1126,-151,48,-152,51,-300,1129});
    states[653] = new State(-214);
    states[654] = new State(-215);
    states[655] = new State(new int[]{7,169,8,658,123,174,12,-642,100,-642},new int[]{-70,656,-298,657});
    states[656] = new State(-781);
    states[657] = new State(-233);
    states[658] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,663,8,664,21,270,22,275,76,541,20,599,40,610,5,619,18,688,37,697,44,701,9,-810},new int[]{-67,659,-71,661,-88,662,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-322,838,-100,673,-323,696});
    states[659] = new State(new int[]{9,660});
    states[660] = new State(-643);
    states[661] = new State(new int[]{100,522,12,-809,9,-809});
    states[662] = new State(-591);
    states[663] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,92,-599,10,-599,98,-599,101,-599,33,-599,104,-599,2,-599,9,-599,100,-599,12,-599,99,-599,32,-599,84,-599,83,-599,82,-599,81,-599},new int[]{-147,431,-151,48,-152,51});
    states[664] = new State(new int[]{53,665,9,667,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619,18,688},new int[]{-87,433,-359,435,-102,531,-147,1107,-101,719,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-358,722,-100,723});
    states[665] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,666,-151,48,-152,51});
    states[666] = new State(new int[]{110,511});
    states[667] = new State(new int[]{5,675,127,-1008},new int[]{-325,668});
    states[668] = new State(new int[]{127,669});
    states[669] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,20,599,18,688,37,697,44,701,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-328,670,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,671,-117,602,-322,672,-100,673,-323,696,-330,863,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[670] = new State(-987);
    states[671] = new State(new int[]{92,-616,10,-616,98,-616,101,-616,33,-616,104,-616,2,-616,9,-616,100,-616,12,-616,99,-616,32,-616,85,-616,84,-616,83,-616,82,-616,81,-616,86,-616,13,-610});
    states[672] = new State(-617);
    states[673] = new State(new int[]{5,675,127,-1008},new int[]{-335,674,-325,538});
    states[674] = new State(-992);
    states[675] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,679,142,473,24,336,48,479,49,569,34,573,73,577},new int[]{-277,676,-272,677,-93,181,-106,290,-107,291,-180,678,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-256,684,-249,685,-281,686,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-300,687});
    states[676] = new State(-1009);
    states[677] = new State(-486);
    states[678] = new State(new int[]{7,169,123,174,8,-254,118,-254,117,-254,131,-254,132,-254,133,-254,134,-254,130,-254,6,-254,116,-254,115,-254,128,-254,129,-254,127,-254},new int[]{-298,657});
    states[679] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-80,680,-78,307,-276,308,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[680] = new State(new int[]{9,681,100,682});
    states[681] = new State(-249);
    states[682] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-78,683,-276,308,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[683] = new State(-262);
    states[684] = new State(-487);
    states[685] = new State(-488);
    states[686] = new State(-489);
    states[687] = new State(-490);
    states[688] = new State(new int[]{18,688,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-24,689,-23,695,-100,693,-147,694,-151,48,-152,51});
    states[689] = new State(new int[]{100,690});
    states[690] = new State(new int[]{18,688,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-23,691,-100,693,-147,694,-151,48,-152,51});
    states[691] = new State(new int[]{9,692,100,-981});
    states[692] = new State(-977);
    states[693] = new State(-978);
    states[694] = new State(-979);
    states[695] = new State(-980);
    states[696] = new State(-993);
    states[697] = new State(new int[]{8,1097,5,675,127,-1008},new int[]{-325,698});
    states[698] = new State(new int[]{127,699});
    states[699] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,20,599,18,688,37,697,44,701,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-328,700,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,671,-117,602,-322,672,-100,673,-323,696,-330,863,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[700] = new State(-997);
    states[701] = new State(new int[]{127,702,8,1088});
    states[702] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,705,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-329,703,-212,704,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-4,724,-330,725,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[703] = new State(-1000);
    states[704] = new State(-1025);
    states[705] = new State(new int[]{53,665,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,721,21,270,22,275,76,541,20,599,40,610,5,619,18,688},new int[]{-87,433,-359,435,-102,440,-111,706,-4,717,-101,719,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,720,-132,373,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-358,722,-100,723});
    states[706] = new State(new int[]{100,707,8,375,7,387,142,422,4,423,15,425,138,-776,136,-776,118,-776,117,-776,131,-776,132,-776,133,-776,134,-776,130,-776,116,-776,115,-776,128,-776,129,-776,126,-776,6,-776,5,-776,120,-776,125,-776,123,-776,121,-776,124,-776,122,-776,137,-776,135,-776,16,-776,9,-776,13,-776,119,-776,110,-776,111,-776,112,-776,113,-776,114,-776,11,-787,17,-787});
    states[707] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,432,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541},new int[]{-336,708,-111,716,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715});
    states[708] = new State(new int[]{9,709,100,712});
    states[709] = new State(new int[]{110,416,111,417,112,418,113,419,114,420},new int[]{-194,710});
    states[710] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-87,711,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[711] = new State(-522);
    states[712] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,432,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541},new int[]{-111,713,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715});
    states[713] = new State(new int[]{8,375,7,387,142,422,4,423,9,-524,100,-524,11,-787,17,-787});
    states[714] = new State(new int[]{7,44,11,-788,17,-788});
    states[715] = new State(new int[]{7,546});
    states[716] = new State(new int[]{8,375,7,387,142,422,4,423,9,-523,100,-523,11,-787,17,-787});
    states[717] = new State(new int[]{9,718});
    states[718] = new State(-1022);
    states[719] = new State(new int[]{9,-604,100,-982});
    states[720] = new State(new int[]{110,416,111,417,112,418,113,419,114,420,138,-769,136,-769,118,-769,117,-769,131,-769,132,-769,133,-769,134,-769,130,-769,116,-769,115,-769,128,-769,129,-769,126,-769,6,-769,5,-769,120,-769,125,-769,123,-769,121,-769,124,-769,122,-769,137,-769,135,-769,16,-769,9,-769,100,-769,13,-769,2,-769,119,-761},new int[]{-194,25});
    states[721] = new State(new int[]{53,665,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619,18,688},new int[]{-87,433,-359,435,-102,440,-111,706,-101,719,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-358,722,-100,723});
    states[722] = new State(-790);
    states[723] = new State(-983);
    states[724] = new State(-1026);
    states[725] = new State(-1027);
    states[726] = new State(-1010);
    states[727] = new State(-1011);
    states[728] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,729,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[729] = new State(new int[]{51,730});
    states[730] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493,98,-493,101,-493,33,-493,104,-493,2,-493,9,-493,100,-493,12,-493,99,-493,32,-493,84,-493,83,-493,82,-493,81,-493},new int[]{-260,731,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[731] = new State(new int[]{32,732,92,-532,10,-532,98,-532,101,-532,33,-532,104,-532,2,-532,9,-532,100,-532,12,-532,99,-532,85,-532,84,-532,83,-532,82,-532,81,-532,86,-532});
    states[732] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493,98,-493,101,-493,33,-493,104,-493,2,-493,9,-493,100,-493,12,-493,99,-493,32,-493,84,-493,83,-493,82,-493,81,-493},new int[]{-260,733,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[733] = new State(-533);
    states[734] = new State(-495);
    states[735] = new State(-496);
    states[736] = new State(new int[]{155,738,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-143,737,-147,739,-151,48,-152,51});
    states[737] = new State(-528);
    states[738] = new State(-99);
    states[739] = new State(-100);
    states[740] = new State(-497);
    states[741] = new State(-498);
    states[742] = new State(-499);
    states[743] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,744,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[744] = new State(new int[]{58,745});
    states[745] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163,32,753,92,-552},new int[]{-36,746,-253,1085,-262,1087,-74,1078,-110,1084,-94,1083,-90,192,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[746] = new State(new int[]{10,749,32,753,92,-552},new int[]{-253,747});
    states[747] = new State(new int[]{92,748});
    states[748] = new State(-543);
    states[749] = new State(new int[]{32,753,143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163,92,-552},new int[]{-253,750,-262,752,-74,1078,-110,1084,-94,1083,-90,192,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[750] = new State(new int[]{92,751});
    states[751] = new State(-544);
    states[752] = new State(-547);
    states[753] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,757,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493,92,-493},new int[]{-252,754,-261,755,-260,22,-4,23,-113,24,-132,373,-111,386,-147,756,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890,-143,1045});
    states[754] = new State(new int[]{10,20,92,-553});
    states[755] = new State(-530);
    states[756] = new State(new int[]{8,-793,7,-793,142,-793,4,-793,15,-793,110,-793,111,-793,112,-793,113,-793,114,-793,92,-793,10,-793,11,-793,17,-793,98,-793,101,-793,33,-793,104,-793,2,-793,5,-100});
    states[757] = new State(new int[]{7,-189,11,-189,17,-189,5,-99});
    states[758] = new State(-500);
    states[759] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,757,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,98,-493,10,-493},new int[]{-252,760,-261,755,-260,22,-4,23,-113,24,-132,373,-111,386,-147,756,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890,-143,1045});
    states[760] = new State(new int[]{98,761,10,20});
    states[761] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-87,762,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[762] = new State(-554);
    states[763] = new State(-501);
    states[764] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,765,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[765] = new State(new int[]{99,1074,141,-557,143,-557,85,-557,86,-557,80,-557,78,-557,160,-557,87,-557,45,-557,42,-557,8,-557,21,-557,22,-557,144,-557,147,-557,145,-557,146,-557,155,-557,158,-557,157,-557,156,-557,76,-557,57,-557,91,-557,40,-557,25,-557,97,-557,54,-557,35,-557,55,-557,102,-557,47,-557,36,-557,53,-557,60,-557,74,-557,72,-557,38,-557,92,-557,10,-557,98,-557,101,-557,33,-557,104,-557,2,-557,9,-557,100,-557,12,-557,32,-557,84,-557,83,-557,82,-557,81,-557},new int[]{-291,766});
    states[766] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493,98,-493,101,-493,33,-493,104,-493,2,-493,9,-493,100,-493,12,-493,99,-493,32,-493,84,-493,83,-493,82,-493,81,-493},new int[]{-260,767,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[767] = new State(-555);
    states[768] = new State(-502);
    states[769] = new State(new int[]{53,1077,143,-564,85,-564,86,-564,80,-564,78,-564,160,-564,87,-564},new int[]{-19,770});
    states[770] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,771,-151,48,-152,51});
    states[771] = new State(new int[]{5,1027,110,-562},new int[]{-274,772});
    states[772] = new State(new int[]{110,773});
    states[773] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,774,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[774] = new State(new int[]{70,1075,71,1076},new int[]{-119,775});
    states[775] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,776,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[776] = new State(new int[]{160,1070,99,1074,141,-557,143,-557,85,-557,86,-557,80,-557,78,-557,87,-557,45,-557,42,-557,8,-557,21,-557,22,-557,144,-557,147,-557,145,-557,146,-557,155,-557,158,-557,157,-557,156,-557,76,-557,57,-557,91,-557,40,-557,25,-557,97,-557,54,-557,35,-557,55,-557,102,-557,47,-557,36,-557,53,-557,60,-557,74,-557,72,-557,38,-557,92,-557,10,-557,98,-557,101,-557,33,-557,104,-557,2,-557,9,-557,100,-557,12,-557,32,-557,84,-557,83,-557,82,-557,81,-557},new int[]{-291,777});
    states[777] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493,98,-493,101,-493,33,-493,104,-493,2,-493,9,-493,100,-493,12,-493,99,-493,32,-493,84,-493,83,-493,82,-493,81,-493},new int[]{-260,778,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[778] = new State(-570);
    states[779] = new State(-503);
    states[780] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,663,8,664,21,270,22,275,76,541,20,599,40,610,5,619,18,688,37,697,44,701},new int[]{-71,781,-88,662,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-322,838,-100,673,-323,696});
    states[781] = new State(new int[]{99,782,100,522});
    states[782] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493,98,-493,101,-493,33,-493,104,-493,2,-493,9,-493,100,-493,12,-493,99,-493,32,-493,84,-493,83,-493,82,-493,81,-493},new int[]{-260,783,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[783] = new State(-572);
    states[784] = new State(-504);
    states[785] = new State(-505);
    states[786] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,757,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493,101,-493,33,-493},new int[]{-252,787,-261,755,-260,22,-4,23,-113,24,-132,373,-111,386,-147,756,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890,-143,1045});
    states[787] = new State(new int[]{10,20,101,789,33,1048},new int[]{-289,788});
    states[788] = new State(-574);
    states[789] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,757,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493},new int[]{-252,790,-261,755,-260,22,-4,23,-113,24,-132,373,-111,386,-147,756,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890,-143,1045});
    states[790] = new State(new int[]{92,791,10,20});
    states[791] = new State(-575);
    states[792] = new State(-506);
    states[793] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619,92,-589,10,-589,98,-589,101,-589,33,-589,104,-589,2,-589,9,-589,100,-589,12,-589,99,-589,32,-589,84,-589,83,-589,82,-589,81,-589},new int[]{-87,794,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[794] = new State(-590);
    states[795] = new State(-507);
    states[796] = new State(new int[]{53,1029,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,797,-151,48,-152,51});
    states[797] = new State(new int[]{5,1027,137,-562},new int[]{-274,798});
    states[798] = new State(new int[]{137,799});
    states[799] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,800,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[800] = new State(new int[]{87,1025,99,-560},new int[]{-360,801});
    states[801] = new State(new int[]{99,802});
    states[802] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493,98,-493,101,-493,33,-493,104,-493,2,-493,9,-493,100,-493,12,-493,99,-493,32,-493,84,-493,83,-493,82,-493,81,-493},new int[]{-260,803,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[803] = new State(-567);
    states[804] = new State(-508);
    states[805] = new State(new int[]{8,807,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-311,806,-158,815,-147,814,-151,48,-152,51});
    states[806] = new State(-518);
    states[807] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,808,-151,48,-152,51});
    states[808] = new State(new int[]{100,809});
    states[809] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-158,810,-147,814,-151,48,-152,51});
    states[810] = new State(new int[]{9,811,100,452});
    states[811] = new State(new int[]{110,812});
    states[812] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-87,813,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[813] = new State(-520);
    states[814] = new State(-342);
    states[815] = new State(new int[]{5,816,100,452,110,1023});
    states[816] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-276,817,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[817] = new State(new int[]{110,1021,120,1022,92,-406,10,-406,98,-406,101,-406,33,-406,104,-406,2,-406,9,-406,100,-406,12,-406,99,-406,32,-406,85,-406,84,-406,83,-406,82,-406,81,-406,86,-406},new int[]{-338,818});
    states[818] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,992,135,845,116,368,115,369,63,163,37,697,44,701,40,610},new int[]{-86,819,-85,820,-84,263,-90,264,-91,225,-81,821,-13,238,-10,248,-14,211,-147,860,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859,-95,1009,-243,1010,-323,1019,-242,1020});
    states[819] = new State(-408);
    states[820] = new State(-409);
    states[821] = new State(new int[]{6,822,116,234,115,235,128,236,129,237,120,-118,125,-118,123,-118,121,-118,124,-118,122,-118,137,-118,13,-118,16,-118,92,-118,10,-118,98,-118,101,-118,33,-118,104,-118,2,-118,9,-118,100,-118,12,-118,99,-118,32,-118,85,-118,84,-118,83,-118,82,-118,81,-118,86,-118},new int[]{-193,201});
    states[822] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-13,823,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853});
    states[823] = new State(new int[]{136,239,138,240,118,241,117,242,131,243,132,244,133,245,134,246,130,247,92,-410,10,-410,98,-410,101,-410,33,-410,104,-410,2,-410,9,-410,100,-410,12,-410,99,-410,32,-410,85,-410,84,-410,83,-410,82,-410,81,-410,86,-410},new int[]{-201,203,-195,208});
    states[824] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-69,825,-77,352,-92,362,-87,355,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[825] = new State(new int[]{76,826});
    states[826] = new State(-165);
    states[827] = new State(-157);
    states[828] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,839,135,845,116,368,115,369,63,163},new int[]{-10,829,-14,830,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,847,-173,849,-57,850});
    states[829] = new State(-158);
    states[830] = new State(new int[]{4,213,11,215,7,831,142,833,8,834,136,-155,138,-155,118,-155,117,-155,131,-155,132,-155,133,-155,134,-155,130,-155,116,-155,115,-155,128,-155,129,-155,120,-155,125,-155,123,-155,121,-155,124,-155,122,-155,137,-155,13,-155,16,-155,6,-155,100,-155,9,-155,12,-155,5,-155,92,-155,10,-155,98,-155,101,-155,33,-155,104,-155,2,-155,99,-155,32,-155,85,-155,84,-155,83,-155,82,-155,81,-155,86,-155},new int[]{-12,212});
    states[831] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148},new int[]{-138,832,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[832] = new State(-177);
    states[833] = new State(-178);
    states[834] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,663,8,664,21,270,22,275,76,541,20,599,40,610,5,619,18,688,37,697,44,701,9,-182},new int[]{-76,835,-71,837,-88,662,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-322,838,-100,673,-323,696});
    states[835] = new State(new int[]{9,836});
    states[836] = new State(-179);
    states[837] = new State(new int[]{100,522,9,-181});
    states[838] = new State(-598);
    states[839] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-90,840,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[840] = new State(new int[]{9,841,13,193,16,197});
    states[841] = new State(-159);
    states[842] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-90,843,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[843] = new State(new int[]{9,844,13,193,16,197});
    states[844] = new State(new int[]{136,-159,138,-159,118,-159,117,-159,131,-159,132,-159,133,-159,134,-159,130,-159,116,-159,115,-159,128,-159,129,-159,120,-159,125,-159,123,-159,121,-159,124,-159,122,-159,137,-159,13,-159,16,-159,6,-159,100,-159,9,-159,12,-159,5,-159,92,-159,10,-159,98,-159,101,-159,33,-159,104,-159,2,-159,99,-159,32,-159,85,-159,84,-159,83,-159,82,-159,81,-159,86,-159,119,-154});
    states[845] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,839,135,845,116,368,115,369,63,163},new int[]{-10,846,-14,830,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,847,-173,849,-57,850});
    states[846] = new State(-160);
    states[847] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,839,135,845,116,368,115,369,63,163},new int[]{-10,848,-14,830,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,847,-173,849,-57,850});
    states[848] = new State(-161);
    states[849] = new State(-162);
    states[850] = new State(-163);
    states[851] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-10,848,-269,852,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-11,853});
    states[852] = new State(-140);
    states[853] = new State(new int[]{119,854});
    states[854] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-10,855,-269,856,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-11,853});
    states[855] = new State(-138);
    states[856] = new State(-139);
    states[857] = new State(-142);
    states[858] = new State(-143);
    states[859] = new State(-121);
    states[860] = new State(new int[]{127,861,4,-168,11,-168,7,-168,142,-168,8,-168,136,-168,138,-168,118,-168,117,-168,131,-168,132,-168,133,-168,134,-168,130,-168,6,-168,116,-168,115,-168,128,-168,129,-168,120,-168,125,-168,123,-168,121,-168,124,-168,122,-168,137,-168,13,-168,16,-168,92,-168,10,-168,98,-168,101,-168,33,-168,104,-168,2,-168,9,-168,100,-168,12,-168,99,-168,32,-168,85,-168,84,-168,83,-168,82,-168,81,-168,86,-168,119,-168});
    states[861] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,20,599,18,688,37,697,44,701,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-328,862,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,671,-117,602,-322,672,-100,673,-323,696,-330,863,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[862] = new State(-412);
    states[863] = new State(-1024);
    states[864] = new State(-1012);
    states[865] = new State(-1013);
    states[866] = new State(-1014);
    states[867] = new State(-1015);
    states[868] = new State(-1016);
    states[869] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,870,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[870] = new State(new int[]{99,871});
    states[871] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493,98,-493,101,-493,33,-493,104,-493,2,-493,9,-493,100,-493,12,-493,99,-493,32,-493,84,-493,83,-493,82,-493,81,-493},new int[]{-260,872,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[872] = new State(-515);
    states[873] = new State(-509);
    states[874] = new State(-595);
    states[875] = new State(-596);
    states[876] = new State(-510);
    states[877] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,878,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[878] = new State(new int[]{99,879});
    states[879] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493,98,-493,101,-493,33,-493,104,-493,2,-493,9,-493,100,-493,12,-493,99,-493,32,-493,84,-493,83,-493,82,-493,81,-493},new int[]{-260,880,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[880] = new State(-558);
    states[881] = new State(-511);
    states[882] = new State(new int[]{73,884,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,664,21,270,22,275,76,541,20,599,40,610,18,688,37,697,44,701},new int[]{-103,883,-101,886,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-322,887,-100,673,-323,696});
    states[883] = new State(-516);
    states[884] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,664,21,270,22,275,76,541,20,599,40,610,18,688,37,697,44,701},new int[]{-103,885,-101,886,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-322,887,-100,673,-323,696});
    states[885] = new State(-517);
    states[886] = new State(-613);
    states[887] = new State(-614);
    states[888] = new State(-512);
    states[889] = new State(-513);
    states[890] = new State(-514);
    states[891] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,892,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[892] = new State(new int[]{55,893});
    states[893] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,56,971,21,270,22,275,11,931,8,944},new int[]{-350,894,-349,985,-342,901,-283,906,-180,205,-147,207,-151,48,-152,51,-341,963,-357,966,-339,974,-15,969,-165,149,-167,150,-166,154,-16,157,-257,972,-294,973,-343,975,-344,978});
    states[894] = new State(new int[]{10,897,32,753,92,-552},new int[]{-253,895});
    states[895] = new State(new int[]{92,896});
    states[896] = new State(-534);
    states[897] = new State(new int[]{32,753,143,47,85,49,86,50,80,52,78,53,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,56,971,21,270,22,275,11,931,8,944,92,-552},new int[]{-253,898,-349,900,-342,901,-283,906,-180,205,-147,207,-151,48,-152,51,-341,963,-357,966,-339,974,-15,969,-165,149,-167,150,-166,154,-16,157,-257,972,-294,973,-343,975,-344,978});
    states[898] = new State(new int[]{92,899});
    states[899] = new State(-535);
    states[900] = new State(-537);
    states[901] = new State(new int[]{39,902});
    states[902] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,903,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[903] = new State(new int[]{5,904});
    states[904] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493,32,-493,92,-493},new int[]{-260,905,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[905] = new State(-538);
    states[906] = new State(new int[]{8,907,100,-655,5,-655});
    states[907] = new State(new int[]{14,912,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,116,368,115,369,143,47,85,49,86,50,80,52,78,53,160,54,87,55,53,919,11,931,8,944},new int[]{-354,908,-352,962,-15,913,-165,149,-167,150,-166,154,-16,157,-199,914,-147,916,-151,48,-152,51,-342,923,-283,924,-180,205,-343,930,-344,961});
    states[908] = new State(new int[]{9,909,10,910,100,928});
    states[909] = new State(new int[]{39,-649,5,-650});
    states[910] = new State(new int[]{14,912,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,116,368,115,369,143,47,85,49,86,50,80,52,78,53,160,54,87,55,53,919,11,931,8,944},new int[]{-352,911,-15,913,-165,149,-167,150,-166,154,-16,157,-199,914,-147,916,-151,48,-152,51,-342,923,-283,924,-180,205,-343,930,-344,961});
    states[911] = new State(-681);
    states[912] = new State(-693);
    states[913] = new State(-694);
    states[914] = new State(new int[]{144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161},new int[]{-15,915,-165,149,-167,150,-166,154,-16,157});
    states[915] = new State(-695);
    states[916] = new State(new int[]{5,917,9,-697,10,-697,100,-697,7,-259,4,-259,123,-259,8,-259});
    states[917] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-276,918,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[918] = new State(-696);
    states[919] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,920,-151,48,-152,51});
    states[920] = new State(new int[]{5,921,9,-699,10,-699,100,-699});
    states[921] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-276,922,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[922] = new State(-698);
    states[923] = new State(-700);
    states[924] = new State(new int[]{8,925});
    states[925] = new State(new int[]{14,912,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,116,368,115,369,143,47,85,49,86,50,80,52,78,53,160,54,87,55,53,919,11,931,8,944},new int[]{-354,926,-352,962,-15,913,-165,149,-167,150,-166,154,-16,157,-199,914,-147,916,-151,48,-152,51,-342,923,-283,924,-180,205,-343,930,-344,961});
    states[926] = new State(new int[]{9,927,10,910,100,928});
    states[927] = new State(-649);
    states[928] = new State(new int[]{14,912,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,116,368,115,369,143,47,85,49,86,50,80,52,78,53,160,54,87,55,53,919,11,931,8,944},new int[]{-352,929,-15,913,-165,149,-167,150,-166,154,-16,157,-199,914,-147,916,-151,48,-152,51,-342,923,-283,924,-180,205,-343,930,-344,961});
    states[929] = new State(-682);
    states[930] = new State(-701);
    states[931] = new State(new int[]{144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,53,938,14,940,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,931,8,944,6,959},new int[]{-355,932,-345,960,-15,936,-165,149,-167,150,-166,154,-16,157,-347,937,-342,941,-283,924,-180,205,-147,207,-151,48,-152,51,-343,942,-344,943});
    states[932] = new State(new int[]{12,933,100,934});
    states[933] = new State(-659);
    states[934] = new State(new int[]{144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,53,938,14,940,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,931,8,944,6,959},new int[]{-345,935,-15,936,-165,149,-167,150,-166,154,-16,157,-347,937,-342,941,-283,924,-180,205,-147,207,-151,48,-152,51,-343,942,-344,943});
    states[935] = new State(-661);
    states[936] = new State(-662);
    states[937] = new State(-663);
    states[938] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,939,-151,48,-152,51});
    states[939] = new State(-669);
    states[940] = new State(-664);
    states[941] = new State(-665);
    states[942] = new State(-666);
    states[943] = new State(-667);
    states[944] = new State(new int[]{14,949,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,116,368,115,369,53,953,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,931,8,944},new int[]{-356,945,-346,958,-15,950,-165,149,-167,150,-166,154,-16,157,-199,951,-342,955,-283,924,-180,205,-147,207,-151,48,-152,51,-343,956,-344,957});
    states[945] = new State(new int[]{9,946,100,947});
    states[946] = new State(-670);
    states[947] = new State(new int[]{14,949,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,116,368,115,369,53,953,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,931,8,944},new int[]{-346,948,-15,950,-165,149,-167,150,-166,154,-16,157,-199,951,-342,955,-283,924,-180,205,-147,207,-151,48,-152,51,-343,956,-344,957});
    states[948] = new State(-679);
    states[949] = new State(-671);
    states[950] = new State(-672);
    states[951] = new State(new int[]{144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161},new int[]{-15,952,-165,149,-167,150,-166,154,-16,157});
    states[952] = new State(-673);
    states[953] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,954,-151,48,-152,51});
    states[954] = new State(-674);
    states[955] = new State(-675);
    states[956] = new State(-676);
    states[957] = new State(-677);
    states[958] = new State(-678);
    states[959] = new State(-668);
    states[960] = new State(-660);
    states[961] = new State(-702);
    states[962] = new State(-680);
    states[963] = new State(new int[]{5,964});
    states[964] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493,32,-493,92,-493},new int[]{-260,965,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[965] = new State(-539);
    states[966] = new State(new int[]{100,967,5,-651});
    states[967] = new State(new int[]{144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,143,47,85,49,86,50,80,52,78,53,160,54,87,55,56,971,21,270,22,275},new int[]{-339,968,-15,969,-165,149,-167,150,-166,154,-16,157,-283,970,-180,205,-147,207,-151,48,-152,51,-257,972,-294,973});
    states[968] = new State(-653);
    states[969] = new State(-654);
    states[970] = new State(-655);
    states[971] = new State(-656);
    states[972] = new State(-657);
    states[973] = new State(-658);
    states[974] = new State(-652);
    states[975] = new State(new int[]{5,976});
    states[976] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493,32,-493,92,-493},new int[]{-260,977,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[977] = new State(-540);
    states[978] = new State(new int[]{39,979,5,983});
    states[979] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,980,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[980] = new State(new int[]{5,981});
    states[981] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493,32,-493,92,-493},new int[]{-260,982,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[982] = new State(-541);
    states[983] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493,32,-493,92,-493},new int[]{-260,984,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[984] = new State(-542);
    states[985] = new State(-536);
    states[986] = new State(-1017);
    states[987] = new State(-1018);
    states[988] = new State(-1019);
    states[989] = new State(-1020);
    states[990] = new State(-1021);
    states[991] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,664,21,270,22,275,76,541,20,599,40,610,18,688,37,697,44,701},new int[]{-103,883,-101,886,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-322,887,-100,673,-323,696});
    states[992] = new State(new int[]{9,1000,143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,1005,135,845,116,368,115,369,63,163},new int[]{-90,993,-66,994,-245,998,-91,225,-81,233,-13,238,-10,248,-14,211,-147,1004,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859,-65,260,-85,1008,-84,263,-95,1009,-243,1010,-244,1011,-246,1018,-136,1014});
    states[993] = new State(new int[]{9,844,13,193,16,197,100,-193});
    states[994] = new State(new int[]{9,995});
    states[995] = new State(new int[]{127,996,92,-196,10,-196,98,-196,101,-196,33,-196,104,-196,2,-196,9,-196,100,-196,12,-196,99,-196,32,-196,85,-196,84,-196,83,-196,82,-196,81,-196,86,-196});
    states[996] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,20,599,18,688,37,697,44,701,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-328,997,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,671,-117,602,-322,672,-100,673,-323,696,-330,863,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[997] = new State(-414);
    states[998] = new State(new int[]{9,999});
    states[999] = new State(-201);
    states[1000] = new State(new int[]{5,454,127,-1006},new int[]{-324,1001});
    states[1001] = new State(new int[]{127,1002});
    states[1002] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,20,599,18,688,37,697,44,701,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-328,1003,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,671,-117,602,-322,672,-100,673,-323,696,-330,863,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[1003] = new State(-413);
    states[1004] = new State(new int[]{4,-168,11,-168,7,-168,142,-168,8,-168,136,-168,138,-168,118,-168,117,-168,131,-168,132,-168,133,-168,134,-168,130,-168,116,-168,115,-168,128,-168,129,-168,120,-168,125,-168,123,-168,121,-168,124,-168,122,-168,137,-168,9,-168,13,-168,16,-168,100,-168,119,-168,5,-207});
    states[1005] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,1005,135,845,116,368,115,369,63,163,9,-197},new int[]{-90,993,-66,1006,-245,998,-91,225,-81,233,-13,238,-10,248,-14,211,-147,1004,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859,-65,260,-85,1008,-84,263,-95,1009,-243,1010,-244,1011,-246,1018,-136,1014});
    states[1006] = new State(new int[]{9,1007});
    states[1007] = new State(-196);
    states[1008] = new State(-199);
    states[1009] = new State(-194);
    states[1010] = new State(-195);
    states[1011] = new State(new int[]{10,1012,9,-202});
    states[1012] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,9,-203},new int[]{-246,1013,-136,1014,-147,1017,-151,48,-152,51});
    states[1013] = new State(-205);
    states[1014] = new State(new int[]{5,1015});
    states[1015] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,1005,135,845,116,368,115,369,63,163},new int[]{-84,1016,-90,264,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859,-95,1009,-243,1010});
    states[1016] = new State(-206);
    states[1017] = new State(-207);
    states[1018] = new State(-204);
    states[1019] = new State(-411);
    states[1020] = new State(-415);
    states[1021] = new State(-404);
    states[1022] = new State(-405);
    states[1023] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,663,8,664,21,270,22,275,76,541,20,599,40,610,5,619,18,688,37,697,44,701},new int[]{-88,1024,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-322,838,-100,673,-323,696});
    states[1024] = new State(-407);
    states[1025] = new State(new int[]{143,1026});
    states[1026] = new State(-559);
    states[1027] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-276,1028,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1028] = new State(-561);
    states[1029] = new State(new int[]{8,1037,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,1030,-151,48,-152,51});
    states[1030] = new State(new int[]{5,1027,137,-562},new int[]{-274,1031});
    states[1031] = new State(new int[]{137,1032});
    states[1032] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,1033,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[1033] = new State(new int[]{87,1025,99,-560},new int[]{-360,1034});
    states[1034] = new State(new int[]{99,1035});
    states[1035] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493,98,-493,101,-493,33,-493,104,-493,2,-493,9,-493,100,-493,12,-493,99,-493,32,-493,84,-493,83,-493,82,-493,81,-493},new int[]{-260,1036,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[1036] = new State(-568);
    states[1037] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-158,1038,-147,814,-151,48,-152,51});
    states[1038] = new State(new int[]{9,1039,100,452});
    states[1039] = new State(new int[]{137,1040});
    states[1040] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,1041,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[1041] = new State(new int[]{87,1025,99,-560},new int[]{-360,1042});
    states[1042] = new State(new int[]{99,1043});
    states[1043] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493,98,-493,101,-493,33,-493,104,-493,2,-493,9,-493,100,-493,12,-493,99,-493,32,-493,84,-493,83,-493,82,-493,81,-493},new int[]{-260,1044,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[1044] = new State(-569);
    states[1045] = new State(new int[]{5,1046});
    states[1046] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,757,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493,98,-493,101,-493,33,-493,104,-493,2,-493},new int[]{-261,1047,-260,22,-4,23,-113,24,-132,373,-111,386,-147,756,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890,-143,1045});
    states[1047] = new State(-492);
    states[1048] = new State(new int[]{79,1056,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,757,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493,92,-493},new int[]{-60,1049,-63,1051,-62,1068,-252,1069,-261,755,-260,22,-4,23,-113,24,-132,373,-111,386,-147,756,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890,-143,1045});
    states[1049] = new State(new int[]{92,1050});
    states[1050] = new State(-576);
    states[1051] = new State(new int[]{10,1053,32,1066,92,-582},new int[]{-254,1052});
    states[1052] = new State(-577);
    states[1053] = new State(new int[]{79,1056,32,1066,92,-582},new int[]{-62,1054,-254,1055});
    states[1054] = new State(-581);
    states[1055] = new State(-578);
    states[1056] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-64,1057,-179,1060,-180,1061,-147,1062,-151,48,-152,51,-140,1063});
    states[1057] = new State(new int[]{99,1058});
    states[1058] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493,32,-493,92,-493},new int[]{-260,1059,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[1059] = new State(-584);
    states[1060] = new State(-585);
    states[1061] = new State(new int[]{7,169,99,-587});
    states[1062] = new State(new int[]{7,-259,99,-259,5,-588});
    states[1063] = new State(new int[]{5,1064});
    states[1064] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-179,1065,-180,1061,-147,207,-151,48,-152,51});
    states[1065] = new State(-586);
    states[1066] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,757,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493,92,-493},new int[]{-252,1067,-261,755,-260,22,-4,23,-113,24,-132,373,-111,386,-147,756,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890,-143,1045});
    states[1067] = new State(new int[]{10,20,92,-583});
    states[1068] = new State(-580);
    states[1069] = new State(new int[]{10,20,92,-579});
    states[1070] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,1071,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[1071] = new State(new int[]{99,1072});
    states[1072] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493,98,-493,101,-493,33,-493,104,-493,2,-493,9,-493,100,-493,12,-493,99,-493,32,-493,84,-493,83,-493,82,-493,81,-493},new int[]{-260,1073,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[1073] = new State(-571);
    states[1074] = new State(-556);
    states[1075] = new State(-565);
    states[1076] = new State(-566);
    states[1077] = new State(-563);
    states[1078] = new State(new int[]{5,1079,100,1081});
    states[1079] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493,32,-493,92,-493},new int[]{-260,1080,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[1080] = new State(-548);
    states[1081] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-110,1082,-94,1083,-90,192,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[1082] = new State(-550);
    states[1083] = new State(-551);
    states[1084] = new State(-549);
    states[1085] = new State(new int[]{92,1086});
    states[1086] = new State(-545);
    states[1087] = new State(-546);
    states[1088] = new State(new int[]{9,1089,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-326,1092,-327,1096,-158,450,-147,814,-151,48,-152,51});
    states[1089] = new State(new int[]{127,1090});
    states[1090] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,705,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-329,1091,-212,704,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-4,724,-330,725,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[1091] = new State(-1001);
    states[1092] = new State(new int[]{9,1093,10,448});
    states[1093] = new State(new int[]{127,1094});
    states[1094] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,705,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-329,1095,-212,704,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-4,724,-330,725,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[1095] = new State(-1002);
    states[1096] = new State(-1003);
    states[1097] = new State(new int[]{9,1098,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-326,1102,-327,1096,-158,450,-147,814,-151,48,-152,51});
    states[1098] = new State(new int[]{5,675,127,-1008},new int[]{-325,1099});
    states[1099] = new State(new int[]{127,1100});
    states[1100] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,20,599,18,688,37,697,44,701,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-328,1101,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,671,-117,602,-322,672,-100,673,-323,696,-330,863,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[1101] = new State(-998);
    states[1102] = new State(new int[]{9,1103,10,448});
    states[1103] = new State(new int[]{5,675,127,-1008},new int[]{-325,1104});
    states[1104] = new State(new int[]{127,1105});
    states[1105] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,20,599,18,688,37,697,44,701,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-328,1106,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,671,-117,602,-322,672,-100,673,-323,696,-330,863,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[1106] = new State(-999);
    states[1107] = new State(new int[]{5,1108,10,1120,8,-793,7,-793,142,-793,4,-793,15,-793,110,-793,111,-793,112,-793,113,-793,114,-793,138,-793,136,-793,118,-793,117,-793,131,-793,132,-793,133,-793,134,-793,130,-793,116,-793,115,-793,128,-793,129,-793,126,-793,6,-793,120,-793,125,-793,123,-793,121,-793,124,-793,122,-793,137,-793,135,-793,16,-793,9,-793,100,-793,13,-793,119,-793,11,-793,17,-793});
    states[1108] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-275,1109,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1109] = new State(new int[]{9,1110,10,1114});
    states[1110] = new State(new int[]{5,675,127,-1008},new int[]{-325,1111});
    states[1111] = new State(new int[]{127,1112});
    states[1112] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,20,599,18,688,37,697,44,701,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-328,1113,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,671,-117,602,-322,672,-100,673,-323,696,-330,863,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[1113] = new State(-988);
    states[1114] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-326,1115,-327,1096,-158,450,-147,814,-151,48,-152,51});
    states[1115] = new State(new int[]{9,1116,10,448});
    states[1116] = new State(new int[]{5,675,127,-1008},new int[]{-325,1117});
    states[1117] = new State(new int[]{127,1118});
    states[1118] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,20,599,18,688,37,697,44,701,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-328,1119,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,671,-117,602,-322,672,-100,673,-323,696,-330,863,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[1119] = new State(-990);
    states[1120] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-326,1121,-327,1096,-158,450,-147,814,-151,48,-152,51});
    states[1121] = new State(new int[]{9,1122,10,448});
    states[1122] = new State(new int[]{5,675,127,-1008},new int[]{-325,1123});
    states[1123] = new State(new int[]{127,1124});
    states[1124] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,20,599,18,688,37,697,44,701,91,17,40,728,54,764,97,759,35,769,36,796,72,869,25,743,102,786,60,877,47,793,74,991},new int[]{-328,1125,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,671,-117,602,-322,672,-100,673,-323,696,-330,863,-255,726,-153,727,-318,864,-247,865,-124,866,-123,867,-125,868,-35,986,-301,987,-169,988,-248,989,-126,990});
    states[1125] = new State(-989);
    states[1126] = new State(new int[]{5,1127,7,-259,8,-259,123,-259,12,-259,100,-259});
    states[1127] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-9,1128,-180,655,-147,207,-151,48,-152,51,-300,1129});
    states[1128] = new State(-216);
    states[1129] = new State(new int[]{8,658,12,-642,100,-642},new int[]{-70,1130});
    states[1130] = new State(-782);
    states[1131] = new State(-213);
    states[1132] = new State(-209);
    states[1133] = new State(-472);
    states[1134] = new State(-690);
    states[1135] = new State(new int[]{8,1136});
    states[1136] = new State(new int[]{14,562,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,53,564,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,931,8,944},new int[]{-353,1137,-351,1143,-15,563,-165,149,-167,150,-166,154,-16,157,-340,1134,-283,1135,-180,205,-147,207,-151,48,-152,51,-343,1141,-344,1142});
    states[1137] = new State(new int[]{9,1138,10,560,100,1139});
    states[1138] = new State(-648);
    states[1139] = new State(new int[]{14,562,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,53,564,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,931,8,944},new int[]{-351,1140,-15,563,-165,149,-167,150,-166,154,-16,157,-340,1134,-283,1135,-180,205,-147,207,-151,48,-152,51,-343,1141,-344,1142});
    states[1140] = new State(-685);
    states[1141] = new State(-691);
    states[1142] = new State(-692);
    states[1143] = new State(-683);
    states[1144] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,18,688},new int[]{-102,1145,-101,1146,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-100,723});
    states[1145] = new State(-985);
    states[1146] = new State(-982);
    states[1147] = new State(-984);
    states[1148] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,5,619},new int[]{-120,1149,-105,1151,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,623,-267,598});
    states[1149] = new State(new int[]{12,1150});
    states[1150] = new State(-802);
    states[1151] = new State(new int[]{5,322,6,34});
    states[1152] = new State(new int[]{53,1160},new int[]{-337,1153});
    states[1153] = new State(new int[]{9,1154,100,1157});
    states[1154] = new State(new int[]{110,1155});
    states[1155] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-87,1156,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[1156] = new State(-519);
    states[1157] = new State(new int[]{53,1158});
    states[1158] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,1159,-151,48,-152,51});
    states[1159] = new State(-526);
    states[1160] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,1161,-151,48,-152,51});
    states[1161] = new State(-525);
    states[1162] = new State(new int[]{148,1166,150,1167,151,1168,152,1169,154,1170,153,1171,107,-827,91,-827,59,-827,29,-827,66,-827,50,-827,53,-827,62,-827,11,-827,28,-827,26,-827,44,-827,37,-827,19,-827,30,-827,31,-827,46,-827,27,-827,92,-827,84,-827,83,-827,82,-827,81,-827,23,-827,149,-827,41,-827},new int[]{-206,1163,-209,1172});
    states[1163] = new State(new int[]{10,1164});
    states[1164] = new State(new int[]{148,1166,150,1167,151,1168,152,1169,154,1170,153,1171,107,-828,91,-828,59,-828,29,-828,66,-828,50,-828,53,-828,62,-828,11,-828,28,-828,26,-828,44,-828,37,-828,19,-828,30,-828,31,-828,46,-828,27,-828,92,-828,84,-828,83,-828,82,-828,81,-828,23,-828,149,-828,41,-828},new int[]{-209,1165});
    states[1165] = new State(-832);
    states[1166] = new State(-844);
    states[1167] = new State(-845);
    states[1168] = new State(-846);
    states[1169] = new State(-847);
    states[1170] = new State(-848);
    states[1171] = new State(-849);
    states[1172] = new State(-831);
    states[1173] = new State(-375);
    states[1174] = new State(-446);
    states[1175] = new State(-447);
    states[1176] = new State(new int[]{8,-452,110,-452,10,-452,11,-452,5,-452,7,-449});
    states[1177] = new State(new int[]{123,1179,8,-455,110,-455,10,-455,7,-455,11,-455,5,-455},new int[]{-155,1178});
    states[1178] = new State(-456);
    states[1179] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-158,1180,-147,814,-151,48,-152,51});
    states[1180] = new State(new int[]{121,1181,100,452});
    states[1181] = new State(-320);
    states[1182] = new State(-457);
    states[1183] = new State(new int[]{123,1179,8,-453,110,-453,10,-453,11,-453,5,-453},new int[]{-155,1184});
    states[1184] = new State(-454);
    states[1185] = new State(new int[]{7,1186});
    states[1186] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-142,1187,-149,1188,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183});
    states[1187] = new State(-448);
    states[1188] = new State(-451);
    states[1189] = new State(-450);
    states[1190] = new State(-437);
    states[1191] = new State(new int[]{44,1321,37,1356},new int[]{-216,1192,-228,1193,-225,1320,-229,1355});
    states[1192] = new State(-439);
    states[1193] = new State(new int[]{107,1311,59,-70,29,-70,66,-70,50,-70,53,-70,62,-70,91,-70},new int[]{-176,1194,-43,1195,-39,1198,-61,1310});
    states[1194] = new State(-440);
    states[1195] = new State(new int[]{91,17},new int[]{-255,1196});
    states[1196] = new State(new int[]{10,1197});
    states[1197] = new State(-467);
    states[1198] = new State(new int[]{59,1201,29,1222,66,1226,50,1438,53,1453,62,1455,91,-69},new int[]{-46,1199,-168,1200,-29,1207,-52,1224,-288,1228,-309,1440});
    states[1199] = new State(-71);
    states[1200] = new State(-87);
    states[1201] = new State(new int[]{155,738,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-156,1202,-143,1206,-147,739,-151,48,-152,51});
    states[1202] = new State(new int[]{10,1203,100,1204});
    states[1203] = new State(-96);
    states[1204] = new State(new int[]{155,738,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-143,1205,-147,739,-151,48,-152,51});
    states[1205] = new State(-98);
    states[1206] = new State(-97);
    states[1207] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,59,-88,29,-88,66,-88,50,-88,53,-88,62,-88,91,-88},new int[]{-27,1208,-28,1209,-141,1211,-147,1221,-151,48,-152,51});
    states[1208] = new State(-102);
    states[1209] = new State(new int[]{10,1210});
    states[1210] = new State(-112);
    states[1211] = new State(new int[]{120,1212,5,1217});
    states[1212] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,1215,135,845,116,368,115,369,63,163},new int[]{-109,1213,-90,1214,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859,-95,1216});
    states[1213] = new State(-113);
    states[1214] = new State(new int[]{13,193,16,197,10,-115,92,-115,84,-115,83,-115,82,-115,81,-115});
    states[1215] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,1005,135,845,116,368,115,369,63,163,9,-197},new int[]{-90,993,-66,1006,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859,-65,260,-85,1008,-84,263,-95,1009,-243,1010});
    states[1216] = new State(-116);
    states[1217] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-276,1218,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1218] = new State(new int[]{120,1219});
    states[1219] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,1005,135,845,116,368,115,369,63,163},new int[]{-84,1220,-90,264,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859,-95,1009,-243,1010});
    states[1220] = new State(-114);
    states[1221] = new State(-117);
    states[1222] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-27,1223,-28,1209,-141,1211,-147,1221,-151,48,-152,51});
    states[1223] = new State(-101);
    states[1224] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,59,-89,29,-89,66,-89,50,-89,53,-89,62,-89,91,-89},new int[]{-27,1225,-28,1209,-141,1211,-147,1221,-151,48,-152,51});
    states[1225] = new State(-104);
    states[1226] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-27,1227,-28,1209,-141,1211,-147,1221,-151,48,-152,51});
    states[1227] = new State(-103);
    states[1228] = new State(new int[]{11,649,59,-90,29,-90,66,-90,50,-90,53,-90,62,-90,91,-90,143,-211,85,-211,86,-211,80,-211,78,-211,160,-211,87,-211},new int[]{-49,1229,-6,1230,-250,1132});
    states[1229] = new State(-106);
    states[1230] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,649},new int[]{-50,1231,-250,495,-144,1232,-147,1430,-151,48,-152,51,-145,1435});
    states[1231] = new State(-208);
    states[1232] = new State(new int[]{120,1233});
    states[1233] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625,68,1424,69,1425,148,1426,27,1427,28,1428,26,-302,43,-302,64,-302},new int[]{-286,1234,-276,1236,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582,-30,1237,-21,1238,-22,1422,-20,1429});
    states[1234] = new State(new int[]{10,1235});
    states[1235] = new State(-217);
    states[1236] = new State(-222);
    states[1237] = new State(-223);
    states[1238] = new State(new int[]{26,1416,43,1417,64,1418},new int[]{-290,1239});
    states[1239] = new State(new int[]{8,1337,23,-314,11,-314,92,-314,84,-314,83,-314,82,-314,81,-314,29,-314,143,-314,85,-314,86,-314,80,-314,78,-314,160,-314,87,-314,62,-314,28,-314,26,-314,44,-314,37,-314,19,-314,30,-314,31,-314,46,-314,27,-314,10,-314},new int[]{-183,1240});
    states[1240] = new State(new int[]{23,1328,11,-321,92,-321,84,-321,83,-321,82,-321,81,-321,29,-321,143,-321,85,-321,86,-321,80,-321,78,-321,160,-321,87,-321,62,-321,28,-321,26,-321,44,-321,37,-321,19,-321,30,-321,31,-321,46,-321,27,-321,10,-321},new int[]{-317,1241,-316,1326,-315,1348});
    states[1241] = new State(new int[]{11,649,10,-312,92,-338,84,-338,83,-338,82,-338,81,-338,29,-211,143,-211,85,-211,86,-211,80,-211,78,-211,160,-211,87,-211,62,-211,28,-211,26,-211,44,-211,37,-211,19,-211,30,-211,31,-211,46,-211,27,-211},new int[]{-26,1242,-25,1243,-32,1249,-34,486,-45,1250,-6,1251,-250,1132,-33,1413,-54,1415,-53,492,-55,1414});
    states[1242] = new State(-295);
    states[1243] = new State(new int[]{92,1244,84,1245,83,1246,82,1247,81,1248},new int[]{-7,484});
    states[1244] = new State(-313);
    states[1245] = new State(-334);
    states[1246] = new State(-335);
    states[1247] = new State(-336);
    states[1248] = new State(-337);
    states[1249] = new State(-332);
    states[1250] = new State(-346);
    states[1251] = new State(new int[]{29,1253,143,47,85,49,86,50,80,52,78,53,160,54,87,55,62,1257,28,1373,26,1374,11,649,44,1321,37,1356,19,1376,30,1384,31,1391,46,1398,27,1407},new int[]{-51,1252,-250,495,-222,494,-219,496,-258,497,-312,1255,-311,1256,-158,815,-147,814,-151,48,-152,51,-3,1261,-230,1375,-228,1309,-225,1320,-229,1355,-227,1382,-215,1395,-216,1396,-218,1397});
    states[1252] = new State(-348);
    states[1253] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-28,1254,-141,1211,-147,1221,-151,48,-152,51});
    states[1254] = new State(-353);
    states[1255] = new State(-354);
    states[1256] = new State(-358);
    states[1257] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-158,1258,-147,814,-151,48,-152,51});
    states[1258] = new State(new int[]{5,1259,100,452});
    states[1259] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-276,1260,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1260] = new State(-359);
    states[1261] = new State(new int[]{30,500,19,1191,46,1263,27,1301,143,47,85,49,86,50,80,52,78,53,160,54,87,55,62,1257,44,1321,37,1356},new int[]{-312,1262,-230,499,-216,1190,-311,1256,-158,815,-147,814,-151,48,-152,51,-228,1309,-225,1320,-229,1355});
    states[1262] = new State(-355);
    states[1263] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-170,1264,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[1264] = new State(new int[]{11,1292,5,-389},new int[]{-233,1265,-238,1289});
    states[1265] = new State(new int[]{85,1278,86,1284,10,-396},new int[]{-202,1266});
    states[1266] = new State(new int[]{10,1267});
    states[1267] = new State(new int[]{63,1272,153,1274,152,1275,148,1276,151,1277,11,-386,28,-386,26,-386,44,-386,37,-386,19,-386,30,-386,31,-386,46,-386,27,-386,92,-386,84,-386,83,-386,82,-386,81,-386},new int[]{-205,1268,-210,1269});
    states[1268] = new State(-380);
    states[1269] = new State(new int[]{10,1270});
    states[1270] = new State(new int[]{63,1272,11,-386,28,-386,26,-386,44,-386,37,-386,19,-386,30,-386,31,-386,46,-386,27,-386,92,-386,84,-386,83,-386,82,-386,81,-386},new int[]{-205,1271});
    states[1271] = new State(-381);
    states[1272] = new State(new int[]{10,1273});
    states[1273] = new State(-387);
    states[1274] = new State(-850);
    states[1275] = new State(-851);
    states[1276] = new State(-852);
    states[1277] = new State(-853);
    states[1278] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,663,8,664,21,270,22,275,76,541,20,599,40,610,5,619,18,688,37,697,44,701,10,-395},new int[]{-114,1279,-88,1283,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-322,838,-100,673,-323,696});
    states[1279] = new State(new int[]{86,1281,10,-399},new int[]{-203,1280});
    states[1280] = new State(-397);
    states[1281] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493},new int[]{-260,1282,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[1282] = new State(-400);
    states[1283] = new State(-394);
    states[1284] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493},new int[]{-260,1285,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[1285] = new State(new int[]{85,1287,10,-401},new int[]{-204,1286});
    states[1286] = new State(-398);
    states[1287] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,663,8,664,21,270,22,275,76,541,20,599,40,610,5,619,18,688,37,697,44,701,10,-395},new int[]{-114,1288,-88,1283,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-322,838,-100,673,-323,696});
    states[1288] = new State(-402);
    states[1289] = new State(new int[]{5,1290});
    states[1290] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-275,1291,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1291] = new State(-388);
    states[1292] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-237,1293,-236,1300,-158,1297,-147,814,-151,48,-152,51});
    states[1293] = new State(new int[]{12,1294,10,1295});
    states[1294] = new State(-390);
    states[1295] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-236,1296,-158,1297,-147,814,-151,48,-152,51});
    states[1296] = new State(-392);
    states[1297] = new State(new int[]{5,1298,100,452});
    states[1298] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-275,1299,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1299] = new State(-393);
    states[1300] = new State(-391);
    states[1301] = new State(new int[]{46,1302});
    states[1302] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-170,1303,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[1303] = new State(new int[]{11,1292,5,-389},new int[]{-233,1304,-238,1289});
    states[1304] = new State(new int[]{110,1307,10,-385},new int[]{-211,1305});
    states[1305] = new State(new int[]{10,1306});
    states[1306] = new State(-383);
    states[1307] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-87,1308,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[1308] = new State(-384);
    states[1309] = new State(new int[]{107,1311,11,-369,28,-369,26,-369,44,-369,37,-369,19,-369,30,-369,31,-369,46,-369,27,-369,92,-369,84,-369,83,-369,82,-369,81,-369,59,-70,29,-70,66,-70,50,-70,53,-70,62,-70,91,-70},new int[]{-176,1194,-43,1195,-39,1198,-61,1310});
    states[1310] = new State(-468);
    states[1311] = new State(new int[]{10,1319,143,47,85,49,86,50,80,52,78,53,160,54,87,55,144,152,147,153,145,155,146,156},new int[]{-108,1312,-147,1316,-151,48,-152,51,-165,1317,-167,150,-166,154});
    states[1312] = new State(new int[]{80,1313,10,1318});
    states[1313] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,144,152,147,153,145,155,146,156},new int[]{-108,1314,-147,1316,-151,48,-152,51,-165,1317,-167,150,-166,154});
    states[1314] = new State(new int[]{10,1315});
    states[1315] = new State(-461);
    states[1316] = new State(-464);
    states[1317] = new State(-465);
    states[1318] = new State(-462);
    states[1319] = new State(-463);
    states[1320] = new State(-370);
    states[1321] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-171,1322,-170,1174,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[1322] = new State(new int[]{8,585,10,-469,110,-469},new int[]{-128,1323});
    states[1323] = new State(new int[]{10,1353,110,-829},new int[]{-207,1324,-208,1349});
    states[1324] = new State(new int[]{23,1328,107,-321,91,-321,59,-321,29,-321,66,-321,50,-321,53,-321,62,-321,11,-321,28,-321,26,-321,44,-321,37,-321,19,-321,30,-321,31,-321,46,-321,27,-321,92,-321,84,-321,83,-321,82,-321,81,-321,149,-321,41,-321},new int[]{-317,1325,-316,1326,-315,1348});
    states[1325] = new State(-458);
    states[1326] = new State(new int[]{23,1328,11,-322,92,-322,84,-322,83,-322,82,-322,81,-322,29,-322,143,-322,85,-322,86,-322,80,-322,78,-322,160,-322,87,-322,62,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322,10,-322,107,-322,91,-322,59,-322,66,-322,50,-322,53,-322,149,-322,41,-322},new int[]{-315,1327});
    states[1327] = new State(-324);
    states[1328] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-158,1329,-147,814,-151,48,-152,51});
    states[1329] = new State(new int[]{5,1330,100,452});
    states[1330] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,1336,49,569,34,573,73,577,44,583,37,625,26,1345,30,1346},new int[]{-287,1331,-285,1347,-276,1335,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1331] = new State(new int[]{10,1332,100,1333});
    states[1332] = new State(-325);
    states[1333] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,1336,49,569,34,573,73,577,44,583,37,625,26,1345,30,1346},new int[]{-285,1334,-276,1335,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1334] = new State(-327);
    states[1335] = new State(-328);
    states[1336] = new State(new int[]{8,1337,10,-330,100,-330,23,-314,11,-314,92,-314,84,-314,83,-314,82,-314,81,-314,29,-314,143,-314,85,-314,86,-314,80,-314,78,-314,160,-314,87,-314,62,-314,28,-314,26,-314,44,-314,37,-314,19,-314,30,-314,31,-314,46,-314,27,-314},new int[]{-183,480});
    states[1337] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-182,1338,-181,1344,-180,1342,-147,207,-151,48,-152,51,-300,1343});
    states[1338] = new State(new int[]{9,1339,100,1340});
    states[1339] = new State(-315);
    states[1340] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-181,1341,-180,1342,-147,207,-151,48,-152,51,-300,1343});
    states[1341] = new State(-317);
    states[1342] = new State(new int[]{7,169,123,174,9,-318,100,-318},new int[]{-298,657});
    states[1343] = new State(-319);
    states[1344] = new State(-316);
    states[1345] = new State(-329);
    states[1346] = new State(-331);
    states[1347] = new State(-326);
    states[1348] = new State(-323);
    states[1349] = new State(new int[]{110,1350});
    states[1350] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493},new int[]{-260,1351,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[1351] = new State(new int[]{10,1352});
    states[1352] = new State(-443);
    states[1353] = new State(new int[]{148,1166,150,1167,151,1168,152,1169,154,1170,153,1171,23,-827,107,-827,91,-827,59,-827,29,-827,66,-827,50,-827,53,-827,62,-827,11,-827,28,-827,26,-827,44,-827,37,-827,19,-827,30,-827,31,-827,46,-827,27,-827,92,-827,84,-827,83,-827,82,-827,81,-827,149,-827},new int[]{-206,1354,-209,1172});
    states[1354] = new State(new int[]{10,1164,110,-830});
    states[1355] = new State(-371);
    states[1356] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-170,1357,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[1357] = new State(new int[]{8,585,5,-469,10,-469,110,-469},new int[]{-128,1358});
    states[1358] = new State(new int[]{5,1361,10,1353,110,-829},new int[]{-207,1359,-208,1369});
    states[1359] = new State(new int[]{23,1328,107,-321,91,-321,59,-321,29,-321,66,-321,50,-321,53,-321,62,-321,11,-321,28,-321,26,-321,44,-321,37,-321,19,-321,30,-321,31,-321,46,-321,27,-321,92,-321,84,-321,83,-321,82,-321,81,-321,149,-321,41,-321},new int[]{-317,1360,-316,1326,-315,1348});
    states[1360] = new State(-459);
    states[1361] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-275,1362,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1362] = new State(new int[]{10,1353,110,-829},new int[]{-207,1363,-208,1365});
    states[1363] = new State(new int[]{23,1328,107,-321,91,-321,59,-321,29,-321,66,-321,50,-321,53,-321,62,-321,11,-321,28,-321,26,-321,44,-321,37,-321,19,-321,30,-321,31,-321,46,-321,27,-321,92,-321,84,-321,83,-321,82,-321,81,-321,149,-321,41,-321},new int[]{-317,1364,-316,1326,-315,1348});
    states[1364] = new State(-460);
    states[1365] = new State(new int[]{110,1366});
    states[1366] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,664,21,270,22,275,76,541,20,599,40,610,18,688,37,697,44,701},new int[]{-103,1367,-101,886,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-322,887,-100,673,-323,696});
    states[1367] = new State(new int[]{10,1368});
    states[1368] = new State(-441);
    states[1369] = new State(new int[]{110,1370});
    states[1370] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,664,21,270,22,275,76,541,20,599,40,610,18,688,37,697,44,701},new int[]{-103,1371,-101,886,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-322,887,-100,673,-323,696});
    states[1371] = new State(new int[]{10,1372});
    states[1372] = new State(-442);
    states[1373] = new State(-356);
    states[1374] = new State(-357);
    states[1375] = new State(-365);
    states[1376] = new State(new int[]{28,1373,26,1374,44,1321,37,1356},new int[]{-3,1377,-230,1380,-216,1381,-228,1309,-225,1320,-229,1355});
    states[1377] = new State(new int[]{44,1321,37,1356},new int[]{-230,1378,-216,1379,-228,1309,-225,1320,-229,1355});
    states[1378] = new State(-366);
    states[1379] = new State(-438);
    states[1380] = new State(-367);
    states[1381] = new State(-436);
    states[1382] = new State(new int[]{107,1311,11,-368,28,-368,26,-368,44,-368,37,-368,19,-368,30,-368,31,-368,46,-368,27,-368,92,-368,84,-368,83,-368,82,-368,81,-368,59,-70,29,-70,66,-70,50,-70,53,-70,62,-70,91,-70},new int[]{-176,1383,-43,1195,-39,1198,-61,1310});
    states[1383] = new State(-420);
    states[1384] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,8,-376,110,-376,10,-376},new int[]{-172,1385,-171,1173,-170,1174,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[1385] = new State(new int[]{8,585,110,-469,10,-469},new int[]{-128,1386});
    states[1386] = new State(new int[]{110,1388,10,1162},new int[]{-207,1387});
    states[1387] = new State(-372);
    states[1388] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493},new int[]{-260,1389,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[1389] = new State(new int[]{10,1390});
    states[1390] = new State(-421);
    states[1391] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,8,-376,10,-376},new int[]{-172,1392,-171,1173,-170,1174,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[1392] = new State(new int[]{8,585,10,-469},new int[]{-128,1393});
    states[1393] = new State(new int[]{10,1162},new int[]{-207,1394});
    states[1394] = new State(-374);
    states[1395] = new State(-362);
    states[1396] = new State(-435);
    states[1397] = new State(-363);
    states[1398] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-170,1399,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[1399] = new State(new int[]{11,1292,5,-389},new int[]{-233,1400,-238,1289});
    states[1400] = new State(new int[]{85,1278,86,1284,10,-396},new int[]{-202,1401});
    states[1401] = new State(new int[]{10,1402});
    states[1402] = new State(new int[]{63,1272,153,1274,152,1275,148,1276,151,1277,11,-386,28,-386,26,-386,44,-386,37,-386,19,-386,30,-386,31,-386,46,-386,27,-386,92,-386,84,-386,83,-386,82,-386,81,-386},new int[]{-205,1403,-210,1404});
    states[1403] = new State(-378);
    states[1404] = new State(new int[]{10,1405});
    states[1405] = new State(new int[]{63,1272,11,-386,28,-386,26,-386,44,-386,37,-386,19,-386,30,-386,31,-386,46,-386,27,-386,92,-386,84,-386,83,-386,82,-386,81,-386},new int[]{-205,1406});
    states[1406] = new State(-379);
    states[1407] = new State(new int[]{46,1408});
    states[1408] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-170,1409,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[1409] = new State(new int[]{11,1292,5,-389},new int[]{-233,1410,-238,1289});
    states[1410] = new State(new int[]{110,1307,10,-385},new int[]{-211,1411});
    states[1411] = new State(new int[]{10,1412});
    states[1412] = new State(-382);
    states[1413] = new State(new int[]{11,649,92,-340,84,-340,83,-340,82,-340,81,-340,28,-211,26,-211,44,-211,37,-211,19,-211,30,-211,31,-211,46,-211,27,-211},new int[]{-54,491,-53,492,-6,493,-250,1132,-55,1414});
    states[1414] = new State(-352);
    states[1415] = new State(-349);
    states[1416] = new State(-306);
    states[1417] = new State(-307);
    states[1418] = new State(new int[]{26,1419,48,1420,43,1421,8,-308,23,-308,11,-308,92,-308,84,-308,83,-308,82,-308,81,-308,29,-308,143,-308,85,-308,86,-308,80,-308,78,-308,160,-308,87,-308,62,-308,28,-308,44,-308,37,-308,19,-308,30,-308,31,-308,46,-308,27,-308,10,-308});
    states[1419] = new State(-309);
    states[1420] = new State(-310);
    states[1421] = new State(-311);
    states[1422] = new State(new int[]{68,1424,69,1425,148,1426,27,1427,28,1428,26,-303,43,-303,64,-303},new int[]{-20,1423});
    states[1423] = new State(-305);
    states[1424] = new State(-297);
    states[1425] = new State(-298);
    states[1426] = new State(-299);
    states[1427] = new State(-300);
    states[1428] = new State(-301);
    states[1429] = new State(-304);
    states[1430] = new State(new int[]{123,1432,120,-219},new int[]{-155,1431});
    states[1431] = new State(-220);
    states[1432] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-158,1433,-147,814,-151,48,-152,51});
    states[1433] = new State(new int[]{122,1434,121,1181,100,452});
    states[1434] = new State(-221);
    states[1435] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625,68,1424,69,1425,148,1426,27,1427,28,1428,26,-302,43,-302,64,-302},new int[]{-286,1436,-276,1236,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582,-30,1237,-21,1238,-22,1422,-20,1429});
    states[1436] = new State(new int[]{10,1437});
    states[1437] = new State(-218);
    states[1438] = new State(new int[]{11,649,143,-211,85,-211,86,-211,80,-211,78,-211,160,-211,87,-211},new int[]{-49,1439,-6,1230,-250,1132});
    states[1439] = new State(-105);
    states[1440] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,8,1445,59,-91,29,-91,66,-91,50,-91,53,-91,62,-91,91,-91},new int[]{-313,1441,-310,1442,-311,1443,-158,815,-147,814,-151,48,-152,51});
    states[1441] = new State(-111);
    states[1442] = new State(-107);
    states[1443] = new State(new int[]{10,1444});
    states[1444] = new State(-403);
    states[1445] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,1446,-151,48,-152,51});
    states[1446] = new State(new int[]{100,1447});
    states[1447] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-158,1448,-147,814,-151,48,-152,51});
    states[1448] = new State(new int[]{9,1449,100,452});
    states[1449] = new State(new int[]{110,1450});
    states[1450] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,1451,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[1451] = new State(new int[]{10,1452});
    states[1452] = new State(-108);
    states[1453] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,8,1445},new int[]{-313,1454,-310,1442,-311,1443,-158,815,-147,814,-151,48,-152,51});
    states[1454] = new State(-109);
    states[1455] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,8,1445},new int[]{-313,1456,-310,1442,-311,1443,-158,815,-147,814,-151,48,-152,51});
    states[1456] = new State(-110);
    states[1457] = new State(-244);
    states[1458] = new State(-245);
    states[1459] = new State(new int[]{127,467,121,-246,100,-246,120,-246,9,-246,8,-246,138,-246,136,-246,118,-246,117,-246,131,-246,132,-246,133,-246,134,-246,130,-246,116,-246,115,-246,128,-246,129,-246,126,-246,6,-246,5,-246,125,-246,123,-246,124,-246,122,-246,137,-246,135,-246,16,-246,92,-246,10,-246,98,-246,101,-246,33,-246,104,-246,2,-246,12,-246,99,-246,32,-246,85,-246,84,-246,83,-246,82,-246,81,-246,86,-246,13,-246,76,-246,51,-246,58,-246,141,-246,143,-246,80,-246,78,-246,160,-246,87,-246,45,-246,42,-246,21,-246,22,-246,144,-246,147,-246,145,-246,146,-246,155,-246,158,-246,157,-246,156,-246,57,-246,91,-246,40,-246,25,-246,97,-246,54,-246,35,-246,55,-246,102,-246,47,-246,36,-246,53,-246,60,-246,74,-246,72,-246,38,-246,70,-246,71,-246,110,-246});
    states[1460] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,1461,-151,48,-152,51});
    states[1461] = new State(new int[]{110,1462});
    states[1462] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-87,514,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[1463] = new State(new int[]{110,1464,127,525,8,-793,7,-793,142,-793,4,-793,15,-793,138,-793,136,-793,118,-793,117,-793,131,-793,132,-793,133,-793,134,-793,130,-793,116,-793,115,-793,128,-793,129,-793,126,-793,6,-793,5,-793,120,-793,125,-793,123,-793,121,-793,124,-793,122,-793,137,-793,135,-793,16,-793,100,-793,9,-793,13,-793,119,-793,11,-793,17,-793});
    states[1464] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,1465,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[1465] = new State(-601);
    states[1466] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,100,-603,9,-603},new int[]{-147,431,-151,48,-152,51});
    states[1467] = new State(-602);
    states[1468] = new State(-593);
    states[1469] = new State(-780);
    states[1470] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,679,12,-278,100,-278},new int[]{-271,1471,-272,1472,-93,181,-106,290,-107,291,-180,460,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154});
    states[1471] = new State(-276);
    states[1472] = new State(-277);
    states[1473] = new State(-275);
    states[1474] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-276,1475,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1475] = new State(-274);
    states[1476] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,24,336},new int[]{-283,1477,-278,1478,-180,205,-147,207,-151,48,-152,51,-270,477});
    states[1477] = new State(-737);
    states[1478] = new State(-738);
    states[1479] = new State(-751);
    states[1480] = new State(-752);
    states[1481] = new State(-753);
    states[1482] = new State(-754);
    states[1483] = new State(-755);
    states[1484] = new State(-756);
    states[1485] = new State(-757);
    states[1486] = new State(-239);
    states[1487] = new State(-235);
    states[1488] = new State(-626);
    states[1489] = new State(new int[]{8,1490});
    states[1490] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-333,1491,-332,1499,-147,1495,-151,48,-152,51,-101,1498,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[1491] = new State(new int[]{9,1492,100,1493});
    states[1492] = new State(-637);
    states[1493] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-332,1494,-147,1495,-151,48,-152,51,-101,1498,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[1494] = new State(-641);
    states[1495] = new State(new int[]{110,1496,8,-793,7,-793,142,-793,4,-793,15,-793,138,-793,136,-793,118,-793,117,-793,131,-793,132,-793,133,-793,134,-793,130,-793,116,-793,115,-793,128,-793,129,-793,126,-793,6,-793,120,-793,125,-793,123,-793,121,-793,124,-793,122,-793,137,-793,135,-793,16,-793,9,-793,100,-793,13,-793,119,-793,11,-793,17,-793});
    states[1496] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610},new int[]{-101,1497,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609});
    states[1497] = new State(-638);
    states[1498] = new State(-639);
    states[1499] = new State(-640);
    states[1500] = new State(new int[]{13,193,16,197,5,-705,12,-705});
    states[1501] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-90,1502,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[1502] = new State(new int[]{13,193,16,197,100,-188,9,-188,12,-188,5,-188});
    states[1503] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163,5,-706,12,-706},new int[]{-122,1504,-90,1500,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[1504] = new State(new int[]{5,1505,12,-712});
    states[1505] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,824,56,827,141,828,8,842,135,845,116,368,115,369,63,163},new int[]{-90,1506,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,851,-173,849,-57,850,-265,857,-269,858,-11,853,-241,859});
    states[1506] = new State(new int[]{13,193,16,197,12,-714});
    states[1507] = new State(-185);
    states[1508] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156},new int[]{-93,1509,-106,290,-107,291,-180,460,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154});
    states[1509] = new State(new int[]{116,234,115,235,128,236,129,237,13,-248,121,-248,100,-248,120,-248,9,-248,8,-248,138,-248,136,-248,118,-248,117,-248,131,-248,132,-248,133,-248,134,-248,130,-248,126,-248,6,-248,5,-248,125,-248,123,-248,124,-248,122,-248,137,-248,135,-248,16,-248,92,-248,10,-248,98,-248,101,-248,33,-248,104,-248,2,-248,12,-248,99,-248,32,-248,85,-248,84,-248,83,-248,82,-248,81,-248,86,-248,76,-248,51,-248,58,-248,141,-248,143,-248,80,-248,78,-248,160,-248,87,-248,45,-248,42,-248,21,-248,22,-248,144,-248,147,-248,145,-248,146,-248,155,-248,158,-248,157,-248,156,-248,57,-248,91,-248,40,-248,25,-248,97,-248,54,-248,35,-248,55,-248,102,-248,47,-248,36,-248,53,-248,60,-248,74,-248,72,-248,38,-248,70,-248,71,-248,127,-248,110,-248},new int[]{-193,182});
    states[1510] = new State(-632);
    states[1511] = new State(new int[]{13,343});
    states[1512] = new State(new int[]{13,466});
    states[1513] = new State(-727);
    states[1514] = new State(-646);
    states[1515] = new State(-35);
    states[1516] = new State(new int[]{59,1201,29,1222,66,1226,50,1438,53,1453,62,1455,11,649,91,-64,92,-64,103,-64,44,-211,37,-211,28,-211,26,-211,19,-211,30,-211,31,-211},new int[]{-47,1517,-168,1518,-29,1519,-52,1520,-288,1521,-309,1522,-220,1523,-6,1524,-250,1132});
    states[1517] = new State(-68);
    states[1518] = new State(-78);
    states[1519] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,59,-79,29,-79,66,-79,50,-79,53,-79,62,-79,11,-79,44,-79,37,-79,28,-79,26,-79,19,-79,30,-79,31,-79,91,-79,92,-79,103,-79},new int[]{-27,1208,-28,1209,-141,1211,-147,1221,-151,48,-152,51});
    states[1520] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,59,-80,29,-80,66,-80,50,-80,53,-80,62,-80,11,-80,44,-80,37,-80,28,-80,26,-80,19,-80,30,-80,31,-80,91,-80,92,-80,103,-80},new int[]{-27,1225,-28,1209,-141,1211,-147,1221,-151,48,-152,51});
    states[1521] = new State(new int[]{11,649,59,-81,29,-81,66,-81,50,-81,53,-81,62,-81,44,-81,37,-81,28,-81,26,-81,19,-81,30,-81,31,-81,91,-81,92,-81,103,-81,143,-211,85,-211,86,-211,80,-211,78,-211,160,-211,87,-211},new int[]{-49,1229,-6,1230,-250,1132});
    states[1522] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,8,1445,59,-82,29,-82,66,-82,50,-82,53,-82,62,-82,11,-82,44,-82,37,-82,28,-82,26,-82,19,-82,30,-82,31,-82,91,-82,92,-82,103,-82},new int[]{-313,1441,-310,1442,-311,1443,-158,815,-147,814,-151,48,-152,51});
    states[1523] = new State(-83);
    states[1524] = new State(new int[]{44,1537,37,1544,28,1373,26,1374,19,1571,30,1578,31,1391,11,649},new int[]{-213,1525,-250,495,-214,1526,-221,1527,-228,1528,-225,1320,-229,1355,-3,1561,-217,1575,-227,1576});
    states[1525] = new State(-86);
    states[1526] = new State(-84);
    states[1527] = new State(-423);
    states[1528] = new State(new int[]{149,1530,107,1311,59,-67,29,-67,66,-67,50,-67,53,-67,62,-67,11,-67,44,-67,37,-67,28,-67,26,-67,19,-67,30,-67,31,-67,91,-67},new int[]{-178,1529,-177,1532,-41,1533,-42,1516,-61,1536});
    states[1529] = new State(-428);
    states[1530] = new State(new int[]{10,1531});
    states[1531] = new State(-434);
    states[1532] = new State(-444);
    states[1533] = new State(new int[]{91,17},new int[]{-255,1534});
    states[1534] = new State(new int[]{10,1535});
    states[1535] = new State(-466);
    states[1536] = new State(-445);
    states[1537] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-171,1538,-170,1174,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[1538] = new State(new int[]{8,585,10,-469,110,-469},new int[]{-128,1539});
    states[1539] = new State(new int[]{10,1353,110,-829},new int[]{-207,1324,-208,1540});
    states[1540] = new State(new int[]{110,1541});
    states[1541] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493},new int[]{-260,1542,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[1542] = new State(new int[]{10,1543});
    states[1543] = new State(-433);
    states[1544] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-170,1545,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[1545] = new State(new int[]{8,585,5,-469,10,-469,110,-469},new int[]{-128,1546});
    states[1546] = new State(new int[]{5,1547,10,1353,110,-829},new int[]{-207,1359,-208,1555});
    states[1547] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-275,1548,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1548] = new State(new int[]{10,1353,110,-829},new int[]{-207,1363,-208,1549});
    states[1549] = new State(new int[]{110,1550});
    states[1550] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,664,21,270,22,275,76,541,20,599,40,610,18,688,37,697,44,701},new int[]{-101,1551,-322,1553,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-100,673,-323,696});
    states[1551] = new State(new int[]{10,1552});
    states[1552] = new State(-429);
    states[1553] = new State(new int[]{10,1554});
    states[1554] = new State(-431);
    states[1555] = new State(new int[]{110,1556});
    states[1556] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,664,21,270,22,275,76,541,20,599,40,610,18,688,37,697,44,701},new int[]{-101,1557,-322,1559,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-100,673,-323,696});
    states[1557] = new State(new int[]{10,1558});
    states[1558] = new State(-430);
    states[1559] = new State(new int[]{10,1560});
    states[1560] = new State(-432);
    states[1561] = new State(new int[]{19,1563,30,1565,44,1537,37,1544},new int[]{-221,1562,-228,1528,-225,1320,-229,1355});
    states[1562] = new State(-424);
    states[1563] = new State(new int[]{44,1537,37,1544},new int[]{-221,1564,-228,1528,-225,1320,-229,1355});
    states[1564] = new State(-427);
    states[1565] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,8,-376,110,-376,10,-376},new int[]{-172,1566,-171,1173,-170,1174,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[1566] = new State(new int[]{8,585,110,-469,10,-469},new int[]{-128,1567});
    states[1567] = new State(new int[]{110,1568,10,1162},new int[]{-207,503});
    states[1568] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493},new int[]{-260,1569,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[1569] = new State(new int[]{10,1570});
    states[1570] = new State(-419);
    states[1571] = new State(new int[]{44,1537,37,1544,28,1373,26,1374},new int[]{-221,1572,-3,1573,-228,1528,-225,1320,-229,1355});
    states[1572] = new State(-425);
    states[1573] = new State(new int[]{44,1537,37,1544},new int[]{-221,1574,-228,1528,-225,1320,-229,1355});
    states[1574] = new State(-426);
    states[1575] = new State(-85);
    states[1576] = new State(-67,new int[]{-177,1577,-41,1533,-42,1516});
    states[1577] = new State(-417);
    states[1578] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,8,-376,110,-376,10,-376},new int[]{-172,1579,-171,1173,-170,1174,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[1579] = new State(new int[]{8,585,110,-469,10,-469},new int[]{-128,1580});
    states[1580] = new State(new int[]{110,1581,10,1162},new int[]{-207,1387});
    states[1581] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,10,-493},new int[]{-260,1582,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890});
    states[1582] = new State(new int[]{10,1583});
    states[1583] = new State(-418);
    states[1584] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148},new int[]{-303,1585,-307,1595,-157,1589,-138,1594,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[1585] = new State(new int[]{10,1586,100,1587});
    states[1586] = new State(-38);
    states[1587] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148},new int[]{-307,1588,-157,1589,-138,1594,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[1588] = new State(-44);
    states[1589] = new State(new int[]{7,1590,137,1592,10,-45,100,-45});
    states[1590] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148},new int[]{-138,1591,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[1591] = new State(-37);
    states[1592] = new State(new int[]{144,1593});
    states[1593] = new State(-46);
    states[1594] = new State(-36);
    states[1595] = new State(-43);
    states[1596] = new State(new int[]{3,1598,52,-15,91,-15,59,-15,29,-15,66,-15,50,-15,53,-15,62,-15,11,-15,44,-15,37,-15,28,-15,26,-15,19,-15,30,-15,31,-15,43,-15,92,-15,103,-15},new int[]{-184,1597});
    states[1597] = new State(-17);
    states[1598] = new State(new int[]{143,1599,144,1600});
    states[1599] = new State(-18);
    states[1600] = new State(-19);
    states[1601] = new State(-16);
    states[1602] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,1603,-151,48,-152,51});
    states[1603] = new State(new int[]{10,1605,8,1606},new int[]{-187,1604});
    states[1604] = new State(-28);
    states[1605] = new State(-29);
    states[1606] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-189,1607,-146,1613,-147,1612,-151,48,-152,51});
    states[1607] = new State(new int[]{9,1608,100,1610});
    states[1608] = new State(new int[]{10,1609});
    states[1609] = new State(-30);
    states[1610] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-146,1611,-147,1612,-151,48,-152,51});
    states[1611] = new State(-32);
    states[1612] = new State(-33);
    states[1613] = new State(-31);
    states[1614] = new State(-3);
    states[1615] = new State(new int[]{43,1636,52,-41,59,-41,29,-41,66,-41,50,-41,53,-41,62,-41,11,-41,44,-41,37,-41,28,-41,26,-41,19,-41,30,-41,31,-41,92,-41,103,-41,91,-41},new int[]{-162,1616,-163,1633,-302,1662});
    states[1616] = new State(new int[]{41,1630},new int[]{-161,1617});
    states[1617] = new State(new int[]{92,1620,103,1621,91,1627},new int[]{-154,1618});
    states[1618] = new State(new int[]{7,1619});
    states[1619] = new State(-47);
    states[1620] = new State(-57);
    states[1621] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,757,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,104,-493,10,-493},new int[]{-252,1622,-261,755,-260,22,-4,23,-113,24,-132,373,-111,386,-147,756,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890,-143,1045});
    states[1622] = new State(new int[]{92,1623,104,1624,10,20});
    states[1623] = new State(-58);
    states[1624] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,757,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493},new int[]{-252,1625,-261,755,-260,22,-4,23,-113,24,-132,373,-111,386,-147,756,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890,-143,1045});
    states[1625] = new State(new int[]{92,1626,10,20});
    states[1626] = new State(-59);
    states[1627] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,757,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,92,-493,10,-493},new int[]{-252,1628,-261,755,-260,22,-4,23,-113,24,-132,373,-111,386,-147,756,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890,-143,1045});
    states[1628] = new State(new int[]{92,1629,10,20});
    states[1629] = new State(-60);
    states[1630] = new State(-41,new int[]{-302,1631});
    states[1631] = new State(new int[]{52,1584,59,-67,29,-67,66,-67,50,-67,53,-67,62,-67,11,-67,44,-67,37,-67,28,-67,26,-67,19,-67,30,-67,31,-67,92,-67,103,-67,91,-67},new int[]{-41,1632,-304,14,-42,1516});
    states[1632] = new State(-55);
    states[1633] = new State(new int[]{92,1620,103,1621,91,1627},new int[]{-154,1634});
    states[1634] = new State(new int[]{7,1635});
    states[1635] = new State(-48);
    states[1636] = new State(-41,new int[]{-302,1637});
    states[1637] = new State(new int[]{52,1584,29,-62,66,-62,50,-62,53,-62,62,-62,11,-62,44,-62,37,-62,41,-62},new int[]{-40,1638,-304,14,-38,1639});
    states[1638] = new State(-54);
    states[1639] = new State(new int[]{29,1222,66,1226,50,1438,53,1453,62,1455,11,649,41,-61,44,-211,37,-211},new int[]{-48,1640,-29,1641,-52,1642,-288,1643,-309,1644,-232,1645,-6,1646,-250,1132,-231,1661});
    states[1640] = new State(-63);
    states[1641] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,29,-72,66,-72,50,-72,53,-72,62,-72,11,-72,44,-72,37,-72,41,-72},new int[]{-27,1208,-28,1209,-141,1211,-147,1221,-151,48,-152,51});
    states[1642] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,29,-73,66,-73,50,-73,53,-73,62,-73,11,-73,44,-73,37,-73,41,-73},new int[]{-27,1225,-28,1209,-141,1211,-147,1221,-151,48,-152,51});
    states[1643] = new State(new int[]{11,649,29,-74,66,-74,50,-74,53,-74,62,-74,44,-74,37,-74,41,-74,143,-211,85,-211,86,-211,80,-211,78,-211,160,-211,87,-211},new int[]{-49,1229,-6,1230,-250,1132});
    states[1644] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,8,1445,29,-75,66,-75,50,-75,53,-75,62,-75,11,-75,44,-75,37,-75,41,-75},new int[]{-313,1441,-310,1442,-311,1443,-158,815,-147,814,-151,48,-152,51});
    states[1645] = new State(-76);
    states[1646] = new State(new int[]{44,1653,11,649,37,1656},new int[]{-225,1647,-250,495,-229,1650});
    states[1647] = new State(new int[]{149,1648,29,-92,66,-92,50,-92,53,-92,62,-92,11,-92,44,-92,37,-92,41,-92});
    states[1648] = new State(new int[]{10,1649});
    states[1649] = new State(-93);
    states[1650] = new State(new int[]{149,1651,29,-94,66,-94,50,-94,53,-94,62,-94,11,-94,44,-94,37,-94,41,-94});
    states[1651] = new State(new int[]{10,1652});
    states[1652] = new State(-95);
    states[1653] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-171,1654,-170,1174,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[1654] = new State(new int[]{8,585,10,-469},new int[]{-128,1655});
    states[1655] = new State(new int[]{10,1162},new int[]{-207,1324});
    states[1656] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-170,1657,-142,1175,-137,1176,-134,1177,-147,1182,-151,48,-152,51,-191,1183,-334,1185,-149,1189});
    states[1657] = new State(new int[]{8,585,5,-469,10,-469},new int[]{-128,1658});
    states[1658] = new State(new int[]{5,1659,10,1162},new int[]{-207,1359});
    states[1659] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,625},new int[]{-275,1660,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1660] = new State(new int[]{10,1162},new int[]{-207,1363});
    states[1661] = new State(-77);
    states[1662] = new State(new int[]{52,1584,59,-67,29,-67,66,-67,50,-67,53,-67,62,-67,11,-67,44,-67,37,-67,28,-67,26,-67,19,-67,30,-67,31,-67,92,-67,103,-67,91,-67},new int[]{-41,1663,-304,14,-42,1516});
    states[1663] = new State(-56);
    states[1664] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-139,1665,-147,1668,-151,48,-152,51});
    states[1665] = new State(new int[]{10,1666});
    states[1666] = new State(new int[]{3,1598,43,-14,92,-14,103,-14,91,-14,52,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-185,1667,-186,1596,-184,1601});
    states[1667] = new State(-49);
    states[1668] = new State(-53);
    states[1669] = new State(-51);
    states[1670] = new State(-52);
    states[1671] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148},new int[]{-157,1672,-138,1594,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[1672] = new State(new int[]{10,1673,7,1590});
    states[1673] = new State(new int[]{3,1598,43,-14,92,-14,103,-14,91,-14,52,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-185,1674,-186,1596,-184,1601});
    states[1674] = new State(-50);
    states[1675] = new State(-4);
    states[1676] = new State(new int[]{50,1678,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,20,599,40,610,5,619},new int[]{-87,1677,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618});
    states[1677] = new State(-7);
    states[1678] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-144,1679,-147,1680,-151,48,-152,51});
    states[1679] = new State(-8);
    states[1680] = new State(new int[]{123,1179,2,-219},new int[]{-155,1431});
    states[1681] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-320,1682,-321,1683,-147,1687,-151,48,-152,51});
    states[1682] = new State(-9);
    states[1683] = new State(new int[]{7,1684,123,174,2,-785},new int[]{-298,1686});
    states[1684] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148},new int[]{-138,1685,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[1685] = new State(-784);
    states[1686] = new State(-786);
    states[1687] = new State(-783);
    states[1688] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,508,21,270,22,275,76,541,20,599,40,610,5,619,53,805},new int[]{-259,1689,-87,1690,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,720,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,601,-117,602,-242,609,-120,618,-4,1691,-314,1692});
    states[1689] = new State(-10);
    states[1690] = new State(-11);
    states[1691] = new State(-12);
    states[1692] = new State(-13);
    states[1693] = new State(new int[]{52,1584,141,-39,143,-39,85,-39,86,-39,80,-39,78,-39,160,-39,87,-39,45,-39,42,-39,8,-39,21,-39,22,-39,144,-39,147,-39,145,-39,146,-39,155,-39,158,-39,157,-39,156,-39,76,-39,57,-39,91,-39,40,-39,25,-39,97,-39,54,-39,35,-39,55,-39,102,-39,47,-39,36,-39,53,-39,60,-39,74,-39,72,-39,38,-39,11,-39,10,-39,44,-39,37,-39,2,-39},new int[]{-305,1694,-304,1699});
    states[1694] = new State(-65,new int[]{-44,1695});
    states[1695] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,757,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,11,649,10,-493,2,-493,44,-211,37,-211},new int[]{-252,1696,-6,1697,-261,755,-260,22,-4,23,-113,24,-132,373,-111,386,-147,756,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890,-143,1045,-250,1132});
    states[1696] = new State(new int[]{10,20,2,-5});
    states[1697] = new State(new int[]{44,1537,37,1544,11,649},new int[]{-221,1698,-250,495,-228,1528,-225,1320,-229,1355});
    states[1698] = new State(-66);
    states[1699] = new State(-40);
    states[1700] = new State(new int[]{52,1584,141,-39,143,-39,85,-39,86,-39,80,-39,78,-39,160,-39,87,-39,45,-39,42,-39,8,-39,21,-39,22,-39,144,-39,147,-39,145,-39,146,-39,155,-39,158,-39,157,-39,156,-39,76,-39,57,-39,91,-39,40,-39,25,-39,97,-39,54,-39,35,-39,55,-39,102,-39,47,-39,36,-39,53,-39,60,-39,74,-39,72,-39,38,-39,11,-39,10,-39,44,-39,37,-39,2,-39},new int[]{-305,1701,-304,1699});
    states[1701] = new State(-65,new int[]{-44,1702});
    states[1702] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,757,158,159,157,160,156,161,76,541,57,736,91,17,40,728,25,743,97,759,54,764,35,769,55,780,102,786,47,793,36,796,53,805,60,877,74,882,72,869,38,891,11,649,10,-493,2,-493,44,-211,37,-211},new int[]{-252,1703,-6,1697,-261,755,-260,22,-4,23,-113,24,-132,373,-111,386,-147,756,-151,48,-152,51,-191,429,-257,516,-294,517,-15,714,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,715,-116,548,-212,734,-133,735,-255,740,-153,741,-35,742,-247,758,-318,763,-124,768,-319,779,-160,784,-301,785,-248,792,-123,795,-314,804,-59,873,-174,874,-173,875,-169,876,-126,881,-127,888,-125,889,-348,890,-143,1045,-250,1132});
    states[1703] = new State(new int[]{10,20,2,-6});

    rules[1] = new Rule(-361, new int[]{-1,2});
    rules[2] = new Rule(-1, new int[]{-234});
    rules[3] = new Rule(-1, new int[]{-306});
    rules[4] = new Rule(-1, new int[]{-175});
    rules[5] = new Rule(-1, new int[]{75,-305,-44,-252});
    rules[6] = new Rule(-1, new int[]{77,-305,-44,-252});
    rules[7] = new Rule(-175, new int[]{88,-87});
    rules[8] = new Rule(-175, new int[]{88,50,-144});
    rules[9] = new Rule(-175, new int[]{90,-320});
    rules[10] = new Rule(-175, new int[]{89,-259});
    rules[11] = new Rule(-259, new int[]{-87});
    rules[12] = new Rule(-259, new int[]{-4});
    rules[13] = new Rule(-259, new int[]{-314});
    rules[14] = new Rule(-185, new int[]{});
    rules[15] = new Rule(-185, new int[]{-186});
    rules[16] = new Rule(-186, new int[]{-184});
    rules[17] = new Rule(-186, new int[]{-186,-184});
    rules[18] = new Rule(-184, new int[]{3,143});
    rules[19] = new Rule(-184, new int[]{3,144});
    rules[20] = new Rule(-234, new int[]{-235,-185,-302,-18,-188});
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
    rules[34] = new Rule(-18, new int[]{-37,-255});
    rules[35] = new Rule(-37, new int[]{-41});
    rules[36] = new Rule(-157, new int[]{-138});
    rules[37] = new Rule(-157, new int[]{-157,7,-138});
    rules[38] = new Rule(-304, new int[]{52,-303,10});
    rules[39] = new Rule(-305, new int[]{});
    rules[40] = new Rule(-305, new int[]{-304});
    rules[41] = new Rule(-302, new int[]{});
    rules[42] = new Rule(-302, new int[]{-302,-304});
    rules[43] = new Rule(-303, new int[]{-307});
    rules[44] = new Rule(-303, new int[]{-303,100,-307});
    rules[45] = new Rule(-307, new int[]{-157});
    rules[46] = new Rule(-307, new int[]{-157,137,144});
    rules[47] = new Rule(-306, new int[]{-308,-162,-161,-154,7});
    rules[48] = new Rule(-306, new int[]{-308,-163,-154,7});
    rules[49] = new Rule(-308, new int[]{-2,-139,10,-185});
    rules[50] = new Rule(-308, new int[]{109,-157,10,-185});
    rules[51] = new Rule(-2, new int[]{105});
    rules[52] = new Rule(-2, new int[]{106});
    rules[53] = new Rule(-139, new int[]{-147});
    rules[54] = new Rule(-162, new int[]{43,-302,-40});
    rules[55] = new Rule(-161, new int[]{41,-302,-41});
    rules[56] = new Rule(-163, new int[]{-302,-41});
    rules[57] = new Rule(-154, new int[]{92});
    rules[58] = new Rule(-154, new int[]{103,-252,92});
    rules[59] = new Rule(-154, new int[]{103,-252,104,-252,92});
    rules[60] = new Rule(-154, new int[]{91,-252,92});
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
    rules[74] = new Rule(-48, new int[]{-288});
    rules[75] = new Rule(-48, new int[]{-309});
    rules[76] = new Rule(-48, new int[]{-232});
    rules[77] = new Rule(-48, new int[]{-231});
    rules[78] = new Rule(-47, new int[]{-168});
    rules[79] = new Rule(-47, new int[]{-29});
    rules[80] = new Rule(-47, new int[]{-52});
    rules[81] = new Rule(-47, new int[]{-288});
    rules[82] = new Rule(-47, new int[]{-309});
    rules[83] = new Rule(-47, new int[]{-220});
    rules[84] = new Rule(-213, new int[]{-214});
    rules[85] = new Rule(-213, new int[]{-217});
    rules[86] = new Rule(-220, new int[]{-6,-213});
    rules[87] = new Rule(-46, new int[]{-168});
    rules[88] = new Rule(-46, new int[]{-29});
    rules[89] = new Rule(-46, new int[]{-52});
    rules[90] = new Rule(-46, new int[]{-288});
    rules[91] = new Rule(-46, new int[]{-309});
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
    rules[105] = new Rule(-288, new int[]{50,-49});
    rules[106] = new Rule(-288, new int[]{-288,-49});
    rules[107] = new Rule(-313, new int[]{-310});
    rules[108] = new Rule(-313, new int[]{8,-147,100,-158,9,110,-101,10});
    rules[109] = new Rule(-309, new int[]{53,-313});
    rules[110] = new Rule(-309, new int[]{62,-313});
    rules[111] = new Rule(-309, new int[]{-309,-313});
    rules[112] = new Rule(-27, new int[]{-28,10});
    rules[113] = new Rule(-28, new int[]{-141,120,-109});
    rules[114] = new Rule(-28, new int[]{-141,5,-276,120,-84});
    rules[115] = new Rule(-109, new int[]{-90});
    rules[116] = new Rule(-109, new int[]{-95});
    rules[117] = new Rule(-141, new int[]{-147});
    rules[118] = new Rule(-91, new int[]{-81});
    rules[119] = new Rule(-91, new int[]{-91,-192,-81});
    rules[120] = new Rule(-90, new int[]{-91});
    rules[121] = new Rule(-90, new int[]{-241});
    rules[122] = new Rule(-90, new int[]{-90,16,-91});
    rules[123] = new Rule(-241, new int[]{-90,13,-90,5,-90});
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
    rules[137] = new Rule(-265, new int[]{-13,-201,-283});
    rules[138] = new Rule(-269, new int[]{-11,119,-10});
    rules[139] = new Rule(-269, new int[]{-11,119,-269});
    rules[140] = new Rule(-269, new int[]{-199,-269});
    rules[141] = new Rule(-13, new int[]{-10});
    rules[142] = new Rule(-13, new int[]{-265});
    rules[143] = new Rule(-13, new int[]{-269});
    rules[144] = new Rule(-13, new int[]{-13,-195,-10});
    rules[145] = new Rule(-13, new int[]{-13,-195,-269});
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
    rules[164] = new Rule(-239, new int[]{11,-69,12});
    rules[165] = new Rule(-239, new int[]{76,-69,76});
    rules[166] = new Rule(-199, new int[]{116});
    rules[167] = new Rule(-199, new int[]{115});
    rules[168] = new Rule(-14, new int[]{-147});
    rules[169] = new Rule(-14, new int[]{-165});
    rules[170] = new Rule(-14, new int[]{-16});
    rules[171] = new Rule(-14, new int[]{42,-147});
    rules[172] = new Rule(-14, new int[]{-257});
    rules[173] = new Rule(-14, new int[]{-294});
    rules[174] = new Rule(-14, new int[]{-14,-12});
    rules[175] = new Rule(-14, new int[]{-14,4,-298});
    rules[176] = new Rule(-14, new int[]{-14,11,-121,12});
    rules[177] = new Rule(-12, new int[]{7,-138});
    rules[178] = new Rule(-12, new int[]{142});
    rules[179] = new Rule(-12, new int[]{8,-76,9});
    rules[180] = new Rule(-12, new int[]{11,-75,12});
    rules[181] = new Rule(-76, new int[]{-71});
    rules[182] = new Rule(-76, new int[]{});
    rules[183] = new Rule(-75, new int[]{-73});
    rules[184] = new Rule(-75, new int[]{});
    rules[185] = new Rule(-73, new int[]{-94});
    rules[186] = new Rule(-73, new int[]{-73,100,-94});
    rules[187] = new Rule(-94, new int[]{-90});
    rules[188] = new Rule(-94, new int[]{-90,6,-90});
    rules[189] = new Rule(-16, new int[]{155});
    rules[190] = new Rule(-16, new int[]{158});
    rules[191] = new Rule(-16, new int[]{157});
    rules[192] = new Rule(-16, new int[]{156});
    rules[193] = new Rule(-84, new int[]{-90});
    rules[194] = new Rule(-84, new int[]{-95});
    rules[195] = new Rule(-84, new int[]{-243});
    rules[196] = new Rule(-95, new int[]{8,-66,9});
    rules[197] = new Rule(-66, new int[]{});
    rules[198] = new Rule(-66, new int[]{-65});
    rules[199] = new Rule(-65, new int[]{-85});
    rules[200] = new Rule(-65, new int[]{-65,100,-85});
    rules[201] = new Rule(-243, new int[]{8,-245,9});
    rules[202] = new Rule(-245, new int[]{-244});
    rules[203] = new Rule(-245, new int[]{-244,10});
    rules[204] = new Rule(-244, new int[]{-246});
    rules[205] = new Rule(-244, new int[]{-244,10,-246});
    rules[206] = new Rule(-246, new int[]{-136,5,-84});
    rules[207] = new Rule(-136, new int[]{-147});
    rules[208] = new Rule(-49, new int[]{-6,-50});
    rules[209] = new Rule(-6, new int[]{-250});
    rules[210] = new Rule(-6, new int[]{-6,-250});
    rules[211] = new Rule(-6, new int[]{});
    rules[212] = new Rule(-250, new int[]{11,-251,12});
    rules[213] = new Rule(-251, new int[]{-8});
    rules[214] = new Rule(-251, new int[]{-251,100,-8});
    rules[215] = new Rule(-8, new int[]{-9});
    rules[216] = new Rule(-8, new int[]{-147,5,-9});
    rules[217] = new Rule(-50, new int[]{-144,120,-286,10});
    rules[218] = new Rule(-50, new int[]{-145,-286,10});
    rules[219] = new Rule(-144, new int[]{-147});
    rules[220] = new Rule(-144, new int[]{-147,-155});
    rules[221] = new Rule(-145, new int[]{-147,123,-158,122});
    rules[222] = new Rule(-286, new int[]{-276});
    rules[223] = new Rule(-286, new int[]{-30});
    rules[224] = new Rule(-273, new int[]{-272,13});
    rules[225] = new Rule(-273, new int[]{-300,13});
    rules[226] = new Rule(-276, new int[]{-272});
    rules[227] = new Rule(-276, new int[]{-273});
    rules[228] = new Rule(-276, new int[]{-256});
    rules[229] = new Rule(-276, new int[]{-249});
    rules[230] = new Rule(-276, new int[]{-281});
    rules[231] = new Rule(-276, new int[]{-226});
    rules[232] = new Rule(-276, new int[]{-300});
    rules[233] = new Rule(-300, new int[]{-180,-298});
    rules[234] = new Rule(-298, new int[]{123,-296,121});
    rules[235] = new Rule(-299, new int[]{125});
    rules[236] = new Rule(-299, new int[]{123,-297,121});
    rules[237] = new Rule(-296, new int[]{-279});
    rules[238] = new Rule(-296, new int[]{-296,100,-279});
    rules[239] = new Rule(-297, new int[]{-280});
    rules[240] = new Rule(-297, new int[]{-297,100,-280});
    rules[241] = new Rule(-280, new int[]{});
    rules[242] = new Rule(-279, new int[]{-272});
    rules[243] = new Rule(-279, new int[]{-272,13});
    rules[244] = new Rule(-279, new int[]{-281});
    rules[245] = new Rule(-279, new int[]{-226});
    rules[246] = new Rule(-279, new int[]{-300});
    rules[247] = new Rule(-272, new int[]{-93});
    rules[248] = new Rule(-272, new int[]{-93,6,-93});
    rules[249] = new Rule(-272, new int[]{8,-80,9});
    rules[250] = new Rule(-93, new int[]{-106});
    rules[251] = new Rule(-93, new int[]{-93,-193,-106});
    rules[252] = new Rule(-106, new int[]{-107});
    rules[253] = new Rule(-106, new int[]{-106,-195,-107});
    rules[254] = new Rule(-107, new int[]{-180});
    rules[255] = new Rule(-107, new int[]{-16});
    rules[256] = new Rule(-107, new int[]{-199,-107});
    rules[257] = new Rule(-107, new int[]{-165});
    rules[258] = new Rule(-107, new int[]{-107,8,-75,9});
    rules[259] = new Rule(-180, new int[]{-147});
    rules[260] = new Rule(-180, new int[]{-180,7,-138});
    rules[261] = new Rule(-80, new int[]{-78});
    rules[262] = new Rule(-80, new int[]{-80,100,-78});
    rules[263] = new Rule(-78, new int[]{-276});
    rules[264] = new Rule(-78, new int[]{-276,120,-87});
    rules[265] = new Rule(-249, new int[]{142,-275});
    rules[266] = new Rule(-281, new int[]{-278});
    rules[267] = new Rule(-281, new int[]{-31});
    rules[268] = new Rule(-281, new int[]{-263});
    rules[269] = new Rule(-281, new int[]{-130});
    rules[270] = new Rule(-281, new int[]{-131});
    rules[271] = new Rule(-131, new int[]{73,58,-276});
    rules[272] = new Rule(-278, new int[]{24,11,-164,12,58,-276});
    rules[273] = new Rule(-278, new int[]{-270});
    rules[274] = new Rule(-270, new int[]{24,58,-276});
    rules[275] = new Rule(-164, new int[]{-271});
    rules[276] = new Rule(-164, new int[]{-164,100,-271});
    rules[277] = new Rule(-271, new int[]{-272});
    rules[278] = new Rule(-271, new int[]{});
    rules[279] = new Rule(-263, new int[]{49,58,-276});
    rules[280] = new Rule(-130, new int[]{34,58,-276});
    rules[281] = new Rule(-130, new int[]{34});
    rules[282] = new Rule(-256, new int[]{143,11,-90,12});
    rules[283] = new Rule(-226, new int[]{-224});
    rules[284] = new Rule(-224, new int[]{-223});
    rules[285] = new Rule(-223, new int[]{44,-128});
    rules[286] = new Rule(-223, new int[]{37,-128,5,-275});
    rules[287] = new Rule(-223, new int[]{-180,127,-279});
    rules[288] = new Rule(-223, new int[]{-300,127,-279});
    rules[289] = new Rule(-223, new int[]{8,9,127,-279});
    rules[290] = new Rule(-223, new int[]{8,-80,9,127,-279});
    rules[291] = new Rule(-223, new int[]{-180,127,8,9});
    rules[292] = new Rule(-223, new int[]{-300,127,8,9});
    rules[293] = new Rule(-223, new int[]{8,9,127,8,9});
    rules[294] = new Rule(-223, new int[]{8,-80,9,127,8,9});
    rules[295] = new Rule(-30, new int[]{-21,-290,-183,-317,-26});
    rules[296] = new Rule(-31, new int[]{48,-183,-317,-25,92});
    rules[297] = new Rule(-20, new int[]{68});
    rules[298] = new Rule(-20, new int[]{69});
    rules[299] = new Rule(-20, new int[]{148});
    rules[300] = new Rule(-20, new int[]{27});
    rules[301] = new Rule(-20, new int[]{28});
    rules[302] = new Rule(-21, new int[]{});
    rules[303] = new Rule(-21, new int[]{-22});
    rules[304] = new Rule(-22, new int[]{-20});
    rules[305] = new Rule(-22, new int[]{-22,-20});
    rules[306] = new Rule(-290, new int[]{26});
    rules[307] = new Rule(-290, new int[]{43});
    rules[308] = new Rule(-290, new int[]{64});
    rules[309] = new Rule(-290, new int[]{64,26});
    rules[310] = new Rule(-290, new int[]{64,48});
    rules[311] = new Rule(-290, new int[]{64,43});
    rules[312] = new Rule(-26, new int[]{});
    rules[313] = new Rule(-26, new int[]{-25,92});
    rules[314] = new Rule(-183, new int[]{});
    rules[315] = new Rule(-183, new int[]{8,-182,9});
    rules[316] = new Rule(-182, new int[]{-181});
    rules[317] = new Rule(-182, new int[]{-182,100,-181});
    rules[318] = new Rule(-181, new int[]{-180});
    rules[319] = new Rule(-181, new int[]{-300});
    rules[320] = new Rule(-155, new int[]{123,-158,121});
    rules[321] = new Rule(-317, new int[]{});
    rules[322] = new Rule(-317, new int[]{-316});
    rules[323] = new Rule(-316, new int[]{-315});
    rules[324] = new Rule(-316, new int[]{-316,-315});
    rules[325] = new Rule(-315, new int[]{23,-158,5,-287,10});
    rules[326] = new Rule(-287, new int[]{-285});
    rules[327] = new Rule(-287, new int[]{-287,100,-285});
    rules[328] = new Rule(-285, new int[]{-276});
    rules[329] = new Rule(-285, new int[]{26});
    rules[330] = new Rule(-285, new int[]{48});
    rules[331] = new Rule(-285, new int[]{30});
    rules[332] = new Rule(-25, new int[]{-32});
    rules[333] = new Rule(-25, new int[]{-25,-7,-32});
    rules[334] = new Rule(-7, new int[]{84});
    rules[335] = new Rule(-7, new int[]{83});
    rules[336] = new Rule(-7, new int[]{82});
    rules[337] = new Rule(-7, new int[]{81});
    rules[338] = new Rule(-32, new int[]{});
    rules[339] = new Rule(-32, new int[]{-34,-190});
    rules[340] = new Rule(-32, new int[]{-33});
    rules[341] = new Rule(-32, new int[]{-34,10,-33});
    rules[342] = new Rule(-158, new int[]{-147});
    rules[343] = new Rule(-158, new int[]{-158,100,-147});
    rules[344] = new Rule(-190, new int[]{});
    rules[345] = new Rule(-190, new int[]{10});
    rules[346] = new Rule(-34, new int[]{-45});
    rules[347] = new Rule(-34, new int[]{-34,10,-45});
    rules[348] = new Rule(-45, new int[]{-6,-51});
    rules[349] = new Rule(-33, new int[]{-54});
    rules[350] = new Rule(-33, new int[]{-33,-54});
    rules[351] = new Rule(-54, new int[]{-53});
    rules[352] = new Rule(-54, new int[]{-55});
    rules[353] = new Rule(-51, new int[]{29,-28});
    rules[354] = new Rule(-51, new int[]{-312});
    rules[355] = new Rule(-51, new int[]{-3,-312});
    rules[356] = new Rule(-3, new int[]{28});
    rules[357] = new Rule(-3, new int[]{26});
    rules[358] = new Rule(-312, new int[]{-311});
    rules[359] = new Rule(-312, new int[]{62,-158,5,-276});
    rules[360] = new Rule(-53, new int[]{-6,-222});
    rules[361] = new Rule(-53, new int[]{-6,-219});
    rules[362] = new Rule(-219, new int[]{-215});
    rules[363] = new Rule(-219, new int[]{-218});
    rules[364] = new Rule(-222, new int[]{-3,-230});
    rules[365] = new Rule(-222, new int[]{-230});
    rules[366] = new Rule(-222, new int[]{19,-3,-230});
    rules[367] = new Rule(-222, new int[]{19,-230});
    rules[368] = new Rule(-222, new int[]{-227});
    rules[369] = new Rule(-230, new int[]{-228});
    rules[370] = new Rule(-228, new int[]{-225});
    rules[371] = new Rule(-228, new int[]{-229});
    rules[372] = new Rule(-227, new int[]{30,-172,-128,-207});
    rules[373] = new Rule(-227, new int[]{-3,30,-172,-128,-207});
    rules[374] = new Rule(-227, new int[]{31,-172,-128,-207});
    rules[375] = new Rule(-172, new int[]{-171});
    rules[376] = new Rule(-172, new int[]{});
    rules[377] = new Rule(-55, new int[]{-6,-258});
    rules[378] = new Rule(-258, new int[]{46,-170,-233,-202,10,-205});
    rules[379] = new Rule(-258, new int[]{46,-170,-233,-202,10,-210,10,-205});
    rules[380] = new Rule(-258, new int[]{-3,46,-170,-233,-202,10,-205});
    rules[381] = new Rule(-258, new int[]{-3,46,-170,-233,-202,10,-210,10,-205});
    rules[382] = new Rule(-258, new int[]{27,46,-170,-233,-211,10});
    rules[383] = new Rule(-258, new int[]{-3,27,46,-170,-233,-211,10});
    rules[384] = new Rule(-211, new int[]{110,-87});
    rules[385] = new Rule(-211, new int[]{});
    rules[386] = new Rule(-205, new int[]{});
    rules[387] = new Rule(-205, new int[]{63,10});
    rules[388] = new Rule(-233, new int[]{-238,5,-275});
    rules[389] = new Rule(-238, new int[]{});
    rules[390] = new Rule(-238, new int[]{11,-237,12});
    rules[391] = new Rule(-237, new int[]{-236});
    rules[392] = new Rule(-237, new int[]{-237,10,-236});
    rules[393] = new Rule(-236, new int[]{-158,5,-275});
    rules[394] = new Rule(-114, new int[]{-88});
    rules[395] = new Rule(-114, new int[]{});
    rules[396] = new Rule(-202, new int[]{});
    rules[397] = new Rule(-202, new int[]{85,-114,-203});
    rules[398] = new Rule(-202, new int[]{86,-260,-204});
    rules[399] = new Rule(-203, new int[]{});
    rules[400] = new Rule(-203, new int[]{86,-260});
    rules[401] = new Rule(-204, new int[]{});
    rules[402] = new Rule(-204, new int[]{85,-114});
    rules[403] = new Rule(-310, new int[]{-311,10});
    rules[404] = new Rule(-338, new int[]{110});
    rules[405] = new Rule(-338, new int[]{120});
    rules[406] = new Rule(-311, new int[]{-158,5,-276});
    rules[407] = new Rule(-311, new int[]{-158,110,-88});
    rules[408] = new Rule(-311, new int[]{-158,5,-276,-338,-86});
    rules[409] = new Rule(-86, new int[]{-85});
    rules[410] = new Rule(-86, new int[]{-81,6,-13});
    rules[411] = new Rule(-86, new int[]{-323});
    rules[412] = new Rule(-86, new int[]{-147,127,-328});
    rules[413] = new Rule(-86, new int[]{8,9,-324,127,-328});
    rules[414] = new Rule(-86, new int[]{8,-66,9,127,-328});
    rules[415] = new Rule(-86, new int[]{-242});
    rules[416] = new Rule(-85, new int[]{-84});
    rules[417] = new Rule(-217, new int[]{-227,-177});
    rules[418] = new Rule(-217, new int[]{30,-172,-128,110,-260,10});
    rules[419] = new Rule(-217, new int[]{-3,30,-172,-128,110,-260,10});
    rules[420] = new Rule(-218, new int[]{-227,-176});
    rules[421] = new Rule(-218, new int[]{30,-172,-128,110,-260,10});
    rules[422] = new Rule(-218, new int[]{-3,30,-172,-128,110,-260,10});
    rules[423] = new Rule(-214, new int[]{-221});
    rules[424] = new Rule(-214, new int[]{-3,-221});
    rules[425] = new Rule(-214, new int[]{19,-221});
    rules[426] = new Rule(-214, new int[]{19,-3,-221});
    rules[427] = new Rule(-214, new int[]{-3,19,-221});
    rules[428] = new Rule(-221, new int[]{-228,-178});
    rules[429] = new Rule(-221, new int[]{37,-170,-128,5,-275,-208,110,-101,10});
    rules[430] = new Rule(-221, new int[]{37,-170,-128,-208,110,-101,10});
    rules[431] = new Rule(-221, new int[]{37,-170,-128,5,-275,-208,110,-322,10});
    rules[432] = new Rule(-221, new int[]{37,-170,-128,-208,110,-322,10});
    rules[433] = new Rule(-221, new int[]{44,-171,-128,-208,110,-260,10});
    rules[434] = new Rule(-221, new int[]{-228,149,10});
    rules[435] = new Rule(-215, new int[]{-216});
    rules[436] = new Rule(-215, new int[]{19,-216});
    rules[437] = new Rule(-215, new int[]{-3,-216});
    rules[438] = new Rule(-215, new int[]{19,-3,-216});
    rules[439] = new Rule(-215, new int[]{-3,19,-216});
    rules[440] = new Rule(-216, new int[]{-228,-176});
    rules[441] = new Rule(-216, new int[]{37,-170,-128,5,-275,-208,110,-103,10});
    rules[442] = new Rule(-216, new int[]{37,-170,-128,-208,110,-103,10});
    rules[443] = new Rule(-216, new int[]{44,-171,-128,-208,110,-260,10});
    rules[444] = new Rule(-178, new int[]{-177});
    rules[445] = new Rule(-178, new int[]{-61});
    rules[446] = new Rule(-171, new int[]{-170});
    rules[447] = new Rule(-170, new int[]{-142});
    rules[448] = new Rule(-170, new int[]{-334,7,-142});
    rules[449] = new Rule(-149, new int[]{-137});
    rules[450] = new Rule(-334, new int[]{-149});
    rules[451] = new Rule(-334, new int[]{-334,7,-149});
    rules[452] = new Rule(-142, new int[]{-137});
    rules[453] = new Rule(-142, new int[]{-191});
    rules[454] = new Rule(-142, new int[]{-191,-155});
    rules[455] = new Rule(-137, new int[]{-134});
    rules[456] = new Rule(-137, new int[]{-134,-155});
    rules[457] = new Rule(-134, new int[]{-147});
    rules[458] = new Rule(-225, new int[]{44,-171,-128,-207,-317});
    rules[459] = new Rule(-229, new int[]{37,-170,-128,-207,-317});
    rules[460] = new Rule(-229, new int[]{37,-170,-128,5,-275,-207,-317});
    rules[461] = new Rule(-61, new int[]{107,-108,80,-108,10});
    rules[462] = new Rule(-61, new int[]{107,-108,10});
    rules[463] = new Rule(-61, new int[]{107,10});
    rules[464] = new Rule(-108, new int[]{-147});
    rules[465] = new Rule(-108, new int[]{-165});
    rules[466] = new Rule(-177, new int[]{-41,-255,10});
    rules[467] = new Rule(-176, new int[]{-43,-255,10});
    rules[468] = new Rule(-176, new int[]{-61});
    rules[469] = new Rule(-128, new int[]{});
    rules[470] = new Rule(-128, new int[]{8,9});
    rules[471] = new Rule(-128, new int[]{8,-129,9});
    rules[472] = new Rule(-129, new int[]{-56});
    rules[473] = new Rule(-129, new int[]{-129,10,-56});
    rules[474] = new Rule(-56, new int[]{-6,-295});
    rules[475] = new Rule(-295, new int[]{-159,5,-275});
    rules[476] = new Rule(-295, new int[]{53,-159,5,-275});
    rules[477] = new Rule(-295, new int[]{29,-159,5,-275});
    rules[478] = new Rule(-295, new int[]{108,-159,5,-275});
    rules[479] = new Rule(-295, new int[]{-159,5,-275,110,-87});
    rules[480] = new Rule(-295, new int[]{53,-159,5,-275,110,-87});
    rules[481] = new Rule(-295, new int[]{29,-159,5,-275,110,-87});
    rules[482] = new Rule(-159, new int[]{-135});
    rules[483] = new Rule(-159, new int[]{-159,100,-135});
    rules[484] = new Rule(-135, new int[]{-147});
    rules[485] = new Rule(-275, new int[]{-276});
    rules[486] = new Rule(-277, new int[]{-272});
    rules[487] = new Rule(-277, new int[]{-256});
    rules[488] = new Rule(-277, new int[]{-249});
    rules[489] = new Rule(-277, new int[]{-281});
    rules[490] = new Rule(-277, new int[]{-300});
    rules[491] = new Rule(-261, new int[]{-260});
    rules[492] = new Rule(-261, new int[]{-143,5,-261});
    rules[493] = new Rule(-260, new int[]{});
    rules[494] = new Rule(-260, new int[]{-4});
    rules[495] = new Rule(-260, new int[]{-212});
    rules[496] = new Rule(-260, new int[]{-133});
    rules[497] = new Rule(-260, new int[]{-255});
    rules[498] = new Rule(-260, new int[]{-153});
    rules[499] = new Rule(-260, new int[]{-35});
    rules[500] = new Rule(-260, new int[]{-247});
    rules[501] = new Rule(-260, new int[]{-318});
    rules[502] = new Rule(-260, new int[]{-124});
    rules[503] = new Rule(-260, new int[]{-319});
    rules[504] = new Rule(-260, new int[]{-160});
    rules[505] = new Rule(-260, new int[]{-301});
    rules[506] = new Rule(-260, new int[]{-248});
    rules[507] = new Rule(-260, new int[]{-123});
    rules[508] = new Rule(-260, new int[]{-314});
    rules[509] = new Rule(-260, new int[]{-59});
    rules[510] = new Rule(-260, new int[]{-169});
    rules[511] = new Rule(-260, new int[]{-126});
    rules[512] = new Rule(-260, new int[]{-127});
    rules[513] = new Rule(-260, new int[]{-125});
    rules[514] = new Rule(-260, new int[]{-348});
    rules[515] = new Rule(-125, new int[]{72,-101,99,-260});
    rules[516] = new Rule(-126, new int[]{74,-103});
    rules[517] = new Rule(-127, new int[]{74,73,-103});
    rules[518] = new Rule(-314, new int[]{53,-311});
    rules[519] = new Rule(-314, new int[]{8,53,-147,100,-337,9,110,-87});
    rules[520] = new Rule(-314, new int[]{53,8,-147,100,-158,9,110,-87});
    rules[521] = new Rule(-4, new int[]{-113,-194,-88});
    rules[522] = new Rule(-4, new int[]{8,-111,100,-336,9,-194,-87});
    rules[523] = new Rule(-336, new int[]{-111});
    rules[524] = new Rule(-336, new int[]{-336,100,-111});
    rules[525] = new Rule(-337, new int[]{53,-147});
    rules[526] = new Rule(-337, new int[]{-337,100,53,-147});
    rules[527] = new Rule(-212, new int[]{-113});
    rules[528] = new Rule(-133, new int[]{57,-143});
    rules[529] = new Rule(-255, new int[]{91,-252,92});
    rules[530] = new Rule(-252, new int[]{-261});
    rules[531] = new Rule(-252, new int[]{-252,10,-261});
    rules[532] = new Rule(-153, new int[]{40,-101,51,-260});
    rules[533] = new Rule(-153, new int[]{40,-101,51,-260,32,-260});
    rules[534] = new Rule(-348, new int[]{38,-101,55,-350,-253,92});
    rules[535] = new Rule(-348, new int[]{38,-101,55,-350,10,-253,92});
    rules[536] = new Rule(-350, new int[]{-349});
    rules[537] = new Rule(-350, new int[]{-350,10,-349});
    rules[538] = new Rule(-349, new int[]{-342,39,-101,5,-260});
    rules[539] = new Rule(-349, new int[]{-341,5,-260});
    rules[540] = new Rule(-349, new int[]{-343,5,-260});
    rules[541] = new Rule(-349, new int[]{-344,39,-101,5,-260});
    rules[542] = new Rule(-349, new int[]{-344,5,-260});
    rules[543] = new Rule(-35, new int[]{25,-101,58,-36,-253,92});
    rules[544] = new Rule(-35, new int[]{25,-101,58,-36,10,-253,92});
    rules[545] = new Rule(-35, new int[]{25,-101,58,-253,92});
    rules[546] = new Rule(-36, new int[]{-262});
    rules[547] = new Rule(-36, new int[]{-36,10,-262});
    rules[548] = new Rule(-262, new int[]{-74,5,-260});
    rules[549] = new Rule(-74, new int[]{-110});
    rules[550] = new Rule(-74, new int[]{-74,100,-110});
    rules[551] = new Rule(-110, new int[]{-94});
    rules[552] = new Rule(-253, new int[]{});
    rules[553] = new Rule(-253, new int[]{32,-252});
    rules[554] = new Rule(-247, new int[]{97,-252,98,-87});
    rules[555] = new Rule(-318, new int[]{54,-101,-291,-260});
    rules[556] = new Rule(-291, new int[]{99});
    rules[557] = new Rule(-291, new int[]{});
    rules[558] = new Rule(-169, new int[]{60,-101,99,-260});
    rules[559] = new Rule(-360, new int[]{87,143});
    rules[560] = new Rule(-360, new int[]{});
    rules[561] = new Rule(-274, new int[]{5,-276});
    rules[562] = new Rule(-274, new int[]{});
    rules[563] = new Rule(-19, new int[]{53});
    rules[564] = new Rule(-19, new int[]{});
    rules[565] = new Rule(-119, new int[]{70});
    rules[566] = new Rule(-119, new int[]{71});
    rules[567] = new Rule(-123, new int[]{36,-147,-274,137,-101,-360,99,-260});
    rules[568] = new Rule(-123, new int[]{36,53,-147,-274,137,-101,-360,99,-260});
    rules[569] = new Rule(-123, new int[]{36,53,8,-158,9,137,-101,-360,99,-260});
    rules[570] = new Rule(-124, new int[]{35,-19,-147,-274,110,-101,-119,-101,-291,-260});
    rules[571] = new Rule(-124, new int[]{35,-19,-147,-274,110,-101,-119,-101,160,-101,99,-260});
    rules[572] = new Rule(-319, new int[]{55,-71,99,-260});
    rules[573] = new Rule(-160, new int[]{42});
    rules[574] = new Rule(-301, new int[]{102,-252,-289});
    rules[575] = new Rule(-289, new int[]{101,-252,92});
    rules[576] = new Rule(-289, new int[]{33,-60,92});
    rules[577] = new Rule(-60, new int[]{-63,-254});
    rules[578] = new Rule(-60, new int[]{-63,10,-254});
    rules[579] = new Rule(-60, new int[]{-252});
    rules[580] = new Rule(-63, new int[]{-62});
    rules[581] = new Rule(-63, new int[]{-63,10,-62});
    rules[582] = new Rule(-254, new int[]{});
    rules[583] = new Rule(-254, new int[]{32,-252});
    rules[584] = new Rule(-62, new int[]{79,-64,99,-260});
    rules[585] = new Rule(-64, new int[]{-179});
    rules[586] = new Rule(-64, new int[]{-140,5,-179});
    rules[587] = new Rule(-179, new int[]{-180});
    rules[588] = new Rule(-140, new int[]{-147});
    rules[589] = new Rule(-248, new int[]{47});
    rules[590] = new Rule(-248, new int[]{47,-87});
    rules[591] = new Rule(-71, new int[]{-88});
    rules[592] = new Rule(-71, new int[]{-71,100,-88});
    rules[593] = new Rule(-72, new int[]{-89});
    rules[594] = new Rule(-72, new int[]{-72,100,-89});
    rules[595] = new Rule(-59, new int[]{-174});
    rules[596] = new Rule(-174, new int[]{-173});
    rules[597] = new Rule(-88, new int[]{-87});
    rules[598] = new Rule(-88, new int[]{-322});
    rules[599] = new Rule(-88, new int[]{42});
    rules[600] = new Rule(-89, new int[]{-87});
    rules[601] = new Rule(-89, new int[]{-147,110,-101});
    rules[602] = new Rule(-89, new int[]{-322});
    rules[603] = new Rule(-89, new int[]{42});
    rules[604] = new Rule(-87, new int[]{-101});
    rules[605] = new Rule(-87, new int[]{-120});
    rules[606] = new Rule(-101, new int[]{-99});
    rules[607] = new Rule(-101, new int[]{-240});
    rules[608] = new Rule(-101, new int[]{-242});
    rules[609] = new Rule(-117, new int[]{-99});
    rules[610] = new Rule(-117, new int[]{-240});
    rules[611] = new Rule(-118, new int[]{-99});
    rules[612] = new Rule(-118, new int[]{-242});
    rules[613] = new Rule(-103, new int[]{-101});
    rules[614] = new Rule(-103, new int[]{-322});
    rules[615] = new Rule(-104, new int[]{-99});
    rules[616] = new Rule(-104, new int[]{-240});
    rules[617] = new Rule(-104, new int[]{-322});
    rules[618] = new Rule(-99, new int[]{-98});
    rules[619] = new Rule(-99, new int[]{20,-98});
    rules[620] = new Rule(-99, new int[]{-99,16,-98});
    rules[621] = new Rule(-257, new int[]{21,8,-283,9});
    rules[622] = new Rule(-294, new int[]{22,8,-283,9});
    rules[623] = new Rule(-294, new int[]{22,8,-282,9});
    rules[624] = new Rule(-240, new int[]{-117,13,-117,5,-117});
    rules[625] = new Rule(-242, new int[]{40,-118,51,-118,32,-118});
    rules[626] = new Rule(-282, new int[]{-180,-299});
    rules[627] = new Rule(-282, new int[]{-180,4,-299});
    rules[628] = new Rule(-283, new int[]{-180});
    rules[629] = new Rule(-283, new int[]{-180,-298});
    rules[630] = new Rule(-283, new int[]{-180,4,-298});
    rules[631] = new Rule(-284, new int[]{-283});
    rules[632] = new Rule(-284, new int[]{-273});
    rules[633] = new Rule(-5, new int[]{8,-66,9});
    rules[634] = new Rule(-5, new int[]{});
    rules[635] = new Rule(-173, new int[]{78,-283,-70});
    rules[636] = new Rule(-173, new int[]{78,-283,11,-67,12,-5});
    rules[637] = new Rule(-173, new int[]{78,26,8,-333,9});
    rules[638] = new Rule(-332, new int[]{-147,110,-101});
    rules[639] = new Rule(-332, new int[]{-101});
    rules[640] = new Rule(-333, new int[]{-332});
    rules[641] = new Rule(-333, new int[]{-333,100,-332});
    rules[642] = new Rule(-70, new int[]{});
    rules[643] = new Rule(-70, new int[]{8,-67,9});
    rules[644] = new Rule(-98, new int[]{-105});
    rules[645] = new Rule(-98, new int[]{-98,-196,-105});
    rules[646] = new Rule(-98, new int[]{-98,-196,-242});
    rules[647] = new Rule(-98, new int[]{-266,8,-353,9});
    rules[648] = new Rule(-340, new int[]{-283,8,-353,9});
    rules[649] = new Rule(-342, new int[]{-283,8,-354,9});
    rules[650] = new Rule(-341, new int[]{-283,8,-354,9});
    rules[651] = new Rule(-341, new int[]{-357});
    rules[652] = new Rule(-357, new int[]{-339});
    rules[653] = new Rule(-357, new int[]{-357,100,-339});
    rules[654] = new Rule(-339, new int[]{-15});
    rules[655] = new Rule(-339, new int[]{-283});
    rules[656] = new Rule(-339, new int[]{56});
    rules[657] = new Rule(-339, new int[]{-257});
    rules[658] = new Rule(-339, new int[]{-294});
    rules[659] = new Rule(-343, new int[]{11,-355,12});
    rules[660] = new Rule(-355, new int[]{-345});
    rules[661] = new Rule(-355, new int[]{-355,100,-345});
    rules[662] = new Rule(-345, new int[]{-15});
    rules[663] = new Rule(-345, new int[]{-347});
    rules[664] = new Rule(-345, new int[]{14});
    rules[665] = new Rule(-345, new int[]{-342});
    rules[666] = new Rule(-345, new int[]{-343});
    rules[667] = new Rule(-345, new int[]{-344});
    rules[668] = new Rule(-345, new int[]{6});
    rules[669] = new Rule(-347, new int[]{53,-147});
    rules[670] = new Rule(-344, new int[]{8,-356,9});
    rules[671] = new Rule(-346, new int[]{14});
    rules[672] = new Rule(-346, new int[]{-15});
    rules[673] = new Rule(-346, new int[]{-199,-15});
    rules[674] = new Rule(-346, new int[]{53,-147});
    rules[675] = new Rule(-346, new int[]{-342});
    rules[676] = new Rule(-346, new int[]{-343});
    rules[677] = new Rule(-346, new int[]{-344});
    rules[678] = new Rule(-356, new int[]{-346});
    rules[679] = new Rule(-356, new int[]{-356,100,-346});
    rules[680] = new Rule(-354, new int[]{-352});
    rules[681] = new Rule(-354, new int[]{-354,10,-352});
    rules[682] = new Rule(-354, new int[]{-354,100,-352});
    rules[683] = new Rule(-353, new int[]{-351});
    rules[684] = new Rule(-353, new int[]{-353,10,-351});
    rules[685] = new Rule(-353, new int[]{-353,100,-351});
    rules[686] = new Rule(-351, new int[]{14});
    rules[687] = new Rule(-351, new int[]{-15});
    rules[688] = new Rule(-351, new int[]{53,-147,5,-276});
    rules[689] = new Rule(-351, new int[]{53,-147});
    rules[690] = new Rule(-351, new int[]{-340});
    rules[691] = new Rule(-351, new int[]{-343});
    rules[692] = new Rule(-351, new int[]{-344});
    rules[693] = new Rule(-352, new int[]{14});
    rules[694] = new Rule(-352, new int[]{-15});
    rules[695] = new Rule(-352, new int[]{-199,-15});
    rules[696] = new Rule(-352, new int[]{-147,5,-276});
    rules[697] = new Rule(-352, new int[]{-147});
    rules[698] = new Rule(-352, new int[]{53,-147,5,-276});
    rules[699] = new Rule(-352, new int[]{53,-147});
    rules[700] = new Rule(-352, new int[]{-342});
    rules[701] = new Rule(-352, new int[]{-343});
    rules[702] = new Rule(-352, new int[]{-344});
    rules[703] = new Rule(-115, new int[]{-105});
    rules[704] = new Rule(-115, new int[]{});
    rules[705] = new Rule(-122, new int[]{-90});
    rules[706] = new Rule(-122, new int[]{});
    rules[707] = new Rule(-120, new int[]{-105,5,-115});
    rules[708] = new Rule(-120, new int[]{5,-115});
    rules[709] = new Rule(-120, new int[]{-105,5,-115,5,-105});
    rules[710] = new Rule(-120, new int[]{5,-115,5,-105});
    rules[711] = new Rule(-121, new int[]{-90,5,-122});
    rules[712] = new Rule(-121, new int[]{5,-122});
    rules[713] = new Rule(-121, new int[]{-90,5,-122,5,-90});
    rules[714] = new Rule(-121, new int[]{5,-122,5,-90});
    rules[715] = new Rule(-196, new int[]{120});
    rules[716] = new Rule(-196, new int[]{125});
    rules[717] = new Rule(-196, new int[]{123});
    rules[718] = new Rule(-196, new int[]{121});
    rules[719] = new Rule(-196, new int[]{124});
    rules[720] = new Rule(-196, new int[]{122});
    rules[721] = new Rule(-196, new int[]{137});
    rules[722] = new Rule(-196, new int[]{135,137});
    rules[723] = new Rule(-105, new int[]{-83});
    rules[724] = new Rule(-105, new int[]{-105,6,-83});
    rules[725] = new Rule(-83, new int[]{-82});
    rules[726] = new Rule(-83, new int[]{-83,-197,-82});
    rules[727] = new Rule(-83, new int[]{-83,-197,-242});
    rules[728] = new Rule(-197, new int[]{116});
    rules[729] = new Rule(-197, new int[]{115});
    rules[730] = new Rule(-197, new int[]{128});
    rules[731] = new Rule(-197, new int[]{129});
    rules[732] = new Rule(-197, new int[]{126});
    rules[733] = new Rule(-201, new int[]{136});
    rules[734] = new Rule(-201, new int[]{138});
    rules[735] = new Rule(-264, new int[]{-266});
    rules[736] = new Rule(-264, new int[]{-267});
    rules[737] = new Rule(-267, new int[]{-82,136,-283});
    rules[738] = new Rule(-267, new int[]{-82,136,-278});
    rules[739] = new Rule(-266, new int[]{-82,138,-283});
    rules[740] = new Rule(-266, new int[]{-82,138,-278});
    rules[741] = new Rule(-268, new int[]{-97,119,-96});
    rules[742] = new Rule(-268, new int[]{-97,119,-268});
    rules[743] = new Rule(-268, new int[]{-199,-268});
    rules[744] = new Rule(-82, new int[]{-96});
    rules[745] = new Rule(-82, new int[]{-173});
    rules[746] = new Rule(-82, new int[]{-268});
    rules[747] = new Rule(-82, new int[]{-82,-198,-96});
    rules[748] = new Rule(-82, new int[]{-82,-198,-268});
    rules[749] = new Rule(-82, new int[]{-82,-198,-242});
    rules[750] = new Rule(-82, new int[]{-264});
    rules[751] = new Rule(-198, new int[]{118});
    rules[752] = new Rule(-198, new int[]{117});
    rules[753] = new Rule(-198, new int[]{131});
    rules[754] = new Rule(-198, new int[]{132});
    rules[755] = new Rule(-198, new int[]{133});
    rules[756] = new Rule(-198, new int[]{134});
    rules[757] = new Rule(-198, new int[]{130});
    rules[758] = new Rule(-57, new int[]{63,8,-284,9});
    rules[759] = new Rule(-58, new int[]{8,-102,100,-79,-324,-331,9});
    rules[760] = new Rule(-97, new int[]{-15});
    rules[761] = new Rule(-97, new int[]{-113});
    rules[762] = new Rule(-96, new int[]{56});
    rules[763] = new Rule(-96, new int[]{-15});
    rules[764] = new Rule(-96, new int[]{-57});
    rules[765] = new Rule(-96, new int[]{11,-69,12});
    rules[766] = new Rule(-96, new int[]{135,-96});
    rules[767] = new Rule(-96, new int[]{-199,-96});
    rules[768] = new Rule(-96, new int[]{142,-96});
    rules[769] = new Rule(-96, new int[]{-113});
    rules[770] = new Rule(-96, new int[]{-58});
    rules[771] = new Rule(-15, new int[]{-165});
    rules[772] = new Rule(-15, new int[]{-16});
    rules[773] = new Rule(-116, new int[]{-111,15,-111});
    rules[774] = new Rule(-116, new int[]{-111,15,-116});
    rules[775] = new Rule(-113, new int[]{-132,-111});
    rules[776] = new Rule(-113, new int[]{-111});
    rules[777] = new Rule(-113, new int[]{-116});
    rules[778] = new Rule(-113, new int[]{8,53,-147,110,-99,9});
    rules[779] = new Rule(-132, new int[]{141});
    rules[780] = new Rule(-132, new int[]{-132,141});
    rules[781] = new Rule(-9, new int[]{-180,-70});
    rules[782] = new Rule(-9, new int[]{-300,-70});
    rules[783] = new Rule(-321, new int[]{-147});
    rules[784] = new Rule(-321, new int[]{-321,7,-138});
    rules[785] = new Rule(-320, new int[]{-321});
    rules[786] = new Rule(-320, new int[]{-321,-298});
    rules[787] = new Rule(-17, new int[]{-111});
    rules[788] = new Rule(-17, new int[]{-15});
    rules[789] = new Rule(-358, new int[]{53,-147,110,-87,10});
    rules[790] = new Rule(-359, new int[]{-358});
    rules[791] = new Rule(-359, new int[]{-359,-358});
    rules[792] = new Rule(-112, new int[]{-111,8,-68,9});
    rules[793] = new Rule(-111, new int[]{-147});
    rules[794] = new Rule(-111, new int[]{-191});
    rules[795] = new Rule(-111, new int[]{42,-147});
    rules[796] = new Rule(-111, new int[]{8,-87,9});
    rules[797] = new Rule(-111, new int[]{8,-359,-87,9});
    rules[798] = new Rule(-111, new int[]{-257});
    rules[799] = new Rule(-111, new int[]{-294});
    rules[800] = new Rule(-111, new int[]{-15,7,-138});
    rules[801] = new Rule(-111, new int[]{-17,11,-71,12});
    rules[802] = new Rule(-111, new int[]{-17,17,-120,12});
    rules[803] = new Rule(-111, new int[]{76,-69,76});
    rules[804] = new Rule(-111, new int[]{-112});
    rules[805] = new Rule(-111, new int[]{-111,7,-148});
    rules[806] = new Rule(-111, new int[]{-58,7,-148});
    rules[807] = new Rule(-111, new int[]{-111,142});
    rules[808] = new Rule(-111, new int[]{-111,4,-298});
    rules[809] = new Rule(-67, new int[]{-71});
    rules[810] = new Rule(-67, new int[]{});
    rules[811] = new Rule(-68, new int[]{-72});
    rules[812] = new Rule(-68, new int[]{});
    rules[813] = new Rule(-69, new int[]{-77});
    rules[814] = new Rule(-69, new int[]{});
    rules[815] = new Rule(-77, new int[]{-92});
    rules[816] = new Rule(-77, new int[]{-77,100,-92});
    rules[817] = new Rule(-92, new int[]{-87});
    rules[818] = new Rule(-92, new int[]{-87,6,-87});
    rules[819] = new Rule(-166, new int[]{144});
    rules[820] = new Rule(-166, new int[]{147});
    rules[821] = new Rule(-165, new int[]{-167});
    rules[822] = new Rule(-165, new int[]{145});
    rules[823] = new Rule(-165, new int[]{146});
    rules[824] = new Rule(-167, new int[]{-166});
    rules[825] = new Rule(-167, new int[]{-167,-166});
    rules[826] = new Rule(-191, new int[]{45,-200});
    rules[827] = new Rule(-207, new int[]{10});
    rules[828] = new Rule(-207, new int[]{10,-206,10});
    rules[829] = new Rule(-208, new int[]{});
    rules[830] = new Rule(-208, new int[]{10,-206});
    rules[831] = new Rule(-206, new int[]{-209});
    rules[832] = new Rule(-206, new int[]{-206,10,-209});
    rules[833] = new Rule(-147, new int[]{143});
    rules[834] = new Rule(-147, new int[]{-151});
    rules[835] = new Rule(-147, new int[]{-152});
    rules[836] = new Rule(-147, new int[]{160});
    rules[837] = new Rule(-147, new int[]{87});
    rules[838] = new Rule(-138, new int[]{-147});
    rules[839] = new Rule(-138, new int[]{-292});
    rules[840] = new Rule(-138, new int[]{-293});
    rules[841] = new Rule(-148, new int[]{-147});
    rules[842] = new Rule(-148, new int[]{-292});
    rules[843] = new Rule(-148, new int[]{-191});
    rules[844] = new Rule(-209, new int[]{148});
    rules[845] = new Rule(-209, new int[]{150});
    rules[846] = new Rule(-209, new int[]{151});
    rules[847] = new Rule(-209, new int[]{152});
    rules[848] = new Rule(-209, new int[]{154});
    rules[849] = new Rule(-209, new int[]{153});
    rules[850] = new Rule(-210, new int[]{153});
    rules[851] = new Rule(-210, new int[]{152});
    rules[852] = new Rule(-210, new int[]{148});
    rules[853] = new Rule(-210, new int[]{151});
    rules[854] = new Rule(-151, new int[]{85});
    rules[855] = new Rule(-151, new int[]{86});
    rules[856] = new Rule(-152, new int[]{80});
    rules[857] = new Rule(-152, new int[]{78});
    rules[858] = new Rule(-150, new int[]{84});
    rules[859] = new Rule(-150, new int[]{83});
    rules[860] = new Rule(-150, new int[]{82});
    rules[861] = new Rule(-150, new int[]{81});
    rules[862] = new Rule(-292, new int[]{-150});
    rules[863] = new Rule(-292, new int[]{68});
    rules[864] = new Rule(-292, new int[]{64});
    rules[865] = new Rule(-292, new int[]{128});
    rules[866] = new Rule(-292, new int[]{22});
    rules[867] = new Rule(-292, new int[]{21});
    rules[868] = new Rule(-292, new int[]{63});
    rules[869] = new Rule(-292, new int[]{23});
    rules[870] = new Rule(-292, new int[]{129});
    rules[871] = new Rule(-292, new int[]{130});
    rules[872] = new Rule(-292, new int[]{131});
    rules[873] = new Rule(-292, new int[]{132});
    rules[874] = new Rule(-292, new int[]{133});
    rules[875] = new Rule(-292, new int[]{134});
    rules[876] = new Rule(-292, new int[]{135});
    rules[877] = new Rule(-292, new int[]{136});
    rules[878] = new Rule(-292, new int[]{137});
    rules[879] = new Rule(-292, new int[]{138});
    rules[880] = new Rule(-292, new int[]{24});
    rules[881] = new Rule(-292, new int[]{73});
    rules[882] = new Rule(-292, new int[]{91});
    rules[883] = new Rule(-292, new int[]{25});
    rules[884] = new Rule(-292, new int[]{26});
    rules[885] = new Rule(-292, new int[]{29});
    rules[886] = new Rule(-292, new int[]{30});
    rules[887] = new Rule(-292, new int[]{31});
    rules[888] = new Rule(-292, new int[]{71});
    rules[889] = new Rule(-292, new int[]{99});
    rules[890] = new Rule(-292, new int[]{32});
    rules[891] = new Rule(-292, new int[]{92});
    rules[892] = new Rule(-292, new int[]{33});
    rules[893] = new Rule(-292, new int[]{34});
    rules[894] = new Rule(-292, new int[]{27});
    rules[895] = new Rule(-292, new int[]{104});
    rules[896] = new Rule(-292, new int[]{101});
    rules[897] = new Rule(-292, new int[]{35});
    rules[898] = new Rule(-292, new int[]{36});
    rules[899] = new Rule(-292, new int[]{37});
    rules[900] = new Rule(-292, new int[]{40});
    rules[901] = new Rule(-292, new int[]{41});
    rules[902] = new Rule(-292, new int[]{42});
    rules[903] = new Rule(-292, new int[]{103});
    rules[904] = new Rule(-292, new int[]{43});
    rules[905] = new Rule(-292, new int[]{44});
    rules[906] = new Rule(-292, new int[]{46});
    rules[907] = new Rule(-292, new int[]{47});
    rules[908] = new Rule(-292, new int[]{48});
    rules[909] = new Rule(-292, new int[]{97});
    rules[910] = new Rule(-292, new int[]{49});
    rules[911] = new Rule(-292, new int[]{102});
    rules[912] = new Rule(-292, new int[]{50});
    rules[913] = new Rule(-292, new int[]{28});
    rules[914] = new Rule(-292, new int[]{51});
    rules[915] = new Rule(-292, new int[]{70});
    rules[916] = new Rule(-292, new int[]{98});
    rules[917] = new Rule(-292, new int[]{52});
    rules[918] = new Rule(-292, new int[]{53});
    rules[919] = new Rule(-292, new int[]{54});
    rules[920] = new Rule(-292, new int[]{55});
    rules[921] = new Rule(-292, new int[]{56});
    rules[922] = new Rule(-292, new int[]{57});
    rules[923] = new Rule(-292, new int[]{58});
    rules[924] = new Rule(-292, new int[]{59});
    rules[925] = new Rule(-292, new int[]{61});
    rules[926] = new Rule(-292, new int[]{105});
    rules[927] = new Rule(-292, new int[]{106});
    rules[928] = new Rule(-292, new int[]{109});
    rules[929] = new Rule(-292, new int[]{107});
    rules[930] = new Rule(-292, new int[]{108});
    rules[931] = new Rule(-292, new int[]{62});
    rules[932] = new Rule(-292, new int[]{74});
    rules[933] = new Rule(-292, new int[]{38});
    rules[934] = new Rule(-292, new int[]{39});
    rules[935] = new Rule(-292, new int[]{69});
    rules[936] = new Rule(-292, new int[]{148});
    rules[937] = new Rule(-292, new int[]{60});
    rules[938] = new Rule(-292, new int[]{139});
    rules[939] = new Rule(-292, new int[]{140});
    rules[940] = new Rule(-292, new int[]{79});
    rules[941] = new Rule(-292, new int[]{153});
    rules[942] = new Rule(-292, new int[]{152});
    rules[943] = new Rule(-292, new int[]{72});
    rules[944] = new Rule(-292, new int[]{154});
    rules[945] = new Rule(-292, new int[]{150});
    rules[946] = new Rule(-292, new int[]{151});
    rules[947] = new Rule(-292, new int[]{149});
    rules[948] = new Rule(-293, new int[]{45});
    rules[949] = new Rule(-200, new int[]{115});
    rules[950] = new Rule(-200, new int[]{116});
    rules[951] = new Rule(-200, new int[]{117});
    rules[952] = new Rule(-200, new int[]{118});
    rules[953] = new Rule(-200, new int[]{120});
    rules[954] = new Rule(-200, new int[]{121});
    rules[955] = new Rule(-200, new int[]{122});
    rules[956] = new Rule(-200, new int[]{123});
    rules[957] = new Rule(-200, new int[]{124});
    rules[958] = new Rule(-200, new int[]{125});
    rules[959] = new Rule(-200, new int[]{128});
    rules[960] = new Rule(-200, new int[]{129});
    rules[961] = new Rule(-200, new int[]{130});
    rules[962] = new Rule(-200, new int[]{131});
    rules[963] = new Rule(-200, new int[]{132});
    rules[964] = new Rule(-200, new int[]{133});
    rules[965] = new Rule(-200, new int[]{134});
    rules[966] = new Rule(-200, new int[]{135});
    rules[967] = new Rule(-200, new int[]{137});
    rules[968] = new Rule(-200, new int[]{139});
    rules[969] = new Rule(-200, new int[]{140});
    rules[970] = new Rule(-200, new int[]{-194});
    rules[971] = new Rule(-200, new int[]{119});
    rules[972] = new Rule(-194, new int[]{110});
    rules[973] = new Rule(-194, new int[]{111});
    rules[974] = new Rule(-194, new int[]{112});
    rules[975] = new Rule(-194, new int[]{113});
    rules[976] = new Rule(-194, new int[]{114});
    rules[977] = new Rule(-100, new int[]{18,-24,100,-23,9});
    rules[978] = new Rule(-23, new int[]{-100});
    rules[979] = new Rule(-23, new int[]{-147});
    rules[980] = new Rule(-24, new int[]{-23});
    rules[981] = new Rule(-24, new int[]{-24,100,-23});
    rules[982] = new Rule(-102, new int[]{-101});
    rules[983] = new Rule(-102, new int[]{-100});
    rules[984] = new Rule(-79, new int[]{-102});
    rules[985] = new Rule(-79, new int[]{-79,100,-102});
    rules[986] = new Rule(-322, new int[]{-147,127,-328});
    rules[987] = new Rule(-322, new int[]{8,9,-325,127,-328});
    rules[988] = new Rule(-322, new int[]{8,-147,5,-275,9,-325,127,-328});
    rules[989] = new Rule(-322, new int[]{8,-147,10,-326,9,-325,127,-328});
    rules[990] = new Rule(-322, new int[]{8,-147,5,-275,10,-326,9,-325,127,-328});
    rules[991] = new Rule(-322, new int[]{8,-102,100,-79,-324,-331,9,-335});
    rules[992] = new Rule(-322, new int[]{-100,-335});
    rules[993] = new Rule(-322, new int[]{-323});
    rules[994] = new Rule(-331, new int[]{});
    rules[995] = new Rule(-331, new int[]{10,-326});
    rules[996] = new Rule(-335, new int[]{-325,127,-328});
    rules[997] = new Rule(-323, new int[]{37,-325,127,-328});
    rules[998] = new Rule(-323, new int[]{37,8,9,-325,127,-328});
    rules[999] = new Rule(-323, new int[]{37,8,-326,9,-325,127,-328});
    rules[1000] = new Rule(-323, new int[]{44,127,-329});
    rules[1001] = new Rule(-323, new int[]{44,8,9,127,-329});
    rules[1002] = new Rule(-323, new int[]{44,8,-326,9,127,-329});
    rules[1003] = new Rule(-326, new int[]{-327});
    rules[1004] = new Rule(-326, new int[]{-326,10,-327});
    rules[1005] = new Rule(-327, new int[]{-158,-324});
    rules[1006] = new Rule(-324, new int[]{});
    rules[1007] = new Rule(-324, new int[]{5,-275});
    rules[1008] = new Rule(-325, new int[]{});
    rules[1009] = new Rule(-325, new int[]{5,-277});
    rules[1010] = new Rule(-330, new int[]{-255});
    rules[1011] = new Rule(-330, new int[]{-153});
    rules[1012] = new Rule(-330, new int[]{-318});
    rules[1013] = new Rule(-330, new int[]{-247});
    rules[1014] = new Rule(-330, new int[]{-124});
    rules[1015] = new Rule(-330, new int[]{-123});
    rules[1016] = new Rule(-330, new int[]{-125});
    rules[1017] = new Rule(-330, new int[]{-35});
    rules[1018] = new Rule(-330, new int[]{-301});
    rules[1019] = new Rule(-330, new int[]{-169});
    rules[1020] = new Rule(-330, new int[]{-248});
    rules[1021] = new Rule(-330, new int[]{-126});
    rules[1022] = new Rule(-330, new int[]{8,-4,9});
    rules[1023] = new Rule(-328, new int[]{-104});
    rules[1024] = new Rule(-328, new int[]{-330});
    rules[1025] = new Rule(-329, new int[]{-212});
    rules[1026] = new Rule(-329, new int[]{-4});
    rules[1027] = new Rule(-329, new int[]{-330});
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
    switch (action)
    {
      case 2: // parse_goal -> program_file
{ root = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 3: // parse_goal -> unit_file
{ root = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 4: // parse_goal -> parts
{ root = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 5: // parse_goal -> tkShortProgram, uses_clause_one_or_empty, 
              //               decl_sect_list_proc_func_only, stmt_list
{ 
			var stl = ValueStack[ValueStack.Depth-1].stn as statement_list;
			stl.left_logical_bracket = new token_info("");
			stl.right_logical_bracket = new token_info("");
			var ul = ValueStack[ValueStack.Depth-3].stn as uses_list;
			root = CurrentSemanticValue.stn = NewProgramModule(null, null, ul, new block(ValueStack[ValueStack.Depth-2].stn as declarations, stl, LocationStack[LocationStack.Depth-1]), new token_info(""), LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1])); 
		}
        break;
      case 6: // parse_goal -> tkShortSFProgram, uses_clause_one_or_empty, 
              //               decl_sect_list_proc_func_only, stmt_list
{
			var stl = ValueStack[ValueStack.Depth-1].stn as statement_list;
			stl.left_logical_bracket = new token_info("");
			stl.right_logical_bracket = new token_info("");
			var un = new unit_or_namespace(new ident_list("SF"),null);
			var ul = ValueStack[ValueStack.Depth-3].stn as uses_list;
			if (ul == null)
			//var un1 = new unit_or_namespace(new ident_list("School"),null);
				ul = new uses_list(un,null);
			else ul.Insert(0,un);
			//ul.Add(un1);
			root = CurrentSemanticValue.stn = NewProgramModule(null, null, ul, new block(ValueStack[ValueStack.Depth-2].stn as declarations, stl, CurrentLocationSpan), new token_info(""), LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1])); 
		}
        break;
      case 7: // parts -> tkParseModeExpression, expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 8: // parts -> tkParseModeExpression, tkType, type_decl_identifier
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 9: // parts -> tkParseModeType, variable_as_type
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 10: // parts -> tkParseModeStatement, stmt_or_expression
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 11: // stmt_or_expression -> expr
{ CurrentSemanticValue.stn = new expression_as_statement(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);}
        break;
      case 12: // stmt_or_expression -> assignment
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 13: // stmt_or_expression -> var_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 14: // optional_head_compiler_directives -> /* empty */
{ CurrentSemanticValue.ob = null; }
        break;
      case 15: // optional_head_compiler_directives -> head_compiler_directives
{ CurrentSemanticValue.ob = null; }
        break;
      case 16: // head_compiler_directives -> one_compiler_directive
{ CurrentSemanticValue.ob = null; }
        break;
      case 17: // head_compiler_directives -> head_compiler_directives, one_compiler_directive
{ CurrentSemanticValue.ob = null; }
        break;
      case 18: // one_compiler_directive -> tkDirectiveName, tkIdentifier
{
			parsertools.AddErrorFromResource("UNSUPPORTED_OLD_DIRECTIVES",CurrentLocationSpan);
			CurrentSemanticValue.ob = null;
        }
        break;
      case 19: // one_compiler_directive -> tkDirectiveName, tkStringLiteral
{
			parsertools.AddErrorFromResource("UNSUPPORTED_OLD_DIRECTIVES",CurrentLocationSpan);
			CurrentSemanticValue.ob = null;
        }
        break;
      case 20: // program_file -> program_header, optional_head_compiler_directives, uses_clause, 
               //                 program_block, optional_tk_point
{ 
			CurrentSemanticValue.stn = NewProgramModule(ValueStack[ValueStack.Depth-5].stn as program_name, ValueStack[ValueStack.Depth-4].ob, ValueStack[ValueStack.Depth-3].stn as uses_list, ValueStack[ValueStack.Depth-2].stn, ValueStack[ValueStack.Depth-1].ob, CurrentLocationSpan);
        }
        break;
      case 21: // optional_tk_point -> tkPoint
{ CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 22: // optional_tk_point -> tkSemiColon
{ CurrentSemanticValue.ob = null; }
        break;
      case 23: // optional_tk_point -> tkColon
{ CurrentSemanticValue.ob = null; }
        break;
      case 24: // optional_tk_point -> tkComma
{ CurrentSemanticValue.ob = null; }
        break;
      case 25: // optional_tk_point -> tkDotDot
{ CurrentSemanticValue.ob = null; }
        break;
      case 27: // program_header -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 28: // program_header -> tkProgram, identifier, program_heading_2
{ CurrentSemanticValue.stn = new program_name(ValueStack[ValueStack.Depth-2].id,CurrentLocationSpan); }
        break;
      case 29: // program_heading_2 -> tkSemiColon
{ CurrentSemanticValue.ob = null; }
        break;
      case 30: // program_heading_2 -> tkRoundOpen, program_param_list, tkRoundClose, tkSemiColon
{ CurrentSemanticValue.ob = null; }
        break;
      case 31: // program_param_list -> program_param
{ CurrentSemanticValue.ob = null; }
        break;
      case 32: // program_param_list -> program_param_list, tkComma, program_param
{ CurrentSemanticValue.ob = null; }
        break;
      case 33: // program_param -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 34: // program_block -> program_decl_sect_list, compound_stmt
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-2].stn as declarations, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
        }
        break;
      case 35: // program_decl_sect_list -> decl_sect_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 36: // ident_or_keyword_pointseparator_list -> identifier_or_keyword
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 37: // ident_or_keyword_pointseparator_list -> ident_or_keyword_pointseparator_list, 
               //                                         tkPoint, identifier_or_keyword
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 38: // uses_clause_one -> tkUses, used_units_list, tkSemiColon
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 39: // uses_clause_one_or_empty -> /* empty */
{ 
			CurrentSemanticValue.stn = null; 
		}
        break;
      case 40: // uses_clause_one_or_empty -> uses_clause_one
{
			if (parsertools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new uses_closure(ValueStack[ValueStack.Depth-1].stn as uses_list,CurrentLocationSpan);
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 41: // uses_clause -> /* empty */
{ 
			CurrentSemanticValue.stn = null; 
		}
        break;
      case 42: // uses_clause -> uses_clause, uses_clause_one
{ 
   			if (parsertools.buildTreeForFormatter)
   			{
	        	if (ValueStack[ValueStack.Depth-2].stn == null)
                {
	        		CurrentSemanticValue.stn = new uses_closure(ValueStack[ValueStack.Depth-1].stn as uses_list,CurrentLocationSpan);
                }
	        	else {
                    (ValueStack[ValueStack.Depth-2].stn as uses_closure).AddUsesList(ValueStack[ValueStack.Depth-1].stn as uses_list,CurrentLocationSpan);
                    CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
                }
   			}
   			else 
   			{
	        	if (ValueStack[ValueStack.Depth-2].stn == null)
                {
                    CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
                    CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
                }
	        	else 
                {
                    (ValueStack[ValueStack.Depth-2].stn as uses_list).AddUsesList(ValueStack[ValueStack.Depth-1].stn as uses_list,CurrentLocationSpan);
                    CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
                    CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
                }
			}
		}
        break;
      case 43: // used_units_list -> used_unit_name
{ 
		  CurrentSemanticValue.stn = new uses_list(ValueStack[ValueStack.Depth-1].stn as unit_or_namespace,CurrentLocationSpan);
        }
        break;
      case 44: // used_units_list -> used_units_list, tkComma, used_unit_name
{ 
		  CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as uses_list).Add(ValueStack[ValueStack.Depth-1].stn as unit_or_namespace, CurrentLocationSpan);
        }
        break;
      case 45: // used_unit_name -> ident_or_keyword_pointseparator_list
{ 
			CurrentSemanticValue.stn = new unit_or_namespace(ValueStack[ValueStack.Depth-1].stn as ident_list,CurrentLocationSpan); 
		}
        break;
      case 46: // used_unit_name -> ident_or_keyword_pointseparator_list, tkIn, tkStringLiteral
{ 
        	if (ValueStack[ValueStack.Depth-1].stn is char_const _cc)
        		ValueStack[ValueStack.Depth-1].stn = new string_const(_cc.cconst.ToString());
			CurrentSemanticValue.stn = new uses_unit_in(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].stn as string_const, CurrentLocationSpan);
        }
        break;
      case 47: // unit_file -> unit_header, interface_part, implementation_part, 
               //              initialization_part, tkPoint
{ 
			CurrentSemanticValue.stn = new unit_module(ValueStack[ValueStack.Depth-5].stn as unit_name, ValueStack[ValueStack.Depth-4].stn as interface_node, ValueStack[ValueStack.Depth-3].stn as implementation_node, 
			  (ValueStack[ValueStack.Depth-2].stn as initfinal_part).initialization_sect, (ValueStack[ValueStack.Depth-2].stn as initfinal_part).finalization_sect, /*$1 as attribute_list*/ null, CurrentLocationSpan);                    
		}
        break;
      case 48: // unit_file -> unit_header, abc_interface_part, initialization_part, tkPoint
{ 
			CurrentSemanticValue.stn = new unit_module(ValueStack[ValueStack.Depth-4].stn as unit_name, ValueStack[ValueStack.Depth-3].stn as interface_node, null, 
			  (ValueStack[ValueStack.Depth-2].stn as initfinal_part).initialization_sect, (ValueStack[ValueStack.Depth-2].stn as initfinal_part).finalization_sect, /*$1 as attribute_list*/ null, CurrentLocationSpan);
        }
        break;
      case 49: // unit_header -> unit_key_word, unit_name, tkSemiColon, 
               //                optional_head_compiler_directives
{ 
			CurrentSemanticValue.stn = NewUnitHeading(new ident(ValueStack[ValueStack.Depth-4].ti.text, LocationStack[LocationStack.Depth-4]), ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan); 
		}
        break;
      case 50: // unit_header -> tkNamespace, ident_or_keyword_pointseparator_list, tkSemiColon, 
               //                optional_head_compiler_directives
{
            CurrentSemanticValue.stn = NewNamespaceHeading(new ident(ValueStack[ValueStack.Depth-4].ti.text, LocationStack[LocationStack.Depth-4]), ValueStack[ValueStack.Depth-3].stn as ident_list, CurrentLocationSpan);
        }
        break;
      case 51: // unit_key_word -> tkUnit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 52: // unit_key_word -> tkLibrary
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 53: // unit_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 54: // interface_part -> tkInterface, uses_clause, interface_decl_sect_list
{ 
			CurrentSemanticValue.stn = new interface_node(ValueStack[ValueStack.Depth-1].stn as declarations, ValueStack[ValueStack.Depth-2].stn as uses_list, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1])); 
        }
        break;
      case 55: // implementation_part -> tkImplementation, uses_clause, decl_sect_list
{ 
			CurrentSemanticValue.stn = new implementation_node(ValueStack[ValueStack.Depth-2].stn as uses_list, ValueStack[ValueStack.Depth-1].stn as declarations, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1])); 
        }
        break;
      case 56: // abc_interface_part -> uses_clause, decl_sect_list
{ 
			CurrentSemanticValue.stn = new interface_node(ValueStack[ValueStack.Depth-1].stn as declarations, ValueStack[ValueStack.Depth-2].stn as uses_list, null, null); 
        }
        break;
      case 57: // initialization_part -> tkEnd
{ 
			CurrentSemanticValue.stn = new initfinal_part(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 58: // initialization_part -> tkInitialization, stmt_list, tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, null, null, CurrentLocationSpan);
        }
        break;
      case 59: // initialization_part -> tkInitialization, stmt_list, tkFinalization, stmt_list, 
               //                        tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-5].ti, ValueStack[ValueStack.Depth-4].stn as statement_list, ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, CurrentLocationSpan);
        }
        break;
      case 60: // initialization_part -> tkBegin, stmt_list, tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, null, null, CurrentLocationSpan);
        }
        break;
      case 61: // interface_decl_sect_list -> int_decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 62: // int_decl_sect_list1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations();  
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 63: // int_decl_sect_list1 -> int_decl_sect_list1, int_decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 64: // decl_sect_list -> decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 65: // decl_sect_list_proc_func_only -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations(); 
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 66: // decl_sect_list_proc_func_only -> decl_sect_list_proc_func_only, 
               //                                  attribute_declarations, 
               //                                  proc_func_decl_noclass
{
			var dcl = ValueStack[ValueStack.Depth-3].stn as declarations;
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).AssignAttrList(ValueStack[ValueStack.Depth-2].stn as attribute_list);
			if (dcl.Count == 0)			
				CurrentSemanticValue.stn = dcl.Add(ValueStack[ValueStack.Depth-1].stn as declaration, LocationStack[LocationStack.Depth-1]);
			else
			{
				var sc = dcl.source_context;
				sc = sc.Merge(ValueStack[ValueStack.Depth-1].stn.source_context);
				CurrentSemanticValue.stn = dcl.Add(ValueStack[ValueStack.Depth-1].stn as declaration, LocationStack[LocationStack.Depth-1]);
				CurrentSemanticValue.stn.source_context = sc;			
			}
		}
        break;
      case 67: // decl_sect_list1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations(); 
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 68: // decl_sect_list1 -> decl_sect_list1, decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 69: // inclass_decl_sect_list -> inclass_decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 70: // inclass_decl_sect_list1 -> /* empty */
{ 
        	CurrentSemanticValue.stn = new declarations(); 
        }
        break;
      case 71: // inclass_decl_sect_list1 -> inclass_decl_sect_list1, abc_decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 72: // int_decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 73: // int_decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 74: // int_decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 75: // int_decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 76: // int_decl_sect -> int_proc_header
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 77: // int_decl_sect -> int_func_header
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 78: // decl_sect -> label_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 79: // decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 80: // decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 81: // decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 82: // decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 83: // decl_sect -> proc_func_constr_destr_decl_with_attr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 84: // proc_func_constr_destr_decl -> proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 85: // proc_func_constr_destr_decl -> constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 86: // proc_func_constr_destr_decl_with_attr -> attribute_declarations, 
               //                                          proc_func_constr_destr_decl
{
		    (ValueStack[ValueStack.Depth-1].stn as procedure_definition).AssignAttrList(ValueStack[ValueStack.Depth-2].stn as attribute_list);
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 87: // abc_decl_sect -> label_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 88: // abc_decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 89: // abc_decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 90: // abc_decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 91: // abc_decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 92: // int_proc_header -> attribute_declarations, proc_header
{  
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
			(CurrentSemanticValue.td as procedure_header).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
        }
        break;
      case 93: // int_proc_header -> attribute_declarations, proc_header, tkForward, tkSemiColon
{  
			CurrentSemanticValue.td = NewProcedureHeader(ValueStack[ValueStack.Depth-4].stn as attribute_list, ValueStack[ValueStack.Depth-3].td as procedure_header, ValueStack[ValueStack.Depth-2].id as procedure_attribute, CurrentLocationSpan);
		}
        break;
      case 94: // int_func_header -> attribute_declarations, func_header
{  
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
			(CurrentSemanticValue.td as procedure_header).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
        }
        break;
      case 95: // int_func_header -> attribute_declarations, func_header, tkForward, tkSemiColon
{  
			CurrentSemanticValue.td = NewProcedureHeader(ValueStack[ValueStack.Depth-4].stn as attribute_list, ValueStack[ValueStack.Depth-3].td as procedure_header, ValueStack[ValueStack.Depth-2].id as procedure_attribute, CurrentLocationSpan);
		}
        break;
      case 96: // label_decl_sect -> tkLabel, label_list, tkSemiColon
{ 
			CurrentSemanticValue.stn = new label_definitions(ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
		}
        break;
      case 97: // label_list -> label_name
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 98: // label_list -> label_list, tkComma, label_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 99: // label_name -> tkInteger
{ 
			CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ex.ToString(), CurrentLocationSpan);
		}
        break;
      case 100: // label_name -> identifier
{ 
			CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; 
		}
        break;
      case 101: // const_decl_sect -> tkConst, const_decl
{ 
			CurrentSemanticValue.stn = new consts_definitions_list(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
		}
        break;
      case 102: // const_decl_sect -> const_decl_sect, const_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as consts_definitions_list).Add(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
		}
        break;
      case 103: // res_str_decl_sect -> tkResourceString, const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 104: // res_str_decl_sect -> res_str_decl_sect, const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 105: // type_decl_sect -> tkType, type_decl
{ 
            CurrentSemanticValue.stn = new type_declarations(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
		}
        break;
      case 106: // type_decl_sect -> type_decl_sect, type_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as type_declarations).Add(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
		}
        break;
      case 107: // var_decl_with_assign_var_tuple -> var_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 108: // var_decl_with_assign_var_tuple -> tkRoundOpen, identifier, tkComma, ident_list, 
                //                                   tkRoundClose, tkAssign, expr_l1, 
                //                                   tkSemiColon
{
			(ValueStack[ValueStack.Depth-5].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-7].id);
			ValueStack[ValueStack.Depth-5].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]);
			CurrentSemanticValue.stn = new var_tuple_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
		}
        break;
      case 109: // var_decl_sect -> tkVar, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 110: // var_decl_sect -> tkEvent, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);                        
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).is_event = true;
        }
        break;
      case 111: // var_decl_sect -> var_decl_sect, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as variable_definitions).Add(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 112: // const_decl -> only_const_decl, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 113: // only_const_decl -> const_name, tkEqual, init_const_expr
{ 
			CurrentSemanticValue.stn = new simple_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 114: // only_const_decl -> const_name, tkColon, type_ref, tkEqual, typed_const
{ 
			CurrentSemanticValue.stn = new typed_const_definition(ValueStack[ValueStack.Depth-5].id, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-3].td, CurrentLocationSpan);
		}
        break;
      case 115: // init_const_expr -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 116: // init_const_expr -> array_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 117: // const_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 118: // const_relop_expr -> const_simple_expr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 119: // const_relop_expr -> const_relop_expr, const_relop, const_simple_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 120: // const_expr -> const_relop_expr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 121: // const_expr -> question_constexpr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 122: // const_expr -> const_expr, tkDoubleQuestion, const_relop_expr
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 123: // question_constexpr -> const_expr, tkQuestion, const_expr, tkColon, const_expr
{ CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 124: // const_relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 125: // const_relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 126: // const_relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 127: // const_relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 128: // const_relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 129: // const_relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 130: // const_relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 131: // const_simple_expr -> const_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 132: // const_simple_expr -> const_simple_expr, const_addop, const_term
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 133: // const_addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 134: // const_addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 135: // const_addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 136: // const_addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 137: // as_is_constexpr -> const_term, typecast_op, simple_or_template_type_reference
{ 
			CurrentSemanticValue.ex = NewAsIsConstexpr(ValueStack[ValueStack.Depth-3].ex, (op_typecast)ValueStack[ValueStack.Depth-2].ob, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);                                
		}
        break;
      case 138: // power_constexpr -> const_factor_without_unary_op, tkStarStar, const_factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 139: // power_constexpr -> const_factor_without_unary_op, tkStarStar, power_constexpr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 140: // power_constexpr -> sign, power_constexpr
{ CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 141: // const_term -> const_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 142: // const_term -> as_is_constexpr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 143: // const_term -> power_constexpr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 144: // const_term -> const_term, const_mulop, const_factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 145: // const_term -> const_term, const_mulop, power_constexpr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 146: // const_mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 147: // const_mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 148: // const_mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 149: // const_mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 150: // const_mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 151: // const_mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 152: // const_mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 153: // const_factor_without_unary_op -> const_variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 154: // const_factor_without_unary_op -> tkRoundOpen, const_expr, tkRoundClose
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 155: // const_factor -> const_variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 156: // const_factor -> const_set
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 157: // const_factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 158: // const_factor -> tkAddressOf, const_factor
{ 
			CurrentSemanticValue.ex = new get_address(ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);  
		}
        break;
      case 159: // const_factor -> tkRoundOpen, const_expr, tkRoundClose
{ 
	 	    CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 160: // const_factor -> tkNot, const_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 161: // const_factor -> sign, const_factor
{ 
		    // �?�?�?наЯ ко�?�?ек�?иЯ �?ел�?�? конс�?ан�?
			if (ValueStack[ValueStack.Depth-2].op.type == Operators.Minus)
			{
			    var i64 = ValueStack[ValueStack.Depth-1].ex as int64_const;
				if (i64 != null && i64.val == (Int64)Int32.MaxValue + 1)
				{
					CurrentSemanticValue.ex = new int32_const(Int32.MinValue,CurrentLocationSpan);
					break;
				}
				var ui64 = ValueStack[ValueStack.Depth-1].ex as uint64_const;
				if (ui64 != null && ui64.val == (UInt64)Int64.MaxValue + 1)
				{
					CurrentSemanticValue.ex = new int64_const(Int64.MinValue,CurrentLocationSpan);
					break;
				}
				if (ui64 != null && ui64.val > (UInt64)Int64.MaxValue + 1)
				{
					parsertools.AddErrorFromResource("BAD_INT2",CurrentLocationSpan);
					break;
				}
			    // можно сдела�?�? в�?�?исление конс�?ан�?�? с вмон�?и�?ованн�?м мин�?сом
			}
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 162: // const_factor -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 163: // const_factor -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 164: // const_set -> tkSquareOpen, elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new pascal_set_constant(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 165: // const_set -> tkVertParen, elem_list, tkVertParen
{ 
			CurrentSemanticValue.ex = new array_const_new(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 166: // sign -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 167: // sign -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 168: // const_variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 169: // const_variable -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 170: // const_variable -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 171: // const_variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 172: // const_variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 173: // const_variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 174: // const_variable -> const_variable, const_variable_2
{
			CurrentSemanticValue.ex = NewConstVariable(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 175: // const_variable -> const_variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 176: // const_variable -> const_variable, tkSquareOpen, format_const_expr, 
                //                   tkSquareClose
{ 
    		var fe = ValueStack[ValueStack.Depth-2].ex as format_expr;
            if (!parsertools.buildTreeForFormatter)
            {
                if (fe.expr == null)
                    fe.expr = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
                if (fe.format1 == null)
                    fe.format1 = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
            }
    		CurrentSemanticValue.ex = new slice_expr(ValueStack[ValueStack.Depth-4].ex as addressed_value,fe.expr,fe.format1,fe.format2,CurrentLocationSpan);
		}
        break;
      case 177: // const_variable_2 -> tkPoint, identifier_or_keyword
{ 
			CurrentSemanticValue.ex = new dot_node(null, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 178: // const_variable_2 -> tkDeref
{ 
			CurrentSemanticValue.ex = new roof_dereference();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 179: // const_variable_2 -> tkRoundOpen, optional_const_func_expr_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 180: // const_variable_2 -> tkSquareOpen, const_elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new indexer(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 181: // optional_const_func_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 182: // optional_const_func_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 183: // const_elem_list -> const_elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 185: // const_elem_list1 -> const_elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 186: // const_elem_list1 -> const_elem_list1, tkComma, const_elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 187: // const_elem -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 188: // const_elem -> const_expr, tkDotDot, const_expr
{ 
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 189: // unsigned_number -> tkInteger
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 190: // unsigned_number -> tkHex
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 191: // unsigned_number -> tkFloat
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 192: // unsigned_number -> tkBigInteger
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 193: // typed_const -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 194: // typed_const -> array_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 195: // typed_const -> record_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 196: // array_const -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new array_const(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 198: // typed_const_list -> typed_const_list1
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 199: // typed_const_list1 -> typed_const_plus
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 200: // typed_const_list1 -> typed_const_list1, tkComma, typed_const_plus
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 201: // record_const -> tkRoundOpen, const_field_list, tkRoundClose
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 202: // const_field_list -> const_field_list_1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 203: // const_field_list -> const_field_list_1, tkSemiColon
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 204: // const_field_list_1 -> const_field
{ 
			CurrentSemanticValue.ex = new record_const(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
		}
        break;
      case 205: // const_field_list_1 -> const_field_list_1, tkSemiColon, const_field
{ 
			CurrentSemanticValue.ex = (ValueStack[ValueStack.Depth-3].ex as record_const).Add(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
		}
        break;
      case 206: // const_field -> const_field_name, tkColon, typed_const
{ 
			CurrentSemanticValue.stn = new record_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 207: // const_field_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 208: // type_decl -> attribute_declarations, simple_type_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = LocationStack[LocationStack.Depth-1];
        }
        break;
      case 209: // attribute_declarations -> attribute_declaration
{ 
			CurrentSemanticValue.stn = new attribute_list(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
    }
        break;
      case 210: // attribute_declarations -> attribute_declarations, attribute_declaration
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
		}
        break;
      case 211: // attribute_declarations -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 212: // attribute_declaration -> tkSquareOpen, one_or_some_attribute, tkSquareClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 213: // one_or_some_attribute -> one_attribute
{ 
			CurrentSemanticValue.stn = new simple_attribute_list(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
		}
        break;
      case 214: // one_or_some_attribute -> one_or_some_attribute, tkComma, one_attribute
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as simple_attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
		}
        break;
      case 215: // one_attribute -> attribute_variable
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 216: // one_attribute -> identifier, tkColon, attribute_variable
{  
			(ValueStack[ValueStack.Depth-1].stn as attribute).qualifier = ValueStack[ValueStack.Depth-3].id;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 217: // simple_type_decl -> type_decl_identifier, tkEqual, type_decl_type, tkSemiColon
{ 
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-4].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
		}
        break;
      case 218: // simple_type_decl -> template_identifier_with_equal, type_decl_type, tkSemiColon
{ 
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
		}
        break;
      case 219: // type_decl_identifier -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 220: // type_decl_identifier -> identifier, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 221: // template_identifier_with_equal -> identifier, tkLower, ident_list, 
                //                                   tkGreaterEqual
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-4].id.name, ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 222: // type_decl_type -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 223: // type_decl_type -> object_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 224: // simple_type_question -> simple_type, tkQuestion
{
            if (parsertools.buildTreeForFormatter)
   			{
                CurrentSemanticValue.td = ValueStack[ValueStack.Depth-2].td;
            }
            else
            {
                var l = new List<ident>();
                l.Add(new ident("System"));
                l.Add(new ident("Nullable"));
                CurrentSemanticValue.td = new template_type_reference(new named_type_reference(l), new template_param_list(ValueStack[ValueStack.Depth-2].td), CurrentLocationSpan);
            }
		}
        break;
      case 225: // simple_type_question -> template_type, tkQuestion
{
            if (parsertools.buildTreeForFormatter)
   			{
                CurrentSemanticValue.td = ValueStack[ValueStack.Depth-2].td;
            }
            else
            {
                var l = new List<ident>();
                l.Add(new ident("System"));
                l.Add(new ident("Nullable"));
                CurrentSemanticValue.td = new template_type_reference(new named_type_reference(l), new template_param_list(ValueStack[ValueStack.Depth-2].td), CurrentLocationSpan);
            }
		}
        break;
      case 226: // type_ref -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 227: // type_ref -> simple_type_question
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 228: // type_ref -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 229: // type_ref -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 230: // type_ref -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 231: // type_ref -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 232: // type_ref -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 233: // template_type -> simple_type_identifier, template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference(ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan); 
		}
        break;
      case 234: // template_type_params -> tkLower, template_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 235: // template_type_empty_params -> tkNotEqual
{
            var ntr = new named_type_reference(new ident(""), CurrentLocationSpan);
            
			CurrentSemanticValue.stn = new template_param_list(ntr, CurrentLocationSpan);
            ntr.source_context = new SourceContext(CurrentSemanticValue.stn.source_context.end_position.line_num, CurrentSemanticValue.stn.source_context.end_position.column_num, CurrentSemanticValue.stn.source_context.begin_position.line_num, CurrentSemanticValue.stn.source_context.begin_position.column_num);
		}
        break;
      case 236: // template_type_empty_params -> tkLower, template_empty_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 237: // template_param_list -> template_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 238: // template_param_list -> template_param_list, tkComma, template_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 239: // template_empty_param_list -> template_empty_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 240: // template_empty_param_list -> template_empty_param_list, tkComma, 
                //                              template_empty_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 241: // template_empty_param -> /* empty */
{ 
            CurrentSemanticValue.td = new named_type_reference(new ident(""), CurrentLocationSpan);
        }
        break;
      case 242: // template_param -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 243: // template_param -> simple_type, tkQuestion
{
            if (parsertools.buildTreeForFormatter)
   			{
                CurrentSemanticValue.td = ValueStack[ValueStack.Depth-2].td;
            }
            else
            {
                var l = new List<ident>();
                l.Add(new ident("System"));
                l.Add(new ident("Nullable"));
                CurrentSemanticValue.td = new template_type_reference(new named_type_reference(l), new template_param_list(ValueStack[ValueStack.Depth-2].td), CurrentLocationSpan);
            }
		}
        break;
      case 244: // template_param -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 245: // template_param -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 246: // template_param -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 247: // simple_type -> range_expr
{
	    	CurrentSemanticValue.td = parsertools.ConvertDotNodeOrIdentToNamedTypeReference(ValueStack[ValueStack.Depth-1].ex); 
	    }
        break;
      case 248: // simple_type -> range_expr, tkDotDot, range_expr
{ 
			CurrentSemanticValue.td = new diapason(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 249: // simple_type -> tkRoundOpen, enumeration_id_list, tkRoundClose
{ 
			CurrentSemanticValue.td = new enum_type_definition(ValueStack[ValueStack.Depth-2].stn as enumerator_list, CurrentLocationSpan);  
		}
        break;
      case 250: // range_expr -> range_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 251: // range_expr -> range_expr, const_addop, range_term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 252: // range_term -> range_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 253: // range_term -> range_term, const_mulop, range_factor
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 254: // range_factor -> simple_type_identifier
{ 
			CurrentSemanticValue.ex = parsertools.ConvertNamedTypeReferenceToDotNodeOrIdent(ValueStack[ValueStack.Depth-1].td as named_type_reference);
        }
        break;
      case 255: // range_factor -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 256: // range_factor -> sign, range_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 257: // range_factor -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 258: // range_factor -> range_factor, tkRoundOpen, const_elem_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value, ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 259: // simple_type_identifier -> identifier
{ 
			CurrentSemanticValue.td = new named_type_reference(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 260: // simple_type_identifier -> simple_type_identifier, tkPoint, 
                //                           identifier_or_keyword
{ 
			CurrentSemanticValue.td = (ValueStack[ValueStack.Depth-3].td as named_type_reference).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 261: // enumeration_id_list -> enumeration_id
{ 
			CurrentSemanticValue.stn = new enumerator_list(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 262: // enumeration_id_list -> enumeration_id_list, tkComma, enumeration_id
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as enumerator_list).Add(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 263: // enumeration_id -> type_ref
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan); 
		}
        break;
      case 264: // enumeration_id -> type_ref, tkEqual, expr
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 265: // pointer_type -> tkDeref, fptype
{ 
			CurrentSemanticValue.td = new ref_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 266: // structured_type -> array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 267: // structured_type -> record_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 268: // structured_type -> set_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 269: // structured_type -> file_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 270: // structured_type -> sequence_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 271: // sequence_type -> tkSequence, tkOf, type_ref
{
			CurrentSemanticValue.td = new sequence_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 272: // array_type -> tkArray, tkSquareOpen, simple_type_list, tkSquareClose, tkOf, 
                //               type_ref
{ 
			CurrentSemanticValue.td = new array_type(ValueStack[ValueStack.Depth-4].stn as indexers_types, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 273: // array_type -> unsized_array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 274: // unsized_array_type -> tkArray, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new array_type(null, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 275: // simple_type_list -> simple_type_or_
{ 
			CurrentSemanticValue.stn = new indexers_types(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 276: // simple_type_list -> simple_type_list, tkComma, simple_type_or_
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as indexers_types).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 277: // simple_type_or_ -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 278: // simple_type_or_ -> /* empty */
{ CurrentSemanticValue.td = null; }
        break;
      case 279: // set_type -> tkSet, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new set_type_definition(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 280: // file_type -> tkFile, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new file_type(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 281: // file_type -> tkFile
{ 
			CurrentSemanticValue.td = new file_type();  
			CurrentSemanticValue.td.source_context = CurrentLocationSpan;
		}
        break;
      case 282: // string_type -> tkIdentifier, tkSquareOpen, const_expr, tkSquareClose
{ 
			CurrentSemanticValue.td = new string_num_definition(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-4].id, CurrentLocationSpan);
		}
        break;
      case 283: // procedural_type -> procedural_type_kind
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 284: // procedural_type_kind -> proc_type_decl
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 285: // proc_type_decl -> tkProcedure, fp_list
{ 
			CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-1].stn as formal_parameters,null,null,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 286: // proc_type_decl -> tkFunction, fp_list, tkColon, fptype
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, null, null, null, ValueStack[ValueStack.Depth-1].td as type_definition, CurrentLocationSpan);
        }
        break;
      case 287: // proc_type_decl -> simple_type_identifier, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 288: // proc_type_decl -> template_type, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 289: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 290: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-4].stn as enumerator_list,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 291: // proc_type_decl -> simple_type_identifier, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 292: // proc_type_decl -> template_type, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 293: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,null,CurrentLocationSpan);
    	}
        break;
      case 294: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-5].stn as enumerator_list,null,CurrentLocationSpan);
    	}
        break;
      case 295: // object_type -> class_attributes, class_or_interface_keyword, 
                //                optional_base_classes, optional_where_section, 
                //                optional_component_list_seq_end
{ 
            var cd = NewObjectType((class_attribute)ValueStack[ValueStack.Depth-5].ob, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].stn as named_type_reference_list, ValueStack[ValueStack.Depth-2].stn as where_definition_list, ValueStack[ValueStack.Depth-1].stn as class_body_list, CurrentLocationSpan); 
			CurrentSemanticValue.td = cd;
		}
        break;
      case 296: // record_type -> tkRecord, optional_base_classes, optional_where_section, 
                //                member_list_section, tkEnd
{ 
			var nnrt = new class_definition(ValueStack[ValueStack.Depth-4].stn as named_type_reference_list, ValueStack[ValueStack.Depth-2].stn as class_body_list, class_keyword.Record, null, ValueStack[ValueStack.Depth-3].stn as where_definition_list, class_attribute.None, false, CurrentLocationSpan); 
			if (/*nnrt.body!=null && nnrt.body.class_def_blocks!=null && 
				nnrt.body.class_def_blocks.Count>0 &&*/ 
				nnrt.body.class_def_blocks[0].access_mod==null)
			{
                nnrt.body.class_def_blocks[0].access_mod = new access_modifer_node(access_modifer.public_modifer);
			}        
			CurrentSemanticValue.td = nnrt;
		}
        break;
      case 297: // class_attribute -> tkSealed
{ CurrentSemanticValue.ob = class_attribute.Sealed; }
        break;
      case 298: // class_attribute -> tkPartial
{ CurrentSemanticValue.ob = class_attribute.Partial; }
        break;
      case 299: // class_attribute -> tkAbstract
{ CurrentSemanticValue.ob = class_attribute.Abstract; }
        break;
      case 300: // class_attribute -> tkAuto
{ CurrentSemanticValue.ob = class_attribute.Auto; }
        break;
      case 301: // class_attribute -> tkStatic
{ CurrentSemanticValue.ob = class_attribute.Static; }
        break;
      case 302: // class_attributes -> /* empty */
{ 
			CurrentSemanticValue.ob = class_attribute.None; 
		}
        break;
      case 303: // class_attributes -> class_attributes1
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 304: // class_attributes1 -> class_attribute
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 305: // class_attributes1 -> class_attributes1, class_attribute
{
            if (((class_attribute)ValueStack[ValueStack.Depth-2].ob & (class_attribute)ValueStack[ValueStack.Depth-1].ob) == (class_attribute)ValueStack[ValueStack.Depth-1].ob)
                parsertools.AddErrorFromResource("ATTRIBUTE_REDECLARED",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.ob  = ((class_attribute)ValueStack[ValueStack.Depth-2].ob) | ((class_attribute)ValueStack[ValueStack.Depth-1].ob);
			//$$ = $1;
		}
        break;
      case 306: // class_or_interface_keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 307: // class_or_interface_keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 308: // class_or_interface_keyword -> tkTemplate
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-1].ti);
		}
        break;
      case 309: // class_or_interface_keyword -> tkTemplate, tkClass
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "c", CurrentLocationSpan);
		}
        break;
      case 310: // class_or_interface_keyword -> tkTemplate, tkRecord
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "r", CurrentLocationSpan);
		}
        break;
      case 311: // class_or_interface_keyword -> tkTemplate, tkInterface
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "i", CurrentLocationSpan);
		}
        break;
      case 312: // optional_component_list_seq_end -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 313: // optional_component_list_seq_end -> member_list_section, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 315: // optional_base_classes -> tkRoundOpen, base_classes_names_list, tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 316: // base_classes_names_list -> base_class_name
{ 
			CurrentSemanticValue.stn = new named_type_reference_list(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 317: // base_classes_names_list -> base_classes_names_list, tkComma, base_class_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as named_type_reference_list).Add(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 318: // base_class_name -> simple_type_identifier
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 319: // base_class_name -> template_type
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 320: // template_arguments -> tkLower, ident_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 321: // optional_where_section -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 322: // optional_where_section -> where_part_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 323: // where_part_list -> where_part
{ 
			CurrentSemanticValue.stn = new where_definition_list(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 324: // where_part_list -> where_part_list, where_part
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as where_definition_list).Add(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 325: // where_part -> tkWhere, ident_list, tkColon, type_ref_and_secific_list, 
                //               tkSemiColon
{ 
			CurrentSemanticValue.stn = new where_definition(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-2].stn as where_type_specificator_list, CurrentLocationSpan); 
		}
        break;
      case 326: // type_ref_and_secific_list -> type_ref_or_secific
{ 
			CurrentSemanticValue.stn = new where_type_specificator_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 327: // type_ref_and_secific_list -> type_ref_and_secific_list, tkComma, 
                //                              type_ref_or_secific
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as where_type_specificator_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 328: // type_ref_or_secific -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 329: // type_ref_or_secific -> tkClass
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefClass, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 330: // type_ref_or_secific -> tkRecord
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefValueType, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 331: // type_ref_or_secific -> tkConstructor
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 332: // member_list_section -> member_list
{ 
			CurrentSemanticValue.stn = new class_body_list(ValueStack[ValueStack.Depth-1].stn as class_members, CurrentLocationSpan);
        }
        break;
      case 333: // member_list_section -> member_list_section, ot_visibility_specifier, 
                //                        member_list
{ 
		    (ValueStack[ValueStack.Depth-1].stn as class_members).access_mod = ValueStack[ValueStack.Depth-2].stn as access_modifer_node;
			(ValueStack[ValueStack.Depth-3].stn as class_body_list).Add(ValueStack[ValueStack.Depth-1].stn as class_members,CurrentLocationSpan);
			
			if ((ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks[0].Count == 0)
                (ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks.RemoveAt(0);
			
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 334: // ot_visibility_specifier -> tkInternal
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.internal_modifer, CurrentLocationSpan); }
        break;
      case 335: // ot_visibility_specifier -> tkPublic
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.public_modifer, CurrentLocationSpan); }
        break;
      case 336: // ot_visibility_specifier -> tkProtected
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.protected_modifer, CurrentLocationSpan); }
        break;
      case 337: // ot_visibility_specifier -> tkPrivate
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.private_modifer, CurrentLocationSpan); }
        break;
      case 338: // member_list -> /* empty */
{ CurrentSemanticValue.stn = new class_members(); }
        break;
      case 339: // member_list -> field_or_const_definition_list, optional_semicolon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 340: // member_list -> method_decl_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 341: // member_list -> field_or_const_definition_list, tkSemiColon, method_decl_list
{  
			(ValueStack[ValueStack.Depth-3].stn as class_members).members.AddRange((ValueStack[ValueStack.Depth-1].stn as class_members).members);
			(ValueStack[ValueStack.Depth-3].stn as class_members).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 342: // ident_list -> identifier
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 343: // ident_list -> ident_list, tkComma, identifier
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 344: // optional_semicolon -> /* empty */
{ CurrentSemanticValue.ob = null; }
        break;
      case 345: // optional_semicolon -> tkSemiColon
{ CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 346: // field_or_const_definition_list -> field_or_const_definition
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 347: // field_or_const_definition_list -> field_or_const_definition_list, tkSemiColon, 
                //                                   field_or_const_definition
{   
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 348: // field_or_const_definition -> attribute_declarations, 
                //                              simple_field_or_const_definition
{  
		    (ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 349: // method_decl_list -> method_or_property_decl
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 350: // method_decl_list -> method_decl_list, method_or_property_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 351: // method_or_property_decl -> method_decl_withattr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 352: // method_or_property_decl -> property_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 353: // simple_field_or_const_definition -> tkConst, only_const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 354: // simple_field_or_const_definition -> field_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 355: // simple_field_or_const_definition -> class_or_static, field_definition
{ 
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).var_attr = definition_attribute.Static;
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 356: // class_or_static -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 357: // class_or_static -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 358: // field_definition -> var_decl_part
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 359: // field_definition -> tkEvent, ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, true, CurrentLocationSpan); 
        }
        break;
      case 360: // method_decl_withattr -> attribute_declarations, method_header
{  
			(ValueStack[ValueStack.Depth-1].td as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td;
        }
        break;
      case 361: // method_decl_withattr -> attribute_declarations, method_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
            if (ValueStack[ValueStack.Depth-1].stn is procedure_definition && (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
                (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
     }
        break;
      case 362: // method_decl -> inclass_proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 363: // method_decl -> inclass_constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 364: // method_header -> class_or_static, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 365: // method_header -> method_procfunc_header
{ 
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; 
		}
        break;
      case 366: // method_header -> tkAsync, class_or_static, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
			(ValueStack[ValueStack.Depth-1].td as procedure_header).IsAsync = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 367: // method_header -> tkAsync, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).IsAsync = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; 
		}
        break;
      case 368: // method_header -> constr_destr_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 369: // method_procfunc_header -> proc_func_header
{ 
			CurrentSemanticValue.td = NewProcfuncHeading(ValueStack[ValueStack.Depth-1].td as procedure_header);
		}
        break;
      case 370: // proc_func_header -> proc_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 371: // proc_func_header -> func_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 372: // constr_destr_header -> tkConstructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 373: // constr_destr_header -> class_or_static, tkConstructor, optional_proc_name, 
                //                        fp_list, optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,true,null,null,CurrentLocationSpan);
        }
        break;
      case 374: // constr_destr_header -> tkDestructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new destructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name, false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 375: // optional_proc_name -> proc_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 376: // optional_proc_name -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 377: // property_definition -> attribute_declarations, simple_property_definition
{  
			CurrentSemanticValue.stn = NewPropertyDefinition(ValueStack[ValueStack.Depth-2].stn as attribute_list, ValueStack[ValueStack.Depth-1].stn as declaration, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 378: // simple_property_definition -> tkProperty, func_name, property_interface, 
                //                               property_specifiers, tkSemiColon, 
                //                               array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        }
        break;
      case 379: // simple_property_definition -> tkProperty, func_name, property_interface, 
                //                               property_specifiers, tkSemiColon, 
                //                               property_modificator, tkSemiColon, 
                //                               array_defaultproperty
{ 
            proc_attribute pa = proc_attribute.attr_none;
            if (ValueStack[ValueStack.Depth-3].id.name.ToLower() == "virtual")
               	pa = proc_attribute.attr_virtual;
 			else if (ValueStack[ValueStack.Depth-3].id.name.ToLower() == "override") 
 			    pa = proc_attribute.attr_override;
            else if (ValueStack[ValueStack.Depth-3].id.name.ToLower() == "abstract") 
 			    pa = proc_attribute.attr_abstract;
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-7].stn as method_name, ValueStack[ValueStack.Depth-6].stn as property_interface, ValueStack[ValueStack.Depth-5].stn as property_accessors, pa, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        }
        break;
      case 380: // simple_property_definition -> class_or_static, tkProperty, func_name, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        	(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
        }
        break;
      case 381: // simple_property_definition -> class_or_static, tkProperty, func_name, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, property_modificator, tkSemiColon, 
                //                               array_defaultproperty
{ 
			parsertools.AddErrorFromResource("STATIC_PROPERTIES_CANNOT_HAVE_ATTRBUTE_{0}",LocationStack[LocationStack.Depth-3],ValueStack[ValueStack.Depth-3].id.name);        	
        }
        break;
      case 382: // simple_property_definition -> tkAuto, tkProperty, func_name, property_interface, 
                //                               optional_property_initialization, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
		}
        break;
      case 383: // simple_property_definition -> class_or_static, tkAuto, tkProperty, func_name, 
                //                               property_interface, 
                //                               optional_property_initialization, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
		}
        break;
      case 384: // optional_property_initialization -> tkAssign, expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 385: // optional_property_initialization -> /* empty */
{ CurrentSemanticValue.ex = null; }
        break;
      case 386: // array_defaultproperty -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 387: // array_defaultproperty -> tkDefault, tkSemiColon
{ 
			CurrentSemanticValue.stn = new property_array_default();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 388: // property_interface -> property_parameter_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new property_interface(ValueStack[ValueStack.Depth-3].stn as property_parameter_list, ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan);
        }
        break;
      case 389: // property_parameter_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 390: // property_parameter_list -> tkSquareOpen, parameter_decl_list, tkSquareClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 391: // parameter_decl_list -> parameter_decl
{ 
			CurrentSemanticValue.stn = new property_parameter_list(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
		}
        break;
      case 392: // parameter_decl_list -> parameter_decl_list, tkSemiColon, parameter_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as property_parameter_list).Add(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
		}
        break;
      case 393: // parameter_decl -> ident_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new property_parameter(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 394: // optional_read_expr -> expr_with_func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 395: // optional_read_expr -> /* empty */
{ CurrentSemanticValue.ex = null; }
        break;
      case 397: // property_specifiers -> tkRead, optional_read_expr, write_property_specifiers
{ 
        	if (ValueStack[ValueStack.Depth-2].ex == null || ValueStack[ValueStack.Depth-2].ex is ident) // с�?анда�?�?н�?е свойс�?ва
        	{
        		CurrentSemanticValue.stn = NewPropertySpecifiersRead(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-2].ex as ident, null, null, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan);
        	}
        	else // �?ас�?и�?енн�?е свойс�?ва
        	{
				var id = NewId("#GetGen", LocationStack[LocationStack.Depth-2]);
                procedure_definition pr = null;
                if (!parsertools.buildTreeForFormatter)
                    pr = CreateAndAddToClassReadFunc(ValueStack[ValueStack.Depth-2].ex, id, LocationStack[LocationStack.Depth-2]);
				CurrentSemanticValue.stn = NewPropertySpecifiersRead(ValueStack[ValueStack.Depth-3].id, id, pr, ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan); // $2 пе�?еда�?�?ся для �?о�?ма�?и�?ования 
			}
        }
        break;
      case 398: // property_specifiers -> tkWrite, unlabelled_stmt, read_property_specifiers
{ 
        	if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
        	{
        	
        		CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-3].id, null, null, null, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan);
        	}
        	else if (ValueStack[ValueStack.Depth-2].stn is procedure_call && (ValueStack[ValueStack.Depth-2].stn as procedure_call).is_ident) // с�?анда�?�?н�?е свойс�?ва
        	{
        	
        		CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-3].id, (ValueStack[ValueStack.Depth-2].stn as procedure_call).func_name as ident, null, null, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan);  // с�?а�?�?е свойс�?ва - с иден�?и�?ика�?о�?ом
        	}
        	else // �?ас�?и�?енн�?е свойс�?ва
        	{
				var id = NewId("#SetGen", LocationStack[LocationStack.Depth-2]);
                procedure_definition pr = null;
                if (!parsertools.buildTreeForFormatter)
                    pr = CreateAndAddToClassWriteProc(ValueStack[ValueStack.Depth-2].stn as statement,id,LocationStack[LocationStack.Depth-2]);
                if (parsertools.buildTreeForFormatter)
					CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-3].id, id, pr, ValueStack[ValueStack.Depth-2].stn as statement, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan); // $2 пе�?еда�?�?ся для �?о�?ма�?и�?ования
				else CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-3].id, id, pr, null, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan); 	
			}
        }
        break;
      case 400: // write_property_specifiers -> tkWrite, unlabelled_stmt
{ 
        	if (ValueStack[ValueStack.Depth-1].stn is empty_statement)
        	{
        	
        		CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-2].id, null, null, null, null, CurrentLocationSpan);
        	}
        	else if (ValueStack[ValueStack.Depth-1].stn is procedure_call && (ValueStack[ValueStack.Depth-1].stn as procedure_call).is_ident)
        	{
        		CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-2].id, (ValueStack[ValueStack.Depth-1].stn as procedure_call).func_name as ident, null, null, null, CurrentLocationSpan); // с�?а�?�?е свойс�?ва - с иден�?и�?ика�?о�?ом
        	}
        	else 
        	{
				var id = NewId("#SetGen", LocationStack[LocationStack.Depth-1]);
                procedure_definition pr = null;
                if (!parsertools.buildTreeForFormatter)
                    pr = CreateAndAddToClassWriteProc(ValueStack[ValueStack.Depth-1].stn as statement,id,LocationStack[LocationStack.Depth-1]);
                if (parsertools.buildTreeForFormatter)
					CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-2].id, id, pr, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
				else CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-2].id, id, pr, null, null, CurrentLocationSpan);	
			}
       }
        break;
      case 402: // read_property_specifiers -> tkRead, optional_read_expr
{ 
        	if (ValueStack[ValueStack.Depth-1].ex == null || ValueStack[ValueStack.Depth-1].ex is ident)
        	{
        		CurrentSemanticValue.stn = NewPropertySpecifiersRead(ValueStack[ValueStack.Depth-2].id, ValueStack[ValueStack.Depth-1].ex as ident, null, null, null, CurrentLocationSpan);
        	}
        	else 
        	{
				var id = NewId("#GetGen", LocationStack[LocationStack.Depth-1]);
                procedure_definition pr = null;
                if (!parsertools.buildTreeForFormatter)
                    pr = CreateAndAddToClassReadFunc(ValueStack[ValueStack.Depth-1].ex,id,LocationStack[LocationStack.Depth-1]);
				CurrentSemanticValue.stn = NewPropertySpecifiersRead(ValueStack[ValueStack.Depth-2].id, id, pr, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan);
			}
       }
        break;
      case 403: // var_decl -> var_decl_part, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 406: // var_decl_part -> ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, false, CurrentLocationSpan);
		}
        break;
      case 407: // var_decl_part -> ident_list, tkAssign, expr_with_func_decl_lambda
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, null, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan);		
		}
        break;
      case 408: // var_decl_part -> ident_list, tkColon, type_ref, tkAssignOrEqual, 
                //                  typed_var_init_expression
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan); 
		}
        break;
      case 409: // typed_var_init_expression -> typed_const_plus
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 410: // typed_var_init_expression -> const_simple_expr, tkDotDot, const_term
{ 
		if (parsertools.buildTreeForFormatter)
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		else 
			CurrentSemanticValue.ex = new diapason_expr_new(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); 
		}
        break;
      case 411: // typed_var_init_expression -> expl_func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 412: // typed_var_init_expression -> identifier, tkArrow, lambda_function_body
{  
			var idList = new ident_list(ValueStack[ValueStack.Depth-3].id, LocationStack[LocationStack.Depth-3]); 
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 413: // typed_var_init_expression -> tkRoundOpen, tkRoundClose, lambda_type_ref, 
                //                              tkArrow, lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 414: // typed_var_init_expression -> tkRoundOpen, typed_const_list, tkRoundClose, 
                //                              tkArrow, lambda_function_body
{  
		    var el = ValueStack[ValueStack.Depth-4].stn as expression_list;
		    var cnt = el.Count;
		    
			var idList = new ident_list();
			idList.source_context = LocationStack[LocationStack.Depth-4];
			
			for (int j = 0; j < cnt; j++)
			{
				if (!(el.expressions[j] is ident))
					parsertools.AddErrorFromResource("ONE_TKIDENTIFIER",el.expressions[j].source_context);
				idList.idents.Add(el.expressions[j] as ident);
			}	
				
			var any = new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-4]);	
				
			var formalPars = new formal_parameters(new typed_parameters(idList, any, parametr_kind.none, null, LocationStack[LocationStack.Depth-4]), LocationStack[LocationStack.Depth-4]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, any, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 415: // typed_var_init_expression -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 416: // typed_const_plus -> typed_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 417: // constr_destr_decl -> constr_destr_header, block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 418: // constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, tkAssign, 
                //                      unlabelled_stmt, tkSemiColon
{ 
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,false,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-6].Merge(LocationStack[LocationStack.Depth-2]));
            if (parsertools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
        }
        break;
      case 419: // constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
                //                      fp_list, tkAssign, unlabelled_stmt, tkSemiColon
{ 
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,true,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-2]));
            if (parsertools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
        }
        break;
      case 420: // inclass_constr_destr_decl -> constr_destr_header, inclass_block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 421: // inclass_constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, 
                //                              tkAssign, unlabelled_stmt, tkSemiColon
{ 
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,false,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-6].Merge(LocationStack[LocationStack.Depth-2]));
            if (parsertools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
        }
        break;
      case 422: // inclass_constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
                //                              fp_list, tkAssign, unlabelled_stmt, tkSemiColon
{ 
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,true,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-2]));
            if (parsertools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
        }
        break;
      case 423: // proc_func_decl -> proc_func_decl_noclass
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 424: // proc_func_decl -> class_or_static, proc_func_decl_noclass
{ 
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 425: // proc_func_decl -> tkAsync, proc_func_decl_noclass
{
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;		
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 426: // proc_func_decl -> tkAsync, class_or_static, proc_func_decl_noclass
{ 
        	(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 427: // proc_func_decl -> class_or_static, tkAsync, proc_func_decl_noclass
{ 
        	(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 428: // proc_func_decl_noclass -> proc_func_header, proc_func_external_block
{
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
        }
        break;
      case 429: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                           optional_method_modificators1, tkAssign, expr_l1, 
                //                           tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 430: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, expr_l1, 
                //                           tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].ex is dot_question_node)
				parsertools.AddErrorFromResource("DOT_QUECTION_IN_SHORT_FUN",LocationStack[LocationStack.Depth-2]);
	
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 431: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                           optional_method_modificators1, tkAssign, 
                //                           func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 432: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, 
                //                           func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 433: // proc_func_decl_noclass -> tkProcedure, proc_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, 
                //                           unlabelled_stmt, tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-2]);
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 434: // proc_func_decl_noclass -> proc_func_header, tkForward, tkSemiColon
{
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-3].td as procedure_header, null, CurrentLocationSpan);
            (CurrentSemanticValue.stn as procedure_definition).proc_header.proc_attributes.Add((procedure_attribute)ValueStack[ValueStack.Depth-2].id, ValueStack[ValueStack.Depth-2].id.source_context);
		}
        break;
      case 435: // inclass_proc_func_decl -> inclass_proc_func_decl_noclass
{ 
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
        }
        break;
      case 436: // inclass_proc_func_decl -> tkAsync, inclass_proc_func_decl_noclass
{ 
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
        }
        break;
      case 437: // inclass_proc_func_decl -> class_or_static, inclass_proc_func_decl_noclass
{ 
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
		    {
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			}
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 438: // inclass_proc_func_decl -> tkAsync, class_or_static, 
                //                           inclass_proc_func_decl_noclass
{ 
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
		    {
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			}
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 439: // inclass_proc_func_decl -> class_or_static, tkAsync, 
                //                           inclass_proc_func_decl_noclass
{ 
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
		    {
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			}
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 440: // inclass_proc_func_decl_noclass -> proc_func_header, inclass_block
{
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
		}
        break;
      case 441: // inclass_proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, 
                //                                   fptype, optional_method_modificators1, 
                //                                   tkAssign, expr_l1_func_decl_lambda, 
                //                                   tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
			if (parsertools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 442: // inclass_proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                                   optional_method_modificators1, tkAssign, 
                //                                   expr_l1_func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
			if (parsertools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 443: // inclass_proc_func_decl_noclass -> tkProcedure, proc_name, fp_list, 
                //                                   optional_method_modificators1, tkAssign, 
                //                                   unlabelled_stmt, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
			if (parsertools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 444: // proc_func_external_block -> block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 445: // proc_func_external_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 446: // proc_name -> func_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 447: // func_name -> func_meth_name_ident
{ 
			CurrentSemanticValue.stn = new method_name(null,null, ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan); 
		}
        break;
      case 448: // func_name -> func_class_name_ident_list, tkPoint, func_meth_name_ident
{ 
        	var ln = ValueStack[ValueStack.Depth-3].ob as List<ident>;
        	var cnt = ln.Count;
        	if (cnt == 1)
				CurrentSemanticValue.stn = new method_name(null, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
			else 	
				CurrentSemanticValue.stn = new method_name(ln, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
		}
        break;
      case 449: // func_class_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 450: // func_class_name_ident_list -> func_class_name_ident
{ 
			CurrentSemanticValue.ob = new List<ident>(); 
			(CurrentSemanticValue.ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
		}
        break;
      case 451: // func_class_name_ident_list -> func_class_name_ident_list, tkPoint, 
                //                               func_class_name_ident
{ 
			(ValueStack[ValueStack.Depth-3].ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob; 
		}
        break;
      case 452: // func_meth_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 453: // func_meth_name_ident -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 454: // func_meth_name_ident -> operator_name_ident, template_arguments
{ CurrentSemanticValue.id = new template_operator_name(null, ValueStack[ValueStack.Depth-1].stn as ident_list, ValueStack[ValueStack.Depth-2].ex as operator_name_ident, CurrentLocationSpan); }
        break;
      case 455: // func_name_with_template_args -> func_name_ident
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 456: // func_name_with_template_args -> func_name_ident, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 457: // func_name_ident -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 458: // proc_header -> tkProcedure, proc_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{ 
        	CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, CurrentLocationSpan); 
        }
        break;
      case 459: // func_header -> tkFunction, func_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, null, CurrentLocationSpan); 
		}
        break;
      case 460: // func_header -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                optional_method_modificators, optional_where_section
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, ValueStack[ValueStack.Depth-3].td as type_definition, CurrentLocationSpan); 
        }
        break;
      case 461: // external_block -> tkExternal, external_directive_ident, tkName, 
                //                   external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 462: // external_block -> tkExternal, external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-2].ex, null, CurrentLocationSpan); 
		}
        break;
      case 463: // external_block -> tkExternal, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(null, null, CurrentLocationSpan); 
		}
        break;
      case 464: // external_directive_ident -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 465: // external_directive_ident -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 466: // block -> decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 467: // inclass_block -> inclass_decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 468: // inclass_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 469: // fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 470: // fp_list -> tkRoundOpen, tkRoundClose
{ 
			CurrentSemanticValue.stn = null;
		}
        break;
      case 471: // fp_list -> tkRoundOpen, fp_sect_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			if (CurrentSemanticValue.stn != null)
				CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 472: // fp_sect_list -> fp_sect
{ 
			CurrentSemanticValue.stn = new formal_parameters(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
        }
        break;
      case 473: // fp_sect_list -> fp_sect_list, tkSemiColon, fp_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);   
        }
        break;
      case 474: // fp_sect -> attribute_declarations, simple_fp_sect
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as  attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 475: // simple_fp_sect -> param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan); 
		}
        break;
      case 476: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.var_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 477: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.const_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 478: // simple_fp_sect -> tkParams, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td,parametr_kind.params_parametr,null, CurrentLocationSpan);  
		}
        break;
      case 479: // simple_fp_sect -> param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.none, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 480: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.var_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 481: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.const_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 482: // param_name_list -> param_name
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 483: // param_name_list -> param_name_list, tkComma, param_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);  
		}
        break;
      case 484: // param_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 485: // fptype -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 486: // fptype_noproctype -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 487: // fptype_noproctype -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 488: // fptype_noproctype -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 489: // fptype_noproctype -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 490: // fptype_noproctype -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 491: // stmt -> unlabelled_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 492: // stmt -> label_name, tkColon, stmt
{ 
			CurrentSemanticValue.stn = new labeled_statement(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);  
		}
        break;
      case 493: // unlabelled_stmt -> /* empty */
{ 
			CurrentSemanticValue.stn = new empty_statement(); 
			CurrentSemanticValue.stn.source_context = null;
		}
        break;
      case 494: // unlabelled_stmt -> assignment
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 495: // unlabelled_stmt -> proc_call
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 496: // unlabelled_stmt -> goto_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 497: // unlabelled_stmt -> compound_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 498: // unlabelled_stmt -> if_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 499: // unlabelled_stmt -> case_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 500: // unlabelled_stmt -> repeat_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 501: // unlabelled_stmt -> while_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 502: // unlabelled_stmt -> for_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 503: // unlabelled_stmt -> with_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 504: // unlabelled_stmt -> inherited_message
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 505: // unlabelled_stmt -> try_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 506: // unlabelled_stmt -> raise_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 507: // unlabelled_stmt -> foreach_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 508: // unlabelled_stmt -> var_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 509: // unlabelled_stmt -> expr_as_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 510: // unlabelled_stmt -> lock_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 511: // unlabelled_stmt -> yield_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 512: // unlabelled_stmt -> yield_sequence_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 513: // unlabelled_stmt -> loop_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 514: // unlabelled_stmt -> match_with
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 515: // loop_stmt -> tkLoop, expr_l1, tkDo, unlabelled_stmt
{
			CurrentSemanticValue.stn = new loop_stmt(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].stn as statement,CurrentLocationSpan);
		}
        break;
      case 516: // yield_stmt -> tkYield, expr_l1_func_decl_lambda
{
			CurrentSemanticValue.stn = new yield_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 517: // yield_sequence_stmt -> tkYield, tkSequence, expr_l1_func_decl_lambda
{
			CurrentSemanticValue.stn = new yield_sequence_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 518: // var_stmt -> tkVar, var_decl_part
{ 
			CurrentSemanticValue.stn = new var_statement(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 519: // var_stmt -> tkRoundOpen, tkVar, identifier, tkComma, var_ident_list, 
                //             tkRoundClose, tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].ob as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].ob as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 520: // var_stmt -> tkVar, tkRoundOpen, identifier, tkComma, ident_list, tkRoundClose, 
                //             tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			ValueStack[ValueStack.Depth-4].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
	    }
        break;
      case 521: // assignment -> var_reference, assign_operator, expr_with_func_decl_lambda
{      
        	if (!(ValueStack[ValueStack.Depth-3].ex is addressed_value))
        		parsertools.AddErrorFromResource("LEFT_SIDE_CANNOT_BE_ASSIGNED_TO",CurrentLocationSpan);
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan);
        }
        break;
      case 522: // assignment -> tkRoundOpen, variable, tkComma, variable_list, tkRoundClose, 
                //               assign_operator, expr
{
			if (ValueStack[ValueStack.Depth-2].op.type != Operators.Assignment)
			    parsertools.AddErrorFromResource("ONLY_BASE_ASSIGNMENT_FOR_TUPLE",LocationStack[LocationStack.Depth-2]);
			(ValueStack[ValueStack.Depth-4].ob as addressed_value_list).Insert(0,ValueStack[ValueStack.Depth-6].ex as addressed_value);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_tuple(ValueStack[ValueStack.Depth-4].ob as addressed_value_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 523: // variable_list -> variable
{
		CurrentSemanticValue.ob = new addressed_value_list(ValueStack[ValueStack.Depth-1].ex as addressed_value,LocationStack[LocationStack.Depth-1]);
	}
        break;
      case 524: // variable_list -> variable_list, tkComma, variable
{
		(ValueStack[ValueStack.Depth-3].ob as addressed_value_list).Add(ValueStack[ValueStack.Depth-1].ex as addressed_value);
		(ValueStack[ValueStack.Depth-3].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
	}
        break;
      case 525: // var_ident_list -> tkVar, identifier
{
		CurrentSemanticValue.ob = new ident_list(ValueStack[ValueStack.Depth-1].id,CurrentLocationSpan);
	}
        break;
      case 526: // var_ident_list -> var_ident_list, tkComma, tkVar, identifier
{
		(ValueStack[ValueStack.Depth-4].ob as ident_list).Add(ValueStack[ValueStack.Depth-1].id);
		(ValueStack[ValueStack.Depth-4].ob as ident_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-4].ob;
	}
        break;
      case 527: // proc_call -> var_reference
{ 
			CurrentSemanticValue.stn = new procedure_call(ValueStack[ValueStack.Depth-1].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex is ident, CurrentLocationSpan); 
		}
        break;
      case 528: // goto_stmt -> tkGoto, label_name
{ 
			CurrentSemanticValue.stn = new goto_statement(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 529: // compound_stmt -> tkBegin, stmt_list, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			(CurrentSemanticValue.stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(CurrentSemanticValue.stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 530: // stmt_list -> stmt
{ 
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 531: // stmt_list -> stmt_list, tkSemiColon, stmt
{  
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as statement_list).Add(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
        }
        break;
      case 532: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan); 
        }
        break;
      case 533: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt, tkElse, unlabelled_stmt
{
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as statement, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 534: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, else_case, tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 535: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, tkSemiColon, else_case, 
                //               tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 536: // pattern_cases -> pattern_case
{
            CurrentSemanticValue.stn = new pattern_cases(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 537: // pattern_cases -> pattern_cases, tkSemiColon, pattern_case
{
            CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as pattern_cases).Add(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 538: // pattern_case -> pattern_optional_var, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 539: // pattern_case -> deconstruction_or_const_pattern, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 540: // pattern_case -> collection_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 541: // pattern_case -> tuple_pattern, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
		}
        break;
      case 542: // pattern_case -> tuple_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 543: // case_stmt -> tkCase, expr_l1, tkOf, case_list, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 544: // case_stmt -> tkCase, expr_l1, tkOf, case_list, tkSemiColon, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 545: // case_stmt -> tkCase, expr_l1, tkOf, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-4].ex, NewCaseItem(new empty_statement(), null), ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 546: // case_list -> case_item
{
			if (ValueStack[ValueStack.Depth-1].stn is empty_statement) 
				CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, null);
			else CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 547: // case_list -> case_list, tkSemiColon, case_item
{ 
			CurrentSemanticValue.stn = AddCaseItem(ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 548: // case_item -> case_label_list, tkColon, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new case_variant(ValueStack[ValueStack.Depth-3].stn as expression_list, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 549: // case_label_list -> case_label
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 550: // case_label_list -> case_label_list, tkComma, case_label
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 551: // case_label -> const_elem
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 552: // else_case -> /* empty */
{ CurrentSemanticValue.stn = null;}
        break;
      case 553: // else_case -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 554: // repeat_stmt -> tkRepeat, stmt_list, tkUntil, expr
{ 
		    CurrentSemanticValue.stn = new repeat_node(ValueStack[ValueStack.Depth-3].stn as statement_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-3].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-4].ti;
			(ValueStack[ValueStack.Depth-3].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-2].ti;
			ValueStack[ValueStack.Depth-3].stn.source_context = LocationStack[LocationStack.Depth-4].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 555: // while_stmt -> tkWhile, expr_l1, optional_tk_do, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewWhileStmt(ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);    
        }
        break;
      case 556: // optional_tk_do -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 557: // optional_tk_do -> /* empty */
{ CurrentSemanticValue.ti = null; }
        break;
      case 558: // lock_stmt -> tkLock, expr_l1, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new lock_stmt(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 559: // index_or_nothing -> tkIndex, tkIdentifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 560: // index_or_nothing -> /* empty */
{ CurrentSemanticValue.id = null; }
        break;
      case 561: // optional_type_specification -> tkColon, type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 562: // optional_type_specification -> /* empty */
{ CurrentSemanticValue.td = null; }
        break;
      case 563: // optional_var -> tkVar
{ CurrentSemanticValue.ob = true; }
        break;
      case 564: // optional_var -> /* empty */
{ CurrentSemanticValue.ob = false; }
        break;
      case 565: // for_cycle_type -> tkTo
{ CurrentSemanticValue.ob = for_cycle_type.to; }
        break;
      case 566: // for_cycle_type -> tkDownto
{ CurrentSemanticValue.ob = for_cycle_type.downto; }
        break;
      case 567: // foreach_stmt -> tkForeach, identifier, optional_type_specification, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
            if (ValueStack[ValueStack.Depth-6].td == null)
                parsertools.AddWarningFromResource("USING_UNLOCAL_FOREACH_VARIABLE", ValueStack[ValueStack.Depth-7].id.source_context);
        }
        break;
      case 568: // foreach_stmt -> tkForeach, tkVar, identifier, optional_type_specification, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
{ 
        	if (ValueStack[ValueStack.Depth-6].td == null)
				CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, new no_type_foreach(), ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
			else CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
        }
        break;
      case 569: // foreach_stmt -> tkForeach, tkVar, tkRoundOpen, ident_list, tkRoundClose, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
{ 
        	if (parsertools.buildTreeForFormatter)
        	{
        		var il = ValueStack[ValueStack.Depth-7].stn as ident_list;
        		il.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6]); // н�?жно для �?о�?ма�?и�?ования
        		CurrentSemanticValue.stn = new foreach_stmt_formatting(il,ValueStack[ValueStack.Depth-4].ex,ValueStack[ValueStack.Depth-1].stn as statement,ValueStack[ValueStack.Depth-3].id,CurrentLocationSpan);
        	}
        	else
        	{
        		// �?с�?�? п�?облема - непоня�?но, где здес�? сдела�?�? семан�?�?еский �?зел для п�?ове�?ки
        		// �?�?ове�?и�?�? можно и в foreach, но где-�?о должен б�?�?�? ма�?ке�?, �?�?о э�?о са�?а�?н�?й �?зел
        		// Нап�?име�?, иден�?и�?ика�?о�? #fe - но э�?о пло�?ая идея
                var id = NewId("#fe",LocationStack[LocationStack.Depth-7]);
                var tttt = new assign_var_tuple(ValueStack[ValueStack.Depth-7].stn as ident_list, id, CurrentLocationSpan);
                statement_list nine = ValueStack[ValueStack.Depth-1].stn is statement_list ? ValueStack[ValueStack.Depth-1].stn as statement_list : new statement_list(ValueStack[ValueStack.Depth-1].stn as statement,LocationStack[LocationStack.Depth-2]);
                nine.Insert(0,tttt);
			    var fe = new foreach_stmt(id, new no_type_foreach(), ValueStack[ValueStack.Depth-4].ex, nine, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
			    fe.ext = ValueStack[ValueStack.Depth-7].stn as ident_list;
			    CurrentSemanticValue.stn = fe;
			}
        }
        break;
      case 570: // for_stmt -> tkFor, optional_var, identifier, optional_type_specification, 
                //             tkAssign, expr_l1, for_cycle_type, expr_l1, optional_tk_do, 
                //             unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-9].ob, ValueStack[ValueStack.Depth-8].id, ValueStack[ValueStack.Depth-7].td, ValueStack[ValueStack.Depth-5].ex, (for_cycle_type)ValueStack[ValueStack.Depth-4].ob, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 571: // for_stmt -> tkFor, optional_var, identifier, optional_type_specification, 
                //             tkAssign, expr_l1, for_cycle_type, expr_l1, tkStep, expr_l1, tkDo, 
                //             unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-11].ob, ValueStack[ValueStack.Depth-10].id, ValueStack[ValueStack.Depth-9].td, ValueStack[ValueStack.Depth-7].ex, (for_cycle_type)ValueStack[ValueStack.Depth-6].ob, ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 572: // with_stmt -> tkWith, expr_list, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new with_statement(ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 573: // inherited_message -> tkInherited
{ 
			CurrentSemanticValue.stn = new inherited_message();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 574: // try_stmt -> tkTry, stmt_list, try_handler
{ 
			CurrentSemanticValue.stn = new try_stmt(ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].stn as try_handler, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			ValueStack[ValueStack.Depth-2].stn.source_context = LocationStack[LocationStack.Depth-3].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 575: // try_handler -> tkFinally, stmt_list, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_finally(ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan);
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(ValueStack[ValueStack.Depth-2].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
		}
        break;
      case 576: // try_handler -> tkExcept, exception_block, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_except((exception_block)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan);  
			if ((ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list != null)
			{
				(ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list.source_context = CurrentLocationSpan;
				(ValueStack[ValueStack.Depth-2].stn as exception_block).source_context = CurrentLocationSpan;
			}
		}
        break;
      case 577: // exception_block -> exception_handler_list, exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-2].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 578: // exception_block -> exception_handler_list, tkSemiColon, 
                //                    exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-3].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 579: // exception_block -> stmt_list
{ 
			CurrentSemanticValue.stn = new exception_block(ValueStack[ValueStack.Depth-1].stn as statement_list, null, null, LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 580: // exception_handler_list -> exception_handler
{ 
			CurrentSemanticValue.stn = new exception_handler_list(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 581: // exception_handler_list -> exception_handler_list, tkSemiColon, 
                //                           exception_handler
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as exception_handler_list).Add(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 582: // exception_block_else_branch -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 583: // exception_block_else_branch -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 584: // exception_handler -> tkOn, exception_identifier, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new exception_handler((ValueStack[ValueStack.Depth-3].stn as exception_ident).variable, (ValueStack[ValueStack.Depth-3].stn as exception_ident).type_name, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 585: // exception_identifier -> exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(null, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 586: // exception_identifier -> exception_variable, tkColon, 
                //                         exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(ValueStack[ValueStack.Depth-3].id, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 587: // exception_class_type_identifier -> simple_type_identifier
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 588: // exception_variable -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 589: // raise_stmt -> tkRaise
{ 
			CurrentSemanticValue.stn = new raise_stmt(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 590: // raise_stmt -> tkRaise, expr
{ 
			CurrentSemanticValue.stn = new raise_stmt(ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan);  
		}
        break;
      case 591: // expr_list -> expr_with_func_decl_lambda
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 592: // expr_list -> expr_list, tkComma, expr_with_func_decl_lambda
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 593: // expr_list_func_param -> expr_with_func_decl_lambda_ass
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 594: // expr_list_func_param -> expr_list_func_param, tkComma, 
                //                         expr_with_func_decl_lambda_ass
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 595: // expr_as_stmt -> allowable_expr_as_stmt
{ 
			CurrentSemanticValue.stn = new expression_as_statement(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 596: // allowable_expr_as_stmt -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 597: // expr_with_func_decl_lambda -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 598: // expr_with_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 599: // expr_with_func_decl_lambda -> tkInherited
{ CurrentSemanticValue.ex = new inherited_ident("", CurrentLocationSpan); }
        break;
      case 600: // expr_with_func_decl_lambda_ass -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 601: // expr_with_func_decl_lambda_ass -> identifier, tkAssign, expr_l1
{ CurrentSemanticValue.ex = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); }
        break;
      case 602: // expr_with_func_decl_lambda_ass -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 603: // expr_with_func_decl_lambda_ass -> tkInherited
{ CurrentSemanticValue.ex = new inherited_ident("", CurrentLocationSpan); }
        break;
      case 604: // expr -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 605: // expr -> format_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 606: // expr_l1 -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 607: // expr_l1 -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 608: // expr_l1 -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 609: // expr_l1_for_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 610: // expr_l1_for_question_expr -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 611: // expr_l1_for_new_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 612: // expr_l1_for_new_question_expr -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 613: // expr_l1_func_decl_lambda -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 614: // expr_l1_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 615: // expr_l1_for_lambda -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 616: // expr_l1_for_lambda -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 617: // expr_l1_for_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 618: // expr_dq -> relop_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 619: // expr_dq -> tkAwait, relop_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 620: // expr_dq -> expr_dq, tkDoubleQuestion, relop_expr
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 621: // sizeof_expr -> tkSizeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new sizeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, null, CurrentLocationSpan);  
		}
        break;
      case 622: // typeof_expr -> tkTypeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 623: // typeof_expr -> tkTypeOf, tkRoundOpen, empty_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 624: // question_expr -> expr_l1_for_question_expr, tkQuestion, 
                //                  expr_l1_for_question_expr, tkColon, 
                //                  expr_l1_for_question_expr
{ 
            if (ValueStack[ValueStack.Depth-3].ex is nil_const && ValueStack[ValueStack.Depth-1].ex is nil_const)
            	parsertools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 625: // new_question_expr -> tkIf, expr_l1_for_new_question_expr, tkThen, 
                //                      expr_l1_for_new_question_expr, tkElse, 
                //                      expr_l1_for_new_question_expr
{ 
        	if (parsertools.buildTreeForFormatter)
        	{
        		CurrentSemanticValue.ex = new if_expr_new(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        	}
        	else
        	{
            	if (ValueStack[ValueStack.Depth-3].ex is nil_const && ValueStack[ValueStack.Depth-1].ex is nil_const)
            		parsertools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",LocationStack[LocationStack.Depth-3]);
				CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
			}			
		}
        break;
      case 626: // empty_template_type_reference -> simple_type_identifier, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 627: // empty_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
        }
        break;
      case 628: // simple_or_template_type_reference -> simple_type_identifier
{ 
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 629: // simple_or_template_type_reference -> simple_type_identifier, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 630: // simple_or_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 631: // simple_or_template_or_question_type_reference -> 
                //                                                  simple_or_template_type_reference
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 632: // simple_or_template_or_question_type_reference -> simple_type_question
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 633: // optional_array_initializer -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = new array_const((expression_list)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan); 
		}
        break;
      case 635: // new_expr -> tkNew, simple_or_template_type_reference, 
                //             optional_expr_list_with_bracket
{
			CurrentSemanticValue.ex = new new_expr(ValueStack[ValueStack.Depth-2].td, ValueStack[ValueStack.Depth-1].stn as expression_list, false, null, CurrentLocationSpan);
        }
        break;
      case 636: // new_expr -> tkNew, simple_or_template_type_reference, tkSquareOpen, 
                //             optional_expr_list, tkSquareClose, optional_array_initializer
{
        	var el = ValueStack[ValueStack.Depth-3].stn as expression_list;
        	if (el == null)
        	{
        		var cnt = 0;
        		var ac = ValueStack[ValueStack.Depth-1].stn as array_const;
        		if (ac != null && ac.elements != null)
	        	    cnt = ac.elements.Count;
	        	else parsertools.AddErrorFromResource("WITHOUT_INIT_AND_SIZE",LocationStack[LocationStack.Depth-2]);
        		el = new expression_list(new int32_const(cnt),LocationStack[LocationStack.Depth-6]);
        	}	
			CurrentSemanticValue.ex = new new_expr(ValueStack[ValueStack.Depth-5].td, el, true, ValueStack[ValueStack.Depth-1].stn as array_const, CurrentLocationSpan);
        }
        break;
      case 637: // new_expr -> tkNew, tkClass, tkRoundOpen, list_fields_in_unnamed_object, 
                //             tkRoundClose
{
        // sugared node	
        	var l = ValueStack[ValueStack.Depth-2].ob as name_assign_expr_list;
        	var exprs = l.name_expr.Select(x=>x.expr.Clone() as expression).ToList();
        	var typename = "AnonymousType#"+Guid();
        	var type = new named_type_reference(typename,LocationStack[LocationStack.Depth-5]);
        	
			// node new_expr - for code generation of new node
			var ne = new new_expr(type, new expression_list(exprs), CurrentLocationSpan);
			// node unnamed_type_object - for formatting and code generation (new node and Anonymous class)
			CurrentSemanticValue.ex = new unnamed_type_object(l, true, ne, CurrentLocationSpan);
        }
        break;
      case 638: // field_in_unnamed_object -> identifier, tkAssign, expr_l1
{
		    if (ValueStack[ValueStack.Depth-1].ex is nil_const)
				parsertools.AddErrorFromResource("NIL_IN_UNNAMED_OBJECT",CurrentLocationSpan);		    
			CurrentSemanticValue.ob = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 639: // field_in_unnamed_object -> expr_l1
{
			ident name = null;
			var id = ValueStack[ValueStack.Depth-1].ex as ident;
			dot_node dot;
			if (id != null)
				name = id;
			else 
            {
            	dot = ValueStack[ValueStack.Depth-1].ex as dot_node;
            	if (dot != null)
            	{
            		name = dot.right as ident;
            	}            	
            } 
			if (name == null)
				parsertools.errors.Add(new bad_anon_type(parsertools.currentFileName, LocationStack[LocationStack.Depth-1], null));	
			CurrentSemanticValue.ob = new name_assign_expr(name,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 640: // list_fields_in_unnamed_object -> field_in_unnamed_object
{
			var l = new name_assign_expr_list();
			CurrentSemanticValue.ob = l.Add(ValueStack[ValueStack.Depth-1].ob as name_assign_expr);
		}
        break;
      case 641: // list_fields_in_unnamed_object -> list_fields_in_unnamed_object, tkComma, 
                //                                  field_in_unnamed_object
{
			var nel = ValueStack[ValueStack.Depth-3].ob as name_assign_expr_list;
			var ss = nel.name_expr.Select(ne=>ne.name.name).FirstOrDefault(x=>string.Compare(x,(ValueStack[ValueStack.Depth-1].ob as name_assign_expr).name.name,true)==0);
            if (ss != null)
            	parsertools.errors.Add(new anon_type_duplicate_name(parsertools.currentFileName, LocationStack[LocationStack.Depth-1], null));
			nel.Add(ValueStack[ValueStack.Depth-1].ob as name_assign_expr);
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
		}
        break;
      case 642: // optional_expr_list_with_bracket -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 643: // optional_expr_list_with_bracket -> tkRoundOpen, optional_expr_list, 
                //                                    tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 644: // relop_expr -> simple_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 645: // relop_expr -> relop_expr, relop, simple_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 646: // relop_expr -> relop_expr, relop, new_question_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 647: // relop_expr -> is_type_expr, tkRoundOpen, pattern_out_param_list, tkRoundClose
{
            var isTypeCheck = ValueStack[ValueStack.Depth-4].ex as typecast_node;
            var deconstructorPattern = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, isTypeCheck.type_def, null, CurrentLocationSpan); 
            CurrentSemanticValue.ex = new is_pattern_expr(isTypeCheck.expr, deconstructorPattern, CurrentLocationSpan);
        }
        break;
      case 648: // pattern -> simple_or_template_type_reference, tkRoundOpen, 
                //            pattern_out_param_list, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 649: // pattern_optional_var -> simple_or_template_type_reference, tkRoundOpen, 
                //                         pattern_out_param_list_optional_var, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 650: // deconstruction_or_const_pattern -> simple_or_template_type_reference, 
                //                                    tkRoundOpen, 
                //                                    pattern_out_param_list_optional_var, 
                //                                    tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 651: // deconstruction_or_const_pattern -> const_pattern_expr_list
{
		    CurrentSemanticValue.stn = new const_pattern(ValueStack[ValueStack.Depth-1].ob as List<syntax_tree_node>, CurrentLocationSpan); 
		}
        break;
      case 652: // const_pattern_expr_list -> const_pattern_expression
{ 
			CurrentSemanticValue.ob = new List<syntax_tree_node>(); 
			(CurrentSemanticValue.ob as List<syntax_tree_node>).Add(ValueStack[ValueStack.Depth-1].stn);
		}
        break;
      case 653: // const_pattern_expr_list -> const_pattern_expr_list, tkComma, 
                //                            const_pattern_expression
{ 
			var list = ValueStack[ValueStack.Depth-3].ob as List<syntax_tree_node>;
            list.Add(ValueStack[ValueStack.Depth-1].stn);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 654: // const_pattern_expression -> literal_or_number
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 655: // const_pattern_expression -> simple_or_template_type_reference
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 656: // const_pattern_expression -> tkNil
{ 
			CurrentSemanticValue.stn = new nil_const();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 657: // const_pattern_expression -> sizeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 658: // const_pattern_expression -> typeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 659: // collection_pattern -> tkSquareOpen, collection_pattern_expr_list, tkSquareClose
{
			CurrentSemanticValue.stn = new collection_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 660: // collection_pattern_expr_list -> collection_pattern_list_item
{
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 661: // collection_pattern_expr_list -> collection_pattern_expr_list, tkComma, 
                //                                 collection_pattern_list_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 662: // collection_pattern_list_item -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 663: // collection_pattern_list_item -> collection_pattern_var_item
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 664: // collection_pattern_list_item -> tkUnderscore
{
			CurrentSemanticValue.stn = new collection_pattern_wild_card(CurrentLocationSpan);
		}
        break;
      case 665: // collection_pattern_list_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 666: // collection_pattern_list_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 667: // collection_pattern_list_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 668: // collection_pattern_list_item -> tkDotDot
{
			CurrentSemanticValue.stn = new collection_pattern_gap_parameter(CurrentLocationSpan);
		}
        break;
      case 669: // collection_pattern_var_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new collection_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 670: // tuple_pattern -> tkRoundOpen, tuple_pattern_item_list, tkRoundClose
{
			if ((ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>).Count>6) 
				parsertools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",CurrentLocationSpan);
			CurrentSemanticValue.stn = new tuple_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 671: // tuple_pattern_item -> tkUnderscore
{ 
			CurrentSemanticValue.stn = new tuple_pattern_wild_card(CurrentLocationSpan); 
		}
        break;
      case 672: // tuple_pattern_item -> literal_or_number
{ 
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 673: // tuple_pattern_item -> sign, literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 674: // tuple_pattern_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new tuple_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 675: // tuple_pattern_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 676: // tuple_pattern_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 677: // tuple_pattern_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 678: // tuple_pattern_item_list -> tuple_pattern_item
{ 
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 679: // tuple_pattern_item_list -> tuple_pattern_item_list, tkComma, tuple_pattern_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 680: // pattern_out_param_list_optional_var -> pattern_out_param_optional_var
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 681: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkSemiColon, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 682: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkComma, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 683: // pattern_out_param_list -> pattern_out_param
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 684: // pattern_out_param_list -> pattern_out_param_list, tkSemiColon, 
                //                           pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 685: // pattern_out_param_list -> pattern_out_param_list, tkComma, pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 686: // pattern_out_param -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 687: // pattern_out_param -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 688: // pattern_out_param -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 689: // pattern_out_param -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 690: // pattern_out_param -> pattern
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 691: // pattern_out_param -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 692: // pattern_out_param -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 693: // pattern_out_param_optional_var -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 694: // pattern_out_param_optional_var -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 695: // pattern_out_param_optional_var -> sign, literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 696: // pattern_out_param_optional_var -> identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, false, CurrentLocationSpan);
        }
        break;
      case 697: // pattern_out_param_optional_var -> identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, false, CurrentLocationSpan);
        }
        break;
      case 698: // pattern_out_param_optional_var -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 699: // pattern_out_param_optional_var -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 700: // pattern_out_param_optional_var -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 701: // pattern_out_param_optional_var -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 702: // pattern_out_param_optional_var -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 703: // simple_expr_or_nothing -> simple_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 704: // simple_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 705: // const_expr_or_nothing -> const_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 706: // const_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 707: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing
{
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 708: // format_expr -> tkColon, simple_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 709: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing, tkColon, 
                //                simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 710: // format_expr -> tkColon, simple_expr_or_nothing, tkColon, simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 711: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 712: // format_const_expr -> tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 713: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing, tkColon, 
                //                      const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 714: // format_const_expr -> tkColon, const_expr_or_nothing, tkColon, const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 715: // relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 716: // relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 717: // relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 718: // relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 719: // relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 720: // relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 721: // relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 722: // relop -> tkNot, tkIn
{ 
			if (parsertools.buildTreeForFormatter)
				CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op;
			else
			{
				CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op;	
				CurrentSemanticValue.op.type = Operators.NotIn;
			}				
		}
        break;
      case 723: // simple_expr -> term1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 724: // simple_expr -> simple_expr, tkDotDot, term1
{ 
		if (parsertools.buildTreeForFormatter)
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		else 
			CurrentSemanticValue.ex = new diapason_expr_new(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); 
	}
        break;
      case 725: // term1 -> term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 726: // term1 -> term1, addop, term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 727: // term1 -> term1, addop, new_question_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 728: // addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 729: // addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 730: // addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 731: // addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 732: // addop -> tkCSharpStyleOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 733: // typecast_op -> tkAs
{ 
			CurrentSemanticValue.ob = op_typecast.as_op; 
		}
        break;
      case 734: // typecast_op -> tkIs
{ 
			CurrentSemanticValue.ob = op_typecast.is_op; 
		}
        break;
      case 735: // as_is_expr -> is_type_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 736: // as_is_expr -> as_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 737: // as_expr -> term, tkAs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 738: // as_expr -> term, tkAs, array_type
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
	    }
        break;
      case 739: // is_type_expr -> term, tkIs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 740: // is_type_expr -> term, tkIs, array_type
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
	    }
        break;
      case 741: // power_expr -> factor_without_unary_op, tkStarStar, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 742: // power_expr -> factor_without_unary_op, tkStarStar, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 743: // power_expr -> sign, power_expr
{ CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 744: // term -> factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 745: // term -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 746: // term -> power_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 747: // term -> term, mulop, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 748: // term -> term, mulop, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 749: // term -> term, mulop, new_question_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 750: // term -> as_is_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 751: // mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 752: // mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 753: // mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 754: // mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 755: // mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 756: // mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 757: // mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 758: // default_expr -> tkDefault, tkRoundOpen, 
                //                 simple_or_template_or_question_type_reference, tkRoundClose
{ 
			CurrentSemanticValue.ex = new default_operator(ValueStack[ValueStack.Depth-2].td as named_type_reference, CurrentLocationSpan);  
		}
        break;
      case 759: // tuple -> tkRoundOpen, expr_l1_or_unpacked, tkComma, expr_l1_or_unpacked_list, 
                //          lambda_type_ref, optional_full_lambda_fp_list, tkRoundClose
{
			if (ValueStack[ValueStack.Depth-6].ex is unpacked_list_of_ident_or_list) 
				parsertools.AddErrorFromResource("EXPRESSION_EXPECTED",LocationStack[LocationStack.Depth-6]);
			foreach (var ex in (ValueStack[ValueStack.Depth-4].stn as expression_list).expressions)
				if (ex is unpacked_list_of_ident_or_list)
					parsertools.AddErrorFromResource("EXPRESSION_EXPECTED",ex.source_context);
			if (!(ValueStack[ValueStack.Depth-3].td is lambda_inferred_type)) 
				parsertools.AddErrorFromResource("BAD_TUPLE",LocationStack[LocationStack.Depth-3]);
			if (ValueStack[ValueStack.Depth-2].stn != null) 
				parsertools.AddErrorFromResource("BAD_TUPLE",LocationStack[LocationStack.Depth-2]);

			if ((ValueStack[ValueStack.Depth-4].stn as expression_list).Count>6) 
				parsertools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",CurrentLocationSpan);
            (ValueStack[ValueStack.Depth-4].stn as expression_list).Insert(0,ValueStack[ValueStack.Depth-6].ex);
			CurrentSemanticValue.ex = new tuple_node(ValueStack[ValueStack.Depth-4].stn as expression_list,CurrentLocationSpan);
		}
        break;
      case 760: // factor_without_unary_op -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 761: // factor_without_unary_op -> var_reference
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 762: // factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 763: // factor -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 764: // factor -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 765: // factor -> tkSquareOpen, elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new pascal_set_constant(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 766: // factor -> tkNot, factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 767: // factor -> sign, factor
{
			if (ValueStack[ValueStack.Depth-2].op.type == Operators.Minus)
			{
			    var i64 = ValueStack[ValueStack.Depth-1].ex as int64_const;
				if (i64 != null && i64.val == (Int64)Int32.MaxValue + 1)
				{
					CurrentSemanticValue.ex = new int32_const(Int32.MinValue,CurrentLocationSpan);
					break;
				}
				var ui64 = ValueStack[ValueStack.Depth-1].ex as uint64_const;
				if (ui64 != null && ui64.val == (UInt64)Int64.MaxValue + 1)
				{
					CurrentSemanticValue.ex = new int64_const(Int64.MinValue,CurrentLocationSpan);
					break;
				}
				if (ui64 != null && ui64.val > (UInt64)Int64.MaxValue + 1)
				{
					parsertools.AddErrorFromResource("BAD_INT2",CurrentLocationSpan);
					break;
				}
			    // можно сдела�?�? в�?�?исление конс�?ан�?�? с вмон�?и�?ованн�?м мин�?сом
			}
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan);
		}
        break;
      case 768: // factor -> tkDeref, factor
{
            CurrentSemanticValue.ex = new index(ValueStack[ValueStack.Depth-1].ex, true, CurrentLocationSpan);
        }
        break;
      case 769: // factor -> var_reference
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 770: // factor -> tuple
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 771: // literal_or_number -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 772: // literal_or_number -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 773: // var_question_point -> variable, tkQuestionPoint, variable
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 774: // var_question_point -> variable, tkQuestionPoint, var_question_point
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 775: // var_reference -> var_address, variable
{
			CurrentSemanticValue.ex = NewVarReference(ValueStack[ValueStack.Depth-2].stn as get_address, ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);
		}
        break;
      case 776: // var_reference -> variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 777: // var_reference -> var_question_point
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 778: // var_reference -> tkRoundOpen, tkVar, identifier, tkAssign, expr_dq, 
                //                  tkRoundClose
{ CurrentSemanticValue.ex = new let_var_expr(ValueStack[ValueStack.Depth-4].id,ValueStack[ValueStack.Depth-2].ex,CurrentLocationSpan); }
        break;
      case 779: // var_address -> tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(CurrentLocationSpan);
		}
        break;
      case 780: // var_address -> var_address, tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(ValueStack[ValueStack.Depth-2].stn as get_address, CurrentLocationSpan);
		}
        break;
      case 781: // attribute_variable -> simple_type_identifier, optional_expr_list_with_bracket
{ 
			CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
		}
        break;
      case 782: // attribute_variable -> template_type, optional_expr_list_with_bracket
{
            CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 783: // dotted_identifier -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 784: // dotted_identifier -> dotted_identifier, tkPoint, identifier_or_keyword
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parsertools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
		}
        break;
      case 785: // variable_as_type -> dotted_identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;}
        break;
      case 786: // variable_as_type -> dotted_identifier, template_type_params
{ CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-2].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);   }
        break;
      case 787: // variable_or_literal_or_number -> variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 788: // variable_or_literal_or_number -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 789: // var_with_init_for_expr_with_let -> tkVar, identifier, tkAssign, expr, 
                //                                    tkSemiColon
{
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-4].id as addressed_value, ValueStack[ValueStack.Depth-2].ex, Operators.Assignment, CurrentLocationSpan);
		}
        break;
      case 790: // var_with_init_for_expr_with_let_list -> var_with_init_for_expr_with_let
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 791: // var_with_init_for_expr_with_let_list -> var_with_init_for_expr_with_let_list, 
                //                                         var_with_init_for_expr_with_let
{
			ValueStack[ValueStack.Depth-2].stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
		}
        break;
      case 792: // proc_func_call -> variable, tkRoundOpen, optional_expr_list_func_param, 
                //                   tkRoundClose
{
			if (ValueStack[ValueStack.Depth-4].ex is index)
				parsertools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-4], "^");
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 793: // variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 794: // variable -> operator_name_ident
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 795: // variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 796: // variable -> tkRoundOpen, expr, tkRoundClose
{
		    if (!parsertools.buildTreeForFormatter) 
            {
                ValueStack[ValueStack.Depth-2].ex.source_context = CurrentLocationSpan;
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
            } 
			else CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
        }
        break;
      case 797: // variable -> tkRoundOpen, var_with_init_for_expr_with_let_list, expr, 
                //             tkRoundClose
{
		    if (!parsertools.buildTreeForFormatter) 
            {
                ValueStack[ValueStack.Depth-2].ex.source_context = CurrentLocationSpan;
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
            } 
			else CurrentSemanticValue.ex = new expression_with_let(ValueStack[ValueStack.Depth-3].stn as statement_list, ValueStack[ValueStack.Depth-3].stn as expression, CurrentLocationSpan);
		}
        break;
      case 798: // variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 799: // variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 800: // variable -> literal_or_number, tkPoint, identifier_or_keyword
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parsertools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");		
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 801: // variable -> variable_or_literal_or_number, tkSquareOpen, expr_list, 
                //             tkSquareClose
{
        	var el = ValueStack[ValueStack.Depth-2].stn as expression_list; // SSM 10/03/16
        	if (el.Count==1 && el.expressions[0] is format_expr) 
        	{
        		var fe = el.expressions[0] as format_expr;
                if (!parsertools.buildTreeForFormatter)
                {
                    if (fe.expr == null)
                        fe.expr = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
                    if (fe.format1 == null)
                        fe.format1 = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
                }
        		CurrentSemanticValue.ex = new slice_expr(ValueStack[ValueStack.Depth-4].ex as addressed_value,fe.expr,fe.format1,fe.format2,CurrentLocationSpan);
			}   
			// многоме�?н�?е с�?ез�?
            else if (el.expressions.Any(e => e is format_expr))
            {
            	if (el.expressions.Count > 4)
            		parsertools.AddErrorFromResource("SLICES_OF MULTIDIMENSIONAL_ARRAYS_ALLOW_ONLY_FOR_RANK_LT_5",CurrentLocationSpan); // С�?ез�? многоме�?н�?�? массивов �?аз�?е�?ен�? �?ол�?ко для массивов �?азме�?нос�?и < 5  
                var ll = new List<Tuple<expression, expression, expression>>();
                foreach (var ex in el.expressions)
                {
                    if (ex is format_expr fe)
                    {
                        if (fe.expr == null)
                            fe.expr = new int32_const(int.MaxValue, fe.source_context);
                        if (fe.format1 == null)
                            fe.format1 = new int32_const(int.MaxValue, fe.source_context);
                        if (fe.format2 == null)
                            fe.format2 = new int32_const(1, fe.source_context);
                        ll.Add(Tuple.Create(fe.expr, fe.format1, fe.format2));
                    }
                    else
                    {
                    	ll.Add(Tuple.Create(ex, (expression)new int32_const(0, ex.source_context), (expression)new int32_const(int.MaxValue, ex.source_context))); // скаля�?ное зна�?ение вмес�?о с�?еза
                    }
				}
				var sle = new slice_expr(ValueStack[ValueStack.Depth-4].ex as addressed_value,null,null,null,CurrentLocationSpan);
				sle.slices = ll;
				CurrentSemanticValue.ex = sle;
            }
			else CurrentSemanticValue.ex = new indexer(ValueStack[ValueStack.Depth-4].ex as addressed_value, el, CurrentLocationSpan);
        }
        break;
      case 802: // variable -> variable_or_literal_or_number, tkQuestionSquareOpen, format_expr, 
                //             tkSquareClose
{
        	var fe = ValueStack[ValueStack.Depth-2].ex as format_expr; // SSM 9/01/17
            if (!parsertools.buildTreeForFormatter)
            {
                if (fe.expr == null)
                    fe.expr = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
                if (fe.format1 == null)
                    fe.format1 = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
            }
      		CurrentSemanticValue.ex = new slice_expr_question(ValueStack[ValueStack.Depth-4].ex as addressed_value,fe.expr,fe.format1,fe.format2,CurrentLocationSpan);
        }
        break;
      case 803: // variable -> tkVertParen, elem_list, tkVertParen
{ 
			CurrentSemanticValue.ex = new array_const_new(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 804: // variable -> proc_func_call
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 805: // variable -> variable, tkPoint, identifier_keyword_operatorname
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parsertools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 806: // variable -> tuple, tkPoint, identifier_keyword_operatorname
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 807: // variable -> variable, tkDeref
{
			CurrentSemanticValue.ex = new roof_dereference(ValueStack[ValueStack.Depth-2].ex as addressed_value,CurrentLocationSpan);
        }
        break;
      case 808: // variable -> variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 809: // optional_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 810: // optional_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 811: // optional_expr_list_func_param -> expr_list_func_param
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 812: // optional_expr_list_func_param -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 813: // elem_list -> elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 814: // elem_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 815: // elem_list1 -> elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 816: // elem_list1 -> elem_list1, tkComma, elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 817: // elem -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 818: // elem -> expr, tkDotDot, expr
{ CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 819: // one_literal -> tkStringLiteral
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 820: // one_literal -> tkAsciiChar
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 821: // literal -> literal_list
{ 
			CurrentSemanticValue.ex = NewLiteral(ValueStack[ValueStack.Depth-1].stn as literal_const_line);
        }
        break;
      case 822: // literal -> tkFormatStringLiteral
{
            if (parsertools.buildTreeForFormatter)
   			{
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as string_const;
            }
            else
            {
                CurrentSemanticValue.ex = NewFormatString(ValueStack[ValueStack.Depth-1].stn as string_const);
            }
        }
        break;
      case 823: // literal -> tkMultilineStringLiteral
{
            if (parsertools.buildTreeForFormatter)
   			{
   				var sc = ValueStack[ValueStack.Depth-1].stn as string_const;
   				sc.IsMultiline = true;
                CurrentSemanticValue.ex = sc;
            }
            else
            {
                CurrentSemanticValue.ex = NewLiteral(new literal_const_line(ValueStack[ValueStack.Depth-1].stn as literal, CurrentLocationSpan));
            }
        }
        break;
      case 824: // literal_list -> one_literal
{ 
			CurrentSemanticValue.stn = new literal_const_line(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 825: // literal_list -> literal_list, one_literal
{ 
        	var line = ValueStack[ValueStack.Depth-2].stn as literal_const_line;
            if (line.literals.Last() is string_const && ValueStack[ValueStack.Depth-1].ex is string_const)
            	parsertools.AddErrorFromResource("TWO_STRING_LITERALS_IN_SUCCESSION",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.stn = line.Add(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 826: // operator_name_ident -> tkOperator, overload_operator
{ 
			CurrentSemanticValue.ex = new operator_name_ident((ValueStack[ValueStack.Depth-1].op as op_type_node).text, (ValueStack[ValueStack.Depth-1].op as op_type_node).type, CurrentLocationSpan);
		}
        break;
      case 827: // optional_method_modificators -> tkSemiColon
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 828: // optional_method_modificators -> tkSemiColon, meth_modificators, tkSemiColon
{ 
			//parsertools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 829: // optional_method_modificators1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 830: // optional_method_modificators1 -> tkSemiColon, meth_modificators
{ 
			//parsertools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 831: // meth_modificators -> meth_modificator
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan); 
		}
        break;
      case 832: // meth_modificators -> meth_modificators, tkSemiColon, meth_modificator
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as procedure_attributes_list).Add(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan);  
		}
        break;
      case 833: // identifier -> tkIdentifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 834: // identifier -> property_specifier_directives
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 835: // identifier -> non_reserved
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 836: // identifier -> tkStep
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 837: // identifier -> tkIndex
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 838: // identifier_or_keyword -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 839: // identifier_or_keyword -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 840: // identifier_or_keyword -> reserved_keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 841: // identifier_keyword_operatorname -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 842: // identifier_keyword_operatorname -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 843: // identifier_keyword_operatorname -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 844: // meth_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 845: // meth_modificator -> tkOverload
{ 
            CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id;
            parsertools.AddWarningFromResource("OVERLOAD_IS_NOT_USED", ValueStack[ValueStack.Depth-1].id.source_context);
        }
        break;
      case 846: // meth_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 847: // meth_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 848: // meth_modificator -> tkExtensionMethod
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 849: // meth_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 850: // property_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 851: // property_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 852: // property_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 853: // property_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 854: // property_specifier_directives -> tkRead
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 855: // property_specifier_directives -> tkWrite
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 856: // non_reserved -> tkName
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 857: // non_reserved -> tkNew
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 858: // visibility_specifier -> tkInternal
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 859: // visibility_specifier -> tkPublic
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 860: // visibility_specifier -> tkProtected
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 861: // visibility_specifier -> tkPrivate
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 862: // keyword -> visibility_specifier
{ 
			CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  
		}
        break;
      case 863: // keyword -> tkSealed
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 864: // keyword -> tkTemplate
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 865: // keyword -> tkOr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 866: // keyword -> tkTypeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 867: // keyword -> tkSizeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 868: // keyword -> tkDefault
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 869: // keyword -> tkWhere
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 870: // keyword -> tkXor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 871: // keyword -> tkAnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 872: // keyword -> tkDiv
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 873: // keyword -> tkMod
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 874: // keyword -> tkShl
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 875: // keyword -> tkShr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 876: // keyword -> tkNot
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 877: // keyword -> tkAs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 878: // keyword -> tkIn
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 879: // keyword -> tkIs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 880: // keyword -> tkArray
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 881: // keyword -> tkSequence
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 882: // keyword -> tkBegin
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 883: // keyword -> tkCase
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 884: // keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 885: // keyword -> tkConst
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 886: // keyword -> tkConstructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 887: // keyword -> tkDestructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 888: // keyword -> tkDownto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 889: // keyword -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 890: // keyword -> tkElse
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 891: // keyword -> tkEnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 892: // keyword -> tkExcept
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 893: // keyword -> tkFile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 894: // keyword -> tkAuto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 895: // keyword -> tkFinalization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 896: // keyword -> tkFinally
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 897: // keyword -> tkFor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 898: // keyword -> tkForeach
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 899: // keyword -> tkFunction
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 900: // keyword -> tkIf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 901: // keyword -> tkImplementation
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 902: // keyword -> tkInherited
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 903: // keyword -> tkInitialization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 904: // keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 905: // keyword -> tkProcedure
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 906: // keyword -> tkProperty
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 907: // keyword -> tkRaise
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 908: // keyword -> tkRecord
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 909: // keyword -> tkRepeat
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 910: // keyword -> tkSet
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 911: // keyword -> tkTry
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 912: // keyword -> tkType
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 913: // keyword -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 914: // keyword -> tkThen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 915: // keyword -> tkTo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 916: // keyword -> tkUntil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 917: // keyword -> tkUses
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 918: // keyword -> tkVar
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 919: // keyword -> tkWhile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 920: // keyword -> tkWith
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 921: // keyword -> tkNil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 922: // keyword -> tkGoto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 923: // keyword -> tkOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 924: // keyword -> tkLabel
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 925: // keyword -> tkProgram
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 926: // keyword -> tkUnit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 927: // keyword -> tkLibrary
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 928: // keyword -> tkNamespace
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 929: // keyword -> tkExternal
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 930: // keyword -> tkParams
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 931: // keyword -> tkEvent
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 932: // keyword -> tkYield
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 933: // keyword -> tkMatch
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 934: // keyword -> tkWhen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 935: // keyword -> tkPartial
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 936: // keyword -> tkAbstract
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 937: // keyword -> tkLock
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 938: // keyword -> tkImplicit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 939: // keyword -> tkExplicit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 940: // keyword -> tkOn
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 941: // keyword -> tkVirtual
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 942: // keyword -> tkOverride
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 943: // keyword -> tkLoop
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 944: // keyword -> tkExtensionMethod
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 945: // keyword -> tkOverload
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 946: // keyword -> tkReintroduce
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 947: // keyword -> tkForward
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 948: // reserved_keyword -> tkOperator
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 949: // overload_operator -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 950: // overload_operator -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 951: // overload_operator -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 952: // overload_operator -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 953: // overload_operator -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 954: // overload_operator -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 955: // overload_operator -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 956: // overload_operator -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 957: // overload_operator -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 958: // overload_operator -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 959: // overload_operator -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 960: // overload_operator -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 961: // overload_operator -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 962: // overload_operator -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 963: // overload_operator -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 964: // overload_operator -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 965: // overload_operator -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 966: // overload_operator -> tkNot
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 967: // overload_operator -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 968: // overload_operator -> tkImplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 969: // overload_operator -> tkExplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 970: // overload_operator -> assign_operator
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 971: // overload_operator -> tkStarStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 972: // assign_operator -> tkAssign
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 973: // assign_operator -> tkPlusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 974: // assign_operator -> tkMinusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 975: // assign_operator -> tkMultEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 976: // assign_operator -> tkDivEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 977: // lambda_unpacked_params -> tkBackSlashRoundOpen, 
                //                           lambda_list_of_unpacked_params_or_id, tkComma, 
                //                           lambda_unpacked_params_or_id, tkRoundClose
{
			// �?ез�?л�?�?а�? надо п�?исвои�?�? каком�? �?о са�?а�?ном�? пол�? в function_lambda_definition
			(ValueStack[ValueStack.Depth-4].ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-2].ob as ident_or_list);
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-4].ob as unpacked_list_of_ident_or_list;
		}
        break;
      case 978: // lambda_unpacked_params_or_id -> lambda_unpacked_params
{
			CurrentSemanticValue.ob = new ident_or_list(ValueStack[ValueStack.Depth-1].ex as unpacked_list_of_ident_or_list);
		}
        break;
      case 979: // lambda_unpacked_params_or_id -> identifier
{
			CurrentSemanticValue.ob = new ident_or_list(ValueStack[ValueStack.Depth-1].id as ident);
		}
        break;
      case 980: // lambda_list_of_unpacked_params_or_id -> lambda_unpacked_params_or_id
{
			CurrentSemanticValue.ob = new unpacked_list_of_ident_or_list();
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-1].ob as ident_or_list);
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).source_context = LocationStack[LocationStack.Depth-1];
		}
        break;
      case 981: // lambda_list_of_unpacked_params_or_id -> lambda_list_of_unpacked_params_or_id, 
                //                                         tkComma, lambda_unpacked_params_or_id
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-1].ob as ident_or_list);
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 982: // expr_l1_or_unpacked -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 983: // expr_l1_or_unpacked -> lambda_unpacked_params
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 984: // expr_l1_or_unpacked_list -> expr_l1_or_unpacked
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 985: // expr_l1_or_unpacked_list -> expr_l1_or_unpacked_list, tkComma, 
                //                             expr_l1_or_unpacked
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 986: // func_decl_lambda -> identifier, tkArrow, lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-3].id, LocationStack[LocationStack.Depth-3]); 
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
			//var sl = $3 as statement_list;
			//if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName($3, "Result") != null) // если э�?о б�?ло в�?�?ажение или ес�?�? пе�?еменная Result, �?о ав�?ов�?вод �?ипа 
			    CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
			//else 
			//$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, $3 as statement_list, @$);  
		}
        break;
      case 987: // func_decl_lambda -> tkRoundOpen, tkRoundClose, lambda_type_ref_noproctype, 
                //                     tkArrow, lambda_function_body
{
		    // �?дес�? надо анализи�?ова�?�? по �?ел�? и либо ос�?авля�?�? lambda_inferred_type, либо дела�?�? его null!
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, sl, CurrentLocationSpan);	
		}
        break;
      case 988: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkRoundClose, 
                //                     lambda_type_ref_noproctype, tkArrow, lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-7].id, LocationStack[LocationStack.Depth-7]); 
            var loc = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]);
			var formalPars = new formal_parameters(new typed_parameters(idList, ValueStack[ValueStack.Depth-5].td, parametr_kind.none, null, loc), loc);
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, sl, CurrentLocationSpan);	
		}
        break;
      case 989: // func_decl_lambda -> tkRoundOpen, identifier, tkSemiColon, full_lambda_fp_list, 
                //                     tkRoundClose, lambda_type_ref_noproctype, tkArrow, 
                //                     lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-7].id, LocationStack[LocationStack.Depth-7]);
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new lambda_any_type_node_syntax(), null), parametr_kind.none, null, LocationStack[LocationStack.Depth-7]), LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]));
			for (int i = 0; i < (ValueStack[ValueStack.Depth-5].stn as formal_parameters).Count; i++)
				formalPars.Add((ValueStack[ValueStack.Depth-5].stn as formal_parameters).params_list[i]);
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, sl, CurrentLocationSpan);	
		}
        break;
      case 990: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkSemiColon, 
                //                     full_lambda_fp_list, tkRoundClose, 
                //                     lambda_type_ref_noproctype, tkArrow, lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-9].id, LocationStack[LocationStack.Depth-9]);
            var loc = LexLocation.MergeAll(LocationStack[LocationStack.Depth-9],LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7]);
			var formalPars = new formal_parameters(new typed_parameters(idList, ValueStack[ValueStack.Depth-7].td, parametr_kind.none, null, loc), LexLocation.MergeAll(LocationStack[LocationStack.Depth-9],LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]));
			for (int i = 0; i < (ValueStack[ValueStack.Depth-5].stn as formal_parameters).Count; i++)
				formalPars.Add((ValueStack[ValueStack.Depth-5].stn as formal_parameters).params_list[i]);
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, sl, CurrentLocationSpan);
		}
        break;
      case 991: // func_decl_lambda -> tkRoundOpen, expr_l1_or_unpacked, tkComma, 
                //                     expr_l1_or_unpacked_list, lambda_type_ref, 
                //                     optional_full_lambda_fp_list, tkRoundClose, rem_lambda
{ 
			var pair = ValueStack[ValueStack.Depth-1].ob as pair_type_stlist;
			
			if (ValueStack[ValueStack.Depth-4].td is lambda_inferred_type)
			{
				// добавим с�?да \(x,y)
				// �?�?ой�?ис�? по всем expr_list1. �?сли �?о�?я б�? одна - �?ипа ident_or_list �?о пой�?и по э�?ой ве�?ке и в�?й�?и
				// �?беди�?�?ся, �?�?о $6 = null
				// с�?о�?ми�?ова�?�? List<expression> для unpacked_params и п�?исвои�?�?
				var has_unpacked = false;
				if (ValueStack[ValueStack.Depth-7].ex is unpacked_list_of_ident_or_list)
					has_unpacked = true;
				if (!has_unpacked)
					foreach (var x in (ValueStack[ValueStack.Depth-5].stn as expression_list).expressions)
					{
						if (x is unpacked_list_of_ident_or_list)
						{
							has_unpacked = true;
							break;
						}
					}
				if (has_unpacked) // �?�?�? новая ве�?ка
				{
					if (ValueStack[ValueStack.Depth-3].stn != null)
					{
						parsertools.AddErrorFromResource("SEMICOLON_IN_PARAMS",LocationStack[LocationStack.Depth-3]);
					}
				
					var lst_ex = new List<expression>();
					lst_ex.Add(ValueStack[ValueStack.Depth-7].ex as expression);
					foreach (var x in (ValueStack[ValueStack.Depth-5].stn as expression_list).expressions)
						lst_ex.Add(x);
					
					function_lambda_definition fld = null; //= new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, 
    					//new lambda_inferred_type(new lambda_any_type_node_syntax(), @2), pair.exprs, @$);

					var sl1 = pair.exprs;
			    	if (sl1.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl1, "result") != null) // �?о надо в�?води�?�?
						fld = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, pair.tn, pair.exprs, CurrentLocationSpan);
					else fld = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, pair.exprs, CurrentLocationSpan);	

					fld.unpacked_params = lst_ex;
					CurrentSemanticValue.ex = fld;					
					return;
				}
				
				var formal_pars = new formal_parameters();
				var idd = ValueStack[ValueStack.Depth-7].ex as ident;
				if (idd==null)
					parsertools.AddErrorFromResource("ONE_TKIDENTIFIER",LocationStack[LocationStack.Depth-7]);
				var lambda_inf_type = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
				var new_typed_pars = new typed_parameters(new ident_list(idd, idd.source_context), lambda_inf_type, parametr_kind.none, null, idd.source_context);
				formal_pars.Add(new_typed_pars);
				foreach (var id in (ValueStack[ValueStack.Depth-5].stn as expression_list).expressions)
				{
					var idd1 = id as ident;
					if (idd1==null)
						parsertools.AddErrorFromResource("ONE_TKIDENTIFIER",id.source_context);
					
					lambda_inf_type = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
					new_typed_pars = new typed_parameters(new ident_list(idd1, idd1.source_context), lambda_inf_type, parametr_kind.none, null, idd1.source_context);
					formal_pars.Add(new_typed_pars);
				}
				
				if (ValueStack[ValueStack.Depth-3].stn != null)
					for (int i = 0; i < (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Count; i++)
						formal_pars.Add((ValueStack[ValueStack.Depth-3].stn as formal_parameters).params_list[i]);		
					
				formal_pars.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]);
			    
			    var sl = pair.exprs;
			    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
					CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formal_pars, pair.tn, pair.exprs, CurrentLocationSpan);
				else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formal_pars, null, pair.exprs, CurrentLocationSpan);	
			}
			else
			{			
				var loc = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]);
				var idd = ValueStack[ValueStack.Depth-7].ex as ident;
				if (idd==null)
					parsertools.AddErrorFromResource("ONE_TKIDENTIFIER",LocationStack[LocationStack.Depth-7]);
				
				var idList = new ident_list(idd, loc);
				
				var iddlist = (ValueStack[ValueStack.Depth-5].stn as expression_list).expressions;
				
				for (int j = 0; j < iddlist.Count; j++)
				{
					var idd2 = iddlist[j] as ident;
					if (idd2==null)
						parsertools.AddErrorFromResource("ONE_TKIDENTIFIER",idd2.source_context);
					idList.Add(idd2);
				}	
				var parsType = ValueStack[ValueStack.Depth-4].td;
				var formalPars = new formal_parameters(new typed_parameters(idList, parsType, parametr_kind.none, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4])), LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]));
				
				if (ValueStack[ValueStack.Depth-3].stn != null)
					for (int i = 0; i < (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Count; i++)
						formalPars.Add((ValueStack[ValueStack.Depth-3].stn as formal_parameters).params_list[i]);

				var sl = pair.exprs;
			    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
					CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, pair.tn, pair.exprs, CurrentLocationSpan);
				else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, pair.exprs, CurrentLocationSpan);
			}
		}
        break;
      case 992: // func_decl_lambda -> lambda_unpacked_params, rem_lambda
{
    		var pair = ValueStack[ValueStack.Depth-1].ob as pair_type_stlist;
    		// пока �?о�?мал�?н�?е па�?аме�?�?�? - null. Раск�?оем и�? са�?а�?н�?м визи�?о�?ом
    		CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, 
    			new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-2]), pair.exprs, CurrentLocationSpan);
    		// unpacked_params - э�?о для одного па�?аме�?�?а. �?ля нескол�?ки�? - надо д�?�?г�?�? с�?�?�?к�?�?�?�?. �?озможно, список списков
    		var lst_ex = new List<expression>();
    		lst_ex.Add(ValueStack[ValueStack.Depth-2].ex as unpacked_list_of_ident_or_list);
    		(CurrentSemanticValue.ex as function_lambda_definition).unpacked_params = lst_ex;  
    	}
        break;
      case 993: // func_decl_lambda -> expl_func_decl_lambda
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 994: // optional_full_lambda_fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 995: // optional_full_lambda_fp_list -> tkSemiColon, full_lambda_fp_list
{
		CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
	}
        break;
      case 996: // rem_lambda -> lambda_type_ref_noproctype, tkArrow, lambda_function_body
{ 
		    CurrentSemanticValue.ob = new pair_type_stlist(ValueStack[ValueStack.Depth-3].td,ValueStack[ValueStack.Depth-1].stn as statement_list);
		}
        break;
      case 997: // expl_func_decl_lambda -> tkFunction, lambda_type_ref_noproctype, tkArrow, 
                //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 998: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, tkRoundClose, 
                //                          lambda_type_ref_noproctype, tkArrow, 
                //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 999: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, full_lambda_fp_list, 
                //                          tkRoundClose, lambda_type_ref_noproctype, tkArrow, 
                //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 1000: // expl_func_decl_lambda -> tkProcedure, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1001: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, tkRoundClose, tkArrow, 
                 //                          lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1002: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, full_lambda_fp_list, 
                 //                          tkRoundClose, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-4].stn as formal_parameters, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1003: // full_lambda_fp_list -> lambda_simple_fp_sect
{
			var typed_pars = ValueStack[ValueStack.Depth-1].stn as typed_parameters;
			if (typed_pars.vars_type is lambda_inferred_type)
			{
				CurrentSemanticValue.stn = new formal_parameters();
				foreach (var id in typed_pars.idents.idents)
				{
					var lambda_inf_type = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
					var new_typed_pars = new typed_parameters(new ident_list(id, id.source_context), lambda_inf_type, parametr_kind.none, null, id.source_context);
					(CurrentSemanticValue.stn as formal_parameters).Add(new_typed_pars);
				}
				CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
			}
			else
			{
				CurrentSemanticValue.stn = new formal_parameters(typed_pars, CurrentLocationSpan);
			}
		}
        break;
      case 1004: // full_lambda_fp_list -> full_lambda_fp_list, tkSemiColon, lambda_simple_fp_sect
{
			CurrentSemanticValue.stn =(ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
		}
        break;
      case 1005: // lambda_simple_fp_sect -> ident_list, lambda_type_ref
{
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-2].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan);
		}
        break;
      case 1006: // lambda_type_ref -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
		}
        break;
      case 1007: // lambda_type_ref -> tkColon, fptype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 1008: // lambda_type_ref_noproctype -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
		}
        break;
      case 1009: // lambda_type_ref_noproctype -> tkColon, fptype_noproctype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 1010: // common_lambda_body -> compound_stmt
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 1011: // common_lambda_body -> if_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1012: // common_lambda_body -> while_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1013: // common_lambda_body -> repeat_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1014: // common_lambda_body -> for_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1015: // common_lambda_body -> foreach_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1016: // common_lambda_body -> loop_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1017: // common_lambda_body -> case_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1018: // common_lambda_body -> try_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1019: // common_lambda_body -> lock_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1020: // common_lambda_body -> raise_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1021: // common_lambda_body -> yield_stmt
{
			parsertools.AddErrorFromResource("YIELD_STATEMENT_CANNOT_BE_USED_IN_LAMBDA_BODY", CurrentLocationSpan);
		}
        break;
      case 1022: // common_lambda_body -> tkRoundOpen, assignment, tkRoundClose
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-2]);
		}
        break;
      case 1023: // lambda_function_body -> expr_l1_for_lambda
{
		    var id = SyntaxVisitors.HasNameVisitor.HasName(ValueStack[ValueStack.Depth-1].ex, "Result"); 
            if (id != null)
            {
                 parsertools.AddErrorFromResource("RESULT_IDENT_NOT_EXPECTED_IN_THIS_CONTEXT", id.source_context);
            }
			var sl = new statement_list(new assign("result",ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan),CurrentLocationSpan); // надо поме�?а�?�? е�?�? и assign как ав�?осгене�?и�?ованн�?й для лямбд�? - �?�?об�? зап�?е�?и�?�? явн�?й Result
			sl.expr_lambda_body = true;
			CurrentSemanticValue.stn = sl;
		}
        break;
      case 1024: // lambda_function_body -> common_lambda_body
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 1025: // lambda_procedure_body -> proc_call
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1026: // lambda_procedure_body -> assignment
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1027: // lambda_procedure_body -> common_lambda_body
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
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
