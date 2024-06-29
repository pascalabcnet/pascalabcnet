// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.6
// Machine:  DESKTOP-G8V08V4
// DateTime: 17.06.2024 16:43:46
// UserName: ?????????
// Input file <ABCPascal.y>

// options: no-lines gplex

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
  // Verbatim content from ABCPascal.y
// Э�?и об�?явления добавля�?�?ся в класс GPPGParser, п�?едс�?авля�?�?ий собой па�?се�?, гене�?и�?�?ем�?й сис�?емой gppg
    public syntax_tree_node root; // �?о�?невой �?зел син�?акси�?еского де�?ева 

    public int maxErrors = 10;
    public PascalParserTools parserTools;
	public ParserLambdaHelper lambdaHelper = new ParserLambdaHelper();
	
    public GPPGParser(AbstractScanner<PascalABCCompiler.ParserTools.Union, LexLocation> scanner) : base(scanner) { }
  // End verbatim content from ABCPascal.y

#pragma warning disable 649
  private static Dictionary<int, string> aliasses;
#pragma warning restore 649
  private static Rule[] rules = new Rule[1028];
  private static State[] states = new State[1703];
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
    states[0] = new State(new int[]{61,1601,105,1668,106,1669,109,1670,88,1675,90,1680,89,1687,75,1692,77,1699,3,-27,52,-27,91,-27,59,-27,29,-27,66,-27,50,-27,53,-27,62,-27,11,-27,44,-27,37,-27,28,-27,26,-27,19,-27,30,-27,31,-27},new int[]{-1,1,-234,3,-235,4,-306,1613,-308,1614,-2,1663,-175,1674});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{3,1597,52,-14,91,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-185,5,-186,1595,-184,1600});
    states[5] = new State(-41,new int[]{-302,6});
    states[6] = new State(new int[]{52,1582,59,-68,29,-68,66,-68,50,-68,53,-68,62,-68,11,-68,44,-68,37,-68,28,-68,26,-68,19,-68,30,-68,31,-68,91,-68},new int[]{-18,7,-304,14,-37,15,-41,1513,-42,1514});
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
    states[17] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,755,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494},new int[]{-252,18,-261,753,-260,22,-4,23,-113,24,-132,373,-111,386,-147,754,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888,-143,1043});
    states[18] = new State(new int[]{92,19,10,20});
    states[19] = new State(-530);
    states[20] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,755,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494},new int[]{-261,21,-260,22,-4,23,-113,24,-132,373,-111,386,-147,754,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888,-143,1043});
    states[21] = new State(-532);
    states[22] = new State(-492);
    states[23] = new State(-495);
    states[24] = new State(new int[]{110,416,111,417,112,418,113,419,114,420,92,-528,10,-528,98,-528,101,-528,33,-528,104,-528,2,-528,9,-528,100,-528,12,-528,99,-528,32,-528,85,-528,84,-528,83,-528,82,-528,81,-528,86,-528},new int[]{-194,25});
    states[25] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,661,8,662,21,270,22,275,76,541,40,608,5,617,18,686,37,695,44,699},new int[]{-88,26,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-322,836,-100,671,-323,694});
    states[26] = new State(-522);
    states[27] = new State(-598);
    states[28] = new State(-605);
    states[29] = new State(new int[]{16,30,92,-607,10,-607,98,-607,101,-607,33,-607,104,-607,2,-607,9,-607,100,-607,12,-607,99,-607,32,-607,85,-607,84,-607,83,-607,82,-607,81,-607,86,-607,6,-607,76,-607,5,-607,51,-607,58,-607,141,-607,143,-607,80,-607,78,-607,160,-607,87,-607,45,-607,42,-607,8,-607,21,-607,22,-607,144,-607,147,-607,145,-607,146,-607,155,-607,158,-607,157,-607,156,-607,57,-607,91,-607,40,-607,25,-607,97,-607,54,-607,35,-607,55,-607,102,-607,47,-607,36,-607,53,-607,60,-607,74,-607,72,-607,38,-607,70,-607,71,-607,13,-610});
    states[30] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-98,31,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598});
    states[31] = new State(new int[]{120,312,125,313,123,314,121,315,124,316,122,317,137,318,135,319,16,-620,92,-620,10,-620,98,-620,101,-620,33,-620,104,-620,2,-620,9,-620,100,-620,12,-620,99,-620,32,-620,85,-620,84,-620,83,-620,82,-620,81,-620,86,-620,13,-620,6,-620,76,-620,5,-620,51,-620,58,-620,141,-620,143,-620,80,-620,78,-620,160,-620,87,-620,45,-620,42,-620,8,-620,21,-620,22,-620,144,-620,147,-620,145,-620,146,-620,155,-620,158,-620,157,-620,156,-620,57,-620,91,-620,40,-620,25,-620,97,-620,54,-620,35,-620,55,-620,102,-620,47,-620,36,-620,53,-620,60,-620,74,-620,72,-620,38,-620,70,-620,71,-620,116,-620,115,-620,128,-620,129,-620,126,-620,138,-620,136,-620,118,-620,117,-620,131,-620,132,-620,133,-620,134,-620,130,-620},new int[]{-196,32});
    states[32] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-105,33,-242,1512,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,621,-267,598});
    states[33] = new State(new int[]{6,34,120,-645,125,-645,123,-645,121,-645,124,-645,122,-645,137,-645,135,-645,16,-645,92,-645,10,-645,98,-645,101,-645,33,-645,104,-645,2,-645,9,-645,100,-645,12,-645,99,-645,32,-645,85,-645,84,-645,83,-645,82,-645,81,-645,86,-645,13,-645,76,-645,5,-645,51,-645,58,-645,141,-645,143,-645,80,-645,78,-645,160,-645,87,-645,45,-645,42,-645,8,-645,21,-645,22,-645,144,-645,147,-645,145,-645,146,-645,155,-645,158,-645,157,-645,156,-645,57,-645,91,-645,40,-645,25,-645,97,-645,54,-645,35,-645,55,-645,102,-645,47,-645,36,-645,53,-645,60,-645,74,-645,72,-645,38,-645,70,-645,71,-645,116,-645,115,-645,128,-645,129,-645,126,-645,138,-645,136,-645,118,-645,117,-645,131,-645,132,-645,133,-645,134,-645,130,-645});
    states[34] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-83,35,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,621,-267,598});
    states[35] = new State(new int[]{116,327,115,328,128,329,129,330,126,331,6,-724,5,-724,120,-724,125,-724,123,-724,121,-724,124,-724,122,-724,137,-724,135,-724,16,-724,92,-724,10,-724,98,-724,101,-724,33,-724,104,-724,2,-724,9,-724,100,-724,12,-724,99,-724,32,-724,85,-724,84,-724,83,-724,82,-724,81,-724,86,-724,13,-724,76,-724,51,-724,58,-724,141,-724,143,-724,80,-724,78,-724,160,-724,87,-724,45,-724,42,-724,8,-724,21,-724,22,-724,144,-724,147,-724,145,-724,146,-724,155,-724,158,-724,157,-724,156,-724,57,-724,91,-724,40,-724,25,-724,97,-724,54,-724,35,-724,55,-724,102,-724,47,-724,36,-724,53,-724,60,-724,74,-724,72,-724,38,-724,70,-724,71,-724,138,-724,136,-724,118,-724,117,-724,131,-724,132,-724,133,-724,134,-724,130,-724},new int[]{-197,36});
    states[36] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-82,37,-242,1511,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,621,-267,598});
    states[37] = new State(new int[]{138,333,136,1474,118,1477,117,1478,131,1479,132,1480,133,1481,134,1482,130,1483,116,-726,115,-726,128,-726,129,-726,126,-726,6,-726,5,-726,120,-726,125,-726,123,-726,121,-726,124,-726,122,-726,137,-726,135,-726,16,-726,92,-726,10,-726,98,-726,101,-726,33,-726,104,-726,2,-726,9,-726,100,-726,12,-726,99,-726,32,-726,85,-726,84,-726,83,-726,82,-726,81,-726,86,-726,13,-726,76,-726,51,-726,58,-726,141,-726,143,-726,80,-726,78,-726,160,-726,87,-726,45,-726,42,-726,8,-726,21,-726,22,-726,144,-726,147,-726,145,-726,146,-726,155,-726,158,-726,157,-726,156,-726,57,-726,91,-726,40,-726,25,-726,97,-726,54,-726,35,-726,55,-726,102,-726,47,-726,36,-726,53,-726,60,-726,74,-726,72,-726,38,-726,70,-726,71,-726},new int[]{-198,38});
    states[38] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-96,39,-268,40,-242,41,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-97,551});
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
    states[158] = new State(-190);
    states[159] = new State(-191);
    states[160] = new State(-192);
    states[161] = new State(-193);
    states[162] = new State(-764);
    states[163] = new State(new int[]{8,164});
    states[164] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,677},new int[]{-284,165,-283,167,-180,168,-147,207,-151,48,-152,51,-273,1508,-272,1509,-93,181,-106,290,-107,291,-16,457,-199,458,-165,461,-167,150,-166,154,-300,1510});
    states[165] = new State(new int[]{9,166});
    states[166] = new State(-758);
    states[167] = new State(-631);
    states[168] = new State(new int[]{7,169,4,172,123,174,9,-628,8,-255,118,-255,117,-255,131,-255,132,-255,133,-255,134,-255,130,-255,6,-255,116,-255,115,-255,128,-255,129,-255,13,-255},new int[]{-298,171});
    states[169] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148},new int[]{-138,170,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[170] = new State(-261);
    states[171] = new State(new int[]{9,-629,13,-234});
    states[172] = new State(new int[]{123,174},new int[]{-298,173});
    states[173] = new State(-630);
    states[174] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-296,175,-279,289,-272,179,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-281,1455,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,1456,-224,581,-223,582,-300,1457});
    states[175] = new State(new int[]{121,176,100,177});
    states[176] = new State(-235);
    states[177] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-279,178,-272,179,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-281,1455,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,1456,-224,581,-223,582,-300,1457});
    states[178] = new State(-239);
    states[179] = new State(new int[]{13,180,121,-243,100,-243,120,-243,9,-243,8,-243,138,-243,136,-243,118,-243,117,-243,131,-243,132,-243,133,-243,134,-243,130,-243,116,-243,115,-243,128,-243,129,-243,126,-243,6,-243,5,-243,125,-243,123,-243,124,-243,122,-243,137,-243,135,-243,16,-243,92,-243,10,-243,98,-243,101,-243,33,-243,104,-243,2,-243,12,-243,99,-243,32,-243,85,-243,84,-243,83,-243,82,-243,81,-243,86,-243,76,-243,51,-243,58,-243,141,-243,143,-243,80,-243,78,-243,160,-243,87,-243,45,-243,42,-243,21,-243,22,-243,144,-243,147,-243,145,-243,146,-243,155,-243,158,-243,157,-243,156,-243,57,-243,91,-243,40,-243,25,-243,97,-243,54,-243,35,-243,55,-243,102,-243,47,-243,36,-243,53,-243,60,-243,74,-243,72,-243,38,-243,70,-243,71,-243,127,-243,110,-243});
    states[180] = new State(-244);
    states[181] = new State(new int[]{6,1506,116,234,115,235,128,236,129,237,13,-248,121,-248,100,-248,120,-248,9,-248,8,-248,138,-248,136,-248,118,-248,117,-248,131,-248,132,-248,133,-248,134,-248,130,-248,126,-248,5,-248,125,-248,123,-248,124,-248,122,-248,137,-248,135,-248,16,-248,92,-248,10,-248,98,-248,101,-248,33,-248,104,-248,2,-248,12,-248,99,-248,32,-248,85,-248,84,-248,83,-248,82,-248,81,-248,86,-248,76,-248,51,-248,58,-248,141,-248,143,-248,80,-248,78,-248,160,-248,87,-248,45,-248,42,-248,21,-248,22,-248,144,-248,147,-248,145,-248,146,-248,155,-248,158,-248,157,-248,156,-248,57,-248,91,-248,40,-248,25,-248,97,-248,54,-248,35,-248,55,-248,102,-248,47,-248,36,-248,53,-248,60,-248,74,-248,72,-248,38,-248,70,-248,71,-248,127,-248,110,-248},new int[]{-193,182});
    states[182] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156},new int[]{-106,183,-107,291,-180,460,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154});
    states[183] = new State(new int[]{118,241,117,242,131,243,132,244,133,245,134,246,130,247,6,-252,116,-252,115,-252,128,-252,129,-252,13,-252,121,-252,100,-252,120,-252,9,-252,8,-252,138,-252,136,-252,126,-252,5,-252,125,-252,123,-252,124,-252,122,-252,137,-252,135,-252,16,-252,92,-252,10,-252,98,-252,101,-252,33,-252,104,-252,2,-252,12,-252,99,-252,32,-252,85,-252,84,-252,83,-252,82,-252,81,-252,86,-252,76,-252,51,-252,58,-252,141,-252,143,-252,80,-252,78,-252,160,-252,87,-252,45,-252,42,-252,21,-252,22,-252,144,-252,147,-252,145,-252,146,-252,155,-252,158,-252,157,-252,156,-252,57,-252,91,-252,40,-252,25,-252,97,-252,54,-252,35,-252,55,-252,102,-252,47,-252,36,-252,53,-252,60,-252,74,-252,72,-252,38,-252,70,-252,71,-252,127,-252,110,-252},new int[]{-195,184});
    states[184] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156},new int[]{-107,185,-180,460,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154});
    states[185] = new State(new int[]{8,186,118,-254,117,-254,131,-254,132,-254,133,-254,134,-254,130,-254,6,-254,116,-254,115,-254,128,-254,129,-254,13,-254,121,-254,100,-254,120,-254,9,-254,138,-254,136,-254,126,-254,5,-254,125,-254,123,-254,124,-254,122,-254,137,-254,135,-254,16,-254,92,-254,10,-254,98,-254,101,-254,33,-254,104,-254,2,-254,12,-254,99,-254,32,-254,85,-254,84,-254,83,-254,82,-254,81,-254,86,-254,76,-254,51,-254,58,-254,141,-254,143,-254,80,-254,78,-254,160,-254,87,-254,45,-254,42,-254,21,-254,22,-254,144,-254,147,-254,145,-254,146,-254,155,-254,158,-254,157,-254,156,-254,57,-254,91,-254,40,-254,25,-254,97,-254,54,-254,35,-254,55,-254,102,-254,47,-254,36,-254,53,-254,60,-254,74,-254,72,-254,38,-254,70,-254,71,-254,127,-254,110,-254});
    states[186] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163,9,-185},new int[]{-75,187,-73,189,-94,1505,-90,192,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[187] = new State(new int[]{9,188});
    states[188] = new State(-259);
    states[189] = new State(new int[]{100,190,9,-184,12,-184});
    states[190] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-94,191,-90,192,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[191] = new State(-187);
    states[192] = new State(new int[]{13,193,16,197,6,1499,100,-188,9,-188,12,-188,5,-188});
    states[193] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-90,194,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[194] = new State(new int[]{5,195,13,193,16,197});
    states[195] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-90,196,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[196] = new State(new int[]{13,193,16,197,6,-124,100,-124,9,-124,12,-124,5,-124,92,-124,10,-124,98,-124,101,-124,33,-124,104,-124,2,-124,99,-124,32,-124,85,-124,84,-124,83,-124,82,-124,81,-124,86,-124});
    states[197] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-91,198,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851});
    states[198] = new State(new int[]{120,226,125,227,123,228,121,229,124,230,122,231,137,232,13,-123,16,-123,6,-123,100,-123,9,-123,12,-123,5,-123,92,-123,10,-123,98,-123,101,-123,33,-123,104,-123,2,-123,99,-123,32,-123,85,-123,84,-123,83,-123,82,-123,81,-123,86,-123},new int[]{-192,199});
    states[199] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-81,200,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851});
    states[200] = new State(new int[]{116,234,115,235,128,236,129,237,120,-120,125,-120,123,-120,121,-120,124,-120,122,-120,137,-120,13,-120,16,-120,6,-120,100,-120,9,-120,12,-120,5,-120,92,-120,10,-120,98,-120,101,-120,33,-120,104,-120,2,-120,99,-120,32,-120,85,-120,84,-120,83,-120,82,-120,81,-120,86,-120},new int[]{-193,201});
    states[201] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-13,202,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851});
    states[202] = new State(new int[]{136,239,138,240,118,241,117,242,131,243,132,244,133,245,134,246,130,247,116,-133,115,-133,128,-133,129,-133,120,-133,125,-133,123,-133,121,-133,124,-133,122,-133,137,-133,13,-133,16,-133,6,-133,100,-133,9,-133,12,-133,5,-133,92,-133,10,-133,98,-133,101,-133,33,-133,104,-133,2,-133,99,-133,32,-133,85,-133,84,-133,83,-133,82,-133,81,-133,86,-133},new int[]{-201,203,-195,208});
    states[203] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-283,204,-180,205,-147,207,-151,48,-152,51});
    states[204] = new State(-138);
    states[205] = new State(new int[]{7,169,4,172,123,174,136,-628,138,-628,118,-628,117,-628,131,-628,132,-628,133,-628,134,-628,130,-628,116,-628,115,-628,128,-628,129,-628,120,-628,125,-628,121,-628,124,-628,122,-628,137,-628,13,-628,16,-628,6,-628,100,-628,9,-628,12,-628,5,-628,92,-628,10,-628,98,-628,101,-628,33,-628,104,-628,2,-628,99,-628,32,-628,85,-628,84,-628,83,-628,82,-628,81,-628,86,-628,11,-628,8,-628,126,-628,135,-628,76,-628,51,-628,58,-628,141,-628,143,-628,80,-628,78,-628,160,-628,87,-628,45,-628,42,-628,21,-628,22,-628,144,-628,147,-628,145,-628,146,-628,155,-628,158,-628,157,-628,156,-628,57,-628,91,-628,40,-628,25,-628,97,-628,54,-628,35,-628,55,-628,102,-628,47,-628,36,-628,53,-628,60,-628,74,-628,72,-628,38,-628,70,-628,71,-628},new int[]{-298,206});
    states[206] = new State(-629);
    states[207] = new State(-260);
    states[208] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-10,209,-269,210,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-11,851});
    states[209] = new State(-145);
    states[210] = new State(-146);
    states[211] = new State(new int[]{4,213,11,215,7,829,142,831,8,832,136,-156,138,-156,118,-156,117,-156,131,-156,132,-156,133,-156,134,-156,130,-156,116,-156,115,-156,128,-156,129,-156,120,-156,125,-156,123,-156,121,-156,124,-156,122,-156,137,-156,13,-156,16,-156,6,-156,100,-156,9,-156,12,-156,5,-156,92,-156,10,-156,98,-156,101,-156,33,-156,104,-156,2,-156,99,-156,32,-156,85,-156,84,-156,83,-156,82,-156,81,-156,86,-156,119,-154},new int[]{-12,212});
    states[212] = new State(-175);
    states[213] = new State(new int[]{123,174},new int[]{-298,214});
    states[214] = new State(-176);
    states[215] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163,5,1501,12,-185},new int[]{-121,216,-75,218,-90,220,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857,-73,189,-94,1505});
    states[216] = new State(new int[]{12,217});
    states[217] = new State(-177);
    states[218] = new State(new int[]{12,219});
    states[219] = new State(-181);
    states[220] = new State(new int[]{5,221,13,193,16,197,6,1499,100,-188,12,-188});
    states[221] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163,5,-706,12,-706},new int[]{-122,222,-90,1498,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[222] = new State(new int[]{5,223,12,-711});
    states[223] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-90,224,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[224] = new State(new int[]{13,193,16,197,12,-713});
    states[225] = new State(new int[]{120,226,125,227,123,228,121,229,124,230,122,231,137,232,13,-121,16,-121,6,-121,100,-121,9,-121,12,-121,5,-121,92,-121,10,-121,98,-121,101,-121,33,-121,104,-121,2,-121,99,-121,32,-121,85,-121,84,-121,83,-121,82,-121,81,-121,86,-121},new int[]{-192,199});
    states[226] = new State(-125);
    states[227] = new State(-126);
    states[228] = new State(-127);
    states[229] = new State(-128);
    states[230] = new State(-129);
    states[231] = new State(-130);
    states[232] = new State(-131);
    states[233] = new State(new int[]{116,234,115,235,128,236,129,237,120,-119,125,-119,123,-119,121,-119,124,-119,122,-119,137,-119,13,-119,16,-119,6,-119,100,-119,9,-119,12,-119,5,-119,92,-119,10,-119,98,-119,101,-119,33,-119,104,-119,2,-119,99,-119,32,-119,85,-119,84,-119,83,-119,82,-119,81,-119,86,-119},new int[]{-193,201});
    states[234] = new State(-134);
    states[235] = new State(-135);
    states[236] = new State(-136);
    states[237] = new State(-137);
    states[238] = new State(new int[]{136,239,138,240,118,241,117,242,131,243,132,244,133,245,134,246,130,247,116,-132,115,-132,128,-132,129,-132,120,-132,125,-132,123,-132,121,-132,124,-132,122,-132,137,-132,13,-132,16,-132,6,-132,100,-132,9,-132,12,-132,5,-132,92,-132,10,-132,98,-132,101,-132,33,-132,104,-132,2,-132,99,-132,32,-132,85,-132,84,-132,83,-132,82,-132,81,-132,86,-132},new int[]{-201,203,-195,208});
    states[239] = new State(-733);
    states[240] = new State(-734);
    states[241] = new State(-147);
    states[242] = new State(-148);
    states[243] = new State(-149);
    states[244] = new State(-150);
    states[245] = new State(-151);
    states[246] = new State(-152);
    states[247] = new State(-153);
    states[248] = new State(-142);
    states[249] = new State(-169);
    states[250] = new State(new int[]{26,1487,143,47,85,49,86,50,80,52,78,53,160,54,87,55,8,-857,7,-857,142,-857,4,-857,15,-857,110,-857,111,-857,112,-857,113,-857,114,-857,92,-857,10,-857,11,-857,17,-857,5,-857,98,-857,101,-857,33,-857,104,-857,2,-857,127,-857,138,-857,136,-857,118,-857,117,-857,131,-857,132,-857,133,-857,134,-857,130,-857,116,-857,115,-857,128,-857,129,-857,126,-857,6,-857,120,-857,125,-857,123,-857,121,-857,124,-857,122,-857,137,-857,135,-857,16,-857,9,-857,100,-857,12,-857,99,-857,32,-857,84,-857,83,-857,82,-857,81,-857,13,-857,119,-857,76,-857,51,-857,58,-857,141,-857,45,-857,42,-857,21,-857,22,-857,144,-857,147,-857,145,-857,146,-857,155,-857,158,-857,157,-857,156,-857,57,-857,91,-857,40,-857,25,-857,97,-857,54,-857,35,-857,55,-857,102,-857,47,-857,36,-857,53,-857,60,-857,74,-857,72,-857,38,-857,70,-857,71,-857},new int[]{-283,251,-180,205,-147,207,-151,48,-152,51});
    states[251] = new State(new int[]{11,253,8,656,92,-642,10,-642,98,-642,101,-642,33,-642,104,-642,2,-642,138,-642,136,-642,118,-642,117,-642,131,-642,132,-642,133,-642,134,-642,130,-642,116,-642,115,-642,128,-642,129,-642,126,-642,6,-642,5,-642,120,-642,125,-642,123,-642,121,-642,124,-642,122,-642,137,-642,135,-642,16,-642,9,-642,100,-642,12,-642,99,-642,32,-642,85,-642,84,-642,83,-642,82,-642,81,-642,86,-642,13,-642,76,-642,51,-642,58,-642,141,-642,143,-642,80,-642,78,-642,160,-642,87,-642,45,-642,42,-642,21,-642,22,-642,144,-642,147,-642,145,-642,146,-642,155,-642,158,-642,157,-642,156,-642,57,-642,91,-642,40,-642,25,-642,97,-642,54,-642,35,-642,55,-642,102,-642,47,-642,36,-642,53,-642,60,-642,74,-642,72,-642,38,-642,70,-642,71,-642},new int[]{-70,252});
    states[252] = new State(-635);
    states[253] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,661,8,662,21,270,22,275,76,541,40,608,5,617,18,686,37,695,44,699,12,-810},new int[]{-67,254,-71,659,-88,660,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-322,836,-100,671,-323,694});
    states[254] = new State(new int[]{12,255});
    states[255] = new State(new int[]{8,257,92,-634,10,-634,98,-634,101,-634,33,-634,104,-634,2,-634,138,-634,136,-634,118,-634,117,-634,131,-634,132,-634,133,-634,134,-634,130,-634,116,-634,115,-634,128,-634,129,-634,126,-634,6,-634,5,-634,120,-634,125,-634,123,-634,121,-634,124,-634,122,-634,137,-634,135,-634,16,-634,9,-634,100,-634,12,-634,99,-634,32,-634,85,-634,84,-634,83,-634,82,-634,81,-634,86,-634,13,-634,76,-634,51,-634,58,-634,141,-634,143,-634,80,-634,78,-634,160,-634,87,-634,45,-634,42,-634,21,-634,22,-634,144,-634,147,-634,145,-634,146,-634,155,-634,158,-634,157,-634,156,-634,57,-634,91,-634,40,-634,25,-634,97,-634,54,-634,35,-634,55,-634,102,-634,47,-634,36,-634,53,-634,60,-634,74,-634,72,-634,38,-634,70,-634,71,-634},new int[]{-5,256});
    states[256] = new State(-636);
    states[257] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,1003,135,843,116,368,115,369,63,163,9,-198},new int[]{-66,258,-65,260,-85,1006,-84,263,-90,264,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857,-95,1007,-243,1008});
    states[258] = new State(new int[]{9,259});
    states[259] = new State(-633);
    states[260] = new State(new int[]{100,261,9,-199});
    states[261] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,1003,135,843,116,368,115,369,63,163},new int[]{-85,262,-84,263,-90,264,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857,-95,1007,-243,1008});
    states[262] = new State(-201);
    states[263] = new State(-417);
    states[264] = new State(new int[]{13,193,16,197,100,-194,9,-194,92,-194,10,-194,98,-194,101,-194,33,-194,104,-194,2,-194,12,-194,99,-194,32,-194,85,-194,84,-194,83,-194,82,-194,81,-194,86,-194});
    states[265] = new State(-170);
    states[266] = new State(-171);
    states[267] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,268,-151,48,-152,51});
    states[268] = new State(-172);
    states[269] = new State(-173);
    states[270] = new State(new int[]{8,271});
    states[271] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-283,272,-180,205,-147,207,-151,48,-152,51});
    states[272] = new State(new int[]{9,273});
    states[273] = new State(-621);
    states[274] = new State(-174);
    states[275] = new State(new int[]{8,276});
    states[276] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-283,277,-282,279,-180,281,-147,207,-151,48,-152,51});
    states[277] = new State(new int[]{9,278});
    states[278] = new State(-622);
    states[279] = new State(new int[]{9,280});
    states[280] = new State(-623);
    states[281] = new State(new int[]{7,169,4,282,123,284,125,1485,9,-628},new int[]{-298,206,-299,1486});
    states[282] = new State(new int[]{123,284,125,1485},new int[]{-298,173,-299,283});
    states[283] = new State(-627);
    states[284] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,24,336,48,479,49,569,34,573,73,577,44,583,37,623,121,-242,100,-242},new int[]{-296,175,-297,285,-279,289,-272,179,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-281,1455,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,1456,-224,581,-223,582,-300,1457,-280,1484});
    states[285] = new State(new int[]{121,286,100,287});
    states[286] = new State(-237);
    states[287] = new State(-242,new int[]{-280,288});
    states[288] = new State(-241);
    states[289] = new State(-238);
    states[290] = new State(new int[]{118,241,117,242,131,243,132,244,133,245,134,246,130,247,6,-251,116,-251,115,-251,128,-251,129,-251,13,-251,121,-251,100,-251,120,-251,9,-251,8,-251,138,-251,136,-251,126,-251,5,-251,125,-251,123,-251,124,-251,122,-251,137,-251,135,-251,16,-251,92,-251,10,-251,98,-251,101,-251,33,-251,104,-251,2,-251,12,-251,99,-251,32,-251,85,-251,84,-251,83,-251,82,-251,81,-251,86,-251,76,-251,51,-251,58,-251,141,-251,143,-251,80,-251,78,-251,160,-251,87,-251,45,-251,42,-251,21,-251,22,-251,144,-251,147,-251,145,-251,146,-251,155,-251,158,-251,157,-251,156,-251,57,-251,91,-251,40,-251,25,-251,97,-251,54,-251,35,-251,55,-251,102,-251,47,-251,36,-251,53,-251,60,-251,74,-251,72,-251,38,-251,70,-251,71,-251,127,-251,110,-251},new int[]{-195,184});
    states[291] = new State(new int[]{8,186,118,-253,117,-253,131,-253,132,-253,133,-253,134,-253,130,-253,6,-253,116,-253,115,-253,128,-253,129,-253,13,-253,121,-253,100,-253,120,-253,9,-253,138,-253,136,-253,126,-253,5,-253,125,-253,123,-253,124,-253,122,-253,137,-253,135,-253,16,-253,92,-253,10,-253,98,-253,101,-253,33,-253,104,-253,2,-253,12,-253,99,-253,32,-253,85,-253,84,-253,83,-253,82,-253,81,-253,86,-253,76,-253,51,-253,58,-253,141,-253,143,-253,80,-253,78,-253,160,-253,87,-253,45,-253,42,-253,21,-253,22,-253,144,-253,147,-253,145,-253,146,-253,155,-253,158,-253,157,-253,156,-253,57,-253,91,-253,40,-253,25,-253,97,-253,54,-253,35,-253,55,-253,102,-253,47,-253,36,-253,53,-253,60,-253,74,-253,72,-253,38,-253,70,-253,71,-253,127,-253,110,-253});
    states[292] = new State(new int[]{7,169,127,293,123,174,8,-255,118,-255,117,-255,131,-255,132,-255,133,-255,134,-255,130,-255,6,-255,116,-255,115,-255,128,-255,129,-255,13,-255,121,-255,100,-255,120,-255,9,-255,138,-255,136,-255,126,-255,5,-255,125,-255,124,-255,122,-255,137,-255,135,-255,16,-255,92,-255,10,-255,98,-255,101,-255,33,-255,104,-255,2,-255,12,-255,99,-255,32,-255,85,-255,84,-255,83,-255,82,-255,81,-255,86,-255,76,-255,51,-255,58,-255,141,-255,143,-255,80,-255,78,-255,160,-255,87,-255,45,-255,42,-255,21,-255,22,-255,144,-255,147,-255,145,-255,146,-255,155,-255,158,-255,157,-255,156,-255,57,-255,91,-255,40,-255,25,-255,97,-255,54,-255,35,-255,55,-255,102,-255,47,-255,36,-255,53,-255,60,-255,74,-255,72,-255,38,-255,70,-255,71,-255,110,-255},new int[]{-298,655});
    states[293] = new State(new int[]{8,295,143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-279,294,-272,179,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-281,1455,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,1456,-224,581,-223,582,-300,1457});
    states[294] = new State(-288);
    states[295] = new State(new int[]{9,296,143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-80,301,-78,307,-276,308,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[296] = new State(new int[]{127,297,121,-292,100,-292,120,-292,9,-292,8,-292,138,-292,136,-292,118,-292,117,-292,131,-292,132,-292,133,-292,134,-292,130,-292,116,-292,115,-292,128,-292,129,-292,126,-292,6,-292,5,-292,125,-292,123,-292,124,-292,122,-292,137,-292,135,-292,16,-292,92,-292,10,-292,98,-292,101,-292,33,-292,104,-292,2,-292,12,-292,99,-292,32,-292,85,-292,84,-292,83,-292,82,-292,81,-292,86,-292,13,-292,76,-292,51,-292,58,-292,141,-292,143,-292,80,-292,78,-292,160,-292,87,-292,45,-292,42,-292,21,-292,22,-292,144,-292,147,-292,145,-292,146,-292,155,-292,158,-292,157,-292,156,-292,57,-292,91,-292,40,-292,25,-292,97,-292,54,-292,35,-292,55,-292,102,-292,47,-292,36,-292,53,-292,60,-292,74,-292,72,-292,38,-292,70,-292,71,-292,110,-292});
    states[297] = new State(new int[]{8,299,143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-279,298,-272,179,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-281,1455,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,1456,-224,581,-223,582,-300,1457});
    states[298] = new State(-290);
    states[299] = new State(new int[]{9,300,143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-80,301,-78,307,-276,308,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[300] = new State(new int[]{127,297,121,-294,100,-294,120,-294,9,-294,8,-294,138,-294,136,-294,118,-294,117,-294,131,-294,132,-294,133,-294,134,-294,130,-294,116,-294,115,-294,128,-294,129,-294,126,-294,6,-294,5,-294,125,-294,123,-294,124,-294,122,-294,137,-294,135,-294,16,-294,92,-294,10,-294,98,-294,101,-294,33,-294,104,-294,2,-294,12,-294,99,-294,32,-294,85,-294,84,-294,83,-294,82,-294,81,-294,86,-294,13,-294,76,-294,51,-294,58,-294,141,-294,143,-294,80,-294,78,-294,160,-294,87,-294,45,-294,42,-294,21,-294,22,-294,144,-294,147,-294,145,-294,146,-294,155,-294,158,-294,157,-294,156,-294,57,-294,91,-294,40,-294,25,-294,97,-294,54,-294,35,-294,55,-294,102,-294,47,-294,36,-294,53,-294,60,-294,74,-294,72,-294,38,-294,70,-294,71,-294,110,-294});
    states[301] = new State(new int[]{9,302,100,680});
    states[302] = new State(new int[]{127,303,13,-250,121,-250,100,-250,120,-250,9,-250,8,-250,138,-250,136,-250,118,-250,117,-250,131,-250,132,-250,133,-250,134,-250,130,-250,116,-250,115,-250,128,-250,129,-250,126,-250,6,-250,5,-250,125,-250,123,-250,124,-250,122,-250,137,-250,135,-250,16,-250,92,-250,10,-250,98,-250,101,-250,33,-250,104,-250,2,-250,12,-250,99,-250,32,-250,85,-250,84,-250,83,-250,82,-250,81,-250,86,-250,76,-250,51,-250,58,-250,141,-250,143,-250,80,-250,78,-250,160,-250,87,-250,45,-250,42,-250,21,-250,22,-250,144,-250,147,-250,145,-250,146,-250,155,-250,158,-250,157,-250,156,-250,57,-250,91,-250,40,-250,25,-250,97,-250,54,-250,35,-250,55,-250,102,-250,47,-250,36,-250,53,-250,60,-250,74,-250,72,-250,38,-250,70,-250,71,-250,110,-250});
    states[303] = new State(new int[]{8,305,143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-279,304,-272,179,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-281,1455,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,1456,-224,581,-223,582,-300,1457});
    states[304] = new State(-291);
    states[305] = new State(new int[]{9,306,143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-80,301,-78,307,-276,308,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[306] = new State(new int[]{127,297,121,-295,100,-295,120,-295,9,-295,8,-295,138,-295,136,-295,118,-295,117,-295,131,-295,132,-295,133,-295,134,-295,130,-295,116,-295,115,-295,128,-295,129,-295,126,-295,6,-295,5,-295,125,-295,123,-295,124,-295,122,-295,137,-295,135,-295,16,-295,92,-295,10,-295,98,-295,101,-295,33,-295,104,-295,2,-295,12,-295,99,-295,32,-295,85,-295,84,-295,83,-295,82,-295,81,-295,86,-295,13,-295,76,-295,51,-295,58,-295,141,-295,143,-295,80,-295,78,-295,160,-295,87,-295,45,-295,42,-295,21,-295,22,-295,144,-295,147,-295,145,-295,146,-295,155,-295,158,-295,157,-295,156,-295,57,-295,91,-295,40,-295,25,-295,97,-295,54,-295,35,-295,55,-295,102,-295,47,-295,36,-295,53,-295,60,-295,74,-295,72,-295,38,-295,70,-295,71,-295,110,-295});
    states[307] = new State(-262);
    states[308] = new State(new int[]{120,309,9,-264,100,-264});
    states[309] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-87,310,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[310] = new State(-265);
    states[311] = new State(new int[]{120,312,125,313,123,314,121,315,124,316,122,317,137,318,135,319,16,-619,92,-619,10,-619,98,-619,101,-619,33,-619,104,-619,2,-619,9,-619,100,-619,12,-619,99,-619,32,-619,85,-619,84,-619,83,-619,82,-619,81,-619,86,-619,13,-619,6,-619,76,-619,5,-619,51,-619,58,-619,141,-619,143,-619,80,-619,78,-619,160,-619,87,-619,45,-619,42,-619,8,-619,21,-619,22,-619,144,-619,147,-619,145,-619,146,-619,155,-619,158,-619,157,-619,156,-619,57,-619,91,-619,40,-619,25,-619,97,-619,54,-619,35,-619,55,-619,102,-619,47,-619,36,-619,53,-619,60,-619,74,-619,72,-619,38,-619,70,-619,71,-619,116,-619,115,-619,128,-619,129,-619,126,-619,138,-619,136,-619,118,-619,117,-619,131,-619,132,-619,133,-619,134,-619,130,-619},new int[]{-196,32});
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
    states[322] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,5,-704,92,-704,10,-704,98,-704,101,-704,33,-704,104,-704,2,-704,9,-704,100,-704,12,-704,99,-704,32,-704,84,-704,83,-704,82,-704,81,-704,6,-704},new int[]{-115,323,-105,622,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,621,-267,598});
    states[323] = new State(new int[]{5,324,92,-707,10,-707,98,-707,101,-707,33,-707,104,-707,2,-707,9,-707,100,-707,12,-707,99,-707,32,-707,85,-707,84,-707,83,-707,82,-707,81,-707,86,-707,6,-707,76,-707});
    states[324] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-105,325,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,621,-267,598});
    states[325] = new State(new int[]{6,34,92,-709,10,-709,98,-709,101,-709,33,-709,104,-709,2,-709,9,-709,100,-709,12,-709,99,-709,32,-709,85,-709,84,-709,83,-709,82,-709,81,-709,86,-709,76,-709});
    states[326] = new State(new int[]{116,327,115,328,128,329,129,330,126,331,6,-723,5,-723,120,-723,125,-723,123,-723,121,-723,124,-723,122,-723,137,-723,135,-723,16,-723,92,-723,10,-723,98,-723,101,-723,33,-723,104,-723,2,-723,9,-723,100,-723,12,-723,99,-723,32,-723,85,-723,84,-723,83,-723,82,-723,81,-723,86,-723,13,-723,76,-723,51,-723,58,-723,141,-723,143,-723,80,-723,78,-723,160,-723,87,-723,45,-723,42,-723,8,-723,21,-723,22,-723,144,-723,147,-723,145,-723,146,-723,155,-723,158,-723,157,-723,156,-723,57,-723,91,-723,40,-723,25,-723,97,-723,54,-723,35,-723,55,-723,102,-723,47,-723,36,-723,53,-723,60,-723,74,-723,72,-723,38,-723,70,-723,71,-723,138,-723,136,-723,118,-723,117,-723,131,-723,132,-723,133,-723,134,-723,130,-723},new int[]{-197,36});
    states[327] = new State(-728);
    states[328] = new State(-729);
    states[329] = new State(-730);
    states[330] = new State(-731);
    states[331] = new State(-732);
    states[332] = new State(new int[]{138,333,136,1474,118,1477,117,1478,131,1479,132,1480,133,1481,134,1482,130,1483,116,-725,115,-725,128,-725,129,-725,126,-725,6,-725,5,-725,120,-725,125,-725,123,-725,121,-725,124,-725,122,-725,137,-725,135,-725,16,-725,92,-725,10,-725,98,-725,101,-725,33,-725,104,-725,2,-725,9,-725,100,-725,12,-725,99,-725,32,-725,85,-725,84,-725,83,-725,82,-725,81,-725,86,-725,13,-725,76,-725,51,-725,58,-725,141,-725,143,-725,80,-725,78,-725,160,-725,87,-725,45,-725,42,-725,8,-725,21,-725,22,-725,144,-725,147,-725,145,-725,146,-725,155,-725,158,-725,157,-725,156,-725,57,-725,91,-725,40,-725,25,-725,97,-725,54,-725,35,-725,55,-725,102,-725,47,-725,36,-725,53,-725,60,-725,74,-725,72,-725,38,-725,70,-725,71,-725},new int[]{-198,38});
    states[333] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,24,336},new int[]{-283,334,-278,335,-180,205,-147,207,-151,48,-152,51,-270,477});
    states[334] = new State(-739);
    states[335] = new State(-740);
    states[336] = new State(new int[]{11,337,58,1472});
    states[337] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,677,12,-279,100,-279},new int[]{-164,338,-271,1471,-272,1470,-93,181,-106,290,-107,291,-180,460,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154});
    states[338] = new State(new int[]{12,339,100,1468});
    states[339] = new State(new int[]{58,340});
    states[340] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-276,341,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[341] = new State(-273);
    states[342] = new State(new int[]{13,343,120,-227,9,-227,100,-227,121,-227,8,-227,138,-227,136,-227,118,-227,117,-227,131,-227,132,-227,133,-227,134,-227,130,-227,116,-227,115,-227,128,-227,129,-227,126,-227,6,-227,5,-227,125,-227,123,-227,124,-227,122,-227,137,-227,135,-227,16,-227,92,-227,10,-227,98,-227,101,-227,33,-227,104,-227,2,-227,12,-227,99,-227,32,-227,85,-227,84,-227,83,-227,82,-227,81,-227,86,-227,76,-227,51,-227,58,-227,141,-227,143,-227,80,-227,78,-227,160,-227,87,-227,45,-227,42,-227,21,-227,22,-227,144,-227,147,-227,145,-227,146,-227,155,-227,158,-227,157,-227,156,-227,57,-227,91,-227,40,-227,25,-227,97,-227,54,-227,35,-227,55,-227,102,-227,47,-227,36,-227,53,-227,60,-227,74,-227,72,-227,38,-227,70,-227,71,-227,127,-227,110,-227});
    states[343] = new State(-225);
    states[344] = new State(new int[]{11,345,7,-833,127,-833,123,-833,8,-833,118,-833,117,-833,131,-833,132,-833,133,-833,134,-833,130,-833,6,-833,116,-833,115,-833,128,-833,129,-833,13,-833,120,-833,9,-833,100,-833,121,-833,138,-833,136,-833,126,-833,5,-833,125,-833,124,-833,122,-833,137,-833,135,-833,16,-833,92,-833,10,-833,98,-833,101,-833,33,-833,104,-833,2,-833,12,-833,99,-833,32,-833,85,-833,84,-833,83,-833,82,-833,81,-833,86,-833,76,-833,51,-833,58,-833,141,-833,143,-833,80,-833,78,-833,160,-833,87,-833,45,-833,42,-833,21,-833,22,-833,144,-833,147,-833,145,-833,146,-833,155,-833,158,-833,157,-833,156,-833,57,-833,91,-833,40,-833,25,-833,97,-833,54,-833,35,-833,55,-833,102,-833,47,-833,36,-833,53,-833,60,-833,74,-833,72,-833,38,-833,70,-833,71,-833,110,-833});
    states[345] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-90,346,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[346] = new State(new int[]{12,347,13,193,16,197});
    states[347] = new State(-283);
    states[348] = new State(-157);
    states[349] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617,12,-814},new int[]{-69,350,-77,352,-92,362,-87,355,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[350] = new State(new int[]{12,351});
    states[351] = new State(-165);
    states[352] = new State(new int[]{100,353,12,-813,76,-813});
    states[353] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-92,354,-87,355,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[354] = new State(-816);
    states[355] = new State(new int[]{6,356,100,-817,12,-817,76,-817});
    states[356] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-87,357,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[357] = new State(-818);
    states[358] = new State(-744);
    states[359] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617,12,-814},new int[]{-69,360,-77,352,-92,362,-87,355,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[360] = new State(new int[]{12,361});
    states[361] = new State(-765);
    states[362] = new State(-815);
    states[363] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-96,364,-15,365,-165,149,-167,150,-166,154,-16,157,-57,162,-199,366,-113,372,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548});
    states[364] = new State(-766);
    states[365] = new State(new int[]{7,44,138,-763,136,-763,118,-763,117,-763,131,-763,132,-763,133,-763,134,-763,130,-763,116,-763,115,-763,128,-763,129,-763,126,-763,6,-763,5,-763,120,-763,125,-763,123,-763,121,-763,124,-763,122,-763,137,-763,135,-763,16,-763,92,-763,10,-763,98,-763,101,-763,33,-763,104,-763,2,-763,9,-763,100,-763,12,-763,99,-763,32,-763,85,-763,84,-763,83,-763,82,-763,81,-763,86,-763,13,-763,76,-763,51,-763,58,-763,141,-763,143,-763,80,-763,78,-763,160,-763,87,-763,45,-763,42,-763,8,-763,21,-763,22,-763,144,-763,147,-763,145,-763,146,-763,155,-763,158,-763,157,-763,156,-763,57,-763,91,-763,40,-763,25,-763,97,-763,54,-763,35,-763,55,-763,102,-763,47,-763,36,-763,53,-763,60,-763,74,-763,72,-763,38,-763,70,-763,71,-763,11,-788,17,-788});
    states[366] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-96,367,-15,365,-165,149,-167,150,-166,154,-16,157,-57,162,-199,366,-113,372,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548});
    states[367] = new State(-767);
    states[368] = new State(-167);
    states[369] = new State(-168);
    states[370] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-96,371,-15,365,-165,149,-167,150,-166,154,-16,157,-57,162,-199,366,-113,372,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548});
    states[371] = new State(-768);
    states[372] = new State(-769);
    states[373] = new State(new int[]{141,1467,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,432,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541},new int[]{-111,374,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713});
    states[374] = new State(new int[]{8,375,7,387,142,422,4,423,110,-775,111,-775,112,-775,113,-775,114,-775,92,-775,10,-775,98,-775,101,-775,33,-775,104,-775,2,-775,138,-775,136,-775,118,-775,117,-775,131,-775,132,-775,133,-775,134,-775,130,-775,116,-775,115,-775,128,-775,129,-775,126,-775,6,-775,5,-775,120,-775,125,-775,123,-775,121,-775,124,-775,122,-775,137,-775,135,-775,16,-775,9,-775,100,-775,12,-775,99,-775,32,-775,85,-775,84,-775,83,-775,82,-775,81,-775,86,-775,13,-775,119,-775,76,-775,51,-775,58,-775,141,-775,143,-775,80,-775,78,-775,160,-775,87,-775,45,-775,42,-775,21,-775,22,-775,144,-775,147,-775,145,-775,146,-775,155,-775,158,-775,157,-775,156,-775,57,-775,91,-775,40,-775,25,-775,97,-775,54,-775,35,-775,55,-775,102,-775,47,-775,36,-775,53,-775,60,-775,74,-775,72,-775,38,-775,70,-775,71,-775,11,-787,17,-787});
    states[375] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,1464,8,662,21,270,22,275,76,541,40,608,5,617,18,686,37,695,44,699,9,-812},new int[]{-68,376,-72,378,-89,1466,-87,381,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,1461,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-322,1465,-100,671,-323,694});
    states[376] = new State(new int[]{9,377});
    states[377] = new State(-792);
    states[378] = new State(new int[]{100,379,9,-811});
    states[379] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,1464,8,662,21,270,22,275,76,541,40,608,5,617,18,686,37,695,44,699},new int[]{-89,380,-87,381,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,1461,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-322,1465,-100,671,-323,694});
    states[380] = new State(-595);
    states[381] = new State(-601);
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
    states[425] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,432,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541},new int[]{-111,426,-116,427,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713});
    states[426] = new State(new int[]{8,375,7,387,142,422,4,423,15,425,110,-773,111,-773,112,-773,113,-773,114,-773,92,-773,10,-773,98,-773,101,-773,33,-773,104,-773,2,-773,138,-773,136,-773,118,-773,117,-773,131,-773,132,-773,133,-773,134,-773,130,-773,116,-773,115,-773,128,-773,129,-773,126,-773,6,-773,5,-773,120,-773,125,-773,123,-773,121,-773,124,-773,122,-773,137,-773,135,-773,16,-773,9,-773,100,-773,12,-773,99,-773,32,-773,85,-773,84,-773,83,-773,82,-773,81,-773,86,-773,13,-773,119,-773,76,-773,51,-773,58,-773,141,-773,143,-773,80,-773,78,-773,160,-773,87,-773,45,-773,42,-773,21,-773,22,-773,144,-773,147,-773,145,-773,146,-773,155,-773,158,-773,157,-773,156,-773,57,-773,91,-773,40,-773,25,-773,97,-773,54,-773,35,-773,55,-773,102,-773,47,-773,36,-773,53,-773,60,-773,74,-773,72,-773,38,-773,70,-773,71,-773,11,-787,17,-787});
    states[427] = new State(-774);
    states[428] = new State(-793);
    states[429] = new State(-794);
    states[430] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,431,-151,48,-152,51});
    states[431] = new State(-795);
    states[432] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617,53,1458,18,686},new int[]{-87,433,-359,435,-102,440,-101,717,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-358,720,-100,721});
    states[433] = new State(new int[]{9,434});
    states[434] = new State(-796);
    states[435] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617,53,1458},new int[]{-87,436,-358,438,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[436] = new State(new int[]{9,437});
    states[437] = new State(-797);
    states[438] = new State(-791);
    states[439] = new State(new int[]{53,663,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617,18,686},new int[]{-87,433,-359,435,-102,440,-101,717,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-358,720,-100,721});
    states[440] = new State(new int[]{100,441});
    states[441] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,18,686},new int[]{-79,442,-102,1145,-101,1144,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-100,721});
    states[442] = new State(new int[]{100,1142,5,454,10,-1006,9,-1006},new int[]{-324,443});
    states[443] = new State(new int[]{10,446,9,-994},new int[]{-331,444});
    states[444] = new State(new int[]{9,445});
    states[445] = new State(-759);
    states[446] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-326,447,-327,1094,-158,450,-147,812,-151,48,-152,51});
    states[447] = new State(new int[]{10,448,9,-995});
    states[448] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-327,449,-158,450,-147,812,-151,48,-152,51});
    states[449] = new State(-1004);
    states[450] = new State(new int[]{100,452,5,454,10,-1006,9,-1006},new int[]{-324,451});
    states[451] = new State(-1005);
    states[452] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,453,-151,48,-152,51});
    states[453] = new State(-344);
    states[454] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-275,455,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[455] = new State(-1007);
    states[456] = new State(-486);
    states[457] = new State(-256);
    states[458] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156},new int[]{-107,459,-180,460,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154});
    states[459] = new State(new int[]{8,186,118,-257,117,-257,131,-257,132,-257,133,-257,134,-257,130,-257,6,-257,116,-257,115,-257,128,-257,129,-257,13,-257,121,-257,100,-257,120,-257,9,-257,138,-257,136,-257,126,-257,5,-257,125,-257,123,-257,124,-257,122,-257,137,-257,135,-257,16,-257,92,-257,10,-257,98,-257,101,-257,33,-257,104,-257,2,-257,12,-257,99,-257,32,-257,85,-257,84,-257,83,-257,82,-257,81,-257,86,-257,76,-257,51,-257,58,-257,141,-257,143,-257,80,-257,78,-257,160,-257,87,-257,45,-257,42,-257,21,-257,22,-257,144,-257,147,-257,145,-257,146,-257,155,-257,158,-257,157,-257,156,-257,57,-257,91,-257,40,-257,25,-257,97,-257,54,-257,35,-257,55,-257,102,-257,47,-257,36,-257,53,-257,60,-257,74,-257,72,-257,38,-257,70,-257,71,-257,127,-257,110,-257});
    states[460] = new State(new int[]{7,169,8,-255,118,-255,117,-255,131,-255,132,-255,133,-255,134,-255,130,-255,6,-255,116,-255,115,-255,128,-255,129,-255,13,-255,121,-255,100,-255,120,-255,9,-255,138,-255,136,-255,126,-255,5,-255,125,-255,123,-255,124,-255,122,-255,137,-255,135,-255,16,-255,92,-255,10,-255,98,-255,101,-255,33,-255,104,-255,2,-255,12,-255,99,-255,32,-255,85,-255,84,-255,83,-255,82,-255,81,-255,86,-255,76,-255,51,-255,58,-255,141,-255,143,-255,80,-255,78,-255,160,-255,87,-255,45,-255,42,-255,21,-255,22,-255,144,-255,147,-255,145,-255,146,-255,155,-255,158,-255,157,-255,156,-255,57,-255,91,-255,40,-255,25,-255,97,-255,54,-255,35,-255,55,-255,102,-255,47,-255,36,-255,53,-255,60,-255,74,-255,72,-255,38,-255,70,-255,71,-255,127,-255,110,-255});
    states[461] = new State(-258);
    states[462] = new State(new int[]{9,463,143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-80,301,-78,307,-276,308,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[463] = new State(new int[]{127,297});
    states[464] = new State(-228);
    states[465] = new State(new int[]{13,466,127,467,120,-233,9,-233,100,-233,121,-233,8,-233,138,-233,136,-233,118,-233,117,-233,131,-233,132,-233,133,-233,134,-233,130,-233,116,-233,115,-233,128,-233,129,-233,126,-233,6,-233,5,-233,125,-233,123,-233,124,-233,122,-233,137,-233,135,-233,16,-233,92,-233,10,-233,98,-233,101,-233,33,-233,104,-233,2,-233,12,-233,99,-233,32,-233,85,-233,84,-233,83,-233,82,-233,81,-233,86,-233,76,-233,51,-233,58,-233,141,-233,143,-233,80,-233,78,-233,160,-233,87,-233,45,-233,42,-233,21,-233,22,-233,144,-233,147,-233,145,-233,146,-233,155,-233,158,-233,157,-233,156,-233,57,-233,91,-233,40,-233,25,-233,97,-233,54,-233,35,-233,55,-233,102,-233,47,-233,36,-233,53,-233,60,-233,74,-233,72,-233,38,-233,70,-233,71,-233,110,-233});
    states[466] = new State(-226);
    states[467] = new State(new int[]{8,469,143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-279,468,-272,179,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-281,1455,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,1456,-224,581,-223,582,-300,1457});
    states[468] = new State(-289);
    states[469] = new State(new int[]{9,470,143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-80,301,-78,307,-276,308,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[470] = new State(new int[]{127,297,121,-293,100,-293,120,-293,9,-293,8,-293,138,-293,136,-293,118,-293,117,-293,131,-293,132,-293,133,-293,134,-293,130,-293,116,-293,115,-293,128,-293,129,-293,126,-293,6,-293,5,-293,125,-293,123,-293,124,-293,122,-293,137,-293,135,-293,16,-293,92,-293,10,-293,98,-293,101,-293,33,-293,104,-293,2,-293,12,-293,99,-293,32,-293,85,-293,84,-293,83,-293,82,-293,81,-293,86,-293,13,-293,76,-293,51,-293,58,-293,141,-293,143,-293,80,-293,78,-293,160,-293,87,-293,45,-293,42,-293,21,-293,22,-293,144,-293,147,-293,145,-293,146,-293,155,-293,158,-293,157,-293,156,-293,57,-293,91,-293,40,-293,25,-293,97,-293,54,-293,35,-293,55,-293,102,-293,47,-293,36,-293,53,-293,60,-293,74,-293,72,-293,38,-293,70,-293,71,-293,110,-293});
    states[471] = new State(-229);
    states[472] = new State(-230);
    states[473] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-275,474,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[474] = new State(-266);
    states[475] = new State(-231);
    states[476] = new State(-267);
    states[477] = new State(-274);
    states[478] = new State(-268);
    states[479] = new State(new int[]{8,1335,23,-315,11,-315,92,-315,84,-315,83,-315,82,-315,81,-315,29,-315,143,-315,85,-315,86,-315,80,-315,78,-315,160,-315,87,-315,62,-315,28,-315,26,-315,44,-315,37,-315,19,-315,30,-315,31,-315,46,-315,27,-315},new int[]{-183,480});
    states[480] = new State(new int[]{23,1326,11,-322,92,-322,84,-322,83,-322,82,-322,81,-322,29,-322,143,-322,85,-322,86,-322,80,-322,78,-322,160,-322,87,-322,62,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322},new int[]{-317,481,-316,1324,-315,1346});
    states[481] = new State(new int[]{11,647,92,-339,84,-339,83,-339,82,-339,81,-339,29,-212,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,62,-212,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-25,482,-32,1247,-34,486,-45,1248,-6,1249,-250,1130,-33,1411,-54,1413,-53,492,-55,1412});
    states[482] = new State(new int[]{92,483,84,1243,83,1244,82,1245,81,1246},new int[]{-7,484});
    states[483] = new State(-297);
    states[484] = new State(new int[]{11,647,92,-339,84,-339,83,-339,82,-339,81,-339,29,-212,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,62,-212,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-32,485,-34,486,-45,1248,-6,1249,-250,1130,-33,1411,-54,1413,-53,492,-55,1412});
    states[485] = new State(-334);
    states[486] = new State(new int[]{10,488,92,-345,84,-345,83,-345,82,-345,81,-345},new int[]{-190,487});
    states[487] = new State(-340);
    states[488] = new State(new int[]{11,647,92,-346,84,-346,83,-346,82,-346,81,-346,29,-212,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,62,-212,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-45,489,-33,490,-6,1249,-250,1130,-54,1413,-53,492,-55,1412});
    states[489] = new State(-348);
    states[490] = new State(new int[]{11,647,92,-342,84,-342,83,-342,82,-342,81,-342,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-54,491,-53,492,-6,493,-250,1130,-55,1412});
    states[491] = new State(-351);
    states[492] = new State(-352);
    states[493] = new State(new int[]{28,1371,26,1372,44,1319,37,1354,19,1374,30,1382,31,1389,11,647,46,1396,27,1405},new int[]{-222,494,-250,495,-219,496,-258,497,-3,498,-230,1373,-228,1307,-225,1318,-229,1353,-227,1380,-215,1393,-216,1394,-218,1395});
    states[494] = new State(-361);
    states[495] = new State(-211);
    states[496] = new State(-362);
    states[497] = new State(-378);
    states[498] = new State(new int[]{30,500,19,1189,46,1261,27,1299,44,1319,37,1354},new int[]{-230,499,-216,1188,-228,1307,-225,1318,-229,1353});
    states[499] = new State(-365);
    states[500] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,8,-377,110,-377,10,-377},new int[]{-172,501,-171,1171,-170,1172,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[501] = new State(new int[]{8,585,110,-470,10,-470},new int[]{-128,502});
    states[502] = new State(new int[]{110,504,10,1160},new int[]{-207,503});
    states[503] = new State(-374);
    states[504] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494},new int[]{-260,505,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[505] = new State(new int[]{10,506});
    states[506] = new State(-423);
    states[507] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,92,-574,10,-574,98,-574,101,-574,33,-574,104,-574,2,-574,9,-574,100,-574,12,-574,99,-574,32,-574,84,-574,83,-574,82,-574,81,-574},new int[]{-147,431,-151,48,-152,51});
    states[508] = new State(new int[]{53,509,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617,18,686},new int[]{-87,433,-359,435,-102,440,-111,704,-101,717,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-358,720,-100,721});
    states[509] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,510,-151,48,-152,51});
    states[510] = new State(new int[]{110,511,100,1150});
    states[511] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-99,512,-87,514,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-101,28,-240,599,-117,600,-242,607,-120,616});
    states[512] = new State(new int[]{9,513,16,30,10,-607,13,-610});
    states[513] = new State(-778);
    states[514] = new State(new int[]{10,515});
    states[515] = new State(-789);
    states[516] = new State(-798);
    states[517] = new State(-799);
    states[518] = new State(new int[]{11,519,17,1146});
    states[519] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,661,8,662,21,270,22,275,76,541,40,608,5,617,18,686,37,695,44,699},new int[]{-71,520,-88,660,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-322,836,-100,671,-323,694});
    states[520] = new State(new int[]{12,521,100,522});
    states[521] = new State(-801);
    states[522] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,661,8,662,21,270,22,275,76,541,40,608,5,617,18,686,37,695,44,699},new int[]{-88,523,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-322,836,-100,671,-323,694});
    states[523] = new State(-593);
    states[524] = new State(new int[]{127,525,8,-793,7,-793,142,-793,4,-793,15,-793,138,-793,136,-793,118,-793,117,-793,131,-793,132,-793,133,-793,134,-793,130,-793,116,-793,115,-793,128,-793,129,-793,126,-793,6,-793,5,-793,120,-793,125,-793,123,-793,121,-793,124,-793,122,-793,137,-793,135,-793,16,-793,92,-793,10,-793,98,-793,101,-793,33,-793,104,-793,2,-793,9,-793,100,-793,12,-793,99,-793,32,-793,85,-793,84,-793,83,-793,82,-793,81,-793,86,-793,13,-793,119,-793,11,-793,17,-793});
    states[525] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,18,686,37,695,44,699,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-328,526,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,669,-117,600,-322,670,-100,671,-323,694,-330,861,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[526] = new State(-986);
    states[527] = new State(-1023);
    states[528] = new State(new int[]{16,30,92,-616,10,-616,98,-616,101,-616,33,-616,104,-616,2,-616,9,-616,100,-616,12,-616,99,-616,32,-616,85,-616,84,-616,83,-616,82,-616,81,-616,86,-616,13,-610});
    states[529] = new State(new int[]{6,34,120,-644,125,-644,123,-644,121,-644,124,-644,122,-644,137,-644,135,-644,16,-644,92,-644,10,-644,98,-644,101,-644,33,-644,104,-644,2,-644,9,-644,100,-644,12,-644,99,-644,32,-644,85,-644,84,-644,83,-644,82,-644,81,-644,86,-644,13,-644,76,-644,5,-644,51,-644,58,-644,141,-644,143,-644,80,-644,78,-644,160,-644,87,-644,45,-644,42,-644,8,-644,21,-644,22,-644,144,-644,147,-644,145,-644,146,-644,155,-644,158,-644,157,-644,156,-644,57,-644,91,-644,40,-644,25,-644,97,-644,54,-644,35,-644,55,-644,102,-644,47,-644,36,-644,53,-644,60,-644,74,-644,72,-644,38,-644,70,-644,71,-644,116,-644,115,-644,128,-644,129,-644,126,-644,138,-644,136,-644,118,-644,117,-644,131,-644,132,-644,133,-644,134,-644,130,-644});
    states[530] = new State(new int[]{53,663,9,665,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,719,21,270,22,275,76,541,40,608,5,617,18,686},new int[]{-87,433,-359,435,-102,531,-147,1105,-4,715,-101,717,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,718,-132,373,-111,386,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-358,720,-100,721});
    states[531] = new State(new int[]{100,532});
    states[532] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,18,686},new int[]{-79,533,-102,1145,-101,1144,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-100,721});
    states[533] = new State(new int[]{100,1142,5,454,10,-1006,9,-1006},new int[]{-324,534});
    states[534] = new State(new int[]{10,446,9,-994},new int[]{-331,535});
    states[535] = new State(new int[]{9,536});
    states[536] = new State(new int[]{5,673,7,-759,138,-759,136,-759,118,-759,117,-759,131,-759,132,-759,133,-759,134,-759,130,-759,116,-759,115,-759,128,-759,129,-759,126,-759,6,-759,120,-759,125,-759,123,-759,121,-759,124,-759,122,-759,137,-759,135,-759,16,-759,92,-759,10,-759,98,-759,101,-759,33,-759,104,-759,2,-759,9,-759,100,-759,12,-759,99,-759,32,-759,85,-759,84,-759,83,-759,82,-759,81,-759,86,-759,13,-759,127,-1008},new int[]{-335,537,-325,538});
    states[537] = new State(-991);
    states[538] = new State(new int[]{127,539});
    states[539] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,18,686,37,695,44,699,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-328,540,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,669,-117,600,-322,670,-100,671,-323,694,-330,861,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[540] = new State(-996);
    states[541] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-69,542,-77,352,-92,362,-87,355,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
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
    states[557] = new State(new int[]{14,562,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,53,564,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,929,8,942},new int[]{-353,558,-351,1141,-15,563,-165,149,-167,150,-166,154,-16,157,-340,1132,-283,1133,-180,205,-147,207,-151,48,-152,51,-343,1139,-344,1140});
    states[558] = new State(new int[]{9,559,10,560,100,1137});
    states[559] = new State(-647);
    states[560] = new State(new int[]{14,562,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,53,564,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,929,8,942},new int[]{-351,561,-15,563,-165,149,-167,150,-166,154,-16,157,-340,1132,-283,1133,-180,205,-147,207,-151,48,-152,51,-343,1139,-344,1140});
    states[561] = new State(-684);
    states[562] = new State(-686);
    states[563] = new State(-687);
    states[564] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,565,-151,48,-152,51});
    states[565] = new State(new int[]{5,566,9,-689,10,-689,100,-689});
    states[566] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-276,567,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[567] = new State(-688);
    states[568] = new State(-269);
    states[569] = new State(new int[]{58,570});
    states[570] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-276,571,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[571] = new State(-280);
    states[572] = new State(-270);
    states[573] = new State(new int[]{58,574,121,-282,100,-282,120,-282,9,-282,8,-282,138,-282,136,-282,118,-282,117,-282,131,-282,132,-282,133,-282,134,-282,130,-282,116,-282,115,-282,128,-282,129,-282,126,-282,6,-282,5,-282,125,-282,123,-282,124,-282,122,-282,137,-282,135,-282,16,-282,92,-282,10,-282,98,-282,101,-282,33,-282,104,-282,2,-282,12,-282,99,-282,32,-282,85,-282,84,-282,83,-282,82,-282,81,-282,86,-282,13,-282,76,-282,51,-282,141,-282,143,-282,80,-282,78,-282,160,-282,87,-282,45,-282,42,-282,21,-282,22,-282,144,-282,147,-282,145,-282,146,-282,155,-282,158,-282,157,-282,156,-282,57,-282,91,-282,40,-282,25,-282,97,-282,54,-282,35,-282,55,-282,102,-282,47,-282,36,-282,53,-282,60,-282,74,-282,72,-282,38,-282,70,-282,71,-282,127,-282,110,-282});
    states[574] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-276,575,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[575] = new State(-281);
    states[576] = new State(-271);
    states[577] = new State(new int[]{58,578});
    states[578] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-276,579,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[579] = new State(-272);
    states[580] = new State(-232);
    states[581] = new State(-284);
    states[582] = new State(-285);
    states[583] = new State(new int[]{8,585,121,-470,100,-470,120,-470,9,-470,138,-470,136,-470,118,-470,117,-470,131,-470,132,-470,133,-470,134,-470,130,-470,116,-470,115,-470,128,-470,129,-470,126,-470,6,-470,5,-470,125,-470,123,-470,124,-470,122,-470,137,-470,135,-470,16,-470,92,-470,10,-470,98,-470,101,-470,33,-470,104,-470,2,-470,12,-470,99,-470,32,-470,85,-470,84,-470,83,-470,82,-470,81,-470,86,-470,13,-470,76,-470,51,-470,58,-470,141,-470,143,-470,80,-470,78,-470,160,-470,87,-470,45,-470,42,-470,21,-470,22,-470,144,-470,147,-470,145,-470,146,-470,155,-470,158,-470,157,-470,156,-470,57,-470,91,-470,40,-470,25,-470,97,-470,54,-470,35,-470,55,-470,102,-470,47,-470,36,-470,53,-470,60,-470,74,-470,72,-470,38,-470,70,-470,71,-470,127,-470,110,-470},new int[]{-128,584});
    states[584] = new State(-286);
    states[585] = new State(new int[]{9,586,11,647,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,53,-212,29,-212,108,-212},new int[]{-129,587,-56,1131,-6,591,-250,1130});
    states[586] = new State(-471);
    states[587] = new State(new int[]{9,588,10,589});
    states[588] = new State(-472);
    states[589] = new State(new int[]{11,647,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,53,-212,29,-212,108,-212},new int[]{-56,590,-6,591,-250,1130});
    states[590] = new State(-474);
    states[591] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,53,631,29,637,108,643,11,647},new int[]{-295,592,-250,495,-159,593,-135,630,-147,629,-151,48,-152,51});
    states[592] = new State(-475);
    states[593] = new State(new int[]{5,594,100,627});
    states[594] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-275,595,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[595] = new State(new int[]{110,596,9,-476,10,-476});
    states[596] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-87,597,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[597] = new State(-480);
    states[598] = new State(-736);
    states[599] = new State(new int[]{92,-608,10,-608,98,-608,101,-608,33,-608,104,-608,2,-608,9,-608,100,-608,12,-608,99,-608,32,-608,85,-608,84,-608,83,-608,82,-608,81,-608,86,-608,6,-608,76,-608,5,-608,51,-608,58,-608,141,-608,143,-608,80,-608,78,-608,160,-608,87,-608,45,-608,42,-608,8,-608,21,-608,22,-608,144,-608,147,-608,145,-608,146,-608,155,-608,158,-608,157,-608,156,-608,57,-608,91,-608,40,-608,25,-608,97,-608,54,-608,35,-608,55,-608,102,-608,47,-608,36,-608,53,-608,60,-608,74,-608,72,-608,38,-608,70,-608,71,-608,13,-611});
    states[600] = new State(new int[]{13,601});
    states[601] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-117,602,-99,605,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,606});
    states[602] = new State(new int[]{5,603,13,601});
    states[603] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-117,604,-99,605,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,606});
    states[604] = new State(new int[]{13,601,92,-624,10,-624,98,-624,101,-624,33,-624,104,-624,2,-624,9,-624,100,-624,12,-624,99,-624,32,-624,85,-624,84,-624,83,-624,82,-624,81,-624,86,-624,6,-624,76,-624,5,-624,51,-624,58,-624,141,-624,143,-624,80,-624,78,-624,160,-624,87,-624,45,-624,42,-624,8,-624,21,-624,22,-624,144,-624,147,-624,145,-624,146,-624,155,-624,158,-624,157,-624,156,-624,57,-624,91,-624,40,-624,25,-624,97,-624,54,-624,35,-624,55,-624,102,-624,47,-624,36,-624,53,-624,60,-624,74,-624,72,-624,38,-624,70,-624,71,-624});
    states[605] = new State(new int[]{16,30,5,-610,13,-610,92,-610,10,-610,98,-610,101,-610,33,-610,104,-610,2,-610,9,-610,100,-610,12,-610,99,-610,32,-610,85,-610,84,-610,83,-610,82,-610,81,-610,86,-610,6,-610,76,-610,51,-610,58,-610,141,-610,143,-610,80,-610,78,-610,160,-610,87,-610,45,-610,42,-610,8,-610,21,-610,22,-610,144,-610,147,-610,145,-610,146,-610,155,-610,158,-610,157,-610,156,-610,57,-610,91,-610,40,-610,25,-610,97,-610,54,-610,35,-610,55,-610,102,-610,47,-610,36,-610,53,-610,60,-610,74,-610,72,-610,38,-610,70,-610,71,-610});
    states[606] = new State(-611);
    states[607] = new State(-609);
    states[608] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-118,609,-99,614,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-242,615});
    states[609] = new State(new int[]{51,610});
    states[610] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-118,611,-99,614,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-242,615});
    states[611] = new State(new int[]{32,612});
    states[612] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-118,613,-99,614,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-242,615});
    states[613] = new State(-625);
    states[614] = new State(new int[]{16,30,51,-612,32,-612,120,-612,125,-612,123,-612,121,-612,124,-612,122,-612,137,-612,135,-612,92,-612,10,-612,98,-612,101,-612,33,-612,104,-612,2,-612,9,-612,100,-612,12,-612,99,-612,85,-612,84,-612,83,-612,82,-612,81,-612,86,-612,13,-612,6,-612,76,-612,5,-612,58,-612,141,-612,143,-612,80,-612,78,-612,160,-612,87,-612,45,-612,42,-612,8,-612,21,-612,22,-612,144,-612,147,-612,145,-612,146,-612,155,-612,158,-612,157,-612,156,-612,57,-612,91,-612,40,-612,25,-612,97,-612,54,-612,35,-612,55,-612,102,-612,47,-612,36,-612,53,-612,60,-612,74,-612,72,-612,38,-612,70,-612,71,-612,116,-612,115,-612,128,-612,129,-612,126,-612,138,-612,136,-612,118,-612,117,-612,131,-612,132,-612,133,-612,134,-612,130,-612});
    states[615] = new State(-613);
    states[616] = new State(-606);
    states[617] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,5,-704,92,-704,10,-704,98,-704,101,-704,33,-704,104,-704,2,-704,9,-704,100,-704,12,-704,99,-704,32,-704,84,-704,83,-704,82,-704,81,-704,6,-704},new int[]{-115,618,-105,622,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,621,-267,598});
    states[618] = new State(new int[]{5,619,92,-708,10,-708,98,-708,101,-708,33,-708,104,-708,2,-708,9,-708,100,-708,12,-708,99,-708,32,-708,85,-708,84,-708,83,-708,82,-708,81,-708,86,-708,6,-708,76,-708});
    states[619] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541},new int[]{-105,620,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,621,-267,598});
    states[620] = new State(new int[]{6,34,92,-710,10,-710,98,-710,101,-710,33,-710,104,-710,2,-710,9,-710,100,-710,12,-710,99,-710,32,-710,85,-710,84,-710,83,-710,82,-710,81,-710,86,-710,76,-710});
    states[621] = new State(-735);
    states[622] = new State(new int[]{6,34,5,-703,92,-703,10,-703,98,-703,101,-703,33,-703,104,-703,2,-703,9,-703,100,-703,12,-703,99,-703,32,-703,85,-703,84,-703,83,-703,82,-703,81,-703,86,-703,76,-703});
    states[623] = new State(new int[]{8,585,5,-470},new int[]{-128,624});
    states[624] = new State(new int[]{5,625});
    states[625] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-275,626,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[626] = new State(-287);
    states[627] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-135,628,-147,629,-151,48,-152,51});
    states[628] = new State(-484);
    states[629] = new State(-485);
    states[630] = new State(-483);
    states[631] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-159,632,-135,630,-147,629,-151,48,-152,51});
    states[632] = new State(new int[]{5,633,100,627});
    states[633] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-275,634,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[634] = new State(new int[]{110,635,9,-477,10,-477});
    states[635] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-87,636,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[636] = new State(-481);
    states[637] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-159,638,-135,630,-147,629,-151,48,-152,51});
    states[638] = new State(new int[]{5,639,100,627});
    states[639] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-275,640,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[640] = new State(new int[]{110,641,9,-478,10,-478});
    states[641] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-87,642,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[642] = new State(-482);
    states[643] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-159,644,-135,630,-147,629,-151,48,-152,51});
    states[644] = new State(new int[]{5,645,100,627});
    states[645] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-275,646,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[646] = new State(-479);
    states[647] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-251,648,-8,1129,-9,652,-180,653,-147,1124,-151,48,-152,51,-300,1127});
    states[648] = new State(new int[]{12,649,100,650});
    states[649] = new State(-213);
    states[650] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-8,651,-9,652,-180,653,-147,1124,-151,48,-152,51,-300,1127});
    states[651] = new State(-215);
    states[652] = new State(-216);
    states[653] = new State(new int[]{7,169,8,656,123,174,12,-642,100,-642},new int[]{-70,654,-298,655});
    states[654] = new State(-781);
    states[655] = new State(-234);
    states[656] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,661,8,662,21,270,22,275,76,541,40,608,5,617,18,686,37,695,44,699,9,-810},new int[]{-67,657,-71,659,-88,660,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-322,836,-100,671,-323,694});
    states[657] = new State(new int[]{9,658});
    states[658] = new State(-643);
    states[659] = new State(new int[]{100,522,12,-809,9,-809});
    states[660] = new State(-592);
    states[661] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,92,-600,10,-600,98,-600,101,-600,33,-600,104,-600,2,-600,9,-600,100,-600,12,-600,99,-600,32,-600,84,-600,83,-600,82,-600,81,-600},new int[]{-147,431,-151,48,-152,51});
    states[662] = new State(new int[]{53,663,9,665,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617,18,686},new int[]{-87,433,-359,435,-102,531,-147,1105,-101,717,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-358,720,-100,721});
    states[663] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,664,-151,48,-152,51});
    states[664] = new State(new int[]{110,511});
    states[665] = new State(new int[]{5,673,127,-1008},new int[]{-325,666});
    states[666] = new State(new int[]{127,667});
    states[667] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,18,686,37,695,44,699,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-328,668,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,669,-117,600,-322,670,-100,671,-323,694,-330,861,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[668] = new State(-987);
    states[669] = new State(new int[]{92,-617,10,-617,98,-617,101,-617,33,-617,104,-617,2,-617,9,-617,100,-617,12,-617,99,-617,32,-617,85,-617,84,-617,83,-617,82,-617,81,-617,86,-617,13,-611});
    states[670] = new State(-618);
    states[671] = new State(new int[]{5,673,127,-1008},new int[]{-335,672,-325,538});
    states[672] = new State(-992);
    states[673] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,677,142,473,24,336,48,479,49,569,34,573,73,577},new int[]{-277,674,-272,675,-93,181,-106,290,-107,291,-180,676,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-256,682,-249,683,-281,684,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-300,685});
    states[674] = new State(-1009);
    states[675] = new State(-487);
    states[676] = new State(new int[]{7,169,123,174,8,-255,118,-255,117,-255,131,-255,132,-255,133,-255,134,-255,130,-255,6,-255,116,-255,115,-255,128,-255,129,-255,127,-255},new int[]{-298,655});
    states[677] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-80,678,-78,307,-276,308,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[678] = new State(new int[]{9,679,100,680});
    states[679] = new State(-250);
    states[680] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-78,681,-276,308,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[681] = new State(-263);
    states[682] = new State(-488);
    states[683] = new State(-489);
    states[684] = new State(-490);
    states[685] = new State(-491);
    states[686] = new State(new int[]{18,686,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-24,687,-23,693,-100,691,-147,692,-151,48,-152,51});
    states[687] = new State(new int[]{100,688});
    states[688] = new State(new int[]{18,686,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-23,689,-100,691,-147,692,-151,48,-152,51});
    states[689] = new State(new int[]{9,690,100,-981});
    states[690] = new State(-977);
    states[691] = new State(-978);
    states[692] = new State(-979);
    states[693] = new State(-980);
    states[694] = new State(-993);
    states[695] = new State(new int[]{8,1095,5,673,127,-1008},new int[]{-325,696});
    states[696] = new State(new int[]{127,697});
    states[697] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,18,686,37,695,44,699,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-328,698,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,669,-117,600,-322,670,-100,671,-323,694,-330,861,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[698] = new State(-997);
    states[699] = new State(new int[]{127,700,8,1086});
    states[700] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,703,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-329,701,-212,702,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-4,722,-330,723,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[701] = new State(-1000);
    states[702] = new State(-1025);
    states[703] = new State(new int[]{53,663,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,719,21,270,22,275,76,541,40,608,5,617,18,686},new int[]{-87,433,-359,435,-102,440,-111,704,-4,715,-101,717,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,718,-132,373,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-358,720,-100,721});
    states[704] = new State(new int[]{100,705,8,375,7,387,142,422,4,423,15,425,138,-776,136,-776,118,-776,117,-776,131,-776,132,-776,133,-776,134,-776,130,-776,116,-776,115,-776,128,-776,129,-776,126,-776,6,-776,5,-776,120,-776,125,-776,123,-776,121,-776,124,-776,122,-776,137,-776,135,-776,16,-776,9,-776,13,-776,119,-776,110,-776,111,-776,112,-776,113,-776,114,-776,11,-787,17,-787});
    states[705] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,432,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541},new int[]{-336,706,-111,714,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713});
    states[706] = new State(new int[]{9,707,100,710});
    states[707] = new State(new int[]{110,416,111,417,112,418,113,419,114,420},new int[]{-194,708});
    states[708] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-87,709,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[709] = new State(-523);
    states[710] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,432,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541},new int[]{-111,711,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713});
    states[711] = new State(new int[]{8,375,7,387,142,422,4,423,9,-525,100,-525,11,-787,17,-787});
    states[712] = new State(new int[]{7,44,11,-788,17,-788});
    states[713] = new State(new int[]{7,546});
    states[714] = new State(new int[]{8,375,7,387,142,422,4,423,9,-524,100,-524,11,-787,17,-787});
    states[715] = new State(new int[]{9,716});
    states[716] = new State(-1022);
    states[717] = new State(new int[]{9,-605,100,-982});
    states[718] = new State(new int[]{110,416,111,417,112,418,113,419,114,420,138,-769,136,-769,118,-769,117,-769,131,-769,132,-769,133,-769,134,-769,130,-769,116,-769,115,-769,128,-769,129,-769,126,-769,6,-769,5,-769,120,-769,125,-769,123,-769,121,-769,124,-769,122,-769,137,-769,135,-769,16,-769,9,-769,100,-769,13,-769,2,-769,119,-761},new int[]{-194,25});
    states[719] = new State(new int[]{53,663,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617,18,686},new int[]{-87,433,-359,435,-102,440,-111,704,-101,717,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-358,720,-100,721});
    states[720] = new State(-790);
    states[721] = new State(-983);
    states[722] = new State(-1026);
    states[723] = new State(-1027);
    states[724] = new State(-1010);
    states[725] = new State(-1011);
    states[726] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,727,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[727] = new State(new int[]{51,728});
    states[728] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,729,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[729] = new State(new int[]{32,730,92,-533,10,-533,98,-533,101,-533,33,-533,104,-533,2,-533,9,-533,100,-533,12,-533,99,-533,85,-533,84,-533,83,-533,82,-533,81,-533,86,-533});
    states[730] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,731,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[731] = new State(-534);
    states[732] = new State(-496);
    states[733] = new State(-497);
    states[734] = new State(new int[]{155,736,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-143,735,-147,737,-151,48,-152,51});
    states[735] = new State(-529);
    states[736] = new State(-100);
    states[737] = new State(-101);
    states[738] = new State(-498);
    states[739] = new State(-499);
    states[740] = new State(-500);
    states[741] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,742,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[742] = new State(new int[]{58,743});
    states[743] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163,32,751,92,-553},new int[]{-36,744,-253,1083,-262,1085,-74,1076,-110,1082,-94,1081,-90,192,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[744] = new State(new int[]{10,747,32,751,92,-553},new int[]{-253,745});
    states[745] = new State(new int[]{92,746});
    states[746] = new State(-544);
    states[747] = new State(new int[]{32,751,143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163,92,-553},new int[]{-253,748,-262,750,-74,1076,-110,1082,-94,1081,-90,192,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[748] = new State(new int[]{92,749});
    states[749] = new State(-545);
    states[750] = new State(-548);
    states[751] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,755,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494,92,-494},new int[]{-252,752,-261,753,-260,22,-4,23,-113,24,-132,373,-111,386,-147,754,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888,-143,1043});
    states[752] = new State(new int[]{10,20,92,-554});
    states[753] = new State(-531);
    states[754] = new State(new int[]{8,-793,7,-793,142,-793,4,-793,15,-793,110,-793,111,-793,112,-793,113,-793,114,-793,92,-793,10,-793,11,-793,17,-793,98,-793,101,-793,33,-793,104,-793,2,-793,5,-101});
    states[755] = new State(new int[]{7,-190,11,-190,17,-190,5,-100});
    states[756] = new State(-501);
    states[757] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,755,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,98,-494,10,-494},new int[]{-252,758,-261,753,-260,22,-4,23,-113,24,-132,373,-111,386,-147,754,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888,-143,1043});
    states[758] = new State(new int[]{98,759,10,20});
    states[759] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-87,760,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[760] = new State(-555);
    states[761] = new State(-502);
    states[762] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,763,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[763] = new State(new int[]{99,1072,141,-558,143,-558,85,-558,86,-558,80,-558,78,-558,160,-558,87,-558,45,-558,42,-558,8,-558,21,-558,22,-558,144,-558,147,-558,145,-558,146,-558,155,-558,158,-558,157,-558,156,-558,76,-558,57,-558,91,-558,40,-558,25,-558,97,-558,54,-558,35,-558,55,-558,102,-558,47,-558,36,-558,53,-558,60,-558,74,-558,72,-558,38,-558,92,-558,10,-558,98,-558,101,-558,33,-558,104,-558,2,-558,9,-558,100,-558,12,-558,32,-558,84,-558,83,-558,82,-558,81,-558},new int[]{-291,764});
    states[764] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,765,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[765] = new State(-556);
    states[766] = new State(-503);
    states[767] = new State(new int[]{53,1075,143,-565,85,-565,86,-565,80,-565,78,-565,160,-565,87,-565},new int[]{-19,768});
    states[768] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,769,-151,48,-152,51});
    states[769] = new State(new int[]{5,1025,110,-563},new int[]{-274,770});
    states[770] = new State(new int[]{110,771});
    states[771] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,772,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[772] = new State(new int[]{70,1073,71,1074},new int[]{-119,773});
    states[773] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,774,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[774] = new State(new int[]{160,1068,99,1072,141,-558,143,-558,85,-558,86,-558,80,-558,78,-558,87,-558,45,-558,42,-558,8,-558,21,-558,22,-558,144,-558,147,-558,145,-558,146,-558,155,-558,158,-558,157,-558,156,-558,76,-558,57,-558,91,-558,40,-558,25,-558,97,-558,54,-558,35,-558,55,-558,102,-558,47,-558,36,-558,53,-558,60,-558,74,-558,72,-558,38,-558,92,-558,10,-558,98,-558,101,-558,33,-558,104,-558,2,-558,9,-558,100,-558,12,-558,32,-558,84,-558,83,-558,82,-558,81,-558},new int[]{-291,775});
    states[775] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,776,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[776] = new State(-571);
    states[777] = new State(-504);
    states[778] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,661,8,662,21,270,22,275,76,541,40,608,5,617,18,686,37,695,44,699},new int[]{-71,779,-88,660,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-322,836,-100,671,-323,694});
    states[779] = new State(new int[]{99,780,100,522});
    states[780] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,781,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[781] = new State(-573);
    states[782] = new State(-505);
    states[783] = new State(-506);
    states[784] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,755,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494,101,-494,33,-494},new int[]{-252,785,-261,753,-260,22,-4,23,-113,24,-132,373,-111,386,-147,754,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888,-143,1043});
    states[785] = new State(new int[]{10,20,101,787,33,1046},new int[]{-289,786});
    states[786] = new State(-575);
    states[787] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,755,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494},new int[]{-252,788,-261,753,-260,22,-4,23,-113,24,-132,373,-111,386,-147,754,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888,-143,1043});
    states[788] = new State(new int[]{92,789,10,20});
    states[789] = new State(-576);
    states[790] = new State(-507);
    states[791] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617,92,-590,10,-590,98,-590,101,-590,33,-590,104,-590,2,-590,9,-590,100,-590,12,-590,99,-590,32,-590,84,-590,83,-590,82,-590,81,-590},new int[]{-87,792,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[792] = new State(-591);
    states[793] = new State(-508);
    states[794] = new State(new int[]{53,1027,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,795,-151,48,-152,51});
    states[795] = new State(new int[]{5,1025,137,-563},new int[]{-274,796});
    states[796] = new State(new int[]{137,797});
    states[797] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,798,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[798] = new State(new int[]{87,1023,99,-561},new int[]{-360,799});
    states[799] = new State(new int[]{99,800});
    states[800] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,801,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[801] = new State(-568);
    states[802] = new State(-509);
    states[803] = new State(new int[]{8,805,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-311,804,-158,813,-147,812,-151,48,-152,51});
    states[804] = new State(-519);
    states[805] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,806,-151,48,-152,51});
    states[806] = new State(new int[]{100,807});
    states[807] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-158,808,-147,812,-151,48,-152,51});
    states[808] = new State(new int[]{9,809,100,452});
    states[809] = new State(new int[]{110,810});
    states[810] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-87,811,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[811] = new State(-521);
    states[812] = new State(-343);
    states[813] = new State(new int[]{5,814,100,452,110,1021});
    states[814] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-276,815,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[815] = new State(new int[]{110,1019,120,1020,92,-407,10,-407,98,-407,101,-407,33,-407,104,-407,2,-407,9,-407,100,-407,12,-407,99,-407,32,-407,85,-407,84,-407,83,-407,82,-407,81,-407,86,-407},new int[]{-338,816});
    states[816] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,990,135,843,116,368,115,369,63,163,37,695,44,699,40,608},new int[]{-86,817,-85,818,-84,263,-90,264,-91,225,-81,819,-13,238,-10,248,-14,211,-147,858,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857,-95,1007,-243,1008,-323,1017,-242,1018});
    states[817] = new State(-409);
    states[818] = new State(-410);
    states[819] = new State(new int[]{6,820,116,234,115,235,128,236,129,237,120,-119,125,-119,123,-119,121,-119,124,-119,122,-119,137,-119,13,-119,16,-119,92,-119,10,-119,98,-119,101,-119,33,-119,104,-119,2,-119,9,-119,100,-119,12,-119,99,-119,32,-119,85,-119,84,-119,83,-119,82,-119,81,-119,86,-119},new int[]{-193,201});
    states[820] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-13,821,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851});
    states[821] = new State(new int[]{136,239,138,240,118,241,117,242,131,243,132,244,133,245,134,246,130,247,92,-411,10,-411,98,-411,101,-411,33,-411,104,-411,2,-411,9,-411,100,-411,12,-411,99,-411,32,-411,85,-411,84,-411,83,-411,82,-411,81,-411,86,-411},new int[]{-201,203,-195,208});
    states[822] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-69,823,-77,352,-92,362,-87,355,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[823] = new State(new int[]{76,824});
    states[824] = new State(-166);
    states[825] = new State(-158);
    states[826] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,837,135,843,116,368,115,369,63,163},new int[]{-10,827,-14,828,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,845,-173,847,-57,848});
    states[827] = new State(-159);
    states[828] = new State(new int[]{4,213,11,215,7,829,142,831,8,832,136,-156,138,-156,118,-156,117,-156,131,-156,132,-156,133,-156,134,-156,130,-156,116,-156,115,-156,128,-156,129,-156,120,-156,125,-156,123,-156,121,-156,124,-156,122,-156,137,-156,13,-156,16,-156,6,-156,100,-156,9,-156,12,-156,5,-156,92,-156,10,-156,98,-156,101,-156,33,-156,104,-156,2,-156,99,-156,32,-156,85,-156,84,-156,83,-156,82,-156,81,-156,86,-156},new int[]{-12,212});
    states[829] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148},new int[]{-138,830,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[830] = new State(-178);
    states[831] = new State(-179);
    states[832] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,661,8,662,21,270,22,275,76,541,40,608,5,617,18,686,37,695,44,699,9,-183},new int[]{-76,833,-71,835,-88,660,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-322,836,-100,671,-323,694});
    states[833] = new State(new int[]{9,834});
    states[834] = new State(-180);
    states[835] = new State(new int[]{100,522,9,-182});
    states[836] = new State(-599);
    states[837] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-90,838,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[838] = new State(new int[]{9,839,13,193,16,197});
    states[839] = new State(-160);
    states[840] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-90,841,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[841] = new State(new int[]{9,842,13,193,16,197});
    states[842] = new State(new int[]{136,-160,138,-160,118,-160,117,-160,131,-160,132,-160,133,-160,134,-160,130,-160,116,-160,115,-160,128,-160,129,-160,120,-160,125,-160,123,-160,121,-160,124,-160,122,-160,137,-160,13,-160,16,-160,6,-160,100,-160,9,-160,12,-160,5,-160,92,-160,10,-160,98,-160,101,-160,33,-160,104,-160,2,-160,99,-160,32,-160,85,-160,84,-160,83,-160,82,-160,81,-160,86,-160,119,-155});
    states[843] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,837,135,843,116,368,115,369,63,163},new int[]{-10,844,-14,828,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,845,-173,847,-57,848});
    states[844] = new State(-161);
    states[845] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,837,135,843,116,368,115,369,63,163},new int[]{-10,846,-14,828,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,845,-173,847,-57,848});
    states[846] = new State(-162);
    states[847] = new State(-163);
    states[848] = new State(-164);
    states[849] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-10,846,-269,850,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-11,851});
    states[850] = new State(-141);
    states[851] = new State(new int[]{119,852});
    states[852] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-10,853,-269,854,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-11,851});
    states[853] = new State(-139);
    states[854] = new State(-140);
    states[855] = new State(-143);
    states[856] = new State(-144);
    states[857] = new State(-122);
    states[858] = new State(new int[]{127,859,4,-169,11,-169,7,-169,142,-169,8,-169,136,-169,138,-169,118,-169,117,-169,131,-169,132,-169,133,-169,134,-169,130,-169,6,-169,116,-169,115,-169,128,-169,129,-169,120,-169,125,-169,123,-169,121,-169,124,-169,122,-169,137,-169,13,-169,16,-169,92,-169,10,-169,98,-169,101,-169,33,-169,104,-169,2,-169,9,-169,100,-169,12,-169,99,-169,32,-169,85,-169,84,-169,83,-169,82,-169,81,-169,86,-169,119,-169});
    states[859] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,18,686,37,695,44,699,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-328,860,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,669,-117,600,-322,670,-100,671,-323,694,-330,861,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[860] = new State(-413);
    states[861] = new State(-1024);
    states[862] = new State(-1012);
    states[863] = new State(-1013);
    states[864] = new State(-1014);
    states[865] = new State(-1015);
    states[866] = new State(-1016);
    states[867] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,868,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[868] = new State(new int[]{99,869});
    states[869] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,870,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[870] = new State(-516);
    states[871] = new State(-510);
    states[872] = new State(-596);
    states[873] = new State(-597);
    states[874] = new State(-511);
    states[875] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,876,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[876] = new State(new int[]{99,877});
    states[877] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,878,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[878] = new State(-559);
    states[879] = new State(-512);
    states[880] = new State(new int[]{73,882,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,662,21,270,22,275,76,541,40,608,18,686,37,695,44,699},new int[]{-103,881,-101,884,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-322,885,-100,671,-323,694});
    states[881] = new State(-517);
    states[882] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,662,21,270,22,275,76,541,40,608,18,686,37,695,44,699},new int[]{-103,883,-101,884,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-322,885,-100,671,-323,694});
    states[883] = new State(-518);
    states[884] = new State(-614);
    states[885] = new State(-615);
    states[886] = new State(-513);
    states[887] = new State(-514);
    states[888] = new State(-515);
    states[889] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,890,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[890] = new State(new int[]{55,891});
    states[891] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,56,969,21,270,22,275,11,929,8,942},new int[]{-350,892,-349,983,-342,899,-283,904,-180,205,-147,207,-151,48,-152,51,-341,961,-357,964,-339,972,-15,967,-165,149,-167,150,-166,154,-16,157,-257,970,-294,971,-343,973,-344,976});
    states[892] = new State(new int[]{10,895,32,751,92,-553},new int[]{-253,893});
    states[893] = new State(new int[]{92,894});
    states[894] = new State(-535);
    states[895] = new State(new int[]{32,751,143,47,85,49,86,50,80,52,78,53,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,56,969,21,270,22,275,11,929,8,942,92,-553},new int[]{-253,896,-349,898,-342,899,-283,904,-180,205,-147,207,-151,48,-152,51,-341,961,-357,964,-339,972,-15,967,-165,149,-167,150,-166,154,-16,157,-257,970,-294,971,-343,973,-344,976});
    states[896] = new State(new int[]{92,897});
    states[897] = new State(-536);
    states[898] = new State(-538);
    states[899] = new State(new int[]{39,900});
    states[900] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,901,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[901] = new State(new int[]{5,902});
    states[902] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494,32,-494,92,-494},new int[]{-260,903,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[903] = new State(-539);
    states[904] = new State(new int[]{8,905,100,-655,5,-655});
    states[905] = new State(new int[]{14,910,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,116,368,115,369,143,47,85,49,86,50,80,52,78,53,160,54,87,55,53,917,11,929,8,942},new int[]{-354,906,-352,960,-15,911,-165,149,-167,150,-166,154,-16,157,-199,912,-147,914,-151,48,-152,51,-342,921,-283,922,-180,205,-343,928,-344,959});
    states[906] = new State(new int[]{9,907,10,908,100,926});
    states[907] = new State(new int[]{39,-649,5,-650});
    states[908] = new State(new int[]{14,910,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,116,368,115,369,143,47,85,49,86,50,80,52,78,53,160,54,87,55,53,917,11,929,8,942},new int[]{-352,909,-15,911,-165,149,-167,150,-166,154,-16,157,-199,912,-147,914,-151,48,-152,51,-342,921,-283,922,-180,205,-343,928,-344,959});
    states[909] = new State(-681);
    states[910] = new State(-693);
    states[911] = new State(-694);
    states[912] = new State(new int[]{144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161},new int[]{-15,913,-165,149,-167,150,-166,154,-16,157});
    states[913] = new State(-695);
    states[914] = new State(new int[]{5,915,9,-697,10,-697,100,-697,7,-260,4,-260,123,-260,8,-260});
    states[915] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-276,916,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[916] = new State(-696);
    states[917] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,918,-151,48,-152,51});
    states[918] = new State(new int[]{5,919,9,-699,10,-699,100,-699});
    states[919] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-276,920,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[920] = new State(-698);
    states[921] = new State(-700);
    states[922] = new State(new int[]{8,923});
    states[923] = new State(new int[]{14,910,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,116,368,115,369,143,47,85,49,86,50,80,52,78,53,160,54,87,55,53,917,11,929,8,942},new int[]{-354,924,-352,960,-15,911,-165,149,-167,150,-166,154,-16,157,-199,912,-147,914,-151,48,-152,51,-342,921,-283,922,-180,205,-343,928,-344,959});
    states[924] = new State(new int[]{9,925,10,908,100,926});
    states[925] = new State(-649);
    states[926] = new State(new int[]{14,910,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,116,368,115,369,143,47,85,49,86,50,80,52,78,53,160,54,87,55,53,917,11,929,8,942},new int[]{-352,927,-15,911,-165,149,-167,150,-166,154,-16,157,-199,912,-147,914,-151,48,-152,51,-342,921,-283,922,-180,205,-343,928,-344,959});
    states[927] = new State(-682);
    states[928] = new State(-701);
    states[929] = new State(new int[]{144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,53,936,14,938,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,929,8,942,6,957},new int[]{-355,930,-345,958,-15,934,-165,149,-167,150,-166,154,-16,157,-347,935,-342,939,-283,922,-180,205,-147,207,-151,48,-152,51,-343,940,-344,941});
    states[930] = new State(new int[]{12,931,100,932});
    states[931] = new State(-659);
    states[932] = new State(new int[]{144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,53,936,14,938,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,929,8,942,6,957},new int[]{-345,933,-15,934,-165,149,-167,150,-166,154,-16,157,-347,935,-342,939,-283,922,-180,205,-147,207,-151,48,-152,51,-343,940,-344,941});
    states[933] = new State(-661);
    states[934] = new State(-662);
    states[935] = new State(-663);
    states[936] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,937,-151,48,-152,51});
    states[937] = new State(-669);
    states[938] = new State(-664);
    states[939] = new State(-665);
    states[940] = new State(-666);
    states[941] = new State(-667);
    states[942] = new State(new int[]{14,947,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,116,368,115,369,53,951,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,929,8,942},new int[]{-356,943,-346,956,-15,948,-165,149,-167,150,-166,154,-16,157,-199,949,-342,953,-283,922,-180,205,-147,207,-151,48,-152,51,-343,954,-344,955});
    states[943] = new State(new int[]{9,944,100,945});
    states[944] = new State(-670);
    states[945] = new State(new int[]{14,947,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,116,368,115,369,53,951,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,929,8,942},new int[]{-346,946,-15,948,-165,149,-167,150,-166,154,-16,157,-199,949,-342,953,-283,922,-180,205,-147,207,-151,48,-152,51,-343,954,-344,955});
    states[946] = new State(-679);
    states[947] = new State(-671);
    states[948] = new State(-672);
    states[949] = new State(new int[]{144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161},new int[]{-15,950,-165,149,-167,150,-166,154,-16,157});
    states[950] = new State(-673);
    states[951] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,952,-151,48,-152,51});
    states[952] = new State(-674);
    states[953] = new State(-675);
    states[954] = new State(-676);
    states[955] = new State(-677);
    states[956] = new State(-678);
    states[957] = new State(-668);
    states[958] = new State(-660);
    states[959] = new State(-702);
    states[960] = new State(-680);
    states[961] = new State(new int[]{5,962});
    states[962] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494,32,-494,92,-494},new int[]{-260,963,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[963] = new State(-540);
    states[964] = new State(new int[]{100,965,5,-651});
    states[965] = new State(new int[]{144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,143,47,85,49,86,50,80,52,78,53,160,54,87,55,56,969,21,270,22,275},new int[]{-339,966,-15,967,-165,149,-167,150,-166,154,-16,157,-283,968,-180,205,-147,207,-151,48,-152,51,-257,970,-294,971});
    states[966] = new State(-653);
    states[967] = new State(-654);
    states[968] = new State(-655);
    states[969] = new State(-656);
    states[970] = new State(-657);
    states[971] = new State(-658);
    states[972] = new State(-652);
    states[973] = new State(new int[]{5,974});
    states[974] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494,32,-494,92,-494},new int[]{-260,975,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[975] = new State(-541);
    states[976] = new State(new int[]{39,977,5,981});
    states[977] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,978,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[978] = new State(new int[]{5,979});
    states[979] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494,32,-494,92,-494},new int[]{-260,980,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[980] = new State(-542);
    states[981] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494,32,-494,92,-494},new int[]{-260,982,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[982] = new State(-543);
    states[983] = new State(-537);
    states[984] = new State(-1017);
    states[985] = new State(-1018);
    states[986] = new State(-1019);
    states[987] = new State(-1020);
    states[988] = new State(-1021);
    states[989] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,662,21,270,22,275,76,541,40,608,18,686,37,695,44,699},new int[]{-103,881,-101,884,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-322,885,-100,671,-323,694});
    states[990] = new State(new int[]{9,998,143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,1003,135,843,116,368,115,369,63,163},new int[]{-90,991,-66,992,-245,996,-91,225,-81,233,-13,238,-10,248,-14,211,-147,1002,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857,-65,260,-85,1006,-84,263,-95,1007,-243,1008,-244,1009,-246,1016,-136,1012});
    states[991] = new State(new int[]{9,842,13,193,16,197,100,-194});
    states[992] = new State(new int[]{9,993});
    states[993] = new State(new int[]{127,994,92,-197,10,-197,98,-197,101,-197,33,-197,104,-197,2,-197,9,-197,100,-197,12,-197,99,-197,32,-197,85,-197,84,-197,83,-197,82,-197,81,-197,86,-197});
    states[994] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,18,686,37,695,44,699,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-328,995,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,669,-117,600,-322,670,-100,671,-323,694,-330,861,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[995] = new State(-415);
    states[996] = new State(new int[]{9,997});
    states[997] = new State(-202);
    states[998] = new State(new int[]{5,454,127,-1006},new int[]{-324,999});
    states[999] = new State(new int[]{127,1000});
    states[1000] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,18,686,37,695,44,699,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-328,1001,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,669,-117,600,-322,670,-100,671,-323,694,-330,861,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[1001] = new State(-414);
    states[1002] = new State(new int[]{4,-169,11,-169,7,-169,142,-169,8,-169,136,-169,138,-169,118,-169,117,-169,131,-169,132,-169,133,-169,134,-169,130,-169,116,-169,115,-169,128,-169,129,-169,120,-169,125,-169,123,-169,121,-169,124,-169,122,-169,137,-169,9,-169,13,-169,16,-169,100,-169,119,-169,5,-208});
    states[1003] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,1003,135,843,116,368,115,369,63,163,9,-198},new int[]{-90,991,-66,1004,-245,996,-91,225,-81,233,-13,238,-10,248,-14,211,-147,1002,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857,-65,260,-85,1006,-84,263,-95,1007,-243,1008,-244,1009,-246,1016,-136,1012});
    states[1004] = new State(new int[]{9,1005});
    states[1005] = new State(-197);
    states[1006] = new State(-200);
    states[1007] = new State(-195);
    states[1008] = new State(-196);
    states[1009] = new State(new int[]{10,1010,9,-203});
    states[1010] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,9,-204},new int[]{-246,1011,-136,1012,-147,1015,-151,48,-152,51});
    states[1011] = new State(-206);
    states[1012] = new State(new int[]{5,1013});
    states[1013] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,1003,135,843,116,368,115,369,63,163},new int[]{-84,1014,-90,264,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857,-95,1007,-243,1008});
    states[1014] = new State(-207);
    states[1015] = new State(-208);
    states[1016] = new State(-205);
    states[1017] = new State(-412);
    states[1018] = new State(-416);
    states[1019] = new State(-405);
    states[1020] = new State(-406);
    states[1021] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,661,8,662,21,270,22,275,76,541,40,608,5,617,18,686,37,695,44,699},new int[]{-88,1022,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-322,836,-100,671,-323,694});
    states[1022] = new State(-408);
    states[1023] = new State(new int[]{143,1024});
    states[1024] = new State(-560);
    states[1025] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-276,1026,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1026] = new State(-562);
    states[1027] = new State(new int[]{8,1035,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,1028,-151,48,-152,51});
    states[1028] = new State(new int[]{5,1025,137,-563},new int[]{-274,1029});
    states[1029] = new State(new int[]{137,1030});
    states[1030] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,1031,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[1031] = new State(new int[]{87,1023,99,-561},new int[]{-360,1032});
    states[1032] = new State(new int[]{99,1033});
    states[1033] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,1034,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[1034] = new State(-569);
    states[1035] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-158,1036,-147,812,-151,48,-152,51});
    states[1036] = new State(new int[]{9,1037,100,452});
    states[1037] = new State(new int[]{137,1038});
    states[1038] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,1039,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[1039] = new State(new int[]{87,1023,99,-561},new int[]{-360,1040});
    states[1040] = new State(new int[]{99,1041});
    states[1041] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,1042,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[1042] = new State(-570);
    states[1043] = new State(new int[]{5,1044});
    states[1044] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,755,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494},new int[]{-261,1045,-260,22,-4,23,-113,24,-132,373,-111,386,-147,754,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888,-143,1043});
    states[1045] = new State(-493);
    states[1046] = new State(new int[]{79,1054,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,755,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494,92,-494},new int[]{-60,1047,-63,1049,-62,1066,-252,1067,-261,753,-260,22,-4,23,-113,24,-132,373,-111,386,-147,754,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888,-143,1043});
    states[1047] = new State(new int[]{92,1048});
    states[1048] = new State(-577);
    states[1049] = new State(new int[]{10,1051,32,1064,92,-583},new int[]{-254,1050});
    states[1050] = new State(-578);
    states[1051] = new State(new int[]{79,1054,32,1064,92,-583},new int[]{-62,1052,-254,1053});
    states[1052] = new State(-582);
    states[1053] = new State(-579);
    states[1054] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-64,1055,-179,1058,-180,1059,-147,1060,-151,48,-152,51,-140,1061});
    states[1055] = new State(new int[]{99,1056});
    states[1056] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494,32,-494,92,-494},new int[]{-260,1057,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[1057] = new State(-585);
    states[1058] = new State(-586);
    states[1059] = new State(new int[]{7,169,99,-588});
    states[1060] = new State(new int[]{7,-260,99,-260,5,-589});
    states[1061] = new State(new int[]{5,1062});
    states[1062] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-179,1063,-180,1059,-147,207,-151,48,-152,51});
    states[1063] = new State(-587);
    states[1064] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,755,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494,92,-494},new int[]{-252,1065,-261,753,-260,22,-4,23,-113,24,-132,373,-111,386,-147,754,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888,-143,1043});
    states[1065] = new State(new int[]{10,20,92,-584});
    states[1066] = new State(-581);
    states[1067] = new State(new int[]{10,20,92,-580});
    states[1068] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,1069,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[1069] = new State(new int[]{99,1070});
    states[1070] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,100,-494,12,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,1071,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[1071] = new State(-572);
    states[1072] = new State(-557);
    states[1073] = new State(-566);
    states[1074] = new State(-567);
    states[1075] = new State(-564);
    states[1076] = new State(new int[]{5,1077,100,1079});
    states[1077] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494,32,-494,92,-494},new int[]{-260,1078,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[1078] = new State(-549);
    states[1079] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-110,1080,-94,1081,-90,192,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[1080] = new State(-551);
    states[1081] = new State(-552);
    states[1082] = new State(-550);
    states[1083] = new State(new int[]{92,1084});
    states[1084] = new State(-546);
    states[1085] = new State(-547);
    states[1086] = new State(new int[]{9,1087,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-326,1090,-327,1094,-158,450,-147,812,-151,48,-152,51});
    states[1087] = new State(new int[]{127,1088});
    states[1088] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,703,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-329,1089,-212,702,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-4,722,-330,723,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[1089] = new State(-1001);
    states[1090] = new State(new int[]{9,1091,10,448});
    states[1091] = new State(new int[]{127,1092});
    states[1092] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,42,430,8,703,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-329,1093,-212,702,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-4,722,-330,723,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[1093] = new State(-1002);
    states[1094] = new State(-1003);
    states[1095] = new State(new int[]{9,1096,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-326,1100,-327,1094,-158,450,-147,812,-151,48,-152,51});
    states[1096] = new State(new int[]{5,673,127,-1008},new int[]{-325,1097});
    states[1097] = new State(new int[]{127,1098});
    states[1098] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,18,686,37,695,44,699,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-328,1099,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,669,-117,600,-322,670,-100,671,-323,694,-330,861,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[1099] = new State(-998);
    states[1100] = new State(new int[]{9,1101,10,448});
    states[1101] = new State(new int[]{5,673,127,-1008},new int[]{-325,1102});
    states[1102] = new State(new int[]{127,1103});
    states[1103] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,18,686,37,695,44,699,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-328,1104,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,669,-117,600,-322,670,-100,671,-323,694,-330,861,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[1104] = new State(-999);
    states[1105] = new State(new int[]{5,1106,10,1118,8,-793,7,-793,142,-793,4,-793,15,-793,110,-793,111,-793,112,-793,113,-793,114,-793,138,-793,136,-793,118,-793,117,-793,131,-793,132,-793,133,-793,134,-793,130,-793,116,-793,115,-793,128,-793,129,-793,126,-793,6,-793,120,-793,125,-793,123,-793,121,-793,124,-793,122,-793,137,-793,135,-793,16,-793,9,-793,100,-793,13,-793,119,-793,11,-793,17,-793});
    states[1106] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-275,1107,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1107] = new State(new int[]{9,1108,10,1112});
    states[1108] = new State(new int[]{5,673,127,-1008},new int[]{-325,1109});
    states[1109] = new State(new int[]{127,1110});
    states[1110] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,18,686,37,695,44,699,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-328,1111,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,669,-117,600,-322,670,-100,671,-323,694,-330,861,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[1111] = new State(-988);
    states[1112] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-326,1113,-327,1094,-158,450,-147,812,-151,48,-152,51});
    states[1113] = new State(new int[]{9,1114,10,448});
    states[1114] = new State(new int[]{5,673,127,-1008},new int[]{-325,1115});
    states[1115] = new State(new int[]{127,1116});
    states[1116] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,18,686,37,695,44,699,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-328,1117,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,669,-117,600,-322,670,-100,671,-323,694,-330,861,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[1117] = new State(-990);
    states[1118] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-326,1119,-327,1094,-158,450,-147,812,-151,48,-152,51});
    states[1119] = new State(new int[]{9,1120,10,448});
    states[1120] = new State(new int[]{5,673,127,-1008},new int[]{-325,1121});
    states[1121] = new State(new int[]{127,1122});
    states[1122] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,530,21,270,22,275,76,541,18,686,37,695,44,699,91,17,40,726,54,762,97,757,35,767,36,794,72,867,25,741,102,784,60,875,47,791,74,989},new int[]{-328,1123,-104,527,-99,528,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,669,-117,600,-322,670,-100,671,-323,694,-330,861,-255,724,-153,725,-318,862,-247,863,-124,864,-123,865,-125,866,-35,984,-301,985,-169,986,-248,987,-126,988});
    states[1123] = new State(-989);
    states[1124] = new State(new int[]{5,1125,7,-260,8,-260,123,-260,12,-260,100,-260});
    states[1125] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-9,1126,-180,653,-147,207,-151,48,-152,51,-300,1127});
    states[1126] = new State(-217);
    states[1127] = new State(new int[]{8,656,12,-642,100,-642},new int[]{-70,1128});
    states[1128] = new State(-782);
    states[1129] = new State(-214);
    states[1130] = new State(-210);
    states[1131] = new State(-473);
    states[1132] = new State(-690);
    states[1133] = new State(new int[]{8,1134});
    states[1134] = new State(new int[]{14,562,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,53,564,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,929,8,942},new int[]{-353,1135,-351,1141,-15,563,-165,149,-167,150,-166,154,-16,157,-340,1132,-283,1133,-180,205,-147,207,-151,48,-152,51,-343,1139,-344,1140});
    states[1135] = new State(new int[]{9,1136,10,560,100,1137});
    states[1136] = new State(-648);
    states[1137] = new State(new int[]{14,562,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,53,564,143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,929,8,942},new int[]{-351,1138,-15,563,-165,149,-167,150,-166,154,-16,157,-340,1132,-283,1133,-180,205,-147,207,-151,48,-152,51,-343,1139,-344,1140});
    states[1138] = new State(-685);
    states[1139] = new State(-691);
    states[1140] = new State(-692);
    states[1141] = new State(-683);
    states[1142] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,18,686},new int[]{-102,1143,-101,1144,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-100,721});
    states[1143] = new State(-985);
    states[1144] = new State(-982);
    states[1145] = new State(-984);
    states[1146] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,5,617},new int[]{-120,1147,-105,1149,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,621,-267,598});
    states[1147] = new State(new int[]{12,1148});
    states[1148] = new State(-802);
    states[1149] = new State(new int[]{5,322,6,34});
    states[1150] = new State(new int[]{53,1158},new int[]{-337,1151});
    states[1151] = new State(new int[]{9,1152,100,1155});
    states[1152] = new State(new int[]{110,1153});
    states[1153] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-87,1154,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[1154] = new State(-520);
    states[1155] = new State(new int[]{53,1156});
    states[1156] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,1157,-151,48,-152,51});
    states[1157] = new State(-527);
    states[1158] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,1159,-151,48,-152,51});
    states[1159] = new State(-526);
    states[1160] = new State(new int[]{148,1164,150,1165,151,1166,152,1167,154,1168,153,1169,107,-827,91,-827,59,-827,29,-827,66,-827,50,-827,53,-827,62,-827,11,-827,28,-827,26,-827,44,-827,37,-827,19,-827,30,-827,31,-827,46,-827,27,-827,92,-827,84,-827,83,-827,82,-827,81,-827,23,-827,149,-827,41,-827},new int[]{-206,1161,-209,1170});
    states[1161] = new State(new int[]{10,1162});
    states[1162] = new State(new int[]{148,1164,150,1165,151,1166,152,1167,154,1168,153,1169,107,-828,91,-828,59,-828,29,-828,66,-828,50,-828,53,-828,62,-828,11,-828,28,-828,26,-828,44,-828,37,-828,19,-828,30,-828,31,-828,46,-828,27,-828,92,-828,84,-828,83,-828,82,-828,81,-828,23,-828,149,-828,41,-828},new int[]{-209,1163});
    states[1163] = new State(-832);
    states[1164] = new State(-844);
    states[1165] = new State(-845);
    states[1166] = new State(-846);
    states[1167] = new State(-847);
    states[1168] = new State(-848);
    states[1169] = new State(-849);
    states[1170] = new State(-831);
    states[1171] = new State(-376);
    states[1172] = new State(-447);
    states[1173] = new State(-448);
    states[1174] = new State(new int[]{8,-453,110,-453,10,-453,11,-453,5,-453,7,-450});
    states[1175] = new State(new int[]{123,1177,8,-456,110,-456,10,-456,7,-456,11,-456,5,-456},new int[]{-155,1176});
    states[1176] = new State(-457);
    states[1177] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-158,1178,-147,812,-151,48,-152,51});
    states[1178] = new State(new int[]{121,1179,100,452});
    states[1179] = new State(-321);
    states[1180] = new State(-458);
    states[1181] = new State(new int[]{123,1177,8,-454,110,-454,10,-454,11,-454,5,-454},new int[]{-155,1182});
    states[1182] = new State(-455);
    states[1183] = new State(new int[]{7,1184});
    states[1184] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-142,1185,-149,1186,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181});
    states[1185] = new State(-449);
    states[1186] = new State(-452);
    states[1187] = new State(-451);
    states[1188] = new State(-438);
    states[1189] = new State(new int[]{44,1319,37,1354},new int[]{-216,1190,-228,1191,-225,1318,-229,1353});
    states[1190] = new State(-440);
    states[1191] = new State(new int[]{107,1309,59,-71,29,-71,66,-71,50,-71,53,-71,62,-71,91,-71},new int[]{-176,1192,-43,1193,-39,1196,-61,1308});
    states[1192] = new State(-441);
    states[1193] = new State(new int[]{91,17},new int[]{-255,1194});
    states[1194] = new State(new int[]{10,1195});
    states[1195] = new State(-468);
    states[1196] = new State(new int[]{59,1199,29,1220,66,1224,50,1436,53,1451,62,1453,91,-70},new int[]{-46,1197,-168,1198,-29,1205,-52,1222,-288,1226,-309,1438});
    states[1197] = new State(-72);
    states[1198] = new State(-88);
    states[1199] = new State(new int[]{155,736,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-156,1200,-143,1204,-147,737,-151,48,-152,51});
    states[1200] = new State(new int[]{10,1201,100,1202});
    states[1201] = new State(-97);
    states[1202] = new State(new int[]{155,736,143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-143,1203,-147,737,-151,48,-152,51});
    states[1203] = new State(-99);
    states[1204] = new State(-98);
    states[1205] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,59,-89,29,-89,66,-89,50,-89,53,-89,62,-89,91,-89},new int[]{-27,1206,-28,1207,-141,1209,-147,1219,-151,48,-152,51});
    states[1206] = new State(-103);
    states[1207] = new State(new int[]{10,1208});
    states[1208] = new State(-113);
    states[1209] = new State(new int[]{120,1210,5,1215});
    states[1210] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,1213,135,843,116,368,115,369,63,163},new int[]{-109,1211,-90,1212,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857,-95,1214});
    states[1211] = new State(-114);
    states[1212] = new State(new int[]{13,193,16,197,10,-116,92,-116,84,-116,83,-116,82,-116,81,-116});
    states[1213] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,1003,135,843,116,368,115,369,63,163,9,-198},new int[]{-90,991,-66,1004,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857,-65,260,-85,1006,-84,263,-95,1007,-243,1008});
    states[1214] = new State(-117);
    states[1215] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-276,1216,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1216] = new State(new int[]{120,1217});
    states[1217] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,1003,135,843,116,368,115,369,63,163},new int[]{-84,1218,-90,264,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857,-95,1007,-243,1008});
    states[1218] = new State(-115);
    states[1219] = new State(-118);
    states[1220] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-27,1221,-28,1207,-141,1209,-147,1219,-151,48,-152,51});
    states[1221] = new State(-102);
    states[1222] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,59,-90,29,-90,66,-90,50,-90,53,-90,62,-90,91,-90},new int[]{-27,1223,-28,1207,-141,1209,-147,1219,-151,48,-152,51});
    states[1223] = new State(-105);
    states[1224] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-27,1225,-28,1207,-141,1209,-147,1219,-151,48,-152,51});
    states[1225] = new State(-104);
    states[1226] = new State(new int[]{11,647,59,-91,29,-91,66,-91,50,-91,53,-91,62,-91,91,-91,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212},new int[]{-49,1227,-6,1228,-250,1130});
    states[1227] = new State(-107);
    states[1228] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,11,647},new int[]{-50,1229,-250,495,-144,1230,-147,1428,-151,48,-152,51,-145,1433});
    states[1229] = new State(-209);
    states[1230] = new State(new int[]{120,1231});
    states[1231] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623,68,1422,69,1423,148,1424,27,1425,28,1426,26,-303,43,-303,64,-303},new int[]{-286,1232,-276,1234,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582,-30,1235,-21,1236,-22,1420,-20,1427});
    states[1232] = new State(new int[]{10,1233});
    states[1233] = new State(-218);
    states[1234] = new State(-223);
    states[1235] = new State(-224);
    states[1236] = new State(new int[]{26,1414,43,1415,64,1416},new int[]{-290,1237});
    states[1237] = new State(new int[]{8,1335,23,-315,11,-315,92,-315,84,-315,83,-315,82,-315,81,-315,29,-315,143,-315,85,-315,86,-315,80,-315,78,-315,160,-315,87,-315,62,-315,28,-315,26,-315,44,-315,37,-315,19,-315,30,-315,31,-315,46,-315,27,-315,10,-315},new int[]{-183,1238});
    states[1238] = new State(new int[]{23,1326,11,-322,92,-322,84,-322,83,-322,82,-322,81,-322,29,-322,143,-322,85,-322,86,-322,80,-322,78,-322,160,-322,87,-322,62,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322,10,-322},new int[]{-317,1239,-316,1324,-315,1346});
    states[1239] = new State(new int[]{11,647,10,-313,92,-339,84,-339,83,-339,82,-339,81,-339,29,-212,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,62,-212,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-26,1240,-25,1241,-32,1247,-34,486,-45,1248,-6,1249,-250,1130,-33,1411,-54,1413,-53,492,-55,1412});
    states[1240] = new State(-296);
    states[1241] = new State(new int[]{92,1242,84,1243,83,1244,82,1245,81,1246},new int[]{-7,484});
    states[1242] = new State(-314);
    states[1243] = new State(-335);
    states[1244] = new State(-336);
    states[1245] = new State(-337);
    states[1246] = new State(-338);
    states[1247] = new State(-333);
    states[1248] = new State(-347);
    states[1249] = new State(new int[]{29,1251,143,47,85,49,86,50,80,52,78,53,160,54,87,55,62,1255,28,1371,26,1372,11,647,44,1319,37,1354,19,1374,30,1382,31,1389,46,1396,27,1405},new int[]{-51,1250,-250,495,-222,494,-219,496,-258,497,-312,1253,-311,1254,-158,813,-147,812,-151,48,-152,51,-3,1259,-230,1373,-228,1307,-225,1318,-229,1353,-227,1380,-215,1393,-216,1394,-218,1395});
    states[1250] = new State(-349);
    states[1251] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-28,1252,-141,1209,-147,1219,-151,48,-152,51});
    states[1252] = new State(-354);
    states[1253] = new State(-355);
    states[1254] = new State(-359);
    states[1255] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-158,1256,-147,812,-151,48,-152,51});
    states[1256] = new State(new int[]{5,1257,100,452});
    states[1257] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-276,1258,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1258] = new State(-360);
    states[1259] = new State(new int[]{30,500,19,1189,46,1261,27,1299,143,47,85,49,86,50,80,52,78,53,160,54,87,55,62,1255,44,1319,37,1354},new int[]{-312,1260,-230,499,-216,1188,-311,1254,-158,813,-147,812,-151,48,-152,51,-228,1307,-225,1318,-229,1353});
    states[1260] = new State(-356);
    states[1261] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-170,1262,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[1262] = new State(new int[]{11,1290,5,-390},new int[]{-233,1263,-238,1287});
    states[1263] = new State(new int[]{85,1276,86,1282,10,-397},new int[]{-202,1264});
    states[1264] = new State(new int[]{10,1265});
    states[1265] = new State(new int[]{63,1270,153,1272,152,1273,148,1274,151,1275,11,-387,28,-387,26,-387,44,-387,37,-387,19,-387,30,-387,31,-387,46,-387,27,-387,92,-387,84,-387,83,-387,82,-387,81,-387},new int[]{-205,1266,-210,1267});
    states[1266] = new State(-381);
    states[1267] = new State(new int[]{10,1268});
    states[1268] = new State(new int[]{63,1270,11,-387,28,-387,26,-387,44,-387,37,-387,19,-387,30,-387,31,-387,46,-387,27,-387,92,-387,84,-387,83,-387,82,-387,81,-387},new int[]{-205,1269});
    states[1269] = new State(-382);
    states[1270] = new State(new int[]{10,1271});
    states[1271] = new State(-388);
    states[1272] = new State(-850);
    states[1273] = new State(-851);
    states[1274] = new State(-852);
    states[1275] = new State(-853);
    states[1276] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,661,8,662,21,270,22,275,76,541,40,608,5,617,18,686,37,695,44,699,10,-396},new int[]{-114,1277,-88,1281,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-322,836,-100,671,-323,694});
    states[1277] = new State(new int[]{86,1279,10,-400},new int[]{-203,1278});
    states[1278] = new State(-398);
    states[1279] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494},new int[]{-260,1280,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[1280] = new State(-401);
    states[1281] = new State(-395);
    states[1282] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494},new int[]{-260,1283,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[1283] = new State(new int[]{85,1285,10,-402},new int[]{-204,1284});
    states[1284] = new State(-399);
    states[1285] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,661,8,662,21,270,22,275,76,541,40,608,5,617,18,686,37,695,44,699,10,-396},new int[]{-114,1286,-88,1281,-87,27,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-322,836,-100,671,-323,694});
    states[1286] = new State(-403);
    states[1287] = new State(new int[]{5,1288});
    states[1288] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-275,1289,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1289] = new State(-389);
    states[1290] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-237,1291,-236,1298,-158,1295,-147,812,-151,48,-152,51});
    states[1291] = new State(new int[]{12,1292,10,1293});
    states[1292] = new State(-391);
    states[1293] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-236,1294,-158,1295,-147,812,-151,48,-152,51});
    states[1294] = new State(-393);
    states[1295] = new State(new int[]{5,1296,100,452});
    states[1296] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-275,1297,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1297] = new State(-394);
    states[1298] = new State(-392);
    states[1299] = new State(new int[]{46,1300});
    states[1300] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-170,1301,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[1301] = new State(new int[]{11,1290,5,-390},new int[]{-233,1302,-238,1287});
    states[1302] = new State(new int[]{110,1305,10,-386},new int[]{-211,1303});
    states[1303] = new State(new int[]{10,1304});
    states[1304] = new State(-384);
    states[1305] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-87,1306,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[1306] = new State(-385);
    states[1307] = new State(new int[]{107,1309,11,-370,28,-370,26,-370,44,-370,37,-370,19,-370,30,-370,31,-370,46,-370,27,-370,92,-370,84,-370,83,-370,82,-370,81,-370,59,-71,29,-71,66,-71,50,-71,53,-71,62,-71,91,-71},new int[]{-176,1192,-43,1193,-39,1196,-61,1308});
    states[1308] = new State(-469);
    states[1309] = new State(new int[]{10,1317,143,47,85,49,86,50,80,52,78,53,160,54,87,55,144,152,147,153,145,155,146,156},new int[]{-108,1310,-147,1314,-151,48,-152,51,-165,1315,-167,150,-166,154});
    states[1310] = new State(new int[]{80,1311,10,1316});
    states[1311] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,144,152,147,153,145,155,146,156},new int[]{-108,1312,-147,1314,-151,48,-152,51,-165,1315,-167,150,-166,154});
    states[1312] = new State(new int[]{10,1313});
    states[1313] = new State(-462);
    states[1314] = new State(-465);
    states[1315] = new State(-466);
    states[1316] = new State(-463);
    states[1317] = new State(-464);
    states[1318] = new State(-371);
    states[1319] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-171,1320,-170,1172,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[1320] = new State(new int[]{8,585,10,-470,110,-470},new int[]{-128,1321});
    states[1321] = new State(new int[]{10,1351,110,-829},new int[]{-207,1322,-208,1347});
    states[1322] = new State(new int[]{23,1326,107,-322,91,-322,59,-322,29,-322,66,-322,50,-322,53,-322,62,-322,11,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322,92,-322,84,-322,83,-322,82,-322,81,-322,149,-322,41,-322},new int[]{-317,1323,-316,1324,-315,1346});
    states[1323] = new State(-459);
    states[1324] = new State(new int[]{23,1326,11,-323,92,-323,84,-323,83,-323,82,-323,81,-323,29,-323,143,-323,85,-323,86,-323,80,-323,78,-323,160,-323,87,-323,62,-323,28,-323,26,-323,44,-323,37,-323,19,-323,30,-323,31,-323,46,-323,27,-323,10,-323,107,-323,91,-323,59,-323,66,-323,50,-323,53,-323,149,-323,41,-323},new int[]{-315,1325});
    states[1325] = new State(-325);
    states[1326] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-158,1327,-147,812,-151,48,-152,51});
    states[1327] = new State(new int[]{5,1328,100,452});
    states[1328] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,1334,49,569,34,573,73,577,44,583,37,623,26,1343,30,1344},new int[]{-287,1329,-285,1345,-276,1333,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1329] = new State(new int[]{10,1330,100,1331});
    states[1330] = new State(-326);
    states[1331] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,1334,49,569,34,573,73,577,44,583,37,623,26,1343,30,1344},new int[]{-285,1332,-276,1333,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1332] = new State(-328);
    states[1333] = new State(-329);
    states[1334] = new State(new int[]{8,1335,10,-331,100,-331,23,-315,11,-315,92,-315,84,-315,83,-315,82,-315,81,-315,29,-315,143,-315,85,-315,86,-315,80,-315,78,-315,160,-315,87,-315,62,-315,28,-315,26,-315,44,-315,37,-315,19,-315,30,-315,31,-315,46,-315,27,-315},new int[]{-183,480});
    states[1335] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-182,1336,-181,1342,-180,1340,-147,207,-151,48,-152,51,-300,1341});
    states[1336] = new State(new int[]{9,1337,100,1338});
    states[1337] = new State(-316);
    states[1338] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-181,1339,-180,1340,-147,207,-151,48,-152,51,-300,1341});
    states[1339] = new State(-318);
    states[1340] = new State(new int[]{7,169,123,174,9,-319,100,-319},new int[]{-298,655});
    states[1341] = new State(-320);
    states[1342] = new State(-317);
    states[1343] = new State(-330);
    states[1344] = new State(-332);
    states[1345] = new State(-327);
    states[1346] = new State(-324);
    states[1347] = new State(new int[]{110,1348});
    states[1348] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494},new int[]{-260,1349,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[1349] = new State(new int[]{10,1350});
    states[1350] = new State(-444);
    states[1351] = new State(new int[]{148,1164,150,1165,151,1166,152,1167,154,1168,153,1169,23,-827,107,-827,91,-827,59,-827,29,-827,66,-827,50,-827,53,-827,62,-827,11,-827,28,-827,26,-827,44,-827,37,-827,19,-827,30,-827,31,-827,46,-827,27,-827,92,-827,84,-827,83,-827,82,-827,81,-827,149,-827},new int[]{-206,1352,-209,1170});
    states[1352] = new State(new int[]{10,1162,110,-830});
    states[1353] = new State(-372);
    states[1354] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-170,1355,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[1355] = new State(new int[]{8,585,5,-470,10,-470,110,-470},new int[]{-128,1356});
    states[1356] = new State(new int[]{5,1359,10,1351,110,-829},new int[]{-207,1357,-208,1367});
    states[1357] = new State(new int[]{23,1326,107,-322,91,-322,59,-322,29,-322,66,-322,50,-322,53,-322,62,-322,11,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322,92,-322,84,-322,83,-322,82,-322,81,-322,149,-322,41,-322},new int[]{-317,1358,-316,1324,-315,1346});
    states[1358] = new State(-460);
    states[1359] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-275,1360,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1360] = new State(new int[]{10,1351,110,-829},new int[]{-207,1361,-208,1363});
    states[1361] = new State(new int[]{23,1326,107,-322,91,-322,59,-322,29,-322,66,-322,50,-322,53,-322,62,-322,11,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322,92,-322,84,-322,83,-322,82,-322,81,-322,149,-322,41,-322},new int[]{-317,1362,-316,1324,-315,1346});
    states[1362] = new State(-461);
    states[1363] = new State(new int[]{110,1364});
    states[1364] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,662,21,270,22,275,76,541,40,608,18,686,37,695,44,699},new int[]{-103,1365,-101,884,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-322,885,-100,671,-323,694});
    states[1365] = new State(new int[]{10,1366});
    states[1366] = new State(-442);
    states[1367] = new State(new int[]{110,1368});
    states[1368] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,662,21,270,22,275,76,541,40,608,18,686,37,695,44,699},new int[]{-103,1369,-101,884,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-322,885,-100,671,-323,694});
    states[1369] = new State(new int[]{10,1370});
    states[1370] = new State(-443);
    states[1371] = new State(-357);
    states[1372] = new State(-358);
    states[1373] = new State(-366);
    states[1374] = new State(new int[]{28,1371,26,1372,44,1319,37,1354},new int[]{-3,1375,-230,1378,-216,1379,-228,1307,-225,1318,-229,1353});
    states[1375] = new State(new int[]{44,1319,37,1354},new int[]{-230,1376,-216,1377,-228,1307,-225,1318,-229,1353});
    states[1376] = new State(-367);
    states[1377] = new State(-439);
    states[1378] = new State(-368);
    states[1379] = new State(-437);
    states[1380] = new State(new int[]{107,1309,11,-369,28,-369,26,-369,44,-369,37,-369,19,-369,30,-369,31,-369,46,-369,27,-369,92,-369,84,-369,83,-369,82,-369,81,-369,59,-71,29,-71,66,-71,50,-71,53,-71,62,-71,91,-71},new int[]{-176,1381,-43,1193,-39,1196,-61,1308});
    states[1381] = new State(-421);
    states[1382] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,8,-377,110,-377,10,-377},new int[]{-172,1383,-171,1171,-170,1172,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[1383] = new State(new int[]{8,585,110,-470,10,-470},new int[]{-128,1384});
    states[1384] = new State(new int[]{110,1386,10,1160},new int[]{-207,1385});
    states[1385] = new State(-373);
    states[1386] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494},new int[]{-260,1387,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[1387] = new State(new int[]{10,1388});
    states[1388] = new State(-422);
    states[1389] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,8,-377,10,-377},new int[]{-172,1390,-171,1171,-170,1172,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[1390] = new State(new int[]{8,585,10,-470},new int[]{-128,1391});
    states[1391] = new State(new int[]{10,1160},new int[]{-207,1392});
    states[1392] = new State(-375);
    states[1393] = new State(-363);
    states[1394] = new State(-436);
    states[1395] = new State(-364);
    states[1396] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-170,1397,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[1397] = new State(new int[]{11,1290,5,-390},new int[]{-233,1398,-238,1287});
    states[1398] = new State(new int[]{85,1276,86,1282,10,-397},new int[]{-202,1399});
    states[1399] = new State(new int[]{10,1400});
    states[1400] = new State(new int[]{63,1270,153,1272,152,1273,148,1274,151,1275,11,-387,28,-387,26,-387,44,-387,37,-387,19,-387,30,-387,31,-387,46,-387,27,-387,92,-387,84,-387,83,-387,82,-387,81,-387},new int[]{-205,1401,-210,1402});
    states[1401] = new State(-379);
    states[1402] = new State(new int[]{10,1403});
    states[1403] = new State(new int[]{63,1270,11,-387,28,-387,26,-387,44,-387,37,-387,19,-387,30,-387,31,-387,46,-387,27,-387,92,-387,84,-387,83,-387,82,-387,81,-387},new int[]{-205,1404});
    states[1404] = new State(-380);
    states[1405] = new State(new int[]{46,1406});
    states[1406] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-170,1407,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[1407] = new State(new int[]{11,1290,5,-390},new int[]{-233,1408,-238,1287});
    states[1408] = new State(new int[]{110,1305,10,-386},new int[]{-211,1409});
    states[1409] = new State(new int[]{10,1410});
    states[1410] = new State(-383);
    states[1411] = new State(new int[]{11,647,92,-341,84,-341,83,-341,82,-341,81,-341,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-54,491,-53,492,-6,493,-250,1130,-55,1412});
    states[1412] = new State(-353);
    states[1413] = new State(-350);
    states[1414] = new State(-307);
    states[1415] = new State(-308);
    states[1416] = new State(new int[]{26,1417,48,1418,43,1419,8,-309,23,-309,11,-309,92,-309,84,-309,83,-309,82,-309,81,-309,29,-309,143,-309,85,-309,86,-309,80,-309,78,-309,160,-309,87,-309,62,-309,28,-309,44,-309,37,-309,19,-309,30,-309,31,-309,46,-309,27,-309,10,-309});
    states[1417] = new State(-310);
    states[1418] = new State(-311);
    states[1419] = new State(-312);
    states[1420] = new State(new int[]{68,1422,69,1423,148,1424,27,1425,28,1426,26,-304,43,-304,64,-304},new int[]{-20,1421});
    states[1421] = new State(-306);
    states[1422] = new State(-298);
    states[1423] = new State(-299);
    states[1424] = new State(-300);
    states[1425] = new State(-301);
    states[1426] = new State(-302);
    states[1427] = new State(-305);
    states[1428] = new State(new int[]{123,1430,120,-220},new int[]{-155,1429});
    states[1429] = new State(-221);
    states[1430] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-158,1431,-147,812,-151,48,-152,51});
    states[1431] = new State(new int[]{122,1432,121,1179,100,452});
    states[1432] = new State(-222);
    states[1433] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623,68,1422,69,1423,148,1424,27,1425,28,1426,26,-303,43,-303,64,-303},new int[]{-286,1434,-276,1234,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582,-30,1235,-21,1236,-22,1420,-20,1427});
    states[1434] = new State(new int[]{10,1435});
    states[1435] = new State(-219);
    states[1436] = new State(new int[]{11,647,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212},new int[]{-49,1437,-6,1228,-250,1130});
    states[1437] = new State(-106);
    states[1438] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,8,1443,59,-92,29,-92,66,-92,50,-92,53,-92,62,-92,91,-92},new int[]{-313,1439,-310,1440,-311,1441,-158,813,-147,812,-151,48,-152,51});
    states[1439] = new State(-112);
    states[1440] = new State(-108);
    states[1441] = new State(new int[]{10,1442});
    states[1442] = new State(-404);
    states[1443] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,1444,-151,48,-152,51});
    states[1444] = new State(new int[]{100,1445});
    states[1445] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-158,1446,-147,812,-151,48,-152,51});
    states[1446] = new State(new int[]{9,1447,100,452});
    states[1447] = new State(new int[]{110,1448});
    states[1448] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,1449,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[1449] = new State(new int[]{10,1450});
    states[1450] = new State(-109);
    states[1451] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,8,1443},new int[]{-313,1452,-310,1440,-311,1441,-158,813,-147,812,-151,48,-152,51});
    states[1452] = new State(-110);
    states[1453] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,8,1443},new int[]{-313,1454,-310,1440,-311,1441,-158,813,-147,812,-151,48,-152,51});
    states[1454] = new State(-111);
    states[1455] = new State(-245);
    states[1456] = new State(-246);
    states[1457] = new State(new int[]{127,467,121,-247,100,-247,120,-247,9,-247,8,-247,138,-247,136,-247,118,-247,117,-247,131,-247,132,-247,133,-247,134,-247,130,-247,116,-247,115,-247,128,-247,129,-247,126,-247,6,-247,5,-247,125,-247,123,-247,124,-247,122,-247,137,-247,135,-247,16,-247,92,-247,10,-247,98,-247,101,-247,33,-247,104,-247,2,-247,12,-247,99,-247,32,-247,85,-247,84,-247,83,-247,82,-247,81,-247,86,-247,13,-247,76,-247,51,-247,58,-247,141,-247,143,-247,80,-247,78,-247,160,-247,87,-247,45,-247,42,-247,21,-247,22,-247,144,-247,147,-247,145,-247,146,-247,155,-247,158,-247,157,-247,156,-247,57,-247,91,-247,40,-247,25,-247,97,-247,54,-247,35,-247,55,-247,102,-247,47,-247,36,-247,53,-247,60,-247,74,-247,72,-247,38,-247,70,-247,71,-247,110,-247});
    states[1458] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,1459,-151,48,-152,51});
    states[1459] = new State(new int[]{110,1460});
    states[1460] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-87,514,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[1461] = new State(new int[]{110,1462,127,525,8,-793,7,-793,142,-793,4,-793,15,-793,138,-793,136,-793,118,-793,117,-793,131,-793,132,-793,133,-793,134,-793,130,-793,116,-793,115,-793,128,-793,129,-793,126,-793,6,-793,5,-793,120,-793,125,-793,123,-793,121,-793,124,-793,122,-793,137,-793,135,-793,16,-793,100,-793,9,-793,13,-793,119,-793,11,-793,17,-793});
    states[1462] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,1463,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[1463] = new State(-602);
    states[1464] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,100,-604,9,-604},new int[]{-147,431,-151,48,-152,51});
    states[1465] = new State(-603);
    states[1466] = new State(-594);
    states[1467] = new State(-780);
    states[1468] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,677,12,-279,100,-279},new int[]{-271,1469,-272,1470,-93,181,-106,290,-107,291,-180,460,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154});
    states[1469] = new State(-277);
    states[1470] = new State(-278);
    states[1471] = new State(-276);
    states[1472] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-276,1473,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1473] = new State(-275);
    states[1474] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,24,336},new int[]{-283,1475,-278,1476,-180,205,-147,207,-151,48,-152,51,-270,477});
    states[1475] = new State(-737);
    states[1476] = new State(-738);
    states[1477] = new State(-751);
    states[1478] = new State(-752);
    states[1479] = new State(-753);
    states[1480] = new State(-754);
    states[1481] = new State(-755);
    states[1482] = new State(-756);
    states[1483] = new State(-757);
    states[1484] = new State(-240);
    states[1485] = new State(-236);
    states[1486] = new State(-626);
    states[1487] = new State(new int[]{8,1488});
    states[1488] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-333,1489,-332,1497,-147,1493,-151,48,-152,51,-101,1496,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[1489] = new State(new int[]{9,1490,100,1491});
    states[1490] = new State(-637);
    states[1491] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-332,1492,-147,1493,-151,48,-152,51,-101,1496,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[1492] = new State(-641);
    states[1493] = new State(new int[]{110,1494,8,-793,7,-793,142,-793,4,-793,15,-793,138,-793,136,-793,118,-793,117,-793,131,-793,132,-793,133,-793,134,-793,130,-793,116,-793,115,-793,128,-793,129,-793,126,-793,6,-793,120,-793,125,-793,123,-793,121,-793,124,-793,122,-793,137,-793,135,-793,16,-793,9,-793,100,-793,13,-793,119,-793,11,-793,17,-793});
    states[1494] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608},new int[]{-101,1495,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607});
    states[1495] = new State(-638);
    states[1496] = new State(-639);
    states[1497] = new State(-640);
    states[1498] = new State(new int[]{13,193,16,197,5,-705,12,-705});
    states[1499] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-90,1500,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[1500] = new State(new int[]{13,193,16,197,100,-189,9,-189,12,-189,5,-189});
    states[1501] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163,5,-706,12,-706},new int[]{-122,1502,-90,1498,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[1502] = new State(new int[]{5,1503,12,-712});
    states[1503] = new State(new int[]{143,47,85,49,86,50,80,52,78,250,160,54,87,55,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,42,267,21,270,22,275,11,349,76,822,56,825,141,826,8,840,135,843,116,368,115,369,63,163},new int[]{-90,1504,-91,225,-81,233,-13,238,-10,248,-14,211,-147,249,-151,48,-152,51,-165,265,-167,150,-166,154,-16,266,-257,269,-294,274,-239,348,-199,849,-173,847,-57,848,-265,855,-269,856,-11,851,-241,857});
    states[1504] = new State(new int[]{13,193,16,197,12,-714});
    states[1505] = new State(-186);
    states[1506] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156},new int[]{-93,1507,-106,290,-107,291,-180,460,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154});
    states[1507] = new State(new int[]{116,234,115,235,128,236,129,237,13,-249,121,-249,100,-249,120,-249,9,-249,8,-249,138,-249,136,-249,118,-249,117,-249,131,-249,132,-249,133,-249,134,-249,130,-249,126,-249,6,-249,5,-249,125,-249,123,-249,124,-249,122,-249,137,-249,135,-249,16,-249,92,-249,10,-249,98,-249,101,-249,33,-249,104,-249,2,-249,12,-249,99,-249,32,-249,85,-249,84,-249,83,-249,82,-249,81,-249,86,-249,76,-249,51,-249,58,-249,141,-249,143,-249,80,-249,78,-249,160,-249,87,-249,45,-249,42,-249,21,-249,22,-249,144,-249,147,-249,145,-249,146,-249,155,-249,158,-249,157,-249,156,-249,57,-249,91,-249,40,-249,25,-249,97,-249,54,-249,35,-249,55,-249,102,-249,47,-249,36,-249,53,-249,60,-249,74,-249,72,-249,38,-249,70,-249,71,-249,127,-249,110,-249},new int[]{-193,182});
    states[1508] = new State(-632);
    states[1509] = new State(new int[]{13,343});
    states[1510] = new State(new int[]{13,466});
    states[1511] = new State(-727);
    states[1512] = new State(-646);
    states[1513] = new State(-35);
    states[1514] = new State(new int[]{59,1199,29,1220,66,1224,50,1436,53,1451,62,1453,11,647,91,-65,92,-65,103,-65,44,-212,37,-212,28,-212,26,-212,19,-212,30,-212,31,-212},new int[]{-47,1515,-168,1516,-29,1517,-52,1518,-288,1519,-309,1520,-220,1521,-6,1522,-250,1130});
    states[1515] = new State(-69);
    states[1516] = new State(-79);
    states[1517] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,59,-80,29,-80,66,-80,50,-80,53,-80,62,-80,11,-80,44,-80,37,-80,28,-80,26,-80,19,-80,30,-80,31,-80,91,-80,92,-80,103,-80},new int[]{-27,1206,-28,1207,-141,1209,-147,1219,-151,48,-152,51});
    states[1518] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,59,-81,29,-81,66,-81,50,-81,53,-81,62,-81,11,-81,44,-81,37,-81,28,-81,26,-81,19,-81,30,-81,31,-81,91,-81,92,-81,103,-81},new int[]{-27,1223,-28,1207,-141,1209,-147,1219,-151,48,-152,51});
    states[1519] = new State(new int[]{11,647,59,-82,29,-82,66,-82,50,-82,53,-82,62,-82,44,-82,37,-82,28,-82,26,-82,19,-82,30,-82,31,-82,91,-82,92,-82,103,-82,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212},new int[]{-49,1227,-6,1228,-250,1130});
    states[1520] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,8,1443,59,-83,29,-83,66,-83,50,-83,53,-83,62,-83,11,-83,44,-83,37,-83,28,-83,26,-83,19,-83,30,-83,31,-83,91,-83,92,-83,103,-83},new int[]{-313,1439,-310,1440,-311,1441,-158,813,-147,812,-151,48,-152,51});
    states[1521] = new State(-84);
    states[1522] = new State(new int[]{44,1535,37,1542,28,1371,26,1372,19,1569,30,1576,31,1389,11,647},new int[]{-213,1523,-250,495,-214,1524,-221,1525,-228,1526,-225,1318,-229,1353,-3,1559,-217,1573,-227,1574});
    states[1523] = new State(-87);
    states[1524] = new State(-85);
    states[1525] = new State(-424);
    states[1526] = new State(new int[]{149,1528,107,1309,59,-68,29,-68,66,-68,50,-68,53,-68,62,-68,11,-68,44,-68,37,-68,28,-68,26,-68,19,-68,30,-68,31,-68,91,-68},new int[]{-178,1527,-177,1530,-41,1531,-42,1514,-61,1534});
    states[1527] = new State(-429);
    states[1528] = new State(new int[]{10,1529});
    states[1529] = new State(-435);
    states[1530] = new State(-445);
    states[1531] = new State(new int[]{91,17},new int[]{-255,1532});
    states[1532] = new State(new int[]{10,1533});
    states[1533] = new State(-467);
    states[1534] = new State(-446);
    states[1535] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-171,1536,-170,1172,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[1536] = new State(new int[]{8,585,10,-470,110,-470},new int[]{-128,1537});
    states[1537] = new State(new int[]{10,1351,110,-829},new int[]{-207,1322,-208,1538});
    states[1538] = new State(new int[]{110,1539});
    states[1539] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494},new int[]{-260,1540,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[1540] = new State(new int[]{10,1541});
    states[1541] = new State(-434);
    states[1542] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-170,1543,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[1543] = new State(new int[]{8,585,5,-470,10,-470,110,-470},new int[]{-128,1544});
    states[1544] = new State(new int[]{5,1545,10,1351,110,-829},new int[]{-207,1357,-208,1553});
    states[1545] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-275,1546,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1546] = new State(new int[]{10,1351,110,-829},new int[]{-207,1361,-208,1547});
    states[1547] = new State(new int[]{110,1548});
    states[1548] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,662,21,270,22,275,76,541,40,608,18,686,37,695,44,699},new int[]{-101,1549,-322,1551,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-100,671,-323,694});
    states[1549] = new State(new int[]{10,1550});
    states[1550] = new State(-430);
    states[1551] = new State(new int[]{10,1552});
    states[1552] = new State(-432);
    states[1553] = new State(new int[]{110,1554});
    states[1554] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,662,21,270,22,275,76,541,40,608,18,686,37,695,44,699},new int[]{-101,1555,-322,1557,-99,29,-98,311,-105,529,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,524,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-100,671,-323,694});
    states[1555] = new State(new int[]{10,1556});
    states[1556] = new State(-431);
    states[1557] = new State(new int[]{10,1558});
    states[1558] = new State(-433);
    states[1559] = new State(new int[]{19,1561,30,1563,44,1535,37,1542},new int[]{-221,1560,-228,1526,-225,1318,-229,1353});
    states[1560] = new State(-425);
    states[1561] = new State(new int[]{44,1535,37,1542},new int[]{-221,1562,-228,1526,-225,1318,-229,1353});
    states[1562] = new State(-428);
    states[1563] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,8,-377,110,-377,10,-377},new int[]{-172,1564,-171,1171,-170,1172,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[1564] = new State(new int[]{8,585,110,-470,10,-470},new int[]{-128,1565});
    states[1565] = new State(new int[]{110,1566,10,1160},new int[]{-207,503});
    states[1566] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494},new int[]{-260,1567,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[1567] = new State(new int[]{10,1568});
    states[1568] = new State(-420);
    states[1569] = new State(new int[]{44,1535,37,1542,28,1371,26,1372},new int[]{-221,1570,-3,1571,-228,1526,-225,1318,-229,1353});
    states[1570] = new State(-426);
    states[1571] = new State(new int[]{44,1535,37,1542},new int[]{-221,1572,-228,1526,-225,1318,-229,1353});
    states[1572] = new State(-427);
    states[1573] = new State(-86);
    states[1574] = new State(-68,new int[]{-177,1575,-41,1531,-42,1514});
    states[1575] = new State(-418);
    states[1576] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392,8,-377,110,-377,10,-377},new int[]{-172,1577,-171,1171,-170,1172,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[1577] = new State(new int[]{8,585,110,-470,10,-470},new int[]{-128,1578});
    states[1578] = new State(new int[]{110,1579,10,1160},new int[]{-207,1385});
    states[1579] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,10,-494},new int[]{-260,1580,-4,23,-113,24,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888});
    states[1580] = new State(new int[]{10,1581});
    states[1581] = new State(-419);
    states[1582] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148,144,1593},new int[]{-303,1583,-307,1594,-157,1587,-138,1592,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[1583] = new State(new int[]{10,1584,100,1585});
    states[1584] = new State(-38);
    states[1585] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148,144,1593},new int[]{-307,1586,-157,1587,-138,1592,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[1586] = new State(-44);
    states[1587] = new State(new int[]{7,1588,137,1590,10,-45,100,-45});
    states[1588] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148},new int[]{-138,1589,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[1589] = new State(-37);
    states[1590] = new State(new int[]{144,1591});
    states[1591] = new State(-47);
    states[1592] = new State(-36);
    states[1593] = new State(-46);
    states[1594] = new State(-43);
    states[1595] = new State(new int[]{3,1597,52,-15,91,-15,59,-15,29,-15,66,-15,50,-15,53,-15,62,-15,11,-15,44,-15,37,-15,28,-15,26,-15,19,-15,30,-15,31,-15,43,-15,92,-15,103,-15},new int[]{-184,1596});
    states[1596] = new State(-17);
    states[1597] = new State(new int[]{143,1598,144,1599});
    states[1598] = new State(-18);
    states[1599] = new State(-19);
    states[1600] = new State(-16);
    states[1601] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-147,1602,-151,48,-152,51});
    states[1602] = new State(new int[]{10,1604,8,1605},new int[]{-187,1603});
    states[1603] = new State(-28);
    states[1604] = new State(-29);
    states[1605] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-189,1606,-146,1612,-147,1611,-151,48,-152,51});
    states[1606] = new State(new int[]{9,1607,100,1609});
    states[1607] = new State(new int[]{10,1608});
    states[1608] = new State(-30);
    states[1609] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-146,1610,-147,1611,-151,48,-152,51});
    states[1610] = new State(-32);
    states[1611] = new State(-33);
    states[1612] = new State(-31);
    states[1613] = new State(-3);
    states[1614] = new State(new int[]{43,1635,52,-41,59,-41,29,-41,66,-41,50,-41,53,-41,62,-41,11,-41,44,-41,37,-41,28,-41,26,-41,19,-41,30,-41,31,-41,92,-41,103,-41,91,-41},new int[]{-162,1615,-163,1632,-302,1661});
    states[1615] = new State(new int[]{41,1629},new int[]{-161,1616});
    states[1616] = new State(new int[]{92,1619,103,1620,91,1626},new int[]{-154,1617});
    states[1617] = new State(new int[]{7,1618});
    states[1618] = new State(-48);
    states[1619] = new State(-58);
    states[1620] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,755,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,104,-494,10,-494},new int[]{-252,1621,-261,753,-260,22,-4,23,-113,24,-132,373,-111,386,-147,754,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888,-143,1043});
    states[1621] = new State(new int[]{92,1622,104,1623,10,20});
    states[1622] = new State(-59);
    states[1623] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,755,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494},new int[]{-252,1624,-261,753,-260,22,-4,23,-113,24,-132,373,-111,386,-147,754,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888,-143,1043});
    states[1624] = new State(new int[]{92,1625,10,20});
    states[1625] = new State(-60);
    states[1626] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,755,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,92,-494,10,-494},new int[]{-252,1627,-261,753,-260,22,-4,23,-113,24,-132,373,-111,386,-147,754,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888,-143,1043});
    states[1627] = new State(new int[]{92,1628,10,20});
    states[1628] = new State(-61);
    states[1629] = new State(-41,new int[]{-302,1630});
    states[1630] = new State(new int[]{52,1582,59,-68,29,-68,66,-68,50,-68,53,-68,62,-68,11,-68,44,-68,37,-68,28,-68,26,-68,19,-68,30,-68,31,-68,92,-68,103,-68,91,-68},new int[]{-41,1631,-304,14,-42,1514});
    states[1631] = new State(-56);
    states[1632] = new State(new int[]{92,1619,103,1620,91,1626},new int[]{-154,1633});
    states[1633] = new State(new int[]{7,1634});
    states[1634] = new State(-49);
    states[1635] = new State(-41,new int[]{-302,1636});
    states[1636] = new State(new int[]{52,1582,29,-63,66,-63,50,-63,53,-63,62,-63,11,-63,44,-63,37,-63,41,-63},new int[]{-40,1637,-304,14,-38,1638});
    states[1637] = new State(-55);
    states[1638] = new State(new int[]{29,1220,66,1224,50,1436,53,1451,62,1453,11,647,41,-62,44,-212,37,-212},new int[]{-48,1639,-29,1640,-52,1641,-288,1642,-309,1643,-232,1644,-6,1645,-250,1130,-231,1660});
    states[1639] = new State(-64);
    states[1640] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,29,-73,66,-73,50,-73,53,-73,62,-73,11,-73,44,-73,37,-73,41,-73},new int[]{-27,1206,-28,1207,-141,1209,-147,1219,-151,48,-152,51});
    states[1641] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,29,-74,66,-74,50,-74,53,-74,62,-74,11,-74,44,-74,37,-74,41,-74},new int[]{-27,1223,-28,1207,-141,1209,-147,1219,-151,48,-152,51});
    states[1642] = new State(new int[]{11,647,29,-75,66,-75,50,-75,53,-75,62,-75,44,-75,37,-75,41,-75,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212},new int[]{-49,1227,-6,1228,-250,1130});
    states[1643] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,8,1443,29,-76,66,-76,50,-76,53,-76,62,-76,11,-76,44,-76,37,-76,41,-76},new int[]{-313,1439,-310,1440,-311,1441,-158,813,-147,812,-151,48,-152,51});
    states[1644] = new State(-77);
    states[1645] = new State(new int[]{44,1652,11,647,37,1655},new int[]{-225,1646,-250,495,-229,1649});
    states[1646] = new State(new int[]{149,1647,29,-93,66,-93,50,-93,53,-93,62,-93,11,-93,44,-93,37,-93,41,-93});
    states[1647] = new State(new int[]{10,1648});
    states[1648] = new State(-94);
    states[1649] = new State(new int[]{149,1650,29,-95,66,-95,50,-95,53,-95,62,-95,11,-95,44,-95,37,-95,41,-95});
    states[1650] = new State(new int[]{10,1651});
    states[1651] = new State(-96);
    states[1652] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-171,1653,-170,1172,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[1653] = new State(new int[]{8,585,10,-470},new int[]{-128,1654});
    states[1654] = new State(new int[]{10,1160},new int[]{-207,1322});
    states[1655] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,45,392},new int[]{-170,1656,-142,1173,-137,1174,-134,1175,-147,1180,-151,48,-152,51,-191,1181,-334,1183,-149,1187});
    states[1656] = new State(new int[]{8,585,5,-470,10,-470},new int[]{-128,1657});
    states[1657] = new State(new int[]{5,1658,10,1160},new int[]{-207,1357});
    states[1658] = new State(new int[]{143,344,85,49,86,50,80,52,78,53,160,54,87,55,155,158,158,159,157,160,156,161,116,368,115,369,144,152,147,153,145,155,146,156,8,462,142,473,24,336,48,479,49,569,34,573,73,577,44,583,37,623},new int[]{-275,1659,-276,456,-272,342,-93,181,-106,290,-107,291,-180,292,-147,207,-151,48,-152,51,-16,457,-199,458,-165,461,-167,150,-166,154,-273,464,-300,465,-256,471,-249,472,-281,475,-278,476,-270,477,-31,478,-263,568,-130,572,-131,576,-226,580,-224,581,-223,582});
    states[1659] = new State(new int[]{10,1160},new int[]{-207,1361});
    states[1660] = new State(-78);
    states[1661] = new State(new int[]{52,1582,59,-68,29,-68,66,-68,50,-68,53,-68,62,-68,11,-68,44,-68,37,-68,28,-68,26,-68,19,-68,30,-68,31,-68,92,-68,103,-68,91,-68},new int[]{-41,1662,-304,14,-42,1514});
    states[1662] = new State(-57);
    states[1663] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-139,1664,-147,1667,-151,48,-152,51});
    states[1664] = new State(new int[]{10,1665});
    states[1665] = new State(new int[]{3,1597,43,-14,92,-14,103,-14,91,-14,52,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-185,1666,-186,1595,-184,1600});
    states[1666] = new State(-50);
    states[1667] = new State(-54);
    states[1668] = new State(-52);
    states[1669] = new State(-53);
    states[1670] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148},new int[]{-157,1671,-138,1592,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[1671] = new State(new int[]{10,1672,7,1588});
    states[1672] = new State(new int[]{3,1597,43,-14,92,-14,103,-14,91,-14,52,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-185,1673,-186,1595,-184,1600});
    states[1673] = new State(-51);
    states[1674] = new State(-4);
    states[1675] = new State(new int[]{50,1677,56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,439,21,270,22,275,76,541,40,608,5,617},new int[]{-87,1676,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,384,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616});
    states[1676] = new State(-7);
    states[1677] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-144,1678,-147,1679,-151,48,-152,51});
    states[1678] = new State(-8);
    states[1679] = new State(new int[]{123,1177,2,-220},new int[]{-155,1429});
    states[1680] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55},new int[]{-320,1681,-321,1682,-147,1686,-151,48,-152,51});
    states[1681] = new State(-9);
    states[1682] = new State(new int[]{7,1683,123,174,2,-785},new int[]{-298,1685});
    states[1683] = new State(new int[]{143,47,85,49,86,50,80,52,78,53,160,54,87,55,84,58,83,59,82,60,81,61,68,62,64,63,128,64,22,65,21,66,63,67,23,68,129,69,130,70,131,71,132,72,133,73,134,74,135,75,136,76,137,77,138,78,24,79,73,80,91,81,25,82,26,83,29,84,30,85,31,86,71,87,99,88,32,89,92,90,33,91,34,92,27,93,104,94,101,95,35,96,36,97,37,98,40,99,41,100,42,101,103,102,43,103,44,104,46,105,47,106,48,107,97,108,49,109,102,110,50,111,28,112,51,113,70,114,98,115,52,116,53,117,54,118,55,119,56,120,57,121,58,122,59,123,61,124,105,125,106,126,109,127,107,128,108,129,62,130,74,131,38,132,39,133,69,134,148,135,60,136,139,137,140,138,79,139,153,140,152,141,72,142,154,143,150,144,151,145,149,146,45,148},new int[]{-138,1684,-147,46,-151,48,-152,51,-292,56,-150,57,-293,147});
    states[1684] = new State(-784);
    states[1685] = new State(-786);
    states[1686] = new State(-783);
    states[1687] = new State(new int[]{56,42,144,152,147,153,145,155,146,156,155,158,158,159,157,160,156,161,63,163,11,359,135,363,116,368,115,369,142,370,141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,430,8,508,21,270,22,275,76,541,40,608,5,617,53,803},new int[]{-259,1688,-87,1689,-101,28,-99,29,-98,311,-105,321,-83,326,-82,332,-96,358,-15,43,-165,149,-167,150,-166,154,-16,157,-57,162,-199,382,-113,718,-132,373,-111,386,-147,428,-151,48,-152,51,-191,429,-257,516,-294,517,-17,518,-112,544,-58,545,-116,548,-173,549,-268,550,-97,551,-264,555,-266,556,-267,598,-240,599,-117,600,-242,607,-120,616,-4,1690,-314,1691});
    states[1688] = new State(-10);
    states[1689] = new State(-11);
    states[1690] = new State(-12);
    states[1691] = new State(-13);
    states[1692] = new State(new int[]{52,1582,141,-39,143,-39,85,-39,86,-39,80,-39,78,-39,160,-39,87,-39,45,-39,42,-39,8,-39,21,-39,22,-39,144,-39,147,-39,145,-39,146,-39,155,-39,158,-39,157,-39,156,-39,76,-39,57,-39,91,-39,40,-39,25,-39,97,-39,54,-39,35,-39,55,-39,102,-39,47,-39,36,-39,53,-39,60,-39,74,-39,72,-39,38,-39,11,-39,10,-39,44,-39,37,-39,2,-39},new int[]{-305,1693,-304,1698});
    states[1693] = new State(-66,new int[]{-44,1694});
    states[1694] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,755,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,11,647,10,-494,2,-494,44,-212,37,-212},new int[]{-252,1695,-6,1696,-261,753,-260,22,-4,23,-113,24,-132,373,-111,386,-147,754,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888,-143,1043,-250,1130});
    states[1695] = new State(new int[]{10,20,2,-5});
    states[1696] = new State(new int[]{44,1535,37,1542,11,647},new int[]{-221,1697,-250,495,-228,1526,-225,1318,-229,1353});
    states[1697] = new State(-67);
    states[1698] = new State(-40);
    states[1699] = new State(new int[]{52,1582,141,-39,143,-39,85,-39,86,-39,80,-39,78,-39,160,-39,87,-39,45,-39,42,-39,8,-39,21,-39,22,-39,144,-39,147,-39,145,-39,146,-39,155,-39,158,-39,157,-39,156,-39,76,-39,57,-39,91,-39,40,-39,25,-39,97,-39,54,-39,35,-39,55,-39,102,-39,47,-39,36,-39,53,-39,60,-39,74,-39,72,-39,38,-39,11,-39,10,-39,44,-39,37,-39,2,-39},new int[]{-305,1700,-304,1698});
    states[1700] = new State(-66,new int[]{-44,1701});
    states[1701] = new State(new int[]{141,385,143,47,85,49,86,50,80,52,78,250,160,54,87,55,45,392,42,507,8,508,21,270,22,275,144,152,147,153,145,155,146,156,155,755,158,159,157,160,156,161,76,541,57,734,91,17,40,726,25,741,97,757,54,762,35,767,55,778,102,784,47,791,36,794,53,803,60,875,74,880,72,867,38,889,11,647,10,-494,2,-494,44,-212,37,-212},new int[]{-252,1702,-6,1696,-261,753,-260,22,-4,23,-113,24,-132,373,-111,386,-147,754,-151,48,-152,51,-191,429,-257,516,-294,517,-15,712,-165,149,-167,150,-166,154,-16,157,-17,518,-112,544,-58,713,-116,548,-212,732,-133,733,-255,738,-153,739,-35,740,-247,756,-318,761,-124,766,-319,777,-160,782,-301,783,-248,790,-123,793,-314,802,-59,871,-174,872,-173,873,-169,874,-126,879,-127,886,-125,887,-348,888,-143,1043,-250,1130});
    states[1702] = new State(new int[]{10,20,2,-6});

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
    rules[46] = new Rule(-307, new int[]{144});
    rules[47] = new Rule(-307, new int[]{-157,137,144});
    rules[48] = new Rule(-306, new int[]{-308,-162,-161,-154,7});
    rules[49] = new Rule(-306, new int[]{-308,-163,-154,7});
    rules[50] = new Rule(-308, new int[]{-2,-139,10,-185});
    rules[51] = new Rule(-308, new int[]{109,-157,10,-185});
    rules[52] = new Rule(-2, new int[]{105});
    rules[53] = new Rule(-2, new int[]{106});
    rules[54] = new Rule(-139, new int[]{-147});
    rules[55] = new Rule(-162, new int[]{43,-302,-40});
    rules[56] = new Rule(-161, new int[]{41,-302,-41});
    rules[57] = new Rule(-163, new int[]{-302,-41});
    rules[58] = new Rule(-154, new int[]{92});
    rules[59] = new Rule(-154, new int[]{103,-252,92});
    rules[60] = new Rule(-154, new int[]{103,-252,104,-252,92});
    rules[61] = new Rule(-154, new int[]{91,-252,92});
    rules[62] = new Rule(-40, new int[]{-38});
    rules[63] = new Rule(-38, new int[]{});
    rules[64] = new Rule(-38, new int[]{-38,-48});
    rules[65] = new Rule(-41, new int[]{-42});
    rules[66] = new Rule(-44, new int[]{});
    rules[67] = new Rule(-44, new int[]{-44,-6,-221});
    rules[68] = new Rule(-42, new int[]{});
    rules[69] = new Rule(-42, new int[]{-42,-47});
    rules[70] = new Rule(-43, new int[]{-39});
    rules[71] = new Rule(-39, new int[]{});
    rules[72] = new Rule(-39, new int[]{-39,-46});
    rules[73] = new Rule(-48, new int[]{-29});
    rules[74] = new Rule(-48, new int[]{-52});
    rules[75] = new Rule(-48, new int[]{-288});
    rules[76] = new Rule(-48, new int[]{-309});
    rules[77] = new Rule(-48, new int[]{-232});
    rules[78] = new Rule(-48, new int[]{-231});
    rules[79] = new Rule(-47, new int[]{-168});
    rules[80] = new Rule(-47, new int[]{-29});
    rules[81] = new Rule(-47, new int[]{-52});
    rules[82] = new Rule(-47, new int[]{-288});
    rules[83] = new Rule(-47, new int[]{-309});
    rules[84] = new Rule(-47, new int[]{-220});
    rules[85] = new Rule(-213, new int[]{-214});
    rules[86] = new Rule(-213, new int[]{-217});
    rules[87] = new Rule(-220, new int[]{-6,-213});
    rules[88] = new Rule(-46, new int[]{-168});
    rules[89] = new Rule(-46, new int[]{-29});
    rules[90] = new Rule(-46, new int[]{-52});
    rules[91] = new Rule(-46, new int[]{-288});
    rules[92] = new Rule(-46, new int[]{-309});
    rules[93] = new Rule(-232, new int[]{-6,-225});
    rules[94] = new Rule(-232, new int[]{-6,-225,149,10});
    rules[95] = new Rule(-231, new int[]{-6,-229});
    rules[96] = new Rule(-231, new int[]{-6,-229,149,10});
    rules[97] = new Rule(-168, new int[]{59,-156,10});
    rules[98] = new Rule(-156, new int[]{-143});
    rules[99] = new Rule(-156, new int[]{-156,100,-143});
    rules[100] = new Rule(-143, new int[]{155});
    rules[101] = new Rule(-143, new int[]{-147});
    rules[102] = new Rule(-29, new int[]{29,-27});
    rules[103] = new Rule(-29, new int[]{-29,-27});
    rules[104] = new Rule(-52, new int[]{66,-27});
    rules[105] = new Rule(-52, new int[]{-52,-27});
    rules[106] = new Rule(-288, new int[]{50,-49});
    rules[107] = new Rule(-288, new int[]{-288,-49});
    rules[108] = new Rule(-313, new int[]{-310});
    rules[109] = new Rule(-313, new int[]{8,-147,100,-158,9,110,-101,10});
    rules[110] = new Rule(-309, new int[]{53,-313});
    rules[111] = new Rule(-309, new int[]{62,-313});
    rules[112] = new Rule(-309, new int[]{-309,-313});
    rules[113] = new Rule(-27, new int[]{-28,10});
    rules[114] = new Rule(-28, new int[]{-141,120,-109});
    rules[115] = new Rule(-28, new int[]{-141,5,-276,120,-84});
    rules[116] = new Rule(-109, new int[]{-90});
    rules[117] = new Rule(-109, new int[]{-95});
    rules[118] = new Rule(-141, new int[]{-147});
    rules[119] = new Rule(-91, new int[]{-81});
    rules[120] = new Rule(-91, new int[]{-91,-192,-81});
    rules[121] = new Rule(-90, new int[]{-91});
    rules[122] = new Rule(-90, new int[]{-241});
    rules[123] = new Rule(-90, new int[]{-90,16,-91});
    rules[124] = new Rule(-241, new int[]{-90,13,-90,5,-90});
    rules[125] = new Rule(-192, new int[]{120});
    rules[126] = new Rule(-192, new int[]{125});
    rules[127] = new Rule(-192, new int[]{123});
    rules[128] = new Rule(-192, new int[]{121});
    rules[129] = new Rule(-192, new int[]{124});
    rules[130] = new Rule(-192, new int[]{122});
    rules[131] = new Rule(-192, new int[]{137});
    rules[132] = new Rule(-81, new int[]{-13});
    rules[133] = new Rule(-81, new int[]{-81,-193,-13});
    rules[134] = new Rule(-193, new int[]{116});
    rules[135] = new Rule(-193, new int[]{115});
    rules[136] = new Rule(-193, new int[]{128});
    rules[137] = new Rule(-193, new int[]{129});
    rules[138] = new Rule(-265, new int[]{-13,-201,-283});
    rules[139] = new Rule(-269, new int[]{-11,119,-10});
    rules[140] = new Rule(-269, new int[]{-11,119,-269});
    rules[141] = new Rule(-269, new int[]{-199,-269});
    rules[142] = new Rule(-13, new int[]{-10});
    rules[143] = new Rule(-13, new int[]{-265});
    rules[144] = new Rule(-13, new int[]{-269});
    rules[145] = new Rule(-13, new int[]{-13,-195,-10});
    rules[146] = new Rule(-13, new int[]{-13,-195,-269});
    rules[147] = new Rule(-195, new int[]{118});
    rules[148] = new Rule(-195, new int[]{117});
    rules[149] = new Rule(-195, new int[]{131});
    rules[150] = new Rule(-195, new int[]{132});
    rules[151] = new Rule(-195, new int[]{133});
    rules[152] = new Rule(-195, new int[]{134});
    rules[153] = new Rule(-195, new int[]{130});
    rules[154] = new Rule(-11, new int[]{-14});
    rules[155] = new Rule(-11, new int[]{8,-90,9});
    rules[156] = new Rule(-10, new int[]{-14});
    rules[157] = new Rule(-10, new int[]{-239});
    rules[158] = new Rule(-10, new int[]{56});
    rules[159] = new Rule(-10, new int[]{141,-10});
    rules[160] = new Rule(-10, new int[]{8,-90,9});
    rules[161] = new Rule(-10, new int[]{135,-10});
    rules[162] = new Rule(-10, new int[]{-199,-10});
    rules[163] = new Rule(-10, new int[]{-173});
    rules[164] = new Rule(-10, new int[]{-57});
    rules[165] = new Rule(-239, new int[]{11,-69,12});
    rules[166] = new Rule(-239, new int[]{76,-69,76});
    rules[167] = new Rule(-199, new int[]{116});
    rules[168] = new Rule(-199, new int[]{115});
    rules[169] = new Rule(-14, new int[]{-147});
    rules[170] = new Rule(-14, new int[]{-165});
    rules[171] = new Rule(-14, new int[]{-16});
    rules[172] = new Rule(-14, new int[]{42,-147});
    rules[173] = new Rule(-14, new int[]{-257});
    rules[174] = new Rule(-14, new int[]{-294});
    rules[175] = new Rule(-14, new int[]{-14,-12});
    rules[176] = new Rule(-14, new int[]{-14,4,-298});
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
    rules[196] = new Rule(-84, new int[]{-243});
    rules[197] = new Rule(-95, new int[]{8,-66,9});
    rules[198] = new Rule(-66, new int[]{});
    rules[199] = new Rule(-66, new int[]{-65});
    rules[200] = new Rule(-65, new int[]{-85});
    rules[201] = new Rule(-65, new int[]{-65,100,-85});
    rules[202] = new Rule(-243, new int[]{8,-245,9});
    rules[203] = new Rule(-245, new int[]{-244});
    rules[204] = new Rule(-245, new int[]{-244,10});
    rules[205] = new Rule(-244, new int[]{-246});
    rules[206] = new Rule(-244, new int[]{-244,10,-246});
    rules[207] = new Rule(-246, new int[]{-136,5,-84});
    rules[208] = new Rule(-136, new int[]{-147});
    rules[209] = new Rule(-49, new int[]{-6,-50});
    rules[210] = new Rule(-6, new int[]{-250});
    rules[211] = new Rule(-6, new int[]{-6,-250});
    rules[212] = new Rule(-6, new int[]{});
    rules[213] = new Rule(-250, new int[]{11,-251,12});
    rules[214] = new Rule(-251, new int[]{-8});
    rules[215] = new Rule(-251, new int[]{-251,100,-8});
    rules[216] = new Rule(-8, new int[]{-9});
    rules[217] = new Rule(-8, new int[]{-147,5,-9});
    rules[218] = new Rule(-50, new int[]{-144,120,-286,10});
    rules[219] = new Rule(-50, new int[]{-145,-286,10});
    rules[220] = new Rule(-144, new int[]{-147});
    rules[221] = new Rule(-144, new int[]{-147,-155});
    rules[222] = new Rule(-145, new int[]{-147,123,-158,122});
    rules[223] = new Rule(-286, new int[]{-276});
    rules[224] = new Rule(-286, new int[]{-30});
    rules[225] = new Rule(-273, new int[]{-272,13});
    rules[226] = new Rule(-273, new int[]{-300,13});
    rules[227] = new Rule(-276, new int[]{-272});
    rules[228] = new Rule(-276, new int[]{-273});
    rules[229] = new Rule(-276, new int[]{-256});
    rules[230] = new Rule(-276, new int[]{-249});
    rules[231] = new Rule(-276, new int[]{-281});
    rules[232] = new Rule(-276, new int[]{-226});
    rules[233] = new Rule(-276, new int[]{-300});
    rules[234] = new Rule(-300, new int[]{-180,-298});
    rules[235] = new Rule(-298, new int[]{123,-296,121});
    rules[236] = new Rule(-299, new int[]{125});
    rules[237] = new Rule(-299, new int[]{123,-297,121});
    rules[238] = new Rule(-296, new int[]{-279});
    rules[239] = new Rule(-296, new int[]{-296,100,-279});
    rules[240] = new Rule(-297, new int[]{-280});
    rules[241] = new Rule(-297, new int[]{-297,100,-280});
    rules[242] = new Rule(-280, new int[]{});
    rules[243] = new Rule(-279, new int[]{-272});
    rules[244] = new Rule(-279, new int[]{-272,13});
    rules[245] = new Rule(-279, new int[]{-281});
    rules[246] = new Rule(-279, new int[]{-226});
    rules[247] = new Rule(-279, new int[]{-300});
    rules[248] = new Rule(-272, new int[]{-93});
    rules[249] = new Rule(-272, new int[]{-93,6,-93});
    rules[250] = new Rule(-272, new int[]{8,-80,9});
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
    rules[264] = new Rule(-78, new int[]{-276});
    rules[265] = new Rule(-78, new int[]{-276,120,-87});
    rules[266] = new Rule(-249, new int[]{142,-275});
    rules[267] = new Rule(-281, new int[]{-278});
    rules[268] = new Rule(-281, new int[]{-31});
    rules[269] = new Rule(-281, new int[]{-263});
    rules[270] = new Rule(-281, new int[]{-130});
    rules[271] = new Rule(-281, new int[]{-131});
    rules[272] = new Rule(-131, new int[]{73,58,-276});
    rules[273] = new Rule(-278, new int[]{24,11,-164,12,58,-276});
    rules[274] = new Rule(-278, new int[]{-270});
    rules[275] = new Rule(-270, new int[]{24,58,-276});
    rules[276] = new Rule(-164, new int[]{-271});
    rules[277] = new Rule(-164, new int[]{-164,100,-271});
    rules[278] = new Rule(-271, new int[]{-272});
    rules[279] = new Rule(-271, new int[]{});
    rules[280] = new Rule(-263, new int[]{49,58,-276});
    rules[281] = new Rule(-130, new int[]{34,58,-276});
    rules[282] = new Rule(-130, new int[]{34});
    rules[283] = new Rule(-256, new int[]{143,11,-90,12});
    rules[284] = new Rule(-226, new int[]{-224});
    rules[285] = new Rule(-224, new int[]{-223});
    rules[286] = new Rule(-223, new int[]{44,-128});
    rules[287] = new Rule(-223, new int[]{37,-128,5,-275});
    rules[288] = new Rule(-223, new int[]{-180,127,-279});
    rules[289] = new Rule(-223, new int[]{-300,127,-279});
    rules[290] = new Rule(-223, new int[]{8,9,127,-279});
    rules[291] = new Rule(-223, new int[]{8,-80,9,127,-279});
    rules[292] = new Rule(-223, new int[]{-180,127,8,9});
    rules[293] = new Rule(-223, new int[]{-300,127,8,9});
    rules[294] = new Rule(-223, new int[]{8,9,127,8,9});
    rules[295] = new Rule(-223, new int[]{8,-80,9,127,8,9});
    rules[296] = new Rule(-30, new int[]{-21,-290,-183,-317,-26});
    rules[297] = new Rule(-31, new int[]{48,-183,-317,-25,92});
    rules[298] = new Rule(-20, new int[]{68});
    rules[299] = new Rule(-20, new int[]{69});
    rules[300] = new Rule(-20, new int[]{148});
    rules[301] = new Rule(-20, new int[]{27});
    rules[302] = new Rule(-20, new int[]{28});
    rules[303] = new Rule(-21, new int[]{});
    rules[304] = new Rule(-21, new int[]{-22});
    rules[305] = new Rule(-22, new int[]{-20});
    rules[306] = new Rule(-22, new int[]{-22,-20});
    rules[307] = new Rule(-290, new int[]{26});
    rules[308] = new Rule(-290, new int[]{43});
    rules[309] = new Rule(-290, new int[]{64});
    rules[310] = new Rule(-290, new int[]{64,26});
    rules[311] = new Rule(-290, new int[]{64,48});
    rules[312] = new Rule(-290, new int[]{64,43});
    rules[313] = new Rule(-26, new int[]{});
    rules[314] = new Rule(-26, new int[]{-25,92});
    rules[315] = new Rule(-183, new int[]{});
    rules[316] = new Rule(-183, new int[]{8,-182,9});
    rules[317] = new Rule(-182, new int[]{-181});
    rules[318] = new Rule(-182, new int[]{-182,100,-181});
    rules[319] = new Rule(-181, new int[]{-180});
    rules[320] = new Rule(-181, new int[]{-300});
    rules[321] = new Rule(-155, new int[]{123,-158,121});
    rules[322] = new Rule(-317, new int[]{});
    rules[323] = new Rule(-317, new int[]{-316});
    rules[324] = new Rule(-316, new int[]{-315});
    rules[325] = new Rule(-316, new int[]{-316,-315});
    rules[326] = new Rule(-315, new int[]{23,-158,5,-287,10});
    rules[327] = new Rule(-287, new int[]{-285});
    rules[328] = new Rule(-287, new int[]{-287,100,-285});
    rules[329] = new Rule(-285, new int[]{-276});
    rules[330] = new Rule(-285, new int[]{26});
    rules[331] = new Rule(-285, new int[]{48});
    rules[332] = new Rule(-285, new int[]{30});
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
    rules[355] = new Rule(-51, new int[]{-312});
    rules[356] = new Rule(-51, new int[]{-3,-312});
    rules[357] = new Rule(-3, new int[]{28});
    rules[358] = new Rule(-3, new int[]{26});
    rules[359] = new Rule(-312, new int[]{-311});
    rules[360] = new Rule(-312, new int[]{62,-158,5,-276});
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
    rules[378] = new Rule(-55, new int[]{-6,-258});
    rules[379] = new Rule(-258, new int[]{46,-170,-233,-202,10,-205});
    rules[380] = new Rule(-258, new int[]{46,-170,-233,-202,10,-210,10,-205});
    rules[381] = new Rule(-258, new int[]{-3,46,-170,-233,-202,10,-205});
    rules[382] = new Rule(-258, new int[]{-3,46,-170,-233,-202,10,-210,10,-205});
    rules[383] = new Rule(-258, new int[]{27,46,-170,-233,-211,10});
    rules[384] = new Rule(-258, new int[]{-3,27,46,-170,-233,-211,10});
    rules[385] = new Rule(-211, new int[]{110,-87});
    rules[386] = new Rule(-211, new int[]{});
    rules[387] = new Rule(-205, new int[]{});
    rules[388] = new Rule(-205, new int[]{63,10});
    rules[389] = new Rule(-233, new int[]{-238,5,-275});
    rules[390] = new Rule(-238, new int[]{});
    rules[391] = new Rule(-238, new int[]{11,-237,12});
    rules[392] = new Rule(-237, new int[]{-236});
    rules[393] = new Rule(-237, new int[]{-237,10,-236});
    rules[394] = new Rule(-236, new int[]{-158,5,-275});
    rules[395] = new Rule(-114, new int[]{-88});
    rules[396] = new Rule(-114, new int[]{});
    rules[397] = new Rule(-202, new int[]{});
    rules[398] = new Rule(-202, new int[]{85,-114,-203});
    rules[399] = new Rule(-202, new int[]{86,-260,-204});
    rules[400] = new Rule(-203, new int[]{});
    rules[401] = new Rule(-203, new int[]{86,-260});
    rules[402] = new Rule(-204, new int[]{});
    rules[403] = new Rule(-204, new int[]{85,-114});
    rules[404] = new Rule(-310, new int[]{-311,10});
    rules[405] = new Rule(-338, new int[]{110});
    rules[406] = new Rule(-338, new int[]{120});
    rules[407] = new Rule(-311, new int[]{-158,5,-276});
    rules[408] = new Rule(-311, new int[]{-158,110,-88});
    rules[409] = new Rule(-311, new int[]{-158,5,-276,-338,-86});
    rules[410] = new Rule(-86, new int[]{-85});
    rules[411] = new Rule(-86, new int[]{-81,6,-13});
    rules[412] = new Rule(-86, new int[]{-323});
    rules[413] = new Rule(-86, new int[]{-147,127,-328});
    rules[414] = new Rule(-86, new int[]{8,9,-324,127,-328});
    rules[415] = new Rule(-86, new int[]{8,-66,9,127,-328});
    rules[416] = new Rule(-86, new int[]{-242});
    rules[417] = new Rule(-85, new int[]{-84});
    rules[418] = new Rule(-217, new int[]{-227,-177});
    rules[419] = new Rule(-217, new int[]{30,-172,-128,110,-260,10});
    rules[420] = new Rule(-217, new int[]{-3,30,-172,-128,110,-260,10});
    rules[421] = new Rule(-218, new int[]{-227,-176});
    rules[422] = new Rule(-218, new int[]{30,-172,-128,110,-260,10});
    rules[423] = new Rule(-218, new int[]{-3,30,-172,-128,110,-260,10});
    rules[424] = new Rule(-214, new int[]{-221});
    rules[425] = new Rule(-214, new int[]{-3,-221});
    rules[426] = new Rule(-214, new int[]{19,-221});
    rules[427] = new Rule(-214, new int[]{19,-3,-221});
    rules[428] = new Rule(-214, new int[]{-3,19,-221});
    rules[429] = new Rule(-221, new int[]{-228,-178});
    rules[430] = new Rule(-221, new int[]{37,-170,-128,5,-275,-208,110,-101,10});
    rules[431] = new Rule(-221, new int[]{37,-170,-128,-208,110,-101,10});
    rules[432] = new Rule(-221, new int[]{37,-170,-128,5,-275,-208,110,-322,10});
    rules[433] = new Rule(-221, new int[]{37,-170,-128,-208,110,-322,10});
    rules[434] = new Rule(-221, new int[]{44,-171,-128,-208,110,-260,10});
    rules[435] = new Rule(-221, new int[]{-228,149,10});
    rules[436] = new Rule(-215, new int[]{-216});
    rules[437] = new Rule(-215, new int[]{19,-216});
    rules[438] = new Rule(-215, new int[]{-3,-216});
    rules[439] = new Rule(-215, new int[]{19,-3,-216});
    rules[440] = new Rule(-215, new int[]{-3,19,-216});
    rules[441] = new Rule(-216, new int[]{-228,-176});
    rules[442] = new Rule(-216, new int[]{37,-170,-128,5,-275,-208,110,-103,10});
    rules[443] = new Rule(-216, new int[]{37,-170,-128,-208,110,-103,10});
    rules[444] = new Rule(-216, new int[]{44,-171,-128,-208,110,-260,10});
    rules[445] = new Rule(-178, new int[]{-177});
    rules[446] = new Rule(-178, new int[]{-61});
    rules[447] = new Rule(-171, new int[]{-170});
    rules[448] = new Rule(-170, new int[]{-142});
    rules[449] = new Rule(-170, new int[]{-334,7,-142});
    rules[450] = new Rule(-149, new int[]{-137});
    rules[451] = new Rule(-334, new int[]{-149});
    rules[452] = new Rule(-334, new int[]{-334,7,-149});
    rules[453] = new Rule(-142, new int[]{-137});
    rules[454] = new Rule(-142, new int[]{-191});
    rules[455] = new Rule(-142, new int[]{-191,-155});
    rules[456] = new Rule(-137, new int[]{-134});
    rules[457] = new Rule(-137, new int[]{-134,-155});
    rules[458] = new Rule(-134, new int[]{-147});
    rules[459] = new Rule(-225, new int[]{44,-171,-128,-207,-317});
    rules[460] = new Rule(-229, new int[]{37,-170,-128,-207,-317});
    rules[461] = new Rule(-229, new int[]{37,-170,-128,5,-275,-207,-317});
    rules[462] = new Rule(-61, new int[]{107,-108,80,-108,10});
    rules[463] = new Rule(-61, new int[]{107,-108,10});
    rules[464] = new Rule(-61, new int[]{107,10});
    rules[465] = new Rule(-108, new int[]{-147});
    rules[466] = new Rule(-108, new int[]{-165});
    rules[467] = new Rule(-177, new int[]{-41,-255,10});
    rules[468] = new Rule(-176, new int[]{-43,-255,10});
    rules[469] = new Rule(-176, new int[]{-61});
    rules[470] = new Rule(-128, new int[]{});
    rules[471] = new Rule(-128, new int[]{8,9});
    rules[472] = new Rule(-128, new int[]{8,-129,9});
    rules[473] = new Rule(-129, new int[]{-56});
    rules[474] = new Rule(-129, new int[]{-129,10,-56});
    rules[475] = new Rule(-56, new int[]{-6,-295});
    rules[476] = new Rule(-295, new int[]{-159,5,-275});
    rules[477] = new Rule(-295, new int[]{53,-159,5,-275});
    rules[478] = new Rule(-295, new int[]{29,-159,5,-275});
    rules[479] = new Rule(-295, new int[]{108,-159,5,-275});
    rules[480] = new Rule(-295, new int[]{-159,5,-275,110,-87});
    rules[481] = new Rule(-295, new int[]{53,-159,5,-275,110,-87});
    rules[482] = new Rule(-295, new int[]{29,-159,5,-275,110,-87});
    rules[483] = new Rule(-159, new int[]{-135});
    rules[484] = new Rule(-159, new int[]{-159,100,-135});
    rules[485] = new Rule(-135, new int[]{-147});
    rules[486] = new Rule(-275, new int[]{-276});
    rules[487] = new Rule(-277, new int[]{-272});
    rules[488] = new Rule(-277, new int[]{-256});
    rules[489] = new Rule(-277, new int[]{-249});
    rules[490] = new Rule(-277, new int[]{-281});
    rules[491] = new Rule(-277, new int[]{-300});
    rules[492] = new Rule(-261, new int[]{-260});
    rules[493] = new Rule(-261, new int[]{-143,5,-261});
    rules[494] = new Rule(-260, new int[]{});
    rules[495] = new Rule(-260, new int[]{-4});
    rules[496] = new Rule(-260, new int[]{-212});
    rules[497] = new Rule(-260, new int[]{-133});
    rules[498] = new Rule(-260, new int[]{-255});
    rules[499] = new Rule(-260, new int[]{-153});
    rules[500] = new Rule(-260, new int[]{-35});
    rules[501] = new Rule(-260, new int[]{-247});
    rules[502] = new Rule(-260, new int[]{-318});
    rules[503] = new Rule(-260, new int[]{-124});
    rules[504] = new Rule(-260, new int[]{-319});
    rules[505] = new Rule(-260, new int[]{-160});
    rules[506] = new Rule(-260, new int[]{-301});
    rules[507] = new Rule(-260, new int[]{-248});
    rules[508] = new Rule(-260, new int[]{-123});
    rules[509] = new Rule(-260, new int[]{-314});
    rules[510] = new Rule(-260, new int[]{-59});
    rules[511] = new Rule(-260, new int[]{-169});
    rules[512] = new Rule(-260, new int[]{-126});
    rules[513] = new Rule(-260, new int[]{-127});
    rules[514] = new Rule(-260, new int[]{-125});
    rules[515] = new Rule(-260, new int[]{-348});
    rules[516] = new Rule(-125, new int[]{72,-101,99,-260});
    rules[517] = new Rule(-126, new int[]{74,-103});
    rules[518] = new Rule(-127, new int[]{74,73,-103});
    rules[519] = new Rule(-314, new int[]{53,-311});
    rules[520] = new Rule(-314, new int[]{8,53,-147,100,-337,9,110,-87});
    rules[521] = new Rule(-314, new int[]{53,8,-147,100,-158,9,110,-87});
    rules[522] = new Rule(-4, new int[]{-113,-194,-88});
    rules[523] = new Rule(-4, new int[]{8,-111,100,-336,9,-194,-87});
    rules[524] = new Rule(-336, new int[]{-111});
    rules[525] = new Rule(-336, new int[]{-336,100,-111});
    rules[526] = new Rule(-337, new int[]{53,-147});
    rules[527] = new Rule(-337, new int[]{-337,100,53,-147});
    rules[528] = new Rule(-212, new int[]{-113});
    rules[529] = new Rule(-133, new int[]{57,-143});
    rules[530] = new Rule(-255, new int[]{91,-252,92});
    rules[531] = new Rule(-252, new int[]{-261});
    rules[532] = new Rule(-252, new int[]{-252,10,-261});
    rules[533] = new Rule(-153, new int[]{40,-101,51,-260});
    rules[534] = new Rule(-153, new int[]{40,-101,51,-260,32,-260});
    rules[535] = new Rule(-348, new int[]{38,-101,55,-350,-253,92});
    rules[536] = new Rule(-348, new int[]{38,-101,55,-350,10,-253,92});
    rules[537] = new Rule(-350, new int[]{-349});
    rules[538] = new Rule(-350, new int[]{-350,10,-349});
    rules[539] = new Rule(-349, new int[]{-342,39,-101,5,-260});
    rules[540] = new Rule(-349, new int[]{-341,5,-260});
    rules[541] = new Rule(-349, new int[]{-343,5,-260});
    rules[542] = new Rule(-349, new int[]{-344,39,-101,5,-260});
    rules[543] = new Rule(-349, new int[]{-344,5,-260});
    rules[544] = new Rule(-35, new int[]{25,-101,58,-36,-253,92});
    rules[545] = new Rule(-35, new int[]{25,-101,58,-36,10,-253,92});
    rules[546] = new Rule(-35, new int[]{25,-101,58,-253,92});
    rules[547] = new Rule(-36, new int[]{-262});
    rules[548] = new Rule(-36, new int[]{-36,10,-262});
    rules[549] = new Rule(-262, new int[]{-74,5,-260});
    rules[550] = new Rule(-74, new int[]{-110});
    rules[551] = new Rule(-74, new int[]{-74,100,-110});
    rules[552] = new Rule(-110, new int[]{-94});
    rules[553] = new Rule(-253, new int[]{});
    rules[554] = new Rule(-253, new int[]{32,-252});
    rules[555] = new Rule(-247, new int[]{97,-252,98,-87});
    rules[556] = new Rule(-318, new int[]{54,-101,-291,-260});
    rules[557] = new Rule(-291, new int[]{99});
    rules[558] = new Rule(-291, new int[]{});
    rules[559] = new Rule(-169, new int[]{60,-101,99,-260});
    rules[560] = new Rule(-360, new int[]{87,143});
    rules[561] = new Rule(-360, new int[]{});
    rules[562] = new Rule(-274, new int[]{5,-276});
    rules[563] = new Rule(-274, new int[]{});
    rules[564] = new Rule(-19, new int[]{53});
    rules[565] = new Rule(-19, new int[]{});
    rules[566] = new Rule(-119, new int[]{70});
    rules[567] = new Rule(-119, new int[]{71});
    rules[568] = new Rule(-123, new int[]{36,-147,-274,137,-101,-360,99,-260});
    rules[569] = new Rule(-123, new int[]{36,53,-147,-274,137,-101,-360,99,-260});
    rules[570] = new Rule(-123, new int[]{36,53,8,-158,9,137,-101,-360,99,-260});
    rules[571] = new Rule(-124, new int[]{35,-19,-147,-274,110,-101,-119,-101,-291,-260});
    rules[572] = new Rule(-124, new int[]{35,-19,-147,-274,110,-101,-119,-101,160,-101,99,-260});
    rules[573] = new Rule(-319, new int[]{55,-71,99,-260});
    rules[574] = new Rule(-160, new int[]{42});
    rules[575] = new Rule(-301, new int[]{102,-252,-289});
    rules[576] = new Rule(-289, new int[]{101,-252,92});
    rules[577] = new Rule(-289, new int[]{33,-60,92});
    rules[578] = new Rule(-60, new int[]{-63,-254});
    rules[579] = new Rule(-60, new int[]{-63,10,-254});
    rules[580] = new Rule(-60, new int[]{-252});
    rules[581] = new Rule(-63, new int[]{-62});
    rules[582] = new Rule(-63, new int[]{-63,10,-62});
    rules[583] = new Rule(-254, new int[]{});
    rules[584] = new Rule(-254, new int[]{32,-252});
    rules[585] = new Rule(-62, new int[]{79,-64,99,-260});
    rules[586] = new Rule(-64, new int[]{-179});
    rules[587] = new Rule(-64, new int[]{-140,5,-179});
    rules[588] = new Rule(-179, new int[]{-180});
    rules[589] = new Rule(-140, new int[]{-147});
    rules[590] = new Rule(-248, new int[]{47});
    rules[591] = new Rule(-248, new int[]{47,-87});
    rules[592] = new Rule(-71, new int[]{-88});
    rules[593] = new Rule(-71, new int[]{-71,100,-88});
    rules[594] = new Rule(-72, new int[]{-89});
    rules[595] = new Rule(-72, new int[]{-72,100,-89});
    rules[596] = new Rule(-59, new int[]{-174});
    rules[597] = new Rule(-174, new int[]{-173});
    rules[598] = new Rule(-88, new int[]{-87});
    rules[599] = new Rule(-88, new int[]{-322});
    rules[600] = new Rule(-88, new int[]{42});
    rules[601] = new Rule(-89, new int[]{-87});
    rules[602] = new Rule(-89, new int[]{-147,110,-101});
    rules[603] = new Rule(-89, new int[]{-322});
    rules[604] = new Rule(-89, new int[]{42});
    rules[605] = new Rule(-87, new int[]{-101});
    rules[606] = new Rule(-87, new int[]{-120});
    rules[607] = new Rule(-101, new int[]{-99});
    rules[608] = new Rule(-101, new int[]{-240});
    rules[609] = new Rule(-101, new int[]{-242});
    rules[610] = new Rule(-117, new int[]{-99});
    rules[611] = new Rule(-117, new int[]{-240});
    rules[612] = new Rule(-118, new int[]{-99});
    rules[613] = new Rule(-118, new int[]{-242});
    rules[614] = new Rule(-103, new int[]{-101});
    rules[615] = new Rule(-103, new int[]{-322});
    rules[616] = new Rule(-104, new int[]{-99});
    rules[617] = new Rule(-104, new int[]{-240});
    rules[618] = new Rule(-104, new int[]{-322});
    rules[619] = new Rule(-99, new int[]{-98});
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
			parserTools.AddErrorFromResource("UNSUPPORTED_OLD_DIRECTIVES",CurrentLocationSpan);
			CurrentSemanticValue.ob = null;
        }
        break;
      case 19: // one_compiler_directive -> tkDirectiveName, tkStringLiteral
{
			parserTools.AddErrorFromResource("UNSUPPORTED_OLD_DIRECTIVES",CurrentLocationSpan);
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
			if (parserTools.buildTreeForFormatter)
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
   			if (parserTools.buildTreeForFormatter)
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
      case 46: // used_unit_name -> tkStringLiteral
{ 
        	if (ValueStack[ValueStack.Depth-1].stn is char_const _cc)
        		ValueStack[ValueStack.Depth-1].stn = new string_const(_cc.cconst.ToString());
			CurrentSemanticValue.stn = new uses_unit_in(null, ValueStack[ValueStack.Depth-1].stn as string_const, CurrentLocationSpan);
        }
        break;
      case 47: // used_unit_name -> ident_or_keyword_pointseparator_list, tkIn, tkStringLiteral
{ 
        	if (ValueStack[ValueStack.Depth-1].stn is char_const _cc)
        		ValueStack[ValueStack.Depth-1].stn = new string_const(_cc.cconst.ToString());
			CurrentSemanticValue.stn = new uses_unit_in(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].stn as string_const, CurrentLocationSpan);
        }
        break;
      case 48: // unit_file -> unit_header, interface_part, implementation_part, 
               //              initialization_part, tkPoint
{ 
			CurrentSemanticValue.stn = new unit_module(ValueStack[ValueStack.Depth-5].stn as unit_name, ValueStack[ValueStack.Depth-4].stn as interface_node, ValueStack[ValueStack.Depth-3].stn as implementation_node, 
			  (ValueStack[ValueStack.Depth-2].stn as initfinal_part).initialization_sect, (ValueStack[ValueStack.Depth-2].stn as initfinal_part).finalization_sect, /*$1 as attribute_list*/ null, CurrentLocationSpan);   
			(CurrentSemanticValue.stn as compilation_unit).Language = PascalABCCompiler.StringConstants.pascalLanguageName;                
		}
        break;
      case 49: // unit_file -> unit_header, abc_interface_part, initialization_part, tkPoint
{ 
			CurrentSemanticValue.stn = new unit_module(ValueStack[ValueStack.Depth-4].stn as unit_name, ValueStack[ValueStack.Depth-3].stn as interface_node, null, 
			  (ValueStack[ValueStack.Depth-2].stn as initfinal_part).initialization_sect, (ValueStack[ValueStack.Depth-2].stn as initfinal_part).finalization_sect, /*$1 as attribute_list*/ null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as compilation_unit).Language = PascalABCCompiler.StringConstants.pascalLanguageName;   
        }
        break;
      case 50: // unit_header -> unit_key_word, unit_name, tkSemiColon, 
               //                optional_head_compiler_directives
{ 
			CurrentSemanticValue.stn = NewUnitHeading(new ident(ValueStack[ValueStack.Depth-4].ti.text, LocationStack[LocationStack.Depth-4]), ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan); 
		}
        break;
      case 51: // unit_header -> tkNamespace, ident_or_keyword_pointseparator_list, tkSemiColon, 
               //                optional_head_compiler_directives
{
            CurrentSemanticValue.stn = NewNamespaceHeading(new ident(ValueStack[ValueStack.Depth-4].ti.text, LocationStack[LocationStack.Depth-4]), ValueStack[ValueStack.Depth-3].stn as ident_list, CurrentLocationSpan);
        }
        break;
      case 52: // unit_key_word -> tkUnit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 53: // unit_key_word -> tkLibrary
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 54: // unit_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 55: // interface_part -> tkInterface, uses_clause, interface_decl_sect_list
{ 
			CurrentSemanticValue.stn = new interface_node(ValueStack[ValueStack.Depth-1].stn as declarations, ValueStack[ValueStack.Depth-2].stn as uses_list, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1])); 
        }
        break;
      case 56: // implementation_part -> tkImplementation, uses_clause, decl_sect_list
{ 
			CurrentSemanticValue.stn = new implementation_node(ValueStack[ValueStack.Depth-2].stn as uses_list, ValueStack[ValueStack.Depth-1].stn as declarations, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1])); 
        }
        break;
      case 57: // abc_interface_part -> uses_clause, decl_sect_list
{ 
			CurrentSemanticValue.stn = new interface_node(ValueStack[ValueStack.Depth-1].stn as declarations, ValueStack[ValueStack.Depth-2].stn as uses_list, null, null); 
        }
        break;
      case 58: // initialization_part -> tkEnd
{ 
			CurrentSemanticValue.stn = new initfinal_part(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 59: // initialization_part -> tkInitialization, stmt_list, tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, null, null, CurrentLocationSpan);
        }
        break;
      case 60: // initialization_part -> tkInitialization, stmt_list, tkFinalization, stmt_list, 
               //                        tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-5].ti, ValueStack[ValueStack.Depth-4].stn as statement_list, ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, CurrentLocationSpan);
        }
        break;
      case 61: // initialization_part -> tkBegin, stmt_list, tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, null, null, CurrentLocationSpan);
        }
        break;
      case 62: // interface_decl_sect_list -> int_decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 63: // int_decl_sect_list1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations();  
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 64: // int_decl_sect_list1 -> int_decl_sect_list1, int_decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 65: // decl_sect_list -> decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 66: // decl_sect_list_proc_func_only -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations(); 
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 67: // decl_sect_list_proc_func_only -> decl_sect_list_proc_func_only, 
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
      case 68: // decl_sect_list1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations(); 
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 69: // decl_sect_list1 -> decl_sect_list1, decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 70: // inclass_decl_sect_list -> inclass_decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 71: // inclass_decl_sect_list1 -> /* empty */
{ 
        	CurrentSemanticValue.stn = new declarations(); 
        }
        break;
      case 72: // inclass_decl_sect_list1 -> inclass_decl_sect_list1, abc_decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 73: // int_decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 74: // int_decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 75: // int_decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 76: // int_decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 77: // int_decl_sect -> int_proc_header
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 78: // int_decl_sect -> int_func_header
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 79: // decl_sect -> label_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 80: // decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 81: // decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 82: // decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 83: // decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 84: // decl_sect -> proc_func_constr_destr_decl_with_attr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 85: // proc_func_constr_destr_decl -> proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 86: // proc_func_constr_destr_decl -> constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 87: // proc_func_constr_destr_decl_with_attr -> attribute_declarations, 
               //                                          proc_func_constr_destr_decl
{
		    (ValueStack[ValueStack.Depth-1].stn as procedure_definition).AssignAttrList(ValueStack[ValueStack.Depth-2].stn as attribute_list);
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 88: // abc_decl_sect -> label_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 89: // abc_decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 90: // abc_decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 91: // abc_decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 92: // abc_decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 93: // int_proc_header -> attribute_declarations, proc_header
{  
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
			(CurrentSemanticValue.td as procedure_header).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
        }
        break;
      case 94: // int_proc_header -> attribute_declarations, proc_header, tkForward, tkSemiColon
{  
			CurrentSemanticValue.td = NewProcedureHeader(ValueStack[ValueStack.Depth-4].stn as attribute_list, ValueStack[ValueStack.Depth-3].td as procedure_header, ValueStack[ValueStack.Depth-2].id as procedure_attribute, CurrentLocationSpan);
		}
        break;
      case 95: // int_func_header -> attribute_declarations, func_header
{  
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
			(CurrentSemanticValue.td as procedure_header).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
        }
        break;
      case 96: // int_func_header -> attribute_declarations, func_header, tkForward, tkSemiColon
{  
			CurrentSemanticValue.td = NewProcedureHeader(ValueStack[ValueStack.Depth-4].stn as attribute_list, ValueStack[ValueStack.Depth-3].td as procedure_header, ValueStack[ValueStack.Depth-2].id as procedure_attribute, CurrentLocationSpan);
		}
        break;
      case 97: // label_decl_sect -> tkLabel, label_list, tkSemiColon
{ 
			CurrentSemanticValue.stn = new label_definitions(ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
		}
        break;
      case 98: // label_list -> label_name
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 99: // label_list -> label_list, tkComma, label_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 100: // label_name -> tkInteger
{ 
			CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ex.ToString(), CurrentLocationSpan);
		}
        break;
      case 101: // label_name -> identifier
{ 
			CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; 
		}
        break;
      case 102: // const_decl_sect -> tkConst, const_decl
{ 
			CurrentSemanticValue.stn = new consts_definitions_list(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
		}
        break;
      case 103: // const_decl_sect -> const_decl_sect, const_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as consts_definitions_list).Add(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
		}
        break;
      case 104: // res_str_decl_sect -> tkResourceString, const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 105: // res_str_decl_sect -> res_str_decl_sect, const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 106: // type_decl_sect -> tkType, type_decl
{ 
            CurrentSemanticValue.stn = new type_declarations(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
		}
        break;
      case 107: // type_decl_sect -> type_decl_sect, type_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as type_declarations).Add(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
		}
        break;
      case 108: // var_decl_with_assign_var_tuple -> var_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 109: // var_decl_with_assign_var_tuple -> tkRoundOpen, identifier, tkComma, ident_list, 
                //                                   tkRoundClose, tkAssign, expr_l1, 
                //                                   tkSemiColon
{
			(ValueStack[ValueStack.Depth-5].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-7].id);
			ValueStack[ValueStack.Depth-5].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]);
			CurrentSemanticValue.stn = new var_tuple_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
		}
        break;
      case 110: // var_decl_sect -> tkVar, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 111: // var_decl_sect -> tkEvent, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);                        
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).is_event = true;
        }
        break;
      case 112: // var_decl_sect -> var_decl_sect, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as variable_definitions).Add(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 113: // const_decl -> only_const_decl, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 114: // only_const_decl -> const_name, tkEqual, init_const_expr
{ 
			CurrentSemanticValue.stn = new simple_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 115: // only_const_decl -> const_name, tkColon, type_ref, tkEqual, typed_const
{ 
			CurrentSemanticValue.stn = new typed_const_definition(ValueStack[ValueStack.Depth-5].id, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-3].td, CurrentLocationSpan);
		}
        break;
      case 116: // init_const_expr -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 117: // init_const_expr -> array_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 118: // const_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 119: // const_relop_expr -> const_simple_expr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 120: // const_relop_expr -> const_relop_expr, const_relop, const_simple_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 121: // const_expr -> const_relop_expr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 122: // const_expr -> question_constexpr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 123: // const_expr -> const_expr, tkDoubleQuestion, const_relop_expr
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 124: // question_constexpr -> const_expr, tkQuestion, const_expr, tkColon, const_expr
{ CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 125: // const_relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 126: // const_relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 127: // const_relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 128: // const_relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 129: // const_relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 130: // const_relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 131: // const_relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 132: // const_simple_expr -> const_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 133: // const_simple_expr -> const_simple_expr, const_addop, const_term
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 134: // const_addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 135: // const_addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 136: // const_addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 137: // const_addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 138: // as_is_constexpr -> const_term, typecast_op, simple_or_template_type_reference
{ 
			CurrentSemanticValue.ex = NewAsIsConstexpr(ValueStack[ValueStack.Depth-3].ex, (op_typecast)ValueStack[ValueStack.Depth-2].ob, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);                                
		}
        break;
      case 139: // power_constexpr -> const_factor_without_unary_op, tkStarStar, const_factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 140: // power_constexpr -> const_factor_without_unary_op, tkStarStar, power_constexpr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 141: // power_constexpr -> sign, power_constexpr
{ CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 142: // const_term -> const_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 143: // const_term -> as_is_constexpr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 144: // const_term -> power_constexpr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 145: // const_term -> const_term, const_mulop, const_factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 146: // const_term -> const_term, const_mulop, power_constexpr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 147: // const_mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 148: // const_mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 149: // const_mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 150: // const_mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 151: // const_mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 152: // const_mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 153: // const_mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 154: // const_factor_without_unary_op -> const_variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 155: // const_factor_without_unary_op -> tkRoundOpen, const_expr, tkRoundClose
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 156: // const_factor -> const_variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 157: // const_factor -> const_set
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 158: // const_factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 159: // const_factor -> tkAddressOf, const_factor
{ 
			CurrentSemanticValue.ex = new get_address(ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);  
		}
        break;
      case 160: // const_factor -> tkRoundOpen, const_expr, tkRoundClose
{ 
	 	    CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 161: // const_factor -> tkNot, const_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 162: // const_factor -> sign, const_factor
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
					parserTools.AddErrorFromResource("BAD_INT2",CurrentLocationSpan);
					break;
				}
			    // можно сдела�?�? в�?�?исление конс�?ан�?�? с вмон�?и�?ованн�?м мин�?сом
			}
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 163: // const_factor -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 164: // const_factor -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 165: // const_set -> tkSquareOpen, elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new pascal_set_constant(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 166: // const_set -> tkVertParen, elem_list, tkVertParen
{ 
			CurrentSemanticValue.ex = new array_const_new(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 167: // sign -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 168: // sign -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 169: // const_variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 170: // const_variable -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 171: // const_variable -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 172: // const_variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 173: // const_variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 174: // const_variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 175: // const_variable -> const_variable, const_variable_2
{
			CurrentSemanticValue.ex = NewConstVariable(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 176: // const_variable -> const_variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 177: // const_variable -> const_variable, tkSquareOpen, format_const_expr, 
                //                   tkSquareClose
{ 
    		var fe = ValueStack[ValueStack.Depth-2].ex as format_expr;
            if (!parserTools.buildTreeForFormatter)
            {
                if (fe.expr == null)
                    fe.expr = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
                if (fe.format1 == null)
                    fe.format1 = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
            }
    		CurrentSemanticValue.ex = new slice_expr(ValueStack[ValueStack.Depth-4].ex as addressed_value,fe.expr,fe.format1,fe.format2,CurrentLocationSpan);
		}
        break;
      case 178: // const_variable_2 -> tkPoint, identifier_or_keyword
{ 
			CurrentSemanticValue.ex = new dot_node(null, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 179: // const_variable_2 -> tkDeref
{ 
			CurrentSemanticValue.ex = new roof_dereference();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 180: // const_variable_2 -> tkRoundOpen, optional_const_func_expr_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 181: // const_variable_2 -> tkSquareOpen, const_elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new indexer(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 182: // optional_const_func_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 183: // optional_const_func_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 184: // const_elem_list -> const_elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 186: // const_elem_list1 -> const_elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 187: // const_elem_list1 -> const_elem_list1, tkComma, const_elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 188: // const_elem -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 189: // const_elem -> const_expr, tkDotDot, const_expr
{ 
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 190: // unsigned_number -> tkInteger
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 191: // unsigned_number -> tkHex
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 192: // unsigned_number -> tkFloat
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 193: // unsigned_number -> tkBigInteger
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 194: // typed_const -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 195: // typed_const -> array_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 196: // typed_const -> record_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 197: // array_const -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new array_const(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 199: // typed_const_list -> typed_const_list1
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 200: // typed_const_list1 -> typed_const_plus
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 201: // typed_const_list1 -> typed_const_list1, tkComma, typed_const_plus
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 202: // record_const -> tkRoundOpen, const_field_list, tkRoundClose
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 203: // const_field_list -> const_field_list_1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 204: // const_field_list -> const_field_list_1, tkSemiColon
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 205: // const_field_list_1 -> const_field
{ 
			CurrentSemanticValue.ex = new record_const(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
		}
        break;
      case 206: // const_field_list_1 -> const_field_list_1, tkSemiColon, const_field
{ 
			CurrentSemanticValue.ex = (ValueStack[ValueStack.Depth-3].ex as record_const).Add(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
		}
        break;
      case 207: // const_field -> const_field_name, tkColon, typed_const
{ 
			CurrentSemanticValue.stn = new record_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 208: // const_field_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 209: // type_decl -> attribute_declarations, simple_type_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = LocationStack[LocationStack.Depth-1];
        }
        break;
      case 210: // attribute_declarations -> attribute_declaration
{ 
			CurrentSemanticValue.stn = new attribute_list(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
    }
        break;
      case 211: // attribute_declarations -> attribute_declarations, attribute_declaration
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
		}
        break;
      case 212: // attribute_declarations -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 213: // attribute_declaration -> tkSquareOpen, one_or_some_attribute, tkSquareClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 214: // one_or_some_attribute -> one_attribute
{ 
			CurrentSemanticValue.stn = new simple_attribute_list(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
		}
        break;
      case 215: // one_or_some_attribute -> one_or_some_attribute, tkComma, one_attribute
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as simple_attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
		}
        break;
      case 216: // one_attribute -> attribute_variable
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 217: // one_attribute -> identifier, tkColon, attribute_variable
{  
			(ValueStack[ValueStack.Depth-1].stn as attribute).qualifier = ValueStack[ValueStack.Depth-3].id;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 218: // simple_type_decl -> type_decl_identifier, tkEqual, type_decl_type, tkSemiColon
{ 
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-4].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
		}
        break;
      case 219: // simple_type_decl -> template_identifier_with_equal, type_decl_type, tkSemiColon
{ 
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
		}
        break;
      case 220: // type_decl_identifier -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 221: // type_decl_identifier -> identifier, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 222: // template_identifier_with_equal -> identifier, tkLower, ident_list, 
                //                                   tkGreaterEqual
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-4].id.name, ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 223: // type_decl_type -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 224: // type_decl_type -> object_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 225: // simple_type_question -> simple_type, tkQuestion
{
            if (parserTools.buildTreeForFormatter)
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
      case 226: // simple_type_question -> template_type, tkQuestion
{
            if (parserTools.buildTreeForFormatter)
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
      case 227: // type_ref -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 228: // type_ref -> simple_type_question
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 229: // type_ref -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 230: // type_ref -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 231: // type_ref -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 232: // type_ref -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 233: // type_ref -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 234: // template_type -> simple_type_identifier, template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference(ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan); 
		}
        break;
      case 235: // template_type_params -> tkLower, template_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 236: // template_type_empty_params -> tkNotEqual
{
            var ntr = new named_type_reference(new ident(""), CurrentLocationSpan);
            
			CurrentSemanticValue.stn = new template_param_list(ntr, CurrentLocationSpan);
            ntr.source_context = new SourceContext(CurrentSemanticValue.stn.source_context.end_position.line_num, CurrentSemanticValue.stn.source_context.end_position.column_num, CurrentSemanticValue.stn.source_context.begin_position.line_num, CurrentSemanticValue.stn.source_context.begin_position.column_num);
		}
        break;
      case 237: // template_type_empty_params -> tkLower, template_empty_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 238: // template_param_list -> template_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 239: // template_param_list -> template_param_list, tkComma, template_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 240: // template_empty_param_list -> template_empty_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 241: // template_empty_param_list -> template_empty_param_list, tkComma, 
                //                              template_empty_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 242: // template_empty_param -> /* empty */
{ 
            CurrentSemanticValue.td = new named_type_reference(new ident(""), CurrentLocationSpan);
        }
        break;
      case 243: // template_param -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 244: // template_param -> simple_type, tkQuestion
{
            if (parserTools.buildTreeForFormatter)
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
      case 245: // template_param -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 246: // template_param -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 247: // template_param -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 248: // simple_type -> range_expr
{
	    	CurrentSemanticValue.td = parserTools.ConvertDotNodeOrIdentToNamedTypeReference(ValueStack[ValueStack.Depth-1].ex); 
	    }
        break;
      case 249: // simple_type -> range_expr, tkDotDot, range_expr
{ 
			CurrentSemanticValue.td = new diapason(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 250: // simple_type -> tkRoundOpen, enumeration_id_list, tkRoundClose
{ 
			CurrentSemanticValue.td = new enum_type_definition(ValueStack[ValueStack.Depth-2].stn as enumerator_list, CurrentLocationSpan);  
		}
        break;
      case 251: // range_expr -> range_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 252: // range_expr -> range_expr, const_addop, range_term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 253: // range_term -> range_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 254: // range_term -> range_term, const_mulop, range_factor
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 255: // range_factor -> simple_type_identifier
{ 
			CurrentSemanticValue.ex = parserTools.ConvertNamedTypeReferenceToDotNodeOrIdent(ValueStack[ValueStack.Depth-1].td as named_type_reference);
        }
        break;
      case 256: // range_factor -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 257: // range_factor -> sign, range_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 258: // range_factor -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 259: // range_factor -> range_factor, tkRoundOpen, const_elem_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value, ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 260: // simple_type_identifier -> identifier
{ 
			CurrentSemanticValue.td = new named_type_reference(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 261: // simple_type_identifier -> simple_type_identifier, tkPoint, 
                //                           identifier_or_keyword
{ 
			CurrentSemanticValue.td = (ValueStack[ValueStack.Depth-3].td as named_type_reference).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 262: // enumeration_id_list -> enumeration_id
{ 
			CurrentSemanticValue.stn = new enumerator_list(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 263: // enumeration_id_list -> enumeration_id_list, tkComma, enumeration_id
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as enumerator_list).Add(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 264: // enumeration_id -> type_ref
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan); 
		}
        break;
      case 265: // enumeration_id -> type_ref, tkEqual, expr
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 266: // pointer_type -> tkDeref, fptype
{ 
			CurrentSemanticValue.td = new ref_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 267: // structured_type -> array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 268: // structured_type -> record_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 269: // structured_type -> set_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 270: // structured_type -> file_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 271: // structured_type -> sequence_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 272: // sequence_type -> tkSequence, tkOf, type_ref
{
			CurrentSemanticValue.td = new sequence_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 273: // array_type -> tkArray, tkSquareOpen, simple_type_list, tkSquareClose, tkOf, 
                //               type_ref
{ 
			CurrentSemanticValue.td = new array_type(ValueStack[ValueStack.Depth-4].stn as indexers_types, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 274: // array_type -> unsized_array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 275: // unsized_array_type -> tkArray, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new array_type(null, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 276: // simple_type_list -> simple_type_or_
{ 
			CurrentSemanticValue.stn = new indexers_types(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 277: // simple_type_list -> simple_type_list, tkComma, simple_type_or_
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as indexers_types).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 278: // simple_type_or_ -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 279: // simple_type_or_ -> /* empty */
{ CurrentSemanticValue.td = null; }
        break;
      case 280: // set_type -> tkSet, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new set_type_definition(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 281: // file_type -> tkFile, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new file_type(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 282: // file_type -> tkFile
{ 
			CurrentSemanticValue.td = new file_type();  
			CurrentSemanticValue.td.source_context = CurrentLocationSpan;
		}
        break;
      case 283: // string_type -> tkIdentifier, tkSquareOpen, const_expr, tkSquareClose
{ 
			CurrentSemanticValue.td = new string_num_definition(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-4].id, CurrentLocationSpan);
		}
        break;
      case 284: // procedural_type -> procedural_type_kind
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 285: // procedural_type_kind -> proc_type_decl
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 286: // proc_type_decl -> tkProcedure, fp_list
{ 
			CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-1].stn as formal_parameters,null,null,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 287: // proc_type_decl -> tkFunction, fp_list, tkColon, fptype
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, null, null, null, ValueStack[ValueStack.Depth-1].td as type_definition, CurrentLocationSpan);
        }
        break;
      case 288: // proc_type_decl -> simple_type_identifier, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 289: // proc_type_decl -> template_type, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 290: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 291: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-4].stn as enumerator_list,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 292: // proc_type_decl -> simple_type_identifier, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 293: // proc_type_decl -> template_type, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 294: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,null,CurrentLocationSpan);
    	}
        break;
      case 295: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-5].stn as enumerator_list,null,CurrentLocationSpan);
    	}
        break;
      case 296: // object_type -> class_attributes, class_or_interface_keyword, 
                //                optional_base_classes, optional_where_section, 
                //                optional_component_list_seq_end
{ 
            var cd = NewObjectType((class_attribute)ValueStack[ValueStack.Depth-5].ob, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].stn as named_type_reference_list, ValueStack[ValueStack.Depth-2].stn as where_definition_list, ValueStack[ValueStack.Depth-1].stn as class_body_list, CurrentLocationSpan); 
			CurrentSemanticValue.td = cd;
		}
        break;
      case 297: // record_type -> tkRecord, optional_base_classes, optional_where_section, 
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
      case 298: // class_attribute -> tkSealed
{ CurrentSemanticValue.ob = class_attribute.Sealed; }
        break;
      case 299: // class_attribute -> tkPartial
{ CurrentSemanticValue.ob = class_attribute.Partial; }
        break;
      case 300: // class_attribute -> tkAbstract
{ CurrentSemanticValue.ob = class_attribute.Abstract; }
        break;
      case 301: // class_attribute -> tkAuto
{ CurrentSemanticValue.ob = class_attribute.Auto; }
        break;
      case 302: // class_attribute -> tkStatic
{ CurrentSemanticValue.ob = class_attribute.Static; }
        break;
      case 303: // class_attributes -> /* empty */
{ 
			CurrentSemanticValue.ob = class_attribute.None; 
		}
        break;
      case 304: // class_attributes -> class_attributes1
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 305: // class_attributes1 -> class_attribute
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 306: // class_attributes1 -> class_attributes1, class_attribute
{
            if (((class_attribute)ValueStack[ValueStack.Depth-2].ob & (class_attribute)ValueStack[ValueStack.Depth-1].ob) == (class_attribute)ValueStack[ValueStack.Depth-1].ob)
                parserTools.AddErrorFromResource("ATTRIBUTE_REDECLARED",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.ob  = ((class_attribute)ValueStack[ValueStack.Depth-2].ob) | ((class_attribute)ValueStack[ValueStack.Depth-1].ob);
			//$$ = $1;
		}
        break;
      case 307: // class_or_interface_keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 308: // class_or_interface_keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 309: // class_or_interface_keyword -> tkTemplate
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-1].ti);
		}
        break;
      case 310: // class_or_interface_keyword -> tkTemplate, tkClass
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "c", CurrentLocationSpan);
		}
        break;
      case 311: // class_or_interface_keyword -> tkTemplate, tkRecord
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "r", CurrentLocationSpan);
		}
        break;
      case 312: // class_or_interface_keyword -> tkTemplate, tkInterface
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "i", CurrentLocationSpan);
		}
        break;
      case 313: // optional_component_list_seq_end -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 314: // optional_component_list_seq_end -> member_list_section, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 316: // optional_base_classes -> tkRoundOpen, base_classes_names_list, tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 317: // base_classes_names_list -> base_class_name
{ 
			CurrentSemanticValue.stn = new named_type_reference_list(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 318: // base_classes_names_list -> base_classes_names_list, tkComma, base_class_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as named_type_reference_list).Add(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 319: // base_class_name -> simple_type_identifier
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 320: // base_class_name -> template_type
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 321: // template_arguments -> tkLower, ident_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 322: // optional_where_section -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 323: // optional_where_section -> where_part_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 324: // where_part_list -> where_part
{ 
			CurrentSemanticValue.stn = new where_definition_list(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 325: // where_part_list -> where_part_list, where_part
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as where_definition_list).Add(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 326: // where_part -> tkWhere, ident_list, tkColon, type_ref_and_secific_list, 
                //               tkSemiColon
{ 
			CurrentSemanticValue.stn = new where_definition(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-2].stn as where_type_specificator_list, CurrentLocationSpan); 
		}
        break;
      case 327: // type_ref_and_secific_list -> type_ref_or_secific
{ 
			CurrentSemanticValue.stn = new where_type_specificator_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 328: // type_ref_and_secific_list -> type_ref_and_secific_list, tkComma, 
                //                              type_ref_or_secific
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as where_type_specificator_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 329: // type_ref_or_secific -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 330: // type_ref_or_secific -> tkClass
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefClass, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 331: // type_ref_or_secific -> tkRecord
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefValueType, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 332: // type_ref_or_secific -> tkConstructor
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 333: // member_list_section -> member_list
{ 
			CurrentSemanticValue.stn = new class_body_list(ValueStack[ValueStack.Depth-1].stn as class_members, CurrentLocationSpan);
        }
        break;
      case 334: // member_list_section -> member_list_section, ot_visibility_specifier, 
                //                        member_list
{ 
		    (ValueStack[ValueStack.Depth-1].stn as class_members).access_mod = ValueStack[ValueStack.Depth-2].stn as access_modifer_node;
			(ValueStack[ValueStack.Depth-3].stn as class_body_list).Add(ValueStack[ValueStack.Depth-1].stn as class_members,CurrentLocationSpan);
			
			if ((ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks[0].Count == 0)
                (ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks.RemoveAt(0);
			
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 335: // ot_visibility_specifier -> tkInternal
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.internal_modifer, CurrentLocationSpan); }
        break;
      case 336: // ot_visibility_specifier -> tkPublic
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.public_modifer, CurrentLocationSpan); }
        break;
      case 337: // ot_visibility_specifier -> tkProtected
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.protected_modifer, CurrentLocationSpan); }
        break;
      case 338: // ot_visibility_specifier -> tkPrivate
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.private_modifer, CurrentLocationSpan); }
        break;
      case 339: // member_list -> /* empty */
{ CurrentSemanticValue.stn = new class_members(); }
        break;
      case 340: // member_list -> field_or_const_definition_list, optional_semicolon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 341: // member_list -> method_decl_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 342: // member_list -> field_or_const_definition_list, tkSemiColon, method_decl_list
{  
			(ValueStack[ValueStack.Depth-3].stn as class_members).members.AddRange((ValueStack[ValueStack.Depth-1].stn as class_members).members);
			(ValueStack[ValueStack.Depth-3].stn as class_members).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 343: // ident_list -> identifier
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 344: // ident_list -> ident_list, tkComma, identifier
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 345: // optional_semicolon -> /* empty */
{ CurrentSemanticValue.ob = null; }
        break;
      case 346: // optional_semicolon -> tkSemiColon
{ CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 347: // field_or_const_definition_list -> field_or_const_definition
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 348: // field_or_const_definition_list -> field_or_const_definition_list, tkSemiColon, 
                //                                   field_or_const_definition
{   
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 349: // field_or_const_definition -> attribute_declarations, 
                //                              simple_field_or_const_definition
{  
		    (ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 350: // method_decl_list -> method_or_property_decl
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 351: // method_decl_list -> method_decl_list, method_or_property_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 352: // method_or_property_decl -> method_decl_withattr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 353: // method_or_property_decl -> property_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 354: // simple_field_or_const_definition -> tkConst, only_const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 355: // simple_field_or_const_definition -> field_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 356: // simple_field_or_const_definition -> class_or_static, field_definition
{ 
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).var_attr = definition_attribute.Static;
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 357: // class_or_static -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 358: // class_or_static -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 359: // field_definition -> var_decl_part
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 360: // field_definition -> tkEvent, ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, true, CurrentLocationSpan); 
        }
        break;
      case 361: // method_decl_withattr -> attribute_declarations, method_header
{  
			(ValueStack[ValueStack.Depth-1].td as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td;
        }
        break;
      case 362: // method_decl_withattr -> attribute_declarations, method_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
            if (ValueStack[ValueStack.Depth-1].stn is procedure_definition && (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
                (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
     }
        break;
      case 363: // method_decl -> inclass_proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 364: // method_decl -> inclass_constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 365: // method_header -> class_or_static, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 366: // method_header -> method_procfunc_header
{ 
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; 
		}
        break;
      case 367: // method_header -> tkAsync, class_or_static, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
			(ValueStack[ValueStack.Depth-1].td as procedure_header).IsAsync = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 368: // method_header -> tkAsync, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).IsAsync = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; 
		}
        break;
      case 369: // method_header -> constr_destr_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 370: // method_procfunc_header -> proc_func_header
{ 
			CurrentSemanticValue.td = NewProcfuncHeading(ValueStack[ValueStack.Depth-1].td as procedure_header);
		}
        break;
      case 371: // proc_func_header -> proc_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 372: // proc_func_header -> func_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 373: // constr_destr_header -> tkConstructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 374: // constr_destr_header -> class_or_static, tkConstructor, optional_proc_name, 
                //                        fp_list, optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,true,null,null,CurrentLocationSpan);
        }
        break;
      case 375: // constr_destr_header -> tkDestructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new destructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name, false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 376: // optional_proc_name -> proc_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 377: // optional_proc_name -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 378: // property_definition -> attribute_declarations, simple_property_definition
{  
			CurrentSemanticValue.stn = NewPropertyDefinition(ValueStack[ValueStack.Depth-2].stn as attribute_list, ValueStack[ValueStack.Depth-1].stn as declaration, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 379: // simple_property_definition -> tkProperty, func_name, property_interface, 
                //                               property_specifiers, tkSemiColon, 
                //                               array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        }
        break;
      case 380: // simple_property_definition -> tkProperty, func_name, property_interface, 
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
      case 381: // simple_property_definition -> class_or_static, tkProperty, func_name, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        	(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
        }
        break;
      case 382: // simple_property_definition -> class_or_static, tkProperty, func_name, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, property_modificator, tkSemiColon, 
                //                               array_defaultproperty
{ 
			parserTools.AddErrorFromResource("STATIC_PROPERTIES_CANNOT_HAVE_ATTRBUTE_{0}",LocationStack[LocationStack.Depth-3],ValueStack[ValueStack.Depth-3].id.name);        	
        }
        break;
      case 383: // simple_property_definition -> tkAuto, tkProperty, func_name, property_interface, 
                //                               optional_property_initialization, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
		}
        break;
      case 384: // simple_property_definition -> class_or_static, tkAuto, tkProperty, func_name, 
                //                               property_interface, 
                //                               optional_property_initialization, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
		}
        break;
      case 385: // optional_property_initialization -> tkAssign, expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 386: // optional_property_initialization -> /* empty */
{ CurrentSemanticValue.ex = null; }
        break;
      case 387: // array_defaultproperty -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 388: // array_defaultproperty -> tkDefault, tkSemiColon
{ 
			CurrentSemanticValue.stn = new property_array_default();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 389: // property_interface -> property_parameter_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new property_interface(ValueStack[ValueStack.Depth-3].stn as property_parameter_list, ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan);
        }
        break;
      case 390: // property_parameter_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 391: // property_parameter_list -> tkSquareOpen, parameter_decl_list, tkSquareClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 392: // parameter_decl_list -> parameter_decl
{ 
			CurrentSemanticValue.stn = new property_parameter_list(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
		}
        break;
      case 393: // parameter_decl_list -> parameter_decl_list, tkSemiColon, parameter_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as property_parameter_list).Add(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
		}
        break;
      case 394: // parameter_decl -> ident_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new property_parameter(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 395: // optional_read_expr -> expr_with_func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 396: // optional_read_expr -> /* empty */
{ CurrentSemanticValue.ex = null; }
        break;
      case 398: // property_specifiers -> tkRead, optional_read_expr, write_property_specifiers
{ 
        	if (ValueStack[ValueStack.Depth-2].ex == null || ValueStack[ValueStack.Depth-2].ex is ident) // с�?анда�?�?н�?е свойс�?ва
        	{
        		CurrentSemanticValue.stn = NewPropertySpecifiersRead(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-2].ex as ident, null, null, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan);
        	}
        	else // �?ас�?и�?енн�?е свойс�?ва
        	{
				var id = NewId("#GetGen", LocationStack[LocationStack.Depth-2]);
                procedure_definition pr = null;
                if (!parserTools.buildTreeForFormatter)
                    pr = CreateAndAddToClassReadFunc(ValueStack[ValueStack.Depth-2].ex, id, LocationStack[LocationStack.Depth-2]);
				CurrentSemanticValue.stn = NewPropertySpecifiersRead(ValueStack[ValueStack.Depth-3].id, id, pr, ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan); // $2 пе�?еда�?�?ся для �?о�?ма�?и�?ования 
			}
        }
        break;
      case 399: // property_specifiers -> tkWrite, unlabelled_stmt, read_property_specifiers
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
                if (!parserTools.buildTreeForFormatter)
                    pr = CreateAndAddToClassWriteProc(ValueStack[ValueStack.Depth-2].stn as statement,id,LocationStack[LocationStack.Depth-2]);
                if (parserTools.buildTreeForFormatter)
					CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-3].id, id, pr, ValueStack[ValueStack.Depth-2].stn as statement, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan); // $2 пе�?еда�?�?ся для �?о�?ма�?и�?ования
				else CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-3].id, id, pr, null, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan); 	
			}
        }
        break;
      case 401: // write_property_specifiers -> tkWrite, unlabelled_stmt
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
                if (!parserTools.buildTreeForFormatter)
                    pr = CreateAndAddToClassWriteProc(ValueStack[ValueStack.Depth-1].stn as statement,id,LocationStack[LocationStack.Depth-1]);
                if (parserTools.buildTreeForFormatter)
					CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-2].id, id, pr, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
				else CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-2].id, id, pr, null, null, CurrentLocationSpan);	
			}
       }
        break;
      case 403: // read_property_specifiers -> tkRead, optional_read_expr
{ 
        	if (ValueStack[ValueStack.Depth-1].ex == null || ValueStack[ValueStack.Depth-1].ex is ident)
        	{
        		CurrentSemanticValue.stn = NewPropertySpecifiersRead(ValueStack[ValueStack.Depth-2].id, ValueStack[ValueStack.Depth-1].ex as ident, null, null, null, CurrentLocationSpan);
        	}
        	else 
        	{
				var id = NewId("#GetGen", LocationStack[LocationStack.Depth-1]);
                procedure_definition pr = null;
                if (!parserTools.buildTreeForFormatter)
                    pr = CreateAndAddToClassReadFunc(ValueStack[ValueStack.Depth-1].ex,id,LocationStack[LocationStack.Depth-1]);
				CurrentSemanticValue.stn = NewPropertySpecifiersRead(ValueStack[ValueStack.Depth-2].id, id, pr, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan);
			}
       }
        break;
      case 404: // var_decl -> var_decl_part, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 407: // var_decl_part -> ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, false, CurrentLocationSpan);
		}
        break;
      case 408: // var_decl_part -> ident_list, tkAssign, expr_with_func_decl_lambda
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, null, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan);		
		}
        break;
      case 409: // var_decl_part -> ident_list, tkColon, type_ref, tkAssignOrEqual, 
                //                  typed_var_init_expression
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan); 
		}
        break;
      case 410: // typed_var_init_expression -> typed_const_plus
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 411: // typed_var_init_expression -> const_simple_expr, tkDotDot, const_term
{ 
		if (parserTools.buildTreeForFormatter)
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		else 
			CurrentSemanticValue.ex = new diapason_expr_new(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); 
		}
        break;
      case 412: // typed_var_init_expression -> expl_func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 413: // typed_var_init_expression -> identifier, tkArrow, lambda_function_body
{  
			var idList = new ident_list(ValueStack[ValueStack.Depth-3].id, LocationStack[LocationStack.Depth-3]); 
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 414: // typed_var_init_expression -> tkRoundOpen, tkRoundClose, lambda_type_ref, 
                //                              tkArrow, lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 415: // typed_var_init_expression -> tkRoundOpen, typed_const_list, tkRoundClose, 
                //                              tkArrow, lambda_function_body
{  
		    var el = ValueStack[ValueStack.Depth-4].stn as expression_list;
		    var cnt = el.Count;
		    
			var idList = new ident_list();
			idList.source_context = LocationStack[LocationStack.Depth-4];
			
			for (int j = 0; j < cnt; j++)
			{
				if (!(el.expressions[j] is ident))
					parserTools.AddErrorFromResource("ONE_TKIDENTIFIER",el.expressions[j].source_context);
				idList.idents.Add(el.expressions[j] as ident);
			}	
				
			var any = new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-4]);	
				
			var formalPars = new formal_parameters(new typed_parameters(idList, any, parametr_kind.none, null, LocationStack[LocationStack.Depth-4]), LocationStack[LocationStack.Depth-4]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, any, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 416: // typed_var_init_expression -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 417: // typed_const_plus -> typed_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 418: // constr_destr_decl -> constr_destr_header, block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 419: // constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, tkAssign, 
                //                      unlabelled_stmt, tkSemiColon
{ 
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parserTools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,false,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-6].Merge(LocationStack[LocationStack.Depth-2]));
            if (parserTools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
        }
        break;
      case 420: // constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
                //                      fp_list, tkAssign, unlabelled_stmt, tkSemiColon
{ 
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parserTools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,true,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-2]));
            if (parserTools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
        }
        break;
      case 421: // inclass_constr_destr_decl -> constr_destr_header, inclass_block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 422: // inclass_constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, 
                //                              tkAssign, unlabelled_stmt, tkSemiColon
{ 
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parserTools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,false,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-6].Merge(LocationStack[LocationStack.Depth-2]));
            if (parserTools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
        }
        break;
      case 423: // inclass_constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
                //                              fp_list, tkAssign, unlabelled_stmt, tkSemiColon
{ 
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parserTools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,true,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-2]));
            if (parserTools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
        }
        break;
      case 424: // proc_func_decl -> proc_func_decl_noclass
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 425: // proc_func_decl -> class_or_static, proc_func_decl_noclass
{ 
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 426: // proc_func_decl -> tkAsync, proc_func_decl_noclass
{
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;		
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 427: // proc_func_decl -> tkAsync, class_or_static, proc_func_decl_noclass
{ 
        	(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 428: // proc_func_decl -> class_or_static, tkAsync, proc_func_decl_noclass
{ 
        	(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 429: // proc_func_decl_noclass -> proc_func_header, proc_func_external_block
{
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
        }
        break;
      case 430: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                           optional_method_modificators1, tkAssign, expr_l1, 
                //                           tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 431: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, expr_l1, 
                //                           tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].ex is dot_question_node)
				parserTools.AddErrorFromResource("DOT_QUECTION_IN_SHORT_FUN",LocationStack[LocationStack.Depth-2]);
	
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 432: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                           optional_method_modificators1, tkAssign, 
                //                           func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 433: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, 
                //                           func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 434: // proc_func_decl_noclass -> tkProcedure, proc_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, 
                //                           unlabelled_stmt, tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parserTools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-2]);
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 435: // proc_func_decl_noclass -> proc_func_header, tkForward, tkSemiColon
{
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-3].td as procedure_header, null, CurrentLocationSpan);
            (CurrentSemanticValue.stn as procedure_definition).proc_header.proc_attributes.Add((procedure_attribute)ValueStack[ValueStack.Depth-2].id, ValueStack[ValueStack.Depth-2].id.source_context);
		}
        break;
      case 436: // inclass_proc_func_decl -> inclass_proc_func_decl_noclass
{ 
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
        }
        break;
      case 437: // inclass_proc_func_decl -> tkAsync, inclass_proc_func_decl_noclass
{ 
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
        }
        break;
      case 438: // inclass_proc_func_decl -> class_or_static, inclass_proc_func_decl_noclass
{ 
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
		    {
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			}
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 439: // inclass_proc_func_decl -> tkAsync, class_or_static, 
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
      case 440: // inclass_proc_func_decl -> class_or_static, tkAsync, 
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
      case 441: // inclass_proc_func_decl_noclass -> proc_func_header, inclass_block
{
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
		}
        break;
      case 442: // inclass_proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, 
                //                                   fptype, optional_method_modificators1, 
                //                                   tkAssign, expr_l1_func_decl_lambda, 
                //                                   tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
			if (parserTools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 443: // inclass_proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                                   optional_method_modificators1, tkAssign, 
                //                                   expr_l1_func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
			if (parserTools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 444: // inclass_proc_func_decl_noclass -> tkProcedure, proc_name, fp_list, 
                //                                   optional_method_modificators1, tkAssign, 
                //                                   unlabelled_stmt, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
			if (parserTools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 445: // proc_func_external_block -> block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 446: // proc_func_external_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 447: // proc_name -> func_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 448: // func_name -> func_meth_name_ident
{ 
			CurrentSemanticValue.stn = new method_name(null,null, ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan); 
		}
        break;
      case 449: // func_name -> func_class_name_ident_list, tkPoint, func_meth_name_ident
{ 
        	var ln = ValueStack[ValueStack.Depth-3].ob as List<ident>;
        	var cnt = ln.Count;
        	if (cnt == 1)
				CurrentSemanticValue.stn = new method_name(null, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
			else 	
				CurrentSemanticValue.stn = new method_name(ln, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
		}
        break;
      case 450: // func_class_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 451: // func_class_name_ident_list -> func_class_name_ident
{ 
			CurrentSemanticValue.ob = new List<ident>(); 
			(CurrentSemanticValue.ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
		}
        break;
      case 452: // func_class_name_ident_list -> func_class_name_ident_list, tkPoint, 
                //                               func_class_name_ident
{ 
			(ValueStack[ValueStack.Depth-3].ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob; 
		}
        break;
      case 453: // func_meth_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 454: // func_meth_name_ident -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 455: // func_meth_name_ident -> operator_name_ident, template_arguments
{ CurrentSemanticValue.id = new template_operator_name(null, ValueStack[ValueStack.Depth-1].stn as ident_list, ValueStack[ValueStack.Depth-2].ex as operator_name_ident, CurrentLocationSpan); }
        break;
      case 456: // func_name_with_template_args -> func_name_ident
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 457: // func_name_with_template_args -> func_name_ident, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 458: // func_name_ident -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 459: // proc_header -> tkProcedure, proc_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{ 
        	CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, CurrentLocationSpan); 
        }
        break;
      case 460: // func_header -> tkFunction, func_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, null, CurrentLocationSpan); 
		}
        break;
      case 461: // func_header -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                optional_method_modificators, optional_where_section
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, ValueStack[ValueStack.Depth-3].td as type_definition, CurrentLocationSpan); 
        }
        break;
      case 462: // external_block -> tkExternal, external_directive_ident, tkName, 
                //                   external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 463: // external_block -> tkExternal, external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-2].ex, null, CurrentLocationSpan); 
		}
        break;
      case 464: // external_block -> tkExternal, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(null, null, CurrentLocationSpan); 
		}
        break;
      case 465: // external_directive_ident -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 466: // external_directive_ident -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 467: // block -> decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 468: // inclass_block -> inclass_decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 469: // inclass_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 470: // fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 471: // fp_list -> tkRoundOpen, tkRoundClose
{ 
			CurrentSemanticValue.stn = null;
		}
        break;
      case 472: // fp_list -> tkRoundOpen, fp_sect_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			if (CurrentSemanticValue.stn != null)
				CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 473: // fp_sect_list -> fp_sect
{ 
			CurrentSemanticValue.stn = new formal_parameters(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
        }
        break;
      case 474: // fp_sect_list -> fp_sect_list, tkSemiColon, fp_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);   
        }
        break;
      case 475: // fp_sect -> attribute_declarations, simple_fp_sect
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as  attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 476: // simple_fp_sect -> param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan); 
		}
        break;
      case 477: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.var_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 478: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.const_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 479: // simple_fp_sect -> tkParams, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td,parametr_kind.params_parametr,null, CurrentLocationSpan);  
		}
        break;
      case 480: // simple_fp_sect -> param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.none, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 481: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.var_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 482: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.const_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 483: // param_name_list -> param_name
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 484: // param_name_list -> param_name_list, tkComma, param_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);  
		}
        break;
      case 485: // param_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 486: // fptype -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 487: // fptype_noproctype -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 488: // fptype_noproctype -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 489: // fptype_noproctype -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 490: // fptype_noproctype -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 491: // fptype_noproctype -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 492: // stmt -> unlabelled_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 493: // stmt -> label_name, tkColon, stmt
{ 
			CurrentSemanticValue.stn = new labeled_statement(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);  
		}
        break;
      case 494: // unlabelled_stmt -> /* empty */
{ 
			CurrentSemanticValue.stn = new empty_statement(); 
			CurrentSemanticValue.stn.source_context = null;
		}
        break;
      case 495: // unlabelled_stmt -> assignment
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 496: // unlabelled_stmt -> proc_call
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 497: // unlabelled_stmt -> goto_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 498: // unlabelled_stmt -> compound_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 499: // unlabelled_stmt -> if_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 500: // unlabelled_stmt -> case_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 501: // unlabelled_stmt -> repeat_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 502: // unlabelled_stmt -> while_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 503: // unlabelled_stmt -> for_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 504: // unlabelled_stmt -> with_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 505: // unlabelled_stmt -> inherited_message
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 506: // unlabelled_stmt -> try_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 507: // unlabelled_stmt -> raise_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 508: // unlabelled_stmt -> foreach_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 509: // unlabelled_stmt -> var_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 510: // unlabelled_stmt -> expr_as_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 511: // unlabelled_stmt -> lock_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 512: // unlabelled_stmt -> yield_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 513: // unlabelled_stmt -> yield_sequence_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 514: // unlabelled_stmt -> loop_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 515: // unlabelled_stmt -> match_with
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 516: // loop_stmt -> tkLoop, expr_l1, tkDo, unlabelled_stmt
{
			CurrentSemanticValue.stn = new loop_stmt(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].stn as statement,CurrentLocationSpan);
		}
        break;
      case 517: // yield_stmt -> tkYield, expr_l1_func_decl_lambda
{
			CurrentSemanticValue.stn = new yield_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 518: // yield_sequence_stmt -> tkYield, tkSequence, expr_l1_func_decl_lambda
{
			CurrentSemanticValue.stn = new yield_sequence_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 519: // var_stmt -> tkVar, var_decl_part
{ 
			CurrentSemanticValue.stn = new var_statement(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 520: // var_stmt -> tkRoundOpen, tkVar, identifier, tkComma, var_ident_list, 
                //             tkRoundClose, tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].ob as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].ob as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 521: // var_stmt -> tkVar, tkRoundOpen, identifier, tkComma, ident_list, tkRoundClose, 
                //             tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			ValueStack[ValueStack.Depth-4].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
	    }
        break;
      case 522: // assignment -> var_reference, assign_operator, expr_with_func_decl_lambda
{      
        	if (!(ValueStack[ValueStack.Depth-3].ex is addressed_value))
        		parserTools.AddErrorFromResource("LEFT_SIDE_CANNOT_BE_ASSIGNED_TO",CurrentLocationSpan);
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan);
        }
        break;
      case 523: // assignment -> tkRoundOpen, variable, tkComma, variable_list, tkRoundClose, 
                //               assign_operator, expr
{
			if (ValueStack[ValueStack.Depth-2].op.type != Operators.Assignment)
			    parserTools.AddErrorFromResource("ONLY_BASE_ASSIGNMENT_FOR_TUPLE",LocationStack[LocationStack.Depth-2]);
			(ValueStack[ValueStack.Depth-4].ob as addressed_value_list).Insert(0,ValueStack[ValueStack.Depth-6].ex as addressed_value);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_tuple(ValueStack[ValueStack.Depth-4].ob as addressed_value_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 524: // variable_list -> variable
{
		CurrentSemanticValue.ob = new addressed_value_list(ValueStack[ValueStack.Depth-1].ex as addressed_value,LocationStack[LocationStack.Depth-1]);
	}
        break;
      case 525: // variable_list -> variable_list, tkComma, variable
{
		(ValueStack[ValueStack.Depth-3].ob as addressed_value_list).Add(ValueStack[ValueStack.Depth-1].ex as addressed_value);
		(ValueStack[ValueStack.Depth-3].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
	}
        break;
      case 526: // var_ident_list -> tkVar, identifier
{
		CurrentSemanticValue.ob = new ident_list(ValueStack[ValueStack.Depth-1].id,CurrentLocationSpan);
	}
        break;
      case 527: // var_ident_list -> var_ident_list, tkComma, tkVar, identifier
{
		(ValueStack[ValueStack.Depth-4].ob as ident_list).Add(ValueStack[ValueStack.Depth-1].id);
		(ValueStack[ValueStack.Depth-4].ob as ident_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-4].ob;
	}
        break;
      case 528: // proc_call -> var_reference
{ 
			CurrentSemanticValue.stn = new procedure_call(ValueStack[ValueStack.Depth-1].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex is ident, CurrentLocationSpan); 
		}
        break;
      case 529: // goto_stmt -> tkGoto, label_name
{ 
			CurrentSemanticValue.stn = new goto_statement(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 530: // compound_stmt -> tkBegin, stmt_list, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			(CurrentSemanticValue.stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(CurrentSemanticValue.stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 531: // stmt_list -> stmt
{ 
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 532: // stmt_list -> stmt_list, tkSemiColon, stmt
{  
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as statement_list).Add(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
        }
        break;
      case 533: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan); 
        }
        break;
      case 534: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt, tkElse, unlabelled_stmt
{
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as statement, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 535: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, else_case, tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 536: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, tkSemiColon, else_case, 
                //               tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 537: // pattern_cases -> pattern_case
{
            CurrentSemanticValue.stn = new pattern_cases(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 538: // pattern_cases -> pattern_cases, tkSemiColon, pattern_case
{
            CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as pattern_cases).Add(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 539: // pattern_case -> pattern_optional_var, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 540: // pattern_case -> deconstruction_or_const_pattern, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 541: // pattern_case -> collection_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 542: // pattern_case -> tuple_pattern, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
		}
        break;
      case 543: // pattern_case -> tuple_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 544: // case_stmt -> tkCase, expr_l1, tkOf, case_list, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 545: // case_stmt -> tkCase, expr_l1, tkOf, case_list, tkSemiColon, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 546: // case_stmt -> tkCase, expr_l1, tkOf, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-4].ex, NewCaseItem(new empty_statement(), null), ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 547: // case_list -> case_item
{
			if (ValueStack[ValueStack.Depth-1].stn is empty_statement) 
				CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, null);
			else CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 548: // case_list -> case_list, tkSemiColon, case_item
{ 
			CurrentSemanticValue.stn = AddCaseItem(ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 549: // case_item -> case_label_list, tkColon, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new case_variant(ValueStack[ValueStack.Depth-3].stn as expression_list, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 550: // case_label_list -> case_label
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 551: // case_label_list -> case_label_list, tkComma, case_label
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 552: // case_label -> const_elem
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 553: // else_case -> /* empty */
{ CurrentSemanticValue.stn = null;}
        break;
      case 554: // else_case -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 555: // repeat_stmt -> tkRepeat, stmt_list, tkUntil, expr
{ 
		    CurrentSemanticValue.stn = new repeat_node(ValueStack[ValueStack.Depth-3].stn as statement_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-3].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-4].ti;
			(ValueStack[ValueStack.Depth-3].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-2].ti;
			ValueStack[ValueStack.Depth-3].stn.source_context = LocationStack[LocationStack.Depth-4].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 556: // while_stmt -> tkWhile, expr_l1, optional_tk_do, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewWhileStmt(ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);    
        }
        break;
      case 557: // optional_tk_do -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 558: // optional_tk_do -> /* empty */
{ CurrentSemanticValue.ti = null; }
        break;
      case 559: // lock_stmt -> tkLock, expr_l1, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new lock_stmt(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 560: // index_or_nothing -> tkIndex, tkIdentifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 561: // index_or_nothing -> /* empty */
{ CurrentSemanticValue.id = null; }
        break;
      case 562: // optional_type_specification -> tkColon, type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 563: // optional_type_specification -> /* empty */
{ CurrentSemanticValue.td = null; }
        break;
      case 564: // optional_var -> tkVar
{ CurrentSemanticValue.ob = true; }
        break;
      case 565: // optional_var -> /* empty */
{ CurrentSemanticValue.ob = false; }
        break;
      case 566: // for_cycle_type -> tkTo
{ CurrentSemanticValue.ob = for_cycle_type.to; }
        break;
      case 567: // for_cycle_type -> tkDownto
{ CurrentSemanticValue.ob = for_cycle_type.downto; }
        break;
      case 568: // foreach_stmt -> tkForeach, identifier, optional_type_specification, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
            if (ValueStack[ValueStack.Depth-6].td == null)
                parserTools.AddWarningFromResource("USING_UNLOCAL_FOREACH_VARIABLE", ValueStack[ValueStack.Depth-7].id.source_context);
        }
        break;
      case 569: // foreach_stmt -> tkForeach, tkVar, identifier, optional_type_specification, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
{ 
        	if (ValueStack[ValueStack.Depth-6].td == null)
				CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, new no_type_foreach(), ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
			else CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
        }
        break;
      case 570: // foreach_stmt -> tkForeach, tkVar, tkRoundOpen, ident_list, tkRoundClose, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
{ 
        	if (parserTools.buildTreeForFormatter)
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
      case 571: // for_stmt -> tkFor, optional_var, identifier, optional_type_specification, 
                //             tkAssign, expr_l1, for_cycle_type, expr_l1, optional_tk_do, 
                //             unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-9].ob, ValueStack[ValueStack.Depth-8].id, ValueStack[ValueStack.Depth-7].td, ValueStack[ValueStack.Depth-5].ex, (for_cycle_type)ValueStack[ValueStack.Depth-4].ob, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 572: // for_stmt -> tkFor, optional_var, identifier, optional_type_specification, 
                //             tkAssign, expr_l1, for_cycle_type, expr_l1, tkStep, expr_l1, tkDo, 
                //             unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-11].ob, ValueStack[ValueStack.Depth-10].id, ValueStack[ValueStack.Depth-9].td, ValueStack[ValueStack.Depth-7].ex, (for_cycle_type)ValueStack[ValueStack.Depth-6].ob, ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 573: // with_stmt -> tkWith, expr_list, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new with_statement(ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 574: // inherited_message -> tkInherited
{ 
			CurrentSemanticValue.stn = new inherited_message();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 575: // try_stmt -> tkTry, stmt_list, try_handler
{ 
			CurrentSemanticValue.stn = new try_stmt(ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].stn as try_handler, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			ValueStack[ValueStack.Depth-2].stn.source_context = LocationStack[LocationStack.Depth-3].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 576: // try_handler -> tkFinally, stmt_list, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_finally(ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan);
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(ValueStack[ValueStack.Depth-2].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
		}
        break;
      case 577: // try_handler -> tkExcept, exception_block, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_except((exception_block)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan);  
			if ((ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list != null)
			{
				(ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list.source_context = CurrentLocationSpan;
				(ValueStack[ValueStack.Depth-2].stn as exception_block).source_context = CurrentLocationSpan;
			}
		}
        break;
      case 578: // exception_block -> exception_handler_list, exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-2].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 579: // exception_block -> exception_handler_list, tkSemiColon, 
                //                    exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-3].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 580: // exception_block -> stmt_list
{ 
			CurrentSemanticValue.stn = new exception_block(ValueStack[ValueStack.Depth-1].stn as statement_list, null, null, LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 581: // exception_handler_list -> exception_handler
{ 
			CurrentSemanticValue.stn = new exception_handler_list(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 582: // exception_handler_list -> exception_handler_list, tkSemiColon, 
                //                           exception_handler
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as exception_handler_list).Add(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 583: // exception_block_else_branch -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 584: // exception_block_else_branch -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 585: // exception_handler -> tkOn, exception_identifier, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new exception_handler((ValueStack[ValueStack.Depth-3].stn as exception_ident).variable, (ValueStack[ValueStack.Depth-3].stn as exception_ident).type_name, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 586: // exception_identifier -> exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(null, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 587: // exception_identifier -> exception_variable, tkColon, 
                //                         exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(ValueStack[ValueStack.Depth-3].id, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 588: // exception_class_type_identifier -> simple_type_identifier
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 589: // exception_variable -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 590: // raise_stmt -> tkRaise
{ 
			CurrentSemanticValue.stn = new raise_stmt(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 591: // raise_stmt -> tkRaise, expr
{ 
			CurrentSemanticValue.stn = new raise_stmt(ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan);  
		}
        break;
      case 592: // expr_list -> expr_with_func_decl_lambda
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 593: // expr_list -> expr_list, tkComma, expr_with_func_decl_lambda
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 594: // expr_list_func_param -> expr_with_func_decl_lambda_ass
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 595: // expr_list_func_param -> expr_list_func_param, tkComma, 
                //                         expr_with_func_decl_lambda_ass
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 596: // expr_as_stmt -> allowable_expr_as_stmt
{ 
			CurrentSemanticValue.stn = new expression_as_statement(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 597: // allowable_expr_as_stmt -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 598: // expr_with_func_decl_lambda -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 599: // expr_with_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 600: // expr_with_func_decl_lambda -> tkInherited
{ CurrentSemanticValue.ex = new inherited_ident("", CurrentLocationSpan); }
        break;
      case 601: // expr_with_func_decl_lambda_ass -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 602: // expr_with_func_decl_lambda_ass -> identifier, tkAssign, expr_l1
{ CurrentSemanticValue.ex = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); }
        break;
      case 603: // expr_with_func_decl_lambda_ass -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 604: // expr_with_func_decl_lambda_ass -> tkInherited
{ CurrentSemanticValue.ex = new inherited_ident("", CurrentLocationSpan); }
        break;
      case 605: // expr -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 606: // expr -> format_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 607: // expr_l1 -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 608: // expr_l1 -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 609: // expr_l1 -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 610: // expr_l1_for_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 611: // expr_l1_for_question_expr -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 612: // expr_l1_for_new_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 613: // expr_l1_for_new_question_expr -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 614: // expr_l1_func_decl_lambda -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 615: // expr_l1_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 616: // expr_l1_for_lambda -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 617: // expr_l1_for_lambda -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 618: // expr_l1_for_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 619: // expr_dq -> relop_expr
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
            	parserTools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 625: // new_question_expr -> tkIf, expr_l1_for_new_question_expr, tkThen, 
                //                      expr_l1_for_new_question_expr, tkElse, 
                //                      expr_l1_for_new_question_expr
{ 
        	if (parserTools.buildTreeForFormatter)
        	{
        		CurrentSemanticValue.ex = new if_expr_new(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        	}
        	else
        	{
            	if (ValueStack[ValueStack.Depth-3].ex is nil_const && ValueStack[ValueStack.Depth-1].ex is nil_const)
            		parserTools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",LocationStack[LocationStack.Depth-3]);
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
	        	else parserTools.AddErrorFromResource("WITHOUT_INIT_AND_SIZE",LocationStack[LocationStack.Depth-2]);
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
				parserTools.AddErrorFromResource("NIL_IN_UNNAMED_OBJECT",CurrentLocationSpan);		    
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
				parserTools.errors.Add(new bad_anon_type(parserTools.currentFileName, LocationStack[LocationStack.Depth-1], null));	
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
            	parserTools.errors.Add(new anon_type_duplicate_name(parserTools.currentFileName, LocationStack[LocationStack.Depth-1], null));
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
				parserTools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",CurrentLocationSpan);
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
			if (parserTools.buildTreeForFormatter)
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
		if (parserTools.buildTreeForFormatter)
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
				parserTools.AddErrorFromResource("EXPRESSION_EXPECTED",LocationStack[LocationStack.Depth-6]);
			foreach (var ex in (ValueStack[ValueStack.Depth-4].stn as expression_list).expressions)
				if (ex is unpacked_list_of_ident_or_list)
					parserTools.AddErrorFromResource("EXPRESSION_EXPECTED",ex.source_context);
			if (!(ValueStack[ValueStack.Depth-3].td is lambda_inferred_type)) 
				parserTools.AddErrorFromResource("BAD_TUPLE",LocationStack[LocationStack.Depth-3]);
			if (ValueStack[ValueStack.Depth-2].stn != null) 
				parserTools.AddErrorFromResource("BAD_TUPLE",LocationStack[LocationStack.Depth-2]);

			if ((ValueStack[ValueStack.Depth-4].stn as expression_list).Count>6) 
				parserTools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",CurrentLocationSpan);
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
					parserTools.AddErrorFromResource("BAD_INT2",CurrentLocationSpan);
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
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
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
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-4], "^");
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
		    if (!parserTools.buildTreeForFormatter) 
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
		    if (!parserTools.buildTreeForFormatter) 
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
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");		
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
                if (!parserTools.buildTreeForFormatter)
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
            		parserTools.AddErrorFromResource("SLICES_OF MULTIDIMENSIONAL_ARRAYS_ALLOW_ONLY_FOR_RANK_LT_5",CurrentLocationSpan); // С�?ез�? многоме�?н�?�? массивов �?аз�?е�?ен�? �?ол�?ко для массивов �?азме�?нос�?и < 5  
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
            if (!parserTools.buildTreeForFormatter)
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
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
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
            if (parserTools.buildTreeForFormatter)
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
            if (parserTools.buildTreeForFormatter)
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
            	parserTools.AddErrorFromResource("TWO_STRING_LITERALS_IN_SUCCESSION",LocationStack[LocationStack.Depth-1]);
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
			//parserTools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
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
			//parserTools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
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
            parserTools.AddWarningFromResource("OVERLOAD_IS_NOT_USED", ValueStack[ValueStack.Depth-1].id.source_context);
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
						parserTools.AddErrorFromResource("SEMICOLON_IN_PARAMS",LocationStack[LocationStack.Depth-3]);
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
					parserTools.AddErrorFromResource("ONE_TKIDENTIFIER",LocationStack[LocationStack.Depth-7]);
				var lambda_inf_type = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
				var new_typed_pars = new typed_parameters(new ident_list(idd, idd.source_context), lambda_inf_type, parametr_kind.none, null, idd.source_context);
				formal_pars.Add(new_typed_pars);
				foreach (var id in (ValueStack[ValueStack.Depth-5].stn as expression_list).expressions)
				{
					var idd1 = id as ident;
					if (idd1==null)
						parserTools.AddErrorFromResource("ONE_TKIDENTIFIER",id.source_context);
					
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
					parserTools.AddErrorFromResource("ONE_TKIDENTIFIER",LocationStack[LocationStack.Depth-7]);
				
				var idList = new ident_list(idd, loc);
				
				var iddlist = (ValueStack[ValueStack.Depth-5].stn as expression_list).expressions;
				
				for (int j = 0; j < iddlist.Count; j++)
				{
					var idd2 = iddlist[j] as ident;
					if (idd2==null)
						parserTools.AddErrorFromResource("ONE_TKIDENTIFIER",idd2.source_context);
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
			parserTools.AddErrorFromResource("YIELD_STATEMENT_CANNOT_BE_USED_IN_LAMBDA_BODY", CurrentLocationSpan);
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
                 parserTools.AddErrorFromResource("RESULT_IDENT_NOT_EXPECTED_IN_THIS_CONTEXT", id.source_context);
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
