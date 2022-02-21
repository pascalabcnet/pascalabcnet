// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.6
// Machine:  DESKTOP-G8V08V4
// DateTime: 17.02.2022 17:47:16
// UserName: ?????????
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
    tkSizeOf=19,tkTypeOf=20,tkWhere=21,tkArray=22,tkCase=23,tkClass=24,
    tkAuto=25,tkStatic=26,tkConst=27,tkConstructor=28,tkDestructor=29,tkElse=30,
    tkExcept=31,tkFile=32,tkFor=33,tkForeach=34,tkFunction=35,tkMatch=36,
    tkWhen=37,tkIf=38,tkImplementation=39,tkInherited=40,tkInterface=41,tkProcedure=42,
    tkOperator=43,tkProperty=44,tkRaise=45,tkRecord=46,tkSet=47,tkType=48,
    tkThen=49,tkUses=50,tkVar=51,tkWhile=52,tkWith=53,tkNil=54,
    tkGoto=55,tkOf=56,tkLabel=57,tkLock=58,tkProgram=59,tkEvent=60,
    tkDefault=61,tkTemplate=62,tkPacked=63,tkExports=64,tkResourceString=65,tkThreadvar=66,
    tkSealed=67,tkPartial=68,tkTo=69,tkDownto=70,tkLoop=71,tkSequence=72,
    tkYield=73,tkShortProgram=74,tkVertParen=75,tkShortSFProgram=76,tkNew=77,tkOn=78,
    tkName=79,tkPrivate=80,tkProtected=81,tkPublic=82,tkInternal=83,tkRead=84,
    tkWrite=85,tkParseModeExpression=86,tkParseModeStatement=87,tkParseModeType=88,tkBegin=89,tkEnd=90,
    tkAsmBody=91,tkILCode=92,tkError=93,INVISIBLE=94,tkRepeat=95,tkUntil=96,
    tkDo=97,tkComma=98,tkFinally=99,tkTry=100,tkInitialization=101,tkFinalization=102,
    tkUnit=103,tkLibrary=104,tkExternal=105,tkParams=106,tkNamespace=107,tkAssign=108,
    tkPlusEqual=109,tkMinusEqual=110,tkMultEqual=111,tkDivEqual=112,tkMinus=113,tkPlus=114,
    tkSlash=115,tkStar=116,tkStarStar=117,tkEqual=118,tkGreater=119,tkGreaterEqual=120,
    tkLower=121,tkLowerEqual=122,tkNotEqual=123,tkCSharpStyleOr=124,tkArrow=125,tkOr=126,
    tkXor=127,tkAnd=128,tkDiv=129,tkMod=130,tkShl=131,tkShr=132,
    tkNot=133,tkAs=134,tkIn=135,tkIs=136,tkImplicit=137,tkExplicit=138,
    tkAddressOf=139,tkDeref=140,tkIdentifier=141,tkStringLiteral=142,tkFormatStringLiteral=143,tkAsciiChar=144,
    tkAbstract=145,tkForward=146,tkOverload=147,tkReintroduce=148,tkOverride=149,tkVirtual=150,
    tkExtensionMethod=151,tkInteger=152,tkBigInteger=153,tkFloat=154,tkHex=155,tkUnknown=156,
    tkStep=157};

// Abstract base class for GPLEX scanners
public abstract class ScanBase : AbstractScanner<PascalABCSavParser.Union,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

public partial class GPPGParser: ShiftReduceParser<PascalABCSavParser.Union, LexLocation>
{
  // Verbatim content from ABCPascal.y
// ��� ���������� ����������� � ����� GPPGParser, �������������� ����� ������, ������������ �������� gppg
    public syntax_tree_node root; // �������� ���� ��������������� ������ 

    public List<Error> errors;
    public string current_file_name;
    public int max_errors = 10;
    public PT parsertools;
    public List<compiler_directive> CompilerDirectives;
	public ParserLambdaHelper lambdaHelper = new ParserLambdaHelper();
	
    public GPPGParser(AbstractScanner<PascalABCSavParser.Union, LexLocation> scanner) : base(scanner) { }
  // End verbatim content from ABCPascal.y

#pragma warning disable 649
  private static Dictionary<int, string> aliasses;
#pragma warning restore 649
  private static Rule[] rules = new Rule[1009];
  private static State[] states = new State[1675];
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
      "elem_list", "optional_expr_list_with_bracket", "expr_list", "const_elem_list1", 
      "case_label_list", "const_elem_list", "optional_const_func_expr_list", 
      "elem_list1", "enumeration_id", "expr_l1_or_unpacked_list", "enumeration_id_list", 
      "const_simple_expr", "term", "term1", "typed_const", "typed_const_plus", 
      "typed_var_init_expression", "expr", "expr_with_func_decl_lambda", "const_expr", 
      "const_relop_expr", "elem", "range_expr", "const_elem", "array_const", 
      "factor", "factor_without_unary_op", "relop_expr", "expr_dq", "lambda_unpacked_params", 
      "expr_l1", "expr_l1_or_unpacked", "expr_l1_func_decl_lambda", "expr_l1_for_lambda", 
      "simple_expr", "range_term", "range_factor", "external_directive_ident", 
      "init_const_expr", "case_label", "variable", "var_reference", "optional_read_expr", 
      "simple_expr_or_nothing", "var_question_point", "expr_l1_for_question_expr", 
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
      "simple_type_question", "foreach_stmt_ident_dype_opt", "fptype", "type_ref", 
      "fptype_noproctype", "array_type", "template_param", "template_empty_param", 
      "structured_type", "unpacked_structured_type", "empty_template_type_reference", 
      "simple_or_template_type_reference", "simple_or_template_or_question_type_reference", 
      "type_ref_or_secific", "for_stmt_decl_or_assign", "type_decl_type", "type_ref_and_secific_list", 
      "type_decl_sect", "try_handler", "class_or_interface_keyword", "optional_tk_do", 
      "keyword", "reserved_keyword", "typeof_expr", "simple_fp_sect", "template_param_list", 
      "template_empty_param_list", "template_type_params", "template_type_empty_params", 
      "template_type", "try_stmt", "uses_clause", "used_units_list", "uses_clause_one", 
      "uses_clause_one_or_empty", "unit_file", "used_unit_name", "unit_header", 
      "var_decl_sect", "var_decl", "var_decl_part", "field_definition", "var_decl_with_assign_var_tuple", 
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
      "var_with_init_for_expr_with_let_list", "$accept", };

  static GPPGParser() {
    states[0] = new State(new int[]{59,1573,103,1640,104,1641,107,1642,86,1647,88,1652,87,1659,74,1664,76,1671,3,-27,50,-27,89,-27,57,-27,27,-27,65,-27,48,-27,51,-27,60,-27,11,-27,42,-27,35,-27,26,-27,24,-27,28,-27,29,-27},new int[]{-1,1,-230,3,-231,4,-304,1585,-306,1586,-2,1635,-171,1646});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{3,1569,50,-14,89,-14,57,-14,27,-14,65,-14,48,-14,51,-14,60,-14,11,-14,42,-14,35,-14,26,-14,24,-14,28,-14,29,-14},new int[]{-181,5,-182,1567,-180,1572});
    states[5] = new State(-41,new int[]{-300,6});
    states[6] = new State(new int[]{50,1555,57,-67,27,-67,65,-67,48,-67,51,-67,60,-67,11,-67,42,-67,35,-67,26,-67,24,-67,28,-67,29,-67,89,-67},new int[]{-18,7,-302,14,-37,15,-41,1492,-42,1493});
    states[7] = new State(new int[]{7,9,10,10,5,11,98,12,6,13,2,-26},new int[]{-184,8});
    states[8] = new State(-20);
    states[9] = new State(-21);
    states[10] = new State(-22);
    states[11] = new State(-23);
    states[12] = new State(-24);
    states[13] = new State(-25);
    states[14] = new State(-42);
    states[15] = new State(new int[]{89,17},new int[]{-251,16});
    states[16] = new State(-34);
    states[17] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,748,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486},new int[]{-248,18,-257,746,-256,22,-4,23,-109,24,-128,373,-108,385,-143,747,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879,-139,1034});
    states[18] = new State(new int[]{90,19,10,20});
    states[19] = new State(-522);
    states[20] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,748,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486,96,-486,99,-486,31,-486,102,-486,2,-486},new int[]{-257,21,-256,22,-4,23,-109,24,-128,373,-108,385,-143,747,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879,-139,1034});
    states[21] = new State(-524);
    states[22] = new State(-484);
    states[23] = new State(-487);
    states[24] = new State(new int[]{108,415,109,416,110,417,111,418,112,419,90,-520,10,-520,96,-520,99,-520,31,-520,102,-520,2,-520,9,-520,98,-520,12,-520,97,-520,30,-520,84,-520,83,-520,82,-520,81,-520,80,-520,85,-520},new int[]{-190,25});
    states[25] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,651,8,652,19,268,20,273,75,462,38,600,5,609,18,674,35,683,42,687},new int[]{-86,26,-85,27,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-320,827,-97,659,-321,682});
    states[26] = new State(-514);
    states[27] = new State(-589);
    states[28] = new State(-592);
    states[29] = new State(new int[]{16,30,90,-594,10,-594,96,-594,99,-594,31,-594,102,-594,2,-594,9,-594,98,-594,12,-594,97,-594,30,-594,84,-594,83,-594,82,-594,81,-594,80,-594,85,-594,6,-594,75,-594,5,-594,49,-594,56,-594,139,-594,141,-594,79,-594,77,-594,157,-594,43,-594,40,-594,8,-594,19,-594,20,-594,142,-594,144,-594,143,-594,152,-594,155,-594,154,-594,153,-594,55,-594,89,-594,38,-594,23,-594,95,-594,52,-594,33,-594,53,-594,100,-594,45,-594,34,-594,51,-594,58,-594,73,-594,71,-594,36,-594,69,-594,70,-594,13,-597});
    states[30] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462},new int[]{-95,31,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590});
    states[31] = new State(new int[]{118,312,123,313,121,314,119,315,122,316,120,317,135,318,133,319,16,-607,90,-607,10,-607,96,-607,99,-607,31,-607,102,-607,2,-607,9,-607,98,-607,12,-607,97,-607,30,-607,84,-607,83,-607,82,-607,81,-607,80,-607,85,-607,13,-607,6,-607,75,-607,5,-607,49,-607,56,-607,139,-607,141,-607,79,-607,77,-607,157,-607,43,-607,40,-607,8,-607,19,-607,20,-607,142,-607,144,-607,143,-607,152,-607,155,-607,154,-607,153,-607,55,-607,89,-607,38,-607,23,-607,95,-607,52,-607,33,-607,53,-607,100,-607,45,-607,34,-607,51,-607,58,-607,73,-607,71,-607,36,-607,69,-607,70,-607,114,-607,113,-607,126,-607,127,-607,124,-607,136,-607,134,-607,116,-607,115,-607,129,-607,130,-607,131,-607,132,-607,128,-607},new int[]{-192,32});
    states[32] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-102,33,-238,1491,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,613,-263,590});
    states[33] = new State(new int[]{6,34,118,-632,123,-632,121,-632,119,-632,122,-632,120,-632,135,-632,133,-632,16,-632,90,-632,10,-632,96,-632,99,-632,31,-632,102,-632,2,-632,9,-632,98,-632,12,-632,97,-632,30,-632,84,-632,83,-632,82,-632,81,-632,80,-632,85,-632,13,-632,75,-632,5,-632,49,-632,56,-632,139,-632,141,-632,79,-632,77,-632,157,-632,43,-632,40,-632,8,-632,19,-632,20,-632,142,-632,144,-632,143,-632,152,-632,155,-632,154,-632,153,-632,55,-632,89,-632,38,-632,23,-632,95,-632,52,-632,33,-632,53,-632,100,-632,45,-632,34,-632,51,-632,58,-632,73,-632,71,-632,36,-632,69,-632,70,-632,114,-632,113,-632,126,-632,127,-632,124,-632,136,-632,134,-632,116,-632,115,-632,129,-632,130,-632,131,-632,132,-632,128,-632});
    states[34] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462},new int[]{-81,35,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,613,-263,590});
    states[35] = new State(new int[]{114,327,113,328,126,329,127,330,124,331,6,-711,5,-711,118,-711,123,-711,121,-711,119,-711,122,-711,120,-711,135,-711,133,-711,16,-711,90,-711,10,-711,96,-711,99,-711,31,-711,102,-711,2,-711,9,-711,98,-711,12,-711,97,-711,30,-711,84,-711,83,-711,82,-711,81,-711,80,-711,85,-711,13,-711,75,-711,49,-711,56,-711,139,-711,141,-711,79,-711,77,-711,157,-711,43,-711,40,-711,8,-711,19,-711,20,-711,142,-711,144,-711,143,-711,152,-711,155,-711,154,-711,153,-711,55,-711,89,-711,38,-711,23,-711,95,-711,52,-711,33,-711,53,-711,100,-711,45,-711,34,-711,51,-711,58,-711,73,-711,71,-711,36,-711,69,-711,70,-711,136,-711,134,-711,116,-711,115,-711,129,-711,130,-711,131,-711,132,-711,128,-711},new int[]{-193,36});
    states[36] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-80,37,-238,1490,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,613,-263,590});
    states[37] = new State(new int[]{136,333,134,1453,116,1456,115,1457,129,1458,130,1459,131,1460,132,1461,128,1462,114,-713,113,-713,126,-713,127,-713,124,-713,6,-713,5,-713,118,-713,123,-713,121,-713,119,-713,122,-713,120,-713,135,-713,133,-713,16,-713,90,-713,10,-713,96,-713,99,-713,31,-713,102,-713,2,-713,9,-713,98,-713,12,-713,97,-713,30,-713,84,-713,83,-713,82,-713,81,-713,80,-713,85,-713,13,-713,75,-713,49,-713,56,-713,139,-713,141,-713,79,-713,77,-713,157,-713,43,-713,40,-713,8,-713,19,-713,20,-713,142,-713,144,-713,143,-713,152,-713,155,-713,154,-713,153,-713,55,-713,89,-713,38,-713,23,-713,95,-713,52,-713,33,-713,53,-713,100,-713,45,-713,34,-713,51,-713,58,-713,73,-713,71,-713,36,-713,69,-713,70,-713},new int[]{-194,38});
    states[38] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,53,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-93,39,-264,40,-238,41,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-94,471});
    states[39] = new State(-734);
    states[40] = new State(-735);
    states[41] = new State(-736);
    states[42] = new State(-749);
    states[43] = new State(new int[]{7,44,136,-750,134,-750,116,-750,115,-750,129,-750,130,-750,131,-750,132,-750,128,-750,114,-750,113,-750,126,-750,127,-750,124,-750,6,-750,5,-750,118,-750,123,-750,121,-750,119,-750,122,-750,120,-750,135,-750,133,-750,16,-750,90,-750,10,-750,96,-750,99,-750,31,-750,102,-750,2,-750,9,-750,98,-750,12,-750,97,-750,30,-750,84,-750,83,-750,82,-750,81,-750,80,-750,85,-750,13,-750,75,-750,49,-750,56,-750,139,-750,141,-750,79,-750,77,-750,157,-750,43,-750,40,-750,8,-750,19,-750,20,-750,142,-750,144,-750,143,-750,152,-750,155,-750,154,-750,153,-750,55,-750,89,-750,38,-750,23,-750,95,-750,52,-750,33,-750,53,-750,100,-750,45,-750,34,-750,51,-750,58,-750,73,-750,71,-750,36,-750,69,-750,70,-750,11,-774,17,-774,117,-747});
    states[44] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,83,57,82,58,81,59,80,60,67,61,62,62,126,63,20,64,19,65,61,66,21,67,127,68,128,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,22,78,72,79,89,80,23,81,24,82,27,83,28,84,29,85,70,86,97,87,30,88,90,89,31,90,32,91,25,92,102,93,99,94,33,95,34,96,35,97,38,98,39,99,40,100,101,101,41,102,42,103,44,104,45,105,46,106,95,107,47,108,100,109,48,110,26,111,49,112,69,113,96,114,50,115,51,116,52,117,53,118,54,119,55,120,56,121,57,122,59,123,103,124,104,125,107,126,105,127,106,128,60,129,73,130,36,131,37,132,68,133,145,134,58,135,137,136,138,137,78,138,150,139,149,140,71,141,151,142,147,143,148,144,146,145,43,147},new int[]{-134,45,-143,46,-147,48,-148,51,-290,55,-146,56,-291,146});
    states[45] = new State(-785);
    states[46] = new State(-819);
    states[47] = new State(-815);
    states[48] = new State(-816);
    states[49] = new State(-835);
    states[50] = new State(-836);
    states[51] = new State(-817);
    states[52] = new State(-837);
    states[53] = new State(-838);
    states[54] = new State(-818);
    states[55] = new State(-820);
    states[56] = new State(-843);
    states[57] = new State(-839);
    states[58] = new State(-840);
    states[59] = new State(-841);
    states[60] = new State(-842);
    states[61] = new State(-844);
    states[62] = new State(-845);
    states[63] = new State(-846);
    states[64] = new State(-847);
    states[65] = new State(-848);
    states[66] = new State(-849);
    states[67] = new State(-850);
    states[68] = new State(-851);
    states[69] = new State(-852);
    states[70] = new State(-853);
    states[71] = new State(-854);
    states[72] = new State(-855);
    states[73] = new State(-856);
    states[74] = new State(-857);
    states[75] = new State(-858);
    states[76] = new State(-859);
    states[77] = new State(-860);
    states[78] = new State(-861);
    states[79] = new State(-862);
    states[80] = new State(-863);
    states[81] = new State(-864);
    states[82] = new State(-865);
    states[83] = new State(-866);
    states[84] = new State(-867);
    states[85] = new State(-868);
    states[86] = new State(-869);
    states[87] = new State(-870);
    states[88] = new State(-871);
    states[89] = new State(-872);
    states[90] = new State(-873);
    states[91] = new State(-874);
    states[92] = new State(-875);
    states[93] = new State(-876);
    states[94] = new State(-877);
    states[95] = new State(-878);
    states[96] = new State(-879);
    states[97] = new State(-880);
    states[98] = new State(-881);
    states[99] = new State(-882);
    states[100] = new State(-883);
    states[101] = new State(-884);
    states[102] = new State(-885);
    states[103] = new State(-886);
    states[104] = new State(-887);
    states[105] = new State(-888);
    states[106] = new State(-889);
    states[107] = new State(-890);
    states[108] = new State(-891);
    states[109] = new State(-892);
    states[110] = new State(-893);
    states[111] = new State(-894);
    states[112] = new State(-895);
    states[113] = new State(-896);
    states[114] = new State(-897);
    states[115] = new State(-898);
    states[116] = new State(-899);
    states[117] = new State(-900);
    states[118] = new State(-901);
    states[119] = new State(-902);
    states[120] = new State(-903);
    states[121] = new State(-904);
    states[122] = new State(-905);
    states[123] = new State(-906);
    states[124] = new State(-907);
    states[125] = new State(-908);
    states[126] = new State(-909);
    states[127] = new State(-910);
    states[128] = new State(-911);
    states[129] = new State(-912);
    states[130] = new State(-913);
    states[131] = new State(-914);
    states[132] = new State(-915);
    states[133] = new State(-916);
    states[134] = new State(-917);
    states[135] = new State(-918);
    states[136] = new State(-919);
    states[137] = new State(-920);
    states[138] = new State(-921);
    states[139] = new State(-922);
    states[140] = new State(-923);
    states[141] = new State(-924);
    states[142] = new State(-925);
    states[143] = new State(-926);
    states[144] = new State(-927);
    states[145] = new State(-928);
    states[146] = new State(-821);
    states[147] = new State(-929);
    states[148] = new State(-758);
    states[149] = new State(new int[]{142,151,144,152,7,-804,11,-804,17,-804,136,-804,134,-804,116,-804,115,-804,129,-804,130,-804,131,-804,132,-804,128,-804,114,-804,113,-804,126,-804,127,-804,124,-804,6,-804,5,-804,118,-804,123,-804,121,-804,119,-804,122,-804,120,-804,135,-804,133,-804,16,-804,90,-804,10,-804,96,-804,99,-804,31,-804,102,-804,2,-804,9,-804,98,-804,12,-804,97,-804,30,-804,84,-804,83,-804,82,-804,81,-804,80,-804,85,-804,13,-804,117,-804,75,-804,49,-804,56,-804,139,-804,141,-804,79,-804,77,-804,157,-804,43,-804,40,-804,8,-804,19,-804,20,-804,143,-804,152,-804,155,-804,154,-804,153,-804,55,-804,89,-804,38,-804,23,-804,95,-804,52,-804,33,-804,53,-804,100,-804,45,-804,34,-804,51,-804,58,-804,73,-804,71,-804,36,-804,69,-804,70,-804,125,-804,108,-804,4,-804,140,-804},new int[]{-162,150});
    states[150] = new State(-807);
    states[151] = new State(-802);
    states[152] = new State(-803);
    states[153] = new State(-806);
    states[154] = new State(-805);
    states[155] = new State(-759);
    states[156] = new State(-189);
    states[157] = new State(-190);
    states[158] = new State(-191);
    states[159] = new State(-192);
    states[160] = new State(-751);
    states[161] = new State(new int[]{8,162});
    states[162] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,665},new int[]{-281,163,-280,165,-176,166,-143,205,-147,48,-148,51,-269,1487,-268,1488,-90,179,-103,288,-104,289,-16,488,-195,489,-161,492,-163,149,-162,153,-298,1489});
    states[163] = new State(new int[]{9,164});
    states[164] = new State(-745);
    states[165] = new State(-618);
    states[166] = new State(new int[]{7,167,4,170,121,172,9,-615,8,-254,116,-254,115,-254,129,-254,130,-254,131,-254,132,-254,128,-254,6,-254,114,-254,113,-254,126,-254,127,-254,13,-254},new int[]{-296,169});
    states[167] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,83,57,82,58,81,59,80,60,67,61,62,62,126,63,20,64,19,65,61,66,21,67,127,68,128,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,22,78,72,79,89,80,23,81,24,82,27,83,28,84,29,85,70,86,97,87,30,88,90,89,31,90,32,91,25,92,102,93,99,94,33,95,34,96,35,97,38,98,39,99,40,100,101,101,41,102,42,103,44,104,45,105,46,106,95,107,47,108,100,109,48,110,26,111,49,112,69,113,96,114,50,115,51,116,52,117,53,118,54,119,55,120,56,121,57,122,59,123,103,124,104,125,107,126,105,127,106,128,60,129,73,130,36,131,37,132,68,133,145,134,58,135,137,136,138,137,78,138,150,139,149,140,71,141,151,142,147,143,148,144,146,145,43,147},new int[]{-134,168,-143,46,-147,48,-148,51,-290,55,-146,56,-291,146});
    states[168] = new State(-260);
    states[169] = new State(new int[]{9,-616,13,-233});
    states[170] = new State(new int[]{121,172},new int[]{-296,171});
    states[171] = new State(-617);
    states[172] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-294,173,-275,287,-268,177,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-277,1429,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,1430,-220,573,-219,574,-298,1431});
    states[173] = new State(new int[]{119,174,98,175});
    states[174] = new State(-234);
    states[175] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-275,176,-268,177,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-277,1429,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,1430,-220,573,-219,574,-298,1431});
    states[176] = new State(-238);
    states[177] = new State(new int[]{13,178,119,-242,98,-242,118,-242,9,-242,8,-242,136,-242,134,-242,116,-242,115,-242,129,-242,130,-242,131,-242,132,-242,128,-242,114,-242,113,-242,126,-242,127,-242,124,-242,6,-242,5,-242,123,-242,121,-242,122,-242,120,-242,135,-242,133,-242,16,-242,90,-242,10,-242,96,-242,99,-242,31,-242,102,-242,2,-242,12,-242,97,-242,30,-242,84,-242,83,-242,82,-242,81,-242,80,-242,85,-242,75,-242,49,-242,56,-242,139,-242,141,-242,79,-242,77,-242,157,-242,43,-242,40,-242,19,-242,20,-242,142,-242,144,-242,143,-242,152,-242,155,-242,154,-242,153,-242,55,-242,89,-242,38,-242,23,-242,95,-242,52,-242,33,-242,53,-242,100,-242,45,-242,34,-242,51,-242,58,-242,73,-242,71,-242,36,-242,69,-242,70,-242,125,-242,108,-242});
    states[178] = new State(-243);
    states[179] = new State(new int[]{6,1485,114,232,113,233,126,234,127,235,13,-247,119,-247,98,-247,118,-247,9,-247,8,-247,136,-247,134,-247,116,-247,115,-247,129,-247,130,-247,131,-247,132,-247,128,-247,124,-247,5,-247,123,-247,121,-247,122,-247,120,-247,135,-247,133,-247,16,-247,90,-247,10,-247,96,-247,99,-247,31,-247,102,-247,2,-247,12,-247,97,-247,30,-247,84,-247,83,-247,82,-247,81,-247,80,-247,85,-247,75,-247,49,-247,56,-247,139,-247,141,-247,79,-247,77,-247,157,-247,43,-247,40,-247,19,-247,20,-247,142,-247,144,-247,143,-247,152,-247,155,-247,154,-247,153,-247,55,-247,89,-247,38,-247,23,-247,95,-247,52,-247,33,-247,53,-247,100,-247,45,-247,34,-247,51,-247,58,-247,73,-247,71,-247,36,-247,69,-247,70,-247,125,-247,108,-247},new int[]{-189,180});
    states[180] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154},new int[]{-103,181,-104,289,-176,491,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153});
    states[181] = new State(new int[]{116,239,115,240,129,241,130,242,131,243,132,244,128,245,6,-251,114,-251,113,-251,126,-251,127,-251,13,-251,119,-251,98,-251,118,-251,9,-251,8,-251,136,-251,134,-251,124,-251,5,-251,123,-251,121,-251,122,-251,120,-251,135,-251,133,-251,16,-251,90,-251,10,-251,96,-251,99,-251,31,-251,102,-251,2,-251,12,-251,97,-251,30,-251,84,-251,83,-251,82,-251,81,-251,80,-251,85,-251,75,-251,49,-251,56,-251,139,-251,141,-251,79,-251,77,-251,157,-251,43,-251,40,-251,19,-251,20,-251,142,-251,144,-251,143,-251,152,-251,155,-251,154,-251,153,-251,55,-251,89,-251,38,-251,23,-251,95,-251,52,-251,33,-251,53,-251,100,-251,45,-251,34,-251,51,-251,58,-251,73,-251,71,-251,36,-251,69,-251,70,-251,125,-251,108,-251},new int[]{-191,182});
    states[182] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154},new int[]{-104,183,-176,491,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153});
    states[183] = new State(new int[]{8,184,116,-253,115,-253,129,-253,130,-253,131,-253,132,-253,128,-253,6,-253,114,-253,113,-253,126,-253,127,-253,13,-253,119,-253,98,-253,118,-253,9,-253,136,-253,134,-253,124,-253,5,-253,123,-253,121,-253,122,-253,120,-253,135,-253,133,-253,16,-253,90,-253,10,-253,96,-253,99,-253,31,-253,102,-253,2,-253,12,-253,97,-253,30,-253,84,-253,83,-253,82,-253,81,-253,80,-253,85,-253,75,-253,49,-253,56,-253,139,-253,141,-253,79,-253,77,-253,157,-253,43,-253,40,-253,19,-253,20,-253,142,-253,144,-253,143,-253,152,-253,155,-253,154,-253,153,-253,55,-253,89,-253,38,-253,23,-253,95,-253,52,-253,33,-253,53,-253,100,-253,45,-253,34,-253,51,-253,58,-253,73,-253,71,-253,36,-253,69,-253,70,-253,125,-253,108,-253});
    states[184] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161,9,-184},new int[]{-73,185,-71,187,-91,1484,-87,190,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[185] = new State(new int[]{9,186});
    states[186] = new State(-258);
    states[187] = new State(new int[]{98,188,9,-183,12,-183});
    states[188] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-91,189,-87,190,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[189] = new State(-186);
    states[190] = new State(new int[]{13,191,16,195,6,1478,98,-187,9,-187,12,-187,5,-187});
    states[191] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-87,192,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[192] = new State(new int[]{5,193,13,191,16,195});
    states[193] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-87,194,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[194] = new State(new int[]{13,191,16,195,6,-123,98,-123,9,-123,12,-123,5,-123,90,-123,10,-123,96,-123,99,-123,31,-123,102,-123,2,-123,97,-123,30,-123,84,-123,83,-123,82,-123,81,-123,80,-123,85,-123});
    states[195] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-88,196,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842});
    states[196] = new State(new int[]{118,224,123,225,121,226,119,227,122,228,120,229,135,230,13,-122,16,-122,6,-122,98,-122,9,-122,12,-122,5,-122,90,-122,10,-122,96,-122,99,-122,31,-122,102,-122,2,-122,97,-122,30,-122,84,-122,83,-122,82,-122,81,-122,80,-122,85,-122},new int[]{-188,197});
    states[197] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-79,198,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842});
    states[198] = new State(new int[]{114,232,113,233,126,234,127,235,118,-119,123,-119,121,-119,119,-119,122,-119,120,-119,135,-119,13,-119,16,-119,6,-119,98,-119,9,-119,12,-119,5,-119,90,-119,10,-119,96,-119,99,-119,31,-119,102,-119,2,-119,97,-119,30,-119,84,-119,83,-119,82,-119,81,-119,80,-119,85,-119},new int[]{-189,199});
    states[199] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-13,200,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842});
    states[200] = new State(new int[]{134,237,136,238,116,239,115,240,129,241,130,242,131,243,132,244,128,245,114,-132,113,-132,126,-132,127,-132,118,-132,123,-132,121,-132,119,-132,122,-132,120,-132,135,-132,13,-132,16,-132,6,-132,98,-132,9,-132,12,-132,5,-132,90,-132,10,-132,96,-132,99,-132,31,-132,102,-132,2,-132,97,-132,30,-132,84,-132,83,-132,82,-132,81,-132,80,-132,85,-132},new int[]{-197,201,-191,206});
    states[201] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-280,202,-176,203,-143,205,-147,48,-148,51});
    states[202] = new State(-137);
    states[203] = new State(new int[]{7,167,4,170,121,172,134,-615,136,-615,116,-615,115,-615,129,-615,130,-615,131,-615,132,-615,128,-615,114,-615,113,-615,126,-615,127,-615,118,-615,123,-615,119,-615,122,-615,120,-615,135,-615,13,-615,16,-615,6,-615,98,-615,9,-615,12,-615,5,-615,90,-615,10,-615,96,-615,99,-615,31,-615,102,-615,2,-615,97,-615,30,-615,84,-615,83,-615,82,-615,81,-615,80,-615,85,-615,11,-615,8,-615,124,-615,133,-615,75,-615,49,-615,56,-615,139,-615,141,-615,79,-615,77,-615,157,-615,43,-615,40,-615,19,-615,20,-615,142,-615,144,-615,143,-615,152,-615,155,-615,154,-615,153,-615,55,-615,89,-615,38,-615,23,-615,95,-615,52,-615,33,-615,53,-615,100,-615,45,-615,34,-615,51,-615,58,-615,73,-615,71,-615,36,-615,69,-615,70,-615},new int[]{-296,204});
    states[204] = new State(-616);
    states[205] = new State(-259);
    states[206] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-10,207,-265,208,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-11,842});
    states[207] = new State(-144);
    states[208] = new State(-145);
    states[209] = new State(new int[]{4,211,11,213,7,820,140,822,8,823,134,-155,136,-155,116,-155,115,-155,129,-155,130,-155,131,-155,132,-155,128,-155,114,-155,113,-155,126,-155,127,-155,118,-155,123,-155,121,-155,119,-155,122,-155,120,-155,135,-155,13,-155,16,-155,6,-155,98,-155,9,-155,12,-155,5,-155,90,-155,10,-155,96,-155,99,-155,31,-155,102,-155,2,-155,97,-155,30,-155,84,-155,83,-155,82,-155,81,-155,80,-155,85,-155,117,-153},new int[]{-12,210});
    states[210] = new State(-174);
    states[211] = new State(new int[]{121,172},new int[]{-296,212});
    states[212] = new State(-175);
    states[213] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161,5,1480,12,-184},new int[]{-117,214,-73,216,-87,218,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848,-71,187,-91,1484});
    states[214] = new State(new int[]{12,215});
    states[215] = new State(-176);
    states[216] = new State(new int[]{12,217});
    states[217] = new State(-180);
    states[218] = new State(new int[]{5,219,13,191,16,195,6,1478,98,-187,12,-187});
    states[219] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161,5,-693,12,-693},new int[]{-118,220,-87,1477,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[220] = new State(new int[]{5,221,12,-698});
    states[221] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-87,222,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[222] = new State(new int[]{13,191,16,195,12,-700});
    states[223] = new State(new int[]{118,224,123,225,121,226,119,227,122,228,120,229,135,230,13,-120,16,-120,6,-120,98,-120,9,-120,12,-120,5,-120,90,-120,10,-120,96,-120,99,-120,31,-120,102,-120,2,-120,97,-120,30,-120,84,-120,83,-120,82,-120,81,-120,80,-120,85,-120},new int[]{-188,197});
    states[224] = new State(-124);
    states[225] = new State(-125);
    states[226] = new State(-126);
    states[227] = new State(-127);
    states[228] = new State(-128);
    states[229] = new State(-129);
    states[230] = new State(-130);
    states[231] = new State(new int[]{114,232,113,233,126,234,127,235,118,-118,123,-118,121,-118,119,-118,122,-118,120,-118,135,-118,13,-118,16,-118,6,-118,98,-118,9,-118,12,-118,5,-118,90,-118,10,-118,96,-118,99,-118,31,-118,102,-118,2,-118,97,-118,30,-118,84,-118,83,-118,82,-118,81,-118,80,-118,85,-118},new int[]{-189,199});
    states[232] = new State(-133);
    states[233] = new State(-134);
    states[234] = new State(-135);
    states[235] = new State(-136);
    states[236] = new State(new int[]{134,237,136,238,116,239,115,240,129,241,130,242,131,243,132,244,128,245,114,-131,113,-131,126,-131,127,-131,118,-131,123,-131,121,-131,119,-131,122,-131,120,-131,135,-131,13,-131,16,-131,6,-131,98,-131,9,-131,12,-131,5,-131,90,-131,10,-131,96,-131,99,-131,31,-131,102,-131,2,-131,97,-131,30,-131,84,-131,83,-131,82,-131,81,-131,80,-131,85,-131},new int[]{-197,201,-191,206});
    states[237] = new State(-720);
    states[238] = new State(-721);
    states[239] = new State(-146);
    states[240] = new State(-147);
    states[241] = new State(-148);
    states[242] = new State(-149);
    states[243] = new State(-150);
    states[244] = new State(-151);
    states[245] = new State(-152);
    states[246] = new State(-141);
    states[247] = new State(-168);
    states[248] = new State(new int[]{24,1466,141,47,84,49,85,50,79,52,77,53,157,54,8,-838,7,-838,140,-838,4,-838,15,-838,108,-838,109,-838,110,-838,111,-838,112,-838,90,-838,10,-838,11,-838,17,-838,5,-838,96,-838,99,-838,31,-838,102,-838,2,-838,125,-838,136,-838,134,-838,116,-838,115,-838,129,-838,130,-838,131,-838,132,-838,128,-838,114,-838,113,-838,126,-838,127,-838,124,-838,6,-838,118,-838,123,-838,121,-838,119,-838,122,-838,120,-838,135,-838,133,-838,16,-838,9,-838,98,-838,12,-838,97,-838,30,-838,83,-838,82,-838,81,-838,80,-838,13,-838,117,-838,75,-838,49,-838,56,-838,139,-838,43,-838,40,-838,19,-838,20,-838,142,-838,144,-838,143,-838,152,-838,155,-838,154,-838,153,-838,55,-838,89,-838,38,-838,23,-838,95,-838,52,-838,33,-838,53,-838,100,-838,45,-838,34,-838,51,-838,58,-838,73,-838,71,-838,36,-838,69,-838,70,-838},new int[]{-280,249,-176,203,-143,205,-147,48,-148,51});
    states[249] = new State(new int[]{11,251,8,648,90,-629,10,-629,96,-629,99,-629,31,-629,102,-629,2,-629,136,-629,134,-629,116,-629,115,-629,129,-629,130,-629,131,-629,132,-629,128,-629,114,-629,113,-629,126,-629,127,-629,124,-629,6,-629,5,-629,118,-629,123,-629,121,-629,119,-629,122,-629,120,-629,135,-629,133,-629,16,-629,9,-629,98,-629,12,-629,97,-629,30,-629,84,-629,83,-629,82,-629,81,-629,80,-629,85,-629,13,-629,75,-629,49,-629,56,-629,139,-629,141,-629,79,-629,77,-629,157,-629,43,-629,40,-629,19,-629,20,-629,142,-629,144,-629,143,-629,152,-629,155,-629,154,-629,153,-629,55,-629,89,-629,38,-629,23,-629,95,-629,52,-629,33,-629,53,-629,100,-629,45,-629,34,-629,51,-629,58,-629,73,-629,71,-629,36,-629,69,-629,70,-629},new int[]{-69,250});
    states[250] = new State(-622);
    states[251] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,651,8,652,19,268,20,273,75,462,38,600,5,609,18,674,35,683,42,687,12,-795},new int[]{-67,252,-70,378,-86,444,-85,27,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-320,827,-97,659,-321,682});
    states[252] = new State(new int[]{12,253});
    states[253] = new State(new int[]{8,255,90,-621,10,-621,96,-621,99,-621,31,-621,102,-621,2,-621,136,-621,134,-621,116,-621,115,-621,129,-621,130,-621,131,-621,132,-621,128,-621,114,-621,113,-621,126,-621,127,-621,124,-621,6,-621,5,-621,118,-621,123,-621,121,-621,119,-621,122,-621,120,-621,135,-621,133,-621,16,-621,9,-621,98,-621,12,-621,97,-621,30,-621,84,-621,83,-621,82,-621,81,-621,80,-621,85,-621,13,-621,75,-621,49,-621,56,-621,139,-621,141,-621,79,-621,77,-621,157,-621,43,-621,40,-621,19,-621,20,-621,142,-621,144,-621,143,-621,152,-621,155,-621,154,-621,153,-621,55,-621,89,-621,38,-621,23,-621,95,-621,52,-621,33,-621,53,-621,100,-621,45,-621,34,-621,51,-621,58,-621,73,-621,71,-621,36,-621,69,-621,70,-621},new int[]{-5,254});
    states[254] = new State(-623);
    states[255] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,994,133,834,114,368,113,369,61,161,9,-197},new int[]{-66,256,-65,258,-83,997,-82,261,-87,262,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848,-92,998,-239,999});
    states[256] = new State(new int[]{9,257});
    states[257] = new State(-620);
    states[258] = new State(new int[]{98,259,9,-198});
    states[259] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,994,133,834,114,368,113,369,61,161},new int[]{-83,260,-82,261,-87,262,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848,-92,998,-239,999});
    states[260] = new State(-200);
    states[261] = new State(-415);
    states[262] = new State(new int[]{13,191,16,195,98,-193,9,-193,90,-193,10,-193,96,-193,99,-193,31,-193,102,-193,2,-193,12,-193,97,-193,30,-193,84,-193,83,-193,82,-193,81,-193,80,-193,85,-193});
    states[263] = new State(-169);
    states[264] = new State(-170);
    states[265] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,266,-147,48,-148,51});
    states[266] = new State(-171);
    states[267] = new State(-172);
    states[268] = new State(new int[]{8,269});
    states[269] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-280,270,-176,203,-143,205,-147,48,-148,51});
    states[270] = new State(new int[]{9,271});
    states[271] = new State(-608);
    states[272] = new State(-173);
    states[273] = new State(new int[]{8,274});
    states[274] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-280,275,-279,277,-176,279,-143,205,-147,48,-148,51});
    states[275] = new State(new int[]{9,276});
    states[276] = new State(-609);
    states[277] = new State(new int[]{9,278});
    states[278] = new State(-610);
    states[279] = new State(new int[]{7,167,4,280,121,282,123,1464,9,-615},new int[]{-296,204,-297,1465});
    states[280] = new State(new int[]{121,282,123,1464},new int[]{-296,171,-297,281});
    states[281] = new State(-614);
    states[282] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615,119,-241,98,-241},new int[]{-294,173,-295,283,-275,287,-268,177,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-277,1429,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,1430,-220,573,-219,574,-298,1431,-276,1463});
    states[283] = new State(new int[]{119,284,98,285});
    states[284] = new State(-236);
    states[285] = new State(-241,new int[]{-276,286});
    states[286] = new State(-240);
    states[287] = new State(-237);
    states[288] = new State(new int[]{116,239,115,240,129,241,130,242,131,243,132,244,128,245,6,-250,114,-250,113,-250,126,-250,127,-250,13,-250,119,-250,98,-250,118,-250,9,-250,8,-250,136,-250,134,-250,124,-250,5,-250,123,-250,121,-250,122,-250,120,-250,135,-250,133,-250,16,-250,90,-250,10,-250,96,-250,99,-250,31,-250,102,-250,2,-250,12,-250,97,-250,30,-250,84,-250,83,-250,82,-250,81,-250,80,-250,85,-250,75,-250,49,-250,56,-250,139,-250,141,-250,79,-250,77,-250,157,-250,43,-250,40,-250,19,-250,20,-250,142,-250,144,-250,143,-250,152,-250,155,-250,154,-250,153,-250,55,-250,89,-250,38,-250,23,-250,95,-250,52,-250,33,-250,53,-250,100,-250,45,-250,34,-250,51,-250,58,-250,73,-250,71,-250,36,-250,69,-250,70,-250,125,-250,108,-250},new int[]{-191,182});
    states[289] = new State(new int[]{8,184,116,-252,115,-252,129,-252,130,-252,131,-252,132,-252,128,-252,6,-252,114,-252,113,-252,126,-252,127,-252,13,-252,119,-252,98,-252,118,-252,9,-252,136,-252,134,-252,124,-252,5,-252,123,-252,121,-252,122,-252,120,-252,135,-252,133,-252,16,-252,90,-252,10,-252,96,-252,99,-252,31,-252,102,-252,2,-252,12,-252,97,-252,30,-252,84,-252,83,-252,82,-252,81,-252,80,-252,85,-252,75,-252,49,-252,56,-252,139,-252,141,-252,79,-252,77,-252,157,-252,43,-252,40,-252,19,-252,20,-252,142,-252,144,-252,143,-252,152,-252,155,-252,154,-252,153,-252,55,-252,89,-252,38,-252,23,-252,95,-252,52,-252,33,-252,53,-252,100,-252,45,-252,34,-252,51,-252,58,-252,73,-252,71,-252,36,-252,69,-252,70,-252,125,-252,108,-252});
    states[290] = new State(new int[]{7,167,125,291,121,172,8,-254,116,-254,115,-254,129,-254,130,-254,131,-254,132,-254,128,-254,6,-254,114,-254,113,-254,126,-254,127,-254,13,-254,119,-254,98,-254,118,-254,9,-254,136,-254,134,-254,124,-254,5,-254,123,-254,122,-254,120,-254,135,-254,133,-254,16,-254,90,-254,10,-254,96,-254,99,-254,31,-254,102,-254,2,-254,12,-254,97,-254,30,-254,84,-254,83,-254,82,-254,81,-254,80,-254,85,-254,75,-254,49,-254,56,-254,139,-254,141,-254,79,-254,77,-254,157,-254,43,-254,40,-254,19,-254,20,-254,142,-254,144,-254,143,-254,152,-254,155,-254,154,-254,153,-254,55,-254,89,-254,38,-254,23,-254,95,-254,52,-254,33,-254,53,-254,100,-254,45,-254,34,-254,51,-254,58,-254,73,-254,71,-254,36,-254,69,-254,70,-254,108,-254},new int[]{-296,647});
    states[291] = new State(new int[]{8,293,141,47,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-275,292,-268,177,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-277,1429,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,1430,-220,573,-219,574,-298,1431});
    states[292] = new State(-289);
    states[293] = new State(new int[]{9,294,141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-78,299,-76,305,-272,308,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[294] = new State(new int[]{125,295,119,-293,98,-293,118,-293,9,-293,8,-293,136,-293,134,-293,116,-293,115,-293,129,-293,130,-293,131,-293,132,-293,128,-293,114,-293,113,-293,126,-293,127,-293,124,-293,6,-293,5,-293,123,-293,121,-293,122,-293,120,-293,135,-293,133,-293,16,-293,90,-293,10,-293,96,-293,99,-293,31,-293,102,-293,2,-293,12,-293,97,-293,30,-293,84,-293,83,-293,82,-293,81,-293,80,-293,85,-293,13,-293,75,-293,49,-293,56,-293,139,-293,141,-293,79,-293,77,-293,157,-293,43,-293,40,-293,19,-293,20,-293,142,-293,144,-293,143,-293,152,-293,155,-293,154,-293,153,-293,55,-293,89,-293,38,-293,23,-293,95,-293,52,-293,33,-293,53,-293,100,-293,45,-293,34,-293,51,-293,58,-293,73,-293,71,-293,36,-293,69,-293,70,-293,108,-293});
    states[295] = new State(new int[]{8,297,141,47,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-275,296,-268,177,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-277,1429,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,1430,-220,573,-219,574,-298,1431});
    states[296] = new State(-291);
    states[297] = new State(new int[]{9,298,141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-78,299,-76,305,-272,308,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[298] = new State(new int[]{125,295,119,-295,98,-295,118,-295,9,-295,8,-295,136,-295,134,-295,116,-295,115,-295,129,-295,130,-295,131,-295,132,-295,128,-295,114,-295,113,-295,126,-295,127,-295,124,-295,6,-295,5,-295,123,-295,121,-295,122,-295,120,-295,135,-295,133,-295,16,-295,90,-295,10,-295,96,-295,99,-295,31,-295,102,-295,2,-295,12,-295,97,-295,30,-295,84,-295,83,-295,82,-295,81,-295,80,-295,85,-295,13,-295,75,-295,49,-295,56,-295,139,-295,141,-295,79,-295,77,-295,157,-295,43,-295,40,-295,19,-295,20,-295,142,-295,144,-295,143,-295,152,-295,155,-295,154,-295,153,-295,55,-295,89,-295,38,-295,23,-295,95,-295,52,-295,33,-295,53,-295,100,-295,45,-295,34,-295,51,-295,58,-295,73,-295,71,-295,36,-295,69,-295,70,-295,108,-295});
    states[299] = new State(new int[]{9,300,98,668});
    states[300] = new State(new int[]{125,301,13,-249,119,-249,98,-249,118,-249,9,-249,8,-249,136,-249,134,-249,116,-249,115,-249,129,-249,130,-249,131,-249,132,-249,128,-249,114,-249,113,-249,126,-249,127,-249,124,-249,6,-249,5,-249,123,-249,121,-249,122,-249,120,-249,135,-249,133,-249,16,-249,90,-249,10,-249,96,-249,99,-249,31,-249,102,-249,2,-249,12,-249,97,-249,30,-249,84,-249,83,-249,82,-249,81,-249,80,-249,85,-249,75,-249,49,-249,56,-249,139,-249,141,-249,79,-249,77,-249,157,-249,43,-249,40,-249,19,-249,20,-249,142,-249,144,-249,143,-249,152,-249,155,-249,154,-249,153,-249,55,-249,89,-249,38,-249,23,-249,95,-249,52,-249,33,-249,53,-249,100,-249,45,-249,34,-249,51,-249,58,-249,73,-249,71,-249,36,-249,69,-249,70,-249,108,-249});
    states[301] = new State(new int[]{8,303,141,47,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-275,302,-268,177,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-277,1429,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,1430,-220,573,-219,574,-298,1431});
    states[302] = new State(-292);
    states[303] = new State(new int[]{9,304,141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-78,299,-76,305,-272,308,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[304] = new State(new int[]{125,295,119,-296,98,-296,118,-296,9,-296,8,-296,136,-296,134,-296,116,-296,115,-296,129,-296,130,-296,131,-296,132,-296,128,-296,114,-296,113,-296,126,-296,127,-296,124,-296,6,-296,5,-296,123,-296,121,-296,122,-296,120,-296,135,-296,133,-296,16,-296,90,-296,10,-296,96,-296,99,-296,31,-296,102,-296,2,-296,12,-296,97,-296,30,-296,84,-296,83,-296,82,-296,81,-296,80,-296,85,-296,13,-296,75,-296,49,-296,56,-296,139,-296,141,-296,79,-296,77,-296,157,-296,43,-296,40,-296,19,-296,20,-296,142,-296,144,-296,143,-296,152,-296,155,-296,154,-296,153,-296,55,-296,89,-296,38,-296,23,-296,95,-296,52,-296,33,-296,53,-296,100,-296,45,-296,34,-296,51,-296,58,-296,73,-296,71,-296,36,-296,69,-296,70,-296,108,-296});
    states[305] = new State(new int[]{98,306});
    states[306] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-76,307,-272,308,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[307] = new State(-261);
    states[308] = new State(new int[]{118,309,98,-263,9,-263});
    states[309] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-85,310,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[310] = new State(-264);
    states[311] = new State(new int[]{118,312,123,313,121,314,119,315,122,316,120,317,135,318,133,319,16,-606,90,-606,10,-606,96,-606,99,-606,31,-606,102,-606,2,-606,9,-606,98,-606,12,-606,97,-606,30,-606,84,-606,83,-606,82,-606,81,-606,80,-606,85,-606,13,-606,6,-606,75,-606,5,-606,49,-606,56,-606,139,-606,141,-606,79,-606,77,-606,157,-606,43,-606,40,-606,8,-606,19,-606,20,-606,142,-606,144,-606,143,-606,152,-606,155,-606,154,-606,153,-606,55,-606,89,-606,38,-606,23,-606,95,-606,52,-606,33,-606,53,-606,100,-606,45,-606,34,-606,51,-606,58,-606,73,-606,71,-606,36,-606,69,-606,70,-606,114,-606,113,-606,126,-606,127,-606,124,-606,136,-606,134,-606,116,-606,115,-606,129,-606,130,-606,131,-606,132,-606,128,-606},new int[]{-192,32});
    states[312] = new State(-702);
    states[313] = new State(-703);
    states[314] = new State(-704);
    states[315] = new State(-705);
    states[316] = new State(-706);
    states[317] = new State(-707);
    states[318] = new State(-708);
    states[319] = new State(new int[]{135,320});
    states[320] = new State(-709);
    states[321] = new State(new int[]{6,34,5,322,118,-631,123,-631,121,-631,119,-631,122,-631,120,-631,135,-631,133,-631,16,-631,90,-631,10,-631,96,-631,99,-631,31,-631,102,-631,2,-631,9,-631,98,-631,12,-631,97,-631,30,-631,84,-631,83,-631,82,-631,81,-631,80,-631,85,-631,13,-631,75,-631});
    states[322] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,5,-691,90,-691,10,-691,96,-691,99,-691,31,-691,102,-691,2,-691,9,-691,98,-691,12,-691,97,-691,30,-691,83,-691,82,-691,81,-691,80,-691,6,-691},new int[]{-111,323,-102,614,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,613,-263,590});
    states[323] = new State(new int[]{5,324,90,-694,10,-694,96,-694,99,-694,31,-694,102,-694,2,-694,9,-694,98,-694,12,-694,97,-694,30,-694,84,-694,83,-694,82,-694,81,-694,80,-694,85,-694,6,-694,75,-694});
    states[324] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462},new int[]{-102,325,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,613,-263,590});
    states[325] = new State(new int[]{6,34,90,-696,10,-696,96,-696,99,-696,31,-696,102,-696,2,-696,9,-696,98,-696,12,-696,97,-696,30,-696,84,-696,83,-696,82,-696,81,-696,80,-696,85,-696,75,-696});
    states[326] = new State(new int[]{114,327,113,328,126,329,127,330,124,331,6,-710,5,-710,118,-710,123,-710,121,-710,119,-710,122,-710,120,-710,135,-710,133,-710,16,-710,90,-710,10,-710,96,-710,99,-710,31,-710,102,-710,2,-710,9,-710,98,-710,12,-710,97,-710,30,-710,84,-710,83,-710,82,-710,81,-710,80,-710,85,-710,13,-710,75,-710,49,-710,56,-710,139,-710,141,-710,79,-710,77,-710,157,-710,43,-710,40,-710,8,-710,19,-710,20,-710,142,-710,144,-710,143,-710,152,-710,155,-710,154,-710,153,-710,55,-710,89,-710,38,-710,23,-710,95,-710,52,-710,33,-710,53,-710,100,-710,45,-710,34,-710,51,-710,58,-710,73,-710,71,-710,36,-710,69,-710,70,-710,136,-710,134,-710,116,-710,115,-710,129,-710,130,-710,131,-710,132,-710,128,-710},new int[]{-193,36});
    states[327] = new State(-715);
    states[328] = new State(-716);
    states[329] = new State(-717);
    states[330] = new State(-718);
    states[331] = new State(-719);
    states[332] = new State(new int[]{136,333,134,1453,116,1456,115,1457,129,1458,130,1459,131,1460,132,1461,128,1462,114,-712,113,-712,126,-712,127,-712,124,-712,6,-712,5,-712,118,-712,123,-712,121,-712,119,-712,122,-712,120,-712,135,-712,133,-712,16,-712,90,-712,10,-712,96,-712,99,-712,31,-712,102,-712,2,-712,9,-712,98,-712,12,-712,97,-712,30,-712,84,-712,83,-712,82,-712,81,-712,80,-712,85,-712,13,-712,75,-712,49,-712,56,-712,139,-712,141,-712,79,-712,77,-712,157,-712,43,-712,40,-712,8,-712,19,-712,20,-712,142,-712,144,-712,143,-712,152,-712,155,-712,154,-712,153,-712,55,-712,89,-712,38,-712,23,-712,95,-712,52,-712,33,-712,53,-712,100,-712,45,-712,34,-712,51,-712,58,-712,73,-712,71,-712,36,-712,69,-712,70,-712},new int[]{-194,38});
    states[333] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,22,336},new int[]{-280,334,-274,335,-176,203,-143,205,-147,48,-148,51,-266,510});
    states[334] = new State(-726);
    states[335] = new State(-727);
    states[336] = new State(new int[]{11,337,56,1451});
    states[337] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,665,12,-280,98,-280},new int[]{-160,338,-267,1450,-268,1449,-90,179,-103,288,-104,289,-176,491,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153});
    states[338] = new State(new int[]{12,339,98,1447});
    states[339] = new State(new int[]{56,340});
    states[340] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-272,341,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[341] = new State(-274);
    states[342] = new State(new int[]{13,343,118,-226,98,-226,9,-226,119,-226,8,-226,136,-226,134,-226,116,-226,115,-226,129,-226,130,-226,131,-226,132,-226,128,-226,114,-226,113,-226,126,-226,127,-226,124,-226,6,-226,5,-226,123,-226,121,-226,122,-226,120,-226,135,-226,133,-226,16,-226,90,-226,10,-226,96,-226,99,-226,31,-226,102,-226,2,-226,12,-226,97,-226,30,-226,84,-226,83,-226,82,-226,81,-226,80,-226,85,-226,75,-226,49,-226,56,-226,139,-226,141,-226,79,-226,77,-226,157,-226,43,-226,40,-226,19,-226,20,-226,142,-226,144,-226,143,-226,152,-226,155,-226,154,-226,153,-226,55,-226,89,-226,38,-226,23,-226,95,-226,52,-226,33,-226,53,-226,100,-226,45,-226,34,-226,51,-226,58,-226,73,-226,71,-226,36,-226,69,-226,70,-226,125,-226,108,-226});
    states[343] = new State(-224);
    states[344] = new State(new int[]{11,345,7,-815,125,-815,121,-815,8,-815,116,-815,115,-815,129,-815,130,-815,131,-815,132,-815,128,-815,6,-815,114,-815,113,-815,126,-815,127,-815,13,-815,118,-815,98,-815,9,-815,119,-815,136,-815,134,-815,124,-815,5,-815,123,-815,122,-815,120,-815,135,-815,133,-815,16,-815,90,-815,10,-815,96,-815,99,-815,31,-815,102,-815,2,-815,12,-815,97,-815,30,-815,84,-815,83,-815,82,-815,81,-815,80,-815,85,-815,75,-815,49,-815,56,-815,139,-815,141,-815,79,-815,77,-815,157,-815,43,-815,40,-815,19,-815,20,-815,142,-815,144,-815,143,-815,152,-815,155,-815,154,-815,153,-815,55,-815,89,-815,38,-815,23,-815,95,-815,52,-815,33,-815,53,-815,100,-815,45,-815,34,-815,51,-815,58,-815,73,-815,71,-815,36,-815,69,-815,70,-815,108,-815});
    states[345] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-87,346,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[346] = new State(new int[]{12,347,13,191,16,195});
    states[347] = new State(-284);
    states[348] = new State(-156);
    states[349] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609,12,-797},new int[]{-68,350,-75,352,-89,362,-85,355,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[350] = new State(new int[]{12,351});
    states[351] = new State(-164);
    states[352] = new State(new int[]{98,353,12,-796,75,-796});
    states[353] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-89,354,-85,355,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[354] = new State(-799);
    states[355] = new State(new int[]{6,356,98,-800,12,-800,75,-800});
    states[356] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-85,357,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[357] = new State(-801);
    states[358] = new State(-731);
    states[359] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609,12,-797},new int[]{-68,360,-75,352,-89,362,-85,355,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[360] = new State(new int[]{12,361});
    states[361] = new State(-752);
    states[362] = new State(-798);
    states[363] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,53,157,54,43,391,40,429,8,431,19,268,20,273,75,462},new int[]{-93,364,-15,365,-161,148,-163,149,-162,153,-16,155,-57,160,-195,366,-109,372,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468});
    states[364] = new State(-753);
    states[365] = new State(new int[]{7,44,136,-750,134,-750,116,-750,115,-750,129,-750,130,-750,131,-750,132,-750,128,-750,114,-750,113,-750,126,-750,127,-750,124,-750,6,-750,5,-750,118,-750,123,-750,121,-750,119,-750,122,-750,120,-750,135,-750,133,-750,16,-750,90,-750,10,-750,96,-750,99,-750,31,-750,102,-750,2,-750,9,-750,98,-750,12,-750,97,-750,30,-750,84,-750,83,-750,82,-750,81,-750,80,-750,85,-750,13,-750,75,-750,49,-750,56,-750,139,-750,141,-750,79,-750,77,-750,157,-750,43,-750,40,-750,8,-750,19,-750,20,-750,142,-750,144,-750,143,-750,152,-750,155,-750,154,-750,153,-750,55,-750,89,-750,38,-750,23,-750,95,-750,52,-750,33,-750,53,-750,100,-750,45,-750,34,-750,51,-750,58,-750,73,-750,71,-750,36,-750,69,-750,70,-750,11,-774,17,-774});
    states[366] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,53,157,54,43,391,40,429,8,431,19,268,20,273,75,462},new int[]{-93,367,-15,365,-161,148,-163,149,-162,153,-16,155,-57,160,-195,366,-109,372,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468});
    states[367] = new State(-754);
    states[368] = new State(-166);
    states[369] = new State(-167);
    states[370] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,53,157,54,43,391,40,429,8,431,19,268,20,273,75,462},new int[]{-93,371,-15,365,-161,148,-163,149,-162,153,-16,155,-57,160,-195,366,-109,372,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468});
    states[371] = new State(-755);
    states[372] = new State(-756);
    states[373] = new State(new int[]{139,1446,141,47,84,49,85,50,79,52,77,53,157,54,43,391,40,429,8,431,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462},new int[]{-108,374,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701});
    states[374] = new State(new int[]{8,375,7,386,140,421,4,422,108,-762,109,-762,110,-762,111,-762,112,-762,90,-762,10,-762,96,-762,99,-762,31,-762,102,-762,2,-762,136,-762,134,-762,116,-762,115,-762,129,-762,130,-762,131,-762,132,-762,128,-762,114,-762,113,-762,126,-762,127,-762,124,-762,6,-762,5,-762,118,-762,123,-762,121,-762,119,-762,122,-762,120,-762,135,-762,133,-762,16,-762,9,-762,98,-762,12,-762,97,-762,30,-762,84,-762,83,-762,82,-762,81,-762,80,-762,85,-762,13,-762,117,-762,75,-762,49,-762,56,-762,139,-762,141,-762,79,-762,77,-762,157,-762,43,-762,40,-762,19,-762,20,-762,142,-762,144,-762,143,-762,152,-762,155,-762,154,-762,153,-762,55,-762,89,-762,38,-762,23,-762,95,-762,52,-762,33,-762,53,-762,100,-762,45,-762,34,-762,51,-762,58,-762,73,-762,71,-762,36,-762,69,-762,70,-762,11,-773,17,-773});
    states[375] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,651,8,652,19,268,20,273,75,462,38,600,5,609,18,674,35,683,42,687,9,-795},new int[]{-67,376,-70,378,-86,444,-85,27,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-320,827,-97,659,-321,682});
    states[376] = new State(new int[]{9,377});
    states[377] = new State(-789);
    states[378] = new State(new int[]{98,379,12,-794,9,-794});
    states[379] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,651,8,652,19,268,20,273,75,462,38,600,5,609,18,674,35,683,42,687},new int[]{-86,380,-85,27,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-320,827,-97,659,-321,682});
    states[380] = new State(-586);
    states[381] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,53,157,54,43,391,40,429,8,431,19,268,20,273,75,462},new int[]{-93,367,-264,382,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-94,471});
    states[382] = new State(-730);
    states[383] = new State(new int[]{136,-756,134,-756,116,-756,115,-756,129,-756,130,-756,131,-756,132,-756,128,-756,114,-756,113,-756,126,-756,127,-756,124,-756,6,-756,5,-756,118,-756,123,-756,121,-756,119,-756,122,-756,120,-756,135,-756,133,-756,16,-756,90,-756,10,-756,96,-756,99,-756,31,-756,102,-756,2,-756,9,-756,98,-756,12,-756,97,-756,30,-756,84,-756,83,-756,82,-756,81,-756,80,-756,85,-756,13,-756,75,-756,49,-756,56,-756,139,-756,141,-756,79,-756,77,-756,157,-756,43,-756,40,-756,8,-756,19,-756,20,-756,142,-756,144,-756,143,-756,152,-756,155,-756,154,-756,153,-756,55,-756,89,-756,38,-756,23,-756,95,-756,52,-756,33,-756,53,-756,100,-756,45,-756,34,-756,51,-756,58,-756,73,-756,71,-756,36,-756,69,-756,70,-756,117,-748});
    states[384] = new State(-765);
    states[385] = new State(new int[]{8,375,7,386,140,421,4,422,15,424,108,-763,109,-763,110,-763,111,-763,112,-763,90,-763,10,-763,96,-763,99,-763,31,-763,102,-763,2,-763,136,-763,134,-763,116,-763,115,-763,129,-763,130,-763,131,-763,132,-763,128,-763,114,-763,113,-763,126,-763,127,-763,124,-763,6,-763,5,-763,118,-763,123,-763,121,-763,119,-763,122,-763,120,-763,135,-763,133,-763,16,-763,9,-763,98,-763,12,-763,97,-763,30,-763,84,-763,83,-763,82,-763,81,-763,80,-763,85,-763,13,-763,117,-763,75,-763,49,-763,56,-763,139,-763,141,-763,79,-763,77,-763,157,-763,43,-763,40,-763,19,-763,20,-763,142,-763,144,-763,143,-763,152,-763,155,-763,154,-763,153,-763,55,-763,89,-763,38,-763,23,-763,95,-763,52,-763,33,-763,53,-763,100,-763,45,-763,34,-763,51,-763,58,-763,73,-763,71,-763,36,-763,69,-763,70,-763,11,-773,17,-773});
    states[386] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,83,57,82,58,81,59,80,60,67,61,62,62,126,63,20,64,19,65,61,66,21,67,127,68,128,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,22,78,72,79,89,80,23,81,24,82,27,83,28,84,29,85,70,86,97,87,30,88,90,89,31,90,32,91,25,92,102,93,99,94,33,95,34,96,35,97,38,98,39,99,40,100,101,101,41,102,42,103,44,104,45,105,46,106,95,107,47,108,100,109,48,110,26,111,49,112,69,113,96,114,50,115,51,116,52,117,53,118,54,119,55,120,56,121,57,122,59,123,103,124,104,125,107,126,105,127,106,128,60,129,73,130,36,131,37,132,68,133,145,134,58,135,137,136,138,137,78,138,150,139,149,140,71,141,151,142,147,143,148,144,146,145,43,391},new int[]{-144,387,-143,388,-147,48,-148,51,-290,389,-146,56,-187,390});
    states[387] = new State(-790);
    states[388] = new State(-822);
    states[389] = new State(-823);
    states[390] = new State(-824);
    states[391] = new State(new int[]{113,393,114,394,115,395,116,396,118,397,119,398,120,399,121,400,122,401,123,402,126,403,127,404,128,405,129,406,130,407,131,408,132,409,133,410,135,411,137,412,138,413,108,415,109,416,110,417,111,418,112,419,117,420},new int[]{-196,392,-190,414});
    states[392] = new State(-808);
    states[393] = new State(-930);
    states[394] = new State(-931);
    states[395] = new State(-932);
    states[396] = new State(-933);
    states[397] = new State(-934);
    states[398] = new State(-935);
    states[399] = new State(-936);
    states[400] = new State(-937);
    states[401] = new State(-938);
    states[402] = new State(-939);
    states[403] = new State(-940);
    states[404] = new State(-941);
    states[405] = new State(-942);
    states[406] = new State(-943);
    states[407] = new State(-944);
    states[408] = new State(-945);
    states[409] = new State(-946);
    states[410] = new State(-947);
    states[411] = new State(-948);
    states[412] = new State(-949);
    states[413] = new State(-950);
    states[414] = new State(-951);
    states[415] = new State(-953);
    states[416] = new State(-954);
    states[417] = new State(-955);
    states[418] = new State(-956);
    states[419] = new State(-957);
    states[420] = new State(-952);
    states[421] = new State(-792);
    states[422] = new State(new int[]{121,172},new int[]{-296,423});
    states[423] = new State(-793);
    states[424] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391,40,429,8,431,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462},new int[]{-108,425,-112,426,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701});
    states[425] = new State(new int[]{8,375,7,386,140,421,4,422,15,424,108,-760,109,-760,110,-760,111,-760,112,-760,90,-760,10,-760,96,-760,99,-760,31,-760,102,-760,2,-760,136,-760,134,-760,116,-760,115,-760,129,-760,130,-760,131,-760,132,-760,128,-760,114,-760,113,-760,126,-760,127,-760,124,-760,6,-760,5,-760,118,-760,123,-760,121,-760,119,-760,122,-760,120,-760,135,-760,133,-760,16,-760,9,-760,98,-760,12,-760,97,-760,30,-760,84,-760,83,-760,82,-760,81,-760,80,-760,85,-760,13,-760,117,-760,75,-760,49,-760,56,-760,139,-760,141,-760,79,-760,77,-760,157,-760,43,-760,40,-760,19,-760,20,-760,142,-760,144,-760,143,-760,152,-760,155,-760,154,-760,153,-760,55,-760,89,-760,38,-760,23,-760,95,-760,52,-760,33,-760,53,-760,100,-760,45,-760,34,-760,51,-760,58,-760,73,-760,71,-760,36,-760,69,-760,70,-760,11,-773,17,-773});
    states[426] = new State(-761);
    states[427] = new State(-778);
    states[428] = new State(-779);
    states[429] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,430,-147,48,-148,51});
    states[430] = new State(-780);
    states[431] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609,51,709,18,674},new int[]{-85,432,-357,434,-99,542,-98,705,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-356,708,-97,714});
    states[432] = new State(new int[]{9,433});
    states[433] = new State(-781);
    states[434] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609,51,709},new int[]{-85,435,-356,437,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[435] = new State(new int[]{9,436});
    states[436] = new State(-782);
    states[437] = new State(-777);
    states[438] = new State(-783);
    states[439] = new State(-784);
    states[440] = new State(new int[]{11,441,17,1442});
    states[441] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,651,8,652,19,268,20,273,75,462,38,600,5,609,18,674,35,683,42,687},new int[]{-70,442,-86,444,-85,27,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-320,827,-97,659,-321,682});
    states[442] = new State(new int[]{12,443,98,379});
    states[443] = new State(-786);
    states[444] = new State(-585);
    states[445] = new State(new int[]{125,446,8,-778,7,-778,140,-778,4,-778,15,-778,136,-778,134,-778,116,-778,115,-778,129,-778,130,-778,131,-778,132,-778,128,-778,114,-778,113,-778,126,-778,127,-778,124,-778,6,-778,5,-778,118,-778,123,-778,121,-778,119,-778,122,-778,120,-778,135,-778,133,-778,16,-778,90,-778,10,-778,96,-778,99,-778,31,-778,102,-778,2,-778,9,-778,98,-778,12,-778,97,-778,30,-778,84,-778,83,-778,82,-778,81,-778,80,-778,85,-778,13,-778,117,-778,11,-778,17,-778});
    states[446] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,451,19,268,20,273,75,462,18,674,35,683,42,687,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-326,447,-101,448,-96,449,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,657,-113,592,-320,658,-97,659,-321,682,-328,852,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[447] = new State(-967);
    states[448] = new State(-1004);
    states[449] = new State(new int[]{16,30,90,-603,10,-603,96,-603,99,-603,31,-603,102,-603,2,-603,9,-603,98,-603,12,-603,97,-603,30,-603,84,-603,83,-603,82,-603,81,-603,80,-603,85,-603,13,-597});
    states[450] = new State(new int[]{6,34,118,-631,123,-631,121,-631,119,-631,122,-631,120,-631,135,-631,133,-631,16,-631,90,-631,10,-631,96,-631,99,-631,31,-631,102,-631,2,-631,9,-631,98,-631,12,-631,97,-631,30,-631,84,-631,83,-631,82,-631,81,-631,80,-631,85,-631,13,-631,75,-631,5,-631,49,-631,56,-631,139,-631,141,-631,79,-631,77,-631,157,-631,43,-631,40,-631,8,-631,19,-631,20,-631,142,-631,144,-631,143,-631,152,-631,155,-631,154,-631,153,-631,55,-631,89,-631,38,-631,23,-631,95,-631,52,-631,33,-631,53,-631,100,-631,45,-631,34,-631,51,-631,58,-631,73,-631,71,-631,36,-631,69,-631,70,-631,114,-631,113,-631,126,-631,127,-631,124,-631,136,-631,134,-631,116,-631,115,-631,129,-631,130,-631,131,-631,132,-631,128,-631});
    states[451] = new State(new int[]{9,653,54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,707,19,268,20,273,75,462,38,600,5,609,51,709,18,674},new int[]{-85,432,-357,434,-99,452,-143,1100,-4,703,-98,705,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,706,-128,373,-108,385,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-356,708,-97,714});
    states[452] = new State(new int[]{98,453});
    states[453] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,18,674},new int[]{-77,454,-99,1130,-98,1129,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-97,714});
    states[454] = new State(new int[]{98,1127,5,556,10,-987,9,-987},new int[]{-322,455});
    states[455] = new State(new int[]{10,548,9,-975},new int[]{-329,456});
    states[456] = new State(new int[]{9,457});
    states[457] = new State(new int[]{5,661,7,-746,136,-746,134,-746,116,-746,115,-746,129,-746,130,-746,131,-746,132,-746,128,-746,114,-746,113,-746,126,-746,127,-746,124,-746,6,-746,118,-746,123,-746,121,-746,119,-746,122,-746,120,-746,135,-746,133,-746,16,-746,90,-746,10,-746,96,-746,99,-746,31,-746,102,-746,2,-746,9,-746,98,-746,12,-746,97,-746,30,-746,84,-746,83,-746,82,-746,81,-746,80,-746,85,-746,13,-746,125,-989},new int[]{-333,458,-323,459});
    states[458] = new State(-972);
    states[459] = new State(new int[]{125,460});
    states[460] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,451,19,268,20,273,75,462,18,674,35,683,42,687,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-326,461,-101,448,-96,449,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,657,-113,592,-320,658,-97,659,-321,682,-328,852,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[461] = new State(-977);
    states[462] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-68,463,-75,352,-89,362,-85,355,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[463] = new State(new int[]{75,464});
    states[464] = new State(-788);
    states[465] = new State(new int[]{7,466,136,-757,134,-757,116,-757,115,-757,129,-757,130,-757,131,-757,132,-757,128,-757,114,-757,113,-757,126,-757,127,-757,124,-757,6,-757,5,-757,118,-757,123,-757,121,-757,119,-757,122,-757,120,-757,135,-757,133,-757,16,-757,90,-757,10,-757,96,-757,99,-757,31,-757,102,-757,2,-757,9,-757,98,-757,12,-757,97,-757,30,-757,84,-757,83,-757,82,-757,81,-757,80,-757,85,-757,13,-757,75,-757,49,-757,56,-757,139,-757,141,-757,79,-757,77,-757,157,-757,43,-757,40,-757,8,-757,19,-757,20,-757,142,-757,144,-757,143,-757,152,-757,155,-757,154,-757,153,-757,55,-757,89,-757,38,-757,23,-757,95,-757,52,-757,33,-757,53,-757,100,-757,45,-757,34,-757,51,-757,58,-757,73,-757,71,-757,36,-757,69,-757,70,-757});
    states[466] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,83,57,82,58,81,59,80,60,67,61,62,62,126,63,20,64,19,65,61,66,21,67,127,68,128,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,22,78,72,79,89,80,23,81,24,82,27,83,28,84,29,85,70,86,97,87,30,88,90,89,31,90,32,91,25,92,102,93,99,94,33,95,34,96,35,97,38,98,39,99,40,100,101,101,41,102,42,103,44,104,45,105,46,106,95,107,47,108,100,109,48,110,26,111,49,112,69,113,96,114,50,115,51,116,52,117,53,118,54,119,55,120,56,121,57,122,59,123,103,124,104,125,107,126,105,127,106,128,60,129,73,130,36,131,37,132,68,133,145,134,58,135,137,136,138,137,78,138,150,139,149,140,71,141,151,142,147,143,148,144,146,145,43,391},new int[]{-144,467,-143,388,-147,48,-148,51,-290,389,-146,56,-187,390});
    states[467] = new State(-791);
    states[468] = new State(-764);
    states[469] = new State(-732);
    states[470] = new State(-733);
    states[471] = new State(new int[]{117,472});
    states[472] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,53,157,54,43,391,40,429,8,431,19,268,20,273,75,462},new int[]{-93,473,-264,474,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-94,471});
    states[473] = new State(-728);
    states[474] = new State(-729);
    states[475] = new State(-737);
    states[476] = new State(new int[]{8,477,136,-722,134,-722,116,-722,115,-722,129,-722,130,-722,131,-722,132,-722,128,-722,114,-722,113,-722,126,-722,127,-722,124,-722,6,-722,5,-722,118,-722,123,-722,121,-722,119,-722,122,-722,120,-722,135,-722,133,-722,16,-722,90,-722,10,-722,96,-722,99,-722,31,-722,102,-722,2,-722,9,-722,98,-722,12,-722,97,-722,30,-722,84,-722,83,-722,82,-722,81,-722,80,-722,85,-722,13,-722,75,-722,49,-722,56,-722,139,-722,141,-722,79,-722,77,-722,157,-722,43,-722,40,-722,19,-722,20,-722,142,-722,144,-722,143,-722,152,-722,155,-722,154,-722,153,-722,55,-722,89,-722,38,-722,23,-722,95,-722,52,-722,33,-722,53,-722,100,-722,45,-722,34,-722,51,-722,58,-722,73,-722,71,-722,36,-722,69,-722,70,-722});
    states[477] = new State(new int[]{14,482,142,151,144,152,143,154,152,156,155,157,154,158,153,159,51,484,141,47,84,49,85,50,79,52,77,53,157,54,11,920,8,933},new int[]{-351,478,-349,1441,-15,483,-161,148,-163,149,-162,153,-16,155,-338,1432,-280,1433,-176,203,-143,205,-147,48,-148,51,-341,1439,-342,1440});
    states[478] = new State(new int[]{9,479,10,480,98,1437});
    states[479] = new State(-634);
    states[480] = new State(new int[]{14,482,142,151,144,152,143,154,152,156,155,157,154,158,153,159,51,484,141,47,84,49,85,50,79,52,77,53,157,54,11,920,8,933},new int[]{-349,481,-15,483,-161,148,-163,149,-162,153,-16,155,-338,1432,-280,1433,-176,203,-143,205,-147,48,-148,51,-341,1439,-342,1440});
    states[481] = new State(-671);
    states[482] = new State(-673);
    states[483] = new State(-674);
    states[484] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,485,-147,48,-148,51});
    states[485] = new State(new int[]{5,486,9,-676,10,-676,98,-676});
    states[486] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-272,487,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[487] = new State(-675);
    states[488] = new State(-255);
    states[489] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154},new int[]{-104,490,-176,491,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153});
    states[490] = new State(new int[]{8,184,116,-256,115,-256,129,-256,130,-256,131,-256,132,-256,128,-256,6,-256,114,-256,113,-256,126,-256,127,-256,13,-256,119,-256,98,-256,118,-256,9,-256,136,-256,134,-256,124,-256,5,-256,123,-256,121,-256,122,-256,120,-256,135,-256,133,-256,16,-256,90,-256,10,-256,96,-256,99,-256,31,-256,102,-256,2,-256,12,-256,97,-256,30,-256,84,-256,83,-256,82,-256,81,-256,80,-256,85,-256,75,-256,49,-256,56,-256,139,-256,141,-256,79,-256,77,-256,157,-256,43,-256,40,-256,19,-256,20,-256,142,-256,144,-256,143,-256,152,-256,155,-256,154,-256,153,-256,55,-256,89,-256,38,-256,23,-256,95,-256,52,-256,33,-256,53,-256,100,-256,45,-256,34,-256,51,-256,58,-256,73,-256,71,-256,36,-256,69,-256,70,-256,125,-256,108,-256});
    states[491] = new State(new int[]{7,167,8,-254,116,-254,115,-254,129,-254,130,-254,131,-254,132,-254,128,-254,6,-254,114,-254,113,-254,126,-254,127,-254,13,-254,119,-254,98,-254,118,-254,9,-254,136,-254,134,-254,124,-254,5,-254,123,-254,121,-254,122,-254,120,-254,135,-254,133,-254,16,-254,90,-254,10,-254,96,-254,99,-254,31,-254,102,-254,2,-254,12,-254,97,-254,30,-254,84,-254,83,-254,82,-254,81,-254,80,-254,85,-254,75,-254,49,-254,56,-254,139,-254,141,-254,79,-254,77,-254,157,-254,43,-254,40,-254,19,-254,20,-254,142,-254,144,-254,143,-254,152,-254,155,-254,154,-254,153,-254,55,-254,89,-254,38,-254,23,-254,95,-254,52,-254,33,-254,53,-254,100,-254,45,-254,34,-254,51,-254,58,-254,73,-254,71,-254,36,-254,69,-254,70,-254,125,-254,108,-254});
    states[492] = new State(-257);
    states[493] = new State(new int[]{9,494,141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-78,299,-76,305,-272,308,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[494] = new State(new int[]{125,295});
    states[495] = new State(-227);
    states[496] = new State(new int[]{13,497,125,498,118,-232,98,-232,9,-232,119,-232,8,-232,136,-232,134,-232,116,-232,115,-232,129,-232,130,-232,131,-232,132,-232,128,-232,114,-232,113,-232,126,-232,127,-232,124,-232,6,-232,5,-232,123,-232,121,-232,122,-232,120,-232,135,-232,133,-232,16,-232,90,-232,10,-232,96,-232,99,-232,31,-232,102,-232,2,-232,12,-232,97,-232,30,-232,84,-232,83,-232,82,-232,81,-232,80,-232,85,-232,75,-232,49,-232,56,-232,139,-232,141,-232,79,-232,77,-232,157,-232,43,-232,40,-232,19,-232,20,-232,142,-232,144,-232,143,-232,152,-232,155,-232,154,-232,153,-232,55,-232,89,-232,38,-232,23,-232,95,-232,52,-232,33,-232,53,-232,100,-232,45,-232,34,-232,51,-232,58,-232,73,-232,71,-232,36,-232,69,-232,70,-232,108,-232});
    states[497] = new State(-225);
    states[498] = new State(new int[]{8,500,141,47,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-275,499,-268,177,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-277,1429,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,1430,-220,573,-219,574,-298,1431});
    states[499] = new State(-290);
    states[500] = new State(new int[]{9,501,141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-78,299,-76,305,-272,308,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[501] = new State(new int[]{125,295,119,-294,98,-294,118,-294,9,-294,8,-294,136,-294,134,-294,116,-294,115,-294,129,-294,130,-294,131,-294,132,-294,128,-294,114,-294,113,-294,126,-294,127,-294,124,-294,6,-294,5,-294,123,-294,121,-294,122,-294,120,-294,135,-294,133,-294,16,-294,90,-294,10,-294,96,-294,99,-294,31,-294,102,-294,2,-294,12,-294,97,-294,30,-294,84,-294,83,-294,82,-294,81,-294,80,-294,85,-294,13,-294,75,-294,49,-294,56,-294,139,-294,141,-294,79,-294,77,-294,157,-294,43,-294,40,-294,19,-294,20,-294,142,-294,144,-294,143,-294,152,-294,155,-294,154,-294,153,-294,55,-294,89,-294,38,-294,23,-294,95,-294,52,-294,33,-294,53,-294,100,-294,45,-294,34,-294,51,-294,58,-294,73,-294,71,-294,36,-294,69,-294,70,-294,108,-294});
    states[502] = new State(-228);
    states[503] = new State(-229);
    states[504] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-271,505,-272,506,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[505] = new State(-265);
    states[506] = new State(-478);
    states[507] = new State(-230);
    states[508] = new State(-266);
    states[509] = new State(-268);
    states[510] = new State(-275);
    states[511] = new State(-269);
    states[512] = new State(new int[]{8,1305,21,-316,11,-316,90,-316,83,-316,82,-316,81,-316,80,-316,27,-316,141,-316,84,-316,85,-316,79,-316,77,-316,157,-316,60,-316,26,-316,24,-316,42,-316,35,-316,28,-316,29,-316,44,-316,25,-316},new int[]{-179,513});
    states[513] = new State(new int[]{21,1296,11,-323,90,-323,83,-323,82,-323,81,-323,80,-323,27,-323,141,-323,84,-323,85,-323,79,-323,77,-323,157,-323,60,-323,26,-323,24,-323,42,-323,35,-323,28,-323,29,-323,44,-323,25,-323},new int[]{-315,514,-314,1294,-313,1316});
    states[514] = new State(new int[]{11,639,90,-340,83,-340,82,-340,81,-340,80,-340,27,-211,141,-211,84,-211,85,-211,79,-211,77,-211,157,-211,60,-211,26,-211,24,-211,42,-211,35,-211,28,-211,29,-211,44,-211,25,-211},new int[]{-25,515,-32,1274,-34,519,-45,1275,-6,1276,-246,1125,-33,1385,-54,1387,-53,525,-55,1386});
    states[515] = new State(new int[]{90,516,83,1270,82,1271,81,1272,80,1273},new int[]{-7,517});
    states[516] = new State(-298);
    states[517] = new State(new int[]{11,639,90,-340,83,-340,82,-340,81,-340,80,-340,27,-211,141,-211,84,-211,85,-211,79,-211,77,-211,157,-211,60,-211,26,-211,24,-211,42,-211,35,-211,28,-211,29,-211,44,-211,25,-211},new int[]{-32,518,-34,519,-45,1275,-6,1276,-246,1125,-33,1385,-54,1387,-53,525,-55,1386});
    states[518] = new State(-335);
    states[519] = new State(new int[]{10,521,90,-346,83,-346,82,-346,81,-346,80,-346},new int[]{-186,520});
    states[520] = new State(-341);
    states[521] = new State(new int[]{11,639,90,-347,83,-347,82,-347,81,-347,80,-347,27,-211,141,-211,84,-211,85,-211,79,-211,77,-211,157,-211,60,-211,26,-211,24,-211,42,-211,35,-211,28,-211,29,-211,44,-211,25,-211},new int[]{-45,522,-33,523,-6,1276,-246,1125,-54,1387,-53,525,-55,1386});
    states[522] = new State(-349);
    states[523] = new State(new int[]{11,639,90,-343,83,-343,82,-343,81,-343,80,-343,26,-211,24,-211,42,-211,35,-211,28,-211,29,-211,44,-211,25,-211},new int[]{-54,524,-53,525,-6,526,-246,1125,-55,1386});
    states[524] = new State(-352);
    states[525] = new State(-353);
    states[526] = new State(new int[]{26,1341,24,1342,42,1289,35,1324,28,1356,29,1363,11,639,44,1370,25,1379},new int[]{-218,527,-246,528,-215,529,-254,530,-3,531,-226,1343,-224,1218,-221,1288,-225,1323,-223,1344,-211,1367,-212,1368,-214,1369});
    states[527] = new State(-362);
    states[528] = new State(-210);
    states[529] = new State(-363);
    states[530] = new State(-377);
    states[531] = new State(new int[]{28,533,44,1172,25,1210,42,1289,35,1324},new int[]{-226,532,-212,1171,-224,1218,-221,1288,-225,1323});
    states[532] = new State(-366);
    states[533] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391,8,-376,108,-376,10,-376},new int[]{-168,534,-167,1154,-166,1155,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[534] = new State(new int[]{8,577,108,-462,10,-462},new int[]{-124,535});
    states[535] = new State(new int[]{108,537,10,1143},new int[]{-203,536});
    states[536] = new State(-373);
    states[537] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486},new int[]{-256,538,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[538] = new State(new int[]{10,539});
    states[539] = new State(-421);
    states[540] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,90,-567,10,-567,96,-567,99,-567,31,-567,102,-567,2,-567,9,-567,98,-567,12,-567,97,-567,30,-567,83,-567,82,-567,81,-567,80,-567},new int[]{-143,430,-147,48,-148,51});
    states[541] = new State(new int[]{51,1131,54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609,18,674},new int[]{-85,432,-357,434,-99,542,-108,692,-98,705,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-356,708,-97,714});
    states[542] = new State(new int[]{98,543});
    states[543] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,18,674},new int[]{-77,544,-99,1130,-98,1129,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-97,714});
    states[544] = new State(new int[]{98,1127,5,556,10,-987,9,-987},new int[]{-322,545});
    states[545] = new State(new int[]{10,548,9,-975},new int[]{-329,546});
    states[546] = new State(new int[]{9,547});
    states[547] = new State(-746);
    states[548] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-324,549,-325,1089,-154,552,-143,803,-147,48,-148,51});
    states[549] = new State(new int[]{10,550,9,-976});
    states[550] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-325,551,-154,552,-143,803,-147,48,-148,51});
    states[551] = new State(-985);
    states[552] = new State(new int[]{98,554,5,556,10,-987,9,-987},new int[]{-322,553});
    states[553] = new State(-986);
    states[554] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,555,-147,48,-148,51});
    states[555] = new State(-345);
    states[556] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-271,557,-272,506,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[557] = new State(-988);
    states[558] = new State(-270);
    states[559] = new State(new int[]{56,560});
    states[560] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-272,561,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[561] = new State(-281);
    states[562] = new State(-271);
    states[563] = new State(new int[]{56,564,119,-283,98,-283,118,-283,9,-283,8,-283,136,-283,134,-283,116,-283,115,-283,129,-283,130,-283,131,-283,132,-283,128,-283,114,-283,113,-283,126,-283,127,-283,124,-283,6,-283,5,-283,123,-283,121,-283,122,-283,120,-283,135,-283,133,-283,16,-283,90,-283,10,-283,96,-283,99,-283,31,-283,102,-283,2,-283,12,-283,97,-283,30,-283,84,-283,83,-283,82,-283,81,-283,80,-283,85,-283,13,-283,75,-283,49,-283,139,-283,141,-283,79,-283,77,-283,157,-283,43,-283,40,-283,19,-283,20,-283,142,-283,144,-283,143,-283,152,-283,155,-283,154,-283,153,-283,55,-283,89,-283,38,-283,23,-283,95,-283,52,-283,33,-283,53,-283,100,-283,45,-283,34,-283,51,-283,58,-283,73,-283,71,-283,36,-283,69,-283,70,-283,125,-283,108,-283});
    states[564] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-272,565,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[565] = new State(-282);
    states[566] = new State(-272);
    states[567] = new State(new int[]{56,568});
    states[568] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-272,569,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[569] = new State(-273);
    states[570] = new State(new int[]{22,336,46,512,47,559,32,563,72,567},new int[]{-278,571,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566});
    states[571] = new State(-267);
    states[572] = new State(-231);
    states[573] = new State(-285);
    states[574] = new State(-286);
    states[575] = new State(new int[]{8,577,119,-462,98,-462,118,-462,9,-462,136,-462,134,-462,116,-462,115,-462,129,-462,130,-462,131,-462,132,-462,128,-462,114,-462,113,-462,126,-462,127,-462,124,-462,6,-462,5,-462,123,-462,121,-462,122,-462,120,-462,135,-462,133,-462,16,-462,90,-462,10,-462,96,-462,99,-462,31,-462,102,-462,2,-462,12,-462,97,-462,30,-462,84,-462,83,-462,82,-462,81,-462,80,-462,85,-462,13,-462,75,-462,49,-462,56,-462,139,-462,141,-462,79,-462,77,-462,157,-462,43,-462,40,-462,19,-462,20,-462,142,-462,144,-462,143,-462,152,-462,155,-462,154,-462,153,-462,55,-462,89,-462,38,-462,23,-462,95,-462,52,-462,33,-462,53,-462,100,-462,45,-462,34,-462,51,-462,58,-462,73,-462,71,-462,36,-462,69,-462,70,-462,125,-462,108,-462},new int[]{-124,576});
    states[576] = new State(-287);
    states[577] = new State(new int[]{9,578,11,639,141,-211,84,-211,85,-211,79,-211,77,-211,157,-211,51,-211,27,-211,106,-211},new int[]{-125,579,-56,1126,-6,583,-246,1125});
    states[578] = new State(-463);
    states[579] = new State(new int[]{9,580,10,581});
    states[580] = new State(-464);
    states[581] = new State(new int[]{11,639,141,-211,84,-211,85,-211,79,-211,77,-211,157,-211,51,-211,27,-211,106,-211},new int[]{-56,582,-6,583,-246,1125});
    states[582] = new State(-466);
    states[583] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,51,623,27,629,106,635,11,639},new int[]{-293,584,-246,528,-155,585,-131,622,-143,621,-147,48,-148,51});
    states[584] = new State(-467);
    states[585] = new State(new int[]{5,586,98,619});
    states[586] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-271,587,-272,506,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[587] = new State(new int[]{108,588,9,-468,10,-468});
    states[588] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-85,589,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[589] = new State(-472);
    states[590] = new State(-723);
    states[591] = new State(new int[]{90,-595,10,-595,96,-595,99,-595,31,-595,102,-595,2,-595,9,-595,98,-595,12,-595,97,-595,30,-595,84,-595,83,-595,82,-595,81,-595,80,-595,85,-595,6,-595,75,-595,5,-595,49,-595,56,-595,139,-595,141,-595,79,-595,77,-595,157,-595,43,-595,40,-595,8,-595,19,-595,20,-595,142,-595,144,-595,143,-595,152,-595,155,-595,154,-595,153,-595,55,-595,89,-595,38,-595,23,-595,95,-595,52,-595,33,-595,53,-595,100,-595,45,-595,34,-595,51,-595,58,-595,73,-595,71,-595,36,-595,69,-595,70,-595,13,-598});
    states[592] = new State(new int[]{13,593});
    states[593] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462},new int[]{-113,594,-96,597,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,598});
    states[594] = new State(new int[]{5,595,13,593});
    states[595] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462},new int[]{-113,596,-96,597,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,598});
    states[596] = new State(new int[]{13,593,90,-611,10,-611,96,-611,99,-611,31,-611,102,-611,2,-611,9,-611,98,-611,12,-611,97,-611,30,-611,84,-611,83,-611,82,-611,81,-611,80,-611,85,-611,6,-611,75,-611,5,-611,49,-611,56,-611,139,-611,141,-611,79,-611,77,-611,157,-611,43,-611,40,-611,8,-611,19,-611,20,-611,142,-611,144,-611,143,-611,152,-611,155,-611,154,-611,153,-611,55,-611,89,-611,38,-611,23,-611,95,-611,52,-611,33,-611,53,-611,100,-611,45,-611,34,-611,51,-611,58,-611,73,-611,71,-611,36,-611,69,-611,70,-611});
    states[597] = new State(new int[]{16,30,5,-597,13,-597,90,-597,10,-597,96,-597,99,-597,31,-597,102,-597,2,-597,9,-597,98,-597,12,-597,97,-597,30,-597,84,-597,83,-597,82,-597,81,-597,80,-597,85,-597,6,-597,75,-597,49,-597,56,-597,139,-597,141,-597,79,-597,77,-597,157,-597,43,-597,40,-597,8,-597,19,-597,20,-597,142,-597,144,-597,143,-597,152,-597,155,-597,154,-597,153,-597,55,-597,89,-597,38,-597,23,-597,95,-597,52,-597,33,-597,53,-597,100,-597,45,-597,34,-597,51,-597,58,-597,73,-597,71,-597,36,-597,69,-597,70,-597});
    states[598] = new State(-598);
    states[599] = new State(-596);
    states[600] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-114,601,-96,606,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-238,607});
    states[601] = new State(new int[]{49,602});
    states[602] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-114,603,-96,606,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-238,607});
    states[603] = new State(new int[]{30,604});
    states[604] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-114,605,-96,606,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-238,607});
    states[605] = new State(-612);
    states[606] = new State(new int[]{16,30,49,-599,30,-599,118,-599,123,-599,121,-599,119,-599,122,-599,120,-599,135,-599,133,-599,90,-599,10,-599,96,-599,99,-599,31,-599,102,-599,2,-599,9,-599,98,-599,12,-599,97,-599,84,-599,83,-599,82,-599,81,-599,80,-599,85,-599,13,-599,6,-599,75,-599,5,-599,56,-599,139,-599,141,-599,79,-599,77,-599,157,-599,43,-599,40,-599,8,-599,19,-599,20,-599,142,-599,144,-599,143,-599,152,-599,155,-599,154,-599,153,-599,55,-599,89,-599,38,-599,23,-599,95,-599,52,-599,33,-599,53,-599,100,-599,45,-599,34,-599,51,-599,58,-599,73,-599,71,-599,36,-599,69,-599,70,-599,114,-599,113,-599,126,-599,127,-599,124,-599,136,-599,134,-599,116,-599,115,-599,129,-599,130,-599,131,-599,132,-599,128,-599});
    states[607] = new State(-600);
    states[608] = new State(-593);
    states[609] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,5,-691,90,-691,10,-691,96,-691,99,-691,31,-691,102,-691,2,-691,9,-691,98,-691,12,-691,97,-691,30,-691,83,-691,82,-691,81,-691,80,-691,6,-691},new int[]{-111,610,-102,614,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,613,-263,590});
    states[610] = new State(new int[]{5,611,90,-695,10,-695,96,-695,99,-695,31,-695,102,-695,2,-695,9,-695,98,-695,12,-695,97,-695,30,-695,84,-695,83,-695,82,-695,81,-695,80,-695,85,-695,6,-695,75,-695});
    states[611] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462},new int[]{-102,612,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,613,-263,590});
    states[612] = new State(new int[]{6,34,90,-697,10,-697,96,-697,99,-697,31,-697,102,-697,2,-697,9,-697,98,-697,12,-697,97,-697,30,-697,84,-697,83,-697,82,-697,81,-697,80,-697,85,-697,75,-697});
    states[613] = new State(-722);
    states[614] = new State(new int[]{6,34,5,-690,90,-690,10,-690,96,-690,99,-690,31,-690,102,-690,2,-690,9,-690,98,-690,12,-690,97,-690,30,-690,84,-690,83,-690,82,-690,81,-690,80,-690,85,-690,75,-690});
    states[615] = new State(new int[]{8,577,5,-462},new int[]{-124,616});
    states[616] = new State(new int[]{5,617});
    states[617] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-271,618,-272,506,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[618] = new State(-288);
    states[619] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-131,620,-143,621,-147,48,-148,51});
    states[620] = new State(-476);
    states[621] = new State(-477);
    states[622] = new State(-475);
    states[623] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-155,624,-131,622,-143,621,-147,48,-148,51});
    states[624] = new State(new int[]{5,625,98,619});
    states[625] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-271,626,-272,506,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[626] = new State(new int[]{108,627,9,-469,10,-469});
    states[627] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-85,628,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[628] = new State(-473);
    states[629] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-155,630,-131,622,-143,621,-147,48,-148,51});
    states[630] = new State(new int[]{5,631,98,619});
    states[631] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-271,632,-272,506,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[632] = new State(new int[]{108,633,9,-470,10,-470});
    states[633] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-85,634,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[634] = new State(-474);
    states[635] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-155,636,-131,622,-143,621,-147,48,-148,51});
    states[636] = new State(new int[]{5,637,98,619});
    states[637] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-271,638,-272,506,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[638] = new State(-471);
    states[639] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-247,640,-8,1124,-9,644,-176,645,-143,1119,-147,48,-148,51,-298,1122});
    states[640] = new State(new int[]{12,641,98,642});
    states[641] = new State(-212);
    states[642] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-8,643,-9,644,-176,645,-143,1119,-147,48,-148,51,-298,1122});
    states[643] = new State(-214);
    states[644] = new State(-215);
    states[645] = new State(new int[]{7,167,8,648,121,172,12,-629,98,-629},new int[]{-69,646,-296,647});
    states[646] = new State(-767);
    states[647] = new State(-233);
    states[648] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,651,8,652,19,268,20,273,75,462,38,600,5,609,18,674,35,683,42,687,9,-795},new int[]{-67,649,-70,378,-86,444,-85,27,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-320,827,-97,659,-321,682});
    states[649] = new State(new int[]{9,650});
    states[650] = new State(-630);
    states[651] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,90,-591,10,-591,96,-591,99,-591,31,-591,102,-591,2,-591,9,-591,98,-591,12,-591,97,-591,30,-591,83,-591,82,-591,81,-591,80,-591},new int[]{-143,430,-147,48,-148,51});
    states[652] = new State(new int[]{9,653,54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609,51,709,18,674},new int[]{-85,432,-357,434,-99,452,-143,1100,-98,705,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-356,708,-97,714});
    states[653] = new State(new int[]{5,661,125,-989},new int[]{-323,654});
    states[654] = new State(new int[]{125,655});
    states[655] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,451,19,268,20,273,75,462,18,674,35,683,42,687,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-326,656,-101,448,-96,449,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,657,-113,592,-320,658,-97,659,-321,682,-328,852,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[656] = new State(-968);
    states[657] = new State(new int[]{90,-604,10,-604,96,-604,99,-604,31,-604,102,-604,2,-604,9,-604,98,-604,12,-604,97,-604,30,-604,84,-604,83,-604,82,-604,81,-604,80,-604,85,-604,13,-598});
    states[658] = new State(-605);
    states[659] = new State(new int[]{5,661,125,-989},new int[]{-333,660,-323,459});
    states[660] = new State(-973);
    states[661] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,665,140,504,22,336,46,512,47,559,32,563,72,567,63,570},new int[]{-273,662,-268,663,-90,179,-103,288,-104,289,-176,664,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-252,670,-245,671,-277,672,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-298,673});
    states[662] = new State(-990);
    states[663] = new State(-479);
    states[664] = new State(new int[]{7,167,121,172,8,-254,116,-254,115,-254,129,-254,130,-254,131,-254,132,-254,128,-254,6,-254,114,-254,113,-254,126,-254,127,-254,125,-254},new int[]{-296,647});
    states[665] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-78,666,-76,305,-272,308,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[666] = new State(new int[]{9,667,98,668});
    states[667] = new State(-249);
    states[668] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-76,669,-272,308,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[669] = new State(-262);
    states[670] = new State(-480);
    states[671] = new State(-481);
    states[672] = new State(-482);
    states[673] = new State(-483);
    states[674] = new State(new int[]{18,674,141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-24,675,-23,681,-97,679,-143,680,-147,48,-148,51});
    states[675] = new State(new int[]{98,676});
    states[676] = new State(new int[]{18,674,141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-23,677,-97,679,-143,680,-147,48,-148,51});
    states[677] = new State(new int[]{9,678,98,-962});
    states[678] = new State(-958);
    states[679] = new State(-959);
    states[680] = new State(-960);
    states[681] = new State(-961);
    states[682] = new State(-974);
    states[683] = new State(new int[]{8,1090,5,661,125,-989},new int[]{-323,684});
    states[684] = new State(new int[]{125,685});
    states[685] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,451,19,268,20,273,75,462,18,674,35,683,42,687,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-326,686,-101,448,-96,449,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,657,-113,592,-320,658,-97,659,-321,682,-328,852,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[686] = new State(-978);
    states[687] = new State(new int[]{125,688,8,1081});
    states[688] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,53,157,54,43,391,40,429,8,691,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-327,689,-208,690,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-4,715,-328,716,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[689] = new State(-981);
    states[690] = new State(-1006);
    states[691] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,707,19,268,20,273,75,462,38,600,5,609,51,709,18,674},new int[]{-85,432,-357,434,-99,542,-108,692,-4,703,-98,705,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,706,-128,373,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-356,708,-97,714});
    states[692] = new State(new int[]{98,693,8,375,7,386,140,421,4,422,15,424,136,-763,134,-763,116,-763,115,-763,129,-763,130,-763,131,-763,132,-763,128,-763,114,-763,113,-763,126,-763,127,-763,124,-763,6,-763,5,-763,118,-763,123,-763,121,-763,119,-763,122,-763,120,-763,135,-763,133,-763,16,-763,9,-763,13,-763,117,-763,108,-763,109,-763,110,-763,111,-763,112,-763,11,-773,17,-773});
    states[693] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391,40,429,8,431,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462},new int[]{-334,694,-108,702,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701});
    states[694] = new State(new int[]{9,695,98,698});
    states[695] = new State(new int[]{108,415,109,416,110,417,111,418,112,419},new int[]{-190,696});
    states[696] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-85,697,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[697] = new State(-515);
    states[698] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391,40,429,8,431,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462},new int[]{-108,699,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701});
    states[699] = new State(new int[]{8,375,7,386,140,421,4,422,9,-517,98,-517,11,-773,17,-773});
    states[700] = new State(new int[]{7,44,11,-774,17,-774});
    states[701] = new State(new int[]{7,466});
    states[702] = new State(new int[]{8,375,7,386,140,421,4,422,9,-516,98,-516,11,-773,17,-773});
    states[703] = new State(new int[]{9,704});
    states[704] = new State(-1003);
    states[705] = new State(new int[]{9,-592,98,-963});
    states[706] = new State(new int[]{108,415,109,416,110,417,111,418,112,419,136,-756,134,-756,116,-756,115,-756,129,-756,130,-756,131,-756,132,-756,128,-756,114,-756,113,-756,126,-756,127,-756,124,-756,6,-756,5,-756,118,-756,123,-756,121,-756,119,-756,122,-756,120,-756,135,-756,133,-756,16,-756,9,-756,98,-756,13,-756,2,-756,117,-748},new int[]{-190,25});
    states[707] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609,51,709,18,674},new int[]{-85,432,-357,434,-99,542,-108,692,-98,705,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-356,708,-97,714});
    states[708] = new State(-776);
    states[709] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,710,-147,48,-148,51});
    states[710] = new State(new int[]{108,711});
    states[711] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-85,712,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[712] = new State(new int[]{10,713});
    states[713] = new State(-775);
    states[714] = new State(-964);
    states[715] = new State(-1007);
    states[716] = new State(-1008);
    states[717] = new State(-991);
    states[718] = new State(-992);
    states[719] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,720,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[720] = new State(new int[]{49,721});
    states[721] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486,96,-486,99,-486,31,-486,102,-486,2,-486,9,-486,98,-486,12,-486,97,-486,30,-486,83,-486,82,-486,81,-486,80,-486},new int[]{-256,722,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[722] = new State(new int[]{30,723,90,-525,10,-525,96,-525,99,-525,31,-525,102,-525,2,-525,9,-525,98,-525,12,-525,97,-525,84,-525,83,-525,82,-525,81,-525,80,-525,85,-525});
    states[723] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486,96,-486,99,-486,31,-486,102,-486,2,-486,9,-486,98,-486,12,-486,97,-486,30,-486,83,-486,82,-486,81,-486,80,-486},new int[]{-256,724,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[724] = new State(-526);
    states[725] = new State(-488);
    states[726] = new State(-489);
    states[727] = new State(new int[]{152,729,141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-139,728,-143,730,-147,48,-148,51});
    states[728] = new State(-521);
    states[729] = new State(-99);
    states[730] = new State(-100);
    states[731] = new State(-490);
    states[732] = new State(-491);
    states[733] = new State(-492);
    states[734] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,735,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[735] = new State(new int[]{56,736});
    states[736] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161,30,744,90,-545},new int[]{-36,737,-249,1078,-258,1080,-72,1071,-107,1077,-91,1076,-87,190,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[737] = new State(new int[]{10,740,30,744,90,-545},new int[]{-249,738});
    states[738] = new State(new int[]{90,739});
    states[739] = new State(-536);
    states[740] = new State(new int[]{30,744,141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161,90,-545},new int[]{-249,741,-258,743,-72,1071,-107,1077,-91,1076,-87,190,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[741] = new State(new int[]{90,742});
    states[742] = new State(-537);
    states[743] = new State(-540);
    states[744] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,748,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486,90,-486},new int[]{-248,745,-257,746,-256,22,-4,23,-109,24,-128,373,-108,385,-143,747,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879,-139,1034});
    states[745] = new State(new int[]{10,20,90,-546});
    states[746] = new State(-523);
    states[747] = new State(new int[]{8,-778,7,-778,140,-778,4,-778,15,-778,108,-778,109,-778,110,-778,111,-778,112,-778,90,-778,10,-778,11,-778,17,-778,96,-778,99,-778,31,-778,102,-778,2,-778,5,-100});
    states[748] = new State(new int[]{7,-189,11,-189,17,-189,5,-99});
    states[749] = new State(-493);
    states[750] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,748,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,96,-486,10,-486},new int[]{-248,751,-257,746,-256,22,-4,23,-109,24,-128,373,-108,385,-143,747,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879,-139,1034});
    states[751] = new State(new int[]{96,752,10,20});
    states[752] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-85,753,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[753] = new State(-547);
    states[754] = new State(-494);
    states[755] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,756,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[756] = new State(new int[]{97,1063,139,-550,141,-550,84,-550,85,-550,79,-550,77,-550,157,-550,43,-550,40,-550,8,-550,19,-550,20,-550,142,-550,144,-550,143,-550,152,-550,155,-550,154,-550,153,-550,75,-550,55,-550,89,-550,38,-550,23,-550,95,-550,52,-550,33,-550,53,-550,100,-550,45,-550,34,-550,51,-550,58,-550,73,-550,71,-550,36,-550,90,-550,10,-550,96,-550,99,-550,31,-550,102,-550,2,-550,9,-550,98,-550,12,-550,30,-550,83,-550,82,-550,81,-550,80,-550},new int[]{-289,757});
    states[757] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486,96,-486,99,-486,31,-486,102,-486,2,-486,9,-486,98,-486,12,-486,97,-486,30,-486,83,-486,82,-486,81,-486,80,-486},new int[]{-256,758,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[758] = new State(-548);
    states[759] = new State(-495);
    states[760] = new State(new int[]{51,1070,141,-561,84,-561,85,-561,79,-561,77,-561,157,-561},new int[]{-19,761});
    states[761] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,762,-147,48,-148,51});
    states[762] = new State(new int[]{108,1066,5,1067},new int[]{-283,763});
    states[763] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,764,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[764] = new State(new int[]{69,1064,70,1065},new int[]{-115,765});
    states[765] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,766,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[766] = new State(new int[]{157,1059,97,1063,139,-550,141,-550,84,-550,85,-550,79,-550,77,-550,43,-550,40,-550,8,-550,19,-550,20,-550,142,-550,144,-550,143,-550,152,-550,155,-550,154,-550,153,-550,75,-550,55,-550,89,-550,38,-550,23,-550,95,-550,52,-550,33,-550,53,-550,100,-550,45,-550,34,-550,51,-550,58,-550,73,-550,71,-550,36,-550,90,-550,10,-550,96,-550,99,-550,31,-550,102,-550,2,-550,9,-550,98,-550,12,-550,30,-550,83,-550,82,-550,81,-550,80,-550},new int[]{-289,767});
    states[767] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486,96,-486,99,-486,31,-486,102,-486,2,-486,9,-486,98,-486,12,-486,97,-486,30,-486,83,-486,82,-486,81,-486,80,-486},new int[]{-256,768,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[768] = new State(-558);
    states[769] = new State(-496);
    states[770] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,651,8,652,19,268,20,273,75,462,38,600,5,609,18,674,35,683,42,687},new int[]{-70,771,-86,444,-85,27,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-320,827,-97,659,-321,682});
    states[771] = new State(new int[]{97,772,98,379});
    states[772] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486,96,-486,99,-486,31,-486,102,-486,2,-486,9,-486,98,-486,12,-486,97,-486,30,-486,83,-486,82,-486,81,-486,80,-486},new int[]{-256,773,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[773] = new State(-566);
    states[774] = new State(-497);
    states[775] = new State(-498);
    states[776] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,748,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486,99,-486,31,-486},new int[]{-248,777,-257,746,-256,22,-4,23,-109,24,-128,373,-108,385,-143,747,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879,-139,1034});
    states[777] = new State(new int[]{10,20,99,779,31,1037},new int[]{-287,778});
    states[778] = new State(-568);
    states[779] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,748,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486},new int[]{-248,780,-257,746,-256,22,-4,23,-109,24,-128,373,-108,385,-143,747,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879,-139,1034});
    states[780] = new State(new int[]{90,781,10,20});
    states[781] = new State(-569);
    states[782] = new State(-499);
    states[783] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609,90,-583,10,-583,96,-583,99,-583,31,-583,102,-583,2,-583,9,-583,98,-583,12,-583,97,-583,30,-583,83,-583,82,-583,81,-583,80,-583},new int[]{-85,784,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[784] = new State(-584);
    states[785] = new State(-500);
    states[786] = new State(new int[]{51,1015,141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,787,-147,48,-148,51});
    states[787] = new State(new int[]{5,1013,135,-557},new int[]{-270,788});
    states[788] = new State(new int[]{135,789});
    states[789] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,790,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[790] = new State(new int[]{97,791});
    states[791] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486,96,-486,99,-486,31,-486,102,-486,2,-486,9,-486,98,-486,12,-486,97,-486,30,-486,83,-486,82,-486,81,-486,80,-486},new int[]{-256,792,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[792] = new State(-552);
    states[793] = new State(-501);
    states[794] = new State(new int[]{8,796,141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-309,795,-154,804,-143,803,-147,48,-148,51});
    states[795] = new State(-511);
    states[796] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,797,-147,48,-148,51});
    states[797] = new State(new int[]{98,798});
    states[798] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-154,799,-143,803,-147,48,-148,51});
    states[799] = new State(new int[]{9,800,98,554});
    states[800] = new State(new int[]{108,801});
    states[801] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-85,802,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[802] = new State(-513);
    states[803] = new State(-344);
    states[804] = new State(new int[]{5,805,98,554,108,1011});
    states[805] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-272,806,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[806] = new State(new int[]{108,1009,118,1010,90,-406,10,-406,96,-406,99,-406,31,-406,102,-406,2,-406,9,-406,98,-406,12,-406,97,-406,30,-406,84,-406,83,-406,82,-406,81,-406,80,-406,85,-406},new int[]{-336,807});
    states[807] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,981,133,834,114,368,113,369,61,161,35,683,42,687},new int[]{-84,808,-83,809,-82,261,-87,262,-88,223,-79,810,-13,236,-10,246,-14,209,-143,849,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848,-92,998,-239,999,-321,1008});
    states[808] = new State(-408);
    states[809] = new State(-409);
    states[810] = new State(new int[]{6,811,114,232,113,233,126,234,127,235,118,-118,123,-118,121,-118,119,-118,122,-118,120,-118,135,-118,13,-118,16,-118,90,-118,10,-118,96,-118,99,-118,31,-118,102,-118,2,-118,9,-118,98,-118,12,-118,97,-118,30,-118,84,-118,83,-118,82,-118,81,-118,80,-118,85,-118},new int[]{-189,199});
    states[811] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-13,812,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842});
    states[812] = new State(new int[]{134,237,136,238,116,239,115,240,129,241,130,242,131,243,132,244,128,245,90,-410,10,-410,96,-410,99,-410,31,-410,102,-410,2,-410,9,-410,98,-410,12,-410,97,-410,30,-410,84,-410,83,-410,82,-410,81,-410,80,-410,85,-410},new int[]{-197,201,-191,206});
    states[813] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-68,814,-75,352,-89,362,-85,355,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[814] = new State(new int[]{75,815});
    states[815] = new State(-165);
    states[816] = new State(-157);
    states[817] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,828,133,834,114,368,113,369,61,161},new int[]{-10,818,-14,819,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,836,-169,838,-57,839});
    states[818] = new State(-158);
    states[819] = new State(new int[]{4,211,11,213,7,820,140,822,8,823,134,-155,136,-155,116,-155,115,-155,129,-155,130,-155,131,-155,132,-155,128,-155,114,-155,113,-155,126,-155,127,-155,118,-155,123,-155,121,-155,119,-155,122,-155,120,-155,135,-155,13,-155,16,-155,6,-155,98,-155,9,-155,12,-155,5,-155,90,-155,10,-155,96,-155,99,-155,31,-155,102,-155,2,-155,97,-155,30,-155,84,-155,83,-155,82,-155,81,-155,80,-155,85,-155},new int[]{-12,210});
    states[820] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,83,57,82,58,81,59,80,60,67,61,62,62,126,63,20,64,19,65,61,66,21,67,127,68,128,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,22,78,72,79,89,80,23,81,24,82,27,83,28,84,29,85,70,86,97,87,30,88,90,89,31,90,32,91,25,92,102,93,99,94,33,95,34,96,35,97,38,98,39,99,40,100,101,101,41,102,42,103,44,104,45,105,46,106,95,107,47,108,100,109,48,110,26,111,49,112,69,113,96,114,50,115,51,116,52,117,53,118,54,119,55,120,56,121,57,122,59,123,103,124,104,125,107,126,105,127,106,128,60,129,73,130,36,131,37,132,68,133,145,134,58,135,137,136,138,137,78,138,150,139,149,140,71,141,151,142,147,143,148,144,146,145,43,147},new int[]{-134,821,-143,46,-147,48,-148,51,-290,55,-146,56,-291,146});
    states[821] = new State(-177);
    states[822] = new State(-178);
    states[823] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,651,8,652,19,268,20,273,75,462,38,600,5,609,18,674,35,683,42,687,9,-182},new int[]{-74,824,-70,826,-86,444,-85,27,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-320,827,-97,659,-321,682});
    states[824] = new State(new int[]{9,825});
    states[825] = new State(-179);
    states[826] = new State(new int[]{98,379,9,-181});
    states[827] = new State(-590);
    states[828] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-87,829,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[829] = new State(new int[]{9,830,13,191,16,195});
    states[830] = new State(-159);
    states[831] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-87,832,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[832] = new State(new int[]{9,833,13,191,16,195});
    states[833] = new State(new int[]{134,-159,136,-159,116,-159,115,-159,129,-159,130,-159,131,-159,132,-159,128,-159,114,-159,113,-159,126,-159,127,-159,118,-159,123,-159,121,-159,119,-159,122,-159,120,-159,135,-159,13,-159,16,-159,6,-159,98,-159,9,-159,12,-159,5,-159,90,-159,10,-159,96,-159,99,-159,31,-159,102,-159,2,-159,97,-159,30,-159,84,-159,83,-159,82,-159,81,-159,80,-159,85,-159,117,-154});
    states[834] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,828,133,834,114,368,113,369,61,161},new int[]{-10,835,-14,819,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,836,-169,838,-57,839});
    states[835] = new State(-160);
    states[836] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,828,133,834,114,368,113,369,61,161},new int[]{-10,837,-14,819,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,836,-169,838,-57,839});
    states[837] = new State(-161);
    states[838] = new State(-162);
    states[839] = new State(-163);
    states[840] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-10,837,-265,841,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-11,842});
    states[841] = new State(-140);
    states[842] = new State(new int[]{117,843});
    states[843] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-10,844,-265,845,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-11,842});
    states[844] = new State(-138);
    states[845] = new State(-139);
    states[846] = new State(-142);
    states[847] = new State(-143);
    states[848] = new State(-121);
    states[849] = new State(new int[]{125,850,4,-168,11,-168,7,-168,140,-168,8,-168,134,-168,136,-168,116,-168,115,-168,129,-168,130,-168,131,-168,132,-168,128,-168,6,-168,114,-168,113,-168,126,-168,127,-168,118,-168,123,-168,121,-168,119,-168,122,-168,120,-168,135,-168,13,-168,16,-168,90,-168,10,-168,96,-168,99,-168,31,-168,102,-168,2,-168,9,-168,98,-168,12,-168,97,-168,30,-168,84,-168,83,-168,82,-168,81,-168,80,-168,85,-168,117,-168});
    states[850] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,451,19,268,20,273,75,462,18,674,35,683,42,687,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-326,851,-101,448,-96,449,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,657,-113,592,-320,658,-97,659,-321,682,-328,852,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[851] = new State(-412);
    states[852] = new State(-1005);
    states[853] = new State(-993);
    states[854] = new State(-994);
    states[855] = new State(-995);
    states[856] = new State(-996);
    states[857] = new State(-997);
    states[858] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,859,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[859] = new State(new int[]{97,860});
    states[860] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486,96,-486,99,-486,31,-486,102,-486,2,-486,9,-486,98,-486,12,-486,97,-486,30,-486,83,-486,82,-486,81,-486,80,-486},new int[]{-256,861,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[861] = new State(-508);
    states[862] = new State(-502);
    states[863] = new State(-587);
    states[864] = new State(-588);
    states[865] = new State(-503);
    states[866] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,867,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[867] = new State(new int[]{97,868});
    states[868] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486,96,-486,99,-486,31,-486,102,-486,2,-486,9,-486,98,-486,12,-486,97,-486,30,-486,83,-486,82,-486,81,-486,80,-486},new int[]{-256,869,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[869] = new State(-551);
    states[870] = new State(-504);
    states[871] = new State(new int[]{72,873,54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,652,19,268,20,273,75,462,38,600,18,674,35,683,42,687},new int[]{-100,872,-98,875,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-320,876,-97,659,-321,682});
    states[872] = new State(-509);
    states[873] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,652,19,268,20,273,75,462,38,600,18,674,35,683,42,687},new int[]{-100,874,-98,875,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-320,876,-97,659,-321,682});
    states[874] = new State(-510);
    states[875] = new State(-601);
    states[876] = new State(-602);
    states[877] = new State(-505);
    states[878] = new State(-506);
    states[879] = new State(-507);
    states[880] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,881,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[881] = new State(new int[]{53,882});
    states[882] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,54,960,19,268,20,273,11,920,8,933},new int[]{-348,883,-347,974,-340,890,-280,895,-176,203,-143,205,-147,48,-148,51,-339,952,-355,955,-337,963,-15,958,-161,148,-163,149,-162,153,-16,155,-253,961,-292,962,-341,964,-342,967});
    states[883] = new State(new int[]{10,886,30,744,90,-545},new int[]{-249,884});
    states[884] = new State(new int[]{90,885});
    states[885] = new State(-527);
    states[886] = new State(new int[]{30,744,141,47,84,49,85,50,79,52,77,53,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,54,960,19,268,20,273,11,920,8,933,90,-545},new int[]{-249,887,-347,889,-340,890,-280,895,-176,203,-143,205,-147,48,-148,51,-339,952,-355,955,-337,963,-15,958,-161,148,-163,149,-162,153,-16,155,-253,961,-292,962,-341,964,-342,967});
    states[887] = new State(new int[]{90,888});
    states[888] = new State(-528);
    states[889] = new State(-530);
    states[890] = new State(new int[]{37,891});
    states[891] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,892,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[892] = new State(new int[]{5,893});
    states[893] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486,30,-486,90,-486},new int[]{-256,894,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[894] = new State(-531);
    states[895] = new State(new int[]{8,896,98,-642,5,-642});
    states[896] = new State(new int[]{14,901,142,151,144,152,143,154,152,156,155,157,154,158,153,159,114,368,113,369,141,47,84,49,85,50,79,52,77,53,157,54,51,908,11,920,8,933},new int[]{-352,897,-350,951,-15,902,-161,148,-163,149,-162,153,-16,155,-195,903,-143,905,-147,48,-148,51,-340,912,-280,913,-176,203,-341,919,-342,950});
    states[897] = new State(new int[]{9,898,10,899,98,917});
    states[898] = new State(new int[]{37,-636,5,-637});
    states[899] = new State(new int[]{14,901,142,151,144,152,143,154,152,156,155,157,154,158,153,159,114,368,113,369,141,47,84,49,85,50,79,52,77,53,157,54,51,908,11,920,8,933},new int[]{-350,900,-15,902,-161,148,-163,149,-162,153,-16,155,-195,903,-143,905,-147,48,-148,51,-340,912,-280,913,-176,203,-341,919,-342,950});
    states[900] = new State(-668);
    states[901] = new State(-680);
    states[902] = new State(-681);
    states[903] = new State(new int[]{142,151,144,152,143,154,152,156,155,157,154,158,153,159},new int[]{-15,904,-161,148,-163,149,-162,153,-16,155});
    states[904] = new State(-682);
    states[905] = new State(new int[]{5,906,9,-684,10,-684,98,-684,7,-259,4,-259,121,-259,8,-259});
    states[906] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-272,907,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[907] = new State(-683);
    states[908] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,909,-147,48,-148,51});
    states[909] = new State(new int[]{5,910,9,-686,10,-686,98,-686});
    states[910] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-272,911,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[911] = new State(-685);
    states[912] = new State(-687);
    states[913] = new State(new int[]{8,914});
    states[914] = new State(new int[]{14,901,142,151,144,152,143,154,152,156,155,157,154,158,153,159,114,368,113,369,141,47,84,49,85,50,79,52,77,53,157,54,51,908,11,920,8,933},new int[]{-352,915,-350,951,-15,902,-161,148,-163,149,-162,153,-16,155,-195,903,-143,905,-147,48,-148,51,-340,912,-280,913,-176,203,-341,919,-342,950});
    states[915] = new State(new int[]{9,916,10,899,98,917});
    states[916] = new State(-636);
    states[917] = new State(new int[]{14,901,142,151,144,152,143,154,152,156,155,157,154,158,153,159,114,368,113,369,141,47,84,49,85,50,79,52,77,53,157,54,51,908,11,920,8,933},new int[]{-350,918,-15,902,-161,148,-163,149,-162,153,-16,155,-195,903,-143,905,-147,48,-148,51,-340,912,-280,913,-176,203,-341,919,-342,950});
    states[918] = new State(-669);
    states[919] = new State(-688);
    states[920] = new State(new int[]{142,151,144,152,143,154,152,156,155,157,154,158,153,159,51,927,14,929,141,47,84,49,85,50,79,52,77,53,157,54,11,920,8,933,6,948},new int[]{-353,921,-343,949,-15,925,-161,148,-163,149,-162,153,-16,155,-345,926,-340,930,-280,913,-176,203,-143,205,-147,48,-148,51,-341,931,-342,932});
    states[921] = new State(new int[]{12,922,98,923});
    states[922] = new State(-646);
    states[923] = new State(new int[]{142,151,144,152,143,154,152,156,155,157,154,158,153,159,51,927,14,929,141,47,84,49,85,50,79,52,77,53,157,54,11,920,8,933,6,948},new int[]{-343,924,-15,925,-161,148,-163,149,-162,153,-16,155,-345,926,-340,930,-280,913,-176,203,-143,205,-147,48,-148,51,-341,931,-342,932});
    states[924] = new State(-648);
    states[925] = new State(-649);
    states[926] = new State(-650);
    states[927] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,928,-147,48,-148,51});
    states[928] = new State(-656);
    states[929] = new State(-651);
    states[930] = new State(-652);
    states[931] = new State(-653);
    states[932] = new State(-654);
    states[933] = new State(new int[]{14,938,142,151,144,152,143,154,152,156,155,157,154,158,153,159,114,368,113,369,51,942,141,47,84,49,85,50,79,52,77,53,157,54,11,920,8,933},new int[]{-354,934,-344,947,-15,939,-161,148,-163,149,-162,153,-16,155,-195,940,-340,944,-280,913,-176,203,-143,205,-147,48,-148,51,-341,945,-342,946});
    states[934] = new State(new int[]{9,935,98,936});
    states[935] = new State(-657);
    states[936] = new State(new int[]{14,938,142,151,144,152,143,154,152,156,155,157,154,158,153,159,114,368,113,369,51,942,141,47,84,49,85,50,79,52,77,53,157,54,11,920,8,933},new int[]{-344,937,-15,939,-161,148,-163,149,-162,153,-16,155,-195,940,-340,944,-280,913,-176,203,-143,205,-147,48,-148,51,-341,945,-342,946});
    states[937] = new State(-666);
    states[938] = new State(-658);
    states[939] = new State(-659);
    states[940] = new State(new int[]{142,151,144,152,143,154,152,156,155,157,154,158,153,159},new int[]{-15,941,-161,148,-163,149,-162,153,-16,155});
    states[941] = new State(-660);
    states[942] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,943,-147,48,-148,51});
    states[943] = new State(-661);
    states[944] = new State(-662);
    states[945] = new State(-663);
    states[946] = new State(-664);
    states[947] = new State(-665);
    states[948] = new State(-655);
    states[949] = new State(-647);
    states[950] = new State(-689);
    states[951] = new State(-667);
    states[952] = new State(new int[]{5,953});
    states[953] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486,30,-486,90,-486},new int[]{-256,954,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[954] = new State(-532);
    states[955] = new State(new int[]{98,956,5,-638});
    states[956] = new State(new int[]{142,151,144,152,143,154,152,156,155,157,154,158,153,159,141,47,84,49,85,50,79,52,77,53,157,54,54,960,19,268,20,273},new int[]{-337,957,-15,958,-161,148,-163,149,-162,153,-16,155,-280,959,-176,203,-143,205,-147,48,-148,51,-253,961,-292,962});
    states[957] = new State(-640);
    states[958] = new State(-641);
    states[959] = new State(-642);
    states[960] = new State(-643);
    states[961] = new State(-644);
    states[962] = new State(-645);
    states[963] = new State(-639);
    states[964] = new State(new int[]{5,965});
    states[965] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486,30,-486,90,-486},new int[]{-256,966,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[966] = new State(-533);
    states[967] = new State(new int[]{37,968,5,972});
    states[968] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,969,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[969] = new State(new int[]{5,970});
    states[970] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486,30,-486,90,-486},new int[]{-256,971,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[971] = new State(-534);
    states[972] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486,30,-486,90,-486},new int[]{-256,973,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[973] = new State(-535);
    states[974] = new State(-529);
    states[975] = new State(-998);
    states[976] = new State(-999);
    states[977] = new State(-1000);
    states[978] = new State(-1001);
    states[979] = new State(-1002);
    states[980] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,652,19,268,20,273,75,462,38,600,18,674,35,683,42,687},new int[]{-100,872,-98,875,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-320,876,-97,659,-321,682});
    states[981] = new State(new int[]{9,989,141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,994,133,834,114,368,113,369,61,161},new int[]{-87,982,-66,983,-241,987,-88,223,-79,231,-13,236,-10,246,-14,209,-143,993,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848,-65,258,-83,997,-82,261,-92,998,-239,999,-240,1000,-242,1007,-132,1003});
    states[982] = new State(new int[]{9,833,13,191,16,195,98,-193});
    states[983] = new State(new int[]{9,984});
    states[984] = new State(new int[]{125,985,90,-196,10,-196,96,-196,99,-196,31,-196,102,-196,2,-196,9,-196,98,-196,12,-196,97,-196,30,-196,84,-196,83,-196,82,-196,81,-196,80,-196,85,-196});
    states[985] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,451,19,268,20,273,75,462,18,674,35,683,42,687,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-326,986,-101,448,-96,449,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,657,-113,592,-320,658,-97,659,-321,682,-328,852,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[986] = new State(-414);
    states[987] = new State(new int[]{9,988});
    states[988] = new State(-201);
    states[989] = new State(new int[]{5,556,125,-987},new int[]{-322,990});
    states[990] = new State(new int[]{125,991});
    states[991] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,451,19,268,20,273,75,462,18,674,35,683,42,687,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-326,992,-101,448,-96,449,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,657,-113,592,-320,658,-97,659,-321,682,-328,852,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[992] = new State(-413);
    states[993] = new State(new int[]{4,-168,11,-168,7,-168,140,-168,8,-168,134,-168,136,-168,116,-168,115,-168,129,-168,130,-168,131,-168,132,-168,128,-168,114,-168,113,-168,126,-168,127,-168,118,-168,123,-168,121,-168,119,-168,122,-168,120,-168,135,-168,9,-168,13,-168,16,-168,98,-168,117,-168,5,-207});
    states[994] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,994,133,834,114,368,113,369,61,161,9,-197},new int[]{-87,982,-66,995,-241,987,-88,223,-79,231,-13,236,-10,246,-14,209,-143,993,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848,-65,258,-83,997,-82,261,-92,998,-239,999,-240,1000,-242,1007,-132,1003});
    states[995] = new State(new int[]{9,996});
    states[996] = new State(-196);
    states[997] = new State(-199);
    states[998] = new State(-194);
    states[999] = new State(-195);
    states[1000] = new State(new int[]{10,1001,9,-202});
    states[1001] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,9,-203},new int[]{-242,1002,-132,1003,-143,1006,-147,48,-148,51});
    states[1002] = new State(-205);
    states[1003] = new State(new int[]{5,1004});
    states[1004] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,994,133,834,114,368,113,369,61,161},new int[]{-82,1005,-87,262,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848,-92,998,-239,999});
    states[1005] = new State(-206);
    states[1006] = new State(-207);
    states[1007] = new State(-204);
    states[1008] = new State(-411);
    states[1009] = new State(-404);
    states[1010] = new State(-405);
    states[1011] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,651,8,652,19,268,20,273,75,462,38,600,5,609,18,674,35,683,42,687},new int[]{-86,1012,-85,27,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-320,827,-97,659,-321,682});
    states[1012] = new State(-407);
    states[1013] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-272,1014,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[1014] = new State(-556);
    states[1015] = new State(new int[]{8,1027,141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,1016,-147,48,-148,51});
    states[1016] = new State(new int[]{5,1017,135,1023});
    states[1017] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-272,1018,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[1018] = new State(new int[]{135,1019});
    states[1019] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,1020,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[1020] = new State(new int[]{97,1021});
    states[1021] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486,96,-486,99,-486,31,-486,102,-486,2,-486,9,-486,98,-486,12,-486,97,-486,30,-486,83,-486,82,-486,81,-486,80,-486},new int[]{-256,1022,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[1022] = new State(-553);
    states[1023] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,1024,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[1024] = new State(new int[]{97,1025});
    states[1025] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486,96,-486,99,-486,31,-486,102,-486,2,-486,9,-486,98,-486,12,-486,97,-486,30,-486,83,-486,82,-486,81,-486,80,-486},new int[]{-256,1026,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[1026] = new State(-554);
    states[1027] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-154,1028,-143,803,-147,48,-148,51});
    states[1028] = new State(new int[]{9,1029,98,554});
    states[1029] = new State(new int[]{135,1030});
    states[1030] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,1031,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[1031] = new State(new int[]{97,1032});
    states[1032] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486,96,-486,99,-486,31,-486,102,-486,2,-486,9,-486,98,-486,12,-486,97,-486,30,-486,83,-486,82,-486,81,-486,80,-486},new int[]{-256,1033,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[1033] = new State(-555);
    states[1034] = new State(new int[]{5,1035});
    states[1035] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,748,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486,96,-486,99,-486,31,-486,102,-486,2,-486},new int[]{-257,1036,-256,22,-4,23,-109,24,-128,373,-108,385,-143,747,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879,-139,1034});
    states[1036] = new State(-485);
    states[1037] = new State(new int[]{78,1045,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,748,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486,90,-486},new int[]{-60,1038,-63,1040,-62,1057,-248,1058,-257,746,-256,22,-4,23,-109,24,-128,373,-108,385,-143,747,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879,-139,1034});
    states[1038] = new State(new int[]{90,1039});
    states[1039] = new State(-570);
    states[1040] = new State(new int[]{10,1042,30,1055,90,-576},new int[]{-250,1041});
    states[1041] = new State(-571);
    states[1042] = new State(new int[]{78,1045,30,1055,90,-576},new int[]{-62,1043,-250,1044});
    states[1043] = new State(-575);
    states[1044] = new State(-572);
    states[1045] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-64,1046,-175,1049,-176,1050,-143,1051,-147,48,-148,51,-136,1052});
    states[1046] = new State(new int[]{97,1047});
    states[1047] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486,30,-486,90,-486},new int[]{-256,1048,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[1048] = new State(-578);
    states[1049] = new State(-579);
    states[1050] = new State(new int[]{7,167,97,-581});
    states[1051] = new State(new int[]{7,-259,97,-259,5,-582});
    states[1052] = new State(new int[]{5,1053});
    states[1053] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-175,1054,-176,1050,-143,205,-147,48,-148,51});
    states[1054] = new State(-580);
    states[1055] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,748,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486,90,-486},new int[]{-248,1056,-257,746,-256,22,-4,23,-109,24,-128,373,-108,385,-143,747,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879,-139,1034});
    states[1056] = new State(new int[]{10,20,90,-577});
    states[1057] = new State(-574);
    states[1058] = new State(new int[]{10,20,90,-573});
    states[1059] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,1060,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[1060] = new State(new int[]{97,1063,139,-550,141,-550,84,-550,85,-550,79,-550,77,-550,157,-550,43,-550,40,-550,8,-550,19,-550,20,-550,142,-550,144,-550,143,-550,152,-550,155,-550,154,-550,153,-550,75,-550,55,-550,89,-550,38,-550,23,-550,95,-550,52,-550,33,-550,53,-550,100,-550,45,-550,34,-550,51,-550,58,-550,73,-550,71,-550,36,-550,90,-550,10,-550,96,-550,99,-550,31,-550,102,-550,2,-550,9,-550,98,-550,12,-550,30,-550,83,-550,82,-550,81,-550,80,-550},new int[]{-289,1061});
    states[1061] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486,96,-486,99,-486,31,-486,102,-486,2,-486,9,-486,98,-486,12,-486,97,-486,30,-486,83,-486,82,-486,81,-486,80,-486},new int[]{-256,1062,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[1062] = new State(-559);
    states[1063] = new State(-549);
    states[1064] = new State(-564);
    states[1065] = new State(-565);
    states[1066] = new State(-562);
    states[1067] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-176,1068,-143,205,-147,48,-148,51});
    states[1068] = new State(new int[]{108,1069,7,167});
    states[1069] = new State(-563);
    states[1070] = new State(-560);
    states[1071] = new State(new int[]{5,1072,98,1074});
    states[1072] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486,30,-486,90,-486},new int[]{-256,1073,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[1073] = new State(-541);
    states[1074] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-107,1075,-91,1076,-87,190,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[1075] = new State(-543);
    states[1076] = new State(-544);
    states[1077] = new State(-542);
    states[1078] = new State(new int[]{90,1079});
    states[1079] = new State(-538);
    states[1080] = new State(-539);
    states[1081] = new State(new int[]{9,1082,141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-324,1085,-325,1089,-154,552,-143,803,-147,48,-148,51});
    states[1082] = new State(new int[]{125,1083});
    states[1083] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,53,157,54,43,391,40,429,8,691,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-327,1084,-208,690,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-4,715,-328,716,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[1084] = new State(-982);
    states[1085] = new State(new int[]{9,1086,10,550});
    states[1086] = new State(new int[]{125,1087});
    states[1087] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,53,157,54,43,391,40,429,8,691,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-327,1088,-208,690,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-4,715,-328,716,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[1088] = new State(-983);
    states[1089] = new State(-984);
    states[1090] = new State(new int[]{9,1091,141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-324,1095,-325,1089,-154,552,-143,803,-147,48,-148,51});
    states[1091] = new State(new int[]{5,661,125,-989},new int[]{-323,1092});
    states[1092] = new State(new int[]{125,1093});
    states[1093] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,451,19,268,20,273,75,462,18,674,35,683,42,687,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-326,1094,-101,448,-96,449,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,657,-113,592,-320,658,-97,659,-321,682,-328,852,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[1094] = new State(-979);
    states[1095] = new State(new int[]{9,1096,10,550});
    states[1096] = new State(new int[]{5,661,125,-989},new int[]{-323,1097});
    states[1097] = new State(new int[]{125,1098});
    states[1098] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,451,19,268,20,273,75,462,18,674,35,683,42,687,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-326,1099,-101,448,-96,449,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,657,-113,592,-320,658,-97,659,-321,682,-328,852,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[1099] = new State(-980);
    states[1100] = new State(new int[]{5,1101,10,1113,8,-778,7,-778,140,-778,4,-778,15,-778,108,-778,109,-778,110,-778,111,-778,112,-778,136,-778,134,-778,116,-778,115,-778,129,-778,130,-778,131,-778,132,-778,128,-778,114,-778,113,-778,126,-778,127,-778,124,-778,6,-778,118,-778,123,-778,121,-778,119,-778,122,-778,120,-778,135,-778,133,-778,16,-778,9,-778,98,-778,13,-778,117,-778,11,-778,17,-778});
    states[1101] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-271,1102,-272,506,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[1102] = new State(new int[]{9,1103,10,1107});
    states[1103] = new State(new int[]{5,661,125,-989},new int[]{-323,1104});
    states[1104] = new State(new int[]{125,1105});
    states[1105] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,451,19,268,20,273,75,462,18,674,35,683,42,687,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-326,1106,-101,448,-96,449,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,657,-113,592,-320,658,-97,659,-321,682,-328,852,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[1106] = new State(-969);
    states[1107] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-324,1108,-325,1089,-154,552,-143,803,-147,48,-148,51});
    states[1108] = new State(new int[]{9,1109,10,550});
    states[1109] = new State(new int[]{5,661,125,-989},new int[]{-323,1110});
    states[1110] = new State(new int[]{125,1111});
    states[1111] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,451,19,268,20,273,75,462,18,674,35,683,42,687,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-326,1112,-101,448,-96,449,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,657,-113,592,-320,658,-97,659,-321,682,-328,852,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[1112] = new State(-971);
    states[1113] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-324,1114,-325,1089,-154,552,-143,803,-147,48,-148,51});
    states[1114] = new State(new int[]{9,1115,10,550});
    states[1115] = new State(new int[]{5,661,125,-989},new int[]{-323,1116});
    states[1116] = new State(new int[]{125,1117});
    states[1117] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,451,19,268,20,273,75,462,18,674,35,683,42,687,89,17,38,719,52,755,95,750,33,760,34,786,71,858,23,734,100,776,58,866,45,783,73,980},new int[]{-326,1118,-101,448,-96,449,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,657,-113,592,-320,658,-97,659,-321,682,-328,852,-251,717,-149,718,-316,853,-243,854,-120,855,-119,856,-121,857,-35,975,-299,976,-165,977,-244,978,-122,979});
    states[1118] = new State(-970);
    states[1119] = new State(new int[]{5,1120,7,-259,8,-259,121,-259,12,-259,98,-259});
    states[1120] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-9,1121,-176,645,-143,205,-147,48,-148,51,-298,1122});
    states[1121] = new State(-216);
    states[1122] = new State(new int[]{8,648,12,-629,98,-629},new int[]{-69,1123});
    states[1123] = new State(-768);
    states[1124] = new State(-213);
    states[1125] = new State(-209);
    states[1126] = new State(-465);
    states[1127] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,18,674},new int[]{-99,1128,-98,1129,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-97,714});
    states[1128] = new State(-966);
    states[1129] = new State(-963);
    states[1130] = new State(-965);
    states[1131] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,1132,-147,48,-148,51});
    states[1132] = new State(new int[]{98,1133,108,711});
    states[1133] = new State(new int[]{51,1141},new int[]{-335,1134});
    states[1134] = new State(new int[]{9,1135,98,1138});
    states[1135] = new State(new int[]{108,1136});
    states[1136] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-85,1137,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[1137] = new State(-512);
    states[1138] = new State(new int[]{51,1139});
    states[1139] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,1140,-147,48,-148,51});
    states[1140] = new State(-519);
    states[1141] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,1142,-147,48,-148,51});
    states[1142] = new State(-518);
    states[1143] = new State(new int[]{145,1147,147,1148,148,1149,149,1150,151,1151,150,1152,105,-809,89,-809,57,-809,27,-809,65,-809,48,-809,51,-809,60,-809,11,-809,26,-809,24,-809,42,-809,35,-809,28,-809,29,-809,44,-809,25,-809,90,-809,83,-809,82,-809,81,-809,80,-809,21,-809,146,-809,39,-809},new int[]{-202,1144,-205,1153});
    states[1144] = new State(new int[]{10,1145});
    states[1145] = new State(new int[]{145,1147,147,1148,148,1149,149,1150,151,1151,150,1152,105,-810,89,-810,57,-810,27,-810,65,-810,48,-810,51,-810,60,-810,11,-810,26,-810,24,-810,42,-810,35,-810,28,-810,29,-810,44,-810,25,-810,90,-810,83,-810,82,-810,81,-810,80,-810,21,-810,146,-810,39,-810},new int[]{-205,1146});
    states[1146] = new State(-814);
    states[1147] = new State(-825);
    states[1148] = new State(-826);
    states[1149] = new State(-827);
    states[1150] = new State(-828);
    states[1151] = new State(-829);
    states[1152] = new State(-830);
    states[1153] = new State(-813);
    states[1154] = new State(-375);
    states[1155] = new State(-439);
    states[1156] = new State(-440);
    states[1157] = new State(new int[]{8,-445,108,-445,10,-445,11,-445,5,-445,7,-442});
    states[1158] = new State(new int[]{121,1160,8,-448,108,-448,10,-448,7,-448,11,-448,5,-448},new int[]{-151,1159});
    states[1159] = new State(-449);
    states[1160] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-154,1161,-143,803,-147,48,-148,51});
    states[1161] = new State(new int[]{119,1162,98,554});
    states[1162] = new State(-322);
    states[1163] = new State(-450);
    states[1164] = new State(new int[]{121,1160,8,-446,108,-446,10,-446,11,-446,5,-446},new int[]{-151,1165});
    states[1165] = new State(-447);
    states[1166] = new State(new int[]{7,1167});
    states[1167] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391},new int[]{-138,1168,-145,1169,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164});
    states[1168] = new State(-441);
    states[1169] = new State(-444);
    states[1170] = new State(-443);
    states[1171] = new State(-432);
    states[1172] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391},new int[]{-166,1173,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[1173] = new State(new int[]{11,1201,5,-389},new int[]{-229,1174,-234,1198});
    states[1174] = new State(new int[]{84,1187,85,1193,10,-396},new int[]{-198,1175});
    states[1175] = new State(new int[]{10,1176});
    states[1176] = new State(new int[]{61,1181,150,1183,149,1184,145,1185,148,1186,11,-386,26,-386,24,-386,42,-386,35,-386,28,-386,29,-386,44,-386,25,-386,90,-386,83,-386,82,-386,81,-386,80,-386},new int[]{-201,1177,-206,1178});
    states[1177] = new State(-380);
    states[1178] = new State(new int[]{10,1179});
    states[1179] = new State(new int[]{61,1181,11,-386,26,-386,24,-386,42,-386,35,-386,28,-386,29,-386,44,-386,25,-386,90,-386,83,-386,82,-386,81,-386,80,-386},new int[]{-201,1180});
    states[1180] = new State(-381);
    states[1181] = new State(new int[]{10,1182});
    states[1182] = new State(-387);
    states[1183] = new State(-831);
    states[1184] = new State(-832);
    states[1185] = new State(-833);
    states[1186] = new State(-834);
    states[1187] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,651,8,652,19,268,20,273,75,462,38,600,5,609,18,674,35,683,42,687,10,-395},new int[]{-110,1188,-86,1192,-85,27,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-320,827,-97,659,-321,682});
    states[1188] = new State(new int[]{85,1190,10,-399},new int[]{-199,1189});
    states[1189] = new State(-397);
    states[1190] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486},new int[]{-256,1191,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[1191] = new State(-400);
    states[1192] = new State(-394);
    states[1193] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486},new int[]{-256,1194,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[1194] = new State(new int[]{84,1196,10,-401},new int[]{-200,1195});
    states[1195] = new State(-398);
    states[1196] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,651,8,652,19,268,20,273,75,462,38,600,5,609,18,674,35,683,42,687,10,-395},new int[]{-110,1197,-86,1192,-85,27,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-320,827,-97,659,-321,682});
    states[1197] = new State(-402);
    states[1198] = new State(new int[]{5,1199});
    states[1199] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-271,1200,-272,506,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[1200] = new State(-388);
    states[1201] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-233,1202,-232,1209,-154,1206,-143,803,-147,48,-148,51});
    states[1202] = new State(new int[]{12,1203,10,1204});
    states[1203] = new State(-390);
    states[1204] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-232,1205,-154,1206,-143,803,-147,48,-148,51});
    states[1205] = new State(-392);
    states[1206] = new State(new int[]{5,1207,98,554});
    states[1207] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-271,1208,-272,506,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[1208] = new State(-393);
    states[1209] = new State(-391);
    states[1210] = new State(new int[]{44,1211});
    states[1211] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391},new int[]{-166,1212,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[1212] = new State(new int[]{11,1201,5,-389},new int[]{-229,1213,-234,1198});
    states[1213] = new State(new int[]{108,1216,10,-385},new int[]{-207,1214});
    states[1214] = new State(new int[]{10,1215});
    states[1215] = new State(-383);
    states[1216] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-85,1217,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[1217] = new State(-384);
    states[1218] = new State(new int[]{105,1347,11,-369,26,-369,24,-369,42,-369,35,-369,28,-369,29,-369,44,-369,25,-369,90,-369,83,-369,82,-369,81,-369,80,-369,57,-70,27,-70,65,-70,48,-70,51,-70,60,-70,89,-70},new int[]{-172,1219,-43,1220,-39,1223,-61,1346});
    states[1219] = new State(-433);
    states[1220] = new State(new int[]{89,17},new int[]{-251,1221});
    states[1221] = new State(new int[]{10,1222});
    states[1222] = new State(-460);
    states[1223] = new State(new int[]{57,1226,27,1247,65,1251,48,1410,51,1425,60,1427,89,-69},new int[]{-46,1224,-164,1225,-29,1232,-52,1249,-286,1253,-307,1412});
    states[1224] = new State(-71);
    states[1225] = new State(-87);
    states[1226] = new State(new int[]{152,729,141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-152,1227,-139,1231,-143,730,-147,48,-148,51});
    states[1227] = new State(new int[]{10,1228,98,1229});
    states[1228] = new State(-96);
    states[1229] = new State(new int[]{152,729,141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-139,1230,-143,730,-147,48,-148,51});
    states[1230] = new State(-98);
    states[1231] = new State(-97);
    states[1232] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,57,-88,27,-88,65,-88,48,-88,51,-88,60,-88,89,-88},new int[]{-27,1233,-28,1234,-137,1236,-143,1246,-147,48,-148,51});
    states[1233] = new State(-102);
    states[1234] = new State(new int[]{10,1235});
    states[1235] = new State(-112);
    states[1236] = new State(new int[]{118,1237,5,1242});
    states[1237] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,1240,133,834,114,368,113,369,61,161},new int[]{-106,1238,-87,1239,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848,-92,1241});
    states[1238] = new State(-113);
    states[1239] = new State(new int[]{13,191,16,195,10,-115,90,-115,83,-115,82,-115,81,-115,80,-115});
    states[1240] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,994,133,834,114,368,113,369,61,161,9,-197},new int[]{-87,982,-66,995,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848,-65,258,-83,997,-82,261,-92,998,-239,999});
    states[1241] = new State(-116);
    states[1242] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-272,1243,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[1243] = new State(new int[]{118,1244});
    states[1244] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,994,133,834,114,368,113,369,61,161},new int[]{-82,1245,-87,262,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848,-92,998,-239,999});
    states[1245] = new State(-114);
    states[1246] = new State(-117);
    states[1247] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-27,1248,-28,1234,-137,1236,-143,1246,-147,48,-148,51});
    states[1248] = new State(-101);
    states[1249] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,57,-89,27,-89,65,-89,48,-89,51,-89,60,-89,89,-89},new int[]{-27,1250,-28,1234,-137,1236,-143,1246,-147,48,-148,51});
    states[1250] = new State(-104);
    states[1251] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-27,1252,-28,1234,-137,1236,-143,1246,-147,48,-148,51});
    states[1252] = new State(-103);
    states[1253] = new State(new int[]{11,639,57,-90,27,-90,65,-90,48,-90,51,-90,60,-90,89,-90,141,-211,84,-211,85,-211,79,-211,77,-211,157,-211},new int[]{-49,1254,-6,1255,-246,1125});
    states[1254] = new State(-106);
    states[1255] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,11,639},new int[]{-50,1256,-246,528,-140,1257,-143,1402,-147,48,-148,51,-141,1407});
    states[1256] = new State(-208);
    states[1257] = new State(new int[]{118,1258});
    states[1258] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615,67,1396,68,1397,145,1398,25,1399,26,1400,24,-304,41,-304,62,-304},new int[]{-284,1259,-272,1261,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574,-30,1262,-21,1263,-22,1394,-20,1401});
    states[1259] = new State(new int[]{10,1260});
    states[1260] = new State(-217);
    states[1261] = new State(-222);
    states[1262] = new State(-223);
    states[1263] = new State(new int[]{24,1388,41,1389,62,1390},new int[]{-288,1264});
    states[1264] = new State(new int[]{8,1305,21,-316,11,-316,90,-316,83,-316,82,-316,81,-316,80,-316,27,-316,141,-316,84,-316,85,-316,79,-316,77,-316,157,-316,60,-316,26,-316,24,-316,42,-316,35,-316,28,-316,29,-316,44,-316,25,-316,10,-316},new int[]{-179,1265});
    states[1265] = new State(new int[]{21,1296,11,-323,90,-323,83,-323,82,-323,81,-323,80,-323,27,-323,141,-323,84,-323,85,-323,79,-323,77,-323,157,-323,60,-323,26,-323,24,-323,42,-323,35,-323,28,-323,29,-323,44,-323,25,-323,10,-323},new int[]{-315,1266,-314,1294,-313,1316});
    states[1266] = new State(new int[]{11,639,10,-314,90,-340,83,-340,82,-340,81,-340,80,-340,27,-211,141,-211,84,-211,85,-211,79,-211,77,-211,157,-211,60,-211,26,-211,24,-211,42,-211,35,-211,28,-211,29,-211,44,-211,25,-211},new int[]{-26,1267,-25,1268,-32,1274,-34,519,-45,1275,-6,1276,-246,1125,-33,1385,-54,1387,-53,525,-55,1386});
    states[1267] = new State(-297);
    states[1268] = new State(new int[]{90,1269,83,1270,82,1271,81,1272,80,1273},new int[]{-7,517});
    states[1269] = new State(-315);
    states[1270] = new State(-336);
    states[1271] = new State(-337);
    states[1272] = new State(-338);
    states[1273] = new State(-339);
    states[1274] = new State(-334);
    states[1275] = new State(-348);
    states[1276] = new State(new int[]{27,1278,141,47,84,49,85,50,79,52,77,53,157,54,60,1282,26,1341,24,1342,11,639,42,1289,35,1324,28,1356,29,1363,44,1370,25,1379},new int[]{-51,1277,-246,528,-218,527,-215,529,-254,530,-310,1280,-309,1281,-154,804,-143,803,-147,48,-148,51,-3,1286,-226,1343,-224,1218,-221,1288,-225,1323,-223,1344,-211,1367,-212,1368,-214,1369});
    states[1277] = new State(-350);
    states[1278] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-28,1279,-137,1236,-143,1246,-147,48,-148,51});
    states[1279] = new State(-355);
    states[1280] = new State(-356);
    states[1281] = new State(-360);
    states[1282] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-154,1283,-143,803,-147,48,-148,51});
    states[1283] = new State(new int[]{5,1284,98,554});
    states[1284] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-272,1285,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[1285] = new State(-361);
    states[1286] = new State(new int[]{28,533,44,1172,25,1210,141,47,84,49,85,50,79,52,77,53,157,54,60,1282,42,1289,35,1324},new int[]{-310,1287,-226,532,-212,1171,-309,1281,-154,804,-143,803,-147,48,-148,51,-224,1218,-221,1288,-225,1323});
    states[1287] = new State(-357);
    states[1288] = new State(-370);
    states[1289] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391},new int[]{-167,1290,-166,1155,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[1290] = new State(new int[]{8,577,10,-462,108,-462},new int[]{-124,1291});
    states[1291] = new State(new int[]{10,1321,108,-811},new int[]{-203,1292,-204,1317});
    states[1292] = new State(new int[]{21,1296,105,-323,89,-323,57,-323,27,-323,65,-323,48,-323,51,-323,60,-323,11,-323,26,-323,24,-323,42,-323,35,-323,28,-323,29,-323,44,-323,25,-323,90,-323,83,-323,82,-323,81,-323,80,-323,146,-323,39,-323},new int[]{-315,1293,-314,1294,-313,1316});
    states[1293] = new State(-451);
    states[1294] = new State(new int[]{21,1296,11,-324,90,-324,83,-324,82,-324,81,-324,80,-324,27,-324,141,-324,84,-324,85,-324,79,-324,77,-324,157,-324,60,-324,26,-324,24,-324,42,-324,35,-324,28,-324,29,-324,44,-324,25,-324,10,-324,105,-324,89,-324,57,-324,65,-324,48,-324,51,-324,146,-324,39,-324},new int[]{-313,1295});
    states[1295] = new State(-326);
    states[1296] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-154,1297,-143,803,-147,48,-148,51});
    states[1297] = new State(new int[]{5,1298,98,554});
    states[1298] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,1304,47,559,32,563,72,567,63,570,42,575,35,615,24,1313,28,1314},new int[]{-285,1299,-282,1315,-272,1303,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[1299] = new State(new int[]{10,1300,98,1301});
    states[1300] = new State(-327);
    states[1301] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,1304,47,559,32,563,72,567,63,570,42,575,35,615,24,1313,28,1314},new int[]{-282,1302,-272,1303,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[1302] = new State(-329);
    states[1303] = new State(-330);
    states[1304] = new State(new int[]{8,1305,10,-332,98,-332,21,-316,11,-316,90,-316,83,-316,82,-316,81,-316,80,-316,27,-316,141,-316,84,-316,85,-316,79,-316,77,-316,157,-316,60,-316,26,-316,24,-316,42,-316,35,-316,28,-316,29,-316,44,-316,25,-316},new int[]{-179,513});
    states[1305] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-178,1306,-177,1312,-176,1310,-143,205,-147,48,-148,51,-298,1311});
    states[1306] = new State(new int[]{9,1307,98,1308});
    states[1307] = new State(-317);
    states[1308] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-177,1309,-176,1310,-143,205,-147,48,-148,51,-298,1311});
    states[1309] = new State(-319);
    states[1310] = new State(new int[]{7,167,121,172,9,-320,98,-320},new int[]{-296,647});
    states[1311] = new State(-321);
    states[1312] = new State(-318);
    states[1313] = new State(-331);
    states[1314] = new State(-333);
    states[1315] = new State(-328);
    states[1316] = new State(-325);
    states[1317] = new State(new int[]{108,1318});
    states[1318] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486},new int[]{-256,1319,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[1319] = new State(new int[]{10,1320});
    states[1320] = new State(-436);
    states[1321] = new State(new int[]{145,1147,147,1148,148,1149,149,1150,151,1151,150,1152,21,-809,105,-809,89,-809,57,-809,27,-809,65,-809,48,-809,51,-809,60,-809,11,-809,26,-809,24,-809,42,-809,35,-809,28,-809,29,-809,44,-809,25,-809,90,-809,83,-809,82,-809,81,-809,80,-809,146,-809},new int[]{-202,1322,-205,1153});
    states[1322] = new State(new int[]{10,1145,108,-812});
    states[1323] = new State(-371);
    states[1324] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391},new int[]{-166,1325,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[1325] = new State(new int[]{8,577,5,-462,10,-462,108,-462},new int[]{-124,1326});
    states[1326] = new State(new int[]{5,1329,10,1321,108,-811},new int[]{-203,1327,-204,1337});
    states[1327] = new State(new int[]{21,1296,105,-323,89,-323,57,-323,27,-323,65,-323,48,-323,51,-323,60,-323,11,-323,26,-323,24,-323,42,-323,35,-323,28,-323,29,-323,44,-323,25,-323,90,-323,83,-323,82,-323,81,-323,80,-323,146,-323,39,-323},new int[]{-315,1328,-314,1294,-313,1316});
    states[1328] = new State(-452);
    states[1329] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-271,1330,-272,506,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[1330] = new State(new int[]{10,1321,108,-811},new int[]{-203,1331,-204,1333});
    states[1331] = new State(new int[]{21,1296,105,-323,89,-323,57,-323,27,-323,65,-323,48,-323,51,-323,60,-323,11,-323,26,-323,24,-323,42,-323,35,-323,28,-323,29,-323,44,-323,25,-323,90,-323,83,-323,82,-323,81,-323,80,-323,146,-323,39,-323},new int[]{-315,1332,-314,1294,-313,1316});
    states[1332] = new State(-453);
    states[1333] = new State(new int[]{108,1334});
    states[1334] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,652,19,268,20,273,75,462,38,600,18,674,35,683,42,687},new int[]{-100,1335,-98,875,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-320,876,-97,659,-321,682});
    states[1335] = new State(new int[]{10,1336});
    states[1336] = new State(-434);
    states[1337] = new State(new int[]{108,1338});
    states[1338] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,652,19,268,20,273,75,462,38,600,18,674,35,683,42,687},new int[]{-100,1339,-98,875,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-320,876,-97,659,-321,682});
    states[1339] = new State(new int[]{10,1340});
    states[1340] = new State(-435);
    states[1341] = new State(-358);
    states[1342] = new State(-359);
    states[1343] = new State(-367);
    states[1344] = new State(new int[]{105,1347,11,-368,26,-368,24,-368,42,-368,35,-368,28,-368,29,-368,44,-368,25,-368,90,-368,83,-368,82,-368,81,-368,80,-368,57,-70,27,-70,65,-70,48,-70,51,-70,60,-70,89,-70},new int[]{-172,1345,-43,1220,-39,1223,-61,1346});
    states[1345] = new State(-419);
    states[1346] = new State(-461);
    states[1347] = new State(new int[]{10,1355,141,47,84,49,85,50,79,52,77,53,157,54,142,151,144,152,143,154},new int[]{-105,1348,-143,1352,-147,48,-148,51,-161,1353,-163,149,-162,153});
    states[1348] = new State(new int[]{79,1349,10,1354});
    states[1349] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,142,151,144,152,143,154},new int[]{-105,1350,-143,1352,-147,48,-148,51,-161,1353,-163,149,-162,153});
    states[1350] = new State(new int[]{10,1351});
    states[1351] = new State(-454);
    states[1352] = new State(-457);
    states[1353] = new State(-458);
    states[1354] = new State(-455);
    states[1355] = new State(-456);
    states[1356] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391,8,-376,108,-376,10,-376},new int[]{-168,1357,-167,1154,-166,1155,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[1357] = new State(new int[]{8,577,108,-462,10,-462},new int[]{-124,1358});
    states[1358] = new State(new int[]{108,1360,10,1143},new int[]{-203,1359});
    states[1359] = new State(-372);
    states[1360] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486},new int[]{-256,1361,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[1361] = new State(new int[]{10,1362});
    states[1362] = new State(-420);
    states[1363] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391,8,-376,10,-376},new int[]{-168,1364,-167,1154,-166,1155,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[1364] = new State(new int[]{8,577,10,-462},new int[]{-124,1365});
    states[1365] = new State(new int[]{10,1143},new int[]{-203,1366});
    states[1366] = new State(-374);
    states[1367] = new State(-364);
    states[1368] = new State(-431);
    states[1369] = new State(-365);
    states[1370] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391},new int[]{-166,1371,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[1371] = new State(new int[]{11,1201,5,-389},new int[]{-229,1372,-234,1198});
    states[1372] = new State(new int[]{84,1187,85,1193,10,-396},new int[]{-198,1373});
    states[1373] = new State(new int[]{10,1374});
    states[1374] = new State(new int[]{61,1181,150,1183,149,1184,145,1185,148,1186,11,-386,26,-386,24,-386,42,-386,35,-386,28,-386,29,-386,44,-386,25,-386,90,-386,83,-386,82,-386,81,-386,80,-386},new int[]{-201,1375,-206,1376});
    states[1375] = new State(-378);
    states[1376] = new State(new int[]{10,1377});
    states[1377] = new State(new int[]{61,1181,11,-386,26,-386,24,-386,42,-386,35,-386,28,-386,29,-386,44,-386,25,-386,90,-386,83,-386,82,-386,81,-386,80,-386},new int[]{-201,1378});
    states[1378] = new State(-379);
    states[1379] = new State(new int[]{44,1380});
    states[1380] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391},new int[]{-166,1381,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[1381] = new State(new int[]{11,1201,5,-389},new int[]{-229,1382,-234,1198});
    states[1382] = new State(new int[]{108,1216,10,-385},new int[]{-207,1383});
    states[1383] = new State(new int[]{10,1384});
    states[1384] = new State(-382);
    states[1385] = new State(new int[]{11,639,90,-342,83,-342,82,-342,81,-342,80,-342,26,-211,24,-211,42,-211,35,-211,28,-211,29,-211,44,-211,25,-211},new int[]{-54,524,-53,525,-6,526,-246,1125,-55,1386});
    states[1386] = new State(-354);
    states[1387] = new State(-351);
    states[1388] = new State(-308);
    states[1389] = new State(-309);
    states[1390] = new State(new int[]{24,1391,46,1392,41,1393,8,-310,21,-310,11,-310,90,-310,83,-310,82,-310,81,-310,80,-310,27,-310,141,-310,84,-310,85,-310,79,-310,77,-310,157,-310,60,-310,26,-310,42,-310,35,-310,28,-310,29,-310,44,-310,25,-310,10,-310});
    states[1391] = new State(-311);
    states[1392] = new State(-312);
    states[1393] = new State(-313);
    states[1394] = new State(new int[]{67,1396,68,1397,145,1398,25,1399,26,1400,24,-305,41,-305,62,-305},new int[]{-20,1395});
    states[1395] = new State(-307);
    states[1396] = new State(-299);
    states[1397] = new State(-300);
    states[1398] = new State(-301);
    states[1399] = new State(-302);
    states[1400] = new State(-303);
    states[1401] = new State(-306);
    states[1402] = new State(new int[]{121,1404,118,-219},new int[]{-151,1403});
    states[1403] = new State(-220);
    states[1404] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-154,1405,-143,803,-147,48,-148,51});
    states[1405] = new State(new int[]{120,1406,119,1162,98,554});
    states[1406] = new State(-221);
    states[1407] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615,67,1396,68,1397,145,1398,25,1399,26,1400,24,-304,41,-304,62,-304},new int[]{-284,1408,-272,1261,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574,-30,1262,-21,1263,-22,1394,-20,1401});
    states[1408] = new State(new int[]{10,1409});
    states[1409] = new State(-218);
    states[1410] = new State(new int[]{11,639,141,-211,84,-211,85,-211,79,-211,77,-211,157,-211},new int[]{-49,1411,-6,1255,-246,1125});
    states[1411] = new State(-105);
    states[1412] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,8,1417,57,-91,27,-91,65,-91,48,-91,51,-91,60,-91,89,-91},new int[]{-311,1413,-308,1414,-309,1415,-154,804,-143,803,-147,48,-148,51});
    states[1413] = new State(-111);
    states[1414] = new State(-107);
    states[1415] = new State(new int[]{10,1416});
    states[1416] = new State(-403);
    states[1417] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,1418,-147,48,-148,51});
    states[1418] = new State(new int[]{98,1419});
    states[1419] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-154,1420,-143,803,-147,48,-148,51});
    states[1420] = new State(new int[]{9,1421,98,554});
    states[1421] = new State(new int[]{108,1422});
    states[1422] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,1423,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[1423] = new State(new int[]{10,1424});
    states[1424] = new State(-108);
    states[1425] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,8,1417},new int[]{-311,1426,-308,1414,-309,1415,-154,804,-143,803,-147,48,-148,51});
    states[1426] = new State(-109);
    states[1427] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,8,1417},new int[]{-311,1428,-308,1414,-309,1415,-154,804,-143,803,-147,48,-148,51});
    states[1428] = new State(-110);
    states[1429] = new State(-244);
    states[1430] = new State(-245);
    states[1431] = new State(new int[]{125,498,119,-246,98,-246,118,-246,9,-246,8,-246,136,-246,134,-246,116,-246,115,-246,129,-246,130,-246,131,-246,132,-246,128,-246,114,-246,113,-246,126,-246,127,-246,124,-246,6,-246,5,-246,123,-246,121,-246,122,-246,120,-246,135,-246,133,-246,16,-246,90,-246,10,-246,96,-246,99,-246,31,-246,102,-246,2,-246,12,-246,97,-246,30,-246,84,-246,83,-246,82,-246,81,-246,80,-246,85,-246,13,-246,75,-246,49,-246,56,-246,139,-246,141,-246,79,-246,77,-246,157,-246,43,-246,40,-246,19,-246,20,-246,142,-246,144,-246,143,-246,152,-246,155,-246,154,-246,153,-246,55,-246,89,-246,38,-246,23,-246,95,-246,52,-246,33,-246,53,-246,100,-246,45,-246,34,-246,51,-246,58,-246,73,-246,71,-246,36,-246,69,-246,70,-246,108,-246});
    states[1432] = new State(-677);
    states[1433] = new State(new int[]{8,1434});
    states[1434] = new State(new int[]{14,482,142,151,144,152,143,154,152,156,155,157,154,158,153,159,51,484,141,47,84,49,85,50,79,52,77,53,157,54,11,920,8,933},new int[]{-351,1435,-349,1441,-15,483,-161,148,-163,149,-162,153,-16,155,-338,1432,-280,1433,-176,203,-143,205,-147,48,-148,51,-341,1439,-342,1440});
    states[1435] = new State(new int[]{9,1436,10,480,98,1437});
    states[1436] = new State(-635);
    states[1437] = new State(new int[]{14,482,142,151,144,152,143,154,152,156,155,157,154,158,153,159,51,484,141,47,84,49,85,50,79,52,77,53,157,54,11,920,8,933},new int[]{-349,1438,-15,483,-161,148,-163,149,-162,153,-16,155,-338,1432,-280,1433,-176,203,-143,205,-147,48,-148,51,-341,1439,-342,1440});
    states[1438] = new State(-672);
    states[1439] = new State(-678);
    states[1440] = new State(-679);
    states[1441] = new State(-670);
    states[1442] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,5,609},new int[]{-116,1443,-102,1445,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,613,-263,590});
    states[1443] = new State(new int[]{12,1444});
    states[1444] = new State(-787);
    states[1445] = new State(new int[]{5,322,6,34});
    states[1446] = new State(-766);
    states[1447] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,665,12,-280,98,-280},new int[]{-267,1448,-268,1449,-90,179,-103,288,-104,289,-176,491,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153});
    states[1448] = new State(-278);
    states[1449] = new State(-279);
    states[1450] = new State(-277);
    states[1451] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-272,1452,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[1452] = new State(-276);
    states[1453] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,22,336},new int[]{-280,1454,-274,1455,-176,203,-143,205,-147,48,-148,51,-266,510});
    states[1454] = new State(-724);
    states[1455] = new State(-725);
    states[1456] = new State(-738);
    states[1457] = new State(-739);
    states[1458] = new State(-740);
    states[1459] = new State(-741);
    states[1460] = new State(-742);
    states[1461] = new State(-743);
    states[1462] = new State(-744);
    states[1463] = new State(-239);
    states[1464] = new State(-235);
    states[1465] = new State(-613);
    states[1466] = new State(new int[]{8,1467});
    states[1467] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-331,1468,-330,1476,-143,1472,-147,48,-148,51,-98,1475,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[1468] = new State(new int[]{9,1469,98,1470});
    states[1469] = new State(-624);
    states[1470] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-330,1471,-143,1472,-147,48,-148,51,-98,1475,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[1471] = new State(-628);
    states[1472] = new State(new int[]{108,1473,8,-778,7,-778,140,-778,4,-778,15,-778,136,-778,134,-778,116,-778,115,-778,129,-778,130,-778,131,-778,132,-778,128,-778,114,-778,113,-778,126,-778,127,-778,124,-778,6,-778,118,-778,123,-778,121,-778,119,-778,122,-778,120,-778,135,-778,133,-778,16,-778,9,-778,98,-778,13,-778,117,-778,11,-778,17,-778});
    states[1473] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600},new int[]{-98,1474,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599});
    states[1474] = new State(-625);
    states[1475] = new State(-626);
    states[1476] = new State(-627);
    states[1477] = new State(new int[]{13,191,16,195,5,-692,12,-692});
    states[1478] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-87,1479,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[1479] = new State(new int[]{13,191,16,195,98,-188,9,-188,12,-188,5,-188});
    states[1480] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161,5,-693,12,-693},new int[]{-118,1481,-87,1477,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[1481] = new State(new int[]{5,1482,12,-699});
    states[1482] = new State(new int[]{141,47,84,49,85,50,79,52,77,248,157,54,142,151,144,152,143,154,152,156,155,157,154,158,153,159,40,265,19,268,20,273,11,349,75,813,54,816,139,817,8,831,133,834,114,368,113,369,61,161},new int[]{-87,1483,-88,223,-79,231,-13,236,-10,246,-14,209,-143,247,-147,48,-148,51,-161,263,-163,149,-162,153,-16,264,-253,267,-292,272,-235,348,-195,840,-169,838,-57,839,-261,846,-265,847,-11,842,-237,848});
    states[1483] = new State(new int[]{13,191,16,195,12,-701});
    states[1484] = new State(-185);
    states[1485] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154},new int[]{-90,1486,-103,288,-104,289,-176,491,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153});
    states[1486] = new State(new int[]{114,232,113,233,126,234,127,235,13,-248,119,-248,98,-248,118,-248,9,-248,8,-248,136,-248,134,-248,116,-248,115,-248,129,-248,130,-248,131,-248,132,-248,128,-248,124,-248,6,-248,5,-248,123,-248,121,-248,122,-248,120,-248,135,-248,133,-248,16,-248,90,-248,10,-248,96,-248,99,-248,31,-248,102,-248,2,-248,12,-248,97,-248,30,-248,84,-248,83,-248,82,-248,81,-248,80,-248,85,-248,75,-248,49,-248,56,-248,139,-248,141,-248,79,-248,77,-248,157,-248,43,-248,40,-248,19,-248,20,-248,142,-248,144,-248,143,-248,152,-248,155,-248,154,-248,153,-248,55,-248,89,-248,38,-248,23,-248,95,-248,52,-248,33,-248,53,-248,100,-248,45,-248,34,-248,51,-248,58,-248,73,-248,71,-248,36,-248,69,-248,70,-248,125,-248,108,-248},new int[]{-189,180});
    states[1487] = new State(-619);
    states[1488] = new State(new int[]{13,343});
    states[1489] = new State(new int[]{13,497});
    states[1490] = new State(-714);
    states[1491] = new State(-633);
    states[1492] = new State(-35);
    states[1493] = new State(new int[]{57,1226,27,1247,65,1251,48,1410,51,1425,60,1427,11,639,89,-64,90,-64,101,-64,42,-211,35,-211,26,-211,24,-211,28,-211,29,-211},new int[]{-47,1494,-164,1495,-29,1496,-52,1497,-286,1498,-307,1499,-216,1500,-6,1501,-246,1125});
    states[1494] = new State(-68);
    states[1495] = new State(-78);
    states[1496] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,57,-79,27,-79,65,-79,48,-79,51,-79,60,-79,11,-79,42,-79,35,-79,26,-79,24,-79,28,-79,29,-79,89,-79,90,-79,101,-79},new int[]{-27,1233,-28,1234,-137,1236,-143,1246,-147,48,-148,51});
    states[1497] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,57,-80,27,-80,65,-80,48,-80,51,-80,60,-80,11,-80,42,-80,35,-80,26,-80,24,-80,28,-80,29,-80,89,-80,90,-80,101,-80},new int[]{-27,1250,-28,1234,-137,1236,-143,1246,-147,48,-148,51});
    states[1498] = new State(new int[]{11,639,57,-81,27,-81,65,-81,48,-81,51,-81,60,-81,42,-81,35,-81,26,-81,24,-81,28,-81,29,-81,89,-81,90,-81,101,-81,141,-211,84,-211,85,-211,79,-211,77,-211,157,-211},new int[]{-49,1254,-6,1255,-246,1125});
    states[1499] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,8,1417,57,-82,27,-82,65,-82,48,-82,51,-82,60,-82,11,-82,42,-82,35,-82,26,-82,24,-82,28,-82,29,-82,89,-82,90,-82,101,-82},new int[]{-311,1413,-308,1414,-309,1415,-154,804,-143,803,-147,48,-148,51});
    states[1500] = new State(-83);
    states[1501] = new State(new int[]{42,1514,35,1521,26,1341,24,1342,28,1549,29,1363,11,639},new int[]{-209,1502,-246,528,-210,1503,-217,1504,-224,1505,-221,1288,-225,1323,-3,1538,-213,1546,-223,1547});
    states[1502] = new State(-86);
    states[1503] = new State(-84);
    states[1504] = new State(-422);
    states[1505] = new State(new int[]{146,1507,105,1347,57,-67,27,-67,65,-67,48,-67,51,-67,60,-67,11,-67,42,-67,35,-67,26,-67,24,-67,28,-67,29,-67,89,-67},new int[]{-174,1506,-173,1509,-41,1510,-42,1493,-61,1513});
    states[1506] = new State(-424);
    states[1507] = new State(new int[]{10,1508});
    states[1508] = new State(-430);
    states[1509] = new State(-437);
    states[1510] = new State(new int[]{89,17},new int[]{-251,1511});
    states[1511] = new State(new int[]{10,1512});
    states[1512] = new State(-459);
    states[1513] = new State(-438);
    states[1514] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391},new int[]{-167,1515,-166,1155,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[1515] = new State(new int[]{8,577,10,-462,108,-462},new int[]{-124,1516});
    states[1516] = new State(new int[]{10,1321,108,-811},new int[]{-203,1292,-204,1517});
    states[1517] = new State(new int[]{108,1518});
    states[1518] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486},new int[]{-256,1519,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[1519] = new State(new int[]{10,1520});
    states[1520] = new State(-429);
    states[1521] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391},new int[]{-166,1522,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[1522] = new State(new int[]{8,577,5,-462,10,-462,108,-462},new int[]{-124,1523});
    states[1523] = new State(new int[]{5,1524,10,1321,108,-811},new int[]{-203,1327,-204,1532});
    states[1524] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-271,1525,-272,506,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[1525] = new State(new int[]{10,1321,108,-811},new int[]{-203,1331,-204,1526});
    states[1526] = new State(new int[]{108,1527});
    states[1527] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,652,19,268,20,273,75,462,38,600,18,674,35,683,42,687},new int[]{-98,1528,-320,1530,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-97,659,-321,682});
    states[1528] = new State(new int[]{10,1529});
    states[1529] = new State(-425);
    states[1530] = new State(new int[]{10,1531});
    states[1531] = new State(-427);
    states[1532] = new State(new int[]{108,1533});
    states[1533] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,652,19,268,20,273,75,462,38,600,18,674,35,683,42,687},new int[]{-98,1534,-320,1536,-96,29,-95,311,-102,450,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,445,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-97,659,-321,682});
    states[1534] = new State(new int[]{10,1535});
    states[1535] = new State(-426);
    states[1536] = new State(new int[]{10,1537});
    states[1537] = new State(-428);
    states[1538] = new State(new int[]{28,1540,42,1514,35,1521},new int[]{-217,1539,-224,1505,-221,1288,-225,1323});
    states[1539] = new State(-423);
    states[1540] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391,8,-376,108,-376,10,-376},new int[]{-168,1541,-167,1154,-166,1155,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[1541] = new State(new int[]{8,577,108,-462,10,-462},new int[]{-124,1542});
    states[1542] = new State(new int[]{108,1543,10,1143},new int[]{-203,536});
    states[1543] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486},new int[]{-256,1544,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[1544] = new State(new int[]{10,1545});
    states[1545] = new State(-418);
    states[1546] = new State(-85);
    states[1547] = new State(-67,new int[]{-173,1548,-41,1510,-42,1493});
    states[1548] = new State(-416);
    states[1549] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391,8,-376,108,-376,10,-376},new int[]{-168,1550,-167,1154,-166,1155,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[1550] = new State(new int[]{8,577,108,-462,10,-462},new int[]{-124,1551});
    states[1551] = new State(new int[]{108,1552,10,1143},new int[]{-203,1359});
    states[1552] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,156,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,10,-486},new int[]{-256,1553,-4,23,-109,24,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879});
    states[1553] = new State(new int[]{10,1554});
    states[1554] = new State(-417);
    states[1555] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,83,57,82,58,81,59,80,60,67,61,62,62,126,63,20,64,19,65,61,66,21,67,127,68,128,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,22,78,72,79,89,80,23,81,24,82,27,83,28,84,29,85,70,86,97,87,30,88,90,89,31,90,32,91,25,92,102,93,99,94,33,95,34,96,35,97,38,98,39,99,40,100,101,101,41,102,42,103,44,104,45,105,46,106,95,107,47,108,100,109,48,110,26,111,49,112,69,113,96,114,50,115,51,116,52,117,53,118,54,119,55,120,56,121,57,122,59,123,103,124,104,125,107,126,105,127,106,128,60,129,73,130,36,131,37,132,68,133,145,134,58,135,137,136,138,137,78,138,150,139,149,140,71,141,151,142,147,143,148,144,146,145,43,147},new int[]{-301,1556,-305,1566,-153,1560,-134,1565,-143,46,-147,48,-148,51,-290,55,-146,56,-291,146});
    states[1556] = new State(new int[]{10,1557,98,1558});
    states[1557] = new State(-38);
    states[1558] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,83,57,82,58,81,59,80,60,67,61,62,62,126,63,20,64,19,65,61,66,21,67,127,68,128,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,22,78,72,79,89,80,23,81,24,82,27,83,28,84,29,85,70,86,97,87,30,88,90,89,31,90,32,91,25,92,102,93,99,94,33,95,34,96,35,97,38,98,39,99,40,100,101,101,41,102,42,103,44,104,45,105,46,106,95,107,47,108,100,109,48,110,26,111,49,112,69,113,96,114,50,115,51,116,52,117,53,118,54,119,55,120,56,121,57,122,59,123,103,124,104,125,107,126,105,127,106,128,60,129,73,130,36,131,37,132,68,133,145,134,58,135,137,136,138,137,78,138,150,139,149,140,71,141,151,142,147,143,148,144,146,145,43,147},new int[]{-305,1559,-153,1560,-134,1565,-143,46,-147,48,-148,51,-290,55,-146,56,-291,146});
    states[1559] = new State(-44);
    states[1560] = new State(new int[]{7,1561,135,1563,10,-45,98,-45});
    states[1561] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,83,57,82,58,81,59,80,60,67,61,62,62,126,63,20,64,19,65,61,66,21,67,127,68,128,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,22,78,72,79,89,80,23,81,24,82,27,83,28,84,29,85,70,86,97,87,30,88,90,89,31,90,32,91,25,92,102,93,99,94,33,95,34,96,35,97,38,98,39,99,40,100,101,101,41,102,42,103,44,104,45,105,46,106,95,107,47,108,100,109,48,110,26,111,49,112,69,113,96,114,50,115,51,116,52,117,53,118,54,119,55,120,56,121,57,122,59,123,103,124,104,125,107,126,105,127,106,128,60,129,73,130,36,131,37,132,68,133,145,134,58,135,137,136,138,137,78,138,150,139,149,140,71,141,151,142,147,143,148,144,146,145,43,147},new int[]{-134,1562,-143,46,-147,48,-148,51,-290,55,-146,56,-291,146});
    states[1562] = new State(-37);
    states[1563] = new State(new int[]{142,1564});
    states[1564] = new State(-46);
    states[1565] = new State(-36);
    states[1566] = new State(-43);
    states[1567] = new State(new int[]{3,1569,50,-15,89,-15,57,-15,27,-15,65,-15,48,-15,51,-15,60,-15,11,-15,42,-15,35,-15,26,-15,24,-15,28,-15,29,-15,41,-15,90,-15,101,-15},new int[]{-180,1568});
    states[1568] = new State(-17);
    states[1569] = new State(new int[]{141,1570,142,1571});
    states[1570] = new State(-18);
    states[1571] = new State(-19);
    states[1572] = new State(-16);
    states[1573] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-143,1574,-147,48,-148,51});
    states[1574] = new State(new int[]{10,1576,8,1577},new int[]{-183,1575});
    states[1575] = new State(-28);
    states[1576] = new State(-29);
    states[1577] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-185,1578,-142,1584,-143,1583,-147,48,-148,51});
    states[1578] = new State(new int[]{9,1579,98,1581});
    states[1579] = new State(new int[]{10,1580});
    states[1580] = new State(-30);
    states[1581] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-142,1582,-143,1583,-147,48,-148,51});
    states[1582] = new State(-32);
    states[1583] = new State(-33);
    states[1584] = new State(-31);
    states[1585] = new State(-3);
    states[1586] = new State(new int[]{41,1607,50,-41,57,-41,27,-41,65,-41,48,-41,51,-41,60,-41,11,-41,42,-41,35,-41,26,-41,24,-41,28,-41,29,-41,90,-41,101,-41,89,-41},new int[]{-158,1587,-159,1604,-300,1633});
    states[1587] = new State(new int[]{39,1601},new int[]{-157,1588});
    states[1588] = new State(new int[]{90,1591,101,1592,89,1598},new int[]{-150,1589});
    states[1589] = new State(new int[]{7,1590});
    states[1590] = new State(-47);
    states[1591] = new State(-57);
    states[1592] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,748,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,102,-486,10,-486},new int[]{-248,1593,-257,746,-256,22,-4,23,-109,24,-128,373,-108,385,-143,747,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879,-139,1034});
    states[1593] = new State(new int[]{90,1594,102,1595,10,20});
    states[1594] = new State(-58);
    states[1595] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,748,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486},new int[]{-248,1596,-257,746,-256,22,-4,23,-109,24,-128,373,-108,385,-143,747,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879,-139,1034});
    states[1596] = new State(new int[]{90,1597,10,20});
    states[1597] = new State(-59);
    states[1598] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,748,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,90,-486,10,-486},new int[]{-248,1599,-257,746,-256,22,-4,23,-109,24,-128,373,-108,385,-143,747,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879,-139,1034});
    states[1599] = new State(new int[]{90,1600,10,20});
    states[1600] = new State(-60);
    states[1601] = new State(-41,new int[]{-300,1602});
    states[1602] = new State(new int[]{50,1555,57,-67,27,-67,65,-67,48,-67,51,-67,60,-67,11,-67,42,-67,35,-67,26,-67,24,-67,28,-67,29,-67,90,-67,101,-67,89,-67},new int[]{-41,1603,-302,14,-42,1493});
    states[1603] = new State(-55);
    states[1604] = new State(new int[]{90,1591,101,1592,89,1598},new int[]{-150,1605});
    states[1605] = new State(new int[]{7,1606});
    states[1606] = new State(-48);
    states[1607] = new State(-41,new int[]{-300,1608});
    states[1608] = new State(new int[]{50,1555,27,-62,65,-62,48,-62,51,-62,60,-62,11,-62,42,-62,35,-62,39,-62},new int[]{-40,1609,-302,14,-38,1610});
    states[1609] = new State(-54);
    states[1610] = new State(new int[]{27,1247,65,1251,48,1410,51,1425,60,1427,11,639,39,-61,42,-211,35,-211},new int[]{-48,1611,-29,1612,-52,1613,-286,1614,-307,1615,-228,1616,-6,1617,-246,1125,-227,1632});
    states[1611] = new State(-63);
    states[1612] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,27,-72,65,-72,48,-72,51,-72,60,-72,11,-72,42,-72,35,-72,39,-72},new int[]{-27,1233,-28,1234,-137,1236,-143,1246,-147,48,-148,51});
    states[1613] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,27,-73,65,-73,48,-73,51,-73,60,-73,11,-73,42,-73,35,-73,39,-73},new int[]{-27,1250,-28,1234,-137,1236,-143,1246,-147,48,-148,51});
    states[1614] = new State(new int[]{11,639,27,-74,65,-74,48,-74,51,-74,60,-74,42,-74,35,-74,39,-74,141,-211,84,-211,85,-211,79,-211,77,-211,157,-211},new int[]{-49,1254,-6,1255,-246,1125});
    states[1615] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,8,1417,27,-75,65,-75,48,-75,51,-75,60,-75,11,-75,42,-75,35,-75,39,-75},new int[]{-311,1413,-308,1414,-309,1415,-154,804,-143,803,-147,48,-148,51});
    states[1616] = new State(-76);
    states[1617] = new State(new int[]{42,1624,11,639,35,1627},new int[]{-221,1618,-246,528,-225,1621});
    states[1618] = new State(new int[]{146,1619,27,-92,65,-92,48,-92,51,-92,60,-92,11,-92,42,-92,35,-92,39,-92});
    states[1619] = new State(new int[]{10,1620});
    states[1620] = new State(-93);
    states[1621] = new State(new int[]{146,1622,27,-94,65,-94,48,-94,51,-94,60,-94,11,-94,42,-94,35,-94,39,-94});
    states[1622] = new State(new int[]{10,1623});
    states[1623] = new State(-95);
    states[1624] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391},new int[]{-167,1625,-166,1155,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[1625] = new State(new int[]{8,577,10,-462},new int[]{-124,1626});
    states[1626] = new State(new int[]{10,1143},new int[]{-203,1292});
    states[1627] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,43,391},new int[]{-166,1628,-138,1156,-133,1157,-130,1158,-143,1163,-147,48,-148,51,-187,1164,-332,1166,-145,1170});
    states[1628] = new State(new int[]{8,577,5,-462,10,-462},new int[]{-124,1629});
    states[1629] = new State(new int[]{5,1630,10,1143},new int[]{-203,1327});
    states[1630] = new State(new int[]{141,344,84,49,85,50,79,52,77,53,157,54,152,156,155,157,154,158,153,159,114,368,113,369,142,151,144,152,143,154,8,493,140,504,22,336,46,512,47,559,32,563,72,567,63,570,42,575,35,615},new int[]{-271,1631,-272,506,-268,342,-90,179,-103,288,-104,289,-176,290,-143,205,-147,48,-148,51,-16,488,-195,489,-161,492,-163,149,-162,153,-269,495,-298,496,-252,502,-245,503,-277,507,-278,508,-274,509,-266,510,-31,511,-259,558,-126,562,-127,566,-222,572,-220,573,-219,574});
    states[1631] = new State(new int[]{10,1143},new int[]{-203,1331});
    states[1632] = new State(-77);
    states[1633] = new State(new int[]{50,1555,57,-67,27,-67,65,-67,48,-67,51,-67,60,-67,11,-67,42,-67,35,-67,26,-67,24,-67,28,-67,29,-67,90,-67,101,-67,89,-67},new int[]{-41,1634,-302,14,-42,1493});
    states[1634] = new State(-56);
    states[1635] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-135,1636,-143,1639,-147,48,-148,51});
    states[1636] = new State(new int[]{10,1637});
    states[1637] = new State(new int[]{3,1569,41,-14,90,-14,101,-14,89,-14,50,-14,57,-14,27,-14,65,-14,48,-14,51,-14,60,-14,11,-14,42,-14,35,-14,26,-14,24,-14,28,-14,29,-14},new int[]{-181,1638,-182,1567,-180,1572});
    states[1638] = new State(-49);
    states[1639] = new State(-53);
    states[1640] = new State(-51);
    states[1641] = new State(-52);
    states[1642] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,83,57,82,58,81,59,80,60,67,61,62,62,126,63,20,64,19,65,61,66,21,67,127,68,128,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,22,78,72,79,89,80,23,81,24,82,27,83,28,84,29,85,70,86,97,87,30,88,90,89,31,90,32,91,25,92,102,93,99,94,33,95,34,96,35,97,38,98,39,99,40,100,101,101,41,102,42,103,44,104,45,105,46,106,95,107,47,108,100,109,48,110,26,111,49,112,69,113,96,114,50,115,51,116,52,117,53,118,54,119,55,120,56,121,57,122,59,123,103,124,104,125,107,126,105,127,106,128,60,129,73,130,36,131,37,132,68,133,145,134,58,135,137,136,138,137,78,138,150,139,149,140,71,141,151,142,147,143,148,144,146,145,43,147},new int[]{-153,1643,-134,1565,-143,46,-147,48,-148,51,-290,55,-146,56,-291,146});
    states[1643] = new State(new int[]{10,1644,7,1561});
    states[1644] = new State(new int[]{3,1569,41,-14,90,-14,101,-14,89,-14,50,-14,57,-14,27,-14,65,-14,48,-14,51,-14,60,-14,11,-14,42,-14,35,-14,26,-14,24,-14,28,-14,29,-14},new int[]{-181,1645,-182,1567,-180,1572});
    states[1645] = new State(-50);
    states[1646] = new State(-4);
    states[1647] = new State(new int[]{48,1649,54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,431,19,268,20,273,75,462,38,600,5,609},new int[]{-85,1648,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,383,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608});
    states[1648] = new State(-7);
    states[1649] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-140,1650,-143,1651,-147,48,-148,51});
    states[1650] = new State(-8);
    states[1651] = new State(new int[]{121,1160,2,-219},new int[]{-151,1403});
    states[1652] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54},new int[]{-318,1653,-319,1654,-143,1658,-147,48,-148,51});
    states[1653] = new State(-9);
    states[1654] = new State(new int[]{7,1655,121,172,2,-771},new int[]{-296,1657});
    states[1655] = new State(new int[]{141,47,84,49,85,50,79,52,77,53,157,54,83,57,82,58,81,59,80,60,67,61,62,62,126,63,20,64,19,65,61,66,21,67,127,68,128,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,22,78,72,79,89,80,23,81,24,82,27,83,28,84,29,85,70,86,97,87,30,88,90,89,31,90,32,91,25,92,102,93,99,94,33,95,34,96,35,97,38,98,39,99,40,100,101,101,41,102,42,103,44,104,45,105,46,106,95,107,47,108,100,109,48,110,26,111,49,112,69,113,96,114,50,115,51,116,52,117,53,118,54,119,55,120,56,121,57,122,59,123,103,124,104,125,107,126,105,127,106,128,60,129,73,130,36,131,37,132,68,133,145,134,58,135,137,136,138,137,78,138,150,139,149,140,71,141,151,142,147,143,148,144,146,145,43,147},new int[]{-134,1656,-143,46,-147,48,-148,51,-290,55,-146,56,-291,146});
    states[1656] = new State(-770);
    states[1657] = new State(-772);
    states[1658] = new State(-769);
    states[1659] = new State(new int[]{54,42,142,151,144,152,143,154,152,156,155,157,154,158,153,159,61,161,11,359,133,363,114,368,113,369,140,370,139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,429,8,541,19,268,20,273,75,462,38,600,5,609,51,794},new int[]{-255,1660,-85,1661,-98,28,-96,29,-95,311,-102,321,-81,326,-80,332,-93,358,-15,43,-161,148,-163,149,-162,153,-16,155,-57,160,-195,381,-109,706,-128,373,-108,385,-143,427,-147,48,-148,51,-187,428,-253,438,-292,439,-17,440,-58,465,-112,468,-169,469,-264,470,-94,471,-260,475,-262,476,-263,590,-236,591,-113,592,-238,599,-116,608,-4,1662,-312,1663});
    states[1660] = new State(-10);
    states[1661] = new State(-11);
    states[1662] = new State(-12);
    states[1663] = new State(-13);
    states[1664] = new State(new int[]{50,1555,139,-39,141,-39,84,-39,85,-39,79,-39,77,-39,157,-39,43,-39,40,-39,8,-39,19,-39,20,-39,142,-39,144,-39,143,-39,152,-39,155,-39,154,-39,153,-39,75,-39,55,-39,89,-39,38,-39,23,-39,95,-39,52,-39,33,-39,53,-39,100,-39,45,-39,34,-39,51,-39,58,-39,73,-39,71,-39,36,-39,11,-39,10,-39,42,-39,35,-39,2,-39},new int[]{-303,1665,-302,1670});
    states[1665] = new State(-65,new int[]{-44,1666});
    states[1666] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,748,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,11,639,10,-486,2,-486,42,-211,35,-211},new int[]{-248,1667,-6,1668,-257,746,-256,22,-4,23,-109,24,-128,373,-108,385,-143,747,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879,-139,1034,-246,1125});
    states[1667] = new State(new int[]{10,20,2,-5});
    states[1668] = new State(new int[]{42,1514,35,1521,11,639},new int[]{-217,1669,-246,528,-224,1505,-221,1288,-225,1323});
    states[1669] = new State(-66);
    states[1670] = new State(-40);
    states[1671] = new State(new int[]{50,1555,139,-39,141,-39,84,-39,85,-39,79,-39,77,-39,157,-39,43,-39,40,-39,8,-39,19,-39,20,-39,142,-39,144,-39,143,-39,152,-39,155,-39,154,-39,153,-39,75,-39,55,-39,89,-39,38,-39,23,-39,95,-39,52,-39,33,-39,53,-39,100,-39,45,-39,34,-39,51,-39,58,-39,73,-39,71,-39,36,-39,11,-39,10,-39,42,-39,35,-39,2,-39},new int[]{-303,1672,-302,1670});
    states[1672] = new State(-65,new int[]{-44,1673});
    states[1673] = new State(new int[]{139,384,141,47,84,49,85,50,79,52,77,248,157,54,43,391,40,540,8,541,19,268,20,273,142,151,144,152,143,154,152,748,155,157,154,158,153,159,75,462,55,727,89,17,38,719,23,734,95,750,52,755,33,760,53,770,100,776,45,783,34,786,51,794,58,866,73,871,71,858,36,880,11,639,10,-486,2,-486,42,-211,35,-211},new int[]{-248,1674,-6,1668,-257,746,-256,22,-4,23,-109,24,-128,373,-108,385,-143,747,-147,48,-148,51,-187,428,-253,438,-292,439,-15,700,-161,148,-163,149,-162,153,-16,155,-17,440,-58,701,-112,468,-208,725,-129,726,-251,731,-149,732,-35,733,-243,749,-316,754,-120,759,-317,769,-156,774,-299,775,-244,782,-119,785,-312,793,-59,862,-170,863,-169,864,-165,865,-122,870,-123,877,-121,878,-346,879,-139,1034,-246,1125});
    states[1674] = new State(new int[]{10,20,2,-6});

    rules[1] = new Rule(-358, new int[]{-1,2});
    rules[2] = new Rule(-1, new int[]{-230});
    rules[3] = new Rule(-1, new int[]{-304});
    rules[4] = new Rule(-1, new int[]{-171});
    rules[5] = new Rule(-1, new int[]{74,-303,-44,-248});
    rules[6] = new Rule(-1, new int[]{76,-303,-44,-248});
    rules[7] = new Rule(-171, new int[]{86,-85});
    rules[8] = new Rule(-171, new int[]{86,48,-140});
    rules[9] = new Rule(-171, new int[]{88,-318});
    rules[10] = new Rule(-171, new int[]{87,-255});
    rules[11] = new Rule(-255, new int[]{-85});
    rules[12] = new Rule(-255, new int[]{-4});
    rules[13] = new Rule(-255, new int[]{-312});
    rules[14] = new Rule(-181, new int[]{});
    rules[15] = new Rule(-181, new int[]{-182});
    rules[16] = new Rule(-182, new int[]{-180});
    rules[17] = new Rule(-182, new int[]{-182,-180});
    rules[18] = new Rule(-180, new int[]{3,141});
    rules[19] = new Rule(-180, new int[]{3,142});
    rules[20] = new Rule(-230, new int[]{-231,-181,-300,-18,-184});
    rules[21] = new Rule(-184, new int[]{7});
    rules[22] = new Rule(-184, new int[]{10});
    rules[23] = new Rule(-184, new int[]{5});
    rules[24] = new Rule(-184, new int[]{98});
    rules[25] = new Rule(-184, new int[]{6});
    rules[26] = new Rule(-184, new int[]{});
    rules[27] = new Rule(-231, new int[]{});
    rules[28] = new Rule(-231, new int[]{59,-143,-183});
    rules[29] = new Rule(-183, new int[]{10});
    rules[30] = new Rule(-183, new int[]{8,-185,9,10});
    rules[31] = new Rule(-185, new int[]{-142});
    rules[32] = new Rule(-185, new int[]{-185,98,-142});
    rules[33] = new Rule(-142, new int[]{-143});
    rules[34] = new Rule(-18, new int[]{-37,-251});
    rules[35] = new Rule(-37, new int[]{-41});
    rules[36] = new Rule(-153, new int[]{-134});
    rules[37] = new Rule(-153, new int[]{-153,7,-134});
    rules[38] = new Rule(-302, new int[]{50,-301,10});
    rules[39] = new Rule(-303, new int[]{});
    rules[40] = new Rule(-303, new int[]{-302});
    rules[41] = new Rule(-300, new int[]{});
    rules[42] = new Rule(-300, new int[]{-300,-302});
    rules[43] = new Rule(-301, new int[]{-305});
    rules[44] = new Rule(-301, new int[]{-301,98,-305});
    rules[45] = new Rule(-305, new int[]{-153});
    rules[46] = new Rule(-305, new int[]{-153,135,142});
    rules[47] = new Rule(-304, new int[]{-306,-158,-157,-150,7});
    rules[48] = new Rule(-304, new int[]{-306,-159,-150,7});
    rules[49] = new Rule(-306, new int[]{-2,-135,10,-181});
    rules[50] = new Rule(-306, new int[]{107,-153,10,-181});
    rules[51] = new Rule(-2, new int[]{103});
    rules[52] = new Rule(-2, new int[]{104});
    rules[53] = new Rule(-135, new int[]{-143});
    rules[54] = new Rule(-158, new int[]{41,-300,-40});
    rules[55] = new Rule(-157, new int[]{39,-300,-41});
    rules[56] = new Rule(-159, new int[]{-300,-41});
    rules[57] = new Rule(-150, new int[]{90});
    rules[58] = new Rule(-150, new int[]{101,-248,90});
    rules[59] = new Rule(-150, new int[]{101,-248,102,-248,90});
    rules[60] = new Rule(-150, new int[]{89,-248,90});
    rules[61] = new Rule(-40, new int[]{-38});
    rules[62] = new Rule(-38, new int[]{});
    rules[63] = new Rule(-38, new int[]{-38,-48});
    rules[64] = new Rule(-41, new int[]{-42});
    rules[65] = new Rule(-44, new int[]{});
    rules[66] = new Rule(-44, new int[]{-44,-6,-217});
    rules[67] = new Rule(-42, new int[]{});
    rules[68] = new Rule(-42, new int[]{-42,-47});
    rules[69] = new Rule(-43, new int[]{-39});
    rules[70] = new Rule(-39, new int[]{});
    rules[71] = new Rule(-39, new int[]{-39,-46});
    rules[72] = new Rule(-48, new int[]{-29});
    rules[73] = new Rule(-48, new int[]{-52});
    rules[74] = new Rule(-48, new int[]{-286});
    rules[75] = new Rule(-48, new int[]{-307});
    rules[76] = new Rule(-48, new int[]{-228});
    rules[77] = new Rule(-48, new int[]{-227});
    rules[78] = new Rule(-47, new int[]{-164});
    rules[79] = new Rule(-47, new int[]{-29});
    rules[80] = new Rule(-47, new int[]{-52});
    rules[81] = new Rule(-47, new int[]{-286});
    rules[82] = new Rule(-47, new int[]{-307});
    rules[83] = new Rule(-47, new int[]{-216});
    rules[84] = new Rule(-209, new int[]{-210});
    rules[85] = new Rule(-209, new int[]{-213});
    rules[86] = new Rule(-216, new int[]{-6,-209});
    rules[87] = new Rule(-46, new int[]{-164});
    rules[88] = new Rule(-46, new int[]{-29});
    rules[89] = new Rule(-46, new int[]{-52});
    rules[90] = new Rule(-46, new int[]{-286});
    rules[91] = new Rule(-46, new int[]{-307});
    rules[92] = new Rule(-228, new int[]{-6,-221});
    rules[93] = new Rule(-228, new int[]{-6,-221,146,10});
    rules[94] = new Rule(-227, new int[]{-6,-225});
    rules[95] = new Rule(-227, new int[]{-6,-225,146,10});
    rules[96] = new Rule(-164, new int[]{57,-152,10});
    rules[97] = new Rule(-152, new int[]{-139});
    rules[98] = new Rule(-152, new int[]{-152,98,-139});
    rules[99] = new Rule(-139, new int[]{152});
    rules[100] = new Rule(-139, new int[]{-143});
    rules[101] = new Rule(-29, new int[]{27,-27});
    rules[102] = new Rule(-29, new int[]{-29,-27});
    rules[103] = new Rule(-52, new int[]{65,-27});
    rules[104] = new Rule(-52, new int[]{-52,-27});
    rules[105] = new Rule(-286, new int[]{48,-49});
    rules[106] = new Rule(-286, new int[]{-286,-49});
    rules[107] = new Rule(-311, new int[]{-308});
    rules[108] = new Rule(-311, new int[]{8,-143,98,-154,9,108,-98,10});
    rules[109] = new Rule(-307, new int[]{51,-311});
    rules[110] = new Rule(-307, new int[]{60,-311});
    rules[111] = new Rule(-307, new int[]{-307,-311});
    rules[112] = new Rule(-27, new int[]{-28,10});
    rules[113] = new Rule(-28, new int[]{-137,118,-106});
    rules[114] = new Rule(-28, new int[]{-137,5,-272,118,-82});
    rules[115] = new Rule(-106, new int[]{-87});
    rules[116] = new Rule(-106, new int[]{-92});
    rules[117] = new Rule(-137, new int[]{-143});
    rules[118] = new Rule(-88, new int[]{-79});
    rules[119] = new Rule(-88, new int[]{-88,-188,-79});
    rules[120] = new Rule(-87, new int[]{-88});
    rules[121] = new Rule(-87, new int[]{-237});
    rules[122] = new Rule(-87, new int[]{-87,16,-88});
    rules[123] = new Rule(-237, new int[]{-87,13,-87,5,-87});
    rules[124] = new Rule(-188, new int[]{118});
    rules[125] = new Rule(-188, new int[]{123});
    rules[126] = new Rule(-188, new int[]{121});
    rules[127] = new Rule(-188, new int[]{119});
    rules[128] = new Rule(-188, new int[]{122});
    rules[129] = new Rule(-188, new int[]{120});
    rules[130] = new Rule(-188, new int[]{135});
    rules[131] = new Rule(-79, new int[]{-13});
    rules[132] = new Rule(-79, new int[]{-79,-189,-13});
    rules[133] = new Rule(-189, new int[]{114});
    rules[134] = new Rule(-189, new int[]{113});
    rules[135] = new Rule(-189, new int[]{126});
    rules[136] = new Rule(-189, new int[]{127});
    rules[137] = new Rule(-261, new int[]{-13,-197,-280});
    rules[138] = new Rule(-265, new int[]{-11,117,-10});
    rules[139] = new Rule(-265, new int[]{-11,117,-265});
    rules[140] = new Rule(-265, new int[]{-195,-265});
    rules[141] = new Rule(-13, new int[]{-10});
    rules[142] = new Rule(-13, new int[]{-261});
    rules[143] = new Rule(-13, new int[]{-265});
    rules[144] = new Rule(-13, new int[]{-13,-191,-10});
    rules[145] = new Rule(-13, new int[]{-13,-191,-265});
    rules[146] = new Rule(-191, new int[]{116});
    rules[147] = new Rule(-191, new int[]{115});
    rules[148] = new Rule(-191, new int[]{129});
    rules[149] = new Rule(-191, new int[]{130});
    rules[150] = new Rule(-191, new int[]{131});
    rules[151] = new Rule(-191, new int[]{132});
    rules[152] = new Rule(-191, new int[]{128});
    rules[153] = new Rule(-11, new int[]{-14});
    rules[154] = new Rule(-11, new int[]{8,-87,9});
    rules[155] = new Rule(-10, new int[]{-14});
    rules[156] = new Rule(-10, new int[]{-235});
    rules[157] = new Rule(-10, new int[]{54});
    rules[158] = new Rule(-10, new int[]{139,-10});
    rules[159] = new Rule(-10, new int[]{8,-87,9});
    rules[160] = new Rule(-10, new int[]{133,-10});
    rules[161] = new Rule(-10, new int[]{-195,-10});
    rules[162] = new Rule(-10, new int[]{-169});
    rules[163] = new Rule(-10, new int[]{-57});
    rules[164] = new Rule(-235, new int[]{11,-68,12});
    rules[165] = new Rule(-235, new int[]{75,-68,75});
    rules[166] = new Rule(-195, new int[]{114});
    rules[167] = new Rule(-195, new int[]{113});
    rules[168] = new Rule(-14, new int[]{-143});
    rules[169] = new Rule(-14, new int[]{-161});
    rules[170] = new Rule(-14, new int[]{-16});
    rules[171] = new Rule(-14, new int[]{40,-143});
    rules[172] = new Rule(-14, new int[]{-253});
    rules[173] = new Rule(-14, new int[]{-292});
    rules[174] = new Rule(-14, new int[]{-14,-12});
    rules[175] = new Rule(-14, new int[]{-14,4,-296});
    rules[176] = new Rule(-14, new int[]{-14,11,-117,12});
    rules[177] = new Rule(-12, new int[]{7,-134});
    rules[178] = new Rule(-12, new int[]{140});
    rules[179] = new Rule(-12, new int[]{8,-74,9});
    rules[180] = new Rule(-12, new int[]{11,-73,12});
    rules[181] = new Rule(-74, new int[]{-70});
    rules[182] = new Rule(-74, new int[]{});
    rules[183] = new Rule(-73, new int[]{-71});
    rules[184] = new Rule(-73, new int[]{});
    rules[185] = new Rule(-71, new int[]{-91});
    rules[186] = new Rule(-71, new int[]{-71,98,-91});
    rules[187] = new Rule(-91, new int[]{-87});
    rules[188] = new Rule(-91, new int[]{-87,6,-87});
    rules[189] = new Rule(-16, new int[]{152});
    rules[190] = new Rule(-16, new int[]{155});
    rules[191] = new Rule(-16, new int[]{154});
    rules[192] = new Rule(-16, new int[]{153});
    rules[193] = new Rule(-82, new int[]{-87});
    rules[194] = new Rule(-82, new int[]{-92});
    rules[195] = new Rule(-82, new int[]{-239});
    rules[196] = new Rule(-92, new int[]{8,-66,9});
    rules[197] = new Rule(-66, new int[]{});
    rules[198] = new Rule(-66, new int[]{-65});
    rules[199] = new Rule(-65, new int[]{-83});
    rules[200] = new Rule(-65, new int[]{-65,98,-83});
    rules[201] = new Rule(-239, new int[]{8,-241,9});
    rules[202] = new Rule(-241, new int[]{-240});
    rules[203] = new Rule(-241, new int[]{-240,10});
    rules[204] = new Rule(-240, new int[]{-242});
    rules[205] = new Rule(-240, new int[]{-240,10,-242});
    rules[206] = new Rule(-242, new int[]{-132,5,-82});
    rules[207] = new Rule(-132, new int[]{-143});
    rules[208] = new Rule(-49, new int[]{-6,-50});
    rules[209] = new Rule(-6, new int[]{-246});
    rules[210] = new Rule(-6, new int[]{-6,-246});
    rules[211] = new Rule(-6, new int[]{});
    rules[212] = new Rule(-246, new int[]{11,-247,12});
    rules[213] = new Rule(-247, new int[]{-8});
    rules[214] = new Rule(-247, new int[]{-247,98,-8});
    rules[215] = new Rule(-8, new int[]{-9});
    rules[216] = new Rule(-8, new int[]{-143,5,-9});
    rules[217] = new Rule(-50, new int[]{-140,118,-284,10});
    rules[218] = new Rule(-50, new int[]{-141,-284,10});
    rules[219] = new Rule(-140, new int[]{-143});
    rules[220] = new Rule(-140, new int[]{-143,-151});
    rules[221] = new Rule(-141, new int[]{-143,121,-154,120});
    rules[222] = new Rule(-284, new int[]{-272});
    rules[223] = new Rule(-284, new int[]{-30});
    rules[224] = new Rule(-269, new int[]{-268,13});
    rules[225] = new Rule(-269, new int[]{-298,13});
    rules[226] = new Rule(-272, new int[]{-268});
    rules[227] = new Rule(-272, new int[]{-269});
    rules[228] = new Rule(-272, new int[]{-252});
    rules[229] = new Rule(-272, new int[]{-245});
    rules[230] = new Rule(-272, new int[]{-277});
    rules[231] = new Rule(-272, new int[]{-222});
    rules[232] = new Rule(-272, new int[]{-298});
    rules[233] = new Rule(-298, new int[]{-176,-296});
    rules[234] = new Rule(-296, new int[]{121,-294,119});
    rules[235] = new Rule(-297, new int[]{123});
    rules[236] = new Rule(-297, new int[]{121,-295,119});
    rules[237] = new Rule(-294, new int[]{-275});
    rules[238] = new Rule(-294, new int[]{-294,98,-275});
    rules[239] = new Rule(-295, new int[]{-276});
    rules[240] = new Rule(-295, new int[]{-295,98,-276});
    rules[241] = new Rule(-276, new int[]{});
    rules[242] = new Rule(-275, new int[]{-268});
    rules[243] = new Rule(-275, new int[]{-268,13});
    rules[244] = new Rule(-275, new int[]{-277});
    rules[245] = new Rule(-275, new int[]{-222});
    rules[246] = new Rule(-275, new int[]{-298});
    rules[247] = new Rule(-268, new int[]{-90});
    rules[248] = new Rule(-268, new int[]{-90,6,-90});
    rules[249] = new Rule(-268, new int[]{8,-78,9});
    rules[250] = new Rule(-90, new int[]{-103});
    rules[251] = new Rule(-90, new int[]{-90,-189,-103});
    rules[252] = new Rule(-103, new int[]{-104});
    rules[253] = new Rule(-103, new int[]{-103,-191,-104});
    rules[254] = new Rule(-104, new int[]{-176});
    rules[255] = new Rule(-104, new int[]{-16});
    rules[256] = new Rule(-104, new int[]{-195,-104});
    rules[257] = new Rule(-104, new int[]{-161});
    rules[258] = new Rule(-104, new int[]{-104,8,-73,9});
    rules[259] = new Rule(-176, new int[]{-143});
    rules[260] = new Rule(-176, new int[]{-176,7,-134});
    rules[261] = new Rule(-78, new int[]{-76,98,-76});
    rules[262] = new Rule(-78, new int[]{-78,98,-76});
    rules[263] = new Rule(-76, new int[]{-272});
    rules[264] = new Rule(-76, new int[]{-272,118,-85});
    rules[265] = new Rule(-245, new int[]{140,-271});
    rules[266] = new Rule(-277, new int[]{-278});
    rules[267] = new Rule(-277, new int[]{63,-278});
    rules[268] = new Rule(-278, new int[]{-274});
    rules[269] = new Rule(-278, new int[]{-31});
    rules[270] = new Rule(-278, new int[]{-259});
    rules[271] = new Rule(-278, new int[]{-126});
    rules[272] = new Rule(-278, new int[]{-127});
    rules[273] = new Rule(-127, new int[]{72,56,-272});
    rules[274] = new Rule(-274, new int[]{22,11,-160,12,56,-272});
    rules[275] = new Rule(-274, new int[]{-266});
    rules[276] = new Rule(-266, new int[]{22,56,-272});
    rules[277] = new Rule(-160, new int[]{-267});
    rules[278] = new Rule(-160, new int[]{-160,98,-267});
    rules[279] = new Rule(-267, new int[]{-268});
    rules[280] = new Rule(-267, new int[]{});
    rules[281] = new Rule(-259, new int[]{47,56,-272});
    rules[282] = new Rule(-126, new int[]{32,56,-272});
    rules[283] = new Rule(-126, new int[]{32});
    rules[284] = new Rule(-252, new int[]{141,11,-87,12});
    rules[285] = new Rule(-222, new int[]{-220});
    rules[286] = new Rule(-220, new int[]{-219});
    rules[287] = new Rule(-219, new int[]{42,-124});
    rules[288] = new Rule(-219, new int[]{35,-124,5,-271});
    rules[289] = new Rule(-219, new int[]{-176,125,-275});
    rules[290] = new Rule(-219, new int[]{-298,125,-275});
    rules[291] = new Rule(-219, new int[]{8,9,125,-275});
    rules[292] = new Rule(-219, new int[]{8,-78,9,125,-275});
    rules[293] = new Rule(-219, new int[]{-176,125,8,9});
    rules[294] = new Rule(-219, new int[]{-298,125,8,9});
    rules[295] = new Rule(-219, new int[]{8,9,125,8,9});
    rules[296] = new Rule(-219, new int[]{8,-78,9,125,8,9});
    rules[297] = new Rule(-30, new int[]{-21,-288,-179,-315,-26});
    rules[298] = new Rule(-31, new int[]{46,-179,-315,-25,90});
    rules[299] = new Rule(-20, new int[]{67});
    rules[300] = new Rule(-20, new int[]{68});
    rules[301] = new Rule(-20, new int[]{145});
    rules[302] = new Rule(-20, new int[]{25});
    rules[303] = new Rule(-20, new int[]{26});
    rules[304] = new Rule(-21, new int[]{});
    rules[305] = new Rule(-21, new int[]{-22});
    rules[306] = new Rule(-22, new int[]{-20});
    rules[307] = new Rule(-22, new int[]{-22,-20});
    rules[308] = new Rule(-288, new int[]{24});
    rules[309] = new Rule(-288, new int[]{41});
    rules[310] = new Rule(-288, new int[]{62});
    rules[311] = new Rule(-288, new int[]{62,24});
    rules[312] = new Rule(-288, new int[]{62,46});
    rules[313] = new Rule(-288, new int[]{62,41});
    rules[314] = new Rule(-26, new int[]{});
    rules[315] = new Rule(-26, new int[]{-25,90});
    rules[316] = new Rule(-179, new int[]{});
    rules[317] = new Rule(-179, new int[]{8,-178,9});
    rules[318] = new Rule(-178, new int[]{-177});
    rules[319] = new Rule(-178, new int[]{-178,98,-177});
    rules[320] = new Rule(-177, new int[]{-176});
    rules[321] = new Rule(-177, new int[]{-298});
    rules[322] = new Rule(-151, new int[]{121,-154,119});
    rules[323] = new Rule(-315, new int[]{});
    rules[324] = new Rule(-315, new int[]{-314});
    rules[325] = new Rule(-314, new int[]{-313});
    rules[326] = new Rule(-314, new int[]{-314,-313});
    rules[327] = new Rule(-313, new int[]{21,-154,5,-285,10});
    rules[328] = new Rule(-285, new int[]{-282});
    rules[329] = new Rule(-285, new int[]{-285,98,-282});
    rules[330] = new Rule(-282, new int[]{-272});
    rules[331] = new Rule(-282, new int[]{24});
    rules[332] = new Rule(-282, new int[]{46});
    rules[333] = new Rule(-282, new int[]{28});
    rules[334] = new Rule(-25, new int[]{-32});
    rules[335] = new Rule(-25, new int[]{-25,-7,-32});
    rules[336] = new Rule(-7, new int[]{83});
    rules[337] = new Rule(-7, new int[]{82});
    rules[338] = new Rule(-7, new int[]{81});
    rules[339] = new Rule(-7, new int[]{80});
    rules[340] = new Rule(-32, new int[]{});
    rules[341] = new Rule(-32, new int[]{-34,-186});
    rules[342] = new Rule(-32, new int[]{-33});
    rules[343] = new Rule(-32, new int[]{-34,10,-33});
    rules[344] = new Rule(-154, new int[]{-143});
    rules[345] = new Rule(-154, new int[]{-154,98,-143});
    rules[346] = new Rule(-186, new int[]{});
    rules[347] = new Rule(-186, new int[]{10});
    rules[348] = new Rule(-34, new int[]{-45});
    rules[349] = new Rule(-34, new int[]{-34,10,-45});
    rules[350] = new Rule(-45, new int[]{-6,-51});
    rules[351] = new Rule(-33, new int[]{-54});
    rules[352] = new Rule(-33, new int[]{-33,-54});
    rules[353] = new Rule(-54, new int[]{-53});
    rules[354] = new Rule(-54, new int[]{-55});
    rules[355] = new Rule(-51, new int[]{27,-28});
    rules[356] = new Rule(-51, new int[]{-310});
    rules[357] = new Rule(-51, new int[]{-3,-310});
    rules[358] = new Rule(-3, new int[]{26});
    rules[359] = new Rule(-3, new int[]{24});
    rules[360] = new Rule(-310, new int[]{-309});
    rules[361] = new Rule(-310, new int[]{60,-154,5,-272});
    rules[362] = new Rule(-53, new int[]{-6,-218});
    rules[363] = new Rule(-53, new int[]{-6,-215});
    rules[364] = new Rule(-215, new int[]{-211});
    rules[365] = new Rule(-215, new int[]{-214});
    rules[366] = new Rule(-218, new int[]{-3,-226});
    rules[367] = new Rule(-218, new int[]{-226});
    rules[368] = new Rule(-218, new int[]{-223});
    rules[369] = new Rule(-226, new int[]{-224});
    rules[370] = new Rule(-224, new int[]{-221});
    rules[371] = new Rule(-224, new int[]{-225});
    rules[372] = new Rule(-223, new int[]{28,-168,-124,-203});
    rules[373] = new Rule(-223, new int[]{-3,28,-168,-124,-203});
    rules[374] = new Rule(-223, new int[]{29,-168,-124,-203});
    rules[375] = new Rule(-168, new int[]{-167});
    rules[376] = new Rule(-168, new int[]{});
    rules[377] = new Rule(-55, new int[]{-6,-254});
    rules[378] = new Rule(-254, new int[]{44,-166,-229,-198,10,-201});
    rules[379] = new Rule(-254, new int[]{44,-166,-229,-198,10,-206,10,-201});
    rules[380] = new Rule(-254, new int[]{-3,44,-166,-229,-198,10,-201});
    rules[381] = new Rule(-254, new int[]{-3,44,-166,-229,-198,10,-206,10,-201});
    rules[382] = new Rule(-254, new int[]{25,44,-166,-229,-207,10});
    rules[383] = new Rule(-254, new int[]{-3,25,44,-166,-229,-207,10});
    rules[384] = new Rule(-207, new int[]{108,-85});
    rules[385] = new Rule(-207, new int[]{});
    rules[386] = new Rule(-201, new int[]{});
    rules[387] = new Rule(-201, new int[]{61,10});
    rules[388] = new Rule(-229, new int[]{-234,5,-271});
    rules[389] = new Rule(-234, new int[]{});
    rules[390] = new Rule(-234, new int[]{11,-233,12});
    rules[391] = new Rule(-233, new int[]{-232});
    rules[392] = new Rule(-233, new int[]{-233,10,-232});
    rules[393] = new Rule(-232, new int[]{-154,5,-271});
    rules[394] = new Rule(-110, new int[]{-86});
    rules[395] = new Rule(-110, new int[]{});
    rules[396] = new Rule(-198, new int[]{});
    rules[397] = new Rule(-198, new int[]{84,-110,-199});
    rules[398] = new Rule(-198, new int[]{85,-256,-200});
    rules[399] = new Rule(-199, new int[]{});
    rules[400] = new Rule(-199, new int[]{85,-256});
    rules[401] = new Rule(-200, new int[]{});
    rules[402] = new Rule(-200, new int[]{84,-110});
    rules[403] = new Rule(-308, new int[]{-309,10});
    rules[404] = new Rule(-336, new int[]{108});
    rules[405] = new Rule(-336, new int[]{118});
    rules[406] = new Rule(-309, new int[]{-154,5,-272});
    rules[407] = new Rule(-309, new int[]{-154,108,-86});
    rules[408] = new Rule(-309, new int[]{-154,5,-272,-336,-84});
    rules[409] = new Rule(-84, new int[]{-83});
    rules[410] = new Rule(-84, new int[]{-79,6,-13});
    rules[411] = new Rule(-84, new int[]{-321});
    rules[412] = new Rule(-84, new int[]{-143,125,-326});
    rules[413] = new Rule(-84, new int[]{8,9,-322,125,-326});
    rules[414] = new Rule(-84, new int[]{8,-66,9,125,-326});
    rules[415] = new Rule(-83, new int[]{-82});
    rules[416] = new Rule(-213, new int[]{-223,-173});
    rules[417] = new Rule(-213, new int[]{28,-168,-124,108,-256,10});
    rules[418] = new Rule(-213, new int[]{-3,28,-168,-124,108,-256,10});
    rules[419] = new Rule(-214, new int[]{-223,-172});
    rules[420] = new Rule(-214, new int[]{28,-168,-124,108,-256,10});
    rules[421] = new Rule(-214, new int[]{-3,28,-168,-124,108,-256,10});
    rules[422] = new Rule(-210, new int[]{-217});
    rules[423] = new Rule(-210, new int[]{-3,-217});
    rules[424] = new Rule(-217, new int[]{-224,-174});
    rules[425] = new Rule(-217, new int[]{35,-166,-124,5,-271,-204,108,-98,10});
    rules[426] = new Rule(-217, new int[]{35,-166,-124,-204,108,-98,10});
    rules[427] = new Rule(-217, new int[]{35,-166,-124,5,-271,-204,108,-320,10});
    rules[428] = new Rule(-217, new int[]{35,-166,-124,-204,108,-320,10});
    rules[429] = new Rule(-217, new int[]{42,-167,-124,-204,108,-256,10});
    rules[430] = new Rule(-217, new int[]{-224,146,10});
    rules[431] = new Rule(-211, new int[]{-212});
    rules[432] = new Rule(-211, new int[]{-3,-212});
    rules[433] = new Rule(-212, new int[]{-224,-172});
    rules[434] = new Rule(-212, new int[]{35,-166,-124,5,-271,-204,108,-100,10});
    rules[435] = new Rule(-212, new int[]{35,-166,-124,-204,108,-100,10});
    rules[436] = new Rule(-212, new int[]{42,-167,-124,-204,108,-256,10});
    rules[437] = new Rule(-174, new int[]{-173});
    rules[438] = new Rule(-174, new int[]{-61});
    rules[439] = new Rule(-167, new int[]{-166});
    rules[440] = new Rule(-166, new int[]{-138});
    rules[441] = new Rule(-166, new int[]{-332,7,-138});
    rules[442] = new Rule(-145, new int[]{-133});
    rules[443] = new Rule(-332, new int[]{-145});
    rules[444] = new Rule(-332, new int[]{-332,7,-145});
    rules[445] = new Rule(-138, new int[]{-133});
    rules[446] = new Rule(-138, new int[]{-187});
    rules[447] = new Rule(-138, new int[]{-187,-151});
    rules[448] = new Rule(-133, new int[]{-130});
    rules[449] = new Rule(-133, new int[]{-130,-151});
    rules[450] = new Rule(-130, new int[]{-143});
    rules[451] = new Rule(-221, new int[]{42,-167,-124,-203,-315});
    rules[452] = new Rule(-225, new int[]{35,-166,-124,-203,-315});
    rules[453] = new Rule(-225, new int[]{35,-166,-124,5,-271,-203,-315});
    rules[454] = new Rule(-61, new int[]{105,-105,79,-105,10});
    rules[455] = new Rule(-61, new int[]{105,-105,10});
    rules[456] = new Rule(-61, new int[]{105,10});
    rules[457] = new Rule(-105, new int[]{-143});
    rules[458] = new Rule(-105, new int[]{-161});
    rules[459] = new Rule(-173, new int[]{-41,-251,10});
    rules[460] = new Rule(-172, new int[]{-43,-251,10});
    rules[461] = new Rule(-172, new int[]{-61});
    rules[462] = new Rule(-124, new int[]{});
    rules[463] = new Rule(-124, new int[]{8,9});
    rules[464] = new Rule(-124, new int[]{8,-125,9});
    rules[465] = new Rule(-125, new int[]{-56});
    rules[466] = new Rule(-125, new int[]{-125,10,-56});
    rules[467] = new Rule(-56, new int[]{-6,-293});
    rules[468] = new Rule(-293, new int[]{-155,5,-271});
    rules[469] = new Rule(-293, new int[]{51,-155,5,-271});
    rules[470] = new Rule(-293, new int[]{27,-155,5,-271});
    rules[471] = new Rule(-293, new int[]{106,-155,5,-271});
    rules[472] = new Rule(-293, new int[]{-155,5,-271,108,-85});
    rules[473] = new Rule(-293, new int[]{51,-155,5,-271,108,-85});
    rules[474] = new Rule(-293, new int[]{27,-155,5,-271,108,-85});
    rules[475] = new Rule(-155, new int[]{-131});
    rules[476] = new Rule(-155, new int[]{-155,98,-131});
    rules[477] = new Rule(-131, new int[]{-143});
    rules[478] = new Rule(-271, new int[]{-272});
    rules[479] = new Rule(-273, new int[]{-268});
    rules[480] = new Rule(-273, new int[]{-252});
    rules[481] = new Rule(-273, new int[]{-245});
    rules[482] = new Rule(-273, new int[]{-277});
    rules[483] = new Rule(-273, new int[]{-298});
    rules[484] = new Rule(-257, new int[]{-256});
    rules[485] = new Rule(-257, new int[]{-139,5,-257});
    rules[486] = new Rule(-256, new int[]{});
    rules[487] = new Rule(-256, new int[]{-4});
    rules[488] = new Rule(-256, new int[]{-208});
    rules[489] = new Rule(-256, new int[]{-129});
    rules[490] = new Rule(-256, new int[]{-251});
    rules[491] = new Rule(-256, new int[]{-149});
    rules[492] = new Rule(-256, new int[]{-35});
    rules[493] = new Rule(-256, new int[]{-243});
    rules[494] = new Rule(-256, new int[]{-316});
    rules[495] = new Rule(-256, new int[]{-120});
    rules[496] = new Rule(-256, new int[]{-317});
    rules[497] = new Rule(-256, new int[]{-156});
    rules[498] = new Rule(-256, new int[]{-299});
    rules[499] = new Rule(-256, new int[]{-244});
    rules[500] = new Rule(-256, new int[]{-119});
    rules[501] = new Rule(-256, new int[]{-312});
    rules[502] = new Rule(-256, new int[]{-59});
    rules[503] = new Rule(-256, new int[]{-165});
    rules[504] = new Rule(-256, new int[]{-122});
    rules[505] = new Rule(-256, new int[]{-123});
    rules[506] = new Rule(-256, new int[]{-121});
    rules[507] = new Rule(-256, new int[]{-346});
    rules[508] = new Rule(-121, new int[]{71,-98,97,-256});
    rules[509] = new Rule(-122, new int[]{73,-100});
    rules[510] = new Rule(-123, new int[]{73,72,-100});
    rules[511] = new Rule(-312, new int[]{51,-309});
    rules[512] = new Rule(-312, new int[]{8,51,-143,98,-335,9,108,-85});
    rules[513] = new Rule(-312, new int[]{51,8,-143,98,-154,9,108,-85});
    rules[514] = new Rule(-4, new int[]{-109,-190,-86});
    rules[515] = new Rule(-4, new int[]{8,-108,98,-334,9,-190,-85});
    rules[516] = new Rule(-334, new int[]{-108});
    rules[517] = new Rule(-334, new int[]{-334,98,-108});
    rules[518] = new Rule(-335, new int[]{51,-143});
    rules[519] = new Rule(-335, new int[]{-335,98,51,-143});
    rules[520] = new Rule(-208, new int[]{-109});
    rules[521] = new Rule(-129, new int[]{55,-139});
    rules[522] = new Rule(-251, new int[]{89,-248,90});
    rules[523] = new Rule(-248, new int[]{-257});
    rules[524] = new Rule(-248, new int[]{-248,10,-257});
    rules[525] = new Rule(-149, new int[]{38,-98,49,-256});
    rules[526] = new Rule(-149, new int[]{38,-98,49,-256,30,-256});
    rules[527] = new Rule(-346, new int[]{36,-98,53,-348,-249,90});
    rules[528] = new Rule(-346, new int[]{36,-98,53,-348,10,-249,90});
    rules[529] = new Rule(-348, new int[]{-347});
    rules[530] = new Rule(-348, new int[]{-348,10,-347});
    rules[531] = new Rule(-347, new int[]{-340,37,-98,5,-256});
    rules[532] = new Rule(-347, new int[]{-339,5,-256});
    rules[533] = new Rule(-347, new int[]{-341,5,-256});
    rules[534] = new Rule(-347, new int[]{-342,37,-98,5,-256});
    rules[535] = new Rule(-347, new int[]{-342,5,-256});
    rules[536] = new Rule(-35, new int[]{23,-98,56,-36,-249,90});
    rules[537] = new Rule(-35, new int[]{23,-98,56,-36,10,-249,90});
    rules[538] = new Rule(-35, new int[]{23,-98,56,-249,90});
    rules[539] = new Rule(-36, new int[]{-258});
    rules[540] = new Rule(-36, new int[]{-36,10,-258});
    rules[541] = new Rule(-258, new int[]{-72,5,-256});
    rules[542] = new Rule(-72, new int[]{-107});
    rules[543] = new Rule(-72, new int[]{-72,98,-107});
    rules[544] = new Rule(-107, new int[]{-91});
    rules[545] = new Rule(-249, new int[]{});
    rules[546] = new Rule(-249, new int[]{30,-248});
    rules[547] = new Rule(-243, new int[]{95,-248,96,-85});
    rules[548] = new Rule(-316, new int[]{52,-98,-289,-256});
    rules[549] = new Rule(-289, new int[]{97});
    rules[550] = new Rule(-289, new int[]{});
    rules[551] = new Rule(-165, new int[]{58,-98,97,-256});
    rules[552] = new Rule(-119, new int[]{34,-143,-270,135,-98,97,-256});
    rules[553] = new Rule(-119, new int[]{34,51,-143,5,-272,135,-98,97,-256});
    rules[554] = new Rule(-119, new int[]{34,51,-143,135,-98,97,-256});
    rules[555] = new Rule(-119, new int[]{34,51,8,-154,9,135,-98,97,-256});
    rules[556] = new Rule(-270, new int[]{5,-272});
    rules[557] = new Rule(-270, new int[]{});
    rules[558] = new Rule(-120, new int[]{33,-19,-143,-283,-98,-115,-98,-289,-256});
    rules[559] = new Rule(-120, new int[]{33,-19,-143,-283,-98,-115,-98,157,-98,-289,-256});
    rules[560] = new Rule(-19, new int[]{51});
    rules[561] = new Rule(-19, new int[]{});
    rules[562] = new Rule(-283, new int[]{108});
    rules[563] = new Rule(-283, new int[]{5,-176,108});
    rules[564] = new Rule(-115, new int[]{69});
    rules[565] = new Rule(-115, new int[]{70});
    rules[566] = new Rule(-317, new int[]{53,-70,97,-256});
    rules[567] = new Rule(-156, new int[]{40});
    rules[568] = new Rule(-299, new int[]{100,-248,-287});
    rules[569] = new Rule(-287, new int[]{99,-248,90});
    rules[570] = new Rule(-287, new int[]{31,-60,90});
    rules[571] = new Rule(-60, new int[]{-63,-250});
    rules[572] = new Rule(-60, new int[]{-63,10,-250});
    rules[573] = new Rule(-60, new int[]{-248});
    rules[574] = new Rule(-63, new int[]{-62});
    rules[575] = new Rule(-63, new int[]{-63,10,-62});
    rules[576] = new Rule(-250, new int[]{});
    rules[577] = new Rule(-250, new int[]{30,-248});
    rules[578] = new Rule(-62, new int[]{78,-64,97,-256});
    rules[579] = new Rule(-64, new int[]{-175});
    rules[580] = new Rule(-64, new int[]{-136,5,-175});
    rules[581] = new Rule(-175, new int[]{-176});
    rules[582] = new Rule(-136, new int[]{-143});
    rules[583] = new Rule(-244, new int[]{45});
    rules[584] = new Rule(-244, new int[]{45,-85});
    rules[585] = new Rule(-70, new int[]{-86});
    rules[586] = new Rule(-70, new int[]{-70,98,-86});
    rules[587] = new Rule(-59, new int[]{-170});
    rules[588] = new Rule(-170, new int[]{-169});
    rules[589] = new Rule(-86, new int[]{-85});
    rules[590] = new Rule(-86, new int[]{-320});
    rules[591] = new Rule(-86, new int[]{40});
    rules[592] = new Rule(-85, new int[]{-98});
    rules[593] = new Rule(-85, new int[]{-116});
    rules[594] = new Rule(-98, new int[]{-96});
    rules[595] = new Rule(-98, new int[]{-236});
    rules[596] = new Rule(-98, new int[]{-238});
    rules[597] = new Rule(-113, new int[]{-96});
    rules[598] = new Rule(-113, new int[]{-236});
    rules[599] = new Rule(-114, new int[]{-96});
    rules[600] = new Rule(-114, new int[]{-238});
    rules[601] = new Rule(-100, new int[]{-98});
    rules[602] = new Rule(-100, new int[]{-320});
    rules[603] = new Rule(-101, new int[]{-96});
    rules[604] = new Rule(-101, new int[]{-236});
    rules[605] = new Rule(-101, new int[]{-320});
    rules[606] = new Rule(-96, new int[]{-95});
    rules[607] = new Rule(-96, new int[]{-96,16,-95});
    rules[608] = new Rule(-253, new int[]{19,8,-280,9});
    rules[609] = new Rule(-292, new int[]{20,8,-280,9});
    rules[610] = new Rule(-292, new int[]{20,8,-279,9});
    rules[611] = new Rule(-236, new int[]{-113,13,-113,5,-113});
    rules[612] = new Rule(-238, new int[]{38,-114,49,-114,30,-114});
    rules[613] = new Rule(-279, new int[]{-176,-297});
    rules[614] = new Rule(-279, new int[]{-176,4,-297});
    rules[615] = new Rule(-280, new int[]{-176});
    rules[616] = new Rule(-280, new int[]{-176,-296});
    rules[617] = new Rule(-280, new int[]{-176,4,-296});
    rules[618] = new Rule(-281, new int[]{-280});
    rules[619] = new Rule(-281, new int[]{-269});
    rules[620] = new Rule(-5, new int[]{8,-66,9});
    rules[621] = new Rule(-5, new int[]{});
    rules[622] = new Rule(-169, new int[]{77,-280,-69});
    rules[623] = new Rule(-169, new int[]{77,-280,11,-67,12,-5});
    rules[624] = new Rule(-169, new int[]{77,24,8,-331,9});
    rules[625] = new Rule(-330, new int[]{-143,108,-98});
    rules[626] = new Rule(-330, new int[]{-98});
    rules[627] = new Rule(-331, new int[]{-330});
    rules[628] = new Rule(-331, new int[]{-331,98,-330});
    rules[629] = new Rule(-69, new int[]{});
    rules[630] = new Rule(-69, new int[]{8,-67,9});
    rules[631] = new Rule(-95, new int[]{-102});
    rules[632] = new Rule(-95, new int[]{-95,-192,-102});
    rules[633] = new Rule(-95, new int[]{-95,-192,-238});
    rules[634] = new Rule(-95, new int[]{-262,8,-351,9});
    rules[635] = new Rule(-338, new int[]{-280,8,-351,9});
    rules[636] = new Rule(-340, new int[]{-280,8,-352,9});
    rules[637] = new Rule(-339, new int[]{-280,8,-352,9});
    rules[638] = new Rule(-339, new int[]{-355});
    rules[639] = new Rule(-355, new int[]{-337});
    rules[640] = new Rule(-355, new int[]{-355,98,-337});
    rules[641] = new Rule(-337, new int[]{-15});
    rules[642] = new Rule(-337, new int[]{-280});
    rules[643] = new Rule(-337, new int[]{54});
    rules[644] = new Rule(-337, new int[]{-253});
    rules[645] = new Rule(-337, new int[]{-292});
    rules[646] = new Rule(-341, new int[]{11,-353,12});
    rules[647] = new Rule(-353, new int[]{-343});
    rules[648] = new Rule(-353, new int[]{-353,98,-343});
    rules[649] = new Rule(-343, new int[]{-15});
    rules[650] = new Rule(-343, new int[]{-345});
    rules[651] = new Rule(-343, new int[]{14});
    rules[652] = new Rule(-343, new int[]{-340});
    rules[653] = new Rule(-343, new int[]{-341});
    rules[654] = new Rule(-343, new int[]{-342});
    rules[655] = new Rule(-343, new int[]{6});
    rules[656] = new Rule(-345, new int[]{51,-143});
    rules[657] = new Rule(-342, new int[]{8,-354,9});
    rules[658] = new Rule(-344, new int[]{14});
    rules[659] = new Rule(-344, new int[]{-15});
    rules[660] = new Rule(-344, new int[]{-195,-15});
    rules[661] = new Rule(-344, new int[]{51,-143});
    rules[662] = new Rule(-344, new int[]{-340});
    rules[663] = new Rule(-344, new int[]{-341});
    rules[664] = new Rule(-344, new int[]{-342});
    rules[665] = new Rule(-354, new int[]{-344});
    rules[666] = new Rule(-354, new int[]{-354,98,-344});
    rules[667] = new Rule(-352, new int[]{-350});
    rules[668] = new Rule(-352, new int[]{-352,10,-350});
    rules[669] = new Rule(-352, new int[]{-352,98,-350});
    rules[670] = new Rule(-351, new int[]{-349});
    rules[671] = new Rule(-351, new int[]{-351,10,-349});
    rules[672] = new Rule(-351, new int[]{-351,98,-349});
    rules[673] = new Rule(-349, new int[]{14});
    rules[674] = new Rule(-349, new int[]{-15});
    rules[675] = new Rule(-349, new int[]{51,-143,5,-272});
    rules[676] = new Rule(-349, new int[]{51,-143});
    rules[677] = new Rule(-349, new int[]{-338});
    rules[678] = new Rule(-349, new int[]{-341});
    rules[679] = new Rule(-349, new int[]{-342});
    rules[680] = new Rule(-350, new int[]{14});
    rules[681] = new Rule(-350, new int[]{-15});
    rules[682] = new Rule(-350, new int[]{-195,-15});
    rules[683] = new Rule(-350, new int[]{-143,5,-272});
    rules[684] = new Rule(-350, new int[]{-143});
    rules[685] = new Rule(-350, new int[]{51,-143,5,-272});
    rules[686] = new Rule(-350, new int[]{51,-143});
    rules[687] = new Rule(-350, new int[]{-340});
    rules[688] = new Rule(-350, new int[]{-341});
    rules[689] = new Rule(-350, new int[]{-342});
    rules[690] = new Rule(-111, new int[]{-102});
    rules[691] = new Rule(-111, new int[]{});
    rules[692] = new Rule(-118, new int[]{-87});
    rules[693] = new Rule(-118, new int[]{});
    rules[694] = new Rule(-116, new int[]{-102,5,-111});
    rules[695] = new Rule(-116, new int[]{5,-111});
    rules[696] = new Rule(-116, new int[]{-102,5,-111,5,-102});
    rules[697] = new Rule(-116, new int[]{5,-111,5,-102});
    rules[698] = new Rule(-117, new int[]{-87,5,-118});
    rules[699] = new Rule(-117, new int[]{5,-118});
    rules[700] = new Rule(-117, new int[]{-87,5,-118,5,-87});
    rules[701] = new Rule(-117, new int[]{5,-118,5,-87});
    rules[702] = new Rule(-192, new int[]{118});
    rules[703] = new Rule(-192, new int[]{123});
    rules[704] = new Rule(-192, new int[]{121});
    rules[705] = new Rule(-192, new int[]{119});
    rules[706] = new Rule(-192, new int[]{122});
    rules[707] = new Rule(-192, new int[]{120});
    rules[708] = new Rule(-192, new int[]{135});
    rules[709] = new Rule(-192, new int[]{133,135});
    rules[710] = new Rule(-102, new int[]{-81});
    rules[711] = new Rule(-102, new int[]{-102,6,-81});
    rules[712] = new Rule(-81, new int[]{-80});
    rules[713] = new Rule(-81, new int[]{-81,-193,-80});
    rules[714] = new Rule(-81, new int[]{-81,-193,-238});
    rules[715] = new Rule(-193, new int[]{114});
    rules[716] = new Rule(-193, new int[]{113});
    rules[717] = new Rule(-193, new int[]{126});
    rules[718] = new Rule(-193, new int[]{127});
    rules[719] = new Rule(-193, new int[]{124});
    rules[720] = new Rule(-197, new int[]{134});
    rules[721] = new Rule(-197, new int[]{136});
    rules[722] = new Rule(-260, new int[]{-262});
    rules[723] = new Rule(-260, new int[]{-263});
    rules[724] = new Rule(-263, new int[]{-80,134,-280});
    rules[725] = new Rule(-263, new int[]{-80,134,-274});
    rules[726] = new Rule(-262, new int[]{-80,136,-280});
    rules[727] = new Rule(-262, new int[]{-80,136,-274});
    rules[728] = new Rule(-264, new int[]{-94,117,-93});
    rules[729] = new Rule(-264, new int[]{-94,117,-264});
    rules[730] = new Rule(-264, new int[]{-195,-264});
    rules[731] = new Rule(-80, new int[]{-93});
    rules[732] = new Rule(-80, new int[]{-169});
    rules[733] = new Rule(-80, new int[]{-264});
    rules[734] = new Rule(-80, new int[]{-80,-194,-93});
    rules[735] = new Rule(-80, new int[]{-80,-194,-264});
    rules[736] = new Rule(-80, new int[]{-80,-194,-238});
    rules[737] = new Rule(-80, new int[]{-260});
    rules[738] = new Rule(-194, new int[]{116});
    rules[739] = new Rule(-194, new int[]{115});
    rules[740] = new Rule(-194, new int[]{129});
    rules[741] = new Rule(-194, new int[]{130});
    rules[742] = new Rule(-194, new int[]{131});
    rules[743] = new Rule(-194, new int[]{132});
    rules[744] = new Rule(-194, new int[]{128});
    rules[745] = new Rule(-57, new int[]{61,8,-281,9});
    rules[746] = new Rule(-58, new int[]{8,-99,98,-77,-322,-329,9});
    rules[747] = new Rule(-94, new int[]{-15});
    rules[748] = new Rule(-94, new int[]{-109});
    rules[749] = new Rule(-93, new int[]{54});
    rules[750] = new Rule(-93, new int[]{-15});
    rules[751] = new Rule(-93, new int[]{-57});
    rules[752] = new Rule(-93, new int[]{11,-68,12});
    rules[753] = new Rule(-93, new int[]{133,-93});
    rules[754] = new Rule(-93, new int[]{-195,-93});
    rules[755] = new Rule(-93, new int[]{140,-93});
    rules[756] = new Rule(-93, new int[]{-109});
    rules[757] = new Rule(-93, new int[]{-58});
    rules[758] = new Rule(-15, new int[]{-161});
    rules[759] = new Rule(-15, new int[]{-16});
    rules[760] = new Rule(-112, new int[]{-108,15,-108});
    rules[761] = new Rule(-112, new int[]{-108,15,-112});
    rules[762] = new Rule(-109, new int[]{-128,-108});
    rules[763] = new Rule(-109, new int[]{-108});
    rules[764] = new Rule(-109, new int[]{-112});
    rules[765] = new Rule(-128, new int[]{139});
    rules[766] = new Rule(-128, new int[]{-128,139});
    rules[767] = new Rule(-9, new int[]{-176,-69});
    rules[768] = new Rule(-9, new int[]{-298,-69});
    rules[769] = new Rule(-319, new int[]{-143});
    rules[770] = new Rule(-319, new int[]{-319,7,-134});
    rules[771] = new Rule(-318, new int[]{-319});
    rules[772] = new Rule(-318, new int[]{-319,-296});
    rules[773] = new Rule(-17, new int[]{-108});
    rules[774] = new Rule(-17, new int[]{-15});
    rules[775] = new Rule(-356, new int[]{51,-143,108,-85,10});
    rules[776] = new Rule(-357, new int[]{-356});
    rules[777] = new Rule(-357, new int[]{-357,-356});
    rules[778] = new Rule(-108, new int[]{-143});
    rules[779] = new Rule(-108, new int[]{-187});
    rules[780] = new Rule(-108, new int[]{40,-143});
    rules[781] = new Rule(-108, new int[]{8,-85,9});
    rules[782] = new Rule(-108, new int[]{8,-357,-85,9});
    rules[783] = new Rule(-108, new int[]{-253});
    rules[784] = new Rule(-108, new int[]{-292});
    rules[785] = new Rule(-108, new int[]{-15,7,-134});
    rules[786] = new Rule(-108, new int[]{-17,11,-70,12});
    rules[787] = new Rule(-108, new int[]{-17,17,-116,12});
    rules[788] = new Rule(-108, new int[]{75,-68,75});
    rules[789] = new Rule(-108, new int[]{-108,8,-67,9});
    rules[790] = new Rule(-108, new int[]{-108,7,-144});
    rules[791] = new Rule(-108, new int[]{-58,7,-144});
    rules[792] = new Rule(-108, new int[]{-108,140});
    rules[793] = new Rule(-108, new int[]{-108,4,-296});
    rules[794] = new Rule(-67, new int[]{-70});
    rules[795] = new Rule(-67, new int[]{});
    rules[796] = new Rule(-68, new int[]{-75});
    rules[797] = new Rule(-68, new int[]{});
    rules[798] = new Rule(-75, new int[]{-89});
    rules[799] = new Rule(-75, new int[]{-75,98,-89});
    rules[800] = new Rule(-89, new int[]{-85});
    rules[801] = new Rule(-89, new int[]{-85,6,-85});
    rules[802] = new Rule(-162, new int[]{142});
    rules[803] = new Rule(-162, new int[]{144});
    rules[804] = new Rule(-161, new int[]{-163});
    rules[805] = new Rule(-161, new int[]{143});
    rules[806] = new Rule(-163, new int[]{-162});
    rules[807] = new Rule(-163, new int[]{-163,-162});
    rules[808] = new Rule(-187, new int[]{43,-196});
    rules[809] = new Rule(-203, new int[]{10});
    rules[810] = new Rule(-203, new int[]{10,-202,10});
    rules[811] = new Rule(-204, new int[]{});
    rules[812] = new Rule(-204, new int[]{10,-202});
    rules[813] = new Rule(-202, new int[]{-205});
    rules[814] = new Rule(-202, new int[]{-202,10,-205});
    rules[815] = new Rule(-143, new int[]{141});
    rules[816] = new Rule(-143, new int[]{-147});
    rules[817] = new Rule(-143, new int[]{-148});
    rules[818] = new Rule(-143, new int[]{157});
    rules[819] = new Rule(-134, new int[]{-143});
    rules[820] = new Rule(-134, new int[]{-290});
    rules[821] = new Rule(-134, new int[]{-291});
    rules[822] = new Rule(-144, new int[]{-143});
    rules[823] = new Rule(-144, new int[]{-290});
    rules[824] = new Rule(-144, new int[]{-187});
    rules[825] = new Rule(-205, new int[]{145});
    rules[826] = new Rule(-205, new int[]{147});
    rules[827] = new Rule(-205, new int[]{148});
    rules[828] = new Rule(-205, new int[]{149});
    rules[829] = new Rule(-205, new int[]{151});
    rules[830] = new Rule(-205, new int[]{150});
    rules[831] = new Rule(-206, new int[]{150});
    rules[832] = new Rule(-206, new int[]{149});
    rules[833] = new Rule(-206, new int[]{145});
    rules[834] = new Rule(-206, new int[]{148});
    rules[835] = new Rule(-147, new int[]{84});
    rules[836] = new Rule(-147, new int[]{85});
    rules[837] = new Rule(-148, new int[]{79});
    rules[838] = new Rule(-148, new int[]{77});
    rules[839] = new Rule(-146, new int[]{83});
    rules[840] = new Rule(-146, new int[]{82});
    rules[841] = new Rule(-146, new int[]{81});
    rules[842] = new Rule(-146, new int[]{80});
    rules[843] = new Rule(-290, new int[]{-146});
    rules[844] = new Rule(-290, new int[]{67});
    rules[845] = new Rule(-290, new int[]{62});
    rules[846] = new Rule(-290, new int[]{126});
    rules[847] = new Rule(-290, new int[]{20});
    rules[848] = new Rule(-290, new int[]{19});
    rules[849] = new Rule(-290, new int[]{61});
    rules[850] = new Rule(-290, new int[]{21});
    rules[851] = new Rule(-290, new int[]{127});
    rules[852] = new Rule(-290, new int[]{128});
    rules[853] = new Rule(-290, new int[]{129});
    rules[854] = new Rule(-290, new int[]{130});
    rules[855] = new Rule(-290, new int[]{131});
    rules[856] = new Rule(-290, new int[]{132});
    rules[857] = new Rule(-290, new int[]{133});
    rules[858] = new Rule(-290, new int[]{134});
    rules[859] = new Rule(-290, new int[]{135});
    rules[860] = new Rule(-290, new int[]{136});
    rules[861] = new Rule(-290, new int[]{22});
    rules[862] = new Rule(-290, new int[]{72});
    rules[863] = new Rule(-290, new int[]{89});
    rules[864] = new Rule(-290, new int[]{23});
    rules[865] = new Rule(-290, new int[]{24});
    rules[866] = new Rule(-290, new int[]{27});
    rules[867] = new Rule(-290, new int[]{28});
    rules[868] = new Rule(-290, new int[]{29});
    rules[869] = new Rule(-290, new int[]{70});
    rules[870] = new Rule(-290, new int[]{97});
    rules[871] = new Rule(-290, new int[]{30});
    rules[872] = new Rule(-290, new int[]{90});
    rules[873] = new Rule(-290, new int[]{31});
    rules[874] = new Rule(-290, new int[]{32});
    rules[875] = new Rule(-290, new int[]{25});
    rules[876] = new Rule(-290, new int[]{102});
    rules[877] = new Rule(-290, new int[]{99});
    rules[878] = new Rule(-290, new int[]{33});
    rules[879] = new Rule(-290, new int[]{34});
    rules[880] = new Rule(-290, new int[]{35});
    rules[881] = new Rule(-290, new int[]{38});
    rules[882] = new Rule(-290, new int[]{39});
    rules[883] = new Rule(-290, new int[]{40});
    rules[884] = new Rule(-290, new int[]{101});
    rules[885] = new Rule(-290, new int[]{41});
    rules[886] = new Rule(-290, new int[]{42});
    rules[887] = new Rule(-290, new int[]{44});
    rules[888] = new Rule(-290, new int[]{45});
    rules[889] = new Rule(-290, new int[]{46});
    rules[890] = new Rule(-290, new int[]{95});
    rules[891] = new Rule(-290, new int[]{47});
    rules[892] = new Rule(-290, new int[]{100});
    rules[893] = new Rule(-290, new int[]{48});
    rules[894] = new Rule(-290, new int[]{26});
    rules[895] = new Rule(-290, new int[]{49});
    rules[896] = new Rule(-290, new int[]{69});
    rules[897] = new Rule(-290, new int[]{96});
    rules[898] = new Rule(-290, new int[]{50});
    rules[899] = new Rule(-290, new int[]{51});
    rules[900] = new Rule(-290, new int[]{52});
    rules[901] = new Rule(-290, new int[]{53});
    rules[902] = new Rule(-290, new int[]{54});
    rules[903] = new Rule(-290, new int[]{55});
    rules[904] = new Rule(-290, new int[]{56});
    rules[905] = new Rule(-290, new int[]{57});
    rules[906] = new Rule(-290, new int[]{59});
    rules[907] = new Rule(-290, new int[]{103});
    rules[908] = new Rule(-290, new int[]{104});
    rules[909] = new Rule(-290, new int[]{107});
    rules[910] = new Rule(-290, new int[]{105});
    rules[911] = new Rule(-290, new int[]{106});
    rules[912] = new Rule(-290, new int[]{60});
    rules[913] = new Rule(-290, new int[]{73});
    rules[914] = new Rule(-290, new int[]{36});
    rules[915] = new Rule(-290, new int[]{37});
    rules[916] = new Rule(-290, new int[]{68});
    rules[917] = new Rule(-290, new int[]{145});
    rules[918] = new Rule(-290, new int[]{58});
    rules[919] = new Rule(-290, new int[]{137});
    rules[920] = new Rule(-290, new int[]{138});
    rules[921] = new Rule(-290, new int[]{78});
    rules[922] = new Rule(-290, new int[]{150});
    rules[923] = new Rule(-290, new int[]{149});
    rules[924] = new Rule(-290, new int[]{71});
    rules[925] = new Rule(-290, new int[]{151});
    rules[926] = new Rule(-290, new int[]{147});
    rules[927] = new Rule(-290, new int[]{148});
    rules[928] = new Rule(-290, new int[]{146});
    rules[929] = new Rule(-291, new int[]{43});
    rules[930] = new Rule(-196, new int[]{113});
    rules[931] = new Rule(-196, new int[]{114});
    rules[932] = new Rule(-196, new int[]{115});
    rules[933] = new Rule(-196, new int[]{116});
    rules[934] = new Rule(-196, new int[]{118});
    rules[935] = new Rule(-196, new int[]{119});
    rules[936] = new Rule(-196, new int[]{120});
    rules[937] = new Rule(-196, new int[]{121});
    rules[938] = new Rule(-196, new int[]{122});
    rules[939] = new Rule(-196, new int[]{123});
    rules[940] = new Rule(-196, new int[]{126});
    rules[941] = new Rule(-196, new int[]{127});
    rules[942] = new Rule(-196, new int[]{128});
    rules[943] = new Rule(-196, new int[]{129});
    rules[944] = new Rule(-196, new int[]{130});
    rules[945] = new Rule(-196, new int[]{131});
    rules[946] = new Rule(-196, new int[]{132});
    rules[947] = new Rule(-196, new int[]{133});
    rules[948] = new Rule(-196, new int[]{135});
    rules[949] = new Rule(-196, new int[]{137});
    rules[950] = new Rule(-196, new int[]{138});
    rules[951] = new Rule(-196, new int[]{-190});
    rules[952] = new Rule(-196, new int[]{117});
    rules[953] = new Rule(-190, new int[]{108});
    rules[954] = new Rule(-190, new int[]{109});
    rules[955] = new Rule(-190, new int[]{110});
    rules[956] = new Rule(-190, new int[]{111});
    rules[957] = new Rule(-190, new int[]{112});
    rules[958] = new Rule(-97, new int[]{18,-24,98,-23,9});
    rules[959] = new Rule(-23, new int[]{-97});
    rules[960] = new Rule(-23, new int[]{-143});
    rules[961] = new Rule(-24, new int[]{-23});
    rules[962] = new Rule(-24, new int[]{-24,98,-23});
    rules[963] = new Rule(-99, new int[]{-98});
    rules[964] = new Rule(-99, new int[]{-97});
    rules[965] = new Rule(-77, new int[]{-99});
    rules[966] = new Rule(-77, new int[]{-77,98,-99});
    rules[967] = new Rule(-320, new int[]{-143,125,-326});
    rules[968] = new Rule(-320, new int[]{8,9,-323,125,-326});
    rules[969] = new Rule(-320, new int[]{8,-143,5,-271,9,-323,125,-326});
    rules[970] = new Rule(-320, new int[]{8,-143,10,-324,9,-323,125,-326});
    rules[971] = new Rule(-320, new int[]{8,-143,5,-271,10,-324,9,-323,125,-326});
    rules[972] = new Rule(-320, new int[]{8,-99,98,-77,-322,-329,9,-333});
    rules[973] = new Rule(-320, new int[]{-97,-333});
    rules[974] = new Rule(-320, new int[]{-321});
    rules[975] = new Rule(-329, new int[]{});
    rules[976] = new Rule(-329, new int[]{10,-324});
    rules[977] = new Rule(-333, new int[]{-323,125,-326});
    rules[978] = new Rule(-321, new int[]{35,-323,125,-326});
    rules[979] = new Rule(-321, new int[]{35,8,9,-323,125,-326});
    rules[980] = new Rule(-321, new int[]{35,8,-324,9,-323,125,-326});
    rules[981] = new Rule(-321, new int[]{42,125,-327});
    rules[982] = new Rule(-321, new int[]{42,8,9,125,-327});
    rules[983] = new Rule(-321, new int[]{42,8,-324,9,125,-327});
    rules[984] = new Rule(-324, new int[]{-325});
    rules[985] = new Rule(-324, new int[]{-324,10,-325});
    rules[986] = new Rule(-325, new int[]{-154,-322});
    rules[987] = new Rule(-322, new int[]{});
    rules[988] = new Rule(-322, new int[]{5,-271});
    rules[989] = new Rule(-323, new int[]{});
    rules[990] = new Rule(-323, new int[]{5,-273});
    rules[991] = new Rule(-328, new int[]{-251});
    rules[992] = new Rule(-328, new int[]{-149});
    rules[993] = new Rule(-328, new int[]{-316});
    rules[994] = new Rule(-328, new int[]{-243});
    rules[995] = new Rule(-328, new int[]{-120});
    rules[996] = new Rule(-328, new int[]{-119});
    rules[997] = new Rule(-328, new int[]{-121});
    rules[998] = new Rule(-328, new int[]{-35});
    rules[999] = new Rule(-328, new int[]{-299});
    rules[1000] = new Rule(-328, new int[]{-165});
    rules[1001] = new Rule(-328, new int[]{-244});
    rules[1002] = new Rule(-328, new int[]{-122});
    rules[1003] = new Rule(-328, new int[]{8,-4,9});
    rules[1004] = new Rule(-326, new int[]{-101});
    rules[1005] = new Rule(-326, new int[]{-328});
    rules[1006] = new Rule(-327, new int[]{-208});
    rules[1007] = new Rule(-327, new int[]{-4});
    rules[1008] = new Rule(-327, new int[]{-328});
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
			if (parsertools.build_tree_for_formatter)
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
   			if (parsertools.build_tree_for_formatter)
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
		    // ������ ��������� ����� ��������
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
			    // ����� ������� ���������� ��������� � �������������� �������
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
            if (!parsertools.build_tree_for_formatter)
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
            if (parsertools.build_tree_for_formatter)
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
            if (parsertools.build_tree_for_formatter)
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
            if (parsertools.build_tree_for_formatter)
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
      case 261: // enumeration_id_list -> enumeration_id, tkComma, enumeration_id
{ 
			CurrentSemanticValue.stn = new enumerator_list(ValueStack[ValueStack.Depth-3].stn as enumerator, CurrentLocationSpan);
			(CurrentSemanticValue.stn as enumerator_list).Add(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
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
      case 266: // structured_type -> unpacked_structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 267: // structured_type -> tkPacked, unpacked_structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 268: // unpacked_structured_type -> array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 269: // unpacked_structured_type -> record_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 270: // unpacked_structured_type -> set_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 271: // unpacked_structured_type -> file_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 272: // unpacked_structured_type -> sequence_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 273: // sequence_type -> tkSequence, tkOf, type_ref
{
			CurrentSemanticValue.td = new sequence_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 274: // array_type -> tkArray, tkSquareOpen, simple_type_list, tkSquareClose, tkOf, 
                //               type_ref
{ 
			CurrentSemanticValue.td = new array_type(ValueStack[ValueStack.Depth-4].stn as indexers_types, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 275: // array_type -> unsized_array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 276: // unsized_array_type -> tkArray, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new array_type(null, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 277: // simple_type_list -> simple_type_or_
{ 
			CurrentSemanticValue.stn = new indexers_types(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 278: // simple_type_list -> simple_type_list, tkComma, simple_type_or_
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as indexers_types).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 279: // simple_type_or_ -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 280: // simple_type_or_ -> /* empty */
{ CurrentSemanticValue.td = null; }
        break;
      case 281: // set_type -> tkSet, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new set_type_definition(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 282: // file_type -> tkFile, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new file_type(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 283: // file_type -> tkFile
{ 
			CurrentSemanticValue.td = new file_type();  
			CurrentSemanticValue.td.source_context = CurrentLocationSpan;
		}
        break;
      case 284: // string_type -> tkIdentifier, tkSquareOpen, const_expr, tkSquareClose
{ 
			CurrentSemanticValue.td = new string_num_definition(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-4].id, CurrentLocationSpan);
		}
        break;
      case 285: // procedural_type -> procedural_type_kind
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 286: // procedural_type_kind -> proc_type_decl
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 287: // proc_type_decl -> tkProcedure, fp_list
{ 
			CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-1].stn as formal_parameters,null,null,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 288: // proc_type_decl -> tkFunction, fp_list, tkColon, fptype
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, null, null, null, ValueStack[ValueStack.Depth-1].td as type_definition, CurrentLocationSpan);
        }
        break;
      case 289: // proc_type_decl -> simple_type_identifier, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 290: // proc_type_decl -> template_type, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 291: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 292: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-4].stn as enumerator_list,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 293: // proc_type_decl -> simple_type_identifier, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 294: // proc_type_decl -> template_type, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 295: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,null,CurrentLocationSpan);
    	}
        break;
      case 296: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-5].stn as enumerator_list,null,CurrentLocationSpan);
    	}
        break;
      case 297: // object_type -> class_attributes, class_or_interface_keyword, 
                //                optional_base_classes, optional_where_section, 
                //                optional_component_list_seq_end
{ 
            var cd = NewObjectType((class_attribute)ValueStack[ValueStack.Depth-5].ob, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].stn as named_type_reference_list, ValueStack[ValueStack.Depth-2].stn as where_definition_list, ValueStack[ValueStack.Depth-1].stn as class_body_list, CurrentLocationSpan); 
			CurrentSemanticValue.td = cd;
		}
        break;
      case 298: // record_type -> tkRecord, optional_base_classes, optional_where_section, 
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
      case 299: // class_attribute -> tkSealed
{ CurrentSemanticValue.ob = class_attribute.Sealed; }
        break;
      case 300: // class_attribute -> tkPartial
{ CurrentSemanticValue.ob = class_attribute.Partial; }
        break;
      case 301: // class_attribute -> tkAbstract
{ CurrentSemanticValue.ob = class_attribute.Abstract; }
        break;
      case 302: // class_attribute -> tkAuto
{ CurrentSemanticValue.ob = class_attribute.Auto; }
        break;
      case 303: // class_attribute -> tkStatic
{ CurrentSemanticValue.ob = class_attribute.Static; }
        break;
      case 304: // class_attributes -> /* empty */
{ 
			CurrentSemanticValue.ob = class_attribute.None; 
		}
        break;
      case 305: // class_attributes -> class_attributes1
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 306: // class_attributes1 -> class_attribute
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 307: // class_attributes1 -> class_attributes1, class_attribute
{
            if (((class_attribute)ValueStack[ValueStack.Depth-2].ob & (class_attribute)ValueStack[ValueStack.Depth-1].ob) == (class_attribute)ValueStack[ValueStack.Depth-1].ob)
                parsertools.AddErrorFromResource("ATTRIBUTE_REDECLARED",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.ob  = ((class_attribute)ValueStack[ValueStack.Depth-2].ob) | ((class_attribute)ValueStack[ValueStack.Depth-1].ob);
			//$$ = $1;
		}
        break;
      case 308: // class_or_interface_keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 309: // class_or_interface_keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 310: // class_or_interface_keyword -> tkTemplate
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-1].ti);
		}
        break;
      case 311: // class_or_interface_keyword -> tkTemplate, tkClass
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "c", CurrentLocationSpan);
		}
        break;
      case 312: // class_or_interface_keyword -> tkTemplate, tkRecord
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "r", CurrentLocationSpan);
		}
        break;
      case 313: // class_or_interface_keyword -> tkTemplate, tkInterface
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "i", CurrentLocationSpan);
		}
        break;
      case 314: // optional_component_list_seq_end -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 315: // optional_component_list_seq_end -> member_list_section, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 317: // optional_base_classes -> tkRoundOpen, base_classes_names_list, tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 318: // base_classes_names_list -> base_class_name
{ 
			CurrentSemanticValue.stn = new named_type_reference_list(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 319: // base_classes_names_list -> base_classes_names_list, tkComma, base_class_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as named_type_reference_list).Add(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 320: // base_class_name -> simple_type_identifier
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 321: // base_class_name -> template_type
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 322: // template_arguments -> tkLower, ident_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 323: // optional_where_section -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 324: // optional_where_section -> where_part_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 325: // where_part_list -> where_part
{ 
			CurrentSemanticValue.stn = new where_definition_list(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 326: // where_part_list -> where_part_list, where_part
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as where_definition_list).Add(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 327: // where_part -> tkWhere, ident_list, tkColon, type_ref_and_secific_list, 
                //               tkSemiColon
{ 
			CurrentSemanticValue.stn = new where_definition(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-2].stn as where_type_specificator_list, CurrentLocationSpan); 
		}
        break;
      case 328: // type_ref_and_secific_list -> type_ref_or_secific
{ 
			CurrentSemanticValue.stn = new where_type_specificator_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 329: // type_ref_and_secific_list -> type_ref_and_secific_list, tkComma, 
                //                              type_ref_or_secific
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as where_type_specificator_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 330: // type_ref_or_secific -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 331: // type_ref_or_secific -> tkClass
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefClass, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 332: // type_ref_or_secific -> tkRecord
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefValueType, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 333: // type_ref_or_secific -> tkConstructor
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 334: // member_list_section -> member_list
{ 
			CurrentSemanticValue.stn = new class_body_list(ValueStack[ValueStack.Depth-1].stn as class_members, CurrentLocationSpan);
        }
        break;
      case 335: // member_list_section -> member_list_section, ot_visibility_specifier, 
                //                        member_list
{ 
		    (ValueStack[ValueStack.Depth-1].stn as class_members).access_mod = ValueStack[ValueStack.Depth-2].stn as access_modifer_node;
			(ValueStack[ValueStack.Depth-3].stn as class_body_list).Add(ValueStack[ValueStack.Depth-1].stn as class_members,CurrentLocationSpan);
			
			if ((ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks[0].Count == 0)
                (ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks.RemoveAt(0);
			
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 336: // ot_visibility_specifier -> tkInternal
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.internal_modifer, CurrentLocationSpan); }
        break;
      case 337: // ot_visibility_specifier -> tkPublic
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.public_modifer, CurrentLocationSpan); }
        break;
      case 338: // ot_visibility_specifier -> tkProtected
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.protected_modifer, CurrentLocationSpan); }
        break;
      case 339: // ot_visibility_specifier -> tkPrivate
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.private_modifer, CurrentLocationSpan); }
        break;
      case 340: // member_list -> /* empty */
{ CurrentSemanticValue.stn = new class_members(); }
        break;
      case 341: // member_list -> field_or_const_definition_list, optional_semicolon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 342: // member_list -> method_decl_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 343: // member_list -> field_or_const_definition_list, tkSemiColon, method_decl_list
{  
			(ValueStack[ValueStack.Depth-3].stn as class_members).members.AddRange((ValueStack[ValueStack.Depth-1].stn as class_members).members);
			(ValueStack[ValueStack.Depth-3].stn as class_members).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 344: // ident_list -> identifier
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 345: // ident_list -> ident_list, tkComma, identifier
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 346: // optional_semicolon -> /* empty */
{ CurrentSemanticValue.ob = null; }
        break;
      case 347: // optional_semicolon -> tkSemiColon
{ CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 348: // field_or_const_definition_list -> field_or_const_definition
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 349: // field_or_const_definition_list -> field_or_const_definition_list, tkSemiColon, 
                //                                   field_or_const_definition
{   
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 350: // field_or_const_definition -> attribute_declarations, 
                //                              simple_field_or_const_definition
{  
		    (ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 351: // method_decl_list -> method_or_property_decl
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 352: // method_decl_list -> method_decl_list, method_or_property_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 353: // method_or_property_decl -> method_decl_withattr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 354: // method_or_property_decl -> property_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 355: // simple_field_or_const_definition -> tkConst, only_const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 356: // simple_field_or_const_definition -> field_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 357: // simple_field_or_const_definition -> class_or_static, field_definition
{ 
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).var_attr = definition_attribute.Static;
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 358: // class_or_static -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 359: // class_or_static -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 360: // field_definition -> var_decl_part
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 361: // field_definition -> tkEvent, ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, true, CurrentLocationSpan); 
        }
        break;
      case 362: // method_decl_withattr -> attribute_declarations, method_header
{  
			(ValueStack[ValueStack.Depth-1].td as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td;
        }
        break;
      case 363: // method_decl_withattr -> attribute_declarations, method_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
            if (ValueStack[ValueStack.Depth-1].stn is procedure_definition && (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
                (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
     }
        break;
      case 364: // method_decl -> inclass_proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 365: // method_decl -> inclass_constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 366: // method_header -> class_or_static, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 367: // method_header -> method_procfunc_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
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
        	if (ValueStack[ValueStack.Depth-2].ex == null || ValueStack[ValueStack.Depth-2].ex is ident) // ����������� ��������
        	{
        		CurrentSemanticValue.stn = NewPropertySpecifiersRead(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-2].ex as ident, null, null, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan);
        	}
        	else // ����������� ��������
        	{
				var id = NewId("#GetGen", LocationStack[LocationStack.Depth-2]);
                procedure_definition pr = null;
                if (!parsertools.build_tree_for_formatter)
                    pr = CreateAndAddToClassReadFunc(ValueStack[ValueStack.Depth-2].ex, id, LocationStack[LocationStack.Depth-2]);
				CurrentSemanticValue.stn = NewPropertySpecifiersRead(ValueStack[ValueStack.Depth-3].id, id, pr, ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan); // $2 ��������� ��� �������������� 
			}
        }
        break;
      case 398: // property_specifiers -> tkWrite, unlabelled_stmt, read_property_specifiers
{ 
        	if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
        	{
        	
        		CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-3].id, null, null, null, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan);
        	}
        	else if (ValueStack[ValueStack.Depth-2].stn is procedure_call && (ValueStack[ValueStack.Depth-2].stn as procedure_call).is_ident) // ����������� ��������
        	{
        	
        		CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-3].id, (ValueStack[ValueStack.Depth-2].stn as procedure_call).func_name as ident, null, null, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan);  // ������ �������� - � ���������������
        	}
        	else // ����������� ��������
        	{
				var id = NewId("#SetGen", LocationStack[LocationStack.Depth-2]);
                procedure_definition pr = null;
                if (!parsertools.build_tree_for_formatter)
                    pr = CreateAndAddToClassWriteProc(ValueStack[ValueStack.Depth-2].stn as statement,id,LocationStack[LocationStack.Depth-2]);
                if (parsertools.build_tree_for_formatter)
					CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-3].id, id, pr, ValueStack[ValueStack.Depth-2].stn as statement, ValueStack[ValueStack.Depth-1].stn as property_accessors, CurrentLocationSpan); // $2 ��������� ��� ��������������
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
        		CurrentSemanticValue.stn = NewPropertySpecifiersWrite(ValueStack[ValueStack.Depth-2].id, (ValueStack[ValueStack.Depth-1].stn as procedure_call).func_name as ident, null, null, null, CurrentLocationSpan); // ������ �������� - � ���������������
        	}
        	else 
        	{
				var id = NewId("#SetGen", LocationStack[LocationStack.Depth-1]);
                procedure_definition pr = null;
                if (!parsertools.build_tree_for_formatter)
                    pr = CreateAndAddToClassWriteProc(ValueStack[ValueStack.Depth-1].stn as statement,id,LocationStack[LocationStack.Depth-1]);
                if (parsertools.build_tree_for_formatter)
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
                if (!parsertools.build_tree_for_formatter)
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
		if (parsertools.build_tree_for_formatter)
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
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
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
				
			var any = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-4]);	
				
			var formalPars = new formal_parameters(new typed_parameters(idList, any, parametr_kind.none, null, LocationStack[LocationStack.Depth-4]), LocationStack[LocationStack.Depth-4]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, any, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 415: // typed_const_plus -> typed_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 416: // constr_destr_decl -> constr_destr_header, block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 417: // constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, tkAssign, 
                //                      unlabelled_stmt, tkSemiColon
{ 
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,false,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-6].Merge(LocationStack[LocationStack.Depth-2]));
            if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
        }
        break;
      case 418: // constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
                //                      fp_list, tkAssign, unlabelled_stmt, tkSemiColon
{ 
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,true,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-2]));
            if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
        }
        break;
      case 419: // inclass_constr_destr_decl -> constr_destr_header, inclass_block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 420: // inclass_constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, 
                //                              tkAssign, unlabelled_stmt, tkSemiColon
{ 
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,false,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-6].Merge(LocationStack[LocationStack.Depth-2]));
            if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
        }
        break;
      case 421: // inclass_constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
                //                              fp_list, tkAssign, unlabelled_stmt, tkSemiColon
{ 
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,true,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-2]));
            if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
        }
        break;
      case 422: // proc_func_decl -> proc_func_decl_noclass
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 423: // proc_func_decl -> class_or_static, proc_func_decl_noclass
{ 
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 424: // proc_func_decl_noclass -> proc_func_header, proc_func_external_block
{
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
        }
        break;
      case 425: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                           optional_method_modificators1, tkAssign, expr_l1, 
                //                           tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 426: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, expr_l1, 
                //                           tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].ex is dot_question_node)
				parsertools.AddErrorFromResource("DOT_QUECTION_IN_SHORT_FUN",LocationStack[LocationStack.Depth-2]);
	
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 427: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                           optional_method_modificators1, tkAssign, 
                //                           func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 428: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, 
                //                           func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 429: // proc_func_decl_noclass -> tkProcedure, proc_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, 
                //                           unlabelled_stmt, tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-2]);
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 430: // proc_func_decl_noclass -> proc_func_header, tkForward, tkSemiColon
{
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-3].td as procedure_header, null, CurrentLocationSpan);
            (CurrentSemanticValue.stn as procedure_definition).proc_header.proc_attributes.Add((procedure_attribute)ValueStack[ValueStack.Depth-2].id, ValueStack[ValueStack.Depth-2].id.source_context);
		}
        break;
      case 431: // inclass_proc_func_decl -> inclass_proc_func_decl_noclass
{ 
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
        }
        break;
      case 432: // inclass_proc_func_decl -> class_or_static, inclass_proc_func_decl_noclass
{ 
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 433: // inclass_proc_func_decl_noclass -> proc_func_header, inclass_block
{
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
		}
        break;
      case 434: // inclass_proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, 
                //                                   fptype, optional_method_modificators1, 
                //                                   tkAssign, expr_l1_func_decl_lambda, 
                //                                   tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
			if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 435: // inclass_proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                                   optional_method_modificators1, tkAssign, 
                //                                   expr_l1_func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
			if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 436: // inclass_proc_func_decl_noclass -> tkProcedure, proc_name, fp_list, 
                //                                   optional_method_modificators1, tkAssign, 
                //                                   unlabelled_stmt, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
			if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 437: // proc_func_external_block -> block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 438: // proc_func_external_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 439: // proc_name -> func_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 440: // func_name -> func_meth_name_ident
{ 
			CurrentSemanticValue.stn = new method_name(null,null, ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan); 
		}
        break;
      case 441: // func_name -> func_class_name_ident_list, tkPoint, func_meth_name_ident
{ 
        	var ln = ValueStack[ValueStack.Depth-3].ob as List<ident>;
        	var cnt = ln.Count;
        	if (cnt == 1)
				CurrentSemanticValue.stn = new method_name(null, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
			else 	
				CurrentSemanticValue.stn = new method_name(ln, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
		}
        break;
      case 442: // func_class_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 443: // func_class_name_ident_list -> func_class_name_ident
{ 
			CurrentSemanticValue.ob = new List<ident>(); 
			(CurrentSemanticValue.ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
		}
        break;
      case 444: // func_class_name_ident_list -> func_class_name_ident_list, tkPoint, 
                //                               func_class_name_ident
{ 
			(ValueStack[ValueStack.Depth-3].ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob; 
		}
        break;
      case 445: // func_meth_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 446: // func_meth_name_ident -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 447: // func_meth_name_ident -> operator_name_ident, template_arguments
{ CurrentSemanticValue.id = new template_operator_name(null, ValueStack[ValueStack.Depth-1].stn as ident_list, ValueStack[ValueStack.Depth-2].ex as operator_name_ident, CurrentLocationSpan); }
        break;
      case 448: // func_name_with_template_args -> func_name_ident
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 449: // func_name_with_template_args -> func_name_ident, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 450: // func_name_ident -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 451: // proc_header -> tkProcedure, proc_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{ 
        	CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, CurrentLocationSpan); 
        }
        break;
      case 452: // func_header -> tkFunction, func_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, null, CurrentLocationSpan); 
		}
        break;
      case 453: // func_header -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                optional_method_modificators, optional_where_section
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, ValueStack[ValueStack.Depth-3].td as type_definition, CurrentLocationSpan); 
        }
        break;
      case 454: // external_block -> tkExternal, external_directive_ident, tkName, 
                //                   external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 455: // external_block -> tkExternal, external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-2].ex, null, CurrentLocationSpan); 
		}
        break;
      case 456: // external_block -> tkExternal, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(null, null, CurrentLocationSpan); 
		}
        break;
      case 457: // external_directive_ident -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 458: // external_directive_ident -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 459: // block -> decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 460: // inclass_block -> inclass_decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 461: // inclass_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 462: // fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 463: // fp_list -> tkRoundOpen, tkRoundClose
{ 
			CurrentSemanticValue.stn = null;
		}
        break;
      case 464: // fp_list -> tkRoundOpen, fp_sect_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			if (CurrentSemanticValue.stn != null)
				CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 465: // fp_sect_list -> fp_sect
{ 
			CurrentSemanticValue.stn = new formal_parameters(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
        }
        break;
      case 466: // fp_sect_list -> fp_sect_list, tkSemiColon, fp_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);   
        }
        break;
      case 467: // fp_sect -> attribute_declarations, simple_fp_sect
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as  attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 468: // simple_fp_sect -> param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan); 
		}
        break;
      case 469: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.var_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 470: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.const_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 471: // simple_fp_sect -> tkParams, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td,parametr_kind.params_parametr,null, CurrentLocationSpan);  
		}
        break;
      case 472: // simple_fp_sect -> param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.none, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 473: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.var_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 474: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.const_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 475: // param_name_list -> param_name
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 476: // param_name_list -> param_name_list, tkComma, param_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);  
		}
        break;
      case 477: // param_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 478: // fptype -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 479: // fptype_noproctype -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 480: // fptype_noproctype -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 481: // fptype_noproctype -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 482: // fptype_noproctype -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 483: // fptype_noproctype -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 484: // stmt -> unlabelled_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 485: // stmt -> label_name, tkColon, stmt
{ 
			CurrentSemanticValue.stn = new labeled_statement(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);  
		}
        break;
      case 486: // unlabelled_stmt -> /* empty */
{ 
			CurrentSemanticValue.stn = new empty_statement(); 
			CurrentSemanticValue.stn.source_context = null;
		}
        break;
      case 487: // unlabelled_stmt -> assignment
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 488: // unlabelled_stmt -> proc_call
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 489: // unlabelled_stmt -> goto_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 490: // unlabelled_stmt -> compound_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 491: // unlabelled_stmt -> if_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 492: // unlabelled_stmt -> case_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 493: // unlabelled_stmt -> repeat_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 494: // unlabelled_stmt -> while_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 495: // unlabelled_stmt -> for_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 496: // unlabelled_stmt -> with_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 497: // unlabelled_stmt -> inherited_message
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 498: // unlabelled_stmt -> try_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 499: // unlabelled_stmt -> raise_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 500: // unlabelled_stmt -> foreach_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 501: // unlabelled_stmt -> var_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 502: // unlabelled_stmt -> expr_as_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 503: // unlabelled_stmt -> lock_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 504: // unlabelled_stmt -> yield_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 505: // unlabelled_stmt -> yield_sequence_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 506: // unlabelled_stmt -> loop_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 507: // unlabelled_stmt -> match_with
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 508: // loop_stmt -> tkLoop, expr_l1, tkDo, unlabelled_stmt
{
			CurrentSemanticValue.stn = new loop_stmt(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].stn as statement,CurrentLocationSpan);
		}
        break;
      case 509: // yield_stmt -> tkYield, expr_l1_func_decl_lambda
{
			CurrentSemanticValue.stn = new yield_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 510: // yield_sequence_stmt -> tkYield, tkSequence, expr_l1_func_decl_lambda
{
			CurrentSemanticValue.stn = new yield_sequence_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 511: // var_stmt -> tkVar, var_decl_part
{ 
			CurrentSemanticValue.stn = new var_statement(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 512: // var_stmt -> tkRoundOpen, tkVar, identifier, tkComma, var_ident_list, 
                //             tkRoundClose, tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].ob as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].ob as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 513: // var_stmt -> tkVar, tkRoundOpen, identifier, tkComma, ident_list, tkRoundClose, 
                //             tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			ValueStack[ValueStack.Depth-4].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
	    }
        break;
      case 514: // assignment -> var_reference, assign_operator, expr_with_func_decl_lambda
{      
        	if (!(ValueStack[ValueStack.Depth-3].ex is addressed_value))
        		parsertools.AddErrorFromResource("LEFT_SIDE_CANNOT_BE_ASSIGNED_TO",CurrentLocationSpan);
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan);
        }
        break;
      case 515: // assignment -> tkRoundOpen, variable, tkComma, variable_list, tkRoundClose, 
                //               assign_operator, expr
{
			if (ValueStack[ValueStack.Depth-2].op.type != Operators.Assignment)
			    parsertools.AddErrorFromResource("ONLY_BASE_ASSIGNMENT_FOR_TUPLE",LocationStack[LocationStack.Depth-2]);
			(ValueStack[ValueStack.Depth-4].ob as addressed_value_list).Insert(0,ValueStack[ValueStack.Depth-6].ex as addressed_value);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_tuple(ValueStack[ValueStack.Depth-4].ob as addressed_value_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 516: // variable_list -> variable
{
		CurrentSemanticValue.ob = new addressed_value_list(ValueStack[ValueStack.Depth-1].ex as addressed_value,LocationStack[LocationStack.Depth-1]);
	}
        break;
      case 517: // variable_list -> variable_list, tkComma, variable
{
		(ValueStack[ValueStack.Depth-3].ob as addressed_value_list).Add(ValueStack[ValueStack.Depth-1].ex as addressed_value);
		(ValueStack[ValueStack.Depth-3].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
	}
        break;
      case 518: // var_ident_list -> tkVar, identifier
{
		CurrentSemanticValue.ob = new ident_list(ValueStack[ValueStack.Depth-1].id,CurrentLocationSpan);
	}
        break;
      case 519: // var_ident_list -> var_ident_list, tkComma, tkVar, identifier
{
		(ValueStack[ValueStack.Depth-4].ob as ident_list).Add(ValueStack[ValueStack.Depth-1].id);
		(ValueStack[ValueStack.Depth-4].ob as ident_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-4].ob;
	}
        break;
      case 520: // proc_call -> var_reference
{ 
			CurrentSemanticValue.stn = new procedure_call(ValueStack[ValueStack.Depth-1].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex is ident, CurrentLocationSpan); 
		}
        break;
      case 521: // goto_stmt -> tkGoto, label_name
{ 
			CurrentSemanticValue.stn = new goto_statement(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 522: // compound_stmt -> tkBegin, stmt_list, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			(CurrentSemanticValue.stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(CurrentSemanticValue.stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 523: // stmt_list -> stmt
{ 
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 524: // stmt_list -> stmt_list, tkSemiColon, stmt
{  
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as statement_list).Add(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
        }
        break;
      case 525: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan); 
        }
        break;
      case 526: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt, tkElse, unlabelled_stmt
{
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as statement, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 527: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, else_case, tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 528: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, tkSemiColon, else_case, 
                //               tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 529: // pattern_cases -> pattern_case
{
            CurrentSemanticValue.stn = new pattern_cases(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 530: // pattern_cases -> pattern_cases, tkSemiColon, pattern_case
{
            CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as pattern_cases).Add(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 531: // pattern_case -> pattern_optional_var, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 532: // pattern_case -> deconstruction_or_const_pattern, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 533: // pattern_case -> collection_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 534: // pattern_case -> tuple_pattern, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
		}
        break;
      case 535: // pattern_case -> tuple_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 536: // case_stmt -> tkCase, expr_l1, tkOf, case_list, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 537: // case_stmt -> tkCase, expr_l1, tkOf, case_list, tkSemiColon, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 538: // case_stmt -> tkCase, expr_l1, tkOf, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-4].ex, NewCaseItem(new empty_statement(), null), ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 539: // case_list -> case_item
{
			if (ValueStack[ValueStack.Depth-1].stn is empty_statement) 
				CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, null);
			else CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 540: // case_list -> case_list, tkSemiColon, case_item
{ 
			CurrentSemanticValue.stn = AddCaseItem(ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 541: // case_item -> case_label_list, tkColon, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new case_variant(ValueStack[ValueStack.Depth-3].stn as expression_list, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 542: // case_label_list -> case_label
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 543: // case_label_list -> case_label_list, tkComma, case_label
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 544: // case_label -> const_elem
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 545: // else_case -> /* empty */
{ CurrentSemanticValue.stn = null;}
        break;
      case 546: // else_case -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 547: // repeat_stmt -> tkRepeat, stmt_list, tkUntil, expr
{ 
		    CurrentSemanticValue.stn = new repeat_node(ValueStack[ValueStack.Depth-3].stn as statement_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-3].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-4].ti;
			(ValueStack[ValueStack.Depth-3].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-2].ti;
			ValueStack[ValueStack.Depth-3].stn.source_context = LocationStack[LocationStack.Depth-4].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 548: // while_stmt -> tkWhile, expr_l1, optional_tk_do, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewWhileStmt(ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);    
        }
        break;
      case 549: // optional_tk_do -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 550: // optional_tk_do -> /* empty */
{ CurrentSemanticValue.ti = null; }
        break;
      case 551: // lock_stmt -> tkLock, expr_l1, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new lock_stmt(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 552: // foreach_stmt -> tkForeach, identifier, foreach_stmt_ident_dype_opt, tkIn, 
                //                 expr_l1, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-6].id, ValueStack[ValueStack.Depth-5].td, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
            if (ValueStack[ValueStack.Depth-5].td == null)
                parsertools.AddWarningFromResource("USING_UNLOCAL_FOREACH_VARIABLE", ValueStack[ValueStack.Depth-6].id.source_context);
        }
        break;
      case 553: // foreach_stmt -> tkForeach, tkVar, identifier, tkColon, type_ref, tkIn, expr_l1, 
                //                 tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-5].td, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 554: // foreach_stmt -> tkForeach, tkVar, identifier, tkIn, expr_l1, tkDo, 
                //                 unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-5].id, new no_type_foreach(), ValueStack[ValueStack.Depth-3].ex, (statement)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 555: // foreach_stmt -> tkForeach, tkVar, tkRoundOpen, ident_list, tkRoundClose, tkIn, 
                //                 expr_l1, tkDo, unlabelled_stmt
{ 
        	if (parsertools.build_tree_for_formatter)
        	{
        		var il = ValueStack[ValueStack.Depth-6].stn as ident_list;
        		il.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]); // ����� ��� ��������������
        		CurrentSemanticValue.stn = new foreach_stmt_formatting(il,ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].stn as statement,CurrentLocationSpan);
        	}
        	else
        	{
        		// ���� �������� - ���������, ��� ����� ������� ������������ ���� ��� ��������
        		// ��������� ����� � � foreach, �� ���-�� ������ ���� ������, ��� ��� �������� ����
        		// ��������, ������������� #fe - �� ��� ������ ����
                var id = NewId("#fe",LocationStack[LocationStack.Depth-6]);
                var tttt = new assign_var_tuple(ValueStack[ValueStack.Depth-6].stn as ident_list, id, CurrentLocationSpan);
                statement_list nine = ValueStack[ValueStack.Depth-1].stn is statement_list ? ValueStack[ValueStack.Depth-1].stn as statement_list : new statement_list(ValueStack[ValueStack.Depth-1].stn as statement,LocationStack[LocationStack.Depth-1]);
                nine.Insert(0,tttt);
			    var fe = new foreach_stmt(id, new no_type_foreach(), ValueStack[ValueStack.Depth-3].ex, nine, CurrentLocationSpan);
			    fe.ext = ValueStack[ValueStack.Depth-6].stn as ident_list;
			    CurrentSemanticValue.stn = fe;
			}
        }
        break;
      case 556: // foreach_stmt_ident_dype_opt -> tkColon, type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 558: // for_stmt -> tkFor, optional_var, identifier, for_stmt_decl_or_assign, expr_l1, 
                //             for_cycle_type, expr_l1, optional_tk_do, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-8].ob, ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-5].ex, (for_cycle_type)ValueStack[ValueStack.Depth-4].ob, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 559: // for_stmt -> tkFor, optional_var, identifier, for_stmt_decl_or_assign, expr_l1, 
                //             for_cycle_type, expr_l1, tkStep, expr_l1, optional_tk_do, 
                //             unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-10].ob, ValueStack[ValueStack.Depth-9].id, ValueStack[ValueStack.Depth-8].td, ValueStack[ValueStack.Depth-7].ex, (for_cycle_type)ValueStack[ValueStack.Depth-6].ob, ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 560: // optional_var -> tkVar
{ CurrentSemanticValue.ob = true; }
        break;
      case 561: // optional_var -> /* empty */
{ CurrentSemanticValue.ob = false; }
        break;
      case 563: // for_stmt_decl_or_assign -> tkColon, simple_type_identifier, tkAssign
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-2].td; }
        break;
      case 564: // for_cycle_type -> tkTo
{ CurrentSemanticValue.ob = for_cycle_type.to; }
        break;
      case 565: // for_cycle_type -> tkDownto
{ CurrentSemanticValue.ob = for_cycle_type.downto; }
        break;
      case 566: // with_stmt -> tkWith, expr_list, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new with_statement(ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 567: // inherited_message -> tkInherited
{ 
			CurrentSemanticValue.stn = new inherited_message();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 568: // try_stmt -> tkTry, stmt_list, try_handler
{ 
			CurrentSemanticValue.stn = new try_stmt(ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].stn as try_handler, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			ValueStack[ValueStack.Depth-2].stn.source_context = LocationStack[LocationStack.Depth-3].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 569: // try_handler -> tkFinally, stmt_list, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_finally(ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan);
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(ValueStack[ValueStack.Depth-2].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
		}
        break;
      case 570: // try_handler -> tkExcept, exception_block, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_except((exception_block)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan);  
			if ((ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list != null)
			{
				(ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list.source_context = CurrentLocationSpan;
				(ValueStack[ValueStack.Depth-2].stn as exception_block).source_context = CurrentLocationSpan;
			}
		}
        break;
      case 571: // exception_block -> exception_handler_list, exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-2].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 572: // exception_block -> exception_handler_list, tkSemiColon, 
                //                    exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-3].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 573: // exception_block -> stmt_list
{ 
			CurrentSemanticValue.stn = new exception_block(ValueStack[ValueStack.Depth-1].stn as statement_list, null, null, LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 574: // exception_handler_list -> exception_handler
{ 
			CurrentSemanticValue.stn = new exception_handler_list(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 575: // exception_handler_list -> exception_handler_list, tkSemiColon, 
                //                           exception_handler
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as exception_handler_list).Add(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 576: // exception_block_else_branch -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 577: // exception_block_else_branch -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 578: // exception_handler -> tkOn, exception_identifier, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new exception_handler((ValueStack[ValueStack.Depth-3].stn as exception_ident).variable, (ValueStack[ValueStack.Depth-3].stn as exception_ident).type_name, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 579: // exception_identifier -> exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(null, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 580: // exception_identifier -> exception_variable, tkColon, 
                //                         exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(ValueStack[ValueStack.Depth-3].id, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 581: // exception_class_type_identifier -> simple_type_identifier
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 582: // exception_variable -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 583: // raise_stmt -> tkRaise
{ 
			CurrentSemanticValue.stn = new raise_stmt(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 584: // raise_stmt -> tkRaise, expr
{ 
			CurrentSemanticValue.stn = new raise_stmt(ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan);  
		}
        break;
      case 585: // expr_list -> expr_with_func_decl_lambda
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 586: // expr_list -> expr_list, tkComma, expr_with_func_decl_lambda
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 587: // expr_as_stmt -> allowable_expr_as_stmt
{ 
			CurrentSemanticValue.stn = new expression_as_statement(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 588: // allowable_expr_as_stmt -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 589: // expr_with_func_decl_lambda -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 590: // expr_with_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 591: // expr_with_func_decl_lambda -> tkInherited
{ CurrentSemanticValue.ex = new inherited_ident("", CurrentLocationSpan); }
        break;
      case 592: // expr -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 593: // expr -> format_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 594: // expr_l1 -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 595: // expr_l1 -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 596: // expr_l1 -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 597: // expr_l1_for_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 598: // expr_l1_for_question_expr -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 599: // expr_l1_for_new_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 600: // expr_l1_for_new_question_expr -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 601: // expr_l1_func_decl_lambda -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 602: // expr_l1_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 603: // expr_l1_for_lambda -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 604: // expr_l1_for_lambda -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 605: // expr_l1_for_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 606: // expr_dq -> relop_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 607: // expr_dq -> expr_dq, tkDoubleQuestion, relop_expr
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 608: // sizeof_expr -> tkSizeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new sizeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, null, CurrentLocationSpan);  
		}
        break;
      case 609: // typeof_expr -> tkTypeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 610: // typeof_expr -> tkTypeOf, tkRoundOpen, empty_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 611: // question_expr -> expr_l1_for_question_expr, tkQuestion, 
                //                  expr_l1_for_question_expr, tkColon, 
                //                  expr_l1_for_question_expr
{ 
            if (ValueStack[ValueStack.Depth-3].ex is nil_const && ValueStack[ValueStack.Depth-1].ex is nil_const)
            	parsertools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 612: // new_question_expr -> tkIf, expr_l1_for_new_question_expr, tkThen, 
                //                      expr_l1_for_new_question_expr, tkElse, 
                //                      expr_l1_for_new_question_expr
{ 
        	if (parsertools.build_tree_for_formatter)
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
      case 613: // empty_template_type_reference -> simple_type_identifier, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 614: // empty_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
        }
        break;
      case 615: // simple_or_template_type_reference -> simple_type_identifier
{ 
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 616: // simple_or_template_type_reference -> simple_type_identifier, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 617: // simple_or_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 618: // simple_or_template_or_question_type_reference -> 
                //                                                  simple_or_template_type_reference
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 619: // simple_or_template_or_question_type_reference -> simple_type_question
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 620: // optional_array_initializer -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = new array_const((expression_list)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan); 
		}
        break;
      case 622: // new_expr -> tkNew, simple_or_template_type_reference, 
                //             optional_expr_list_with_bracket
{
			CurrentSemanticValue.ex = new new_expr(ValueStack[ValueStack.Depth-2].td, ValueStack[ValueStack.Depth-1].stn as expression_list, false, null, CurrentLocationSpan);
        }
        break;
      case 623: // new_expr -> tkNew, simple_or_template_type_reference, tkSquareOpen, 
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
      case 624: // new_expr -> tkNew, tkClass, tkRoundOpen, list_fields_in_unnamed_object, 
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
      case 625: // field_in_unnamed_object -> identifier, tkAssign, expr_l1
{
		    if (ValueStack[ValueStack.Depth-1].ex is nil_const)
				parsertools.AddErrorFromResource("NIL_IN_UNNAMED_OBJECT",CurrentLocationSpan);		    
			CurrentSemanticValue.ob = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 626: // field_in_unnamed_object -> expr_l1
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
				parsertools.errors.Add(new bad_anon_type(parsertools.CurrentFileName, LocationStack[LocationStack.Depth-1], null));	
			CurrentSemanticValue.ob = new name_assign_expr(name,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 627: // list_fields_in_unnamed_object -> field_in_unnamed_object
{
			var l = new name_assign_expr_list();
			CurrentSemanticValue.ob = l.Add(ValueStack[ValueStack.Depth-1].ob as name_assign_expr);
		}
        break;
      case 628: // list_fields_in_unnamed_object -> list_fields_in_unnamed_object, tkComma, 
                //                                  field_in_unnamed_object
{
			var nel = ValueStack[ValueStack.Depth-3].ob as name_assign_expr_list;
			var ss = nel.name_expr.Select(ne=>ne.name.name).FirstOrDefault(x=>string.Compare(x,(ValueStack[ValueStack.Depth-1].ob as name_assign_expr).name.name,true)==0);
            if (ss != null)
            	parsertools.errors.Add(new anon_type_duplicate_name(parsertools.CurrentFileName, LocationStack[LocationStack.Depth-1], null));
			nel.Add(ValueStack[ValueStack.Depth-1].ob as name_assign_expr);
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
		}
        break;
      case 629: // optional_expr_list_with_bracket -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 630: // optional_expr_list_with_bracket -> tkRoundOpen, optional_expr_list, 
                //                                    tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 631: // relop_expr -> simple_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 632: // relop_expr -> relop_expr, relop, simple_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 633: // relop_expr -> relop_expr, relop, new_question_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 634: // relop_expr -> is_type_expr, tkRoundOpen, pattern_out_param_list, tkRoundClose
{
            var isTypeCheck = ValueStack[ValueStack.Depth-4].ex as typecast_node;
            var deconstructorPattern = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, isTypeCheck.type_def, null, CurrentLocationSpan); 
            CurrentSemanticValue.ex = new is_pattern_expr(isTypeCheck.expr, deconstructorPattern, CurrentLocationSpan);
        }
        break;
      case 635: // pattern -> simple_or_template_type_reference, tkRoundOpen, 
                //            pattern_out_param_list, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 636: // pattern_optional_var -> simple_or_template_type_reference, tkRoundOpen, 
                //                         pattern_out_param_list_optional_var, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 637: // deconstruction_or_const_pattern -> simple_or_template_type_reference, 
                //                                    tkRoundOpen, 
                //                                    pattern_out_param_list_optional_var, 
                //                                    tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 638: // deconstruction_or_const_pattern -> const_pattern_expr_list
{
		    CurrentSemanticValue.stn = new const_pattern(ValueStack[ValueStack.Depth-1].ob as List<syntax_tree_node>, CurrentLocationSpan); 
		}
        break;
      case 639: // const_pattern_expr_list -> const_pattern_expression
{ 
			CurrentSemanticValue.ob = new List<syntax_tree_node>(); 
			(CurrentSemanticValue.ob as List<syntax_tree_node>).Add(ValueStack[ValueStack.Depth-1].stn);
		}
        break;
      case 640: // const_pattern_expr_list -> const_pattern_expr_list, tkComma, 
                //                            const_pattern_expression
{ 
			var list = ValueStack[ValueStack.Depth-3].ob as List<syntax_tree_node>;
            list.Add(ValueStack[ValueStack.Depth-1].stn);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 641: // const_pattern_expression -> literal_or_number
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 642: // const_pattern_expression -> simple_or_template_type_reference
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 643: // const_pattern_expression -> tkNil
{ 
			CurrentSemanticValue.stn = new nil_const();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 644: // const_pattern_expression -> sizeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 645: // const_pattern_expression -> typeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 646: // collection_pattern -> tkSquareOpen, collection_pattern_expr_list, tkSquareClose
{
			CurrentSemanticValue.stn = new collection_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 647: // collection_pattern_expr_list -> collection_pattern_list_item
{
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 648: // collection_pattern_expr_list -> collection_pattern_expr_list, tkComma, 
                //                                 collection_pattern_list_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 649: // collection_pattern_list_item -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 650: // collection_pattern_list_item -> collection_pattern_var_item
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 651: // collection_pattern_list_item -> tkUnderscore
{
			CurrentSemanticValue.stn = new collection_pattern_wild_card(CurrentLocationSpan);
		}
        break;
      case 652: // collection_pattern_list_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 653: // collection_pattern_list_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 654: // collection_pattern_list_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 655: // collection_pattern_list_item -> tkDotDot
{
			CurrentSemanticValue.stn = new collection_pattern_gap_parameter(CurrentLocationSpan);
		}
        break;
      case 656: // collection_pattern_var_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new collection_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 657: // tuple_pattern -> tkRoundOpen, tuple_pattern_item_list, tkRoundClose
{
			if ((ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>).Count>6) 
				parsertools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",CurrentLocationSpan);
			CurrentSemanticValue.stn = new tuple_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 658: // tuple_pattern_item -> tkUnderscore
{ 
			CurrentSemanticValue.stn = new tuple_pattern_wild_card(CurrentLocationSpan); 
		}
        break;
      case 659: // tuple_pattern_item -> literal_or_number
{ 
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 660: // tuple_pattern_item -> sign, literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 661: // tuple_pattern_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new tuple_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 662: // tuple_pattern_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 663: // tuple_pattern_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 664: // tuple_pattern_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 665: // tuple_pattern_item_list -> tuple_pattern_item
{ 
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 666: // tuple_pattern_item_list -> tuple_pattern_item_list, tkComma, tuple_pattern_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 667: // pattern_out_param_list_optional_var -> pattern_out_param_optional_var
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 668: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkSemiColon, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 669: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkComma, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 670: // pattern_out_param_list -> pattern_out_param
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 671: // pattern_out_param_list -> pattern_out_param_list, tkSemiColon, 
                //                           pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 672: // pattern_out_param_list -> pattern_out_param_list, tkComma, pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 673: // pattern_out_param -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 674: // pattern_out_param -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 675: // pattern_out_param -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 676: // pattern_out_param -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 677: // pattern_out_param -> pattern
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 678: // pattern_out_param -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 679: // pattern_out_param -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 680: // pattern_out_param_optional_var -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 681: // pattern_out_param_optional_var -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 682: // pattern_out_param_optional_var -> sign, literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 683: // pattern_out_param_optional_var -> identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, false, CurrentLocationSpan);
        }
        break;
      case 684: // pattern_out_param_optional_var -> identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, false, CurrentLocationSpan);
        }
        break;
      case 685: // pattern_out_param_optional_var -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 686: // pattern_out_param_optional_var -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 687: // pattern_out_param_optional_var -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 688: // pattern_out_param_optional_var -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 689: // pattern_out_param_optional_var -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 690: // simple_expr_or_nothing -> simple_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 691: // simple_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 692: // const_expr_or_nothing -> const_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 693: // const_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 694: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing
{
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 695: // format_expr -> tkColon, simple_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 696: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing, tkColon, 
                //                simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 697: // format_expr -> tkColon, simple_expr_or_nothing, tkColon, simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 698: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 699: // format_const_expr -> tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 700: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing, tkColon, 
                //                      const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 701: // format_const_expr -> tkColon, const_expr_or_nothing, tkColon, const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 702: // relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 703: // relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 704: // relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 705: // relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 706: // relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 707: // relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 708: // relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 709: // relop -> tkNot, tkIn
{ 
			if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op;
			else
			{
				CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op;	
				CurrentSemanticValue.op.type = Operators.NotIn;
			}				
		}
        break;
      case 710: // simple_expr -> term1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 711: // simple_expr -> simple_expr, tkDotDot, term1
{ 
		if (parsertools.build_tree_for_formatter)
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		else 
			CurrentSemanticValue.ex = new diapason_expr_new(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); 
	}
        break;
      case 712: // term1 -> term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 713: // term1 -> term1, addop, term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 714: // term1 -> term1, addop, new_question_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 715: // addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 716: // addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 717: // addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 718: // addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 719: // addop -> tkCSharpStyleOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 720: // typecast_op -> tkAs
{ 
			CurrentSemanticValue.ob = op_typecast.as_op; 
		}
        break;
      case 721: // typecast_op -> tkIs
{ 
			CurrentSemanticValue.ob = op_typecast.is_op; 
		}
        break;
      case 722: // as_is_expr -> is_type_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 723: // as_is_expr -> as_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 724: // as_expr -> term, tkAs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 725: // as_expr -> term, tkAs, array_type
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
	    }
        break;
      case 726: // is_type_expr -> term, tkIs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 727: // is_type_expr -> term, tkIs, array_type
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
	    }
        break;
      case 728: // power_expr -> factor_without_unary_op, tkStarStar, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 729: // power_expr -> factor_without_unary_op, tkStarStar, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 730: // power_expr -> sign, power_expr
{ CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 731: // term -> factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 732: // term -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 733: // term -> power_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 734: // term -> term, mulop, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 735: // term -> term, mulop, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 736: // term -> term, mulop, new_question_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 737: // term -> as_is_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 738: // mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 739: // mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 740: // mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 741: // mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 742: // mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 743: // mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 744: // mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 745: // default_expr -> tkDefault, tkRoundOpen, 
                //                 simple_or_template_or_question_type_reference, tkRoundClose
{ 
			CurrentSemanticValue.ex = new default_operator(ValueStack[ValueStack.Depth-2].td as named_type_reference, CurrentLocationSpan);  
		}
        break;
      case 746: // tuple -> tkRoundOpen, expr_l1_or_unpacked, tkComma, expr_l1_or_unpacked_list, 
                //          lambda_type_ref, optional_full_lambda_fp_list, tkRoundClose
{
			if (ValueStack[ValueStack.Depth-6].ex is unpacked_list_of_ident_or_list) 
				parsertools.AddErrorFromResource("EXPRESSION_EXPECTED",LocationStack[LocationStack.Depth-6]);
			foreach (var ex in (ValueStack[ValueStack.Depth-4].stn as expression_list).expressions)
				if (ex is unpacked_list_of_ident_or_list)
					parsertools.AddErrorFromResource("EXPRESSION_EXPECTED",ex.source_context);
			/*if ($5 != null) 
				parsertools.AddErrorFromResource("BAD_TUPLE",@5);
			if ($6 != null) 
				parsertools.AddErrorFromResource("BAD_TUPLE",@6);*/

			if ((ValueStack[ValueStack.Depth-4].stn as expression_list).Count>6) 
				parsertools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",CurrentLocationSpan);
            (ValueStack[ValueStack.Depth-4].stn as expression_list).Insert(0,ValueStack[ValueStack.Depth-6].ex);
			CurrentSemanticValue.ex = new tuple_node(ValueStack[ValueStack.Depth-4].stn as expression_list,CurrentLocationSpan);
		}
        break;
      case 747: // factor_without_unary_op -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 748: // factor_without_unary_op -> var_reference
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 749: // factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 750: // factor -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 751: // factor -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 752: // factor -> tkSquareOpen, elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new pascal_set_constant(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 753: // factor -> tkNot, factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 754: // factor -> sign, factor
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
			    // ����� ������� ���������� ��������� � �������������� �������
			}
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan);
		}
        break;
      case 755: // factor -> tkDeref, factor
{
            CurrentSemanticValue.ex = new index(ValueStack[ValueStack.Depth-1].ex, true, CurrentLocationSpan);
        }
        break;
      case 756: // factor -> var_reference
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 757: // factor -> tuple
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 758: // literal_or_number -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 759: // literal_or_number -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 760: // var_question_point -> variable, tkQuestionPoint, variable
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 761: // var_question_point -> variable, tkQuestionPoint, var_question_point
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 762: // var_reference -> var_address, variable
{
			CurrentSemanticValue.ex = NewVarReference(ValueStack[ValueStack.Depth-2].stn as get_address, ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);
		}
        break;
      case 763: // var_reference -> variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 764: // var_reference -> var_question_point
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 765: // var_address -> tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(CurrentLocationSpan);
		}
        break;
      case 766: // var_address -> var_address, tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(ValueStack[ValueStack.Depth-2].stn as get_address, CurrentLocationSpan);
		}
        break;
      case 767: // attribute_variable -> simple_type_identifier, optional_expr_list_with_bracket
{ 
			CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
		}
        break;
      case 768: // attribute_variable -> template_type, optional_expr_list_with_bracket
{
            CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 769: // dotted_identifier -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 770: // dotted_identifier -> dotted_identifier, tkPoint, identifier_or_keyword
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parsertools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
		}
        break;
      case 771: // variable_as_type -> dotted_identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;}
        break;
      case 772: // variable_as_type -> dotted_identifier, template_type_params
{ CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-2].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);   }
        break;
      case 773: // variable_or_literal_or_number -> variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 774: // variable_or_literal_or_number -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 775: // var_with_init_for_expr_with_let -> tkVar, identifier, tkAssign, expr, 
                //                                    tkSemiColon
{
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-4].id as addressed_value, ValueStack[ValueStack.Depth-2].ex, Operators.Assignment, CurrentLocationSpan);
		}
        break;
      case 776: // var_with_init_for_expr_with_let_list -> var_with_init_for_expr_with_let
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 777: // var_with_init_for_expr_with_let_list -> var_with_init_for_expr_with_let_list, 
                //                                         var_with_init_for_expr_with_let
{
			ValueStack[ValueStack.Depth-2].stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
		}
        break;
      case 778: // variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 779: // variable -> operator_name_ident
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 780: // variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 781: // variable -> tkRoundOpen, expr, tkRoundClose
{
		    if (!parsertools.build_tree_for_formatter) 
            {
                ValueStack[ValueStack.Depth-2].ex.source_context = CurrentLocationSpan;
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
            } 
			else CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
        }
        break;
      case 782: // variable -> tkRoundOpen, var_with_init_for_expr_with_let_list, expr, 
                //             tkRoundClose
{
		    if (!parsertools.build_tree_for_formatter) 
            {
                ValueStack[ValueStack.Depth-2].ex.source_context = CurrentLocationSpan;
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
            } 
			else CurrentSemanticValue.ex = new expression_with_let(ValueStack[ValueStack.Depth-3].stn as statement_list, ValueStack[ValueStack.Depth-3].stn as expression, CurrentLocationSpan);
		}
        break;
      case 783: // variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 784: // variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 785: // variable -> literal_or_number, tkPoint, identifier_or_keyword
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parsertools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");		
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 786: // variable -> variable_or_literal_or_number, tkSquareOpen, expr_list, 
                //             tkSquareClose
{
        	var el = ValueStack[ValueStack.Depth-2].stn as expression_list; // SSM 10/03/16
        	if (el.Count==1 && el.expressions[0] is format_expr) 
        	{
        		var fe = el.expressions[0] as format_expr;
                if (!parsertools.build_tree_for_formatter)
                {
                    if (fe.expr == null)
                        fe.expr = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
                    if (fe.format1 == null)
                        fe.format1 = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
                }
        		CurrentSemanticValue.ex = new slice_expr(ValueStack[ValueStack.Depth-4].ex as addressed_value,fe.expr,fe.format1,fe.format2,CurrentLocationSpan);
			}   
			// ����������� �����
            else if (el.expressions.Any(e => e is format_expr))
            {
            	if (el.expressions.Count > 4)
            		parsertools.AddErrorFromResource("SLICES_OF MULTIDIMENSIONAL_ARRAYS_ALLOW_ONLY_FOR_RANK_LT_5",CurrentLocationSpan); // ����� ����������� �������� ��������� ������ ��� �������� ����������� < 5  
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
                    	ll.Add(Tuple.Create(ex, (expression)new int32_const(0, ex.source_context), (expression)new int32_const(int.MaxValue, ex.source_context))); // ��������� �������� ������ �����
                    }
				}
				var sle = new slice_expr(ValueStack[ValueStack.Depth-4].ex as addressed_value,null,null,null,CurrentLocationSpan);
				sle.slices = ll;
				CurrentSemanticValue.ex = sle;
            }
			else CurrentSemanticValue.ex = new indexer(ValueStack[ValueStack.Depth-4].ex as addressed_value, el, CurrentLocationSpan);
        }
        break;
      case 787: // variable -> variable_or_literal_or_number, tkQuestionSquareOpen, format_expr, 
                //             tkSquareClose
{
        	var fe = ValueStack[ValueStack.Depth-2].ex as format_expr; // SSM 9/01/17
            if (!parsertools.build_tree_for_formatter)
            {
                if (fe.expr == null)
                    fe.expr = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
                if (fe.format1 == null)
                    fe.format1 = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-2]);
            }
      		CurrentSemanticValue.ex = new slice_expr_question(ValueStack[ValueStack.Depth-4].ex as addressed_value,fe.expr,fe.format1,fe.format2,CurrentLocationSpan);
        }
        break;
      case 788: // variable -> tkVertParen, elem_list, tkVertParen
{ 
			CurrentSemanticValue.ex = new array_const_new(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 789: // variable -> variable, tkRoundOpen, optional_expr_list, tkRoundClose
{
			if (ValueStack[ValueStack.Depth-4].ex is index)
				parsertools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-4], "^");
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 790: // variable -> variable, tkPoint, identifier_keyword_operatorname
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parsertools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 791: // variable -> tuple, tkPoint, identifier_keyword_operatorname
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 792: // variable -> variable, tkDeref
{
			CurrentSemanticValue.ex = new roof_dereference(ValueStack[ValueStack.Depth-2].ex as addressed_value,CurrentLocationSpan);
        }
        break;
      case 793: // variable -> variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 794: // optional_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 795: // optional_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 796: // elem_list -> elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 797: // elem_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 798: // elem_list1 -> elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 799: // elem_list1 -> elem_list1, tkComma, elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 800: // elem -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 801: // elem -> expr, tkDotDot, expr
{ CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 802: // one_literal -> tkStringLiteral
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 803: // one_literal -> tkAsciiChar
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 804: // literal -> literal_list
{ 
			CurrentSemanticValue.ex = NewLiteral(ValueStack[ValueStack.Depth-1].stn as literal_const_line);
        }
        break;
      case 805: // literal -> tkFormatStringLiteral
{
            if (parsertools.build_tree_for_formatter)
   			{
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as string_const;
            }
            else
            {
                CurrentSemanticValue.ex = NewFormatString(ValueStack[ValueStack.Depth-1].stn as string_const);
            }
        }
        break;
      case 806: // literal_list -> one_literal
{ 
			CurrentSemanticValue.stn = new literal_const_line(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 807: // literal_list -> literal_list, one_literal
{ 
        	var line = ValueStack[ValueStack.Depth-2].stn as literal_const_line;
            if (line.literals.Last() is string_const && ValueStack[ValueStack.Depth-1].ex is string_const)
            	parsertools.AddErrorFromResource("TWO_STRING_LITERALS_IN_SUCCESSION",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.stn = line.Add(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 808: // operator_name_ident -> tkOperator, overload_operator
{ 
			CurrentSemanticValue.ex = new operator_name_ident((ValueStack[ValueStack.Depth-1].op as op_type_node).text, (ValueStack[ValueStack.Depth-1].op as op_type_node).type, CurrentLocationSpan);
		}
        break;
      case 809: // optional_method_modificators -> tkSemiColon
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 810: // optional_method_modificators -> tkSemiColon, meth_modificators, tkSemiColon
{ 
			//parsertools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 811: // optional_method_modificators1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 812: // optional_method_modificators1 -> tkSemiColon, meth_modificators
{ 
			//parsertools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 813: // meth_modificators -> meth_modificator
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan); 
		}
        break;
      case 814: // meth_modificators -> meth_modificators, tkSemiColon, meth_modificator
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as procedure_attributes_list).Add(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan);  
		}
        break;
      case 815: // identifier -> tkIdentifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 816: // identifier -> property_specifier_directives
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 817: // identifier -> non_reserved
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 818: // identifier -> tkStep
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 819: // identifier_or_keyword -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 820: // identifier_or_keyword -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 821: // identifier_or_keyword -> reserved_keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 822: // identifier_keyword_operatorname -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 823: // identifier_keyword_operatorname -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 824: // identifier_keyword_operatorname -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 825: // meth_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 826: // meth_modificator -> tkOverload
{ 
            CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id;
            parsertools.AddWarningFromResource("OVERLOAD_IS_NOT_USED", ValueStack[ValueStack.Depth-1].id.source_context);
        }
        break;
      case 827: // meth_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 828: // meth_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 829: // meth_modificator -> tkExtensionMethod
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 830: // meth_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 831: // property_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 832: // property_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 833: // property_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 834: // property_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 835: // property_specifier_directives -> tkRead
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 836: // property_specifier_directives -> tkWrite
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 837: // non_reserved -> tkName
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 838: // non_reserved -> tkNew
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 839: // visibility_specifier -> tkInternal
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 840: // visibility_specifier -> tkPublic
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 841: // visibility_specifier -> tkProtected
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 842: // visibility_specifier -> tkPrivate
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 843: // keyword -> visibility_specifier
{ 
			CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  
		}
        break;
      case 844: // keyword -> tkSealed
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 845: // keyword -> tkTemplate
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 846: // keyword -> tkOr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 847: // keyword -> tkTypeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 848: // keyword -> tkSizeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 849: // keyword -> tkDefault
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 850: // keyword -> tkWhere
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 851: // keyword -> tkXor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 852: // keyword -> tkAnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 853: // keyword -> tkDiv
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 854: // keyword -> tkMod
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 855: // keyword -> tkShl
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 856: // keyword -> tkShr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 857: // keyword -> tkNot
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 858: // keyword -> tkAs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 859: // keyword -> tkIn
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 860: // keyword -> tkIs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 861: // keyword -> tkArray
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 862: // keyword -> tkSequence
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 863: // keyword -> tkBegin
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 864: // keyword -> tkCase
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 865: // keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 866: // keyword -> tkConst
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 867: // keyword -> tkConstructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 868: // keyword -> tkDestructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 869: // keyword -> tkDownto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 870: // keyword -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 871: // keyword -> tkElse
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 872: // keyword -> tkEnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 873: // keyword -> tkExcept
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 874: // keyword -> tkFile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 875: // keyword -> tkAuto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 876: // keyword -> tkFinalization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 877: // keyword -> tkFinally
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 878: // keyword -> tkFor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 879: // keyword -> tkForeach
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 880: // keyword -> tkFunction
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 881: // keyword -> tkIf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 882: // keyword -> tkImplementation
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 883: // keyword -> tkInherited
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 884: // keyword -> tkInitialization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 885: // keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 886: // keyword -> tkProcedure
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 887: // keyword -> tkProperty
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 888: // keyword -> tkRaise
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 889: // keyword -> tkRecord
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 890: // keyword -> tkRepeat
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 891: // keyword -> tkSet
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 892: // keyword -> tkTry
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 893: // keyword -> tkType
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 894: // keyword -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 895: // keyword -> tkThen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 896: // keyword -> tkTo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 897: // keyword -> tkUntil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 898: // keyword -> tkUses
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 899: // keyword -> tkVar
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 900: // keyword -> tkWhile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 901: // keyword -> tkWith
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 902: // keyword -> tkNil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 903: // keyword -> tkGoto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 904: // keyword -> tkOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 905: // keyword -> tkLabel
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 906: // keyword -> tkProgram
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 907: // keyword -> tkUnit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 908: // keyword -> tkLibrary
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 909: // keyword -> tkNamespace
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 910: // keyword -> tkExternal
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 911: // keyword -> tkParams
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 912: // keyword -> tkEvent
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 913: // keyword -> tkYield
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 914: // keyword -> tkMatch
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 915: // keyword -> tkWhen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 916: // keyword -> tkPartial
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 917: // keyword -> tkAbstract
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 918: // keyword -> tkLock
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 919: // keyword -> tkImplicit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 920: // keyword -> tkExplicit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 921: // keyword -> tkOn
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 922: // keyword -> tkVirtual
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 923: // keyword -> tkOverride
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 924: // keyword -> tkLoop
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 925: // keyword -> tkExtensionMethod
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 926: // keyword -> tkOverload
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 927: // keyword -> tkReintroduce
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 928: // keyword -> tkForward
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 929: // reserved_keyword -> tkOperator
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 930: // overload_operator -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 931: // overload_operator -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 932: // overload_operator -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 933: // overload_operator -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 934: // overload_operator -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 935: // overload_operator -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 936: // overload_operator -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 937: // overload_operator -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 938: // overload_operator -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 939: // overload_operator -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 940: // overload_operator -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 941: // overload_operator -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 942: // overload_operator -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 943: // overload_operator -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 944: // overload_operator -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 945: // overload_operator -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 946: // overload_operator -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 947: // overload_operator -> tkNot
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 948: // overload_operator -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 949: // overload_operator -> tkImplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 950: // overload_operator -> tkExplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 951: // overload_operator -> assign_operator
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 952: // overload_operator -> tkStarStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 953: // assign_operator -> tkAssign
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 954: // assign_operator -> tkPlusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 955: // assign_operator -> tkMinusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 956: // assign_operator -> tkMultEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 957: // assign_operator -> tkDivEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 958: // lambda_unpacked_params -> tkBackSlashRoundOpen, 
                //                           lambda_list_of_unpacked_params_or_id, tkComma, 
                //                           lambda_unpacked_params_or_id, tkRoundClose
{
			// ��������� ���� ��������� ������ �� ��������� ���� � function_lambda_definition
			(ValueStack[ValueStack.Depth-4].ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-2].ob as ident_or_list);
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-4].ob as unpacked_list_of_ident_or_list;
		}
        break;
      case 959: // lambda_unpacked_params_or_id -> lambda_unpacked_params
{
			CurrentSemanticValue.ob = new ident_or_list(ValueStack[ValueStack.Depth-1].ex as unpacked_list_of_ident_or_list);
		}
        break;
      case 960: // lambda_unpacked_params_or_id -> identifier
{
			CurrentSemanticValue.ob = new ident_or_list(ValueStack[ValueStack.Depth-1].id as ident);
		}
        break;
      case 961: // lambda_list_of_unpacked_params_or_id -> lambda_unpacked_params_or_id
{
			CurrentSemanticValue.ob = new unpacked_list_of_ident_or_list();
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-1].ob as ident_or_list);
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).source_context = LocationStack[LocationStack.Depth-1];
		}
        break;
      case 962: // lambda_list_of_unpacked_params_or_id -> lambda_list_of_unpacked_params_or_id, 
                //                                         tkComma, lambda_unpacked_params_or_id
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-1].ob as ident_or_list);
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 963: // expr_l1_or_unpacked -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 964: // expr_l1_or_unpacked -> lambda_unpacked_params
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 965: // expr_l1_or_unpacked_list -> expr_l1_or_unpacked
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 966: // expr_l1_or_unpacked_list -> expr_l1_or_unpacked_list, tkComma, 
                //                             expr_l1_or_unpacked
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 967: // func_decl_lambda -> identifier, tkArrow, lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-3].id, LocationStack[LocationStack.Depth-3]); 
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
			//var sl = $3 as statement_list;
			//if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName($3, "Result") != null) // ���� ��� ���� ��������� ��� ���� ���������� Result, �� ��������� ���� 
			    CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
			//else 
			//$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, $3 as statement_list, @$);  
		}
        break;
      case 968: // func_decl_lambda -> tkRoundOpen, tkRoundClose, lambda_type_ref_noproctype, 
                //                     tkArrow, lambda_function_body
{
		    // ����� ���� ������������� �� ���� � ���� ��������� lambda_inferred_type, ���� ������ ��� null!
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �� ���� ��������
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, sl, CurrentLocationSpan);	
		}
        break;
      case 969: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkRoundClose, 
                //                     lambda_type_ref_noproctype, tkArrow, lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-7].id, LocationStack[LocationStack.Depth-7]); 
            var loc = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]);
			var formalPars = new formal_parameters(new typed_parameters(idList, ValueStack[ValueStack.Depth-5].td, parametr_kind.none, null, loc), loc);
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �� ���� ��������
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, sl, CurrentLocationSpan);	
		}
        break;
      case 970: // func_decl_lambda -> tkRoundOpen, identifier, tkSemiColon, full_lambda_fp_list, 
                //                     tkRoundClose, lambda_type_ref_noproctype, tkArrow, 
                //                     lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-7].id, LocationStack[LocationStack.Depth-7]);
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null), parametr_kind.none, null, LocationStack[LocationStack.Depth-7]), LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]));
			for (int i = 0; i < (ValueStack[ValueStack.Depth-5].stn as formal_parameters).Count; i++)
				formalPars.Add((ValueStack[ValueStack.Depth-5].stn as formal_parameters).params_list[i]);
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �� ���� ��������
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, sl, CurrentLocationSpan);	
		}
        break;
      case 971: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkSemiColon, 
                //                     full_lambda_fp_list, tkRoundClose, 
                //                     lambda_type_ref_noproctype, tkArrow, lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-9].id, LocationStack[LocationStack.Depth-9]);
            var loc = LexLocation.MergeAll(LocationStack[LocationStack.Depth-9],LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7]);
			var formalPars = new formal_parameters(new typed_parameters(idList, ValueStack[ValueStack.Depth-7].td, parametr_kind.none, null, loc), LexLocation.MergeAll(LocationStack[LocationStack.Depth-9],LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]));
			for (int i = 0; i < (ValueStack[ValueStack.Depth-5].stn as formal_parameters).Count; i++)
				formalPars.Add((ValueStack[ValueStack.Depth-5].stn as formal_parameters).params_list[i]);
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �� ���� ��������
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, sl, CurrentLocationSpan);
		}
        break;
      case 972: // func_decl_lambda -> tkRoundOpen, expr_l1_or_unpacked, tkComma, 
                //                     expr_l1_or_unpacked_list, lambda_type_ref, 
                //                     optional_full_lambda_fp_list, tkRoundClose, rem_lambda
{ 
			var pair = ValueStack[ValueStack.Depth-1].ob as pair_type_stlist;
			
			if (ValueStack[ValueStack.Depth-4].td is lambda_inferred_type)
			{
				// ������� ���� \(x,y)
				// �������� �� ���� expr_list1. ���� ���� �� ���� - ���� ident_or_list �� ����� �� ���� ����� � �����
				// ���������, ��� $6 = null
				// ������������ List<expression> ��� unpacked_params � ���������
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
				if (has_unpacked) // ��� ����� �����
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
    					//new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), @2), pair.exprs, @$);

					var sl1 = pair.exprs;
			    	if (sl1.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl1, "result") != null) // �� ���� ��������
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
				var lambda_inf_type = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
				var new_typed_pars = new typed_parameters(new ident_list(idd, idd.source_context), lambda_inf_type, parametr_kind.none, null, idd.source_context);
				formal_pars.Add(new_typed_pars);
				foreach (var id in (ValueStack[ValueStack.Depth-5].stn as expression_list).expressions)
				{
					var idd1 = id as ident;
					if (idd1==null)
						parsertools.AddErrorFromResource("ONE_TKIDENTIFIER",id.source_context);
					
					lambda_inf_type = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
					new_typed_pars = new typed_parameters(new ident_list(idd1, idd1.source_context), lambda_inf_type, parametr_kind.none, null, idd1.source_context);
					formal_pars.Add(new_typed_pars);
				}
				
				if (ValueStack[ValueStack.Depth-3].stn != null)
					for (int i = 0; i < (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Count; i++)
						formal_pars.Add((ValueStack[ValueStack.Depth-3].stn as formal_parameters).params_list[i]);		
					
				formal_pars.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]);
			    
			    var sl = pair.exprs;
			    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �� ���� ��������
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
				var formalPars = new formal_parameters(new typed_parameters(idList, parsType, parametr_kind.none, null, loc), LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]));
				
				if (ValueStack[ValueStack.Depth-3].stn != null)
					for (int i = 0; i < (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Count; i++)
						formalPars.Add((ValueStack[ValueStack.Depth-3].stn as formal_parameters).params_list[i]);

				var sl = pair.exprs;
			    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �� ���� ��������
					CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, pair.tn, pair.exprs, CurrentLocationSpan);
				else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, pair.exprs, CurrentLocationSpan);
			}
		}
        break;
      case 973: // func_decl_lambda -> lambda_unpacked_params, rem_lambda
{
    		var pair = ValueStack[ValueStack.Depth-1].ob as pair_type_stlist;
    		// ���� ���������� ��������� - null. �������� �� �������� ���������
    		CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, 
    			new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-2]), pair.exprs, CurrentLocationSpan);
    		// unpacked_params - ��� ��� ������ ���������. ��� ���������� - ���� ������ ���������. ��������, ������ �������
    		var lst_ex = new List<expression>();
    		lst_ex.Add(ValueStack[ValueStack.Depth-2].ex as unpacked_list_of_ident_or_list);
    		(CurrentSemanticValue.ex as function_lambda_definition).unpacked_params = lst_ex;  
    	}
        break;
      case 974: // func_decl_lambda -> expl_func_decl_lambda
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 975: // optional_full_lambda_fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 976: // optional_full_lambda_fp_list -> tkSemiColon, full_lambda_fp_list
{
		CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
	}
        break;
      case 977: // rem_lambda -> lambda_type_ref_noproctype, tkArrow, lambda_function_body
{ 
		    CurrentSemanticValue.ob = new pair_type_stlist(ValueStack[ValueStack.Depth-3].td,ValueStack[ValueStack.Depth-1].stn as statement_list);
		}
        break;
      case 978: // expl_func_decl_lambda -> tkFunction, lambda_type_ref_noproctype, tkArrow, 
                //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 979: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, tkRoundClose, 
                //                          lambda_type_ref_noproctype, tkArrow, 
                //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 980: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, full_lambda_fp_list, 
                //                          tkRoundClose, lambda_type_ref_noproctype, tkArrow, 
                //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 981: // expl_func_decl_lambda -> tkProcedure, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 982: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, tkRoundClose, tkArrow, 
                //                          lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 983: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, full_lambda_fp_list, 
                //                          tkRoundClose, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-4].stn as formal_parameters, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 984: // full_lambda_fp_list -> lambda_simple_fp_sect
{
			var typed_pars = ValueStack[ValueStack.Depth-1].stn as typed_parameters;
			if (typed_pars.vars_type is lambda_inferred_type)
			{
				CurrentSemanticValue.stn = new formal_parameters();
				foreach (var id in typed_pars.idents.idents)
				{
					var lambda_inf_type = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
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
      case 985: // full_lambda_fp_list -> full_lambda_fp_list, tkSemiColon, lambda_simple_fp_sect
{
			CurrentSemanticValue.stn =(ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
		}
        break;
      case 986: // lambda_simple_fp_sect -> ident_list, lambda_type_ref
{
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-2].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan);
		}
        break;
      case 987: // lambda_type_ref -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
		}
        break;
      case 988: // lambda_type_ref -> tkColon, fptype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 989: // lambda_type_ref_noproctype -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
		}
        break;
      case 990: // lambda_type_ref_noproctype -> tkColon, fptype_noproctype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 991: // common_lambda_body -> compound_stmt
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 992: // common_lambda_body -> if_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 993: // common_lambda_body -> while_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 994: // common_lambda_body -> repeat_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 995: // common_lambda_body -> for_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 996: // common_lambda_body -> foreach_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 997: // common_lambda_body -> loop_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 998: // common_lambda_body -> case_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 999: // common_lambda_body -> try_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1000: // common_lambda_body -> lock_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1001: // common_lambda_body -> raise_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1002: // common_lambda_body -> yield_stmt
{
			parsertools.AddErrorFromResource("YIELD_STATEMENT_CANNOT_BE_USED_IN_LAMBDA_BODY", CurrentLocationSpan);
		}
        break;
      case 1003: // common_lambda_body -> tkRoundOpen, assignment, tkRoundClose
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1004: // lambda_function_body -> expr_l1_for_lambda
{
		    var id = SyntaxVisitors.HasNameVisitor.HasName(ValueStack[ValueStack.Depth-1].ex, "Result"); 
            if (id != null)
            {
                 parsertools.AddErrorFromResource("RESULT_IDENT_NOT_EXPECTED_IN_THIS_CONTEXT", id.source_context);
            }
			var sl = new statement_list(new assign("result",ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan),CurrentLocationSpan); // ���� �������� ��� � assign ��� ������������������� ��� ������ - ����� ��������� ����� Result
			sl.expr_lambda_body = true;
			CurrentSemanticValue.stn = sl;
		}
        break;
      case 1005: // lambda_function_body -> common_lambda_body
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 1006: // lambda_procedure_body -> proc_call
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1007: // lambda_procedure_body -> assignment
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1008: // lambda_procedure_body -> common_lambda_body
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
