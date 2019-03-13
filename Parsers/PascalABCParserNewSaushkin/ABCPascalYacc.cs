// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.6
// Machine:  DESKTOP-IF20NRO
// DateTime: 3/13/2019 12:33:33 PM
// UserName: FatCow
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
    tkQuestion=13,tkUnderscore=14,tkQuestionPoint=15,tkDoubleQuestion=16,tkQuestionSquareOpen=17,tkSizeOf=18,
    tkTypeOf=19,tkWhere=20,tkArray=21,tkCase=22,tkClass=23,tkAuto=24,
    tkStatic=25,tkConst=26,tkConstructor=27,tkDestructor=28,tkElse=29,tkExcept=30,
    tkFile=31,tkFor=32,tkForeach=33,tkFunction=34,tkMatch=35,tkWhen=36,
    tkIf=37,tkImplementation=38,tkInherited=39,tkInterface=40,tkProcedure=41,tkOperator=42,
    tkProperty=43,tkRaise=44,tkRecord=45,tkSet=46,tkType=47,tkThen=48,
    tkUses=49,tkVar=50,tkWhile=51,tkWith=52,tkNil=53,tkGoto=54,
    tkOf=55,tkLabel=56,tkLock=57,tkProgram=58,tkEvent=59,tkDefault=60,
    tkTemplate=61,tkPacked=62,tkExports=63,tkResourceString=64,tkThreadvar=65,tkSealed=66,
    tkPartial=67,tkTo=68,tkDownto=69,tkLoop=70,tkSequence=71,tkYield=72,
    tkNew=73,tkOn=74,tkName=75,tkPrivate=76,tkProtected=77,tkPublic=78,
    tkInternal=79,tkRead=80,tkWrite=81,tkParseModeExpression=82,tkParseModeStatement=83,tkParseModeType=84,
    tkBegin=85,tkEnd=86,tkAsmBody=87,tkILCode=88,tkError=89,INVISIBLE=90,
    tkRepeat=91,tkUntil=92,tkDo=93,tkComma=94,tkFinally=95,tkTry=96,
    tkInitialization=97,tkFinalization=98,tkUnit=99,tkLibrary=100,tkExternal=101,tkParams=102,
    tkNamespace=103,tkAssign=104,tkPlusEqual=105,tkMinusEqual=106,tkMultEqual=107,tkDivEqual=108,
    tkMinus=109,tkPlus=110,tkSlash=111,tkStar=112,tkStarStar=113,tkEqual=114,
    tkGreater=115,tkGreaterEqual=116,tkLower=117,tkLowerEqual=118,tkNotEqual=119,tkCSharpStyleOr=120,
    tkArrow=121,tkOr=122,tkXor=123,tkAnd=124,tkDiv=125,tkMod=126,
    tkShl=127,tkShr=128,tkNot=129,tkAs=130,tkIn=131,tkIs=132,
    tkImplicit=133,tkExplicit=134,tkAddressOf=135,tkDeref=136,tkIdentifier=137,tkStringLiteral=138,
    tkFormatStringLiteral=139,tkAsciiChar=140,tkAbstract=141,tkForward=142,tkOverload=143,tkReintroduce=144,
    tkOverride=145,tkVirtual=146,tkExtensionMethod=147,tkInteger=148,tkFloat=149,tkHex=150};

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
  private static Rule[] rules = new Rule[917];
  private static State[] states = new State[1523];
  private static string[] nonTerms = new string[] {
      "parse_goal", "unit_key_word", "class_or_static", "assignment", "optional_array_initializer", 
      "attribute_declarations", "ot_visibility_specifier", "one_attribute", "attribute_variable", 
      "const_factor", "const_variable_2", "const_term", "const_variable", "literal_or_number", 
      "unsigned_number", "variable_or_literal_or_number", "program_block", "optional_var", 
      "class_attribute", "class_attributes", "class_attributes1", "member_list_section", 
      "optional_component_list_seq_end", "const_decl", "only_const_decl", "const_decl_sect", 
      "object_type", "record_type", "member_list", "method_decl_list", "field_or_const_definition_list", 
      "case_stmt", "case_list", "program_decl_sect_list", "int_decl_sect_list1", 
      "inclass_decl_sect_list1", "interface_decl_sect_list", "decl_sect_list", 
      "decl_sect_list1", "inclass_decl_sect_list", "field_or_const_definition", 
      "abc_decl_sect", "decl_sect", "int_decl_sect", "type_decl", "simple_type_decl", 
      "simple_field_or_const_definition", "res_str_decl_sect", "method_decl_withattr", 
      "method_or_property_decl", "property_definition", "fp_sect", "default_expr", 
      "tuple", "expr_as_stmt", "exception_block", "external_block", "exception_handler", 
      "exception_handler_list", "exception_identifier", "typed_const_list1", 
      "typed_const_list", "optional_expr_list", "elem_list", "optional_expr_list_with_bracket", 
      "expr_list", "const_elem_list1", "case_label_list", "const_elem_list", 
      "optional_const_func_expr_list", "elem_list1", "enumeration_id", "expr_l1_list", 
      "enumeration_id_list", "const_simple_expr", "term", "simple_term", "typed_const", 
      "typed_const_plus", "typed_var_init_expression", "expr", "expr_with_func_decl_lambda", 
      "const_expr", "elem", "range_expr", "const_elem", "array_const", "factor", 
      "relop_expr", "expr_dq", "expr_l1", "expr_l1_func_decl_lambda", "simple_expr", 
      "range_term", "range_factor", "external_directive_ident", "init_const_expr", 
      "case_label", "variable", "var_reference", "optional_read_expr", "simple_expr_or_nothing", 
      "var_question_point", "for_cycle_type", "format_expr", "format_const_expr", 
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
      "lock_stmt", "func_name", "proc_name", "optional_proc_name", "qualified_identifier", 
      "new_expr", "allowable_expr_as_stmt", "parts", "inclass_block", "block", 
      "proc_func_external_block", "exception_class_type_identifier", "simple_type_identifier", 
      "base_class_name", "base_classes_names_list", "optional_base_classes", 
      "one_compiler_directive", "optional_head_compiler_directives", "head_compiler_directives", 
      "program_heading_2", "optional_tk_point", "program_param_list", "optional_semicolon", 
      "operator_name_ident", "const_relop", "const_addop", "assign_operator", 
      "const_mulop", "relop", "addop", "mulop", "sign", "overload_operator", 
      "typecast_op", "property_specifiers", "write_property_specifiers", "read_property_specifiers", 
      "array_defaultproperty", "meth_modificators", "optional_method_modificators", 
      "optional_method_modificators1", "meth_modificator", "property_modificator", 
      "proc_call", "proc_func_constr_destr_decl", "proc_func_decl", "inclass_proc_func_decl", 
      "inclass_proc_func_decl_noclass", "constr_destr_decl", "inclass_constr_destr_decl", 
      "method_decl", "proc_func_constr_destr_decl_with_attr", "proc_func_decl_noclass", 
      "method_header", "proc_type_decl", "procedural_type_kind", "proc_header", 
      "procedural_type", "constr_destr_header", "proc_func_header", "func_header", 
      "method_procfunc_header", "int_func_header", "int_proc_header", "property_interface", 
      "program_file", "program_header", "parameter_decl", "parameter_decl_list", 
      "property_parameter_list", "const_set", "question_expr", "question_constexpr", 
      "record_const", "const_field_list_1", "const_field_list", "const_field", 
      "repeat_stmt", "raise_stmt", "pointer_type", "attribute_declaration", "one_or_some_attribute", 
      "stmt_list", "else_case", "exception_block_else_branch", "compound_stmt", 
      "string_type", "sizeof_expr", "simple_property_definition", "stmt_or_expression", 
      "unlabelled_stmt", "stmt", "case_item", "set_type", "as_is_expr", "as_is_constexpr", 
      "is_expr", "as_expr", "power_expr", "power_constexpr", "unsized_array_type", 
      "simple_type_or_", "simple_type", "simple_type_question", "foreach_stmt_ident_dype_opt", 
      "fptype", "type_ref", "fptype_noproctype", "array_type", "template_param", 
      "template_empty_param", "structured_type", "unpacked_structured_type", 
      "empty_template_type_reference", "simple_or_template_type_reference", "type_ref_or_secific", 
      "for_stmt_decl_or_assign", "type_decl_type", "type_ref_and_secific_list", 
      "type_decl_sect", "try_handler", "class_or_interface_keyword", "optional_tk_do", 
      "keyword", "reserved_keyword", "typeof_expr", "simple_fp_sect", "template_param_list", 
      "template_empty_param_list", "template_type_params", "template_type_empty_params", 
      "template_type", "try_stmt", "uses_clause", "used_units_list", "unit_file", 
      "used_unit_name", "unit_header", "var_decl_sect", "var_decl", "var_decl_part", 
      "field_definition", "var_decl_with_assign_var_tuple", "var_stmt", "where_part", 
      "where_part_list", "optional_where_section", "while_stmt", "with_stmt", 
      "variable_as_type", "dotted_identifier", "func_decl_lambda", "expl_func_decl_lambda", 
      "lambda_type_ref", "lambda_type_ref_noproctype", "full_lambda_fp_list", 
      "lambda_simple_fp_sect", "lambda_function_body", "lambda_procedure_body", 
      "common_lambda_body", "optional_full_lambda_fp_list", "field_in_unnamed_object", 
      "list_fields_in_unnamed_object", "func_class_name_ident_list", "rem_lambda", 
      "variable_list", "var_ident_list", "tkAssignOrEqual", "pattern", "pattern_optional_var", 
      "const_pattern", "match_with", "pattern_case", "pattern_cases", "pattern_out_param", 
      "pattern_out_param_optional_var", "tuple_pattern_expr_list", "const_pattern_expr_list", 
      "pattern_out_param_list", "pattern_out_param_list_optional_var", "const_pattern_expression", 
      "tuple_pattern", "tuple_pattern_expr", "$accept", };

  static GPPGParser() {
    states[0] = new State(new int[]{58,1430,11,611,82,1505,84,1510,83,1517,3,-25,49,-25,85,-25,56,-25,26,-25,64,-25,47,-25,50,-25,59,-25,41,-25,34,-25,25,-25,23,-25,27,-25,28,-25,99,-200,100,-200,103,-200},new int[]{-1,1,-219,3,-220,4,-289,1442,-6,1443,-234,949,-161,1504});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{3,1426,49,-12,85,-12,56,-12,26,-12,64,-12,47,-12,50,-12,59,-12,11,-12,41,-12,34,-12,25,-12,23,-12,27,-12,28,-12},new int[]{-171,5,-172,1424,-170,1429});
    states[5] = new State(-36,new int[]{-287,6});
    states[6] = new State(new int[]{49,14,56,-60,26,-60,64,-60,47,-60,50,-60,59,-60,11,-60,41,-60,34,-60,25,-60,23,-60,27,-60,28,-60,85,-60},new int[]{-17,7,-34,114,-38,1361,-39,1362});
    states[7] = new State(new int[]{7,9,10,10,5,11,94,12,6,13,2,-24},new int[]{-174,8});
    states[8] = new State(-18);
    states[9] = new State(-19);
    states[10] = new State(-20);
    states[11] = new State(-21);
    states[12] = new State(-22);
    states[13] = new State(-23);
    states[14] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,30,64,31,65,24,66,98,67,95,68,32,69,33,70,34,71,37,72,38,73,39,74,97,75,40,76,41,77,43,78,44,79,45,80,91,81,46,82,96,83,47,84,25,85,48,86,68,87,92,88,49,89,50,90,51,91,52,92,53,93,54,94,55,95,56,96,58,97,99,98,100,99,103,100,101,101,102,102,59,103,72,104,35,105,36,106,42,108,86,109},new int[]{-288,15,-290,113,-142,19,-123,112,-132,22,-136,24,-137,27,-277,30,-135,31,-278,107});
    states[15] = new State(new int[]{10,16,94,17});
    states[16] = new State(-37);
    states[17] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,30,64,31,65,24,66,98,67,95,68,32,69,33,70,34,71,37,72,38,73,39,74,97,75,40,76,41,77,43,78,44,79,45,80,91,81,46,82,96,83,47,84,25,85,48,86,68,87,92,88,49,89,50,90,51,91,52,92,53,93,54,94,55,95,56,96,58,97,99,98,100,99,103,100,101,101,102,102,59,103,72,104,35,105,36,106,42,108,86,109},new int[]{-290,18,-142,19,-123,112,-132,22,-136,24,-137,27,-277,30,-135,31,-278,107});
    states[18] = new State(-39);
    states[19] = new State(new int[]{7,20,131,110,10,-40,94,-40});
    states[20] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,30,64,31,65,24,66,98,67,95,68,32,69,33,70,34,71,37,72,38,73,39,74,97,75,40,76,41,77,43,78,44,79,45,80,91,81,46,82,96,83,47,84,25,85,48,86,68,87,92,88,49,89,50,90,51,91,52,92,53,93,54,94,55,95,56,96,58,97,99,98,100,99,103,100,101,101,102,102,59,103,72,104,35,105,36,106,42,108,86,109},new int[]{-123,21,-132,22,-136,24,-137,27,-277,30,-135,31,-278,107});
    states[21] = new State(-35);
    states[22] = new State(-752);
    states[23] = new State(-749);
    states[24] = new State(-750);
    states[25] = new State(-767);
    states[26] = new State(-768);
    states[27] = new State(-751);
    states[28] = new State(-769);
    states[29] = new State(-770);
    states[30] = new State(-753);
    states[31] = new State(-775);
    states[32] = new State(-771);
    states[33] = new State(-772);
    states[34] = new State(-773);
    states[35] = new State(-774);
    states[36] = new State(-776);
    states[37] = new State(-777);
    states[38] = new State(-778);
    states[39] = new State(-779);
    states[40] = new State(-780);
    states[41] = new State(-781);
    states[42] = new State(-782);
    states[43] = new State(-783);
    states[44] = new State(-784);
    states[45] = new State(-785);
    states[46] = new State(-786);
    states[47] = new State(-787);
    states[48] = new State(-788);
    states[49] = new State(-789);
    states[50] = new State(-790);
    states[51] = new State(-791);
    states[52] = new State(-792);
    states[53] = new State(-793);
    states[54] = new State(-794);
    states[55] = new State(-795);
    states[56] = new State(-796);
    states[57] = new State(-797);
    states[58] = new State(-798);
    states[59] = new State(-799);
    states[60] = new State(-800);
    states[61] = new State(-801);
    states[62] = new State(-802);
    states[63] = new State(-803);
    states[64] = new State(-804);
    states[65] = new State(-805);
    states[66] = new State(-806);
    states[67] = new State(-807);
    states[68] = new State(-808);
    states[69] = new State(-809);
    states[70] = new State(-810);
    states[71] = new State(-811);
    states[72] = new State(-812);
    states[73] = new State(-813);
    states[74] = new State(-814);
    states[75] = new State(-815);
    states[76] = new State(-816);
    states[77] = new State(-817);
    states[78] = new State(-818);
    states[79] = new State(-819);
    states[80] = new State(-820);
    states[81] = new State(-821);
    states[82] = new State(-822);
    states[83] = new State(-823);
    states[84] = new State(-824);
    states[85] = new State(-825);
    states[86] = new State(-826);
    states[87] = new State(-827);
    states[88] = new State(-828);
    states[89] = new State(-829);
    states[90] = new State(-830);
    states[91] = new State(-831);
    states[92] = new State(-832);
    states[93] = new State(-833);
    states[94] = new State(-834);
    states[95] = new State(-835);
    states[96] = new State(-836);
    states[97] = new State(-837);
    states[98] = new State(-838);
    states[99] = new State(-839);
    states[100] = new State(-840);
    states[101] = new State(-841);
    states[102] = new State(-842);
    states[103] = new State(-843);
    states[104] = new State(-844);
    states[105] = new State(-845);
    states[106] = new State(-846);
    states[107] = new State(-754);
    states[108] = new State(-847);
    states[109] = new State(-848);
    states[110] = new State(new int[]{138,111});
    states[111] = new State(-41);
    states[112] = new State(-34);
    states[113] = new State(-38);
    states[114] = new State(new int[]{85,116},new int[]{-239,115});
    states[115] = new State(-32);
    states[116] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,664,150,155,149,665,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476},new int[]{-236,117,-245,662,-244,121,-4,122,-100,123,-117,338,-99,345,-132,663,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754,-128,890});
    states[117] = new State(new int[]{86,118,10,119});
    states[118] = new State(-512);
    states[119] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,664,150,155,149,665,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476},new int[]{-245,120,-244,121,-4,122,-100,123,-117,338,-99,345,-132,663,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754,-128,890});
    states[120] = new State(-514);
    states[121] = new State(-474);
    states[122] = new State(-477);
    states[123] = new State(new int[]{104,383,105,384,106,385,107,386,108,387,86,-510,10,-510,92,-510,95,-510,30,-510,98,-510,29,-510,94,-510,12,-510,9,-510,93,-510,81,-510,80,-510,2,-510,79,-510,78,-510,77,-510,76,-510},new int[]{-180,124});
    states[124] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,502,18,247,19,252,5,573,34,625,41,856},new int[]{-82,125,-81,126,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,352,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572,-305,623,-306,624});
    states[125] = new State(-504);
    states[126] = new State(-576);
    states[127] = new State(new int[]{13,128,86,-578,10,-578,92,-578,95,-578,30,-578,98,-578,29,-578,94,-578,12,-578,9,-578,93,-578,81,-578,80,-578,2,-578,79,-578,78,-578,77,-578,76,-578,6,-578});
    states[128] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,129,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[129] = new State(new int[]{5,130,13,128});
    states[130] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,131,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[131] = new State(new int[]{13,128,86,-589,10,-589,92,-589,95,-589,30,-589,98,-589,29,-589,94,-589,12,-589,9,-589,93,-589,81,-589,80,-589,2,-589,79,-589,78,-589,77,-589,76,-589,5,-589,6,-589,48,-589,55,-589,135,-589,137,-589,75,-589,73,-589,42,-589,39,-589,8,-589,18,-589,19,-589,138,-589,140,-589,139,-589,148,-589,150,-589,149,-589,54,-589,85,-589,37,-589,22,-589,91,-589,51,-589,32,-589,52,-589,96,-589,44,-589,33,-589,50,-589,57,-589,72,-589,70,-589,35,-589,68,-589,69,-589});
    states[132] = new State(new int[]{16,133,13,-580,86,-580,10,-580,92,-580,95,-580,30,-580,98,-580,29,-580,94,-580,12,-580,9,-580,93,-580,81,-580,80,-580,2,-580,79,-580,78,-580,77,-580,76,-580,5,-580,6,-580,48,-580,55,-580,135,-580,137,-580,75,-580,73,-580,42,-580,39,-580,8,-580,18,-580,19,-580,138,-580,140,-580,139,-580,148,-580,150,-580,149,-580,54,-580,85,-580,37,-580,22,-580,91,-580,51,-580,32,-580,52,-580,96,-580,44,-580,33,-580,50,-580,57,-580,72,-580,70,-580,35,-580,68,-580,69,-580});
    states[133] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-89,134,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570});
    states[134] = new State(new int[]{114,291,119,292,117,293,115,294,118,295,116,296,131,297,16,-585,13,-585,86,-585,10,-585,92,-585,95,-585,30,-585,98,-585,29,-585,94,-585,12,-585,9,-585,93,-585,81,-585,80,-585,2,-585,79,-585,78,-585,77,-585,76,-585,5,-585,6,-585,48,-585,55,-585,135,-585,137,-585,75,-585,73,-585,42,-585,39,-585,8,-585,18,-585,19,-585,138,-585,140,-585,139,-585,148,-585,150,-585,149,-585,54,-585,85,-585,37,-585,22,-585,91,-585,51,-585,32,-585,52,-585,96,-585,44,-585,33,-585,50,-585,57,-585,72,-585,70,-585,35,-585,68,-585,69,-585},new int[]{-182,135});
    states[135] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-93,136,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,577,-251,570});
    states[136] = new State(new int[]{110,303,109,304,122,305,123,306,120,307,114,-607,119,-607,117,-607,115,-607,118,-607,116,-607,131,-607,16,-607,13,-607,86,-607,10,-607,92,-607,95,-607,30,-607,98,-607,29,-607,94,-607,12,-607,9,-607,93,-607,81,-607,80,-607,2,-607,79,-607,78,-607,77,-607,76,-607,5,-607,6,-607,48,-607,55,-607,135,-607,137,-607,75,-607,73,-607,42,-607,39,-607,8,-607,18,-607,19,-607,138,-607,140,-607,139,-607,148,-607,150,-607,149,-607,54,-607,85,-607,37,-607,22,-607,91,-607,51,-607,32,-607,52,-607,96,-607,44,-607,33,-607,50,-607,57,-607,72,-607,70,-607,35,-607,68,-607,69,-607},new int[]{-183,137});
    states[137] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-76,138,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,577,-251,570});
    states[138] = new State(new int[]{132,309,130,312,112,314,111,315,125,316,126,317,127,318,128,319,124,320,5,-659,110,-659,109,-659,122,-659,123,-659,120,-659,114,-659,119,-659,117,-659,115,-659,118,-659,116,-659,131,-659,16,-659,13,-659,86,-659,10,-659,92,-659,95,-659,30,-659,98,-659,29,-659,94,-659,12,-659,9,-659,93,-659,81,-659,80,-659,2,-659,79,-659,78,-659,77,-659,76,-659,6,-659,48,-659,55,-659,135,-659,137,-659,75,-659,73,-659,42,-659,39,-659,8,-659,18,-659,19,-659,138,-659,140,-659,139,-659,148,-659,150,-659,149,-659,54,-659,85,-659,37,-659,22,-659,91,-659,51,-659,32,-659,52,-659,96,-659,44,-659,33,-659,50,-659,57,-659,72,-659,70,-659,35,-659,68,-659,69,-659},new int[]{-184,139});
    states[139] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,29,42,359,39,389,8,391,18,247,19,252},new int[]{-88,140,-252,141,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-77,522});
    states[140] = new State(new int[]{132,-677,130,-677,112,-677,111,-677,125,-677,126,-677,127,-677,128,-677,124,-677,5,-677,110,-677,109,-677,122,-677,123,-677,120,-677,114,-677,119,-677,117,-677,115,-677,118,-677,116,-677,131,-677,16,-677,13,-677,86,-677,10,-677,92,-677,95,-677,30,-677,98,-677,29,-677,94,-677,12,-677,9,-677,93,-677,81,-677,80,-677,2,-677,79,-677,78,-677,77,-677,76,-677,6,-677,48,-677,55,-677,135,-677,137,-677,75,-677,73,-677,42,-677,39,-677,8,-677,18,-677,19,-677,138,-677,140,-677,139,-677,148,-677,150,-677,149,-677,54,-677,85,-677,37,-677,22,-677,91,-677,51,-677,32,-677,52,-677,96,-677,44,-677,33,-677,50,-677,57,-677,72,-677,70,-677,35,-677,68,-677,69,-677,113,-672});
    states[141] = new State(-678);
    states[142] = new State(-689);
    states[143] = new State(new int[]{7,144,132,-690,130,-690,112,-690,111,-690,125,-690,126,-690,127,-690,128,-690,124,-690,5,-690,110,-690,109,-690,122,-690,123,-690,120,-690,114,-690,119,-690,117,-690,115,-690,118,-690,116,-690,131,-690,16,-690,13,-690,86,-690,10,-690,92,-690,95,-690,30,-690,98,-690,29,-690,94,-690,12,-690,9,-690,93,-690,81,-690,80,-690,2,-690,79,-690,78,-690,77,-690,76,-690,113,-690,6,-690,48,-690,55,-690,135,-690,137,-690,75,-690,73,-690,42,-690,39,-690,8,-690,18,-690,19,-690,138,-690,140,-690,139,-690,148,-690,150,-690,149,-690,54,-690,85,-690,37,-690,22,-690,91,-690,51,-690,32,-690,52,-690,96,-690,44,-690,33,-690,50,-690,57,-690,72,-690,70,-690,35,-690,68,-690,69,-690,11,-713});
    states[144] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,30,64,31,65,24,66,98,67,95,68,32,69,33,70,34,71,37,72,38,73,39,74,97,75,40,76,41,77,43,78,44,79,45,80,91,81,46,82,96,83,47,84,25,85,48,86,68,87,92,88,49,89,50,90,51,91,52,92,53,93,54,94,55,95,56,96,58,97,99,98,100,99,103,100,101,101,102,102,59,103,72,104,35,105,36,106,42,108,86,109},new int[]{-123,145,-132,22,-136,24,-137,27,-277,30,-135,31,-278,107});
    states[145] = new State(-720);
    states[146] = new State(-697);
    states[147] = new State(new int[]{138,149,140,150,7,-738,11,-738,132,-738,130,-738,112,-738,111,-738,125,-738,126,-738,127,-738,128,-738,124,-738,5,-738,110,-738,109,-738,122,-738,123,-738,120,-738,114,-738,119,-738,117,-738,115,-738,118,-738,116,-738,131,-738,16,-738,13,-738,86,-738,10,-738,92,-738,95,-738,30,-738,98,-738,29,-738,94,-738,12,-738,9,-738,93,-738,81,-738,80,-738,2,-738,79,-738,78,-738,77,-738,76,-738,113,-738,6,-738,48,-738,55,-738,135,-738,137,-738,75,-738,73,-738,42,-738,39,-738,8,-738,18,-738,19,-738,139,-738,148,-738,150,-738,149,-738,54,-738,85,-738,37,-738,22,-738,91,-738,51,-738,32,-738,52,-738,96,-738,44,-738,33,-738,50,-738,57,-738,72,-738,70,-738,35,-738,68,-738,69,-738,121,-738,104,-738,4,-738,136,-738,36,-738},new int[]{-151,148});
    states[148] = new State(-741);
    states[149] = new State(-736);
    states[150] = new State(-737);
    states[151] = new State(-740);
    states[152] = new State(-739);
    states[153] = new State(-698);
    states[154] = new State(-177);
    states[155] = new State(-178);
    states[156] = new State(-179);
    states[157] = new State(-691);
    states[158] = new State(new int[]{8,159});
    states[159] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-268,160,-166,162,-132,196,-136,24,-137,27});
    states[160] = new State(new int[]{9,161});
    states[161] = new State(-687);
    states[162] = new State(new int[]{7,163,4,166,117,168,9,-592,130,-592,132,-592,112,-592,111,-592,125,-592,126,-592,127,-592,128,-592,124,-592,110,-592,109,-592,122,-592,123,-592,114,-592,119,-592,115,-592,118,-592,116,-592,131,-592,13,-592,6,-592,94,-592,12,-592,5,-592,86,-592,10,-592,92,-592,95,-592,30,-592,98,-592,29,-592,93,-592,81,-592,80,-592,2,-592,79,-592,78,-592,77,-592,76,-592,11,-592,8,-592,120,-592,16,-592,48,-592,55,-592,135,-592,137,-592,75,-592,73,-592,42,-592,39,-592,18,-592,19,-592,138,-592,140,-592,139,-592,148,-592,150,-592,149,-592,54,-592,85,-592,37,-592,22,-592,91,-592,51,-592,32,-592,52,-592,96,-592,44,-592,33,-592,50,-592,57,-592,72,-592,70,-592,35,-592,68,-592,69,-592,113,-592},new int[]{-283,165});
    states[163] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,30,64,31,65,24,66,98,67,95,68,32,69,33,70,34,71,37,72,38,73,39,74,97,75,40,76,41,77,43,78,44,79,45,80,91,81,46,82,96,83,47,84,25,85,48,86,68,87,92,88,49,89,50,90,51,91,52,92,53,93,54,94,55,95,56,96,58,97,99,98,100,99,103,100,101,101,102,102,59,103,72,104,35,105,36,106,42,108,86,109},new int[]{-123,164,-132,22,-136,24,-137,27,-277,30,-135,31,-278,107});
    states[164] = new State(-248);
    states[165] = new State(-593);
    states[166] = new State(new int[]{117,168},new int[]{-283,167});
    states[167] = new State(-594);
    states[168] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-281,169,-263,266,-256,173,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-265,588,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,589,-209,553,-208,554,-285,590});
    states[169] = new State(new int[]{115,170,94,171});
    states[170] = new State(-222);
    states[171] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-263,172,-256,173,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-265,588,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,589,-209,553,-208,554,-285,590});
    states[172] = new State(-226);
    states[173] = new State(new int[]{13,174,115,-230,94,-230,114,-230,9,-230,10,-230,121,-230,104,-230,86,-230,92,-230,95,-230,30,-230,98,-230,29,-230,12,-230,93,-230,81,-230,80,-230,2,-230,79,-230,78,-230,77,-230,76,-230,131,-230});
    states[174] = new State(-231);
    states[175] = new State(new int[]{6,1359,110,1348,109,1349,122,1350,123,1351,13,-235,115,-235,94,-235,114,-235,9,-235,10,-235,121,-235,104,-235,86,-235,92,-235,95,-235,30,-235,98,-235,29,-235,12,-235,93,-235,81,-235,80,-235,2,-235,79,-235,78,-235,77,-235,76,-235,131,-235},new int[]{-179,176});
    states[176] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152},new int[]{-94,177,-95,268,-166,439,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151});
    states[177] = new State(new int[]{112,218,111,219,125,220,126,221,127,222,128,223,124,224,6,-239,110,-239,109,-239,122,-239,123,-239,13,-239,115,-239,94,-239,114,-239,9,-239,10,-239,121,-239,104,-239,86,-239,92,-239,95,-239,30,-239,98,-239,29,-239,12,-239,93,-239,81,-239,80,-239,2,-239,79,-239,78,-239,77,-239,76,-239,131,-239},new int[]{-181,178});
    states[178] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152},new int[]{-95,179,-166,439,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151});
    states[179] = new State(new int[]{8,180,112,-241,111,-241,125,-241,126,-241,127,-241,128,-241,124,-241,6,-241,110,-241,109,-241,122,-241,123,-241,13,-241,115,-241,94,-241,114,-241,9,-241,10,-241,121,-241,104,-241,86,-241,92,-241,95,-241,30,-241,98,-241,29,-241,12,-241,93,-241,81,-241,80,-241,2,-241,79,-241,78,-241,77,-241,76,-241,131,-241});
    states[180] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336,9,-172},new int[]{-69,181,-67,183,-86,421,-83,186,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[181] = new State(new int[]{9,182});
    states[182] = new State(-246);
    states[183] = new State(new int[]{94,184,9,-171,12,-171});
    states[184] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-86,185,-83,186,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[185] = new State(-174);
    states[186] = new State(new int[]{13,187,6,1332,94,-175,9,-175,12,-175,5,-175});
    states[187] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-83,188,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[188] = new State(new int[]{5,189,13,187});
    states[189] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-83,190,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[190] = new State(new int[]{13,187,6,-117,94,-117,9,-117,12,-117,5,-117,86,-117,10,-117,92,-117,95,-117,30,-117,98,-117,29,-117,93,-117,81,-117,80,-117,2,-117,79,-117,78,-117,77,-117,76,-117});
    states[191] = new State(new int[]{110,1348,109,1349,122,1350,123,1351,114,1352,119,1353,117,1354,115,1355,118,1356,116,1357,131,1358,13,-114,6,-114,94,-114,9,-114,12,-114,5,-114,86,-114,10,-114,92,-114,95,-114,30,-114,98,-114,29,-114,93,-114,81,-114,80,-114,2,-114,79,-114,78,-114,77,-114,76,-114},new int[]{-179,192,-178,1346});
    states[192] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-12,193,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434});
    states[193] = new State(new int[]{130,216,132,217,112,218,111,219,125,220,126,221,127,222,128,223,124,224,110,-126,109,-126,122,-126,123,-126,114,-126,119,-126,117,-126,115,-126,118,-126,116,-126,131,-126,13,-126,6,-126,94,-126,9,-126,12,-126,5,-126,86,-126,10,-126,92,-126,95,-126,30,-126,98,-126,29,-126,93,-126,81,-126,80,-126,2,-126,79,-126,78,-126,77,-126,76,-126},new int[]{-187,194,-181,197});
    states[194] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-268,195,-166,162,-132,196,-136,24,-137,27});
    states[195] = new State(-131);
    states[196] = new State(-247);
    states[197] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-10,198,-253,1345,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432});
    states[198] = new State(new int[]{113,199,130,-136,132,-136,112,-136,111,-136,125,-136,126,-136,127,-136,128,-136,124,-136,110,-136,109,-136,122,-136,123,-136,114,-136,119,-136,117,-136,115,-136,118,-136,116,-136,131,-136,13,-136,6,-136,94,-136,9,-136,12,-136,5,-136,86,-136,10,-136,92,-136,95,-136,30,-136,98,-136,29,-136,93,-136,81,-136,80,-136,2,-136,79,-136,78,-136,77,-136,76,-136});
    states[199] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-10,200,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432});
    states[200] = new State(-132);
    states[201] = new State(new int[]{4,203,11,205,7,1338,136,1340,8,1341,113,-145,130,-145,132,-145,112,-145,111,-145,125,-145,126,-145,127,-145,128,-145,124,-145,110,-145,109,-145,122,-145,123,-145,114,-145,119,-145,117,-145,115,-145,118,-145,116,-145,131,-145,13,-145,6,-145,94,-145,9,-145,12,-145,5,-145,86,-145,10,-145,92,-145,95,-145,30,-145,98,-145,29,-145,93,-145,81,-145,80,-145,2,-145,79,-145,78,-145,77,-145,76,-145},new int[]{-11,202});
    states[202] = new State(-162);
    states[203] = new State(new int[]{117,168},new int[]{-283,204});
    states[204] = new State(-163);
    states[205] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336,5,1334,12,-172},new int[]{-106,206,-69,208,-83,210,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435,-67,183,-86,421});
    states[206] = new State(new int[]{12,207});
    states[207] = new State(-164);
    states[208] = new State(new int[]{12,209});
    states[209] = new State(-168);
    states[210] = new State(new int[]{5,211,13,187,6,1332,94,-175,12,-175});
    states[211] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336,5,-642,12,-642},new int[]{-107,212,-83,1331,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[212] = new State(new int[]{5,213,12,-647});
    states[213] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-83,214,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[214] = new State(new int[]{13,187,12,-649});
    states[215] = new State(new int[]{130,216,132,217,112,218,111,219,125,220,126,221,127,222,128,223,124,224,110,-125,109,-125,122,-125,123,-125,114,-125,119,-125,117,-125,115,-125,118,-125,116,-125,131,-125,13,-125,6,-125,94,-125,9,-125,12,-125,5,-125,86,-125,10,-125,92,-125,95,-125,30,-125,98,-125,29,-125,93,-125,81,-125,80,-125,2,-125,79,-125,78,-125,77,-125,76,-125},new int[]{-187,194,-181,197});
    states[216] = new State(-665);
    states[217] = new State(-666);
    states[218] = new State(-138);
    states[219] = new State(-139);
    states[220] = new State(-140);
    states[221] = new State(-141);
    states[222] = new State(-142);
    states[223] = new State(-143);
    states[224] = new State(-144);
    states[225] = new State(new int[]{113,199,130,-133,132,-133,112,-133,111,-133,125,-133,126,-133,127,-133,128,-133,124,-133,110,-133,109,-133,122,-133,123,-133,114,-133,119,-133,117,-133,115,-133,118,-133,116,-133,131,-133,13,-133,6,-133,94,-133,9,-133,12,-133,5,-133,86,-133,10,-133,92,-133,95,-133,30,-133,98,-133,29,-133,93,-133,81,-133,80,-133,2,-133,79,-133,78,-133,77,-133,76,-133});
    states[226] = new State(-156);
    states[227] = new State(new int[]{23,1320,137,23,80,25,81,26,75,28,73,29,17,-770,8,-770,7,-770,136,-770,4,-770,15,-770,104,-770,105,-770,106,-770,107,-770,108,-770,86,-770,10,-770,11,-770,5,-770,92,-770,95,-770,30,-770,98,-770,121,-770,132,-770,130,-770,112,-770,111,-770,125,-770,126,-770,127,-770,128,-770,124,-770,110,-770,109,-770,122,-770,123,-770,120,-770,114,-770,119,-770,117,-770,115,-770,118,-770,116,-770,131,-770,16,-770,13,-770,29,-770,94,-770,12,-770,9,-770,93,-770,2,-770,79,-770,78,-770,77,-770,76,-770,113,-770,6,-770,48,-770,55,-770,135,-770,42,-770,39,-770,18,-770,19,-770,138,-770,140,-770,139,-770,148,-770,150,-770,149,-770,54,-770,85,-770,37,-770,22,-770,91,-770,51,-770,32,-770,52,-770,96,-770,44,-770,33,-770,50,-770,57,-770,72,-770,70,-770,35,-770,68,-770,69,-770},new int[]{-268,228,-166,162,-132,196,-136,24,-137,27});
    states[228] = new State(new int[]{11,230,8,620,86,-604,10,-604,92,-604,95,-604,30,-604,98,-604,132,-604,130,-604,112,-604,111,-604,125,-604,126,-604,127,-604,128,-604,124,-604,5,-604,110,-604,109,-604,122,-604,123,-604,120,-604,114,-604,119,-604,117,-604,115,-604,118,-604,116,-604,131,-604,16,-604,13,-604,29,-604,94,-604,12,-604,9,-604,93,-604,81,-604,80,-604,2,-604,79,-604,78,-604,77,-604,76,-604,6,-604,48,-604,55,-604,135,-604,137,-604,75,-604,73,-604,42,-604,39,-604,18,-604,19,-604,138,-604,140,-604,139,-604,148,-604,150,-604,149,-604,54,-604,85,-604,37,-604,22,-604,91,-604,51,-604,32,-604,52,-604,96,-604,44,-604,33,-604,50,-604,57,-604,72,-604,70,-604,35,-604,68,-604,69,-604,113,-604},new int[]{-65,229});
    states[229] = new State(-597);
    states[230] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,502,18,247,19,252,5,573,34,625,41,856,12,-729},new int[]{-63,231,-66,349,-82,501,-81,126,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,352,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572,-305,623,-306,624});
    states[231] = new State(new int[]{12,232});
    states[232] = new State(new int[]{8,234,86,-596,10,-596,92,-596,95,-596,30,-596,98,-596,132,-596,130,-596,112,-596,111,-596,125,-596,126,-596,127,-596,128,-596,124,-596,5,-596,110,-596,109,-596,122,-596,123,-596,120,-596,114,-596,119,-596,117,-596,115,-596,118,-596,116,-596,131,-596,16,-596,13,-596,29,-596,94,-596,12,-596,9,-596,93,-596,81,-596,80,-596,2,-596,79,-596,78,-596,77,-596,76,-596,6,-596,48,-596,55,-596,135,-596,137,-596,75,-596,73,-596,42,-596,39,-596,18,-596,19,-596,138,-596,140,-596,139,-596,148,-596,150,-596,149,-596,54,-596,85,-596,37,-596,22,-596,91,-596,51,-596,32,-596,52,-596,96,-596,44,-596,33,-596,50,-596,57,-596,72,-596,70,-596,35,-596,68,-596,69,-596,113,-596},new int[]{-5,233});
    states[233] = new State(-598);
    states[234] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,840,129,428,110,335,109,336,60,158,9,-186},new int[]{-62,235,-61,237,-79,843,-78,240,-83,241,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435,-87,851,-227,852,-53,844});
    states[235] = new State(new int[]{9,236});
    states[236] = new State(-595);
    states[237] = new State(new int[]{94,238,9,-187});
    states[238] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,840,129,428,110,335,109,336,60,158},new int[]{-79,239,-78,240,-83,241,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435,-87,851,-227,852,-53,844});
    states[239] = new State(-189);
    states[240] = new State(-404);
    states[241] = new State(new int[]{13,187,94,-180,9,-180,86,-180,10,-180,92,-180,95,-180,30,-180,98,-180,29,-180,12,-180,93,-180,81,-180,80,-180,2,-180,79,-180,78,-180,77,-180,76,-180});
    states[242] = new State(-157);
    states[243] = new State(-158);
    states[244] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-132,245,-136,24,-137,27});
    states[245] = new State(-159);
    states[246] = new State(-160);
    states[247] = new State(new int[]{8,248});
    states[248] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-268,249,-166,162,-132,196,-136,24,-137,27});
    states[249] = new State(new int[]{9,250});
    states[250] = new State(-586);
    states[251] = new State(-161);
    states[252] = new State(new int[]{8,253});
    states[253] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-268,254,-267,256,-166,258,-132,196,-136,24,-137,27});
    states[254] = new State(new int[]{9,255});
    states[255] = new State(-587);
    states[256] = new State(new int[]{9,257});
    states[257] = new State(-588);
    states[258] = new State(new int[]{7,163,4,259,117,261,119,1318,9,-592},new int[]{-283,165,-284,1319});
    states[259] = new State(new int[]{117,261,119,1318},new int[]{-283,167,-284,260});
    states[260] = new State(-591);
    states[261] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579,115,-229,94,-229},new int[]{-281,169,-282,262,-263,266,-256,173,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-265,588,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,589,-209,553,-208,554,-285,590,-264,1317});
    states[262] = new State(new int[]{115,263,94,264});
    states[263] = new State(-224);
    states[264] = new State(-229,new int[]{-264,265});
    states[265] = new State(-228);
    states[266] = new State(-225);
    states[267] = new State(new int[]{112,218,111,219,125,220,126,221,127,222,128,223,124,224,6,-238,110,-238,109,-238,122,-238,123,-238,13,-238,115,-238,94,-238,114,-238,9,-238,10,-238,121,-238,104,-238,86,-238,92,-238,95,-238,30,-238,98,-238,29,-238,12,-238,93,-238,81,-238,80,-238,2,-238,79,-238,78,-238,77,-238,76,-238,131,-238},new int[]{-181,178});
    states[268] = new State(new int[]{8,180,112,-240,111,-240,125,-240,126,-240,127,-240,128,-240,124,-240,6,-240,110,-240,109,-240,122,-240,123,-240,13,-240,115,-240,94,-240,114,-240,9,-240,10,-240,121,-240,104,-240,86,-240,92,-240,95,-240,30,-240,98,-240,29,-240,12,-240,93,-240,81,-240,80,-240,2,-240,79,-240,78,-240,77,-240,76,-240,131,-240});
    states[269] = new State(new int[]{7,163,121,270,117,168,8,-242,112,-242,111,-242,125,-242,126,-242,127,-242,128,-242,124,-242,6,-242,110,-242,109,-242,122,-242,123,-242,13,-242,115,-242,94,-242,114,-242,9,-242,10,-242,104,-242,86,-242,92,-242,95,-242,30,-242,98,-242,29,-242,12,-242,93,-242,81,-242,80,-242,2,-242,79,-242,78,-242,77,-242,76,-242,131,-242},new int[]{-283,619});
    states[270] = new State(new int[]{8,272,137,23,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-263,271,-256,173,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-265,588,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,589,-209,553,-208,554,-285,590});
    states[271] = new State(-277);
    states[272] = new State(new int[]{9,273,137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-74,278,-72,284,-260,287,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[273] = new State(new int[]{121,274,115,-281,94,-281,114,-281,9,-281,10,-281,104,-281,86,-281,92,-281,95,-281,30,-281,98,-281,29,-281,12,-281,93,-281,81,-281,80,-281,2,-281,79,-281,78,-281,77,-281,76,-281,131,-281});
    states[274] = new State(new int[]{8,276,137,23,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-263,275,-256,173,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-265,588,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,589,-209,553,-208,554,-285,590});
    states[275] = new State(-279);
    states[276] = new State(new int[]{9,277,137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-74,278,-72,284,-260,287,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[277] = new State(new int[]{121,274,115,-283,94,-283,114,-283,9,-283,10,-283,104,-283,86,-283,92,-283,95,-283,30,-283,98,-283,29,-283,12,-283,93,-283,81,-283,80,-283,2,-283,79,-283,78,-283,77,-283,76,-283,131,-283});
    states[278] = new State(new int[]{9,279,94,966});
    states[279] = new State(new int[]{121,280,13,-237,115,-237,94,-237,114,-237,9,-237,10,-237,104,-237,86,-237,92,-237,95,-237,30,-237,98,-237,29,-237,12,-237,93,-237,81,-237,80,-237,2,-237,79,-237,78,-237,77,-237,76,-237,131,-237});
    states[280] = new State(new int[]{8,282,137,23,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-263,281,-256,173,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-265,588,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,589,-209,553,-208,554,-285,590});
    states[281] = new State(-280);
    states[282] = new State(new int[]{9,283,137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-74,278,-72,284,-260,287,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[283] = new State(new int[]{121,274,115,-284,94,-284,114,-284,9,-284,10,-284,104,-284,86,-284,92,-284,95,-284,30,-284,98,-284,29,-284,12,-284,93,-284,81,-284,80,-284,2,-284,79,-284,78,-284,77,-284,76,-284,131,-284});
    states[284] = new State(new int[]{94,285});
    states[285] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-72,286,-260,287,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[286] = new State(-249);
    states[287] = new State(new int[]{114,288,94,-251,9,-251});
    states[288] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,289,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[289] = new State(-252);
    states[290] = new State(new int[]{114,291,119,292,117,293,115,294,118,295,116,296,131,297,16,-584,13,-584,86,-584,10,-584,92,-584,95,-584,30,-584,98,-584,29,-584,94,-584,12,-584,9,-584,93,-584,81,-584,80,-584,2,-584,79,-584,78,-584,77,-584,76,-584,5,-584,6,-584,48,-584,55,-584,135,-584,137,-584,75,-584,73,-584,42,-584,39,-584,8,-584,18,-584,19,-584,138,-584,140,-584,139,-584,148,-584,150,-584,149,-584,54,-584,85,-584,37,-584,22,-584,91,-584,51,-584,32,-584,52,-584,96,-584,44,-584,33,-584,50,-584,57,-584,72,-584,70,-584,35,-584,68,-584,69,-584},new int[]{-182,135});
    states[291] = new State(-651);
    states[292] = new State(-652);
    states[293] = new State(-653);
    states[294] = new State(-654);
    states[295] = new State(-655);
    states[296] = new State(-656);
    states[297] = new State(-657);
    states[298] = new State(new int[]{5,299,110,303,109,304,122,305,123,306,120,307,114,-606,119,-606,117,-606,115,-606,118,-606,116,-606,131,-606,16,-606,13,-606,86,-606,10,-606,92,-606,95,-606,30,-606,98,-606,29,-606,94,-606,12,-606,9,-606,93,-606,81,-606,80,-606,2,-606,79,-606,78,-606,77,-606,76,-606,6,-606},new int[]{-183,137});
    states[299] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,-640,86,-640,10,-640,92,-640,95,-640,30,-640,98,-640,29,-640,94,-640,12,-640,9,-640,93,-640,2,-640,79,-640,78,-640,77,-640,76,-640,6,-640},new int[]{-102,300,-93,578,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,577,-251,570});
    states[300] = new State(new int[]{5,301,86,-643,10,-643,92,-643,95,-643,30,-643,98,-643,29,-643,94,-643,12,-643,9,-643,93,-643,81,-643,80,-643,2,-643,79,-643,78,-643,77,-643,76,-643,6,-643});
    states[301] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-93,302,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,577,-251,570});
    states[302] = new State(new int[]{110,303,109,304,122,305,123,306,120,307,86,-645,10,-645,92,-645,95,-645,30,-645,98,-645,29,-645,94,-645,12,-645,9,-645,93,-645,81,-645,80,-645,2,-645,79,-645,78,-645,77,-645,76,-645,6,-645},new int[]{-183,137});
    states[303] = new State(-660);
    states[304] = new State(-661);
    states[305] = new State(-662);
    states[306] = new State(-663);
    states[307] = new State(-664);
    states[308] = new State(new int[]{132,309,130,312,112,314,111,315,125,316,126,317,127,318,128,319,124,320,5,-658,110,-658,109,-658,122,-658,123,-658,120,-658,114,-658,119,-658,117,-658,115,-658,118,-658,116,-658,131,-658,16,-658,13,-658,86,-658,10,-658,92,-658,95,-658,30,-658,98,-658,29,-658,94,-658,12,-658,9,-658,93,-658,81,-658,80,-658,2,-658,79,-658,78,-658,77,-658,76,-658,6,-658,48,-658,55,-658,135,-658,137,-658,75,-658,73,-658,42,-658,39,-658,8,-658,18,-658,19,-658,138,-658,140,-658,139,-658,148,-658,150,-658,149,-658,54,-658,85,-658,37,-658,22,-658,91,-658,51,-658,32,-658,52,-658,96,-658,44,-658,33,-658,50,-658,57,-658,72,-658,70,-658,35,-658,68,-658,69,-658},new int[]{-184,139});
    states[309] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,138,149,140,150,139,152,148,154,150,155,149,156},new int[]{-268,310,-14,311,-166,162,-132,196,-136,24,-137,27,-150,146,-152,147,-151,151,-15,153});
    states[310] = new State(-670);
    states[311] = new State(-671);
    states[312] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-268,313,-166,162,-132,196,-136,24,-137,27});
    states[313] = new State(-669);
    states[314] = new State(-680);
    states[315] = new State(-681);
    states[316] = new State(-682);
    states[317] = new State(-683);
    states[318] = new State(-684);
    states[319] = new State(-685);
    states[320] = new State(-686);
    states[321] = new State(new int[]{132,-674,130,-674,112,-674,111,-674,125,-674,126,-674,127,-674,128,-674,124,-674,5,-674,110,-674,109,-674,122,-674,123,-674,120,-674,114,-674,119,-674,117,-674,115,-674,118,-674,116,-674,131,-674,16,-674,13,-674,86,-674,10,-674,92,-674,95,-674,30,-674,98,-674,29,-674,94,-674,12,-674,9,-674,93,-674,81,-674,80,-674,2,-674,79,-674,78,-674,77,-674,76,-674,6,-674,48,-674,55,-674,135,-674,137,-674,75,-674,73,-674,42,-674,39,-674,8,-674,18,-674,19,-674,138,-674,140,-674,139,-674,148,-674,150,-674,149,-674,54,-674,85,-674,37,-674,22,-674,91,-674,51,-674,32,-674,52,-674,96,-674,44,-674,33,-674,50,-674,57,-674,72,-674,70,-674,35,-674,68,-674,69,-674,113,-672});
    states[322] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573,12,-731},new int[]{-64,323,-71,325,-84,1316,-81,328,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[323] = new State(new int[]{12,324});
    states[324] = new State(-692);
    states[325] = new State(new int[]{94,326,12,-730});
    states[326] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-84,327,-81,328,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[327] = new State(-733);
    states[328] = new State(new int[]{6,329,94,-734,12,-734});
    states[329] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,330,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[330] = new State(-735);
    states[331] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,29,42,359,39,389,8,391,18,247,19,252},new int[]{-88,332,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519});
    states[332] = new State(-693);
    states[333] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,29,42,359,39,389,8,391,18,247,19,252},new int[]{-88,334,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519});
    states[334] = new State(-694);
    states[335] = new State(-154);
    states[336] = new State(-155);
    states[337] = new State(-695);
    states[338] = new State(new int[]{135,1315,137,23,80,25,81,26,75,28,73,29,42,359,39,389,8,391,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156},new int[]{-99,339,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639});
    states[339] = new State(new int[]{17,340,8,346,7,1000,136,1002,4,1003,104,-701,105,-701,106,-701,107,-701,108,-701,86,-701,10,-701,92,-701,95,-701,30,-701,98,-701,132,-701,130,-701,112,-701,111,-701,125,-701,126,-701,127,-701,128,-701,124,-701,5,-701,110,-701,109,-701,122,-701,123,-701,120,-701,114,-701,119,-701,117,-701,115,-701,118,-701,116,-701,131,-701,16,-701,13,-701,29,-701,94,-701,12,-701,9,-701,93,-701,81,-701,80,-701,2,-701,79,-701,78,-701,77,-701,76,-701,113,-701,6,-701,48,-701,55,-701,135,-701,137,-701,75,-701,73,-701,42,-701,39,-701,18,-701,19,-701,138,-701,140,-701,139,-701,148,-701,150,-701,149,-701,54,-701,85,-701,37,-701,22,-701,91,-701,51,-701,32,-701,52,-701,96,-701,44,-701,33,-701,50,-701,57,-701,72,-701,70,-701,35,-701,68,-701,69,-701,11,-712});
    states[340] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-105,341,-93,343,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,577,-251,570});
    states[341] = new State(new int[]{12,342});
    states[342] = new State(-722);
    states[343] = new State(new int[]{5,299,110,303,109,304,122,305,123,306,120,307},new int[]{-183,137});
    states[344] = new State(-704);
    states[345] = new State(new int[]{17,340,8,346,7,1000,136,1002,4,1003,15,1006,104,-702,105,-702,106,-702,107,-702,108,-702,86,-702,10,-702,92,-702,95,-702,30,-702,98,-702,132,-702,130,-702,112,-702,111,-702,125,-702,126,-702,127,-702,128,-702,124,-702,5,-702,110,-702,109,-702,122,-702,123,-702,120,-702,114,-702,119,-702,117,-702,115,-702,118,-702,116,-702,131,-702,16,-702,13,-702,29,-702,94,-702,12,-702,9,-702,93,-702,81,-702,80,-702,2,-702,79,-702,78,-702,77,-702,76,-702,113,-702,6,-702,48,-702,55,-702,135,-702,137,-702,75,-702,73,-702,42,-702,39,-702,18,-702,19,-702,138,-702,140,-702,139,-702,148,-702,150,-702,149,-702,54,-702,85,-702,37,-702,22,-702,91,-702,51,-702,32,-702,52,-702,96,-702,44,-702,33,-702,50,-702,57,-702,72,-702,70,-702,35,-702,68,-702,69,-702,11,-712});
    states[346] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,502,18,247,19,252,5,573,34,625,41,856,9,-729},new int[]{-63,347,-66,349,-82,501,-81,126,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,352,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572,-305,623,-306,624});
    states[347] = new State(new int[]{9,348});
    states[348] = new State(-723);
    states[349] = new State(new int[]{94,350,12,-728,9,-728});
    states[350] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,502,18,247,19,252,5,573,34,625,41,856},new int[]{-82,351,-81,126,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,352,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572,-305,623,-306,624});
    states[351] = new State(-573);
    states[352] = new State(new int[]{121,353,17,-714,8,-714,7,-714,136,-714,4,-714,15,-714,132,-714,130,-714,112,-714,111,-714,125,-714,126,-714,127,-714,128,-714,124,-714,5,-714,110,-714,109,-714,122,-714,123,-714,120,-714,114,-714,119,-714,117,-714,115,-714,118,-714,116,-714,131,-714,16,-714,13,-714,86,-714,10,-714,92,-714,95,-714,30,-714,98,-714,29,-714,94,-714,12,-714,9,-714,93,-714,81,-714,80,-714,2,-714,79,-714,78,-714,77,-714,76,-714,113,-714,11,-714});
    states[353] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-311,354,-91,355,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-313,629,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[354] = new State(-877);
    states[355] = new State(new int[]{13,128,86,-912,10,-912,92,-912,95,-912,30,-912,98,-912,29,-912,94,-912,12,-912,9,-912,93,-912,81,-912,80,-912,2,-912,79,-912,78,-912,77,-912,76,-912});
    states[356] = new State(new int[]{110,303,109,304,122,305,123,306,120,307,114,-606,119,-606,117,-606,115,-606,118,-606,116,-606,131,-606,16,-606,5,-606,13,-606,86,-606,10,-606,92,-606,95,-606,30,-606,98,-606,29,-606,94,-606,12,-606,9,-606,93,-606,81,-606,80,-606,2,-606,79,-606,78,-606,77,-606,76,-606,6,-606,48,-606,55,-606,135,-606,137,-606,75,-606,73,-606,42,-606,39,-606,8,-606,18,-606,19,-606,138,-606,140,-606,139,-606,148,-606,150,-606,149,-606,54,-606,85,-606,37,-606,22,-606,91,-606,51,-606,32,-606,52,-606,96,-606,44,-606,33,-606,50,-606,57,-606,72,-606,70,-606,35,-606,68,-606,69,-606},new int[]{-183,137});
    states[357] = new State(-714);
    states[358] = new State(-715);
    states[359] = new State(new int[]{109,361,110,362,111,363,112,364,114,365,115,366,116,367,117,368,118,369,119,370,122,371,123,372,124,373,125,374,126,375,127,376,128,377,129,378,131,379,133,380,134,381,104,383,105,384,106,385,107,386,108,387,113,388},new int[]{-186,360,-180,382});
    states[360] = new State(-742);
    states[361] = new State(-849);
    states[362] = new State(-850);
    states[363] = new State(-851);
    states[364] = new State(-852);
    states[365] = new State(-853);
    states[366] = new State(-854);
    states[367] = new State(-855);
    states[368] = new State(-856);
    states[369] = new State(-857);
    states[370] = new State(-858);
    states[371] = new State(-859);
    states[372] = new State(-860);
    states[373] = new State(-861);
    states[374] = new State(-862);
    states[375] = new State(-863);
    states[376] = new State(-864);
    states[377] = new State(-865);
    states[378] = new State(-866);
    states[379] = new State(-867);
    states[380] = new State(-868);
    states[381] = new State(-869);
    states[382] = new State(-870);
    states[383] = new State(-872);
    states[384] = new State(-873);
    states[385] = new State(-874);
    states[386] = new State(-875);
    states[387] = new State(-876);
    states[388] = new State(-871);
    states[389] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-132,390,-136,24,-137,27});
    states[390] = new State(-716);
    states[391] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,392,-91,394,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[392] = new State(new int[]{9,393});
    states[393] = new State(-717);
    states[394] = new State(new int[]{94,395,13,128,9,-578});
    states[395] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-73,396,-91,974,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[396] = new State(new int[]{94,972,5,408,10,-896,9,-896},new int[]{-307,397});
    states[397] = new State(new int[]{10,400,9,-884},new int[]{-314,398});
    states[398] = new State(new int[]{9,399});
    states[399] = new State(-688);
    states[400] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-309,401,-310,871,-143,404,-132,720,-136,24,-137,27});
    states[401] = new State(new int[]{10,402,9,-885});
    states[402] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-310,403,-143,404,-132,720,-136,24,-137,27});
    states[403] = new State(-894);
    states[404] = new State(new int[]{94,406,5,408,10,-896,9,-896},new int[]{-307,405});
    states[405] = new State(-895);
    states[406] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-132,407,-136,24,-137,27});
    states[407] = new State(-333);
    states[408] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-259,409,-260,410,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[409] = new State(-897);
    states[410] = new State(-468);
    states[411] = new State(new int[]{13,412,114,-214,94,-214,9,-214,10,-214,121,-214,115,-214,104,-214,86,-214,92,-214,95,-214,30,-214,98,-214,29,-214,12,-214,93,-214,81,-214,80,-214,2,-214,79,-214,78,-214,77,-214,76,-214,131,-214});
    states[412] = new State(-213);
    states[413] = new State(new int[]{11,414,7,-749,121,-749,117,-749,8,-749,112,-749,111,-749,125,-749,126,-749,127,-749,128,-749,124,-749,6,-749,110,-749,109,-749,122,-749,123,-749,13,-749,114,-749,94,-749,9,-749,10,-749,115,-749,104,-749,86,-749,92,-749,95,-749,30,-749,98,-749,29,-749,12,-749,93,-749,81,-749,80,-749,2,-749,79,-749,78,-749,77,-749,76,-749,131,-749});
    states[414] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-83,415,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[415] = new State(new int[]{12,416,13,187});
    states[416] = new State(-272);
    states[417] = new State(-146);
    states[418] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336,12,-172},new int[]{-69,419,-67,183,-86,421,-83,186,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[419] = new State(new int[]{12,420});
    states[420] = new State(-153);
    states[421] = new State(-173);
    states[422] = new State(-147);
    states[423] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-10,424,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432});
    states[424] = new State(-148);
    states[425] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-83,426,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[426] = new State(new int[]{9,427,13,187});
    states[427] = new State(-149);
    states[428] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-10,429,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432});
    states[429] = new State(-150);
    states[430] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-10,431,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432});
    states[431] = new State(-151);
    states[432] = new State(-152);
    states[433] = new State(-134);
    states[434] = new State(-135);
    states[435] = new State(-116);
    states[436] = new State(-243);
    states[437] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152},new int[]{-95,438,-166,439,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151});
    states[438] = new State(new int[]{8,180,112,-244,111,-244,125,-244,126,-244,127,-244,128,-244,124,-244,6,-244,110,-244,109,-244,122,-244,123,-244,13,-244,115,-244,94,-244,114,-244,9,-244,10,-244,121,-244,104,-244,86,-244,92,-244,95,-244,30,-244,98,-244,29,-244,12,-244,93,-244,81,-244,80,-244,2,-244,79,-244,78,-244,77,-244,76,-244,131,-244});
    states[439] = new State(new int[]{7,163,8,-242,112,-242,111,-242,125,-242,126,-242,127,-242,128,-242,124,-242,6,-242,110,-242,109,-242,122,-242,123,-242,13,-242,115,-242,94,-242,114,-242,9,-242,10,-242,121,-242,104,-242,86,-242,92,-242,95,-242,30,-242,98,-242,29,-242,12,-242,93,-242,81,-242,80,-242,2,-242,79,-242,78,-242,77,-242,76,-242,131,-242});
    states[440] = new State(-245);
    states[441] = new State(new int[]{9,442,137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-74,278,-72,284,-260,287,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[442] = new State(new int[]{121,274});
    states[443] = new State(-215);
    states[444] = new State(-216);
    states[445] = new State(-217);
    states[446] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-259,447,-260,410,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[447] = new State(-253);
    states[448] = new State(-218);
    states[449] = new State(-254);
    states[450] = new State(-256);
    states[451] = new State(new int[]{11,452,55,1313});
    states[452] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,963,12,-268,94,-268},new int[]{-149,453,-255,1312,-256,1311,-85,175,-94,267,-95,268,-166,439,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151});
    states[453] = new State(new int[]{12,454,94,1309});
    states[454] = new State(new int[]{55,455});
    states[455] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-260,456,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[456] = new State(-262);
    states[457] = new State(-263);
    states[458] = new State(-257);
    states[459] = new State(new int[]{8,1184,20,-304,11,-304,86,-304,79,-304,78,-304,77,-304,76,-304,26,-304,137,-304,80,-304,81,-304,75,-304,73,-304,59,-304,25,-304,23,-304,41,-304,34,-304,27,-304,28,-304,43,-304,24,-304},new int[]{-169,460});
    states[460] = new State(new int[]{20,1175,11,-311,86,-311,79,-311,78,-311,77,-311,76,-311,26,-311,137,-311,80,-311,81,-311,75,-311,73,-311,59,-311,25,-311,23,-311,41,-311,34,-311,27,-311,28,-311,43,-311,24,-311},new int[]{-300,461,-299,1173,-298,1195});
    states[461] = new State(new int[]{11,611,86,-328,79,-328,78,-328,77,-328,76,-328,26,-200,137,-200,80,-200,81,-200,75,-200,73,-200,59,-200,25,-200,23,-200,41,-200,34,-200,27,-200,28,-200,43,-200,24,-200},new int[]{-22,462,-29,1153,-31,466,-41,1154,-6,1155,-234,949,-30,1265,-50,1267,-49,472,-51,1266});
    states[462] = new State(new int[]{86,463,79,1149,78,1150,77,1151,76,1152},new int[]{-7,464});
    states[463] = new State(-286);
    states[464] = new State(new int[]{11,611,86,-328,79,-328,78,-328,77,-328,76,-328,26,-200,137,-200,80,-200,81,-200,75,-200,73,-200,59,-200,25,-200,23,-200,41,-200,34,-200,27,-200,28,-200,43,-200,24,-200},new int[]{-29,465,-31,466,-41,1154,-6,1155,-234,949,-30,1265,-50,1267,-49,472,-51,1266});
    states[465] = new State(-323);
    states[466] = new State(new int[]{10,468,86,-334,79,-334,78,-334,77,-334,76,-334},new int[]{-176,467});
    states[467] = new State(-329);
    states[468] = new State(new int[]{11,611,86,-335,79,-335,78,-335,77,-335,76,-335,26,-200,137,-200,80,-200,81,-200,75,-200,73,-200,59,-200,25,-200,23,-200,41,-200,34,-200,27,-200,28,-200,43,-200,24,-200},new int[]{-41,469,-30,470,-6,1155,-234,949,-50,1267,-49,472,-51,1266});
    states[469] = new State(-337);
    states[470] = new State(new int[]{11,611,86,-331,79,-331,78,-331,77,-331,76,-331,25,-200,23,-200,41,-200,34,-200,27,-200,28,-200,43,-200,24,-200},new int[]{-50,471,-49,472,-6,473,-234,949,-51,1266});
    states[471] = new State(-340);
    states[472] = new State(-341);
    states[473] = new State(new int[]{25,1222,23,1223,41,1168,34,1203,27,1237,28,1244,11,611,43,1251,24,1260},new int[]{-207,474,-234,475,-204,476,-242,477,-3,478,-215,1224,-213,1097,-210,1167,-214,1202,-212,1225,-200,1248,-201,1249,-203,1250});
    states[474] = new State(-350);
    states[475] = new State(-199);
    states[476] = new State(-351);
    states[477] = new State(-369);
    states[478] = new State(new int[]{27,480,43,1050,24,1092,41,1168,34,1203},new int[]{-215,479,-201,1049,-213,1097,-210,1167,-214,1202});
    states[479] = new State(-354);
    states[480] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359,8,-364,104,-364,10,-364},new int[]{-157,481,-156,1032,-155,1033,-127,1034,-122,1035,-119,1036,-132,1041,-136,24,-137,27,-177,1042,-317,1044,-134,1048});
    states[481] = new State(new int[]{8,557,104,-452,10,-452},new int[]{-113,482});
    states[482] = new State(new int[]{104,484,10,1021},new int[]{-193,483});
    states[483] = new State(-361);
    states[484] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476},new int[]{-244,485,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[485] = new State(new int[]{10,486});
    states[486] = new State(-411);
    states[487] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,86,-554,10,-554,92,-554,95,-554,30,-554,98,-554,29,-554,94,-554,12,-554,9,-554,93,-554,2,-554,79,-554,78,-554,77,-554,76,-554},new int[]{-132,390,-136,24,-137,27});
    states[488] = new State(new int[]{50,1009,53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,392,-91,394,-99,489,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[489] = new State(new int[]{94,490,17,340,8,346,7,1000,136,1002,4,1003,15,1006,132,-702,130,-702,112,-702,111,-702,125,-702,126,-702,127,-702,128,-702,124,-702,5,-702,110,-702,109,-702,122,-702,123,-702,120,-702,114,-702,119,-702,117,-702,115,-702,118,-702,116,-702,131,-702,16,-702,13,-702,9,-702,113,-702,11,-712});
    states[490] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359,39,389,8,391,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156},new int[]{-319,491,-99,1005,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639});
    states[491] = new State(new int[]{9,492,94,998});
    states[492] = new State(new int[]{104,383,105,384,106,385,107,386,108,387},new int[]{-180,493});
    states[493] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,494,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[494] = new State(-505);
    states[495] = new State(-718);
    states[496] = new State(-719);
    states[497] = new State(new int[]{11,498});
    states[498] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,502,18,247,19,252,5,573,34,625,41,856},new int[]{-66,499,-82,501,-81,126,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,352,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572,-305,623,-306,624});
    states[499] = new State(new int[]{12,500,94,350});
    states[500] = new State(-721);
    states[501] = new State(-572);
    states[502] = new State(new int[]{9,975,53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,392,-91,503,-132,979,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[503] = new State(new int[]{94,504,13,128,9,-578});
    states[504] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-73,505,-91,974,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[505] = new State(new int[]{94,972,5,408,10,-896,9,-896},new int[]{-307,506});
    states[506] = new State(new int[]{10,400,9,-884},new int[]{-314,507});
    states[507] = new State(new int[]{9,508});
    states[508] = new State(new int[]{5,959,7,-688,132,-688,130,-688,112,-688,111,-688,125,-688,126,-688,127,-688,128,-688,124,-688,110,-688,109,-688,122,-688,123,-688,120,-688,114,-688,119,-688,117,-688,115,-688,118,-688,116,-688,131,-688,16,-688,13,-688,86,-688,10,-688,92,-688,95,-688,30,-688,98,-688,29,-688,94,-688,12,-688,9,-688,93,-688,81,-688,80,-688,2,-688,79,-688,78,-688,77,-688,76,-688,113,-688,121,-898},new int[]{-318,509,-308,510});
    states[509] = new State(-882);
    states[510] = new State(new int[]{121,511});
    states[511] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-311,512,-91,355,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-313,629,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[512] = new State(-886);
    states[513] = new State(new int[]{7,514,132,-696,130,-696,112,-696,111,-696,125,-696,126,-696,127,-696,128,-696,124,-696,5,-696,110,-696,109,-696,122,-696,123,-696,120,-696,114,-696,119,-696,117,-696,115,-696,118,-696,116,-696,131,-696,16,-696,13,-696,86,-696,10,-696,92,-696,95,-696,30,-696,98,-696,29,-696,94,-696,12,-696,9,-696,93,-696,81,-696,80,-696,2,-696,79,-696,78,-696,77,-696,76,-696,113,-696,6,-696,48,-696,55,-696,135,-696,137,-696,75,-696,73,-696,42,-696,39,-696,8,-696,18,-696,19,-696,138,-696,140,-696,139,-696,148,-696,150,-696,149,-696,54,-696,85,-696,37,-696,22,-696,91,-696,51,-696,32,-696,52,-696,96,-696,44,-696,33,-696,50,-696,57,-696,72,-696,70,-696,35,-696,68,-696,69,-696});
    states[514] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,30,64,31,65,24,66,98,67,95,68,32,69,33,70,34,71,37,72,38,73,39,74,97,75,40,76,41,77,43,78,44,79,45,80,91,81,46,82,96,83,47,84,25,85,48,86,68,87,92,88,49,89,50,90,51,91,52,92,53,93,54,94,55,95,56,96,58,97,99,98,100,99,103,100,101,101,102,102,59,103,72,104,35,105,36,106,42,359},new int[]{-133,515,-132,516,-136,24,-137,27,-277,517,-135,31,-177,518});
    states[515] = new State(-725);
    states[516] = new State(-755);
    states[517] = new State(-756);
    states[518] = new State(-757);
    states[519] = new State(-703);
    states[520] = new State(-675);
    states[521] = new State(-676);
    states[522] = new State(new int[]{113,523});
    states[523] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,29,42,359,39,389,8,391,18,247,19,252},new int[]{-88,524,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519});
    states[524] = new State(-673);
    states[525] = new State(-679);
    states[526] = new State(new int[]{8,527,132,-667,130,-667,112,-667,111,-667,125,-667,126,-667,127,-667,128,-667,124,-667,5,-667,110,-667,109,-667,122,-667,123,-667,120,-667,114,-667,119,-667,117,-667,115,-667,118,-667,116,-667,131,-667,16,-667,13,-667,86,-667,10,-667,92,-667,95,-667,30,-667,98,-667,29,-667,94,-667,12,-667,9,-667,93,-667,81,-667,80,-667,2,-667,79,-667,78,-667,77,-667,76,-667,6,-667,48,-667,55,-667,135,-667,137,-667,75,-667,73,-667,42,-667,39,-667,18,-667,19,-667,138,-667,140,-667,139,-667,148,-667,150,-667,149,-667,54,-667,85,-667,37,-667,22,-667,91,-667,51,-667,32,-667,52,-667,96,-667,44,-667,33,-667,50,-667,57,-667,72,-667,70,-667,35,-667,68,-667,69,-667});
    states[527] = new State(new int[]{14,532,138,149,140,150,139,152,148,154,150,155,149,156,50,534,137,23,80,25,81,26,75,28,73,29},new int[]{-332,528,-328,958,-14,533,-150,146,-152,147,-151,151,-15,153,-322,951,-268,952,-166,162,-132,196,-136,24,-137,27});
    states[528] = new State(new int[]{9,529,10,530,94,956});
    states[529] = new State(-608);
    states[530] = new State(new int[]{14,532,138,149,140,150,139,152,148,154,150,155,149,156,50,534,137,23,80,25,81,26,75,28,73,29},new int[]{-328,531,-14,533,-150,146,-152,147,-151,151,-15,153,-322,951,-268,952,-166,162,-132,196,-136,24,-137,27});
    states[531] = new State(-625);
    states[532] = new State(-627);
    states[533] = new State(-628);
    states[534] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-132,535,-136,24,-137,27});
    states[535] = new State(new int[]{5,536,9,-630,10,-630,94,-630});
    states[536] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-260,537,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[537] = new State(-629);
    states[538] = new State(-258);
    states[539] = new State(new int[]{55,540});
    states[540] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-260,541,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[541] = new State(-269);
    states[542] = new State(-259);
    states[543] = new State(new int[]{55,544,115,-271,94,-271,114,-271,9,-271,10,-271,121,-271,104,-271,86,-271,92,-271,95,-271,30,-271,98,-271,29,-271,12,-271,93,-271,81,-271,80,-271,2,-271,79,-271,78,-271,77,-271,76,-271,131,-271});
    states[544] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-260,545,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[545] = new State(-270);
    states[546] = new State(-260);
    states[547] = new State(new int[]{55,548});
    states[548] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-260,549,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[549] = new State(-261);
    states[550] = new State(new int[]{21,451,45,459,46,539,31,543,71,547},new int[]{-266,551,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546});
    states[551] = new State(-255);
    states[552] = new State(-219);
    states[553] = new State(-273);
    states[554] = new State(-274);
    states[555] = new State(new int[]{8,557,115,-452,94,-452,114,-452,9,-452,10,-452,121,-452,104,-452,86,-452,92,-452,95,-452,30,-452,98,-452,29,-452,12,-452,93,-452,81,-452,80,-452,2,-452,79,-452,78,-452,77,-452,76,-452,131,-452},new int[]{-113,556});
    states[556] = new State(-275);
    states[557] = new State(new int[]{9,558,11,611,137,-200,80,-200,81,-200,75,-200,73,-200,50,-200,26,-200,102,-200},new int[]{-114,559,-52,950,-6,563,-234,949});
    states[558] = new State(-453);
    states[559] = new State(new int[]{9,560,10,561});
    states[560] = new State(-454);
    states[561] = new State(new int[]{11,611,137,-200,80,-200,81,-200,75,-200,73,-200,50,-200,26,-200,102,-200},new int[]{-52,562,-6,563,-234,949});
    states[562] = new State(-456);
    states[563] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,50,595,26,601,102,607,11,611},new int[]{-280,564,-234,475,-144,565,-120,594,-132,593,-136,24,-137,27});
    states[564] = new State(-457);
    states[565] = new State(new int[]{5,566,94,591});
    states[566] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-259,567,-260,410,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[567] = new State(new int[]{104,568,9,-458,10,-458});
    states[568] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,569,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[569] = new State(-462);
    states[570] = new State(-668);
    states[571] = new State(-581);
    states[572] = new State(-579);
    states[573] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,-640,86,-640,10,-640,92,-640,95,-640,30,-640,98,-640,29,-640,94,-640,12,-640,9,-640,93,-640,2,-640,79,-640,78,-640,77,-640,76,-640,6,-640},new int[]{-102,574,-93,578,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,577,-251,570});
    states[574] = new State(new int[]{5,575,86,-644,10,-644,92,-644,95,-644,30,-644,98,-644,29,-644,94,-644,12,-644,9,-644,93,-644,81,-644,80,-644,2,-644,79,-644,78,-644,77,-644,76,-644,6,-644});
    states[575] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-93,576,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,577,-251,570});
    states[576] = new State(new int[]{110,303,109,304,122,305,123,306,120,307,86,-646,10,-646,92,-646,95,-646,30,-646,98,-646,29,-646,94,-646,12,-646,9,-646,93,-646,81,-646,80,-646,2,-646,79,-646,78,-646,77,-646,76,-646,6,-646},new int[]{-183,137});
    states[577] = new State(-667);
    states[578] = new State(new int[]{110,303,109,304,122,305,123,306,120,307,5,-639,86,-639,10,-639,92,-639,95,-639,30,-639,98,-639,29,-639,94,-639,12,-639,9,-639,93,-639,81,-639,80,-639,2,-639,79,-639,78,-639,77,-639,76,-639,6,-639},new int[]{-183,137});
    states[579] = new State(new int[]{8,557,5,-452},new int[]{-113,580});
    states[580] = new State(new int[]{5,581});
    states[581] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-259,582,-260,410,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[582] = new State(-276);
    states[583] = new State(new int[]{121,584,114,-220,94,-220,9,-220,10,-220,115,-220,104,-220,86,-220,92,-220,95,-220,30,-220,98,-220,29,-220,12,-220,93,-220,81,-220,80,-220,2,-220,79,-220,78,-220,77,-220,76,-220,131,-220});
    states[584] = new State(new int[]{8,586,137,23,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-263,585,-256,173,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-265,588,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,589,-209,553,-208,554,-285,590});
    states[585] = new State(-278);
    states[586] = new State(new int[]{9,587,137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-74,278,-72,284,-260,287,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[587] = new State(new int[]{121,274,115,-282,94,-282,114,-282,9,-282,10,-282,104,-282,86,-282,92,-282,95,-282,30,-282,98,-282,29,-282,12,-282,93,-282,81,-282,80,-282,2,-282,79,-282,78,-282,77,-282,76,-282,131,-282});
    states[588] = new State(-232);
    states[589] = new State(-233);
    states[590] = new State(new int[]{121,584,115,-234,94,-234,114,-234,9,-234,10,-234,104,-234,86,-234,92,-234,95,-234,30,-234,98,-234,29,-234,12,-234,93,-234,81,-234,80,-234,2,-234,79,-234,78,-234,77,-234,76,-234,131,-234});
    states[591] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-120,592,-132,593,-136,24,-137,27});
    states[592] = new State(-466);
    states[593] = new State(-467);
    states[594] = new State(-465);
    states[595] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-144,596,-120,594,-132,593,-136,24,-137,27});
    states[596] = new State(new int[]{5,597,94,591});
    states[597] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-259,598,-260,410,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[598] = new State(new int[]{104,599,9,-459,10,-459});
    states[599] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,600,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[600] = new State(-463);
    states[601] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-144,602,-120,594,-132,593,-136,24,-137,27});
    states[602] = new State(new int[]{5,603,94,591});
    states[603] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-259,604,-260,410,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[604] = new State(new int[]{104,605,9,-460,10,-460});
    states[605] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,606,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[606] = new State(-464);
    states[607] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-144,608,-120,594,-132,593,-136,24,-137,27});
    states[608] = new State(new int[]{5,609,94,591});
    states[609] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-259,610,-260,410,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[610] = new State(-461);
    states[611] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-235,612,-8,948,-9,616,-166,617,-132,943,-136,24,-137,27,-285,946});
    states[612] = new State(new int[]{12,613,94,614});
    states[613] = new State(-201);
    states[614] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-8,615,-9,616,-166,617,-132,943,-136,24,-137,27,-285,946});
    states[615] = new State(-203);
    states[616] = new State(-204);
    states[617] = new State(new int[]{7,163,8,620,117,168,12,-604,94,-604},new int[]{-65,618,-283,619});
    states[618] = new State(-706);
    states[619] = new State(-221);
    states[620] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,502,18,247,19,252,5,573,34,625,41,856,9,-729},new int[]{-63,621,-66,349,-82,501,-81,126,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,352,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572,-305,623,-306,624});
    states[621] = new State(new int[]{9,622});
    states[622] = new State(-605);
    states[623] = new State(-577);
    states[624] = new State(-883);
    states[625] = new State(new int[]{8,933,5,408,121,-896},new int[]{-307,626});
    states[626] = new State(new int[]{121,627});
    states[627] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-311,628,-91,355,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-313,629,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[628] = new State(-887);
    states[629] = new State(-913);
    states[630] = new State(-900);
    states[631] = new State(-901);
    states[632] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,633,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[633] = new State(new int[]{48,634,13,128});
    states[634] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,29,-476,94,-476,12,-476,9,-476,93,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-244,635,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[635] = new State(new int[]{29,636,86,-515,10,-515,92,-515,95,-515,30,-515,98,-515,94,-515,12,-515,9,-515,93,-515,81,-515,80,-515,2,-515,79,-515,78,-515,77,-515,76,-515});
    states[636] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,29,-476,94,-476,12,-476,9,-476,93,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-244,637,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[637] = new State(-516);
    states[638] = new State(new int[]{7,144,11,-713});
    states[639] = new State(new int[]{7,514});
    states[640] = new State(-478);
    states[641] = new State(-479);
    states[642] = new State(new int[]{148,644,149,645,137,23,80,25,81,26,75,28,73,29},new int[]{-128,643,-132,646,-136,24,-137,27});
    states[643] = new State(-511);
    states[644] = new State(-92);
    states[645] = new State(-93);
    states[646] = new State(-94);
    states[647] = new State(-480);
    states[648] = new State(-481);
    states[649] = new State(-482);
    states[650] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,651,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[651] = new State(new int[]{55,652,13,128});
    states[652] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336,29,660,86,-534},new int[]{-33,653,-237,930,-246,932,-68,923,-98,929,-86,928,-83,186,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[653] = new State(new int[]{10,656,29,660,86,-534},new int[]{-237,654});
    states[654] = new State(new int[]{86,655});
    states[655] = new State(-525);
    states[656] = new State(new int[]{29,660,137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336,86,-534},new int[]{-237,657,-246,659,-68,923,-98,929,-86,928,-83,186,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[657] = new State(new int[]{86,658});
    states[658] = new State(-526);
    states[659] = new State(-529);
    states[660] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,664,150,155,149,665,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476,86,-476},new int[]{-236,661,-245,662,-244,121,-4,122,-100,123,-117,338,-99,345,-132,663,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754,-128,890});
    states[661] = new State(new int[]{10,119,86,-535});
    states[662] = new State(-513);
    states[663] = new State(new int[]{17,-714,8,-714,7,-714,136,-714,4,-714,15,-714,104,-714,105,-714,106,-714,107,-714,108,-714,86,-714,10,-714,11,-714,92,-714,95,-714,30,-714,98,-714,5,-94});
    states[664] = new State(new int[]{7,-177,11,-177,5,-92});
    states[665] = new State(new int[]{7,-179,11,-179,5,-93});
    states[666] = new State(-483);
    states[667] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,664,150,155,149,665,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,92,-476,10,-476},new int[]{-236,668,-245,662,-244,121,-4,122,-100,123,-117,338,-99,345,-132,663,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754,-128,890});
    states[668] = new State(new int[]{92,669,10,119});
    states[669] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,670,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[670] = new State(-536);
    states[671] = new State(-484);
    states[672] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,673,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[673] = new State(new int[]{13,128,93,915,135,-539,137,-539,80,-539,81,-539,75,-539,73,-539,42,-539,39,-539,8,-539,18,-539,19,-539,138,-539,140,-539,139,-539,148,-539,150,-539,149,-539,54,-539,85,-539,37,-539,22,-539,91,-539,51,-539,32,-539,52,-539,96,-539,44,-539,33,-539,50,-539,57,-539,72,-539,70,-539,35,-539,86,-539,10,-539,92,-539,95,-539,30,-539,98,-539,29,-539,94,-539,12,-539,9,-539,2,-539,79,-539,78,-539,77,-539,76,-539},new int[]{-276,674});
    states[674] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,29,-476,94,-476,12,-476,9,-476,93,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-244,675,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[675] = new State(-537);
    states[676] = new State(-485);
    states[677] = new State(new int[]{50,922,137,-548,80,-548,81,-548,75,-548,73,-548},new int[]{-18,678});
    states[678] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-132,679,-136,24,-137,27});
    states[679] = new State(new int[]{104,918,5,919},new int[]{-270,680});
    states[680] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,681,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[681] = new State(new int[]{13,128,68,916,69,917},new int[]{-104,682});
    states[682] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,683,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[683] = new State(new int[]{13,128,93,915,135,-539,137,-539,80,-539,81,-539,75,-539,73,-539,42,-539,39,-539,8,-539,18,-539,19,-539,138,-539,140,-539,139,-539,148,-539,150,-539,149,-539,54,-539,85,-539,37,-539,22,-539,91,-539,51,-539,32,-539,52,-539,96,-539,44,-539,33,-539,50,-539,57,-539,72,-539,70,-539,35,-539,86,-539,10,-539,92,-539,95,-539,30,-539,98,-539,29,-539,94,-539,12,-539,9,-539,2,-539,79,-539,78,-539,77,-539,76,-539},new int[]{-276,684});
    states[684] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,29,-476,94,-476,12,-476,9,-476,93,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-244,685,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[685] = new State(-546);
    states[686] = new State(-486);
    states[687] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,502,18,247,19,252,5,573,34,625,41,856},new int[]{-66,688,-82,501,-81,126,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,352,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572,-305,623,-306,624});
    states[688] = new State(new int[]{93,689,94,350});
    states[689] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,29,-476,94,-476,12,-476,9,-476,93,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-244,690,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[690] = new State(-553);
    states[691] = new State(-487);
    states[692] = new State(-488);
    states[693] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,664,150,155,149,665,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476,95,-476,30,-476},new int[]{-236,694,-245,662,-244,121,-4,122,-100,123,-117,338,-99,345,-132,663,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754,-128,890});
    states[694] = new State(new int[]{10,119,95,696,30,893},new int[]{-274,695});
    states[695] = new State(-555);
    states[696] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,664,150,155,149,665,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476},new int[]{-236,697,-245,662,-244,121,-4,122,-100,123,-117,338,-99,345,-132,663,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754,-128,890});
    states[697] = new State(new int[]{86,698,10,119});
    states[698] = new State(-556);
    states[699] = new State(-489);
    states[700] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573,86,-570,10,-570,92,-570,95,-570,30,-570,98,-570,29,-570,94,-570,12,-570,9,-570,93,-570,2,-570,79,-570,78,-570,77,-570,76,-570},new int[]{-81,701,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[701] = new State(-571);
    states[702] = new State(-490);
    states[703] = new State(new int[]{50,878,137,23,80,25,81,26,75,28,73,29},new int[]{-132,704,-136,24,-137,27});
    states[704] = new State(new int[]{5,876,131,-545},new int[]{-258,705});
    states[705] = new State(new int[]{131,706});
    states[706] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,707,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[707] = new State(new int[]{93,708,13,128});
    states[708] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,29,-476,94,-476,12,-476,9,-476,93,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-244,709,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[709] = new State(-541);
    states[710] = new State(-491);
    states[711] = new State(new int[]{8,713,137,23,80,25,81,26,75,28,73,29},new int[]{-294,712,-143,721,-132,720,-136,24,-137,27});
    states[712] = new State(-501);
    states[713] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-132,714,-136,24,-137,27});
    states[714] = new State(new int[]{94,715});
    states[715] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-143,716,-132,720,-136,24,-137,27});
    states[716] = new State(new int[]{9,717,94,406});
    states[717] = new State(new int[]{104,718});
    states[718] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,719,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[719] = new State(-503);
    states[720] = new State(-332);
    states[721] = new State(new int[]{5,722,94,406,104,874});
    states[722] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-260,723,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[723] = new State(new int[]{104,872,114,873,86,-396,10,-396,92,-396,95,-396,30,-396,98,-396,29,-396,94,-396,12,-396,9,-396,93,-396,81,-396,80,-396,2,-396,79,-396,78,-396,77,-396,76,-396},new int[]{-321,724});
    states[724] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,823,129,428,110,335,109,336,60,158,34,625,41,856},new int[]{-80,725,-79,726,-78,240,-83,241,-75,191,-12,215,-10,225,-13,201,-132,727,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435,-87,851,-227,852,-53,844,-306,855});
    states[725] = new State(-398);
    states[726] = new State(-399);
    states[727] = new State(new int[]{121,728,4,-156,11,-156,7,-156,136,-156,8,-156,113,-156,130,-156,132,-156,112,-156,111,-156,125,-156,126,-156,127,-156,128,-156,124,-156,110,-156,109,-156,122,-156,123,-156,114,-156,119,-156,117,-156,115,-156,118,-156,116,-156,131,-156,13,-156,86,-156,10,-156,92,-156,95,-156,30,-156,98,-156,29,-156,94,-156,12,-156,9,-156,93,-156,81,-156,80,-156,2,-156,79,-156,78,-156,77,-156,76,-156});
    states[728] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-311,729,-91,355,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-313,629,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[729] = new State(-401);
    states[730] = new State(-902);
    states[731] = new State(-903);
    states[732] = new State(-904);
    states[733] = new State(-905);
    states[734] = new State(-906);
    states[735] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,736,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[736] = new State(new int[]{93,737,13,128});
    states[737] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,29,-476,94,-476,12,-476,9,-476,93,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-244,738,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[738] = new State(-498);
    states[739] = new State(-492);
    states[740] = new State(-574);
    states[741] = new State(-575);
    states[742] = new State(-493);
    states[743] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,744,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[744] = new State(new int[]{93,745,13,128});
    states[745] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,29,-476,94,-476,12,-476,9,-476,93,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-244,746,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[746] = new State(-540);
    states[747] = new State(-494);
    states[748] = new State(new int[]{71,750,53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,749,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[749] = new State(new int[]{13,128,86,-499,10,-499,92,-499,95,-499,30,-499,98,-499,29,-499,94,-499,12,-499,9,-499,93,-499,81,-499,80,-499,2,-499,79,-499,78,-499,77,-499,76,-499});
    states[750] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,751,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[751] = new State(new int[]{13,128,86,-500,10,-500,92,-500,95,-500,30,-500,98,-500,29,-500,94,-500,12,-500,9,-500,93,-500,81,-500,80,-500,2,-500,79,-500,78,-500,77,-500,76,-500});
    states[752] = new State(-495);
    states[753] = new State(-496);
    states[754] = new State(-497);
    states[755] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,756,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[756] = new State(new int[]{52,757,13,128});
    states[757] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,138,149,140,150,139,152,148,154,150,155,149,156,8,803},new int[]{-327,758,-326,816,-323,765,-268,772,-166,162,-132,196,-136,24,-137,27,-324,791,-331,798,-334,815,-14,801,-150,146,-152,147,-151,151,-15,153,-335,802});
    states[758] = new State(new int[]{10,761,29,660,86,-534},new int[]{-237,759});
    states[759] = new State(new int[]{86,760});
    states[760] = new State(-517);
    states[761] = new State(new int[]{29,660,137,23,80,25,81,26,75,28,73,29,138,149,140,150,139,152,148,154,150,155,149,156,8,803,86,-534},new int[]{-237,762,-326,764,-323,765,-268,772,-166,162,-132,196,-136,24,-137,27,-324,791,-331,798,-334,815,-14,801,-150,146,-152,147,-151,151,-15,153,-335,802});
    states[762] = new State(new int[]{86,763});
    states[763] = new State(-518);
    states[764] = new State(-520);
    states[765] = new State(new int[]{36,766,5,770});
    states[766] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,767,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[767] = new State(new int[]{5,768,13,128});
    states[768] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476,29,-476,86,-476},new int[]{-244,769,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[769] = new State(-521);
    states[770] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476,29,-476,86,-476},new int[]{-244,771,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[771] = new State(-522);
    states[772] = new State(new int[]{8,773});
    states[773] = new State(new int[]{14,778,138,149,140,150,139,152,148,154,150,155,149,156,137,23,80,25,81,26,75,28,73,29,50,783},new int[]{-333,774,-329,790,-14,779,-150,146,-152,147,-151,151,-15,153,-132,780,-136,24,-137,27,-323,787,-268,772,-166,162});
    states[774] = new State(new int[]{9,775,10,776,94,788});
    states[775] = new State(-610);
    states[776] = new State(new int[]{14,778,138,149,140,150,139,152,148,154,150,155,149,156,137,23,80,25,81,26,75,28,73,29,50,783},new int[]{-329,777,-14,779,-150,146,-152,147,-151,151,-15,153,-132,780,-136,24,-137,27,-323,787,-268,772,-166,162});
    states[777] = new State(-622);
    states[778] = new State(-632);
    states[779] = new State(-633);
    states[780] = new State(new int[]{5,781,9,-635,10,-635,94,-635,7,-247,4,-247,117,-247,8,-247});
    states[781] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-260,782,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[782] = new State(-634);
    states[783] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-132,784,-136,24,-137,27});
    states[784] = new State(new int[]{5,785,9,-637,10,-637,94,-637});
    states[785] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-260,786,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[786] = new State(-636);
    states[787] = new State(-638);
    states[788] = new State(new int[]{14,778,138,149,140,150,139,152,148,154,150,155,149,156,137,23,80,25,81,26,75,28,73,29,50,783},new int[]{-329,789,-14,779,-150,146,-152,147,-151,151,-15,153,-132,780,-136,24,-137,27,-323,787,-268,772,-166,162});
    states[789] = new State(-623);
    states[790] = new State(-621);
    states[791] = new State(new int[]{36,792,5,796});
    states[792] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,793,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[793] = new State(new int[]{5,794,13,128});
    states[794] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476,29,-476,86,-476},new int[]{-244,795,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[795] = new State(-523);
    states[796] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476,29,-476,86,-476},new int[]{-244,797,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[797] = new State(-524);
    states[798] = new State(new int[]{94,799,36,-611,5,-611});
    states[799] = new State(new int[]{138,149,140,150,139,152,148,154,150,155,149,156,8,803},new int[]{-334,800,-14,801,-150,146,-152,147,-151,151,-15,153,-335,802});
    states[800] = new State(-613);
    states[801] = new State(-614);
    states[802] = new State(-615);
    states[803] = new State(new int[]{14,812,138,149,140,150,139,152,148,154,150,155,149,156},new int[]{-336,804,-14,813,-150,146,-152,147,-151,151,-15,153});
    states[804] = new State(new int[]{94,805});
    states[805] = new State(new int[]{14,812,138,149,140,150,139,152,148,154,150,155,149,156},new int[]{-330,806,-336,814,-14,813,-150,146,-152,147,-151,151,-15,153});
    states[806] = new State(new int[]{94,810,5,408,10,-896,9,-896},new int[]{-307,807});
    states[807] = new State(new int[]{10,400,9,-884},new int[]{-314,808});
    states[808] = new State(new int[]{9,809});
    states[809] = new State(-616);
    states[810] = new State(new int[]{14,812,138,149,140,150,139,152,148,154,150,155,149,156},new int[]{-336,811,-14,813,-150,146,-152,147,-151,151,-15,153});
    states[811] = new State(-620);
    states[812] = new State(-617);
    states[813] = new State(-618);
    states[814] = new State(-619);
    states[815] = new State(-612);
    states[816] = new State(-519);
    states[817] = new State(-907);
    states[818] = new State(-908);
    states[819] = new State(-909);
    states[820] = new State(-910);
    states[821] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,749,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[822] = new State(-911);
    states[823] = new State(new int[]{9,835,137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,840,129,428,110,335,109,336,60,158},new int[]{-83,824,-62,825,-227,829,-87,831,-229,833,-75,191,-12,215,-10,225,-13,201,-132,839,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435,-61,237,-79,843,-78,240,-53,844,-228,845,-230,854,-121,848});
    states[824] = new State(new int[]{9,427,13,187,94,-180});
    states[825] = new State(new int[]{9,826});
    states[826] = new State(new int[]{121,827,86,-183,10,-183,92,-183,95,-183,30,-183,98,-183,29,-183,94,-183,12,-183,9,-183,93,-183,81,-183,80,-183,2,-183,79,-183,78,-183,77,-183,76,-183});
    states[827] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-311,828,-91,355,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-313,629,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[828] = new State(-403);
    states[829] = new State(new int[]{9,830,94,-182});
    states[830] = new State(-184);
    states[831] = new State(new int[]{9,832,94,-181});
    states[832] = new State(-185);
    states[833] = new State(new int[]{9,834});
    states[834] = new State(-190);
    states[835] = new State(new int[]{5,408,121,-896},new int[]{-307,836});
    states[836] = new State(new int[]{121,837});
    states[837] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-311,838,-91,355,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-313,629,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[838] = new State(-402);
    states[839] = new State(new int[]{4,-156,11,-156,7,-156,136,-156,8,-156,113,-156,130,-156,132,-156,112,-156,111,-156,125,-156,126,-156,127,-156,128,-156,124,-156,110,-156,109,-156,122,-156,123,-156,114,-156,119,-156,117,-156,115,-156,118,-156,116,-156,131,-156,9,-156,13,-156,94,-156,5,-196});
    states[840] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,840,129,428,110,335,109,336,60,158,9,-186},new int[]{-83,824,-62,841,-227,829,-87,831,-229,833,-75,191,-12,215,-10,225,-13,201,-132,839,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435,-61,237,-79,843,-78,240,-53,844,-228,845,-230,854,-121,848});
    states[841] = new State(new int[]{9,842});
    states[842] = new State(-183);
    states[843] = new State(-188);
    states[844] = new State(-405);
    states[845] = new State(new int[]{10,846,9,-191});
    states[846] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,9,-192},new int[]{-230,847,-121,848,-132,853,-136,24,-137,27});
    states[847] = new State(-194);
    states[848] = new State(new int[]{5,849});
    states[849] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,840,129,428,110,335,109,336},new int[]{-78,850,-83,241,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435,-87,851,-227,852});
    states[850] = new State(-195);
    states[851] = new State(-181);
    states[852] = new State(-182);
    states[853] = new State(-196);
    states[854] = new State(-193);
    states[855] = new State(-400);
    states[856] = new State(new int[]{121,857,8,863});
    states[857] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,29,42,359,39,389,8,860,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-312,858,-197,859,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-4,861,-313,862,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[858] = new State(-890);
    states[859] = new State(-914);
    states[860] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,392,-91,394,-99,489,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[861] = new State(-915);
    states[862] = new State(-916);
    states[863] = new State(new int[]{9,864,137,23,80,25,81,26,75,28,73,29},new int[]{-309,867,-310,871,-143,404,-132,720,-136,24,-137,27});
    states[864] = new State(new int[]{121,865});
    states[865] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,29,42,359,39,389,8,860,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-312,866,-197,859,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-4,861,-313,862,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[866] = new State(-891);
    states[867] = new State(new int[]{9,868,10,402});
    states[868] = new State(new int[]{121,869});
    states[869] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,29,42,359,39,389,8,860,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-312,870,-197,859,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-4,861,-313,862,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[870] = new State(-892);
    states[871] = new State(-893);
    states[872] = new State(-394);
    states[873] = new State(-395);
    states[874] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,875,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[875] = new State(-397);
    states[876] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-260,877,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[877] = new State(-544);
    states[878] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-132,879,-136,24,-137,27});
    states[879] = new State(new int[]{5,880,131,886});
    states[880] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-260,881,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[881] = new State(new int[]{131,882});
    states[882] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,883,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[883] = new State(new int[]{93,884,13,128});
    states[884] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,29,-476,94,-476,12,-476,9,-476,93,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-244,885,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[885] = new State(-542);
    states[886] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,887,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[887] = new State(new int[]{93,888,13,128});
    states[888] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,29,-476,94,-476,12,-476,9,-476,93,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-244,889,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[889] = new State(-543);
    states[890] = new State(new int[]{5,891});
    states[891] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,664,150,155,149,665,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476},new int[]{-245,892,-244,121,-4,122,-100,123,-117,338,-99,345,-132,663,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754,-128,890});
    states[892] = new State(-475);
    states[893] = new State(new int[]{74,901,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,664,150,155,149,665,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476,86,-476},new int[]{-56,894,-59,896,-58,913,-236,914,-245,662,-244,121,-4,122,-100,123,-117,338,-99,345,-132,663,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754,-128,890});
    states[894] = new State(new int[]{86,895});
    states[895] = new State(-557);
    states[896] = new State(new int[]{10,898,29,911,86,-563},new int[]{-238,897});
    states[897] = new State(-558);
    states[898] = new State(new int[]{74,901,29,911,86,-563},new int[]{-58,899,-238,900});
    states[899] = new State(-562);
    states[900] = new State(-559);
    states[901] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-60,902,-165,905,-166,906,-132,907,-136,24,-137,27,-125,908});
    states[902] = new State(new int[]{93,903});
    states[903] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476,29,-476,86,-476},new int[]{-244,904,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[904] = new State(-565);
    states[905] = new State(-566);
    states[906] = new State(new int[]{7,163,93,-568});
    states[907] = new State(new int[]{7,-247,93,-247,5,-569});
    states[908] = new State(new int[]{5,909});
    states[909] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-165,910,-166,906,-132,196,-136,24,-137,27});
    states[910] = new State(-567);
    states[911] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,664,150,155,149,665,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476,86,-476},new int[]{-236,912,-245,662,-244,121,-4,122,-100,123,-117,338,-99,345,-132,663,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754,-128,890});
    states[912] = new State(new int[]{10,119,86,-564});
    states[913] = new State(-561);
    states[914] = new State(new int[]{10,119,86,-560});
    states[915] = new State(-538);
    states[916] = new State(-551);
    states[917] = new State(-552);
    states[918] = new State(-549);
    states[919] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-166,920,-132,196,-136,24,-137,27});
    states[920] = new State(new int[]{104,921,7,163});
    states[921] = new State(-550);
    states[922] = new State(-547);
    states[923] = new State(new int[]{5,924,94,926});
    states[924] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476,29,-476,86,-476},new int[]{-244,925,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[925] = new State(-530);
    states[926] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-98,927,-86,928,-83,186,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[927] = new State(-532);
    states[928] = new State(-533);
    states[929] = new State(-531);
    states[930] = new State(new int[]{86,931});
    states[931] = new State(-527);
    states[932] = new State(-528);
    states[933] = new State(new int[]{9,934,137,23,80,25,81,26,75,28,73,29},new int[]{-309,938,-310,871,-143,404,-132,720,-136,24,-137,27});
    states[934] = new State(new int[]{5,408,121,-896},new int[]{-307,935});
    states[935] = new State(new int[]{121,936});
    states[936] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-311,937,-91,355,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-313,629,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[937] = new State(-888);
    states[938] = new State(new int[]{9,939,10,402});
    states[939] = new State(new int[]{5,408,121,-896},new int[]{-307,940});
    states[940] = new State(new int[]{121,941});
    states[941] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-311,942,-91,355,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-313,629,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[942] = new State(-889);
    states[943] = new State(new int[]{5,944,7,-247,8,-247,117,-247,12,-247,94,-247});
    states[944] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-9,945,-166,617,-132,196,-136,24,-137,27,-285,946});
    states[945] = new State(-205);
    states[946] = new State(new int[]{8,620,12,-604,94,-604},new int[]{-65,947});
    states[947] = new State(-707);
    states[948] = new State(-202);
    states[949] = new State(-198);
    states[950] = new State(-455);
    states[951] = new State(-631);
    states[952] = new State(new int[]{8,953});
    states[953] = new State(new int[]{14,532,138,149,140,150,139,152,148,154,150,155,149,156,50,534,137,23,80,25,81,26,75,28,73,29},new int[]{-332,954,-328,958,-14,533,-150,146,-152,147,-151,151,-15,153,-322,951,-268,952,-166,162,-132,196,-136,24,-137,27});
    states[954] = new State(new int[]{9,955,10,530,94,956});
    states[955] = new State(-609);
    states[956] = new State(new int[]{14,532,138,149,140,150,139,152,148,154,150,155,149,156,50,534,137,23,80,25,81,26,75,28,73,29},new int[]{-328,957,-14,533,-150,146,-152,147,-151,151,-15,153,-322,951,-268,952,-166,162,-132,196,-136,24,-137,27});
    states[957] = new State(-626);
    states[958] = new State(-624);
    states[959] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,963,136,446,21,451,45,459,46,539,31,543,71,547,62,550},new int[]{-261,960,-256,961,-85,175,-94,267,-95,268,-166,962,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-240,968,-233,969,-265,970,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-285,971});
    states[960] = new State(-899);
    states[961] = new State(-469);
    states[962] = new State(new int[]{7,163,117,168,8,-242,112,-242,111,-242,125,-242,126,-242,127,-242,128,-242,124,-242,6,-242,110,-242,109,-242,122,-242,123,-242,121,-242},new int[]{-283,619});
    states[963] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-74,964,-72,284,-260,287,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[964] = new State(new int[]{9,965,94,966});
    states[965] = new State(-237);
    states[966] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-72,967,-260,287,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[967] = new State(-250);
    states[968] = new State(-470);
    states[969] = new State(-471);
    states[970] = new State(-472);
    states[971] = new State(-473);
    states[972] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,973,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[973] = new State(new int[]{13,128,94,-113,5,-113,10,-113,9,-113});
    states[974] = new State(new int[]{13,128,94,-112,5,-112,10,-112,9,-112});
    states[975] = new State(new int[]{5,959,121,-898},new int[]{-308,976});
    states[976] = new State(new int[]{121,977});
    states[977] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-311,978,-91,355,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-313,629,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[978] = new State(-878);
    states[979] = new State(new int[]{5,980,10,992,17,-714,8,-714,7,-714,136,-714,4,-714,15,-714,132,-714,130,-714,112,-714,111,-714,125,-714,126,-714,127,-714,128,-714,124,-714,110,-714,109,-714,122,-714,123,-714,120,-714,114,-714,119,-714,117,-714,115,-714,118,-714,116,-714,131,-714,16,-714,94,-714,13,-714,9,-714,113,-714,11,-714});
    states[980] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-259,981,-260,410,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[981] = new State(new int[]{9,982,10,986});
    states[982] = new State(new int[]{5,959,121,-898},new int[]{-308,983});
    states[983] = new State(new int[]{121,984});
    states[984] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-311,985,-91,355,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-313,629,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[985] = new State(-879);
    states[986] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-309,987,-310,871,-143,404,-132,720,-136,24,-137,27});
    states[987] = new State(new int[]{9,988,10,402});
    states[988] = new State(new int[]{5,959,121,-898},new int[]{-308,989});
    states[989] = new State(new int[]{121,990});
    states[990] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-311,991,-91,355,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-313,629,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[991] = new State(-881);
    states[992] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-309,993,-310,871,-143,404,-132,720,-136,24,-137,27});
    states[993] = new State(new int[]{9,994,10,402});
    states[994] = new State(new int[]{5,959,121,-898},new int[]{-308,995});
    states[995] = new State(new int[]{121,996});
    states[996] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,85,116,37,632,51,672,91,667,32,677,33,703,70,735,22,650,96,693,57,743,72,821,44,700},new int[]{-311,997,-91,355,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-313,629,-239,630,-138,631,-301,730,-231,731,-109,732,-108,733,-110,734,-32,817,-286,818,-154,819,-111,820,-232,822});
    states[997] = new State(-880);
    states[998] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359,39,389,8,391,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156},new int[]{-99,999,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639});
    states[999] = new State(new int[]{17,340,8,346,7,1000,136,1002,4,1003,9,-507,94,-507,11,-712});
    states[1000] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,30,64,31,65,24,66,98,67,95,68,32,69,33,70,34,71,37,72,38,73,39,74,97,75,40,76,41,77,43,78,44,79,45,80,91,81,46,82,96,83,47,84,25,85,48,86,68,87,92,88,49,89,50,90,51,91,52,92,53,93,54,94,55,95,56,96,58,97,99,98,100,99,103,100,101,101,102,102,59,103,72,104,35,105,36,106,42,359},new int[]{-133,1001,-132,516,-136,24,-137,27,-277,517,-135,31,-177,518});
    states[1001] = new State(-724);
    states[1002] = new State(-726);
    states[1003] = new State(new int[]{117,168},new int[]{-283,1004});
    states[1004] = new State(-727);
    states[1005] = new State(new int[]{17,340,8,346,7,1000,136,1002,4,1003,9,-506,94,-506,11,-712});
    states[1006] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359,39,389,8,391,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156},new int[]{-99,1007,-103,1008,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639});
    states[1007] = new State(new int[]{17,340,8,346,7,1000,136,1002,4,1003,15,1006,104,-699,105,-699,106,-699,107,-699,108,-699,86,-699,10,-699,92,-699,95,-699,30,-699,98,-699,132,-699,130,-699,112,-699,111,-699,125,-699,126,-699,127,-699,128,-699,124,-699,5,-699,110,-699,109,-699,122,-699,123,-699,120,-699,114,-699,119,-699,117,-699,115,-699,118,-699,116,-699,131,-699,16,-699,13,-699,29,-699,94,-699,12,-699,9,-699,93,-699,81,-699,80,-699,2,-699,79,-699,78,-699,77,-699,76,-699,113,-699,6,-699,48,-699,55,-699,135,-699,137,-699,75,-699,73,-699,42,-699,39,-699,18,-699,19,-699,138,-699,140,-699,139,-699,148,-699,150,-699,149,-699,54,-699,85,-699,37,-699,22,-699,91,-699,51,-699,32,-699,52,-699,96,-699,44,-699,33,-699,50,-699,57,-699,72,-699,70,-699,35,-699,68,-699,69,-699,11,-712});
    states[1008] = new State(-700);
    states[1009] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-132,1010,-136,24,-137,27});
    states[1010] = new State(new int[]{94,1011});
    states[1011] = new State(new int[]{50,1019},new int[]{-320,1012});
    states[1012] = new State(new int[]{9,1013,94,1016});
    states[1013] = new State(new int[]{104,1014});
    states[1014] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,1015,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[1015] = new State(-502);
    states[1016] = new State(new int[]{50,1017});
    states[1017] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-132,1018,-136,24,-137,27});
    states[1018] = new State(-509);
    states[1019] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-132,1020,-136,24,-137,27});
    states[1020] = new State(-508);
    states[1021] = new State(new int[]{141,1025,143,1026,144,1027,145,1028,147,1029,146,1030,101,-743,85,-743,56,-743,26,-743,64,-743,47,-743,50,-743,59,-743,11,-743,25,-743,23,-743,41,-743,34,-743,27,-743,28,-743,43,-743,24,-743,86,-743,79,-743,78,-743,77,-743,76,-743,20,-743,142,-743,38,-743},new int[]{-192,1022,-195,1031});
    states[1022] = new State(new int[]{10,1023});
    states[1023] = new State(new int[]{141,1025,143,1026,144,1027,145,1028,147,1029,146,1030,101,-744,85,-744,56,-744,26,-744,64,-744,47,-744,50,-744,59,-744,11,-744,25,-744,23,-744,41,-744,34,-744,27,-744,28,-744,43,-744,24,-744,86,-744,79,-744,78,-744,77,-744,76,-744,20,-744,142,-744,38,-744},new int[]{-195,1024});
    states[1024] = new State(-748);
    states[1025] = new State(-758);
    states[1026] = new State(-759);
    states[1027] = new State(-760);
    states[1028] = new State(-761);
    states[1029] = new State(-762);
    states[1030] = new State(-763);
    states[1031] = new State(-747);
    states[1032] = new State(-363);
    states[1033] = new State(-429);
    states[1034] = new State(-430);
    states[1035] = new State(new int[]{8,-435,104,-435,10,-435,5,-435,7,-432});
    states[1036] = new State(new int[]{117,1038,8,-438,104,-438,10,-438,7,-438,5,-438},new int[]{-140,1037});
    states[1037] = new State(-439);
    states[1038] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-143,1039,-132,720,-136,24,-137,27});
    states[1039] = new State(new int[]{115,1040,94,406});
    states[1040] = new State(-310);
    states[1041] = new State(-440);
    states[1042] = new State(new int[]{117,1038,8,-436,104,-436,10,-436,5,-436},new int[]{-140,1043});
    states[1043] = new State(-437);
    states[1044] = new State(new int[]{7,1045});
    states[1045] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359},new int[]{-127,1046,-134,1047,-122,1035,-119,1036,-132,1041,-136,24,-137,27,-177,1042});
    states[1046] = new State(-431);
    states[1047] = new State(-434);
    states[1048] = new State(-433);
    states[1049] = new State(-422);
    states[1050] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35},new int[]{-158,1051,-132,1090,-136,24,-137,27,-135,1091});
    states[1051] = new State(new int[]{7,1075,11,1081,5,-379},new int[]{-218,1052,-223,1078});
    states[1052] = new State(new int[]{80,1064,81,1070,10,-386},new int[]{-188,1053});
    states[1053] = new State(new int[]{10,1054});
    states[1054] = new State(new int[]{60,1059,146,1061,145,1062,141,1063,11,-376,25,-376,23,-376,41,-376,34,-376,27,-376,28,-376,43,-376,24,-376,86,-376,79,-376,78,-376,77,-376,76,-376},new int[]{-191,1055,-196,1056});
    states[1055] = new State(-372);
    states[1056] = new State(new int[]{10,1057});
    states[1057] = new State(new int[]{60,1059,11,-376,25,-376,23,-376,41,-376,34,-376,27,-376,28,-376,43,-376,24,-376,86,-376,79,-376,78,-376,77,-376,76,-376},new int[]{-191,1058});
    states[1058] = new State(-373);
    states[1059] = new State(new int[]{10,1060});
    states[1060] = new State(-377);
    states[1061] = new State(-764);
    states[1062] = new State(-765);
    states[1063] = new State(-766);
    states[1064] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,502,18,247,19,252,5,573,34,625,41,856,10,-385},new int[]{-101,1065,-82,1069,-81,126,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,352,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572,-305,623,-306,624});
    states[1065] = new State(new int[]{81,1067,10,-389},new int[]{-189,1066});
    states[1066] = new State(-387);
    states[1067] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476},new int[]{-244,1068,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[1068] = new State(-390);
    states[1069] = new State(-384);
    states[1070] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476},new int[]{-244,1071,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[1071] = new State(new int[]{80,1073,10,-391},new int[]{-190,1072});
    states[1072] = new State(-388);
    states[1073] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,502,18,247,19,252,5,573,34,625,41,856,10,-385},new int[]{-101,1074,-82,1069,-81,126,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,352,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572,-305,623,-306,624});
    states[1074] = new State(-392);
    states[1075] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35},new int[]{-132,1076,-135,1077,-136,24,-137,27});
    states[1076] = new State(-367);
    states[1077] = new State(-368);
    states[1078] = new State(new int[]{5,1079});
    states[1079] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-259,1080,-260,410,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[1080] = new State(-378);
    states[1081] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-222,1082,-221,1089,-143,1086,-132,720,-136,24,-137,27});
    states[1082] = new State(new int[]{12,1083,10,1084});
    states[1083] = new State(-380);
    states[1084] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-221,1085,-143,1086,-132,720,-136,24,-137,27});
    states[1085] = new State(-382);
    states[1086] = new State(new int[]{5,1087,94,406});
    states[1087] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-259,1088,-260,410,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[1088] = new State(-383);
    states[1089] = new State(-381);
    states[1090] = new State(-365);
    states[1091] = new State(-366);
    states[1092] = new State(new int[]{43,1093});
    states[1093] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35},new int[]{-158,1094,-132,1090,-136,24,-137,27,-135,1091});
    states[1094] = new State(new int[]{7,1075,11,1081,5,-379},new int[]{-218,1095,-223,1078});
    states[1095] = new State(new int[]{10,1096});
    states[1096] = new State(-375);
    states[1097] = new State(new int[]{101,1228,11,-357,25,-357,23,-357,41,-357,34,-357,27,-357,28,-357,43,-357,24,-357,86,-357,79,-357,78,-357,77,-357,76,-357,56,-63,26,-63,64,-63,47,-63,50,-63,59,-63,85,-63},new int[]{-162,1098,-40,1099,-36,1102,-57,1227});
    states[1098] = new State(-423);
    states[1099] = new State(new int[]{85,116},new int[]{-239,1100});
    states[1100] = new State(new int[]{10,1101});
    states[1101] = new State(-450);
    states[1102] = new State(new int[]{56,1105,26,1126,64,1130,47,1290,50,1305,59,1307,85,-62},new int[]{-42,1103,-153,1104,-26,1111,-48,1128,-273,1132,-292,1292});
    states[1103] = new State(-64);
    states[1104] = new State(-80);
    states[1105] = new State(new int[]{148,644,149,645,137,23,80,25,81,26,75,28,73,29},new int[]{-141,1106,-128,1110,-132,646,-136,24,-137,27});
    states[1106] = new State(new int[]{10,1107,94,1108});
    states[1107] = new State(-89);
    states[1108] = new State(new int[]{148,644,149,645,137,23,80,25,81,26,75,28,73,29},new int[]{-128,1109,-132,646,-136,24,-137,27});
    states[1109] = new State(-91);
    states[1110] = new State(-90);
    states[1111] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,56,-81,26,-81,64,-81,47,-81,50,-81,59,-81,85,-81},new int[]{-24,1112,-25,1113,-126,1115,-132,1125,-136,24,-137,27});
    states[1112] = new State(-96);
    states[1113] = new State(new int[]{10,1114});
    states[1114] = new State(-106);
    states[1115] = new State(new int[]{114,1116,5,1121});
    states[1116] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,1119,129,428,110,335,109,336},new int[]{-97,1117,-83,1118,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435,-87,1120});
    states[1117] = new State(-107);
    states[1118] = new State(new int[]{13,187,10,-109,86,-109,79,-109,78,-109,77,-109,76,-109});
    states[1119] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,840,129,428,110,335,109,336,60,158,9,-186},new int[]{-83,824,-62,841,-227,829,-87,831,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435,-61,237,-79,843,-78,240,-53,844});
    states[1120] = new State(-110);
    states[1121] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-260,1122,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[1122] = new State(new int[]{114,1123});
    states[1123] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,840,129,428,110,335,109,336},new int[]{-78,1124,-83,241,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435,-87,851,-227,852});
    states[1124] = new State(-108);
    states[1125] = new State(-111);
    states[1126] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-24,1127,-25,1113,-126,1115,-132,1125,-136,24,-137,27});
    states[1127] = new State(-95);
    states[1128] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,56,-82,26,-82,64,-82,47,-82,50,-82,59,-82,85,-82},new int[]{-24,1129,-25,1113,-126,1115,-132,1125,-136,24,-137,27});
    states[1129] = new State(-98);
    states[1130] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-24,1131,-25,1113,-126,1115,-132,1125,-136,24,-137,27});
    states[1131] = new State(-97);
    states[1132] = new State(new int[]{11,611,56,-83,26,-83,64,-83,47,-83,50,-83,59,-83,85,-83,137,-200,80,-200,81,-200,75,-200,73,-200},new int[]{-45,1133,-6,1134,-234,949});
    states[1133] = new State(-100);
    states[1134] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,11,611},new int[]{-46,1135,-234,475,-129,1136,-132,1282,-136,24,-137,27,-130,1287});
    states[1135] = new State(-197);
    states[1136] = new State(new int[]{114,1137});
    states[1137] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579,66,1276,67,1277,141,1278,24,1279,25,1280,23,-292,40,-292,61,-292},new int[]{-271,1138,-260,1140,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583,-27,1141,-20,1142,-21,1274,-19,1281});
    states[1138] = new State(new int[]{10,1139});
    states[1139] = new State(-206);
    states[1140] = new State(-211);
    states[1141] = new State(-212);
    states[1142] = new State(new int[]{23,1268,40,1269,61,1270},new int[]{-275,1143});
    states[1143] = new State(new int[]{8,1184,20,-304,11,-304,86,-304,79,-304,78,-304,77,-304,76,-304,26,-304,137,-304,80,-304,81,-304,75,-304,73,-304,59,-304,25,-304,23,-304,41,-304,34,-304,27,-304,28,-304,43,-304,24,-304,10,-304},new int[]{-169,1144});
    states[1144] = new State(new int[]{20,1175,11,-311,86,-311,79,-311,78,-311,77,-311,76,-311,26,-311,137,-311,80,-311,81,-311,75,-311,73,-311,59,-311,25,-311,23,-311,41,-311,34,-311,27,-311,28,-311,43,-311,24,-311,10,-311},new int[]{-300,1145,-299,1173,-298,1195});
    states[1145] = new State(new int[]{11,611,10,-302,86,-328,79,-328,78,-328,77,-328,76,-328,26,-200,137,-200,80,-200,81,-200,75,-200,73,-200,59,-200,25,-200,23,-200,41,-200,34,-200,27,-200,28,-200,43,-200,24,-200},new int[]{-23,1146,-22,1147,-29,1153,-31,466,-41,1154,-6,1155,-234,949,-30,1265,-50,1267,-49,472,-51,1266});
    states[1146] = new State(-285);
    states[1147] = new State(new int[]{86,1148,79,1149,78,1150,77,1151,76,1152},new int[]{-7,464});
    states[1148] = new State(-303);
    states[1149] = new State(-324);
    states[1150] = new State(-325);
    states[1151] = new State(-326);
    states[1152] = new State(-327);
    states[1153] = new State(-322);
    states[1154] = new State(-336);
    states[1155] = new State(new int[]{26,1157,137,23,80,25,81,26,75,28,73,29,59,1161,25,1222,23,1223,11,611,41,1168,34,1203,27,1237,28,1244,43,1251,24,1260},new int[]{-47,1156,-234,475,-207,474,-204,476,-242,477,-295,1159,-294,1160,-143,721,-132,720,-136,24,-137,27,-3,1165,-215,1224,-213,1097,-210,1167,-214,1202,-212,1225,-200,1248,-201,1249,-203,1250});
    states[1156] = new State(-338);
    states[1157] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-25,1158,-126,1115,-132,1125,-136,24,-137,27});
    states[1158] = new State(-343);
    states[1159] = new State(-344);
    states[1160] = new State(-348);
    states[1161] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-143,1162,-132,720,-136,24,-137,27});
    states[1162] = new State(new int[]{5,1163,94,406});
    states[1163] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-260,1164,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[1164] = new State(-349);
    states[1165] = new State(new int[]{27,480,43,1050,24,1092,137,23,80,25,81,26,75,28,73,29,59,1161,41,1168,34,1203},new int[]{-295,1166,-215,479,-201,1049,-294,1160,-143,721,-132,720,-136,24,-137,27,-213,1097,-210,1167,-214,1202});
    states[1166] = new State(-345);
    states[1167] = new State(-358);
    states[1168] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359},new int[]{-156,1169,-155,1033,-127,1034,-122,1035,-119,1036,-132,1041,-136,24,-137,27,-177,1042,-317,1044,-134,1048});
    states[1169] = new State(new int[]{8,557,10,-452,104,-452},new int[]{-113,1170});
    states[1170] = new State(new int[]{10,1200,104,-745},new int[]{-193,1171,-194,1196});
    states[1171] = new State(new int[]{20,1175,101,-311,85,-311,56,-311,26,-311,64,-311,47,-311,50,-311,59,-311,11,-311,25,-311,23,-311,41,-311,34,-311,27,-311,28,-311,43,-311,24,-311,86,-311,79,-311,78,-311,77,-311,76,-311,142,-311,38,-311},new int[]{-300,1172,-299,1173,-298,1195});
    states[1172] = new State(-441);
    states[1173] = new State(new int[]{20,1175,11,-312,86,-312,79,-312,78,-312,77,-312,76,-312,26,-312,137,-312,80,-312,81,-312,75,-312,73,-312,59,-312,25,-312,23,-312,41,-312,34,-312,27,-312,28,-312,43,-312,24,-312,10,-312,101,-312,85,-312,56,-312,64,-312,47,-312,50,-312,142,-312,38,-312},new int[]{-298,1174});
    states[1174] = new State(-314);
    states[1175] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-143,1176,-132,720,-136,24,-137,27});
    states[1176] = new State(new int[]{5,1177,94,406});
    states[1177] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,1183,46,539,31,543,71,547,62,550,41,555,34,579,23,1192,27,1193},new int[]{-272,1178,-269,1194,-260,1182,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[1178] = new State(new int[]{10,1179,94,1180});
    states[1179] = new State(-315);
    states[1180] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,1183,46,539,31,543,71,547,62,550,41,555,34,579,23,1192,27,1193},new int[]{-269,1181,-260,1182,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[1181] = new State(-317);
    states[1182] = new State(-318);
    states[1183] = new State(new int[]{8,1184,10,-320,94,-320,20,-304,11,-304,86,-304,79,-304,78,-304,77,-304,76,-304,26,-304,137,-304,80,-304,81,-304,75,-304,73,-304,59,-304,25,-304,23,-304,41,-304,34,-304,27,-304,28,-304,43,-304,24,-304},new int[]{-169,460});
    states[1184] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-168,1185,-167,1191,-166,1189,-132,196,-136,24,-137,27,-285,1190});
    states[1185] = new State(new int[]{9,1186,94,1187});
    states[1186] = new State(-305);
    states[1187] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-167,1188,-166,1189,-132,196,-136,24,-137,27,-285,1190});
    states[1188] = new State(-307);
    states[1189] = new State(new int[]{7,163,117,168,9,-308,94,-308},new int[]{-283,619});
    states[1190] = new State(-309);
    states[1191] = new State(-306);
    states[1192] = new State(-319);
    states[1193] = new State(-321);
    states[1194] = new State(-316);
    states[1195] = new State(-313);
    states[1196] = new State(new int[]{104,1197});
    states[1197] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476},new int[]{-244,1198,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[1198] = new State(new int[]{10,1199});
    states[1199] = new State(-426);
    states[1200] = new State(new int[]{141,1025,143,1026,144,1027,145,1028,147,1029,146,1030,20,-743,101,-743,85,-743,56,-743,26,-743,64,-743,47,-743,50,-743,59,-743,11,-743,25,-743,23,-743,41,-743,34,-743,27,-743,28,-743,43,-743,24,-743,86,-743,79,-743,78,-743,77,-743,76,-743,142,-743},new int[]{-192,1201,-195,1031});
    states[1201] = new State(new int[]{10,1023,104,-746});
    states[1202] = new State(-359);
    states[1203] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359},new int[]{-155,1204,-127,1034,-122,1035,-119,1036,-132,1041,-136,24,-137,27,-177,1042,-317,1044,-134,1048});
    states[1204] = new State(new int[]{8,557,5,-452,10,-452,104,-452},new int[]{-113,1205});
    states[1205] = new State(new int[]{5,1208,10,1200,104,-745},new int[]{-193,1206,-194,1218});
    states[1206] = new State(new int[]{20,1175,101,-311,85,-311,56,-311,26,-311,64,-311,47,-311,50,-311,59,-311,11,-311,25,-311,23,-311,41,-311,34,-311,27,-311,28,-311,43,-311,24,-311,86,-311,79,-311,78,-311,77,-311,76,-311,142,-311,38,-311},new int[]{-300,1207,-299,1173,-298,1195});
    states[1207] = new State(-442);
    states[1208] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-259,1209,-260,410,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[1209] = new State(new int[]{10,1200,104,-745},new int[]{-193,1210,-194,1212});
    states[1210] = new State(new int[]{20,1175,101,-311,85,-311,56,-311,26,-311,64,-311,47,-311,50,-311,59,-311,11,-311,25,-311,23,-311,41,-311,34,-311,27,-311,28,-311,43,-311,24,-311,86,-311,79,-311,78,-311,77,-311,76,-311,142,-311,38,-311},new int[]{-300,1211,-299,1173,-298,1195});
    states[1211] = new State(-443);
    states[1212] = new State(new int[]{104,1213});
    states[1213] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,502,18,247,19,252,34,625,41,856},new int[]{-92,1214,-91,1216,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,352,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-305,1217,-306,624});
    states[1214] = new State(new int[]{10,1215});
    states[1215] = new State(-424);
    states[1216] = new State(new int[]{13,128,10,-582});
    states[1217] = new State(-583);
    states[1218] = new State(new int[]{104,1219});
    states[1219] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,502,18,247,19,252,34,625,41,856},new int[]{-92,1220,-91,1216,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,352,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-305,1217,-306,624});
    states[1220] = new State(new int[]{10,1221});
    states[1221] = new State(-425);
    states[1222] = new State(-346);
    states[1223] = new State(-347);
    states[1224] = new State(-355);
    states[1225] = new State(new int[]{101,1228,11,-356,25,-356,23,-356,41,-356,34,-356,27,-356,28,-356,43,-356,24,-356,86,-356,79,-356,78,-356,77,-356,76,-356,56,-63,26,-63,64,-63,47,-63,50,-63,59,-63,85,-63},new int[]{-162,1226,-40,1099,-36,1102,-57,1227});
    states[1226] = new State(-409);
    states[1227] = new State(-451);
    states[1228] = new State(new int[]{10,1236,137,23,80,25,81,26,75,28,73,29,138,149,140,150,139,152},new int[]{-96,1229,-132,1233,-136,24,-137,27,-150,1234,-152,147,-151,151});
    states[1229] = new State(new int[]{75,1230,10,1235});
    states[1230] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,138,149,140,150,139,152},new int[]{-96,1231,-132,1233,-136,24,-137,27,-150,1234,-152,147,-151,151});
    states[1231] = new State(new int[]{10,1232});
    states[1232] = new State(-444);
    states[1233] = new State(-447);
    states[1234] = new State(-448);
    states[1235] = new State(-445);
    states[1236] = new State(-446);
    states[1237] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359,8,-364,104,-364,10,-364},new int[]{-157,1238,-156,1032,-155,1033,-127,1034,-122,1035,-119,1036,-132,1041,-136,24,-137,27,-177,1042,-317,1044,-134,1048});
    states[1238] = new State(new int[]{8,557,104,-452,10,-452},new int[]{-113,1239});
    states[1239] = new State(new int[]{104,1241,10,1021},new int[]{-193,1240});
    states[1240] = new State(-360);
    states[1241] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476},new int[]{-244,1242,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[1242] = new State(new int[]{10,1243});
    states[1243] = new State(-410);
    states[1244] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359,8,-364,10,-364},new int[]{-157,1245,-156,1032,-155,1033,-127,1034,-122,1035,-119,1036,-132,1041,-136,24,-137,27,-177,1042,-317,1044,-134,1048});
    states[1245] = new State(new int[]{8,557,10,-452},new int[]{-113,1246});
    states[1246] = new State(new int[]{10,1021},new int[]{-193,1247});
    states[1247] = new State(-362);
    states[1248] = new State(-352);
    states[1249] = new State(-421);
    states[1250] = new State(-353);
    states[1251] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35},new int[]{-158,1252,-132,1090,-136,24,-137,27,-135,1091});
    states[1252] = new State(new int[]{7,1075,11,1081,5,-379},new int[]{-218,1253,-223,1078});
    states[1253] = new State(new int[]{80,1064,81,1070,10,-386},new int[]{-188,1254});
    states[1254] = new State(new int[]{10,1255});
    states[1255] = new State(new int[]{60,1059,146,1061,145,1062,141,1063,11,-376,25,-376,23,-376,41,-376,34,-376,27,-376,28,-376,43,-376,24,-376,86,-376,79,-376,78,-376,77,-376,76,-376},new int[]{-191,1256,-196,1257});
    states[1256] = new State(-370);
    states[1257] = new State(new int[]{10,1258});
    states[1258] = new State(new int[]{60,1059,11,-376,25,-376,23,-376,41,-376,34,-376,27,-376,28,-376,43,-376,24,-376,86,-376,79,-376,78,-376,77,-376,76,-376},new int[]{-191,1259});
    states[1259] = new State(-371);
    states[1260] = new State(new int[]{43,1261});
    states[1261] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35},new int[]{-158,1262,-132,1090,-136,24,-137,27,-135,1091});
    states[1262] = new State(new int[]{7,1075,11,1081,5,-379},new int[]{-218,1263,-223,1078});
    states[1263] = new State(new int[]{10,1264});
    states[1264] = new State(-374);
    states[1265] = new State(new int[]{11,611,86,-330,79,-330,78,-330,77,-330,76,-330,25,-200,23,-200,41,-200,34,-200,27,-200,28,-200,43,-200,24,-200},new int[]{-50,471,-49,472,-6,473,-234,949,-51,1266});
    states[1266] = new State(-342);
    states[1267] = new State(-339);
    states[1268] = new State(-296);
    states[1269] = new State(-297);
    states[1270] = new State(new int[]{23,1271,45,1272,40,1273,8,-298,20,-298,11,-298,86,-298,79,-298,78,-298,77,-298,76,-298,26,-298,137,-298,80,-298,81,-298,75,-298,73,-298,59,-298,25,-298,41,-298,34,-298,27,-298,28,-298,43,-298,24,-298,10,-298});
    states[1271] = new State(-299);
    states[1272] = new State(-300);
    states[1273] = new State(-301);
    states[1274] = new State(new int[]{66,1276,67,1277,141,1278,24,1279,25,1280,23,-293,40,-293,61,-293},new int[]{-19,1275});
    states[1275] = new State(-295);
    states[1276] = new State(-287);
    states[1277] = new State(-288);
    states[1278] = new State(-289);
    states[1279] = new State(-290);
    states[1280] = new State(-291);
    states[1281] = new State(-294);
    states[1282] = new State(new int[]{117,1284,114,-208},new int[]{-140,1283});
    states[1283] = new State(-209);
    states[1284] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-143,1285,-132,720,-136,24,-137,27});
    states[1285] = new State(new int[]{116,1286,115,1040,94,406});
    states[1286] = new State(-210);
    states[1287] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579,66,1276,67,1277,141,1278,24,1279,25,1280,23,-292,40,-292,61,-292},new int[]{-271,1288,-260,1140,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583,-27,1141,-20,1142,-21,1274,-19,1281});
    states[1288] = new State(new int[]{10,1289});
    states[1289] = new State(-207);
    states[1290] = new State(new int[]{11,611,137,-200,80,-200,81,-200,75,-200,73,-200},new int[]{-45,1291,-6,1134,-234,949});
    states[1291] = new State(-99);
    states[1292] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,8,1297,56,-84,26,-84,64,-84,47,-84,50,-84,59,-84,85,-84},new int[]{-296,1293,-293,1294,-294,1295,-143,721,-132,720,-136,24,-137,27});
    states[1293] = new State(-105);
    states[1294] = new State(-101);
    states[1295] = new State(new int[]{10,1296});
    states[1296] = new State(-393);
    states[1297] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-132,1298,-136,24,-137,27});
    states[1298] = new State(new int[]{94,1299});
    states[1299] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-143,1300,-132,720,-136,24,-137,27});
    states[1300] = new State(new int[]{9,1301,94,406});
    states[1301] = new State(new int[]{104,1302});
    states[1302] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-91,1303,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571});
    states[1303] = new State(new int[]{10,1304,13,128});
    states[1304] = new State(-102);
    states[1305] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,8,1297},new int[]{-296,1306,-293,1294,-294,1295,-143,721,-132,720,-136,24,-137,27});
    states[1306] = new State(-103);
    states[1307] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,8,1297},new int[]{-296,1308,-293,1294,-294,1295,-143,721,-132,720,-136,24,-137,27});
    states[1308] = new State(-104);
    states[1309] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,963,12,-268,94,-268},new int[]{-255,1310,-256,1311,-85,175,-94,267,-95,268,-166,439,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151});
    states[1310] = new State(-266);
    states[1311] = new State(-267);
    states[1312] = new State(-265);
    states[1313] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-260,1314,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[1314] = new State(-264);
    states[1315] = new State(-705);
    states[1316] = new State(-732);
    states[1317] = new State(-227);
    states[1318] = new State(-223);
    states[1319] = new State(-590);
    states[1320] = new State(new int[]{8,1321});
    states[1321] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,42,359,39,389,8,391,18,247,19,252},new int[]{-316,1322,-315,1330,-132,1326,-136,24,-137,27,-89,1329,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570});
    states[1322] = new State(new int[]{9,1323,94,1324});
    states[1323] = new State(-599);
    states[1324] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,42,359,39,389,8,391,18,247,19,252},new int[]{-315,1325,-132,1326,-136,24,-137,27,-89,1329,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570});
    states[1325] = new State(-603);
    states[1326] = new State(new int[]{104,1327,17,-714,8,-714,7,-714,136,-714,4,-714,15,-714,132,-714,130,-714,112,-714,111,-714,125,-714,126,-714,127,-714,128,-714,124,-714,110,-714,109,-714,122,-714,123,-714,120,-714,114,-714,119,-714,117,-714,115,-714,118,-714,116,-714,131,-714,9,-714,94,-714,113,-714,11,-714});
    states[1327] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252},new int[]{-89,1328,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570});
    states[1328] = new State(new int[]{114,291,119,292,117,293,115,294,118,295,116,296,131,297,9,-600,94,-600},new int[]{-182,135});
    states[1329] = new State(new int[]{114,291,119,292,117,293,115,294,118,295,116,296,131,297,9,-601,94,-601},new int[]{-182,135});
    states[1330] = new State(-602);
    states[1331] = new State(new int[]{13,187,5,-641,12,-641});
    states[1332] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-83,1333,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[1333] = new State(new int[]{13,187,94,-176,9,-176,12,-176,5,-176});
    states[1334] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336,5,-642,12,-642},new int[]{-107,1335,-83,1331,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[1335] = new State(new int[]{5,1336,12,-648});
    states[1336] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-83,1337,-75,191,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434,-226,435});
    states[1337] = new State(new int[]{13,187,12,-650});
    states[1338] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,30,64,31,65,24,66,98,67,95,68,32,69,33,70,34,71,37,72,38,73,39,74,97,75,40,76,41,77,43,78,44,79,45,80,91,81,46,82,96,83,47,84,25,85,48,86,68,87,92,88,49,89,50,90,51,91,52,92,53,93,54,94,55,95,56,96,58,97,99,98,100,99,103,100,101,101,102,102,59,103,72,104,35,105,36,106,42,108,86,109},new int[]{-123,1339,-132,22,-136,24,-137,27,-277,30,-135,31,-278,107});
    states[1339] = new State(-165);
    states[1340] = new State(-166);
    states[1341] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,502,18,247,19,252,5,573,34,625,41,856,9,-170},new int[]{-70,1342,-66,1344,-82,501,-81,126,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,352,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572,-305,623,-306,624});
    states[1342] = new State(new int[]{9,1343});
    states[1343] = new State(-167);
    states[1344] = new State(new int[]{94,350,9,-169});
    states[1345] = new State(-137);
    states[1346] = new State(new int[]{137,23,80,25,81,26,75,28,73,227,138,149,140,150,139,152,148,154,150,155,149,156,39,244,18,247,19,252,11,418,53,422,135,423,8,425,129,428,110,335,109,336},new int[]{-75,1347,-12,215,-10,225,-13,201,-132,226,-136,24,-137,27,-150,242,-152,147,-151,151,-15,243,-241,246,-279,251,-224,417,-185,430,-159,432,-249,433,-253,434});
    states[1347] = new State(new int[]{110,1348,109,1349,122,1350,123,1351,13,-115,6,-115,94,-115,9,-115,12,-115,5,-115,86,-115,10,-115,92,-115,95,-115,30,-115,98,-115,29,-115,93,-115,81,-115,80,-115,2,-115,79,-115,78,-115,77,-115,76,-115},new int[]{-179,192});
    states[1348] = new State(-127);
    states[1349] = new State(-128);
    states[1350] = new State(-129);
    states[1351] = new State(-130);
    states[1352] = new State(-118);
    states[1353] = new State(-119);
    states[1354] = new State(-120);
    states[1355] = new State(-121);
    states[1356] = new State(-122);
    states[1357] = new State(-123);
    states[1358] = new State(-124);
    states[1359] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152},new int[]{-85,1360,-94,267,-95,268,-166,439,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151});
    states[1360] = new State(new int[]{110,1348,109,1349,122,1350,123,1351,13,-236,115,-236,94,-236,114,-236,9,-236,10,-236,121,-236,104,-236,86,-236,92,-236,95,-236,30,-236,98,-236,29,-236,12,-236,93,-236,81,-236,80,-236,2,-236,79,-236,78,-236,77,-236,76,-236,131,-236},new int[]{-179,176});
    states[1361] = new State(-33);
    states[1362] = new State(new int[]{56,1105,26,1126,64,1130,47,1290,50,1305,59,1307,11,611,85,-59,86,-59,97,-59,41,-200,34,-200,25,-200,23,-200,27,-200,28,-200},new int[]{-43,1363,-153,1364,-26,1365,-48,1366,-273,1367,-292,1368,-205,1369,-6,1370,-234,949});
    states[1363] = new State(-61);
    states[1364] = new State(-71);
    states[1365] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,56,-72,26,-72,64,-72,47,-72,50,-72,59,-72,11,-72,41,-72,34,-72,25,-72,23,-72,27,-72,28,-72,85,-72,86,-72,97,-72},new int[]{-24,1112,-25,1113,-126,1115,-132,1125,-136,24,-137,27});
    states[1366] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,56,-73,26,-73,64,-73,47,-73,50,-73,59,-73,11,-73,41,-73,34,-73,25,-73,23,-73,27,-73,28,-73,85,-73,86,-73,97,-73},new int[]{-24,1129,-25,1113,-126,1115,-132,1125,-136,24,-137,27});
    states[1367] = new State(new int[]{11,611,56,-74,26,-74,64,-74,47,-74,50,-74,59,-74,41,-74,34,-74,25,-74,23,-74,27,-74,28,-74,85,-74,86,-74,97,-74,137,-200,80,-200,81,-200,75,-200,73,-200},new int[]{-45,1133,-6,1134,-234,949});
    states[1368] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,8,1297,56,-75,26,-75,64,-75,47,-75,50,-75,59,-75,11,-75,41,-75,34,-75,25,-75,23,-75,27,-75,28,-75,85,-75,86,-75,97,-75},new int[]{-296,1293,-293,1294,-294,1295,-143,721,-132,720,-136,24,-137,27});
    states[1369] = new State(-76);
    states[1370] = new State(new int[]{41,1383,34,1390,25,1222,23,1223,27,1418,28,1244,11,611},new int[]{-198,1371,-234,475,-199,1372,-206,1373,-213,1374,-210,1167,-214,1202,-3,1407,-202,1415,-212,1416});
    states[1371] = new State(-79);
    states[1372] = new State(-77);
    states[1373] = new State(-412);
    states[1374] = new State(new int[]{142,1376,101,1228,56,-60,26,-60,64,-60,47,-60,50,-60,59,-60,11,-60,41,-60,34,-60,25,-60,23,-60,27,-60,28,-60,85,-60},new int[]{-164,1375,-163,1378,-38,1379,-39,1362,-57,1382});
    states[1375] = new State(-414);
    states[1376] = new State(new int[]{10,1377});
    states[1377] = new State(-420);
    states[1378] = new State(-427);
    states[1379] = new State(new int[]{85,116},new int[]{-239,1380});
    states[1380] = new State(new int[]{10,1381});
    states[1381] = new State(-449);
    states[1382] = new State(-428);
    states[1383] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359},new int[]{-156,1384,-155,1033,-127,1034,-122,1035,-119,1036,-132,1041,-136,24,-137,27,-177,1042,-317,1044,-134,1048});
    states[1384] = new State(new int[]{8,557,10,-452,104,-452},new int[]{-113,1385});
    states[1385] = new State(new int[]{10,1200,104,-745},new int[]{-193,1171,-194,1386});
    states[1386] = new State(new int[]{104,1387});
    states[1387] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476},new int[]{-244,1388,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[1388] = new State(new int[]{10,1389});
    states[1389] = new State(-419);
    states[1390] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359},new int[]{-155,1391,-127,1034,-122,1035,-119,1036,-132,1041,-136,24,-137,27,-177,1042,-317,1044,-134,1048});
    states[1391] = new State(new int[]{8,557,5,-452,10,-452,104,-452},new int[]{-113,1392});
    states[1392] = new State(new int[]{5,1393,10,1200,104,-745},new int[]{-193,1206,-194,1401});
    states[1393] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-259,1394,-260,410,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[1394] = new State(new int[]{10,1200,104,-745},new int[]{-193,1210,-194,1395});
    states[1395] = new State(new int[]{104,1396});
    states[1396] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,502,18,247,19,252,34,625,41,856},new int[]{-91,1397,-305,1399,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,352,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-306,624});
    states[1397] = new State(new int[]{10,1398,13,128});
    states[1398] = new State(-415);
    states[1399] = new State(new int[]{10,1400});
    states[1400] = new State(-417);
    states[1401] = new State(new int[]{104,1402});
    states[1402] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,502,18,247,19,252,34,625,41,856},new int[]{-91,1403,-305,1405,-90,132,-89,290,-93,356,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,352,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-306,624});
    states[1403] = new State(new int[]{10,1404,13,128});
    states[1404] = new State(-416);
    states[1405] = new State(new int[]{10,1406});
    states[1406] = new State(-418);
    states[1407] = new State(new int[]{27,1409,41,1383,34,1390},new int[]{-206,1408,-213,1374,-210,1167,-214,1202});
    states[1408] = new State(-413);
    states[1409] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359,8,-364,104,-364,10,-364},new int[]{-157,1410,-156,1032,-155,1033,-127,1034,-122,1035,-119,1036,-132,1041,-136,24,-137,27,-177,1042,-317,1044,-134,1048});
    states[1410] = new State(new int[]{8,557,104,-452,10,-452},new int[]{-113,1411});
    states[1411] = new State(new int[]{104,1412,10,1021},new int[]{-193,483});
    states[1412] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476},new int[]{-244,1413,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[1413] = new State(new int[]{10,1414});
    states[1414] = new State(-408);
    states[1415] = new State(-78);
    states[1416] = new State(-60,new int[]{-163,1417,-38,1379,-39,1362});
    states[1417] = new State(-406);
    states[1418] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359,8,-364,104,-364,10,-364},new int[]{-157,1419,-156,1032,-155,1033,-127,1034,-122,1035,-119,1036,-132,1041,-136,24,-137,27,-177,1042,-317,1044,-134,1048});
    states[1419] = new State(new int[]{8,557,104,-452,10,-452},new int[]{-113,1420});
    states[1420] = new State(new int[]{104,1421,10,1021},new int[]{-193,1240});
    states[1421] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,154,150,155,149,156,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,10,-476},new int[]{-244,1422,-4,122,-100,123,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754});
    states[1422] = new State(new int[]{10,1423});
    states[1423] = new State(-407);
    states[1424] = new State(new int[]{3,1426,49,-13,85,-13,56,-13,26,-13,64,-13,47,-13,50,-13,59,-13,11,-13,41,-13,34,-13,25,-13,23,-13,27,-13,28,-13,40,-13,86,-13,97,-13},new int[]{-170,1425});
    states[1425] = new State(-15);
    states[1426] = new State(new int[]{137,1427,138,1428});
    states[1427] = new State(-16);
    states[1428] = new State(-17);
    states[1429] = new State(-14);
    states[1430] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-132,1431,-136,24,-137,27});
    states[1431] = new State(new int[]{10,1433,8,1434},new int[]{-173,1432});
    states[1432] = new State(-26);
    states[1433] = new State(-27);
    states[1434] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-175,1435,-131,1441,-132,1440,-136,24,-137,27});
    states[1435] = new State(new int[]{9,1436,94,1438});
    states[1436] = new State(new int[]{10,1437});
    states[1437] = new State(-28);
    states[1438] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-131,1439,-132,1440,-136,24,-137,27});
    states[1439] = new State(-30);
    states[1440] = new State(-31);
    states[1441] = new State(-29);
    states[1442] = new State(-3);
    states[1443] = new State(new int[]{99,1498,100,1499,103,1500,11,611},new int[]{-291,1444,-234,475,-2,1493});
    states[1444] = new State(new int[]{40,1465,49,-36,56,-36,26,-36,64,-36,47,-36,50,-36,59,-36,11,-36,41,-36,34,-36,25,-36,23,-36,27,-36,28,-36,86,-36,97,-36,85,-36},new int[]{-147,1445,-148,1462,-287,1491});
    states[1445] = new State(new int[]{38,1459},new int[]{-146,1446});
    states[1446] = new State(new int[]{86,1449,97,1450,85,1456},new int[]{-139,1447});
    states[1447] = new State(new int[]{7,1448});
    states[1448] = new State(-42);
    states[1449] = new State(-52);
    states[1450] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,664,150,155,149,665,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,98,-476,10,-476},new int[]{-236,1451,-245,662,-244,121,-4,122,-100,123,-117,338,-99,345,-132,663,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754,-128,890});
    states[1451] = new State(new int[]{86,1452,98,1453,10,119});
    states[1452] = new State(-53);
    states[1453] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,664,150,155,149,665,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476},new int[]{-236,1454,-245,662,-244,121,-4,122,-100,123,-117,338,-99,345,-132,663,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754,-128,890});
    states[1454] = new State(new int[]{86,1455,10,119});
    states[1455] = new State(-54);
    states[1456] = new State(new int[]{135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,487,8,488,18,247,19,252,138,149,140,150,139,152,148,664,150,155,149,665,54,642,85,116,37,632,22,650,91,667,51,672,32,677,52,687,96,693,44,700,33,703,50,711,57,743,72,748,70,735,35,755,86,-476,10,-476},new int[]{-236,1457,-245,662,-244,121,-4,122,-100,123,-117,338,-99,345,-132,663,-136,24,-137,27,-177,358,-241,495,-279,496,-14,638,-150,146,-152,147,-151,151,-15,153,-16,497,-54,639,-103,519,-197,640,-118,641,-239,647,-138,648,-32,649,-231,666,-301,671,-109,676,-302,686,-145,691,-286,692,-232,699,-108,702,-297,710,-55,739,-160,740,-159,741,-154,742,-111,747,-112,752,-110,753,-325,754,-128,890});
    states[1457] = new State(new int[]{86,1458,10,119});
    states[1458] = new State(-55);
    states[1459] = new State(-36,new int[]{-287,1460});
    states[1460] = new State(new int[]{49,14,56,-60,26,-60,64,-60,47,-60,50,-60,59,-60,11,-60,41,-60,34,-60,25,-60,23,-60,27,-60,28,-60,86,-60,97,-60,85,-60},new int[]{-38,1461,-39,1362});
    states[1461] = new State(-50);
    states[1462] = new State(new int[]{86,1449,97,1450,85,1456},new int[]{-139,1463});
    states[1463] = new State(new int[]{7,1464});
    states[1464] = new State(-43);
    states[1465] = new State(-36,new int[]{-287,1466});
    states[1466] = new State(new int[]{49,14,26,-57,64,-57,47,-57,50,-57,59,-57,11,-57,41,-57,34,-57,38,-57},new int[]{-37,1467,-35,1468});
    states[1467] = new State(-49);
    states[1468] = new State(new int[]{26,1126,64,1130,47,1290,50,1305,59,1307,11,611,38,-56,41,-200,34,-200},new int[]{-44,1469,-26,1470,-48,1471,-273,1472,-292,1473,-217,1474,-6,1475,-234,949,-216,1490});
    states[1469] = new State(-58);
    states[1470] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,26,-65,64,-65,47,-65,50,-65,59,-65,11,-65,41,-65,34,-65,38,-65},new int[]{-24,1112,-25,1113,-126,1115,-132,1125,-136,24,-137,27});
    states[1471] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,26,-66,64,-66,47,-66,50,-66,59,-66,11,-66,41,-66,34,-66,38,-66},new int[]{-24,1129,-25,1113,-126,1115,-132,1125,-136,24,-137,27});
    states[1472] = new State(new int[]{11,611,26,-67,64,-67,47,-67,50,-67,59,-67,41,-67,34,-67,38,-67,137,-200,80,-200,81,-200,75,-200,73,-200},new int[]{-45,1133,-6,1134,-234,949});
    states[1473] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,8,1297,26,-68,64,-68,47,-68,50,-68,59,-68,11,-68,41,-68,34,-68,38,-68},new int[]{-296,1293,-293,1294,-294,1295,-143,721,-132,720,-136,24,-137,27});
    states[1474] = new State(-69);
    states[1475] = new State(new int[]{41,1482,11,611,34,1485},new int[]{-210,1476,-234,475,-214,1479});
    states[1476] = new State(new int[]{142,1477,26,-85,64,-85,47,-85,50,-85,59,-85,11,-85,41,-85,34,-85,38,-85});
    states[1477] = new State(new int[]{10,1478});
    states[1478] = new State(-86);
    states[1479] = new State(new int[]{142,1480,26,-87,64,-87,47,-87,50,-87,59,-87,11,-87,41,-87,34,-87,38,-87});
    states[1480] = new State(new int[]{10,1481});
    states[1481] = new State(-88);
    states[1482] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359},new int[]{-156,1483,-155,1033,-127,1034,-122,1035,-119,1036,-132,1041,-136,24,-137,27,-177,1042,-317,1044,-134,1048});
    states[1483] = new State(new int[]{8,557,10,-452},new int[]{-113,1484});
    states[1484] = new State(new int[]{10,1021},new int[]{-193,1171});
    states[1485] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,359},new int[]{-155,1486,-127,1034,-122,1035,-119,1036,-132,1041,-136,24,-137,27,-177,1042,-317,1044,-134,1048});
    states[1486] = new State(new int[]{8,557,5,-452,10,-452},new int[]{-113,1487});
    states[1487] = new State(new int[]{5,1488,10,1021},new int[]{-193,1206});
    states[1488] = new State(new int[]{137,413,80,25,81,26,75,28,73,29,148,154,150,155,149,156,110,335,109,336,138,149,140,150,139,152,8,441,136,446,21,451,45,459,46,539,31,543,71,547,62,550,41,555,34,579},new int[]{-259,1489,-260,410,-256,411,-85,175,-94,267,-95,268,-166,269,-132,196,-136,24,-137,27,-15,436,-185,437,-150,440,-152,147,-151,151,-257,443,-240,444,-233,445,-265,448,-266,449,-262,450,-254,457,-28,458,-247,538,-115,542,-116,546,-211,552,-209,553,-208,554,-285,583});
    states[1489] = new State(new int[]{10,1021},new int[]{-193,1210});
    states[1490] = new State(-70);
    states[1491] = new State(new int[]{49,14,56,-60,26,-60,64,-60,47,-60,50,-60,59,-60,11,-60,41,-60,34,-60,25,-60,23,-60,27,-60,28,-60,86,-60,97,-60,85,-60},new int[]{-38,1492,-39,1362});
    states[1492] = new State(-51);
    states[1493] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-124,1494,-132,1497,-136,24,-137,27});
    states[1494] = new State(new int[]{10,1495});
    states[1495] = new State(new int[]{3,1426,40,-12,86,-12,97,-12,85,-12,49,-12,56,-12,26,-12,64,-12,47,-12,50,-12,59,-12,11,-12,41,-12,34,-12,25,-12,23,-12,27,-12,28,-12},new int[]{-171,1496,-172,1424,-170,1429});
    states[1496] = new State(-44);
    states[1497] = new State(-48);
    states[1498] = new State(-46);
    states[1499] = new State(-47);
    states[1500] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,30,64,31,65,24,66,98,67,95,68,32,69,33,70,34,71,37,72,38,73,39,74,97,75,40,76,41,77,43,78,44,79,45,80,91,81,46,82,96,83,47,84,25,85,48,86,68,87,92,88,49,89,50,90,51,91,52,92,53,93,54,94,55,95,56,96,58,97,99,98,100,99,103,100,101,101,102,102,59,103,72,104,35,105,36,106,42,108,86,109},new int[]{-142,1501,-123,112,-132,22,-136,24,-137,27,-277,30,-135,31,-278,107});
    states[1501] = new State(new int[]{10,1502,7,20});
    states[1502] = new State(new int[]{3,1426,40,-12,86,-12,97,-12,85,-12,49,-12,56,-12,26,-12,64,-12,47,-12,50,-12,59,-12,11,-12,41,-12,34,-12,25,-12,23,-12,27,-12,28,-12},new int[]{-171,1503,-172,1424,-170,1429});
    states[1503] = new State(-45);
    states[1504] = new State(-4);
    states[1505] = new State(new int[]{47,1507,53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,391,18,247,19,252,5,573},new int[]{-81,1506,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,337,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572});
    states[1506] = new State(-5);
    states[1507] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-129,1508,-132,1509,-136,24,-137,27});
    states[1508] = new State(-6);
    states[1509] = new State(new int[]{117,1038,2,-208},new int[]{-140,1283});
    states[1510] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-303,1511,-304,1512,-132,1516,-136,24,-137,27});
    states[1511] = new State(-7);
    states[1512] = new State(new int[]{7,1513,117,168,2,-710},new int[]{-283,1515});
    states[1513] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,30,64,31,65,24,66,98,67,95,68,32,69,33,70,34,71,37,72,38,73,39,74,97,75,40,76,41,77,43,78,44,79,45,80,91,81,46,82,96,83,47,84,25,85,48,86,68,87,92,88,49,89,50,90,51,91,52,92,53,93,54,94,55,95,56,96,58,97,99,98,100,99,103,100,101,101,102,102,59,103,72,104,35,105,36,106,42,108,86,109},new int[]{-123,1514,-132,22,-136,24,-137,27,-277,30,-135,31,-278,107});
    states[1514] = new State(-709);
    states[1515] = new State(-711);
    states[1516] = new State(-708);
    states[1517] = new State(new int[]{53,142,138,149,140,150,139,152,148,154,150,155,149,156,60,158,11,322,129,331,110,335,109,336,135,344,137,23,80,25,81,26,75,28,73,227,42,359,39,389,8,488,18,247,19,252,5,573,50,711},new int[]{-243,1518,-81,1519,-91,127,-90,132,-89,290,-93,298,-76,308,-88,321,-14,143,-150,146,-152,147,-151,151,-15,153,-53,157,-185,333,-100,1520,-117,338,-99,345,-132,357,-136,24,-137,27,-177,358,-241,495,-279,496,-16,497,-54,513,-103,519,-159,520,-252,521,-77,522,-248,525,-250,526,-251,570,-225,571,-105,572,-4,1521,-297,1522});
    states[1518] = new State(-8);
    states[1519] = new State(-9);
    states[1520] = new State(new int[]{104,383,105,384,106,385,107,386,108,387,132,-695,130,-695,112,-695,111,-695,125,-695,126,-695,127,-695,128,-695,124,-695,5,-695,110,-695,109,-695,122,-695,123,-695,120,-695,114,-695,119,-695,117,-695,115,-695,118,-695,116,-695,131,-695,16,-695,13,-695,2,-695,113,-695},new int[]{-180,124});
    states[1521] = new State(-10);
    states[1522] = new State(-11);

    rules[1] = new Rule(-337, new int[]{-1,2});
    rules[2] = new Rule(-1, new int[]{-219});
    rules[3] = new Rule(-1, new int[]{-289});
    rules[4] = new Rule(-1, new int[]{-161});
    rules[5] = new Rule(-161, new int[]{82,-81});
    rules[6] = new Rule(-161, new int[]{82,47,-129});
    rules[7] = new Rule(-161, new int[]{84,-303});
    rules[8] = new Rule(-161, new int[]{83,-243});
    rules[9] = new Rule(-243, new int[]{-81});
    rules[10] = new Rule(-243, new int[]{-4});
    rules[11] = new Rule(-243, new int[]{-297});
    rules[12] = new Rule(-171, new int[]{});
    rules[13] = new Rule(-171, new int[]{-172});
    rules[14] = new Rule(-172, new int[]{-170});
    rules[15] = new Rule(-172, new int[]{-172,-170});
    rules[16] = new Rule(-170, new int[]{3,137});
    rules[17] = new Rule(-170, new int[]{3,138});
    rules[18] = new Rule(-219, new int[]{-220,-171,-287,-17,-174});
    rules[19] = new Rule(-174, new int[]{7});
    rules[20] = new Rule(-174, new int[]{10});
    rules[21] = new Rule(-174, new int[]{5});
    rules[22] = new Rule(-174, new int[]{94});
    rules[23] = new Rule(-174, new int[]{6});
    rules[24] = new Rule(-174, new int[]{});
    rules[25] = new Rule(-220, new int[]{});
    rules[26] = new Rule(-220, new int[]{58,-132,-173});
    rules[27] = new Rule(-173, new int[]{10});
    rules[28] = new Rule(-173, new int[]{8,-175,9,10});
    rules[29] = new Rule(-175, new int[]{-131});
    rules[30] = new Rule(-175, new int[]{-175,94,-131});
    rules[31] = new Rule(-131, new int[]{-132});
    rules[32] = new Rule(-17, new int[]{-34,-239});
    rules[33] = new Rule(-34, new int[]{-38});
    rules[34] = new Rule(-142, new int[]{-123});
    rules[35] = new Rule(-142, new int[]{-142,7,-123});
    rules[36] = new Rule(-287, new int[]{});
    rules[37] = new Rule(-287, new int[]{-287,49,-288,10});
    rules[38] = new Rule(-288, new int[]{-290});
    rules[39] = new Rule(-288, new int[]{-288,94,-290});
    rules[40] = new Rule(-290, new int[]{-142});
    rules[41] = new Rule(-290, new int[]{-142,131,138});
    rules[42] = new Rule(-289, new int[]{-6,-291,-147,-146,-139,7});
    rules[43] = new Rule(-289, new int[]{-6,-291,-148,-139,7});
    rules[44] = new Rule(-291, new int[]{-2,-124,10,-171});
    rules[45] = new Rule(-291, new int[]{103,-142,10,-171});
    rules[46] = new Rule(-2, new int[]{99});
    rules[47] = new Rule(-2, new int[]{100});
    rules[48] = new Rule(-124, new int[]{-132});
    rules[49] = new Rule(-147, new int[]{40,-287,-37});
    rules[50] = new Rule(-146, new int[]{38,-287,-38});
    rules[51] = new Rule(-148, new int[]{-287,-38});
    rules[52] = new Rule(-139, new int[]{86});
    rules[53] = new Rule(-139, new int[]{97,-236,86});
    rules[54] = new Rule(-139, new int[]{97,-236,98,-236,86});
    rules[55] = new Rule(-139, new int[]{85,-236,86});
    rules[56] = new Rule(-37, new int[]{-35});
    rules[57] = new Rule(-35, new int[]{});
    rules[58] = new Rule(-35, new int[]{-35,-44});
    rules[59] = new Rule(-38, new int[]{-39});
    rules[60] = new Rule(-39, new int[]{});
    rules[61] = new Rule(-39, new int[]{-39,-43});
    rules[62] = new Rule(-40, new int[]{-36});
    rules[63] = new Rule(-36, new int[]{});
    rules[64] = new Rule(-36, new int[]{-36,-42});
    rules[65] = new Rule(-44, new int[]{-26});
    rules[66] = new Rule(-44, new int[]{-48});
    rules[67] = new Rule(-44, new int[]{-273});
    rules[68] = new Rule(-44, new int[]{-292});
    rules[69] = new Rule(-44, new int[]{-217});
    rules[70] = new Rule(-44, new int[]{-216});
    rules[71] = new Rule(-43, new int[]{-153});
    rules[72] = new Rule(-43, new int[]{-26});
    rules[73] = new Rule(-43, new int[]{-48});
    rules[74] = new Rule(-43, new int[]{-273});
    rules[75] = new Rule(-43, new int[]{-292});
    rules[76] = new Rule(-43, new int[]{-205});
    rules[77] = new Rule(-198, new int[]{-199});
    rules[78] = new Rule(-198, new int[]{-202});
    rules[79] = new Rule(-205, new int[]{-6,-198});
    rules[80] = new Rule(-42, new int[]{-153});
    rules[81] = new Rule(-42, new int[]{-26});
    rules[82] = new Rule(-42, new int[]{-48});
    rules[83] = new Rule(-42, new int[]{-273});
    rules[84] = new Rule(-42, new int[]{-292});
    rules[85] = new Rule(-217, new int[]{-6,-210});
    rules[86] = new Rule(-217, new int[]{-6,-210,142,10});
    rules[87] = new Rule(-216, new int[]{-6,-214});
    rules[88] = new Rule(-216, new int[]{-6,-214,142,10});
    rules[89] = new Rule(-153, new int[]{56,-141,10});
    rules[90] = new Rule(-141, new int[]{-128});
    rules[91] = new Rule(-141, new int[]{-141,94,-128});
    rules[92] = new Rule(-128, new int[]{148});
    rules[93] = new Rule(-128, new int[]{149});
    rules[94] = new Rule(-128, new int[]{-132});
    rules[95] = new Rule(-26, new int[]{26,-24});
    rules[96] = new Rule(-26, new int[]{-26,-24});
    rules[97] = new Rule(-48, new int[]{64,-24});
    rules[98] = new Rule(-48, new int[]{-48,-24});
    rules[99] = new Rule(-273, new int[]{47,-45});
    rules[100] = new Rule(-273, new int[]{-273,-45});
    rules[101] = new Rule(-296, new int[]{-293});
    rules[102] = new Rule(-296, new int[]{8,-132,94,-143,9,104,-91,10});
    rules[103] = new Rule(-292, new int[]{50,-296});
    rules[104] = new Rule(-292, new int[]{59,-296});
    rules[105] = new Rule(-292, new int[]{-292,-296});
    rules[106] = new Rule(-24, new int[]{-25,10});
    rules[107] = new Rule(-25, new int[]{-126,114,-97});
    rules[108] = new Rule(-25, new int[]{-126,5,-260,114,-78});
    rules[109] = new Rule(-97, new int[]{-83});
    rules[110] = new Rule(-97, new int[]{-87});
    rules[111] = new Rule(-126, new int[]{-132});
    rules[112] = new Rule(-73, new int[]{-91});
    rules[113] = new Rule(-73, new int[]{-73,94,-91});
    rules[114] = new Rule(-83, new int[]{-75});
    rules[115] = new Rule(-83, new int[]{-75,-178,-75});
    rules[116] = new Rule(-83, new int[]{-226});
    rules[117] = new Rule(-226, new int[]{-83,13,-83,5,-83});
    rules[118] = new Rule(-178, new int[]{114});
    rules[119] = new Rule(-178, new int[]{119});
    rules[120] = new Rule(-178, new int[]{117});
    rules[121] = new Rule(-178, new int[]{115});
    rules[122] = new Rule(-178, new int[]{118});
    rules[123] = new Rule(-178, new int[]{116});
    rules[124] = new Rule(-178, new int[]{131});
    rules[125] = new Rule(-75, new int[]{-12});
    rules[126] = new Rule(-75, new int[]{-75,-179,-12});
    rules[127] = new Rule(-179, new int[]{110});
    rules[128] = new Rule(-179, new int[]{109});
    rules[129] = new Rule(-179, new int[]{122});
    rules[130] = new Rule(-179, new int[]{123});
    rules[131] = new Rule(-249, new int[]{-12,-187,-268});
    rules[132] = new Rule(-253, new int[]{-10,113,-10});
    rules[133] = new Rule(-12, new int[]{-10});
    rules[134] = new Rule(-12, new int[]{-249});
    rules[135] = new Rule(-12, new int[]{-253});
    rules[136] = new Rule(-12, new int[]{-12,-181,-10});
    rules[137] = new Rule(-12, new int[]{-12,-181,-253});
    rules[138] = new Rule(-181, new int[]{112});
    rules[139] = new Rule(-181, new int[]{111});
    rules[140] = new Rule(-181, new int[]{125});
    rules[141] = new Rule(-181, new int[]{126});
    rules[142] = new Rule(-181, new int[]{127});
    rules[143] = new Rule(-181, new int[]{128});
    rules[144] = new Rule(-181, new int[]{124});
    rules[145] = new Rule(-10, new int[]{-13});
    rules[146] = new Rule(-10, new int[]{-224});
    rules[147] = new Rule(-10, new int[]{53});
    rules[148] = new Rule(-10, new int[]{135,-10});
    rules[149] = new Rule(-10, new int[]{8,-83,9});
    rules[150] = new Rule(-10, new int[]{129,-10});
    rules[151] = new Rule(-10, new int[]{-185,-10});
    rules[152] = new Rule(-10, new int[]{-159});
    rules[153] = new Rule(-224, new int[]{11,-69,12});
    rules[154] = new Rule(-185, new int[]{110});
    rules[155] = new Rule(-185, new int[]{109});
    rules[156] = new Rule(-13, new int[]{-132});
    rules[157] = new Rule(-13, new int[]{-150});
    rules[158] = new Rule(-13, new int[]{-15});
    rules[159] = new Rule(-13, new int[]{39,-132});
    rules[160] = new Rule(-13, new int[]{-241});
    rules[161] = new Rule(-13, new int[]{-279});
    rules[162] = new Rule(-13, new int[]{-13,-11});
    rules[163] = new Rule(-13, new int[]{-13,4,-283});
    rules[164] = new Rule(-13, new int[]{-13,11,-106,12});
    rules[165] = new Rule(-11, new int[]{7,-123});
    rules[166] = new Rule(-11, new int[]{136});
    rules[167] = new Rule(-11, new int[]{8,-70,9});
    rules[168] = new Rule(-11, new int[]{11,-69,12});
    rules[169] = new Rule(-70, new int[]{-66});
    rules[170] = new Rule(-70, new int[]{});
    rules[171] = new Rule(-69, new int[]{-67});
    rules[172] = new Rule(-69, new int[]{});
    rules[173] = new Rule(-67, new int[]{-86});
    rules[174] = new Rule(-67, new int[]{-67,94,-86});
    rules[175] = new Rule(-86, new int[]{-83});
    rules[176] = new Rule(-86, new int[]{-83,6,-83});
    rules[177] = new Rule(-15, new int[]{148});
    rules[178] = new Rule(-15, new int[]{150});
    rules[179] = new Rule(-15, new int[]{149});
    rules[180] = new Rule(-78, new int[]{-83});
    rules[181] = new Rule(-78, new int[]{-87});
    rules[182] = new Rule(-78, new int[]{-227});
    rules[183] = new Rule(-87, new int[]{8,-62,9});
    rules[184] = new Rule(-87, new int[]{8,-227,9});
    rules[185] = new Rule(-87, new int[]{8,-87,9});
    rules[186] = new Rule(-62, new int[]{});
    rules[187] = new Rule(-62, new int[]{-61});
    rules[188] = new Rule(-61, new int[]{-79});
    rules[189] = new Rule(-61, new int[]{-61,94,-79});
    rules[190] = new Rule(-227, new int[]{8,-229,9});
    rules[191] = new Rule(-229, new int[]{-228});
    rules[192] = new Rule(-229, new int[]{-228,10});
    rules[193] = new Rule(-228, new int[]{-230});
    rules[194] = new Rule(-228, new int[]{-228,10,-230});
    rules[195] = new Rule(-230, new int[]{-121,5,-78});
    rules[196] = new Rule(-121, new int[]{-132});
    rules[197] = new Rule(-45, new int[]{-6,-46});
    rules[198] = new Rule(-6, new int[]{-234});
    rules[199] = new Rule(-6, new int[]{-6,-234});
    rules[200] = new Rule(-6, new int[]{});
    rules[201] = new Rule(-234, new int[]{11,-235,12});
    rules[202] = new Rule(-235, new int[]{-8});
    rules[203] = new Rule(-235, new int[]{-235,94,-8});
    rules[204] = new Rule(-8, new int[]{-9});
    rules[205] = new Rule(-8, new int[]{-132,5,-9});
    rules[206] = new Rule(-46, new int[]{-129,114,-271,10});
    rules[207] = new Rule(-46, new int[]{-130,-271,10});
    rules[208] = new Rule(-129, new int[]{-132});
    rules[209] = new Rule(-129, new int[]{-132,-140});
    rules[210] = new Rule(-130, new int[]{-132,117,-143,116});
    rules[211] = new Rule(-271, new int[]{-260});
    rules[212] = new Rule(-271, new int[]{-27});
    rules[213] = new Rule(-257, new int[]{-256,13});
    rules[214] = new Rule(-260, new int[]{-256});
    rules[215] = new Rule(-260, new int[]{-257});
    rules[216] = new Rule(-260, new int[]{-240});
    rules[217] = new Rule(-260, new int[]{-233});
    rules[218] = new Rule(-260, new int[]{-265});
    rules[219] = new Rule(-260, new int[]{-211});
    rules[220] = new Rule(-260, new int[]{-285});
    rules[221] = new Rule(-285, new int[]{-166,-283});
    rules[222] = new Rule(-283, new int[]{117,-281,115});
    rules[223] = new Rule(-284, new int[]{119});
    rules[224] = new Rule(-284, new int[]{117,-282,115});
    rules[225] = new Rule(-281, new int[]{-263});
    rules[226] = new Rule(-281, new int[]{-281,94,-263});
    rules[227] = new Rule(-282, new int[]{-264});
    rules[228] = new Rule(-282, new int[]{-282,94,-264});
    rules[229] = new Rule(-264, new int[]{});
    rules[230] = new Rule(-263, new int[]{-256});
    rules[231] = new Rule(-263, new int[]{-256,13});
    rules[232] = new Rule(-263, new int[]{-265});
    rules[233] = new Rule(-263, new int[]{-211});
    rules[234] = new Rule(-263, new int[]{-285});
    rules[235] = new Rule(-256, new int[]{-85});
    rules[236] = new Rule(-256, new int[]{-85,6,-85});
    rules[237] = new Rule(-256, new int[]{8,-74,9});
    rules[238] = new Rule(-85, new int[]{-94});
    rules[239] = new Rule(-85, new int[]{-85,-179,-94});
    rules[240] = new Rule(-94, new int[]{-95});
    rules[241] = new Rule(-94, new int[]{-94,-181,-95});
    rules[242] = new Rule(-95, new int[]{-166});
    rules[243] = new Rule(-95, new int[]{-15});
    rules[244] = new Rule(-95, new int[]{-185,-95});
    rules[245] = new Rule(-95, new int[]{-150});
    rules[246] = new Rule(-95, new int[]{-95,8,-69,9});
    rules[247] = new Rule(-166, new int[]{-132});
    rules[248] = new Rule(-166, new int[]{-166,7,-123});
    rules[249] = new Rule(-74, new int[]{-72,94,-72});
    rules[250] = new Rule(-74, new int[]{-74,94,-72});
    rules[251] = new Rule(-72, new int[]{-260});
    rules[252] = new Rule(-72, new int[]{-260,114,-81});
    rules[253] = new Rule(-233, new int[]{136,-259});
    rules[254] = new Rule(-265, new int[]{-266});
    rules[255] = new Rule(-265, new int[]{62,-266});
    rules[256] = new Rule(-266, new int[]{-262});
    rules[257] = new Rule(-266, new int[]{-28});
    rules[258] = new Rule(-266, new int[]{-247});
    rules[259] = new Rule(-266, new int[]{-115});
    rules[260] = new Rule(-266, new int[]{-116});
    rules[261] = new Rule(-116, new int[]{71,55,-260});
    rules[262] = new Rule(-262, new int[]{21,11,-149,12,55,-260});
    rules[263] = new Rule(-262, new int[]{-254});
    rules[264] = new Rule(-254, new int[]{21,55,-260});
    rules[265] = new Rule(-149, new int[]{-255});
    rules[266] = new Rule(-149, new int[]{-149,94,-255});
    rules[267] = new Rule(-255, new int[]{-256});
    rules[268] = new Rule(-255, new int[]{});
    rules[269] = new Rule(-247, new int[]{46,55,-260});
    rules[270] = new Rule(-115, new int[]{31,55,-260});
    rules[271] = new Rule(-115, new int[]{31});
    rules[272] = new Rule(-240, new int[]{137,11,-83,12});
    rules[273] = new Rule(-211, new int[]{-209});
    rules[274] = new Rule(-209, new int[]{-208});
    rules[275] = new Rule(-208, new int[]{41,-113});
    rules[276] = new Rule(-208, new int[]{34,-113,5,-259});
    rules[277] = new Rule(-208, new int[]{-166,121,-263});
    rules[278] = new Rule(-208, new int[]{-285,121,-263});
    rules[279] = new Rule(-208, new int[]{8,9,121,-263});
    rules[280] = new Rule(-208, new int[]{8,-74,9,121,-263});
    rules[281] = new Rule(-208, new int[]{-166,121,8,9});
    rules[282] = new Rule(-208, new int[]{-285,121,8,9});
    rules[283] = new Rule(-208, new int[]{8,9,121,8,9});
    rules[284] = new Rule(-208, new int[]{8,-74,9,121,8,9});
    rules[285] = new Rule(-27, new int[]{-20,-275,-169,-300,-23});
    rules[286] = new Rule(-28, new int[]{45,-169,-300,-22,86});
    rules[287] = new Rule(-19, new int[]{66});
    rules[288] = new Rule(-19, new int[]{67});
    rules[289] = new Rule(-19, new int[]{141});
    rules[290] = new Rule(-19, new int[]{24});
    rules[291] = new Rule(-19, new int[]{25});
    rules[292] = new Rule(-20, new int[]{});
    rules[293] = new Rule(-20, new int[]{-21});
    rules[294] = new Rule(-21, new int[]{-19});
    rules[295] = new Rule(-21, new int[]{-21,-19});
    rules[296] = new Rule(-275, new int[]{23});
    rules[297] = new Rule(-275, new int[]{40});
    rules[298] = new Rule(-275, new int[]{61});
    rules[299] = new Rule(-275, new int[]{61,23});
    rules[300] = new Rule(-275, new int[]{61,45});
    rules[301] = new Rule(-275, new int[]{61,40});
    rules[302] = new Rule(-23, new int[]{});
    rules[303] = new Rule(-23, new int[]{-22,86});
    rules[304] = new Rule(-169, new int[]{});
    rules[305] = new Rule(-169, new int[]{8,-168,9});
    rules[306] = new Rule(-168, new int[]{-167});
    rules[307] = new Rule(-168, new int[]{-168,94,-167});
    rules[308] = new Rule(-167, new int[]{-166});
    rules[309] = new Rule(-167, new int[]{-285});
    rules[310] = new Rule(-140, new int[]{117,-143,115});
    rules[311] = new Rule(-300, new int[]{});
    rules[312] = new Rule(-300, new int[]{-299});
    rules[313] = new Rule(-299, new int[]{-298});
    rules[314] = new Rule(-299, new int[]{-299,-298});
    rules[315] = new Rule(-298, new int[]{20,-143,5,-272,10});
    rules[316] = new Rule(-272, new int[]{-269});
    rules[317] = new Rule(-272, new int[]{-272,94,-269});
    rules[318] = new Rule(-269, new int[]{-260});
    rules[319] = new Rule(-269, new int[]{23});
    rules[320] = new Rule(-269, new int[]{45});
    rules[321] = new Rule(-269, new int[]{27});
    rules[322] = new Rule(-22, new int[]{-29});
    rules[323] = new Rule(-22, new int[]{-22,-7,-29});
    rules[324] = new Rule(-7, new int[]{79});
    rules[325] = new Rule(-7, new int[]{78});
    rules[326] = new Rule(-7, new int[]{77});
    rules[327] = new Rule(-7, new int[]{76});
    rules[328] = new Rule(-29, new int[]{});
    rules[329] = new Rule(-29, new int[]{-31,-176});
    rules[330] = new Rule(-29, new int[]{-30});
    rules[331] = new Rule(-29, new int[]{-31,10,-30});
    rules[332] = new Rule(-143, new int[]{-132});
    rules[333] = new Rule(-143, new int[]{-143,94,-132});
    rules[334] = new Rule(-176, new int[]{});
    rules[335] = new Rule(-176, new int[]{10});
    rules[336] = new Rule(-31, new int[]{-41});
    rules[337] = new Rule(-31, new int[]{-31,10,-41});
    rules[338] = new Rule(-41, new int[]{-6,-47});
    rules[339] = new Rule(-30, new int[]{-50});
    rules[340] = new Rule(-30, new int[]{-30,-50});
    rules[341] = new Rule(-50, new int[]{-49});
    rules[342] = new Rule(-50, new int[]{-51});
    rules[343] = new Rule(-47, new int[]{26,-25});
    rules[344] = new Rule(-47, new int[]{-295});
    rules[345] = new Rule(-47, new int[]{-3,-295});
    rules[346] = new Rule(-3, new int[]{25});
    rules[347] = new Rule(-3, new int[]{23});
    rules[348] = new Rule(-295, new int[]{-294});
    rules[349] = new Rule(-295, new int[]{59,-143,5,-260});
    rules[350] = new Rule(-49, new int[]{-6,-207});
    rules[351] = new Rule(-49, new int[]{-6,-204});
    rules[352] = new Rule(-204, new int[]{-200});
    rules[353] = new Rule(-204, new int[]{-203});
    rules[354] = new Rule(-207, new int[]{-3,-215});
    rules[355] = new Rule(-207, new int[]{-215});
    rules[356] = new Rule(-207, new int[]{-212});
    rules[357] = new Rule(-215, new int[]{-213});
    rules[358] = new Rule(-213, new int[]{-210});
    rules[359] = new Rule(-213, new int[]{-214});
    rules[360] = new Rule(-212, new int[]{27,-157,-113,-193});
    rules[361] = new Rule(-212, new int[]{-3,27,-157,-113,-193});
    rules[362] = new Rule(-212, new int[]{28,-157,-113,-193});
    rules[363] = new Rule(-157, new int[]{-156});
    rules[364] = new Rule(-157, new int[]{});
    rules[365] = new Rule(-158, new int[]{-132});
    rules[366] = new Rule(-158, new int[]{-135});
    rules[367] = new Rule(-158, new int[]{-158,7,-132});
    rules[368] = new Rule(-158, new int[]{-158,7,-135});
    rules[369] = new Rule(-51, new int[]{-6,-242});
    rules[370] = new Rule(-242, new int[]{43,-158,-218,-188,10,-191});
    rules[371] = new Rule(-242, new int[]{43,-158,-218,-188,10,-196,10,-191});
    rules[372] = new Rule(-242, new int[]{-3,43,-158,-218,-188,10,-191});
    rules[373] = new Rule(-242, new int[]{-3,43,-158,-218,-188,10,-196,10,-191});
    rules[374] = new Rule(-242, new int[]{24,43,-158,-218,10});
    rules[375] = new Rule(-242, new int[]{-3,24,43,-158,-218,10});
    rules[376] = new Rule(-191, new int[]{});
    rules[377] = new Rule(-191, new int[]{60,10});
    rules[378] = new Rule(-218, new int[]{-223,5,-259});
    rules[379] = new Rule(-223, new int[]{});
    rules[380] = new Rule(-223, new int[]{11,-222,12});
    rules[381] = new Rule(-222, new int[]{-221});
    rules[382] = new Rule(-222, new int[]{-222,10,-221});
    rules[383] = new Rule(-221, new int[]{-143,5,-259});
    rules[384] = new Rule(-101, new int[]{-82});
    rules[385] = new Rule(-101, new int[]{});
    rules[386] = new Rule(-188, new int[]{});
    rules[387] = new Rule(-188, new int[]{80,-101,-189});
    rules[388] = new Rule(-188, new int[]{81,-244,-190});
    rules[389] = new Rule(-189, new int[]{});
    rules[390] = new Rule(-189, new int[]{81,-244});
    rules[391] = new Rule(-190, new int[]{});
    rules[392] = new Rule(-190, new int[]{80,-101});
    rules[393] = new Rule(-293, new int[]{-294,10});
    rules[394] = new Rule(-321, new int[]{104});
    rules[395] = new Rule(-321, new int[]{114});
    rules[396] = new Rule(-294, new int[]{-143,5,-260});
    rules[397] = new Rule(-294, new int[]{-143,104,-81});
    rules[398] = new Rule(-294, new int[]{-143,5,-260,-321,-80});
    rules[399] = new Rule(-80, new int[]{-79});
    rules[400] = new Rule(-80, new int[]{-306});
    rules[401] = new Rule(-80, new int[]{-132,121,-311});
    rules[402] = new Rule(-80, new int[]{8,9,-307,121,-311});
    rules[403] = new Rule(-80, new int[]{8,-62,9,121,-311});
    rules[404] = new Rule(-79, new int[]{-78});
    rules[405] = new Rule(-79, new int[]{-53});
    rules[406] = new Rule(-202, new int[]{-212,-163});
    rules[407] = new Rule(-202, new int[]{27,-157,-113,104,-244,10});
    rules[408] = new Rule(-202, new int[]{-3,27,-157,-113,104,-244,10});
    rules[409] = new Rule(-203, new int[]{-212,-162});
    rules[410] = new Rule(-203, new int[]{27,-157,-113,104,-244,10});
    rules[411] = new Rule(-203, new int[]{-3,27,-157,-113,104,-244,10});
    rules[412] = new Rule(-199, new int[]{-206});
    rules[413] = new Rule(-199, new int[]{-3,-206});
    rules[414] = new Rule(-206, new int[]{-213,-164});
    rules[415] = new Rule(-206, new int[]{34,-155,-113,5,-259,-194,104,-91,10});
    rules[416] = new Rule(-206, new int[]{34,-155,-113,-194,104,-91,10});
    rules[417] = new Rule(-206, new int[]{34,-155,-113,5,-259,-194,104,-305,10});
    rules[418] = new Rule(-206, new int[]{34,-155,-113,-194,104,-305,10});
    rules[419] = new Rule(-206, new int[]{41,-156,-113,-194,104,-244,10});
    rules[420] = new Rule(-206, new int[]{-213,142,10});
    rules[421] = new Rule(-200, new int[]{-201});
    rules[422] = new Rule(-200, new int[]{-3,-201});
    rules[423] = new Rule(-201, new int[]{-213,-162});
    rules[424] = new Rule(-201, new int[]{34,-155,-113,5,-259,-194,104,-92,10});
    rules[425] = new Rule(-201, new int[]{34,-155,-113,-194,104,-92,10});
    rules[426] = new Rule(-201, new int[]{41,-156,-113,-194,104,-244,10});
    rules[427] = new Rule(-164, new int[]{-163});
    rules[428] = new Rule(-164, new int[]{-57});
    rules[429] = new Rule(-156, new int[]{-155});
    rules[430] = new Rule(-155, new int[]{-127});
    rules[431] = new Rule(-155, new int[]{-317,7,-127});
    rules[432] = new Rule(-134, new int[]{-122});
    rules[433] = new Rule(-317, new int[]{-134});
    rules[434] = new Rule(-317, new int[]{-317,7,-134});
    rules[435] = new Rule(-127, new int[]{-122});
    rules[436] = new Rule(-127, new int[]{-177});
    rules[437] = new Rule(-127, new int[]{-177,-140});
    rules[438] = new Rule(-122, new int[]{-119});
    rules[439] = new Rule(-122, new int[]{-119,-140});
    rules[440] = new Rule(-119, new int[]{-132});
    rules[441] = new Rule(-210, new int[]{41,-156,-113,-193,-300});
    rules[442] = new Rule(-214, new int[]{34,-155,-113,-193,-300});
    rules[443] = new Rule(-214, new int[]{34,-155,-113,5,-259,-193,-300});
    rules[444] = new Rule(-57, new int[]{101,-96,75,-96,10});
    rules[445] = new Rule(-57, new int[]{101,-96,10});
    rules[446] = new Rule(-57, new int[]{101,10});
    rules[447] = new Rule(-96, new int[]{-132});
    rules[448] = new Rule(-96, new int[]{-150});
    rules[449] = new Rule(-163, new int[]{-38,-239,10});
    rules[450] = new Rule(-162, new int[]{-40,-239,10});
    rules[451] = new Rule(-162, new int[]{-57});
    rules[452] = new Rule(-113, new int[]{});
    rules[453] = new Rule(-113, new int[]{8,9});
    rules[454] = new Rule(-113, new int[]{8,-114,9});
    rules[455] = new Rule(-114, new int[]{-52});
    rules[456] = new Rule(-114, new int[]{-114,10,-52});
    rules[457] = new Rule(-52, new int[]{-6,-280});
    rules[458] = new Rule(-280, new int[]{-144,5,-259});
    rules[459] = new Rule(-280, new int[]{50,-144,5,-259});
    rules[460] = new Rule(-280, new int[]{26,-144,5,-259});
    rules[461] = new Rule(-280, new int[]{102,-144,5,-259});
    rules[462] = new Rule(-280, new int[]{-144,5,-259,104,-81});
    rules[463] = new Rule(-280, new int[]{50,-144,5,-259,104,-81});
    rules[464] = new Rule(-280, new int[]{26,-144,5,-259,104,-81});
    rules[465] = new Rule(-144, new int[]{-120});
    rules[466] = new Rule(-144, new int[]{-144,94,-120});
    rules[467] = new Rule(-120, new int[]{-132});
    rules[468] = new Rule(-259, new int[]{-260});
    rules[469] = new Rule(-261, new int[]{-256});
    rules[470] = new Rule(-261, new int[]{-240});
    rules[471] = new Rule(-261, new int[]{-233});
    rules[472] = new Rule(-261, new int[]{-265});
    rules[473] = new Rule(-261, new int[]{-285});
    rules[474] = new Rule(-245, new int[]{-244});
    rules[475] = new Rule(-245, new int[]{-128,5,-245});
    rules[476] = new Rule(-244, new int[]{});
    rules[477] = new Rule(-244, new int[]{-4});
    rules[478] = new Rule(-244, new int[]{-197});
    rules[479] = new Rule(-244, new int[]{-118});
    rules[480] = new Rule(-244, new int[]{-239});
    rules[481] = new Rule(-244, new int[]{-138});
    rules[482] = new Rule(-244, new int[]{-32});
    rules[483] = new Rule(-244, new int[]{-231});
    rules[484] = new Rule(-244, new int[]{-301});
    rules[485] = new Rule(-244, new int[]{-109});
    rules[486] = new Rule(-244, new int[]{-302});
    rules[487] = new Rule(-244, new int[]{-145});
    rules[488] = new Rule(-244, new int[]{-286});
    rules[489] = new Rule(-244, new int[]{-232});
    rules[490] = new Rule(-244, new int[]{-108});
    rules[491] = new Rule(-244, new int[]{-297});
    rules[492] = new Rule(-244, new int[]{-55});
    rules[493] = new Rule(-244, new int[]{-154});
    rules[494] = new Rule(-244, new int[]{-111});
    rules[495] = new Rule(-244, new int[]{-112});
    rules[496] = new Rule(-244, new int[]{-110});
    rules[497] = new Rule(-244, new int[]{-325});
    rules[498] = new Rule(-110, new int[]{70,-91,93,-244});
    rules[499] = new Rule(-111, new int[]{72,-91});
    rules[500] = new Rule(-112, new int[]{72,71,-91});
    rules[501] = new Rule(-297, new int[]{50,-294});
    rules[502] = new Rule(-297, new int[]{8,50,-132,94,-320,9,104,-81});
    rules[503] = new Rule(-297, new int[]{50,8,-132,94,-143,9,104,-81});
    rules[504] = new Rule(-4, new int[]{-100,-180,-82});
    rules[505] = new Rule(-4, new int[]{8,-99,94,-319,9,-180,-81});
    rules[506] = new Rule(-319, new int[]{-99});
    rules[507] = new Rule(-319, new int[]{-319,94,-99});
    rules[508] = new Rule(-320, new int[]{50,-132});
    rules[509] = new Rule(-320, new int[]{-320,94,50,-132});
    rules[510] = new Rule(-197, new int[]{-100});
    rules[511] = new Rule(-118, new int[]{54,-128});
    rules[512] = new Rule(-239, new int[]{85,-236,86});
    rules[513] = new Rule(-236, new int[]{-245});
    rules[514] = new Rule(-236, new int[]{-236,10,-245});
    rules[515] = new Rule(-138, new int[]{37,-91,48,-244});
    rules[516] = new Rule(-138, new int[]{37,-91,48,-244,29,-244});
    rules[517] = new Rule(-325, new int[]{35,-91,52,-327,-237,86});
    rules[518] = new Rule(-325, new int[]{35,-91,52,-327,10,-237,86});
    rules[519] = new Rule(-327, new int[]{-326});
    rules[520] = new Rule(-327, new int[]{-327,10,-326});
    rules[521] = new Rule(-326, new int[]{-323,36,-91,5,-244});
    rules[522] = new Rule(-326, new int[]{-323,5,-244});
    rules[523] = new Rule(-326, new int[]{-324,36,-91,5,-244});
    rules[524] = new Rule(-326, new int[]{-324,5,-244});
    rules[525] = new Rule(-32, new int[]{22,-91,55,-33,-237,86});
    rules[526] = new Rule(-32, new int[]{22,-91,55,-33,10,-237,86});
    rules[527] = new Rule(-32, new int[]{22,-91,55,-237,86});
    rules[528] = new Rule(-33, new int[]{-246});
    rules[529] = new Rule(-33, new int[]{-33,10,-246});
    rules[530] = new Rule(-246, new int[]{-68,5,-244});
    rules[531] = new Rule(-68, new int[]{-98});
    rules[532] = new Rule(-68, new int[]{-68,94,-98});
    rules[533] = new Rule(-98, new int[]{-86});
    rules[534] = new Rule(-237, new int[]{});
    rules[535] = new Rule(-237, new int[]{29,-236});
    rules[536] = new Rule(-231, new int[]{91,-236,92,-81});
    rules[537] = new Rule(-301, new int[]{51,-91,-276,-244});
    rules[538] = new Rule(-276, new int[]{93});
    rules[539] = new Rule(-276, new int[]{});
    rules[540] = new Rule(-154, new int[]{57,-91,93,-244});
    rules[541] = new Rule(-108, new int[]{33,-132,-258,131,-91,93,-244});
    rules[542] = new Rule(-108, new int[]{33,50,-132,5,-260,131,-91,93,-244});
    rules[543] = new Rule(-108, new int[]{33,50,-132,131,-91,93,-244});
    rules[544] = new Rule(-258, new int[]{5,-260});
    rules[545] = new Rule(-258, new int[]{});
    rules[546] = new Rule(-109, new int[]{32,-18,-132,-270,-91,-104,-91,-276,-244});
    rules[547] = new Rule(-18, new int[]{50});
    rules[548] = new Rule(-18, new int[]{});
    rules[549] = new Rule(-270, new int[]{104});
    rules[550] = new Rule(-270, new int[]{5,-166,104});
    rules[551] = new Rule(-104, new int[]{68});
    rules[552] = new Rule(-104, new int[]{69});
    rules[553] = new Rule(-302, new int[]{52,-66,93,-244});
    rules[554] = new Rule(-145, new int[]{39});
    rules[555] = new Rule(-286, new int[]{96,-236,-274});
    rules[556] = new Rule(-274, new int[]{95,-236,86});
    rules[557] = new Rule(-274, new int[]{30,-56,86});
    rules[558] = new Rule(-56, new int[]{-59,-238});
    rules[559] = new Rule(-56, new int[]{-59,10,-238});
    rules[560] = new Rule(-56, new int[]{-236});
    rules[561] = new Rule(-59, new int[]{-58});
    rules[562] = new Rule(-59, new int[]{-59,10,-58});
    rules[563] = new Rule(-238, new int[]{});
    rules[564] = new Rule(-238, new int[]{29,-236});
    rules[565] = new Rule(-58, new int[]{74,-60,93,-244});
    rules[566] = new Rule(-60, new int[]{-165});
    rules[567] = new Rule(-60, new int[]{-125,5,-165});
    rules[568] = new Rule(-165, new int[]{-166});
    rules[569] = new Rule(-125, new int[]{-132});
    rules[570] = new Rule(-232, new int[]{44});
    rules[571] = new Rule(-232, new int[]{44,-81});
    rules[572] = new Rule(-66, new int[]{-82});
    rules[573] = new Rule(-66, new int[]{-66,94,-82});
    rules[574] = new Rule(-55, new int[]{-160});
    rules[575] = new Rule(-160, new int[]{-159});
    rules[576] = new Rule(-82, new int[]{-81});
    rules[577] = new Rule(-82, new int[]{-305});
    rules[578] = new Rule(-81, new int[]{-91});
    rules[579] = new Rule(-81, new int[]{-105});
    rules[580] = new Rule(-91, new int[]{-90});
    rules[581] = new Rule(-91, new int[]{-225});
    rules[582] = new Rule(-92, new int[]{-91});
    rules[583] = new Rule(-92, new int[]{-305});
    rules[584] = new Rule(-90, new int[]{-89});
    rules[585] = new Rule(-90, new int[]{-90,16,-89});
    rules[586] = new Rule(-241, new int[]{18,8,-268,9});
    rules[587] = new Rule(-279, new int[]{19,8,-268,9});
    rules[588] = new Rule(-279, new int[]{19,8,-267,9});
    rules[589] = new Rule(-225, new int[]{-91,13,-91,5,-91});
    rules[590] = new Rule(-267, new int[]{-166,-284});
    rules[591] = new Rule(-267, new int[]{-166,4,-284});
    rules[592] = new Rule(-268, new int[]{-166});
    rules[593] = new Rule(-268, new int[]{-166,-283});
    rules[594] = new Rule(-268, new int[]{-166,4,-283});
    rules[595] = new Rule(-5, new int[]{8,-62,9});
    rules[596] = new Rule(-5, new int[]{});
    rules[597] = new Rule(-159, new int[]{73,-268,-65});
    rules[598] = new Rule(-159, new int[]{73,-268,11,-63,12,-5});
    rules[599] = new Rule(-159, new int[]{73,23,8,-316,9});
    rules[600] = new Rule(-315, new int[]{-132,104,-89});
    rules[601] = new Rule(-315, new int[]{-89});
    rules[602] = new Rule(-316, new int[]{-315});
    rules[603] = new Rule(-316, new int[]{-316,94,-315});
    rules[604] = new Rule(-65, new int[]{});
    rules[605] = new Rule(-65, new int[]{8,-63,9});
    rules[606] = new Rule(-89, new int[]{-93});
    rules[607] = new Rule(-89, new int[]{-89,-182,-93});
    rules[608] = new Rule(-89, new int[]{-250,8,-332,9});
    rules[609] = new Rule(-322, new int[]{-268,8,-332,9});
    rules[610] = new Rule(-323, new int[]{-268,8,-333,9});
    rules[611] = new Rule(-324, new int[]{-331});
    rules[612] = new Rule(-331, new int[]{-334});
    rules[613] = new Rule(-331, new int[]{-331,94,-334});
    rules[614] = new Rule(-334, new int[]{-14});
    rules[615] = new Rule(-334, new int[]{-335});
    rules[616] = new Rule(-335, new int[]{8,-336,94,-330,-307,-314,9});
    rules[617] = new Rule(-336, new int[]{14});
    rules[618] = new Rule(-336, new int[]{-14});
    rules[619] = new Rule(-330, new int[]{-336});
    rules[620] = new Rule(-330, new int[]{-330,94,-336});
    rules[621] = new Rule(-333, new int[]{-329});
    rules[622] = new Rule(-333, new int[]{-333,10,-329});
    rules[623] = new Rule(-333, new int[]{-333,94,-329});
    rules[624] = new Rule(-332, new int[]{-328});
    rules[625] = new Rule(-332, new int[]{-332,10,-328});
    rules[626] = new Rule(-332, new int[]{-332,94,-328});
    rules[627] = new Rule(-328, new int[]{14});
    rules[628] = new Rule(-328, new int[]{-14});
    rules[629] = new Rule(-328, new int[]{50,-132,5,-260});
    rules[630] = new Rule(-328, new int[]{50,-132});
    rules[631] = new Rule(-328, new int[]{-322});
    rules[632] = new Rule(-329, new int[]{14});
    rules[633] = new Rule(-329, new int[]{-14});
    rules[634] = new Rule(-329, new int[]{-132,5,-260});
    rules[635] = new Rule(-329, new int[]{-132});
    rules[636] = new Rule(-329, new int[]{50,-132,5,-260});
    rules[637] = new Rule(-329, new int[]{50,-132});
    rules[638] = new Rule(-329, new int[]{-323});
    rules[639] = new Rule(-102, new int[]{-93});
    rules[640] = new Rule(-102, new int[]{});
    rules[641] = new Rule(-107, new int[]{-83});
    rules[642] = new Rule(-107, new int[]{});
    rules[643] = new Rule(-105, new int[]{-93,5,-102});
    rules[644] = new Rule(-105, new int[]{5,-102});
    rules[645] = new Rule(-105, new int[]{-93,5,-102,5,-93});
    rules[646] = new Rule(-105, new int[]{5,-102,5,-93});
    rules[647] = new Rule(-106, new int[]{-83,5,-107});
    rules[648] = new Rule(-106, new int[]{5,-107});
    rules[649] = new Rule(-106, new int[]{-83,5,-107,5,-83});
    rules[650] = new Rule(-106, new int[]{5,-107,5,-83});
    rules[651] = new Rule(-182, new int[]{114});
    rules[652] = new Rule(-182, new int[]{119});
    rules[653] = new Rule(-182, new int[]{117});
    rules[654] = new Rule(-182, new int[]{115});
    rules[655] = new Rule(-182, new int[]{118});
    rules[656] = new Rule(-182, new int[]{116});
    rules[657] = new Rule(-182, new int[]{131});
    rules[658] = new Rule(-93, new int[]{-76});
    rules[659] = new Rule(-93, new int[]{-93,-183,-76});
    rules[660] = new Rule(-183, new int[]{110});
    rules[661] = new Rule(-183, new int[]{109});
    rules[662] = new Rule(-183, new int[]{122});
    rules[663] = new Rule(-183, new int[]{123});
    rules[664] = new Rule(-183, new int[]{120});
    rules[665] = new Rule(-187, new int[]{130});
    rules[666] = new Rule(-187, new int[]{132});
    rules[667] = new Rule(-248, new int[]{-250});
    rules[668] = new Rule(-248, new int[]{-251});
    rules[669] = new Rule(-251, new int[]{-76,130,-268});
    rules[670] = new Rule(-250, new int[]{-76,132,-268});
    rules[671] = new Rule(-250, new int[]{-76,132,-14});
    rules[672] = new Rule(-77, new int[]{-88});
    rules[673] = new Rule(-252, new int[]{-77,113,-88});
    rules[674] = new Rule(-76, new int[]{-88});
    rules[675] = new Rule(-76, new int[]{-159});
    rules[676] = new Rule(-76, new int[]{-252});
    rules[677] = new Rule(-76, new int[]{-76,-184,-88});
    rules[678] = new Rule(-76, new int[]{-76,-184,-252});
    rules[679] = new Rule(-76, new int[]{-248});
    rules[680] = new Rule(-184, new int[]{112});
    rules[681] = new Rule(-184, new int[]{111});
    rules[682] = new Rule(-184, new int[]{125});
    rules[683] = new Rule(-184, new int[]{126});
    rules[684] = new Rule(-184, new int[]{127});
    rules[685] = new Rule(-184, new int[]{128});
    rules[686] = new Rule(-184, new int[]{124});
    rules[687] = new Rule(-53, new int[]{60,8,-268,9});
    rules[688] = new Rule(-54, new int[]{8,-91,94,-73,-307,-314,9});
    rules[689] = new Rule(-88, new int[]{53});
    rules[690] = new Rule(-88, new int[]{-14});
    rules[691] = new Rule(-88, new int[]{-53});
    rules[692] = new Rule(-88, new int[]{11,-64,12});
    rules[693] = new Rule(-88, new int[]{129,-88});
    rules[694] = new Rule(-88, new int[]{-185,-88});
    rules[695] = new Rule(-88, new int[]{-100});
    rules[696] = new Rule(-88, new int[]{-54});
    rules[697] = new Rule(-14, new int[]{-150});
    rules[698] = new Rule(-14, new int[]{-15});
    rules[699] = new Rule(-103, new int[]{-99,15,-99});
    rules[700] = new Rule(-103, new int[]{-99,15,-103});
    rules[701] = new Rule(-100, new int[]{-117,-99});
    rules[702] = new Rule(-100, new int[]{-99});
    rules[703] = new Rule(-100, new int[]{-103});
    rules[704] = new Rule(-117, new int[]{135});
    rules[705] = new Rule(-117, new int[]{-117,135});
    rules[706] = new Rule(-9, new int[]{-166,-65});
    rules[707] = new Rule(-9, new int[]{-285,-65});
    rules[708] = new Rule(-304, new int[]{-132});
    rules[709] = new Rule(-304, new int[]{-304,7,-123});
    rules[710] = new Rule(-303, new int[]{-304});
    rules[711] = new Rule(-303, new int[]{-304,-283});
    rules[712] = new Rule(-16, new int[]{-99});
    rules[713] = new Rule(-16, new int[]{-14});
    rules[714] = new Rule(-99, new int[]{-132});
    rules[715] = new Rule(-99, new int[]{-177});
    rules[716] = new Rule(-99, new int[]{39,-132});
    rules[717] = new Rule(-99, new int[]{8,-81,9});
    rules[718] = new Rule(-99, new int[]{-241});
    rules[719] = new Rule(-99, new int[]{-279});
    rules[720] = new Rule(-99, new int[]{-14,7,-123});
    rules[721] = new Rule(-99, new int[]{-16,11,-66,12});
    rules[722] = new Rule(-99, new int[]{-99,17,-105,12});
    rules[723] = new Rule(-99, new int[]{-99,8,-63,9});
    rules[724] = new Rule(-99, new int[]{-99,7,-133});
    rules[725] = new Rule(-99, new int[]{-54,7,-133});
    rules[726] = new Rule(-99, new int[]{-99,136});
    rules[727] = new Rule(-99, new int[]{-99,4,-283});
    rules[728] = new Rule(-63, new int[]{-66});
    rules[729] = new Rule(-63, new int[]{});
    rules[730] = new Rule(-64, new int[]{-71});
    rules[731] = new Rule(-64, new int[]{});
    rules[732] = new Rule(-71, new int[]{-84});
    rules[733] = new Rule(-71, new int[]{-71,94,-84});
    rules[734] = new Rule(-84, new int[]{-81});
    rules[735] = new Rule(-84, new int[]{-81,6,-81});
    rules[736] = new Rule(-151, new int[]{138});
    rules[737] = new Rule(-151, new int[]{140});
    rules[738] = new Rule(-150, new int[]{-152});
    rules[739] = new Rule(-150, new int[]{139});
    rules[740] = new Rule(-152, new int[]{-151});
    rules[741] = new Rule(-152, new int[]{-152,-151});
    rules[742] = new Rule(-177, new int[]{42,-186});
    rules[743] = new Rule(-193, new int[]{10});
    rules[744] = new Rule(-193, new int[]{10,-192,10});
    rules[745] = new Rule(-194, new int[]{});
    rules[746] = new Rule(-194, new int[]{10,-192});
    rules[747] = new Rule(-192, new int[]{-195});
    rules[748] = new Rule(-192, new int[]{-192,10,-195});
    rules[749] = new Rule(-132, new int[]{137});
    rules[750] = new Rule(-132, new int[]{-136});
    rules[751] = new Rule(-132, new int[]{-137});
    rules[752] = new Rule(-123, new int[]{-132});
    rules[753] = new Rule(-123, new int[]{-277});
    rules[754] = new Rule(-123, new int[]{-278});
    rules[755] = new Rule(-133, new int[]{-132});
    rules[756] = new Rule(-133, new int[]{-277});
    rules[757] = new Rule(-133, new int[]{-177});
    rules[758] = new Rule(-195, new int[]{141});
    rules[759] = new Rule(-195, new int[]{143});
    rules[760] = new Rule(-195, new int[]{144});
    rules[761] = new Rule(-195, new int[]{145});
    rules[762] = new Rule(-195, new int[]{147});
    rules[763] = new Rule(-195, new int[]{146});
    rules[764] = new Rule(-196, new int[]{146});
    rules[765] = new Rule(-196, new int[]{145});
    rules[766] = new Rule(-196, new int[]{141});
    rules[767] = new Rule(-136, new int[]{80});
    rules[768] = new Rule(-136, new int[]{81});
    rules[769] = new Rule(-137, new int[]{75});
    rules[770] = new Rule(-137, new int[]{73});
    rules[771] = new Rule(-135, new int[]{79});
    rules[772] = new Rule(-135, new int[]{78});
    rules[773] = new Rule(-135, new int[]{77});
    rules[774] = new Rule(-135, new int[]{76});
    rules[775] = new Rule(-277, new int[]{-135});
    rules[776] = new Rule(-277, new int[]{66});
    rules[777] = new Rule(-277, new int[]{61});
    rules[778] = new Rule(-277, new int[]{122});
    rules[779] = new Rule(-277, new int[]{19});
    rules[780] = new Rule(-277, new int[]{18});
    rules[781] = new Rule(-277, new int[]{60});
    rules[782] = new Rule(-277, new int[]{20});
    rules[783] = new Rule(-277, new int[]{123});
    rules[784] = new Rule(-277, new int[]{124});
    rules[785] = new Rule(-277, new int[]{125});
    rules[786] = new Rule(-277, new int[]{126});
    rules[787] = new Rule(-277, new int[]{127});
    rules[788] = new Rule(-277, new int[]{128});
    rules[789] = new Rule(-277, new int[]{129});
    rules[790] = new Rule(-277, new int[]{130});
    rules[791] = new Rule(-277, new int[]{131});
    rules[792] = new Rule(-277, new int[]{132});
    rules[793] = new Rule(-277, new int[]{21});
    rules[794] = new Rule(-277, new int[]{71});
    rules[795] = new Rule(-277, new int[]{85});
    rules[796] = new Rule(-277, new int[]{22});
    rules[797] = new Rule(-277, new int[]{23});
    rules[798] = new Rule(-277, new int[]{26});
    rules[799] = new Rule(-277, new int[]{27});
    rules[800] = new Rule(-277, new int[]{28});
    rules[801] = new Rule(-277, new int[]{69});
    rules[802] = new Rule(-277, new int[]{93});
    rules[803] = new Rule(-277, new int[]{29});
    rules[804] = new Rule(-277, new int[]{30});
    rules[805] = new Rule(-277, new int[]{31});
    rules[806] = new Rule(-277, new int[]{24});
    rules[807] = new Rule(-277, new int[]{98});
    rules[808] = new Rule(-277, new int[]{95});
    rules[809] = new Rule(-277, new int[]{32});
    rules[810] = new Rule(-277, new int[]{33});
    rules[811] = new Rule(-277, new int[]{34});
    rules[812] = new Rule(-277, new int[]{37});
    rules[813] = new Rule(-277, new int[]{38});
    rules[814] = new Rule(-277, new int[]{39});
    rules[815] = new Rule(-277, new int[]{97});
    rules[816] = new Rule(-277, new int[]{40});
    rules[817] = new Rule(-277, new int[]{41});
    rules[818] = new Rule(-277, new int[]{43});
    rules[819] = new Rule(-277, new int[]{44});
    rules[820] = new Rule(-277, new int[]{45});
    rules[821] = new Rule(-277, new int[]{91});
    rules[822] = new Rule(-277, new int[]{46});
    rules[823] = new Rule(-277, new int[]{96});
    rules[824] = new Rule(-277, new int[]{47});
    rules[825] = new Rule(-277, new int[]{25});
    rules[826] = new Rule(-277, new int[]{48});
    rules[827] = new Rule(-277, new int[]{68});
    rules[828] = new Rule(-277, new int[]{92});
    rules[829] = new Rule(-277, new int[]{49});
    rules[830] = new Rule(-277, new int[]{50});
    rules[831] = new Rule(-277, new int[]{51});
    rules[832] = new Rule(-277, new int[]{52});
    rules[833] = new Rule(-277, new int[]{53});
    rules[834] = new Rule(-277, new int[]{54});
    rules[835] = new Rule(-277, new int[]{55});
    rules[836] = new Rule(-277, new int[]{56});
    rules[837] = new Rule(-277, new int[]{58});
    rules[838] = new Rule(-277, new int[]{99});
    rules[839] = new Rule(-277, new int[]{100});
    rules[840] = new Rule(-277, new int[]{103});
    rules[841] = new Rule(-277, new int[]{101});
    rules[842] = new Rule(-277, new int[]{102});
    rules[843] = new Rule(-277, new int[]{59});
    rules[844] = new Rule(-277, new int[]{72});
    rules[845] = new Rule(-277, new int[]{35});
    rules[846] = new Rule(-277, new int[]{36});
    rules[847] = new Rule(-278, new int[]{42});
    rules[848] = new Rule(-278, new int[]{86});
    rules[849] = new Rule(-186, new int[]{109});
    rules[850] = new Rule(-186, new int[]{110});
    rules[851] = new Rule(-186, new int[]{111});
    rules[852] = new Rule(-186, new int[]{112});
    rules[853] = new Rule(-186, new int[]{114});
    rules[854] = new Rule(-186, new int[]{115});
    rules[855] = new Rule(-186, new int[]{116});
    rules[856] = new Rule(-186, new int[]{117});
    rules[857] = new Rule(-186, new int[]{118});
    rules[858] = new Rule(-186, new int[]{119});
    rules[859] = new Rule(-186, new int[]{122});
    rules[860] = new Rule(-186, new int[]{123});
    rules[861] = new Rule(-186, new int[]{124});
    rules[862] = new Rule(-186, new int[]{125});
    rules[863] = new Rule(-186, new int[]{126});
    rules[864] = new Rule(-186, new int[]{127});
    rules[865] = new Rule(-186, new int[]{128});
    rules[866] = new Rule(-186, new int[]{129});
    rules[867] = new Rule(-186, new int[]{131});
    rules[868] = new Rule(-186, new int[]{133});
    rules[869] = new Rule(-186, new int[]{134});
    rules[870] = new Rule(-186, new int[]{-180});
    rules[871] = new Rule(-186, new int[]{113});
    rules[872] = new Rule(-180, new int[]{104});
    rules[873] = new Rule(-180, new int[]{105});
    rules[874] = new Rule(-180, new int[]{106});
    rules[875] = new Rule(-180, new int[]{107});
    rules[876] = new Rule(-180, new int[]{108});
    rules[877] = new Rule(-305, new int[]{-132,121,-311});
    rules[878] = new Rule(-305, new int[]{8,9,-308,121,-311});
    rules[879] = new Rule(-305, new int[]{8,-132,5,-259,9,-308,121,-311});
    rules[880] = new Rule(-305, new int[]{8,-132,10,-309,9,-308,121,-311});
    rules[881] = new Rule(-305, new int[]{8,-132,5,-259,10,-309,9,-308,121,-311});
    rules[882] = new Rule(-305, new int[]{8,-91,94,-73,-307,-314,9,-318});
    rules[883] = new Rule(-305, new int[]{-306});
    rules[884] = new Rule(-314, new int[]{});
    rules[885] = new Rule(-314, new int[]{10,-309});
    rules[886] = new Rule(-318, new int[]{-308,121,-311});
    rules[887] = new Rule(-306, new int[]{34,-307,121,-311});
    rules[888] = new Rule(-306, new int[]{34,8,9,-307,121,-311});
    rules[889] = new Rule(-306, new int[]{34,8,-309,9,-307,121,-311});
    rules[890] = new Rule(-306, new int[]{41,121,-312});
    rules[891] = new Rule(-306, new int[]{41,8,9,121,-312});
    rules[892] = new Rule(-306, new int[]{41,8,-309,9,121,-312});
    rules[893] = new Rule(-309, new int[]{-310});
    rules[894] = new Rule(-309, new int[]{-309,10,-310});
    rules[895] = new Rule(-310, new int[]{-143,-307});
    rules[896] = new Rule(-307, new int[]{});
    rules[897] = new Rule(-307, new int[]{5,-259});
    rules[898] = new Rule(-308, new int[]{});
    rules[899] = new Rule(-308, new int[]{5,-261});
    rules[900] = new Rule(-313, new int[]{-239});
    rules[901] = new Rule(-313, new int[]{-138});
    rules[902] = new Rule(-313, new int[]{-301});
    rules[903] = new Rule(-313, new int[]{-231});
    rules[904] = new Rule(-313, new int[]{-109});
    rules[905] = new Rule(-313, new int[]{-108});
    rules[906] = new Rule(-313, new int[]{-110});
    rules[907] = new Rule(-313, new int[]{-32});
    rules[908] = new Rule(-313, new int[]{-286});
    rules[909] = new Rule(-313, new int[]{-154});
    rules[910] = new Rule(-313, new int[]{-111});
    rules[911] = new Rule(-313, new int[]{-232});
    rules[912] = new Rule(-311, new int[]{-91});
    rules[913] = new Rule(-311, new int[]{-313});
    rules[914] = new Rule(-312, new int[]{-197});
    rules[915] = new Rule(-312, new int[]{-4});
    rules[916] = new Rule(-312, new int[]{-313});
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
      case 5: // parts -> tkParseModeExpression, expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 6: // parts -> tkParseModeExpression, tkType, type_decl_identifier
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 7: // parts -> tkParseModeType, variable_as_type
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 8: // parts -> tkParseModeStatement, stmt_or_expression
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 9: // stmt_or_expression -> expr
{ CurrentSemanticValue.stn = new expression_as_statement(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);}
        break;
      case 10: // stmt_or_expression -> assignment
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 11: // stmt_or_expression -> var_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 12: // optional_head_compiler_directives -> /* empty */
{ CurrentSemanticValue.ob = null; }
        break;
      case 13: // optional_head_compiler_directives -> head_compiler_directives
{ CurrentSemanticValue.ob = null; }
        break;
      case 14: // head_compiler_directives -> one_compiler_directive
{ CurrentSemanticValue.ob = null; }
        break;
      case 15: // head_compiler_directives -> head_compiler_directives, one_compiler_directive
{ CurrentSemanticValue.ob = null; }
        break;
      case 16: // one_compiler_directive -> tkDirectiveName, tkIdentifier
{
			parsertools.AddErrorFromResource("UNSUPPORTED_OLD_DIRECTIVES",CurrentLocationSpan);
			CurrentSemanticValue.ob = null;
        }
        break;
      case 17: // one_compiler_directive -> tkDirectiveName, tkStringLiteral
{
			parsertools.AddErrorFromResource("UNSUPPORTED_OLD_DIRECTIVES",CurrentLocationSpan);
			CurrentSemanticValue.ob = null;
        }
        break;
      case 18: // program_file -> program_header, optional_head_compiler_directives, uses_clause, 
               //                 program_block, optional_tk_point
{ 
			CurrentSemanticValue.stn = NewProgramModule(ValueStack[ValueStack.Depth-5].stn as program_name, ValueStack[ValueStack.Depth-4].ob, ValueStack[ValueStack.Depth-3].stn as uses_list, ValueStack[ValueStack.Depth-2].stn, ValueStack[ValueStack.Depth-1].ob, CurrentLocationSpan);
        }
        break;
      case 19: // optional_tk_point -> tkPoint
{ CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 20: // optional_tk_point -> tkSemiColon
{ CurrentSemanticValue.ob = null; }
        break;
      case 21: // optional_tk_point -> tkColon
{ CurrentSemanticValue.ob = null; }
        break;
      case 22: // optional_tk_point -> tkComma
{ CurrentSemanticValue.ob = null; }
        break;
      case 23: // optional_tk_point -> tkDotDot
{ CurrentSemanticValue.ob = null; }
        break;
      case 25: // program_header -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 26: // program_header -> tkProgram, identifier, program_heading_2
{ CurrentSemanticValue.stn = new program_name(ValueStack[ValueStack.Depth-2].id,CurrentLocationSpan); }
        break;
      case 27: // program_heading_2 -> tkSemiColon
{ CurrentSemanticValue.ob = null; }
        break;
      case 28: // program_heading_2 -> tkRoundOpen, program_param_list, tkRoundClose, tkSemiColon
{ CurrentSemanticValue.ob = null; }
        break;
      case 29: // program_param_list -> program_param
{ CurrentSemanticValue.ob = null; }
        break;
      case 30: // program_param_list -> program_param_list, tkComma, program_param
{ CurrentSemanticValue.ob = null; }
        break;
      case 31: // program_param -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 32: // program_block -> program_decl_sect_list, compound_stmt
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-2].stn as declarations, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
        }
        break;
      case 33: // program_decl_sect_list -> decl_sect_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 34: // ident_or_keyword_pointseparator_list -> identifier_or_keyword
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 35: // ident_or_keyword_pointseparator_list -> ident_or_keyword_pointseparator_list, 
               //                                         tkPoint, identifier_or_keyword
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 36: // uses_clause -> /* empty */
{ 
			CurrentSemanticValue.stn = null; 
		}
        break;
      case 37: // uses_clause -> uses_clause, tkUses, used_units_list, tkSemiColon
{ 
   			if (parsertools.build_tree_for_formatter)
   			{
	        	if (ValueStack[ValueStack.Depth-4].stn == null)
                {
	        		CurrentSemanticValue.stn = new uses_closure(ValueStack[ValueStack.Depth-2].stn as uses_list,CurrentLocationSpan);
                }
	        	else {
                    (ValueStack[ValueStack.Depth-4].stn as uses_closure).AddUsesList(ValueStack[ValueStack.Depth-2].stn as uses_list,CurrentLocationSpan);
                    CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-4].stn;
                }
   			}
   			else 
   			{
	        	if (ValueStack[ValueStack.Depth-4].stn == null)
                {
                    CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
                    CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
                }
	        	else 
                {
                    (ValueStack[ValueStack.Depth-4].stn as uses_list).AddUsesList(ValueStack[ValueStack.Depth-2].stn as uses_list,CurrentLocationSpan);
                    CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-4].stn;
                    CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
                }
			}
		}
        break;
      case 38: // used_units_list -> used_unit_name
{ 
		  CurrentSemanticValue.stn = new uses_list(ValueStack[ValueStack.Depth-1].stn as unit_or_namespace,CurrentLocationSpan);
        }
        break;
      case 39: // used_units_list -> used_units_list, tkComma, used_unit_name
{ 
		  CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as uses_list).Add(ValueStack[ValueStack.Depth-1].stn as unit_or_namespace, CurrentLocationSpan);
        }
        break;
      case 40: // used_unit_name -> ident_or_keyword_pointseparator_list
{ 
			CurrentSemanticValue.stn = new unit_or_namespace(ValueStack[ValueStack.Depth-1].stn as ident_list,CurrentLocationSpan); 
		}
        break;
      case 41: // used_unit_name -> ident_or_keyword_pointseparator_list, tkIn, tkStringLiteral
{ 
			CurrentSemanticValue.stn = new uses_unit_in(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].stn as string_const, CurrentLocationSpan);
        }
        break;
      case 42: // unit_file -> attribute_declarations, unit_header, interface_part, 
               //              implementation_part, initialization_part, tkPoint
{ 
			CurrentSemanticValue.stn = new unit_module(ValueStack[ValueStack.Depth-5].stn as unit_name, ValueStack[ValueStack.Depth-4].stn as interface_node, ValueStack[ValueStack.Depth-3].stn as implementation_node, (ValueStack[ValueStack.Depth-2].stn as initfinal_part).initialization_sect, (ValueStack[ValueStack.Depth-2].stn as initfinal_part).finalization_sect, ValueStack[ValueStack.Depth-6].stn as attribute_list, CurrentLocationSpan);                    
		}
        break;
      case 43: // unit_file -> attribute_declarations, unit_header, abc_interface_part, 
               //              initialization_part, tkPoint
{ 
			CurrentSemanticValue.stn = new unit_module(ValueStack[ValueStack.Depth-4].stn as unit_name, ValueStack[ValueStack.Depth-3].stn as interface_node, null, (ValueStack[ValueStack.Depth-2].stn as initfinal_part).initialization_sect, (ValueStack[ValueStack.Depth-2].stn as initfinal_part).finalization_sect, ValueStack[ValueStack.Depth-5].stn as attribute_list, CurrentLocationSpan);
        }
        break;
      case 44: // unit_header -> unit_key_word, unit_name, tkSemiColon, 
               //                optional_head_compiler_directives
{ 
			CurrentSemanticValue.stn = NewUnitHeading(new ident(ValueStack[ValueStack.Depth-4].ti.text, LocationStack[LocationStack.Depth-4]), ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan); 
		}
        break;
      case 45: // unit_header -> tkNamespace, ident_or_keyword_pointseparator_list, tkSemiColon, 
               //                optional_head_compiler_directives
{
            CurrentSemanticValue.stn = NewNamespaceHeading(new ident(ValueStack[ValueStack.Depth-4].ti.text, LocationStack[LocationStack.Depth-4]), ValueStack[ValueStack.Depth-3].stn as ident_list, CurrentLocationSpan);
        }
        break;
      case 46: // unit_key_word -> tkUnit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 47: // unit_key_word -> tkLibrary
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 48: // unit_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 49: // interface_part -> tkInterface, uses_clause, interface_decl_sect_list
{ 
			CurrentSemanticValue.stn = new interface_node(ValueStack[ValueStack.Depth-1].stn as declarations, ValueStack[ValueStack.Depth-2].stn as uses_list, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1])); 
        }
        break;
      case 50: // implementation_part -> tkImplementation, uses_clause, decl_sect_list
{ 
			CurrentSemanticValue.stn = new implementation_node(ValueStack[ValueStack.Depth-2].stn as uses_list, ValueStack[ValueStack.Depth-1].stn as declarations, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1])); 
        }
        break;
      case 51: // abc_interface_part -> uses_clause, decl_sect_list
{ 
			CurrentSemanticValue.stn = new interface_node(ValueStack[ValueStack.Depth-1].stn as declarations, ValueStack[ValueStack.Depth-2].stn as uses_list, null, null); 
        }
        break;
      case 52: // initialization_part -> tkEnd
{ 
			CurrentSemanticValue.stn = new initfinal_part(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 53: // initialization_part -> tkInitialization, stmt_list, tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, null, null, CurrentLocationSpan);
        }
        break;
      case 54: // initialization_part -> tkInitialization, stmt_list, tkFinalization, stmt_list, 
               //                        tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-5].ti, ValueStack[ValueStack.Depth-4].stn as statement_list, ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, CurrentLocationSpan);
        }
        break;
      case 55: // initialization_part -> tkBegin, stmt_list, tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, null, null, CurrentLocationSpan);
        }
        break;
      case 56: // interface_decl_sect_list -> int_decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 57: // int_decl_sect_list1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations();  
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 58: // int_decl_sect_list1 -> int_decl_sect_list1, int_decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 59: // decl_sect_list -> decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 60: // decl_sect_list1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations(); 
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 61: // decl_sect_list1 -> decl_sect_list1, decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 62: // inclass_decl_sect_list -> inclass_decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 63: // inclass_decl_sect_list1 -> /* empty */
{ 
        	CurrentSemanticValue.stn = new declarations(); 
        }
        break;
      case 64: // inclass_decl_sect_list1 -> inclass_decl_sect_list1, abc_decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 65: // int_decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 66: // int_decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 67: // int_decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 68: // int_decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 69: // int_decl_sect -> int_proc_header
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 70: // int_decl_sect -> int_func_header
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 71: // decl_sect -> label_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 72: // decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 73: // decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 74: // decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 75: // decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 76: // decl_sect -> proc_func_constr_destr_decl_with_attr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 77: // proc_func_constr_destr_decl -> proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 78: // proc_func_constr_destr_decl -> constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 79: // proc_func_constr_destr_decl_with_attr -> attribute_declarations, 
               //                                          proc_func_constr_destr_decl
{
		    (ValueStack[ValueStack.Depth-1].stn as procedure_definition).AssignAttrList(ValueStack[ValueStack.Depth-2].stn as attribute_list);
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 80: // abc_decl_sect -> label_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 81: // abc_decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 82: // abc_decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 83: // abc_decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 84: // abc_decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 85: // int_proc_header -> attribute_declarations, proc_header
{  
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
			(CurrentSemanticValue.td as procedure_header).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
        }
        break;
      case 86: // int_proc_header -> attribute_declarations, proc_header, tkForward, tkSemiColon
{  
			CurrentSemanticValue.td = NewProcedureHeader(ValueStack[ValueStack.Depth-4].stn as attribute_list, ValueStack[ValueStack.Depth-3].td as procedure_header, ValueStack[ValueStack.Depth-2].id as procedure_attribute, CurrentLocationSpan);
		}
        break;
      case 87: // int_func_header -> attribute_declarations, func_header
{  
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
			(CurrentSemanticValue.td as procedure_header).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
        }
        break;
      case 88: // int_func_header -> attribute_declarations, func_header, tkForward, tkSemiColon
{  
			CurrentSemanticValue.td = NewProcedureHeader(ValueStack[ValueStack.Depth-4].stn as attribute_list, ValueStack[ValueStack.Depth-3].td as procedure_header, ValueStack[ValueStack.Depth-2].id as procedure_attribute, CurrentLocationSpan);
		}
        break;
      case 89: // label_decl_sect -> tkLabel, label_list, tkSemiColon
{ 
			CurrentSemanticValue.stn = new label_definitions(ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
		}
        break;
      case 90: // label_list -> label_name
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 91: // label_list -> label_list, tkComma, label_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 92: // label_name -> tkInteger
{ 
			CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ex.ToString(), CurrentLocationSpan);
		}
        break;
      case 93: // label_name -> tkFloat
{ 
			CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ex.ToString(), CurrentLocationSpan);  
		}
        break;
      case 94: // label_name -> identifier
{ 
			CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; 
		}
        break;
      case 95: // const_decl_sect -> tkConst, const_decl
{ 
			CurrentSemanticValue.stn = new consts_definitions_list(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
		}
        break;
      case 96: // const_decl_sect -> const_decl_sect, const_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as consts_definitions_list).Add(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
		}
        break;
      case 97: // res_str_decl_sect -> tkResourceString, const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 98: // res_str_decl_sect -> res_str_decl_sect, const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 99: // type_decl_sect -> tkType, type_decl
{ 
            CurrentSemanticValue.stn = new type_declarations(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
		}
        break;
      case 100: // type_decl_sect -> type_decl_sect, type_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as type_declarations).Add(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
		}
        break;
      case 101: // var_decl_with_assign_var_tuple -> var_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 102: // var_decl_with_assign_var_tuple -> tkRoundOpen, identifier, tkComma, ident_list, 
                //                                   tkRoundClose, tkAssign, expr_l1, 
                //                                   tkSemiColon
{
			(ValueStack[ValueStack.Depth-5].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-7].id);
			ValueStack[ValueStack.Depth-5].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]);
			CurrentSemanticValue.stn = new var_tuple_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
		}
        break;
      case 103: // var_decl_sect -> tkVar, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 104: // var_decl_sect -> tkEvent, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);                        
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).is_event = true;
        }
        break;
      case 105: // var_decl_sect -> var_decl_sect, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as variable_definitions).Add(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 106: // const_decl -> only_const_decl, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 107: // only_const_decl -> const_name, tkEqual, init_const_expr
{ 
			CurrentSemanticValue.stn = new simple_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 108: // only_const_decl -> const_name, tkColon, type_ref, tkEqual, typed_const
{ 
			CurrentSemanticValue.stn = new typed_const_definition(ValueStack[ValueStack.Depth-5].id, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-3].td, CurrentLocationSpan);
		}
        break;
      case 109: // init_const_expr -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 110: // init_const_expr -> array_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 111: // const_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 112: // expr_l1_list -> expr_l1
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 113: // expr_l1_list -> expr_l1_list, tkComma, expr_l1
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 114: // const_expr -> const_simple_expr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 115: // const_expr -> const_simple_expr, const_relop, const_simple_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 116: // const_expr -> question_constexpr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 117: // question_constexpr -> const_expr, tkQuestion, const_expr, tkColon, const_expr
{ CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 118: // const_relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 119: // const_relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 120: // const_relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 121: // const_relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 122: // const_relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 123: // const_relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 124: // const_relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 125: // const_simple_expr -> const_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 126: // const_simple_expr -> const_simple_expr, const_addop, const_term
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 127: // const_addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 128: // const_addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 129: // const_addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 130: // const_addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 131: // as_is_constexpr -> const_term, typecast_op, simple_or_template_type_reference
{ 
			CurrentSemanticValue.ex = NewAsIsConstexpr(ValueStack[ValueStack.Depth-3].ex, (op_typecast)ValueStack[ValueStack.Depth-2].ob, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);                                
		}
        break;
      case 132: // power_constexpr -> const_factor, tkStarStar, const_factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 133: // const_term -> const_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 134: // const_term -> as_is_constexpr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 135: // const_term -> power_constexpr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 136: // const_term -> const_term, const_mulop, const_factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 137: // const_term -> const_term, const_mulop, power_constexpr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 138: // const_mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 139: // const_mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 140: // const_mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 141: // const_mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 142: // const_mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 143: // const_mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 144: // const_mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 145: // const_factor -> const_variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 146: // const_factor -> const_set
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 147: // const_factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 148: // const_factor -> tkAddressOf, const_factor
{ 
			CurrentSemanticValue.ex = new get_address(ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);  
		}
        break;
      case 149: // const_factor -> tkRoundOpen, const_expr, tkRoundClose
{ 
	 	    CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 150: // const_factor -> tkNot, const_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 151: // const_factor -> sign, const_factor
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
      case 152: // const_factor -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 153: // const_set -> tkSquareOpen, const_elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new pascal_set_constant(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 154: // sign -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 155: // sign -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 156: // const_variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 157: // const_variable -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 158: // const_variable -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 159: // const_variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 160: // const_variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 161: // const_variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 162: // const_variable -> const_variable, const_variable_2
{
			CurrentSemanticValue.ex = NewConstVariable(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 163: // const_variable -> const_variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 164: // const_variable -> const_variable, tkSquareOpen, format_const_expr, 
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
      case 165: // const_variable_2 -> tkPoint, identifier_or_keyword
{ 
			CurrentSemanticValue.ex = new dot_node(null, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 166: // const_variable_2 -> tkDeref
{ 
			CurrentSemanticValue.ex = new roof_dereference();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 167: // const_variable_2 -> tkRoundOpen, optional_const_func_expr_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 168: // const_variable_2 -> tkSquareOpen, const_elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new indexer(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 169: // optional_const_func_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 170: // optional_const_func_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 171: // const_elem_list -> const_elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 173: // const_elem_list1 -> const_elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 174: // const_elem_list1 -> const_elem_list1, tkComma, const_elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 175: // const_elem -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 176: // const_elem -> const_expr, tkDotDot, const_expr
{ 
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 177: // unsigned_number -> tkInteger
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 178: // unsigned_number -> tkHex
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 179: // unsigned_number -> tkFloat
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 180: // typed_const -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 181: // typed_const -> array_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 182: // typed_const -> record_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 183: // array_const -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new array_const(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 184: // array_const -> tkRoundOpen, record_const, tkRoundClose
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 185: // array_const -> tkRoundOpen, array_const, tkRoundClose
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 187: // typed_const_list -> typed_const_list1
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 188: // typed_const_list1 -> typed_const_plus
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 189: // typed_const_list1 -> typed_const_list1, tkComma, typed_const_plus
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 190: // record_const -> tkRoundOpen, const_field_list, tkRoundClose
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 191: // const_field_list -> const_field_list_1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 192: // const_field_list -> const_field_list_1, tkSemiColon
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 193: // const_field_list_1 -> const_field
{ 
			CurrentSemanticValue.ex = new record_const(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
		}
        break;
      case 194: // const_field_list_1 -> const_field_list_1, tkSemiColon, const_field
{ 
			CurrentSemanticValue.ex = (ValueStack[ValueStack.Depth-3].ex as record_const).Add(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
		}
        break;
      case 195: // const_field -> const_field_name, tkColon, typed_const
{ 
			CurrentSemanticValue.stn = new record_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 196: // const_field_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 197: // type_decl -> attribute_declarations, simple_type_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = LocationStack[LocationStack.Depth-1];
        }
        break;
      case 198: // attribute_declarations -> attribute_declaration
{ 
			CurrentSemanticValue.stn = new attribute_list(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
    }
        break;
      case 199: // attribute_declarations -> attribute_declarations, attribute_declaration
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
		}
        break;
      case 200: // attribute_declarations -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 201: // attribute_declaration -> tkSquareOpen, one_or_some_attribute, tkSquareClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 202: // one_or_some_attribute -> one_attribute
{ 
			CurrentSemanticValue.stn = new simple_attribute_list(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
		}
        break;
      case 203: // one_or_some_attribute -> one_or_some_attribute, tkComma, one_attribute
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as simple_attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
		}
        break;
      case 204: // one_attribute -> attribute_variable
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 205: // one_attribute -> identifier, tkColon, attribute_variable
{  
			(ValueStack[ValueStack.Depth-1].stn as attribute).qualifier = ValueStack[ValueStack.Depth-3].id;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 206: // simple_type_decl -> type_decl_identifier, tkEqual, type_decl_type, tkSemiColon
{ 
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-4].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
		}
        break;
      case 207: // simple_type_decl -> template_identifier_with_equal, type_decl_type, tkSemiColon
{ 
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
		}
        break;
      case 208: // type_decl_identifier -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 209: // type_decl_identifier -> identifier, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 210: // template_identifier_with_equal -> identifier, tkLower, ident_list, 
                //                                   tkGreaterEqual
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-4].id.name, ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 211: // type_decl_type -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 212: // type_decl_type -> object_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 213: // simple_type_question -> simple_type, tkQuestion
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
      case 214: // type_ref -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 215: // type_ref -> simple_type_question
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 216: // type_ref -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 217: // type_ref -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 218: // type_ref -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 219: // type_ref -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 220: // type_ref -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 221: // template_type -> simple_type_identifier, template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference(ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan); 
		}
        break;
      case 222: // template_type_params -> tkLower, template_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 223: // template_type_empty_params -> tkNotEqual
{
            var ntr = new named_type_reference(new ident(""), CurrentLocationSpan);
            
			CurrentSemanticValue.stn = new template_param_list(ntr, CurrentLocationSpan);
            ntr.source_context = new SourceContext(CurrentSemanticValue.stn.source_context.end_position.line_num, CurrentSemanticValue.stn.source_context.end_position.column_num, CurrentSemanticValue.stn.source_context.begin_position.line_num, CurrentSemanticValue.stn.source_context.begin_position.column_num);
		}
        break;
      case 224: // template_type_empty_params -> tkLower, template_empty_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 225: // template_param_list -> template_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 226: // template_param_list -> template_param_list, tkComma, template_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 227: // template_empty_param_list -> template_empty_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 228: // template_empty_param_list -> template_empty_param_list, tkComma, 
                //                              template_empty_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 229: // template_empty_param -> /* empty */
{ 
            CurrentSemanticValue.td = new named_type_reference(new ident(""), CurrentLocationSpan);
        }
        break;
      case 230: // template_param -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 231: // template_param -> simple_type, tkQuestion
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
      case 232: // template_param -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 233: // template_param -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 234: // template_param -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 235: // simple_type -> range_expr
{
	    	CurrentSemanticValue.td = parsertools.ConvertDotNodeOrIdentToNamedTypeReference(ValueStack[ValueStack.Depth-1].ex); 
	    }
        break;
      case 236: // simple_type -> range_expr, tkDotDot, range_expr
{ 
			CurrentSemanticValue.td = new diapason(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 237: // simple_type -> tkRoundOpen, enumeration_id_list, tkRoundClose
{ 
			CurrentSemanticValue.td = new enum_type_definition(ValueStack[ValueStack.Depth-2].stn as enumerator_list, CurrentLocationSpan);  
		}
        break;
      case 238: // range_expr -> range_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 239: // range_expr -> range_expr, const_addop, range_term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 240: // range_term -> range_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 241: // range_term -> range_term, const_mulop, range_factor
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 242: // range_factor -> simple_type_identifier
{ 
			CurrentSemanticValue.ex = parsertools.ConvertNamedTypeReferenceToDotNodeOrIdent(ValueStack[ValueStack.Depth-1].td as named_type_reference);
        }
        break;
      case 243: // range_factor -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 244: // range_factor -> sign, range_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 245: // range_factor -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 246: // range_factor -> range_factor, tkRoundOpen, const_elem_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value, ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 247: // simple_type_identifier -> identifier
{ 
			CurrentSemanticValue.td = new named_type_reference(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 248: // simple_type_identifier -> simple_type_identifier, tkPoint, 
                //                           identifier_or_keyword
{ 
			CurrentSemanticValue.td = (ValueStack[ValueStack.Depth-3].td as named_type_reference).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 249: // enumeration_id_list -> enumeration_id, tkComma, enumeration_id
{ 
			CurrentSemanticValue.stn = new enumerator_list(ValueStack[ValueStack.Depth-3].stn as enumerator, CurrentLocationSpan);
			(CurrentSemanticValue.stn as enumerator_list).Add(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 250: // enumeration_id_list -> enumeration_id_list, tkComma, enumeration_id
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as enumerator_list).Add(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 251: // enumeration_id -> type_ref
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan); 
		}
        break;
      case 252: // enumeration_id -> type_ref, tkEqual, expr
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 253: // pointer_type -> tkDeref, fptype
{ 
			CurrentSemanticValue.td = new ref_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 254: // structured_type -> unpacked_structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 255: // structured_type -> tkPacked, unpacked_structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 256: // unpacked_structured_type -> array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 257: // unpacked_structured_type -> record_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 258: // unpacked_structured_type -> set_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 259: // unpacked_structured_type -> file_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 260: // unpacked_structured_type -> sequence_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 261: // sequence_type -> tkSequence, tkOf, type_ref
{
			CurrentSemanticValue.td = new sequence_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 262: // array_type -> tkArray, tkSquareOpen, simple_type_list, tkSquareClose, tkOf, 
                //               type_ref
{ 
			CurrentSemanticValue.td = new array_type(ValueStack[ValueStack.Depth-4].stn as indexers_types, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 263: // array_type -> unsized_array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 264: // unsized_array_type -> tkArray, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new array_type(null, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 265: // simple_type_list -> simple_type_or_
{ 
			CurrentSemanticValue.stn = new indexers_types(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 266: // simple_type_list -> simple_type_list, tkComma, simple_type_or_
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as indexers_types).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 267: // simple_type_or_ -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 268: // simple_type_or_ -> /* empty */
{ CurrentSemanticValue.td = null; }
        break;
      case 269: // set_type -> tkSet, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new set_type_definition(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 270: // file_type -> tkFile, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new file_type(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 271: // file_type -> tkFile
{ 
			CurrentSemanticValue.td = new file_type();  
			CurrentSemanticValue.td.source_context = CurrentLocationSpan;
		}
        break;
      case 272: // string_type -> tkIdentifier, tkSquareOpen, const_expr, tkSquareClose
{ 
			CurrentSemanticValue.td = new string_num_definition(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-4].id, CurrentLocationSpan);
		}
        break;
      case 273: // procedural_type -> procedural_type_kind
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 274: // procedural_type_kind -> proc_type_decl
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 275: // proc_type_decl -> tkProcedure, fp_list
{ 
			CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-1].stn as formal_parameters,null,null,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 276: // proc_type_decl -> tkFunction, fp_list, tkColon, fptype
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, null, null, null, ValueStack[ValueStack.Depth-1].td as type_definition, CurrentLocationSpan);
        }
        break;
      case 277: // proc_type_decl -> simple_type_identifier, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 278: // proc_type_decl -> template_type, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 279: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 280: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-4].stn as enumerator_list,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 281: // proc_type_decl -> simple_type_identifier, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 282: // proc_type_decl -> template_type, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 283: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,null,CurrentLocationSpan);
    	}
        break;
      case 284: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-5].stn as enumerator_list,null,CurrentLocationSpan);
    	}
        break;
      case 285: // object_type -> class_attributes, class_or_interface_keyword, 
                //                optional_base_classes, optional_where_section, 
                //                optional_component_list_seq_end
{ 
            var cd = NewObjectType((class_attribute)ValueStack[ValueStack.Depth-5].ob, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].stn as named_type_reference_list, ValueStack[ValueStack.Depth-2].stn as where_definition_list, ValueStack[ValueStack.Depth-1].stn as class_body_list, CurrentLocationSpan); 
			CurrentSemanticValue.td = cd;
            var tt = cd.DescendantNodes().OfType<class_definition>().Where(cld => cld.keyword == class_keyword.Record);
            if (tt.Count()>0)
            {
                foreach (var ttt in tt)
                {
	                var sc = ttt.source_context;
	                parsertools.AddErrorFromResource("NESTED_RECORD_DEFINITIONS_ARE_FORBIDDEN", new LexLocation(sc.begin_position.line_num, sc.begin_position.column_num-1, sc.end_position.line_num, sc.end_position.column_num, sc.FileName));
                }
            }
		}
        break;
      case 286: // record_type -> tkRecord, optional_base_classes, optional_where_section, 
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
      case 287: // class_attribute -> tkSealed
{ CurrentSemanticValue.ob = class_attribute.Sealed; }
        break;
      case 288: // class_attribute -> tkPartial
{ CurrentSemanticValue.ob = class_attribute.Partial; }
        break;
      case 289: // class_attribute -> tkAbstract
{ CurrentSemanticValue.ob = class_attribute.Abstract; }
        break;
      case 290: // class_attribute -> tkAuto
{ CurrentSemanticValue.ob = class_attribute.Auto; }
        break;
      case 291: // class_attribute -> tkStatic
{ CurrentSemanticValue.ob = class_attribute.Static; }
        break;
      case 292: // class_attributes -> /* empty */
{ 
			CurrentSemanticValue.ob = class_attribute.None; 
		}
        break;
      case 293: // class_attributes -> class_attributes1
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 294: // class_attributes1 -> class_attribute
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 295: // class_attributes1 -> class_attributes1, class_attribute
{
            if (((class_attribute)ValueStack[ValueStack.Depth-2].ob & (class_attribute)ValueStack[ValueStack.Depth-1].ob) == (class_attribute)ValueStack[ValueStack.Depth-1].ob)
                parsertools.AddErrorFromResource("ATTRIBUTE_REDECLARED",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.ob  = ((class_attribute)ValueStack[ValueStack.Depth-2].ob) | ((class_attribute)ValueStack[ValueStack.Depth-1].ob);
			//$$ = $1;
		}
        break;
      case 296: // class_or_interface_keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 297: // class_or_interface_keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 298: // class_or_interface_keyword -> tkTemplate
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-1].ti);
		}
        break;
      case 299: // class_or_interface_keyword -> tkTemplate, tkClass
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "c", CurrentLocationSpan);
		}
        break;
      case 300: // class_or_interface_keyword -> tkTemplate, tkRecord
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "r", CurrentLocationSpan);
		}
        break;
      case 301: // class_or_interface_keyword -> tkTemplate, tkInterface
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "i", CurrentLocationSpan);
		}
        break;
      case 302: // optional_component_list_seq_end -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 303: // optional_component_list_seq_end -> member_list_section, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 305: // optional_base_classes -> tkRoundOpen, base_classes_names_list, tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 306: // base_classes_names_list -> base_class_name
{ 
			CurrentSemanticValue.stn = new named_type_reference_list(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 307: // base_classes_names_list -> base_classes_names_list, tkComma, base_class_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as named_type_reference_list).Add(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 308: // base_class_name -> simple_type_identifier
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 309: // base_class_name -> template_type
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 310: // template_arguments -> tkLower, ident_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 311: // optional_where_section -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 312: // optional_where_section -> where_part_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 313: // where_part_list -> where_part
{ 
			CurrentSemanticValue.stn = new where_definition_list(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 314: // where_part_list -> where_part_list, where_part
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as where_definition_list).Add(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 315: // where_part -> tkWhere, ident_list, tkColon, type_ref_and_secific_list, 
                //               tkSemiColon
{ 
			CurrentSemanticValue.stn = new where_definition(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-2].stn as where_type_specificator_list, CurrentLocationSpan); 
		}
        break;
      case 316: // type_ref_and_secific_list -> type_ref_or_secific
{ 
			CurrentSemanticValue.stn = new where_type_specificator_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 317: // type_ref_and_secific_list -> type_ref_and_secific_list, tkComma, 
                //                              type_ref_or_secific
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as where_type_specificator_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 318: // type_ref_or_secific -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 319: // type_ref_or_secific -> tkClass
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefClass, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 320: // type_ref_or_secific -> tkRecord
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefValueType, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 321: // type_ref_or_secific -> tkConstructor
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 322: // member_list_section -> member_list
{ 
			CurrentSemanticValue.stn = new class_body_list(ValueStack[ValueStack.Depth-1].stn as class_members, CurrentLocationSpan);
        }
        break;
      case 323: // member_list_section -> member_list_section, ot_visibility_specifier, 
                //                        member_list
{ 
		    (ValueStack[ValueStack.Depth-1].stn as class_members).access_mod = ValueStack[ValueStack.Depth-2].stn as access_modifer_node;
			(ValueStack[ValueStack.Depth-3].stn as class_body_list).Add(ValueStack[ValueStack.Depth-1].stn as class_members,CurrentLocationSpan);
			
			if ((ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks[0].Count == 0)
                (ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks.RemoveAt(0);
			
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 324: // ot_visibility_specifier -> tkInternal
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.internal_modifer, CurrentLocationSpan); }
        break;
      case 325: // ot_visibility_specifier -> tkPublic
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.public_modifer, CurrentLocationSpan); }
        break;
      case 326: // ot_visibility_specifier -> tkProtected
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.protected_modifer, CurrentLocationSpan); }
        break;
      case 327: // ot_visibility_specifier -> tkPrivate
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.private_modifer, CurrentLocationSpan); }
        break;
      case 328: // member_list -> /* empty */
{ CurrentSemanticValue.stn = new class_members(); }
        break;
      case 329: // member_list -> field_or_const_definition_list, optional_semicolon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 330: // member_list -> method_decl_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 331: // member_list -> field_or_const_definition_list, tkSemiColon, method_decl_list
{  
			(ValueStack[ValueStack.Depth-3].stn as class_members).members.AddRange((ValueStack[ValueStack.Depth-1].stn as class_members).members);
			(ValueStack[ValueStack.Depth-3].stn as class_members).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 332: // ident_list -> identifier
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 333: // ident_list -> ident_list, tkComma, identifier
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 334: // optional_semicolon -> /* empty */
{ CurrentSemanticValue.ob = null; }
        break;
      case 335: // optional_semicolon -> tkSemiColon
{ CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 336: // field_or_const_definition_list -> field_or_const_definition
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 337: // field_or_const_definition_list -> field_or_const_definition_list, tkSemiColon, 
                //                                   field_or_const_definition
{   
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 338: // field_or_const_definition -> attribute_declarations, 
                //                              simple_field_or_const_definition
{  
		    (ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 339: // method_decl_list -> method_or_property_decl
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 340: // method_decl_list -> method_decl_list, method_or_property_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 341: // method_or_property_decl -> method_decl_withattr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 342: // method_or_property_decl -> property_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 343: // simple_field_or_const_definition -> tkConst, only_const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 344: // simple_field_or_const_definition -> field_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 345: // simple_field_or_const_definition -> class_or_static, field_definition
{ 
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).var_attr = definition_attribute.Static;
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 346: // class_or_static -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 347: // class_or_static -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 348: // field_definition -> var_decl_part
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 349: // field_definition -> tkEvent, ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, true, CurrentLocationSpan); 
        }
        break;
      case 350: // method_decl_withattr -> attribute_declarations, method_header
{  
			(ValueStack[ValueStack.Depth-1].td as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td;
        }
        break;
      case 351: // method_decl_withattr -> attribute_declarations, method_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
            if (ValueStack[ValueStack.Depth-1].stn is procedure_definition && (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
                (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
     }
        break;
      case 352: // method_decl -> inclass_proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 353: // method_decl -> inclass_constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 354: // method_header -> class_or_static, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 355: // method_header -> method_procfunc_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 356: // method_header -> constr_destr_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 357: // method_procfunc_header -> proc_func_header
{ 
			CurrentSemanticValue.td = NewProcfuncHeading(ValueStack[ValueStack.Depth-1].td as procedure_header);
		}
        break;
      case 358: // proc_func_header -> proc_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 359: // proc_func_header -> func_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 360: // constr_destr_header -> tkConstructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 361: // constr_destr_header -> class_or_static, tkConstructor, optional_proc_name, 
                //                        fp_list, optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,true,null,null,CurrentLocationSpan);
        }
        break;
      case 362: // constr_destr_header -> tkDestructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new destructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name, false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 363: // optional_proc_name -> proc_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 364: // optional_proc_name -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 365: // qualified_identifier -> identifier
{ CurrentSemanticValue.stn = new method_name(null,null,ValueStack[ValueStack.Depth-1].id,null,CurrentLocationSpan); }
        break;
      case 366: // qualified_identifier -> visibility_specifier
{ CurrentSemanticValue.stn = new method_name(null,null,ValueStack[ValueStack.Depth-1].id,null,CurrentLocationSpan); }
        break;
      case 367: // qualified_identifier -> qualified_identifier, tkPoint, identifier
{
			CurrentSemanticValue.stn = NewQualifiedIdentifier(ValueStack[ValueStack.Depth-3].stn as method_name, ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
        }
        break;
      case 368: // qualified_identifier -> qualified_identifier, tkPoint, visibility_specifier
{
			CurrentSemanticValue.stn = NewQualifiedIdentifier(ValueStack[ValueStack.Depth-3].stn as method_name, ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
        }
        break;
      case 369: // property_definition -> attribute_declarations, simple_property_definition
{  
			CurrentSemanticValue.stn = NewPropertyDefinition(ValueStack[ValueStack.Depth-2].stn as attribute_list, ValueStack[ValueStack.Depth-1].stn as declaration, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 370: // simple_property_definition -> tkProperty, qualified_identifier, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        }
        break;
      case 371: // simple_property_definition -> tkProperty, qualified_identifier, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, property_modificator, tkSemiColon, 
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
      case 372: // simple_property_definition -> class_or_static, tkProperty, qualified_identifier, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        	(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
        }
        break;
      case 373: // simple_property_definition -> class_or_static, tkProperty, qualified_identifier, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, property_modificator, tkSemiColon, 
                //                               array_defaultproperty
{ 
			parsertools.AddErrorFromResource("STATIC_PROPERTIES_CANNOT_HAVE_ATTRBUTE_{0}",LocationStack[LocationStack.Depth-3],ValueStack[ValueStack.Depth-3].id.name);        	
        }
        break;
      case 374: // simple_property_definition -> tkAuto, tkProperty, qualified_identifier, 
                //                               property_interface, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-3].stn as method_name, ValueStack[ValueStack.Depth-2].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
		}
        break;
      case 375: // simple_property_definition -> class_or_static, tkAuto, tkProperty, 
                //                               qualified_identifier, property_interface, 
                //                               tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-3].stn as method_name, ValueStack[ValueStack.Depth-2].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
		}
        break;
      case 376: // array_defaultproperty -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 377: // array_defaultproperty -> tkDefault, tkSemiColon
{ 
			CurrentSemanticValue.stn = new property_array_default();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 378: // property_interface -> property_parameter_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new property_interface(ValueStack[ValueStack.Depth-3].stn as property_parameter_list, ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan);
        }
        break;
      case 379: // property_parameter_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 380: // property_parameter_list -> tkSquareOpen, parameter_decl_list, tkSquareClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 381: // parameter_decl_list -> parameter_decl
{ 
			CurrentSemanticValue.stn = new property_parameter_list(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
		}
        break;
      case 382: // parameter_decl_list -> parameter_decl_list, tkSemiColon, parameter_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as property_parameter_list).Add(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
		}
        break;
      case 383: // parameter_decl -> ident_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new property_parameter(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 384: // optional_read_expr -> expr_with_func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 385: // optional_read_expr -> /* empty */
{ CurrentSemanticValue.ex = null; }
        break;
      case 387: // property_specifiers -> tkRead, optional_read_expr, write_property_specifiers
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
      case 388: // property_specifiers -> tkWrite, unlabelled_stmt, read_property_specifiers
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
      case 390: // write_property_specifiers -> tkWrite, unlabelled_stmt
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
      case 392: // read_property_specifiers -> tkRead, optional_read_expr
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
      case 393: // var_decl -> var_decl_part, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 396: // var_decl_part -> ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, false, CurrentLocationSpan);
		}
        break;
      case 397: // var_decl_part -> ident_list, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, null, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan);		
		}
        break;
      case 398: // var_decl_part -> ident_list, tkColon, type_ref, tkAssignOrEqual, 
                //                  typed_var_init_expression
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan); 
		}
        break;
      case 399: // typed_var_init_expression -> typed_const_plus
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 400: // typed_var_init_expression -> expl_func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 401: // typed_var_init_expression -> identifier, tkArrow, lambda_function_body
{  
			var idList = new ident_list(ValueStack[ValueStack.Depth-3].id, LocationStack[LocationStack.Depth-3]); 
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 402: // typed_var_init_expression -> tkRoundOpen, tkRoundClose, lambda_type_ref, 
                //                              tkArrow, lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 403: // typed_var_init_expression -> tkRoundOpen, typed_const_list, tkRoundClose, 
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
      case 404: // typed_const_plus -> typed_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 405: // typed_const_plus -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 406: // constr_destr_decl -> constr_destr_header, block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 407: // constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, tkAssign, 
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
      case 408: // constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
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
      case 409: // inclass_constr_destr_decl -> constr_destr_header, inclass_block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 410: // inclass_constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, 
                //                              tkAssign, unlabelled_stmt, tkSemiColon
{ 
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,false,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]));
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), CurrentLocationSpan);
            if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
        }
        break;
      case 411: // inclass_constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
                //                              fp_list, tkAssign, unlabelled_stmt, tkSemiColon
{ 
   			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-1]);
            var tmp = new constructor(null,ValueStack[ValueStack.Depth-4].stn as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan),ValueStack[ValueStack.Depth-5].stn as method_name,false,true,null,null,LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]));
            CurrentSemanticValue.stn = new procedure_definition(tmp as procedure_header, new block(null,new statement_list(ValueStack[ValueStack.Depth-2].stn as statement,LocationStack[LocationStack.Depth-2]),LocationStack[LocationStack.Depth-2]), CurrentLocationSpan);
            if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
        }
        break;
      case 412: // proc_func_decl -> proc_func_decl_noclass
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 413: // proc_func_decl -> class_or_static, proc_func_decl_noclass
{ 
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 414: // proc_func_decl_noclass -> proc_func_header, proc_func_external_block
{
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
        }
        break;
      case 415: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                           optional_method_modificators1, tkAssign, expr_l1, 
                //                           tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 416: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, expr_l1, 
                //                           tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].ex is dot_question_node)
				parsertools.AddErrorFromResource("DOT_QUECTION_IN_SHORT_FUN",LocationStack[LocationStack.Depth-2]);
	
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 417: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                           optional_method_modificators1, tkAssign, 
                //                           func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 418: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, 
                //                           func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 419: // proc_func_decl_noclass -> tkProcedure, proc_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, 
                //                           unlabelled_stmt, tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-2]);
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 420: // proc_func_decl_noclass -> proc_func_header, tkForward, tkSemiColon
{
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-3].td as procedure_header, null, CurrentLocationSpan);
            (CurrentSemanticValue.stn as procedure_definition).proc_header.proc_attributes.Add((procedure_attribute)ValueStack[ValueStack.Depth-2].id, ValueStack[ValueStack.Depth-2].id.source_context);
		}
        break;
      case 421: // inclass_proc_func_decl -> inclass_proc_func_decl_noclass
{ 
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
        }
        break;
      case 422: // inclass_proc_func_decl -> class_or_static, inclass_proc_func_decl_noclass
{ 
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 423: // inclass_proc_func_decl_noclass -> proc_func_header, inclass_block
{
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
		}
        break;
      case 424: // inclass_proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, 
                //                                   fptype, optional_method_modificators1, 
                //                                   tkAssign, expr_l1_func_decl_lambda, 
                //                                   tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
			if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 425: // inclass_proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                                   optional_method_modificators1, tkAssign, 
                //                                   expr_l1_func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
			if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 426: // inclass_proc_func_decl_noclass -> tkProcedure, proc_name, fp_list, 
                //                                   optional_method_modificators1, tkAssign, 
                //                                   unlabelled_stmt, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
			if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 427: // proc_func_external_block -> block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 428: // proc_func_external_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 429: // proc_name -> func_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 430: // func_name -> func_meth_name_ident
{ 
			CurrentSemanticValue.stn = new method_name(null,null, ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan); 
		}
        break;
      case 431: // func_name -> func_class_name_ident_list, tkPoint, func_meth_name_ident
{ 
        	var ln = ValueStack[ValueStack.Depth-3].ob as List<ident>;
        	var cnt = ln.Count;
        	if (cnt == 1)
				CurrentSemanticValue.stn = new method_name(null, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
			else 	
				CurrentSemanticValue.stn = new method_name(ln, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
		}
        break;
      case 432: // func_class_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 433: // func_class_name_ident_list -> func_class_name_ident
{ 
			CurrentSemanticValue.ob = new List<ident>(); 
			(CurrentSemanticValue.ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
		}
        break;
      case 434: // func_class_name_ident_list -> func_class_name_ident_list, tkPoint, 
                //                               func_class_name_ident
{ 
			(ValueStack[ValueStack.Depth-3].ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob; 
		}
        break;
      case 435: // func_meth_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 436: // func_meth_name_ident -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 437: // func_meth_name_ident -> operator_name_ident, template_arguments
{ CurrentSemanticValue.id = new template_operator_name(null, ValueStack[ValueStack.Depth-1].stn as ident_list, ValueStack[ValueStack.Depth-2].ex as operator_name_ident, CurrentLocationSpan); }
        break;
      case 438: // func_name_with_template_args -> func_name_ident
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 439: // func_name_with_template_args -> func_name_ident, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 440: // func_name_ident -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 441: // proc_header -> tkProcedure, proc_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{ 
        	CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, CurrentLocationSpan); 
        }
        break;
      case 442: // func_header -> tkFunction, func_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, null, CurrentLocationSpan); 
		}
        break;
      case 443: // func_header -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                optional_method_modificators, optional_where_section
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, ValueStack[ValueStack.Depth-3].td as type_definition, CurrentLocationSpan); 
        }
        break;
      case 444: // external_block -> tkExternal, external_directive_ident, tkName, 
                //                   external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 445: // external_block -> tkExternal, external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-2].ex, null, CurrentLocationSpan); 
		}
        break;
      case 446: // external_block -> tkExternal, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(null, null, CurrentLocationSpan); 
		}
        break;
      case 447: // external_directive_ident -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 448: // external_directive_ident -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 449: // block -> decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 450: // inclass_block -> inclass_decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 451: // inclass_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 452: // fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 453: // fp_list -> tkRoundOpen, tkRoundClose
{ 
			CurrentSemanticValue.stn = null;
		}
        break;
      case 454: // fp_list -> tkRoundOpen, fp_sect_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			if (CurrentSemanticValue.stn != null)
				CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 455: // fp_sect_list -> fp_sect
{ 
			CurrentSemanticValue.stn = new formal_parameters(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
        }
        break;
      case 456: // fp_sect_list -> fp_sect_list, tkSemiColon, fp_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);   
        }
        break;
      case 457: // fp_sect -> attribute_declarations, simple_fp_sect
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as  attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 458: // simple_fp_sect -> param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan); 
		}
        break;
      case 459: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.var_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 460: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.const_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 461: // simple_fp_sect -> tkParams, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td,parametr_kind.params_parametr,null, CurrentLocationSpan);  
		}
        break;
      case 462: // simple_fp_sect -> param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.none, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 463: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.var_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 464: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.const_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 465: // param_name_list -> param_name
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 466: // param_name_list -> param_name_list, tkComma, param_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);  
		}
        break;
      case 467: // param_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 468: // fptype -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 469: // fptype_noproctype -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 470: // fptype_noproctype -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 471: // fptype_noproctype -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 472: // fptype_noproctype -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 473: // fptype_noproctype -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 474: // stmt -> unlabelled_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 475: // stmt -> label_name, tkColon, stmt
{ 
			CurrentSemanticValue.stn = new labeled_statement(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);  
		}
        break;
      case 476: // unlabelled_stmt -> /* empty */
{ 
			CurrentSemanticValue.stn = new empty_statement(); 
			CurrentSemanticValue.stn.source_context = null;
		}
        break;
      case 477: // unlabelled_stmt -> assignment
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 478: // unlabelled_stmt -> proc_call
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 479: // unlabelled_stmt -> goto_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 480: // unlabelled_stmt -> compound_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 481: // unlabelled_stmt -> if_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 482: // unlabelled_stmt -> case_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 483: // unlabelled_stmt -> repeat_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 484: // unlabelled_stmt -> while_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 485: // unlabelled_stmt -> for_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 486: // unlabelled_stmt -> with_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 487: // unlabelled_stmt -> inherited_message
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 488: // unlabelled_stmt -> try_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 489: // unlabelled_stmt -> raise_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 490: // unlabelled_stmt -> foreach_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 491: // unlabelled_stmt -> var_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 492: // unlabelled_stmt -> expr_as_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 493: // unlabelled_stmt -> lock_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 494: // unlabelled_stmt -> yield_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 495: // unlabelled_stmt -> yield_sequence_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 496: // unlabelled_stmt -> loop_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 497: // unlabelled_stmt -> match_with
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 498: // loop_stmt -> tkLoop, expr_l1, tkDo, unlabelled_stmt
{
			CurrentSemanticValue.stn = new loop_stmt(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].stn as statement,CurrentLocationSpan);
		}
        break;
      case 499: // yield_stmt -> tkYield, expr_l1
{
			CurrentSemanticValue.stn = new yield_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 500: // yield_sequence_stmt -> tkYield, tkSequence, expr_l1
{
			CurrentSemanticValue.stn = new yield_sequence_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 501: // var_stmt -> tkVar, var_decl_part
{ 
			CurrentSemanticValue.stn = new var_statement(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 502: // var_stmt -> tkRoundOpen, tkVar, identifier, tkComma, var_ident_list, 
                //             tkRoundClose, tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].ob as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].ob as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 503: // var_stmt -> tkVar, tkRoundOpen, identifier, tkComma, ident_list, tkRoundClose, 
                //             tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			ValueStack[ValueStack.Depth-4].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
	    }
        break;
      case 504: // assignment -> var_reference, assign_operator, expr_with_func_decl_lambda
{      
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan);
        }
        break;
      case 505: // assignment -> tkRoundOpen, variable, tkComma, variable_list, tkRoundClose, 
                //               assign_operator, expr
{
			if (ValueStack[ValueStack.Depth-2].op.type != Operators.Assignment)
			    parsertools.AddErrorFromResource("ONLY_BASE_ASSIGNMENT_FOR_TUPLE",LocationStack[LocationStack.Depth-2]);
			(ValueStack[ValueStack.Depth-4].ob as addressed_value_list).Insert(0,ValueStack[ValueStack.Depth-6].ex as addressed_value);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_tuple(ValueStack[ValueStack.Depth-4].ob as addressed_value_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 506: // variable_list -> variable
{
		CurrentSemanticValue.ob = new addressed_value_list(ValueStack[ValueStack.Depth-1].ex as addressed_value,LocationStack[LocationStack.Depth-1]);
	}
        break;
      case 507: // variable_list -> variable_list, tkComma, variable
{
		(ValueStack[ValueStack.Depth-3].ob as addressed_value_list).Add(ValueStack[ValueStack.Depth-1].ex as addressed_value);
		(ValueStack[ValueStack.Depth-3].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
	}
        break;
      case 508: // var_ident_list -> tkVar, identifier
{
		CurrentSemanticValue.ob = new ident_list(ValueStack[ValueStack.Depth-1].id,CurrentLocationSpan);
	}
        break;
      case 509: // var_ident_list -> var_ident_list, tkComma, tkVar, identifier
{
		(ValueStack[ValueStack.Depth-4].ob as ident_list).Add(ValueStack[ValueStack.Depth-1].id);
		(ValueStack[ValueStack.Depth-4].ob as ident_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-4].ob;
	}
        break;
      case 510: // proc_call -> var_reference
{ 
			CurrentSemanticValue.stn = new procedure_call(ValueStack[ValueStack.Depth-1].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex is ident, CurrentLocationSpan); 
		}
        break;
      case 511: // goto_stmt -> tkGoto, label_name
{ 
			CurrentSemanticValue.stn = new goto_statement(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 512: // compound_stmt -> tkBegin, stmt_list, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			(CurrentSemanticValue.stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(CurrentSemanticValue.stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 513: // stmt_list -> stmt
{ 
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 514: // stmt_list -> stmt_list, tkSemiColon, stmt
{  
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as statement_list).Add(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
        }
        break;
      case 515: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan); 
        }
        break;
      case 516: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt, tkElse, unlabelled_stmt
{
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as statement, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 517: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, else_case, tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 518: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, tkSemiColon, else_case, 
                //               tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 519: // pattern_cases -> pattern_case
{
            CurrentSemanticValue.stn = new pattern_cases(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 520: // pattern_cases -> pattern_cases, tkSemiColon, pattern_case
{
            CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as pattern_cases).Add(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 521: // pattern_case -> pattern_optional_var, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 522: // pattern_case -> pattern_optional_var, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 523: // pattern_case -> const_pattern, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
		}
        break;
      case 524: // pattern_case -> const_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 525: // case_stmt -> tkCase, expr_l1, tkOf, case_list, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 526: // case_stmt -> tkCase, expr_l1, tkOf, case_list, tkSemiColon, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 527: // case_stmt -> tkCase, expr_l1, tkOf, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-4].ex, NewCaseItem(new empty_statement(), null), ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 528: // case_list -> case_item
{
			if (ValueStack[ValueStack.Depth-1].stn is empty_statement) 
				CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, null);
			else CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 529: // case_list -> case_list, tkSemiColon, case_item
{ 
			CurrentSemanticValue.stn = AddCaseItem(ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 530: // case_item -> case_label_list, tkColon, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new case_variant(ValueStack[ValueStack.Depth-3].stn as expression_list, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 531: // case_label_list -> case_label
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 532: // case_label_list -> case_label_list, tkComma, case_label
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 533: // case_label -> const_elem
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 534: // else_case -> /* empty */
{ CurrentSemanticValue.stn = null;}
        break;
      case 535: // else_case -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 536: // repeat_stmt -> tkRepeat, stmt_list, tkUntil, expr
{ 
		    CurrentSemanticValue.stn = new repeat_node(ValueStack[ValueStack.Depth-3].stn as statement_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-3].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-4].ti;
			(ValueStack[ValueStack.Depth-3].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-2].ti;
			ValueStack[ValueStack.Depth-3].stn.source_context = LocationStack[LocationStack.Depth-4].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 537: // while_stmt -> tkWhile, expr_l1, optional_tk_do, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewWhileStmt(ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);    
        }
        break;
      case 538: // optional_tk_do -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 539: // optional_tk_do -> /* empty */
{ CurrentSemanticValue.ti = null; }
        break;
      case 540: // lock_stmt -> tkLock, expr_l1, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new lock_stmt(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 541: // foreach_stmt -> tkForeach, identifier, foreach_stmt_ident_dype_opt, tkIn, 
                //                 expr_l1, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-6].id, ValueStack[ValueStack.Depth-5].td, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
            if (ValueStack[ValueStack.Depth-5].td == null)
                parsertools.AddWarningFromResource("USING_UNLOCAL_FOREACH_VARIABLE", ValueStack[ValueStack.Depth-6].id.source_context);
        }
        break;
      case 542: // foreach_stmt -> tkForeach, tkVar, identifier, tkColon, type_ref, tkIn, expr_l1, 
                //                 tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-5].td, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 543: // foreach_stmt -> tkForeach, tkVar, identifier, tkIn, expr_l1, tkDo, 
                //                 unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-5].id, new no_type_foreach(), ValueStack[ValueStack.Depth-3].ex, (statement)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 544: // foreach_stmt_ident_dype_opt -> tkColon, type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 546: // for_stmt -> tkFor, optional_var, identifier, for_stmt_decl_or_assign, expr_l1, 
                //             for_cycle_type, expr_l1, optional_tk_do, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-8].ob, ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-5].ex, (for_cycle_type)ValueStack[ValueStack.Depth-4].ob, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
        }
        break;
      case 547: // optional_var -> tkVar
{ CurrentSemanticValue.ob = true; }
        break;
      case 548: // optional_var -> /* empty */
{ CurrentSemanticValue.ob = false; }
        break;
      case 550: // for_stmt_decl_or_assign -> tkColon, simple_type_identifier, tkAssign
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-2].td; }
        break;
      case 551: // for_cycle_type -> tkTo
{ CurrentSemanticValue.ob = for_cycle_type.to; }
        break;
      case 552: // for_cycle_type -> tkDownto
{ CurrentSemanticValue.ob = for_cycle_type.downto; }
        break;
      case 553: // with_stmt -> tkWith, expr_list, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new with_statement(ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 554: // inherited_message -> tkInherited
{ 
			CurrentSemanticValue.stn = new inherited_message();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 555: // try_stmt -> tkTry, stmt_list, try_handler
{ 
			CurrentSemanticValue.stn = new try_stmt(ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].stn as try_handler, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			ValueStack[ValueStack.Depth-2].stn.source_context = LocationStack[LocationStack.Depth-3].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 556: // try_handler -> tkFinally, stmt_list, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_finally(ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan);
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(ValueStack[ValueStack.Depth-2].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
		}
        break;
      case 557: // try_handler -> tkExcept, exception_block, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_except((exception_block)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan);  
			if ((ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list != null)
			{
				(ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list.source_context = CurrentLocationSpan;
				(ValueStack[ValueStack.Depth-2].stn as exception_block).source_context = CurrentLocationSpan;
			}
		}
        break;
      case 558: // exception_block -> exception_handler_list, exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-2].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 559: // exception_block -> exception_handler_list, tkSemiColon, 
                //                    exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-3].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 560: // exception_block -> stmt_list
{ 
			CurrentSemanticValue.stn = new exception_block(ValueStack[ValueStack.Depth-1].stn as statement_list, null, null, LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 561: // exception_handler_list -> exception_handler
{ 
			CurrentSemanticValue.stn = new exception_handler_list(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 562: // exception_handler_list -> exception_handler_list, tkSemiColon, 
                //                           exception_handler
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as exception_handler_list).Add(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 563: // exception_block_else_branch -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 564: // exception_block_else_branch -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 565: // exception_handler -> tkOn, exception_identifier, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new exception_handler((ValueStack[ValueStack.Depth-3].stn as exception_ident).variable, (ValueStack[ValueStack.Depth-3].stn as exception_ident).type_name, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 566: // exception_identifier -> exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(null, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 567: // exception_identifier -> exception_variable, tkColon, 
                //                         exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(ValueStack[ValueStack.Depth-3].id, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 568: // exception_class_type_identifier -> simple_type_identifier
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 569: // exception_variable -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 570: // raise_stmt -> tkRaise
{ 
			CurrentSemanticValue.stn = new raise_stmt(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 571: // raise_stmt -> tkRaise, expr
{ 
			CurrentSemanticValue.stn = new raise_stmt(ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan);  
		}
        break;
      case 572: // expr_list -> expr_with_func_decl_lambda
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 573: // expr_list -> expr_list, tkComma, expr_with_func_decl_lambda
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 574: // expr_as_stmt -> allowable_expr_as_stmt
{ 
			CurrentSemanticValue.stn = new expression_as_statement(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 575: // allowable_expr_as_stmt -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 576: // expr_with_func_decl_lambda -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 577: // expr_with_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 578: // expr -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 579: // expr -> format_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 580: // expr_l1 -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 581: // expr_l1 -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 582: // expr_l1_func_decl_lambda -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 583: // expr_l1_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 584: // expr_dq -> relop_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 585: // expr_dq -> expr_dq, tkDoubleQuestion, relop_expr
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 586: // sizeof_expr -> tkSizeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new sizeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, null, CurrentLocationSpan);  
		}
        break;
      case 587: // typeof_expr -> tkTypeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 588: // typeof_expr -> tkTypeOf, tkRoundOpen, empty_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 589: // question_expr -> expr_l1, tkQuestion, expr_l1, tkColon, expr_l1
{ 
            if (ValueStack[ValueStack.Depth-3].ex is nil_const && ValueStack[ValueStack.Depth-1].ex is nil_const)
            	parsertools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 590: // empty_template_type_reference -> simple_type_identifier, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 591: // empty_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
        }
        break;
      case 592: // simple_or_template_type_reference -> simple_type_identifier
{ 
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 593: // simple_or_template_type_reference -> simple_type_identifier, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 594: // simple_or_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 595: // optional_array_initializer -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = new array_const((expression_list)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan); 
		}
        break;
      case 597: // new_expr -> tkNew, simple_or_template_type_reference, 
                //             optional_expr_list_with_bracket
{
			CurrentSemanticValue.ex = new new_expr(ValueStack[ValueStack.Depth-2].td, ValueStack[ValueStack.Depth-1].stn as expression_list, false, null, CurrentLocationSpan);
        }
        break;
      case 598: // new_expr -> tkNew, simple_or_template_type_reference, tkSquareOpen, 
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
      case 599: // new_expr -> tkNew, tkClass, tkRoundOpen, list_fields_in_unnamed_object, 
                //             tkRoundClose
{
        // sugared node	
        	var l = ValueStack[ValueStack.Depth-2].ob as name_assign_expr_list;
        	var exprs = l.name_expr.Select(x=>x.expr).ToList();
        	var typename = "AnonymousType#"+Guid();
        	var type = new named_type_reference(typename,LocationStack[LocationStack.Depth-5]);
        	
			// node new_expr - for code generation of new node
			var ne = new new_expr(type, new expression_list(exprs), CurrentLocationSpan);
			// node unnamed_type_object - for formatting and code generation (new node and Anonymous class)
			CurrentSemanticValue.ex = new unnamed_type_object(l, true, ne, CurrentLocationSpan);
        }
        break;
      case 600: // field_in_unnamed_object -> identifier, tkAssign, relop_expr
{
			CurrentSemanticValue.ob = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 601: // field_in_unnamed_object -> relop_expr
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
      case 602: // list_fields_in_unnamed_object -> field_in_unnamed_object
{
			var l = new name_assign_expr_list();
			CurrentSemanticValue.ob = l.Add(ValueStack[ValueStack.Depth-1].ob as name_assign_expr);
		}
        break;
      case 603: // list_fields_in_unnamed_object -> list_fields_in_unnamed_object, tkComma, 
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
      case 604: // optional_expr_list_with_bracket -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 605: // optional_expr_list_with_bracket -> tkRoundOpen, optional_expr_list, 
                //                                    tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 606: // relop_expr -> simple_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 607: // relop_expr -> relop_expr, relop, simple_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 608: // relop_expr -> is_expr, tkRoundOpen, pattern_out_param_list, tkRoundClose
{
            var isTypeCheck = ValueStack[ValueStack.Depth-4].ex as typecast_node;
            var deconstructorPattern = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_deconstructor_parameter>, isTypeCheck.type_def, CurrentLocationSpan); 
            CurrentSemanticValue.ex = new is_pattern_expr(isTypeCheck.expr, deconstructorPattern, null, CurrentLocationSpan);
        }
        break;
      case 609: // pattern -> simple_or_template_type_reference, tkRoundOpen, 
                //            pattern_out_param_list, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_deconstructor_parameter>, ValueStack[ValueStack.Depth-4].td, CurrentLocationSpan); 
        }
        break;
      case 610: // pattern_optional_var -> simple_or_template_type_reference, tkRoundOpen, 
                //                         pattern_out_param_list_optional_var, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_deconstructor_parameter>, ValueStack[ValueStack.Depth-4].td, CurrentLocationSpan); 
        }
        break;
      case 611: // const_pattern -> const_pattern_expr_list
{
			CurrentSemanticValue.stn = new const_pattern(ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 612: // const_pattern_expr_list -> const_pattern_expression
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 613: // const_pattern_expr_list -> const_pattern_expr_list, tkComma, 
                //                            const_pattern_expression
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 614: // const_pattern_expression -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;  }
        break;
      case 615: // const_pattern_expression -> tuple_pattern
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;  }
        break;
      case 616: // tuple_pattern -> tkRoundOpen, tuple_pattern_expr, tkComma, 
                //                  tuple_pattern_expr_list, lambda_type_ref, 
                //                  optional_full_lambda_fp_list, tkRoundClose
{
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
      case 617: // tuple_pattern_expr -> tkUnderscore
{ CurrentSemanticValue.ex = new tuple_wild_card(); }
        break;
      case 618: // tuple_pattern_expr -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 619: // tuple_pattern_expr_list -> tuple_pattern_expr
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 620: // tuple_pattern_expr_list -> tuple_pattern_expr_list, tkComma, tuple_pattern_expr
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 621: // pattern_out_param_list_optional_var -> pattern_out_param_optional_var
{
            CurrentSemanticValue.ob = new List<pattern_deconstructor_parameter>();
            (CurrentSemanticValue.ob as List<pattern_deconstructor_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_deconstructor_parameter);
        }
        break;
      case 622: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkSemiColon, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_deconstructor_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_deconstructor_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 623: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkComma, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_deconstructor_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_deconstructor_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 624: // pattern_out_param_list -> pattern_out_param
{
            CurrentSemanticValue.ob = new List<pattern_deconstructor_parameter>();
            (CurrentSemanticValue.ob as List<pattern_deconstructor_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_deconstructor_parameter);
        }
        break;
      case 625: // pattern_out_param_list -> pattern_out_param_list, tkSemiColon, 
                //                           pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_deconstructor_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_deconstructor_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 626: // pattern_out_param_list -> pattern_out_param_list, tkComma, pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_deconstructor_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_deconstructor_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 627: // pattern_out_param -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter();
		}
        break;
      case 628: // pattern_out_param -> literal_or_number
{
			CurrentSemanticValue.stn = new const_deconstructor_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 629: // pattern_out_param -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 630: // pattern_out_param -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 631: // pattern_out_param -> pattern
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 632: // pattern_out_param_optional_var -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter();
		}
        break;
      case 633: // pattern_out_param_optional_var -> literal_or_number
{
			CurrentSemanticValue.stn = new const_deconstructor_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 634: // pattern_out_param_optional_var -> identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 635: // pattern_out_param_optional_var -> identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 636: // pattern_out_param_optional_var -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 637: // pattern_out_param_optional_var -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 638: // pattern_out_param_optional_var -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 639: // simple_expr_or_nothing -> simple_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 640: // simple_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 641: // const_expr_or_nothing -> const_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 642: // const_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 643: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 644: // format_expr -> tkColon, simple_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 645: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing, tkColon, 
                //                simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 646: // format_expr -> tkColon, simple_expr_or_nothing, tkColon, simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 647: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 648: // format_const_expr -> tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 649: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing, tkColon, 
                //                      const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 650: // format_const_expr -> tkColon, const_expr_or_nothing, tkColon, const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 651: // relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 652: // relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 653: // relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 654: // relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 655: // relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 656: // relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 657: // relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 658: // simple_expr -> term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 659: // simple_expr -> simple_expr, addop, term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 660: // addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 661: // addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 662: // addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 663: // addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 664: // addop -> tkCSharpStyleOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 665: // typecast_op -> tkAs
{ 
			CurrentSemanticValue.ob = op_typecast.as_op; 
		}
        break;
      case 666: // typecast_op -> tkIs
{ 
			CurrentSemanticValue.ob = op_typecast.is_op; 
		}
        break;
      case 667: // as_is_expr -> is_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 668: // as_is_expr -> as_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 669: // as_expr -> term, tkAs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 670: // is_expr -> term, tkIs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 671: // is_expr -> term, tkIs, literal_or_number
{
            CurrentSemanticValue.ex = NewIsObjectExpr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 672: // simple_term -> factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 673: // power_expr -> simple_term, tkStarStar, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 674: // term -> factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 675: // term -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 676: // term -> power_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 677: // term -> term, mulop, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 678: // term -> term, mulop, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 679: // term -> as_is_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 680: // mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 681: // mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 682: // mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 683: // mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 684: // mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 685: // mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 686: // mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 687: // default_expr -> tkDefault, tkRoundOpen, simple_or_template_type_reference, 
                //                 tkRoundClose
{ 
			CurrentSemanticValue.ex = new default_operator(ValueStack[ValueStack.Depth-2].td as named_type_reference, CurrentLocationSpan);  
		}
        break;
      case 688: // tuple -> tkRoundOpen, expr_l1, tkComma, expr_l1_list, lambda_type_ref, 
                //          optional_full_lambda_fp_list, tkRoundClose
{
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
      case 689: // factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 690: // factor -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 691: // factor -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 692: // factor -> tkSquareOpen, elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new pascal_set_constant(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 693: // factor -> tkNot, factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 694: // factor -> sign, factor
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
      case 695: // factor -> var_reference
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 696: // factor -> tuple
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 697: // literal_or_number -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 698: // literal_or_number -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 699: // var_question_point -> variable, tkQuestionPoint, variable
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 700: // var_question_point -> variable, tkQuestionPoint, var_question_point
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 701: // var_reference -> var_address, variable
{
			CurrentSemanticValue.ex = NewVarReference(ValueStack[ValueStack.Depth-2].stn as get_address, ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);
		}
        break;
      case 702: // var_reference -> variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 703: // var_reference -> var_question_point
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 704: // var_address -> tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(CurrentLocationSpan);
		}
        break;
      case 705: // var_address -> var_address, tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(ValueStack[ValueStack.Depth-2].stn as get_address, CurrentLocationSpan);
		}
        break;
      case 706: // attribute_variable -> simple_type_identifier, optional_expr_list_with_bracket
{ 
			CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
		}
        break;
      case 707: // attribute_variable -> template_type, optional_expr_list_with_bracket
{
            CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 708: // dotted_identifier -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 709: // dotted_identifier -> dotted_identifier, tkPoint, identifier_or_keyword
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
		}
        break;
      case 710: // variable_as_type -> dotted_identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;}
        break;
      case 711: // variable_as_type -> dotted_identifier, template_type_params
{ CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-2].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);   }
        break;
      case 712: // variable_or_literal_or_number -> variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 713: // variable_or_literal_or_number -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 714: // variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 715: // variable -> operator_name_ident
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 716: // variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 717: // variable -> tkRoundOpen, expr, tkRoundClose
{
		    if (!parsertools.build_tree_for_formatter) 
            {
                ValueStack[ValueStack.Depth-2].ex.source_context = CurrentLocationSpan;
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
            } 
			else CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
        }
        break;
      case 718: // variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 719: // variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 720: // variable -> literal_or_number, tkPoint, identifier_or_keyword
{ 
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 721: // variable -> variable_or_literal_or_number, tkSquareOpen, expr_list, 
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
			else CurrentSemanticValue.ex = new indexer(ValueStack[ValueStack.Depth-4].ex as addressed_value,el, CurrentLocationSpan);
        }
        break;
      case 722: // variable -> variable, tkQuestionSquareOpen, format_expr, tkSquareClose
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
      case 723: // variable -> variable, tkRoundOpen, optional_expr_list, tkRoundClose
{
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 724: // variable -> variable, tkPoint, identifier_keyword_operatorname
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 725: // variable -> tuple, tkPoint, identifier_keyword_operatorname
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 726: // variable -> variable, tkDeref
{
			CurrentSemanticValue.ex = new roof_dereference(ValueStack[ValueStack.Depth-2].ex as addressed_value,CurrentLocationSpan);
        }
        break;
      case 727: // variable -> variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 728: // optional_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 729: // optional_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 730: // elem_list -> elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 731: // elem_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 732: // elem_list1 -> elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 733: // elem_list1 -> elem_list1, tkComma, elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 734: // elem -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 735: // elem -> expr, tkDotDot, expr
{ CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 736: // one_literal -> tkStringLiteral
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 737: // one_literal -> tkAsciiChar
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 738: // literal -> literal_list
{ 
			CurrentSemanticValue.ex = NewLiteral(ValueStack[ValueStack.Depth-1].stn as literal_const_line);
        }
        break;
      case 739: // literal -> tkFormatStringLiteral
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
      case 740: // literal_list -> one_literal
{ 
			CurrentSemanticValue.stn = new literal_const_line(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 741: // literal_list -> literal_list, one_literal
{ 
        	var line = ValueStack[ValueStack.Depth-2].stn as literal_const_line;
            if (line.literals.Last() is string_const && ValueStack[ValueStack.Depth-1].ex is string_const)
            	parsertools.AddErrorFromResource("TWO_STRING_LITERALS_IN_SUCCESSION",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.stn = line.Add(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 742: // operator_name_ident -> tkOperator, overload_operator
{ 
			CurrentSemanticValue.ex = new operator_name_ident((ValueStack[ValueStack.Depth-1].op as op_type_node).text, (ValueStack[ValueStack.Depth-1].op as op_type_node).type, CurrentLocationSpan);
		}
        break;
      case 743: // optional_method_modificators -> tkSemiColon
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 744: // optional_method_modificators -> tkSemiColon, meth_modificators, tkSemiColon
{ 
			//parsertools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 745: // optional_method_modificators1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 746: // optional_method_modificators1 -> tkSemiColon, meth_modificators
{ 
			//parsertools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 747: // meth_modificators -> meth_modificator
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan); 
		}
        break;
      case 748: // meth_modificators -> meth_modificators, tkSemiColon, meth_modificator
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as procedure_attributes_list).Add(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan);  
		}
        break;
      case 749: // identifier -> tkIdentifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 750: // identifier -> property_specifier_directives
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 751: // identifier -> non_reserved
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 752: // identifier_or_keyword -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 753: // identifier_or_keyword -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 754: // identifier_or_keyword -> reserved_keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 755: // identifier_keyword_operatorname -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 756: // identifier_keyword_operatorname -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 757: // identifier_keyword_operatorname -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 758: // meth_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 759: // meth_modificator -> tkOverload
{ 
            CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id;
            parsertools.AddWarningFromResource("OVERLOAD_IS_NOT_USED", ValueStack[ValueStack.Depth-1].id.source_context);
        }
        break;
      case 760: // meth_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 761: // meth_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 762: // meth_modificator -> tkExtensionMethod
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 763: // meth_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 764: // property_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 765: // property_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 766: // property_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 767: // property_specifier_directives -> tkRead
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 768: // property_specifier_directives -> tkWrite
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 769: // non_reserved -> tkName
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 770: // non_reserved -> tkNew
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 771: // visibility_specifier -> tkInternal
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 772: // visibility_specifier -> tkPublic
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 773: // visibility_specifier -> tkProtected
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 774: // visibility_specifier -> tkPrivate
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 775: // keyword -> visibility_specifier
{ 
			CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  
		}
        break;
      case 776: // keyword -> tkSealed
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 777: // keyword -> tkTemplate
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 778: // keyword -> tkOr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 779: // keyword -> tkTypeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 780: // keyword -> tkSizeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 781: // keyword -> tkDefault
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 782: // keyword -> tkWhere
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 783: // keyword -> tkXor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 784: // keyword -> tkAnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 785: // keyword -> tkDiv
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 786: // keyword -> tkMod
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 787: // keyword -> tkShl
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 788: // keyword -> tkShr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 789: // keyword -> tkNot
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 790: // keyword -> tkAs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 791: // keyword -> tkIn
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 792: // keyword -> tkIs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 793: // keyword -> tkArray
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 794: // keyword -> tkSequence
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 795: // keyword -> tkBegin
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 796: // keyword -> tkCase
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 797: // keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 798: // keyword -> tkConst
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 799: // keyword -> tkConstructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 800: // keyword -> tkDestructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 801: // keyword -> tkDownto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 802: // keyword -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 803: // keyword -> tkElse
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 804: // keyword -> tkExcept
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 805: // keyword -> tkFile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 806: // keyword -> tkAuto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 807: // keyword -> tkFinalization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 808: // keyword -> tkFinally
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 809: // keyword -> tkFor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 810: // keyword -> tkForeach
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 811: // keyword -> tkFunction
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 812: // keyword -> tkIf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 813: // keyword -> tkImplementation
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 814: // keyword -> tkInherited
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 815: // keyword -> tkInitialization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 816: // keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 817: // keyword -> tkProcedure
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 818: // keyword -> tkProperty
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 819: // keyword -> tkRaise
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 820: // keyword -> tkRecord
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 821: // keyword -> tkRepeat
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 822: // keyword -> tkSet
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 823: // keyword -> tkTry
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 824: // keyword -> tkType
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 825: // keyword -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 826: // keyword -> tkThen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 827: // keyword -> tkTo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 828: // keyword -> tkUntil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 829: // keyword -> tkUses
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 830: // keyword -> tkVar
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 831: // keyword -> tkWhile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 832: // keyword -> tkWith
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 833: // keyword -> tkNil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 834: // keyword -> tkGoto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 835: // keyword -> tkOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 836: // keyword -> tkLabel
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 837: // keyword -> tkProgram
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 838: // keyword -> tkUnit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 839: // keyword -> tkLibrary
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 840: // keyword -> tkNamespace
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 841: // keyword -> tkExternal
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 842: // keyword -> tkParams
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 843: // keyword -> tkEvent
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 844: // keyword -> tkYield
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 845: // keyword -> tkMatch
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 846: // keyword -> tkWhen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 847: // reserved_keyword -> tkOperator
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 848: // reserved_keyword -> tkEnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 849: // overload_operator -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 850: // overload_operator -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 851: // overload_operator -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 852: // overload_operator -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 853: // overload_operator -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 854: // overload_operator -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 855: // overload_operator -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 856: // overload_operator -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 857: // overload_operator -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 858: // overload_operator -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 859: // overload_operator -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 860: // overload_operator -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 861: // overload_operator -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 862: // overload_operator -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 863: // overload_operator -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 864: // overload_operator -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 865: // overload_operator -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 866: // overload_operator -> tkNot
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 867: // overload_operator -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 868: // overload_operator -> tkImplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 869: // overload_operator -> tkExplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 870: // overload_operator -> assign_operator
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 871: // overload_operator -> tkStarStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 872: // assign_operator -> tkAssign
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 873: // assign_operator -> tkPlusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 874: // assign_operator -> tkMinusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 875: // assign_operator -> tkMultEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 876: // assign_operator -> tkDivEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 877: // func_decl_lambda -> identifier, tkArrow, lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-3].id, LocationStack[LocationStack.Depth-3]); 
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 878: // func_decl_lambda -> tkRoundOpen, tkRoundClose, lambda_type_ref_noproctype, 
                //                     tkArrow, lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 879: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkRoundClose, 
                //                     lambda_type_ref_noproctype, tkArrow, lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-7].id, LocationStack[LocationStack.Depth-7]); 
            var loc = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]);
			var formalPars = new formal_parameters(new typed_parameters(idList, ValueStack[ValueStack.Depth-5].td, parametr_kind.none, null, loc), loc);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 880: // func_decl_lambda -> tkRoundOpen, identifier, tkSemiColon, full_lambda_fp_list, 
                //                     tkRoundClose, lambda_type_ref_noproctype, tkArrow, 
                //                     lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-7].id, LocationStack[LocationStack.Depth-7]);
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null), parametr_kind.none, null, LocationStack[LocationStack.Depth-7]), LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]));
			for (int i = 0; i < (ValueStack[ValueStack.Depth-5].stn as formal_parameters).Count; i++)
				formalPars.Add((ValueStack[ValueStack.Depth-5].stn as formal_parameters).params_list[i]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 881: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkSemiColon, 
                //                     full_lambda_fp_list, tkRoundClose, 
                //                     lambda_type_ref_noproctype, tkArrow, lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-9].id, LocationStack[LocationStack.Depth-9]);
            var loc = LexLocation.MergeAll(LocationStack[LocationStack.Depth-9],LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7]);
			var formalPars = new formal_parameters(new typed_parameters(idList, ValueStack[ValueStack.Depth-7].td, parametr_kind.none, null, loc), LexLocation.MergeAll(LocationStack[LocationStack.Depth-9],LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]));
			for (int i = 0; i < (ValueStack[ValueStack.Depth-5].stn as formal_parameters).Count; i++)
				formalPars.Add((ValueStack[ValueStack.Depth-5].stn as formal_parameters).params_list[i]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 882: // func_decl_lambda -> tkRoundOpen, expr_l1, tkComma, expr_l1_list, 
                //                     lambda_type_ref, optional_full_lambda_fp_list, 
                //                     tkRoundClose, rem_lambda
{ 
			var pair = ValueStack[ValueStack.Depth-1].ob as pair_type_stlist;
			
			if (ValueStack[ValueStack.Depth-4].td is lambda_inferred_type)
			{
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
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formal_pars, pair.tn, pair.exprs, CurrentLocationSpan);
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
					
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, pair.tn, pair.exprs, CurrentLocationSpan);
			}
		}
        break;
      case 883: // func_decl_lambda -> expl_func_decl_lambda
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
		}
        break;
      case 884: // optional_full_lambda_fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 885: // optional_full_lambda_fp_list -> tkSemiColon, full_lambda_fp_list
{
		CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
	}
        break;
      case 886: // rem_lambda -> lambda_type_ref_noproctype, tkArrow, lambda_function_body
{ 
		    CurrentSemanticValue.ob = new pair_type_stlist(ValueStack[ValueStack.Depth-3].td,ValueStack[ValueStack.Depth-1].stn as statement_list);
		}
        break;
      case 887: // expl_func_decl_lambda -> tkFunction, lambda_type_ref, tkArrow, 
                //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 888: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, tkRoundClose, lambda_type_ref, 
                //                          tkArrow, lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 889: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, full_lambda_fp_list, 
                //                          tkRoundClose, lambda_type_ref, tkArrow, 
                //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 890: // expl_func_decl_lambda -> tkProcedure, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 891: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, tkRoundClose, tkArrow, 
                //                          lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 892: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, full_lambda_fp_list, 
                //                          tkRoundClose, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-4].stn as formal_parameters, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 893: // full_lambda_fp_list -> lambda_simple_fp_sect
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
      case 894: // full_lambda_fp_list -> full_lambda_fp_list, tkSemiColon, lambda_simple_fp_sect
{
			CurrentSemanticValue.stn =(ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
		}
        break;
      case 895: // lambda_simple_fp_sect -> ident_list, lambda_type_ref
{
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-2].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan);
		}
        break;
      case 896: // lambda_type_ref -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
		}
        break;
      case 897: // lambda_type_ref -> tkColon, fptype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 898: // lambda_type_ref_noproctype -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
		}
        break;
      case 899: // lambda_type_ref_noproctype -> tkColon, fptype_noproctype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 900: // common_lambda_body -> compound_stmt
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 901: // common_lambda_body -> if_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 902: // common_lambda_body -> while_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 903: // common_lambda_body -> repeat_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 904: // common_lambda_body -> for_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 905: // common_lambda_body -> foreach_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 906: // common_lambda_body -> loop_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 907: // common_lambda_body -> case_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 908: // common_lambda_body -> try_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 909: // common_lambda_body -> lock_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 910: // common_lambda_body -> yield_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 911: // common_lambda_body -> raise_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 912: // lambda_function_body -> expr_l1
{
		    var id = SyntaxVisitors.ExprHasNameVisitor.HasName(ValueStack[ValueStack.Depth-1].ex, "Result"); 
            if (id != null)
            {
                 parsertools.AddErrorFromResource("RESULT_IDENT_NOT_EXPECTED_IN_THIS_CONTEXT", id.source_context);
            }
			var sl = new statement_list(new assign("result",ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan),CurrentLocationSpan); // ���� �������� ��� � assign ��� ������������������� ��� ������ - ����� ��������� ����� Result
			sl.expr_lambda_body = true;
			CurrentSemanticValue.stn = sl;
		}
        break;
      case 913: // lambda_function_body -> common_lambda_body
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 914: // lambda_procedure_body -> proc_call
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 915: // lambda_procedure_body -> assignment
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 916: // lambda_procedure_body -> common_lambda_body
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
